using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.CrossCuttingConcern.Logging.Serilog.Loggers;
using Entity.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public IUserService _userService;
        public IMapper _mapper;
        public ILoggerService _logger;

        public UserController(IUserService userService, IMapper mapper, ILoggerService logger)
        {
            _userService = userService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("me")]
        [ProducesResponseType(typeof(UserDetailsDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Me()
        {
            try
            {
                var result = await _userService.GetMe();

                if (result.Success)
                {
                    return StatusCode(StatusCodes.Status200OK, result.Data);
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
