using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herramientas
{
    public static class JsonHelper
    {
        public static string Serializar(object objeto)
        {
            string cJson = "Json no valido";
            try
            {
                cJson = Newtonsoft.Json.JsonConvert.SerializeObject(objeto);
            }
            catch (Exception ex)
            {
                RequestTelemetryHelper.TrackException(ex);
            }
            return cJson;
        }
        public static T? Deserializar<T>(string objetoEnFormaString)
        {
            try
            {
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(objetoEnFormaString);
            }
            catch (Exception ex)
            {
                RequestTelemetryHelper.TrackException(ex);
            }
            return default(T);
        }




        public static string Object2Json(object obj)
        {
            string json3 = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
            {
                Formatting = Formatting.Indented
            });
            return json3;
        }



        public static bool IsValidJSON(string s)
        {
            try
            {
                Newtonsoft.Json.Linq.JToken.Parse(s);
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
