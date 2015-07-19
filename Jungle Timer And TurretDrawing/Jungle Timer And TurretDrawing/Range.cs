using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;

namespace Jungle_Timer_And_TurretDrawing
{
    class Range
    {
        public static Obj_AI_Hero myHero { get { return ObjectManager.Player; } }
        public static Dictionary<int, Obj_AI_Turret> turretCache = new Dictionary<int, Obj_AI_Turret>();


        internal static void Game_OnGameLoad(EventArgs args)
        {
            Drawing.OnDraw += Drawing_OnDraw;
            Huabian.InitializeCache();
        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Flowers.菜单.Item("Enabled").GetValue<bool>())
            {
                return;
            }

            var turretRange = 800 + myHero.BoundingRadius;

            foreach (var entry in turretCache)
            {
                var turret = entry.Value;

                var circlePadding = 20;

                if (turret == null || !turret.IsValid || turret.IsDead)
                {
                    Utility.DelayAction.Add(1, () => turretCache.Remove(entry.Key));
                    continue;
                }

                if (turret.TotalAttackDamage > 800)
                {
                    continue;
                }

                var distToTurret = myHero.ServerPosition.Distance(turret.Position);
                if (distToTurret < turretRange + 500)
                {
                    var tTarget = turret.Target;
                    if (tTarget.IsValidTarget(float.MaxValue, false))
                    {
                        if (tTarget is Obj_AI_Hero)
                        {
                            Render.Circle.DrawCircle(tTarget.Position, tTarget.BoundingRadius + circlePadding,
                            Color.FromArgb(255, 255, 0, 0), 20);
                        }
                        else
                        {
                            Render.Circle.DrawCircle(tTarget.Position, tTarget.BoundingRadius + circlePadding,
                            Color.FromArgb(255, 0, 255, 0), 10);
                        }
                    }

                    if (tTarget != null && (tTarget.IsMe || (turret.IsAlly && tTarget is Obj_AI_Hero)))
                    {
                        Render.Circle.DrawCircle(turret.Position, turretRange,
                            Color.FromArgb(255, 255, 0, 0), 20);
                    }
                    else
                    {
                        var alpha = distToTurret > turretRange ? (turretRange + 500 - distToTurret) / 2 : 250;
                        Render.Circle.DrawCircle(turret.Position, turretRange,
                            Color.FromArgb((int)alpha, 0, 255, 0), 10);
                    }
                }
            }
        }
    }
}
