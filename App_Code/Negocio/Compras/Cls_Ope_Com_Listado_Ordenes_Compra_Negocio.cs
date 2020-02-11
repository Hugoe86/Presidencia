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
using Presidencia.Listado_Ordenes_Compra.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Listado_Ordenes_Compra_Negocio
/// </summary>
/// 

namespace Presidencia.Listado_Ordenes_Compra.Negocio
{
    public class Cls_Ope_Com_Listado_Ordenes_Compra_Negocio
    {

        #region Variables Internas
        private String No_Orden_Compra = null;
        private String Fecha_Inicial = null;
        private String Proveedor_Id = null;
        private String Fecha_Final = null;
        private String Tipo_Producto_Servicio = null;
        private String Estatus;
        private String Cotizador_ID;
        private String Impresa;
        #endregion


        #region Variables Publicas
        public String P_Impresa
        {
            get { return Impresa; }
            set { Impresa = value; }
        }
        public String P_Cotizador_ID
        {
            get { return Cotizador_ID; }
            set { Cotizador_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Tipo_Producto_Servicio
        {
            get { return Tipo_Producto_Servicio; }
            set { Tipo_Producto_Servicio = value; }
        }
        public String P_No_Orden_Compra
        {
            get { return No_Orden_Compra; }
            set { No_Orden_Compra = value; }
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

        public String P_Proveedor_Id
        {
            get { return Proveedor_Id; }
            set { Proveedor_Id = value; }
        }

        #endregion


        #region Métodos


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consultar_Proveedores
        /// DESCRIPCION:            Instancia al método "Consultar_Proveedores" el cual retorna
        ///                         una tabla con los proveedores
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            18/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consultar_Proveedores()
        {
            return Cls_Ope_Com_Listado_Ordenes_Compra_Datos.Consultar_Proveedores(this);
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Instancia al método "Consulta_Ordenes_Compra" el cual retorna
        ///                         una tabla con las ordenes de compra
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            18/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Ordenes_Compra()
        {
            return Cls_Ope_Com_Listado_Ordenes_Compra_Datos.Consulta_Ordenes_Compra(this);
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Cabecera_Orden_Compra
        /// DESCRIPCION:            Instancia al método "Consulta_Cabecera_Orden_Compra" el cual retorna
        ///                         una tabla con la información de la orden de compra
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            18/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Cabecera_Orden_Compra()
        {
            return Cls_Ope_Com_Listado_Ordenes_Compra_Datos.Consulta_Cabecera_Orden_Compra(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Detalles_Orden_Compra
        /// DESCRIPCION:            Instancia al método "Consulta_Cabecera_Orden_Compra" el cual retorna
        ///                         una tabla con los productos de la orden de compra
        /// PARAMETROS :            
        /// CREO       :            Salvador Hernández Ramírez
        /// FECHA_CREO :            18/Abril/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Detalles_Orden_Compra()
        {
            return Cls_Ope_Com_Listado_Ordenes_Compra_Datos.Consulta_Detalles_Orden_Compra(this);
        }
        public DataTable Consulta_Directores()
        {
            return Cls_Ope_Com_Listado_Ordenes_Compra_Datos.Consulta_Directores();
        }
        #endregion

    }
}
