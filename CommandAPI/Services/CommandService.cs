using AutoMapper;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Repos;
using System.Net;

namespace CommandAPI.Services
{
    public class CommandService : ICommandService
    {
        private readonly ICommandRepo _commandRepository;
        private readonly IMapper _mapper;

        public CommandService(ICommandRepo commandRepository, IMapper mapper)
        {
            _commandRepository = commandRepository;
            _mapper = mapper;
        }

        public APIResponse<CommandReadDto> CreateCommand(CommandCreateDto commandReadDto)
        {
            if (commandReadDto == null)
                return APIResponse<CommandReadDto>.Create(HttpStatusCode.BadRequest, "Command not found", null);
            var commandToCreate = _mapper.Map<Command>(commandReadDto);
            _commandRepository.CreateCommand(commandToCreate);
            _commandRepository.SaveChanges();  
            var commandToReturn = _mapper.Map<CommandReadDto>(commandToCreate);
            return APIResponse<CommandReadDto>.Create(HttpStatusCode.Created, "Command created successfully", commandToReturn);
        }
        public APIResponse<object> DeleteCommand(int id)
        {
            var command = _commandRepository.GetCommandById(id);
            if (command == null)
                return APIResponse<object>.Create(HttpStatusCode.BadRequest, "Command not found", null);
            _commandRepository.DeleteCommand(command);
            _commandRepository.SaveChanges();
            return APIResponse<object>.Create(HttpStatusCode.NoContent, null, null);
        }

        public APIResponse<CommandReadDto> GetCommandById(int id)
        {
            var command = _commandRepository.GetCommandById(id);
            if (command == null)
                return APIResponse<CommandReadDto>.Create(HttpStatusCode.BadRequest, "Command not found", null);
            var commandToReturn = _mapper.Map<CommandReadDto>(command);
            return APIResponse<CommandReadDto>.Create(HttpStatusCode.OK, "Request successful", commandToReturn);
        }

        public APIResponse<IEnumerable<CommandReadDto>> GetCommands()
        {
            var commands = _commandRepository.GetAllCommands();
            var commandsToReturn = _mapper.Map<IEnumerable<CommandReadDto>>(commands);
            return APIResponse<IEnumerable<CommandReadDto>>.Create(HttpStatusCode.OK, "Request successful", commandsToReturn);
        }

        public APIResponse<object> UpdateCommand(int id, CommandUpdateDto command)
        {
            var commandModelFromRepo = _commandRepository.GetCommandById(id);
            if (commandModelFromRepo == null)
                return APIResponse<object>.Create(HttpStatusCode.BadRequest, "Command not found", null);
            _mapper.Map(command, commandModelFromRepo);
            _commandRepository.UpdateCommand(commandModelFromRepo);
            _commandRepository.SaveChanges();
            return APIResponse<object>.Create(HttpStatusCode.NoContent, null, null);
        }
    }
}
