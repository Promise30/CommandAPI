using CommandAPI.Dtos;
using CommandAPI.Models;

namespace CommandAPI.Services
{
    public interface ICommandService
    {
        IEnumerable<CommandReadDto> GetCommands();
        CommandReadDto GetCommandById(int id);
        void CreateCommand(CommandCreateDto commandReadDto);
        void UpdateCommand(int id, CommandUpdateDto command);
        void DeleteCommand(int id);

    }
}
