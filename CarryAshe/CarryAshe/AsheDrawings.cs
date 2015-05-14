using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using Color = System.Drawing.Color;

namespace CarryAshe
{
    internal class AsheDrawings
    {
        private Ashe _parentAssembly;

        public AsheDrawings(Ashe parentAssembly)
        {
            _parentAssembly = parentAssembly;
        }

        public void Initialize()
        {
            var drawOffMenu = _parentAssembly.Menu.Item("CarryAshe.Drawings.Off");
          //  var drawFillColorMenu = _parentAssembly.Menu.Item("CarryAshe.Drawings.FillColor");
            DrawDamage.DamageToUnit = _parentAssembly.GetComboDamage;

            DrawDamage.Enabled = drawOffMenu.GetValue<bool>();
        //    DrawDamage.Fill = drawFillColorMenu.GetValue<Circle>().Active;
         //   DrawDamage.FillColor = drawFillColorMenu.GetValue<Circle>().Color;

            drawOffMenu.ValueChanged += (sender, eventArgs) =>
            {
                DrawDamage.Enabled = eventArgs.GetNewValue<bool>();
            };

            //drawFillColorMenu.ValueChanged += ( sender, eventArgs) =>
            //{
            //    DrawDamage.Fill = eventArgs.GetNewValue<Circle>().Active;
            //    DrawDamage.FillColor = eventArgs.GetNewValue<Circle>().Color;
            //};
        }

        public void Drawing_OnDraw(EventArgs args)
        {

            var drawOff = _parentAssembly.Menu.Item("CarryAshe.Drawings.Off").GetValue<bool>();
            var drawW = _parentAssembly.Menu.Item("CarryAshe.Drawings.W").GetValue<Circle>();

            if (drawOff)
                return;

            if (drawW.Active)
                if (_parentAssembly.GetSpell(Spells.W).Level > 0)
                {
                    Render.Circle.DrawCircle(ObjectManager.Player.Position, _parentAssembly.GetSpell(Spells.W).Range, drawW.Color);
                }

            
        }
    }
}