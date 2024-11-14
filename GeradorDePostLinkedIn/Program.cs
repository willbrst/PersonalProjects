using System;
using System.ClientModel;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Azure;
using Azure.AI.OpenAI;
using OpenAI;
using OpenAI.Chat;

namespace GeradorDePostLinkedIn
{
    class Program
    {
        private const string apiKey = ""; 
        private const string endpoint = "";
        private const string location = "eastus";
        private static OpenAIClient _openAiClient;

        static async Task Main(string[] args)
        {
            string url = "https://www.aidrop.news/p/agi-prazo-de-entrega-economia-chips?_bhlid=62a388fcba79f273fac5edb76354d3152baceda9&last_resource_guid=Post%3A8982e5db-c241-4315-9680-9880bb148f5d&utm_campaign=qual-o-prazo-de-entrega-da-agi&utm_medium=newsletter&utm_source=www.aidrop.news"; // Substitua pela URL desejada
            string textoExtraido = await ExtrairTextoDaUrl(url);

            string textoLimpo = LimparTexto(textoExtraido);

            string postFinal = await GerarPostAzureOpenAI(textoLimpo);
            Console.WriteLine("Post para LinkedIn:");
            Console.WriteLine(postFinal);
        }

        private static string LimparTexto(string texto)
        {
            texto = texto.Replace("\n", " ");
            texto = Regex.Replace(texto, "<.*?>", string.Empty);
            texto = Regex.Replace(texto, @"\s+", " ").Trim();

            return texto;
        }

        private static async Task<string> ExtrairTextoDaUrl(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                try
                {
                    string conteudo = await client.GetStringAsync(url);
                    return conteudo;
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Erro ao acessar a URL: {ex.Message}");
                    return string.Empty;
                }
            }
        }

        private static async Task<string> GerarPostAzureOpenAI(string texto)
        {
            var options = new AzureOpenAIClientOptions
            {
                NetworkTimeout = new TimeSpan(0, 5, 0),
            };
            
            _openAiClient = new AzureOpenAIClient(
                new Uri(endpoint),
                new ApiKeyCredential(apiKey),
                options); ;

            ChatCompletionOptions chatCompletionsOptions = new ChatCompletionOptions
            {
                Temperature = 0.0f,
                MaxOutputTokenCount = 4096
            };

            string prompt = "Considerando as notícias do texto abaixo selecione as 3 que mais atraiam a curiosidade das pessoas em um post no linkedin, estruture-as como um post do linkedin: " + texto;
            var chatMessages = new List<ChatMessage>
            {
                new SystemChatMessage("Você é um assistente que gera posts atrativos para LinkedIn."),
                new UserChatMessage(prompt)
            };

            try
            {
                var chat = _openAiClient.GetChatClient("gpt40");
                var gptResponse = await chat.CompleteChatAsync(chatMessages, chatCompletionsOptions);

                string textResponse = gptResponse.Value.Content[0].Text;
                return textResponse;
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Erro ao gerar post: {ex.Message}");
                return string.Empty;
            }
        }
    }
}
