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
using Presidencia.Relacion_Perc_Dedu_Cuentas_Contables.Datos;

namespace Presidencia.Relacion_Perc_Dedu_Cuentas_Contables.Negocio
{
    public class Cls_Cat_Nom_Perc_Dedu_CC_Deta_Negocio
    {
        #region (Variables Publicas)
        private String Percepcion_Deduccion_ID;
        private String Clave;
        private String Cuenta_Contable_ID;
        private String Cuenta;
        private DataTable Dt_Cuentas_Contables;
        #endregion

        #region (Variables Privadas)
        public String P_Percepcion_Deduccion_ID
        {
            get { return Percepcion_Deduccion_ID; }
            set { Percepcion_Deduccion_ID = value; }
        }

        public String P_Cuenta_Contable_ID
        {
            get { return Cuenta_Contable_ID; }
            set { Cuenta_Contable_ID = value; }
        }

        public String P_CLave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public DataTable P_Dt_Cuentas_Contables
        {
            get { return Dt_Cuentas_Contables; }
            set { Dt_Cuentas_Contables = value; }
        }

        public String P_Cuenta {
            get { return Cuenta; }
            set { Cuenta = value; }
        }
        #endregion

        #region (Metodos)
        public Boolean Alta_Individual() { return Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos.Alta_Individual(this); }
        public Boolean Delete() { return Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos.Delete(this); }        
        public DataTable Consultar_Cuentas_Contables() { return Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos.Consultar_Cuentas_Contables(); }
        public DataTable Consultar_Conceptos() { return Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos.Consultar_Conceptos(); }
        public DataTable Consultar_Cuentas_Contables_X_Concepto() { return Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos.Consultar_Cuentas_Contables_X_Concepto(this); }
        public DataTable Consultar_Cuenta_Contable_X_Cuenta() { return Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos.Consultar_Cuenta_Contable_X_Cuenta(this); }
        public DataTable Consultar_Conceptos_X_Clave() { return Cls_Cat_Nom_Perc_Dedu_CC_Deta_Datos.Consultar_Conceptos_X_Clave(this); }
        #endregion
    }
}
