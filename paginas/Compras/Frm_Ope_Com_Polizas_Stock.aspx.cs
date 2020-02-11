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
using Presidencia.Polizas_Stock.Negocio;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
public partial class paginas_Compras_Frm_Ope_Com_Polizas_Stock : System.Web.UI.Page
{
    public static String P_Salidas_Stock = "SALIDAS_STOCK";
    public static String P_Polizas_Stock_Sap = "POLIZAS_STOCK_SAP";
    public static String P_Salidas_Seleccionadas = "SALIDAS_SELECCIONADAS";
    public static String P_Importe = "IMPORTE"; 
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //llenar campos de fecha
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            //new DateTime(2011, 1, 1, 0, 0, 0, 0)
            Txt_Fecha_Inicial.Text = new DateTime(2011, 1, 1, 0, 0, 0, 0).ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Inicial_Poliza.Text = new DateTime(2011, 1, 1, 0, 0, 0, 0).ToString("dd/MMM/yyyy").ToUpper();//_DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final_Poliza.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            //llenbar combos
            Cmb_Contabilizada.Items.Clear();
            Cmb_Contabilizada.Items.Add("NO");
            Cmb_Contabilizada.Items.Add("SI");
            Cmb_Contabilizada.SelectedIndex = 0;
            //cargar datos en grid
            Llenar_Grid_Salidas();
            //Variable se session
            Hdn_Lista_Salidas.Value = "";
            Hdn_Importe.Value = "";
            Lnk_Mostrar.Visible = false;
            //manejo de div
            //Div_Encabezado.Visible = true;
            Div_Listado_Salidas.Visible = true;
            Div_Polizas_Stock_Sap.Visible = false;
        }
        Lnk_Mostrar.Visible = false;
    }
    private void Llenar_Grid_Salidas() 
    {
        Cls_Ope_Com_Polizas_Stock_Negocio Negocio = new Cls_Ope_Com_Polizas_Stock_Negocio();
        Negocio.P_Contabilizada = Cmb_Contabilizada.SelectedValue.Trim();
        Negocio.P_Fecha_Inicio = Txt_Fecha_Inicial.Text.Trim();
        Negocio.P_Fecha_Fin = Txt_Fecha_Final.Text.Trim();
        DataTable Dt_Ordenes_Salida = Negocio.Consultar_Ordenes_Salida_Stock();
        if (Dt_Ordenes_Salida != null && Dt_Ordenes_Salida.Rows.Count > 0)
        {
            Session[P_Salidas_Stock] = Dt_Ordenes_Salida;
            Grid_Salidas.DataSource = Dt_Ordenes_Salida;
            Grid_Salidas.DataBind();
        }
        else 
        {
            Session[P_Salidas_Stock] = null;
            Grid_Salidas.DataSource = null;
            Grid_Salidas.DataBind(); 
        }
    }

    private void Llenar_Grid_Polizas_Sap()
    {
        Cls_Ope_Com_Polizas_Stock_Negocio Negocio = new Cls_Ope_Com_Polizas_Stock_Negocio();
        //Negocio.P_Contabilizada = Cmb_Contabilizada.SelectedValue.Trim();
        Negocio.P_Fecha_Inicio = Txt_Fecha_Inicial_Poliza.Text.Trim();
        Negocio.P_Fecha_Fin = Txt_Fecha_Final_Poliza.Text.Trim();
        DataTable Dt_Polizas_Stock_Sap = Negocio.Consultar_Polizas_Stock_Generadas_Para_SAP();
        if (Dt_Polizas_Stock_Sap != null && Dt_Polizas_Stock_Sap.Rows.Count > 0)
        {
            Session[P_Polizas_Stock_Sap] = Dt_Polizas_Stock_Sap;
            Grid_Polizas_Stock_Sap.DataSource = Dt_Polizas_Stock_Sap;
            Grid_Polizas_Stock_Sap.DataBind();
        }
        else
        {
            Session[P_Polizas_Stock_Sap] = null;
            Grid_Polizas_Stock_Sap.DataSource = null;
            Grid_Polizas_Stock_Sap.DataBind();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Verifica_Seleccionadas
    //  DESCRIPCIÓN: 
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 27-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public String Verifica_Salidas_Seleccionadas()
    {
        String Requisas_Seleccionadas = "";
        int Contador = 0;
        double Suma_Importes = 0;
        double Importe = 0;
        try
        {
            Excel.Application oXL;
            Excel._Workbook oWB;
            Excel._Worksheet oSheet;
            //Start Excel and get Application object.
            oXL = new Excel.Application();
            oXL.Visible = true;
            //Get a new workbook.
            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;
            //formato a celdas
            //formato de Texto
            oSheet.get_Range(oSheet.Cells[1, 2], oSheet.Cells[1, 2]).Cells.EntireColumn.NumberFormat = "@";
            //formato de numeros
            oSheet.get_Range(oSheet.Cells[1, 11], oSheet.Cells[1, 11]).Cells.EntireColumn.NumberFormat = "#,##0.00";
                
            Contador = 1;
            int indice = -1;

            foreach (GridViewRow Renglon_Grid in Grid_Salidas.Rows)
            {

                indice++;
                Grid_Salidas.SelectedIndex = indice;                
                bool Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Salida")).Checked;
                if (Seleccionado)
                {
                    Contador++;
                    DataTable Dt_Salidas = Session[P_Salidas_Stock] as DataTable;
                    String No_Salida = Grid_Salidas.SelectedDataKey["NO_SALIDA"].ToString();
                    DataRow[] Ordenes_Salida = Dt_Salidas.Select("NO_SALIDA ='" + No_Salida + "'");
                    String Codigo = Ordenes_Salida[0]["CODIGO_PROGRAMATICO"].ToString().Trim();
                    String PEP = Ordenes_Salida[0]["ELEMENTO_PEP"].ToString().Trim();
                    char[] ch = {'-'};
                    if (Contador == 2)
                    {
                        //Sociedad
                        oSheet.Cells[1, 1] = "Sociedad";
                        oSheet.Cells[Contador, 1] = "M15";
                        //Fecha
                        oSheet.Cells[1, 2] = "Fecha";
                        oSheet.Cells[Contador, 2] = DateTime.Now.ToString("ddMMyyy");
                        //Periodo
                        oSheet.Cells[1, 3] = "Periodo";
                        oSheet.Cells[Contador, 3] = "8";
                        //Clase Docto
                        oSheet.Cells[1, 4] = "Clase Docto";
                        oSheet.Cells[Contador, 4] = "SA";
                        //Referencia
                        oSheet.Cells[1, 5] = "Referencia";
                        oSheet.Cells[Contador, 5] = "REGISTRO SALIDAS ALMACEN";
                        //Texto de cabecera
                        oSheet.Cells[1, 6] = "Texto de Cabecera";
                        oSheet.Cells[Contador, 6] = "SALIDAS ALM ";
                        //Clave
                        oSheet.Cells[1, 7] = "Clave";
                        oSheet.Cells[Contador, 7] = "40";
                        //Cuenta
                        oSheet.Cells[1, 8] = "Cuenta";
                        oSheet.Cells[Contador, 8] = Ordenes_Salida[0]["CUENTA"].ToString().Trim();//"512102111";
                        //CME
                        oSheet.Cells[1, 9] = "CME";
                        oSheet.Cells[Contador, 9] = "/";
                        //Clase Mov Act. Fijo
                        oSheet.Cells[1, 10] = "Clase Mov. Act. Fijo";
                        oSheet.Cells[Contador, 10] = "/";
                        //importe
                        oSheet.Cells[1, 11] = "Importe";
                        oSheet.Cells[Contador, 11] = Ordenes_Salida[0]["TOTAL"].ToString().Trim();
                        Importe = Convert.ToDouble(Ordenes_Salida[0]["TOTAL"].ToString().Trim());
                        Suma_Importes += Importe;
                        //cantidad de activos
                        oSheet.Cells[1, 12] = "Cantidad de Activos";
                        oSheet.Cells[Contador, 12] = "/";
                        //Unidad Resp.
                        oSheet.Cells[1, 13] = "Uni. Resp.";
                        oSheet.Cells[Contador, 13] = Codigo.Split(ch)[3].Trim();
                        //Área Funcional
                        oSheet.Cells[1, 14] = "Area Funcional";
                        oSheet.Cells[Contador, 14] = "/";
                        //Partida
                        oSheet.Cells[1, 15] = "Partida";
                        oSheet.Cells[Contador, 15] = "/";
                        //Elemnto PEP
                        oSheet.Cells[1, 16] = "Elemento PEP";
                        oSheet.Cells[Contador, 16] = PEP;
                        //Fondo
                        oSheet.Cells[1, 17] = "Fondo";
                        oSheet.Cells[Contador, 17] = Codigo.Split(ch)[0].Trim();
                        //No. de Orden
                        oSheet.Cells[1, 18] = "No. Orden";
                        oSheet.Cells[Contador, 18] = "/";
                        //No. reserva
                        oSheet.Cells[1, 19] = "No. Reserva";
                        oSheet.Cells[Contador, 19] = "/";
                        //Posicion de Reserva
                        oSheet.Cells[1, 20] = "Pos. Reserva";
                        oSheet.Cells[Contador, 20] = "/";
                        //División(Ramo)
                        oSheet.Cells[1, 21] = "División (Ramo)";
                        oSheet.Cells[Contador, 21] = "M150";
                        //fecha base
                        oSheet.Cells[1, 22] = "Fecha Base";
                        oSheet.Cells[Contador, 22] = "/";
                        //Vía Pago
                        oSheet.Cells[1, 23] = "Vía Pago";
                        oSheet.Cells[Contador, 23] = "/";
                        //Asignación
                        oSheet.Cells[1, 24] = "Asignación";
                        oSheet.Cells[Contador, 24] = "REGISTRO SALIDAS ALMACEN";
                        //texto de posición
                        oSheet.Cells[1, 25] = "Texto de posición";
                        oSheet.Cells[Contador, 25] = "SALIDAS ALM ";
                        oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, 1]).Cells.EntireRow.Interior.ColorIndex = 15;
                        oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1,1]).Cells.EntireRow.Font.Bold = true;
                    }
                    else
                    {
                        //Sociedad
                        oSheet.Cells[Contador, 1] = "/";
                        //Fecha
                        oSheet.Cells[Contador, 2] = "/";
                        //Periodo
                        oSheet.Cells[Contador, 3] = "/";
                        //Clase Docto
                        oSheet.Cells[Contador, 4] = "/";
                        //Referencia
                        oSheet.Cells[Contador, 5] = "/";
                        //Texto de cabecera
                        oSheet.Cells[Contador, 6] = "/";
                        //Clave
                        oSheet.Cells[Contador, 7] = "40";
                        //Cuenta
                        oSheet.Cells[Contador, 8] = Ordenes_Salida[0]["CUENTA"].ToString().Trim();
                        //CME
                        oSheet.Cells[Contador, 9] = "/";
                        //Clase Mov Act. Fijo
                        oSheet.Cells[Contador, 10] = "/";
                        //importe
                        oSheet.Cells[Contador, 11] = Ordenes_Salida[0]["TOTAL"].ToString().Trim();
                        Importe = Convert.ToDouble(Ordenes_Salida[0]["TOTAL"].ToString().Trim());
                        Suma_Importes += Importe;
                        //cantidad de activos
                        oSheet.Cells[Contador, 12] = "/";
                        //Unidad Resp.
                        oSheet.Cells[Contador, 13] = Codigo.Split(ch)[3].Trim();
                        //Área Funcional
                        oSheet.Cells[Contador, 14] = "/";
                        //Partida
                        oSheet.Cells[Contador, 15] = "/";
                        //Elemnto PEP
                        oSheet.Cells[Contador, 16] = PEP;
                        //Fondo
                        oSheet.Cells[Contador, 17] = Codigo.Split(ch)[0].Trim();
                        //No. de Orden
                        oSheet.Cells[Contador, 18] = "/";
                        //No. reserva
                        oSheet.Cells[Contador, 19] = "/";
                        //Posicion de Reserva
                        oSheet.Cells[Contador, 20] = "/";
                        //División(Ramo)
                        oSheet.Cells[Contador, 21] = "M150";
                        //fecha base
                        oSheet.Cells[Contador, 22] = "/";
                        //Vía Pago
                        oSheet.Cells[Contador, 23] = "/";
                        //Asignación
                        oSheet.Cells[Contador, 24] = "REGISTRO SALIDAS ALMACEN";
                        //texto de posición
                        oSheet.Cells[Contador, 25] = "SALIDAS ALM ";                    
                    }
                }
            }//fin de for
            Contador++;
            //Sociedad
            oSheet.Cells[Contador, 1] = "/";
            //Fecha
            oSheet.Cells[Contador, 2] = "/";
            //Periodo
            oSheet.Cells[Contador, 3] = "/";
            //Clase Docto
            oSheet.Cells[Contador, 4] = "/";
            //Referencia
            oSheet.Cells[Contador, 5] = "/";
            //Texto de cabecera
            oSheet.Cells[Contador, 6] = "/";
            //Clave
            oSheet.Cells[Contador, 7] = "50";
            //Cuenta
            oSheet.Cells[Contador, 8] = "115110001";
            //CME
            oSheet.Cells[Contador, 9] = "/";
            //Clase Mov Act. Fijo
            oSheet.Cells[Contador, 10] = "/";
            //importe
            oSheet.Cells[Contador, 11] = Suma_Importes + "";
            //cantidad de activos
            oSheet.Cells[Contador, 12] = "/";
            //Unidad Resp.
            oSheet.Cells[Contador, 13] = "/";
            //Área Funcional
            oSheet.Cells[Contador, 14] = "/";
            //Partida
            oSheet.Cells[Contador, 15] = "/";
            //Elemnto PEP
            oSheet.Cells[Contador, 16] = "/";
            //Fondo
            oSheet.Cells[Contador, 17] = "/";
            //No. de Orden
            oSheet.Cells[Contador, 18] = "/";
            //No. reserva
            oSheet.Cells[Contador, 19] = "/";
            //Posicion de Reserva
            oSheet.Cells[Contador, 20] = "/";
            //División(Ramo)
            oSheet.Cells[Contador, 21] = "M150";
            //fecha base
            oSheet.Cells[Contador, 22] = "/";
            //Vía Pago
            oSheet.Cells[Contador, 23] = "/";
            //Asignación
            oSheet.Cells[Contador, 24] = "REGISTRO SALIDAS ALMACEN";
            //texto de posición
            oSheet.Cells[Contador, 25] = "SALIDAS ALM "; 
            //Contador++;
            for (int r = 2; r <= Contador; r++ )
            {
                for (int c = 26; c <= 49; c++)
                {
                    oSheet.Cells[r, c] = "/";
                }
            }
        }
        catch (Exception Ex)
        {            
            throw new Exception(Ex.ToString());
        }
        return Requisas_Seleccionadas;
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Salidas();
    }
    protected void Btn_Generar_Poliza_Click(object sender, EventArgs e)
    {
        if (Cmb_Contabilizada.SelectedValue == "NO")
        {
            int indice = 0;
            bool Seleccionado = false;
            foreach (GridViewRow Renglon_Grid in Grid_Salidas.Rows)
            {
                Grid_Salidas.SelectedIndex = indice;
                Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Salida")).Checked;
                indice++;
                if (Seleccionado)
                    break;
            }
            if (Seleccionado)
            {
                Hdn_Lista_Salidas.Value = Verifica_Salidas_Seleccionadas2();
                
            }
            else
            {
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "Información", "alert('Debe seleccionar salidas de stock');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "Información", "alert('Las salidas ya fueron registradas');", true);        
        }

    }


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Verifica_Salidas_Seleccionadas
    //  DESCRIPCIÓN: 
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 24 SEP
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public String Verifica_Salidas_Seleccionadas2()
    {
        String Requisas_Seleccionadas = "";
        int Contador = 0;
        double Suma_Importes = 0;
        double Importe = 0;
        //try
        //{
            //####################
            Workbook book = new Workbook();
            WorksheetStyle Hstyle = book.Styles.Add("Encabezado");
            WorksheetStyle Dstyle = book.Styles.Add("Detalles");
            Worksheet sheet = book.Worksheets.Add("Salidas_Stock");
            WorksheetRow row;
        
            String Nombre_Archivo = "Salidas_Stock.xls";
            String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.Properties.Author = "Municipio Irapuato SIAS";

            book.Properties.Created = DateTime.Now;
            //        'Estilo de la hoja de encabezado
            Hstyle.Font.FontName = "Arial";
            Hstyle.Font.Size = 10;
            Hstyle.Font.Bold = true;
            Hstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Hstyle.Font.Color = "Black";
            //Estilo de la hoja de detalles
            Dstyle.Font.FontName = "Arial";
            Dstyle.Font.Size = 10;
            Dstyle.Font.Color = "Black";
            //###################


            //Crea el nuevo renglon del encabezado del documento
            
            row = sheet.Table.Rows.Add();
            //Sociedad
            row.Cells.Add(new WorksheetCell("Sociedad", "Encabezado"));
            //Fecha
            row.Cells.Add(new WorksheetCell("Fecha", "Encabezado"));
            //Periodo
            row.Cells.Add(new WorksheetCell("Periodo", "Encabezado"));
            //Clase Docto
            row.Cells.Add(new WorksheetCell("Clase Docto", "Encabezado"));
            //Referencia
            row.Cells.Add(new WorksheetCell("Referencia", "Encabezado"));
            //Texto de cabecera
            row.Cells.Add(new WorksheetCell("Texto de Cabecera", "Encabezado"));
            //Clave
            row.Cells.Add(new WorksheetCell("Clave", "Encabezado"));
            //Cuenta
            row.Cells.Add(new WorksheetCell("Cuenta", "Encabezado"));
            //CME
            row.Cells.Add(new WorksheetCell("CME", "Encabezado"));
            //Clase Mov Act. Fijo
            row.Cells.Add(new WorksheetCell("Clase Mov. Act. Fijo", "Encabezado"));
            //importe
            row.Cells.Add(new WorksheetCell("Importe", "Encabezado"));
            //cantidad de activos
            row.Cells.Add(new WorksheetCell("Cantidad de Activos", "Encabezado"));
            //Unidad Resp.
            row.Cells.Add(new WorksheetCell("Uni. Resp.", "Encabezado"));
            //Área Funcional
            row.Cells.Add(new WorksheetCell("Area Funcional", "Encabezado"));
            //Partida
            row.Cells.Add(new WorksheetCell("Partida", "Encabezado"));
            //Elemnto PEP
            row.Cells.Add(new WorksheetCell("Elemento PEP", "Encabezado"));
            //Fondo
            row.Cells.Add(new WorksheetCell("Fondo", "Encabezado"));
            //No. de Orden
            row.Cells.Add(new WorksheetCell("No. Orden", "Encabezado"));
            //No. reserva
            row.Cells.Add(new WorksheetCell("No. Reserva", "Encabezado"));
            //Posicion de Reserva
            row.Cells.Add(new WorksheetCell("Pos. Reserva", "Encabezado"));
            //División(Ramo)
            row.Cells.Add(new WorksheetCell("División (Ramo)", "Encabezado"));
            //fecha base
            row.Cells.Add(new WorksheetCell("Fecha Base", "Encabezado"));
            //Vía Pago
            row.Cells.Add(new WorksheetCell("Vía Pago", "Encabezado"));
            //Asignación
            row.Cells.Add(new WorksheetCell("Asignación", "Encabezado"));
            //texto de posición
            row.Cells.Add(new WorksheetCell("Texto de posición", "Encabezado"));
            int Periodo = DateTime.Now.Month;
            Contador = 0;
            int indice = -1;
            foreach (GridViewRow Renglon_Grid in Grid_Salidas.Rows)
            {
                //Crea el nuevo renglon del encabezado del documento               
                indice++;
                Grid_Salidas.SelectedIndex = indice;
                bool Seleccionado = ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Salida")).Checked;
                if (Seleccionado)
                {
                    Requisas_Seleccionadas += Renglon_Grid.Cells[1].Text + ",";
                    row = sheet.Table.Rows.Add();
                    Contador++;
                    DataTable Dt_Salidas = Session[P_Salidas_Stock] as DataTable;
                    String No_Salida = Grid_Salidas.SelectedDataKey["NO_SALIDA"].ToString();
                    DataRow[] Ordenes_Salida = Dt_Salidas.Select("NO_SALIDA ='" + No_Salida + "'");
                    String Codigo = Ordenes_Salida[0]["CODIGO_PROGRAMATICO"].ToString().Trim();
                    String PEP = Ordenes_Salida[0]["ELEMENTO_PEP"].ToString().Trim();
                    char[] ch = { '-' };
                    if (Contador == 1)
                    {
                        //Sociedad
                        row.Cells.Add(new WorksheetCell("M15", "Detalles"));
                        //Fecha
                        row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("ddMMyyy"), "Detalles"));
                        //Periodo
                        row.Cells.Add(new WorksheetCell(Periodo.ToString(), "Detalles"));
                        //Clase Docto
                        row.Cells.Add(new WorksheetCell("SA", "Detalles"));
                        //Referencia
                        row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
                        //Texto de cabecera
                        row.Cells.Add(new WorksheetCell("SALIDAS ALM ", "Detalles"));
                        //Cave
                        row.Cells.Add(new WorksheetCell("40", "Detalles"));
                        //Cuenta
                        row.Cells.Add(new WorksheetCell(Ordenes_Salida[0]["CUENTA"].ToString().Trim(), "Detalles"));
                        //CME
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Clase Mov Act. Fijo
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //importe
                        row.Cells.Add(new WorksheetCell(Ordenes_Salida[0]["TOTAL"].ToString().Trim(), "Detalles"));
                        Importe = Convert.ToDouble(Ordenes_Salida[0]["TOTAL"].ToString().Trim());
                        Suma_Importes += Importe;
                        //cantidad de activos
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Unidad Resp.
                        //row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[6].Trim(), "Detalles"));
                        row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[4].Trim(), "Detalles"));
                        //Área Funcional
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Partida
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Elemnto PEP
                        row.Cells.Add(new WorksheetCell(PEP, "Detalles"));
                        //Fondo
                        row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[0].Trim() + "-" + Codigo.Split(ch)[1].Trim(), "Detalles"));
                        //No. de Orden
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //No. reserva
                        row.Cells.Add(new WorksheetCell(Ordenes_Salida[0]["NO_RESERVA"].ToString().Trim(), "Detalles"));
                        //Posicion de Reserva
                        row.Cells.Add(new WorksheetCell("1", "Detalles"));
                        //División(Ramo)
                        row.Cells.Add(new WorksheetCell("M150", "Detalles"));
                        //fecha base
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Vía Pago
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Asignación
                        row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
                        //texto de posición
                        row.Cells.Add(new WorksheetCell("SALIDAS ALM", "Detalles"));

                        for (int i = 0; i < 24; i++)
                        {
                            row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        }

                    }
                    else
                    {
                        //Sociedad
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Fecha
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Periodo
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Clase Docto
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Referencia
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Texto de cabecera
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Clave
                        row.Cells.Add(new WorksheetCell("40", "Detalles"));
                        //Cuenta
                        row.Cells.Add(new WorksheetCell(Ordenes_Salida[0]["CUENTA"].ToString().Trim(), "Detalles"));
                        //CME
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Clase Mov Act. Fijo
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //importe
                        row.Cells.Add(new WorksheetCell(Ordenes_Salida[0]["TOTAL"].ToString().Trim(), "Detalles"));
                        Importe = Convert.ToDouble(Ordenes_Salida[0]["TOTAL"].ToString().Trim());
                        Suma_Importes += Importe;
                        //cantidad de activos
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Unidad Resp.
                        //row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[3].Trim(), "Detalles"));
                        row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[4].Trim(), "Detalles"));
                        //Área Funcional
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Partida
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Elemnto PEP
                        row.Cells.Add(new WorksheetCell(PEP, "Detalles"));
                        //Fondo
                        row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[0].Trim(), "Detalles"));
                        //No. de Orden
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //No. reserva
                        row.Cells.Add(new WorksheetCell(Ordenes_Salida[0]["NO_RESERVA"].ToString().Trim(), "Detalles"));
                        //Posicion de Reserva
                        row.Cells.Add(new WorksheetCell("1", "Detalles"));
                        //División(Ramo)
                        row.Cells.Add(new WorksheetCell("M150", "Detalles"));
                        //fecha base
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Vía Pago
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        //Asignación
                        row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
                        //texto de posición
                        row.Cells.Add(new WorksheetCell("SALIDAS ALM", "Detalles"));
                        for (int i = 0; i < 24; i++)
                        {
                            row.Cells.Add(new WorksheetCell("/", "Detalles"));
                        }
                    }//fin de else
                }
            }//fin de for
            //Contador++;
            row = sheet.Table.Rows.Add();
            //Sociedad
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Fecha
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Periodo
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Clase Docto
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Referencia
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Texto de cabecera
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Clave
            row.Cells.Add(new WorksheetCell("50", "Detalles"));
            //Cuenta
            row.Cells.Add(new WorksheetCell("115110001", "Detalles"));
            //CME
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Clase Mov Act. Fijo
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //importe
            row.Cells.Add(new WorksheetCell( Suma_Importes.ToString(), "Detalles"));
            //cantidad de activos
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Unidad Resp.
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Área Funcional
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Partida
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Elemnto PEP
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Fondo
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //No. de Orden
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //No. reserva
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Posicion de Reserva
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //División(Ramo)
            row.Cells.Add(new WorksheetCell("M150", "Detalles"));
            //fecha base
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Vía Pago
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
            //Asignación
            row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
            //texto de posición
            row.Cells.Add(new WorksheetCell("SALIDAS ALM", "Detalles"));
            Hdn_Importe.Value = Suma_Importes.ToString();
            if (Requisas_Seleccionadas.Length > 0)
            {
                Requisas_Seleccionadas = Requisas_Seleccionadas.Substring(0, Requisas_Seleccionadas.Length - 1);
            }
            Hdn_Lista_Salidas.Value = Requisas_Seleccionadas; 
            for (int i = 0; i < 24; i++)
            {
                row.Cells.Add(new WorksheetCell("/", "Detalles"));
            }
            book.Save(Ruta_Archivo);
            //Guardar Póliza
            int registros = Guardar_Poliza();
            Llenar_Grid_Salidas();
        return Requisas_Seleccionadas;
    }
    //protected void Mostrar_Archivo()
    //{
    //    String Pagina = "../Archivo/Salidas_Stock.xls";

    //    try
    //    {
    //        Pagina = Pagina + "";
    //        ScriptManager.RegisterStartupScript(this, this.GetType(), "Window",
    //            "window.open('" + Pagina + "', 'Requisición','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    //    }
    //    catch (Exception Ex)
    //    {
    //        throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
    //    }
    //}

    public void Abrir_Archivo(String Ruta_Archivo) 
    {
     
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();

        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta_Archivo);
        //           'Visualiza el archivo
        Response.WriteFile(Ruta_Archivo);
        Response.Flush();
        Response.Close();
    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        Abrir_Archivo("C:/SIAS/Archivos/Salidas_Stock.xls");
    }
    protected void Chk_Seleccionar_Todo_CheckedChanged(object sender, EventArgs e)
    {
        int indice = 0;
        if (Chk_Seleccionar_Todo.Checked)
        {
            indice = 0;
            foreach (GridViewRow Renglon_Grid in Grid_Salidas.Rows)
            {                
                Grid_Salidas.SelectedIndex = indice;
                ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Salida")).Checked = true;
                indice++;
            }
        }
        else 
        {
            indice = 0;
            foreach (GridViewRow Renglon_Grid in Grid_Salidas.Rows)
            {
                Grid_Salidas.SelectedIndex = indice;
                ((System.Web.UI.WebControls.CheckBox)Renglon_Grid.FindControl("Chk_Salida")).Checked = false;
                indice++;
            }
        }
    }
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        if (Hdn_Lista_Salidas.Value != "")
        {            
            Guardar_Poliza();
            Hdn_Lista_Salidas.Value = "";
            Hdn_Importe.Value = "";
        }
        else 
        {
            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "Información", "alert('Debe generar una póliza');", true);        
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Div_Listado_Salidas.Visible = true;
            Div_Polizas_Stock_Sap.Visible = false;
            Btn_Salir.ToolTip = "Inicio";
        }
    }
    private int Guardar_Poliza()
    {
        Cls_Ope_Com_Polizas_Stock_Negocio Negocio = new Cls_Ope_Com_Polizas_Stock_Negocio();
        Negocio.P_Lista_Salidas = Hdn_Lista_Salidas.Value;
        Negocio.P_Importe = Hdn_Importe.Value;
        int Registros_Afectados = Negocio.Guardar_Poliza_SAP_Stock();
        
        if (Registros_Afectados > 0)
        {
            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "Información", "alert('Póliza guardada');", true);
            Lnk_Mostrar.Visible = true;
        }
        else 
        {
            ScriptManager.RegisterStartupScript(
                this, this.GetType(), "Información", "alert('No se pudo guardar la póliza');", true);
            Lnk_Mostrar.Visible = false;
        }
        return Registros_Afectados;
    }
    //private void Consultar_Polizas()     
    //{
    //    Cls_Ope_Com_Polizas_Stock_Negocio Negocio = new Cls_Ope_Com_Polizas_Stock_Negocio();       
    //    Negocio.Consultar_Ordenes_Salida_Stock();
    //}
    protected void Lnk_Mostrar_Click(object sender, EventArgs e)
    {
        String Nombre_Archivo = "Salidas_Stock.xls";
        String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
        Response.Clear();
        Response.ClearContent();
        Response.ClearHeaders();

        Response.ContentType = "application/x-msexcel";
        Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta_Archivo);
        //           'Visualiza el archivo
        Response.WriteFile(Ruta_Archivo);
        Response.Flush();
        Response.Close();
        Lnk_Mostrar.Visible = false;
    }
    protected void Cmb_Contabilizada_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Grid_Salidas();
    }
    protected void Lnk_Buscar_Polizas_Anteriores_Click(object sender, EventArgs e)
    {
        Div_Listado_Salidas.Visible = false;
        Div_Polizas_Stock_Sap.Visible = true;
        Btn_Salir.ToolTip = "Regresar";
        Llenar_Grid_Polizas_Sap();
    }
    protected void Btn_Buscar_Polizas_Stock_Sap_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Polizas_Sap();
    }

    private void Hacer_Documento_Excel(DataTable Dt_Salidas, String Periodo) 
    {
        String Requisas_Seleccionadas = "";
        int Contador = 0;
        double Suma_Importes = 0;
        double Importe = 0;
        try
        {
        //####################
        Workbook book = new Workbook();
        WorksheetStyle Hstyle = book.Styles.Add("Encabezado");
        WorksheetStyle Dstyle = book.Styles.Add("Detalles");
        Worksheet sheet = book.Worksheets.Add("Salidas_Stock");
        WorksheetRow row;

        String Nombre_Archivo = "Salidas_Stock.xls";
        String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
        book.ExcelWorkbook.ActiveSheetIndex = 1;
        book.Properties.Author = "Municipio Irapuato SIAS";

        book.Properties.Created = DateTime.Now;
        //        'Estilo de la hoja de encabezado
        Hstyle.Font.FontName = "Arial";
        Hstyle.Font.Size = 10;
        Hstyle.Font.Bold = true;
        Hstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        Hstyle.Font.Color = "Black";
        //Estilo de la hoja de detalles
        Dstyle.Font.FontName = "Arial";
        Dstyle.Font.Size = 10;
        Dstyle.Font.Color = "Black";
        //###################

        //Crea el nuevo renglon del encabezado del documento

        row = sheet.Table.Rows.Add();
        //Sociedad
        row.Cells.Add(new WorksheetCell("Sociedad", "Encabezado"));
        //Fecha
        row.Cells.Add(new WorksheetCell("Fecha", "Encabezado"));
        //Periodo
        row.Cells.Add(new WorksheetCell("Periodo", "Encabezado"));
        //Clase Docto
        row.Cells.Add(new WorksheetCell("Clase Docto", "Encabezado"));
        //Referencia
        row.Cells.Add(new WorksheetCell("Referencia", "Encabezado"));
        //Texto de cabecera
        row.Cells.Add(new WorksheetCell("Texto de Cabecera", "Encabezado"));
        //Clave
        row.Cells.Add(new WorksheetCell("Clave", "Encabezado"));
        //Cuenta
        row.Cells.Add(new WorksheetCell("Cuenta", "Encabezado"));
        //CME
        row.Cells.Add(new WorksheetCell("CME", "Encabezado"));
        //Clase Mov Act. Fijo
        row.Cells.Add(new WorksheetCell("Clase Mov. Act. Fijo", "Encabezado"));
        //importe
        row.Cells.Add(new WorksheetCell("Importe", "Encabezado"));
        //cantidad de activos
        row.Cells.Add(new WorksheetCell("Cantidad de Activos", "Encabezado"));
        //Unidad Resp.
        row.Cells.Add(new WorksheetCell("Uni. Resp.", "Encabezado"));
        //Área Funcional
        row.Cells.Add(new WorksheetCell("Area Funcional", "Encabezado"));
        //Partida
        row.Cells.Add(new WorksheetCell("Partida", "Encabezado"));
        //Elemnto PEP
        row.Cells.Add(new WorksheetCell("Elemento PEP", "Encabezado"));
        //Fondo
        row.Cells.Add(new WorksheetCell("Fondo", "Encabezado"));
        //No. de Orden
        row.Cells.Add(new WorksheetCell("No. Orden", "Encabezado"));
        //No. reserva
        row.Cells.Add(new WorksheetCell("No. Reserva", "Encabezado"));
        //Posicion de Reserva
        row.Cells.Add(new WorksheetCell("Pos. Reserva", "Encabezado"));
        //División(Ramo)
        row.Cells.Add(new WorksheetCell("División (Ramo)", "Encabezado"));
        //fecha base
        row.Cells.Add(new WorksheetCell("Fecha Base", "Encabezado"));
        //Vía Pago
        row.Cells.Add(new WorksheetCell("Vía Pago", "Encabezado"));
        //Asignación
        row.Cells.Add(new WorksheetCell("Asignación", "Encabezado"));
        //texto de posición
        row.Cells.Add(new WorksheetCell("Texto de posición", "Encabezado"));
        //int Periodo = DateTime.Now.Month;
        Contador = 0;
        //        int indice = -1;
        foreach (DataRow Salida in Dt_Salidas.Rows)
        {
            //Crea el nuevo renglon del encabezado del documento               
            if (true)
            {
                row = sheet.Table.Rows.Add();
                Contador++;
                String Codigo = Salida["CODIGO_PROGRAMATICO"].ToString().Trim();
                String PEP = Salida["ELEMENTO_PEP"].ToString().Trim();
                char[] ch = { '-' };
                if (Contador == 1)
                {
                    //Sociedad
                    row.Cells.Add(new WorksheetCell("M15", "Detalles"));
                    //Fecha
                    row.Cells.Add(new WorksheetCell(DateTime.Now.ToString("ddMMyyy"), "Detalles"));
                    //Periodo
                    row.Cells.Add(new WorksheetCell(Periodo, "Detalles"));
                    //Clase Docto
                    row.Cells.Add(new WorksheetCell("SA", "Detalles"));
                    //Referencia
                    row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
                    //Texto de cabecera
                    row.Cells.Add(new WorksheetCell("SALIDAS ALM ", "Detalles"));
                    //Cave
                    row.Cells.Add(new WorksheetCell("40", "Detalles"));
                    //Cuenta
                    row.Cells.Add(new WorksheetCell(Salida["CUENTA"].ToString().Trim(), "Detalles"));
                    //CME
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Clase Mov Act. Fijo
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //importe
                    row.Cells.Add(new WorksheetCell(Salida["TOTAL"].ToString().Trim(), "Detalles"));
                    Importe = Convert.ToDouble(Salida["TOTAL"].ToString().Trim());
                    Suma_Importes += Importe;
                    //cantidad de activos
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Unidad Resp.
                    row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[3].Trim(), "Detalles"));
                    //Área Funcional
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Partida
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Elemnto PEP
                    row.Cells.Add(new WorksheetCell(PEP, "Detalles"));
                    //Fondo
                    row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[0].Trim(), "Detalles"));
                    //No. de Orden
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //No. reserva
                    row.Cells.Add(new WorksheetCell(Salida["NO_RESERVA"].ToString().Trim(), "Detalles"));
                    //Posicion de Reserva
                    row.Cells.Add(new WorksheetCell("1", "Detalles"));
                    //División(Ramo)
                    row.Cells.Add(new WorksheetCell("M150", "Detalles"));
                    //fecha base
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Vía Pago
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Asignación
                    row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
                    //texto de posición
                    row.Cells.Add(new WorksheetCell("SALIDAS ALM", "Detalles"));

                    for (int i = 0; i < 24; i++)
                    {
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    }
                }
                else
                {
                    //Sociedad
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Fecha
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Periodo
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Clase Docto
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Referencia
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Texto de cabecera
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Clave
                    row.Cells.Add(new WorksheetCell("40", "Detalles"));
                    //Cuenta
                    row.Cells.Add(new WorksheetCell(Salida["CUENTA"].ToString().Trim(), "Detalles"));
                    //CME
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Clase Mov Act. Fijo
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //importe
                    row.Cells.Add(new WorksheetCell(Salida["TOTAL"].ToString().Trim(), "Detalles"));
                    Importe = Convert.ToDouble(Salida["TOTAL"].ToString().Trim());
                    Suma_Importes += Importe;
                    //cantidad de activos
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Unidad Resp.
                    row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[3].Trim(), "Detalles"));
                    //Área Funcional
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Partida
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Elemnto PEP
                    row.Cells.Add(new WorksheetCell(PEP, "Detalles"));
                    //Fondo
                    row.Cells.Add(new WorksheetCell(Codigo.Split(ch)[0].Trim(), "Detalles"));
                    //No. de Orden
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //No. reserva
                    row.Cells.Add(new WorksheetCell(Salida["NO_RESERVA"].ToString().Trim(), "Detalles"));
                    //Posicion de Reserva
                    row.Cells.Add(new WorksheetCell("1", "Detalles"));
                    //División(Ramo)
                    row.Cells.Add(new WorksheetCell("M150", "Detalles"));
                    //fecha base
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Vía Pago
                    row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    //Asignación
                    row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
                    //texto de posición
                    row.Cells.Add(new WorksheetCell("SALIDAS ALM", "Detalles"));
                    for (int i = 0; i < 24; i++)
                    {
                        row.Cells.Add(new WorksheetCell("/", "Detalles"));
                    }
                }//fin de else
            }
        }//fin de for
        //Contador++;
        row = sheet.Table.Rows.Add();
        //Sociedad
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Fecha
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Periodo
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Clase Docto
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Referencia
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Texto de cabecera
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Clave
        row.Cells.Add(new WorksheetCell("50", "Detalles"));
        //Cuenta
        row.Cells.Add(new WorksheetCell("115110001", "Detalles"));
        //CME
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Clase Mov Act. Fijo
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //importe
        row.Cells.Add(new WorksheetCell(Suma_Importes.ToString(), "Detalles"));
        //cantidad de activos
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Unidad Resp.
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Área Funcional
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Partida
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Elemnto PEP
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Fondo
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //No. de Orden
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //No. reserva
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Posicion de Reserva
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //División(Ramo)
        row.Cells.Add(new WorksheetCell("M150", "Detalles"));
        //fecha base
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Vía Pago
        row.Cells.Add(new WorksheetCell("/", "Detalles"));
        //Asignación
        row.Cells.Add(new WorksheetCell("REGISTRO SALIDAS ALMACEN", "Detalles"));
        //texto de posición
        row.Cells.Add(new WorksheetCell("SALIDAS ALM", "Detalles"));
        Hdn_Importe.Value = Suma_Importes.ToString();
        if (Requisas_Seleccionadas.Length > 0)
        {
            Requisas_Seleccionadas = Requisas_Seleccionadas.Substring(0, Requisas_Seleccionadas.Length - 1);
        }
        Hdn_Lista_Salidas.Value = Requisas_Seleccionadas;
        for (int i = 0; i < 24; i++)
        {
            row.Cells.Add(new WorksheetCell("/", "Detalles"));
        }
        book.Save(Ruta_Archivo);
        //Guardar Póliza
        int registros = Guardar_Poliza();
        Abrir_Archivo(Ruta_Archivo);
        }
        catch (Exception Ex)
        {

            System.Windows.Forms.MessageBox.Show(Ex.ToString());
        }
    }
    protected void Btn_Imprimir_Requisicion_Click(object sender, ImageClickEventArgs e)
    {
        String No_Poliza = ((ImageButton)sender).CommandArgument;
        Cls_Ope_Com_Polizas_Stock_Negocio Negocio = new Cls_Ope_Com_Polizas_Stock_Negocio();
        Negocio.P_No_Poliza_Stock = No_Poliza;
        DataTable Dt_Polizas = Session[P_Polizas_Stock_Sap] as DataTable;
        String Periodo = (Convert.ToDateTime(Dt_Polizas.Rows[0]["FECHA_CREO"].ToString())).Year.ToString();
        DataTable Dt_Salidas = Negocio.Consultar_Ordenes_Salida_Stock_Por_Poliza_Generada();
        Hacer_Documento_Excel(Dt_Salidas,Periodo);
    }    
}
