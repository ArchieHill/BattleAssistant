using Battle_Assistant.Common;
using Battle_Assistant.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace Battle_Assistant.Helpers
{
    public static class FileHelper
    {
        public static void CopyToIncomingEmail(BattleModel battle)
        {
            File.Copy(battle.BattleFile.Path, battle.Game.IncomingEmailFolder.Path + "\\" + battle.BattleFile.Name, true);
            battle.Status = Status.YOUR_TURN;
            battle.LastAction = Actions.COPY_TO_INCOMING_EMAIL;
        }

        public static void CopyToSharedDrive(BattleModel battle)
        {
            File.Copy(battle.BattleFile.Path, battle.Opponent.SharedDir.Path + "\\" + battle.BattleFile.Name, true);
            battle.Status = Status.WAITING;
            battle.LastAction = Actions.COPY_TO_SHAREDDRIVE;
        }
    }
}
