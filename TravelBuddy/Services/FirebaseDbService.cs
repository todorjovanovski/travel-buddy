using System.Text.Json;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Firebase.Storage;
using OpenAI.Chat;
using TravelBuddy.Constants;
using TravelBuddy.Models;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using TripForm = TravelBuddy.Models.DTOs.TripForm;
using User = TravelBuddy.Models.User;

namespace TravelBuddy.Services;

public class FirebaseDbService : IFirebaseDbService
{
    private readonly IFirebaseAuthClient _firebaseAuthClient;
    private FirebaseClient FirebaseDatabase => new(FirebaseConstants.DbClient, 
        new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => _firebaseAuthClient.User == null
                ? Task.FromResult(FirebaseConstants.AnonymousUser)
                : _firebaseAuthClient.User.GetIdTokenAsync()
        });
    private FirebaseStorage FirebaseStorage => new(FirebaseConstants.StorageClient,
        new FirebaseStorageOptions
        {
            AuthTokenAsyncFactory = () => _firebaseAuthClient.User == null
                ? Task.FromResult(FirebaseConstants.AnonymousUser)
                : _firebaseAuthClient.User.GetIdTokenAsync()
        });

    public string LoggedInUserId => _firebaseAuthClient.User?.Uid ?? string.Empty;

    public FirebaseDbService(IFirebaseAuthClient firebaseAuthClient)
    {
        _firebaseAuthClient = firebaseAuthClient;
    }

    public async Task CreateUser(string fullName)
    {
        var user = new User
        {
            Id = _firebaseAuthClient.User.Uid,
            Name = fullName
        };
        
        await FirebaseDatabase
            .Child(FirebaseConstants.Users)
            .Child(_firebaseAuthClient.User.Uid)
            .PutAsync( JsonSerializer.Serialize(user));
    }

    public async Task<string> UploadUserPhoto(string fileName, Stream fileStream)
    {
        var extension = Path.GetExtension(fileName).Replace(".", "");
        return await FirebaseStorage
            .Child(FirebaseConstants.Users)
            .Child(_firebaseAuthClient.User.Uid)
            .Child(fileName)
            .PutAsync(fileStream, CancellationToken.None, $"image/{extension}");
    }

    public async Task<string> GetPhotoUrl(string fileName)
    {
        return await FirebaseStorage
            .Child(FirebaseConstants.Users)
            .Child(_firebaseAuthClient.User.Uid)
            .Child(fileName)
            .GetDownloadUrlAsync();
    }

    public async Task<User?> FetchLoggedInUser()
    {
        try
        {
            var userJson = await FirebaseDatabase
                .Child(FirebaseConstants.Users)
                .Child(_firebaseAuthClient.User.Uid)
                .OnceAsJsonAsync();
            return JsonSerializer.Deserialize<User>(userJson);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return null;
        }
    }

    public async Task<User> FetchUser(string userId)
    {
        var userJson = await FirebaseDatabase
            .Child(FirebaseConstants.Users)
            .Child(userId)
            .OnceAsJsonAsync();
        return JsonSerializer.Deserialize<User>(userJson)!;
    }

    public async Task UpdateUserData(User user)
    {
        var serializedUser = JsonSerializer.Serialize(user);
        await FirebaseDatabase
            .Child(FirebaseConstants.Users)
            .Child(user.Id)
            .PutAsync(serializedUser);
    }

    public IObservable<FirebaseEvent<User>> ObserveUserChanges()
    {
        return FirebaseDatabase
            .Child(FirebaseConstants.Users)
            .AsObservable<User>();
    }

    public async Task CreateTrip(Trip trip)
    {
        await FirebaseDatabase
            .Child(FirebaseConstants.Trips)
            .Child(trip.Id.ToString)
            .PutAsync(JsonSerializer.Serialize(trip));
    }

    public async Task UpdateTripData(Trip trip)
    {
        await FirebaseDatabase
            .Child(FirebaseConstants.Trips)
            .Child(trip.Id)
            .PutAsync(JsonSerializer.Serialize(trip));
    }

    public async Task<Trip> FetchTrip(string tripId)
    {
        var trip = await FirebaseDatabase
            .Child(FirebaseConstants.Trips)
            .Child(tripId)
            .OnceAsJsonAsync();
        return JsonSerializer.Deserialize<Trip>(trip)!;
    }

    public async Task<IEnumerable<Trip>> FetchTrips(TripForm? tripForm)
    {
        try
        {
            var tripsJson = await FirebaseDatabase
                .Child(FirebaseConstants.Trips)
                .OnceAsJsonAsync();
            var trips = JsonSerializer.Deserialize<Dictionary<string, Trip>>(tripsJson)?.Values;
            return (tripForm == null ? trips : trips?.Where(t => t.MatchesValuesOf(tripForm)))?
                .Where(t => t.OwnerId != _firebaseAuthClient.User?.Uid) ?? [];
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task CreateChat(Chat chat)
    {
        await FirebaseDatabase
            .Child(FirebaseConstants.Chats)
            .Child(chat.Id)
            .Child(FirebaseConstants.Chat)
            .PutAsync(JsonSerializer.Serialize(chat));
    }

    public async Task CreateTourGuideChat(TourGuideChat chat)
    {
        var assistantMessage = new AssistantChatMessage("Feel free to ask me anything about your trip. I can help you with recommendations, itinerary planning, and more!");
        var tourGuideIntroMessage = new TourGuideMessage
        {
            ChatId = chat.Id,
            ChatMessage = assistantMessage.Content.First().Text,
            SenderId = TourGuideConstants.BotId,
            Time = DateTime.UtcNow
        };
        chat.Messages.Add(tourGuideIntroMessage);
        await FirebaseDatabase
            .Child(FirebaseConstants.TourGuideChats)
            .Child(chat.Id)
            .Child(FirebaseConstants.TourGuideChats)
            .PutAsync(JsonSerializer.Serialize(chat));
    }

    public async Task<Chat> FetchChat(string chatId)
    {
        var chatJson = await FirebaseDatabase
            .Child(FirebaseConstants.Chats)
            .Child(chatId)
            .Child(FirebaseConstants.Chat)
            .OnceAsJsonAsync();
        return JsonSerializer.Deserialize<Chat>(chatJson)!;
    }

    public async Task<TourGuideChat> FetchTourGuideChat(string chatId)
    {
        var chatJson = await FirebaseDatabase
            .Child(FirebaseConstants.TourGuideChats)
            .Child(chatId)
            .Child(FirebaseConstants.TourGuideChats)
            .OnceAsJsonAsync();
        return JsonSerializer.Deserialize<TourGuideChat>(chatJson)!;
    }

    public IObservable<FirebaseEvent<Chat>> ObserveChatChanges(string chatId)
    {
        return FirebaseDatabase
            .Child(FirebaseConstants.Chats)
            .Child(chatId)
            .AsObservable<Chat>();
    }

    public async Task SendMessage(string message, Chat chat, bool isBot)
    {
        var newMessage = new Message
        {
            ChatId = chat.Id,
            Content = message,
            SenderId = isBot ? TourGuideConstants.BotId : _firebaseAuthClient.User.Uid,
            Time = DateTime.UtcNow
        };
        chat.Messages.Add(newMessage);
        chat.LastMessage = newMessage;
        var serializedChat = JsonSerializer.Serialize(chat);
        await FirebaseDatabase
            .Child(FirebaseConstants.Chats)
            .Child(chat.Id)
            .Child(FirebaseConstants.Chat)
            .PatchAsync(serializedChat);
    }

    public async Task InsertTourGuideInteraction(TourGuideChat chat)
    {
        var serializedChat = JsonSerializer.Serialize(chat);
        await FirebaseDatabase
            .Child(FirebaseConstants.TourGuideChats)
            .Child(chat.Id)
            .Child(FirebaseConstants.TourGuideChats)
            .PatchAsync(serializedChat);
    }

    public async Task DeleteTrip(string tripId)
    {
        await FirebaseDatabase
            .Child(FirebaseConstants.Trips)
            .Child(tripId)
            .DeleteAsync();
    }

    public async Task RequestToJoinTrip(string tripId, string tripOwnerId, User requestingUser)
    {
        var tripRequest = new TripRequest
        {
            TripOwnerId = tripOwnerId,
            RequestingUser = requestingUser,
            TripId = tripId
        };
        await FirebaseDatabase
            .Child(FirebaseConstants.TripRequests)
            .Child(tripOwnerId)
            .Child(tripId)
            .Child(tripRequest.Id)
            .PutAsync(JsonSerializer.Serialize(tripRequest));
    }
    
    public IObservable<FirebaseEvent<TripRequest>> ObserveTripRequests(string tripId)
    {
        return FirebaseDatabase
            .Child(FirebaseConstants.TripRequests)
            .Child(_firebaseAuthClient.User.Uid)
            .Child(tripId)
            .AsObservable<TripRequest>();
    }

    public async Task<IEnumerable<TripRequest>?> FetchTripRequests(string tripId)
    {
        var tripRequestsJson = await FirebaseDatabase
            .Child(FirebaseConstants.TripRequests)
            .Child(_firebaseAuthClient.User.Uid)
            .Child(tripId)
            .OnceAsJsonAsync();
        return JsonSerializer.Deserialize<Dictionary<string, TripRequest>>(tripRequestsJson)?.Values;
    }

    public async Task UpdateTripRequestData(TripRequest tripRequest)
    {
        await FirebaseDatabase
            .Child(FirebaseConstants.TripRequests)
            .Child(tripRequest.TripOwnerId)
            .Child(tripRequest.TripId)
            .Child(tripRequest.Id)
            .PatchAsync(JsonSerializer.Serialize(tripRequest));
    }
}