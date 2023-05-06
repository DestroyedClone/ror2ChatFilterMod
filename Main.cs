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

        public static ConfigEntry<ChatFilterType> cfgPlayerPickupMessages;
        public static ConfigEntry<bool> cfgNPCPickupMessages;
        public static ConfigEntry<ChatFilterType> cfgDeathMessages;
        public static ConfigEntry<ChatFilterType> cfgJoinMessages;
        public static ConfigEntry<ChatFilterType> cfgLeaveMessages;
        public static ConfigEntry<ChatFilterType> cfgAhoy;
        public static ConfigEntry<bool> cfgNpcMessages;
        public static ConfigEntry<ChatFilterType> cfgAchievement;
        public static ConfigEntry<bool> cfgFamily;
        public static ConfigEntry<ChatFilterType> cfgTeleporterActivation;
        public static ConfigEntry<ChatFilterType> cfgSuppressor;
        public static ConfigEntry<bool> cfgPortalShopWillOpen;
        public static ConfigEntry<bool> cfgPortalGoldshoresWillOpen;
        public static ConfigEntry<bool> cfgPortalMSWillOpen;
        public static ConfigEntry<bool> cfgPortalShopOpen;
        public static ConfigEntry<bool> cfgPortalGoldshoresOpen;
        public static ConfigEntry<bool> cfgPortalMSOpen;
        public static ConfigEntry<bool> cfgMountainTeleporter;
        public static ConfigEntry<ChatFilterType> cfgShrineChanceWin;
        public static ConfigEntry<ChatFilterType> cfgShrineChanceFail;
        public static ConfigEntry<bool> cfgSeer;
        public static ConfigEntry<ChatFilterType> cfgShrineBoss;
        public static ConfigEntry<ChatFilterType> cfgShrineBlood;
        public static ConfigEntry<ChatFilterType> cfgShrineRestack;
        public static ConfigEntry<ChatFilterType> cfgShrineHealing;
        public static ConfigEntry<ChatFilterType> cfgShrineCombat;
        public static ConfigEntry<bool> cfgArenaEnd;
        public static ConfigEntry<ChatFilterType> cfgPetFrog;

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
                            showMessage = ShouldShowClient(playerPickupChatMessage, cfgPlayerPickupMessages);
                            break;
                    }
                    break;

                case Chat.PlayerDeathChatMessage playerDeathChatMessage:
                    showMessage = ShouldShowClient(playerDeathChatMessage, cfgDeathMessages);
                    break;

                case Chat.PlayerChatMessage chatMsg:
                    switch (chatMsg.baseToken)
                    {
                        case "PLAYER_CONNECTED":
                            showMessage = ShouldShowClient(chatMsg, cfgJoinMessages);
                            break;

                        case "PLAYER_DISCONNECTED":
                            showMessage = ShouldShowClient(chatMsg, cfgLeaveMessages);
                            break;
                    }
                    break;

                case Chat.BodyChatMessage bodyChatMessage:
                    showMessage = bodyChatMessage.token switch
                    {
                        "EQUIPMENT_BOSSHUNTERCONSUMED_CHAT" => ShouldShowClient(bodyChatMessage, cfgAhoy),
                        _ => true
                    };
                    if (showMessage)
                        ModCompat.ModCompatCheck_BodyChatMessage(bodyChatMessage);
                    break;

                case Chat.NpcChatMessage _:
                    showMessage = cfgNpcMessages.Value;
                    break;

                case ColoredTokenChatMessage coloredTokenChatMessage:
                    showMessage = coloredTokenChatMessage.baseToken switch
                    {
                        "VOID_SUPPRESSOR_USE_MESSAGE" => ShouldShowClient(coloredTokenChatMessage, cfgSuppressor),
                        _ => true
                    };
                    if (showMessage)
                        showMessage = ModCompat.ModCompatCheck_ColoredTokenChatMessage(coloredTokenChatMessage);
                    break;

                case Chat.SubjectFormatChatMessage subjectFormatChatMessage:
                    showMessage = subjectFormatChatMessage.baseToken switch
                    {
                        "ACHIEVEMENT_UNLOCKED_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgAchievement),
                        "SHRINE_CHANCE_FAIL_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShrineChanceFail),
                        "SHRINE_CHANCE_SUCCESS_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShrineChanceWin),
                        "SHRINE_BOSS_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShrineBoss),
                        "SHRINE_BLOOD_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShrineBlood),
                        "SHRINE_RESTACK_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShrineRestack),
                        "SHRINE_HEALING_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShrineHealing),
                        "SHRINE_COMBAT_USE_MESSAGE" => ShouldShowClient(subjectFormatChatMessage, cfgShrineCombat),
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
                        "PLAYER_ACTIVATED_TELEPORTER" => ShouldShowClient(subjectChatMessage, cfgTeleporterActivation),
                        "PET_FROG" => ShouldShowClient(subjectChatMessage, cfgPetFrog),
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
                        "PORTAL_SHOP_WILL_OPEN" => cfgPortalShopWillOpen.Value,
                        "PORTAL_GOLDSHORES_WILL_OPEN" => cfgPortalGoldshoresWillOpen.Value,
                        "PORTAL_MS_WILL_OPEN" => cfgPortalMSWillOpen.Value,
                        "SHRINE_BOSS_BEGIN_TRIAL" => cfgMountainTeleporter.Value,
                        "PORTAL_SHOP_OPEN" => cfgPortalMSOpen.Value,
                        "PORTAL_GOLDSHORES_OPEN" => cfgPortalGoldshoresOpen.Value,
                        "PORTAL_MS_OPEN" => cfgPortalMSOpen.Value,
                        "ARENA_END" => cfgArenaEnd.Value,
                        _ => simpleChatMessage.baseToken.StartsWith("BAZAAR_SEER_")
                            ? cfgSeer.Value
                            : !simpleChatMessage.baseToken.StartsWith("FAMILY_")
                            || cfgFamily.Value
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

            var clientKey = "Client";
            ConfigEntry<bool> Bind(string k, bool t, string d)
            {
                return Config.Bind(clientKey, k, t, d);
            }
            ConfigEntry<ChatFilterType> Bind2(string k, ChatFilterType t, string d)
            {
                return Config.Bind(clientKey, k, t, d);
            }
            cfgPlayerPickupMessages = Config.Bind(clientKey, "Player Pickup", ChatFilterType.All, "<style=cEvent>{0} picked up {1}{2}</color>");
            cfgTimestampFormat = Config.Bind(clientKey, "Timestamp Format", "{0} {1}", "First parameter is the timestamp, the second is the original message. Leave empty to disable.");

            cfgDeathMessages = Bind2("Death", ChatFilterType.All, "");
            cfgJoinMessages = Bind2("Join", ChatFilterType.All, "<style=cEvent>{0} connected.</color>");
            cfgLeaveMessages = Bind2("Leave", ChatFilterType.All, "<style=cEvent>{0} disconnected.</color>");
            cfgNPCPickupMessages = Bind("NPC Pickup", true, "Scav pickups, void fields item adds");
            cfgAhoy = Bind2("Ahoy", ChatFilterType.All, "Ahoy!");
            cfgNpcMessages = Bind("NPC", true, "Mithrix messages");
            cfgAchievement = Bind2("Achievement", ChatFilterType.All, "<color=#ccd3e0>{0} achieved <color=#BDE151>{1}</color></color>");
            cfgFamily = Bind("Family", true, "");
            cfgTeleporterActivation = Bind2("Teleporter Activation", ChatFilterType.All, "<style=cEvent>{0} activated the <style=cDeath>Teleporter <sprite name=\"TP\" tint=1></style>.</style>");
            cfgSuppressor = Bind2("Suppressor", ChatFilterType.All, "<style=cShrine>{0} eradicated {1} from the universe.");
            cfgPortalShopWillOpen = Bind("Portal Shop Will Open", true, "<style=cWorldEvent>A blue orb appears..</style>");
            cfgPortalGoldshoresWillOpen = Bind("Portal Goldshores Will Open", true, "<style=cWorldEvent>A gold orb appears..</style>");
            cfgPortalMSWillOpen = Bind("Portal MS Will Open", true, "<style=cWorldEvent>A celestial orb appears..</style>");
            cfgPortalShopOpen = Bind("Portal Shop Open", true, "<style=cWorldEvent>A blue portal appears..</style>");
            cfgPortalGoldshoresOpen = Bind("Portal Goldshores Open", true, "<style=cWorldEvent>A gold portal appears..</style>");
            cfgPortalMSOpen = Bind("Portal MS Open", true, "<style=cWorldEvent>A celestial portal appears..</style>");
            cfgMountainTeleporter = Bind("Mountain Teleporter", true, "<style=cShrine>Let the challenge of the Mountain... begin!</style>");
            cfgShrineChanceWin = Bind2("Shrine Chance Win", ChatFilterType.All, "<style=cShrine>{0} offered to the shrine and was rewarded!</color>");
            cfgShrineChanceFail = Bind2("Shrine Chance Fail", ChatFilterType.All, "<style=cShrine>{0} offered to the shrine and gained nothing.</color>");
            cfgSeer = Bind("Seer", true, "<style=cWorldEvent>You dream of STAGEHINT.</style>");
            cfgShrineBoss = Bind2("Shrine Boss", ChatFilterType.All, "<style=cShrine>{0} has invited the challenge of the Mountain..</color>");
            cfgShrineBlood = Bind2("Shrine Blood", ChatFilterType.All, "<style=cShrine>{0} feels a searing pain, and has gained {1} gold.</color>");
            cfgShrineRestack = Bind2("Shrine Restack", ChatFilterType.All, "<style=cShrine>{0} is... sequenced.</color>");
            cfgShrineHealing = Bind2("Shrine Healing", ChatFilterType.All, "\"<style=cShrine>{0} is embraced by the healing warmth of the Woods.</color>");
            cfgShrineCombat = Bind2("Shrine Combat", ChatFilterType.All, "<style=cShrine>{0} has summoned {1}s to fight.</color>");
            cfgArenaEnd = Bind("Arena End", false, "<style=cWorldEvent>The Cell stabilizes.</style>");
            cfgPetFrog = Bind2("Pet Frog", ChatFilterType.All, "{0} pet the frog.");

            #endregion 
        }
    }
}