using System;
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
using UnityEngine;
using UnityEngine.Networking;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace ror2ChatFilterMod
{
    [BepInPlugin("com.DestroyedClone.RoR2ChatFilterMod", "Chat Filter Mod", "0.0.1")]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    public class Main : BaseUnityPlugin
    {
        //public ConfigEntry<ChatMessageType> cfgShowPickupMessages;
        //public ConfigEntry<bool> cfgTimeStamp;

        //public ConfigEntry<bool> cfgShowDeathMessagesClient;
        public static ConfigEntry<bool> cfgShowPingMoveClient;
        public static ConfigEntry<bool> cfgShowPingAttackClient;
        public static ConfigEntry<bool> cfgShowPingInteractableClient;
        public static ConfigEntry<bool> cfgShowPingInteractableWithCostClient;

        public static ConfigEntry<bool> cfgShowDeathMessagesServer;
        public static ConfigEntry<bool> cfgShowJoinMessagesServer;
        public static ConfigEntry<bool> cfgShowLeaveMessagesServer;
        public static ConfigEntry<bool> cfgShowPlayerPickupMessagesServer;
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
        public static ConfigEntry<bool> cfgShowServer;

        public static CharacterBody characterBody;

        public static Dictionary<string, string> langName_to_suffixDefault = new Dictionary<string, string>();
        public static Dictionary<string, string> langName_to_suffixEnemy = new Dictionary<string, string>();
        public static Dictionary<string, string> langName_to_suffixInteractable = new Dictionary<string, string>();
        public static Dictionary<string, string> langName_to_suffixInteractableWithCost = new Dictionary<string, string>();

        public static Dictionary<string, bool> subjectFormatChatMessage_to_config = new Dictionary<string, bool>()
        {

        };

        public static string currentLangName = "en";

        /*public enum ChatMessageType
        {
            enabled,
            merge,
            disabled
        }*/

        public void Setup()
        {
            langName_to_suffixDefault.Clear();
            langName_to_suffixEnemy.Clear();
            langName_to_suffixInteractable.Clear();
            langName_to_suffixInteractableWithCost.Clear();
            string Token(string token)
            {
                return token.Substring(token.IndexOf("}") + 1);
            }
            foreach (var lang in RoR2.Language.steamLanguageTable)
            {
                var langName = lang.Value.webApiName;
                var text1 = Language.GetString("PLAYER_PING_DEFAULT", langName);
                var text2 = Language.GetString("PLAYER_PING_ENEMY", langName);
                var text3 = Language.GetString("PLAYER_PING_INTERACTABLE", langName);
                var text4 = Language.GetString("PLAYER_PING_INTERACTABLE_WITH_COST", langName);

                langName_to_suffixDefault[langName] = Token(text1);
                langName_to_suffixEnemy[langName] = Token(text2);
                langName_to_suffixInteractable[langName] = Token(text3);
                langName_to_suffixInteractableWithCost[langName] = Token(text4);
            }
        }


        public void Start()
        {
            Setup();
            SetupConfig();

            On.RoR2.Chat.SendBroadcastChat_ChatMessageBase += Chat_SendBroadcastChat_ChatMessageBase;
            On.RoR2.Chat.AddMessage_string += Chat_AddMessage_string;

            //server

            Language.onCurrentLanguageChanged += Language_onCurrentLanguageChanged;
                //On.RoR2.GenericPickupController.SendPickupMessage += GenericPickupController_SendPickupMessage;
            if (!cfgShowFamilyServer.Value)
                On.RoR2.ClassicStageInfo.BroadcastFamilySelection += ClassicStageInfo_BroadcastFamilySelection;

            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                ModCompat_RiskOfOptions();
            }
        }
        public void SetupConfig()
        {
            #region Server
            ConfigEntry<bool> BindServer(string k, bool t, string d)
            {
                return Config.Bind("Server", k, t, d);
            }
            cfgShowDeathMessagesServer = BindServer("Show Death Messages", true, "");
            cfgShowJoinMessagesServer = BindServer("Show Join Messages", true, "<style=cEvent>{0} connected.</color>");
            cfgShowLeaveMessagesServer = BindServer("Show Leave Messages", true, "<style=cEvent>{0} disconnected.</color>");
            cfgShowPlayerPickupMessagesServer = BindServer("Show Player Pickup Messages", false, "<style=cEvent>{0} picked up {1}{2}</color>");
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
            cfgShowServer = BindServer("Show Server Messages", true, "");
            #endregion
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void ModCompat_RiskOfOptions()
        {
            void A(ConfigEntry<bool> configEntry)
            {
                ModSettingsManager.AddOption(new CheckBoxOption(configEntry, new CheckBoxConfig()
                {
                    category = "Server"
                }));
            }
            A(cfgShowDeathMessagesServer);
            A(cfgShowJoinMessagesServer);
            A(cfgShowLeaveMessagesServer);
            A(cfgShowPlayerPickupMessagesServer);
            A(cfgShowNPCPickupMessagesServer);
            A(cfgShowAhoyServer);
            A(cfgShowNpcServer);
            A(cfgShowAchievementServer);
            A(cfgShowFamilyServer);
            A(cfgShowTeleporterActivationServer);
            A(cfgShowSuppressorServer);
            A(cfgShowPortalShopWillOpenServer);
            A(cfgShowPortalGoldshoresWillOpenServer);
            A(cfgShowPortalMSWillOpenServer);
            A(cfgShowPortalShopOpenServer);
            A(cfgShowPortalGoldshoresOpenServer);
            A(cfgShowPortalMSOpenServer);
            A(cfgShowMountainTeleporterServer);
            A(cfgShowShrineChanceWinServer);
            A(cfgShowShrineChanceFailServer);
            A(cfgShowSeerServer);
            A(cfgShowShrineBossServer);
            A(cfgShowShrineBloodServer);
            A(cfgShowShrineRestackServer);
            A(cfgShowShrineHealingServer);
            A(cfgShowShrineCombatServer);
            A(cfgShowArenaEndServer);
            A(cfgShowPetFrogServer);
            A(cfgShowServer);
        }

        private System.Collections.IEnumerator ClassicStageInfo_BroadcastFamilySelection(On.RoR2.ClassicStageInfo.orig_BroadcastFamilySelection orig, ClassicStageInfo self, string familySelectionChatString)
        {
            return null;
        }

        private void Language_onCurrentLanguageChanged()
        {
            currentLangName = Language.currentLanguageName;
        }

        private void Chat_AddMessage_string(On.RoR2.Chat.orig_AddMessage_string orig, string message)
        {
            if (message.EndsWith(">"))
            {
                //funny PingIndicator
                if (message.EndsWith(langName_to_suffixDefault[currentLangName]) && !cfgShowPingMoveClient.Value)
                {
                    return;
                }
                else if (message.EndsWith(langName_to_suffixEnemy[currentLangName]) && !cfgShowPingAttackClient.Value)
                {
                    return;
                }
                else if (message.EndsWith(langName_to_suffixInteractable[currentLangName]) && !cfgShowPingInteractableClient.Value)
                {
                    return;
                }
                else if (message.EndsWith(langName_to_suffixInteractableWithCost[currentLangName]) && !cfgShowPingInteractableWithCostClient.Value)
                {
                    return;
                }
            }
            orig(message);
        }

        //server
        private void Chat_SendBroadcastChat_ChatMessageBase(On.RoR2.Chat.orig_SendBroadcastChat_ChatMessageBase orig, ChatMessageBase message)
        {
            bool showMessage = true;

            switch (message)
            {
                case Chat.PlayerDeathChatMessage playerDeathChatMessage:
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
                        case "PLAYER_PICKUP":
                            showMessage = cfgShowPlayerPickupMessagesServer.Value;
                            break;
                        case "MONSTER_PICKUP":
                            showMessage = cfgShowNPCPickupMessagesServer.Value;
                            break;
                        case "VOIDCAMP_COMPLETE":
                            showMessage = true;
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
                case Chat.NpcChatMessage npcChatMessage:
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
