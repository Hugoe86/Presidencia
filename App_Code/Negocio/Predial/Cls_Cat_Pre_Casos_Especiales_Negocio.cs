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
using Presidencia.Catalogo_Casos_Especiales.Datos;
/// <summary>
/// Summary description for Cls_Cat_Pre_Casos_Especiales_Negocio
/// </summary>

namespace Presidencia.Catalogo_Casos_Especiales.Negocio
{
    public class Cls_Cat_Pre_Casos_Especiales_Negocio
    {

        #region Variables Internas

        private String Caso_Especial_ID;
        private String Identificador;
        private String Descripcion;
        private String Articulo;
        private String Inciso;
        private String Observaciones;
        private String Estatus;
        private String Tipo;
        private String Usuario;
        private String Porcentaje;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;

        #endregion

        #region Variables Publicas

        public String P_Caso_Especial_ID
        {
            get { return Caso_Especial_ID; }
            set { Caso_Especial_ID = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Identificador
        {
            get { return Identificador; }
            set { Identificador = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Articulo
        {
            get { return Articulo; }
            set { Articulo = value; }
        }

        public String P_Inciso
        {
            get { return Inciso; }
            set { Inciso = value; }
        }

        public String P_Porcentaje
        {
            get { return Porcentaje; }
            set { Porcentaje = value; }
        }

        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
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

        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value.Trim(); }
        }

        public String P_Filtros_Dinamicos
        {
            get { return Filtros_Dinamicos; }
            set { Filtros_Dinamicos = value.Trim(); }
        }

        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value.Trim(); }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value.Trim(); }
        }

        #endregion

        #region Metodos

        public void Alta_Caso_Especial()
        {
            Cls_Cat_Pre_Casos_Especiales_Datos.Alta_Caso_Especial(this);
        }

        public void Modificar_Caso_Especial()
        {
            Cls_Cat_Pre_Casos_Especiales_Datos.Modificar_Caso_Especial(this);
        }

        public void Eliminar_Caso_Especial()
        {
            Cls_Cat_Pre_Casos_Especiales_Datos.Eliminar_Caso_Especial(this);
        }

        public Cls_Cat_Pre_Casos_Especiales_Negocio Consultar_Datos_Caso_Especial()
        {
            return Cls_Cat_Pre_Casos_Especiales_Datos.Consultar_Datos_Caso_Especial(this);
        }

        public DataTable Consultar_Casos_Especiales()
        {
            return Cls_Cat_Pre_Casos_Especiales_Datos.Consultar_Casos_Especiales(this);
        }

        public DataTable Consultar_Datos_Reporte()
        {
            return Cls_Cat_Pre_Casos_Especiales_Datos.Consultar_Datos_Reporte(this);
        }

        public DataTable Consultar_Nombre_Beneficios()
        {
            return Cls_Cat_Pre_Casos_Especiales_Datos.Consultar_Nombre_Beneficios(this);
        }

        #endregion

    }
}