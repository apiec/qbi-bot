using Discord.Commands;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using SixLabors.ImageSharp.Drawing.Processing;
using SixLabors.ImageSharp.PixelFormats;

namespace QbiBot.Modules
{
    public class DeepFryModule : ModuleBase<SocketCommandContext>
    {
        [Command("fry")]
        public async Task FryAsync()
        {
            var attachment = Context.Message.Attachments.ElementAtOrDefault(0);
            if (attachment is null || attachment.Height is null || attachment.Width is null)
            {
                await ReplyAsync("Didn't find an image attached to this message. Attach an image and try again.");
                return;
            }

            using var client = new HttpClient();
            var response = await client.GetAsync(attachment.Url);
            if (response.IsSuccessStatusCode is false)
            {
                await ReplyAsync("Can't download image.");
                return;
            }

            using var inStream = await response.Content.ReadAsStreamAsync();   
            using var outStream = new MemoryStream();

            using var image = Image.Load(inStream);

            image.Mutate(x => x
                .Resize((int)attachment.Width / 7, (int)attachment.Height / 7)
                .SetGraphicsOptions(o => o.ColorBlendingMode = PixelColorBlendingMode.Multiply)
                .Fill(Color.Yellow)
                .Saturate(3)
                .Contrast(2)
                .Brightness(1.1f)
                .Resize((int)attachment.Width, (int)attachment.Height)
            );

            await image.SaveAsPngAsync(outStream);
            outStream.Position = 0;
            var fileName = attachment.Filename.Split('.')[0];

            await Context.Channel.SendFileAsync(outStream, $"{fileName}_deepfried.png");
        }
    }
}
