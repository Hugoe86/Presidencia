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
using Presidencia.Almacen_Recepcion_Material.Datos;

/// <summary>
/// Summary description for Cls_Alm_Com_Recepcion_Material_Negocio
/// </summary>
namespace Presidencia.Almacen_Recepcion_Material.Negocio
{
    public class Cls_Alm_Com_Recepcion_Material_Negocio
    {
        public Cls_Alm_Com_Recepcion_Material_Negocio()
        {
        }

        #region (Variables Locales)
            private String No_Orden_Compra;
            private String No_Requisicion;
            private String Usuario;
            private String Empleado_Almacen_ID;
            private String No_ContraRecibo;
            private String Proveedor_ID;
            private String Observaciones;
            private String Busqueda;
            private String Resguardada;
            private String Usuario_Id_Resguardo;
            private String No_Factura_Proveedor;
            private String Fecha_Inicio_B;
            private DataTable Dt_Facturas_Proveedores;
            private DataTable Dt_Productos_Stock;
            private DataTable Dt_Ordenes_Compra;
            private DateTime Fecha_Factura;
            private DateTime Fecha_Pago;
            private Double SubTotal_Con_Impuesto;
            private Double SubTotal_Sin_Impuesto;
            private Double IVA;
            private Double IEPS;
            private Double Total;
            private long No_Factura_Interno;
            
            
        #endregion

        #region (Variables Publicas)
            public String P_No_Orden_Compra
            {
                get { return No_Orden_Compra; }
                set { No_Orden_Compra = value; }
            }
            public String P_No_Requisicion
            {
                get { return No_Requisicion; }
                set { No_Requisicion = value; }
            }
   
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Empleado_Almacen_ID
            {
                get { return Empleado_Almacen_ID; }
                set { Empleado_Almacen_ID = value; }
            }

            public String P_No_ContraRecibo
            {
                get { return No_ContraRecibo; }
                set { No_ContraRecibo = value; }
            }

            public String P_Proveedor_ID
            {
                get { return Proveedor_ID; }
                set { Proveedor_ID = value; }
            }

            public DateTime P_Fecha_Pago
            {
                get { return Fecha_Pago; }
                set { Fecha_Pago = value; }
            }

            public String P_Observaciones
            {
                get { return Observaciones; }
                set { Observaciones = value; }
            }

            public DataTable P_Dt_Facturas_Proveedores
            {
                get { return Dt_Facturas_Proveedores; }
                set { Dt_Facturas_Proveedores = value; }
            }

            public String P_Busqueda
            {
                get { return Busqueda; }
                set { Busqueda = value; }
            }

            public String P_Resguardada
            {
                get { return Resguardada; }
                set { Resguardada = value; }
            }

            public String P_Usuario_Id_Resguardo
            {
                get { return Usuario_Id_Resguardo; }
                set { Usuario_Id_Resguardo = value; }
            }

            public DataTable P_Dt_Productos_Stock
            {
                get { return Dt_Productos_Stock; }
                set { Dt_Productos_Stock = value; }
            }

            public long P_No_Factura_Interno
            {
                get { return No_Factura_Interno; }
                set { No_Factura_Interno = value; }
            }

            public String P_No_Factura_Proveedor
            {
              get { return No_Factura_Proveedor; }
              set { No_Factura_Proveedor = value; }
            }

            public DateTime P_Fecha_Factura
            {
              get { return Fecha_Factura; }
              set { Fecha_Factura = value; }
            }

            public Double P_SubTotal_Con_Impuesto
            {
              get { return SubTotal_Con_Impuesto; }
              set { SubTotal_Con_Impuesto = value; }
            }

            public Double P_SubTotal_Sin_Impuesto
            {
              get { return SubTotal_Sin_Impuesto; }
              set { SubTotal_Sin_Impuesto = value; }
            }

            public Double P_IVA
            {
              get { return IVA; }
              set { IVA = value; }
            }

            public Double P_IEPS
            {
              get { return IEPS; }
              set { IEPS = value; }
            }

