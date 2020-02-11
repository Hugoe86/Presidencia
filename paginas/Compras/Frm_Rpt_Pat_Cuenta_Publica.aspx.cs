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
using Presidencia.Sessiones;
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Text;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;

public partial class paginas_Compras_Frm_Rpt_Pat_Cuenta_Publica : System.Web.UI.Page {

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se carga cuando la Pagina de Inicia.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Diciembre/2011  
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Combos();
                Txt_Fecha_Inicial.Text = String.Format("{0:dd/MMM/yyyy}", new DateTime(DateTime.Today.Year, DateTime.Today.Month, 1));
                Txt_Fecha_Final.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
            }
        }

    #endregion

    #region "Metodos"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN: Llena los combos del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Llenar_Combos()  {
        try {
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            DataTable Dt_Temporal = null;

            //Se llena combo de Dependencias
            BM_Negocio.P_Tipo_DataTable = "DEPENDENCIAS";
            BM_Negocio.P_Estatus = "ACTIVO";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Dependencia.DataSource = Dt_Temporal;
            Cmb_Dependencia.DataTextField = "NOMBRE";
            Cmb_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencia.DataBind();
            Cmb_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));

            Cls_Cat_Pat_Com_Procedencias_Negocio Procedencia_Negocio = new Cls_Cat_Pat_Com_Procedencias_Negocio();
            Procedencia_Negocio.P_Estatus = "VIGENTE";
            Procedencia_Negocio.P_Tipo_DataTable = "PROCEDENCIAS";
            Cmb_Procedencia.DataSource = Procedencia_Negocio.Consultar_DataTable();
            Cmb_Procedencia.DataTextField = "NOMBRE";
            Cmb_Procedencia.DataValueField = "PROCEDENCIA_ID";
            Cmb_Procedencia.DataBind();
            Cmb_Procedencia.Items.Insert(0, new ListItem("< TODAS >", ""));

        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Verificar al Llenar los Combos.";
            Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Busqueda_Empleados_Resguardo
    ///DESCRIPCIÓN: Llena el Grid con los empleados que cumplan el filtro
    ///PROPIEDADES:     
    ///CREO:                 
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 14/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Generar_Reporte(String Tipo) {
        try {
            Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            if (Cmb_Movimiento.SelectedIndex > 0) { Negocio.P_Tipo = Cmb_Movimiento.SelectedItem.Value.Trim(); } else { Negocio.P_Tipo = "TODAS"; }
            if (Cmb_Tipo_Bien.SelectedIndex > 0) { Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value.Trim(); } else { Negocio.P_Tipo_Bien = "TODOS"; }
            if (Cmb_Procedencia.SelectedIndex > 0) { Negocio.P_Procedencia = Cmb_Procedencia.SelectedItem.Value.Trim(); }
            if (Cmb_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value.Trim(); }
            if (Txt_Fecha_Inicial.Text.Trim().Length > 0) {
                Negocio.P_Fecha_Modificacion_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim());
                Negocio.P_Tomar_Fecha_Inicial_Modificacion = true;
            }  
            if (Txt_Fecha_Final.Text.Trim().Length > 0) {
                Negocio.P_Fecha_Modificacion_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim());
                Negocio.P_Tomar_Fecha_Final_Modificacion = true;
            }
            DataTable Dt_Datos = Negocio.Obtener_Datos_Reporte_Cuenta_Publica_Completo();

            if (Tipo.Equals("VER_PDF")) {
                Dt_Datos.TableName = "DT_DATOS";
                DataSet Ds_Consulta = new DataSet();
                Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                Ds_Rpt_Pat_Cuenta_Publica Ds_Reporte = new Ds_Rpt_Pat_Cuenta_Publica();
                Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Cuenta_Publica.rpt", "PDF");
            } else  if (Tipo.Equals("VER_EXCEL")) {
                Dt_Datos.TableName = "DT_DATOS";
                DataSet Ds_Consulta = new DataSet();
                Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                Ds_Rpt_Pat_Cuenta_Publica Ds_Reporte = new Ds_Rpt_Pat_Cuenta_Publica();
                Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Cuenta_Publica.rpt", "EXCEL");
            } else if (Tipo.Equals("DESCARGAR_EXCEL")) {
                Cambiar_Encabezados(Dt_Datos);
                Pasar_DataTable_A_Excel(Dt_Datos);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta.
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, String Nombre_Reporte, String Tipo) {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        if (Tipo.Equals("PDF")) { Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Cuenta_Publica.pdf"); } else if (Tipo.Equals("EXCEL")) { Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Cuenta_Publica.xls"); }
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        if (Tipo.Equals("PDF")) { Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat; } else if (Tipo.Equals("EXCEL")) { Export_Options.ExportFormatType = ExportFormatType.Excel; }
        Reporte.Export(Export_Options);
        String Ruta = "";
        if (Tipo.Equals("PDF")) { Ruta = "../../Reporte/Rpt_Pat_Cuenta_Publica.pdf"; } else if (Tipo.Equals("EXCEL")) { Ruta = "../../Reporte/Rpt_Pat_Cuenta_Publica.xls"; }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Filtros
    ///DESCRIPCIÓN: Genera y Carga los Filtros del Reporte
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 14/Dic/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Obtener_Filtros() {
        DataTable Dt_Datos = new DataTable();
        Dt_Datos.Columns.Add("Filtro", Type.GetType("System.String"));
        Dt_Datos.Columns.Add("Valor", Type.GetType("System.String"));
        if (Cmb_Dependencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Unidad Responsable: ", Cmb_Dependencia.SelectedItem.Text.Trim()); }
        if (Cmb_Procedencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Procedencia: ", Cmb_Procedencia.SelectedItem.Text.Trim()); }
        if (Txt_Fecha_Inicial.Text.Trim().Length > 0) {
            Cargar_Filtro(Dt_Datos, "Fecha Inicial: ", Txt_Fecha_Inicial.Text.Trim());
        }
        if (Txt_Fecha_Final.Text.Trim().Length > 0) {
            Cargar_Filtro(Dt_Datos, "Fecha Final: ", Txt_Fecha_Final.Text.Trim());
        }
        if (Cmb_Tipo_Bien.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Tipo de Bienes: ", Cmb_Tipo_Bien.SelectedItem.Text.Trim()); } else { Cargar_Filtro(Dt_Datos, "Tipo de Bienes: ", "ANIMALES/BIENES MUEBLES/VEHÍCULOS"); }
        if (Cmb_Movimiento.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Tipo de Movimientos: ", Cmb_Movimiento.SelectedItem.Text.Trim()); } else { Cargar_Filtro(Dt_Datos, "Tipo de Movimientos: ", "ALTAS/BAJAS"); }
        return Dt_Datos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Filtro
    ///DESCRIPCIÓN: Carga los Filtros.
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 14/Dic/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Cargar_Filtro(DataTable Dt_Datos, String Filtro, String Valor) {
        if (Dt_Datos != null) {
            DataRow Fila = Dt_Datos.NewRow();
            Fila["Filtro"] = Filtro;
            Fila["Valor"] = Valor;
            Dt_Datos.Rows.Add(Fila);
        }
        return Dt_Datos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cambiar_Encabezados
    ///DESCRIPCIÓN: Cambia los nombres de los encabezados antes de mandarlos a excel
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 14/Dic/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Cambiar_Encabezados(DataTable Dt_Datos) {
        Dt_Datos.Columns["MOVIMIENTO"].ColumnName = "MOVIMIENTO";
        Dt_Datos.Columns["FECHA"].ColumnName = "FECHA";
        Dt_Datos.Columns["CANTIDAD"].ColumnName = "CANTIDAD";
        Dt_Datos.Columns["TIPO_BIEN"].ColumnName = "TIPO DE BIEN";
        Dt_Datos.Columns["NUMERO_INVENTARIO"].ColumnName = "CLAVE DEL REGISTRO O DEL INVENTARIO";
        Dt_Datos.Columns["CARACTERISTICAS"].ColumnName = "CARACTERISTICAS DEL BIEN, MARCA, MODELO, COLOR, SERIE, MATERIAL";
        Dt_Datos.Columns["CONDICIONES"].ColumnName = "CONDICIONES";
        Dt_Datos.Columns["DEPENDENCIA"].ColumnName = "ASIGNADO AL DEPARTAMENTO";
        Dt_Datos.Columns["RESPONSABLE"].ColumnName = "RESPONSABLE";
        Dt_Datos.Columns["IMPORTE"].ColumnName = "IMPORTE";
        Dt_Datos.Columns["PROVEEDOR"].ColumnName = "PROVEEDOR";
        Dt_Datos.Columns["NO_FACTURA"].ColumnName = "NO. DE FACTURA";
        Dt_Datos.Columns["OBSERVACIONES"].ColumnName = "OBSERVACIONES";
        return Dt_Datos;
    }

    /// *************************************************************************************************************************
    /// Nombre: Pasar_DataTable_A_Excel
    /// 
    /// Descripción: Pasa DataTable a Excel.
    /// 
    /// Parámetros: Dt_Reporte.- DataTable que se pasara a excel.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 18/Octubre/2011.
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// *************************************************************************************************************************
    public void Pasar_DataTable_A_Excel(System.Data.DataTable Dt_Reporte)
    {
        String Ruta = "Cuenta Publica [" + (String.Format("{0:dd_MMM_yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text))) + " A " + (String.Format("{0:dd_MMM_yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text))) + "].xls";//Variable que almacenara el nombre del archivo. 

        try
        {
            //Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

            Libro.Properties.Title = "Reporte de Cuenta Publica de Patrimonio";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Patrimonio";

            //Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Registros");
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
            Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Cabecera.Font.Color = "#FFFFFF";
            Estilo_Cabecera.Interior.Color = "#193d61";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Alignment.WrapText = true;

            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 8;
            Estilo_Contenido.Font.Bold = true;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Alignment.WrapText = true;

            //Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//Movimiento
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));//Fecha
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(70));//Cantidad
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Tipo de Bien
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));//Numero de Inventario.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Caracteristicas.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Condiciones.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(180));//Dependencia.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(250));//Proveedor.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(250));//Responsable.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(115));//Importe.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Factura.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(250));//Observaciones.
            if (Dt_Reporte.Columns.Contains("BIEN_ID")) Dt_Reporte.Columns.Remove("BIEN_ID");
            if (Dt_Reporte is System.Data.DataTable) {
                if (Dt_Reporte.Rows.Count > 0) {
                    foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns) {
                        if (COLUMNA is System.Data.DataColumn) {
                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(COLUMNA.ColumnName, "HeaderStyle"));
                        }
                        Renglon.Height = 65;
                    }

                    foreach (System.Data.DataRow FILA in Dt_Reporte.Rows) {
                        if (FILA is System.Data.DataRow) {
                            Renglon = Hoja.Table.Rows.Add();

                            foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns) {
                                if (COLUMNA is System.Data.DataColumn) {
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                }
                            }
                            Renglon.Height = 35;
                            Renglon.AutoFitHeight = true;
                        }
                    }
                }
            }

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

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Lanza el Reporte en PDF para Imprimir.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
            try {
                Mev_Mee_Txt_Fecha_Inicial.Validate();
                Mev_Txt_Fecha_Final.Validate();
                Generar_Reporte("VER_PDF");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Excel_Click
        ///DESCRIPCIÓN: Lanza el Reporte en Excel.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e) {
            try {
                Mev_Mee_Txt_Fecha_Inicial.Validate();
                Mev_Txt_Fecha_Final.Validate();
                Generar_Reporte("VER_EXCEL");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Descargar_Excel_Click
        ///DESCRIPCIÓN: Descarga el Listado a Excel.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 21/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Btn_Descargar_Excel_Click(object sender, EventArgs e) {
             try {
                Mev_Mee_Txt_Fecha_Inicial.Validate();
                Mev_Txt_Fecha_Final.Validate();
                Generar_Reporte("DESCARGAR_EXCEL");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        
    #endregion

}