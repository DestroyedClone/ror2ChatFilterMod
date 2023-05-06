using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using UnityEngine;
using static ror2ChatFilterMod.Main;

namespace ror2ChatFilterMod
{
    internal class ModCompat
    {
        public static bool IsAnyModLoaded = false;
        public const string FakeUsername1 = "Username1";
        public const string FakeUsername2 = "Username2";
        public const string FakeUsername3 = "Username3";


        public const string KEY_VSTWITCH = "VsTwitch";
        public static bool modloaded_VsTwitch = false;

        //https://github.com/JustDerb/RoR2-VsTwitch/blob/c7a0894e2abe380541b53d55e9a22e3cb043de5a/Events/EventFactory.cs#L221
        public const string mc_VsTwitch_SimpleChat_BaseToken_ChallengeToken = "<color=#9147ff>Twitch Chat feels the boss should be harder on the next stage.</color>";
        public static ConfigEntry<bool> cfgVsTwitch_Challenge;

        //https://github.com/JustDerb/RoR2-VsTwitch/blob/c7a0894e2abe380541b53d55e9a22e3cb043de5a/Events/EventFactory.cs#L338
        //<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>
        public static string mc_VsTwitch_SimpleChat_BaseToken_StartsWith_AllyToken = $"<color=#00FF00>";

        public const string mc_VsTwitch_SimpleChat_BaseToken_EndsWith_AllyToken = "</color> <color=#9147ff> enters the game to help you...</color>";
        public static ConfigEntry<bool> cfgVsTwitch_AllyToken;

        public static string mc_VsTwitch_Ally_Description = $"<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>{FakeUsername2}</color><color=#9147ff> enters the game to help you...</color>";
        //https://github.com/JustDerb/RoR2-VsTwitch/blob/c7a0894e2abe380541b53d55e9a22e3cb043de5a/VsTwitch.cs#L245
        //im lazing a little, seems useful?

        //https://github.com/Mordrog/RoR2-TPVoting/blob/af75740cd894e146672ec493978483e5ebdbf46a/TPVoting/Modules/Helpers/ChatHelper.cs#L15
        //public static bool modloaded_TPVoting;

        //https://github.com/MonsterSkinMan/GOTCE/blob/3e4d99234752df2a8acad71ef376aca02857e3ed/GOTCE/Artifact/ArtifactOfWoolie.cs#L59
        public static bool modloaded_GOTCE;
        public const string KEY_GOTCE = "GOTCE";
        public const string mc_GOTCE_SimpleChat_BaseToken_RushOrDieToken = "<color=#e5eefc>{0}</color>";
        public const string mc_GOTCE_SimpleChat_ParamToken_RushOrDieToken = "You got outscaled, idiot.";
        public static ConfigEntry<bool> cfgGOTCE_RushOrDie;

        //https://github.com/AngeloTadeucci/RoR2_RaidInfo/blob/eeb96b302c3cdd843b427245319d29d68d3becc4/Message.cs#L6
        //https://github.com/harbingerofme/R2DS-Essentials/blob/165184420d5995f3be6f85913251a349e568000a/Modules/MotD.cs#L167

        public static ConfigEntry<bool> cfgUltimateCustomRun_Welcome;

        //https://github.com/KosmosisDire/TeammateRevive/blob/3b8e4e77c8c258ff82fc9372374ec664410332cd/TeammateRevive/Content/Artifact/DeathCurseArtifact.cs#L31
        public static bool modloaded_TeammateRevive;
        public const string KEY_TEAMMATEREVIVE = "TeammateRevive";
        public const string mc_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseDisabledToken = "<color=\"yellow\">Artifact of Death Curse is disabled because run started in single player.</color>";
        public static ConfigEntry<bool> cfgTeammateRevive_DeathCurseDisabled;
        public const string mc_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseEnforcedByServerToken = "<color=\"yellow\">Artifact of Death Curse is enforced by server.</color>";
        public static ConfigEntry<bool> cfgTeammateRevive_DeathCurseEnforcedByServer;

        //https://github.com/ThinkInvis/RoR2-TinkersSatchel/blob/559eae30e461e0ead649e67b3be312a4765dbf56/Items/LunarEqp/Compass.cs#L210
        public static bool modloaded_TinkersSatchel;
        public const string KEY_TINKERSSATCHEL = "TinkersSatchel";
        public const string mc_TinkersSatchel_SubjectFormatChatMessage_BaseToken_Compass = "TKSAT_COMPASS_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgTinkersSatchel_Compass;
        //public const string mc_TinkersSatchel_SubjectChatMessage_BaseToken_MonkeyPawActivate = "TKSAT_MONKEYSPAW_ACTIVATED";
        //public static ConfigEntry<ChatFilterType> cfgTinkersSatchel_MonkeyPawActivate;
        //public const string mc_TinkersSatchel_ColoredTokenChatMessage_BaseToken_MonkeyPawItemGrant = "TKSAT_MONKEYSPAW_ACTIVATED";
        //public static ConfigEntry<bool> cfgTinkersSatchel_MonkeyPawItemGrant;

        //https://github.com/SylmarDev/SpireItems/blob/3a4c8ef16ff58bd5457523d6efb475bddf7fe0d5/SpireItems/Relics/Tier2/BloodIdol.cs#L80
        public static bool modloaded_SpireItems;
        public const string KEY_SPIREITEMS = "SpireItems";
        //for some reason its a subjectformatchatmessage despite not using any subjects???
        public const string mc_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolSingleToken = "<style=cEvent>Your <color=#FFC733>golden idol</color> begins to dull in color and begins bleeding from its eyes. The bleeding never ceases.</style>";

        public const string mc_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolMultipleToken = "<style=cEvent>Your <color=#FFC733>golden idols</color> begin to dull in color and begin bleeding from their eyes. The bleeding never ceases.</style>";
        public static ConfigEntry<bool> cfgSpireItems_BloodIdol;

        //https://github.com/prodzpod/RoR2-BossAntiSoftlock/blob/4273b77ee0105efca8dbd9e936c1137a1ae0d005/BossAntiSoftlock.cs#L68
        public static bool modloaded_BossAntiSoftlock;
        public const string KEY_BOSSANTISOFTLOCK = "BossAntiSoftlock";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_StartsWith_ResetBossPositionsToken = "<color=#93c47d>Boss Anti-Softlock:</color> Resetting monster positions... ";
        public static ConfigEntry<bool> cfgBossAntiSoftlock_ResetBossPosition;
        public const string mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_ErrorResetToken = "<color=#93c47d>Boss Anti-Softlock:</color> Error resetting monster positions; check console for more info!";
        public static ConfigEntry<bool> cfgBossAntiSoftlock_ErrorReset;
        public const string mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_ModHintToken = "<color=#93c47d>Boss Anti-Softlock:</color> Type '/bossreset' to reset monster positions.";
        public static ConfigEntry<bool> cfgBossAntiSoftlock_ModHint;
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset1 = "/bossreset";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset2 = "/boss_reset";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset3 = "/resetboss";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset4 = "/resetbosses";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset5 = "/reset_boss";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset6 = "/reset_bosses";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset7 = "/br";
        public const string mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset8 = "/rb";
        public static ConfigEntry<bool> cfgBossAntiSoftlock_Command;

