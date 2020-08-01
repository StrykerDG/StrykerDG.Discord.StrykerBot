using System;
using System.Collections.Generic;
using System.Text;

namespace StrykerDG.Discord.StrykerBot.Config
{
    public class BotSettings
    {
        public string DiscordToken { get; set; }
        public ulong LogChannelId { get; set; }
        public ulong WelcomeChannelId { get; set; }
        public string AdminRoleName { get; set; }
        public ulong AdminRoleId { get; set; }
    }
}
