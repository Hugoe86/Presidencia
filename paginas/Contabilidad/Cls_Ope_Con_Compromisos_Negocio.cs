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
using Presidnecia.Compromisos_Contabilidad.Datos;

namespace Presidencia.Compromisos_Contabilidad.Negocios
{
    public class Cls_Ope_Con_Compromisos_Negocio
    {
        #region (Variables_Internas)
        private string No_Compromiso;
        private string Estatus;
        private string Cuenta_Contable_ID;
        private string Monto_Comprometido;
        private string Concepto;
        private string Proyecto_Programa_ID;
        private string Fuente_Financiamiento_ID;
        private string Area_Funcional_ID;
        private string Partida_ID;
        private string Dependencia_ID;
        private string Empleado_ID;
        private string Proveedor_ID;
        private string Contratista_ID;
        private string Nombre_Beneficiario;
        private string Usuario_Creo;
        private string Usuario_Modifico;
        private string Fecha_Creo;
        private string Fecha_Modifico;
        #endregion

        #region (Variables_Publicas)
        public string P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public string P_Proveedor_ID
        {
            get { return Proveedor_ID; }
            set { Proveedor_ID = value; }
        }
        public string P_Contratista_ID
        {
            get { return Contratista_ID; }
            set { Contratista_ID = value; }
        }
        public string P_Nombre_Beneficiario
        {
            get { return Nombre_Beneficiario; }
            set { Nombre_Beneficiario = value; }
        }
        public string P_No_Compromiso
        {
            get { return No_Compromiso; }
            set { No_Compromiso = value; }
        }
        public string P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public string P_Cuenta_Contable_ID
        {
            get { return Cuenta_Contable_ID; }
            set { Cuenta_Contable_ID = value; }
        }
        public string P_Monto_Comprometido
        {
            get { return Monto_Comprometido; }
            set { Monto_Comprometido = value; }
        }
        public string P_Concepto
        {
            get { return Concepto; }
            set { Concepto = value; }
        }
        public string P_Proyecto_Programa_ID
        {
            get { return Proyecto_Programa_ID; }
            set { Proyecto_Programa_ID = value; }
        }
        public string P_Fuente_Financiamiento_ID
        {
            get { return Fuente_Financiamiento_ID; }
            set { Fuente_Financiamiento_ID = value; }
        }
        public string P_Area_Funcional_ID
        {
            get { return Area_Funcional_ID; }
            set { Area_Funcional_ID = value; }
        }
        public string P_Partida_ID
        {
            get { return Partida_ID; }
            set { Partida_ID = value; }
        }
        public string P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
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
        public DataTable Consulta_Compromisos()
        {
            return Cls_Ope_Con_Compromisos_Datos.Consulta_Compromisos(this);
        }
        public DataTable Consulta_Proveedores()
        {
            return Cls_Ope_Con_Compromisos_Datos.Consulta_Proveedores();
        }
        public void Alta_Compromisos()
        {
            Cls_Ope_Con_Compromisos_Datos.Alta_Compromisos(this);
        }
        public void Modificar_Montos()
        {
            Cls_Ope_Con_Compromisos_Datos.Modificar_Montos(this);
        }
        public void Modificar_Compromisos()
        {
            Cls_Ope_Con_Compromisos_Datos.Modificar_Compromisos(this);
        }
        public void Eliminar_Compromisos()
        {
            Cls_Ope_Con_Compromisos_Datos.Eliminar_Compromisos(this);
        }
        #endregion
    }
}