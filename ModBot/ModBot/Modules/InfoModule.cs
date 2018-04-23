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
        [Summary("Zwraca czas reakcji bota")]
        public async Task PingAsync()
        {
            await ReplyAsync(":ping_pong: Pong! :stopwatch: " + Context.Client.Latency);
        }

        [Command("whois")]
        [Summary("Zwraca globalną nazwę użytkownika")]
        public async Task WhoIsAsync([Summary("(opcjonalne)nazwa użytkownika.")] SocketUser user = null)
        {
            var userInfo = user ?? Context.Message.Author;
            await ReplyAsync($"{userInfo.Username}#{userInfo.Discriminator}");
        }

        [Command("userinfo")]
        [Summary("Zwraca szczegółowe innformacje o użytkowniku")]
        public async Task UserInfoAsync([Summary("(opcjonalne)nazwa użytkownika")] SocketUser user = null)
        {
            var userInfo = user ?? Context.Message.Author;
            var embed = new EmbedBuilder();
            embed.WithAuthor(userInfo);
            embed.AddField("Nick",((SocketGuildUser)userInfo).Nickname==null ? userInfo.Username : ((SocketGuildUser)userInfo).Nickname);
            embed.AddField("Status", userInfo.Status.ToString());
            embed.AddField("W grze", userInfo.Game.HasValue ? userInfo.Game.Value.ToString() : "brak");
            embed.AddField("Dołączył", ((SocketGuildUser)userInfo).JoinedAt.ToString());
            embed.WithColor(Color.Blue);
            await Context.Channel.SendMessageAsync("", false, embed);
        }
    }
}
