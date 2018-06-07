using Discord.Commands;
using Discord;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class Help : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        public async Task HelpAsync() {
            EmbedBuilder builder = new EmbedBuilder();

            builder
                .AddField("help", "This help menu!")
                .AddField("ping", "Pong!")
                .AddInlineField("kick [user mention]", "Kicks the mentioned user.")
                .AddInlineField("ban [user mention] (days to prune) (reason)", "Bans the user with a reason(optional), and deletes their messages over the past however many days(optional).")
                .AddInlineField("userinfo [user mention]", "Returns a chunk of the mentioned useres info related to Discord and the server you're in.")
                .AddField("Warnings", "Warning system.")
                .AddInlineField("show [user mention] (page number)", "Shows the warnings of a user. Requires a role called \"ctOS Warnings\" or the Manage Messages Permission")
                .AddInlineField("warn [user mention] [warning text]", "Adds a warning to the user. Requires a role called \"ctOS Warnings\" or the Manage Messages Permission")
                .AddInlineField("delete [user mention] [warning # or \"all\"]", "Deletes a warning message from the user. Requires a role called \"ctOS Warnings\" or the Manage Messages permission. Warning: This command will only delete warnings given in the server you send this command in.")
                .WithColor(Color.DarkBlue);

            await ReplyAsync("", false, builder.Build());
        }
    }
}
