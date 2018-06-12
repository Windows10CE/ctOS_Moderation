using Discord;
using Discord.Commands;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class Clear : ModuleBase<SocketCommandContext> {
        [Command("clear"), RequireBotPermission(GuildPermission.ManageMessages), RequireUserPermission(GuildPermission.ManageMessages)]
        public async Task ClearMessagesAsync(int numOfMesages) {
            var messages = await Context.Channel.GetMessagesAsync(numOfMesages + 1).Flatten();

            await Context.Channel.DeleteMessagesAsync(messages);

            IUserMessage reply = await ReplyAsync("The messages have been deleted!");
            await Task.Delay(3000);
            await reply.DeleteAsync();
        }
    }
}
