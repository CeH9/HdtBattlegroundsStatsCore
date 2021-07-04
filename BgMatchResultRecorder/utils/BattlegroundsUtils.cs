using Hearthstone_Deck_Tracker.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BgMatchResultRecorder
{
    class BattlegroundsUtils
    {
        public static bool IsShoppingPhase(ActivePlayer player)
        {
            return player == ActivePlayer.Player;
        }

        public static bool IsCombatPhase(ActivePlayer player)
        {
            return player == ActivePlayer.Opponent;
        }
    }
}
