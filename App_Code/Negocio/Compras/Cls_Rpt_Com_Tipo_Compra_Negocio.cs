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
using Presidencia.Rpt_Tipo_Compra.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Com_Tipo_Compra_Negocio
/// </summary>
namespace Presidencia.Rpt_Tipo_Compra.Negocio
{

    public class Cls_Rpt_Com_Tipo_Compra_Negocio
    {
        ///*******************************************************************
        ///VARIABLES INTERNAS
        ///*******************************************************************
        #region Variables Internas
        private String Tipo_Compra;
        private String Requisicion_ID;
        private String Empleado_Cotizador_ID;
        private String Empleado_Proveedor_ID;
        private String Producto_ID;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Tipo_Articulo;

       

        #endregion

        ///*******************************************************************
        ///VARIABLES PUBLICAS
        ///*******************************************************************
        #region Variables Publicas
        public String P_Tipo_Compra
        {
            get { return Tipo_Compra; }
            set { Tipo_Compra = value; }
        }

        public String P_Requisicion_ID
        {
            get { return Requisicion_ID; }
            set { Requisicion_ID = value; }
        }

        public String P_Empleado_Cotizador_ID
        {
            get { return Empleado_Cotizador_ID; }
            set { Empleado_Cotizador_ID = value; }
        }

        public String P_Empleado_Proveedor_ID
        {
            get { return Empleado_Proveedor_ID; }
            set { Empleado_Proveedor_ID = value; }
        }

        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
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

        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }
        #endregion

        ///*******************************************************************
        ///VARIABLES METODOS
        ///*******************************************************************
        #region Metodos 

        public DataTable Consultar_Requisiciones()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Proveedores()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Proveedores(this);
        }

        public DataTable Consultar_Cotizadores()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Cotizadores(this);
        }

        public DataTable Consultar_Productos()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Productos(this);
        }


        public DataSet Consultar_Compra_Directa()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Compra_Directa(this);

        }

        public DataSet Consultar_Licitacion()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Licitacion(this);
        }

        public DataSet Consultar_Comite_Compra()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Comite_Compra(this);
        }

        public DataSet Consultar_Cotizacion()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Cotizacion(this);
        }

        public DataTable Consultar_Servicios()
        {
            return Cls_Rpt_Com_Tipo_Compra_Datos.Consultar_Servicios(this);
        }

        #endregion 

    }
}