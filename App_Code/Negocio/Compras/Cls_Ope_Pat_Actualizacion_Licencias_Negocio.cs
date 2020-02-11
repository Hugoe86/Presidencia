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
using Presidencia.Control_Patrimonial_Actualizacion_Licencias.Datos;

/// <summary>
/// Summary description for Cls_Ope_Pat_Actualizacion_Licencias_Negocio
/// </summary>

namespace Presidencia.Control_Patrimonial_Actualizacion_Licencias.Negocio {

    public class Cls_Ope_Pat_Actualizacion_Licencias_Negocio {

        #region "Variables Internas"

            private String Busqueda_RFC = null;
            private String Busqueda_No_Empleado = null;
            private String Busqueda_Dependencia_ID = null;
            private String Busqueda_Empleado_ID = null;
            private String Busqueda_Empleado_Nombre = null;
            private String No_Licencia = null;
            private String Tipo_Licencia = null;
            private DateTime Fecha_Vencimiento;
            private String Usuario_Nombre = null;

        #endregion "Variables Internas"

        #region "Variables Publicas"

            public String P_Busqueda_RFC {
                get { return Busqueda_RFC; }
                set { Busqueda_RFC = value; }
            }
            public String P_Busqueda_No_Empleado {
                get { return Busqueda_No_Empleado; }
                set { Busqueda_No_Empleado = value; }
            }
            public String P_Busqueda_Dependencia_ID {
                get { return Busqueda_Dependencia_ID; }
                set { Busqueda_Dependencia_ID = value; }
            }
            public String P_Busqueda_Empleado_ID {
                get { return Busqueda_Empleado_ID; }
                set { Busqueda_Empleado_ID = value; }
            }
            public String P_Busqueda_Empleado_Nombre
            {
                get { return Busqueda_Empleado_Nombre; }
                set { Busqueda_Empleado_Nombre = value; }
            }
            public String P_No_Licencia
            {
                get { return No_Licencia; }
                set { No_Licencia = value; }
            }
            public DateTime P_Fecha_Vencimiento
            {
                get { return Fecha_Vencimiento; }
                set { Fecha_Vencimiento = value; }
            }
            public String P_Usuario_Nombre
            {
                get { return Usuario_Nombre; }
                set { Usuario_Nombre = value; }
            }
            public String P_Tipo_Licencia
            {
                get { return Tipo_Licencia; }
                set { Tipo_Licencia = value; }
            }

        #endregion "Variables Publicas"

        #region "Metodos"

            public DataTable Consultar_Empleados() {
                return Cls_Ope_Pat_Actualizacion_Licencias_Datos.Consultar_Empleados(this);
            }
            public DataTable Consultar_Dependecias() {
                return Cls_Ope_Pat_Actualizacion_Licencias_Datos.Consultar_Dependencias();
            }
            public void Modificar_Datos_Licencia() {
                Cls_Ope_Pat_Actualizacion_Licencias_Datos.Modificar_Datos_Licencia(this);
            }

        #endregion "Metodos"

    }

}
