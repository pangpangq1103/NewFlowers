using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Linq;

namespace Flowers_Vayne
{
    class VayneE
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;
        private static float VNHunterReborn_lastCondemnCheck;
        private static Vector3 VNHunterReborn_predictedPosition;
        private static Vector3 VNHunterReborn_predictedEndPosition;
        private static readonly Notification VNHunterReborn_CondemnNotification = new Notification("Condemned", 5500);
        public static Vector3 challenge_condemnEndPos;
        public static Vector3 challenge_condemnEndPosSimplified;

        public static void Flowers_VNE()
        {
            var target = TargetSelector.GetTarget(1000, TargetSelector.DamageType.Physical);

            if (target == null)
                return;

            if (CondemnCheck(Player.Position, out target))
            {
                FlowersVayne.E.Cast(target);
            }
        }

        public static bool CondemnCheck(Vector3 Position, out Obj_AI_Hero target)
        {
            if (Position.UnderTurret(true))
            {
                target = null;
                return false;
            }
            foreach (var En in HeroManager.Enemies.Where(hero => hero.IsValidTarget() && hero.Distance(Player.Position) <= FlowersVayne
                .E.Range))
            {
                var EPred = FlowersVayne
                .E.GetPrediction(En);
                int pushDist = FlowersVayne.Menu.Item("nightmoon.e.pushdistance").GetValue<Slider>().Value;
                var FinalPosition = EPred.UnitPosition.To2D().Extend(Position.To2D(), -pushDist).To3D();
                for (int i = 1; i < pushDist; i += (int)En.BoundingRadius)
                {
                    Vector3 loc3 = EPred.UnitPosition.To2D().Extend(Position.To2D(), -i).To3D();
                    var OrTurret = Helpers.UnderTurret(FinalPosition);
                    var OrFountain = Helpers.Fountain(FinalPosition);
                    if (Helpers.Wall(loc3) || OrTurret || OrFountain)
                    {
                        target = En;
                        return true;
                    }
                }
            }
            target = null;
            return false;
        }

        public static void SharpShooter_E()
        {
            foreach (var En in HeroManager.Enemies.Where
                (hero => hero.IsValidTarget(FlowersVayne.E.Range) &&
                    !hero.HasBuffOfType(BuffType.SpellShield) &&
                    !hero.HasBuffOfType(BuffType.SpellImmunity)))
            {

                var EPred = FlowersVayne.E.GetPrediction(En);
                int pushDist = 425;
                var FinalPosition = EPred.UnitPosition.To2D().Extend(Player.ServerPosition.To2D(), -pushDist).To3D();

                for (int i = 1; i < pushDist; i += (int)En.BoundingRadius)
                {
                    Vector3 loc3 = EPred.UnitPosition.To2D().Extend(Player.ServerPosition.To2D(), -i).To3D();

                    if (loc3.IsWall() || SharpShooter_E_isAllyFountain(FinalPosition))
                        FlowersVayne.E.Cast(En);
                }
            }
        }

        private static bool SharpShooter_E_isAllyFountain(Vector3 FinalPosition)
        {
            float fountainRange = 750;
            var map = Utility.Map.GetMap();
            if (map != null && map.Type == Utility.Map.MapType.SummonersRift)
            {
                fountainRange = 1050;
            }
            return
                ObjectManager.Get<GameObject>().Where
                    (spawnPoint => spawnPoint is Obj_SpawnPoint && spawnPoint.IsAlly)
                        .Any(spawnPoint => Vector2.Distance
                            (FinalPosition.To2D(), spawnPoint.Position.To2D())
                                < fountainRange);
        }

        public static void VNHunterRoborn_E()
        {
            Obj_AI_Hero target;

            if (VNHunterRoborn_CondemnCheck(ObjectManager.Player.ServerPosition, out target))
            {
                if (target.IsValidTarget(FlowersVayne.E.Range) && (target is Obj_AI_Hero))
                {
                    FlowersVayne.E.Cast(target);
                }
            }
        }

