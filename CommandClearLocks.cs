using System.Collections.Generic;
using Rocket.API;
using Rocket.Unturned.Chat;
using UnityEngine;
using Rocket.Core;
using Rocket.Unturned.Player;
using SDG.Unturned;
using System;
using Steamworks;
using System.Linq;
using static AntiLock.Main;

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
                if (args.Length > 0)
                {
                    if (!caller.HasPermission(conf.AllowOtherPermission))
                        throw new ArgumentException(R.Translate(NoPermission));

                    var name = string.Join(" ", args);
                    var other = UnturnedPlayer.FromName(name);
                    if (other is null)
                        throw new ArgumentException(inst.Translate(PlayerNotFound));

                    target = other;
                }
                msg = inst.Translate(target.CSteamID == up.CSteamID ? ClearSuccess : ClearSuccessOther, target.DisplayName, ClearLocks(target));
            }
            catch (ArgumentException ex) { color = Color.red; msg = ex.Message; }
            UnturnedChat.Say(up, msg, color);
        }

        /// <returns>Amount of cleared locks</returns>
        int ClearLocks(UnturnedPlayer up) => up != null ?
            VehicleManager.vehicles.Select(v =>
            {
                var unlock = v.lockedOwner == up.CSteamID && v.isLocked;
                if (unlock)
                    VehicleManager.ServerSetVehicleLock(v, CSteamID.Nil, CSteamID.Nil, false);
                return unlock ? 1 : 0;
            }).Sum() : 0;
    }
}