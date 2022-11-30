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

        public static RoleInfo jester = new RoleInfo("小丑", Jester.color, "被放逐出去", "被放逐出去", RoleId.Jester, true);
        public static RoleInfo mayor = new RoleInfo("市长", Mayor.color, "你威望更大，票也更多", "你威望更大票也更多", RoleId.Mayor);
        public static RoleInfo portalmaker = new RoleInfo("传送门制造师", Portalmaker.color, "你能制作传送门", "你能制作传送门", RoleId.Portalmaker);
        public static RoleInfo engineer = new RoleInfo("工程师",  Engineer.color, "维护飞船上的设备", "修理飞船破坏", RoleId.Engineer);
        public static RoleInfo sheriff = new RoleInfo("警长", Sheriff.color, "执法<color=#FF1919FF>内鬼</color>", "执法内鬼", RoleId.Sheriff);
        public static RoleInfo deputy = new RoleInfo("捕快", Sheriff.color, "给<color=#FF1919FF>内鬼</color>戴上一对白色小手镯", "给内鬼戴上手铐", RoleId.Deputy);
        public static RoleInfo lighter = new RoleInfo("小灯人", Lighter.color, "你照亮了黑暗", "你照亮了黑暗", RoleId.Lighter);
        public static RoleInfo godfather = new RoleInfo("教父", Godfather.color, "杀死所有船员", "杀死所有船员", RoleId.Godfather);
        public static RoleInfo mafioso = new RoleInfo("黑手党小弟", Mafioso.color, "为<color=#FF1919FF>黑手党</color>工作杀死所有船员", "杀光所有船员", RoleId.Mafioso);
        public static RoleInfo janitor = new RoleInfo("清洁工", Janitor.color, "为<color=#FF1919FF>黑手党</color>工作处理后事", "清理尸体", RoleId.Janitor);
        public static RoleInfo morphling = new RoleInfo("化形者", Morphling.color, "变化成其他人的样貌迷惑他人", "变化成其他人的样貌", RoleId.Morphling);
        public static RoleInfo camouflager = new RoleInfo("伪装师", Camouflager.color, "让玩家们变成小黑人", "隐藏所有玩家", RoleId.Camouflager);
        public static RoleInfo evilHacker = new RoleInfo("邪恶黑客", EvilHacker.color, "黑入系统并击杀船员", "当个黑客并杀光船员", RoleId.EvilHacker);
        public static RoleInfo vampire = new RoleInfo("吸血鬼", Vampire.color, "杀光船员们的血", "吸光敌人的血", RoleId.Vampire);
        public static RoleInfo eraser = new RoleInfo("抹除者", Eraser.color, "杀光船员并抹去他们的职业", "抹去他们的职业", RoleId.Eraser);
        public static RoleInfo trickster = new RoleInfo("骗术师", Trickster.color, "使用你的魔术盒到处移动", "熄灯杀光船员", RoleId.Trickster);
        public static RoleInfo cleaner = new RoleInfo("清理者", Cleaner.color, "杀光船员并不留下痕迹", "清理尸体", RoleId.Cleaner);
        public static RoleInfo warlock = new RoleInfo("术士", Warlock.color, "给每个人下术并杀光", "用术杀光船员", RoleId.Warlock);
        public static RoleInfo bountyHunter = new RoleInfo("赏金猎人", BountyHunter.color, "猎杀你的赏金目标", "猎杀你的赏金目标", RoleId.BountyHunter);
        public static RoleInfo detective = new RoleInfo("侦探", Detective.color, "利用线索找出<color=#FF1919FF>内鬼</color>", "检查留下的脚印", RoleId.Detective);
        public static RoleInfo timeMaster = new RoleInfo("时间之主", TimeMaster.color, "用你的时间盾牌拯救你自己", "使用你的时间之盾", RoleId.TimeMaster);
        public static RoleInfo medic = new RoleInfo("医生", Medic.color, "用你的护盾保护他人", "给其他玩家护盾", RoleId.Medic);
        public static RoleInfo swapper = new RoleInfo("换票师", Swapper.color, "交换玩家票数放逐<color=#FF1919FF>内鬼</color>", "交换票数", RoleId.Swapper);
        public static RoleInfo seer = new RoleInfo("灵媒", Seer.color, "你能看见灵魂与知晓死亡", "你能看见灵魂与知晓死亡", RoleId.Seer);
        public static RoleInfo hacker = new RoleInfo("黑客", Hacker.color, "黑入系统找出<color=#FF1919FF>内鬼</color>", "黑入系统并找出内鬼", RoleId.Hacker);
        public static RoleInfo tracker = new RoleInfo("追踪者", Tracker.color, "追踪<color=#FF1919FF>内鬼</color>", "追踪内鬼", RoleId.Tracker);
        public static RoleInfo snitch = new RoleInfo("告密者", Snitch.color, "完成任务揭露内鬼<color=#FF1919FF>内鬼</color>", "完成你的任务", RoleId.Snitch);
        public static RoleInfo jackal = new RoleInfo("豺狼", Jackal.color, "杀光船员和<color=#FF1919FF>内鬼</color>夺取胜利", "杀光所有人", RoleId.Jackal, true);
        public static RoleInfo sidekick = new RoleInfo("跟班", Sidekick.color, "帮助你的老大夺取胜利", "帮助你的老大杀光所有人", RoleId.Sidekick, true);
        public static RoleInfo spy = new RoleInfo("间谍", Spy.color, "在<color=#FF1919FF>内鬼阵营</color>卧底", "观察找出内鬼", RoleId.Spy);
        public static RoleInfo securityGuard = new RoleInfo("保安", SecurityGuard.color, "封锁管道放监控", "封锁管道放监控", RoleId.SecurityGuard);
        public static RoleInfo arsonist = new RoleInfo("纵火犯", Arsonist.color, "给所有人涂油并点燃", "给所有人涂油并点燃", RoleId.Arsonist, true);
        public static RoleInfo goodGuesser = new RoleInfo("正义的赌怪", Guesser.color, "来一场豪赌吧", "来一场豪赌吧", RoleId.NiceGuesser);
        public static RoleInfo badGuesser = new RoleInfo("邪恶的赌怪", Palette.ImpostorRed, "来一场豪赌吧", "来一场豪赌吧", RoleId.EvilGuesser);
        public static RoleInfo vulture = new RoleInfo("秃鹫", Vulture.color, "吃鸡腿（尸体）吃得饱饱的", "吃鸡腿（尸体）吃得饱饱的", RoleId.Vulture, true);
        public static RoleInfo medium = new RoleInfo("通灵师", Medium.color, "跟灵魂交流获取信息", "跟灵魂交流获取信息", RoleId.Medium);
        public static RoleInfo madmate = new RoleInfo("叛徒", Madmate.color, "帮助<color=#FF1919FF>内鬼</color>", "帮助内鬼", RoleId.Madmate);
        public static RoleInfo trapper = new RoleInfo("陷阱师", Trapper.color, "放置陷阱", "放置陷阱", RoleId.Trapper);
        public static RoleInfo lawyer = new RoleInfo("律师", Lawyer.color, "为你的客户辩护", "为你的客户辩护", RoleId.Lawyer, true);
        public static RoleInfo prosecutor = new RoleInfo("检察官", Lawyer.color, "把你的目标放逐", "把你的目标放逐", RoleId.Prosecutor, true);
        public static RoleInfo pursuer = new RoleInfo("追击者", Pursuer.color, "打击内鬼", "打击内鬼", RoleId.Pursuer);
        public static RoleInfo impostor = new RoleInfo("内鬼", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "破坏并杀光所有人"), "破坏并杀光所有人", RoleId.Impostor);
        public static RoleInfo crewmate = new RoleInfo("船员", Color.white, "找出内鬼", "找出内鬼", RoleId.Crewmate);
        public static RoleInfo witch = new RoleInfo("女巫", Witch.color, "给船员施法诅咒", "给船员施法诅咒", RoleId.Witch);
        public static RoleInfo ninja = new RoleInfo("忍者", Ninja.color, "出其不意，刺杀敌人", "出其不意，刺杀敌人", RoleId.Ninja);
        public static RoleInfo thief = new RoleInfo("小偷", Thief.color, "通过击杀窃取他们的职业", "通过击杀窃取他们的职业", RoleId.Thief, true);

        public static RoleInfo hunter = new RoleInfo("猎人", Palette.ImpostorRed, Helpers.cs(Palette.ImpostorRed, "寻找并杀死所有人"), "寻找并杀死所有人", RoleId.Impostor);
        public static RoleInfo hunted = new RoleInfo("逃脱者", Color.white, "躲藏", "躲藏", RoleId.Crewmate);

        public static RoleInfo yasuna = new RoleInfo("亚苏娜", Yasuna.color, "放逐可疑的船员。", "放逐可疑的船员。", RoleId.Yasuna);
        public static RoleInfo yasunaJr = new RoleInfo("小亚苏娜", YasunaJr.color, "拿走可疑船员的选票。", "拿走可疑船员的选票。", RoleId.YasunaJr);
        public static RoleInfo evilYasuna = new RoleInfo("邪恶亚苏娜", Palette.ImpostorRed, "放逐聪明的船员", "放逐聪明的船员", RoleId.EvilYasuna);
        public static RoleInfo taskMaster = new RoleInfo("任务大师", TaskMaster.color, "完成所有的额外任务/n带领船员的团队取得胜利。", "完成所有的额外任务/n带领船员的团队取得胜利。", RoleId.TaskMaster);
        public static RoleInfo doorHacker = new RoleInfo("门客", DoorHacker.color, "溜过门去，掩盖你的踪迹。", "溜过门去，掩盖你的踪迹。", RoleId.DoorHacker);
        public static RoleInfo kataomoi = new RoleInfo("单思者", Kataomoi.color, "与你的单恋成为一体。", "与你的单恋成为一体。", RoleId.Kataomoi, true);
        public static RoleInfo killerCreator = new RoleInfo("杀手教官", KillerCreator.color, "训练出疯狂杀手", "训练出疯狂杀手", RoleId.KillerCreator);
        public static RoleInfo madmateKiller = new RoleInfo("疯狂杀手", MadmateKiller.color, "如果内鬼没了就杀了船员", "如果内鬼没了就杀了船员", RoleId.MadmateKiller);

        // Task Vs Mode
        public static RoleInfo taskRacer = new RoleInfo("任务狂", TaskRacer.color, "在每个人之前完成任务\n并以第一为目标!", "在每个人之前完成任务\n并以第一为目标!", RoleId.TaskRacer);

        // Modifier
        public static RoleInfo bloody = new RoleInfo("溅血者", Color.yellow, "凶手留下了血腥的痕迹", "凶手留下了血腥的痕迹", RoleId.Bloody, false, true);
        public static RoleInfo antiTeleport = new RoleInfo("通讯兵", Color.yellow, "远程开会", "远程开会", RoleId.AntiTeleport, false, true);
        public static RoleInfo tiebreaker = new RoleInfo("破局者", Color.yellow, "打破平票残局", "打破平票残局", RoleId.Tiebreaker, false, true);
        public static RoleInfo bait = new RoleInfo("诱饵", Color.yellow, "你是诱饵", "你是诱饵", RoleId.Bait, false, true);
        public static RoleInfo sunglasses = new RoleInfo("视觉患者", Color.yellow, "你得到了太阳镜", "你得到了太阳镜", RoleId.Sunglasses, false, true);
        public static RoleInfo lover = new RoleInfo("恋人", Lovers.color, $"你恋爱了", $"你恋爱了", RoleId.Lover, false, true);
        public static RoleInfo mini = new RoleInfo("小人", Color.yellow, "在你长大之前没人能伤害你", "没有人能伤害你", RoleId.Mini, false, true);
        public static RoleInfo vip = new RoleInfo("VIP", Color.yellow, "你是VIP", "每个人都知道你的死讯", RoleId.Vip, false, true);
        public static RoleInfo invert = new RoleInfo("酒鬼", Color.yellow, "你喝醉了晕头转向", "你迷失了方向", RoleId.Invert, false, true);
        public static RoleInfo chameleon = new RoleInfo("变色龙", Color.yellow, "你的身体会因为环境而变色隐身", "你的身体会因为环境而变色隐身", RoleId.Chameleon, false, true);
        public static RoleInfo shifter = new RoleInfo("交换师", Color.yellow, "与他人交换职业", "与他人交换职业", RoleId.Shifter, false, true);


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
