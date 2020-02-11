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
using Presidencia.Constantes;
using Presidencia.Autorizar_Req_Listado.Negocio;
using Presidencia.Generar_Requisicion.Negocio;

public partial class paginas_Almacen_Frm_Ope_Alm_Autorizar_Requisiciones_Listado : System.Web.UI.Page
{
    #region Variables
   
    //Variable que permitira validar el Estatus Inicial de la requisicion seleccionada
    private static String Estatus_Inicial;
    #endregion

    #region Page Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se ejecuta cuando se carga la pagina
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));

        if (!IsPostBack)
        {
            Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Clase_Negocio= new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
            ViewState["SortDirection"] = "ASC";
           
            Estado_Formulario("inicial");
            Clase_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Clase_Negocio.P_Empleado_ID = Cls_Sessiones.No_Empleado;
            Limpiar_Componentes();
            Llenar_Grid_Requisiciones(Clase_Negocio);
        }
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
    }



    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Componentes
    ///DESCRIPCIÓN: Metodo que limpia los componentes del catalogo
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Limpiar_Componentes()
    {
        Txt_Busqueda.Text = "";
        Txt_Dependencia.Text = "";
        Txt_Tipo.Text = "";
        Txt_Folio.Text = "";
        Txt_Fecha_Generacion.Text = "";
        Txt_Subtotal.Text = "";
        Txt_IVA.Text = "";
        Txt_Total.Text = "";
        Txt_Total_Cotizado.Text = "";
        Txt_IVA_Cotizado.Text = "";
        Txt_Subtotal_Cotizado.Text = "";
        Chk_Verificacion.Checked = false;
        Txt_Tipo_Articulo.Text = "";
        Llenar_Estatus_Cotizado();
        Txt_Justificacion.Text = "";
        Txt_Especificacion.Text = "";
        Div_Productos.Visible = false;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 14/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Estatus_Cotizado()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("CONFIRMADA");
        Cmb_Estatus.Items.Add("COTIZADA-RECHAZADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Formulario
    ///DESCRIPCIÓN: Metodo que indica el estado de los botones del formulario
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Formulario(String Estado)
    {
        switch (Estado)
        {
            case "inicial":
                //Boton Salir
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Div_Busqueda.Visible = true;
                Div_Requisiciones.Visible = true;
                Div_Productos.Visible = false;
                Div_Productos_Cotizados.Visible = false;
                Div_Comentarios.Visible = false;

               
                break;
            case "modificar":
                ////Boton Salir
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.Enabled = true;
                Btn_Salir.Visible = true;
                Div_Busqueda.Visible = false;
                Div_Requisiciones.Visible = false;
                Div_Productos.Visible = true;
                Div_Comentarios.Visible = true;

                break;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus 
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus de la requisicion seleccionada
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Validar_Estatus()
    {
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "+ Debe seleccionar un Estatus <br />";
            Div_Contenedor_Msj_Error.Visible = true;
            Txt_Comentario.Text = "";
        }

    }


    #region Observaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Div_Comentarios
    ///DESCRIPCIÓN: Metodo que carga e inicializa los componentes del ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Cargar_Div_Comentarios(bool Visible)
    {
        Div_Comentarios.Visible = Visible;


        if (Visible == true)
        {
            Llenar_Grid_Comentarios();
            //Validaciones 
            Btn_Alta_Observacion.Visible = true;
            Btn_Alta_Observacion.ToolTip = "Evaluar";
            Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/accept.png";
            Btn_Alta_Observacion.Enabled = true;
            Btn_Cancelar_Observacion.Visible = false;
            Txt_Comentario.Text = "";
            Txt_Comentario.Enabled = false;
        }
        else
        {
            Session["Ds_Comentarios"] = null;
            Grid_Comentarios.DataSource = new DataTable();
            Grid_Comentarios.DataBind();
        }
    }

    #endregion

    #endregion

    #region Grid

    #region Grid Requisiciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Requisicion
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        DataSet Ds_Requisiciones = (DataSet)Session["Ds_Requisiciones"];
        Grid_Requisiciones.DataSource = Ds_Requisiciones;
        Grid_Requisiciones.DataBind();

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite seleccionar un registro dentro de un grid
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
        //Validaciones de los botones
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //Cambiamos el estado del formulario
        Estado_Formulario("modificar");
        //GridViewRow representa una fila individual de un control gridview
        GridViewRow selectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
        String Requisicion_Seleccionada = Convert.ToString(selectedRow.Cells[1].Text.Trim());
        Requisicion_Negocio.P_Folio = Requisicion_Seleccionada;
        DataSet Dato_Requisicion = Requisicion_Negocio.Consulta_Requisiciones();
        //Cargamos los valores con los datos de la requisicion seleccionada en las cajas de texto
        Txt_Dependencia.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[0].ToString();
        Txt_Tipo.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[2].ToString();
        Txt_Folio.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[3].ToString();
        Txt_Fecha_Generacion.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[4].ToString();
        //llenado del Combo
        Estatus_Inicial = Dato_Requisicion.Tables[0].Rows[0].ItemArray[5].ToString();
        Session["Requisicion_ID"] = Dato_Requisicion.Tables[0].Rows[0].ItemArray[10].ToString().Trim();
        if (Estatus_Inicial == "COTIZADA")
        {
            //Llenamos el grid con los datos que cotizo el proveedor 
             Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString();
            Requisicion_Negocio.P_Tipo_Articulo = Dato_Requisicion.Tables[0].Rows[0].ItemArray[14].ToString().Trim();
            //Llenamos el Grid de Productos Cotizados
            DataTable Dt_Cotizados = Requisicion_Negocio.Consulta_Productos_Cotizados();
            Session["Dt_Cotizados"] = Dt_Cotizados;
            Gri_Productos_Cotizados.DataSource = Dt_Cotizados;
            Gri_Productos_Cotizados.DataBind();
            //Cargamos el combo de estatus pero solo con los estatus de Confirmada, Cotizada-Rechazada
            Llenar_Estatus_Cotizado();
            Div_Productos_Cotizados.Visible = true;
            Div_Productos.Visible = true;
            //Asignamos el total Cotizado
            if (Dato_Requisicion.Tables[0].Rows[0].ItemArray[15].ToString() != String.Empty)
                Txt_Total_Cotizado.Text = String.Format("{0:C}",double.Parse(Dato_Requisicion.Tables[0].Rows[0].ItemArray[15].ToString()));
            if (Dato_Requisicion.Tables[0].Rows[0].ItemArray[16].ToString() != String.Empty)
                Txt_Subtotal_Cotizado.Text = String.Format("{0:C}",double.Parse(Dato_Requisicion.Tables[0].Rows[0].ItemArray[16].ToString()));
            if (Dato_Requisicion.Tables[0].Rows[0].ItemArray[17].ToString()!= String.Empty)
                Txt_IVA_Cotizado.Text = String.Format("{0:C}",double.Parse(Dato_Requisicion.Tables[0].Rows[0].ItemArray[17].ToString()));
        }
        if(Dato_Requisicion.Tables[0].Rows[0].ItemArray[6].ToString()!= String.Empty)
            Txt_Subtotal.Text = String.Format("{0:C}",double.Parse(Dato_Requisicion.Tables[0].Rows[0].ItemArray[6].ToString()));
        //Txt_IEPS.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[7].ToString();
        if (Dato_Requisicion.Tables[0].Rows[0].ItemArray[8].ToString() != String.Empty)
            Txt_IVA.Text =String.Format("{0:C}",double.Parse(Dato_Requisicion.Tables[0].Rows[0].ItemArray[8].ToString()));
        if (Dato_Requisicion.Tables[0].Rows[0].ItemArray[6].ToString() != String.Empty)
            Txt_Total.Text = String.Format("{0:C}",double.Parse(Dato_Requisicion.Tables[0].Rows[0].ItemArray[9].ToString()));
        Requisicion_Negocio.P_Requisicion_ID = Dato_Requisicion.Tables[0].Rows[0].ItemArray[10].ToString();
        Session["Requisicion_ID"] = Dato_Requisicion.Tables[0].Rows[0].ItemArray[10].ToString();
        Txt_Justificacion.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[11].ToString();
        Txt_Especificacion.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[12].ToString();
        //llenamos el grid de Productos
        Llenar_Grid_Productos();
        Div_Productos.Visible = true;
        //Seleccionamos el Check de Verificar
        if (Dato_Requisicion.Tables[0].Rows[0].ItemArray[13].ToString() == "SI")
        {
            Chk_Verificacion.Checked = true;
        }
        else
        {
            Chk_Verificacion.Checked = false;
        }
        Txt_Tipo_Articulo.Text = Dato_Requisicion.Tables[0].Rows[0].ItemArray[14].ToString();
        //Habilitamos el boton de Modificar
        //Llenamos el grid de productos de acuerdo a la requisicion seleccionada        
        Cargar_Div_Comentarios(true);
        Requisicion_Negocio.P_Folio = null;

       
    }

    protected void Grid_Requisiciones_Sorting(object sender, GridViewSortEventArgs e)
    {
        Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
        Requisicion_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
        Requisicion_Negocio.P_Empleado_ID = Cls_Sessiones.No_Empleado;

        Llenar_Grid_Requisiciones(Requisicion_Negocio);

        DataTable Dt_Requisiciones = (Grid_Requisiciones.DataSource as DataSet).Tables[0];

        if (Dt_Requisiciones != null)
        {
            DataView Dv_Requisiciones = new DataView(Dt_Requisiciones);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Requisiciones.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Requisiciones.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Requisiciones.DataSource = Dv_Requisiciones;
            Grid_Requisiciones.DataBind();
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Requisiciones
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Requisiciones
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Requisiciones(Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Datos)
    {
        DataSet Data_Set = Requisicion_Datos.Consulta_Requisiciones();
        Div_Requisiciones.Visible = true;
        Session["Ds_Requisiciones"] = null;
        //dEJAMOS VACIO EL GRID DE REQUISICIONES
        Grid_Requisiciones.DataSource = new DataTable();
        Grid_Requisiciones.DataBind();
        if (Data_Set.Tables[0].Rows.Count != 0)
        {
            Session["Ds_Requisiciones"] = Data_Set;
            Grid_Requisiciones.DataSource = Data_Set;
            Grid_Requisiciones.DataBind();
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();
        }

    }

    #endregion

    #region Grid Productos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Productos
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Productos()
    {
        Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio =new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
        Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString().Trim();
        DataSet Data_Set = Requisicion_Negocio.Consulta_Productos();
        if (Data_Set.Tables[0].Rows.Count != 0)
        {
            Div_Productos.Visible = true;
            Session["Dt_Productos"] = Data_Set;
            Grid_Productos.DataSource = Data_Set;
            Grid_Productos.DataBind();
        }
        else
        {
            Div_Requisiciones.Visible = false;

        }
    }

    protected void Grid_Productos_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet Ds = (DataSet)Session["Dt_Productos"];
        DataTable Dt_Productos = Ds.Tables[0];

        if (Dt_Productos != null)
        {
            DataView Dv_Productos = new DataView(Dt_Productos);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Productos.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Productos.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Productos.DataSource = Dv_Productos;
            Grid_Productos.DataBind();
        }

    }

    protected void Grid_Productos_Cotizados_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Productos_Cot = (DataTable)Session["Dt_Cotizados"];

        if (Dt_Productos_Cot != null)
        {
            DataView Dv_Productos_Cot = new DataView(Dt_Productos_Cot);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Productos_Cot.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Productos_Cot.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Gri_Productos_Cotizados.DataSource = Dv_Productos_Cot;
            Gri_Productos_Cotizados.DataBind();
        }

    }


    #endregion



    #region Grid Observaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Comentarios_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite paginar el Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Grid_Comentarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        //GridViewRow representa una fila individual de un control gridview
        GridViewRow selectedRow = Grid_Comentarios.Rows[Grid_Comentarios.SelectedIndex];
        //Cargamos los valores con los datos de la requisicion seleccionada en las cajas de texto
        Txt_Comentario.Text = Convert.ToString(selectedRow.Cells[2].Text);
        Txt_Comentario.Enabled = false;
        Llenar_Grid_Comentarios();

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Comentarios_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite seleccionar un registro en el Grid_Comentarios
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Comentarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

        //Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString();
        //Llenar_Grid_Comentarios();
        Grid_Comentarios.PageIndex = e.NewPageIndex;
        Grid_Comentarios.DataSource = (DataSet)Session["Ds_Comentarios"];
        Grid_Comentarios.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Comentarios
    ///DESCRIPCIÓN: Metodo que permite seleccionar un registro en el Grid_Comentarios
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Comentarios()
    {
        Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
        Requisicion_Negocio.P_Requisicion_ID = Session["Requisicion_ID"].ToString().Trim();
        DataSet Data_Set = Requisicion_Negocio.Consulta_Observaciones();
        Session["Ds_Comentarios"] = Data_Set;
        if (Data_Set.Tables[0].Rows.Count != 0)
        {
            Grid_Comentarios.DataSource = Data_Set;
            Grid_Comentarios.DataBind();
            Div_Comentarios.Visible = true;
            Grid_Comentarios.Visible = true;
        }
    }
    #endregion

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del boton salir que tiene tambien la funcionalidad de cancelar y salir 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
        switch (Btn_Salir.ToolTip)
        {

            case "Listado":

                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = false;
                Estado_Formulario("inicial");
                Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 0;
                Limpiar_Componentes();
                Div_Productos.Visible = false;
                Div_Comentarios.Visible = false;
                Div_Busqueda.Visible = true;
                Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
                Llenar_Grid_Requisiciones(Requisicion_Negocio);
                break;
            case "Inicio":
                Limpiar_Componentes();
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
        }//fin del switch

    }

    


    #region Manejo Observaciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Alta_Observacion_Click
    ///DESCRIPCIÓN: Evento del boton dar de alta una observacion, que a su ves permitira modificar el estatus
    ///             de la Requisicion. 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Alta_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
        //Cambiamos la imagen del boton 
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        switch (Btn_Alta_Observacion.ToolTip)
        {
            case "Evaluar":
                Btn_Alta_Observacion.ToolTip = "Guardar";
                Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Alta_Observacion.Enabled = true;
                Btn_Cancelar_Observacion.Visible = true;
                Txt_Comentario.Enabled = true;
                Txt_Comentario.Text = "";
                Cmb_Estatus.Enabled = true;

                break;
            case "Guardar":
                //Validamos que seleccione un estatus
                Validar_Estatus();
              

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    try
                    {
                        //Asignamos los valores a la clase de negocio
                        //Generamos el Id de la nueva Observacion
                        Requisicion_Negocio.P_Observacion_ID = Requisicion_Negocio.Generar_ID();
                        Requisicion_Negocio.P_Folio = Txt_Folio.Text;
                        Requisicion_Negocio.P_Tipo = Txt_Tipo.Text.Trim();
                        //Hacemos nulos los que no son necesarios
                        Requisicion_Negocio.P_Estatus_Busqueda = null;
                        Requisicion_Negocio.P_Campo_Busqueda = null;
                        Requisicion_Negocio.P_Fecha_Inicial = null;
                        //Obtenemos el Id de la requisicion seleccionada
                        DataSet Dato_Requisicion_ID = Requisicion_Negocio.Consulta_Requisiciones();
                        Requisicion_Negocio.P_Requisicion_ID = Dato_Requisicion_ID.Tables[0].Rows[0].ItemArray[10].ToString();
                        Requisicion_Negocio.P_Comentario = Txt_Comentario.Text;
                        Requisicion_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
                        Requisicion_Negocio.Modificar_Requisicion();
                        if (Txt_Comentario.Text.Trim().Length > 0)
                        {
                            Requisicion_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Requisicion_Negocio.Alta_Observaciones();
                        }
                        //Registramos el historial
                        Cls_Ope_Com_Requisiciones_Negocio Requisicion = new Cls_Ope_Com_Requisiciones_Negocio();
                        Requisicion.Registrar_Historial(Cmb_Estatus.SelectedValue.Trim(), Requisicion_Negocio.P_Requisicion_ID);
                        if (Requisicion_Negocio.P_Estatus == "COTIZADA-RECHAZADA")
                            Requisicion.Registrar_Historial("PROVEEDOR", Requisicion_Negocio.P_Requisicion_ID);
                        //Registramos la accion en la bitacora de Eventos
                        Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
                        Estado_Formulario("inicial");
                        Llenar_Grid_Requisiciones(Requisicion_Negocio);
                        Txt_Comentario.Enabled = false;
                        Cmb_Estatus.Enabled = false;
                        //Modificamos los botones
                        Btn_Alta_Observacion.ToolTip = "Evaluar";
                        Btn_Alta_Observacion.ImageUrl = "~/paginas/imagenes/paginas/accept.png";
                        Btn_Alta_Observacion.Enabled = true;
                        Btn_Cancelar_Observacion.Visible = false;
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Requisiciones", "alert('Requisición modificada');", true);
                    }
                    catch (Exception Ex)
                    {
                        Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
                        Estado_Formulario("inicial");
                        Llenar_Grid_Requisiciones(Requisicion_Negocio);
                        throw new Exception("Error al modificar la Requisicion. Error: [" + Ex.Message + "]");
                    }
                    
                }
                break;


        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Observacion_Click
    ///DESCRIPCIÓN: Evento del boton Cancelar una observacion u comentario
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cancelar_Observacion_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio Requisicion_Negocio = new Cls_Ope_Alm_Autorizar_Requisiciones_Listado_Negocio();
        Limpiar_Componentes();
        Cmb_Estatus.Enabled = false;
        Cargar_Div_Comentarios(false);
        Estado_Formulario("inicial");
        Llenar_Grid_Requisiciones(Requisicion_Negocio);
    }

    #endregion

    #endregion
}
