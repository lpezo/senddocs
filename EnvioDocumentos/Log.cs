using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace EnvioDocumentos
{
    class Log : IDisposable
    {
        private StreamWriter log = null;

        public void CrearLog(string nombre)
        {
            var sarch = "Logs\\Log_" + DateTime.Now.ToString("yyyyMMdd_") + nombre + ".log";
            if (!Directory.Exists("Logs"))
            {
                Directory.CreateDirectory("Logs");
            }
            log = new StreamWriter(sarch, true, Encoding.Default);
        }

        public void write(string texto, string formato = null)
        {
            if (string.IsNullOrEmpty(formato)) {
                Console.WriteLine(texto);
                log.WriteLine(texto);
            }
            else
            {
                Console.WriteLine(texto, formato);
                log.WriteLine(string.Format(texto, formato));
            }
        }

        public void Cerrar()
        {
            log.Close();
            log = null;
        }

        public void Dispose()
        {
            if (log != null)
                log.Close();
        }
    }
}
