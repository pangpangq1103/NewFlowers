using SharpDX;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Jungle_Timer_And_TurretDrawing
{
    class Huabian
    {
        public static string FormatTime(double time)
        {
            TimeSpan t = TimeSpan.FromSeconds(time);
            if (t.Minutes > 0)
            {
                return string.Format("{0:D1}:{1:D2}", t.Minutes, t.Seconds);
            }
            return string.Format("{0:D}", t.Seconds);
        }

        public static void DrawText(Font font, String text, int posX, int posY, Color color)
        {
            Rectangle rec = font.MeasureText(null, text, SharpDX.Direct3D9.FontDrawFlags.Center);
            font.DrawText(null, text, posX + 1 + rec.X, posY + 1, Color.Black);
            font.DrawText(null, text, posX + rec.X, posY + 1, Color.Black);
            font.DrawText(null, text, posX - 1 + rec.X, posY - 1, Color.Black);
            font.DrawText(null, text, posX + rec.X, posY - 1, Color.Black);
            font.DrawText(null, text, posX + rec.X, posY, color);
        }

        public static void DrawText1(SharpDX.Direct3D9.Font font, String text, int posX, int posY, SharpDX.Color color)
        {
            Rectangle rec = font.MeasureText(null, text, SharpDX.Direct3D9.FontDrawFlags.Center);
            font.DrawText(null, text, posX + 1 + rec.X, posY + 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX + rec.X, posY + 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX - 1 + rec.X, posY - 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX + rec.X, posY - 1, SharpDX.Color.Black);
            font.DrawText(null, text, posX + rec.X, posY, color);
        }
        public static Font Text1 = new SharpDX.Direct3D9.Font(Drawing.Direct3DDevice, new FontDescription
        {
            FaceName = "Calibri",
            Height = 13,
            OutputPrecision = FontPrecision.Default,
            Quality = FontQuality.Default,
        });

        internal static void InitializeCache()
        {
            foreach (var obj in ObjectManager.Get<Obj_AI_Turret>())
            {
                if (!Range.turretCache.ContainsKey(obj.NetworkId))
                {
                    Range.turretCache.Add(obj.NetworkId, obj);
                }
            }
        }
    }
}
