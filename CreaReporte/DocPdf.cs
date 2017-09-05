using Data;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;


namespace CreaReporte
{
    public class DocPdf
    {
        public void Visualiza(Documento doc, List<Detalle> listadetalle)
        {
            // Create a MigraDoc document
            Document document = Documents.CreateDocument(doc, listadetalle);
            //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            //MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = document;
            renderer.RenderDocument();
            // Save the document...
            string filename = doc.numerodocumentocliente + "-" + doc.tipodocumento + "-" + doc.serienumero + ".pdf";
            renderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);



            //public static string crearDirectorio(string directorio, string fecha)
            //{

            //    string ano = fecha.Substring(0, 4);
            //    string mes = fecha.Substring(6, 8);
            //    string dia = fecha.Substring(10, 12);

            //    var temp = Path.Combine(directorio, ano);
            //    if (!File.Exists(temp))
            //        Directory.CreateDirectory(temp);
            //    temp = Path.Combine(directorio, mes);
            //    if (!File.Exists(temp))
            //        Directory.CreateDirectory(temp);
            //    temp = Path.Combine(directorio, dia);
            //    if (!File.Exists(temp))
            //        Directory.CreateDirectory(temp);



            //    return temp;

            //}
        }
    }
}
