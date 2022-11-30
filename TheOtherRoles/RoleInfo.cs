using System.Linq;
using System;
using System.Collections.Generic;
using TheOtherRoles.Players;
using static TheOtherRoles.TheOtherRoles;
using UnityEngine;
using TheOtherRoles.Utilities;
using TheOtherRoles.CustomGameModes;

namespace TheOtherRoles
{
    class RoleInfo {
        public Color color;
        public string name;
        public string introDescription;
        public string shortDescription;
        public RoleId roleId;
        public bool isNeutral;
        public bool isModifier;

        RoleInfo(string name, Color color, string introDescription, string shortDescription, RoleId roleId, bool isNeutral = false, bool isModifier = false) {
            this.color = color;
            this.name = name;
            this.introDescription = introDescription;
            this.shortDescription = shortDescription;
            this.roleId = roleId;
            this.isNeutral = isNeutral;
            this.isModifier = isModifier;
        }

        public static RoleInfo jester = new RoleInfo("小丑", Jester.color, "Get voted out", "Get voted out", RoleId.Jester, true);
        public static RoleInfo mayor = new RoleInfo("市长", Mayor.color, "Your vote counts twice", "Your vote counts twice", RoleId.Mayor);
        public static RoleInfo portalmaker = new RoleInfo("传送门制造师", Portalmaker.color, "You can create portals", "You can create portals", RoleId.Portalmaker);
        public static RoleInfo engineer = new RoleInfo("工程师",  Engineer.color, "Maintain important systems on the ship", "Repair the ship", RoleId.Engineer);
        public static RoleInfo sheriff = new RoleInfo("警长", Sheriff.color, "Shoot the <color=#FF1919FF>Impostors</color>", "Shoot the Impostors", RoleId.Sheriff);
        public static RoleInfo deputy = new RoleInfo("捕快", Sheriff.color, "Handcuff the <color=#FF1919FF>Impostors</color>", "Handcuff the Impostors", RoleId.Deputy);
        public static RoleInfo lighter = new RoleInfo("小灯人", Lighter.color, "Your light never goes out", "Your light never goes out", RoleId.Lighter);
        public static RoleInfo godfather = new RoleInfo("教父", Godfather.color, "Kill all Crewmates", "Kill all Crewmates", RoleId.Godfather);
        public static RoleInfo mafioso = new RoleInfo("黑手党小弟", Mafioso.color, "Work with the <color=#FF1919FF>Mafia</color> to kill the Crewmates", "Kill all Crewmates", RoleId.Mafioso);
        public static RoleInfo janitor = new RoleInfo("清洁工", Janitor.color, "Work with the <color=#FF1919FF>Mafia</color> by hiding dead bodies", "Hide dead bodies", RoleId.Janitor);
        public static RoleInfo morphling = new RoleInfo("化形者", Morphling.color, "Change your look to not get caught", "Change your look", RoleId.Morphling);
        public static RoleInfo camouflager = new RoleInfo("伪装师", Camouflager.color, "Camouflage and kill the Crewmates", "Hide among others", RoleId.Camouflager);
        public static RoleInfo evilHacker = new RoleInfo("邪恶黑客", EvilHacker.color, "Hack systems and kill the Crewmates", "Hack to kill the Crewmates", RoleId.EvilHacker);
        public static RoleInfo vampire = new RoleInfo("吸血鬼", Vampire.color, "Kill the Crewmates with your bites", "Bite your enemies", RoleId.Vampire);
        public static RoleInfo eraser = new RoleInfo("抹除者", Eraser.color, "Kill the Crewmates and erase their roles", "Erase the roles of your enemies", RoleId.Eraser);
        public static RoleInfo trickster = new RoleInfo("骗术师", Trickster.color, "Use your jack-in-the-boxes to surprise others", "Surprise your enemies", RoleId.Trickster);
        public static RoleInfo cleaner = new RoleInfo("清理者", Cleaner.color, "Kill everyone and leave no traces", "Clean up dead bodies", RoleId.Cleaner);
        public static RoleInfo warlock = new RoleInfo("术士", Warlock.color, "Curse other players and kill everyone", "Curse and kill everyone", RoleId.Warlock);
        public static RoleInfo bountyHunter = new RoleInfo("赏金猎人", BountyHunter.color, "Hunt your bounty down", "Hunt your bounty down", RoleId.BountyHunter);
        public static RoleInfo detective = new RoleInfo("侦探", Detective.color, "Find the <color=#FF1919FF>Impostors</color> by examining footprints", "Examine footprints", RoleId.Detective);
        public static RoleInfo timeMaster = new RoleInfo("时间之主", TimeMaster.color, "Save yourself with your time shield", "Use your time shield", RoleId.TimeMaster);
        public static RoleInfo medic = new RoleInfo("医生", Medic.color, "Protect someone with your shield", "Protect other players", RoleId.Medic);
        public static RoleInfo swapper = new RoleInfo("换票师", Swapper.color, "Swap votes to exile the <color=#FF1919FF>Impostors</color>", "Swap votes", RoleId.Swapper);
        public static RoleInfo seer = new RoleInfo("灵媒", Seer.color, "You will see players die", "You will see players die", RoleId.Seer);
        public static RoleInfo hacker = new RoleInfo("黑客", Hacker.color, "Hack systems to find the <color=#FF1919FF>Impostors</color>", "Hack to find the Impostors", RoleId.Hacker);
        public static RoleInfo tracker = new RoleInfo("追踪者", Tracker.color, "Track the <color=#FF1919FF>Impostors</color> down", "Track the Impostors down", RoleId.Tracker);
        public static RoleInfo snitch = new RoleInfo("告密者", Snitch.color, "Finish your tasks to find the <color=#FF1919FF>Impostors</color>", "Finish your tasks", RoleId.Snitch);
        public static RoleInfo jackal = new RoleInfo("豺狼", Jackal.color, "Kill all Crewmates and <color=#FF1919FF>Impostors</color> to win", "Kill everyone", RoleId.Jackal, true);
        public static RoleInfo sidekick = new RoleInfo("跟班", Sidekick.color, "Help your Jackal to kill everyone", "Help your Jackal to kill everyone", RoleId.Sidekick, true);
        public static RoleInfo spy = new RoleInfo("间谍", Spy.color, "Confuse the <color=#FF1919FF>Impostors</color>", "Confuse the Impostors", RoleId.Spy);
        public static RoleInfo securityGuard = new RoleInfo("保安", SecurityGuard.color, "Seal vents and place cameras", "Seal vents and place cameras", RoleId.SecurityGuard);
        public static RoleInfo arsonist = new RoleInfo("纵火犯", Arsonist.color, "Let them burn", "Let them burn", RoleId.Arsonist, true);
        public static RoleInfo goodGuesser = new RoleInfo("正义的赌怪", Guesser.color, "Guess and shoot", "Guess and shoot", RoleId.NiceGuesser);
        public static RoleInfo badGuesser = new RoleInfo("邪恶的赌怪", Palette.ImpostorRed, "Guess and shoot", "Guess and shoot", RoleId.EvilGuesser);
        public static RoleInfo vulture = new RoleInfo("秃鹫", Vulture.color, "Eat corpses to win", "Eat dead bodies", RoleId.Vulture, true);
        public static RoleInfo medium = new RoleInfo("通灵师", Medium.color, "Question the souls of the dead to gain informations", "Question the souls", RoleId.Medium);
        public static RoleInfo madmate = new RoleInfo("叛徒", Madmate.color, "Help the <color=#FF1919FF>Impostors</color>", "Help the Impostors", RoleId.Madmate);
        public static RoleInfo trapper = new RoleInfo("陷阱师", Trapper.color, "Place traps to find the Impostors", "Place traps", RoleId.Trapper);
        public static RoleInfo lawyer = new RoleInfo("律师", Lawyer.color, "Defend your client", "Defend your client", RoleId.Lawyer, true);
        public static RoleInfo prosecutor = new RoleInfo("检察官", Lawyer.color, "Vote out your target", "Vote our your target", RoleId.Prosecutor, true);
        public static RoleInfo pursuer = new RoleInfo("追击者", Pursuer.color, "Blank the Impostors", "Blank the Impostors", RoleId.Pursuer);
        public static RoleInfo impostor = new RoleInfo("内鬼", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "Sabotage and kill everyone"), "Sabotage and kill everyone", RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo("船员", Color.white, "Find the Impostors", "Find the Impostors", RoleId.Crewmate);
        public static RoleInfo witch = new RoleInfo("女巫", Witch.color, "Cast a spell upon your foes", "Cast a spell upon your foes", RoleId.Witch);
        public static RoleInfo ninja = new RoleInfo("忍者", Ninja.color, "Surprise and assassinate your foes", "Surprise and assassinate your foes", RoleId.Ninja);
        public static RoleInfo thief = new RoleInfo("小偷", Thief.color, "Steal a killers role by killing them", "Steal a killers role", RoleId.Thief, true);

