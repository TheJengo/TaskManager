using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Business.Autofac;
using Core.CrossCuttingConcern.Logging.Serilog.Loggers;
using Core.Entity.Concrete;
using Core.Utilities.Security.Jwt;
using Entity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IAuthService _authService;
        private IMapper _mapper;
        private ILoggerService _logger;

        public AuthController(IAuthService authService, IMapper mapper, ILoggerService logger)
        {
            _authService = authService;
            _mapper = mapper;
            _logger = logger;
        }

        /// <summary>
        /// User logs in with its registration info.
        /// </summary>
        /// <param name="userForLoginDto"></param>
        /// <returns></returns>
        [HttpPost("login")]
        [ProducesResponseType(typeof(AccessToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Login([FromBody] UserForLoginDto userForLoginDto)
        {
            try
            {
                var loginResult = await _authService.UserLogin(userForLoginDto);

                if (loginResult.Success)
                {
                    var result = _authService.CreateAccessToken(_mapper.Map<User>(loginResult.Data));
                    if (result.Success)
                    {
                        _logger.LogInformation("@{userId} logged in.", loginResult.Data.Id);

                        return StatusCode(StatusCodes.Status200OK, result.Data);
                    }

                    return StatusCode(StatusCodes.Status200OK, result.Message);
                }
                else
                {
                    return StatusCode(StatusCodes.Status400BadRequest, loginResult.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// User registers with its unique Email and Password info.
        /// </summary>
        /// <param name="userForRegisterDto"></param>
        /// <returns></returns>
        [HttpPost("register")]
        [ProducesResponseType(typeof(AccessToken), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Register([FromBody] UserForRegisterDto userForRegisterDto)
        {
            try
            {
                var userExists = await _authService.UserExists(userForRegisterDto.Email);

                if (!userExists.Success)
                {
                    return StatusCode(StatusCodes.Status400BadRequest, userExists.Message);
                }

                var registerResult = await _authService.UserRegister(userForRegisterDto, userForRegisterDto.Password);
                var result = _authService.CreateAccessToken(_mapper.Map<User>(registerResult.Data));

                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
                }

                return StatusCode(StatusCodes.Status400BadRequest, userExists.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        /// <summary>
        /// User logout endpoint.
        /// </summary>
        /// <returns></returns>
        [HttpPost("logout")]
        [ProducesResponseType(typeof(string), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Logout()
        {
            try
            {
                var result = await _authService.Logout();

                if (result.Success)
                {
                    _logger.LogInformation("@{userId} logged out.", SecuredClaimer.GetUserId());

                    return StatusCode(StatusCodes.Status200OK);
                }

                return StatusCode(StatusCodes.Status400BadRequest, result.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
