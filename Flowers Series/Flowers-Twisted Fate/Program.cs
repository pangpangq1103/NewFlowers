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
        private static Menu Menu;
        public const string ChampionName = "TwistedFate";
        //ping Emery
        private static int LastPingT = 0;
        private static Vector2 PingLocation;

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

            Game.PrintChat("Flowers " + Player.CharData.BaseSkinName + " Loaded!");
            Game.PrintChat("Credit : NightMoon!");
            Notifications.AddNotification("Emmm .", 10000);
            Notifications.AddNotification("You Don't Need Luck", 10000);
            Notifications.AddNotification("Because it was so perfect", 10000);
            Notifications.AddNotification("Flowers Twisted Fate", 10000);
            Notifications.AddNotification("Credit : NightMoon", 10000);

            Menu = new Menu("FL - Twisted Fate", "flowersKappa", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            Menu.AddSubMenu(targetSelectorMenu);

            Orbwalker = new Orbwalking.Orbwalker(Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker")));

            Menu.AddSubMenu(new Menu("Combo", "Combo"));
            Menu.SubMenu("Combo").AddItem(new MenuItem("lzq", "Use Q")).SetValue(true);
            Menu.SubMenu("Combo").AddItem(new MenuItem("lzw", "Use W(Yellow And Blue)")).SetValue(true);
            Menu.SubMenu("Combo").AddItem(new MenuItem("lzwBMama", "Use Blue Mana <=%", true).SetValue(new Slider(20, 0, 50)));


            Menu.AddSubMenu(new Menu("Harass", "Harass"));
            Menu.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q")).SetValue(true);
            Menu.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "Auto Q").SetValue(new KeyBind("U".ToCharArray()[0], KeyBindType.Toggle)));
            Menu.SubMenu("Harass").AddItem(new MenuItem("srw", "Use W(Blue Card)")).SetValue(true);
            Menu.SubMenu("Harass").AddItem(new MenuItem("srwr", "Use W(Red Card)")).SetValue(true);

            Menu.AddSubMenu(new Menu("Clear", "Clear"));
            Menu.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q LaneClear").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("qxw", "Use W LaneClear (Red or Blue)").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("qxmp", "LC Use Blue Mana <=%", true).SetValue(new Slider(45, 0, 100)));
            Menu.SubMenu("Clear").AddItem(new MenuItem("qyq", "Use Q JungleClear").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("qyw", "Use W JungleClear (Red or Blue)").SetValue(true));
            Menu.SubMenu("Clear").AddItem(new MenuItem("qymp", "JC Use Blue Mana <=%", true).SetValue(new Slider(45, 0, 100)));

            Menu.AddSubMenu(new Menu("Card Select", "CardSelect"));
            Menu.SubMenu("CardSelect").AddItem(new MenuItem("blue", "Blue Card").SetValue(new KeyBind("E".ToCharArray()[0], KeyBindType.Press)));
            Menu.SubMenu("CardSelect").AddItem(new MenuItem("yellow", "Yellow Card").SetValue(new KeyBind("W".ToCharArray()[0], KeyBindType.Press)));
            Menu.SubMenu("CardSelect").AddItem(new MenuItem("red", "Red Card").SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Press)));

            Menu.AddSubMenu(new Menu("Misc", "Misc"));
            Menu.SubMenu("Misc").AddItem(new MenuItem("KSQ", "Use Q KS/Stun")).SetValue(true);
            //add
            Menu.SubMenu("Misc").AddItem(new MenuItem("dd", "Use W Interrupt Spell")).SetValue(true);
            Menu.SubMenu("Misc").AddItem(new MenuItem("tj", "Use W Anti GapCloser")).SetValue(true);
            //x
            Menu.SubMenu("Misc").AddItem(new MenuItem("AutoYellow", "Auto Yellow Card In Uit").SetValue(true));
            Menu.SubMenu("Misc").AddItem(new MenuItem("PingLH", "Ping Can Kill Emery (Only local)").SetValue(false));

            Menu.AddSubMenu(new Menu("Draw", "Draw"));
            //add
            Menu.SubMenu("Draw").AddItem(new MenuItem("drawoff", "Disabled All Drawing").SetValue(false));
            //x
            Menu.SubMenu("Draw").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            Menu.SubMenu("Draw").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(true, Color.FromArgb(0, 255, 0))));
            Menu.SubMenu("Draw").AddItem(new MenuItem("drawingR2", "R Range (MiniMap)").SetValue(new Circle(true, Color.FromArgb(255, 255, 255))));
            Menu.SubMenu("Draw").AddItem(new MenuItem("drawingAA", "Real AA&W Range(花边 Style)").SetValue(true));
            Menu.SubMenu("Draw").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));
            //add

            //Damage after combo:
            var dmgAfterComboItem = new MenuItem("DamageAfterCombo", "Draw damage after combo").SetValue(true);
            Utility.HpBarDamageIndicator.DamageToUnit = ComboDamage;
            Utility.HpBarDamageIndicator.Enabled = dmgAfterComboItem.GetValue<bool>();
            dmgAfterComboItem.ValueChanged += delegate(object sender, OnValueChangeEventArgs eventArgs)
            {
                Utility.HpBarDamageIndicator.Enabled = eventArgs.GetNewValue<bool>();
            };
            Menu.SubMenu("Draw").AddItem(dmgAfterComboItem);

            Menu.AddItem(new MenuItem("Credit", "Credit : NightMoon"));
            Menu.AddItem(new MenuItem("Version", "Version : 1.0.0.5"));

            Menu.AddToMainMenu();

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
            if (Menu.Item("dd").GetValue<bool>() && W.IsReady() && W.IsInRange(target))
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
            if (Menu.Item("tj").GetValue<bool>() && W.IsReady() && W.IsInRange(gapcloser.End))
            {
                CardSelect.StartSelecting(Cards.Yellow);
            }
        }

        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            var 落地自动黄 = Menu.Item("AutoYellow").GetValue<bool>();

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
            var disdraw = Menu.Item("drawoff").GetValue<bool>();

            if (disdraw)
            {
                return;
            }

            var FlowersStyle = Menu.Item("drawingAA").GetValue<bool>();
            var AA目标OKTWStyle = Menu.Item("orb").GetValue<bool>();
            var Q范围 = Menu.Item("drawingQ").GetValue<Circle>();
            var R范围 = Menu.Item("drawingR").GetValue<Circle>();

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
        static float ComboDamage(Obj_AI_Hero hero)
        {
            var dmg = 0d;
            dmg += Player.GetSpellDamage(hero, SpellSlot.Q) * 2;
            dmg += Player.GetSpellDamage(hero, SpellSlot.W);
            dmg += Player.GetSpellDamage(hero, SpellSlot.Q);

            if (ObjectManager.Player.GetSpellSlot("SummonerIgnite") != SpellSlot.Unknown)
            {
                dmg += ObjectManager.Player.GetSummonerSpellDamage(hero, Damage.SummonerSpell.Ignite);
            }

            return (float)dmg;
        }

        static void 地图显示(EventArgs args)
        {
            var 小地图R = Menu.Item("drawingR2").GetValue<Circle>();

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

            if (Menu.Item("yellow").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Yellow);

            if (Menu.Item("blue").GetValue<KeyBind>().Active)
                CardSelect.StartSelecting(Cards.Blue);

            if (Menu.Item("red").GetValue<KeyBind>().Active)
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

            if (Menu.Item("AutoQ").GetValue<KeyBind>().Active)
            {
                骚扰();
            }
            自动Q();

            PingCanKill();

        }

        static void PingCanKill()
        {
            if (Menu.Item("PingLH").GetValue<bool>())
                foreach (
                    var enemy in
                        ObjectManager.Get<Obj_AI_Hero>()
                            .Where(
                                h =>
                                    ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready &&
                                    h.IsValidTarget() && ComboDamage(h) > h.Health))
                {
                    Ping(enemy.Position.To2D());
                }
        }

        static void Ping(Vector2 vector2)
        {
            if (Utils.TickCount - LastPingT < 30 * 1000)
            {
                return;
            }

            LastPingT = Utils.TickCount;
            PingLocation = vector2;
            SimplePing();

            Utility.DelayAction.Add(150, SimplePing);
            Utility.DelayAction.Add(300, SimplePing);
            Utility.DelayAction.Add(400, SimplePing);
            Utility.DelayAction.Add(800, SimplePing);
        }

        static void SimplePing()
        {
            Game.ShowPing(PingCategory.Fallback, PingLocation, true);
        }

        static void 连招()
        {
            var Combotarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical);

            if (Menu.Item("lzw").GetValue<bool>())
            {
                if (W.IsReady())
                {
                    if (Combotarget.IsValidTarget(W.Range))
                    {
                        if (getManaPer < Menu.Item("qxmp").GetValue<Slider>().Value)
                            CardSelect.StartSelecting(Cards.Blue);
                        else
                            CardSelect.StartSelecting(Cards.Yellow);
                    }
                }

                if (!W.IsReady() || Player.HasBuff("PickACard"))
                {
                    foreach (var target in ObjectManager.Get<Obj_AI_Hero>().Where
                        (target => !target.IsMe && target.Team != ObjectManager.Player.Team))
                        if (target.Health < W.GetDamage(target) && Player.Distance(target, true) < 600 &&
                            !target.IsDead && target.IsValidTarget())
                        {
                            CardSelect.StartSelecting(Cards.Blue);
                        }
                }
            }

            if (Menu.Item("lzq").GetValue<bool>())
            {
                if (Q.IsReady())
                {
                    if (Combotarget.IsValidTarget(Q.Range))
                    {
                        var Qpre = Q.GetPrediction(Combotarget);


                        if (Qpre.Hitchance >= HitChance.VeryHigh)
                        {
                            Q.Cast(Qpre.CastPosition);
                        }

                        if (Q.IsReady() &&
                            ((
                            Combotarget.HasBuffOfType(BuffType.Stun) ||
                            Combotarget.HasBuffOfType(BuffType.Snare) ||
                            Combotarget.HasBuffOfType(BuffType.Knockup)
                            ))
                            )
                        {
                            Q.CastIfHitchanceEquals(Combotarget, HitChance.VeryHigh, true);
                        }
                    }
                }
            }
        }

        static void 骚扰()
        {
            var target = TargetSelector.GetTarget(1300, TargetSelector.DamageType.Physical);
            if (Q.IsReady() && (Menu.Item("srq").GetValue<bool>()))
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

            if (Player.Distance(target.ServerPosition) < Player.AttackRange - 40 && !Menu.Item("srw").GetValue<bool>())
            {
                CardSelect.StartSelecting(Cards.Blue);
            }

            if (Player.Distance(target, true) < Player.AttackRange - 150 && !Menu.Item("srwr").GetValue<bool>())
            {
                CardSelect.StartSelecting(Cards.Red);
            }

        }

        static void 清线()
        {

            if (Q.IsReady() && Menu.Item("qxq").GetValue<bool>() && getManaPer > 40)
            {
                var allMinionsQ = MinionManager.GetMinions(Player.ServerPosition, Q.Range, MinionTypes.All, MinionTeam.Enemy);
                var locQ = Q.GetLineFarmLocation(allMinionsQ);

                if (locQ.MinionsHit >= 3)
                    Q.Cast(locQ.Position);
            }

            var minioncount = MinionManager.GetMinions(Player.Position, 1500).Count;

            if(!Menu.Item("qxw").GetValue<bool>())
            {
                if (minioncount > 0)
                {
                    if (getManaPer > Menu.Item("qxmp").GetValue<Slider>().Value)
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
        }

        static void 清野()
        {

            var mobs = MinionManager.GetMinions(Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(Player) + 50,
                MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (mobs.Count <= 0)
                return;

            if (Q.IsReady() && Menu.Item("qyq").GetValue<bool>() && getManaPer > 45)
            {
                Q.Cast(mobs[0].Position);
            }

            if (W.IsReady() && Menu.Item("qyw").GetValue<bool>())
            {
                if (getManaPer > Menu.Item("qymp").GetValue<Slider>().Value)
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

            if (!Menu.Item("KSQ").GetValue<bool>())
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

            if (Player.Spellbook.CanUseSpell(SpellSlot.Q) == SpellState.Ready && !Menu.Item("KSQ").GetValue<bool>())
                foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>())
                {
                    if (enemy.IsValidTarget(Q.Range * 2))
                    {
                        var pred = Q.GetPrediction(enemy);
                        if ((pred.Hitchance == HitChance.Immobile && !Menu.Item("KSQ").GetValue<bool>()))
                        {
                            Q.Cast(enemy);
                        }
                    }
                }
        }
    }
    //  This  is Esk0r CardSelect ~  GitHub:Github.com/Esk0r/LeagueSharp/
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
            ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
        }

        public static void StartSelecting(Cards card)
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && Status == SelectStatus.Ready)
            {
                Select = card;
                if (Utils.TickCount - LastWSent > 170 + Game.Ping / 2)
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, ObjectManager.Player);
                    LastWSent = Utils.TickCount;
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
