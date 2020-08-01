using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StrykerDG.Discord.StrykerBot.Config;
using StrykerDG.Discord.StrykerBot.Services;
using System;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot
{
    public class Program
    {
        // Configuration Objects
        private IConfiguration _configuration;
        private IServiceProvider _services;

        // Discord.Net Objects
        private DiscordSocketClient _client;
        private CommandService _commandService;

        // Custom Handlers and Settings
        private CommandHandler _commandHandler;
        private UserHandler _userHandler;
        private LoggingService _logService;
        private BotSettings _botSettings;

        public static void Main(string[] args) =>
            new Program().MainAsync().GetAwaiter().GetResult();

        public async Task MainAsync()
        {
            // Create the Discord objects
            _client = new DiscordSocketClient();
            _commandService = new CommandService(new CommandServiceConfig
            {
                LogLevel = LogSeverity.Info,
                CaseSensitiveCommands = false
            });

            _logService = new LoggingService(_client, _commandService);

            // Configure Dependency Injection
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            _services = serviceCollection.BuildServiceProvider();

            // Initialize our handlers
            _commandHandler = new CommandHandler(_client, _commandService, _services);
            _userHandler = new UserHandler(_client, _botSettings);

            await _commandHandler.InstallCommandAsync();
            await _userHandler.InstallCommandAsync();

            // Connect to Discord
            await _client.LoginAsync(
                TokenType.Bot,
                _botSettings.DiscordToken
            );

            await _client.StartAsync();

            // Block the task until the program is closed
            await Task.Delay(-1);
        }

        private void ConfigureServices(IServiceCollection services)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            _botSettings = new BotSettings();
            _configuration.GetSection("BotSettings").Bind(_botSettings);

            // Allow DI for Settings
            services.Configure<BotSettings>(_configuration.GetSection("BotSettings"));
            services.AddSingleton(_logService);
        }
    }
}
