namespace Flowers_Tristana
{

    #region

    using LeagueSharp;
    using LeagueSharp.Common;
    using Item = LeagueSharp.Common.Data.ItemData;
    using SharpDX;
    using System;
    using System.Collections.Generic;
    using System.Drawing;
    using System.Linq;
    using Color = System.Drawing.Color;

    #endregion

    public class HiddenObj
    {
        public int type;
        public float endTime { get; set; }
        public Vector3 pos { get; set; }
    }

    public class ChampionInfo
    {
        public int NetworkId { get; set; }
        public Vector3 LastVisablePos { get; set; }
        public float LastVisableTime { get; set; }
        public Vector3 PredictedPos { get; set; }
        public float StartRecallTime { get; set; }
        public float AbortRecallTime { get; set; }
        public float FinishRecallTime { get; set; }

        public ChampionInfo()
        {
            LastVisableTime = Game.Time;
            StartRecallTime = 0;
            AbortRecallTime = 0;
            FinishRecallTime = 0;
        }
    }

    public static class Program
    {
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public const string ChampionName = "Tristana";
        public static Spell Q, W, E, R;
        public static Menu Menu;
        public static Orbwalking.Orbwalker Orbwalker;
        public static bool rengar = false;
        public static Obj_AI_Hero Vayne = null;
        public static List<HiddenObj> HiddenObjList = new List<HiddenObj>();
        public static List<Obj_AI_Hero> Enemies = new List<Obj_AI_Hero>();
        public static List<ChampionInfo> ChampionInfoList = new List<ChampionInfo>();
        private static Items.Item VisionWard = new Items.Item(2043, 550f), OracleLens = new Items.Item(3364, 550f), WardN = new Items.Item(2044, 600f), TrinketN = new Items.Item(3340, 600f), SightStone = new Items.Item(2049, 600f), EOTOasis = new Items.Item(2302, 600f), EOTEquinox = new Items.Item(2303, 600f), EOTWatchers = new Items.Item(2301, 600f), FarsightOrb = new Items.Item(3342, 4000f), ScryingOrb = new Items.Item(3363, 3500f);
        //public static SoundPlayer Tristana = new SoundPlayer(Resources.Tristana);

        /// <summary>
        /// 主方法
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += LoadEvents;
        }

