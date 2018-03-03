using BotTutorials.Helpers;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class BingSearchDemo : IDialog<object>
    {
        private string searchType = string.Empty;
        private string query = string.Empty;

        private const string BING_KEY = "<ENTER_YOUR_KEY_HERE>";

        private const string searchWeb = "Search Web";
        private const string searchImage = "Search Image";
        private const string searchVideo = "Search Video";
        private const string searchNews = "Search News";
        private const string treadingNews = "Treading News";
        private const string searchEntity = "Search Entity";
        private const string searchPlace = "Search Place";
        private const string checkSpell = "Check Spell";

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Choice(
                context: context,
                resume: ResumeAfterSearchTypeSelecting,
                prompt: "Select search type that you want to perform",
                options: new List<string> {
                    searchWeb,
                    searchImage,
                    searchVideo,
                    searchNews,
                    treadingNews,
                    searchEntity,
                    searchPlace,
                    checkSpell
                }
            );
        }

        private async Task ResumeAfterSearchTypeSelecting(IDialogContext context, IAwaitable<string> result)
        {
            searchType = (await result) as string;
            PromptDialog.Text(
                context: context,
                resume: ResumeAfterEnteringQuery,
                prompt: "Enter your query"
            );
        }

        private async Task ResumeAfterEnteringQuery(IDialogContext context, IAwaitable<string> result)
        {
            query = (await result) as string;
            switch (searchType)
            {
                case searchWeb:
                    {
                        await BingSearchHelper.SearchWebAsync(context, BING_KEY, query);
                        break;
                    }
                case searchImage:
                    {
                        await BingSearchHelper.SearchImageAsync(context, BING_KEY, query);
                        break;
                    }
                case searchVideo:
                    {
                        await BingSearchHelper.SearchVideoAsync(context, BING_KEY, query);
                        break;
                    }
                case searchNews:
                    {
                        await BingSearchHelper.SearchNewsAsync(context, BING_KEY, query);
                        break;
                    }
                case treadingNews:
                    {
                        await BingSearchHelper.GetTreadingNewsAsync(context, BING_KEY);
                        break;
                    }
                case searchEntity:
                    {
                        await BingSearchHelper.SearchEntityAsync(context, BING_KEY, query);
                        break;
                    }
                case searchPlace:
                    {
                        await BingSearchHelper.SearchPlaceAsync(context, BING_KEY, query);
                        break;
                    }
                case checkSpell:
                    {
                        await BingSearchHelper.CheckSpellAsync(context, BING_KEY, query);
                        break;
                    }
            }

            context.Wait(MessageReceivedAsync);
        }
    }
}