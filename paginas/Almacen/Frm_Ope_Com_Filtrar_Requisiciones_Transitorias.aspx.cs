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
using System.Collections.Generic;

public partial class paginas_Almacen_Frm_Ope_Com_Filtrar_Requisiciones_Transitorias : System.Web.UI.Page
{

    #region VARIABLES / CONSTANTES
    //objeto de la clase de negocio de dependencias para acceder a la clase de datos y realizar copnexion
    private Cls_Cat_Dependencias_Negocio Dependencia_Negocio;
    //objeto de la clase de negocio de Requisicion para acceder a la clase de datos y realizar copnexion
    private Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio;
    //objeto en donde se guarda un id de producto o servicio para siempre tener referencia
    //private static String PS_ID;
    // private static String PS_ID = "PS_ID";
    private Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar_Requisicion;
    private int Contador_Columna;
    private String Informacion;

    private static String P_Dt_Productos_Servicios = "P_Dt_Productos_Servicios_Filtrar";
    //private static String P_Dt_Partidas = "P_Dt_Partidas";
    private static String P_Dt_Productos = "P_Dt_Productos_Filtrar";
    private static String P_Dt_Requisiciones = "P_Dt_Requisiciones_Transitorias_Filtrar";
    //private static String P_Dt_Productos_Servicios_Modal = "P_Dt_Productos_Servicios_Modal";

    private const String Operacion_Comprometer = "COMPROMETER";
    private const String Operacion_Descomprometer = "DESCOMPROMETER";
    private const String Operacion_Quitar_Renglon = "QUITAR";
    private const String Operacion_Agregar_Renglon_Nuevo = "AGREGAR_NUEVO";
    private const String Operacion_Agregar_Renglon_Copia = "AGREGAR_COPIA";

    private const String SubFijo_Requisicion = "RQ-";
    private const String EST_EN_CONSTRUCCION = "EN CONSTRUCCION";
    private const String EST_GENERADA = "GENERADA";
    private const String EST_CANCELADA = "CANCELADA";
    private const String EST_REVISAR = "REVISAR";
    private const String EST_RECHAZADA = "RECHAZADA";
    private const String EST_AUTORIZADA = "AUTORIZADA";
    private const String EST_PROCESAR = "PROCESAR";
    private const String EST_FILTRADA = "FILTRADA";

    private const String TIPO_STOCK = "STOCK";
    private const String TIPO_TRANSITORIA = "TRANSITORIA";

    private const String MODO_LISTADO = "LISTADO";
    private const String MODO_INICIAL = "INICIAL";
    private const String MODO_MODIFICAR = "MODIFICAR";
    private const String MODO_NUEVO = "NUEVO";

    #endregion

