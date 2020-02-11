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
using Presidencia.Tipo_Trabajador.Datos;

/// <summary>
/// Summary description for Cls_Cat_Nom_Tipo_Trabajador_Negocio
/// </summary>
namespace Presidencia.Tipo_Trabajador.Negocios
{
    public class Cls_Cat_Nom_Tipo_Trabajador_Negocio
    {
        public Cls_Cat_Nom_Tipo_Trabajador_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Tipo_Trabajador_ID;
        private String DESCRIPCION;
        private String Estatus;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        //Manejo de propiedades
        public String P_Tipo_Trabajador_ID
        {
            get { return Tipo_Trabajador_ID; }
            set { Tipo_Trabajador_ID = value; }
        }

        public String P_Descripcion
        {
            get { return DESCRIPCION; }
            set { DESCRIPCION = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
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
        #region(Metodos)
        //Metodos
        public void Alta_Tipo_Trabajador()
        {
            Cls_Cat_Nom_Tipo_Trabajador_Datos.Alta_Tipo_Trabajador(this);
        }

        public void Elimina_Tipo_Trabajor()
        {
            Cls_Cat_Nom_Tipo_Trabajador_Datos.Elimina_Tipo_Trabajador(this);
        }

        public void Modifica_Tipo_Trabajador()
        {
            Cls_Cat_Nom_Tipo_Trabajador_Datos.Modifica_Tipo_Trabajador(this);
        }

        public DataTable Consulta_Tipo_Trabajador()
        {
            return Cls_Cat_Nom_Tipo_Trabajador_Datos.Consulta_Tipos_Trabajadores(this);
        }
        #endregion
    }
}