using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using ModBot.Helpers;
using ModBot.models;

namespace ModBot.Modules
{
    public class CasinoModule : ModuleBase<SocketCommandContext>
    {
        DatabaseManager database = new DatabaseManager();

        [Command("kasyno-wejdź")]
        [Summary("Umożliwia interakcję z kasynem")]
        public async Task EnterCasino()
        {
            var userInfo = Context.Message.Author;
            if(database.GetUser(userInfo.Id) == null)
            {
                User user = new User();
                user.setUID(userInfo.Id);
                user.setWallet(200);
                database.AddUser(user);
                user = database.GetUser(userInfo.Id);
                if (user!= null)
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Sukces!", string.Format("Użytkowniku {0}, zostałeś wpuszczony do kasyna, łap drobne na start!", ((SocketGuildUser)userInfo).Nickname));
                    embed.WithColor(Color.Green);
                    await Context.Channel.SendMessageAsync("", false, embed);
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Błąd!", string.Format("Użytkowniku {0}, coś poszło nie tak, i nie wszedłeś do kasyna, spróbuj ponownie!", ((SocketGuildUser)userInfo).Nickname));
                    embed.WithColor(Color.Red);
                    await Context.Channel.SendMessageAsync("", false, embed);
                }
            }else
            {
                var embed = new EmbedBuilder();
                embed.AddField("Hola!", string.Format("Użytkowniku {0}, już jesteś w kasynie!", ((SocketGuildUser)userInfo).Nickname));
                embed.WithColor(Color.Red);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }

        [Command("portfel")]
        [Summary("zwraca ilość pieniędzy którą dysponuje użytkownik")]
        public async Task GetWallet([Summary("(opcjonalne)nazwa użytkownika")] SocketUser user = null)
        {
            var userInfo = user == null ? Context.Message.Author : user;
            User tUser = new User();
            tUser = database.GetUser(userInfo.Id);
            if (tUser == null)
            {
                var embed = new EmbedBuilder();
                embed.WithAuthor(userInfo);
                embed.AddField("Nie znaleziono!", "Ten użytkownik nie jest zarejestrowany w kasynie.");
                embed.WithColor(Color.Red);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithAuthor(userInfo);
                embed.AddField("Stan konta", tUser.getWallet().ToString());
                embed.WithColor(Color.Gold);
                await Context.Channel.SendMessageAsync("", false, embed);
            }
        }
    }
}
