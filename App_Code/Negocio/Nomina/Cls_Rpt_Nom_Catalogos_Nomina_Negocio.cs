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
using Presidencia.Rpt_Cat_Nomina.Datos;

namespace Presidencia.Rpt_Cat_Nomina.Negocio
{
    public class Cls_Rpt_Nom_Catalogos_Nomina_Negocio
    {
        #region (Variables Privadas)
        private String Tabla;
        private String Mi_SQL;
        #endregion

        #region (Variables Públicas)
        public String P_Tabla
        {
            get { return Tabla; }
            set { Tabla = value; }
        }

        public String P_Mi_SQL
        {
            get { return Mi_SQL; }
            set { Mi_SQL = value; }
        }
        #endregion

        #region (Métodos)
        public DataTable Consultar_Tablas_Nomina() {
            return Cls_Rpt_Nom_Catalogos_Nomina_Datos.Consultar_Tablas_Nomina();
        }

        public DataTable Consultar_Campos_Por_Tabla() {
            return Cls_Rpt_Nom_Catalogos_Nomina_Datos.Consultar_Campos_Por_Tabla(this);
        }

        public DataTable Ejecutar_Consulta() {
            return Cls_Rpt_Nom_Catalogos_Nomina_Datos.Ejecutar_Consulta(this);
        }
        #endregion
    }
}
