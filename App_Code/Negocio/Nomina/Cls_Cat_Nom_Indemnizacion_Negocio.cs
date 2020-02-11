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
using Presidencia.Indemnizacion.Datos;

namespace Presidencia.Indemnizacion.Negocio
{
    public class Cls_Cat_Nom_Indemnizacion_Negocio
    {
        #region (Variables Publicas)
        private String Indemnizacion_ID;
        private String Nombre;
        private String Dias;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region (Variables Publicas)
        public String P_Indemnizacion_ID {
            get { return Indemnizacion_ID; }
            set { Indemnizacion_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Dias {
            get { return Dias; }
            set { Dias = value; }
        }

        public String P_Comentarios {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Usuario_Creo {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }

        public String P_Usuario_Modifico {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        #endregion 

        #region (Variables Publicas)
        public Boolean Alta_Indemnizacion() {
            return Cls_Cat_Nom_Indemnizacion_Datos.Alta_Indemnizacion(this);
        }

        public Boolean Actualizar_Indemnizacion()
        {
            return Cls_Cat_Nom_Indemnizacion_Datos.Actualizar_Indemnizacion(this);
        }

        public Boolean Eliminar_Indemnizacion()
        {
            return Cls_Cat_Nom_Indemnizacion_Datos.Eliminar_Indemnizacion(this);
        }

        public DataTable Consultar_Indemnizaciones()
        {
            return Cls_Cat_Nom_Indemnizacion_Datos.Consultar_Indemnizacion(this);
        }
        #endregion

    }
}
