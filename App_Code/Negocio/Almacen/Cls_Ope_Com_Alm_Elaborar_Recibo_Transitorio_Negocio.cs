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
using Presidencia.Almacen_Elaborar_Recibo_Transitorio.Datos;


/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio
/// </summary>
/// 

namespace Presidencia.Almacen_Elaborar_Recibo_Transitorio.Negocio
{
    public class Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Negocio
    {

        #region (Variables Locales)
        private String Tipo_Combo;
        private DataTable Dt_Combo;
        private String No_Orden_Compra;
        private String No_Requisicion;
        private String No_Contra_Recibo;
        private String No_Serie;
        private String Proveedor_ID;
        private String Producto_ID;
        private DataTable Dt_Serie_Productos;
        private DataTable Dt_Productos;
        private String Fecha_Inicio_B;
        private String Fecha_Fin_B;
        private String Tipo_Recibo;
        private String No_Recibo_Transitorio;
        private String Usuario_Creo;
        private String Responsable_ID;
        private String Unidad_Responsable_ID;
        

        #endregion

        #region (Variables Publicas)

        public String P_Tipo_Recibo
        {
            get { return Tipo_Recibo; }
            set { Tipo_Recibo = value; }
        }

        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }

        public DataTable P_Dt_Combo
        {
            get { return Dt_Combo; }
            set { Dt_Combo = value; }
        }
        public DataTable P_Dt_Productos
        {
            get { return Dt_Productos; }
            set { Dt_Productos = value; }
        }

        public String P_Tipo_Combo
        {
            get { return Tipo_Combo; }
            set { Tipo_Combo = value; }
        }
        public String P_No_Orden_Compra
        {
            get { return No_Orden_Compra; }
            set { No_Orden_Compra = value; }
        }
        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
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
        public String P_Producto_ID
        {
            get { return Producto_ID; }
            set { Producto_ID = value; }
        }
        public DataTable P_Dt_Serie_Productos
        {
            get { return Dt_Serie_Productos; }
            set { Dt_Serie_Productos = value; }
        }
        public String P_No_Serie
        {
            get { return No_Serie; }
            set { No_Serie = value; }
        }
        public String P_No_Recibo_Transitorio
        {
            get { return No_Recibo_Transitorio; }
            set { No_Recibo_Transitorio = value; }
        }
        public String P_No_Contra_Recibo
        {
            get { return No_Contra_Recibo; }
            set { No_Contra_Recibo = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Responsable_ID
        {
            get { return Responsable_ID; }
            set { Responsable_ID = value; }
        }
        public String P_Unidad_Responsable_ID
        {
            get { return Responsable_ID; }
            set { Responsable_ID = value; }
        }

        #endregion 

        ///NOMBRE DE LA FUNCIÓN:    Consulta_Requisiciones
        ///DESCRIPCIÓN:             Método utilizado para consultar las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              17/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Llenar_Combo()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Llenar_Combo(this);
        }


        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Consultar ordenes de compra con estatus "RESIVIDA"
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            22/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Ordenes_Compra()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consultar_Ordenes_Compra(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Consultar ordenes de compra con estatus "RESIVIDA"
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            22/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Productos_Orden_Compra()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Productos_Orden_Compra(this);
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
        public DataTable Consulta_Productos_Recibo_Transitorio()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Productos_Recibo_Transitorio(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Productos_Requision
        /// DESCRIPCION:            Consulta los productos cuando se realiza un recibo transitorio por totalidad
        ///                         ya que estos productos son consultados de la requisición y no de la tabla donde PROD_CONTRARECIBO
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            21/Julio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Productos_Requision()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Productos_Requision(this);
        }

        

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Empleados_Almacen
        /// DESCRIPCION:            Consultar los empleados que pertenecen al Area "Almacén"
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            22/Marzo/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Empleados_Almacen()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Empleados_Almacen(this);
        }

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Consultar ordenes de compra con estatus "RESIVIDA"
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            02/Julio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Datos_Generales()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Datos_Generales(this);
        }

         ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Datos_Generales_Recibo_Transitorio
        /// DESCRIPCION:            Consultar la información que va en el encabezado de los recibos transitorios
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            09/Julio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Datos_Generales_Recibo_Transitorio()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Datos_Generales_Recibo_Transitorio(this);
        }
        
               ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Datos_Generales_Recibo_Transitorio
        /// DESCRIPCION:            Consultar la información que va en el encabezado de los recibos transitorios
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            09/Julio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Datos_Generales_Recibo_Transitorio_Totalidad()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Datos_Generales_Recibo_Transitorio_Totalidad(this);
        }
        

        ///*******************************************************************************
        /// NOMBRE DE LA CLASE:     Consulta_Ordenes_Compra
        /// DESCRIPCION:            Consultar ordenes de compra con estatus "RESIVIDA"
        /// PARAMETROS :            
        /// CREO       :            Salvador  Hernández Ramírez
        /// FECHA_CREO :            11/Julio/2011
        /// MODIFICO          :
        /// FECHA_MODIFICO    :
        /// CAUSA_MODIFICACION:
        ///*******************************************************************************/
        public DataTable Consulta_Facturas()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Consulta_Facturas(this);
        }
        

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Crear_Recibo
        ///DESCRIPCIÓN:             Método utilizado para instanciar al método que optiene el Id_Consecutivo
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              26/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public Int64 Guardar_Recibo()
        {
            return Cls_Ope_Com_Alm_Elaborar_Recibo_Transitorio_Datos.Guardar_Recibo(this);
        }
    }
}