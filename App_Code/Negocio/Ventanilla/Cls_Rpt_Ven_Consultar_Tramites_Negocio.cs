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
using Presidencia.Ventanilla_Consultar_Tramites.Datos;

namespace Presidencia.Ventanilla_Consultar_Tramites.Negocio
{
    public class Cls_Rpt_Ven_Consultar_Tramites_Negocio
    {
        #region Variables Privadas
        private String Usuario;
        private String Solicitud_id;
        private String Clave_Solicitud;
        private String Solicitud_ID;
        private String Email;
        private String Estatus;
        private DateTime Dtime_Fecha_Inicio;
        private DateTime Dtime_Fecha_Fin;
        private String Dependencia_ID;
        private String Filtro;
        private String Nombre_Tramite;
        private Boolean Solicitudes_Pendiente_Proceso;
        private String Clave;
        private String Fundamento;
        #endregion

        #region Variables publicas
        //  tipo String
        public String P_Usuario
        {
            get { return Usuario; }
            set { Usuario = value; }
        }
        public String P_Solicitud_id
        {
            get { return Solicitud_id; }
            set { Solicitud_id = value; }
        }
        public String P_Clave_Solicitud
        {
            get { return Clave_Solicitud; }
            set { Clave_Solicitud = value; }
        }
        public String P_Solicitud_ID
        {
            get { return Solicitud_ID; }
            set { Solicitud_ID = value; }
        }
        public String P_Email
        {
            get { return Email; }
            set { Email = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Filtro
        {
            get { return Filtro; }
            set { Filtro = value; }
        }
        public String P_Nombre_Tramite
        {
            get { return Nombre_Tramite; }
            set { Nombre_Tramite = value; }
        }
        public String P_Clave
        {
            get { return Clave; }
            set { Clave = value; }
        }
        public String P_Fundamento
        {
            get { return Fundamento; }
            set { Fundamento = value; }
        }

        //  DateTime
        public DateTime P_Dtime_Fecha_Inicio
        {
            get { return Dtime_Fecha_Inicio; }
            set { Dtime_Fecha_Inicio = value; }
        }
        public DateTime P_Dtime_Fecha_Fin
        {
            get { return Dtime_Fecha_Fin; }
            set { Dtime_Fecha_Fin = value; }
        }
        public Boolean P_Solicitudes_Pendiente_Proceso
        {
            get { return Solicitudes_Pendiente_Proceso; }
            set { Solicitudes_Pendiente_Proceso = value; }
        }

     
        #endregion

        #region Metodos
        public DataTable Consultar_Tramites()
        {
            return Cls_Rpt_Ven_Consultar_Tramites_Datos.Consultar_Tramites(this);
        }

        public DataTable Consultar_Historial_Actividades()
        {
            return Cls_Rpt_Ven_Consultar_Tramites_Datos.Consultar_Historial_Actividades(this);
        }

        public DataTable Consultar_Historial_Documentos()
        {
            return Cls_Rpt_Ven_Consultar_Tramites_Datos.Consultar_Historial_Documentos(this);
        }
        public DataTable Consultar_Clave_Fundamento()
        {
            return Cls_Rpt_Ven_Consultar_Tramites_Datos.Consultar_Clave_Fundamento(this);
        }
        public DataTable Consultar_Fecha_Pasivo()
        {
            return Cls_Rpt_Ven_Consultar_Tramites_Datos.Consultar_Fecha_Pasivo(this);
        }
        public DataTable Consultar_Datos_Pasivo()
        {
            return Cls_Rpt_Ven_Consultar_Tramites_Datos.Consultar_Datos_Pasivo(this);
        }
        #endregion
    }
}