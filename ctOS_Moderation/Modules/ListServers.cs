using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using System.Linq;

namespace ctOS_Moderation.Modules {
    public class ListServers : ModuleBase<SocketCommandContext> {
        [Command("listservers"), RequireOwner]
        public async Task GetServerListAsync() {
            string filename = Path.Combine(StaticValues.CTOSModDir, "temp.txt");
            StringBuilder builder = new StringBuilder();

            foreach (SocketGuild server in Context.Client.Guilds) {
                string invite = Context.Guild.GetUser(Context.Client.CurrentUser.Id).GuildPermissions.CreateInstantInvite ? (await server.GetInvitesAsync()).Where(x => !x.IsTemporary && !x.IsRevoked && x.MaxUses > x.Uses).Select(x => x.Url).DefaultIfEmpty("None").First() : "None";
                builder.Append($"{server.Name} {invite}\n");
            }
            File.WriteAllText(filename, builder.ToString());
            await Context.Channel.SendFileAsync(filename, "These are all the servers this bot is currently in.");
            File.Delete(filename);
        }
    }
}
