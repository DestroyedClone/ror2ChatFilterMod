using BepInEx;
using BepInEx.Configuration;
using RoR2;
using System.Security;
using System.Security.Permissions;

[module: UnverifiableCode]
#pragma warning disable CS0618 // Type or member is obsolete
[assembly: SecurityPermission(SecurityAction.RequestMinimum, SkipVerification = true)]
#pragma warning restore CS0618 // Type or member is obsolete

namespace ror2ChatFilterMod
{
    [BepInPlugin("com.DestroyedClone.RoR2ChatFilterMod", "Chat Filter Mod", "1.0.0")]
    [BepInDependency("com.rune580.riskofoptions", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.justinderby.vstwitch", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.TheBestAssociatedLargelyLudicrousSillyheadGroup.GOTCE", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Heyimnoop.NemesisSlab", BepInDependency.DependencyFlags.SoftDependency)] //for gotce shit
    [BepInDependency("com.HIFU.UltimateCustomRun", BepInDependency.DependencyFlags.SoftDependency)] //deprecated
    [BepInDependency("KosmosisDire.TeammateRevival", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.ThinkInvisible.TinkersSatchel", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("SylmarDev.SpireItems", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("dev.wildbook.multitudes", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.evaisa.moreshrines", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Nuxlar.UmbralMithrix", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.niwith.DropInMultiplayer", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.PopcornFactory.DarthVaderMod", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Viliger.ShrineOfRepair", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("BALLS.WellRoundedBalance", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("Gorakh.RiskOfChaos", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.funkfrog_sipondo.sharesuite", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Moffein.BossKillTimer", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.rob.Direseeker", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("local.difficulty.multitudes", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.ContactLight.LostInTransit", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.Zenithrium.vanillaVoid", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.themysticsword.mysticsitems", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.ChirrLover.Starstorm2Unofficial", BepInDependency.DependencyFlags.SoftDependency)]
    [BepInDependency("com.justinderby.bossantisoftlock", BepInDependency.DependencyFlags.SoftDependency)]
    public class Main : BaseUnityPlugin
    {
        public static ConfigEntry<string> cfgTimestampFormat;

        public static ConfigEntry<ChatFilterType> cfgShowPlayerPickupMessagesClient;
        public static ConfigEntry<ChatFilterType> cfgShowDeathMessagesClient;
        public static ConfigEntry<ChatFilterType> cfgShowJoinMessagesClient;
        public static ConfigEntry<ChatFilterType> cfgShowLeaveMessagesClient;
        public static ConfigEntry<bool> cfgShowNPCPickupMessagesClient;
        public static ConfigEntry<ChatFilterType> cfgShowAhoyClient;
        public static ConfigEntry<bool> cfgShowNpcClient;
        public static ConfigEntry<ChatFilterType> cfgShowAchievementClient;
        public static ConfigEntry<bool> cfgShowFamilyClient;
        public static ConfigEntry<ChatFilterType> cfgShowTeleporterActivationClient;
        public static ConfigEntry<ChatFilterType> cfgShowSuppressorClient;
        public static ConfigEntry<bool> cfgShowPortalShopWillOpenClient;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresWillOpenClient;
        public static ConfigEntry<bool> cfgShowPortalMSWillOpenClient;
        public static ConfigEntry<bool> cfgShowPortalShopOpenClient;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresOpenClient;
        public static ConfigEntry<bool> cfgShowPortalMSOpenClient;
        public static ConfigEntry<bool> cfgShowMountainTeleporterClient;
        public static ConfigEntry<ChatFilterType> cfgShowShrineChanceWinClient;
        public static ConfigEntry<ChatFilterType> cfgShowShrineChanceFailClient;
        public static ConfigEntry<bool> cfgShowSeerClient;
        public static ConfigEntry<ChatFilterType> cfgShowShrineBossClient;
        public static ConfigEntry<ChatFilterType> cfgShowShrineBloodClient;
        public static ConfigEntry<ChatFilterType> cfgShowShrineRestackClient;
        public static ConfigEntry<ChatFilterType> cfgShowShrineHealingClient;
        public static ConfigEntry<ChatFilterType> cfgShowShrineCombatClient;
        public static ConfigEntry<bool> cfgShowArenaEndClient;
        public static ConfigEntry<ChatFilterType> cfgShowPetFrogClient;

        public enum ChatFilterType
        {
            All,
            Myself,
            Others,
            Off
        }

        public void Awake()
        {
            SetupConfig();

            On.RoR2.Chat.AddMessage_ChatMessageBase += ChatFilterClient;
            ModCompat.Initialize(Config);
        }

        public static bool ShouldShowClient(ChatMessageBase chatMessage, ConfigEntry<ChatFilterType> configEntry)
        {
            if (chatMessage is Chat.PlayerPickupChatMessage playerPickupChatMessage)
                switch (configEntry.Value)
                {
                    case ChatFilterType.All:
                        return true;

                    case ChatFilterType.Myself:
                        return playerPickupChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Others:
                        return playerPickupChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Off:
                        return false;
                }
            else if (chatMessage is Chat.PlayerDeathChatMessage playerDeathChatMessage)
                switch (configEntry.Value)
                {
                    case ChatFilterType.All:
                        return true;

                    case ChatFilterType.Myself:
                        return playerDeathChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Others:
                        return playerDeathChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Off:
                        return false;
                }
            else if (chatMessage is Chat.PlayerChatMessage playerChatMessage)
                switch (configEntry.Value)
                {
                    case ChatFilterType.All:
                        return true;

                    case ChatFilterType.Myself:
                        return playerChatMessage.networkPlayerName.steamId == LocalUserManager.GetFirstLocalUser().currentNetworkUser.GetNetworkPlayerName().steamId;

                    case ChatFilterType.Others:
                        return playerChatMessage.networkPlayerName.steamId != LocalUserManager.GetFirstLocalUser().currentNetworkUser.GetNetworkPlayerName().steamId;

                    case ChatFilterType.Off:
                        return false;
                }
            else if (chatMessage is Chat.BodyChatMessage bodyChatMessage)
            {
                switch (configEntry.Value)
                {
                    case ChatFilterType.All:
                        return true;

                    case ChatFilterType.Myself:
                        return bodyChatMessage.bodyObject == LocalUserManager.GetFirstLocalUser().cachedBody.gameObject;

                    case ChatFilterType.Others:
                        return bodyChatMessage.bodyObject != LocalUserManager.GetFirstLocalUser().cachedBody.gameObject;

                    case ChatFilterType.Off:
                        return false;
                }
            }
            else if (chatMessage is ColoredTokenChatMessage coloredTokenChatMessage)
            {
                switch (configEntry.Value)
                {
                    case ChatFilterType.All:
                        return true;

                    case ChatFilterType.Myself:
                        return coloredTokenChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Others:
                        return coloredTokenChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Off:
                        return false;
                }
            }
            else if (chatMessage is Chat.SubjectFormatChatMessage subjectFormatChatMessage)
            {
                switch (configEntry.Value)
                {
                    case ChatFilterType.All:
                        return true;

                    case ChatFilterType.Myself:
                        return subjectFormatChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Others:
                        return subjectFormatChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Off:
                        return false;
                }
            }
            else if (chatMessage is SubjectChatMessage subjectChatMessage)
            {
                switch (configEntry.Value)
                {
                    case ChatFilterType.All:
                        return true;

                    case ChatFilterType.Myself:
                        return subjectChatMessage.subjectAsCharacterBody == LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Others:
                        return subjectChatMessage.subjectAsCharacterBody != LocalUserManager.GetFirstLocalUser().cachedBody;

                    case ChatFilterType.Off:
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
                case Chat.UserChatMessage userChatMessage:
                    if (showMessage)
                    {
                        ModCompat.ModCompatCheck_UserChatMessage(userChatMessage);
                    }
                    break;

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
                    if (showMessage)
                        ModCompat.ModCompatCheck_BodyChatMessage(bodyChatMessage);
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
                    if (showMessage)
                        showMessage = ModCompat.ModCompatCheck_ColoredTokenChatMessage(coloredTokenChatMessage);
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
                    if (showMessage) //check for true, if its false its already been filtered
                    {
                        showMessage = ModCompat.ModCompatCheck_SubjectFormatChatMessage(subjectFormatChatMessage);
                    }
                    break;

                case SubjectChatMessage subjectChatMessage:
                    showMessage = subjectChatMessage.baseToken switch
                    {
                        "PLAYER_ACTIVATED_TELEPORTER" => ShouldShowClient(subjectChatMessage, cfgShowTeleporterActivationClient),
                        "PET_FROG" => ShouldShowClient(subjectChatMessage, cfgShowPetFrogClient),
                        _ => true
                    };
                    if (showMessage)
                        showMessage = ModCompat.ModCompatCheck_SubjectChatMessage(subjectChatMessage);
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
                    if (showMessage) //check for true, if its false its already been filtered
                    {
                        showMessage = ModCompat.ModCompatCheck_SimpleChatMessage(simpleChatMessage);
                    }
                    break;
            }

            if (showMessage)
                orig(message);
        }

        public void SetupConfig()
        {
            #region Client

            var clientKey = "Client";
            ConfigEntry<bool> BindClient(string k, bool t, string d)
            {
                return Config.Bind(clientKey, k, t, d);
            }
            ConfigEntry<ChatFilterType> BindClient2(string k, ChatFilterType t, string d)
            {
                return Config.Bind(clientKey, k, t, d);
            }
            cfgShowPlayerPickupMessagesClient = Config.Bind(clientKey, "Player Pickup Messages", ChatFilterType.All, "<style=cEvent>{0} picked up {1}{2}</color>");
            cfgTimestampFormat = Config.Bind(clientKey, "Timestamp Format", "{0} {1}", "First parameter is the timestamp, the second is the original message. Leave empty to disable.");

            cfgShowDeathMessagesClient = BindClient2("Death Messages", ChatFilterType.All, "");
            cfgShowJoinMessagesClient = BindClient2("Join Messages", ChatFilterType.All, "<style=cEvent>{0} connected.</color>");
            cfgShowLeaveMessagesClient = BindClient2("Leave Messages", ChatFilterType.All, "<style=cEvent>{0} disconnected.</color>");
            cfgShowNPCPickupMessagesClient = BindClient("NPC Pickup Messages", true, "Scav pickups, void fields item adds");
            cfgShowAhoyClient = BindClient2("Ahoy Messages", ChatFilterType.All, "Ahoy!");
            cfgShowNpcClient = BindClient("NPC Messages", true, "Mithrix messages");
            cfgShowAchievementClient = BindClient2("Achievement Messages", ChatFilterType.All, "<color=#ccd3e0>{0} achieved <color=#BDE151>{1}</color></color>");
            cfgShowFamilyClient = BindClient("Family Messages", true, "");
            cfgShowTeleporterActivationClient = BindClient2("Teleporter Activation Messages", ChatFilterType.All, "<style=cEvent>{0} activated the <style=cDeath>Teleporter <sprite name=\"TP\" tint=1></style>.</style>");
            cfgShowSuppressorClient = BindClient2("Suppressor Messages", ChatFilterType.All, "<style=cShrine>{0} eradicated {1} from the universe.");
            cfgShowPortalShopWillOpenClient = BindClient("Portal Shop Will Open Messages", true, "<style=cWorldEvent>A blue orb appears..</style>");
            cfgShowPortalGoldshoresWillOpenClient = BindClient("Portal Goldshores Will Open Messages", true, "<style=cWorldEvent>A gold orb appears..</style>");
            cfgShowPortalMSWillOpenClient = BindClient("Portal MS Will Open Messages", true, "<style=cWorldEvent>A celestial orb appears..</style>");
            cfgShowPortalShopOpenClient = BindClient("Portal Shop Open Messages", true, "<style=cWorldEvent>A blue portal appears..</style>");
            cfgShowPortalGoldshoresOpenClient = BindClient("Portal Goldshores Open Messages", true, "<style=cWorldEvent>A gold portal appears..</style>");
            cfgShowPortalMSOpenClient = BindClient("Portal MS Open Messages", true, "<style=cWorldEvent>A celestial portal appears..</style>");
            cfgShowMountainTeleporterClient = BindClient("Mountain Teleporter Messages", true, "<style=cShrine>Let the challenge of the Mountain... begin!</style>");
            cfgShowShrineChanceWinClient = BindClient2("Shrine Chance Win Messages", ChatFilterType.All, "<style=cShrine>{0} offered to the shrine and was rewarded!</color>");
            cfgShowShrineChanceFailClient = BindClient2("Shrine Chance Fail Messages", ChatFilterType.All, "<style=cShrine>{0} offered to the shrine and gained nothing.</color>");
            cfgShowSeerClient = BindClient("Seer Messages", true, "<style=cWorldEvent>You dream of STAGEHINT.</style>");
            cfgShowShrineBossClient = BindClient2("Shrine Boss Messages", ChatFilterType.All, "<style=cShrine>{0} has invited the challenge of the Mountain..</color>");
            cfgShowShrineBloodClient = BindClient2("Shrine Blood Messages", ChatFilterType.All, "<style=cShrine>{0} feels a searing pain, and has gained {1} gold.</color>");
            cfgShowShrineRestackClient = BindClient2("Shrine Restack Messages", ChatFilterType.All, "<style=cShrine>{0} is... sequenced.</color>");
            cfgShowShrineHealingClient = BindClient2("Shrine Healing Messages", ChatFilterType.All, "\"<style=cShrine>{0} is embraced by the healing warmth of the Woods.</color>");
            cfgShowShrineCombatClient = BindClient2("Shrine Combat Messages", ChatFilterType.All, "<style=cShrine>{0} has summoned {1}s to fight.</color>");
            cfgShowArenaEndClient = BindClient("Arena End Messages", false, "<style=cWorldEvent>The Cell stabilizes.</style>");
            cfgShowPetFrogClient = BindClient2("Pet Frog Messages", ChatFilterType.All, "{0} pet the frog.");

            #endregion Client
        }
    }
}