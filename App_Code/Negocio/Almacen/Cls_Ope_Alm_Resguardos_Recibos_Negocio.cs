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
using Presidencia.Resguardos_Recibos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Alm_Resguardos_Recibos_Negocio
/// </summary>
/// 

namespace Presidencia.Resguardos_Recibos.Negocio
{
    public class Cls_Ope_Alm_Resguardos_Recibos_Negocio
    {
        public Cls_Ope_Alm_Resguardos_Recibos_Negocio()
        {
        }

        #region (Variables Locales)
        private String Tipo_Combo;
        private String No_Orden_Compra;
        private String No_Inventario;
        private String No_Requisicion;
        private String No_Contra_Recibo;
        private String No_Serie;
        private String No_Factura;
        private String Producto_ID;
        private DataTable Dt_Productos;
        private String Fecha_Inicio_B;
        private String Fecha_Fin_B;
        private String Operacion; // Guarda si es Resguardo o Recibo
        private String Usuario_Creo;
        private String Unidad_Responsable_ID;
        private String Area_ID;
        private String Color_ID;
        private String Material_ID;
        private String Marca_ID;
        private String Modelo;
        private String Garantia;
        private String Responsable_ID;
        private String Empleado_Almacen_ID;
        private String Proveedor_ID;
        private String Observaciones;
        private String Costo;
        private String Producto;
         private String Descripcion;
       
        
        private String Fecha_Adquisicion;
        private String Fecha_Inventario;

        #endregion  

        #region (Variables Publicas)

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_No_Orden_Compra
        {
            get { return No_Orden_Compra; }
            set { No_Orden_Compra = value; }
        }
        public String P_Tipo_Combo
        {
            get { return Tipo_Combo; }
            set { Tipo_Combo = value; }
        }
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        public String P_No_Contra_Recibo
        {
            get { return No_Contra_Recibo; }
            set { No_Contra_Recibo = value; }
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
        public String P_Operacion
        {
            get { return Operacion; }
            set { Operacion = value; }
        }
        public String P_Unidad_Responsable_ID
        {
            get { return Unidad_Responsable_ID; }
            set { Unidad_Responsable_ID = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }
        public String P_Responsable_ID
        {
            get { return Responsable_ID; }
            set { Responsable_ID = value; }
        }

        public String P_Empleado_Almacen_ID
        {
            get { return Empleado_Almacen_ID; }
            set { Empleado_Almacen_ID = value; }
        }

        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }

        public String P_No_Inventario
        {
            get { return No_Inventario; }
            set { No_Inventario = value; }
        }

        public String P_Color_ID
        {
            get { return Color_ID; }
            set { Color_ID = value; }
        }

        public String P_Material_ID
        {
            get { return Material_ID; }
            set { Material_ID = value; }
        }

        public String P_No_Serie
        {
            get { return No_Serie; }
            set { No_Serie = value; }
        }

        public String P_No_Factura
        {
            get { return No_Factura; }
            set { No_Factura = value; }
        }

        public String P_Costo
        {
            get { return Costo; }
            set { Costo = value; }
        }

        public String P_Producto
        {
            get { return Producto; }
            set { Producto = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Fecha_Adquisicion
        {
            get { return Fecha_Adquisicion; }
            set { Fecha_Adquisicion = value; }
        }

        public String P_Fecha_Inventario
        {
            get { return Fecha_Inventario; }
            set { Fecha_Inventario = value; }
        }
        public String P_Marca_ID
        {
            get { return Marca_ID; }
            set { Marca_ID = value; }
        }

        public String P_Modelo
        {
            get { return Modelo; }
            set { Modelo = value; }
        }

        public String P_Garantia
        {
            get { return Garantia; }
            set { Garantia = value; }
        }

        #endregion


        #region (Metodos)

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Llenar_Combo
        ///DESCRIPCIÓN:             Método utilizado para consultar las dependencias, Areas y Empleado
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Llenar_Combo()
        {
            return Cls_Ope_Alm_Resguardos_Recibos_Datos.Llenar_Combo(this);
        }

         ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Datos_G_Ordenes_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar los datos generales de la orden de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Datos_G_Ordenes_Compra()
        {
            return Cls_Ope_Alm_Resguardos_Recibos_Datos.Consultar_Datos_G_Ordenes_Compra(this);
        }
        

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Consultar_Ordenes_Compra
        ///DESCRIPCIÓN:             Método utilizado para consultar las ordenes de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Ordenes_Compra()
        {
            return Cls_Ope_Alm_Resguardos_Recibos_Datos.Consultar_Ordenes_Compra(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Llenar_Combo
        ///DESCRIPCIÓN:             Método utilizado para consultar las dependencias, Areas y Empleado
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Productos_Requisicion()
        {
            return Cls_Ope_Alm_Resguardos_Recibos_Datos.Consultar_Productos_Requisicion(this);
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Llenar_Combo
        ///DESCRIPCIÓN:             Método utilizado para consultar las dependencias, Areas y Empleado
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Resguardar_Producto()
        {
             Cls_Ope_Alm_Resguardos_Recibos_Datos.Resguardar_Producto(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Alta_Resguardo
        ///DESCRIPCIÓN:             Método utilizado para consultar las dependencias, Areas y Empleado
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Alta_Resguardo_Recibo()
        {
            return Cls_Ope_Alm_Resguardos_Recibos_Datos.Alta_Resguardo_Recibo(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Actualizar_Orden_Compra
        ///DESCRIPCIÓN:             Método utilizado para actualizar la orden de compra
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              01/Agosto/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Actualizar_Orden_Compra()
        {
            Cls_Ope_Alm_Resguardos_Recibos_Datos.Actualizar_Orden_Compra(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN:    Alta_Resguardo
        ///DESCRIPCIÓN:             Método utilizado para consultar las dependencias, Areas y Empleado
        ///PARAMETROS:   
        ///CREO:                    Salvador Hernández Ramírez
        ///FECHA_CREO:              19/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Recibos_Resguardos()
        {
            return Cls_Ope_Alm_Resguardos_Recibos_Datos.Consulta_Recibos_Resguardos(this);
        }

        public String Consulta_Datos_Reimpresion_Resguardo_Recibo()
        {
            return Cls_Ope_Alm_Resguardos_Recibos_Datos.Consulta_Datos_Reimpresion_Resguardo_Recibo(this);
        }              
        #endregion
    }
}