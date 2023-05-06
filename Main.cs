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

        public static ConfigEntry<ChatFilterType> cfgShowPlayerPickupMessages;
        public static ConfigEntry<ChatFilterType> cfgShowDeathMessages;
        public static ConfigEntry<ChatFilterType> cfgShowJoinMessages;
        public static ConfigEntry<ChatFilterType> cfgShowLeaveMessages;
        public static ConfigEntry<bool> cfgShowNPCPickupMessages;
        public static ConfigEntry<ChatFilterType> cfgShowAhoy;
        public static ConfigEntry<bool> cfgShowNpc;
        public static ConfigEntry<ChatFilterType> cfgShowAchievement;
        public static ConfigEntry<bool> cfgShowFamily;
        public static ConfigEntry<ChatFilterType> cfgShowTeleporterActivation;
        public static ConfigEntry<ChatFilterType> cfgShowSuppressor;
        public static ConfigEntry<bool> cfgShowPortalShopWillOpen;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresWillOpen;
        public static ConfigEntry<bool> cfgShowPortalMSWillOpen;
        public static ConfigEntry<bool> cfgShowPortalShopOpen;
        public static ConfigEntry<bool> cfgShowPortalGoldshoresOpen;
        public static ConfigEntry<bool> cfgShowPortalMSOpen;
        public static ConfigEntry<bool> cfgShowMountainTeleporter;
        public static ConfigEntry<ChatFilterType> cfgShowShrineChanceWin;
        public static ConfigEntry<ChatFilterType> cfgShowShrineChanceFail;
        public static ConfigEntry<bool> cfgShowSeer;
        public static ConfigEntry<ChatFilterType> cfgShowShrineBoss;
        public static ConfigEntry<ChatFilterType> cfgShowShrineBlood;
        public static ConfigEntry<ChatFilterType> cfgShowShrineRestack;
        public static ConfigEntry<ChatFilterType> cfgShowShrineHealing;
        public static ConfigEntry<ChatFilterType> cfgShowShrineCombat;
        public static ConfigEntry<bool> cfgShowArenaEnd;
        public static ConfigEntry<ChatFilterType> cfgShowPetFrog;

        public enum ChatFilterType
        {
            All,
            Myself,
            Others,
            Off
        }

        public void Start()
        {
            SetupConfig();

            On.RoR2.Chat.AddMessage_ChatMessageBase += ChatFilter;
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

        private void ChatFilter(On.RoR2.Chat.orig_AddMessage_ChatMessageBase orig, ChatMessageBase message)
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
                            showMessage = ShouldShowClient(playerPickupChatMessage, cfgShowPlayerPickupMessages);
                            break;
                    }
                    break;

                case Chat.PlayerDeathChatMessage playerDeathChatMessage:
                    showMessage = ShouldShowClient(playerDeathChatMessage, cfgShowDeathMessages);
                    break;

                case Chat.PlayerChatMessage chatMsg:
                    switch (chatMsg.baseToken)
                    {
                        case "PLAYER_CONNECTED":
                            showMessage = ShouldShowClient(chatMsg, cfgShowJoinMessages);
                            break;

                        case "PLAYER_DISCONNECTED":
                            showMessage = ShouldShowClient(chatMsg, cfgShowLeaveMessages);
                            break;
                    }
                    break;

                case Chat.BodyChatMessage bodyChatMessage:
                    showMessage = bodyChatMessage.token switch
                    {
                        "EQUIPMENT_BOSSHUNTERCONSUMED_CHAT" => ShouldShowClient(bodyChatMessage, cfgShowAhoy),
                        _ => true
                    };
                    if (showMessage)
                        ModCompat.ModCompatCheck_BodyChatMessage(bodyChatMessage);
                    break;

                case Chat.NpcChatMessage _:
                    showMessage = cfgShowNpc.Value;
                    break;

                case ColoredTokenChatMessage coloredTokenChatMessage:
                    showMessage = coloredTokenChatMessage.baseToken switch
                    {
                        "VOID_SUPPRESSOR_USE_MESSAGE" => ShouldShowClient(coloredTokenChatMessage, cfgShowSuppressor),
                        _ => true
                    };
                    if (showMessage)
                        showMessage = ModCompat.ModCompatCheck_ColoredTokenChatMessage(coloredTokenChatMessage);
                    break;

                case Chat.SubjectFormatChatMessage subjectFormatChatMessage:
                    showMessage = subjectFormatChatMessage.baseToken switch
                    {
                        "ACHIEVEMENT_UNLOCKED_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowAchievement),
                        "SHRINE_CHANCE_FAIL_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineChanceFail),
                        "SHRINE_CHANCE_SUCCESS_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineChanceWin),
                        "SHRINE_BOSS_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineBoss),
                        "SHRINE_BLOOD_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineBlood),
                        "SHRINE_RESTACK_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineRestack),
                        "SHRINE_HEALING_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineHealing),
                        "SHRINE_COMBAT_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShowShrineCombat),
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
                        "PLAYER_ACTIVATED_TELEPORTER" => ShouldShowClient(subjectChatMessage, cfgShowTeleporterActivation),
                        "PET_FROG" => ShouldShowClient(subjectChatMessage, cfgShowPetFrog),
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
                        "PORTAL_SHOP_WILL_OPEN" => cfgShowPortalShopWillOpen.Value,
                        "PORTAL_GOLDSHORES_WILL_OPEN" => cfgShowPortalGoldshoresWillOpen.Value,
                        "PORTAL_MS_WILL_OPEN" => cfgShowPortalMSWillOpen.Value,
                        "SHRINE_BOSS_BEGIN_TRIAL" => cfgShowMountainTeleporter.Value,
                        "PORTAL_SHOP_OPEN" => cfgShowPortalMSOpen.Value,
                        "PORTAL_GOLDSHORES_OPEN" => cfgShowPortalGoldshoresOpen.Value,
                        "PORTAL_MS_OPEN" => cfgShowPortalMSOpen.Value,
                        "ARENA_END" => cfgShowArenaEnd.Value,
                        _ => simpleChatMessage.baseToken.StartsWith("BAZAAR_SEER_")
                            ? cfgShowSeer.Value
                            : !simpleChatMessage.baseToken.StartsWith("FAMILY_")
                            || cfgShowFamily.Value
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
            #region 

            var clientKey = "";
            ConfigEntry<bool> Bind(string k, bool t, string d)
            {
                return Config.Bind(clientKey, k, t, d);
            }
            ConfigEntry<ChatFilterType> Bind2(string k, ChatFilterType t, string d)
            {
                return Config.Bind(clientKey, k, t, d);
            }
            cfgShowPlayerPickupMessages = Config.Bind(clientKey, "Player Pickup Messages", ChatFilterType.All, "<style=cEvent>{0} picked up {1}{2}</color>");
            cfgTimestampFormat = Config.Bind(clientKey, "Timestamp Format", "{0} {1}", "First parameter is the timestamp, the second is the original message. Leave empty to disable.");

            cfgShowDeathMessages = Bind2("Death Messages", ChatFilterType.All, "");
            cfgShowJoinMessages = Bind2("Join Messages", ChatFilterType.All, "<style=cEvent>{0} connected.</color>");
            cfgShowLeaveMessages = Bind2("Leave Messages", ChatFilterType.All, "<style=cEvent>{0} disconnected.</color>");
            cfgShowNPCPickupMessages = Bind("NPC Pickup Messages", true, "Scav pickups, void fields item adds");
            cfgShowAhoy = Bind2("Ahoy Messages", ChatFilterType.All, "Ahoy!");
            cfgShowNpc = Bind("NPC Messages", true, "Mithrix messages");
            cfgShowAchievement = Bind2("Achievement Messages", ChatFilterType.All, "<color=#ccd3e0>{0} achieved <color=#BDE151>{1}</color></color>");
            cfgShowFamily = Bind("Family Messages", true, "");
            cfgShowTeleporterActivation = Bind2("Teleporter Activation Messages", ChatFilterType.All, "<style=cEvent>{0} activated the <style=cDeath>Teleporter <sprite name=\"TP\" tint=1></style>.</style>");
            cfgShowSuppressor = Bind2("Suppressor Messages", ChatFilterType.All, "<style=cShrine>{0} eradicated {1} from the universe.");
            cfgShowPortalShopWillOpen = Bind("Portal Shop Will Open Messages", true, "<style=cWorldEvent>A blue orb appears..</style>");
            cfgShowPortalGoldshoresWillOpen = Bind("Portal Goldshores Will Open Messages", true, "<style=cWorldEvent>A gold orb appears..</style>");
            cfgShowPortalMSWillOpen = Bind("Portal MS Will Open Messages", true, "<style=cWorldEvent>A celestial orb appears..</style>");
            cfgShowPortalShopOpen = Bind("Portal Shop Open Messages", true, "<style=cWorldEvent>A blue portal appears..</style>");
            cfgShowPortalGoldshoresOpen = Bind("Portal Goldshores Open Messages", true, "<style=cWorldEvent>A gold portal appears..</style>");
            cfgShowPortalMSOpen = Bind("Portal MS Open Messages", true, "<style=cWorldEvent>A celestial portal appears..</style>");
            cfgShowMountainTeleporter = Bind("Mountain Teleporter Messages", true, "<style=cShrine>Let the challenge of the Mountain... begin!</style>");
            cfgShowShrineChanceWin = Bind2("Shrine Chance Win Messages", ChatFilterType.All, "<style=cShrine>{0} offered to the shrine and was rewarded!</color>");
            cfgShowShrineChanceFail = Bind2("Shrine Chance Fail Messages", ChatFilterType.All, "<style=cShrine>{0} offered to the shrine and gained nothing.</color>");
            cfgShowSeer = Bind("Seer Messages", true, "<style=cWorldEvent>You dream of STAGEHINT.</style>");
            cfgShowShrineBoss = Bind2("Shrine Boss Messages", ChatFilterType.All, "<style=cShrine>{0} has invited the challenge of the Mountain..</color>");
            cfgShowShrineBlood = Bind2("Shrine Blood Messages", ChatFilterType.All, "<style=cShrine>{0} feels a searing pain, and has gained {1} gold.</color>");
            cfgShowShrineRestack = Bind2("Shrine Restack Messages", ChatFilterType.All, "<style=cShrine>{0} is... sequenced.</color>");
            cfgShowShrineHealing = Bind2("Shrine Healing Messages", ChatFilterType.All, "\"<style=cShrine>{0} is embraced by the healing warmth of the Woods.</color>");
            cfgShowShrineCombat = Bind2("Shrine Combat Messages", ChatFilterType.All, "<style=cShrine>{0} has summoned {1}s to fight.</color>");
            cfgShowArenaEnd = Bind("Arena End Messages", false, "<style=cWorldEvent>The Cell stabilizes.</style>");
            cfgShowPetFrog = Bind2("Pet Frog Messages", ChatFilterType.All, "{0} pet the frog.");

            #endregion 
        }
    }
}