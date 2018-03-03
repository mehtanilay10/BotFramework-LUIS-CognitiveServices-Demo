using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.IO;
using System.Threading.Tasks;
using BotTutorials.Models;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class ProactiveMessageDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = context.Activity as Activity;
            ProactiveMessageData data = new ProactiveMessageData
            {
                ActivityId = activity.Id,
                ChannelId = activity.ChannelId,
                ConversationId = activity.Conversation.Id,
                FromId = activity.From.Id,
                FromName = activity.From.Name,
                Message = activity.Text,
                RecipientId = activity.Recipient.Id,
                RecipientName = activity.Recipient.Name,
                ServiceUrl = activity.ServiceUrl
            };

            var textData = Newtonsoft.Json.JsonConvert.SerializeObject(data);
            File.WriteAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/data.json"), textData);

            await context.PostAsync("We received your query and reply you as i got solution.");
        }
    }
}