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

namespace Riven
{
    class burstCombo
    {
        public static bool canq = true;
        public static bool canaa = true;
        public static bool canItem = true;
        public static bool canw = true;
        public static bool cane = true;

        public static int lastRFire = 0;
        public static bool canr = true;

        public static class times
        {
            public static int q = 0;
            public static int w = 0;
            public static int e = 0;
            public static int r = 0;
            public static int item = 0;
            public static int aa = 0;
        }
    }

    class timeMeasurands
    {
        public static int lastUltCastTime_ME = 0;
    }
    class lost
    {
        static Spell Q = new Spell(SpellSlot.Q, 220);
        static Spell W = new Spell(SpellSlot.W);
        static Spell E = new Spell(SpellSlot.E, 270f);
        static Spell R = new Spell(SpellSlot.R);
        static Spell Flash = new Spell(Player.GetSpellSlot("SummonerFlash"), 425);
        static Vector2 pos;
        static Vector2 wallStartPos;
        static Vector2 endPos;
        static Vector2 lastWallStartPos;
        static Vector2 lastEndPos;
        static System.Drawing.Color lastColor;
        static Vector2 lastWallJumpStartPos;
        static Vector2 lastWallJumpEndPos;
        static System.Drawing.Color drawColor;
        static int qCount = 0;
        static Vector2 pos2;
        static Vector2 pos3;
        static Vector2 wardJumpPos;
        static Obj_AI_Hero target;
        static int tick = 0;
        static int wallJumpTick = 0;

        private static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }
        private static float Hp百分比(Obj_AI_Hero Player)
        {
            return Player.Health * 100 / Player.MaxHealth;
        }
        public const string ChampionName = "Riven";
        public static Orbwalking.Orbwalker Orbwalker;
        public static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Notifications.AddNotification("Flowers Riven by NightMoon", 1000);
            Notifications.AddNotification("Version : 1.0.0.0", 1000);

            huabian.RivenMenu();

            Game.OnUpdate += 总菜单;
            Drawing.OnDraw += 总显示;
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;

        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
                return;

            if (huabian.菜单.Item("burstComboExpanded").GetValue<KeyBind>().Active)
            {
                switch (args.SData.Name)
                {
                    case "RivenMartyr":
                        burstCombo.canw = false;
                        burstCombo.times.w = Utils.GameTimeTickCount;
                        break;
                    case "rivenizunablade":
                        burstCombo.canr = false;
                        break;
                    case "RivenTriCleave":
                        burstCombo.canq = false;
                        break;
                    case "ItemTiamatCleave":
                        burstCombo.canItem = false;
                        break;
                }
            }

            if (args.SData.Name.Contains("Attack"))
            {
                burstCombo.times.aa = Utils.GameTimeTickCount;
                if (huabian.菜单.Item("burstComboExpanded").GetValue<KeyBind>().Active)
                    burstCombo.canaa = false;
            }


