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
using Presidencia.Generar_Faltas_Retardos_Empleados.Datos;

namespace Presidencia.Generar_Faltas_Retardos_Empleados.Negocio
{
    public class Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Negocio
    {
        #region (Variables Internas)
            //Propiedades
            private DataTable Dt_Lista_Faltas_Retardos;
            private DateTime Fecha_Inicio;
            private DateTime Fecha_Termino;
            private Int32 No_Nomina;
            private String Nomina_ID;
            private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
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
            public DataTable P_Dt_Lista_Faltas_Retardos
            {
                get { return Dt_Lista_Faltas_Retardos; }
                set { Dt_Lista_Faltas_Retardos = value; }
            }
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
        #endregion
        #region (Metodos)
            public DataTable Consulta_Lista_Faltas_Retardos()
            {
                return Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Datos.Consulta_Lista_Faltas_Retardos(this);
            }
            public void Alta_Faltas_Retardos()
            {
                Cls_Ope_Nom_Generar_Faltas_Retardos_Empleados_Datos.Alta_Faltas_Retardos(this);
            }
        #endregion
    }	
}
