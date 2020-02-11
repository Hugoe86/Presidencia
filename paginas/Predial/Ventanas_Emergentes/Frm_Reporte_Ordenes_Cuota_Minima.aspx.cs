using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.IO;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Reporte_Ordenes_Cuota_Minima : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo del evento carga de la Página
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        // validar sesion, si no hay, redireccionar a la pagina de login
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
            Response.Redirect("../../Paginas_Generales/Frm_Apl_Login.aspx");

        if (!IsPostBack)
        {
            Cargar_Ordenes_Generadas();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ordenes_Generadas
    ///DESCRIPCIÓN          : Metodo que carga las ordenes de una fecha dada
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-nov-2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Ordenes_Generadas()
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Ordenes_Generadas = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        String Filtro_Fecha = "";
        String Filtro_Observaciones = "";
        DataTable Dt_Cuentas;

        Lbl_Title.Text = " ORDENES GENERADAS:";

        // recuperar parametros recibidos
        if (Request.QueryString["Fecha"] != null)
        {
            Filtro_Fecha = Request.QueryString["Fecha"].ToString();
        }
        // obtener el año de generacion de adeudos
        if (Request.QueryString["Anio"] != null)
        {
            Filtro_Observaciones = "GENERACION DE ADEUDOS " + Request.QueryString["Anio"].ToString();
        }

        try
        {
            Dt_Cuentas = Ordenes_Generadas.Consultar_Ordenes_Cuota_Minima(Filtro_Fecha, Filtro_Observaciones);

            Grid_Ordenes_Generadas.DataSource = Dt_Cuentas;
            Grid_Ordenes_Generadas.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Desglose_Adeudos: Cargar_Adeudos: " + Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        // cerrar ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Exportar_Excel_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Exportar a Excel
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Ruta = "Ordenes_Cuota_Minima.xls";

        Ruta = Generar_Archivo_Excel(Ruta);
        if (Ruta != "")
        {
            Mostrar_Archivo(Ruta);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Generar_Archivo_Excel
    /// DESCRIPCIÓN: Generar archivo xls con ordenes generadas por cuota minima
    /// PARÁMETROS:
    /// 		1. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public String Generar_Archivo_Excel(String Nombre_Archivo)
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Cuentas = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        String Filtro_Fecha = "";
        String Filtro_Observaciones = "";
        DataTable Dt_Cuentas;

        Lbl_Title.Text = " ORDENES GENERADAS:";

        // recuperar parametros recibidos
        if (Request.QueryString["Fecha"] != null)
        {
            Filtro_Fecha = Request.QueryString["Fecha"].ToString();
        }
        // obtener el año de generacion de adeudos
        if (Request.QueryString["Anio"] != null)
        {
            Filtro_Observaciones = "GENERACION DE ADEUDOS " + Request.QueryString["Anio"].ToString();
        }

        try
        {
            // consultar los datos
            Dt_Cuentas = Rs_Cuentas.Consultar_Ordenes_Cuota_Minima(Filtro_Fecha, Filtro_Observaciones);
        }
        catch (Exception Ex)
        {
            throw new Exception("Generar_Archivo_Excel: " + Ex.Message);
        }

        try
        {
            // archivo excel
            Workbook libro1 = new Workbook();
            WorksheetStyle Hstyle = libro1.Styles.Add("Encabezado");
            WorksheetStyle Dstyle = libro1.Styles.Add("Detalles");
            Worksheet Hoja = libro1.Worksheets.Add("Cuentas");
            WorksheetRow Fila;

            String Ruta_Archivo = @HttpContext.Current.Server.MapPath("../../Archivos/" + Nombre_Archivo);
            libro1.ExcelWorkbook.ActiveSheetIndex = 1;
            libro1.Properties.Version = "xlExcel8";
            libro1.Properties.Author = "Municipio Irapuato SIAS";

            // Estilo de la hoja de encabezado
            Hstyle.Font.FontName = "Arial";
            Hstyle.Font.Size = 10;
            Hstyle.Font.Bold = true;
            Hstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Hstyle.Font.Color = "Black";
            //Estilo de la hoja de detalles
            Dstyle.Font.FontName = "Arial";
            Dstyle.Font.Size = 10;
            Dstyle.Font.Color = "Black";

            Fila = Hoja.Table.Rows.Add();
            // nombres columnas
            Fila.Cells.Add(new WorksheetCell("CUENTA PREDIAL"));
            Fila.Cells.Add(new WorksheetCell("NO. ORDEN"));
            Fila.Cells.Add(new WorksheetCell("NO. NOTA"));


            foreach (DataRow Fila_Dato in Dt_Cuentas.Rows)
            {

                Fila = Hoja.Table.Rows.Add();
                // insertar valores en las celdas
                Fila.Cells.Add(new WorksheetCell(Fila_Dato[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString(), "Detalles"));
                Fila.Cells.Add(new WorksheetCell(Fila_Dato[Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString(), "Detalles"));
                Fila.Cells.Add(new WorksheetCell(Fila_Dato[Ope_Pre_Orden_Variacion.Campo_No_Nota].ToString(), "Detalles"));
            }

            // si el archivo no existe, 
            string ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
            String Nombre_Directorio = Path.GetDirectoryName(ruta);
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
            }
            // si el directorio no existe, eliminarlo
            if (!Directory.Exists(Nombre_Directorio))
            {
                Directory.CreateDirectory(Nombre_Directorio);
            }

            // Guardar archivo
            libro1.Save(ruta);

            return ruta;
        }
        catch (Exception ex)
        {
            throw new Exception("Generar_Archivo_Excel: " + ex.Message.ToString(), ex);
        }

    } // termina metodo Generar_Archivo_Excel

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Mostrar_Archivo(String Ruta)
    {

        // ofrecer para descarga
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();
        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
        //           'Visualiza el archivo
        Response.WriteFile(Ruta);
        Response.Flush();
        Response.Close();
    }

}
