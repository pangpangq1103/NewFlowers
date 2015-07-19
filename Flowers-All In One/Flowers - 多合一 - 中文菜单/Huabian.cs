using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Flowers___All_In_One
{
    class Huabian
    {
        public static Menu 菜单;
        public static void Menu()
        {
            菜单 = new Menu("Flowers - All In One", "All In One.", true);

            菜单.AddSubMenu(new Menu("              ---自动 功能---", "Auto"));
            菜单.SubMenu("Auto").AddItem(new MenuItem("Lantern", "              ---自动灯笼"));
            菜单.SubMenu("Auto").AddItem(new MenuItem("Auto", "低血量自动捡灯笼").SetValue(true));
            菜单.SubMenu("Auto").AddItem(new MenuItem("Low", "血量百分比").SetValue(new Slider(20, 30, 5)));
            菜单.SubMenu("Auto").AddItem(new MenuItem("Hotkey", "按键").SetValue(new KeyBind(32, KeyBindType.Press)));
            菜单.SubMenu("Auto").AddItem(new MenuItem("Ignite", "              ---自动点燃"));
            菜单.SubMenu("Auto").AddItem(new MenuItem("ignitekey1", "点燃按键").SetValue(new KeyBind('D', KeyBindType.Press)));
            菜单.SubMenu("Auto").AddItem(new MenuItem("ignitetarget", "释放目标").SetValue(new StringList(new[] { "低血量", "目标选择器" })));
            菜单.SubMenu("Auto").AddItem(new MenuItem("igniteautokill", "自动击杀").SetValue(true));
            菜单.SubMenu("Auto").AddItem(new MenuItem("Zy", "              ---自动中亚"));
            菜单.SubMenu("Auto").AddItem(new MenuItem("enableseraph", "使用大天使之拥").SetValue(true));
            菜单.SubMenu("Auto").AddItem(new MenuItem("enablehpzhonya", "低血量自动中亚").SetValue(true));
            菜单.SubMenu("Auto").AddItem(new MenuItem("hptozhonya", "血量百分比")).SetValue(new Slider(25, 0, 100));
            菜单.SubMenu("Auto").AddItem(new MenuItem("minspelldmg", "造成伤害 % (不是很危险)")).SetValue(new Slider(45, 0, 100));
            菜单.SubMenu("Auto").AddItem(new MenuItem("remaininghealth", "危险血量 %")).SetValue(new Slider(15, 0, 100));


            菜单.AddSubMenu(new Menu("              ---监测 反隐---", "WARDD"));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("GrassWard", "              ---进草插眼"));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("ward", "敌人进草自动插眼").SetValue(true));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("wardC", "仅按键使用").SetValue(false));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("Combo", "按键").SetValue(new KeyBind(32, KeyBindType.Press)));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("GrassWard1", "              ---反隐监测"));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("Always", "总是使用").SetValue(false));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("Use", "连招使用").SetValue(new KeyBind(32, KeyBindType.Press)));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("GrassWard12", "              ---反隐监测"));
            菜单.SubMenu("WARDD").AddItem(new MenuItem("ta-wposok", "显示眼位监测").SetValue(true));


            菜单.AddSubMenu(new Menu("              ---打野 助手---", "Jungle"));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Smite", "              ---自动惩戒"));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Fuckyou", "自动惩戒").SetValue(new KeyBind('M', KeyBindType.Toggle)));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Fuckyou1", "自动惩戒对象"));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Fuckyou2", "BUFF,大龙,小龙"));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Jungle1", "              ---无伤打野"));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Show", "开启显示").SetValue(false));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Jungle2", "              ---打野计时"));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Timer", "开启大地图显示").SetValue(true));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("Timer1", "开启小地图显示").SetValue(true));
            菜单.SubMenu("Jungle").AddItem(new MenuItem("JungleTimerFormat", "时间格式:").SetValue(new StringList(new[] { "分:秒", "秒" })));

            菜单.AddSubMenu(new Menu("              ---后摇 设置---", "Movement"));
            菜单.SubMenu("Movement").AddItem(new MenuItem("AA122", "              ---平砍后摇"));
            菜单.SubMenu("Movement").AddItem(new MenuItem("AA", "AA后摇启用")).SetValue(true);
            菜单.SubMenu("Movement").AddItem(new MenuItem("AA1", "让你更自然的AA"));
            菜单.SubMenu("Movement").AddItem(new MenuItem("AA1221", "              ---重置普攻"));
            菜单.SubMenu("Movement").AddItem(new MenuItem("ChampEnabled", "对英雄启用").SetValue(new KeyBind(32, KeyBindType.Press)));
            菜单.SubMenu("Movement").AddItem(new MenuItem("TowerEnabled", "对塔启用").SetValue(new KeyBind(32, KeyBindType.Press)));
            菜单.SubMenu("Movement").AddItem(new MenuItem("Movement", "              ---移动延迟"));
            菜单.SubMenu("Movement").AddItem(new MenuItem("MovementEnabled", "移动延迟启用").SetValue(true));
            菜单.SubMenu("Movement").AddItem(new MenuItem("MovementDelay", "移动延迟")).SetValue(new Slider(80, 0, 400));
            菜单.SubMenu("Movement").AddItem(new MenuItem("Movement1", "让躲避更加人性化"));

            菜单.AddSubMenu(new Menu("              ---其他 设置---", "Others"));
            菜单.SubMenu("Others").AddItem(new MenuItem("PTXY", "              ---炮塔血量"));
            菜单.SubMenu("Others").AddItem(new MenuItem("TIHealth", "炮塔&水晶血量").SetValue(new StringList(new[] { "百分比", "生命值" })));
            菜单.SubMenu("Others").AddItem(new MenuItem("HealthActive", "开启").SetValue(true));
          //  菜单.SubMenu("Others").AddItem(new MenuItem("CD", "              ---技能冷却"));
            菜单.SubMenu("Others").AddItem(new MenuItem("Skins", "              ---换肤功能"));
            菜单.SubMenu("Others").AddItem(new MenuItem("forall", "重置皮肤(需要F5)").SetValue(true));


            菜单.AddSubMenu(new Menu("              ---开发 功能---", "Flowers"));
            菜单.SubMenu("Flowers").AddItem(new MenuItem("Flowers1", "              ---开发功能"));
            菜单.SubMenu("Flowers").AddItem(new MenuItem("ZoomHack", "无限视距").SetValue(false));
            菜单.SubMenu("Flowers").AddItem(new MenuItem("Cast", "禁止技能指示").SetValue(false));
            菜单.SubMenu("Flowers").AddItem(new MenuItem("TowerRanges", "防御塔范围(接近显示)").SetValue(true));
            菜单.SubMenu("Flowers").AddItem(new MenuItem("AntiAFK", "防挂机").SetValue(true));
            菜单.SubMenu("Flowers").AddItem(new MenuItem("msg", "使用无反应请F5一次"));


            菜单.AddSubMenu(new Menu("              ---功能 介绍---", "Lost"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg1", "自动灯笼"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg2", "自动点燃"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg14", "自动中亚"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg3", "进草插眼"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg4", "反隐监测"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg13", "眼位监控"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg5", "自动惩戒"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg6", "无伤打野"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg7", "打野计时"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg8", "平砍后摇"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg9", "重置普攻"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg10", "移动延迟"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg11", "炮塔血量"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg15", "换肤功能"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg16", "无限视距"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg17", "禁止技能指示"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg18", "防御塔范围(接近显示)"));
            菜单.SubMenu("Lost").AddItem(new MenuItem("msg19", "防挂机"));


          //  菜单.SubMenu("Lost").AddItem(new MenuItem("msg12", "技能冷却"));
            菜单.AddItem(new MenuItem("Flowers", "---作者:花边下丶情未央---"));
            菜单.AddItem(new MenuItem("Flowers1", "---QQ  1076751236---"));
            菜单.AddItem(new MenuItem("Flowers12", "---对外QQ群299606556---"));

            菜单.AddToMainMenu();
        }
       
    }
}
