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
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzp", "Use Q").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "Use E").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeee", "Use E KS").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeeeeee", "Max E Stack").SetValue(new Slider(5, 1, 20)));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzmp", "Combo Mana <=%").SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("Harass", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "Auto Harass").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srmp", "Harass Mana <=%").SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("Clear", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q LaneClear").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxqqq", "Use Q Millions").SetValue(new Slider(3, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxe", "Use E LaneClear").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxeee", "Use E Millions").SetValue(new Slider(2, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "Use Q JungleClear").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "Use E JungleClear").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("eqiangyeguai", "Use E Steal Jungle").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "Clear Mana <=%").SetValue(new Slider(60, 0, 100)));

            /*{        
            
            
            菜单.AddSubMenu(new Menu("R Setting", "RSetting"));
            菜单.SubMenu("RSetting").AddItem(new MenuItem("jilao", "Save Your Sweetheart").SetValue(true));
            菜单.SubMenu("RSetting").AddItem(new MenuItem("jilaohp", "Sweetheart Hp <=%").SetValue(new Slider(20, 0, 100)));
                         }*/
            菜单.AddSubMenu(new Menu("Item", "Item"));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseYUU", "Use Youmuu's Ghostblade").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRK", "Use Blade of the Ruined King").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBC", "Use Bilgewater Cutlass").SetValue(true));

            菜单.AddSubMenu(new Menu("Flee", "Flee"));
            菜单.SubMenu("Flee").AddItem(new MenuItem("Flee", "Flee").SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press)));            


         /*   菜单.AddSubMenu(new Menu("Balista", "BiuBiuBiu"));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("useToggle", "Toggle").SetValue(false));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("useOnComboKey", "Enabled").SetValue(new KeyBind(32, KeyBindType.Press)));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("minRange", "Min Range to Balista", true).SetValue(new Slider(700, 100, 1449)));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("maxRange", "Max Range to Balista", true).SetValue(new Slider(1500, 100, 1500)));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("BiuBiuBiu1", "Kalista and Blitzcrank biubiubiu~"));

            菜单.AddSubMenu(new Menu("Balista Target", "BalistaTarget"));*/

            菜单.AddSubMenu(new Menu("Misc", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("DamageExxx", "Auto E -> When millions will die and hero has buff").SetValue(true));
            菜单.SubMenu("Misc").AddItem(new MenuItem("usePackets", "Use Packets").SetValue(false));

            菜单.AddSubMenu(new Menu("Drawings", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "W Range").SetValue(new Circle(false, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
         /*   菜单.SubMenu("Drawing").AddItem(new MenuItem("minBRange", "Balista Min Range").SetValue(new Circle(false, Color.Chartreuse)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("maxBRange", "Balista Max Range").SetValue(new Circle(false, Color.Green)));*/
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "Draw Minion LastHit").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "Draw Minion Near Kill").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "Real AA Range(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("DrawEDamage", "Drawing E Damage").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "Jungle position").SetValue(true));

            菜单.AddSubMenu(new Menu("Message", "Message"));
            菜单.SubMenu("Message").AddItem(new MenuItem("Credit", "Credit : NightMoon"));
            菜单.SubMenu("Message").AddItem(new MenuItem("Version", "Version : 1.0.0.0"));
            菜单.SubMenu("Message").AddItem(new MenuItem("ttt", "if you have more Bug pls tell me"));

            菜单.AddToMainMenu();
        }
    }
}
