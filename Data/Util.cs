using System;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Text;

namespace Data
{
    public class Util
    {
        public static string GetString(SqlDataReader reader, string nombre)
        {
            int pos = reader.GetOrdinal(nombre);
            if (pos >= 0)
            {
                if (reader.IsDBNull(pos))
                    return "";
                else
                    return reader.GetString(pos);
            }
            else
                throw new Exception("No se encontro el campo " + nombre);
        }




        public static int GetInt(SqlDataReader reader, string nombre)
        {
            int pos = reader.GetOrdinal(nombre);
            if (pos >= 0)
            {
                if (reader.IsDBNull(pos))
                    return 0;
                else
                    return reader.GetInt32(pos);
            }
            else
                throw new Exception("No se encontro el campo " + nombre);
        }



        public static byte[] ImageToByte(System.Drawing.Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }


        public static byte GetByte(SqlDataReader reader, string nombre)
        {
            int pos = reader.GetOrdinal(nombre);
            if (pos >= 0)
            {
                if (reader.IsDBNull(pos))
                    return 0;
                else
                    return reader.GetByte(pos);
            }
            else
                throw new Exception("No se encontro el campo " + nombre);
        }

        public static string ObtenerDirectorio(string ruta, string fecha)
        {
            var dest = Path.Combine(ruta, fecha.Replace('-', '\\'));
            if (Directory.Exists(dest))
                return dest;

            var aa = fecha.Substring(0, 4);
            var mm = fecha.Substring(5, 2);
            var dd = fecha.Substring(8, 2);

            ruta = Path.Combine(ruta, aa);
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            ruta = Path.Combine(ruta, mm);
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            ruta = Path.Combine(ruta, dd);
            if (!Directory.Exists(ruta))
                Directory.CreateDirectory(ruta);

            return ruta;

        }

        public static string GetBase64(string archzip)
        {
            byte[] bat64 = File.ReadAllBytes(archzip);
            string archBat64 = Convert.ToBase64String(bat64);

            return archBat64;
        }

        public static string ObtenerArchivo(string ruta, string fecha, string unico)
        {
            var dest = ObtenerDirectorio(ruta, fecha);
            var arch = Path.Combine(dest, unico);
            if (File.Exists(arch))
                return arch;
            arch = Path.Combine(ruta, unico);
            if (File.Exists(arch))
                return arch;
            return "";
        }


        public static float ToNumber(string numero)
        {
            float ret = 0;
            float.TryParse(numero.Trim(), out ret);
            return ret;
        }



        public static string LimpiarParaJson(string pValor)
        {
            while (pValor.Contains("'") )
            {
                pValor = pValor.Replace("'", " ");
            }
            return pValor;
        }
        
    }
}
