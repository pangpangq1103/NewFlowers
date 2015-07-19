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
            Notifications.AddNotification("JungleTimer", 2000);
            Notifications.AddNotification("HealthTurret", 2000);
            Notifications.AddNotification("TurretRange", 2000);
            Notifications.AddNotification("Three In One", 2000);
            Flowers.Menu();
            CustomEvents.Game.OnGameLoad += JungleTimer.Game_OnGameLoad;
            CustomEvents.Game.OnGameLoad += Tower.Game_OnGameLoad;
            CustomEvents.Game.OnGameLoad += Range.Game_OnGameLoad;
        }
    }
}
