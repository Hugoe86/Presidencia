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
//using Presidencia.Orden_Compra.Negocio;
using Presidencia.Listado_Ordenes_Compra.Negocio;
using Presidencia.Reportes;
using Presidencia.Seguimiento_Ordenes.Negocio;

public partial class paginas_Compras_Frm_Ope_Com_Seguimiento_Ordenes_Compra : System.Web.UI.Page
{
    #region VARIABLES / CONSTANTES
    //objeto de la clase de negocio de dependencias para acceder a la clase de datos y realizar copnexion
    private Cls_Cat_Dependencias_Negocio Dependencia_Negocio;
    //objeto de la clase de negocio de Requisicion para acceder a la clase de datos y realizar copnexion
    private Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio;
    //objeto en donde se guarda un id de producto o servicio para siempre tener referencia
    private int Contador_Columna;
    private String Informacion;

    private static String P_Dt_Productos_Servicios = "P_Dt_Productos_Servicios_Seguimiento_OC";
    //private static String P_Dt_Partidas = "P_Dt_Partidas";
    private static String P_Dt_Requisiciones = "P_Dt_Requisiciones_Transitorias_Seguimiento_OC";
    //private static String P_Dt_Productos_Servicios_Modal = "P_Dt_Productos_Servicios_Modal";

    private const String Operacion_Comprometer = "COMPROMETER";
    private const String Operacion_Descomprometer = "DESCOMPROMETER";
    private const String Operacion_Quitar_Renglon = "QUITAR";
    private const String Operacion_Agregar_Renglon_Nuevo = "AGREGAR_NUEVO";
    private const String Operacion_Agregar_Renglon_Copia = "AGREGAR_COPIA";

