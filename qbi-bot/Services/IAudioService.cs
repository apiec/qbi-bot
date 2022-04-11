using Discord.Audio;
using Discord.Commands;

namespace QbiBot.Services
{
    public interface IAudioService
    {
        Task ConnectToAudioChannelFromContext(SocketCommandContext context);
        Task Stream(string url);
        Task LeaveChannel();
    }
}