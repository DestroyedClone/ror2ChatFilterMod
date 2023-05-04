using BepInEx.Configuration;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RiskOfOptions;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using static ror2ChatFilterMod.Main;
using BepInEx;
using BepInEx.Configuration;
using RiskOfOptions;
using RiskOfOptions.OptionConfigs;
using RiskOfOptions.Options;
using RoR2;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Permissions;
using UnityEngine;

namespace ror2ChatFilterMod
{
    public class Server
    {

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

        public void Initialize()
        {
            On.RoR2.Chat.SendBroadcastChat_ChatMessageBase += ChatFilterServer;
            if (!cfgShowFamilyServer.Value)
                On.RoR2.ClassicStageInfo.BroadcastFamilySelection += ClassicStageInfo_BroadcastFamilySelection;


            if (BepInEx.Bootstrap.Chainloader.PluginInfos.ContainsKey("com.rune580.riskofoptions"))
            {
                ModCompat_RiskOfOptionsServer();
            }
        }

        public void SetupConfigServer(ConfigFile Config)
        {
            ConfigEntry<bool> BindServer(string k, bool t, string d)
            {
                return Config.Bind("Server", k, t, d);
            }
            cfgShowDeathMessagesServer = BindServer("Death Messages", true, "");
            cfgShowJoinMessagesServer = BindServer("Join Messages", true, "<style=cEvent>{0} connected.</color>");
            cfgShowLeaveMessagesServer = BindServer("Leave Messages", true, "<style=cEvent>{0} disconnected.</color>");
            cfgShowNPCPickupMessagesServer = BindServer("NPC Pickup Messages", true, "Scav pickups, void fields item adds");
            cfgShowAhoyServer = BindServer("Ahoy Messages", false, "Ahoy!");
            cfgShowNpcServer = BindServer("NPC Messages", true, "Mithrix messages");
            cfgShowAchievementServer = BindServer("Achievement Messages", false, "<color=#ccd3e0>{0} achieved <color=#BDE151>{1}</color></color>");
            cfgShowFamilyServer = BindServer("Family Messages", true, "");
            cfgShowTeleporterActivationServer = BindServer("Teleporter Activation Messages", true, "<style=cEvent>{0} activated the <style=cDeath>Teleporter <sprite name=\"TP\" tint=1></style>.</style>");
            cfgShowSuppressorServer = BindServer("Suppressor Messages", true, "<style=cShrine>{0} eradicated {1} from the universe.");
            cfgShowPortalShopWillOpenServer = BindServer("Portal Shop Will Open Messages", true, "<style=cWorldEvent>A blue orb appears..</style>");
            cfgShowPortalGoldshoresWillOpenServer = BindServer("Portal Goldshores Will Open Messages", true, "<style=cWorldEvent>A gold orb appears..</style>");
            cfgShowPortalMSWillOpenServer = BindServer("Portal MS Will Open Messages", true, "<style=cWorldEvent>A celestial orb appears..</style>");
            cfgShowPortalShopOpenServer = BindServer("Portal Shop Open Messages", true, "<style=cWorldEvent>A blue portal appears..</style>");
            cfgShowPortalGoldshoresOpenServer = BindServer("Portal Goldshores Open Messages", true, "<style=cWorldEvent>A gold portal appears..</style>");
            cfgShowPortalMSOpenServer = BindServer("Portal MS Open Messages", true, "<style=cWorldEvent>A celestial portal appears..</style>");
            cfgShowMountainTeleporterServer = BindServer("Mountain Teleporter Messages", true, "<style=cShrine>Let the challenge of the Mountain... begin!</style>");
            cfgShowShrineChanceWinServer = BindServer("Shrine Chance Win Messages", false, "<style=cShrine>{0} offered to the shrine and was rewarded!</color>");
            cfgShowShrineChanceFailServer = BindServer("Shrine Chance Fail Messages", false, "<style=cShrine>{0} offered to the shrine and gained nothing.</color>");
            cfgShowSeerServer = BindServer("Seer Messages", true, "<style=cWorldEvent>You dream of STAGEHINT.</style>");
            cfgShowShrineBossServer = BindServer("Shrine Boss Messages", true, "<style=cShrine>{0} has invited the challenge of the Mountain..</color>");
            cfgShowShrineBloodServer = BindServer("Shrine Blood Messages", false, "<style=cShrine>{0} feels a searing pain, and has gained {1} gold.</color>");
            cfgShowShrineRestackServer = BindServer("Shrine Restack Messages", true, "<style=cShrine>{0} is... sequenced.</color>");
            cfgShowShrineHealingServer = BindServer("Shrine Healing Messages", false, "\"<style=cShrine>{0} is embraced by the healing warmth of the Woods.</color>");
            cfgShowShrineCombatServer = BindServer("Shrine Combat Messages", true, "<style=cShrine>{0} has summoned {1}s to fight.</color>");
            cfgShowArenaEndServer = BindServer("Arena End Messages", false, "<style=cWorldEvent>The Cell stabilizes.</style>");
            cfgShowPetFrogServer = BindServer("Pet Frog Messages", true, "{0} pet the frog.");
        }


        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private static void ModCompat_RiskOfOptionsServer()
        {
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

        public System.Collections.IEnumerator ClassicStageInfo_BroadcastFamilySelection(On.RoR2.ClassicStageInfo.orig_BroadcastFamilySelection orig, ClassicStageInfo self, string familySelectionChatString)
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
