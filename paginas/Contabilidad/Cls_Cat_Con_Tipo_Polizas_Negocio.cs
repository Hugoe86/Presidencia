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
using Presidencia.Tipo_Polizas.Datos;

namespace Presidencia.Tipo_Polizas.Negocios
{
    public class Cls_Cat_Con_Tipo_Polizas_Negocio
    {
        #region (Variables_Internas)
        private String Tipo_Poliza_ID;
        private String Descripcion;
        private String Abreviacion;
        private String Comentarios;
        private String Nombre_Usuario;

        public String P_Nombre_Usuario
        {
            get { return Nombre_Usuario; }
            set { Nombre_Usuario = value; }
        }
        #endregion

        #region (Variables_Publicas)
        public String P_Tipo_Poliza_ID
        {
            get { return Tipo_Poliza_ID; }
            set { Tipo_Poliza_ID = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Abreviacion
        {
            get { return Abreviacion; }
            set { Abreviacion = value; }
        }

        public String P_Comentarios
        {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        #endregion

        #region (Metodos)
            public void Alta_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Datos.Alta_Tipo_Poliza(this);
            }
            public void Modificar_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Datos.Modificar_Tipo_Poliza(this);
            }
            public void Eliminar_Tipo_Poliza()
            {
                Cls_Cat_Con_Tipo_Polizas_Datos.Eliminar_Tipo_Poliza(this);
            }
            public DataTable Consulta_Datos_Tipo_Poliza()
            {
                return Cls_Cat_Con_Tipo_Polizas_Datos.Consulta_Datos_Tipo_Poliza(this);
            }
            public DataTable Consulta_Tipos_Poliza()
            {
                return Cls_Cat_Con_Tipo_Polizas_Datos.Consulta_Tipos_Poliza(this);
            }
        #endregion
    }
}