        public static RoleInfo hunter = new RoleInfo("猎人", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "Seek and kill everyone"), "Seek and kill everyone", RoleId.Impostor);
        public static RoleInfo hunted = new RoleInfo("逃脱者", Color.white, "Hide", "Hide", RoleId.Crewmate);

        public static RoleInfo yasuna = new RoleInfo("亚苏娜", Yasuna.color, "Exile suspicious crewmates.", "Exile suspicious crewmates.", RoleId.Yasuna);
        public static RoleInfo yasunaJr = new RoleInfo("小亚苏娜", YasunaJr.color, "Take away the votes of the suspicious crewmates.", "Take away the votes of the suspicious crewmates.", RoleId.YasunaJr);
        public static RoleInfo evilYasuna = new RoleInfo("邪恶亚苏娜", Palette.ImpostorRed, "Exile smart crewmates.", "Exile smart crewmates.", RoleId.EvilYasuna);
        public static RoleInfo taskMaster = new RoleInfo("任务大师", TaskMaster.color, "Complete all extra tasks to lead\ncrewmate's team to victory.", "Complete all extra tasks to lead\ncrewmate's team to victory.", RoleId.TaskMaster);
        public static RoleInfo doorHacker = new RoleInfo("门客", DoorHacker.color, "Slip through the door and cover your tracks.", "Slip through the door and cover your tracks.", RoleId.DoorHacker);
        public static RoleInfo kataomoi = new RoleInfo("单思者", Kataomoi.color, "Become one with your unrequited love.", "Become one with your unrequited love.", RoleId.Kataomoi, true);
        public static RoleInfo killerCreator = new RoleInfo("杀手教官", KillerCreator.color, "Make a Madmate Killer.", "Make a Madmate Killer.", RoleId.KillerCreator);
        public static RoleInfo madmateKiller = new RoleInfo("疯狂杀手", MadmateKiller.color, "If Impostor dies, kill the Crewmates.", "If Impostor dies, kill the Crewmates.", RoleId.MadmateKiller);

