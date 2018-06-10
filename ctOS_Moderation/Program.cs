﻿using System;
using Discord;
using Discord.WebSocket;
using System.Threading.Tasks;
using System.IO;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

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

            new Program().RunBotAsync().GetAwaiter().GetResult();
        }

        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _service;

        public async Task RunBotAsync() {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _service = new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .BuildServiceProvider();

            string botToken = File.ReadAllText(Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "Token"));

            _client.Log += Log;

            await RegisterCommandsAsync();

            await _client.LoginAsync(TokenType.Bot, botToken);

            await _client.StartAsync();

            await _client.SetGameAsync("cm.help");

            await Task.Delay(3000);
            Console.WriteLine("");

            await Task.Delay(-1);
        }

        public Task Log(LogMessage arg) {
            Console.WriteLine(arg);

            return Task.CompletedTask;
        }

        public async Task RegisterCommandsAsync() {
            _client.MessageReceived += HandleCommandAsync;

            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());
        }

        public async Task HandleCommandAsync(SocketMessage s) {
            SocketUserMessage message = s as SocketUserMessage;
            if (message == null || message.Author.IsBot) return;

            int argPos = 0;
            if ((message.HasStringPrefix(StaticValues.GetServerPrefix(new SocketCommandContext(_client, message).Guild.Id), ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos) || message.ToString() == "cm.prefix") && !(new SocketCommandContext(_client, message).IsPrivate)) {
                var context = new SocketCommandContext(_client, message);
                IResult result;

                if (message.ToString() == "cm.prefix" && message.HasStringPrefix("cm.", ref argPos))
                    result = await _commands.ExecuteAsync(context, argPos, _service);
                else
                    result = await _commands.ExecuteAsync(context, argPos, _service);



                if (!result.IsSuccess) {
                    await context.Channel.SendMessageAsync(result.ErrorReason);
                    if (result.Error != CommandError.UnknownCommand && result.Error != CommandError.BadArgCount && result.Error != CommandError.ObjectNotFound)
                        Console.WriteLine(result.ErrorReason + "\n");
                }
            }
        }
    }
}
