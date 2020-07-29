using Discord.Commands;
using Discord.WebSocket;
using StrykerDG.Discord.StrykerBot.Modules;
//using System.Reflection;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;

        public CommandHandler(DiscordSocketClient client, CommandService service)
        {
            _client = client;
            _commandService = service;
        }

        public async Task InstallCommandAsync()
        {
            // Hook the MessageReceived event into our handler
            _client.MessageReceived += HandleCommandAsync;

            // Discover all command modules and load them.
            // Services should be null if not using Dependency Injection
            //await _commandService.AddModulesAsync(
            //    assembly: Assembly.GetEntryAssembly(),
            //    services: null
            //);
            await _commandService.AddModuleAsync<InfoModule>(null);
        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            // Don't process the command if it was a system message
            var message = messageParam as SocketUserMessage;
            if (message == null)
                return;

            // Create a number to track where the prefix ends and the command begins
            int argPos = 0;

            // Only process the command if it's from a user and contains a ! prefix
            var prefixOrUser =
                message.HasCharPrefix('!', ref argPos) ||
                message.HasMentionPrefix(_client.CurrentUser, ref argPos);
            var isBot = message.Author.IsBot;
            if (!prefixOrUser || isBot)
                return;

            // Create a websocket-based context based on the message
            var context = new SocketCommandContext(_client, message);

            // Execute the command with the context and service provider
            // for precondition checks. Result states if succuessful
            var result = await _commandService.ExecuteAsync(
                context: context,
                argPos: argPos,
                services: null
            );

            // Inform the user if the command fails
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
