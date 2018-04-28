using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class Kick : ModuleBase<SocketCommandContext>
    {
        [Command("kick"), RequireBotPermission(GuildPermission.KickMembers), RequireUserPermission(GuildPermission.KickMembers)]
        public async Task KickAsync(SocketGuildUser user, [Remainder] string reason = null) {
            var IGuildUserToKick = user as IGuildUser;
            await IGuildUserToKick.KickAsync(reason);
            await ReplyAsync($"Kicked {user.Mention}.");
        }
    }
}
