using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace EnvioDocumentos
{
    class crearReporte
    {

        public static string crearDirectorio(string directorio, string fecha)
        {

            string ano = fecha.Substring(0, 4);
            string mes = fecha.Substring(6, 8);
            string dia = fecha.Substring(10, 12);

            var temp = Path.Combine(directorio, ano);
            if (!File.Exists(temp))
                Directory.CreateDirectory(temp);
            temp = Path.Combine(directorio, mes);
            if (!File.Exists(temp))
                Directory.CreateDirectory(temp);
            temp = Path.Combine(directorio, dia);
            if (!File.Exists(temp))
                Directory.CreateDirectory(temp);



            return temp;

        }



    }
}
