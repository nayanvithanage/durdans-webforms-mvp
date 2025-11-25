using System;
using System.Linq;
using Durdans_WebForms_MVP.Data;
using Durdans_WebForms_MVP.Models;

namespace Durdans_WebForms_MVP.DAL
{
    public class UserRepository : IDisposable
    {
        private ClinicDbContext _context;

        public UserRepository()
        {
            _context = new ClinicDbContext();
        }

        public User GetUserByUsername(string username)
        {
            return _context.Users.FirstOrDefault(u => u.Username == username);
        }

        public int InsertUser(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
            return user.Id;
        }

        public void Dispose()
        {
            _context?.Dispose();
        }
    }
}
