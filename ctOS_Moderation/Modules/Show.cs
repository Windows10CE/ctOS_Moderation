using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using ctOS_Moderation.Modules.Preconditions;
using static ctOS_Moderation.Modules.JSONHelper;

namespace ctOS_Moderation.Modules {
    public class Show : ModuleBase<SocketCommandContext> {
        [Command("show"), RequireManageMessagesOrRole]
        public async Task ShowWarningsAsync(SocketGuildUser userToCheck, int pageNumber = 1) {
            string filename = Path.Combine(StaticValues.WarningsDir, userToCheck.Id.ToString() + ".json");

            if (File.Exists(filename)) {
                List<JObject> warnings = GetJSONObjects(filename);
                EmbedBuilder warningsToDisplay = new EmbedBuilder();

                int warningListStart = (5 * pageNumber) - 5;
                int warningListEnd = warningListStart + 4;
                double numOfPagesDouble = (double)warnings.Count / 5.0;
                int numOfPages = (int)Math.Round(numOfPagesDouble + 0.5, 0);
                int numOfWarnings = 0;
                int numOfWarningsShown = 0;

                if (pageNumber != 0 && !(pageNumber > numOfPages) || pageNumber == 1) {
                    if (numOfPages == 0) numOfPages = 1;
                    foreach (JObject JObj in warnings) {
                        numOfWarnings++;
                        if (warnings.IndexOf(JObj) >= warningListStart && warnings.IndexOf(JObj) <= warningListEnd) {
                            numOfWarningsShown++;
                            warningsToDisplay
                            .AddField($"Warning {numOfWarningsShown + warningListStart}:", GetJObjectValue(JObj, "Warning"))
                            .AddInlineField("Given By:", GetJObjectValue(JObj, "Given By"))
                            .AddInlineField("Given In:", GetJObjectValue(JObj, "Warning Location"));
                        }
                    }

                    await ReplyAsync($"User @{userToCheck.Username}#{userToCheck.Discriminator} has {warnings.Count} warning(s).\nThere are {numOfPages} page(s).", false, warningsToDisplay.WithColor(Color.DarkRed).Build());
                } else {
                    await ReplyAsync("Invalid page number.");
                }
            } else {
                await ReplyAsync($"{userToCheck.Username}#{userToCheck.Discriminator} has no warnings.");
            }
        }
    }
}
