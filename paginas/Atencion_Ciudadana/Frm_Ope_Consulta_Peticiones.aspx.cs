using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Consulta_Peticiones.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;
using Presidencia.Colonias.Negocios;
using Presidencia.Programas_AC.Negocio;
using Presidencia.Asuntos_AC.Negocio;
using Presidencia.Constantes;
using System.Web;
using System.IO;
using System.Text;

public partial class paginas_Atencion_Ciudadana_Frm_Ope_Consulta_Peticiones : System.Web.UI.Page
{
    #region Page Load / Init
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga y refresca la página llenando el combo de dependencias
    ///PARAMETROS:  
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 26/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager_Consulta_Peticiones.RegisterPostBackControl(Btn_Exportar_Excel);

        if (!Page.IsPostBack)
        {
            LLenar_Combos();
            // inicializar fecha día actual
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

            // registro de scripts del lado del servidor para mostrar ventanas emergentes para búsqueda avanzada
            string Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Asuntos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Asunto.Attributes.Add("onclick", Ventana_Modal);
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);
        }
    }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Consulta_Peticiones_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene la información de la petición seleccionada en
    ///el grid y la coloca en el modal popup para poder verla de forma mas detallada
    ///PARAMETROS:  
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Consulta_Peticiones_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Neg_Consulta_Peticiones = new Cls_Ope_Consulta_Peticiones_Negocio();

        Lbl_Informacion.Text = "";
        Img_Advertencia.Visible = false;
        Lbl_Informacion.Enabled = false;

        Neg_Consulta_Peticiones.P_Folio = Grid_Consulta_Peticiones.SelectedRow.Cells[1].Text;
        DataSet Data_Set = Neg_Consulta_Peticiones.Consulta_Peticion_Detallada();
        if (Data_Set != null && Data_Set.Tables.Count > 0 && Data_Set.Tables[0].Rows.Count > 0)
        {
            string Domicilio = Data_Set.Tables[0].Rows[0]["DIRECCION"].ToString();
            string Referencia = Data_Set.Tables[0].Rows[0][Ope_Ate_Peticiones.Campo_Referencia].ToString().Trim();
            // si hay una referencia, agregarla a la dirección
            if (Referencia.Length > 0)
            {
                Domicilio = Domicilio + " " + Referencia;
            }
            Data_Set.Tables[0].Rows[0].BeginEdit();
            Data_Set.Tables[0].Rows[0]["DIRECCION"] = Domicilio;
            Data_Set.Tables[0].AcceptChanges();

            Ds_Ope_Consulta_Peticiones_Especifico ds_consulta_peticiones = new Ds_Ope_Consulta_Peticiones_Especifico();
            Generar_Reporte(Data_Set, ds_consulta_peticiones, "Rpt_Ope_Consulta_Peticiones_Especifico.rpt");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Consulta_Peticiones_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Consulta_Peticiones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Mostrar_Informacion("");

        if (Session["Dt_Peticiones"] != null)
        {

            DataTable Dt_Peticiones = (DataTable)Session["Dt_Peticiones"];
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dt_Peticiones.DefaultView.Sort = e.SortExpression + " DESC";
                Grid_Consulta_Peticiones.DataSource = Dt_Peticiones;
                Grid_Consulta_Peticiones.DataBind();
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dt_Peticiones.DefaultView.Sort = e.SortExpression + " ASC";
                Grid_Consulta_Peticiones.DataSource = Dt_Peticiones;
                Grid_Consulta_Peticiones.DataBind();
                ViewState["SortDirection"] = "ASC";
            }

        }
    }

    #endregion Grid

    #region Métodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Habilita o deshabilita la muestra en pantalla del mensaje 
    ///de Mostrar_Informacion para el usuario
    ///PARAMETROS: 1.- Condición, entero para saber si es 1 habilita para que se muestre mensaje si es cero
    ///deshabilita para que no se muestre el mensaje
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 23/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion(int Condicion)
    {
        if (Condicion == 1)
        {
            Lbl_Informacion.Enabled = true;
            Img_Advertencia.Visible = true;
        }
        else
        {
            Lbl_Informacion.Text = "";
            Lbl_Informacion.Enabled = false;
            Img_Advertencia.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Muestra en la página el mensaje que se recibe como parámetro
    ///PARAMETROS: 1.- Mensaje: cadena de texto a mostrar como mensaje en la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion(string Mensaje)
    {
        if (!String.IsNullOrEmpty(Mensaje))
        {
            Lbl_Informacion.Text = Mensaje;
            Lbl_Informacion.Enabled = true;
            Img_Advertencia.Visible = true;
        }
        else
        {
            Lbl_Informacion.Text = "";
            Lbl_Informacion.Enabled = false;
            Img_Advertencia.Visible = false;
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.- Data_Set.- contiene la información de la consulta a la base de datos
    ///             2.-Ds_Reporte, objeto que contiene la instancia del Data set físico del Reporte a generar
    ///             3.-Ds_Reporte, contiene la Ruta del Reporte a mostrar en pantalla
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataSet Data_Set, DataSet Ds_Reporte, string Nombre_Reporte)
    {

        ReportDocument Reporte = new ReportDocument();
        string File_Path = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

        Reporte.Load(File_Path);
        DataRow Renglon;

        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            Renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
        }
        Reporte.SetDataSource(Ds_Reporte);

        //1
        ExportOptions Export_Options = new ExportOptions();
        //2
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        //3
        //4
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Consulta_Peticiones.pdf");
        //5
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        //6
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        //7
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        //8
        Reporte.Export(Export_Options);
        //9
        String Ruta = "../../Reporte/Rpt_Consulta_Peticiones.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: LLenar_Combos
    ///DESCRIPCIÓN: Consulta los datos para los combos y llama al método que carga los datos de la consulta en
    ///             los combos
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void LLenar_Combos()
    {
        var Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();
        var Obj_Colonias = new Cls_Cat_Ate_Colonias_Negocio();
        var Obj_Programas = new Cls_Cat_Ate_Programas_Negocio();
        var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();

        try
        {
            // Combo de Programas (origen)
            Obj_Programas.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Origen, Obj_Programas.Consultar_Registros(), 0, 2);
            // Combo de Colonia
            Obj_Colonias.P_Filtros_Dinamicos = Cat_Ate_Colonias.Campo_Estatus + " LIKE '%VIGENTE%'";
            Llenar_Combo_Con_DataTable(Cmb_Colonia, Obj_Colonias.Consulta_Datos().Tables[0]);
            // Combo de Dependencia
            Obj_Dependencias.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Dependencia, Obj_Dependencias.Consulta_Dependencias());
            // Combo de Asunto
            Obj_Asunto.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asunto.Consultar_Registros());
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal)
    {
        // ordenar elementos de la tabla
        Dt_Temporal.DefaultView.Sort = Dt_Temporal.Columns[1].ColumnName + " ASC";

        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataValueField = Dt_Temporal.Columns[0].ToString();
        Obj_Combo.DataTextField = Dt_Temporal.Columns[1].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    /// 		3. Indice_Campo_Valor: entero con el número de columna de la tabla con el valor para el combo
    /// 		4. Indice_Campo_Texto: entero con el número de columna de la tabla con el texto para el combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal, int Indice_Campo_Valor, int Indice_Campo_Texto)
    {
        // ordenar elementos de la tabla
        Dt_Temporal.DefaultView.Sort = Dt_Temporal.Columns[Indice_Campo_Valor].ColumnName + " ASC";

        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataTextField = Dt_Temporal.Columns[Indice_Campo_Texto].ToString();
        Obj_Combo.DataValueField = Dt_Temporal.Columns[Indice_Campo_Valor].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Peticiones
    ///DESCRIPCIÓN: Consulta las peticiones con los parámetros introducidos por el usuario
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected DataTable Consultar_Peticiones()
    {
        Boolean Error = false;
        String Cadena_Informacion = "Sea tan amable de:<br/>";
        var Neg_Consulta_Peticiones = new Cls_Ope_Consulta_Peticiones_Negocio();

        // si se seleccionó el filtro por fecha, validar rango de fecha y agregar como parámetro
        if (Ckb_Fecha.Checked)
        {
            DateTime Fecha_Inicial;
            DateTime Fecha_Final;
            DateTime.TryParse(Txt_Fecha_Inicio.Text, out Fecha_Inicial);
            DateTime.TryParse(Txt_Fecha_Fin.Text, out Fecha_Final);

            // validar que la fecha inicial sea menor a la final y agregar como parámetro si es diferente a la fecha mínima
            if (Fecha_Inicial != DateTime.MinValue && Fecha_Final != DateTime.MinValue && Fecha_Inicial > Fecha_Final)
            {
                Cadena_Informacion += " + revisar el rango de fechas proporcionado<br/>";
                Error = true;
            }
            else
            {
                if (Fecha_Inicial != DateTime.MinValue)
                {
                    Neg_Consulta_Peticiones.P_Fecha_Inicio = Fecha_Inicial.ToString("dd-MM-yyyy");
                }
                if (Fecha_Final != DateTime.MinValue)
                {
                    Neg_Consulta_Peticiones.P_Fecha_Fin = Fecha_Final.ToString("dd-MM-yyyy");
                }
            }
        }
        // si se proporcionó un folio agregar como parámetro
        if (Chk_Folio.Checked)
        {
            if (Txt_Folio.Text.Length > 0)
            {
                Neg_Consulta_Peticiones.P_Folio = Txt_Folio.Text;
            }
            else
            {
                Cadena_Informacion += "+ ingresar el número de folio<br/>";
                Error = true;
            }
        }
        // si se seleccionó una dependencia agregar como parámetro
        if (Chk_Dependencia.Checked)
        {
            if (Cmb_Dependencia.SelectedIndex > 0)
            {
                Neg_Consulta_Peticiones.P_Dependencia = Cmb_Dependencia.SelectedValue;
            }
            else
            {
                Cadena_Informacion += "+ seleccionar una Dependencia del listado<br/>";
                Error = true;
            }
        }
        // si se seleccionó un ASUNTO agregar como parámetro
        if (Chk_Asunto.Checked)
        {
            if (Cmb_Asunto.SelectedIndex > 0)
            {
                Neg_Consulta_Peticiones.P_Asunto_ID = Cmb_Asunto.SelectedValue;
            }
            else
            {
                Cadena_Informacion += "+ seleccionar un Asunto del listado<br/>";
                Error = true;
            }
        }
        // si se seleccionó una COLONIA agregar como parámetro
        if (Chk_Colonia.Checked)
        {
            if (Cmb_Colonia.SelectedIndex > 0)
            {
                Neg_Consulta_Peticiones.P_Colonia_ID = Cmb_Colonia.SelectedValue;
            }
            else
            {
                Cadena_Informacion += "+ seleccionar una Colonia del listado<br/>";
                Error = true;
            }
        }
        // si se seleccionó un ESTATUS agregar como parámetro
        if (Chk_Estatus.Checked)
        {
            if (Cmb_Estatus.SelectedIndex > 0)
            {
                Neg_Consulta_Peticiones.P_Estatus = Cmb_Estatus.SelectedValue;
            }
            else
            {
                Cadena_Informacion += "+ seleccionar un Estatus del listado<br/>";
                Error = true;
            }
        }
        // si se seleccionó un origen agregar como parámetro
        if (Chk_Origen.Checked)
        {
            if (Cmb_Origen.SelectedIndex > 0)
            {
                Neg_Consulta_Peticiones.P_Programa_ID = Convert.ToInt32(Cmb_Origen.SelectedValue);
            }
            else
            {
                Cadena_Informacion += "+ seleccionar un área del listado<br/>";
                Error = true;
            }
        }
        // si se ingresó un nombre agregar como parámetro
        if (Chk_Nombre_Solicitante.Checked)
        {
            if (Txt_Nombre_Solicitante.Text.Length > 0)
            {
                Neg_Consulta_Peticiones.P_Nombre_Solicitante = Txt_Nombre_Solicitante.Text;
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ ingresar el nombre de solicitante<br/>";
                Error = true;
            }
        }

        // si hay mensajes de error, mostrarlos, si no, ejecutar consulta y mostrar resultados
        if (Error)
        {
            Lbl_Informacion.Text = Cadena_Informacion;
            Mostrar_Informacion(1);
        }
        else
        {
            DataSet Data_Set = Neg_Consulta_Peticiones.Consulta_Peticiones_General();
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
            {
                return Data_Set.Tables[0];
            }
        }

        return null;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Exportar_Excel
    ///DESCRIPCIÓN: genera un archivo de Excel con la información en el dataset a través del crystal reports
    ///PARAMETROS:  1. Data_Set: contiene la Mostrar_Informacion de la consulta a la base de datos
    ///             2. Ds_Reporte: objeto que contiene la instancia del Data set físico del Reporte a generar
    ///             3. Nombre_Reporte: contiene la Ruta del Reporte a mostrar en pantalla
    ///             4. Nombre_Xls: nombre con el que se generará en disco el archivo xls
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 14-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Exportar_Excel(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Xls)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../../paginas/Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

        Reporte.Load(File_Path);
        DataRow Renglon;

        // pasar datos del dataset con información al dataset del reporte
        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            Renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
        }
        Reporte.SetDataSource(Ds_Reporte);

        // generar reporte
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_Xls);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);

        Abrir_Reporte(Server.MapPath("../../Reporte/" + Nombre_Xls));
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Abrir_Reporte
    ///DESCRIPCIÓN: Generar el encabezado para ofrecer para descarga el archivo recibido como parámetro
    ///PARÁMETROS:
    /// 		1. Ruta_Absoluta_Archivo: texto con la ruta absoluta del archivo a generar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 02-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Abrir_Reporte(String Ruta_Absoluta_Archivo)
    {
        String Nombre_Archivo = String.Empty;//Nombre del archivo a descargar.
        String Extension_Archivo = String.Empty;//Extension del archivo
        String Tipo_Archivo = String.Empty;//MimeType. Forma de retorno de los datos del servidor al cliente.

        System.IO.FileInfo Archivo = new System.IO.FileInfo(Ruta_Absoluta_Archivo);//Crear instancia para leer la información del archivo.

        Nombre_Archivo = Path.GetFileName(Ruta_Absoluta_Archivo);//Obtenemos el nombre completo del archivo.
        Extension_Archivo = Path.GetExtension(Ruta_Absoluta_Archivo);//Obtenemos la extensión del archivo.

        //Validamos la extensión del archivo.
        if (!String.IsNullOrEmpty(Extension_Archivo))
            Extension_Archivo = Extension_Archivo.Trim().ToLower();

        //Obtenemos el estandar [MimeType] para retorno de datos al cliente.
        switch (Extension_Archivo)
        {
            case ".doc":
                Tipo_Archivo = "Application/msword";
                break;
            case ".docm":
                Tipo_Archivo = "application/vnd.ms-word.document.macroEnabled.12";
                break;
            case ".dotx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.wordprocessingml.template";
                break;
            case ".dotm":
                Tipo_Archivo = "application/vnd.ms-word.template.macroEnabled.12";
                break;
            case ".docx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                break;

            case ".xls":
                Tipo_Archivo = "application/vnd.ms-excel";
                break;
            case ".xlsx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                break;
            case ".xlsm":
                Tipo_Archivo = "application/vnd.ms-excel.sheet.macroEnabled.12";
                break;
            case ".xltx":
                Tipo_Archivo = "application/vnd.openxmlformats-officedocument.spreadsheetml.template";
                break;

            case ".pdf":
                Tipo_Archivo = "Application/pdf";
                break;
            default:
                Tipo_Archivo = "text/plain";
                break;
        }

        //Borra toda la salida de contenido de la secuencia del búfer
        Response.Clear();
        //Obtiene o establece un valor que indica si la salida se va a almacenar en el búfer y se va a enviar después de que se haya terminado de procesar la respuesta completa
        Response.Buffer = true;
        //Obtiene o establece el tipo MIME HTTP de la secuencia de salida
        Response.ContentType = Tipo_Archivo;
        //Esta linea es opcional, en donde podemos cambiar el nombre del fichero a descargar (para que sea diferente al original)
        Response.AddHeader("Content-Disposition", "attachment;filename=" + (Archivo.Name).Replace(Session.SessionID, String.Empty));
        //Establecemos el juego de caracteres HTTP de la secuencia de salida
        Response.Charset = "UTF-8";
        //Establecemos el juego de caracteres HTTP de la secuencia de salida
        //Response.ContentEncoding = Encoding.Default;
        //Escribimos el fichero a enviar
        Response.WriteFile(Archivo.FullName);
        //Envía al cliente toda la salida almacenada en el búfer
        Response.Flush();
        //Envía al cliente toda la salida del búfer actual, detiene la ejecución de la página y provoca el evento se detenga.
        Response.End();
    }

    #endregion Métodos

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Consultar_Click
    ///DESCRIPCIÓN: Verifica la opción de filtro de búsqueda seleccionado y realiza la 
    ///petición a la bd para  obtener la información solicitada
    ///PARAMETROS:  
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Consultar_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Peticiones;

        Mostrar_Informacion("");

        Dt_Peticiones = Consultar_Peticiones();

        if (Dt_Peticiones != null && Dt_Peticiones.Rows.Count > 0)
        {
            Grid_Consulta_Peticiones.DataSource = Dt_Peticiones;
            Grid_Consulta_Peticiones.DataBind();
            Session["Dt_Peticiones"] = Dt_Peticiones;
            ViewState["SortDirection"] = "DESC";
        }
        else
        {
            Grid_Consulta_Peticiones.DataSource = null;
            Grid_Consulta_Peticiones.DataBind();
            Lbl_Informacion.Text = "No se encontraron registros con la información proporcionada.";
            Img_Advertencia.Visible = true;
            Lbl_Informacion.Enabled = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Folio_CheckedChanged
    ///DESCRIPCIÓN: valida que cuando se selecciona el filtro de búsqueda por folio,
    ///los demás se deshabiliten y se refresquen 
    ///PARAMETROS:  
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Folio_CheckedChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion(null);

        if (Chk_Folio.Checked)
        {
            Txt_Folio.Enabled = true;
        }
        else
        {
            Txt_Folio.Enabled = false;
            Txt_Folio.Text = "";

        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Dependencias_CheckedChanged
    ///DESCRIPCIÓN: valida que cuando se selecciona el filtro de búsqueda por dependencia,
    ///los demás se deshabiliten y se refresquen 
    ///PARAMETROS:  
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Dependencia_CheckedChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion(null);

        if (Chk_Dependencia.Checked)
        {
            Cmb_Dependencia.Enabled = true;
            Btn_Buscar_Dependencia.Enabled = true;
        }
        else
        {
            Btn_Buscar_Dependencia.Enabled = false;
            Cmb_Dependencia.Enabled = false;
            Cmb_Dependencia.SelectedIndex = -1;
        }
        // llamar al evento cambio de índice para actualizar elementos del combo asunto
        Cmb_Dependencia_SelectedIndexChanged(null, null);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Asunto_CheckedChanged
    ///DESCRIPCIÓN: maneja el evento cambio de selección el control Chk_Asunto: 
    ///             habilita o deshabilita el combo Asunto
    ///PARAMETROS:  
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Asunto_CheckedChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion(null);

        if (Chk_Asunto.Checked)
        {
            Cmb_Asunto.Enabled = true;
            Btn_Buscar_Asunto.Enabled = true;
        }
        else
        {
            Cmb_Asunto.Enabled = false;
            Cmb_Asunto.SelectedIndex = -1;
            Btn_Buscar_Asunto.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Colonia_CheckedChanged
    ///DESCRIPCIÓN: maneja el evento cambio de selección el control Chk_Colonia: 
    ///             habilita o deshabilita el combo colonia
    ///PARAMETROS:  
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 28-may-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Colonia_CheckedChanged(object sender, EventArgs e)
    {
        Mostrar_Informacion(null);

        if (Chk_Colonia.Checked)
        {
            Cmb_Colonia.Enabled = true;
            Btn_Buscar_Colonia.Enabled = true;
        }
        else
        {
            Cmb_Colonia.Enabled = false;
            Cmb_Colonia.SelectedIndex = -1;
            Btn_Buscar_Colonia.Enabled = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencia_SelectedIndexChanged
    ///DESCRIPCIÓN: Se cargan los elementos del combo asunto filtrando por la dependencia seleccionada
    ///PARAMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 19-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();
        string Valor_Actual_Asunto = "";

        try
        {
            // si hay una dependencia seleccionada, filtrar asuntos por dependencia
            if (Cmb_Dependencia.SelectedIndex > 0)
            {
                Obj_Asunto.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            }
            // guardar el valor actual del combo asunto
            if (Cmb_Asunto.SelectedIndex > 0)
            {
                Valor_Actual_Asunto = Cmb_Asunto.SelectedValue;
            }
            // cargar combo de Asunto
            Obj_Asunto.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asunto.Consultar_Registros());
            // volver a seleccionar valor anterior en el combo asunto si se guardó un valor
            if (Valor_Actual_Asunto != "")
            {
                // validar que el valor guardado existe en el combo
                if (Cmb_Asunto.Items.FindByValue(Valor_Actual_Asunto) != null)
                {
                    Cmb_Asunto.SelectedValue = Valor_Actual_Asunto;
                }
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("No se pudo mostrar información: " + Ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN:Genera un Reporte con la lista de peticiones que regresa el método Consulta_Peticiones
    ///PARAMETROS:  
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Peticiones = null;
        DataSet Ds_Peticiones = new DataSet();

        Mostrar_Informacion("");

        try
        {
            Dt_Peticiones = Consultar_Peticiones();

            if (Dt_Peticiones != null)
            {
                Ds_Peticiones.Tables.Add(Dt_Peticiones.Copy());
                Ds_Ope_Consulta_Peticiones ds_consulta_peticiones = new Ds_Ope_Consulta_Peticiones();
                Generar_Reporte(Ds_Peticiones, ds_consulta_peticiones, "Rpt_Ope_Consulta_Peticiones.rpt");
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Exportar_Excel_Click
    ///DESCRIPCIÓN: Genera un archivo de Excel con la lista de peticiones que regresa el método Consulta_Peticiones
    ///PARAMETROS:  
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 14-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Peticiones = null;
        DataSet Ds_Peticiones = new DataSet();

        Mostrar_Informacion("");

        try
        {
            Dt_Peticiones = Consultar_Peticiones();

            if (Dt_Peticiones != null)
            {
                Ds_Peticiones.Tables.Add(Dt_Peticiones.Copy());
                Ds_Ope_Consulta_Peticiones ds_consulta_peticiones = new Ds_Ope_Consulta_Peticiones();
                Exportar_Excel(Ds_Peticiones, ds_consulta_peticiones, "Rpt_Ope_Consulta_Peticiones.rpt", "Consulta_Peticiones.xls");
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Origen_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita los controles 
    ///si este control se ha seleccionado 
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Origen_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Origen.Checked)
        {
            Cmb_Origen.Enabled = true;
        }
        else
        {
            Cmb_Origen.Enabled = false;
            Cmb_Origen.SelectedIndex = -1;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Estatus_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita los controles si este control se ha seleccionado 
    ///PARAMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 27-may-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Estatus.Checked)
        {
            Cmb_Estatus.Enabled = true;
        }
        else
        {
            Cmb_Estatus.Enabled = false;
            Cmb_Origen.SelectedIndex = -1;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Btn_Salir_Click
    ///DESCRIPCIÓN: redirecciona al usuario a la pagina principal
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 30/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Session.Remove("Dt_Peticiones");
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Nombre_Solicitante_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita los controles 
    ///si este control se ha seleccionado 
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 5/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Nombre_Solicitante_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Nombre_Solicitante.Checked)
        {
            Txt_Nombre_Solicitante.Enabled = true;
        }
        else
        {
            Txt_Nombre_Solicitante.Enabled = false;
            Txt_Nombre_Solicitante.Text = "";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
            {
                try
                {
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Colonia.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonia.SelectedValue = Colonia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message);
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("NOMBRE_COLONIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Asunto_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID del asunto seleccionado en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Asunto_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_ASUNTOS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_ASUNTOS"]) == true)
            {
                try
                {
                    // volver a cargar combo asuntos
                    var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();
                    Obj_Asunto.P_Estatus = "ACTIVO";
                    Llenar_Combo_Con_DataTable(Cmb_Asunto, Obj_Asunto.Consultar_Registros());

                    string Asunto_ID = Session["ASUNTO_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo Asuntos contiene el ID, seleccionar
                    if (Cmb_Asunto.Items.FindByValue(Asunto_ID) != null)
                    {
                        Cmb_Asunto.SelectedValue = Asunto_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message);
                }

                // limpiar variables de sesión
                Session.Remove("ASUNTO_ID");
                Session.Remove("NOMBRE_ASUNTO");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_ASUNTOS");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_DEPENDENCIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_DEPENDENCIAS"]) == true)
            {
                try
                {
                    string Dependencia_ID = Session["DEPENDENCIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Dependencia.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencia.SelectedValue = Dependencia_ID;
                    }
                }
                catch (Exception Ex)
                {
                    Mostrar_Informacion(Ex.Message);
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }

    #endregion Eventos

}