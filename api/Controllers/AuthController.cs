using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTO.AuthDto;
using api.JWT;
using api.Models;
using api.Repository.Interface;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthenticationRepository _authenticationRepository;
        private readonly ITokenManager _tokenManager;
        private readonly IMapper _mapper;

        public AuthController(IAuthenticationRepository authenticationRepository,ITokenManager tokenManager, IMapper mapper)
        {
            _authenticationRepository = authenticationRepository;
            _tokenManager = tokenManager;
            _mapper = mapper;
        }

        [HttpPost("RegisterUser")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult Create(RegisterDto user)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            user.Password = passwordHash;

            var userModel = _mapper.Map<User>(user);
            var result = _authenticationRepository.RegisterUser(userModel);
            if (result > 0)
            {
                return Ok(new {message = "User registered successfully"});
            }
            return BadRequest();
        }


        [HttpPost("CheckCredentials")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult GetDetails(CheckCredentialsDto user)
        {
            var userModel = _mapper.Map<User>(user);
            var authUser = _authenticationRepository.CheckCredentials(userModel);
            if (authUser == null)
            {
                return NotFound();
            }
            if(authUser != null && !BCrypt.Net.BCrypt.Verify(user.Password, authUser.Password))
            {
                return BadRequest("Incorrect Passwor! Please check your password!");
            }

            var roleName = _authenticationRepository.GetUserRole(authUser.RoleId); 
            var authResponse = new AuthResponse() { IsAuthenticated = true, Role = roleName, Token = _tokenManager.GenerateToken(authUser, roleName)};
            return Ok(authResponse);
        }      
    }
} 