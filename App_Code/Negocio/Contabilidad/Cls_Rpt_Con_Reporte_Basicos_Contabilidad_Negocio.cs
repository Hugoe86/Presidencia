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
using Presidencia.Reporte_Basicos_Contabilidad.Datos;

namespace Presidencia.Reporte_Basicos_Contabilidad.Negocio
{
    public class Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio
    {
        public Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Negocio()
        {
        }
        #region Variables Internas
            private String Cuenta_Inicial;
            private String Cuenta_Final;
            private String Saldos_Ceros;
            private String Mes_Anio;
            private String Montos_Cero;
            private String Fecha_Inicial;
            private String Fecha_Final;
            private String Poliza_Inicial;            
            private String Poliza_Final;
            private String Tipo_Polizas;
        #endregion
        #region Variables Publicas
            public String P_Cuenta_Inicial
            {
                get { return Cuenta_Inicial; }
                set { Cuenta_Inicial = value; }
            }
            public String P_Cuenta_Final
            {
                get { return Cuenta_Final; }
                set { Cuenta_Final = value; }
            }
            public String P_Saldos_Ceros
            {
                get { return Saldos_Ceros; }
                set { Saldos_Ceros = value; }
            }
            public String P_Mes_Anio
            {
                get { return Mes_Anio; }
                set { Mes_Anio = value; }
            }
            public String P_Montos_Cero
            {
                get { return Montos_Cero; }
                set { Montos_Cero = value; }
            }
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
            public String P_Poliza_Inicial
            {
                get { return Poliza_Inicial; }
                set { Poliza_Inicial = value; }
            }
            public String P_Poliza_Final
            {
                get { return Poliza_Final; }
                set { Poliza_Final = value; }
            }
            public String P_Tipo_Polizas
            {
                get { return Tipo_Polizas; }
                set { Tipo_Polizas = value; }
            }
        #endregion
        #region Metodos
            public DataTable Consulta_Balanza_Mensual_Debe_Haber()
            {
                return Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos.Consulta_Balanza_Mensual_Debe_Haber(this);
            }
            public DataTable Consulta_Balanza_Mensual()
            {
                return Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos.Consulta_Balanza_Mensual(this);
            }
            public DataTable Consulta_Diario_General()
            {
                return Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos.Consulta_Diario_General(this);
            }
            public DataTable Consulta_Diario_General_Detalles()
            {
                return Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos.Consulta_Diario_General_Detalles(this);
            }
            public DataTable Consulta_Libro_Mayor()
            {
                return Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos.Consulta_Libro_Mayor(this);
            }
            public DataTable Consulta_Libro_Diario()
            {
                return Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos.Consulta_Libro_Diario(this);
            }
            public DataSet Consulta_Tipo_Poliza()
            {
                return Cls_Rpt_Con_Reporte_Basicos_Contabilidad_Datos.Consulta_Tipo_Poliza(this);
            }
        #endregion
    }
}