        //https://github.com/TeamMoonstorm/MoonstormSharedUtils/blob/f6db127b4dc0b4605922fa5343e2f518c5bf3258/Runtime/Code/Classes/EntityStates/EventState.cs#L94
        //???

        //https://github.com/bb010g/wildbook-R2Mods/blob/a95b5a4843f6d5b4efef73db00f5390324dbda6a/Multitudes/Multitudes.cs#L107
        public static bool modloaded_Multitudes;
        public const string KEY_MULTITUDES = "Multitudes";
        public const string mc_Multitudes_SimpleChatMessage_BaseToken_SendMultiplierToken = "Multitudes set to: {0}";
        public static ConfigEntry<bool> cfgMultitudes_SendMultiplier;

        //bettershrinesrewrite
        public static bool modloaded_BetterShrines;
        public const string KEY_BETTERSHRINES = "BetterShrines";
        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineHeresyToken = "SHRINE_HERESY_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgBetterShrines_ShrineHeresy;
        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineDisorderToken = "SHRINE_DISORDER_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgBetterShrines_ShrineDisorder;
        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineFallenToken = "SHRINE_FALLEN_USED";
        public static ConfigEntry<ChatFilterType> cfgBetterShrines_ShrineFallen;
        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineImpToken = "SHRINE_IMP_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgBetterShrines_ShrineImp;
        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispAcceptToken = "SHRINE_WISP_ACCEPT_MESSAGE";
        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispDenyToken = "SHRINE_WISP_DENY_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgBetterShrines_ShrineWisp;
        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineShieldingToken = "SHRINE_SHIELDING_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgBetterShrines_ShrineShielding;

        public const string mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineChancePunishedToken = "SHRINE_CHANCE_PUNISHED_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgBetterShrines_ShrineChancePunished;

        //https://github.com/niwith/DropInMultiplayer/blob/9f31beee9ed8c594c75cdfa4de4c193bcc4adfa7/DropInMultiplayerV2
        public static bool modloaded_DropInMultiplayer;
        public const string KEY_DROPINMULTIPLAYER = "DropInMultiplayer";
        //server defines this
        //we could cache it when it happens but who knows what format it is, thats why we're doing just default token
        public const string mc_DropInMultiplayer_SimpleChatMessage_BaseToken_EndsWith_WelcomeToken = "! Join the game by typing '/join_as {survivor name}' in chat (or '/join_as random'). To get a list of availible survivors, type '/list_survivors' in chat";

        public static ConfigEntry<bool> cfgDropInMultiplayer_Welcome;
        public const string mc_DropInMultiplayer_SimpleChatMessage_BaseToken_MissingCommandToken = "Unable to find command, try /help";
        public static ConfigEntry<bool> cfgDropInMultiplayer_MissingCommand;

        // UNUSED
        //there's some fucking commands here that we have to get
        //https://github.com/niwith/DropInMultiplayer/blob/9f31beee9ed8c594c75cdfa4de4c193bcc4adfa7/DropInMultiplayerV2/Main.cs#L768
        //1. Cache what user most recently said /COMMAND
        //2. Then filter the command based
        //might fuck up
        public static NetworkUser mostRecentNetworkUserChat;

        public const string mc_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_HelpToken = "/HELP";
        public const string mc_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_JoinToken = "/JOIN";
        public const string mc_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_JoinAsToken = "/JOIN_AS";
        public const string mc_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_ListSurvivorsToken = "/LIST_SURVIVORS";
        public const string mc_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_ListBodiesToken = "/LIST_BODIES";
        public static ConfigEntry<bool> cfgDropInMultiplayer_Commands;

        //https://github.com/Ethanol10/darth-vader-ror2/blob/e155c78a6513aae472de1d80825ba934878fb9c8/DarthVaderMod/DarthVaderPlugin.cs#L126
        public static bool modloaded_DarthVader;
        public const string KEY_DARTHVADER = "DarthVader";
        public const string mc_DarthVader_SimpleChatMessage_BaseToken_DeathMessageToken = "There was too much Sand";
        public static ConfigEntry<bool> cfgDarthVader_DeathMessage;

        public static bool modloaded_ShrineOfRepair;
        public const string KEY_SHRINEOFREPAIR = "ShrineOfRepair";
        //https://github.com/viliger2/ShrineOfRepair/blob/1544dc66f9815e7dc446a9335956f5a9ccaf95fa/RoR2_ShrineOfRepair/Modules/Interactables/ShrineOfRepairPurchase.cs#L261
        public const string mc_ShrineOfRepair_SubjectFormatChatMessage_BaseToken_ShrineRepairInteractToken = "INTERACTABLE_SHRINE_REPAIR_INTERACT";

        public static ConfigEntry<ChatFilterType> cfgShrineOfRepair_Interact;

        //https://github.com/viliger2/ShrineOfRepair/blob/1544dc66f9815e7dc446a9335956f5a9ccaf95fa/RoR2_ShrineOfRepair/Modules/Interactables/ShrineOfRepairPicker.cs#L310
        public const string mc_ShrineOfRepair_SubjectFormatChatMessage_BaseToken_ShrineRepairInteractPickerToken = "INTERACTABLE_SHRINE_REPAIR_INTERACT_PICKER";

        public static ConfigEntry<ChatFilterType> cfgShrineOfRepair_InteractPicker;

        //https://github.com/IBurn36360/Refightilization/blob/75043cf8605515e99c58fda145460628401d7535/Refightilization/Refightilization.cs#L137
        //public static bool modloaded_Refightilization;
        //uses language tokens goodly but it's a fucking SimpleChatMessage so there's too many cases I need to check and too many false positives

        public static bool modloaded_WellRoundedBalance;
        public const string KEY_WELLROUNDEDBALANCE = "WellRoundedBalance";
        public const string mc_WellRoundedBalance_SubjectFormatChatMessage_BaseToken_PluriCorruptedToken = "PLURI_CORRUPTED";
        public static ConfigEntry<ChatFilterType> cfgWRB_PluriCorrupted;

        //https://github.com/search?q=repo%3AAwokeinanEnigma%2FCloudburst%20Chat&type=code
        //no point in doing it now

        public static bool modloaded_RiskOfChaos;
        public const string KEY_RISKOFCHAOS = "RiskOfChaos";
        public const string mc_RiskOfChaos_SimpleChatMessage_BaseToken_LoginFailFormatToken = "TWITCH_EFFECT_VOTING_LOGIN_FAIL_FORMAT";
        public static ConfigEntry<bool> cfgRiskOfChaos_LoginFail;

        //public static string mc_RiskOfChaos_SimpleChatMessage_ParamToken_ Language.GetString("TWITCH_LOGIN_FAIL_NOT_LOGGED_IN")
        public const string mc_RiskOfChaos_SimpleChatMessage_BaseToken_ChaosEffectActivateToken = "CHAOS_EFFECT_ACTIVATE";

        public static ConfigEntry<bool> cfgRiskOfChaos_ChaosEffectActivate;

