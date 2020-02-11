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
using System.Collections.Generic;
using Presidencia.Incidencias_Checadas.Datos;

namespace Presidencia.Incidencias_Checadas.Negocios
{
    public class Cls_Ope_Nom_Incidencias_Checadas_Negocio
    {
        #region (Variables Internas)
            private DataTable Dt_Checadas;
            private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
            public DataTable P_Dt_Checadas
            {
                get { return Dt_Checadas; }
                set { Dt_Checadas = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
        #endregion
        #region (Metodos)
            public DataTable Consulta_Checadas_Empleados_SQL()
            {
                return Cls_Ope_Nom_Incidencias_Checadas_Datos.Consulta_Checadas_Empleados_SQL();
            }
            public void Alta_Incidencias_Checadas()
            {
                Cls_Ope_Nom_Incidencias_Checadas_Datos.Alta_Incidencias_Checadas(this);
            }
        #endregion
    }
}
