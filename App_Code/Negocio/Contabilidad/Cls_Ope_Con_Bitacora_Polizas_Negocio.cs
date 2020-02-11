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
using Presidencia.Bitacora_Polizas.Datos;

namespace Presidencia.Bitacora_Polizas.Negocio
{
    public class Cls_Ope_Con_Bitacora_Polizas_Negocio
    {
        #region (Variables_Internas)
        private string No_Bitacora;
        private string No_Poliza;
        private string Tipo_Poliza_ID;
        private string Mes_Ano;
        private string Cuenta_Contable_ID;
        private string Debe;
        private string Haber;
        private string Usuario_Creo;
        private string Fecha_Creo;
        private string Usuario_Modifico;
        private string Fecha_Modifico;
        #endregion

        #region (Variables_Publicas)
        public string P_No_Bitacora
        {
            get { return No_Bitacora; }
            set { No_Bitacora = value; }
        }
        public string P_No_Poliza
        {
            get { return No_Poliza; }
            set { No_Poliza = value; }
        }
        public string P_Tipo_Poliza_ID
        {
            get { return Tipo_Poliza_ID; }
            set { Tipo_Poliza_ID = value; }
        }
        public string P_Mes_Ano
        {
            get { return Mes_Ano; }
            set { Mes_Ano = value; }
        }
        public string P_Cuenta_Contable_ID
        {
            get { return Cuenta_Contable_ID; }
            set { Cuenta_Contable_ID = value; }
        }
        public string P_Debe
        {
            get { return Debe; }
            set { Debe = value; }
        }
        public string P_Haber
        {
            get { return Haber; }
            set { Haber = value; }
        }
        public string P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public string P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }
        public string P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        public string P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }
        #endregion

        #region (Metodos)
        public void Alta_Bitacora()
        {
            Cls_Ope_Con_Bitacora_Polizas_Datos.Alta_Bitacora(this);
        }
        #endregion
    }
}