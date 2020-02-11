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
using Presidencia.Pass_Proveedor.Datos;

/// <summary>
/// Summary description for Cls_Ope_Cambiar_Pass_Prov_Negocio
/// </summary>
/// 
namespace Presidencia.Pass_Proveedor.Negocio
{
    public class Cls_Ope_Cambiar_Pass_Prov_Negocio
    {
        ///*******************************************************************************
        /// VARIABLES INTERNAS 
        ///******************************************************************************
        #region Variables_Internas
        private String Num_Proveedor;
        private String Password_Nuevo;

        #endregion


        ///*******************************************************************************
        /// VARIABLES PUBLICAS
        ///*******************************************************************************
        #region Variables_Publicas

        public String P_Num_Proveedor
        {
            get { return Num_Proveedor; }
            set { Num_Proveedor = value; }
        }

        public String P_Password_Nuevo
        {
            get { return Password_Nuevo; }
            set { Password_Nuevo = value; }
        }

        #endregion

        ///*******************************************************************************
        /// METODOS
        ///*******************************************************************************
        #region Metodos


        public DataTable Consultar_Proveedor()
        {
            return Cls_Ope_Cambiar_Pass_Prov_Datos.Consultar_Proveedor(this);
        }

        public String Modificar_Password_Proveedor()
        {
            return Cls_Ope_Cambiar_Pass_Prov_Datos.Modificar_Password_Proveedor(this);
        }

        #endregion

    }
}//fin del namespace
