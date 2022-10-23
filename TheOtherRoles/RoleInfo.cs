using System.Linq;
using System;
using System.Collections.Generic;
using TheOtherRoles.Players;
using static TheOtherRoles.TheOtherRoles;
using UnityEngine;

namespace TheOtherRoles
{
    public class RoleInfo {
        public Color color;
        public virtual string name { get { return ModTranslation.getString(nameKey+"name"); } }
        public virtual string introDescription { get { return ModTranslation.getString(nameKey + "introDescription"); } }
        public virtual string shortDescription { get { return ModTranslation.getString(nameKey + "shortdescription"); } }
        public string nameKey;
        public RoleId roleId;
        public bool isNeutral;
        public bool isModifier;
        public static RoleInfo jester;
        public static RoleInfo mayor;
        public static RoleInfo portalmaker;
        public static RoleInfo engineer;
        public static RoleInfo sheriff;
        public static RoleInfo deputy;
        public static RoleInfo lighter;
        public static RoleInfo godfather;
        public static RoleInfo mafioso;
        public static RoleInfo janitor;
        public static RoleInfo morphling;
        public static RoleInfo camouflager;
        public static RoleInfo evilhacker;
        public static RoleInfo vampire;
        public static RoleInfo eraser;
        public static RoleInfo trickster;
        public static RoleInfo cleaner;
        public static RoleInfo warlock;
        public static RoleInfo bountyhunter;
        public static RoleInfo detective;
        public static RoleInfo timemaster;
        public static RoleInfo medic;
        public static RoleInfo shifter;
        public static RoleInfo swapper;
        public static RoleInfo seer;
        public static RoleInfo hacker;
        public static RoleInfo tracker;
        public static RoleInfo snitch;
        public static RoleInfo jackal;
        public static RoleInfo sidekick;
        public static RoleInfo spy;
        public static RoleInfo securityguard;
        public static RoleInfo arsonist;
        public static RoleInfo niceguesser;
        public static RoleInfo evilguesser;
        public static RoleInfo vulture;
        public static RoleInfo medium;
        public static RoleInfo madmate;
        public static RoleInfo lawyer;
        public static RoleInfo pursuer;
        public static RoleInfo impostor;
        public static RoleInfo crewmate;
        public static RoleInfo witch;
        public static RoleInfo ninja;
        public static RoleInfo yasuna;
        public static RoleInfo evilyasuna;
        public static RoleInfo taskmaster;
        public static RoleInfo doorhacker;
        public static RoleInfo kataomoi;
        public static RoleInfo killercreator;
        public static RoleInfo madmatekiller;
        public static RoleInfo taskracer;
        public static RoleInfo bloody;
        public static RoleInfo antitp;
        public static RoleInfo tiebreaker;
        public static RoleInfo bait;
        public static RoleInfo sunglasses;
        public static RoleInfo lover;
        public static RoleInfo mini;
        public static RoleInfo vip;
        public static RoleInfo invert;
        public static List<RoleInfo> allRoleInfos;

        RoleInfo(string name, Color color, RoleId roleId, bool isNeutral = false, bool isModifier = false) {
            this.color = color;
            this.nameKey = name;
            this.roleId = roleId;
            this.isNeutral = isNeutral;
            this.isModifier = isModifier;
        }

