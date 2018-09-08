using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    [Group("settings"), RequireUserPermission(GuildPermission.ManageGuild)]
    public class Prefix : ModuleBase<SocketCommandContext> {
        [Command("prefix")]
        public async Task SetPrefixAsync([Remainder] string prefix = "printprefix") {
            string currentPrefix = StaticValues.GetGuildPrefix(Context.Guild.Id);
            if (prefix == "printprefix") {
                await ReplyAsync($"The prefix is \"{currentPrefix}\"");
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

        [Command("logchannel")]
        public async Task SetLogChannelAsync(SocketTextChannel channel) {
            var (channelID, enabled) = StaticValues.GuildLogChannel(Context.Guild.Id);
            if (channelID == channel.Id) {
                await ReplyAsync("That is already the log channel!");
                return;
            }

            string filepath = Path.Combine(StaticValues.ServerSettingsDir, Context.Guild.Id + ".json");
            var json = File.Exists(filepath) ? JObject.Parse(File.ReadAllText(filepath)) : StaticValues.DefaultConfigFile;

            json["logchannel"] = new JObject(
                new JProperty("enabled", true),
                new JProperty("channelID", channel.Id));

            File.WriteAllText(filepath, json.ToString());

            await ReplyAsync($"Log Channel changed to {channel.Mention}!");
        }
    }
}
