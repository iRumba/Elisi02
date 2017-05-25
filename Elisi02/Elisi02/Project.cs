using System;
using System.Collections.Generic;
using System.Linq;
using Elisi02.Models;
using System.Xml.Serialization;
using System.IO;
using System.Threading.Tasks;

namespace Elisi02
{
    public class Project : IProject
    {
        IEnumerable<PumpState> _states;

        string _commandsFilePath;

        string _statesFilePath;

        IEnumerable<PumpState> States
        {
            get
            {
                return _states ?? (_states = DeserializeXmlCollection<PumpState>("states", _statesFilePath));
            }
        }

        public CommandResults GetCommandResult(Command command)
        {
            var states = States.Where(s => s.TS >= command.TS && s.TS <= command.TS.AddSeconds(5)).ToList();
            
            if (states.Count > 1)
                throw new InvalidOperationException("Не получилось определить, какая реакция соответствовала команде");

            // Результат выполнения команды не был записан в файл
            if (states.Count==0)
                return CommandResults.None;

            var state = states[0];

            // Агрегат включился
            if (state.StatusIsOn)
                return CommandResults.SwitchedOn;

            // Агрегат выключился
            if (command.Value == CommandType.Off)
                return CommandResults.SwitchedOff;

            // Агрегат не включился по причине низкого давления на входе
            if (!state.PressureIsAvailable)
                return CommandResults.LowPressure;

            // Агрегат не включился по причине отсутствия напряжения
            if (!state.VoltageIsAvailable)
                return CommandResults.LowVoltage;

            // Результат выполнения команды не определен
            return CommandResults.Unknown;
        }

        public IEnumerable<Command> GetCommands()
        {
            return DeserializeXmlCollection<Command>("commands", _commandsFilePath);
        }

        public void SetFiles(string commands, string states)
        {
            _commandsFilePath = commands;
            _statesFilePath = states;
        }

        public async Task<IEnumerable<Command>> GetCommandsAsync()
        {
            return await Task.Run(() => GetCommands());
        }

        IEnumerable<T> DeserializeXmlCollection<T>(string collectionName, string filePath)
        {
            IEnumerable<T> res;

            var xml = new XmlSerializer(typeof(T[]), new XmlRootAttribute(collectionName));
            using (var stream = new FileStream(filePath, FileMode.Open))
            {
                res = (IEnumerable<T>)xml.Deserialize(stream);
            }
            return res;
        }

        public async Task<CommandResults> GetCommandResultAsync(Command command)
        {
            return await Task.Run(() => GetCommandResult(command));
        }
    }
}
