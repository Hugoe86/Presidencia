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
using Presidencia.Recotizar.Datos;

/// <summary>
/// Summary description for Cls_Ope_Com_Recotizar_Requisicion_Negocio
/// </summary>
/// 
namespace Presidencia.Recotizar.Negocio
{

    public class Cls_Ope_Com_Recotizar_Requisicion_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS
        ///*******************************************************************************
        #region Variables_Internas
        private String No_Requisicion;
        private String Folio;
        private String Estatus;

       


        #endregion


        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas
        public String P_No_Requisicion
        {
            get { return No_Requisicion; }
            set { No_Requisicion = value; }
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
        #endregion


        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos

        public DataTable Consultar_Requisiciones()
        {
            return Cls_Ope_Com_Recotizar_Requisicion_Datos.Consultar_Requisiciones(this);
        }

        public DataTable Consultar_Productos_Servicios()
        {
            return Cls_Ope_Com_Recotizar_Requisicion_Datos.Consultar_Productos_Servicios(this);

        }

        public String Modificar_Requisicion()
        {
            return Cls_Ope_Com_Recotizar_Requisicion_Datos.Modificar_Requisicion(this);
        }
        #endregion



    }
}