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
using Presidencia.Reporte_Pago_Cajas_Diario.Datos;

namespace Presidencia.Reporte_Pago_Cajas_Diario.Negocio
{
    public class Cls_Rpt_Caj_Pagos_Negocio
    {
        #region (Variables Privadas)
        private String Clave_Rama;
        private String Clave_Grupo;
        private String Clave_Ingreso;
        private String Fecha_Inicio_Busqueda;
        private String Fecha_Fin_Busqueda;
        #endregion

        #region (Variables Públicas)
        public String P_Fecha_Inicio_Busqueda {
            get { return Fecha_Inicio_Busqueda; }
            set { Fecha_Inicio_Busqueda = value; }
        }

        public String P_Fecha_Fin_Busqueda
        {
            get { return Fecha_Fin_Busqueda; }
            set { Fecha_Fin_Busqueda = value; }
        }

        public String P_Clave_Rama
        {
            get { return Clave_Rama; }
            set { Clave_Rama = value; }
        }

        public String P_Clave_Grupo
        {
            get { return Clave_Grupo; }
            set { Clave_Grupo = value; }
        }

        public String P_Clave_Ingreso
        {
            get { return Clave_Ingreso; }
            set { Clave_Ingreso = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Consultar_Rama()
        {
            return Cls_Rpt_Caj_Pagos_Datos.Consultar_Rama(this);
        }

        public DataTable Consultar_Grupos()
        {
            return Cls_Rpt_Caj_Pagos_Datos.Consultar_Grupos(this);
        }

        public DataTable Consultar_Ingresos()
        {
            return Cls_Rpt_Caj_Pagos_Datos.Consultar_Ingresos(this);
        }

        public DataSet Rpt_Caj_Ingresos(String Dependencia_ID, String Turno_ID, String Caja_ID) {
            return Cls_Rpt_Caj_Pagos_Datos.Rpt_Caj_Ingresos(Dependencia_ID, Turno_ID, Caja_ID);
        }
        #endregion
    }
}
