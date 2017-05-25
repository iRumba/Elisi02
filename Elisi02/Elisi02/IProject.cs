using Elisi02.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Elisi02
{
    public interface IProject
    {
        IEnumerable<Command> GetCommands();
        Task<IEnumerable<Command>> GetCommandsAsync();
        CommandResults GetCommandResult(Command command);
        Task<CommandResults> GetCommandResultAsync(Command command);
        void SetFiles(string commands, string states);
    }
}
