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
using Presidencia.Catalogo_Tramites_Parametros.Datos;

namespace Presidencia.Catalogo_Tramites_Parametros.Negocio
{
    public class Cls_Cat_Tra_Parametros_Negocio
    {
        #region Variables Privadas
        private String Correo_Encabezado;
        private String Correo_Cuerpo;
        private String Correo_Despedida;
        private String Correo_Firma;
        private String Usuario;
        #endregion

        #region Variables publicas
        public String P_Correo_Encabezado
        {
            get { return Correo_Encabezado; }
            set { Correo_Encabezado = value; }
        }
        public String P_Correo_Cuerpo
        {
            get { return Correo_Cuerpo; }
            set { Correo_Cuerpo = value; }
        }
        public String P_Correo_Despedida
        {
            get { return Correo_Despedida; }
            set { Correo_Despedida = value; }
        }
        public String P_Correo_Firma
        {
            get { return Correo_Firma; }
            set { Correo_Firma = value; }
        }
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region Metodos
        public int Actualizar_Parametros()
        {
            return Cls_Cat_Tra_Parametros_Datos.Actualizar_Parametros(this);
        }
        public DataTable Consultar_Parametros()
        {
            return Cls_Cat_Tra_Parametros_Datos.Consultar_Parametros(this);
        }
        #endregion
    }
}
