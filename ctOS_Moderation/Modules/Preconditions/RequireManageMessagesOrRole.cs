using Discord;
using Discord.Commands;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ctOS_Moderation.Modules.Preconditions {
    class RequireManageMessagesOrRole : PreconditionAttribute {
        public async override Task<PreconditionResult> CheckPermissions(ICommandContext context, CommandInfo command, IServiceProvider services) {
            List<ulong> roleIds = (context.User as IGuildUser).RoleIds.ToList();
            bool doesUserHaveRole = roleIds.Any(x => context.Guild.GetRole(x).Name == "ctOS Warnings");

            bool userHasManageMessages = (context.User as IGuildUser).GuildPermissions.ManageMessages;

            if (doesUserHaveRole || userHasManageMessages)
                return PreconditionResult.FromSuccess();
            else
                return PreconditionResult.FromError("You do not have the \"ctOS Warnings\" role or the Manage Messages permission!");
        }
    }
}
