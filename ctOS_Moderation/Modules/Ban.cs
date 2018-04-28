using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class Ban : ModuleBase<SocketCommandContext>
    {
        [Command("ban"), RequireBotPermission(GuildPermission.BanMembers), RequireUserPermission(GuildPermission.BanMembers)]
        public async Task BanAsync(SocketGuildUser user, int pruneDays = 0, [Remainder] string reason = null) {
            if (!(pruneDays < 0) && !(pruneDays > 7)) {
                ulong userID = user.Id;
                await Context.Guild.AddBanAsync(userID, pruneDays, reason);
                await ReplyAsync($"Banned {user.Mention} and deleted messages from the past {pruneDays} day(s).");
            } else {
                await ReplyAsync("Days to prune must be from 0-7");
            }
        }
    }
}
