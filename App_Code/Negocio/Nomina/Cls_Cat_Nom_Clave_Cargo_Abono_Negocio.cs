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
using Presidencia.Claves_Cargo_Abono.Datos;

namespace Presidencia.Claves_Cargo_Abono.Negocio
{
    public class Cls_Cat_Nom_Clave_Cargo_Abono_Negocio
    {
        #region (Variables Privadas)
        private String Cargo_Abono_ID;
        private String Clave;
        private String Descripcion;
        private String Usuario;
        #endregion
    
        #region (Variables Públicas)
        public String P_Cargo_Abono_ID
        {
            get { return Cargo_Abono_ID; }
            set { Cargo_Abono_ID = value; }
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
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        #endregion

        #region (Metodos)
        public Boolean Alta() { return Cls_Cat_Nom_Clave_Cargo_Abono_Datos.Alta(this);  }
        public Boolean Actualizar() { return Cls_Cat_Nom_Clave_Cargo_Abono_Datos.Actualizar(this); }
        public Boolean Delete() { return Cls_Cat_Nom_Clave_Cargo_Abono_Datos.Delete(this); }
        public DataTable Consultar_Clave_Cargo_Abono() { return Cls_Cat_Nom_Clave_Cargo_Abono_Datos.Consultar_Clave_Cargo_Abono(this); }
        #endregion
    }
}
