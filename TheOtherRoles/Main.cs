﻿global using UnhollowerBaseLib;
global using UnhollowerBaseLib.Attributes;
global using UnhollowerRuntimeLib;

using BepInEx;
using BepInEx.Configuration;
using BepInEx.IL2CPP;
using HarmonyLib;
using Hazel;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using TheOtherRoles.Modules;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using TheOtherRoles.Objects;

namespace TheOtherRoles
{
    [BepInPlugin(Id, "The Other Roles MR H", VersionString)]
    [BepInDependency(SubmergedCompatibility.SUBMERGED_GUID, BepInDependency.DependencyFlags.SoftDependency)]
    [BepInProcess("Among Us.exe")]
    public class TheOtherRolesPlugin : BasePlugin
    {
        public const string Id = "me.eisbison.theotherroles";

        public const string VersionString = "0.1";

        public static System.Version Version = System.Version.Parse(VersionString);
        internal static BepInEx.Logging.ManualLogSource Logger;

        public Harmony Harmony { get; } = new Harmony(Id);
        public static TheOtherRolesPlugin Instance;

        public static int optionsPage = 2;
        public static int optionsPageMax = optionsPage + 1;

        public static ConfigEntry<bool> DebugMode { get; private set; }
        public static ConfigEntry<bool> ViewSeacretMode { get; private set; }
        public static ConfigEntry<bool> GhostsSeeTasks { get; set; }
        public static ConfigEntry<bool> GhostsSeeRoles { get; set; }
        public static ConfigEntry<bool> GhostsSeeModifier { get; set; }
        public static ConfigEntry<bool> GhostsSeeVotes{ get; set; }
        public static ConfigEntry<bool> ShowRoleSummary { get; set; }
        public static ConfigEntry<bool> ShowLighterDarker { get; set; }
        public static ConfigEntry<bool> EnableSoundEffects { get; set; }
        public static ConfigEntry<bool> EnableHorseMode { get; set; }
        public static ConfigEntry<string> Ip { get; set; }
        public static ConfigEntry<ushort> Port { get; set; }
        public static ConfigEntry<string> ShowPopUpVersion { get; set; }

        public static Sprite ModStamp;
        public static Sprite CustomPreset;

        public static IRegionInfo[] defaultRegions;

        // This is part of the Mini.RegionInstaller, Licensed under GPLv3
        // file="RegionInstallPlugin.cs" company="miniduikboot">
        public static void UpdateRegions() {
            ServerManager serverManager = FastDestroyableSingleton<ServerManager>.Instance;
            var regions = new IRegionInfo[] {
                new DnsRegionInfo("mods.hopto.org", "Modded NA (MNA)", StringNames.NoTranslation, "mods.hopto.org", 443, false).CastFast<IRegionInfo>(),
                new DnsRegionInfo("au-eu.duikbo.at", "Modded EU (MEU)", StringNames.NoTranslation, "au-eu.duikbo.at", 22023, false).CastFast<IRegionInfo>(),
                new DnsRegionInfo(Ip.Value, "Custom", StringNames.NoTranslation, Ip.Value, Port.Value, false).CastFast<IRegionInfo>()
            };
            
            IRegionInfo ? currentRegion = serverManager.CurrentRegion;
            Logger.LogInfo($"Adding {regions.Length} regions");
            foreach (IRegionInfo region in regions) {
                if (region == null) 
                    Logger.LogError("Could not add region");
                else {
                    if (currentRegion != null && region.Name.Equals(currentRegion.Name, StringComparison.OrdinalIgnoreCase)) 
                        currentRegion = region;               
                    serverManager.AddOrUpdateRegion(region);
                }
            }

            // AU remembers the previous region that was set, so we need to restore it
            if (currentRegion != null) {
                Logger.LogDebug("Resetting previous region");
                serverManager.SetRegion(currentRegion);
            }
        }

        public override void Load() {
            Logger = Log;
            Instance = this;

            DebugMode = Config.Bind("Custom", "Enable Debug Mode", false);
            ViewSeacretMode = Config.Bind("Custom", "View Seacret Mode", false);
            GhostsSeeTasks = Config.Bind("Custom", "Ghosts See Remaining Tasks", true);
            GhostsSeeRoles = Config.Bind("Custom", "Ghosts See Roles", true);
            GhostsSeeModifier = Config.Bind("Custom", "Ghosts See Modifier", true);
            GhostsSeeVotes = Config.Bind("Custom", "Ghosts See Votes", true);
            ShowRoleSummary = Config.Bind("Custom", "Show Role Summary", true);
            ShowLighterDarker = Config.Bind("Custom", "Show Lighter / Darker", false);
            EnableSoundEffects = Config.Bind("Custom", "Enable Sound Effects", true);
            EnableHorseMode = Config.Bind("Custom", "Enable Horse Mode", false);
            ShowPopUpVersion = Config.Bind("Custom", "Show PopUp", "0");

            Ip = Config.Bind("Custom", "Custom Server IP", "127.0.0.1");
            Port = Config.Bind("Custom", "Custom Server Port", (ushort)22023);
            defaultRegions = ServerManager.DefaultRegions;

            UpdateRegions();

            GameOptionsData.RecommendedImpostors = GameOptionsData.MaxImpostors = Enumerable.Repeat(3, 16).ToArray(); // Max Imp = Recommended Imp = 3
            GameOptionsData.MinPlayers = Enumerable.Repeat(4, 15).ToArray(); // Min Players = 4

            DebugMode = Config.Bind("Custom", "Enable Debug Mode", false);
            Harmony.PatchAll();

            CustomOptionHolder.Load();
            CustomColors.Load();
            Patches.FreeNamePatch.Initialize();

            if (BepInExUpdater.UpdateRequired)
            {
                AddComponent<BepInExUpdater>();
                return;
            }
            
            SubmergedCompatibility.Initialize();
            AddComponent<ModUpdateBehaviour>();
        }
        public static Sprite GetModStamp() {
            if (ModStamp) return ModStamp;
            return ModStamp = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.ModStamp.png", 150f);
        }
        public static Sprite GetCustomPreset() {
            if (CustomPreset) return CustomPreset;
            return CustomPreset = Helpers.loadSpriteFromResources("TheOtherRoles.Resources.CustomPreset.png", 150f);
        }
    }

