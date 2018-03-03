using BotTutorials.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace BotTutorials.Helpers
{
    public class NewsAPIHelper
    {
        public static string NEWS_API_HOME = "https://newsapi.org/v2";

        private static Attachment CreateAttachmentFromArticle(NewsArticle article)
        {
            HeroCard heroCard = new HeroCard
            {
                Title = article.title,
                Subtitle = article.publishedAt.ToString(),
                Text = article.description,
                Images = new List<CardImage> {
                    new CardImage(article.urlToImage),
                },
                Buttons = new List<CardAction> {
                    new CardAction(ActionTypes.OpenUrl, "View Article", value: article.url)
                }
            };

            return heroCard.ToAttachment();

        }

        #region Helper's static Methods

        public static IMessageActivity GetHeadlines(IDialogContext context, string key, int maximumCards = 5)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var url = $"{NEWS_API_HOME}/top-headlines?sources=the-washington-post&apiKey={key}";
            var jsonData = new WebClient().DownloadString(url);

            try
            {
                NewsResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsResponse>(jsonData);
                if (response.status.Equals("ok") && response.articles.Count > 0)
                {
                    foreach (NewsArticle article in response.articles.GetRange(0, maximumCards))
                    {
                        message.Attachments.Add(CreateAttachmentFromArticle(article));
                    }
                }
            }
            catch (Exception ex)
            {
                message.Text = ex.Message;
            }

            return message;
        }

        public static IMessageActivity GetNewsFromSource(IDialogContext context, string key, string source, int maximumCards = 5)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var url = $"{NEWS_API_HOME}/top-headlines?sources={source}&apiKey={key}";
            var jsonData = new WebClient().DownloadString(url);

            try
            {
                NewsResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsResponse>(jsonData);
                if (response.status.Equals("ok") && response.articles.Count > 0)
                {
                    foreach (NewsArticle article in response.articles.Take(maximumCards))
                    {
                        message.Attachments.Add(CreateAttachmentFromArticle(article));
                    }
                }
            }
            catch (Exception ex)
            {
                message.Text = ex.Message;
            }

            return message;
        }

        public static IMessageActivity GetNewsForTopic(IDialogContext context, string key, string query)
        {
            var message = context.MakeMessage();
            message.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            var url = $"{NEWS_API_HOME}/everything?q={query}&sortBy=popularity&apiKey={key}";
            var jsonData = new WebClient().DownloadString(url);
            int maximumCards = 10;

            try
            {
                NewsResponse response = Newtonsoft.Json.JsonConvert.DeserializeObject<NewsResponse>(jsonData);
                if (response.status.Equals("ok") && response.articles.Count > 0)
                {
                    foreach (NewsArticle article in response.articles.Take(maximumCards))
                    {
                        message.Attachments.Add(CreateAttachmentFromArticle(article));
                    }
                }
            }
            catch (Exception ex)
            {
                message.Text = ex.Message;
            }

            return message;
        }

        #endregion
    }

}