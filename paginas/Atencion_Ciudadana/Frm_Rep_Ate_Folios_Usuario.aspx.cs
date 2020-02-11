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
using Presidencia.Registro_Peticion.Datos;

public partial class paginas_Atencion_Ciudadana_Frm_Rep_Ate_Folios_Usuario : System.Web.UI.Page
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
        var Obj_Programas = new Cls_Cat_Ate_Programas_Negocio();

        try
        {
            // Combo de Programas (origen)
            Obj_Programas.P_Estatus = "ACTIVO";
            Llenar_Combo_Con_DataTable(Cmb_Origen, Obj_Programas.Consultar_Registros(), 0, 2);
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
        string Ruta_Directorio_Reportes = "../../Reporte/";

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

        // si el directorio no existe, crearlo
        if (!Directory.Exists(Server.MapPath(Ruta_Directorio_Reportes)))
        {
            Directory.CreateDirectory(Server.MapPath(Ruta_Directorio_Reportes));
        }

        // generar reporte de crystal
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta_Directorio_Reportes + Nombre_Pdf);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        // mostrar archivo generado
        String Ruta = Ruta_Directorio_Reportes + Nombre_Pdf;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
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
        string Ruta_Directorio_Reportes = "../../Reporte/";

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

        // si el directorio no existe, crearlo
        if (!Directory.Exists(Server.MapPath(Ruta_Directorio_Reportes)))
        {
            Directory.CreateDirectory(Ruta_Directorio_Reportes);
        }

        // generar reporte de crystal
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta_Directorio_Reportes + Nombre_Xls);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        // mostrar archivo generado
        Abrir_Reporte(Server.MapPath(Ruta_Directorio_Reportes + Nombre_Xls));
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
        // si se encontraron errores al validar campos, regresar mensajes, si no, ejecutar consulta
        if (Error)
        {
            return Cadena_Informacion;
        }
        else
        {
            if (Cmb_Tipo_Reporte.SelectedValue == "FOLIOS_POR_USUARIO")
            {
                Reporte_Negocio.P_Campos_Dinamicos = ", COUNT(" + Ope_Ate_Peticiones.Campo_Usuario_Creo
                    + ") OVER (Partition By " + Ope_Ate_Peticiones.Campo_Usuario_Creo + ") TOTAL_GRUPO";
                Reporte_Negocio.P_Ordenamiento_Dinamico = " TOTAL_GRUPO DESC, " + Ope_Ate_Peticiones.Campo_Usuario_Creo + " ASC";
                Ds_Peticiones = Reporte_Negocio.Consulta_Estadistica_Peticiones_Por_Usuario();
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
                    if (Cmb_Tipo_Reporte.SelectedValue == "FOLIOS_POR_USUARIO")
                    {
                        Ds_Ate_Estadisticas_Peticiones Ds_Reporte = new Ds_Ate_Estadisticas_Peticiones();
                        Generar_Reporte_PDF(Data_Set, Ds_Reporte, "Rpt_Rep_Ate_Folios_Usuario.rpt", "Rpt_Folios_Usuario_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".pdf");
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
                    if (Cmb_Tipo_Reporte.SelectedValue == "FOLIOS_POR_USUARIO")
                    {
                        Ds_Ate_Estadisticas_Peticiones Ds_Reporte = new Ds_Ate_Estadisticas_Peticiones();
                        Exportar_Excel(Data_Set, Ds_Reporte, "Rpt_Rep_Ate_Folios_Usuario.rpt", "Rpt_Folios_Usuario_" + Cls_Sessiones.No_Empleado + DateTime.Now.ToString("yyMMddHHmmss") + ".xls");
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

    #endregion Eventos



}
