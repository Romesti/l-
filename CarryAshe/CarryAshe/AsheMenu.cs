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
        public  HitChance CustomHitChance
        {
            get { return GetHitchance(); }
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
            Menu = new Menu("CarryAshe", "menu", true);
            var orbwalkerMenu = new Menu("Orbwalker", "orbwalker");
            _parentAssembly.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);

            Menu.AddSubMenu(orbwalkerMenu);

            var targetSelector = new Menu("Target Selector", "TargetSelector");
            TargetSelector.AddToMenu(targetSelector);

            Menu.AddSubMenu(targetSelector);

            var drawingMenu = new Menu("Drawings", "CarryAshe.Drawings");
            drawingMenu.AddItem(new MenuItem("CarryAshe.Drawings.Off", "Activate Drawings").SetValue(true));
            drawingMenu.AddItem(new MenuItem("CarryAshe.Drawings.W", "Draw W Range").SetValue(new Circle()));
           // drawingMenu.AddItem(new MenuItem("CarryAshe.Drawings.FillColor", "Fill colour", true).SetValue(new Circle(true, Color.FromArgb(204, 204, 0, 0))));
            Menu.AddSubMenu(drawingMenu);

            var comboMenu = new Menu("Combo", "CarryAshe.Combo");
            comboMenu.AddItem(new MenuItem("CarryAshe.Combo.HitChance", "Hitchance").SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 3)));
            comboMenu.AddItem(new MenuItem("CarryAshe.Combo.UseQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("CarryAshe.Combo.UseW", "Use W").SetValue(true));
            comboMenu.AddItem(new MenuItem("CarryAshe.Combo.UseR", "Use R").SetValue(true));

            Menu.AddSubMenu(comboMenu);

            var harassMenu = new Menu("Harrass", "CarryAshe.Harass");
            harassMenu.AddItem(new MenuItem("CarryAshe.Harass.UseQ", "Use Q").SetValue(true));
            harassMenu.AddItem(new MenuItem("CarryAshe.Harass.UseW", "Use W").SetValue(true));
            harassMenu.AddItem(new MenuItem("CarryAshe.Harass.ManaThreshold", "Minimum mana for harass")).SetValue(new Slider(55));

            Menu.AddSubMenu(harassMenu);

            var clearMenu = new Menu("Lane & Jungler Clear", "CarryAshe.Clear");
            clearMenu.AddItem(new MenuItem("CarryAshe.Clear.UseQ", "Use Q").SetValue(true));
            clearMenu.AddItem(new MenuItem("CarryAshe.Clear.UseW", "Use W").SetValue(true));

            Menu.AddSubMenu(clearMenu);

            var itemMenu = new Menu("Items", "Items");
            itemMenu.AddItem(new MenuItem("CarryAshe.Items.Youmuu", "Use Youmuu's Ghostblade").SetValue(true));
            itemMenu.AddItem(new MenuItem("CarryAshe.Items.Cutlass", "Use Cutlass").SetValue(true));
            itemMenu.AddItem(new MenuItem("CarryAshe.Items.Blade", "Use Blade of the Ruined King").SetValue(true));
            itemMenu.AddItem(new MenuItem("CarryAshe.Items.azeazzaeae", ""));
            itemMenu.AddItem(new MenuItem("CarryAshe.Items.Blade.EnemyEHP", "Enemy HP Percentage").SetValue(new Slider(80, 100, 0)));
            itemMenu.AddItem(new MenuItem("CarryAshe.Items.Blade.EnemyMHP", "My HP Percentage").SetValue(new Slider(80, 100, 0)));
            
            Menu.AddSubMenu(itemMenu);


            var credits = new Menu("Credits", "Romesti");
            credits.AddItem(new MenuItem("CarryAshe.Paypal", "You can make a donation via paypal :)"));
            Menu.AddSubMenu(credits);

            Menu.AddItem(new MenuItem("422442fsaafs4242f", ""));
            Menu.AddItem(new MenuItem("422442fsaafsf", "Version "+Ashe.ScriptVersion));
            Menu.AddItem(new MenuItem("fsasfafsfsafsa", "Made By Romesti"));

            Menu.AddToMainMenu();


            Console.WriteLine("Menu Loaded lol");
        }

        /// <summary>
        /// Credit to jQuery
        /// </summary>
        /// <returns>The hitchance selected in the menu</returns>
        private  HitChance GetHitchance()
        {
            switch (Menu.Item("CarryAshe.Combo.HitChance").GetValue<StringList>().SelectedIndex)
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
    }
}
