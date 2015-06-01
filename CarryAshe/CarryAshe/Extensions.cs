using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace CarryAshe
{
    internal static class Extensions
    {
        private static string Namespace = null;
        public static String GetNamespace(this Object o)
        {
            return o.GetType().Namespace;
        }

        public static String GetCurrentFunctionName()
        {
            return System.Reflection.MethodBase.GetCurrentMethod().Name;
        }

        public static MenuItem GetItemEndKey(this Menu menu, string key,[CallerMemberNameAttribute] string parentKey = null){
            if (Namespace == null)
            {
                MethodBase current = System.Reflection.MethodBase.GetCurrentMethod();
                Namespace = current.DeclaringType.Namespace;
            }
            return menu.Item(String.Format("{0}.{1}.{2}", Namespace, parentKey, key));
        }

        public static MenuItem AddItem<T>(this Menu m,string key,string displayName,T value){
            var me = new MenuItem(String.Format("{0}.{1}", m.Name, key), displayName).SetValue(value);
            m.AddItem(me);
            return me;
        }

        public static HitChance GetHitchance(this MenuItem m)
        {
            try
            {
                switch (m.GetValue<StringList>().SelectedIndex)
                {
                    case 0:
                        return HitChance.Low;
                    case 1:
                        return HitChance.Medium;
                    case 2:
                        return HitChance.High;
                    case 3:
                        return HitChance.VeryHigh;
                    default:
                        return HitChance.Medium;
                }
            }
            catch
            {
                return HitChance.Immobile;
            }
        }


 


    }
}
