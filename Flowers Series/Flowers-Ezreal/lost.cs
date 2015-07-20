using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using LeagueSharp.Common.Data;
using Item = LeagueSharp.Common.Items.Item;
using SharpDX;

namespace Flowers_Ezreal
{
    class lost
    {
        static Spell Q;
        static Spell W;
        static Spell E;
        static Spell R;

        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }

        public static bool UsePackets
        {
            get { return Ezreal.Lost.Item("packets").GetValue<bool>(); } 
        }
        private static float Mp(Obj_AI_Hero Player)
        {
            return Player.Mana / Player.MaxMana * 100; 
        }

        private static float Hp(Obj_AI_Hero Player)
        {
            return Player.Health / Player.MaxHealth * 100;
        }

        public const string ChampionName = "Ezreal";

        public static Orbwalking.Orbwalker Orbwalker;

        public static readonly Item 弯刀 = ItemData.Bilgewater_Cutlass.GetItem();
        public static readonly Item 破败 = ItemData.Blade_of_the_Ruined_King.GetItem();
        public static readonly Item 幽梦 = ItemData.Youmuus_Ghostblade.GetItem();

        public static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Q = new Spell(SpellSlot.Q, 1150f);
            W = new Spell(SpellSlot.W, 1000f);
            E = new Spell(SpellSlot.E, 475f);
            R = new Spell(SpellSlot.R);

            Q.SetSkillshot(0.25f, 60f, 2000f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.25f, 80f, 1600f, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(1f, 160f, 2000f, false, SkillshotType.SkillshotLine);

            Notifications.AddNotification("Flowers Ezreal by NightMoon", 1000);
            Notifications.AddNotification("Version : 1.0.0.0", 1000);
            Game.PrintChat("Flowers-Ezreal Loaded!~~~ Version : 1.0.0.0 Thanks for your use!");

            Ezreal.EzrealMenu();

            Game.OnUpdate += 总菜单;
            Drawing.OnDraw += 范围显示;
        }

