using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using SharpDX.Direct3D9;
using Font = SharpDX.Direct3D9.Font;
using Color = System.Drawing.Color;
using System.Globalization;

namespace Jungle_Timer_And_TurretDrawing
{
    class Tower
    {

        public static void Game_OnGameLoad(EventArgs args)
        {
            Game.OnUpdate += Game_OnGameUpdate;
            Drawing.OnEndScene += 小地图显示;
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            if (Flowers.菜单.Item("HealthActive").GetValue<bool>())
            {
                foreach (Obj_AI_Turret turret in ObjectManager.Get<Obj_AI_Turret>())
                {
                    if ((turret.HealthPercentage() == 100))
                    {
                        continue;
                    }
                    int health = 0;
                    switch (Flowers.菜单.Item("TIHealth").GetValue<StringList>().SelectedIndex)
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
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.LimeGreen);
                    }
                    else if (perHealth < 75 && perHealth >= 50)
                    {
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                            SharpDX.Color.YellowGreen);
                    }
                    else if (perHealth < 50 && perHealth >= 25)
                    {
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Orange);
                    }
                    else if (perHealth < 25)
                    {
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Red);
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
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.LimeGreen);
                        }
                        else if (health < 75 && health >= 50)
                        {
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.YellowGreen);
                        }
                        else if (health < 50 && health >= 25)
                        {
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.Orange);
                        }
                        else if (health < 25)
                        {
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Red);
                        }
                    }
                }
            }
        }
        private static void 小地图显示(EventArgs args)
        {

            if (Flowers.菜单.Item("HealthActive").GetValue<bool>())
            {
                foreach (Obj_AI_Turret turret in ObjectManager.Get<Obj_AI_Turret>())
                {
                    if ((turret.HealthPercentage() == 100))
                    {
                        continue;
                    }
                    int health = 0;
                    switch (Flowers.菜单.Item("TIHealth").GetValue<StringList>().SelectedIndex)
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
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.LimeGreen);
                    }
                    else if (perHealth < 75 && perHealth >= 50)
                    {
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                            SharpDX.Color.YellowGreen);
                    }
                    else if (perHealth < 50 && perHealth >= 25)
                    {
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Orange);
                    }
                    else if (perHealth < 25)
                    {
                        Huabian.DrawText1(
                            Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Red);
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
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.LimeGreen);
                        }
                        else if (health < 75 && health >= 50)
                        {
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.YellowGreen);
                        }
                        else if (health < 50 && health >= 25)
                        {
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1],
                                SharpDX.Color.Orange);
                        }
                        else if (health < 25)
                        {
                            Huabian.DrawText1(
                                Huabian.Text1, health.ToString(CultureInfo.InvariantCulture), (int)pos[0], (int)pos[1], SharpDX.Color.Red);
                        }
                    }
                }
            }
        }
    }
}
