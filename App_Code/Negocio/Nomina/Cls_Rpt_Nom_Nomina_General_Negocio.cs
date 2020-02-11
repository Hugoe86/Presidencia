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
using Presidencia.Reportes_Nomina_General.Datos;

namespace Presidencia.Reportes_Nomina_General.Negocio
{
    public class Cls_Rpt_Nom_Nomina_General_Negocio
    {
        #region Variables Privadas
        private String Nomina_id;
        private String No_Nomina;
        private String Tipo;
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
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        #endregion

        #region(Metodos)
        public DataTable Consultar_Nomina_General()
        {
            return Cls_Rpt_Nom_Nomina_General_Datos.Consultar_Nomina_General(this);
        }
        #endregion
    }
}