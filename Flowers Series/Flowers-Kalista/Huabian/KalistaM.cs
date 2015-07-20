using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;

namespace Flowers滑板鞋_重生_
{
    class KalistaM
    {
        public static Menu 菜单;
        public static void KalistaMenu()
        {
            菜单 = new Menu("Flowers-Kalista", "Lost.", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            菜单.AddSubMenu(orbwalkerMenu);

            菜单.AddSubMenu(new Menu("Combo", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzp", "Use Q", true).SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "Use E", true).SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeee", "Use E KS", true).SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeeeeee", "Max E Stack", true).SetValue(new Slider(5, 1, 20)));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzmp", "Combo Mana <=%", true).SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("Harass", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q", true).SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "Auto Q Harass", true).SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srmp", "Harass Mana <=%", true).SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("Clear", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q LaneClear", true).SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxqqq", "Use Q Millions", true).SetValue(new Slider(3, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxe", "Use E LaneClear", true).SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxeee", "Use E Millions", true).SetValue(new Slider(2, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "Use Q JungleClear", true).SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "Use E JungleClear", true).SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("eqiangyeguai", "Use E Steal Jungle", true).SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "Clear Mana <=%", true).SetValue(new Slider(60, 0, 100)));

            菜单.AddSubMenu(new Menu("Item", "Item"));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseYUU", "Use Youmuu's Ghostblade", true).SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRK", "Use Blade of the Ruined King", true).SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBC", "Use Bilgewater Cutlass", true).SetValue(true));

            菜单.AddSubMenu(new Menu("Flee", "Flee"));
            菜单.SubMenu("Flee").AddItem(new MenuItem("Flee", "Flee", true).SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press)));            


            菜单.AddSubMenu(new Menu("Misc", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("DamageExxx", "Auto E -> When millions will die and hero has buff", true).SetValue(true));
            菜单.SubMenu("Misc").AddItem(new MenuItem("AutoW", "Auto W", true).SetValue(false));
            菜单.SubMenu("Misc").AddItem(new MenuItem("autowenemyclose", "Dont Send W with an enemy in X Range:", true).SetValue(new Slider(2000, 0, 5000)));
            菜单.SubMenu("Misc").AddItem(new MenuItem("harassEoutOfRange", "Use E when out of range", true).SetValue(true));
            菜单.SubMenu("Misc").AddItem(new MenuItem("KillSteal", "KillSteal", true).SetValue(true));



            菜单.AddSubMenu(new Menu("Drawings", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "W Range").SetValue(new Circle(false, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "Draw Minion LastHit").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "Draw Minion Near Kill").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "Real AA Range(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("DrawEDamage", "Drawing E Damage").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "Jungle position").SetValue(true));

            菜单.AddItem(new MenuItem("Credit", "Credit : NightMoon"));
            菜单.AddToMainMenu();
        }
    }
}
