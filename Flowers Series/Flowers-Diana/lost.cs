using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using LeagueSharp.Common.Data;
using Item = LeagueSharp.Common.Items.Item;

namespace FlowersDiana
{
    class lost
    {
        static Spell Q = new Spell(SpellSlot.Q, 830);
        static Spell W = new Spell(SpellSlot.W, 200) { Delay = 250 };
        static Spell E = new Spell(SpellSlot.E, 250) { Delay = 300 };
        static Spell R = new Spell(SpellSlot.R, 825) { Delay = 300 };
        static SkillshotDetector detector;
        static int qTick = 0;
        static int lastRCountRaise = 0;
        static bool canMove = true;
        static int rCount = 0;
        static bool doInstaQ = false;
        static bool inTripleCombo = false;
        static HitChance GetHitchance(string item)
        {
            switch (huabian.菜单.Item(item).GetValue<StringList>().SelectedIndex)
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
                    return HitChance.Medium;
            }
        }
        static bool isActive(string menuItem)
        {
            return huabian.菜单.Item(menuItem).GetValue<KeyBind>().Active;
        }
        static bool isTrue(string menuItem)
        {
            return huabian.菜单.Item(menuItem).GetValue<bool>();
        }
        static float getValue(string menuItem)
        {
            return huabian.菜单.Item(menuItem).GetValue<Slider>().Value;
        }
        static string GetREvadePoint()
        {
            switch (huabian.菜单.Item("rEvadePoint").GetValue<StringList>().SelectedIndex)
            {
                case 0:
                    return "MaxDistFromSkillShot";
                case 1:
                    return "MaxDistFromSender";
                case 2:
                    return "NearestPointFromSkillshotEnd";
                case 3:
                    return "NearestPointFromSelf";
                default:
                    return "MaxDistFromSender";
            }
        }

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
        public const string ChampionName = "Diana";
        public static Orbwalking.Orbwalker Orbwalker;
        public static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Notifications.AddNotification("Flowers Diana by NightMoon", 1000);
            Notifications.AddNotification("Version : 1.0.0.0", 1000);

            huabian.DianaMenu();
            detector = new SkillshotDetector();

