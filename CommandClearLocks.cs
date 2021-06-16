using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using static AntiLock.Main;
using System;
using Steamworks;

namespace AntiLock
{
    public class CommandClearLocks : IRocketCommand
    {
        #region Properties

        public AllowedCaller AllowedCaller => AllowedCaller.Player;

        public string Name => "clearlocks";

        public string Help => "Clears all of the caller's locks, or another player's lock if an argument is passed.";

        public string Syntax => "/clearlocks <player [optional]>";

        public List<string> Aliases => new List<string> { "unlockall" };

        public List<string> Permissions => new List<string> { "antilock.clearlocks" };

        #endregion

        public void Execute(IRocketPlayer caller, string[] args)
        {
            var up = (UnturnedPlayer)caller;
            var target = up;
            string msg;
            var color = Color.green;
            try
            {
                if (args.Length == 1)
                {
                    if (!caller.HasPermission(conf.AllowOtherPermission))
                        throw new ArgumentException(R.Translate(NoPermission));

                    var other = UnturnedPlayer.FromName(args[0]);
                    if (other == null)
                        throw new ArgumentException(inst.Translate(PlayerNotFound));

                    target = other;
                }
                msg = inst.Translate(target.CSteamID == up.CSteamID ? ClearSuccess : ClearSuccessOther, target.DisplayName, ClearLocks(target));
            }
            catch (ArgumentException ex) { color = Color.red; msg = ex.Message; }
            UnturnedChat.Say(up, msg, color);
        }

        /// <returns>Amount of cleared locks</returns>
        int ClearLocks(UnturnedPlayer up)
        {
            int count = 0;
            if (up != null)
            {
                foreach (var v in VehicleManager.vehicles)
                {
                    if (v.lockedOwner == up.CSteamID && v.isLocked)
                    {
                        VehicleManager.ServerSetVehicleLock(v, CSteamID.Nil, CSteamID.Nil, false);
                        count++;
                    }
                }
            }
            return count;
        }
    }
}