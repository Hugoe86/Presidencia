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
using Presidencia.Catalogo_Claves_Grupos_Movimiento.Datos;

namespace Presidencia.Catalogo_Claves_Grupos_Movimiento.Negocio
{
    public class Cls_Cat_Pre_Claves_Grupos_Movimiento_Negocio
    {

        #region Variables Internas

        private String Grupo_Movimiento_ID;
        private String Nombre;
        private String Estatus;
        private String Usuario;
        private String Tipo_DataTable;
        private String Clave;
        private DataTable Grupos_Movimiento;
        private DataTable Dt_Detalles_Folios;

        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Join;

        #endregion

        #region Variables Publicas

        public String P_Grupo_Movimiento_ID
        {
            get { return Grupo_Movimiento_ID; }
            set { Grupo_Movimiento_ID = value; }
        }

        public String P_Nombre
        {
            get { return Nombre; }
            set { Nombre = value; }
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

        public String P_Tipo_DataTable
        {
            get { return Tipo_DataTable; }
            set { Tipo_DataTable = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public DataTable P_Grupos_Movimiento
        {
            get { return Grupos_Movimiento; }
            set { Grupos_Movimiento = value; }
        }

        public DataTable P_Dt_Detalles_Folios
        {
            get { return Dt_Detalles_Folios; }
            set { Dt_Detalles_Folios = value; }
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

        public String P_Join
        {
            get { return Join; }
            set { P_Join = value; }
        }
        
        #endregion

        #region Metodos

        public void Alta_Grupo_Movimiento()
        {
            Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos.Alta_Grupo_Movimiento(this);
        }

        public void Modificar_Grupo_Movimiento()
        {
            Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos.Modificar_Grupo_Movimiento(this);
        }

        public void Eliminar_Grupo_Movimiento()
        {
            Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos.Eliminar_Grupo_Movimiento(this);
        }

        public DataTable Consultar_DataTable()
        {
            return Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos.Consultar_DataTable(this);
        }

        public DataTable Consultar_Detalles_Folios()
        {
            return Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos.Consultar_Detalles_Folios(this);
        }

        public DataTable Consultar_Grupos_Movimientos()
        {
            return Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos.Consultar_Grupos_Movimientos(this);
        }

        public String Obtener_Clave_Maxima()
        {
            return Cls_Cat_Pre_Claves_Grupos_Movimiento_Datos.Obtener_Clave_Maxima();
        }

        #endregion

    }
}