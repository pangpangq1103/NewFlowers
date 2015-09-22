using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Linq;
using Color = System.Drawing.Color;

namespace Flowers_Vayne
{
    class FlowersVayne
    {
        public static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        public static void Game_OnGameLoad(EventArgs args)
        {
            switch(Player.CharData.BaseSkinName)
            {
                case("Vayne"):
                    {
                        Vayne();
                        break;
                    }
            }
        }

        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static Orbwalking.Orbwalker Orbwalker;
        public static Menu Menu;
        public static Items.Item WardS = new Items.Item(2043, 600f);
        public static Items.Item WardN = new Items.Item(2044, 600f);
        public static Items.Item TrinketN = new Items.Item(3340, 600f);
        public static float WardTime = 0;
        public static Vector3 positionWard;
        public static Obj_AI_Hero WardTarget;
        public static Vector3 TumbleOrder;
        public static Spell Q;
        public static Spell W;
        public static Spell E;
        public static Spell R;
        public static SpellSlot Flash;
        public static Spell trinketSpell;
        public static Vector3 TumbleOrderPos = Vector3.Zero;
        public static bool _canWallTumble;
        public static Vector3 _dragPreV3 = new Vector2(12050, 4828).To3D();
        public static Vector3 _dragAftV3 = new Vector2(11510, 4470).To3D();
        public static Vector3 _midPreV3 = new Vector2(6962, 8952).To3D();
        public static Vector3 _midAftV3 = new Vector2(6667, 8794).To3D();
        public static Vector3 position;

        public static void Vayne()
        {
            LoadMenu();
            LoadSpell();
            Helpers.Load();
            _canWallTumble = (Utility.Map.GetMap().Type == Utility.Map.MapType.SummonersRift);
            TumbleOrderPos = position;
            Notification();
        }

        private static void Notification()
        {
            Game.PrintChat("Flowers " +Player.CharData.BaseSkinName + " Loaded!");
            Game.PrintChat("Credit : NightMoon!");
            Notifications.AddNotification("Emmm .", 10000);
            Notifications.AddNotification("You Don't Need Luck", 10000);
            Notifications.AddNotification("Because it was so perfect", 10000);
            Notifications.AddNotification("Credit : NightMoon", 10000);
        }

        public static void LoadSpell()
        {
            Q = new Spell(SpellSlot.Q, 300f);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 650f);
            E.SetTargetted(0.25f, 1600f);
            R = new Spell(SpellSlot.R);
            trinketSpell = new Spell(SpellSlot.Trinket);
            Flash = ObjectManager.Player.GetSpellSlot("summonerflash");
        }

