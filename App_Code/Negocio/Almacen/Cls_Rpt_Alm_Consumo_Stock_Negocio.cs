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
using Presidencia.Consumo_Stock.Datos;

namespace Presidencia.Consumo_Stock.Negocio
{
    public class Cls_Rpt_Alm_Consumo_Stock_Negocio
    {
        #region (Variables)
        public string P_Dependencia_ID { get; set; }
        public string P_Producto_ID { get; set; }
        public string P_Partida_ID { get; set; }
        public string P_Fecha_Inicio { get; set; }
        public string P_Fecha_Fin { get; set; }
        #endregion

        #region (Metodos)
        public DataTable Consultar_Departamentos() { return Cls_Rpt_Alm_Consumo_Stock_Datos.Consultar_Departamentos(this); }
        public DataTable Consultar_Productos() { return Cls_Rpt_Alm_Consumo_Stock_Datos.Consultar_Productos(this); }
        public DataTable Consultar_Partidas_Presupuestales() { return Cls_Rpt_Alm_Consumo_Stock_Datos.Consultar_Partidas_Presupuestales(this); }
        public DataTable Consultar_Consumo_Stock() { return Cls_Rpt_Alm_Consumo_Stock_Datos.Consultar_Consumo_Stock(this); }
        #endregion
    }
}
