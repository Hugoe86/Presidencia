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
using Presidencia.Definir_Cotizadores.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Definir_Cotizadores_Negocio
/// </summary>

namespace Presidencia.Definir_Cotizadores.Negocio
{
    public class Cls_Ope_Com_Definir_Cotizadores_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas

        private DataTable Dt_Productos;
        private DataTable Dt_Servicios;
        private DataTable Dt_Requisiciones;
        private String No_Requisicion;
        private String Ope_Com_Req_Producto_ID;
        private String Tipo_Articulo;
        private bool Reasignar;

        public bool P_Reasignar
        {
            get { return Reasignar; }
            set { Reasignar = value; }
        }


        #endregion
        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public DataTable P_Dt_Requisiciones
        {
            get { return Dt_Requisiciones; }
            set { Dt_Requisiciones = value; }
        }

        public DataTable P_Dt_Servicios
        {
            get { return Dt_Servicios; }
            set { Dt_Servicios = value; }
        }

        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        public String P_Ope_Com_Req_Producto_ID
        {
            get { return Ope_Com_Req_Producto_ID; }
            set { Ope_Com_Req_Producto_ID = value; }
        }

        public String P_Tipo_Articulo
        {
            get { return Tipo_Articulo; }
            set { Tipo_Articulo = value; }
        }

        public DataTable P_Dt_Productos
        {
            get { return Dt_Productos; }
            set { Dt_Productos = value; }
        }
        #endregion
        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Productos_Servicios()
        {
            return Cls_Ope_Com_Definir_Cotizadores_Datos.Consultar_Productos_Servicios(this);

        }



        public DataTable Consultar_Partidas_Especificas()
        {
            return Cls_Ope_Com_Definir_Cotizadores_Datos.Consultar_Partidas_Especificas(this);
        }

        public DataTable Consultar_Cotizadores()
        {
            return Cls_Ope_Com_Definir_Cotizadores_Datos.Consultar_Cotizadores();
        }

        public DataTable Consultar_Requisiciones()
        {
            return Cls_Ope_Com_Definir_Cotizadores_Datos.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Detalle_Requisicion()
        {
            return Cls_Ope_Com_Definir_Cotizadores_Datos.Consultar_Detalle_Requisicion(this);
        }

        public bool Alta_Cotizadores_Asignados()
        {
            return Cls_Ope_Com_Definir_Cotizadores_Datos.Alta_Cotizadores_Asignados(this);
        }

        public bool Reasignar_Cotizadores()
        {
            return Cls_Ope_Com_Definir_Cotizadores_Datos.Reasignar_Cotizadores(this);
        }
        #endregion


    }
}