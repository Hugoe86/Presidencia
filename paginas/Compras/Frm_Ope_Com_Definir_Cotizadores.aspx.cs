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
using Presidencia.Definir_Cotizadores.Negocio;
using System.Collections.Generic;
using Presidencia.Sessiones;
using Presidencia.Constantes;

public partial class paginas_Compras_Frm_Ope_Com_Definir_Cotizadores : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:  Page_Load
    ///DESCRIPCIÓN: Metodo del page load de la pagina
    ///PARAMETROS: 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {

        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Configurar_Formulario("Inicio");
            Llenar_Grid_Requisiciones();
            Llenar_Combo_Partidas(Cmb_Partidas);
            Llenar_Combo_Cotizadores(Cmb_Cotizador);
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
            case "Inicio":

                Div_Cotizadores.Visible = false;


                //Boton Modificar
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Asignar Cotizadores";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/sias_new.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Btn_Reasignar.Text = "Reasignar";
                Btn_Reasignar.Enabled = true;

                Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = false;
                Div_Cotizadores.Visible = true;
                Cmb_Partidas.Enabled = false;
                Cmb_Cotizador.Enabled = false;
               // Btn_Asignar_Cotizador.Enabled = false;
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
                
                Btn_Reasignar.Enabled = false;

                Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = false;
                Div_Cotizadores.Visible = true;
                //Btn_Asignar_Cotizador.Enabled = true;
                Cmb_Partidas.Enabled = true;
                Cmb_Cotizador.Enabled = true;
                break;

        }//fin del switch

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Conceptos
    ///DESCRIPCIÓN: Metodo que Consulta los conceptos de las requisiciones en el Grid y los asigna al Combo de Conceptos
    ///PARAMETROS: 1.- DropDownList Cmb_Combo: combo dentro de la pagina a llenar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Partidas(DropDownList Cmb_Combo)
    {
        Cmb_Combo.Items.Clear();
        Cls_Ope_Com_Definir_Cotizadores_Negocio Negocios = new Cls_Ope_Com_Definir_Cotizadores_Negocio();
        Negocios.P_Tipo_Articulo = "PRODUCTO";
        if (Btn_Reasignar.Text == "Reasignar")
        {
            //asignamos el valor de si es reasignacion o no para ver cual consulta se realiza.
            Negocios.P_Reasignar = false;
        }
        if (Btn_Reasignar.Text == "Asignar")
        {
            //asignamos el valor de si es reasignacion o no para ver cual consulta se realiza.
            Negocios.P_Reasignar = true;
        }

        DataTable Data_Table = Negocios.Consultar_Partidas_Especificas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Combo, Data_Table);
        Cmb_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cotizadores
    ///DESCRIPCIÓN: Metodo que Consulta los cotizadores dados de alta en la tabla CAT_COM_COTIZADORES
    ///PARAMETROS: 1.- DropDownList Cmb_Combo: combo dentro de la pagina a llenar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Cotizadores(DropDownList Cmb_Combo)
    {
        Cmb_Combo.Items.Clear();
        Cls_Ope_Com_Definir_Cotizadores_Negocio Negocios = new Cls_Ope_Com_Definir_Cotizadores_Negocio();
        DataTable Data_Table = Negocios.Consultar_Cotizadores();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Combo, Data_Table);
        Cmb_Combo.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Asignar_Cotizadores_Checks
    ///DESCRIPCIÓN: Metodo que asigna el cotizador seleccionado con el check en el Grid_Requisiciones
    ///PARAMETROS: 1.- GridView MyGrid: GRid que contiene el Check Seleccionado
    ///            2.- String Nombre_Check: Nombre del check a evaluar, dentro del Grid_Requisiciones
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Asignar_Cotizadores_Checks(GridView MyGrid, String Nombre_Check)
    {
        //Obtenemos el numero de Checkbox seleccionados
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            bool isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked;

            if (isChecked)
            {
                //Asignamos el valor seleccionado a 
                Dt_Requisiciones.Rows[i]["COTIZADOR_ID"] = Cmb_Cotizador.SelectedValue;
                Dt_Requisiciones.Rows[i]["NOMBRE_COTIZADOR"] = Cmb_Cotizador.SelectedItem;
            }
        }//fin del for i

        //llenamos el grid nuevamente 
        Session["Dt_Requisiciones"] = Dt_Requisiciones;
        Grid_Requisiciones.DataSource = Dt_Requisiciones;
        Grid_Requisiciones.DataBind();

        
    }


    public void Limpiar_Sessiones()
    {
        Session["Dt_Requisiciones"] = null;
        Session["No_Requisicion"] = null;
        Session["Dt_Producto_Servicio"] = null;
        Session["Dt_Servicios"] = null;

    }

    public void Limpiar_Detalle_Requisicion()
    {
        Txt_Dependencia.Text = "";
        Txt_Folio.Text = "";
        Txt_Concepto.Text = "";
        Txt_Fecha_Generacion.Text = "";
        Txt_Tipo.Text = "";
        Txt_Tipo_Articulo.Text = "";
        Txt_Estatus.Text = "";
        Chk_Verificacion.Checked = false;
        Txt_Justificacion.Text = "";
        Txt_Especificacion.Text = "";
        Txt_Total.Text = "";
        Txt_IEPS.Text = "";
        Txt_IVA.Text = "";
        Txt_Subtotal.Text = "";
        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        Seleccionar_Cheks(Grid_Requisiciones, "Chk_Requisicion", false);
        //Btn_Asignar_Cotizador.Enabled = true;
    }

   
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
        
        //creamos la instancia de la clase de negocios
        Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocio = new Cls_Ope_Com_Definir_Cotizadores_Negocio();
        Btn_Salir.ToolTip = "Listado";
        //Consultamos los datos del producto seleccionado
        GridViewRow Row = Grid_Requisiciones.SelectedRow;
        Clase_Negocio.P_No_Requisicion = Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        Session["No_Requisicion"] = Clase_Negocio.P_No_Requisicion;
        //Consultamos los detalles del producto seleccionado 
        DataTable Dt_Detalle_Requisicion = Clase_Negocio.Consultar_Detalle_Requisicion();
        //Mostramos el div de detalle y el grid de Requisiciones
        Div_Cotizadores.Visible = false;
        Div_Detalle_Requisicion.Visible = true;
        Grid_Requisiciones.Visible = false;
        //Btn_Asignar_Cotizador.Enabled = false;
        //llenamos la informacion del detalle de la requisicion seleccionada
        Txt_Dependencia.Text = Dt_Detalle_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
        Txt_Folio.Text = Dt_Detalle_Requisicion.Rows[0]["FOLIO"].ToString().Trim();
        Txt_Concepto.Text = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO"].ToString().Trim();
        Txt_Fecha_Generacion.Text = Dt_Detalle_Requisicion.Rows[0]["FECHA_GENERACION"].ToString().Trim();
        Txt_Tipo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO"].ToString().Trim();
        Txt_Tipo_Articulo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO_ARTICULO"].ToString().Trim();
        Txt_Estatus.Text = Dt_Detalle_Requisicion.Rows[0]["ESTATUS"].ToString().Trim();
        Txt_Justificacion.Text = Dt_Detalle_Requisicion.Rows[0]["JUSTIFICACION_COMPRA"].ToString().Trim();
        Txt_Especificacion.Text = Dt_Detalle_Requisicion.Rows[0]["ESPECIFICACION_PROD_SERV"].ToString().Trim();
        Txt_Subtotal.Text = Dt_Detalle_Requisicion.Rows[0]["SUBTOTAL"].ToString().Trim();
        Txt_IEPS.Text = Dt_Detalle_Requisicion.Rows[0]["IEPS"].ToString().Trim();
        Txt_IVA.Text = Dt_Detalle_Requisicion.Rows[0]["IVA"].ToString().Trim();
        Txt_Total.Text = Dt_Detalle_Requisicion.Rows[0]["TOTAL"].ToString().Trim();

        //VALIDAMOS EL CAMPO DE VERIFICAR CARACTERISTICAS, GARANTIA Y POLIZAS
        if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "NO" || Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == String.Empty)
        {
            Chk_Verificacion.Checked = false;
        }
        if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "SI")
        {
            Chk_Verificacion.Checked = true;
        }

        //CONSULTAMOS LOS PRODUCTOS DE LA REQUISICION SELECCIONADA
        Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
        Clase_Negocio.P_Tipo_Articulo = Txt_Tipo_Articulo.Text;

        DataTable Dt_Producto_Servicio = Clase_Negocio.Consultar_Productos_Servicios();
        
        if (Dt_Producto_Servicio.Rows.Count != 0)
        {
            Session["Dt_Producto_Servicio"] = Dt_Producto_Servicio;
            Grid_Productos.DataSource = Dt_Producto_Servicio;
            Grid_Productos.DataBind();
        }
        else
        {
            Grid_Productos.DataSource = new DataTable();
            Grid_Productos.DataBind();
        }
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

            Grid_Requisiciones.DataSource = Dv_Requisiciones;
            Grid_Requisiciones.DataBind();
            //Guardamos el cambio dentro de la variable de session de Dt_Requisiciones
            Session["Dt_Requisiciones"] = (DataTable)Dv_Requisiciones.Table;
            Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
            
        }

    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:Llenar_Grid_Requisiciones
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Requisiciones
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Requisiciones()
    {
        Cls_Ope_Com_Definir_Cotizadores_Negocio Definir_Cotizadores = new Cls_Ope_Com_Definir_Cotizadores_Negocio();
        DataTable Dt_Requisiciones = Definir_Cotizadores.Consultar_Requisiciones();
        if (Dt_Requisiciones.Rows.Count != 0)
        {
            Session["Dt_Requisiciones"] = Dt_Requisiciones;
            Grid_Requisiciones.DataSource = Dt_Requisiciones;
            Grid_Requisiciones.DataBind();
            Grid_Requisiciones.Visible = true;
        }
        else
        {
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = " No se encontraron Requisiciones con productos a Asignar Cotizador";
        }
    }
    #endregion

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
    #endregion Grid

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Cotizadores_Click
    ///DESCRIPCIÓN: Evento del Boton agregar_cotizadores que guarda los cambios 
    ///realizados al asignar cotizadores a las requisiciones
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Nuevo.ToolTip)
        {
            case "Asignar Cotizadores":
                if(Grid_Requisiciones.Rows.Count != 0)
                    Configurar_Formulario("Nuevo");

                break;
            case "Guardar":
            
            Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocios = new Cls_Ope_Com_Definir_Cotizadores_Negocio();

            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                //cargamos los datos
                Clase_Negocios.P_Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
                 bool Operacion_Realizada = false;
                if (Btn_Reasignar.Text == "Reasignar")
                {
                    Operacion_Realizada = Clase_Negocios.Alta_Cotizadores_Asignados();
                }
                else {
                    Operacion_Realizada = Clase_Negocios.Reasignar_Cotizadores();
                }
                if (Operacion_Realizada)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Definir Cotizadores", "alert('Se asignaron satisfactoriamente los cotizadores');", true);
                if(!Operacion_Realizada)
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Definir Cotizadores", "alert('No se pudieron asignar los cotizadores');", true);
            }
            Configurar_Formulario("Inicio");
            //limpiamos variables de SEssion
            Limpiar_Sessiones();
            Llenar_Grid_Requisiciones();
            Llenar_Combo_Partidas(Cmb_Partidas);
            Llenar_Combo_Cotizadores(Cmb_Cotizador);
            break;
        }
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
                Seleccionar_Cheks(Grid_Requisiciones, "Chk_Requisicion", false);
                Limpiar_Sessiones();
                Llenar_Grid_Requisiciones();
                Llenar_Combo_Partidas(Cmb_Partidas);
                Llenar_Combo_Cotizadores(Cmb_Cotizador);
                Cmb_Cotizador.SelectedIndex = 0;
                break;
            case "Listado":
                Configurar_Formulario("Inicio");

                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Asignar_Cotizador_Click
    ///DESCRIPCIÓN: Evento del Boton Asignar_Cotizadores que agrega el cotizador
    ///seleccionado ya sea en base a un concepto o con apoyo del Check dentro del Grid
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Asignar_Cotizador_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Cmb_Partidas.SelectedIndex == 0)
        {
            if (!Checks_Seleccionados(Grid_Requisiciones, "Chk_Requisicion"))
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario Seleccionar un Concepto";
            }
        }
        if (Cmb_Cotizador.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario Seleccionar un Cotizador";

        }
        if(Div_Contenedor_Msj_Error.Visible ==false)
        {
            //en caso de seleccionar algun check dentro del grid realizara lo siguiente
            if (Checks_Seleccionados(Grid_Requisiciones, "Chk_Requisicion"))
            {
                //Asignamos el cotizador solo a las requisiciones seleccionadas en el check box dentro del Grid_Requisiciones
                Asignar_Cotizadores_Checks(Grid_Requisiciones, "Chk_Requisicion");

            }// fin del if Checks_Seleccionados
            else
            {
                DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
                for (int i = 0; i < Dt_Requisiciones.Rows.Count; i++)
                {
                    if (Dt_Requisiciones.Rows[i]["PARTIDA_ID"].ToString().Trim() == Cmb_Partidas.SelectedValue)
                    {
                        Dt_Requisiciones.Rows[i]["COTIZADOR_ID"] = Cmb_Cotizador.SelectedValue;
                        Dt_Requisiciones.Rows[i]["NOMBRE_COTIZADOR"] = Cmb_Cotizador.SelectedItem;
                    }
                }
                //llenamos con los nuevos valores el grid de productos
                Session["Dt_Requisiciones"] = Dt_Requisiciones;
                Grid_Requisiciones.DataSource = Dt_Requisiciones;
                Grid_Requisiciones.DataBind();
            }//Fin del Else Checks_Seleccionados

        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Click
    ///DESCRIPCIÓN: Evento del Boton Cerrar que cierra el div con los detalles de la requisicion seleccionada
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Detalle_Requisicion.Visible = false;
        Div_Cotizadores.Visible = true;
        Grid_Requisiciones.Visible = true;
        Grid_Requisiciones.DataSource = (DataTable)Session["Dt_Requisiciones"];
        Grid_Requisiciones.DataBind();

        //limpiamos las cajas del detalle requisiciones
        Limpiar_Detalle_Requisicion();
        

    }

    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Checks_Seleccionados
    ///DESCRIPCIÓN:Metodo que refresa un bool indicndo si se selecciono el check dentro del Grid
    ///PARAMETROS:1.- GridView MyGrid: Grid que contiene el Check
    ///           2.-  String Nombre_Check: Nombre del Check que esta dentro del Grid
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public bool Checks_Seleccionados(GridView MyGrid, String Nombre_Check)
    {
        
        bool isChecked= false;
        //Obtenemos el numero de Checkbox seleccionados
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            isChecked = ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked;

            if (isChecked)
            {
                //si se encuentra alguno seleccionado se rompe 
                break;
            }
        }//fin del for i

        return isChecked;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Seleccionar_Cheks
    ///DESCRIPCIÓN: Metodo que selecciona los checkbox dentro de un Grid view de acuerdo al parametro estado
    ///PARAMETROS:  1.- GridView MyGrid grid que se va a recorrer
    ///             2.- String Nombre_check nombre del checkbox dentro del grid
    ///             3.- bool Estado estado al que se desea cambiar los check box del grid
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 22/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Seleccionar_Cheks(GridView MyGrid, String Nombre_Check, bool Estado)
    {

        //Seleccionamos todos los checks
        for (int i = 0; i < MyGrid.Rows.Count; i++)
        {
            GridViewRow row = MyGrid.Rows[i];
            ((System.Web.UI.WebControls.CheckBox)row.FindControl(Nombre_Check)).Checked = Estado;
        }//fin del for i


    }//fin de Seleccionar Cheks

    protected void Btn_Reasignar_Click(object sender, EventArgs e)
    {
        Cls_Ope_Com_Definir_Cotizadores_Negocio Clase_Negocio = new Cls_Ope_Com_Definir_Cotizadores_Negocio();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch(Btn_Reasignar.Text)
        {
            case "Reasignar":
                Div_Detalle_Requisicion.Visible = false;
                Limpiar_Detalle_Requisicion();
               

                Clase_Negocio.P_Reasignar = true;
                DataTable Dt_Requisiciones=Clase_Negocio.Consultar_Requisiciones();
                if (Dt_Requisiciones.Rows.Count != 0)
                {
                    Session["Dt_Requisiciones"] = Dt_Requisiciones;
                    Grid_Requisiciones.DataSource = Dt_Requisiciones;
                    Grid_Requisiciones.DataBind();
                    Grid_Requisiciones.Visible = true;
                }
                else
                {
                    Grid_Requisiciones.DataSource = new DataTable();
                    Grid_Requisiciones.DataBind();
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = " No se encontraron Requisiciones con productos a Asignar Cotizador";
                }
                Btn_Reasignar.Text = "Asignar";
                //Llenamos el Combo de Partidas
                Llenar_Combo_Partidas(Cmb_Partidas);
            break;

            case "Asignar":
                Configurar_Formulario("Inicio");
                Llenar_Grid_Requisiciones();
            break;
    }//fIN DEL SWIRCH
    }


    #endregion


    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
   
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion
    
    protected void Cmb_Cotizador_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Cmb_Partidas.SelectedIndex == 0)
        {
            if (!Checks_Seleccionados(Grid_Requisiciones, "Chk_Requisicion"))
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario Seleccionar un Concepto";
                Cmb_Cotizador.SelectedIndex = 0;
            }
        }
        if (Cmb_Cotizador.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario Seleccionar un Cotizador";
            Cmb_Cotizador.SelectedIndex = 0;

        }
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //en caso de seleccionar algun check dentro del grid realizara lo siguiente
            if (Checks_Seleccionados(Grid_Requisiciones, "Chk_Requisicion"))
            {
                //Asignamos el cotizador solo a las requisiciones seleccionadas en el check box dentro del Grid_Requisiciones
                Asignar_Cotizadores_Checks(Grid_Requisiciones, "Chk_Requisicion");

            }// fin del if Checks_Seleccionados
            else
            {
                DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
                for (int i = 0; i < Dt_Requisiciones.Rows.Count; i++)
                {
                    if (Dt_Requisiciones.Rows[i]["PARTIDA_ID"].ToString().Trim() == Cmb_Partidas.SelectedValue)
                    {
                        Dt_Requisiciones.Rows[i]["COTIZADOR_ID"] = Cmb_Cotizador.SelectedValue;
                        Dt_Requisiciones.Rows[i]["NOMBRE_COTIZADOR"] = Cmb_Cotizador.SelectedItem;
                    }
                }
                //llenamos con los nuevos valores el grid de productos
                Session["Dt_Requisiciones"] = Dt_Requisiciones;
                Grid_Requisiciones.DataSource = Dt_Requisiciones;
                Grid_Requisiciones.DataBind();
            }//Fin del Else Checks_Seleccionados
            Cmb_Cotizador.SelectedIndex = 0;
        }
    }


}//fin del class