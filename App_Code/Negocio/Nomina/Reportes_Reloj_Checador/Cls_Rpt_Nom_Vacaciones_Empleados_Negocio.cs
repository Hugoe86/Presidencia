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
using Presidencia.Reportes_nomina_Vacaciones_Empleados.Datos;

namespace Presidencia.Reportes_nomina_Vacaciones_Empleados.Negocio
{
    public class Cls_Rpt_Nom_Vacaciones_Empleados_Negocio
    {
        #region Variables Privadas
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        private String Empleado_ID;
        private String No_Empleado;
        private String Nomina_id;
        private String No_Nomina;
        #endregion


        #region Variables Publicas
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
        public String P_Nomina_id
        {
            get { return Nomina_id; }
            set { Nomina_id = value; }
        }
        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        #endregion

        #region(Metodos)
        public DataTable Consultar_Dias_Vacaciones()
        {
            return Cls_Rpt_Nom_Vacaciones_Empleados_Datos.Consultar_Dias_Vacaciones(this);
        }
        #endregion
    }
}