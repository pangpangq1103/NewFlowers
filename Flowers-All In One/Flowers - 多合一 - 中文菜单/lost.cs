#region 神奇收缩大法

#region 引用
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using LeagueSharp;
using LeagueSharp.Common;
using System.Drawing;
using Color = System.Drawing.Color;
using SharpDX;
using SharpDX.Direct3D9;
using Rectangle = SharpDX.Rectangle;
using Font = SharpDX.Direct3D9.Font;
#endregion

#region 花边 出品
namespace Flowers___All_In_One
{

    #region Lost in love
    class lost
    {
        #region 静态调用
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        public struct Spells{public string ChampionName;public string SpellName;public SpellSlot slot;}
        private static Dictionary<String, int> ChampSkins = new Dictionary<String, int>();
        private static bool setupdone;
        private const String LanternName = "ThreshLantern";
        public static Spell SummonerDot;
        public static Vector3 positionWard;
        private static Obj_AI_Hero WardTarget;
        private static float WardTime = 0;
        private static int _nextTime;
        public static float LastMove;
        public static double windup;
        public static List<Spells> SpellList = new List<Spells>();
        public static int Delay = 0;
        public static float VayneBuffEndTime = 0;
        private static Boolean special;
        private static Boolean special2;
        private static Boolean special3;
        private static Boolean correctForm;
        private static Spell Reset;
        public static bool hasSmite;
        private static SpellDataInst Summoner1;
        private static SpellDataInst Summoner2;
        public static Spell smite;
        public static double damage;
        private static int Plevel;
        public static Obj_AI_Base mob;
        private static bool delayingzhonya;
        private static System.Drawing.Color posCol = System.Drawing.Color.Green;

        public static Font Text1 = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription
           {
               FaceName = "Calibri",
               Height = 13,
               OutputPrecision = FontPrecision.Default,
               Quality = FontQuality.Default,
           });

        private static string[] MinionNames = 
        {
            "SRU_Blue", "SRU_Red", "SRU_Dragon", "SRU_Baron"
        };

        private static List<Vector3> pos = new List<Vector3> { new Vector3(11848, 3992, -68f),
                                                               new Vector3(10486, 5116, -62f),
                                                               new Vector3(10386, 3056, -49f),
                                                               new Vector3(9410, 5626, -71f),
                                                               new Vector3(8398, 6460, -71f),
                                                               new Vector3(6540, 8302, -71f),
                                                               new Vector3(5266, 9094, -71f),
                                                               new Vector3(4436, 9650, -66f),
                                                               new Vector3(3020, 10892, -70f),
                                                               new Vector3(4436, 11790, -56f)
        };

        private static readonly int[] SmitePurple = { 3713, 3726, 3725, 3724, 3723 };
        private static readonly int[] SmiteGrey = { 3711, 3722, 3721, 3720, 3719 };
        private static readonly int[] SmiteRed = { 3715, 3718, 3717, 3716, 3714 };
        private static readonly int[] SmiteBlue = { 3706, 3710, 3709, 3708, 3707 };

        private static readonly Items.Item Zhonya = new Items.Item(3157, 0);
        private static readonly Items.Item Seraph = new Items.Item(3040, 0);
        public static Items.Item TrinketN = new Items.Item(3340, 600f);
        public static Items.Item SightStone = new Items.Item(2049, 600f);
        public static Items.Item WardS = new Items.Item(2043, 600f);
        public static Items.Item WardN = new Items.Item(2044, 600f);
        #endregion

        #region 炮塔血量
        public static void DrawText1(SharpDX.Direct3D9.Font font, String text, int posX, int posY, SharpDX.Color color)
        {
            Rectangle rec = font.MeasureText(null, text, SharpDX.Direct3D9.FontDrawFlags.Center);
            font.DrawText(null, text, posX + 1 + rec.X, posY + 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX + rec.X, posY + 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX - 1 + rec.X, posY - 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX + rec.X, posY - 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX + rec.X, posY, color);
        }
        #endregion

