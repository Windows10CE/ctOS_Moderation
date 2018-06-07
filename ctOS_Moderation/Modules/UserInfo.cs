using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class UserInfo : ModuleBase<SocketCommandContext> {
        [Command("userinfo")]
        public async Task GetUserInfoAsync(SocketGuildUser user) {
            EmbedBuilder builder = new EmbedBuilder();

            StringBuilder stringbuilder = new StringBuilder();
            user.Roles.Select(async x => stringbuilder.Append(await TestStringForNullOrEmpty(x.Name)));
            if (stringbuilder.ToString() == String.Empty || stringbuilder.ToString() == null)
                stringbuilder.Append("This user has no roles.");

            builder
                .AddField("User Info", $"User info for {user.Mention}")
                .AddInlineField("Username and Discrim", $"{user.Username}#{user.Discriminator}")
                .AddInlineField("Created on", user.CreatedAt)
                .AddInlineField("Joined on", user.JoinedAt)
                .AddInlineField("User ID", user.Id)
                .AddInlineField("Roles", stringbuilder.ToString())
                .AddInlineField("Online Status", user.Status)
                .AddInlineField("Is a Bot", user.IsBot.ToString().ToLower());

            await ReplyAsync("", false, builder.Build());
        }
        public async Task<string> TestStringForNullOrEmpty(string stringToTest) {
            if (stringToTest != null && stringToTest != String.Empty)
                return stringToTest;
            else
                return String.Empty;
        }
    }
}
