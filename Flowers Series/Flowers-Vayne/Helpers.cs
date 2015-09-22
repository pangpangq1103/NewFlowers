using ClipperLib;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using Color = System.Drawing.Color;
using Path = System.Collections.Generic.List<ClipperLib.IntPoint>;


namespace Flowers_Vayne
{
    internal static class Helpers
    {
        public static List<Obj_AI_Hero> _heroes;
        public static void Load()
        {
            _heroes = ObjectManager.Get<Obj_AI_Hero>().ToList();
        }
        public static bool IsJ4FlagThere(Vector3 position, Obj_AI_Hero target)
        {
            return ObjectManager.Get<Obj_AI_Base>().Any(m => m.Distance(position) <= target.BoundingRadius && m.Name == "Beacon");
        }
        public static bool challenge_IsCollisionable(this Vector3 pos)
        {
            return NavMesh.GetCollisionFlags(pos).HasFlag(CollisionFlags.Wall) ||
                (NavMesh.GetCollisionFlags(pos).HasFlag(CollisionFlags.Building) && ObjectManager.Player.Distance(pos) < 550);
        }
        public static bool UnderTurret(Vector3 Position)
        {
            foreach (var tur in ObjectManager.Get<Obj_AI_Turret>().Where(turr => turr.IsAlly && (turr.Health != 0)))
            {
                if (tur.Distance(Position) <= 975f) return true;
            }
            return false;
        }
        public static bool UltActive()
        {
            return FlowersVayne.Player.Buffs.Any(b => b.Name.ToLower().Contains("vayneinquisition"));
        }
        public static List<Obj_AI_Hero> EnemyHeroes
        {
            get { return _heroes.FindAll(h => h.IsEnemy); }
        }
        public static bool TumbleActive()
        {
            return FlowersVayne.Player.Buffs.Any(b => b.Name.ToLower().Contains("vaynetumblebonus"));
        }
        public static bool Fountain(Vector3 Position)
        {
            float fountainRange = 750;
            var map = Utility.Map.GetMap();
            if (map != null && map.Type == Utility.Map.MapType.SummonersRift)
            {
                fountainRange = 1050;
            }
            return
                ObjectManager.Get<GameObject>()
                    .Where(spawnPoint => spawnPoint is Obj_SpawnPoint && spawnPoint.IsAlly)
                    .Any(
                        spawnPoint =>
                            Vector2.Distance(Position.To2D(), spawnPoint.Position.To2D()) <
                            fountainRange);
        }
        public static bool Wall(Vector3 Pos)
        {
            return Pos.IsWall();
        }

        public static bool UnderTurret(this Vector3 pos, GameObjectTeam TurretTeam)
        {
            return ObjectManager.Get<Obj_AI_Turret>().Any(t => t.Distance(pos) < 600 && t.Team == TurretTeam);
        }
        public static int FlashTime = 0;
        public static Vector2 Center;
        public static float Radius;

