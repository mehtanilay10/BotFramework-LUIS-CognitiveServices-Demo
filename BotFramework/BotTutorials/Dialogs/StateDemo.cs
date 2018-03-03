using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Web;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class StateDemo : IDialog<object>
    {
        const string STATE_KEY = "DefaultSearchEngine";

        Dictionary<string, string> searchEngineURLs = new Dictionary<string, string>
        {
            { "Google", "https://www.google.co.in/search?q=" },
            { "Bing", "http://www.bing.com/search?q=" },
            { "Yahoo", "https://search.yahoo.com/search?q=" },
            { "DuckDuckGo", "https://duckduckgo.com/?q=" },
        };

        public async Task StartAsync(IDialogContext context)
        {
            context.ConversationData.SetValue(STATE_KEY, "Bing");
            context.Wait(this.MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var message = (await result) as IMessageActivity;
            if(message.Text.Equals("change"))
            {
                PromptDialog.Choice(
                    context: context,
                    prompt: "Select default search engine",
                    resume: ResumeAfterChangingSearchEngine,
                    options: searchEngineURLs.Keys
                );
                return;
            }
            else if(message.Text.Equals("default"))
            {
                string userDefault = string.Empty;
                string conversationDefault = context.ConversationData.GetValue<string>(STATE_KEY);
                if (context.PrivateConversationData.TryGetValue<string>(STATE_KEY, out userDefault))
                    await context.PostAsync($"Your default search engine is **{userDefault}** and conversation's default search engine is **{conversationDefault}**.");
                else
                    await context.PostAsync($"Conversation's default search engine is **{conversationDefault}**");
            }
            else
            {
                string defaultSearch;
                context.PrivateConversationData.TryGetValue<string>(STATE_KEY, out defaultSearch);
                if (string.IsNullOrEmpty(defaultSearch))
                    defaultSearch = context.ConversationData.GetValue<string>(STATE_KEY);

                await context.PostAsync($"{searchEngineURLs[defaultSearch]}{HttpUtility.UrlEncode(message.Text)}");
            }

            context.Wait(MessageReceivedAsync);
        }

        private async Task ResumeAfterChangingSearchEngine(IDialogContext context, IAwaitable<string> result)
        {
            var newDefault = await result;
            context.PrivateConversationData.SetValue(STATE_KEY, newDefault);
            await context.PostAsync($"Your default search engine is changed to **{newDefault}**");
            context.Wait(this.MessageReceivedAsync);
        }
    }
}