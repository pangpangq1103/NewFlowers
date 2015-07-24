using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;

namespace Flowers_TwitchFate
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
            Notifications.AddNotification("Version : 1.0.0.2", 1000);

            菜单 = new Menu("FL - Twisted Fate", "flowersKappa", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            Orbwalker = new Orbwalking.Orbwalker(菜单.AddSubMenu(new Menu("Orbwalker", "Orbwalker")));

            菜单.AddSubMenu(new Menu("Combo", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("haha2", "Mana < 20% Auto Blue Card"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("haha", "Only Active Combo Key~"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("haha1", "Enjoy your game~"));


            菜单.AddSubMenu(new Menu("Harass", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q")).SetValue(true);
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "Auto Q").SetValue(new KeyBind("U".ToCharArray()[0], KeyBindType.Toggle)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srw", "Use W(Blue Card)")).SetValue(true);
            菜单.SubMenu("Harass").AddItem(new MenuItem("srwr", "Use W(Red Card)")).SetValue(true);

            菜单.AddSubMenu(new Menu("Clear", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q").SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxw", "Use W(Red or Blue)").SetValue(true));

            菜单.AddSubMenu(new Menu("Card Select", "CardSelect"));
            菜单.SubMenu("CardSelect").AddItem(new MenuItem("blue", "Blue Card").SetValue(new KeyBind("E".ToCharArray()[0], KeyBindType.Press)));
            菜单.SubMenu("CardSelect").AddItem(new MenuItem("yellow", "Yellow Card").SetValue(new KeyBind("W".ToCharArray()[0], KeyBindType.Press)));
            菜单.SubMenu("CardSelect").AddItem(new MenuItem("red", "Red Card").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));

            菜单.AddSubMenu(new Menu("Misc", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("KSQ", "Use Q KS/Stun")).SetValue(true);
            //add
            菜单.SubMenu("Misc").AddItem(new MenuItem("dd", "Use W Interrupt Spell")).SetValue(true);
            菜单.SubMenu("Misc").AddItem(new MenuItem("tj", "Use W Anti GapCloser")).SetValue(true);
            //x
            菜单.SubMenu("Misc").AddItem(new MenuItem("AutoYellow", "Auto Yellow Card In Uit").SetValue(true));

            菜单.AddSubMenu(new Menu("Draw", "Draw"));
            //add
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawoff", "Disabled All Drawing").SetValue(false));
            //x
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(true, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingR2", "R Range (MiniMap)").SetValue(new Circle(true, Color.FromArgb(255, 255, 255))));
            菜单.SubMenu("Draw").AddItem(new MenuItem("drawingAA", "Real AA&W Range(花边 Style)").SetValue(true));
            菜单.SubMenu("Draw").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));

            菜单.AddItem(new MenuItem("Credit", "Credit : NightMoon"));
            菜单.AddItem(new MenuItem("Version", "Version : 1.0.0.1"));

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
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero target, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (菜单.Item("dd").GetValue<bool>() && W.IsReady() && W.IsInRange(target))
            {
                CardSelect.StartSelecting(Cards.Yellow);
            }
        }

        static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (gapcloser.Sender.IsAlly)
            {
                return;
            }
            if (菜单.Item("tj").GetValue<bool>() && W.IsReady() && W.IsInRange(gapcloser.End))
            {
                CardSelect.StartSelecting(Cards.Yellow);
            }
        }

        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var 落地自动黄 = 菜单.Item("AutoYellow").GetValue<bool>();

            if (args.SData.Name == "gate" && 落地自动黄)
            {
                CardSelect.StartSelecting(Cards.Yellow);
            }
        }

        static void OrbwalkingOnBeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (args.Target is Obj_AI_Hero)
                args.Process = CardSelect.Status != SelectStatus.Selecting && Utils.TickCount - CardSelect.LastWSent > 300;
        }

        static void 范围显示(EventArgs args)
        {
            var disdraw = 菜单.Item("drawoff").GetValue<bool>();

            if (disdraw)
            {
                return;
            }

            var FlowersStyle = 菜单.Item("drawingAA").GetValue<bool>();
            var AA目标OKTWStyle = 菜单.Item("orb").GetValue<bool>();
            var Q范围 = 菜单.Item("drawingQ").GetValue<Circle>();
            var R范围 = 菜单.Item("drawingR").GetValue<Circle>();

            if (FlowersStyle)
            {
                    Color FlowersAAStyle = Color.LightGreen;
                    var Wbuff = Player.Spellbook.GetSpell(SpellSlot.W).Name;
                    if(Wbuff == "goldcardlock")
                    {
                        FlowersAAStyle = Color.Gold;
                    }
                    else if(Wbuff == "bluecardlock")
                    {
                        FlowersAAStyle = Color.Blue;
                    }
                    else if (Wbuff == "redcardlock")
                    {
                        FlowersAAStyle = Color.Red;
                    }
                    else if (Wbuff == "PickACard")
                    {
                        FlowersAAStyle = Color.GreenYellow;
                    }

                    Render.Circle.DrawCircle(Player.Position, Orbwalking.GetRealAutoAttackRange(Player), FlowersAAStyle ,2);
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
        static void 地图显示(EventArgs args)
        {
            var 小地图R = 菜单.Item("drawingR2").GetValue<Circle>();

            if (小地图R.Active)
            {
                Utility.DrawCircle(Player.Position, 5500, 小地图R.Color, 1, 30, true);
            }
        }

        static void 主菜单(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (菜单.Item("yellow").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Yellow);

            if (菜单.Item("blue").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Blue);

            if (菜单.Item("red").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Red);

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    连招();
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

        }

        static void 连招()
        {
            var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

            if (W.IsReady())
            {
                if (target.IsValidTarget(W.Range))
                {
                    if (getManaPer < 20)
                        CardSelect.StartSelecting(Cards.Blue);
                    else
                        CardSelect.StartSelecting(Cards.Yellow);
                }
            }

            if (Q.IsReady())
            {
                if (target.IsValidTarget(Q.Range))
                {
                    var Qpre = Q.GetPrediction(target);


                    if (Qpre.Hitchance >= HitChance.VeryHigh)
                    {
                        Q.Cast(Qpre.CastPosition);
                    }

                    if (Q.IsReady() && 
                        ((
                        target.HasBuffOfType(BuffType.Stun) ||
                        target.HasBuffOfType(BuffType.Snare) ||
                        target.HasBuffOfType(BuffType.Knockup)
                        ))
                        )
                    {
                        Q.CastIfHitchanceEquals(target, HitChance.High, true);
                    }
                }
            }

        }

        static void 骚扰()
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
        static void 骚扰2()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);

            if (Player.Distance(target.ServerPosition) < Player.AttackRange - 40 && !菜单.Item("srw").GetValue<bool>())
            {
                CardSelect.StartSelecting(Cards.Blue);
            }

            if (Player.Distance(target, true) < Player.AttackRange - 150 && !菜单.Item("srwr").GetValue<bool>())
            {
                CardSelect.StartSelecting(Cards.Red);
            }

        }

        static void 清线()
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

        static void 清野()
        {
            var 使用Q = 菜单.Item("qxq").GetValue<bool>();
            var Clear = 菜单.Item("qxw").GetValue<bool>();

            var mobs = MinionManager.GetMinions(Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(Player) + 50,
                MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

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
        static void 自动Q()
        {
            var QQQ = 菜单.Item("KSQ").GetValue<bool>();

            if (!QQQ)
                return;

            foreach (Obj_AI_Hero target in ObjectManager.Get<Obj_AI_Hero>().Where(x => x.IsValidTarget(Q.Range) && x.IsEnemy 
                && !x.IsDead && !x.HasBuffOfType(BuffType.Invulnerability)))
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
    }

    public enum Cards
    {
        Red,
        Yellow,
        Blue,
        None,
    }

    public enum SelectStatus
    {
        Ready,
        Selecting,
        Selected,
        Cooldown,
    }
    class CardSelect
    {
        public static Cards Select;
        public static int LastWSent = 0;
        public static int LastSendWSent = 0;
        public static SelectStatus Status
        {
            get;
            set;
        }
        static CardSelect()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private static void SendWPacket()
        {
            LastSendWSent = Utils.TickCount;
            ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
        }

        public static void StartSelecting(Cards card)
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && Status == SelectStatus.Ready)
            {
                Select = card;
                if (Utils.TickCount - LastWSent > 200)
                {
                    if (ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, ObjectManager.Player))
                    {
                        LastWSent = Utils.TickCount;
                    }
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            var wName = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name;
            var wState = ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.W);

            if ((wState == SpellState.Ready &&
                 wName == "PickACard" &&
                 (Status != SelectStatus.Selecting || Utils.TickCount - LastWSent > 500)) ||
                ObjectManager.Player.IsDead)
            {
                Status = SelectStatus.Ready;
            }
            else
                if (wState == SpellState.Cooldown &&
                    wName == "PickACard")
                {
                    Select = Cards.None;
                    Status = SelectStatus.Cooldown;
                }
                else
                    if (wState == SpellState.Surpressed &&
                        !ObjectManager.Player.IsDead)
                    {
                        Status = SelectStatus.Selected;
                    }

            if (Select == Cards.Blue && wName == "bluecardlock")
            {
                SendWPacket();
            }
            else
                if (Select == Cards.Yellow && wName == "goldcardlock")
                {
                    SendWPacket();
                }
                else
                    if (Select == Cards.Red && wName == "redcardlock")
                    {
                        SendWPacket();
                    }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name == "PickACard")
            {
                Status = SelectStatus.Selecting;
            }

            if (args.SData.Name == "goldcardlock" || args.SData.Name == "bluecardlock" ||
                args.SData.Name == "redcardlock")
            {
                Status = SelectStatus.Selected;
            }
        }
    }
}
