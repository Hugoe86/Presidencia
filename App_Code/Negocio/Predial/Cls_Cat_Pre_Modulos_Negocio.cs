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
using Presidencia.Catalogo_Modulos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Modulos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Modulos.Negocio
{
    public class Cls_Cat_Pre_Modulos_Negocio
    {
        #region Variables Internas

        private String Modulo_Id;
        private String Estatus;
        private String Clave;
        private String Ubicacion;
        private String Descripcion;
        private String filtro;

        #endregion

        #region Variables Publicas

        public String P_Id_Modulo
        {
            get { return Modulo_Id; }
            set { Modulo_Id = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Ubicacion
        {
            get { return Ubicacion; }
            set { Ubicacion = value; }
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

        public void Alta_Modulo()
        {
            Cls_Cat_Pre_Modulos_Datos.Alta_Modulos(this);
        }

        public void Modificar_Modulo()
        {
            Cls_Cat_Pre_Modulos_Datos.Modificar_Modulos(this);
        }

        public void Eliminar_Modulo()
        {
            Cls_Cat_Pre_Modulos_Datos.Eliminar_Modulo(this);
        }

        public Cls_Cat_Pre_Modulos_Negocio Consultar_Datos_Modulo()
        {
            return Cls_Cat_Pre_Modulos_Datos.Consultar_Datos_Modulos(this);
        }

        public Cls_Cat_Pre_Modulos_Negocio Consultar_Nombre_Modulo()
        {
            return Cls_Cat_Pre_Modulos_Datos.Consultar_Nombre_Modulo(this);
        }

        public DataTable Consultar_Modulo()
        {
            return Cls_Cat_Pre_Modulos_Datos.Consultar_Modulos(this);
        }

        public DataTable Consultar_Nombre_Modulos()
        {
            return Cls_Cat_Pre_Modulos_Datos.Consultar_Nombre_Modulos();
        }

        #endregion

    }
}