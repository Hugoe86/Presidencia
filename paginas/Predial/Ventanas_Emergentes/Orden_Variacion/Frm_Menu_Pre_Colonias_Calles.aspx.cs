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
using Presidencia.Colonias.Negocios;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Constantes;

public partial class paginas_Predial_Ventanas_Emergentes_Orden_Variacion_Frm_Menu_Pre_Colonias_Calles : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            if (!Cargar_Colonias(0))
            {
                Lbl_Ecabezado_Mensaje.Text = "No hay datos qué mostrar";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
            Session["BUSQUEDA_COLONIA_CALLE"] = false;
        }
        Frm_Menu_Pre_Colonias_Calles.Page.Title = "Ubicaciones";
        Lbl_Title.Text = "Selecione una Ubicación por favor";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Colonias
    ///DESCRIPCIÓN          : Método que carga los datos de las Colonias existentes en el catálogo de Cat_Ate_Colonias
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private Boolean Cargar_Colonias(int Page_Index)
    {
        Boolean Colonias_Cargadas = false;
        try
        {
            Cls_Cat_Ate_Colonias_Negocio Colonias = new Cls_Cat_Ate_Colonias_Negocio();

            Colonias.P_Filtros_Dinamicos = "";
            Grid_Colonias.DataSource = Colonias.Consultar_Colonias();
            Grid_Colonias.PageIndex = Page_Index;
            Grid_Colonias.DataBind();
            Colonias_Cargadas = true;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Colonias_Cargadas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Calles
    ///DESCRIPCIÓN          : Método que carga los datos de las Calles existentes en el catálogo de Cat_Pre_Calles
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private Boolean Cargar_Calles(int Page_Index)
    {
        Boolean Calles_Cargadas = false;
        try
        {
            //Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();

            //Calles.P_Filtros_Dinamicos = Cat_Pre_Calles.Campo_Calle_ID + " IN (SELECT " + Cat_Pre_Calles_Colonias.Campo_Calle_ID + " FROM " + Cat_Pre_Calles_Colonias.Tabla_Cat_Pre_Calles_Colonias + " WHERE " + Cat_Pre_Calles_Colonias.Campo_Colonia_ID + " = '" + Grid_Colonias.Rows[Grid_Colonias.SelectedIndex].Cells[1].Text + "')";
            //Grid_Calles.DataSource = Calles.Consultar_Calles();
            //Grid_Calles.PageIndex = Page_Index;
            //Grid_Calles.DataBind();
            //Calles_Cargadas = true;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Calles_Cargadas;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_COLONIA_CALLE"] = false;
        Session.Remove("COLONIA_ID");
        Session.Remove("NOMBRE_COLONIA");
        Session.Remove("CALLE_ID");
        Session.Remove("NOMBRE_CALLE");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Aceptar
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 02/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_COLONIA_CALLE"] = true;
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Colonias_PageIndexChanging
    ///DESCRIPCIÓN          : Evento PageIndexChanging del control Grid_Colonias
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Colonias(e.NewPageIndex);
        Grid_Colonias.SelectedIndex = (-1);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Colonias_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control Grid_Colonias
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Colonias_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cargar_Calles(0);
        Grid_Calles.SelectedIndex = (-1);

        Session["COLONIA_ID"] = Grid_Colonias.Rows[Grid_Colonias.SelectedIndex].Cells[1].Text;
        Session["NOMBRE_COLONIA"] = Grid_Colonias.Rows[Grid_Colonias.SelectedIndex].Cells[2].Text;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Calles_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento SelectedIndexChanged del control Grid_Calles
    ///PARAMETROS           : sender y e
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Calles_SelectedIndexChanged(object sender, EventArgs e)
    {
        Session["CALLE_ID"] = Grid_Calles.Rows[Grid_Calles.SelectedIndex].Cells[1].Text;
        Session["NOMBRE_CALLE"] = Grid_Calles.Rows[Grid_Calles.SelectedIndex].Cells[2].Text;
    }
}
