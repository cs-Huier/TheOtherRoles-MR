using HarmonyLib;
using Hazel;
using static TheOtherRoles.TheOtherRoles;
using static TheOtherRoles.HudManagerStartPatch;
using static TheOtherRoles.GameHistory;
using static TheOtherRoles.MapOptions;
using TheOtherRoles.Objects;
using TheOtherRoles.Patches;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System;
using TheOtherRoles.Players;
using TheOtherRoles.Utilities;
using InnerNet;
using TheOtherRoles.CustomGameModes;
using AmongUs.Data;

namespace TheOtherRoles
{
    enum RoleId {
        Jester,
        Mayor,
        Portalmaker,
        Engineer,
        Sheriff,
        Deputy,
        Lighter,
        Godfather,
        Mafioso,
        Janitor,
        Detective,
        TimeMaster,
        Medic,
        Swapper,
        Seer,
        Morphling,
        Camouflager,
        EvilHacker,
        Hacker,
        Tracker,
        Vampire,
        Snitch,
        Jackal,
        Sidekick,
        Eraser,
        Spy,
        Trickster,
        Cleaner,
        Warlock,
        SecurityGuard,
        Arsonist,
        EvilGuesser,
        NiceGuesser,
        BountyHunter,
        Vulture,
        Medium,
        Trapper,
        Madmate,
        Lawyer,
        Prosecutor,
        Pursuer,
        Witch,
        Ninja,
        Thief,
        EvilYasuna,
        Yasuna,
        TaskMaster,
        DoorHacker,
        Kataomoi,
        KillerCreator,
        MadmateKiller,
        Crewmate,
        Impostor,
        // Modifier ---
        Lover,
        Bait,
        Bloody,
        AntiTeleport,
        Tiebreaker,
        Sunglasses,
        Mini,
        Vip,
        Invert,
        Chameleon,
        Shifter,
        // Task Vs Mode ---
        TaskRacer,
    }

    enum CustomRPC
    {
        // Main Controls

        ResetVaribles = 60,
        ShareOptions,
        ForceEnd,
        WorkaroundSetRoles,
        SetRole,
        SetModifier,
        VersionHandshake,
        UseUncheckedVent,
        UncheckedMurderPlayer,
        UncheckedCmdReportDeadBody,
        ConsumeAdminTime,
        UncheckedExilePlayer,
        DynamicMapOption,
        SetGameStarting,
        ShareGamemode,
        ConsumeVitalTime,
        ConsumeSecurityCameraTime,
        UncheckedEndGame,
        UncheckedEndGame_Response,
        UncheckedSetVanilaRole,
        TaskVsMode_Ready, // Task Vs Mode
        TaskVsMode_Start, // Task Vs Mode
        TaskVsMode_AllTaskCompleted, // Task Vs Mode
        TaskVsMode_MakeItTheSameTaskAsTheHost, // Task Vs Mode
        TaskVsMode_MakeItTheSameTaskAsTheHostDetail, // Task Vs Mode

        // Role functionality

        EngineerFixLights = 101,
        EngineerFixSubmergedOxygen,
        EngineerUsedRepair,
        CleanBody,
        MedicSetShielded,
        ShieldedMurderAttempt,
        TimeMasterShield,
        TimeMasterRewindTime,
        ShifterShift,
        SwapperSwap,
        MorphlingMorph,
        CamouflagerCamouflage,
        TrackerUsedTracker,
        VampireSetBitten,
        PlaceGarlic,
        EvilHackerCreatesMadmate,
        DeputyUsedHandcuffs,
        DeputyPromotes,
        JackalCreatesSidekick,
        SidekickPromotes,
        ErasePlayerRoles,
        SetFutureErased,
        SetFutureShifted,
        SetFutureShielded,
        SetFutureSpelled,
        PlaceNinjaTrace,
        PlacePortal,
        UsePortal,
        PlaceJackInTheBox,
        LightsOut,
        PlaceCamera,
        SealVent,
        ArsonistWin,
        GuesserShoot,
        VultureWin,
        LawyerSetTarget,
        LawyerPromotesToPursuer,
        SetBlanked,
        Bloody,
        SetFirstKill,
        Invert,
        SetTiebreak,
        SetInvisible,
        ThiefStealsRole,
        SetTrap,
        TriggerTrap,

        YasunaSpecialVote,
        YasunaSpecialVote_DoCastVote,
        TaskMasterSetExTasks,
        TaskMasterUpdateExTasks,
        DoorHackerDone,
        KataomoiSetTarget,
        KataomoiWin,
        KataomoiStalking,
        Synchronize,
        KillerCreatorCreatesMadmateKiller,
        MadmateKillerPromotes,

        // Gamemode
        SetGuesserGm,
        HuntedShield,
        HuntedRewindTime,
        ShareTimer,
    }

    public static class RPCProcedure
    {
        public static byte uncheckedEndGameReason = (byte)CustomGameOverReason.Unused;
        static HashSet<byte> uncheckedEndGameResponsePlayerId = new HashSet<byte>();

        // Main Controls

        public static void resetVariables()
        {
            Garlic.clearGarlics();
            JackInTheBox.clearJackInTheBoxes();
            NinjaTrace.clearTraces();
            Portal.clearPortals();
            Bloodytrail.resetSprites();
            Trap.clearTraps();
            clearAndReloadMapOptions();
            clearAndReloadRoles();
            clearGameHistory();
            setCustomButtonCooldowns();
            Helpers.toggleZoom(reset: true);
            MapBehaviourPatch2.ResetIcons();
            SpawnInMinigamePatch.reset();
            ElectricPatch.reset();
            MadmateTaskHelper.Reset();
            GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 0;
        }

        public static void HandleShareOptions(byte numberOfOptions, MessageReader reader)
        {
            try
            {
                for (int i = 0; i < numberOfOptions; i++)
                {
                    uint optionId = reader.ReadPackedUInt32();
                    uint selection = reader.ReadPackedUInt32();
                    CustomOption option = CustomOption.options.First(option => option.id == (int)optionId);
                    option.updateSelection((int)selection);
                }
            }
            catch (Exception e)
            {
                TheOtherRolesPlugin.Logger.LogError("Error while deserializing options: " + e.Message);
            }
        }

        public static void forceEnd()
        {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
            {
                if (!player.Data.Role.IsImpostor)
                {
                    player.RemoveInfected();
                    player.MurderPlayer(player);
                    player.Data.IsDead = true;
                }
            }
        }

        public static void shareGamemode(byte gm)
        {
            MapOptions.gameMode = (CustomGamemodes)gm;
        }

        public static void workaroundSetRoles(byte numberOfRoles, MessageReader reader)
        {
            for (int i = 0; i < numberOfRoles; i++)
            {
                byte playerId = (byte)reader.ReadPackedUInt32();
                byte roleId = (byte)reader.ReadPackedUInt32();
                try
                {
                    setRole(roleId, playerId);
                }
                catch (Exception e)
                {
                    TheOtherRolesPlugin.Logger.LogError("Error while deserializing roles: " + e.Message);
                }
            }
        }

