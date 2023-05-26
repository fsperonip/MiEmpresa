using Conexiones;
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
            try
            {
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
                //RequestTelemetryHelper.TrackException(ex, new() { 
                //    { "idProduct", idProduct } 
                //});
            }
            return "";
        }

    }
}
