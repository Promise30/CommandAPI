using CommandAPI.Data;
using CommandAPI.Models;

namespace CommandAPI.Repos
{
    public class CommandRepo : ICommandRepo
    {
        private readonly CommandDbContext _context;
        public CommandRepo(CommandDbContext context)
        {
            _context = context;
        }
        public void CreateCommand(Command cmd)
        {
            _context.Commands.Add(cmd);
        }

        public void DeleteCommand(Command cmd)
        {
            _context.Commands.Remove(cmd);
        }

        public IEnumerable<Command> GetAllCommands()
        {
            return _context.Commands.ToList();
        }

        public Command GetCommandById(int id)
        {
            return _context.Commands.FirstOrDefault(c=> c.Id == id);    
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }

        public void UpdateCommand(Command cmd)
        {
            _context.Commands.Update(cmd);
        }
    }
}
