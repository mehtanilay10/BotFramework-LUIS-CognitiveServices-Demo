using System;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch;
using Microsoft.Azure.CognitiveServices.Search.ImageSearch.Models;
using Microsoft.Azure.CognitiveServices.Search.VideoSearch;
using Microsoft.Azure.CognitiveServices.Search.EntitySearch;
using Microsoft.Azure.CognitiveServices.Search.NewsSearch;
using Microsoft.Azure.CognitiveServices.SpellCheck;

namespace BotTutorials.Helpers
{
    public class BingSearchHelper
    {
        public async static Task SearchWebAsync(IDialogContext context, string key, string query)
        {
            IWebSearchAPI client = new WebSearchAPI(new Microsoft.Azure.CognitiveServices.Search.WebSearch.ApiKeyServiceClientCredentials(key));
            var result = await client.Web.SearchAsync(query: query, count: 3, safeSearch: Microsoft.Azure.CognitiveServices.Search.WebSearch.Models.SafeSearch.Strict);

            if(result?.WebPages?.Value?.Count > 0)
            {
                await context.PostAsync($"Here is top 3 web search result for **{query}**");
                foreach(var item in result.WebPages.Value)
                {
                    HeroCard card = new HeroCard
                    {
                        Title = item.Name,
                        Text = item.Snippet,
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "Open Page", value: item.Url)
                        }
                    };

                    var message = context.MakeMessage();
                    message.Attachments.Add(card.ToAttachment());
                    await context.PostAsync(message);
                }
            }
        }

        public async static Task SearchImageAsync(IDialogContext context, string key, string query)
        {
            IImageSearchAPI client = new ImageSearchAPI(new Microsoft.Azure.CognitiveServices.Search.ImageSearch.ApiKeyServiceClientCredentials(key));
            var result = await client.Images.SearchAsync(query: query, count: 5, imageType: ImageType.Line, aspect: ImageAspect.Square);

            if(result?.Value?.Count > 0)
            {
                await context.PostAsync($"Here is top 5 Image search result for **{query}**");
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach(var item in result.Value)
                {
                    HeroCard card = new HeroCard
                    {
                        Title = item.Name,
                        Images = new List<CardImage>
                        {
                            new CardImage(item.ContentUrl)
                        },
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "View Image", value: item.ContentUrl)
                        }
                    };
                    message.Attachments.Add(card.ToAttachment());
                }
                await context.PostAsync(message);
            }
        }

        public async static Task SearchVideoAsync(IDialogContext context, string key, string query)
        {
            IVideoSearchAPI client = new VideoSearchAPI(new Microsoft.Azure.CognitiveServices.Search.VideoSearch.ApiKeyServiceClientCredentials(key));
            var result = await client.Videos.SearchAsync(query: query, count: 3, length: Microsoft.Azure.CognitiveServices.Search.VideoSearch.Models.VideoLength.Short, freshness: Microsoft.Azure.CognitiveServices.Search.VideoSearch.Models.Freshness.Week);

            if (result?.Value?.Count > 0)
            {
                await context.PostAsync($"Here is top 5 Video search result for **{query}**");
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (var item in result.Value)
                {
                    HeroCard card = new HeroCard
                    {
                        Title = item.Name,
                        Images = new List<CardImage>
                        {
                            new CardImage(item.ThumbnailUrl)
                        },
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "View Video", value: item.ContentUrl)
                        }
                    };
                    message.Attachments.Add(card.ToAttachment());
                }
                await context.PostAsync(message);
            }
        }

        public async static Task SearchNewsAsync(IDialogContext context, string key, string query)
        {
            INewsSearchAPI client = new NewsSearchAPI(new Microsoft.Azure.CognitiveServices.Search.NewsSearch.ApiKeyServiceClientCredentials(key));
            var result = await client.News.SearchAsync(query: query, count: 3);

            if (result?.Value?.Count > 0)
            {
                await context.PostAsync($"Here is top 5 News search result for **{query}**");
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (var item in result.Value)
                {
                    HeroCard card = new HeroCard
                    {
                        Title = item.Name,
                        Text = item.Description,
                        Images = new List<CardImage>
                        {
                            new CardImage(item.Image.Thumbnail.ContentUrl)
                        },
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "Read More", value: item.Url)
                        }
                    };
                    message.Attachments.Add(card.ToAttachment());
                }
                await context.PostAsync(message);
            }
        }

        public async static Task GetTreadingNewsAsync(IDialogContext context, string key)
        {
            INewsSearchAPI client = new NewsSearchAPI(new Microsoft.Azure.CognitiveServices.Search.NewsSearch.ApiKeyServiceClientCredentials(key));
            var result = await client.News.TrendingAsync(count: 3);

            if (result?.Value?.Count > 0)
            {
                await context.PostAsync($"Here is top 5 Treading News");
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (var item in result.Value)
                {
                    HeroCard card = new HeroCard
                    {
                        Title = item.Name,
                        Images = new List<CardImage>
                        {
                            new CardImage(item.Image.Url)
                        },
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "Read More", value: item.Url)
                        }
                    };
                    message.Attachments.Add(card.ToAttachment());
                }
                await context.PostAsync(message);
            }
        }

        public async static Task SearchEntityAsync(IDialogContext context, string key, string query)
        {
            IEntitySearchAPI client = new EntitySearchAPI(new Microsoft.Azure.CognitiveServices.Search.EntitySearch.ApiKeyServiceClientCredentials(key));
            var result = await client.Entities.SearchAsync(query: query);

            if (result?.Entities?.Value?.Count > 0)
            {
                await context.PostAsync($"Entity search: **{query}**");
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (var item in result.Entities.Value)
                {
                    HeroCard card = new HeroCard
                    {
                        Title = item.Name,
                        Text = item.Description,
                        Images = new List<CardImage>
                        {
                            new CardImage(item.Image.HostPageUrl)
                        },
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "Read More", value: item.Url)
                        }
                    };
                    message.Attachments.Add(card.ToAttachment());
                }
                await context.PostAsync(message);
            }
        }

        public async static Task SearchPlaceAsync(IDialogContext context, string key, string query)
        {
            IEntitySearchAPI client = new EntitySearchAPI(new Microsoft.Azure.CognitiveServices.Search.EntitySearch.ApiKeyServiceClientCredentials(key));
            var result = await client.Entities.SearchAsync(query: query);

            if (result?.Places?.Value?.Count > 0)
            {
                await context.PostAsync($"Places search: **{query}**");
                var message = context.MakeMessage();
                message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

                foreach (var item in result.Places.Value)
                {
                    var place = item as Microsoft.Azure.CognitiveServices.Search.EntitySearch.Models.Place;
                    HeroCard card = new HeroCard
                    {
                        Title = place.Name,
                        Text = $"Address: {place.Address.AddressLocality}, {place.Address.AddressRegion}, {place.Address.AddressCountry} Postal: {place.Address.PostalCode}",
                        Subtitle = $"Telephone: {place.Telephone}",
                        Buttons = new List<CardAction>
                        {
                            new CardAction(ActionTypes.OpenUrl, "View More", value: place.WebSearchUrl)
                        }
                    };
                    message.Attachments.Add(card.ToAttachment());
                }
                await context.PostAsync(message);
            }
        }

        public async static Task CheckSpellAsync(IDialogContext context, string key, string query)
        {
            ISpellCheckAPI client = new SpellCheckAPI(new Microsoft.Azure.CognitiveServices.SpellCheck.ApiKeyServiceClientCredentials(key));
            var result = await client.SpellCheckerAsync(text: query);
            string suggestionText = query;
            foreach(var item in result.FlaggedTokens)
            {
                var firstSugestion = item.Suggestions.FirstOrDefault();
                if (firstSugestion != null)
                    suggestionText = suggestionText.Replace(item.Token, $"**{firstSugestion.Suggestion}**");
            }

            if (query.Equals(suggestionText))
                await context.PostAsync("Not found any suggestions.");
            else
                await context.PostAsync($"Did you mean: {suggestionText}");
        }
    }
}