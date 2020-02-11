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
using Presidencia.Reporte_Credito_Fonacot.Datos;

namespace Presidencia.Reporte_Credito_Fonacot.Negocio
{
    public class Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio
    {
        public Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio()
        {
        }
        #region Variables Internas
            private String Empleado_ID;
            private String No_Credito;
            private String Folio_Fonacot;
            private String Usuario_Creo;
            private String Nomina;
            private String Periodo;
        #endregion
        #region Variables Publicas
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_No_Credito
            {
                get { return No_Credito; }
                set { No_Credito = value; }
            }
            public String P_Folio_Fonacot
            {
                get { return Folio_Fonacot; }
                set { Folio_Fonacot = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public String P_Nomina
            {
                get { return Nomina; }
                set { Nomina = value; }
            }
            public String P_Periodo
            {
                get { return Periodo; }
                set { Periodo = value; }
            }
        #endregion
        #region Metodos
            public DataTable Consulta_CreditoS_Fonacot_Empleado()
            {
                return Cls_Rpt_Nom_Reporte_Credito_Fonacot_Datos.Consulta_CreditoS_Fonacot_Empleado(this);
            }
            public DataTable Consulta_Credito_Fonacot()
            {
                return Cls_Rpt_Nom_Reporte_Credito_Fonacot_Datos.Consulta_Credito_Fonacot(this);
            }
        
            
        #endregion
    }
}