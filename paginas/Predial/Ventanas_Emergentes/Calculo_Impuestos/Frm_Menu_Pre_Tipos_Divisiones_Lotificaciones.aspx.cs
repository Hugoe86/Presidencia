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
using Presidencia.Sessiones;
using Presidencia.Catalogo_Divisiones.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Calculo_Impuestos_Frm_Menu_Pre_Tipos_Divisiones_Lotificaciones : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
            Response.Redirect("../../../Paginas_Generales/Frm_Apl_Login.aspx");
        if (!IsPostBack)
        {
            Cargar_Tasas_Divisiones_Lotificaciones(0);
            Session["BUSQUEDA_TASA_DIVISION_LOTIFICACION"] = false;
        }
        Frm_Menu_Pre_Tipos_Divisiones_Lotificaciones.Page.Title = "Tipos Divisiones y Lotificaciones";
        Lbl_Title.Text = "Selecione un Tipo de División o Lotificación por favor";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Tasas_Traslado
    ///DESCRIPCIÓN          : Metodo que carga los datos de Tasas existentes en el catálogo de Cat_Pre_Impuestos_Traslado
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Tasas_Divisiones_Lotificaciones(int Page_Index)
    {
        try
        {
            Cls_Cat_Pre_Divisiones_Negocio Divisiones_Lotificaiones = new Cls_Cat_Pre_Divisiones_Negocio();

            Grid_Tasas.Columns[4].Visible = true;
            Grid_Tasas.DataSource = Divisiones_Lotificaiones.Consultar_Tasas_Divisines_Lotificaciones();
            Grid_Tasas.PageIndex = Page_Index;
            Grid_Tasas.DataBind();
            Grid_Tasas.Columns[4].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Tasas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control DataGrid Grid_Tasas
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Tasas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["ANIO"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[1].Text;
        Session["CONCEPTO"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[2].Text;
        Session["IMPUESTO_DIVISION_LOT_ID"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[4].Text;
        Session["TASA"] = Grid_Tasas.Rows[Grid_Tasas.SelectedIndex].Cells[3].Text;

        Session["BUSQUEDA_TASA_DIVISION_LOTIFICACION"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// DESCRIPCIÓN: Manejar el clic en el boton de busqueda
    ///             buscar impuestos por el año proporcionado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 08-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        int Anio;
        try
        {
            Cls_Cat_Pre_Divisiones_Negocio Divisiones_Lotificaciones = new Cls_Cat_Pre_Divisiones_Negocio();

            if (int.TryParse(Txt_Busqueda.Text, out Anio) && Anio > 0)
            {
                Divisiones_Lotificaciones.P_Anio = Anio;
            }
            Txt_Busqueda.Text = "";

            Grid_Tasas.Columns[4].Visible = true;
            Grid_Tasas.DataSource = Divisiones_Lotificaciones.Consultar_Tasas_Divisines_Lotificaciones();
            Grid_Tasas.DataBind();
            Grid_Tasas.Columns[4].Visible = false;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_TASA_DIVISION_LOTIFICACION"] = false;
        Session.Remove("ANIO");
        Session.Remove("CONCEPTO");
        Session.Remove("IMPUESTO_DIVISION_LOT_ID");
        Session.Remove("TASA");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    protected void Grid_Tasas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Tasas_Divisiones_Lotificaciones(e.NewPageIndex);
        Grid_Tasas.SelectedIndex = (-1);
    }
}
