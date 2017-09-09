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
         static string mee=null;

        static void Main(string[] args)
        {
             
            string dir = @"E:\ProgramaC#\senddocs\logs\";
            string logs = Path.Combine(dir, "logs_sendSunat.txt");
            string user = "", password = "", db = "", server = "";
            getvariablesconexion(ref user, ref password, ref db, ref server);
            SqlConnection connection = null;
            connection = GetConnection(user, password, db, server);
            string query = "select top 5 * from cpe_doc_cab where estadoRegistro = 'C' or (estadoregistro = 'C' and (encustodia = 0 or encustodia is null)) order by fechaestado;";
            //string query = "select * from cpe_doc_cab where tipodocumento='01' and serienumero='F001-00000553' and fechaemision='2017-08-03'";
            //'F001-00001264' and fechaemision = '2017-08-21

            //
            var resultselect = string.Format(query);

            //Console.WriteLine(resultselect);
            //Console.ReadLine();
            var command = new SqlCommand(resultselect, connection);

            var reader = command.ExecuteReader();

            var list = new List<Documento>();



            var listaErrores = new List<Documento>();
#if !DEBUG
            try
            {
#endif
  
                while (reader.Read())
                {
                    var document = new Documento(reader);
                    list.Add(document);
                    Console.WriteLine(document);
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
            using (StreamWriter sw = new StreamWriter(logs, false, Encoding.Default)

)
            {
                foreach (var doc in list)
                {

                    try
                    {

                        string insert = "insert into db_sunat (serienumero, fecha, idcp) VALUES (" + doc.serienumero + "," + doc.fechaemision + "," + doc.idcp + ")";
                        var insertt = string.Format(insert);
                        var commandInsert = new SqlCommand(insertt, connection);
                        commandInsert.ExecuteNonQuery();
                    }

                    catch (Exception ex)
                    {
                        if (ex.Message != null)
                        {
                            mee = ex.Message;
                        }

                    }


                    string upt = "update cpe_doc_cab set estadoregistro='S' where serienumero=" + doc.serienumero + "";
                    string uptError = "update cpe_doc_cab set estadoregistro='E' , mensajeerror=+" + mee + "+ where serienumero=" + doc.serienumero + "";
                    if (mee != null)
                    {
                         var update = string.Format(uptError);
                    }
                    else {
                        var  update = string.Format(upt);
                    }
                    // var comandoUpt = new SqlCommand(update, connection);
                    //comandoUpt.ExecuteNonQuery();
                    var listadetalle = GetDetalle(doc.idcp, connection);
                    progPdf.Visualiza(doc, listadetalle);
                    if (mee != "") { 
                        sw.WriteLine("Se proceso el siguiente documento a Sunat:\t" + doc.serienumero + "\t" +uptError);

                }
                    else
                    {
                        sw.WriteLine("Se proceso el siguiente documento a Sunat:\t" + doc.serienumero + "\t" +upt);

                    }
                    //Console.WriteLine(doc.serienumero);

                    //Console.ReadLine();

                } }

      

            connection.Close();
            
        }

        private static List<Detalle> GetDetalle(int idcpe, SqlConnection connection )
        {
            var lista = new List<Detalle>();

            string query = "select * from cpe_doc_det where idcpe= {0}";
            var resul = string.Format(query, idcpe);
            var command = new SqlCommand(resul, connection);
            var reader = command.ExecuteReader();
            try
            {

                while (reader.Read())
                {
                    var document = new Detalle(reader);
                    lista.Add(document);
                    Console.WriteLine(document);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally{

                reader.Close();

            }

           

            return lista;
        }

        private static SqlConnection GetConnection(string server, string db, string user, string password)
        {
            SqlConnection cn;
            string cx;
            //string cx = string.Format("UID={0};PWD={1};Server={2};Database={3};", user, password, server, db);

            if (!string.IsNullOrEmpty(user))
            {
                 cx = string.Format("UID={0};PWD={1};Server={2};Database={3};", user, password, server, db);

            }
            else
            {
                 cx = string.Format("INTEGRATED SECURITY=SSPI;Server={0};Database={1};", server, db);
            }
            //Console.WriteLine(cx);
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
