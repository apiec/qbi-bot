using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace QbiBot.Modules
{
    public class PingModule : ModuleBase<SocketCommandContext>
    {
        [Command("ping")]
        [Summary("Responds \"pong\" to \"ping\"")]
        public Task PingAsync() => ReplyAsync("pong");

        [Command("square")]
        public Task Square(int n) => ReplyAsync($"{n * n}");

        [Command("spierdalaj")]
        [Summary("😎")]
        public Task SpierdalajAsync(SocketUser user)
            => ReplyAsync($"Spierdalaj {user.Mention} 😎");

    }
}
