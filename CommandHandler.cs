using Discord.Commands;
using Discord.WebSocket;
using StrykerDG.Discord.StrykerBot.Modules;
using System;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly CommandService _commandService;
        private readonly IServiceProvider _services;

        public CommandHandler(DiscordSocketClient client, CommandService commandService, IServiceProvider services = null)
        {
            _client = client;
            _commandService = commandService;
            _services = services;
        }

        public async Task InstallCommandAsync()
        {
            _client.MessageReceived += HandleCommandAsync;
            await _commandService.AddModuleAsync<InfoModule>(_services);
            await _commandService.AddModuleAsync<AdminModule>(_services);
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
                services: _services
            );

            // Inform the user if the command fails
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }
    }
}
