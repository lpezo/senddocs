using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Data;
using System.IO;
using EnvioDocumentos;
using CreaReporte;

namespace EnvioDocumentos
{
    class rutaXml : Documento
    {
     public void documentoL(Documento doc, string dir, string serienumero, string tp, string ruc)
        {
        dir= @"E:\ProgramaC#\senddocs\FirmaXml\";
            var docLeido = Path.Combine(dir,ruc+"-"+tp+"-"+serienumero+".xml.zip");
            if (File.Exists(docLeido))
            {
                Console.WriteLine(docLeido);
                Console.ReadLine();

            }


        }   
        


     

    }
}
