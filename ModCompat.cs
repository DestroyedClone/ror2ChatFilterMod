using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;
using RoR2;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;
using static ror2ChatFilterMod.Main;

namespace ror2ChatFilterMod
{
    internal class ModCompat
    {
        public static bool IsAnyModLoaded = false;

        public static bool modloaded_VsTwitch = false;
        //https://github.com/JustDerb/RoR2-VsTwitch/blob/c7a0894e2abe380541b53d55e9a22e3cb043de5a/Events/EventFactory.cs#L221
        public const string modcompat_VsTwitch_SimpleChat_BaseToken_ChallengeToken = "<color=#9147ff>Twitch Chat feels the boss should be harder on the next stage.</color>";
        public static ConfigEntry<bool> cfgModcompat_VsTwitch_Challenge;
        //https://github.com/JustDerb/RoR2-VsTwitch/blob/c7a0894e2abe380541b53d55e9a22e3cb043de5a/Events/EventFactory.cs#L338
        //<color=#{ColorUtility.ToHtmlStringRGB(Color.green)}>
        public static string modcompat_VsTwitch_SimpleChat_BaseToken_StartsWith_AllyToken = $"<color=#00FF00>";
        public const string modcompat_VsTwitch_SimpleChat_BaseToken_EndsWith_AllyToken = "</color> <color=#9147ff> enters the game to help you...</color>";
        public static ConfigEntry<bool> cfgModcompat_VsTwitch_AllyToken;

        //https://github.com/Mordrog/RoR2-TPVoting/blob/af75740cd894e146672ec493978483e5ebdbf46a/TPVoting/Modules/Helpers/ChatHelper.cs#L15
        //public static bool modloaded_TPVoting;

        //https://github.com/MonsterSkinMan/GOTCE/blob/3e4d99234752df2a8acad71ef376aca02857e3ed/GOTCE/Artifact/ArtifactOfWoolie.cs#L59
        public static bool modloaded_GOTCE;
        public const string modcompat_GOTCE_SimpleChat_BaseToken_RushOrDieToken = "<color=#e5eefc>{0}</color>";
        public const string modcompat_GOTCE_SimpleChat_ParamToken_RushOrDieToken = "You got outscaled, idiot.";
        public static ConfigEntry<bool> cfgModcompat_GOTCE_RushOrDie;

        //https://github.com/AngeloTadeucci/RoR2_RaidInfo/blob/eeb96b302c3cdd843b427245319d29d68d3becc4/Message.cs#L6
        //https://github.com/harbingerofme/R2DS-Essentials/blob/165184420d5995f3be6f85913251a349e568000a/Modules/MotD.cs#L167
        //https://github.com/hifoomin/UltimateCustomRun/blob/3144665504a47c7d3ad32e3226fc05a82b7bdc69/UCR.Content/SendChatNotif.cs#L23
        public static bool modloaded_UltimateCustomRun;
        public const string modcompat_UltimateCustomRun_SimpleChatMessage_BaseToken_Welcome = "</size></color><color=#BFA9D3>Thanks for trying out </color><color=#8932D5>UltimateCustomRun.</color>\n" +
          "<color=#BFA9D3>For any mod devs that see this, feel free to contribute and make the mod as good as possible.\n" +
          "There is a to-do list regarding items in the Main Class.\n" +
          "<i>Github PR's / Issues are best</i>, but DMs and pings are also welcome. Have fun and peace out! \u2764</color>";
        public static ConfigEntry<bool> cfgModcompat_UltimateCustomRun_Welcome;

        //https://github.com/KosmosisDire/TeammateRevive/blob/3b8e4e77c8c258ff82fc9372374ec664410332cd/TeammateRevive/Content/Artifact/DeathCurseArtifact.cs#L31
        public static bool modloaded_TeammateRevive;
        public const string modcompat_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseDisabledToken = "<color=\"yellow\">Artifact of Death Curse is disabled because run started in single player.</color>";
        public static ConfigEntry<bool> cfgModcompat_TeammateRevive_DeathCurseDisabled;
        public const string modcompat_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseEnforcedByServerToken = "<color=\"yellow\">Artifact of Death Curse is enforced by server.</color>";
        public static ConfigEntry<bool> cfgModcompat_TeammateRevive_DeathCurseEnforcedByServer;