        //https://github.com/FunkFrog/ShareSuite/blob/c97cccdf71f6405b1c914c1ff091104942b6c6cc/ShareSuite/ChatHandler.cs#L52
        public static bool modloaded_ShareSuite;
        public const string KEY_SHARESUITE = "ShareSuite";
        private const string ShareSuite_GrayColor = "7e91af";
        private const string ShareSuite_RedColor = "ed4c40";
        //private const string LinkColor = "5cb1ed";
        //private const string ErrorColor = "ff0000";

        public static string mc_ShareSuite_SimpleChatMessage_BaseToken_NotRepeatedMessageToken = $"<color=#{ShareSuite_GrayColor}>(This message will </color><color=#{ShareSuite_RedColor}>NOT</color>"
                                 + $"<color=#{ShareSuite_GrayColor}> display again!) </color>";

        public static string mc_ShareSuite_SimpleChatMessage_BaseToken_MessageToken = $"<color=#{ShareSuite_GrayColor}>Hey there! Thanks for installing </color>"
                      + $"<color=#{ShareSuite_RedColor}>ShareSuite 2.8</color><color=#{ShareSuite_GrayColor}>!"
                      + " You should now receive logbook updates, and item description popups upon picking up items."
                      + " (You can turn Rich Messages back on now!) This mod is now compatible with Yeet, and"
                      + " some general maintenance has been done to the default blacklists! Have fun!</color>";

        public static string mc_ShareSuite_SimpleChatMessage_BaseToken_ClickChatBoxToken = $"<color=#{ShareSuite_RedColor}>(Click the chat box to view the full message)</color>";
        public static ConfigEntry<bool> cfgShareSuite_NotRepeatedMessage;
        public static ConfigEntry<bool> cfgShareSuite_Message;
        public static ConfigEntry<bool> cfgShareSuite_ClickChatBox;

        //https://github.com/Moffein/BossKillTimer/blob/master/TitanKillTimer/Class1.cs
        public static bool modloaded_BossKillTimer;
        public const string KEY_BOSSKILLTIMER = "BossKillTimer";
        public const string mc_BossKillTimer_SimpleChatMessage_BaseToken_StartsWith_InstantKillToken = "<style=cIsHealing>INSTANT KILL!</style> <style=cIsHealth>";
        public const string mc_BossKillTimer_SimpleChatMessage_BaseToken_StartsWith_KillToken = "<style=cIsHealth>";
        public const string mc_BossKillTimer_SimpleChatMessage_BaseToken_EndsWith_KillToken = "</style> seconds!";
        public static ConfigEntry<bool> cfgBossKillTimer_InstantKill;
        public static ConfigEntry<bool> cfgBossKillTimer_Kill;

        //moff direseeker
        public static bool modloaded_Direseeker;
        public const string KEY_DIRESEEKER = "Direseeker";
        public const string mc_Direseeker_SimpleChatMessage_BaseToken_SpawnWarningToken = "DIRESEEKER_SPAWN_WARNING";
        public const string mc_Direseeker_SimpleChatMessage_BaseToken_SpawnBeginToken = "DIRESEEKER_SPAWN_BEGIN";
        public static ConfigEntry<bool> cfgDireseeker_SpawnWarning;
        public static ConfigEntry<bool> cfgDireseeker_SpawnBegin;

        //https://github.com/6thmoon/MultitudesDifficulty/blob/c3e2dc166a327a39efa9482d9b28191daeb9d90f/Session.cs#L55
        public static bool modloaded_MultitudesDifficulty;
        public const string KEY_MULTITUDESDIFFICULTY = "MultitudesDifficulty";
        public const string mc_MultitudesDifficulty_SimpleChatMessage_BaseToken_EclipseToken = "<color=#6AAA5F>Good luck.</color";

        //the other tokens im just hardcoding
        public const string mc_MultitudesDifficulty_SimpleChatMessage_BaseToken_StartsWith_DescToken = "<style=cStack>>Player Count:</style> ";

        public static ConfigEntry<bool> cfgMultitudesDifficulty_Welcome;

        public static bool modloaded_LostInTransit;
        public const string KEY_LOSTINTRANSIT = "LostInTransit";
        public const string mc_LostInTransit_BodyChatMessage_BaseToken_BossHunterOption1Token = "EQUIPMENT_BOSSHUNTERCONSUMED_CHAT";
        public const string mc_LostInTransit_BodyChatMessage_BaseToken_BossHunterOption2Token = "LIT_EQUIPMENT_BOSSHUNTERCONSUMED_CHAT";
        public static ConfigEntry<ChatFilterType> cfgLostInTransit_BossHunterBeatingEmbryo;
        //checks against bodyObject, is this compatible with ShouldShowClient????????

        public static bool modloaded_vanillaVoid;
        public const string KEY_VANILLAVOID = "VanillaVoid";
        public const string mc_vanillaVoid_SimpleChatMessage_BaseToken_PortalSpawnToken = "<color=#DD7AC6>The rift opens...</color>";
        public static ConfigEntry<bool> cfgvanillaVoid_PortalSpawn;

        public static bool modloaded_MysticsItems;
        public const string KEY_MYSTICSITEMS = "MysticsItems";
        //https://github.com/TheMysticSword/MysticsItems/blob/f7fa9b5bf808290196cb2a4bab6c66b5fbb167f8/Interactables/ShrineLegendary.cs#L222
        public const string mc_MysticsItems_SubjectFormatChatMessage_BaseToken_ShrineLegendaryToken = "MYSTICSITEMS_SHRINE_LEGENDARY_USE_MESSAGE";

        public static ConfigEntry<ChatFilterType> cfgMysticsItems_ShrineLegendary;

        //https://github.com/TheMysticSword/MysticsItems/blob/f7fa9b5bf808290196cb2a4bab6c66b5fbb167f8/Items/Tier3/TreasureMap.cs#L248
        public const string mc_MysticsItems_SimpleChatMessage_BaseToken_TreasureMapToken = "MYSTICSITEMS_TREASUREMAP_WARNING";

        public static ConfigEntry<bool> cfgMysticsItems_TreasureMap;

        public static bool modloaded_SS2U;
        public const string KEY_SS2U = "Starstorm 2 Unofficial";
        //https://github.com/Moffein/Starstorm2Unofficial/blob/3f0d60dd626d8baf06b7b1f0ce25ec7b3a029520/Starstorm%202/Cores/NemesisInvasion/Components/NemesisInvasionManager.cs#L120
        public const string mc_SS2U_SimpleChatMessage_BaseToken_NemesisModeDeactivatedToken = "NEMESIS_MODE_DEACTIVATED";

        public static ConfigEntry<bool> cfgSS2U_NemesisDeactivated;
        public const string mc_SS2U_SimpleChatMessage_BaseToken_NemesisModeActivatedWarningToken = "NEMESIS_MODE_ACTIVE_WARNING";
        public static ConfigEntry<bool> cfgSS2U_NemesisWarning;

