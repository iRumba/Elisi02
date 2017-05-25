using Elisi02.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elisi02
{
    public partial class MainForm : Form, IView, INotifyPropertyChanged
    {
        string _commandsFilePath;
        string _statesFilePath;

        public MainForm()
        {
            InitializeComponent();
            txtCommands.DataBindings.Add(new Binding("Text", this, nameof(CommandsFilePath)));
            txtStates.DataBindings.Add(new Binding("Text", this, nameof(StatesFilePath)));
        }

        public event Action RaiseLoadCommands;
        public event PropertyChangedEventHandler PropertyChanged;

        public void Run()
        {
            Application.Run(this);
        }

        public Func<Command, CommandResults> GetCommandResult { get; set; }

        public string CommandsFilePath
        {
            get
            {
                return _commandsFilePath;
            }
            set
            {
                if (_commandsFilePath != value)
                {
                    _commandsFilePath = value;
                    OnPropertyChanged(nameof(CommandsFilePath));
                }
            }
        }

        public string StatesFilePath
        {
            get
            {
                return _statesFilePath;
            }
            set
            {
                if (_statesFilePath != value)
                {
                    _statesFilePath = value;
                    OnPropertyChanged(nameof(StatesFilePath));
                }
            }
        }

        public async Task LoadCommandsAsync(IEnumerable<Command> commands)
        {
            lstCommands.Items.Clear();
            foreach (var command in commands)
            {
                var val = command.Value.ToString();
                string result = string.Empty;
                await Task.Run(() => 
                {
                    try
                    {
                        switch (GetCommandResult?.Invoke(command))
                        {
                            case CommandResults.SwitchedOn:
                                result = "Включен";
                                break;
                            case CommandResults.SwitchedOff:
                                result = "Выключен";
                                break;
                            case CommandResults.LowPressure:
                                result = "Не включен (низкое давление)";
                                break;
                            case CommandResults.LowVoltage:
                                result = "Не включен (отсутствует напряжение)";
                                break;
                            case CommandResults.None:
                                result = "Результат не был записан в файл";
                                break;
                            case CommandResults.Unknown:
                                result = "Результат выполнения не ясен";
                                break;
                            default:
                                result = "Ошибка: Представление не настроено";
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        result = $"Ошибка: {ex.Message}";
                    }
                });
                

                lstCommands.Items.Add(new ListViewItem(new string[] { command.TimeStamp, val, result }));
            }

        }

        public void SetCommandsFileError(string msg)
        {
            errorProvider1.SetError(txtCommands, msg);
        }

        public void SetStatesFileError(string msg)
        {
            errorProvider1.SetError(txtStates, msg);
        }

        public void RaiseException(Exception ex)
        {
            var currentEx = ex;
            var sb = new StringBuilder();
            while (currentEx != null)
            {
                sb.AppendLine(currentEx.Message);
                currentEx = currentEx.InnerException;
            }

            MessageBox.Show(sb.ToString(), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        void txtCommands_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtCommands, null);
        }

        void txtStates_TextChanged(object sender, EventArgs e)
        {
            errorProvider1.SetError(txtStates, null);
        }

        void button2_Click(object sender, EventArgs e)
        {
            dlgXml.Title = "Выбор файла с историей команд";
            dlgXml.FileName = CommandsFilePath;
            if (dlgXml.ShowDialog() == DialogResult.OK)
            {
                CommandsFilePath = dlgXml.FileName;
            }
        }

        void button3_Click(object sender, EventArgs e)
        {
            dlgXml.Title = "Выбор файла с историей состояний";
            dlgXml.FileName = StatesFilePath;
            if (dlgXml.ShowDialog() == DialogResult.OK)
            {
                StatesFilePath = dlgXml.FileName;
            }
        }

        void button1_Click(object sender, EventArgs e)
        {
            RaiseLoadCommands?.Invoke();
        }

        void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
