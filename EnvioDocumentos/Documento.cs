using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnvioDocumentos
{
    class Documento
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

        public Documento(SqlDataReader reader)
        {
            this.idcp = Util.GetInt(reader, "idcpe");
            this.rucempresa = Util.GetString(reader, "rucempresa");
            this.totalventa = Util.GetString(reader, "totalventa");
        }




        public override string ToString()
        {
            return string.Format("idcp = {0} rucempresa = {1} totalventa={2} ", idcp, rucempresa, totalventa);
        }


    }
}
