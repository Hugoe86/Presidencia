using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Multas_Fraccionamientos.Negocio;
using System.Data;

public partial class paginas_Predial_Ventanas_Emergentes_Fraccionamientos_Frm_Menu_Pre_Multas : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : sender y e
    ///CREO                 : Ismael Prieto Sánchez
    ///FECHA_CREO           : 31/Mayo/2012 
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
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_MULTA_ID");
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_MULTA");
            Txt_Busqueda_Año.Text = DateTime.Now.Year.ToString();
            Cargar_Montos_Multas(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Montos_Multas
    ///DESCRIPCIÓN          : Metodo que carga los datos de multas existentes en el catálogo de Cat_Pre_Multas_Fraccionamientos
    ///PARAMETROS: 
    ///CREO                 : Ismael Prieto Sánchez
    ///FECHA_CREO           : 31/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Montos_Multas(int Page_Index)
    {
        Cls_Cat_Pre_Multas_Fraccionamientos_Negocio Multas = new Cls_Cat_Pre_Multas_Fraccionamientos_Negocio(); //Asigna para el manejo de la clase de negocio

        try
        {
            //Asigna los filtros para la busqueda
            if (Txt_Busqueda_Año.Text.Trim() != "")
            {
                Multas.P_Incluir_Campos_Foraneos = true;
                Multas.P_Identificador = "";
                if (Txt_Busqueda_Año.Text.Length != 0)
                {
                    Multas.P_Filtros_Dinamicos = " " + Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Año + " LIKE '%" + Txt_Busqueda_Año.Text + "%'";
                }
                //Asigna al grid
                Grid_Multas.Columns[1].Visible = true;
                Grid_Multas.DataSource = Multas.Consultar_Cuotas_Multas();
                Grid_Multas.PageIndex = Page_Index;
                Grid_Multas.DataBind();
                Grid_Multas.Columns[1].Visible = false;
            }
            else
            {
                Grid_Multas.DataSource = null;
                Grid_Multas.PageIndex = 0;
                Grid_Multas.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Ismael Prieto Sánchez
    ///FECHA_CREO           : 31/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session.Remove("IMPUESTO_FRACCIONAMIENTO_MULTA_ID");
        Session.Remove("IMPUESTO_FRACCIONAMIENTO_MULTA");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Costos_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Busqueda Costos
    ///PARAMETROS           : sender y e
    ///CREO                 : Ismael Prieto Sánchez
    ///FECHA_CREO           : 31/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Btn_Busqueda_Costos_Click(object sender, EventArgs e)
    {
        Cargar_Montos_Multas(0);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Costos_PageIndexChanging
    ///DESCRIPCIÓN          : Realiza la paginacion sobre el grid
    ///PARAMETROS: 
    ///CREO                 : Ismael Prieto Sánchez
    ///FECHA_CREO           : 31/Mayo/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Multas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Cargar_Montos_Multas(e.NewPageIndex);
            Grid_Multas.SelectedIndex = (-1);
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
    protected void Grid_Multas_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Asigna los datos
        Session["IMPUESTO_FRACCIONAMIENTO_MULTA_ID"] = Grid_Multas.Rows[Grid_Multas.SelectedIndex].Cells[1].Text;
        Session["IMPUESTO_FRACCIONAMIENTO_MULTA"] = Grid_Multas.Rows[Grid_Multas.SelectedIndex].Cells[3].Text;

        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
}
