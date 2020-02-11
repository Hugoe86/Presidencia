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
using Presidencia.Almacen_Impresion_Recibos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Impresion_Recibos_Negocio
/// </summary>
/// 

namespace Presidencia.Almacen_Impresion_Recibos.Negocio
{
    public class Cls_Ope_Com_Alm_Impresion_Recibos_Negocio
    {

        #region Variables Locales
            private String No_Recibo;
            private String No_Orden_Compra;
            private String No_Requisicion; 
            private String No_Contra_Recibo;
            private String Fecha_Inicial;
            private String Fecha_Final;
            private String Proveedor;
            private String Factura;
            private String Tipo_Tabla;
            private String Tipo_Articulo;
        #endregion

        #region Variables Publicas

            public String P_No_Recibo
            {
                get { return No_Recibo; }
                set { No_Recibo = value; }
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

            public String P_Fecha_Final
            {
                get { return Fecha_Final; }
                set { Fecha_Final = value; }
            }

            public String P_Fecha_Inicial
            {
                get { return Fecha_Inicial; }
                set { Fecha_Inicial = value; }
            }

            public String P_Proveedor
            {
                get { return Proveedor; }
                set { Proveedor = value; }
            }

            public String P_Factura
            {
                get { return Factura; }
                set { Factura = value; }
            }

            public String P_Tipo_Tabla
            {
                get { return Tipo_Tabla; }
                set { Tipo_Tabla = value; }
            }

            public String P_No_Contra_Recibo
            {
                get { return No_Contra_Recibo; }
                set { No_Contra_Recibo = value; }
            }
            public String P_Tipo_Articulo
            {
                get { return Tipo_Articulo; }
                set { Tipo_Articulo = value; }
            }

        
        #endregion

        #region Métodos


        // METODOS UTILIZADOS PARA CONSULTAR LOS RECIBOS TRANSITORIOS REALIZADOS POR UNIDAD
        public DataTable Consulta_Recibos_Transitorios()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consultar_Recibos_Transitorios(this);
        }

                // METODOS UTILIZADOS PARA CONSULTAR LOS RECIBOS TRANSITORIOS REALIZADOS POR TOTALIDAD
        public DataTable Consulta_Recibos_Transitorios_Totalidad()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Recibos_Transitorios_Totalidad(this);
        }
        

        public DataTable Consulta_Productos_Recibo_Transitorios()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Productos_Recibo_Transitorios(this);
        }


        // METODOS UTILIZADOS PARA CONSULTAR LOS CONTRA RECIVOS
        public DataTable Consulta_Contra_Recibos()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Contra_Recibos(this);
        }

        public DataTable Consulta_Productos_Contra_Recibo()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Productos_Contra_Recibo(this);
        }

        public DataTable Consulta_Documentos_Soporte()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Documentos_Soporte(this);
        }

        public DataTable Consulta_Detalles_Contra_Recibo()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Detalles_Contra_Recibo(this);
        }

        public DataTable Consulta_Tablas()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Tablas(this);
        }

        public DataTable Consultar_Facturas_ContraRecibo()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consultar_Facturas_ContraRecibo(this);
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
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consultar_Datos_Generales_ContraRecibo(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Montos_Orden_Compra
        /// DESCRIPCION:            Consultar los montos de la orden de compra
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            12/Julio/2011 
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Montos_Orden_Compra()
        {
            return Cls_Ope_Com_Alm_Impresion_Recibos_Datos.Consulta_Montos_Orden_Compra(this);
        }
      
       
        #endregion

    }
}
