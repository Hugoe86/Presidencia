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
using System.Collections.Generic;
using Presidencia.Programas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Nom_Programas_Negocio
/// </summary>
namespace Presidencia.Programas.Negocios
{
    public class Cls_Cat_Nom_Programas_Negocio
    {
        public Cls_Cat_Nom_Programas_Negocio()
        {
        }
        #region (Variables Internas)
        //Propiedades
        private String Programa_ID;
        private String Dependencia_ID;
        private String Nombre;
        private String Estatus;
        private String Comentarios;
        private String Nombre_Usuario;
        #endregion
        #region (Variables Publicas)
        public String P_Programa_ID
        {
            get { return Programa_ID; }
            set { Programa_ID = value; }
        }

        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
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
        public void Alta_Programa()
        {
            Cls_Cat_Nom_Programas_Datos.Alta_Programa(this);
        }
        public void Modificar_Programa()
        {
            Cls_Cat_Nom_Programas_Datos.Modificar_Programa(this);
        }
        public void Eliminar_Programa()
        {
            Cls_Cat_Nom_Programas_Datos.Eliminar_Programa(this);
        }
        public DataTable Consulta_Datos_Programas()
        {
            return Cls_Cat_Nom_Programas_Datos.Consulta_Datos_Programas(this);
        }
        public DataTable Consulta_Programas()
        {
            return Cls_Cat_Nom_Programas_Datos.Consulta_Programas(this);
        }
        #endregion
    }
}