        public static void LoadMenu()
        {
            Menu = new Menu("花边-VN-Flowers", "FLVN", true);

            var tsMenu = new Menu("[FL] Target Selector", "nightmoon.targetselect.menu");
            TargetSelector.AddToMenu(tsMenu);
            Menu.AddSubMenu(tsMenu);

            var orbMenu = new Menu("[FL] Orbwalker", "nightmoon.orbwalker.menu");
            Orbwalker = new Orbwalking.Orbwalker(orbMenu);
            Menu.AddSubMenu(orbMenu);

            var QSetting = new Menu("[FL] Use Q", "nightmoon.q.setting");
            QSetting.AddItem(new MenuItem("nightmoon.q.combo", "Use Q").SetValue(true));//Done
            QSetting.AddItem(new MenuItem("nightmoon.q.check", "Q Check").SetValue(true));//Done
            QSetting.AddItem(new MenuItem("nightmoon.q.tomouse", "Q To Mouse").SetValue(false));//Done
            QSetting.AddItem(new MenuItem("nightmoon.q.afteraa", "AA - Q - AA").SetValue(true));//Done
            Menu.AddSubMenu(QSetting);

            var ESetting = new Menu("[FL] Use E", "nightmoon.e.setting");
            ESetting.AddItem(new MenuItem("nightmoon.e.use", "Use E").SetValue(true));//Done
            ESetting.AddItem(new MenuItem("nightmoon.e.mode", "E Mode").SetValue
                (new StringList(new[] { "Flowers", "FlowersText", "Marksman", "SharpShooter", "VnHunterReborn" })));//Done
            ESetting.AddItem(new MenuItem("nightmoon.e.pushdistance", "E Push Distance").SetValue(new Slider(425, 350, 500)));//Done
            ESetting.AddItem(new MenuItem("nightmoon.e.fash", "E Push Fash").SetValue(true));//Done
            ESetting.AddItem(new MenuItem("nightmoon.e.kill", "E KillSteal").SetValue(true));//Done
            Menu.AddSubMenu(ESetting);

            var RSetting = new Menu("[FL] Use R", "nightmoon.r.setting");
            RSetting.AddItem(new MenuItem("nightmoon.r.use", "Use R Smart").SetValue(true));//Done
            RSetting.AddItem(new MenuItem("nightmoon.r.count", "Emeries Counts").SetValue(new Slider(2, 1, 5)));//Done
            //RSetting.AddItem(new MenuItem("nightmoon.r.range", "Emeries in Range").SetValue(new Slider(650, 400, 2000)));//ToDo
            Menu.AddSubMenu(RSetting);

            var Ward = new Menu("[FL] AntiBitch", "nightmoon.ward.setting");
            //Ward.AddItem(new MenuItem("nightmoon.blue.buy", "Auto Buy Blue Tracket").SetValue(true));//ToDo
            //Ward.AddItem(new MenuItem("nightmoon.blue.level", "Blue Trinket Level").SetValue(new Slider(6, 1, 18)));//ToDo
            //Ward.AddItem(new MenuItem("nightmoon.pink.use", "Auto Pink Ward").SetValue(true));//ToDo
            Ward.AddItem(new MenuItem("nightmoon.ward.use", "Auto Ward Enemy In Grass").SetValue(true));//Done
            Ward.AddItem(new MenuItem("nightmoon.ward.combo", "Only Combo").SetValue(false));//Done
            Menu.AddSubMenu(Ward);

            var Misc = new Menu("[FL] Misc", "nightmoon.misc.setting");
            Misc.AddItem(new MenuItem("nightmoon.e.antigapcloser", "AntiGapcloser!").SetValue(true));//Done
            Misc.AddItem(new MenuItem("nightmoon.e.interrupter", "Interrupter!").SetValue(true));//Done
            Misc.AddItem(new MenuItem("nightmoon.q.autowall", "Auto Wall Tumble!").SetValue(true));//Done
            Misc.AddItem(new MenuItem("nightmoon.q.wall", "Wall Tumble")).SetValue(new KeyBind("Z".ToCharArray()[0], KeyBindType.Press));//Done
            Menu.AddSubMenu(Misc);

            var EvadeSetting = new Menu("[FL] Evade", "nightmoon.evade.setting");//ToDo
            EvadeSetting.AddItem(new MenuItem("nightmoon.evade.soon", "Soon"));
            /*
            EvadeSetting.AddItem(new MenuItem("nightmoon.q.evade", "Use Q").SetValue(true));
            EvadeSetting.AddItem(new MenuItem("nightmoon.evade.enable", "Enable").SetValue(true));*/
            Menu.AddSubMenu(EvadeSetting);

            var Drawings = new Menu("[FL] Drawings", "nightmoon.drawing.setting");
            Drawings.AddItem(new MenuItem("nightmoon.e.range", "Draw E Range").SetValue
                (new Circle(false, System.Drawing.Color.FromArgb(255, 255, 0))));//Done
            Drawings.AddItem(new MenuItem("nightmoon.wall.range", "Draw Wall Tumble").SetValue
                (new Circle(true, Color.IndianRed)));//Done
            Menu.AddSubMenu(Drawings);

            Menu.AddItem(new MenuItem("Credit", "Credit : NightMoon"));

            Menu.AddToMainMenu();


            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Obj_AI_Hero.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
        }

        static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (Player.CountEnemiesInRange(1000) >=
                FlowersVayne.Menu.Item("nightmoon.r.count").GetValue<Slider>().Value)
            {
                if (!R.IsReady())
                {
                    R.Cast();
                }
            }