        public static void setRole(byte roleId, byte playerId)
        {
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == playerId) {
                    switch ((RoleId)roleId) {
                        case RoleId.Jester:
                            Jester.jester = player;
                            break;
                        case RoleId.Mayor:
                            Mayor.mayor = player;
                            break;
                        case RoleId.Portalmaker:
                            Portalmaker.portalmaker = player;
                            break;
                        case RoleId.Engineer:
                            Engineer.engineer = player;
                            break;
                        case RoleId.Sheriff:
                            Sheriff.sheriff = player;
                            break;
                        case RoleId.Deputy:
                            Deputy.deputy = player;
                            break;
                        case RoleId.Lighter:
                            Lighter.lighter = player;
                            break;
                        case RoleId.Godfather:
                            Godfather.godfather = player;
                            break;
                        case RoleId.Mafioso:
                            Mafioso.mafioso = player;
                            break;
                        case RoleId.Janitor:
                            Janitor.janitor = player;
                            break;
                        case RoleId.Detective:
                            Detective.detective = player;
                            break;
                        case RoleId.TimeMaster:
                            TimeMaster.timeMaster = player;
                            break;
                        case RoleId.Medic:
                            Medic.medic = player;
                            break;
                        case RoleId.Shifter:
                            Shifter.shifter = player;
                            break;
                        case RoleId.Swapper:
                            Swapper.swapper = player;
                            break;
                        case RoleId.Seer:
                            Seer.seer = player;
                            break;
                        case RoleId.Morphling:
                            Morphling.morphling = player;
                            break;
                        case RoleId.Camouflager:
                            Camouflager.camouflager = player;
                            break;
                        case RoleId.EvilHacker:
                            EvilHacker.evilHacker = player;
                            break;
                        case RoleId.Hacker:
                            Hacker.hacker = player;
                            break;
                        case RoleId.Tracker:
                            Tracker.tracker = player;
                            break;
                        case RoleId.Vampire:
                            Vampire.vampire = player;
                            break;
                        case RoleId.Snitch:
                            Snitch.snitch = player;
                            break;
                        case RoleId.Jackal:
                            Jackal.jackal = player;
                            break;
                        case RoleId.Sidekick:
                            Sidekick.sidekick = player;
                            break;
                        case RoleId.Eraser:
                            Eraser.eraser = player;
                            break;
                        case RoleId.Spy:
                            Spy.spy = player;
                            break;
                        case RoleId.Trickster:
                            Trickster.trickster = player;
                            break;
                        case RoleId.Cleaner:
                            Cleaner.cleaner = player;
                            break;
                        case RoleId.Warlock:
                            Warlock.warlock = player;
                            break;
                        case RoleId.SecurityGuard:
                            SecurityGuard.securityGuard = player;
                            break;
                        case RoleId.Arsonist:
                            Arsonist.arsonist = player;
                            break;
                        case RoleId.EvilGuesser:
                            Guesser.evilGuesser = player;
                            break;
                        case RoleId.NiceGuesser:
                            Guesser.niceGuesser = player;
                            break;
                        case RoleId.BountyHunter:
                            BountyHunter.bountyHunter = player;
                            break;
                        case RoleId.Vulture:
                            Vulture.vulture = player;
                            break;
                        case RoleId.Medium:
                            Medium.medium = player;
                            break;
                        case RoleId.Trapper:
                            Trapper.trapper = player;
                            break;
                        case RoleId.Madmate:
                            Madmate.madmate = player;
                            break;
                        case RoleId.Lawyer:
                            Lawyer.lawyer = player;
                            break;
                        case RoleId.Prosecutor:
                            Lawyer.lawyer = player;
                            Lawyer.isProsecutor = true;
                            break;
                        case RoleId.Pursuer:
                            Pursuer.pursuer = player;
                            break;
                        case RoleId.Witch:
                            Witch.witch = player;
                            break;
                        case RoleId.Ninja:
                            Ninja.ninja = player;
                            break;
                        case RoleId.Thief:
                            Thief.thief = player;
                            break;
                        case RoleId.Yasuna:
                        case RoleId.EvilYasuna:
                            Yasuna.yasuna = player;
                            break;
                        case RoleId.TaskMaster:
                            TaskMaster.taskMaster = player;
                            break;
                        case RoleId.DoorHacker:
                            DoorHacker.doorHacker = player;
                            break;
                        case RoleId.Kataomoi:
                            Kataomoi.kataomoi = player;
                            break;
                        case RoleId.KillerCreator:
                            KillerCreator.killerCreator = player;
                            break;
                        case RoleId.MadmateKiller:
                            MadmateKiller.madmateKiller = player;
                            break;

                        // Task Vs Mode
                        case RoleId.TaskRacer:
                            TaskRacer.addTaskRacer(player);
                            break;
                    }
                }
        }

        public static void setModifier(byte modifierId, byte playerId, byte flag) {
            PlayerControl player = Helpers.playerById(playerId); 
            switch ((RoleId)modifierId) {
                case RoleId.Bait:
                    Bait.bait.Add(player);
                    break;
                case RoleId.Lover:
                    if (flag == 0) Lovers.lover1 = player;
                    else Lovers.lover2 = player;
                    break;
                case RoleId.Bloody:
                    Bloody.bloody.Add(player);
                    break;
                case RoleId.AntiTeleport:
                    AntiTeleport.antiTeleport.Add(player);
                    break;
                case RoleId.Tiebreaker:
                    Tiebreaker.tiebreaker = player;
                    break;
                case RoleId.Sunglasses:
                    Sunglasses.sunglasses.Add(player);
                    break;
                case RoleId.Mini:
                    Mini.mini = player;
                    break;
                case RoleId.Vip:
                    Vip.vip.Add(player);
                    break;
                case RoleId.Invert:
                    Invert.invert.Add(player);
                    break;
                case RoleId.Chameleon:
                    Chameleon.chameleon.Add(player);
                    break;
                case RoleId.Shifter:
                    Shifter.shifter = player;
                    break;
            }
        }

        public static void versionHandshake(int major, int minor, int build, int revision, Guid guid, int clientId) {
            System.Version ver;
            if (revision < 0) 
                ver = new System.Version(major, minor, build);
            else 
                ver = new System.Version(major, minor, build, revision);
            GameStartManagerPatch.playerVersions[clientId] = new GameStartManagerPatch.PlayerVersion(ver, guid);
        }

        public static void useUncheckedVent(int ventId, byte playerId, byte isEnter) {
            PlayerControl player = Helpers.playerById(playerId);
            if (player == null) return;
            // Fill dummy MessageReader and call MyPhysics.HandleRpc as the corountines cannot be accessed
            MessageReader reader = new MessageReader();
            byte[] bytes = BitConverter.GetBytes(ventId);
            if (!BitConverter.IsLittleEndian)
                Array.Reverse(bytes);
            reader.Buffer = bytes;
            reader.Length = bytes.Length;

            JackInTheBox.startAnimation(ventId);
            player.MyPhysics.HandleRpc(isEnter != 0 ? (byte)19 : (byte)20, reader);
        }

        public static void uncheckedMurderPlayer(byte sourceId, byte targetId, byte showAnimation) {
            if (AmongUsClient.Instance.GameState != InnerNet.InnerNetClient.GameStates.Started) return;
            PlayerControl source = Helpers.playerById(sourceId);
            PlayerControl target = Helpers.playerById(targetId);
            if (source != null && target != null) {
                if (showAnimation == 0) KillAnimationCoPerformKillPatch.hideNextAnimation = true;
                source.MurderPlayer(target);
                // Task Vs Mode
                if (TaskRacer.isValid()) {
                    TaskRacer.updateControl();
                }
            }
        }

        public static void uncheckedCmdReportDeadBody(byte sourceId, byte targetId) {
            PlayerControl source = Helpers.playerById(sourceId);
            var t = targetId == Byte.MaxValue ? null : Helpers.playerById(targetId).Data;
            if (source != null) source.ReportDeadBody(t);
        }

        public static void consumeAdminTime(float delta) {
            MapOptions.adminTimer -= delta;
        }

        public static void consumeVitalTime(float delta) {
            vitalsTimer -= delta;
        }

        public static void consumeSecurityCameraTime(float delta) {
            securityCameraTimer -= delta;
        }

        // Task Vs Mode
        public static void taskVsModeReady(byte playerId) {
            if (!TaskRacer.isValid()) return;
            var taskRacer = TaskRacer.getTaskRacer(playerId);
            if (taskRacer == null) return;
            TaskRacer.onReady(taskRacer);
        }

        // Task Vs Mode
        public static void taskVsModeStart() {
            if (!TaskRacer.isValid()) return;
            TaskRacer.startGame();
        }

        // Task Vs Mode
        public static void taskVsModeAllTaskCompleted(byte playerId, ulong timeMilliSec) {
            if (!TaskRacer.isValid()) return;
            TaskRacer.setTaskCompleteTimeSec(playerId, timeMilliSec);
        }

        // Task Vs Mode
        public static void taskVsModeMakeItTheSameTaskAsTheHost(byte[] taskTypeIds) {
            if (!TaskRacer.isValid()) return;
            TaskRacer.setHostTasks(taskTypeIds);
        }

        // Task Vs Mode
        public static void taskVsModeMakeItTheSameTaskAsTheHostDetail(uint taskId, byte[] data) {
            if (!TaskRacer.isValid()) return;
            TaskRacer.setHostTaskDetail(taskId, data);
        }

        public static void uncheckedExilePlayer(byte targetId) {
            PlayerControl target = Helpers.playerById(targetId);
            if (target != null) target.Exiled();
        }

        public static void dynamicMapOption(byte mapId) {
            PlayerControl.GameOptions.MapId = mapId;
        }

        public static void setGameStarting()
        {
            GameStartManagerPatch.GameStartManagerUpdatePatch.startingTimer = 5f;
        }

        public static void uncheckedEndGame(byte reason) {
            uncheckedEndGameReason = reason;
            AmongUsClient.Instance.GameState = InnerNetClient.GameStates.Ended;
            Il2CppSystem.Collections.Generic.List<ClientData> allClients = AmongUsClient.Instance.allClients;
            lock (allClients) {
                AmongUsClient.Instance.allClients.Clear();
            }
            var dispatcher = AmongUsClient.Instance.Dispatcher;
            lock (dispatcher) {
                AmongUsClient.Instance.Dispatcher.Add(new Action(() => {
                    MapUtilities.CachedShipStatus.enabled = false;
                    AmongUsClient.Instance.OnGameEnd(new EndGameResult((GameOverReason)reason, false));
                }));
            }
        }

