using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace FlowersPrediction
{
    class Program
    {
        private static Obj_AI_Hero Player = ObjectManager.Player;
        private static Menu Menu;
        internal static Orbwalking.Orbwalker Orbwalker;
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {

            Menu = new Menu("Flowers Prediction", "Prediction", true);

            var PredictionMenu = new Menu("Perdiction", "perdiction");
            Prediction.AddToMenu(PredictionMenu);
            Menu.AddSubMenu(PredictionMenu);

            Menu.AddItem(new MenuItem("PTE", "Please to Enjoy"));
            Menu.AddItem(new MenuItem("PTE1", "This is Common Prediction"));

            Menu.AddToMainMenu();
        }
    }
}
