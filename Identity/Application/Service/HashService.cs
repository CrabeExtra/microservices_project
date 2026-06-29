using Identity.Application.Service.Interface;
using Identity.Domain.Entity;
using Microsoft.AspNetCore.Identity;

// Apparently Argon2id is pretty strong, but it would take a few extra minutes and this is a hobby project.
namespace Identity.Application.Service
{
    // decided to not use the userManager here. Unnecessary atm.
    public class HashService(IPasswordHasher<User> hasher) : IHashService
    {

        public string Hash(string str)
        {
            return hasher.HashPassword(null!, str);
        }

        public bool Verify(string str, string hash)
        {
            var result = hasher.VerifyHashedPassword(
                null!,
                hash,
                str
            );

            return result != PasswordVerificationResult.Failed;
        }
    }
}