        /// <summary>
        /// 加载事件
        /// </summary>
        /// <param name="args"></param>
        private static void LoadEvents(EventArgs args)
        {
            if (Player.CharData.BaseSkinName != ChampionName)
            {
                return;
            }

            Menu = new Menu("Flowers - Tristana", "FLTA", true);
            Menu.SetFontStyle(FontStyle.Regular, SharpDX.Color.Red);

            Menu.AddItem(new MenuItem("nightmoon.menu.lanuguage", "Language Switch (Need F5): ").SetValue(new StringList(new[] { "English", "Chinese" }, 0)));

            if (Menu.Item("nightmoon.menu.load").GetValue<StringList>().SelectedIndex == 0)
            {
                LoadEnglish();
            }
            else if (Menu.Item("nightmoon.menu.load").GetValue<StringList>().SelectedIndex == 1)
            {
                LoadMenu();
            }

            Menu.AddItem(new MenuItem("nightmoon.credit", "Credit : NightMoon"));

            Menu.AddToMainMenu();

            LoadSpells();
            CheckVersion.Check();

            foreach (var hero in ObjectManager.Get<Obj_AI_Hero>())
            {
                if (hero.IsEnemy)
                {
                    if (hero.ChampionName == "Rengar")
                        rengar = true;
                    if (hero.ChampionName == "Vayne")
                        Vayne = hero;
                }
            }

            //if (Menu.Item("nightmoon.sound.bool").GetValue<bool>())
            //{
            //    PlaySound(Tristana);
            //}

            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            GameObject.OnCreate += GameObject_OnCreate;
            GameObject.OnDelete += GameObject_OnDelete;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnLevelUp += Obj_AI_Base_OnLevelUp;
            AntiGapcloser.OnEnemyGapcloser += AntiGapcloser_OnEnemyGapcloser;
            Interrupter2.OnInterruptableTarget += Interrupter2_OnInterruptableTarget;
            Orbwalking.BeforeAttack += Orbwalking_BeforeAttack;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        /// <summary>
        /// OKTW自动真眼逻辑
        /// </summary>
        /// <param name="释放位置"></param>
        private static void CastVisionWards(Vector3 position)
        {
            if (Menu.Item("nightmoon.ward.pink").GetValue<bool>())
            {
                if (OracleLens.IsReady())
                {
                    OracleLens.Cast(Player.Position.Extend(position, OracleLens.Range));
                }
                else
                if (VisionWard.IsReady())
                {
                    VisionWard.Cast(Player.Position.Extend(position, VisionWard.Range));
                }
            }
        }

        /// <summary>
        /// 自动插眼判断位置
        /// </summary>
        /// <param name="眼位名字"></param>
        /// <param name="释放位置"></param>
        private static void AddWard(string name,Vector3 posCast)
        {
            switch (name)
            {
                //PINKS
                case "visionward":
                    HiddenObjList.Add(new HiddenObj() { type = 2, pos = posCast, endTime = float.MaxValue });
                    break;
                case "trinkettotemlvl3B":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 180 });
                    break;
                //SIGH WARD
                case "itemghostward":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 180 });
                    break;
                case "wrigglelantern":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 180 });
                    break;
                case "sightward":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 180 });
                    break;
                case "itemferalflare":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 180 });
                    break;
                //TRINKET
                case "trinkettotemlvl1":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 60 });
                    break;
                case "trinkettotemlvl2":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 120 });
                    break;
                case "trinkettotemlvl3":
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = posCast, endTime = Game.Time + 180 });
                    break;
                //others
                case "teemorcast":
                    HiddenObjList.Add(new HiddenObj() { type = 3, pos = posCast, endTime = Game.Time + 300 });
                    break;
                case "noxious trap":
                    HiddenObjList.Add(new HiddenObj() { type = 3, pos = posCast, endTime = Game.Time + 300 });
                    break;
                case "JackInTheBox":
                    HiddenObjList.Add(new HiddenObj() { type = 3, pos = posCast, endTime = Game.Time + 100 });
                    break;
                case "Jack In The Box":
                    HiddenObjList.Add(new HiddenObj() { type = 3, pos = posCast, endTime = Game.Time + 100 });
                    break;
            }
        }

        /// <summary>
        /// OKTW 自动真眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (sender.IsEnemy && !sender.IsMinion && sender is Obj_AI_Hero)
            {
                AddWard(args.SData.Name.ToLower(), args.End);

                if (sender.Distance(Player.Position) < 800)
                {
                    switch (args.SData.Name)
                    {
                        case "akalismokebomb":
                            CastVisionWards(sender.ServerPosition);
                            break;
                        case "deceive":
                            CastVisionWards(sender.ServerPosition);
                            break;
                        case "khazixr":
                            CastVisionWards(sender.ServerPosition);
                            break;
                        case "khazixrlong":
                            CastVisionWards(sender.ServerPosition);
                            break;
                        case "talonshadowassault":
                            CastVisionWards(sender.ServerPosition);
                            break;
                        case "monkeykingdecoy":
                            CastVisionWards(sender.ServerPosition);
                            break;
                        case "RengarR":
                            CastVisionWards(sender.ServerPosition);
                            break;
                        case "TwitchHideInShadows":
                            CastVisionWards(sender.ServerPosition);
                            break;
                    }
                }
            }

        }

        /// <summary>
        /// OKTW 自动插眼逻辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void GameObject_OnDelete(GameObject sender, EventArgs args)
        {
            if (!sender.IsEnemy || sender.IsAlly || sender.Type != GameObjectType.obj_AI_Minion)
                return;

            foreach (var obj in HiddenObjList)
            {
                if (obj.pos == sender.Position)
                {
                    HiddenObjList.Remove(obj);
                    return;
                }
                else if (obj.type == 3 && obj.pos.Distance(sender.Position) < 100)
                {
                    HiddenObjList.Remove(obj);
                    return;
                }
                else if (obj.pos.Distance(sender.Position) < 400)
                {
                    if (obj.type == 2 && sender.Name.ToLower() == "visionward")
                    {
                        HiddenObjList.Remove(obj);
                        return;
                    }
                    else if ((obj.type == 0 || obj.type == 1) && sender.Name.ToLower() == "sightward")
                    {
                        HiddenObjList.Remove(obj);
                        return;
                    }
                }
            }
        }

        /// <summary>
        /// 自动R狮子狗跟螳螂 (在ScienceARK的小炮中提取) And OKTW的自动插眼
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        private static void GameObject_OnCreate(GameObject sender, EventArgs args)
        {
            //Credit ScienceARK
            var Rengar = HeroManager.Enemies.Find(heros => heros.ChampionName.Equals("Rengar"));
            var Khazix = HeroManager.Enemies.Find(heros => heros.ChampionName.Equals("Khazix"));

            if (Rengar != null || Khazix != null)
            {
                if(Menu.Item("nightmoon.r.rk").GetValue<bool>())
                {
                    if(sender.Name == ("Rengar_LeapSound.troy") && sender.Position.Distance(Player.Position) < R.Range)
                    {
                        R.Cast(Rengar);
                    }

                    if(sender.Name == ("Khazix_Base_E_Tar.troy") && sender.Position.Distance(Player.Position) <= 300)
                    {
                        R.Cast(Khazix);
                    }
                }
            }

            //Credit Sebby
            if (!sender.IsEnemy || sender.IsAlly)
                return;

            if (sender.Type == GameObjectType.MissileClient && (sender is MissileClient))
            {
                var missile = (MissileClient)sender;

                if (!missile.SpellCaster.IsVisible)
                {

                    if ((missile.SData.Name == "BantamTrapShort" || missile.SData.Name == "BantamTrapBounceSpell") && !HiddenObjList.Exists(x => missile.EndPosition == x.pos))
                        AddWard("teemorcast", missile.EndPosition);
                }
            }

            if (sender.Type == GameObjectType.obj_AI_Minion && (sender.Name.ToLower() == "visionward" || sender.Name.ToLower() == "sightward") && !HiddenObjList.Exists(x => x.pos.Distance(sender.Position) < 100))
            {
                foreach (var obj in HiddenObjList)
                {
                    if (obj.pos.Distance(sender.Position) < 400)
                    {
                        if (obj.type == 0)
                        {
                            HiddenObjList.Remove(obj);
                            return;
                        }
                    }
                }

                var dupa = (Obj_AI_Minion)sender;
                if (dupa.Mana == 0)
                    HiddenObjList.Add(new HiddenObj() { type = 2, pos = sender.Position, endTime = float.MaxValue });
                else
                    HiddenObjList.Add(new HiddenObj() { type = 1, pos = sender.Position, endTime = Game.Time + dupa.Mana });
            }

            if (rengar && sender.Position.Distance(Player.Position) < 800)
            {
                switch (sender.Name)
                {
                    case "Rengar_LeapSound.troy":
                        CastVisionWards(sender.Position);
                        break;
                    case "Rengar_Base_R_Alert":
                        CastVisionWards(sender.Position);
                        break;
                }
            }

        }

        /// <summary>
        /// 音乐播放
        /// </summary>
        /// <param name="音乐"></param>
        //public static void PlaySound(SoundPlayer sound = null)
        //{
        //    if (sound != null)
        //    {
        //        try
        //        {
        //            sound.Play();
        //        }
        //        catch (Exception ex)
        //        {
        //            Console.WriteLine(ex);
        //        }
        //    }
        //}

        /// <summary>
        /// 循环事件 包括各种小事件触发
        /// </summary>
        /// <param name="事件"></param>
        private static void Game_OnUpdate(EventArgs args)
        {
            if(Player.IsDead)
            {
                return;
            }

            switch(Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    {
                        Combo();
                        break;
                    }
                case Orbwalking.OrbwalkingMode.Mixed:
                    {
                        Harass();
                        break;
                    }
            }

            RKS();
            REKS();
            AutoWardBuySebby();
            AutoWardLogicBySebby();
        }

        /// <summary>
        /// 幽梦 弯刀 破败使用逻辑
        /// </summary>
        private static void ItemUse()
        {
            var Target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

            if(Target == null)
            {
                return;
            }

            var Borke = Item.Blade_of_the_Ruined_King.GetItem();
            var Youmeng = Item.Youmuus_Ghostblade.GetItem();
            var Blige = Item.Bilgewater_Cutlass.GetItem();

            if(Menu.Item("nightmoon.item.youmeng").GetValue<bool>())
            {
                if (Youmeng.IsReady() && Youmeng.IsOwned(Player))
                {
                    if(Target.IsValidTarget(E.Range))
                    {
                        Youmeng.Cast();

                        if(Menu.Item("nightmoon.q.youmeng").GetValue<bool>() && CanCastQ() && Youmeng.Cast())
                        {
                            Q.Cast();
                        }
                    }

                    if(Player.CountEnemiesInRange(1200) < 2 && Menu.Item("nightmoon.item.youmeng.dush").GetValue<bool>())
                    {
                        Youmeng.Cast();

                        if (Menu.Item("nightmoon.q.youmeng").GetValue<bool>() && CanCastQ() && Youmeng.Cast())
                        {
                            Q.Cast();
                        }
                    }
                }
            }


            if(Menu.Item("nightmoon.item.blige").GetValue<bool>())
            {
                if (Blige.IsReady() && Blige.IsOwned(Player))
                {
                    if(Blige.IsInRange(Target))
                    {
                        if(Target.HealthPercent <= Menu.Item("nightmoon.item.blige.enemyhp").GetValue<Slider>().Value)
                        {
                            Blige.Cast(Target);
                        }
                    }
                }
            }

            if (Menu.Item("nightmoon.item.borke").GetValue<bool>())
            {
                if(Borke.IsReady() && Borke.IsOwned(Player))
                {
                    if(Borke.IsInRange(Target))
                    {
                        if (Target.HealthPercent <= Menu.Item("nightmoon.item.borke.enemyhp").GetValue<Slider>().Value)
                        {
                            Borke.Cast(Target);
                        }

                        if (Player.HealthPercent <= Menu.Item("nightmoon.item.borke.mehp").GetValue<Slider>().Value)
                        {
                            Borke.Cast(Target);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Credit Sebby
        /// </summary>
        private static void AutoWardLogicBySebby()
        {
            foreach (var enemy in Enemies.Where(enemy => enemy.IsValid && !enemy.IsVisible && !enemy.IsDead))
            {
                var need = ChampionInfoList.Find(x => x.NetworkId == enemy.NetworkId);

                if (need == null || need.PredictedPos == null)
                    return;

                var timer = Game.Time - need.LastVisableTime;

                if (timer < 4)
                {
                    if (Menu.Item("nightmoon.ward.onlycombo").GetValue<bool>() && !(Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo))
                    {
                        return;
                    }

                    if (NavMesh.IsWallOfGrass(need.PredictedPos, 0))
                    {
                        if (need.PredictedPos.Distance(Player.Position) < 600 && Menu.Item("nightmoon.ward.auto").GetValue<bool>())
                        {
                            if (TrinketN.IsReady())
                            {
                                TrinketN.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                            else if (SightStone.IsReady())
                            {
                                SightStone.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                            else if (WardN.IsReady())
                            {
                                WardN.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                            else if (EOTOasis.IsReady())
                            {
                                EOTOasis.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                            else if (EOTEquinox.IsReady())
                            {
                                EOTEquinox.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                            else if (EOTWatchers.IsReady())
                            {
                                EOTWatchers.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                        }

                        if (need.PredictedPos.Distance(Player.Position) < 1400 && Menu.Item("nightmoon.ward.autoblue").GetValue<bool>())
                        {
                            if (FarsightOrb.IsReady())
                            {
                                FarsightOrb.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                            else if (ScryingOrb.IsReady())
                            {
                                ScryingOrb.Cast(need.PredictedPos);
                                need.LastVisableTime = Game.Time - 5;
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 自动眼位By Sebby
        /// </summary>
        private static void AutoWardBuySebby()
        {
            foreach (var obj in HiddenObjList)
            {
                if (obj.endTime < Game.Time)
                {
                    HiddenObjList.Remove(obj);
                    return;
                }
            }

            if (Menu.Item("nightmoon.ward.buyblue").GetValue<bool>() && Player.InFountain() && !ScryingOrb.IsOwned() && Player.Level > 8)
            {
                ObjectManager.Player.BuyItem(ItemId.Scrying_Orb_Trinket);
            }

            if (rengar && Player.HasBuff("rengarralertsound"))
            {
                CastVisionWards(Player.ServerPosition);
            }

            if (Vayne != null && Vayne.IsValidTarget(1000) && Vayne.HasBuff("vaynetumblefade"))
            {
                CastVisionWards(Vayne.ServerPosition);
            }
        }

        /// <summary>
        /// R+E击杀逻辑
        /// </summary>
        private static void REKS()
        {
            foreach (var enemy in from enemy in HeroManager.Enemies.Where(e => R.CanCast(e))
                                  let etargetstacks = enemy.Buffs.Find(buff => buff.Name == "tristanaecharge")
                                  where R.GetDamage(enemy) + E.GetDamage(enemy) + etargetstacks?.Count * 0.30 * E.GetDamage(enemy) >=
                                        enemy.Health
                                  select enemy)
            {
                if(CanCastR())
                {
                    R.CastOnUnit(enemy);
                    return;
                }
            }
        }

        /// <summary>
        /// R击杀敌人逻辑
        /// </summary>
        private static void RKS()
        {
            if(Menu.Item("nightmoon.r.ks").GetValue<bool>())
            {
                var Target = HeroManager.Enemies.OrderByDescending
                    (x => x.Health).FirstOrDefault(x => x.isKillableAndValidTarget(R.GetDamage(x), TargetSelector.DamageType.Physical, R.Range) && !x.ECanKill());
                if (Target != null)
                {
                    if(CanCastR())
                    {
                        R.CastOnUnit(Target);
                    }
                }
            }
        }

        /// <summary>
        /// 自动E兵骚扰敌人逻辑
        /// </summary>
        private static void Harass()
        {
            if (Menu.Item("nightmoon.e.quickharass").GetValue<bool>())
            {
                foreach (var minion in MinionManager.GetMinions(E.Range) .Where
                    (m => E.CanCast(m) && m.Health < Player.GetAutoAttackDamage(m) && m.CountEnemiesInRange(m.BoundingRadius + 150) >= 1))
                {
                    var etarget = E.GetTarget();
                    if (etarget != null)
                    {
                        return;
                    }

                    if(CanCastE())
                    {
                        E.CastOnUnit(minion);
                        Orbwalker.ForceTarget(minion);
                    }
                }
            }
        }

        /// <summary>
        /// 自动E敌人 R自保逻辑
        /// </summary>
        private static void Combo()
        {
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);

            if (target == null || !target.IsValidTarget())
            {
                return;
            }

            ItemUse();

            if (!Menu.Item("nightmoon.q.onlye").GetValue<bool>() && Menu.Item("nightmoon.q.combo").GetValue<bool>())
            {
                if(target.IsValidTarget(E.Range))
                {
                    if (CanCastQ())
                    {
                        Q.Cast();
                    }
                }
            }

            if (E.IsInRange(target) && Menu.Item("nightmoon." + target.ChampionName + "euse").GetValue<bool>() && CanCastE())
            {
                E.CastOnUnit(target);

                if (Menu.Item("nightmoon.q.onlye").GetValue<bool>() && CanCastQ() && !E.IsReady())
                {
                    Q.Cast();
                }
            }

            if (Menu.Item("nightmoon.r.self").GetValue<Slider>().Value != 0 && Player.HealthPercent <= Menu.Item("nightmoon.r.self").GetValue<Slider>().Value)
            {
                var dangerenemy =HeroManager.Enemies.Where(e => R.CanCast(e)).OrderBy(enemy => enemy.Distance(Player)).FirstOrDefault();
                if (dangerenemy != null)
                {
                    if(CanCastR())
                    {
                        R.CastOnUnit(dangerenemy);
                    }
                }
            }
        }

        /// <summary>
        /// 设置技能的释放范围
        /// </summary>
        /// <param name="自己"></param>
        /// <param name="事件"></param>
        private static void Obj_AI_Base_OnLevelUp(Obj_AI_Base sender, EventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            var lvl = (7 * (Player.Level - 1));
            Q.Range = 605 + lvl;
            E.Range = 635 + lvl;
            R.Range = 635 + lvl;
        }

        /// <summary>
        /// 使用R 阻止被人突脸(在R范围内)
        /// </summary>
        /// <param name="突进者"></param>
        private static void AntiGapcloser_OnEnemyGapcloser(ActiveGapcloser gapcloser)
        {
            if (Menu.Item("nightmoon.r.gap").GetValue<bool>())
            {
                if (gapcloser.End.Distance(ObjectManager.Player.Position) <= 200 && gapcloser.Sender.IsValidTarget(R.Range))
                {
                    if(CanCastR())
                    {
                        R.CastOnUnit(gapcloser.Sender);
                    }
                }
            }
        }

        /// <summary>
        /// 使用R 打断技能 技能危险度大于高的自动R(在R范围内)
        /// </summary>
        /// <param name="目标"></param>
        /// <param name="技能"></param>
        private static void Interrupter2_OnInterruptableTarget(Obj_AI_Hero sender, Interrupter2.InterruptableTargetEventArgs args)
        {
            if (Menu.Item("nightmoon.r.int").GetValue<bool>())
            {
                if (args.DangerLevel >= Interrupter2.DangerLevel.High && sender.IsValidTarget(R.Range))
                {
                    if(CanCastR())
                    {
                        R.CastOnUnit(sender);
                    }
                }
            }
        }

        /// <summary>
        /// 判断是否有足够蓝量释放R
        /// </summary>
        /// <returns></returns>
        public static bool CanCastR()
        {
            if (Player.ManaPercent > Menu.Item("nightmoon.r.mana").GetValue<Slider>().Value && R.IsReady())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否有足够蓝量释放E
        /// </summary>
        /// <returns></returns>
        public static bool CanCastE()
        {
            if(Player.ManaPercent > Menu.Item("nightmoon.e.mana").GetValue<Slider>().Value && E.IsReady())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 判断是否有足够蓝量释放Q
        /// </summary>
        /// <returns></returns>
        public static bool CanCastQ()
        {
            if (Player.ManaPercent > Menu.Item("nightmoon.q.mana").GetValue<Slider>().Value && Q.IsReady())
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 检测清线Q还有 连招Q使用逻辑(包括打塔自动Q) 还有选择目标问题
        /// </summary>
        /// <param name="args"></param>
        private static void Orbwalking_BeforeAttack(Orbwalking.BeforeAttackEventArgs args)
        {
            if(Menu.Item("nightmoon.e.forcetarget").GetValue<bool>())
            {
                if(Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo || Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
                {
                    foreach (var enemy in HeroManager.Enemies.Where(enemy => enemy.HasBuff("tristanaechargesound")))
                    {
                        TargetSelector.SetTarget(enemy);
                        return;
                    }
                    TargetSelector.SetTarget(TargetSelector.GetTarget(Orbwalking.GetRealAutoAttackRange(Player), TargetSelector.DamageType.Physical));
                }
            }

            if (args.Unit.IsMe && Orbwalking.InAutoAttackRange(args.Target))
            {
                switch(Orbwalker.ActiveMode)
                {
                    case Orbwalking.OrbwalkingMode.Combo:
                        {
                            if (Menu.Item("nightmoon.q.onlye").GetValue<bool>() && CanCastQ())
                            {
                                Obj_AI_Hero Target = args.Target.Type == GameObjectType.obj_AI_Hero ? (Obj_AI_Hero)args.Target : null;
                                if (Target.HasBuff("tristanaechargesound") || Target.HasBuff("tristanaecharge"))
                                {
                                    Q.Cast();
                                }
                            }
                            else if(!Menu.Item("nightmoon.q.onlye").GetValue<bool>() && CanCastQ())
                            {
                                Q.Cast();
                            }
                            break;
                        }
                    case Orbwalking.OrbwalkingMode.LaneClear:
                        {

                            if (Menu.Item("nightmoon.q.jc").GetValue<bool>())
                            {
                                if (MinionManager.GetMinions
                                    (Orbwalking.GetRealAutoAttackRange(ObjectManager.Player), 
                                    MinionTypes.All, MinionTeam.Neutral, MinionOrderTypes.MaxHealth).
                                    Any(x => x.NetworkId == args.Target.NetworkId) &&
                                    CanCastQ())
                                {
                                    Q.Cast();
                                }
                            }


                            //Obj_AI_Base Turret = (args.Target.Type == GameObjectType.obj_AI_Turret || args.Target.Type == GameObjectType.obj_Turret) ? (Obj_AI_Base)args.Target : null; 
                            //if (Turret.HasBuff("tristanaechargesound") || Turret.HasBuff("tristanaecharge"))
                            //{
                            //    if(Menu.Item("nightmoon.q.tower").GetValue<bool>())
                            //    {
                            //        if (!Player.IsWindingUp && Player.CountEnemiesInRange(1000) < 1 && CanCastQ())
                            //        {
                            //            Q.Cast();
                            //        }
                            //    }
                            //}
                            break;
                        }
                }
            }
        }

        /// <summary>
        /// 攻击后检测放E打塔的条件
        /// </summary>
        /// <param name="自身"></param>
        /// <param name="防御塔"></param>
        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            if(Menu.Item("nightmoon.e.tower").GetValue<bool>())
            {
                if (unit.IsMe && target != null)
                {
                    if (target.Type == GameObjectType.obj_AI_Turret || target.Type == GameObjectType.obj_Turret)
                    {
                        if (CanCastE())
                        {
                            E.CastOnUnit(target as Obj_AI_Base);
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 显示E爆炸范围 显示E R 可击杀
        /// </summary>
        /// <param name="args"></param>
        private static void Drawing_OnDraw(EventArgs args)
        {

            if(Player.IsDead)
            {
                return;
            }

            if(Menu.Item("nightmoon.draw.e").GetValue<bool>() && !E.IsReady())
            {
                var ETurret = ObjectManager.Get<Obj_AI_Turret>().FirstOrDefault(t => !t.IsDead && t.HasBuff("tristanaecharge"));
                var ETarget = HeroManager.Enemies.FirstOrDefault(e => !e.IsDead && e.HasBuff("tristanaecharge"));

                if (ETurret != null)
                {
                    var eturretstacks = ETurret.Buffs.Find(buff => buff.Name == "tristanaecharge").Count;

                    if (ETurret.Health < (E.GetDamage(ETurret) + (((eturretstacks * 0.30)) * E.GetDamage(ETurret))))
                    {
                        Drawing.DrawCircle(ETurret.Position, 300 + ETurret.BoundingRadius, Color.Red);
                    }
                    else if (ETurret.Health > (E.GetDamage(ETurret) + (((eturretstacks * 0.30)) * E.GetDamage(ETurret))))
                    {
                        Drawing.DrawCircle(ETurret.Position, 300 + ETurret.BoundingRadius, Color.Orange);
                    }
                }

                if (ETarget != null)
                {
                    var etargetstacks = ETarget.Buffs.Find(buff => buff.Name == "tristanaecharge").Count;

                    if (ETarget.Health < (E.GetDamage(ETarget) + (((etargetstacks * 0.30)) * E.GetDamage(ETarget))))
                    {
                        Drawing.DrawCircle(ETarget.Position, 150 + ETarget.BoundingRadius, Color.Red);
                    }
                    else if (ETarget.Health > (E.GetDamage(ETarget) + (((etargetstacks * 0.30)) * E.GetDamage(ETarget))))
                    {
                        Drawing.DrawCircle(ETarget.Position, 150 + ETarget.BoundingRadius, Color.Orange);
                    }
                }
            }

            if (Menu.Item("nightmoon.draw.eks").GetValue<Circle>().Active)
            {
                foreach (var Target in HeroManager.Enemies.Where(x => x.IsValidTarget() && x.ECanKill()))
                {

                    var TargetPos = Drawing.WorldToScreen(Target.Position);
                    Render.Circle.DrawCircle(Target.Position, Target.BoundingRadius, Menu.Item("nightmoon.draw.eks").GetValue<Circle>().Color);
                    Drawing.DrawText(TargetPos.X, TargetPos.Y - 50, Menu.Item("nightmoon.draw.eks").GetValue<Circle>().Color, "Kill For E");
                }
            }

            if (Menu.Item("nightmoon.draw.rks").GetValue<Circle>().Active && CanCastR())
            {
                foreach (var Target in HeroManager.Enemies.Where(x => x.isKillableAndValidTarget(R.GetDamage(x), TargetSelector.DamageType.Magical)))
                {
                    var TargetPos = Drawing.WorldToScreen(Target.Position);
                    Render.Circle.DrawCircle(Target.Position, Target.BoundingRadius, Menu.Item("nightmoon.draw.rks").GetValue<Circle>().Color);
                    Drawing.DrawText(TargetPos.X, TargetPos.Y - 20, Menu.Item("nightmoon.draw.rks").GetValue<Circle>().Color, "Kill For R");
                }
            }
        }

        /// <summary>
        /// 判断E是否能击杀
        /// </summary>
        /// <param name="目标"></param>
        /// <returns></returns>
        public static bool ECanKill(this Obj_AI_Base target)
        {
            if (target.HasBuff("tristanaecharge"))
            {
                if (target.isKillableAndValidTarget
                    (Damage.GetSpellDamage(ObjectManager.Player, target, SpellSlot.E) * (target.GetBuffCount("tristanaecharge") * 0.30) + 
                    Damage.GetSpellDamage(ObjectManager.Player, target, SpellSlot.E), TargetSelector.DamageType.Physical))
                {
                    return true;
                }
                return false;
            }
            return false;
        }

        /// <summary>
        /// 棒子的自带是否带无法击杀buff检测
        /// </summary>
        /// <param name="目标"></param>
        /// <param name="伤害计算"></param>
        /// <param name="伤害类型"></param>
        /// <param name="目标位置"></param>
        /// <returns></returns>
        public static bool isKillableAndValidTarget(this Obj_AI_Base Target, double CalculatedDamage, TargetSelector.DamageType damageType, float distance = float.MaxValue)
        {
            if (Target == null || !Target.IsValidTarget(distance) || Target.Health <= 0 || Target.CharData.BaseSkinName == "gangplankbarrel")
                return false;

            if (Target.HasBuff("kindredrnodeathbuff"))
            {
                return false;
            }

            // Tryndamere's Undying Rage (R)
            if (Target.HasBuff("Undying Rage") && Target.Health <= Target.MaxHealth * 0.10f)
            {
                return false;
            }

            // Kayle's Intervention (R)
            if (Target.HasBuff("JudicatorIntervention"))
            {
                return false;
            }

            // Poppy's Diplomatic Immunity (R)
            if (Target.HasBuff("DiplomaticImmunity") && !ObjectManager.Player.HasBuff("poppyulttargetmark"))
            {
                //TODO: Get the actual target mark buff name
                return false;
            }

            // Banshee's Veil (PASSIVE)
            if (Target.HasBuff("BansheesVeil"))
            {
                // TODO: Get exact Banshee's Veil buff name.
                return false;
            }

            // Sivir's Spell Shield (E)
            if (Target.HasBuff("SivirShield"))
            {
                // TODO: Get exact Sivir's Spell Shield buff name
                return false;
            }

            // Nocturne's Shroud of Darkness (W)
            if (Target.HasBuff("ShroudofDarkness"))
            {
                // TODO: Get exact Nocturne's Shourd of Darkness buff name
                return false;
            }

            if (ObjectManager.Player.HasBuff("summonerexhaust"))
                CalculatedDamage *= 0.6;

            if (Target.CharData.BaseSkinName == "Blitzcrank")
                if (!Target.HasBuff("manabarriercooldown"))
                    if (Target.Health + Target.HPRegenRate + (damageType == TargetSelector.DamageType.Physical ? Target.PhysicalShield : Target.MagicalShield) + (Target.Mana * 0.6) + Target.PARRegenRate < CalculatedDamage)
                        return true;

            if (Target.CharData.BaseSkinName == "Garen")
                if (Target.HasBuff("GarenW"))
                    CalculatedDamage *= 0.7;


            if (Target.HasBuff("FerociousHowl"))
                CalculatedDamage *= 0.3;

            BuffInstance dragonSlayerBuff = ObjectManager.Player.GetBuff("s5test_dragonslayerbuff");
            if (dragonSlayerBuff != null)
                if (Target.IsMinion)
                {
                    if (dragonSlayerBuff.Count >= 4)
                        CalculatedDamage += dragonSlayerBuff.Count == 5 ? CalculatedDamage * 0.30 : CalculatedDamage * 0.15;

                    if (Target.CharData.BaseSkinName.ToLowerInvariant().Contains("dragon"))
                        CalculatedDamage *= 1 - (dragonSlayerBuff.Count * 0.07);
                }

            if (Target.CharData.BaseSkinName.ToLowerInvariant().Contains("baron") && ObjectManager.Player.HasBuff("barontarget"))
                CalculatedDamage *= 0.5;

            return Target.Health + Target.HPRegenRate + (damageType == TargetSelector.DamageType.Physical ? Target.PhysicalShield : Target.MagicalShield) < CalculatedDamage - 2;
        }

        /// <summary>
        /// 中文菜单
        /// </summary>
        private static void LoadMenu()
        {
            Menu.AddSubMenu(new Menu("走砍设置", "nightmoon.orbwalking.setting"));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("nightmoon.orbwalking.setting"));

            Menu.AddSubMenu(new Menu("[FL] 技能设置", "nightmoon.spell.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.DarkBlue));
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.combo", "连招时智能使用Q").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.jc", "使用Q自动清野").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.youmeng", "使用幽梦连招后自动Q").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.onlye", "仅使用E后再用Q").SetTooltip("对方身上有E才用Q攻击").SetValue(false));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.tower", "自动E塔").SetTooltip("自动E塔").SetValue(true));//1
            //Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.tower", "E塔后自动接Q").SetTooltip("附近木有英雄才这样").SetValue(false));
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.uselist", "使用E对象:").SetTooltip("自动E英雄列表"));//1
            foreach (var enemy in HeroManager.Enemies)
            {
                Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon." + enemy.ChampionName + "euse", "英雄:" + enemy.ChampionName).SetValue(true));//1
            }
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.forcetarget", "集中攻击被E的目标").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.quickharass", "使用E快速骚扰").SetTooltip("当一个要死的小兵附近有英雄并且放E爆炸能吃伤害 就放E给小兵自动击杀小兵").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.r.self", "使用R自保-自己Hp最低百分比").SetValue(new Slider(20)));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.r.ks", "R击杀")).SetTooltip("R的伤害足够才释放").SetValue(true);//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.re.ks", "R+E击杀")).SetTooltip("R+E的伤害足够才释放").SetValue(true);//1

            Menu.AddSubMenu(new Menu("[FL] 反突打断", "nightmoon.misc.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.CadetBlue));
            Menu.SubMenu("nightmoon.misc.setting").AddItem(new MenuItem("nightmoon.r.gap", "使用R反突进")).SetValue(true);//1
            Menu.SubMenu("nightmoon.misc.setting").AddItem(new MenuItem("nightmoon.r.rk", "使用R反狮子狗跟螳螂")).SetValue(true);//1
            Menu.SubMenu("nightmoon.misc.setting").AddItem(new MenuItem("nightmoon.r.int", "使用R打断技能")).SetValue(true);//1

            Menu.AddSubMenu(new Menu("[FL] 蓝量管理", "nightmoon.mana.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.LawnGreen));
            Menu.SubMenu("nightmoon.mana.setting").AddItem(new MenuItem("nightmoon.q.mana", "全局使用Q最低蓝量").SetValue(new Slider(10)));//1
            Menu.SubMenu("nightmoon.mana.setting").AddItem(new MenuItem("nightmoon.e.mana", "全局使用E最低蓝量").SetValue(new Slider(15)));//1
            Menu.SubMenu("nightmoon.mana.setting").AddItem(new MenuItem("nightmoon.r.mana", "全局使用R最低蓝量").SetValue(new Slider(10)));//1

            Menu.AddSubMenu(new Menu("[FL] 物品使用", "nightmoon.item.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.SkyBlue));
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.youmeng", "使用幽梦")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.youmeng.dush", "自动追人击杀")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.blige", "使用弯刀")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.blige.enemyhp", "敌人当前Hp").SetValue(new Slider(80)));//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.borke", "使用破败")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.borke.enemyhp", "敌人当前Hp").SetValue(new Slider(80)));//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.borke.mehp", "自己当前Hp").SetValue(new Slider(60)));//1

            Menu.AddSubMenu(new Menu("[FL] 自动插眼", "nightmoon.ward.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.Red));
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.auto", "自动进草插眼").SetValue(true));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.autoblue", "自动用灯泡").SetValue(true));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.onlycombo", "仅连招模式使用以上设置").SetValue(true));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.buyblue", "Lv9后自动买灯泡").SetValue(false));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.pink", "自动真眼,扫描").SetValue(true));//1

            Menu.AddSubMenu(new Menu("[FL] 显示设置", "nightmoon.draw.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Menu.SubMenu("nightmoon.draw.setting").AddItem(new MenuItem("nightmoon.draw.e", "显示E爆炸半径")).SetTooltip("仅身上有E才显示 并且根据是否能击杀颜色变化 防御塔也会显示").SetValue(true);//1
            Menu.SubMenu("nightmoon.draw.setting").AddItem(new MenuItem("nightmoon.draw.eks", "显示E击杀目标").SetValue(new Circle(true, Color.Red)));//1
            Menu.SubMenu("nightmoon.draw.setting").AddItem(new MenuItem("nightmoon.draw.rks", "显示R击杀目标").SetValue(new Circle(true, Color.Red)));//1

            Menu.AddItem(new MenuItem("nightmoon.sound.bool", "开局音效").SetValue(true));
        }

        /// <summary>
        /// 英文菜单
        /// </summary>
        private static void LoadEnglish()
        {
            Menu.AddSubMenu(new Menu("[FL] Orbwalker Setting", "nightmoon.orbwalking.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Orbwalker = new Orbwalking.Orbwalker(Menu.SubMenu("nightmoon.orbwalking.setting"));

            Menu.AddSubMenu(new Menu("[FL] Spells Setting", "nightmoon.spell.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.combo", "Use Q In Combo").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.jc", "Use Q In Jungle").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.youmeng", "Auto Q If Use Ghostblade").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.onlye", "Only Have E buffs Use Q").SetValue(false));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.tower", "Auto E Towers").SetValue(true));//1
            //Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.q.tower", "If E Tower Auto Q").SetValue(false));
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.uselist", "Use E list:"));//1
            foreach (var enemy in HeroManager.Enemies)
            {
                Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon." + enemy.ChampionName + "euse", "Heros :" + enemy.ChampionName).SetValue(true));//1
            }
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.forcetarget", "Force Attack E Target").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.e.quickharass", "Use E QuickHarass").SetTooltip("if a minion will died and enemy will heart").SetValue(true));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.r.self", "Use R 丨If Hp <=%").SetValue(new Slider(20)));//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.r.ks", "Use R Killsteal")).SetValue(true);//1
            Menu.SubMenu("nightmoon.spell.setting").AddItem(new MenuItem("nightmoon.re.ks", "Use R+E Killsteal")).SetValue(true);//1

            Menu.AddSubMenu(new Menu("[FL] Misc Setting", "nightmoon.misc.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Menu.SubMenu("nightmoon.misc.setting").AddItem(new MenuItem("nightmoon.r.gap", "Use R AntiGapcloser")).SetValue(true);//1
            Menu.SubMenu("nightmoon.misc.setting").AddItem(new MenuItem("nightmoon.r.rk", "Use R Anti Rengar&Khazix")).SetValue(true);//1
            Menu.SubMenu("nightmoon.misc.setting").AddItem(new MenuItem("nightmoon.r.int", "Use R Interrupter")).SetValue(true);//1

            Menu.AddSubMenu(new Menu("[FL] Mana Manager", "nightmoon.mana.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Menu.SubMenu("nightmoon.mana.setting").AddItem(new MenuItem("nightmoon.q.mana", "Whole Use Q Mana Control").SetValue(new Slider(10)));//1
            Menu.SubMenu("nightmoon.mana.setting").AddItem(new MenuItem("nightmoon.e.mana", "Whole Use E Mana Control").SetValue(new Slider(15)));//1
            Menu.SubMenu("nightmoon.mana.setting").AddItem(new MenuItem("nightmoon.r.mana", "Whole Use R Mana Control").SetValue(new Slider(10)));//1

            Menu.AddSubMenu(new Menu("[FL] Items Use", "nightmoon.item.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.youmeng", "Use Ghostblade")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.youmeng.dush", "Use Ghostblade To 1v1")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.blige", "Use Cutlass")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.blige.enemyhp", "Enemy Hp <=%").SetValue(new Slider(80)));//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.borke", "Use Borke")).SetValue(true);//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.borke.enemyhp", "Enemy Hp <=%").SetValue(new Slider(80)));//1
            Menu.SubMenu("nightmoon.item.setting").AddItem(new MenuItem("nightmoon.item.borke.mehp", "My Hp <=%").SetValue(new Slider(60)));//1

            Menu.AddSubMenu(new Menu("[FL] Auto Ward", "nightmoon.ward.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.auto", "Auto Ward").SetValue(true));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.autoblue", "Auto Use Blue").SetValue(true));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.onlycombo", "Only Combo").SetValue(true));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.buyblue", "Auto Buy Blue In Lv9").SetValue(false));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.ward.pink", "Auto Pink Ward").SetValue(true));//1
            Menu.SubMenu("nightmoon.ward.setting").AddItem(new MenuItem("nightmoon.thanks.sebby", "Credit:Sebby, Thanks God").SetValue(true));//1

            Menu.AddSubMenu(new Menu("[FL] Draw Setting", "nightmoon.draw.setting").SetFontStyle(FontStyle.Regular, SharpDX.Color.GreenYellow));
            Menu.SubMenu("nightmoon.draw.setting").AddItem(new MenuItem("nightmoon.draw.e", "Draw E Bomb Range")).SetTooltip("Credit God!").SetValue(true);//1
            Menu.SubMenu("nightmoon.draw.setting").AddItem(new MenuItem("nightmoon.draw.eks", "Draw E Killsteal Target").SetValue(new Circle(true, Color.Red)));//1
            Menu.SubMenu("nightmoon.draw.setting").AddItem(new MenuItem("nightmoon.draw.rks", "Draw R Killsteal Target").SetValue(new Circle(true, Color.Red)));//1

            //Menu.AddItem(new MenuItem("nightmoon.sound.bool", "Play Sound").SetValue(true));
        }

        /// <summary>
        /// 小炮技能
        /// </summary>
        private static void LoadSpells()
        {
            Q = new Spell(SpellSlot.Q);
            E = new Spell(SpellSlot.E, 630);
            R = new Spell(SpellSlot.R, 630);
        }

    }
}
