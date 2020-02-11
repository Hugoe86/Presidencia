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
using Presidencia.Control_Patrimonial_Reporte_Movimientos.Datos;

/// <summary>
/// Summary description for Cls_Rpt_Pat_Listado_Movimientos_Negocio
/// </summary>
/// 

namespace Presidencia.Control_Patrimonial_Reporte_Movimientos.Negocio {

    public class Cls_Rpt_Pat_Listado_Movimientos_Negocio{
        
            #region "Variables Internas"

                private String Busqueda_Dependencia_ID = null;
                private String Busqueda_Empleado_ID = null;
                private DateTime Busqueda_Fecha_Inicial;
                private Boolean Tomar_Fecha_Inicial = false;
                private DateTime Busqueda_Fecha_Final;
                private Boolean Tomar_Fecha_Final = false;
                private Boolean Altas = false;
                private Boolean Modificaciones = false;
                private Boolean Bajas = false;

            #endregion "Variables Internas"

            #region "Variables Publicas"

                public String P_Busqueda_Dependencia_ID {
                    get { return Busqueda_Dependencia_ID; }
                    set { Busqueda_Dependencia_ID = value; }
                }
                public String P_Busqueda_Empleado_ID {
                    get { return Busqueda_Empleado_ID; }
                    set { Busqueda_Empleado_ID = value; }
                }
                public DateTime P_Busqueda_Fecha_Inicial
                {
                    get { return Busqueda_Fecha_Inicial; }
                    set { Busqueda_Fecha_Inicial = value; }
                }
                public Boolean P_Tomar_Fecha_Inicial
                {
                    get { return Tomar_Fecha_Inicial; }
                    set { Tomar_Fecha_Inicial = value; }
                }
                public DateTime P_Busqueda_Fecha_Final
                {
                    get { return Busqueda_Fecha_Final; }
                    set { Busqueda_Fecha_Final = value; }
                }
                public Boolean P_Tomar_Fecha_Final
                {
                    get { return Tomar_Fecha_Final; }
                    set { Tomar_Fecha_Final = value; }
                }
                public Boolean P_Altas
                {
                    get { return Altas; }
                    set { Altas = value; }
                }
                public Boolean P_Modificaciones
                {
                    get { return Modificaciones; }
                    set { Modificaciones = value; }
                }
                public Boolean P_Bajas
                {
                    get { return Bajas; }
                    set { Bajas = value; }
                }

            #endregion "Variables Publicas"

            #region "Metodos"

                public DataTable Consultar_Empleados() {
                    return Cls_Rpt_Pat_Listado_Movimientos_Datos.Consultar_Empleados(this);
                }

                public DataTable Consultar_Dependecias() {
                    return Cls_Rpt_Pat_Listado_Movimientos_Datos.Consultar_Dependencias();
                }

                public DataTable Consultar_Registros_Reporte() {
                    DataTable Dt_Registros = new DataTable("DT_REGISTROS");
                    Dt_Registros.Columns.Add("MOVIMIENTO", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("FECHA", Type.GetType("System.DateTime"));
                    Dt_Registros.Columns.Add("CANTIDAD", Type.GetType("System.Int32"));
                    Dt_Registros.Columns.Add("TIPO_BIEN", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("NO_INVENTARIO", Type.GetType("System.Int64"));
                    Dt_Registros.Columns.Add("CARACTERISTICAS", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("CONDICIONES", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("DEPENDENCIA", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("RESPONSABLE", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("COSTO", Type.GetType("System.Double"));
                    Dt_Registros.Columns.Add("PROVEEDOR", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("OBSERVACIONES", Type.GetType("System.String"));
                    Dt_Registros.Columns.Add("NO_FACTURA", Type.GetType("System.Int64"));

                    return Cls_Rpt_Pat_Listado_Movimientos_Datos.Consultar_Registros_Altas(this);
                    //return Dt_Registros;
                }

            #endregion "Metodos"
    }

}