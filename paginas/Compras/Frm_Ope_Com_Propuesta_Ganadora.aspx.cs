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
using Presidencia.Propuesta_Ganadora.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Administrar_Requisiciones.Negocios;


public partial class paginas_Compras_Frm_Ope_Com_Propuesta_Ganadora : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Configurar_Formulario("Inicio");
            Llenar_Grid_Requisiciones();
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
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        switch (Estatus)
        {
            case "Inicio":

                Div_Detalle_Requisicion.Visible = false;
                Div_Grid_Requisiciones.Visible = true;

                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = false;
                Grid_Productos.Enabled = false;
                Cmb_Estatus.Enabled = false;
                Cmb_Proveedores_Principal.Enabled = false;
                Txt_Comentarios.Enabled = false;
                break;
            case "Nuevo":

                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Modificar.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //
                Div_Grid_Requisiciones.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = true;
                Cmb_Estatus.Enabled = true;
                Cmb_Proveedores_Principal.Enabled = true;
                Txt_Comentarios.Enabled = true;
                break;
        }//fin del switch

    }


    public void Limpiar_Componentes()
    {

        Session["Concepto_ID"] = null;
        Session["Dt_Productos"] = null;
        Session["Dt_Requisiciones"] = null;
        Session["No_Requisicion"] = null;
        Session["TIPO_ARTICULO"] = null;
        Session["Concepto_ID"] = null;

        Txt_Dependencia.Text = "";
        Txt_Concepto.Text = "";
        Txt_Folio.Text = "";
        Txt_Concepto.Text = "";
        Txt_Fecha_Generacion.Text = "";
        Txt_Tipo.Text = "";
        Txt_Tipo_Articulo.Text = "";
        Chk_Verificacion.Checked = false;
        Txt_Justificacion.Text = "";
        Txt_Especificacion.Text = "";
        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        Txt_SubTotal_Cotizado_Requisicion.Text = "";
        Txt_Total_Cotizado_Requisicion.Text = "";
        Txt_IEPS_Cotizado.Text = "";
        Txt_IVA_Cotizado.Text = "";
        Txt_Comentarios.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus
    ///DESCRIPCIÓN: Metodo que carga el combo Cmb_Estatus
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("PROVEEDOR");
        Cmb_Estatus.Items.Add("COTIZADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
    }

    public void Calcular_Importe_Total()
    {
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        double Total_Cotizado = 0;
        double IVA_Cotizado = 0;
        double IEPS_Cotizado = 0;
        double Subtotal_Cotizado = 0;
        if (Dt_Productos.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                if (Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado].ToString().Trim() != String.Empty)
                {
                    IVA_Cotizado = IVA_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado].ToString().Trim());
                }
                else
                {
                    IVA_Cotizado = IVA_Cotizado + 0;
                }

                if (Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado].ToString().Trim() != String.Empty)
                {
                    IEPS_Cotizado = IEPS_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado].ToString().Trim());
                }
                else
                {
                    IEPS_Cotizado = IEPS_Cotizado + 0;
                }
                if (Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado].ToString().Trim() != String.Empty)
                {
                    Subtotal_Cotizado = Subtotal_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado].ToString().Trim());
                }
                else
                {
                    Subtotal_Cotizado = Subtotal_Cotizado + 0;
                }
                if (Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim() != String.Empty)
                {
                    Total_Cotizado = Total_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim());
                }
                else
                {
                    Total_Cotizado = Total_Cotizado + 0 ;
                }
            }
            Txt_Total_Cotizado_Requisicion.Text = Total_Cotizado.ToString();
            Txt_SubTotal_Cotizado_Requisicion.Text = Subtotal_Cotizado.ToString();
            Txt_IVA_Cotizado.Text = IVA_Cotizado.ToString();
            Txt_IEPS_Cotizado.Text = IEPS_Cotizado.ToString();




        }
        else
        {

            Txt_Total_Cotizado_Requisicion.Text = "0.0";
            Txt_SubTotal_Cotizado_Requisicion.Text = "0.0";
            Txt_IVA_Cotizado.Text = "0.0";
            Txt_IEPS_Cotizado.Text = "0.0";

        }
    }
    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    #region Grid_Requisicion
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
        Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio = new Cls_Ope_Com_Propuesta_Ganadora_Negocio();
        DataTable Dt_Requisiciones = Clase_Negocio.Consultar_Requisiciones();
        if (Dt_Requisiciones.Rows.Count != 0)
        {
            Session["Dt_Requisiciones"] = Dt_Requisiciones;
            Grid_Requisiciones.DataSource = Dt_Requisiciones;
            Grid_Requisiciones.DataBind();

        }
        else
        {
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();

        }
    }

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

        Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio = new Cls_Ope_Com_Propuesta_Ganadora_Negocio();

        GridViewRow Row = Grid_Requisiciones.SelectedRow;
        Clase_Negocio.P_No_Requisicion = Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        Session["No_Requisicion"] = Clase_Negocio.P_No_Requisicion;
        //llenamos combo estatus
        Llenar_Combo_Estatus();

        //Consultamos los detalles del producto seleccionado 
        DataTable Dt_Detalle_Requisicion = Clase_Negocio.Consultar_Detalle_Requisicion();
        //Mostramos el div de detalle y el grid de Requisiciones
        Div_Grid_Requisiciones.Visible = false;
        Div_Detalle_Requisicion.Visible = true;
        Btn_Salir.ToolTip = "Listado";
        //llenamos la informacion del detalle de la requisicion seleccionada
        Txt_Dependencia.Text = Dt_Detalle_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
        Txt_Folio.Text = Dt_Detalle_Requisicion.Rows[0]["FOLIO"].ToString().Trim();
        Txt_Concepto.Text = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO"].ToString().Trim();
        Txt_Fecha_Generacion.Text = Dt_Detalle_Requisicion.Rows[0]["FECHA_GENERACION"].ToString().Trim();
        Txt_Tipo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO"].ToString().Trim();
        Txt_Tipo_Articulo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO_ARTICULO"].ToString().Trim();
        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Dt_Detalle_Requisicion.Rows[0]["ESTATUS"].ToString().Trim()));
        Txt_Justificacion.Text = Dt_Detalle_Requisicion.Rows[0]["JUSTIFICACION_COMPRA"].ToString().Trim();
        Txt_Especificacion.Text = Dt_Detalle_Requisicion.Rows[0]["ESPECIFICACION_PROD_SERV"].ToString().Trim();
        Txt_IEPS_Cotizado.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_IEPS_Cotizado].ToString().Trim();
        Txt_IVA_Cotizado.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_IVA_Cotizado].ToString().Trim();
        Txt_IEPS_Cotizado.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_IEPS_Cotizado].ToString().Trim();
        Txt_Total_Cotizado_Requisicion.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total_Cotizado].ToString().Trim();
        Txt_SubTotal_Cotizado_Requisicion.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Subtotal_Cotizado].ToString().Trim();

        Txt_Compra_Especial.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Especial_Ramo_33].ToString().Trim();

        Session["TIPO_ARTICULO"] = Txt_Tipo_Articulo.Text.Trim();
        Session["Concepto_ID"] = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO_ID"].ToString().Trim();
        //VALIDAMOS EL CAMPO DE VERIFICAR CARACTERISTICAS, GARANTIA Y POLIZAS
        if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "NO" || Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == String.Empty)
        {
            Chk_Verificacion.Checked = false;
        }
        if (Dt_Detalle_Requisicion.Rows[0]["VERIFICACION_ENTREGA"].ToString().Trim() == "SI")
        {
            Chk_Verificacion.Checked = true;
        }
        //Consultamos los productos de esta requisicion
        Clase_Negocio.P_Tipo_Articulo = Session["TIPO_ARTICULO"].ToString().Trim();
        DataTable Dt_Productos = Clase_Negocio.Consultar_Productos_Servicios();
        if (Dt_Productos.Rows.Count != 0)
        {
            Session["Dt_Productos"] = Dt_Productos;
            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();
            Grid_Productos.Visible = true;
            Grid_Productos.Enabled = false;
            //Llenamos los Text Box con los datos del Dt_Productos
            //Consultamos los proveedores
            Clase_Negocio.P_Concepto_ID = Session["Concepto_ID"].ToString().Trim();
            DataTable Dt_Proveedores = Clase_Negocio.Consultar_Proveedores();
            //cargamos el combo principal de proveedores                        
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Proveedores_Principal, Dt_Proveedores, 1, 0);

            for (int i = 0; i < Grid_Productos.Rows.Count; i++)
            {

                DropDownList Cmb_Temporal_Proveedores = (DropDownList)Grid_Productos.Rows[i].FindControl("Cmb_Proveedor");
                Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Temporal_Proveedores, Dt_Proveedores);
                Cmb_Temporal_Proveedores.SelectedIndex = 0;
                //Llenamos los Combos de acuerdo al proveedor que tiene asignado  && Cmb_Temporal_Proveedores.SelectedItem.Text.Contains(Dt_Productos.Rows[i]["Proveedor_ID"].ToString().Trim())
                if (Dt_Productos.Rows[i]["Proveedor_ID"].ToString().Trim() != String.Empty)
                {
                    try
                    {
                        Cmb_Temporal_Proveedores.SelectedIndex = Cmb_Temporal_Proveedores.Items.IndexOf(Cmb_Temporal_Proveedores.Items.FindByText(Dt_Productos.Rows[i]["Nombre_Proveedor"].ToString().Trim()));
                    }
                    catch
                    {
                        //En caso de no encontrar el proveedor se selecciona vacio el combo
                        Cmb_Temporal_Proveedores.SelectedIndex = 0;
                    }
                }

            }//Fin del for
        }
        Div_Grid_Requisiciones.Visible = false;
        Div_Detalle_Requisicion.Visible = true;
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
    }

    #endregion Grid_Requisiciones

    #region Grid_Productos
    public void Llenar_Grid_Productos()
    {
        Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio = new Cls_Ope_Com_Propuesta_Ganadora_Negocio();
        DataTable Dt_Requisiciones = Clase_Negocio.Consultar_Requisiciones();
        if (Dt_Requisiciones.Rows.Count != 0)
        {
            Session["Dt_Requisiciones"] = Dt_Requisiciones;
            Grid_Requisiciones.DataSource = Dt_Requisiciones;
            Grid_Requisiciones.DataBind();

        }
        else
        {
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();

        }


    }

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

    }



    #endregion


    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

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
                Llenar_Grid_Requisiciones();
                Limpiar_Componentes();
                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Llenar_Grid_Requisiciones();
                Limpiar_Componentes();


                break;
        }
    }

    protected void Btn_Calcular_Precios_Cotizados_Click(object sender, EventArgs e)
    {
        DataTable Dt_Productos = new DataTable();
        Dt_Productos = (DataTable)Session["Dt_Productos"];
        Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio = new Cls_Ope_Com_Propuesta_Ganadora_Negocio();

        Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();

        for (int i = 0; i < Dt_Productos.Rows.Count; i++)
        {
            Dt_Productos = (DataTable)Session["Dt_Productos"];
            DropDownList Cmb_Temporal_Proveedores = (DropDownList)Grid_Productos.Rows[i].FindControl("Cmb_Proveedor");
            //Asignamos los valores que se cotizaron en la propuesta del cotizador
            Clase_Negocio.P_Producto_ID = Dt_Productos.Rows[i]["PROD_SERV_ID"].ToString().Trim();

            //Asignamos el proveedor seleccionado al grid
            if (Cmb_Temporal_Proveedores.SelectedIndex != 0)
            {
                Clase_Negocio.P_Proveedor_ID = Cmb_Temporal_Proveedores.SelectedValue;
                Dt_Productos.Rows[i]["Proveedor_ID"] = Cmb_Temporal_Proveedores.SelectedValue;
                Dt_Productos.Rows[i]["Nombre_Proveedor"] = Cmb_Temporal_Proveedores.SelectedItem;
                //Consutamos el producto seleccionado para pasar los valores calculados de lo ya cotizado del producto k corresponde a la posicion del for
                DataTable Dt_Producto_Propuesta = Clase_Negocio.Consultar_Productos_Propuesta();
                Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado].ToString().Trim();
                Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado].ToString().Trim();
                Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado].ToString().Trim();
                Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado].ToString().Trim();
                Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Sin_Imp_Cotizado].ToString().Trim();
                Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado].ToString().Trim();

                Dt_Productos.Rows[i]["NOMBRE_PROD_SERV_OC"] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Descripcion_Producto].ToString().Trim();
                Dt_Productos.Rows[i]["MARCA_OC"] = Dt_Producto_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Marca].ToString().Trim();
            }
            Session["Dt_Productos"] = Dt_Productos;

        }
        //llenamos nuevamente el grid de productos
        Grid_Productos.DataSource = Dt_Productos;
        Grid_Productos.DataBind();
        //Cargamos nuevamente el grid con los proveedores y el proveedor seleccionado 
        DataTable Dt_Proveedores = Clase_Negocio.Consultar_Proveedores();
        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {

            DropDownList Cmb_Temporal_Proveedores = (DropDownList)Grid_Productos.Rows[i].FindControl("Cmb_Proveedor");
            Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Temporal_Proveedores, Dt_Proveedores);
            Cmb_Temporal_Proveedores.SelectedIndex = 0;
            //Llenamos los Combos de acuerdo al proveedor que tiene asignado  && Cmb_Temporal_Proveedores.SelectedItem.Text.Contains(Dt_Productos.Rows[i]["Proveedor_ID"].ToString().Trim())
            if (Dt_Productos.Rows[i]["Proveedor_ID"].ToString().Trim() != String.Empty)
            {
                try
                {
                    Cmb_Temporal_Proveedores.SelectedIndex = Cmb_Temporal_Proveedores.Items.IndexOf(Cmb_Temporal_Proveedores.Items.FindByText(Dt_Productos.Rows[i]["Nombre_Proveedor"].ToString().Trim()));
                }
                catch
                {
                    //En caso de no encontrar el proveedor se selecciona vacio el combo
                    Cmb_Temporal_Proveedores.SelectedIndex = 0;
                }
            }

        }//Fin del for


        //CAlculamos el total cotizado
        Calcular_Importe_Total();
    }
    protected void Cmb_Proveedores_Principal_SelectedIndexChanged(object sender, EventArgs e)
    {
        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {

            DropDownList Cmb_Temporal_Proveedores = (DropDownList)Grid_Productos.Rows[i].FindControl("Cmb_Proveedor");
            try
            {
                Cmb_Temporal_Proveedores.SelectedValue = Cmb_Proveedores_Principal.SelectedValue;
            }
            catch
            {
                //En caso de no encontrar el proveedor se selecciona vacio el combo
                Cmb_Temporal_Proveedores.SelectedIndex = 0;
            }           

        }//Fin del for
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Cls_Ope_Com_Propuesta_Ganadora_Negocio Clase_Negocio = new Cls_Ope_Com_Propuesta_Ganadora_Negocio();
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                if (Txt_Folio.Text != String.Empty)
                {
                    Configurar_Formulario("Nuevo");
                    Cmb_Estatus.Enabled = true;
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario selecciona una Requisicion";
                }
                break;
            case "Guardar":

                Configurar_Formulario("Inicio");
                //Realizamos las validaciones
                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario seleccionar un Estatus";
                }//fin del if
                if (Txt_Total_Cotizado_Requisicion.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "</br>Es necesario calcular los precios";
                }
                //Validamos en caso de seleccionar el estatus de Cotizada
                if (Cmb_Estatus.SelectedValue.Trim() == "COTIZADA")
                {
                    DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
                    //Validamos que todos los productos sean cotizados
                    for (int i = 0; i < Dt_Productos.Rows.Count; i++)
                    {
                        if (Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"].ToString().Trim() == String.Empty)
                        {
                            Div_Contenedor_Msj_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Es necesario asignar precio cotizado a todos los productos";
                            break;
                        }
                    }
                }
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    //Cargamos datos al objeto de clase negocio
                    Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
                    Clase_Negocio.P_Dt_Productos = (DataTable)Session["Dt_Productos"];
                    Clase_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;

                    if (Txt_IEPS_Cotizado.Text.Trim() != String.Empty)
                        Clase_Negocio.P_IEPS_Cotizado = Txt_IEPS_Cotizado.Text;
                    else
                        Clase_Negocio.P_IEPS_Cotizado = "0";
                    if (Txt_IVA_Cotizado.Text.Trim() != String.Empty)
                        Clase_Negocio.P_IVA_Cotizado = Txt_IVA_Cotizado.Text;
                    else
                        Clase_Negocio.P_IVA_Cotizado = "0";
                    if (Txt_Total_Cotizado_Requisicion.Text.Trim() != String.Empty)
                        Clase_Negocio.P_Total_Cotizado = Txt_Total_Cotizado_Requisicion.Text;
                    else
                        Clase_Negocio.P_Total_Cotizado = "0";
                    if (Txt_SubTotal_Cotizado_Requisicion.Text.Trim() != String.Empty)
                        Clase_Negocio.P_Subtotal_Cotizado = Txt_SubTotal_Cotizado_Requisicion.Text;
                    else
                        Clase_Negocio.P_Subtotal_Cotizado = "0";

                    bool Operacion_Realizada = false;
                    Operacion_Realizada = Clase_Negocio.Agregar_Cotizaciones();


                    if (Operacion_Realizada == true)
                    {
                        Operacion_Realizada = Clase_Negocio.Modificar_Requisicion();
                        //Agregamos el comentario 
                        Cls_Ope_Com_Administrar_Requisiciones_Negocio Requisicion_Negocio = new Cls_Ope_Com_Administrar_Requisiciones_Negocio();
                        if (Txt_Comentarios.Text.Trim() != String.Empty)
                        {
                            Requisicion_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Requisicion_Negocio.P_Comentario = Txt_Comentarios.Text.Trim();
                            Requisicion_Negocio.P_Estatus = Clase_Negocio.P_Estatus;
                            Requisicion_Negocio.P_Requisicion_ID = Clase_Negocio.P_No_Requisicion;
                            Requisicion_Negocio.Alta_Observaciones();
                        }  


                        if (Operacion_Realizada == true)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cotizacion Ganadora", "alert('Se Guardo la Cotizacion Ganadora Exitosamente');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cotizacion Ganadora", "alert('No se Guardo la Cotizacion Ganadora');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cotizacion Ganadora", "alert('No se Guardo la Cotizacion Ganadora');", true);
                    }

                }

                Limpiar_Componentes();
                Llenar_Grid_Requisiciones();
                break;

        }//fin del switch
    }

    #endregion

}
