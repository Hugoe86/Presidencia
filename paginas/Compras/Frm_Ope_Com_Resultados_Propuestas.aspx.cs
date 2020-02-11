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
using Presidencia.Resultados_Propuesta.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Imprimir_Propuestas.Negocio;

public partial class paginas_Compras_Frm_Ope_Com_Resultados_Propuestas : System.Web.UI.Page
{

    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();
            ViewState["SortDirection"] = "ASC";
            Configurar_Formulario("Inicio");
            Llenar_Grid_Requisiciones(Clase_Negocio);
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
                Div_Detalle_Requisicion.Visible = false;
                Div_Grid_Requisiciones.Visible = true;
                Div_Busqueda.Visible = true;
                //Boton Modificar
                Btn_Nuevo.Visible = false;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "/paginas/imagenes/paginas/icono_salir.png";
                //
                Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = false;
                Grid_Productos.Enabled = false;
                //Cargamos las fechas al dia de hoy
                Txt_Busqueda_Fecha_Entrega_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Fecha_Entrega_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Fecha_Elaboracion_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Fecha_Elaboracion_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Vigencia_Propuesta_Ini.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Busqueda_Vigencia_Propuesta_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");

                Txt_Busqueda_Fecha_Entrega_Ini.Enabled = false;
                Txt_Busqueda_Fecha_Entrega_Fin.Enabled = false;
                Txt_Busqueda_Fecha_Elaboracion_Ini.Enabled = false;
                Txt_Busqueda_Fecha_Elaboracion_Fin.Enabled = false;
                Txt_Busqueda_Vigencia_Propuesta_Ini.Enabled = false;
                Txt_Busqueda_Vigencia_Propuesta_Fin.Enabled = false;

                //Deseleccionamos los check box
                Chk_Fecha_Elaboracion.Checked = false;
                Chk_Fecha_Entrega.Checked = false;
                Chk_Vigencia_Propuesta.Checked = false;
                //DEshabilitamos botones
                Btn_Busqueda_Fecha_Elaboracion_Ini.Enabled = false;
                Btn_Busqueda_Fecha_Elaboracion_Fin.Enabled = false;
                Btn_Busqueda_Fecha_Entrega_Fin.Enabled = false;
                Btn_Busqueda_Fecha_Entrega_Ini.Enabled = false;
                Btn_Busqueda_Vigencia_Propuesta_Ini.Enabled = false;
                Btn_Busqueda_Vigencia_Propuesta_Fin.Enabled = false;

                Btn_Imprimir.Visible = false;
                break;
            case "General":
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Cotizar";
                Div_Busqueda.Visible = false;
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Div_Grid_Requisiciones.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = false;
                //Deshabilitar controles 
                Txt_Reg_Padron_Prov.Enabled = false;
                Txt_Garantia.Enabled = false;
                Txt_Tiempo_Entrega.Enabled = false;
                Txt_Fecha_Elaboracio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Cmb_Estatus_Propuesta.Enabled = false;

