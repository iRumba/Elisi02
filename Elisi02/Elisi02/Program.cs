using System;
using System.Windows.Forms;

namespace Elisi02
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            var presenter = new Presenter(new MainForm(), new Project());
            presenter.Run();
        }
    }
}
