using LeagueSharp;
using LeagueSharp.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowersTwistedFate
{
    //  This  is Esk0r CardSelect ~  GitHub:Github.com/Esk0r/LeagueSharp/
    public enum Cards
    {
        Red,
        Yellow,
        Blue,
        None,
    }

    public enum SelectStatus
    {
        Ready,
        Selecting,
        Selected,
        Cooldown,
    }

    public static class CardSelect
    {
        public static Cards Select;
        public static int LastWSent = 0;
        public static int LastSendWSent = 0;
        public static Obj_AI_Hero Player = ObjectManager.Player;
        public static SelectStatus Status
        {
            get;
            set;
        }
        static CardSelect()
        {
            Obj_AI_Base.OnProcessSpellCast += Obj_AI_Hero_OnProcessSpellCast;
            Game.OnUpdate += Game_OnGameUpdate;
        }

        private static void SendWPacket()
        {
            ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
        }

        public static void StartSelecting(Cards card)
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && Status == SelectStatus.Ready)
            {
                Select = card;
                if (Utils.TickCount - LastWSent > 170 + Game.Ping / 2)
                {
                    ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, ObjectManager.Player);
                    LastWSent = Utils.TickCount;
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            var wName = Player.Spellbook.GetSpell(SpellSlot.W).Name;
            var wState = Player.Spellbook.CanUseSpell(SpellSlot.W);

            if ((wState == SpellState.Ready && wName == "PickACard" && (Status != SelectStatus.Selecting || Utils.TickCount - LastWSent > 500)) || Player.IsDead)
            {
                Status = SelectStatus.Ready;
            }
            else
                if (wState == SpellState.Cooldown && wName == "PickACard")
                {
                    Select = Cards.None;
                    Status = SelectStatus.Cooldown;
                }
                else
                    if (wState == SpellState.Surpressed && !Player.IsDead)
                    {
                        Status = SelectStatus.Selected;
                    }

            if (Select == Cards.Blue && wName == "bluecardlock")
            {
                SendWPacket();
            }
            else
                if (Select == Cards.Yellow && wName == "goldcardlock")
                {
                    SendWPacket();
                }
                else
                    if (Select == Cards.Red && wName == "redcardlock")
                    {
                        SendWPacket();
                    }
        }

        private static void Obj_AI_Hero_OnProcessSpellCast(Obj_AI_Base sender, GameObjectProcessSpellCastEventArgs args)
        {
            if (!sender.IsMe)
            {
                return;
            }

            if (args.SData.Name == "PickACard")
            {
                Status = SelectStatus.Selecting;
            }

            if (args.SData.Name == "goldcardlock" || args.SData.Name == "bluecardlock" ||
                args.SData.Name == "redcardlock")
            {
                Status = SelectStatus.Selected;
            }
        }
    }
}
