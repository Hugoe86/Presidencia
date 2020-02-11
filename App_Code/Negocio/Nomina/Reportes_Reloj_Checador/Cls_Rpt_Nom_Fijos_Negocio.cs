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
using Presidencia.Reportes_nomina_Fijos.Datos;

namespace Presidencia.Reportes_nomina_Fijos.Negocio
{
    public class Cls_Rpt_Nom_Fijos_Negocio
    {
        #region Variables Privadas
        private String Fecha_Inicial;
        private String Fecha_Final;
        private String Dependencia_ID;
        private String Tipo_Nomina_ID;
        private String Empleado_ID;
        private String No_Empleado;
        private String Concepto;
        private String Nomina_id;
        private String No_Nomina;
        private String Clave_Deduccion;
        private String Tipo;
        private String Tipo_Asignacion;
        #endregion


        #region Variables Publicas
        public String P_Fecha_Inicial
        {
            get { return Fecha_Inicial; }
            set { Fecha_Inicial = value; }
        }
        public String P_Fecha_Final
        {
            get { return Fecha_Final; }
            set { Fecha_Final = value; }
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
        public String P_Empleado_ID
        {
            get { return Empleado_ID; }
            set { Empleado_ID = value; }
        }
        public String P_No_Empleado
        {
            get { return No_Empleado; }
            set { No_Empleado = value; }
        }
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
        public String P_Clave_Deduccion
        {
            get { return Clave_Deduccion; }
            set { Clave_Deduccion = value; }
        }
        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Tipo_Asignacion
        {
            get { return Tipo_Asignacion; }
            set { Tipo_Asignacion = value; }
        }
        #endregion

        #region(Metodos)
        public DataTable Consultar_Deducciones_Fijas()
        {
            return Cls_Rpt_Nom_Fijos_Datos.Consultar_Deducciones_Fijas(this);
        }

        public DataTable Consultar_Deducciones_Variables()
        {
            return Cls_Rpt_Nom_Fijos_Datos.Consultar_Deducciones_Variables(this);
        }

        public DataTable Consultar_Percepciones()
        {
            return Cls_Rpt_Nom_Fijos_Datos.Consultar_Percepciones(this);
        }
        #endregion

    }
}