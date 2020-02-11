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
using Presidencia.Nomina_Pago_Indemnizacion.Datos;

namespace Presidencia.Nomina_Pago_Indemnizacion.Negocio
{
    public class Cls_Cat_Nom_Pago_Indemnizacion_Negocio
    {
        #region(Variables Privadas)
        private String Empleado_ID;
        private String Tipo_Finiquito;
        #endregion

        #region(Variables Publicas)
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_Tipo_Finiquito
        {
            get { return Tipo_Finiquito; }
            set { Tipo_Finiquito = value; }
        }
        #endregion

        #region(Metodos)
        public void Modificar_Empleado_Tipo_Finiquito()
        {
            Cls_Cat_Nom_Pago_Indemnizacion_Datos.Modificar_Empleado_Tipo_Finiquito(this);
        }
        #endregion

    }
}