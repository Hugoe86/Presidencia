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
using Presidencia.Almacen_Elaborar_Contrarecibo.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio
/// </summary>
/// 

namespace Presidencia.Almacen_Elaborar_Contrarecibo.Negocio
{

    public class Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio
    {
        public Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        #region (Variables Locales)
        private String Documento_ID;
        private String No_Orden_Compra;
        private String No_Requisicion;
        private Int64 No_Contra_Recibo;
        private String Empleado_Almacen_ID;
        private String Proveedor_ID;
        private String Fecha_Pago;
        private String Fecha_Resepcion;
        private String No_Factura_Interno;
        private String Usuario_Creo;
        private String Observaciones;
        private String No_Factura_Proveedor;
        private String Fecha_Factura;
        private DataTable Dt_Productos_OC;
        private DataTable Dt_Documentos_Soporte;
        private DataTable Dt_Facturas_Proveedor;
        private DataTable Dt_Ordenes_Compra;
        private DataTable Dt_Actualizar_Productos;
        private Double SubTotal;
        private Double IVA;    
        private Double Total;
        private String Fecha_Inicio_B;
        private String Fecha_Fin_B;
        private String Tipo_Orden_Compra;
        private String Partida_ID;
        private String Proyecto_Programa_ID;
        private String Dependencia_ID;
        private String Tipo_Articulo;

        #endregion

        #region (Variables Publicas)

            public String P_Documento_ID
            {
                get { return Documento_ID; }
                set { Documento_ID = value; }
            }
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
            public String P_Empleado_Almacen_ID
            {
                get { return Empleado_Almacen_ID; }
                set { Empleado_Almacen_ID = value; }
            }
            public Int64 P_No_Contra_Recibo
            {
                get { return No_Contra_Recibo; }
                set { No_Contra_Recibo = value; }
            }
            public String P_Proveedor_ID
            {
                get { return Proveedor_ID; }
                set { Proveedor_ID = value; }
            }
            public String P_Fecha_Pago
            {
                get { return Fecha_Pago; }
                set { Fecha_Pago = value; }
            }
            public String P_Observaciones
            {
                get { return Observaciones; }
                set { Observaciones = value; }
            }
            public DataTable P_Dt_Productos_OC
            {
                get { return Dt_Productos_OC; }
                set { Dt_Productos_OC = value; }
            }
            public DataTable P_Dt_Actualizar_Productos
            {
                get { return Dt_Actualizar_Productos; }
                set { Dt_Actualizar_Productos = value; }
            }
            public DataTable P_Dt_Documentos_Soporte
            {
                get { return Dt_Documentos_Soporte; }
                set { Dt_Documentos_Soporte = value; }
            }
            public DataTable P_Dt_Facturas_Proveedor
            {
                get { return Dt_Facturas_Proveedor; }
                set { Dt_Facturas_Proveedor = value; }
            }
            public DataTable P_Dt_Ordenes_Compra
            {
                get { return Dt_Ordenes_Compra; }
                set { Dt_Ordenes_Compra = value; }
            }
            public Double P_SubTotal
            {
                get { return SubTotal; }
                set { SubTotal = value; }
            }
            public Double P_IVA
            {
                get { return IVA; }
                set { IVA = value; }
            }
            public Double P_Total
            {
                get { return Total; }
                set { Total = value; }
            }
            public String P_Fecha_Inicio_B
            {
                get { return Fecha_Inicio_B; }
                set { Fecha_Inicio_B = value; }
            }
            public String P_Fecha_Fin_B
            {
                get { return Fecha_Fin_B; }
                set { Fecha_Fin_B = value; }
            }
            public String P_No_Factura_Proveedor
            {
                get { return No_Factura_Proveedor; }
                set { No_Factura_Proveedor = value; }
            }
            public String P_Fecha_Factura
            {
                get { return Fecha_Factura; }
                set { Fecha_Factura = value; }
            }
            public String P_Fecha_Resepcion
            {
                get { return Fecha_Resepcion; }
                set { Fecha_Resepcion = value; }
            }
            public String P_No_Factura_Interno
            {
                get { return No_Factura_Interno; }
                set { No_Factura_Interno = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public String P_Tipo_Orden_Compra
            {
                get { return Tipo_Orden_Compra; }
                set { Tipo_Orden_Compra = value; }
            }
            public String P_Partida_ID
            {
                get { return Partida_ID; }
                set { Partida_ID = value; }
            }
            public String P_Proyecto_Programa_ID
            {
                get { return Proyecto_Programa_ID; }
                set { Proyecto_Programa_ID = value; }
            }
            public String P_Dependencia_ID
            {
                get { return Dependencia_ID; }
                set { Dependencia_ID = value; }
            }
            public String P_Tipo_Articulo
            {
                get { return Tipo_Articulo; }
                set { Tipo_Articulo = value; }
            }

            #endregion



        #region (Metodos)

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
            /// DESCRIPCION:            Consultar ordenes de compra con estatus "RESIVIDA"
            /// PARAMETROS :            
            /// CREO       :            Salvador  Hernández Ramírez
            /// FECHA_CREO :            14/Marzo/2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Ordenes_Compra()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consulta_Ordenes_Compra(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Documentos_Soporte
            /// DESCRIPCION:            Método utilizado para consultar los Documentos de Soporte de la tabla "CAT_COM_DOCUMENTOS"
            /// PARAMETROS :            Datos: Variable de la capa de negocios que contiene los datos para realizar la consulta
            ///                         
            /// CREO       :            Salvador Hernandez Ramirez
            /// FECHA_CREO :            14/Marzo/2011  
            /// MODIFICO          :     
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Documentos_Soporte()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consulta_Documentos_Soporte(this);
            }


            ///******************************************************************************* 
            /// NOMBRE DE LA CLASE:     Consulta_Proveedores
            /// DESCRIPCION:            Consultar los datos de los proveedores
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            16/Marzo/2011  
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Proveedores()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consulta_Proveedores(this);
            }


