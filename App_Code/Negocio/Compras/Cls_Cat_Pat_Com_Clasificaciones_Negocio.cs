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
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Clasificaciones_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio {

    public class Cls_Cat_Pat_Com_Clasificaciones_Negocio {

        #region Variables Internas

            private String Clasificacion_ID = null;
            private String Clave = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Clasificacion_ID
            {
                get { return Clasificacion_ID; }
                set { Clasificacion_ID = value; }
            }

            public String P_Clave
            {
                get { return Clave; }
                set { Clave = value; }
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

            public void Alta_Clasificacion() {
                Cls_Cat_Pat_Com_Clasificaciones_Datos.Alta_Clasificacion(this);
            }

            public void Modificar_Clasificacion() {
                Cls_Cat_Pat_Com_Clasificaciones_Datos.Modificar_Clasificacion(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Clasificaciones_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Clasificacion() {
                Cls_Cat_Pat_Com_Clasificaciones_Datos.Eliminar_Clasificacion(this);
            }

        #endregion

    }

}