using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Core.CrossCuttingConcern.Logging.Serilog.Loggers;
using Entity.Concrete;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [Route("api/tasks/types")]
    [ApiController]
    public class TaskTypeController : ControllerBase
    {
        public ITaskTypeService _taskTypeService;
        public IMapper _mapper;
        public ILoggerService _logger;

        public TaskTypeController(ITaskTypeService taskTypeService, IMapper mapper, ILoggerService logger)
        {
            _taskTypeService = taskTypeService;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(typeof(List<TaskType>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _taskTypeService.GetAllTaskTypes();

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

        [HttpPost]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Post([FromBody] params TaskType[] taskTypes)
        {
            try
            {
                var addResult = await _taskTypeService.Add(taskTypes);

                if (addResult.Success)
                {
                    return StatusCode(StatusCodes.Status200OK);
                }

                return StatusCode(StatusCodes.Status400BadRequest, addResult.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);

                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut]
        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> Put([FromBody] params TaskType[] taskTypes)
        {
            try
            {
                var updateResult = await _taskTypeService.Update(taskTypes);

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
        public async Task<IActionResult> Delete([FromBody] params TaskType[] taskTypes)
        {
            try
            {
                var deleteResult = await _taskTypeService.Delete(taskTypes);

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