        //https://github.com/ThinkInvis/RoR2-TinkersSatchel/blob/559eae30e461e0ead649e67b3be312a4765dbf56/Items/LunarEqp/Compass.cs#L210
        public static bool modloaded_TinkersSatchel;
        public const string modcompat_TinkersSatchel_SubjectFormatChatMessage_BaseToken_Compass = "TKSAT_COMPASS_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgModCompat_TinkersSatchel_Compass;

        //https://github.com/SylmarDev/SpireItems/blob/3a4c8ef16ff58bd5457523d6efb475bddf7fe0d5/SpireItems/Relics/Tier2/BloodIdol.cs#L80
        public static bool modloaded_SpireItems;
        //for some reason its a subjectformatchatmessage despite not using any subjects???
        public const string modcompat_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolSingleToken = "<style=cEvent>Your <color=#FFC733>golden idol</color> begins to dull in color and begins bleeding from its eyes. The bleeding never ceases.</style>";
        public const string modcompat_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolMultipleToken = "<style=cEvent>Your <color=#FFC733>golden idols</color> begin to dull in color and begin bleeding from their eyes. The bleeding never ceases.</style>";
        public static ConfigEntry<bool> cfgModCompat_SpireItems_BloodIdol;

        //https://github.com/prodzpod/RoR2-BossAntiSoftlock/blob/4273b77ee0105efca8dbd9e936c1137a1ae0d005/BossAntiSoftlock.cs#L68
        public static bool modloaded_BossAntiSoftlock;
        public const string modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_StartsWith_ResetBossPositionsToken = "<color=#93c47d>Boss Anti-Softlock:</color> Resetting boss positions... ";
        public static ConfigEntry<bool> cfgModCompat_BossAntiSoftlock_ResetBossPosition;
        public const string modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_ErrorResetToken = "<color=#93c47d>Boss Anti-Softlock:</color> Error resetting boss positions; check console for more info!";
        public static ConfigEntry<bool> cfgModCompat_BossAntiSoftlock_ErrorReset;
        public const string modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_ModHintToken = "<color=#93c47d>Boss Anti-Softlock:</color> Type '/bossreset' to reset boss positions.";
        public static ConfigEntry<bool> cfgModCompat_BossAntiSoftlock_ModHint;

        //https://github.com/TeamMoonstorm/MoonstormSharedUtils/blob/f6db127b4dc0b4605922fa5343e2f518c5bf3258/Runtime/Code/Classes/EntityStates/EventState.cs#L94
        //???

        //https://github.com/bb010g/wildbook-R2Mods/blob/a95b5a4843f6d5b4efef73db00f5390324dbda6a/Multitudes/Multitudes.cs#L107
        public static bool modloaded_Multitudes;
        public const string modcompat_Multitudes_SimpleChatMessage_BaseToken_SendMultiplierToken = "Multitudes set to: {0}";
        public static ConfigEntry<bool> cfgModCompat_Multitudes_SendMultiplier;

        //bettershrinesrewrite
        public static bool modloaded_BetterShrines;
        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineHeresyToken = "SHRINE_HERESY_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgModCompat_BetterShrines_ShrineHeresy;
        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineDisorderToken = "SHRINE_DISORDER_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgModCompat_BetterShrines_ShrineDisorder;
        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineFallenToken = "SHRINE_FALLEN_USED";
        public static ConfigEntry<ChatFilterType> cfgModCompat_BetterShrines_ShrineFallen;
        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineImpToken = "SHRINE_IMP_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgModCompat_BetterShrines_ShrineImp;
        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispAcceptToken = "SHRINE_WISP_ACCEPT_MESSAGE";
        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispDenyToken = "SHRINE_WISP_DENY_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgModCompat_BetterShrines_ShrineWisp;
        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineShieldingToken = "SHRINE_SHIELDING_USE_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgModCompat_BetterShrines_ShrineShielding;

