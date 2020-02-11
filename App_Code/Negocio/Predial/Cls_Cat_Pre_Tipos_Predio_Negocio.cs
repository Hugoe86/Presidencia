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
using Presidencia.Catalogo_Tipos_Predio.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Estados_Predio_Negocio
/// </summary>

namespace Presidencia.Catalogo_Tipos_Predio.Negocio
{
    public class Cls_Cat_Pre_Tipos_Predio_Negocio
    {

        #region Variables Internas

            private String Tipo_Predio_ID;
            private String Descripcion;
            private String Usuario;
            private String Tipo_DataTable;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;

        #endregion

        #region Variables Publicas

            public String P_Tipo_Predio_ID
            {
                get { return Tipo_Predio_ID; }
                set { Tipo_Predio_ID = value; }
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

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
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

            public void Alta_Tipo_Predio() {
                Cls_Cat_Pre_Tipos_Predio_Datos.Alta_Tipo_Predio(this);
            }

            public void Modificar_Tipo_Predio() {
                Cls_Cat_Pre_Tipos_Predio_Datos.Modificar_Tipo_Predio(this);
            }

            public void Eliminar_Tipo_Predio() {
                Cls_Cat_Pre_Tipos_Predio_Datos.Eliminar_Tipo_Predio(this);
            }

            public DataTable Consultar_Tipo_Predio()
            {
                return Cls_Cat_Pre_Tipos_Predio_Datos.Consultar_Tipo_Predio(this);
            }

        #endregion

    }
}