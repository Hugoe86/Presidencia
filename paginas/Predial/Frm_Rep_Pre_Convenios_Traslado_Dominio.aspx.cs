using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reportes_Predial_Convenios.Negocio;

public partial class paginas_Predial_Frm_Rep_Pre_Convenios_Traslado_Dominio : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion

#region METODOS

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 23-jul-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            // inicializar campos con fecha actual
            Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            // si la tabla no trae datos, mostrar mensaje
            if (Ds_Convenio.Tables[1].Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Text = "No se encontraron registros con el criterio seleccionado.";
                Lbl_Mensaje_Error.Visible = true;
                return;
            }

            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Convenio);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_Convenio = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(Archivo_Convenio, "Convenio");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Exportar_Reporte
    ///DESCRIPCIÓN          : Crea un Dataset con los datos de la consulta de convenios
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 11-ene-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Exportar_Reporte()
    {
        var Consulta_Convenios = new Cls_Rep_Pre_Convenios_Negocio();
        var Ds_Convenio = new Ds_Rep_Pre_Convenios_Predial();
        DataTable Dt_Convenios;
        DateTime Desde_Fecha;
        DateTime Hasta_Fecha;
        String Mensaje_Titulo = "";
        String Mensaje_Incluidos = "";

        // verificar si el usuario selecciono un estatus
        if (Cmb_Estatus.SelectedIndex > 0)
        {
            //  pasar el dato a la capa de negocio para la consulta y crear mensaje para encabezado
            Consulta_Convenios.P_Estatus = Cmb_Estatus.SelectedValue;
            Mensaje_Incluidos += " con estatus " + Cmb_Estatus.SelectedValue;
        }

        // tratar de obtener fecha inicial
        if (DateTime.TryParse(Txt_Fecha_Inicial.Text, out Desde_Fecha) && Desde_Fecha != DateTime.MinValue)
        {
            // mensaje para encabezado
            Mensaje_Titulo += " Desde el " + Desde_Fecha.ToString("d-MMM-yyyy");
        }
        else
        {
            Txt_Fecha_Inicial.Text = "";
        }

        // tratar de obtener fecha final
        if (DateTime.TryParse(Txt_Fecha_Final.Text, out Hasta_Fecha) && Hasta_Fecha != DateTime.MinValue)
        {
            // mensaje para encabezado
            Mensaje_Titulo += " Hasta el " + Hasta_Fecha.ToString("d-MMM-yyyy");
        }
        else
        {
            Txt_Fecha_Final.Text = "";
        }

        // pasar el dato a la capa de negocio para la consulta
        Consulta_Convenios.P_Desde_Fecha = Desde_Fecha;
        Consulta_Convenios.P_Hasta_Fecha = Hasta_Fecha;

        // validar si el usuario selecciono opciones en el combo incluir reestructuras
        if (Cmb_Incluir_Reestructuras.SelectedIndex > 0)
        {
            // validar si se selecciono solo convenios
            if (Cmb_Incluir_Reestructuras.SelectedIndex == 1)
            {
                //  pasar el dato a la capa de negocio para la consulta
                Consulta_Convenios.P_Solo_Convenios = true;
                Mensaje_Incluidos = "Convenios de traslado de dominio" + Mensaje_Incluidos;
            }

            // validar si se selecciono solo convenios
            else if (Cmb_Incluir_Reestructuras.SelectedIndex == 2)
            {
                //  pasar el dato a la capa de negocio para la consulta
                Consulta_Convenios.P_Solo_Reestructuras = true;
                Mensaje_Incluidos = "Reestructuras de traslado de dominio" + Mensaje_Incluidos;
            }
        }
        else
        {
            Mensaje_Incluidos = "Convenios y reestructuras de traslado de dominio" + Mensaje_Incluidos;
        }

        // agregar datos generales al dataset
        var Dr_Convenio = Ds_Convenio.Tables[0].NewRow();
        Dr_Convenio["Titulo"] = "Convenios de traslado de dominio";
        Dr_Convenio["Encabezado"] = Mensaje_Titulo;
        Dr_Convenio["Mensaje_Incluidos"] = Mensaje_Incluidos;
        Dr_Convenio["Fecha"] = DateTime.Now.ToString("dd-MMM-yyyy");
        Ds_Convenio.Tables[0].Rows.Add(Dr_Convenio);

        // llamar metodo de consulta
        Dt_Convenios = Consulta_Convenios.Consultar_Convenios_Traslado_Reporte().Copy();
        Dt_Convenios.TableName = "Dt_Convenios_Predial";
        // agregar tabla obtenida de la consulta al dataset
        if (Dt_Convenios != null)
        {
            Ds_Convenio.Tables.Remove("Dt_Convenios_Predial");
            Ds_Convenio.Tables.Add(Dt_Convenios);
        }

        return Ds_Convenio;
    }

#endregion METODOS


#region EVENTOS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Exportar_pdf_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Exportar_pdf que genera el reporte en pdf
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Exportar_pdf_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Convenios_Traslado_Dominio.rpt", "Reporte_Convenios_Traslado", "pdf", ExportFormatType.PortableDocFormat);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Exportar convenio a pdf: " + Ex.Message;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Exportar_Excel_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Exportar_Excel que genera el reporte en Excel
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-ene-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Convenios_Traslado_Dominio.rpt", "Reporte_Convenios_Traslado", "xls", ExportFormatType.Excel);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Exportar convenio a excel: " + Ex.Message;
            Img_Error.Visible = true;
        }
    }

#endregion EVENTOS

}
