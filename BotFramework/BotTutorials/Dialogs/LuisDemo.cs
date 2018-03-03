using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Luis;
using Microsoft.Bot.Builder.Luis.Models;
using System.Threading.Tasks;
using BotTutorials.Helpers;

namespace BotTutorials.Dialogs
{
    [Serializable]
    [LuisModel("<ENTER_YOUR_LUIS_APP_ID>", "<ENTER_YOUR_AZURE_SUBSCRIPTION_KEY>")]
    public class LuisDemo: LuisDialog<object>
    {
        private string API_KEY = "<ENTER_YOUR_KEY_HERE>";

        [LuisIntent("")]
        [LuisIntent("None")]
        public async Task None(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I am in default intent");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Welcome")]
        public async Task Greeting(IDialogContext context, LuisResult result)
        {
            await context.PostAsync("I am in Welcome intent");
            context.Wait(MessageReceived);
        }

        [LuisIntent("Display")]
        public async Task Display(IDialogContext context, LuisResult result)
        {
            EntityRecommendation entity = null;
            result.TryFindEntity("builtin.number", out entity);
            if (entity != null)
            {
                object displayCards;
                entity.Resolution.TryGetValue("value", out displayCards);

                await context.PostAsync(NewsAPIHelper.GetHeadlines(context, API_KEY, Convert.ToInt32(displayCards)));
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Sources")]
        public async Task Sources(IDialogContext context, LuisResult result)
        {
            EntityRecommendation entity = null;
            result.TryFindEntity("NewsApp.SourceName", out entity);
            if (entity != null)
            {
                object sourceNames;
                entity.Resolution.TryGetValue("values", out sourceNames);
                string sourceName = ((System.Collections.IEnumerable)sourceNames).Cast<object>().FirstOrDefault().ToString();

                await context.PostAsync(NewsAPIHelper.GetNewsFromSource(context, API_KEY, sourceName));
                context.Wait(MessageReceived);
            }
        }

        [LuisIntent("Topical")]
        public async Task Topical(IDialogContext context, LuisResult result)
        {
            EntityRecommendation entity = null;
            result.TryFindEntity("NewsApp.TopicName", out entity);
            if (entity != null)
            {
                await context.PostAsync(NewsAPIHelper.GetNewsForTopic(context, API_KEY, entity.Entity));
                context.Wait(MessageReceived);
            }
        }
    }
}