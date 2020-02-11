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
using Presidencia.Reporte_Presupuesto_Egresos.Datos;

namespace Presidencia.Reporte_Presupuesto_Egresos.Negocio
{
    public class Cls_Rpt_Presupuesto_Egresos_Negocio
    {
        #region(Variables Privadas)
            String Grupo_Dependencia_ID;
            String Dependencia_ID;
            String Programa_ID;
            String Partida_ID;
            String Anio;
        #endregion

        #region(Variables Publicas)
            public String P_Grupo_Dependencia_ID
            {
                get { return Grupo_Dependencia_ID; }
                set { Grupo_Dependencia_ID = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Programa_ID
            {
                get { return Programa_ID; }
                set { Programa_ID = value; }
            }
            public String P_Partida_ID
            {
                get { return Partida_ID; }
                set { Partida_ID = value; }
            }
            public String P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            }
        #endregion

        #region(Metodos)
            public DataTable Consultar_Dependencias()
            {
                return Cls_Rpt_Presupuesto_Egresos_Datos.Consultar_Dependencias(this);
            }
            public DataTable Consultar_Presupuesto_Dependencias()
            {
                return Cls_Rpt_Presupuesto_Egresos_Datos.Consultar_Presupuesto_Dependencias(this);
            }
            public DataTable Consultar_Presupuesto_Programa()
            {
                return Cls_Rpt_Presupuesto_Egresos_Datos.Consultar_Presupuesto_Programa(this);
            }
            public DataTable Consultar_Presupuesto_Partida()
            {
                return Cls_Rpt_Presupuesto_Egresos_Datos.Consultar_Presupuesto_Partida(this);
            }
            public DataTable Consultar_Presupuesto_Año()
            {
                return Cls_Rpt_Presupuesto_Egresos_Datos.Consultar_Presupuesto_Año(this);
            }
        
        #endregion
    }
}
