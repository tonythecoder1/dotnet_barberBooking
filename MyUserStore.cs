using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using MySqlConnector;
using Dapper;
using dotidentity.Models;

namespace dotidentity
{
    public class MyUserStore : IUserStore<MyUser>, IUserPasswordStore<MyUser>
    {
        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
{
    using (var dbConnection = GetDbConnection())
    {
        await dbConnection.ExecuteAsync(
    "INSERT INTO Users (Id, UserName, NormalizedUserName, Email, NormalizedEmail, PasswordHash) " +
    "VALUES (@id, @username, @normalizedusername, @email, @normalizedemail, @passwordhash)",
    new {
        id = user.Id ?? Guid.NewGuid().ToString(), // 🔥 Garante que sempre há um Id
        username = user.UserName,
        normalizedusername = user.NormalizedUserName,
        email = user.Email,
        normalizedemail = user.NormalizedEmail,
        passwordhash = user.PasswordHash
    });


        Console.WriteLine("✅ Inserção bem-sucedida na base de dados.");
    }
    return IdentityResult.Success;
}

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var dbConnection = GetDbConnection())
            {
                await dbConnection.ExecuteAsync(
                    "DELETE FROM Users WHERE UserName = @username",
                    new { username = user.UserName }
                );

                Console.WriteLine("✅ Usuário removido com sucesso.");
            }

            return IdentityResult.Success;
        }

        public void Dispose()
        {
            // Dapper não precisa de gerenciamento adicional de recursos
        }

        public static DbConnection GetDbConnection()
        {
            var connectionString = "Server=127.0.0.1;Port=3306;Database=dotnet_db;User ID=root;Password=12345678;";
            var dbConnection = new MySqlConnection(connectionString);
            dbConnection.Open();
            return dbConnection;
        }

        public async Task<MyUser?> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var dbConnection = GetDbConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<MyUser>(
                    "SELECT * FROM Users WHERE Id = @id",
                    new { id = userId }
                );
            }
        }

        public async Task<MyUser?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var dbConnection = GetDbConnection())
            {
                return await dbConnection.QueryFirstOrDefaultAsync<MyUser>(
                    "SELECT * FROM Users WHERE NormalizedUserName = @normalizedUserName",
                    new { normalizedUserName = normalizedUserName }
                );
            }
        }

        public Task<string?> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id ?? string.Empty);
        }

        public Task<string?> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string? normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string? userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var dbConnection = GetDbConnection())
            {
                await dbConnection.ExecuteAsync(
                    "UPDATE Users " +
                    "SET UserName = @username, " +
                    "NormalizedUserName = @normalizedusername, " +
                    "PasswordHash = @passwordhash " +
                    "WHERE Id = @id",
                    new {
                        id = user.Id,
                        username = user.UserName,
                        normalizedusername = user.NormalizedUserName,
                        passwordhash = user.PasswordHash
                    });

                Console.WriteLine("✅ Atualização bem-sucedida.");
            }

            return IdentityResult.Success;
        }

        public Task SetPasswordHashAsync(MyUser user, string? passwordHash, CancellationToken cancellationToken)
        {
            user.PasswordHash = passwordHash;
            return Task.CompletedTask;
        }

        public Task<string?> GetPasswordHashAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.PasswordHash);
        }

        public Task<bool> HasPasswordAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(!string.IsNullOrEmpty(user.PasswordHash));
        }
    }
}
