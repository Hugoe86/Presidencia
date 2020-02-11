//using Presidencia.Compras_Operacion_Gasto.Negocio;
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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Generar_Requisicion.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;
using Presidencia.Administrar_Requisiciones.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Registro_Gastos.Negocio;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Com_Registrar_Gastos : System.Web.UI.Page
{
    #region VARIABLES / CONSTANTES
    //objeto de la clase de negocio de dependencias para acceder a la clase de datos y realizar copnexion
    private Cls_Cat_Dependencias_Negocio Dependencia_Negocio;
    //objeto de la clase de negocio de Requisicion para acceder a la clase de datos y realizar copnexion
    private Cls_Ope_Com_Registro_Gastos_Negocio Gastos_Negocio;
    //Cls_Ope_Com_Registro_Gastos_Negocio
    //objeto en donde se guarda un id de producto o servicio para siempre tener referencia
    //private static String PS_ID;

    private Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion;
    private int Contador_Columna;
    private String Informacion;

    private static String P_Dt_Productos_Servicios = "P_Dt_Productos_Servicios_Gasto";
    private static String P_Dt_Partidas = "P_Dt_Partidas_Gasto";
    private static String P_Dt_Requisiciones = "P_Dt_Requisiciones_Gasto";

    private const String Operacion_Comprometer = "COMPROMETER";
    private const String Operacion_Descomprometer = "DESCOMPROMETER";
    private const String Operacion_Quitar_Renglon = "QUITAR";
    private const String Operacion_Agregar_Renglon_Nuevo = "AGREGAR_NUEVO";
    private const String Operacion_Agregar_Renglon_Copia = "AGREGAR_COPIA";

    private const String SubFijo_Requisicion = "GT-";
    //private static DataTable P_Dt_Requisiciones;
    #endregion

    #region PAGE LOAD / INIT
    protected void Page_Load(object sender, EventArgs e)
    {
        //Valores de primera vez
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "DESC";
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Inicial.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            //llenar combo dependencias
            Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia, Dt_Dependencias, 1, 0);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Panel, Dt_Dependencias, 1, 0);
            Cmb_Dependencia.SelectedIndex = 0;
            Cmb_Dependencia_Panel.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            Cmb_Dependencia_Panel.Enabled = false;
            Llenar_Combos_Busqueda();
            Llenar_Grid_Gastos();
            Llenar_Combos_Generales();
            Habilitar_Controles("Uno");
        }
        //Tooltips
        Agregar_Tooltip_Combo(Cmb_Fte_Financiamiento);
        Agregar_Tooltip_Combo(Cmb_Programa);
        Agregar_Tooltip_Combo(Cmb_Partida);

        Mostrar_Informacion("", false);
        //Limpiar_Formulario();
    }
    #endregion
    private void Construir_DataTables()
    {
        Session[P_Dt_Productos_Servicios] = Construir_Tabla_Detalles_Requisicion();
        Session[P_Dt_Partidas] = Construir_Tabla_Presupuestos();
    }
    //#region EVENTOS
    //Boton NUEVO de la Requisicion en la barra de herramientas
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Nuevo.ToolTip == "Nuevo")
        {
            Construir_DataTables();
            Limpiar_Grids_Y_DataTables();            
            Limpiar_Cajas_Texto();
            Llenar_Combos_Generales();
            Habilitar_Controles("Nuevo");
            Div_Listado_Requisiciones.Visible = false;
            Div_Contenido.Visible = true;
            Construir_DataTables();
            Actualizar_Grid_Partidas_Productos();
        }
        else if (Btn_Nuevo.ToolTip == "Dar de Alta")
        {
            if (Validaciones(true))
            {
                //AGREGAR VALORES A CAPA DE NEGOCIO Y LLAMAR INSERTAR
                Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
                //Gastos_Negocio.P_Comentarios = Txt_Comentario.Text;
                Gastos_Negocio.P_Folio = Txt_Folio.Text;
                Gastos_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
                Gastos_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;                
                Gastos_Negocio.P_Subtotal = Txt_Subtotal.Text;
                Gastos_Negocio.P_Total = Txt_Total.Text;
                Gastos_Negocio.P_IVA = Txt_IVA.Text;
                Gastos_Negocio.P_IEPS = Txt_IEPS.Text;
                Gastos_Negocio.P_Dt_Productos_Servicios = (DataTable)Session[P_Dt_Productos_Servicios];
                Gastos_Negocio.P_Justificacion_Compra = Txt_Justificacion.Text;
                Gastos_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue.Trim();
                Gastos_Negocio.P_Dt_Partidas = (DataTable)Session[P_Dt_Partidas];
                Gastos_Negocio.P_Fuente_Financiamiento = Cmb_Fte_Financiamiento.SelectedValue;
                Gastos_Negocio.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue;
                Gastos_Negocio.P_Anio_Presupuesto = DateTime.Now.Year;
                Gastos_Negocio.Proceso_Registrar_Gasto();
                ScriptManager.RegisterStartupScript(
                    this, this.GetType(), "Requisiciones", "alert('Gasto registrado');", true);
                Habilitar_Controles("Uno");
                Llenar_Grid_Gastos();
            }
            else
            {
                Mostrar_Informacion(Informacion, true);
            }
        }//fin if Nuevo
    }

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Modificar.ToolTip == "Modificar")
        {
            if (Txt_Folio.Text.Trim() != "")
            {
                if (Cmb_Estatus.SelectedValue == "CANCELADA")
                {
                    Mostrar_Informacion("No se puede modificar un gasto cancelado", true);
                }
                else
                {
                    Habilitar_Controles("Modificar");
                }
            }
            else
            {
                Mostrar_Informacion("Debe seleccionar un gasto de la lista", true);
            }
        }
        else if (Btn_Modificar.ToolTip == "Actualizar")
        {
            //Proceso para modificar
            if (Validaciones(true))
            {
                //Cargar datos Negocio
                Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
                Gastos_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                Gastos_Negocio.P_Subtotal = Txt_Subtotal.Text;
                Gastos_Negocio.P_Total = Txt_Total.Text;
                Gastos_Negocio.P_IVA = Txt_IVA.Text;
                Gastos_Negocio.P_IEPS = Txt_IEPS.Text;
                Gastos_Negocio.P_Dt_Productos_Servicios = (DataTable)Session[P_Dt_Productos_Servicios];
                Gastos_Negocio.P_Justificacion_Compra = Txt_Justificacion.Text;
                Gastos_Negocio.P_Gasto_ID = Txt_Folio.Text.Replace(SubFijo_Requisicion,"");
                Gastos_Negocio.P_Anio_Presupuesto = DateTime.Now.Year;
                if (Gastos_Negocio.P_Estatus == "CANCELADA")
                {
                    double Monto_Total = double.Parse(Txt_Total.Text.Trim());
                    String Partida_ID = Cmb_Partida.SelectedValue.Trim();
                    //Se descompromete los presupuestos en P_Dt_Partidas                                    
                    Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
                        Partida_ID, (DataTable)Session[P_Dt_Partidas], Operacion_Descomprometer, Monto_Total);
                }
                Gastos_Negocio.P_Dt_Partidas = (DataTable)Session[P_Dt_Partidas];
                Gastos_Negocio.Proceso_Actualizar_Gasto();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisiciones", "alert('El Gasto fué modificado');", true);
                Habilitar_Controles("Inicial");
            }
            else
            {
                Mostrar_Informacion(Informacion, true);
            }
        }
    }


    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Cancelar")
        {
            if (Btn_Nuevo.ToolTip == "Dar de Alta")
            {
                Limpiar_Formulario();
            }
            Habilitar_Controles("Inicial");
        }
        else
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }

    public void Evento_Combo_Dependencia()
    {
        //Cargar los programas de la dependencia seleccionada
        Cmb_Programa.Items.Clear();
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
        Gastos_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        DataTable Data_Table_Proyectos = Gastos_Negocio.Consultar_Proyectos_Programas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Programa, Data_Table_Proyectos);
        Cmb_Programa.SelectedIndex = 0;
        //Limpiar las partidas
        Cmb_Partida.Items.Clear();
        Cmb_Partida.Items.Add("<<SELECCIONAR>>");
        Cmb_Partida.SelectedIndex = 0;
    }

    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        Evento_Combo_Dependencia();
    }

    protected void Cmb_Programa_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Programa.SelectedIndex != 0)
        {
            Cmb_Partida.Items.Clear();
            Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
            Gastos_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            Gastos_Negocio.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue;
            DataTable Data_Table = Gastos_Negocio.Consultar_Partidas_De_Un_Programa();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partida, Data_Table, 1, 0);
            Cmb_Partida.SelectedIndex = 0;
            Lbl_Disponible_Partida.Text = "$ 0.00";
        }
    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Habilitar_Controles("Inicial");
        Llenar_Grid_Gastos();
    }

    private void Evento_Boton_Agregar_Producto()
    {
        Cmb_Fte_Financiamiento.Enabled = false;
        Cmb_Programa.Enabled = false;       
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
        //AGREGAR LOS DATOS AL RENGLON DE MI DATA SET DE PRODUCTOS
        DataRow Renglon_Producto = ((DataTable)Session[P_Dt_Productos_Servicios]).NewRow();
        //DataTable Dt_Conceptos = Cls_Util.Consultar_Conceptos_De_Partidas("'" + Cmb_Partida.SelectedValue + "'");

        double Importe = double.Parse(Txt_Cantidad.Text) * double.Parse(Txt_Precio_Unitario.Text);
        double Monto_IEPS = 0;
        double Monto_IVA = 0;
        String Porcentaje_IEPS = "0";
        String Porcentaje_IVA = "16";
        double Monto_Total = 0;
        Renglon_Producto["Producto_Servicio"] = Txt_Producto_Servicio.Text;
        Renglon_Producto["Costo"] = Txt_Precio_Unitario.Text;
        Renglon_Producto["Importe"] = Importe.ToString();
        Renglon_Producto["Cantidad"] = Txt_Cantidad.Text;
        Renglon_Producto["Identificador"] = Numero_Aleatorio();
        //Calculo IEPS
        if (Cmb_IEPS.SelectedValue.Trim() == "SI")
        {
            Monto_IEPS = (Importe * double.Parse(Porcentaje_IEPS)) / 100;
            Renglon_Producto["IEPS"] = Monto_IEPS;
        }
        else
        {
            Renglon_Producto["IEPS"] = 0;
        }
        //Calculo IVA
        if (Cmb_IVA.SelectedValue.Trim() == "SI")
        {
            Monto_IVA = ((Importe + Monto_IEPS) * double.Parse(Porcentaje_IVA)) / 100;
            Renglon_Producto["IVA"] = Monto_IVA;
        }
        else
        {
            Renglon_Producto["IVA"] = 0;
        }

        Monto_Total = Importe + Monto_IEPS + Monto_IVA;
        Renglon_Producto["MONTO_TOTAL"] = Monto_Total;
        double Disponible_Partida = Verifica_Disponible_De_Una_Partida_En_Dt_Partidas(
            Cmb_Partida.SelectedValue.Trim(),
            (DataTable)Session[P_Dt_Partidas],
            Cmb_Dependencia.SelectedValue.Trim(),
            Cmb_Fte_Financiamiento.SelectedValue.Trim(),
            Cmb_Programa.SelectedValue.Trim());
        if (Disponible_Partida >= Importe)
        {
            //Aqui se agrega el producto al DataSet Ds_Productos_Servicios
            Session[P_Dt_Productos_Servicios] = Agregar_Quitar_Renglones_A_DataTable(
                (DataTable)Session[P_Dt_Productos_Servicios], Renglon_Producto, Operacion_Agregar_Renglon_Nuevo);
            Refrescar_Grid();
            Calcular_Impuestos();
            //Comprometer Presupuesto en P_Dt_Partidas
            Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
                Cmb_Partida.SelectedValue.Trim(), (DataTable)Session[P_Dt_Partidas], Operacion_Comprometer, Monto_Total);
            Actualizar_Grid_Partidas_Productos();
        }
        else
        {
            Mostrar_Informacion("Presupuesto insuficiente en la partida: " + Cmb_Partida.SelectedItem.ToString(), true);
        }
        Txt_Precio_Unitario.Text = "";
        Txt_Cantidad.Text = "";
        Txt_Producto_Servicio.Text = "";
        Cmb_IVA.SelectedIndex = 0;
        Cmb_IEPS.SelectedIndex = 0;
    }

    //*********************************************************************************************
    //*********************************************************************************************
    //*********************************************************************************************
    #region PRESUPUESTOS
    //GAC
    private DataTable Verifica_Disponible_De_Una_Partida_En_BD(
        String Dependencia_ID,
        String Fte_Financiamiento_ID,
        String Programa_ID,
        String Partida_ID)
    {
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
        Gastos_Negocio.P_Dependencia_ID = Dependencia_ID;
        Gastos_Negocio.P_Fuente_Financiamiento = Fte_Financiamiento_ID;
        Gastos_Negocio.P_Proyecto_Programa_ID = Programa_ID;
        Gastos_Negocio.P_Partida_ID = Partida_ID;
        Gastos_Negocio.P_Anio_Presupuesto = DateTime.Now.Year;
        DataTable Dt_Partida = Gastos_Negocio.Consultar_Presupuesto_Partidas();
        try
        {
            if (Dt_Partida == null || Dt_Partida.Rows.Count <= 0)
            {
                Dt_Partida = null;
            }
        }
        catch (Exception Ex)
        {
            String Str = Ex.ToString();
            Mostrar_Informacion(Str, true);
            return null;
        }
        return Dt_Partida;
    }

    //GAC - Verifica el presupueto de una partida con 
    private double Verifica_Disponible_De_Una_Partida_En_Dt_Partidas(
        String Partida_ID,
        DataTable Partidas,
        String Dependencia_ID,
        String Fte_Financiamiento_ID,
        String Programa_ID)
    {
        double Disponible = 0;
        try
        {
            DataRow[] Renglones = Partidas.Select("Partida_ID = '" + Partida_ID + "'");
            //Si encuentra el presupuesto
            if (Renglones.Length > 0)
            {
                Disponible = double.Parse(Renglones[0]["MONTO_DISPONIBLE"].ToString());
            }//si no encuentra el presupuesto consulta en la base de datos
            else
            {
                DataTable _DataTable = Verifica_Disponible_De_Una_Partida_En_BD(
                    Dependencia_ID, Fte_Financiamiento_ID,
                    Programa_ID, Partida_ID);
                if (_DataTable != null)
                {
                    Disponible = double.Parse(_DataTable.Rows[0]["MONTO_DISPONIBLE"].ToString());
                    Agregar_Quitar_Renglones_A_DataTable(
                        (DataTable)Session[P_Dt_Partidas], _DataTable.Rows[0], Operacion_Agregar_Renglon_Copia);
                }
            }
        }
        catch (Exception Ex)
        {
            String Str = Ex.ToString();
            Mostrar_Informacion(Str, true);
            return -1;
        }
        return Disponible;
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
            }//si no encuentra el presupuesto consulta en la base de datos
        }
        catch (Exception Ex)
        {
            String Str = Ex.ToString();
            Mostrar_Informacion(Str, true);
        }
    }

    #endregion

    //*********************************************************************************************
    //*********************************************************************************************
    //*********************************************************************************************
    #region PRODUCTOS
    //GAC
    //private DataTable Verifica_Disponible_De_Un_Producto_En_BD(String Producto_ID)
    //{
    //    Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
    //    Gastos_Negocio.P_Producto_ID = Producto_ID;
    //    DataTable Producto = Gastos_Negocio.Consultar_Poducto_Por_ID();
    //    try
    //    {
    //        if (Producto == null || Producto.Rows.Count <= 0)
    //        {
    //            Producto = null;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        String Str = Ex.ToString();
    //        Mostrar_Informacion(Str, true);
    //        return null;
    //    }
    //    return Producto;
    //}

    //GAC
    //private int Verifica_Disponible_De_Un_Producto_De_Stock_En_Dt_Productos(String Producto_ID, DataTable Productos)
    //{
    //    int Disponible = 0;
    //    try
    //    {
    //        DataRow[] Renglones = Productos.Select("Producto_ID = '" + Producto_ID + "'");
    //        //Si encuentra el producto
    //        if (Renglones.Length > 0)
    //        {
    //            Disponible = int.Parse(Renglones[0]["DISPONIBLE"].ToString());
    //        }//si no encuentra el producto consulta en la base de datos
    //        else
    //        {
    //            DataTable _DataTable = Verifica_Disponible_De_Un_Producto_En_BD(Producto_ID);
    //            if (_DataTable != null)
    //            {
    //                Disponible = int.Parse(_DataTable.Rows[0]["DISPONIBLE"].ToString());
    //                //Agrego los productos consultados al DataTable de Productos
    //                P_Dt_Productos.ImportRow(_DataTable.Rows[0]);
    //                P_Dt_Productos.AcceptChanges();
    //            }
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        String Str = Ex.ToString();
    //        Mostrar_Informacion(Str, true);
    //        return -1;
    //    }
    //    return Disponible;
    //}

    //private void Comprometer_O_Descomprometer_Disponible_A_Productos_En_Dt_Productos(String Producto_ID, DataTable Productos, String Operacion, int Cantidad)
    //{
    //    int Disponible = 0;
    //    int Comprometido = 0;
    //    try
    //    {
    //        DataRow[] Renglones = Productos.Select("Producto_ID = '" + Producto_ID + "'");
    //        //Si encuentra el presupuesto
    //        if (Renglones.Length > 0)
    //        {
    //            Disponible = int.Parse(Renglones[0]["DISPONIBLE"].ToString());
    //            Comprometido = int.Parse(Renglones[0]["COMPROMETIDO"].ToString());
    //            if (Operacion == Operacion_Comprometer)
    //            {
    //                Disponible = Disponible - Cantidad;
    //                Comprometido = Comprometido + Cantidad;
    //            }
    //            else if (Operacion == Operacion_Descomprometer)
    //            {
    //                Disponible = Disponible + Cantidad;
    //                Comprometido = Comprometido - Cantidad;
    //            }
    //            Renglones[0]["DISPONIBLE"] = Disponible.ToString();
    //            Renglones[0]["COMPROMETIDO"] = Comprometido.ToString();
    //        }//si no encuentra el presupuesto consulta en la base de datos
    //    }
    //    catch (Exception Ex)
    //    {
    //        String Str = Ex.ToString();
    //        Mostrar_Informacion(Str, true);
    //    }
    //}


    #endregion
    //protected void IBtn_MDP_Prod_Serv_Buscar_Click(object sender, ImageClickEventArgs e)
    //{
        ////Limpiar_Modal_Produtos_Servicios();
        //DataTable Dt_Prod_Srv_Tmp = null;
        //Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();

        //Gastos_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
        //Gastos_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue;

        ////PRUEBA
        //Gastos_Negocio.P_Tipo = "STOCK";
        //Gastos_Negocio.P_Partida_ID = "0000000051";
        ////FIN PRUEBA

        //if (Txt_Nombre.Text.Trim().Length > 0)
        //{
        //    Gastos_Negocio.P_Nombre_Producto_Servicio = Txt_Nombre.Text;
        //}
        //if (Cmb_Tipo.SelectedValue == "STOCK")
        //{
        //    Grid_Productos_Servicios_Modal.Columns[4].Visible = true;
        //    Dt_Prod_Srv_Tmp = Gastos_Negocio.Consultar_Productos();
        //}
        //else if (Cmb_Tipo.SelectedValue == "TRANSITORIA")
        //{
        //    Grid_Productos_Servicios_Modal.Columns[4].Visible = false;
        //    if (Cmb_Producto_Servicio.SelectedValue == "PRODUCTO")
        //    {
        //        Dt_Prod_Srv_Tmp = Gastos_Negocio.Consultar_Productos();
        //    }
        //    else if (Cmb_Producto_Servicio.SelectedValue == "SERVICIO")
        //    {
        //        Gastos_Negocio.P_Partida_ID = "0000000218";
        //        Dt_Prod_Srv_Tmp = Gastos_Negocio.Consultar_Servicios();
        //    }
        //}

        //if (Dt_Prod_Srv_Tmp != null && Dt_Prod_Srv_Tmp.Rows.Count > 0)
        //{
        //    P_Dt_Productos_Servicios_Modal = Dt_Prod_Srv_Tmp;
        //    Grid_Productos_Servicios_Modal.DataSource = Dt_Prod_Srv_Tmp;
        //    Grid_Productos_Servicios_Modal.DataBind();
        //    Grid_Productos_Servicios_Modal.Visible = true;
        //}
        //Modal_Busqueda_Prod_Serv.Show();
    //}

    //protected void IBtn_MDP_Prod_Serv_Cerrar_Click(object sender, ImageClickEventArgs e)
    //{
    //    Limpiar_Modal_Produtos_Servicios();
    //}



    protected void Ibtn_Agregar_Producto_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Producto_Servicio.Text.Length > 0 && Txt_Cantidad.Text.Length > 0)
        {
            //Verificar si el producto o servicio ya se encuentra agregado
            //if (Busca_Productos_Servicios_Duplicados(PS_ID))
            //{
            //    Mostrar_Informacion("El Producto/Servicio ya se encuentra en la lista", true);
            //    return;
            //}
            Evento_Boton_Agregar_Producto();
        }
        else
        {
            Mostrar_Informacion("Debe buscar un producto o servicio para ser agregado e indicar la cantidad solicitada.", true);
        }
    }

    protected void Lnk_Observaciones_Click(object sender, EventArgs e)
    {
        if (Lnk_Observaciones.Text == "Mostrar")
        {
            Administrar_Requisicion = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
            Administrar_Requisicion.P_Requisicion_ID = Txt_Folio.Text.Replace(SubFijo_Requisicion, "");
            DataSet Data_Set = Administrar_Requisicion.Consulta_Observaciones();
            if (Data_Set != null && Data_Set.Tables[0].Rows.Count != 0)
            {
                Div_Comentarios.Visible = true;
                Grid_Comentarios.DataSource = Data_Set;
                Grid_Comentarios.DataBind();

            }
            Lnk_Observaciones.Text = "Ocultar";
        }
        else
        {
            Div_Comentarios.Visible = false;
            Grid_Comentarios.DataSource = null;
            Grid_Comentarios.DataBind();
            Lnk_Observaciones.Text = "Mostrar";
        }
    }



    //protected void Cmb_Producto_Servicio_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (Cmb_Producto_Servicio.SelectedValue == "SERVICIO")
    //    {
    //        Txt_Cantidad.Text = "1";
    //        Txt_Cantidad.Enabled = false;
    //        Lbl_Categoria.Text = "Servicio";
    //    }
    //    else
    //    {
    //        Lbl_Categoria.Text = "Producto";
    //        Txt_Cantidad.Enabled = true;
    //    }
    //}

    //protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    if (Cmb_Tipo.SelectedValue == "STOCK")
    //    {
    //        Cmb_Producto_Servicio.SelectedValue = "PRODUCTO";
    //        Cmb_Producto_Servicio.Enabled = false;
    //        Lbl_Categoria.Text = "Producto";
    //    }
    //    else
    //    {
    //        Cmb_Producto_Servicio.SelectedIndex = 0;
    //        Cmb_Producto_Servicio.Enabled = true;
    //    }
    //}

    //protected void Btn_Cerrar_Click(object sender, EventArgs e)
    //{
    //    Limpiar_Modal_Produtos_Servicios();
    //}

    protected void Btn_Listar_Requisiciones_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Gastos();
        Habilitar_Controles("Uno");
    }

    //#endregion

    #region EVENTOS GRID


    //Arreglado 31mar 2011
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.DataSource = (DataTable)Session[P_Dt_Requisiciones];
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataBind();
    }

    protected void Grid_Comentarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow Renglon = Grid_Comentarios.SelectedRow;
        Txt_Comentario.Text = Renglon.Cells[1].Text;
    }

    protected void Grid_Comentarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Administrar_Requisicion = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        Administrar_Requisicion.P_Requisicion_ID = Txt_Folio.Text.Replace(SubFijo_Requisicion, "");
        DataSet Data_Set = Administrar_Requisicion.Consulta_Observaciones();
        Grid_Comentarios.DataSource = Data_Set;
        Grid_Comentarios.PageIndex = e.NewPageIndex;
        Grid_Comentarios.DataBind();
    }
    protected void Btn_Seleccionar_Gasto_Click(object sender, ImageClickEventArgs e)
    {
        Construir_DataTables();
        Actualizar_Grid_Partidas_Productos();
        //DataTable Dt_Comentarios = (DataTable)Grid_Comentarios.DataSource;
        Habilitar_Controles("Inicial");
        //Construir_DataTables();
        Llenar_Combos_Generales();
        Div_Listado_Requisiciones.Visible = false;
        Div_Contenido.Visible = true;
        Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();

        String No_Requisicion = ((ImageButton)sender).CommandArgument;//Grid_Requisiciones.SelectedDataKey["Gasto_ID"].ToString();
        DataRow[] Requisicion = ((DataTable)Session[P_Dt_Requisiciones]).Select("Gasto_ID = " + No_Requisicion);

        Txt_Folio.Text = Requisicion[0][Ope_Com_Gastos.Campo_Folio].ToString();
        String Fecha = Requisicion[0][Ope_Com_Gastos.Campo_Fecha_Solicitud].ToString();
        Fecha = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Fecha));
        Txt_Fecha.Text = Fecha.ToUpper();
        //Seleccionar combo dependencia
        Cmb_Dependencia.SelectedValue =

            Requisicion[0][Ope_Com_Gastos.Campo_Dependencia_ID].ToString().Trim();
        //Seleccionar estatus
        Cmb_Estatus.SelectedValue =
            Requisicion[0][Ope_Com_Gastos.Campo_Estatus].ToString().Trim();
        //Poner Justificación
        Txt_Justificacion.Text =
            Requisicion[0][Ope_Com_Gastos.Campo_Justificacion].ToString();
        //Total de la requisición
        Txt_Total.Text =
            Requisicion[0][Ope_Com_Gastos.Campo_Costo_Total_Gasto].ToString();
        //LLenar DataTable P_Dt_Productos_Servicios
        Gastos_Negocio.P_Gasto_ID = No_Requisicion;
        Session[P_Dt_Productos_Servicios] = Gastos_Negocio.Consultar_Productos_Servicios();

        //String Str_Partidas_IDs = "";
        //String Str_Productos_IDs = "";
        if (Session[P_Dt_Productos_Servicios] != null && ((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Count > 0)
        {
            Grid_Productos_Servicios.DataSource = (DataTable)Session[P_Dt_Productos_Servicios];
            Grid_Productos_Servicios.DataBind();
            //Recorrer P_Dt_Productos_Servicios para obtener las partidas y los producos
            //    foreach (DataRow Row in P_Dt_Productos_Servicios.Rows)
            //    {
            //        Str_Partidas_IDs = Str_Partidas_IDs + Row["PARTIDA_ID"].ToString() + ",";
            ////        Str_Productos_IDs = Str_Productos_IDs + Row["PROD_SERV_ID"].ToString() + ",";
            //    }
            //    if (Str_Partidas_IDs.Length > 0)
            //    {
            //        Str_Partidas_IDs = Str_Partidas_IDs.Substring(0, Str_Partidas_IDs.Length - 1);
            //    }
            //    if (Str_Productos_IDs.Length > 0)
            //    {
            //        Str_Productos_IDs = Str_Productos_IDs.Substring(0, Str_Productos_IDs.Length - 1);
            //    }
        }
        //Llenar DataTable P_Dt_Productos
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();

        //Llenar DataTable P_Dt_Partidas
        Gastos_Negocio.P_Fuente_Financiamiento =
            Requisicion[0][Ope_Com_Gastos.Campo_Fuente_Financiamiento_ID].ToString().Trim();

        Cmb_Fte_Financiamiento.SelectedValue =
            Requisicion[0][Ope_Com_Gastos.Campo_Fuente_Financiamiento_ID].ToString().Trim();

        Gastos_Negocio.P_Dependencia_ID =
            Requisicion[0][Ope_Com_Gastos.Campo_Dependencia_ID].ToString().Trim();

        Gastos_Negocio.P_Proyecto_Programa_ID =
            Requisicion[0][Ope_Com_Gastos.Campo_Proyecto_Programa_ID].ToString().Trim();

        //Se llena el combo partidas
        Cmb_Programa.SelectedValue =
            Requisicion[0][Ope_Com_Gastos.Campo_Proyecto_Programa_ID].ToString().Trim();

        //Llenar Combo Partidas con el evento del Combo Programas

        //Consultar las partidas usadas 1
        Gastos_Negocio.P_Partida_ID =
            Requisicion[0][Ope_Com_Gastos.Campo_Partida_ID].ToString().Trim();

        Gastos_Negocio.P_Anio_Presupuesto = DateTime.Now.Year;
        //Consultar las partidas usadas 2
        Session[P_Dt_Partidas] = Gastos_Negocio.Consultar_Presupuesto_Partidas();

        //Llenar Combo Partidas con el evento del Combo Programas
        DataTable Data_Table = Gastos_Negocio.Consultar_Partidas_De_Un_Programa();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Partida, Data_Table, 1, 0);
        Cmb_Partida.SelectedValue = Requisicion[0][Ope_Com_Gastos.Campo_Partida_ID].ToString().Trim();
        Actualizar_Grid_Partidas_Productos();
        ////Poner último comentario        
    }

    protected void Btn_Grid_Gastos_Eliminar_Prod_Serv_Click(object sender, ImageClickEventArgs e)
    {
        String Identificador = ((ImageButton)sender).CommandArgument;
        DataRow[] _DataRow = ((DataTable)Session[P_Dt_Productos_Servicios]).Select("IDENTIFICADOR = '" + Identificador + "'");
        if (_DataRow != null && _DataRow.Length > 0)
        {
            //Se descompromete los presupuestos en P_Dt_Partidas  
            String Partida_ID = Cmb_Partida.SelectedValue.Trim();
            Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
                Partida_ID, (DataTable)Session[P_Dt_Partidas], Operacion_Descomprometer, 2);
            ((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Remove(_DataRow[0]);
            ((DataTable)Session[P_Dt_Productos_Servicios]).AcceptChanges();
            Refrescar_Grid();
            Calcular_Impuestos();
            //Se actualizan los grids de partidas y productos disponibles
            Actualizar_Grid_Partidas_Productos();
        }
    }

    #endregion

    #region METODOS

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha()
    {
        bool Respuesta = false;
        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        try
        {
            //Convertimos el Texto de los TextBox fecha a dateTime
            Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
            Date2 = DateTime.Parse(Txt_Fecha_Final.Text);
            if (Date1 <= Date2)
            {
                Respuesta = true;
            }
        }
        catch (Exception e)
        {
            String str = e.ToString();
            Respuesta = false;
        }
        return Respuesta;
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Habilitar_Controles
    // DESCRIPCIÓN: Habilita la configuracion de acuerdo a la operacion     
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: 30/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO
    // CAUSA_MODIFICACIÓN   
    // *******************************************************************************/
    private void Habilitar_Controles(String Modo)
    {
        try
        {
            switch (Modo)
            {
                case "Uno":
                    Btn_Nuevo.Visible = true;
                    Configuracion_Acceso("Frm_Ope_Com_Registrar_Gastos.aspx");
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Eliminar.ToolTip = "Eliminar";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Listar_Requisiciones.Visible = false;
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Txt_Fecha.Enabled = false;
                    Txt_Folio.Enabled = false;
                    Cmb_Dependencia.Enabled = false;
                    //Cmb_Area.Enabled = false;
                    Cmb_Programa.Enabled = false;
                    Cmb_Partida.Enabled = false;
                    //Cmb_Tipo.Enabled = false;
                    Cmb_Estatus.Enabled = false;
                    //Ibtn_Buscar_Producto.Enabled = false;
                    Ibtn_Agregar_Producto.Enabled = false;
                    Grid_Productos_Servicios.Enabled = false;
                   // Cmb_Producto_Servicio.Enabled = false;
                    Txt_Total.Enabled = false;
                    //Txt_Producto_Servicio.Enabled = false;
                    Txt_Comentario.Enabled = false;
                    Txt_Cantidad.Enabled = false;
                    Txt_Justificacion.Enabled = false;
                    //Txt_Especificaciones.Enabled = false;
                    //Chk_Verificar.Enabled = false;
                    break;

                case "Inicial":
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Configuracion_Acceso("Frm_Ope_Com_Registrar_Gastos.aspx");
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Eliminar.ToolTip = "Eliminar";
                    Btn_Salir.ToolTip = "Inicio";

                    Btn_Listar_Requisiciones.Visible = true;
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                    Txt_Fecha.Enabled = false;
                    Txt_Folio.Enabled = false;
                    Cmb_Dependencia.Enabled = false;
                    Cmb_Fte_Financiamiento.Enabled = false;
                    Cmb_Programa.Enabled = false;
                    Cmb_Partida.Enabled = false;
                    //Cmb_Tipo.Enabled = false;
                    Cmb_Estatus.Enabled = false;
                    //Cmb_Producto_Servicio.Enabled = false;
                    //Ibtn_Buscar_Producto.Enabled = false;
                    Ibtn_Agregar_Producto.Enabled = false;
                    Grid_Productos_Servicios.Enabled = false;
                    //Cmb_Producto_Servicio.Enabled = false;
                    Txt_Comentario.Enabled = false;
                    Txt_Cantidad.Enabled = false;
                    Txt_Justificacion.Enabled = false;
                    //Txt_Especificaciones.Enabled = false;
                    //Chk_Verificar.Enabled = false;
                    break;
                //Estado de Nuevo
                case "Nuevo":
                    Btn_Listar_Requisiciones.Visible = false;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Salir.Visible = true;
                    Cmb_Dependencia.Enabled = false;
                    Cmb_Fte_Financiamiento.Enabled = true;
                    Cmb_Programa.Enabled = true;
                    Cmb_Partida.Enabled = true;
                    //Cmb_Tipo.Enabled = true;
                    Cmb_Estatus.Enabled = true;
                    //Ibtn_Buscar_Producto.Enabled = true;
                    Ibtn_Agregar_Producto.Enabled = true;
                    Grid_Productos_Servicios.Enabled = true;
                    //Cmb_Producto_Servicio.Enabled = true;
                    Txt_Comentario.Enabled = true;
                    Txt_Comentario.Text = "";
                    Txt_Cantidad.Enabled = true;
                    Txt_Justificacion.Enabled = true;
                    //Txt_Especificaciones.Enabled = true;
                    //Chk_Verificar.Enabled = true;
                    //Poner la fecha
                    Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
                    //Verificar tipo empleado logueado
                    Cmb_Estatus.SelectedValue = "EN CONSTRUCCION";
                    Txt_Folio.Text = SubFijo_Requisicion +
                            Presidencia.Registro_Gastos.Datos.Cls_Ope_Com_Registro_Gastos_Datos.Obtener_Consecutivo(
                            Ope_Com_Gastos.Campo_Gasto_ID, Ope_Com_Gastos.Tabla_Ope_Com_Gastos);
                    break;
                //Estado de Modificar
                case "Modificar":
                    Btn_Listar_Requisiciones.Visible = false;
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Cmb_Dependencia.Enabled = false;
                    Cmb_Fte_Financiamiento.Enabled = false;
                    Cmb_Programa.Enabled = false;
                    Cmb_Partida.Enabled = false;
                    //Cmb_Tipo.Enabled = false;
                    Cmb_Estatus.Enabled = true;
                    //Cmb_Producto_Servicio.Enabled = false;
                    //Ibtn_Buscar_Producto.Enabled = true;
                    Ibtn_Agregar_Producto.Enabled = true;
                    Grid_Productos_Servicios.Enabled = true;
                    Txt_Comentario.Enabled = true;
                    Txt_Comentario.Text = "";
                    Txt_Cantidad.Enabled = true;
                    Txt_Justificacion.Enabled = true;
                    //Txt_Especificaciones.Enabled = true;
                    //Chk_Verificar.Enabled = true;
                    //if (Cmb_Tipo.SelectedValue == "STOCK")
                    //    Cmb_Producto_Servicio.Enabled = false;
                    //else
                    //    Cmb_Producto_Servicio.Enabled = true;

                    //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
                    //DataTable Table = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
                    //if (Table != null)
                    //{
                    //    String Grupo_Rol = Table.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                    //    if (Grupo_Rol == "00001" || Grupo_Rol == "00002" || Grupo_Rol == "00003")
                    //    {
                    //        //Cmb_Area.Enabled = true;
                    //    }
                    //}
                    break;
                default: break;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Informacion(ex.ToString(), true);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Calcular_Impuestos
    ///DESCRIPCIÓN: Calcula IVA, IEPS, SUBTOTAL y TOTAL y lo spone en sus cajas de texto
    ///PARAMETROS: 
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 22/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Calcular_Impuestos()
    {
        double Subtotal = 0;
        double Total = 0;
        double IVA = 0;
        double IEPS = 0;
        if (Session[P_Dt_Productos_Servicios] != null && ((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Count > 0)
        {
            foreach (DataRow Renglon in ((DataTable)Session[P_Dt_Productos_Servicios]).Rows)
            {
                Subtotal = Subtotal + double.Parse(Renglon["Monto_Total"].ToString());
                IVA = IVA + double.Parse(Renglon["IVA"].ToString());
                IEPS = IEPS + double.Parse(Renglon["IEPS"].ToString());
            }
        }
        Total = Subtotal + IVA + IEPS;
        Txt_Subtotal.Text = Subtotal.ToString();
        Txt_IVA.Text = IVA.ToString();
        Txt_Total.Text = Total.ToString();
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
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion.Text = txt;
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    // DESCRIPCIÓN: Limpia las areas de texto y deja los combos en su valor inical
    // RETORNA: 
    // CREO: 
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Limpiar_Formulario()
    {
        //Cmb_Tipo.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Programa.Items.Clear();
        Cmb_Partida.Items.Clear();
       // Cmb_Producto_Servicio.SelectedIndex = 0;
        //Chk_Verificar.Checked = false;
        Limpiar_Cajas_Texto();
        Limpiar_Grids_Y_DataTables();
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Limpiar_Cajas_Texto
    // DESCRIPCIÓN: Limpia las areas de texto y deja los combos en su valor inical
    // RETORNA: 
    // CREO: 
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Limpiar_Cajas_Texto()
    {
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Txt_Producto_Servicio.Text = "";
        Txt_Cantidad.Text = "";
        Txt_Comentario.Text = "";
        Txt_Busqueda.Text = "";
        Txt_Subtotal.Text = "0.0";
        Txt_IEPS.Text = "0.0";
        Txt_IVA.Text = "0.0";
        Txt_Total.Text = "0.0";
        Txt_Justificacion.Text = "";
        Lbl_Disponible_Partida.Text = "$ 0.00";
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Limpiar_Grids_Y_DataTables
    // DESCRIPCIÓN: Limpia las grids y datatable de la página
    // CREO: 
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Limpiar_Grids_Y_DataTables()
    {
        Grid_Productos_Servicios.DataSource = null;
        Grid_Productos_Servicios.DataBind();
        Grid_Comentarios.DataSource = null;
        Grid_Comentarios.DataBind();
        //P_Dt_Productos_Servicios.Rows.Clear();
    }


    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Grid_Gastos
    // DESCRIPCIÓN: Llena el grid principal de requisiciones
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: Diciembre/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Grid_Gastos()
    {
        Div_Contenido.Visible = false;
        Div_Listado_Requisiciones.Visible = true;
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
        Gastos_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        //Gastos_Negocio.P_Fecha_Inicial = Txt_Fecha_Inicial.Text;
        Gastos_Negocio.P_Fecha_Inicial = String.Format("{0:dd/MM/yy}", Txt_Fecha_Inicial.Text);
        //Gastos_Negocio.P_Fecha_Final = Txt_Fecha_Final.Text;
        Gastos_Negocio.P_Fecha_Final = String.Format("{0:dd/MM/yy}", Txt_Fecha_Final.Text);
        Gastos_Negocio.P_Folio = Txt_Busqueda.Text;
        Gastos_Negocio.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue.Trim();
        Session[P_Dt_Requisiciones] = null;
        Session[P_Dt_Requisiciones] = Gastos_Negocio.Consultar_Gastos();
        if (Session[P_Dt_Requisiciones] != null && ((DataTable)Session[P_Dt_Requisiciones]).Rows.Count > 0)
        {
            Div_Contenido.Visible = false;
            Grid_Requisiciones.DataSource = (DataTable)Session[P_Dt_Requisiciones];
            Grid_Requisiciones.DataBind();
        }
        else
        {
            Mostrar_Informacion("No se encontraron datos con los criterios de búsqueda", true);
            Grid_Requisiciones.DataSource = (DataTable)Session[P_Dt_Requisiciones];
            Grid_Requisiciones.DataBind();
        }
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Combos_Generales()
    // DESCRIPCIÓN: Llena los combos principales de la interfaz de usuario
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: Diciembre/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combos_Generales()
    {
        //Limpiar_Formulario();
        //Coombo estatus
        Txt_Subtotal.Text = "0.0";
        Txt_Total.Text = "0.0";
        Txt_IEPS.Text = "0.0";
        Txt_IVA.Text = "0.0";
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("EN CONSTRUCCION");
        Cmb_Estatus.Items.Add("GENERADA");
        Cmb_Estatus.Items.Add("CANCELADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
        Cmb_IVA.Items.Clear();
        //Cmb_IVA.Items.Add("?");
        Cmb_IVA.Items.Add("SI");
        Cmb_IVA.Items.Add("NO");
        //Cmb_IVA.Items[0].Value = "0";
        Cmb_IVA.Items[0].Selected = true;
        Cmb_IEPS.Items.Clear();
        //Cmb_IEPS.Items.Add("?");
        Cmb_IEPS.Items.Add("SI");
        Cmb_IEPS.Items.Add("NO");
        //Cmb_IEPS.Items[0].Value = "0";
        Cmb_IEPS.Items[1].Selected = true;
        //Seleccionar combo dependencias
        Cmb_Dependencia.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        Cmb_Programa.Items.Clear();
        Cmb_Fte_Financiamiento.Items.Clear();
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
        Gastos_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        DataTable Dt_Fte_Financiamiento = Gastos_Negocio.Consultar_Fuentes_Financiamiento();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Fte_Financiamiento, Dt_Fte_Financiamiento, 1, 0);

        DataTable Data_Table_Proyectos = Gastos_Negocio.Consultar_Proyectos_Programas();
        Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Programa, Data_Table_Proyectos, 1, 0);

        ////Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
        //DataTable Tabla = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
        //if (Tabla != null)
        //{
        //  String Grupo_Rol = Tabla.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
        //  if (Grupo_Rol == "00001" || Grupo_Rol == "00002" || Grupo_Rol == "00003")
        //  {
        //      Cmb_Dependencia.Enabled = true;
        //      Cmb_Area.Enabled = true;
        //  }
        //}


    }
    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Combos_Busqueda()
    // DESCRIPCIÓN: Llena los combos principales de la interfaz de usuario
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: Diciembre/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combos_Busqueda()
    {
        Cmb_Estatus_Busqueda.Items.Clear();
        Cmb_Estatus_Busqueda.Items.Add("EN CONSTRUCCION,GENERADA");
        Cmb_Estatus_Busqueda.Items.Add("EN CONSTRUCCION");
        Cmb_Estatus_Busqueda.Items.Add("GENERADA");
        Cmb_Estatus_Busqueda.Items.Add("CANCELADA");
        Cmb_Estatus_Busqueda.Items.Add("REVISAR");
        Cmb_Estatus_Busqueda.Items[0].Selected = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Refrescar_Grid
    ///DESCRIPCIÓN: refresca el gris con los registros de asuntos mas actuales 
    ///que existen en la base de datos
    ///PARAMETROS: 
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 02/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Refrescar_Grid()
    {
        Grid_Productos_Servicios.DataSource = (DataTable)Session[P_Dt_Productos_Servicios];
        Grid_Productos_Servicios.DataBind();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Longitud
    ///DESCRIPCIÓN: Valida la longitud del texto que se recibe en un TextBox
    ///PARAMETROS:             
    ///CREO: Gustavo Angeles Cruz
    ///FECHA_CREO: 20/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Boolean Validar_Longitud(TextBox Txt_Control, int Int_Tamaño)
    {
        Boolean Bln_Bandera;
        Bln_Bandera = false;
        //Verifica el tamaño de el control
        if (Txt_Control.Text.Length >= Int_Tamaño)
            Bln_Bandera = true;
        return Bln_Bandera;
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Generar_Tabla_Informacion
    // DESCRIPCIÓN: Crea una tabla con la informacion que se requiere ingresar al formulario
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Generar_Tabla_Informacion()
    {
        Contador_Columna = Contador_Columna + 1;
        if (Contador_Columna > 2)
        {
            Contador_Columna = 0;
            Informacion += "</tr><tr>";
        }
    }

    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Validaciones
    // DESCRIPCIÓN: Genera el String con la informacion que falta y ejecuta la 
    // operacion solicitada si las validaciones son positivas
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: 24/Agosto/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private Boolean Validaciones(bool Validar_Completo)
    {
        Boolean Bln_Bandera;
        Contador_Columna = 0;
        Bln_Bandera = true;
        Informacion += "<table style='width: 100%;font-size:9px;' >" +
            "<tr colspan='3'>Es necesario:</tr>" +
            "<tr>";
        //Verifica que campos esten seleccionados o tengan valor valor
        //if (Cmb_Tipo.SelectedIndex == 0)
        //{
        //    Informacion += "<td>+ Seleccionar Tipo. </td>";
        //    Bln_Bandera = false;
        //    Generar_Tabla_Informacion();
        //}
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Informacion += "<td>+ Seleccionar Estatus.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Dependencia.SelectedIndex == 0)
        {
            Informacion += "<td>+ Seleccionar Dependencia.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Programa.SelectedIndex == 0)
        {
            Informacion += "<td>+ Seleccionar Programa.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Validar_Completo)
        {
            if (Session[P_Dt_Productos_Servicios] != null)
            {
                if (((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Count == 0)
                {
                    Informacion += "<td>+ Agregar Producto/Servicio.</td>";
                    Bln_Bandera = false;
                    Generar_Tabla_Informacion();
                }
            }
            if (Txt_Justificacion.Text.Trim().Length == 0)
            {
                Informacion += "<td>+ Campo Justificación.</td>";
                Bln_Bandera = false;
                Generar_Tabla_Informacion();
            }

            //if (Txt_Comentario.Text.Trim().Length == 0)
            //{
            //    Informacion += "<td>+ Campo Comentarios.</td>";
            //    Bln_Bandera = false;
            //    Generar_Tabla_Informacion();
            //}

            else
            {
                if (Validar_Longitud(Txt_Comentario, 250))
                {
                    Informacion += "<td>+ Campo comentario excede la longitud permitida.</td>";
                    Bln_Bandera = false;
                    Generar_Tabla_Informacion();
                }
            }
        }
        Informacion += "</tr></table>";
        return Bln_Bandera;
    }

    //private void Limpiar_Modal_Produtos_Servicios()
    //{
    //    Txt_Nombre.Text = "";
    //    Grid_Productos_Servicios_Modal.DataSource = null;
    //    Grid_Productos_Servicios_Modal.DataBind();
    //}

    private DataTable Construir_Tabla_Detalles_Requisicion()
    {
        DataTable Tabla = new DataTable();
        DataColumn Columna = null;
        DataTable Tabla_Base_Datos =
            Presidencia.Generar_Requisicion.
            Datos.Cls_Ope_Com_Requisiciones_Datos.
            //Consultar_Columnas_De_Tabla_BD(Ope_Com_Req_Producto.Tabla_Ope_Com_Req_Producto);
            Consultar_Columnas_De_Tabla_BD(Ope_Com_Gastos_Detalles.Tabla_Ope_Com_Gastos_Detalles);
        foreach (DataRow Renglon in Tabla_Base_Datos.Rows)
        {
            Columna = new DataColumn(Renglon["COLUMNA"].ToString(), System.Type.GetType("System.String"));
            Tabla.Columns.Add(Columna);
        }
        return Tabla;
    }

    private DataTable Construir_Tabla_Presupuestos()
    {
        DataTable Tabla = new DataTable();
        DataColumn Columna = null;
        DataTable Tabla_Base_Datos =
            Presidencia.Generar_Requisicion.
            Datos.Cls_Ope_Com_Requisiciones_Datos.
            Consultar_Columnas_De_Tabla_BD(Cat_Com_Dep_Presupuesto.Tabla_Cat_Com_Dep_Presupuesto);
        foreach (DataRow Renglon in Tabla_Base_Datos.Rows)
        {
            Columna = new DataColumn(Renglon["COLUMNA"].ToString(), System.Type.GetType("System.String"));
            Tabla.Columns.Add(Columna);
        }
        Columna = new DataColumn("CLAVE", System.Type.GetType("System.String"));
        Tabla.Columns.Add(Columna);
        return Tabla;
    }

    private DataTable Construir_Tabla_Productos()
    {
        DataTable Tabla = new DataTable();
        DataColumn Columna = null;
        DataTable Tabla_Base_Datos =
            Presidencia.Generar_Requisicion.
            Datos.Cls_Ope_Com_Requisiciones_Datos.
            Consultar_Columnas_De_Tabla_BD(Cat_Com_Productos.Tabla_Cat_Com_Productos);
        foreach (DataRow Renglon in Tabla_Base_Datos.Rows)
        {
            Columna = new DataColumn(Renglon["COLUMNA"].ToString(), System.Type.GetType("System.String"));
            Tabla.Columns.Add(Columna);
        }
        return Tabla;
    }

    #endregion

    protected void Cmb_Partida_SelectedIndexChanged1(object sender, EventArgs e)
    {
        Lbl_Partida.Text = "";
        Lbl_Clave.Text = "";
        Lbl_Disponible.Text = "";
        Lbl_Fecha_Asignacion.Text = "";
        //Div_Presupuesto.Visible = true;
        Gastos_Negocio = new Cls_Ope_Com_Registro_Gastos_Negocio();
        Gastos_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue.Trim();
        Gastos_Negocio.P_Proyecto_Programa_ID = Cmb_Programa.SelectedValue.Trim();
        Gastos_Negocio.P_Fuente_Financiamiento = Cmb_Fte_Financiamiento.SelectedValue.Trim();
        Gastos_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue.Trim();
        Gastos_Negocio.P_Anio_Presupuesto = DateTime.Now.Year;
        DataTable Dt_Presupuesto = Gastos_Negocio.Consultar_Presupuesto_Partidas();
        if (Dt_Presupuesto != null && Dt_Presupuesto.Rows.Count > 0)
        {
            DataRow Renglon = Dt_Presupuesto.Rows[0];
            Lbl_Partida.Text = Renglon["NOMBRE"].ToString();
            Lbl_Clave.Text = Renglon["CLAVE"].ToString();
            Lbl_Disponible.Text = Renglon["MONTO_DISPONIBLE"].ToString();

            Lbl_Disponible_Partida.Text = " $ " + Renglon["MONTO_DISPONIBLE"].ToString();
            String fecha = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Renglon["FECHA_CREO"].ToString()));
            Lbl_Fecha_Asignacion.Text = fecha;
        }
        else
        {
            Lbl_Partida.Text = "Sin presupuesto asignado";
            Lbl_Clave.Text = "---";
            Lbl_Disponible.Text = "---";
            Lbl_Fecha_Asignacion.Text = "---";
            Lbl_Disponible_Partida.Text = "Sin presupuesto asignado";
        }
    }

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
            ((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Remove(_DataRow);
        }
        return _DataTable;
    }

    private void Actualizar_Grid_Partidas_Productos()
    {
        DataRow[] Partida = ((DataTable)Session[P_Dt_Partidas]).Select("Partida_ID ='" + Cmb_Partida.SelectedValue.Trim() + "'");
        if (Partida != null && Partida.Length > 0)
        {
            Lbl_Disponible_Partida.Text = " $ " + Partida[0]["Monto_Disponible"].ToString();
        }
        //Grid_Partidas_Tmp.DataSource = P_Dt_Partidas;
        //Grid_Partidas_Tmp.DataBind();
        //Grid_Productos_Tmp.DataSource = P_Dt_Productos;
        //Grid_Productos_Tmp.DataBind();
    }
    private void Agregar_Tooltip_Combo(DropDownList Combo)
    {
        foreach (ListItem Item in Combo.Items)
        {
            Item.Attributes.Add("Title", Item.Text);
        }
    }
    protected void Grid_Productos_Servicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos_Servicios.DataSource = (DataTable)Session[P_Dt_Productos_Servicios];
        Grid_Productos_Servicios.PageIndex = e.NewPageIndex;
        Grid_Productos_Servicios.DataBind();
    }
    protected void Cmb_Fte_Financiamiento_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Cmb_Partida.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            String Str = Ex.ToString();
        }
        Lbl_Disponible_Partida.Text = "$ 0.00";
    }
    private int Numero_Aleatorio()     
    {
        int Numero_Aleatorio = 0;        
        while(true)
        {
            bool Repetido = false;
            Numero_Aleatorio = DateTime.Now.Millisecond;
            foreach(DataRow Renglon in ((DataTable)Session[P_Dt_Productos_Servicios]).Rows)
            {
                if (Renglon["IDENTIFICADOR"].ToString().Trim() == (Numero_Aleatorio + "")) 
                {
                    Repetido = true;
                }
            }
            if (!Repetido)
                break;
        }
        return Numero_Aleatorio;
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
            Botones.Add(Btn_Nuevo);
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Buscar);

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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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
    #endregion
    #region ORDENAR GRIDS
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
        Grid_Sorting(Grid_Requisiciones, (DataTable)Session[P_Dt_Requisiciones], e);
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