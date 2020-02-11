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
using Presidencia.Horarios_Empleados.Datos;

namespace Presidencia.Horarios_Empleados.Negocio
{
    public class Cls_Ope_Nom_Horarios_Empleados_Negocio
    {
        #region (Variables Internas)
            //Propiedades
            private String No_Horario_Empleado;
            private String Empleado_ID;
            private DateTime Fecha_Inicio;
            private DateTime Fecha_Termino;
            private DateTime Horario_Entrada;
            private DateTime Horario_Salida;
            private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
            public String P_No_Horario_Empleado
            {
                get { return No_Horario_Empleado; }
                set { No_Horario_Empleado = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public DateTime P_Fecha_Inicio
            {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }
            public DateTime P_Fecha_Termino
            {
                get { return Fecha_Termino; }
                set { Fecha_Termino = value; }
            }
            public DateTime P_Horario_Entrada
            {
                get { return Horario_Entrada; }
                set { Horario_Entrada = value; }
            }
            public DateTime P_Horario_Salida
            {
                get { return Horario_Salida; }
                set { Horario_Salida = value; }
            }
            public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        #endregion
        #region (Metodos)
            public void Alta_Horario_Empleado()
            {
                Cls_Ope_Nom_Horarios_Empleados_Datos.Alta_Horario_Empleado(this);
            }
            public void Modificar_Horario_Empleado()
            {
                Cls_Ope_Nom_Horarios_Empleados_Datos.Modificar_Horario_Empleado(this);
            }
            public void Eliminar_Horario_Empleado()
            {
                Cls_Ope_Nom_Horarios_Empleados_Datos.Eliminar_Horario_Empleado(this);
            }
            public DataTable Consulta_Horarios_Empleados()
            {
                return Cls_Ope_Nom_Horarios_Empleados_Datos.Consulta_Horarios_Empleados(this);
            }
            public DataTable Consulta_Datos_Empleado()
            {
                return Cls_Ope_Nom_Horarios_Empleados_Datos.Consulta_Datos_Empleado(this);
            }
        #endregion
    }
}
