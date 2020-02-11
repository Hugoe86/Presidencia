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
using Presidencia.Catalogo_Conceptos.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pre_Conceptos_Negocio
/// </summary>

namespace Presidencia.Catalogo_Conceptos.Negocio{
    public class Cls_Cat_Pre_Conceptos_Negocio{
        
        #region Variables Internas
        
            private String Concepto_Predial_ID;
            private String Identificador;
            private String Tipo_Concepto;
            private String Descripcion;
            private String Estatus;
            private String Año;
            private String Usuario;
            private DataTable Conceptos_Impuestos_Predial;
            private DataTable Conceptos_Impuestos_Traslacion;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;
            private Boolean Incluir_Campos_Foraneos;

        #endregion

        #region Variables Publicas
            
            public String P_Concepto_Predial_ID{
                get { return Concepto_Predial_ID; }
                set { Concepto_Predial_ID = value; }
            }

            public String P_Identificador{
                get { return Identificador; }
                set { Identificador = value; }
            }

            public String P_Tipo_Concepto{
                get { return Tipo_Concepto; }
                set { Tipo_Concepto = value; }
            }

            public String P_Descripcion{
                get { return Descripcion; }
                set { Descripcion = value; }
            }

            public String P_Estatus{
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Año
            {
                get { return Año; }
                set { Año = value; }
            }

            public String P_Usuario{
                get { return Usuario; }
                set { Usuario = value; }
            }

            public DataTable P_Conceptos_Impuestos_Predial{
                get { return Conceptos_Impuestos_Predial; }
                set { Conceptos_Impuestos_Predial = value; }
            }

            public DataTable P_Conceptos_Impuestos_Traslacion{
                get { return Conceptos_Impuestos_Traslacion; }
                set { Conceptos_Impuestos_Traslacion = value; }
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

            public void Alta_Concepto(){
                Cls_Cat_Pre_Conceptos_Datos.Alta_Concepto(this);
            }

            public void Modifcar_Concepto(){
                Cls_Cat_Pre_Conceptos_Datos.Modificar_Concepto(this);
            }

            public DataTable Consultar_Conceptos(){
                return Cls_Cat_Pre_Conceptos_Datos.Consultar_Conceptos(this);
            }

            public Cls_Cat_Pre_Conceptos_Negocio Consultar_Datos_Concepto()
            {
                return Cls_Cat_Pre_Conceptos_Datos.Consultar_Datos_Concepto(this);
            }

            public DataTable Consultar_Tasas_Traslado()
            {
                return Cls_Cat_Pre_Conceptos_Datos.Consultar_Tasas_Traslado(this);
            }

            public void Eliminar_Concepto() {
                Cls_Cat_Pre_Conceptos_Datos.Eliminar_Concepto(this);
            }
        #endregion

    }
}