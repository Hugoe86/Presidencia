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
using Presidencia.Nomina_Catalogo_Plazas.Datos;

/// <summary>
/// Summary description for Cls_Cat_Pat_Com_Zonas_Negocio
/// </summary>
namespace Presidencia.Nomina_Catalogo_Plazas.Negocio {

    public class Cls_Cat_Nom_Plazas_Negocio
    {

        #region Variables Internas

            private String Plaza_ID = null;
            private String Nombre = null;
            private String Estatus = null;
            private String Comentarios = null;
            private String Usuario = null;
             
        #endregion

        #region Variables Publicas

            public String P_Plaza_ID
            {
                get { return Plaza_ID; }
                set { Plaza_ID = value; }
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
            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }
            public String P_Usuario
            {
                get { return Usuario; }
                set { Usuario = value; }
            }

        #endregion

        #region Metodos

            public void Alta_Plaza() {
                Cls_Cat_Nom_Plazas_Datos.Alta_Plaza(this);
            }

            public void Modificar_Plaza() {
                Cls_Cat_Nom_Plazas_Datos.Modificar_Plaza(this);
            }

            public DataTable Consultar_Plazas() {
                return Cls_Cat_Nom_Plazas_Datos.Consultar_Plazas(this);
            }

            public void Eliminar_Plaza() {
                Cls_Cat_Nom_Plazas_Datos.Eliminar_Plaza(this);
            }

            public Cls_Cat_Nom_Plazas_Negocio Consultar_Datos_Plaza() {
                return Cls_Cat_Nom_Plazas_Datos.Consultar_Datos_Plaza(this);
            }

        #endregion

    }

}