using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using LeagueSharp.Common.Data;
using Item = LeagueSharp.Common.Items.Item;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;

namespace Flowers___Varus
{
    class lost
    {
        static Spell Q;
        static Spell W;
        static Spell E;
        static Spell R;
        internal static float getManaPer
        {
            get { return Player.Mana / Player.MaxMana * 100; }
        }
        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }
        private static float Hp百分比(Obj_AI_Hero Player)
        {
            return Player.Health * 100 / Player.MaxHealth;
        }
        public const string ChampionName = "Varus";
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

            Notifications.AddNotification("Flowers Varus by NightMoon", 1000);
            Notifications.AddNotification("`                  And  Lost`", 1000);
            Notifications.AddNotification("Version : 1.0.0.0", 1000);
            Game.PrintChat("Flowers-Varus Loaded!~~~ Version : 1.0.0.0 Thanks for your use!");
            Game.PrintChat("What are the deficiencies please immediately feedback. Thank you! ");

            flowers.VarusMenu();

            Q = new Spell(SpellSlot.Q, 1600f);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 925f);
            R = new Spell(SpellSlot.R, 1200f);

            Q.SetSkillshot(.25f, 70f, 1650f, false, SkillshotType.SkillshotLine);
            E.SetSkillshot(.50f, 250f, 1400f, false, SkillshotType.SkillshotCircle);
            R.SetSkillshot(.25f, 120f, 1950f, false, SkillshotType.SkillshotLine);

            Q.SetCharged("VarusQ", "VarusQ", 250, 1600, 1.2f);

            Game.OnUpdate += 主菜单;
            Drawing.OnDraw += 主显示;
            Orbwalking.BeforeAttack += AA前;
            Utility.HpBarDamageIndicator.DamageToUnit = 获取连招伤害;
        }

        private static float 获取连招伤害(Obj_AI_Hero target)
        {
            var fComboDamage = 0f;

            if (Q.IsReady())
                fComboDamage += (float)Player.GetSpellDamage(target, SpellSlot.Q);

            if (W.Level > 0)
                fComboDamage += (float)ObjectManager.Player.GetSpellDamage(target, SpellSlot.W);

            if (E.IsReady())
                fComboDamage += (float)ObjectManager.Player.GetSpellDamage(target, SpellSlot.E);

            if (R.IsReady())
                fComboDamage += (float)ObjectManager.Player.GetSpellDamage(target, SpellSlot.R);

            if (ObjectManager.Player.GetSpellSlot("summonerdot") != SpellSlot.Unknown &&
                ObjectManager.Player.Spellbook.CanUseSpell(ObjectManager.Player.GetSpellSlot("summonerdot")) ==
                SpellState.Ready && Player.Distance(target, true) < 550)
                fComboDamage += (float)ObjectManager.Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite);

            if (Items.CanUseItem(3144) && Player.Distance(target, true) < 550)
                fComboDamage += (float)ObjectManager.Player.GetItemDamage(target, Damage.DamageItems.Bilgewater);

            if (Items.CanUseItem(3153) && Player.Distance(target, true) < 550)
                fComboDamage += (float)ObjectManager.Player.GetItemDamage(target, Damage.DamageItems.Botrk);

            return fComboDamage;
        }

        private static void AA前(Orbwalking.BeforeAttackEventArgs args)
        {
            args.Process = !Q.IsCharging;
        }

        private static HitChance Cast()
        {
            switch (flowers.菜单.Item("qhitChance").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return HitChance.Low;
                case 1:
                    return HitChance.Medium;
                case 2:
                    return HitChance.High;
                case 3:
                    return HitChance.VeryHigh;
                default:
                    return HitChance.VeryHigh;
            }
        }

        private static void 主显示(EventArgs args)
        {
            var AA范围OKTWStyle = flowers.菜单.Item("drawingAA").GetValue<bool>();
            var AA目标OKTWStyle = flowers.菜单.Item("orb").GetValue<bool>();
            var Q范围 = flowers.菜单.Item("drawingQ").GetValue<Circle>();
            var E范围 = flowers.菜单.Item("drawingE").GetValue<Circle>();
            var R范围 = flowers.菜单.Item("drawingR").GetValue<Circle>();
            var 补刀小兵 = flowers.菜单.Item("bdxb").GetValue<Circle>();
            var 附近可击杀 = flowers.菜单.Item("fjkjs").GetValue<Circle>();

            if (AA范围OKTWStyle)
            {
                if (Hp百分比(Player) > 60)
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.GreenYellow, 2);
                else if (Hp百分比(Player) > 30)
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

            if (E.IsReady() && E范围.Active)
                Render.Circle.DrawCircle(Player.Position, E.Range, E范围.Color);

            if (R.IsReady() && R范围.Active)
                Render.Circle.DrawCircle(Player.Position, R.Range, R范围.Color);

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

            if (Game.MapId == (GameMapId)11 && flowers.菜单.Item("wushangdaye").GetValue<bool>())
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

        private static void 主菜单(EventArgs args)
        {
            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    骚扰2();
                    骚扰1();
                    RRR你菊花();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    骚扰1();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    清线();
                    清野();
                    break;
            }
            if (flowers.菜单.Item("AutoHarass").GetValue<KeyBind>().Active)
            {
                骚扰2();
            }

            手动R();
        }

        private static void RRR你菊花()
        {
            if (flowers.菜单.Item("lzr").GetValue<bool>() && Player.CountEnemiesInRange(R.Range) >= flowers.菜单.Item("lzrrr").GetValue<Slider>().Value && R.IsReady())
            {
                R.CastOnBestTarget();
            }
        }

        private static int 堆叠数(Obj_AI_Base target)
        {
            return
                target.Buffs.Where(xBuff => xBuff.Name == "varuswdebuff" && target.IsValidTarget(Q.Range))
                    .Select(xBuff => xBuff.Count)
                    .FirstOrDefault();
        }

        private static void 手动R()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Physical);

            if (R.IsReady() && target.IsValidTarget() && flowers.菜单.Item("shoudongR").GetValue<KeyBind>().Active)
            {
                R.CastOnUnit(target);
            }
        }

        private static void 清野()
        {
            var 清野Q = flowers.菜单.Item("qyq").GetValue<bool>();
            var 清野E = flowers.菜单.Item("qye").GetValue<bool>();

            if (!Orbwalking.CanMove(1) || !(getManaPer > flowers.菜单.Item("qxmp").GetValue<Slider>().Value))
                return;

            var Mobs = MinionManager.GetMinions(Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(Player) + 100, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (Mobs.Count <= 0)
                return;

            foreach (var minion in Mobs)
            {
                if (Q.IsReady() && 清野Q)
                {
                    if (!Q.IsCharging)
                    {
                        Q.StartCharging();
                    }
                    else
                    {
                        Q.CastOnUnit(minion);
                    }
                }

                if (E.IsReady() && 清野E && E.CanCast(Mobs[0]))
                {
                    E.Cast();
                }
            }    
        }

        private static void 清线()
        {
            var 清线Q = flowers.菜单.Item("qxq").GetValue<bool>();
            var 清线E = flowers.菜单.Item("qxe").GetValue<bool>();
            var 清线Q小兵 = flowers.菜单.Item("qxqqq").GetValue<Slider>().Value;
            var 清线E小兵 = flowers.菜单.Item("qxeee").GetValue<Slider>().Value;

            if (!Orbwalking.CanMove(1) || !(getManaPer > flowers.菜单.Item("qxmp").GetValue<Slider>().Value))
                return;

            var Minions = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy);

            if (Minions.Count <= 0)
                return;

            if (Q.IsReady() && 清线Q)
            {
                var allMinions = MinionManager.GetMinions(Player.ServerPosition, Q.Range);
                {
                    foreach (var minion in
                        allMinions.Where(
                            minion => minion.Health <= Player.GetSpellDamage(minion, SpellSlot.Q)))
                    {

                        var killcount = 0;

                        foreach (var colminion in Minions)
                        {
                            if (colminion.Health <= Q.GetDamage(colminion))
                            {
                                killcount++;
                            }
                            else
                            {
                                break;
                            }
                        }

                        if (killcount >= 清线Q小兵)
                        {
                            Q.Cast(minion.ServerPosition);
                            return;
                        }
                    }
                }
            }

            if (!清线E || !E.IsReady())
                return;

            var minionkillcount =
                Minions.Count(x => E.CanCast(x) && x.Health <= E.GetDamage(x));

            if (minionkillcount >= 清线E小兵)
            {
                foreach (var minion in Minions.Where(x => x.Health <= E.GetDamage(x)))
                {
                    E.Cast(minion);
                }
            }
        }

        private static void 骚扰1()
        {

            var 骚扰Q = flowers.菜单.Item("srq").GetValue<bool>();
            var 骚扰E = flowers.菜单.Item("sre").GetValue<bool>();
            var target = TargetSelector.GetTarget(Q.ChargedMaxRange, TargetSelector.DamageType.Physical);


            if (骚扰E && E.IsReady())
            {
                E.Cast(target);
            }

            if (Q.IsReady() && 骚扰Q)
            {
                if (Q.IsCharging)
                {
                    var prediction = Q.GetPrediction(target);
                    var distance = Player.ServerPosition.Distance(prediction.UnitPosition + 200 * (prediction.UnitPosition - Player.ServerPosition).Normalized(), true);
                    if (distance < Q.RangeSqr)
                    {
                        if (Q.Cast(prediction.CastPosition))
                            return;
                    }
                }
                else
                {
                    if (Player.AttackRange + 180 > Player.Distance(target, true))
                    {
                        if (W.Level == 0 || 堆叠数(target) >= 2 || Q.GetDamage(target) > target.Health)
                            Q.StartCharging();
                    }
                    else
                    {
                        Q.StartCharging();
                    }
                }
            }
        }

        private static void 骚扰2()
        {

            if (E.IsReady())
            {
                if (ObjectManager.Player.HasBuff("Recall"))
                    return;
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (target != null)
                {
                    E.Cast(target, false, true);
                }
            }
        }

    }
}
