using Data;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

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


        protected string EnviarSoap(string fsoap, int numserver)
        {
            //Prueba 
            //var webAddr = "https://e-beta.sunat.gob.pe/ol-ti-itcpfegem-beta/billService";
            //Produccion

            //var webAddr = "https://www.sunat.gob.pe/ol-ti-itcpfegem/billService";
            var webAddr = "https://www.sunat.gob.pe/ol-it-wsconscpegem/billConsultService?wsdl";

            string ssoap = "";
            using (var sr = new StreamReader(fsoap, Encoding.Default))
            {
                ssoap = sr.ReadToEnd();
            }

            var httpWebRequest = (HttpWebRequest)WebRequest.Create(webAddr);
            httpWebRequest.AllowWriteStreamBuffering = false;
            httpWebRequest.ContentType = "text/xml;charset=UTF-8";
            httpWebRequest.Accept = "text/xml";
            httpWebRequest.ContentLength = ssoap.Length;
            httpWebRequest.Credentials = CredentialCache.DefaultCredentials;
            httpWebRequest.Method = "POST";
            httpWebRequest.Headers.Add("SOAPAction", "sendBill");

            log.write("\t->" + webAddr);

            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                streamWriter.Write(ssoap);
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



        protected string GetSoap(string usuario, string clave, string filezip, string tipo)
        {

            var bat64 = Util.GetBase64(filezip);
            string nombre = Path.GetFileNameWithoutExtension(Path.GetFileNameWithoutExtension(filezip));


            var strsoap = "<soapenv:Envelope xmlns:soapenv=\"http://schemas.xmlsoap.org/soap/envelope/\" xmlns:ser=\"http://service.sunat.gob.pe/\"" +
            " xmlns:wsse=\"http://docs.oasis-open.org/wss/2004/01/oasis-200401-wss-wssecurity-secext-1.0.xsd/\">" +
            "  <soapenv:Header>" +
            "    <wsse:Security>" +
            "      <wsse:UsernameToken>" +
            "       <wsse:Username>{0}</wsse:Username>" +
            "        <wsse:Password>{1}</wsse:Password>" +
            "      </wsse:UsernameToken></wsse:Security>" +
            "  </soapenv:Header>" +
            "  <soapenv:Body>" +
            "    <ser:{2}>" +
            "        <fileName>{3}</fileName>" +
            "        <contentFile>{4}</contentFile>" +
            "      </ser:{2}>" +
            "    </soapenv:Body></soapenv:Envelope>";
           




            return string.Format(strsoap, usuario, clave, tipo, nombre+".zip" , bat64 );
           
        }


    }


}
