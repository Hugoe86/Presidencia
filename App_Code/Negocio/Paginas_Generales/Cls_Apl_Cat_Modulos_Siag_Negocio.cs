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
using Presidencia.Modulos_SIAG.Datos;

namespace Presidencia.Modulos_SIAG.Negocio
{

    public class Cls_Apl_Cat_Modulos_Siag_Negocio
    {
        #region(Variables Privadas)
        private String Modulo_ID;
        private String Nombre;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region(Variables Publicas)
        public String P_Modulo_ID
        {
            get { return Modulo_ID; }
            set { Modulo_ID = value; }
        }
        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        #endregion

        #region(Metodos)
        public Boolean Alta_Modulo_Siag()
        {
            return Cls_Apl_Cat_Modulos_Siag_Datos.Alta_Modulo_Siag(this);
        }
        public Boolean Modificar_Modulo_Siag()
        {
            return Cls_Apl_Cat_Modulos_Siag_Datos.Modificar_Modulo_Siag(this);
        }
        public Boolean Eliminar_Modulo_Siag()
        {
            return Cls_Apl_Cat_Modulos_Siag_Datos.Eliminar_Modulo_Siag(this);
        }
        public DataTable Consultar_Modulo_Siag()
        {
            return Cls_Apl_Cat_Modulos_Siag_Datos.Consultar_Modulo_Siag(this);
        }
        #endregion
    }
}