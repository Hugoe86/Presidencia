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
using Presidencia.Cotizaciones_Proveedores.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Cotizaciones_Proveedores_Negocio
/// </summary>
namespace Presidencia.Cotizaciones_Proveedores.Negocio
{

    public class Cls_Ope_Com_Cotizaciones_Proveedores_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS
        ///*******************************************************************************
        #region Variables_Internas
        private String No_Cotizacion;
        private String Folio;
        private String Estatus;
        private String Total;
        private String Total_Cotizado;
        private String Usuario;
        private String Usuario_ID;
        private String Giro_ID;
        private String Tipo;
        private String Producto_ID;
        private DataTable Dt_Productos;
        private String Ope_Com_Req_Producto_ID;
        private String Lista_Requisiciones;
        private String Listado_Almacen;

       
        Cls_Ope_Com_Cotizaciones_Proveedores_Datos Datos_Cotizaciones;

        #endregion
        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas
        public String P_No_Cotizacion
        {
            get { return No_Cotizacion; }
            set { No_Cotizacion = value; }
        }
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Total
        {
            get { return Total; }
            set { Total = value; }
        }
        public String P_Total_Cotizado
        {
            get { return Total_Cotizado; }
            set { Total_Cotizado = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Usuario_ID
        {
            get { return Usuario_ID; }
            set { Usuario_ID = value; }
        }

        public String P_Giro_ID
        {
            get { return Giro_ID; }
            set { Giro_ID = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
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

        public String P_Ope_Com_Req_Producto_ID
        {
            get { return Ope_Com_Req_Producto_ID; }
            set { Ope_Com_Req_Producto_ID = value; }
        }

        public String P_Lista_Requisiciones
        {
            get { return Lista_Requisiciones; }
            set { Lista_Requisiciones = value; }
        }

        public String P_Listado_Almacen
        {
            get { return Listado_Almacen; }
            set { Listado_Almacen = value; }
        }
        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public Cls_Ope_Com_Cotizaciones_Proveedores_Negocio()
        {
            Datos_Cotizaciones = new Cls_Ope_Com_Cotizaciones_Proveedores_Datos();
        }

        public DataTable Consultar_Cotizaciones()
        {
            return Datos_Cotizaciones.Consultar_Cotizaciones(this);
        }

        public bool Empleado_Cotizador()
        {
            return Datos_Cotizaciones.Consultar_Cotizadores();
        }

        public DataTable Consultar_Concepto_Cotizador()
        {
            return Datos_Cotizaciones.Consultar_Concepto_Cotizador(this);
        }

        public DataTable Consultar_Detalle_Consolidacion()
        {
            return Datos_Cotizaciones.Consultar_Detalle_Consolidacion(this);
        }

        public DataTable Consultar_Detalle_Requisicion()
        {
            return Datos_Cotizaciones.Consultar_Detalle_Requisicion(this);
        }

        public DataTable Consulta_Productos()
        {
            return Datos_Cotizaciones.Consulta_Productos(this);
        }

        public DataTable Consulta_Proveedores()
        {
            return Datos_Cotizaciones.Consulta_Proveedores(this);
        }

        public DataTable Consultar_Proveedores_Asignados()
        {
            return Datos_Cotizaciones.Consultar_Proveedores_Asignados(this);
        }

        public DataTable Consultar_Impuesto_Producto()
        {
            return Datos_Cotizaciones.Consultar_Impuesto_Producto(this);
        }

        public void Modificar_Detalle_Productos()
        {
        Datos_Cotizaciones.Modificar_Detalle_Productos(this);
        }

        public void Modificar_Cotizacion()
        {
            Datos_Cotizaciones.Modificar_Cotizacion(this);
        }

        #endregion
    }//fin del class
}//fin namespace
