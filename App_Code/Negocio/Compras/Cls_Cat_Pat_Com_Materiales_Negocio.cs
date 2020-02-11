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
using Presidencia.Control_Patrimonial_Catalogo_Materiales.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Materiales_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Materiales.Negocio {

    public class Cls_Cat_Pat_Com_Materiales_Negocio
    {

        #region Variables Internas

            private String Material_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Material_ID
            {
                get { return Material_ID; }
                set { Material_ID = value; }
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

            public void Alta_Material() {
                Cls_Cat_Pat_Com_Materiales_Datos.Alta_Material(this);
            }

            public void Modificar_Material() {
                Cls_Cat_Pat_Com_Materiales_Datos.Modificar_Material(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Materiales_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Material() {
                Cls_Cat_Pat_Com_Materiales_Datos.Eliminar_Material(this);
            }

        #endregion

    }

}