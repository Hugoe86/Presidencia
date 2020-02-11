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
using Presidencia.Catalogo_Casos_Especiales.Negocio;
using Presidencia.Constantes;

public partial class paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Casos_Especiales : System.Web.UI.Page
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
            if (!Cargar_Casos_Especiales(0))
            {
                Lbl_Ecabezado_Mensaje.Text = "No hay datos qué mostrar";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Session["BUSQUEDA_CASO_ESPECIAL"] = false;
        }
        Frm_Menu_Pre_Casos_Especiales.Page.Title = "Casos Especiales";
        Lbl_Title.Text = "Selecione un Caso Especial por favor";
    }

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
    private Boolean Cargar_Casos_Especiales(int Page_Index)
    {
        Boolean Casos_Especiales_Cargados = false;
        try
        {
            Cls_Cat_Pre_Casos_Especiales_Negocio Casos_Especiales = new Cls_Cat_Pre_Casos_Especiales_Negocio();

            Casos_Especiales.P_Campos_Dinamicos = Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " AS CASO_ESPECIAL_ID, " + Cat_Pre_Casos_Especiales.Campo_Identificador + " AS IDENTIFICADOR, " + Cat_Pre_Casos_Especiales.Campo_Descripcion + " AS DESCRIPCION, " + Cat_Pre_Casos_Especiales.Campo_Articulo + " AS ARTICULO, " + Cat_Pre_Casos_Especiales.Campo_Inciso + " AS INCISO, " + Cat_Pre_Casos_Especiales.Campo_Aplicar_Descuento + " AS APLICA_DESCUENTO";
            Casos_Especiales.P_Filtros_Dinamicos = Cat_Pre_Casos_Especiales.Campo_Estatus + " = 'ACTIVO'";
            Grid_Casos_Especiales.DataSource = Casos_Especiales.Consultar_Casos_Especiales();
            Grid_Casos_Especiales.PageIndex = Page_Index;
            Grid_Casos_Especiales.DataBind();
            Casos_Especiales_Cargados = true;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Casos_Especiales_Cargados;
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
        Session["BUSQUEDA_CASO_ESPECIAL"] = false;
        Session.Remove("CASO_ESPECIAL_ID");
        Session.Remove("IDENTIFICADOR");
        Session.Remove("DESCRIPCION");
        Session.Remove("ARTICULO");
        Session.Remove("INCISO");
        Session.Remove("APLICA_DESCUENTO");
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
        Session["BUSQUEDA_CASO_ESPECIAL"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);

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
    protected void Grid_Casos_Especiales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Casos_Especiales(e.NewPageIndex);
        Grid_Casos_Especiales.SelectedIndex = (-1);
    }

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
    protected void Grid_Casos_Especiales_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CASO_ESPECIAL_ID"] = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[1].Text;
        Session["IDENTIFICADOR"] = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[2].Text;
        Session["DESCRIPCION"] = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[3].Text;
        Session["ARTICULO"] = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[4].Text;
        Session["INCISO"] = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[5].Text;
        Session["APLICA_DESCUENTO"] = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[6].Text;

        Lbl_Caso_Especial_ID.Text = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[1].Text;
        Lbl_Identificador.Text = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[2].Text;
        Lbl_Descripcion.Text = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[3].Text;
        Lbl_Artículo.Text = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[4].Text;
        Lbl_Inciso.Text = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[5].Text;
        Lbl_Aplica_Descuento.Text = Grid_Casos_Especiales.Rows[Grid_Casos_Especiales.SelectedIndex].Cells[6].Text;
    }
}
