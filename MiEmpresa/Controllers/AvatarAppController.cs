using Conexiones;
using Herramientas;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MiEmpresa.Controllers
{
    [MiFiltroPreActionyPostAction]
    [ApiController]
    public class AvatarAppController : ControllerBase
    {

        // https://localhost:7168/Prueba/283989?key=Q284GCt%KXEZ!QJJ@Et6iDJ2mM0i9c
        [HttpGet("Prueba/{id}")]
        public async Task<string> Prueba(string id)
        {
            string apiKey = "YXV0aDB8NjQ2ZDM1OWFhMjkzNTA1MDA1ZjljNjZhfDQ0NjE3M2U2Yjk";
            OutscraperHelper outscraper = new OutscraperHelper(apiKey);

            try
            {
                string url = "https://www.amazon.com/dp/1612680194";
                int limit = 3;
                bool async = false;

                string response = await outscraper.GetAmazonReviews(url, limit, async);
                //Console.WriteLine(response);
            
                var messages = new List<dynamic>
                                {
                                    new {role = "system",
                                        content = "Responderé en calidad de asesor experto en marketing, brindando una visión especializada en la materia."},
                                    new {role = "user",
                                        content = "Hola, ¿A que te dedicas?" }
                                };
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

    }
}
