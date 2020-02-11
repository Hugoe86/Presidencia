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
using Presidencia.Percepciones_Deducciones_Fijas.Datos;

namespace Presidencia.Percepciones_Deducciones_Fijas.Negocio
{
    public class Cls_Ope_Nom_Percepciones_Deducciones_Fijas_Negocio
    {
        #region (Variables Privadas)
        private DataTable Dt_Percepciones_Tipo_Nomina;
        private DataTable Dt_Deducciones_Tipo_Nomina;
        private String Empleado_ID;
        private String Concepto;
        #endregion

        #region (Variables Públicas)
        public DataTable P_Dt_Percepciones_Tipo_Nomina
        {
            get { return Dt_Percepciones_Tipo_Nomina; }
            set { Dt_Percepciones_Tipo_Nomina = value; }
        }

        public DataTable P_Dt_Deducciones_Tipo_Nomina
        {
            get { return Dt_Deducciones_Tipo_Nomina; }
            set { Dt_Deducciones_Tipo_Nomina = value; }
        }

        public String P_Empleado_ID {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }

        public String P_Concepto {
            get { return Concepto; }
            set { Concepto = value; }
        }
        #endregion

        #region (Métodos)
        public void Registro_Percepciones_Deducciones_Tipo_Nomina() {
            Cls_Ope_Nom_Percepciones_Deducciones_Fijas_Datos.Registro_Percepciones_Deducciones_Tipo_Nomina(this);
        }
        #endregion
    }
}
