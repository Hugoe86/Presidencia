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
using Presidencia.Consolidar_Requisicion.Negocio;
using System.Collections.Generic;

public partial class paginas_Listado_Consolidaciones : System.Web.UI.Page
{
    #region ATRIBUTOS
    private Cls_Ope_Com_Consolidar_Requisicion_Negocio Negocio_Consolidar;
    private static DataTable P_Dt_Consolidaciones;
    private static DataTable P_Dt_Articulos_Consolidaciones;
    private static DataTable P_Dt_Requisiciones_Consolidacion;
    private static String P_No_Consolidacion;
    //private static String P_Requisas_Seleccionadas;
    #endregion

    #region LOAD/INIT
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            DateTime _DateTime = DateTime.Now.AddDays(-1.0);
            Txt_Fecha_Inicio.Text = _DateTime.ToString("dd/MMM/yyyy").ToUpper();
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy").ToUpper();
            Llenar_Grid_Consolidaciones();                       
        }
        Mostrar_Informacion("", false);

        //Configuracion_Acceso("Frm_Ope_Com_Listado_Consolidaciones.aspx");
    }
    #endregion

    #region EVENTOS
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Compras/Frm_Ope_Com_Consolidar_Requisicion.aspx");
    }
    
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Llenar_Grid_Consolidaciones();
    }
    #endregion

    #region EVENTOS GRID
    protected void Grid_Requisas_Consolidadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisas_Consolidadas.DataSource = P_Dt_Articulos_Consolidaciones;
        Grid_Requisas_Consolidadas.PageIndex = e.NewPageIndex;
        Grid_Requisas_Consolidadas.DataBind();
    }
    protected void Grid_Consolidaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Consolidaciones.DataSource = P_Dt_Consolidaciones;
        Grid_Consolidaciones.PageIndex = e.NewPageIndex;
        Grid_Consolidaciones.DataBind();
    }
    protected void Grid_Consolidaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridViewRow Row = Grid_Consolidaciones.SelectedRow;
        //String ID = Row.Cells[1].Text;
//        System.Windows.Forms.MessageBox.Show("Click");
        String ID = Grid_Consolidaciones.SelectedDataKey["NO_CONSOLIDACION"].ToString();
        P_No_Consolidacion = ID;
        Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_No_Consolidacion = ID;
        Negocio_Consolidar.P_Requisas_Seleccionadas = Grid_Consolidaciones.SelectedDataKey["LISTA_REQUISICIONES"].ToString();
        Negocio_Consolidar.P_Tipo_Articulo = Grid_Consolidaciones.SelectedDataKey["TIPO"].ToString().Trim();
        Div_Informacion.Visible = true;
        //CARGAR REQUISICIONES DE CONSOLIDACION SELECCIONADA
        P_Dt_Requisiciones_Consolidacion = Negocio_Consolidar.Consultar_Requisiciones_Consolidacion();
        Grid_Requisiciones.DataSource = P_Dt_Requisiciones_Consolidacion;
        Grid_Requisiciones.DataBind();
        //CARGAR PRODUCTOS
        DataTable Dt_Articulos = null;
        if (Negocio_Consolidar.P_Tipo_Articulo == "PRODUCTO")
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Productos();
        }
        else if (Negocio_Consolidar.P_Tipo_Articulo == "SERVICIO")
        {
            Dt_Articulos = Negocio_Consolidar.Consolidar_Requisiciones_Servicios();
        }
        P_Dt_Articulos_Consolidaciones = Dt_Articulos;
        Grid_Requisas_Consolidadas.DataSource = Dt_Articulos;
        Grid_Requisas_Consolidadas.DataBind();
        //NUMERO DE CONSOLIDACION Y MONTO
        Txt_Num_Consolidacion.Text = Grid_Consolidaciones.SelectedDataKey["FOLIO"].ToString();
        Txt_Total.Text = "$ " + Grid_Consolidaciones.SelectedDataKey["MONTO"].ToString();


    }
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
    //NOMBRE DE LA FUNCIÓN: Llenar_Combos_Generales
    //DESCRIPCIÓN: 
    //PARAMETROS: 
    //CREO: Gustavo Angeles Cruz
    //FECHA_CREO: Diciembre/2010
    //MODIFICO:
    //FECHA_MODIFICO:
    //CAUSA_MODIFICACIÓN:
    //********************************************************************************/
    public void Llenar_Combos_Generales()
    {
        //Cmb_Consolidar.Items.Add("SIN CONSOLIDAR");
        //Cmb_Consolidar.Items.Add("CONSOLIDADAS");
        //Cmb_Consolidar.Items.Add("AMBAS");
        //Cmb_Consolidar.Items[0].Value = "0";
        //Cmb_Consolidar.Items[0].Selected = true;
    }
    public void Llenar_Grid_Consolidaciones()
    {
        Negocio_Consolidar = new Cls_Ope_Com_Consolidar_Requisicion_Negocio();
        Negocio_Consolidar.P_Fecha_Inicial = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()));
        Negocio_Consolidar.P_Fecha_Final = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Final.Text.Trim()));
        Negocio_Consolidar.P_Estatus_Consolidacion = "GENERADA";
        Negocio_Consolidar.P_Folio = Txt_Consolidacion_Busqueda.Text.Trim();
        DataTable _DataTable = Negocio_Consolidar.Consultar_Consolidaciones();
        if (_DataTable != null)
        {
            P_Dt_Consolidaciones = _DataTable;
            Grid_Consolidaciones.DataSource = _DataTable;
            Grid_Consolidaciones.DataBind();
        }
        else
        {
            P_Dt_Consolidaciones = null;
            Grid_Consolidaciones.DataSource = null;
            Grid_Consolidaciones.DataBind();
            Mostrar_Informacion("No existen datos",true);
        }
    }

    #endregion

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        int No_Renglon;
        try
        {
            No_Renglon = Grid_Consolidaciones.SelectedIndex;
        }
        catch(Exception ex)
        {
            String Str = ex.ToString();
            No_Renglon = -1;
        }
        if (No_Renglon > -1)
        {
            Response.Redirect("../Compras/Frm_Ope_Com_Consolidar_Requisicion.aspx?" + P_No_Consolidacion);
        }
        else 
        {
            Mostrar_Informacion("Debe seleccionar una Consolidación para modificar!!",true);
        }        
    }
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.DataSource = P_Dt_Requisiciones_Consolidacion;
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataBind();
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
            Botones.Add(Btn_Modificar);
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
}
