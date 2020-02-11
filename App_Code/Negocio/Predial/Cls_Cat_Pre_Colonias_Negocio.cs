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
using Presidencia.Catalogo_Colonias.Datos;

namespace Presidencia.Catalogo_Colonias.Negocio{
    
    public class Cls_Cat_Pre_Colonias_Negocio {

        #region Variables Internas

        private String Colonia_ID;
        private String Nombre = "";
        private String Clave;
        private String Descripcion;
        private String Estatus;
        private String Tipo;
        private String Usuario;
        private String Sector;
        private DataTable Colonias;

        #endregion

        #region Variables Publicas

        public String P_Colonia_ID
        {
            get { return Colonia_ID; }
            set { Colonia_ID = value; }
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

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
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

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public DataTable P_Colonias
        {
            get { return Colonias; }
            set { Colonias = value; }
        }
        public String P_Sector
        {
            get { return Sector; }
            set { Sector = value; }
        }
        #endregion

        #region Metodos
        
        public void Alta_Colonia()
        {
            Cls_Cat_Pre_Colonias_Datos.Alta_Colonia(this);
        }

        public String Ultima_Clave()
        {
            return Cls_Cat_Pre_Colonias_Datos.Ultima_Clave(this);
        }

        public void Modificar_Colonia()
        {
            Cls_Cat_Pre_Colonias_Datos.Modificar_Colonia(this);
        }

        public void Eliminar_Colonia()
        {
            Cls_Cat_Pre_Colonias_Datos.Eliminar_Colonia(this);
        }

        public DataTable Consultar_Nombre() //Busqueda
        {
            return Cls_Cat_Pre_Colonias_Datos.Consultar_Nombre(this);
        }

        public DataTable Consultar_Colonias()
        {
            return Cls_Cat_Pre_Colonias_Datos.Consultar_Colonias();
        }

        public DataTable Llenar_Combo_Tipos()
        {
            return Cls_Cat_Pre_Colonias_Datos.Consultar_Tipos();
        }

        public DataTable Consultar_Nombre_Id_Colonias()
        {
            return Cls_Cat_Pre_Colonias_Datos.Consultar_Nombre_Colonias(this);
        }
        public void Actualizar_Sector_Colonia()
        {
            Cls_Cat_Pre_Colonias_Datos.Actualizar_Sector_Colonia(this);
        }
        public DataTable Consultar_Sectores()
        {
            return Cls_Cat_Pre_Colonias_Datos.Consultar_Sectores(this);
        }
        #endregion

    }
}