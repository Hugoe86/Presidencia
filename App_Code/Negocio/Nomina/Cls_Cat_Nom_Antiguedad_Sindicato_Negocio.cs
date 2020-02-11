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
using Presidencia.Antiguedad_Sindicato.Datos;

namespace Presidencia.Antiguedad_Sindicato.Negocio
{
    public class Cls_Cat_Nom_Antiguedad_Sindicato_Negocio
    {
        #region (Variables Internas)
        private String Antiguedad_Sindicato_ID;
        private Int32 Anios;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region (Variables Publicas)
        public String P_Antiguedad_Sindicato_ID
        {
            get { return Antiguedad_Sindicato_ID; }
            set { Antiguedad_Sindicato_ID = value; }
        }

        public Int32 P_Anios
        {
            get { return Anios; }
            set { Anios = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
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
        public Boolean Alta_Antiguedad_Sindicato() {
            return Cls_Cat_Nom_Antiguedad_Sindicato_Datos.Alta_Antiguedad_Sindicato(this);
        }
        public Boolean Modificar_Antiguedad_Sindicato()
        {
            return Cls_Cat_Nom_Antiguedad_Sindicato_Datos.Modificar_Antiguedad_Sindicato(this);
        }
        public Boolean Eliminar_Antiguedad_Sindicato()
        {
            return Cls_Cat_Nom_Antiguedad_Sindicato_Datos.Eliminar_Antiguedad_Sindicato(this);
        }
        public DataTable Consultar_Antiguedad_Sindicato()
        {
            return Cls_Cat_Nom_Antiguedad_Sindicato_Datos.Consultar_Antiguedad_Sindicato(this);
        }
        #endregion
    }
}
