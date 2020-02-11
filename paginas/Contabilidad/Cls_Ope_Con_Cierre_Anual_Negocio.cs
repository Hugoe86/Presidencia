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
using Presidencia.Cierre_Anual.Datos;

namespace Presidencia.Cierre_Anual.Negocio
{
    public class Cls_Ope_Con_Cierre_Anual_Negocio
    {
        #region (Variables Privadas)
        private string Cuenta_Inicial;
        private string Cuenta_Final;
        private Boolean Cuenta_Contable_Rango;
        private string No_Cierre_Anual;
        private string Cuenta_Contable_ID_Inicio;
        private string Cuenta_Contable_ID_Fin;
        private string Cuenta_Contable_ID;
        private double Total_Debe;
        private double Total_Haber;
        private double Diferencia;
        private string Anio;
        private string Descripcion;
        private string Usuario_Creo;
        private string Usuario_Modifico;
        private string Fecha_Creo;
        private string Fecha_Modifico;
        #endregion

        #region (Variables Publicas)
        public string P_Cuenta_Inicial
        {
            get { return Cuenta_Inicial; }
            set { Cuenta_Inicial = value; }
        }
        public string P_Cuenta_Contable_ID
        {
            get { return Cuenta_Contable_ID; }
            set { Cuenta_Contable_ID = value; }
        }
        public string P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        public string P_Cuenta_Final
        {
            get { return Cuenta_Final; }
            set { Cuenta_Final = value; }
        }
        public Boolean P_Cuenta_Contable_Rango
        {
            get { return Cuenta_Contable_Rango; }
            set { Cuenta_Contable_Rango = value; }
        }
        public string P_No_Cierre_Anual
        {
            get { return No_Cierre_Anual; }
            set { No_Cierre_Anual = value; }
        }
        public string P_Cuenta_Contable_ID_Inicio
        {
            get { return Cuenta_Contable_ID_Inicio; }
            set { Cuenta_Contable_ID_Inicio = value; }
        }
        public string P_Cuenta_Contable_ID_Fin
        {
            get { return Cuenta_Contable_ID_Fin; }
            set { Cuenta_Contable_ID_Fin = value; }
        }
        public double P_Total_Debe
        {
            get { return Total_Debe; }
            set { Total_Debe = value; }
        }
        public double P_Total_Haber
        {
            get { return Total_Haber; }
            set { Total_Haber = value; }
        }
        public double P_Diferencia
        {
            get { return Diferencia; }
            set { Diferencia = value; }
        }
        public string P_Descripcion
        {
            get { return Descripcion; }
            set { Descripcion = value; }
        }
        public string P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public string P_Usuario_Modifico
        {
            get { return Usuario_Modifico; }
            set { Usuario_Modifico = value; }
        }
        public string P_Fecha_Creo
        {
            get { return Fecha_Creo; }
            set { Fecha_Creo = value; }
        }
        public string P_Fecha_Modifico
        {
            get { return Fecha_Modifico; }
            set { Fecha_Modifico = value; }
        }
        #endregion

        #region (Metodos)
        public DataTable Consulta_Datos_Cuentas_Contables()
        {
            return Cls_Ope_Con_Cierre_Anual_Datos.Consulta_Datos_Cuentas_Contables(this);
        }
        public void Alta_Cierre_Mensual()
        {
            Cls_Ope_Con_Cierre_Anual_Datos.Alta_Cierre_Mensual(this);
        }
        #endregion
    }
}