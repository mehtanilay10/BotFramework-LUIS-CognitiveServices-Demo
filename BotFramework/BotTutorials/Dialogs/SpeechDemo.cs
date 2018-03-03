using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class SpeechDemo : IDialog<object>
    {
        public async Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = context.Activity as Activity;
            Activity reply = activity.CreateReply(@"Your order no: ######## has been registered.
                                <br />You will receive it before this weekend. <br />Thanks for order.");
            reply.Speak = "YOur order is received. And its details are here.";
            await context.PostAsync(reply);
        }
    }
}