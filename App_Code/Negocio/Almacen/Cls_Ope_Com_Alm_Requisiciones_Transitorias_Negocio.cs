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
using Presidencia.Requisiciones_Transitorias.Datos;

namespace Presidencia.Requisiciones_Transitorias.Negocio
{

    public class Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio
    {
        public Cls_Ope_Com_Alm_Requisiciones_Transitorias_Negocio()
        {
        }
        #region Variables Locales

        private String Tipo_Data_Table;
        private String No_Requisicion;
        private String No_Orden_Compra;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Dependencia_ID;
        private String Area_ID;
        private String Estatus;
        private String Empleado_Recibio_ID;
        private String Empleado_Almacen_ID;
        private String Nombre_Empleado_Almacen;
        private DataTable Dt_Productos_Requisicion;
        private Int64 No_Orden_Salida;
        private String Proyecto_Programa_ID;
        private String No_Empleado;

        #endregion

        #region Variables Publicas

        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
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

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }

        public String P_Tipo_Data_Table
        {
            get { return Tipo_Data_Table; }
            set { Tipo_Data_Table = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }


        public String P_Empleado_Recibio_ID
        {
            get { return Empleado_Recibio_ID; }
            set { Empleado_Recibio_ID = value; }
        }

        public String P_Empleado_Almacen_ID
        {
            get { return Empleado_Almacen_ID; }
            set { Empleado_Almacen_ID = value; }
        }

        public String P_Nombre_Empleado_Almacen
        {
            get { return Nombre_Empleado_Almacen; }
            set { Nombre_Empleado_Almacen = value; }
        }

        public Int64 P_No_Orden_Salida
        {
            get { return No_Orden_Salida; }
            set { No_Orden_Salida = value; }
        }
        public DataTable P_Dt_Productos_Requisicion
        {
            get { return Dt_Productos_Requisicion; }
            set { Dt_Productos_Requisicion = value; }
        }
        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }

        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        
        #endregion

        #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN:          Método utilizado para consultar las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Requisiciones()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consulta_Requisiciones(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Detalles_Requisicion
        ///DESCRIPCIÓN:          Método utilizado para consultar la información general de la requisición
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Detalles_Requisicion()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consulta_Detalles_Requisicion(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Detalles_Requisicion
        ///DESCRIPCIÓN:          Método utilizado para consultar los productos de las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Productos_Requisicion()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consulta_Productos_Requisicion(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN:          Metodo que manda llamar a su metodo correspondiente de la capa de datos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           22/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_DataTable()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consultar_DataTable(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Pragrama_Financiamiento
        ///DESCRIPCIÓN:          Metodo que manda llamar a su método correspondiente de la capa de datos
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           23/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Pragrama_Financiamiento()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consulta_Pragrama_Financiamiento(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Alta_Orden_Salida
        ///DESCRIPCIÓN:          Metodo que manda llamar al método donde se da de alta la orden de salida
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           23/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public long Alta_Orden_Salida()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Alta_Orden_Salida(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Informacion_General_OS
        ///DESCRIPCIÓN:          Metodo que manda llamar a su método en el cual se consulta
        ///                      la información general de la orden de salida.
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Informacion_General_OS()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consultar_Informacion_General_OS(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Orden_Salida
        ///DESCRIPCIÓN:          Método que manda llamar a su método en el cual se consultan
        ///                      los detalles de la orden de salida.
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_Detalles_Orden_Salida()
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consultar_Detalles_Orden_Salida(this);
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_Detalles_Orden_Salida
        ///DESCRIPCIÓN:          Método que manda llamar a su método en el cual se consultan
        ///                      los detalles de la orden de salida.
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           24/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Consultar_Cantidad_Entregada(String Producto, String Requicision)
        {
            return Cls_Ope_Alm_Requisiciones_Transitorias_Datos.Consultar_Cantidad_Entregada(Producto, Requicision);
        }

        #endregion
    }
}