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
using Presidencia.Tipo_Resultado.Datos;

namespace Presidencia.Tipo_Resultado.Negocios
{
    public class Cls_Cat_Con_Tipo_Resultado_Negocio
    {
        #region (Variables_Internas)
            private String Tipo_Resultado_ID;
            private String Descripcion;
            private String Comentarios;
            private String Nombre_Usuario;
        #endregion

        #region (Variables Públicas)
            public String P_Tipo_Resultado_ID
            {
                get { return Tipo_Resultado_ID; }
                set { Tipo_Resultado_ID = value; }
            }

            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }

            public String P_Comentarios
            {
                get { return Comentarios; }
                set { Comentarios = value; }
            }

            public String P_Nombre_Usuario
            {
                get { return Nombre_Usuario; }
                set { Nombre_Usuario = value; }
            }
        #endregion

        #region (Métodos)
            public void Alta_Tipo_Resultado()
            {
                Cls_Cat_Con_Tipo_Resultado_Datos.Alta_Tipo_Resultado(this);
            }
            public void Modificar_Tipo_Resultado()
            {
                Cls_Cat_Con_Tipo_Resultado_Datos.Modificar_Tipo_Resultado(this);
            }
            public void Eliminar_Tipo_Resultado()
            {
                Cls_Cat_Con_Tipo_Resultado_Datos.Eliminar_Tipo_Resultado(this);
            }
            public DataTable Consulta_Datos_Tipo_Resultado()
            {
                return Cls_Cat_Con_Tipo_Resultado_Datos.Consulta_Datos_Tipo_Resultado(this);
            }
            public DataTable Consulta_Tipos_Resultado()
            {
                return Cls_Cat_Con_Tipo_Resultado_Datos.Consulta_Tipos_Resultado(this);
            }
        #endregion
    }	
}
