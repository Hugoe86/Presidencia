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
using Presidencia.Predial_Pae_Notificaciones.Datos;

namespace Presidencia.Predial_Pae_Notificaciones.Negocio
{
    public class Cls_Ope_Pre_Pae_Notificaciones_Negocio
    {
        #region Variables Internas
        private String No_Notificacion;
        private String No_Detalle_Etapa;
        private String Fecha_Hora;
        private String Estatus;
        private String Notificador;
        private String Recibio;
        private String Acuse_Recibo;
        private String Folio;
        private String Medio_Notificacion;
        private String Proceso;

        private String Filtro;
        private String Campos_Dinamicos;
        private String Agrupar_Dinamico;
        private String Cuenta_predial;
        #endregion

        #region Variables Publicas  
        public String P_No_Notificacion
        {
            get { return No_Notificacion; }
            set { No_Notificacion = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
        }
        public String P_Fecha_Hora
        {
            get { return Fecha_Hora; }
            set { Fecha_Hora = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }  
        public String P_Notificador
        {
            get { return Notificador; }
            set { Notificador = value; }
        }
        public String P_Recibio
        {
            get { return Recibio; }
            set { Recibio = value; }
        }
        public String P_Acuse_Recibo
        {
            get { return Acuse_Recibo; }
            set { Acuse_Recibo = value; }
        }
        public String P_Folio
        {
            get { return Folio; }
            set { Folio = value; }
        }
        public String P_Medio_Notificacion
        {
            get { return Medio_Notificacion; }
            set { Medio_Notificacion = value; }
        }
        public String P_Proceso
        {
            get { return Proceso; }
            set { Proceso = value; }
        }
        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }
        public String P_Campos_Dinamicos
        {
            get { return Campos_Dinamicos; }
            set { Campos_Dinamicos = value; }
        }
        public String P_Agrupar_Dinamico
        {
            get { return Agrupar_Dinamico; }
            set { Agrupar_Dinamico = value; }
        }
        public String P_Cuenta_predial
        {
            get { return Cuenta_predial; }
            set { Cuenta_predial = value; }
        }
        #endregion

        #region Metodos
        public void Alta_Notificaciones()
        {
            Cls_Ope_Pre_Pae_Notificaciones_Datos.Alta_Notificaciones(this);
        }
        public DataTable Consultar_Notificacion()
        {
            return Cls_Ope_Pre_Pae_Notificaciones_Datos.Consultar_Notificacion(this);
        }
        public DataTable Consulta_Notificacion_Cuenta_Predial()
        {
            return Cls_Ope_Pre_Pae_Notificaciones_Datos.Consulta_Notificacion_Cuenta_Predial(this);
        }
        #endregion
    }
}
