using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace CarryAshe
{
    internal class AsheMenu
    {
        #region Attributes
        private Ashe _parentAssembly;
        #endregion

        #region Properties
        public HitChance ComboHitChance
        {
            get { return Menu.GetItemEndKey("HitChance", "Combo").GetHitchance(); }
        }


        #endregion
        public Menu Menu { get; private set; }


        public AsheMenu(Ashe parentAssembly)
        {
            this._parentAssembly = parentAssembly;
            Menu = new Menu("CarryAshe", "menu", true);
        }
        public void Initialize()
        {
            int i = 0;
            Console.WriteLine(i++);

            Menu = new Menu("CarryAshe", "CarryAsheMenu", true);
            Console.WriteLine(i++);
            var orbwalkerMenu = new Menu("Orbwalker", "orbwalker");
            Console.WriteLine(i++);
            _parentAssembly.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Console.WriteLine(i++);

            Menu.AddSubMenu(orbwalkerMenu);
            Console.WriteLine(i++);

            var targetSelector = new Menu("Target Selector", "TargetSelector");
            TargetSelector.AddToMenu(targetSelector);

            Menu.AddSubMenu(targetSelector);
            Console.WriteLine(i++);

            var drawingMenu = new Menu("Drawings", _parentAssembly.GetNamespace() + ".Drawings");
            drawingMenu.AddItem("Off", "Activate Drawings", true);
            drawingMenu.AddItem("W", "Draw W Range", new Circle());
            drawingMenu.AddItem("AutoR", "Draw Auto R Range", new Circle());
            drawingMenu.AddItem("FillColor", "Fill color",new Circle(true, Color.FromArgb(204, 204, 0, 0)));
            Menu.AddSubMenu(drawingMenu);
            Console.WriteLine(i++);

            var comboMenu = new Menu("Combo", _parentAssembly.GetNamespace() + ".Combo");
            comboMenu.AddItem("HitChance", "Hitchance", new StringList(new[] { "Low", "Medium", "High", "Very High" }, 3));
            comboMenu.AddItem("UseQ", "Use Q", true);
            comboMenu.AddItem("UseW", "Use W", true);
            comboMenu.AddItem("UseWMana", "Minimum Mana to Use W", new Slider(10));
            comboMenu.AddItem("UseR", "Use R", true);
            comboMenu.AddItem("SaveR", "Save Mana for R", true);

            Menu.AddSubMenu(comboMenu);
            Console.WriteLine(i++);

            var harassMenu = new Menu("Harrass", _parentAssembly.GetNamespace() + ".Harass");
            harassMenu.AddItem("UseQ", "Use Q", true);
            harassMenu.AddItem("UseW", "Use W", true);
            harassMenu.AddItem("ManaThreshold", "Minimum mana for harass", new Slider(55));

            Menu.AddSubMenu(harassMenu);
            Console.WriteLine(i++);

            var clearMenu = new Menu("Lane & Jungler Clear", _parentAssembly.GetNamespace() + ".Clear");
            clearMenu.AddItem("UseQ", "Use Q", true);
            clearMenu.AddItem("UseW", "Use W", true);

            Menu.AddSubMenu(clearMenu);
            Console.WriteLine(i++);

            var itemMenu = new Menu("Items", _parentAssembly.GetNamespace() + ".Items");
            itemMenu.AddItem("Youmuu", "Use Youmuu's Ghostblade", true);
            itemMenu.AddItem("Cutlass", "Use Cutlass", true);
            itemMenu.AddItem("Blade", "Use Blade of the Ruined King", true);
            itemMenu.AddItem(new MenuItem(_parentAssembly.GetNamespace() + ".Items.azeazzaeae", ""));
            itemMenu.AddItem("BladeEnemyEHP", "Enemy HP Percentage", new Slider(80, 100, 0));
            itemMenu.AddItem("BladeEnemyMHP", "My HP Percentage", new Slider(80, 100, 0));

            Menu.AddSubMenu(itemMenu);
            Console.WriteLine(i++);

            var miscMenu = new Menu("Misc", _parentAssembly.GetNamespace() + ".Misc");
            var miscAutoRMenu = new Menu("Auto R", _parentAssembly.GetNamespace() + ".Misc.AutoR");
            miscAutoRMenu.AddItem("Toggle", "Auto R when in range", new KeyBind(84, KeyBindType.Press)); // T Key
            miscAutoRMenu.AddItem("Range", "Auto R Range", new Slider(2000, Convert.ToInt32(_parentAssembly.Player.AttackRange), 4000));
            miscAutoRMenu.AddItem("Hitchance", "Auto R Hitchance", new StringList(new[] { "Low", "Medium", "High", "Very High" }, 3));
            miscMenu.AddSubMenu(miscAutoRMenu);

            Menu.AddSubMenu(miscMenu);

            var credits = new Menu("Credits", "Romesti");
            credits.AddItem(new MenuItem(_parentAssembly.GetNamespace() + ".Paypal", "You can make a donation via paypal :)"));
            Menu.AddSubMenu(credits);
            Console.WriteLine(i++);

            Menu.AddItem(new MenuItem("422442fsaafs4242f", ""));
            Menu.AddItem(new MenuItem("422442fsaafsf", "Version " + _parentAssembly.ScriptVersion));
            Menu.AddItem(new MenuItem("fsasfafsfsafsa", "Made By Romesti"));

            Menu.AddToMainMenu();
            Console.WriteLine(i++);


            Console.WriteLine("Menu Loaded lol");
        }



    }
}