        //https://github.com/Moffein/Starstorm2Unofficial/blob/3f0d60dd626d8baf06b7b1f0ce25ec7b3a029520/Starstorm%202/Cores/EventsCore.cs#L216
        public const string mc_SS2U_SimpleChatMessage_BaseToken_StormWarnToken = "<style=cWorldEvent><sprite name=\"CloudRight\">     A storm is approaching...</style>";

        public static ConfigEntry<bool> cfgSS2U_StormWarn;
        public const string mc_SS2U_SimpleChatMessage_BaseToken_StormStartToken = "(Storm started.)";
        public static ConfigEntry<bool> cfgSS2U_StormStart;
        public const string mc_SS2U_SimpleChatMessage_BaseToken_StormEndToken = "(Storm ended.)";
        public static ConfigEntry<bool> cfgSS2U_StormEnd;

        //new Color(0.149f, 0.0039f, 0.2117f)
        //https://github.com/Moffein/Starstorm2Unofficial/blob/3f0d60dd626d8baf06b7b1f0ce25ec7b3a029520/Starstorm%202/Survivors/Nemmando/NemmandoCore.cs#L920
        public const string mc_SS2U_SimpleChatMessage_BaseToken_NemmandoVoidDeathPreventToken = "<color=#26010D>He laughs in the face of the void.</color>";

        public static ConfigEntry<bool> cfgSS2U_NemmandoVoidDeath;
        public const string mc_SS2U_BaseToken_BrotherKillChirr1 = "SS2UBROTHER_KILL_CHIRR1";
        public const string mc_SS2U_BaseToken_BrotherKillChirr2 = "SS2UBROTHER_KILL_CHIRR2";
        public static ConfigEntry<bool> cfgSS2U_BrotherKillChirr;
        public const string mc_SS2U_BaseToken_ChirrBefriendBrother = "SS2UBROTHERHURT_CHIRR_BEFRIEND_1";
        public static ConfigEntry<bool> cfgSS2U_ChirrBefriendBrother;

        //https://github.com/Lodington/Aerolt/blob/cbec82a5ec4032303309d75448f9a3e7036eaa5b/Aerolt/Managers/TeleporterManager.cs#L53
        //no point because you WANT to see these, i guess ill do it if its requested
        //https://github.com/1TeaL/DekuRor2/blob/126005879225711500883a7b7b5c6d77d6765de8/DekuVS/DekuMod/Modules/Controllers/DekuController.cs#L258
        //you kinda need this info
        //https://github.com/Moffein/ItemStats/blob/b4a9ebe5c241fc9b030d57fbb8d377d9f5d19187/ItemStats/ItemStatsPlugin.cs#L98
        //You have to enable a config setting to see that message in the first place

        public static void Initialize(ConfigFile Config)
        {
            SetupConfig(Config);
            CheckLoadedMods();
        }

        public static void CheckLoadedMods()
        {
            static bool IsModLoaded(string key)
            {
                var result = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(key);
                IsAnyModLoaded = IsAnyModLoaded || result;
                return result;
            }
            modloaded_BossAntiSoftlock = IsModLoaded("com.justinderby.bossantisoftlock");

            modloaded_VsTwitch = IsModLoaded("com.justinderby.vstwitch");
            modloaded_GOTCE = IsModLoaded("com.TheBestAssociatedLargelyLudicrousSillyheadGroup.GOTCE");
            modloaded_TeammateRevive = IsModLoaded("KosmosisDire.TeammateRevival");
            modloaded_TinkersSatchel = IsModLoaded("com.ThinkInvisible.TinkersSatchel");
            modloaded_SpireItems = IsModLoaded("SylmarDev.SpireItems");
            modloaded_Multitudes = IsModLoaded("dev.wildbook.multitudes");
            modloaded_BetterShrines = IsModLoaded("com.evaisa.moreshrines");
            modloaded_DropInMultiplayer = IsModLoaded("com.niwith.DropInMultiplayer");
            modloaded_DarthVader = IsModLoaded("com.PopcornFactory.DarthVaderMod");
            modloaded_ShrineOfRepair = IsModLoaded("com.Viliger.ShrineOfRepair");
            modloaded_WellRoundedBalance = IsModLoaded("BALLS.WellRoundedBalance");
            modloaded_RiskOfChaos = IsModLoaded("Gorakh.RiskOfChaos");
            modloaded_ShareSuite = IsModLoaded("com.funkfrog_sipondo.sharesuite");
            modloaded_BossKillTimer = IsModLoaded("com.Moffein.BossKillTimer");
            modloaded_Direseeker = IsModLoaded("com.rob.Direseeker");
            modloaded_MultitudesDifficulty = IsModLoaded("local.difficulty.multitudes");
            modloaded_LostInTransit = IsModLoaded("com.ContactLight.LostInTransit");
            modloaded_vanillaVoid = IsModLoaded("com.Zenithrium.vanillaVoid");
            modloaded_MysticsItems = IsModLoaded("com.themysticsword.mysticsitems");
            modloaded_SS2U = IsModLoaded("com.ChirrLover.Starstorm2Unofficial");

            if (IsModLoaded("com.rune580.riskofoptions"))
            {
                ModCompat_RiskOfOptions();
            }
        }

        public static string GiveDescLineByLine(params string[] lines)
        {
            var result = "";
            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                result += line;
                if (i != lines.Length - 1)
                    result += "\n";
            }
            return result;
        }

