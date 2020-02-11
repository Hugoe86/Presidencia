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
using Presidencia.Bancos_Nomina.Datos;

namespace Presidencia.Bancos_Nomina.Negocio
{
    public class Cls_Cat_Nom_Bancos_Negocio
    {
        #region (Variables Privadas)
        private String Banco_ID;
        private String Nombre;
        private String No_Cuenta;
        private String Sucursal;
        private String Referencia;
        private String Comentarios;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        private String Tipo;
        private String Plan_Pago;
        private String No_Meses;
        #endregion

        #region (Variables Públicas)
        public String P_Banco_ID {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }
        public String P_Nombre {
            get { return Nombre; }
            set { Nombre = value; }
        }
        public String P_No_Cuenta {
            get { return No_Cuenta; }
            set { No_Cuenta = value; }
        }
        public String P_Sucursal {
            get { return Sucursal; }
            set { Sucursal = value; }
        }
        public String P_Referencia {
            get { return Referencia; }
            set { Referencia = value; }
        }
        public String P_Comentarios {
            get { return Comentarios; }
            set { Comentarios = value; }
        }
        public String P_Usuario_Creo {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Usuario_Modifico {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }

        public String P_Tipo {
            get { return Tipo; }
            set { Tipo = value; }
        }

        public String P_Plan_Pago {
            get { return Plan_Pago; }
            set { Plan_Pago = value; }
        }

        public String P_No_Meses {
            get { return No_Meses; }
            set { No_Meses = value; }
        }
        #endregion

        #region (Métodos)
        public Boolean Alta_Banco() {
            return Cls_Cat_Nom_Bancos_Datos.Alta_Banco(this);
        }
        public Boolean Modificar_Banco() {
            return Cls_Cat_Nom_Bancos_Datos.Modificar_Banco(this);
        }
        public Boolean Eliminar_Banco() {
            return Cls_Cat_Nom_Bancos_Datos.Eliminar_Banco(this);
        }
        public DataTable Consulta_Bancos() {
            return Cls_Cat_Nom_Bancos_Datos.Consulta_Bancos(this);
        }
        #endregion
    }
}
