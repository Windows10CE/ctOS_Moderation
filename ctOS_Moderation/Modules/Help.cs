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
                .AddField("warn", "Warning system.")
                .AddInlineField("warn add [user mention] [warning text]", "Addes a warning to a user, requires you to have a role called \"ctOS Warnings\"")
                .AddInlineField("warn show [user mention] (page number)", "Shows all the warns a user has across all servers. Also requires you to have a role called \"ctOS Warnings\"")
                .WithColor(Color.DarkBlue);

            await ReplyAsync("", false, builder.Build());
        }
    }
}
