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
using Presidencia.Reportes_Inventarios_Stock.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Reportes;
using Presidencia.Almacen_Generar_Kardex_Productos.Negocio;
using CarlosAg.ExcelXmlWriter;

using SharpContent.ApplicationBlocks.Data;
using System.Data.OracleClient;
using Presidencia.Inventarios_De_Stock.Negocio;


using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;


public partial class paginas_Almacen_Frm_Ope_Alm_Reportes_Inventarios_Stock : System.Web.UI.Page
{
    #region Variables

    Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio Consulta_Inventarios = new Cls_Ope_Com_Alm_Rpts_Inventarios_Stock_Negocio();

    #endregion

    #region Load

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Estatus_Inicial_Componentes();
        }
    }

    #endregion

    #region Metodos

    private void Cargar_Partidas()
    {
        Cls_Ope_Alm_Inventarios_Stock_Negocio Negocio = new Cls_Ope_Alm_Inventarios_Stock_Negocio();
        DataTable Dt_Partidas = Negocio.Consultar_Partidas_Stock();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partidas_STOCK, Dt_Partidas, 1, 0);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Inicial_Componentes
    ///DESCRIPCIÓN:          Método utilizado para asignar propiedades a los componentes
    ///                      Iniciales
    ///PARAMETROS:  
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estatus_Inicial_Componentes()
    {
        Txt_Fecha_Inicial.Text = "";
        Txt_Fecha_Final.Text = "";
        Btn_Fecha_Final.Enabled = true;
        Btn_Fecha_Inicial.Enabled = true;
        //llenar combo PArtidas 
        Cargar_Partidas();
    }
    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN:          Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///                      en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
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
           
                if ((Txt_Fecha_Inicial.Text.Length != 0))
                {
                        // Convertimos el Texto de los TextBox fecha a dateTime
                        Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
                        Date2 = DateTime.Parse(Txt_Fecha_Final.Text);
                        
                        //Validamos que las fechas sean iguales o la final sea mayor que la inicial, de lo contrario se manda un mensaje de error 
                        if ((Date1 < Date2) | (Date1 == Date2))
                        {
                                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                                Consulta_Inventarios.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text.Trim());
                                Consulta_Inventarios.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final.Text.Trim());
                                Fecha_Valida = true;
                        }
                        else
                        {
                            Lbl_Informacion.Text = " La fecha inicial no pude ser mayor que la fecha final <br />";
                            Fecha_Valida = false;
                        }
                  
            }
            return Fecha_Valida;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Validar_Fechas()
    {
        bool Correcto = false;
        if (Txt_Fecha_Inicial.Text.Trim() != String.Empty || Txt_Fecha_Final.Text.Trim() != String.Empty)
        {
            Correcto = true;
        }
        return Correcto;

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN:          Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:           1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                      en caso de que cumpla la condicion del if
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {
        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica a mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }



    

     private int Sumar_Movimientos(DataTable Dt_Tabla, String Prooducto_ID) 
        {
            int Suma = 0;
            int Cantidad = 0;
            foreach(DataRow Renglon in Dt_Tabla.Rows)
            {
                if (Renglon["PRODUCTO_ID"].ToString().Trim() == Prooducto_ID)
                {
                    Cantidad = Convert.IsDBNull(Renglon["CANTIDAD"]) ? 0: Convert.ToInt32(Renglon["CANTIDAD"]);                                  
                    Suma += Cantidad;
                }
            }
            return Suma; 
        }


    private DataTable Consultar_Mi_Kardex_Inicial()
        {           
            Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();

            DateTime Fecha = DateTime.Now.AddYears(-20);
            Kardex_negocio.P_Fecha_I = string.Format("{0:dd/MM/yyyy}", Fecha);
            Kardex_negocio.P_Fecha_F = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim()));
           
            if (Cmb_Partidas_STOCK.SelectedValue != "0")
            {
                Kardex_negocio.P_Partida_ID = Cmb_Partidas_STOCK.SelectedValue;
            }

            //CONSLTAR KARDEX ANTES DE LA FECHA INICIAL
            DataTable Dt_Kardex = Kardex_negocio.Consultar_Kardex_Actualizado();
            DataTable Dt_Entradas = Kardex_negocio.Consultar_Entradas();
            DataTable Dt_Entradas_Ajuste = Kardex_negocio.Consultar_Entradas_Ajuste();
            DataTable Dt_Salidas = Kardex_negocio.Consultar_Salidas();
            DataTable Dt_Salidas_Ajuste = Kardex_negocio.Consultar_Salidas_Ajuste();
            DataTable Dt_Comprometidos = Kardex_negocio.Consultar_Compromisos();
            int Inicial = 0;
            int Entrada = 0;
            int Ajuste_Entrada = 0;
            int Salida = 0;
            int Ajuste_Salida = 0;

            foreach (DataRow Producto in Dt_Kardex.Rows)
            {
                Producto["ENTRADA"] = Sumar_Movimientos(Dt_Entradas, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["AJUSTE_ENTRADA"] = Sumar_Movimientos(Dt_Entradas_Ajuste, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["SALIDA"] = Sumar_Movimientos(Dt_Salidas, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["AJUSTE_SALIDA"] = Sumar_Movimientos(Dt_Salidas_Ajuste, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["COMPROMETIDO"] = Sumar_Movimientos(Dt_Comprometidos, Producto["PRODUCTO_ID"].ToString().Trim());
            }

            //Calcular  EXISTENCIA y DISPONIBLE
            foreach (DataRow Renglon in Dt_Kardex.Rows)
            {
                Inicial = Convert.IsDBNull(Renglon["INICIAL"]) ? 0 : Convert.ToInt32(Renglon["INICIAL"]);
                Entrada = Convert.IsDBNull(Renglon["ENTRADA"]) ? 0 : Convert.ToInt32(Renglon["ENTRADA"]);
                Ajuste_Entrada = Convert.IsDBNull(Renglon["AJUSTE_ENTRADA"]) ? 0 : Convert.ToInt32(Renglon["AJUSTE_ENTRADA"]);
                Salida = Convert.IsDBNull(Renglon["SALIDA"]) ? 0 : Convert.ToInt32(Renglon["SALIDA"]);
                Ajuste_Salida = Convert.IsDBNull(Renglon["AJUSTE_SALIDA"]) ? 0 : Convert.ToInt32(Renglon["AJUSTE_SALIDA"]);
                Renglon["EXISTENCIA"] = Inicial +
                                        Entrada +
                                        Ajuste_Entrada -
                                        Salida -
                                        Ajuste_Salida;
                Renglon["INICIAL"] = Renglon["EXISTENCIA"];
            }       
            return Dt_Kardex;
        }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Inventarios_Stock
    ///DESCRIPCIÓN:          Método utilizado para instanciar al los métodos: "Validar_Opciones_Consulta", 
    ///                      "Consultar_Inventarios_Stock", Consultar_Ajustes_Inventario  y Generar_Reporte
    ///PARAMETROS:            
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           17/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Consultar_Inventarios_Stock(String Formato)
    {
        

       

        }

    #endregion

    #region Eventos

    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Excel_Click
    ///DESCRIPCIÓN:          Evento utilizado instanciar el método Consultar_Inventarios_Stock
    ///                      y generar el reporte en formato excel
    ///PARAMETROS:            
    ///                      
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           04/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Informacion.Text = "";

        if (Validar_Fechas())
        {
            //ANTES D ELA FECHA INICIAL
            DataTable Dt_Kardex = Consultar_Mi_Kardex_Inicial();
            //DESPUES DE LA FECHA INICIAL
            Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
            Kardex_negocio.P_Fecha_I = Txt_Fecha_Inicial.Text.Trim();
            Kardex_negocio.P_Fecha_F = Txt_Fecha_Final.Text.Trim();
            Kardex_negocio.P_Fecha_I = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_I));
            Kardex_negocio.P_Fecha_F = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_F));

            if (Cmb_Partidas_STOCK.SelectedValue != "0")
            {
                Kardex_negocio.P_Partida_ID = Cmb_Partidas_STOCK.SelectedValue;
            }
            //CONSULTAR KARDEX PARA SABER SOLO CUALES SON LOS PRODUCTOS DE STOCK
            //Y MODIFICAR LOS DATOS CON LAS CONSULTAS DE ENTRADAS, SAKLIDAS;ETC

            DataTable Dt_Entradas = Kardex_negocio.Consultar_Entradas();
            DataTable Dt_Entradas_Ajuste = Kardex_negocio.Consultar_Entradas_Ajuste();
            DataTable Dt_Salidas = Kardex_negocio.Consultar_Salidas();
            DataTable Dt_Salidas_Ajuste = Kardex_negocio.Consultar_Salidas_Ajuste();
            DataTable Dt_Comprometidos = Kardex_negocio.Consultar_Compromisos();

            foreach (DataRow Producto in Dt_Kardex.Rows)
            {
                Producto["ENTRADA"] = Sumar_Movimientos(Dt_Entradas, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["AJUSTE_ENTRADA"] = Sumar_Movimientos(Dt_Entradas_Ajuste, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["SALIDA"] = Sumar_Movimientos(Dt_Salidas, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["AJUSTE_SALIDA"] = Sumar_Movimientos(Dt_Salidas_Ajuste, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["COMPROMETIDO"] = Sumar_Movimientos(Dt_Comprometidos, Producto["PRODUCTO_ID"].ToString().Trim());
                //CALCULAR  EXISTENCIA y DISPONIBLE

                Producto["EXISTENCIA"] = Convert.ToInt32(Producto["INICIAL"]) +
                                        Convert.ToInt32(Producto["ENTRADA"]) +
                                        Convert.ToInt32(Producto["AJUSTE_ENTRADA"]) -
                                        Convert.ToInt32(Producto["SALIDA"]) -
                                        Convert.ToInt32(Producto["AJUSTE_SALIDA"]);

                Producto["DISPONIBLE"] = Convert.ToInt32(Producto["INICIAL"]) +
                                        Convert.ToInt32(Producto["ENTRADA"]) +
                                        Convert.ToInt32(Producto["AJUSTE_ENTRADA"]) -
                                        Convert.ToInt32(Producto["SALIDA"]) -
                                        Convert.ToInt32(Producto["AJUSTE_SALIDA"]) -
                                        Convert.ToInt32(Producto["COMPROMETIDO"]);

            }




            int[] Arreglo = new int[5];


            WorksheetCell Celda;

            //####################
            Workbook book = new Workbook();
            WorksheetStyle Hstyle = book.Styles.Add("Encabezado");
            WorksheetStyle Dstyle = book.Styles.Add("Detalles");
            WorksheetStyle Celda_Combinada_Style = book.Styles.Add("Celda_Combinada");
            WorksheetStyle Datos_Generales_Style = book.Styles.Add("Datos_Generales");
            WorksheetStyle Titulo_Datos_Generales_Style = book.Styles.Add("Titulo_Datos_Generales");
            WorksheetStyle Encabezado_Principal_Style = book.Styles.Add("Encabezado_Principal");
            Worksheet sheet = book.Worksheets.Add("Kardex");
            WorksheetRow row;

            String Nombre_Archivo = "Inventario.xls";
            String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
            book.ExcelWorkbook.ActiveSheetIndex = 1;
            book.Properties.Author = "Municipio Irapuato SIAG";
            book.Properties.Created = DateTime.Now;

            //Estilo de la hoja de encabezado
            Hstyle.Font.FontName = "Arial";
            Hstyle.Font.Size = 10;
            Hstyle.Font.Bold = true;
            Hstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Hstyle.Font.Color = "Black";
            Hstyle.Interior.Color = "LightGray";
            Hstyle.Interior.Pattern = StyleInteriorPattern.Solid;
            Hstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Hstyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Hstyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Hstyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Hstyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            //Estilo de la hoja de detalles
            Dstyle.Font.FontName = "Arial";
            Dstyle.Font.Size = 10;
            Dstyle.Font.Color = "Black";
            Dstyle.NumberFormat = "###";
            Dstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Dstyle.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Dstyle.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            //Estilo de celda merge
            Celda_Combinada_Style.Font.FontName = "Arial";
            Celda_Combinada_Style.Font.Size = 10;
            Celda_Combinada_Style.Font.Color = "Black";
            Celda_Combinada_Style.Interior.Color = "LightGray";
            Celda_Combinada_Style.Font.Bold = true;
            Celda_Combinada_Style.Interior.Pattern = StyleInteriorPattern.Solid;
            Celda_Combinada_Style.NumberFormat = "###";
            Celda_Combinada_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Celda_Combinada_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Celda_Combinada_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Celda_Combinada_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Celda_Combinada_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //Estilo de Titulo de Datos Generales
            Titulo_Datos_Generales_Style.Font.FontName = "Arial";
            Titulo_Datos_Generales_Style.Font.Size = 10;
            Titulo_Datos_Generales_Style.Font.Color = "Black";
            Titulo_Datos_Generales_Style.Interior.Color = "LightBlue";
            Titulo_Datos_Generales_Style.Font.Bold = true;
            Titulo_Datos_Generales_Style.Interior.Pattern = StyleInteriorPattern.Solid;
            Titulo_Datos_Generales_Style.NumberFormat = "###";
            Titulo_Datos_Generales_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Titulo_Datos_Generales_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //Estilo de Datos_Generales
            Datos_Generales_Style.Font.FontName = "Arial";
            Datos_Generales_Style.Font.Size = 10;
            Datos_Generales_Style.Font.Color = "Black";
            Datos_Generales_Style.NumberFormat = "###";
            Datos_Generales_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
            Datos_Generales_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
            Datos_Generales_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

            //Estilo de Encabezado Principal
            Encabezado_Principal_Style.Font.FontName = "Arial";
            Encabezado_Principal_Style.Font.Size = 16;
            Encabezado_Principal_Style.Font.Color = "Black";
            Encabezado_Principal_Style.Font.Bold = true;
            Encabezado_Principal_Style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
            Encabezado_Principal_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
            Encabezado_Principal_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
            Encabezado_Principal_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
            Encabezado_Principal_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

            //###################
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(85));
            sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));

            row = sheet.Table.Rows.Add();
            Celda = row.Cells.Add("INVENTARIO DEL " + Txt_Fecha_Inicial.Text + " AL " + Txt_Fecha_Final.Text);
            Celda.MergeAcross = 7;
            Celda.StyleID = "Encabezado_Principal";

            if (Cmb_Partidas_STOCK.SelectedValue != "0")
            {
                row = sheet.Table.Rows.Add();
                Celda = row.Cells.Add("PARTIDA ESPECIFICA: " + Cmb_Partidas_STOCK.SelectedItem.Text);
                Celda.MergeAcross = 18;
                Celda.StyleID = "Encabezado_Principal";
            }
            int Contador_Productos = 0;
            int Limite = 1;
            foreach (DataRow Productos in Dt_Kardex.Rows)
            {
                Contador_Productos++;
                //if (Cmb_Mostrar_Detalles.SelectedValue == "SI")
                //{
                //    Limite = Dt_Kardex.Rows.Count;
                //}
                if (Contador_Productos <= Limite)
                {
                    row = sheet.Table.Rows.Add();
                    row.Cells.Add(new WorksheetCell("CLAVE", "Titulo_Datos_Generales"));
                    Celda = row.Cells.Add("NOMBRE");
                    Celda.MergeAcross = 1;
                    Celda.StyleID = "Titulo_Datos_Generales";
                    Celda = row.Cells.Add("DESCRIPCION");
                    Celda.MergeAcross = 6;
                    Celda.StyleID = "Titulo_Datos_Generales";
                    row.Cells.Add(new WorksheetCell("INICIAL", "Titulo_Datos_Generales"));
                    row.Cells.Add(new WorksheetCell("EXISTENCIA", "Titulo_Datos_Generales"));
                    row.Cells.Add(new WorksheetCell("DISPONIBLE", "Titulo_Datos_Generales"));
                    row.Cells.Add(new WorksheetCell("COMPROMETIDO", "Titulo_Datos_Generales"));
                    row.Cells.Add(new WorksheetCell("ENTRADA", "Titulo_Datos_Generales"));
                    row.Cells.Add(new WorksheetCell("ENTRADA AJUSTE", "Titulo_Datos_Generales"));
                    row.Cells.Add(new WorksheetCell("SALIDA", "Titulo_Datos_Generales"));
                    row.Cells.Add(new WorksheetCell("SALIDA AJUSTE", "Titulo_Datos_Generales"));
                }
                //Generales
                row = sheet.Table.Rows.Add();
                row.Cells.Add(new WorksheetCell(Productos["CLAVE"].ToString(), "Detalles"));
                Celda = row.Cells.Add(Productos["NOMBRE"].ToString());
                Celda.MergeAcross = 1;
                Celda.StyleID = "Datos_Generales";
                Celda = row.Cells.Add(Productos["DESCRIPCION"].ToString());
                Celda.MergeAcross = 6;
                Celda.StyleID = "Datos_Generales";
                row.Cells.Add(new WorksheetCell(Productos["INICIAL"].ToString(), "Detalles"));
                row.Cells.Add(new WorksheetCell(Productos["EXISTENCIA"].ToString(), "Detalles"));
                row.Cells.Add(new WorksheetCell(Productos["DISPONIBLE"].ToString(), "Detalles"));
                row.Cells.Add(new WorksheetCell(Productos["COMPROMETIDO"].ToString(), "Detalles"));
                row.Cells.Add(new WorksheetCell(Productos["ENTRADA"].ToString(), "Detalles"));
                row.Cells.Add(new WorksheetCell(Productos["AJUSTE_ENTRADA"].ToString(), "Detalles"));
                row.Cells.Add(new WorksheetCell(Productos["SALIDA"].ToString(), "Detalles"));
                row.Cells.Add(new WorksheetCell(Productos["AJUSTE_SALIDA"].ToString(), "Detalles"));


            }// Fin de foreach

            book.Save(Ruta_Archivo);
            Response.Clear();
            Response.ClearContent();
            Response.ClearHeaders();

            Response.ContentType = "application/x-msexcel";
            Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta_Archivo);
            //Visualiza el archivo
            Response.WriteFile(Ruta_Archivo);
            Response.Flush();
            Response.Close();
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Informacion.Text = "Es necesario seleccionar una Fecha Valida";
        }
    }
    
 
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN:          Evento  utilizadao para ir a la página principal
    ///                      de la cplicación
    ///PARAMETROS:           
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           02/Mayo/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }


    #endregion

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Informacion.Text = "";

        if (Validar_Fechas())
        {
            //ANTES D ELA FECHA INICIAL
            DataTable Dt_Kardex = Consultar_Mi_Kardex_Inicial();
            //DESPUES DE LA FECHA INICIAL
            Cls_Ope_Alm_Generar_Kardex_Productos_Negocio Kardex_negocio = new Cls_Ope_Alm_Generar_Kardex_Productos_Negocio();
            Kardex_negocio.P_Fecha_I = Txt_Fecha_Inicial.Text.Trim();
            Kardex_negocio.P_Fecha_F = Txt_Fecha_Final.Text.Trim();
            Kardex_negocio.P_Fecha_I = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_I));
            Kardex_negocio.P_Fecha_F = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Kardex_negocio.P_Fecha_F));

            if (Cmb_Partidas_STOCK.SelectedValue != "0")
            {
                Kardex_negocio.P_Partida_ID = Cmb_Partidas_STOCK.SelectedValue;
            }
            //CONSULTAR KARDEX PARA SABER SOLO CUALES SON LOS PRODUCTOS DE STOCK
            //Y MODIFICAR LOS DATOS CON LAS CONSULTAS DE ENTRADAS, SAKLIDAS;ETC

            DataTable Dt_Entradas = Kardex_negocio.Consultar_Entradas();
            DataTable Dt_Entradas_Ajuste = Kardex_negocio.Consultar_Entradas_Ajuste();
            DataTable Dt_Salidas = Kardex_negocio.Consultar_Salidas();
            DataTable Dt_Salidas_Ajuste = Kardex_negocio.Consultar_Salidas_Ajuste();
            DataTable Dt_Comprometidos = Kardex_negocio.Consultar_Compromisos();
            Ds_Alm_Inventario_Stock_General Ds_Fisico = new Ds_Alm_Inventario_Stock_General();

            DataTable Dt_Inventario = new DataTable();
             Dt_Inventario.Columns.Add("CLAVE", typeof(System.String));
                Dt_Inventario.Columns.Add("NOMBRE", typeof(System.String));
                Dt_Inventario.Columns.Add("DESCRIPCION", typeof(System.String));
                Dt_Inventario.Columns.Add("EXISTENCIA", typeof(System.String));
                Dt_Inventario.Columns.Add("COMPROMETIDO", typeof(System.String));
                Dt_Inventario.Columns.Add("DISPONIBLE", typeof(System.String));
                Dt_Inventario.Columns.Add("MINIMO", typeof(System.String));
                Dt_Inventario.Columns.Add("MAXIMO", typeof(System.String));
                Dt_Inventario.Columns.Add("REORDEN", typeof(System.String));
                Dt_Inventario.Columns.Add("COSTO_PROMEDIO", typeof(System.String));
                Dt_Inventario.Columns.Add("ACUMULADO", typeof(System.String));
              
            foreach (DataRow Producto in Dt_Kardex.Rows)
            {
                DataRow Fila_Nueva = Dt_Inventario.NewRow();

                Producto["ENTRADA"] = Sumar_Movimientos(Dt_Entradas, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["AJUSTE_ENTRADA"] = Sumar_Movimientos(Dt_Entradas_Ajuste, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["SALIDA"] = Sumar_Movimientos(Dt_Salidas, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["AJUSTE_SALIDA"] = Sumar_Movimientos(Dt_Salidas_Ajuste, Producto["PRODUCTO_ID"].ToString().Trim());
                Producto["COMPROMETIDO"] = Sumar_Movimientos(Dt_Comprometidos, Producto["PRODUCTO_ID"].ToString().Trim());
                //CALCULAR  EXISTENCIA y DISPONIBLE

                Producto["EXISTENCIA"] = Convert.ToInt32(Producto["INICIAL"]) +
                                        Convert.ToInt32(Producto["ENTRADA"]) +
                                        Convert.ToInt32(Producto["AJUSTE_ENTRADA"]) -
                                        Convert.ToInt32(Producto["SALIDA"]) -
                                        Convert.ToInt32(Producto["AJUSTE_SALIDA"]);

                Producto["DISPONIBLE"] = Convert.ToInt32(Producto["INICIAL"]) +
                                        Convert.ToInt32(Producto["ENTRADA"]) +
                                        Convert.ToInt32(Producto["AJUSTE_ENTRADA"]) -
                                        Convert.ToInt32(Producto["SALIDA"]) -
                                        Convert.ToInt32(Producto["AJUSTE_SALIDA"]) -
                                        Convert.ToInt32(Producto["COMPROMETIDO"]);
                Fila_Nueva["CLAVE"] = Producto["CLAVE"];
                Fila_Nueva["NOMBRE"] = Producto["NOMBRE"];
                Fila_Nueva["DESCRIPCION"] = Producto["DESCRIPCION"];
                Fila_Nueva["EXISTENCIA"] = Producto["EXISTENCIA"];
                Fila_Nueva["COMPROMETIDO"] = Producto["COMPROMETIDO"];
                Fila_Nueva["DISPONIBLE"] = Producto["DISPONIBLE"];
                Fila_Nueva["MINIMO"] = "N/A";
                Fila_Nueva["MAXIMO"] = "N/A";

                Dt_Inventario.Rows.Add(Fila_Nueva);
                Dt_Inventario.AcceptChanges();
            }

            Generar_Reporte(Dt_Inventario, Ds_Fisico, "Rpt_Alm_Inventarios_Generales.rpt");

        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Informacion.Text = "Es necesario seleccionar una Fecha Valida";
        }


       
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
}
