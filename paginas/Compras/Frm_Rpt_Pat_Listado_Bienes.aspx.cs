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
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;

public partial class paginas_Compras_Frm_Rpt_Pat_Listado_Bienes : System.Web.UI.Page {

#region  Page Load
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo Inicial de la página.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e) {
        Div_Contenedor_Msj_Error.Visible = false;
        try {
            if (!IsPostBack) {
                Llenar_Combo_Dependencias();
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

#endregion

#region Metodos

     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta.
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Bienes.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Rpt_Pat_Listado_Bienes.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta.
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Exportar_Excel(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte) {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Bienes.xls");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Rpt_Pat_Listado_Bienes.xls";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Independientes
    ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:  28/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Dependencias() {
        try {
            Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();

            //SE LLENA EL COMBO DE DEPENDENCIAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "DEPENDENCIAS";
            DataTable Dependencias = Combos.Consultar_DataTable();
            DataRow Fila_Dependencia = Dependencias.NewRow();
            Fila_Dependencia["DEPENDENCIA_ID"] = "TODAS";
            Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;-- TODAS --&gt;");
            Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
            Cmb_Busqueda_Resguardantes_Dependencias.DataSource = Dependencias;
            Cmb_Busqueda_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Resguardantes_Dependencias.DataTextField = "NOMBRE";
            Cmb_Busqueda_Resguardantes_Dependencias.DataBind();

        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados_Busqueda
    ///DESCRIPCIÓN: Llena el combo de Empleados del Modal de Busqueda.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:  28/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Empleados_Busqueda(DataTable Tabla) {
        try {
            DataRow Fila_Empleado = Tabla.NewRow();
            Fila_Empleado["EMPLEADO_ID"] = "TODOS";
            Fila_Empleado["NOMBRE"] = HttpUtility.HtmlDecode("&lt;-- TODOS --&gt;");
            Tabla.Rows.InsertAt(Fila_Empleado, 0);
            Cmb_Busqueda_Nombre_Resguardante.DataSource = Tabla;
            Cmb_Busqueda_Nombre_Resguardante.DataValueField = "EMPLEADO_ID";
            Cmb_Busqueda_Nombre_Resguardante.DataTextField = "NOMBRE";
            Cmb_Busqueda_Nombre_Resguardante.DataBind();
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

#endregion
    
#region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:  28/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Busqueda_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
        try {
            if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0) {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Combo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Combo.P_Tipo_DataTable = "EMPLEADOS";
                Combo.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                DataTable Tabla = Combo.Consultar_DataTable();
                Llenar_Combo_Empleados_Busqueda(Tabla);
            } else {
                DataTable Tabla = new DataTable();
                Tabla.Columns.Add("EMPLEADO_ID", Type.GetType("System.String"));
                Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                Llenar_Combo_Empleados_Busqueda(Tabla);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
    ///DESCRIPCIÓN: Manda llamar los datos para cergarlos en el reporte dependiendo de
    ///             los filtros seleccionados.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
        try {
            //Cls_Rpt_Pat_Listado_Bienes_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            //Reporte_Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value;
            //if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0)
            //{
            //    Reporte_Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text.Trim());
            //    Reporte_Negocio.P_Tomar_Fecha_Inicial = true;
            //}
            //if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) {
            //    Reporte_Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text.Trim());
            //    Reporte_Negocio.P_Tomar_Fecha_Final = true;
            //}
            //if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0)
            //{
            //    Reporte_Negocio.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value;
            //}
            //if (Cmb_Busqueda_Nombre_Resguardante.SelectedIndex > 0)
            //{
            //    Reporte_Negocio.P_Resguardante_ID = Cmb_Busqueda_Nombre_Resguardante.SelectedItem.Value;
            //}
            //DataTable Dt_Resultados_Reporte = Reporte_Negocio.Obtener_Datos_Reporte();
            //Dt_Resultados_Reporte.TableName = "DATOS_GENERALES";
            //DataSet Ds_Consulta = new DataSet();
            //Ds_Consulta.Tables.Add(Dt_Resultados_Reporte);
            //Ds_Rpt_Pat_Listado_Bienes Ds_Reporte = new Ds_Rpt_Pat_Listado_Bienes();
            //Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Bienes.rpt");
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Excel_Click
    ///DESCRIPCIÓN: Manda llamar los datos para cergarlos en el reporte dependiendo de
    ///             los filtros seleccionados.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:  30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try {
            //Cls_Rpt_Pat_Listado_Bienes_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            //Reporte_Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value;
            //if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0)
            //{
            //    Reporte_Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text.Trim());
            //    Reporte_Negocio.P_Tomar_Fecha_Inicial = true;
            //}
            //if (Cmb_Busqueda_Resguardantes_Dependencias.SelectedIndex > 0)
            //{
            //    Reporte_Negocio.P_Dependencia_ID = Cmb_Busqueda_Resguardantes_Dependencias.SelectedItem.Value;
            //}
            //if (Cmb_Busqueda_Nombre_Resguardante.SelectedIndex > 0)
            //{
            //    Reporte_Negocio.P_Resguardante_ID = Cmb_Busqueda_Nombre_Resguardante.SelectedItem.Value;
            //}
            //DataTable Dt_Resultados_Reporte = Reporte_Negocio.Obtener_Datos_Reporte();
            //Dt_Resultados_Reporte.TableName = "DATOS_GENERALES";
            //DataSet Ds_Consulta = new DataSet();
            //Ds_Consulta.Tables.Add(Dt_Resultados_Reporte);
            //Ds_Rpt_Pat_Listado_Bienes Ds_Reporte = new Ds_Rpt_Pat_Listado_Bienes();
            //Exportar_Excel(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Bienes.rpt");
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

#endregion

}
