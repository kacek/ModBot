using Discord;
using Discord.WebSocket;
using System;
using System.Threading.Tasks;

namespace ModBot
{
    public class Program
    {
        public static void Main(string[] args) => new Program().MainAsync().GetAwaiter().GetResult();

        private DiscordSocketClient _client;

        public async Task MainAsync()
        {
            _client = new DiscordSocketClient();

            _client.Log += Log;

            Console.WriteLine("Insert Token:");
            string token = Console.ReadLine();
            try
            {
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

        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            return Task.CompletedTask;
        }
    }
}
