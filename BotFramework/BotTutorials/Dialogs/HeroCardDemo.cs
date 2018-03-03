using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class HeroCardDemo : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageRecievedAsync);
        }

        private async Task MessageRecievedAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = context.MakeMessage();
            var activity = await result;

            //message.AttachmentLayout = AttachmentLayoutTypes.Carousel;
            message.Attachments.Add(GetCard(activity.Text));
            //message.Attachments.Add(GetCard("OneMoreImage"));
            //message.Attachments.Add(GetCard("LAstImage"));
            await context.PostAsync(message);
        }

        private Attachment GetCard(string title)
        {
            Dictionary<string, string> imageUrls = new Dictionary<string, string>
            {
                {"Audio", "http://is1.mzstatic.com/image/thumb/Purple3/v4/38/d1/f9/38d1f995-9867-89b3-8ffb-5c300be01f11/source/1024x1024sr.jpg" },
                {"Video", "https://cdn2.iconfinder.com/data/icons/metro-ui-dock/512/Windows_Media_Player_alt.png" },
                {"Animation", "https://i-bitzedge.com/wp-content/uploads/2017/08/windows-photo-icon.png" }
            };
            Dictionary<string, string> mediaUrls = new Dictionary<string, string>
            {
                {"Audio", "https://freewavesamples.com/files/1980s-Casio-Celesta-C5.wav" },
                {"Video", "http://sample-videos.com/video/mp4/240/big_buck_bunny_240p_1mb.mp4" },
                {"Animation", "https://infinity.noelblack.com/files/icons-animation.gif" }
            };

            string imageUrl = imageUrls[title];
            string mediaUrl = mediaUrls[title];

            var heroCard = new AnimationCard()
            {
                Title = title,
                Subtitle = "Subtitle for a card",
                Text = "Some descriptive text that will display on card",
                Image = new ThumbnailUrl
                {
                    Url = imageUrl
                },
                Autoloop = true,
                Autostart = true,
                Media = new List<MediaUrl>
                {
                    new MediaUrl(mediaUrl)
                },
                Buttons = new List<CardAction>
                {
                    new CardAction(ActionTypes.OpenUrl, "View Media File", value: mediaUrl)
                }
            };

            return heroCard.ToAttachment();
        }
    }
}