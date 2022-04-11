using Discord.Commands;
using QbiBot.Services;

namespace QbiBot.Modules
{
    public class AudioModule : ModuleBase<SocketCommandContext>
    {
        private readonly IAudioService audioService;

        public AudioModule(IAudioService audioService)
        {
            this.audioService = audioService;
        }

        [Command("play", RunMode = RunMode.Async)]
        public async Task Play(string url)
        {
            await audioService.ConnectToAudioChannelFromContext(Context);
            await audioService.Stream(url.Split('&')[0]);
        }

        [Command("stop", RunMode = RunMode.Async)]
        public async Task Stop()
        {
            await audioService.LeaveChannel();
        }

    }
}