        #region 打野计时
        public static string FormatTime(double time)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            if (t.Minutes > 0)
            {
                return string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds);
            }
            return string.Format("{0:D}", t.Seconds);
        }

        private static readonly List<JungleCamp> jungleCamps = new List<JungleCamp>();
        private static Font mapFont;
        private static Font miniMapFont;
        public class JungleCamp
        {
            public bool IsDead;
            public String Name;
            public string[] Names;
            public int NextRespawnTime;
            public Vector3 Position;
            public int RespawnTime;
            public bool Visibled;

            public JungleCamp(String name, int respawnTime, Vector3 position, string[] names)
            {
                Name = name;
                RespawnTime = respawnTime;
                Position = position;
                Names = names;
                IsDead = false;
                Visibled = false;
            }
        }

        #endregion

        #region 总
        public static void Game_OnGameLoad(EventArgs args)
        {

           Notifications.AddNotification("All In One by NightMoon", 1000);
           Notifications.AddNotification("              And  Lost`", 1000);
           Notifications.AddNotification("Version : 1.0.0.0", 1000);
           Game.PrintChat("Flowers - All In One Loaded!~~~ Version : 1.0.0.0 Thanks for your use!");
           Game.PrintChat("What are the deficiencies please immediately feedback. Thank you! ");

           Huabian.Menu();

           #region 换肤功能
           try
           {
               foreach (var hero in HeroManager.AllHeroes)
               {
                   if (!Huabian.菜单.Item("forall").GetValue<bool>() && hero.Name != ObjectManager.Player.Name)
                   {
                       continue;
                   }

                   var currenthero = hero;

                   var skinselect = Huabian.菜单.SubMenu("Others").AddItem(
                           new MenuItem("skin." + hero.ChampionName, hero.ChampionName + " (" + "Flowers" + ")")
                               .SetValue(
                                   new StringList(
                                       new[]
                                        {
                                            "Skin 0", "Skin 1", "Skin 2", "Skin 3", "Skin 4", "Skin 5", "Skin 6", "Skin 7",
                                            "Skin 8", "Skin 9", "Skin 10"
                                        }, 0)));

                   ChampSkins.Add(hero.Name, skinselect.GetValue<StringList>().SelectedIndex);

                   hero.SetSkin(hero.ChampionName, ChampSkins[hero.Name]);

                   skinselect.ValueChanged += delegate(Object sender, OnValueChangeEventArgs args1)
                   {
                       ChampSkins[currenthero.Name] = args1.GetNewValue<StringList>().SelectedIndex;
                       currenthero.SetSkin(currenthero.ChampionName, ChampSkins[currenthero.Name]);
                   };
               }
           }
           catch (Exception e)
           {
               Console.Write(e + " " + e.StackTrace);
           }
           setupdone = true;

           #endregion

           #region 技能冷却

           #endregion

           #region 自动惩戒

           Summoner1 = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner1);
           Summoner2 = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.Summoner2);
           int level = ObjectManager.Player.Level;
           Plevel = level;

           if (new[] { "s5_summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick", "s5_summonersmiteduel", "summonersmite" }.Contains(Summoner1.Name))
           {
               smite = new Spell(SpellSlot.Summoner1, 570f);
               setSmiteDamage();
           }

           if (new[] { "s5summonersmiteplayerganker", "itemsmiteaoe", "s5_summonersmitequick", "s5_summonersmiteduel", "summonersmite" }.Contains(Summoner2.Name))
           {
               smite = new Spell(SpellSlot.Summoner2, 570f);
               setSmiteDamage();
           }
           #endregion

           #region 炮塔血量

           #endregion

           #region 反隐监测技能
           SpellList.Add(new Spells { ChampionName = "akali", SpellName = "akalismokebomb", slot = SpellSlot.W });   //Akali W
            SpellList.Add(new Spells { ChampionName = "shaco", SpellName = "deceive", slot = SpellSlot.Q }); //Shaco Q
            SpellList.Add(new Spells { ChampionName = "khazix", SpellName = "khazixr", slot = SpellSlot.R }); //Khazix R
            SpellList.Add(new Spells { ChampionName = "khazix", SpellName = "khazixrlong", slot = SpellSlot.R }); //Khazix R Evolved
            SpellList.Add(new Spells { ChampionName = "talon", SpellName = "talonshadowassault", slot = SpellSlot.R }); //Talon R
            SpellList.Add(new Spells { ChampionName = "monkeyking", SpellName = "monkeykingdecoy", slot = SpellSlot.W }); //Wukong W
            SpellList.Add(new Spells { ChampionName = "vayne", SpellName = "vaynetumble", slot = SpellSlot.Q }); //Vayne R-Q
            #endregion

           #region 打野计时

            jungleCamps.Add(new JungleCamp("SRU_Blue", 300, new Vector3(3871.489f, 7901.054f, 51.90324f),new[] { "SRU_Blue1.1.1", "SRU_BlueMini1.1.2", "SRU_BlueMini21.1.3" }));
            jungleCamps.Add(new JungleCamp("SRU_Murkwolf", 100, new Vector3(3780.628f, 6443.984f, 52.4632f),new[] { "SRU_Murkwolf2.1.1", "SRU_MurkwolfMini2.1.2", "SRU_MurkwolfMini2.1.3" }));
            jungleCamps.Add(new JungleCamp("SRU_Razorbeak", 100, new Vector3(6823.895f, 5457.756f, 53.12784f),new[]{ "SRU_Razorbeak3.1.1", "SRU_RazorbeakMini3.1.2", "SRU_RazorbeakMini3.1.3", "SRU_RazorbeakMini3.1.4"}));
            jungleCamps.Add(new JungleCamp("SRU_Red", 300, new Vector3(7862f, 4112f, 53.71951f),new[] { "SRU_Red4.1.1", "SRU_RedMini4.1.2", "SRU_RedMini4.1.3" }));
            jungleCamps.Add(new JungleCamp("SRU_Krug", 100, new Vector3(8532.471f, 2737.948f, 50.58278f),new[] { "SRU_Krug5.1.2", "SRU_KrugMini5.1.1" }));
            jungleCamps.Add(new JungleCamp("SRU_Dragon", 360, new Vector3(9866.148f, 4414.014f, -71.2406f), new[] { "SRU_Dragon6.1.1" }));
            jungleCamps.Add(new JungleCamp( "SRU_Blue", 300, new Vector3(10931.73f, 6990.844f, 51.72291f),new[] { "SRU_Blue7.1.1", "SRU_BlueMini7.1.2", "SRU_BlueMini27.1.3" }));
            jungleCamps.Add(new JungleCamp("SRU_Murkwolf", 100, new Vector3(11008f, 8386f, 62.13136f),new[] { "SRU_Murkwolf8.1.1", "SRU_MurkwolfMini8.1.2", "SRU_MurkwolfMini8.1.3" }));
            jungleCamps.Add(new JungleCamp("SRU_Razorbeak", 100, new Vector3(7986.997f, 9471.389f, 52.34794f),new[]{ "SRU_Razorbeak9.1.1", "SRU_RazorbeakMini9.1.2", "SRU_RazorbeakMini9.1.3", "SRU_RazorbeakMini9.1.4"}));
            jungleCamps.Add(new JungleCamp("SRU_Red", 300, new Vector3(7016.869f, 10775.55f, 56.00922f),new[] { "SRU_Red10.1.1", "SRU_RedMini10.1.2", "SRU_RedMini10.1.3" }));
            jungleCamps.Add(new JungleCamp("SRU_Krug", 100, new Vector3(6317.092f, 12146.46f, 56.4768f),new[] { "SRU_Krug11.1.2", "SRU_KrugMini11.1.1" }));
            jungleCamps.Add(new JungleCamp("SRU_Baron", 420, new Vector3(5007.124f, 10471.45f, -71.2406f), new[] { "SRU_Baron12.1.1" }));
            jungleCamps.Add(new JungleCamp("SRU_Gromp", 100, new Vector3(2090.628f, 8427.984f, 51.77725f), new[] { "SRU_Gromp13.1.1" }));
            jungleCamps.Add(new JungleCamp("SRU_Gromp", 100, new Vector3(12702f, 6444f, 51.69143f), new[] { "SRU_Gromp14.1.1" }));
            jungleCamps.Add(new JungleCamp("Sru_Crab", 180, new Vector3(10500f, 5170f, -62.8102f), new[] { "Sru_Crab15.1.1" }));
            jungleCamps.Add( new JungleCamp("Sru_Crab", 180, new Vector3(4400f, 9600f, -66.53082f), new[] { "Sru_Crab16.1.1" }));

            mapFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Times New Roman", 20));
            miniMapFont = new Font(Drawing.Direct3DDevice, new System.Drawing.Font("Times New Roman", 9));

           #endregion

           #region 重置普攻
            switch (ObjectManager.Player.ChampionName)
            {
                case "Jax": Reset = new Spell(SpellSlot.W);
                    break;
                case "Fiora": Reset = new Spell(SpellSlot.E);
                    break;
                case "Poppy": Reset = new Spell(SpellSlot.Q);
                    break;
                case "MasterYi": Reset = new Spell(SpellSlot.W);
                    break;
                case "Talon": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Leona": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Blitzcrank": Reset = new Spell(SpellSlot.E);
                    break;
                case "Darius": Reset = new Spell(SpellSlot.W);
                    break;
                case "Garen": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Hecarim": Reset = new Spell(SpellSlot.E);
                    break;
                case "Jayce": Reset = new Spell(SpellSlot.W);
                    break;
                case "Kassadin": Reset = new Spell(SpellSlot.W);
                    break;
                case "MissFortune": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Mordekaiser": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Nasus": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Nautilus": Reset = new Spell(SpellSlot.W);
                    break;
                case "Nidalee": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Renekton": Reset = new Spell(SpellSlot.W);
                    break;
                case "Riven": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Sejuani": Reset = new Spell(SpellSlot.W);
                    break;
                case "Shyvana": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Sivir": Reset = new Spell(SpellSlot.W);
                    break;
                case "Trundle": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Vi": Reset = new Spell(SpellSlot.E);
                    break;
                case "Volibear": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Wukong": Reset = new Spell(SpellSlot.Q);
                    break;
                case "XinZhao": Reset = new Spell(SpellSlot.Q);
                    break;
                case "Yorick": Reset = new Spell(SpellSlot.Q);
                    break;
                default: Game.PrintChat("Champion not supported");
                    break;

                    switch (ObjectManager.Player.ChampionName)
                    {
                        case "MasterYi": special = true;
                            break;
                        case "Riven": special2 = true;
                            break;
                        case "Nidalee": special3 = true;
                            break;
                        case "Jayce": special3 = true;
                            break;
                    }
            }
            #endregion

           #region 调用API
            Drawing.OnDraw += 总显示;
            Game.OnUpdate += 主菜单;
            Drawing.OnEndScene += 小地图显示;
            Obj_AI_Base.OnIssueOrder += Obj_AI_Base_OnIssueOrder;
            GameObject.OnFloatPropertyChange += FloatPropertyChange;
            Obj_AI_Base.OnCreate += Obj_AI_Base_OnCreate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
            Obj_AI_Base.OnProcessSpellCast += Zhongyas;
            Orbwalking.AfterAttack += Orbwalking_AfterAttack;
            #endregion

        }

        #endregion

        #region 自动中亚
        private static void Zhongyas(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            
            // return if ally or non hero spell
        
            if (Player.IsDead || (!Zhonya.IsReady() && !Seraph.IsReady()) || !(sender is Obj_AI_Hero) || sender.IsAlly || !args.Target.IsMe || args.SData.IsAutoAttack() || sender.IsMe)
            {
                return;
            }
            DangerousSpells.Data Spellinfo = null;
            try
            {
                Spellinfo = DangerousSpells.GetByName(args.SData.Name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e + e.StackTrace);
                return;
            }
          
      
            if (Spellinfo != null)
            {
                    Game.PrintChat("Attempting to Zhonya: " + args.SData.Name);
                    var delay = Spellinfo.BaseDelay * 1000;
                    if (Zhonya.IsReady())
                    {
                        Utility.DelayAction.Add((int) delay, () => Zhonya.Cast());
                        return;
                    }
                    if (Seraph.IsReady() && Huabian.菜单.Item("enableseraph").GetValue<bool>())
                    {
                        Utility.DelayAction.Add((int) delay, () => Seraph.Cast());
                    }
                    return;
            }

            if (Huabian.菜单.Item("enablehpzhonya").GetValue<bool>() && (Zhonya.IsReady() || Seraph.IsReady()))
                {
                    var incomingspelldmg = sender.GetSpellDamage(Player, args.SData.Name);
                    var calcdmg = sender.CalcDamage(
                        Player, sender.GetDamageSpell(Player, args.SData.Name).DamageType, incomingspelldmg);
                    var remaininghealth = Player.Health - calcdmg;
                    var slidervalue = Huabian.菜单.Item("minspelldmg").GetValue<Slider>().Value / 100f;
                    var hptozhonya = Huabian.菜单.Item("hptozhonya").GetValue<Slider>().Value;
                    var remaininghealthslider = Huabian.菜单.Item("remaininghealth").GetValue<Slider>().Value / 100f;
                    if ((calcdmg / Player.Health) >= slidervalue || Player.HealthPercent <= hptozhonya || remaininghealth <= remaininghealthslider * Player.Health)
                    {
                        Console.WriteLine("Attempting to Zhonya because incoming spell costs " + calcdmg / Player.Health
                            + " of our health.");
                        if (Zhonya.IsReady())
                        {
                            Zhonya.Cast();
                            return;
                        }
                        if (Seraph.IsReady() && Huabian.菜单.Item("enableseraph").GetValue<bool>())
                        {
                            Seraph.Cast();
                        }
                    }
                }
        }

        static void BuffDetector()
        {
            foreach (var buff in Player.Buffs)
            {
                var isbadbuff = DangerousBuffs.ScaryBuffs.ContainsKey(buff.Name);

                if (isbadbuff)
                {
                    var bufftime = DangerousBuffs.ScaryBuffs[buff.Name];
                    if (!Zhonya.IsReady())
                    {
                        if (bufftime.Equals(0))
                        {
                            Zhonya.Cast();
                            return;
                        }
                        delayingzhonya = true;
                        Utility.DelayAction.Add(
                            (int)bufftime, () =>
                            {
                                Zhonya.Cast();
                                delayingzhonya = false;
                            });
                        return;
                    }

                    if (!Zhonya.IsReady() && Huabian.菜单.Item("enableseraph").GetValue<bool>() && !delayingzhonya)
                    {
                        if (bufftime.Equals(0))
                        {
                            Seraph.Cast();
                            return;
                        }
                        Utility.DelayAction.Add((int)bufftime, () => Seraph.Cast());
                    }
                }
            }
        }

        #endregion

        #region 换肤功能
        private static void FloatPropertyChange(GameObject sender, GameObjectFloatPropertyChangeEventArgs args)
        {

            try
            {
                if (!setupdone || !(sender is Obj_AI_Hero) || args.Property != "mHP" || sender.Name != ObjectManager.Player.Name && !Huabian.菜单.Item("forall").GetValue<bool>())
                {
                    return;
                }

                var hero = (Obj_AI_Hero)sender;

                if (args.OldValue.Equals(args.NewValue) && args.NewValue.Equals(hero.MaxHealth) && !hero.IsDead)
                {
                    hero.SetSkin(hero.ChampionName, ChampSkins[hero.Name]);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

        #region 技能冷却

        #endregion

        #region 自动惩戒
        public static Obj_AI_Minion GetNearest(Vector3 pos)
        {
            var minions =
                ObjectManager.Get<Obj_AI_Minion>()
                    .Where(minion => minion.IsValid && MinionNames.Any(name => minion.Name.StartsWith(name)) && !MinionNames.Any(name => minion.Name.Contains("Mini")) && !MinionNames.Any(name => minion.Name.Contains("Spawn")));
            var objAiMinions = minions as Obj_AI_Minion[] ?? minions.ToArray();
            Obj_AI_Minion sMinion = objAiMinions.FirstOrDefault();
            double? nearest = null;
            foreach (Obj_AI_Minion minion in objAiMinions)
            {
                double distance = Vector3.Distance(pos, minion.Position);
                if (nearest == null || nearest > distance)
                {
                    nearest = distance;
                    sMinion = minion;
                }
            }
            return sMinion;
        }

        public static void setSmiteSlot()
        {
            SpellSlot smiteSlot;
            if (SmiteBlue.Any(x => Items.HasItem(x)))
                smiteSlot = ObjectManager.Player.GetSpellSlot("s5_summonersmiteplayerganker");
            else if (SmiteRed.Any(x => Items.HasItem(x)))
                smiteSlot = ObjectManager.Player.GetSpellSlot("s5_summonersmiteduel");
            else if (SmiteGrey.Any(x => Items.HasItem(x)))
                smiteSlot = ObjectManager.Player.GetSpellSlot("s5_summonersmitequick");
            else if (SmitePurple.Any(x => Items.HasItem(x)))
                smiteSlot = ObjectManager.Player.GetSpellSlot("itemsmiteaoe");
            else
                smiteSlot = ObjectManager.Player.GetSpellSlot("summonersmite");
            smite.Slot = smiteSlot;
        }

        private static void setSmiteDamage()
        {
            int level = ObjectManager.Player.Level;
            int[] smitedamage =
            {
                20*level + 370,
                30*level + 330,
                40*level + 240,
                50*level + 100
            };
            damage = smitedamage.Max();
        }

        #endregion

        #region AA
        private static void Orbwalking_AfterAttack(AttackableUnit unit, AttackableUnit target)
        {
            
            if (unit.IsMe && target.IsValidTarget())
            {
                if (target.Type == GameObjectType.obj_AI_Hero && Huabian.菜单.Item("ChampEnabled").GetValue<KeyBind>().Active)
                {
                    if (special == true)
                    {
                        Reset.Cast();
                        ObjectManager.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                    else if (special2 == true)
                    {
                        Reset.Cast(target.Position);
                    }
                    else if (special3 == true && correctForm)
                    {
                        Reset.Cast();
                    }
                    else if (special3 == true && !correctForm)
                    {
                        correctForm = false;
                    }
                    else
                    {
                        Reset.Cast();
                    }
                }
                else if (target.Type == GameObjectType.obj_AI_Turret && Huabian.菜单.Item("TowerEnabled").GetValue<KeyBind>().Active)
                {
                    if (special == true)
                    {
                        Reset.Cast();
                        ObjectManager.Player.IssueOrder(GameObjectOrder.AttackUnit, target);
                    }
                    else if (special2 == true)
                    {
                        Reset.Cast(target.Position);
                    }
                    else if (special3 == true && correctForm)
                    {
                            Reset.Cast();
                    }
                    else if (special3 == true && !correctForm)
                    {
                        correctForm = false;
                    }
                    else
                    {
                        Reset.Cast();
                    }
                }
            }
        }
        #endregion

        #region 反隐监测
        private static void Obj_AI_Base_OnCreate(GameObject sender, EventArgs args)
        {

            if (!Huabian.菜单.Item("Use").GetValue<KeyBind>().Active && !Huabian.菜单.Item("Always").GetValue<bool>())
                return;

            var Rengar = HeroManager.Enemies.Find(x => x.ChampionName.ToLower() == "rengar");

            if (Rengar == null)
                return;

            if (ObjectManager.Player.Distance(sender.Position) < 1600)
            {
                Console.WriteLine("Sender : " + sender.Name);
            }

            if (sender.IsEnemy && sender.Name.Contains("Rengar_Base_R_Alert"))
            {
                if (ObjectManager.Player.HasBuff("rengarralertsound") &&
                !CheckWard() &&
                !Rengar.IsVisible &&
                !Rengar.IsDead &&
                    CheckSlot() != SpellSlot.Unknown)
                {
                    ObjectManager.Player.Spellbook.CastSpell(CheckSlot(), ObjectManager.Player.Position);
                }
            }
        }

        private static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!Huabian.菜单.Item("Use").GetValue<KeyBind>().Active && !Huabian.菜单.Item("Always").GetValue<bool>())
                return;

            if (!sender.IsEnemy || sender.IsDead || !(sender is Obj_AI_Hero))
                return;

            if (SpellList.Exists(x => x.SpellName.Contains(args.SData.Name.ToLower())))
            {
                var _sender = sender as Obj_AI_Hero;

                if (CheckSlot() == SpellSlot.Unknown)
                    return;

                if (CheckWard())
                    return;

                if (ObjectManager.Player.Distance(sender.Position) > 700)
                    return;

                if (args.SData.Name.ToLower().Contains("vaynetumble") && Game.Time > VayneBuffEndTime)
                    return;

                if (Environment.TickCount - Delay > 1500 || Delay == 0)
                {
                    var pos = ObjectManager.Player.Distance(args.End) > 600 ? ObjectManager.Player.Position : args.End;
                    ObjectManager.Player.Spellbook.CastSpell(CheckSlot(), pos);
                    Delay = Environment.TickCount;
                }
            }
        }

        private static SpellSlot CheckSlot()
        {
            SpellSlot slot = SpellSlot.Unknown;

            if (Items.CanUseItem(3362) && Items.HasItem(3362, ObjectManager.Player))
            {
                slot = SpellSlot.Trinket;
            }
            else if (Items.CanUseItem(2043) && Items.HasItem(2043, ObjectManager.Player))
            {
                slot = ObjectManager.Player.GetSpellSlot("VisionWard");
            }
            else if (Items.CanUseItem(3364) && Items.HasItem(3364, ObjectManager.Player))
            {
                slot = SpellSlot.Trinket;
            }
            return slot;
        }

        private static bool CheckWard()
        {
            var status = false;

            foreach (var a in ObjectManager.Get<Obj_AI_Minion>().Where(x => x.Name == "VisionWard"))
            {
                if (ObjectManager.Player.Distance(a.Position) < 450)
                {
                    status = true;
                }
            }

            return status;
        }

        #endregion

        #region 移动延迟
        private static void Obj_AI_Base_OnIssueOrder(Obj_AI_Base sender, GameObjectIssueOrderEventArgs args)
        {
            if (sender == null || !sender.IsValid || !sender.IsMe || args.Order != GameObjectOrder.MoveTo ||
                !Huabian.菜单.Item("MovementEnabled").GetValue<bool>())
            {
                return;
            }

            if (Environment.TickCount - LastMove < Huabian.菜单.Item("MovementDelay").GetValue<Slider>().Value)
            {
                args.Process = false;
                return;
            }

            LastMove = Environment.TickCount;
        }

        #endregion

        #region 小地图显示
        private static void 小地图显示(EventArgs args)
        {

            #region 打野计时
            if (Huabian.菜单.Item("Timer1").GetValue<bool>())
            {
                foreach (JungleCamp jungleCamp in jungleCamps.Where(camp => camp.NextRespawnTime > 0))
                {
                    int timeClock = jungleCamp.NextRespawnTime - (int)Game.ClockTime;
                    string time = Huabian.菜单.Item("JungleTimerFormat").GetValue<StringList>().SelectedIndex == 0
                        ? FormatTime(timeClock)
                        : timeClock.ToString(CultureInfo.InvariantCulture);
                    Vector2 pos = Drawing.WorldToMinimap(jungleCamp.Position);
                    if (timeClock >= 150)
                    {
                        DrawText1(miniMapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.White);
                    }
                    if (timeClock < 150 && timeClock >= 100)
                    {
                        DrawText1(miniMapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.LimeGreen);
                    }
                    if (timeClock < 100 && timeClock >= 50)
                    {
                        DrawText1(miniMapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.Orange);
                    }
                    if (timeClock < 50)
                    {
                        DrawText1(miniMapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.Red);
                    }
                }
            }

            #endregion

            #region 炮塔血量
            if (Huabian.菜单.Item("HealthActive").GetValue<bool>())
            {
                foreach (Obj_AI_Turret turret in ObjectManager.Get<Obj_AI_Turret>())
                {
                    if ((turret.HealthPercentage() == 100))
                    {
                        continue;
                    }
                    int health = 0;
                    switch (Huabian.菜单.Item("TIHealth").GetValue<StringList>().SelectedIndex)
                    {
                        case 0:
                            health = (int)turret.HealthPercentage();
                            break;

                        case 1:
                            health = (int)turret.Health;
                            break;
                    }
                    Vector2 pos = Drawing.WorldToMinimap(turret.Position);
                    var perHealth = (int)turret.HealthPercentage();
                    if (perHealth >= 75)
                    {
                        lost.DrawText1(
                            Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.LimeGreen);
                    }
                    else if (perHealth < 75 && perHealth >= 50)
                    {
                        lost.DrawText1(
                            Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                            SharpDX.Color.YellowGreen);
                    }
                    else if (perHealth < 50 && perHealth >= 25)
                    {
                        lost.DrawText1(
                            Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Orange);
                    }
                    else if (perHealth < 25)
                    {
                        lost.DrawText1(
                            Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Red);
                    }
                }
                foreach (Obj_BarracksDampener inhibitor in ObjectManager.Get<Obj_BarracksDampener>())
                {
                    if (inhibitor.Health != 0 && (inhibitor.Health / inhibitor.MaxHealth) * 100 != 100)
                    {
                        Vector2 pos = Drawing.WorldToMinimap(inhibitor.Position);
                        var health = (int)((inhibitor.Health / inhibitor.MaxHealth) * 100);
                        if (health >= 75)
                        {
                            lost.DrawText1(
                                lost.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.LimeGreen);
                        }
                        else if (health < 75 && health >= 50)
                        {
                            lost.DrawText1(
                                Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.YellowGreen);
                        }
                        else if (health < 50 && health >= 25)
                        {
                            lost.DrawText1(
                                Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.Orange);
                        }
                        else if (health < 25)
                        {
                            lost.DrawText1(
                                Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Red);
                        }
                    }
                }
            }
            #endregion

        }

        #endregion

        #region 总显示
        private static void 总显示(EventArgs args)
        {

            #region 无伤打野

            var show = Huabian.菜单.Item("Show").GetValue<bool>();
            if (show)
            {
                var circleRange = 75f;
                if (Game.MapId == (GameMapId)11)
                {
                    Render.Circle.DrawCircle(new Vector3(7461.018f, 3253.575f, 52.57141f), circleRange, Color.Blue); // blue team :red
                    Render.Circle.DrawCircle(new Vector3(3511.601f, 8745.617f, 52.57141f), circleRange, Color.Blue); // blue team :blue
                    Render.Circle.DrawCircle(new Vector3(7462.053f, 2489.813f, 52.57141f), circleRange, Color.Blue); // blue team :golems
                    Render.Circle.DrawCircle(new Vector3(3144.897f, 7106.449f, 51.89026f), circleRange, Color.Blue); // blue team :wolfs
                    Render.Circle.DrawCircle(new Vector3(7770.341f, 5061.238f, 49.26587f), circleRange, Color.Blue); // blue team :wariaths

                    Render.Circle.DrawCircle(new Vector3(10930.93f, 5405.83f, -68.72192f), circleRange, Color.Yellow); // Dragon

                    Render.Circle.DrawCircle(new Vector3(7326.056f, 11643.01f, 50.21985f), circleRange, Color.Red); // red team :red
                    Render.Circle.DrawCircle(new Vector3(11417.6f, 6216.028f, 51.00244f), circleRange, Color.Red); // red team :blue
                    Render.Circle.DrawCircle(new Vector3(7368.408f, 12488.37f, 56.47668f), circleRange, Color.Red); // red team :golems
                    Render.Circle.DrawCircle(new Vector3(10342.77f, 8896.083f, 51.72742f), circleRange, Color.Red); // red team :wolfs
                    Render.Circle.DrawCircle(new Vector3(7001.741f, 9915.717f, 54.02466f), circleRange, Color.Red); // red team :wariaths                    

                }
                else if (Game.MapId == GameMapId.SummonersRift)
                {
                    Render.Circle.DrawCircle(new Vector3(7444.86f, 2980.26f, 56.26f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(7232.57f, 4671.71f, 51.95f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(7232.57f, 4671.71f, 55.25f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(3402.31f, 8429.14f, 53.79f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(6859.18f, 11497.25f, 52.69f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(7010.90f, 10021.69f, 57.37f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(9850.36f, 8781.23f, 52.63f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(11128.29f, 6225.54f, 54.85f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(10270.61f, 4974.52f, 54f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(7213.78f, 2103.27f, 54.74f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(4142.55f, 5695.95f, 55.26f), circleRange, Color.Blue);
                    Render.Circle.DrawCircle(new Vector3(6905.46f, 12402.21f, 53.68f), circleRange, Color.Blue);
                }
            }
            #endregion

            #region 打野计时
            if (Huabian.菜单.Item("Timer").GetValue<bool>())
            {
                foreach (JungleCamp jungleCamp in jungleCamps.Where(camp => camp.NextRespawnTime > 0))
                {
                    int timeClock = jungleCamp.NextRespawnTime - (int)Game.ClockTime;
                    string time = Huabian.菜单.Item("JungleTimerFormat").GetValue<StringList>().SelectedIndex == 0
                        ? FormatTime(timeClock)
                        : timeClock.ToString(CultureInfo.InvariantCulture);
                    Vector2 pos = Drawing.WorldToScreen(jungleCamp.Position);
                    if (timeClock >= 150)
                    {
                        DrawText1(mapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.White);
                    }
                    if (timeClock < 150 && timeClock >= 100)
                    {
                        DrawText1(mapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.LimeGreen);
                    }
                    if (timeClock < 100 && timeClock >= 50)
                    {
                        DrawText1(mapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.Orange);
                    }
                    if (timeClock < 50)
                    {
                        DrawText1(mapFont, time, (int)pos.X, (int)pos.Y - 8, SharpDX.Color.Red);
                    }
                }
            }
            #endregion

            #region 眼位监测

            if (Huabian.菜单.Item("ta-wposok").GetValue<bool>())
            {
                for (int i = 0; i < pos.Count; ++i)
                {
                    if (TrinketN.IsReady())
                    {
                        posCol = System.Drawing.Color.Green;
                    }
                    else
                    {
                        posCol = System.Drawing.Color.Red;
                    }
                    if (TrinketN.IsInRange(pos[i]))
                    {
                        Render.Circle.DrawCircle(pos[i], 50, posCol);
                    }
                }
            }

            #endregion

        }

        #endregion

        #region 主菜单
        private static void 主菜单(EventArgs args)
        {
            Lantern();//自动灯笼
            Flowers();//开发功能
            Ignite();//自动点燃
            Ward();//进草插眼
            chongzhiAA();//重置普攻
            Timer();//打野计时
            Smite();//自动惩戒
            AAhouyao();//AA后摇
            CDtracker();//技能冷却
        }

        #endregion

        #region 技能冷却
        private static void CDtracker()
        {

        }

        #endregion

        #region 自动惩戒
        private static void Smite()
        {
            setSmiteSlot();
            if (ObjectManager.Player.Level > Plevel)
            {
                Plevel = ObjectManager.Player.Level;
                setSmiteDamage();
            }
            if (Huabian.菜单.Item("Fuckyou").GetValue<KeyBind>().Active)
            {
                mob = GetNearest(ObjectManager.Player.ServerPosition);
                if (mob != null)
                {
                    if (Player.Spellbook.CanUseSpell(smite.Slot) == SpellState.Ready && damage >= mob.Health && Vector3.Distance(ObjectManager.Player.ServerPosition, mob.ServerPosition) <= smite.Range)
                    {
                        Player.Spellbook.CastSpell(smite.Slot, mob);
                    }
                }
            }
        }

        #endregion

        #region 重置普攻
        private static void chongzhiAA()
        {
            if (special3 == true)
            {
                if (ObjectManager.Player.ChampionName == "Jayce" && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "ShockBlast")
                {
                    correctForm = true;
                }
                if (ObjectManager.Player.ChampionName == "Nidalee" && Player.Spellbook.GetSpell(SpellSlot.Q).Name == "Takedown")
                {
                    correctForm = true;
                }
            }
        }
        #endregion

        #region AA后摇
        private static void AAhouyao()
        {
            if (Huabian.菜单.Item("Open").GetValue<bool>())
            {
                {
                    windup = Game.Ping * 1.5;
                }
            }
            else
            {
                windup = 40;
            }
        }
        #endregion

        #region 打野计时
        private static void Timer()
        {
            if (Huabian.菜单.Item("Timer").GetValue<bool>() || Huabian.菜单.Item("Timer1").GetValue<bool>())
            {
                if ((int)Game.ClockTime - _nextTime >= 0)
                {
                    _nextTime = (int)Game.ClockTime + 1;
                    IEnumerable<Obj_AI_Base> minions =
                        ObjectManager.Get<Obj_AI_Base>()
                            .Where(minion => !minion.IsDead && minion.IsValid && minion.Name.ToUpper().StartsWith("SRU"));

                    IEnumerable<JungleCamp> junglesAlive =
                        jungleCamps.Where(
                            jungle =>
                                !jungle.IsDead &&
                                jungle.Names.Any(
                                    s =>
                                        minions.Where(minion => minion.Name == s)
                                            .Select(minion => minion.Name)
                                            .FirstOrDefault() != null));
                    foreach (JungleCamp jungle in junglesAlive)
                    {
                        jungle.Visibled = true;
                    }
                    IEnumerable<JungleCamp> junglesDead =
                        jungleCamps.Where(
                            jungle =>
                                !jungle.IsDead && jungle.Visibled &&
                                jungle.Names.All(
                                    s =>
                                        minions.Where(minion => minion.Name == s)
                                            .Select(minion => minion.Name)
                                            .FirstOrDefault() == null));
                    foreach (JungleCamp jungle in junglesDead)
                    {
                        jungle.IsDead = true;
                        jungle.Visibled = false;
                        jungle.NextRespawnTime = (int)Game.ClockTime + jungle.RespawnTime;
                    }
                    foreach (JungleCamp jungleCamp in
                        jungleCamps.Where(jungleCamp => (jungleCamp.NextRespawnTime - (int)Game.ClockTime) <= 0))
                    {
                        jungleCamp.IsDead = false;
                        jungleCamp.NextRespawnTime = 0;
                    }
                }
            }
		}

        #endregion

        #region 进草插眼
        private static void Ward()
        {
            if (Huabian.菜单.Item("ward").GetValue<bool>() || (Huabian.菜单.Item("ward").GetValue<bool>() && Huabian.菜单.Item("Combo").GetValue<KeyBind>().Active && Huabian.菜单.Item("wardC").GetValue<bool>()))
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
                    else if (SightStone.IsReady())
                        SightStone.Cast(positionWard);
                    else if (WardS.IsReady())
                        WardS.Cast(positionWard);
                    else if (WardN.IsReady())
                        WardN.Cast(positionWard);
                }
            }
        }

        #endregion

        #region 自动点燃
        private static void Ignite()
        {
            if (ObjectManager.Player.GetSpellSlot("SummonerDot") == SpellSlot.Unknown)
            {
                return;
            }

            SummonerDot = new Spell(ObjectManager.Player.GetSpellSlot("SummonerDot"), 550);
            SummonerDot.SetTargetted(0.1f, float.MaxValue);

            if (Huabian.菜单.Item("ignitekey1").GetValue<KeyBind>().Active)
            {
                if (!SummonerDot.IsReady()
                          || ObjectManager.Player.CountEnemiesInRange(SummonerDot.Range) == 0)
                {
                    return;
                }

                Obj_AI_Hero target;

                if (Huabian.菜单.Item("ignitetarget").GetValue<StringList>().SelectedIndex == 0)
                {
                    target =HeroManager.Enemies.Where(hero => hero.IsValidTarget(SummonerDot.Range)).OrderBy(hero => hero.Health).First();
                }
                else
                {
                    target = TargetSelector.GetTarget(ObjectManager.Player,SummonerDot.Range,TargetSelector.DamageType.True);
                }

                if (target != null)
                {
                    SummonerDot.CastOnUnit(target);
                }
            }

            if (SummonerDot.IsReady() && Huabian.菜单.Item("igniteautokill").GetValue<bool>())
            {
                ObjectManager.Get<Obj_AI_Hero>().Where(h =>h.IsValidTarget(SummonerDot.Range)&& h.Health < ObjectManager.Player.GetSummonerSpellDamage(h, Damage.SummonerSpell.Ignite)).Any(enemy => SummonerDot.Cast(enemy).IsCasted());
            }

        }
        public void CastIgnite(EventArgs args)
        {
            ObjectManager.Get<Obj_AI_Hero>().Where(h =>h.IsValidTarget(SummonerDot.Range)&& h.Health < ObjectManager.Player.GetSummonerSpellDamage(h, Damage.SummonerSpell.Ignite)).Any(enemy => SummonerDot.Cast(enemy).IsCasted());
        }
        private void CastIgnite(WndEventArgs args)
        {
            if (args.Msg == (uint)WindowsMessages.WM_KEYDOWN&& ObjectManager.Player.Spellbook.ActiveSpellSlot == SummonerDot.Slot)
            {
                if (!SummonerDot.IsReady() || ObjectManager.Player.CountEnemiesInRange(SummonerDot.Range) == 0)
                {
                    return;
                }

                Obj_AI_Hero target;

                if (Huabian.菜单.Item("ignitetarget").GetValue<StringList>().SelectedIndex == 0)
                {
                    target = HeroManager.Enemies.Where(hero => hero.IsValidTarget(SummonerDot.Range)).OrderBy(hero => hero.Health).First();
                }
                else
                {
                    target = TargetSelector.GetTarget(ObjectManager.Player, SummonerDot.Range, TargetSelector.DamageType.True);
                }

                if (target != null)
                {
                    SummonerDot.CastOnUnit(target);
                }
            }
        }

        #endregion

        #region 自动灯笼
        private static bool IsLow()
        {
            return Player.Health * 100 / Player.MaxHealth <= Huabian.菜单.Item("Low").GetValue<Slider>().Value;
        }

        private static void Lantern()
        {
            if (!(IsLow() && Huabian.菜单.Item("Auto").IsActive()) && !Huabian.菜单.Item("Hotkey").IsActive())
            {
                return;
            }

            var lantern =
                ObjectManager.Get<Obj_AI_Base>().FirstOrDefault(o => o.IsValid && o.IsAlly && o.Name.Equals(LanternName));

            if (lantern != null && Player.Distance(lantern,true) <= 500 && Player.Spellbook.GetSpell((SpellSlot)62).Name.Equals("LanternWAlly"))
            {
                Player.Spellbook.CastSpell((SpellSlot)62, lantern);
            }
        }

        #endregion

        #region 开发功能
        private static void Flowers()
        {

            if (Huabian.菜单.Item("ZoomHack").GetValue<bool>())
            {
                Hacks.ZoomHack = true;
            }
            else
            {
                Hacks.ZoomHack = false;
            }

            if (Huabian.菜单.Item("Cast").GetValue<bool>())
            {
                Hacks.DisableCastIndicator = true;
            }
            else
            {
                Hacks.DisableCastIndicator = false;
            }

            if (Huabian.菜单.Item("TowerRanges").GetValue<bool>())
            {
                Hacks.TowerRanges = true;
            }
            else
            {
                Hacks.TowerRanges = false;
            }

            if (Huabian.菜单.Item("AntiAFK").GetValue<bool>())
            {
                Hacks.AntiAFK = true;
            }
            else
            {
                Hacks.AntiAFK = false;
            }
        }

        #endregion

    }

    #endregion

}
#endregion

#endregion