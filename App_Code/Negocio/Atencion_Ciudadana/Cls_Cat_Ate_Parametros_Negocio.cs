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
using Presidencia.Catalogo_Atencion_Ciudadana_Parametros.Datos;

namespace Presidencia.Catalogo_Atencion_Ciudadana_Parametros.Negocio
{
    public class Cls_Cat_Ate_Parametros_Negocio
    {
        public Cls_Cat_Ate_Parametros_Negocio()
        {
        }

        #region Variables Privadas
        private String Programa_ID_Web;
        private String Programa_ID_Ventanilla;
        private String Usuario;
        private String Programa_ID_Genera_Consecutivo;
        private String Programa_ID_Atiende_Direccion;
        #endregion

        #region Variables publicas

        public String P_Programa_ID_Web
        {
            get { return Programa_ID_Web; }
            set { Programa_ID_Web = value; }
        }
        public String P_Programa_ID_Ventanilla
        {
            get { return Programa_ID_Ventanilla; }
            set { Programa_ID_Ventanilla = value; }
        }
        public String P_Programa_ID_Genera_Consecutivo
        {
            get { return Programa_ID_Genera_Consecutivo; }
            set { Programa_ID_Genera_Consecutivo = value; }
        }
        public String P_Programa_ID_Atiende_Direccion
        {
            get { return Programa_ID_Atiende_Direccion; }
            set { Programa_ID_Atiende_Direccion = value; }
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
            return Cls_Cat_Ate_Parametros_Datos.Actualizar_Parametros(this);
        }
        public DataTable Consultar_Parametros()
        {
            return Cls_Cat_Ate_Parametros_Datos.Consultar_Parametros();
        }
        #endregion
    }
}
