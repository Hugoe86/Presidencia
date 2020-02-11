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
using Presidencia.Seguimiento_Listado.Datos;

/// <summary>
/// Summary description for Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio
/// </summary>

namespace Presidencia.Seguimiento_Listado.Negocios
{

    public class Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        
        private String Listado_ID;
        private String Cotizadora;
        //Variable que almacena la fecha inicial de la busqueda avanzada
        private String Fecha_Inicial;
        //Variable que almacena la fecha final de la busqueda avanzada
        private String Fecha_Final;
        //Variable que almacena el ID de la partida
        private String Partida_ID;
        //Variable que almacena el folio a buscar 
        private String Folio_Busqueda;
        private String Requisicion_ID;
        private String Tipo;



      
        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

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

        public String P_Listado_ID
        {
            get { return Listado_ID; }
            set { Listado_ID = value; }
        }
        public String P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }

        public String P_Cotizadora
        {
            get { return Cotizadora; }
            set { Cotizadora = value; }
        }
        public String P_Folio_Busqueda
        {
            get { return Folio_Busqueda; }
            set { Folio_Busqueda = value; }
        }
        public String P_Requisicion_ID
        {
            get { return Requisicion_ID; }
            set { Requisicion_ID = value; }
        }


        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consulta_Listado()
        {
            return Cls_Ope_Alm_Seguimiento_Listado_Almacen_Datos.Consulta_Listado(this);
        }

        public DataTable Consulta_Requisiciones_Listado()
        {
            return Cls_Ope_Alm_Seguimiento_Listado_Almacen_Datos.Consulta_Requisiciones_Listado(this);

        }
        public DataTable Consulta_Detalles_Listado()
        {
            return Cls_Ope_Alm_Seguimiento_Listado_Almacen_Datos.Consulta_Detalles_Listado(this);
        }

        public DataTable Consulta_Listado_Reporte()
        {
            return Cls_Ope_Alm_Seguimiento_Listado_Almacen_Datos.Consulta_Listado_Reporte(this);
        }

        public DataTable Consulta_Listado_Detalles_Reporte()
        {
            return Cls_Ope_Alm_Seguimiento_Listado_Almacen_Datos.Consulta_Listado_Detalles_Reporte(this);
        }
        #endregion

    }
}//Fin del namespace