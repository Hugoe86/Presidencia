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
using Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio
/// </summary>

namespace Presidencia.Catalogo_Rangos_De_Descuentos_Por_Rol.Negocio
{
    public class Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio
    {

        #region Variables Internas

        private String Rangos_De_Descuento_Por_Rol_Id;
        private String Rol_Id;
        private String Estatus;
        private Int32 Porcentaje_Maximo;
        private String Comentarios;
        private String Nombre_Empleado;
        private String Empleado_Id;
        private String Tipo;
        private String Filtro_Dinamico;
        private String filtro;

        #endregion 

        #region Variables Publicas

        public String P_Rangos_De_Descuento_Por_Rol_Id
        {
            get { return Rangos_De_Descuento_Por_Rol_Id; }
            set { Rangos_De_Descuento_Por_Rol_Id = value; }
        }

        public String P_Filtro
        {
            get { return filtro; }
            set { filtro = value; }
        }

        public String P_Filtro_Dinamico
        {
            get { return Filtro_Dinamico; }
            set { Filtro_Dinamico = value; }
        }

        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }

        public String P_Empleado_Id
        {
            get { return Empleado_Id; }
            set { Empleado_Id = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public Int32 P_Porcentaje_Maximo
        {
            get { return Porcentaje_Maximo; }
            set { Porcentaje_Maximo = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Rangos_De_Descuento_Por_Rol()
        {
            Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos.Alta_Rangos_De_Descuento_Por_Rol(this);
        }

        public void Modificar_Rangos_De_Descuento_Por_Rol()
        {
            Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos.Modificar_Rangos_De_Descuento_Por_Rol(this);
        }

        public Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Consultar_Datos_Rangos_De_Descuento_Por_Rol()
        {
            return Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos.Consultar_Datos_Rangos_De_Descuento_Por_Rol(this);
        }

        public Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Negocio Consultar_Datos_Empleados()
        {
            return Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos.Consultar_Datos_Empleado(this);
        }

        public DataTable Consultar_Rangos_De_Descuento_Por_Rol()
        {
            return Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos.Consultar_Rangos_De_Descuento_Por_Rol(this);
        }

        public DataTable Consultar_Empleados()
        {
            return Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos.Consultar_Empleado(this);
        }

        public DataTable Consultar_Rangos_De_Descuento_Por_Rol_Completo()
        {
            return Cls_Cat_Pre_Rangos_De_Descuento_Por_Rol_Datos.Consultar_Rangos_De_Descuento_Por_Rol_Completo(this);
        }

        #endregion

    }
}