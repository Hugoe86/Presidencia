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
using Presidencia.Sap_Partida_Generica.Datos;

namespace Presidencia.Sap_Partida_Generica.Negocio
{
    public class Cls_Cat_Sap_Partida_Generica_Negocio
    {
        #region(Variables Privadas)
        private String Partida_Generica_ID;
        private String Clave;
        private String Descripcion;
        private String Estatus;
        private String Capitulo_ID;
        private String Concepto_ID;
        private String Usuario_Creo;
        private String Usuario_Modifico;
        #endregion

        #region(Variables Públicas)
        public String P_Partida_Generica_ID {
            get { return Partida_Generica_ID; }
            set { Partida_Generica_ID = value; }
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
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Concepto_ID
        {
            get { return Concepto_ID; }
            set { Concepto_ID = value; }
        }

        public String P_Capitulo_ID {
            get { return Capitulo_ID; }
            set { Capitulo_ID = value; }
        }

        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public String P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        #endregion

        #region(Métodos)
        public Boolean Alta_Partida_Generica() {
            return Cls_Cat_Sap_Partida_Generica_Datos.Alta_Partida_Generica(this);
        }
        public Boolean Baja_Partida_Generica()
        {
            return Cls_Cat_Sap_Partida_Generica_Datos.Baja_Partida_Generica(this);
        }
        public Boolean Modificar_Partida_Generica()
        {
            return Cls_Cat_Sap_Partida_Generica_Datos.Modificar_Partida_Generica(this);
        }
        public DataTable Consultar_Partidas_Genericas()
        {
            return Cls_Cat_Sap_Partida_Generica_Datos.Consulta_Partidas_Genericas(this);
        }
        public DataTable Consultar_Capitulo_Concepto() {
            return Cls_Cat_Sap_Partida_Generica_Datos.Consultar_Capitulo_Concepto(this);
        }
        public DataTable Consultar_Sap_Capitulos()
        {
            return Cls_Cat_Sap_Partida_Generica_Datos.Consultar_Sap_Capitulos();
        }
        public DataTable Consultar_Conceptos_Pertencen_Capitulo() {
            return Cls_Cat_Sap_Partida_Generica_Datos.Consultar_Conceptos_Pertencen_Capitulo(this);
        }
        #endregion
    }
}
