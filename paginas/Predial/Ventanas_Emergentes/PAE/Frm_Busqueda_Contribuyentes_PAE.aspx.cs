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
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Predial_Ventanas_Emergentes_PAE_Frm_Busqueda_Contribuyentes_PAE : System.Web.UI.Page
{
    #region
    public static String M_Paterno;
    public static String M_Materno;
    public static String M_Nombre;
    public static DateTime M_Fecha;
    public static String M_RFC;
    #endregion
    #region Page_Load
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_CONTRIBUYENTE"] = false;            
            Buscar.Visible = true;
        }
        Frm_Menu_Pre_Propietarios.Page.Title = "Propietarios";
        Lbl_Title.Text = "Selecione un Propietario por favor";
    }
    #endregion
    #region Metodos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Contribuyentes
    ///DESCRIPCIÓN          : Método que carga los datos de los Propietarios existentes en el catálogo de Cat_Pre_Contribuyentes
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private Boolean Cargar_Contribuyentes(int Page_Index)
    {
        Boolean Contribuyentes_Cargados = false;
        DataTable Dt_Contributentes;
        try
        {
            Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();

            Contribuyentes.P_RFC = M_RFC;
            Contribuyentes.P_Nombre = M_Nombre;
            Dt_Contributentes = Contribuyentes.Consultar_Menu_Contribuyentes();
            if (Dt_Contributentes.Rows.Count > 0)
            {
                Grid_Contribuyentes.DataSource = Dt_Contributentes;
                Grid_Contribuyentes.PageIndex = Page_Index;
                Grid_Contribuyentes.DataBind();
            }
            else
            {
                Grid_Contribuyentes.DataSource = null;
                Grid_Contribuyentes.PageIndex = Page_Index;
                Grid_Contribuyentes.DataBind();
                
                Buscar.Visible = false;
                Btn_Regresar.AlternateText = "Cancelar";
            }
            Contribuyentes_Cargados = true;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Contribuyentes_Cargados;
    }
    #endregion
    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Busqueda_Click(object sender, ImageClickEventArgs e)
    {
        String Validar_Campos = Txt_Rfc.Text.Trim() + Txt_Nombre.Text.Trim();


        if (!String.IsNullOrEmpty(Validar_Campos))
        {
            M_RFC = Txt_Rfc.Text.Trim();
            M_Nombre = Txt_Nombre.Text.Trim();

            if (!Cargar_Contribuyentes(0))
            {
                Grid_Contribuyentes.DataSource = null;
                Grid_Contribuyentes.DataBind();
                Lbl_Ecabezado_Mensaje.Text = "No hay datos qué mostrar";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;

            }
        }
    }
        ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_CONTRIBUYENTE"] = false;
        Session.Remove("CONTRIBUYENTE_ID");
        Session.Remove("CONTRIBUYENTE_NOMBRE");
        Session.Remove("RFC");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Nombre_TextChanged
    ///DESCRIPCIÓN          : buscar datos
    ///PARAMETROS           : sender y e
    ///CREO                 : Toledo Rodriguez Jesus
    ///FECHA_CREO           : 18/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Txt_Nombre_TextChanged(object sender, EventArgs e)
    {
        ImageClickEventArgs e1 = null;
        Btn_Busqueda_Click(null, e1);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Rfc_TextChanged
    ///DESCRIPCIÓN          : buscar datos
    ///PARAMETROS           : sender y e
    ///CREO                 : Toledo Rodriguez Jesus
    ///FECHA_CREO           : 18/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Txt_Rfc_TextChanged(object sender, EventArgs e)
    {
        ImageClickEventArgs e1 = null;
        Btn_Busqueda_Click(null, e1);
    }
    #endregion
    #region Grids
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Contribuyentes_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control Grid_Contribuyentes
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Contribuyentes_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CONTRIBUYENTE_ID"] = Grid_Contribuyentes.Rows[Grid_Contribuyentes.SelectedIndex].Cells[1].Text;
        Session["CONTRIBUYENTE_NOMBRE"] = Grid_Contribuyentes.Rows[Grid_Contribuyentes.SelectedIndex].Cells[2].Text;
        Session["RFC"] = Grid_Contribuyentes.Rows[Grid_Contribuyentes.SelectedIndex].Cells[3].Text;

        Session["BUSQUEDA_CONTRIBUYENTE"] = true;
        //Cierra la ventana
        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cerrar", "window.close();", true);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Contribuyentes_PageIndexChanging
    ///DESCRIPCIÓN          : Evento PageIndexChanging del control Grid_Contribuyentes
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Contribuyentes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Contribuyentes(e.NewPageIndex);
        Grid_Contribuyentes.SelectedIndex = (-1);
    }
    #endregion
}
