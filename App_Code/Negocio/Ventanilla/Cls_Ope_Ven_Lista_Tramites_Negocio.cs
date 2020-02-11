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
using Presidencia.Ventanilla_Lista_Tramites.Datos;

namespace Presidencia.Ventanilla_Lista_Tramites.Negocio
{
    public class Cls_Ope_Ven_Lista_Tramites_Negocio
    {
        #region Variables Privadas
        private String Tramite_ID;
        private String Nombre_Tramite;
        private String Clave_Tramite;
        private String Dependencia_Tramite;
        private String Estatus;
        #endregion

        #region Variables publicas
        public String P_Tramite_ID
        {
            get { return Tramite_ID; }
            set { Tramite_ID = value; }
        }
        public String P_Nombre_Tramite
        {
            get { return Nombre_Tramite; }
            set { Nombre_Tramite = value; }
        }
        public String P_Clave_Tramite
        {
            get { return Clave_Tramite; }
            set { Clave_Tramite = value; }
        }
        public String P_Dependencia_Tramite
        {
            get { return Dependencia_Tramite; }
            set { Dependencia_Tramite = value; }
        }
        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }
        #endregion

        #region Metodos
        public DataTable Consultar_Documentos_Tramites()
        {
            return Cls_Ope_Ven_Lista_Tramites_Datos.Consultar_Documentos_Tramites(this);
        }

        public DataTable Consultar_Tramites_Populares()
        {
            return Cls_Ope_Ven_Lista_Tramites_Datos.Consultar_Tramites_Populares(this);
        }

        public DataTable Consultar_Tramites()
        {
            return Cls_Ope_Ven_Lista_Tramites_Datos.Consultar_Tramites(this);
        }

        public DataTable Consultar_Actividades_Tramites()
        {
            return Cls_Ope_Ven_Lista_Tramites_Datos.Consultar_Actividades_Tramites(this);
        }
        #endregion
    }
}