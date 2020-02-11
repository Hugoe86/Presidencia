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
using Presidencia.Asistencias.Datos;

namespace Presidencia.Asistencias.Negocio
{
    public class Cls_Ope_Nom_Asistencias_Negocio
    {
        #region (Variables Internas)
            //Propiedades
            private String No_Asistencia;
            private String Empleado_ID;
            private String Reloj_Checador_ID;
            private DateTime Fecha_Hora_Entrada;
            private DateTime Fecha_Hora_Salida;
            private String Fecha_Inicio;
            private String Fecha_Termino;            
            private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
            public String P_No_Asistencia
            {
                get { return No_Asistencia; }
                set { No_Asistencia = value; }
            }
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
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
            public String P_Fecha_Inicio
            {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }
            public String P_Fecha_Termino
            {
                get { return Fecha_Termino; }
                set { Fecha_Termino = value; }
            }
        #endregion
        #region (Metodos)
            public void Alta_Asistencia()
            {
                Cls_Ope_Nom_Asistencias_Datos.Alta_Asistencia(this);
            }
            public void Modificar_Asistencia()
            {
                Cls_Ope_Nom_Asistencias_Datos.Modificar_Asistencia(this);
            }
            public void Eliminar_Asistencia()
            {
                Cls_Ope_Nom_Asistencias_Datos.Eliminar_Asistencia(this);
            }
            public DataTable Consulta_Asistencia()
            {
                return Cls_Ope_Nom_Asistencias_Datos.Consulta_Asistencia(this);
            }
            public DataTable Consulta_Datos_Empleado()
            {
                return Cls_Ope_Nom_Asistencias_Datos.Consulta_Datos_Empleado(this);
            }
        #endregion
    }
}