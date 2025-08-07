using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using TravelBuddy.Models;
using TravelBuddy.Services.Interfaces;

namespace TravelBuddy.Utils;

public static class Extensions
{
    /// <summary>
    /// Makes a deep copy of the User
    /// </summary>
    /// <param name="user">User object to be copied</param>
    /// <returns>Deep copy of the user</returns>
    public static User Copy(this User user)
    {
        return new User
        {
            Id = user.Id,
            Name = user.Name,
            Birthdate = user.Birthdate,
            Bio = user.Bio,
            Location = user.Location,
            FavoriteActivities = user.FavoriteActivities,
            ProfilePhotos = user.ProfilePhotos
        };
    }
    
    /// <summary>
    /// Makes a readable value of the enum by adding whitespaces capitalizing characters
    /// </summary>
    /// <param name="value">Enum value</param>
    /// <returns>Readable version of the Enum</returns>
    public static string ToReadableString(this Enum value)
    {
        var input = value.ToString();
        var result = Regex.Replace(input, "(\\B[A-Z])", " $1").Trim();
        return result;
    }
    
    
    public static bool TryGetValueAs<TKey, TVal>(this IDictionary<TKey, object> dictionary, TKey key, out TVal value)
    {
        if (dictionary.TryGetValue(key, out var receivedObject) && receivedObject is TVal typedObject)
        {
            value = typedObject;
            return true;
        }

        value = default!;
        return false;
    }
    
    /// <summary>
    /// Maps the description to its corresponding Enum value
    /// </summary>
    /// <param name="description">Description attribute of the enum value</param>
    /// <typeparam name="T">Enum type</typeparam>
    /// <returns>Enum value for the given description</returns>
    public static T? FromDescription<T>(this string description) where T : struct, Enum
    {
        foreach (var field in typeof(T).GetFields(BindingFlags.Public | BindingFlags.Static))
        {
            var attr = field.GetCustomAttribute<DescriptionAttribute>();
            if (attr != null && attr.Description == description)
                return (T)field.GetValue(null)!;

            // Also match against enum name just in case
            if (field.Name == description)
                return (T)field.GetValue(null)!;
        }

        return null;
    }
    
    /// <summary>
    /// Gets the Description attribute of the Enum value
    /// </summary>
    /// <param name="value">Enum value</param>
    /// <returns>Description attribute value</returns>
    public static string GetDescription(this Enum value)
    {
        var field = value.GetType().GetField(value.ToString());

        var attr = field?.GetCustomAttribute<DescriptionAttribute>();
        return attr?.Description ?? value.ToString();
    }

    /// <summary>
    /// Converts the trips to trip cards
    /// </summary>
    /// <param name="trips">Fetched trips</param>
    /// <param name="firebaseDbService">DBService instance to be used</param>
    /// <returns>TripCard objects from the given trips</returns>
    public static async Task<ObservableCollection<TripCard>> ToTripCards(this IEnumerable<Trip> trips, IFirebaseDbService firebaseDbService)
    {
        var tripCards = new ObservableCollection<TripCard>();
        
        foreach (var trip in trips)
        {
            if (trip.Status is not TripStatus.Active) continue;
            var participants = new ObservableCollection<Participant>();
            
            foreach (var participantId in trip.ParticipantIds)
            {
                var participant = await firebaseDbService.FetchUser(participantId);
                participants.Add(new Participant
                {
                    Name = participant.Name,
                    ProfileImageSource = participant.ProfilePhotos.FirstOrDefault()?.Url
                });
            }
            
            var tripOwner = await firebaseDbService.FetchUser(trip.OwnerId);

            //var photoUrl = await unsplashService.GetRandomPhotoUrlAsync(trip.Destination.ToString());

            var activeTrip = new TripCard
            {
                TripImageSource = "",
                TripOwner = tripOwner,
                Title = trip.Title,
                Destination = trip.Destination.ToString(),
                Date = $"{trip.StartDate:dd.MM.yyyy} -  {trip.EndDate:dd.MM.yyyy}",
                Budget = trip.Budget.GetDescription(),
                AdditionalInfo = trip.AdditionalInfo,
                Participants = participants
            };
            tripCards.Add(activeTrip);
        }

        return tripCards;
    }
    
    /// <summary>
    /// Checks is the trip matches any of the non-empty values of the trip form.
    /// </summary>
    /// <param name="trip">Trip to be compared</param>
    /// <param name="tripForm">TripForm used for the filter</param>
    /// <returns>Boolean indicating whether the filter will match the given trip.</returns>
    public static bool MatchesValuesOf(this Trip trip, TripForm tripForm)
    {
        if (!string.IsNullOrWhiteSpace(tripForm.Title) && tripForm.Title != trip.Title)
            return false;
        if (!string.IsNullOrWhiteSpace(tripForm.Destination) && tripForm.Destination != trip.Destination.ToString())
            return false;
        if (tripForm.StartDate != trip.StartDate)
            return false;
        if (tripForm.EndDate != trip.EndDate)
            return false;
        if (!string.IsNullOrWhiteSpace(tripForm.Budget) && tripForm.Budget != trip.Budget.GetDescription())
            return false;
        if (!string.IsNullOrWhiteSpace(tripForm.GroupSize) && tripForm.GroupSize != trip.GroupSize.GetDescription())
            return false;
        var selectedActivities =
            tripForm.SelectedActivities.Select(obj => Enum.Parse<Activity>(obj.ToString()!)).ToList();
        if (tripForm.SelectedActivities.Count > 0 && selectedActivities.Intersect(trip.Activities).Any())
            return false;
        return true;
    }
}