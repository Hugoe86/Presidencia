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
using Presidencia.PSM.Datos;

namespace Presidencia.PSM.Negocio
{
    public class Cls_Rpt_Nom_Prevision_Social_Multiple_Negocio
    {
        #region (Privadas)
        private String No_Empleado;
        private String Nombre;
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        #endregion

        #region (Publica)
        public String P_No_Empleado {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Reporte_Prevision_Social_Multiple() { return Cls_Rpt_Nom_Prevision_Social_Multiple_Datos.Reporte_Prevision_Social_Multiple(this); }
        #endregion

    }
}