        public const string modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineChancePunishedToken = "SHRINE_CHANCE_PUNISHED_MESSAGE";
        public static ConfigEntry<ChatFilterType> cfgModCompat_BetterShrines_ShrineChancePunished;

        //https://github.com/nuxlar/AmalgamMithrix/blob/ff041e9df07e845198488a14aa74f26c8a839dbf/AmalgamMithrix/AmalgamMithrix.cs#L505
        //amalgam is older version of https://github.com/nuxlar/UmbralMithrix
        //no harm in keeping i guess
        public static bool modloaded_AmalgamMithrix;
        public const string modcompat_AmalgamMithrix_SimpleChatMessage_BaseToken_UmbralShrineToken = "<color=#8826dd>The Umbral King awaits...</color>";
        public static ConfigEntry<bool> cfgModCompat_AmalgamMithrix_UmbralShrine;

        //https://github.com/niwith/DropInMultiplayer/blob/9f31beee9ed8c594c75cdfa4de4c193bcc4adfa7/DropInMultiplayerV2
        public static bool modloaded_DropInMultiplayer;
        //server defines this
        //we could cache it when it happens but who knows what format it is, thats why we're doing just default token
        public const string modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_EndsWith_WelcomeToken = "! Join the game by typing '/join_as {survivor name}' in chat (or '/join_as random'). To get a list of availible survivors, type '/list_survivors' in chat";
        public static ConfigEntry<bool> cfgDropInMultiplayer_Welcome;
        public const string modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_MissingCommandToken = "Unable to find command, try /help";
        public static ConfigEntry<bool> cfgDropInMultiplayer_MissingCommand;

        // UNUSED
        //there's some fucking commands here that we have to get
        //https://github.com/niwith/DropInMultiplayer/blob/9f31beee9ed8c594c75cdfa4de4c193bcc4adfa7/DropInMultiplayerV2/Main.cs#L768
        //1. Cache what user most recently said /COMMAND
        //2. Then filter the command based
        //might fuck up
        public static NetworkUser mostRecentNetworkUserChat;
        public const string modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_HelpToken = "/HELP";
        public const string modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_JoinToken = "/JOIN";
        public const string modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_JoinAsToken = "/JOIN_AS";
        public const string modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_ListSurvivorsToken = "/LIST_SURVIVORS";
        public const string modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_Command_ListBodiesToken = "/LIST_BODIES";
        public static ConfigEntry<bool> cfgDropInMultiplayer_Commands;

        public static bool modloaded_DarthVader;
        public const string modcompat_DarthVader_SimpleChatMessage_BaseToken_DeathMessageToken = "There was too much Sand";
        public static ConfigEntry<bool> cfgDarthVader_DeathMessage;


        public static void Initialize(ConfigFile Config)
        {
            CheckLoadedMods();
            SetupConfig(Config);
        }

        public static void CheckLoadedMods()
        {
            static bool IsModLoaded(string key)
            {
                var result = BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey(key);
                IsAnyModLoaded = IsAnyModLoaded || result;
                return result;
            }
            modloaded_VsTwitch = IsModLoaded("com.justinderby.vstwitch");
            modloaded_GOTCE = IsModLoaded("com.TheBestAssociatedLargelyLudicrousSillyheadGroup.GOTCE");
            modloaded_UltimateCustomRun = IsModLoaded("com.HIFU.UltimateCustomRun");
            modloaded_TeammateRevive = IsModLoaded("KosmosisDire.TeammateRevival");
            modloaded_TinkersSatchel = IsModLoaded("com.ThinkInvisible.TinkersSatchel");
            modloaded_SpireItems = IsModLoaded("SylmarDev.SpireItems");
            modloaded_Multitudes = IsModLoaded("dev.wildbook.multitudes");
            modloaded_BetterShrines = IsModLoaded("com.evaisa.moreshrines");
            modloaded_AmalgamMithrix = IsModLoaded("com.Nuxlar.UmbralMithrix");
            modloaded_DropInMultiplayer = IsModLoaded("com.niwith.DropInMultiplayer");
            modloaded_DarthVader = IsModLoaded("com.PopcornFactory.DarthVaderMod");
            if (IsModLoaded("com.rune580.riskofoptions"))
            {
                ModCompat_RiskOfOptions();
            }
        }

