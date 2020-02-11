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
using Presidencia.Control_Patrimonial_Catalogo_Orientaciones_Inmuebles.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Orientaciones_Inmuebles
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Orientaciones_Inmuebles.Negocio {

    public class Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio {

        #region Variables Internas

            private String Orientacion_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Orientacion_ID
            {
                get { return Orientacion_ID; }
                set { Orientacion_ID = value; }
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

            public void Alta_Orientacion() {
                Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Datos.Alta_Orientacion(this);
            }

            public void Modificar_Orientacion() {
                Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Datos.Modificar_Orientacion(this);
            }

            public DataTable Consultar_Orientaciones() {
                return Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Datos.Consultar_Orientaciones(this);
            }

            public Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio Consultar_Detalles_Orientacion() {
                return Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Datos.Consultar_Detalles_Orientacion(this);
            }

            public void Eliminar_Orientacion() {
                Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Datos.Eliminar_Orientacion(this);
            }

        #endregion

    }

}