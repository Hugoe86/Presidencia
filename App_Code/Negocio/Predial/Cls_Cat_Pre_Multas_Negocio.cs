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
using Presidencia.Catalogo_Multas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Multas_Negocio
/// </summary>

namespace Presidencia.Catalogo_Multas.Negocio{
    public class Cls_Cat_Pre_Multas_Negocio {

        #region Variables Internas

            private String Multa_ID;
            private String Identificador;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private DataTable Multas_Cuotas;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;
            private String Desde;
            private String Hasta;
            private Boolean Incluir_Campos_Foraneos;

        #endregion

        #region Variables Publicas

            public String P_Multa_ID
            {
                get { return Multa_ID; }
                set { Multa_ID = value; }
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

            public DataTable P_Multas_Cuotas
            {
                get { return Multas_Cuotas; }
                set { Multas_Cuotas = value; }
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

            public String P_Desde
            {
                get { return Desde; }
                set { Desde = value; }
            }

            public String P_Hasta
            {
                get { return Hasta; }
                set { Hasta = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Multa() {
                Cls_Cat_Pre_Multas_Datos.Alta_Multa(this);
            }   

            public void Modificar_Multa(){
                Cls_Cat_Pre_Multas_Datos.Modificar_Multa(this);
            }

            public void Eliminar_Multa(){
                Cls_Cat_Pre_Multas_Datos.Eliminar_Multa(this);
            }

            public DataTable Consultar_Multas()
            {
                return Cls_Cat_Pre_Multas_Datos.Consultar_Multas(this);
            }

            public DataTable Consultar_Cuotas_Multas()
            {
                return Cls_Cat_Pre_Multas_Datos.Consultar_Cuotas_Multas(this);
            }

            public Cls_Cat_Pre_Multas_Negocio Consultar_Datos_Multa() {
                return Cls_Cat_Pre_Multas_Datos.Consultar_Datos_Multa(this);
            }

        #endregion

    }
}