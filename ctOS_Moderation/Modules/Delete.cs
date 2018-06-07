using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ctOS_Moderation.Modules.JSONHelper;

namespace ctOS_Moderation.Modules {
    public class Delete : ModuleBase<SocketCommandContext> {
        [Command("delete")]
        public async Task RemoveWarningAsync(SocketGuildUser userToRemove, string warningNumber) {
            var user = Context.User as SocketGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "ctOS Warnings");

            if (!user.Roles.Contains(role) && !user.GuildPermissions.ManageMessages) {
                await ReplyAsync("You do not have the required permissions!");
                return;
            }

            string filename = Path.Combine(StaticValues.WarningsDir, userToRemove.Id + ".json");
            if (!File.Exists(filename)) {
                await ReplyAsync("The user does not have any warnings to remove!");
                return;
            }
            if (!int.TryParse(warningNumber, out int a) && warningNumber.ToLower() != "all") {
                await ReplyAsync("Please give a proper warning number, or the \"all\" variable!");
                return;
            }
            List<JObject> jObjs = GetJSONObjects(filename);
            List<WarningObj> serverWarns = new List<WarningObj>();
            int i = 1;
            foreach (JObject jObj in jObjs)
                if (GetJObjectValue(jObj, "Server ID") == Context.Guild.Id.ToString()) {
                    serverWarns.Add(new WarningObj {
                        Warning = GetJObjectValue(jObj, "Warning"),
                        WarningNumber = i,
                        Object = jObj
                    });
                    i++;
                }
            if (!(serverWarns.Count > 0)) {
                await ReplyAsync("This server has never given any warnings to this user!");
                return;
            }

            bool warningIsServerWarn = false;
            if (warningNumber.ToLower() == "all") {
                foreach (WarningObj warning in serverWarns)
                    jObjs.Remove(warning.Object);
                warningIsServerWarn = true;
            } else {
                int warningNumberInt = int.Parse(warningNumber);
                foreach (WarningObj warning in serverWarns) {
                    if (warningNumberInt == warning.WarningNumber) {
                        jObjs.Remove(warning.Object);
                        warningIsServerWarn = true;
                    }
                }
            }
            if (!warningIsServerWarn)
                await ReplyAsync("That warning is not given from this server!");

            StringBuilder builder = new StringBuilder();
            foreach (JObject jObj in jObjs)
                builder.Append(jObj.ToString() + "\n");
            if (jObjs.Count > 0)
                await File.WriteAllTextAsync(filename, builder.ToString());
            else
                File.Delete(filename);
            await ReplyAsync("The warning(s) have been deleted!");
        }
        public class WarningObj {
            public string Warning;
            public int WarningNumber;
            public JObject Object;
        }
    }
}
