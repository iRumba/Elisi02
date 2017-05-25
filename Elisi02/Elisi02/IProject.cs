using Elisi02.Models;
using System.Collections.Generic;

namespace Elisi02
{
    public interface IProject
    {
        IEnumerable<Command> GetCommands();
        CommandResults GetCommandResult(Command command);
        void SetFiles(string commands, string states);
    }
}
