using System.Threading.Tasks;
using Discord.Commands;
using Microsoft.CodeAnalysis.CSharp.Scripting;
using Microsoft.CodeAnalysis.Scripting;

namespace ctOS_Moderation.Modules {
    public class Eval : ModuleBase<SocketCommandContext> {
        public class DiscordGlobals {
            public SocketCommandContext Context;
        }
        [Command("eval"), RequireOwner]
        public async Task EvalAsync([Remainder] string command) {
            DiscordGlobals globals = new DiscordGlobals { Context = this.Context };
            string result = (await CSharpScript.EvaluateAsync(command, ScriptOptions.Default.WithImports("System").WithReferences("System.Core").WithImports("System.Linq"), globals)).ToString();
            if (!string.IsNullOrEmpty(result))
                await ReplyAsync(result);
        }
    }
}