        private static void 范围显示(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (Ezreal.Lost.Item("closeD").GetValue<bool>())
            {
                return;
            }

            var AA范围OKTWStyle = Ezreal.Lost.Item("drawingAA").GetValue<bool>();
            var AA目标OKTWStyle = Ezreal.Lost.Item("orb").GetValue<bool>();
            var Q范围 = Ezreal.Lost.Item("drawingQ").GetValue<Circle>();
            var W范围 = Ezreal.Lost.Item("drawingW").GetValue<Circle>();
            var E范围 = Ezreal.Lost.Item("drawingE").GetValue<Circle>();
            var 补刀小兵 = Ezreal.Lost.Item("bdxb").GetValue<Circle>();
            var 附近可击杀 = Ezreal.Lost.Item("fjkjs").GetValue<Circle>();
            var 无伤打野 = Ezreal.Lost.Item("wushangdaye").GetValue<bool>();

            if (AA范围OKTWStyle)
            {
                if (Hp(Player) > 60)
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.GreenYellow, 2);
                else if (Hp(Player) > 30)
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.Orange, 3);
                else
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.Red, 4);
            }

            if (AA目标OKTWStyle)
            {
                var orbT = Orbwalker.GetTarget();

                if (orbT.IsValidTarget())
                {
                    if (orbT.Health > orbT.MaxHealth * 0.6)
                        Render.Circle.DrawCircle(orbT.Position, orbT.BoundingRadius + 15, System.Drawing.Color.GreenYellow, 5);
                    else if (orbT.Health > orbT.MaxHealth * 0.3)
                        Render.Circle.DrawCircle(orbT.Position, orbT.BoundingRadius + 15, System.Drawing.Color.Orange, 5);
                    else
                        Render.Circle.DrawCircle(orbT.Position, orbT.BoundingRadius + 15, System.Drawing.Color.Red, 5);
                }
            }

            if (Q.IsReady() && Q范围.Active)
                Render.Circle.DrawCircle(Player.Position, Q.Range, Q范围.Color);

            if (W.IsReady() && W范围.Active)
                Render.Circle.DrawCircle(Player.Position, W.Range, W范围.Color);

            if (E.IsReady() && E范围.Active)
                Render.Circle.DrawCircle(Player.Position, E.Range, E范围.Color);

            if (补刀小兵.Active || 附近可击杀.Active)
            {
                var xMinions = MinionManager.GetMinions(Player.Position, Player.AttackRange + Player.BoundingRadius + 300, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth);

                foreach (var xMinion in xMinions)
                {
                    if (补刀小兵.Active && Player.GetAutoAttackDamage(xMinion) >= xMinion.Health)
                        Render.Circle.DrawCircle(xMinion.Position, xMinion.BoundingRadius, 补刀小兵.Color, 5);
                    else if (附近可击杀.Active && Player.GetAutoAttackDamage(xMinion) * 2 >= xMinion.Health)
                        Render.Circle.DrawCircle(xMinion.Position, xMinion.BoundingRadius, 附近可击杀.Color, 5);
                }
            }

            if (Game.MapId == (GameMapId)11 && 无伤打野)
            {
                const float circleRange = 100f;

                Render.Circle.DrawCircle(new Vector3(7461.018f, 3253.575f, 52.57141f), circleRange, System.Drawing.Color.Orange, 3); // blue team :red
                Render.Circle.DrawCircle(new Vector3(3511.601f, 8745.617f, 52.57141f), circleRange, System.Drawing.Color.Orange, 3); // blue team :blue
                Render.Circle.DrawCircle(new Vector3(7462.053f, 2489.813f, 52.57141f), circleRange, System.Drawing.Color.Orange, 3); // blue team :golems
                Render.Circle.DrawCircle(new Vector3(3144.897f, 7106.449f, 51.89026f), circleRange, System.Drawing.Color.Orange, 3); // blue team :wolfs
                Render.Circle.DrawCircle(new Vector3(7770.341f, 5061.238f, 49.26587f), circleRange, System.Drawing.Color.Orange, 3); // blue team :wariaths
                Render.Circle.DrawCircle(new Vector3(10930.93f, 5405.83f, -68.72192f), circleRange, System.Drawing.Color.Red, 3); // Dragon
                Render.Circle.DrawCircle(new Vector3(7326.056f, 11643.01f, 50.21985f), circleRange, System.Drawing.Color.Orange, 3); // red team :red
                Render.Circle.DrawCircle(new Vector3(11417.6f, 6216.028f, 51.00244f), circleRange, System.Drawing.Color.Orange, 3); // red team :blue
                Render.Circle.DrawCircle(new Vector3(7368.408f, 12488.37f, 56.47668f), circleRange, System.Drawing.Color.Orange, 3); // red team :golems
                Render.Circle.DrawCircle(new Vector3(10342.77f, 8896.083f, 51.72742f), circleRange, System.Drawing.Color.Orange, 3); // red team :wolfs
                Render.Circle.DrawCircle(new Vector3(7001.741f, 9915.717f, 54.02466f), circleRange, System.Drawing.Color.Orange, 3); // red team :wariaths                    
            }

        }

        public static bool UseBotrk(Obj_AI_Hero target)
        {
            var 使用弯刀 = Ezreal.Lost.Item("UseBC").GetValue<bool>();
            var 使用破败 = Ezreal.Lost.Item("UseBRK").GetValue<bool>();

            if (使用破败 && 破败.IsReady() && target.IsValidTarget(破败.Range) &&
                Player.Health + Player.GetItemDamage(target, Damage.DamageItems.Botrk) < Player.MaxHealth)
            {
                return 破败.Cast(target);
            }
            else if (使用弯刀 && 弯刀.IsReady() && target.IsValidTarget(弯刀.Range))
            {
                return 弯刀.Cast(target);
            }
            return false;
        }

        public static bool UseYoumuu(Obj_AI_Base target)
        {
            var 使用幽梦 = Ezreal.Lost.Item("USEYUU").GetValue<bool>();

            if (使用幽梦 && 幽梦.IsReady() && target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Player) + 50))
            {
                return 幽梦.Cast();
            }
            return false;
        }

        static Boolean ExtraCheckForFarm(Obj_AI_Base minion)
        {
            if (minion == null)
                return false;

            if (minion.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Player) + minion.BoundingRadius))
            {
                if (minion.Health <= Player.GetAutoAttackDamage(minion, true))
                    return false;
                else
                    return true;
            }
            else
                return true;
        }

        private static void 总菜单(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    连招();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    自动Q();
                    骚扰();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    清线();
                    清野();
                    break;
            }

            if (Ezreal.Lost.Item("AutoQ", true).GetValue<KeyBind>().Active)
            {
                自动Q();
            }

            if (Ezreal.Lost.Item("eee", true).GetValue<KeyBind>().Active)
            {
                自动E();
            }
            if (Ezreal.Lost.Item("shoudongr", true).GetValue<KeyBind>().Active)
            {
                自动R();
            }
            技能命中();
        }

        private static void 技能命中()
        {
            if (Ezreal.Lost.Item("Hit", true).GetValue<Boolean>())
            {
                Q.MinHitChance = HitChance.VeryHigh;
                R.MinHitChance = HitChance.VeryHigh;
            }
            else
            {
                Q.MinHitChance = HitChance.High;
                R.MinHitChance = HitChance.High;
            }
        }


        private static void 连招()
        {
            if (!Orbwalking.CanMove(1))
                return;

            if (Q.IsReady() && Ezreal.Lost.Item("lzq", true).GetValue<Boolean>())
            {
                var t = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    Q.Cast(t);
            }

            if (W.IsReady() && Ezreal.Lost.Item("lzw", true).GetValue<Boolean>())
            {
                var t = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical);
                if (t != null)
                    W.Cast(t);
            }

            if (R.IsReady() && Ezreal.Lost.Item("lzr", true).GetValue<Boolean>())
            {
                自动R();
            }
        }

        private static void 自动Q()
        {
            if (!Orbwalking.CanMove(1) || Player.HasBuff("Recall"))
                return;

            if (Q.IsReady() && Ezreal.Lost.Item("AutoQ", true).GetValue<Boolean>())
            {
                var Qtarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical, true);

                if (Q.CanCast(Qtarget))
                    Q.Cast(Qtarget);
            }
        }

        private static void 骚扰()
        {
            if (!Orbwalking.CanMove(1) || !(Mp(Player) > Ezreal.Lost.Item("srmp", true).GetValue<Slider>().Value) || Player.HasBuff("Recall"))
                return;

            if (Q.IsReady() && Ezreal.Lost.Item("srq", true).GetValue<Boolean>())
            {
                var Qtarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical, true);

                if (Q.CanCast(Qtarget))
                    Q.Cast(Qtarget);
            }

            if (W.IsReady() && Ezreal.Lost.Item("srw", true).GetValue<Boolean>())
            {
                var Wtarget = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Physical, true);

                if (W.CanCast(Wtarget) && W.GetPrediction(Wtarget).Hitchance >= HitChance.VeryHigh)
                    W.Cast(Wtarget);
            }
        }

        private static void 清线()
        {
            if (!Orbwalking.CanMove(1) || !(Mp(Player) > Ezreal.Lost.Item("qxmp", true).GetValue<Slider>().Value))
                return;

            var Minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy);

            if (Minions.Count <= 0)
                return;

            if (Q.IsReady() && Ezreal.Lost.Item("qxq", true).GetValue<Boolean>())
            {
                var qtarget = Minions.Where(x => Q.CanCast(x) && Q.GetPrediction(x).Hitchance >= HitChance.Medium && ExtraCheckForFarm(x) && x.Health <= Q.GetDamage(x)).OrderByDescending(x => x.Health).FirstOrDefault();

                if (Q.CanCast(qtarget))
                    Q.Cast(qtarget);
            }
        }

        private static void 清野()
        {
            if (!Orbwalking.CanMove(1) || !(Mp(Player) > Ezreal.Lost.Item("qxmp", true).GetValue<Slider>().Value))
                return;

            var Mobs = MinionManager.GetMinions(Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(Player) + 100, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (Mobs.Count <= 0)
                return;

            if (Q.IsReady() && Ezreal.Lost.Item("qyq", true).GetValue<Boolean>())
            {
                var qtarget = Mobs.Where(x => Q.CanCast(x) && Q.GetPrediction(x).Hitchance >= HitChance.Medium && ExtraCheckForFarm(x)).OrderBy(x => x.Health).FirstOrDefault();

                if (Q.CanCast(qtarget))
                    Q.Cast(qtarget);
            }
        }

        private static void 自动E()
        {
            if (E.IsReady())
            {
                E.Cast(Game.CursorPos, true);
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
        }

        private static void 自动R()
        {
            if (R.IsReady())
            {
                var minRange = Ezreal.Lost.Item("minR").GetValue<Slider>().Value;

                var target = ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(minRange) && hero.IsEnemy).OrderBy(hero => hero.Health).FirstOrDefault();
                R.Cast(target, UsePackets);
            }
        }

    }
}
