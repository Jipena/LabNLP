using Azure;
using Azure.AI.Language.QuestionAnswering;
using Azure.AI.TextAnalytics;
using System;
using System.IO;
using Microsoft.Extensions.Configuration;
using System.Text;
using static System.Net.Mime.MediaTypeNames;

namespace LabNLP
{
    class Program
    {
        static void Main(string[] args)
        {
            // Language Service nycklar o endpoint
            Uri endpoint = new Uri("https://languageservicewesteur.cognitiveservices.azure.com/");
            AzureKeyCredential credential = new AzureKeyCredential("c7ab6bf805f040049df094dd5a8d6789");
            string projectName = "LearnFAQ";
            string deploymentName = "production";
            QuestionAnsweringClient client = new QuestionAnsweringClient(endpoint, credential);
            QuestionAnsweringProject project = new QuestionAnsweringProject(projectName, deploymentName);

            //Cog Service nycklar o endpoint
            Uri cogendpoint = new Uri("https://cogservicewesteur.cognitiveservices.azure.com/");
            AzureKeyCredential cogcredential = new AzureKeyCredential("d8c714c57f8b455ea2df3148224d691a");
            TextAnalyticsClient cogclient = new TextAnalyticsClient(cogendpoint, cogcredential);

            // Skapar question variabeln för att använda i while loopen
            var question = "";

            while (question.ToLower() != "quit")
            {
                // promta ut att användaren kan skriva frågor och ta emot frågorna i question variabeln
                Console.WriteLine("Ask me anything or write quit to exit.");
                question = Console.ReadLine();

                // Ta emot fråga och skicka till QnA
                Response<AnswersResult> response = client.GetAnswers(question, project);
                
                foreach (KnowledgeBaseAnswer answer in response.Value.Answers)
                {
                    DetectedLanguage detectedLanguage = cogclient.DetectLanguage(question);
                    DocumentSentiment sentimentAnalysis = cogclient.AnalyzeSentiment(question);
                    DetectedLanguage detectedLanguage1 = cogclient.DetectLanguage(answer.Answer);
                    DocumentSentiment sentimentAnalysis1 = cogclient.AnalyzeSentiment(answer.Answer);

                    Console.WriteLine($"Question:{question}");
                    Console.WriteLine($"Language:{detectedLanguage.Name}");
                    Console.WriteLine($"Sentiment:{sentimentAnalysis.Sentiment}");
                    Console.WriteLine($"Answer:{answer.Answer}");
                    Console.WriteLine($"Language:{detectedLanguage1.Name}");
                    Console.WriteLine($"Sentiment:{sentimentAnalysis1.Sentiment}");
                }
            } 
        }
    }
}