        public static void Init() {
        jester = new RoleInfo("jester", Jester.color, RoleId.Jester, true);
        mayor = new RoleInfo("mayor", Mayor.color, RoleId.Mayor);
        portalmaker = new RoleInfo("portalmaker", Portalmaker.color, RoleId.Portalmaker);
        engineer = new RoleInfo("engineer", Engineer.color, RoleId.Engineer);
        sheriff = new RoleInfo("sheriff", Sheriff.color,  RoleId.Sheriff);
        deputy = new RoleInfo("deputy", Sheriff.color, RoleId.Deputy);
        lighter = new RoleInfo("lighter", Lighter.color, RoleId.Lighter);
        godfather = new RoleInfo("godfather", Godfather.color, RoleId.Godfather);
        mafioso = new RoleInfo("mafioso", Mafioso.color, RoleId.Mafioso);
        janitor = new RoleInfo("janitor", Janitor.color, RoleId.Janitor);
        morphling = new RoleInfo("morphling", Morphling.color, RoleId.Morphling);
        camouflager = new RoleInfo("camouflager", Camouflager.color, RoleId.Camouflager);
        evilhacker = new RoleInfo("evilHacker", EvilHacker.color, RoleId.EvilHacker);
        vampire = new RoleInfo("vampire", Vampire.color,  RoleId.Vampire);
        eraser = new RoleInfo("eraser", Eraser.color,RoleId.Eraser);
        trickster = new RoleInfo("trickster", Trickster.color,  RoleId.Trickster);
        cleaner = new RoleInfo("cleaner", Cleaner.color,  RoleId.Cleaner);
        warlock = new RoleInfo("warlock", Warlock.color, RoleId.Warlock);
        bountyhunter = new RoleInfo("bountyhunter", BountyHunter.color,RoleId.BountyHunter);
        detective = new RoleInfo("detective", Detective.color, RoleId.Detective);
        timemaster = new RoleInfo("timemaster", TimeMaster.color,RoleId.TimeMaster);
        medic = new RoleInfo("medic", Medic.color,  RoleId.Medic);
        shifter = new RoleInfo("shifter", Shifter.color, RoleId.Shifter);
        swapper = new RoleInfo("swapper", Swapper.color, RoleId.Swapper);
        seer = new RoleInfo("seer", Seer.color, RoleId.Seer);
        hacker = new RoleInfo("hacker", Hacker.color,  RoleId.Hacker);
        tracker = new RoleInfo("tracker", Tracker.color,RoleId.Tracker);
        snitch = new RoleInfo("snitch", Snitch.color, RoleId.Snitch);
        jackal = new RoleInfo("jackal", Jackal.color,RoleId.Jackal, true);
        sidekick = new RoleInfo("sidekick", Sidekick.color, RoleId.Sidekick, true);
        spy = new RoleInfo("spy", Spy.color, RoleId.Spy);
        securityguard = new RoleInfo("securityguard", SecurityGuard.color,  RoleId.SecurityGuard);
        arsonist = new RoleInfo("arsonist", Arsonist.color,RoleId.Arsonist, true);
        niceguesser = new RoleInfo("niceguesser", Guesser.color, RoleId.NiceGuesser);
        evilguesser = new RoleInfo("evilguesser", Palette.ImpostorRed, RoleId.EvilGuesser);
        vulture = new RoleInfo("vulture", Vulture.color, RoleId.Vulture, true);
        medium = new RoleInfo("medium", Medium.color,  RoleId.Medium);
        madmate = new RoleInfo("madmate", Madmate.color,RoleId.Madmate);
        lawyer = new RoleInfo("lawyer", Lawyer.color, RoleId.Lawyer, true);
        pursuer = new RoleInfo("pursuer", Pursuer.color, RoleId.Pursuer);
        impostor = new RoleInfo("impostor", Palette.ImpostorRed,RoleId.Impostor);
        crewmate = new RoleInfo("crewmate", Color.white, RoleId.Crewmate);
        witch = new RoleInfo("witch", Witch.color,  RoleId.Witch);
        ninja = new RoleInfo("ninja", Ninja.color, RoleId.Ninja);
        yasuna = new RoleInfo("yasuna", Yasuna.color, RoleId.Yasuna);
        evilyasuna = new RoleInfo("evilyasuna", Palette.ImpostorRed,  RoleId.EvilYasuna);
        taskmaster = new RoleInfo("taskmaster", TaskMaster.color, RoleId.TaskMaster);
        doorhacker = new RoleInfo("doorhacker", DoorHacker.color, RoleId.DoorHacker);
        kataomoi = new RoleInfo("kataomoi", Kataomoi.color, RoleId.Kataomoi, true);
        killercreator = new RoleInfo("killercreator", KillerCreator.color,  RoleId.KillerCreator);
        madmatekiller = new RoleInfo("madmatekiller", MadmateKiller.color, RoleId.MadmateKiller);

        // Task Vs Mode
        taskracer = new RoleInfo("taskracer", TaskRacer.color,  RoleId.TaskRacer);

        // Modifier
        bloody = new RoleInfo("bloody", Color.yellow,RoleId.Bloody, false, true);
        antitp = new RoleInfo("antitp", Color.yellow,RoleId.AntiTeleport, false, true);
        tiebreaker = new RoleInfo("tiebreaker", Color.yellow,  RoleId.Tiebreaker, false, true);
        bait = new RoleInfo("bait", Color.yellow, RoleId.Bait, false, true);
        sunglasses = new RoleInfo("sunglasses", Color.yellow, RoleId.Sunglasses, false, true);
        lover = new RoleInfo("lover", Lovers.color, RoleId.Lover, false, true);
        mini = new RoleInfo("mini", Color.yellow,  RoleId.Mini, false, true);
        vip = new RoleInfo("vIP", Color.yellow, RoleId.Vip, false, true);
        invert = new RoleInfo("invert", Color.yellow, RoleId.Invert, false, true);

            allRoleInfos = new List<RoleInfo>()
            {
            impostor,
            godfather,
            mafioso,
            janitor,
            morphling,
            camouflager,
            evilhacker,
            vampire,
            eraser,
            trickster,
            cleaner,
            warlock,
            bountyhunter,
            witch,
            ninja,
            niceguesser,
            evilguesser,
            lover,
            jester,
            arsonist,
            jackal,
            sidekick,
            vulture,
            pursuer,
            lawyer,
            crewmate,
            shifter,
            mayor,
            portalmaker,
            engineer,
            sheriff,
            deputy,
            lighter,
            detective,
            timemaster,
            medic,
            swapper,
            seer,
            hacker,
            tracker,
            snitch,
            spy,
            securityguard,
            bait,
            medium,
            madmate,
            bloody,
            antitp,
            tiebreaker,
            sunglasses,
            mini,
            vip,
            invert,
            yasuna,
            evilyasuna,
            taskmaster,
            doorhacker,
            kataomoi,
            killercreator,
            madmatekiller,

            // Task Vs Mode
            taskracer,
            };
        }

