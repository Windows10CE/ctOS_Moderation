using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace ctOS_Moderation.Modules
{
    [Group("warn")]
    public class Warning : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task WarnHelpAsync() {
            EmbedBuilder builder = new EmbedBuilder();

            builder
                .AddInlineField("warn show [user mention]", "Shows the warnings of a user.")
                .AddInlineField("warn add [user mention] [warning text]", "Adds a warning to the user. Requires a role called \"ctOS Warnings\"")
                .WithColor(Color.Blue);

            await ReplyAsync("", false, builder.Build());
        }

        [Command("add")]
        public async Task AddWarnAsync(SocketGuildUser userToWarn, [Remainder] string warning) {
            var user = Context.User as SocketGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "ctOS Warnings");
            if (user.Roles.Contains(role)) {
                string userID = userToWarn.Id.ToString();
                string warningsDir = @"c:\Users\aaron\Documents\Moderation\";
                string userWarningsFile = warningsDir + userID + @".json";
                string usernameOfWarned = $"@{userToWarn.Username}#{userToWarn.Discriminator}";

                JObject warningObj = new JObject(
                    new JProperty("Warning Server", Context.Guild.Name),
                    new JProperty("Warning", warning),
                    new JProperty("Given By", usernameOfWarned));
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
            } else {
                await ReplyAsync("You don't have the needed role. (ctOS Warnings)");
            }
        }
    }
}
