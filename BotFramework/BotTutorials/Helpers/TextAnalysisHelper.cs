using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics;
using Microsoft.Azure.CognitiveServices.Language.TextAnalytics.Models;
using System.Threading.Tasks;

namespace BotTutorials.Helpers
{
    public class TextAnalysisHelper
    {
        public static async Task<string> ObtainLanguage(string key, AzureRegions region, string text)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI();
            client.SubscriptionKey = key;
            client.AzureRegion = region;

            BatchInput input = new BatchInput(new List<Input>()
            {
                new Input("1", text)
            });
            var result = await client.DetectLanguageAsync(input);
            var languages = result.Documents.FirstOrDefault()?.DetectedLanguages;
            if (languages.Count > 0)
                return string.Join(", ", languages.Select(x => x.Name));
            else
                return "Unable to detect language!";
        }

        public static async Task<string> ObtainKeyPhrase(string key, AzureRegions region, string text)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI();
            client.SubscriptionKey = key;
            client.AzureRegion = region;

            MultiLanguageBatchInput input = new MultiLanguageBatchInput(new List<MultiLanguageInput>()
            {
                new MultiLanguageInput("en", "1", text)
            });
            var result = await client.KeyPhrasesAsync(input);
            var keyphrases = result.Documents.FirstOrDefault()?.KeyPhrases;
            if (keyphrases.Count > 0)
                return string.Join(", ", keyphrases);
            else
                return "Not found any Keyphrases!";
        }

        public static async Task<string> ObtainSentiment(string key, AzureRegions region, string text)
        {
            ITextAnalyticsAPI client = new TextAnalyticsAPI();
            client.SubscriptionKey = key;
            client.AzureRegion = region;

            MultiLanguageBatchInput input = new MultiLanguageBatchInput(new List<MultiLanguageInput>()
            {
                new MultiLanguageInput("en", "1", text)
            });
            var result = await client.SentimentAsync(input);
            var score = result.Documents.FirstOrDefault()?.Score;
            return $"Score for {text} is **{score}**";
        }
    }
}