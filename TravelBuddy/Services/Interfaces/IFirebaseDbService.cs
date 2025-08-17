using Firebase.Database.Streaming;
using TravelBuddy.Models;
using TripForm = TravelBuddy.Models.DTOs.TripForm;

namespace TravelBuddy.Services.Interfaces;

public interface IFirebaseDbService
{
    public string LoggedInUserId { get; }
    public Task CreateUser(string fullName);
    public Task<string> UploadUserPhoto(string fileName, Stream fileStream);
    public Task<string> GetPhotoUrl(string fileName);
    public Task<User?> FetchLoggedInUser();
    public Task<User> FetchUser(string userId);
    public Task UpdateUserData(User user);
    public IObservable<FirebaseEvent<User>> ObserveUserChanges();
    public Task CreateTrip(Trip trip);
    public Task UpdateTripData(Trip trip);
    public Task<Trip> FetchTrip(string tripId);
    public Task<IEnumerable<Trip>> FetchTrips(TripForm? tripForm = null);
    public Task CreateChat(Chat chat);
    public Task<Chat> FetchCurrentChat(string chatId);
    public IObservable<FirebaseEvent<Chat>> ObserveChatChanges(string chatId);
    public Task SendMessage(string message, Chat chat);
    public Task DeleteTrip(string tripId);
    public Task RequestToJoinTrip(string tripId, string tripOwnerId, User requestingUser);
    public IObservable<FirebaseEvent<TripRequest>> ObserveTripRequests(string tripId);
    public Task<IEnumerable<TripRequest>?> FetchTripRequests(string tripId);
    public Task UpdateTripRequestData(TripRequest tripRequest);
}