        public static void SetupConfig(ConfigFile Config)
        {
            //no modcheck otherwise you'd have to launch with it on to generate and ehhhhh
            cfgSpireItems_BloodIdol = Config.Bind(KEY_SPIREITEMS, "BloodIdol", true, mc_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolSingleToken);
            cfgVsTwitch_Challenge = Config.Bind(KEY_VSTWITCH, "Challenge", true, mc_VsTwitch_SimpleChat_BaseToken_ChallengeToken);
            cfgVsTwitch_AllyToken = Config.Bind(KEY_VSTWITCH, "AllyToken", true, mc_VsTwitch_Ally_Description);
            cfgGOTCE_RushOrDie = Config.Bind(KEY_GOTCE, "Woolies Artifact", true, string.Format(mc_GOTCE_SimpleChat_BaseToken_RushOrDieToken, mc_GOTCE_SimpleChat_ParamToken_RushOrDieToken));
            cfgTeammateRevive_DeathCurseDisabled = Config.Bind(KEY_TEAMMATEREVIVE, "DeathCurseDisabled", true, mc_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseDisabledToken);
            cfgTeammateRevive_DeathCurseEnforcedByServer = Config.Bind(KEY_TEAMMATEREVIVE, "DeathCurseEnforcedByServer", true, mc_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseEnforcedByServerToken);
            cfgTinkersSatchel_Compass = Config.Bind(KEY_TINKERSSATCHEL, KEY_TINKERSSATCHEL, ChatFilterType.All, mc_TinkersSatchel_SubjectFormatChatMessage_BaseToken_Compass);
            //cfgTinkersSatchel_MonkeyPawActivate = Config.Bind(KEY_TINKERSSATCHEL, "MonkeyPawActivate", ChatFilterType.All, mc_TinkersSatchel_SubjectChatMessage_BaseToken_MonkeyPawActivate);
            //cfgTinkersSatchel_MonkeyPawItemGrant = Config.Bind(KEY_TINKERSSATCHEL, "MonkeyPawItemGrant", true, mc_TinkersSatchel_ColoredTokenChatMessage_BaseToken_MonkeyPawItemGrant);
            cfgBossAntiSoftlock_ResetBossPosition = Config.Bind(KEY_BOSSANTISOFTLOCK, "ResetBossPosition", true, mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_StartsWith_ResetBossPositionsToken);
            cfgBossAntiSoftlock_ErrorReset = Config.Bind(KEY_BOSSANTISOFTLOCK, "Error Reset", true, mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_ErrorResetToken);
            cfgBossAntiSoftlock_ModHint = Config.Bind(KEY_BOSSANTISOFTLOCK, "ModHint", true, mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_ModHintToken);
            cfgBossAntiSoftlock_Command = Config.Bind(KEY_BOSSANTISOFTLOCK, "Commands", true,
                GiveDescLineByLine(mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset1,
                mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset2,
                mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset3,
                mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset4,
                mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset5,
                mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset6,
                mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset7,
                mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset8));
            cfgMultitudes_SendMultiplier = Config.Bind(KEY_MULTITUDES, "SendMultiplier", true, mc_Multitudes_SimpleChatMessage_BaseToken_SendMultiplierToken);
            cfgBetterShrines_ShrineHeresy = Config.Bind(KEY_BETTERSHRINES, "ShrineHeresy", ChatFilterType.All, mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineHeresyToken);
            cfgBetterShrines_ShrineDisorder = Config.Bind(KEY_BETTERSHRINES, "ShrineDisorder", ChatFilterType.All, mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineDisorderToken);
            cfgBetterShrines_ShrineFallen = Config.Bind(KEY_BETTERSHRINES, "ShrineFallen", ChatFilterType.All, mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineFallenToken);
            cfgBetterShrines_ShrineImp = Config.Bind(KEY_BETTERSHRINES, "ShrineImp", ChatFilterType.All, mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineImpToken);
            cfgBetterShrines_ShrineWisp = Config.Bind(KEY_BETTERSHRINES, "ShrineWisp", ChatFilterType.All, mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispAcceptToken + "\n" + mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispDenyToken);
            cfgBetterShrines_ShrineShielding = Config.Bind(KEY_BETTERSHRINES, "ShrineShielding", ChatFilterType.All, mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineShieldingToken);
            cfgBetterShrines_ShrineChancePunished = Config.Bind(KEY_BETTERSHRINES, "ShrineChancePunished", ChatFilterType.All, mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineChancePunishedToken);
            cfgDropInMultiplayer_Welcome = Config.Bind(KEY_DROPINMULTIPLAYER, "Welcome", true, mc_DropInMultiplayer_SimpleChatMessage_BaseToken_EndsWith_WelcomeToken);
            cfgDropInMultiplayer_MissingCommand = Config.Bind(KEY_DROPINMULTIPLAYER, "MissingCommand", true, mc_DropInMultiplayer_SimpleChatMessage_BaseToken_MissingCommandToken);
            cfgDarthVader_DeathMessage = Config.Bind(KEY_DARTHVADER, "DeathMessage", true, mc_DarthVader_SimpleChatMessage_BaseToken_DeathMessageToken);
            cfgShrineOfRepair_Interact = Config.Bind(KEY_SHRINEOFREPAIR, "Interact", ChatFilterType.All, mc_ShrineOfRepair_SubjectFormatChatMessage_BaseToken_ShrineRepairInteractToken);
            cfgShrineOfRepair_InteractPicker = Config.Bind(KEY_SHRINEOFREPAIR, "InteractPicker", ChatFilterType.All, mc_ShrineOfRepair_SubjectFormatChatMessage_BaseToken_ShrineRepairInteractPickerToken);
            cfgWRB_PluriCorrupted = Config.Bind(KEY_WELLROUNDEDBALANCE, "PluriCorrupted", ChatFilterType.All, mc_WellRoundedBalance_SubjectFormatChatMessage_BaseToken_PluriCorruptedToken);
            cfgRiskOfChaos_ChaosEffectActivate = Config.Bind(KEY_RISKOFCHAOS, "ChaosEffectActivate", true, mc_RiskOfChaos_SimpleChatMessage_BaseToken_ChaosEffectActivateToken);
            cfgRiskOfChaos_LoginFail = Config.Bind(KEY_RISKOFCHAOS, "LoginFail", true, mc_RiskOfChaos_SimpleChatMessage_BaseToken_LoginFailFormatToken);
            cfgShareSuite_NotRepeatedMessage = Config.Bind(KEY_SHARESUITE, "NotRepeatedMessage", true, mc_ShareSuite_SimpleChatMessage_BaseToken_NotRepeatedMessageToken);
            cfgShareSuite_Message = Config.Bind(KEY_SHARESUITE, "Message", true, mc_ShareSuite_SimpleChatMessage_BaseToken_MessageToken);
            cfgShareSuite_ClickChatBox = Config.Bind(KEY_SHARESUITE, "ClickChatBox", true, mc_ShareSuite_SimpleChatMessage_BaseToken_ClickChatBoxToken);
            cfgBossKillTimer_InstantKill = Config.Bind(KEY_BOSSKILLTIMER, "InstantKill", true);
            cfgBossKillTimer_Kill = Config.Bind(KEY_BOSSKILLTIMER, "Kill", true);
            cfgDireseeker_SpawnWarning = Config.Bind(KEY_DIRESEEKER, "SpawnWarning", true, mc_Direseeker_SimpleChatMessage_BaseToken_SpawnWarningToken);
            cfgDireseeker_SpawnBegin = Config.Bind(KEY_DIRESEEKER, "SpawnBegin", true, mc_Direseeker_SimpleChatMessage_BaseToken_SpawnBeginToken);
            cfgMultitudesDifficulty_Welcome = Config.Bind(KEY_MULTITUDESDIFFICULTY, "Welcome", true);
            cfgLostInTransit_BossHunterBeatingEmbryo = Config.Bind(KEY_LOSTINTRANSIT, "BossHunterBeatingEmbryo", ChatFilterType.All, GiveDescLineByLine(mc_LostInTransit_BodyChatMessage_BaseToken_BossHunterOption1Token, mc_LostInTransit_BodyChatMessage_BaseToken_BossHunterOption2Token));
            cfgvanillaVoid_PortalSpawn = Config.Bind(KEY_VANILLAVOID, "PortalSpawn", true, mc_vanillaVoid_SimpleChatMessage_BaseToken_PortalSpawnToken);
            cfgMysticsItems_ShrineLegendary = Config.Bind(KEY_MYSTICSITEMS, "ShrineLegendary", ChatFilterType.All, mc_MysticsItems_SubjectFormatChatMessage_BaseToken_ShrineLegendaryToken);
            cfgMysticsItems_TreasureMap = Config.Bind(KEY_MYSTICSITEMS, "TreasureMap", true, mc_MysticsItems_SimpleChatMessage_BaseToken_TreasureMapToken);
            cfgSS2U_NemesisDeactivated = Config.Bind(KEY_SS2U, "NemesisDeactivated", true, mc_SS2U_SimpleChatMessage_BaseToken_NemesisModeDeactivatedToken);
            cfgSS2U_NemesisWarning = Config.Bind(KEY_SS2U, "NemesisWarning", true, mc_SS2U_SimpleChatMessage_BaseToken_NemesisModeActivatedWarningToken);
            cfgSS2U_StormWarn = Config.Bind(KEY_SS2U, "StormWarn", true, mc_SS2U_SimpleChatMessage_BaseToken_StormWarnToken);
            cfgSS2U_StormStart = Config.Bind(KEY_SS2U, "StormStart", true, mc_SS2U_SimpleChatMessage_BaseToken_StormStartToken);
            cfgSS2U_StormEnd = Config.Bind(KEY_SS2U, "StormEnd", true, mc_SS2U_SimpleChatMessage_BaseToken_StormEndToken);
            cfgSS2U_NemmandoVoidDeath = Config.Bind(KEY_SS2U, "NemmandoVoidDeathPrevention", true, mc_SS2U_SimpleChatMessage_BaseToken_NemmandoVoidDeathPreventToken);
            cfgSS2U_BrotherKillChirr = Config.Bind(KEY_SS2U, "BrotherKillChirr", true, GiveDescLineByLine(mc_SS2U_BaseToken_BrotherKillChirr1, mc_SS2U_BaseToken_BrotherKillChirr2));
            cfgSS2U_ChirrBefriendBrother = Config.Bind(KEY_SS2U, "ChirrBefriendBrother", true, mc_SS2U_BaseToken_ChirrBefriendBrother);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void ModCompat_RiskOfOptions()
        {
            static void A(ConfigEntry<bool> configEntry, string Category)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(configEntry, new CheckBoxConfig()
                {
                    category = Category
                }));
            }
            static void A2(ConfigEntry<ChatFilterType> entry, string Category)
            {
                ModSettingsManager.AddOption(new ChoiceOption(entry, new ChoiceConfig()
                {
                    category = Category
                }));
            }

