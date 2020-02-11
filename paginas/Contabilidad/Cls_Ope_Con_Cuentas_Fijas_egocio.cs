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
using Presidencia.Cuentas_Contables_Fijas.Datos;

/// <summary>
/// Summary description for Cls_Ope_Con_Cuentas_Fijas_egocio
/// </summary>
/// 
namespace Presidencia.Cuentas_Contables_Fijas.Negocio
{
    public class Cls_Ope_Con_Cuentas_Fijas_Negocio
    {
        private String Cuenta_Almacen_General;
        private String Cuenta_Iva_Acreditable_Compras;

        public String P_Cuenta_Almacen_General
        {
            get { return Cuenta_Almacen_General; }
            set { Cuenta_Almacen_General = value; }
        }
        public String P_Cuenta_Iva_Acreditable_Compras
        {
            get { return Cuenta_Iva_Acreditable_Compras; }
            set { Cuenta_Iva_Acreditable_Compras = value; }
        }

        #region MÉTODOS
        public Cls_Ope_Con_Cuentas_Fijas_Negocio()
        {

        }
        public DataTable Consultar_Cuentas_Contables()
        {
            return Cls_Ope_Con_Cuentas_Fijas_Datos.Consultar_Cuentas_Contables(this);
        }
        public int Guardar_Cuentas_Fijas()
        {
            return Cls_Ope_Con_Cuentas_Fijas_Datos.Guardar_Cuentas_Fijas(this);
        }
        public DataTable Consultar_Cuentas_Fijas()
        {
            return Cls_Ope_Con_Cuentas_Fijas_Datos.Consultar_Cuentas_Fijas(this);
        }
        #endregion
    }
}