            public Double P_Total
            {
              get { return Total; }
              set { Total = value; }
            }

            public DataTable P_Dt_Ordenes_Compra
            {
                get { return Dt_Ordenes_Compra; }
                set { Dt_Ordenes_Compra = value; }
            }
            public String P_Fecha_Inicio_B
            {
                get { return Fecha_Inicio_B; }
                set { Fecha_Inicio_B = value; }
            }
            private String Fecha_Fin_B;

            public String P_Fecha_Fin_B
            {
                get { return Fecha_Fin_B; }
                set { Fecha_Fin_B = value; }
            }
        #endregion

        #region (Metodos)

            ///******************************************************************************* 
            /// NOMBRE DE LA CLASE:     Consulta_Proveedores
            /// DESCRIPCION:            Consultar los datos de los proveedores
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            30/Diciembre/2010 13:52 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Proveedores()
            {
                return Cls_Alm_Com_Recepcion_Material_Datos.Consulta_Proveedores(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
            /// DESCRIPCION:            Consultar ordenes de compra
            /// PARAMETROS :            
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            30/Diciembre/2010 13:52 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Ordenes_Compra()
            {
                return Cls_Alm_Com_Recepcion_Material_Datos.Consulta_Ordenes_Compra(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Orden_Compra_Detalles
            /// DESCRIPCION:            Consultar los detalles de una orden de compra
            /// PARAMETROS :          
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            30/Diciembre/2010 13:52 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Orden_Compra_Detalles()
            {
                return Cls_Alm_Com_Recepcion_Material_Datos.Consulta_Orden_Compra_Detalles(this);
            }

         ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Servicios_OC
            /// DESCRIPCION:            Consultar los servicios de una orden de compra
            /// PARAMETROS :          
            /// CREO       :            Noe Mosqueda Valadez
            /// FECHA_CREO :            30/Diciembre/2010 13:52 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Servicios_OC()
            {
                return Cls_Alm_Com_Recepcion_Material_Datos.Consulta_Servicios_OC(this);
            }
        

         
   
            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Actualizar_Orden_Compra
            /// DESCRIPCION:            Se actualizan las ordenes de compra al estatus "RECIBIDA",
            ///                         y se resguardan los productos de la orden de compra.
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            25/Febrero/2010 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Actualizar_Orden_Compra()
            {
                Cls_Alm_Com_Recepcion_Material_Datos.Actualizar_Orden_Compra(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Montos_Orden_Compra
            /// DESCRIPCION:            Accede al método que obtienen los montos de la orden de compra seleccionada por el usuario
            /// PARAMETROS :                                 
            /// CREO       :            Noe Mosqueda Valadez  
            /// FECHA_CREO :            24/Febrero/2010 
            /// MODIFICO          :     Se implemtento el método "Alta_Bitacora"
            /// FECHA_MODIFICO    :     03/Marzo/2011
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Montos_Orden_Compra()
            {
                return Cls_Alm_Com_Recepcion_Material_Datos.Montos_Orden_Compra(this);
            }

            ///******************************************************************************* 
            /// NOMBRE DE LA CLASE:     Consultar_Datos_Orden_Compra
            /// DESCRIPCION:            onsultar los datos de la orden de compra
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            19/Marzo/2011  
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consultar_Datos_Orden_Compra()
            {
                return Cls_Alm_Com_Recepcion_Material_Datos.Consultar_Datos_Orden_Compra(this);
            }
        
            ///******************************************************************************* 
            /// NOMBRE DE LA CLASE:     Consultar_Productos_Servicios_Orden_Compra
            /// DESCRIPCION:            onsultar los datos de la orden de compra
            /// PARAMETROS :            
            /// CREO       :            Gustavo Angeles Cruz
            /// FECHA_CREO :            25/Jul/2011  
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consultar_Productos_Servicios_Orden_Compra()
            {
                return Cls_Alm_Com_Recepcion_Material_Datos.Consultar_Productos_Servicios_Orden_Compra(this);
            }
        #endregion
    }
}