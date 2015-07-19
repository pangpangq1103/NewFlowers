using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;

namespace Flowers_Ezreal
{
    class Ezreal
    {
        public static Menu Lost;
        public static void EzrealMenu()
        {
            Lost = new Menu("Flowers-Ezreal", "Lost.", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Lost.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            Lost.AddSubMenu(orbwalkerMenu);

            Lost.AddSubMenu(new Menu("Combo", "Combo"));
            Lost.SubMenu("Combo").AddItem(new MenuItem("lzp", "Use Q").SetValue(true));
            Lost.SubMenu("Combo").AddItem(new MenuItem("lzw", "Use W").SetValue(true));
            Lost.SubMenu("Combo").AddItem(new MenuItem("lzr", "Use R").SetValue(true));

            Lost.AddSubMenu(new Menu("Harass", "Harass"));
            Lost.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q").SetValue(true));
            Lost.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "Auto Q").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            Lost.SubMenu("Harass").AddItem(new MenuItem("srw", "Use W").SetValue(false));
            Lost.SubMenu("Harass").AddItem(new MenuItem("srmp", "Harass Mana <=%").SetValue(new Slider(40, 0, 100)));

            Lost.AddSubMenu(new Menu("Clear", "Clear"));
            Lost.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q LaneClear").SetValue(true));
            Lost.SubMenu("Clear").AddItem(new MenuItem("qyq", "Use Q JungleClear").SetValue(true));
            Lost.SubMenu("Clear").AddItem(new MenuItem("qxmp", "Clear Mana <=%").SetValue(new Slider(30, 0, 100)));

            Lost.AddSubMenu(new Menu("Last Hit", "LastHit"));
            Lost.SubMenu("LastHit").AddItem(new MenuItem("bdq", "Use Q LastHit").SetValue(true));

            Lost.AddSubMenu(new Menu("R Setting", "Uit"));
            Lost.SubMenu("Uit").AddItem(new MenuItem("autor", "R KS").SetValue(true));
            Lost.SubMenu("Uit").AddItem(new MenuItem("shoudongr", "Semi-automatic R").SetValue(new KeyBind("R".ToCharArray()[0], KeyBindType.Press)));
            Lost.SubMenu("Uit").AddItem(new MenuItem("minR", "min R range").SetValue(new Slider(500, 1500, 0)));

            Lost.AddSubMenu(new Menu("Item", "Item"));
            Lost.SubMenu("Item").AddItem(new MenuItem("UseYUU", "Use Youmuu's Ghostblade").SetValue(true));
            Lost.SubMenu("Item").AddItem(new MenuItem("UseBRK", "Use Blade of the Ruined King").SetValue(true));
            Lost.SubMenu("Item").AddItem(new MenuItem("UseBC", "Use Bilgewater Cutlass").SetValue(true));

            Lost.AddSubMenu(new Menu("Misc", "Misc"));
            Lost.SubMenu("Misc").AddItem(new MenuItem("packets", "Use Packets").SetValue(true));
            Lost.SubMenu("Misc").AddItem(new MenuItem("eee", " Jump to mouse").SetValue(new KeyBind("E".ToCharArray()[0], KeyBindType.Press)));
            Lost.SubMenu("Misc").AddItem(new MenuItem("Hit", "Spell HitChance").SetValue(true));
            Lost.SubMenu("Misc").AddItem(new MenuItem("Hit1", "true = VeryHigh"));
            Lost.SubMenu("Misc").AddItem(new MenuItem("Hit2", "false = High"));

            Lost.AddSubMenu(new Menu("Drawings", "Drawing"));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("closeD", "Dis Draw").SetValue(true));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "W Range").SetValue(new Circle(true, Color.FromArgb(202, 170, 255))));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "Draw Minion LastHit").SetValue(new Circle(true, Color.GreenYellow)));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "Draw Minion Near Kill").SetValue(new Circle(true, Color.Gray)));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "Real AA Range(OKTW© Style)").SetValue(true));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));
            Lost.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "Jungle position").SetValue(true));

            Lost.AddSubMenu(new Menu("Message", "Message"));
            Lost.SubMenu("Message").AddItem(new MenuItem("Credit", "Credit : NightMoon"));
            Lost.SubMenu("Message").AddItem(new MenuItem("Version", "Version : 1.0.0.0"));

            Lost.AddToMainMenu();
        }
    }
}