                Btn_Imprimir.Visible = true;

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
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = true;
                //habilitar controles 
                Txt_Reg_Padron_Prov.Enabled = true;
                Txt_Garantia.Enabled = true;
                Txt_Tiempo_Entrega.Enabled = true;
                Txt_Fecha_Elaboracio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Cmb_Estatus_Propuesta.Enabled = true;

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
        Session["Proveedor_ID"] = null;
        Session["No_Requisicion"] = null;
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
        Txt_Reg_Padron_Prov.Text = "";
        Txt_Vigencia.Text = "";
        Txt_Fecha_Elaboracio.Text = "";
        Txt_Garantia.Text = "";
        Txt_Tiempo_Entrega.Text = "";

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
    ///NOMBRE DE LA FUNCIÓN:Llenar_Grid_Requisiciones
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Requisiciones
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Requisiciones(Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio)
    {
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
        Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();
        GridViewRow selectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
        int num_fila = Grid_Requisiciones.SelectedIndex;
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

        Clase_Negocio.P_Proveedor_ID = Dt_Requisiciones.Rows[0]["Proveedor_ID"].ToString().Trim();
        Session["Proveedor_ID"] = Clase_Negocio.P_Proveedor_ID.Trim();

        Clase_Negocio.P_No_Requisicion = Grid_Requisiciones.SelectedDataKey["No_Requisicion"].ToString();
        Session["No_Requisicion"] = Clase_Negocio.P_No_Requisicion;
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
        Txt_Estatus.Text = Dt_Detalle_Requisicion.Rows[0]["ESTATUS"].ToString().Trim();
        Txt_Justificacion.Text = Dt_Detalle_Requisicion.Rows[0]["JUSTIFICACION_COMPRA"].ToString().Trim();
        Txt_Especificacion.Text = Dt_Detalle_Requisicion.Rows[0]["ESPECIFICACION_PROD_SERV"].ToString().Trim();
        Session["TIPO_ARTICULO"] = Txt_Tipo_Articulo.Text.Trim();
        Session["Concepto_ID"] = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO_ID"].ToString().Trim();
        //LLenamos los text de la propuesta de Cotizacion
        DataTable Dt_Propuesta = Clase_Negocio.Consultar_Propuesta_Cotizacion();
        Txt_Reg_Padron_Prov.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Registro_Padron_Prov].ToString().Trim();
        Txt_Garantia.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Garantia].ToString().Trim();
        Txt_Fecha_Elaboracio.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion].ToString().Trim();
        Txt_Tiempo_Entrega.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega].ToString().Trim();
        Txt_Vigencia.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Vigencia_Propuesta].ToString().Trim();
        Cmb_Estatus_Propuesta.SelectedValue = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Estatus].ToString().Trim();
        //Asignamos los text
        Txt_Total_Cotizado_Requisicion.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado_Requisicion].ToString().Trim();
        Txt_SubTotal_Cotizado_Requisicion.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Subtotal_Cotizado_Requisicion].ToString().Trim();
        Txt_IEPS_Cotizado.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado_Req].ToString().Trim();
        Txt_IVA_Cotizado.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado_Req].ToString().Trim();



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
        //llenamos el grid de productos
        if (Dt_Productos.Rows.Count != 0)
        {
            Session["Dt_Productos"] = Dt_Productos;
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
                }
                if (Txt_Descripcion_Producto_Cot != null)
                {
                    Txt_Descripcion_Producto_Cot.Text = Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"].ToString().Trim();
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

            Grid_Requisiciones.DataSource = Dv_Requisiciones;
            Grid_Requisiciones.DataBind();
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

    public void Llenar_Grid_Productos()
    {
        Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        Grid_Productos.DataSource = Dt_Productos;
        Grid_Productos.DataBind();

        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
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
            }
            if (Txt_Descripcion_Producto_Cot != null)
            {
                Txt_Descripcion_Producto_Cot.Text = Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"].ToString().Trim();
            }


        }//Fin del for

    }
    #endregion
    #endregion

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        switch (Btn_Nuevo.ToolTip)
        {
            case "Cotizar":
                Configurar_Formulario("Nuevo");

                break;
            case "Guardar":
                //Obtenemos el Id del proveedor
                DataTable Dt_Proveedor_Session = (DataTable)Cls_Sessiones.Datos_Proveedor;
                //Cargamos los datos de la clase de negocios
                Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();
                Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
                Clase_Negocio.P_Proveedor_ID = Session["Proveedor_ID"].ToString().Trim();
                Clase_Negocio.P_Dt_Productos = (DataTable)Session["Dt_Productos"];
                Clase_Negocio.P_Subtotal_Cotizado = Txt_SubTotal_Cotizado_Requisicion.Text.Trim();
                Clase_Negocio.P_Total_Cotizado = Txt_Total_Cotizado_Requisicion.Text.Trim();
                Clase_Negocio.P_IEPS_Cotizado = Txt_IEPS_Cotizado.Text.Trim();
                Clase_Negocio.P_IVA_Cotizado = Txt_IVA_Cotizado.Text.Trim();
                //Validamos que seleccione los datos obligatorios 
                if (Txt_Reg_Padron_Prov.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = " Es obligatorio ingresar el Registro de Padron de Proveedores</br>";

                }
                if (Txt_Vigencia.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += " Es obligatorio ingresar la fecha de la vigencia de la propuesta</br>";

                }
                if (Txt_Garantia.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += " Es obligatorio ingresar la garantia </br>";

                }
                if (Txt_Tiempo_Entrega.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += " Es obligatorio ingresar el tiempo de entrega </br>";

                }

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Configurar_Formulario("Inicio");
                    Clase_Negocio.P_Vigencia_Propuesta = Formato_Fecha(Txt_Vigencia.Text);
                    Clase_Negocio.P_Fecha_Elaboracion = Txt_Fecha_Elaboracio.Text;
                    Clase_Negocio.P_Reg_Padron_Proveedor = Txt_Reg_Padron_Prov.Text;
                    Clase_Negocio.P_Garantia = Txt_Garantia.Text;
                    Clase_Negocio.P_Tiempo_Entrega = Txt_Tiempo_Entrega.Text;
                    Clase_Negocio.P_Estatus_Propuesta = Cmb_Estatus_Propuesta.SelectedValue;

                    //damos de alta 
                    bool Operacion_Realizada = Clase_Negocio.Guardar_Cotizacion();

                    if (Operacion_Realizada)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Propuesta Cotizacion", "alert('Se realizo la Cotizacion Exitosamente');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Propuesta Cotizacion", "alert('No se realizo la Cotizacion');", true);


                    Limpiar_Componentes();
                    //Cargamos otra ves el grid requisiciones
                    Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();
                    Llenar_Grid_Requisiciones(Clase_Negocio);
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
        Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();

        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Response.Redirect("/Inicio");
                //LIMPIAMOS VARIABLES DE SESSION
                Session["Dt_Requisiciones"] = null;

                Session["No_Requisicion"] = null;

                break;
            case "Cancelar":

                Configurar_Formulario("Inicio");
                Limpiar_Componentes();
                Llenar_Grid_Requisiciones(Clase_Negocio);

                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Limpiar_Componentes();
                Llenar_Grid_Requisiciones(Clase_Negocio);
                break;
        }
    }

    protected void Txt_Precio_Unitario_TextChanged(object sender, EventArgs e)
    {
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];

        for (int i = 0; i < Dt_Productos.Rows.Count; i++)
        {

            TextBox Temporal = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");


            if (Temporal != null)
            {
                Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"] = Temporal.Text;

            }//fin del IF
        }



    }

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

    protected void Chk_Fecha_Entrega_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_Elaboracion.Checked == true)
        {
            Btn_Busqueda_Fecha_Entrega_Ini.Enabled = true;
            Btn_Busqueda_Fecha_Entrega_Fin.Enabled = true;

        }
        else
        {
            Btn_Busqueda_Fecha_Entrega_Ini.Enabled = false;
            Btn_Busqueda_Fecha_Entrega_Fin.Enabled = false;

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
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //Verificamos las fechas
        Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();




        Llenar_Grid_Requisiciones(Clase_Negocio);


    }
    #endregion

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS: 1.-TextBox Fecha_Inicial 
    ///            2.-TextBox Fecha_Final
    ///            3.-Label Mensaje_Error
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public bool Verificar_Fecha(TextBox Fecha_Inicial, TextBox Fecha_Final, Label Mensaje_Error)
    {

        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        bool Fecha_Valida = true;

        if ((Fecha_Inicial.Text.Length == 11) && (Fecha_Final.Text.Length == 11))
        {
            //Convertimos el Texto de los TextBox fecha a dateTime
            Date1 = DateTime.Parse(Fecha_Inicial.Text);
            Date2 = DateTime.Parse(Fecha_Final.Text);
            //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
            if ((Date1 < Date2) | (Date1 == Date2))
            {
                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                //Licitaciones_Negocio.P_Fecha_Inicio = Formato_Fecha(Fecha_Inicial.Text);
                //Licitaciones_Negocio.P_Fecha_Fin = Formato_Fecha(Fecha_Final.Text);

            }
            else
            {
                Mensaje_Error.Text += "+ Fecha no valida <br />";
            }
        }
        else
        {
            Mensaje_Error.Text += "+ Fecha no valida <br />";
        }

        return Fecha_Valida;
    }

    protected void Btn_Imprimir_Cot_Click(object sender, ImageClickEventArgs e)
    {
        int Padron = Convert.ToInt32(Txt_Reg_Padron_Prov.Text);
        String Proveedor_ID = String.Format("{0:0000000000}", Padron);
        Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio Clase_Negocio = new Cls_Ope_Com_Imprimir_Propuestas_Cotizacion_Negocio();
        Clase_Negocio.P_Proveedor_ID = Proveedor_ID;
        Clase_Negocio.P_No_Requisicion = Txt_Folio.Text.Replace("RQ-", "");
        Clase_Negocio.P_Archivo_PDF = "Cotizacion_RQ-" + Clase_Negocio.P_No_Requisicion + "_" + Proveedor_ID + ".pdf";
        Clase_Negocio.P_Ruta_RPT = @Server.MapPath("../Rpt/Compras/Rpt_Ope_Com_Cotizacion_Proveedor.rpt");
        Clase_Negocio.P_Ruta_Exportacion = @Server.MapPath("../../Reporte/" + Clase_Negocio.P_Archivo_PDF);
        String Reporte = Clase_Negocio.Imprimir_Cotizacion();
        try
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window",
                "window.open('" + Reporte + "', 'Requisición','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }    



   
}
