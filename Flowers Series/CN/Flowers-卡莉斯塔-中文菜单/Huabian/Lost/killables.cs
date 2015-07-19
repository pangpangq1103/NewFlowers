using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Flowers滑板鞋_重生_
{
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
