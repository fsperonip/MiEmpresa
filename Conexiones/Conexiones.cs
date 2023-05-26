using Herramientas;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace Conexiones
{
    public class Conexiones
    {
        public static string apiKey { get; internal set; } = "";
        public static void ConfigurarEntorno(string appInsightsResourceId)
        {
            GetSecretsFromAzure();
            
            ServicePointManager.DefaultConnectionLimit = 1000;  // 500
            ServicePointManager.Expect100Continue = false;
            ServicePointManager.UseNagleAlgorithm = false;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            //Para agregar appInsights
            //RequestTelemetryHelper.Inicializar(appInsightsResourceId);
        }
        public static void GetSecretsFromAzure()
        {
            //Acá se obtendrían las claves de keyVault
            apiKey = "Q284GCt%KXEZ!QJJ@Et6iDJ2mM0i9c";
        }
    }
}
