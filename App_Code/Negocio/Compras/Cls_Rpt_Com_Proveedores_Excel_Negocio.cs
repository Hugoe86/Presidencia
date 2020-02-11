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
using Presidencia.Compras_Reporte_Proveedores_Excel.Datos;

namespace Presidencia.Compras_Reporte_Proveedores_Excel.Negocio
{
    public class Cls_Rpt_Com_Proveedores_Excel_Negocio
    {
        #region Variables Privadas
        private String Partida_Generica_ID;
        private String Proveedor_ID;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Estatus;
        private String No_Requisicion;
        #endregion

        #region Variables Publicas
        public String P_Partida_Generica_ID
        {
            get { return Partida_Generica_ID; }
            set { Partida_Generica_ID = value; }
        }
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
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
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        #endregion

        #region Metodos
        public DataTable Consultar_Datos_Proveedor()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Datos_Proveedor(this);
        }
        public DataTable Consultar_Ventas_Proveedor()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Ventas_Proveedor(this);
        }
        public DataTable Consultar_Ventas_Realizadas()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Ventas_Realizadas(this);
        }
        public DataTable Consultar_Suma_Ventas_Realizadas()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Suma_Ventas_Realizadas(this);
        }
        public DataTable Consultar_Tipos_Articulos_Vendidos()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Tipos_Articulos_Vendidos(this);
        }
        public DataTable Consultar_Nombre_Articulos_Vendidos()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Nombre_Articulos_Vendidos(this);
        }
        public DataTable Consultar_Fecha_Registro()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Fecha_Registro(this);
        }
        public DataTable Consultar_Fecha_Actualizacion()
        {
            return Cls_Rpt_Com_Proveedores_Excel_Datos.Consultar_Fecha_Actualizacion(this);
        }
        #endregion
    }
}
