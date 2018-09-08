using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Threading.Tasks;
using ctOS_Moderation.Services;

namespace ctOS_Moderation
{
    class Program
    {
        static void Main(string[] args) {
            if (!Directory.Exists(StaticValues.WarningsDir)) {
                if (!Directory.Exists(StaticValues.CTOSModDir)) {
                    Directory.CreateDirectory(StaticValues.CTOSModDir);
                }
                Directory.CreateDirectory(StaticValues.WarningsDir);
            }
            if (!Directory.Exists(StaticValues.ServerSettingsDir))
                Directory.CreateDirectory(StaticValues.ServerSettingsDir);

            new Program().StartupAsync().GetAwaiter().GetResult();
        }

        private IConfigurationRoot _config;

        public async Task StartupAsync() {
            await Console.Out.WriteLineAsync("Starting ctOS_Moderation Discord Bot...");

            var builder = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("config.json");
            _config = builder.Build();

            var service = new ServiceCollection()
                .AddSingleton(new DiscordSocketClient(new DiscordSocketConfig {
                    LogLevel = Discord.LogSeverity.Error,
                    MessageCacheSize = 50
                }))
                .AddSingleton(new CommandService(new CommandServiceConfig {
                    DefaultRunMode = RunMode.Async
                }))
                .AddSingleton<StartupService>()
                .AddSingleton<LogService>()
                .AddSingleton<CommandHandler>()
                .AddSingleton(_config);

            var provider = service.BuildServiceProvider();

            provider.GetRequiredService<LogService>();
            await provider.GetRequiredService<StartupService>().StartupAsync();
            provider.GetRequiredService<CommandHandler>();

            await Console.Out.WriteLineAsync("ctOS_Moderation Bot Started!\n");

            await Task.Delay(-1);
        }
    }
}
