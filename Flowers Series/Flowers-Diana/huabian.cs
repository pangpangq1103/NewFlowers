using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;

namespace FlowersDiana
{
    class huabian
    {
        public static Menu 菜单;
        public static void DianaMenu()
        {
            菜单 = new Menu("Flowers-Diana", "Lost.", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            菜单.AddSubMenu(orbwalkerMenu);

            菜单.AddSubMenu(new Menu("Combo", "combo"));

            菜单.SubMenu("combo").AddSubMenu(new Menu("Q", "q"));
            菜单.SubMenu("combo").SubMenu("q").AddItem(new MenuItem("useQ", "Use Q")).SetValue(true);
            菜单.SubMenu("combo").SubMenu("q").AddItem(new MenuItem("qHitChance", "Hitchance Q").
                SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 2)));

            菜单.SubMenu("combo").AddSubMenu(new Menu("W", "w"));
            菜单.SubMenu("combo").SubMenu("w").AddItem(new MenuItem("useW", "Use W")).SetValue(true);
            菜单.SubMenu("combo").SubMenu("w").AddItem(new MenuItem("wHitChance", "W movement prediction hitchance").
                SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 2)));
            菜单.SubMenu("combo").SubMenu("w").AddItem(new MenuItem("usew_percent_combo", "Use W if % hp")).SetValue(true);
            菜单.SubMenu("combo").SubMenu("w").AddItem(new MenuItem("usew_percent_amount", "")).SetValue(new Slider(20));

            菜单.SubMenu("combo").AddSubMenu(new Menu("E", "e"));
            菜单.SubMenu("combo").SubMenu("e").AddItem(new MenuItem("useE", "Use E")).SetValue(true);
            菜单.SubMenu("combo").SubMenu("e").AddItem(new MenuItem("useER", "Use E while R")).SetValue(true);
            菜单.SubMenu("combo").SubMenu("e").AddItem(new MenuItem("eHitChance", "E movement prediction hitchance").
                SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 2)));

            菜单.SubMenu("combo").AddSubMenu(new Menu("R", "r"));
            菜单.SubMenu("combo").SubMenu("r").AddItem(new MenuItem("useR", "Use R")).SetValue(true);
            菜单.SubMenu("combo").SubMenu("r").AddItem(new MenuItem("useRQ_misc", "Use instant R -> Q")).SetValue(true);
            菜单.SubMenu("combo").SubMenu("r").AddSubMenu(new Menu("Triple R", "tripleR"));
            菜单.SubMenu("combo").SubMenu("r").SubMenu("tripleR").AddItem(new MenuItem("qHitChanceTripleR", "Q hitchance while Triple combo").
                SetValue(new StringList(new[] { "Low", "Medium", "High", "Very High" }, 1)));
            菜单.SubMenu("combo").SubMenu("r").SubMenu("tripleR").AddItem(new MenuItem("tripleRKey", "Do Triple-R")).
                SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Press));
            菜单.SubMenu("combo").SubMenu("r").SubMenu("tripleR").AddItem(new MenuItem("tripleRKeyToggle", "Do Triple-R Toggle")).
                SetValue(new KeyBind("X".ToCharArray()[0], KeyBindType.Toggle));


            菜单.AddSubMenu(new Menu("Harass", "harass"));
            菜单.SubMenu("harass").AddItem(new MenuItem("useQ_harass", "Use Q")).SetValue(true);
            菜单.SubMenu("harass").AddItem(new MenuItem("mana_harass", "Use only if % mana")).SetValue(new Slider(50));
            菜单.SubMenu("harass").AddItem(new MenuItem("info1H", "-----"));
            菜单.SubMenu("harass").AddItem(new MenuItem("harassKeyToggle", "Toggle Harass")).
                SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Toggle));

            菜单.AddSubMenu(new Menu("LaneClear", "waveClear"));

            菜单.SubMenu("waveClear").AddItem(new MenuItem("useQ_waveClear", "Use Q")).SetValue(true);
            菜单.SubMenu("waveClear").AddItem(new MenuItem("useW_waveClear", "Use W")).SetValue(true);

            菜单.SubMenu("waveClear").AddSubMenu(new Menu("E", "eWC"));
            菜单.SubMenu("waveClear").SubMenu("eWC").AddItem(new MenuItem("useEW_waveClear", "Use E -> W")).SetValue(true);
            菜单.SubMenu("waveClear").SubMenu("eWC").AddItem(new MenuItem("useE_waveClear_minions_count", "E at least X minions")).
                SetValue(new Slider(3, 1, 5));
            菜单.SubMenu("waveClear").AddItem(new MenuItem("useR_waveClear", "Use R")).SetValue(true);
            菜单.SubMenu("waveClear").AddItem(new MenuItem("useR_waveClearNoBuff", "Use R no moon light")).SetValue(false);

            菜单.AddSubMenu(new Menu("JungleClear", "junClear"));
            菜单.SubMenu("junClear").AddItem(new MenuItem("useQ_junClear", "Use Q")).SetValue(true);
            菜单.SubMenu("junClear").AddItem(new MenuItem("useW_junClear", "Use W")).SetValue(true);
            菜单.SubMenu("junClear").AddItem(new MenuItem("useEW_junClear", "Use E -> W")).SetValue(true);
            菜单.SubMenu("junClear").AddItem(new MenuItem("useR_junClear", "Use R")).SetValue(true);
            菜单.SubMenu("junClear").AddItem(new MenuItem("useR_junClearNoBuff", "Use R no moon light")).SetValue(false);

            菜单.AddSubMenu(new Menu("Interrupt", "interrupt"));
            菜单.SubMenu("interrupt").AddItem(new MenuItem("useE_interrupt", "Use E")).SetValue(true);

            菜单.AddSubMenu(new Menu("Anti-GapCloser", "antiGap"));
            菜单.SubMenu("antiGap").AddItem(new MenuItem("useE_antigap", "Use E")).SetValue(true);

            菜单.AddSubMenu(new Menu("Misc", "misc"));

            菜单.SubMenu("misc").AddSubMenu(new Menu("W", "w"));
            菜单.SubMenu("misc").SubMenu("w").AddItem(new MenuItem("useWAutoShield", "Use W at targeted spells"))
                .SetValue(true);
            菜单.SubMenu("misc").SubMenu("w").AddItem(new MenuItem("useWAutoShieldSkillShot", "Use W at skillshots"))
                .SetValue(true);
            菜单.SubMenu("misc").SubMenu("w").AddItem(new MenuItem("useWAutoShieldSkillShot_DangerLvl",
                "Min skillshot danger level")).SetValue(new Slider(3, 1, 5));

            菜单.SubMenu("misc").AddSubMenu(new Menu("R", "r"));

            菜单.SubMenu("misc").SubMenu("r").AddItem(new MenuItem("useRToEvade",
                "Use R to evade skillshots")).SetValue(true);
            菜单.SubMenu("misc").SubMenu("r").AddItem(new MenuItem("useRToEvade_DangerLvl",
                "Min skillshot danger level")).SetValue(new Slider(5, 1, 5));
            菜单.SubMenu("misc").SubMenu("r").AddItem(new MenuItem("useHeroes", "Use Heroes if no minions")).SetValue(true);
            菜单.SubMenu("misc").SubMenu("r").AddItem(new MenuItem("heroesOverMinionsBool", "Prioritize heroes over minions"))
                .SetValue(false);
            菜单.SubMenu("misc").SubMenu("r").AddItem(new MenuItem("rEvadePoint", "R Evade point").
                SetValue(new StringList(new[] { "MaxDistFromSkillShot", 
                    "MaxDistFromSender", "NearestPointFromSkillshotEnd", "NearestPointFromSelf" }, 1)));
            菜单.SubMenu("misc").SubMenu("r").AddItem(new MenuItem("extraEvadeDist", "Extra evade dist"))
                .SetValue(new Slider(150, 0, 500));
            菜单.SubMenu("misc").SubMenu("r").AddItem(new MenuItem("heroesOverMinionsKeyBind", "Prioritize heroes over minions"))
                .SetValue(new KeyBind("L".ToCharArray()[0], KeyBindType.Press));


            菜单.AddSubMenu(new Menu("Drawings", "Drawing"));
            菜单.SubMenu("drawing").AddItem(new MenuItem("noDraw", "Turn drawings off")).SetValue(false);
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "W Range").SetValue(new Circle(false, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "Draw Minion LastHit").SetValue(new Circle(true, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "Draw Minion Near Kill").SetValue(new Circle(true, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "Real AA Range(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));

            菜单.AddToMainMenu();
        }
    }
}
