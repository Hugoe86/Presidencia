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
using Presidencia.Calendario_Reloj_Checador.Datos;

namespace Presidencia.Calendario_Reloj_Checador.Negocio
{
    public class Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio
    {
        public Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio()
        {
        }
        #region (Variables Internas)
            private Int16 Anio;
            private String Nomina_ID;
            private Int32 No_Nomina;
            private String Nombre_Usuario;
            private DataTable Dt_Calendario_Reloj;
        #endregion
        #region (Variables Publicas)
            public Int16 P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
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
            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
            public DataTable P_Dt_Calendario_Reloj
            {
                get { return Dt_Calendario_Reloj; }
                set { Dt_Calendario_Reloj = value; }
            }
        #endregion
        #region (Metodos)
            public void Alta_Modificacion_Calendario_Reloj_Checador()
            {
                Cls_Cat_Nom_Calendario_Reloj_Checador_Datos.Alta_Modificacion_Calendario_Reloj_Checador(this);
            }
            public DataTable Consulta_Datos_Calendario_Nominal()
            {
                return Cls_Cat_Nom_Calendario_Reloj_Checador_Datos.Consulta_Datos_Calendario_Nominal(this);
            }
            public DataTable Consulta_Calenadario_Reloj_Checador()
            {
                return Cls_Cat_Nom_Calendario_Reloj_Checador_Datos.Consulta_Calenadario_Reloj_Checador(this);
            }            
            public DataTable Consulta_Fechas_Calendario_Reloj_Checador()
            {
                return Cls_Cat_Nom_Calendario_Reloj_Checador_Datos.Consulta_Fechas_Calendario_Reloj_Checador(this);
            }
        #endregion
    }
}