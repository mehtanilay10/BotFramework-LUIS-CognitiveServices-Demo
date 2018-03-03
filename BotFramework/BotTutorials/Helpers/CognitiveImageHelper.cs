using Microsoft.Azure.CognitiveServices.Vision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision;
using Microsoft.Azure.CognitiveServices.Vision.ComputerVision.Models;
using Microsoft.ProjectOxford.Vision;
using Microsoft.ProjectOxford.Common;
using Microsoft.ProjectOxford.Face;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace BotTutorials.Helpers
{
    public class CognitiveImageHelper
    {
        public static async Task<string> GetImageDescriptionAsync(string key, AzureRegions region, string imageUrl)
        {
            IComputerVisionAPI client = new ComputerVisionAPI(new ApiKeyServiceClientCredentials(key));
            client.AzureRegion = region;

            var result = await client.DescribeImageAsync(imageUrl);
            return result.Captions[0].Text;
        }

        public static async Task<string> GetFaceWithInImageAsync(string key, AzureRegions region, string imageUrl)
        {
            IComputerVisionAPI client = new ComputerVisionAPI(new ApiKeyServiceClientCredentials(key));
            client.AzureRegion = region;
            var requiredFeatures = new List<VisualFeatureTypes> { VisualFeatureTypes.Faces };

            ImageAnalysis imageAnalysis = await client.AnalyzeImageAsync(imageUrl, requiredFeatures);
            StringBuilder faceDetails = new StringBuilder();
            faceDetails.Append($"Found total **{imageAnalysis.Faces.Count}** faces in image.");

            foreach (var face in imageAnalysis.Faces)
            {
                faceDetails.Append($"{Environment.NewLine} - Gender: {face.Gender} | Age: {face.Age}");
            }
            return faceDetails.ToString();
        }

        public static async Task<string> GetTagsForImageAsync(string key, AzureRegions region, string imageUrl)
        {
            StringBuilder tagsDetails = new StringBuilder();
            IComputerVisionAPI client = new ComputerVisionAPI(new ApiKeyServiceClientCredentials(key));
            client.AzureRegion = region;
            var requiredFeatures = new List<VisualFeatureTypes> { VisualFeatureTypes.Tags };

            ImageAnalysis imageAnalysis = await client.AnalyzeImageAsync(imageUrl, requiredFeatures);
            foreach (var tag in imageAnalysis.Tags)
            {
                tagsDetails.AppendLine($"{tag.Name} ({tag.Confidence}), ");
            }
            return tagsDetails.ToString().TrimEnd(", ".ToCharArray());
        }

        public static async Task<string> GetImageDetailsAsync(string key, AzureRegions region, string imageUrl)
        {
            StringBuilder imageDetails = new StringBuilder();
            IComputerVisionAPI client = new ComputerVisionAPI(new ApiKeyServiceClientCredentials(key));
            client.AzureRegion = region;
            var requiredFeatures = new List<VisualFeatureTypes> {
                VisualFeatureTypes.Adult,
                VisualFeatureTypes.Categories,
                VisualFeatureTypes.Color,
                VisualFeatureTypes.ImageType,
            };

            ImageAnalysis imageAnalysis = await client.AnalyzeImageAsync(imageUrl, requiredFeatures);
            imageDetails.AppendLine($"Dominant Background Color: {imageAnalysis.Color.DominantColorBackground} <br />");
            imageDetails.AppendLine($"Dominant Forground Color: {imageAnalysis.Color.DominantColorForeground} <br />");
            imageDetails.AppendLine($"Is Black & White: {imageAnalysis.Color.IsBWImg} <br />");

            string isLineDraw = imageAnalysis.ImageType.LineDrawingType == 1 ? "Yes" : "No";
            imageDetails.AppendLine($"Is Line drawing: {isLineDraw} <br />");
            imageDetails.AppendLine($"Is Adult content: {imageAnalysis.Adult.IsAdultContent} <br />");

            List<string> categoryList = new List<string>();
            foreach (var category in imageAnalysis.Categories)
            {
                categoryList.Add(category.Name);
            }
            imageDetails.AppendLine($"Categories: {string.Join(", ", categoryList)}");

            return imageDetails.ToString();
        }

        public static async Task<string> GetTextFromImageAsync(string key, string endpoint, string imageUrl)
        {
            VisionServiceClient client = new VisionServiceClient(key, endpoint);
            var result = await client.RecognizeTextAsync(imageUrl);
            StringBuilder text = new StringBuilder();
            foreach (var line in result.Regions[0].Lines)
            {
                foreach (var word in line.Words)
                {
                    text.Append(word.Text + " ");
                }
                text.Append("<br />");
            }
            return text.ToString();
        }

        public static async Task<string> GetFaceDetailsAsync(string key, string endpoint, string imageUrl)
        {
            FaceServiceClient client = new FaceServiceClient(key, endpoint);
            List<FaceAttributeType> requiredFaceAttributes = new List<FaceAttributeType>
            {
                FaceAttributeType.Age,
                FaceAttributeType.FacialHair,
                FaceAttributeType.Gender,
                FaceAttributeType.Glasses,
                FaceAttributeType.Makeup,
                FaceAttributeType.Hair
            };

            var result = await client.DetectAsync(imageUrl, returnFaceAttributes: requiredFaceAttributes);
            var attributes = result[0].FaceAttributes;
            StringBuilder faceDetails = new StringBuilder();
            faceDetails.Append($"Age: {attributes.Age} <br />");
            faceDetails.Append($"Gender: {attributes.Gender} <br />");
            faceDetails.Append($"Eye Makeup: {attributes.Makeup.EyeMakeup} <br />");
            faceDetails.Append($"Lip Makeup: {attributes.Makeup.LipMakeup} <br />");
            faceDetails.Append($"Has Beard: {attributes.FacialHair.Beard} <br />");
            faceDetails.Append($"Has Moustache: {attributes.FacialHair.Moustache} <br />");
            faceDetails.Append($"Has Sideburns: {attributes.FacialHair.Sideburns} <br />");
            faceDetails.Append($"Glasses Type: {attributes.Glasses} <br />");

            return faceDetails.ToString();
        }

        public static async Task<string> CompareFaceAsync(string key, string endpoint, string imageUrl)
        {
            FaceServiceClient client = new FaceServiceClient(key, endpoint);
            var face1 = await client.DetectAsync("http://static.independent.co.uk/s3fs-public/thumbnails/image/2013/04/25/10/Robert-Downey-Jr-Iron-Man-3-1.jpg");
            var face2 = await client.DetectAsync(imageUrl);

            var result = await client.VerifyAsync(face1[0].FaceId, face2[0].FaceId);
            return $"Is Both Identical: {result.IsIdentical} <br />Confidence: {result.Confidence}";
        }

        public static async Task<string> GetEmotionsAsync(string key, string endpoint, string imageUrl)
        {
            FaceServiceClient client = new FaceServiceClient(key, endpoint);
            List<FaceAttributeType> requiredFaceAttributes = new List<FaceAttributeType>
            {
                FaceAttributeType.Emotion
            };

            var result = await client.DetectAsync(imageUrl, returnFaceAttributes: requiredFaceAttributes);
            var emotionScore = result[0].FaceAttributes.Emotion;
            StringBuilder emotionDetails = new StringBuilder();
            emotionDetails.Append($"Top Rank Emotion is: {emotionScore.ToRankedList().First().Key} <br />");
            emotionDetails.Append($"Anger: {emotionScore.Anger} <br />");
            emotionDetails.Append($"Contempt: {emotionScore.Contempt} <br />");
            emotionDetails.Append($"Disgust: {emotionScore.Disgust} <br />");
            emotionDetails.Append($"Fear: {emotionScore.Fear} <br />");
            emotionDetails.Append($"Happiness: {emotionScore.Happiness} <br />");
            emotionDetails.Append($"Netural: {emotionScore.Neutral} <br />");
            emotionDetails.Append($"Sadness: {emotionScore.Sadness} <br />");
            emotionDetails.Append($"Surprise: {emotionScore.Surprise} <br />");

            return emotionDetails.ToString();
        }
    }
}