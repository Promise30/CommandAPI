using CommandAPI.Dtos;
using CommandAPI.Models;

namespace CommandAPI.Services
{
    public interface ICommandService
    {
        APIResponse<IEnumerable<CommandReadDto>> GetCommands();
        APIResponse<CommandReadDto> GetCommandById(int id);
        APIResponse<CommandReadDto> CreateCommand(CommandCreateDto commandReadDto);
        APIResponse<object> UpdateCommand(int id, CommandUpdateDto command);
        APIResponse<object> DeleteCommand(int id);

    }
}
