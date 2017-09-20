using Data;
using EnvioDocumentos.SunatTest;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;
using CreaReporte;
using Newtonsoft.Json;

namespace EnvioDocumentos
{
    class Base
    {

        string user = "", password = "", db = "", server = "";
        protected SqlConnection connection = null;

        protected Log log = null;
        protected string _portal = null;
        protected int _oficina ;

        public static string RutaXml()
        {
            string dir = @"E:\ProgramaC#\senddocs\FirmaXml";

            DirectoryInfo ficheros = new DirectoryInfo(dir);

            foreach (var doc in ficheros.GetFiles())
            {

                //Console.WriteLine(doc.Name);
                //Console.ReadLine();

            }
            return dir;


        }



        public void GetConecciones()
        {
            getvariablesconexion(ref user, ref password, ref db, ref server);
            connection = GetConnection(user, password, db, server);
        }


        private string getvariablesconexion(ref string server, ref string db, ref string user, ref string password)
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

        public string getSunat(ref string pwd, ref string user)
        {

            var sr = new StreamReader("config.xml", Encoding.Default);
            var config = sr.ReadToEnd();
            var interno = GetXml(ref config, "ODBC");
            user = GetXml(ref interno, "USUARIO");
            pwd = GetXml(ref interno, "CLAVESU");
            //Console.WriteLine();
            return "";

        }


        private SqlConnection GetConnection(string server, string db, string user, string password)
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

        public static string GetXml(ref string texto, string tag)
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

        protected List<Detalle> GetDetalle(int idcpe, SqlConnection connection)
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
                    //Console.WriteLine(document);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                log.write(ex.Message);
            }
            finally
            {

                reader.Close();

            }



