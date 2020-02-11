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
using System.Collections.Generic;
using Presidencia.Generacion_Asistencias.Datos;

namespace Presidencia.Generacion_Asistencias.Negocio
{
    public class Cls_Ope_Nom_Generacion_Asistencias_Empleados_Negocio
    {
        #region (Variables Internas)
            //Propiedades
            private DataTable Dt_Lista_Asistencia;
            private String Empleado_ID;
            private String Reloj_Checador_ID;
            private DateTime Fecha_Hora_Entrada;
            private DateTime Fecha_Hora_Salida;
            private String Nombre_Usuario;
            private DataTable Dt_Empleados;
        #endregion
        #region (Variables Publicas)
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Reloj_Checador_ID
            {
                get { return Reloj_Checador_ID; }
                set { Reloj_Checador_ID = value; }
            }
            public DateTime P_Fecha_Hora_Entrada
            {
                get { return Fecha_Hora_Entrada; }
                set { Fecha_Hora_Entrada = value; }
            }
            public DateTime P_Fecha_Hora_Salida
            {
                get { return Fecha_Hora_Salida; }
                set { Fecha_Hora_Salida = value; }
            }
            public DataTable P_Dt_Lista_Asistencia
            {
                get { return Dt_Lista_Asistencia; }
                set { Dt_Lista_Asistencia = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }

            public DataTable P_Dt_Empleados {
                get { return Dt_Empleados; }
                set { Dt_Empleados = value; }
            }
        #endregion
        #region (Metodos)
            public DataTable Consulta_Lista_Asistencias_Empleados()
            {
                return Cls_Ope_Nom_Generacion_Asistencias_Empleados_Datos.Consulta_Lista_Asistencias_Empleados(this);
            }
            public void Alta_Asistencias()
            {
                Cls_Ope_Nom_Generacion_Asistencias_Empleados_Datos.Alta_Asistencias(this);
            }

            public DataTable queryListAsistencia() { return Cls_Ope_Nom_Generacion_Asistencias_Empleados_Datos.queryListAsistencia(this); }
        #endregion
    }
}
