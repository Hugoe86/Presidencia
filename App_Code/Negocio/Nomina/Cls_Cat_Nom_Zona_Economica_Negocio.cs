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
using Presidencia.Zona_Economica.Datos;
/// <summary>
/// Summary description for Cls_Cat_Zona_Economica_Negocio
/// </summary>
namespace Presidencia.Zona_Economica.Negocios
{
    public class Cls_Cat_Nom_Zona_Economica_Negocio
    {
        public Cls_Cat_Nom_Zona_Economica_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Zona_ID;
        private String Zona_Economica;
        private Double Salario_Diario;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Zona_ID
        {
            get { return Zona_ID; }
            set { Zona_ID = value; }
        }

        public String P_Zona_Economica
        {
            get { return Zona_Economica; }
            set { Zona_Economica = value; }
        }

        public Double P_Salario_Diario
        {
            get { return Salario_Diario; }
            set { Salario_Diario = value; }
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
        public void Alta_Zona_Economica()
        {
            Cls_Cat_Nom_Zona_Economica_Datos.Alta_Zona_Economica(this);
        }
        public void Modificar_Zona_Economica()
        {
            Cls_Cat_Nom_Zona_Economica_Datos.Modificar_Zona_Economica(this);
        }
        public void Eliminar_Zona_Economica()
        {
            Cls_Cat_Nom_Zona_Economica_Datos.Eliminar_Zona_Economica(this);
        }
        public DataTable Consulta_Datos_Zona_Economica()
        {
            return Cls_Cat_Nom_Zona_Economica_Datos.Consulta_Datos_Zona_Economica(this);
        }
        public DataTable Consulta_Zona_Economica()
        {
            return Cls_Cat_Nom_Zona_Economica_Datos.Consulta_Zona_Economica(this);
        }
        #endregion
    }
}