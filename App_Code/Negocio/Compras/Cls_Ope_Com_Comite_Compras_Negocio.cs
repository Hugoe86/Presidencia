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
using Presidencia.Comite_Compras.Datos;


/// <summary>
/// Summary description for Cls_Ope_Com_Comite_Compras_Negocio
/// </summary>
namespace Presidencia.Comite_Compras.Negocio
{

   
    public class Cls_Ope_Com_Comite_Compras_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///*******************************************************************************
        
        #region Variables_Internas
        private String No_Comite_Compras;
        private String Folio;     
        private String Estatus;
        private String Tipo;
        private String Justificacion;
        private String Comentarios;
        private String Monto_Total;
        private String Usuario;
        private String Usuario_ID;
        private String Lista_Requisiciones;
        private String Lista_Consolidaciones;

       
        private String No_Requisicion;
        private String No_Consolidacion;
        private Cls_Ope_Com_Comite_Compras_Datos Datos_Compras;


        #endregion Fin_Variables_Internas

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************

        #region Variables_Publicas

        public String P_No_Comite_Compras
        {
            get { return No_Comite_Compras; }
            set { No_Comite_Compras = value; }
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
        public String P_Justificacion
        {
            get { return Justificacion; }
            set { Justificacion = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
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

        #endregion Variables_Publicas

        

        public Cls_Ope_Com_Comite_Compras_Negocio()
        {
            Datos_Compras = new Cls_Ope_Com_Comite_Compras_Datos();
        }

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        #region Medodo de Consultas
        public DataTable Consulta_Comite_Compras()
        {
            return Datos_Compras.Consulta_Comite_Compras(this);
        }
        public DataTable Consulta_Consolidaciones()
        {
            return Datos_Compras.Consulta_Consolidaciones(this);
        }

        public DataTable Consultar_Requisiciones()
        {
            return Datos_Compras.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Comite_Detalle_Requisicion()
        {
            return Datos_Compras.Consultar_Comite_Detalle_Requisicion(this);
        }

        public DataTable Consultar_Detalle_Consolidacion()
        {
            return Datos_Compras.Consultar_Detalle_Consolidacion(this);
        }

        #endregion Fin Metodo de Consultas


        public void Alta_Comite_Compras()
        {
            Datos_Compras.Alta_Comite_Compras(this);
        }

        public void Modificar_Comite_Compras()
        {
            Datos_Compras.Modificar_Comite_Compras(this);
        }
        #endregion Fin_Metodos
    }
}