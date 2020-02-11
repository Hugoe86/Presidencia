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
using Presidencia.Reporte_Cat_Catastrales.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Cat_Catastrales_Negocio
/// </summary>
/// 
namespace Presidencia.Reporte_Cat_Catastrales.Negocio
{
    public class Cls_Rpt_Cat_Catastrales_Negocio
    {
        #region Variables
        
            public String P_Fecha_Inicio { get; set; }
            public String P_Fecha_Fin { get; set; }
            public String P_Tipo_Consulta { get; set; }

        #endregion

        #region Metodos

            public DataTable Consultar_Asignacion_Modificacion_Claves()
            {
                return Cls_Rpt_Cat_Catastrales_Datos.Consultar_Asignacion_Modificacion_Claves(this);
            }

            public DataTable Consultar_Areas_Privativas()
            {
                return Cls_Rpt_Cat_Catastrales_Datos.Consultar_Areas_Privativas(this);
            }

        #endregion


    }
}
