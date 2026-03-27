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
        const string BASEADDRESS = "https://localhost:7209/";
        HttpClient client = new HttpClient();
        public StartScreen()
        {
            InitializeComponent();
        }

        private async Task button1_Click(object sender, EventArgs e)
        {
            //string code = CodeText.Text;
            if (!int.TryParse(CodeText.Text, out int code))
            {
                MessageBox.Show("Input Error!","Only Numbers!");
                return;
            }
            HttpResponseMessage msg = await client.GetAsync($"api/ServerController/LoginSession/{code}");
            if (msg.IsSuccessStatusCode)
            {
                //var players = await msg.Content.ReadAsAsync<List<Player>>();
                var players = await msg.Content.ReadAsAsync<List<Player>>();
               
            }
        }
        private void StartScreen_Load(object sender, EventArgs e)
        {
            client.BaseAddress = new Uri(BASEADDRESS);
        }
    }
}
