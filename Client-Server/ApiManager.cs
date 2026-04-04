using CheckerZ.Client_Server;
using CheckerZ.Data.DB;
using System;
using System.Collections.Generic;
using System.Data.Linq;
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

        public static async Task UpdateReplayDataBase()
        {
            HttpResponseMessage msg = await client.GetAsync($"api/Server/SyncGames");
            if (msg.IsSuccessStatusCode)
            {
                List<UpdatedGame> gameList = await msg.Content.ReadAsAsync<List<UpdatedGame>>();
                var dictGames = gameList.ToDictionary(x=> $"{x.PlayerID},{x.GameDate:G}",y=> y.PlayerName);
                using (ReplayDataDataContext DB = new ReplayDataDataContext())
                {
                    var clientGames = DB.GameTables.ToList();
                    foreach (var game in clientGames)
                    {
                        string key = $"{game.PlayerID},{game.GameDate:G}";
                        if (dictGames.ContainsKey(key))
                        {
                            if(game.PlayerName != dictGames[key])
                            {
                                game.PlayerName = dictGames[key];
                            }
                        }
                        else
                        {
                            DB.GameTables.DeleteOnSubmit(game);
                        }
                    }
                    DB.SubmitChanges();
                }

            }
            else
            {
                MessageBox.Show("Error In Synching Game Data");
            }
        }
    }
}
