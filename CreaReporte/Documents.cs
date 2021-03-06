﻿using Data;

using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Shapes;
using MigraDoc.DocumentObjectModel.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using BarcodeLib;
using System.Drawing.Imaging;
//using Gma.QrCodeNet.Encoding;
//using Gma.QrCodeNet.Encoding.Windows.Render;
using System.Drawing;
using System.IO;
using ZXing;
using System.ComponentModel;
using System.Xml;

namespace CreaReporte
{


    class Documents
    {
        private static ITypeDescriptorContext img;

        public static Document CreateDocument(Documento doc, List<Detalle> listadetalle)
        {
            
            
            // Create a new MigraDoc document
            Document document = new Document();
            document.Info.Title = "Documento Electrónico";
            document.Info.Subject = "Documento Electrónico en Migradoc";
            document.Info.Author = "Luis Pezo";
            DefineStyles(document);
            Cabecera(document, doc, listadetalle);
            Cuerpo(document, doc, listadetalle);
            footer(document);
           // barcode(document, doc, listadetalle);

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
            //string ii = Path.Combine(cr, "logo.jpg");
            string cr = @"E:\ProgramaC#\senddocs\logo.jpg";
            if (File.Exists(cr))
            {

                Section sectionlogo = document.LastSection;
                TextFrame logo = new TextFrame();
                logo.RelativeHorizontal = RelativeHorizontal.Margin;
                logo.RelativeVertical = RelativeVertical.Margin;
                var serienn = doc.serienumero.Substring(0, 1);
                if (doc.tipodocumento=="01") {
                    logo.WrapFormat.DistanceTop = new Unit(-230);
            }
                else if (doc.tipodocumento == "03") { logo.WrapFormat.DistanceTop = new Unit(-180); }
                else if (doc.tipodocumento == "07") { logo.WrapFormat.DistanceTop = new Unit(-200); }
                var im = logo.AddImage(@"E:\ProgramaC#\senddocs\logo.jpg");
                im.Width = new Unit(75);
                section.Add(logo);

            }
            TextFrame textFrame = new TextFrame();
       
            //textFrame.MarginTop = new Unit(100, UnitType.Millimeter);
            textFrame.Width = new Unit(200);
            textFrame.Height = new Unit(100);
            textFrame.RelativeHorizontal = RelativeHorizontal.Margin;
            textFrame.RelativeVertical = RelativeVertical.Margin;
            textFrame.WrapFormat.DistanceLeft = new Unit(320);
             if (doc.tipodocumento == "01") {
                textFrame.WrapFormat.DistanceTop = new Unit(-260); }
            else if (doc.tipodocumento == "03")
            {
                textFrame.WrapFormat.DistanceTop = new Unit(-210);
            }
            else if (doc.tipodocumento == "07")
            {
                textFrame.WrapFormat.DistanceTop = new Unit(-230);
            }
            textFrame.LineFormat.Width = new Unit(1);
            textFrame.LineFormat.Color = MigraDoc.DocumentObjectModel.Colors.Black;
            //textFrame.FillFormat.Color = MigraDoc.DocumentObjectModel.Colors.Green;

            var texto = textFrame.AddParagraph("RUC:" + doc.rucempresa);
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            if (doc.tipodocumento=="01")
            {
                texto = textFrame.AddParagraph("FACTURA ELECTRÓNICA");
                texto.Format.Alignment = ParagraphAlignment.Center;
                texto.Style = "Heading1";
            }
            else if (doc.tipodocumento== "03")
            {


                texto = textFrame.AddParagraph("BOLETA ELECTRÓNICA");

                texto.Format.Alignment = ParagraphAlignment.Center;
                texto.Style = "Heading1";
            }
            else if (doc.tipodocumento == "07")
            {


                texto = textFrame.AddParagraph("Nota de Credito Electronica");

                texto.Format.Alignment = ParagraphAlignment.Center;
                texto.Style = "Heading1";
            }
            texto = textFrame.AddParagraph("\n" + doc.serienumero);
            texto.Format.Alignment = ParagraphAlignment.Center;
            texto.Style = "Heading1";
            header.Add(textFrame);
           //string cr = @"E:\ProgramaC#\senddocs\logo.jpg";
            var paragraph = header.AddParagraph(doc.razonsocialempresa);
            paragraph.Style = "Heading2";
            paragraph = header.AddParagraph();

            if (!File.Exists(cr))
            {

                paragraph.AddText(doc.direccionempresa + "\n");
                paragraph.AddText(doc.departamentoempresa + "," + doc.provinciaempresa + "," + doc.distritoempresa + "\n");
                paragraph.AddText(doc.telefono1Empresa + "\n");
            }else
            {
                TextFrame tflogo = new TextFrame();
                var tl = tflogo.AddParagraph();
                tflogo.Width = new Unit(250);
                tflogo.Height = new Unit(100);
                //tflogo.RelativeHorizontal = RelativeHorizontal.Margin;
                //tflogo.RelativeVertical = RelativeVertical.Margin;
              
                tflogo.WrapFormat.DistanceTop=new Unit(-10);
                tflogo.WrapFormat.DistanceLeft = new Unit(80);
                tflogo.WrapFormat.DistanceBottom = new Unit(-60);
                tflogo.AddParagraph(doc.direccionempresa + "\n");
               tflogo.AddParagraph(doc.departamentoempresa + "," + doc.provinciaempresa + "," + doc.distritoempresa + "\n");
                tflogo.AddParagraph(doc.telefono1Empresa + "\n");
                //paragraph.AddText(doc.direccionempresa + "\n");
                //paragraph.AddText(doc.departamentoempresa + "," + doc.provinciaempresa + "," + doc.distritoempresa + "\n");
                //paragraph.AddText(doc.telefono1Empresa + "\n");
                header.Add(tflogo);
            }

            if (doc.tipodocumento=="03")
            {
                paragraph = header.AddParagraph("\n\n\n");
                paragraph.Style = "TOC";
                paragraph.AddFormattedText("\nSEÑOR(es)\t", TextFormat.Bold);
                paragraph.AddText(doc.razonsocialcliente);
                paragraph.AddFormattedText("\nDNI No\t", TextFormat.Bold);
                paragraph.AddText(doc.numerodocumentocliente);
                paragraph.AddFormattedText("\nDIRECCIÓN\t", TextFormat.Bold);
                paragraph.AddText(doc.direccioncliente);
                paragraph.AddFormattedText("\nPARTIDA\t", TextFormat.Bold);
                paragraph.AddText(doc.puntopartida);
            }
            else if (doc.tipodocumento=="01")
            {
                paragraph = header.AddParagraph("\n\n\n");
                paragraph.Style = "TOC";
                paragraph.AddFormattedText("\nSEÑOR(es)\t", TextFormat.Bold);
                paragraph.AddText(doc.razonsocialcliente);
                paragraph.AddFormattedText("\nRUC No\t", TextFormat.Bold);
                paragraph.AddText(doc.numerodocumentocliente);
                paragraph.AddFormattedText("\nDIRECCIÓN\t", TextFormat.Bold);
                paragraph.AddText(doc.direccioncliente);
                paragraph.AddFormattedText("\nPARTIDA\t", TextFormat.Bold);
                paragraph.AddText(doc.puntopartida);
                paragraph.AddFormattedText("\nLLEGADA\t", TextFormat.Bold);
                paragraph.AddText(doc.puntollegada);
                paragraph.AddFormattedText("\nTRASNPORTE\t", TextFormat.Bold);
                paragraph.AddText(doc.codigochofer);
                paragraph.AddFormattedText("\nCOD.VEND\t", TextFormat.Bold);
                paragraph.AddText(doc.codigovendedor);
                paragraph.AddFormattedText("\nNOM.VEND\t", TextFormat.Bold);
                paragraph.AddText(doc.nombrevendedor);
            }
            if (doc.tipodocumento == "07")
            {
                paragraph = header.AddParagraph("\n\n\n");
                paragraph.Style = "TOC";
                paragraph.AddFormattedText("\nSEÑOR(es)\t", TextFormat.Bold);
                paragraph.AddText(doc.razonsocialcliente);
                paragraph.AddFormattedText("\nRUC No\t", TextFormat.Bold);
                paragraph.AddText(doc.numerodocumentocliente);
                paragraph.AddFormattedText("\nDIRECCIÓN\t", TextFormat.Bold);
                paragraph.AddText(doc.direccioncliente);
                paragraph.AddFormattedText("\nDOC.AFECTADO\t", TextFormat.Bold);
                paragraph.AddText(doc.serienumeroafectado+ "\n");
                paragraph.AddText(doc.sustentonotacredeb);
            }

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

            //string cr = @"E:\ProgramaC#\senddocs\logo.jpg";


            //if (File.Exists(cr))
            //{

            //    Section sectionlogo = document.LastSection;
            //    TextFrame logo = new TextFrame();
            //    logo.WrapFormat.DistanceTop = new Unit(-475);
            //    var im = logo.AddImage(@"E:\ProgramaC#\senddocs\logo.jpg");
            //    im.Width = new Unit(75);
            //    section.Add(logo);

            //}

            header.Add(table);



        }

