﻿using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using DSharpPlus.Entities;
using DSharpPlus.Interactivity.Extensions;
using ScrapScramble.BotRelated.Attributes;
using ScrapScramble.Game;
using ScrapScramble.Game.Cards;
using ScrapScramble.Game.Cards.Mechs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.BotRelated.Commands
{    
    public class GameCommands : BaseCommandModule
    {
        [Command("signup")]
        [Description("Signs you up for the next game.")]
        [RequireGuild]
        public async Task SignUp(CommandContext ctx, [Description("The name of your Upgrade")][RemainingText]string mechName)
        {            
            DiscordEmbedBuilder responseMessage;                  

            if (BotInfoHandler.participantsDiscordIds.Contains(ctx.User.Id))
            {
                //already signed up user
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You have already signed up",
                    Description = "You don't have to do anything else to sign up.",
                    Color = DiscordColor.Red
                };
            }
            else if (BotInfoHandler.inGame)
            {
                //a game has already started
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "A game has already started",
                    Description = "You would have to wait for the next game to sign up.",
                    Color = DiscordColor.Red
                };
            }
            else
            {
                bool appeared = false;
                for (int i = 0; i < BotInfoHandler.gameHandler.players.Count(); i++) if (BotInfoHandler.gameHandler.players[i].name.Equals(mechName)) { appeared = true; break; }
                //new user                

                if (appeared)
                {
                    //someone's already taken this name
                    responseMessage = new DiscordEmbedBuilder
                    {
                        Title = "Someone already has this name",
                        Description = "Choose a different name.",
                        Color = DiscordColor.Red
                    };
                }
                else
                {
                    //everything's correct
                    BotInfoHandler.AddPlayer(ctx, mechName);                    

                    responseMessage = new DiscordEmbedBuilder
                    {
                        Title = "Signed up successfully",
                        Description = $"Your Upgrade is called \"{mechName}\"",
                        Color = DiscordColor.Green
                    };

                    await ctx.Client.UpdateStatusAsync(new DiscordActivity
                    {
                        Name = $"({BotInfoHandler.participantsDiscordIds.Count()}) Waiting to >signup",
                        ActivityType = ActivityType.Playing
                    });
                }
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }

        [Command("signoff")]
        [Description("Signs you off from the next game.")]
        [RequireGuild]
        public async Task SignOff(CommandContext ctx)
        {
            DiscordEmbedBuilder responseMessage;

            if (!BotInfoHandler.participantsDiscordIds.Contains(ctx.User.Id))
            {
                //user has not signed up
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You have not signed up",
                    Description = "You can signup using ?signup (Your Upgrade name here).",
                    Color = DiscordColor.Red
                };
            }
            else if (BotInfoHandler.inGame)
            {
                //a game has already started

                int mechIndex = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

                BotInfoHandler.gameHandler.players[mechIndex].lives = 0;
                BotInfoHandler.gameHandler.pairsHandler.opponents[BotInfoHandler.gameHandler.pairsHandler.opponents[mechIndex]] = BotInfoHandler.gameHandler.pairsHandler.opponents[mechIndex];

                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You have been signed off from the current game",
                    Description = "You will be unable to return to the game.",
                    Color = DiscordColor.Green
                };
            }
            else
            {
                //signed up user
                int mechIndex = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You have signed off",
                    Description = "You will not participate in the next game.",
                    Color = DiscordColor.Green
                };

                BotInfoHandler.participantsDiscordIds.RemoveAt(mechIndex);
                BotInfoHandler.gameHandler.RemovePlayer(mechIndex);

                await ctx.Client.UpdateStatusAsync(new DiscordActivity
                {
                    Name = $"({BotInfoHandler.participantsDiscordIds.Count()}) Waiting to >signup",
                    ActivityType = ActivityType.Playing
                });
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }

        [Command("rename")]
        [Description("Renames your Upgrade.")]
        [RequireGuild]
        public async Task MechRename(CommandContext ctx, [Description("The new name of your Upgrade")][RemainingText]string mechName)
        {
            DiscordEmbedBuilder responseMessage;

            if (!BotInfoHandler.participantsDiscordIds.Contains(ctx.User.Id))
            {
                //user has not signed up
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "You have not signed up",
                    Description = "You can signup using ?signup (Your Upgrade name here).",
                    Color = DiscordColor.Red
                };
            }
            else if (BotInfoHandler.inGame)
            {
                //a game has already started
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "A game has already started",
                    Description = "You cannot change your Upgrade name while in game.",
                    Color = DiscordColor.Red
                };
            }
            else
            {
                //signed up user

                bool appeared = false;
                for (int i = 0; i < BotInfoHandler.gameHandler.players.Count(); i++) if (BotInfoHandler.gameHandler.players[i].name.Equals(mechName)) { appeared = true; break; }
                //new user                

                if (appeared)
                {
                    //someone's already taken this name
                    responseMessage = new DiscordEmbedBuilder
                    {
                        Title = "Someone already has this name",
                        Description = "Choose a different name.",
                        Color = DiscordColor.Red
                    };
                }
                else
                {
                    int mechIndex = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);

                    BotInfoHandler.gameHandler.players[mechIndex].name = mechName;

                    responseMessage = new DiscordEmbedBuilder
                    {
                        Title = "Upgrade renamed successfully.",
                        Description = $"Your Upgrade is now called {mechName}",
                        Color = DiscordColor.Green
                    };
                }
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }

        [Aliases("playerslist")]
        [Command("playerlist")]
        [Description("Shows a list of all players currently signed up.")]
        public async Task PlayerList(CommandContext ctx)
        {
            var responseMessage = new DiscordEmbedBuilder
            {
                Title = "List of Participants",
                Color = DiscordColor.Azure,
                Description = string.Empty
            };

            if (BotInfoHandler.gameHandler.players.Count() == 0) responseMessage.Description = "Nobody has signed up yet.";

            for (int i=0; i<BotInfoHandler.gameHandler.players.Count(); i++)
            {
                DiscordUser user = await ctx.Client.GetUserAsync(BotInfoHandler.participantsDiscordIds[i]);
                if (BotInfoHandler.inGame)
                {
                    if (BotInfoHandler.gameHandler.players[i].lives <= 0) responseMessage.Description += ":skull: ";
                    else if (BotInfoHandler.gameHandler.players[i].ready) responseMessage.Description += ":green_square: ";
                    else responseMessage.Description += ":red_square: ";
                }
                responseMessage.Description += $"{i+1}) {BotInfoHandler.gameHandler.players[i].name} ({user.Username})";
                if (BotInfoHandler.inGame) responseMessage.Description += $" - Lives: {BotInfoHandler.gameHandler.players[i].lives}";

                if (i != BotInfoHandler.gameHandler.players.Count()-1) responseMessage.Description += '\n';
            }

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
        }                

        [Command("lookup")]
        [Description("Shows information about an Upgrade.")]
        public async Task LookupUpgrade(CommandContext ctx, [Description("Name of the Upgrade")][RemainingText]string mechName)
        {
            DiscordEmbedBuilder responseMessage;
            if (mechName.Count() == 0)
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = "Incomplete command",
                    Description = "You need to follow up the command with the name of the Upgrade.",
                    Color = DiscordColor.Yellow
                };

                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
                return;
            }

            if (mechName.Length < 3)
            {
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder{
                    Title = "Input Is Too Short",
                    Description = "Your input needs to be at least 3 characters long.",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
                return;
            }

            List<int> candidates = new List<int>();

            for (int i=0; i<BotInfoHandler.gameHandler.pool.mechs.Count(); i++)
            {
                if (BotInfoHandler.gameHandler.pool.mechs[i].name.ToLower().Contains(mechName.ToLower()))
                {
                    candidates.Add(i);
                }
            }

            if (candidates.Count() == 1)
            {
                Upgrade mech = BotInfoHandler.gameHandler.pool.mechs[candidates[0]];
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = $"{mech.name}",
                    Description = $"{mech.Cost}/{mech.creatureData.attack}/{mech.creatureData.health} - {mech.rarity} Upgrade",
                    Color = DiscordColor.Green
                };

                if (Attribute.IsDefined(mech.GetType(), typeof(PackageAttribute)))
                {
                    var package = ((PackageAttribute)Attribute.GetCustomAttribute(mech.GetType(), typeof(PackageAttribute))).Package;
                    responseMessage.Description += $" | {PackageHandler.PackageAttributeToString[package]}";
                }

                if (!mech.cardText.Equals(string.Empty)) responseMessage.Description += $"\n{mech.cardText}";

                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
                return;
            }
            else if (candidates.Count() > 1)
            {
                responseMessage = new DiscordEmbedBuilder
                {
                    Title = $"{candidates.Count()} Matches Found",
                    Color = DiscordColor.Azure
                };

                for (int i=0; i<candidates.Count(); i++)
                {
                    if (i == 0) responseMessage.Description = BotInfoHandler.gameHandler.pool.mechs[candidates[i]].name;
                    else responseMessage.Description += $", {BotInfoHandler.gameHandler.pool.mechs[candidates[i]].name}";
                }

                await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
                return;
            }

            responseMessage = new DiscordEmbedBuilder
            {
                Title = "No Such Upgrade Found",
                Description = "Please make sure you typed the name correctly.",
                Color = DiscordColor.Red
            };

            await ctx.RespondAsync(embed: responseMessage).ConfigureAwait(false);
            return;
        }

        private async Task UpdateBrowseMenuAsync(CommandContext ctx, DiscordMessage msg, int upgradesPerPage, int page, int totalPages)
        {
            DiscordEmbedBuilder newMenuPage = new DiscordEmbedBuilder
            {
                Title = "List of Upgrades",
                Description = $"Page {page}/{totalPages}",
                Color = DiscordColor.Azure,
                Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"Total Upgrades: {BotInfoHandler.gameHandler.pool.mechs.Count()}"}
            };

            Rarity lastRarity = Rarity.NO_RARITY;
            
            string pageInfo = string.Empty;
            for (int i = upgradesPerPage*(page-1); i < upgradesPerPage*page && i <BotInfoHandler.gameHandler.pool.mechs.Count(); i++)
            {
                if (lastRarity != BotInfoHandler.gameHandler.pool.mechs[i].rarity)
                {
                    if (lastRarity != Rarity.NO_RARITY)
                    {
                        newMenuPage.AddField(lastRarity.ToString(), pageInfo);                        
                    }
                    pageInfo = string.Empty;
                    lastRarity = BotInfoHandler.gameHandler.pool.mechs[i].rarity;
                }
                pageInfo += $"{i + 1}) {BotInfoHandler.gameHandler.pool.mechs[i].name}\n";                
            }
            newMenuPage.AddField(lastRarity.ToString(), pageInfo);
            
            await msg.ModifyAsync(embed: newMenuPage.Build()).ConfigureAwait(false);
        }

        [Command("ulist2")]
        public async Task BrowseMenuTest(CommandContext ctx)
        {
            int upgradesPerPage = Math.Min(BotInfoHandler.gameHandler.pool.mechs.Count(), 7);            
            int totalPages = BotInfoHandler.gameHandler.pool.mechs.Count() / upgradesPerPage;
            if (BotInfoHandler.gameHandler.pool.mechs.Count() % upgradesPerPage != 0) totalPages++;

            //DiscordMessage menuMessage = await ctx.RespondAsync(embed: new DiscordEmbedBuilder { Color = DiscordColor.Azure }).ConfigureAwait(false);
            //await this.UpdateBrowseMenuAsync(ctx, menuMessage, upgradesPerPage, page, totalPages);

            var interactivity = ctx.Client.GetInteractivity();

            List<DSharpPlus.Interactivity.Page> allMenuPages = new List<DSharpPlus.Interactivity.Page>();

            DSharpPlus.Interactivity.PaginationEmojis paginationEmojis = new DSharpPlus.Interactivity.PaginationEmojis
            {
                SkipLeft = DiscordEmoji.FromName(ctx.Client, ":rewind:"),
                Left = DiscordEmoji.FromName(ctx.Client, ":arrow_left:"),
                Stop = DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:"),
                Right = DiscordEmoji.FromName(ctx.Client, ":arrow_right:"),
                SkipRight = DiscordEmoji.FromName(ctx.Client, ":fast_forward:")
            };

            for (int page=1; page<=totalPages; page++)
            {
                string description = $"Page {page}/{totalPages}\n";

                Rarity lastRarity = Rarity.NO_RARITY;
                string pageInfo = string.Empty;
                for (int i = upgradesPerPage * (page - 1); i < upgradesPerPage * page && i < BotInfoHandler.gameHandler.pool.mechs.Count(); i++)
                {
                    if (lastRarity != BotInfoHandler.gameHandler.pool.mechs[i].rarity)
                    {
                        if (lastRarity != Rarity.NO_RARITY)
                        {
                            description += $"\n**{lastRarity}**\n{pageInfo}";
                            //newMenuPage.AddField(lastRarity.ToString(), pageInfo);
                        }
                        pageInfo = string.Empty;
                        lastRarity = BotInfoHandler.gameHandler.pool.mechs[i].rarity;
                    }
                    pageInfo += $"{i + 1}) {BotInfoHandler.gameHandler.pool.mechs[i]}\n";
                }
                description += $"\n**{lastRarity}**\n{pageInfo}";
                //newMenuPage.AddField(lastRarity.ToString(), pageInfo);

                DSharpPlus.Interactivity.Page menuPage = new DSharpPlus.Interactivity.Page(embed: new DiscordEmbedBuilder
                {
                    Title = "List of Upgrades",
                    Description = description,
                    Color = DiscordColor.Azure,
                    Footer = new DiscordEmbedBuilder.EmbedFooter { Text = $"Total Upgrades: {BotInfoHandler.gameHandler.pool.mechs.Count()}" }
                });

                allMenuPages.Add(menuPage);
            }

            await interactivity.SendPaginatedMessageAsync(ctx.Channel, ctx.User, allMenuPages, paginationEmojis, DSharpPlus.Interactivity.Enums.PaginationBehaviour.WrapAround, timeoutoverride: TimeSpan.FromMinutes(7));
        }

        [Aliases("upgradelist")]
        [Command("upgradeslist")]
        [Description("Displays an interactable menu, which lists the names of all available Upgrades.")]
        [RequireGuild]
        public async Task BrowseMenu(CommandContext ctx)
        {
            int upgradesPerPage = Math.Min(BotInfoHandler.gameHandler.pool.mechs.Count(), 10);
            int page = 1;
            int totalPages = BotInfoHandler.gameHandler.pool.mechs.Count() / upgradesPerPage;
            if (BotInfoHandler.gameHandler.pool.mechs.Count() % upgradesPerPage != 0) totalPages++;
            
            DiscordMessage menuMessage = await ctx.RespondAsync(embed: new DiscordEmbedBuilder { Color = DiscordColor.Azure}).ConfigureAwait(false);
            await this.UpdateBrowseMenuAsync(ctx, menuMessage, upgradesPerPage, page, totalPages);



            List<DiscordEmoji> buttons = new List<DiscordEmoji>
            {
                DiscordEmoji.FromName(ctx.Client, ":rewind:"),
                DiscordEmoji.FromName(ctx.Client, ":arrow_left:"),
                DiscordEmoji.FromName(ctx.Client, ":no_entry_sign:"),
                DiscordEmoji.FromName(ctx.Client, ":arrow_right:"),
                DiscordEmoji.FromName(ctx.Client, ":fast_forward:")
            };

            for (int i=0; i<buttons.Count(); i++)
            {
                await menuMessage.CreateReactionAsync(buttons[i]).ConfigureAwait(false);                
            }                        

            while (true)
            {
                var interactivity = ctx.Client.GetInteractivity();

                var reactionResult = await interactivity.WaitForReactionAsync(
                    x => x.Message == menuMessage &&
                    x.User == ctx.User &&
                    buttons.Contains(x.Emoji)).ConfigureAwait(false);

                if (reactionResult.TimedOut) break;

                if (reactionResult.Result.Emoji.Equals(buttons[2]))
                {
                    await menuMessage.DeleteAsync().ConfigureAwait(false);
                    break;
                }

                DiscordEmoji foundEmoji;

                if (reactionResult.Result.Emoji.Equals(buttons[0])) { foundEmoji = buttons[0]; page = 1; }
                else if (reactionResult.Result.Emoji.Equals(buttons[1])) { foundEmoji = buttons[1]; page = Math.Max(page - 1, 1); }
                else if (reactionResult.Result.Emoji.Equals(buttons[3])) { foundEmoji = buttons[3]; page = Math.Min(page + 1, totalPages); }
                else if (reactionResult.Result.Emoji.Equals(buttons[4])) { foundEmoji = buttons[4]; page = totalPages; }
                else break;

                await this.UpdateBrowseMenuAsync(ctx, menuMessage, upgradesPerPage, page, totalPages);
                await menuMessage.DeleteReactionAsync(foundEmoji, ctx.User).ConfigureAwait(false);                
            }
        }
    
        [Command("spareparts")]
        [Description("Displays all spare parts.")]
        public async Task SpareParts(CommandContext ctx)
        {
            string msg = string.Empty;

            for (int i=0; i < BotInfoHandler.gameHandler.pool.spareparts.Count(); i++)
            {
                msg += $"{i+1}) {BotInfoHandler.gameHandler.pool.spareparts[i].GetInfo(BotInfoHandler.gameHandler, -1)}";
                if (i != BotInfoHandler.gameHandler.pool.spareparts.Count() - 1) msg += "\n";
            }

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder{
                Title = "List of Spare Parts",
                Description = msg,
                Color = DiscordColor.Azure
            }).ConfigureAwait(false);
        }

        [Command("gameinfo")]
        [Description("Displays information about the current game or the future game's setup.")]
        public async Task GameInfo(CommandContext ctx)
        {
            string msg = string.Empty;

            msg += $"Amount of Players: {BotInfoHandler.gameHandler.players.Count()}";
            msg += $"\nStarting Lives: {BotInfoHandler.gameHandler.startingLives}";
            if (BotInfoHandler.gameHandler.maxManaCap >= 0) msg += $"\nMana Cap: {BotInfoHandler.gameHandler.maxManaCap}";
            else msg += "\nMana Cap: None";
            msg += $"\n\nShop Rarity Breakdown:\n";
            msg += $"C-R-E-L: ";
            msg += $"{BotInfoHandler.gameHandler.shopRarities.common}-";
            msg += $"{BotInfoHandler.gameHandler.shopRarities.rare}-";
            msg += $"{BotInfoHandler.gameHandler.shopRarities.epic}-";
            msg += $"{BotInfoHandler.gameHandler.shopRarities.legendary}";
            
            msg += $"\n\nUpgrade Pool Rarity Breakdown:\n";
            msg += $"C-R-E-L: ";
            msg += $"{CardsFilter.FilterList<Upgrade>(BotInfoHandler.gameHandler.pool.mechs, x => x.rarity == Rarity.Common).Count()}-";
            msg += $"{CardsFilter.FilterList<Upgrade>(BotInfoHandler.gameHandler.pool.mechs, x => x.rarity == Rarity.Rare).Count()}-";
            msg += $"{CardsFilter.FilterList<Upgrade>(BotInfoHandler.gameHandler.pool.mechs, x => x.rarity == Rarity.Epic).Count()}-";
            msg += $"{CardsFilter.FilterList<Upgrade>(BotInfoHandler.gameHandler.pool.mechs, x => x.rarity == Rarity.Legendary).Count()}";

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder {
                Title = "Game Info",
                Description = msg,
                Color = DiscordColor.Azure
            }).ConfigureAwait(false);
        }

        [Command("packages")]
        public async Task ShowPackages(CommandContext ctx)
        {
            string msg = string.Empty;

            int i = 1;
            foreach (var key in BotInfoHandler.gameHandler.packageHandler.Packages.Keys)
            {
                msg += $"{i}) {key}\n";
                i++;
            }

            await ctx.RespondAsync(embed: new DiscordEmbedBuilder { 
                Title = "List of Packages",
                Description = msg,
                Color = DiscordColor.Azure
            }).ConfigureAwait(false);
        }

        [Aliases("package")]
        [Command("packages")]        
        public async Task ShowPackages(CommandContext ctx, [RemainingText]string packageName)
        {
            packageName = packageName.ToLower();
            packageName = PackageHandler.PackageAttributeToString[PackageHandler.LowercaseToPackageAttribute[packageName]];

            if (!BotInfoHandler.gameHandler.packageHandler.Packages.ContainsKey(packageName))
            {
                //package not found
                await ctx.RespondAsync(embed: new DiscordEmbedBuilder
                {
                    Title = "No Such Package Found",
                    Description = "Make sure you've typed the name correctly.",
                    Color = DiscordColor.Red
                }).ConfigureAwait(false);
            }
            else
            {
                var embed = new DiscordEmbedBuilder(){
                    Title = $"{packageName}",
                    Color = DiscordColor.Azure
                };

                string description = string.Empty;

                Rarity lastRarity = Rarity.NO_RARITY;
                int rarityCount = 0;

                for (int i = 0; i < BotInfoHandler.gameHandler.packageHandler.Packages[packageName].Count(); i++)
                {
                    if (lastRarity != BotInfoHandler.gameHandler.packageHandler.Packages[packageName][i].rarity)
                    {
                        if (lastRarity != Rarity.NO_RARITY)
                        {
                            embed.AddField($"{lastRarity.ToString()} ({rarityCount})", description);                            
                        }
                        description = string.Empty;
                        rarityCount = 0;
                        lastRarity = BotInfoHandler.gameHandler.packageHandler.Packages[packageName][i].rarity;
                    }
                    rarityCount++;
                    description += $"- {BotInfoHandler.gameHandler.packageHandler.Packages[packageName][i]}\n";
                }
                embed.AddField($"{lastRarity.ToString()} ({rarityCount})", description);

                await ctx.RespondAsync(embed: embed.Build()).ConfigureAwait(false);
            }            
        }
    }
}