            if (args.SData.Name == "rivenizunablade" && huabian.菜单.Item("burstComboExpanded").GetValue<KeyBind>().Active)
                burstCombo.canq = false;
        }

        private static void 总显示(EventArgs args)
        {

            if (Player.IsDead)
            {
                return;
            }

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

            if (target != null)
                Render.Circle.DrawCircle(target.Position, 100, System.Drawing.Color.White, 50);

            if (huabian.菜单.Item("draw").GetValue<bool>())
            {
                if (Flash.IsReady())
                    Render.Circle.DrawCircle(Player.Position, Flash.Range + E.Range, System.Drawing.Color.White);
                else
                    Render.Circle.DrawCircle(Player.Position, E.Range, System.Drawing.Color.White);
            }

            if (wallStartPos.Distance(endPos) >= 100 && drawColor != System.Drawing.Color.ForestGreen && 
                drawColor != System.Drawing.Color.DarkRed)
            {
                if (!huabian.菜单.Item("jump").GetValue<KeyBind>().Active)
                {
                    lastWallStartPos = wallStartPos;
                    lastEndPos = endPos;
                    lastColor = drawColor;
                    tick = Utils.TickCount;
                }
            }
            else if (wardJumpPos.Distance(Player.Position) >= 100 && drawColor == System.Drawing.Color.ForestGreen)
            {
                //walljumps
                if (!huabian.菜单.Item("jump").GetValue<KeyBind>().Active)
                {
                    lastWallJumpEndPos = wardJumpPos;

                    wallJumpTick = Utils.TickCount;
                }
            }
            else if (wardJumpPos.Distance(Player.Position) >= 100 && drawColor == System.Drawing.Color.DarkRed)
                Render.Circle.DrawCircle(lastWallJumpEndPos.To3D(), 100, System.Drawing.Color.DarkRed);

            if (lastWallStartPos != null && lastEndPos != null
                && Utils.TickCount - tick <= 1000 && drawColor != System.Drawing.Color.ForestGreen)
            {
                Render.Circle.DrawCircle(lastWallStartPos.To3D(), 100, lastColor);
                Render.Circle.DrawCircle(lastEndPos.To3D(), 100, lastColor);
            }
            else if (lastWallStartPos != null && lastEndPos != null && Utils.TickCount - wallJumpTick <= 1000 &&
                drawColor == System.Drawing.Color.ForestGreen)
            {
                Render.Circle.DrawCircle(lastWallJumpEndPos.To3D(), 100, System.Drawing.Color.ForestGreen);
            }
        }

        private static void 总菜单(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            Brust();

            WallJump();

            CheckQCounts();
/*            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    连招();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    骚扰();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    清线();
                    清野();
                    break;
            }*/
        }

        private static void CheckQCounts()
        {
            foreach (var buff in Player.Buffs)
            {
                if (buff.Name.Contains("RivenTriCleave"))
                    qCount = buff.Count;
            }

            if (!Q.IsReady())
                qCount = 0;
        }

        private static void WallJump()
        {
            if (huabian
                .菜单.Item("jump").GetValue<KeyBind>().Active)
            {
                if (drawColor == System.Drawing.Color.Green)
                {
                    if (Player.Distance(lastWallStartPos) > 50)
                        Player.IssueOrder(GameObjectOrder.MoveTo, lastWallStartPos.To3D());
                    else
                        if (Prediction.GetPrediction(Player, 500).UnitPosition.Distance(Player.Position) <= 10 && qCount == 2)
                            Q.Cast(lastEndPos);
                }
                else if (qCount == 2 && drawColor == System.Drawing.Color.ForestGreen)
                {
                    drawColor = System.Drawing.Color.White;
                    Items.UseItem((int)Items.GetWardSlot().Id, Player.Position.To2D().Extend(lastWallJumpEndPos, 400));
                    Q.Cast(lastWallJumpEndPos);
                }
            }


            Vector2 dir = new Vector2(0, 0);

            float scale = 200;

            pos = Player.Position.To2D() + scale * Player.Direction.To2D().Perpendicular();

            if (Utility.IsWall(pos))
            {
                dir = scale * Player.Direction.To2D().Perpendicular();
                Continue(dir);
            }

            if (!Utility.IsWall(pos) || lastColor == System.Drawing.Color.Red)
            {
                scale = 200;

                pos2 = Player.Position.To2D() + (scale * Player.Direction.To2D().Perpendicular()).Perpendicular();
                pos3 = Player.Position.To2D() + (scale * Player.Direction.To2D().Perpendicular()).Perpendicular2();

                if (Utility.IsWall(pos2))
                {
                    dir = (scale * Player.Direction.To2D().Perpendicular()).Perpendicular();
                    wardJumpPos = pos2;
                    Continue(dir, true);
                }
                else if (Utility.IsWall(pos3))
                {
                    dir = (scale * Player.Direction.To2D().Perpendicular()).Perpendicular2();
                    wardJumpPos = pos3;
                    Continue(dir, true);
                }
            }
        }

        private static void Continue(Vector2 dir, bool wardJump = false)
        {
            endPos = MathWallJump.wallEndPos(Player.Position.To2D(), dir);
            wallStartPos = MathWallJump._wallStartPos(Player.Position.To2D(), dir);

            if (!wardJump)
            {
                if (wallStartPos.Distance(endPos) / 2 < Q.Range)
                    drawColor = System.Drawing.Color.Green;
                else
                    drawColor = System.Drawing.Color.Red;

            }
            else if (wardJump)
            {
                if (wallStartPos.Distance(endPos) / 2 < Q.Range)
                    drawColor = System.Drawing.Color.ForestGreen;
                else
                    drawColor = System.Drawing.Color.DarkRed;
            }
        }

        private static void Brust()
        {
            if (huabian.菜单.Item("reset").GetValue<bool>())
            {
                setAllTrue();
                Utility.DelayAction.Add(500, delegate { huabian.菜单.Item("reset").SetValue(false); });
            }

            target = TargetSelector.GetSelectedTarget();
            //target = HeroManager.Enemies.Where(x => x.ChampionName.ToLower().Contains("mundo")).First();

            foreach (var buff in Player.Buffs)
            {
                if (buff.Name.Contains("RivenTriCleave"))
                    qCount = buff.Count;
            }

            if (!Q.IsReady())
                qCount = 0;


            checkCombos();
        }

        private static void checkCombos()
        {
            Items.Item hydra = new Items.Item(3074);
            Items.Item tiamat = new Items.Item(3077);

            bool tiamatRdy = tiamat.IsReady();

            Items.Item item = tiamatRdy ? tiamat : hydra;

            if (huabian.菜单.Item("burstComboExpanded").GetValue<KeyBind>().Active && target != null)
            {
                if (Flash.IsReady() && Player.Distance(target.ServerPosition) <= Flash.Range + E.Range)
                {
                    CastMode(target, item);
                }
                else if (!Flash.IsReady() && Player.Distance(target.ServerPosition) <= E.Range)
                {
                    CastMode(target, item, false);
                }
            }
        }

        private static void CastMode(Obj_AI_Hero target, Item item, bool flash = true)
        {
            //E -> R -> Flash -> W -> Q -> AA -> Hydra -> R2 -> Q2 doublecast

            if (burstCombo.canw)
            {
                E.Cast(target.ServerPosition);
                R.Cast();

                if (flash)
                    Flash.Cast(target.ServerPosition);

                W.Cast();
            }

            if (!burstCombo.canw && qCount == 0)
            {
                Q.Cast(target.ServerPosition);
            }

            if (!burstCombo.canq)
            {
                Player.IssueOrder(GameObjectOrder.AttackUnit, target);
            }

            float delay = huabian.菜单.Item("delay").GetValue<Slider>().Value;

            if (!burstCombo.canaa &&
                Utils.GameTimeTickCount >= burstCombo.times.aa + (Player.AttackDelay * 100) + delay)
            {
                item.Cast();
            }

            if (!burstCombo.canItem)
            {
                R.Cast(target.ServerPosition);
            }

            if (!burstCombo.canr && qCount == 1)
            {
                Q.Cast(target.ServerPosition);
                setAllTrue();
            }
        }

        private static void setAllTrue()
        {
            burstCombo.canaa = true;
            burstCombo.cane = true;
            burstCombo.canItem = true;
            burstCombo.canq = true;
            burstCombo.canr = true;
            burstCombo.canw = true;
        }
    }

    class MathWallJump
    {

        public static List<Vector2> ExtendDist(Vector2 origin, Vector2 currWallPos, float dist)
        {
            //getting OPs
            List<Vector2> resultVecs = new List<Vector2>();

            for (float i = 1; i <= dist; i += .05f)
            {
                if (!Utility.IsWall(origin.Extend(currWallPos, i)))
                    resultVecs.Add(origin.Extend(currWallPos, i));
            }

            return resultVecs;
        }

        public static Vector2 _wallStartPos(Vector2 op, Vector2 dir)
        {
            if (!Utility.IsWall(op + dir))
                return new Vector2();

            Vector2 endPos = op + dir;

            for (float i = 1; i > 0; i -= .005f)
            {
                if (!Utility.IsWall(endPos))
                    break;

                endPos = op + Vector2.Multiply(dir, i);
            }

            return endPos;
        }

        public static Vector2 wallEndPos(Vector2 op, Vector2 dir)
        {
            if (!Utility.IsWall(op + dir))
                return new Vector2();

            Vector2 endPos = op + dir;

            for (float i = 1; i < 500; i += .1f)
            {
                if (!Utility.IsWall(endPos))
                    break;

                endPos = op + Vector2.Multiply(dir, i);
            }

            return endPos;
        }

        public static Vector2 BuildEndPos(Vector2 startPos, Vector2 currentPos)
        {
            var v1 = startPos - currentPos;

            Vector2 result = startPos + v1;

            for (float i = 1; i < 500; i += .1f)
            {
                if (!Utility.IsWall(result))
                    break;

                result = startPos + Vector2.Multiply(v1, i);
            }

            return result;
        }
    }
}