        private static void Cuerpo(Document document, Documento doc, List<Detalle> listadetalle)
        {

            Section section = document.LastSection;
            if (doc.tipodocumento == "01")
            {
                section.PageSetup.TopMargin = new Unit(103, UnitType.Millimeter);
            }
            else if (doc.tipodocumento=="03") { section.PageSetup.TopMargin = new Unit(87, UnitType.Millimeter); }
            else if (doc.tipodocumento=="07") { section.PageSetup.TopMargin = new Unit(93, UnitType.Millimeter); }
            //TextFrame cuerpo = new TextFrame();
            //cuerpo.Width = new Unit(600);
            //cuerpo.WrapFormat.DistanceTop = new Unit(-10);
            //cuerpo.WrapFormat.DistanceBottom = new Unit(-50);
            //var cuerpodata = cuerpo.AddParagraph("Cant.\tCodigo\tDescripcion\tPre.unit.\tSub total\tI.G.V\tTotal\n\n");
            //section.Add(cuerpo);
            section.AddParagraph("Cant.\tCodigo\tDescripcion\tPre.unit.\tSub total\tI.G.V\tTotal\n\n", "Item");
            double t = 0;
            double igv = 0;
            double sub = 0;

            foreach (var det in listadetalle)
            {
                string cant = det.cantidad.Substring(0, 1);

                section.AddParagraph(string.Format("{0}\t{1}\t{2}\t{3}\t{4}\t{5}\t{6}\n", cant, det.codigoitem, det.descripcion,
                    det.preciounitario, det.subtotal, det.importeigv, det.importetotal), "Item");
                //float importet = Util.ToNumber(det.importetotal);
                //float importeigvt = Util.ToNumber(det.importeigv);
                //float subtotalt = Util.ToNumber(det.subtotal);
                //t = t + importet;
                //igv = igv + importeigvt;
                //sub = sub + subtotalt;


            }

            section.AddParagraph("");
            var par = section.AddParagraph("");

            ////string subs = Convert.ToString(sub);
            ////string ts = Convert.ToString(t);
            ////string igvs = Convert.ToString(igv);
            ////string subs2 = subs.Substring(0, 5);
            ////string ts2 = ts.Substring(0, 5);
            ////string igvs2 = igvs.Substring(0, 5);


           


            Table table = new Table();
            table.Borders.Width = 0;
            document.DefaultPageSetup.TopMargin = new Unit(20, UnitType.Millimeter);
            table.Rows.LeftIndent = new Unit(330);

            Column column = table.AddColumn(Unit.FromCentimeter(3.5));

            column = table.AddColumn(Unit.FromCentimeter(2));


            column.Format.Alignment = ParagraphAlignment.Right;



            Row row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            Cell cell = row.Cells[0];
            cell.AddParagraph("SUB TOTAL S/.");
            cell = row.Cells[1];
            cell.AddParagraph(doc.subtotal);




            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("DSCTO GLOBAL S/.");
            cell = row.Cells[1];
            cell.AddParagraph(doc.totaldescuentos);

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP. GRAVADA S/.");
            cell = row.Cells[1];
            cell.AddParagraph(doc.totalgravadas);

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP.EXONERADA S/.");
            cell = row.Cells[1];
            cell.AddParagraph(doc.totalexoneradas);

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP.INAFECTA S/.");
            cell = row.Cells[1];
            cell.AddParagraph(doc.totalnogravada);


            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("OP.GRATUITA S/.");
            cell = row.Cells[1];
            cell.AddParagraph(doc.totalgratuitas);

            row = table.AddRow();
            row.Format.Alignment = ParagraphAlignment.Right;
            cell = row.Cells[0];
            cell.AddParagraph("IGV 18% S/.");
            cell = row.Cells[1];
            cell.AddParagraph(doc.totaligv);

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
            cell.AddParagraph(doc.totalventa);

            TextFrame tablef = new TextFrame();
            tablef.WrapFormat.DistanceTop = new Unit(-115);
            tablef.WrapFormat.DistanceLeft = new Unit(-20);

            BarcodeWriter writer = new BarcodeWriter();
            writer.Format = BarcodeFormat.QR_CODE;
            var bit = writer.Write(string.Format("{0}|{1}|{2}|{3}|{4}|{5}|{6}", doc.rucempresa, doc.tipodocumento, doc.serienumero, doc.totaligv, doc.totalventa, doc.fechaemision, doc.hash));

            var f = tablef.AddImage("qr.bmp");
            f.Width = new Unit(130);
            
            section.Add(table);
            section.Add(tablef);
            var parr= section.AddParagraph("\n\n\t\t\tAutorizado mediante resolución Nro: 203 - 2015/SUNAT");
            var parr2 = section.AddParagraph("\t\t\tRepresentación impresa de la boleta electrónica");
            parr.Format.Alignment = ParagraphAlignment.Left;
            parr2.Format.Alignment = ParagraphAlignment.Center;
            NumToWords totalLetras = new NumToWords();
            string numero = Convert.ToString(doc.totalventa);
            string TotalEnLetras = totalLetras.Convertir(numero, true);
            parr2.Section.AddParagraph("\nSON:" + TotalEnLetras);

            parr2.Format.Alignment = ParagraphAlignment.Left;
        

        }
   