        public static bool VNHunterRoborn_CondemnCheck(Vector3 fromPosition, out Obj_AI_Hero ts)
        {
            if (Environment.TickCount - VNHunterReborn_lastCondemnCheck < 150)
            {
                ts = null;
                return false;
            }
            VNHunterReborn_lastCondemnCheck = Environment.TickCount;

            if ((fromPosition.UnderTurret(true) || !FlowersVayne.E.IsReady()))
            {
                ts = null;
                return false;
            }
            if (
                !HeroManager.Enemies.Any(
                    h =>
                        h.IsValidTarget(FlowersVayne.E.Range) && !h.HasBuffOfType(BuffType.SpellShield) &&
                        !h.HasBuffOfType(BuffType.SpellImmunity)))
            {
                ts = null;
                return false;
            }
            foreach (var target in HeroManager.Enemies.Where(h => h.IsValidTarget(FlowersVayne.E.Range) &&
                !h.HasBuffOfType(BuffType.SpellShield) && !h.HasBuffOfType(BuffType.SpellImmunity)))
            {
                var pushDistance = FlowersVayne.Menu.Item("nightmoon.e.pushdistance").GetValue<Slider>().Value;
                var targetPosition = target.ServerPosition;
                var finalPosition = targetPosition.Extend(fromPosition, -pushDistance);
                var numberOfChecks = (float)Math.Ceiling(pushDistance / 30f);

                if (!target.Equals(FlowersVayne.Orbwalker.GetTarget()))
                {
                    continue;
                }

                for (var i = 1; i <= 30; i++)
                {
                    var v3 = (targetPosition - fromPosition).Normalized();
                    var extendedPosition = targetPosition + v3 * (numberOfChecks * i);
                    var j4Flag = Helpers.IsJ4FlagThere(extendedPosition, target);
                    if ((extendedPosition.IsWall() || j4Flag) && (target.Path.Count() < 2) && !target.IsDashing())
                    {

                        if (target.Health + 10 <=
                            ObjectManager.Player.GetAutoAttackDamage(target))
                        {
                            ts = null;
                            return false;
                        }

                        if (extendedPosition.UnderTurret(true))
                        {
                            ts = null;
                            return false;
                        }

                        if (NavMesh.IsWallOfGrass(extendedPosition, 25) && FlowersVayne.trinketSpell != null)
                        {
                            var wardPosition = ObjectManager.Player.ServerPosition.Extend(
                                extendedPosition,
                                ObjectManager.Player.ServerPosition.Distance(extendedPosition) - 25f);
                            Utility.DelayAction.Add(250, () => FlowersVayne.trinketSpell.Cast(wardPosition));
                        }

                        VNHunterReborn_CondemnNotification.Text = "Condemned " + target.ChampionName;
                        VNHunterReborn_predictedEndPosition = extendedPosition;
                        VNHunterReborn_predictedPosition = targetPosition;

                        ts = target;
                        return true;
                    }
                }
            }
            ts = null;
            return false;
        }

