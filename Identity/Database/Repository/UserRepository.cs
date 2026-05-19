using Identity.Database.Repository.Interface;
using Microsoft.Data.Sqlite;

namespace Identity.Database.Repository;

public class UserRepository(string connectionString) : IUserRepository
{
    public int CreateUser(string username, string email, string passwordHash)
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        var command = connection.CreateCommand();
        command.CommandText =
        @"
            INSERT INTO Users (Username, Email, PasswordHash, CreatedAt)
            VALUES ($username, $email, $passwordHash, $createdAt);
            SELECT last_insert_rowid();
        ";

        command.Parameters.AddWithValue("$username", username);
        command.Parameters.AddWithValue("$email", email);
        command.Parameters.AddWithValue("$passwordHash", passwordHash);
        command.Parameters.AddWithValue("$createdAt", DateTime.UtcNow.ToString("o"));

        var result = command.ExecuteScalar();

        return Convert.ToInt32(result);
    }
}