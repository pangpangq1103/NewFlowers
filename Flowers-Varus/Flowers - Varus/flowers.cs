using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;

namespace Flowers___Varus
{
    class flowers
    {
        public static Menu 菜单;
        public static void VarusMenu()
        {
            菜单 = new Menu("Flowers-Varus", "Lost.", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            菜单.AddSubMenu(orbwalkerMenu);

            菜单.AddSubMenu(new Menu("Combo", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzp", "Use Q").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("qhitChance", "Q Hitchance").SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 3)));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "Use E").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzr", "Use R").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzrrr", "R when Emery >= ")).SetValue(new Slider(2, 1, 5));

            菜单.AddSubMenu(new Menu("Harass", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("qhitChance", "Q Hitchance").SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 3)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("sre", "Use E").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoHarass", "Auto E Harass").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));

            菜单.AddSubMenu(new Menu("Clear", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q LaneClear").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxqqq", "Use Q Millions").SetValue(new Slider(3, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxe", "Use E LaneClear").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxeee", "Use E Millions").SetValue(new Slider(2, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("bde", "Use E LastHit").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "Use Q JungleClear").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "Use E JungleClear").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "Clear Mana <=%").SetValue(new Slider(60, 0, 100)));

            菜单.AddSubMenu(new Menu("Item", "Item"));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseYUU", "Use Youmuu's Ghostblade").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBC", "Use Bilgewater Cutlass").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRK", "Use Blade of the Ruined King").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRKEHP", "Enemy HP Percentage").SetValue(new Slider(80, 100, 0)));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRKMHP", "My HP Percentage").SetValue(new Slider(80, 100, 0)));

            菜单.AddSubMenu(new Menu("Misc", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("shoudongR", "Semimanual R").SetValue(new KeyBind("R".ToCharArray()[0], KeyBindType.Press)));

            菜单.AddSubMenu(new Menu("Drawings", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "Draw Minion LastHit").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "Draw Minion Near Kill").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "Real AA Range(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "Jungle position").SetValue(true));

            菜单.AddSubMenu(new Menu("Message", "Message"));
            菜单.SubMenu("Message").AddItem(new MenuItem("Credit", "Credit : NightMoon"));
            菜单.SubMenu("Message").AddItem(new MenuItem("Version", "Version : 1.0.0.0"));
            菜单.AddToMainMenu();
        }
    }
}
