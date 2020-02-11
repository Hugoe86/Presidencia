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
using Presidencia.Licitacion_Proveedores.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Licitacion_Proveedores_Negocio
/// </summary>
namespace Presidencia.Licitacion_Proveedores.Negocio
{

    public class Cls_Ope_Com_Licitacion_Proveedores_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS
        ///*******************************************************************************
        #region Variables Internas
        private String No_Licitacion;
        private String Folio;
        private String Estatus;
        private String Usuario;
        private String Usuario_ID;
        private String Concepto_ID;
        private String Producto_ID;
        private String Monto_Total;
        private DataTable Dt_Requisiciones;
        private DataTable Dt_Productos;
        private Cls_Ope_Com_Licitacion_Proveedores_Datos Datos_Lic_Pro;
        private String Fecha_Inicio;
        private String Fecha_Fin;
        private String Tipo;
        private String Lista_Requisiciones;

       


        #endregion Fin_Variables_Internas

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables Publicas

        public String P_No_Licitacion
        {
            get { return No_Licitacion; }
            set { No_Licitacion = value; }
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

        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }

        public String P_Monto_Total
        {
            get { return Monto_Total; }
            set { Monto_Total = value; }
        }

        public DataTable P_Dt_Requisiciones
        {
            get { return Dt_Requisiciones; }
            set { Dt_Requisiciones = value; }
        }

        public DataTable P_Dt_Productos
        {
            get { return Dt_Productos; }
            set { Dt_Productos = value; }
        }
        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }
        public String P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }

        public String P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Lista_Requisiciones
        {
            get { return Lista_Requisiciones; }
            set { Lista_Requisiciones = value; }
        }
        #endregion Fin_Variables_Publicas

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        
        public Cls_Ope_Com_Licitacion_Proveedores_Negocio()
        {
            Datos_Lic_Pro = new Cls_Ope_Com_Licitacion_Proveedores_Datos();
        }

        public void Modificar_Licitacion()
        {
            Datos_Lic_Pro.Modificar_Licitacion(this);
        }

        public void Modificar_Licitacion_Detalles()
        {
            Datos_Lic_Pro.Modificar_Licitacion_Detalles(this);
        }

        public DataTable Consultar_Licitaciones()
        {
            return Datos_Lic_Pro.Consultar_Licitaciones(this);
        }

        public DataTable Consulta_Productos_Detalle()
        {
            return Datos_Lic_Pro.Consulta_Productos_Detalle(this);
        }
       
        public DataTable Consultar_Concepto_Proveedor()
        {
            return Datos_Lic_Pro.Consultar_Concepto_Proveedor(this);
        }
        public DataTable Consultar_Impuesto_Producto()
        {
            return Datos_Lic_Pro.Consultar_Impuesto_Producto(this);
        }
        public void Modificar_Presupuesto()
        {
            Datos_Lic_Pro.Modificar_Presupuesto(this);
        }
        #endregion Fin_Metodos
    }//fin del class
}//fin del namespace