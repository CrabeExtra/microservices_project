namespace Identity.Application.Service.Interface;

/// <summary>
/// Service for hashing and verifying strings.
/// </summary>
public interface IHashService
{
    /// <summary>
    /// Hash a string using built in PasswordHasher.
    /// </summary>
    /// <param name="password"></param>
    /// <returns></returns>
    string Hash(string password);

    /// <summary>
    /// Verify a string against a hash using built in PasswordHasher.
    /// </summary>
    /// <param name="password"></param>
    /// <param name="hash"></param>
    /// <returns></returns>
    bool Verify(string password, string hash);
}