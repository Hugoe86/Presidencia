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
using Presidencia.Catalogo_Cat_Claves_Catastrales.Datos;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;

/// <summary>
/// Summary description for Cls_Cat_Cat_Claves_Catastrales
/// </summary>
/// 
namespace Presidencia.Catalogo_Cat_Claves_Catastrales.Negocio
{
    public class Cls_Cat_Cat_Claves_Catastrales_Negocio
    {
        #region Variables Internas
        private String Claves_Catastrales_ID;
        private String Identificador;
        private String Estatus;
        private String Tipo;
        #endregion Variables Internas

        #region Variables Publicas
        public String P_Claves_Catastrales_ID
        {
            get { return Claves_Catastrales_ID; }
            set { Claves_Catastrales_ID = value; }

        }

        public String P_Identificador
        {
            get { return Identificador; }
            set { Identificador = value; }
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
        #endregion Variables Publicas

        #region Metodos
        public Boolean Alta_Claves_Catastrales()
        {
            return Cls_Cat_Cat_Claves_Catastrales_Datos.Alta_Claves_Catastrales(this);
        }
        public Boolean Modificar_Claves_Catastrales()
        {
            return Cls_Cat_Cat_Claves_Catastrales_Datos.Modificar_Claves_Catastrales(this);
        }
        public DataTable Consultar_Claves_Catastrales()
        {
            return Cls_Cat_Cat_Claves_Catastrales_Datos.Consultar_Claves_Catastrales(this);

        }
        #endregion Metodos
    }
}