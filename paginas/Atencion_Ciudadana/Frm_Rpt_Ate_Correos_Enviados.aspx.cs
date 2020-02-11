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
using Presidencia.Operacion_Atencion_Ciudadana_Registro_Correos_Enviados.Negocio;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using System.IO;
using System.Text;
using Presidencia.Sessiones;

public partial class paginas_Atencion_Ciudadana_Frm_Rpt_Ate_Correos_Enviados : System.Web.UI.Page
{
    #region EVENTOS
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            // inicializar campos con fecha del día actual
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Consultar_Click
    ///DESCRIPCIÓN: Manejador del evento click para el botón consultar que llama al método de consulta 
    ///             y muestra el resultado en el grid
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 20-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Consultar_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Consulta;
        Mostrar_Informacion("");
        try
        {
            Dt_Consulta = Consultar_Correos_Enviados();
            // cargar resultado en el grid de la página
            Grid_Correos_Enviados.DataSource = Dt_Consulta;
            Grid_Correos_Enviados.DataBind();
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al consultar datos: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Exportar_pdf_Click
    ///DESCRIPCIÓN: Manejador del evento click para el botón exportar_pdf que llama al método de consulta 
    ///             y muestra el resultado en un archivo pdf
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 20-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Exportar_pdf_Click(object sender, ImageClickEventArgs e)
    {
        string Archivo_Reporte = "";

        Mostrar_Informacion("");
        try
        {

            Archivo_Reporte = Exportar_Reporte(
                Crear_Ds_Correos_Enviados(),
                "Rpt_Rep_Ate_Correos_Enviados.rpt",
                "Rep_Ate_Corres_Enviados_" + Cls_Sessiones.Empleado_ID + "_" + DateTime.Now.ToString("yyMMdd_HHmmss") + ".pdf",
                ExportFormatType.PortableDocFormat);

            Mostrar_Reporte(Path.GetFileName(Archivo_Reporte), "Reporte");
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al generar pdf: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Btn_Exportar_Excel_Click
    ///DESCRIPCIÓN: Manejador del evento click para el botón exportar_excel que llama al método de consulta 
    ///             y muestra el resultado en un archivo de excel
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 20-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {

        string Archivo_Reporte = "";

        Mostrar_Informacion("");
        try
        {

            Archivo_Reporte = Exportar_Reporte(
                Crear_Ds_Correos_Enviados(),
                "Rpt_Rep_Ate_Correos_Enviados.rpt",
                "Rep_Ate_Corres_Enviados_" + Cls_Sessiones.Empleado_ID + "_" + DateTime.Now.ToString("yyMMdd_HHmmss") + ".xls",
                ExportFormatType.Excel);
            Mostrar_Reporte(Path.GetFileName(Archivo_Reporte), "Reporte");
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion("Error al generar pdf: " + Ex.Message);
        }
    }
    #endregion EVENTOS

    #region METODOS

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Consultar_Correos_Enviados
    ///DESCRIPCIÓN: Ejecuta consulta de correos enviados (aplica filtros opcionales dependiendo de la 
    ///         selección en los controles de la página) y regresa un datatable con los resultados de la consulta
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 15-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Consultar_Correos_Enviados()
    {
        var Neg_Consulta_Envios = new Cls_Ope_Ate_Registro_Correo_Enviados_Negocio();
        DataTable Dt_Correos = null;
        DateTime Fecha_Inicial;
        DateTime Fecha_Final;

        // si se especificó alguno de los filtros, agregar como parámetro
        if (Txt_Email.Text.Trim().Length > 0)
        {
            Neg_Consulta_Envios.P_Email = Txt_Email.Text.Trim();
        }
        if (Cmb_Tipo_Envio.SelectedIndex > 0)
        {
            Neg_Consulta_Envios.P_Motivo = Cmb_Tipo_Envio.SelectedValue;
        }
        if (DateTime.TryParse(Txt_Fecha_Inicio.Text, out Fecha_Inicial) && Fecha_Inicial != DateTime.MinValue)
        {
            Neg_Consulta_Envios.P_Fecha_Inicial = Fecha_Inicial;
        }
        if (DateTime.TryParse(Txt_Fecha_Fin.Text, out Fecha_Final) && Fecha_Final != DateTime.MinValue)
        {
            Neg_Consulta_Envios.P_Fecha_Final = Fecha_Final;
        }
        if (Txt_Nombre_Contribuyente.Text.Trim().Length > 0)
        {
            Neg_Consulta_Envios.P_Nombre_Contribuyente = Txt_Nombre_Contribuyente.Text.Trim();
        }

        //// consultar vacantes
        Dt_Correos = Neg_Consulta_Envios.Consultar_Registro_Correos_Enviados();

        return Dt_Correos;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Informacion
    ///DESCRIPCIÓN: Muestra mensaje de información en la página habilitando los controles correspondientes o limpia el mensaje y oculta los controles si se le pasa una cadena de texto vacía como parámetro
    ///PARÁMETROS:
    /// 		1. Mensaje: texto a mostrar en el área de información de la página
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 20-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Informacion(string Mensaje)
    {
        if (!string.IsNullOrEmpty(Mensaje))
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

    //////*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Exportar_Reporte
    ///DESCRIPCIÓN: genera un archivo con los datos especificados utilizando crystal reports y regresa la ruta al archivo generado
    ///PARÁMETROS:
    /// 		1. Ds_Reporte: Dataset con la información para el reporte
    /// 		2. Nombre_Reporte: nombre del archivo .rpt a utilizar para generar el reporte
    /// 		3. Nombre_Archivo_Destino: nombre del archivo a generar
    /// 		4. Tipo_Reporte: formato del archivo a generar (enumeración ExportFormatType)
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public string Exportar_Reporte(DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_Archivo_Destino, ExportFormatType Tipo_Reporte)
    {
        ReportDocument Rpt_Reporte = new ReportDocument();
        String Ruta_Archivo_Rpt = Server.MapPath("../../paginas/Rpt/Atencion_Ciudadana/" + Nombre_Reporte);
        string Ruta_Archivo_Generado = Server.MapPath("../../Reporte/" + Nombre_Archivo_Destino);

        Rpt_Reporte.Load(Ruta_Archivo_Rpt);

        // generar reporte de crystal
        Rpt_Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Ruta_Archivo_Generado;
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = Tipo_Reporte;
        Rpt_Reporte.Export(Export_Options);
        // regresar ruta al archivo generado
        return Ruta_Archivo_Generado;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Abrir_Reporte
    ///DESCRIPCIÓN: Generar el encabezado para ofrecer para descarga el archivo recibido como parámetro
    ///PARÁMETROS:
    /// 		1. Ruta_Absoluta_Archivo: texto con la ruta absoluta del archivo a generar
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Abrir_Reporte(string Ruta_Absoluta_Archivo)
    {
        string Nombre_Archivo;
        string Extension_Archivo;
        string Tipo_Archivo = "";
        System.IO.FileInfo Archivo = null;

        // validar que el archivo existe
        if (File.Exists(Ruta_Absoluta_Archivo))
        {
            Archivo = new System.IO.FileInfo(Ruta_Absoluta_Archivo);//Crear instancia para leer la información del archivo.

            Nombre_Archivo = Path.GetFileName(Ruta_Absoluta_Archivo);//Obtenemos el nombre completo del archivo.
            Extension_Archivo = Path.GetExtension(Ruta_Absoluta_Archivo);//Obtenemos la extensión del archivo.

            //Validamos la extensión del archivo.
            if (!string.IsNullOrEmpty(Extension_Archivo))
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
            Response.AddHeader("Content-Disposition", "attachment;filename=" + (Archivo.Name));
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
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Crear_Ds_Consulta_Envios
    ///DESCRIPCIÓN: Regresa un dataset con la información en el datatable que recibe como parámetro
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected DataSet Crear_Ds_Correos_Enviados()
    {
        DataSet Ds_Correos = new Ds_Rpt_Ate_Correos_Enviados();

        try
        {
            Ds_Correos.Tables.Clear();
            Ds_Correos.Tables.Add(Consultar_Correos_Enviados().Copy());
            Ds_Correos.Tables[0].TableName = "Dt_Correos_Enviados";
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar contenedor de datos: " + Ex.Message);
        }
        return Ds_Correos;
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Mostrar_Reporte
    ///DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    ///PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// 		2. Tipo: parámetro para ventana modal en la que se mostrará el archivo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 21-nov-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
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

    #endregion METODOS
}
