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
            菜单 = new Menu("Flowers-卡莉斯塔重做", "Lost.", true);

            var targetSelectorMenu = new Menu("目标选择", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("走砍设置", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            菜单.AddSubMenu(orbwalkerMenu);

            菜单.AddSubMenu(new Menu("连招设置", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzp", "使用 Q").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "使用 E").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeee", "使用 E 击杀").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeeeeee", "最大 E 堆叠").SetValue(new Slider(5, 1, 20)));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzmp", "连招最低蓝量 <=%").SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("骚扰设置", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "使用 Q").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "自动 骚扰").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srmp", "骚扰最低蓝量 <=%").SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("清理设置", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "使用 Q 清线").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxqqq", "使用 Q 命中小兵数").SetValue(new Slider(3, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxe", "使用 E 清线").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxeee", "使用 E 命中小兵数").SetValue(new Slider(2, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "使用 Q 清野").SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "使用 E 清野").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("eqiangyeguai", "使用 E 抢野怪").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "清理最低蓝量 <=%").SetValue(new Slider(60, 0, 100)));

            /*{        
            
            
            菜单.AddSubMenu(new Menu("R Setting", "RSetting"));
            菜单.SubMenu("RSetting").AddItem(new MenuItem("jilao", "Save Your Sweetheart").SetValue(true));
            菜单.SubMenu("RSetting").AddItem(new MenuItem("jilaohp", "Sweetheart Hp <=%").SetValue(new Slider(20, 0, 100)));
                         }*/
            菜单.AddSubMenu(new Menu("物品使用", "Item"));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseYUU", "使用 幽梦之灵").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRK", "使用 破败王者之刃").SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBC", "使用 比尔沃吉特弯刀").SetValue(true));

            菜单.AddSubMenu(new Menu("逃跑设置", "Flee"));
            菜单.SubMenu("Flee").AddItem(new MenuItem("Flee", "逃跑 按键").SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press)));            


         /*   菜单.AddSubMenu(new Menu("Balista", "BiuBiuBiu"));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("useToggle", "Toggle").SetValue(false));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("useOnComboKey", "Enabled").SetValue(new KeyBind(32, KeyBindType.Press)));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("minRange", "Min Range to Balista", true).SetValue(new Slider(700, 100, 1449)));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("maxRange", "Max Range to Balista", true).SetValue(new Slider(1500, 100, 1500)));
            菜单.SubMenu("BiuBiuBiu").AddItem(new MenuItem("BiuBiuBiu1", "Kalista and Blitzcrank biubiubiu~"));

            菜单.AddSubMenu(new Menu("Balista Target", "BalistaTarget"));*/

            菜单.AddSubMenu(new Menu("杂项设置", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("DamageExxx", "自动 E -> 当小兵可被E击杀并且英雄身上有Buff").SetValue(true));
            菜单.SubMenu("Misc").AddItem(new MenuItem("usePackets", "使用 封包").SetValue(false));

            菜单.AddSubMenu(new Menu("显示设置", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q 范围").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "W 范围").SetValue(new Circle(false, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E 范围").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R 范围").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
         /*   菜单.SubMenu("Drawing").AddItem(new MenuItem("minBRange", "Balista Min Range").SetValue(new Circle(false, Color.Chartreuse)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("maxBRange", "Balista Max Range").SetValue(new Circle(false, Color.Green)));*/
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "显示 可补刀小兵").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "显示 附近可击杀小兵").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "AA 范围(OKTW© 风格)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA 目标(OKTW© 风格)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("DrawEDamage", "显示 E 伤害").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "无伤打野点").SetValue(true));

            菜单.AddSubMenu(new Menu("信息提示", "Message"));
            菜单.SubMenu("Message").AddItem(new MenuItem("Credit", "作者 : 花边下丶情未央"));
            菜单.SubMenu("Message").AddItem(new MenuItem("tttttt", "                Lost."));
            菜单.SubMenu("Message").AddItem(new MenuItem("Version", "版本 : 1.0.0.0"));

            菜单.AddToMainMenu();
        }
    }
}
