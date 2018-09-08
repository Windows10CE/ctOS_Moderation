using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class Kick : ModuleBase<SocketCommandContext>
    {
        [Command("kick"), RequireBotPermission(GuildPermission.KickMembers), RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(SocketGuildUser user, [Remainder] string reason = null) {
            var IGuildUserToKick = user as IGuildUser;
            await IGuildUserToKick.KickAsync(reason);
            await ReplyAsync($"Kicked {user.Mention}.");

            var embed = new EmbedBuilder()
                .WithTitle("Log Message: Kick")
                .WithColor(Color.DarkOrange)
                .WithTimestamp(DateTimeOffset.UtcNow)
                .AddField($"User {user.Username}#{user.Discriminator} has been kicked!", $"User kicked by {Context.User.Mention}")
                .Build();

            await Context.Guild.SendGuildLogMessageAsync(embed);
        }
    }
}
