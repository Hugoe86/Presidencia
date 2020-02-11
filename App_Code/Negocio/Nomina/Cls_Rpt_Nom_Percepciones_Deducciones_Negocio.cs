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
using Presidencia.Reportes_Nomina.Percepciones_Deducciones.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Nom_Percepciones_Deducciones_Negocio
/// </summary>

namespace Presidencia.Reportes_Nomina.Percepciones_Deducciones.Negocio {

    public class Cls_Rpt_Nom_Percepciones_Deducciones_Negocio {

        #region "Variables Internas"
            private String Empleado_ID = String.Empty;
            private String No_Empleado = String.Empty;
            private String Nombre_Empleado = String.Empty;
            private String RFC_Empleado = String.Empty;
            private String Dependencia_ID = String.Empty;
            private String Tipo_Nomina_ID = String.Empty;
            private String Nomina_ID = String.Empty;
            private String No_Nomina = String.Empty;
            private String Tipo_Percepcion_Deduccion = String.Empty;
            private String Percepcion_Deduccion_ID = String.Empty;
            private String Percepcion_Deduccion_Clave = String.Empty;
        #endregion

        #region "Variables Publicas"
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_No_Empleado
            {
                get { return No_Empleado; }
                set { No_Empleado = value; }
            }
            public String P_Nombre_Empleado
            {
                get { return Nombre_Empleado; }
                set { Nombre_Empleado = value; }
            }
            public String P_RFC_Empleado
            {
                get { return RFC_Empleado; }
                set { RFC_Empleado = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Tipo_Nomina_ID
            {
                get { return Tipo_Nomina_ID; }
                set { Tipo_Nomina_ID = value; }
            }
            public String P_Nomina_ID
            {
                get { return Nomina_ID; }
                set { Nomina_ID = value; }
            }
            public String P_No_Nomina
            {
                get { return No_Nomina; }
                set { No_Nomina = value; }
            }
            public String P_Tipo_Percepcion_Deduccion
            {
                get { return Tipo_Percepcion_Deduccion; }
                set { Tipo_Percepcion_Deduccion = value; }
            }
            public String P_Percepcion_Deduccion_ID
            {
                get { return Percepcion_Deduccion_ID; }
                set { Percepcion_Deduccion_ID = value; }
            }
            public String P_Percepcion_Deduccion_Clave
            {
                get { return Percepcion_Deduccion_Clave; }
                set { Percepcion_Deduccion_Clave = value; }
            }
        #endregion

        #region "Metodos"
            public DataTable Consultar_Datos_Percepciones_Deducciones() {
                return Cls_Rpt_Nom_Percepciones_Deducciones_Datos.Consultar_Datos_Percepciones_Deducciones(this);
            }
            public Cls_Rpt_Nom_Percepciones_Deducciones_Negocio Consultar_PD_Clave() {
                return Cls_Rpt_Nom_Percepciones_Deducciones_Datos.Consultar_PD_Clave(this);
            }
            public DataTable Consultar_Empleados() {
                return Cls_Rpt_Nom_Percepciones_Deducciones_Datos.Consultar_Empleados(this);
            }
        #endregion

	}

}