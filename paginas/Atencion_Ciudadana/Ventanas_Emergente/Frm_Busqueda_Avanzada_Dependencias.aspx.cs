using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Consulta_Peticiones.Negocios;

public partial class paginas_Atencion_Ciudadana_Emergentes_Frm_Busqueda_Avanzada_Dependencias : System.Web.UI.Page
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
            Session["BUSQUEDA_DEPENDENCIAS"] = false;
        }
        Frm_Busqueda_Avanzada_Dependencias.Page.Title = "Búsqueda Avanzada de Unidad responsable";
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
        Session["BUSQUEDA_DEPENDENCIAS"] = false;
        Session.Remove("DEPENDENCIA_ID");
        Session.Remove("NOMBRE_DEPENDENCIA");
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
        Session["BUSQUEDA_DEPENDENCIAS"] = true;
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
        Txt_Busqueda_Dependencia.Text = "";
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                : Roberto González Oseguera
    ///FECHA_CREO          : 17-may-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["DEPENDENCIA_ID"] = Grid_Dependencias.SelectedRow.Cells[1].Text;
            Session["NOMBRE_DEPENDENCIA"] = HttpUtility.HtmlDecode(Grid_Dependencias.SelectedRow.Cells[3].Text);

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Dependencias_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Dependencias_Click(object sender, EventArgs e)
    {
        Buscar_Dependencias();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Txt_Busqueda_Dependencia_TextChanged
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    /// CREO                    : Roberto González Oseguera
    /// FECHA_CREO              : 17-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Busqueda_Dependencia_TextChanged(object sender, EventArgs e)
    {
        Buscar_Dependencias();
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Buscar_Dependencias
    ///DESCRIPCIÓN: Ejecura una búsqueda en Cat_Dependencias y el resultado lo carga en el grid
    ///PARÁMETROS:
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Buscar_Dependencias()
    {
        Cls_Ope_Consulta_Peticiones_Negocio Neg_Dependencias = new Cls_Ope_Consulta_Peticiones_Negocio();
        DataTable Dt_Dependencias;

        Neg_Dependencias.P_Comentarios = Txt_Busqueda_Dependencia.Text.Trim();
        Neg_Dependencias.P_Clave = Txt_Busqueda_Dependencia.Text.Trim();
        Neg_Dependencias.P_Nombre = Txt_Busqueda_Dependencia.Text.Trim();
        Neg_Dependencias.P_Estatus = "ACTIVO";

        Dt_Dependencias = Neg_Dependencias.Consultar_Dependencias();
        Grid_Dependencias.Columns[1].Visible = true;
        Grid_Dependencias.DataSource = Dt_Dependencias;
        Grid_Dependencias.DataBind();
        Grid_Dependencias.Columns[1].Visible = false;
    }

}
