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
using Presidencia.Reportes_Nomina.Reporteador_Empleados.Datos;

/// <summary>
/// Summary description for Cls__Rpt_Nom_Reporteador_Empleados_Negocio
/// </summary>

namespace Presidencia.Reportes_Nomina.Reporteador_Empleados.Negocio{
    
    public class Cls_Rpt_Nom_Reporteador_Empleados_Negocio {

        #region "Variables Internas"
            private String Tipo_Nomina_ID = String.Empty;
            private String Dependencia_ID = String.Empty;
            private String No_Empleado = String.Empty;
            private String RFC_Empleado = String.Empty;
            private String Nombre_Empleado = String.Empty;
            private String Tipo_Movimiento = String.Empty;
            private String Empleado_ID = String.Empty;
            private String Motivo_Movimiento = String.Empty;
            private String Tipo_Reporte = String.Empty;
            private DateTime Fecha_Inicial = new DateTime();
            private DateTime Fecha_Final = new DateTime();
        #endregion

        #region "Variables Publicas"
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Tipo_Movimiento
            {
                get { return Tipo_Movimiento; }
                set { Tipo_Movimiento = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Tipo_Reporte
            {
                get { return Tipo_Reporte; }
                set { Tipo_Reporte = value; }
            }
            public String P_Motivo_Movimiento
            {
                get { return Motivo_Movimiento; }
                set { Motivo_Movimiento = value; }
            }
            public String P_Tipo_Nomina_ID
            {
                get { return Tipo_Nomina_ID; }
                set { Tipo_Nomina_ID = value; }
            }
            public String P_No_Empleado
            {
                get { return No_Empleado; }
                set { No_Empleado = value; }
            }
            public String P_RFC_Empleado
            {
                get { return RFC_Empleado; }
                set { RFC_Empleado = value; }
            }
            public String P_Nombre_Empleado
            {
                get { return Nombre_Empleado; }
                set { Nombre_Empleado = value; }
            }
            public DateTime P_Fecha_Inicial
            {
                get { return Fecha_Inicial; }
                set { Fecha_Inicial = value; }
            }
            public DateTime P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        #endregion

        #region "Metodos"
        public DataTable Consultar_Datos_Empleado() {
            return Cls_Rpt_Nom_Reporteador_Empleados_Datos.Consultar_Datos_Empleado(this);
        }
        public DataTable Consultar_Ultimo_Movimiento_Empleado() {
            return Cls_Rpt_Nom_Reporteador_Empleados_Datos.Consultar_Ultimo_Movimiento_Empleado(this);
        }
        public static DataTable Consultar_Dependencias_Activas() {
            return Cls_Rpt_Nom_Reporteador_Empleados_Datos.Consultar_Dependencias_Activas();
        }
        public static DataTable Consultar_Tipos_Nomina() {
            return Cls_Rpt_Nom_Reporteador_Empleados_Datos.Consultar_Tipos_Nomina();
        }
        #endregion

    }

}