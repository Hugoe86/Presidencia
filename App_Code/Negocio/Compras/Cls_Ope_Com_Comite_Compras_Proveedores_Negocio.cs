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
using Presidencia.Comite_Compras_Proveedores.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Comite_Compras_Proveedores_Negocio
/// </summary>
namespace Presidencia.Comite_Compras_Proveedores.Negocio
{

    public class Cls_Ope_Com_Comite_Compras_Proveedores_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS
        ///*******************************************************************************
        #region Variables_Internas

        private Cls_Ope_Com_Comite_Compras_Proveedores_Datos Datos_Comite_Proveedores;
        private String No_Comite_Compras;
        private String Folio;
        private String Estatus;
        private String Monto_Total;
        private String Usuario;
        private String Usuario_ID;
        private String Concepto_ID;
        private String Tipo;
        private String Producto_ID;
        private DataTable Dt_Productos;
        private String Ope_Com_Req_Producto_ID;
        private String Lista_Requisiciones;

       
        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas


        public String P_No_Comite_Compras
        {
            get { return No_Comite_Compras; }
            set { No_Comite_Compras = value; }
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
        public String P_Monto_Total
        {
            get { return Monto_Total; }
            set { Monto_Total = value; }
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

        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
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

        #endregion
        
        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        public Cls_Ope_Com_Comite_Compras_Proveedores_Negocio()
        {
            Datos_Comite_Proveedores = new Cls_Ope_Com_Comite_Compras_Proveedores_Datos();
        }

        public DataTable Consulta_Comite_Compras()
        {
            return Datos_Comite_Proveedores.Consulta_Comite_Compras(this);
        }

        public DataTable Consulta_Productos()
        {
            return Datos_Comite_Proveedores.Consulta_Productos(this);
        }

        public DataTable Consultar_Comite_Detalle_Requisicion()
        {
            return Datos_Comite_Proveedores.Consultar_Comite_Detalle_Requisicion(this);
        }

        public DataTable Consultar_Detalle_Consolidacion()
        {
            return Datos_Comite_Proveedores.Consultar_Detalle_Consolidacion(this);
        }
        public DataTable Consultar_Proveedores_Asignados()
        {
            return Datos_Comite_Proveedores.Consultar_Proveedores_Asignados(this);
        }

        public DataTable Consultar_Concepto_Requisiciones()
        {
            return Datos_Comite_Proveedores.Consultar_Concepto_Requisiciones(this);
        }

        public DataTable Consulta_Proveedores()
        {
            return Datos_Comite_Proveedores.Consulta_Proveedores(this);
        }

        public DataTable Consultar_Impuesto_Producto()
        {
            return Datos_Comite_Proveedores.Consultar_Impuesto_Producto(this);
        }

        public void Modificar_Comite_Compras()
        {
            Datos_Comite_Proveedores.Modificar_Comite_Compras(this);
        }
       

    }

}//fin namespace