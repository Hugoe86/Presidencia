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
using Presidencia.Almacen_Registro_Datos.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Registro_De_Datos_Negocio
/// </summary>
/// 

namespace Presidencia.Almacen_Registro_Datos.Negocio
{

    public class Cls_Ope_Com_Alm_Registro_De_Datos_Negocio
    {
        public Cls_Ope_Com_Alm_Registro_De_Datos_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }


        #region (Variables Locales)

        private DataTable Dt_Productos_Serializados;
        private String No_Orden_Compra;
        private String No_Requisicion;
        private String Tipo_Tabla;
        private String Usuario_Creo;
        private String Empleado_Almacen_ID;
        private String Fecha_Inicio_B;
        private String Fecha_Fin_B;
        private Int64 No_ContraRecibo;
        private String Proveedor_ID;

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

            public String P_Empleado_Almacen_ID
            {
                get { return Empleado_Almacen_ID; }
                set { Empleado_Almacen_ID = value; }
            }

            public Int64 P_No_ContraRecibo
            {
                get { return No_ContraRecibo; }
                set { No_ContraRecibo = value; }
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
            public String P_Tipo_Tabla
            {
                get { return Tipo_Tabla; }
                set { Tipo_Tabla = value; }
            }
            public String P_Usuario_Creo
            {
                get { return Usuario_Creo; }
                set { Usuario_Creo = value; }
            }
            public DataTable P_Dt_Productos_Serializados
            {
                get { return Dt_Productos_Serializados; }
                set { Dt_Productos_Serializados = value; }
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
                return Cls_Ope_Com_Alm_Registro_De_Datos_Datos.Consulta_Ordenes_Compra(this);
            }

           
            ///******************************************************************************* 
            /// NOMBRE DE LA CLASE:     Consulta_Tablas
            /// DESCRIPCION:            Consultar los datos de los proveedores, marcas y colores
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            16/Marzo/2011  
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public DataTable Consulta_Tablas()
            {
                return Cls_Ope_Com_Alm_Registro_De_Datos_Datos.Consulta_Tablas(this);
            }

            ///*******************************************************************************
            /// NOMBRE DE LA CLASE:     Alta_Productos_Inventario
            /// DESCRIPCION:            Método utilizado para dar de Alta los productos de la orden de compra
            ///                         de que se serializaron
            /// PARAMETROS :            
            /// CREO       :            Salvador Hernández Ramírez
            /// FECHA_CREO :            07/Julio/2011 
            /// MODIFICO          :
            /// FECHA_MODIFICO    :
            /// CAUSA_MODIFICACION:
            ///*******************************************************************************/
            public void Alta_Productos_Inventario()
            {
                 Cls_Ope_Com_Alm_Registro_De_Datos_Datos.Alta_Productos_Inventario(this);
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
                return Cls_Ope_Com_Alm_Registro_De_Datos_Datos.Consulta_Productos_Orden_Compra(this);
            }

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
            public Int64 Consulta_Consecutivo()
            {
                return Cls_Ope_Com_Alm_Registro_De_Datos_Datos.Consulta_Consecutivo(this);
            } 
        #endregion
    }
}
