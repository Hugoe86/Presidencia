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
using Presidencia.Nomina_Reportes_Orden_Judicial.Datos;

namespace Presidencia.Nomina_Reportes_Orden_Judicial.Negocio
{
    public class Cls_Rpt_Nom_Orden_Judicial_Negocio
    {
        #region(Variables Privadas)
        private String Empleado_ID;
        private String No_Empleado;
        private String Tipo_Nomina_ID;
        #endregion

        #region(Variables Publicas)
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        #endregion

        #region(Metodos)
        public DataTable Consultar_Orden_Judicial()
        {
            return Cls_Rpt_Nom_Orden_Judicial_Datos.Consultar_Orden_Judicial(this);
        }
        #endregion
    }
}