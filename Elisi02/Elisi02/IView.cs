﻿using Elisi02.Models;
using System;
using System.Collections.Generic;

namespace Elisi02
{
    public interface IView
    {
        event Action RaiseLoadCommands;
        string CommandsFilePath { get; }
        string StatesFilePath { get; }
        void Run();
        void LoadCommands(IEnumerable<Command> commands);
        void SetCommandsFileError(string msg);
        void SetStatesFileError(string msg);
        void RaiseException(Exception ex);
        Func<Command, CommandResults> GetCommandResult { get; set; }
    }
}
