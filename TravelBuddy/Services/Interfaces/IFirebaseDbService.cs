using Firebase.Database.Streaming;
using TravelBuddy.Models;

namespace TravelBuddy.Services.Interfaces;

public interface IFirebaseDbService
{
    public Task CreateUser(string fullName);
    public Task<string> UploadUserPhoto(string fileName, Stream fileStream);
    public Task<string> GetPhotoUrl(string fileName);
    public Task<User?> FetchLoggedInUser();
    public Task<User> FetchUser(string userId);
    public Task UpdateUserData(User user);
    public IObservable<FirebaseEvent<User>> ObserveUserChanges();
    public Task CreateTrip(Trip trip);
    public Task<Trip> FetchTrip(Guid tripId);
    public Task<IEnumerable<Trip>> FetchTrips(TripForm? tripForm = null);
}