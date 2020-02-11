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
using Presidencia.Imprimir_Propuestas.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio
/// </summary>
namespace Presidencia.Imprimir_Propuestas.Negocio
{
    public class Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String No_Requisicion;
        private DataTable Dt_Productos;
        private String Tipo_Articulo;
        private String Proveedor_ID;
        private String Producto_ID;
        private String IVA_Cotizado;
        private String IEPS_Cotizado;
        private String Total_Cotizado;
        private String Subtotal_Cotizado;
        private String Vigencia_Propuesta;
        private String Fecha_Elaboracion;
        private String Garantia;
        private String Tiempo_Entrega;
        private String Reg_Padron_Proveedor;
        private String Estatus_Propuesta;
        private String Requisicion_Busqueda;
        private String Busqueda_Fecha_Generacion_Ini;
        private String Busqueda_Fecha_Generacion_Fin;
        private String Busqueda_Fecha_Entrega_Ini;
        private String Busqueda_Fecha_Entrega_Fin;
        private String Busqueda_Vigencia_Propuesta_Ini;
        private String Busqueda_Vigencia_Propuesta_Fin;
        private String Busqueda_Proveedor;

        private String Ruta_RPT;
        private String Ruta_Exportacion;
        private String Archivo_PDF;
        #endregion


        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas
        public String P_Archivo_PDF
        {
            get { return Archivo_PDF; }
            set { Archivo_PDF = value; }
        }
        public String P_Ruta_RPT
        {
            get { return Ruta_RPT; }
            set { Ruta_RPT = value; }
        }
        public String P_Ruta_Exportacion
        {
            get { return Ruta_Exportacion; }
            set { Ruta_Exportacion = value; }
        }

        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }

        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }

        public DataTable P_Dt_Productos
        {
            get { return Dt_Productos; }
            set { Dt_Productos = value; }
        }

        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }

        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }

        public String P_Subtotal_Cotizado
        {
            get { return Subtotal_Cotizado; }
            set { Subtotal_Cotizado = value; }
        }
        public String P_Total_Cotizado
        {
            get { return Total_Cotizado; }
            set { Total_Cotizado = value; }
        }
        public String P_IEPS_Cotizado
        {
            get { return IEPS_Cotizado; }
            set { IEPS_Cotizado = value; }
        }
        public String P_IVA_Cotizado
        {
            get { return IVA_Cotizado; }
            set { IVA_Cotizado = value; }
        }

        public String P_Vigencia_Propuesta
        {
            get { return Vigencia_Propuesta; }
            set { Vigencia_Propuesta = value; }
        }

        public String P_Fecha_Elaboracion
        {
            get { return Fecha_Elaboracion; }
            set { Fecha_Elaboracion = value; }
        }

        public String P_Garantia
        {
            get { return Garantia; }
            set { Garantia = value; }
        }

        public String P_Tiempo_Entrega
        {
            get { return Tiempo_Entrega; }
            set { Tiempo_Entrega = value; }
        }

        public String P_Reg_Padron_Proveedor
        {
            get { return Reg_Padron_Proveedor; }
            set { Reg_Padron_Proveedor = value; }
        }

        public String P_Estatus_Propuesta
        {
            get { return Estatus_Propuesta; }
            set { Estatus_Propuesta = value; }
        }

        public String P_Requisicion_Busqueda
        {
            get { return Requisicion_Busqueda; }
            set { Requisicion_Busqueda = value; }
        }

        public String P_Busqueda_Fecha_Generacion_Ini
        {
            get { return Busqueda_Fecha_Generacion_Ini; }
            set { Busqueda_Fecha_Generacion_Ini = value; }
        }
        public String P_Busqueda_Fecha_Generacion_Fin
        {
            get { return Busqueda_Fecha_Generacion_Fin; }
            set { Busqueda_Fecha_Generacion_Fin = value; }
        }

        public String P_Busqueda_Fecha_Entrega_Ini
        {
            get { return Busqueda_Fecha_Entrega_Ini; }
            set { Busqueda_Fecha_Entrega_Ini = value; }
        }

        public String P_Busqueda_Fecha_Entrega_Fin
        {
            get { return Busqueda_Fecha_Entrega_Fin; }
            set { Busqueda_Fecha_Entrega_Fin = value; }
        }

        public String P_Busqueda_Vigencia_Propuesta_Ini
        {
            get { return Busqueda_Vigencia_Propuesta_Ini; }
            set { Busqueda_Vigencia_Propuesta_Ini = value; }
        }

        public String P_Busqueda_Vigencia_Propuesta_Fin
        {
            get { return Busqueda_Vigencia_Propuesta_Fin; }
            set { Busqueda_Vigencia_Propuesta_Fin = value; }
        }

        public String P_Busqueda_Proveedor
        {
            get { return Busqueda_Proveedor; }
            set { Busqueda_Proveedor = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Productos_Servicios()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Productos_Servicios(this);
        }

        public DataTable Consultar_Requisiciones()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Detalle_Requisicion()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Detalle_Requisicion(this);
        }

        public DataTable Consultar_Impuesto_Producto()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Impuesto_Producto(this);
        }


        public DataTable Consultar_Propuesta_Cotizacion()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Propuesta_Cotizacion(this);
        }

        public DataTable Consultar_Datos_Proveedor()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Datos_Proveedor(this);

        }

        public DataTable Consultar_Proveedores()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Proveedores(this);
        }

        public DataTable Consultar_Cotizacion()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Consultar_Cotizacion(this);
        }
        public String Imprimir_Cotizacion()
        {
            return Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Datos.Imprimir_Cotizacion(this);
        }

        #endregion
    }
}//fin del namespace