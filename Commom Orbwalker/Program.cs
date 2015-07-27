#region 引用
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = System.Drawing.Color;
using Collision = LeagueSharp.Common.Collision;
#endregion

namespace 花边库引用走砍
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

            Menu = new Menu("Flowers Commom Orbwalker", "花边-引用库走砍", true);

            Orbwalker = new Orbwalking.Orbwalker(Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker")));

            Menu.AddItem(new MenuItem("作者", "作者:花边下丶情未央"));
            Menu.AddItem(new MenuItem("版本", "版本 : 6.6.6.6"));
            Menu.AddItem(new MenuItem("Best 最强系列 为你分享", "Best 最强系列 为你分享"));
            Menu.AddItem(new MenuItem("对外QQ群:299606556", "对外QQ群:299606556"));

            Menu.AddToMainMenu();
        }
    }
}
