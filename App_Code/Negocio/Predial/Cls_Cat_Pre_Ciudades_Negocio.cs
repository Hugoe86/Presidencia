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
using Presidencia.Catalogo_Ciudades.Datos;

namespace Presidencia.Catalogo_Ciudades.Negocio
{

    public class Cls_Cat_Pre_Ciudades_Negocio
    {

        #region Variables Internas

        private String Ciudad_ID;
        private String Nombre;
        private String Clave;
        private String Estado_ID;
        private String Estatus;
        private String Usuario;
        private DataTable Ciudades;

        #endregion

        #region Variables Publicas

        public String P_Ciudad_ID
        {
            get { return Ciudad_ID; }
            set { Ciudad_ID = value; }
        }

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

        public DataTable P_Ciudades
        {
            get { return Ciudades; }
            set { Ciudades = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Ciudad()
        {
            Cls_Cat_Pre_Ciudades_Datos.Alta_Ciudad(this);
        }

        public String Ultima_Clave()
        {
            return Cls_Cat_Pre_Ciudades_Datos.Ultima_Clave(this);
        }

        public void Modificar_Ciudad()
        {
            Cls_Cat_Pre_Ciudades_Datos.Modificar_Ciudad(this);
        }

        public void Eliminar_Ciudad()
        {
            Cls_Cat_Pre_Ciudades_Datos.Eliminar_Ciudad(this);
        }

        public DataTable Consultar_Nombre() //Busqueda
        {
            return Cls_Cat_Pre_Ciudades_Datos.Consultar_Nombre(this);
        }

        public DataTable Consultar_Ciudades()
        {
            return Cls_Cat_Pre_Ciudades_Datos.Consultar_Ciudades();
        }

        public DataTable Llenar_Combo_Estados()
        {
            return Cls_Cat_Pre_Ciudades_Datos.Consultar_Estados();
        }

        #endregion

    }
}