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
using Presidencia.Control_Patrimonial_Reportes_Basicos.Negocio;
using Presidencia.Constantes;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Reportes_Basicos_Datos
/// </summary>

namespace Presidencia.Control_Patrimonial_Reportes_Basicos.Datos {

    public class Cls_Rpt_Pat_Reportes_Basicos_Datos {

        #region Metodos 

            public static DataSet Consultar_Datos_Reporte_Bienes_Muebles(Cls_Rpt_Pat_Reportes_Basicos_Negocio Parametros) {
                DataSet Ds_Reporte_Bienes_Muebles = null;
                try {
                    String Mi_SQL = "SELECT " + Ope_Pat_Bienes_Muebles.Tabla_Ope_Pat_Bienes_Muebles + "." + Ope_Pat_Bienes_Muebles.Campo_Bien_Mueble_ID;
                    Mi_SQL = Mi_SQL + ", " + Cat_Com_Productos.Tabla_Cat_Com_Productos + "." + Cat_Com_Productos.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Dependencias.Tabla_Cat_Dependencias + "." + Cat_Dependencias.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Areas.Tabla_Cat_Areas + "." + Cat_Areas.Campo_Nombre;
                    Mi_SQL = Mi_SQL + ", " + Cat_Pat_Materiales.Tabla_Cat_Pat_Materiales + "." + Cat_Pat_Materiales.Campo_Descripcion;
                }catch(Exception Ex){
                    String Mensaje = "Error al intentar consultar los datos. Error [" + Ex.Message + "]";
                    throw new Exception(Mensaje);
                }
                return Ds_Reporte_Bienes_Muebles;
            }

        #endregion

    }

}