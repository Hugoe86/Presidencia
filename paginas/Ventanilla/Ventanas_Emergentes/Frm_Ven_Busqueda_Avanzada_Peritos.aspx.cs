using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;

public partial class paginas_Emergentes_Ventanilla_Frm_Ven_Busqueda_Avanzada_Peritos : System.Web.UI.Page
{
    String Boton_Busqueda_Pulsado = "";
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Método que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 17-may-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_PERITOS"] = false;
        }
        Frm_Busqueda_Avanzada_Peritos.Page.Title = "Búsqueda Avanzada de Peritos";
        if (Session["Boton_Busqueda_Pulsado"] != null)
        {
            Boton_Busqueda_Pulsado = Session["Boton_Busqueda_Pulsado"].ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : NO APLICA
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 17-may-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_PERITOS"] = false;
        Session.Remove("PERITO_ID");
        Session.Remove("NOMBRE_PERITO");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 17-may-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_PERITOS"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Limpiar
    ///PARAMETROS           : sender y e
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 17-may-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Busqueda_Perito.Text = "";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Perito_PageIndexChanging
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Página del Grid de 
    ///PARÁMETROS          :
    ///CREO                : Roberto González Oseguera
    ///FECHA_CREO          : 17-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Perito_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Buscar_Peritos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Perito_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                : Roberto González Oseguera
    ///FECHA_CREO          : 17-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Perito_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["PERITO_ID"] = Grid_Perito.SelectedRow.Cells[1].Text;
            Session["PERITO_PERITO"] = HttpUtility.HtmlDecode(Grid_Perito.SelectedRow.Cells[3].Text);

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Perito_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Perito_Click(object sender, EventArgs e)
    {
        Buscar_Peritos(0);
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : 
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Busqueda_Perito_TextChanged(object sender, EventArgs e)
    {
        Buscar_Peritos(0);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Buscar_Peritos
    ///DESCRIPCIÓN: Ejecuta una búsqueda de inspectores en la base de datos y el resultado lo carga en el grid
    ///PARÁMETROS:
    /// 		1. Indice_Pagina: número de página en la que se va a cargar el grid
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Buscar_Peritos(int Indice_Pagina)
    {
        Cls_Cat_Ort_Inspectores_Negocio Neg_Peritos = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Peritos;

        Neg_Peritos.P_Nombre = Txt_Busqueda_Perito.Text.Trim();

        Dt_Peritos = Neg_Peritos.Consultar_Inspectores();
        Grid_Perito.Columns[1].Visible = true;
        Grid_Perito.DataSource = Dt_Peritos;
        Grid_Perito.PageIndex = Indice_Pagina;
        Grid_Perito.DataBind();
        Grid_Perito.Columns[1].Visible = false;
    }

}
