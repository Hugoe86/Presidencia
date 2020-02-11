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
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Desglose_Adeudos : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18/sep-2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
            Response.Redirect("../../../Paginas_Generales/Frm_Apl_Login.aspx");
        if (!IsPostBack)
        {
            Cargar_Adeudos(0);
        }
        if (Request.QueryString["Cuenta_Predial"].ToString() != "")
        {
            Lbl_Title.Text = "Adeudos actuales de la cuenta predial: " + Request.QueryString["Cuenta_Predial"].ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Adeudos
    ///DESCRIPCIÓN          : Metodo que carga los adeudos de la cuenta predial
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18/sep-2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Adeudos(int Page_Index)
    {
        String Cuenta;
        try
        {
            Cuenta = Request.QueryString["Cuenta_Predial_ID"].ToString();// Session["Cuenta_Predial_ID"].ToString();
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Tasas = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();

            Grid_Adeudos.DataSource = Tasas.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Cuenta, "POR PAGAR", 0, 0);
            Grid_Adeudos.PageIndex = Page_Index;
            Grid_Adeudos.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Desglose_Adeudos: Cargar_Adeudos: " + Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18/sep-2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        // cerrar ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Adeudos_PageIndexChanging
    ///DESCRIPCIÓN          : Manejo del evento cambio de indice de pagina
    ///PARAMETROS           : 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18/sep-2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Adeudos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Adeudos(e.NewPageIndex);
        Grid_Adeudos.SelectedIndex = (-1);
    }
}
