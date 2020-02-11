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
using Presidencia.Reporte_Plazas.Datos;
namespace Presidencia.Reporte_Plazas.Negocio
{
    public class Cls_Rpt_Nom_Plazas_Negocio
    {
        public Cls_Rpt_Nom_Plazas_Negocio()
        {
        }
        #region Variables Privadas
        private String Dependencia_ID;
        private String Puesto_ID;
        private String Tipo_Nomina;
        private String Usuario_Creo;
        #endregion 
        #region Variables Publicas
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Puesto_ID
        {
            get { return Puesto_ID; }
            set { Puesto_ID = value; }
        }
        public String P_Tipo_Nomina
        {
            get { return Tipo_Nomina; }
            set { Tipo_Nomina = value; }
        }
        public String P_Usuario_Creo
        {
            get { return Usuario_Creo; }
            set { Usuario_Creo = value; }
        }
        #endregion
        #region Metodos
        public DataTable Consultar_Puestos_Depenencia()
        {
            return Cls_Rpt_Nom_Plaza_Datos.Consultar_Puestos_Depenencia(this);
        }
        public DataTable Consultar_Tipo_Nomina()
        {
            return Cls_Rpt_Nom_Plaza_Datos.Consultar_Tipo_Nomina(this);
        }
        public DataTable Consultar_Plazas()
        {
            return Cls_Rpt_Nom_Plaza_Datos.Consultar_Plazas(this);
        }
        #endregion

    }
}