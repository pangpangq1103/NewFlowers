/*
 * Thx
 * xcsoft-Sharpshooter
 * Hellsing-Kalista
 * OKTW-AAStyle
 * jQuery-ELKalista
 * Wiezerzz-Balista
 */
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

namespace Flowers滑板鞋_重生_
{

    class KalistaM
    {
        public static Menu 菜单;
        public static void KalistaMenu()
        {
            菜单 = new Menu("Flowers-Kalista", "Lost.", true);

            var targetSelectorMenu = new Menu("Target Selector", "Target Selector");
            TargetSelector.AddToMenu(targetSelectorMenu);
            菜单.AddSubMenu(targetSelectorMenu);

            var orbwalkerMenu = new Menu("Orbwalker", "Orbwalker");
            lost.Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);
            菜单.AddSubMenu(orbwalkerMenu);

            菜单.AddSubMenu(new Menu("Combo", "Combo"));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzp", "Use Q", true).SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lze", "Use E", true).SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeee", "Use E KS", true).SetValue(true));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzeeeeee", "Max E Stack", true).SetValue(new Slider(5, 1, 20)));
            菜单.SubMenu("Combo").AddItem(new MenuItem("lzmp", "Combo Mana <=%", true).SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("Harass", "Harass"));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srq", "Use Q", true).SetValue(true));
            菜单.SubMenu("Harass").AddItem(new MenuItem("AutoQ", "Auto Q Harass", true).SetValue(new KeyBind("T".ToCharArray()[0], KeyBindType.Toggle)));
            菜单.SubMenu("Harass").AddItem(new MenuItem("srmp", "Harass Mana <=%", true).SetValue(new Slider(50, 0, 100)));

            菜单.AddSubMenu(new Menu("Clear", "Clear"));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxq", "Use Q LaneClear", true).SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxqqq", "Use Q Millions", true).SetValue(new Slider(3, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxe", "Use E LaneClear", true).SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxeee", "Use E Millions", true).SetValue(new Slider(2, 1, 5)));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qyq", "Use Q JungleClear", true).SetValue(false));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qye", "Use E JungleClear", true).SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("eqiangyeguai", "Use E Steal Jungle", true).SetValue(true));
            菜单.SubMenu("Clear").AddItem(new MenuItem("qxmp", "Clear Mana <=%", true).SetValue(new Slider(60, 0, 100)));

            菜单.AddSubMenu(new Menu("Item", "Item"));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseYUU", "Use Youmuu's Ghostblade", true).SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBRK", "Use Blade of the Ruined King", true).SetValue(true));
            菜单.SubMenu("Item").AddItem(new MenuItem("UseBC", "Use Bilgewater Cutlass", true).SetValue(true));

            菜单.AddSubMenu(new Menu("Flee", "Flee"));
            菜单.SubMenu("Flee").AddItem(new MenuItem("Flee", "Flee", true).SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press)));


            菜单.AddSubMenu(new Menu("Misc", "Misc"));
            菜单.SubMenu("Misc").AddItem(new MenuItem("DamageExxx", "Auto E -> When millions will die and hero has buff", true).SetValue(true));
            菜单.SubMenu("Misc").AddItem(new MenuItem("AutoW", "Auto W", true).SetValue(false));
            菜单.SubMenu("Misc").AddItem(new MenuItem("autowenemyclose", "Dont Send W with an enemy in X Range:", true).SetValue(new Slider(2000, 0, 5000)));
            菜单.SubMenu("Misc").AddItem(new MenuItem("harassEoutOfRange", "Use E when out of range", true).SetValue(true));
            菜单.SubMenu("Misc").AddItem(new MenuItem("KillSteal", "KillSteal", true).SetValue(true));



            菜单.AddSubMenu(new Menu("Drawings", "Drawing"));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingQ", "Q Range").SetValue(new Circle(true, Color.FromArgb(138, 101, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingW", "W Range").SetValue(new Circle(false, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingE", "E Range").SetValue(new Circle(true, Color.FromArgb(255, 0, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingR", "R Range").SetValue(new Circle(false, Color.FromArgb(0, 255, 0))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("DrawEDamage", "Drawing E Damage(From Kalima)").SetValue(new Circle(true, Color.FromArgb(202, 170, 255))));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("drawingAA", "Real AA Range(OKTW© Style)").SetValue(true));
            菜单.SubMenu("Drawing").AddItem(new MenuItem("orb", "AA Target(OKTW© Style)").SetValue(true));

            菜单.AddItem(new MenuItem("Credit", "Credit : NightMoon"));
            菜单.AddToMainMenu();
        }
    }
    internal class lost
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
        private int InitTime 
        { 
            get; set;
        }
        private bool IsJumpPossible 
        { 
            get; set; 
        }
        private Vector3 FleePosition 
        { 
            get; set; 
        }

        public const string ChampionName = "Kalista";

        public static Orbwalking.Orbwalker Orbwalker;

        public static readonly Item 弯刀 = LeagueSharp.Common.Data.ItemData.Bilgewater_Cutlass.GetItem();

        public static readonly Item 破败 = LeagueSharp.Common.Data.ItemData.Blade_of_the_Ruined_King.GetItem();

        public static readonly Item 幽梦 = LeagueSharp.Common.Data.ItemData.Youmuus_Ghostblade.GetItem();
        public static void Game_OnGameLoad(EventArgs args)
        {
            if (ObjectManager.Player.ChampionName != ChampionName)
            {
                return;
            }

            Notifications.AddNotification("Flowers Kalista by NightMoon", 2000);
            Notifications.AddNotification("`                  And  Lost`", 2000);
            Notifications.AddNotification("Version : 1.0.0.4", 2000);
            Q = new Spell(SpellSlot.Q, 1160f);
            W = new Spell(SpellSlot.W, 5000f);
            E = new Spell(SpellSlot.E, 1000f);
            R = new Spell(SpellSlot.R, 1500f);

            Q.SetSkillshot(0.25f, 35f, 1600f, true, SkillshotType.SkillshotLine);
            R.SetSkillshot(0.50f, 1500, float.MaxValue, false, SkillshotType.SkillshotCircle);

            KalistaM.KalistaMenu();

            Game.OnUpdate += 总菜单;
            Drawing.OnDraw += 范围显示;
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsMe && args.SData.Name == E.Instance.Name)
                Utility.DelayAction.Add(250, Orbwalking.ResetAutoAttackTimer);

            if (KalistaM.菜单.Item("Save", true).GetValue<Boolean>() && R.IsReady())
            {
                if (sender.Type == GameObjectType.obj_AI_Hero && sender.IsEnemy)
                {
                    var soulbound = HeroManager.Allies.FirstOrDefault(hero => hero.HasBuff("kalistacoopstrikeally") && args.Target.NetworkId == hero.NetworkId && hero.HealthPercent <= KalistaM.菜单.Item("jilaohp").GetValue<Slider>().Value);

                    if (soulbound != null)
                        R.Cast();
                }
            }
        }
        private static List<Drawline> drawlinelist = new List<Drawline>();
        private class Drawline
        {
            public string Name { get; set; }
            public double Timer { get; set; }
            public float Addedon { get; set; }
            //here goes the function stuff...
            public float X { get; set; }
            public float Y { get; set; }
            public float X2 { get; set; }
            public float Y2 { get; set; }
            public float Thickness { get; set; }
            public Color Color { get; set; }
        }
        public static void drawline(string name, float x, float y, float x2, float y2, float thickness, Color color)
        {
            drawlinelist.RemoveAll(xXx => xXx.Name == name);
            drawlinelist.Add(new Drawline() { Name = name, X = x, Y = y, X2 = x2, Y2 = y2, Thickness = thickness, Color = color });
            return;
        }
        static float? ondrawtimers;
        private static void 范围显示(EventArgs args)
        {
           if (Player.IsDead)
           {
               return;
           }
           var timerightnow = Game.ClockTime;
           drawlinelist.RemoveAll(x => timerightnow - x.Addedon > x.Timer);
           var AA范围OKTWStyle = KalistaM.菜单.Item("drawingAA").GetValue<bool>();
           var AA目标OKTWStyle = KalistaM.菜单.Item("orb").GetValue<bool>();
           var Q范围 = KalistaM.菜单.Item("drawingQ").GetValue<Circle>();
           var W范围 = KalistaM.菜单.Item("drawingW").GetValue<Circle>();
           var E范围 = KalistaM.菜单.Item("drawingE").GetValue<Circle>();
           var R范围 = KalistaM.菜单.Item("drawingR").GetValue<Circle>();

           var dEDmG = KalistaM.菜单.Item("DrawEDamage").GetValue<Circle>();
           if (dEDmG.Active && E.Level > 0)
           {
               var enemieswithspears = HeroManager.Enemies.Where(x => x.HasBuff("kalistaexpungemarker") && x.IsHPBarRendered);
               if (enemieswithspears != null)
               {
                   var barsize = 104f;
                   foreach (var enemy in enemieswithspears)
                   {
                       var health = enemy.Health;
                       var maxhealth = enemy.MaxHealth;
                       var pos = enemy.HPBarPosition;
                       var percent = E.GetDamage(enemy) / maxhealth * barsize;
                       var start = pos + (new Vector2(10f, 19f));
                       var end = pos + (new Vector2(10f + percent, 19f));

                       drawline("drawEdmg" + enemy.ChampionName, start[0], start[1], end[0], end[1], 4.0f, dEDmG.Color);
                   }
               }
           }

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


        }

        public static bool UseBotrk(Obj_AI_Hero target)
        {
            var 使用弯刀 = KalistaM.菜单.Item("UseBC", true).GetValue<Boolean>();
            var 使用破败 = KalistaM.菜单.Item("UseBRK", true).GetValue<Boolean>();

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
            var 使用幽梦 = KalistaM.菜单.Item("USEYUU", true).GetValue<Boolean>();

            if (使用幽梦 && 幽梦.IsReady() && target.IsValidTarget(Orbwalking.GetRealAutoAttackRange(Player) + 50))
            {
                return 幽梦.Cast();
            }
            return false;
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
                    骚扰();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    清线();
                    清野();
                    break;
            }

            if (KalistaM.菜单.Item("AutoQ").GetValue<KeyBind>().Active)
            {
                骚扰();
            }

            E抢野怪();
            自动E();
            逃跑();
            KSE();
            自动W();
            OutOfRangeE();
            KillSteal();
        }

        private static void KillSteal()
        {
            if (!KalistaM.菜单.Item("KillSteal", true).GetValue<Boolean>())
            {
                return;
            }
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(h => Q.CanCast(h) || E.CanCast(h)))
            {
                if (hasundyingbuff(enemy)) { continue; }
                var edmg = E.GetDamage(enemy);
                var enemyhealth = enemy.Health;
                var enemyregen = enemy.HPRegenRate / 2;
                if (((enemyhealth + enemyregen) <= edmg) && E.CanCast(enemy) && !hasundyingbuff(enemy))
                {
                    E.Cast(); 
                    return; 
                }
                if (Q.GetPrediction(enemy).Hitchance >= HitChance.High && Q.CanCast(enemy))
                {
                    var qdamage = Player.GetSpellDamage(enemy, SpellSlot.Q);
                    if ((qdamage + edmg) >= (enemyhealth + enemyregen))
                    {
                        Q.Cast(enemy);
                        return;
                    }
                }
            }
        }

        private static bool hasundyingbuff(Obj_AI_Hero enemy)
        {
            var hasbuff = HeroManager.Enemies.Find(a =>
                enemy.CharData.BaseSkinName == a.CharData.BaseSkinName && a.Buffs.Any(b =>
                    b.Name.ToLower().Contains("undying rage") ||
                    b.Name.ToLower().Contains("chrono shift") ||
                    b.Name.ToLower().Contains("judicatorintervention") ||
                    b.Name.ToLower().Contains("poppyditarget")));
            if (hasbuff != null) { return true; }
            return false;
        }

        private static void OutOfRangeE()
        {
               if (!KalistaM.菜单.Item("harassEoutOfRange", true).GetValue<Boolean>()) 
               {
                var Minions = MinionManager.GetMinions(Player.ServerPosition, R.Range, MinionTypes.All, MinionTeam.NotAlly)
                    .FindAll(x => (x.Health + (x.HPRegenRate / 2)) < E.GetDamage(x) && E
                   .CanCast(x));
                if (Minions != null) 
                {
                    var enemy = HeroManager.Enemies.Find(x => E.CanCast(x));
                    if (enemy != null) 
                    E.Cast(); 
                }
               }
        }

        public static float? autoWtimers;
        private static void 自动W()
        {
            var useW = KalistaM.菜单.Item("AutoW", true).GetValue<Boolean>();
            if (useW && W.IsReady()) {
                if (autoWtimers != null) {
                    if ((Game.ClockTime - autoWtimers) > 2) {
                        autoWtimers = null;
                    } else { return; }
                }
                var closestenemy = HeroManager.Enemies.Find(x => Player.ServerPosition.Distance(x.ServerPosition) 
                    < KalistaM.菜单.Item("autowenemyclose", true).GetValue<Slider>().Value);
                if (closestenemy != null) { return; }
                if ((Player.ManaPercent < 50) || Player.IsDashing() || Player.IsWindingUp || Player.InFountain()) { return; }
                fillsentinels();

                Random rnd = new Random();
                var sentineldestinations = _mysentinels.Where(s => !s.Name.Contains("RobotBuddy")).OrderBy(s => rnd.Next()).ToList();
                foreach (var destinations in sentineldestinations) {
                    var distancefromme = Vector3.Distance(Player.Position, destinations.Position);
                    if (sentinelcloserthan(destinations.Position, 1500) == 0 && distancefromme < W.Range) {
                        autoWtimers = Game.ClockTime;
                        W.Cast(destinations.Position);
                        Notifications.AddNotification(new Notification("sending bug to:" + destinations.Name, 5000).SetTextColor(Color.FromArgb(255, 0, 0)));
                        return;
                    }
                }
            }
        }

        private static int sentinelcloserthan(Vector3 vector3, int p)
        {
            foreach (var xxxXxxx in ObjectManager.Get<AttackableUnit>().Where(obj => obj.Name.Contains("RobotBuddy")))
            {
                if (Vector3.Distance(vector3
                    , xxxXxxx.Position) < p) 
                return 1;
            }
            return 0;
        }
        static readonly List<mysentinels> _mysentinels = new List<mysentinels>();
        internal class mysentinels
        {
            public string Name;
            public Vector3 Position;
            public mysentinels(string name, Vector3 position)
            {
                Name = name;
                Position = position;
            }
        }
        private static void fillsentinels()
        {
            _mysentinels.Clear();
            foreach (var xxxXxxx in ObjectManager.Get<AttackableUnit>().Where(obj => obj.Name.Contains("RobotBuddy")))
            {
                _mysentinels.Add(new mysentinels("RobotBuddy", xxxXxxx.Position));
            }
            _mysentinels.Add(new mysentinels("Blue Camp Blue Buff", (Vector3)SummonersRift.Jungle.Blue_BlueBuff));
            _mysentinels.Add(new mysentinels("Blue Camp Red Buff", (Vector3)SummonersRift.Jungle.Blue_RedBuff));
            _mysentinels.Add(new mysentinels("Red Camp Blue Buff", (Vector3)SummonersRift.Jungle.Red_BlueBuff));
            _mysentinels.Add(new mysentinels("Red Camp Red Buff", (Vector3)SummonersRift.Jungle.Red_RedBuff));
            _mysentinels.Add(new mysentinels("Dragon", (Vector3)SummonersRift.River.Dragon));
            _mysentinels.Add(new mysentinels("Baron", (Vector3)SummonersRift.River.Baron));
            _mysentinels.Add(new mysentinels("Mid Bot River", new Vector3(8370f, 6176f, -71.2406f)));
        }

        private static void KSE()
        {
            if (!KalistaM.菜单.Item("killsteal", true).GetValue<Boolean>() || !E.IsReady())
                return;

            var target = HeroManager.Enemies.FirstOrDefault(x => !x.HasBuffOfType(BuffType.Invulnerability) && !x.HasBuffOfType(BuffType.SpellShield) && E.CanCast(x) && (x.Health + (x.HPRegenRate / 2)) <= E.GetDamage(x));

            if (E.CanCast(target))
                E.Cast();
        }

        private static void 连招()
        {
            if (!(getManaPer > KalistaM.菜单.Item("lzmp",true).GetValue<Slider>().Value))
                return;

            if (KalistaM.菜单.Item("lzp", true).GetValue<Boolean>())
            {
                var Qtarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical, true);

                if (Q.CanCast(Qtarget) && Q.GetPrediction(Qtarget).Hitchance >= HitChance.VeryHigh && !Player.IsWindingUp && !Player.IsDashing())
                    Q.Cast(Qtarget);
            }

            var Minion = MinionManager.GetMinions(Player.ServerPosition, E.Range, MinionTypes.All, MinionTeam.Enemy).Where(x => x.Health <= E.GetDamage(x)).OrderBy(x => x.Health).FirstOrDefault();
            var Target = HeroManager.Enemies.Where(x => E.CanCast(x) && E.GetDamage(x) >= 1 && !x.HasBuffOfType(BuffType.Invulnerability) && !x.HasBuffOfType(BuffType.SpellShield)).OrderByDescending(x => E.GetDamage(x)).FirstOrDefault();
            var 连招E目标 = Target;
            if (KalistaM.菜单.Item("lze", true).GetValue<Boolean>() && (E.Instance.State == SpellState.Ready 
                || E.Instance.State == SpellState.Surpressed) && 连招E目标.HasBuffOfType(BuffType.SpellShield))
            {
                if (Player.Distance(连招E目标, true) > Math.Pow(Orbwalking.GetRealAutoAttackRange(连招E目标), 2))
                {
                    var minions = ObjectManager.Get<Obj_AI_Minion>().Where(m => m.IsValidTarget(Orbwalking.GetRealAutoAttackRange(m)));
                    if (minions.Any(m => m.IsRendKillable()))
                    {
                        E.Cast(true);
                    }
                    else
                    {
                        var minion = MinionManager.GetMinions(Player.ServerPosition, E.Range, 
                            MinionTypes.All, MinionTeam.Enemy).Find(m => m.Health > Player.GetAutoAttackDamage(m) 
                                && m.Health < Player.GetAutoAttackDamage(m) + E.GetDamage(m, 
                                (m.HasBuffOfType(BuffType.SpellShield) ? m.GetRendBuff().Count + 1 : 1)));
                        if (minion != null)
                        {
                            Orbwalker.ForceTarget(minion);
                        }
                    }
                }
                else if (E.IsInRange(连招E目标))
                {
                    if (连招E目标.IsRendKillable())
                    {
                        E.Cast(true);
                    }
                    else if (连招E目标.GetRendBuff().Count >= KalistaM.菜单.Item("lzeeeeee",true).GetValue<Slider>().Value)
                    {
                        if (连招E目标.ServerPosition.Distance(Player.ServerPosition, true) > Math.Pow(E.Range * 0.8, 2) ||
                            连招E目标.GetRendBuff().EndTime - Game.Time < 0.3)
                        {
                            E.Cast(true);
                        }
                    }
                }

                var eTarget = HeroManager.Enemies.Where(x => x.IsValidTarget(E.Range) && 
                    E.GetDamage(x) >= 1 && !x.HasBuffOfType(BuffType.Invulnerability) && 
                    !x.HasBuffOfType(BuffType.SpellShield)).OrderByDescending(x => E.GetDamage(x)).FirstOrDefault();

                if (eTarget != null && eTarget.Health <= E.GetDamage(eTarget))
                    E.Cast();
            }

            if (Target.Health <= E.GetDamage(Target) || (E.CanCast(Minion) && E.CanCast(Target)))
                E.Cast();

            if (!KalistaM.菜单.Item("lzeee", true).GetValue<Boolean>() || !E.IsReady())
            {
                return;
            }

            var 连招E目标1 = HeroManager.Enemies.FirstOrDefault(x => !x.HasBuffOfType
                (BuffType.Invulnerability) && !x.HasBuffOfType(BuffType.SpellShield) 
                && E.CanCast(x) && (x.Health + (x.HPRegenRate / 2)) <= E.GetDamage(x));

            if (E.CanCast(连招E目标1))
            {
                E.Cast();
            }
        }
        private static void 骚扰()
        {
            if (!(getManaPer > KalistaM.菜单.Item("srmp",true).GetValue<Slider>().Value))
                return;

            if (KalistaM.菜单.Item("srq", true).GetValue<Boolean>())
            {
                var Qtarget = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Physical, true);

                if (Q.CanCast(Qtarget) && Q.GetPrediction(Qtarget).Hitchance >= HitChance.VeryHigh && !Player.IsWindingUp && !Player.IsDashing())
                    Q.Cast(Qtarget);
            }
        }
        private static void 清野()
        {
            if (!Orbwalking.CanMove(1) || !(getManaPer > KalistaM.菜单.Item("qxmp",true).GetValue<Slider>().Value))
                return;

            var Mobs = MinionManager.GetMinions(Player.ServerPosition, Orbwalking.GetRealAutoAttackRange(Player) + 100, 
                MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth);

            if (Mobs.Count <= 0)
                return;

            if (KalistaM.菜单.Item("qyq", true).GetValue<Boolean>() && Q.CanCast(Mobs[0]))
                Q.Cast(Mobs[0]);

            if (KalistaM.菜单.Item("qye", true).GetValue<Boolean>() && E.CanCast(Mobs[0]))
            {
                if (Mobs[0].Health + (Mobs[0].HPRegenRate / 2) <= E.GetDamage(Mobs[0]))
                    E.Cast();
            }
        }
        private static void 清线()
        {

            if (!Orbwalking.CanMove(1) || !(getManaPer > KalistaM.菜单.Item("qxmp",true).GetValue<Slider>().Value))
                return;

            var Minions = MinionManager.GetMinions(Player.ServerPosition, E.Range, MinionTypes.All, MinionTeam.Enemy);

            if (Minions.Count <= 0)
                return;

            if (KalistaM.菜单.Item("qxq", true).GetValue<Boolean>() && Q.IsReady())
            {
                foreach (var minion in Minions.Where(x => x.Health <= Q.GetDamage(x)))
                {
                    var killcount = 0;

                    foreach (var colminion in Q_GetCollisionMinions
                        (Player, Player.ServerPosition.Extend(minion.ServerPosition, Q.Range)))
                    {
                        if (colminion.Health <= Q.GetDamage(colminion))
                            killcount++;
                        else
                            break;
                    }

                    if (killcount >= KalistaM.菜单.Item("qxqqq",true).GetValue<Slider>().Value)
                    {
                        if (!Player.IsWindingUp && !Player.IsDashing())
                        {
                            Q.Cast(minion.ServerPosition);
                            break;
                        }
                    }
                    if (KalistaM.菜单.Item("qxe", true).GetValue<Boolean>() && E.IsReady())
                    {
                        return;
                    }
                    var minionkillcount = Minions.Count(x => E.CanCast(x) && x.Health <= E.GetDamage(x));
                    if (minionkillcount >= KalistaM.菜单.Item("qxeee", true).GetValue<Slider>().Value)
                        E.Cast();
                }
            }
        }

        private static List<Obj_AI_Base> Q_GetCollisionMinions(Obj_AI_Hero source, Vector3 targetposition)
        {
            var input = new PredictionInput
            {
                Unit = source,
                Radius = Q.Width,
                Delay = Q.Delay,
                Speed = Q.Speed,
            };

            input.CollisionObjects[0] = CollisionableObjects.Minions;

            return LeagueSharp.Common.Collision.GetCollision
                (new List<Vector3> { targetposition }, input)
                .OrderBy(obj => obj.Distance(source, false)).ToList();
        }

        private static void E抢野怪()
        {
            if (!KalistaM.菜单.Item("eqiangyeguai", true).GetValue<Boolean>() || !E.IsReady())
            {
                return;
            }

            if (MinionManager.GetMinions(Player.ServerPosition, E.Range, MinionTypes.All, MinionTeam.Neutral, 
                MinionOrderTypes.MaxHealth).Any(x => E.IsKillable(x)))
                E.Cast();

            if (MinionManager.GetMinions(Player.ServerPosition, E.Range, MinionTypes.All, MinionTeam.Enemy,
                MinionOrderTypes.MaxHealth).Any(x => E.IsKillable(x) && (x.SkinName.ToLower().Contains("siege") 
                    || x.SkinName.ToLower().Contains("super"))))
                E.Cast();
        }

        private static void 自动E()
        {
            var 敌人 = HeroManager.Enemies.Where(o => o.HasBuffOfType(BuffType.SpellShield)).OrderBy(o => 
                o.Distance(Player, true)).FirstOrDefault();

            if (!KalistaM.菜单.Item("DamageExxx", true).GetValue<Boolean>() || !E.IsReady())
            {
                if (敌人 != null)
                {
                    if (敌人.Distance(Player, true) < Math.Pow(E.Range + 200, 2))
                    {
                        if (ObjectManager.Get<Obj_AI_Minion>().Any(o => o.IsRendKillable() && E.IsInRange(o)))
                        {
                            E.Cast();
                        }
                        return;
                    }
                }
            }

            if (MinionManager.GetMinions(E.Range, MinionTypes.All, MinionTeam.Enemy).Any(x => E.IsKillable(x))
                && HeroManager.Enemies.Any(x => x.IsValidTarget(E.Range) && E.GetDamage(x) >= 1 &&
                    !x.HasBuffOfType(BuffType.Invulnerability) && !x.HasBuffOfType(BuffType.SpellShield)))
            {
                E.Cast();
            }
        }

        private static void 逃跑()
        {
            var 逃跑 = KalistaM.菜单.Item("Flee",true).GetValue<KeyBind>().Active;
            var Target = Vector3.Zero;
            var FleePosition = Vector3.Zero;
            var InitTime = 0;

            if (Target != Vector3.Zero)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, Target);

                if (Environment.TickCount - InitTime > 500)
                {
                    Target = Vector3.Zero;
                    InitTime = 0;
                }
                else
                {
                    return;
                }
            }

            if (逃跑)
            {
                var dashObjects = Flee.GetDashObjects();
                Orbwalking.Orbwalk(dashObjects.Count > 0 ? dashObjects[0] : null, Game.CursorPos);
            }
            if (逃跑)
            {
                var wallCheck = Flee.GetFirstWallPoint(Player.Position, Game.CursorPos);

                if (wallCheck != null)
                {
                    wallCheck = Flee.GetFirstWallPoint((Vector3)wallCheck, Game.CursorPos, 5);
                }

                Vector3 movePosition = wallCheck != null ? (Vector3)wallCheck : Game.CursorPos;

                var tempGrid = NavMesh.WorldToGrid(movePosition.X, movePosition.Y);
                FleePosition = NavMesh.GridToWorld((short)tempGrid.X, (short)tempGrid.Y);

                Obj_AI_Base target = null;

                if (逃跑)
                {
                    var dashObjects = Flee.GetDashObjects();
                    if (dashObjects.Count > 0)
                    {
                        target = dashObjects[0];
                    }
                }

                if (Q.IsReady() && wallCheck != null)
                {
                    Vector3 wallPosition = movePosition;

                    Vector2 direction = (Game.CursorPos.To2D() - wallPosition.To2D()).Normalized();
                    float maxAngle = 80;
                    float step = maxAngle / 20;
                    float currentAngle = 0;
                    float currentStep = 0;
                    bool jumpTriggered = false;
                    while (true)
                    {
                        if (currentStep > maxAngle && currentAngle < 0)
                        {
                            break;
                        }

                        if ((currentAngle == 0 || currentAngle < 0) && currentStep != 0)
                        {
                            currentAngle = (currentStep) * (float)Math.PI / 180;
                            currentStep += step;
                        }
                        else if (currentAngle > 0)
                        {
                            currentAngle = -currentAngle;
                        }

                        Vector3 checkPoint;

                        if (currentStep == 0)
                        {
                            currentStep = step;
                            checkPoint = wallPosition + 300 * direction.To3D();
                        }
                        else
                        {
                            checkPoint = wallPosition + 300 * direction.Rotated(currentAngle).To3D();
                        }

                        if (!checkPoint.IsWall())
                        {
                            wallCheck = Flee.GetFirstWallPoint(checkPoint, wallPosition);
                            if (wallCheck != null)
                            {
                                Vector3 wallPositionOpposite = (Vector3)Flee.GetFirstWallPoint((Vector3)wallCheck, wallPosition, 5);

                                if (Player.GetPath(wallPositionOpposite).ToList().To2D().PathLength() - Player.Distance(wallPositionOpposite) > 200)
                                {
                                    if (Player.Distance(wallPositionOpposite, true) < Math.Pow(300 - Player.BoundingRadius / 2, 2))
                                    {
                                        InitTime = Environment.TickCount;
                                        Target = wallPositionOpposite;
                                        Q.Cast(wallPositionOpposite);
                                        jumpTriggered = true;
                                        break;
                                    }
                                    else
                                    {

                                    }
                                }
                            }
                        }
                    }

                    if (!jumpTriggered)
                    {
                        Orbwalking.Orbwalk(target, movePosition, 90f, 0f, true, true);
                    }
                }
                else
                {
                    Orbwalking.Orbwalk(target, movePosition, 90f, 0f, true, true);
                }
            }
        }
    }

    public class Flee
    {
        private static readonly Obj_AI_Hero Player = ObjectManager.Player;
        public static bool IsLyingInCone(Vector2 position, Vector2 apexPoint, Vector2 circleCenter, double aperture)
        {
            double halfAperture = aperture / 2;

            Vector2 apexToXVect = apexPoint - position;

            Vector2 axisVect = apexPoint - circleCenter;

            bool isInInfiniteCone = DotProd(apexToXVect, axisVect) / Magn(apexToXVect) / Magn(axisVect) > Math.Cos(halfAperture);

            if (!isInInfiniteCone)
                return false;

            bool isUnderRoundCap = DotProd(apexToXVect, axisVect) / Magn(axisVect) < Magn(axisVect);

            return isUnderRoundCap;
        }

        private static float DotProd(Vector2 a, Vector2 b)
        {
            return a.X * b.X + a.Y * b.Y;
        }

        private static float Magn(Vector2 a)
        {
            return (float)(Math.Sqrt(a.X * a.X + a.Y * a.Y));
        }


        public static Vector2? GetFirstWallPoint(Vector3 from, Vector3 to, float step = 25)
        {
            return GetFirstWallPoint(from.To2D(), to.To2D(), step);
        }

        public static Vector2? GetFirstWallPoint(Vector2 from, Vector2 to, float step = 25)
        {
            var direction = (to - from).Normalized();

            for (float d = 0; d < from.Distance(to); d = d + step)
            {
                var testPoint = from + d * direction;
                var flags = NavMesh.GetCollisionFlags(testPoint.X, testPoint.Y);
                if (flags.HasFlag(CollisionFlags.Wall) || flags.HasFlag(CollisionFlags.Building))
                {
                    return from + (d - step) * direction;
                }
            }

            return null;
        }

        public static List<Obj_AI_Base> GetDashObjects(IEnumerable<Obj_AI_Base> predefinedObjectList = null)
        {
            List<Obj_AI_Base> objects;
            if (predefinedObjectList != null)
                objects = predefinedObjectList.ToList();
            else
                objects = ObjectManager.Get<Obj_AI_Base>().Where(o => o.IsValidTarget(Orbwalking.GetRealAutoAttackRange(o))).ToList();

            var apexPoint = Player.ServerPosition.To2D() + (Player.ServerPosition.To2D() - Game.CursorPos.To2D()).Normalized() * Orbwalking.GetRealAutoAttackRange(Player);

            return objects.Where(o => Flee.IsLyingInCone(o.ServerPosition.To2D(), apexPoint, Player.ServerPosition.To2D(), Math.PI)).OrderBy(o => o.Distance(apexPoint, true)).ToList();
        }
    }

    static class Killable
    {
        public static readonly Obj_AI_Hero Player = ObjectManager.Player;
        public static AttackableUnit AfterAttackTarget
        {
            get;
            private set;
        }
        public static Orbwalking.Orbwalker Orbwalker
        {
            get { return Orbwalker; }
        }
        public static Spell E
        {
            get;
            private set;
        }

        public static bool HasUBuff(this Obj_AI_Hero target)
        {
            if (target.ChampionName == "Tryndamere" &&
                target.Buffs.Any(b => b.Caster.NetworkId == target.NetworkId && b.IsValidBuff() && b.DisplayName == "Undying Rage"))
            {
                return true;
            }

            if (target.Buffs.Any(b => b.IsValidBuff() && b.DisplayName == "Chrono Shift"))
            {
                return true;
            }

            if (target.Buffs.Any(b => b.IsValidBuff() && b.DisplayName == "JudicatorIntervention"))
            {
                return true;
            }

            if (target.ChampionName == "Poppy")
            {
                if (HeroManager.Allies.Any(o =>
                    !o.IsMe &&
                    o.Buffs.Any(b => b.Caster.NetworkId == target.NetworkId && b.IsValidBuff() && b.DisplayName == "PoppyDITarget")))
                {
                    return true;
                }
            }

            return false;
        }

        private static float[] rawRendDamage = new float[] { 20, 30, 40, 50, 60 };
        private static float[] rawRendDamageMultiplier = new float[] { 0.6f, 0.6f, 0.6f, 0.6f, 0.6f };
        private static float[] rawRendDamagePerSpear = new float[] { 10, 14, 19, 25, 32 };
        private static float[] rawRendDamagePerSpearMultiplier = new float[] { 0.2f, 0.225f, 0.25f, 0.275f, 0.3f };
        public static BuffInstance GetRendBuff(this Obj_AI_Base target)
        {
            return target.Buffs.Find(b => b.Caster.IsMe && b.IsValidBuff() && b.DisplayName == "KalistaExpungeMarker");
        }


        public static bool IsRendKillable(this Obj_AI_Base target)
        {
            var hero = target as Obj_AI_Hero;
            return GetRendDamage(target) > target.Health && (hero == null || !hero.HasUBuff());
        }

        public static float GetRendDamage(Obj_AI_Hero target)
        {
            return (float)GetRendDamage(target, -1);
        }

        public static float GetRendDamage(Obj_AI_Base target, int customStacks = -1)
        {
            // Calculate the damage and return
            return ((float)Player.CalcDamage(target, Damage.DamageType.Physical, GetRawRendDamage(target, customStacks)) - 20) * 0.98f;
        }

        public static float GetRawRendDamage(Obj_AI_Base target, int customStacks = -1)
        {
            // Get buff
            var buff = target.GetRendBuff();

            if (buff != null || customStacks > -1)
            {
                return (rawRendDamage[E.Level - 1] + rawRendDamageMultiplier[E.Level - 1] * Player.TotalAttackDamage()) + // Base damage
                       ((customStacks < 0 ? buff.Count : customStacks) - 1) * // Spear count
                       (rawRendDamagePerSpear[E.Level - 1] + rawRendDamagePerSpearMultiplier[E.Level - 1] * Player.TotalAttackDamage()); // Damage per spear
            }

            return 0;
        }
    }
}
