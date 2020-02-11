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
using Presidencia.Catalogo_Multas.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Calculo_Impuestos_Frm_Menu_Pre_Tasas_Traslado_Dominio : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            if (!Cargar_Cuotas_Multas(0))
            {
                Lbl_Ecabezado_Mensaje.Text = "No hay datos qué mostrar";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Session["BUSQUEDA_CUOTA_MULTA"] = false;
        }
        Frm_Menu_Pre_Multas.Page.Title = "Multas";
        Lbl_Title.Text = "Selecione una Multa por favor";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Cuotas_Multas
    ///DESCRIPCIÓN          : Método que carga los datos de Multas existentes en el catálogo de Cat_Pre_Multas_Cuotas
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private Boolean Cargar_Cuotas_Multas(int Page_Index)
    {
        Boolean Cuotas_Cargadas;
        Cuotas_Cargadas = false;
        try
        {
            Cls_Cat_Pre_Multas_Negocio Multas = new Cls_Cat_Pre_Multas_Negocio();

            Multas.P_Incluir_Campos_Foraneos = true;
            Multas.P_Ordenar_Dinamico = "ANIO DESC";
            Grid_Multas.Columns[1].Visible = true;
            Grid_Multas.Columns[3].Visible = true;
            Grid_Multas.DataSource = Multas.Consultar_Cuotas_Multas();
            Grid_Multas.PageIndex = Page_Index;
            Grid_Multas.DataBind();
            Grid_Multas.Columns[1].Visible = false;
            Grid_Multas.Columns[3].Visible = false;
            Cuotas_Cargadas = true;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Cuotas_Cargadas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Tasas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control DataGrid Grid_Tasas
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Multas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["MULTA_ID"] = Grid_Multas.Rows[Grid_Multas.SelectedIndex].Cells[1].Text;
        Session["MULTA_CUOTA_ID"] = Grid_Multas.Rows[Grid_Multas.SelectedIndex].Cells[3].Text;
        Session["MONTO"] = Grid_Multas.Rows[Grid_Multas.SelectedIndex].Cells[4].Text;
        Btn_Aceptar_Click(Btn_Aceptar, null);
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
        Session["BUSQUEDA_CUOTA_MULTA"] = false;
        Session.Remove("MULTA_ID");
        Session.Remove("MULTA_CUOTA_ID");
        Session.Remove("MONTO");
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
        Session["BUSQUEDA_CUOTA_MULTA"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);

    }
    protected void Grid_Multas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Cuotas_Multas(e.NewPageIndex);
        Grid_Multas.SelectedIndex = (-1);
    }
}
