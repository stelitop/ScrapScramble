using DSharpPlus.CommandsNext;
using DSharpPlus.CommandsNext.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ScrapScramble.BotRelated.Attributes
{
    class RequireBeingPlayer : CheckBaseAttribute
    {
        public RequireBeingPlayer()
        {

        }

        public override Task<bool> ExecuteCheckAsync(CommandContext ctx, bool help)
        {
            bool result = BotInfoHandler.participantsDiscordIds.Contains(ctx.User.Id);

            if (result == false) return Task.FromResult(false);
            
            int mechIndex = BotInfoHandler.participantsDiscordIds.IndexOf(ctx.User.Id);
            if (BotInfoHandler.gameHandler.players[mechIndex].lives <= 0) return Task.FromResult(false);

            return Task.FromResult(true);
        }
    }
}
