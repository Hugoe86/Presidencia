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
using Presidencia.Cierre_Mensual.Datos;

namespace Presidencia.Cierre_Mensual.Negocio
{
    public class Cls_Ope_Con_Cierre_Mensual_Negocio
    {
        #region (Variables_Internas)
            private string Cierre_Mensual_ID;
            private string Cuenta_Contable_ID;
            private string Mes_Anio;
            private string Mes;
            private string Anio;
            private string Estatus;
            private string Fecha_Inicio;
            private string Fecha_Final;
            private string Saldo_Inicial;
            private string Saldo_Final;
            private string Total_Debe;
            private string Total_Haber;
            private string Diferencia;
            private string Usuario_Creo;
            private string Fecha_Creo;
            private string Usuario_Modifico;
            private string Fecha_Modifico;
            private string Descripcion;
        #endregion
        
        #region (Variables_Publicas)
            public string P_Diferencia
            {
                get { return Diferencia; }
                set { Diferencia = value; }
            }
            public string P_Estatus
            {
                get { return Estatus; }
                set { Estatus = value; }
            }
            public string P_Mes
            {
                get { return Mes; }
                set { Mes = value; }
            }
            public string P_Anio
            {
                get { return Anio; }
                set { Anio = value; }
            }
            public string P_Total_Haber
            {
                get { return Total_Haber; }
                set { Total_Haber = value; }
            }
            public string P_Total_Debe
            {
                get { return Total_Debe; }
                set { Total_Debe = value; }
            }
            public string P_Saldo_Final
            {
                get { return Saldo_Final; }
                set { Saldo_Final = value; }
            }
            public string P_Saldo_Inicial
            {
                get { return Saldo_Inicial; }
                set { Saldo_Inicial = value; }
            }
            public string P_Fecha_Final
            {
                get { return Fecha_Final; }
                set { Fecha_Final = value; }
            }
            public string P_Fecha_Inicio
            {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }
            public string P_Mes_Anio
            {
                get { return Mes_Anio; }
                set { Mes_Anio = value; }
            }
            public string P_Cierre_Mensual_ID
            {
                get { return Cierre_Mensual_ID; }
                set { Cierre_Mensual_ID = value; }
            }
            public string P_Cuenta_Contable_ID
            {
                get { return Cuenta_Contable_ID; }
                set { Cuenta_Contable_ID = value; }
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
            public string P_Descripcion
            {
                get { return Descripcion; }
                set { Descripcion = value; }
            }
        #endregion

        #region (Metodos)

            public DataTable Cierre_Mensual()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Cierre_Mensual(this);
            }
            public DataTable Cuentas_Movimientos_Mes()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Cuentas_Movimientos_Mes(this);
            }
            public DataTable Cuentas_Contables_Afectables()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Cuentas_Contables_Afectables(this);
            }
            public DataTable Saldo_Inicial_Cierre_Mensual()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Saldo_Inicial_Cierre_Mensual(this);
            }
            public void Modifica_Cierre_Mensual()
            {
                Cls_Ope_Con_Cierre_Mensual_Datos.Modifica_Cierre_Mensual_Gral(this);
            }
            public void Cierre_Mensual_Alta()
            {
                Cls_Ope_Con_Cierre_Mensual_Datos.Cierre_Mensual_Alta(this);
            }
            public void Alta_Cierre_Mensual_General()
            {
                Cls_Ope_Con_Cierre_Mensual_Datos.Alta_Cierre_Mensual_General(this);
            }
            public void Limpiar_Cierre_Mensual()
            {
                Cls_Ope_Con_Cierre_Mensual_Datos.Limpiar_Cierre_Mensual(this);
            }
            //public void Cierre_Mensual_Actualizar()
            //{
            //    Cls_Ope_Con_Cierre_Mensual_Datos.Cierre_Mensual_Actualizar(this);
            //}

            public void Abrir_Cierre_Mensual()
            {
                Cls_Ope_Con_Cierre_Mensual_Datos.Abrir_Cierre_Mensual(this);
            }
            public DataTable Consulta_Cierre_Mensual_Auxiliar()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Consulta_Cierre_Mensual_Auxiliar(this);
            }
            public DataTable Consulta_Bitacora()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Consulta_Bitacora(this);
            }
        
            public DataTable Consulta_Cierre_Mensual()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Consulta_Cierre_Mensual(this);
            }
            public DataTable Consulta_Cierre_General()
            {
                return Cls_Ope_Con_Cierre_Mensual_Datos.Consulta_Cierre_General(this);
            }
                #endregion
    }
}