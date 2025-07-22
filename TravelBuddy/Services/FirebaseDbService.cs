using System.Reactive.Linq;
using System.Text.Json;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Database.Query;
using Firebase.Database.Streaming;
using Firebase.Storage;
using TravelBuddy.Constants;
using TravelBuddy.Models;
using TravelBuddy.Services.Interfaces;
using TravelBuddy.Utils;
using User = TravelBuddy.Models.User;

namespace TravelBuddy.Services;

public class FirebaseDbService : IFirebaseDbService
{
    private readonly IFirebaseAuthClient _firebaseAuthClient;
    private FirebaseClient FirebaseDatabase => new(FirebaseConstants.DbClient, 
        new FirebaseOptions
        {
            AuthTokenAsyncFactory = () => _firebaseAuthClient.User.GetIdTokenAsync()
        });
    private FirebaseStorage FirebaseStorage => new(FirebaseConstants.StorageClient,
        new FirebaseStorageOptions
        {
            AuthTokenAsyncFactory = () => _firebaseAuthClient.User.GetIdTokenAsync()
        });

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
            .PatchAsync( JsonSerializer.Serialize(user));
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

    public async Task UpdateUserData(User user)
    {
        var serializedUser = JsonSerializer.Serialize(user);
        await FirebaseDatabase
            .Child(FirebaseConstants.Users)
            .Child(_firebaseAuthClient.User.Uid)
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

    public async Task<Trip> FetchTrip(Guid tripId)
    {
        var trip = await FirebaseDatabase
            .Child(FirebaseConstants.Trips)
            .Child(tripId.ToString)
            .OnceAsJsonAsync();
        return JsonSerializer.Deserialize<Trip>(trip)!;
    }
}