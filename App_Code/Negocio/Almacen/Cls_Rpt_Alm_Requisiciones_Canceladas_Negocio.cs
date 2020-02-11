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
using Presidencia.Almacen_Reporte_Requisiciones_Canceladas.Datos;

namespace Presidencia.Almacen_Reporte_Requisiciones_Canceladas.Negocio
{
    public class Cls_Rpt_Alm_Requisiciones_Canceladas_Negocio
    {
        #region(Variables Privadas)
        String Fecha_Inicial;
        String Fecha_Final;
        String No_Requisicion;
        #endregion

        #region(Variables Publicas)
        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }

        #endregion

        #region(Metodos)
        public DataTable Consultar_Requisiciones_Canceladas()
        {
            return Cls_Rpt_Alm_Requisiciones_Canceladas_Datos.Consultar_Requisiciones_Canceladas(this);
        }
        public DataTable Consultar_Motivo_Canceladas()
        {
            return Cls_Rpt_Alm_Requisiciones_Canceladas_Datos.Consultar_Motivo_Canceladas(this);
        }
        #endregion
    }
}
