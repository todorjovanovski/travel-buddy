using System.Text.RegularExpressions;
using TravelBuddy.Models;

namespace TravelBuddy.Utils;

public static class Extensions
{
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
    
    public static object ExtractParameters(this User user)
    {
        return new
        {
            user.Id,
            user.Name,
            user.Birthdate,
            user.Bio,
            user.Location,
            user.FavoriteActivities,
            user.ProfilePhotos
        };
    }
    
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
}