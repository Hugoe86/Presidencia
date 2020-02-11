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
using Presidencia.Orden_Compra.Negocio;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Consolidar_Requisicion.Negocio;

using Presidencia.Listado_Ordenes_Compra.Negocio;
//using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Reportes;
//using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Com_Autorizar_Orden_Compra_Especiales : System.Web.UI.Page
{
    #region VARIABLES INTERNAS
    private Cls_Ope_Com_Orden_Compra_Negocio Negocio_Compra;
    private static DataTable P_Dt_Ordenes_Compra;
    private static DataTable P_Dt_Detalles_Compra;
    private const String LISTADO = "listado";
    private const String INICIAL = "inicial";
    private const String NUEVO = "nuevo";
    private const String MODIFICAR = "modificar";
    #endregion

    #region PAGE LOAD / INIT
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack) 
        {
            DateTime _DateTime = DateTime.Now;
            int dias = _DateTime.Day;
            dias = dias * -1;
            dias++;
            _DateTime = _DateTime.AddDays(dias);
            Txt_Fecha_Inicio.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Llenar_Combos_Generales();
            //Cmb_Estatus.Enabled = false;
            Llenar_Grid_Ordenes_Compra();
            Manejo_Controles(LISTADO);
        }
        Mostrar_Informacion("",false);
    }
    #endregion

    #region MÉTODOS
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
        Div_Contenedor_Msj_Error.Visible = mostrar;
        Lbl_Informacion.Visible = mostrar;
        Img_Warning.Visible = mostrar;
        Lbl_Informacion.Text = txt;
    }

    private void Manejo_Controles(String modo)
    {
        switch (modo)
        {
            case LISTADO:
                Btn_Guardar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Guardar.ToolTip = "Guardar";
                //Configuracion_Acceso("Frm_Ope_Com_Autorizar_Orden_Compra_Especiales.aspx");
                Btn_Guardar.Visible = false;

                Btn_Imprimir_Orden_Compra.Visible = false;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.Visible = true;

                Div_Ordenes_Compra.Visible = true;
                Div_Filtros.Visible = true;
                Div_Articulos.Visible = false;
                break;
            case INICIAL:
                Btn_Guardar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Guardar.ToolTip = "Modificar";
                Btn_Guardar.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Salir.ToolTip = "Regresar";
                Btn_Salir.Visible = true;
                //Txt_Comentarios.Enabled = false;
                Txt_Comentarios.ReadOnly = true;
                Txt_No_Reserva.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Div_Ordenes_Compra.Visible = false;
                Div_Filtros.Visible = false;
                Div_Articulos.Visible = true;
                Btn_Imprimir_Orden_Compra.Visible = true;
                //Configuracion_Acceso("Frm_Ope_Com_Autorizar_Orden_Compra_Especiales.aspx");
                break;
            case NUEVO:
                break;
            case MODIFICAR:
                Btn_Guardar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Guardar.ToolTip = "Guardar";
                Btn_Guardar.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.Visible = true;
                Btn_Imprimir_Orden_Compra.Visible = false;
                //Txt_Comentarios.Enabled = true;
                Txt_Comentarios.ReadOnly = true;
                Txt_No_Reserva.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Div_Ordenes_Compra.Visible = false;
                Div_Filtros.Visible = false;
                Div_Articulos.Visible = true;
                break;
        }
    }

    private void Llenar_Grid_Ordenes_Compra() 
    {
        Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        Negocio_Compra.P_Fecha_Inicial = Txt_Fecha_Inicio.Text.Trim();
        Negocio_Compra.P_Fecha_Final = Txt_Fecha_Final.Text.Trim();
        Negocio_Compra.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue.Trim();
        Negocio_Compra.P_Folio = Txt_Orden_Compra_Busqueda.Text.Trim();
        P_Dt_Ordenes_Compra = Negocio_Compra.Consultar_Ordenes_Compra_Especiales();
        if (P_Dt_Ordenes_Compra != null && P_Dt_Ordenes_Compra.Rows.Count > 0)
        {
            Grid_Ordenes_Compra.DataSource = P_Dt_Ordenes_Compra;
            Grid_Ordenes_Compra.DataBind();

        }
        else 
        {
            P_Dt_Ordenes_Compra = null;
            Grid_Ordenes_Compra.DataSource = P_Dt_Ordenes_Compra;
            Grid_Ordenes_Compra.DataBind();
        }
    }

    #endregion


    protected void Grid_Ordenes_Compra_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Ordenes_Compra.DataSource = P_Dt_Ordenes_Compra;
        Grid_Ordenes_Compra.PageIndex = e.NewPageIndex;
        Grid_Ordenes_Compra.DataBind();
    }

    protected void Grid_Detalles_Compra_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Detalles_Compra.DataSource = P_Dt_Detalles_Compra;
        Grid_Detalles_Compra.PageIndex = e.NewPageIndex;
        Grid_Detalles_Compra.DataBind();
    }
    protected void Grid_Ordenes_Compra_SelectedIndexChanged(object sender, EventArgs e)
    {       
        String No_Orden_Compra = Grid_Ordenes_Compra.SelectedDataKey["NO_ORDEN_COMPRA"].ToString().Trim();
        String Lista_Requisiciones = Grid_Ordenes_Compra.SelectedDataKey["LISTA_REQUISICIONES"].ToString().Trim();
        String Tipo_Articulo = Grid_Ordenes_Compra.SelectedDataKey["TIPO_ARTICULO"].ToString().Trim();
        String Total = Grid_Ordenes_Compra.SelectedDataKey["TOTAL"].ToString().Trim();
        String Tipo_Proceso = Grid_Ordenes_Compra.SelectedDataKey["TIPO_PROCESO"].ToString().Trim();
        String Folio = Grid_Ordenes_Compra.SelectedDataKey["FOLIO"].ToString().Trim();
        String Estatus = Grid_Ordenes_Compra.SelectedDataKey["ESTATUS"].ToString().Trim();
        String No_Reserva = Grid_Ordenes_Compra.SelectedDataKey["NO_RESERVA"].ToString().Trim();
        String Justificacion = Grid_Ordenes_Compra.SelectedDataKey["JUSTIFICACION_COMPRA"].ToString().Trim();
        String Codigo_Programatico = Grid_Ordenes_Compra.SelectedDataKey["CODIGO"].ToString().Trim();
        P_Dt_Detalles_Compra = Consolidar(Lista_Requisiciones, Tipo_Articulo);
        Grid_Detalles_Compra.DataSource = P_Dt_Detalles_Compra;
        Grid_Detalles_Compra.DataBind();
        double Importe_Total = Convert.ToDouble(Total);
        Txt_Total.Text = String.Format("{0:C}",Importe_Total);
        Txt_Proceso_Compra.Text = Tipo_Proceso;
        Txt_Identificador_Compra.Text = Folio;
        Txt_Listado_Requisiciones.Text = "RQ-" + Lista_Requisiciones;
        Txt_No_Reserva.Text = No_Reserva;
        Cmb_Estatus.SelectedValue = Estatus;
        Txt_Comentarios.Text = Justificacion;
        Txt_Codigo_Programatico.Text = Codigo_Programatico;        
        Manejo_Controles(INICIAL);
    }

    private DataTable Consolidar(String Requisas_Seleccionadas, String Tipo_Articulo)
    {
        Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio_Consolidar =
            new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_Requisas_Seleccionadas = Requisas_Seleccionadas;
        Negocio_Consolidar.P_Estatus = "CONFIRMADA";
        DataTable Dt_Articulos = null;
        //P_Dt_Detalle_de_Compra = null;
        if (Tipo_Articulo == "PRODUCTO")
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Productos();
        }
        else if (Tipo_Articulo == "SERVICIO")
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Servicios();
        }
        return Dt_Articulos;
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
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("GENERADA");
        Cmb_Estatus.Items.Add("AUTORIZADA");
        Cmb_Estatus.Items.Add("RECHAZADA");
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Estatus_Busqueda.Items.Clear();
        Cmb_Estatus_Busqueda.Items.Add("GENERADA");
        Cmb_Estatus_Busqueda.Items.Add("AUTORIZADA");
        //Cmb_Estatus_Busqueda.Items.Add("CANCELADA");
        Cmb_Estatus_Busqueda.SelectedIndex = 0;
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Ordenes_Compra();
    }
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Guardar.ToolTip == "Modificar") 
        {
            Manejo_Controles(MODIFICAR);
        }
        else
        if (Btn_Guardar.ToolTip == "Guardar")
        {
            if (Cmb_Estatus.SelectedValue.Trim() == "RECHAZADA" && Txt_Motivo_Rechazo.Text.Trim().Length <= 0)
            {
                Mostrar_Informacion("Debe ingresar el motivo por el cual rechaza la orden de compra", true);
            }
            else if (Cmb_Estatus.SelectedValue.Trim() == "AUTORIZADA" && Txt_No_Reserva.Text.Trim().Length <= 0)
            {
                Mostrar_Informacion("Debe ingresar el número de reserva", true);
            }
            else
            {
                Actualizar_Orden_Compra();
                String Mensaje = "Los cambios fueron guardados";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
                Llenar_Grid_Ordenes_Compra();
                Manejo_Controles(LISTADO);
            }
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip == "Inicio")
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        if (Btn_Salir.ToolTip == "Regresar")
        {
            Manejo_Controles(LISTADO);
            Llenar_Grid_Ordenes_Compra();
        }
        if (Btn_Salir.ToolTip == "Cancelar")
        {
            Manejo_Controles(INICIAL);
        }
        Txt_Motivo_Rechazo.Visible = false;
        Lbl_Motivo_Rechazo.Visible = false;
    }
    private void Actualizar_Orden_Compra()
    {
        Negocio_Compra = new Cls_Ope_Com_Orden_Compra_Negocio();
        String Str_Num = Grid_Ordenes_Compra.SelectedDataKey["NO_ORDEN_COMPRA"].ToString().Trim();
        long No_Orden_Compra = long.Parse(Str_Num);
        Negocio_Compra.P_No_Orden_Compra = No_Orden_Compra;
        Negocio_Compra.P_Estatus = Cmb_Estatus.SelectedValue.Trim();
        Negocio_Compra.P_Comentarios = Txt_Motivo_Rechazo.Text.Trim();// Txt_Comentarios.Text.Trim();
        Negocio_Compra.P_No_Reserva = Txt_No_Reserva.Text.Trim();
        int Renglones_Afectados = Negocio_Compra.Actualizar_Orden_Compra();
        String Mensaje = "";
        if (Renglones_Afectados > 0)
        {
            Mensaje = "La Orden de Compra fue actualizada";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
        }
        else 
        {
            Mensaje = "No se actualizaron registros";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Información", "alert('" + Mensaje + "');", true);
        }
    }

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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Orden_Compra_Click
    ///DESCRIPCIÓN:          Evento utilizado para instanciar los métodos que consultan la 
    ///                      orden de compra seleccionada por el usuario y los productos de esta misma.
    ///PARAMETROS:          
    ///CREO:                 Gustavo AC
    ///FECHA_CREO:           
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Orden_Compra_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Listado_Negocio = new Cls_Ope_Com_Listado_Ordenes_Compra_Negocio();
        String No_Orden_Compra = Txt_Identificador_Compra.Text.Trim().Replace("OC-","");
        DataTable Dt_Cabecera_OC = new DataTable();
        DataTable Dt_Detalles_OC = new DataTable();
        //ImageButton Btn_Imprimir_Orden_Compra = null;
        String Formato = "PDF";
        try
        {
        //    Btn_Imprimir_Orden_Compra = (ImageButton)sender;
        //    No_Orden_Compra = Btn_Imprimir_Orden_Compra.CommandArgument;
            Listado_Negocio.P_No_Orden_Compra = No_Orden_Compra.Trim();
            // Consultar Cabecera de la Orden de compra
            Dt_Cabecera_OC = Listado_Negocio.Consulta_Cabecera_Orden_Compra();
            // Consultar los detalles de la Orden de compra
            Dt_Detalles_OC = Listado_Negocio.Consulta_Detalles_Orden_Compra();
            // Instanciar el DataSet Fisico
            if (Convert.ToInt32(Dt_Cabecera_OC.Rows[0]["TOTAL"]) >= 50000)
            {
                Ds_Ope_Com_Orden_Compra_Tes Ds_Orden_Compra = new Ds_Ope_Com_Orden_Compra_Tes();
                // Instanciar al método que muestra el reporte
                Generar_Reporte(Dt_Cabecera_OC, Dt_Detalles_OC, Ds_Orden_Compra, Formato, true);
            }
            else
            {
                Ds_Ope_Com_Orden_Compra Ds_Orden_Compra = new Ds_Ope_Com_Orden_Compra();
                // Instanciar al método que muestra el reporte
                Generar_Reporte(Dt_Cabecera_OC, Dt_Detalles_OC, Ds_Orden_Compra, Formato, false);
            }
        }
        catch (Exception ex)
        {

        //    Div_Contenedor_Msj_Error.Visible = true;
        //    Lbl_Informacion.Text = (ex.Message);
        //    Div_Ordenes_Compra.Visible = false;
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
    #endregion
}