            #region Vanilla

            var vanilla = "Vanilla";
            A2(cfgPlayerPickupMessages, vanilla);
            A2(cfgDeathMessages, "Vanilla");
            A2(cfgJoinMessages, "Vanilla");
            A2(cfgLeaveMessages, "Vanilla");
            A(cfgNPCPickupMessages, "Vanilla");
            A2(cfgAhoy, "Vanilla");
            A(cfgNpcMessages, "Vanilla");
            A2(cfgAchievement, "Vanilla");
            A(cfgFamily, "Vanilla");
            A2(cfgTeleporterActivation, "Vanilla");
            A2(cfgSuppressor, "Vanilla");
            A(cfgPortalShopWillOpen, "Vanilla");
            A(cfgPortalGoldshoresWillOpen, "Vanilla");
            A(cfgPortalMSWillOpen, "Vanilla");
            A(cfgPortalShopOpen, "Vanilla");
            A(cfgPortalGoldshoresOpen, "Vanilla");
            A(cfgPortalMSOpen, "Vanilla");
            A(cfgMountainTeleporter, "Vanilla");
            A2(cfgShrineChanceWin, "Vanilla");
            A2(cfgShrineChanceFail, "Vanilla");
            A(cfgSeer, "Vanilla");
            A2(cfgShrineBoss, "Vanilla");
            A2(cfgShrineBlood, "Vanilla");
            A2(cfgShrineRestack, "Vanilla");
            A2(cfgShrineHealing, "Vanilla");
            A2(cfgShrineCombat, "Vanilla");
            A(cfgArenaEnd, "Vanilla");
            A2(cfgPetFrog, "Vanilla");

            #endregion Vanilla



