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
using Presidencia.Orden_Salida.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Orden_Salida_Datos
/// </summary>
/// 

namespace Presidencia.Orden_Salida.Negocio
{
    public class Cls_Ope_Com_Alm_Orden_Salida_Negocio
    {
        #region Variables Locales

        private String Tipo_Data_Table;
        private String No_Requisicion;
        private String No_Orden_Compra;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Dependencia;
        private String Area;
        private String Tipo_Salida;
        private String No_Orden_Salida;

        public String P_No_Orden_Salida
        {
            get { return No_Orden_Salida; }
            set { No_Orden_Salida = value; }
        }

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

        public String P_Dependencia
        {
            get { return Dependencia; }
            set { Dependencia = value; }
        }

        public String P_Area
        {
            get { return Area; }
            set { Area = value; }
        }

        public String P_Tipo_Salida
        {
            get { return Tipo_Salida; }
            set { Tipo_Salida = value; }
        }
        

        public String P_Tipo_Data_Table
        {
            get { return Tipo_Data_Table; }
            set { Tipo_Data_Table = value; }
        }

        #endregion

        #region Metodos


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones
        ///DESCRIPCIÓN: Método utilizado para consultar las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO: Salvador Hernández Ramírez
        ///FECHA_CREO: 17/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Requisiciones()
        {
            return Cls_Ope_Com_Alm_Orden_Salida_Datos.Consulta_Requisiciones(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion
        ///DESCRIPCIÓN: Método utilizado para consultar los productos de las requisiciones de stock de almacén
        ///PARAMETROS:   
        ///CREO: Salvador Hernández Ramírez
        ///FECHA_CREO: 17/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Productos_Orden_Salida()
        {
            return Cls_Ope_Com_Alm_Orden_Salida_Datos.Consulta_Productos_Orden_Salida(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN:  Metod que manda llamar a su metodo correspondiente de la capa de datos
        ///PARAMETROS:   
        ///CREO: Salvador Hernández Ramírez
        ///FECHA_CREO: 18/Febrero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_DataTable()
        {
            return Cls_Ope_Com_Alm_Orden_Salida_Datos.Consultar_DataTable(this);
        }


        // PARA la reimpresion de las ordenes de salida

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
            return Cls_Ope_Com_Alm_Orden_Salida_Datos.Consultar_Detalles_Orden_Salida(this);
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
            return Cls_Ope_Com_Alm_Orden_Salida_Datos.Consultar_Informacion_General_OS(this);
        }

        #endregion

    }
}