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
using Presidencia.Nomina_Tipos_Pago.Datos;

namespace Presidencia.Nomina_Tipos_Pago.Negocio
{
    public class Cls_Cat_Nom_Tipos_Pago_Negocio
    {
        #region (Variables Privadas)
        private String Tipo_Pago_ID;
        private String Nombre;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region (Variables Publicas)
        public String P_Tipo_Pago_ID
        {
            get { return Tipo_Pago_ID; }
            set { Tipo_Pago_ID = value; }
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

        #region (Metodos)
        public Boolean Alta_Tipo_Pago()
        {
            return Cls_Cat_Nom_Tipos_Pago_Datos.Alta_Tipo_Pago(this);
        }
        public Boolean Modificar_Tipo_Pago()
        {
            return Cls_Cat_Nom_Tipos_Pago_Datos.Modificar_Tipo_Pago(this);
        }
        public Boolean Eliminar_Tipo_Pago()
        {
            return Cls_Cat_Nom_Tipos_Pago_Datos.Eliminar_Tipo_Pago(this);
        }
        public DataTable Consultar_Tipo_Pago()
        {
            return Cls_Cat_Nom_Tipos_Pago_Datos.Consultar_Tipo_Pago(this);
        }
        #endregion
    }
}