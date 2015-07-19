#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using Collision = LeagueSharp.Common.Collision;
#endregion

namespace 模版
{
    class Program
    {
        public const string ChampionName = "ChampionName";//英雄名字得改
        public static float 蓝量比 = Player.Mana / Player.MaxMana * 100;
        private static Obj_AI_Hero Player = ObjectManager.Player;
        private static Menu 菜单;
        internal static Orbwalking.Orbwalker 走砍;
        private static Spell Q;
        private static Spell W;
        private static Spell E;
        private static Spell R;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R);

            菜单 = new Menu("Flowers-ChampionName", "ChampionName", true);//英雄名字得改

            var targetSelectorMenu = new Menu("目标 选择", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            走砍 = new Orbwalking.Orbwalker(菜单.AddSubMenu(new Menu("走砍 设置", "Orbwalker")));

            菜单.AddSubMenu(new Menu("连招 设置", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzq", "使用 Q").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzw", "使用 W").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzr", "使用 R").SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzmp", "连招丨最低蓝量比").SetValue(new Slider(30, 0, 100)));

            菜单.AddSubMenu(new Menu("骚扰 设置", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "使用 Q").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srw", "使用 W").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("sre", "使用 E").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srr", "使用 R").SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srmp", "骚扰丨最低蓝量比").SetValue(new Slider(40, 0, 100)));

