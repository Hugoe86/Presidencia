using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Asuntos_AC.Negocio;

public partial class paginas_Atencion_Ciudadana_Emergentes_Frm_Busqueda_Avanzada_Asuntos : System.Web.UI.Page
{
    String Boton_Busqueda_Pulsado = "";
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
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
            Session["BUSQUEDA_ASUNTOS"] = false;
        }
        Frm_Busqueda_Avanzada_Asuntos.Page.Title = "Búsqueda Avanzada de Asuntos";
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
        Session["BUSQUEDA_ASUNTOS"] = false;
        Session.Remove("ASUNTO_ID");
        Session.Remove("NOMBRE_ASUNTO");
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
        Session["BUSQUEDA_ASUNTOS"] = true;
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
        Txt_Busqueda_Asunto.Text = "";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Asuntos_PageIndexChanging
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Página del Grid de 
    ///PARÁMETROS          :
    ///CREO                : Roberto González Oseguera
    ///FECHA_CREO          : 17-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Asuntos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Buscar_Asuntos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Asuntos_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                : Roberto González Oseguera
    ///FECHA_CREO          : 17-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Asuntos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ASUNTO_ID"] = Grid_Asuntos.SelectedRow.Cells[1].Text;
            Session["NOMBRE_ASUNTO"] = HttpUtility.HtmlDecode(Grid_Asuntos.SelectedRow.Cells[3].Text);

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Asuntos_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Asuntos_Click(object sender, EventArgs e)
    {
        Buscar_Asuntos(0);
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Txt_Busqueda_Asunto_TextChanged
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Busqueda_Asunto_TextChanged(object sender, EventArgs e)
    {
        Buscar_Asuntos(0);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Buscar_Asuntos
    ///DESCRIPCIÓN: Ejecura una búsqueda de asuntos en la base de datos y el resultado lo carga en el grid
    ///PARÁMETROS:
    /// 		1. Indice_Pagina: número de página en la que se va a cargar el grid
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Buscar_Asuntos(int Indice_Pagina)
    {
        Cls_Cat_Ate_Asuntos_Negocio Neg_Asuntos = new Cls_Cat_Ate_Asuntos_Negocio();
        DataTable Dt_Asuntos;

        Neg_Asuntos.P_Nombre = Txt_Busqueda_Asunto.Text.Trim();
        Neg_Asuntos.P_Descripcion = Txt_Busqueda_Asunto.Text.Trim();
        Neg_Asuntos.P_Clave = Txt_Busqueda_Asunto.Text.Trim();
        Neg_Asuntos.P_Estatus = "ACTIVO";

        Dt_Asuntos = Neg_Asuntos.Consultar_Registros();
        Grid_Asuntos.Columns[1].Visible = true;
        Grid_Asuntos.DataSource = Dt_Asuntos;
        Grid_Asuntos.PageIndex = Indice_Pagina;
        Grid_Asuntos.DataBind();
        Grid_Asuntos.Columns[1].Visible = false;
    }

}