    #region PAGE LOAD / INIT
    protected void Page_Load(object sender, EventArgs e)
    {
        //Valores de primera vez
        if (!IsPostBack)
        {
            Session["Activa"] = true;
            ViewState["SortDirection"] = "DESC";
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            _DateTime = _DateTime.AddMonths(-1);
            Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Inicial.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            //llenar combo dependencias
            Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia, Dt_Dependencias, 1, 0);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Panel, Dt_Dependencias, 1, 0);
            Cmb_Dependencia.SelectedIndex = 0;
            //Cmb_Dependencia_Panel.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            Cmb_Dependencia_Panel.SelectedIndex = 0;
            Cmb_Dependencia_Panel.Enabled = true;
            Llenar_Combos_Busqueda();
            String[] Datos_Combo = 
                { EST_EN_CONSTRUCCION, EST_GENERADA, EST_CANCELADA, EST_AUTORIZADA, 
                  EST_FILTRADA,EST_RECHAZADA,EST_REVISAR};
            Llenar_Combo(Cmb_Estatus, Datos_Combo);
            Llenar_Grid_Requisiciones();
            Habilitar_Controles(MODO_LISTADO);
        }
        //Tooltips
        //Agregar_Tooltip_Combo(Cmb_Fte_Financiamiento);
        //Agregar_Tooltip_Combo(Cmb_Programa);
        //Agregar_Tooltip_Combo(Cmb_Partida);
        Mostrar_Informacion("", false);
        //Modal_Busqueda_Prod_Serv.Hide();
    }
    private void Formato_Importes()
    {
        Txt_Subtotal.Text = String.Format("{0:C}", decimal.Parse(Txt_Subtotal.Text));
        Txt_IEPS.Text = String.Format("{0:C}", decimal.Parse(Txt_IEPS.Text));
        Txt_IVA.Text = String.Format("{0:C}", decimal.Parse(Txt_IVA.Text));
        Txt_Total.Text = String.Format("{0:C}", decimal.Parse(Txt_Total.Text)); 
    }
    #endregion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Construir_DataTables()
    {
        //Session[P_Dt_Productos_Servicios_Modal] = null;
        //Session[PS_ID] = null;
        //Session[P_Dt_Productos_Servicios] = Construir_Tabla_Detalles_Requisicion();
        //Session[P_Dt_Partidas] = Construir_Tabla_Presupuestos();
        //Session[P_Dt_Productos] = Construir_Tabla_Productos();
    }
    #region EVENTOS

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo(DropDownList Combo, String[] Items)
    {
        Combo.Items.Clear();
        Combo.Items.Add("<<SELECCIONAR>>");
        foreach (String _Item in Items)
        {
            Combo.Items.Add(_Item);
        }
        Combo.Items[0].Value = "0";
        Combo.Items[0].Selected = true;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        //Limpiar_Modal_Produtos_Servicios();
        if (Btn_Modificar.ToolTip == "Modificar")
        {
            if (Txt_Folio.Text.Trim() != "")
            {
                String Estatus_Tmp = Cmb_Estatus.SelectedValue;
                String[] Datos_Combo = { EST_PROCESAR, EST_REVISAR };
                Cmb_Estatus.SelectedIndex = 0;                
                Llenar_Combo(Cmb_Estatus, Datos_Combo);
                Habilitar_Controles(MODO_MODIFICAR);
            }
            else
            {
                Mostrar_Informacion("Debe seleccionar una requisición", true);
            }
        }
        else if (Btn_Modificar.ToolTip == "Actualizar")
        {
            String Mensaje = "";
            //Proceso para modificar
            if (Cmb_Estatus.SelectedIndex != 0)
            {
                if (Cmb_Estatus.SelectedValue.Trim() == EST_PROCESAR ||
                    (Cmb_Estatus.SelectedValue.Trim() == EST_REVISAR && Txt_Comentario.Text.Trim().Length > 0))
                {
                    //Cargar datos Negocio
                    Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
                    Requisicion_Negocio.P_Folio = Txt_Folio.Text;
                    Requisicion_Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
                    bool Actualizado = Requisicion_Negocio.Proceso_Filtrar();
                    if (Actualizado)
                    {
                        //Registrar Historial
                        Requisicion_Negocio.Registrar_Historial(Requisicion_Negocio.P_Estatus, Requisicion_Negocio.P_Folio);
                    }

                    if (Actualizado && Txt_Comentario.Text.Trim().Length > 0)
                    {
                        Administrar_Requisicion = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                        Administrar_Requisicion.P_Requisicion_ID = Txt_Folio.Text.Replace(SubFijo_Requisicion, "");
                        Administrar_Requisicion.P_Comentario = Txt_Comentario.Text;
                        Administrar_Requisicion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Administrar_Requisicion.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
                        Administrar_Requisicion.Alta_Observaciones();
                        DataSet Data_Set = Administrar_Requisicion.Consulta_Observaciones();
                        Grid_Comentarios.DataSource = Data_Set;
                        Grid_Comentarios.DataBind();
                    }

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisiciones", "alert('Requisición modificada');", true);
                    Llenar_Grid_Requisiciones();
                    Habilitar_Controles(MODO_LISTADO);


                    String[] Datos_Combo = 
                        { EST_EN_CONSTRUCCION, EST_GENERADA, EST_CANCELADA, EST_AUTORIZADA, 
                            EST_FILTRADA,EST_RECHAZADA,EST_REVISAR};
                    Llenar_Combo(Cmb_Estatus, Datos_Combo);

                }
                else
                {
                    Mostrar_Informacion("Debe ingresar un comentario", true);
                }
            }
            else
            {
                Mostrar_Informacion("Debe seleccionar un estatus", true);
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Cancelar")
        {
            if (Btn_Nuevo.ToolTip == "Dar de Alta")
            {
                Limpiar_Formulario();
            }
            Habilitar_Controles(MODO_INICIAL);
            String[] Datos_Combo = { EST_AUTORIZADA };
            Llenar_Combo(Cmb_Estatus, Datos_Combo);
            Cmb_Estatus.SelectedValue = EST_AUTORIZADA;
        }
        else
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Evento_Combo_Dependencia()
    {
        //Cargar los programas de la dependencia seleccionada
        // Cmb_Programa.Items.Clear();
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        DataTable Data_Table_Proyectos = Requisicion_Negocio.Consultar_Proyectos_Programas();
        //Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Programa, Data_Table_Proyectos);
        // Cmb_Programa.SelectedIndex = 0;
        //Limpiar las partidas
        // Cmb_Partida.Items.Clear();
        //Cmb_Partida.Items.Add("<<SELECCIONAR>>");
        //Cmb_Partida.SelectedIndex = 0;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        Evento_Combo_Dependencia();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        //Habilitar_Controles(MODO_INICIAL);
        Llenar_Grid_Requisiciones();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private double Formato_Double(double numero)
    {
        try
        {
            String Str_Numero = numero.ToString("#.##");
            numero = Convert.ToDouble(Str_Numero);
        }
        catch (Exception Ex)
        {
            String Str = Ex.ToString();
            numero = 0;
        }
        return numero;
    }



    //*********************************************************************************************
    //*********************************************************************************************
    //*********************************************************************************************
    #region PRESUPUESTOS



    #endregion

    //*********************************************************************************************
    //*********************************************************************************************
    //*********************************************************************************************
    #region PRODUCTOS


    #endregion




    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
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




    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Click(object sender, EventArgs e)
    {
        // Limpiar_Modal_Produtos_Servicios();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Listar_Requisiciones_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Requisiciones();
        Habilitar_Controles(MODO_LISTADO);
    }

    #endregion

    #region EVENTOS GRID
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Evento_Grid_Requisiciones_Seleccionar(String Dato)
    {
        //Actualizar_Grid_Partidas_Productos();
        DataTable Dt_Comentarios = (DataTable)Grid_Comentarios.DataSource;
        Habilitar_Controles(MODO_INICIAL);
        Txt_Justificacion.Visible = true;
        Construir_DataTables();
        Llenar_Combos_Generales();
        Div_Listado_Requisiciones.Visible = false;
        Div_Contenido.Visible = true;
        //GridViewRow Row = Grid_Requisiciones.SelectedRow;
        //String ID = Row.Cells[1].Text;
        Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        String No_Requisicion = Dato;//Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        DataRow[] Requisicion = ((DataTable)Session[P_Dt_Requisiciones]).Select("No_Requisicion = '" + No_Requisicion + "'");

        Txt_Folio.Text = Requisicion[0][Ope_Com_Requisiciones.Campo_Folio].ToString();
        String Fecha = Requisicion[0][Ope_Com_Requisiciones.Campo_Fecha_Creo].ToString();
        Fecha = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Fecha));
        Txt_Fecha.Text = Fecha.ToUpper();
        //Seleccionar combo dependencia
        Cmb_Dependencia.SelectedValue =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Dependencia_ID].ToString().Trim();
        //Seleccionar Tipo
        Cmb_Tipo.SelectedValue =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Tipo].ToString().Trim();
        //Seleccionar estatus
        Cmb_Estatus.SelectedValue =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Estatus].ToString().Trim();
        //Seleccionar Categoria
        Cmb_Producto_Servicio.SelectedValue =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Tipo_Articulo].ToString().Trim();
        //Poner Justificación
        Txt_Justificacion.Text =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Justificacion_Compra].ToString();
        //Poner especificaciones
        Txt_Especificaciones.Text =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv].ToString();
        Txt_Subtotal.Text =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Subtotal].ToString();
        Txt_IEPS.Text =
            Requisicion[0][Ope_Com_Requisiciones.Campo_IEPS].ToString();
        Txt_IVA.Text =
            Requisicion[0][Ope_Com_Requisiciones.Campo_IVA].ToString();
        //Total de la requisición
        Txt_Total.Text =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Total].ToString();
        //LLenar DataTable P_Dt_Productos_Servicios
        Requisicion_Negocio.P_Requisicion_ID = No_Requisicion;
        Requisicion_Negocio.P_Tipo_Articulo = Cmb_Producto_Servicio.SelectedValue.Trim();
        Session[P_Dt_Productos_Servicios] = Requisicion_Negocio.Consultar_Productos_Servicios_Requisiciones();
        String Str_Partidas_IDs = "";
        String Str_Productos_IDs = "";
        if (Session[P_Dt_Productos_Servicios] != null && ((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Count > 0)
        {
            Grid_Productos_Servicios.DataSource = Session[P_Dt_Productos_Servicios] as DataTable;
            Grid_Productos_Servicios.DataBind();
            //Recorrer P_Dt_Productos_Servicios para obtener las partidas y los producos
            foreach (DataRow Row in ((DataTable)Session[P_Dt_Productos_Servicios]).Rows)
            {
                Str_Partidas_IDs = Str_Partidas_IDs + Row["PARTIDA_ID"].ToString() + ",";
                Str_Productos_IDs = Str_Productos_IDs + Row["PROD_SERV_ID"].ToString() + ",";
            }
            if (Str_Partidas_IDs.Length > 0)
            {
                Str_Partidas_IDs = Str_Partidas_IDs.Substring(0, Str_Partidas_IDs.Length - 1);
            }
            if (Str_Productos_IDs.Length > 0)
            {
                Str_Productos_IDs = Str_Productos_IDs.Substring(0, Str_Productos_IDs.Length - 1);
            }
        }
        //Llenar DataTable P_Dt_Productos
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Requisicion_Negocio.P_Producto_ID = Str_Productos_IDs;
        Session[P_Dt_Productos] = Requisicion_Negocio.Consultar_Poducto_Por_ID();
        //Actualizar 
        Cls_Ope_Com_Administrar_Requisiciones_Negocio Administrar = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        Administrar.P_Requisicion_ID = Txt_Folio.Text.Replace(SubFijo_Requisicion, "");
        DataSet Data_Set = Administrar.Consulta_Observaciones();
        if (Data_Set != null && Data_Set.Tables[0].Rows.Count != 0)
        {
            Div_Comentarios.Visible = true;
            Grid_Comentarios.DataSource = Data_Set;
            Grid_Comentarios.DataBind();
        }
        else
        {
            Div_Comentarios.Visible = false;
            Grid_Comentarios.DataSource = null; ;
            Grid_Comentarios.DataBind();
        }

        Formato_Importes();

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Requisicion_Click(object sender, ImageClickEventArgs e)
    {
        String No_Requisicion = ((ImageButton)sender).CommandArgument;
        Evento_Grid_Requisiciones_Seleccionar(No_Requisicion);
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.DataSource = ((DataTable)Session[P_Dt_Requisiciones]);
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataBind();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Comentarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow Renglon = Grid_Comentarios.SelectedRow;
        Txt_Comentario.Text = Renglon.Cells[1].Text;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Comentarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Administrar_Requisicion = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
        Administrar_Requisicion.P_Requisicion_ID = Txt_Folio.Text.Replace(SubFijo_Requisicion, "");
        DataSet Data_Set = Administrar_Requisicion.Consulta_Observaciones();
        Grid_Comentarios.DataSource = Data_Set;
        Grid_Comentarios.PageIndex = e.NewPageIndex;
        Grid_Comentarios.DataBind();
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_Servicios_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        //String Comando = e.CommandName.ToString();
        //if (Comando != "Page")
        //{
        //    int indice = Convert.ToInt32(e.CommandArgument);
        //    //GridViewRow Renglon = Grid_Productos_Servicios.Rows[indice];
        //    Grid_Productos_Servicios.SelectedIndex = indice;
        //    String Tipo = Grid_Productos_Servicios.SelectedDataKey["Tipo"].ToString().Trim();
        //    String Producto_Servicio_ID =
        //        Grid_Productos_Servicios.SelectedDataKey["Prod_Serv_ID"].ToString().Trim();
        //    DataRow[] _DataRow = ((DataTable)Session[P_Dt_Productos_Servicios]).Select("Prod_Serv_ID = '" + Producto_Servicio_ID + "'");


        //    if (_DataRow != null && _DataRow.Length > 0)
        //    {
        //        int Cantidad_Productos = int.Parse(_DataRow[0]["CANTIDAD"].ToString().Trim());
        //        double Monto = double.Parse(_DataRow[0]["MONTO"].ToString().Trim());
        //        double Monto_Total = double.Parse(_DataRow[0]["MONTO_TOTAL"].ToString().Trim());

        //        double Monto_IEPS = double.Parse(_DataRow[0]["MONTO_IEPS"].ToString().Trim());
        //        double Monto_IVA = double.Parse(_DataRow[0]["MONTO_IVA"].ToString().Trim());

        //        String Partida_ID = _DataRow[0]["Partida_ID"].ToString().Trim();
        //        //seleccionar el combo de partidas
        //        try {
        //            Cmb_Partida.SelectedValue = Partida_ID;
        //        } catch(Exception Ex){
        //            String Str = Ex.ToString();
        //            Cmb_Partida.SelectedIndex = 0;
        //            Lbl_Disponible_Partida.Text = "La partida fue removida";
        //        }
        //        switch (Comando)
        //        {
        //            case "Eliminar":
        //                //Se descompromete los presupuestos en P_Dt_Partidas                                     
        //                Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
        //                    Partida_ID, ((DataTable)Session[P_Dt_Partidas]), Operacion_Descomprometer, Monto_Total);
        //                //Se descompromete los Productos en P_Dt_Productos
        //                if (Cmb_Tipo.SelectedValue.Trim() == "STOCK")
        //                {
        //                    Comprometer_O_Descomprometer_Disponible_A_Productos_En_Dt_Productos(
        //                        Producto_Servicio_ID, ((DataTable)Session[P_Dt_Productos]), Operacion_Descomprometer, Cantidad_Productos);
        //                }
        //                ((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Remove(_DataRow[0]);
        //                ((DataTable)Session[P_Dt_Productos_Servicios]).AcceptChanges();
        //                Refrescar_Grid();
        //                Calcular_Impuestos();
        //                //Se actualizan los grids de partidas y productos disponibles
        //                Actualizar_Grid_Partidas_Productos();
        //                break;
        //            case "Mas":
        //                if (Tipo == "PRODUCTO")
        //                {
        //                    double Monto_IEPS_A_Comprometer = Monto_IEPS / Cantidad_Productos;
        //                    double Monto_IVA_A_Comprometer = Monto_IVA / Cantidad_Productos;
        //                    double Monto_A_Comprometer = Monto / Cantidad_Productos;
        //                    double Monto_Total_A_Comprometer = Monto_Total / Cantidad_Productos;

        //                    bool Agregar_Producto = true;
        //                    double Disponible_Partida = Verifica_Disponible_De_Una_Partida_En_Dt_Partidas(
        //                        Partida_ID, ((DataTable)Session[P_Dt_Partidas]),
        //                        Cmb_Dependencia.SelectedValue.Trim(),
        //                        _DataRow[0]["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim(),
        //                        _DataRow[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim());

        //                    if (Disponible_Partida >= Monto_A_Comprometer)
        //                    {
        //                        if (Cmb_Tipo.SelectedValue == "STOCK")
        //                        {
        //                            int Disponible_Producto =
        //                                Verifica_Disponible_De_Un_Producto_De_Stock_En_Dt_Productos(
        //                                    Producto_Servicio_ID, ((DataTable)Session[P_Dt_Productos]));
        //                            if (Disponible_Producto >= 1)
        //                            {
        //                                //Se comprometen los productos en el Dt_Productos, todo es virtual
        //                                Comprometer_O_Descomprometer_Disponible_A_Productos_En_Dt_Productos(
        //                                    Producto_Servicio_ID, ((DataTable)Session[P_Dt_Productos]), Operacion_Comprometer, 1);
        //                            }
        //                            else
        //                            {
        //                                Agregar_Producto = false;
        //                                Mostrar_Informacion(
        //                                    "La cantidad de productos no se encuentra disponible: Existencia [" +
        //                                    Disponible_Producto + "]", true);
        //                            }
        //                        }
        //                        if (Agregar_Producto)
        //                        {
        //                            //Se actualizan los productos
        //                            _DataRow[0]["CANTIDAD"] = (Cantidad_Productos + 1);
        //                            _DataRow[0]["MONTO"] = (Monto + Monto_A_Comprometer);
        //                            _DataRow[0]["MONTO_IEPS"] = (Monto_IEPS + Monto_IEPS_A_Comprometer);
        //                            _DataRow[0]["MONTO_IVA"] = (Monto_IVA + Monto_IVA_A_Comprometer);
        //                            _DataRow[0]["MONTO_TOTAL"] = (Monto_Total + Monto_Total_A_Comprometer);
        //                            Refrescar_Grid();
        //                            Calcular_Impuestos();
        //                            //Comprometer Presupuesto en P_Dt_Partidas
        //                            Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
        //                                Partida_ID, ((DataTable)Session[P_Dt_Partidas]), Operacion_Comprometer, Monto_Total_A_Comprometer);
        //                            //Actualizo grids de productos y partidas
        //                            Actualizar_Grid_Partidas_Productos();
        //                        }
        //                    }
        //                    else
        //                    {
        //                        Mostrar_Informacion(
        //                            "Presupuesto insuficiente en la partida: " +
        //                            _DataRow[0]["PARTIDA_ID"].ToString(), true);
        //                    }
        //                }
        //                else if (Tipo == "SERVICIO")
        //                {
        //                    Mostrar_Informacion(
        //                        "Los Servicios solo pueden tener en cantidad (1) uno", true);
        //                }
        //                break;
        //            case "Menos":
        //                if (Tipo == "PRODUCTO")
        //                {
        //                    int ct = int.Parse(_DataRow[0]["CANTIDAD"].ToString().Trim());
        //                    if (ct > 1)
        //                    {
        //                        double Monto_IEPS_A_Descomprometer = Monto_IEPS / Cantidad_Productos;
        //                        double Monto_IVA_A_Descomprometer = Monto_IVA / Cantidad_Productos;
        //                        double Monto_A_Descomprometer = Monto / Cantidad_Productos;
        //                        double Monto_Total_A_Descomprometer = Monto_Total / Cantidad_Productos;

        //                        //bool Agregar_Producto = true;
        //                        double Disponible_Partida = Verifica_Disponible_De_Una_Partida_En_Dt_Partidas(
        //                            Partida_ID, ((DataTable)Session[P_Dt_Partidas]),
        //                            Cmb_Dependencia.SelectedValue.Trim(),
        //                            _DataRow[0]["FUENTE_FINANCIAMIENTO_ID"].ToString().Trim(),
        //                            _DataRow[0]["PROYECTO_PROGRAMA_ID"].ToString().Trim());
        //                        if (Cmb_Tipo.SelectedValue == "STOCK")
        //                        {
        //                            int Disponible_Producto =
        //                                Verifica_Disponible_De_Un_Producto_De_Stock_En_Dt_Productos(
        //                                    Producto_Servicio_ID, ((DataTable)Session[P_Dt_Productos]));
        //                            //Se descomprometen los productos en el Dt_Productos, todo es virtual
        //                            Comprometer_O_Descomprometer_Disponible_A_Productos_En_Dt_Productos(
        //                                Producto_Servicio_ID, ((DataTable)Session[P_Dt_Productos]), Operacion_Descomprometer, 1);
        //                        }
        //                        //Se actualizan los productos
        //                        //_DataRow[0]["CANTIDAD"] = (Cantidad_Productos - 1);
        //                        //_DataRow[0]["MONTO_TOTAL"] = (Monto_Total - Monto_A_Descomprometer);

        //                        _DataRow[0]["CANTIDAD"] = (Cantidad_Productos - 1);
        //                        _DataRow[0]["MONTO"] = (Monto - Monto_A_Descomprometer);
        //                        _DataRow[0]["MONTO_IEPS"] = (Monto_IEPS - Monto_IEPS_A_Descomprometer);
        //                        _DataRow[0]["MONTO_IVA"] = (Monto_IVA - Monto_IVA_A_Descomprometer);
        //                        _DataRow[0]["MONTO_TOTAL"] = (Monto_Total - Monto_Total_A_Descomprometer);

        //                        //Comprometer Presupuesto en P_Dt_Partidas
        //                        Comprometer_O_Descomprometer_Disponible_A_Partida_En_Dt_Partidas(
        //                            Partida_ID, ((DataTable)Session[P_Dt_Partidas]), Operacion_Descomprometer, Monto_Total_A_Descomprometer);
        //                        //Actualizo grids de productos y partidas
        //                        Actualizar_Grid_Partidas_Productos();
        //                    }
        //                }
        //                else if (Tipo == "SERVICIO")
        //                {
        //                    //AQUI NO PASA NADA
        //                }
        //                break;
        //        }
        //        Refrescar_Grid();
        //        Calcular_Impuestos();

        //        Actualizar_Disponible_Productos("Producto_ID", Producto_Servicio_ID, ((DataTable)Session[P_Dt_Productos]));

        //    }
        //}
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
                case MODO_LISTADO:

                    Configuracion_Acceso("Frm_Ope_Com_Filtrar_Requisiciones_Transitorias.aspx");
                    Btn_Nuevo.Visible = false;
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
                    //Cmb_Programa.Enabled = false;
                    //Cmb_Partida.Enabled = false;
                    Cmb_Tipo.Enabled = false;
                    Cmb_Estatus.Enabled = false;
                    //Ibtn_Buscar_Producto.Enabled = false;
                    //Ibtn_Agregar_Producto.Enabled = false;
                    Grid_Productos_Servicios.Enabled = false;
                    Cmb_Producto_Servicio.Enabled = false;
                    Txt_Total.Enabled = false;
                    //Txt_Producto_Servicio.Enabled = false;
                    Txt_Comentario.Enabled = false;
                    //Txt_Cantidad.Enabled = false;
                    Txt_Justificacion.Enabled = false;
                    Txt_Especificaciones.Enabled = false;
                    Chk_Verificar.Enabled = false;
                    //Div_Contenido.Visible = false;
                    //Div_Listado_Requisiciones.Visible = true;
                    break;

                case MODO_INICIAL:
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Configuracion_Acceso("Frm_Ope_Com_Filtrar_Requisiciones_Transitorias.aspx");
                    Btn_Nuevo.Visible = false;
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
                    //Cmb_Fte_Financiamiento.Enabled = false;
                    //Cmb_Programa.Enabled = false;
                    //Cmb_Partida.Enabled = false;
                    Cmb_Tipo.Enabled = false;
                    Cmb_Estatus.Enabled = false;
                    Cmb_Producto_Servicio.Enabled = false;
                    //Ibtn_Buscar_Producto.Enabled = false;
                    //Ibtn_Agregar_Producto.Enabled = false;                    
                    Grid_Productos_Servicios.Enabled = false;
                    Cmb_Producto_Servicio.Enabled = false;
                    Txt_Comentario.Enabled = false;
                    //Txt_Cantidad.Enabled = false;
                    Txt_Justificacion.Enabled = true;
                    Txt_Especificaciones.Enabled = false;
                    Chk_Verificar.Enabled = false;
                    //Lbl_Disponible_Partida.Text = "$ 0.00";
                    //Lbl_Disponible_Producto.Text = "Disponible: 0 / Precio aproximado: $ 0.00";
                    break;
                //Estado de Nuevo
                case MODO_NUEVO:
                    Btn_Listar_Requisiciones.Visible = false;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Salir.Visible = true;
                    Cmb_Dependencia.Enabled = false;
                    // Cmb_Fte_Financiamiento.Enabled = true;
                    // Cmb_Programa.Enabled = true;
                    //Cmb_Partida.Enabled = true;
                    Cmb_Tipo.Enabled = true;
                    Cmb_Estatus.Enabled = true;
                    // Ibtn_Buscar_Producto.Enabled = true;
                    // Ibtn_Agregar_Producto.Enabled = true;
                    Grid_Productos_Servicios.Enabled = true;
                    Cmb_Producto_Servicio.Enabled = true;
                    Txt_Comentario.Enabled = true;
                    Txt_Comentario.Text = "";
                    //Txt_Cantidad.Enabled = true;
                    Txt_Justificacion.Enabled = true;
                    Txt_Especificaciones.Enabled = true;
                    Chk_Verificar.Enabled = true;
                    //Poner la fecha
                    Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
                    //Verificar tipo empleado logueado
                    Cmb_Estatus.SelectedValue = "EN CONSTRUCCION";
                    Txt_Folio.Text = "Asigna folio al guardar requisición";
                    // Lbl_Disponible_Producto.Text = "Disponible: 0 / Precio aproximado: $ 0.00";
                    Div_Comentarios.Visible = false;
                    break;
                //Estado de Modificar
                case MODO_MODIFICAR:
                    Btn_Listar_Requisiciones.Visible = false;
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Cmb_Dependencia.Enabled = false;

                    Cmb_Tipo.Enabled = false;
                    Cmb_Estatus.Enabled = true;
                    Cmb_Producto_Servicio.Enabled = false;

                    Grid_Productos_Servicios.Enabled = true;
                    Txt_Comentario.Enabled = true;
                    Txt_Comentario.Text = "";

                    Txt_Justificacion.Enabled = true;
                    Txt_Especificaciones.Enabled = false;
                    Chk_Verificar.Enabled = false;
                    Div_Comentarios.Visible = true;

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
        Cmb_Tipo.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        //Cmb_Programa.Items.Clear();
        //Cmb_Partida.Items.Clear();
        Cmb_Producto_Servicio.SelectedIndex = 0;
        Chk_Verificar.Checked = false;
        //Limpiar_Cajas_Texto();
        //Limpiar_Grids_Y_DataTables();        
    }





    ///*******************************************************************************
    // NOMBRE DE LA FUNCIÓN: Llenar_Grid_Requisiciones
    // DESCRIPCIÓN: Llena el grid principal de requisiciones
    // RETORNA: 
    // CREO: Gustavo Angeles Cruz
    // FECHA_CREO: Diciembre/2010 
    // MODIFICO:
    // FECHA_MODIFICO:
    // CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Grid_Requisiciones()
    {
        Div_Contenido.Visible = false;
        Div_Listado_Requisiciones.Visible = true;
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia_Panel.SelectedValue.Trim();//Cls_Sessiones.Dependencia_ID_Empleado.ToString();

        //Requisicion_Negocio.P_Fecha_Inicial = Txt_Fecha_Inicial.Text;
        Requisicion_Negocio.P_Fecha_Inicial = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Inicial.Text);

        Requisicion_Negocio.P_Fecha_Final = Txt_Fecha_Final.Text;
        Requisicion_Negocio.P_Fecha_Final = String.Format("{0:dd-MMM-yyyy}", Txt_Fecha_Final.Text);

        if (Txt_Busqueda.Text.Trim().Length > 0)
        {
            String No_Requisa = Txt_Busqueda.Text;
            No_Requisa = No_Requisa.ToUpper();
            No_Requisa = No_Requisa.Replace("RQ-", "");
            int Int_No_Requisa = 0;
            try
            {
                Int_No_Requisa = int.Parse(No_Requisa);
            }
            catch (Exception Ex)
            {
                String Str = Ex.ToString();
                No_Requisa = "0";
            }
            Requisicion_Negocio.P_Requisicion_ID = No_Requisa;
        }
        Requisicion_Negocio.P_Tipo = TIPO_TRANSITORIA;
        Requisicion_Negocio.P_Estatus = EST_AUTORIZADA;
        Session[P_Dt_Requisiciones] = Requisicion_Negocio.Consultar_Requisiciones();
        if (Session[P_Dt_Requisiciones] != null && ((DataTable)Session[P_Dt_Requisiciones]).Rows.Count > 0)
        {
            Div_Contenido.Visible = false;
            Grid_Requisiciones.DataSource = Session[P_Dt_Requisiciones] as DataTable;
            Grid_Requisiciones.DataBind();
        }
        else
        {
            //Mostrar_Informacion("No se encontraron requisiciones con los criterios de búsqueda", true);
            Grid_Requisiciones.DataSource = Session[P_Dt_Requisiciones] as DataTable;
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
        Txt_Subtotal.Text = "0.0";
        Txt_Total.Text = "0.0";
        Txt_IEPS.Text = "0.0";
        Txt_IVA.Text = "0.0";
        //Llenar combo de tipo
        Cmb_Tipo.Items.Clear();
        Cmb_Tipo.Items.Add("<<SELECCIONAR>>");
        Cmb_Tipo.Items.Add("STOCK");
        Cmb_Tipo.Items.Add("TRANSITORIA");
        Cmb_Tipo.Items[0].Value = "0";
        Cmb_Tipo.Items[0].Selected = true;
        //Llenar combo de Productos Servicios
        Cmb_Producto_Servicio.Items.Clear();
        Cmb_Producto_Servicio.Items.Add("<ELEGIR>");
        Cmb_Producto_Servicio.Items.Add("PRODUCTO");
        Cmb_Producto_Servicio.Items.Add("SERVICIO");
        Cmb_Producto_Servicio.Items[0].Value = "0";
        Cmb_Producto_Servicio.Items[0].Selected = true;
        //Seleccionar combo dependencias
        Cmb_Dependencia.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        //Cmb_Programa.Items.Clear();
        //Cmb_Fte_Financiamiento.Items.Clear();
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Requisicion_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        //DataTable Dt_Fte_Financiamiento = Requisicion_Negocio.Consultar_Fuentes_Financiamiento();
        //Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Fte_Financiamiento, Dt_Fte_Financiamiento, 1, 0);

        //DataTable Data_Table_Proyectos = Requisicion_Negocio.Consultar_Proyectos_Programas();
        //Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Programa, Data_Table_Proyectos, 1, 0);

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
        Cmb_Tipo_Busqueda.Items.Clear();
        Cmb_Tipo_Busqueda.Items.Add(TIPO_TRANSITORIA);
        Cmb_Tipo_Busqueda.Items[0].Selected = true;
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
        Grid_Productos_Servicios.DataSource = ((DataTable)Session[P_Dt_Productos_Servicios]);
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
        if (Cmb_Estatus.SelectedValue.Trim() == "")
        {
            Informacion += "<td>+ Seleccionar Estatus.</td>";
            Bln_Bandera = false;
            Generar_Tabla_Informacion();
        }
        if (Cmb_Dependencia.SelectedIndex == 0)
        {
            //    Informacion += "<td>+ Seleccionar Dependencia.</td>";
            //    Bln_Bandera = false;
            //    Generar_Tabla_Informacion();
        }
        //if (Cmb_Programa.SelectedIndex == 0)
        //{
        //    Informacion += "<td>+ Seleccionar Programa.</td>";
        //    Bln_Bandera = false;
        //    Generar_Tabla_Informacion();
        //}
        //if (Validar_Completo)
        //{
        //    if (Session[P_Dt_Productos_Servicios] != null)
        //    {
        //        if (((DataTable)Session[P_Dt_Productos_Servicios]).Rows.Count == 0)
        //        {
        //            Informacion += "<td>+ Agregar Producto/Servicio.</td>";
        //            Bln_Bandera = false;
        //            Generar_Tabla_Informacion();
        //        }
        //    }
        //    if (Txt_Justificacion.Text.Trim().Length == 0)
        //    {
        //        Informacion += "<td>+ Campo Justificación.</td>";
        //        Bln_Bandera = false;
        //        Generar_Tabla_Informacion();
        //    }

        //    //if (Txt_Comentario.Text.Trim().Length == 0)
        //    //{
        //    //    Informacion += "<td>+ Campo Comentarios.</td>";
        //    //    Bln_Bandera = false;
        //    //    Generar_Tabla_Informacion();
        //    //}

        //    else
        //    {
        //        if (Validar_Longitud(Txt_Comentario, 250))
        //        {
        //            Informacion += "<td>+ Campo comentario excede la longitud permitida.</td>";
        //            Bln_Bandera = false;
        //            Generar_Tabla_Informacion();
        //        }
        //    }
        //}
        Informacion += "</tr></table>";
        return Bln_Bandera;
    }




    #endregion


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Agregar_Tooltip_Combo(DropDownList Combo)
    {
        foreach (ListItem Item in Combo.Items)
        {
            Item.Attributes.Add("Title", Item.Text);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_Servicios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos_Servicios.DataSource = ((DataTable)Session[P_Dt_Productos_Servicios]);
        Grid_Productos_Servicios.PageIndex = e.NewPageIndex;
        Grid_Productos_Servicios.DataBind();
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
        Grid_Sorting(Grid_Requisiciones, ((DataTable)Session[P_Dt_Requisiciones]), e);
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

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
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

    protected void Grid_Requisiciones_RowDataBound(object sender, GridViewRowEventArgs e)
    {

        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    String Folio = e.Row.Cells[1].Text.Trim();

        //    DataRow[] Renglon = ((DataTable)Session[P_Dt_Requisiciones]).Select("FOLIO = '" + Folio + "'");
        //    if (Renglon.Length > 0)
        //    {
        //        ImageButton Boton = (ImageButton)e.Row.FindControl("Btn_Alerta");
        //        String Estatus = Renglon[0]["ESTATUS"].ToString().Trim();
        //        if (Estatus == EST_EN_CONSTRUCCION && Renglon[0]["ALERTA"].ToString().Trim() == "AMARILLO")
        //        {                    
        //            Boton.ImageUrl = "../imagenes/gridview/circle_yellow.png";
        //            Boton.Visible = true;
        //        }
        //        else 
        //        {
        //            Boton.Visible = false;
        //        }
        //    }
        //}
    }
}
