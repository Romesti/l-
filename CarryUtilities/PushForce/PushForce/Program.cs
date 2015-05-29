using LeagueSharp.Common;
using LeagueSharp;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CarryLibrary.Extensions;
namespace PushForce
{
    class Program
    {

        private static Menu menu;
        static void Main(string[] args)
        {
            LeagueSharp.Common.CustomEvents.Game.OnGameLoad += GameLoad;
            
        }

        private static void GameLoad(EventArgs args)
        {
 	        CarryLibrary.PushForce.OnPushedLane += OnPushed;
            LeagueSharp.Game.OnUpdate += onGameUpdate;
            Drawing.OnDraw += Drawing_OnDraw;

            menu = new Menu("PushForce", "pushforce", true);
            menu.AddItem(new MenuItem("getCurrentPosition", "getCurrentPosition", false).SetValue(new KeyBind(84, KeyBindType.Press)));
            menu.AddToMainMenu();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            foreach (var lane in FrontLines.Keys)
            {
                Drawing.DrawCircle(FrontLines[lane], 100, System.Drawing.Color.White);
            }
        }

        private static void onGameUpdate(EventArgs args)
        {
            if (menu.Item("getCurrentPosition").GetValue<KeyBind>().Active)
            {
                var minions = MinionManager.GetMinions(1000, MinionTypes.All, MinionTeam.All);
                
                string s = "";
                if (minions.FirstOrDefault(m => m.IsEnemy) != null)
                {
                    s += "enemy " + minions.FirstOrDefault(m => m.IsEnemy).GetLane();
                }
                if (minions.FirstOrDefault(m => m.IsAlly) != null)
                {
                    s += " ally " + minions.FirstOrDefault(m => m.IsAlly).GetLane();
                }
                if (s != "")
                    Notifications.AddNotification(s);
            }

        }

        private static IDictionary<CarryLibrary.Enumerations.Lane, Vector3> FrontLines = new Dictionary<CarryLibrary.Enumerations.Lane,Vector3>();

        private static void OnPushed(CarryLibrary.PushForce.PushedLaneArgs e)
        {
            Notifications.AddNotification(String.Format("Lane {0} is pushed ! factor {1}",e.Lane.ToString(),e.PushingForce),1000);
            FrontLines[e.Lane] = e.Frontline;

        }


    }
}
