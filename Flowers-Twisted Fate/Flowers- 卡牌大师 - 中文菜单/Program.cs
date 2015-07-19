using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Kappa
{
    class Program
    {
        private static Obj_AI_Hero Player
        { 
            get
            { 
                return ObjectManager.Player; 
            }
        }
        internal static float getManaPer
        {
            get { return Player.Mana / Player.MaxMana * 100; }
        }

        private static float getHeaper(Obj_AI_Hero Player)
        {
            return Player.Health * 100 / Player.MaxHealth;
        }

        private static Spell Q;
        private static Spell W;
        private static Spell R;
        private static Orbwalking.Orbwalker Orbwalker;
        private static Menu 菜单;
        public const string ChampionName = "TwistedFate";
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

            Notifications.AddNotification("Flowers Twisted by NightMoon", 1000);
            Notifications.AddNotification("`                  And  Lost`", 1000);
            Notifications.AddNotification("Version : 1.0.0.0", 1000);
            Game.PrintChat("Flowers-Twisted Fate Loaded!~~~ Version : 1.0.0.0 Thanks for your use!");
            Game.PrintChat("What are the deficiencies please immediately feedback. Thank you! ");

            菜单 = new Menu("Flowers - 卡牌大师", "flowersKappa", true);

            var targetSelectorMenu = new Menu("目标选择", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            Orbwalker = new Orbwalking.Orbwalker(菜单.AddSubMenu(new Menu("走砍设置", "Orbwalker")));

            菜单.AddSubMenu(new Menu("连招设置", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("haha", "仅需按你连招的按键~"));

            菜单.AddSubMenu(new Menu("骚扰设置", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "使用 Q")).SetValue(true);
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "自动 Q").SetValue(new KeyBind("U".ToCharArray()[0], KeyBindType.Press)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srw", "使用 W(蓝牌)")).SetValue(true);

            菜单.AddSubMenu(new Menu("清理设置", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "使用 Q").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxw", "使用 W(红或蓝)").SetValue(true));

            菜单.AddSubMenu(new Menu("卡牌选择", "CardSelect"));
            菜单.SubMenu("CardSelect").AddItem(new MenuItem("blue", "蓝牌").SetValue(new KeyBind("E".ToCharArray()[0],KeyBindType.Press)));
            菜单.SubMenu("CardSelect").AddItem(new MenuItem("yellow", "黄牌").SetValue(new KeyBind("W".ToCharArray()[0], KeyBindType.Press)));
            菜单.SubMenu("CardSelect").AddItem(new MenuItem("red", "红牌").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));

            菜单.AddSubMenu(new Menu("杂项设置", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("KSQ", "使用 Q 击杀/眩晕")).SetValue(true);
          //  菜单.SubMenu("Misc").AddItem(new MenuItem("dd", "Use W Interrupt Spell")).SetValue(true);
          //  菜单.SubMenu("Misc").AddItem(new MenuItem("tj", "Use W Anti GapCloser")).SetValue(true);
            菜单.SubMenu("Misc").AddItem(new MenuItem("AutoYellow", "落地黄牌").SetValue(true));

            菜单.AddSubMenu(new Menu("显示设置", "Draw"));
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingQ", "Q 范围").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingR", "R 范围").SetValue(new Circle(true, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingR2", "R 范围 (小地图)").SetValue(new Circle(true, Color.FromArgb(255, 255, 255))));
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingAA", "AA 范围(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Draw").AddItem(new MenuItem("orb", "AA 目标(OKTW© Style)").SetValue(true));

            菜单.AddItem(new MenuItem("Credit", "作者 : 花边下丶情未央"));
            菜单.AddItem(new MenuItem("Version", "版本 : 1.0.0.0"));

            菜单.AddToMainMenu();

            Q = new Spell(SpellSlot.Q, 1450f);
            W = new Spell(SpellSlot.W, Orbwalking.GetRealAutoAttackRange(Player));
            R = new Spell(SpellSlot.R, 5500f);

            Q.SetSkillshot(0.25f, 40f, 1000f, false, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.3f, 80f, 1600, true, SkillshotType.SkillshotLine);

            Game.OnUpdate += 主菜单;
            Drawing.OnDraw += 范围显示;
            Drawing.OnEndScene += 地图显示;
            Orbwalking.BeforeAttack += OrbwalkingOnBeforeAttack;//H7 
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;//H7 
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var 落地自动黄 = 菜单.Item("AutoYellow").GetValue<bool>();

            if (args.SData.Name == "gate" && 落地自动黄)
            {
                CardSelect.StartSelecting(Cards.Yellow);
            }
        }

        private static void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (args.Target is Obj_AI_Hero)
                args.Process = CardSelect.Status != SelectStatus.Selecting && Environment.TickCount - CardSelect.LastWSent > 300;
        }

        private static void 范围显示(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            var AA范围OKTWStyle = 菜单.Item("drawingAA").GetValue<bool>();
            var AA目标OKTWStyle = 菜单.Item("orb").GetValue<bool>();
            var Q范围 = 菜单.Item("drawingQ").GetValue<Circle>();
            var R范围 = 菜单.Item("drawingR").GetValue<Circle>();

            if (AA范围OKTWStyle)
            {
                if (getHeaper(Player) > 60)
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.GreenYellow);
                else if (getHeaper(Player) > 30)
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.Orange);
                else
                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), System.Drawing.Color.Red);
            }

            if (AA目标OKTWStyle)
            {
                var orbT = Orbwalker.GetTarget();

                if (orbT.IsValidTarget())
                {
                    if (orbT.Health > orbT.MaxHealth * 0.6)
                        Render.Circle.DrawCircle(orbT.Position, orbT.BoundingRadius + 15, System.Drawing.Color.GreenYellow);
                    else if (orbT.Health > orbT.MaxHealth * 0.3)
                        Render.Circle.DrawCircle(orbT.Position, orbT.BoundingRadius + 15, System.Drawing.Color.Orange);
                    else
                        Render.Circle.DrawCircle(orbT.Position, orbT.BoundingRadius + 15, System.Drawing.Color.Red);
                }
            }

            if (Q.IsReady() && Q范围.Active)
                Render.Circle.DrawCircle(Player.Position, Q.Range, Q范围.Color);

            if (R.IsReady() && R范围.Active)
                Render.Circle.DrawCircle(Player.Position, 5500, R范围.Color);

        }
        private static void 地图显示(EventArgs args)
        {
            var 小地图R = 菜单.Item("drawingR2").GetValue<Circle>();

            if (小地图R.Active)
            {
                Utility.DrawCircle(Player.Position, 5500, 小地图R.Color, 1, 30, true);
            }
        }

        private static void 主菜单(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    if (W.IsReady())
                    {
                        CardSelect.StartSelecting(Cards.Yellow);
                    }
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    骚扰();
                    骚扰2();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    清线();
                    清野();
                    break;
            }

            if (菜单.Item("AutoQ").GetValue<KeyBind>().Active)
            {
                骚扰();
            }
            自动Q();
            选牌();
          //  杂项Q();
        }

        private static void 连招()
        {

        }

        private static void 骚扰()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            if (Q.IsReady() && (菜单.Item("srq").GetValue<bool>()))
            {
                var Qprediction = Q.GetPrediction(target);

                if (Qprediction.Hitchance >= HitChance.VeryHigh)
                {
                    Q.Cast(Qprediction.CastPosition);
                }
            }
        }
        private static void 骚扰2()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);

            if (Player.Distance(target.ServerPosition) < Player.AttackRange - 40)
            {
                CardSelect.StartSelecting(Cards.Blue);
            }

        }

        private static void 清线()
        {
            var 使用Q = 菜单.Item("qxq").GetValue<bool>();

            if (Q.IsReady() && 使用Q && getManaPer > 45)
            {
                var allMinionsQ = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy);
                var locQ = Q.GetLineFarmLocation(allMinionsQ);

                if (locQ.MinionsHit >= 3)
                    Q.Cast(locQ.Position);
            }

            var minioncount = MinionManager.GetMinions(Player.Position, 1500).Count;

            if (minioncount > 0)
            {
                if (getManaPer > 45)
                {
                    if (minioncount >= 3)
                        CardSelect.StartSelecting(Cards.Red);
                    else
                        CardSelect.StartSelecting(Cards.Blue);
                }
                else
                    CardSelect.StartSelecting(Cards.Blue);
            }
        }

        private static void 清野()
        {
            var 使用Q = 菜单.Item("qxq").GetValue<bool>();
            var Clear = 菜单.Item("qxw").GetValue<bool>();

            var mobs = MinionManager.GetMinions(Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(Player) + 50, MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count <= 0)
                return;

            if (Q.IsReady() && 使用Q && getManaPer > 45)
            {
                Q.Cast(mobs[0].Position);
            }

            if (W.IsReady() && Clear)
            {
                if (getManaPer > 45)
                {
                    if (mobs.Count >= 2)
                        CardSelect.StartSelecting(Cards.Red);
                }
                else
                    CardSelect.StartSelecting(Cards.Blue);
            }
        }
        private static void 自动Q()
        {
            var QQQ = 菜单.Item("KSQ").GetValue<bool>();

            if (!QQQ)
                return;

            foreach (Obj_AI_Hero target in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsValidTarget(Q.Range) && x.IsEnemy && !x.IsDead && !x.HasBuffOfType(BuffType.Invulnerability)))
            {
                if (target != null)
                {
                    if (Q.GetDamage(target) >= target.Health + 20 & Q.GetPrediction(target).Hitchance >= HitChance.VeryHigh)
                    {
                        if (Q.IsReady())
                            Q.Cast(target);
                    }
                }
            }

            if (Player.Spellbook.CanUseSpell(SpellSlot.Q) == SpellState.Ready && QQQ)
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (enemy.IsValidTarget(Q.Range * 2))
                    {
                        var pred = Q.GetPrediction(enemy);
                        if ((pred.Hitchance == HitChance.Immobile && QQQ))
                        {
                            Q.Cast(enemy);
                        }
                    }
                }
        }
        private static void 选牌()
        {
            if (菜单.Item("yellow").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Yellow);

            if (菜单.Item("blue").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Blue);

            if (菜单.Item("red").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Red);
        }
      /*  private static void 杂项Q()
        {
            var 打断 = 菜单.Item("dd").GetValue<bool>();
            var 突进 = 菜单.Item("tj").GetValue<bool>();

        }*/
    }

}
