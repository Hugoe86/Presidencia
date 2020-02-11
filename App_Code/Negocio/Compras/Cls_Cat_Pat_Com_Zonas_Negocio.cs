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
using Presidencia.Control_Patrimonial_Catalogo_Zonas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Zonas_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Zonas.Negocio {

    public class Cls_Cat_Pat_Com_Zonas_Negocio {

        #region Variables Internas

            private String Zona_ID = null;
            private String Descripcion = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Zona_ID
            {
                get { return Zona_ID; }
                set { Zona_ID = value; }
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

            public void Alta_Zona() {
                Cls_Cat_Pat_Com_Zonas_Datos.Alta_Zona(this);
            }

            public void Modificar_Zona() {
                Cls_Cat_Pat_Com_Zonas_Datos.Modificar_Zona(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Zonas_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Zona() {
                Cls_Cat_Pat_Com_Zonas_Datos.Eliminar_Zona(this);
            }

        #endregion

    }

}