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
            var userInfo = (user ?? Context.User) as SocketGuildUser;
            if (userInfo == null) return;
            var game = userInfo.Activity as Game;
            
            var embed = new EmbedBuilder();
            embed.WithAuthor(userInfo);
            embed.AddField("Nick", userInfo.Nickname ?? userInfo.Username);
            embed.AddField("Status", userInfo.Status.ToString());
            embed.AddField("W grze", game != null ? game.Name : "brak");
            embed.AddField("Dołączył", userInfo.JoinedAt.ToString());
            embed.WithColor(Color.Blue);
            await Context.Channel.SendMessageAsync("", false, embed.Build());
        }
    }
}
