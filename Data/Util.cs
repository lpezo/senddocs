using System;
using System.Data.SqlClient;

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
