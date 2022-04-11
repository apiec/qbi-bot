using Discord;
using Discord.Audio;
using Discord.Commands;
using Discord.WebSocket;
using System.Diagnostics;

namespace QbiBot.Services
{
    public class AudioService : IAudioService
    {
        private IAudioClient? _client;

        public async Task ConnectToAudioChannelFromContext(SocketCommandContext context)
        {
            var user = context.User as SocketGuildUser;

            if (user?.VoiceChannel is null)
            {
                await context.Message.Channel.SendMessageAsync("User must be in a voice channel, or a voice channel must be passed as an argument.");
            }
            else
            {
                if (_client is not null)
                {
                    _client.Dispose();
                }
                _client = await user.VoiceChannel.ConnectAsync();
            }            
        }

        public async Task LeaveChannel()
        {
            if (_client is not null)
            {
                await _client.StopAsync();
                _client.Dispose();
                _client = null;
            }
        }

        public async Task Stream(string url)
        {
            if (_client is null)
                return;

            using var stream = CreateYoutubeStream(url);
            using var output = stream.StandardOutput.BaseStream;
            using var discord = _client.CreatePCMStream(AudioApplication.Mixed, 96000);
            try { await output.CopyToAsync(discord); }
            finally { await discord.FlushAsync(); }
        }

        private Process CreateYoutubeStream(string url)
        {
            ProcessStartInfo ffmpeg = new ProcessStartInfo
            {
                FileName = "bash",
                Arguments = $"-c \"yt-dlp --no-check-certificate -f bestaudio -o - {url} | ffmpeg -i pipe:0 -f s16le -ar 48000 -ac 2 pipe:1 -loglevel quiet\"",
                //Arguments = $@"/C C:\mpv\youtube-dl.exe --no-check-certificate -f bestaudio -o - {url} | C:\ffmpeg\bin\ffmpeg.exe -i pipe:0 -f s16le -ar 48000 -ac 2 pipe:1",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                CreateNoWindow = true
            };

            return Process.Start(ffmpeg);
        }

    }
}