        public static Vector3 GetTumblePos(this Obj_AI_Hero target)
        {
            //if the target is not a melee and he's alone he's not really a danger to us, proceed to 1v1 him :^ )
            if (!target.IsMelee && ObjectManager.Player.CountEnemiesInRange(800) == 1) return Game.CursorPos;

            var flash = FlowersVayne.Flash;
            var flashedAtTick = FlashTime;
            if (!flash.IsReady())
            {
                if (Environment.TickCount - flashedAtTick < 500) return Vector3.Zero;
            }

            var aRC = new Geometry.Circle(ObjectManager.Player.ServerPosition.To2D(), 300).ToPolygon().ToClipperPath();
            var cP = Game.CursorPos;
            var tP = target.ServerPosition;
            var pList = new List<Vector3>();
            var additionalDistance = (0.106 + Game.Ping / 2000f) * target.MoveSpeed;

            if ((!cP.IsWall() && !cP.UnderTurret(true) && cP.Distance(tP) > 325 && cP.Distance(tP) < 550 &&
                 (cP.CountEnemiesInRange(425) <= cP.CountAlliesInRange(325)))) return cP;

            foreach (var p in aRC)
            {
                var v3 = new Vector2(p.X, p.Y).To3D();

                if (target.IsFacing(ObjectManager.Player))
                {
                    if (!v3.IsWall() && !v3.UnderTurret(true) && v3.Distance(tP) > 325 && v3.Distance(tP) < 550 &&
                        (v3.CountEnemiesInRange(425) <= v3.CountAlliesInRange(325))) pList.Add(v3);
                }
                else
                {
                    if (!v3.IsWall() && !v3.UnderTurret(true) && v3.Distance(tP) > 325 &&
                        v3.Distance(tP) < (550 - additionalDistance) &&
                        (v3.CountEnemiesInRange(425) <= v3.CountAlliesInRange(325))) pList.Add(v3);
                }
            }
            if (ObjectManager.Player.UnderTurret() || ObjectManager.Player.CountEnemiesInRange(800) == 1)
            {
                return pList.Count > 1 ? pList.OrderBy(el => el.Distance(cP)).FirstOrDefault() : Vector3.Zero;
            }
            return pList.Count > 1 ? pList.OrderByDescending(el => el.Distance(tP)).FirstOrDefault() : Vector3.Zero;
        }
        public static bool OkToQ(Vector3 position)
        {
            if (position.UnderTurret(true) && !ObjectManager.Player.UnderTurret(true))
                return false;
            var allies = position.CountAlliesInRange(ObjectManager.Player.AttackRange);
            var enemies = position.CountEnemiesInRange(ObjectManager.Player.AttackRange);
            var lhEnemies = GetLhEnemiesNearPosition(position, ObjectManager.Player.AttackRange).Count();

            if (enemies == 1) //It's a 1v1, safe to assume I can E
            {
                return true;
            }

            //Adding 1 for the Player
            return (allies + 1 > enemies - lhEnemies);
        }
        public static List<Obj_AI_Hero> GetLhEnemiesNearPosition(Vector3 position, float range)
        {
            return HeroManager.Enemies.Where(hero => hero.IsValidTarget(range, true, position) && hero.HealthPercentage() <= 15).ToList();
        }
    }
    public static class Geometry
    {
        private const int CircleLineSegmentN = 22;

        public class Circle
        {
            public Vector2 Center;
            public float Radius;
            public Circle(Vector2 center, float radius)
            {
                Center = center;
                Radius = radius;
            }
            public Polygon ToPolygon(int offset = 0, float overrideWidth = -1)
            {
                var result = new Polygon();
                var outRadius = (overrideWidth > 0
                    ? overrideWidth
                    : (offset + Radius) / (float)Math.Cos(2 * Math.PI / CircleLineSegmentN));
                for (var i = 1; i <= CircleLineSegmentN; i++)
                {
                    var angle = i * 2 * Math.PI / CircleLineSegmentN;
                    var point = new Vector2(
                        Center.X + outRadius * (float)Math.Cos(angle), Center.Y + outRadius * (float)Math.Sin(angle));
                    result.Add(point);
                }
                return result;
            }
        }

        public class Polygon
        {
            public List<Vector2> Points = new List<Vector2>();

            public void Add(Vector2 point)
            {
                Points.Add(point);
            }

            public Path ToClipperPath()
            {
                var result = new Path(Points.Count);
                foreach (var point in Points)
                {
                    result.Add(new IntPoint(point.X, point.Y));
                }
                return result;
            }

            public bool IsOutside(Vector2 point)
            {
                var p = new IntPoint(point.X, point.Y);
                return Clipper.PointInPolygon(p, ToClipperPath()) != 1;
            }

            public void Draw(Color color, int width = 1)
            {
                for (var i = 0; i <= Points.Count - 1; i++)
                {
                    var nextIndex = (Points.Count - 1 == i) ? 0 : (i + 1);
                    Util.DrawLineInWorld(Points[i].To3D(), Points[nextIndex].To3D(), width, color);
                }
            }
        }
        public static class Util
        {
            public static void DrawLineInWorld(Vector3 start, Vector3 end, int width, Color color)
            {
                var from = Drawing.WorldToScreen(start);
                var to = Drawing.WorldToScreen(end);
                Drawing.DrawLine(from[0], from[1], to[0], to[1], width, color);
            }
        }
    }
}
