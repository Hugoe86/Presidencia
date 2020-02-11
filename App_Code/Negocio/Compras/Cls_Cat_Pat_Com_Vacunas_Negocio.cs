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
using Presidencia.Control_Patrimonial_Catalogo_Vacunas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Vacunas_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Catalogo_Vacunas.Negocio
{

    public class Cls_Cat_Pat_Com_Vacunas_Negocio
    {

        #region Variables Internas

            private String Vacuna_ID = null;
            private String Nombre = null;
            private String Comentarios = null;
            private String Estatus = null;
            private String Usuario = null;
            private String Tipo_DataTable = null;
             
        #endregion

        #region Variables Publicas

            public String P_Vacuna_ID
            {
                get { return Vacuna_ID; }
                set { Vacuna_ID = value; }
            }

            public String P_Nombre {
                get { return Nombre; }
                set { Nombre = value; }
            }

            public String P_Comentarios {
                get { return Comentarios; }
                set { Comentarios = value; }
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

            public void Alta_Vacuna() {
                Cls_Cat_Pat_Com_Vacunas_Datos.Alta_Vacuna(this);
            }

            public void Modificar_Vacuna() {
                Cls_Cat_Pat_Com_Vacunas_Datos.Modificar_Vacuna(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Cat_Pat_Com_Vacunas_Datos.Consultar_DataTable(this);
            }

            public void Eliminar_Vacuna() {
                Cls_Cat_Pat_Com_Vacunas_Datos.Eliminar_Vacuna(this);
            }

            public Cls_Cat_Pat_Com_Vacunas_Negocio Consultar_Datos_Vacuna() {
                return Cls_Cat_Pat_Com_Vacunas_Datos.Consultar_Datos_Vacuna(this);
            }

        #endregion

    }

}