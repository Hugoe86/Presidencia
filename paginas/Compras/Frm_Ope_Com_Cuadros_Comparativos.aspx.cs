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
using Presidencia.Cuadro_Comparativo.Negocio;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Orden_Compra.Negocio;
using Presidencia.Imprimir_Propuestas.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;


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

            ViewState["SortDirection"] = "DESC";
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);

            Habilitar_Controles(MODO_LISTADO);
            Llenar_Combo_Cotizadores();
            Llenar_Grid_Requisiciones();

            Div_Principal.Visible = true;
            Div_Secundario.Visible = false;

            //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
            DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
            if (Dt_Grupo_Rol != null)
            {
                String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
                if (Grupo_Rol == "00001" || Grupo_Rol == "00002")
                {
                    Cmb_Cotizadores.Enabled = true;
                }
                else
                {
                    Cmb_Cotizadores.Enabled = false;
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
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Regresar")
        {
            Div_Principal.Visible = true;
            Div_Secundario.Visible = false;
            Btn_Salir.ToolTip = "Inicio";
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
    protected void Cmb_Cotizadores_SelectedIndexChanged(object sender, EventArgs e)
    {
        Llenar_Grid_Requisiciones();
        //Llenar_Grid_Ordenes_Compra();
        //Llenar_Grid_Ordenes_Compra("GENERADA','AUTORIZADA','RECHAZADA", "NO", "", "", "");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN:          Evento utilizado llenar el Grid con las ordenes de compra
    ///PARAMETROS:          
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           19/Abril/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Requisiciones();
        //Llenar_Grid_Ordenes_Compra();
        //Llenar_Grid_Ordenes_Compra("", "SI", Txt_Busqueda.Text.Trim(), Txt_Fecha_Inicial.Text, Txt_Fecha_Final.Text);
    }
    #endregion

    #region EVENTOS GRID


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Requisicion_Click
    ///DESCRIPCIÓN: 
    ///
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Mostrar_Cuadro_Tecnico_Click(object sender, ImageClickEventArgs e)
    {
        String No_Requisicion = ((ImageButton)sender).CommandArgument;
        Crear_Cuadro_Comparativo_Aspectos_Tecnicos(No_Requisicion);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Requisicion_Click
    ///DESCRIPCIÓN: 
    ///
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 9/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Mostrar_Cuadro_Economico_Click(object sender, ImageClickEventArgs e)
    {
        String No_Requisicion = Txt_No_Requisicion.Text.Replace("RQ-", "");//((ImageButton)sender).CommandArgument;
        Crear_Cuadro_Comparativo_Economico(No_Requisicion);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Seleccionar_Click
    ///DESCRIPCIÓN: 
    ///
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 27 Marzo 2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Click(object sender, ImageClickEventArgs e)
    {

        String No_Requisicion = ((ImageButton)sender).CommandArgument;
        //verificar si rq es dividida
        DataTable Dt_RQ = Session[P_Dt_Requisiciones] as DataTable;
        DataRow[] Dr_Rows = Dt_RQ.Select("NO_REQUISICION = " + No_Requisicion);
        Session["CODIGO_PROGRAMATICO"] = Dr_Rows[0]["CODIGO_PROGRAMATICO"].ToString().Trim();
        Session["NOMBRE_DEPENDENCIA"] = Dr_Rows[0]["NOMBRE_DEPENDENCIA"].ToString().Trim();
        bool Flag = Convert.IsDBNull(Dr_Rows[0]["REQ_ORIGEN_ID"]);
        if (Flag == false)
        {
            No_Requisicion = Dr_Rows[0]["REQ_ORIGEN_ID"].ToString().Trim();
            Lbl_Requisicion_Dividida.Visible = true;
        }
        else
        {
            Lbl_Requisicion_Dividida.Visible = false;
        }

        Div_Principal.Visible = false;
        Div_Secundario.Visible = true;
        Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio = new Cls_Ope_Com_Cuadro_Comparativo_Negocio();
        Negocio.P_No_Requisicion = No_Requisicion;
        //DataTable Dt_Requisicion = (DataTable)Session[P_Dt_Requisiciones];
        //DataTable Dt_Productos = Negocio.Consultar_Productos_Requisicion();
        DataTable Dt_Proveedores = Negocio.Consultar_Proveedores_Que_Cotizaron();
        Btn_Salir.ToolTip = "Regresar";
        Txt_No_Requisicion.Text = "RQ-" + No_Requisicion;
        Grid_Proveedores.DataSource = Dt_Proveedores;
        Grid_Proveedores.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Cotizacion_Click
    ///DESCRIPCIÓN: 
    ///
    ///CREO: Gustavo Angeles
    ///FECHA_CREO: 27 Marzo 2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Cotizacion_Click(object sender, ImageClickEventArgs e)
    {
        String Proveedor_ID = ((ImageButton)sender).CommandArgument;
        Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();
        Clase_Negocio.P_Proveedor_ID = Proveedor_ID;
        Clase_Negocio.P_No_Requisicion = Txt_No_Requisicion.Text.Replace("RQ-", "");
        Clase_Negocio.P_Archivo_PDF = "Cotizacion_RQ-" + Clase_Negocio.P_No_Requisicion + "_" + Proveedor_ID + ".pdf";
        Clase_Negocio.P_Ruta_RPT = @Server.MapPath("../Rpt/Compras/Rpt_Ope_Com_Cotizacion_Proveedor.rpt");
        Clase_Negocio.P_Ruta_Exportacion = @Server.MapPath("../../Reporte/" + Clase_Negocio.P_Archivo_PDF);
        String Reporte = Clase_Negocio.Imprimir_Cotizacion();
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window",
                "window.open('" + Reporte + "', 'Requisición','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }

    }

    #endregion

    #region METODOS
    ///*******************************************************************************
    //NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cotizadores
    //DESCRIPCIÓN:Llenar_Combo_Cotizadores
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: 14/Oct/2011 
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    private void Llenar_Combo_Cotizadores()
    {
        Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        DataTable Dt_Cotizadores = Negocio_Compra.Consultar_Cotizadores();
        if (Dt_Cotizadores != null && Dt_Cotizadores.Rows.Count > 0)
        {
            Cls_Util.Llenar_Combo_Con_DataTable_Generico
                (Cmb_Cotizadores, Dt_Cotizadores, Cat_Com_Cotizadores.Campo_Nombre_Completo, Cat_Com_Cotizadores.Campo_Empleado_ID);
            Cmb_Cotizadores.SelectedValue = Cls_Sessiones.Empleado_ID;
        }
        else
        {
            Cmb_Cotizadores.Items.Clear();
            Cmb_Cotizadores.Items.Add("COTIZADORES");
            Cmb_Cotizadores.SelectedIndex = 0;
        }
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
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    break;

                case MODO_INICIAL:
                    Btn_Salir.Visible = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    break;
                //Estado de Nuevo
                case MODO_NUEVO:
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Salir.Visible = true;

                    break;
                //Estado de Modificar
                case MODO_MODIFICAR:
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.Visible = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
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
        Div_Listado_Requisiciones.Visible = true;
        Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
        Requisicion_Negocio.P_Dependencia_ID = Cls_Sessiones.Dependencia_ID_Empleado.ToString();
        Requisicion_Negocio.P_Folio = Txt_Busqueda.Text;
        Requisicion_Negocio.P_Tipo = TIPO_TRANSITORIA;
        Requisicion_Negocio.P_Cotizador_ID = Cmb_Cotizadores.SelectedValue;
        //Verificar si su rol es jefe de dependencia, admin de modulo o admin de sistema
        //DataTable Dt_Grupo_Rol = Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Rol_ID.ToString());
        //if (Dt_Grupo_Rol != null)
        //{
        //    String Grupo_Rol = Dt_Grupo_Rol.Rows[0][Apl_Cat_Roles.Campo_Grupo_Roles_ID].ToString();
        //    if (Grupo_Rol == "00001" || Grupo_Rol == "00002" )
        //    {
        //        Cmb_Cotizadores.Enabled = true;
        //    }
        //    else 
        //    {
        //        Cmb_Cotizadores.Enabled = true;
        //    }
        //}



        //Requisicion_Negocio.P_Estatus = "'PROVEEDOR','COTIZADA','COMPRA',''";
        DataTable _DataTable = Requisicion_Negocio.Consultar_Requisiciones_En_Web();
        Session[P_Dt_Requisiciones] = _DataTable;
        if (Session[P_Dt_Requisiciones] != null && ((DataTable)Session[P_Dt_Requisiciones]).Rows.Count > 0)
        {
            Grid_Requisiciones.DataSource = Session[P_Dt_Requisiciones] as DataTable;
            Grid_Requisiciones.DataBind();
        }
        else
        {
            Grid_Requisiciones.DataSource = Session[P_Dt_Requisiciones] as DataTable;
            Grid_Requisiciones.DataBind();
        }
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


    #endregion

    private void Crear_Cuadro_Comparativo_Economico(String No_Requisicion)
    {
        Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio = new Cls_Ope_Com_Cuadro_Comparativo_Negocio();
        Negocio.P_No_Requisicion = No_Requisicion;
        DataTable Dt_Requisicion = (DataTable)Session[P_Dt_Requisiciones];
        DataTable Dt_Productos = Negocio.Consultar_Productos_Requisicion();
        DataTable Dt_Proveedores = Negocio.Consultar_Proveedores_Que_Cotizaron();
        DataTable Dt_Productos_Cotizados = null;
        int No_Proveedores_Adjudicados = 0;
        String Nombres_Proveedores_Adjudicados = "";

        DataRow[] Rows = null;
        String Nombre_Unidad_Responsable = null;
        String Nombre_Fuente_Financiamiento = null;
        String Nombre_Partida = null;
        String Codigo_Programatico = null;
        Rows = Dt_Requisicion.Select("NO_REQUISICION = '" + No_Requisicion + "'");
        Codigo_Programatico = Session["CODIGO_PROGRAMATICO"].ToString().Trim();
        Nombre_Unidad_Responsable = Session["NOMBRE_DEPENDENCIA"].ToString().Trim();

        //Obtengo la fuente de financiamieto
        Nombre_Fuente_Financiamiento = Dt_Productos.Rows[0]["NOMBRE_FUENTE"].ToString().Trim();
        //Obtengo el nmombre de la partida
        Nombre_Partida = Dt_Productos.Rows[0]["NOMBRE_PARTIDA"].ToString().Trim();
        //LIBRERIAS C.A.G.
        //####################
        Workbook book = new Workbook();
        WorksheetStyle Encabezado_Reporte_Style = book.Styles.Add("Encabezado_Reporte");
        WorksheetStyle Hstyle = book.Styles.Add("Encabezado");
        WorksheetStyle Nombre_Proveedor_Style = book.Styles.Add("Nombre_Proveedor");
        WorksheetStyle Dstyle = book.Styles.Add("Detalles");
        WorksheetStyle Detalles_Ganador_Style = book.Styles.Add("Detalles_Ganador");
        WorksheetStyle Numero_Style = book.Styles.Add("Numero");
        WorksheetStyle Numero_Ganador_Style = book.Styles.Add("Numero_Ganador");
        WorksheetStyle Descripcion_Producto_Style = book.Styles.Add("Descripcion_Producto");
        WorksheetStyle Celda_Combinada_Style = book.Styles.Add("Celda_Combinada");
        WorksheetStyle Datos_Generales_Style = book.Styles.Add("Datos_Generales");
        WorksheetStyle Totales_Style = book.Styles.Add("Totales");
        WorksheetStyle Titulo_Datos_Generales_Style = book.Styles.Add("Titulo_Datos_Generales");
        WorksheetStyle Encabezado_Principal_Style = book.Styles.Add("Encabezado_Principal");
        WorksheetStyle Pie_Pagina_Style = book.Styles.Add("Pie_Pagina");
        WorksheetStyle Fundamento_Style = book.Styles.Add("Fundamento");
        Worksheet sheet = book.Worksheets.Add("Cuadro_Economico");
        WorksheetRow row;
        WorksheetCell Celda;

        String Nombre_Archivo = "Cuadro_Comparativo.xls";
        String Ruta_Archivo = @Server.MapPath("../../Archivos/" + Nombre_Archivo);
        book.ExcelWorkbook.ActiveSheetIndex = 1;
        book.Properties.Author = "Municipio Irapuato";
        book.Properties.Created = DateTime.Now;


        Encabezado_Reporte_Style.Font.FontName = "Arial";
        Encabezado_Reporte_Style.Font.Size = 10;
        Encabezado_Reporte_Style.Font.Color = "Black";
        Encabezado_Reporte_Style.Font.Bold = true;
        Encabezado_Reporte_Style.Alignment.Horizontal = StyleHorizontalAlignment.Left;

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

        //Estilo Nombre_Proveedor
        Nombre_Proveedor_Style.Font.FontName = "Arial";
        Nombre_Proveedor_Style.Font.Size = 10;
        Nombre_Proveedor_Style.Font.Bold = true;
        Nombre_Proveedor_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        Nombre_Proveedor_Style.Alignment.WrapText = true;
        Nombre_Proveedor_Style.Font.Color = "Black";
        Nombre_Proveedor_Style.Interior.Color = "LightGray";
        Nombre_Proveedor_Style.Interior.Pattern = StyleInteriorPattern.Solid;
        Nombre_Proveedor_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        Nombre_Proveedor_Style.Alignment.Vertical = StyleVerticalAlignment.Top;
        Nombre_Proveedor_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
        Nombre_Proveedor_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
        Nombre_Proveedor_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
        Nombre_Proveedor_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

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


        //Estilo de la hoja de detalles
        Detalles_Ganador_Style.Font.FontName = "Arial";
        Detalles_Ganador_Style.Font.Size = 10;
        Detalles_Ganador_Style.Font.Color = "Black";
        Detalles_Ganador_Style.Interior.Color = "Yellow";
        Detalles_Ganador_Style.Interior.Pattern = StyleInteriorPattern.Solid;
        Detalles_Ganador_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        Detalles_Ganador_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
        Detalles_Ganador_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
        Detalles_Ganador_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
        Detalles_Ganador_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

        //Estilo de la hoja de detalles
        Descripcion_Producto_Style.Font.FontName = "Arial";
        Descripcion_Producto_Style.Font.Size = 10;
        Descripcion_Producto_Style.Font.Color = "Black";
        Descripcion_Producto_Style.Alignment.WrapText = true;

        Descripcion_Producto_Style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        Descripcion_Producto_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
        Descripcion_Producto_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
        Descripcion_Producto_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
        Descripcion_Producto_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

        //Estilo de la hoja de detalles numero
        Numero_Style.Font.FontName = "Arial";
        Numero_Style.Font.Size = 10;
        Numero_Style.Font.Color = "Black";
        Numero_Style.NumberFormat = "$#,##0.00";
        Numero_Style.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        Numero_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
        Numero_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
        Numero_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
        Numero_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

        //Estilo de la hoja de detalles numero
        Numero_Ganador_Style.Font.FontName = "Arial";
        Numero_Ganador_Style.Font.Size = 10;
        Numero_Ganador_Style.Font.Color = "Black";
        Numero_Ganador_Style.NumberFormat = "$#,##0.00";
        Numero_Ganador_Style.Interior.Color = "Yellow";
        Numero_Ganador_Style.Interior.Pattern = StyleInteriorPattern.Solid;
        Numero_Ganador_Style.Alignment.Horizontal = StyleHorizontalAlignment.Right;
        Numero_Ganador_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
        Numero_Ganador_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
        Numero_Ganador_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
        Numero_Ganador_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

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

        //Estilo de Datos_Generales
        Totales_Style.Font.FontName = "Arial";
        Totales_Style.Font.Size = 10;
        Totales_Style.Font.Color = "Black";
        Totales_Style.Font.Bold = true;
        Totales_Style.NumberFormat = "###";
        Totales_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
        Totales_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
        Totales_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
        Totales_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
        Totales_Style.Alignment.Horizontal = StyleHorizontalAlignment.Right;

        //Estilo de Encabezado Principal
        Encabezado_Principal_Style.Font.FontName = "Arial";
        Encabezado_Principal_Style.Font.Size = 10;
        Encabezado_Principal_Style.Font.Color = "Black";
        Encabezado_Principal_Style.Font.Bold = true;
        Encabezado_Principal_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;

        //Estilo Fundamento
        Fundamento_Style.Font.FontName = "Arial";
        Fundamento_Style.Font.Size = 10;
        Fundamento_Style.Font.Color = "Black";
        Fundamento_Style.Font.Bold = true;
        Fundamento_Style.Alignment.Horizontal = StyleHorizontalAlignment.Left;
        Fundamento_Style.Alignment.Vertical = StyleVerticalAlignment.Top;
        Fundamento_Style.Alignment.WrapText = true;
        Fundamento_Style.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
        Fundamento_Style.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
        Fundamento_Style.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
        Fundamento_Style.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");

        //Estilo Pie Pagina
        Pie_Pagina_Style.Font.FontName = "Arial";
        Pie_Pagina_Style.Font.Size = 10;
        Pie_Pagina_Style.Font.Color = "Black";
        Pie_Pagina_Style.Font.Bold = true;
        Pie_Pagina_Style.Alignment.Horizontal = StyleHorizontalAlignment.Center;
        Pie_Pagina_Style.Alignment.Vertical = StyleVerticalAlignment.Top;

        //###################
        sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
        sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));
        sheet.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(280));

        row = sheet.Table.Rows.Add();
        row.Cells.Add(new WorksheetCell("", "Encabezado_Reporte"));

        WorksheetRow Renglon1_Datos_Proveedor;
        WorksheetRow Renglon2_Datos_Proveedor;
        WorksheetRow Renglon_Encabezado_De_Informacion;
        WorksheetRow Renglon_Informacion_Producto;

        int Contador_Lineas = 0;

        try
        {
            int Inicio = 10;
            int C = 3;
            int R = Inicio;

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 6, "OFICIALÍA MAYOR");

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 6, "DIRECCIÓN DE ADQUISICIONES");

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 6, "REQUISICIÓN NO. " + Negocio.P_No_Requisicion + ", " + Nombre_Partida);

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 6, "UNIDAD RESPONSABLE: " + Nombre_Unidad_Responsable);

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 6, "FUENTE DE RECURSOS: " + Nombre_Fuente_Financiamiento);

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 6, "CODIGO PROGRAMATICO: " + Codigo_Programatico);

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 10, "");

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Principal", Dt_Proveedores.Rows.Count * 3 + 2, "CUADRO COMPARATIVO");

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Encabezado_Reporte", 10, "");

            Renglon1_Datos_Proveedor = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(Renglon1_Datos_Proveedor, 3, null, "");

            Renglon2_Datos_Proveedor = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(Renglon2_Datos_Proveedor, 3, null, "");

            Renglon_Encabezado_De_Informacion = sheet.Table.Rows.Add();
            Renglon_Encabezado_De_Informacion.Cells.Add(new WorksheetCell("UNIDAD", "Encabezado"));
            Renglon_Encabezado_De_Informacion.Cells.Add(new WorksheetCell("CANTIDAD", "Encabezado"));
            Celda = Renglon_Encabezado_De_Informacion.Cells.Add("DESCRIPCIÓN");
            Celda.StyleID = "Encabezado";

            //Renglon_Informacion_Producto = sheet.Table.Rows.Add();
            foreach (DataRow Producto in Dt_Productos.Rows)
            {
                Renglon_Informacion_Producto = sheet.Table.Rows.Add();
                Renglon_Informacion_Producto.Cells.Add(new WorksheetCell(Producto["UNIDAD"].ToString(), "Detalles"));
                Renglon_Informacion_Producto.Cells.Add(new WorksheetCell(Producto["CANTIDAD"].ToString(), "Detalles"));
                Celda = Renglon_Informacion_Producto.Cells.Add(Producto["NOMBRE_PRODUCTO_SERVICIO"].ToString());
                Celda.StyleID = "Descripcion_Producto";
                Celda.Row.AutoFitHeight = true;
            }

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "SUBTOTAL");

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "IVA");

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "TOTAL");

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "TIEMPO DE ENTREGA");

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "CONDICIONES DE PAGO");

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "GARANTIA");

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "VIGENCIA DE COTIZACION");

            row = sheet.Table.Rows.Add();
            Rellenar_Con_Celdas(row, 2, null, null);
            Celda_Combinada(row, "Totales", 0, "OBSERVACIONES");

            WorksheetRow Renglon_Cabecera_Datos = sheet.Table.Rows.Add();
            foreach (DataRow Proveedor in Dt_Proveedores.Rows)
            {

                //Datos del proveedor
                Negocio.P_Proveedor_ID = Proveedor["PROVEEDOR_ID"].ToString().Trim();
                //ajustar texto
                String Str_Aux = "";
                if (Convert.IsDBNull(Proveedor["FECHA_DE_REGISTRO"]))
                {
                    Str_Aux = "**/***/****";
                }
                else
                {
                    Str_Aux = String.Format("{0:dd/MMM/yyyy}", Proveedor["FECHA_DE_REGISTRO"]);
                }
                Celda = Renglon1_Datos_Proveedor.Cells.Add("Prov. " + Convert.ToInt32(Proveedor["PROVEEDOR_ID"].ToString().Trim()) + " - Reg. " + Str_Aux);
                Celda.StyleID = "Encabezado";
                Celda.MergeAcross = 2;

                if (Proveedor["COMPANIA"].ToString().Contains(Proveedor["NOMBRE"].ToString().Trim()))
                {
                    Str_Aux = Proveedor["NOMBRE"].ToString().Trim();
                }
                else
                {
                    Str_Aux = Proveedor["NOMBRE"].ToString().Trim() + " / " + Proveedor["COMPANIA"].ToString().Trim();
                }
                Celda = Renglon2_Datos_Proveedor.Cells.Add(Str_Aux);
                Celda.StyleID = "Nombre_Proveedor";
                Celda.MergeAcross = 2;
                Celda.Row.Height = 30;
                //Celda.Row.AutoFitHeight = true;


                //Consultamos los productos cotizados del proveedor seleccionado
                Dt_Productos_Cotizados = Negocio.Consultar_Precios_Cotizados();
                //A cada uno de los productos le agrego el cotizado del proveedor

                Renglon_Encabezado_De_Informacion.Cells.Add(new WorksheetCell("MARCA", "Encabezado"));
                Renglon_Encabezado_De_Informacion.Cells.Add(new WorksheetCell("P.UNITARIO", "Encabezado"));
                Renglon_Encabezado_De_Informacion.Cells.Add(new WorksheetCell("IMPORTE", "Encabezado"));
                //row.Cells.Add(new WorksheetCell(i.ToString(), DataType.Number));

                DataRow[] Producto_Cotizado = null;
                Contador_Lineas = 13;
                String Estilo_Marca = "";
                String Estilo_Numeros = "";
                bool Proveedor_Ganador = false;
                foreach (DataRow Producto in Dt_Productos.Rows)
                {
                    R++;
                    Producto_Cotizado = Dt_Productos_Cotizados.Select("PROD_SERV_ID = '" +
                        Producto["PROD_SERV_ID"].ToString().Trim() + "'");
                    //marca que ofrece el proveedor

                    if (Producto_Cotizado[0]["RESULTADO"].ToString().Trim() == "ACEPTADA")
                    {
                        Estilo_Marca = "Detalles_Ganador";
                        Estilo_Numeros = "Numero_Ganador";
                        Proveedor_Ganador = true;
                    }
                    else
                    {
                        Estilo_Marca = "Detalles";
                        Estilo_Numeros = "Numero";
                    }
                    Renglon_Informacion_Producto = sheet.Table.Rows[Contador_Lineas];
                    Renglon_Informacion_Producto.Cells.Add(new WorksheetCell(Producto_Cotizado[0]["MARCA"].ToString().Trim(), Estilo_Marca));
                    Renglon_Informacion_Producto.Cells.Add
                        (new WorksheetCell(Producto_Cotizado[0]["PRECIO_U_SIN_IMP_COTIZADO"].ToString().Trim(), Estilo_Numeros));
                    Renglon_Informacion_Producto.Cells.Add
                        (new WorksheetCell(Producto_Cotizado[0]["SUBTOTAL_COTIZADO"].ToString().Trim(), Estilo_Numeros));

                    Contador_Lineas++;
                }
                if (Proveedor_Ganador)
                {
                    Nombres_Proveedores_Adjudicados += ", " + Proveedor["NOMBRE"].ToString().Trim();
                    No_Proveedores_Adjudicados++;
                }
                //Subtotal
                try
                {
                    row = sheet.Table.Rows[Contador_Lineas];
                    Celda_Combinada(row, "Numero", 2, Producto_Cotizado[0]["SUBTOTAL_COTIZADO_REQUISICION"].ToString());

                    Contador_Lineas++;
                    row = sheet.Table.Rows[Contador_Lineas];
                    Celda_Combinada(row, "Numero", 2, Producto_Cotizado[0]["IVA_COTIZADO_REQ"].ToString());

                    Contador_Lineas++;
                    row = sheet.Table.Rows[Contador_Lineas];
                    Celda_Combinada(row, "Numero", 2, Producto_Cotizado[0]["TOTAL_COTIZADO_REQUISICION"].ToString());

                    Contador_Lineas++;
                    row = sheet.Table.Rows[Contador_Lineas];
                    Celda_Combinada(row, "Detalles", 2, Producto_Cotizado[0]["TIEMPO_ENTREGA"].ToString() + " DÍAS HÁBILES");

                    Contador_Lineas++;
                    row = sheet.Table.Rows[Contador_Lineas]; //Negocio.Consultar_Dias_Credito_De_Proveedor(Proveedor["PROVEEDOR_ID"].ToString().Trim())
                    Celda_Combinada(row, "Detalles", 2, "8 a 10 DÍAS HÁBILES");

                    Contador_Lineas++;
                    row = sheet.Table.Rows[Contador_Lineas];
                    Celda_Combinada(row, "Detalles", 2, Producto_Cotizado[0]["GARANTIA"].ToString());

                    Contador_Lineas++;
                    row = sheet.Table.Rows[Contador_Lineas];
                    Celda_Combinada(row, "Detalles", 2, String.Format("{0:dd/MMM/yyyy}", Producto_Cotizado[0]["VIGENCIA_PROPUESTA"]));

                    Contador_Lineas++;
                    row = sheet.Table.Rows[Contador_Lineas];
                    Celda_Combinada(row, "Detalles", 2, "....");


                }
                catch (Exception Ex)
                {
                    // throw new Exception( Ex.ToString());
                    R += 5;
                }
                C += 3;
            }
            //Fundamento legal
            row = sheet.Table.Rows.Add();
            String Texto = "Fundamento Legal: Por importe se adjudica conforme al Art. 24 de la Ley de " +
                "Adquisiciones, Enajenaciones, Arrendamiento y Contratación de Servicios " +
                "del Sector Público en el Estado de Guanajuato y del Art. 37 de la Ley de " +
                "Presupuesto General de Egresos";
            if (No_Proveedores_Adjudicados > 1)
            {
                Texto += " a los proveedores " + Nombres_Proveedores_Adjudicados + " los articulos señalados en el cuadro superior.";
            }
            else if (No_Proveedores_Adjudicados == 1)
            {
                Texto += " al proveedor " + Nombres_Proveedores_Adjudicados;
            }
            Celda = row.Cells.Add(Texto);
            Celda.StyleID = "Fundamento";
            Celda.MergeAcross = 3;
            Celda.Row.Height = 60;

            row = sheet.Table.Rows.Add();
            row = sheet.Table.Rows.Add();


            //Fecha ELaboró
            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Pie_Pagina", Dt_Proveedores.Rows.Count * 3 + 2, Cls_Sessiones.Nombre_Empleado);

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Pie_Pagina", Dt_Proveedores.Rows.Count * 3 + 2, "NOMBRE Y FIRMA DE QUIEN REALIZÓ LA EVALUACIÓN");

            row = sheet.Table.Rows.Add();
            Celda_Combinada(row, "Pie_Pagina", Dt_Proveedores.Rows.Count * 3 + 2, "FECHA DE ELABORACIÓN: " + DateTime.Now.ToString("dd/MMM/yyyy"));
            //Titulo



            ////GUARDAR Y MOSTRAR REPORTE 
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
        catch (Exception theException)
        {
            String errorMessage;
            errorMessage = "Error: ";
            errorMessage = String.Concat(errorMessage, theException.Message);
            errorMessage = String.Concat(errorMessage, " Line: ");
            errorMessage = String.Concat(errorMessage, theException.Source);
        }
    }
    private void Celda_Combinada(WorksheetRow Renglon, String Estilo, int Num_Celdas, String Texto)
    {
        WorksheetCell Celda = Renglon.Cells.Add(Texto);
        if (Estilo != null)
        {
            Celda.StyleID = Estilo;
        }
        Celda.MergeAcross = Num_Celdas;
    }
    private void Rellenar_Con_Celdas(WorksheetRow Renglon, int Num_Celdas, String Estilo, String Texto)
    {
        if (Texto == null)
            Texto = "";
        for (int i = 0; i < Num_Celdas; i++)
        {
            if (Estilo == null)
            {
                Renglon.Cells.Add(new WorksheetCell(Texto));
            }
            else
            {
                Renglon.Cells.Add(new WorksheetCell(Texto, Estilo));
            }
        }
    }
    private void Crear_Cuadro_Comparativo_Aspectos_Tecnicos(String No_Requisicion)
    {
        Cls_Ope_Com_Cuadro_Comparativo_Negocio Negocio = new Cls_Ope_Com_Cuadro_Comparativo_Negocio();
        Negocio.P_No_Requisicion = No_Requisicion;
        DataTable Dt_Requisicion = (DataTable)Session[P_Dt_Requisiciones];
        DataTable Dt_Productos = Negocio.Consultar_Productos_Requisicion();
        DataTable Dt_Proveedores = Negocio.Consultar_Proveedores_Que_Cotizaron();

        DataRow[] Rows = null;
        String Nombre_Unidad_Responsable = null;
        String Nombre_Fuente_Financiamiento = null;
        String Nombre_Partida = null;
        Rows = Dt_Requisicion.Select("NO_REQUISICION = '" + No_Requisicion + "'");
        if (Rows != null && Rows.Length > 0)
        {
            Nombre_Unidad_Responsable = Rows[0]["NOMBRE_DEPENDENCIA"].ToString().Trim();
        }
        else
        {
            Nombre_Unidad_Responsable = "";
        }
        //Obtengo la fuente de financiamieto
        Nombre_Fuente_Financiamiento = Dt_Productos.Rows[0]["NOMBRE_FUENTE"].ToString().Trim();
        //Obtengo el nmombre de la partida
        Nombre_Partida = Dt_Productos.Rows[0]["NOMBRE_PARTIDA"].ToString().Trim();
        //Iniciar Excel
        Excel.Application oXL;
        Excel._Workbook oWB;
        Excel._Worksheet oSheet;
        //Excel.Range oRng;
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
            oSheet.Cells[4, 1] = "UNIDAD RESPONSABLE: " + Nombre_Unidad_Responsable;
            oSheet.Cells[5, 1] = "FUENTE DE RECURSOS: " + Nombre_Fuente_Financiamiento;
            oSheet.get_Range(oSheet.Cells[1, 1], oSheet.Cells[5, 1]).Font.Bold = true;

            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).EntireColumn.ColumnWidth = 32;
            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).WrapText = true;
            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).HorizontalAlignment = Excel.XlHAlign.xlHAlignCenter;
            oSheet.get_Range(oSheet.Cells[R, 1], oSheet.Cells[R + 4, 100]).VerticalAlignment = Excel.XlVAlign.xlVAlignCenter;
            oSheet.Cells[R, 1] = "DESCRIPCIÓN";
            oSheet.Cells[R + 1, 1] = Nombre_Partida;
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
}
