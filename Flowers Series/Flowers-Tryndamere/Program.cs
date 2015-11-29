using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using ItemData = LeagueSharp.Common.Data.ItemData;
using Item = LeagueSharp.Common.Items.Item;

namespace 花边_蛮子
{
    class Program
    {
        static Obj_AI_Hero Player = ObjectManager.Player;
        static Menu Menu;
        static Spell Q, W, E, R;
        static Orbwalking.Orbwalker Orbwalker;
        static SpellSlot Dot;
        static Item Tiamat = ItemData.Tiamat_Melee_Only.GetItem(); //提亚马特
        static Item Hydra = ItemData.Ravenous_Hydra_Melee_Only.GetItem(); //九头蛇
        static Item Cutlass = ItemData.Bilgewater_Cutlass.GetItem(); //弯刀
        static Item BotRK = ItemData.Blade_of_the_Ruined_King.GetItem(); //破败
        static Item Omen = ItemData.Randuins_Omen.GetItem(); //蓝盾

        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        static void Game_OnGameLoad(EventArgs args)
        {
            if(Player.ChampionName != "Tryndamere")
            {
                return;
            }

            LoadTryndamereSpells();

            Menu = new Menu("Flowers - Tryndamere", "FLTD", true);

            Menu.AddItem(new MenuItem("nightmoon.notify.2", "                         ", true));
            Menu.AddItem(new MenuItem("nightmoon.menu.language", "Language Switch (Need F5): ").SetValue(new StringList(new[] { "English", "Chinese" }, 0)));
            Menu.AddItem(new MenuItem("nightmoon.notify.3", "                         ", true));
            Menu.AddItem(new MenuItem("nightmoon.Credit", "Credit:NightMoon", true));

            if (Menu.Item("nightmoon.menu.language").GetValue<StringList>().SelectedIndex == 0)
            {
                LoadMenu();
            }
            else if (Menu.Item("nightmoon.menu.language").GetValue<StringList>().SelectedIndex == 1)
            {
                LoadTryndamereMenu();
            }
            

            Menu.AddToMainMenu();
            AttackableUnit.OnDamage += AttackableUnit_OnDamage;
            Game.OnUpdate += Game_OnUpdate;
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Base_OnProcessSpellCast;
        }