            /*******************************************************************************
             NOMBRE DE LA CLASE:     Guardar_Contra_Recibo
             DESCRIPCION:            Accede al método que se utiliza para agregar la factura 
             PARAMETROS :                                 
             CREO       :            Salvador Hernández Ramírez
             FECHA_CREO :            04/Junio/2011 
             MODIFICO          :
             FECHA_MODIFICO    :
             CAUSA_MODIFICACION:
            /*******************************************************************************/
            public Int64 Guardar_Contra_Recibo()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Guardar_Contra_Recibo(this);
            }


            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Montos_Orden_Compra
            /// DESCRIPCION:            Accede al método que obtienen los montos de la orden de compra seleccionada por el usuario
            /// PARAMETROS :                                 
            /// CREO       :            Salvador Hernández Ramírez 
            /// FECHA_CREO :            16/Marzo/2011 
            /// MODIFICO          :     
            /// FECHA_MODIFICO    :     
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Montos_Orden_Compra()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Montos_Orden_Compra(this);
            }


            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Actualizar_Orden_Compra
            /// DESCRIPCION:            Se actualizan las ordenes de compra al estatus "",
            ///                         
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            16/Marzo/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Actualizar_Orden_Compra()
            {
                Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Actualizar_Orden_Compra(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Contrarecibo
            /// DESCRIPCION:            Consultar en contrarecibo que se genero
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            16/Marzo/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consultar_Contrarecibo()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consultar_Contrarecibo(this);
            }

            


            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Montos_Orden_Compra
            /// DESCRIPCION:            Consultar los montos de la orden de compra
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            02/julio/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Montos_Orden_Compra()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consulta_Montos_Orden_Compra(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consulta_Productos_Orden_Compra
            /// DESCRIPCION:            Método utilizado para consultar los productos de la orden de compra
            ///                         de la cual se va a generar el contra recibo
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            01/Julio/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Productos_Orden_Compra()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consulta_Productos_Orden_Compra(this);
            }


            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consultar_Datos_Generales_ContraRecibo
            /// DESCRIPCION:            Consultar los datos generales del contra recibo que fue creado
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            06/Julio/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consultar_Datos_Generales_ContraRecibo()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consultar_Datos_Generales_ContraRecibo(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consultar_Facturas_ContraRecibo
            /// DESCRIPCION:            Consultar las facturas del contra Recibo que se genero
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            06/Julio/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consultar_Facturas_ContraRecibo()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consultar_Facturas_ContraRecibo(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consultar_Documentos_ContraRecibo
            /// DESCRIPCION:            Consultar los documentos del contra Recibo que se genero
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            06/Julio/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consultar_Documentos_Contrarecibo()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consultar_Documentos_Contrarecibo(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Generar_Poliza
            /// DESCRIPCION:            Consultar los documentos del contra Recibo que se genero
            /// PARAMETROS :            
            /// CREO       :            
            /// FECHA_CREO :            19 OCT 2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public bool Generar_Poliza()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Generar_Poliza(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Consultar_Facturas
            /// DESCRIPCION:            Consultar los documentos del contra Recibo que se genero
            /// PARAMETROS :            
            /// CREO       :            
            /// FECHA_CREO :            19 OCT 2011
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consultar_Facturas()
            {
                return Cls_Ope_Com_Alm_Elaborar_Contrarecibo_Datos.Consultar_Facturas(this);
            }
        #endregion

    }
}
