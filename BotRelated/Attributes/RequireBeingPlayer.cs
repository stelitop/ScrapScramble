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
            return Task.FromResult(BotInfoHandler.participantsDiscordIds.Contains(ctx.User.Id));
        }
    }
}
