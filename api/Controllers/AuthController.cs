using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.JWT;
using api.Models;
using api.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ITokenManager _tokenManager;

        public AuthController(IAuthenticationRepository authenticationRepository,ITokenManager tokenManager)
        {
            _authenticationRepository = authenticationRepository;
            _tokenManager = tokenManager;
        }

        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create(User user)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;

            var result = _authenticationRepository.RegisterUser(user);
            if (result > 0)
            {
                return Ok();
            }
            return BadRequest();
        }


        [HttpPost("CheckCredentials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetDetails(User user)
        {
            var authUser = _authenticationRepository.CheckCredentials(user);
            if (authUser == null)
            {
                return NotFound();
            }
            if(authUser != null && !BCrypt.Net.BCrypt.Verify(user.Password, authUser.Password))
            {
                return BadRequest("Incorrect Passwor! Please check your password!");
            }

            var authResponse = new AuthResponse() { IsAuthenticated = true, Role = "Dummy Role", Token = _tokenManager.GenerateToken(authUser)};
            return Ok(authResponse);
        }      
    }
} 