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
using Presidencia.Catalogo_Grupos.Datos;
using Presidencia.Catalogo_Ramas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Grupos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Grupos.Negocio
{
    public class Cls_Cat_Pre_Grupos_Negocio
    {
        #region Variables Internas

        private String Grupo_ID;
        private String Clave;
        private String Estatus;
        private String Nombre;
        private String Descripcion;
        private String filtro;
        private String Rama_ID;

        #endregion

        #region Variables Publicas

        public String P_Grupo_ID
        {
            get { return Grupo_ID; }
            set { Grupo_ID = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Filtro
        {
            get { return filtro; }
            set { filtro = value; }
        }

        public String P_Rama_ID
        {
            get { return Rama_ID; }
            set { Rama_ID = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Grupo()
        {
            Cls_Cat_Pre_Grupos_Datos.Alta_Grupos(this);
        }

        public void Modificar_Grupo()
        {
            Cls_Cat_Pre_Grupos_Datos.Modificar_Grupos(this);
        }

        public Cls_Cat_Pre_Grupos_Negocio Consultar_Datos_Grupo()
        {
            return Cls_Cat_Pre_Grupos_Datos.Consultar_Datos_Grupos(this);
        }

        public DataTable Consultar_Grupos()
        {
            return Cls_Cat_Pre_Grupos_Datos.Consultar_Grupos(this);
        }

        public DataTable Consultar_Grupos_Combo()
        {
            return Cls_Cat_Pre_Grupos_Datos.Consultar_Id_Nombre_Grupos();
        }

        public void Eliminar_Grupo()
        {
            Cls_Cat_Pre_Grupos_Datos.Eliminar_Grupo(this);
        }
        #endregion
    }
}