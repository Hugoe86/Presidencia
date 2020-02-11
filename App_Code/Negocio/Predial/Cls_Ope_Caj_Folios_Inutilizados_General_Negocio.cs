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
using Presidencia;
using Presidencia.Folios_Inutilizados_General_Datos;

namespace Presidencia.Folios_Inutilizados_General_Negocio {
    public class Cls_Ope_Caj_Folios_Inutilizados_General_Negocio
    {
       #region Variables Internas
            private String Modulo_ID;
            private String Caja_ID;
            private String Fecha_Inicio;
            private String Fecha_Fin;
            private String Empleado_ID;
       #endregion

       #region Variables Publicas
            public String P_Modulo_ID
            {
                get { return Modulo_ID; }
                set { Modulo_ID = value; }
            }
            public String P_Empleado_ID
            {
                get { return Empleado_ID; }
                set { Empleado_ID = value; }
            }
            public String P_Fecha_Inicio
            {
                get { return Fecha_Inicio ; }
                set { Fecha_Inicio = value; }
            }
            public String P_Fecha_Fin
            {
                get { return Fecha_Fin; }
                set { Fecha_Fin = value; }
            }
            public String P_Caja_ID
        {
            get { return Caja_ID; }
            set { Caja_ID = value; }
        }
        #endregion

       #region Metodos
        public DataTable Consultar_Modulo()
        {
            return Cls_Ope_Caj_Folios_Inutilizados_General_Datos.Consultar_Modulos();
        }

        public DataTable Consultar_Caja()
        {
            return Cls_Ope_Caj_Folios_Inutilizados_General_Datos.Consultar_Cajas(this);
        }

        public DataTable Consultar_Folio()
        {
            return Cls_Ope_Caj_Folios_Inutilizados_General_Datos.Consultar_Folios(this);
        }
       #endregion
    }
}


