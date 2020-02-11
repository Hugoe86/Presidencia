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
using Presidencia.Predial_Pae_Publicaciones.Datos;

namespace Presidencia.Predial_Pae_Publicaciones.Negocio
{
    public class Cls_Ope_Pre_Pae_Publicaciones_Negocio
    {
        #region Variables Internas
        private String No_Publicacion;
        private String No_Detalle_Etapa;
        private String Fecha_Publicacion;
        private String Medio_Publicacion;
        private String Pagina;
        private String Tomo;
        private String Parte;
        private String Foja;
        private String Proceso;
        private String Estatus;

        private String Filtro;
        private String Campos_Dinamicos;
        private String Agrupar_Dinamico;
        #endregion

        #region Variables Publicas
        public String P_No_Publicacion
        {
            get { return No_Publicacion; }
            set { No_Publicacion = value; }
        }
        public String P_No_Detalle_Etapa
        {
            get { return No_Detalle_Etapa; }
            set { No_Detalle_Etapa = value; }
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
        public String P_Proceso
        {
            get { return Proceso; }
            set { Proceso = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
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
        #endregion

        #region Metodos
        public void Alta_Publicaciones()
        {
            Cls_Ope_Pre_Pae_Publicaciones_Datos.Alta_Publicaciones(this);
        }
        public DataTable Consultar_Notificacion()
        {
            return Cls_Ope_Pre_Pae_Publicaciones_Datos.Consultar_Publicacion(this);
        }
        #endregion
    }
}
