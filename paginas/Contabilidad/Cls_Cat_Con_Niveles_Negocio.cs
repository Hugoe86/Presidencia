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
using Presidencia.Niveles.Datos;

namespace Presidencia.Niveles.Negocio
{
    public class Cls_Cat_Con_Niveles_Negocio
    {
        #region (Variables_Internas)
            private String Nivel_ID;
            private String Descripcion;
            private Int32 Inicio_Nivel;
            private Int32 Final_Nivel;
            private String Comentarios;
            private String Nombre_Usuario;
        #endregion
        
        #region (Variables_Publicas)
            public String P_Nivel_ID
            {
                get { return Nivel_ID; }
                set { Nivel_ID = value; }
            }
            public String P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
            public Int32 P_Inicio_Nivel
            {
                get { return Inicio_Nivel; }
                set { Inicio_Nivel = value; }
            }
            public Int32 P_Final_Nivel
            {
                get { return Final_Nivel; }
                set { Final_Nivel = value; }
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

        #region (Metodos)
            public void Alta_Nivel()
            {
                Cls_Cat_Con_Niveles_Datos.Alta_Nivel(this);
            }
            public void Modificar_Nivel()
            {
                Cls_Cat_Con_Niveles_Datos.Modificar_Nivel(this);
            }
            public void Eliminar_Nivel()
            {
                Cls_Cat_Con_Niveles_Datos.Eliminar_Nivel(this);
            }
            public DataTable Consulta_Datos_Nivel()
            {
                return Cls_Cat_Con_Niveles_Datos.Consulta_Datos_Nivel(this);
            }
            public DataTable Consulta_Niveles()
            {
                return Cls_Cat_Con_Niveles_Datos.Consulta_Niveles(this);
            }
        #endregion
    }
}