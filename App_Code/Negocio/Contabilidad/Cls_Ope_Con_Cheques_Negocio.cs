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
using Presidencia.Cheque.Datos;
/// <summary>
/// Summary description for Cls_Ope_Con_Autoriza_Solicitud_Pago
/// </summary>
/// 
namespace Presidencia.Cheque.Negocio
{
    public class Cls_Ope_Con_Cheques_Negocio
    {
        #region "Variables_Internas"
        private string No_Poliza;
        private string Motivo_Cancelacion;
        private string No_Pago;
        private string Banco_ID;
        private string Tipo_Poliza_ID;
        private string Referencia;
        private string Tipo_Pago;
        private string No_Cheque;
        private string No_Solicitud_Pago;
        private string No_Compromiso;
        private string Estatus;
        private string Fecha_Pago;
        private string Empleado_ID_Jefe;
        private string Empleado_ID_Contabilidad;
        private string Fecha_Solicitud;
        private string Fecha_Inicio;
        private string Fecha_Final;
        private string Tipo_Solicitud_Pago_ID;
        private string Comentario;
        private string Monto;
        private string Fecha_Autorizo_Rechazo_Jefe;
        private string Usuario_Creo;
        private string Fecha_Creo;
        private string Usuario_Modifico;
        private string Fecha_Modifico;
        private string Mes_Ano;
        private string Beneficiario_Pago;
        private Double No_Reserva;
        private DataTable Dt_Detalles_Poliza;
        #endregion

        #region ""Variables Externas"
        public string P_Motivo_Cancelacion
        {
            get { return Motivo_Cancelacion; }
            set { Motivo_Cancelacion = value; }
        }
        public string P_Mes_Ano
        {
            get { return Mes_Ano; }
            set { Mes_Ano = value; }
        }
        public string P_Tipo_Poliza_ID
        {
            get { return Tipo_Poliza_ID; }
            set { Tipo_Poliza_ID = value; }
        }
        public string P_No_Pago
        {
            get { return No_Pago; }
            set { No_Pago = value; }
        }
        public string P_Empleado_ID_Contabilidad
        {
            get { return Empleado_ID_Contabilidad; }
            set { Empleado_ID_Contabilidad = value; }
        }
        public string P_Beneficiario_Pago
        {
            get { return Beneficiario_Pago; }
            set { Beneficiario_Pago = value; }
        }
        public string P_No_Solicitud_Pago
        {
            get { return No_Solicitud_Pago; }
            set { No_Solicitud_Pago = value; }
        }
        public string P_No_Compromiso
        {
            get { return No_Compromiso; }
            set { No_Compromiso = value;}
        }
        public string P_Banco_ID
        {
            get { return Banco_ID; }
            set { Banco_ID = value; }
        }
        public string P_Empleado_ID_Jefe
        {
            get { return Empleado_ID_Jefe; }
            set { Empleado_ID_Jefe = value; }

        }
        public string P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public string P_Fecha_Solicitud
        {
            get { return Fecha_Solicitud; }
            set { Fecha_Solicitud = value; }
        }
        public string P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }
        public string P_Fecha_Final
        {
            get { return Fecha_Final ; }
            set { Fecha_Final = value; }
        }
        public string P_Tipo_Solicitud_Pago_ID
        {
            get { return Tipo_Solicitud_Pago_ID; }
            set { Tipo_Solicitud_Pago_ID = value; }
        }
        public string P_Comentario
        {
            get { return Comentario; }
            set { Comentario = value; }
        }
        public string P_Monto
        {
            get { return Monto; }
            set { Monto = value; }
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
        public string P_Fecha_Autorizo_Rechazo_Jefe
        {
            get { return Fecha_Autorizo_Rechazo_Jefe; }
            set { Fecha_Autorizo_Rechazo_Jefe = value; }
        }
        public string P_Referencia
        {
            get { return Referencia; }
            set { Referencia = value; }
        }
        public string P_Fecha_Pago
        {
            get { return Fecha_Pago; }
            set { Fecha_Pago = value; }
        }
        public string P_Tipo_Pago {
            get { return Tipo_Pago;}
            set { Tipo_Pago = value; }
        }
        public string P_No_Cheque
        {
            get { return No_Cheque; }
            set { No_Cheque = value; }
        }
        public string P_No_Poliza
        {
            get { return No_Poliza; }
            set { No_Poliza = value; }
        }
        public Double P_No_Reserva
        {
            get { return No_Reserva; }
            set { No_Reserva = value; }
        }
        public DataTable P_Dt_Detalles_Poliza
        {
            get { return Dt_Detalles_Poliza; }
            set { Dt_Detalles_Poliza = value; }
        } 
        #endregion

        #region "Metodos"
        public DataTable Consulta_Solicitudes_Autorizadas()
        {
            return Cls_Ope_Con_Cheques_Datos.Consulta_Solicitudes_Autorizadas(this);
        }
        public DataTable Consulta_Pago()
        {
            return Cls_Ope_Con_Cheques_Datos.Consulta_Pago(this);
        }
        public DataTable Consulta_Datos_Pago()
        {
            return Cls_Ope_Con_Cheques_Datos.Consulta_Datos_Pago(this);
        }        
        public DataTable Consulta_Tipos_Solicitudes()
        {
            return Cls_Ope_Con_Cheques_Datos.Consulta_Tipos_Solicitudes(this);
        }
        public DataTable Consulta_Bancos()
        {
            return Cls_Ope_Con_Cheques_Datos.Consulta_Bancos(this);
        }
        public DataTable Consulta_Cuenta_Contable_Banco()
        {
            return Cls_Ope_Con_Cheques_Datos.Consulta_Cuenta_Contable_Banco(this);
        }
        
        public void Cambiar_Estatus_Solicitud_Pago()
        {
            Cls_Ope_Con_Cheques_Datos.Cambiar_Estatus_Solicitud_Pago(this);
        }
        public void Alta_Cheque()
        {
            Cls_Ope_Con_Cheques_Datos.Alta_Cheque(this);
        }
        public void Modificar_Pago()
        {
            Cls_Ope_Con_Cheques_Datos.Modificar_Pago(this);
        }
        #endregion
    }
}


