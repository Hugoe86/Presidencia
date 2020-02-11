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
using Presidencia.Control_Patrimonial_Catalogo_Tipos_Adiestramiento.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Tipos_Adiestramiento_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Tipos_Adiestramiento.Negocio {

    public class Cls_Cat_Pat_Com_Tipos_Adiestramiento_Negocio {

        #region Variables Internas

            private String Tipo_Adiestramiento_ID = null;
            private String Nombre = null;
            private String Comentarios = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Tipo_Adiestramiento_ID
            {
                get { return Tipo_Adiestramiento_ID; }
                set { Tipo_Adiestramiento_ID = value; }
            }

            public String P_Nombre
            {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
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

            public void Alta_Tipo_Adiestramiento() {
                Cls_Cat_Pat_Com_Tipos_Adiestramiento_Datos.Alta_Tipo_Adiestramiento(this);
            }

            public void Modificar_Tipo_Adiestramiento() {
                Cls_Cat_Pat_Com_Tipos_Adiestramiento_Datos.Modificar_Tipo_Adiestramiento(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Tipos_Adiestramiento_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Tipo_Adiestramiento() {
                Cls_Cat_Pat_Com_Tipos_Adiestramiento_Datos.Eliminar_Tipo_Adiestramiento(this);
            }

            public Cls_Cat_Pat_Com_Tipos_Adiestramiento_Negocio Consultar_Datos_Tipo_Adiestramiento() {
                return Cls_Cat_Pat_Com_Tipos_Adiestramiento_Datos.Consultar_Datos_Tipo_Adiestramiento(this);
            }

        #endregion

    }

}