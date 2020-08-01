using Discord.WebSocket;
using Microsoft.Extensions.Options;
using StrykerDG.Discord.StrykerBot.Config;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot
{
    public class UserHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly BotSettings _settings;

        public UserHandler(DiscordSocketClient client, BotSettings settings)
        {
            _client = client;
            _settings = settings;
        }

        public Task InstallCommandAsync()
        {
            _client.UserJoined += HandleUserJoined;
            return Task.CompletedTask;
        }

        private async Task HandleUserJoined(SocketGuildUser newUser)
        {
            var welcomeMessage = $"Welcome to StrykerDG, {newUser.Mention}! " +
                $"If you need help with anything, just type !help";

            var welcomeChannel = (SocketTextChannel)_client
                .GetChannel(_settings.WelcomeChannelId);

            await welcomeChannel.SendMessageAsync(welcomeMessage);
        }
    }
}
