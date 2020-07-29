using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot
{
    public class Program
    {
        private DiscordSocketClient _client;
        private CommandService _commandService;
        private CommandHandler _handler;
        //private IServiceProvider _services;

        // Enter into a an async context, allowing us to
        // connect to Discord without worrying about 
        // setting up an async implementation
        public static void Main(string[] args) =>
            new Program().MainAsync().GetAwaiter().GetResult();

        // Our Main() / Entry point method 
        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();
            _commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });

            _client.Log += Log;
            _commandService.Log += Log;

            _handler = new CommandHandler(_client, _commandService);
            await _handler.InstallCommandAsync();

            await _client.LoginAsync(
                TokenType.Bot,
                "<TOKEN GOES HERE>"
            );

            await _client.StartAsync();

            // Block the task until the program is closed
            await Task.Delay(-1);
        }

        /*
        private static IServiceProvider ConfigureServices()
        {
            var map = new ServiceCollection()
                // Repeat for all service classes and dependencies
                .AddSingleton(new SomeServiceClass);

            return map.BuildServiceProvider();
        }
        */

        // Handle Discord.Net's Log events
        private Task Log(LogMessage message)
        {
            // TODO: Actually do something with them
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }
    }
}
