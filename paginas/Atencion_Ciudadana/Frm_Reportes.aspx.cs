using System;
using System.Data;
using System.Web.UI;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Reportes_Atencion_Ciudadana.Negocios;
using Presidencia.Colonias.Negocios;
using Presidencia.Areas.Negocios;
using Presidencia.Dependencias.Negocios;
using System.Web.UI.WebControls;
using Presidencia.Programas_AC.Negocio;
using Presidencia.Asuntos_AC.Negocio;
using Presidencia.Constantes;
using System.Web;
using System.Text;
using System.IO;
using Presidencia.Sessiones;

public partial class paginas_Atencion_Ciudadana_Frm_Reportes : System.Web.UI.Page
{
    #region Page Load / Init
    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager_Reportes.RegisterPostBackControl(Btn_Exportar_Excel);

        if (!Page.IsPostBack)
        {
            Chk_Fecha.Checked = true;

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


    #region METODOS

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
            Llenar_Combo_Con_DataTable(Cmb_Colonias, Obj_Colonias.Consulta_Datos().Tables[0]);
            // Combo de Dependencia
            Obj_Dependencias.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Dependencias, Obj_Dependencias.Consulta_Dependencias());
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
        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataValueField = Dt_Temporal.Columns[0].ToString();
        Obj_Combo.DataTextField = Dt_Temporal.Columns[1].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<Seleccione>"), "0"));
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
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<Seleccione>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_PDF_Detallado
    ///DESCRIPCIÓN: Genera un pdf a través de crystal reports con la información recibida 
    ///         como parámetros en el dataset, forma el domicilio para insertarlo en un sólo 
    ///         registro del dataset para el reporte
    ///PARAMETROS:  1. Data_Set: contiene la información de la consulta
    ///             2. Ds_Reporte: objeto en el que se copian los datos para pasarlo al reporte
    ///             3. Nombre_Reporte: nombre del archivo rpt a utilizar para el reporte
    ///             4. Nombre_Pdf: nombre con el que se generará en disco el archivo pdf
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 20-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte_PDF_Detallado(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Pdf)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);
        string Calle = "";
        string Colonia = "";
        string Numero_Exterior = "";
        string Numero_Interior = "";
        string Referencia = "";
        Reporte.Load(File_Path);
        DataRow Renglon;
        string Filtros = "";

        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            string Domicilio = "";
            // obtener el domicilio
            Calle = Data_Set.Tables[0].Rows[i]["CALLE"].ToString();
            Colonia = Data_Set.Tables[0].Rows[i]["COLONIA"].ToString();
            Referencia = Data_Set.Tables[0].Rows[i][Ope_Ate_Peticiones.Campo_Referencia].ToString();
            Numero_Exterior = Data_Set.Tables[0].Rows[i][Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString();
            Numero_Interior = Data_Set.Tables[0].Rows[i][Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
            // si la calle contiene texto, agregar calle y numeros exterior e interior
            if (Calle.Length > 0)
            {
                Domicilio += Calle + " " + Numero_Exterior + " " + Numero_Interior;
            }
            else
            {
                if (Numero_Exterior.Trim().Length > 0)
                {
                    Domicilio += "Número exterior " + Numero_Exterior;
                }
                if (Numero_Interior.Trim().Length > 0)
                {
                    Domicilio += " int. " + Numero_Interior;
                }
            }
            // si hay una colonia, agregar al domicilio
            if (Colonia.Length > 0)
            {
                Domicilio += " col. " + Colonia;
            }
            // si el domicilio no contiene texto y la referencia sí, agregar referencia
            if (Domicilio.Length <= 0 && Referencia.Length > 0)
            {
                Domicilio = Referencia;
            }
            // agregar referencia al domicilio si no contiene texto la calle o el número exterior
            else if (Referencia.Trim().Length > 0 && (Calle.Length <= 0 || Numero_Exterior.Length <= 0))
            {
                Domicilio += ", " + Referencia;
            }
            // agregar renglon al dataset del reporte
            Renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
            // actualizar el domicilio en la tabla del dataset
            Ds_Reporte.Tables[0].Rows[i]["Domicilio"] = Domicilio;
        }
        // texto filtros
        if (Txt_Fecha_Inicio.Text.Length > 0)
        {
            Filtros += " desde " + Txt_Fecha_Inicio.Text;
        }
        if (Txt_Fecha_Fin.Text.Length > 0)
        {
            Filtros += " hasta " + Txt_Fecha_Fin.Text;
        }
        // agregar al dataset del reporte
        Renglon = Ds_Reporte.Tables[1].NewRow();
        Renglon[0] = Filtros;
        Renglon[1] = Obtener_Texto_Filtros_Aplicados();
        Ds_Reporte.Tables[1].Rows.Add(Renglon);

        // generar reporte de crystal
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_Pdf);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        // mostrar reporte
        String Ruta = "../../Reporte/" + Nombre_Pdf;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_PDF
    ///DESCRIPCIÓN: carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.- Data_Set.- contiene la Mostrar_Informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, objeto que contiene la instancia del Data set físico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene la Ruta del Reporte a mostrar en pantalla
    ///             4.- Nombre_Pdf, nombre con el que se generará en disco el archivo pdf
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 7/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte_PDF(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Pdf)
    {
        string Rango_Fechas = "";
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

        Reporte.Load(File_Path);
        DataRow Renglon;

        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            Renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
        }
        // formar texto para título a partir del rango de fechas
        if (Txt_Fecha_Inicio.Text.Length > 0)
        {
            Rango_Fechas += " desde " + Txt_Fecha_Inicio.Text;
        }
        if (Txt_Fecha_Fin.Text.Length > 0)
        {
            Rango_Fechas += " hasta " + Txt_Fecha_Fin.Text;
        }
        // agregar al dataset del reporte
        Renglon = Ds_Reporte.Tables[1].NewRow();
        Renglon[0] = Rango_Fechas;
        Renglon[1] = Obtener_Texto_Filtros_Aplicados();
        Ds_Reporte.Tables[1].Rows.Add(Renglon);

        // generar reporte de crystal
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_Pdf);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        // mostrar archivo generado
        String Ruta = "../../Reporte/" + Nombre_Pdf;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Exportar_Excel_Detallado
    ///DESCRIPCIÓN: Genera un archivos de Excel a través de crystal reports con la información 
    ///             recibida como parámetros en el dataset, forma el domicilio para insertarlo 
    ///             en un sólo registro del dataset para el reporte
    ///PARAMETROS:  1. Data_Set: contiene la información de la consulta
    ///             2. Ds_Reporte: objeto en el que se copian los datos para pasarlo al reporte
    ///             3. Nombre_Reporte: nombre del archivo rpt a utilizar para el reporte
    ///             4. Nombre_Xls: nombre con el que se generará en disco el archivo de Excel
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 20-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Exportar_Excel_Detallado(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Xls)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Atencion_Ciudadana/" + Nombre_Reporte);
        string Calle = "";
        string Colonia = "";
        string Numero_Exterior = "";
        string Numero_Interior = "";
        string Referencia = "";
        Reporte.Load(File_Path);
        DataRow Renglon;
        string Rango_Fechas = "";

        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            string Domicilio = "";
            // obtener el domicilio
            Calle = Data_Set.Tables[0].Rows[i]["CALLE"].ToString();
            Colonia = Data_Set.Tables[0].Rows[i]["COLONIA"].ToString();
            Referencia = Data_Set.Tables[0].Rows[i][Ope_Ate_Peticiones.Campo_Referencia].ToString();
            Numero_Exterior = Data_Set.Tables[0].Rows[i][Ope_Ate_Peticiones.Campo_Numero_Exterior].ToString();
            Numero_Interior = Data_Set.Tables[0].Rows[i][Ope_Ate_Peticiones.Campo_Numero_Interior].ToString();
            // si la calle contiene texto, agregar calle y números exterior e interior
            if (Calle.Length > 0)
            {
                Domicilio += Calle + " " + Numero_Exterior + " " + Numero_Interior;
            }
            else
            {
                if (Numero_Exterior.Trim().Length > 0)
                {
                    Domicilio += "Número exterior " + Numero_Exterior;
                }
                if (Numero_Interior.Trim().Length > 0)
                {
                    Domicilio += " int. " + Numero_Interior;
                }
            }
            // si hay una colonia, agregar al domicilio
            if (Colonia.Length > 0)
            {
                Domicilio += " col. " + Colonia;
            }
            // si el domicilio no contiene texto y la referencia sí, agregar referencia
            if (Domicilio.Length <= 0 && Referencia.Length > 0)
            {
                Domicilio = Referencia;
            }
            // agregar referencia al domicilio si no contiene texto la calle o el número exterior
            else if (Referencia.Trim().Length > 0 && (Calle.Length <= 0 || Numero_Exterior.Length <= 0))
            {
                Domicilio += ", " + Referencia;
            }
            // agregar renglón al dataset del reporte
            Renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
            // actualizar el domicilio en la tabla del dataset
            Ds_Reporte.Tables[0].Rows[i]["Domicilio"] = Domicilio;
        }

        // formar texto para título a partir del rango de fechas
        if (Txt_Fecha_Inicio.Text.Length > 0)
        {
            Rango_Fechas += " desde " + Txt_Fecha_Inicio.Text;
        }
        if (Txt_Fecha_Fin.Text.Length > 0)
        {
            Rango_Fechas += " hasta " + Txt_Fecha_Fin.Text;
        }
        // agregar al dataset del reporte
        Renglon = Ds_Reporte.Tables[1].NewRow();
        Renglon[0] = Rango_Fechas;
        Renglon[1] = Obtener_Texto_Filtros_Aplicados();
        Ds_Reporte.Tables[1].Rows.Add(Renglon);

        // generar reporte de crystal
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_Xls);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        // mostrar archivo generado
        Abrir_Reporte(Server.MapPath("../../Reporte/" + Nombre_Xls));
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Exportar_Excel
    ///DESCRIPCIÓN: carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.- Data_Set.- contiene la Mostrar_Informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, objeto que contiene la instancia del Data set físico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene la Ruta del Reporte a mostrar en pantalla
    ///             4.- Nombre_Xls, nombre con el que se generará en disco el archivo xls
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 7/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Exportar_Excel(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Xls)
    {
        string Rango_Fechas = "";
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../../paginas/Rpt/Atencion_Ciudadana/" + Nombre_Reporte);

        Reporte.Load(File_Path);
        DataRow Renglon;

        // importar las filas al dataset del reporte
        for (int i = 0; i < Data_Set.Tables[0].Rows.Count; i++)
        {
            Renglon = Data_Set.Tables[0].Rows[i];
            Ds_Reporte.Tables[0].ImportRow(Renglon);
        }

        // formar texto para título a partir del rango de fechas
        if (Txt_Fecha_Inicio.Text.Length > 0)
        {
            Rango_Fechas += " desde " + Txt_Fecha_Inicio.Text;
        }
        if (Txt_Fecha_Fin.Text.Length > 0)
        {
            Rango_Fechas += " hasta " + Txt_Fecha_Fin.Text;
        }
        // agregar al dataset del reporte
        Renglon = Ds_Reporte.Tables[1].NewRow();
        Renglon[0] = Rango_Fechas;
        Renglon[1] = Obtener_Texto_Filtros_Aplicados();
        Ds_Reporte.Tables[1].Rows.Add(Renglon);

        // generar reporte de crystal
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_Xls);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        // mostrar archivo generado
        Abrir_Reporte(Server.MapPath("../../Reporte/" + Nombre_Xls));
    }

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
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Método que cambia de posición los días y meses para acoplarlo al 
    ///formato fecha de oracle
    ///PARAMETROS:  1.- Fecha_Inicial, es la fecha a la cual se le cambiara el formato 
    ///CREO: Silvia Morales
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String fecha_inicial)
    {
        char[] ch = { ' ' };
        String[] str = fecha_inicial.Split(ch);
        String fecha = str[0];
        String[] fecha_nueva = fecha.Split('/');
        String Fecha_Valida = "";
        Fecha_Valida = fecha_nueva[1] + "/" + fecha_nueva[0] + "/" + fecha_nueva[2];
        return Fecha_Valida;
    }// fin de Formato_Fecha

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Crear_Ds_Consulta_Peticiones
    ///DESCRIPCIÓN: Lee los parámetros seleccionados para ejecutar una búsqueda y genera un dataset con los resultados
    ///         Se valida la fecha y selección de controles, si se encuentra un error se regresa como mensaje
    ///PARÁMETROS:
    ///         1. Ds_Peticiones: dataset en el que se van a regresar los datos de la consulta
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 11-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected string Crear_Ds_Consulta_Peticiones(out DataSet Ds_Peticiones)
    {
        var Reporte_Negocio = new Cls_Rpt_Ate_Reportes_Negocio();
        Reporte_Negocio.P_Filtros_Dinamicos = "";
        Ds_Peticiones = null;

        Boolean Error = false;
        String Cadena_Informacion = "Para realizar la consulta debe seleccionar:<br />";
        DateTime Fecha_Inicio;
        DateTime Fecha_Fin;

        // si el filtro por fecha está seleccionado, validar fechas
        if (Chk_Fecha.Checked == true)
        {
            DateTime.TryParse(Txt_Fecha_Inicio.Text, out Fecha_Inicio);
            DateTime.TryParse(Txt_Fecha_Fin.Text, out Fecha_Fin);
            // si las fechas no son válidas, salir del evento mostrando mensaje de error
            if (Fecha_Inicio == DateTime.MinValue || Fecha_Fin == DateTime.MinValue)
            {
                // regresar mensaje
                return Cadena_Informacion + "un rango de fechas válido.";
            }
            else if (Fecha_Inicio > Fecha_Fin)
            {
                // regresar mensaje
                return "El rango de fechas proporcionado es invalido, sea tan amable de verificar.";
            }
            else    // establecer propiedades de fecha para filtrar consulta
            {
                Reporte_Negocio.P_Fecha_Inicio = Fecha_Inicio.ToString("dd/MM/yyyy");
                Reporte_Negocio.P_Fecha_Fin = Fecha_Fin.ToString("dd/MM/yyyy");
            }
        }

        // validar que haya una Dependencia seleccionada si Chk_Dependencias está activado
        if (Chk_Dependencias.Checked)
        {
            if (Cmb_Dependencias.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Dependencia = Cmb_Dependencias.SelectedValue;
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ Una dependencia del listado.<br />";
                Error = true;
            }
        }
        // validar que haya un Estatus seleccionado si Chk_Estatus está activado
        if (Chk_Estatus.Checked == true)
        {
            if (Cmb_Estatus.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                // para el reporte detallado, asignar estatus en p_filtros_dinamicos
                if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_DETALLADO")
                {
                    // si el estatus es EN PROCESO, agregar filtro para que sólo considere los que no tienen tipo_solucion
                    if (Cmb_Estatus.SelectedValue == "EN PROCESO")
                    {
                        Reporte_Negocio.P_Filtros_Dinamicos += "PETICION." + Ope_Ate_Peticiones.Campo_Tipo_Solucion + " IS NULL AND ";
                    }
                    // si el tipo de solucion es TERMINADA, incluir todas las que tengan tipo de solución
                    else if (Cmb_Estatus.SelectedValue == "TERMINADA")
                    {
                        Reporte_Negocio.P_Estatus = "";
                        Reporte_Negocio.P_Filtros_Dinamicos += "PETICION." + Ope_Ate_Peticiones.Campo_Tipo_Solucion + " IN ('POSITIVA','NEGATIVA') AND ";
                    }
                }
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ Un estatus del listado.<br />";
                Error = true;
            }
        }
        // validar que haya una Colonia seleccionada si Chk_Colonias está activado
        if (Chk_Colonias.Checked)
        {
            if (Cmb_Colonias.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Colonia = Cmb_Colonias.SelectedValue;
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ Una colonia del listado.<br />";
                Error = true;
            }

        }
        // validar que haya un Tipo de solución seleccionado si Chk_Tipo_Solucion está activado
        if (Chk_Tipo_Solucion.Checked)
        {
            if (Cmb_Tipo_Solucion.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Tipo_Solucion = Cmb_Tipo_Solucion.SelectedValue;
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ Un tipo de solución del listado.<br />";
                Error = true;
            }
        }
        // validar que haya un Asunto seleccionado si Chk_Asunto está activado
        if (Chk_Asunto.Checked)
        {
            if (Cmb_Asunto.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Asunto_ID = Cmb_Asunto.SelectedValue;
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ Un Asunto del listado.<br />";
                Error = true;
            }
        }
        // validar que haya un Origen seleccionado si Chk_Origen está activado
        if (Chk_Origen.Checked)
        {
            if (Cmb_Origen.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Programa_ID = Cmb_Origen.SelectedValue;
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ Un Origen del listado.<br />";
                Error = true;
            }
        }
        // validar que haya un Sexo seleccionado si Chk_Sexo está activado
        if (Chk_Sexo.Checked)
        {
            if (Cmb_Sexo.SelectedIndex > 0)
            {
                Reporte_Negocio.P_Sexo = Cmb_Sexo.SelectedValue;
            }
            else
            {
                Cadena_Informacion = Cadena_Informacion + "+ Un Sexo del listado.<br />";
                Error = true;
            }
        }
        // agregar condición Folios_Vencidos a parámetros de consulta
        if (Chk_Folios_Vencidos.Checked)
        {
            Reporte_Negocio.P_Folio_Vencido = "FOLIOS_VENCIDOS";
        }
        // agregar condición Folios_Vencidos a parámetros de consulta
        if (Chk_Con_Telefono.Checked)
        {
            // para el reporte detallado o acumulado, lleva el alias PETICION
            if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_DETALLADO" || Cmb_Tipo_Reporte.SelectedValue == "REPORTE_ACUMULADO")
            {
                Reporte_Negocio.P_Filtros_Dinamicos += "PETICION." + Ope_Ate_Peticiones.Campo_Telefono + " IS NOT NULL ";
            }
            else
            {
                Reporte_Negocio.P_Filtros_Dinamicos += Ope_Ate_Peticiones.Campo_Telefono + " IS NOT NULL ";
            }
        }
        // si se encontraron errores al validar campos, regresar mensajes, si no, ejecutar consulta
        if (Error)
        {
            return Cadena_Informacion;
        }
        else
        {
            if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_DETALLADO")
            {
                Ds_Peticiones = Reporte_Negocio.Consulta_Reporte_Detallado_Peticiones();
            }
            else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_ACUMULADO")
            {
                Ds_Peticiones = Reporte_Negocio.Consulta_Reporte_Acumulado_Peticiones();
            }
            else if (Cmb_Tipo_Reporte.SelectedValue == "ESTADISTICAS_DEPENDENCIA")
            {
                Reporte_Negocio.P_Campos_Dinamicos = ", COUNT(" + Ope_Ate_Peticiones.Campo_Dependencia_ID
                    + ") OVER (Partition By " + Ope_Ate_Peticiones.Campo_Dependencia_ID + ") TOTAL_GRUPO";
                Reporte_Negocio.P_Ordenamiento_Dinamico = " TOTAL_GRUPO DESC, DEPENDENCIA ASC";
                Ds_Peticiones = Reporte_Negocio.Consulta_Estadistica_Peticiones();
            }
            else if (Cmb_Tipo_Reporte.SelectedValue == "ESTADISTICAS_ASUNTO")
            {
                Reporte_Negocio.P_Campos_Dinamicos = ", COUNT(" + Ope_Ate_Peticiones.Campo_Asunto_ID
                    + ") OVER (Partition By " + Ope_Ate_Peticiones.Campo_Asunto_ID + ") TOTAL_GRUPO";
                Reporte_Negocio.P_Ordenamiento_Dinamico = " TOTAL_GRUPO DESC, ASUNTO ASC";
                Ds_Peticiones = Reporte_Negocio.Consulta_Estadistica_Peticiones();
            }
            else if (Cmb_Tipo_Reporte.SelectedValue == "TIEMPOS_RESPUESTA")
            {
                Ds_Peticiones = Reporte_Negocio.Consulta_Tiempo_Promedio_Respuesta_Peticion();
            }
        }

        return "";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Obtener_Texto_Filtros_Aplicados
    ///DESCRIPCIÓN: Regresa un texto con los filtros seleccionados
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 23-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected string Obtener_Texto_Filtros_Aplicados()
    {
        string[] Filtros = new string[10];
        int Contador_Filtros = 0;
        string Texto_Filtros = "";

        // validar que haya un Origen seleccionado si Chk_Origen está activado
        if (Chk_Origen.Checked && Cmb_Origen.SelectedIndex > 0)
        {
            Filtros[Contador_Filtros++] = " " + Cmb_Origen.SelectedItem.Text;
        }
        // validar que haya una Dependencia seleccionada si Chk_Dependencias está activado
        if (Chk_Dependencias.Checked && Cmb_Dependencias.SelectedIndex > 0)
        {
            Filtros[Contador_Filtros++] = "de la Unidad responsable: " + Cmb_Dependencias.SelectedItem.Text;
        }
        // validar que haya un Estatus seleccionado si Chk_Estatus está activado
        if (Chk_Estatus.Checked == true && Cmb_Estatus.SelectedIndex > 0)
        {
            Filtros[Contador_Filtros++] = "con estatus: " + Cmb_Estatus.SelectedItem.Text;
        }
        // validar que haya una Colonia seleccionada si Chk_Colonias está activado
        if (Chk_Colonias.Checked && Cmb_Colonias.SelectedIndex > 0)
        {
            Filtros[Contador_Filtros++] = "de la colonia: " + Cmb_Colonias.SelectedItem.Text;
        }
        // validar que haya un Tipo de solución seleccionado si Chk_Tipo_Solucion está activado
        if (Chk_Tipo_Solucion.Checked && Cmb_Tipo_Solucion.SelectedIndex > 0)
        {
            Filtros[Contador_Filtros++] = "con tipo de solución: " + Cmb_Tipo_Solucion.SelectedItem.Text;
        }
        // validar que haya un Asunto seleccionado si Chk_Asunto está activado
        if (Chk_Asunto.Checked && Cmb_Asunto.SelectedIndex > 0)
        {
            Filtros[Contador_Filtros++] = "con asunto: " + Cmb_Asunto.SelectedItem.Text;
        }
        if (Chk_Sexo.Checked && Cmb_Sexo.SelectedIndex > 0)
        {
            Filtros[Contador_Filtros++] = "con sexo: " + Cmb_Sexo.SelectedItem.Text;
        }
        // agregar condición Folios_Vencidos a mensaje de filtros
        if (Chk_Folios_Vencidos.Checked)
        {
            Filtros[Contador_Filtros++] = "con folios vencidos";
        }
        // agregar condición Folios_Vencidos a mensaje de filtros
        if (Chk_Con_Telefono.Checked)
        {
            Filtros[Contador_Filtros++] = "con número de teléfono";
        }

        // si solo hay un filtro seleccionado
        if (Contador_Filtros == 1)
        {
            Texto_Filtros = "Sólo " + Filtros[0];
        }
        else if (Contador_Filtros == 2)
        {
            Texto_Filtros = "Sólo " + Filtros[0] + " y " + Filtros[1];
        }
        else if (Contador_Filtros >= 3)
        {
            Texto_Filtros = "Sólo " + string.Join(", ", Filtros, 0, Contador_Filtros - 2) + ", " + string.Join(", ", Filtros, Contador_Filtros - 2, 2);
        }

        return Texto_Filtros;
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
        Response.ContentEncoding = Encoding.Default;
        //Escribimos el fichero a enviar
        Response.WriteFile(Archivo.FullName);
        //Envía al cliente toda la salida almacenada en el búfer
        Response.Flush();
        //Envía al cliente toda la salida del búfer actual, detiene la ejecución de la página y provoca el evento se detenga.
        Response.End();
    }

    #endregion METODOS

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Reporte_OnSelectedIndexChanged
    ///DESCRIPCIÓN: habilita o deshabilita los controles en la página dependiendo de los 
    ///         filtros que aplican para el tipo de reporte seleccionado
    ///PARAMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 22-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Tipo_Reporte_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        // siempre desactivar el acumulado por colonia, solamente para el reporte detallado
        Chk_Totales_Colonia.Enabled = false;
        Chk_Totales_Colonia.Checked = false;

        switch (Cmb_Tipo_Reporte.SelectedValue)
        {
            case "REPORTE_DETALLADO":
                Chk_Tipo_Solucion.Enabled = true;
                Chk_Folios_Vencidos.Enabled = true;
                Chk_Estatus.Enabled = true;
                Chk_Asunto.Enabled = true;
                Chk_Totales_Colonia.Enabled = true;
                break;
            case "REPORTE_ACUMULADO":
                Chk_Tipo_Solucion.Enabled = false;
                Chk_Tipo_Solucion.Checked = false;
                Cmb_Tipo_Solucion.Enabled = false;
                Cmb_Tipo_Solucion.SelectedIndex = 0;
                Chk_Folios_Vencidos.Enabled = false;
                Chk_Folios_Vencidos.Checked = false;
                // deshabilitar y limpiar estatus
                Chk_Estatus.Enabled = false;
                Chk_Estatus.Checked = false;
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;
                Chk_Asunto.Enabled = true;
                break;
            case "ESTADISTICAS_DEPENDENCIA":
                Chk_Tipo_Solucion.Enabled = false;
                Chk_Tipo_Solucion.Checked = false;
                Cmb_Tipo_Solucion.Enabled = false;
                Cmb_Tipo_Solucion.SelectedIndex = 0;
                Chk_Folios_Vencidos.Enabled = true;
                // deshabilitar y limpiar filtros que no se requieren
                Chk_Estatus.Enabled = false;
                Chk_Estatus.Checked = false;
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;
                Chk_Asunto.Enabled = true;
                break;
            case "ESTADISTICAS_ASUNTO":
                Chk_Tipo_Solucion.Enabled = true;
                Chk_Folios_Vencidos.Enabled = true;
                // deshabilitar y limpiar filtros que no se requieren
                Chk_Asunto.Enabled = false;
                Chk_Asunto.Checked = false;
                Cmb_Asunto.Enabled = false;
                Cmb_Asunto.SelectedIndex = 0;
                Btn_Buscar_Asunto.Enabled = false;
                break;
            case "TIEMPOS_RESPUESTA":
                Chk_Tipo_Solucion.Enabled = false;
                Chk_Folios_Vencidos.Enabled = false;
                // deshabilitar y limpiar filtros que no se requieren
                Chk_Asunto.Enabled = true;
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Dependencias_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita los controles si este control se ha seleccionado
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 3/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Dependencias_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Dependencias.Checked)
        {
            Cmb_Dependencias.Enabled = true;
            Btn_Buscar_Dependencia.Enabled = true;
        }
        else
        {
            Cmb_Dependencias.Enabled = false;
            Cmb_Dependencias.SelectedIndex = 0;
            Btn_Buscar_Dependencia.Enabled = false;
        }
        // llamar al evento cambio de índice para actualizar elementos del combo asunto
        Cmb_Dependencias_SelectedIndexChanged(null, null);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Chk_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Habilitar el combo estatus o des habilitarlo según el estado del checkbox estatus
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 11-jun-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Chk_Estatus_CheckedChanged(object sender, EventArgs e)
    {
        // si el checkbox está seleccionado, activar el combo con estatus
        if (Chk_Estatus.Checked)
        {
            Cmb_Estatus.Enabled = true;
        }
        else
        {
            Cmb_Estatus.Enabled = false;
            Cmb_Estatus.SelectedIndex = 0;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Asunto_CheckedChanged
    ///DESCRIPCIÓN: maneja el evento cambio de selección el control Chk_Asunto: 
    ///             habilita o deshabilita el combo Asunto
    ///PARAMETROS:  
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 11-jun-2012
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
    ///NOMBRE DE LA FUNCIÓN: Chk_Colonias_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita los controles 
    ///si este control se ha seleccionado 
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 3/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Colonias_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Colonias.Checked)
        {
            Cmb_Colonias.Enabled = true;
            Btn_Buscar_Colonia.Enabled = true;
        }
        else
        {
            Cmb_Colonias.Enabled = false;
            Cmb_Colonias.SelectedIndex = 0;
            Btn_Buscar_Colonia.Enabled = false;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Tipo_Solucion_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita los controles 
    ///si este control se ha seleccionado 
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 3/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Tipo_Solucion_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Tipo_Solucion.Checked)
        {
            Cmb_Tipo_Solucion.Enabled = true;
        }
        else
        {
            Cmb_Tipo_Solucion.Enabled = false;
            Cmb_Tipo_Solucion.SelectedIndex = 0;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Sexo_CheckedChanged
    ///DESCRIPCIÓN: deshabilita o habilita el cmb_sexo
    ///si este control se ha seleccionado 
    ///PARAMETROS:
    ///CREO: Roberto González OSeguera
    ///FECHA_CREO: 21-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Sexo_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Sexo.Checked)
        {
            Cmb_Sexo.Enabled = true;
        }
        else
        {
            Cmb_Sexo.Enabled = false;
            Cmb_Sexo.SelectedIndex = 0;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Click
    ///DESCRIPCIÓN: llama al método que consulta las peticiones y lanza el Reporte en pantalla en formato pdf
    ///PARAMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 11-jun-2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
    {
        string Mensaje = "";
        DataSet Data_Set = null;

        // limpiar mensajes de error
        Mostrar_Informacion(null);

        try
        {
            Mensaje = Crear_Ds_Consulta_Peticiones(out Data_Set);
            // si no se recibieron mensajes de error, mostrar reporte
            if (Mensaje.Length <= 0)
            {
                // validar que el dataset contiene datos
                if (Data_Set != null && Data_Set.Tables.Count > 0 && Data_Set.Tables[0].Rows.Count > 0)
                {
                    // generar reporte detallado o acumulado dependiendo de opción seleccionada
                    if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_DETALLADO")
                    {
                        Ds_Reporte_Informacion_Detallada ds_informacion_detallada = new Ds_Reporte_Informacion_Detallada();
                        if (Chk_Dependencias.Checked == false && Chk_Totales_Colonia.Checked == false)
                        {
                            Generar_Reporte_PDF_Detallado(Data_Set, ds_informacion_detallada, "Rpt_Rep_Ate_Informacion_Detalllada_Sin_Agrupar.rpt", "Rpt_Informacion_Detalllada_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
                        }
                        else if(Chk_Totales_Colonia.Checked == true)
                        {
                            Generar_Reporte_PDF_Detallado(Data_Set, ds_informacion_detallada, "Rpt_Informacion_Detallada_Agrupada_Colonia.rpt", "Rpt_Informacion_Detalllada_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
                        }
                        else
                        {
                            Generar_Reporte_PDF_Detallado(Data_Set, ds_informacion_detallada, "Rpt_Informacion_Detalllada.rpt", "Rpt_Informacion_Detalllada_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
                        }
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_ACUMULADO")
                    {
                        Ds_Reporte_Acumulado ds_reporte_acumulado = new Ds_Reporte_Acumulado();
                        Generar_Reporte_PDF(Data_Set, ds_reporte_acumulado, "Rpt_Informacion_Acumulados.rpt", "Rpt_Acumulados_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "ESTADISTICAS_DEPENDENCIA")
                    {
                        Ds_Ate_Estadisticas_Peticiones Ds_Reporte = new Ds_Ate_Estadisticas_Peticiones();
                        Generar_Reporte_PDF(Data_Set, Ds_Reporte, "Rpt_Ope_Ate_Estadisticas_Peticiones.rpt", "Rpt_Estadistica_Dependencia_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "ESTADISTICAS_ASUNTO")
                    {
                        Ds_Ate_Estadisticas_Peticiones Ds_Reporte = new Ds_Ate_Estadisticas_Peticiones();
                        Generar_Reporte_PDF(Data_Set, Ds_Reporte, "Rpt_Ope_Ate_Estadisticas_Peticiones_Asunto.rpt", "Rpt_Estadistica_Asunto_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "TIEMPOS_RESPUESTA")
                    {
                        Ds_Graficas_Tiempos Ds_Reporte = new Ds_Graficas_Tiempos();
                        Generar_Reporte_PDF(Data_Set, Ds_Reporte, "Rpt_Grafica_Tiempos_Dependencias.rpt", "Rpt_Tiempos_Respuesta_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
                    }
                }
                else
                {
                    Lbl_Informacion.Text = "No se encontraron registros con la información proporcionada.";
                    Mostrar_Informacion(1);
                }
            }
            else // mostrar errores de validación de filtros seleccionados
            {
                Mostrar_Informacion(Mensaje);
            }
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al generar reporte: " + Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Exportar_Excel_Click
    ///DESCRIPCIÓN: llama al método que consulta las peticiones y lanza el Reporte en pantalla en formato excel
    ///PARAMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 11-jun-2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, EventArgs e)
    {
        string Mensaje = "";
        DataSet Data_Set = null;

        // limpiar mensajes de error
        Mostrar_Informacion(null);

        try
        {
            Mensaje = Crear_Ds_Consulta_Peticiones(out Data_Set);
            // si no se recibieron mensajes de error, mostrar reporte
            if (Mensaje.Length <= 0)
            {
                if (Data_Set != null && Data_Set.Tables[0].Rows.Count > 0)
                {
                    // generar reporte detallado o acumulado dependiendo de opción seleccionada
                    if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_DETALLADO")
                    {
                        Ds_Reporte_Informacion_Detallada ds_informacion_detallada = new Ds_Reporte_Informacion_Detallada();
                        if (Chk_Totales_Colonia.Checked == false)
                        {
                            Exportar_Excel_Detallado(Data_Set, ds_informacion_detallada, "Rpt_Informacion_Detalllada.rpt", "Rpt_Informacion_Detalllada_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
                        }
                        else // acumulado por colonia
                        {
                            Exportar_Excel_Detallado(Data_Set, ds_informacion_detallada, "Rpt_Informacion_Detallada_Agrupada_Colonia.rpt", "Rpt_Informacion_Detalllada_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
                        }
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE_ACUMULADO")
                    {
                        Ds_Reporte_Acumulado ds_reporte_acumulado = new Ds_Reporte_Acumulado();
                        Exportar_Excel(Data_Set, ds_reporte_acumulado, "Rpt_Informacion_Acumulados.rpt", "Rpt_Acumulados_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "ESTADISTICAS_DEPENDENCIA")
                    {
                        Ds_Ate_Estadisticas_Peticiones Ds_Reporte = new Ds_Ate_Estadisticas_Peticiones();
                        Exportar_Excel(Data_Set, Ds_Reporte, "Rpt_Ope_Ate_Estadisticas_Peticiones.rpt", "Rpt_Acumulados_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "ESTADISTICAS_ASUNTO")
                    {
                        Ds_Ate_Estadisticas_Peticiones Ds_Reporte = new Ds_Ate_Estadisticas_Peticiones();
                        Exportar_Excel(Data_Set, Ds_Reporte, "Rpt_Ope_Ate_Estadisticas_Peticiones_Asunto.rpt", "Rpt_estadistica_Asunto_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
                    }
                    else if (Cmb_Tipo_Reporte.SelectedValue == "TIEMPOS_RESPUESTA")
                    {
                        Ds_Graficas_Tiempos Ds_Reporte = new Ds_Graficas_Tiempos();
                        Exportar_Excel(Data_Set, Ds_Reporte, "Rpt_Grafica_Tiempos_Dependencias.rpt", "Rpt_Tiempos_Respuesta_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
                    }
                }
                else
                {
                    Lbl_Informacion.Text = "No se encontraron registros con la información proporcionada.";
                    Mostrar_Informacion(1);
                }
            }
            else // mostrar errores de validación de filtros seleccionados
            {
                Mostrar_Informacion(Mensaje);
            }
        }
        catch
        {
            Mostrar_Informacion("Error al generar reporte");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Se cargan los elementos del combo asunto filtrando por la dependencia seleccionada
    ///PARAMETROS:
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 19-jun-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        var Obj_Asunto = new Cls_Cat_Ate_Asuntos_Negocio();
        string Valor_Actual_Asunto = "";

        try
        {
            // si hay una dependencia seleccionada, filtrar asuntos por dependencia
            if (Cmb_Dependencias.SelectedIndex > 0)
            {
                Obj_Asunto.P_Dependencia_ID = Cmb_Dependencias.SelectedValue;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación actual que se este realizando o sale del formulario
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 3/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
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
                    if (Cmb_Colonias.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Colonias.SelectedValue = Colonia_ID;
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
                    if (Cmb_Dependencias.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencias.SelectedValue = Dependencia_ID;
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