        // Task Vs Mode
        public static RoleInfo taskRacer = new RoleInfo("任务狂", TaskRacer.color, "Finish the task before everyone\nelse and aim for 1st!", "Finish the task before everyone\nelse and aim for 1st!", RoleId.TaskRacer);

        // Modifier
        public static RoleInfo bloody = new RoleInfo("溅血者", Color.yellow, "Your killer leaves a bloody trail", "Your killer leaves a bloody trail", RoleId.Bloody, false, true);
        public static RoleInfo antiTeleport = new RoleInfo("通讯兵", Color.yellow, "You will not get teleported", "You will not get teleported", RoleId.AntiTeleport, false, true);
        public static RoleInfo tiebreaker = new RoleInfo("破局者", Color.yellow, "Your vote break the tie", "Break the tie", RoleId.Tiebreaker, false, true);
        public static RoleInfo bait = new RoleInfo("诱饵", Color.yellow, "Bait your enemies", "Bait your enemies", RoleId.Bait, false, true);
        public static RoleInfo sunglasses = new RoleInfo("患者", Color.yellow, "You got the sunglasses", "Your vision is reduced", RoleId.Sunglasses, false, true);
        public static RoleInfo lover = new RoleInfo("恋人", Lovers.color, $"You are in love", $"You are in love", RoleId.Lover, false, true);
        public static RoleInfo mini = new RoleInfo("小人", Color.yellow, "No one will harm you until you grow up", "No one will harm you", RoleId.Mini, false, true);
        public static RoleInfo vip = new RoleInfo("VIP", Color.yellow, "You are the VIP", "Everyone is notified when you die", RoleId.Vip, false, true);
        public static RoleInfo invert = new RoleInfo("酒鬼", Color.yellow, "Your movement is inverted", "Your movement is inverted", RoleId.Invert, false, true);
        public static RoleInfo chameleon = new RoleInfo("变色龙", Color.yellow, "You're hard to see when not moving", "You're hard to see when not moving", RoleId.Chameleon, false, true);
        public static RoleInfo shifter = new RoleInfo("交换师", Color.yellow, "Shift your role", "Shift your role", RoleId.Shifter, false, true);