    private const String SubFijo_Requisicion = "OC-";
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
            ViewState["SortDirection"] = "DESC";
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            _DateTime = _DateTime.AddMonths(-1);
            //Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Inicial.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            //llenar combo dependencias
            Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
            DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia, Dt_Dependencias, 1, 0);
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Dependencia_Panel, Dt_Dependencias, 1, 0);
            //Cmb_Dependencia.SelectedIndex = 0;
            //Cmb_Dependencia_Panel.SelectedIndex = 0;
            //Cmb_Dependencia_Panel.Enabled = true;
            Cmb_Dependencia_Panel.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            Cmb_Dependencia_Panel.Enabled = false;
            Llenar_Combos_Busqueda();

            Llenar_Grid_Ordenes_Compra();
            Habilitar_Controles(MODO_LISTADO);
            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            if (Dt_Grupo_Rol != null)
            {
                String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                {
                    Cmb_Dependencia_Panel.Enabled = true;
                }
                else
                {
                    DataTable Dt_URs = Cls_Util.Consultar_URs_De_Empleado(Cls_Sessiones.Empleado_ID);
                    if (Dt_URs.Rows.Count > 1)
                    {
                        Cmb_Dependencia_Panel.Enabled = true;
                        Cls_Util.Llenar_Combo_Con_DataTable_Generico
                            (Cmb_Dependencia_Panel, Dt_URs, 1, 0);
                        Cmb_Dependencia_Panel.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
                    }
                }
            }
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
        Llenar_Grid_Ordenes_Compra();
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
        Llenar_Grid_Ordenes_Compra();
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
    private void Evento_Grid_Ordenes_Compra_Seleccionar(String No_Orden_Compra)
    {
        Habilitar_Controles(MODO_INICIAL);
        Llenar_Combos_Generales();
        Div_Listado_Requisiciones.Visible = false;
        Div_Contenido.Visible = true;
        //String No_Orden_Compra = Dato;
        DataRow[] Orden_Compra = ((DataTable)Session[P_Dt_Requisiciones]).Select("No_Orden_Compra = '" + No_Orden_Compra + "'");

        Txt_Folio.Text = Orden_Compra[0][Ope_Com_Requisiciones.Campo_Folio].ToString();
        //Seleccionar combo dependencia
        Cmb_Dependencia.SelectedValue =
            Orden_Compra[0][Ope_Com_Requisiciones.Campo_Dependencia_ID].ToString().Trim();

        DataTable Dt_Historial = Cls_Util.Consultar_Historial_Orden_Compra(No_Orden_Compra);
        Grid_Productos_Servicios.DataSource = Dt_Historial;
        Grid_Productos_Servicios.DataBind();
        //mostrar observaciones
        //DataTable Dt_OC = Cls_Util.Consultar_Historial_Orden_Compra(No_Orden_Compra);
        //Grid_Comentarios.DataSource = Dt_OC;
        //Grid_Comentarios.DataBind();
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
        String No_Orden_Compra = ((ImageButton)sender).CommandArgument;
        Evento_Grid_Ordenes_Compra_Seleccionar(No_Orden_Compra);
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

                    Configuracion_Acceso("Frm_Ope_Com_Seguimiento_Ordenes_Compra.aspx");
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
                    //Txt_Fecha.Enabled = false;
                    Txt_Folio.Enabled = false;
                    Cmb_Dependencia.Enabled = false;

                    break;

                case MODO_INICIAL:
                    Btn_Nuevo.Visible = false;
                    
                    Configuracion_Acceso("Frm_Ope_Com_Seguimiento_Ordenes_Compra.aspx");
                    Btn_Modificar.Visible = false;
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

                    //Txt_Fecha.Enabled = false;
                    Txt_Folio.Enabled = false;
                    Cmb_Dependencia.Enabled = false;

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
    public void Llenar_Grid_Ordenes_Compra()
    {
        Div_Contenido.Visible = false;
        Div_Listado_Requisiciones.Visible = true;
        Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio Negocio = new Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio();
       // Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        //Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia_Panel.SelectedValue.Trim();
        //if (Requisicion_Negocio.P_Dependencia_ID == "0") 
        //{
        //    Requisicion_Negocio.P_Dependencia_ID = "00000";
        //}
        //Requisicion_Negocio.P_Fecha_Inicial = Txt_Fecha_Inicial.Text;
        Negocio.P_Fecha_Inicial = String.Format("{0:dd-MM-yyyy}", Txt_Fecha_Inicial.Text);
        //Negocio.P_Fecha_Final = Txt_Fecha_Final.Text;
        Negocio.P_Fecha_Final = String.Format("{0:dd-MM-yyyy}", Txt_Fecha_Final.Text);
        Negocio.P_Folio = Txt_Busqueda.Text.Trim();
        if (Txt_Busqueda.Text.Trim().Length > 0)
        {
            String No_OC = Txt_Busqueda.Text;
            No_OC = No_OC.ToUpper();
            No_OC = No_OC.Replace("OC-", "");
            int Int_No_OC = 0;
            try
            {
                Int_No_OC = int.Parse(No_OC);
            }
            catch (Exception Ex)
            {
                String Str = Ex.ToString();
                No_OC = "0";
            }
            Negocio.P_No_Orden_Compra = long.Parse(No_OC);
        }
        //Requisicion_Negocio.P_Tipo = Cmb_Tipo_Busqueda.SelectedValue.Trim();
        //Requisicion_Negocio.P_Estatus = EST_AUTORIZADA;
        Session[P_Dt_Requisiciones] = Negocio.Consultar_Ordenes_Compra();
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
        Cmb_Tipo_Busqueda.Items.Add(TIPO_TRANSITORIA);
        Cmb_Tipo_Busqueda.Items.Add(TIPO_STOCK);
        Cmb_Tipo_Busqueda.Items[0].Selected = true;
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
        ImageButton Boton = (ImageButton)sender;
        String No_Orden_Compra = "";
        No_Orden_Compra = Boton.CommandArgument;
        if (Boton.ToolTip == "Imprimir")
        {

            DataTable Dt_Cabecera_OC = new DataTable();
            DataTable Dt_Detalles_OC = new DataTable();
            ImageButton Btn_Imprimir_Orden_Compra = null;
            String Formato = "PDF";
            Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio Listado_Negocio = new Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio();
            try
            {
                Btn_Imprimir_Orden_Compra = (ImageButton)sender;
                No_Orden_Compra = Btn_Imprimir_Orden_Compra.CommandArgument;
                Listado_Negocio.P_No_Orden_Compra = Convert.ToInt64( No_Orden_Compra.Trim());
                // Consultar Cabecera de la Orden de compra
                Dt_Cabecera_OC = Listado_Negocio.Consulta_Cabecera_Orden_Compra();
                // Consultar los detalles de la Orden de compra
                Dt_Detalles_OC = Listado_Negocio.Consulta_Detalles_Orden_Compra();
                // Instanciar el DataSet Fisico
                if (Convert.ToInt32(Dt_Cabecera_OC.Rows[0]["TOTAL"]) >= 50000)
                {
                    Ds_Ope_Com_Orden_Compra Ds_Orden_Compra = new Ds_Ope_Com_Orden_Compra();
                    // Instanciar al método que muestra el reporte
                    Generar_Reporte(Dt_Cabecera_OC, Dt_Detalles_OC, Ds_Orden_Compra, Formato, true);
                }
                else
                {
                    Ds_Ope_Com_Orden_Compra Ds_Orden_Compra = new Ds_Ope_Com_Orden_Compra();
                    // Instanciar al método que muestra el reporte
                    Generar_Reporte(Dt_Cabecera_OC, Dt_Detalles_OC, Ds_Orden_Compra, Formato, false);
                }

                Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio Negocio_Cmp = new Cls_Ope_Com_Seguimiento_Ordenes_Compra_Negocio();
                Negocio_Cmp.P_No_Orden_Compra = long.Parse(No_Orden_Compra);
               
                Llenar_Grid_Ordenes_Compra();
            }
            catch (Exception ex)
            {
                Div_Contenido.Visible = true;
                Lbl_Informacion.Text = (ex.Message);
            }
        }
        else
        {
            DataTable Dt = ((DataTable)Session["Dt_Ordenes_Compra"]);
            if (Boton.ToolTip == "Rechazada")
            {
                DataRow[] _DataRow = ((DataTable)Session["Dt_Ordenes_Compra"]).Select(Ope_Com_Ordenes_Compra.Campo_No_Orden_Compra + " = " + No_Orden_Compra);
                //Boton.ToolTip = _DataRow[0][Ope_Com_Ordenes_Compra.Campo_Comentarios].ToString();
            }
            ScriptManager.RegisterStartupScript(
               this, this.GetType(), "Requisiciones", "alert('" + Boton.ToolTip + "');", true);
        }    

    }

    #region REPORTES


    //$$$$$$$$$$$$$$$$$$$$


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.- Dt_Cabecera.- Contiene la informacion general de la orden de compra
    ///                      2.- Dt_Detalles.- Contiene los productos de la orden de compra
    ///                      3.- Ds_Recibo.- Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataTable Dt_Cabecera, DataTable Dt_Detalles, DataSet Ds_Reporte, String Formato, bool Tesorero)
    {

        DataRow Renglon;
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        DataTable Dt_Dir;
        Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Listado = new Cls_Ope_Com_Listado_Ordenes_Compra_Negocio();
        Dt_Dir = Listado.Consulta_Directores();

        if (Dt_Dir.Rows.Count > 0)
        {
            Dt_Cabecera.Columns.Add("DIRECTOR_ADQUISICIONES", typeof(String));
            Dt_Cabecera.Columns.Add("OFICIALIA_MAYOR", typeof(String));
            Dt_Cabecera.Columns.Add("TESORERO", typeof(String));

            Dt_Cabecera.Rows[0]["DIRECTOR_ADQUISICIONES"] = Dt_Dir.Rows[0]["DIRECTOR_ADQUISICIONES"];
            Dt_Cabecera.Rows[0]["OFICIALIA_MAYOR"] = Dt_Dir.Rows[0]["OFICIALIA_MAYOR"];
            Dt_Cabecera.Rows[0]["TESORERO"] = Dt_Dir.Rows[0]["TESORERO"];

            Renglon = Dt_Cabecera.Rows[0];
            Ds_Reporte.Tables[0].ImportRow(Renglon);

            String Folio = Dt_Cabecera.Rows[0]["FOLIO"].ToString();

            for (int Cont_Elementos = 0; Cont_Elementos < Dt_Detalles.Rows.Count; Cont_Elementos++)
            {
                Renglon = Dt_Detalles.Rows[Cont_Elementos]; //Instanciar renglon e importarlo
                Ds_Reporte.Tables[1].ImportRow(Renglon);
                Ds_Reporte.Tables[1].Rows[Cont_Elementos].SetField("FOLIO", Folio);
            }

            // Ruta donde se encuentra el reporte Crystal
            if (Tesorero)
            {
                Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Ope_Com_Orden_Compra_Tes.rpt";
            }
            else
            {
                Ruta_Reporte_Crystal = "../Rpt/Compras/Rpt_Ope_Com_Orden_Compra.rpt";
            }

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Rpt_List_OrdenC_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            // Se da el nombre del reporte que se va generar
            if (Formato == "PDF")
                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            else if (Formato == "Excel")
                Nombre_Reporte_Generar = Nombre_Reporte + ".xls";  // Es el nombre del repote en Excel que se va a generar

            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
            Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
        }
        else
        {
            throw new Exception("Error al Intentar consultar los Directores que autorizarán la Orden de compra ");
        }
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



    #endregion
}