        public static void uncheckedEndGameResponse(byte playerId) {
            if (!uncheckedEndGameResponsePlayerId.Contains(playerId))
                uncheckedEndGameResponsePlayerId.Add(playerId);

            if (AmongUsClient.Instance.AmHost) {
                bool is_send = true;
                foreach (var p in CachedPlayer.AllPlayers) {
                    if (!p.PlayerControl.isDummy && !p.PlayerControl.notRealPlayer && !p.Data.Disconnected && !uncheckedEndGameResponsePlayerId.Contains(p.PlayerId)) {
                        is_send = false;
                        break;
                    }
                }
                if (is_send) {
                    ShipStatus.RpcEndGame((GameOverReason)uncheckedEndGameReason, false);
                    uncheckedEndGameReason = (byte)CustomGameOverReason.Unused;
                    uncheckedEndGameResponsePlayerId.Clear();
                }
            }
        }

        public static void uncheckedSetVanilaRole(byte playerId, RoleTypes type) {
            var player = Helpers.playerById(playerId);
            if (player == null) return;
            DestroyableSingleton<RoleManager>.Instance.SetRole(player, type);
            player.Data.Role.Role = type;
        }

        // Role functionality

        public static void engineerFixLights() {
            SwitchSystem switchSystem = MapUtilities.Systems[SystemTypes.Electrical].CastFast<SwitchSystem>();
            switchSystem.ActualSwitches = switchSystem.ExpectedSwitches;
        }

        public static void engineerFixSubmergedOxygen() {
            SubmergedCompatibility.RepairOxygen();
        }

        public static void engineerUsedRepair() {
            Engineer.remainingFixes--;
        }

        public static void cleanBody(byte playerId) {
            DeadBody[] array = UnityEngine.Object.FindObjectsOfType<DeadBody>();
            for (int i = 0; i < array.Length; i++) {
                if (GameData.Instance.GetPlayerById(array[i].ParentId).PlayerId == playerId) {
                    UnityEngine.Object.Destroy(array[i].gameObject);
                }     
            }
        }

        public static void timeMasterRewindTime() {
            TimeMaster.shieldActive = false; // Shield is no longer active when rewinding
            SoundEffectsManager.stop("timemasterShield");  // Shield sound stopped when rewinding
            if(TimeMaster.timeMaster != null && TimeMaster.timeMaster == CachedPlayer.LocalPlayer.PlayerControl) {
                resetTimeMasterButton();
            }
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(TimeMaster.rewindTime / 2, new Action<float>((p) => {
                if (p == 1f) FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = false;
            })));

            if (TimeMaster.timeMaster == null || CachedPlayer.LocalPlayer.PlayerControl == TimeMaster.timeMaster) return; // Time Master himself does not rewind

            TimeMaster.isRewinding = true;

