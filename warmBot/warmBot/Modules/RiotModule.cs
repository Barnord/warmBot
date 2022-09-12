using Discord.Commands;
using Discord.WebSocket;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using warmBot.Managers;
using warmBot.Core;

namespace warmBot.Modules
{
    internal class RiotModule : ModuleBase<SocketCommandContext>
    {
        

        [Command("user")]
        [Summary("Sets the summoner of a UserDetails object")]
        public async Task UserCommand([Remainder] string sid) => await Bot.channel.SendMessageAsync(UserManager.SetSummoner(Context.User, sid));
    }
}
