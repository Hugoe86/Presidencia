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
using Presidencia.Cuadro_Comparativo.Negocio;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using Presidencia.Sessiones;
public partial class paginas_Compras_Frm_Ope_Com_Cuadro_Comparativo : System.Web.UI.Page
{
    private static String P_Dt_Cuadro = "CUADRO";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "DESC";
            
            Crear_Cuadro_Comparativo_Economico();
            Crear_Cuadro_Comparativo_Aspectos_Tecnicos();
            //Crear_GridView((DataTable)Session[P_Dt_Cuadro]);
            //Grid_Cuadro_Comparativo.DataSource = (DataTable)Session[P_Dt_Cuadro];
            
            //Grid_Cuadro_Comparativo.DataBind();
            //Cargar_Requisicion();
        }
    }

    private void Crear_Cuadro_Comparativo_Economico() 
    {
        Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio = new Cls_Ope_Com_Cuadro_Comparativo_Negocio();
        Negocio.P_No_Requisicion = "31";
        DataTable Dt_Requisicion = Negocio.Consultar_Requisicion();
        DataTable Dt_Productos = Negocio.Consultar_Productos_Requisicion();
        DataTable Dt_Proveedores = Negocio.Consultar_Proveedores_Que_Cotizaron();
        DataTable Dt_Productos_Cotizados = null;

        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        //Excel.Range oRng;
        try
        {
            int Inicio = 10;
            int C = 3;
            int R = Inicio;
            //Start Excel and get Application object.
            oXL = new Excel.Application();
            oXL.Visible = true;
            //Get a new workbook.
            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;            
            //Agregar cabecera
            //Juntamos la sceldas 
            oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[2, 1], oSheet.Cells[2, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[3, 1], oSheet.Cells[3, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[4, 1], oSheet.Cells[4, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[5, 1], oSheet.Cells[5, 3]).Merge(true);
            oSheet.Cells[1, 1] = "OFICIALÍA MAYOR";
            oSheet.Cells[2, 1] = "DIRECCIÓN DE ADQUISICIONES";
            oSheet.Cells[3, 1] = "REQUISICIÓN NO. " + Negocio.P_No_Requisicion;
            oSheet.Cells[4, 1] = "UNIDAD RESPONSABLE:";
            oSheet.Cells[5, 1] = "FUENTE DE RECURSOS:";
            oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[5, 1]).Font.Bold = true;
            //Para poner color a todo un renglon
            //oSheet.get_Range(oSheet.Cells[5, 1], oSheet.Cells[5, 3]).Cells.EntireRow.Interior.ColorIndex = 50;
            //for (int i = 1; i < 255; i++ )
            //{
            //    oSheet.get_Range(oSheet.Cells[30, i], oSheet.Cells[30, i]).Interior.ColorIndex = i;
            //    oSheet.Cells[30, i] = "" + i;
            //}

            //Agregar datos de requisición
            oSheet.Cells[R, 1] = "UNIDAD";
            oSheet.Cells[R, 2] = "CANTIDAD";            
            oSheet.Cells[R, 3] = "DESCRIPCIÓN";
            //ajustar ancho de columna
            //oSheet.get_Range(oSheet.Cells[R, 3], oSheet.Cells[R, 3]).ColumnWidth = 40;
            oSheet.get_Range(oSheet.Cells[R, 3], oSheet.Cells[R, 3]).EntireColumn.ColumnWidth = 50;
            oSheet.get_Range(oSheet.Cells[R, 3], oSheet.Cells[R, 3]).EntireColumn.WrapText = true;
            oSheet.get_Range(oSheet.Cells[R, 3], oSheet.Cells[R, 3]).EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignJustify;
            //Poner negritas a renglon
            oSheet.get_Range(oSheet.Cells[R-2, 1], oSheet.Cells[R, 999]).Font.Bold = true;
            //alinear cantidad y unidad de medida
            oSheet.get_Range(oSheet.Cells[R+1 , 1], oSheet.Cells[R + 500, 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            //se cargan los datos de la requisa, los productos
            foreach (DataRow Producto in Dt_Productos.Rows)
            {
                R++;                
                oSheet.Cells[R, 1] = Producto["UNIDAD"].ToString();
                oSheet.Cells[R, 2] = Producto["CANTIDAD"].ToString();
                oSheet.Cells[R, 3] = Producto["NOMBRE_PRODUCTO_SERVICIO"].ToString();                
            }
            R++;
            //poner negritas los encabezados SUBTOTAL, IVA, TOTAL, ETC.
            oSheet.get_Range(oSheet.Cells[R, 3], oSheet.Cells[R + 6, 3]).Font.Bold = true;
            oSheet.get_Range(oSheet.Cells[R + 6, 3], oSheet.Cells[R, 3]).HorizontalAlignment = Excel.XlHAlign.xlHAlignRight;
            //poner negritas la fila de subtotal, iva, total
            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 2, 1]).EntireRow.Font.Bold = true;
            oSheet.Cells[R, C] = "SUBTOTAL";
            R++;
            oSheet.Cells[R, C] = "IVA";
            R++;
            oSheet.Cells[R, C] = "TOTAL";
            R++;
            oSheet.Cells[R, C] = "TIEMPO DE ENTREGA";
            R++;
            oSheet.Cells[R, C] = "CONDICIONES DE PAGO";
            R++;
            oSheet.Cells[R, C] = "GARANTIA";
            R++;
            oSheet.Cells[R, C] = "VIGENCIA COTIZACIÓN";
            R = Inicio;
            C ++;
            foreach (DataRow Proveedor in Dt_Proveedores.Rows)
            {
                R = Inicio;                
                //Datos del proveedor
                Negocio.P_Proveedor_ID = Proveedor["PROVEEDOR_ID"].ToString().Trim();
                oSheet.Cells[R - 2, C] = Proveedor["COMPANIA"].ToString().Trim();
                //ajustar texto
                String Fecha = String.Format("{0:dd/MMM/yyyy}", Proveedor["FECHA_CREO"]);
                oSheet.Cells[R - 1, C] = "Prov. " + Proveedor["PROVEEDOR_ID"].ToString().Trim() + " - Reg. " + Fecha;
                //Juntamos la sceldas de nombre y registro fecha
                oSheet.get_Range(oSheet.Cells[R - 2, C], oSheet.Cells[R - 2, C + 2]).Merge(true);
                oSheet.get_Range(oSheet.Cells[R - 1, C], oSheet.Cells[R - 1, C + 2]).Merge(true);
                //Ajustar texto de nombre y fech DE REGISTRO DEL PROVEEDOR                
                oSheet.get_Range(oSheet.Cells[R - 2, C], oSheet.Cells[R - 2, C]).WrapText = true;
                oSheet.get_Range(oSheet.Cells[R - 2, C], oSheet.Cells[R - 2, C]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                oSheet.get_Range(oSheet.Cells[R - 1, C], oSheet.Cells[R - 1, C]).WrapText = true;
                oSheet.get_Range(oSheet.Cells[R - 1, C], oSheet.Cells[R - 1, C]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;

                //Consultamos los productos cotizados del proveedor seleccionado
                Dt_Productos_Cotizados = Negocio.Consultar_Precios_Cotizados();
                //A cada uno d elos productos le agrego el cotizado del proveedor
                oSheet.Cells[R, C] = "MARCA";                
                oSheet.Cells[R, C + 1] = "P.UNITARIO";
                oSheet.Cells[R, C + 2] = "IMPORTE";
                //Ajustar celda de MARCA
                oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C]).EntireColumn.ColumnWidth = 18;
                oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C]).EntireColumn.WrapText = true;
                oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C]).EntireColumn.HorizontalAlignment = Excel.XlHAlign.xlHAlignJustify;
                DataRow[] Producto_Cotizado = null;
                foreach (DataRow Producto in Dt_Productos.Rows)
                {
                    R++;                    
                    Producto_Cotizado = Dt_Productos_Cotizados.Select("PROD_SERV_ID = '" +
                        Producto["PROD_SERV_ID"].ToString().Trim() + "'");
                    //marca que ofrece el proveedor
                    oSheet.Cells[R, C] = Producto_Cotizado[0]["MARCA"].ToString().Trim();
                    //precio unitario
                    //Se pone formato a las celdas
                    oSheet.get_Range(oSheet.Cells[R, C + 1], oSheet.Cells[R, C + 1]).NumberFormat = "$ #,##0.00";
                    oSheet.Cells[R, C + 1] = Producto_Cotizado[0]["PRECIO_U_SIN_IMP_COTIZADO"].ToString().Trim();
                    //importe 
                    //Se pone formato a las celdas
                    oSheet.get_Range(oSheet.Cells[R, C + 2], oSheet.Cells[R, C + 2]).NumberFormat = "$ #,##0.00";
                    oSheet.Cells[R, C + 2] = Producto_Cotizado[0]["TOTAL_COTIZADO"].ToString().Trim();
                }
                //Subtotal
                try
                {
                    R++;
                    oSheet.get_Range(oSheet.Cells[R, C + 2], oSheet.Cells[R, C + 2]).NumberFormat = "$ #,##0.00";
                    oSheet.Cells[R, C+2] = Producto_Cotizado[0]["SUBTOTAL_COTIZADO_REQUISICION"].ToString().Trim();
                    R++;
                    oSheet.get_Range(oSheet.Cells[R, C + 2], oSheet.Cells[R, C + 2]).NumberFormat = "$ #,##0.00";
                    oSheet.Cells[R, C+2] = Producto_Cotizado[0]["IVA_COTIZADO_REQ"].ToString().Trim();
                    R++;
                    oSheet.get_Range(oSheet.Cells[R, C + 2], oSheet.Cells[R, C + 2]).NumberFormat = "$ #,##0.00";
                    oSheet.Cells[R, C+2] = Producto_Cotizado[0]["TOTAL_COTIZADO_REQUISICION"].ToString().Trim();
                    R++;
                    oSheet.Cells[R, C] = Producto_Cotizado[0]["TIEMPO_ENTREGA"].ToString().Trim();
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).WrapText = true;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).Merge(true);
                    R++;
                    oSheet.Cells[R, C] = Producto_Cotizado[0]["CONDICIONES_PAGO"].ToString().Trim();
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).WrapText = true;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).Merge(true);
                    R++;
                    oSheet.Cells[R, C] = Producto_Cotizado[0]["GARANTIA"].ToString().Trim();
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).WrapText = true;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).Merge(true);
                    R++;
                    oSheet.Cells[R, C] = String.Format("{0:dd/MMM/yyyy}",Producto_Cotizado[0]["VIGENCIA_PROPUESTA"]);
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).WrapText = true;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
                    oSheet.get_Range(oSheet.Cells[R, C], oSheet.Cells[R, C + 2]).Merge(true);
                }
                catch(Exception Ex)
                {
                    Ex.ToString();
                    R += 5;
                }
                C += 3;
            }
            //Fecha ELaboró
            oSheet.get_Range(oSheet.Cells[R + 2, 1], oSheet.Cells[R + 2, 3]).WrapText = true;
            oSheet.get_Range(oSheet.Cells[R + 2, 1], oSheet.Cells[R + 2, 3]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            oSheet.get_Range(oSheet.Cells[R + 2, 1], oSheet.Cells[R + 2, 3]).Merge(true);
            oSheet.Cells[R + 2, 1] = "FECHA DE ELABORACIÓN: " + DateTime.Now.ToString("dd/MMM/yyyy");
            //Empleado que elaboró
            oSheet.get_Range(oSheet.Cells[R + 3, 1], oSheet.Cells[R + 3, 3]).WrapText = true;
            oSheet.get_Range(oSheet.Cells[R + 3, 1], oSheet.Cells[R + 3, 3]).HorizontalAlignment = Excel.XlHAlign.xlHAlignLeft;
            oSheet.get_Range(oSheet.Cells[R + 3, 1], oSheet.Cells[R + 3, 3]).Merge(true);
            oSheet.Cells[R + 3, 1] = "ELABORÓ: " + Cls_Sessiones.Nombre_Empleado;

            //Poner bordes
            for (int i = Inicio; i <= R; i++)
            {
                for (int j = 1; j < 4; j++)
                {
                    oSheet.get_Range(oSheet.Cells[i, j], oSheet.Cells[i, j]).BorderAround(Excel.XlLineStyle.xlContinuous,
                    Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic,
                        Excel.XlColorIndex.xlColorIndexAutomatic);
                }
            }
            for (int i = 8; i <= R; i++)
            {
                for (int j = 4; j < C; j++)
                {
                    oSheet.get_Range(oSheet.Cells[i, j], oSheet.Cells[i, j]).BorderAround(Excel.XlLineStyle.xlContinuous,
                    Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic,
                        Excel.XlColorIndex.xlColorIndexAutomatic);
                }
            }
            //Poner color a una celda
            oSheet.get_Range(oSheet.Cells[Inicio, 1], oSheet.Cells[Inicio, C-1]).Interior.ColorIndex = 15;            
            //Titulo de la Tabla Comparativa            
            oSheet.get_Range(oSheet.Cells[Inicio - 4, 4], oSheet.Cells[Inicio - 4, C - 1]).Merge(true);
            oSheet.get_Range(oSheet.Cells[Inicio - 4, 4], oSheet.Cells[Inicio - 4, C - 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.get_Range(oSheet.Cells[Inicio - 4, 4], oSheet.Cells[Inicio - 4, C - 1]).Font.Bold = true;
            oSheet.Cells[Inicio - 4, 4] = "TABLA COMPARATIVA ECONÓMICA";
        }
        catch (Exception theException)
        {
            String errorMessage;
            errorMessage = "Error: ";
            errorMessage = String.Concat(errorMessage, theException.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, theException.Source);
        }
    }

    private void Crear_Cuadro_Comparativo_Aspectos_Tecnicos()
    {
        Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio = new Cls_Ope_Com_Cuadro_Comparativo_Negocio();
        Negocio.P_No_Requisicion = "31";
        DataTable Dt_Requisicion = Negocio.Consultar_Requisicion();
        DataTable Dt_Productos = Negocio.Consultar_Productos_Requisicion();
        DataTable Dt_Proveedores = Negocio.Consultar_Proveedores_Que_Cotizaron();
        DataTable Dt_Productos_Cotizados = null;

        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        Excel.Range oRng;
        try
        {
            int Renglon_Inicio = 9;
            int C = 2;
            int R = Renglon_Inicio;
            //Start Excel and get Application object.
            oXL = new Excel.Application();
            oXL.Visible = true;
            //Get a new workbook.
            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing.Value));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;
            //Agregar cabecera
            //Juntamos la sceldas 
            oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[1, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[2, 1], oSheet.Cells[2, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[3, 1], oSheet.Cells[3, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[4, 1], oSheet.Cells[4, 3]).Merge(true);
            oSheet.get_Range(oSheet.Cells[5, 1], oSheet.Cells[5, 3]).Merge(true);
            oSheet.Cells[1, 1] = "OFICIALÍA MAYOR";
            oSheet.Cells[2, 1] = "DIRECCIÓN DE ADQUISICIONES";
            oSheet.Cells[3, 1] = "REQUISICIÓN NO. " + Negocio.P_No_Requisicion;
            oSheet.Cells[4, 1] = "UNIDAD RESPONSABLE:";
            oSheet.Cells[5, 1] = "FUENTE DE RECURSOS:";
            oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[5, 1]).Font.Bold = true;

            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).EntireColumn.ColumnWidth = 32;
            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).WrapText = true;
            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            oSheet.Cells[R, 1] = "DESCRIPCIÓN";
            oSheet.Cells[R + 1, 1] = "x";
            foreach (DataRow Proveedor in Dt_Proveedores.Rows)
            {
                R = Renglon_Inicio;
                //establecer formato a celdas
                //Datos del proveedor
                Negocio.P_Proveedor_ID = Proveedor["PROVEEDOR_ID"].ToString().Trim();

                String Fecha_Registro = String.Format("{0:dd/MMM/yyyy}", Proveedor["FECHA_CREO"]);
                String Datos_Proveedor = Proveedor["COMPANIA"].ToString().Trim() + " \r\n Prov. " +
                    Proveedor["PROVEEDOR_ID"].ToString().Trim() + " - Reg. " + Fecha_Registro;
                oSheet.Cells[R, C] = Datos_Proveedor;
                //ajustar texto
                R++;
                oSheet.Cells[R, C] = "CUMPLE   (SÍ)  (NO) \r\n EN CASO NEGATIVO ANOTAR LOS MOTIVOS:";               
                oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R, 1]).EntireRow.RowHeight = 120;
                oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).VerticalAlignment = Excel.XlVAlign.xlVAlignTop;
                C++;
            }
            //Fecha ELaboró
            oSheet.get_Range(oSheet.Cells[R + 3, 1], oSheet.Cells[R + 3, 3]).WrapText = true;
            oSheet.get_Range(oSheet.Cells[R + 3, 1], oSheet.Cells[R + 3, 3]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.get_Range(oSheet.Cells[R + 3, 1], oSheet.Cells[R + 3, 3]).Merge(true);
            oSheet.Cells[R + 3, 1] = "NOMBRE Y FIRMA DE QUIEN REALIZÓ LA EVALUACÍON: ";
            //Empleado que elaboró
            oSheet.get_Range(oSheet.Cells[R + 4, 1], oSheet.Cells[R + 4, 3]).WrapText = true;
            oSheet.get_Range(oSheet.Cells[R + 4, 1], oSheet.Cells[R + 4, 3]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.get_Range(oSheet.Cells[R + 4, 1], oSheet.Cells[R + 4, 3]).Merge(true);
            oSheet.Cells[R + 4, 1] = "FECHA DE ELABORACIÓN: " + DateTime.Now.ToString("dd/MMM/yyyy");

            //Poner bordes
            for (int i = Renglon_Inicio; i <= Renglon_Inicio + 1; i++)
            {
                for (int j = 1; j < C; j++)
                {
                    oSheet.get_Range(oSheet.Cells[i, j], oSheet.Cells[i, j]).BorderAround(Excel.XlLineStyle.xlContinuous,
                    Excel.XlBorderWeight.xlThin, Excel.XlColorIndex.xlColorIndexAutomatic,
                        Excel.XlColorIndex.xlColorIndexAutomatic);
                }
            }
            //Poner color a una celda
            oSheet.get_Range(oSheet.Cells[Renglon_Inicio, 1], oSheet.Cells[Renglon_Inicio, C - 1]).Interior.ColorIndex = 15;
            //Titulo de la Tabla Comparativa            
            oSheet.get_Range(oSheet.Cells[Renglon_Inicio - 2, 2], oSheet.Cells[Renglon_Inicio - 2, C - 1]).Merge(true);
            oSheet.get_Range(oSheet.Cells[Renglon_Inicio - 2, 2], oSheet.Cells[Renglon_Inicio - 2, C - 1]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.get_Range(oSheet.Cells[Renglon_Inicio - 2, 2], oSheet.Cells[Renglon_Inicio - 2, C - 1]).Font.Bold = true;
            oSheet.Cells[Renglon_Inicio - 2, 2] = "TABLA COMPARATIVA DE ASPECTOS TÉCNICOS";
        }
        catch (Exception theException)
        {
            String errorMessage;
            errorMessage = "Error: ";
            errorMessage = String.Concat(errorMessage, theException.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, theException.Source);
        }
    }



    private void Crear_GridView(DataTable Dt_Tabla) 
    {
    //    Grid_Cuadro_Comparativo.AutoGenerateColumns = false;
    //    int No_Columnas = Dt_Tabla.Columns.Count;
    //    String[] Nombre_Columna = new String[No_Columnas];
    //    for (int i = 0; i < No_Columnas; i++)
    //    {
    //        Nombre_Columna[i] = Dt_Tabla.Columns[i].ColumnName;
    //    }
    //    for (int i = 0; i < No_Columnas; i++)
    //    {
    //        BoundField Columna = new BoundField();
    //        Columna.DataField = Nombre_Columna[i];
    //        Columna.HeaderText = Nombre_Columna[i];
    //        Columna.ItemStyle.Wrap = true;
    //        Columna.ItemStyle.Width = 2;
    //        if (No_Columnas < 2) 
    //        {
    //            Columna.Visible = false;
           
    //        }
    //        Grid_Cuadro_Comparativo.Columns.Add(Columna);
    //    }  




        //if (Dt_Proveedores != null) 
        //{
        //    //Por cada proveedor obtener sus productos
        //    foreach (DataRow Renglon in Dt_Proveedores.Rows) 
        //    {
        //        Negocio.P_Proveedor_ID = Renglon["PROVEEDOR_ID"].ToString().Trim();
        //        //Agrego la columna correspondiente al proveedor
        //        Dt_Productos.Columns.Add(Renglon["PROVEEDOR_ID"].ToString().Trim());
        //        //Consultamos los productos cotizados del proveedor seleccionado
        //        Dt_Productos_Cotizados = Negocio.Consultar_Precios_Cotizados();
        //        //A cada uno d elos productos le agrego el cotizado del proveedor
        //        foreach(DataRow Producto in Dt_Productos.Rows)
        //        {
        //            DataRow[] Producto_Cotizado = Dt_Productos_Cotizados.Select("PROD_SERV_ID = '" + 
        //                Producto["PROD_SERV_ID"].ToString().Trim() + "'");
        //            Producto[Renglon["PROVEEDOR_ID"].ToString().Trim()] = Producto_Cotizado[0]["PRECIO_U_SIN_IMP_COTIZADO"].ToString().Trim();                     
        //        }
        //    }
        //}
        //Session[P_Dt_Cuadro] = Dt_Productos;
    }
}
