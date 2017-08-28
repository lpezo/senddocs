using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

 namespace Data
{
 public class Detalle
    {

        public string descripcion { get; set; }
        public string codigoitem { get; set; }
        public string cantidad { get; set; }
        public string importetotal { get; set; }
        public string importeigv { get; set; }
        public string subtotal { get; set; }

        public Detalle(SqlDataReader reader)
        {
            this.subtotal = Util.GetString(reader, "subtotal");
            this.cantidad = Util.GetString(reader, "cantidad");
            this.descripcion = Util.GetString(reader, "descripcion");
            this.codigoitem = Util.GetString(reader, "codigoitem");
            this.importetotal = Util.GetString(reader, "importetotal");
            this.importeigv = Util.GetString(reader, "importeigv");
        }
    }




}
