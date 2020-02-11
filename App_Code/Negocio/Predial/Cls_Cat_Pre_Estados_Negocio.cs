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
using Presidencia.Catalogo_Estados.Datos;

namespace Presidencia.Catalogo_Estados.Negocio{
    
    public class Cls_Cat_Pre_Estados_Negocio {

        #region Variables Internas

        private String Nombre;
        private String Clave;
        private String Estado_ID;
        private String Estatus;
        private String Usuario;
        private DataTable Estados;

        #endregion

        #region Variables Publicas

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Estado_ID
        {
            get { return Estado_ID; }
            set { Estado_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public DataTable P_Estados
        {
            get { return Estados; }
            set { Estados = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Estado()
        {
            Cls_Cat_Pre_Estados_Datos.Alta_Estado(this);
        }

        public String Ultima_Clave()
        {
            return Cls_Cat_Pre_Estados_Datos.Ultima_Clave(this);
        }

        public void Modificar_Estado()
        {
            Cls_Cat_Pre_Estados_Datos.Modificar_Estado(this);
        }

        public void Eliminar_Estado()
        {
            Cls_Cat_Pre_Estados_Datos.Eliminar_Estado(this);
        }

        public DataTable Consultar_Nombre() //Busqueda
        {
            return Cls_Cat_Pre_Estados_Datos.Consultar_Nombre(this);
        }

        public DataTable Consultar_Estados()
        {
            return Cls_Cat_Pre_Estados_Datos.Consultar_Estados();
        }

        #endregion

    }
}