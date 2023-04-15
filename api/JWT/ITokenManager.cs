using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.JWT
{
    public interface ITokenManager
    {
        string GenerateToken(User user);
    }
}