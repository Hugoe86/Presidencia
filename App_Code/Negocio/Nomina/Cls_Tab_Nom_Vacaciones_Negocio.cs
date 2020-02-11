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
using Presidencia.Vacaciones.Datos;

/// <summary>
/// Summary description for Cls_Tab_Nom_Vacaciones_Negocio
/// </summary>
namespace Presidencia.Vacaciones.Negocios
{
    public class Cls_Tab_Nom_Vacaciones_Negocio
    {
        public Cls_Tab_Nom_Vacaciones_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Vacacion_ID;
        private int Antiguedad;
        private int Dias;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Vacacion_ID
        {
            get { return Vacacion_ID; }
            set { Vacacion_ID = value; }
        }

        public int P_Antiguedad
        {
            get { return Antiguedad; }
            set { Antiguedad = value; }
        }

        public int P_Dias
        {
            get { return Dias; }
            set { Dias = value; }
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
        public void Alta_Vacacion()
        {
            Cls_Tab_Nom_Vacaciones_Datos.Alta_Vacacion(this);
        }
        public void Modificar_Vacacion()
        {
            Cls_Tab_Nom_Vacaciones_Datos.Modificar_Vacacion(this);
        }
        public void Eliminar_Vacacion()
        {
            Cls_Tab_Nom_Vacaciones_Datos.Eliminar_Vacacion(this);
        }
        public DataTable Consulta_Datos_Vacaciones()
        {
            return Cls_Tab_Nom_Vacaciones_Datos.Consulta_Datos_Vacaciones(this);
        }
        #endregion
    }
}