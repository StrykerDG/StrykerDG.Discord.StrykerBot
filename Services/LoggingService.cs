using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot.Services
{
    public class LoggingService
    {
        public LoggingService(DiscordSocketClient client, CommandService commandService)
        {
            client.Log += LogAsync;
            commandService.Log += LogAsync;
        }

        public async Task WriteLog(LogSeverity severity, string message, SocketTextChannel channel)
        {
            var prefix = severity.ToString();
            if(severity == LogSeverity.Critical)
                prefix = $"**{severity.ToString().ToUpper()}**";
            if (severity == LogSeverity.Error)
                prefix = $"*{severity}*";

            var log = $"{prefix} {message}";
            await channel.SendMessageAsync(log);
        }

        private Task LogAsync(LogMessage message)
        {
            // TODO: Implement actions
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }

}
