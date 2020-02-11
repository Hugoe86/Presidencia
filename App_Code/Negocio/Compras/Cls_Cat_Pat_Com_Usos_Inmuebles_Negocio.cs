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
using Presidencia.Control_Patrimonial_Catalogo_Usos_Inmuebles.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Usos_Inmuebles
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Usos_Inmuebles.Negocio {

    public class Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio{

        #region Variables Internas

        private String Uso_ID = null;
        private String Descripcion = null;
        private String Estatus = null;
        private String Usuario = null;
        private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Uso_ID
            {
                get { return Uso_ID; }
                set { Uso_ID = value; }
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

            public void Alta_Uso() {
                Cls_Cat_Pat_Com_Usos_Inmuebles_Datos.Alta_Uso(this);
            }

            public void Modificar_Uso() {
                Cls_Cat_Pat_Com_Usos_Inmuebles_Datos.Modificar_Uso(this);
            }

            public DataTable Consultar_Usos() {
                return Cls_Cat_Pat_Com_Usos_Inmuebles_Datos.Consultar_Usos(this);
            }

            public Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio Consultar_Detalles_Uso()
            {
                return Cls_Cat_Pat_Com_Usos_Inmuebles_Datos.Consultar_Detalles_Uso(this);
            }

            public void Eliminar_Uso() {
                Cls_Cat_Pat_Com_Usos_Inmuebles_Datos.Eliminar_Uso(this);
            }

        #endregion

    }
}

