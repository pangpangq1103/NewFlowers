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
            菜单 = new Menu("Flowers-韦鲁斯", "Lost.", true);

            var targetSelectorMenu = new Menu("目标选择", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("走砍设置", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            菜单.AddSubMenu(orbwalkerMenu);

            菜单.AddSubMenu(new Menu("连招设置", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzp", "使用 Q").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("qhitChance", "Q 命中率").SetValue(new StringList(new[] { "低", "中", "高", "非常高" }, 3)));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "使用 E").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzr", "使用 R").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzrrr", "R 最少敌人数 >= ")).SetValue(new Slider(2, 1, 5));

            菜单.AddSubMenu(new Menu("骚扰设置", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "使用 Q").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("qhitChance", "Q 命中率").SetValue(new StringList(new[] { "低", "中", "高", "非常高" }, 3)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("sre", "使用 E").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoHarass", "自动 E 骚扰").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));

            菜单.AddSubMenu(new Menu("清理设置", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "使用 Q 清线").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxqqq", "使用 Q 命中小兵").SetValue(new Slider(3, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxe", "使用 E 清线").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxeee", "使用 E 命中小兵").SetValue(new Slider(2, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "使用 Q 清野").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "使用 E 清野").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "清理 最低蓝量比 <=%").SetValue(new Slider(60, 0, 100)));

            菜单.AddSubMenu(new Menu("杂项设置", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("shoudongR", "手动 R").SetValue(new KeyBind("R".ToCharArray()[0], KeyBindType.Press)));

            菜单.AddSubMenu(new Menu("显示设置", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q 范围").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E 范围").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R 范围").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "小兵补刀血量显示").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "附近可击杀的小兵").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "AA 范围(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA 目标(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "无伤打野点").SetValue(true));

            菜单.AddSubMenu(new Menu("信息", "Message"));
            菜单.SubMenu("Message").AddItem(new MenuItem("Credit", "作者 : 花边下丶情未央"));
            菜单.SubMenu("Message").AddItem(new MenuItem("Version", "Version : 1.0.0.0"));
			
			菜单.AddToMainMenu();
        }
    }
}
