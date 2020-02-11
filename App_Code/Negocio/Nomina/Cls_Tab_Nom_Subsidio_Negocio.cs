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
using Presidencia.Subsidio.Datos;

/// <summary>
/// Summary description for Cls_Tab_Nom_Subsidio
/// </summary>
namespace Presidencia.Subsidio.Negocios
{
    public class Cls_Tab_Nom_Subsidio_Negocio
    {
        public Cls_Tab_Nom_Subsidio_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Subsidio_ID;
        private double Limite_Inferior;
        private double Subsidio;
        private String Tipo_Nomina;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Subsidio_ID
        {
            get { return Subsidio_ID; }
            set { Subsidio_ID = value; }
        }

        public double P_Limite_Inferior
        {
            get { return Limite_Inferior; }
            set { Limite_Inferior = value; }
        }

        public double P_Subsidio
        {
            get { return Subsidio; }
            set { Subsidio = value; }
        }

        public String P_Tipo_Nomina
        {
            get { return Tipo_Nomina; }
            set { Tipo_Nomina = value; }
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
        public void Alta_Subsidio()
        {
            Cls_Tab_Nom_Subsidio_Datos.Alta_Subsidio(this);
        }
        public void Modificar_Subsidio()
        {
            Cls_Tab_Nom_Subsidio_Datos.Modificar_Subsidio(this);
        }
        public void Eliminar_Subsidio()
        {
            Cls_Tab_Nom_Subsidio_Datos.Eliminar_Subsidio(this);
        }
        public DataTable Consulta_Datos_Subsidio()
        {
            return Cls_Tab_Nom_Subsidio_Datos.Consulta_Datos_Subsidio(this);
        }
        #endregion
    }
}