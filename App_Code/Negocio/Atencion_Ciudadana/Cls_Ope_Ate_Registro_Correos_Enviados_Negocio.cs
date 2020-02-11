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
using Presidencia.Operacion_Atencion_Ciudadana_Registro_Correos_Enviados.Datos;
using System.Data.OracleClient;

namespace Presidencia.Operacion_Atencion_Ciudadana_Registro_Correos_Enviados.Negocio
{
    public class Cls_Ope_Ate_Registro_Correo_Enviados_Negocio
    {
        public Cls_Ope_Ate_Registro_Correo_Enviados_Negocio()
        {
        }

        #region Variables Privadas
        private string No_Envio;
        private string Destinatario;
        private string Motivo;
        private string Email;
        private string Nombre_Contribuyente;
        private DateTime Fecha_Notificacion;
        private DateTime Fecha_Inicial;
        private DateTime Fecha_Final;
        private string Contribuyente_ID;
        private string Usuario_Creo;
        private OracleCommand Comando_Oracle; 
        #endregion

        #region Variables publicas

        public string P_No_Envio
        {
            get { return No_Envio; }
            set { No_Envio = value; }
        }
        public string P_Destinatario
        {
            get { return Destinatario; }
            set { Destinatario = value; }
        }
        public string P_Motivo
        {
            get { return Motivo; }
            set { Motivo = value; }
        }
        public string P_Email
        {
            get { return Email; }
            set { Email = value; }
        }
        public string P_Nombre_Contribuyente
        {
            get { return Nombre_Contribuyente; }
            set { Nombre_Contribuyente = value; }
        }
        public DateTime P_Fecha_Notificacion
        {
            get { return Fecha_Notificacion; }
            set { Fecha_Notificacion = value; }
        }
        public DateTime P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public DateTime P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
        }
        public string P_Contribuyente_ID
        {
            get { return Contribuyente_ID; }
            set { Contribuyente_ID = value; }
        }
        public string P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        public OracleCommand P_Comando_Oracle
        {
            get { return Comando_Oracle; }
            set { Comando_Oracle = value; }
        }
        #endregion


        #region Metodos
        public int Alta_Registro_Correo_Enviado()
        {
            return Cls_Ope_Ate_Registro_Correo_Enviados_Datos.Alta_Registro_Correo_Enviado(this);
        }
        public DataTable Consultar_Registro_Correos_Enviados()
        {
            return Cls_Ope_Ate_Registro_Correo_Enviados_Datos.Consultar_Registro_Correos_Enviados(this);
        }
        #endregion
    }
}
