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
using Presidencia.Cotizacion_Manual_Pull.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Listado_Ordenes_Compra.Negocio;
using Presidencia.Orden_Compra.Negocio;


public partial class paginas_Compras_Frm_Ope_Com_Modificar_Datos_Orden_Compra : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        Cls_Sessiones.Mostrar_Menu = true;
        if (!IsPostBack)
        {
            Session["Activa"] = true;
            ViewState["SortDirection"] = "ASC";
            Configurar_Formulario("Busqueda");
           
        }
    }

    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configurar_Formulario
    ///DESCRIPCIÓN: Metodo que configura el formulario con respecto al estado de habilitado o visible
    ///´de los componentes de la pagina
    ///PARAMETROS: 1.- String Estatus: Estatus que puede tomar el formulario con respecto a sus componentes, ya sea "Inicio" o "Nuevo"
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Configurar_Formulario(String Estatus)
    {

        switch (Estatus)
        {
            case "Busqueda":

                Div_Detalle_Requisicion.Visible = true;
                Div_Grid_Requisiciones.Visible = false;
                Div_Busqueda.Visible = true;

                //Boton Modificar
                Btn_Nuevo.Visible = false;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                //Grid_Requisiciones.Visible = true;
                //Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = false;
                //Cargamos las fechas al dia de hoy
                Txt_Busqueda_Fecha_Elaboracion_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Fecha_Elaboracion_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Vigencia_Propuesta_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Vigencia_Propuesta_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                
                Txt_Busqueda_Fecha_Elaboracion_Ini.Enabled = false;
                Txt_Busqueda_Fecha_Elaboracion_Fin.Enabled = false;
                Txt_Busqueda_Vigencia_Propuesta_Ini.Enabled = false;
                Txt_Busqueda_Vigencia_Propuesta_Fin.Enabled = false;

                //Deseleccionamos los check box
                Chk_Fecha_Elaboracion.Checked = false;
                Chk_Vigencia_Propuesta.Checked = false;
                //DEshabilitamos botones
                Btn_Busqueda_Fecha_Elaboracion_Ini.Enabled = false;
                Btn_Busqueda_Fecha_Elaboracion_Fin.Enabled = false;
                Btn_Busqueda_Vigencia_Propuesta_Ini.Enabled = false;
                Btn_Busqueda_Vigencia_Propuesta_Fin.Enabled = false;
                //llenamos el combo de cotizadores
                Txt_Condiciones_Compra.Enabled = true;
             
                break;
            case "Inicio":
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Cotizar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Enabled = true;
                Btn_Nuevo.Visible = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Div_Busqueda.Visible = true;
                Div_Grid_Requisiciones.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = false;
                //Deshabilitar controles 
                Txt_Condiciones_Compra.Enabled = false;
                break;
            case "Nuevo":

                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //
                Div_Grid_Requisiciones.Visible = false;
                Div_Busqueda.Visible = true;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = true;
                //habilitar controles 
                Txt_Condiciones_Compra.Enabled = true;

                break;
        }//fin del switch

    }



    public void Limpiar_Componentes()
    {
        Session["Concepto_ID"] = null;
        Session["Dt_Productos_OC"] = null;
        Session["Dt_Requisiciones"] = null;
        Session["No_Requisicion"] = null;
        Session["TIPO_ARTICULO"] = null;
        Session["Concepto_ID"] = null;
        Session["Proveedor_ID"] = null;
        Session["No_Requisicion"] = null;
        Txt_Proveedor.Text = "";
        Txt_Dependencia.Text = "";
        
        Txt_Folio.Text = "";
        Txt_Reserva.Text = "";
        Txt_Condiciones_Compra.Text = "";
        Txt_Codigo_Programatico.Text = "";

        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        Txt_SubTotal_Cotizado_Requisicion.Text = "";
        Txt_Total_Cotizado_Requisicion.Text = "";
        Txt_IEPS_Cotizado.Text = "";
        Txt_IVA_Cotizado.Text = "";


    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Formato_Fecha
    ///DESCRIPCIÓN: Metodo que cambia el mes dic a dec para que oracle lo acepte
    ///PARAMETROS:  1.- String Fecha, es la fecha a la cual se le cambiara el formato 
    ///                     en caso de que cumpla la condicion del if
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 2/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Formato_Fecha(String Fecha)
    {

        String Fecha_Valida = Fecha;
        //Se le aplica un split a la fecha 
        String[] aux = Fecha.Split('/');
        //Se modifica el es a solo mayusculas para que oracle acepte el formato. 
        switch (aux[1])
        {
            case "dic":
                aux[1] = "DEC";
                break;
        }
        //Concatenamos la fecha, y se cambia el orden a DD-MMM-YYYY para que sea una fecha valida para oracle
        Fecha_Valida = aux[0] + "-" + aux[1] + "-" + aux[2];
        return Fecha_Valida;
    }// fin de Formato_Fecha
    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    #region Grid_Requisiciones


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del Grid_Requisiciones al seleccionar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
        GridViewRow selectedRow = null;//Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
        int num_fila = 0;// Grid_Requisiciones.SelectedIndex;
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

        Clase_Negocio.P_Proveedor_ID = Dt_Requisiciones.Rows[num_fila]["Proveedor_ID"].ToString().Trim();
        Session["Proveedor_ID"] = Clase_Negocio.P_Proveedor_ID.Trim();

        Clase_Negocio.P_No_Requisicion = 0+"";// Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        Session["No_Requisicion"] = Clase_Negocio.P_No_Requisicion;
        //Consultamos los detalles del producto seleccionado 
        DataTable Dt_Detalle_Requisicion = Clase_Negocio.Consultar_Detalle_Requisicion();
        //Mostramos el div de detalle y el grid de Requisiciones
        Div_Grid_Requisiciones.Visible = false;
        Div_Detalle_Requisicion.Visible = true;
        Btn_Salir.ToolTip = "Listado";
        //llenamos la informacion del detalle de la requisicion seleccionada
        Txt_Proveedor.Text = Dt_Requisiciones.Rows[num_fila]["Nombre_Proveedor"].ToString().Trim();
        Txt_Dependencia.Text = Dt_Detalle_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
        Txt_Folio.Text = Dt_Detalle_Requisicion.Rows[0]["FOLIO"].ToString().Trim();
    
    



        Session["Concepto_ID"] = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO_ID"].ToString().Trim();
        //LLenamos los text de la propuesta de Cotizacion
        DataTable Dt_Propuesta = Clase_Negocio.Consultar_Propuesta_Cotizacion();


        //Asignamos los text
        Txt_Total_Cotizado_Requisicion.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado_Requisicion].ToString().Trim();
        Txt_SubTotal_Cotizado_Requisicion.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Subtotal_Cotizado_Requisicion].ToString().Trim();
        Txt_IEPS_Cotizado.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado_Req].ToString().Trim();
        Txt_IVA_Cotizado.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado_Req].ToString().Trim();



        //VALIDAMOS EL CAMPO DE VERIFICAR CARACTERISTICAS, GARANTIA Y POLIZAS

        //Consultamos los productos de esta requisicion
        Clase_Negocio.P_Tipo_Articulo = Session["TIPO_ARTICULO"].ToString().Trim();
        DataTable Dt_Productos = Clase_Negocio.Consultar_Productos_Servicios();
        //llenamos el grid de productos
        if (Dt_Productos.Rows.Count != 0)
        {
            Session["Dt_Productos_OC"] = Dt_Productos;
            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();
            Grid_Productos.Visible = true;
            Grid_Productos.Enabled = false;
            //Llenamos los Text Box con los datos del Dt_Productos

            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {

                TextBox Txt_Precio_Unitario = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
                TextBox Txt_Marca = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Marca");
                TextBox Txt_Descripcion_Producto_Cot = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Descripcion_Producto_Cot");

                if (Txt_Precio_Unitario != null)
                {
                    Txt_Precio_Unitario.Text = Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"].ToString().Trim();
                    
                }//fin del IF
                if (Txt_Marca != null)
                {
                    Txt_Marca.Text = Dt_Productos.Rows[i]["Marca"].ToString().Trim();
                    if (Txt_Marca.Text.Trim().Length == 0)
                    {
                        Txt_Marca.Text = "SIN MARCA";
                    }
                }
                if (Txt_Descripcion_Producto_Cot != null)
                {
                    Txt_Descripcion_Producto_Cot.Text = Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"].ToString().Trim();
                    if (Txt_Descripcion_Producto_Cot.Text.Trim().Length == 0)
                    {
                        Txt_Descripcion_Producto_Cot.Text = Dt_Productos.Rows[i]["NOMBRE_DESCRIPCION"].ToString().Trim();
                    }
                }

            }//Fin del for

        }


        Div_Grid_Requisiciones.Visible = false;
        Div_Detalle_Requisicion.Visible = true;

        Configurar_Formulario("General");
        Btn_Salir.ToolTip = "Listado";

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Grid_Requisiciones_Sorting
    ///DESCRIPCIÓN: Evento para ordenar por columna seleccionada en el Grid_Requisiciones
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

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

            //Guardamos el cambio dentro de la variable de session de Dt_Requisiciones
            Session["Dt_Requisiciones"] = (DataTable)Dv_Requisiciones.Table;
            Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

        }

    }
    #endregion

    #region Grid_Productos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Grid_Productos_Sorting
    ///DESCRIPCIÓN: Evento para ordenar por columna seleccionada en el Grid_Productos
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Productos = (DataTable)Session["Dt_Producto_Servicio"];

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

    private void Actualizar_Dt_Productos() 
    {
        DataTable Dt_Productos = Session["Dt_Productos_OC"] as DataTable;
        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {

            //TextBox Txt_Precio_Unitario = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
            TextBox Txt_Marca = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Marca");
            TextBox Txt_Descripcion_Producto_Cot = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Descripcion_Producto_Cot");

            if (Txt_Marca != null)
            {
                Dt_Productos.Rows[i]["MARCA"] = Txt_Marca.Text;
            }
            if (Txt_Descripcion_Producto_Cot != null)
            {
                Dt_Productos.Rows[i]["PRODUCTO"] = Txt_Descripcion_Producto_Cot.Text;
            }
        }
        Session["Dt_Productos_OC"] = Dt_Productos;
    }

    public void Llenar_Grid_Productos()
    {
        Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos_OC"];
        Grid_Productos.DataSource = Dt_Productos;
        Grid_Productos.DataBind();

        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {

            //TextBox Txt_Precio_Unitario = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
            TextBox Txt_Marca = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Marca");
            TextBox Txt_Descripcion_Producto_Cot = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Descripcion_Producto_Cot");

            if (Txt_Marca != null)
            {
                Txt_Marca.Text = Dt_Productos.Rows[i]["Marca"].ToString().Trim();
            }
            if (Txt_Descripcion_Producto_Cot != null)
            {
                Txt_Descripcion_Producto_Cot.Text = Dt_Productos.Rows[i]["PRODUCTO"].ToString().Trim();
            }


        }//Fin del for

    }
    #endregion
    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos
    public bool Tiene_Marca_Descripcion_Texto_Vacio()
    {
        bool Resultado = false;
        TextBox T_Marca;
        TextBox T_Descripcion;
        foreach (GridViewRow Renglon_Grid in Grid_Productos.Rows)
        {
            T_Marca = ((System.Web.UI.WebControls.TextBox)Renglon_Grid.FindControl("Txt_Marca"));
            T_Descripcion = ((System.Web.UI.WebControls.TextBox)Renglon_Grid.FindControl("Txt_Descripcion_Producto_Cot"));
            if (T_Marca.Text.Trim().Length == 0 || T_Descripcion.Text.Trim().Length == 0)
            {
                Resultado = true;
                break;
            }
        }
        return Resultado;
    }

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        switch (Btn_Nuevo.ToolTip)
        {
            case "Cotizar":
                //if (Txt_Reserva.Text.Trim().Length > 0)
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Orden de Compra", "alert('No se puede modificar Orden de Compra con reserva asignada');", true);
                //}
                //else
                //{
                //    Configurar_Formulario("Nuevo");
                //}
                    Configurar_Formulario("Nuevo");
                break;
            case "Guardar":
                //Obtenemos el Id del proveedor
                DataTable Dt_Proveedor_Session = (DataTable)Cls_Sessiones.Datos_Proveedor;

                //Validamos que seleccione los datos obligatorios 
                if (Tiene_Marca_Descripcion_Texto_Vacio())
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "- Es necesario ingresar marca y descripción para todos los articulos solicitados</br>";
                }


                if (Div_Contenedor_Msj_Error.Visible == false)
                {
    
                    //Cargamos los datos de la clase de negocios
                    //Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
                    
                    //Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
                    //Clase_Negocio.P_Proveedor_ID = Session["Proveedor_ID"].ToString().Trim();
                    //Clase_Negocio.P_Dt_Productos = (DataTable)Session["Dt_Productos_OC"];
                    //Clase_Negocio.P_Subtotal_Cotizado = Txt_SubTotal_Cotizado_Requisicion.Text.Trim();
                    //Clase_Negocio.P_Total_Cotizado = Txt_Total_Cotizado_Requisicion.Text.Trim();
                    //Clase_Negocio.P_IEPS_Cotizado = Txt_IEPS_Cotizado.Text.Trim();
                    //Clase_Negocio.P_IVA_Cotizado = Txt_IVA_Cotizado.Text.Trim();
                    Actualizar_Dt_Productos();
                    Cls_Ope_Com_Orden_Compra_Negocio Negocio_OC = new Cls_Ope_Com_Orden_Compra_Negocio();
                    Negocio_OC.P_Condicion1 = Txt_Condiciones_Compra.Text.Trim();
                    long Orden_Compra = long.Parse(Txt_Folio.Text.Replace("OC-","").Trim());
                    Negocio_OC.P_No_Orden_Compra = Orden_Compra;
                    Negocio_OC.P_Dt_Detalles_Orden_Compra = Session["Dt_Productos_OC"] as DataTable;
                    int RowsAfected = Negocio_OC.Actualizar_Descripcion_Productos_OC();
                    Configurar_Formulario("Inicio");
           
                    //damos de alta 
                    //bool Operacion_Realizada = Clase_Negocio.Guardar_Cotizacion();

                    if (RowsAfected > 0)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Orden de Compra", "alert('Se modificó exitosamente la Orden de Compra');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Orden de Compra", "alert('No se guardaron los cambios en la Orden de Compra');", true);


                    Limpiar_Componentes();
                    ////Cargamos otra ves el grid requisiciones

                }
                break;
        }//fin del Switch

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del Boton Salir
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                //LIMPIAMOS VARIABLES DE SESSION
                Session["Dt_Requisiciones"] = null;

                Session["No_Requisicion"] = null;

                break;
            case "Cancelar":
                Configurar_Formulario("Inicio");
                //Limpiar_Componentes();
       

                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Limpiar_Componentes();
       
                break;
        }
    }



    #endregion


    protected void Chk_Fecha_Elaboracion_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_Elaboracion.Checked == true)
        {
            Btn_Busqueda_Fecha_Elaboracion_Ini.Enabled = true;
            Btn_Busqueda_Fecha_Elaboracion_Fin.Enabled = true;
        }
        else
        {
            Btn_Busqueda_Fecha_Elaboracion_Ini.Enabled = false;
            Btn_Busqueda_Fecha_Elaboracion_Fin.Enabled = false;
        }
    }

  

    protected void Chk_Vigencia_Propuesta_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Vigencia_Propuesta.Checked == true)
        {
            Btn_Busqueda_Vigencia_Propuesta_Ini.Enabled = true;
            Btn_Busqueda_Vigencia_Propuesta_Fin.Enabled = true;
        }
        else
        {
            Btn_Busqueda_Vigencia_Propuesta_Ini.Enabled = false;
            Btn_Busqueda_Vigencia_Propuesta_Fin.Enabled = false;
        }
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
       
        String No_Orden_Compra = Txt_Requisicion_Busqueda.Text.Trim();
        Cls_Ope_Com_Listado_Ordenes_Compra_Negocio Listado_Negocio = new Cls_Ope_Com_Listado_Ordenes_Compra_Negocio();

        DataTable Dt_Cabecera_OC = new DataTable();
        DataTable Dt_Detalles_OC = new DataTable();
        try
        {
            Listado_Negocio.P_No_Orden_Compra = No_Orden_Compra.Trim();
            // Consultar Cabecera de la Orden de compra
            Dt_Cabecera_OC = Listado_Negocio.Consulta_Cabecera_Orden_Compra();
            // Consultar los detalles de la Orden de compra
            Dt_Detalles_OC = Listado_Negocio.Consulta_Detalles_Orden_Compra();
            if (Dt_Detalles_OC != null && Dt_Detalles_OC.Rows.Count > 0)
            {
                String Text = Dt_Cabecera_OC.Rows[0]["CODIGO_PROGRAMATICO"].ToString().Replace("[", "");
                Text = Text.Replace("]", "");
                Txt_Codigo_Programatico.Text = Text;
                Txt_Folio.Text = Dt_Cabecera_OC.Rows[0]["FOLIO"].ToString();
                Txt_Proveedor.Text = Dt_Cabecera_OC.Rows[0]["PROVEEDOR"].ToString();
                Txt_Dependencia.Text = Dt_Cabecera_OC.Rows[0]["DIRECCION"].ToString();
                Txt_SubTotal_Cotizado_Requisicion.Text = Dt_Cabecera_OC.Rows[0]["SUBTOTAL"].ToString();
                Txt_IVA_Cotizado.Text = Dt_Cabecera_OC.Rows[0]["IVA"].ToString();
                Txt_IEPS_Cotizado.Text = Dt_Cabecera_OC.Rows[0]["IEPS"].ToString();
                Txt_Total_Cotizado_Requisicion.Text = Dt_Cabecera_OC.Rows[0]["TOTAL"].ToString();
                Txt_Condiciones_Compra.Text = Dt_Cabecera_OC.Rows[0]["CONDICION1"].ToString();
                Txt_Reserva.Text = Dt_Cabecera_OC.Rows[0][Ope_Com_Ordenes_Compra.Campo_No_Reserva].ToString();
                //Llenar grid
                Session["Dt_Productos_OC"] = Dt_Detalles_OC;
                Llenar_Grid_Productos();
                Configurar_Formulario("Inicio");
            }
            else
            {
                Configurar_Formulario("Busqueda");
                ScriptManager.RegisterStartupScript
                    (this, this.GetType(), "Orden Compra", "alert('No se encontró la Orden de Compra');", true);
            }
        }
        catch(Exception Ex)
        {
            Configurar_Formulario("Busqueda");
            ScriptManager.RegisterStartupScript
                (this, this.GetType(), "Orden Compra", "alert('No se encontró la Orden de Compra');", true);
        }
    }



    protected void Btn_Calcular_Precios_Cotizados_Click(object sender, EventArgs e)
    {
  

    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {

    }



}
