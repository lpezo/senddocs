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


namespace EnvioDocumentos
{
    class Base
    {

        string user = "", password = "", db = "", server = "";
        protected SqlConnection connection = null;

        protected Log log = null;


        public static string RutaXml()
        {
            string dir = @"E:\ProgramaC#\senddocs\FirmaXml";

            DirectoryInfo ficheros = new DirectoryInfo(dir);

            foreach (var doc in ficheros.GetFiles())
            {

                Console.WriteLine(doc.Name);
                Console.ReadLine();

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
            Console.WriteLine();
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
                    Console.WriteLine(document);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {

                reader.Close();

            }



            return lista;
        }

        //protected string EnviarSoap(string fsoap, int numserver)
        //{
        //    var webAddr = "https://www.sunat.gob.pe/ol-ti-itcpfegem/billService";
        //    //var _url = "http://xxxxxxxxx/Service1.asmx";
        //    //var _action = "http://xxxxxxxx/Service1.asmx?op=HelloWorld";

        //    XmlDocument soapEnvelopeXml = CreateSoapEnvelope();
        //    var webRequest = (HttpWebRequest)WebRequest.Create(webAddr);
        //    InsertSoapEnvelopeIntoWebRequest(soapEnvelopeXml, webRequest);

        //    // begin async call to web request.
        //    IAsyncResult asyncResult = webRequest.BeginGetResponse(null, null);

        //    // suspend this thread until call is complete. You might want to
        //    // do something usefull here like update your UI.
        //    asyncResult.AsyncWaitHandle.WaitOne();

        //    // get the response from the completed web request.
        //    string soapResult;
        //    using (WebResponse webResponse = webRequest.EndGetResponse(asyncResult))
        //    {
        //        using (StreamReader rd = new StreamReader(webResponse.GetResponseStream()))
        //        {
        //            soapResult = rd.ReadToEnd();
        //        }

        //        Console.Write(soapResult);
        //        Console.ReadLine();
        //        return soapResult;
        //    }

        //}

        //private static HttpWebRequest CreateWebRequest(string url, string action)
        //{
        //    HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
        //    webRequest.Headers.Add("SOAPAction", action);
        //    webRequest.ContentType = "text/xml;charset=\"utf-8\"";
        //    webRequest.Accept = "text/xml";
        //    webRequest.Method = "POST";
        //    return webRequest;
        //}

        //private static XmlDocument CreateSoapEnvelope(string usuario, string clave, string filezip, string tipo)
        //{
        //    filezip = "20114208346-01-F001-00001711.xml.zip.soap";
        //    var bat64 = Util.GetBase64(filezip);
        //    string nombre = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filezip));
        //    XmlDocument soapEnvelopeDocument = new XmlDocument();
        //    string strsoap = @"<?xml version=""1.0"" encoding=""utf-8""?>
        //    <soapenv:Envelope xmlns:soapenv=""http://schemas.xmlsoap.org/soap/envelope/"" xmlns:ser=""http://service.sunat.gob.pe/"" xmlns:wsse =""http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd/"">
        //      <soapenv:Header>
        //        <wsse:Security>
        //          <wsse:UsernameToken>
        //           <wsse:Username>20600337832FACTURA2</wsse:Username>
        //            <wsse:Password>krx32aFF</wsse:Password>
        //          </wsse:UsernameToken></wsse:Security>
        //      </soapenv:Header>
        //      <soapenv:Body>
        //        <ser:{2}>
        //            <fileName>20114208346-01-F001-00001711.xml.zip.soap</fileName>
        //            <contentFile>{4}</contentFile>
        //          </ser:{2}>
        //        </soapenv:Body></soapenv:Envelope>";

        //    soapEnvelopeDocument.LoadXml(strsoap);
        //    return string.Format(srtsoa,  usuario, clave, tipo, nombre + ".zip", bat64);
        //}

        //private static void InsertSoapEnvelopeIntoWebRequest(XmlDocument soapEnvelopeXml, HttpWebRequest webRequest)
        //{
        //    using (Stream stream = webRequest.GetRequestStream())
        //    {
        //        soapEnvelopeXml.Save(stream);
        //    }
        //}

        //protected string EnviarSoap(string fsoap, int numserver)
        // {
        //     //Prueba 
        //     //var webAddr = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";
        //     //Produccion

        //     var webAddr = "https://www.sunat.gob.pe/ol-ti-itcpfegem/billService";

        //     string ssoap = "";
        //     using (var sr = new StreamReader(fsoap, Encoding.Default))
        //     {
        //         ssoap = sr.ReadToEnd();
        //     }

        //     var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
        //    //
        //     httpWebRequest.AllowWriteStreamBuffering = false;
        //     httpWebRequest.ContentType = "text/xml;charset=UTF-8";
        //     httpWebRequest.Accept = "text/xml";
        //     httpWebRequest.Method = "POST";
        //     httpWebRequest.Headers.Add("SOAPAction", "sendBill");
        //     log.write("\t->" + webAddr);

        //     using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
        //     {
        //         streamWriter.Write(ssoap);
        //         streamWriter.Flush();
        //         streamWriter.Close();
        //     }

        //     var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
        //     using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
        //     {
        //         var result = streamReader.ReadToEnd();
        //         if (!string.IsNullOrEmpty(result))
        //             log.write(result);
        //         return result;
        //     }
        // }
        protected string EnviarSoap(string fzip, out bool esError)
        {

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

        protected void docEnviadoAsunatSinError(string serienumero, string td)
        {
            {
                string sql = "UPDATE cpe_doc_cab set mensajeerror=' ', ensunat=1, estadoregistro='L' where serienumero='" + serienumero + "' and tipodocumento='" + td + "'";
                string query = string.Format(sql, serienumero, td);
                SqlCommand command = new SqlCommand(query, connection);
                command.ExecuteNonQuery();
            }
        }
        protected void InsertErrorMessage(string message, string serienumero, string td)
        {
            string messageE = LimpiarParaJson(message);
            string sql = "UPDATE cpe_doc_cab set mensajeerror='" + messageE + "' where serienumero='" + serienumero + "' and tipodocumento='" +td+"'";
            Console.WriteLine(sql);
            string query = string.Format( sql, message, serienumero, td);
            SqlCommand command = new SqlCommand(query,connection);
             command.ExecuteNonQuery();
     
        }

        private string LimpiarParaJson(string pValor)
        {

            var cars = "'\\{}\"";
            var sb = new StringBuilder(pValor);
            foreach (var car in cars)
                sb.Replace(car, ' ');
            return sb.ToString();
        }


        //protected string GetSoap(string usuario, string clave, string filezip, string tipo)
        //{

        //    var bat64 = Util.GetBase64(filezip);
        //    string nombre = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filezip));


        //    var strsoap = "<?xml version=\"1.0\" encoding=\"utf-8\"?>" +
        //    "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ser=\"http://service.sunat.gob.pe/\" xmlns:wsse =\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd/\">" +
        //    "  <soapenv:Header>" +
        //    "    <wsse:Security>" +
        //    "      <wsse:UsernameToken>" +
        //    "       <wsse:Username>{0}</wsse:Username>" +
        //    "        <wsse:Password>{1}</wsse:Password>" +
        //    "      </wsse:UsernameToken></wsse:Security>" +
        //    "  </soapenv:Header>" +
        //    "  <soapenv:Body>" +
        //    "    <ser:{2}>" +
        //    "        <fileName>{3}</fileName>" +
        //    "        <contentFile>{4}</contentFile>" +
        //    "      </ser:{2}>" +
        //    "    </soapenv:Body></soapenv:Envelope>";





        //    return string.Format(strsoap, usuario, clave, tipo, nombre+".zip" , bat64 );

        //}


    }


}