        public static void SetupConfig(ConfigFile Config)
        {
            //no modcheck otherwise you'd have to launch with it on to generate and ehhhhh
            cfgModCompat_SpireItems_BloodIdol = Config.Bind("SpireItems", "BloodIdol", true, modcompat_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolSingleToken);
            cfgModcompat_VsTwitch_Challenge = Config.Bind("VsTwitch", "Challenge", true, modcompat_VsTwitch_SimpleChat_BaseToken_ChallengeToken);
            cfgModcompat_VsTwitch_AllyToken = Config.Bind("VsTwitch", "AllyToken", true, modcompat_VsTwitch_SimpleChat_BaseToken_StartsWith_AllyToken+modcompat_VsTwitch_SimpleChat_BaseToken_StartsWith_AllyToken);
            cfgModcompat_GOTCE_RushOrDie = Config.Bind("GOTCE", "Woolie's Artifact", true, string.Format(modcompat_GOTCE_SimpleChat_BaseToken_RushOrDieToken, modcompat_GOTCE_SimpleChat_ParamToken_RushOrDieToken));
            cfgModcompat_UltimateCustomRun_Welcome = Config.Bind("UltimateCustomRun", "Welcome", true, modcompat_UltimateCustomRun_SimpleChatMessage_BaseToken_Welcome);
            cfgModcompat_TeammateRevive_DeathCurseDisabled = Config.Bind("TeammateRevive", "DeathCurseDisabled", true, modcompat_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseDisabledToken);
            cfgModcompat_TeammateRevive_DeathCurseEnforcedByServer = Config.Bind("TeammateRevive", "DeathCurseEnforcedByServer", true, modcompat_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseEnforcedByServerToken);
            cfgModCompat_TinkersSatchel_Compass = Config.Bind("TinkersSatchel", "Compass", ChatFilterType.All, modcompat_TinkersSatchel_SubjectFormatChatMessage_BaseToken_Compass);
            cfgModCompat_BossAntiSoftlock_ResetBossPosition = Config.Bind("BossAntiSoftlock", "ResetBossPosition", true, modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_StartsWith_ResetBossPositionsToken);
            cfgModCompat_BossAntiSoftlock_ErrorReset = Config.Bind("BossAntiSoftlock", "Error Reset", true, modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_ErrorResetToken);
            cfgModCompat_BossAntiSoftlock_ModHint = Config.Bind("BossAntiSoftlock", "ModHint", true, modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_ModHintToken);
            cfgModCompat_Multitudes_SendMultiplier = Config.Bind("Multitudes", "SendMultiplier", true, modcompat_Multitudes_SimpleChatMessage_BaseToken_SendMultiplierToken);
            cfgModCompat_BetterShrines_ShrineHeresy = Config.Bind("BetterShrines", "ShrineHeresy", ChatFilterType.All, modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineHeresyToken);
            cfgModCompat_BetterShrines_ShrineDisorder = Config.Bind("BetterShrines", "ShrineDisorder", ChatFilterType.All, modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineDisorderToken);
            cfgModCompat_BetterShrines_ShrineFallen = Config.Bind("BetterShrines", "ShrineFallen", ChatFilterType.All, modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineFallenToken);
            cfgModCompat_BetterShrines_ShrineImp = Config.Bind("BetterShrines", "ShrineImp", ChatFilterType.All, modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineImpToken);
            cfgModCompat_BetterShrines_ShrineWisp = Config.Bind("BetterShrines", "ShrineWisp", ChatFilterType.All, modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispAcceptToken+"\n"+modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispDenyToken);
            cfgModCompat_BetterShrines_ShrineShielding = Config.Bind("BetterShrines", "ShrineShielding", ChatFilterType.All, modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineShieldingToken);
            cfgModCompat_BetterShrines_ShrineChancePunished = Config.Bind("BetterShrines", "ShrineChancePunished", ChatFilterType.All, modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineChancePunishedToken);
            cfgModCompat_AmalgamMithrix_UmbralShrine = Config.Bind("AmalgamMithrix", "UmbralShrine", true, modcompat_AmalgamMithrix_SimpleChatMessage_BaseToken_UmbralShrineToken);
            cfgDropInMultiplayer_Welcome = Config.Bind("DropInMultiplayer", "Welcome", true, modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_EndsWith_WelcomeToken);
            cfgDropInMultiplayer_MissingCommand = Config.Bind("DropInMultiplayer", "MissingCommand", true, modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_MissingCommandToken);
            cfgDarthVader_DeathMessage = Config.Bind("DarthVader", "Death Message", true, modcompat_DarthVader_SimpleChatMessage_BaseToken_DeathMessageToken);
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
            A2(cfgShowPlayerPickupMessagesClient, vanilla);
            A2(cfgShowDeathMessagesClient, "Vanilla");
            A2(cfgShowJoinMessagesClient, "Vanilla");
            A2(cfgShowLeaveMessagesClient, "Vanilla");
            A(cfgShowNPCPickupMessagesClient, "Vanilla");
            A2(cfgShowAhoyClient, "Vanilla");
            A(cfgShowNpcClient, "Vanilla");
            A2(cfgShowAchievementClient, "Vanilla");
            A(cfgShowFamilyClient, "Vanilla");
            A2(cfgShowTeleporterActivationClient, "Vanilla");
            A2(cfgShowSuppressorClient, "Vanilla");
            A(cfgShowPortalShopWillOpenClient, "Vanilla");
            A(cfgShowPortalGoldshoresWillOpenClient, "Vanilla");
            A(cfgShowPortalMSWillOpenClient, "Vanilla");
            A(cfgShowPortalShopOpenClient, "Vanilla");
            A(cfgShowPortalGoldshoresOpenClient, "Vanilla");
            A(cfgShowPortalMSOpenClient, "Vanilla");
            A(cfgShowMountainTeleporterClient, "Vanilla");
            A2(cfgShowShrineChanceWinClient, "Vanilla");
            A2(cfgShowShrineChanceFailClient, "Vanilla");
            A(cfgShowSeerClient, "Vanilla");
            A2(cfgShowShrineBossClient, "Vanilla");
            A2(cfgShowShrineBloodClient, "Vanilla");
            A2(cfgShowShrineRestackClient, "Vanilla");
            A2(cfgShowShrineHealingClient, "Vanilla");
            A2(cfgShowShrineCombatClient, "Vanilla");
            A(cfgShowArenaEndClient, "Vanilla");
            A2(cfgShowPetFrogClient, "Vanilla");
            #endregion Vanilla

            if (modloaded_BossAntiSoftlock)
            {
                A(cfgModCompat_BossAntiSoftlock_ResetBossPosition, "BossAntiSoftlock");
                A(cfgModCompat_BossAntiSoftlock_ErrorReset, "BossAntiSoftlock");
                A(cfgModCompat_BossAntiSoftlock_ModHint, "BossAntiSoftlock");
            }
            if (modloaded_SpireItems)
            {
                A(cfgModCompat_SpireItems_BloodIdol, "SpireItems");
            }

            if (modloaded_VsTwitch)
            {
                A(cfgModcompat_VsTwitch_Challenge, "VsTwitch");
                A(cfgModcompat_VsTwitch_AllyToken, "VsTwitch");
            }
            if (modloaded_GOTCE)
            {
                A(cfgModcompat_GOTCE_RushOrDie, "GOTCE");
            }
            if (modloaded_UltimateCustomRun)
            {
                A(cfgModcompat_UltimateCustomRun_Welcome, "UltimateCustomRun");
            }
            if (modloaded_TeammateRevive)
            {
                A(cfgModcompat_TeammateRevive_DeathCurseDisabled, "TeammateRevive");
                A(cfgModcompat_TeammateRevive_DeathCurseEnforcedByServer, "TeammateRevive");
            }
            if (modloaded_TinkersSatchel)
            {
                A2(cfgModCompat_TinkersSatchel_Compass, "TinkersSatchel");
            }
            if (modloaded_Multitudes)
            {
                A(cfgModCompat_Multitudes_SendMultiplier, "Multitudes");
            }
            if (modloaded_BetterShrines)
            {
                A2(cfgModCompat_BetterShrines_ShrineHeresy, "BetterShrines");
                A2(cfgModCompat_BetterShrines_ShrineDisorder, "BetterShrines");
                A2(cfgModCompat_BetterShrines_ShrineFallen, "BetterShrines");
                A2(cfgModCompat_BetterShrines_ShrineImp, "BetterShrines");
                A2(cfgModCompat_BetterShrines_ShrineWisp, "BetterShrines");
                A2(cfgModCompat_BetterShrines_ShrineShielding, "BetterShrines");
                A2(cfgModCompat_BetterShrines_ShrineChancePunished, "BetterShrines");
            }
            if (modloaded_AmalgamMithrix)
            {
                A(cfgModCompat_AmalgamMithrix_UmbralShrine, "AmalgamMithrix");
            }
            if (modloaded_DropInMultiplayer)
            {
                A(cfgDropInMultiplayer_Welcome, "DropInMultiplayer");
                A(cfgDropInMultiplayer_MissingCommand, "DropInMultiplayer");
            }
            if (modloaded_DarthVader)
            {
                A(cfgDarthVader_DeathMessage, "DarthVader");
            }
        }

