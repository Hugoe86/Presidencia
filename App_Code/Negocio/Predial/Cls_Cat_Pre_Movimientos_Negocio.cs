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
using Presidencia.Catalogo_Movimientos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Movimientos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Movimientos.Negocio
{
    public class Cls_Cat_Pre_Movimientos_Negocio
    {

        #region Variables Internas

        private String Movimiento_ID;
        private String Identificador;
        private String Grupo_Id;
        private String Descripcion;
        private String Traslado;
        private String Estatus;
        private String Usuario;
        private String Campos_Dinamicos;
        private String Filtros_Dinamicos;
        private String Agrupar_Dinamico;
        private String Ordenar_Dinamico;
        private String Aplica;
        private String Cargar_Modulos;
        private Boolean Incluir_Campos_Foraneos;

        #endregion

        #region Variables Publicas

        public String P_Movimiento_ID
        {
            get { return Movimiento_ID; }
            set { Movimiento_ID = value; }
        }

        public String P_Grupo_ID
        {
            get { return Grupo_Id; }
            set { Grupo_Id = value; }
        }

        public String P_Aplica
        {
            get { return Aplica; }
            set { Aplica = value; }
        }

        public String P_Cargar_Modulos
        {
            get { return Cargar_Modulos; }
            set { Cargar_Modulos = value; }
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

        public String P_Traslado
        {
            get { return Traslado; }
            set { Traslado = value; }
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

        public Boolean P_Incluir_Campos_Foraneos
        {
            get { return Incluir_Campos_Foraneos; }
            set { Incluir_Campos_Foraneos = value; }
        }

        #endregion

        #region Metodos

        public void Alta_Movimiento()
        {
            Cls_Cat_Pre_Movimientos_Datos.Alta_Movimiento(this);
        }

        public void Modificar_Movimiento()
        {
            Cls_Cat_Pre_Movimientos_Datos.Modificar_Movimiento(this);
        }

        public void Eliminar_Movimiento()
        {
            Cls_Cat_Pre_Movimientos_Datos.Eliminar_Movimiento(this);
        }

        public Cls_Cat_Pre_Movimientos_Negocio Consultar_Datos_Movimiento()
        {
            return Cls_Cat_Pre_Movimientos_Datos.Consultar_Datos_Movimiento(this);
        }

        public DataTable Consultar_Movimientos()
        {
            return Cls_Cat_Pre_Movimientos_Datos.Consultar_Movimientos(this);
        }

        public DataTable Consultar_Nombre_Id_Movimientos()
        {
            return Cls_Cat_Pre_Movimientos_Datos.Consultar_Nombre_Id_Movimientos(this);
        }

        public DataTable Consultar_Movimientos_Cancelacion()
        {
            return Cls_Cat_Pre_Movimientos_Datos.Consultar_Movimientos_Cancelacion(this);
        }

        public DataTable Consultar_Movimientos_Bajas_Directas()
        {
            return Cls_Cat_Pre_Movimientos_Datos.Consultar_Movimientos_Bajas_Directas(this);
        }

        public Boolean Validar_Existe()
        {
            return Cls_Cat_Pre_Movimientos_Datos.Validar_Existe(this);
        }

        #endregion

    }
}