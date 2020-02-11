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
using Presidencia.Catalogo_Cat_Elementos_Construccion.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Elementos_Construccion_Negocio
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Elementos_Construccion.Negocio
{
    public class Cls_Cat_Cat_Elementos_Construccion_Negocio
    {
        #region Variables Internas

        private String Elemento_Construccion_ID;
        private String Elemento_Construccion;
        private String Estatus;

        #endregion

        #region Variables Publicas

        public String P_Elemento_Construccion_ID
        {
            get { return Elemento_Construccion_ID; }
            set { Elemento_Construccion_ID = value; }
        }

        public String P_Elemento_Construccion
        {
            get { return Elemento_Construccion; }
            set { Elemento_Construccion = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Elemento_Construccion()
        {
            return Cls_Cat_Cat_Elementos_Construccion_Datos.Alta_Elemento_Construccion(this);
        }

        public Boolean Modificar_Elemento_Construccion()
        {
            return Cls_Cat_Cat_Elementos_Construccion_Datos.Modificar_Elemento_Construccion(this);
        }

        public DataTable Consultar_Elemento_Construccion()
        {
            return Cls_Cat_Cat_Elementos_Construccion_Datos.Consultar_Elemento_Construccion(this);
        }

        #endregion
    }
}