using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;

namespace AutoUltLeveler
{
    internal class AutoUltLeveler
    {

        public static String ScriptVersion { get { return typeof(AutoUltLeveler).Assembly.GetName().Version.ToString(); } }
        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        private static Menu _menu;
        public static void OnLoad(EventArgs args)
        {

            AutoLevel.UpdateSequence(new List<SpellSlot>() { SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.R, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.R, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.Unknown, SpellSlot.R });
            _menu = new Menu("AutoUltLeveling", "menu", true);
            _menu.AddItem(new MenuItem("autoultlevel_activate", "Auto Ult Leveling").SetValue(true));
            _menu.AddItem(new MenuItem("42424242", "Credits - Romesti"));
            _menu.Item("autoultlevel_activate").ValueChanged += OnChange;
            AutoLevel.Enable();
            _menu.AddToMainMenu();
        }

        private static void OnChange(object sender, OnValueChangeEventArgs e)
        {
            if (e.GetNewValue<bool>())
                AutoLevel.Enable();
            else
                AutoLevel.Disable();
        }


    }
}
