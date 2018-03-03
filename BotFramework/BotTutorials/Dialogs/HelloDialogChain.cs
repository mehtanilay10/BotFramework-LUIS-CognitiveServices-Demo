using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace BotTutorials.Dialogs
{
    public class HelloDialogChain
    {
        public static readonly IDialog<string> dialog = Chain.PostToChain()
            .Select(x => x.Text)
            .Switch(
                Chain.Case(
                    new Regex("^Hello", RegexOptions.IgnoreCase),
                    (cotext, text) => Chain.Return("Welcome to Bot Application.").PostToUser()
                    ),
                Chain.Case(
                    new Regex("How are you", RegexOptions.IgnoreCase),
                    (cotext, text) => Chain.Return("I am fine as always.").PostToUser()
                    ),
                Chain.Default<string, IDialog<string>>(
                    (cotext, text) => Chain.Return("Welcome to Bot Application.").PostToUser()
                    )
            )
            .Unwrap();
    }
}