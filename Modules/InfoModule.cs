using Discord.Commands;
using System.Threading.Tasks;

namespace StrykerDG.Discord.StrykerBot.Modules
{
    public class InfoModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Summary("Provides a help menu")]
        public Task HelpAsync() =>
            ReplyAsync("How can I help? Please say !test, or !dev");
    }
}
