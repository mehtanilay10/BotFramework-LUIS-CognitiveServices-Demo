using System;
using System.Collections.Generic;

namespace BotTutorials.Models
{
    public class NewsArticle
    {
        public NewsSource source { get; set; }
        public string author { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public string urlToImage { get; set; }
        public DateTime? publishedAt { get; set; }
    }

    public class NewsResponse
    {
        public string status { get; set; }
        public int totalRecords { get; set; }
        public List<NewsArticle> articles { get; set; }
    }

    public class NewsSource
    {
        public string id { get; set; }
        public string name { get; set; }
    }
}