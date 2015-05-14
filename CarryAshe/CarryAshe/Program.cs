using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarryAshe
{
    class Program
    {

        internal static Ashe _champ = new Ashe();
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += _champ.OnLoad;
        }
    }
}