    // Deactivate bans, since I always leave my local testing game and ban myself
    [HarmonyPatch(typeof(StatsManager), nameof(StatsManager.AmBanned), MethodType.Getter)]
    public static class AmBannedPatch
    {
        public static void Postfix(out bool __result)
        {
            __result = false;
        }
    }
    [HarmonyPatch(typeof(ChatController), nameof(ChatController.Awake))]
    public static class ChatControllerAwakePatch {
        private static void Prefix() {
            if (!EOSManager.Instance.isKWSMinor) {
                SaveManager.chatModeType = 1;
                SaveManager.isGuest = false;
            }
        }
    }
    
    // Debugging tools
    [HarmonyPatch(typeof(KeyboardJoystick), nameof(KeyboardJoystick.Update))]
    public static class DebugManager
    {
        private static readonly System.Random random = new System.Random((int)DateTime.Now.Ticks);
        public static List<PlayerControl> bots = new List<PlayerControl>();

        public static void Postfix(KeyboardJoystick __instance)
        {
            if (!TheOtherRolesPlugin.DebugMode.Value) return;

            // Spawn dummys
            if (Input.GetKeyDown(KeyCode.F)) {
                var playerControl = UnityEngine.Object.Instantiate(AmongUsClient.Instance.PlayerPrefab);
                var i = playerControl.PlayerId = (byte) GameData.Instance.GetAvailableId();

                bots.Add(playerControl);
                GameData.Instance.AddPlayer(playerControl);
                AmongUsClient.Instance.Spawn(playerControl, -2, InnerNet.SpawnFlags.None);
                
                playerControl.transform.position = CachedPlayer.LocalPlayer.transform.position;
#if true
                playerControl.GetComponent<DummyBehaviour>().enabled = true;
                playerControl.NetTransform.enabled = false;
#else
                playerControl.GetComponent<DummyBehaviour>().enabled = false;
                playerControl.isDummy = false;
                playerControl.notRealPlayer = true;
                playerControl.NetTransform.enabled = true;
#endif
                playerControl.SetName(RandomString(10));
                playerControl.SetColor((byte) random.Next(Palette.PlayerColors.Length));
                GameData.Instance.RpcSetTasks(playerControl.PlayerId, new byte[0]);
            }

            // Terminate round
            if(Input.GetKeyDown(KeyCode.L)) {
                MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.ForceEnd, Hazel.SendOption.Reliable, -1);
                AmongUsClient.Instance.FinishRpcImmediately(writer);
                RPCProcedure.forceEnd();
            }

#if false
            if (Input.GetKey(KeyCode.LeftControl))
			{
                if (FastDestroyableSingleton<HudManager>.Instance != null)
				{
                    if (Input.GetKeyDown(KeyCode.Alpha9))
                    {
                        if (debugText != null)
                        {
                            if (debugText.gameObject != null)
                                GameObject.DestroyImmediate(debugText.gameObject);
                            debugText = null;
                        }
                    }

                    if (Input.GetKey(KeyCode.Alpha0))
                    {
                        if (debugText == null || debugText.gameObject == null)
                        {
                            RoomTracker roomTracker = FastDestroyableSingleton<HudManager>.Instance.roomTracker;
                            GameObject gameObject = UnityEngine.Object.Instantiate(roomTracker.gameObject);
                            UnityEngine.Object.DestroyImmediate(gameObject.GetComponent<RoomTracker>());
                            gameObject.transform.SetParent(FastDestroyableSingleton<HudManager>.Instance.transform);
                            gameObject.transform.localPosition = new Vector3(0, 0, -930f);
                            gameObject.transform.localScale = Vector3.one * 1f;
                            debugText = gameObject.GetComponent<TMPro.TMP_Text>();
                            debugText.rectTransform.sizeDelta = new Vector2(Screen.width, Screen.height);
                        }

                        var builder = new System.Text.StringBuilder();
                        {
                        }

                        debugText.text = builder.ToString();
                        debugText.gameObject.SetActive(true);
                    }
                    else if (debugText != null)
                    {
                        debugText.gameObject.SetActive(false);
                    }
                }
 

                if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    for (int i = 0; i < bots.Count; ++i)
                    {
                        int index = random.Next(bots.Count);
                        while (bots[index].Data.IsDead)
                            index = random.Next(bots.Count);
                        MeetingHud.Instance.CmdCastVote(bots[i].PlayerId, bots[index].PlayerId);
                    }
                }

                if (Input.GetKeyDown(KeyCode.Alpha2))
                {
                    MessageWriter killWriter = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.UncheckedMurderPlayer, Hazel.SendOption.Reliable, -1);
                    killWriter.Write(CachedPlayer.LocalPlayer.PlayerId);
                    killWriter.Write(CachedPlayer.LocalPlayer.PlayerId);
                    killWriter.Write(byte.MaxValue);
                    AmongUsClient.Instance.FinishRpcImmediately(killWriter);
                    RPCProcedure.uncheckedMurderPlayer(CachedPlayer.LocalPlayer.PlayerId, CachedPlayer.LocalPlayer.PlayerId, Byte.MaxValue);
                }
            }
 
#endif
        }

#if false
        static TMPro.TMP_Text debugText;
#endif

        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
                .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
