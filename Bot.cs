using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using HtmlAgilityPack;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using ScrapScramble.BotRelated;
using ScrapScramble.BotRelated.Commands;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

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
                EnableDms = true,
            };

            Commands = Client.UseCommandsNext(commandsConfig);

            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<GameCommands>();
            Commands.RegisterCommands<GameOperatorCommands>();
            Commands.RegisterCommands<PlayerOnlyCommands>();
            Commands.RegisterCommands<GameSettingsCommands>();

            await Client.ConnectAsync();            

            await Task.Delay(-1);
        }

        private async Task OnClientReady(DiscordClient client, ReadyEventArgs e)
        {
            VoidNoParamsCaller caller = new VoidNoParamsCaller(this.LoadKeywordsFromWebsite);
            caller.BeginInvoke(null, null);

            //this.LoadKeywordsFromWebsite();
                
            await Client.UpdateStatusAsync(new DiscordActivity
            {
                Name = $"({BotInfoHandler.participantsDiscordIds.Count()}) Waiting to >signup",
                ActivityType = ActivityType.Playing
            });

            return;
        }

        private void LoadKeywordsFromWebsite()
        {         
            //// Create a request for the URL.
            //WebRequest request = WebRequest.Create(
            //  "https://github.com/stelitop/ScrapScramble/wiki/Keywords");
            //// If required by the server, set the credentials.
            //request.Credentials = CredentialCache.DefaultCredentials;

            //// Get the response.
            //WebResponse response = request.GetResponse();
            //// Display the status.
            //Console.WriteLine(((HttpWebResponse)response).StatusDescription);

            //string responseFromServer;

            //// Get the stream containing content returned by the server.
            //// The using block ensures the stream is automatically closed.
            //using (Stream dataStream = response.GetResponseStream())
            //{
            //    // Open the stream using a StreamReader for easy access.
            //    StreamReader reader = new StreamReader(dataStream);
            //    // Read the content.
            //    responseFromServer = reader.ReadToEnd();
            //    // Display the content.                
            //}

            ////Console.WriteLine(responseFromServer);

            //// Close the response.
            //response.Close();

            var htmlWeb = new HtmlWeb();
            var documentNode = htmlWeb.Load("https://github.com/stelitop/ScrapScramble/wiki/Keywords").DocumentNode;

            //var keywords = documentNode.Descendants("li");
            var keywords = documentNode.Descendants("li").Where(x => x.Descendants("p").Count() == 1);

            string KWDescriptionFull = string.Empty;

            foreach (var kw in keywords)
            {
                KWDescriptionFull += $"- {kw.InnerText.Trim()}\n";                                                
            }

            BotInfoHandler.CommandInformation.KeywordDescription = KWDescriptionFull;        
        }
    }

    public delegate void VoidNoParamsCaller();
}
