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
            菜单 = new Menu("打野计时塔血量范围", "Lost In Love.", true);

            菜单.SubMenu("打野计时").AddItem(new MenuItem("Timer", "大地图显示").SetValue(true));
            菜单.SubMenu("打野计时").AddItem(new MenuItem("Timer1", "小地图显示").SetValue(true));
            菜单.SubMenu("打野计时").AddItem(new MenuItem("JungleTimerFormat", "时间格式:").SetValue(new StringList(new[] { "Min:Sec", "Sec" })));

            菜单.SubMenu("塔血量").AddItem(new MenuItem("TIHealth", "塔血量显示格式").SetValue(new StringList(new[] { "Per", "Num" })));
            菜单.SubMenu("塔血量").AddItem(new MenuItem("HealthActive", "启用").SetValue(true));

            菜单.SubMenu("塔范围").AddItem(new MenuItem("RangeEnabled", "启用").SetValue(true));

            菜单.AddItem(new MenuItem("Flowers", "作者 : 花边"));
            菜单.AddItem(new MenuItem("Version", "版本 : 1.0.0.0"));

            菜单.AddToMainMenu();
        }
    }
}
