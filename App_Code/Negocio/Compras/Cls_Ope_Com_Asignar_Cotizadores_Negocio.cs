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
using Presidencia.Asignar_Cotizadores.Datos;


/// <summary>
/// Summary description for Cls_Ope_Com_Asignar_Cotizadores_Negocio
/// </summary>
namespace Presidencia.Asignar_Cotizadores.Negocio
{

    public class Cls_Ope_Com_Asignar_Cotizadores_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///*******************************************************************************
        #region Variables_Internas

        private String No_Cotizacion;
        private String Folio;
        private String Estatus;
        private String Concepto_ID;
        private String Cotizador_ID;
        private DataTable Dt_Cotizadores;
        private String Listado_Requisiciones;

       
        Cls_Ope_Com_Asignar_Cotizadores_Datos Datos_Cotizador;
        #endregion Fin Variables_Internas

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

        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

        public String P_Cotizador_ID
        {
            get { return Cotizador_ID; }
            set { Cotizador_ID = value; }
        }
        public DataTable P_Dt_Cotizadores
        {
            get { return Dt_Cotizadores; }
            set { Dt_Cotizadores = value; }
        }

        public String P_Listado_Requisiciones
        {
            get { return Listado_Requisiciones; }
            set { Listado_Requisiciones = value; }
        }
        #endregion Fin Variables_Publicas 

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos
        
        public Cls_Ope_Com_Asignar_Cotizadores_Negocio()
        {
            Datos_Cotizador = new Cls_Ope_Com_Asignar_Cotizadores_Datos();
        }

        public void Modificar_Cotizacion()
        {
            Datos_Cotizador.Modificar_Cotizacion(this);
        }

        public DataTable Consultar_Cotizaciones()
        {
            return Datos_Cotizador.Consultar_Cotizaciones(this);
        }

        public DataTable Consultar_Conceptos()
        {
            return Datos_Cotizador.Consultar_Conceptos(this);
        }

        public DataTable Consultar_Detalle_Cotizaciones()
        {
            return Datos_Cotizador.Consultar_Detalle_Cotizaciones(this);
        }
        public DataTable Consultar_Cotizadores()
        {
            return Datos_Cotizador.Consultar_Cotizadores(this);
        }

        #endregion Fin Metodos

    }//fin class
}//fin namespace