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
using Presidencia.Compras.Impresion_Requisiciones.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Compras_Frm_Ope_Com_Autorizar_Requisiciones_Stock : System.Web.UI.Page
{
    #region VARIABLES / CONSTANTES
    //objeto de la clase de negocio de dependencias para acceder a la clase de datos y realizar copnexion
    private Cls_Cat_Dependencias_Negocio Dependencia_Negocio;
    //objeto de la clase de negocio de Requisicion para acceder a la clase de datos y realizar copnexion
    private Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio;
    //objeto en donde se guarda un id de producto o servicio para siempre tener referencia
    private int Contador_Columna;
    private String Informacion;
    private static String P_Dt_Productos_Servicios = "P_Dt_Productos_Servicios_Autoriza_STOCK";
    //private static String P_Dt_Partidas = "P_Dt_Partidas";
    private static String P_Dt_Requisiciones = "P_Dt_Requisiciones_Transitorias_Autoriza_STOCK";
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
            Cls_Sessiones.Mostrar_Menu = true;
            ViewState["SortDirection"] = "DESC";
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            Txt_Fecha_Inicial.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            //llenar combo dependencias
            Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia, Dt_Dependencias, 1, 0);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Panel, Dt_Dependencias, 1, 0);
            //Cmb_Dependencia.SelectedIndex = 0;
            Cmb_Dependencia_Panel.SelectedIndex = 0;
            Cmb_Dependencia_Panel.Enabled = true;
            Llenar_Combos_Busqueda();
            Llenar_Grid_Requisiciones();
            Habilitar_Controles(MODO_LISTADO);
            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            //DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            //if (Dt_Grupo_Rol != null)
            //{
            //    String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
            //    if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
            //    {
            //        Cmb_Dependencia_Panel.Enabled = true;
            //    }
            //    else
            //    {
            //        DataTable Dt_URs = Cls_Util.Consultar_URs_De_Empleado(Cls_Sessiones.Empleado_ID);
            //        if (Dt_URs.Rows.Count > 1)
            //        {
            //            Cmb_Dependencia_Panel.Enabled = true;
            //            Cls_Util.Llenar_Combo_Con_DataTable_Generico
            //                (Cmb_Dependencia_Panel, Dt_URs, 1, 0);
            //            Cmb_Dependencia_Panel.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            //        }
            //    }
            //}
        }
        Mostrar_Informacion("", false);
    }
    #endregion

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
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
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
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
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
        Habilitar_Controles(MODO_INICIAL);
        Llenar_Grid_Requisiciones();
    }

    ///*******************************************************************************





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
        //DataTable Dt_Comentarios = (DataTable)Grid_Comentarios.DataSource;
        Habilitar_Controles(MODO_INICIAL);
        //Txt_Justificacion.Visible = true;
  
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
        Txt_Codigo_Programatico.Text = Requisicion[0][Ope_Com_Requisiciones.Campo_Codigo_Programatico].ToString();
        Txt_Justificación.Text = Requisicion[0][Ope_Com_Requisiciones.Campo_Justificacion_Compra].ToString();
        double importes = double.Parse(Requisicion[0][Ope_Com_Requisiciones.Campo_Total].ToString().Trim());

        Txt_Importe.Text = String.Format("{0:C}",importes);
        //Seleccionar combo dependencia
        Cmb_Dependencia.SelectedValue =
            Requisicion[0][Ope_Com_Requisiciones.Campo_Dependencia_ID].ToString().Trim();
        Requisicion_Negocio.P_Requisicion_ID = No_Requisicion;
        DataTable Dt_Historial = Requisicion_Negocio.Consultar_Historial_Requisicion();       
        Grid_Productos_Servicios.DataSource = Dt_Historial;
        Grid_Productos_Servicios.DataBind();
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
        //GridViewRow Renglon = Grid_Comentarios.SelectedRow;
        //Txt_Comentario.Text = Renglon.Cells[1].Text;
    }







    #endregion

    #region METODOS




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

                    //Configuracion_Acceso("Frm_Ope_Com_Autorizar_Requisiciones_Stock.aspx");                                          
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = true;
                    Btn_Guardar.Visible = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Eliminar.ToolTip = "Eliminar";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Listar_Requisiciones.Visible = false;
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    //Txt_Fecha.Enabled = false;
                    Txt_Folio.Enabled = false;
                    Cmb_Dependencia.Enabled = false;
                    Lbl_Motivo_Rechazo.Visible = false;
                    Txt_Motivo_Rechazo.Visible = false;
                    Cmb_Estatus.SelectedValue = "ALMACEN";
                    Txt_No_Reserva.Text = "";
                    break;

                case MODO_INICIAL:
                    Btn_Nuevo.Visible = false;
                   // Configuracion_Acceso("Frm_Ope_Com_Autorizar_Requisiciones_Stock.aspx");
                    Btn_Modificar.Visible = false;
                    Btn_Nuevo.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.Visible = false;
                    Btn_Guardar.Visible = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Eliminar.ToolTip = "Eliminar";
                    Btn_Salir.ToolTip = "Inicio";

                    Btn_Listar_Requisiciones.Visible = true;
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Eliminar.ImageUrl = "~/paginas/imagenes/paginas/icono_eliminar.png";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                    //Txt_Fecha.Enabled = false;
                    Txt_Folio.Enabled = false;
                    Cmb_Dependencia.Enabled = false;
                    Cmb_Estatus.SelectedValue = "ALMACEN";
                    Txt_No_Reserva.Text = "";
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
                    Cmb_Estatus.SelectedValue = "ALMACEN";
                    Txt_No_Reserva.Text = "";
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
        Habilitar_Controles(MODO_LISTADO);
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        //Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia_Panel.SelectedValue.Trim();
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
        Requisicion_Negocio.P_Tipo = "STOCK";// Cmb_Tipo_Busqueda.SelectedValue.Trim();
        Requisicion_Negocio.P_Estatus = EST_AUTORIZADA;
        Session[P_Dt_Requisiciones] = Requisicion_Negocio.Consultar_Requisiciones_Generales();
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

        Cmb_Dependencia.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        //Cmb_Programa.Items.Clear();
        //Cmb_Fte_Financiamiento.Items.Clear();
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Requisicion_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado.ToString();



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
        Cmb_Tipo_Busqueda.Items.Add("TODOS");
       // Cmb_Tipo_Busqueda.Items.Add(TIPO_TRANSITORIA);
        Cmb_Tipo_Busqueda.Items.Add(TIPO_STOCK);
        Cmb_Tipo_Busqueda.Items[0].Selected = true;

        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("ALMACEN");
        Cmb_Estatus.Items.Add(EST_AUTORIZADA);
        Cmb_Estatus.Items.Add("RECHAZADA");
        Cmb_Estatus.Items[0].Selected = true;
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
    protected void Btn_Imprimir_Requisicion_Click(object sender, ImageClickEventArgs e)
    {
        String No_Requisicion = ((ImageButton)sender).CommandArgument;
        DataSet Ds_Reporte = null;
        DataTable Dt_Requisicion = null;
        Cls_Ope_Com_Impresion_Requisiciones_Negocio Req_Negocio = new Cls_Ope_Com_Impresion_Requisiciones_Negocio();
        DataTable Dt_Cabecera = new DataTable();
        DataTable Dt_Detalles = new DataTable();
        try
        {
            String Requisicion_ID = No_Requisicion;//Txt_Folio.Text.Replace("RQ-", "");
            Req_Negocio.P_Requisicion_ID = Requisicion_ID.Trim();
            //Txt_Folio.Text.Replace("RQ-","");
            Dt_Cabecera = Req_Negocio.Consultar_Requisiciones();
            Dt_Detalles = Req_Negocio.Consultar_Requisiciones_Detalles();
            //Ds_Ope_Com_Requisiciones Ds_Requisiciones = new Ds_Ope_Com_Requisiciones();
            //    Generar_Reporte(Dt_Cabecera, Dt_Detalles, Ds_Requisiciones);
            Ds_Reporte = new DataSet();
            //Dt_Requisicion = Req_Negocio.Consultar_Requisiciones();
            Dt_Cabecera.TableName = "REQUISICION";
            Dt_Detalles.TableName = "DETALLES";
            Ds_Reporte.Tables.Add(Dt_Cabecera.Copy());
            Ds_Reporte.Tables.Add(Dt_Detalles.Copy());
            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ope_Com_Requisiciones.rpt", "Requisicion.pdf");
        }
        catch (Exception Ex)
        {
            Mostrar_Informacion(Ex.Message.ToString(), true);
        }

    }
    private void Asignar_Reserva()
    {
        Cls_Ope_Com_Requisiciones_Negocio Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Negocio.P_Folio = Txt_Folio.Text.Trim();
        Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
        Negocio.P_No_Reserva = Txt_No_Reserva.Text.Trim();
        int Renglones_Afectados = Negocio.Asignar_Reserva(); //Asignar_No_Reserva();
        String Mensaje = "";
        if (Renglones_Afectados > 0)
        {
            Mensaje = "Requisición actualizada";
        }
        else
        {
            Mensaje = "No se actualizaron registros";
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
    }
    private void Rechazar_Requisicion_Por_Contabilidad()
    {
        Cls_Ope_Com_Requisiciones_Negocio Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Negocio.P_Folio = Txt_Folio.Text.Trim();
        Negocio.P_Requisicion_ID = Negocio.P_Folio.Replace("RQ-","");
        Negocio.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
        Negocio.P_Comentarios = Txt_Motivo_Rechazo.Text.Trim();
        Negocio.P_No_Reserva = Txt_No_Reserva.Text.Trim();
        int Renglones_Afectados = Negocio.Rechaza_Contabilidad();//Asignar_No_Reserva();
        String Mensaje = "";
        if (Renglones_Afectados > 0)
        {
            Mensaje = "Requisición actualizada";
        }
        else
        {
            Mensaje = "No se actualizaron registros, verifique con el administrador del sistema";
        }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
    }
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        String Estatus = Cmb_Estatus.SelectedValue.Trim();
        switch(Estatus)
        {
            case "ALMACEN":
                if (Txt_No_Reserva.Text.Trim().Length > 0)
                {
                    Asignar_Reserva();
                    Llenar_Grid_Requisiciones();
                }
                else
                {
                    Mostrar_Informacion("Debe ingresar el número de reserva", true);
                }
                break;
            case "RECHAZADA":
                if (Txt_Motivo_Rechazo.Text.Trim().Length > 0)
                {
                    //Asignar_Reserva();
                    Rechazar_Requisicion_Por_Contabilidad();
                    Llenar_Grid_Requisiciones();
                }
                else
                {
                    Mostrar_Informacion("Debe ingresar motivo de rechazo", true);
                }
                break;
            default:
                Mostrar_Informacion("Debe ingresar el número de reserva", true);
                break;               
        }


    }
    protected void Cmb_Estatus_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Estatus.SelectedValue == "RECHAZADA")
        {
            Txt_Motivo_Rechazo.Visible = true;
            Lbl_Motivo_Rechazo.Visible = true;
        }
        else
        {
            Txt_Motivo_Rechazo.Visible = false;
            Lbl_Motivo_Rechazo.Visible = false; 
        }
    }
    #region REPORTES

    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Compras/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window",
                "window.open('" + Pagina + "', 'Requisición','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

}
