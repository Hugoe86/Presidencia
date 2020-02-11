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
using Presidencia.Control_Patrimonial_Catalogo_Areas_Donacion.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Areas_Donacion_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Areas_Donacion.Negocio {

    public class Cls_Cat_Pat_Com_Areas_Donacion_Negocio {

        #region Variables Internas

            private String Area_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Area_ID
            {
                get { return Area_ID; }
                set { Area_ID = value; }
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

            public void Alta_Area() {
                Cls_Cat_Pat_Com_Areas_Donacion_Datos.Alta_Area(this);
            }

            public void Modificar_Area() {
                Cls_Cat_Pat_Com_Areas_Donacion_Datos.Modificar_Area(this);
            }

            public DataTable Consultar_Areas() {
                return Cls_Cat_Pat_Com_Areas_Donacion_Datos.Consultar_Areas(this);
            }

            public Cls_Cat_Pat_Com_Areas_Donacion_Negocio Consultar_Detalles_Area()
            {
                return Cls_Cat_Pat_Com_Areas_Donacion_Datos.Consultar_Detalles_Area(this);
            }

            public void Eliminar_Area() {
                Cls_Cat_Pat_Com_Areas_Donacion_Datos.Eliminar_Area(this);
            }

        #endregion
	    
    }

}