using AutoMapper;
using CommandAPI.Dtos;
using CommandAPI.Models;
using CommandAPI.Repos;

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

        public void CreateCommand(CommandCreateDto commandReadDto)
        {
            if (commandReadDto == null)
                throw new ArgumentException("Command not found");
            var commandToCreate = _mapper.Map<Command>(commandReadDto);
            _commandRepository.CreateCommand(commandToCreate);
            _commandRepository.SaveChanges();   
        }
        public void DeleteCommand(int id)
        {
            var command = _commandRepository.GetCommandById(id);
            if (command == null)
                throw new ArgumentException("Command not found");
            _commandRepository.DeleteCommand(command);
            _commandRepository.SaveChanges();
        }

        public CommandReadDto GetCommandById(int id)
        {
            var command = _commandRepository.GetCommandById(id);
            if (command == null)
                throw new ArgumentException("Command not found");
            return _mapper.Map<CommandReadDto>(command);
        }

        public IEnumerable<CommandReadDto> GetCommands()
        {
            var commands = _commandRepository.GetAllCommands();
            return _mapper.Map<IEnumerable<CommandReadDto>>(commands);
        }

        public void UpdateCommand(int id, CommandUpdateDto command)
        {
            var commandModelFromRepo = _commandRepository.GetCommandById(id);
            if (commandModelFromRepo == null)
                throw new ArgumentException("Command not found");
            _mapper.Map(command, commandModelFromRepo);
            _commandRepository.UpdateCommand(commandModelFromRepo);
            _commandRepository.SaveChanges();
        }
    }
}