        static void Obj_AI_Base_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name == "ItemTitanicHydraCleave")
            {
                Orbwalking.ResetAutoAttackTimer();
            }
        }

        static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead || Player.IsRecalling())
            {
                return;
            }

            var UseEFlee = Menu.Item("nightmoon.useEFlee.Spells").GetValue<KeyBind>();

            switch (Orbwalker.ActiveMode)
            {
                case Orbwalking.OrbwalkingMode.Combo:
                    Combo();
                    break;
                case Orbwalking.OrbwalkingMode.Mixed:
                    Harass();
                    break;
                case Orbwalking.OrbwalkingMode.LaneClear:
                    Clear();
                    break;
            }

            KillSteal();

            if (UseEFlee.Active)
            {
                E.Cast(Game.CursorPos);
                Player.IssueOrder(GameObjectOrder.MoveTo, Game.CursorPos);
            }
        }

        static void KillSteal()
        {
            var UseE = Menu.Item("nightmoon.useE.Spells").GetValue<bool>();
            var UseDot = Menu.Item("nightmoon.useDoT.summers").GetValue<bool>();

            if (UseDot && Dot.IsReady())
            {
                var target = TargetSelector.GetTarget(600, TargetSelector.DamageType.True);
                if (target != null)
                {
                    if(Dot.IsReady() && target.IsValidTarget(600) && target.Health + 5 < Player.GetSummonerSpellDamage(target, Damage.SummonerSpell.Ignite))
                    {
                        Player.Spellbook.CastSpell(Dot, target);
                    }
                }
            }

            if (UseE && E.IsReady())
            {
                var target = E.GetTarget(E.Width);
                if (target != null && E.IsKillable(target))
                {
                    var predE = E.GetPrediction(target, true);
                    if (predE.Hitchance >= E.MinHitChance)
                    {
                        E.Cast(predE.CastPosition.Extend(Player.ServerPosition, -100));
                    }
                }
            }
        }

        static void Clear()
        {
            var UseE = Menu.Item("nightmoon.useE.Spells").GetValue<bool>();
            var tiamat = Menu.Item("nightmoon.useTyHt.items").GetValue<bool>();
            var minions = MinionManager.GetMinions(E.Range, MinionTypes.All, MinionTeam.NotAlly, MinionOrderTypes.MaxHealth);

            if (!minions.Any())
            {
                return;
            }

            if (UseE && E.IsReady())
            {
                var pos = E.GetLineFarmLocation(minions.Cast<Obj_AI_Base>().ToList(), 200);
                if (pos.MinionsHit > 0)
                {
                    E.Cast(pos.Position.Extend(Player.ServerPosition.To2D(), -100));
                }
            }

            if (tiamat)
            {
                var item = Hydra.IsReady() ? Hydra : Tiamat;
                if (item.IsReady() &&
                    (minions.Count(i => item.IsInRange(i)) > 2 ||
                     minions.Any(i => i.MaxHealth >= 1200 && i.Distance(Player) < item.Range - 80)))
                {
                    item.Cast();
                }
            }
        }

        static void Harass()
        {
            var UseE = Menu.Item("nightmoon.useE.Spells").GetValue<bool>();

            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
            var predE = E.GetPrediction(target, true);

            if (target != null)
            {
                if (predE.Hitchance >= E.MinHitChance && E.IsReady() && UseE)
                {
                    E.Cast(predE.CastPosition.Extend(Player.ServerPosition, -100));
                }
            }
        }

        static void Combo()
        {
            var UseW = Menu.Item("nightmoon.useW.Spells").GetValue<bool>();
            var UseWftf = Menu.Item("nightmoon.useWfacetoface.Spells").GetValue<bool>();
            var UseE = Menu.Item("nightmoon.useE.Spells").GetValue<bool>();
            var UseDot = Menu.Item("nightmoon.useDoT.summers").GetValue<bool>();
            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
            var predE = E.GetPrediction(target, true);

            ItemUse();

            if (target != null)
            {
                if (UseW && UseWftf && target.IsFacing(Player) && Orbwalker.InAutoAttackRange(target) &&
                    Player.GetAutoAttackDamage(target, true) < target.GetAutoAttackDamage(Player, true))
                {
                    W.Cast();
                }

                if (UseW && W.IsReady() && !target.IsFacing(Player) && Orbwalker.InAutoAttackRange(target))
                {
                    W.Cast();
                }

                if (predE.Hitchance >= E.MinHitChance && E.IsReady() && UseE)
                {
                    E.Cast(predE.CastPosition.Extend(Player.ServerPosition, -100));
                }
            }

            if (UseDot && Dot.IsReady())
            {
                Player.Spellbook.CastSpell(Dot,target);
            }
        }

        static void ItemUse()
        {
            var tiamat = Menu.Item("nightmoon.useTyHt.items").GetValue<bool>();
            var botrk = Menu.Item("nightmoon.useCtBk.items").GetValue<bool>();
            var omen = Menu.Item("nightmoon.useOmen.items").GetValue<bool>();

            var target = TargetSelector.GetTarget(1500f, TargetSelector.DamageType.Physical);

            if (Items.HasItem(Hydra.Id, Player) && Hydra.IsReady() && target.IsValidTarget(Hydra.Range) && tiamat)
            {
                Hydra.Cast();
            }

            if (Items.HasItem(Tiamat.Id, Player) && Tiamat.IsReady() && target.IsValidTarget(Tiamat.Range) && tiamat)
            {
                Tiamat.Cast();
            }

            if (Items.HasItem(Cutlass.Id, Player) && Cutlass.IsReady() && target.IsValidTarget(Cutlass.Range) && botrk)
            {
                Cutlass.Cast(target);
            }

            if (Items.HasItem(BotRK.Id, Player) && BotRK.IsReady() && target.IsValidTarget(BotRK.Range) && botrk)
            {
                BotRK.Cast(target);
            }

            if (Items.HasItem(Omen.Id, Player) && Omen.IsReady() && target.IsValidTarget(Omen.Range) && omen)
            {
                if (Player.IsFacing(target) && !target.IsFacing(Player) || !Player.IsFacing(target) && target.IsFacing(Player))
                {
                    Omen.Cast();
                }
            }
        }

        static void AttackableUnit_OnDamage(AttackableUnit sender, AttackableUnitDamageEventArgs args)
        {
            var UseQ = Menu.Item("nightmoon.useQ.Spells").GetValue<bool>();
            var UseQBelow = Menu.Item("nightmoon.useQbelow.Spells").GetValue<Slider>();
            var UseR = Menu.Item("nightmoon.useR.Spells").GetValue<bool>();
            var UseRBelow = Menu.Item("nightmoon.useRbelow.Spells").GetValue<Slider>();

            if (args.TargetNetworkId != Player.NetworkId || Orbwalker.ActiveMode != Orbwalking.OrbwalkingMode.Combo)
            {
                return;
            }

            if (R.IsReady() && UseR && Player.HealthPercent < UseRBelow.Value && Player.HasBuff("UndyingRage"))
            {
                R.Cast();
            }

            if(UseQ && Player.HealthPercent < UseQBelow.Value)
            {
                Q.Cast();
            }
        }

        static void LoadTryndamereMenu()
        {
            var orbMenu = new Menu("[FL] 走砍设置", "nightmoon.orbwalker.menu");
            Orbwalker = new Orbwalking.Orbwalker(orbMenu);
            Menu.AddSubMenu(orbMenu);

            var SpellsMenu = new Menu("[FL] 技能设置","nightmoon.spells.menu");
            SpellsMenu.AddItem(new MenuItem("nightmoon.useQ.Spells", "使用 Q", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useQbelow.Spells", "使用Q丨血量低于", true).SetValue(new Slider(20, 0, 100)));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useW.Spells", "使用 W", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useWfacetoface.Spells", "使用 W(面对面)", true).SetValue(false));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useE.Spells", "使用 E", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useEKillsteal.Spells", "使用 E 击杀", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useEFlee.Spells", "使用 E 逃跑")).SetValue(new KeyBind('Z', KeyBindType.Press));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useR.Spells", "使用 R", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useRbelow.Spells", "使用R丨血量低于").SetValue(new Slider(15, 0, 95)));
            Menu.AddSubMenu(SpellsMenu);

            Menu.AddItem(new MenuItem("nightmoon.notify.1", "                         ", true));

            var itemssummersMenu = new Menu("[FL] 物品召唤师技能","nightmoon.items&summers.menu");
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useDoT.summers", "使用 点燃", true).SetValue(true));
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useTyHt.items", "使用 提亚马特/九头蛇", true).SetValue(true));
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useCtBk.items", "使用 弯刀/破败", true).SetValue(true));
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useOmen.items", "使用 蓝盾", true).SetValue(true));
            Menu.AddSubMenu(itemssummersMenu);
        }

        private static void LoadMenu()
        {
            var orbMenu = new Menu("[FL] Orbwalker Setting", "nightmoon.orbwalker.menu");
            Orbwalker = new Orbwalking.Orbwalker(orbMenu);
            Menu.AddSubMenu(orbMenu);

            Menu.AddItem(new MenuItem("nightmoon.notify.0", "                          "));

            var SpellsMenu = new Menu("[FL] Spells Setting", "nightmoon.spells.menu");
            SpellsMenu.AddItem(new MenuItem("nightmoon.useQ.Spells", "Auto Q", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useQbelow.Spells", "Use Q丨If Hp <=%", true).SetValue(new Slider(20, 0, 100)));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useW.Spells", "Auto W", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useWfacetoface.Spells", "Auto W(Face to Face)", true).SetValue(false));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useE.Spells", "Auto E", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useEKillsteal.Spells", "Auto E Killsteal", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useEFlee.Spells", "Flee Key")).SetValue(new KeyBind('Z', KeyBindType.Press));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useR.Spells", "Auto R", true).SetValue(true));
            SpellsMenu.AddItem(new MenuItem("nightmoon.useRbelow.Spells", "Use R丨If Hp <=%").SetValue(new Slider(15, 0, 95)));
            Menu.AddSubMenu(SpellsMenu);

            Menu.AddItem(new MenuItem("nightmoon.notify.1", "                         ", true));

            var itemssummersMenu = new Menu("[FL] Summoner&Items", "nightmoon.items&summers.menu");
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useDoT.summers", "Use Ignite", true).SetValue(true));
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useTyHt.items", "Use Tiamat/Hydra", true).SetValue(true));
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useCtBk.items", "Use Cutlass/BotRK", true).SetValue(true));
            itemssummersMenu.AddItem(new MenuItem("nightmoon.useOmen.items", "Use Omem", true).SetValue(true));
            Menu.AddSubMenu(itemssummersMenu);
        }

        static void LoadTryndamereSpells()
        {
            Q = new Spell(SpellSlot.Q);
            W = new Spell(SpellSlot.W, 440f);
            E = new Spell(SpellSlot.E, 660f);
            E.SetSkillshot(0f, 70f, 600, false, SkillshotType.SkillshotLine);
            R = new Spell(SpellSlot.R);

            Dot = Player.GetSpellSlot("summonerdot");
        }
    }
}
