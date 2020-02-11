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
using Presidencia.Catalogo_Cat_Construccion_Dominante.Datos;

/// <summary>
/// Summary description for Cls_Cat_Cat_Construccion_Dominante_Negocio
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Construccion_Dominante.Negocio
{
    public class Cls_Cat_Cat_Construccion_Dominante_Negocio
    {
        #region Variables Internas

        private String Construccion_Dominante_ID;
        private String Construccion_Dominante;
        private String Estatus;

        #endregion

        #region Variables Publicas

        public String P_Construccion_Dominante_ID
        {
            get { return Construccion_Dominante_ID; }
            set { Construccion_Dominante_ID = value; }
        }

        public String P_Construccion_Dominante
        {
            get { return Construccion_Dominante; }
            set { Construccion_Dominante = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        #endregion

        #region Metodos

        public Boolean Alta_Construccion_Dominante()
        {
            return Cls_Cat_Cat_Construccion_Dominante_Datos.Alta_Construccion_Dominante(this);
        }

        public Boolean Modificar_Construccion_Dominante()
        {
            return Cls_Cat_Cat_Construccion_Dominante_Datos.Modificar_Construccion_Dominante(this);
        }

        public DataTable Consultar_Construccion_Dominante()
        {
            return Cls_Cat_Cat_Construccion_Dominante_Datos.Consultar_Construccion_Dominante(this);
        }

        #endregion
    }
}