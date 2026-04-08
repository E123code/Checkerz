using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckerZ
{
    internal static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (StartScreen startScreen = new StartScreen())
            {
                if (startScreen.ShowDialog() == DialogResult.OK)
                { 

                    Application.Run(new GameEngine());
                    startScreen.Close();
                }
            }
        }
    }
}
