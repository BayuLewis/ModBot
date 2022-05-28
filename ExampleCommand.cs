using Discord.Commands;
using System.IO;
using ModBot;
using Newtonsoft.Json;

public class ExampleCommand : ModuleBase<SocketCommandContext>
{
    [Command("hey")]
    [Summary("Just says hi.")]

    public async Task SayAsync()
    {
        Console.WriteLine("Command used");
        await ReplyAsync("Hey!");
    }
    
    // If you know how to add a command that add a bad word to the .json file, feel free top contribute.
    
}