        private static void footer(Document document)
        {
            Section section2 = document.LastSection;
            section2.PageSetup.OddAndEvenPagesHeaderFooter = false;
            section2.PageSetup.StartingNumber = 1;
            section2.PageSetup.BottomMargin = new Unit(1, UnitType.Centimeter);
            HeaderFooter footer = section2.Footers.Primary;
            
            ////TextFrame framefooter = new TextFrame();
            ////framefooter.Width = new Unit(500);
            ////framefooter.Height = new Unit(5);
            ////framefooter.WrapFormat.DistanceBottom= new Unit(-200);
            ////var par = framefooter.AddParagraph();

            ////par.AddFormattedText("Para consultar el documento ingrese a portal.factura.com.pe/corpora");
            ////par.Format.Alignment = ParagraphAlignment.Left;
            ////var parr = framefooter.AddParagraph();
            ////parr.AddFormattedText("Powered");
            ////parr.Format.Alignment = ParagraphAlignment.Right;
            ////parr.Format.RightIndent = new Unit(100);
            ////var parrr = framefooter.AddParagraph();
            ////parrr.AddFormattedText("\t\t\tby VIDA.SOFTWARE");
            ////parrr.Format.Alignment = ParagraphAlignment.Right;

            //section2.Add(framefooter);


            var paragraph = footer.AddParagraph();
            //paragraph.AddFormattedText("\n\n\n\n\n");
            paragraph.AddFormattedText("Para consultar el documento ingrese a portal.factura.com.pe/corpora\n\t\t\t\tPowered\n\t\t\t\t\tby VIDA.SOFTWARE");
            paragraph.Format.Alignment = ParagraphAlignment.Left;


            //NumToWords totalWord = new NumToWords();
            //string numero = "57.60";
            //Console.WriteLine(totalWord.Convertir(numero,true));
            //Console.ReadLine();

        }




        private static object ImageToByte(Bitmap aztecBitmap)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
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
