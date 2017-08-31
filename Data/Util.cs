using System;
using System.Data.SqlClient;
using System.Drawing;
using static System.Net.Mime.MediaTypeNames;

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

        public static float ToNumber(string numero)
        {
            float ret = 0;
            float.TryParse(numero.Trim(), out ret);
            return ret;
        }

    }
}
