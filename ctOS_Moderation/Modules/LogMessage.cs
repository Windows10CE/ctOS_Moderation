using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public static class LogMessage {
        public static async Task SendGuildLogMessageAsync(this SocketGuild guild, Embed message) {
            var (channelID, enabled) = StaticValues.GuildLogChannel(guild.Id);

            if (!enabled) return;

            if (guild.GetChannel(channelID) is SocketTextChannel logChannel)
                await logChannel.SendMessageAsync("", false, message);
        }
    }
}
