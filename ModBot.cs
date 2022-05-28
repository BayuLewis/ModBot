using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json;
using static ModBot.EnviromentVariables;

namespace ModBot
{
    class ModBot
    {
        private static DiscordSocketClient client;
        private static CommandService _command;
        private CommandHandler commandHandler;
        private  static IServiceProvider _service;
        static async Task Main(string[] args)
        {
            var config = new DiscordSocketConfig { MessageCacheSize = 200 };
            client = new DiscordSocketClient(config);
            client.Log += Log;
            await client.LoginAsync(TokenType.Bot, Token);
            await client.StartAsync();
            client.MessageReceived += MessageReceived;
            _command = new CommandService();
            CommandHandler commandHandler = new CommandHandler(client, _command);
            await commandHandler.InstallCommandsAsync();
            await Task.Delay(-1);
        }
        
        

        private static Task Log(LogMessage message)
        {
            Console.WriteLine(message.ToString());
            return Task.CompletedTask;
        }

        private static async Task MessageReceived(SocketMessage message)
        {
            Console.WriteLine($"{message.Content}");
            if (message.Author.Id == client.CurrentUser.Id) return;

            // var badWords = await JsonFileReader.Read<BadWords>(@"C:\Users\bayul\Desktop\badwords.json");

            var badWords = JsonConvert.DeserializeObject<BadWords>(File.ReadAllText(@"C:\Users\bayul\Desktop\badwords.json"));
            foreach (var words in badWords.words)
            {
                if (message.Content.Equals(words, StringComparison.OrdinalIgnoreCase)) 
                {
                    await message.DeleteAsync();
                    Console.WriteLine($"{message.Author.Mention} {words} is not allowed here!");
                }
            }
            
            var ignoreCases = JsonConvert.DeserializeObject<IgnoreWords>(File.ReadAllText(@"C:\Users\bayul\Desktop\ignoreCases.json"));
            foreach (var words in ignoreCases.words)
            {
                return;
            }
        }


        public class BadWords
        {
            [JsonProperty("words")]
            public List<string> words { get; set; }
        }

        public class IgnoreWords
        {
            [JsonProperty("words")]
            public List<string> words { get; set; }
        }
    }
}