            if (MapBehaviour.Instance)
                MapBehaviour.Instance.Close();
            if (Minigame.Instance)
                Minigame.Instance.ForceClose();
            CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
        }

        public static void timeMasterShield() {
            TimeMaster.shieldActive = true;
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(TimeMaster.shieldDuration, new Action<float>((p) => {
                if (p == 1f) TimeMaster.shieldActive = false;
            })));
        }

        public static void medicSetShielded(byte shieldedId) {
            Medic.usedShield = true;
            Medic.shielded = Helpers.playerById(shieldedId);
            Medic.futureShielded = null;
        }

        public static void shieldedMurderAttempt() {
            if (Medic.shielded == null || Medic.medic == null) return;
            
            bool isShieldedAndShow = Medic.shielded == CachedPlayer.LocalPlayer.PlayerControl && Medic.showAttemptToShielded;
            isShieldedAndShow = isShieldedAndShow && (Medic.meetingAfterShielding || !Medic.showShieldAfterMeeting);  // Dont show attempt, if shield is not shown yet
            bool isMedicAndShow = Medic.medic == CachedPlayer.LocalPlayer.PlayerControl && Medic.showAttemptToMedic;

            if (isShieldedAndShow || isMedicAndShow) Helpers.showFlash(Palette.ImpostorRed, duration: 0.5f);
        }

        public static void shifterShift(byte targetId) {
            PlayerControl oldShifter = Shifter.shifter;
            PlayerControl player = Helpers.playerById(targetId);
            if (player == null || oldShifter == null) return;

            Shifter.futureShift = null;
            Shifter.clearAndReload();

            // Suicide (exile) when impostor or impostor variants
            if (player.Data.Role.IsImpostor || Helpers.isNeutral(player)) {
                oldShifter.Exiled();
                return;
            }

            Shifter.shiftRole(oldShifter, player);

            // Set cooldowns to max for both players
            if (CachedPlayer.LocalPlayer.PlayerControl == oldShifter || CachedPlayer.LocalPlayer.PlayerControl == player)
                CustomButton.ResetAllCooldowns();
        }

        public static void swapperSwap(byte playerId1, byte playerId2) {
            if (MeetingHud.Instance) {
                Swapper.playerId1 = playerId1;
                Swapper.playerId2 = playerId2;
            }
        }

        public static void morphlingMorph(byte playerId) {  
            PlayerControl target = Helpers.playerById(playerId);
            if (Morphling.morphling == null || target == null) return;

            Morphling.morphTimer = Morphling.duration;
            Morphling.morphTarget = target;
            if (Camouflager.camouflageTimer <= 0f)
                Morphling.morphling.setLook(target.Data.PlayerName, target.Data.DefaultOutfit.ColorId, target.Data.DefaultOutfit.HatId, target.Data.DefaultOutfit.VisorId, target.Data.DefaultOutfit.SkinId, target.Data.DefaultOutfit.PetId);
        }

        public static void camouflagerCamouflage() {
            if (Camouflager.camouflager == null) return;

            Camouflager.camouflageTimer = Camouflager.duration;
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                player.setLook("", 6, "", "", "", "");
        }

        public static void vampireSetBitten(byte targetId, byte performReset) {
            if (performReset != 0) {
                Vampire.bitten = null;
                return;
            }

            if (Vampire.vampire == null) return;
            foreach (PlayerControl player in CachedPlayer.AllPlayers) {
                if (player.PlayerId == targetId && !player.Data.IsDead) {
                        Vampire.bitten = player;
                }
            }
        }

        public static void placeGarlic(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0*sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1*sizeof(float));
            new Garlic(position);
        }

        public static void trackerUsedTracker(byte targetId) {
            Tracker.usedTracker = true;
            foreach (PlayerControl player in CachedPlayer.AllPlayers)
                if (player.PlayerId == targetId)
                    Tracker.tracked = player;
        }

        public static void evilHackerCreatesMadmate(byte targetId) {
            foreach (PlayerControl player in CachedPlayer.AllPlayers) {
                if (player.PlayerId == targetId) {
                    player.RemoveInfected();
                    erasePlayerRoles(player.PlayerId, true);
                    Madmate.madmate = player;
                    EvilHacker.canCreateMadmate = false;
                    return;
                }
            }
        }

        public static void deputyUsedHandcuffs(byte targetId)
        {
            Deputy.remainingHandcuffs--;
            Deputy.handcuffedPlayers.Add(targetId);
        }

        public static void deputyPromotes()
        {
            if (Deputy.deputy != null) {  // Deputy should never be null here, but there appeared to be a race condition during testing, which was removed.
                Sheriff.replaceCurrentSheriff(Deputy.deputy);
                Sheriff.formerDeputy = Deputy.deputy;
                Deputy.deputy = null;
                // No clear and reload, as we need to keep the number of handcuffs left etc
            }
        }

        public static void jackalCreatesSidekick(byte targetId) {
            PlayerControl player = Helpers.playerById(targetId);
            if (player == null) return;
            if (Lawyer.target == player && Lawyer.isProsecutor && Lawyer.lawyer != null && !Lawyer.lawyer.Data.IsDead) Lawyer.isProsecutor = false;

            if (!Jackal.canCreateSidekickFromImpostor && player.Data.Role.IsImpostor) {
                Jackal.fakeSidekick = player;
            } else {
                bool wasSpy = Spy.spy != null && player == Spy.spy;
                bool wasImpostor = player.Data.Role.IsImpostor;  // This can only be reached if impostors can be sidekicked.
                FastDestroyableSingleton<RoleManager>.Instance.SetRole(player, RoleTypes.Crewmate);
                if (player == Lawyer.lawyer && Lawyer.target != null)
                {
                    Transform playerInfoTransform = Lawyer.target.cosmetics.nameText.transform.parent.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (playerInfo != null) playerInfo.text = "";
                }
                erasePlayerRoles(player.PlayerId, true);
                Sidekick.sidekick = player;
                if (player.PlayerId == CachedPlayer.LocalPlayer.PlayerId) CachedPlayer.LocalPlayer.PlayerControl.moveable = true;
                if (wasSpy || wasImpostor) Sidekick.wasTeamRed = true;
                Sidekick.wasSpy = wasSpy;
                Sidekick.wasImpostor = wasImpostor;
                if (player == CachedPlayer.LocalPlayer.PlayerControl) SoundEffectsManager.play("jackalSidekick");
            }
            Jackal.canCreateSidekick = false;
        }

        public static void sidekickPromotes() {
            Jackal.removeCurrentJackal();
            Jackal.jackal = Sidekick.sidekick;
            Jackal.canCreateSidekick = Jackal.jackalPromotedFromSidekickCanCreateSidekick;
            Jackal.wasTeamRed = Sidekick.wasTeamRed;
            Jackal.wasSpy = Sidekick.wasSpy;
            Jackal.wasImpostor = Sidekick.wasImpostor;
            Sidekick.clearAndReload();
            return;
        }
        
        public static void erasePlayerRoles(byte playerId, bool ignoreModifier = true) {
            PlayerControl player = Helpers.playerById(playerId);
            if (player == null) return;

            // Crewmate roles
            if (player == Mayor.mayor) Mayor.clearAndReload();
            if (player == Portalmaker.portalmaker) Portalmaker.clearAndReload();
            if (player == Engineer.engineer) Engineer.clearAndReload();
            if (player == Sheriff.sheriff) Sheriff.clearAndReload();
            if (player == Deputy.deputy) Deputy.clearAndReload();
            if (player == Lighter.lighter) Lighter.clearAndReload();
            if (player == Detective.detective) Detective.clearAndReload();
            if (player == TimeMaster.timeMaster) TimeMaster.clearAndReload();
            if (player == Medic.medic) Medic.clearAndReload();
            if (player == Shifter.shifter) Shifter.clearAndReload();
            if (player == Seer.seer) Seer.clearAndReload();
            if (player == Hacker.hacker) Hacker.clearAndReload();
            if (player == Tracker.tracker) Tracker.clearAndReload();
            if (player == Snitch.snitch) Snitch.clearAndReload();
            if (player == Swapper.swapper) Swapper.clearAndReload();
            if (player == Spy.spy) Spy.clearAndReload();
            if (player == SecurityGuard.securityGuard) SecurityGuard.clearAndReload();
            if (player == Medium.medium) Medium.clearAndReload();
            if (player == Trapper.trapper) Trapper.clearAndReload();
            if (player == Madmate.madmate) Madmate.clearAndReload();
            if (player == Yasuna.yasuna) Yasuna.clearAndReload();
            if (player == TaskMaster.taskMaster) TaskMaster.clearAndReload();

            // Impostor roles
            if (player == Morphling.morphling) Morphling.clearAndReload();
            if (player == Camouflager.camouflager) Camouflager.clearAndReload();
            if (player == Godfather.godfather) Godfather.clearAndReload();
            if (player == Mafioso.mafioso) Mafioso.clearAndReload();
            if (player == Janitor.janitor) Janitor.clearAndReload();
            if (player == Vampire.vampire) Vampire.clearAndReload();
            if (player == Eraser.eraser) Eraser.clearAndReload();
            if (player == Trickster.trickster) Trickster.clearAndReload();
            if (player == Cleaner.cleaner) Cleaner.clearAndReload();
            if (player == Warlock.warlock) Warlock.clearAndReload();
            if (player == Witch.witch) Witch.clearAndReload();
            if (player == Ninja.ninja) Ninja.clearAndReload();
            if (player == DoorHacker.doorHacker) DoorHacker.clearAndReload();
            if (player == KillerCreator.killerCreator) KillerCreator.clearAndReload();
            if (player == MadmateKiller.madmateKiller) MadmateKiller.clearAndReload();

            // Other roles
            if (player == Jester.jester) Jester.clearAndReload();
            if (player == Arsonist.arsonist) Arsonist.clearAndReload();
            if (player == Kataomoi.kataomoi) Kataomoi.clearAndReload();
            if (Guesser.isGuesser(player.PlayerId)) Guesser.clear(player.PlayerId);
            if (player == Jackal.jackal) { // Promote Sidekick and hence override the the Jackal or erase Jackal
                if (Sidekick.promotesToJackal && Sidekick.sidekick != null && !Sidekick.sidekick.Data.IsDead) {
                    RPCProcedure.sidekickPromotes();
                } else {
                    Jackal.clearAndReload();
                }
            }
            if (player == Sidekick.sidekick) Sidekick.clearAndReload();
            if (player == BountyHunter.bountyHunter) BountyHunter.clearAndReload();
            if (player == Vulture.vulture) Vulture.clearAndReload();
            if (player == Lawyer.lawyer) Lawyer.clearAndReload();
            if (player == Pursuer.pursuer) Pursuer.clearAndReload();
            if (player == Thief.thief) Thief.clearAndReload();

            // Modifier
            if (!ignoreModifier)
            {
                if (player == Lovers.lover1 || player == Lovers.lover2) Lovers.clearAndReload(); // The whole Lover couple is being erased
                if (Bait.bait.Any(x => x.PlayerId == player.PlayerId)) Bait.bait.RemoveAll(x => x.PlayerId == player.PlayerId);
                if (Bloody.bloody.Any(x => x.PlayerId == player.PlayerId)) Bloody.bloody.RemoveAll(x => x.PlayerId == player.PlayerId);
                if (AntiTeleport.antiTeleport.Any(x => x.PlayerId == player.PlayerId)) AntiTeleport.antiTeleport.RemoveAll(x => x.PlayerId == player.PlayerId);
                if (Sunglasses.sunglasses.Any(x => x.PlayerId == player.PlayerId)) Sunglasses.sunglasses.RemoveAll(x => x.PlayerId == player.PlayerId);
                if (player == Tiebreaker.tiebreaker) Tiebreaker.clearAndReload();
                if (player == Mini.mini) Mini.clearAndReload();
                if (Vip.vip.Any(x => x.PlayerId == player.PlayerId)) Vip.vip.RemoveAll(x => x.PlayerId == player.PlayerId);
                if (Invert.invert.Any(x => x.PlayerId == player.PlayerId)) Invert.invert.RemoveAll(x => x.PlayerId == player.PlayerId);
                if (Chameleon.chameleon.Any(x => x.PlayerId == player.PlayerId)) Chameleon.chameleon.RemoveAll(x => x.PlayerId == player.PlayerId);
            }
        }

        public static void setFutureErased(byte playerId) {
            PlayerControl player = Helpers.playerById(playerId);
            if (Eraser.futureErased == null) 
                Eraser.futureErased = new List<PlayerControl>();
            if (player != null) {
                Eraser.futureErased.Add(player);
            }
        }

        public static void setFutureShifted(byte playerId) {
            Shifter.futureShift = Helpers.playerById(playerId);
        }

        public static void setFutureShielded(byte playerId) {
            Medic.futureShielded = Helpers.playerById(playerId);
            Medic.usedShield = true;
        }

        public static void setFutureSpelled(byte playerId) {
            PlayerControl player = Helpers.playerById(playerId);
            if (Witch.futureSpelled == null)
                Witch.futureSpelled = new List<PlayerControl>();
            if (player != null) {
                Witch.futureSpelled.Add(player);
            }
        }

        public static void placeNinjaTrace(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new NinjaTrace(position, Ninja.traceTime);
        }

        public static void setInvisible(byte playerId, byte flag)
        {
            PlayerControl target = Helpers.playerById(playerId);
            if (target == null) return;
            if (flag == byte.MaxValue)
            {
                target.cosmetics.currentBodySprite.BodySprite.color = Color.white;
                target.cosmetics.colorBlindText.gameObject.SetActive(DataManager.Settings.Accessibility.ColorBlindMode);
                if (Camouflager.camouflageTimer <= 0) target.setDefaultLook();
                Ninja.isInvisble = false;
                return;
            }

            target.setLook("", 6, "", "", "", "");
            Color color = Color.clear;           
            if (CachedPlayer.LocalPlayer.Data.Role.IsImpostor || CachedPlayer.LocalPlayer.Data.IsDead) color.a = 0.1f;
            target.cosmetics.currentBodySprite.BodySprite.color = color;
            target.cosmetics.colorBlindText.gameObject.SetActive(false);
            Ninja.invisibleTimer = Ninja.invisibleDuration;
            Ninja.isInvisble = true;
        }

        public static void placePortal(byte[] buff) {
            Vector3 position = Vector2.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new Portal(position);
        }

        public static void usePortal(byte playerId) {
            Portal.startTeleport(playerId);
        }

        public static void placeJackInTheBox(byte[] buff) {
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0*sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1*sizeof(float));
            new JackInTheBox(position);
        }

        public static void lightsOut() {
            Trickster.lightsOutTimer = Trickster.lightsOutDuration;
            // If the local player is impostor indicate lights out
            if (Helpers.hasImpVision(GameData.Instance.GetPlayerById(CachedPlayer.LocalPlayer.PlayerId))) {
                new CustomMessage("Lights are out", Trickster.lightsOutDuration);
            }
        }

        public static void placeCamera(byte[] buff) {
            var referenceCamera = UnityEngine.Object.FindObjectOfType<SurvCamera>(); 
            if (referenceCamera == null) return; // Mira HQ

            SecurityGuard.remainingScrews -= SecurityGuard.camPrice;
            SecurityGuard.placedCameras++;

            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0*sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1*sizeof(float));

            var camera = UnityEngine.Object.Instantiate<SurvCamera>(referenceCamera);
            camera.transform.position = new Vector3(position.x, position.y, referenceCamera.transform.position.z - 1f);
            camera.CamName = $"Security Camera {SecurityGuard.placedCameras}";
            camera.Offset = new Vector3(0f, 0f, camera.Offset.z);
            if (PlayerControl.GameOptions.MapId == 2 || PlayerControl.GameOptions.MapId == 4) camera.transform.localRotation = new Quaternion(0, 0, 1, 1); // Polus and Airship 

            if (SubmergedCompatibility.IsSubmerged) {
                // remove 2d box collider of console, so that no barrier can be created. (irrelevant for now, but who knows... maybe we need it later)
                var fixConsole = camera.transform.FindChild("FixConsole");
                if (fixConsole != null) {
                    var boxCollider = fixConsole.GetComponent<BoxCollider2D>();
                    if (boxCollider != null) UnityEngine.Object.Destroy(boxCollider);
                }
            }


            if (CachedPlayer.LocalPlayer.PlayerControl == SecurityGuard.securityGuard) {
                camera.gameObject.SetActive(true);
                camera.gameObject.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
            } else {
                camera.gameObject.SetActive(false);
            }
            MapOptions.camerasToAdd.Add(camera);
        }

        public static void sealVent(int ventId) {
            Vent vent = MapUtilities.CachedShipStatus.AllVents.FirstOrDefault((x) => x != null && x.Id == ventId);
            if (vent == null) return;

            SecurityGuard.remainingScrews -= SecurityGuard.ventPrice;
            if (CachedPlayer.LocalPlayer.PlayerControl == SecurityGuard.securityGuard) {
                PowerTools.SpriteAnim animator = vent.GetComponent<PowerTools.SpriteAnim>(); 
                animator?.Stop();
                vent.EnterVentAnim = vent.ExitVentAnim = null;
                vent.myRend.sprite = animator == null ? SecurityGuard.getStaticVentSealedSprite() : SecurityGuard.getAnimatedVentSealedSprite();
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 0) vent.myRend.sprite = SecurityGuard.getSubmergedCentralUpperSealedSprite();
                if (SubmergedCompatibility.IsSubmerged && vent.Id == 14) vent.myRend.sprite = SecurityGuard.getSubmergedCentralLowerSealedSprite();
                vent.myRend.color = new Color(1f, 1f, 1f, 0.5f);
                vent.name = "FutureSealedVent_" + vent.name;
            }

            MapOptions.ventsToSeal.Add(vent);
        }

        public static void arsonistWin() {
            Arsonist.triggerArsonistWin = true;
            foreach (PlayerControl p in CachedPlayer.AllPlayers) {
                if (p != Arsonist.arsonist) p.Exiled();
            }
        }

        public static void vultureWin() {
            Vulture.triggerVultureWin = true;
        }

        public static void lawyerSetTarget(byte playerId) {
            Lawyer.target = Helpers.playerById(playerId);
        }

        public static void lawyerPromotesToPursuer() {
            PlayerControl player = Lawyer.lawyer;
            PlayerControl client = Lawyer.target;
            Lawyer.clearAndReload(false);

            Pursuer.pursuer = player;

            if (player.PlayerId == CachedPlayer.LocalPlayer.PlayerId && client != null) {
                    Transform playerInfoTransform = client.cosmetics.nameText.transform.parent.FindChild("Info");
                    TMPro.TextMeshPro playerInfo = playerInfoTransform != null ? playerInfoTransform.GetComponent<TMPro.TextMeshPro>() : null;
                    if (playerInfo != null) playerInfo.text = "";
            }
        }

        public static void guesserShoot(byte killerId, byte dyingTargetId, byte guessedTargetId, byte guessedRoleId) {
            PlayerControl dyingTarget = Helpers.playerById(dyingTargetId);
            if (dyingTarget == null ) return;
            if (Lawyer.target != null && dyingTarget == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for guesses
            PlayerControl dyingLoverPartner = Lovers.bothDie ? dyingTarget.getPartner() : null; // Lover check
            if (Lawyer.target != null && dyingLoverPartner == Lawyer.target) Lawyer.targetWasGuessed = true;  // Lawyer shouldn't be exiled with the client for guesses
            dyingTarget.Exiled();
            PlayerControl kataomoiPlayer = Kataomoi.kataomoi != null && Kataomoi.target == dyingTarget ? Kataomoi.kataomoi : null; // Kataomoi check
            byte partnerId = dyingLoverPartner != null ? dyingLoverPartner.PlayerId : dyingTargetId;
            byte partnerId2 = kataomoiPlayer != null ? kataomoiPlayer.PlayerId : dyingTargetId;

            HandleGuesser.remainingShots(killerId, true);
            if (Constants.ShouldPlaySfx()) SoundManager.Instance.PlaySound(dyingTarget.KillSfx, false, 0.8f);
            if (MeetingHud.Instance) {
                foreach (PlayerVoteArea pva in MeetingHud.Instance.playerStates) {
                    if (pva.TargetPlayerId == dyingTargetId || pva.TargetPlayerId == partnerId || pva.TargetPlayerId == partnerId2) {
                        pva.SetDead(pva.DidReport, true);
                        pva.Overlay.gameObject.SetActive(true);
                    }

                    //Give players back their vote if target is shot dead
                    if (pva.VotedFor != dyingTargetId || pva.VotedFor != partnerId || pva.VotedFor != partnerId2) continue;
                    pva.UnsetVote();
                    var voteAreaPlayer = Helpers.playerById(pva.TargetPlayerId);
                    if (!voteAreaPlayer.AmOwner) continue;
                    MeetingHud.Instance.ClearVote();

                }
                if (AmongUsClient.Instance.AmHost) 
                    MeetingHud.Instance.CheckForEndVoting();
            }
            PlayerControl guesser = Helpers.playerById(killerId);
            if (FastDestroyableSingleton<HudManager>.Instance != null && guesser != null)
                if (CachedPlayer.LocalPlayer.PlayerControl == dyingTarget) {
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(guesser.Data, dyingTarget.Data);
                    if (MeetingHudPatch.guesserUI != null) MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
                } else if (dyingLoverPartner != null && CachedPlayer.LocalPlayer.PlayerControl == dyingLoverPartner) {
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(dyingLoverPartner.Data, dyingLoverPartner.Data);
                    if (MeetingHudPatch.guesserUI != null) MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
                }
            
                else if (kataomoiPlayer != null && CachedPlayer.LocalPlayer.PlayerControl == kataomoiPlayer)
                    FastDestroyableSingleton<HudManager>.Instance.KillOverlay.ShowKillAnimation(kataomoiPlayer.Data, kataomoiPlayer.Data);

            // remove shoot button from targets for all guessers and close their guesserUI
            if (GuesserGM.isGuesser(PlayerControl.LocalPlayer.PlayerId) && PlayerControl.LocalPlayer != guesser && !PlayerControl.LocalPlayer.Data.IsDead && GuesserGM.remainingShots(PlayerControl.LocalPlayer.PlayerId) > 0 && MeetingHud.Instance)
            {
                MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingTarget.PlayerId && x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });
                if (dyingLoverPartner != null)
                    MeetingHud.Instance.playerStates.ToList().ForEach(x => { if (x.TargetPlayerId == dyingLoverPartner.PlayerId && x.transform.FindChild("ShootButton") != null) UnityEngine.Object.Destroy(x.transform.FindChild("ShootButton").gameObject); });

                if (MeetingHudPatch.guesserUI != null && MeetingHudPatch.guesserUIExitButton != null)
                {
                    if (MeetingHudPatch.guesserCurrentTarget == dyingTarget.PlayerId)
                        MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
                    else if (dyingLoverPartner != null && MeetingHudPatch.guesserCurrentTarget == dyingLoverPartner.PlayerId)
                        MeetingHudPatch.guesserUIExitButton.OnClick.Invoke();
                }
            }

            PlayerControl guessedTarget = Helpers.playerById(guessedTargetId);
            if (CachedPlayer.LocalPlayer.Data.IsDead && guessedTarget != null && guesser != null) {
                RoleInfo roleInfo = RoleInfo.allRoleInfos.FirstOrDefault(x => (byte)x.roleId == guessedRoleId);
                string msg = $"{guesser.Data.PlayerName} guessed the role {roleInfo?.name ?? ""} for {guessedTarget.Data.PlayerName}!";
                if (AmongUsClient.Instance.AmClient && FastDestroyableSingleton<HudManager>.Instance)
                    FastDestroyableSingleton<HudManager>.Instance.Chat.AddChat(guesser, msg);
                if (msg.IndexOf("who", StringComparison.OrdinalIgnoreCase) >= 0)
                    FastDestroyableSingleton<Assets.CoreScripts.Telemetry>.Instance.SendWho();
            }
        }

        public static void setBlanked(byte playerId, byte value) {
            PlayerControl target = Helpers.playerById(playerId);
            if (target == null) return;
            Pursuer.blankedList.RemoveAll(x => x.PlayerId == playerId);
            if (value > 0) Pursuer.blankedList.Add(target);            
        }

        public static void bloody(byte killerPlayerId, byte bloodyPlayerId) {
            if (Bloody.active.ContainsKey(killerPlayerId)) return;
            Bloody.active.Add(killerPlayerId, Bloody.duration);
            Bloody.bloodyKillerMap.Add(killerPlayerId, bloodyPlayerId);
        }

        public static void setFirstKill(byte playerId) {
            PlayerControl target = Helpers.playerById(playerId);
            if (target == null) return;
            MapOptions.firstKillPlayer = target;
        }

        public static void setTiebreak()
        {
            Tiebreaker.isTiebreak = true;
        }

        public static void thiefStealsRole(byte playerId)
        {
            PlayerControl target = Helpers.playerById(playerId);
            PlayerControl thief = Thief.thief;
            if (target == null) return;
            if (target == Sheriff.sheriff) Sheriff.sheriff = thief;
            if (target == Jackal.jackal)
            {
                Jackal.jackal = thief;
                Jackal.formerJackals.Add(target);
            }
            if (target == Sidekick.sidekick)
            {
                Sidekick.sidekick = thief;
                Jackal.formerJackals.Add(target);
            }
            if (target == Guesser.evilGuesser) Guesser.evilGuesser = thief;
            if (target == Godfather.godfather) Godfather.godfather = thief;
            if (target == Mafioso.mafioso) Mafioso.mafioso = thief;
            if (target == Janitor.janitor) Janitor.janitor = thief;
            if (target == Morphling.morphling) Morphling.morphling = thief;
            if (target == Camouflager.camouflager) Camouflager.camouflager = thief;
            if (target == Vampire.vampire) Vampire.vampire = thief;
            if (target == Eraser.eraser) Eraser.eraser = thief;
            if (target == Trickster.trickster) Trickster.trickster = thief;
            if (target == Cleaner.cleaner) Cleaner.cleaner = thief;
            if (target == Warlock.warlock) Warlock.warlock = thief;
            if (target == BountyHunter.bountyHunter) BountyHunter.bountyHunter = thief;
            if (target == Witch.witch) Witch.witch = thief;
            if (target == Ninja.ninja) Ninja.ninja = thief;
            if (target.Data.Role.IsImpostor)
            {
                RoleManager.Instance.SetRole(Thief.thief, RoleTypes.Impostor);
                FastDestroyableSingleton<HudManager>.Instance.KillButton.SetCoolDown(Thief.thief.killTimer, PlayerControl.GameOptions.KillCooldown);
            }
            if (Lawyer.lawyer != null && target == Lawyer.target)
                Lawyer.target = thief;
            if (Thief.thief == PlayerControl.LocalPlayer) CustomButton.ResetAllCooldowns();
            Thief.clearAndReload();
            Thief.formerThief = thief;  // After clearAndReload, else it would get reset...
        }

        public static void setTrap(byte[] buff)
        {
            if (Trapper.trapper == null) return;
            Trapper.charges -= 1;
            Vector3 position = Vector3.zero;
            position.x = BitConverter.ToSingle(buff, 0 * sizeof(float));
            position.y = BitConverter.ToSingle(buff, 1 * sizeof(float));
            new Trap(position);
        }

        public static void triggerTrap(byte playerId, byte trapId)
        {
            Trap.triggerTrap(playerId, trapId);
        }

        public static void setGuesserGm(byte playerId)
        {
            PlayerControl target = Helpers.playerById(playerId);
            if (target == null) return;
            new GuesserGM(target);
        }

        public static void shareTimer(float punish)
        {
            HideNSeek.timer -= punish;
        }

        public static void huntedShield(byte playerId)
        {
            if (!Hunted.timeshieldActive.Contains(playerId)) Hunted.timeshieldActive.Add(playerId);
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Hunted.shieldDuration, new Action<float>((p) => {
                if (p == 1f) Hunted.timeshieldActive.Remove(playerId);
            })));
        }

        public static void huntedRewindTime(byte playerId)
        {
            Hunted.timeshieldActive.Remove(playerId); // Shield is no longer active when rewinding
            SoundEffectsManager.stop("timemasterShield");  // Shield sound stopped when rewinding
            if (playerId == CachedPlayer.LocalPlayer.PlayerControl.PlayerId)
            {
                resetHuntedRewindButton();
            }
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.color = new Color(0f, 0.5f, 0.8f, 0.3f);
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = true;
            FastDestroyableSingleton<HudManager>.Instance.FullScreen.gameObject.SetActive(true);
            FastDestroyableSingleton<HudManager>.Instance.StartCoroutine(Effects.Lerp(Hunted.shieldRewindTime, new Action<float>((p) => {
                if (p == 1f) FastDestroyableSingleton<HudManager>.Instance.FullScreen.enabled = false;
            })));

            if (!CachedPlayer.LocalPlayer.Data.Role.IsImpostor) return; // only rewind hunter

            TimeMaster.isRewinding = true;

            if (MapBehaviour.Instance)
                MapBehaviour.Instance.Close();
            if (Minigame.Instance)
                Minigame.Instance.ForceClose();
            CachedPlayer.LocalPlayer.PlayerControl.moveable = false;
        }

        public static void yasunaSpecialVote(byte playerid, byte targetid) {
            if (!MeetingHud.Instance) return;
            if (!Yasuna.isYasuna(playerid)) return;
            PlayerControl target = Helpers.playerById(targetid);
            if (target == null) return;
            Yasuna.specialVoteTargetPlayerId = targetid;
            Yasuna.remainingSpecialVotes(true);
        }

        public static void yasunaSpecialVote_DoCastVote() {
            if (!MeetingHud.Instance) return;
            if (!Yasuna.isYasuna(CachedPlayer.LocalPlayer.PlayerControl.PlayerId)) return;
            PlayerControl target = Helpers.playerById(Yasuna.specialVoteTargetPlayerId);
            if (target == null) return;
            MeetingHud.Instance.CmdCastVote(CachedPlayer.LocalPlayer.PlayerControl.PlayerId, target.PlayerId);
        }

        public static void taskMasterSetExTasks(byte playerId, byte oldTaskMasterPlayerId, byte[] taskTypeIds) {
            PlayerControl oldTaskMasterPlayer = Helpers.playerById(oldTaskMasterPlayerId);
            if (oldTaskMasterPlayer != null) {
                oldTaskMasterPlayer.clearAllTasks();
                TaskMaster.oldTaskMasterPlayerId = oldTaskMasterPlayerId;
            }

            if (!TaskMaster.isTaskMaster(playerId)) 
                return;
            GameData.PlayerInfo player = GameData.Instance.GetPlayerById(playerId);
            if (player == null)
                return;

            if (taskTypeIds != null && taskTypeIds.Length > 0) {
                player.Object.clearAllTasks();
                player.Tasks = new Il2CppSystem.Collections.Generic.List<GameData.TaskInfo>(taskTypeIds.Length);
                for (int i = 0; i < taskTypeIds.Length; i++) {
                    player.Tasks.Add(new GameData.TaskInfo(taskTypeIds[i], (uint)i));
                    player.Tasks[i].Id = (uint)i;
                }
                for (int i = 0; i < player.Tasks.Count; i++) {
                    GameData.TaskInfo taskInfo = player.Tasks[i];
                    NormalPlayerTask normalPlayerTask = UnityEngine.Object.Instantiate(MapUtilities.CachedShipStatus.GetTaskById(taskInfo.TypeId), player.Object.transform);
                    normalPlayerTask.Id = taskInfo.Id;
                    normalPlayerTask.Owner = player.Object;
                    normalPlayerTask.Initialize();
                    player.Object.myTasks.Add(normalPlayerTask);
                }
                TaskMaster.isTaskComplete = true;
            } else {
                TaskMaster.isTaskComplete = false;
            }
        }

        public static void taskMasterUpdateExTasks(byte clearExTasks, byte allExTasks) {
            if (TaskMaster.taskMaster == null) return;
            TaskMaster.clearExTasks = clearExTasks;
            TaskMaster.allExTasks = allExTasks;
        }

        public static void doorHackerDone(byte playerId) {
            PlayerControl player = Helpers.playerById(playerId);
            if (DoorHacker.doorHacker == null || DoorHacker.doorHacker != player) return;
            DoorHacker.DisableDoors(playerId);
        }

        public static void kataomoiSetTarget(byte playerId) {
            Kataomoi.target = Helpers.playerById(playerId);
        }

        public static void kataomoiWin() {
            if (Kataomoi.kataomoi == null) return;

            Kataomoi.triggerKataomoiWin = true;
            if (Kataomoi.target != null)
                Kataomoi.target.Exiled();
        }

        public static void kataomoiStalking(byte playerId) {
            PlayerControl player = Helpers.playerById(playerId);
            if (Kataomoi.kataomoi == null || Kataomoi.kataomoi != player) return;

            Kataomoi.doStalking();
        }

        public static void synchronize(byte playerId, int tag) {
            SpawnInMinigamePatch.SynchronizeData((SpawnInMinigamePatch.SynchronizeTag)tag, playerId);
        }

        public static void killerCreatorCreatesMadmateKiller(byte targetId) {
            if (KillerCreator.killerCreator == null) return;
            if (MadmateKiller.madmateKiller != null) return;

            foreach (PlayerControl player in CachedPlayer.AllPlayers) {
                if (player.PlayerId == targetId) {
                    erasePlayerRoles(player.PlayerId, true);
                    MadmateKiller.madmateKiller = player;

                    if (player == CachedPlayer.LocalPlayer.PlayerControl)
                        SoundEffectsManager.play("jackalSidekick");

                    DestroyableSingleton<RoleManager>.Instance.SetRole(player, RoleTypes.Crewmate);
                    break;
                }
            }
        }

        public static void madmateKillerPromotes() {
            if (KillerCreator.killerCreator == null) return;
            if (MadmateKiller.madmateKiller == null || MadmateKiller.madmateKiller.Data.RoleType == RoleTypes.Impostor) return;

            MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(PlayerControl.LocalPlayer.NetId, (byte)CustomRPC.UncheckedSetVanilaRole, SendOption.Reliable);
            writer.Write(MadmateKiller.madmateKiller.PlayerId);
            writer.Write((byte)RoleTypes.Impostor);
            AmongUsClient.Instance.FinishRpcImmediately(writer);
            uncheckedSetVanilaRole(MadmateKiller.madmateKiller.PlayerId, RoleTypes.Impostor);
        }
    }

    [HarmonyPatch(typeof(PlayerControl), nameof(PlayerControl.HandleRpc))]
    class RPCHandlerPatch
    {
        static void Postfix([HarmonyArgument(0)]byte callId, [HarmonyArgument(1)]MessageReader reader)
        {
            byte packetId = callId;
            switch (packetId) {

                // Main Controls

                case (byte)CustomRPC.ResetVaribles:
                    RPCProcedure.resetVariables();
                    break;
                case (byte)CustomRPC.ShareOptions:
                    RPCProcedure.HandleShareOptions(reader.ReadByte(), reader);
                    break;
                case (byte)CustomRPC.ForceEnd:
                    RPCProcedure.forceEnd();
                    break; 
                case (byte)CustomRPC.WorkaroundSetRoles:
                    RPCProcedure.workaroundSetRoles(reader.ReadByte(), reader);
                    break;
                case (byte)CustomRPC.SetRole:
                    byte roleId = reader.ReadByte();
                    byte playerId = reader.ReadByte();
                    RPCProcedure.setRole(roleId, playerId);
                    break;
                case (byte)CustomRPC.SetModifier:
                    byte modifierId = reader.ReadByte();
                    byte pId = reader.ReadByte();
                    byte flag = reader.ReadByte();
                    RPCProcedure.setModifier(modifierId, pId, flag);
                    break;
                case (byte)CustomRPC.VersionHandshake:
                    byte major = reader.ReadByte();
                    byte minor = reader.ReadByte();
                    byte patch = reader.ReadByte();
                    float timer = reader.ReadSingle();
                    if (!AmongUsClient.Instance.AmHost && timer >= 0f) GameStartManagerPatch.timer = timer;
                    int versionOwnerId = reader.ReadPackedInt32();
                    byte revision = 0xFF;
                    Guid guid;
                    if (reader.Length - reader.Position >= 17) { // enough bytes left to read
                        revision = reader.ReadByte();
                        // GUID
                        byte[] gbytes = reader.ReadBytes(16);
                        guid = new Guid(gbytes);
                    } else {
                        guid = new Guid(new byte[16]);
                    }
                    RPCProcedure.versionHandshake(major, minor, patch, revision == 0xFF ? -1 : revision, guid, versionOwnerId);
                    break;
                case (byte)CustomRPC.UseUncheckedVent:
                    int ventId = reader.ReadPackedInt32();
                    byte ventingPlayer = reader.ReadByte();
                    byte isEnter = reader.ReadByte();
                    RPCProcedure.useUncheckedVent(ventId, ventingPlayer, isEnter);
                    break;
                case (byte)CustomRPC.UncheckedMurderPlayer:
                    byte source = reader.ReadByte();
                    byte target = reader.ReadByte();
                    byte showAnimation = reader.ReadByte();
                    RPCProcedure.uncheckedMurderPlayer(source, target, showAnimation);
                    break;
                case (byte)CustomRPC.UncheckedExilePlayer:
                    byte exileTarget = reader.ReadByte();
                    RPCProcedure.uncheckedExilePlayer(exileTarget);
                    break;
                case (byte)CustomRPC.UncheckedCmdReportDeadBody:
                    byte reportSource = reader.ReadByte();
                    byte reportTarget = reader.ReadByte();
                    RPCProcedure.uncheckedCmdReportDeadBody(reportSource, reportTarget);
                    break;
                case (byte)CustomRPC.DynamicMapOption:
                    byte mapId = reader.ReadByte();
                    RPCProcedure.dynamicMapOption(mapId);
                    break;
                case (byte)CustomRPC.SetGameStarting:
                    RPCProcedure.setGameStarting();
                    break;
                case (byte)CustomRPC.ConsumeAdminTime:
                    float delta = reader.ReadSingle();
                    RPCProcedure.consumeAdminTime(delta);
                    break;
                case (byte)CustomRPC.ConsumeVitalTime:
                    RPCProcedure.consumeVitalTime(reader.ReadSingle());
                    break;
                case (byte)CustomRPC.ConsumeSecurityCameraTime:
                    RPCProcedure.consumeSecurityCameraTime(reader.ReadSingle());
                    break;
                case (byte)CustomRPC.UncheckedEndGame:
                    byte reason = reader.ReadByte();
                    RPCProcedure.uncheckedEndGame(reason);
                    break;
                case (byte)CustomRPC.UncheckedEndGame_Response:
                    playerId = reader.ReadByte();
                    RPCProcedure.uncheckedEndGameResponse(playerId);
                    break;
                case (byte)CustomRPC.UncheckedSetVanilaRole:
                    RPCProcedure.uncheckedSetVanilaRole(reader.ReadByte(), (RoleTypes)reader.ReadByte());
                    break;
                case (byte)CustomRPC.TaskVsMode_Ready: // Task Vs Mode
                    RPCProcedure.taskVsModeReady(reader.ReadByte());
                    break;
                case (byte)CustomRPC.TaskVsMode_Start: // Task Vs Mode
                    RPCProcedure.taskVsModeStart();
                    break;
                case (byte)CustomRPC.TaskVsMode_AllTaskCompleted: // Task Vs Mode
                    playerId = reader.ReadByte();
                    var timeMilliSec = reader.ReadUInt64();
                    RPCProcedure.taskVsModeAllTaskCompleted(playerId, timeMilliSec);
                    break;

                case (byte)CustomRPC.TaskVsMode_MakeItTheSameTaskAsTheHost: // Task Vs Mode
                    byte[] taskTypeIds = reader.BytesRemaining > 0 ? reader.ReadBytes(reader.BytesRemaining) : null;
                    RPCProcedure.taskVsModeMakeItTheSameTaskAsTheHost(taskTypeIds);
                    break;

                case (byte)CustomRPC.TaskVsMode_MakeItTheSameTaskAsTheHostDetail: // Task Vs Mode
                    uint taskId = reader.ReadUInt32();
                    byte[] data = reader.BytesRemaining > 0 ? reader.ReadBytes(reader.BytesRemaining) : null;
                    RPCProcedure.taskVsModeMakeItTheSameTaskAsTheHostDetail(taskId, data);
                    break;

                // Role functionality

                case (byte)CustomRPC.EngineerFixLights:
                    RPCProcedure.engineerFixLights();
                    break;
                case (byte)CustomRPC.EngineerFixSubmergedOxygen:
                    RPCProcedure.engineerFixSubmergedOxygen();
                    break;
                case (byte)CustomRPC.EngineerUsedRepair:
                    RPCProcedure.engineerUsedRepair();
                    break;
                case (byte)CustomRPC.CleanBody:
                    RPCProcedure.cleanBody(reader.ReadByte());
                    break;
                case (byte)CustomRPC.TimeMasterRewindTime:
                    RPCProcedure.timeMasterRewindTime();
                    break;
                case (byte)CustomRPC.TimeMasterShield:
                    RPCProcedure.timeMasterShield();
                    break;
                case (byte)CustomRPC.MedicSetShielded:
                    RPCProcedure.medicSetShielded(reader.ReadByte());
                    break;
                case (byte)CustomRPC.ShieldedMurderAttempt:
                    RPCProcedure.shieldedMurderAttempt();
                    break;
                case (byte)CustomRPC.ShifterShift:
                    RPCProcedure.shifterShift(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SwapperSwap:
                    byte playerId1 = reader.ReadByte();
                    byte playerId2 = reader.ReadByte();
                    RPCProcedure.swapperSwap(playerId1, playerId2);
                    break;
                case (byte)CustomRPC.MorphlingMorph:
                    RPCProcedure.morphlingMorph(reader.ReadByte());
                    break;
                case (byte)CustomRPC.CamouflagerCamouflage:
                    RPCProcedure.camouflagerCamouflage();
                    break;
                case (byte)CustomRPC.VampireSetBitten:
                    byte bittenId = reader.ReadByte();
                    byte reset = reader.ReadByte();
                    RPCProcedure.vampireSetBitten(bittenId, reset);
                    break;
                case (byte)CustomRPC.PlaceGarlic:
                    RPCProcedure.placeGarlic(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.TrackerUsedTracker:
                    RPCProcedure.trackerUsedTracker(reader.ReadByte());
                    break;               
                case (byte)CustomRPC.DeputyUsedHandcuffs:
                    RPCProcedure.deputyUsedHandcuffs(reader.ReadByte());
                    break;
                case (byte)CustomRPC.EvilHackerCreatesMadmate:
                    RPCProcedure.evilHackerCreatesMadmate(reader.ReadByte());
                    break;
                case (byte)CustomRPC.DeputyPromotes:
                    RPCProcedure.deputyPromotes();
                    break;
                case (byte)CustomRPC.JackalCreatesSidekick:
                    RPCProcedure.jackalCreatesSidekick(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SidekickPromotes:
                    RPCProcedure.sidekickPromotes();
                    break;
                case (byte)CustomRPC.ErasePlayerRoles:
                    byte eraseTarget = reader.ReadByte();
                    RPCProcedure.erasePlayerRoles(eraseTarget);
                    Eraser.alreadyErased.Add(eraseTarget);
                    break;
                case (byte)CustomRPC.SetFutureErased:
                    RPCProcedure.setFutureErased(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SetFutureShifted:
                    RPCProcedure.setFutureShifted(reader.ReadByte());
                    break;
                case (byte)CustomRPC.SetFutureShielded:
                    RPCProcedure.setFutureShielded(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PlaceNinjaTrace:
                    RPCProcedure.placeNinjaTrace(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.PlacePortal:
                    RPCProcedure.placePortal(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.UsePortal:
                    RPCProcedure.usePortal(reader.ReadByte());
                    break;
                case (byte)CustomRPC.PlaceJackInTheBox:
                    RPCProcedure.placeJackInTheBox(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.LightsOut:
                    RPCProcedure.lightsOut();
                    break;
                case (byte)CustomRPC.PlaceCamera:
                    RPCProcedure.placeCamera(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.SealVent:
                    RPCProcedure.sealVent(reader.ReadPackedInt32());
                    break;
                case (byte)CustomRPC.ArsonistWin:
                    RPCProcedure.arsonistWin();
                    break;
                case (byte)CustomRPC.GuesserShoot:
                    byte killerId = reader.ReadByte();
                    byte dyingTarget = reader.ReadByte();
                    byte guessedTarget = reader.ReadByte();
                    byte guessedRoleId = reader.ReadByte();
                    RPCProcedure.guesserShoot(killerId, dyingTarget, guessedTarget, guessedRoleId);
                    break;
                case (byte)CustomRPC.VultureWin:
                    RPCProcedure.vultureWin();
                    break;
                case (byte)CustomRPC.LawyerSetTarget:
                    RPCProcedure.lawyerSetTarget(reader.ReadByte()); 
                    break;
                case (byte)CustomRPC.LawyerPromotesToPursuer:
                    RPCProcedure.lawyerPromotesToPursuer();
                    break;
                case (byte)CustomRPC.SetBlanked:
                    var pid = reader.ReadByte();
                    var blankedValue = reader.ReadByte();
                    RPCProcedure.setBlanked(pid, blankedValue);
                    break;
                case (byte)CustomRPC.SetFutureSpelled:
                    RPCProcedure.setFutureSpelled(reader.ReadByte());
                    break;
                case (byte)CustomRPC.Bloody:
                    byte bloodyKiller = reader.ReadByte();
                    byte bloodyDead = reader.ReadByte();
                    RPCProcedure.bloody(bloodyKiller, bloodyDead);
                    break;
                case (byte)CustomRPC.SetFirstKill:
                    byte firstKill = reader.ReadByte();
                    RPCProcedure.setFirstKill(firstKill);
                    break;
                case (byte)CustomRPC.SetTiebreak:
                    RPCProcedure.setTiebreak();
                    break;
                case (byte)CustomRPC.SetInvisible:
                    byte invisiblePlayer = reader.ReadByte();
                    byte invisibleFlag = reader.ReadByte();
                    RPCProcedure.setInvisible(invisiblePlayer, invisibleFlag);
                    break;            
                case (byte)CustomRPC.YasunaSpecialVote:
                    byte id = reader.ReadByte();
                    byte targetId = reader.ReadByte();
                    RPCProcedure.yasunaSpecialVote(id, targetId);
                    if (AmongUsClient.Instance.AmHost && Yasuna.isYasuna(id)) {
                        int clientId = Helpers.GetClientId(Yasuna.yasuna);
                        MessageWriter writer = AmongUsClient.Instance.StartRpcImmediately(CachedPlayer.LocalPlayer.PlayerControl.NetId, (byte)CustomRPC.YasunaSpecialVote_DoCastVote, Hazel.SendOption.Reliable, clientId);
                        AmongUsClient.Instance.FinishRpcImmediately(writer);
                    }
                    break;
                case (byte)CustomRPC.YasunaSpecialVote_DoCastVote:
                    RPCProcedure.yasunaSpecialVote_DoCastVote();
                    break;
                case (byte)CustomRPC.TaskMasterSetExTasks:
                    playerId = reader.ReadByte();
                    byte oldTaskMasterPlayerId = reader.ReadByte();
                    taskTypeIds = reader.BytesRemaining > 0 ? reader.ReadBytes(reader.BytesRemaining) : null;
                    RPCProcedure.taskMasterSetExTasks(playerId, oldTaskMasterPlayerId, taskTypeIds);
                    break;
                case (byte)CustomRPC.TaskMasterUpdateExTasks:
                    byte clearExTasks = reader.ReadByte();
                    byte allExTasks = reader.ReadByte();
                    RPCProcedure.taskMasterUpdateExTasks(clearExTasks, allExTasks);
                    break;
                case (byte)CustomRPC.DoorHackerDone:
                    playerId = reader.ReadByte();
                    RPCProcedure.doorHackerDone(playerId);
                    break;
                case (byte)CustomRPC.KataomoiSetTarget:
                    playerId = reader.ReadByte();
                    RPCProcedure.kataomoiSetTarget(playerId);
                    break;
                case (byte)CustomRPC.KataomoiWin:
                    RPCProcedure.kataomoiWin();
                    break;
                case (byte)CustomRPC.KataomoiStalking:
                    playerId = reader.ReadByte();
                    RPCProcedure.kataomoiStalking(playerId);
                    break;
                case (byte)CustomRPC.Synchronize:
                    RPCProcedure.synchronize(reader.ReadByte(), reader.ReadInt32());
                    break;
                case (byte)CustomRPC.KillerCreatorCreatesMadmateKiller:
                    RPCProcedure.killerCreatorCreatesMadmateKiller(reader.ReadByte());
                    break;
                case (byte)CustomRPC.MadmateKillerPromotes:
                    RPCProcedure.madmateKillerPromotes();
                    break;
                case (byte)CustomRPC.ThiefStealsRole:
                    byte thiefTargetId = reader.ReadByte();
                    RPCProcedure.thiefStealsRole(thiefTargetId);
                    break;
                case (byte)CustomRPC.SetTrap:
                    RPCProcedure.setTrap(reader.ReadBytesAndSize());
                    break;
                case (byte)CustomRPC.TriggerTrap:
                    byte trappedPlayer = reader.ReadByte();
                    byte trapId = reader.ReadByte();
                    RPCProcedure.triggerTrap(trappedPlayer, trapId);
                    break;
                case (byte)CustomRPC.ShareGamemode:
                    byte gm = reader.ReadByte();
                    RPCProcedure.shareGamemode(gm);
                    break;

                // Game mode

                case (byte)CustomRPC.SetGuesserGm:
                    byte guesserGm = reader.ReadByte();
                    RPCProcedure.setGuesserGm(guesserGm);
                    break;
                case (byte)CustomRPC.ShareTimer:
                    float punish = reader.ReadSingle();
                    RPCProcedure.shareTimer(punish);
                    break;
                case (byte)CustomRPC.HuntedShield:
                    byte huntedPlayer = reader.ReadByte();
                    RPCProcedure.huntedShield(huntedPlayer);
                    break;
                case (byte)CustomRPC.HuntedRewindTime:
                    byte rewindPlayer = reader.ReadByte();
                    RPCProcedure.huntedRewindTime(rewindPlayer);
                    break;
            }
        }
    }
}
