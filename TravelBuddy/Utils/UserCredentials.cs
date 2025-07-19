namespace TravelBuddy.Utils;

using System;
using System.Linq;
using System.Net.Mail;

public class MissingFieldException() : Exception("All fields are required.");
public class InvalidEmailException(string email) : Exception($"The email '{email}' is not in a valid format.");
public class WeakPasswordException() : Exception("The password must be 8 or more characters and contain at least one digit and special character.");

public static class UserCredentials
{
    /// <summary>
    /// Throws a specific exception if any of the validations fail.
    /// Call this before attempting to register / log in the user.
    /// </summary>
    public static void ValidateCredentials(string fullName, string email, string password)
    {
        if (string.IsNullOrWhiteSpace(fullName) || string.IsNullOrWhiteSpace(email) ||
            string.IsNullOrWhiteSpace(password)) throw new MissingFieldException();

        if (!IsValidEmail(email)) throw new InvalidEmailException(email);

        if (!IsPasswordValid(password)) throw new WeakPasswordException();
    }

    public static bool IsValidEmail(string email)
    {
        try
        {
            var addr = new MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
    }

    /// <returns>
    ///   string.Empty if the password is strong; otherwise a human-readable reason.
    /// </returns>
    public static bool IsPasswordValid(string password)
    {
        return password.Length >= 8 &&
               password.Any(char.IsLetter) &&
               password.Any(char.IsDigit) &&
               password.Any(ch => !char.IsLetterOrDigit(ch));
    }
}
