﻿using System;
using BepInEx;
using BepInEx.Configuration;
using UnityEngine;
using RiskOfOptions;
using RoR2;
using System.Collections.Generic;
using JetBrains.Annotations;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using TMPro;
using UnityEngine.Networking;
using System.Text.RegularExpressions;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace ror2ChatFilterMod
{
    [BepInPlugin("com.DestroyedClone.RoR2ChatFilterMod", "Chat Filter Mod", "1.0.0")]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    public class Main : BaseUnityPlugin
    {
        public static ConfigEntry<string> cfgTimestampFormat;

        public static ConfigEntry<FilterType> cfgShowPlayerPickupMessagesClient;
        public static ConfigEntry<FilterType> cfgShowDeathMessagesClient;
        public static ConfigEntry<FilterType> cfgShowJoinMessagesClient;
        public static ConfigEntry<FilterType> cfgShowLeaveMessagesClient;
        public static ConfigEntry<bool> cfgShowNPCPickupMessagesClient;
        public static ConfigEntry<FilterType> cfgShowAhoyClient;
        public static ConfigEntry<bool> cfgShowNpcClient;
        public static ConfigEntry<FilterType> cfgShowAchievementClient;
        public static ConfigEntry<bool> cfgShowFamilyClient;
        public static ConfigEntry<FilterType> cfgShowTeleporterActivationClient;
        public static ConfigEntry<FilterType> cfgShowSuppressorClient;
        public static ConfigEntry<bool> cfgShowPortalShopWillOpenClient;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresWillOpenClient;
        public static ConfigEntry<bool> cfgShowPortalMSWillOpenClient;
        public static ConfigEntry<bool> cfgShowPortalShopOpenClient;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresOpenClient;
        public static ConfigEntry<bool> cfgShowPortalMSOpenClient;
        public static ConfigEntry<bool> cfgShowMountainTeleporterClient;
        public static ConfigEntry<FilterType> cfgShowShrineChanceWinClient;
        public static ConfigEntry<FilterType> cfgShowShrineChanceFailClient;
        public static ConfigEntry<bool> cfgShowSeerClient;
        public static ConfigEntry<FilterType> cfgShowShrineBossClient;
        public static ConfigEntry<FilterType> cfgShowShrineBloodClient;
        public static ConfigEntry<FilterType> cfgShowShrineRestackClient;
        public static ConfigEntry<FilterType> cfgShowShrineHealingClient;
        public static ConfigEntry<FilterType> cfgShowShrineCombatClient;
        public static ConfigEntry<bool> cfgShowArenaEndClient;
        public static ConfigEntry<FilterType> cfgShowPetFrogClient;

        public static ConfigEntry<bool> cfgShowDeathMessagesServer;
        public static ConfigEntry<bool> cfgShowJoinMessagesServer;
        public static ConfigEntry<bool> cfgShowLeaveMessagesServer;
        public static ConfigEntry<bool> cfgShowNPCPickupMessagesServer;
        public static ConfigEntry<bool> cfgShowAhoyServer;
        public static ConfigEntry<bool> cfgShowNpcServer;
        public static ConfigEntry<bool> cfgShowAchievementServer;
        public static ConfigEntry<bool> cfgShowFamilyServer;
        public static ConfigEntry<bool> cfgShowTeleporterActivationServer;
        public static ConfigEntry<bool> cfgShowSuppressorServer;
        public static ConfigEntry<bool> cfgShowPortalShopWillOpenServer;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresWillOpenServer;
        public static ConfigEntry<bool> cfgShowPortalMSWillOpenServer;
        public static ConfigEntry<bool> cfgShowPortalShopOpenServer;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresOpenServer;
        public static ConfigEntry<bool> cfgShowPortalMSOpenServer;
        public static ConfigEntry<bool> cfgShowMountainTeleporterServer;
        public static ConfigEntry<bool> cfgShowShrineChanceWinServer;
        public static ConfigEntry<bool> cfgShowShrineChanceFailServer;
        public static ConfigEntry<bool> cfgShowSeerServer;
        public static ConfigEntry<bool> cfgShowShrineBossServer;
        public static ConfigEntry<bool> cfgShowShrineBloodServer;
        public static ConfigEntry<bool> cfgShowShrineRestackServer;
        public static ConfigEntry<bool> cfgShowShrineHealingServer;
        public static ConfigEntry<bool> cfgShowShrineCombatServer;
        public static ConfigEntry<bool> cfgShowArenaEndServer;
        public static ConfigEntry<bool> cfgShowPetFrogServer;

        public enum FilterType
        {
            All,
            Myself,
            Others,
            Off
        }


        public void Start()
        {
            SetupConfig();

            On.RoR2.Chat.SendBroadcastChat_ChatMessageBase += ChatFilterServer;
            On.RoR2.Chat.AddMessage_ChatMessageBase += ChatFilterClient;

            //server

                //On.RoR2.GenericPickupController.SendPickupMessage += GenericPickupController_SendPickupMessage;
            if (!cfgShowFamilyServer.Value)
                On.RoR2.ClassicStageInfo.BroadcastFamilySelection += ClassicStageInfo_BroadcastFamilySelection;

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                ModCompat_RiskOfOptions();
            }
        }
        private static bool ShouldShowClient(ChatMessageBase chatMessage, ConfigEntry<FilterType> configEntry)
        {
            if (chatMessage is Chat.PlayerPickupChatMessage playerPickupChatMessage)
                switch (configEntry.Value)
                {
                    case FilterType.All:
                        return true;
                    case FilterType.Myself:
                        return playerPickupChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Others:
                        return playerPickupChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Off:
                        return false;
                }
            else if (chatMessage is Chat.PlayerDeathChatMessage playerDeathChatMessage)
                switch (configEntry.Value)
                {
                    case FilterType.All:
                        return true;
                    case FilterType.Myself:
                        return playerDeathChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Others:
                        return playerDeathChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Off:
                        return false;
                }
            else if (chatMessage is Chat.PlayerChatMessage playerChatMessage)
                switch (configEntry.Value)
                {
                    case FilterType.All:
                        return true;
                    case FilterType.Myself:
                        return playerChatMessage.networkPlayerName.steamId == LocalUserManager.GetFirstLocalUser().currentNetworkUser.GetNetworkPlayerName().steamId;
                    case FilterType.Others:
                        return playerChatMessage.networkPlayerName.steamId != LocalUserManager.GetFirstLocalUser().currentNetworkUser.GetNetworkPlayerName().steamId;
                    case FilterType.Off:
                        return false;
                }
            else if (chatMessage is Chat.BodyChatMessage bodyChatMessage)
            {
                switch (configEntry.Value)
                {
                    case FilterType.All:
                        return true;
                    case FilterType.Myself:
                        return bodyChatMessage.bodyObject == LocalUserManager.GetFirstLocalUser().cachedBody.gameObject;
                    case FilterType.Others:
                        return bodyChatMessage.bodyObject != LocalUserManager.GetFirstLocalUser().cachedBody.gameObject;
                    case FilterType.Off:
                        return false;
                }
            }
            else if (chatMessage is ColoredTokenChatMessage coloredTokenChatMessage)
            {
                switch (configEntry.Value)
                {
                    case FilterType.All:
                        return true;
                    case FilterType.Myself:
                        return coloredTokenChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Others:
                        return coloredTokenChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Off:
                        return false;
                }
            }
            else if (chatMessage is Chat.SubjectFormatChatMessage subjectFormatChatMessage)
            {
                switch (configEntry.Value)
                {
                    case FilterType.All:
                        return true;
                    case FilterType.Myself:
                        return subjectFormatChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Others:
                        return subjectFormatChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Off:
                        return false;
                }
            }
            else if (chatMessage is SubjectChatMessage subjectChatMessage)
            {
                switch (configEntry.Value)
                {
                    case FilterType.All:
                        return true;
                    case FilterType.Myself:
                        return subjectChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Others:
                        return subjectChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;
                    case FilterType.Off:
                        return false;
                }
            }
            return true;
        }

        private void ChatFilterClient(On.RoR2.Chat.orig_AddMessage_ChatMessageBase orig, ChatMessageBase message)
        {

            bool showMessage = true;
            switch (message)
            {
                case Chat.PlayerPickupChatMessage playerPickupChatMessage:
                    switch (playerPickupChatMessage.baseToken)
                    {
                        case "PLAYER_PICKUP":
                            showMessage = ShouldShowClient(playerPickupChatMessage, cfgShowPlayerPickupMessagesClient);
                            break;
                    }
                    break;
                case Chat.PlayerDeathChatMessage playerDeathChatMessage:
                    showMessage = ShouldShowClient(playerDeathChatMessage, cfgShowDeathMessagesClient);
                    break;
                case Chat.PlayerChatMessage chatMsg:
                    switch (chatMsg.baseToken)
                    {
                        case "PLAYER_CONNECTED":
                            showMessage = ShouldShowClient(chatMsg, cfgShowJoinMessagesClient);
                            break;
                        case "PLAYER_DISCONNECTED":
                            showMessage = ShouldShowClient(chatMsg, cfgShowLeaveMessagesClient);
                            break;
                    }
                    break;
                case Chat.BodyChatMessage bodyChatMessage:
                    showMessage = bodyChatMessage.token switch
                    {
                        "EQUIPMENT_BOSSHUNTERCONSUMED_CHAT" => ShouldShowClient(bodyChatMessage, cfgShowAhoyClient),
                        _ => true
                    };
                    break;
                case Chat.NpcChatMessage _:
                    showMessage = cfgShowNpcClient.Value;
                    break;
                case ColoredTokenChatMessage coloredTokenChatMessage:
                    showMessage = coloredTokenChatMessage.baseToken switch
                    {
                        "VOID_SUPPRESSOR_USE_MESSAGE" => ShouldShowClient(coloredTokenChatMessage, cfgShowSuppressorClient),
                        _ => true
                    };
                    break;
                case Chat.SubjectFormatChatMessage subjectFormatChatMessage:
                    showMessage = subjectFormatChatMessage.baseToken switch
                    {
                        "ACHIEVEMENT_UNLOCKED_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowAchievementClient),
                        "SHRINE_CHANCE_FAIL_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineChanceFailClient),
                        "SHRINE_CHANCE_SUCCESS_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineChanceWinClient),
                        "SHRINE_BOSS_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineBossClient),
                        "SHRINE_BLOOD_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineBloodClient),
                        "SHRINE_RESTACK_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineRestackClient),
                        "SHRINE_HEALING_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineHealingClient),
                        "SHRINE_COMBAT_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineCombatClient),
                        _ => true
                    };
                    break;
                case SubjectChatMessage subjectChatMessage:
                    showMessage = subjectChatMessage.baseToken switch
                    {
                        "PLAYER_ACTIVATED_TELEPORTER" => ShouldShowClient(subjectChatMessage, cfgShowTeleporterActivationClient),
                        "PET_FROG" => ShouldShowClient(subjectChatMessage, cfgShowPetFrogClient),
                        _ => true
                    };
                    break;
                case Chat.SimpleChatMessage simpleChatMessage:
                    //InfiniteTower: stageTransitionChatToken
                    //InfiniteTower: beginChatToken
                    //InfiniteTower: suddenDeathChatToken
                    //STONEGATE_OPEN
                    //LUNAR_TELEPORTER_IDLE
                    //LUNAR_TELEPORTER_ACTIVE
                    //VULTURE_EGG_WARNING
                    //VULTURE_EGG_BEGIN
                    showMessage = simpleChatMessage.baseToken switch
                    {
                        "PORTAL_SHOP_WILL_OPEN" => cfgShowPortalShopWillOpenClient.Value,
                        "PORTAL_GOLDSHORES_WILL_OPEN" => cfgShowPortalGoldshoresWillOpenClient.Value,
                        "PORTAL_MS_WILL_OPEN" => cfgShowPortalMSWillOpenClient.Value,
                        "SHRINE_BOSS_BEGIN_TRIAL" => cfgShowMountainTeleporterClient.Value,
                        "PORTAL_SHOP_OPEN" => cfgShowPortalMSOpenClient.Value,
                        "PORTAL_GOLDSHORES_OPEN" => cfgShowPortalGoldshoresOpenClient.Value,
                        "PORTAL_MS_OPEN" => cfgShowPortalMSOpenClient.Value,
                        "ARENA_END" => cfgShowArenaEndClient.Value,
                        _ => simpleChatMessage.baseToken.StartsWith("BAZAAR_SEER_")
                            ? cfgShowSeerClient.Value
                            : !simpleChatMessage.baseToken.StartsWith("FAMILY_")
|| cfgShowFamilyClient.Value
                    };
                    break;
            }

            if (showMessage)
                orig(message);
        }


        public void SetupConfig()
        {
            #region Client
            ConfigEntry<bool> BindClient(string k, bool t, string d)
            {
                return Config.Bind("Client", k, t, d);
            }
            ConfigEntry<FilterType> BindClient2(string k, FilterType t, string d)
            {
                return Config.Bind("Client", k, t, d);
            }
            cfgShowPlayerPickupMessagesClient = Config.Bind("Client", "Show Player Pickup Messages", FilterType.All, "<style=cEvent>{0} picked up {1}{2}</color>");
            cfgTimestampFormat = Config.Bind("Client", "Timestamp Format", "{0} {1}", "First parameter is the timestamp, the second is the original message. Leave empty to disable.");

            cfgShowDeathMessagesClient = BindClient2("Show Death Messages", FilterType.All, "");
            cfgShowJoinMessagesClient = BindClient2("Show Join Messages", FilterType.All, "<style=cEvent>{0} connected.</color>");
            cfgShowLeaveMessagesClient = BindClient2("Show Leave Messages", FilterType.All, "<style=cEvent>{0} disconnected.</color>");
            cfgShowNPCPickupMessagesClient = BindClient("Show NPC Pickup Messages", true, "Scav pickups, void fields item adds");
            cfgShowAhoyClient = BindClient2("Show Ahoy Messages", FilterType.All, "Ahoy!");
            cfgShowNpcClient = BindClient("Show NPC Messages", true, "Mithrix messages");
            cfgShowAchievementClient = BindClient2("Show Achievement Messages", FilterType.All, "<color=#ccd3e0>{0} achieved <color=#BDE151>{1}</color></color>");
            cfgShowFamilyClient = BindClient("Show Family Messages", true, "");
            cfgShowTeleporterActivationClient = BindClient2("Show Teleporter Activation Messages", FilterType.All, "<style=cEvent>{0} activated the <style=cDeath>Teleporter <sprite name=\"TP\" tint=1></style>.</style>");
            cfgShowSuppressorClient = BindClient2("Show Suppressor Messages", FilterType.All, "<style=cShrine>{0} eradicated {1} from the universe.");
            cfgShowPortalShopWillOpenClient = BindClient("Show Portal Shop Will Open Messages", true, "<style=cWorldEvent>A blue orb appears..</style>");
            cfgShowPortalGoldshoresWillOpenClient = BindClient("Show Portal Goldshores Will Open Messages", true, "<style=cWorldEvent>A gold orb appears..</style>");
            cfgShowPortalMSWillOpenClient = BindClient("Show Portal MS Will Open Messages", true, "<style=cWorldEvent>A celestial orb appears..</style>");
            cfgShowPortalShopOpenClient = BindClient("Show Portal Shop Open Messages", true, "<style=cWorldEvent>A blue portal appears..</style>");
            cfgShowPortalGoldshoresOpenClient = BindClient("Show Portal Goldshores Open Messages", true, "<style=cWorldEvent>A gold portal appears..</style>");
            cfgShowPortalMSOpenClient = BindClient("Show Portal MS Open Messages", true, "<style=cWorldEvent>A celestial portal appears..</style>");
            cfgShowMountainTeleporterClient = BindClient("Show Mountain Teleporter Messages", true, "<style=cShrine>Let the challenge of the Mountain... begin!</style>");
            cfgShowShrineChanceWinClient = BindClient2("Show Shrine Chance Win Messages", FilterType.All, "<style=cShrine>{0} offered to the shrine and was rewarded!</color>");
            cfgShowShrineChanceFailClient = BindClient2("Show Shrine Chance Fail Messages", FilterType.All, "<style=cShrine>{0} offered to the shrine and gained nothing.</color>");
            cfgShowSeerClient = BindClient("Show Seer Messages", true, "<style=cWorldEvent>You dream of STAGEHINT.</style>");
            cfgShowShrineBossClient = BindClient2("Show Shrine Boss Messages", FilterType.All, "<style=cShrine>{0} has invited the challenge of the Mountain..</color>");
            cfgShowShrineBloodClient = BindClient2("Show Shrine Blood Messages", FilterType.All, "<style=cShrine>{0} feels a searing pain, and has gained {1} gold.</color>");
            cfgShowShrineRestackClient = BindClient2("Show Shrine Restack Messages", FilterType.All, "<style=cShrine>{0} is... sequenced.</color>");
            cfgShowShrineHealingClient = BindClient2("Show Shrine Healing Messages", FilterType.All, "\"<style=cShrine>{0} is embraced by the healing warmth of the Woods.</color>");
            cfgShowShrineCombatClient = BindClient2("Show Shrine Combat Messages", FilterType.All, "<style=cShrine>{0} has summoned {1}s to fight.</color>");
            cfgShowArenaEndClient = BindClient("Show Arena End Messages", false, "<style=cWorldEvent>The Cell stabilizes.</style>");
            cfgShowPetFrogClient = BindClient2("Show Pet Frog Messages", FilterType.All, "{0} pet the frog.");
            #endregion

            #region Server
            ConfigEntry<bool> BindServer(string k, bool t, string d)
            {
                return Config.Bind("Server", k, t, d);
            }
            cfgShowDeathMessagesServer = BindServer("Show Death Messages", true, "");
            cfgShowJoinMessagesServer = BindServer("Show Join Messages", true, "<style=cEvent>{0} connected.</color>");
            cfgShowLeaveMessagesServer = BindServer("Show Leave Messages", true, "<style=cEvent>{0} disconnected.</color>");
            cfgShowNPCPickupMessagesServer = BindServer("Show NPC Pickup Messages", true, "Scav pickups, void fields item adds");
            cfgShowAhoyServer = BindServer("Show Ahoy Messages", false, "Ahoy!");
            cfgShowNpcServer = BindServer("Show NPC Messages", true, "Mithrix messages");
            cfgShowAchievementServer = BindServer("Show Achievement Messages", false, "<color=#ccd3e0>{0} achieved <color=#BDE151>{1}</color></color>");
            cfgShowFamilyServer = BindServer("Show Family Messages", true, "");
            cfgShowTeleporterActivationServer = BindServer("Show Teleporter Activation Messages", true, "<style=cEvent>{0} activated the <style=cDeath>Teleporter <sprite name=\"TP\" tint=1></style>.</style>");
            cfgShowSuppressorServer = BindServer("Show Suppressor Messages", true, "<style=cShrine>{0} eradicated {1} from the universe.");
            cfgShowPortalShopWillOpenServer = BindServer("Show Portal Shop Will Open Messages", true, "<style=cWorldEvent>A blue orb appears..</style>");
            cfgShowPortalGoldshoresWillOpenServer = BindServer("Show Portal Goldshores Will Open Messages", true, "<style=cWorldEvent>A gold orb appears..</style>");
            cfgShowPortalMSWillOpenServer = BindServer("Show Portal MS Will Open Messages", true, "<style=cWorldEvent>A celestial orb appears..</style>");
            cfgShowPortalShopOpenServer = BindServer("Show Portal Shop Open Messages", true, "<style=cWorldEvent>A blue portal appears..</style>");
            cfgShowPortalGoldshoresOpenServer = BindServer("Show Portal Goldshores Open Messages", true, "<style=cWorldEvent>A gold portal appears..</style>");
            cfgShowPortalMSOpenServer = BindServer("Show Portal MS Open Messages", true, "<style=cWorldEvent>A celestial portal appears..</style>");
            cfgShowMountainTeleporterServer = BindServer("Show Mountain Teleporter Messages", true, "<style=cShrine>Let the challenge of the Mountain... begin!</style>");
            cfgShowShrineChanceWinServer = BindServer("Show Shrine Chance Win Messages", false, "<style=cShrine>{0} offered to the shrine and was rewarded!</color>");
            cfgShowShrineChanceFailServer = BindServer("Show Shrine Chance Fail Messages", false, "<style=cShrine>{0} offered to the shrine and gained nothing.</color>");
            cfgShowSeerServer = BindServer("Show Seer Messages", true, "<style=cWorldEvent>You dream of STAGEHINT.</style>");
            cfgShowShrineBossServer = BindServer("Show Shrine Boss Messages", true, "<style=cShrine>{0} has invited the challenge of the Mountain..</color>");
            cfgShowShrineBloodServer = BindServer("Show Shrine Blood Messages", false, "<style=cShrine>{0} feels a searing pain, and has gained {1} gold.</color>");
            cfgShowShrineRestackServer = BindServer("Show Shrine Restack Messages", true, "<style=cShrine>{0} is... sequenced.</color>");
            cfgShowShrineHealingServer = BindServer("Show Shrine Healing Messages", false, "\"<style=cShrine>{0} is embraced by the healing warmth of the Woods.</color>");
            cfgShowShrineCombatServer = BindServer("Show Shrine Combat Messages", true, "<style=cShrine>{0} has summoned {1}s to fight.</color>");
            cfgShowArenaEndServer = BindServer("Show Arena End Messages", false, "<style=cWorldEvent>The Cell stabilizes.</style>");
            cfgShowPetFrogServer = BindServer("Show Pet Frog Messages", true, "{0} pet the frog.");
            #endregion
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void ModCompat_RiskOfOptions()
        {
            static void A(ConfigEntry<bool> configEntry)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(configEntry, new CheckBoxConfig()
                {
                    category = "Server"
                }));
            }
            static void A2(ConfigEntry<FilterType> entry)
            {
                ModSettingsManager.AddOption(new ChoiceOption(entry, new ChoiceConfig()
                {
                    category = "Client"
                }));
            }
            A2(cfgShowPlayerPickupMessagesClient);
            A2(cfgShowDeathMessagesClient);
            A2(cfgShowJoinMessagesClient);
            A2(cfgShowLeaveMessagesClient);
            A(cfgShowNPCPickupMessagesClient);
            A2(cfgShowAhoyClient);
            A(cfgShowNpcClient);
            A2(cfgShowAchievementClient);
            A(cfgShowFamilyClient);
            A2(cfgShowTeleporterActivationClient);
            A2(cfgShowSuppressorClient);
            A(cfgShowPortalShopWillOpenClient);
            A(cfgShowPortalGoldshoresWillOpenClient);
            A(cfgShowPortalMSWillOpenClient);
            A(cfgShowPortalShopOpenClient);
            A(cfgShowPortalGoldshoresOpenClient);
            A(cfgShowPortalMSOpenClient);
            A(cfgShowMountainTeleporterClient);
            A2(cfgShowShrineChanceWinClient);
            A2(cfgShowShrineChanceFailClient);
            A(cfgShowSeerClient);
            A2(cfgShowShrineBossClient);
            A2(cfgShowShrineBloodClient);
            A2(cfgShowShrineRestackClient);
            A2(cfgShowShrineHealingClient);
            A2(cfgShowShrineCombatClient);
            A(cfgShowArenaEndClient);
            A2(cfgShowPetFrogClient);

            static void B(ConfigEntry<bool> configEntry)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(configEntry, new CheckBoxConfig()
                {
                    category = "Server"
                }));
            }
            B(cfgShowDeathMessagesServer);
            B(cfgShowJoinMessagesServer);
            B(cfgShowLeaveMessagesServer);
            B(cfgShowNPCPickupMessagesServer);
            B(cfgShowAhoyServer);
            B(cfgShowNpcServer);
            B(cfgShowAchievementServer);
            B(cfgShowFamilyServer);
            B(cfgShowTeleporterActivationServer);
            B(cfgShowSuppressorServer);
            B(cfgShowPortalShopWillOpenServer);
            B(cfgShowPortalGoldshoresWillOpenServer);
            B(cfgShowPortalMSWillOpenServer);
            B(cfgShowPortalShopOpenServer);
            B(cfgShowPortalGoldshoresOpenServer);
            B(cfgShowPortalMSOpenServer);
            B(cfgShowMountainTeleporterServer);
            B(cfgShowShrineChanceWinServer);
            B(cfgShowShrineChanceFailServer);
            B(cfgShowSeerServer);
            B(cfgShowShrineBossServer);
            B(cfgShowShrineBloodServer);
            B(cfgShowShrineRestackServer);
            B(cfgShowShrineHealingServer);
            B(cfgShowShrineCombatServer);
            B(cfgShowArenaEndServer);
            B(cfgShowPetFrogServer);
        }

        private System.Collections.IEnumerator ClassicStageInfo_BroadcastFamilySelection(On.RoR2.ClassicStageInfo.orig_BroadcastFamilySelection orig, ClassicStageInfo self, string familySelectionChatString)
        {
            return null;
        }

        //server
        private void ChatFilterServer(On.RoR2.Chat.orig_SendBroadcastChat_ChatMessageBase orig, ChatMessageBase message)
        {
            bool showMessage = true;

            switch (message)
            {
                case Chat.PlayerDeathChatMessage _:
                    showMessage = cfgShowDeathMessagesServer.Value;
                    break;
                case Chat.PlayerChatMessage chatMsg:
                    switch (chatMsg.baseToken)
                    {
                        case "PLAYER_CONNECTED":
                            showMessage = cfgShowJoinMessagesServer.Value;
                            break;
                        case "PLAYER_DISCONNECTED":
                            showMessage = cfgShowLeaveMessagesServer.Value;
                            break;
                    }
                    break;
                case Chat.PlayerPickupChatMessage playerPickupChatMessage:
                    switch (playerPickupChatMessage.baseToken)
                    {
                        //INFINITETOWER_ADD_ITEM
                        //VOIDCAMP_COMPLETE
                        case "MONSTER_PICKUP":
                            showMessage = cfgShowNPCPickupMessagesServer.Value;
                            break;
                    }
                    break;
                case Chat.BodyChatMessage bodyChatMessage:
                    showMessage = bodyChatMessage.token switch
                    {
                        "EQUIPMENT_BOSSHUNTERCONSUMED_CHAT" => cfgShowAhoyServer.Value,
                        _ => true
                    };
                    break;
                case Chat.NpcChatMessage _:
                    showMessage = cfgShowNpcServer.Value;
                    break;
                case ColoredTokenChatMessage coloredTokenChatMessage:
                    showMessage = coloredTokenChatMessage.baseToken switch
                    {
                        "VOID_SUPPRESSOR_USE_MESSAGE" => cfgShowSuppressorServer.Value,
                        _ => true
                    };
                    break;
                case Chat.SubjectFormatChatMessage subjectFormatChatMessage:
                    showMessage = subjectFormatChatMessage.baseToken switch
                    {
                        "ACHIEVEMENT_UNLOCKED_MESSAGE" => cfgShowAchievementServer.Value,
                        "SHRINE_CHANCE_FAIL_MESSAGE" => cfgShowShrineChanceFailServer.Value,
                        "SHRINE_CHANCE_SUCCESS_MESSAGE" => cfgShowShrineChanceWinServer.Value,
                        "SHRINE_BOSS_USE_MESSAGE" => cfgShowShrineBossServer.Value,
                        "SHRINE_BLOOD_USE_MESSAGE" => cfgShowShrineBloodServer.Value,
                        "SHRINE_RESTACK_USE_MESSAGE" => cfgShowShrineRestackServer.Value,
                        "SHRINE_HEALING_USE_MESSAGE" => cfgShowShrineHealingServer.Value,
                        "SHRINE_COMBAT_USE_MESSAGE" => cfgShowShrineCombatServer.Value,
                        _ => true
                    };
                    break;
                case SubjectChatMessage subjectChatMessage:
                    showMessage = subjectChatMessage.baseToken switch
                    {
                        "PLAYER_ACTIVATED_TELEPORTER" => cfgShowTeleporterActivationServer.Value,
                        "PET_FROG" => cfgShowPetFrogServer.Value,
                        _ => true
                    };
                    break;
                case Chat.SimpleChatMessage simpleChatMessage:
                    //InfiniteTower: stageTransitionChatToken
                    //InfiniteTower: beginChatToken
                    //InfiniteTower: suddenDeathChatToken
                    //STONEGATE_OPEN
                    //LUNAR_TELEPORTER_IDLE
                    //LUNAR_TELEPORTER_ACTIVE
                    //VULTURE_EGG_WARNING
                    //VULTURE_EGG_BEGIN
                    showMessage = simpleChatMessage.baseToken switch
                    {
                        "PORTAL_SHOP_WILL_OPEN" => cfgShowPortalShopWillOpenServer.Value,
                        "PORTAL_GOLDSHORES_WILL_OPEN" => cfgShowPortalGoldshoresWillOpenServer.Value,
                        "PORTAL_MS_WILL_OPEN" => cfgShowPortalMSWillOpenServer.Value,
                        "SHRINE_BOSS_BEGIN_TRIAL" => cfgShowMountainTeleporterServer.Value,
                        "PORTAL_SHOP_OPEN" => cfgShowPortalMSOpenServer.Value,
                        "PORTAL_GOLDSHORES_OPEN" => cfgShowPortalGoldshoresOpenServer.Value,
                        "PORTAL_MS_OPEN" => cfgShowPortalMSOpenServer.Value,
                        "ARENA_END" => cfgShowArenaEndServer.Value,
                        _ => simpleChatMessage.baseToken.StartsWith("BAZAAR_SEER_")
                            ? cfgShowSeerServer.Value
                            : true
                    };
                    break;
            }
            if (showMessage)
            {
                orig(message);
            }
        }
    }
}
