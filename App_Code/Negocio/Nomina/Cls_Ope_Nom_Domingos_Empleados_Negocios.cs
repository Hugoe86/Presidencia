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
using Presidencia.Domingos_Trabajados.Datos;

namespace Presidencia.Domingos_Trabajados.Negocios
{
    public class Cls_Ope_Nom_Domingos_Empleados_Negocios
    {
        #region (Variables Internas)
            private String No_Domingo;
            private String Dependencia_ID;
            private DateTime Fecha;
            private String Estatus;
            private DateTime Fecha_Inicio;
            private DateTime Fecha_Final;            
            private String Comentarios;
            private String Nombre_Usuario;
            private DataTable Detalles_Domingos_Empleados;
            private String Empleado_ID;
            private String Nomina_ID;
            private Int32 No_Nomina;
        #endregion

        #region (Variables Publicas)
            public String P_No_Domingo
            {
                get { return No_Domingo; }
                set { No_Domingo = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public DateTime P_Fecha
            {
                get { return Fecha; }
                set { Fecha = value; }
            }
            public DateTime P_Fecha_Inicio
            {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }
            public DateTime P_Fecha_Final
            {
                get { return Fecha_Final; }
                set { Fecha_Final = value; }
            }
            public String P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
            public DataTable P_Detalles_Domingos_Empleados
            {
                get { return Detalles_Domingos_Empleados; }
                set { Detalles_Domingos_Empleados = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Nomina_ID
            {
                get { return Nomina_ID; }
                set { Nomina_ID = value; }
            }
            public Int32 P_No_Nomina
            {
                get { return No_Nomina; }
                set { No_Nomina = value; }
            }
        #endregion

        #region (Metodos)
            public void Alta_Domingo_Trabajado()
            {
                Cls_Ope_Nom_Domingos_Empleados_Datos.Alta_Domingo_Trabajado(this);
            }
            public void Modificar_Domingo_Trabajado()
            {
                Cls_Ope_Nom_Domingos_Empleados_Datos.Modificar_Domingo_Trabajado(this);
            }
            public void Eliminar_Domingo_Trabajado()
            {
                Cls_Ope_Nom_Domingos_Empleados_Datos.Eliminar_Domingo_Trabajado(this);
            }
            public DataTable Consulta_Datos_Domingos_Trabajados()
            {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Consulta_Datos_Domingos_Trabajados(this);
            }
            public DataTable Consulta_Empleados_Domingo_Trabajado()
            {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Consulta_Empleados_Domingo_Trabajado(this);
            }
            public DataTable Consulta_Domingos_Trabajados()
            {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Consulta_Domingos_Trabajados(this);
            }
            public DataTable Consulta_Domingos_Empleado()
            {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Consulta_Domingos_Empleado(this);
            }

            public DataTable Consultar_Domingos() {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Consultar_Domingos(this);
            }

            public Boolean Cambiar_Estatus_Domingo() {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Cambiar_Estatus_Domingo(this);
            }

            public DataTable Consultar_Informacion_Empleado_Incidencias(String Empleado_ID) {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Consultar_Informacion_Empleado_Incidencias(Empleado_ID);
            }

            public DataTable Consultar_Reporte_Prima_Dominical()
            {
                return Cls_Ope_Nom_Domingos_Empleados_Datos.Consultar_Reporte_Prima_Dominical(this);
            }

        #endregion

    }
}
