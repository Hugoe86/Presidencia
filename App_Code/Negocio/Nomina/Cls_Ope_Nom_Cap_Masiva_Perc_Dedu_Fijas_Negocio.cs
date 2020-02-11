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
using Presidencia.Captura_Masiva_Perc_Deduc_Fijas.Datos;

namespace Presidencia.Captura_Masiva_Perc_Deduc_Fijas.Negocio
{
    public class Cls_Ope_Nom_Cap_Masiva_Perc_Dedu_Fijas_Negocio
    {
        #region (Variables Privadas)
        private String Percepcion_Deduccion_ID;
        private DataTable Dt_Empleados;
        private String Nomina_ID;
        private String No_Nomina;
        private String Referencia;
        #endregion

        #region (Variables Publicas)
        public String P_Percepcion_Deduccion_ID
        {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }

        public DataTable P_Dt_Empleados {
            get { return Dt_Empleados; }
            set { Dt_Empleados = value; }
        }

        public String P_Nomina_ID {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public String P_No_Nomina {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }
        #endregion

        #region (Métodos)
        public Boolean Alta_Percepciones_Deducciones_Fijas()
        {
            return Cls_Ope_Nom_Cap_Masiva_Perc_Dedu_Fijas_Datos.Alta_Percepciones_Deducciones_Fijas(this);
        }
        #endregion
    }
}
