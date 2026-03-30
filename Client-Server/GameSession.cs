using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckerZ.Objects
{
    public class Session
    {
        private static readonly Session instance = new Session();

        public static Session Instance
        {
            get { return instance; }
        }

        public List<Player> Players { get; set; } = new List<Player>();
    }
}
