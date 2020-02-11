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
using System.Data.OracleClient;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Cargar_Presupuesto_Calendarizado.Datos;
/// <summary>
/// Summary description for Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio
/// </summary>
/// 
namespace Presidencia.Cargar_Presupuesto_Calendarizado.Negocio
{
    public class Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio
    {
        private String Tipo;
        private String Anio;

        public String P_Tipo
        {
            get { return Tipo; }
            set { Tipo = value; }
        }
        public String P_Anio
        {
            get { return Anio; }
            set { Anio = value; }
        }
        #region MÉTODOS
        public Cls_Ope_Psp_Cargar_Presup_Calendarizado_Negocio()
        {
        }
        public int Consultar_Presupuesto_Calendarizado()
        {
            return 0;
        }
        public DataTable Consultar_Anios_Presupuestados()
        {
            return Cls_Ope_Psp_Cargar_Presup_Calendarizado_Datos.Consultar_Anios_Presupuestados();
        }
        public double Consultar_Importe_Presupuesto_Aprobado()
        {
            return Cls_Ope_Psp_Cargar_Presup_Calendarizado_Datos.Consultar_Importe_Presupuesto_Aprobado(this);
        }
        #endregion
    }
}
