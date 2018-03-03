using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class ChannelSpecificDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = (await result) as IMessageActivity;
            var reply = context.MakeMessage();
            switch(message.ChannelId)
            {
                case "telegram":
                    reply.ChannelData = Newtonsoft.Json.JsonConvert.DeserializeObject("{\"method\": \"sendSticker\",\"parameters\": {\"sticker\": {\"url\": \"https://media.giphy.com/media/MqyCeU1saSuEE/giphy_s.gif\",\"mediaType\": \"image/gif\"}}}");
                    break;
                case "slack":
                    reply.ChannelData = Newtonsoft.Json.JsonConvert.DeserializeObject("{\"text\": \"Want to learn more about Payload data for Slack\",\"attachments\": [{\"title\": \"Visit Slack API Documentation\",\"title_link\": \"https://api.slack.com/\",\"image_url\": \"https://blog.agilebits.com/wp-content/uploads/2014/09/Slack-icon.png\",\"color\": \"#3AA3E3\"}, {\"title\": \"Add Your another Message\",\"text\": \"This part contains more escriptive text...\",\"color\": \"#242424\"}]}");
                    break;
                case "facebook":
                    reply.ChannelData = Newtonsoft.Json.JsonConvert.DeserializeObject("{\"attachment\": {\"type\": \"image\",\"payload\": {\"url\": \"https://dummyimage.com/480x350/f0dff0/232540.jpg&text=Facebook%20Bot\",\"is_reusable\": true}}}");
                    break;
                default:
                    reply.Text = "This will be send to other channels";
                    break;
            }
            await context.PostAsync(reply);
        }
    }
}