            Game.OnUpdate += 总菜单;
            Drawing.OnDraw += 总显示;
            lost.Orbwalker.SetMovement(true);
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
        }

        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (E.IsReady() && huabian.菜单.Item("useE_antigap").GetValue<bool>() &&
                gapcloser.End.Distance(Player.Position) <= E.Range)
                E.Cast();
        }

        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (E.IsReady() && sender.Distance(Player.Position) <= E.Range &&
                huabian.菜单.Item("useE_interrupt").GetValue<bool>())
                E.Cast();
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                if (args.SData.TargettingType == SpellDataTargetType.Unit &&
                    args.Target.IsMe &&
                    huabian.菜单.Item("useWAutoShield").GetValue<bool>())
                    W.Cast();

                return;
            }
            if (lost.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                switch (args.SData.Name)
                {
                    case "DianaArc":
                        qTick = Utils.TickCount;
                        break;
                    case "DianaVortex":
                        if (lost.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear && W.IsReady())
                            W.Cast();
                        break;
                    case "DianaTeleport":
                        if (doInstaQ)
                        {
                            Q.Cast(Q.GetPrediction((Obj_AI_Hero)args.Target).CastPosition);
                            doInstaQ = false;
                        }
                        else if (args.SData.Name == "DianaTeleport")
                        {
                            rCount++;
                            lastRCountRaise = Utils.TickCount;
                            var unit = (Obj_AI_Hero)args.Target;

                            if (!unit.HasBuff("dianamoonlight"))
                                rCount = 0;
                        }
                        break;
                }
            }

            if (args.SData.Name == "DianaTeleport" && E.IsReady() && huabian.菜单.Item("useER").GetValue<bool>())
            {
                var enemies = HeroManager.Enemies.Where(x => x.IsValidTarget());

                foreach (var enemy in enemies)
                {
                    var pPos = Prediction.GetPrediction(enemy, 50 + E.Delay);

                    if (pPos.Hitchance >= GetHitchance("eHitChance") && pPos.UnitPosition.Distance(args.End) <= E.Range)
                    {
                        Utility.DelayAction.Add
                        (50, delegate 
                        {
                            E.Cast();
                        }
                        );
                        break;
                    }
                }
            }

        }

        private static void 总显示(EventArgs args)
        {

            if (Player.IsDead)
            {
                return;
            }
            if (!huabian.菜单.Item("noDraw").GetValue<bool>())
                return;

            var AA范围OKTWStyle = huabian.菜单.Item("drawingAA").GetValue<bool>();
            var AA目标OKTWStyle = huabian.菜单.Item("orb").GetValue<bool>();
            var Q范围 = huabian.菜单.Item("drawingQ").GetValue<Circle>();
            var W范围 = huabian.菜单.Item("drawingW").GetValue<Circle>();
            var E范围 = huabian.菜单.Item("drawingE").GetValue<Circle>();
            var R范围 = huabian.菜单.Item("drawingR").GetValue<Circle>();
            var 补刀小兵 = huabian.菜单.Item("bdxb").GetValue<Circle>();
            var 附近可击杀 = huabian.菜单.Item("fjkjs").GetValue<Circle>();

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

            if (W.IsReady() && W范围.Active)
                Render.Circle.DrawCircle(Player.Position, W.Range, W范围.Color);

            if (E.IsReady() && E范围.Active)
                Render.Circle.DrawCircle(Player.Position, E.Range - 30, E范围.Color);

            if (R.IsReady() && R范围.Active)
                Render.Circle.DrawCircle(Player.Position, R.Range + 50, R范围.Color);

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
        }

        private static void 总菜单(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            SkillshotsResult.DetectedSkillShots.RemoveAll(x => !x.IsActive());
            evade();

            if (!isActive("tripleRKey") && !isActive("tripleRKeyToggle") && Q.IsReady())
                rCount = 0;

            if (!E.IsReady())
                canMove = true;

            if (!isActive("tripleRKey") && !isActive("tripleRKeyToggle") && inTripleCombo)
            {
                inTripleCombo = false;
                rCount = 0;
            }

            lost.Orbwalker.SetMovement(canMove);

            var target = (TargetSelector.GetTarget(1000, TargetSelector.DamageType.Physical) ??
               TargetSelector.GetTarget(1000, TargetSelector.DamageType.Magical)) ?? TargetSelector.GetTarget
               (1000, TargetSelector.DamageType.True);

            bool targetBuff = target != null ? target.HasBuff("dianamoonlight") : false;

            if (lost.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo && target != null)
                checkCombo(target, targetBuff);
            else
            {
                checkOtherModes(target);
            }
        }

        private static void checkOtherModes(Obj_AI_Hero target)
        {
            if (lost.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear ||
                lost.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed ||
                lost.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LastHit)
            {
                if (lost.Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
                {
                    var mobs = MinionManager.GetMinions(
                    ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Neutral,
                        MinionOrderTypes.MaxHealth);

                    if (mobs.Any(x => x.Distance(Player.Position) <= 500))
                        junClear();
                    else
                        clear();
                }

                harass(target);
            }
            else if (isActive("harassKeyToggle"))
                harass(target);
        }

        private static void clear()
        {
            if (Orbwalking.CanAttack())
                return;

            var mins = MinionManager.GetMinions(1000);

            if (mins.Count <= 0)
                return;

            var qPos = Q.GetCircularFarmLocation(mins);
            var ePos = E.GetCircularFarmLocation(mins);

            if (qPos.MinionsHit >= 1 && isTrue("useQ_waveClear") && Q.IsReady())
                Q.Cast(qPos.Position);

            int eMinsHit = mins.Count(x => x.Distance(Player.Position) <= E.Range);

            if (eMinsHit >= getValue("useE_waveClear_minions_count") && isTrue("useEW_waveClear") && E.IsReady() && W.IsReady())
                E.Cast();

            if (W.IsReady() && !E.IsReady() && isTrue("useW_waveClear"))
            {
                if (mins.OrderBy(x => x.Distance(Player.Position)).FirstOrDefault().Distance(Player.Position) <= W.Range)
                    W.Cast();
            }

            if (isTrue("useR_waveClear") && R.IsReady())
            {
                var minions = mins.Where(x => x.Distance(Player.Position) <= R.Range).OrderBy(x => x.Health);

                try
                {
                    foreach (var min in minions)
                    {
                        if (min.HasBuff("dianamoonlight"))
                            R.Cast(min);
                    }
                }
                finally
                {
                    if (Utils.TickCount - qTick >= 500)
                    {
                        if (R.IsReady() && isTrue("useR_waveClearNoBuff"))
                        {
                            var minion = mins.Where(x => x.Distance(Player.Position) <= R.Range).
                                OrderBy(x => x.Health).FirstOrDefault();

                            R.Cast(minion);
                        }
                    }
                }
            }

        }

        private static void junClear()
        {
            if (Orbwalking.CanAttack())
                return;

            var mobs = MinionManager.GetMinions(
                        ObjectManager.Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Neutral,
                            MinionOrderTypes.MaxHealth).Where(x => x.IsValidTarget() && !x.IsDead).OrderBy(x => -x.MaxHealth);

            if (mobs.Count() <= 0)
                return;

            List<Vector2> minsPos = new List<Vector2>();

            foreach (var min in mobs)
            {
                minsPos.Add(min.Position.To2D());
            }

            if (E.IsReady() && isTrue("useEW_junClear") && W.IsReady() && E.Level > 0 && mobs.Count() > 1)
            {
                Vector2 center;
                float temp;

                MEC.FindMinimalBoundingCircle(minsPos, out center, out temp);

                if (Player.Distance(center) > 100)
                {
                    canMove = false;
                    Player.IssueOrder(GameObjectOrder.MoveTo, center.To3D());
                }
                else
                    E.Cast();
            }

            if (W.IsReady() && (!E.IsReady() || mobs.Count() < 2) && isTrue("useW_junClear"))
            {
                if (mobs.OrderBy(x => x.Distance(Player.Position)).FirstOrDefault().Distance(Player.Position) <= W.Range)
                    W.Cast();
            }

            if (isTrue("useQ_junClear"))
                Q.Cast(mobs.FirstOrDefault());


            if (isTrue("useEW_junClear") && !E.IsReady() && isTrue("useW_junClear"))
                W.Cast();
            else if (!isTrue("useEW_junClear") && isTrue("useW_junClear"))
                if (mobs.OrderBy(x => x.Distance(Player.Position)).FirstOrDefault().Distance(Player.Position) <= W.Range &&
                    W.IsReady())
                    W.Cast();


            if (isTrue("useR_junClear") && R.IsReady())
            {
                var minions = mobs.Where(x => x.Distance(Player.Position) <= R.Range).OrderByDescending(x => x.MaxHealth);

                try
                {
                    foreach (var min in minions)
                    {
                        if (min.HasBuff("dianamoonlight"))
                            R.Cast(min);
                    }
                }
                finally
                {
                    if (R.IsReady() && isTrue("useR_junClearNoBuff"))
                    {
                        var minion = mobs.Where(x => x.Distance(Player.Position) <= R.Range).OrderBy
                            (x => x.MaxHealth).FirstOrDefault();

                        R.Cast(minion);
                    }
                }
            }

        }

        private static void harass(Obj_AI_Hero target)
        {
            if (Q.IsReady() && target != null)
            {
                var pPos = Q.GetPrediction(target);
                if (pPos.Hitchance >= HitChance.High && Player.ManaPercent >= getValue("mana_harass"))
                    Q.Cast(pPos.CastPosition);
            }
        }

        private static void checkCombo(Obj_AI_Hero target, bool targetBuff)
        {
            if (R.GetDamage(target) >= target.Health && R.IsReady() &&
                R.CanCast(target) && Player.Distance(target.Position) <= R.Range && isTrue("useR"))
            {
                R.Cast(target);

                if (isTrue("useRQ_misc"))
                    doInstaQ = true;
            }
            else
            {
                if (Q.IsReady())
                {
                    var pPos = Q.GetPrediction(target);

                    if (isActive("tripleRKey") || isActive("tripleRKeyToggle"))
                    {
                        if (pPos.Hitchance >= GetHitchance("qHitChanceTripleR"))
                            Q.Cast(pPos.CastPosition);
                    }
                    else if (pPos.Hitchance >= GetHitchance("qHitChance"))
                        Q.Cast(pPos.CastPosition);
                }

                if (E.IsReady())
                {
                    var pPos = Prediction.GetPrediction(target, E.Delay);

                    if (pPos.Hitchance >= GetHitchance("eHitChance") && pPos.UnitPosition.Distance(Player.Position) <= E.Range)
                        E.Cast();
                }

                if (W.IsReady() && !E.IsReady())
                {
                    var pPos = Prediction.GetPrediction(target, W.Delay);

                    if (pPos.Hitchance >= GetHitchance("wHitChance"))
                        W.Cast();
                }

                if (Player.HealthPercent <= getValue("usew_percent_amount"))
                    W.Cast();

                if (R.IsReady() && Player.Distance(target.Position) <= R.Range && isTrue("useR"))
                {
                    if (!isActive("tripleRKey") && !isActive("tripleRKeyToggle"))
                    {
                        if (targetBuff)
                            R.Cast(target);
                        else if (rCount == 1 || !Q.IsReady())
                            R.Cast(target);
                    }
                    else
                    {
                        inTripleCombo = true;

                        if (rCount == 2)
                            //3rd R
                            R.Cast(target);
                        else if (targetBuff && rCount == 1)
                            R.Cast(target);
                        else if (rCount == 0 && targetBuff && Utils.TickCount - qTick >= 2900
                            && Utils.TickCount - qTick <= 3000)
                            R.Cast(target);
                    }
                }
            }

        }

        private static void evade()
        {
            foreach (var skill in SkillshotsResult.DetectedSkillShots)
            {
                var hitTime = skill.StartTick + skill.SpellData.Delay + skill.SpellData.ExtraDuration +
                                1000 * (skill.Start.Distance(Player.Position) / skill.SpellData.MissileSpeed);

                int timeLeft = (int)(hitTime - Environment.TickCount);

                checkHit(skill, timeLeft);
            }
        }

        private static void checkHit(Skillshot skill, int timeLeft)
        {
            Obj_AI_Hero sender = HeroManager.Enemies.FirstOrDefault(
                        x => x.ChampionName == skill.SpellData.ChampionName);

            var wMana = new[] { 0, 60, 70, 80, 90, 100 };
            var rMana = new[] { 0, 50, 65, 80 };

            if (isTrue("useWAutoShieldSkillShot") &&
                    skill.SpellData.DangerValue >= getValue("useWAutoShieldSkillShot_DangerLvl")
                    && skill.SpellData.DangerValue < 5 && W.IsReady() && W.Level > 0 &&
                    Player.Mana >= wMana[W.Level])
            {
                if (skill.IsAboutToHit(timeLeft, Player) && timeLeft > W.Delay)
                    W.Cast();
            }
            else if (isTrue("useRToEvade") && skill.SpellData.DangerValue >= getValue("useRToEvade_DangerLvl")
                && R.Level > 0 && R.IsReady() && Player.Mana >= rMana[R.Level])
            {
                if (skill.IsAboutToHit(timeLeft, Player) && timeLeft > R.Delay
                    && skill.SpellData.DangerValue >= getValue("useRToEvade_DangerLvl"))
                {
                    var availableMinions = MinionManager.GetMinions(R.Range, MinionTypes.All, MinionTeam.NotAlly);
                    var availableHeroes_Base = HeroManager.Enemies.Where(x => x.IsValidTarget() && !x.IsDead
                            && x.Distance(Player.Position) <= R.Range).Select(x => (Obj_AI_Base)x).ToList();

                    var fleeObjectsTEMP = availableMinions.Count <= 0 && isTrue("useHeroes") ? availableHeroes_Base :
                        availableMinions;

                    fleeObjectsTEMP = (isTrue("heroesOverMinionsBool") || isActive("heroesOverMinionsKeyBind"))
                        && availableHeroes_Base.Count >= 0 ? availableHeroes_Base :
                        fleeObjectsTEMP;

                    List<Obj_AI_Base> fleeObjects = new List<Obj_AI_Base>(); ;

                    foreach (var obj in fleeObjectsTEMP)
                    {
                        var hitTimeForFleeObj = skill.StartTick + skill.SpellData.Delay + skill.SpellData.ExtraDuration +
                                1000 * (skill.Start.Distance(obj.Position) / skill.SpellData.MissileSpeed);

                        int timeLeftForFleeObj = (int)(hitTimeForFleeObj - Environment.TickCount);

                        if (!skill.IsAboutToHit(timeLeftForFleeObj, obj))
                            fleeObjects.Add(obj);
                    }

                    if (fleeObjects.Count() > 0)
                    {
                        Obj_AI_Base bestObject;

                        switch (GetREvadePoint())
                        {
                            case "MaxDistFromSkillShot":
                                bestObject = fleeObjects.OrderBy(x => x.Distance(skill.End)).LastOrDefault();
                                R.Cast(bestObject);

                                break;
                            case "MaxDistFromSender":

                                bestObject = fleeObjects.OrderBy(x => x.Distance(sender.Position)).LastOrDefault();
                                R.Cast(bestObject);

                                break;
                            case "NearestPointFromSkillshotEnd":

                                bestObject = fleeObjects
                                    .OrderBy(x => x.Distance(skill.End)).FirstOrDefault();
                                R.Cast(bestObject);

                                break;
                            case "NearestPointFromSelf":

                                bestObject = fleeObjects
                                    .OrderBy(x => x.Distance(Player.Position)).FirstOrDefault();
                                R.Cast(bestObject);

                                break;
                            default: //"MaxDistFromSender"

                                bestObject = fleeObjects.OrderBy(x => x.Distance(sender.Position)).LastOrDefault();
                                R.Cast(bestObject);

                                break;
                        }
                    }
                }
            }
            else if (isTrue("useWAutoShieldSkillShot") &&
                    skill.SpellData.DangerValue == 5 && W.IsReady() && W.Level > 0 &&
                    Player.Mana >= wMana[W.Level])
            {
                if (skill.IsAboutToHit(timeLeft, Player) && timeLeft > W.Delay)
                    W.Cast();
            }
        }
    }
}
