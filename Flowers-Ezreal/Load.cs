using LeagueSharp.Common;

namespace Flowers_Ezreal
{
    class Load
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += lost.Game_OnGameLoad;
        }
    }
}
