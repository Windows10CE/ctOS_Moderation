using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace ctOS_Moderation.Services {
    class CommandHandler {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;
        private readonly IServiceProvider _provider;

        public CommandHandler(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config, IServiceProvider provider) {
            _discord = discord;
            _commands = commands;
            _config = config;
            _provider = provider;

            _discord.MessageReceived += OnMessageReceivedAsync;
        }

        public async Task OnMessageReceivedAsync(SocketMessage e) {
            if (!(e is SocketUserMessage msg) || msg.Author.Id == _discord.CurrentUser.Id) return;

            SocketCommandContext context = new SocketCommandContext(_discord, msg);

            int argPos = 0;
            if (msg.HasStringPrefix(StaticValues.GetGuildPrefix(context.Guild.Id), ref argPos) || msg.HasMentionPrefix(_discord.CurrentUser, ref argPos)) {
                var result = await _commands.ExecuteAsync(context, argPos, _provider);

                if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    await context.Channel.SendMessageAsync(result.ToString());
            }
        }
    }
}
