using Data;
using MigraDoc.DocumentObjectModel;
using MigraDoc.Rendering;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            string filename = "Factura.pdf";
            renderer.PdfDocument.Save(filename);
            // ...and start a viewer.
            Process.Start(filename);
        }
    }
}
