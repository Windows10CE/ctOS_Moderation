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
                .AddField("prefix", "Get or set the prefix of the server. Requires the Manage Server permission to change the prefix.")
                .AddField("kick [user mention]", "Kicks the mentioned user.")
                .AddField("ban [user mention] (days to prune) (reason)", "Bans the user with a reason(optional), and deletes their messages over the past however many days(optional).")
                .AddField("userinfo [user mention]", "Returns a chunk of the mentioned useres info related to Discord and the server you're in.")
                .AddField("embedbuild [embed items, you can have as many as you like]", "The embed item format is [\"Item Title\":\"Item Content\"] without the sqaure brackets. This command requires the Manage Messages permission.")
                .AddField("show [user mention] (page number)", "Shows the warnings of a user. Requires a role called \"ctOS Warnings\" or the Manage Messages Permission")
                .AddField("warn [user mention] [warning text]", "Adds a warning to the user. Requires a role called \"ctOS Warnings\" or the Manage Messages Permission")
                .AddField("delete [user mention] [warning # or \"all\"]", "Deletes a warning message from the user. Requires a role called \"ctOS Warnings\" or the Manage Messages permission. Warning: This command will only delete warnings given in the server you send this command in.")
                .WithColor(Color.DarkBlue);

            await Context.User.SendMessageAsync("", false, builder.Build());
        }
    }
}
