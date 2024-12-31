using AutoMapper;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Repos;
using CommandAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        public ActionResult<IEnumerable<Command>> GetAllCommands()
        {
            var commands = _commandService.GetCommands();
            return Ok(commands);
        }
        [HttpGet("{id:int}", Name = "GetCommand")]
        public ActionResult<Command> GetCommandById(int id)
        {
            var command = _commandService.GetCommandById(id);
            if (command == null)
                return NotFound();
            return Ok(command);
        }
        [HttpPost]
        public ActionResult<Command> CreateCommand(CommandCreateDto commandCreateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            _commandService.CreateCommand(commandCreateDto);
            return NoContent();
          
        }
        [HttpPut("{id:int}")]
        public ActionResult UpdateCommand(int id, CommandUpdateDto commandUpdateDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(commandUpdateDto);
            }

            _commandService.UpdateCommand(id, commandUpdateDto);
           
            return NoContent();
        }
        [HttpDelete("{id:int}")]
        public ActionResult DeleteCommand(int id)
        {
            _commandService.DeleteCommand(id);
            return NoContent();
        }
        
    }
}
