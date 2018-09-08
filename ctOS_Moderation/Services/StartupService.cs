using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace ctOS_Moderation.Services {
    class StartupService {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;
        private readonly IConfigurationRoot _config;

        public StartupService(DiscordSocketClient discord, CommandService commands, IConfigurationRoot config) {
            _discord = discord;
            _commands = commands;
            _config = config;
        }

        public async Task StartupAsync() {
            string botToken = _config["Token"];
            if (String.IsNullOrWhiteSpace(botToken)) throw new ArgumentNullException("Token cannot be empty or null!");

            await _discord.LoginAsync(Discord.TokenType.Bot, botToken);
            await _discord.StartAsync();

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }
    }
}
