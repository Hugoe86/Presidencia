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
using Presidencia.Catalogo_Tipos_Constancias.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Tipos_Constancias_Negocio
/// </summary>

namespace Presidencia.Catalogo_Tipos_Constancias.Negocio{
    public class Cls_Cat_Pre_Tipos_Constancias_Negocio {

        #region Variables Internas
        
        private String Tipo_Constancia_ID;
        private String Clave;
        private String Nombre;
        private Int32 Año;
        private Double Costo;
        private String Estatus;
        private String Certificacion;
        private String Descripcion;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        

        #endregion

        #region Variables Publicas

        public String P_Tipo_Constancia_ID
        {
            get { return Tipo_Constancia_ID; }
            set { Tipo_Constancia_ID = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
        }

        public Int32 P_Año
        {
            get { return Año; }
            set { Año = value; }
        }

        public Double P_Costo
        {
            get { return Costo; }
            set { Costo = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Certificacion
        {
            get { return Certificacion; }
            set { Certificacion = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }

        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }

        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value; }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value; }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

     

        #endregion

        #region Metodos

            public Boolean Alta_Tipo_Constancia() {
                return Cls_Cat_Pre_Tipos_Constancias_Datos.Alta_Tipo_Constancia(this);
            }

            public Boolean Modificar_Tipo_Constancia() {
                return Cls_Cat_Pre_Tipos_Constancias_Datos.Modificar_Tipo_Constancia(this);
            }

            public Boolean Eliminar_Tipo_Constancia() {
                return Cls_Cat_Pre_Tipos_Constancias_Datos.Eliminar_Tipo_Constancia(this);
            }

            //public Cls_Cat_Pre_Tipos_Constancias_Negocio Consultar_Datos_Tipo_Constancia() {
            //    return Cls_Cat_Pre_Tipos_Constancias_Datos.Consultar_Datos_Tipo_Constancia(this);
            //}

            public DataTable Consultar_Tipos_Constancias() {
                return Cls_Cat_Pre_Tipos_Constancias_Datos.Consultar_Tipos_Constancias(this);
            }

        #endregion

    }
}
