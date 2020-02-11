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
using Presidencia.Reporte_Requisiciones.Datos;

/// <summary>
/// Summary description for Cls_Ope_Reporte_Requisiciones_Negocio
/// </summary>
namespace Presidencia.Reporte_Requisiciones.Negocios
{

    public class Cls_Rpt_Com_Reporte_Requisiciones_Negocio
    {
        ///*******************************************************************
        ///VARIABLES INTERNAS
        ///*******************************************************************
        #region Variables Internas

        private String No_Requisicion;
        private String Tipo_Compra;
        private String Estatus;
        private String Tipo_Articulo;
        private String Estatus_Busqueda;
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Area_ID;
        private String Dependencia_ID;
        private String Folio_Busqueda;

        
        

        #endregion
        ///*******************************************************************
        ///VARIABLES PUBLICAS
        ///*******************************************************************
        #region Variables Publicas

        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        public String P_Tipo_Compra
        {
            get { return Tipo_Compra; }
            set { Tipo_Compra = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }
        public String P_Estatus_Busqueda
        {
            get { return Estatus_Busqueda; }
            set { Estatus_Busqueda = value; }
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
      
        public String P_Area_ID
        {
            get { return Area_ID; }
            set { Area_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Folio_Busqueda
        {
            get { return Folio_Busqueda; }
            set { Folio_Busqueda = value; }
        } 
        #endregion
        ///*******************************************************************
        ///VARIABLES METODOS
        ///*******************************************************************
        #region Metodos 
        
        public DataTable Consultar_Requisiciones()
        {
            return Cls_Rpt_Com_Reporte_Requisiciones_Datos.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Productos()
        {
            return Cls_Rpt_Com_Reporte_Requisiciones_Datos.Consultar_Productos(this);
        }

        public DataTable Consultar_Comentarios()
        {
            return Cls_Rpt_Com_Reporte_Requisiciones_Datos.Consultar_Comentarios(this);
        }
        
        public DataTable Consultar_Detalle_Compra()
        {
            return Cls_Rpt_Com_Reporte_Requisiciones_Datos.Consultar_Detalle_Compra(this);
        }


        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Areas
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de consultar areas de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Areas()
        {
            return Cls_Rpt_Com_Reporte_Requisiciones_Datos.Consultar_Areas(this);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Consulta_Dependencias
        ///DESCRIPCIÓN: Metodo que manda llamar el metodo de consultar areas de la clase de datos
        ///PARAMETROS:   
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 10/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public DataTable Consulta_Dependencias()
        {
            return Cls_Rpt_Com_Reporte_Requisiciones_Datos.Consultar_Dependencias(this);
        }
        #endregion


    }
}