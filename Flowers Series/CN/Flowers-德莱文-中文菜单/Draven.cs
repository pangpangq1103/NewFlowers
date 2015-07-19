using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using LeagueSharp.Common.Data;
using Item = LeagueSharp.Common.Items.Item;

namespace Flowers_Draven
{
    class Draven
    {
        public List<QRecticle> QReticles = new List<QRecticle>();
        public static Spell Q, W, E, R;
        public static Orbwalking.Orbwalker Orbwalker;

        public const string ChampionName = "Draven";
        public  static Obj_AI_Hero Player
        {
            get { return ObjectManager.Player; }
        }
        public int QCount
        {
            get
            {
                return (Player.HasBuff("dravenspinningattack")
                    ? Player.Buffs.First(x => x.Name == "dravenspinningattack").Count
                    : 0) + QReticles.Count;
            }
        }
        public void Load()
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 1050);
            R = new Spell(SpellSlot.R);
            E.SetSkillshot(250, 130, 1400, false, SkillshotType.SkillshotLine);
            R.SetSkillshot(400, 160, 2000, false, SkillshotType.SkillshotLine);

            DM.DMenu();

            GameObject.OnCreate += GameObjectOnOnCreate;
            GameObject.OnDelete += GameObjectOnOnDelete;
            Game.OnUpdate += GameOnUpdate;
            Drawing.OnDraw += 范围显示;
        }

        private void 范围显示(EventArgs args)
        {
            if (DM.菜单.Item("drawing").GetValue<bool>())
            {
                return;
            }

            if (Player.IsDead)
            {
                return;
            }

            var AA范围OKTWStyle = DM.菜单.Item("drawingAA").GetValue<bool>();
            var AA目标OKTWStyle = DM.菜单.Item("orb").GetValue<bool>();
            var Q范围 = DM.菜单.Item("drawingQ").GetValue<Circle>();
            var E范围 = DM.菜单.Item("drawingE").GetValue<Circle>();
            var 补刀小兵 = DM.菜单.Item("bdxb").GetValue<Circle>();
            var 附近可击杀 = DM.菜单.Item("fjkjs").GetValue<Circle>();
            var 显示斧头落地点 = DM.菜单.Item("DrawAxeLocation").GetValue<bool>();
            var 显示斧头范围 = DM.菜单.Item("DrawAxeRange").GetValue<bool>();

            if (显示斧头落地点)
            {
                var bestAxe =
                    QReticles
                        .Where(
                            x =>
                                x.Position.Distance(Game.CursorPos) <
                                DM.菜单.Item("CatchAxeRange").GetValue<Slider>().Value)
                        .OrderBy(x => x.Position.Distance(Player.ServerPosition))
                        .ThenBy(x => x.Position.Distance(Game.CursorPos))
                        .FirstOrDefault();

                if (bestAxe != null)
                {
                    Render.Circle.DrawCircle(bestAxe.Position, 120, Color.LimeGreen);
                }

                foreach (
                    var axe in
                        QReticles.Where(x => x.Object.NetworkId != (bestAxe == null ? 0 : bestAxe.Object.NetworkId)))
                {
                    Render.Circle.DrawCircle(axe.Position, 120, Color.Yellow);
                }
            }

            if (显示斧头范围)
            {
                Render.Circle.DrawCircle(Game.CursorPos, DM.菜单.Item("CatchAxeRange").GetValue<Slider>().Value,
                    Color.DodgerBlue);
            }

            if (AA范围OKTWStyle)
            {
                if (Player.Health / Player.MaxHealth * 100 > 60)
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.GreenYellow, 2);
                else if (Player.Health / Player.MaxHealth * 100 > 30)
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
                Render.Circle.DrawCircle(Player.Position, E.Range - 30, E范围.Color);

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

            if (Game.MapId == (GameMapId)11 && DM.菜单.Item("wushangdaye").GetValue<bool>())
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


        internal class QRecticle
        {
            public QRecticle(GameObject rectice, int expireTime)
            {
                Object = rectice;
                ExpireTime = expireTime;
            }

            public GameObject Object { get; set; }
            public int ExpireTime { get; set; }

            public Vector3 Position
            {
                get { return Object.Position; }
            }
        }

        private void GameObjectOnOnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.Name.Contains("Draven_Base_Q_reticle_self.troy"))
            {
                return;
            }

            QReticles.RemoveAll(x => x.Object.NetworkId == sender.NetworkId);

        }

        private void GameObjectOnOnCreate(GameObject sender, EventArgs args)
        {
            if (!sender.Name.Contains("Draven_Base_Q_reticle_self.troy"))
            {
                return;
            }

            QReticles.Add(new QRecticle(sender, Environment.TickCount + 1800));
            Utility.DelayAction.Add(1800, () => QReticles.RemoveAll(x => x.Object.NetworkId == sender.NetworkId));
        }

        private void GameOnUpdate(EventArgs args)
        {
            var catchOption = DM.菜单.Item("AxeMode").GetValue<StringList>().SelectedIndex;

            if ((catchOption == 0 && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo) ||
                (catchOption == 1 && Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.None) || catchOption == 2)
            {
                var bestReticle =
                    QReticles
                        .Where(
                            x =>
                                x.Object.Position.Distance(Game.CursorPos) <
                                DM.菜单.Item("CatchAxeRange").GetValue<Slider>().Value)
                        .OrderBy(x => x.Position.Distance(Player.ServerPosition))
                        .ThenBy(x => x.Position.Distance(Game.CursorPos))
                        .FirstOrDefault();

                if (bestReticle != null && bestReticle.Object.Position.Distance(Player.ServerPosition) > 110)
                {
                    var eta = 1000 * (Player.Distance(bestReticle.Position) / Player.MoveSpeed);
                    var expireTime = bestReticle.ExpireTime - Environment.TickCount;

                    if (eta >= expireTime && DM.菜单.Item("UseWForQ", true).IsActive())
                    {
                        W.Cast();
                    }

                    if (DM.菜单.Item("DontCatchUnderTurret").IsActive())
                    {
                        // If we're under the turret as well as the axe, catch the axe
                        if (Player.UnderTurret(true) && bestReticle.Object.Position.UnderTurret(true))
                        {
                            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, bestReticle.Position);
                            }
                            else
                            {
                                Orbwalker.SetOrbwalkingPoint(bestReticle.Position);
                            }
                        }
                        // Catch axe if not under turret
                        else if (!bestReticle.Position.UnderTurret(true))
                        {
                            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                            {
                                Player.IssueOrder(GameObjectOrder.MoveTo, bestReticle.Position);
                            }
                            else
                            {
                                Orbwalker.SetOrbwalkingPoint(bestReticle.Position);
                            }
                        }
                    }
                    else
                    {
                        if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.None)
                        {
                            Player.IssueOrder(GameObjectOrder.MoveTo, bestReticle.Position);
                        }
                        else
                        {
                            Orbwalker.SetOrbwalkingPoint(bestReticle.Position);
                        }
                    }
                }
                else
                {
                    Orbwalker.SetOrbwalkingPoint(Game.CursorPos);
                }
            }
            else
            {
                Orbwalker.SetOrbwalkingPoint(Game.CursorPos);
            }

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    JungleClear();
                    LaneClear();
                    break;
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
            }

        }


        private void Combo()
        {
            var Qbuff = Player.Buffs.Find(b => b.Name.ToLower() == "dravenspinning");
            var Wbuffmove = Player.Buffs.Find(b => b.Name.ToLower() == "dravenfury");
            var Wbuffas = Player.Buffs.Find(b => b.Name.ToLower() == "dravenfurybuff");
            var target = TargetSelector.GetTarget(700, TargetSelector.DamageType.Physical);
            if (Q.IsReady() && DM.菜单.Item("lzq").GetValue<bool>())
            {
                if (Orbwalking.InAutoAttackRange(target) && target != null 
                    && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Qbuff == null) 
                    Q.Cast(); 
                    else if (QCount <= 3 && Qbuff.Count <= 1) 
                    Q.Cast();
                }
            }
            if (W.IsReady() && DM.菜单.Item("lzw").GetValue<bool>())
            {
                if (Orbwalking.InAutoAttackRange(target) && W.IsReady() && target != null 
                    && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Wbuffas == null)
                    {W.Cast(); 
                }
                else if (!Orbwalking.InAutoAttackRange(target) && W.IsReady() && target != null 
                        && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Wbuffmove == null)
                     W.Cast(); 
                }
            }
                if (W.IsReady() && DM.菜单.Item("lzw1").GetValue<bool>())
            {

                if (target == null || !target.IsValidTarget() || target.IsZombie)
                {
                    var target1 = TargetSelector.GetTarget(1100, TargetSelector.DamageType.Physical);
                    if (target != null && target.IsValidTarget() && !target.IsZombie) { W.Cast(); }
                }
            }
            if (E.IsReady() && DM.菜单.Item("lze").GetValue<bool>())
            {
                if (!Player.IsWindingUp && target != null && target.IsValidTarget() && !target.IsZombie) { E.Cast(target); }
            }
            if (R.IsReady())
            {
                if (R.Instance.Name.ToLower().Contains("dravenrcast") && target != null && target.IsValidTarget() &&
                    !target.IsZombie && DM.菜单.Item("lzr").GetValue<bool>() && !Player.IsWindingUp &&
                    target.Health / target.MaxHealth * 100 < DM.菜单.Item("lzrhp").GetValue<Slider>().Value)
                R.Cast(target);
                else if (R.Instance.Name.ToLower().Contains("dravenrdoublecast") && target != null && target.IsValidTarget() &&
                    DM.菜单.Item("lzr1").GetValue<bool>())
                R.Cast();
            }
            if (Items.CanUseItem(3142) && Player.Distance(target.Position) < Player.AttackRange && DM.菜单.Item("YoumuuC").GetValue<bool>())
                Items.UseItem(3142);

            if ((Player.Health / Player.MaxHealth) * 100 < (target.Health / target.MaxHealth) * 100 && (Items.CanUseItem(3153) || Items.CanUseItem(3144)) && DM.菜单.Item("BotrkC").GetValue<bool>())
            {
                Items.UseItem(3144, target);

                Items.UseItem(3153, target);
            }
        }
        }
        private void LaneClear()
        {
            var qxq = DM.菜单.Item("qxq").GetValue<bool>();
            var target =
                 MinionManager.GetMinions(
                     Player.Position, 700, MinionTypes.All, MinionTeam.Enemy, MinionOrderTypes.MaxHealth)
                     .FirstOrDefault(a => !a.Name.ToLower().Contains("ward"));
            if (DM.菜单.Item("qxmp",true).GetValue<Slider>().Value > (Player.Mana / Player.MaxMana * 100))
            {
                return;
            }
            if (qxq && Q.IsReady() &&
                 QCount <= 1 &&
                target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Player)))
            {
                Q.Cast();
            }
        }
        private void JungleClear()
        {
            var qyq = DM.菜单.Item("qyq").GetValue<bool>();
            var qyw = DM.菜单.Item("qyw").GetValue<bool>();
            var qye = DM.菜单.Item("qye").GetValue<bool>();

            var target = MinionManager.GetMinions(Player.Position, 600, MinionTypes.All, MinionTeam.Neutral,
                MinionOrderTypes.MaxHealth).FirstOrDefault();

            if (qyq && Q.IsReady() &&
                 QCount <= 1 &&
                target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Player)))
            {
                Q.Cast();
            }
            if (qyw && !ObjectManager.Player.HasBuff("dravenfurybuff", true) &&
                !ObjectManager.Player.HasBuff("dravenfurybuff",true) && W.IsReady() &&
                target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Player)))
            {
                W.Cast();
            }
            if (qye && E.IsReady() &&
                target.IsValidTarget(E.Range))
            {
                E.Cast(target);
            }

        }

        private void Harass()
        {
            var Qbuff = Player.Buffs.Find(b => b.Name.ToLower() == "dravenspinning");
            var Wbuffmove = Player.Buffs.Find(b => b.Name.ToLower() == "dravenfury");
            var Wbuffas = Player.Buffs.Find(b => b.Name.ToLower() == "dravenfurybuff");
            if (Q.IsReady() && DM.菜单.Item("srq").GetValue<bool>() 
                && Player.Mana / Player.MaxMana * 100 > DM.菜单.Item("srmp").GetValue<Slider>().Value)
            {
                var target = TargetSelector.GetTarget(700, TargetSelector.DamageType.Physical);
                if (Orbwalking.InAutoAttackRange(target) && target != null && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Qbuff == null)
                    Q.Cast();

                    else if (QCount <= 3 && Qbuff.Count <= 1)
                     Q.Cast(); 
                }
            }
            if (W.IsReady() && DM.菜单.Item("srw").GetValue<bool>()
                && Player.Mana / Player.MaxMana * 100 > DM.菜单.Item("srmp").GetValue<Slider>().Value)
            {
                var target = TargetSelector.GetTarget(700, TargetSelector.DamageType.Physical);
                if (Orbwalking.InAutoAttackRange(target) && W.IsReady() && target != null
                    && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Wbuffas == null)
                        W.Cast();
                }
                else if (!Orbwalking.InAutoAttackRange(target) && W.IsReady() && target != null 
                    && target.IsValidTarget() && !target.IsZombie)
                {
                    if (Wbuffmove == null)
                    W.Cast();
                }
            }
            if (W.IsReady() && DM.菜单.Item("srw1").GetValue<bool>() 
                && Player.Mana / Player.MaxMana * 100 > DM.菜单.Item("srmp").GetValue<Slider>().Value)
            {
                var target = TargetSelector.GetTarget(700, TargetSelector.DamageType.Physical);
                if (target == null || !target.IsValidTarget() || target.IsZombie)
                {
                    var target1 = TargetSelector.GetTarget(900, TargetSelector.DamageType.Physical);
                    if (target != null && target.IsValidTarget() && !target.IsZombie) 
                    W.Cast();
                }
            }
            if (E.IsReady() && DM.菜单.Item("sre").GetValue<bool>() 
                && Player.Mana / Player.MaxMana * 100 > DM.菜单.Item("srmp").GetValue<Slider>().Value)
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (!Player.IsWindingUp && target != null && target.IsValidTarget() && !target.IsZombie) 
                E.Cast(target);
            }
        }
    }
}
