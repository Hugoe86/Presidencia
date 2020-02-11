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
using Presidencia.Escolaridad.Datos;

/// <summary>
/// Summary description for Cls_Cat_Nom_Escolaridad
/// </summary>
namespace Presidencia.Escolaridad.Negocios
{
    public class Cls_Cat_Nom_Escolaridad_Negocio
    {
        public Cls_Cat_Nom_Escolaridad_Negocio()
        {
        }
        #region (Variables Internas)
        private String Escolaridad_ID;
        private String Escolaridad;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Escolaridad_ID
        {
            get { return Escolaridad_ID; }
            set { Escolaridad_ID = value; }
        }

        public String P_Escolaridad
        {
            get { return Escolaridad; }
            set { Escolaridad = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        #endregion
        #region (Metodos)
        public void Alta_Escolaridad()
        {
            Cls_Cat_Nom_Escolaridad_Datos.Alta_Escolaridad(this);
        }
        public void Modificar_Escolaridad()
        {
            Cls_Cat_Nom_Escolaridad_Datos.Modificar_Escolaridad(this);
        }
        public void Eliminar_Escolaridad()
        {
            Cls_Cat_Nom_Escolaridad_Datos.Eliminar_Escolaridad(this);
        }
        public DataTable Consulta_Datos_Escolaridad()
        {
            return Cls_Cat_Nom_Escolaridad_Datos.Consulta_Datos_Escolaridad(this);
        }
        public DataTable Consulta_Escolaridad()
        {
            return Cls_Cat_Nom_Escolaridad_Datos.Consulta_Escolaridad(this);
        }
        #endregion
    }
}