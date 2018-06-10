using Newtonsoft.Json.Linq;
using System;
using System.IO;
using static ctOS_Moderation.Modules.JSONHelper;

namespace ctOS_Moderation {
    public static class StaticValues {
        public static string CTOSModDir { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ctOS_Mod"); }}
        public static string WarningsDir { get { return Path.Combine(CTOSModDir, "Warnings"); }}
        public static string ServerSettingsDir { get { return Path.Combine(CTOSModDir, "ServerSettings"); }}
        public static string GetServerPrefix(ulong serverId) {
            string serverConfig = Path.Combine(ServerSettingsDir, serverId.ToString() + ".json");

            if (!File.Exists(serverConfig))
                return "cm.";

            JObject json = JObject.Parse(File.ReadAllText(serverConfig));

            return GetJObjectValue(json, "prefix");
        }
        public static JObject DefaultConfigFile = new JObject(
            new JProperty("prefix", "cm."));
    }
}
