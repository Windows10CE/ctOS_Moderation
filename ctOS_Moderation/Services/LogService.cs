using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.IO;
using System.Threading.Tasks;

namespace ctOS_Moderation.Services {
    class LogService {
        private readonly DiscordSocketClient _discord;
        private readonly CommandService _commands;

        private string _logDirectory => Path.Combine(AppContext.BaseDirectory, "Logs");
        private string _logFile => Path.Combine(_logDirectory, $"{DateTime.UtcNow.ToString("yyyy-MM-dd")}.log.txt");

        public LogService(DiscordSocketClient discord, CommandService commands) {
            _discord = discord;
            _commands = commands;

            _discord.Log += LogAsync;
            _commands.Log += LogAsync;
        }

        private Task LogAsync(LogMessage msg) {
            if (!Directory.Exists(_logDirectory))
                Directory.CreateDirectory(_logDirectory);
            if (!File.Exists(_logFile))
                File.Create(_logFile).Dispose();

            string logText = $"{DateTime.UtcNow.ToString("hh:mm:ss")} [{msg.Severity}] {msg.Source}: {msg.Exception?.ToString() ?? msg.Message}";
            File.AppendAllText(_logFile, logText + "\n");

            return Console.Out.WriteLineAsync(logText);
        }
    }
}
