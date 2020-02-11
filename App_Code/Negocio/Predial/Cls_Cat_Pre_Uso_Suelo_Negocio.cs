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
using Presidencia.Catalogo_Uso_Suelo.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Uso_Suelo_Negocio
/// </summary>

namespace Presidencia.Catalogo_Uso_Suelo.Negocio{
    public class Cls_Cat_Pre_Uso_Suelo_Negocio{

        #region Varibles Internas

            private String Uso_Suelo_ID;
            private String Identificador;
            private String Descripcion;
            private String Estatus;
            private String Usuario;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;

        #endregion
        
        #region Variables Publicas

            public String P_Uso_Suelo_ID
            {
                get { return Uso_Suelo_ID; }
                set { Uso_Suelo_ID = value; }
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

            public void Alta_Uso_Suelo() {
                Cls_Cat_Pre_Uso_Suelo_Datos.Alta_Uso_Suelo(this);
            }

            public void Modificar_Uso_Suelo(){
                Cls_Cat_Pre_Uso_Suelo_Datos.Modificar_Uso_Suelo(this);
            }

            public void Eliminar_Uso_Suelo(){
                Cls_Cat_Pre_Uso_Suelo_Datos.Eliminar_Uso_Suelo(this);
            }

            public Cls_Cat_Pre_Uso_Suelo_Negocio Consultar_Datos_Uso_Suelo() {
                return Cls_Cat_Pre_Uso_Suelo_Datos.Consultar_Datos_Uso_Suelo(this);
            }

            public DataTable Consultar_Uso_Suelo() {
                return Cls_Cat_Pre_Uso_Suelo_Datos.Consultar_Uso_Suelo(this);
            }

        #endregion

    }
}