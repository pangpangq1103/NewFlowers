using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Jungle_Timer_And_TurretDrawing
{
    class Lost
    {
        public static void Game_OnGameLoad(EventArgs args)
        {
            Notifications.AddNotification("打野计时", 2000);
            Notifications.AddNotification("防御塔血量", 2000);
            Notifications.AddNotification("防御塔范围", 2000);
            Notifications.AddNotification("三合一", 2000);
            Flowers.Menu();
            CustomEvents.Game.OnGameLoad += JungleTimer.Game_OnGameLoad;
            CustomEvents.Game.OnGameLoad += Tower.Game_OnGameLoad;
            CustomEvents.Game.OnGameLoad += Range.Game_OnGameLoad;
        }
    }
}
