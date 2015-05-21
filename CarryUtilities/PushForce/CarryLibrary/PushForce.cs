using CarryLibrary.Enumerations;
using LeagueSharp;
using LeagueSharp.SDK.Core;
using LeagueSharp.SDK.Core.Managers;
using LeagueSharp.SDK.Core.UI.Notifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarryLibrary
{
    public static class PushForce
    {
        private static readonly List<PushedLaneArgs> LanePushForce = new List<PushedLaneArgs>{
                new PushedLaneArgs{Lane=Lane.Top,PushingForce=0,Frontline=new SharpDX.Vector3(2192f,12608f,52.83f)},
                new PushedLaneArgs{Lane=Lane.Mid,PushingForce=0,Frontline=new SharpDX.Vector3(7524f,7426f,53.43f)},
                new PushedLaneArgs{Lane=Lane.Bottom,PushingForce=0,Frontline=new SharpDX.Vector3(12566f,2390f,51.68f)}
        };
        public  delegate void OnPushedLaneDelegate(PushedLaneArgs e);
        public static event OnPushedLaneDelegate OnPushedLane;

        private  const double MELEE_VALUE = 2;
        private  const double RANGED_VALUE = 4;
        private  const double CHAMP_VALUE = 20;
        private static Notification _not = new Notification("Notif",100,false);

        private static bool IsSubscribed { get { return OnPushedLane != null && OnPushedLane.GetInvocationList().Count() > 0; } }

        static PushForce()
        {
            Game.OnUpdate += PushForce.OnUpdate;
            Notifications.AddNotification(_not);

        }

        static void OnUpdate(EventArgs args)
        {
            if (IsSubscribed)
            {
                var minions = ObjectHandler.GetFast<Obj_AI_Minion>()
                .Where(minion => minion.Team != GameObjectTeam.Neutral
                        && (!minion.IsStunned && !minion.IsRooted && !minion.IsMoving)
                        && minion.IsMinion)
                        //.GroupBy(minion => minion.Name)
                        .Sum(minion => (minion.IsMeele ?MELEE_VALUE:RANGED_VALUE)*minion.HealthPercent);
               /* foreach (var laneArgs in LanePushForce.ToList())
                {
                    double s = 0;

                    if (minions.FirstOrDefault(m => m.IsAlly) == null || minions.FirstOrDefault(m => m.IsEnemy) == null) continue;
                    Console.WriteLine("lel");

                    Notifications.UpdateNotifications();
                    foreach (var minion in minions)
                    {
                        if (minion.IsAlly)
                        {
                            s += MELEE_VALUE * minion.HealthPercent;
                        }
                        else if (minion.IsEnemy)
                        {
                            s -= MELEE_VALUE * minion.HealthPercent;
                        }
                    }

                    var frontLine = (minions.First(m=>m.IsAlly).Position - minions.First(m=>m.IsEnemy).Position);
                    frontLine.Normalize();

                    minions = MinionManager.GetMinions(
                        laneArgs.Frontline,
                        2000,
                        LeagueSharp.SDK.Core.Enumerations.MinionTypes.Ranged,
                        LeagueSharp.SDK.Core.Enumerations.MinionTeam.All,
                        LeagueSharp.SDK.Core.Enumerations.MinionOrderTypes.None);


                    foreach (var minion in minions)
                    {
                        if (minion.IsAlly)
                        {
                            s += RANGED_VALUE * minion.HealthPercent;
                        }
                        else if (minion.IsEnemy)
                        {
                            s -= RANGED_VALUE * minion.HealthPercent;
                        }
                    }

                    var a = LanePushForce.FindIndex(laneA => laneA.Lane == laneArgs.Lane);
                    LanePushForce[a] = new PushedLaneArgs { Lane = laneArgs.Lane,PushingForce=s,Frontline=frontLine };

                    if (LanePushForce[a].PushingForce > 0)
                    {
                        OnPushedLane(laneArgs);
                    }
                }
                */
            }
        }

       

        public class PushedLaneArgs : EventArgs
        {
            public Lane Lane { get; set; }

            public double PushingForce { get; set; }

            public SharpDX.Vector3 Frontline {get;set;}
        }
    }
}
