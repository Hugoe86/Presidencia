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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Reloj_Checador.Datos;
using System.Collections.Generic;

namespace Presidencia.Reloj_Checador.Negocios
{
    public class Cls_Cat_Nom_Reloj_Checador_Negocio
    {
        #region (Variables_Internas)
            private String Reloj_Checador_ID;
            private String Clave;
            private String Ubicacion;
            private String Comentarios;            
            private String Nombre_Usuario;
        #endregion

        #region (Variables Publicas)
            public String P_Reloj_Checador_ID
            {
                get { return Reloj_Checador_ID; }
                set { Reloj_Checador_ID = value; }
            }
            public String P_Clave
            {
                get { return Clave; }
                set { Clave = value; }
            }
            public String P_Ubicacion
            {
                get { return Ubicacion; }
                set { Ubicacion = value; }
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

        #region(Metodos)
            public void Alta_Reloj_Checador()
            {
                Cls_Cat_Nom_Reloj_Checador_Datos.Alta_Reloj_Checador(this);
            }
            public void Modificar_Reloj_Checador()
            {
                Cls_Cat_Nom_Reloj_Checador_Datos.Modificar_Reloj_Checador(this);
            }
            public void Elimina_Reloj_Checador()
            {
                Cls_Cat_Nom_Reloj_Checador_Datos.Elimina_Reloj_Checador(this);
            }
            public DataTable Consulta_Datos_Reloj_Checador()
            {
                return Cls_Cat_Nom_Reloj_Checador_Datos.Consulta_Datos_Reloj_Checador(this);
            }
            public DataTable Consulta_Reloj_Checador()
            {
                return Cls_Cat_Nom_Reloj_Checador_Datos.Consulta_Reloj_Checador(this);
            }
        #endregion
    }
}
