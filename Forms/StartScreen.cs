using CheckerZ.Objects;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
namespace CheckerZ
{
    //Class to handle entering the game.
    // User has to log in to the current session on the game website with the desired players.
    // This will generate a code which must be entered to start the game.
    public partial class StartScreen : Form
    {
        Session session = Session.Instance;

        public StartScreen()
        {
            InitializeComponent();
        }

        // Get the current session player group from the server
        private async void StartScreen_Load(object sender, EventArgs e)
        {
            await ApiManager.UpdateReplayDataBase();
        }

        // Starts the current session according to session code
        private async void StartSession_Click(object sender, EventArgs e)
        {
            if (!int.TryParse(CodeText.Text, out int code))
            {
                MessageBox.Show("Input Error!", "Only Numbers!", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            session.Players = await ApiManager.GetPlayers(code);
            if (session.Players != null)
            {
                this.DialogResult = DialogResult.OK;
            }
            else
                MessageBox.Show($"Code {code} not found", "Invalid code", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
        }

    }
}
