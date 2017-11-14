using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;

namespace ModBot
{
    public class InfoModule :ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Returns response time.")]
        public async Task PingAsync()
        {
            await ReplyAsync(":ping_pong: Pong! :stopwatch: " + Context.Client.Latency);
        }

        [Command("whois")]
        [Summary("Returns global name of specified user.")]
        public async Task WhoIsAsync([Summary("(optional)user to get global name from.")] SocketUser user = null)
        {
            var userInfo = user ?? Context.Message.Author;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }

        [Command("userinfo")]
        [Summary("Returns detailed information about user.")]
        public async Task UserInfoAsync([Summary("(optional)user to get detailed information about")] SocketUser user = null)
        {
            var userInfo = user ?? Context.Message.Author;
            var embed = new EmbedBuilder();
            embed.WithAuthor(userInfo);
            embed.AddField("Local name",((SocketGuildUser)userInfo).Nickname==null ? userInfo.Username : ((SocketGuildUser)userInfo).Nickname);
            embed.AddField("Status", userInfo.Status.ToString());
            embed.AddField("In game", userInfo.Game.HasValue ? userInfo.Game.Value.ToString() : "none");
            embed.AddField("Joined", ((SocketGuildUser)userInfo).JoinedAt.ToString());
            embed.WithColor(Color.Blue);
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
