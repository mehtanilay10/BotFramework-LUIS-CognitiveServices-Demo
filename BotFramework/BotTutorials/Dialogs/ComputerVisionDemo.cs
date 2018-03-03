using BotTutorials.Helpers;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BotTutorials.Dialogs
{
    [Serializable]
    public class ComputerVisionDemo : IDialog<object>
    {
        string operation = string.Empty;

        const string obtainDescriptionFromImage = "Description from Image";
        const string obtainFacesInImage = "Faces in Image";
        const string obtainTagsForImage = "Tags for Image";
        const string obtainImageDetails = "Image details";
        const string obtainTextFromImage = "Text from Image";
        const string obtainFaceDetails = "Obtain Face details";
        const string compareFaces = "Compare faces";
        const string obtainEmotions = "Obtain Emotions";

        const string VISION_KEY = "<ENTER_YOUR_KEY_HERE>";
        const AzureRegions VISION_REGION = AzureRegions.Australiaeast;
        const string VISION_ENDPOINT = "https://australiaeast.api.cognitive.microsoft.com/vision/v1.0";

        const string FACE_KEY = "<ENTER_YOUR_KEY_HERE>";
        const string FACE_ENDPOINT = "https://australiaeast.api.cognitive.microsoft.com/face/v1.0";


        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);
            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            PromptDialog.Choice(
                context: context,
                resume: ResumeAfterOperationSelecting,
                prompt: "Select operation that you want to perform",
                options: new List<string>
                {
                    obtainDescriptionFromImage,
                    obtainFacesInImage,
                    obtainTagsForImage,
                    obtainImageDetails,
                    obtainTextFromImage,
                    obtainFaceDetails,
                    compareFaces,
                    obtainEmotions
                }
            );
        }

        private async Task ResumeAfterOperationSelecting(IDialogContext context, IAwaitable<object> result)
        {
            operation = (await result) as string;
            PromptDialog.Attachment(
                context: context,
                prompt: "Upload Image to perform operation",
                resume: ResumeAfterRecievingAttachment
            );
        }

        private async Task ResumeAfterRecievingAttachment(IDialogContext context, IAwaitable<IEnumerable<Attachment>> result)
        {
            var images = await result;
            {
                switch (operation)
                {
                    case obtainDescriptionFromImage:
                        {
                            foreach (var image in images)
                            {
                                string caption = await CognitiveImageHelper.GetImageDescriptionAsync(VISION_KEY, VISION_REGION, image.ContentUrl);
                                await context.PostAsync(caption);
                            }
                            break;
                        }
                    case obtainFacesInImage:
                        {
                            foreach (var image in images)
                            {
                                string faceDetails = await CognitiveImageHelper.GetFaceWithInImageAsync(VISION_KEY, VISION_REGION, image.ContentUrl);
                                await context.PostAsync(faceDetails);
                            }
                            break;
                        }
                    case obtainTagsForImage:
                        {
                            foreach (var image in images)
                            {
                                string tags = await CognitiveImageHelper.GetTagsForImageAsync(VISION_KEY, VISION_REGION, image.ContentUrl);
                                await context.PostAsync(tags);
                            }
                            break;
                        }
                    case obtainImageDetails:
                        {
                            foreach (var image in images)
                            {
                                string text = await CognitiveImageHelper.GetImageDetailsAsync(VISION_KEY, VISION_REGION, image.ContentUrl);
                                await context.PostAsync(text);
                            }
                            break;
                        }
                    case obtainTextFromImage:
                        {
                            foreach (var image in images)
                            {
                                string text = await CognitiveImageHelper.GetTextFromImageAsync(VISION_KEY, VISION_ENDPOINT, image.ContentUrl);
                                await context.PostAsync(text);
                            }
                            break;
                        }
                    case obtainFaceDetails:
                        {
                            foreach (var image in images)
                            {
                                string text = await CognitiveImageHelper.GetFaceDetailsAsync(FACE_KEY, FACE_ENDPOINT, image.ContentUrl);
                                await context.PostAsync(text);
                            }
                            break;
                        }
                    case compareFaces:
                        {
                            foreach (var image in images)
                            {
                                string text = await CognitiveImageHelper.CompareFaceAsync(FACE_KEY, FACE_ENDPOINT, image.ContentUrl);
                                await context.PostAsync(text);
                            }
                            break;
                        }
                    case obtainEmotions:
                        {
                            foreach (var image in images)
                            {
                                string text = await CognitiveImageHelper.GetEmotionsAsync(FACE_KEY, FACE_ENDPOINT, image.ContentUrl);
                                await context.PostAsync(text);
                            }
                            break;
                        }
                }
            }
            context.Wait(MessageReceivedAsync);
        }
    }
}