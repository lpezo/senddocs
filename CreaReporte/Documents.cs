using Data;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CreaReporte
{


    class Documents
    {
      

        public static Document CreateDocument(Documento doc, List<Detalle> listadetalle)
        {
            // Create a new MigraDoc document
            Document document = new Document();
            document.Info.Title = "Documento Electrónico";
            document.Info.Subject = "Documento Electrónico en Migradoc";
            document.Info.Author = "Luis Pezo";
            DefineStyles(document);
            Cabecera(document, doc,listadetalle);
            Cuerpo(document, doc, listadetalle);

            return document;
        }

      

        static void Cabecera(Document document, Documento doc, List<Detalle> listadetalle)
        {
            Section section = document.AddSection();
            section.PageSetup.PageFormat = PageFormat.A4;
            section.PageSetup.TopMargin = new Unit(10, UnitType.Millimeter);
            section.PageSetup.LeftMargin = new Unit(10, UnitType.Millimeter);
            section.PageSetup.RightMargin = new Unit(10, UnitType.Millimeter);

            section.PageSetup.OddAndEvenPagesHeaderFooter = false;
            section.PageSetup.StartingNumber = 1;

            HeaderFooter header = section.Headers.Primary;
            //header.AddParagraph("\tOdd Page Header");

            TextFrame textFrame = new TextFrame();
            //textFrame.MarginTop = new Unit(100, UnitType.Millimeter);
            textFrame.Width = new Unit(200);
            textFrame.Height = new Unit(100);
            textFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            textFrame.RelativeVertical = RelativeVertical.Margin;
            textFrame.WrapFormat.DistanceLeft = new Unit(320);
            textFrame.WrapFormat.DistanceTop = new Unit(-200);
            textFrame.LineFormat.Width = new Unit(1);
            textFrame.LineFormat.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            //textFrame.FillFormat.Color = MigraDoc.DocumentObjectModel.Colors.Green;

            var texto = textFrame.AddParagraph("RUC:"+doc.rucempresa);
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            texto = textFrame.AddParagraph("FACTURA ELECTRÓNICA");
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            texto = textFrame.AddParagraph("\n"+ doc.serienumero);
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            header.Add(textFrame);


            var paragraph = header.AddParagraph("Razón Social Empresa");
            paragraph.Style = "Heading2";
            //document.LastSection.AddParagraph("Razón Social Empresa", "Heading2");
            //Paragraph paragraph = document.LastSection.AddParagraph();

            paragraph = header.AddParagraph();
            paragraph.AddText("AV. CAMINO REAL301 #602\n");
            paragraph.AddText("SURCO, LIMA, LIMA\n");
            paragraph.AddText("TELÉFONO: 12345678");

            paragraph = header.AddParagraph("\n\n\n");
            paragraph.Style = "TOC";
            paragraph.AddFormattedText("\nSEÑOR(es)\t", TextFormat.Bold);
            paragraph.AddText ("CARHUAS MIRANDA GLORIA");
            paragraph.AddFormattedText("\nRUC No\t", TextFormat.Bold);
            paragraph.AddText("09509195");
            paragraph.AddFormattedText("\nDIRECCIÓN\t", TextFormat.Bold);
            paragraph.AddText("AV.LAS LOMAS MZ.A LT.3 - Ñ Á ASOC.ALAMEDA DEL AGUSTINO");

            //Agregar Tabla
            paragraph.AddText("\n");

            header.AddParagraph("");


            Table table = new Table();
            table.Borders.Width = 0.75;

            
            Column column = table.AddColumn(Unit.FromCentimeter(4));
            column = table.AddColumn(Unit.FromCentimeter(4));
            column = table.AddColumn(Unit.FromCentimeter(4));
            column = table.AddColumn(Unit.FromCentimeter(4));


            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(Unit.FromCentimeter(4));

            Row row = table.AddRow();
            
            row.Shading.Color = Colors.LightGray;
            row.Format.Alignment = ParagraphAlignment.Center;
            Cell cell = row.Cells[0];
            cell.AddParagraph("NO INTERNO");
            cell = row.Cells[1];
            cell.AddParagraph("FECHA EMISION");
            cell = row.Cells[2];
            cell.AddParagraph("FECHA VENCIMIENTO");
            cell = row.Cells[3];
            cell.AddParagraph("CONDICIONES");
            cell = row.Cells[4];
            cell.AddParagraph("GUIA REMISION");

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Center;
            cell = row.Cells[0];
            cell.AddParagraph();
            cell = row.Cells[1];
            cell.AddParagraph(doc.fechaemision);
            cell = row.Cells[2];
            cell.AddParagraph();
            cell = row.Cells[3];
            cell.AddParagraph(doc.condicion);
            cell = row.Cells[4];
            cell.AddParagraph();

            //table.SetEdge(0, 0, 2, 3, Edge.Box, BorderStyle.Single, 1.5, Colors.Black);

            header.Add(table);

         

        }

        private static void Cuerpo(Document document, Documento doc, List<Detalle> listadetalle)
        {

            Section section = document.LastSection;
            section.PageSetup.TopMargin = new Unit(80, UnitType.Millimeter);


             section.AddParagraph("Cant.\tCodigo\tDescripcion\tPre.unit.\tSub total\tI.G.V\tTotal\n\n", "Item");
            // foreach ( string r in  ) {
            //p.AddText(doc.cantidad + "\t" + doc.importeigv + "\t" + doc.cantidad + "\t" + doc.importeigv + "\t" + doc.importetotal + "\n");
            double t = 0;
            double igv = 0;
            double sub = 0;
            
            
            foreach (var det in listadetalle)
            {
                string cant = det.cantidad.Substring(0, 1);

                section.AddParagraph(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", cant, det.codigoitem, det.descripcion, 
                    det.preciounitario,det.subtotal ,det.importeigv, det.importetotal), "Item");
                float importet = Util.ToNumber(det.importetotal);
                float importeigvt = Util.ToNumber(det.importeigv);
                float subtotalt = Util.ToNumber(det.subtotal);
                t = t + importet;
                igv = igv + importeigvt;
                sub = sub + subtotalt;


            }

            section.AddParagraph("");
            section.AddParagraph("");

            string subs = Convert.ToString(sub);
            string ts = Convert.ToString(t);
            string igvs = Convert.ToString(igv);
            string subs2 = subs.Substring(0, 5);
            string ts2 = ts.Substring(0, 5);
            string igvs2 = igvs.Substring(0, 5);


            //HeaderFooter body = section.Footers.Primary;
            

            Table table = new Table();
            table.Borders.Width = 0;
            table.Rows.LeftIndent = new Unit(330);
            


            Column column = table.AddColumn(Unit.FromCentimeter(3.5) );
            
            column = table.AddColumn(Unit.FromCentimeter(2));
            



            column.Format.Alignment = ParagraphAlignment.Right;


            
            Row row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
           Cell cell = row.Cells[0];
            cell.AddParagraph("SUB TOTAL S/.");
            cell = row.Cells[1];
            cell.AddParagraph(subs2);


       

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("DSCTO GLOBAL S/.");
            cell = row.Cells[1];
            cell.AddParagraph("");

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP. GRAVADA S/.");
            cell = row.Cells[1];
            cell.AddParagraph("");

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP.EXONERADA S/.");
            cell = row.Cells[1];
            cell.AddParagraph("");

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP.INAFECTA S/.");
            cell = row.Cells[1];
            cell.AddParagraph("");


            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP.GRATUITA S/.");
            cell = row.Cells[1];


            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("IGV 18% S/.");
            cell = row.Cells[1];
            cell.AddParagraph(igvs2);
            section.AddParagraph("");

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("");
            cell = row.Cells[1];
            cell.AddParagraph("");
           
       

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("TOTAL S/.");
            cell = row.Cells[1];
            cell.AddParagraph(ts2);

            section.Add(table);

        }

        

        static void section(Document document)
        {
            Section section = document.AddSection();
            section.PageSetup.OddAndEvenPagesHeaderFooter = true;
            section.PageSetup.StartingNumber = 1;


            Table table = new Table();
            table.Borders.Width = 0.2;

            Column column = table.AddColumn(Unit.FromCentimeter(2));
            column = table.AddColumn(Unit.FromCentimeter(4));
            column = table.AddColumn(Unit.FromCentimeter(4));
            column = table.AddColumn(Unit.FromCentimeter(4));


            column.Format.Alignment = ParagraphAlignment.Center;

            table.AddColumn(Unit.FromCentimeter(4));

            Paragraph paragraph = new Paragraph();
            paragraph.AddTab();
            paragraph.AddPageField();

            section.Footers.Primary.Add(table);


        }






        /// <summary>
        /// Defines the styles used in the document.
        /// </summary>
        private static void DefineStyles(Document document)
        {
            // Get the predefined style Normal.
            Style style = document.Styles["Normal"];
            // Because all styles are derived from Normal, the next line changes the 
            // font of the whole document. Or, more exactly, it changes the font of
            // all styles and paragraphs that do not redefine the font.
            style.Font.Name = "Times New Roman";
            // Heading1 to Heading9 are predefined styles with an outline level. An outline level
            // other than OutlineLevel.BodyText automatically creates the outline (or bookmarks) 
            // in PDF.

            style = document.Styles["Heading1"];
            style.Font.Name = "Tahoma";
            style.Font.Size = 14;
            style.Font.Bold = true;
            //style.Font.Color = Colors.DarkBlue;
            style.ParagraphFormat.PageBreakBefore = true;
            style.ParagraphFormat.SpaceAfter = 6;

            style = document.Styles["Heading2"];
            style.Font.Size = 12;
            style.Font.Bold = true;
            style.ParagraphFormat.PageBreakBefore = false;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 6;

            style = document.Styles["Heading3"];
            style.Font.Size = 10;
            style.Font.Bold = true;
            style.Font.Italic = true;
            style.ParagraphFormat.SpaceBefore = 6;
            style.ParagraphFormat.SpaceAfter = 3;

            style = document.Styles[StyleNames.Header];
            style.ParagraphFormat.AddTabStop("16cm", TabAlignment.Right);

            style = document.Styles[StyleNames.Footer];
            style.ParagraphFormat.AddTabStop("8cm", TabAlignment.Center);

            // Create a new style called TextBox based on style Normal
            style = document.Styles.AddStyle("TextBox", "Normal");
            style.ParagraphFormat.Alignment = ParagraphAlignment.Justify;
            style.ParagraphFormat.Borders.Width = 2.5;
            style.ParagraphFormat.Borders.Distance = "3pt";
            //style.ParagraphFormat.Shading.Color = Colors.SkyBlue;

            // Create a new style called TOC based on style Normal
            style = document.Styles.AddStyle("TOC", "Normal");
            style.ParagraphFormat.AddTabStop("2.5cm", TabAlignment.Left, TabLeader.Spaces);
            //style.ParagraphFormat.Font.Color = Colors.Blue;

            style = document.Styles.AddStyle("Item", "Normal");
            style.ParagraphFormat.AddTabStop("1cm", TabAlignment.Left, TabLeader.Spaces);
            style.ParagraphFormat.AddTabStop("3cm", TabAlignment.Left, TabLeader.Spaces);
            style.ParagraphFormat.AddTabStop("11cm", TabAlignment.Right, TabLeader.Spaces);
            style.ParagraphFormat.AddTabStop("13cm", TabAlignment.Right, TabLeader.Spaces);
            style.ParagraphFormat.AddTabStop("15cm", TabAlignment.Right, TabLeader.Spaces);
            style.ParagraphFormat.AddTabStop("17cm", TabAlignment.Right, TabLeader.Spaces);




            //style = document.Styles.AddStyle("Total", "Normal");



        }


    }
}
