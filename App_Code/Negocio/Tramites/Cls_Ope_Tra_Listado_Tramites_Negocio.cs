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
using Presidencia.Listado_Tramites.Datos;
/// <summary>
/// Summary description for Cls_Ope_Tra_Listado_Tramites_Negocio
/// </summary>
/// 
namespace Presidencia.Listado_Tramites.Negocio
{
    public class Cls_Ope_Tra_Listado_Tramites_Negocio
    {
        #region VARIABLES PRIVADAS
        private String Dependencia_ID;
        private String Tramite_ID;
        #endregion

        #region VARIABLES PUBLICAS
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Tramite_ID
        {
            get { return Tramite_ID; }
            set { Tramite_ID = value; }
        }
        #endregion

        #region MÉTODOS
        public Cls_Ope_Tra_Listado_Tramites_Negocio()
        {
            //
            // TODO: Add constructor logic here
            //
        }
        public DataTable Consultar_Tramites()
        {
            return Cls_Ope_Tra_Listado_Tramites_Datos.Consultar_Tramites(this);
        }
        public DataTable Consultar_Unidades_Responsables()
        {
            return Cls_Ope_Tra_Listado_Tramites_Datos.Consultar_Unidades_Responsables(this);
        }

        #endregion
    }
}