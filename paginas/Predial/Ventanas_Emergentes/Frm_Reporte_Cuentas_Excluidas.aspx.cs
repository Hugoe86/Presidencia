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
using System.IO;
using Presidencia.Sessiones;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Operacion_Predial_Cuentas_Exluidas_Cierre_Anual.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Reporte_Cuentas_Excluidas : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo del evento carga de la Página
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31-oct-2011
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
            Cargar_Cuenta_Excluidas();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Adeudos
    ///DESCRIPCIÓN          : Metodo que carga los adeudos de la cuenta predial
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31-oct-2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Cuenta_Excluidas()
    {
        Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Negocio Rs_Cuentas = new Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Negocio();
        String Filtro_Estatus = null;
        String Filtro_Tipo_Suspension = null;
        int Total_Cuentas = 0;

        // recuperar parametros recibidos
        if (Request.QueryString["Tipo_Estatus"] != null)
        {
            Filtro_Estatus = Request.QueryString["Tipo_Estatus"].ToString();
            Lbl_Title.Text = " CUENTAS " + Filtro_Estatus + "S:";
        }
        // para cuentas suspendidas
        else if (Request.QueryString["Tipo_Suspension"] != null)
        {
            Filtro_Tipo_Suspension = " IN ('AMBAS','PREDIAL') ";
            Lbl_Title.Text = " CUENTAS SUSPENDIDAS:";
        }

        try
        {
            Total_Cuentas = Rs_Cuentas.Consultar_Total_Cuentas(Filtro_Estatus, Filtro_Tipo_Suspension);
            Lbl_Title.Text = Total_Cuentas + Lbl_Title.Text;

            Grid_Cuentas_Excluidas.DataSource = Rs_Cuentas.Consultar_Cuentas_Por_Estatus(Filtro_Estatus, Filtro_Tipo_Suspension);
            Grid_Cuentas_Excluidas.DataBind();
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
    ///FECHA_CREO           : 1-nov-2010
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
    ///FECHA_CREO           : 1-nov-2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        String Ruta = "Cuentas_Excluidas.xls";

        Ruta = Generar_Archivo_Excel(Ruta);
        if (Ruta !="")
        {
            Mostrar_Archivo(Ruta);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Generar_Archivo_Excel
    /// DESCRIPCIÓN: Generar archivo xls con cuentas y propietarios
    /// PARÁMETROS:
    /// 		1. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 1-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public String Generar_Archivo_Excel(String Nombre_Archivo)
    {
        Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Negocio Rs_Cuentas = new Cls_Ope_Pre_Cuentas_Exluidas_Cierre_Anual_Negocio();
        String Filtro_Estatus = null;
        String Filtro_Tipo_Suspension = null;
        DataTable Dt_Cuentas = null;

        // recuperar parametros recibidos
        if (Request.QueryString["Tipo_Estatus"] != null)
        {
            Filtro_Estatus = Request.QueryString["Tipo_Estatus"].ToString();
            Lbl_Title.Text = " CUENTAS " + Filtro_Estatus + "S:";
        }
        // para cuentas suspendidas
        else if (Request.QueryString["Tipo_Suspension"] != null)
        {
            Filtro_Tipo_Suspension = " IN ('AMBAS','PREDIAL') ";
            Lbl_Title.Text = " CUENTAS SUSPENDIDAS:";
        }

        try
        {
            // consultar los datos
            Dt_Cuentas = Rs_Cuentas.Consultar_Cuentas_Por_Estatus(Filtro_Estatus, Filtro_Tipo_Suspension);
        }
        catch (Exception Ex)
        {
            throw new Exception("Desglose_Adeudos: Cargar_Adeudos: " + Ex.Message);
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
            Fila.Cells.Add(new WorksheetCell("PROPIETARIO"));


            foreach (DataRow Fila_Dato in Dt_Cuentas.Rows)
            {

                Fila = Hoja.Table.Rows.Add();
                // insertar valores en las celdas
                Fila.Cells.Add(new WorksheetCell(Fila_Dato["CUENTA_PREDIAL"].ToString(), "Detalles"));
                Fila.Cells.Add(new WorksheetCell(Fila_Dato["NOMBRE_PROPIETARIO"].ToString(), "Detalles"));
            }

            // si el archivo no existe, 
            string ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
            if (File.Exists(ruta))
            {
                File.Delete(ruta);
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
    ///FECHA_CREO           : 1-nov-2010
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
