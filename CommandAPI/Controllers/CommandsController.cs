using AutoMapper;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Repos;
using CommandAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace CommandAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommandsController : ControllerBase
    {
        //private readonly ICommandRepo _repository;
        private readonly ICommandService _commandService;
        //private readonly IMapper _mapper;
        public CommandsController(ICommandService commandService)
        {
            _commandService = commandService;
            //_mapper = mapper;
        }
        [HttpGet]
        public IActionResult GetAllCommands()
        {
            var commands = _commandService.GetCommands();
            if(commands.StatusCode == HttpStatusCode.OK)
                return Ok(commands.Data);
            return StatusCode((int)commands.StatusCode, commands);
        }
        [HttpGet("{id:int}", Name = "GetCommand")]
        public IActionResult GetCommandById(int id)
        {
            var command = _commandService.GetCommandById(id);
            if (command.StatusCode == HttpStatusCode.OK)
                return Ok(command.Data);
            return StatusCode((int)command.StatusCode, command);
        }
        [HttpPost]
        public IActionResult CreateCommand(CommandCreateDto commandCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var commandCreated = _commandService.CreateCommand(commandCreateDto);
            if ((int)commandCreated.StatusCode == 201)
                return CreatedAtAction(nameof(GetCommandById), new { id = commandCreated.Data.Id }, commandCreated.Data);
            return StatusCode((int)commandCreated.StatusCode, commandCreated);
          
        }
        [HttpPut("{id:int}")]
        public IActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(commandUpdateDto);
            }

            var result = _commandService.UpdateCommand(id, commandUpdateDto);
            if ((int)result.StatusCode == 204)
                return NoContent();
            return StatusCode((int)result.StatusCode, result);
        }
        [HttpDelete("{id:int}")]
        public IActionResult DeleteCommand(int id)
        {
            var result = _commandService.DeleteCommand(id);
            if ((int)result.StatusCode == 204)
                return NoContent();
            return StatusCode((int)result.StatusCode, result);
        }
        
    }
}
