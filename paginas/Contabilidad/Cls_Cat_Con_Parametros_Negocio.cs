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
using Presidencia.Parametros_Contabilidad.Datos;

namespace Presidencia.Parametros_Contabilidad.Negocio
{
    public class Cls_Cat_Con_Parametros_Negocio
    {
        #region (Variables_Internas)
            private string Parametro_Contabilidad_ID;
            private string Mascara_Cuenta_Contable;
            private string Mes_Contable;
            private string Usuario_Creo;
            private string Fecha_Creo;
            private string Usuario_Modifico;
            private string Fecha_Modifico;
        #endregion

        #region (Variables_Publicas)
            public string P_Mes_Contable
            {
                get { return Mes_Contable; }
                set { Mes_Contable = value; }
            }
            public string P_Parametro_Contabilidad_ID
            {
                get { return Parametro_Contabilidad_ID; }
                set { Parametro_Contabilidad_ID = value; }
            }
            public string P_Mascara_Cuenta_Contable
            {
                get { return Mascara_Cuenta_Contable; }
                set { Mascara_Cuenta_Contable = value; }
            }
            public string P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public string P_Fecha_Creo
            {
                get { return Fecha_Creo; }
                set { Fecha_Creo = value; }
            }
            public string P_Usuario_Modifico
            {
                get { return Usuario_Modifico; }
                set { Usuario_Modifico = value; }
            }
            public string P_Fecha_Modifico
            {
                get { return Fecha_Modifico; }
                set { Fecha_Modifico = value; }
            }
        #endregion

        #region (Metodos)
            public void Alta_Parametros()
            {
                Cls_Cat_Con_Parametros_Datos.Alta_Parametros(this);
            }
            public DataTable Consulta_Parametros()
            {
                return Cls_Cat_Con_Parametros_Datos.Consulta_Parametros();
            }
            public void Modificar_Parametros()
            {
                Cls_Cat_Con_Parametros_Datos.Modificar_Parametros(this);
            }
            public DataTable Consulta_Datos_Parametros()
            {
                return Cls_Cat_Con_Parametros_Datos.Consulta_Datos_Parametros();
            }
        #endregion
    }
}
