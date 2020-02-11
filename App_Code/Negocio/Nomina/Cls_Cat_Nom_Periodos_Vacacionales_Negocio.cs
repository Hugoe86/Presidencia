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
using Presidencia.Periodos_Vacacionales.Datos;

namespace Presidencia.Periodos_Vacacionales.Negocio
{
    public class Cls_Cat_Nom_Periodos_Vacacionales_Negocio
    {
        #region (Variables Publicas)
        private String No_Empleado;
        #endregion

        #region (Variables Privadas)
        public String P_No_Empleado {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Consultar_Periodos_Vacacionales() {
            return Cls_Cat_Nom_Periodos_Vacacionales_Datos.Consultar_Periodos_Vacacionales(this);
        }

        public DataTable Consultar_Vacaciones_Tomadas() {
            return Cls_Cat_Nom_Periodos_Vacacionales_Datos.Consultar_Vacaciones_Tomadas(this);
        }
        #endregion
    }
}
