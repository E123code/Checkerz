using CheckerZ.Client_Server;
using CheckerZ.Data.DB;
using CheckerZ.Objects;
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
    //A class to handle client sides connection to the server
    internal class ApiManager
    {
        private const string BASEADDRESS = "https://localhost:7209/";
        private static HttpClient client;

        static ApiManager()
        {
            client = new HttpClient();

            client.BaseAddress = new Uri(BASEADDRESS);
        }

        //sends a request for the server to get all players who logged into the current game session
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

        //Sending each game that has been played to the server. Happens after every game finish
        public static async Task SaveGameToServer(object game)
        {
            try
            {
                HttpResponseMessage msg = await client.PostAsJsonAsync($"api/Server/SaveGame", game);
                msg.EnsureSuccessStatusCode();

            }
            catch (Exception ex)
            {
                MessageBox.Show("Saving to server Failed!!!", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        // Updating the local data base based on actions that were made in server.
        // if a game was deleted in server it will not appear in the client.
        // if a player was deleted,
        // all his games will be deleted from both server and client replay database.
        public static async Task UpdateReplayDataBase()
        {
            HttpResponseMessage msg = await client.GetAsync($"api/Server/SyncGames");
            if (msg.IsSuccessStatusCode)
            {
                List<UpdatedGame> gameList = await msg.Content.ReadAsAsync<List<UpdatedGame>>();
                var dictGames = gameList.ToDictionary(x => $"{x.PlayerID},{x.GameDate:G}", y => y.PlayerName);
                using (ReplayDataDataContext DB = new ReplayDataDataContext())
                {
                    var clientGames = DB.GameTables.ToList();
                    foreach (var game in clientGames)
                    {
                        string key = $"{game.PlayerID},{game.GameDate:G}";
                        if (dictGames.ContainsKey(key))
                        {
                            if (game.PlayerName != dictGames[key])
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
        //Sending a request for the server to choose an action based on the current game state.
        // The server returns the desired action,
        // The client will execute the action
        public static async Task<MoveCommand> GetComputerMove(List<BoardLocation> playerLocations,List<BoardLocation> computerLocations)
        {
            GameStateRequest gameStateRequest = new GameStateRequest {PlayerLocations = playerLocations, ComputerLocations = computerLocations };
            try
            {
                HttpResponseMessage msg = await client.PostAsJsonAsync($"api/Server/ComputerMove", gameStateRequest);
                msg.EnsureSuccessStatusCode();
                MoveCommand command = await msg.Content.ReadAsAsync<MoveCommand>();
                return command;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Getting computer move failed!!!", "Server Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return null;
        }

    }
}
