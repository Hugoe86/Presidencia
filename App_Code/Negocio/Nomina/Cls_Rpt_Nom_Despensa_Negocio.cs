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
using Presidencia.Reporte_Despensa.Datos;

namespace Presidencia.Reporte_Despensa.Negocio
{
    public class Cls_Rpt_Nom_Despensa_Negocio
    {
        #region (Variables Privadas)
        private String No_Empleado;
        private String RFC_Empleado;
        private String Nombre_Empleado;
        private String Unidad_Responsable;
        private String Nomina_ID;
        private String No_Nomina;
        #endregion

        #region (Variables Públicas)
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_RFC_Empleado
        {
            get { return RFC_Empleado; }
            set { RFC_Empleado = value; }
        }

        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }

        public String P_Unidad_Responsable
        {
            get { return Unidad_Responsable; }
            set { Unidad_Responsable = value; }
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
        #endregion

        #region (Métodos)
        public DataTable Consultar_Despensas() {
            return Cls_Rpt_Nom_Despensa_Datos.Consultar_Despensas(this);
        }
        #endregion
    }
}
