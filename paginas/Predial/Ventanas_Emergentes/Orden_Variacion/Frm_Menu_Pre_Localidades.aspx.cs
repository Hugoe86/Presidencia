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
using Presidencia.Localidades.Negocios;
using Presidencia.Constantes;

public partial class paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Localidades : System.Web.UI.Page
{
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
            if (!Cargar_Localidades(0))
            {
                Lbl_Ecabezado_Mensaje.Text = "No hay datos qué mostrar";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Session["BUSQUEDA_LOCALIDAD"] = false;
        }
        Frm_Menu_Pre_Localidades.Page.Title = "Ubicaciones";
        Lbl_Title.Text = "Selecione una Ubicación por favor";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Localidades
    ///DESCRIPCIÓN          : Método que carga los datos de las Localidades existentes en el catálogo de Cat_Ate_Localidades
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private Boolean Cargar_Localidades(int Page_Index)
    {
        Boolean Colonias_Cargadas = false;
        try
        {
            Cls_Cat_Ate_Localidades_Negocio Localidades = new Cls_Cat_Ate_Localidades_Negocio();

            Localidades.P_Filtros_Dinamicos = "";
            Grid_Colonias.DataSource = Localidades.Consultar_Localidades();
            Grid_Colonias.PageIndex = Page_Index;
            Grid_Colonias.DataBind();
            Colonias_Cargadas = true;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Colonias_Cargadas;
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
        Session["BUSQUEDA_LOCALIDAD"] = false;
        Session.Remove("LOCALIDAD_ID");
        Session.Remove("NOMBRE");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_LOCALIDAD"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Colonias_PageIndexChanging
    ///DESCRIPCIÓN          : Evento PageIndexChanging del control Grid_Colonias
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Localidades_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Localidades(e.NewPageIndex);
        Grid_Colonias.SelectedIndex = (-1);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Colonias_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control Grid_Colonias
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Localidades_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["LOCALIDAD_ID"] = Grid_Colonias.Rows[Grid_Colonias.SelectedIndex].Cells[1].Text;
        Session["NOMBRE"] = Grid_Colonias.Rows[Grid_Colonias.SelectedIndex].Cells[2].Text;
    }
}
