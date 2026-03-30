using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CheckerZ
{
    internal class ApiManager
    {
        private const string BASEADDRESS = "https://localhost:7209/";
        private static HttpClient client;

        static ApiManager()
        {
            client = new HttpClient();

            client.BaseAddress = new Uri(BASEADDRESS);
        }

        public static async Task<List<Player>> GetPlayers(int code)
        {

            HttpResponseMessage msg = await client.GetAsync($"api/Server/LoginSession/{code}");
            if (msg.IsSuccessStatusCode)
            {
                //var players = await msg.Content.ReadAsAsync<List<Player>>();
                var players = await msg.Content.ReadAsAsync<List<Player>>();
                return players;
            }
            return null;
        }
        public static async Task SaveGameToServer(object game)
        {
            try
            {
                HttpResponseMessage msg = await client.PostAsJsonAsync($"api/Server/SaveGame", game);
                msg.EnsureSuccessStatusCode();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Saving to server Failed!!!","Server Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }
    }
}
