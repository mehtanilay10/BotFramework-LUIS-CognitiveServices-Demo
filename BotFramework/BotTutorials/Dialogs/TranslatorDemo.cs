using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Xml;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class TranslatorDemo : IDialog<object>
    {
        public const string key = "<ENTER_YOUR_KEY_HERE>";
        public const string baseUrl = @"http://api.microsofttranslator.com/v2/http.svc/translate?text={0}&from={1}&to={2}&category=general";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            string text = ((await result) as Activity).Text;
            string translation = await TranslateTextAsync(key, text);
            await context.PostAsync(translation);
        }

        public async Task<string> TranslateTextAsync(string key, string text)
        {
            string url = string.Format(baseUrl, text, "en", "de");
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", key);
                var response = await client.GetAsync(url);
                var xml = await response.Content.ReadAsStringAsync();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xml);
                return doc.InnerText;
            }
        }
    }
}