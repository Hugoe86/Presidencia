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
using Presidencia.INFONAVIT.Datos;

/// <summary>
/// Summary description for Cls_Tab_Nom_INFONAVIT_Negocio
/// </summary>
namespace Presidencia.INFONAVIT.Negocios
{
    public class Cls_Tab_Nom_INFONAVIT_Negocio
    {
        public Cls_Tab_Nom_INFONAVIT_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String INFONAVIT_ID;
        private Double Veces_SMGA;
        private Double Porcentaje;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_INFONAVIT_ID
        {
            get { return INFONAVIT_ID; }
            set { INFONAVIT_ID = value; }
        }

        public Double P_Veces_SMGA
        {
            get { return Veces_SMGA; }
            set { Veces_SMGA = value; }
        }

        public Double P_Porcentaje
        {
            get { return Porcentaje; }
            set { Porcentaje = value; }
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
        public void Alta_INFONAVIT()
        {
            Cls_Tab_Nom_INFONAVIT_Datos.Alta_INFONAVIT(this);
        }
        public void Modificar_INFONAVIT()
        {
            Cls_Tab_Nom_INFONAVIT_Datos.Modificar_INFONAVIT(this);
        }
        public void Eliminar_INFONAVIT()
        {
            Cls_Tab_Nom_INFONAVIT_Datos.Eliminar_INFONAVIT(this);
        }
        public DataTable Consulta_Datos_INFONAVIT()
        {
            return Cls_Tab_Nom_INFONAVIT_Datos.Consulta_Datos_INFONAVIT(this);
        }
        #endregion
    }
}