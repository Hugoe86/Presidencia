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
using Presidencia.Control_Patrimonial_Catalogo_Colores.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Colores_Negocio
/// </summary>

namespace Presidencia.Control_Patrimonial_Catalogo_Colores.Negocio {

    public class Cls_Cat_Pat_Com_Colores_Negocio {

        #region Variables Internas

            private String Color_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Color_ID
            {
                get { return Color_ID; }
                set { Color_ID = value; }
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

            public void Alta_Color() {
                Cls_Cat_Pat_Com_Colores_Datos.Alta_Color(this);
            }

            public void Modificar_Color() {
                Cls_Cat_Pat_Com_Colores_Datos.Modificar_Color(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Colores_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Color() {
                Cls_Cat_Pat_Com_Colores_Datos.Eliminar_Color(this);
            }

        #endregion

    }

}