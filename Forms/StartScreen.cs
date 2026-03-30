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
    public partial class StartScreen : Form
    {
        const string BASEADDRESS = "https://localhost:7209";
        HttpClient client = new HttpClient();
        Session session = Session.Instance;

        public StartScreen()
        {
            InitializeComponent();
        }


        private void StartScreen_Load(object sender, EventArgs e)
        {
            client.BaseAddress = new Uri(BASEADDRESS);
        }

        private async void StartSession_Click(object sender, EventArgs e)
        {
            //string code = CodeText.Text;
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
