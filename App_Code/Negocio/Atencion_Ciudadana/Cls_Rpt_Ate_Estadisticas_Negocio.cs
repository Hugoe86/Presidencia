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
using Presidencia.Reporte_Atencion_Ciudadana_Estadisticas.Datos;

namespace Presidencia.Reporte_Atencion_Ciudadana_Estadisticas.Negocios
{
    public class Cls_Rpt_Ate_Estadisticas_Negocio
    {

        /********************************************************************************************************
        * PROPIEDADES
        *********************************************************************************************************/
        #region Variables Internas
        private String[] Dependencias;
        private String Dependencia_Area;
        private String[] Areas;
        private String[] Asuntos;
        private DateTime Fecha_Fin;
        private DateTime Fecha_Inicio;

        #endregion Variables Internas

        #region Variables Publicas

        public String[] P_Dependencias
        {
            get { return Dependencias; }
            set { Dependencias = value; }
        }

        public String[] P_Areas
        {
            get { return Areas; }
            set { Areas = value; }
        }
        public String P_Dependencia_Area
        {
            get { return Dependencia_Area; }
            set { Dependencia_Area = value; }
        }
        public String[] P_Asuntos
        {
            get { return Asuntos; }
            set { Asuntos = value; }
        }

        public DateTime P_Fecha_Inicio
        {
            get { return Fecha_Inicio; }
            set { Fecha_Inicio = value; }
        }

        public DateTime P_Fecha_Fin
        {
            get { return Fecha_Fin; }
            set { Fecha_Fin = value; }
        }
        #endregion Variables Publicas

        public Cls_Rpt_Ate_Estadisticas_Negocio()
        {

        }
        /********************************************************************************************************
        * METODOS
        *********************************************************************************************************/

        #region METODOS

        #endregion METODOS
    }
}