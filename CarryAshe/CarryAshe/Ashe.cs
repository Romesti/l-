using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CarryAshe
{
    internal enum Spells
    {
        Q,
        W,
        E,
        R
    }

    internal class Ashe
    {

        #region Attributes

        /// <summary>
        /// Menu handler
        /// </summary>
        private AsheMenu _menu;

        /// <summary>
        /// Drawing handler
        /// </summary>
        private AsheDrawings _drawings;

        /// <summary>
        /// Ignite Slot
        /// </summary>
        private SpellSlot _ignite;

        /// <summary>
        /// Spell Dictionary
        /// </summary>
        private Dictionary<Spells, Spell> _spells = new Dictionary<Spells, Spell>()
        {
            { Spells.Q, new Spell(SpellSlot.Q) },
            { Spells.W, new Spell(SpellSlot.W, 1200) },
            { Spells.E, new Spell(SpellSlot.E) },
            { Spells.R, new Spell(SpellSlot.R) }
        };
        #endregion

        #region Properties

        /// <summary>
        /// Getter for L#Menu
        /// </summary>
        public Menu Menu {get{return _menu.Menu;}}

        /// <summary>
        /// Getter for Orbwalker
        /// </summary>
        public Orbwalking.Orbwalker Orbwalker { get; set; }



        public int QStacks { get { return Player.GetBuffCount("AsheQ") + Player.GetBuffCount("AsheQCastReady"); } }

        public bool IsQMaxStacked { get { return Player.HasBuff("AsheQCastReady"); } }
        #endregion

        #region Helpers

        /// <summary>
        /// Getter for current script version
        /// </summary>
        public  String ScriptVersion { get { return typeof(Ashe).Assembly.GetName().Version.ToString(); } }

        /// <summary>
        /// Getter for the Player
        /// </summary>
        public  Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        #endregion



        #region Constructor
        /// <summary>
        /// Ctor
        /// </summary>
        public Ashe()
        {
            _menu = new AsheMenu(this);

            _drawings = new AsheDrawings(this);

            _spells[Spells.W].SetSkillshot(0.5f, 100, 900, true, SkillshotType.SkillshotCone);
            _spells[Spells.R].SetSkillshot(0.5f, 100, 1600, false, SkillshotType.SkillshotCone);
        }
        #endregion
        
        #region Functions

        internal Spell GetSpell(Spells spell)
        {
            return _spells[spell];
        }

        /// <summary>
        /// OnLoad Callback
        /// </summary>
        /// <param name="args"></param>
        public void  OnLoad(EventArgs args)
        {
            Console.WriteLine("loadin");

            if (Player.ChampionName != "Ashe")
                return;
            _menu.Initialize();
            _drawings.Initialize();

            Game.OnUpdate += onGameUpdate;
            Drawing.OnDraw += this._drawings.Drawing_OnDraw;
            Notifications.AddNotification(String.Format("{0} by Romesti v{1}",this.GetNamespace(),ScriptVersion), 1000);

        }


        private  void onGameUpdate(EventArgs args)
        {
            if (Player.IsDead)
                return;
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    LaneClear();
                    JungleClear();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
            }

            AutoR();

        }

        #region itemusage

        private  void Items(Obj_AI_Base target)
        {
            var botrk = ItemData.Blade_of_the_Ruined_King.GetItem();
            var ghost = ItemData.Youmuus_Ghostblade.GetItem();
            var cutlass = ItemData.Bilgewater_Cutlass.GetItem();

            var useYoumuu = Menu.GetItemEndKey("Youmuu", "Items").GetValue<bool>();
            var useCutlass = Menu.GetItemEndKey("Cutlass", "Items").GetValue<bool>();
            var useBlade = Menu.GetItemEndKey("Blade", "Items").GetValue<bool>();

            var useBladeEhp = Menu.GetItemEndKey("BladeEnemyEHP","Items").GetValue<Slider>().Value;
            var useBladeMhp = Menu.GetItemEndKey("BladeEnemyMHP", "Items").GetValue<Slider>().Value;

            if (botrk.IsReady() && botrk.IsOwned(Player) && botrk.IsInRange(target) &&
                target.HealthPercent <= useBladeEhp && useBlade)
            {
                botrk.Cast(target);
            }

            if (botrk.IsReady() && botrk.IsOwned(Player) && botrk.IsInRange(target) &&
                Player.HealthPercent <= useBladeMhp && useBlade)
            {
                botrk.Cast(target);
            }

            if (cutlass.IsReady() && cutlass.IsOwned(Player) && cutlass.IsInRange(target) &&
                target.HealthPercent <= useBladeEhp && useCutlass)
            {
                cutlass.Cast(target);
            }

            if (ghost.IsReady() && ghost.IsOwned(Player) && Player.Distance(target,false) < Player.AttackRange*2.0 && useYoumuu)
            {
                ghost.Cast();
            }
        }
        #endregion


        
        #region Main Behaviors
        public  void Combo()
        {
            var useQ = Menu.GetItemEndKey("UseQ").GetValue<bool>();
            var useW = Menu.GetItemEndKey("UseW").GetValue<bool>();
            var useWMana = Menu.GetItemEndKey("UseWMana").GetValue<Slider>().Value;
            var useR = Menu.GetItemEndKey("UseR").GetValue<bool>();
            var saveR = Menu.GetItemEndKey("SaveR").GetValue<bool>();
            var target = TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(Player)+65, TargetSelector.DamageType.Physical);
            
            if (target == null || !target.IsValid)
                return;

            Func<Spells,bool> chechForUlt = (spellslot) => {
                return _spells[Spells.R].IsReady() && saveR && Player.Mana - 50 < 100; 
            };

            Items(target);

            if ( useQ && !chechForUlt(Spells.Q) &&this.IsQMaxStacked&& _spells[Spells.Q].IsReady())
            {
                _spells[Spells.Q].Cast();
            }
            if (useW && !chechForUlt(Spells.W) && Player.ManaPercent > useWMana && _spells[Spells.W].IsReady())
                _spells[Spells.W].CastIfHitchanceEquals(target, _menu.ComboHitChance);

            if (useR && _spells[Spells.R].IsReady())
            {
                _spells[Spells.R].CastIfHitchanceEquals(target, _menu.ComboHitChance);
            }

        }

        public  void Harass()
        {
            var target = TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(null) + 65, TargetSelector.DamageType.Physical);
            if (target == null || !target.IsValid)
                return;



            var useW = Menu.GetItemEndKey("UseW").GetValue<bool>();
            var useQ = Menu.GetItemEndKey("UseQ").GetValue<bool>();
            var manaThreshold = Menu.GetItemEndKey("ManaThreshold").GetValue<Slider>().Value;

            if (Player.ManaPercent < manaThreshold)
                return;

            if (useQ && this.IsQMaxStacked && _spells[Spells.Q].IsReady())
            {
                _spells[Spells.Q].Cast();
            }

            if (useW && _spells[Spells.W].IsReady())
                _spells[Spells.W].CastIfHitchanceEquals(target, _menu.ComboHitChance);

        }

        public  void LaneClear()
        {
            var minions = MinionManager.GetMinions(
                            ObjectManager.Player.ServerPosition,
                                    _spells[Spells.W].Range, MinionTypes.All,
                                    MinionTeam.Enemy,
                                    MinionOrderTypes.MaxHealth)
                            .Where(minion => minion.IsValidTarget(_spells[Spells.W].Range));

            var useQ = Menu.GetItemEndKey("UseQ", "Clear").GetValue<bool>();
            var useW = Menu.GetItemEndKey("UseW", "Clear").GetValue<bool>();

            if (minions.FirstOrDefault() == null) return;

            if (useQ && this.IsQMaxStacked && _spells[Spells.Q].IsReady() && minions.Count(m => Orbwalker.InAutoAttackRange(m)) > 1)
            {
                _spells[Spells.Q].Cast();
            }

            if (useW && _spells[Spells.W].IsReady() && minions.Count() > 3)
            {
                _spells[Spells.W].Cast(minions.Last(), false);
            }
        }

        public void JungleClear()
        {
            var minions = MinionManager.GetMinions(
            ObjectManager.Player.ServerPosition, _spells[Spells.W].Range, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            var useQ = Menu.GetItemEndKey("UseQ", "Clear").GetValue<bool>();
            var useW = Menu.GetItemEndKey("UseW", "Clear").GetValue<bool>();


            var wMinion = minions.FindAll(minion => minion.IsValidTarget(_spells[Spells.W].Range)).FirstOrDefault();
            
            if (wMinion == null) return;

            if (useQ && this.IsQMaxStacked && _spells[Spells.Q].IsReady())
            {
                _spells[Spells.Q].Cast();
            }

            if (useW && _spells[Spells.W].IsReady())
            {
                _spells[Spells.W].Cast(wMinion);
            }
            
        }
        #endregion

        #region Advanced Behaviors
        public void AutoR()
        {
            var autoR = Menu.GetItemEndKey("Toggle", "Misc.AutoR").GetValue<KeyBind>().Active;
            if (!autoR) return;
            var range = Menu.GetItemEndKey("Range", "Misc.AutoR").GetValue<Slider>().Value;
            var hitchance = Menu.GetItemEndKey("Hitchance", "Misc.AutoR").GetHitchance();
            var target = TargetSelector.GetTarget(range, TargetSelector.DamageType.Physical, false);

            if (target == null) return;

            if (_spells[Spells.R].IsReady())
            {
                _spells[Spells.R].CastIfHitchanceEquals(target,hitchance);
            }
        }
        #endregion

        #region ComboDamage

        public  float GetComboDamage(Obj_AI_Base enemy)
        {
            float damage = 0;

            if (_spells[Spells.Q].IsReady())
            {
                damage += _spells[Spells.Q].GetDamage(enemy);
            }

            if (_spells[Spells.W].IsReady())
            {
                damage += _spells[Spells.W].GetDamage(enemy);
            }

            if (_spells[Spells.E].IsReady())
            {
                damage += _spells[Spells.E].GetDamage(enemy);
            }

            if (_spells[Spells.R].IsReady())
            {
                damage += _spells[Spells.R].GetDamage(enemy);
            }

            if (_ignite == SpellSlot.Unknown || Player.Spellbook.CanUseSpell(_ignite) != SpellState.Ready)
            {
                damage += (float)Player.GetSummonerSpellDamage(enemy, Damage.SummonerSpell.Ignite);
            }

            return damage;
        }

        #endregion


        #endregion

    }
}
