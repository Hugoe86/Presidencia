using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Cap_Masiva_Prov_Fijas.Datos;

namespace Presidencia.Cap_Masiva_Prov_Fijas.Negocio
{
    public class Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Negocio
    {
        #region (Variables Privadas)
        private String Empleado_ID;
        private DataTable Dt_Perc_Dedu_Empl;
        private String Nomina_ID;
        private String No_Nomina;
        private String Referencia;
        private String Proveedor_ID;
        #endregion

        #region (Variables Publicas)
        public String P_Empleado_ID {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public DataTable P_Dt_Perc_Dedu_Empl {
            get { return Dt_Perc_Dedu_Empl; }
            set { Dt_Perc_Dedu_Empl = value; }
        }

        public String P_Nomina_ID {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }

        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        #endregion

        #region (Métodos)
        public DataTable Consultar_Perc_Dedu_Empleado() {
            return Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Datos.Consultar_Perc_Dedu_Empleado(this);
        }

        public Boolean Guardar_Deducciones_Fijas() {
            return Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Datos.Guardar_Deducciones_Fijas(this);
        }

        public DataTable Consultar_Claves_Disponibles() {
            return Cls_Ope_Nom_Cap_Masiva_Prov_Fijas_Datos.Consultar_Claves_Disponibles(this);
        }
        #endregion
    }
}
