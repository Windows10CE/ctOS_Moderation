using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using ctOS_Moderation.Modules.Preconditions;
using System;

namespace ctOS_Moderation.Modules {
    public class Warn : ModuleBase<SocketCommandContext>
    {
        [Command("warn"), RequireManageMessagesOrRole]
        public async Task AddWarnAsync(SocketGuildUser userToWarn, [Remainder] string warning) {
            string userID = userToWarn.Id.ToString();
            string userWarningsFile = Path.Combine(StaticValues.WarningsDir, userID + ".json");
            string usernameOfWarned = $"{userToWarn.Username}#{userToWarn.Discriminator}";

            JObject warningObj = new JObject(
                new JProperty("Warning Location", $"{Context.Guild.Name} in channel #{Context.Channel.Name}"),
                new JProperty("Warning", warning),
                new JProperty("Given By", $"@{Context.User.Username}#{Context.User.Discriminator}"),
                new JProperty("Server ID", Context.Guild.Id.ToString()));
            string warningJSONString = warningObj.ToString();

            File.AppendAllText(userWarningsFile, warningJSONString);

            EmbedBuilder builderEnvoker = new EmbedBuilder();
            EmbedBuilder builderDM = new EmbedBuilder();

            builderEnvoker.AddField($"User {usernameOfWarned} has been warned.", $"Warning message: \"{warning}\"").WithColor(Color.DarkRed);
            builderDM
                .AddInlineField("Warning Message", warning)
                .AddInlineField("Warned In", $"{Context.Guild.Name} in channel #{Context.Channel.Name}")
                .AddInlineField("Warned By", Context.User.Mention)
                .WithColor(Color.DarkRed);

            await ReplyAsync("", false, builderEnvoker.Build());
            var userToDM = userToWarn as IUser;
            await UserExtensions.SendMessageAsync(userToDM, "You have been warned.", false, builderDM.Build());

            var embed = builderEnvoker
                .WithTitle("Log Message: Warning")
                .WithTimestamp(DateTimeOffset.UtcNow)
                .Build();

            await Context.Guild.SendGuildLogMessageAsync(embed);
        }
    }
}
