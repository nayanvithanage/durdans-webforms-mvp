using System;
using System.Security.Cryptography;
using System.Text;
using Durdans_WebForms_MVP.DAL;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.BLL
{
    public class UserService : IDisposable
    {
        private UserRepository _userRepo;

        public UserService()
        {
            _userRepo = new UserRepository();
        }

        public User ValidateUser(string username, string password)
        {
            var user = _userRepo.GetUserByUsername(username);
            if (user != null)
            {
                if (VerifyPassword(password, user.PasswordHash))
                {
                    return user;
                }
            }
            return null;
        }

        public int RegisterUser(string username, string password, string email, string role)
        {
            if (_userRepo.GetUserByUsername(username) != null)
            {
                throw new ArgumentException("Username already exists.");
            }

            var user = new User
            {
                Username = username,
                PasswordHash = HashPassword(password),
                Email = email,
                Role = role,
                CreatedDate = DateTime.Now
            };

            return _userRepo.InsertUser(user);
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(bytes);
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }

        public void Dispose()
        {
            _userRepo?.Dispose();
        }
    }
}
