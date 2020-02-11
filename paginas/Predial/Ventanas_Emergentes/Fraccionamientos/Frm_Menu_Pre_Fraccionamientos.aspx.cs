using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Fraccionamientos.Negocio;
using System.Data;

public partial class paginas_Predial_Ventanas_Emergentes_Fraccionamientos_Frm_Menu_Pre_Fraccionamientos : System.Web.UI.Page
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
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO_ID");
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO_DESCRIPCION");
            Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO");
            Txt_Busqueda_Año.Text = DateTime.Now.Year.ToString();
            Cargar_Costos_Fraccionamientos(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Costos_Fraccionamientos
    ///DESCRIPCIÓN          : Metodo que carga los datos de Costos existentes en el catálogo de Cat_Pre_Fraccionamientos
    ///PARAMETROS: 
    ///CREO                 : Ismael Prieto Sánchez
    ///FECHA_CREO           : 31/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Costos_Fraccionamientos(int Page_Index)
    {
        Cls_Cat_Pre_Fraccionamientos_Negocio Costos = new Cls_Cat_Pre_Fraccionamientos_Negocio(); //Asigna para el manejo de la clase de negocio
        
        try
        {
            //Asigna los filtros para la busqueda
            if (Txt_Busqueda_Identificador.Text.Trim() != ""
            || Txt_Busqueda_Descripcion.Text.Trim() != ""
            || Txt_Busqueda_Año.Text.Trim() != "")
            {
                Costos.P_Filtros_Dinamicos = Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Estatus + "='VIGENTE'";
                if (Txt_Busqueda_Identificador.Text.Trim() != "")
                {
                    if (Costos.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Costos.P_Filtros_Dinamicos += " AND ";
                    }
                    Costos.P_Filtros_Dinamicos += Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Identificador + " LIKE '%" + Txt_Busqueda_Identificador.Text.Trim().ToUpper() + "%'";
                }
                if (Txt_Busqueda_Descripcion.Text.Trim() != "")
                {
                    if (Costos.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Costos.P_Filtros_Dinamicos += " AND ";
                    }
                    Costos.P_Filtros_Dinamicos += Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Descripcion + " LIKE '%" + Txt_Busqueda_Descripcion.Text.Trim().ToUpper() + "%'";
                }
                if (Txt_Busqueda_Año.Text.Trim() != "")
                {
                    if (Costos.P_Filtros_Dinamicos.Trim() != "")
                    {
                        Costos.P_Filtros_Dinamicos += " AND ";
                    }
                    Costos.P_Filtros_Dinamicos += Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Año + " = " + Txt_Busqueda_Año.Text.Trim();
                }
                Costos.P_Ordenar_Dinamico = Cat_Pre_Fracc_Impuestos.Tabla_Cat_Pre_Fracc_Impuestos + "." + Cat_Pre_Fracc_Impuestos.Campo_Año + ", " + Cat_Pre_Fraccionamientos.Tabla_Cat_Pre_Fraccionamientos + "." + Cat_Pre_Fraccionamientos.Campo_Identificador;
                //Asigna al grid
                Grid_Costos.Columns[1].Visible = true;
                Grid_Costos.Columns[3].Visible = true;
                Grid_Costos.DataSource = Costos.Consultar_Fraccionamientos_Impuestos();
                Grid_Costos.PageIndex = Page_Index;
                Grid_Costos.DataBind();
                Grid_Costos.Columns[1].Visible = false;
                Grid_Costos.Columns[3].Visible = false;
            }
            else
            {
                Grid_Costos.DataSource = null;
                Grid_Costos.PageIndex = 0;
                Grid_Costos.DataBind();
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
        Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO_ID");
        Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO_DESCRIPCION");
        Session.Remove("IMPUESTO_FRACCIONAMIENTO_COSTO");
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
        Cargar_Costos_Fraccionamientos(0);
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
    protected void Grid_Costos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Cargar_Costos_Fraccionamientos(e.NewPageIndex);
            Grid_Costos.SelectedIndex = (-1);
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
    protected void Grid_Costos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Asigna los datos
        Session["IMPUESTO_FRACCIONAMIENTO_COSTO_ID"] = Grid_Costos.SelectedRow.Cells[1].Text;
        Session["IMPUESTO_FRACCIONAMIENTO_COSTO_DESCRIPCION"] = Grid_Costos.SelectedRow.Cells[4].Text;
        Session["IMPUESTO_FRACCIONAMIENTO_COSTO"] = Grid_Costos.SelectedRow.Cells[6].Text;

        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        //Page.RegisterStartupScript("Cerrar_Script", Pagina);
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
}
