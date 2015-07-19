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
            菜单 = new Menu("Flowers-德莱文", "Feeeez", true);

            var targetSelectorMenu = new Menu("目标选择", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkMenu = new Menu("走砍设置", "orbwalker");
            Draven.Orbwalker = new Orbwalking.Orbwalker(orbwalkMenu);
            菜单.AddSubMenu(orbwalkMenu);

            菜单.AddSubMenu(new Menu("连招设置", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzq", "使用 Q").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzw", "使用 W").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzw1", "使用 W 突进").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "使用 E").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzr", "使用 R").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzr1", "使用 二段R").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzrhp", "使用 R 敌人Hp低于").SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("骚扰设置", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "使用 Q").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srw", "使用 W").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srw1", "使用 W 突进").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("sre", "使用 E").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srmp", "骚扰最低蓝量比").SetValue(new Slider(40, 0, 100)));

            菜单.AddSubMenu(new Menu("清线清野", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qx", "清线 设置"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "使用 Q").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qy", "清野 设置"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "使用 Q").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyw", "使用 W").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "使用 E").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "清线清野丨最低蓝量").SetValue(new Slider(20, 0, 100)));

            菜单.AddSubMenu(new Menu("斧头设置", "axeSetting"));
            菜单.SubMenu("axeSetting").AddItem(
                new MenuItem("AxeMode", "捡斧头模式:").SetValue(new StringList(new[] {"连招", "随机", "一直"},
                    2)));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("DrawAxeLocation", "显示 斧头 位置").SetValue(true));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("DrawAxeRange", "显示 斧头 范围").SetValue(true));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("CatchAxeRange", "捡取 斧头 范围").SetValue(new Slider(800, 120, 1500)));

            菜单.SubMenu("axeSetting").AddItem(new MenuItem("MaxAxes", "最大 斧头数").SetValue(new Slider(2, 1, 3)));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("UseWForQ", "使用W 加速捡斧头").SetValue(true));
            菜单.SubMenu("axeSetting").AddItem(new MenuItem("DontCatchUnderTurret", "禁止 塔下捡斧头").SetValue(true));

            菜单.AddSubMenu(new Menu("物品使用", "Item"));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseYUU", "使用 幽梦").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRK", "使用 破败").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBC", "使用 弯刀").SetValue(true));

            菜单.AddSubMenu(new Menu("范围显示", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawing", "开启 范围 显示").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q 范围").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E 范围").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R 范围").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "显示 小兵 可补刀").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "显示 附近可击杀 小兵").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "AA 范围(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA 目标(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "无伤 打野").SetValue(true));

            菜单.AddToMainMenu();
        }
    }
}
