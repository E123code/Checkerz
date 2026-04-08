using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ
{
    // class to get the players from the server and use it in the replay data base
    public class Player
    {
        public int Id { get; set; }

        public string Name { get; set; } = default;

        public string PhoneNumber { get; set; }

        public string Country { get; set; }

    }
}
