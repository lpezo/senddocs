using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Data
{
    public class ClOficinaFecha
    {
        public int oficina { get; set; }
        public string tipo { get; set; }
        public string serie { get; set; }
        public string estadodocumento { get; set; }
        public string fechaemision { get; set; }
        public string tipodocumento { get; set; }
        public string numero { get; set; }
        public string totalventa { get; set; }
        public string subtotal { get; set; }
        public string totalexoneradas { get; set; }
        public string totalnogravada { get; set; }
        public string totalisc { get; set; }
        public string totaligv { get; set; }
        public string estadoregistro { get; set; }
        public string tipomoneda { get; set; }
        public string mensajeerror { get; set; }
        public string numerodocumentocliente { get; set; }
        public string fechaestado { get; set; }

        public static ClOficinaFecha FromDoc(Documento doc, int oficina)
        {
            var obj = new ClOficinaFecha();
            obj.oficina = oficina;
            obj.tipo = doc.serienumero.StartsWith("F") ? "F" : "R";
            obj.serie = doc.serienumero.Substring(0, 4);
            obj.fechaemision = doc.fechaemision;
            obj.tipodocumento = doc.tipodocumento;
            obj.numero = doc.serienumero.Length >= 13 ? doc.serienumero.Substring(5, 8) : "";
            obj.totalventa = doc.totalventa;
            obj.subtotal = doc.subtotal;
            obj.totalexoneradas = doc.totalexoneradas;
            obj.totalnogravada = doc.totalnogravada;
            obj.totalisc = doc.totalisc;
            obj.totaligv = doc.totaligv;
            obj.estadoregistro = doc.estadoregistro;
            obj.tipomoneda = doc.tipomoneda;
            obj.mensajeerror = doc.mensajeerror;
            obj.numerodocumentocliente = doc.numerodocumentocliente;
            obj.fechaestado = doc.fechaestado.ToString("yyyy-MM-dd");
            return obj;
        }
    }

}
