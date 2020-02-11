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
using Presidencia.Catalogo_Ramas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Ramas_Negocio
/// </summary>

namespace Presidencia.Catalogo_Ramas.Negocio
{
    public class Cls_Cat_Pre_Ramas_Negocio
    {
        #region Variables Internas

        private String Rama_ID;
        private String Clave;
        private String Estatus;
        private String Nombre;
        private String Descripcion;
        private String filtro;

        #endregion 

        #region Variables Publicas

        public String P_Rama_ID
        {
            get { return Rama_ID; }
            set { Rama_ID = value; }
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

        #endregion

        #region Metodos

        public void Alta_Rama()
        {
            Cls_Cat_Pre_Ramas_Datos.Alta_Ramas(this);
        }

        public void Modificar_Rama()
        {
            Cls_Cat_Pre_Ramas_Datos.Modificar_Ramas(this);
        }

        public void Eliminar_Rama()
        {
            Cls_Cat_Pre_Ramas_Datos.Eliminar_Rama(this);
        }

        public Cls_Cat_Pre_Ramas_Negocio Consultar_Datos_Rama()
        {
            return Cls_Cat_Pre_Ramas_Datos.Consultar_Datos_Ramas(this);
        }

        public DataTable Consultar_Rama()
        {
            return Cls_Cat_Pre_Ramas_Datos.Consultar_Ramas(this);
        }

        public DataTable Consultar_Rama_Nombre_Id()
        {
            return Cls_Cat_Pre_Ramas_Datos.Consultar_Ramas_Nombre_Id();
        }

        #endregion

    }
}
