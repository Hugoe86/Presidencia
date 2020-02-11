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
using Presidencia.Control_Patrimonial_Reporte_Licencias.Datos;
/// <summary>
/// Summary description for Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Reporte_Licencias.Negocio {

    public class Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio {

        #region "Variables Internas"

            private String Busqueda_RFC = null;
            private String Busqueda_No_Empleado = null;
            private String Busqueda_Dependencia_ID = null;
            private String Busqueda_Empleado_ID = null;
            private DateTime Busqueda_Fecha_Inicial;
            private Boolean Tomar_Fecha_Inicial = false;
            private DateTime Busqueda_Fecha_Final;
            private Boolean Tomar_Fecha_Final = false;
            private String Busqueda_Con_Sin_Licencia = null;

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
            public DateTime P_Busqueda_Fecha_Inicial
            {
                get { return Busqueda_Fecha_Inicial; }
                set { Busqueda_Fecha_Inicial = value; }
            }
            public Boolean P_Tomar_Fecha_Inicial
            {
                get { return Tomar_Fecha_Inicial; }
                set { Tomar_Fecha_Inicial = value; }
            }
            public DateTime P_Busqueda_Fecha_Final
            {
                get { return Busqueda_Fecha_Final; }
                set { Busqueda_Fecha_Final = value; }
            }
            public Boolean P_Tomar_Fecha_Final
            {
                get { return Tomar_Fecha_Final; }
                set { Tomar_Fecha_Final = value; }
            }
            public String P_Busqueda_Con_Sin_Licencia
            {
                get { return Busqueda_Con_Sin_Licencia; }
                set { Busqueda_Con_Sin_Licencia = value; }
            }

        #endregion "Variables Publicas"

        #region "Metodos"

            public DataTable Consultar_Empleados() {
                return Cls_Rpt_Pat_Com_Licencias_Vencidas_Datos.Consultar_Empleados(this);
            }
            public DataTable Consultar_Dependecias() {
                return Cls_Rpt_Pat_Com_Licencias_Vencidas_Datos.Consultar_Dependencias();
            }

        #endregion "Metodos"
    }
}