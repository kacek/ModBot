using Discord;
using Discord.WebSocket;
using Discord.Commands;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Reflection;
using ModBot.Helpers;
using ModBot.Services;
using ModBot.TypeReaders;
using ModBot.Enums;

namespace ModBot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().StartAsync().GetAwaiter().GetResult();
 
        private DiscordSocketClient _client;
        private CommandService _commands;
        private IServiceProvider _services;
        private DatabaseManager _database;

        public async Task StartAsync()
        {
            _client = new DiscordSocketClient();
            _commands = new CommandService();

            _client.Log += Log;

            _database = new DatabaseManager();
            if(_database.Init() != true)
            {
                Console.WriteLine("error creating/loading XML file");
            }

            Console.WriteLine("Insert Token:");
            string token = Console.ReadLine();

            _services = GetServiceProvider();
            try
            {
                await InstallCommandsAsync();
                await _client.LoginAsync(TokenType.Bot, token);
                await _client.StartAsync();
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
                Console.ReadKey();
            }

            await Task.Delay(-1);
        }

        public async Task InstallCommandsAsync()
        {
            _commands.AddTypeReader<CoinSide>(new CoinSideReader());
            
            _client.MessageReceived += HandleCommandAsync;
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly());

        }

        private async Task HandleCommandAsync(SocketMessage messageParam)
        {
            var message = messageParam as SocketUserMessage;
            if (message == null) return;
            int argPos = 0;
            if (!(message.HasCharPrefix('!', ref argPos) || message.HasMentionPrefix(_client.CurrentUser, ref argPos))) return;
            var context = new SocketCommandContext(_client, message);
            var result = await _commands.ExecuteAsync(context, argPos, _services);
            if (!result.IsSuccess)
                await context.Channel.SendMessageAsync(result.ErrorReason);
        }

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }

        private IServiceProvider GetServiceProvider()
        {
            return new ServiceCollection()
                .AddSingleton(_client)
                .AddSingleton(_commands)
                .AddSingleton(new CasinoService())
                .BuildServiceProvider();
        }
    }
}
