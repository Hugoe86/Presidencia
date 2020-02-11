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
using Presidencia.Cuadro_Comparativo.Negocio;
using Presidencia.Cuadro_Comparativo.Datos;
/// <summary>
/// Summary description for Cls_Ope_Com_Cuadro_Comparativo_Negocio
/// </summary>
/// 
namespace Presidencia.Cuadro_Comparativo.Negocio
{
    public class Cls_Ope_Com_Cuadro_Comparativo_Negocio
    {
        #region VARIABLES INTERNAS
        private String No_Requisicion;
        private String Proveedor_ID;
        private String Tipo_Articulo;
        #endregion

        #region VARIABLES PUBLICAS
        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        #endregion

        #region VARIABLES METODOS
        public DataTable Consultar_Proveedores_Que_Cotizaron()
        {
            return Cls_Ope_Com_Cuadro_Comparativo_Datos.Consultar_Proveedores_Que_Cotizaron(this);
        }        
        public DataTable Consultar_Precios_Cotizados()
        {
            return Cls_Ope_Com_Cuadro_Comparativo_Datos.Consultar_Precios_Cotizados(this);
        }
        public DataTable Consultar_Productos_Requisicion()
        {
            return Cls_Ope_Com_Cuadro_Comparativo_Datos.Consultar_Productos_Requisicion(this);
        }
        public DataTable Consultar_Requisicion()
        {
            return Cls_Ope_Com_Cuadro_Comparativo_Datos.Consultar_Requisicion(this);
        }
        public String Consultar_Dias_Credito_De_Proveedor(String Proveedor_ID)
        {
            return Cls_Ope_Com_Cuadro_Comparativo_Datos.Consultar_Dias_Credito_De_Proveedor(Proveedor_ID);
        }
                   
        #endregion
    }
}