using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class paginas_Predial_Ventanas_Emergentes_PAE_Frm_Motivo_Omision : System.Web.UI.Page
{
    #region Page Load
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno 
    ///FECHA_CREO           : 13/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_MOTIVO"] = false;
        }
        Frm_Motivo_Omision.Page.Title = "OMITIR CUENTA";
        Lbl_Title.Text = "Introduce el Motivo de Omision";
    }
    #endregion
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 13/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_MOTIVO"] = false;
        Session.Remove("MOTIVO");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Acepta el motivo de omision
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 13/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        if (Txt_Motivo.Text != "" && Txt_Motivo.Text.Length < 250)
        {
            Session["MOTIVO"] = Txt_Motivo.Text.ToUpper();
            Session["BUSQUEDA_MOTIVO"] = true;
            //Cierra la ventana
            string Pagina = "<script language='JavaScript'>";
            Pagina += "window.close();";
            Pagina += "</script>";
            //Page.RegisterStartupScript("Cerrar_Script", Pagina);
            ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
        }
        else
        {
            Lbl_Ecabezado_Mensaje.Text = "Introduce el Motivo de Omisión";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
}
