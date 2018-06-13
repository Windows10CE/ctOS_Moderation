using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    [Group("clear"), RequireBotPermission(GuildPermission.ManageMessages), RequireUserPermission(GuildPermission.ManageMessages)]
    public class Clear : ModuleBase<SocketCommandContext> {
        [Command]
        public async Task ClearMessagesAsync(int numOfMesages) {
            var messages = await Context.Channel.GetMessagesAsync(numOfMesages + 1).Flatten();

            await Context.Channel.DeleteMessagesAsync(messages);

            IUserMessage reply = await ReplyAsync("The messages have been deleted!");
            await Task.Delay(3000);
            await reply.DeleteAsync();
        }
        [Command]
        public async Task ClearUserMessagesAsync(IGuildUser user, int numOfMessages) {
            var messages = await Context.Channel.GetMessagesAsync(numOfMessages + 1).Flatten();
            List<IMessage> userMessages = new List<IMessage>();
            foreach (IMessage message in messages)
                if (message.Author.Id == user.Id)
                    userMessages.Add(message);
            await Context.Channel.DeleteMessagesAsync(userMessages.AsEnumerable());

            var reply = await ReplyAsync($"{userMessages.Count} messages from {user.Username}#{user.Discriminator} have been deleted!");
            await Task.Delay(3000);
            await reply.DeleteAsync();
        }
    }
}
