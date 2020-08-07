using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.CrossCuttingConcern.Logging.Serilog.Loggers;
using Core.Extensions;
using Entity.Concrete;
using Entity.Dtos;
using Entity.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        public IUserTaskService _userTaskService;
        public IMapper _mapper;
        public ILoggerService _logger;

        public UserTaskController(IUserTaskService userTaskService, IMapper mapper, ILoggerService logger)
        {
            _userTaskService = userTaskService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _userTaskService.GetAllUserTasksForLoggedInUser();

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

        [HttpGet("all")]
        [ProducesResponseType(typeof(List<UserTask>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await _userTaskService.GetAllUserTasks();

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

        [HttpPost("filter/type")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithTypeFilter([FromBody] GetByScheduleTypeDto getByScheduleTypeDto)
        {
            try
            {
                var result = await _userTaskService.GetByScheduleType(getByScheduleTypeDto);

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

        [HttpPost("filter/date")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithDateFilter([FromBody] GetByDateDto getByDateDto)
        {
            try
            {
                var result = await _userTaskService.GetByDate(getByDateDto);

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

        [HttpPost("daily")]
        [ProducesResponseType(typeof(GroupedTaskDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetAsDaily([FromBody] GetByDateDto getByDateDto)
        {
            try
            {
                var result = await _userTaskService.GetAsDaily(getByDateDto);

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

        [HttpPost("weekly")]
        [ProducesResponseType(typeof(GroupedTaskDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetAsWeekly([FromBody] GetByDateDto getByDateDto)
        {
            try
            {
                var result = await _userTaskService.GetAsWeekly(getByDateDto);

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

        [HttpPost("monthly")]
        [ProducesResponseType(typeof(GroupedTaskDto), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetAsMonthly([FromBody] GetByDateDto getByDateDto)
        {
            try
            {
                var result = await _userTaskService.GetAsMonthly(getByDateDto);

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

        [HttpGet("day/{year}/{value}")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithFilterDay(int year, int value)
        {
            return await GetWithFilter(year, value, TaskScheduleType.Daily);
        }

        [HttpGet("day/now")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithFilterDayNow()
        {
            var now = DateTime.UtcNow;

            return await GetWithFilter(now.Year, now.DayOfYear, TaskScheduleType.Daily);
        }

        [HttpGet("week/{year}/{value}")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithFilterWeek(int year, int value)
        {
            return await GetWithFilter(year, value, TaskScheduleType.Weekly);
        }
        
        [HttpGet("week/now")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithFilterWeekNow()
        {
            var now = DateTime.UtcNow;

            return await GetWithFilter(now.Year, now.GetWeekOfTheYear(), TaskScheduleType.Weekly);
        }

        [HttpGet("month/{year}/{value}")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithFilterMonth(int year, int value)
        {
            return await GetWithFilter(year, value, TaskScheduleType.Monthly);
        }

        [HttpGet("month/now")]
        [ProducesResponseType(typeof(List<UserTaskDto>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetWithFilterMonthNow()
        {
            var now = DateTime.UtcNow;

            return await GetWithFilter(now.Year, now.Month, TaskScheduleType.Monthly);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        private async Task<IActionResult> GetWithFilter(int year, int value, Entity.Enums.TaskScheduleType type)
        {
            try
            {
                var getByScheduleTypeDto = new GetByScheduleTypeDto
                {
                    Year = year,
                    Value = value,
                    Type = type
                };

                var result = await _userTaskService.GetByScheduleType(getByScheduleTypeDto);

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

        [HttpPost()]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post([FromBody]params UserTaskDto[] userTaskDtos)
        {
            try
            {
                var userTasks = _mapper.Map<UserTask[]>(userTaskDtos);
                var addResult = await _userTaskService.Add(userTasks);

                if (addResult.Success)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }

                return StatusCode(StatusCodes.Status400BadRequest, addResult.Message);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Put([FromBody] params UserTaskDto[] userTaskDtos)
        {
            try
            {
                var userTasks = _mapper.Map<UserTask[]>(userTaskDtos);
                var updateResult = await _userTaskService.Update(userTasks);

                if (updateResult.Success)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }

                return StatusCode(StatusCodes.Status400BadRequest, updateResult.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }


        [HttpDelete]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Delete([FromBody] params UserTaskDto[] userTaskDtos)
        {
            try
            {
                var userTasks = _mapper.Map<UserTask[]>(userTaskDtos);
                var deleteResult = await _userTaskService.Delete(userTasks);

                if (deleteResult.Success)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }

                return StatusCode(StatusCodes.Status400BadRequest, deleteResult.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}
