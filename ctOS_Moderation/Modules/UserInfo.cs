using Discord;
using Discord.Commands;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class UserInfo : ModuleBase<SocketCommandContext> {
        [Command("userinfo")]
        public async Task GetUserInfoAsync(IGuildUser user) {
            EmbedBuilder builder = new EmbedBuilder();

            StringBuilder stringbuilder = new StringBuilder();
            foreach (var id in user.RoleIds)
                foreach (var role in Context.Guild.Roles)
                    if (id == role.Id)
                        if (role.Name != "@everyone") {
                            if (stringbuilder.ToString() == String.Empty || stringbuilder.ToString() == null)
                                stringbuilder.Append(role.Name);
                            else
                                stringbuilder.Append(", " + role.Name);
                        }
                                
            if (stringbuilder.ToString() == String.Empty || stringbuilder.ToString() == null)
                stringbuilder.Append("This user has no roles.");

            builder
                .AddField("User Info", $"User info for {user.Mention}")
                .WithThumbnailUrl((user as IUser).GetAvatarUrl())
                .AddInlineField("Username and Discrim", $"{user.Username}#{user.Discriminator}")
                .AddInlineField("Created on", user.CreatedAt)
                .AddInlineField("Joined on", user.JoinedAt)
                .AddInlineField("User ID", user.Id)
                .AddInlineField("Roles", stringbuilder.ToString())
                .AddInlineField("Online Status", user.Status)
                .AddInlineField("Is a Bot", user.IsBot.ToString().ToLower());

            await ReplyAsync("", false, builder.Build());
        }
        public string TestStringForNullOrEmpty(string stringToTest) {
            if (stringToTest != null && stringToTest != String.Empty)
                return stringToTest;
            else
                return String.Empty;
        }
    }
}
