using System.IO;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace ctOS_Moderation.Modules {
    public class ListServers : ModuleBase<SocketCommandContext> {
        [Command("listservers"), RequireOwner]
        public async Task GetServerListAsync() {
            string filename = Path.Combine(StaticValues.CTOSModDir, "temp.txt");
            StringBuilder builder = new StringBuilder();

            foreach (SocketGuild server in Context.Client.Guilds)
                builder.Append(server.Name + "\n");

            File.WriteAllText(filename, builder.ToString());
            await Context.Channel.SendFileAsync(filename, "These are all the servers this bot is currently in.");
            File.Delete(filename);
        }
    }
}
