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
using Presidencia.Nomina_Reporte_Tiempo_Extra.Datos;

namespace Presidencia.Nomina_Reporte_Tiempo_Extra.Negocio
{
    public class Cls_Rpt_Nom_Tiempo_Extra_Negocio
    {
        #region(Variables Privadas)
        private String Nomina_id;
        private String No_Nomina;
        private String Empleado_ID;
        #endregion

        #region(Variables Publicas)
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
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        #endregion

        #region(Metodos)
        public DataTable Consultar_Tiempo_Extra()
        {
            return Cls_Rpt_Nom_Tiempo_Extra_Datos.Consultar_Tiempo_Extra(this);
        }
        public DataTable Consultar_Dia_Festivo()
        {
            return Cls_Rpt_Nom_Tiempo_Extra_Datos.Consultar_Dia_Festivo(this);
        }
        public DataTable Consultar_Prima_Dominical()
        {
            return Cls_Rpt_Nom_Tiempo_Extra_Datos.Consultar_Prima_Dominical(this);
        }
        #endregion
    }
}