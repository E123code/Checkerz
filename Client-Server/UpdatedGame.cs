using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ.Client_Server
{
    // object to synchronize each game existing in server database with clients data base
    internal class UpdatedGame
    {
        public int PlayerID {  get; set; }
        public DateTime GameDate {  get; set; }
        public string PlayerName {  get; set; }
    }
}