            if (modloaded_BossAntiSoftlock)
            {
                A(cfgBossAntiSoftlock_ResetBossPosition, KEY_BOSSANTISOFTLOCK);
                A(cfgBossAntiSoftlock_ErrorReset, KEY_BOSSANTISOFTLOCK);
                A(cfgBossAntiSoftlock_ModHint, KEY_BOSSANTISOFTLOCK);
                A(cfgBossAntiSoftlock_Command, KEY_BOSSANTISOFTLOCK);
            }
            if (modloaded_SpireItems)
            {
                A(cfgSpireItems_BloodIdol, KEY_SPIREITEMS);
            }
            if (modloaded_VsTwitch)
            {
                A(cfgVsTwitch_Challenge, KEY_VSTWITCH);
                A(cfgVsTwitch_AllyToken, KEY_VSTWITCH);
            }
            if (modloaded_GOTCE)
            {
                A(cfgGOTCE_RushOrDie, KEY_GOTCE);
            }
            if (modloaded_TeammateRevive)
            {
                A(cfgTeammateRevive_DeathCurseDisabled, KEY_TEAMMATEREVIVE);
                A(cfgTeammateRevive_DeathCurseEnforcedByServer, KEY_TEAMMATEREVIVE);
            }
            if (modloaded_TinkersSatchel)
            {
                A2(cfgTinkersSatchel_Compass, KEY_TINKERSSATCHEL);
                //A2(cfgTinkersSatchel_MonkeyPawActivate, KEY_TINKERSSATCHEL);
                //A(cfgTinkersSatchel_MonkeyPawItemGrant, KEY_TINKERSSATCHEL);
            }
            if (modloaded_Multitudes)
            {
                A(cfgMultitudes_SendMultiplier, KEY_MULTITUDES);
            }
            if (modloaded_BetterShrines)
            {
                A2(cfgBetterShrines_ShrineHeresy, KEY_BETTERSHRINES);
                A2(cfgBetterShrines_ShrineDisorder, KEY_BETTERSHRINES);
                A2(cfgBetterShrines_ShrineFallen, KEY_BETTERSHRINES);
                A2(cfgBetterShrines_ShrineImp, KEY_BETTERSHRINES);
                A2(cfgBetterShrines_ShrineWisp, KEY_BETTERSHRINES);
                A2(cfgBetterShrines_ShrineShielding, KEY_BETTERSHRINES);
                A2(cfgBetterShrines_ShrineChancePunished, KEY_BETTERSHRINES);
            }
            if (modloaded_DropInMultiplayer)
            {
                A(cfgDropInMultiplayer_Welcome, KEY_DROPINMULTIPLAYER);
                A(cfgDropInMultiplayer_MissingCommand, KEY_DROPINMULTIPLAYER);
            }
            if (modloaded_DarthVader)
            {
                A(cfgDarthVader_DeathMessage, KEY_DARTHVADER);
            }
            if (modloaded_ShrineOfRepair)
            {
                A2(cfgShrineOfRepair_Interact, KEY_SHRINEOFREPAIR);
                A2(cfgShrineOfRepair_InteractPicker, KEY_SHRINEOFREPAIR);
            }
            if (modloaded_WellRoundedBalance)
            {
                A2(cfgWRB_PluriCorrupted, KEY_WELLROUNDEDBALANCE);
            }
            if (modloaded_RiskOfChaos)
            {
                A(cfgRiskOfChaos_LoginFail, KEY_RISKOFCHAOS);
                A(cfgRiskOfChaos_ChaosEffectActivate, KEY_RISKOFCHAOS);
            }
            if (modloaded_ShareSuite)
            {
                A(cfgShareSuite_NotRepeatedMessage, KEY_SHARESUITE);
                A(cfgShareSuite_Message, KEY_SHARESUITE);
                A(cfgShareSuite_ClickChatBox, KEY_SHARESUITE);
            }
            if (modloaded_BossKillTimer)
            {
                A(cfgBossKillTimer_Kill, KEY_BOSSKILLTIMER);
                A(cfgBossKillTimer_InstantKill, KEY_BOSSKILLTIMER);
            }
            if (modloaded_Direseeker)
            {
                A(cfgDireseeker_SpawnWarning, KEY_DIRESEEKER);
                A(cfgDireseeker_SpawnBegin, KEY_DIRESEEKER);
            }
            if (modloaded_MultitudesDifficulty)
            {
                A(cfgMultitudesDifficulty_Welcome, KEY_MULTITUDESDIFFICULTY);
            }
            if (modloaded_LostInTransit)
            {
                A2(cfgLostInTransit_BossHunterBeatingEmbryo, KEY_LOSTINTRANSIT);
            }
            if (modloaded_vanillaVoid)
            {
                A(cfgvanillaVoid_PortalSpawn, KEY_VANILLAVOID);
            }
            if (modloaded_MysticsItems)
            {
                A2(cfgMysticsItems_ShrineLegendary, KEY_MYSTICSITEMS);
                A(cfgMysticsItems_TreasureMap, KEY_MYSTICSITEMS);
            }
            if (modloaded_SS2U)
            {
                A(cfgSS2U_NemesisDeactivated, KEY_SS2U);
                A(cfgSS2U_NemesisWarning, KEY_SS2U);
                A(cfgSS2U_StormWarn, KEY_SS2U);
                A(cfgSS2U_StormStart, KEY_SS2U);
                A(cfgSS2U_StormEnd, KEY_SS2U);
                A(cfgSS2U_NemmandoVoidDeath, KEY_SS2U);
                A(cfgSS2U_BrotherKillChirr, KEY_SS2U);
                A(cfgSS2U_ChirrBefriendBrother, KEY_SS2U);
            }
        }

        public static bool ModCompatCheck_SimpleChatMessage(Chat.SimpleChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            string baseToken = chatMessage.baseToken;
            if (modloaded_VsTwitch)
            {
                if (baseToken == mc_VsTwitch_SimpleChat_BaseToken_ChallengeToken)
                    return cfgVsTwitch_Challenge.Value;
                else if (baseToken.StartsWith(mc_VsTwitch_SimpleChat_BaseToken_StartsWith_AllyToken)
                    && baseToken.EndsWith(mc_VsTwitch_SimpleChat_BaseToken_EndsWith_AllyToken))
                    return cfgVsTwitch_AllyToken.Value;
            }
            if (modloaded_GOTCE)
            {
                if (baseToken == mc_GOTCE_SimpleChat_BaseToken_RushOrDieToken
                    && chatMessage.paramTokens.Length == 1
                    && chatMessage.paramTokens[0] == mc_GOTCE_SimpleChat_ParamToken_RushOrDieToken)
                    return cfgGOTCE_RushOrDie.Value;
            }
            if (modloaded_TeammateRevive)
            {
                if (baseToken == mc_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseDisabledToken)
                    return cfgTeammateRevive_DeathCurseDisabled.Value;
                else if (baseToken == mc_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseEnforcedByServerToken)
                    return cfgTeammateRevive_DeathCurseEnforcedByServer.Value;
            }
            if (modloaded_BossAntiSoftlock)
            {
                if (baseToken.StartsWith(mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_StartsWith_ResetBossPositionsToken))
                    return cfgBossAntiSoftlock_ResetBossPosition.Value;
                else if (baseToken == mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_ErrorResetToken)
                    return cfgBossAntiSoftlock_ErrorReset.Value;
                else if (baseToken == mc_BossAntiSoftlock_SimpleChatMessage_BaseToken_ModHintToken)
                    return cfgBossAntiSoftlock_ModHint.Value;
            }
            if (modloaded_Multitudes)
            {
                if (baseToken == mc_Multitudes_SimpleChatMessage_BaseToken_SendMultiplierToken)
                    return cfgMultitudes_SendMultiplier.Value;
            }
            if (modloaded_DropInMultiplayer)
            {
                if (baseToken.EndsWith(mc_DropInMultiplayer_SimpleChatMessage_BaseToken_EndsWith_WelcomeToken))
                    return cfgDropInMultiplayer_Welcome.Value;
                else if (baseToken == mc_DropInMultiplayer_SimpleChatMessage_BaseToken_MissingCommandToken)
                    return cfgDropInMultiplayer_MissingCommand.Value;
            }
            if (modloaded_DarthVader)
            {
                if (baseToken == mc_DarthVader_SimpleChatMessage_BaseToken_DeathMessageToken)
                    return cfgDarthVader_DeathMessage.Value;
            }
            if (modloaded_RiskOfChaos)
            {
                if (baseToken == mc_RiskOfChaos_SimpleChatMessage_BaseToken_ChaosEffectActivateToken)
                    return cfgRiskOfChaos_ChaosEffectActivate.Value;
                if (baseToken == mc_RiskOfChaos_SimpleChatMessage_BaseToken_LoginFailFormatToken)
                    return cfgRiskOfChaos_LoginFail.Value;
            }
            if (modloaded_ShareSuite)
            {
                if (baseToken == mc_ShareSuite_SimpleChatMessage_BaseToken_NotRepeatedMessageToken)
                    return cfgShareSuite_NotRepeatedMessage.Value;
                else if (baseToken == mc_ShareSuite_SimpleChatMessage_BaseToken_MessageToken)
                    return cfgShareSuite_Message.Value;
                else if (baseToken == mc_ShareSuite_SimpleChatMessage_BaseToken_ClickChatBoxToken)
                    return cfgShareSuite_ClickChatBox.Value;
            }
            if (modloaded_BossKillTimer)
            {
                if (baseToken.EndsWith(mc_BossKillTimer_SimpleChatMessage_BaseToken_EndsWith_KillToken))
                {
                    if (baseToken.StartsWith(mc_BossKillTimer_SimpleChatMessage_BaseToken_StartsWith_InstantKillToken))
                        return cfgBossKillTimer_InstantKill.Value;
                    else if (baseToken.StartsWith(mc_BossKillTimer_SimpleChatMessage_BaseToken_EndsWith_KillToken))
                        return cfgBossKillTimer_Kill.Value;
                }
            }
            if (modloaded_Direseeker)
            {
                if (baseToken == mc_Direseeker_SimpleChatMessage_BaseToken_SpawnWarningToken)
                    return cfgDireseeker_SpawnWarning.Value;
                else if (baseToken == mc_Direseeker_SimpleChatMessage_BaseToken_SpawnBeginToken)
                    return cfgDireseeker_SpawnBegin.Value;
            }
            if (modloaded_MultitudesDifficulty)
            {
                if (baseToken.StartsWith(mc_MultitudesDifficulty_SimpleChatMessage_BaseToken_StartsWith_DescToken))
                {
                    string pattern = "<style=cStack>";
                    int count = Regex.Matches(baseToken, pattern, RegexOptions.IgnoreCase).Count;
                    if (count == 6)
                    {
                        //this should be good enough but a few more to be EXACTLY sure.
                        if (baseToken.Contains("Player Count:")
                            && baseToken.Contains("Extra Item Rewards:")
                            && baseToken.Contains("Player Income:")
                            && baseToken.Contains("Enemy Bonus Health:")
                            && baseToken.Contains("Teleporter Duration:"))
                            return cfgMultitudesDifficulty_Welcome.Value;
                    }
                }
            }
            if (modloaded_vanillaVoid)
            {
                if (baseToken == mc_vanillaVoid_SimpleChatMessage_BaseToken_PortalSpawnToken)
                {
                    return cfgvanillaVoid_PortalSpawn.Value;
                }
            }
            if (modloaded_MysticsItems)
            {
                if (baseToken == mc_MysticsItems_SimpleChatMessage_BaseToken_TreasureMapToken)
                    return cfgMysticsItems_TreasureMap.Value;
            }
            if (modloaded_SS2U)
            {
                if (baseToken == mc_SS2U_SimpleChatMessage_BaseToken_NemesisModeDeactivatedToken)
                    return cfgSS2U_NemesisDeactivated.Value;
                else if (baseToken == mc_SS2U_SimpleChatMessage_BaseToken_NemesisModeActivatedWarningToken)
                    return cfgSS2U_NemesisWarning.Value;
                else if (baseToken == mc_SS2U_SimpleChatMessage_BaseToken_StormWarnToken)
                    return cfgSS2U_StormWarn.Value;
                else if (baseToken == mc_SS2U_SimpleChatMessage_BaseToken_StormStartToken)
                    return cfgSS2U_StormStart.Value;
                else if (baseToken == mc_SS2U_SimpleChatMessage_BaseToken_StormEndToken)
                    return cfgSS2U_StormEnd.Value;
                else if (baseToken == mc_SS2U_SimpleChatMessage_BaseToken_StormEndToken)
                    return cfgSS2U_NemmandoVoidDeath.Value;
                else if (baseToken.StartsWith("<color=#c6d5ff>Mithrix:") && baseToken.EndsWith("</color>"))
                {
                    if (baseToken.Contains(Language.GetString(mc_SS2U_BaseToken_BrotherKillChirr1))
                        || baseToken.Contains(Language.GetString(mc_SS2U_BaseToken_BrotherKillChirr2)))
                        return cfgSS2U_BrotherKillChirr.Value;
                    else if (baseToken.Contains(Language.GetString(mc_SS2U_BaseToken_ChirrBefriendBrother)))
                        return cfgSS2U_ChirrBefriendBrother.Value;
                }
            }

            return true;
        }

        public static bool ModCompatCheck_SubjectFormatChatMessage(Chat.SubjectFormatChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            var baseToken = chatMessage.baseToken;
            if (modloaded_TinkersSatchel)
            {
                if (baseToken == mc_TinkersSatchel_SubjectFormatChatMessage_BaseToken_Compass)
                    return ShouldShowClient(chatMessage, cfgTinkersSatchel_Compass);
            }
            if (modloaded_SpireItems)
            {
                if (baseToken == mc_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolSingleToken
                    || baseToken == mc_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolMultipleToken)
                    return cfgSpireItems_BloodIdol.Value;
            }
            if (modloaded_BetterShrines)
            {
                switch (baseToken)
                {
                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineDisorderToken:
                        return ShouldShowClient(chatMessage, cfgBetterShrines_ShrineDisorder);

                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineFallenToken:
                        return ShouldShowClient(chatMessage, cfgBetterShrines_ShrineFallen);

                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineHeresyToken:
                        return ShouldShowClient(chatMessage, cfgBetterShrines_ShrineHeresy);

                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineImpToken:
                        return ShouldShowClient(chatMessage, cfgBetterShrines_ShrineDisorder);

                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispAcceptToken:
                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispDenyToken:
                        return ShouldShowClient(chatMessage, cfgBetterShrines_ShrineWisp);

                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineShieldingToken:
                        return ShouldShowClient(chatMessage, cfgBetterShrines_ShrineShielding);

                    case mc_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineChancePunishedToken:
                        return ShouldShowClient(chatMessage, cfgBetterShrines_ShrineChancePunished);

                    default:
                        // Handle unknown baseToken value
                        break;
                }
            }
            if (modloaded_WellRoundedBalance)
            {
                if (baseToken == mc_WellRoundedBalance_SubjectFormatChatMessage_BaseToken_PluriCorruptedToken)
                    return ShouldShowClient(chatMessage, cfgWRB_PluriCorrupted);
            }
            if (modloaded_MysticsItems)
            {
                if (baseToken == mc_MysticsItems_SubjectFormatChatMessage_BaseToken_ShrineLegendaryToken)
                    return ShouldShowClient(chatMessage, cfgMysticsItems_ShrineLegendary);
            }
            return true;
        }

        public static bool ModCompatCheck_UserChatMessage(Chat.UserChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            var lower = chatMessage.text.ToLower();
            if (modloaded_BossAntiSoftlock)
            {
                switch (lower)
                {
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset1:
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset2:
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset3:
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset4:
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset5:
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset6:
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset7:
                    case mc_BossAntiSoftlock_SimpleChatMessage_Command_BossReset8:
                        return cfgBossAntiSoftlock_Command.Value;
                }
            }
            return true;
        }

        public static bool ModCompatCheck_BodyChatMessage(Chat.BodyChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            var baseToken = chatMessage.token;

            if (modloaded_LostInTransit)
            {
                if (baseToken == mc_LostInTransit_BodyChatMessage_BaseToken_BossHunterOption1Token
                    || baseToken == mc_LostInTransit_BodyChatMessage_BaseToken_BossHunterOption2Token)
                    return ShouldShowClient(chatMessage, cfgLostInTransit_BossHunterBeatingEmbryo);
            }

            return true;
        }

        public static bool ModCompatCheck_SubjectChatMessage(SubjectChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            //var baseToken = chatMessage.baseToken;

            //if (modloaded_TinkersSatchel)
            //{
            //    if (baseToken == mc_TinkersSatchel_SubjectChatMessage_BaseToken_MonkeyPawActivate)
            //        return ShouldShowClient(chatMessage, cfgTinkersSatchel_MonkeyPawActivate);
            //}

            return true;
        }

        public static bool ModCompatCheck_ColoredTokenChatMessage(ColoredTokenChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            //var baseToken = chatMessage.baseToken;

            //if (modloaded_TinkersSatchel)
            //{
            //    if (baseToken == mc_TinkersSatchel_ColoredTokenChatMessage_BaseToken_MonkeyPawItemGrant)
            //        return cfgTinkersSatchel_MonkeyPawItemGrant.Value;
            //}

            return true;
        }
    }
}