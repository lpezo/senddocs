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

                log.write("ENVIO  DE DOCUMENTOS A SUNAT...");

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
                var interno = GetXml(ref config, "SUNAT");
                string user = GetXml(ref interno, "USUARIO");
                string pwd = GetXml(ref interno, "CLAVESU");

                interno = GetXml(ref config, "CLASE");
                _portal = GetXml(ref interno, "PORTAL");
                string sOficina = GetXml(ref interno, "OFICINA");
                _oficina = Convert.ToInt32(sOficina);

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

                        DocPdf ThreeDirectory = new DocPdf();
                        string path = ThreeDirectory.crearDirectorio("FirmaXml", cadadoc.fechaemision);
                       // Console.WriteLine(cadadoc);
                        //var dirZip = Util.ObtenerDirectorio("FirmaXML", cadadoc.fechaemision);
                        var unico = string.Format("{0}-{1}-{2}.xml.zip", cadadoc.rucempresa, cadadoc.tipodocumento, cadadoc.serienumero);
                        var unicoSinEx= string.Format("{0}-{1}-{2}", cadadoc.rucempresa, cadadoc.tipodocumento, cadadoc.serienumero);
                        var archzip = Util.ObtenerArchivo("FirmaXml", cadadoc.fechaemision, unico);
                   


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
                            
                            string dir = Path.Combine(path,unicoSinEx+".txt");
                            using (StreamWriter me = new StreamWriter(dir, false, Encoding.UTF8)) { 
                                bool eserror = false;
                                var envioSoap = EnviarSoap(archzip, out eserror);
                                //Console.WriteLine(envioSoap);
                               // Console.ReadLine();
                                if (envioSoap == "")
                                {
                                   
                                    docEnviadoAsunatSinError(cadadoc, me);
                                    log.write("El comprobante ha sido registrado exitosamente a Sunat");
                                    me.WriteLine("El comprobante ha sido registrado exitosamente a Sunat");

                                }
                                
                                else if (envioSoap== "El comprobante fue registrado previamente con otros datos")
                                {
                                    string message = "El comprobante fue registrado previamente con otros datos";
                                    UpdateEstado(cadadoc
                                        );
                                    me.WriteLine(message);
                                    log.write(envioSoap);
                                }
                                else if (envioSoap.Contains("OLE") || envioSoap.Contains("Internal Error") 
                                    || envioSoap.Contains("Web Service") || envioSoap.Contains("no puede responder su solicitud")
                                    || envioSoap.Contains("Error al cargar el controlador de impresora")
                                    || envioSoap.Contains("appliacationresponse ni faultcode en CDR"))
                                {
                                    UpdateMensaje(envioSoap, cadadoc);
                                    me.WriteLine(envioSoap);
                                    log.write(envioSoap);
                                }
                                else
                                    InsertErrorMessage(envioSoap, cadadoc);

                                me.WriteLine(envioSoap);
                                log.write(envioSoap);
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

