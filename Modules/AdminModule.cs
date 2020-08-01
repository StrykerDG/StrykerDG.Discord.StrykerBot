using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Microsoft.Extensions.Options;
using StrykerDG.Discord.StrykerBot.Config;
using StrykerDG.Discord.StrykerBot.Services;
using System.Linq;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot.Modules
{
    public class AdminModule : ModuleBase<SocketCommandContext>
    {
        private readonly LoggingService _logger;
        private readonly BotSettings _settings;

        public AdminModule(LoggingService logger, IOptions<BotSettings> settings)
        {
            _logger = logger;
            _settings = settings.Value;
        }

        [Command("kick")]
        [Summary("Kicks the specified user from the guild")]
        public async Task KickAsync(SocketGuildUser username)
        {
            var user = Context.User as SocketGuildUser;
            if (IsAdmin(user))
                await username.KickAsync();
            else
                Log(
                    LogSeverity.Warning,
                    $"{user.Username} attempted to kick {username.Username}"
                );
        }

        [Command("ban")]
        [Summary("Bans the specified user from the guild")]
        public async Task BanAsync(SocketGuildUser username)
        {
            var user = Context.User as SocketGuildUser;
            if (IsAdmin(user))
                await username.BanAsync();
            else
                Log(
                    LogSeverity.Warning,
                    $"{user.Username} attempted to ban {username.Username}"
                );
        }

        // HelperMethods
        private bool IsAdmin(SocketGuildUser user)
        {
            var adminRole = user.Roles
                .Where(r => r.Name == _settings.AdminRoleName)
                .FirstOrDefault();

            return adminRole != null;
        }

        private async void Log(LogSeverity severity, string message)
        {
            ReplyAsync("You do not have permission to do that!");

            var logChannel = (SocketTextChannel)Context.Client
                .GetChannel(_settings.LogChannelId);

            var admins = Context.Guild.GetRole(_settings.AdminRoleId);
            message = $"{admins.Mention} " + message;

            await _logger.WriteLog(severity, message, logChannel);
        }
    }
}
