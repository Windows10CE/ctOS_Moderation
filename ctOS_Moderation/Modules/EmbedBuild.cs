using Discord;
using Discord.Commands;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ctOS_Moderation.Modules {
    public class EmbedBuild : ModuleBase<SocketCommandContext> {
        [Command("embedbuild")]
        public async Task EmbedBuildAsync([Remainder]string args) {
            if (!(Context.User as IGuildUser).GuildPermissions.ManageMessages) {
                await ReplyAsync("You must have the Manage Messages permission!");
                return;
            }

            EmbedBuilder builder = new EmbedBuilder();

            List<EmbedItem> embedItems = new List<EmbedItem>();

            StringBuilder EmbedItemBuilder = new StringBuilder();
            int numOfQuotations = 0;
            foreach (char c in args) {
                if (c == '\"')
                    numOfQuotations++;
                if ((numOfQuotations == 0 || numOfQuotations == 2) && c != ':')
                    continue;
                if (numOfQuotations == 4) {
                    EmbedItemBuilder.Append(c);
                    string[] EmbedItemString = EmbedItemBuilder.ToString().Replace("\"", "").Split(':');
                    if (EmbedItemString.Length != 2) {
                        await ReplyAsync("An unknown error has occured");
                        return;
                    }
                    embedItems.Add(new EmbedItem {
                        EmbedTitle = EmbedItemString[0],
                        EmbedContent = EmbedItemString[1]
                    });
                    EmbedItemBuilder = new StringBuilder();
                    numOfQuotations = 0;
                    continue;
                }
                EmbedItemBuilder.Append(c);
            }

            if (!(embedItems.Count > 0)) {
                await ReplyAsync("Please use the correct format for embeds");
                return;
            }

            foreach (EmbedItem item in embedItems)
                builder.AddField(item.EmbedTitle, item.EmbedContent);

            await ReplyAsync("in dev", false, builder.Build());
        }
        public class EmbedItem {
            public string EmbedTitle;
            public string EmbedContent;
        }
    }
}
