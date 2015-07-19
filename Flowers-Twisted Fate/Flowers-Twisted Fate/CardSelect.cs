using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LeagueSharp;
using LeagueSharp.Common;

namespace Kappa // Card Select -- Credit:Esk0r
{
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
    class CardSelect
    {
        public static Cards Select;
        public static int LastWSent = 0;
        public static int LastSendWSent = 0;
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
            LastSendWSent = Environment.TickCount;
            ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, false);
        }

        public static void StartSelecting(Cards card)
        {
            if (ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name == "PickACard" && Status == SelectStatus.Ready)
            {
                Select = card;
                if (Environment.TickCount - LastWSent > 200)
                {
                    if (ObjectManager.Player.Spellbook.CastSpell(SpellSlot.W, ObjectManager.Player))
                    {
                        LastWSent = Environment.TickCount;
                    }
                }
            }
        }

        private static void Game_OnGameUpdate(EventArgs args)
        {
            var wName = ObjectManager.Player.Spellbook.GetSpell(SpellSlot.W).Name;
            var wState = ObjectManager.Player.Spellbook.CanUseSpell(SpellSlot.W);

            if ((wState == SpellState.Ready &&
                 wName == "PickACard" &&
                 (Status != SelectStatus.Selecting || Environment.TickCount - LastWSent > 500)) ||
                ObjectManager.Player.IsDead)
            {
                Status = SelectStatus.Ready;
            }
            else
                if (wState == SpellState.Cooldown &&
                    wName == "PickACard")
                {
                    Select = Cards.None;
                    Status = SelectStatus.Cooldown;
                }
                else
                    if (wState == SpellState.Surpressed &&
                        !ObjectManager.Player.IsDead)
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
