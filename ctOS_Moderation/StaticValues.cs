using System;
using System.IO;

namespace ctOS_Moderation {
    public static class StaticValues {
        public static string CTOSModDir { get { return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "ctOS_Mod"); }}
        public static string WarningsDir { get { return Path.Combine(CTOSModDir, "Warnings"); } }
    }
}