            菜单.AddSubMenu(new Menu("清线 清野", "Clean"));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qxq", "清线时丨使用 Q").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qxw", "清线时丨使用 W").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qxe", "清线时丨使用 E").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qxr", "清线时丨使用 R").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("---", "--------------"));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qyq", "清野时丨使用 Q").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qyw", "清野时丨使用 W").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qye", "清野时丨使用 E").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qyr", "清野时丨使用 R").SetValue(true));
            菜单.SubMenu("Clean").AddItem(new MenuItem("qxqymp", "清线清野时丨最低蓝量比", true).SetValue(new Slider(40, 0, 100)));

            菜单.AddSubMenu(new Menu("其他 设置", "Others"));
            菜单.SubMenu("Others").AddItem(new MenuItem("1212121", "121212").SetValue(true));
            菜单.SubMenu("Others").AddItem(new MenuItem("1212122", "121212").SetValue(true));
            菜单.SubMenu("Others").AddItem(new MenuItem("1212123", "121212").SetValue(true));
            菜单.SubMenu("Others").AddItem(new MenuItem("1212124", "121212").SetValue(new KeyBind('T', KeyBindType.Press)));

            菜单.AddSubMenu(new Menu("范围 显示", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "显示 AA 范围").SetValue(new Circle(false, Color.FromArgb(0, 230, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "显示 Q 范围").SetValue(new Circle(false, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "显示 W 范围").SetValue(new Circle(false, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "显示 E 范围").SetValue(new Circle(false, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "显示 R 范围").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("AAmb", "AA 目标").SetValue(false));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("bdxb", "可补刀小兵").SetValue(new Circle(false, Color.GreenYellow)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("fjkjs", "附近可击杀小兵").SetValue(new Circle(false, Color.Gray)));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("wushangdaye", "无伤打野点").SetValue(false));

            菜单.AddItem(new MenuItem("Version", "版本 : 0.0.0.1"));

            菜单.AddToMainMenu();
            Drawing.OnDraw += 范围显示;
            Game.OnUpdate += 主菜单;
            Game.PrintChat("Flowers - ChampionName Loaded!~~~ Version : 0.0.0.1 Thanks for your use!");
            Notifications.AddNotification("Flowers - ChampionName Loaded!", 1000);
        }

        private static void 范围显示(EventArgs args)
        {
            if (Player.IsDead)
                return;

            var AA范围 = 菜单.Item("drawingAA").GetValue<Circle>();
            var Q范围 = 菜单.Item("drawingQ").GetValue<Circle>();
            var W范围 = 菜单.Item("drawingW").GetValue<Circle>();
            var E范围 = 菜单.Item("drawingE").GetValue<Circle>();
            var R范围 = 菜单.Item("drawingR").GetValue<Circle>();
            var 可补刀小兵范围 = Program.菜单.Item("bdxb").GetValue<Circle>();
            var 附近可击杀小兵范围 = Program.菜单.Item("fjkjs").GetValue<Circle>();
            var 无伤打野点 = Program.菜单.Item("wushangdaye").GetValue<Circle>();
            var AA目标 = Program.菜单.Item("AAmb").GetValue<Boolean>();


            if (AA范围.Active)
                Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), AA范围.Color);

            if (Q.IsReady() && Q范围.Active)
                Render.Circle.DrawCircle(Player.Position, Q.Range, Q范围.Color);

            if (W.IsReady() && W范围.Active)
                Render.Circle.DrawCircle(Player.Position, W.Range, W范围.Color);

            if (E.IsReady() && E范围.Active)
                Render.Circle.DrawCircle(Player.Position, E.Range, E范围.Color);

            if (R.IsReady() && R范围.Active)
                Render.Circle.DrawCircle(Player.Position, R.Range, R范围.Color);

            if (可补刀小兵范围.Active || 附近可击杀小兵范围.Active)
            {
                var xMinions =
                    MinionManager.GetMinions(Player.Position, Player.AttackRange + Player.BoundingRadius + 300, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);

                foreach (var xMinion in xMinions)
                {
                    if (可补刀小兵范围.Active && Player.GetAutoAttackDamage(xMinion, true) >= xMinion.Health)
                        Render.Circle.DrawCircle(xMinion.Position, xMinion.BoundingRadius, 可补刀小兵范围.Color, 5);
                    else if (附近可击杀小兵范围.Active && Player.GetAutoAttackDamage(xMinion, true) * 2 >= xMinion.Health)
                        Render.Circle.DrawCircle(xMinion.Position, xMinion.BoundingRadius, 附近可击杀小兵范围.Color, 5);
                }
            }

            if (Game.MapId == (GameMapId)11 && AA范围.Active)
            {
                const float circleRange = 100f;

                Render.Circle.DrawCircle(new Vector3(7461.018f, 3253.575f, 52.57141f), circleRange, Color.Blue, 5); // 蓝色方:红
                Render.Circle.DrawCircle(new Vector3(3511.601f, 8745.617f, 52.57141f), circleRange, Color.Blue, 5); // 蓝色方:蓝
                Render.Circle.DrawCircle(new Vector3(7462.053f, 2489.813f, 52.57141f), circleRange, Color.Blue, 5); // 蓝色方:魔像
                Render.Circle.DrawCircle(new Vector3(3144.897f, 7106.449f, 51.89026f), circleRange, Color.Blue, 5); // 蓝色方:狼
                Render.Circle.DrawCircle(new Vector3(7770.341f, 5061.238f, 49.26587f), circleRange, Color.Blue, 5); // 蓝色方:F4
                Render.Circle.DrawCircle(new Vector3(10930.93f, 5405.83f, -68.72192f), circleRange, Color.Yellow, 5); // 小龙
                Render.Circle.DrawCircle(new Vector3(7326.056f, 11643.01f, 50.21985f), circleRange, Color.Red, 5); // 红色方:红
                Render.Circle.DrawCircle(new Vector3(11417.6f, 6216.028f, 51.00244f), circleRange, Color.Red, 5); // 红色方:蓝
                Render.Circle.DrawCircle(new Vector3(7368.408f, 12488.37f, 56.47668f), circleRange, Color.Red, 5); // 红色方:魔像
                Render.Circle.DrawCircle(new Vector3(10342.77f, 8896.083f, 51.72742f), circleRange, Color.Red, 5); // 红色方:狼
                Render.Circle.DrawCircle(new Vector3(7001.741f, 9915.717f, 54.02466f), circleRange, Color.Red, 5); // 红色方:F4                    
            }

            if (AA目标)
            {
                var target = 走砍.GetTarget();

                if (target != null)
                    Render.Circle.DrawCircle(target.Position, target.BoundingRadius + 15, Color.Red, 6);
            }
        }

        private static void 主菜单(EventArgs args)
        {
            if (Player.IsDead)
                return;

            switch (走砍.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    连招();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    骚扰();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    清线清野();
                    break;
            }

        }

        private static void 连招()
        {
            if (!Orbwalking.CanMove(1) || !(蓝量比 > 菜单.Item("lzmp").GetValue<Slider>().Value))
                return;
            //自己添加 
        }

        private static void 骚扰()
        {
            if (!Orbwalking.CanMove(2) || !(蓝量比 > 菜单.Item("srmp").GetValue<Slider>().Value))
                return;
            //自己添加 
        }

        private static void 清线清野()
        {
            if (!Orbwalking.CanMove(1) || !(蓝量比 > 菜单.Item("qxqymp").GetValue<Slider>().Value))
                return;
            //自己添加 
        }

    }
}
