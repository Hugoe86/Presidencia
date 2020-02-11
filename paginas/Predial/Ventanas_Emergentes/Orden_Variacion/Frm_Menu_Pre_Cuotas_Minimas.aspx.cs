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
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Cuotas_Minimas : System.Web.UI.Page
{
    #region Variables
    private static DataTable Dt_Cuota_Minima;
    #endregion
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Christian Perez Ibarra
    ///FECHA_CREO           : 18/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session.Remove("Notario_ID");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    protected void Btn_Busqueda_Cuota_Minima_Click(object sender, EventArgs e)
    {
        try
        {
            Cargar_Grid(0);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid
    ///DESCRIPCIÓN: Realizar la consulta y llenar el grid con estos datos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 21/Julio/2011 12:14:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Grid(int Page_Index)
    {
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();

        try
        {

            Cuotas_Minimas.P_Cuota_Minima_ID  = Txt_Cuota_Minima_ID.Text.Trim();
            Cuotas_Minimas.P_Anio  = Txt_Ano_Cuota_Minima.Text.Trim();
            Cuotas_Minimas.P_Cantidad_Cuota = Txt_Cuota.Text.Trim();
            Dt_Cuota_Minima = Cuotas_Minimas.Consultar_Cuotas_Minimas_Ventana_Emergente();



            //Boolean Variable = Fecha;
            if (Dt_Cuota_Minima.Rows.Count > 0)
            {
                Grid_Cuotas_Minimas.PageIndex = Page_Index;
                Grid_Cuotas_Minimas.DataSource = Dt_Cuota_Minima;
                Grid_Cuotas_Minimas.DataBind();

            }
            else
            {
                Grid_Cuotas_Minimas.DataSource = null;
                Grid_Cuotas_Minimas.DataBind();
                Mensaje_Error("No se encontraron Registros");
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Cuotas_Minimas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Cuotas_Minimas.SelectedIndex = (-1);
            Cargar_Grid(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Grid_Cuotas_Minimas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["Dr_Cuotas_Minimas"] = (Dt_Cuota_Minima.Rows[Grid_Cuotas_Minimas.SelectedIndex +
                    (Grid_Cuotas_Minimas.PageIndex * Grid_Cuotas_Minimas.PageSize)]);

        string Pagina = "<script language='JavaScript'> ";
        Pagina += "window.close(); ";
        Pagina += "</script>";
        Page.RegisterStartupScript("Cerrar_Script", Pagina);
        //ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    private void Mensaje_Error(String P_Mensaje)
    {
        IBtn_Imagen_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }
    private void Mensaje_Error()
    {
        IBtn_Imagen_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }
}
