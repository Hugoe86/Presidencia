using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Predial_Pae_Honorarios.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_PAE_Frm_Detalles_Gastos_De_Ejecucion : System.Web.UI.Page
{
    string cuenta;
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {       
        if (!IsPostBack)
        {
            cuenta = Request.QueryString["Cuenta_Predial"].ToString();
            Session["DETALLES"] = false;
            Muestra_Detalles();
        }
        Frm_Detalle_Gasto_Ejecucion.Page.Title = "DETALLES CUENTA";
        Mensaje_Error();
    }
    #endregion

    #region Metodos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Muestra_Detalles
    ///DESCRIPCIÓN          : Muestra los detalles de los Gastos de la cuenta predial
    ///                       
    ///PARAMETROS: 
    ///CREO                 : Angel Escamilla Trejo
    ///FECHA_CREO           : 15/03/2012 12:24:00 p.m.
    ///MODIFICO:            : Armando Zavala Moreno
    ///FECHA_MODIFICO       : 16/03/2012 01:46:00 pm
    ///CAUSA_MODIFICACIÓN   : Cambio de nombres de session y validacion de las sessiones 
    ///*******************************************************************************
    private void Muestra_Detalles()
    {
        DataTable Dt_Detalles;
        Cls_Ope_Pre_Pae_Honorarios_Negocio Rezagos = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        try
        {
            Mensaje_Error();
            if (!String.IsNullOrEmpty(cuenta))
            {
                Rezagos.P_Cuenta_Predial = cuenta;
                Dt_Detalles = Rezagos.Consultar_Detalles_Honorarios();
                if (Dt_Detalles != null && Dt_Detalles.Rows.Count > 0)
                {
                    Txt_Corriente.Text = String.Format("{0:C2}", Convert.ToDouble(Dt_Detalles.Rows[0]["CORRIENTE"].ToString()));
                    Txt_Rezago.Text = String.Format("{0:C2}", Convert.ToDouble(Dt_Detalles.Rows[0]["REZAGO"].ToString()));
                    Txt_Recargos_Ordinarios.Text = String.Format("{0:C2}", Convert.ToDouble(Dt_Detalles.Rows[0]["ORDINARIOS"].ToString()));
                    Txt_Recargos_Moratorios.Text = String.Format("{0:C2}", Convert.ToDouble(Dt_Detalles.Rows[0]["MORATORIOS"].ToString()));
                    Txt_Honorarios.Text = String.Format("{0:C2}", Convert.ToDouble(Dt_Detalles.Rows[0]["HONORARIOS_TOTAL"].ToString()));
                    Txt_Gasto_Ejecucion.Text = String.Format("{0:C2}", Convert.ToDouble(0));
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Armando Zavala Moreno.
    ///FECHA_CREO  : 17-Abril-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        IBtn_Imagen_Error.Visible = true;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Ecabezado_Mensaje.Text += P_Mensaje + "</br>";
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = true;

    }
    private void Mensaje_Error()
    {
        IBtn_Imagen_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Ecabezado_Mensaje.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }
    #endregion
    #region Eventos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Muestra los detalles de los Gastos de la cuenta predial
    ///                       
    ///PARAMETROS: 
    ///CREO                 : Angel Escamilla Trejo
    ///FECHA_CREO           : 16/03/2012 01:46:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["DETALLES"] = false;
        cuenta = "";
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    #endregion
}
