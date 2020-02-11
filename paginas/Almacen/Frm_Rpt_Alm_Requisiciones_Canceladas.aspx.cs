using System;
using System.Diagnostics;
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
using System.Text;
using Presidencia.DateDiff;
using System.Text.RegularExpressions;
using System.Data.OracleClient;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Almacen_Reporte_Requisiciones_Canceladas.Negocio;

public partial class paginas_Almacen_Frm_Rpt_Alm_Requisiciones_Canceladas : System.Web.UI.Page
{
    #region Variables
    public static String Global_Ruta = "";
    #endregion

    #region Load
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Refresca la sesion del usuario logeado en el sistema
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que existe un usuario logueado en el sistema
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
                Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Inicializa_Controles " + Ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           29/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha()
    {
        DateTime Date1 = new DateTime();  //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date2 = new DateTime();
        Boolean Fecha_Valida = true;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if ((Txt_Fecha_Inicial.Text.Length != 0))
            {
                // Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Final.Text);

                //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    Fecha_Valida = true;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = " La fecha inicial no pude ser mayor que la fecha final <br />";
                    Fecha_Valida = false;
                }
            }

            return Fecha_Valida;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
            throw new Exception(ex.Message, ex);
        }
    }

///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Rpt_Presupuesto_Excel
    /// DESCRIPCION :   Se encarga de generar el archivo de excel pasandole los paramentros
    ///                 al documento
    /// PARAMETROS  :   Dt_Reporte.- Es la consulta que se va a reportar
    /// CREO        :   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   29/Diciembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public void Generar_Rpt_Requisiciones_Candeladas(System.Data.DataTable Dt_Reporte)
    {
        Cls_Rpt_Alm_Requisiciones_Canceladas_Negocio Rs_Requisicion = new Cls_Rpt_Alm_Requisiciones_Canceladas_Negocio();
        WorksheetCell Celda = new WorksheetCell();
        System.Data.DataTable Dt_Consulta = new DataTable();
        System.Data.DataTable Dt_Comentarios= new DataTable();
        String Nombre_Archivo = "";
        String Ruta_Archivo = ""; 
        Double Importe = 0.0;
        String Informacion_Registro = "";
        String[] Fecha_Inicial;
        String[] Fecha_Final;
        String Inicio = "";
        String Final = "";

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Fecha_Inicial = Txt_Fecha_Inicial.Text.Split('/' );
            Fecha_Final = Txt_Fecha_Final.Text.Split('/');

            for (int Contador = 0; Contador <= 2; Contador++)
            {
                if (Contador == 0)
                {
                    Inicio = Fecha_Inicial[Contador];
                    Final = Fecha_Final[Contador];
                }
                else
                {
                    Inicio += "_" + Fecha_Inicial[Contador];
                    Final += "_" + Fecha_Final[Contador];
                }
            }
            Nombre_Archivo = "Rpt_Requisiciones_Canceladas_" + Inicio + "_AL_" + Final + ".xls";
            Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);

            //  Creamos el libro de Excel.
            CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();
            //  propiedades del libro
            Libro.Properties.Title = "Reporte_Requisiciones_Canceladas";
            Libro.Properties.Created = DateTime.Now;
            Libro.Properties.Author = "Presidencia_RH";            

            //  Creamos el estilo cabecera para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
            //  Creamos el estilo cabecera 2 para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera2 = Libro.Styles.Add("HeaderStyle2");
            //  Creamos el estilo cabecera 3 para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera3 = Libro.Styles.Add("HeaderStyle3");
            //  Creamos el estilo contenido para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");
            //  Creamos el estilo contenido del presupuesto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Presupuesto = Libro.Styles.Add("Presupuesto");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Concepto = Libro.Styles.Add("Concepto");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 
            CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cancelacion = Libro.Styles.Add("Cancelacion");
            //  Creamos el estilo contenido del concepto para la hoja de excel. 

            //***************************************inicio de los estilos***********************************************************
            //  estilo para la cabecera
            Estilo_Cabecera.Font.FontName = "Tahoma";
            Estilo_Cabecera.Font.Size = 12;
            Estilo_Cabecera.Font.Bold = true;
            Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Cabecera.Font.Color = "#FFFFFF";
            Estilo_Cabecera.Interior.Color = "Gray";
            Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para la cabecera2
            Estilo_Cabecera2.Font.FontName = "Tahoma";
            Estilo_Cabecera2.Font.Size = 10;
            Estilo_Cabecera2.Font.Bold = true;
            Estilo_Cabecera2.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera2.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Cabecera2.Font.Color = "#FFFFFF";
            Estilo_Cabecera2.Interior.Color = "DarkGray";
            Estilo_Cabecera2.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para la cabecera3
            Estilo_Cabecera3.Font.FontName = "Tahoma";
            Estilo_Cabecera3.Font.Size = 10;
            Estilo_Cabecera3.Font.Bold = true;
            Estilo_Cabecera3.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Cabecera3.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Cabecera3.Font.Color = "#000000";
            Estilo_Cabecera3.Interior.Color = "white";
            Estilo_Cabecera3.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cabecera3.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera3.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera3.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cabecera3.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para el BodyStyle
            Estilo_Contenido.Font.FontName = "Tahoma";
            Estilo_Contenido.Font.Size = 9;
            Estilo_Contenido.Font.Bold = false;
            Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Estilo_Contenido.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Contenido.Font.Color = "#000000";
            Estilo_Contenido.Interior.Color = "White";
            Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para el Concepto
            Estilo_Concepto.Font.FontName = "Tahoma";
            Estilo_Concepto.Font.Size = 9;
            Estilo_Concepto.Font.Bold = false;
            Estilo_Concepto.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Estilo_Concepto.Alignment.Vertical = StyleVerticalAlignment.JustifyDistributed;
            Estilo_Concepto.Font.Color = "#000000";
            Estilo_Concepto.Interior.Color = "White";
            Estilo_Concepto.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Concepto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Concepto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para el presupuesto (importe)
            Estilo_Presupuesto.Font.FontName = "Tahoma";
            Estilo_Presupuesto.Font.Size = 9;
            Estilo_Presupuesto.Font.Bold = false;
            Estilo_Presupuesto.Alignment.Horizontal = StyleHorizontalAlignment.Right;
            Estilo_Presupuesto.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Presupuesto.Font.Color = "#000000";
            Estilo_Presupuesto.Interior.Color = "White";
            Estilo_Presupuesto.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Presupuesto.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Presupuesto.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //  estilo para la cancelacion
            Estilo_Cancelacion.Font.FontName = "Tahoma";
            Estilo_Cancelacion.Font.Size = 9;
            Estilo_Cancelacion.Font.Bold = false;
            Estilo_Cancelacion.Alignment.Horizontal = StyleHorizontalAlignment.JustifyDistributed;
            Estilo_Cancelacion.Alignment.Vertical = StyleVerticalAlignment.Center;
            Estilo_Cancelacion.Font.Color = "#000000";
            Estilo_Cancelacion.Interior.Color = "White";
            Estilo_Cancelacion.Interior.Pattern = StyleInteriorPattern.Solid;
            Estilo_Cancelacion.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cancelacion.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cancelacion.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Estilo_Cancelacion.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            //*************************************** fin de los estilos***********************************************************

            //***************************************Inicio del reporte requisiciones canceladas Hoja 1**********************************************************
           
            //  Creamos una hoja que tendrá el libro.
            CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Requisiciones Canceladas");
            //  Agregamos un renglón a la hoja de excel.
            CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
            //  Agregamos las columnas que tendrá la hoja de excel.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  1 No_Reserva.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  2 Requisicion.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//  3 Estatus.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(150));// 4 Codigo Programatico.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));// 5 total requisicion.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));// 6 total salida.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));// 7 total salida.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));// 8 fecha_Creo.
            Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(200));// 9 Motivo De La cancelacion.

            //  se llena el encabezado principal
            Renglon = Hoja.Table.Rows.Add();
            Celda = Renglon.Cells.Add("Requisiciones De Stock Canceladas del periodo " + Txt_Fecha_Inicial.Text + " al " + Txt_Fecha_Final.Text);
            Celda.MergeAcross = 7; // Merge 9 cells together
            Celda.StyleID = "HeaderStyle";

            Renglon = Hoja.Table.Rows.Add();
            Renglon = Hoja.Table.Rows.Add();

            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("NO RESERVA", "HeaderStyle2"));
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("REQUISICION", "HeaderStyle2"));
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("ESTATUS", "HeaderStyle2"));
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CODIGO PROGRAMATICO", "HeaderStyle2"));
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TOTAL REQUISICON", "HeaderStyle2"));
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TOTAL SALIDA", "HeaderStyle2"));
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("TOTAL CANCELACION", "HeaderStyle2"));
            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("FECHA MOVIMIENTO", "HeaderStyle2"));
            //Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("MOTIVO CANCELACION", "HeaderStyle2"));

            if (Dt_Reporte.Rows.Count > 0)
            {
                //  SE comienza a extraer la informaicon de la onsulta
                foreach (DataRow Renglon_Reporte in Dt_Reporte.Rows)
                {
                    Renglon = Hoja.Table.Rows.Add();

                    //  para numero de reserva
                    Informacion_Registro = (Renglon_Reporte["NO_RESERVA"].ToString());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));

                    //  para la requisicion
                    Informacion_Registro = (Renglon_Reporte["REQUISICION"].ToString().Trim());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));

                    //  para el estatus
                    Informacion_Registro = (Renglon_Reporte["ESTATUS"].ToString());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));

                    //  para el CODIGO PROGRAMATICO
                    Informacion_Registro = (Renglon_Reporte["CODIGO_PROGRAMATICO"].ToString());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));

                    //  para el total de requisiciones
                    Informacion_Registro = (Renglon_Reporte["TOTAL_REQUISICION"].ToString());
                    Importe = Convert.ToDouble(Informacion_Registro);
                    Informacion_Registro = String.Format("{0:n}", Importe);
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "Presupuesto"));

                    //  para el total de salida
                    Informacion_Registro = (Renglon_Reporte["TOTAL_SALIDA"].ToString());
                    Importe = Convert.ToDouble(Informacion_Registro);
                    Informacion_Registro = String.Format("{0:n}", Importe);
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "Presupuesto"));

                    //  para el total de Cancelacion
                    Informacion_Registro = (Renglon_Reporte["TOTAL_CANCELADO"].ToString());
                    Importe = Convert.ToDouble(Informacion_Registro);
                    Informacion_Registro = String.Format("{0:n}", Importe);
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "Presupuesto"));

                    //  para la fecha 
                    Informacion_Registro = (Renglon_Reporte["FECHA_MOVIMIENTO"].ToString());
                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));
                }
            }
            else
            {
                Renglon = Hoja.Table.Rows.Add();
                Informacion_Registro = "-----";
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  1
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  2
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  3
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  4
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  5
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  6
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  7
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  8
                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Informacion_Registro, "BodyStyle"));//  9
            }
            //*************************************** Fin del reporte requisiciones canceladas Hoja 1************************************************************
            Global_Ruta = Ruta_Archivo;
            //se guarda el documento
            Libro.Save(Ruta_Archivo);
            
            //Abre el archivo de excel
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();
            //Response.Buffer = true;
            Response.ContentType = "application/x-msexcel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            //Libro.Save(Response.OutputStream);
            Response.WriteFile(Ruta_Archivo);
            Response.Flush();
            Response.Close();

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }

    #endregion

    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  29/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        System.Data.DataTable Dt_Consulta = new System.Data.DataTable();
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (!Verificar_Fecha())
            {
                Div_Contenedor_Msj_Error.Visible = true;
            }
            else
            {
                Dt_Consulta = Realizar_Consulta();
                Generar_Rpt_Requisiciones_Candeladas(Dt_Consulta);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  29/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Realizar_Consulta
    ///DESCRIPCIÓN: Realizara la consulta de lo que s quiere imprimir
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  29/Diciembre/2011
    ///MODIFICO: GAC
    ///FECHA_MODIFICO:8 JUN 2012
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
     public System.Data.DataTable Realizar_Consulta()
    {
        Cls_Rpt_Alm_Requisiciones_Canceladas_Negocio Rs_Consulta = new Cls_Rpt_Alm_Requisiciones_Canceladas_Negocio();
        System.Data.DataTable Dt_Consulta = new System.Data.DataTable();
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

                Rs_Consulta.P_Fecha_Inicial =  String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim()));
                Rs_Consulta.P_Fecha_Final = String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Fecha_Final.Text.Trim()));

                Dt_Consulta = Rs_Consulta.Consultar_Requisiciones_Canceladas();
           
            return Dt_Consulta;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message;
            throw new Exception(ex.Message, ex);
        }

    }
    
    #endregion


}
