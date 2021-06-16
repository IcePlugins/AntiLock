using Rocket.Core.Logging;
using Rocket.Core.Plugins;
using Rocket.API.Collections;
using HarmonyLib;

namespace AntiLock
{
    public class Main : RocketPlugin<Config>
    {
        public static Main Instance;
        Harmony hInstance;
        internal static Main inst => Instance;
        internal static Config conf => Instance.Configuration.Instance;

        protected override void Load()
        {
            Instance = this;

            hInstance = new Harmony("antilock.harmony");

            Logger.Log($"Loaded AntiLock! Default locks allocated: {conf.DefaultMaxLocks}");
            Logger.Log("Groups:");
            foreach (var group in conf.LockGroups)
                Logger.Log($" {group.Permission} | {group.MaxLocks} locks");

            hInstance.PatchAll();
        }

        protected override void Unload()
        {
            hInstance.UnpatchAll();
        }

        public const string
            NoPermission = "command_no_permission",
            ClearSuccessOther = nameof(ClearSuccessOther),
            ClearSuccess = nameof(ClearSuccess),
            PlayerNotFound = nameof(PlayerNotFound),
            MaxLockedNotice = nameof(MaxLockedNotice),
            LockedNotice = nameof(LockedNotice);
        public override TranslationList DefaultTranslations => new TranslationList
        {
            { ClearSuccessOther, "You've unlocked all of {0}'s vehicles. ({1} in total.)" },
            { ClearSuccess, "You've unlocked all of your vehicles. ({0} in total.)" },
            { PlayerNotFound, "The specified player was not found." },
            { MaxLockedNotice, "You have reached your allocated number of vehicle locks. ({0})" },
            { LockedNotice, "You have locked a vehicle. ({0}/{1} locks remaining.)" }
        };
    }
}
