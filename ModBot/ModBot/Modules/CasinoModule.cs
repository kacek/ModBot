using System;
using System.Collections.Generic;
using System.Text;
using Discord.Commands;
using System.Threading.Tasks;
using Discord.WebSocket;
using Discord;
using ModBot.Helpers;
using ModBot.models;
using ModBot.Services;
using ModBot.Enums;

namespace ModBot.Modules
{
    public class CasinoModule : ModuleBase<SocketCommandContext>
    {
        private DatabaseManager database = new DatabaseManager();
        private CasinoService _casinoService;

        public CasinoModule(CasinoService c)
        {
            _casinoService = c;
        }

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
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
                else
                {
                    var embed = new EmbedBuilder();
                    embed.AddField("Błąd!", string.Format("Użytkowniku {0}, coś poszło nie tak, i nie wszedłeś do kasyna, spróbuj ponownie!", ((SocketGuildUser)userInfo).Nickname));
                    embed.WithColor(Color.Red);
                    await Context.Channel.SendMessageAsync("", false, embed.Build());
                }
            }else
            {
                var embed = new EmbedBuilder();
                embed.AddField("Hola!", string.Format("Użytkowniku {0}, już jesteś w kasynie!", ((SocketGuildUser)userInfo).Nickname));
                embed.WithColor(Color.Red);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }

        [Command("rzut")]
        [Summary("wykonuje rzut monetą")]
        public async Task CoinToss([Summary("strona(orzeł/reszka)")]CoinSide side, [Summary("obstawiana kwota")]uint amount)
        {            
            var tUser = database.GetUser(Context.User.Id);

            if(tUser == null)
            {
                await ReplyAsync("", embed: new EmbedBuilder().WithAuthor(Context.User).WithColor(Color.Red).WithDescription("Przykro mi, ale nie ma cię w kasynie. (Aby wejść do kasyna, użyj komendy !kasyno-wejdź)").Build());
                return;
            }

            if (amount == 0)
            {
                await ReplyAsync("", embed: new EmbedBuilder().WithAuthor(Context.User).WithColor(Color.Red).WithDescription("Na pewno chcesz rzucać nie obstawiając niczego?").Build());
                return;
            }

            if (!tUser.ChangeWalletCnt(-amount))
            {
                await ReplyAsync("", embed: new EmbedBuilder().WithAuthor(Context.User).WithColor(Color.Red).WithDescription("Kolego zaczekaj! Przecież Ty tyle nie posiadasz!").Build());
                return;
            }

            if (side == _casinoService.DrawCoinSide())
            {
                await ReplyAsync("", embed: new EmbedBuilder().WithAuthor(Context.User).WithColor(Color.Green)
                    .WithDescription("Brawo przyjacielu! Trafiłeś!").Build());

                tUser.ChangeWalletCnt(amount*2);
            }
            else
            {
                await ReplyAsync("", embed: new EmbedBuilder().WithAuthor(Context.User).WithColor(Color.Red)
                    .WithDescription("Niestety pudełko.").Build());
            }

            database.UpdateUser(tUser);
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
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
            else
            {
                var embed = new EmbedBuilder();
                embed.WithAuthor(userInfo);
                embed.AddField("Stan konta", tUser.getWallet().ToString());
                embed.WithColor(Color.Gold);
                await Context.Channel.SendMessageAsync("", false, embed.Build());
            }
        }
    }
}