        public static List<RoleInfo> allRoleInfos = new List<RoleInfo>() {
            impostor,
            godfather,
            mafioso,
            janitor,
            morphling,
            camouflager,
            evilHacker,
            vampire,
            eraser,
            trickster,
            cleaner,
            warlock,
            bountyHunter,
            witch,
            ninja,
            goodGuesser,
            badGuesser,
            lover,
            jester,
            arsonist,
            jackal,
            sidekick,
            vulture,
            pursuer,
            lawyer,
            thief,
            prosecutor,
            crewmate,
            mayor,
            portalmaker,
            engineer,
            sheriff,
            deputy,
            lighter,
            detective,
            timeMaster,
            medic,
            swapper,
            seer,
            hacker,
            tracker,
            snitch,
            spy,
            securityGuard,
            bait,
            medium,
            trapper,
            madmate,
            bloody,
            antiTeleport,
            tiebreaker,
            sunglasses,
            mini,
            vip,
            invert,
            chameleon,
            shifter,
            yasuna,
            yasunaJr,
            evilYasuna,
            taskMaster,
            doorHacker,
            kataomoi,
            killerCreator,
            madmateKiller,

            // Task Vs Mode
            taskRacer,
        };

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool showModifier = true) {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Modifier
            if (showModifier) {
                // after dead modifier
                if (!CustomOptionHolder.modifiersAreHidden.getBool() || PlayerControl.LocalPlayer.Data.IsDead || AmongUsClient.Instance.GameState == InnerNet.InnerNetClient.GameStates.Ended)
                {
                    if (Bait.bait.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bait);
                    if (Bloody.bloody.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bloody);
                    if (Vip.vip.Any(x => x.PlayerId == p.PlayerId)) infos.Add(vip);
                }
                if (p == Lovers.lover1 || p == Lovers.lover2) infos.Add(lover);
                if (p == Tiebreaker.tiebreaker) infos.Add(tiebreaker);
                if (AntiTeleport.antiTeleport.Any(x => x.PlayerId == p.PlayerId)) infos.Add(antiTeleport);
                if (Sunglasses.sunglasses.Any(x => x.PlayerId == p.PlayerId)) infos.Add(sunglasses);
                if (p == Mini.mini) infos.Add(mini);
                if (Invert.invert.Any(x => x.PlayerId == p.PlayerId)) infos.Add(invert);
                if (Chameleon.chameleon.Any(x => x.PlayerId == p.PlayerId)) infos.Add(chameleon);
                if (p == Shifter.shifter) infos.Add(shifter);
            }

            int count = infos.Count;  // Save count after modifiers are added so that the role count can be checked

            // Special roles
            if (p == Jester.jester) infos.Add(jester);
            if (p == Mayor.mayor) infos.Add(mayor);
            if (p == Portalmaker.portalmaker) infos.Add(portalmaker);
            if (p == Engineer.engineer) infos.Add(engineer);
            if (p == Sheriff.sheriff || p == Sheriff.formerSheriff) infos.Add(sheriff);
            if (p == Deputy.deputy) infos.Add(deputy);
            if (p == Lighter.lighter) infos.Add(lighter);
            if (p == Godfather.godfather) infos.Add(godfather);
            if (p == Mafioso.mafioso) infos.Add(mafioso);
            if (p == Janitor.janitor) infos.Add(janitor);
            if (p == Morphling.morphling) infos.Add(morphling);
            if (p == Camouflager.camouflager) infos.Add(camouflager);
            if (p == EvilHacker.evilHacker) infos.Add(evilHacker);
            if (p == Vampire.vampire) infos.Add(vampire);
            if (p == Eraser.eraser) infos.Add(eraser);
            if (p == Trickster.trickster) infos.Add(trickster);
            if (p == Cleaner.cleaner) infos.Add(cleaner);
            if (p == Warlock.warlock) infos.Add(warlock);
            if (p == Witch.witch) infos.Add(witch);
            if (p == Ninja.ninja) infos.Add(ninja);
            if (p == Detective.detective) infos.Add(detective);
            if (p == TimeMaster.timeMaster) infos.Add(timeMaster);
            if (p == Medic.medic) infos.Add(medic);
            if (p == Swapper.swapper) infos.Add(swapper);
            if (p == Seer.seer) infos.Add(seer);
            if (p == Hacker.hacker) infos.Add(hacker);
            if (p == Tracker.tracker) infos.Add(tracker);
            if (p == Snitch.snitch) infos.Add(snitch);
            if (p == Jackal.jackal || (Jackal.formerJackals != null && Jackal.formerJackals.Any(x => x.PlayerId == p.PlayerId))) infos.Add(jackal);
            if (p == Sidekick.sidekick) infos.Add(sidekick);
            if (p == Spy.spy) infos.Add(spy);
            if (p == SecurityGuard.securityGuard) infos.Add(securityGuard);
            if (p == Arsonist.arsonist) infos.Add(arsonist);
            if (p == Guesser.niceGuesser) infos.Add(goodGuesser);
            if (p == Guesser.evilGuesser) infos.Add(badGuesser);
            if (p == BountyHunter.bountyHunter) infos.Add(bountyHunter);
            if (p == Vulture.vulture) infos.Add(vulture);
            if (p == Medium.medium) infos.Add(medium);
            if (p == Madmate.madmate) infos.Add(madmate);
            if (p == Lawyer.lawyer && !Lawyer.isProsecutor) infos.Add(lawyer);
            if (p == Lawyer.lawyer && Lawyer.isProsecutor) infos.Add(prosecutor);
            if (p == Trapper.trapper) infos.Add(trapper);
            if (p == Pursuer.pursuer) infos.Add(pursuer);
            if (p == Thief.thief) infos.Add(thief);
            if (p == Yasuna.yasuna) infos.Add(p.Data.Role.IsImpostor ? evilYasuna : yasuna);
            if (p == YasunaJr.yasunaJr) infos.Add(yasunaJr);
            if (p == TaskMaster.taskMaster) infos.Add(taskMaster);
            if (p == DoorHacker.doorHacker) infos.Add(doorHacker);
            if (p == Kataomoi.kataomoi) infos.Add(kataomoi);
            if (p == KillerCreator.killerCreator) infos.Add(killerCreator);
            if (p == MadmateKiller.madmateKiller) infos.Add(madmateKiller);

            // Task Vs Mode
            if (TaskRacer.isTaskRacer(p))
                infos.Add(taskRacer);

            // Default roles (just impostor, just crewmate, or hunter / hunted for hide n seek
            if (infos.Count == count)
            {
                if (p.Data.Role.IsImpostor)
                    infos.Add(MapOptions.gameMode == CustomGamemodes.HideNSeek ? RoleInfo.hunter : RoleInfo.impostor);
                else
                    infos.Add(MapOptions.gameMode == CustomGamemodes.HideNSeek ? RoleInfo.hunted : RoleInfo.crewmate);
            }

            return infos;
        }

        public static String GetRolesString(PlayerControl p, bool useColors, bool showModifier, bool isDead) {
            string roleName;

            // Task Vs Mode
            if (TaskRacer.isValid()) {
                roleName = TaskRacer.getRankText(TaskRacer.getRank(p), useColors);
            } else {
                var roleList = getRoleInfoForPlayer(p, showModifier);
                if (roleList.Count > 0 && roleList[0].roleId == RoleId.TaskMaster && !isDead && TaskMaster.becomeATaskMasterWhenCompleteAllTasks && !TaskMaster.isTaskComplete)
                    roleList[0] = RoleInfo.crewmate;

                roleName = String.Join(" ", roleList.Select(x => useColors ? Helpers.cs(x.color, x.name) : x.name).ToArray());
                if (Lawyer.target != null && p.PlayerId == Lawyer.target.PlayerId && CachedPlayer.LocalPlayer.PlayerControl != Lawyer.target) roleName += (useColors ? Helpers.cs(Pursuer.color, " §") : " §");
                if (HandleGuesser.isGuesserGm && HandleGuesser.isGuesser(p.PlayerId)) roleName += " (Guesser)";
            }

            return roleName;
        }
    }
}
