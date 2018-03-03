using Microsoft.Azure.CognitiveServices.ContentModerator;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using Microsoft.CognitiveServices.ContentModerator;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class ContentModerator : IDialog<object>
    {
        public const string key = "<ENTER_YOUR_KEY_HERE>";
        public const string baseUrl = "australiaeast.api.cognitive.microsoft.com";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            string text = ((await result) as Activity).Text;
            await context.PostAsync(GetPIIDetails(key, baseUrl, text));
        }

        public string GetPIIDetails(string key, string endpoint, string text)
        {
            IContentModeratorClient client = new ContentModeratorClient(new ApiKeyServiceClientCredentials(key));
            client.BaseUrl = endpoint;

            var result = client.TextModeration.ScreenText("eng", "text/plain", text, true, true);
            string address = string.Join(", ", result.PII.Address.Select(x => x.Text));
            string emails = string.Join(", ", result.PII.Email.Select(x => x.Text));
            string ips = string.Join(", ", result.PII.IPA.Select(x => x.Text));
            string phones = string.Join(", ", result.PII.Phone.Select(x => x.Text));
            return $@"**Address:** {address}<br />**Emails:** {emails}<br />
                    **IPs:** {ips}<br />**Phone No:** {phones}";
        }
    }
}