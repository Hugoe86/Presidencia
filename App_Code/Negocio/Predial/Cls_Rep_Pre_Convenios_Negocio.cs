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
using Presidencia.Reportes_Predial_Convenios.Datos;


namespace Presidencia.Reportes_Predial_Convenios.Negocio
{

    public class Cls_Rep_Pre_Convenios_Negocio
    {

        #region Varibles Internas

        private String No_Convenio;
        private String Cuenta_Predial_ID;
        private String Estatus;

        //Para las validaciones

        private bool Solo_Convenios = false;
        private bool Solo_Reestructuras = false;
        private DateTime Desde_Fecha;
        private DateTime Hasta_Fecha;
        private String Ordenar_Dinamico;

        #endregion

        #region Varibles Publicas

        public String P_No_Convenio
        {
            get { return No_Convenio; }
            set { No_Convenio = value; }
        }

        public String P_Cuenta_Predial_ID
        {
            get { return Cuenta_Predial_ID; }
            set { Cuenta_Predial_ID = value; }
        }

        public String P_Estatus
        {
            get { return Estatus; }
            set { Estatus = value; }
        }

        public Boolean P_Solo_Convenios
        {
            get { return Solo_Convenios; }
            set { Solo_Convenios = value; }
        }
        
        public Boolean P_Solo_Reestructuras
        {
            get { return Solo_Reestructuras; }
            set { Solo_Reestructuras = value; }
        }

        public DateTime P_Desde_Fecha
        {
            get { return Desde_Fecha; }
            set { Desde_Fecha = value; }
        }

        public DateTime P_Hasta_Fecha
        {
            get { return Hasta_Fecha; }
            set { Hasta_Fecha = value; }
        }

        public String P_Ordenar_Dinamico
        {
            get { return Ordenar_Dinamico; }
            set { Ordenar_Dinamico = value; }
        }

        #endregion

        #region Metodos

            public DataTable Consultar_Convenios_Predial_Reporte()
            {
                return Cls_Rep_Pre_Convenios_Datos.Consultar_Convenios_Predial_Reporte(this);
            }

            public DataTable Consultar_Convenios_Predial_Detallado_Reporte()
            {
                return Cls_Rep_Pre_Convenios_Datos.Consultar_Convenios_Predial_Detallado_Reporte(this);
            }

            public DataTable Consultar_Convenios_Traslado_Reporte()
            {
                return Cls_Rep_Pre_Convenios_Datos.Consultar_Convenios_Traslado_Reporte(this);
            }

            public DataTable Consultar_Convenios_Der_Supervision_Reporte()
            {
                return Cls_Rep_Pre_Convenios_Datos.Consultar_Convenios_Der_Supervision_Reporte(this);
            }

            public DataTable Consultar_Convenios_Fraccionamiento_Reporte()
            {
                return Cls_Rep_Pre_Convenios_Datos.Consultar_Convenios_Fraccionamiento_Reporte(this);
            }

        #endregion

    }
}   