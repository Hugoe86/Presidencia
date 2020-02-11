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
using Presidencia.Operacion_Resumen_Predio.Negocio;

public partial class paginas_Predial_Frm_Rep_Pre_Cuentas_Pagadas : System.Web.UI.Page
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
            //Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            //Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
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
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Convenio);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte";
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
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(
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
        var Consulta_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        var Ds_Pagos = new Ds_Rep_Pre_Cuentas_Pagadas();
        DataTable Dt_Pagos = new DataTable();
        DateTime Desde_Fecha;
        DateTime Hasta_Fecha;
        String Mensaje_Titulo = "";
        string Tipo_Pago = "";

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
        Consulta_Pagos.P_Desde_Fecha = Desde_Fecha;
        Consulta_Pagos.P_Hasta_Fecha = Hasta_Fecha;

        // pagos de traslado
        if (Opt_Pagos_Traslado.Checked == true)
        {
            Tipo_Pago = "por Traslado";
            Consulta_Pagos.P_Incluir_Propietario = true;
            Dt_Pagos = Consulta_Pagos.Consultar_Pagos_Traslado();

            Dt_Pagos.TableName = "Dt_Pagos_Traslado";
            // agregar tabla obtenida de la consulta al dataset
            if (Dt_Pagos != null)
            {
                Ds_Pagos.Tables.Remove("Dt_Pagos_Traslado");
                Ds_Pagos.Tables.Add(Dt_Pagos.Copy());
            }
        }
        // pagos de constancias
        else if (Opt_Pagos_Constancias.Checked == true)
        {
            Tipo_Pago = "por Constancias";
            Consulta_Pagos.P_Incluir_Propietario = true;
            Dt_Pagos = Consulta_Pagos.Consultar_Pagos_Constancias();

            Dt_Pagos.TableName = "Dt_Pagos_Constancias";
            // agregar tabla obtenida de la consulta al dataset
            if (Dt_Pagos != null)
            {
                Ds_Pagos.Tables.Remove("Dt_Pagos_Constancias");
                Ds_Pagos.Tables.Add(Dt_Pagos.Copy());
            }
        }
        // pagos de derechos de sup.
        else if (Opt_Pagos_Derechos_Supervision.Checked == true)
        {
            Tipo_Pago = "por Derechos de supervisión";
            Consulta_Pagos.P_Incluir_Propietario = true;
            Dt_Pagos = Consulta_Pagos.Consultar_Pagos_Derechos_Supervision();

            Dt_Pagos.TableName = "Dt_Pagos_Derechos";
            // agregar tabla obtenida de la consulta al dataset
            if (Dt_Pagos != null)
            {
                Ds_Pagos.Tables.Remove("Dt_Pagos_Derechos");
                Ds_Pagos.Tables.Add(Dt_Pagos.Copy());
            }
        }
        // pagos de Fraccionamiento
        else if (Opt_Pagos_Fraccionamientos.Checked == true)
        {
            Tipo_Pago = "por Fraccionamiento";
            Consulta_Pagos.P_Incluir_Propietario = true;
            Dt_Pagos = Consulta_Pagos.Consultar_Pagos_Fraccionamientos();

            Dt_Pagos.TableName = "Dt_Pagos_Fraccionamiento";
            // agregar tabla obtenida de la consulta al dataset
            if (Dt_Pagos != null)
            {
                Ds_Pagos.Tables.Remove("Dt_Pagos_Fraccionamiento");
                Ds_Pagos.Tables.Add(Dt_Pagos.Copy());
            }
        }
        // pagos de Predial
        else if (Opt_Pagos_Predial.Checked == true)
        {
            Tipo_Pago = "por Predial";
            Consulta_Pagos.P_Incluir_Propietario = true;
            Dt_Pagos = Consulta_Pagos.Consultar_Historial_Pagos();

            Dt_Pagos.TableName = "Dt_Pagos_Predial";
            // agregar tabla obtenida de la consulta al dataset
            if (Dt_Pagos != null)
            {
                Ds_Pagos.Tables.Remove("Dt_Pagos_Predial");
                Ds_Pagos.Tables.Add(Dt_Pagos.Copy());
            }
        }

        // agregar datos generales al dataset
        var Dr_Convenio = Ds_Pagos.Tables[0].NewRow();
        Dr_Convenio["Titulo"] = "Cuentas pagadas " + Tipo_Pago;
        Dr_Convenio["Encabezado"] = Mensaje_Titulo;
        Dr_Convenio["Fecha"] = DateTime.Now.ToString("dd-MMM-yyyy");
        Ds_Pagos.Tables[0].Rows.Add(Dr_Convenio);


        return Ds_Pagos;
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

            // pagos de traslado
            if (Opt_Pagos_Traslado.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Traslado.rpt", "Pagos_Traslado", "pdf", ExportFormatType.PortableDocFormat);
            }
            else if (Opt_Pagos_Predial.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Predial.rpt", "Pagos_Predial", "pdf", ExportFormatType.PortableDocFormat);
            }
            else if (Opt_Pagos_Fraccionamientos.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Fraccionamientos.rpt", "Pagos_Fraccionamiento", "pdf", ExportFormatType.PortableDocFormat);
            }
            else if (Opt_Pagos_Derechos_Supervision.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Derechos.rpt", "Pagos_Derechos", "pdf", ExportFormatType.PortableDocFormat);
            }
            else if (Opt_Pagos_Constancias.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Constancias.rpt", "Pagos_Constancias", "pdf", ExportFormatType.PortableDocFormat);
            }


        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Exportar a pdf: " + Ex.Message;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Exportar_Excel_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Exportar_Excel que genera el reporte en Excel
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-ene-2012
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
            if (Opt_Pagos_Traslado.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Traslado.rpt", "Pagos_Traslado", "xls", ExportFormatType.Excel);
            }
            else if (Opt_Pagos_Predial.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Predial.rpt", "Pagos_Predial", "xls", ExportFormatType.Excel);
            }
            else if (Opt_Pagos_Fraccionamientos.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Fraccionamientos.rpt", "Pagos_Fraccionamiento", "xls", ExportFormatType.Excel);
            }
            else if (Opt_Pagos_Derechos_Supervision.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Derechos.rpt", "Pagos_Derechos", "xls", ExportFormatType.Excel);
            }
            else if (Opt_Pagos_Constancias.Checked == true)
            {
                Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Rep_Pre_Cuentas_Pagadas_Traslado.rpt", "Pagos_Constancias", "xls", ExportFormatType.Excel);
            }

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Exportar a Excel: " + Ex.Message;
            Img_Error.Visible = true;
        }
    }

#endregion EVENTOS

}
