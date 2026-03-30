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
            //Application.Run(new GameEngine());

            // 1. Open the Start Screen as a temporary dialog
            using (StartScreen startScreen = new StartScreen())
            {
                // 2. Wait for it to close. Did they successfully join?
                if (startScreen.ShowDialog() == DialogResult.OK)
                { 

                    // 4. Launch the actual game, passing the ID into the constructor!
                    Application.Run(new GameEngine());
                    startScreen.Close();
                }
                // If they pressed 'X', it skips the if-statement and the app just exits cleanly!
            }
        }
    }
}
