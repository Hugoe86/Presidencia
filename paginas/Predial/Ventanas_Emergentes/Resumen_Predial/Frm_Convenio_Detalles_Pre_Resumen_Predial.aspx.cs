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
using Presidencia.Operacion_Resumen_Predio;
using Presidencia.Operacion_Resumen_Predio.Negocio;


public partial class paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Convenio_Detalles_Pre_Resumen_Predial : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Llenar_Grid_Convenio_Detalles(0);
    }
    protected void Llenar_Grid_Convenio_Detalles(int Page_Index)
    {
        try
        {
            String Convenio = Request.QueryString["No_Convenio"].ToString();// Session["No_Convenio"].ToString().Trim();
            Cls_Ope_Pre_Resumen_Predio_Negocio Convenios_Detalles = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            Convenios_Detalles.P_No_Convenio = Convenio;
            DataTable Dt_Convenios_Detalles = Convenios_Detalles.Consultar_Convenios_Detalles();
            Grid_Convenios_Detalles.PageIndex = Page_Index;
            Grid_Convenios_Detalles.DataSource = Dt_Convenios_Detalles;
            Grid_Convenios_Detalles.Columns[1].Visible = true; 
            Grid_Convenios_Detalles.DataBind();
            Grid_Convenios_Detalles.Columns[1].Visible = false; 
        }
        catch (Exception Ex)
        {
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_Detalles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Convenios_Detalles.SelectedIndex = (-1);
            Llenar_Grid_Convenio_Detalles(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }
    }

}
