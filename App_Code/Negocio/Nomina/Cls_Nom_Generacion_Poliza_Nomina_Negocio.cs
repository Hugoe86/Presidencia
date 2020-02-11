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
using Presidencia.Generacion_Poliza_Nomina.Datos;

namespace Presidencia.Generacion_Poliza_Nomina.Negocio
{
    public class Cls_Nom_Generacion_Poliza_Nomina_Negocio
    {
        #region (Variables Privadas)
        private String Nomina_ID;
        private String No_Nomina;
        private String Cuenta_Contable_ID = String.Empty;
        private String Proveedor_ID;
        private String Percepcion_Deduccion_ID;
        #endregion

        #region (Variables Publicas)
        public String P_Nomina_ID {
            get { return Nomina_ID; }
            set { Nomina_ID = value; }
        }

        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }

        public String P_Cuenta_Contable_ID
        {
            get { return Cuenta_Contable_ID; }
            set { Cuenta_Contable_ID = value; }
        }

        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }

        public String P_Percepcion_Deduccion_ID {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Consultar_Cuentas_Contables_Nomina() {
            return Cls_Ope_Nom_Generacion_Poliza_Nomina_Datos.Consultar_Cuentas_Contables_Nomina(this);
        }

        public DataTable Consultar_Percepciones_Deducciones_General() {
            return Cls_Ope_Nom_Generacion_Poliza_Nomina_Datos.Consultar_Percepciones_Deducciones_General(this);
        }

        public String Consultar_Cuenta_Contable_Proveedor() {
            return Cls_Ope_Nom_Generacion_Poliza_Nomina_Datos.Consultar_Cuenta_Contable_Proveedor(this);
        }

        public DataTable Consulta_Cuentas_Contables() {
            return Cls_Ope_Nom_Generacion_Poliza_Nomina_Datos.Consulta_Cuentas_Contables(this);
        }
        #endregion
    }
}
