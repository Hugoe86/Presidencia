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
using Presidencia.Inventarios_De_Stock.Negocio;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using System.Windows.Forms;
using CarlosAg.ExcelXmlWriter;
using System.Text;

public partial class paginas_Almacen_Frm_Ope_Alm_Inventario_Stock : System.Web.UI.Page
{
    private static String P_Dt_Inventario = "INVENTARIO";
    protected void Page_Load(object sender, EventArgs e)
    {
        //Cls_Sessiones.Mostrar_Menu = true;
        if (!IsPostBack) 
        {
            Btn_Guardar.Visible = false;
            Btn_Salir.Visible = true;
            Cargar_Partidas();
            Cmb_Tipo.Items.Clear();
            Cmb_Tipo.Items.Add("EXISTENCIAS");
            Cmb_Tipo.Items.Add("COSTEADO");
        }
        Mostrar_Informacion("",false);
    }
    private DataTable Consultar_Inventario_Stock() 
    {
        Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio = new Cls_Ope_Alm_Inventarios_Stock_Negocio();
        Negocio.P_Clave_Producto = Txt_Clave.Text.Trim();
        Negocio.P_Nombre_Producto = Txt_Producto.Text.Trim();
        Negocio.P_Partida_ID = Cmb_Partida.SelectedValue.Trim();
        if (Cmb_Partida.SelectedIndex == 0) 
        {
            Negocio.P_Partida_ID = null;
        }
        DataTable Dt_Stock = Negocio.Consultar_Inventario_Stock();
        if (Dt_Stock != null && Dt_Stock.Rows.Count > 0)
        {
            Lbl_Registros.Visible = true;
            Lbl_Registros.Text = "Registros Encontrados [" + Dt_Stock.Rows.Count + "]";
            //Lbl_Total_Acumulado.Text = "Total Acumulado: $ " +
            //                    String.Format("{0:n}", Negocio.Consultar_Costeo_Inventario()).ToString();
            double Ac = 0;
            foreach (DataRow Renglon in Dt_Stock.Rows)
            {
                Ac += double.Parse(Renglon["ACUMULADO"].ToString());
            }
            Lbl_Total_Acumulado.Text = "Total Acumulado: $ " +
                String.Format("{0:n}", Ac).ToString();
            if (Cmb_Tipo.SelectedValue == "COSTEADO")
            {
                Lbl_Total_Acumulado.Visible = true;
            }
            else
            {
                Lbl_Total_Acumulado.Visible = false;              
            }
        }
        else 
        {
            Lbl_Registros.Visible = false;
        }
        Session[P_Dt_Inventario] = Dt_Stock;
        Grid_Inventario.DataSource = Dt_Stock;
        Grid_Inventario.DataBind();
        return Dt_Stock;
    }
    private void Cargar_Partidas() 
    {
        Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio = new Cls_Ope_Alm_Inventarios_Stock_Negocio();
        DataTable Dt_Partidas = Negocio.Consultar_Partidas_Stock();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partida, Dt_Partidas, 1, 0);
    }
    protected void Btn_Buscar_Click(object sender, EventArgs e)
    {
        Consultar_Inventario_Stock();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Producto_Click
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Producto_Click(object sender, ImageClickEventArgs e)
    {
        String Producto_ID = ((ImageButton)sender).CommandArgument;
        Hdf_Producto_ID.Value = Producto_ID;
        Btn_Salir.ToolTip = "Regresar";
        Btn_Imprimir.Visible = false;
        Div_Listado.Visible = false;
        Div_Contenido.Visible = true;
        Btn_Guardar.Visible = true;
        Btn_Salir.Visible = true;
        //Cargar datos en interfaz
        DataRow Renglon = ((DataTable)Session[P_Dt_Inventario]).Select("PRODUCTO_ID = '" + Producto_ID + "'")[0];
        Txt_Clave_Modificar.Text = Renglon["CLAVE"].ToString();
        Txt_Nombre_Modificar.Text = Renglon["NOMBRE"].ToString();
        Txt_Descripcion_Modificar.Text = Renglon["DESCRIPCION"].ToString();

        Txt_Existencia_Modificar.Text = Renglon["EXISTENCIA"].ToString();
        Txt_Disponible_Modificar.Text = Renglon["DISPONIBLE"].ToString();
        Txt_Comprometido_Modificar.Text = Renglon["COMPROMETIDO"].ToString();
        Txt_Minimo_Modificar.Text = Renglon["MINIMO"].ToString();
        Txt_Maximo_Modificar.Text = Renglon["MAXIMO"].ToString();
        Txt_Reorden_Modificar.Text = Renglon["REORDEN"].ToString();
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Session.Remove(P_Dt_Inventario);
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {

            Div_Listado.Visible = true;
            Div_Contenido.Visible = false;
            Btn_Salir.Visible = true;
            Btn_Salir.ToolTip = "Inicio";
            Btn_Guardar.Visible = false;
            Btn_Imprimir.Visible = true;
        }
    }
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        if (Modificar_Producto())
        {
            Consultar_Inventario_Stock();
            ScriptManager.RegisterStartupScript(
                               this, this.GetType(),
                               "Requisiciones", "alert('Datos Actualizados');", true);
            Div_Contenido.Visible = false;
            Div_Listado.Visible = true;
            Btn_Salir.Visible = true;
            Btn_Salir.ToolTip = "Inicio";
            Btn_Guardar.Visible = false;
            Btn_Imprimir.Visible = true;
            //Btn_Salir.Visible = false;
        }
        else 
        {
            ScriptManager.RegisterStartupScript(
                               this, this.GetType(),
                               "Requisiciones", "alert('No se pudieron actualizar los datos');", true);
        }
    }

    private bool Modificar_Producto() 
    {
        bool respuesta = false;
        try
        {
            double Minimo = Convert.ToDouble(Txt_Minimo_Modificar.Text.Trim());
            double Maximo = Convert.ToDouble(Txt_Maximo_Modificar.Text.Trim());
            double Pto_Reorden = Convert.ToDouble(Txt_Reorden_Modificar.Text.Trim());
            if (Minimo < Maximo && Pto_Reorden < Maximo && Pto_Reorden > Minimo)
            {
                Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio = new Cls_Ope_Alm_Inventarios_Stock_Negocio();
                Negocio.P_Producto_ID = Hdf_Producto_ID.Value.Trim();
                Negocio.P_Minimo = Txt_Minimo_Modificar.Text.Trim();
                Negocio.P_Maximo = Txt_Maximo_Modificar.Text.Trim();
                Negocio.P_Reorden = Txt_Reorden_Modificar.Text.Trim();
                respuesta = Negocio.Modificar_Producto();
            }
            else
            {
                String Mensaje = "Verifique los datos <br>";
                Mensaje += "&nbsp;&nbsp; Minimo deberá ser menor a Máximo <br>";
                Mensaje += "&nbsp;&nbsp; Punto de Reorden deberá ser menor a Máximo <br>";
                Mensaje += "&nbsp;&nbsp; Punto de Reorden deberá ser mayor a Minimo";
                Mostrar_Informacion(Mensaje  , true);
            }
            
        }
        catch(Exception Ex)
        {
            respuesta = false;
        }
        return respuesta;
    }



    public void Generar_Reporte(DataTable data_set, DataSet ds_reporte, string nombre_reporte)
    {

        ReportDocument reporte = new ReportDocument();
        string filePath = Server.MapPath("../Rpt/Almacen/" + nombre_reporte);

        reporte.Load(filePath);
        DataRow renglon;

        for (int i = 0; i < data_set.Rows.Count; i++)
        {
            renglon = data_set.Rows[i];
            ds_reporte.Tables["INVENTARIO"].ImportRow(renglon);
        }
        reporte.SetDataSource(ds_reporte);

        //1
        ExportOptions exportOptions = new ExportOptions();
        //2
        DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
        //3
        //4
        diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Inventario_Stock_General.pdf");
        //5
        exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
        //6
        exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //7
        exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //8
        reporte.Export(exportOptions);
        //9
        string ruta = "../../Reporte/Rpt_Inventario_Stock_General.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Session[P_Dt_Inventario] != null)
        {
            DataSet Ds_Informacion = new DataSet("INVENTARIO");
            DataTable Dt_Productos = (DataTable)Session[P_Dt_Inventario];
            // Ds_Informacion.Tables.Add(Dt_Productos);
            Ds_Alm_Inventario_Stock_General Ds_Fisico = new Ds_Alm_Inventario_Stock_General();
            if (Cmb_Tipo.SelectedValue == "EXISTENCIAS")
            {
                Generar_Reporte(Dt_Productos, Ds_Fisico, "Rpt_Alm_Inventarios_Generales.rpt");
            }
            else
                if (Cmb_Tipo.SelectedValue == "COSTEADO") 
                {
                    Generar_Reporte(Dt_Productos, Ds_Fisico, "Rpt_Alm_Inventarios_Generales_Costeado.rpt");
                }
        }
        else
        {
            ScriptManager.RegisterStartupScript(
                               this, this.GetType(),
                               "Requisiciones", "alert('Debe realizar una búsqueda de productos');", true);            
        }
    }


    protected void Button1_Click1(object sender, EventArgs e)
    {
    }

    protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Tipo.SelectedValue.Trim() == "EXISTENCIAS")
        {
            Lbl_Total_Acumulado.Visible = false;
            Td_Promedio.Visible = false;
            Td_Acumulado.Visible = false;
            Grid_Inventario.Columns[10].Visible = false;
            Grid_Inventario.Columns[11].Visible = false;
        }
        else
            if (Cmb_Tipo.SelectedValue.Trim() == "COSTEADO") 
            {
                Lbl_Total_Acumulado.Visible = true;
                Td_Promedio.Visible = true;
                Td_Acumulado.Visible = true;
                Grid_Inventario.Columns[10].Visible = true;
                Grid_Inventario.Columns[11].Visible = true;
                if (Session[P_Dt_Inventario] != null)
                {
                    Grid_Inventario.DataSource = (DataTable)Session[P_Dt_Inventario];
                    Grid_Inventario.DataBind();
                }
            }        
    }
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Mostrar_Información
    //DESCRIPCIÓN: Llena las areas de texto con el registro seleccionado del grid
    //RETORNA: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 24/Agosto/2010 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/

    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Lbl_Informacion.Style.Add("color", "#990000");
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion.Text = txt;
    }

    /// *************************************************************************************************************************
    /// Nombre: Generar_Excel
    /// 
    /// Descripción: Metodo que devuelve un libro de excel.
    /// 
    /// Parámetros: Dt_Reporte.- Información que se mostrara en el reporte.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 10/Diciembre/2011.
    /// Usuario Modifico: gustavo Angeles Cruz
    /// Fecha Modifico: 2 febrero 2012
    /// Causa Modificación Cambio de variable y nombres del reporte 
    /// *************************************************************************************************************************
    private CarlosAg.ExcelXmlWriter.Workbook Generar_Excel_AG(DataTable Dt_Reporte)
    {
        //Creamos el libro de Excel.
        CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
        try
        {
            Libro.Properties.Title = "Inventario";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Almacén";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Inventario");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");

            Estilo_Cabecera.Font.FontName = "Tahoma";
            Estilo_Cabecera.Font.Size = 10;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Font.Color = "#FFFFFF";
            Estilo_Cabecera.Interior.Color = "#193d61";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 9;
            Estilo_Contenido.Font.Bold = true;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Renglon = Hoja.Table.Rows.Add();
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("REPORTE DE INVENTARIO STOCK", "HeaderStyle"));
            Renglon = Hoja.Table.Rows.Add();           
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(DateTime.Now.ToString(), "HeaderStyle"));
            Renglon = Hoja.Table.Rows.Add(); Renglon = Hoja.Table.Rows.Add();
            if (Dt_Reporte is System.Data.DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                    {
                        if (COLUMNA is System.Data.DataColumn)
                        {
                            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(150));
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(COLUMNA.ColumnName, "HeaderStyle"));
                        }
                    }

                    foreach (System.Data.DataRow FILA in Dt_Reporte.Rows)
                    {
                        if (FILA is System.Data.DataRow)
                        {
                            Renglon = Hoja.Table.Rows.Add();

                            foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                            {
                                if (COLUMNA is System.Data.DataColumn)
                                {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de Empleados. Error: [" + Ex.Message + "]");
        }
        return Libro;
    }

    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {

        if (Session[P_Dt_Inventario] != null)
        {
            DataSet Ds_Informacion = new DataSet("INVENTARIO");
            DataTable Dt_Productos = (DataTable)Session[P_Dt_Inventario];
            String Nombre_Archivo = "Inventario_Stock.xls";
            String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
            
            Libro = Generar_Excel_AG(Dt_Productos);
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        else
        {
            ScriptManager.RegisterStartupScript(
                               this, this.GetType(),
                               "Requisiciones", "alert('Debe realizar una búsqueda de productos');", true);
        }
    }
}
