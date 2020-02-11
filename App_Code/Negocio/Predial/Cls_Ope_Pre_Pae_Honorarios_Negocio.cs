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
using Presidencia.Predial_Pae_Honorarios.Datos;

namespace Presidencia.Predial_Pae_Honorarios.Negocio
{
    public class Cls_Ope_Pre_Pae_Honorarios_Negocio
    {
        #region Variables Internas

        private String No_Honorario;
        private String No_Detalle_Etapa;
        private String Gasto_Ejecucion_Id;
        private String Fecha_Honorario;
        private String Proceso;
        private String Importe;
        private String Fecha_Publicacion;
        private String Medio_Publicacion;
        private String Pagina;
        private String Tomo;
        private String Parte;
        private String Foja;
        private String Estatus;
        private String Observaciones;
        private String Fecha_Hora;
        private String Entrego;
        private String Acuse_Recibo;
        private String Folio_Acuse;
        private String Recibio;
        private String Medio_Notificacion;

        private String Filtro;
        private String Campos_Dinamicos;
        private String Agrupar_Dinamico;
        private String Cuenta_Predial_ID;
        private String Cuenta_Predial;
        #endregion

        #region Variables Publicas
        public String P_No_Honorario
        {
            get {return No_Honorario;}
            set { No_Honorario = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
        }
        public String P_Gasto_Ejecucion_Id
        {
            get { return Gasto_Ejecucion_Id; }
            set { Gasto_Ejecucion_Id = value; }
        }
        public String P_Fecha_Honorario
        {
            get { return Fecha_Honorario; }
            set { Fecha_Honorario = value; }
        }
        public String P_Proceso 
        {
            get { return Proceso; }
            set {Proceso = value;}
        }
        public String P_Importe
        {
            get { return Importe; }
            set { Importe = value; }
        }
        public String P_Fecha_Publicacion
        {
            get { return Fecha_Publicacion; }
            set { Fecha_Publicacion = value; }
        }
        public String P_Medio_Publicacion
        {
            get { return Medio_Publicacion; }
            set { Medio_Publicacion = value; }
        }
        public String P_Pagina
        {
            get { return Pagina; }
            set { Pagina = value; }
        }
        public String P_Tomo
        {
            get { return Tomo; }
            set { Tomo = value; }
        }
        public String P_Parte
        {
            get { return Parte; }
            set { Parte = value; }
        }
        public String P_Foja
        {
            get { return Foja; }
            set { Foja = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        public String P_Observaciones
        {
            get { return Observaciones; }
            set { Observaciones = value; }
        }
        public String P_Fecha_Hora
        {
            get { return Fecha_Hora; }
            set { Fecha_Hora = value; }
        }
        public String P_Entrego
        {
            get { return Entrego; }
            set { Entrego = value; }
        }
        public String P_Acuse_Recibo
        {
            get { return Acuse_Recibo; }
            set { Acuse_Recibo = value; }
        }
        public String P_Folio_Acuse
        {
            get { return Folio_Acuse; }
            set { Folio_Acuse = value; }
        }
        public String P_Recibio
        {
            get { return Recibio; }
            set { Recibio = value; }
        }
        public String P_Medio_Notificacion
        {
            get { return Medio_Notificacion; }
            set { Medio_Notificacion = value; }
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
        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }
        public String P_Cuenta_Predial
        {
            get { return Cuenta_Predial; }
            set { Cuenta_Predial = value; }
        }
        #endregion

        #region Metodos
        public void Alta_Honorario()
        {
            Cls_Ope_Pre_Pae_Honorarios_Datos.Alta_Honorario(this);
        }
        public DataTable Consultar_Honorario()
        {
            return Cls_Ope_Pre_Pae_Honorarios_Datos.Consultar_Honorario(this);
        }

        public DataTable Consultar_Total_Honorarios()
        {
            return Cls_Ope_Pre_Pae_Honorarios_Datos.Consultar_Total_Honorarios(this);
        }
        public DataTable Consultar_Detalles_Honorarios()
        {
            return Cls_Ope_Pre_Pae_Honorarios_Datos.Consultar_Detalles_Honorarios(this);
        }
        #endregion
    }
}
