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
            try
            {
                HttpContext.Response.Headers.Add("Access-Control-Allow-Origin", "https://salmon-sand-00f744b10.3.azurestaticapps.net");
                string keyOutScraper = "YXV0aDB8NjQ2ZDM1OWFhMjkzNTA1MDA1ZjljNjZhfDQ0NjE3M2U2Yjk";
                OutscraperHelper outscraper = new OutscraperHelper(keyOutScraper);
                //string url = "B01MSBZYQW";//"https://www.amazon.com/dp/1612680194";
                int limit = 10;
                bool async = false;

                string jsonString = await outscraper.GetAmazonReviews(asin, limit, async);
                //return JsonHelper.Object2Json(jsonString);
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
                    //content = "Generame un perfil detallado de un Buyer Persona en base a las reeñas que te di. Esto debe incluir:\r\n\r\nInformación demográfica: Edad, género, ubicación, nivel de educación, ocupación, nivel de ingresos, estado civil y detalles familiares.\r\n\r\nInformación psicográfica: Intereses, actividades, opiniones, valores, actitudes y estilo de vida.\r\n\r\nProblemas y desafíos que enfrenta esta persona: ¿Cuáles son los principales obstáculos externos e internos que enfrenta en su vida diaria y en su trabajo? ¿Qué problemas específicos tiene que resolver? ¿Qué miedos o preocupaciones internas le quitan el sueño por la noche?\r\n\r\nProceso de toma de decisiones: ¿Cómo toma decisiones esta persona, particularmente en lo que respecta a las compras? ¿Qué factores considera antes de realizar una compra? ¿Cuáles son sus fuentes de información y cómo busca y evalúa las opciones disponibles?\r\n\r\nNecesidades y deseos: ¿Qué es lo que esta persona realmente necesita y desea? ¿Cómo podrían estas necesidades y deseos estar relacionados con los productos o servicios que se le pueden ofrecer?\r\n\r\nFrustraciones: ¿Qué cosas ha probado y no han funcionado?"
                    content= "Genera un perfil detallado de un Buyer Persona en base a las reseñas que te di con un resultado máximo de 2500 caracteres, teniendo en cuenta los siguientes aspectos:\r\n\r\nInformación demográfica: Proporciona una edad específica, define un género, indica una ciudad o región específica donde vive, describe su nivel de educación, detalla su ocupación concreta, proporciona una cifra específica para el nivel de ingresos, establece su estado civil y describe su situación familiar.\r\n\r\nInformación psicográfica: Describe sus intereses específicos, las actividades que realiza regularmente, sus opiniones en temas relevantes, sus valores personales, su actitud hacia la vida y el estilo de vida que lleva.\r\n\r\nProblemas y desafíos: Identifica los desafíos específicos, tanto externos como internos, que esta persona enfrenta en su vida diaria y en su trabajo. Describe los problemas que necesita resolver y los miedos o preocupaciones internas que le quitan el sueño.\r\n\r\nProceso de toma de decisiones: Detalla cómo toma decisiones esta persona en términos de compras, las consideraciones clave que tiene en cuenta, las fuentes de información que utiliza, y cómo busca y evalúa las opciones disponibles.\r\n\r\nNecesidades y deseos: Define qué es lo que esta persona realmente necesita y desea en términos concretos, y cómo estas necesidades y deseos podrían estar relacionados con un producto o servicio específico que se le pueda ofrecer.\r\n\r\nFrustraciones: ¿Qué cosas ha probado y no le han funcionado?\r\n\r\nPor favor, sé lo más específico y detallado posible en cada punto, evitando respuestas genéricas o con variables."
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
