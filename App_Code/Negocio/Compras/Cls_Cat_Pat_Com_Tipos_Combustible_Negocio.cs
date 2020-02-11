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
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Combustible.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Tipos_Combustible_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Tipos_Combustible.Negocio {

    public class Cls_Cat_Pat_Com_Tipos_Combustible_Negocio {

        #region Variables Internas

            private String Tipo_Combustible_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Tipo_Combustible_ID
            {
                get { return Tipo_Combustible_ID; }
                set { Tipo_Combustible_ID = value; }
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

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Tipo_Combustible() {
                Cls_Cat_Pat_Com_Tipos_Combustible_Datos.Alta_Tipo_Combustible(this);
            }

            public void Modificar_Tipo_Combustible() {
                Cls_Cat_Pat_Com_Tipos_Combustible_Datos.Modificar_Tipo_Combustible(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Tipos_Combustible_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Tipo_Combustible() {
                Cls_Cat_Pat_Com_Tipos_Combustible_Datos.Eliminar_Tipo_Combustible(this);
            }

        #endregion

    }

}