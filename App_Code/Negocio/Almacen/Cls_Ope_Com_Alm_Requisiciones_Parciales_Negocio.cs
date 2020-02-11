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
using Presidencia.Requisiciones_Parciales.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Alm_Requisiciones_Parciale
/// </summary>
/// 

namespace Presidencia.Requisiciones_Parciales.Negocio
{
    public class Cls_Ope_Com_Alm_Requisiciones_Parciales_Negocio
    {
        #region Variables Locales

        private String Tipo_Data_Table;
        private String No_Requisicion;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Dependencia;
        private String Observaciones;
        private String Area;
        private DataTable Dt_Productos_Cancelar;
        private String Proyecto_Programa_ID;
        private String Dependencia_ID;
        private String Estatus; 

        #endregion

        #region Variables Publicas

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
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

        public String P_Tipo_Data_Table
        {
            get { return Tipo_Data_Table; }
            set { Tipo_Data_Table = value; }
        }

        public DataTable P_Dt_Productos_Cancelar
        {
            get { return Dt_Productos_Cancelar; }
            set { Dt_Productos_Cancelar = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }

        public String P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        #endregion

        #region Metodos


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Requisiciones_Parciales
        ///DESCRIPCIÓN:          Método utilizado para consultar las requisiciones parciales de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Requisiciones_Parciales()
        {
            return Cls_Ope_Com_Alm_Requisiciones_Parciales_Datos.Consulta_Requisiciones_Parciales(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Productos_Requisicion_Parcial
        ///DESCRIPCIÓN:          Método utilizado para consultar los prodcutos de las requisiciones parciales de stock de almacén
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Productos_Requisicion_Parcial()
        {
            return Cls_Ope_Com_Alm_Requisiciones_Parciales_Datos.Consulta_Productos_Requisicion_Parcial(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Detalles_Requisicion
        ///DESCRIPCIÓN:          Método utilizado para consultar los detalles de la requisición
        ///PARAMETROS:   
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Detalles_Requisicion()
        {
            return Cls_Ope_Com_Alm_Requisiciones_Parciales_Datos.Consulta_Detalles_Requisicion(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consultar_DataTable
        ///DESCRIPCIÓN:          Método que manda llamar a su metodo correspondiente de la capa de datos
        ///PARAMETROS:           para consultar las dependencias, areas, etc.
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consultar_DataTable()
        {
            return Cls_Ope_Com_Alm_Requisiciones_Parciales_Datos.Consultar_DataTable(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Pragrama_Financiamiento
        ///DESCRIPCIÓN:          Metodo que manda llamar a su método correspondiente de la capa de datos
        ///PARAMETROS:           Para consultar su Programa Financiamiento
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           23/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Pragrama_Financiamiento()
        {
            return Cls_Ope_Com_Alm_Requisiciones_Parciales_Datos.Consulta_Pragrama_Financiamiento(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Liberar_Requisiciones
        ///DESCRIPCIÓN:          Metodo que manda llamar a su método correspondiente de la capa de datos
        ///PARAMETROS:           Para liberar las requisiciones seleccionadas
        ///CREO:                 Salvador Hernández Ramírez
        ///FECHA_CREO:           27/Junio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public String Cancelar_Requisicion()
        {
            return Cls_Ope_Com_Alm_Requisiciones_Parciales_Datos.Cancelar_Requisicion(this);
        }

        #endregion

    }
}