        public static bool ModCompatCheck_SimpleChatMessage(Chat.SimpleChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            string baseToken = chatMessage.baseToken;
            if (modloaded_VsTwitch)
            {
                if (baseToken == modcompat_VsTwitch_SimpleChat_BaseToken_ChallengeToken)
                    return !cfgModcompat_VsTwitch_Challenge.Value;
                else if (baseToken.StartsWith(modcompat_VsTwitch_SimpleChat_BaseToken_StartsWith_AllyToken)
                    && baseToken.EndsWith(modcompat_VsTwitch_SimpleChat_BaseToken_EndsWith_AllyToken))
                    return !cfgModcompat_VsTwitch_AllyToken.Value;
            }
            if (modloaded_GOTCE)
            {
                if (baseToken == modcompat_GOTCE_SimpleChat_BaseToken_RushOrDieToken
                    && chatMessage.paramTokens.Length == 1
                    && chatMessage.paramTokens[0] == modcompat_GOTCE_SimpleChat_ParamToken_RushOrDieToken)
                    return !cfgModcompat_GOTCE_RushOrDie.Value;
            }
            if (modloaded_UltimateCustomRun)
            {
                if (baseToken == modcompat_UltimateCustomRun_SimpleChatMessage_BaseToken_Welcome)
                    return !cfgModcompat_UltimateCustomRun_Welcome.Value;
            }
            if (modloaded_TeammateRevive)
            {
                if (baseToken == modcompat_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseDisabledToken)
                    return !cfgModcompat_TeammateRevive_DeathCurseDisabled.Value;
                else if (baseToken == modcompat_TeammateRevive_SimpleChatMessage_BaseToken_DeathCurseEnforcedByServerToken)
                    return !cfgModcompat_TeammateRevive_DeathCurseEnforcedByServer.Value;
            }
            if (modloaded_BossAntiSoftlock)
            {
                if (baseToken.StartsWith(modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_StartsWith_ResetBossPositionsToken))
                    return !cfgModCompat_BossAntiSoftlock_ResetBossPosition.Value;
                else if (baseToken == modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_ErrorResetToken) 
                    return !cfgModCompat_BossAntiSoftlock_ErrorReset.Value;
                else if (baseToken == modcompat_BossAntiSoftlock_SimpleChatMessage_BaseToken_ModHintToken)
                    return !cfgModCompat_BossAntiSoftlock_ModHint.Value;
            }
            if (modloaded_Multitudes)
            {
                if (baseToken == modcompat_Multitudes_SimpleChatMessage_BaseToken_SendMultiplierToken)
                    return !cfgModCompat_Multitudes_SendMultiplier.Value;
            }
            if (modloaded_AmalgamMithrix)
            {
                if (baseToken == modcompat_AmalgamMithrix_SimpleChatMessage_BaseToken_UmbralShrineToken)
                    return !cfgModCompat_AmalgamMithrix_UmbralShrine.Value;
            }
            if (modloaded_DropInMultiplayer)
            {
                if (baseToken.EndsWith(modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_EndsWith_WelcomeToken))
                    return !cfgDropInMultiplayer_Welcome.Value;
                else if (baseToken == modcompat_DropInMultiplayer_SimpleChatMessage_BaseToken_MissingCommandToken)
                    return !cfgDropInMultiplayer_MissingCommand.Value;

            }
            if (modloaded_DarthVader)
            {
                if (baseToken == modcompat_DarthVader_SimpleChatMessage_BaseToken_DeathMessageToken)
                    return !cfgDarthVader_DeathMessage.Value;
            }

            return true;
        }

