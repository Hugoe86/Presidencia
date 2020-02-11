using System;
using System.Data;
using System.Web.UI;
using Presidencia.Consulta_Peticiones.Negocios;
using System.Web.UI.WebControls;

public partial class paginas_Atencion_Ciudadana_Ventanas_Emergentes_Frm_Ate_Busqueda_Avanzada_Ciudadano : System.Web.UI.Page
{
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Page_Load
    ///DESCRIPCIÓN          : Método que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS           : NO APLICA
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-jun-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        if (!IsPostBack)
        {
            Session["BUSQUEDA_CIUDADANO"] = false;
            Txt_Filtro_Nombre.Focus();
        }
        Frm_Busqueda_Avanzada_Ciudadano.Page.Title = "Búsqueda Avanzada de Ciudadano";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : NO APLICA
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-jun-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_CIUDADANO"] = false;
        Session.Remove("CIUDADANO_ID");
        Session.Remove("NO_PETICION");
        Session.Remove("ANIO_PETICION");
        Session.Remove("PROGRAMA_ID");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Datos_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar datos, regresa 
    ///             en variables de sesión los datos ingresados para la búsqueda
    ///PARAMETROS           : NO APLICA
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-jun-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Datos_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session["NOMBRE"] = Txt_Filtro_Nombre.Text;
            Session["APELLIDO_PATERNO"] = Txt_Filtro_Apellido_Paterno.Text;
            Session["APELLIDO_MATERNO"] = Txt_Filtro_Apellido_Materno.Text;
            Session["TELEFONO"] = Txt_Filtro_Telefono.Text;
            Session["EMAIL"] = Txt_Filtro_Email.Text;

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Limpiar_Click
    ///DESCRIPCIÓN          : Evento Clic del control Button Limpiar
    ///PARAMETROS           : NO APLICA
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-jun-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Filtro_Apellido_Materno.Text = "";
        Txt_Filtro_Apellido_Paterno.Text = "";
        Txt_Filtro_Email.Text = "";
        Txt_Filtro_Nombre.Text = "";
        Txt_Filtro_Telefono.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Evento Clic del control Button Aceptar
    ///PARAMETROS           : NO APLICA
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-jun-2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Session["BUSQUEDA_CIUDADANO"] = true;
        //Cierra la ventana
        string Script_Cerrar_Ventana = "<script language='JavaScript'>";
        Script_Cerrar_Ventana += "window.close();";
        Script_Cerrar_Ventana += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Script_Cerrar_Ventana);
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN      : Grid_Ciudadano_SelectedIndexChanged
    ///DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///PARÁMETROS:
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-jun-2012 
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Ciudadano_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Session["CIUDADANO_ID"] = Grid_Ciudadano.SelectedRow.Cells[1].Text.ToString();
            Session["NO_PETICION"] = Grid_Ciudadano.SelectedRow.Cells[2].Text.ToString();
            Session["ANIO_PETICION"] = Grid_Ciudadano.SelectedRow.Cells[3].Text.ToString();
            Session["PROGRAMA_ID"] = Grid_Ciudadano.SelectedRow.Cells[4].Text.ToString();

            Btn_Aceptar_Click(sender, null);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Grid_Ciudadano_Sorting
    ///DESCRIPCIÓN: Maneja el evento cambio del orden del grid, toma los datos de variables de sesión
    ///PARÁMETROS: NO APLICA
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Ciudadano_Sorting(object sender, GridViewSortEventArgs e)
    {
        if (Session["Dt_Consulta_Solicitantes_Peticiones"] != null)
        {

            DataTable Dt_Consulta = (DataTable)Session["Dt_Consulta_Solicitantes_Peticiones"];
            String Orden = ViewState["SortDirection"].ToString();
            if (Orden.Equals("ASC"))
            {
                Cargar_Datos_Grid(Dt_Consulta, e.SortExpression + " DESC");
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Cargar_Datos_Grid(Dt_Consulta, e.SortExpression + " ASC");
                ViewState["SortDirection"] = "ASC";
            }
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Btn_Buscar_Solicitante_Click
    /// DESCRIPCION             : llama al método que realiza la búsqueda y carga los resultados en el grid
    ///CREO                     : Roberto González Oseguera
    ///FECHA_CREO               : 18-jun-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Solicitante_Click(object sender, EventArgs e)
    {
        // validar que por lo menos un campo contenga texto
        if (Txt_Filtro_Nombre.Text.Trim() != "" || Txt_Filtro_Apellido_Paterno.Text.Trim() != "" || Txt_Filtro_Apellido_Materno.Text.Trim() != "" || Txt_Filtro_Telefono.Text.Trim() != "")
        Buscar_Ciudadano();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Buscar_Tramites
    /// DESCRIPCION             : buscara los tramites
    ///CREO                     : Roberto González Oseguera
    ///FECHA_CREO               : 18-jun-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Buscar_Ciudadano()
    {
        Cls_Ope_Consulta_Peticiones_Negocio Obj_Consulta_Usuario = new Cls_Ope_Consulta_Peticiones_Negocio();
        DataTable Dt_Consulta = null;

        //  filtro nombre
        if (Txt_Filtro_Nombre.Text != "")
            Obj_Consulta_Usuario.P_Nombre_Solicitante = Txt_Filtro_Nombre.Text.Trim();

        //  filtro apellido paterno
        if (Txt_Filtro_Apellido_Paterno.Text != "")
            Obj_Consulta_Usuario.P_Apellido_Paterno = Txt_Filtro_Apellido_Paterno.Text.Trim();

        //  filtro apellido materno
        if (Txt_Filtro_Apellido_Materno.Text != "")
            Obj_Consulta_Usuario.P_Apellido_Materno = Txt_Filtro_Apellido_Materno.Text.Trim();

        //  filtro Email
        if (Txt_Filtro_Email.Text != "")
            Obj_Consulta_Usuario.P_Email = Txt_Filtro_Email.Text.Trim();

        //  filtro teléfono o celular
        if (Txt_Filtro_Telefono.Text != "")
            Obj_Consulta_Usuario.P_Telefono = Txt_Filtro_Telefono.Text.Trim();

        //  se realiza la consulta
        Dt_Consulta = Obj_Consulta_Usuario.Consultar_Solicitantes();

        // validar que se recibieron resultados de la consulta
        if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
        {
            // cargar datos en el grid
            Cargar_Datos_Grid(Dt_Consulta, "NOMBRE_CIUDADANO ASC");
            // guardar tabla en variable de sesión
            Session["Dt_Consulta_Solicitantes_Peticiones"] = Dt_Consulta;
            ViewState["SortDirection"] = "ASC";
        }
        else
        {
            // limpiar grid y eliminar sesión
            Cargar_Datos_Grid(null, null);
            Session["Dt_Consulta_Solicitantes_Peticiones"] = null;
        }
    }

    /// *****************************************************************************************
    /// NOMBRE: Cargar_Datos_Grid
    /// DESCRIPCIÓN: Carga los datos especificados en el grid ciudadano
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18-may-2012
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************
    private void Cargar_Datos_Grid(DataTable Dt_Consulta, string Orden)
    {
        if (Dt_Consulta != null && !string.IsNullOrEmpty(Orden))
        {
            Dt_Consulta.DefaultView.Sort = Orden;
        }

        // mostrar resultado en grid peticiones
        Grid_Ciudadano.Columns[1].Visible = true;
        Grid_Ciudadano.Columns[2].Visible = true;
        Grid_Ciudadano.Columns[3].Visible = true;
        Grid_Ciudadano.Columns[4].Visible = true;
        Grid_Ciudadano.DataSource = Dt_Consulta;
        Grid_Ciudadano.DataBind();
        Grid_Ciudadano.Columns[1].Visible = false;
        Grid_Ciudadano.Columns[2].Visible = false;
        Grid_Ciudadano.Columns[3].Visible = false;
        Grid_Ciudadano.Columns[4].Visible = false;
    }

}
