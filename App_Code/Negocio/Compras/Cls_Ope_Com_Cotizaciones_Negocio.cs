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
using Presidencia.Cotizaciones_Compras.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Cotizaciones_Negocio
/// </summary>
namespace Presidencia.Cotizaciones_Compras.Negocio
{
    public class Cls_Ope_Com_Cotizaciones_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///*******************************************************************************
        #region Variables_Internas
        private String No_Cotizacion;
        private String Folio;
        private String Estatus;
        private String Tipo;
        private String Condiciones;
        private String Monto_Total;
        private String Usuario;
        private String Usuario_ID;
        private String Lista_Requisiciones;
        private String Lista_Consolidaciones;
        private String No_Requisicion;
        private String No_Consolidacion;
        private String Listado_Almacen;

     
        Cls_Ope_Com_Cotizaciones_Datos Datos_Cotizaciones;
        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_No_Cotizacion
        {
            get { return No_Cotizacion; }
            set { No_Cotizacion = value; }
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
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Condiciones
        {
            get { return Condiciones; }
            set { Condiciones = value; }
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
        public String P_Lista_Requisiciones
        {
            get { return Lista_Requisiciones; }
            set { Lista_Requisiciones = value; }
        }
        public String P_Lista_Consolidaciones
        {
            get { return Lista_Consolidaciones; }
            set { Lista_Consolidaciones = value; }
        }
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
        }
        public String P_No_Consolidacion
        {
            get { return No_Consolidacion; }
            set { No_Consolidacion = value; }
        }

        public String P_Listado_Almacen
        {
            get { return Listado_Almacen; }
            set { Listado_Almacen = value; }
        }
       

        #endregion


        public Cls_Ope_Com_Cotizaciones_Negocio()
        {
            Datos_Cotizaciones = new Cls_Ope_Com_Cotizaciones_Datos();
        }

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************

        public DataTable Consultar_Cotizaciones()
        {
            return Datos_Cotizaciones.Consultar_Cotizaciones(this);
        }

        public DataTable Consulta_Consolidaciones()
        {
            return Datos_Cotizaciones.Consulta_Consolidaciones(this);
        }

        public DataTable Consultar_Requisiciones()
        {
            return Datos_Cotizaciones.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Comite_Detalle_Requisicion()
        {
            return Datos_Cotizaciones.Consultar_Comite_Detalle_Requisicion(this);
        }

        public DataTable Consultar_Detalle_Consolidacion()
        {
            return Datos_Cotizaciones.Consultar_Detalle_Consolidacion(this);
        }

        public void Alta_Cotizacion()
        {
            Datos_Cotizaciones.Alta_Cotizacion(this);
        }

        public void Modificar_Cotizaciones()
        {
            Datos_Cotizaciones.Modificar_Cotizacion(this);
        }

        
    }
}//fin namespace