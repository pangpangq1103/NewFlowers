using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;

namespace Flowers_Draven
{
    class DM
    {
        public static Menu 菜单;
        public static void DMenu()
        {
            菜单 = new Menu("Flowers-Draven", "Feeeez", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkMenu = new Menu("Orbwalker", "orbwalker");
            Draven.Orbwalker = new Orbwalking.Orbwalker(orbwalkMenu);
            菜单.AddSubMenu(orbwalkMenu);

            菜单.AddSubMenu(new Menu("Combo", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzq", "Use Q").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzw", "Use W").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzw1", "Use W Dash").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "Use E").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzr", "Use R").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzr1", "Use R2").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzrhp", "Use R Emery Hp <=").SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("Harass", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srw", "Use W").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srw1", "Use W Dash").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("sre", "Use E").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srmp", "Harass Mana <=").SetValue(new Slider(40, 0, 100)));

            菜单.AddSubMenu(new Menu("Clear", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qx", "LaneClear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qy", "JungleClear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "Use Q").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyw", "Use W").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "Use E").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "Clear Mana <=").SetValue(new Slider(20, 0, 100)));

            菜单.AddSubMenu(new Menu("AxeSetting", "axeSetting"));
            菜单.SubMenu("axeSetting").AddItem(
                new MenuItem("AxeMode", "Axe Mode:").SetValue(new StringList(new[] {"Combo", "Ramdom", "Always"},
                    2)));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("DrawAxeLocation", "Draw Axe Location").SetValue(true));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("DrawAxeRange", "Draw Axe Range").SetValue(true));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("CatchAxeRange", "Catch Axe Range").SetValue(new Slider(800, 120, 1500)));

            菜单.SubMenu("axeSetting").AddItem(new MenuItem("MaxAxes", "Max Axes").SetValue(new Slider(2, 1, 3)));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("UseWForQ", "Use W For Q").SetValue(true));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("DontCatchUnderTurret", "Dont Catch Under Turret").SetValue(true));

            菜单.AddSubMenu(new Menu("Item", "Item"));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseYUU", "UseYUU").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRK", "UseBRK").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBC", "UseBC").SetValue(true));

            菜单.AddSubMenu(new Menu("Drawing", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawing", "Dis drawing").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "Million LastHit").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "Millions Can Kill").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "AA Range(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "Jungle Postion").SetValue(true));

            菜单.AddToMainMenu();
        }
    }
}
