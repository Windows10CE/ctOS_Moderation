using System;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace ctOS_Moderation.Modules {
    public class Ban : ModuleBase<SocketCommandContext>
    {
        [Command("ban"), RequireBotPermission(GuildPermission.BanMembers), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(SocketGuildUser user, int pruneDays, [Remainder] string reason = null) {
            if (!(pruneDays < 0) && !(pruneDays > 7)) {
                ulong userID = user.Id;
                await Context.Guild.AddBanAsync(userID, pruneDays, reason);
                await ReplyAsync($"Banned {user.Mention} and deleted messages from the past {pruneDays} day(s).");
            } else {
                await ReplyAsync("Days to prune must be from 0-7");
            }

            var embed = new EmbedBuilder()
                .WithTitle("Log Message: Ban")
                .WithColor(Color.Red)
                .WithTimestamp(DateTimeOffset.UtcNow)
                .AddField($"User {user.Username}#{user.Discriminator} has been banned!", $"User banned by {Context.User.Mention}")
                .Build();

            await Context.Guild.SendGuildLogMessageAsync(embed);
        }
        public async Task BanNoIntAsync(SocketGuildUser user, [Remainder] string reason = null) {
            ulong userID = user.Id;
            await Context.Guild.AddBanAsync(userID, 7, reason);
            await ReplyAsync($"Banned {user.Mention} and deleted messages from the past 7 day(s).");

            var embed = new EmbedBuilder()
                .WithTitle("Log Message: Ban")
                .WithColor(Color.Red)
                .WithTimestamp(DateTimeOffset.UtcNow)
                .AddField($"User {user.Username}#{user.Discriminator} has been banned!", $"User banned by {Context.User.Mention}")
                .Build();

            await Context.Guild.SendGuildLogMessageAsync(embed);
        }
    }
}
