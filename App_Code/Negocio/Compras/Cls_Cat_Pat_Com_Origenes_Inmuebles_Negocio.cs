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

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Origenes_Inmuebles_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Origenes_Inmuebles.Negocio {
    public class Cls_Cat_Pat_Com_Origenes_Inmuebles_Negocio {

        #region Variables Internas

            private String Origen_ID = null;
            private String Nombre = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Origen_ID
            {
                get { return Origen_ID; }
                set { Origen_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
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

            public void Alta_Origen() {
                Cls_Cat_Pat_Com_Origenes_Inmuebles_Datos.Alta_Origen(this);
            }

            public void Modificar_Origen() {
                Cls_Cat_Pat_Com_Origenes_Inmuebles_Datos.Modificar_Origen(this);
            }

            public DataTable Consultar_Origenes() {
                return Cls_Cat_Pat_Com_Origenes_Inmuebles_Datos.Consultar_Origenes(this);
            }

            public Cls_Cat_Pat_Com_Origenes_Inmuebles_Negocio Consultar_Detalles_Origen() {
                return Cls_Cat_Pat_Com_Origenes_Inmuebles_Datos.Consultar_Detalles_Origen(this);
            }

            public void Eliminar_Origen() {
                Cls_Cat_Pat_Com_Origenes_Inmuebles_Datos.Eliminar_Origen(this);
            }

        #endregion
    
    }
}
