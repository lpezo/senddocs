using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using System.Threading.Tasks;
using Data;
using CreaReporte;

namespace EnvioDocumentos
{
    class Program
    {
         static void Main(string[] args)
        {

            string   user = "", password = "", db = "", server = "";
            getvariablesconexion ( ref user,  ref password,  ref db,  ref server);
            SqlConnection connection = null;
            connection = GetConnection(user, password, db, server);
            string query = "select * from cpe_doc_cab where tipodocumento='01' and serienumero='F001-00001264' and fechaemision='2017-08-21'";
            //string query = "select * from cpe_doc_det where tipodocumento='01' and serienumero='F001-00000438' and codigoitem='00000026'";

            var resultselect = string.Format(query);

            Console.WriteLine(resultselect);
            //Console.ReadLine();
            var command = new SqlCommand(resultselect, connection);

            var reader = command.ExecuteReader();
#if !DEBUG
            try
            {
#endif
                var list = new List<Documento>();
                while (reader.Read())
                {
                    var document = new Documento(reader);
                    list.Add(document);
                    //Console.WriteLine(document);
                }
#if !DEBUG
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally{
#endif
                reader.Close();
#if !DEBUG
            }
#endif

            var progPdf = new DocPdf();

            foreach (var doc in list)
            {
                Console.WriteLine(doc);
                progPdf.Visualiza(doc);
            }

            Console.WriteLine("Presione ENTER para continuar...");
            Console.ReadLine();


        }

        private static SqlConnection GetConnection(string server, string db, string user, string password)
        {
            SqlConnection cn;
            string cx = string.Format("UID={0};PWD={1};Server={2};Database={3};", user, password, server, db);
            Console.WriteLine(cx);
            cn = new SqlConnection(cx);
            cn.Open();
            return cn;
        }




        private static string getvariablesconexion(ref string server, ref string db, ref string user, ref string password)
        {

            var sr = new StreamReader("config.xml", Encoding.Default);
            var config = sr.ReadToEnd();
            var interno = GetXml(ref config, "ODBC");
            user = GetXml(ref interno, "USER");
            password = GetXml(ref interno, "PWD");
            server = GetXml(ref interno, "SERVER");
            db = GetXml(ref interno, "DB");
            return "";

        }

        private static string GetXml(ref string texto, string tag)
        {
            //var tag = "ODBC";
            var tag1 = "<" + tag + ">";
            var tag2 = "</" + tag + ">";
            var pos = texto.IndexOf(tag1);
            if (pos >= 0)
            {
                var posini = pos + tag1.Length;
                var pos2 = texto.IndexOf(tag2);
                return texto.Substring(posini, pos2 - posini);
            }
            return "";
        }


    }



}
