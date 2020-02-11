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
using Presidencia.Control_Patrimonial_Reportes.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Reportes_Negocio
/// </summary>
namespace Presidencia.Control_Patrimonial_Reportes.Negocio
{

    public class Cls_Rpt_Pat_Reportes_Negocio
    {

        #region Variables Internas

            private String Orden = null;
            private String Dependencia = null;
            private String Area = null;
            private String Responsable = null;
            private String Tipo = null;
            private String Clasificacion = null;
            private String Tipo_Combustible = null;
            private String Tipo_Vehiculo = null;
            private Boolean Tomar_Fecha_Inicio = false;
            private DateTime Fecha_Inicio;
            private Boolean Tomar_Fecha_Fin = false;
            private DateTime Fecha_Fin;
            private String Tipo_DataTable = null;

        #endregion

        #region Variables Publicas

            public String P_Orden
            {
                get { return Orden; }
                set { Orden = value; }
            }
            public String P_Dependencia
            {
                get { return Dependencia; }
                set { Dependencia = value; }
            }
            public String P_Area
            {
                get { return Area; }
                set { Area = value; }
            }
            public String P_Responsable
            {
                get { return Responsable; }
                set { Responsable = value; }
            }
            public String P_Tipo
            {
                get { return Tipo; }
                set { Tipo = value; }
            }
            public String P_Clasificacion
            {
                get { return Clasificacion; }
                set { Clasificacion = value; }
            }
            public String P_Tipo_Combustible
            {
                get { return Tipo_Combustible; }
                set { Tipo_Combustible = value; }
            }
            public String P_Tipo_Vehiculo
            {
                get { return Tipo_Vehiculo; }
                set { Tipo_Vehiculo = value; }
            }
            public Boolean P_Tomar_Fecha_Inicio
            {
                get { return Tomar_Fecha_Inicio; }
                set { Tomar_Fecha_Inicio = value; }
            }
            public DateTime P_Fecha_Inicio
            {
                get { return Fecha_Inicio; }
                set { Fecha_Inicio = value; }
            }
            public Boolean P_Tomar_Fecha_Fin
            {
                get { return Tomar_Fecha_Fin; }
                set { Tomar_Fecha_Fin = value; }
            }
            public DateTime P_Fecha_Fin
            {
                get { return Fecha_Fin; }
                set { Fecha_Fin = value; }
            }

            public String P_Tipo_DataTable
            {
                get { return Tipo_DataTable; }
                set { Tipo_DataTable = value; }
            }

            

        #endregion

        #region Metodos

            public DataSet Consultar_Datos_Reporte_Licencias_Vencidas() {
                return Cls_Rpt_Pat_Reportes_Datos.Consultar_Datos_Reporte_Licencias_Vencidas(this);
            }

            public DataTable Consultar_DataTable() {
                return Cls_Rpt_Pat_Reportes_Datos.Consultar_DataTable(this);
            }

        #endregion

    }

}