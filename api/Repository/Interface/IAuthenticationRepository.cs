using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.Repository.Interface
{
    public interface IAuthenticationRepository
    {
        int RegisterUser(User user);
        User? CheckCredentials(User user);
        string GetUserRole(int roleId);
    }
}