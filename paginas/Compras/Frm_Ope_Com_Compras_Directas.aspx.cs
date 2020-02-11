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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Compra_Directa.Negocio;
using System.Collections.Generic;
using Presidencia.Generar_Requisicion.Negocio;

public partial class paginas_Compras_Frm_Ope_Com_Compras_Directas : System.Web.UI.Page
{
    #region ATRIBUTOS

    private Cls_Ope_Com_Compra_Directa_Negocio Negocio_Compras;
    private static DataTable P_Dt_Requisiciones;
    private static DataTable P_Dt_Detalles_Requisicion;
    private static DataTable P_Dt_Proveedores_Giro;
    private static DataTable P_Dt_Partidas;
    private static String P_Prod_Serv_ID;
    private const String INICIAL = "inicial";
    private const String NUEVO = "nuevo";
    private const String MODIFICAR = "modificar";
    private const String Operacion_Comprometer = "COMPROMETER";
    private const String Operacion_Descomprometer = "DESCOMPROMETER";    

    #endregion

    #region LOAD/INIT
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Txt_Estatus.Text = "PROCESAR";
            Txt_Tipo.Text = "TRANSITORIA";
            Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Llenar_Combos_Generales();
            Cmb_Tipo_Articulo.SelectedIndex = 0;
            Llenar_Grid_Requisiciones_Filtradas();
            Manejo_Controles(INICIAL);
            ViewState["SortDirection"] = "DESC";
        }
        Mostrar_Informacion("", false);
        Agregar_Tooltip_Combo(Cmb_Giros);
        Agregar_Tooltip_Combo(Cmb_Proveedor);        
    }
    #endregion

    #region EVENTOS
    private bool Validar_Informacion_Completa() 
    {
        bool respuesta = true;
        foreach (DataRow Renglon in P_Dt_Detalles_Requisicion.Rows)
        {
            if (Renglon["NOMBRE_PROVEEDOR"].ToString().Length == 0 ||
                    Renglon["PRECIO_U_SIN_IMP_COTIZADO"].ToString().Length == 0 ||
                        Renglon["SUBTOTAL_COTIZADO"].ToString().Length == 0)
            {
                respuesta = false;
                break;
            }
        }
        return respuesta;
    }

    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        foreach(DataRow Renglon in P_Dt_Detalles_Requisicion.Rows)
        {
            if (Validar_Informacion_Completa())
            {
                Guardar_Orden_Compra_Directa();
                String Mensaje = "La Cotización fue registrada";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
                Llenar_Grid_Requisiciones_Filtradas();
                Manejo_Controles(INICIAL);               
            }
            else 
            {
                Mostrar_Informacion("Debe ingresar proveedores y precios cotizados de los articulos",true);
            }
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        if (Btn_Salir.ToolTip == "Cancelar")
        {
            Manejo_Controles(INICIAL);
        }
    }
    
    #endregion

    #region EVENTOS GRID
    #endregion

    #region METODOS
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Mostrar_Información
    //DESCRIPCIÓN: Llena las areas de texto con el registro seleccionado del grid
    //PARAMETROS: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 24/Agosto/2010 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Mostrar_Informacion(String txt, Boolean mostrar)
    {
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion.Text = txt;
    }
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    //DESCRIPCIÓN: Llena el combo de proveedores de acuerdo al Giro o 
    //Concepto Seleccionado
    //PARAMETROS: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 24/Agosto/2010 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combo_Proveedores(String Giro_ID)
    {
        Negocio_Compras = new Cls_Ope_Com_Compra_Directa_Negocio();
        Negocio_Compras.P_Giro_ID = Giro_ID;
        DataTable _DataTable = Negocio_Compras.Consultar_Proveedores_Por_Concepto();
        P_Dt_Proveedores_Giro = _DataTable;
        if (P_Dt_Proveedores_Giro != null && P_Dt_Proveedores_Giro.Rows.Count > 0)
        {
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Proveedor, _DataTable, 1, 0);
            Agregar_Tooltip_Combo(Cmb_Proveedor);
        }
        else
        {            
            Combo_Vacio(Cmb_Proveedor);
            Mostrar_Informacion("No existen proveedores con el 'Concepto' seleccionado. Verifique su catálogo de Proveedores",true);
        }
    }
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Combo_Vacio
    //DESCRIPCIÓN: Llena el combo solo con el ITEM de SELCCIONAR
    //Concepto Seleccionado
    //PARAMETROS: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 24/Agosto/2010 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Combo_Vacio(DropDownList Combo)
    {
        Combo.Items.Clear();
        Combo.Items.Add("<SELECCIONAR>");
        Combo.Items[0].Value = "0";
        Combo.Items[0].Selected = true;
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Productos_Requisas_Filtradas
    //  DESCRIPCIÓN: Este método obtiene un arreglo se String con todos los productos
    //  que tienen las requisas con estatus de filtradas
    /// PARAMETROS  : 
    /// CREO        : Gustavo Angeles Cruz
    /// FECHA_CREO  : 12-Enero-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public void Llenar_Grid_Requisiciones_Filtradas()
    {
        Negocio_Compras = new Cls_Ope_Com_Compra_Directa_Negocio();
        //Negocio_Consolidar.P_Requisas_Seleccionadas = Verifica_Requisas_Seleccionadas();
        Negocio_Compras.P_Estatus_Requisicion = Txt_Estatus.Text;//debe ser FILTRADA
        Negocio_Compras.P_Tipo_Requisicion = Txt_Tipo.Text;// "TRANSITORIO','AUTORIZADA','FILTRADA'";//debe ser solo transitorio
        Negocio_Compras.P_Tipo_Articulo = Cmb_Tipo_Articulo.SelectedValue.Trim();
        DataTable _DataTable = Negocio_Compras.Consultar_Requisiciones_Filtradas();
        if (_DataTable != null)
        {
            P_Dt_Requisiciones = _DataTable;
            Grid_Requisiciones.DataSource = _DataTable;
            Grid_Requisiciones.DataBind();
        }
        else
        {
            P_Dt_Requisiciones = null;
            Grid_Requisiciones.DataSource = null;
            Grid_Requisiciones.DataBind();
        }
    }
    private void Manejo_Controles(String modo)
    {
        switch (modo)
        {
            case INICIAL:
                Configuracion_Acceso("Frm_Ope_Com_Compras_Directas.aspx");
                Btn_Guardar.ToolTip = "Guardar Cotización";
                Btn_Guardar.Visible = false;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Salir.ToolTip = "Inicio";
                Cmb_Tipo_Articulo.Enabled = true;
                Div_Listado_Requisiciones.Visible = true;
                Div_Orden_Compra.Visible = false;
                Div_Filtros.Visible = true;

                
                break;
            case NUEVO:
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.Visible = true;
                Btn_Guardar.Visible = true;
                Div_Listado_Requisiciones.Visible = false;
                Div_Orden_Compra.Visible = true;
                Div_Filtros.Visible = false;

                Configuracion_Acceso("Frm_Ope_Com_Compras_Directas.aspx");
                break;
            case MODIFICAR:
                //Btn_Guardar.ToolTip = "Actualizar Consolidación";
                //Btn_Guardar.Visible = false;
                //Cmb_Tipo_Articulo.Enabled = false;
                //Btn_Ver_Consolidaciones.Visible = false;
                //Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //Btn_Salir.ToolTip = "Cancelar";
                break;
        }
    }
    #endregion

    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.DataSource = P_Dt_Requisiciones;
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataBind();        
    }
    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Listado_Requisiciones.Visible = false;
        Div_Orden_Compra.Visible = true;
                
        Txt_No_Orden_Compra.Text = "OC-" + Consecutivo_Orden_Compra() + "";
        HDF_No_Requisicion.Value = Grid_Requisiciones.SelectedDataKey["NO_REQUISICION"].ToString();

        String ID = Grid_Requisiciones.SelectedDataKey["NO_REQUISICION"].ToString();
        DataRow[] Dr_Requisicion = P_Dt_Requisiciones.Select("NO_REQUISICION = '" + ID + "'");
        Txt_No_Requisicion.Text = Dr_Requisicion[0]["FOLIO"].ToString();
        Txt_Dependencia.Text = Dr_Requisicion[0]["NOMBRE_DEPENDENCIA"].ToString();
        Txt_Subtotal.Text = Dr_Requisicion[0]["SUBTOTAL"].ToString();
        Txt_IEPS.Text = Dr_Requisicion[0]["IEPS"].ToString();
        Txt_IVA.Text = Dr_Requisicion[0]["IVA"].ToString();
        Txt_Total_Requisicion.Text = Dr_Requisicion[0]["TOTAL"].ToString();

        Txt_Subtotal_Cotizado.Text = Dr_Requisicion[0]["SUBTOTAL_COTIZADO"].ToString();
        Txt_IEPS_Cotizado.Text = Dr_Requisicion[0]["IEPS_COTIZADO"].ToString();
        Txt_IVA_Cotizado.Text = Dr_Requisicion[0]["IVA_COTIZADO"].ToString();
        Txt_Total_Cotizado.Text = Dr_Requisicion[0]["TOTAL_COTIZADO"].ToString();                

        Negocio_Compras = new Cls_Ope_Com_Compra_Directa_Negocio();
        Negocio_Compras.P_No_Requisicion = ID;

        DataTable Dt_Articulos = Negocio_Compras.Consultar_Articulos_Requisiciones_Filtradas();
        if (Dt_Articulos != null)
        {
            P_Dt_Detalles_Requisicion = Dt_Articulos;
            Grid_Productos_Requisiciones.DataSource = Dt_Articulos;
            Grid_Productos_Requisiciones.DataBind();
            DataTable Dt_Giros = Consultar_Conceptos();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Giros, Dt_Giros, 3, 0);
            Agregar_Tooltip_Combo(Cmb_Giros);
        }
        else
        {
            P_Dt_Detalles_Requisicion = null;
            Grid_Productos_Requisiciones.DataSource = null;
            Grid_Productos_Requisiciones.DataBind();
        }       
        //Llenar P_Dt_Partidas para consultar su Disponible
        String Str_Partidas_IDs = "";               
        if (P_Dt_Detalles_Requisicion != null && P_Dt_Detalles_Requisicion.Rows.Count > 0)
        {
            //Recorrer P_Dt_Productos_Servicios para obtener las partidas y los producos
            foreach (DataRow Row in P_Dt_Detalles_Requisicion.Rows)
            {
                Str_Partidas_IDs = Str_Partidas_IDs + Row["PARTIDA_ID"].ToString() + ",";
            }
            if (Str_Partidas_IDs.Length > 0)
            {
                Str_Partidas_IDs = Str_Partidas_IDs.Substring(0, Str_Partidas_IDs.Length - 1);
            }
        }
        //Llenar DataTable P_Dt_Productos
        Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        //Llenar DataTable P_Dt_Partidas
        Requisicion_Negocio.P_Fuente_Financiamiento =
            P_Dt_Detalles_Requisicion.Rows[0]["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim();

        Requisicion_Negocio.P_Dependencia_ID =
            Dr_Requisicion[0]["DEPENDENCIA_ID"].ToString().Trim();
            //Requisicion[0][Ope_Com_Requisiciones.Campo_Dependencia_ID].ToString().Trim();

        Requisicion_Negocio.P_Proyecto_Programa_ID =
            P_Dt_Detalles_Requisicion.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString();

        //Se llena el combo partidas        

        //Consultar las partidas usadas 1
        Requisicion_Negocio.P_Partida_ID = Str_Partidas_IDs;
        Requisicion_Negocio.P_Anio_Presupuesto = DateTime.Now.Year;
        //Consultar las partidas usadas 2
        P_Dt_Partidas = Requisicion_Negocio.Consultar_Presupuesto_Partidas();
        //Llenar Combo Partidas con el evento del Combo Programas
        //DataTable Data_Table = Requisicion_Negocio.Consultar_Partidas_De_Un_Programa();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partidas, P_Dt_Partidas, 3, 0);
        Cmb_Partidas.SelectedIndex = 0;

        Manejo_Controles(NUEVO);
    }
    protected void Grid_Productos_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Modificar_Productos.Visible = true;
        String ID = Grid_Productos_Requisiciones.SelectedDataKey["PROD_SERV_ID"].ToString();
        P_Prod_Serv_ID = ID;
        DataRow[] Dr_Producto = P_Dt_Detalles_Requisicion.Select("PROD_SERV_ID = '" + ID + "'");
        Txt_Producto.Text = Dr_Producto[0]["NOMBRE_PRODUCTO_SERVICIO"].ToString();
        Txt_Precio_Unitario.Text = Dr_Producto[0]["PRECIO_UNITARIO"].ToString();
        //Selccionamos en el combo la partida
        Cmb_Partidas.Enabled = true;
        Cmb_Partidas.SelectedValue = Dr_Producto[0]["PARTIDA_ID"].ToString().Trim();
        Cmb_Partidas.Enabled = false;
        DataRow []Partida = P_Dt_Partidas.Select("PARTIDA_ID ='" + Cmb_Partidas.SelectedValue + "'");
        Lbl_Disponible_Partida.Text = "$ " + Partida[0]["MONTO_DISPONIBLE"].ToString();
    }
    protected void Grid_Productos_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos_Requisiciones.DataSource = P_Dt_Detalles_Requisicion;
        Grid_Productos_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Productos_Requisiciones.DataBind();
    }

    private int Consecutivo_Orden_Compra() 
    { 
        return Presidencia.Orden_Compra.Datos.
            Cls_Ope_Com_Orden_Compra_Datos.
            Obtener_Consecutivo(Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra, Ope_Com_Ordenes_Compra.Tabla_Ope_Com_Ordenes_Compra);
    }

    private DataTable Consultar_Conceptos() 
    {        

        String Lista_Partidas = "'";
        foreach(DataRow Renglon in P_Dt_Detalles_Requisicion.Rows)
        {
            String Partida_ID = Renglon["PARTIDA_ID"].ToString().Trim();
            Lista_Partidas = Lista_Partidas + Partida_ID + "','";
        }
        if (Lista_Partidas.Length > 0)
        {
            Lista_Partidas = Lista_Partidas + "#";
            Lista_Partidas = Lista_Partidas.Replace("','#", "'");
        }
        Negocio_Compras.P_Lista_Partidas = Lista_Partidas;
        DataTable Dt_Conceptos = Negocio_Compras.Consultar_Conceptos_De_Partidas();
        return Dt_Conceptos;
    }
    protected void Cmb_Giros_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Giro_ID = Cmb_Giros.SelectedValue.Trim();
        if (Cmb_Giros.SelectedIndex > 0)
        {
            Llenar_Combo_Proveedores(Giro_ID);
        }
        else
        {
            P_Dt_Proveedores_Giro = null;
            Combo_Vacio(Cmb_Proveedor);
        }
    }

    protected void Btn_Actualizar_Proveedor_Click(object sender, ImageClickEventArgs e)
    {
        if (Cmb_Giros.SelectedIndex == 0)
        {
            Mostrar_Informacion("Debe seleccionar un Concepto de la lista", true);
        }
        else 
        {
            if (Cmb_Proveedor.SelectedIndex == 0)
            {
                Mostrar_Informacion("Debe seleccionar un Proveedor de la lista", true);
            }
            else
            {
                foreach(DataRow Renglon in P_Dt_Detalles_Requisicion.Rows)
                {
                    if(Renglon["CONCEPTO_ID"].ToString().Trim() == Cmb_Giros.SelectedValue.Trim())
                    {
                        Renglon["PROVEEDOR_ID"] = Cmb_Proveedor.SelectedValue.Trim();
                        Renglon["NOMBRE_PROVEEDOR"] = 
                                P_Dt_Proveedores_Giro.Rows[Cmb_Proveedor.SelectedIndex -1]["NOMBRE"].ToString();
                    }
                }
                Grid_Productos_Requisiciones.DataSource = P_Dt_Detalles_Requisicion;
                Grid_Productos_Requisiciones.DataBind();
                //Combo_Vacio(Cmb_Proveedor);
            }
        }
    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Combos_Generales()
    // DESCRIPCIÓN: Llena los combos principales de la interfaz de usuario
    // PARAMETROS: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: Diciembre/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combos_Generales()
    {
        if (Cmb_Tipo_Articulo.Items.Count == 0)
        {
            Cmb_Tipo_Articulo.Items.Add("PRODUCTO");
            Cmb_Tipo_Articulo.Items.Add("SERVICIO");
            Cmb_Tipo_Articulo.Items[0].Selected = true;
        }
    }
    protected void Btn_Actualizar_Precio_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Precio_Unitario.Text.Trim().Length == 0)
        {
            Mostrar_Informacion("Debe ingresar el precio unitario!", true);
            return;
        }
        DataRow[] Dr_Producto = P_Dt_Detalles_Requisicion.Select("PROD_SERV_ID = '" + P_Prod_Serv_ID + "'");
        double Precio_Unitario = double.Parse(Txt_Precio_Unitario.Text.Trim());
        int Cantidad = int.Parse(Dr_Producto[0]["CANTIDAD"].ToString().Trim());
        double Porcentaje_Iva = double.Parse(Dr_Producto[0]["PORCENTAJE_IVA"].ToString().Trim());
        double Porcentaje_IEPS = double.Parse(Dr_Producto[0]["PORCENTAJE_IEPS"].ToString().Trim());

        double Monto_Total = double.Parse(Dr_Producto[0]["MONTO_TOTAL"].ToString().Trim());
        double Monto_IVA = double.Parse(Dr_Producto[0]["MONTO_IVA"].ToString().Trim());
        double Monto_IEPS = double.Parse(Dr_Producto[0]["MONTO_IEPS"].ToString().Trim());
        double Monto = double.Parse(Dr_Producto[0]["MONTO_TOTAL"].ToString().Trim());

        double Monto_Total_Cot = 0;
        double Monto_IVA_Cot = 0;
        double Monto_IEPS_Cot = 0;
        double Monto_Cot = Precio_Unitario * Cantidad;

        double Monto_Comprometer_Descomprometer = 0.0;
        String Str_Disponible = Lbl_Disponible_Partida.Text.Trim();
        Str_Disponible = Str_Disponible.Replace("$","");
        Str_Disponible = Str_Disponible.Trim();
        double Disponible_Partida = double.Parse(Str_Disponible);

        if (Porcentaje_IEPS != 0)
        {
            Monto_IEPS_Cot = Monto_Cot * Porcentaje_IEPS / 100;
        }
        Monto_Total_Cot = Monto_Cot + Monto_IEPS_Cot;

        if (Porcentaje_Iva != 0)
        {
            Monto_IVA_Cot = Monto_Total_Cot * Porcentaje_Iva / 100;
        }
        
        
        Monto_Total_Cot = Monto_Total_Cot + Monto_IVA_Cot;

        Monto_Cot = Formato_Double(Monto_Cot);
        Monto_IVA_Cot = Formato_Double(Monto_IVA_Cot);
        Monto_IEPS_Cot = Formato_Double(Monto_IEPS_Cot);
        Monto_Total_Cot = Formato_Double(Monto_Total_Cot);

        Monto_Comprometer_Descomprometer = Monto_Total_Cot - Monto_Total;

        if (Disponible_Partida >= Monto_Comprometer_Descomprometer)
        {

            if (Monto_Comprometer_Descomprometer < 0)
            {
                Monto_Comprometer_Descomprometer = Monto_Comprometer_Descomprometer * (-1);
                Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
                    Cmb_Partidas.SelectedValue.Trim(), P_Dt_Partidas,
                    Operacion_Descomprometer, Monto_Comprometer_Descomprometer);
            }
            else if (Monto_Comprometer_Descomprometer > 0)
            {
                Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
                    Cmb_Partidas.SelectedValue.Trim(), P_Dt_Partidas,
                    Operacion_Comprometer, Monto_Comprometer_Descomprometer);
            }


            Dr_Producto[0]["PRECIO_U_SIN_IMP_COTIZADO"] = Txt_Precio_Unitario.Text;
            int index = Cmb_Proveedor.SelectedIndex - 1;
            Dr_Producto[0]["SUBTOTAL_COTIZADO"] = Monto_Cot + "";
            Dr_Producto[0]["IVA_COTIZADO"] = Monto_IVA_Cot + "";
            Dr_Producto[0]["IEPS_COTIZADO"] = Monto_IEPS_Cot + "";
            Dr_Producto[0]["TOTAL_COTIZADO"] = Monto_Total_Cot + "";

            double Suma_Subtotal = 0;
            double Suma_IEPS = 0;
            double Suma_IVA = 0;
            double Suma_Total = 0;
            //String str = String.Format({});

            foreach (DataRow Renglon in P_Dt_Detalles_Requisicion.Rows)
            {

                Suma_Subtotal +=
                    Convert.ToDouble(
                        Renglon["SUBTOTAL_COTIZADO"].ToString() != null &&
                        Renglon["SUBTOTAL_COTIZADO"].ToString().Trim().Length > 0 ?
                        Renglon["SUBTOTAL_COTIZADO"].ToString().Trim() : "0");
                Suma_IEPS +=
                    Convert.ToDouble(
                        Renglon["IEPS_COTIZADO"].ToString() != null &&
                        Renglon["IEPS_COTIZADO"].ToString().Trim().Length > 0 ?
                        Renglon["IEPS_COTIZADO"].ToString().Trim() : "0");
                Suma_IVA +=
                        Convert.ToDouble(
                        Renglon["IVA_COTIZADO"].ToString() != null &&
                        Renglon["IVA_COTIZADO"].ToString().Trim().Length > 0 ?
                        Renglon["IVA_COTIZADO"].ToString().Trim() : "0");
                Suma_Total += Convert.ToDouble(
                        Renglon["TOTAL_COTIZADO"].ToString() != null &&
                        Renglon["TOTAL_COTIZADO"].ToString().Trim().Length > 0 ?
                        Renglon["TOTAL_COTIZADO"].ToString().Trim() : "0");
            }

            Txt_Subtotal_Cotizado.Text = Suma_Subtotal + "";
            Txt_IEPS_Cotizado.Text = Suma_IEPS + "";
            Txt_IVA_Cotizado.Text = Suma_IVA + "";
            Txt_Total_Cotizado.Text = Suma_Total + "";
            Grid_Productos_Requisiciones.DataSource = P_Dt_Detalles_Requisicion;
            Grid_Productos_Requisiciones.DataBind();
            Txt_Precio_Unitario.Text = "";
            Txt_Producto.Text = "";
        }
        else 
        {
            Mostrar_Informacion("No existe presupuesto suficiente, Verifique con la Unidad Responsable",true);
        }
        Div_Modificar_Productos.Visible = false;        
    }
    public int Guardar_Orden_Compra_Directa() 
    {
        //Negocio_Compras = new Cls_Ope_Com_Compra_Directa_Negocio();
        //Negocio_Compras.P_Dt_Detalles_Orden_Compra = P_Dt_Detalles_Requisicion;
        ////Negocio_Compras.P_Estatus_Orden_Compra = "GENERADA";
        //Negocio_Compras.P_No_Requisicion = HDF_No_Requisicion.Value;
        //Negocio_Compras.P_Subtotal_Cotizado = Convert.ToDouble(Txt_Subtotal_Cotizado.Text.Trim());
        //Negocio_Compras.P_IEPS_Cotizado = Convert.ToDouble(Txt_IEPS_Cotizado.Text.Trim());
        //Negocio_Compras.P_IVA_Cotizado = Convert.ToDouble(Txt_IVA_Cotizado.Text.Trim());
        //Negocio_Compras.P_Total_Cotizado = Convert.ToDouble(Txt_Total_Cotizado.Text.Trim());
        //Negocio_Compras.Guardar_Actualizacion_Precios_Proveedor();

        //Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        //Requisicion_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado;

        //Requisicion_Negocio.P_Fuente_Financiamiento = 
        //    P_Dt_Detalles_Requisicion.Rows[0]["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim();
        //Requisicion_Negocio.P_Proyecto_Programa_ID =
        //    P_Dt_Detalles_Requisicion.Rows[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim();
        //Requisicion_Negocio.P_Anio_Presupuesto = DateTime.Now.Year;
        //Requisicion_Negocio.P_Dt_Partidas = P_Dt_Partidas;
        //Requisicion_Negocio.Comprometer_Presupuesto_Partidas_Usadas_En_Requisicion();
        return 1;
    }
    protected void Cmb_Tipo_Articulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Grid_Requisiciones_Filtradas();
    }
    private void Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
        String Partida_ID, DataTable Partidas, String Operacion, double Cantidad)
    {
        double Disponible = 0;
        double Comprometido = 0;
        try
        {
            DataRow[] Renglones = Partidas.Select("Partida_ID = '" + Partida_ID + "'");
            //Si encuentra el presupuesto
            if (Renglones.Length > 0)
            {
                Disponible = double.Parse(Renglones[0]["MONTO_DISPONIBLE"].ToString());
                Comprometido = double.Parse(Renglones[0]["MONTO_COMPROMETIDO"].ToString());
                if (Operacion == Operacion_Comprometer)
                {
                    Disponible = Disponible - Cantidad;
                    Comprometido = Comprometido + Cantidad;
                }
                else if (Operacion == Operacion_Descomprometer)
                {
                    Disponible = Disponible + Cantidad;
                    Comprometido = Comprometido - Cantidad;
                }
                Renglones[0]["MONTO_DISPONIBLE"] = Disponible.ToString();
                Renglones[0]["MONTO_COMPROMETIDO"] = Comprometido.ToString();
                Lbl_Disponible_Partida.Text = "$ " + Disponible.ToString();
            }
        }
        catch (Exception Ex)
        {
            String Str = Ex.ToString();
            Mostrar_Informacion(Str, true);
        }
    }
    private double Formato_Double(double numero)
    {
        try
        {
            String Str_Numero = numero.ToString("##.##");
            numero = Convert.ToDouble(Str_Numero);
        }
        catch (Exception Ex)
        {
            String Str = Ex.ToString();
            numero = 0;
        }
        return numero;
    }
    private void Agregar_Tooltip_Combo(DropDownList Combo)
    {
        foreach (ListItem Item in Combo.Items)
        {
            Item.Attributes.Add("Title", Item.Text);
        }
    }

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Guardar);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    /// ******************************************************************************************
    /// NOMBRE: Grid_Requisiciones_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// ******************************************************************************************
    protected void Grid_Requisiciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Grid_Sorting(Grid_Requisiciones, P_Dt_Requisiciones, e);
    }

    /// *****************************************************************************************
    /// NOMBRE: Grid_Sorting
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// CREÓ: Gustavo Angeles Cruz
    /// FECHA CREÓ: 11/Junio/2011
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Grid_Sorting(GridView Grid, DataTable Dt_Table, GridViewSortEventArgs e)
    {
        if (Dt_Table != null)
        {
            DataView Dv_Vista = new DataView(Dt_Table);
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Dv_Vista.Sort = e.SortExpression + " DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Vista.Sort = e.SortExpression + " ASC";
                ViewState["SortDirection"] = "ASC";
            }
            Grid.DataSource = Dv_Vista;
            Grid.DataBind();
        }
    }

    #endregion
}
