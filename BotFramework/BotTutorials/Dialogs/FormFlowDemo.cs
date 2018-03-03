using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.FormFlow;

namespace BotTutorials.Dialogs
{
    public enum LaptopBrand
    {
        HP, Dell, Lenovo, Acer, Microsoft
    }

    public enum LaptopType
    {
        Laptop, Gaming, Ultrabook, Netbook,
    }

    public enum LaptopProcessor
    {
        [Describe("Intel Core I3")]
        [Terms("i3")]
        IntelCoreI3, IntelCoreI5, IntelCoreI7, IntelCoreI9,
        [Terms("amd")]
        AMDDualCore, IntelCoreM
    }

    public enum LaptopOperatingSystem
    {
        Windows8, Windows10, MSDos, Linux
    }

    [Serializable]
    public class FormFlowDemo
    {
        public LaptopType? LaptopType;

        [Optional]
        [Describe(description:"Company", title:"Laptop Brand", subTitle: "There are several other brand present but we are not selling those.")]
        public LaptopBrand? Brand;
        public LaptopProcessor? Processor;

        [Template(TemplateUsage.EnumSelectOne, "Select preferable {&}: {||}", ChoiceStyle =ChoiceStyleOptions.PerLine)]
        public LaptopOperatingSystem? OperatingSystem;


        [Describe("touch screen devices")]
        [Template( TemplateUsage.Bool, "Do you prefer {&}? {||}", ChoiceStyle =ChoiceStyleOptions.Inline)]
        public bool? RequiresTouch;

        [Numeric(2, 12)]
        [Describe(description:"Minimum capacity of RAM")]
        [Template(TemplateUsage.NotUnderstood, "Unable to understood")]
        public int? MinimumRamSize;

        [Pattern(@"^[789]\d{9}$")]
        public string UserMobileNo;

        public static IForm<FormFlowDemo> GetForm()
        {
            OnCompletionAsyncDelegate<FormFlowDemo> onFormComplition = async (context, state) =>
            {
                await context.PostAsync(@"We have noted configuration that you required. We will inform you as we got it.");
            };

            return new FormBuilder<FormFlowDemo>()
                .Message("Welcome to Laptop suggestion Bot")
                .Field(nameof(Processor))
                .Confirm(async(state) =>
                {
                    int price = 0;
                    switch(state.Processor)
                    {
                        case LaptopProcessor.IntelCoreI3: price = 200; break;
                        case LaptopProcessor.IntelCoreI5: price = 300; break;
                        case LaptopProcessor.IntelCoreI7: price = 400; break;
                        case LaptopProcessor.IntelCoreI9: price = 500; break;
                        case LaptopProcessor.AMDDualCore: price = 250; break;
                        case LaptopProcessor.IntelCoreM: price = 280; break;
                    }
                    return new PromptAttribute($"Minimum price for this processor will be {price}. Is this okay?");
                })
                .Field(nameof(UserMobileNo), 
                    validate: async(state, response) => {
                        var validation = new ValidateResult { IsValid = true, Value = response };
                        if((response as string).Equals("9876543210"))
                        {
                            validation.IsValid = false;
                            validation.Feedback = "9876543210 is not allowed";
                        }
                        return validation;
                    })
                .Confirm("You required Laptop with {Processor} and your Mobile no is {UserMobileNo}")
                .OnCompletion(onFormComplition)
                .Build();
        }
    }
}