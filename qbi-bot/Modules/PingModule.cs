using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Globalization;
using System.Text;

namespace QbiBot.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Responds \"pong\" to \"ping\"")]
        public Task PingAsync() => ReplyAsync("pong");

        [Command("check")]
        [Summary("Responds \"pong\" to \"ping\"")]
        public Task Check() => ReplyAsync("Checking in 😎");

        [Command("square")]
        public Task Square(int n) => ReplyAsync($"{n * n}");

        [Command("spierdalaj")]
        [Summary("😎")]
        public Task SpierdalajAsync(SocketUser user)
            => ReplyAsync($"Spierdalaj {user.Mention} 😎");

        [Command("uncurse")]
        [Summary("Uncursed cursed text")]
        private Task Uncurse([Remainder]string text)
            => ReplyAsync(RemoveDiacritics(text));
        

        private static string RemoveDiacritics(string text)
        {
            var normalizedString = text.Normalize(NormalizationForm.FormD);
            var stringBuilder = new StringBuilder();

            foreach (var c in normalizedString)
            {
                var unicodeCategory = CharUnicodeInfo.GetUnicodeCategory(c);
                if (unicodeCategory != UnicodeCategory.NonSpacingMark)
                {
                    stringBuilder.Append(c);
                }
            }

            return stringBuilder.ToString().Normalize(NormalizationForm.FormC);
        }

    }
}
