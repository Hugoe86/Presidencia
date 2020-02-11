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
using Presidencia.Catalogo_Fraccionamientos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Fraccionamientos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Fraccionamientos.Negocio{
    public class Cls_Cat_Pre_Fraccionamientos_Negocio{

        #region Variables Internas

            private String Fraccionamiento_ID;
            private String Fraccionamiento_Impuesto_ID;
            private String Identificador;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private DataTable Fraccionamientos_Impuestos;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;
            private Boolean Campos_Foraneos;
            private DataTable Dt_Fraccionamientos;

        #endregion

        #region Variables Publicas

            public String P_Fraccionamiento_ID
            {
                get { return Fraccionamiento_ID; }
                set { Fraccionamiento_ID = value; }
            }

            public String P_Fraccionamiento_Impuesto_ID
            {
                get { return Fraccionamiento_Impuesto_ID; }
                set { Fraccionamiento_Impuesto_ID = value; }
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

            public DataTable P_Fraccionamientos_Impuestos
            {
                get { return Fraccionamientos_Impuestos; }
                set { Fraccionamientos_Impuestos = value; }
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

            public Boolean P_Campos_Foraneos
            {
                get { return Campos_Foraneos; }
                set { Campos_Foraneos = value; }
            }

            public DataTable P_Dt_Fraccionamientos
            {
                get { return Dt_Fraccionamientos; }
                set { Dt_Fraccionamientos = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Fraccionamiento() {
                Cls_Cat_Pre_Fraccionamientos_Datos.Alta_Fraccionamiento(this);
            }   

            public void Modificar_Fraccionamiento(){
                Cls_Cat_Pre_Fraccionamientos_Datos.Modificar_Fraccionamiento(this);
            }

            public void Eliminar_Fraccionamiento(){
                Cls_Cat_Pre_Fraccionamientos_Datos.Eliminar_Fraccionamiento(this);
            }

            public DataTable Consultar_Fraccionamientos() {
                return Cls_Cat_Pre_Fraccionamientos_Datos.Consultar_Fraccionamientos(this);
            }

            public Cls_Cat_Pre_Fraccionamientos_Negocio Consultar_Datos_Fraccionamiento()
            {
                return Cls_Cat_Pre_Fraccionamientos_Datos.Consultar_Datos_Fraccionamiento(this);
            }

            public DataTable Consultar_Fraccionamientos_Impuestos()
            {
                return Cls_Cat_Pre_Fraccionamientos_Datos.Consultar_Fraccionamientos_Impuestos(this);
            }

            public DataTable Consultar_Fraccionamientos_Detalles_Impuestos()
            {
                return Cls_Cat_Pre_Fraccionamientos_Datos.Consultar_Fraccionamientos_Detalles_Impuestos(this);
            }

        #endregion

    }
}