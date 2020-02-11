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
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Bajas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Tipos_Bajas_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Tipos_Bajas.Negocio {

    public class Cls_Cat_Pat_Com_Tipos_Bajas_Negocio {

        #region Variables Internas

            private String Tipo_Baja_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Tipo_Baja_ID {
                get { return Tipo_Baja_ID; }
                set { Tipo_Baja_ID = value; }
            }

            public String P_Descripcion {
                get { return Descripcion; }
                set { Descripcion = value; }
            }

            public String P_Estatus {
                get { return Estatus; }
                set { Estatus = value; }
            }

            public String P_Usuario {
                get { return Usuario; }
                set { Usuario = value; }
            }

            public String P_Tipo_DataTable {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Tipo_Baja() {
                Cls_Cat_Pat_Com_Tipos_Bajas_Datos.Alta_Tipo_Baja(this);
            }

            public void Modificar_Tipo_Baja() {
                Cls_Cat_Pat_Com_Tipos_Bajas_Datos.Modificar_Tipo_Baja(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Tipos_Bajas_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Tipo_Baja() {
                Cls_Cat_Pat_Com_Tipos_Bajas_Datos.Eliminar_Tipo_Baja(this);
            }

        #endregion

    }

}