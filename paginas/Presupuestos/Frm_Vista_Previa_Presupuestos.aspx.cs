using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;


public partial class paginas_presupuestos_Frm_Vista_Previa_Presupuestos : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "DESC";
            Grid_Presupuestos.DataSource = (DataTable)Session["Dt_Presupuestos"];
            Grid_Presupuestos.DataBind();
        }
    }

    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF, string Formato)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Presupuestos/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        if(Formato=="PDF")
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        if(Formato=="Excel")
            Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reportes/" + Nombre_PDF;
        Mostrar_Reporte(Nombre_PDF, Formato);
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }



    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    protected void Grid_Presupuestos_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Sorting(Grid_Presupuestos, ((DataTable)Session["Dt_Presupuestos"]), e);
    }

    private void Grid_Sorting(GridView Grid, DataTable Dt_Table, GridViewSortEventArgs e)
    {
        if (Dt_Table != null)
        {
            DataView Dv_Vista = new DataView(Dt_Table);
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dv_Vista.Sort = e.SortExpression + " DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Vista.Sort = e.SortExpression + " ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Grid.DataSource = Dv_Vista;
            Grid.DataBind();
            Session["Dt_Presupuestos"] = (DataTable)Dv_Vista.Table;
        }
    }
    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Btn_Exportar_PDF_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el evento del Btn_Exportar_PDF_Click
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Exportar_PDF_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Presupuestos = (DataTable)Session["Dt_Presupuestos"];
        String DESCRIPCION_FILTROS = Session["Filtros_Seleccionados"].ToString().Trim();
        DataTable Dt_Detalle_Filtros = new DataTable();
        DataSet Ds_Rep_Presupuestos = new DataSet();
        Dt_Detalle_Filtros.Columns.Add("DESCRIPCION_FILTROS", typeof(System.String));
        DataRow Fila_Nueva = Dt_Detalle_Filtros.NewRow();
        Fila_Nueva["DESCRIPCION_FILTROS"]= DESCRIPCION_FILTROS;
        Dt_Detalle_Filtros.Rows.Add(Fila_Nueva);
        Dt_Detalle_Filtros.AcceptChanges();
        
        Ds_Rep_Presupuestos.Tables.Add(Dt_Detalle_Filtros.Copy());
        Ds_Rep_Presupuestos.Tables[0].TableName = "Dt_Detalle_Filtros";
        Ds_Rep_Presupuestos.AcceptChanges();
        Ds_Rep_Presupuestos.Tables.Add(Dt_Presupuestos.Copy());
        Ds_Rep_Presupuestos.Tables[1].TableName = "Dt_Presupuestos";
        Ds_Rep_Presupuestos.AcceptChanges();
        Ds_Reporte_Presupuestos Obj_Rpt = new Ds_Reporte_Presupuestos();

        Generar_Reporte(Ds_Rep_Presupuestos, Obj_Rpt, "Rpt_Reporte_Presupuestos.rpt", "Rpt_Reporte_Presupuestos.pdf","PDF");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Btn_Exportar_Excel_Click
    ///DESCRIPCIÓN: Metodo que ejecuta el evento del Btn_Exportar_Excel_Click
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 19/OCT/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Presupuestos = (DataTable)Session["Dt_Presupuestos"];
        String DESCRIPCION_FILTROS = Session["Filtros_Seleccionados"].ToString().Trim();
        DataTable Dt_Detalle_Filtros = new DataTable();
        DataSet Ds_Rep_Presupuestos = new DataSet();
        Dt_Detalle_Filtros.Columns.Add("DESCRIPCION_FILTROS", typeof(System.String));
        DataRow Fila_Nueva = Dt_Detalle_Filtros.NewRow();
        Fila_Nueva["DESCRIPCION_FILTROS"] = DESCRIPCION_FILTROS;
        Dt_Detalle_Filtros.Rows.Add(Fila_Nueva);
        Dt_Detalle_Filtros.AcceptChanges();

        Ds_Rep_Presupuestos.Tables.Add(Dt_Detalle_Filtros.Copy());
        Ds_Rep_Presupuestos.Tables[0].TableName = "Dt_Detalle_Filtros";
        Ds_Rep_Presupuestos.AcceptChanges();
        Ds_Rep_Presupuestos.Tables.Add(Dt_Presupuestos.Copy());
        Ds_Rep_Presupuestos.Tables[1].TableName = "Dt_Presupuestos";
        Ds_Rep_Presupuestos.AcceptChanges();
        Ds_Reporte_Presupuestos Obj_Rpt = new Ds_Reporte_Presupuestos();

        Generar_Reporte(Ds_Rep_Presupuestos, Obj_Rpt, "Rpt_Reporte_Presupuestos.rpt", "Rpt_Reporte_Presupuestos.xls", "Excel");
    }
    #endregion

   
}
