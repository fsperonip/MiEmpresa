using Conexiones;
using Herramientas;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Data;
using System.Text.Json.Nodes;

namespace MiEmpresa.Controllers
{
    [MiFiltroPreActionyPostAction]
    [ApiController]
    public class AvatarAppController : ControllerBase
    {

        // https://localhost:7168/AvatarApp/B01MSBZYQW?key=Q284GCt%KXEZ!QJJ@Et6iDJ2mM0i9c
        [HttpGet("AvatarApp/{asin}")]
        public async Task<string> AvatarApp(string asin)
        {
            string keyOutScraper = "YXV0aDB8NjQ2ZDM1OWFhMjkzNTA1MDA1ZjljNjZhfDQ0NjE3M2U2Yjk";
            OutscraperHelper outscraper = new OutscraperHelper(keyOutScraper);

            try
            {
                //string url = "B01MSBZYQW";//"https://www.amazon.com/dp/1612680194";
                int limit = 15;
                bool async = false;

                string jsonString = await outscraper.GetAmazonReviews(asin, limit, async);
                JObject jsonObject = JObject.Parse(jsonString);
                List<string> bodyList = new List<string>();

                foreach (JObject item in jsonObject["data"][0])
                {
                    string body = (string)item["body"];
                    bodyList.Add(body);
                }

                //Console.WriteLine(response);

                var messages = new List<dynamic>
                                {
                                    new {role = "system",
                                        content = "Responderé en calidad de asesor experto en marketing, brindando una visión especializada en la materia."}
                                };
                foreach(string body in bodyList)
                {
                    string truncatedBody = TruncateText(body, 300);
                    messages.Add(new
                    {
                        role = "user",
                        content = "Reseña: " + truncatedBody
                    });
                }
                messages.Add(new
                {
                    role = "user",
                    //content = "Generar Avatar de Comprador\n\nInstrucciones: Por favor, genera un avatar de comprador basado en las reseñas que te di\n\nUtiliza la información de las reseñas para inferir las características y preferencias del comprador y describe su avatar teniendo en cuenta los productos mencionados. Ten en cuenta que es posible que debas hacer suposiciones basadas en la información limitada de las reseñas.\n\nGenera un avatar de comprador que se ajuste a estas suposiciones y describe sus características y preferencias principales.\n\nDetallame cantidad de reseñas utilizadas para realizar el avatar"
                    content = "Generar Avatar de Comprador\n\nInstrucciones: Por favor, genera un avatar de comprador basado en las reseñas que te di\n\nUtiliza la información de las reseñas para inferir las características y preferencias del comprador y describe su avatar teniendo en cuenta los productos mencionados. Ten en cuenta que es posible que debas hacer suposiciones basadas en la información limitada de las reseñas.\n\nGenera un avatar de comprador que se ajuste a estas suposiciones y describe sus características y preferencias principales."
                });
                var mensaje = await Herramientas.ChatGptHelper.SendMessages(messages);
                return mensaje.ToString();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                //RequestTelemetryHelper.TrackException(ex, new() { 
                //    { "idProduct", idProduct } 
                //});
            }
            return "";
        }

        // Método para truncar el texto a un máximo de caracteres
        public static string TruncateText(string text, int maxLength)
        {
            if (string.IsNullOrEmpty(text))
                return string.Empty;

            if (text.Length <= maxLength)
                return text;

            return text.Substring(0, maxLength);
        }

    }
}
