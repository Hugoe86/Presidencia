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
using Presidencia.Reportes_Alta_Isseg.Datos;

namespace Presidencia.Reportes_Alta_Isseg.Negocio
{
    public class Cls_Rpt_Nom_Alta_Isseg_Negocio
    {
        #region Variables Privadas
        private String Nomina_id;
        private String No_Nomina;
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        private String No_Empleado;
        private String Sindicato_ID;
        private DateTime Fecha_Alta_Isseg_Inicio;
        private DateTime Fecha_Alta_Isseg_Fin;
        private String Nombre_Empleado;
        #endregion

        #region Variables Publicas
        public String P_Nomina_id
        {
            get { return Nomina_id; }
            set { Nomina_id = value; }
        }
        public String P_No_Nomina
        {
            get { return No_Nomina; }
            set { No_Nomina = value; }
        }
        public String P_Dependencia_ID
        {
            get { return Dependencia_ID; }
            set { Dependencia_ID = value; }
        }
        public String P_Tipo_Nomina_ID
        {
            get { return Tipo_Nomina_ID; }
            set { Tipo_Nomina_ID = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
        public String P_Sindicato_ID
        {
            get { return Sindicato_ID; }
            set { Sindicato_ID = value; }
        }
        public DateTime P_Fecha_Alta_Isseg_Inicio
        {
            get { return Fecha_Alta_Isseg_Inicio; }
            set { Fecha_Alta_Isseg_Inicio = value; }
        }
        public DateTime P_Fecha_Alta_Isseg_Fin
        {
            get { return Fecha_Alta_Isseg_Fin; }
            set { Fecha_Alta_Isseg_Fin = value; }
        }
        public String P_Nombre_Empleado
        {
            get { return Nombre_Empleado; }
            set { Nombre_Empleado = value; }
        }
        #endregion

        #region(Metodos)
        public DataTable Consultar_Alta_Empleado_Isseg()
        {
            return Cls_Rpt_Nom_Alta_Isseg_Datos.Consultar_Alta_Empleado_Isseg(this);
        }
        #endregion

    }
}