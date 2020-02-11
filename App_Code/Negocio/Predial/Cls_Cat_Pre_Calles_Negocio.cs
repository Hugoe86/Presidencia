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
using Presidencia.Catalogo_Calles.Datos;

namespace Presidencia.Catalogo_Calles.Negocio{
    
    public class Cls_Cat_Pre_Calles_Negocio {

        #region Variables Internas

        private String Colonia_ID;
        private String Nombre;
        private String Clave;
        private String Calle_ID;
        private String Comentarios;
        private String Estatus;
        private String Usuario;
        private DataTable Colonias;
        private String Nombre_Calle;
        private String Nombre_Colonia;
        private Boolean Mostrar_Nombre_Calle_Nombre_Colonia;
        private String Clave_Colonia;
        private String Clave_Calle;

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

        public String P_Nombre_Calle
        {
            get { return Nombre_Calle; }
            set { Nombre_Calle = value; }
        }

        public String P_Nombre_Colonia
        {
            get { return Nombre_Colonia; }
            set { Nombre_Colonia = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Calle_ID
        {
            get { return Calle_ID; }
            set { Calle_ID = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
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

        public DataTable P_Colonias
        {
            get { return Colonias; }
            set { Colonias = value; }
        }

        public Boolean P_Mostrar_Nombre_Calle_Nombre_Colonia
        {
            get { return Mostrar_Nombre_Calle_Nombre_Colonia; }
            set { Mostrar_Nombre_Calle_Nombre_Colonia = value; }
        }

        public String P_Clave_Colonia
        {
            get { return Clave_Colonia; }
            set { Clave_Colonia = value; }
        }

        public String P_Clave_Calle
        {
            get { return Clave_Calle; }
            set { Clave_Calle = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Calle()
        {
            Cls_Cat_Pre_Calles_Datos.Alta_Calle(this);
        }

        public String Ultima_Clave()
        {
            return Cls_Cat_Pre_Calles_Datos.Ultima_Clave();
        }

        public void Modificar_Calle()
        {
            Cls_Cat_Pre_Calles_Datos.Modificar_Calle(this);
        }

        public void Eliminar_Calle()
        {
            Cls_Cat_Pre_Calles_Datos.Eliminar_Calle(this);
        }

        public DataTable Consultar_Nombre() //Busqueda
        {
            return Cls_Cat_Pre_Calles_Datos.Consultar_Nombre(this);
        }

        public DataTable Consultar_Calles()
        {
            return Cls_Cat_Pre_Calles_Datos.Consultar_Calles(this);
        }

        public DataTable Llenar_Combo_Colonias() 
        {
            return Cls_Cat_Pre_Calles_Datos.Consultar_Colonias();
        }

        public DataTable Consultar_Nombre_Id_Calles()
        {
            return Cls_Cat_Pre_Calles_Datos.Consultar_Nombre_Calles(this);
        }

        public DataTable Consultar_Colonias_Calles()
        {
            return Cls_Cat_Pre_Calles_Datos.Consultar_Colonias_Calles(this);
        }

        #endregion

    }
}