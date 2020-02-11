using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Predial_Pae_Bienes.Negocio;
using Presidencia.Catalogo_Tipos_Bienes.Negocio;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using Presidencia.Sessiones;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;
using Presidencia.Operacion_Predial_Pae_Cuentas_Ingresadas.Negocio;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Cuentas_Ingresadas : System.Web.UI.Page
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones           
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region METODOS

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mostrar_Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mostrar_Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Encabezado_Error.Text = "";
        Lbl_Encabezado_Error.Text = P_Mensaje + "</br>";
    }

    private void Limpia_Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Encabezado_Error.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Fecha_Ingreso
    ///DESCRIPCIÓN: Carga en el combo las diferentes fechas en las que ingresaron cuentas a etapas del PAE
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 10-may-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Fecha_Ingreso()
    {
        DataTable Dt_Fechas_Ingreso = new DataTable();
        try
        {
            var Consulta_Fechas_Ingreso = new Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Negocio();
            Dt_Fechas_Ingreso = Consulta_Fechas_Ingreso.Consulta_Fechas_Etapas();

            Cmb_Fecha_Ingreso.DataTextField = Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio;
            Cmb_Fecha_Ingreso.DataValueField = Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio;
            Cmb_Fecha_Ingreso.DataSource = Dt_Fechas_Ingreso;
            Cmb_Fecha_Ingreso.DataBind();
            Cmb_Fecha_Ingreso.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Inicializa_Controles
    /// DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar diferentes operaciones
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Inicializa_Controles()
    {
        Limpia_Mensaje_Error();
        try
        {
            Cargar_Combo_Fecha_Ingreso();
            Grid_Cuentas_Ingresadas.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Grid_Cuentas_Ingresadas
    /// DESCRIPCIÓN: Carga en el grid Cuentas ingresadas filtrando el datatable que recibe como parámetro
    /// PARÁMETROS: 
    ///         1. Dt_Cuentas: datatable con información que se va a cargar en el grid
    ///         2. Pagina: pagina del grid que se va a mostrar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected int Cargar_Grid_Cuentas_Ingresadas(DataTable Dt_Cuentas, int Pagina)
    {
        DataView Dv_Cuentas_Ingresadas = new DataView();

        // filtrar en una vista las cuentas no omitidas
        Dv_Cuentas_Ingresadas.Table = Dt_Cuentas;
        Dv_Cuentas_Ingresadas.RowFilter = Ope_Pre_Pae_Det_Etapas.Campo_Omitida + " = 'NO'";
        Dv_Cuentas_Ingresadas.Sort = Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " DESC";
        // cargar en el grid las cuentas ingresadas
        Grid_Cuentas_Ingresadas.PageIndex = Pagina;
        Grid_Cuentas_Ingresadas.SelectedIndex = -1;
        Grid_Cuentas_Ingresadas.DataSource = Dv_Cuentas_Ingresadas;
        for (int i = 7; i < Grid_Cuentas_Ingresadas.Columns.Count; i++)
        {
            Grid_Cuentas_Ingresadas.Columns[i].Visible = true;
        }
        Grid_Cuentas_Ingresadas.DataSource = Dv_Cuentas_Ingresadas;
        Grid_Cuentas_Ingresadas.DataBind();
        for (int i = 7; i < Grid_Cuentas_Ingresadas.Columns.Count; i++)
        {
            Grid_Cuentas_Ingresadas.Columns[i].Visible = false;
        }

        return Dv_Cuentas_Ingresadas.Count;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Grid_Cuentas_Omitidas
    /// DESCRIPCIÓN: Carga en el grid Cuentas Omitidas filtrando el datatable que recibe como parámetro
    /// PARÁMETROS: 
    ///         1. Dt_Cuentas: datatable con información que se va a cargar en el grid
    ///         2. Pagina: pagina del grid que se va a mostrar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected int Cargar_Grid_Cuentas_Omitidas(DataTable Dt_Cuentas, int Pagina)
    {
        DataView Dv_Cuentas_Omitidas = new DataView();

        // filtrar en una vista las cuentas omitidas
        Dv_Cuentas_Omitidas.Table = Dt_Cuentas;
        Dv_Cuentas_Omitidas.RowFilter = Ope_Pre_Pae_Det_Etapas.Campo_Omitida + " = 'SI'";
        Dv_Cuentas_Omitidas.Sort = Ope_Pre_Pae_Detalles_Cuentas.Campo_Fecha_Proceso_Cambio + " DESC";
        // cargar en el grid las cuentas omitidas
        Grid_Cuentas_Omitidas.PageIndex = Pagina;
        Grid_Cuentas_Omitidas.SelectedIndex = -1;
        Grid_Cuentas_Omitidas.DataSource = Dv_Cuentas_Omitidas;
        for (int i = 8; i < Grid_Cuentas_Omitidas.Columns.Count; i++)
        {
            Grid_Cuentas_Omitidas.Columns[i].Visible = true;
        }
        Grid_Cuentas_Omitidas.DataSource = Dv_Cuentas_Omitidas;
        Grid_Cuentas_Omitidas.DataBind();
        for (int i = 8; i < Grid_Cuentas_Omitidas.Columns.Count; i++)
        {
            Grid_Cuentas_Omitidas.Columns[i].Visible = false;
        }

        Txt_Despacho_Asignado.Text = "";
        Txt_Numero_Entrega.Text = "";

        return Dv_Cuentas_Omitidas.Count;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal reports con los datos proporcionados en el Dataset
    /// PARÁMETROS:
    /// 		1. Ds_Datos: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Datos, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            // si la tabla no trae datos, mostrar mensaje
            if (Ds_Datos.Tables[1].Rows.Count <= 0)
            {
                Mostrar_Mensaje_Error("No se encontraron registros con el criterio seleccionado.");
                return;
            }

            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Datos);
        }
        catch
        {
            Mostrar_Mensaje_Error("No se pudo cargar el reporte");
        }

        String Archivo_Convenio = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 

        ExportOptions Export_Options_Calculo = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
        Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Convenio);
        Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
        Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options_Calculo.ExportFormatType = Formato;
        Reporte.Export(Export_Options_Calculo);

        Mostrar_Reporte(Archivo_Convenio, "Convenio");
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
    ///DESCRIPCIÓN          : Crea un Dataset con los datos en la sesión
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 12-may-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Exportar_Reporte()
    {
        var Ds_Datos = new Ds_Ope_Pre_PAE_Cuentas_Ingresadas();
        DataTable Dt_Cuentas_Ingresadas = null;
        DateTime Desde_Fecha;
        DateTime Hasta_Fecha;
        String Mensaje_Titulo = "";

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

        if (Cmb_Fecha_Ingreso.SelectedIndex > 0)
        {
            Mensaje_Titulo = "Ingresadas el día " + Cmb_Fecha_Ingreso.SelectedValue;
        }

        if (Cmb_Etapa_PAE.SelectedIndex > 0)
        {
            Mensaje_Titulo += " En proceso " + Cmb_Etapa_PAE.SelectedItem.Text;
        }

        // agregar datos generales al dataset
        var Dr_Datos_Generales = Ds_Datos.Tables["Dt_Generales"].NewRow();
        Dr_Datos_Generales["Titulo"] = "Cuentas ingresadas al PAE";
        Dr_Datos_Generales["Encabezado"] = Mensaje_Titulo;
        Dr_Datos_Generales["Fecha"] = DateTime.Now.ToString("dd-MMM-yyyy");
        Ds_Datos.Tables["Dt_Generales"].Rows.Add(Dr_Datos_Generales);

        if (Session["Dt_Cuentas_Ingresadas"] != null)
        {
            Dt_Cuentas_Ingresadas = (DataTable)Session["Dt_Cuentas_Ingresadas"];
            Dt_Cuentas_Ingresadas.TableName = "Dt_Cuentas_Ingresadas";
        }
        // agregar tabla obtenida de la consulta al dataset
        if (Dt_Cuentas_Ingresadas != null)
        {
            Ds_Datos.Tables.Remove("Dt_Cuentas_Ingresadas");
            Ds_Datos.Tables.Add(Dt_Cuentas_Ingresadas.Copy());
        }

        return Ds_Datos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Buscar_Cuentas_PAE
    /// DESCRIPCIÓN: Buscar cuentas ingresadas al PAE de acuerdo con los filtros especificados
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Buscar_Cuentas_PAE()
    {
        var Consulta_Cuentas_Ingresadas = new Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Negocio();
        DataTable Dt_Cuentas = null;
        DateTime Fecha_Inicial;
        DateTime Fecha_Final;
        DateTime Fecha_Ingreso;
        DataView Dv_Cuentas_Omitidas = new DataView();
        DataView Dv_Cuentas_Ingresadas = new DataView();

        // agregar filtro por despacho a la consulta si hay un despacho seleccionado
        if (Cmb_Etapa_PAE.SelectedIndex > 0)
        {
            Consulta_Cuentas_Ingresadas.P_Proceso_Actual = Cmb_Etapa_PAE.SelectedValue;
        }

        // agregar filtro por Fecha de ingreso a la consulta
        if (Cmb_Fecha_Ingreso.SelectedIndex > 0 && DateTime.TryParse(Cmb_Fecha_Ingreso.SelectedValue, out Fecha_Ingreso))
        {
            Consulta_Cuentas_Ingresadas.P_Fecha_Ingreso = Fecha_Ingreso;
        }

        // agregar filtro hasta fecha a la consulta
        if (DateTime.TryParse(Txt_Fecha_Final.Text, out Fecha_Final))
        {
            Consulta_Cuentas_Ingresadas.P_Fecha_Final = Fecha_Final;
        }

        // agregar filtro desde Fecha a la consulta
        if (DateTime.TryParse(Txt_Fecha_Inicial.Text, out Fecha_Inicial))
        {
            Consulta_Cuentas_Ingresadas.P_Fecha_Inicial = Fecha_Inicial;
        }

        // ejecutar consulta
        Dt_Cuentas = Consulta_Cuentas_Ingresadas.Consulta_Cuentas_Ingresadas();

        // cargar grids
        Txt_Total_Cuentas_Ingresadas.Text = Cargar_Grid_Cuentas_Ingresadas(Dt_Cuentas, 0).ToString();

        Txt_Total_Cuentas_Omitidas.Text = Cargar_Grid_Cuentas_Omitidas(Dt_Cuentas, 0).ToString();

        // guardar tabla en sesión
        Session["Dt_Cuentas_Ingresadas"] = Dt_Cuentas;

    }

    #endregion METODOS

    #region EVENTOS
    #region Texbox
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Inicial_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Inicial_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Inicial.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Inicial.Text, out Fecha_valida))
            {
                Txt_Fecha_Inicial.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Inicial.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Final_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Final_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Final.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Final.Text, out Fecha_valida))
            {
                Txt_Fecha_Final.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Final.Text = "";
            }
        }
    }
    #endregion

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Exportar_pdf_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Exportar_pdf que genera el reporte en pdf
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Exportar_pdf_Click(object sender, ImageClickEventArgs e)
    {
        Limpia_Mensaje_Error();

        try
        {
            Buscar_Cuentas_PAE();
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Ope_Pre_PAE_Cuentas_Ingresadas.rpt", "Cuentas_PAE", "pdf", ExportFormatType.PortableDocFormat);
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error("Exportar a pdf: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Exportar_Excel_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Exportar_Excel que genera el reporte en Excel
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Limpia_Mensaje_Error();

        try
        {
            // solo si hay datos en los grids, si no, mostrar mensaje
            Buscar_Cuentas_PAE();
            Exportar_Reporte(Crear_Ds_Exportar_Reporte(), "Rpt_Ope_Pre_PAE_Cuentas_Ingresadas.rpt", "Cuentas_PAE", "xls", ExportFormatType.Excel);
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error("Exportar a Excel: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta) o Sale del Formulario.
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Session.Remove("Bienes");
                Session.Remove("Lista_Bienes_Eliminar");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                HttpContext.Current.Session.Remove("Activa");
            }
            else
            {
                Inicializa_Controles();
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Cuentas_PAE_Click
    /// DESCRIPCIÓN: Buscar cuentas ingresadas al PAE de acuerdo con los filtros especificados
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 10-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Cuentas_PAE_Click(object sender, ImageClickEventArgs e)
    {
        Limpia_Mensaje_Error();//Limpia el mensaje error

        try
        {
            Buscar_Cuentas_PAE();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error("Btn_Buscar: " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// DESCRIPCIÓN: Buscar cuentas ingresadas al PAE con el folio especificado
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        var Consulta_Cuentas_Ingresadas = new Cls_Ope_Pre_Pae_Cuentas_Ingresadas_Negocio();
        DataTable Dt_Cuentas = null;
        int Folio;
        DataView Dv_Cuentas_Omitidas = new DataView();
        DataView Dv_Cuentas_Ingresadas = new DataView();

        Limpia_Mensaje_Error();//Limpia el mensaje error

        try
        {
            // agregar filtro por despacho a la consulta si hay un despacho seleccionado
            if (Txt_Busqueda.Text.Trim().Length > 0)
            {
                Consulta_Cuentas_Ingresadas.P_Folio = Txt_Busqueda.Text.Trim();
            }

            // ejecutar consulta
            Dt_Cuentas = Consulta_Cuentas_Ingresadas.Consulta_Cuentas_Ingresadas();

            // cargar grids 
            Txt_Total_Cuentas_Ingresadas.Text = Cargar_Grid_Cuentas_Ingresadas(Dt_Cuentas, 0).ToString();

            Txt_Total_Cuentas_Omitidas.Text = Cargar_Grid_Cuentas_Omitidas(Dt_Cuentas, 0).ToString();

            // guardar tabla en sesión
            Session["Dt_Cuentas_Ingresadas"] = Dt_Cuentas;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error("Btn_Buscar: " + ex.Message.ToString());
        }
    }

    #endregion EVENTOS

    #region GRIDS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Cuentas_Ingresadas_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento cambio de página del grid Grid_Cuentas_Ingresadas
    ///             recupera la tabla de cuentas ingresadas de la sesión y llama al método que carga los 
    ///             datos en el grid
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Cuentas_Ingresadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Limpia_Mensaje_Error();
        try
        {
            if (Session["Dt_Cuentas_Ingresadas"] != null)
            {
                DataTable Dt_Cuentas = (DataTable)Session["Dt_Cuentas_Ingresadas"];
                Cargar_Grid_Cuentas_Ingresadas(Dt_Cuentas, e.NewPageIndex);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Cuentas_Ingresadas_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de índice seleccionado en el grid
    ///             
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************   
    protected void Grid_Cuentas_Ingresadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Limpia_Mensaje_Error();

        try
        {
            if (Grid_Cuentas_Ingresadas.SelectedIndex > -1)
            {
                Txt_Despacho_Asignado.Text = Grid_Cuentas_Ingresadas.SelectedRow.Cells[9].Text;
                Txt_Numero_Entrega.Text = Grid_Cuentas_Ingresadas.SelectedRow.Cells[10].Text;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Cuentas_Omitidas_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento cambio de página del grid Grid_Cuentas_Omitidas
    ///             recupera la tabla de cuentas ingresadas de la sesión, filtra las omitidas con un dataview 
    ///             y las muestra en el grid
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Cuentas_Omitidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Limpia_Mensaje_Error();
        try
        {
            if (Session["Dt_Cuentas_Ingresadas"] != null)
            {
                DataTable Dt_Cuentas = (DataTable)Session["Dt_Cuentas_Ingresadas"];
                Cargar_Grid_Cuentas_Omitidas(Dt_Cuentas, e.NewPageIndex);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Cuentas_Omitidas_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de índice seleccionado en el grid
    ///             
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************   
    protected void Grid_Cuentas_Omitidas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Limpia_Mensaje_Error();

        try
        {
            // validar que haya una fila seleccionada
            if (Grid_Cuentas_Omitidas.SelectedIndex > -1)
            {
                Txt_Despacho_Asignado.Text = Grid_Cuentas_Omitidas.SelectedRow.Cells[10].Text;
                Txt_Numero_Entrega.Text = Grid_Cuentas_Omitidas.SelectedRow.Cells[11].Text;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    #endregion GRIDS

}
