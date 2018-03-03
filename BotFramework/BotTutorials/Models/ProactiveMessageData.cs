namespace BotTutorials.Models
{
    public class ProactiveReply
    {
        public string Text { get; set; }
    }

    public class ProactiveMessageData
    {
        public string ActivityId { get; set; }
        public string FromId { get; set; }
        public string FromName { get; set; }
        public string RecipientId { get; set; }
        public string RecipientName { get; set; }
        public string ServiceUrl { get; set; }
        public string ChannelId { get; set; }
        public string ConversationId { get; set; }
        public string Message { get; set; }
    }
}