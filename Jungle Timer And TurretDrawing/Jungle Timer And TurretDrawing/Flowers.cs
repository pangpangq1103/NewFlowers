using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp.Common;

namespace Jungle_Timer_And_TurretDrawing
{
    class Flowers
    {
        public static Menu 菜单;
        internal static void Menu()
        {
            菜单 = new Menu("JGTimer&TurretD", "Lost In Love.", true);

            菜单.SubMenu("Jungle Timer").AddItem(new MenuItem("Timer", "Enabled").SetValue(true));
            菜单.SubMenu("Jungle Timer").AddItem(new MenuItem("Timer1", "MiniHap").SetValue(true));
            菜单.SubMenu("Jungle Timer").AddItem(new MenuItem("JungleTimerFormat", "Timer:").SetValue(new StringList(new[] { "Min:Sec", "Sec" })));

            菜单.SubMenu("Turret Health").AddItem(new MenuItem("TIHealth", "Turret Health").SetValue(new StringList(new[] { "Per", "Num" })));
            菜单.SubMenu("Turret Health").AddItem(new MenuItem("HealthActive", "Enabled").SetValue(true));

            菜单.SubMenu("Turret Range").AddItem(new MenuItem("RangeEnabled", "Enabled").SetValue(true));

            菜单.AddItem(new MenuItem("Flowers", "Credit : NightMoon"));
            菜单.AddItem(new MenuItem("Version", "Version : 1.0.0.0"));

            菜单.AddToMainMenu();
        }
    }
}
