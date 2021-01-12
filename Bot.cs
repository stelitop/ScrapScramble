using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScrapScramble.BotRelated.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble
{
    class Bot
    {
        public DiscordClient Client { get; private set; }
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }

        public async Task RunAsync()
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = await sr.ReadToEndAsync().ConfigureAwait(false);

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug                
                //UseInternalLoggingHandler = true
            };

            Client = new DiscordClient(config);           

            //listens to events
            Client.Ready += OnClientReady;           

            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(60)
            });

            var commandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = true
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<GameCommands>();
            Commands.RegisterCommands<GameOperatorCommands>();
            Commands.RegisterCommands<PlayerOnlyCommands>();            

            await Client.ConnectAsync();            

            await Task.Delay(-1);
        }

        private async Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            await Client.UpdateStatusAsync(new DiscordActivity
            {
                Name = "Scrap Scramble. >help",
                ActivityType = ActivityType.Playing
            });

            return;
        }
    }
}
