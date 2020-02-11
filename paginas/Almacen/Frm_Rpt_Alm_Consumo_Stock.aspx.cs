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
using Presidencia.Consumo_Stock.Negocio;
using Presidencia.Constantes;
using System.Text;
using CarlosAg.ExcelXmlWriter;

public partial class paginas_Almacen_Frm_Rpt_Alm_Consumo_Stock : System.Web.UI.Page
{
    #region (Init/Load)
    /// <summary>
    /// Nombre: Page_Load
    /// 
    /// Descripción: Método que carga la configuración inicial de la página.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 17:24 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack) {
                Configuracion_Inicial();
            }
            Lbl_Ecabezado_Mensaje.Text = string.Empty;
            Lbl_Mensaje_Error.Text = string.Empty;
            IBtn_Imagen_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            IBtn_Imagen_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion

    #region (Metodos)
    /// <summary>
    /// Nombre: Configuracion_Inicial
    /// 
    /// Descripción: Método que habilita la configuración inicial de la página.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 21:02 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    private void Configuracion_Inicial()
    {
        try
        {
            Consultar_Departamentos();//Consultar departamentos que tienen salidas de almacén.
            Consultar_Productos();//Consultar productos que tienen salidas de almacén.
            Consultar_Partidas();//Consultar partidas que tienen salidas de almacén.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error - Método[Configuracion_Inicial]. Excepción: " + Ex.Message);
        }
    }
    /// <summary>
    /// Nombre: Limpiar_Filtros
    /// 
    /// Descripción: Método que limpia los filtros de la búsqueda.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 21:03 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico: 
    /// </summary>
    private void Limpiar_Filtros()
    {
        try
        {
            Txt_Fecha_Inicial.Text = string.Empty;
            Txt_Fecha_Final.Text = string.Empty;
            Cmb_Departamentos.SelectedIndex = (-1);
            Cmb_Productos.SelectedIndex = (-1);
            Cmb_Partidas.SelectedIndex = (-1);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error - Método[Limpiar_Filtros]. Excepción: " + Ex.Message);
        }
    }
    /// <summary>
    /// Nombre: Consultar_Departamentos
    /// 
    /// Descripción: Método que realiza la consulta de los departamentos.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 17:58 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    private void Consultar_Departamentos()
    {
        Cls_Rpt_Alm_Consumo_Stock_Negocio Obj_Consumo_Stock = new Cls_Rpt_Alm_Consumo_Stock_Negocio();//Variable que se utilizara para consultar el consumo de stock.
        DataTable Dt_Resultado = null;//Variable que almacena los resultados de la búsqueda.

        try
        {
            Dt_Resultado = Obj_Consumo_Stock.Consultar_Departamentos();
            Cmb_Departamentos.DataSource = Dt_Resultado;
            Cmb_Departamentos.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Departamentos.DataTextField = "UR";
            Cmb_Departamentos.DataBind();
            Cmb_Departamentos.Items.Insert(0, new ListItem("SELECCIONE", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error - Método[Consultar_Departamentos]. Excepción: " + Ex.Message);
        }
    }
    /// <summary>
    /// Nombre: Consultar_Productos
    /// 
    /// Descripción: Método que realiza la consulta de los productos.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 17:58 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    private void Consultar_Productos()
    {
        Cls_Rpt_Alm_Consumo_Stock_Negocio Obj_Consumo_Stock = new Cls_Rpt_Alm_Consumo_Stock_Negocio();//Variable que se utilizara para consultar el consumo de stock.
        DataTable Dt_Resultado = null;//Variable que almacena los resultados de la búsqueda.

        try
        {
            Dt_Resultado = Obj_Consumo_Stock.Consultar_Productos();
            Cmb_Productos.DataSource = Dt_Resultado;
            Cmb_Productos.DataValueField = Cat_Com_Productos.Campo_Producto_ID;
            Cmb_Productos.DataTextField = "Producto";
            Cmb_Productos.DataBind();
            Cmb_Productos.Items.Insert(0, new ListItem("SELECCIONE", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error - Método[Consultar_Productos]. Excepción: " + Ex.Message);
        }
    }
    /// <summary>
    /// Nombre: Cargar_Consumo_Stock
    /// 
    /// Descripción: Método que realiza la cosulta de las partidas presupuestales.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 17:58 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    private void Consultar_Partidas()
    {
        Cls_Rpt_Alm_Consumo_Stock_Negocio Obj_Consumo_Stock = new Cls_Rpt_Alm_Consumo_Stock_Negocio();//Variable que se utilizara para consultar el consumo de stock.
        DataTable Dt_Resultado = null;//Variable que almacena los resultados de la búsqueda.

        try
        {
            Dt_Resultado = Obj_Consumo_Stock.Consultar_Partidas_Presupuestales();
            Cmb_Partidas.DataSource = Dt_Resultado;
            Cmb_Partidas.DataValueField = Cat_Sap_Partidas_Especificas.Campo_Partida_ID;
            Cmb_Partidas.DataTextField = "Partida";
            Cmb_Partidas.DataBind();
            Cmb_Partidas.Items.Insert(0, new ListItem("SELECCIONE", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error - Método[Consultar_Partidas]. Excepción: " + Ex.Message);
        }
    }
    /// <summary>
    /// Nombre: Pasar_DataTable_A_Excel
    /// 
    /// Descripción: Método que exporta el listado de consumo de stock a excel.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 22:47 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    /// <param name="Dt_Reporte">Listado de consumo de stock a exportar a excel</param>
    public void Pasar_DataTable_A_Excel(System.Data.DataTable Dt_Reporte)
    {
        String Ruta = string.Empty;
        decimal Total = 0;
        decimal Total_Cantidad = 0;

        if (!string.IsNullOrEmpty(Txt_Fecha_Final.Text.Replace("__/___/____", string.Empty)) &&
                !string.IsNullOrEmpty(Txt_Fecha_Final.Text.Replace("__/___/____", string.Empty)))
            Ruta = "Consumo_Stock [" + (String.Format("{0:dd_MMM_yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text))) + " A " +
                (String.Format("{0:dd_MMM_yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text))) + "].xls";
        else
            Ruta = "Consumo de Stock.xls";

        try
        {
            if (Dt_Reporte is DataTable)
            {
                Total = Dt_Reporte.AsEnumerable()
                    .Sum(productos => productos.Field<decimal>("PRECIO"));
                Total_Cantidad = Dt_Reporte.AsEnumerable()
                    .Sum(productos => productos.Field<decimal>("CANTIDAD"));
            }

            //Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

            Libro.Properties.Title = "Reporte de Consumo de Stock";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Almacen";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Registros");
            //Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //Creamos el estilo titulo para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Titulo = Libro.Styles.Add("TitleStyle");
            //Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Subtitulo = Libro.Styles.Add("Subtitulo_Style");
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Producto = Libro.Styles.Add("Producto_Style");

            Estilo_Titulo.Font.FontName = "Arial";
            Estilo_Titulo.Font.Size = 14;
            Estilo_Titulo.Font.Bold = false;
            Estilo_Titulo.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Titulo.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Titulo.Font.Color = "#000000";
            Estilo_Titulo.Interior.Color = "White";
            Estilo_Titulo.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Titulo.Alignment.WrapText = true;

            Estilo_Subtitulo.Font.FontName = "Arial";
            Estilo_Subtitulo.Font.Size = 11;
            Estilo_Subtitulo.Font.Bold = true;
            Estilo_Subtitulo.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Subtitulo.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Subtitulo.Font.Color = "#000000";
            Estilo_Subtitulo.Interior.Color = "White";
            Estilo_Subtitulo.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Subtitulo.Alignment.WrapText = true;

            Estilo_Cabecera.Font.FontName = "Arial";
            Estilo_Cabecera.Font.Size = 8;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Cabecera.Font.Color = "#000000";
            Estilo_Cabecera.Interior.Color = "#C5D9F1";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Alignment.WrapText = true;

            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 8;
            Estilo_Contenido.Font.Bold = false;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Alignment.WrapText = true;

            Estilo_Producto.Font.FontName = "Tahoma";
            Estilo_Producto.Font.Size = 8;
            Estilo_Producto.Font.Bold = false;
            Estilo_Producto.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Estilo_Producto.Font.Color = "#000000";
            Estilo_Producto.Interior.Color = "White";
            Estilo_Producto.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Producto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Producto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Producto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Producto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Producto.Alignment.WrapText = true;

            //SE CARGA LA CABECERA PRINCIPAL DEL ARCHIVO
            WorksheetCell cell = Renglon.Cells.Add();
            Renglon.Cells.Clear();
            cell = Renglon.Cells.Add("JUNTA DE AGUA POTABLE, DRENAJE, ALCANTARILLADO Y ");
            cell.MergeAcross = 6;            // Merge two cells together
            cell.StyleID = "TitleStyle";
            Renglon.Height = 30;
            Renglon = Hoja.Table.Rows.Add();

            cell = Renglon.Cells.Add("SANEAMIENTO DEL MUNICIPIO DE IRAPUATO, GTO. ");
            cell.MergeAcross = 6;            // Merge two cells together
            cell.StyleID = "TitleStyle";
            Renglon.Height = 30;
            Renglon = Hoja.Table.Rows.Add();

            cell = Renglon.Cells.Add("Listado de Consumo de Stock");
            cell.MergeAcross = 6;            // Merge two cells together
            cell.StyleID = "TitleStyle";
            Renglon.Height = 30;
            Renglon = Hoja.Table.Rows.Add();

            if (!string.IsNullOrEmpty(Txt_Fecha_Final.Text.Replace("__/___/____", string.Empty)) && 
                !string.IsNullOrEmpty(Txt_Fecha_Final.Text.Replace("__/___/____", string.Empty)))
            {
                cell = Renglon.Cells.Add("[" + string.Format("{0:dd MMMM yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text)) + " - " +
                    string.Format("{0:dd MMMM yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text)) + "]");
                cell.MergeAcross = 6;            // Merge two cells together
                cell.StyleID = "Subtitulo_Style";
                Renglon.Height = 30;
                Renglon = Hoja.Table.Rows.Add();
            }

            //Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(40));//Clave Producto
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(150));//Producto
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//Unidad
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));//Código Programatico
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(300));//Departamento
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//Cantidad
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//Precio

            if (Dt_Reporte is System.Data.DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns)
                    {
                        if (COLUMNA is System.Data.DataColumn)
                        {
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
                                    if (COLUMNA.DataType.Name.ToLower().Equals("decimal") &&
                                        COLUMNA.ColumnName.ToString().ToLower().Equals("precio"))
                                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(
                                            string.Format("{0:c}", Convert.ToDouble(FILA[COLUMNA.ColumnName])),
                                            DataType.String, "BodyStyle"));
                                    else
                                        Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String,
                                            (COLUMNA.ColumnName.ToString().ToLower().Equals("producto") ? "Producto_Style" : "BodyStyle")));

                                }
                            }
                            Renglon.AutoFitHeight = true;
                        }
                    }
                }
            }

            Renglon = Hoja.Table.Rows.Add();
            Array.ForEach(Dt_Reporte.Columns.OfType<DataColumn>().ToArray(), columna =>
            {                
                if (columna.ColumnName.ToLower().Equals("precio"))
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Total.ToString("c"), "HeaderStyle"));
                else if (columna.ColumnName.ToLower().Equals("cantidad"))
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Total_Cantidad.ToString(), "HeaderStyle"));
                else
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(string.Empty, "HeaderStyle"));
            });
            Renglon.AutoFitHeight = true;

            //Abre el archivo de excel
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Grid)
    /// <summary>
    /// Nombre: Cargar_Consumo_Stock
    /// 
    /// Descripción: Método que realiza la carga y consulta del consumo de stock.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 17:49 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    private void Cargar_Consumo_Stock()
    {
        Cls_Rpt_Alm_Consumo_Stock_Negocio Obj_Consumo_Stock = new Cls_Rpt_Alm_Consumo_Stock_Negocio();//Variable que se utilizara para consultar el consumo de stock.
        DataTable Dt_Resultado = null;//Variable que almacenara el resultado de la búsqueda.

        try
        {
            Obj_Consumo_Stock.P_Dependencia_ID = (Cmb_Departamentos.SelectedIndex > 0) ? Cmb_Departamentos.SelectedValue.Trim() : string.Empty;
            Obj_Consumo_Stock.P_Producto_ID = (Cmb_Productos.SelectedIndex > 0) ? Cmb_Productos.SelectedValue.Trim() : string.Empty;
            Obj_Consumo_Stock.P_Partida_ID = (Cmb_Partidas.SelectedIndex > 0) ? Cmb_Partidas.SelectedValue.Trim() : string.Empty;
            Obj_Consumo_Stock.P_Fecha_Inicio = (!string.IsNullOrEmpty(Txt_Fecha_Inicial.Text.Replace("__/___/____", string.Empty))) ? string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text)) : string.Empty;
            Obj_Consumo_Stock.P_Fecha_Fin = (!string.IsNullOrEmpty(Txt_Fecha_Final.Text.Replace("__/___/____", string.Empty))) ? string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text)) : string.Empty;
            Dt_Resultado = Obj_Consumo_Stock.Consultar_Consumo_Stock();

            Grid_Consumo_Stock.DataSource = (Dt_Resultado.Rows.Count > 0) ? Dt_Resultado : new DataTable();
            Grid_Consumo_Stock.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error - Método[Cargar_Consumo_Stock]. Excepción: " + Ex.Message);
        }
    }
    #endregion

    #region (Eventos)
    /// <summary>
    /// Nombre: Cargar_Consumo_Stock
    /// 
    /// Descripción: Método que realiza la carga y consulta del consumo de stock.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 17:49 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Btn_Consultar_Consumo_Stock_Click(object sender, EventArgs e)
    {
        try
        {
            Cargar_Consumo_Stock();//Ejecuta la consulta del consumo de stock.
        }
        catch (Exception Ex)
        {
            IBtn_Imagen_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    /// <summary>
    /// Nombre: Btn_Reporte_Excel_Click
    /// 
    /// Descripción: Método que genera el reporte de consumo de Stock.
    /// 
    /// Uusario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creo: 05 Diciembre 2013 21:22 Hrs.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    public void Btn_Reporte_Excel_Click(object sender, EventArgs e)
    {
        Cls_Rpt_Alm_Consumo_Stock_Negocio Obj_Consumo_Stock = new Cls_Rpt_Alm_Consumo_Stock_Negocio();//Variable que se utilizara para consultar el consumo de stock.
        DataTable Dt_Resultado = null;//Variable que almacenara el resultado de la búsqueda.
        try
        {
            Obj_Consumo_Stock.P_Dependencia_ID = (Cmb_Departamentos.SelectedIndex > 0) ? Cmb_Departamentos.SelectedValue.Trim() : string.Empty;
            Obj_Consumo_Stock.P_Producto_ID = (Cmb_Productos.SelectedIndex > 0) ? Cmb_Productos.SelectedValue.Trim() : string.Empty;
            Obj_Consumo_Stock.P_Partida_ID = (Cmb_Partidas.SelectedIndex > 0) ? Cmb_Partidas.SelectedValue.Trim() : string.Empty;
            Obj_Consumo_Stock.P_Fecha_Inicio = (!string.IsNullOrEmpty(Txt_Fecha_Inicial.Text.Replace("__/___/____", string.Empty))) ? string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text)) : string.Empty;
            Obj_Consumo_Stock.P_Fecha_Fin = (!string.IsNullOrEmpty(Txt_Fecha_Final.Text.Replace("__/___/____", string.Empty))) ? string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text)) : string.Empty;
            Dt_Resultado = Obj_Consumo_Stock.Consultar_Consumo_Stock();
            Pasar_DataTable_A_Excel(Dt_Resultado);
        }
        catch (Exception Ex)
        {
            IBtn_Imagen_Error.Visible = true;
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }
    #endregion
}
