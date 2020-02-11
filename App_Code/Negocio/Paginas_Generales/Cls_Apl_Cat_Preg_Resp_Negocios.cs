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
using Presidencia.Preguntas_Respuestas.Datos;

namespace Presidencia.Preguntas_Respuestas.Negocio
{
    public class Cls_Apl_Cat_Preg_Resp_Negocios
    {
        #region (Variables Privadas)
        private String Preg_Resp_ID;
        private String Pregunta;
        private String Respuesta;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region (Variables Públicas)
        public String P_Preg_Resp_ID
        {
            get { return Preg_Resp_ID; }
            set { Preg_Resp_ID = value; }
        }

        public String P_Pregunta
        {
            get { return Pregunta; }
            set { Pregunta = value; }
        }

        public String P_Respuesta
        {
            get { return Respuesta; }
            set { Respuesta = value; }
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

        #region (métodos)
        public Boolean Alta_Preguntas() {
            return Cls_Apl_Cat_Preg_Resp_Datos.Alta_Preguntas(this);
        }

        public Boolean Modificar_Preguntas()
        {
            return Cls_Apl_Cat_Preg_Resp_Datos.Modificar_Preguntas(this);
        }

        public DataTable Consultar_Preguntas()
        {
            return Cls_Apl_Cat_Preg_Resp_Datos.Consultar_Preguntas(this);
        }
        #endregion
    }
}
