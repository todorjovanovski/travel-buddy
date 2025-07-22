using System.ComponentModel;
using System.Reflection;
using System.Text.RegularExpressions;
using TravelBuddy.Models;

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
}