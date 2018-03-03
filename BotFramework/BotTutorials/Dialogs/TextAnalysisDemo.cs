using BotTutorials.Helpers;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class TextAnalysisDemo : IDialog<object>
    {
        public const string key = "<ENTER_YOUR_KEY_HERE>";
        public const AzureRegions region = AzureRegions.Westus;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            string text = ((await result) as Activity).Text;
            string message = await TextAnalysisHelper.ObtainSentiment(key, region, text);
            await context.PostAsync(message);
        }
    }
}