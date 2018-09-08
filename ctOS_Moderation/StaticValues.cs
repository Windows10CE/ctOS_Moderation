using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Linq;
using static ctOS_Moderation.Modules.JSONHelper;

namespace ctOS_Moderation {
    public static class StaticValues {
        public static string CTOSModDir { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ctOS_Mod"); }}
        public static string WarningsDir { get { return Path.Combine(CTOSModDir, "Warnings"); }}
        public static string ServerSettingsDir { get { return Path.Combine(CTOSModDir, "ServerSettings"); }}
        public static string GetGuildPrefix(ulong serverId) {
            string serverConfig = Path.Combine(ServerSettingsDir, serverId.ToString() + ".json");

            if (!File.Exists(serverConfig))
                return "cm.";

            JObject json = JObject.Parse(File.ReadAllText(serverConfig));

            return GetJObjectValue(json, "prefix");
        }
        public static (ulong channelID, bool enabled) GuildLogChannel(ulong serverId) {
            string serverConfig = Path.Combine(ServerSettingsDir, serverId.ToString() + ".json");

            if (!File.Exists(serverConfig))
                return (default(ulong), false);

            JObject logChannel = JObject.Parse(JObject.Parse(File.ReadAllText(serverConfig))["logchannel"].ToString());

            ulong channelID = ulong.Parse(logChannel["channelID"].ToString());

            bool enabled = bool.Parse(logChannel["enabled"].ToString());

            return (channelID, enabled);
        }
        public static JObject DefaultConfigFile = new JObject(
            new JProperty("prefix", "cm."),
            new JProperty("logchannel", new JObject(
                new JProperty("enabled", false),
                new JProperty("channelID", 0))));
    }
}
