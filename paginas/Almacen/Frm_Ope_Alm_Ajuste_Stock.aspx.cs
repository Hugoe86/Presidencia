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
using Presidencia.Ajustar_Stock.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Stock;
using Presidencia.Autorizar_Ajuste.Negocio;
using CrystalDecisions.ReportSource;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Almacen_Frm_Ope_Alm_Ajuste_Stock : System.Web.UI.Page
{
    private static String P_Dt_Producto_Buscado = "Dt_Producto_Buscado";
    private static String P_Dt_Productos_Ajustados = "Dt_Productos_Ajustados";
    private static String P_Dt_Ajustes_Stock = "Dt_Ajustes_Stock";
    private const String Operacion_Quitar_Renglon = "QUITAR";
    private const String Operacion_Agregar_Renglon_Nuevo = "AGREGAR_NUEVO";
    private const String Operacion_Agregar_Renglon_Copia = "AGREGAR_COPIA";
    private const String MODO_LISTADO = "LISTADO";
    private const String MODO_NUEVO = "NUEVO";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Activa"] = true;
            ViewState["SortDirection"] = "DESC";
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            int meses = _DateTime.Month;
            meses = meses * -1;
            meses++;
            _DateTime = _DateTime.AddDays(dias);
            _DateTime = _DateTime.AddMonths(meses);
            //Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Inicial.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            //Llenar_Grid_Productos();
            Cmb_Movimiento.Items.Clear();
            Cmb_Movimiento.Items.Add("ENTRADA");
            Cmb_Movimiento.Items.Add("SALIDA");
            Cmb_Movimiento.Items[0].Selected = true;
            Session[P_Dt_Productos_Ajustados] = Construir_Tabla_Productos_Ajustados();
            Habilitar_Botones(MODO_LISTADO);
            Llenar_Grid_Ajustes_Inventario();
        }
        if (Txt_Producto.Text.Trim().Length == 0)
        {
            Txt_Conteo_Fisico.Enabled = false;
        }
        else
        {
            Txt_Conteo_Fisico.Enabled = true;
        }
        Mostrar_Informacion("",false);
    }
    private void Habilitar_Botones(String Caso) 
    {
        switch(Caso)
        {
            case MODO_LISTADO:
                Btn_Salir.ToolTip = "Inicio";
                Btn_Guardar.Visible = false;
                Btn_Nuevo.Visible = true;
                Div_Listado_Ajustes.Visible = true;
                Div_Contenido.Visible = false;
                Limpiar_Form();
                Session[P_Dt_Productos_Ajustados] = Construir_Tabla_Productos_Ajustados();
                break;
            case MODO_NUEVO:
                Btn_Salir.ToolTip = "Regresar";
                Btn_Guardar.Visible = true;
                Btn_Nuevo.Visible = false;
                Div_Listado_Ajustes.Visible = false;
                Div_Contenido.Visible = true;
                Limpiar_Form();
                break;
        }
    }
    protected void Btn_Seleccionar_Producto_Click(object sender, ImageClickEventArgs e)
    {
        String Producto_ID = ((ImageButton)sender).CommandArgument;
        DataTable Dt_Productos = (DataTable)Session[P_Dt_Productos_Ajustados];
        DataRow [] _DataRow = Dt_Productos.Select("PRODUCTO_ID = '" + Producto_ID + "'");
        if (_DataRow != null && _DataRow.Length > 0)
        {
           Dt_Productos = Agregar_Quitar_Renglones_A_DataTable(Dt_Productos, _DataRow[0], Operacion_Quitar_Renglon);
           Session[P_Dt_Productos_Ajustados] = Dt_Productos;
           Resumen_Movimientos();
           Grid_Productos_Ajustados.DataSource = Dt_Productos;
           Grid_Productos_Ajustados.DataBind();
        }
        else
        {
            Mostrar_Informacion("Seleccione producto para eliminar de la lista",false);
        }      
    }

    private void Cargar_Datos_Producto() 
    {
        //llenar campos
        DataTable Dt_Producto = Session[P_Dt_Producto_Buscado] as DataTable;
        if (Dt_Producto != null && Dt_Producto.Rows.Count > 0)
        {
            DataRow[] Producto = Dt_Producto.Select("CLAVE = '" + Txt_Clave.Text.Trim() + "'");
            // Txt_Clave.Text = Producto[0]["CLAVE"].ToString();
            Txt_Producto.Text = Producto[0]["NOMBRE"].ToString();
            Txt_Descripcion.Text = Producto[0]["DESCRIPCION"].ToString();
            Txt_Costo.Text = Producto[0]["COSTO"].ToString();
            Txt_Existencia.Text = Producto[0]["EXISTENCIA"].ToString();
            Hdn_Comprometido.Value = Producto[0]["COMPROMETIDO"].ToString();
        }
        else
        {
            Mostrar_Informacion("No se encontró en producto", true);
        }
    }


    private void Llenar_Grid_Ajustes_Inventario() 
    {
        Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio = new Cls_Ope_Alm_Ajustar_Stock_Negocio();
        Negocio.P_Fecha_Inicial = Txt_Fecha_Inicial.Text.Trim();
        Negocio.P_Fecha_Final = Txt_Fecha_Final.Text.Trim();
        Negocio.P_No_Ajuste = Txt_Busqueda.Text.Trim();
        DataTable Dt_Ajustes = Negocio.Consultar_Ajustes_Inventario();
        if (Dt_Ajustes != null && Dt_Ajustes.Rows.Count > 0)
        {
            Session[P_Dt_Ajustes_Stock] = Dt_Ajustes;
            Grid_Ajustes_Inventario.DataSource = Dt_Ajustes;
            Grid_Ajustes_Inventario.DataBind();
        }
        else
        {
            Grid_Ajustes_Inventario.DataSource = null;
            Grid_Ajustes_Inventario.DataBind();
        }
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Ajustes_Inventario();
    }
    protected void Btn_Buscar_Producto_Click(object sender, ImageClickEventArgs e)
    {

        if (Txt_Clave.Text.Trim().Length > 0)
        {
            Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio = new Cls_Ope_Alm_Ajustar_Stock_Negocio();
            Negocio.P_Clave = Txt_Clave.Text.Trim();
            DataTable Dt_Productos = Negocio.Consultar_Productos();
            Session[P_Dt_Producto_Buscado] = Dt_Productos;
            Cargar_Datos_Producto();
            Txt_Conteo_Fisico.Enabled = true;
            Txt_Importe.Text = "";
            Txt_Diferencia.Text = "";
            Txt_Conteo_Fisico.Text = "";
            Txt_Conteo_Fisico.Focus();
        }
        else
        {
            Mostrar_Informacion("Ingrese una clave para buscar",true);
        }
    }

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Regresar")
        {
            Habilitar_Botones(MODO_LISTADO);
            Div_Listado_Ajustes.Visible = true;
            Div_Contenido.Visible = false;
            Llenar_Grid_Ajustes_Inventario();
        }
        else
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }

    protected void Btn_Seleccionar_Inventario_Click(object sender, ImageClickEventArgs e)
    {
        ScriptManager.RegisterStartupScript(
            this, this.GetType(), "Requisiciones", "alert('Opción temporalmente deshabilitada');", true);
        //String No_Ajuste = ((ImageButton)sender).CommandArgument;
        //DataTable Dt_Ajustes = (DataTable)Session[P_Dt_Ajustes_Stock];
        //DataRow [] Dr_Ajuste = Dt_Ajustes.Select("NO_AJUSTE = " + No_Ajuste);        
        //Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio = new Cls_Ope_Alm_Ajustar_Stock_Negocio();
        //Negocio.P_No_Ajuste = No_Ajuste;
        //DataTable Dt_Productos_De_Ajuste = Negocio.Consultar_Productos_De_Ajuste();
        //DataColumn Columna = new DataColumn("CLAVE", System.Type.GetType("System.String"));
        //Dt_Productos_De_Ajuste.Columns.Add(Columna);
        //Habilitar_Botones(MODO_NUEVO);
        //Session[P_Dt_Productos_Ajustados] = Dt_Productos_De_Ajuste;
        //Grid_Productos_Ajustados.DataSource = Dt_Productos_De_Ajuste;
        //Grid_Productos_Ajustados.DataBind();
        //Txt_Justificacion.Text = Dr_Ajuste[0]["MOTIVO_AJUSTE_COOR"].ToString();
        //Resumen_Movimientos();
    }

    private void Calcular_Movimientos_Ajuste(DataTable Dt_Tabla)
    {
        //Obtenemos el DAtatable que contiene los detalles del ajuste 
        if (Dt_Tabla != null)
        {

        }//Fin del if
    }


    protected void Btn_Imprimir_Ajuste_Inventario_Click(object sender, ImageClickEventArgs e)
    {
        //ScriptManager.RegisterStartupScript(
        //    this, this.GetType(), "Requisiciones", "alert('Opción temporalmente deshabilitada');", true);

        String No_Ajuste = ((ImageButton)sender).CommandArgument;
        //reALIZAMOS LA CONSULTA PARA TRAERNOS LOS DATOS QUE SE GUARDARON
        Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio Clase_Negocio = new Cls_Ope_Alm_Autorizar_Ajuste_Inventario_Negocio();
        Clase_Negocio.P_No_Ajuste = No_Ajuste;
        DataTable Dt_Ajuste_Detalle = Clase_Negocio.Consultar_Ajustes_Inventario();

        //Preparamos el Dataset con los dos datatable para imprimir
        DataSet Ds_Imprimir_Ajuste = new DataSet();

        Dt_Ajuste_Detalle.Columns.Add("ENTRADA_PRODUCTO", typeof(System.String));
        Dt_Ajuste_Detalle.Columns.Add("SALIDA_PRODUCTO", typeof(System.String));
        Dt_Ajuste_Detalle.Columns.Add("PRODUCTO_AJUSTADO", typeof(System.String));
        Dt_Ajuste_Detalle.Columns.Add("ENTRADA_UNIDAD", typeof(System.String));
        Dt_Ajuste_Detalle.Columns.Add("SALIDA_UNIDAD", typeof(System.String));
        Dt_Ajuste_Detalle.Columns.Add("UNIDADES_AJUSTADAS", typeof(System.String));
        Dt_Ajuste_Detalle.Columns.Add("IMPORTE_ENTRADA", typeof(System.Double));
        Dt_Ajuste_Detalle.Columns.Add("IMPORTE_SALIDA", typeof(System.Double));
        Dt_Ajuste_Detalle.Columns.Add("IMPORTE_SALDO", typeof(System.Double));

        //consultar detalles
        DataTable Dt_Productos_Ajuste = Clase_Negocio.Consultar_Detalle_Ajustes();//(DataTable)Session["Dt_Productos_Ajuste"];
        //calculñar datos de ajuste
        //DataTable Dt_Productos_Ajuste = Dt_Tabla;//(DataTable)Session["Dt_Productos_Ajuste"];
        int Entradas_Producto = 0;
        int Salidas_Producto = 0;
        int Total_Producto = 0;
        int Entradas_Unidad = 0;
        int Salidas_Unidad = 0;
        int Total_Unidad = 0;
        double Entradas_Importe = 0;
        double Salidas_Importe = 0;
        double Total_Importe = 0;

        for (int i = 0; i < Dt_Productos_Ajuste.Rows.Count; i++)
        {

            if (Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento].ToString().Trim() == "ENTRADA")
            {
                Entradas_Producto = Entradas_Producto + 1;
                Entradas_Unidad = Entradas_Unidad + int.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Diferencia].ToString().Trim());
                Entradas_Importe = Entradas_Importe + double.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Importe_Diferencia].ToString().Trim());


            }
            if (Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Tipo_Movimiento].ToString().Trim() == "SALIDA")
            {
                Salidas_Producto = Salidas_Producto + 1;
                Salidas_Unidad = Salidas_Unidad + int.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Diferencia].ToString().Trim());
                Salidas_Importe = Salidas_Importe + double.Parse(Dt_Productos_Ajuste.Rows[i][Ope_Alm_Ajustes_Detalles.Campo_Importe_Diferencia].ToString().Trim());
            }
        }//Fin del for
        //Asignamos los valores de las entradas y salidas a las cajas de texto
        //Lbl_Entradas_Producto.Text = Entradas_Producto.ToString();
        //Lbl_Entradas_Unidad.Text = Entradas_Unidad.ToString();
        //Lbl_Importe_Entradas.Text = Entradas_Importe.ToString();
        //Lbl_Salidas_Producto.Text = Salidas_Producto.ToString();
        //Lbl_Salidas_Unidad.Text = Salidas_Unidad.ToString();
        //Lbl_Importe_Salidas.Text = Salidas_Importe.ToString();
        //Lbl_Producto_Ajustado.Text = Total_Producto.ToString();
        //Calculamos el Ajuste del Producto
        Total_Producto = Entradas_Producto - Salidas_Producto;
        //Obtenemos el Total Por UNIDAD
        Total_Unidad = Entradas_Unidad - Salidas_Unidad;
        //Onbtenemos el Total por Importe
        Total_Importe = Entradas_Importe - Salidas_Importe;
        //Asignamos los totales a los Label
        //Lbl_Producto_Ajustado.Text = Total_Producto.ToString();
        //Lbl_Unidades_Ajustadas.Text = Total_Unidad.ToString();
        //Lbl_Importe_Saldo.Text = Total_Importe.ToString();

        //Asignar valores
        Dt_Ajuste_Detalle.Rows[0]["ENTRADA_PRODUCTO"] = Entradas_Producto.ToString();// Lbl_Entradas_Producto.Text.Trim();
        Dt_Ajuste_Detalle.Rows[0]["SALIDA_PRODUCTO"] = Salidas_Producto.ToString();// Lbl_Salidas_Producto.Text.Trim();
        Dt_Ajuste_Detalle.Rows[0]["PRODUCTO_AJUSTADO"] = Total_Producto.ToString(); //Lbl_Producto_Ajustado.Text.Trim();

        Dt_Ajuste_Detalle.Rows[0]["ENTRADA_UNIDAD"] = Entradas_Unidad.ToString();// Lbl_Entradas_Unidad.Text.Trim();
        Dt_Ajuste_Detalle.Rows[0]["SALIDA_UNIDAD"] = Salidas_Unidad.ToString();//Lbl_Salidas_Unidad.Text.Trim();
        Dt_Ajuste_Detalle.Rows[0]["UNIDADES_AJUSTADAS"] = Total_Unidad.ToString();//Lbl_Unidades_Ajustadas.Text.Trim();

        Dt_Ajuste_Detalle.Rows[0]["IMPORTE_ENTRADA"] = Entradas_Importe;// double.Parse(Lbl_Importe_Entradas.Text.Trim());
        Dt_Ajuste_Detalle.Rows[0]["IMPORTE_SALIDA"] = Salidas_Importe;//double.Parse(Lbl_Importe_Salidas.Text.Trim());
        Dt_Ajuste_Detalle.Rows[0]["IMPORTE_SALDO"] = Total_Importe;// double.Parse(Lbl_Importe_Saldo.Text.Trim());

        Ds_Imprimir_Ajuste.Tables.Add(Dt_Ajuste_Detalle.Copy());
        Ds_Imprimir_Ajuste.Tables[0].TableName = "Dt_Ajuste_Detalle";
        Ds_Imprimir_Ajuste.AcceptChanges();
        Ds_Imprimir_Ajuste.Tables.Add(Dt_Productos_Ajuste.Copy());
        Ds_Imprimir_Ajuste.Tables[1].TableName = "Dt_Productos_Ajuste";
        Ds_Imprimir_Ajuste.AcceptChanges();

        Ds_Ope_Alm_Autorizar_Ajuste_Inventario Obj_Ajuste_Inv = new Ds_Ope_Alm_Autorizar_Ajuste_Inventario();
        Generar_Reporte(Ds_Imprimir_Ajuste, Obj_Ajuste_Inv, "Rpt_Ope_Alm_Autorizar_Ajuste_Inventario.rpt", "Rpt_Ope_Alm_Autorizar_Ajuste_Inventario.pdf");

    }

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Session[P_Dt_Productos_Ajustados] = Construir_Tabla_Productos_Ajustados();
        Habilitar_Botones(MODO_NUEVO);
    }
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        if (Session[P_Dt_Productos_Ajustados] != null)
        {
            DataTable Dt_Productos = Session[P_Dt_Productos_Ajustados] as DataTable;
            if (Dt_Productos.Rows.Count > 0)
            {
                if (Txt_Justificacion.Text.Trim().Length > 0)
                {
                    Cls_Ope_Alm_Ajustar_Stock_Negocio Negocio = new Cls_Ope_Alm_Ajustar_Stock_Negocio();
                    Negocio.P_Dt_Productos_Ajustados = Session[P_Dt_Productos_Ajustados] as DataTable;
                    Negocio.P_Estatus = "GENERADO";
                    Negocio.P_Motivo_Ajuste_Coordinador = Txt_Justificacion.Text.Trim();
                    try
                    {
                        int Consecutivo = Negocio.Guardar_Ajustes_Inventario();
                        //comprometer productos que su operación es SALIDA
                        if (Consecutivo > 0) 
                        {
                            DataTable Dt_Ajuste = (DataTable)Session[P_Dt_Productos_Ajustados];
                            foreach(DataRow Dr_Producto in Dt_Ajuste.Rows)
                            {
                                if (Dr_Producto["TIPO_MOVIMIENTO"].ToString() == "SALIDA")
                                {
                                    Cls_Ope_Alm_Stock.Comprometer_Producto
                                        (Dr_Producto["PRODUCTO_ID"].ToString(),int.Parse(Dr_Producto["DIFERENCIA"].ToString()));
                                }
                            }                            
                        }
                        ScriptManager.RegisterStartupScript(
                            this, this.GetType(), "Requisiciones", "alert('Ajuste registrado con el folio: AI-" + Consecutivo + "');", true);
                        Session[P_Dt_Ajustes_Stock] = null;
                        Session[P_Dt_Producto_Buscado] = null;
                        Limpiar_Form();
                        Llenar_Grid_Ajustes_Inventario();
                        Habilitar_Botones(MODO_LISTADO);
                    }
                    catch (Exception Ex)
                    {
                        Mostrar_Informacion(Ex.ToString(),true);
                    }
                }
                else
                {
                    Mostrar_Informacion("Escriba la justificación para el ajuste de stock", true);
                }
            }
            else
            {
                Mostrar_Informacion("No se puede guardar un ajuste sin productos",true);
            }
        }
    }
    private bool Duplicado(DataTable Dt_Tabla, String Producto_ID)
    {
        DataRow []_DataRow = Dt_Tabla.Select("PRODUCTO_ID='" + Producto_ID + "'");
        bool duplex = (_DataRow != null && _DataRow.Length > 0) ? true : false;
        return duplex;
    }
    protected void Btn_Agregar_Click(object sender, EventArgs e)
    {
        if (Txt_Producto.Text.Trim().Length > 0)
        {
            if (Txt_Diferencia.Text.Trim().Length > 0)
            {
                if (int.Parse(Txt_Diferencia.Text.Trim()) != 0)
                {
                    String Producto_ID = String.Format("{0:0000000000}", int.Parse(Txt_Clave.Text.Trim()));
                    
                    if (!Duplicado(((DataTable)Session[P_Dt_Productos_Ajustados]), Producto_ID))
                    {
                        int Disponible_Stock = Cls_Ope_Alm_Stock.Consultar_Disponible(Producto_ID);
                        if ( Math.Abs(int.Parse(Txt_Diferencia.Text.Trim())) > Disponible_Stock && Cmb_Movimiento.SelectedItem.Text.Trim() == "SALIDA")
                        {
                            Mostrar_Informacion("No se puede realizar salida del producto seleccionado, " + 
                                "es posible que existan productos comprometidos", true);
                        }
                        else
                        {
                            DataRow Renglon_Producto = ((DataTable)Session[P_Dt_Productos_Ajustados]).NewRow();
                            Renglon_Producto["PRODUCTO_ID"] = Producto_ID;
                            Renglon_Producto["CLAVE"] = Txt_Clave.Text.Trim();
                            Renglon_Producto["NOMBRE_DESCRIPCION"] = Txt_Producto.Text + ", " + Txt_Descripcion.Text.Trim();
                            Renglon_Producto["IMPORTE_DIFERENCIA"] = Math.Abs(double.Parse(Txt_Importe.Text.Trim())).ToString();// Txt_Importe.Text.Trim();
                            Renglon_Producto["DIFERENCIA"] = Math.Abs(int.Parse(Txt_Diferencia.Text.Trim())).ToString();
                            Renglon_Producto["TIPO_MOVIMIENTO"] = Cmb_Movimiento.SelectedItem.Text.Trim();
                            Renglon_Producto["EXISTENCIA_SISTEMA"] = Txt_Existencia.Text.Trim();
                            Renglon_Producto["CONTEO_FISICO"] = Txt_Conteo_Fisico.Text.Trim();
                            Renglon_Producto["PRECIO_PROMEDIO"] = Txt_Costo.Text.Trim();
                            Agregar_Quitar_Renglones_A_DataTable(((DataTable)Session[P_Dt_Productos_Ajustados]), Renglon_Producto, Operacion_Agregar_Renglon_Nuevo);
                            Grid_Productos_Ajustados.DataSource = (DataTable)Session[P_Dt_Productos_Ajustados];
                            Grid_Productos_Ajustados.DataBind();
                            Txt_Conteo_Fisico.Enabled = false;
                            Resumen_Movimientos();
                            Limpiar_Form();
                            Txt_Clave.Focus();
                        }
                    }
                    else                    
                    {
                        Mostrar_Informacion("El producto que desea agregar ya se encuentra en la lista", true);
                    }
                }
                else
                {
                    Mostrar_Informacion("No se encontraron diferencias para ajuste", true);
                }
            }
            else
            {
                Mostrar_Informacion("Debe Ingresar el Conteo Físico del Producto", true);
            }
        }
        else
        {
            Mostrar_Informacion("Debe buscar un producto", true);
        }
    }
    private void Resumen_Movimientos()
    {
        int Total_Ajustes = 0;
        int Total_Entradas = 0;
        int Total_Salidas = 0;
        int Total_Ajustes_Unidad = 0;
        int Total_Entradas_Unidad = 0;
        int Total_Salidas_Unidas = 0;
        double Total_Importe_Entradas = 0;
        double Total_Importe_Salidas = 0;
        double Saldo = 0;
        //int Total_Ajustes = 0;
        if (Session[P_Dt_Productos_Ajustados] != null)
        {
            DataTable Dt_Productos = Session[P_Dt_Productos_Ajustados] as DataTable;
            if (Dt_Productos != null && Dt_Productos.Rows.Count > 0)
            {
                foreach (DataRow Producto in Dt_Productos.Rows)
                {
                    Total_Ajustes++;
                    if (Producto["TIPO_MOVIMIENTO"].ToString().Trim() == "ENTRADA")
                    {
                        Total_Entradas++;
                        Total_Importe_Entradas += double.Parse(Producto["IMPORTE_DIFERENCIA"].ToString());
                        Total_Entradas_Unidad += int.Parse(Producto["DIFERENCIA"].ToString());
                    }
                    else
                    {
                        Total_Salidas++;
                        Total_Importe_Salidas += double.Parse(Producto["IMPORTE_DIFERENCIA"].ToString());
                        Total_Salidas_Unidas += Math.Abs(int.Parse(Producto["DIFERENCIA"].ToString()));
                    }
                }//for
            }
            else
            {
                Limpiar_Form();
            }
        }
        else
        {
            Limpiar_Form();
        }
        Lbl_Importe_Entradas.Text = Total_Importe_Entradas.ToString();
        Lbl_Total_Entradas.Text = Total_Entradas.ToString();
        Lbl_Entradas_Unidad.Text = Total_Entradas_Unidad.ToString();

        Lbl_Total_Salidas.Text = Total_Salidas.ToString();
        Lbl_Importe_Salidas.Text = Total_Importe_Salidas.ToString();
        Lbl_Salidas_Unidad.Text = Total_Salidas_Unidas.ToString();

        Lbl_Total_Ajustes.Text = Total_Ajustes.ToString();
        Total_Ajustes_Unidad = Total_Entradas_Unidad + Total_Salidas_Unidas;
        Lbl_Unidades_Ajustadas.Text = Total_Ajustes_Unidad.ToString();
        //Total_Ajustes_Unidad = Total_Entradas_Unidad + Total_Salidas_Unidas;
        Saldo = Total_Importe_Entradas - Total_Importe_Salidas;
        Lbl_Importe_Saldo.Text = Saldo.ToString();
        Lbl_Importe_Saldo.ForeColor = (Saldo > 0) ? System.Drawing.Color.Blue : System.Drawing.Color.Red;
    }
    private void Limpiar_Form() 
    {
        Txt_Importe.Text = "";
        Txt_Costo.Text = "";
        Txt_Diferencia.Text = "";
        Txt_Existencia.Text = "";
        Txt_Conteo_Fisico.Text = "";
        Txt_Clave.Text = "";
        Txt_Producto.Text = "";
        Txt_Descripcion.Text = "";
    }
    protected void Txt_Conteo_Fisico_TextChanged(object sender, EventArgs e)
    {
        int Diferencia = int.Parse(Txt_Conteo_Fisico.Text.Trim()) - int.Parse(Txt_Existencia.Text.Trim());
        Txt_Diferencia.Text = Diferencia.ToString();
        double Importe = (double.Parse(Txt_Costo.Text.Trim()) * Diferencia);
        Txt_Importe.Text = String.Format("{0:n}",Importe); 
        if (Diferencia > 0)
        {
            Txt_Diferencia.ForeColor =  System.Drawing.Color.Blue;
            Txt_Importe.ForeColor = System.Drawing.Color.Blue;
            Cmb_Movimiento.SelectedValue = "ENTRADA";
        }
        else 
        {
            Txt_Diferencia.ForeColor = System.Drawing.Color.Red;
            Txt_Importe.ForeColor =  System.Drawing.Color.Red;
            Cmb_Movimiento.SelectedValue = "SALIDA";
        }
        Btn_Agregar.Focus();
    }
    private DataTable Construir_Tabla_Productos_Ajustados()
    {
        DataTable Tabla = new DataTable();
        DataColumn Columna = null;
        DataTable Tabla_Base_Datos =
            Presidencia.Generar_Requisicion.
            Datos.Cls_Ope_Com_Requisiciones_Datos.
            Consultar_Columnas_De_Tabla_BD(Ope_Alm_Ajustes_Detalles.Tabla_Ope_Alm_Ajustes_Almacen);
        foreach (DataRow Renglon in Tabla_Base_Datos.Rows)
        {
            Columna = new DataColumn(Renglon["COLUMNA"].ToString(), System.Type.GetType("System.String"));
            Tabla.Columns.Add(Columna);
        }
        Columna = new DataColumn("CLAVE", System.Type.GetType("System.String"));
        Tabla.Columns.Add(Columna);
        return Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: VAgregar_Quitar_Renglones_A_DataTable
    ///DESCRIPCIÓN: 
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 1 Oct 2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Agregar_Quitar_Renglones_A_DataTable(DataTable _DataTable, DataRow _DataRow, String Operacion)
    {
        if (Operacion == Operacion_Agregar_Renglon_Nuevo)
        {
            _DataTable.Rows.Add(_DataRow);
        }
        else if (Operacion == Operacion_Agregar_Renglon_Copia)
        {
            _DataTable.ImportRow(_DataRow);
            _DataTable.AcceptChanges();
        }
        else if (Operacion == Operacion_Quitar_Renglon)
        {
            ((DataTable)Session[P_Dt_Productos_Ajustados]).Rows.Remove(_DataRow);
        }
        return _DataTable;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Mostrar_Informacion
    ///DESCRIPCIÓN: 
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 1 Oct 2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Lbl_Informacion.Style.Add("color", "#990000");
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion.Text = txt;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Almacen/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/" + Nombre_PDF;
        Mostrar_Reporte(Nombre_PDF, "PDF");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

}
