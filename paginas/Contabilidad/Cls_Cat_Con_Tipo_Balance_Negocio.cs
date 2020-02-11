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
using System.Collections.Generic;
using System.Xml.Linq;
using Presidencia.Tipo_Balance.Datos;

/// <summary>
/// Summary description for Cls_Cat_Con_Tipo_Balance_Negocio
/// </summary>
namespace Presidencia.Tipo_Balance.Negocios
{
    public class Cls_Cat_Con_Tipo_Balance_Negocio
    {
        #region (Variables_Internas)
            private String Tipo_Balance_ID;
            private String Descripcion;
            private String Tipo_Balance;
            private String Comentarios;
            private String Nombre_Usuario;
        #endregion

        #region (Variables Públicas)
            public String P_Tipo_Balance_ID
            {
                get { return Tipo_Balance_ID; }
                set { Tipo_Balance_ID = value; }
            }
            
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            
            public String P_Tipo_Balance
            {
                get { return Tipo_Balance; }
                set { Tipo_Balance = value; }
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
            public void Alta_Tipo_Balance()
            {
                Cls_Cat_Con_Tipo_Balance_Datos.Alta_Tipo_Balance(this);
            }
            public void Modificar_Tipo_Balance()
            {
                Cls_Cat_Con_Tipo_Balance_Datos.Modificar_Tipo_Balance(this);
            }
            public void Eliminar_Tipo_Balance()
            {
                Cls_Cat_Con_Tipo_Balance_Datos.Eliminar_Tipo_Balance(this);
            }
            public DataTable Consulta_Datos_Tipo_Balance()
            {
                return Cls_Cat_Con_Tipo_Balance_Datos.Consulta_Datos_Tipo_Balance(this);
            }
            public DataTable Consulta_Tipo_Balance()
            {
                return Cls_Cat_Con_Tipo_Balance_Datos.Consulta_Tipos_Balance(this);
            }
        #endregion
    }
}

