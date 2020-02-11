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
using Presidencia.Catalogo_Turnos.Datos;

namespace Presidencia.Catalogo_Turnos.Negocio
{
    public class Cls_Cat_Pre_Turnos_Negocio
    {

        #region Variables Internas

            private String Turno_ID;
            private String Nombre;
            private String Hora_Inicio;
            private String Hora_Fin;
            private String Comentarios;
            private String Tipo_DataTable;
            private String Campos_Dinamicos;
            private String Filtros_Dinamicos;
            private String Agrupar_Dinamico;
            private String Ordenar_Dinamico;
            private String Usuario;

        #endregion

        #region Variables Publicas

            public String P_Turno_ID
            {
                get { return Turno_ID; }
                set { Turno_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }
            
            public String P_Hora_Inicio
            {
                get { return Hora_Inicio; }
                set { Hora_Inicio = value; }
            }

            public String P_Hora_Fin
            {
                get { return Hora_Fin; }
                set { Hora_Fin = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
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

            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Turno() {
                Cls_Cat_Pre_Turnos_Datos.Alta_Turno(this);
            }

            public void Modificar_Turno() {
                Cls_Cat_Pre_Turnos_Datos.Modificar_Turno(this);
            }

            public void Eliminar_Turno() {
                Cls_Cat_Pre_Turnos_Datos.Eliminar_Turno(this);
            }

            public DataTable Consultar_Turno()
            {
                return Cls_Cat_Pre_Turnos_Datos.Consultar_Turno();
            }

            public DataTable Consultar_Busqueda()
            {
                return Cls_Cat_Pre_Turnos_Datos.Consultar_Busqueda(this);
            }

        #endregion

    }
}