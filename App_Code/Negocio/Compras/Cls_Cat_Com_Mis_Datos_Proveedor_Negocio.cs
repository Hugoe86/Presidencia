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
using Presidencia.Mis_Datos_Proveedor.Datos;

/// <summary>
/// Summary description for Cls_Cat_Com_Mis_Datos_Proveedor_Negocio
/// </summary>
namespace Presidencia.Mis_Datos_Proveedor.Negocio
{
    public class Cls_Cat_Com_Mis_Datos_Proveedor_Negocio
    {

        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String Proveedor_ID;

        

        #endregion

        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }

        #endregion
        ///*******************************************************************************
        ///METODOS
        ///******************************************************************************

        #region Metodos

        public DataTable Consultar_Datos_Proveedor()
        {
            return Cls_Cat_Com_Mis_Datos_Proveedor_Datos.Consultar_Datos_Proveedor(this);
        }

        public DataTable Consultar_Conceptos_Proveedor()
        {
            return Cls_Cat_Com_Mis_Datos_Proveedor_Datos.Consultar_Conceptos_Proveedor(this);
        }

        #endregion


    }//fin del class
}//fin del namespace