            return lista;
        }

       
        protected string EnviarSoap(string fzip, out bool esError
            )
        {


            DocPdf threeDir = new DocPdf();
            //threeDir.crearDirectorio(fzip, fechaemision );
            var billService = new billServiceClient("BillServicePort.11");
            esError = false;
            var resp = "";
            try
            {
                var bytes = billService.sendBill(Path.GetFileName(fzip).Replace(".xml", ""), File.ReadAllBytes(fzip), "");
                //sCdrZip = System.Text.Encoding.Default.GetString(bytes);
                var rfzip = Path.Combine(Path.GetDirectoryName(fzip), "R-" + Path.GetFileName(fzip));
                File.WriteAllBytes(rfzip, bytes);   
            }
            catch (Exception ex)
            {
                resp = "Error: " + ex.Message;
                esError = true;
            }
            return resp;
        }

        protected void docEnviadoAsunatSinError(Documento doc, StreamWriter me )
        {
            log.write("Envindo a Sunat el documento "+doc.tipodocumento+"-"+doc.serienumero+" ....");
            string sql = "UPDATE cpe_doc_cab set mensajeerror=' ', ensunat=1, estadoregistro='S' where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string sqlCLI = "UPDATE doc_cab_cli set mensajeerror=' ',  estadoregistro='S' where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string query = string.Format(sql, doc.serienumero, doc.tipodocumento);
            string queryCLI = string.Format(sqlCLI, doc.serienumero, doc.tipodocumento);
            SqlCommand command = new SqlCommand(query, connection);
            SqlCommand commandCLI = new SqlCommand(queryCLI, connection);
            log.write("Enviando a CPE");
            command.ExecuteNonQuery();
            log.write(query);
            log.write("Enviando a CLI");
            commandCLI.ExecuteNonQuery();
            log.write(queryCLI);
            log.write("Enviando a Portal...");
            EnviarDoc(doc);
            me.WriteLine("Documento enviado sin errores");
        }



        protected void UpdateEstado( Documento doc)
        {
            log.write("El comprobando ya fue registrado con otros datos");
            string sql = "UPDATE cpe_doc_cab set mensajeerror='El comprobante fue registrado previamente con otros datos', ensunat='1', estadoregistro='S' where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string sqlCLI = "UPDATE doc_cab_cli set mensajeerror='El comprobante fue registrado previamente con otros datos',  estadoregistro='S' where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string query = string.Format(sql, doc.serienumero, doc.tipodocumento);
            string queryCLI = string.Format(sqlCLI, doc.serienumero, doc.tipodocumento);
            SqlCommand command = new SqlCommand(query, connection);
            SqlCommand commandCLI = new SqlCommand(queryCLI, connection);
            log.write("Modificando estado de registro en  CPE");
            command.ExecuteNonQuery();
            log.write(query);
            log.write("Modificando estado de registro en  CLI");
            commandCLI.ExecuteNonQuery();
            log.write(queryCLI);
            log.write("Modificando estado de registro en  Portal...");
            EnviarDoc(doc);

        }

        protected void UpdateMensaje(string message, Documento doc)
        {
            log.write("Error al enviar a Sunat:");
            string messageE = LimpiarParaJson(message);
            log.write(messageE);
            string sql = "UPDATE cpe_doc_cab set mensajeerror='"+ messageE + "' where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string sqlCLI = "UPDATE doc_cab_cli set mensajeerror='" + messageE + "' where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string query = string.Format(sql, message, doc.serienumero, doc.tipodocumento);
            string queryCLI = string.Format(sqlCLI, doc.serienumero, doc.tipodocumento, message);
            SqlCommand command = new SqlCommand(query, connection);
            SqlCommand commandCLI = new SqlCommand(queryCLI, connection);
            log.write("Modificando Mensaje de Error a CPE");
            command.ExecuteNonQuery();
            log.write(query);
            log.write("Modificando Mensaje de Error CLI");
            commandCLI.ExecuteNonQuery();
            log.write(queryCLI);
            log.write("Modificando Mensaje de Error a Portal...");
            EnviarDoc(doc);
        }
        protected void InsertErrorMessage(string message, Documento doc)
        {
            log.write("Error al enviar a Sunat:");
            string messageE = LimpiarParaJson(message);
            log.write(messageE);
            string sql = "UPDATE cpe_doc_cab set mensajeerror='" + messageE + "' , estadoregistro='E', ensunat='0'  where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string sqlCLI = "UPDATE doc_cab_cli set mensajeerror='" + messageE + "' , estadoregistro='E'  where serienumero='" + doc.serienumero + "' and tipodocumento='" + doc.tipodocumento + "'";
            string query = string.Format( sql, message, doc.serienumero, doc.tipodocumento);
            string queryCLI = string.Format(sqlCLI, message, doc.serienumero, doc.tipodocumento);
            SqlCommand command = new SqlCommand(query,connection);
            SqlCommand commandCLI = new SqlCommand(queryCLI, connection);
            log.write("Enviando Error a CPE ");
            command.ExecuteNonQuery();
            log.write(query);
            log.write("Enviando Error a CLI");
            commandCLI.ExecuteNonQuery();
            log.write(queryCLI);
            log.write("Enviando Error a Portal...");
            EnviarDoc(doc);
        }

        protected string EnviarDoc( Documento doc)
        {
            

                log.write("Insertando documento en el portal....");
                var webAddr = "http://" + _portal + "/ws/" + "oficina_fecha.php";
                log.write(webAddr);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "PUT";
                log.write("\t->" + webAddr);

                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    var obj = ClOficinaFecha.FromDoc(doc, _oficina);
                    var serializerSettings = new JsonSerializerSettings();
                    string json = JsonConvert.SerializeObject(obj, serializerSettings);
                    log.write(json);
                    streamWriter.Write(json);
                    streamWriter.Flush();
                    streamWriter.Close();


                }

                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                
            {
                    var result = streamReader.ReadToEnd();
                    if (!string.IsNullOrEmpty(result))
                        log.write(result);
                    return result;
                
            }

            

        }

        private string LimpiarParaJson(string pValor)
        {

            var cars = "'\\{}\"";
            var sb = new StringBuilder(pValor);
            foreach (var car in cars)
                sb.Replace(car, ' ');
            return sb.ToString();
        }

    }


}
