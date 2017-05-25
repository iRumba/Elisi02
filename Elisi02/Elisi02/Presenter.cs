using System;
using System.IO;

namespace Elisi02
{
    public class Presenter
    {
        IView _view;
        IProject _project;

        public Presenter(IView view, IProject project)
        {
            _view = view;
            _project = project;
            _view.GetCommandResult = _project.GetCommandResult;
            _view.RaiseLoadCommands += LoadCommands;
        }

        public void Run()
        {
            _view.Run();
        }

        public void LoadCommands()
        {
            var existing = true;
            if (!File.Exists(_view.CommandsFilePath))
            {
                _view.SetCommandsFileError("Такого файла не существует");
                existing = false;
            }

            if (!File.Exists(_view.StatesFilePath))
            {
                _view.SetStatesFileError("Такого файла не существует");
                existing = false;
            }

            if (!existing)
                return;

            _project.SetFiles(_view.CommandsFilePath, _view.StatesFilePath);

            try
            {
                _view.LoadCommands(_project.GetCommands());
            }
            catch (Exception ex)
            {
                _view.RaiseException(ex);
            }
        }
    }
}
