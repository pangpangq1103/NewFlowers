using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;


namespace Riven
{
    class huabian
    {
        public static Menu 菜单;
        public static void RivenMenu()
        {
            菜单 = new Menu("Flowers-Riven", "Lost.", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            菜单.AddSubMenu(orbwalkerMenu);

            菜单.AddSubMenu(new Menu("BurstCombo", "burstCombo"));
            菜单.SubMenu("burstCombo").AddItem(new MenuItem("burstComboExpanded", "Combo")).SetValue(new KeyBind(32, KeyBindType.Press));
            菜单.SubMenu("burstCombo").AddItem(new MenuItem("reset", "Reset Combo")).SetValue(false);
            菜单.SubMenu("burstCombo").AddItem(new MenuItem("delay", "Delay")).SetValue(new Slider(110, 40, 200));
            菜单.SubMenu("burstCombo").AddItem(new MenuItem("info1", "E -> R -> Flash -> W -> Q -> AA -> Hydra -> R2 -> Q2 doublecast"));

            菜单.AddSubMenu(new Menu("WallJumper", "WallJumper"));
            菜单.SubMenu("WallJumper").AddItem(new MenuItem("jump", "WallJumper")).SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press));

            菜单.AddSubMenu(new Menu("Drawings", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "W Range").SetValue(new Circle(false, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("draw", "Draw engage range")).SetValue(true);
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "Draw Minion LastHit").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "Draw Minion Near Kill").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "Real AA Range(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));

            菜单.AddToMainMenu();
        }
    }
}


