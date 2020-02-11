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
using Presidencia.Tipos_Descuentos_Especificos.Datos;

namespace Presidencia.Tipos_Descuentos_Especificos.Negocio
{
    public class Cls_Cat_Nom_Tipos_Desc_Esp_Negocio
    {
        #region (Variables Privadas)
        private String Tipo_Desc_Esp_ID;
        private String Clave;
        private String Descripcion;
        private String Cargo_Abono_ID;
        private String Usuario;
        #endregion

        #region (Variables Privadas)
        public String P_Tipo_Desc_Esp_ID {
            get { return Tipo_Desc_Esp_ID; }
            set { Tipo_Desc_Esp_ID = value; }
        }

        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }

        public String P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }

        public String P_Cargo_Abono_ID
        {
            get { return Cargo_Abono_ID; }
            set { Cargo_Abono_ID = value; }
        }

        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region (Métodos)
        public Boolean Alta() { return Cls_Cat_Nom_Tipos_Desc_Esp_Datos.Alta(this); }
        public Boolean Actualizar() { return Cls_Cat_Nom_Tipos_Desc_Esp_Datos.Actualizar(this); }
        public Boolean Delete() { return Cls_Cat_Nom_Tipos_Desc_Esp_Datos.Delete(this); }
        public DataTable Consultar() { return Cls_Cat_Nom_Tipos_Desc_Esp_Datos.Consulta_Tipos_Descuentos_Especificos(this); }
        public DataTable Consultar_Clave_Cargo_Abono() { return Cls_Cat_Nom_Tipos_Desc_Esp_Datos.Consultar_Clave_Cargo_Abono(this); }
        #endregion
    }
}