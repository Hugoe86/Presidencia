using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using Presidencia.Catalogo_Cat_Descripcion_Construccion_Rustico.Datos;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;

namespace Presidencia.Catalogo_Cat_Descripcion_Construccion_Rustico.Negocio
{
    public class Cls_Cat_Cat_Descripcion_Construccion_Rustico_Negocio
    {
        #region Varibles Internas
        private String Descripcion_Construccion_Rustico_ID;
        private String Estatus;
        private String Identificador;
        #endregion

        #region Variables Publicas
        public String P_Descripcion_Construccion_Rustico_ID
        {
            get { return Descripcion_Construccion_Rustico_ID; }
            set { Descripcion_Construccion_Rustico_ID = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Identificador
        {
            get { return Identificador; }
            set { Identificador = value; }
        }

        #endregion

        #region metodos
        public Boolean Alta_Descripcion_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Descripcion_Construccion_Rustico_Datos.Alta_Descripcion_Construccion_Rustico(this);
        }
        public Boolean Modificar_Descripcion_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Descripcion_Construccion_Rustico_Datos.Modificar_Descripcion_Construccion_Rustico(this);
        }
        public DataTable Consultar_Descripcion_Construccion_Rustico()
        {
            return Cls_Cat_Cat_Descripcion_Construccion_Rustico_Datos.Consultar_Descripcion_Construccion_Rustico(this);

        }


        #endregion
    }
}