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
using Presidencia.Tipos_Contratos.Datos;
/// <summary>
/// Summary description for Cls_Cat_Nom_Tipos_Contratos_Negocio
/// </summary>
namespace Presidencia.Tipos_Contratos.Negocios
{
    public class Cls_Cat_Nom_Tipos_Contratos_Negocio
    {
        public Cls_Cat_Nom_Tipos_Contratos_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Tipo_Contrato_ID;
        private String DESCRIPCION;
        private String Estatus;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Tipo_Contrato_ID
        {
            get { return Tipo_Contrato_ID; }
            set { Tipo_Contrato_ID = value; }
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
        #region (Metodos)
        public void Alta_Tipo_Contrato()
        {
            Cls_Cat_Nom_Tipos_Contratos_Datos.Alta_Tipo_Contrato(this);
        }
        public void Modificar_Tipo_Contrato()
        {
            Cls_Cat_Nom_Tipos_Contratos_Datos.Modificar_Tipo_Contrato(this);
        }
        public void Eliminar_Tipo_Contrato()
        {
            Cls_Cat_Nom_Tipos_Contratos_Datos.Eliminar_Tipo_Contrato(this);
        }
        public DataTable Consulta_Datos_Tipo_Contrato()
        {
            return Cls_Cat_Nom_Tipos_Contratos_Datos.Consulta_Datos_Tipo_Contrato(this);
        }
        public DataTable Consulta_Tipos_Contratos()
        {
            return Cls_Cat_Nom_Tipos_Contratos_Datos.Consulta_Tipos_Contratos(this);
        }
        #endregion
    }
}