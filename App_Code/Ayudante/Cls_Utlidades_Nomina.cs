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

namespace Presidencia.Utilidades_Nomina
{
    public class Cls_Utlidades_Nomina
    {
        DateTime Fecha;
        Double Dias_Anio;
        Double Dias_Promedio_Mes_Anio;
        public const Double Dias_Mes_Fijo = 30.42;

        public Cls_Utlidades_Nomina(DateTime Fecha) {
            this.Fecha = Fecha;
        }

        public DateTime P_Fecha {
            get { return Fecha; }
            set { Fecha = value; }
        }

        public Double P_Dias_Anio
        {
            get { return ((Double)((DateTime.DaysInMonth(P_Fecha.Year, 2) == 28) ? 365 : 366)); }
            set { Dias_Anio = value; }
        }

        public Double P_Dias_Promedio_Mes_Anio
        {
            get { return ((Double)((DateTime.DaysInMonth(P_Fecha.Year, 2) == 28) ? 365 : 366)) / 12; }
            set { Dias_Promedio_Mes_Anio = value; }
        }

    }
}
