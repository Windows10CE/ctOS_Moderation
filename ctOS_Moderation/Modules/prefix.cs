using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Newtonsoft.Json.Linq;
using static ctOS_Moderation.Modules.JSONHelper;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class Prefix : ModuleBase<SocketCommandContext> {
        [Command("prefix")]
        public async Task SetPrefixAsync([Remainder] string prefix = "printprefix") {
            string currentPrefix = StaticValues.GetServerPrefix(Context.Guild.Id);
            if (prefix == "printprefix") {
                await ReplyAsync($"The prefix is \"{currentPrefix}\"");
                return;
            }
            if (!(Context.User as IGuildUser).GuildPermissions.ManageGuild) {
                await ReplyAsync("This command requires the Manage Server permission.");
                return;
            }
            if (prefix == currentPrefix) {
                await ReplyAsync($"The server is already using the prefix \"{prefix}\"!");
                return;
            }
            if (prefix.Length > 5) {
                await ReplyAsync("The prefix must be shorter than 6 characters.");
                return;
            }
            string configFile = Path.Combine(StaticValues.ServerSettingsDir, Context.Guild.Id.ToString() + ".json");
            JObject config = File.Exists(configFile) ? JObject.Parse(File.ReadAllText(configFile)) : StaticValues.DefaultConfigFile;

            config["prefix"] = prefix;

            File.WriteAllText(configFile, config.ToString());
            await ReplyAsync($"The prefix has been set to \"{prefix}\"!");
        }
    }
}
