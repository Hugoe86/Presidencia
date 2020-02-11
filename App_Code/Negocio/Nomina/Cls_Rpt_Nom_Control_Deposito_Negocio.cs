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
using Presidencia.Reportes_Nomina_Control_Deposito.Datos;

namespace Presidencia.Reportes_Nomina_Control_Deposito.Negocio
{
    public class Cls_Rpt_Nom_Control_Deposito_Negocio
    {
        #region VARIABLES LOCALES
        private String Nomina_id;
        private String No_Nomina;
        private String Tipo_Nomina_ID;
        #endregion

        #region Variables Publicas
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
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        #endregion

        #region METODOS
        public DataTable Consultar_Control_Deposito()
        {
            return Cls_Rpt_Nom_Control_Deposito_Datos.Consultar_Control_Deposito(this);
        }
        #endregion
    }
}
