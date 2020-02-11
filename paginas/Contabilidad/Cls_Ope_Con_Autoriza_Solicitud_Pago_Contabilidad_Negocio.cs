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
using Presidencia.Autoriza_Solicitud_Pago_Contabilidad.Datos;
/// <summary>
/// Summary description for Cls_Ope_Con_Autoriza_Solicitud_Pago
/// </summary>
/// 
namespace Presidencia.Autoriza_Solicitud_Pago_Contabilidad.Negocio
{
    public class Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Negocio
    {
        #region "Variables_Internas"
        private string No_Solicitud_Pago;
        private string No_Compromiso;
        private string Mes_Anio;
        private string Estatus;
        private string Empleado_ID_Jefe;
        private string Empleado_ID_Contabilidad;
        private string Fecha_Solicitud;
        private string Tipo_Solicitud_Pago_ID;
        private string Comentario;
        private string Monto;
        private string Fecha_Autorizo_Rechazo_Jefe;
        private string Usuario_Creo;
        private string Fecha_Creo;
        private string Usuario_Modifico;
        private string Fecha_Modifico;
        #endregion

        #region ""Variables Externas"
        public string P_Empleado_ID_Contabilidad
        {
            get { return Empleado_ID_Contabilidad; }
            set { Empleado_ID_Contabilidad = value; }
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
        public string P_Mes_Anio {
            get { return Mes_Anio; }
            set { Mes_Anio = value; }
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

        #endregion

        #region "Metodos"
        public DataTable Consulta_Solicitudes_SinAutorizar()
        {
            return Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Datos.Consulta_Solicitudes_SinAutotizar(this);
        }
        public void Cambiar_Estatus_Solicitud_Pago()
        {
        Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Datos.Cambiar_Estatus_Solicitud_Pago(this);
        }
        public DataTable Consultar_Solicitud_Pago()
        {
            return Cls_Ope_Con_Autoriza_Solicitud_Pago_Contabilidad_Datos.Consultar_Solicitud_Pago(this);
        }
        #endregion
    }
}


