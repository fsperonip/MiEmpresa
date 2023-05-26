using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Herramientas
{
    public class RequestTelemetryHelper
    {
        private static TelemetryClient AI_CLIENT;
        private static string nombreMaquina = System.Environment.MachineName;


        public static void Inicializar(string appinsightsKey)
        {
            TelemetryConfiguration configuration = new();
            configuration.InstrumentationKey = appinsightsKey;
            AI_CLIENT = new TelemetryClient(configuration);
            AI_CLIENT.TrackTrace("Arrancamos!");
        }


        public class MonitorDeRequest
        {
            private RequestTelemetry request;
            private static string SUCCESS_CODE = "200";
            private static string FAILURE_CODE = "500";

            public MonitorDeRequest(string name)
            {
                request = new RequestTelemetry();
                request.Name = name;
                request.Timestamp = DateTime.Now;
            }

            public void DispatchRequest(bool success)
            {
                request.Duration = request.Timestamp - DateTime.Now;
                request.Success = success;
                request.ResponseCode = (success) ? SUCCESS_CODE : FAILURE_CODE;
                AI_CLIENT.TrackRequest(request);
                AI_CLIENT.Flush();
            }
        }



        public static void Tracear(string cMsg)
        {
            System.Diagnostics.Trace.TraceInformation(cMsg);
            AI_CLIENT.TrackTrace(cMsg);
        }




        public static void TrackEvent(string name, Dictionary<string, string>? properties = null)
        {
            AI_CLIENT.TrackEvent(name, properties, null);
        }
        public static void TrackException(Exception ex, Dictionary<string, string?>? properties = null)
        {
            try
            {
                if (properties == null)
                    properties = new();
                properties.Add("appService", Environment.GetEnvironmentVariable("WEBSITE_SITE_NAME"));
            }
            catch { }

            AI_CLIENT.TrackException(ex, properties, default);
            AI_CLIENT.Flush(); // Para que no se pierda ningún error
        }
        public static void TrackPageView(string name, double duracionEnMilisecs = 0)
        {
            PageViewTelemetry pagina = new PageViewTelemetry(name);
            pagina.Duration = TimeSpan.FromMilliseconds(duracionEnMilisecs);
            AI_CLIENT.TrackPageView(pagina);
        }
        public static void TrackTrace(string name)
        {
            // Si tenemos activado TRACEAR en 
            if (ConfigurationManager.AppSettings["GUARDAR_TRAZAS"] == "SI")
                AI_CLIENT.TrackTrace(name);
        }

        public static void Flush()
        {
            AI_CLIENT.Flush();
        }
    }
}
