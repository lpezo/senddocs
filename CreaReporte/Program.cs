using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CreaReporte
{
    class Program
    {
        static void Main(string[] args)
        {
            // Create a MigraDoc document
            Document document = Documents.CreateDocument();
            //string ddl = MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToString(document);
            //MigraDoc.DocumentObjectModel.IO.DdlWriter.WriteToFile(document, "MigraDoc.mdddl");
            PdfDocumentRenderer renderer = new PdfDocumentRenderer(true, PdfSharp.Pdf.PdfFontEmbedding.Always);
            renderer.Document = document;
            renderer.RenderDocument();
            // Save the document...
            string filename = "Factura.pdf";
            renderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }

    }
}
