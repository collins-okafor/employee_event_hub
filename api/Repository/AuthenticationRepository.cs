using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Models;
using api.Repository.Interface;

namespace api.Repository
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly DataContext _context;

        public AuthenticationRepository(DataContext context)
        {
            _context = context;
        }
        public User? CheckCredentials(User user)
        {
            var userCredentials = _context.Users?.SingleOrDefault(u => u.Email == user.Email);
            return userCredentials;
        }

        public int RegisterUser(User user)
        {
            _context.Users?.Add(user);
            return _context.SaveChanges();
        }
    }
}