        public static void guso_E()
        {
            foreach (var hero in from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f))
                                 let prediction = FlowersVayne.E.GetPrediction(hero)
                                     where NavMesh.GetCollisionFlags(
                                         prediction.UnitPosition.To2D()
                                             .Extend(Player.ServerPosition.To2D(),
                                                 -FlowersVayne.Menu.Item("nightmoon.e.pushdistance").GetValue<Slider>().Value)
                                             .To3D())
                                         .HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(
                                             prediction.UnitPosition.To2D()
                                                 .Extend(Player.ServerPosition.To2D(),
                                                     -(FlowersVayne.Menu.Item("nightmoon.e.pushdistance").GetValue<Slider>().Value / 2))
                                                 .To3D())
                                             .HasFlag(CollisionFlags.Wall)
                                     select hero)
            {
                FlowersVayne.E.Cast(hero);
            }
        }

        public static void FlowersText_E()
        {
            if (ShouldSaveCondemn() || (Player.UnderTurret(true))) return;
            var condemnTargets =
                HeroManager.Enemies.Where(
                    h => Player.Distance(h.ServerPosition) < FlowersVayne.E.Range && !h.HasBuffOfType(BuffType.SpellShield));

            foreach (var hero in condemnTargets)
            {
                var pushDist = Player.ServerPosition.Distance(hero.ServerPosition) + 395;
                if (hero.IsDashing())
                {
                    if (Player.ServerPosition.Extend(hero.GetDashInfo().EndPos.To3D(), -400).challenge_IsCollisionable())
                    {
                        FlowersVayne.E.Cast(hero);
                    }
                    break;
                }

                challenge_condemnEndPosSimplified = Player.ServerPosition.To2D().Extend(hero.ServerPosition.To2D(), pushDist).To3D();

                challenge_condemnEndPos = Player.ServerPosition.To2D().Extend(hero.ServerPosition.To2D(), pushDist).To3D();

                if (challenge_condemnEndPos.challenge_IsCollisionable())
                {
                    if (FlowersVayne.Menu.Item("nightmoon.e.fash").GetValue<Boolean>())
                    {
                        if (hero.IsFacing(Player) || !hero.CanMove || !hero.IsMoving)
                        {
                            FlowersVayne.E.Cast(hero);
                        }
                        return;
                    }

                    if (!hero.CanMove || hero.GetWaypoints().Count <= 1 || !hero.IsMoving)
                    {
                        FlowersVayne.E.Cast(hero);
                        return;
                    }

                    var wayPoints = hero.GetWaypoints();
                    var wCount = wayPoints.Count;

                    if (wayPoints.Count(w => Player.ServerPosition.Extend(w.To3D(), pushDist).challenge_IsCollisionable()) >=
                        wCount)
                    {
                        FlowersVayne.E.Cast(hero);
                        return;
                    }
                }
            }
        }

        private static bool ShouldSaveCondemn()
        {
            if (HeroManager.Enemies.Any(h => h.CharData.BaseSkinName == "Katarina" && h.Distance(Player,true) < 1400 && !h.IsDead && h.IsValidTarget()))
            {
                var katarina = HeroManager.Enemies.FirstOrDefault(h => h.CharData.BaseSkinName == "Katarina");
                var kataR = katarina.GetSpell(SpellSlot.R);
                if (katarina != null)
                {
                    return katarina.IsValid<Obj_AI_Hero>() && kataR.IsReady() ||
                           (katarina.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready);
                }
            }
            if (HeroManager.Enemies.Any(h => h.CharData.BaseSkinName == "Galio" && h.Distance(Player, true) < 1400 && !h.IsDead && h.IsValidTarget()))
            {
                var galio = HeroManager.Enemies.FirstOrDefault(h => h.CharData.BaseSkinName == "Galio");
                if (galio != null)
                {
                    var galioR = galio.GetSpell(SpellSlot.R);
                    return galio.IsValidTarget() && galioR.IsReady() ||
                           (galio.Spellbook.CanUseSpell(SpellSlot.R) == SpellState.Ready);
                }
            }
            return false;

        }

        public static void maskerman_E()
        {
            foreach (var hero in from hero in ObjectManager.Get<Obj_AI_Hero>().Where(hero => hero.IsValidTarget(550f))
                                 let prediction = FlowersVayne.E.GetPrediction(hero)
                                 where NavMesh.GetCollisionFlags(
                                     prediction.UnitPosition.To2D()
                                         .Extend(Player.ServerPosition.To2D(),
                                             -FlowersVayne.Menu.Item("nightmoon.e.pushdistance").GetValue<Slider>().Value)
                                         .To3D())
                                     .HasFlag(CollisionFlags.Wall) || NavMesh.GetCollisionFlags(
                                         prediction.UnitPosition.To2D()
                                             .Extend(Player.ServerPosition.To2D(),
                                                 -(FlowersVayne.Menu.Item("nightmoon.e.pushdistance").GetValue<Slider>().Value / 2))
                                             .To3D())
                                         .HasFlag(CollisionFlags.Wall)
                                 select hero)
            {
                FlowersVayne.E.Cast(hero);
            }
        }

        public static void i_E()
        {
            foreach (var target in
                HeroManager.Enemies.Where(
                h =>
                    h.IsValidTarget(FlowersVayne.E.Range) && !h.HasBuffOfType(BuffType.SpellShield)
                    && !h.HasBuffOfType(BuffType.SpellImmunity)))
            {
                const int PushDistance = 400;
                var targetPosition = target.ServerPosition;
                var endPosition = targetPosition.Extend(ObjectManager.Player.ServerPosition, -PushDistance);
                for (var i = 0; i < PushDistance; i += (int)target.BoundingRadius)
                {
                    var extendedPosition = targetPosition.Extend(ObjectManager.Player.ServerPosition, -i);
                    if (extendedPosition.IsWall() || endPosition.IsWall())
                    {
                        FlowersVayne.E.Cast(target);
                        break;
                    }
                }
            }
        }
    }
}
