using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Data.SqlClient;
using Data;
using CreaReporte;
using EnvioDocumentos;


namespace EnvioDocumentos
{
    
    
    
    
    class EnvioSunat : Base
    {

        
        public void Recorre()
        {
            using (log = new Log())
            {
                log.CrearLog("esend");

                log.write("ENVIO A SUNAT...");

                string query = "select  top 1 * from cpe_doc_cab where serienumero='F001-00001711' and tipodocumento='01'"; //estadoRegistro = 'L' or (estadoregistro = 'C' and (encustodia = 0 or encustodia is null)) order by fechaestado;";


                GetConecciones();

                var resultselect = string.Format(query);
                var command = new SqlCommand(resultselect, connection);
                var reader = command.ExecuteReader();
                var list = new List<Documento>();

                string config = null;
                using (var sr = new StreamReader("config.xml", Encoding.Default))
                {
                    config = sr.ReadToEnd();
                }
                    var interno = GetXml(ref config, "ODBC");
                    string user = GetXml(ref interno, "USUARIO");
                    string pwd = GetXml(ref interno, "CLAVESU");
                    Console.WriteLine("USUARIO: " + user);
                    Console.WriteLine("CLAVE: " + pwd);
              
#if !DEBUG
            try
            {
#endif

                while (reader.Read())
                {
                    var document = new Documento(reader);
                    list.Add(document);
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
                foreach (var cadadoc in list)
                {
                    try
                    {

                     
                        Console.WriteLine(cadadoc);
                        //var dirZip = Util.ObtenerDirectorio("FirmaXML", cadadoc.fechaemision);
                        var unico = string.Format("{0}-{1}-{2}.xml.zip", cadadoc.rucempresa, cadadoc.tipodocumento, cadadoc.serienumero);
                        var unicoSinEx= string.Format("{0}-{1}-{2}", cadadoc.rucempresa, cadadoc.tipodocumento, cadadoc.serienumero);
                        var archzip = Util.ObtenerArchivo("FirmaXML", cadadoc.fechaemision, unico);

                        if (!
                            string.IsNullOrEmpty(archzip))
                        {
                            var listadetalle = GetDetalle(cadadoc.idcp, connection);
                            progPdf.Visualiza(cadadoc, listadetalle);
                            Console.WriteLine("\t{0}", archzip);
                            //var filesoap = archzip+".soap";
                            /*
                            using (var sw = new StreamWriter(filesoap, false, Encoding.UTF8))
                            {
                                var soap = EnviarSoap(filesoap);
                                sw.WriteLine(soap);
                            }
                            */
                            string dir = Path.Combine("FirmaXml",unicoSinEx+".txt");
                            using (StreamWriter me = new StreamWriter(dir, false, Encoding.UTF8)) { 
                            bool eserror = false;
                                var envioSoap = EnviarSoap(archzip, out eserror);
                                if (eserror==false)
                                {
                                    docEnviadoAsunatSinError(cadadoc.serienumero, cadadoc.tipodocumento);
                                    me.WriteLine(envioSoap);
                                }
                                else

                                InsertErrorMessage(envioSoap, cadadoc.serienumero, cadadoc.tipodocumento);
                                me.WriteLine(envioSoap);
                            
                                

                            }
                        }
                        else
                        Console.WriteLine("No existe el XML");

                    }
                    catch (Exception ex)
                    {
                        log.write(ex.Message);
                    }

                    
                }



            } //using log



                //connection.Close();
            
        }


    }

    
}

