using System;
using System.Collections.Generic;
using System.Text;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Threading.Tasks;
using System.Linq;

namespace ctOS_Moderation.Modules
{
    [Group("warn")]
    public class Warning : ModuleBase<SocketCommandContext>
    {
        [Command]
        public async Task WarnHelpAsync() {
            EmbedBuilder builder = new EmbedBuilder();

            builder
                .AddInlineField("warn show [user mention]", "Shows the warnings of a user.")
                .AddInlineField("warn add [user mention] [warning text]", "Adds a warning to the user. Requires a role called \"ctOS Warnings\"")
                .WithColor(Color.Blue);

            await ReplyAsync("", false, builder.Build());
        }

        [Command("add")]
        public async Task AddWarnAsync(SocketGuildUser userToWarn, [Remainder] string warning) {
            var user = Context.User as SocketGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "ctOS Warnings");
            if (user.Roles.Contains(role)) {
                string userID = userToWarn.Id.ToString();
                string warningsDir = @"c:\Users\aaron\Documents\Moderation\";
                string userWarningsFile = warningsDir + userID + @".json";
                string usernameOfWarned = $"@{userToWarn.Username}#{userToWarn.Discriminator}";

                JObject warningObj = new JObject(
                    new JProperty("Warning Location", $"{Context.Guild.Name} in channel #{Context.Channel.Name}"),
                    new JProperty("Warning", warning),
                    new JProperty("Given By", $"@{Context.User.Username}#{Context.User.Discriminator}"));
                string warningJSONString = warningObj.ToString();

                File.AppendAllText(userWarningsFile, warningJSONString);

                EmbedBuilder builderEnvoker = new EmbedBuilder();
                EmbedBuilder builderDM = new EmbedBuilder();

                builderEnvoker.AddField($"User {usernameOfWarned} has been warned.", $"Warning message: \"{warning}\"").WithColor(Color.DarkRed);
                builderDM
                    .AddInlineField("Warning Message", warning)
                    .AddInlineField("Warned In", $"{Context.Guild.Name} in channel #{Context.Channel.Name}")
                    .AddInlineField("Warned By", Context.User.Mention)
                    .WithColor(Color.DarkRed);

                await ReplyAsync("", false, builderEnvoker.Build());
                var userToDM = userToWarn as IUser;
                await UserExtensions.SendMessageAsync(userToDM, "You have been warned.", false, builderDM.Build());
            } else {
                await ReplyAsync("You don't have the needed role. (ctOS Warnings)");
            }
        }
        [Command("show")]
        public async Task ShowWarningsAsync(SocketGuildUser userToCheck) {
            var user = Context.User as SocketGuildUser;
            var role = Context.Guild.Roles.FirstOrDefault(x => x.Name == "ctOS Warnings");
            if (user.Roles.Contains(role)) {
                string filename = $"c:\\Users\\aaron\\Documents\\Moderation\\{userToCheck.Id}.json";

                if (File.Exists(filename)) {
                    List<JObject> warnings = GetJSONObjects(filename);
                    EmbedBuilder warningsToDisplay = new EmbedBuilder();

                    int numOfWarnings = 0;
                    foreach (JObject JObj in warnings) {
                        numOfWarnings++;
                        warningsToDisplay
                            .AddField($"Warning {numOfWarnings.ToString()}:", GetJObjectValue(JObj, "Warning"))
                            .AddInlineField("Given By:", GetJObjectValue(JObj, "Given By"))
                            .AddInlineField("Given In:", GetJObjectValue(JObj, "Warning Location"));
                    }

                    await ReplyAsync($"User @{userToCheck.Username}#{userToCheck.Discriminator} has {numOfWarnings.ToString()} warning(s).", false, warningsToDisplay.WithColor(Color.DarkRed).Build());
                } else {
                    await ReplyAsync($"{userToCheck.Username}#{userToCheck.Discriminator} has no warnings.");
                }
            }
        }

        public List<JObject> GetJSONObjects(string filename) {
            int BracketCount = 0;
            string JSONString = File.ReadAllText(filename);
            List<string> JsonItems = new List<string>();
            StringBuilder Json = new StringBuilder();

            foreach (char c in JSONString) {
                if (c == '{')
                    ++BracketCount;
                else if (c == '}')
                    --BracketCount;
                Json.Append(c);

                if (BracketCount == 0 && c != ' ') {
                    JsonItems.Add(Json.ToString());
                    Json = new StringBuilder();
                }
            }
            List<JObject> JObjs = new List<JObject>();

            foreach(string JObj in JsonItems) {
                JObjs.Add(JObject.Parse(JObj));
            }
            return JObjs;
        }
        string GetJObjectValue(JObject array, string key) {
            foreach (KeyValuePair<string, JToken> keyValuePair in array) {
                if (key == keyValuePair.Key) {
                    return keyValuePair.Value.ToString();
                }
            }
            if (key == "Threat Level") {
                return "No Threat Level Data Found";
            } else {
                Console.WriteLine($"Error, no key found for {key}");
                return String.Empty;
            }
        }
    }
}