        public static List<RoleInfo> getRoleInfoForPlayer(PlayerControl p, bool showModifier = true) {
            List<RoleInfo> infos = new List<RoleInfo>();
            if (p == null) return infos;

            // Modifier
            if (showModifier) {
                // after dead modifier
                if (!CustomOptionHolder.modifiersAreHidden.getBool() || PlayerControl.LocalPlayer.Data.IsDead)
                {
                    if (Bait.bait.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bait);
                    if (Bloody.bloody.Any(x => x.PlayerId == p.PlayerId)) infos.Add(bloody);
                    if (Vip.vip.Any(x => x.PlayerId == p.PlayerId)) infos.Add(vip);
                }
                if (p == Lovers.lover1 || p == Lovers.lover2) infos.Add(lover);
                if (p == Tiebreaker.tiebreaker) infos.Add(tiebreaker);
                if (AntiTeleport.antiTeleport.Any(x => x.PlayerId == p.PlayerId)) infos.Add(antitp);
                if (Sunglasses.sunglasses.Any(x => x.PlayerId == p.PlayerId)) infos.Add(sunglasses);
                if (p == Mini.mini) infos.Add(mini);
                if (Invert.invert.Any(x => x.PlayerId == p.PlayerId)) infos.Add(invert);
            }

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
            if (p == EvilHacker.evilHacker) infos.Add(evilhacker);
            if (p == Vampire.vampire) infos.Add(vampire);
            if (p == Eraser.eraser) infos.Add(eraser);
            if (p == Trickster.trickster) infos.Add(trickster);
            if (p == Cleaner.cleaner) infos.Add(cleaner);
            if (p == Warlock.warlock) infos.Add(warlock);
            if (p == Witch.witch) infos.Add(witch);
            if (p == Ninja.ninja) infos.Add(ninja);
            if (p == Detective.detective) infos.Add(detective);
            if (p == TimeMaster.timeMaster) infos.Add(timemaster);
            if (p == Medic.medic) infos.Add(medic);
            if (p == Shifter.shifter) infos.Add(shifter);
            if (p == Swapper.swapper) infos.Add(swapper);
            if (p == Seer.seer) infos.Add(seer);
            if (p == Hacker.hacker) infos.Add(hacker);
            if (p == Tracker.tracker) infos.Add(tracker);
            if (p == Snitch.snitch) infos.Add(snitch);
            if (p == Jackal.jackal || (Jackal.formerJackals != null && Jackal.formerJackals.Any(x => x.PlayerId == p.PlayerId))) infos.Add(jackal);
            if (p == Sidekick.sidekick) infos.Add(sidekick);
            if (p == Spy.spy) infos.Add(spy);
            if (p == SecurityGuard.securityGuard) infos.Add(securityguard);
            if (p == Arsonist.arsonist) infos.Add(arsonist);
            if (p == Guesser.niceGuesser) infos.Add(niceguesser);
            if (p == Guesser.evilGuesser) infos.Add(evilguesser);
            if (p == BountyHunter.bountyHunter) infos.Add(bountyhunter);
            if (p == Vulture.vulture) infos.Add(vulture);
            if (p == Medium.medium) infos.Add(medium);
            if (p == Madmate.madmate) infos.Add(madmate);
            if (p == Lawyer.lawyer) infos.Add(lawyer);
            if (p == Pursuer.pursuer) infos.Add(pursuer);
            if (p == Yasuna.yasuna) infos.Add(p.Data.Role.IsImpostor ? evilyasuna : yasuna);
            if (p == TaskMaster.taskMaster) infos.Add(taskmaster);
            if (p == DoorHacker.doorHacker) infos.Add(doorhacker);
            if (p == Kataomoi.kataomoi) infos.Add(kataomoi);
            if (p == KillerCreator.killerCreator) infos.Add(killercreator);
            if (p == MadmateKiller.madmateKiller) infos.Add(madmatekiller);

            // Task Vs Mode
            if (TaskRacer.isTaskRacer(p))
                infos.Add(taskracer);

            // Default roles
            if (infos.Count == 0 && p.Data.Role.IsImpostor) infos.Add(impostor); // Just Impostor
            if (infos.Count == 0 && !p.Data.Role.IsImpostor) infos.Add(crewmate); // Just Crewmate

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
            }

            return roleName;
        }
    }
}