        public static bool ModCompatCheck_SubjectFormatChatMessage(Chat.SubjectFormatChatMessage chatMessage)
        {
            if (!IsAnyModLoaded) return true;
            var baseToken = chatMessage.baseToken;
            if (modloaded_TinkersSatchel)
            {
                if (baseToken == modcompat_TinkersSatchel_SubjectFormatChatMessage_BaseToken_Compass)
                    return ShouldShowClient(chatMessage, cfgModCompat_TinkersSatchel_Compass);
            }
            if (modloaded_SpireItems)
            {
                if (baseToken == modcompat_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolSingleToken
                    || baseToken == modcompat_SpireItems_SubjectFormatChatMessage_BaseToken_GoldenIdolMultipleToken)
                    return !cfgModCompat_SpireItems_BloodIdol.Value;
            }
            if (modloaded_BetterShrines)
            {
                switch (baseToken)
                {
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineDisorderToken:
                        return ShouldShowClient(chatMessage, cfgModCompat_BetterShrines_ShrineDisorder);
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineFallenToken:
                        return ShouldShowClient(chatMessage, cfgModCompat_BetterShrines_ShrineFallen);
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineHeresyToken:
                        return ShouldShowClient(chatMessage, cfgModCompat_BetterShrines_ShrineHeresy);
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineImpToken:
                        return ShouldShowClient(chatMessage, cfgModCompat_BetterShrines_ShrineDisorder);
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispAcceptToken:
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineWispDenyToken:
                        return ShouldShowClient(chatMessage, cfgModCompat_BetterShrines_ShrineWisp);
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineShieldingToken:
                        return ShouldShowClient(chatMessage, cfgModCompat_BetterShrines_ShrineShielding);
                    case modcompat_BetterShrines_SubjectFormatChatMessage_BaseToken_ShrineChancePunishedToken:
                        return ShouldShowClient(chatMessage, cfgModCompat_BetterShrines_ShrineChancePunished);
                    default:
                        // Handle unknown baseToken value
                        break;
                }
            }
            return true;
        }
    }
}
