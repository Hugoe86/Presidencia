using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Acciones_AC.Negocio;

public partial class paginas_Atencion_Ciudadana_Emergentes_Frm_Busqueda_Avanzada_Acciones : System.Web.UI.Page
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
            Session["BUSQUEDA_ACCIONES"] = false;
        }
        Frm_Busqueda_Avanzada_Acciones.Page.Title = "Búsqueda Avanzada de Acciones";
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
        Session["BUSQUEDA_ACCIONES"] = false;
        Session.Remove("ACCION_ID");
        Session.Remove("NOMBRE_ACCION");
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
        Session["BUSQUEDA_ACCIONES"] = true;
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
        Txt_Busqueda_Accion.Text = "";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Accion_PageIndexChanging
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Página del Grid de 
    ///PARÁMETROS          :
    ///CREO                : Roberto González Oseguera
    ///FECHA_CREO          : 17-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Accion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Buscar_Acciones(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Accion_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                : Roberto González Oseguera
    ///FECHA_CREO          : 17-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Accion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["ACCION_ID"] = Grid_Accion.SelectedRow.Cells[1].Text;
            Session["NOMBRE_ACCION"] = HttpUtility.HtmlDecode(Grid_Accion.SelectedRow.Cells[3].Text);

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Accion_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Accion_Click(object sender, EventArgs e)
    {
        Buscar_Acciones(0);
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Txt_Busqueda_Accion_TextChanged
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Busqueda_Accion_TextChanged(object sender, EventArgs e)
    {
        Buscar_Acciones(0);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Buscar_Acciones
    ///DESCRIPCIÓN: Ejecura una búsqueda de acciones en la base de datos y el resultado lo carga en el grid
    ///PARÁMETROS:
    /// 		1. Indice_Pagina: número de página en la que se va a cargar el grid
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Buscar_Acciones(int Indice_Pagina)
    {
        Cls_Cat_Ate_Acciones_Negocio Neg_Acciones = new Cls_Cat_Ate_Acciones_Negocio();
        DataTable Dt_Acciones;

        Neg_Acciones.P_Descripcion = Txt_Busqueda_Accion.Text.Trim();
        Neg_Acciones.P_Estatus = "ACTIVO";

        Dt_Acciones = Neg_Acciones.Consultar_Registros();
        Grid_Accion.Columns[1].Visible = true;
        Grid_Accion.DataSource = Dt_Acciones;
        Grid_Accion.PageIndex = Indice_Pagina;
        Grid_Accion.DataBind();
        Grid_Accion.Columns[1].Visible = false;
    }

}
