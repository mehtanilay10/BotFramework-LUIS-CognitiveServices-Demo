using BotTutorials.Models;
using Microsoft.Bot.Connector;
using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace BotTutorials.Controllers
{
    public class ProactiveMessageController : ApiController
    {
        public async Task<HttpResponseMessage> Post([FromBody] ProactiveReply proactiveMessage)
        {
            var jsonData = File.ReadAllText(System.Web.Hosting.HostingEnvironment.MapPath("~/data.json"));
            ProactiveMessageData proactiveData = Newtonsoft.Json.JsonConvert.DeserializeObject<ProactiveMessageData>(jsonData);

            ConversationReference conversationRef = new ConversationReference
            {
                ActivityId = proactiveData.ActivityId,
                Bot = new ChannelAccount(proactiveData.FromId, proactiveData.FromName),
                ChannelId = proactiveData.ChannelId,
                Conversation = new ConversationAccount(id: proactiveData.ConversationId),
                User = new ChannelAccount(proactiveData.RecipientId, proactiveData.RecipientName),
                ServiceUrl = proactiveData.ServiceUrl
            };

            var reply = conversationRef.GetPostToUserMessage().CreateReply();
            reply.Text = $"Hey, I got solution for **{proactiveData.Message}**, {proactiveMessage.Text}";

            var connector = new ConnectorClient(new Uri(proactiveData.ServiceUrl));
            await connector.Conversations.ReplyToActivityAsync(reply);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
