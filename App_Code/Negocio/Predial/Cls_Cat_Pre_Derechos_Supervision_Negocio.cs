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
using Presidencia.Catalogo_Derechos_Supervision.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Derechos_Supervision_Negocio
/// </summary>

namespace Presidencia.Catalogo_Derechos_Supervision.Negocio{
    public class Cls_Cat_Pre_Derechos_Supervision_Negocio{

        #region Variables Internas

            private String Derecho_Supervision_ID;
            private String Derecho_Supervision_Tasa_ID;
            private String Identificador;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private DataTable Derechos_Tasas;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;
            private Boolean Campos_Foraneos;
            private DataTable Dt_Derechos_Supervision;

        #endregion

        #region Variables Publicas

            public String P_Derecho_Supervision_ID
            {
                get { return Derecho_Supervision_ID; }
                set { Derecho_Supervision_ID = value; }
            }

            public String P_Derecho_Supervision_Tasa_ID
            {
                get { return Derecho_Supervision_Tasa_ID; }
                set { Derecho_Supervision_Tasa_ID = value; }
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

            public DataTable P_Derechos_Tasas
            {
                get { return Derechos_Tasas; }
                set { Derechos_Tasas = value; }
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

            public DataTable P_Dt_Derechos_Supervision
            {
                get { return Dt_Derechos_Supervision; }
                set { Dt_Derechos_Supervision = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Derecho_Supervision() {
                Cls_Cat_Pre_Derechos_Supervision_Datos.Alta_Derecho_Supervision(this);
            }

            public void Modificar_Derecho_Supervision(){
                Cls_Cat_Pre_Derechos_Supervision_Datos.Modificar_Derecho_Supervision(this);
            }

            public void Eliminar_Derecho_Supervision(){
                Cls_Cat_Pre_Derechos_Supervision_Datos.Eliminar_Derecho_Supervision(this);
            }

            public DataTable Consultar_Derechos_Supervision(){
                return Cls_Cat_Pre_Derechos_Supervision_Datos.Consultar_Derechos_Supervision(this);
            }

            public Cls_Cat_Pre_Derechos_Supervision_Negocio Consultar_Datos_Derecho_Supervision(){
                return Cls_Cat_Pre_Derechos_Supervision_Datos.Consultar_Datos_Derecho_Supervision(this);
            }

            public DataTable Consultar_Derechos_Supervision_Tasas()
            {
                return Cls_Cat_Pre_Derechos_Supervision_Datos.Consultar_Derechos_Supervision_Tasas(this);
            }

            public DataTable Consultar_Derechos_Supervision_Detalles_Tasas()
            {
                return Cls_Cat_Pre_Derechos_Supervision_Datos.Consultar_Derechos_Supervision_Detalles_Tasas(this);
            }

        #endregion

    }
}