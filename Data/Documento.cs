using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class Documento
    {
        public int idcp { get; set; }
        public string rucempresa { get; set; }
        public string estadoregistro { get; set; }
        public string tipodocafecta { get; set; }
        public string tiponotacredeb { get; set; }
        public string sustentonotacredeb { get; set; }
        public string correocliente { get; set; }
        public string correoempresa { get; set; }
        public string departamentoempresa { get; set; }
        public string direccionempresa { get; set; }
        public string distritoempresa { get; set; }
        public string nombrecomercialempresa { get; set; }
        public string numerodocumentocliente { get; set; }
        public string paiscliente { get; set; }
        public string provinciaempresa { get; set; }
        public string razonsocialcliente { get; set; }
        public string razonsocialempresa { get; set; }
        public string serienumeroafectado { get; set; }
        public string subtotal { get; set; }
        public string tipodocumentocliente { get; set; }
        public string tipomoneda { get; set; }
        public string totaldescuentos { get; set; }
        public string totaligv { get; set; }
        public string totalisc { get; set; }
        public string totalexoneradas { get; set; }
        public string totalgratuitas { get; set; }
        public string totalgravadas { get; set; }
        public string totalnogravada { get; set; }
        public string totalventa { get; set; }
        public string ubigeoempresa { get; set; }
        public string direccioncliente { get; set; }
        public string mensajeerror { get; set; }
        public string condicion { get; set; }
        public string guiaremision { get; set; }
        public string hash { get; set; }
        public string montoPercepcion { get; set; }
        public string totalDocPercepcion { get; set; }
        public int enCustodia { get; set; }
        public string telefono1Empresa { get; set; }
        public int ensunat { get; set; }
        public string fechaemision { get; set; }
        public string tipodocumento { get; set; }
        public string serienumero { get; set; }



        /*public string descripcion { get; set; }
        public string codigo { get; set; }
        public string cantidad { get; set; }
        public string importetotal { get; set; }
        public string importeigv { get; set; }
        public string subtotal2 { get; set; }*/


        public Documento()
        {

        }

        public Documento(SqlDataReader reader)
        {
            /*this.subtotal2 = Util.GetString(reader, "subtotal");
            this.cantidad = Util.GetString(reader, "cantidad");
            this.descripcion = Util.GetString(reader, "descripcion");
            this.codigo = Util.GetString(reader, "codigo");
            this.importetotal = Util.GetString(reader, "importetotal");
            this.importeigv = Util.GetString(reader, "importeigv");*/
            

            this.idcp = Util.GetInt(reader, "idcpe");
            this.rucempresa = Util.GetString(reader, "rucempresa");
            this.totalventa = Util.GetString(reader, "totalventa");
            this.estadoregistro = Util.GetString(reader, "estadoregistro");
            this.tipodocafecta = Util.GetString(reader, "tipodocafecta");
            this.tiponotacredeb = Util.GetString(reader, "tiponotacredeb");
            this.correocliente = Util.GetString(reader, "correocliente");
            this.correoempresa = Util.GetString(reader, "correoempresa");
            this.departamentoempresa = Util.GetString(reader, "departamentoempresa");
            this.direccionempresa = Util.GetString(reader, "direccionempresa");
            this.distritoempresa = Util.GetString(reader, "distritoempresa");
            this.nombrecomercialempresa = Util.GetString(reader, "nombrecomercialempresa");
            this.numerodocumentocliente = Util.GetString(reader, "numerodocumentocliente");
            this.paiscliente = Util.GetString(reader, "paiscliente");
            this.provinciaempresa = Util.GetString(reader, "provinciaempresa");
            this.razonsocialcliente = Util.GetString(reader, "razonsocialcliente");
            this.razonsocialempresa = Util.GetString(reader, "razonsocialempresa");
            this.serienumeroafectado = Util.GetString(reader, "serienumeroafectado");
            this.subtotal = Util.GetString(reader, "subtotal");
            this.tipodocumentocliente = Util.GetString(reader, "tipodocumentocliente");
            this.tipomoneda = Util.GetString(reader, "tipomoneda");
            this.totaldescuentos = Util.GetString(reader, "totaldescuentos");
            this.totaligv = Util.GetString(reader, "totaligv");
            this.totalisc = Util.GetString(reader, "totalisc");
            this.totalexoneradas = Util.GetString(reader, "totalexoneradas");
            this.totalgratuitas = Util.GetString(reader, "totalgratuitas");
            this.totalgravadas = Util.GetString(reader, "totalgravadas");
            this.totalnogravada = Util.GetString(reader, "totalnogravada");
            this.ubigeoempresa = Util.GetString(reader, "ubigeoempresa");
            this.ensunat = Util.GetByte(reader, "ensunat");
            this.direccioncliente = Util.GetString(reader, "direccioncliente");
            this.mensajeerror = Util.GetString(reader, "mensajeerror");
            this.condicion = Util.GetString(reader, "condicion");
            this.guiaremision = Util.GetString(reader, "guiaremision");
            this.enCustodia = Util.GetByte(reader, "enCustodia");
            this.hash = Util.GetString(reader, "hash");
            this.montoPercepcion = Util.GetString(reader, "montoPercepcion");
            this.totalDocPercepcion = Util.GetString(reader, "totalDocPercepcion");
            this.telefono1Empresa = Util.GetString(reader, "telefono1Empresa");
            this.fechaemision = Util.GetString(reader, "fechaemision");
            this.tipodocumento = Util.GetString(reader, "tipodocumento");
            this.serienumero = Util.GetString(reader, "serienumero");
        }

        public override string ToString()
        {
            return string.Format("tipodocumento =  {0}  rucempresa = {1} totalventa = {2} serienumero = {3} fechaemision = {4}" , tipodocumento, rucempresa,totalventa, serienumero,fechaemision);
       
    }


    }
}