            /*if (sender.IsMe && args.SData.Name.ToLower().Contains("condemn") && args.Target.IsValid<Obj_AI_Hero>())
            {
                var target = (Obj_AI_Hero)args.Target;
                if (FlowersVayne.Menu.Item("EQ").GetValue<bool>() && target.IsVisible && !target.HasBuffOfType(BuffType.Stun) && Q.IsReady()) 
                {
                    var tumblePos = target.GetTumblePos();
                    CastQ(tumblePos);
                }
            }*/
        }

        static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (FlowersVayne.Menu.Item("nightmoon.e.antigapcloser").GetValue<bool>() || Player.IsDead)
                return;

            if (gapcloser.Sender.IsValidTarget(1000))
            {
                var targetpos = Drawing.WorldToScreen(gapcloser.Sender.Position);
            }

            if (E.CanCast(gapcloser.Sender))
                E.Cast(gapcloser.Sender);

            if (Q.IsReady() && gapcloser.End.Distance(Player.ServerPosition) < 300)
            {
                TumbleOrder = gapcloser.Sender.GetTumblePos();
                Q.Cast(TumbleOrder);
            }

        }

        static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (FlowersVayne.Menu.Item("nightmoon.e.interrupter").GetValue<bool>() || Player.IsDead)
                return;

            if (sender.IsValidTarget(1000))
            {
                var targetpos = Drawing.WorldToScreen(sender.Position);
                if (E.CanCast(sender))
                    E.Cast(sender);
            }

            if (args.DangerLevel == Interrupter2.DangerLevel.High &&
                sender.IsValidTarget(E.Range) &&
                (sender is Obj_AI_Hero))
            {
                E.Cast(sender);
            }
        }

        static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if (!args.Unit.IsMe) 
                return;

            if (args.Unit.IsMe || Q.IsReady())
            {
                if (Helpers.UltActive() && Helpers.TumbleActive() &&
                    FlowersVayne.Menu.Item("nightmoon.q.check").GetValue<bool>() &&
                    Helpers.EnemyHeroes.Any(h => h.IsMelee && h.Distance(Player) < h.AttackRange + h.BoundingRadius))
                {
                    args.Process = false;
                }
            }

            if (args.Unit.IsMe && Q.IsReady() && FlowersVayne.Menu.Item("nightmoon.q.use").GetValue<bool>())
            {
                if (args.Target.IsValid<Obj_AI_Hero>())
                {
                    var target = (Obj_AI_Hero) args.Target;
                    if (FlowersVayne.Menu.Item("nightmoon.r.use").GetValue<bool>() && R.IsReady() &&
                        Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
                    {
                        if (!target.UnderTurret(true))
                        {
                            R.Cast();
                        }
                    }
                }
            }

            if(Player.HasBuff("vayneinquisition") && !Q.IsReady())
            {
                Q.Cast(Game.CursorPos);
            }
        }

        static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if (!unit.IsMe)
            { 
                return; 
            }

            if (FlowersVayne.Menu.Item("nightmoon.q.combo").GetValue<bool>() &&
                FlowersVayne.Menu.Item("nightmoon.q.tomouse").GetValue<bool>() ||
                FlowersVayne.Menu.Item("nightmoon.q.combo").GetValue<bool>())
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if(Player.ManaPercent <= 35)
                    {
                        if (!Q.IsReady())
                        {
                            Q.Cast(Game.CursorPos);
                        }
                    }
                }
            }

 /*           if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                if (FlowersVayne.Menu.Item("nightmoon.q.combo").GetValue<bool>() &&
                    FlowersVayne.Menu.Item("nightmoon.q.tomouse").GetValue<bool>())
                {

                }

            }*/



            if (FlowersVayne.Menu.Item("nightmoon.q.combo").GetValue<bool>() && FlowersVayne.Menu.Item("nightmoon.q.tomouse").GetValue<bool>())
            {
                if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    if(!Q.IsReady())
                    {
                        Q.Cast(Game.CursorPos);
                    }
                }
            }

            if (unit.IsMe && FlowersVayne.Menu.Item("nightmoon.q.combo").GetValue<bool>() && 
                Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo ||
                unit.IsMe && FlowersVayne.Menu.Item("nightmoon.q.combo").GetValue<bool>() &&
                Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                if (FlowersVayne.Menu.Item("nightmoon.q.afteraa").GetValue<bool>())
                {
                    Q.Cast(Game.CursorPos);
                }
        }

        static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
            { 
                return; 
            }

            if (FlowersVayne.Menu.Item("nightmoon.wall.range").GetValue<Circle>().Active)
            {
                if (_canWallTumble && Player.Distance(_dragPreV3) < 3000)
                    Drawing.DrawCircle(_dragPreV3, 80, FlowersVayne.Menu.Item("nightmoon.wall.range").GetValue<Circle>().Color);
                if (_canWallTumble && Player.Distance(_midPreV3) < 3000)
                    Drawing.DrawCircle(_midPreV3, 80, FlowersVayne.Menu.Item("nightmoon.wall.range").GetValue<Circle>().Color);
            }

            if (E.IsReady() && 
                FlowersVayne.Menu.Item("nightmoon.e.range").GetValue<Circle>().Active)
            {
                Render.Circle.DrawCircle(Player.Position, E.Range,
                    FlowersVayne.Menu.Item("nightmoon.e.range").GetValue<Circle>().Color);
            }

        }
        static void Game_OnUpdate(EventArgs args)
        {
            if (FlowersVayne.Menu.Item("nightmoon.e.use").GetValue<bool>() && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                useE();
            }

            if (FlowersVayne.Menu.Item("nightmoon.e.kill").GetValue<bool>())
            {
                killsteale();
            }

            if (FlowersVayne.Menu.Item("nightmoon.q.combo").GetValue<bool>() && Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                SmartQ();
            }

            if (FlowersVayne.Menu.Item("nightmoon.ward.use").GetValue<bool>() ||
                (FlowersVayne.Menu.Item("nightmoon.ward.use").GetValue<bool>() &&
                 FlowersVayne.Menu.Item("nightmoon.ward.combo").GetValue<bool>() &&
                 Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo))
            {
                FuckBitchgotoglass();
            }

            if (FlowersVayne.Menu.Item("nightmoon.q.wall").GetValue<bool>() || 
                FlowersVayne.Menu.Item("nightmoon.q.autowall").GetValue<KeyBind>().Active)
            {
                if (_canWallTumble && Q.IsReady() && Player.Distance(_dragPreV3) < 500)
                {
                    DragWallTumble();
                }

                if (_canWallTumble && Q.IsReady() && Player.Distance(_midPreV3) < 500)
                {
                    MidWallTumble();
                }
            }
        }
        /// <summary>
        /// Wall Tumble in mid
        /// </summary>
        private static void MidWallTumble()
        {
            if (Player.Distance(_midPreV3) < 115)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, _midPreV3.Randomize(0, 1));
            }
            if (Player.Distance(_midPreV3) < 5)
            {
                Orbwalker.SetMovement(false);
                TumbleOrder = _midAftV3;
                Q.Cast(TumbleOrder);
                Utility.DelayAction.Add(100 + Game.Ping / 2, () =>
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, _midAftV3.Randomize(0, 1));
                    Orbwalker.SetMovement(true);
                });
            }
        }
        /// <summary>
        /// CastQ
        /// </summary>
        /// <param name="position"></param>
        public static void CastQ(Vector3 position)
        {
            if (position != Vector3.Zero)
            {
                Q.Cast(TumbleOrderPos);
            }
        }
        /// <summary>
        /// Wall Tumble in drag
        /// </summary>
        private static void DragWallTumble()
        {
            if (Player.Distance(_dragPreV3) < 115)
            {
                Player.IssueOrder(GameObjectOrder.MoveTo, _dragPreV3.Randomize(0, 1));
            }
            if (Player.Distance(_dragPreV3) < 5)
            {
                Orbwalker.SetMovement(false);
                TumbleOrder = _dragAftV3;
                Q.Cast(TumbleOrder);
                Utility.DelayAction.Add(100 + Game.Ping / 2, () =>
                {
                    Player.IssueOrder(GameObjectOrder.MoveTo, _dragAftV3.Randomize(0, 1));
                    Orbwalker.SetMovement(true);
                });
            }
        }
        /// <summary>
        /// Auto E KS
        /// </summary>
        private static void killsteale()
        {
            foreach (var target in HeroManager.Enemies.OrderByDescending(x => x.Health))
            {
                if (E.CanCast(target) && 
                    E.IsKillable(target))
                {
                    E.Cast(target);
                }
            }
        }
        /// <summary>
        /// How to Use Q
        /// </summary>
        private static void SmartQ()
        {
            if (Player.CountEnemiesInRange(850) <= 2)
            {
                if (Q.IsReady())
                {
                    var target0 = TargetSelector.GetTarget(550, TargetSelector.DamageType.Physical);
                    if (target0.Buffs.Any(buff => buff.Name == "vaynesilvereddebuff" && buff.Count == 2))
                    {
                        Q.Cast(Game.CursorPos);
                    }
                }
            }
            else 
            {
                if (Q.IsReady())
                {
                    foreach (var en in
                    HeroManager.Enemies.Where(
                       hero =>
                           hero.IsValidTarget(550)))
                    {
                        Q.Cast(Game.CursorPos);
                    }
                }
            }

            if (Q.IsReady())
            {
                var currentTarget = TargetSelector.GetTarget
                    (Orbwalking.GetRealAutoAttackRange(null) + 240f, TargetSelector.DamageType.Physical);
                if (!currentTarget.IsValidTarget())
                {
                    return;
                }

                if (currentTarget.ServerPosition.Distance(ObjectManager.Player.ServerPosition) <=
                    Orbwalking.GetRealAutoAttackRange(null))
                {
                    return;
                }

                if (currentTarget.Health + 15 <
                    ObjectManager.Player.GetAutoAttackDamage(currentTarget) +
                    Q.GetDamage(currentTarget))
                {
                    var extendedPosition = ObjectManager.Player.ServerPosition.Extend(
                        currentTarget.ServerPosition, 300f);
                    if (Helpers.OkToQ(extendedPosition))
                    {
                        Q.Cast(extendedPosition);
                        Orbwalking.ResetAutoAttackTimer();
                        Orbwalker.ForceTarget(currentTarget);
                    }
                }
            }

        }

        /// <summary>
        /// Auto Ward When Enemy in Grass
        /// </summary>
        private static void FuckBitchgotoglass()
        {
            foreach (var enemy in ObjectManager.Get<Obj_AI_Hero>().Where(enemy => enemy.IsValidTarget(2000)))
            {
                bool WallOfGrass = NavMesh.IsWallOfGrass(Prediction.GetPrediction(enemy, 0.3f).CastPosition, 0);
                if (WallOfGrass)
                {
                    positionWard = Prediction.GetPrediction(enemy, 0.3f).CastPosition;
                    WardTarget = enemy;
                    WardTime = Game.Time;
                }
            }

            if (Player.Distance(positionWard) < 600 && !WardTarget.IsValidTarget() && Game.Time - WardTime < 5)
            {
                WardTime = Game.Time - 6;
                if (TrinketN.IsReady())
                    TrinketN.Cast(positionWard);
                else if (WardS.IsReady())
                    WardS.Cast(positionWard);
                else if (WardN.IsReady())
                    WardN.Cast(positionWard);
            }
        }
        /// <summary>
        /// How to Use E 
        /// </summary>
        /// <returns></returns>
        public static bool useE()
       {
           var mode = FlowersVayne.Menu.Item("nightmoon.e.mode").GetValue<StringList>().SelectedValue;

           if (mode == "Flowers")
           { 
               if(E.IsReady())
               {
                   VayneE.Flowers_VNE();
                   return true;
               }
               return false;
           }

           if (mode == "FlowersText")
           {
               if (E.IsReady())
               {
                   VayneE.FlowersText_E();
                   return true;
               }
               return false;
               
           }

           if (mode == "Marksman")
           {
               if (E.IsReady())
               {
                   VayneE.maskerman_E();
                   return true;
               }
               return false; 
               
           }

           if(mode == "SharpShooter")
           {
               if (E.IsReady())
               {
                   VayneE.SharpShooter_E();
                   return true;
               }
               return false; 
           }

           if (mode == "VnHunterReborn")
           {
               if (E.IsReady())
               {
                   VayneE.VNHunterRoborn_E();
                   return true;
               }
               return false; 
               
           }

           return false;
       }
    }
}
