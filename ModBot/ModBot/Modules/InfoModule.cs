using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;

namespace ModBot
{
    public class InfoModule :ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("returns response time")]
        public async Task PingAsync()
        {
            await ReplyAsync(":ping_pong: Pong! :stopwatch: " + Context.Client.Latency);
        }
    }
}
