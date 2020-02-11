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
using Presidencia.Cajas_Empleados.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Cajas_Empleados_Negocios
/// </summary>
namespace Presidencia.Cajas_Empleados.Negocios
{
    public class Cls_Cat_Pre_Cajas_Empleados_Negocios
    {
        public Cls_Cat_Pre_Cajas_Empleados_Negocios()
        {
        }
        #region (Variables Internas)
            private String Empleado_ID;
            private String Caja_ID;
            private String Modulo_ID;
        #endregion
        #region (Variables Publicas)
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Caja_ID
            {
                get { return Caja_ID; }
                set { Caja_ID = value; }
            }
            public String P_Modulo_ID
            {
                get { return Modulo_ID; }
                set { Modulo_ID = value; }
            }
        #endregion
        #region (Metodos)
            public void Alta_Caja_Empleado()
            {
                Cls_Cat_Pre_Cajas_Empleados_Datos.Alta_Caja_Empleado(this);
            }
            public void Modificar_Caja_Empleado()
            {
                Cls_Cat_Pre_Cajas_Empleados_Datos.Modificar_Caja_Empleado(this);
            }
            public DataTable Consulta_Modulos_Cajas()
            {
                return Cls_Cat_Pre_Cajas_Empleados_Datos.Consulta_Modulos_Cajas();
            }
            public DataTable Consulta_Cajas_Modulo()
            {
                return Cls_Cat_Pre_Cajas_Empleados_Datos.Consulta_Cajas_Modulo(this);
            }
            public DataTable Consulta_Caja_Empleado()
            {
                return Cls_Cat_Pre_Cajas_Empleados_Datos.Consulta_Caja_Empleado(this);
            }
        #endregion
    }
}