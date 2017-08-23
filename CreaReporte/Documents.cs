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
        public static Document CreateDocument()
        {
            // Create a new MigraDoc document
            Document document = new Document();
            document.Info.Title = "Documento Electrónico";
            document.Info.Subject = "Documento Electrónico en Migradoc";
            document.Info.Author = "Luis Pezo";
            DefineStyles(document);
            Cabecera(document);

            return document;
        }

        static void Cabecera(Document document)
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
            textFrame.Width = new Unit(200);
            textFrame.Height = new Unit(100);
            textFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            textFrame.RelativeVertical = RelativeVertical.Margin;
            textFrame.WrapFormat.DistanceLeft = new Unit(320);
            textFrame.WrapFormat.DistanceTop = new Unit(0);
            textFrame.LineFormat.Width = new Unit(1);
            textFrame.LineFormat.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            //textFrame.FillFormat.Color = MigraDoc.DocumentObjectModel.Colors.Green;
            var texto = textFrame.AddParagraph("RUC: 12345678901");
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            texto = textFrame.AddParagraph("FACTURA ELECTRÓNICA");
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            texto = textFrame.AddParagraph("\nF001-00000001");
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            section.Add(textFrame);


            document.LastSection.AddParagraph("Razón Social Empresa", "Heading2");

            Paragraph paragraph = document.LastSection.AddParagraph();

            paragraph.Format.Alignment = ParagraphAlignment.Left;
            paragraph.AddText("AV. CAMINO REAL301 #602\n");
            paragraph.AddText("SURCO, LIMA, LIMA\n");
            paragraph.AddText("TELÉFONO: 12345678");


            paragraph = document.LastSection.AddParagraph("\n\n\n", "TOC");
            paragraph.AddFormattedText("\nSEÑOR(es)\t", TextFormat.Bold);
            paragraph.AddText ("CARHUAS MIRANDA GLORIA");
            paragraph.AddFormattedText("\nDNI No\t", TextFormat.Bold);
            paragraph.AddText("09509195");
            paragraph.AddFormattedText("\nDIRECCIÓN\t", TextFormat.Bold);
            paragraph.AddText("AV.LAS LOMAS MZ.A LT.3 - Ñ Á ASOC.ALAMEDA DEL AGUSTINO");



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
        }


    }
}
