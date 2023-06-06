using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herramientas
{
    public static class ChatGptHelper
    {
        const string key = "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";
        const string url = "https://api.openai.com/v1/chat/completions";

        public static async Task<dynamic> SendMessages(List<dynamic> messages)
        {
            var request = new
            {
                messages,
                model = "gpt-3.5-turbo",
                max_tokens = 2000,
                temperature = 0.5
            };

            // Send the request and capture the response
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {key}");
            var requestJson = JsonConvert.SerializeObject(request);
            var requestContent = new StringContent(requestJson, System.Text.Encoding.UTF8, "application/json");
            var httpResponseMessage = await httpClient.PostAsync(url, requestContent);
            var jsonString = await httpResponseMessage.Content.ReadAsStringAsync();
            var responseObject = JsonConvert.DeserializeAnonymousType(jsonString, new
            {
                choices = new[] { new { message = new { role = string.Empty, content = string.Empty } } },
                error = new { message = string.Empty }
            });


            ////No cotrolo los errores, si da error que se capture fuera!
            //if (!string.IsNullOrEmpty(responseObject?.error?.message))  // Check for errors
            //{
            //    return responseObject?.error.message;
            //}
            //else  // Add the message object to the message collection
            //{
            var messageObject = responseObject?.choices[0].message;

                return messageObject.content;
            //}


        }
    }
}
