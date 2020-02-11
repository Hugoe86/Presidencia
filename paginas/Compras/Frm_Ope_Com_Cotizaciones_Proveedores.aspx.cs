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
using Presidencia.Cotizaciones_Proveedores.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Com_Cotizaciones_Proveedores : System.Web.UI.Page
{


    ///*******************************************************************************
    /// VARIABLES
    ///*******************************************************************************
    #region Variables
    Cls_Ope_Com_Cotizaciones_Proveedores_Negocio Datos_Cotizacion;
    #endregion

    ///*******************************************************************************
    /// REGION PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        

    }
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Init(object sender, EventArgs e)
    {
        Datos_Cotizacion = new Cls_Ope_Com_Cotizaciones_Proveedores_Negocio();
        //consultamos si es un cotizador o jefe de Comite de compras 
        bool Cotizador = Datos_Cotizacion.Empleado_Cotizador();
        //llenamos por primera ves el combo de estatus 
        if (Cotizador==true)
        {
            Estatus_Formulario("Inicial");
            Llenar_Combo_Estatus();
            Llenar_Grid_Cotizaciones();
            //Llenamos por primera ves el grid de giros

        }
        else
        {
            Estatus_Formulario("Inicial");
            Div_Listado_Cotizaciones.Visible = false;
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No tiene permisos para ver esta información";
        }
    }
    #endregion Page_Load

    ///*******************************************************************************
    /// REGION METODOS
    ///*******************************************************************************
    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estatus_Formulario
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estatus_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicial":
                //Manejo de los Div
                Div_Busqueda.Visible = true;
                Div_Listado_Cotizaciones.Visible = true;
                Div_Datos_Cotizaciones.Visible = false;
                Div_Requisiciones.Visible = false;
                Div_Consolidaciones.Visible = false;
                Div_Grid_Proveedores.Visible = false;
                //Boton Modificar
                Configuracion_Acceso("Frm_Ope_Com_Cotizaciones_Proveedores.aspx");
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                break;
            case "General":
                //Manejo de los Div
                Div_Busqueda.Visible = false;
                Div_Listado_Cotizaciones.Visible = false;
                Div_Datos_Cotizaciones.Visible = true;
                Div_Requisiciones.Visible = false;
                Div_Consolidaciones.Visible = false;
                Div_Grid_Proveedores.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                Configuracion_Acceso("Frm_Ope_Com_Cotizaciones_Proveedores.aspx");
                break;
            case "Modificar":
                //Manejo de los Div
                Div_Busqueda.Visible = false;
                Div_Listado_Cotizaciones.Visible = false;
                Div_Datos_Cotizaciones.Visible = true;
                
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                
                break;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Componentes
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Componentes(bool Estatus)
    {
        Txt_Folio.Enabled = false;
        Txt_Fecha.Enabled = false;
        Txt_Total.Enabled = false;
        Txt_Condiciones.Enabled = false;
        Txt_Tipo.Enabled = false;
        Chk_Listado_Almacen.Enabled = false;
        Cmb_Estatus.Enabled = Estatus;
        //Segunda Pestaña
        Cmb_Concepto.Enabled = Estatus;
        Cmb_Proveedores.Enabled = false;
        Grid_Concepto_Proveedores.Enabled = Estatus;
        Btn_Add_Proveedor.Enabled = Estatus;
        //Tercer Pestaña
        Txt_Nombre_Producto.Enabled = false;
        Txt_Nombre_Proveedor.Enabled = false;
        Txt_Costo_Producto.Enabled = Estatus;
        Btn_Agregar.Enabled = Estatus;
        Grid_Productos.Enabled = Estatus;
        Txt_Total_Cotizado.Enabled = false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 11/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public void Limpiar_Formulario()
    {
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Txt_Tipo.Text = "";
        Txt_Condiciones.Text = "";
        Txt_Total.Text = "";
        Txt_Total_Cotizado.Text = "0";
        Cmb_Estatus.SelectedIndex = 0;
        //Primer Pestaña
        //limpiamos grid de Consolidaciones
        Grid_Consolidaciones.DataBind();
        //limpiamos Grid de Requisiciones
        Grid_Requisiciones.DataBind();
        //Segunda Pestaña
        Cmb_Concepto.Items.Clear();
        Cmb_Proveedores.Items.Clear();
        Grid_Concepto_Proveedores.DataBind();
        //Tercer Pestaña
        Txt_Nombre_Producto.Text = "";
        Txt_Nombre_Proveedor.Text = "";
        Txt_Costo_Producto.Text = "";
        //limpiamos Grid_Productos
        Grid_Productos.DataBind();
        Txt_Total_Cotizado.Text = "";
        //Variables de session 
        Session["Dt_Cotizaciones"] = null;
        Session["No_Cotizacion"] = null;
        Session["Dt_Productos"] = null;
        Session["Dt_Proveedores"] = null;
        Session["Ope_Com_Req_Producto_ID"] = null;
        Session["Producto_Seleccionado"] = null;
        Session["Lista_Requisiciones"] = null;
        //LIMPIAMOS LA CLASE DE NEGOCIOS 
        Datos_Cotizacion = new Cls_Ope_Com_Cotizaciones_Proveedores_Negocio();
        
    }

    public void Cargar_Datos_Negocio()
    {
        Datos_Cotizacion.P_No_Cotizacion = Session["No_Cotizacion"].ToString();
        Datos_Cotizacion.P_Tipo = Txt_Tipo.Text.Trim();
        Datos_Cotizacion.P_Estatus = Cmb_Estatus.SelectedValue;
        Datos_Cotizacion.P_Dt_Productos = (DataTable)Session["Dt_Productos"];
        Datos_Cotizacion.P_Lista_Requisiciones = Session["Lista_Requisiciones"].ToString();
        Datos_Cotizacion.P_Total_Cotizado = Txt_Total_Cotizado.Text.Trim();
        if (Chk_Listado_Almacen.Checked == true)
            Datos_Cotizacion.P_Listado_Almacen = "SI";
        else
            Datos_Cotizacion.P_Listado_Almacen = "NO";
    }

    public void Llenar_Combo_Estatus()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("ASIGNADA");
        Cmb_Estatus.Items.Add("TERMINADA");
        Cmb_Estatus.Items.Add("CANCELADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
    }

    public void Agregar_Proveedor(String Concepto_ID, String Proveedor_ID)
    {
        DataRow[] Filas;
        DataTable Dt_Proveedores = (DataTable)Session["Dt_Proveedores"];
        Filas = Dt_Proveedores.Select("Concepto_ID='" + Concepto_ID + "'");
        if (Filas.Length > 0)
        {
            //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
            //al usuario que elemento ha agregar ya existe en la tabla de grupos.
            Lbl_Mensaje_Error.Text += "+ No se puede agregar el Giro ya que esta ya se ha agregado <br/>";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        else
        {
            DataRow Fila_Nueva = Dt_Proveedores.NewRow();
            Fila_Nueva["Concepto_ID"] = Cmb_Concepto.SelectedValue.Trim();
            Fila_Nueva["Clave_Concepto"] = Cmb_Concepto.SelectedItem.Text.Trim().Substring(0, 4);
            Fila_Nueva["Descripcion_Concepto"] = Cmb_Concepto.SelectedItem.Text.Trim().Substring(5);
            Fila_Nueva["Nombre_Proveedor"] = Cmb_Proveedores.SelectedItem.ToString().Trim();
            Fila_Nueva["Proveedor_ID"] = Cmb_Proveedores.SelectedValue.ToString().Trim();
            Dt_Proveedores.Rows.Add(Fila_Nueva);
            Dt_Proveedores.AcceptChanges();
            Session["Dt_Proveedores"] = Dt_Proveedores;
            //Llenamos el grid de giro_Proveedores
            Grid_Concepto_Proveedores.DataSource = Dt_Proveedores;
            Grid_Concepto_Proveedores.DataBind();
            Div_Grid_Proveedores.Visible = true;
            //Agregamos el Proveedor seleccionado al grid_Productos
            DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];

            //recorremos el dt_productos en busca de productos con el giro seleccionado para asignarlo
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                if (Dt_Productos.Rows[i]["Concepto_ID"].ToString().Trim() == Concepto_ID.Trim())
                {
                    Dt_Productos.Rows[i]["Nombre_Proveedor"] = Fila_Nueva["Nombre_Proveedor"].ToString().Trim();
                    Dt_Productos.Rows[i]["Proveedor_ID"] = Fila_Nueva["Proveedor_ID"].ToString().Trim();
                    Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["Precio_U_Con_Imp_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["IVA_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["IEPS_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["Total_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["Subtotal_Cotizado"] = 0;
                }
            }
            //Cargamos nuevamente el grid de productos 
            Session["Dt_Productos"] = Dt_Productos;
            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();

        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Calcular_Impuestos
    ///DESCRIPCIÓN: Metodo que calcula el impuesto del producto seleccionado por el usuario
    ///PARAMETROS:  1.- DataTable _TableProductos Contiene los productos de la licitacion
    ///             2.- int Num_Fila numero de fila del registro al cual se le calculara el impuesto 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 18/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Calcular_Impuestos(DataTable _TableProductos, int Num_Fila)
    {
        double Impuesto_porcentaje_1 = 0;
        double Impuesto_porcentaje_2 = 0;
        double IEPS = 0;
        double IVA = 0;
        double Importe_Total = 0;
        double Cantidad = 0;
        double Precio_Unitario = 0;
        double Mayor = 0;
        double Menor = 0;
        double Precio_C_Impuesto = 0;
        //aSIGNAMOS A LA CLASE DE NEGOCIOS EL ID DEL PRODUCTO DEL CUAL QUEREMOS CONSULTAR SUS IMPUESTOS
        Datos_Cotizacion.P_Producto_ID = _TableProductos.Rows[Num_Fila]["Prod_Serv_ID"].ToString();
        //cONSULTAMOS LOS IMPUESTOS DEL PRODUCTO 
        DataTable Dt_Impuestos_Producto = Datos_Cotizacion.Consultar_Impuesto_Producto();
        if (Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString() != "")
        {
            Impuesto_porcentaje_1 = double.Parse(Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString());

        }
        if (Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_2"].ToString() != "")
        {
            Impuesto_porcentaje_2 = double.Parse(Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_2"].ToString());

        }
        //Asignamos valores a Cantidad y precio unitario
        Cantidad = double.Parse(_TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Cantidad].ToString());
        Precio_Unitario = double.Parse(_TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado].ToString());
        //Calculas los Impuestos en caso de tener 2 para obtener el importe total del producto
        if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 != 0)
        {
            Mayor = Math.Max(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
            Menor = Math.Min(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
            //Calculamos el Precio con Impuesto
            Precio_C_Impuesto = (Precio_Unitario * Mayor) / 100;
            Precio_C_Impuesto = (Precio_C_Impuesto * Menor) / 100;
            Precio_C_Impuesto = Precio_C_Impuesto + Precio_Unitario;
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado] = Precio_C_Impuesto;
            //Calculamos el IEPS 
            IEPS = ((Precio_Unitario * Cantidad) * Mayor) / 100;
            //Calculamos el IVA
            IVA = ((Precio_Unitario * Cantidad) * Menor) / 100;
            //Primero obtenemos el Impuesto IEPS
            Importe_Total = ((Precio_Unitario * Cantidad) * Mayor) / 100;
            //Despues a lo obtenido del impuesto ieps le sumamos el impuesto Iva
            Importe_Total = (Importe_Total * Menor) / 100;
            //Sumamos el impuesto al importe total 
            Importe_Total = Importe_Total + (Precio_Unitario * Cantidad);
            //Le asignamos el valor a la columna de importe
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Total_Cotizado] = Importe_Total;
        }
        //En caso de tener un solo impuesto 
        if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 == 0)
        {
            //Calculamos el costo con impuesto 
            Precio_C_Impuesto = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado] = Precio_C_Impuesto + Precio_Unitario;
            //Calculamos el importe total
            Importe_Total = ((Precio_Unitario * Cantidad) * Impuesto_porcentaje_1) / 100;
            Importe_Total = Importe_Total + (Precio_Unitario * Cantidad);
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Total_Cotizado] = Importe_Total;
            //Calculamos el monto de IVA o IEPS dependiendo cual le corresponda
            if (Dt_Impuestos_Producto.Rows[0]["TIPO_IMPUESTO_1"].ToString() == "IVA")
            {
                //Asignamos el Monto IVA 
                IVA = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
                IEPS = 0;
            }
            if (Dt_Impuestos_Producto.Rows[0]["TIPO_IMPUESTO_1"].ToString() == "IEPS")
            {
                //Asignamos el moento IEPS
                IEPS = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
                IVA = 0;
            }
        }
        if (Impuesto_porcentaje_1 == 0 && Impuesto_porcentaje_2 == 0)
        {
            //en caso de no tener impuestos el producto
            Importe_Total = (Precio_Unitario * Cantidad);
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Total_Cotizado] = Importe_Total;
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado] = Precio_Unitario;
            IVA = 0;
            IEPS = 0;
        }
        //cARGAMOS LOS DATOS
        _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_IVA_Cotizado] = IVA;
        _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_IEPS_Cotizado] = IEPS;
        //Calculamos el Subtotal, (es el total sin impuestos)
        _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Subtota_Cotizado] = Cantidad * Precio_Unitario;

        return _TableProductos;
    }//fin metodo Calcular_Impuestos

    public void Verificar_Precios_Asignamos()
    {
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        if (Dt_Productos.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                if (Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado].ToString() == "0")
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "+ Es necesario cotizar todos los productos </br>";
                    break;
                }//fin del if
            }//fin del for 
        }//fin if

    }
    #endregion Fin_Metodos


    ///*******************************************************************************
    /// REGION GRID
    ///*******************************************************************************
    #region Grid

    public void Llenar_Grid_Cotizaciones()
    {
        DataTable Dt_Cotizaciones = Datos_Cotizacion.Consultar_Cotizaciones();
        if (Dt_Cotizaciones.Rows.Count != 0)
        {
            Grid_Cotizaciones.DataSource = Dt_Cotizaciones;
            Grid_Cotizaciones.DataBind();
            Session["Dt_Cotizaciones"] = Dt_Cotizaciones;
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "+ No tiene cotizaciones pendientes</br>";
            Grid_Cotizaciones.DataSource = new DataTable();
            Grid_Cotizaciones.DataBind();
        }
    }

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
    protected void Grid_Cotizaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Cotizaciones.PageIndex = e.NewPageIndex;
        Grid_Cotizaciones.DataSource = Session["Dt_Cotizaciones"];
        Grid_Cotizaciones.DataBind();

    }
     ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Comite_Compras_SelectedIndexChanged
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cotizaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Estatus_Formulario("General");
        GridViewRow Row = Grid_Cotizaciones.SelectedRow;
        Datos_Cotizacion.P_No_Cotizacion = Grid_Cotizaciones.SelectedDataKey["No_Cotizacion"].ToString();
        //Consultamos los datos de lal proceso de comite de compras seleccionado 
        DataTable Dt_Cotizacion = Datos_Cotizacion.Consultar_Cotizaciones();
        Session["No_Cotizacion"] = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_No_Cotizacion].ToString();
        Session["Lista_Requisiciones"] = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Lista_Requisiciones].ToString();
        Txt_Folio.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Folio].ToString();
        Txt_Tipo.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Tipo].ToString();
        Datos_Cotizacion.P_Tipo = Txt_Tipo.Text.Trim();
        Txt_Fecha.Text = Dt_Cotizacion.Rows[0]["FECHA"].ToString().Trim();
        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Estatus].ToString().Trim()));
        Txt_Condiciones.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Condiciones].ToString();
        Txt_Total.Text = Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Total].ToString();
        //Seleccionamos el check de listado almacen en caso de ser una requisicion de listado almacen
        if (Dt_Cotizacion.Rows[0][Ope_Com_Cotizaciones.Campo_Listado_Almacen].ToString().Trim() == "SI")
            Chk_Listado_Almacen.Checked = true;
        else
            Chk_Listado_Almacen.Checked = false;
        //llenamos el giro del cotizador para k agregue a los proveedores correspondientes con el giro
        DataTable Data_Table = Datos_Cotizacion.Consultar_Concepto_Cotizador();
        Cmb_Concepto.Items.Clear();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Concepto, Data_Table);
        Cmb_Concepto.SelectedIndex = 0;
        //Llenamos los grid de requisiciones y cotizaciones en caso de existir. 
        Data_Table = Datos_Cotizacion.Consultar_Detalle_Requisicion();
        if (Data_Table.Rows.Count != 0)
        {
            Div_Requisiciones.Visible = true;
            Grid_Requisiciones.DataSource = Data_Table;
            Grid_Requisiciones.DataBind();

        }
        Data_Table = Datos_Cotizacion.Consultar_Detalle_Consolidacion();
        if (Data_Table.Rows.Count != 0)
        {
            Div_Consolidaciones.Visible = true;
            Grid_Consolidaciones.DataSource = Data_Table;
            Grid_Cotizaciones.DataBind();

        }
        //Consultamos los productos pertenecientes a la cotizacion, y al empleado cotizador logueado
        Data_Table = Datos_Cotizacion.Consulta_Productos();
        if (Data_Table.Rows.Count != 0)
        {
            Div_Grid_Productos.Visible = true;
            Grid_Productos.DataSource = Data_Table;
            Grid_Productos.DataBind();
            Session["Dt_Productos"] = Data_Table;
            //Calculamos el total cotizado solo de los productos que le pertenecen al cotizador
            Txt_Total_Cotizado.Text = Calcular_Importe(Data_Table).ToString();

        }
        ////Consultamos los proveedores asignados 
        Data_Table = Datos_Cotizacion.Consultar_Proveedores_Asignados();
        if (Data_Table.Rows.Count != 0)
        {
            Div_Grid_Proveedores.Visible = true;
            Grid_Concepto_Proveedores.DataSource = Data_Table;
            Grid_Concepto_Proveedores.DataBind();
            Session["Dt_Proveedores"] = Data_Table;
        }
        Habilitar_Componentes(false);
       


        
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Calcular_Importe
    ///DESCRIPCIÓN: Metodo que calcula el total cotizado pero solo de los productos mostrados en pantalla, esto con el objetivo
    ///de que el cotizador tengo una vision de lo que se a cotizado pero solo sus productos. 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 31/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public double Calcular_Importe(DataTable Dt_Productos)
    {
        double Total_Cotizado = 0;
        for (int i = 0; i < Dt_Productos.Rows.Count; i++)
        {
            if (Dt_Productos.Rows[i]["TOTAL_COTIZADO"].ToString().Trim() == String.Empty)
            {
                Total_Cotizado += 0;
            }
            else
            {
                Total_Cotizado += double.Parse(Dt_Productos.Rows[i]["TOTAL_COTIZADO"].ToString());
            }
        }
        return Total_Cotizado;
    }


    protected void Grid_Productos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Session["Ope_Com_Req_Producto_ID"] = Grid_Productos.SelectedDataKey["Ope_Com_Req_Producto_ID"].ToString();
        //Cargamos el producto seleccionado en las cajas
        if (Grid_Productos.SelectedIndex > (-1))
        {
            GridViewRow selectedRow = Grid_Productos.Rows[Grid_Productos.SelectedIndex];
            Txt_Nombre_Producto.Text = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();
            Txt_Nombre_Proveedor.Text = HttpUtility.HtmlDecode(selectedRow.Cells[13].Text).ToString();
            //Pasamos a un datatable la variable de session que contiene el listado de productos
            DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
            //Guardamos el numero de la fila seleccionada ya que se ocupara mas adelane
            int registro = Grid_Productos.SelectedIndex;
            Session["Producto_Seleccionado"] = registro;
        }
    }

    protected void Grid_Concepto_Proveedores_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow Renglon;
        GridViewRow selectedRow = Grid_Concepto_Proveedores.Rows[Grid_Concepto_Proveedores.SelectedIndex];
        String Giro_ID = Grid_Concepto_Proveedores.SelectedDataKey["Giro_ID"].ToString();
        Renglones = ((DataTable)Session["Dt_Proveedores"]).Select("GIRO_ID ='" + Giro_ID.Trim() + "'");

        if (Renglones.Length > 0)
        {
            Renglon = Renglones[0];
            DataTable Tabla = (DataTable)Session["Dt_Proveedores"];
            Tabla.Rows.Remove(Renglon);
            //Asignamos el nuevo valor al datatable de Session
            Session["Dt_Proveedores"] = Tabla;
            Grid_Concepto_Proveedores.SelectedIndex = (-1);
            Grid_Concepto_Proveedores.DataSource = Tabla;
            Grid_Concepto_Proveedores.DataBind();
            //
            //Agregamos el Proveedor seleccionado al grid_Productos
            DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];

            //recorremos el dt_productos en busca de productos con el giro seleccionado para asignarlo
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                if (Dt_Productos.Rows[i]["Giro_ID"].ToString().Trim() == Giro_ID.Trim())
                {
                    Dt_Productos.Rows[i]["Nombre_Proveedor"] = null;
                    Dt_Productos.Rows[i]["Proveedor_ID"] = null;
                    //Regresamos a null los valores cotizados para el proveedore que se elimino
                    Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["Precio_U_Con_Imp_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["IVA_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["IEPS_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["Total_Cotizado"] = 0;
                    Dt_Productos.Rows[i]["Subtotal_Cotizado"] = 0;

                }
            }
            //Cargamos nuevamente el grid de productos 
            Session["Dt_Productos"] = Dt_Productos;
            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();
            if (Grid_Concepto_Proveedores.Rows.Count == 0)
            {
                Div_Grid_Proveedores.Visible = false;
            }

        }
    }

    #endregion Fin_Grid

    ///*******************************************************************************
    /// REGION EVENTOS
    ///*******************************************************************************
    #region Eventos

    #region Barra de Botones

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch(Btn_Salir.ToolTip)
        {
            case "Listado":
                Estatus_Formulario("Inicial");
                Limpiar_Formulario();
                Habilitar_Componentes(false);
                break;
            case "Inicio":
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                Limpiar_Formulario();    
                break;
            case "Cancelar":
                Estatus_Formulario("Inicial");
                Limpiar_Formulario();
                Habilitar_Componentes(false);
                break;
        }
    }

    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {

        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                if (Txt_Folio.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "+ Es necesario seleccionar una cotizacion</br>";
                }
                else
                {
                    Estatus_Formulario("Modificar");
                    Habilitar_Componentes(true);
                }
                break;
            case "Actualizar":
                if (Cmb_Estatus.SelectedIndex != 0)
                {
                    try
                    {
                        //Validamos que estatus tiene
                        switch (Cmb_Estatus.SelectedValue.Trim())
                        {
                            case "ASIGNADA":
                                if (Grid_Concepto_Proveedores.Rows.Count == 0)
                                {
                                    //se obliga al usuario asignar todos los proveedores
                                    Div_Contenedor_Msj_Error.Visible = true;
                                    Lbl_Mensaje_Error.Text = "+ Es necesario asignar proveedores a las requisiciones</br>";
                                }
                                if (Div_Contenedor_Msj_Error.Visible == false)
                                {
                                    Cargar_Datos_Negocio();
                                    //LLamamos el metodo de mofificar El proceso de COmite de Compras
                                    Datos_Cotizacion.Modificar_Cotizacion();
                                    //Modificacmos los detalles de los productos
                                    Datos_Cotizacion.Modificar_Detalle_Productos();
                                    Estatus_Formulario("Inicial");
                                    Limpiar_Formulario();
                                    Habilitar_Componentes(false);
                                    Llenar_Grid_Cotizaciones();
                                }

                                break;
                            case "TERMINADA":
                                Verificar_Precios_Asignamos();
                                if (Grid_Concepto_Proveedores.Rows.Count != (Cmb_Concepto.Items.Count - 1))
                                {
                                    //se obliga al usuario asignar todos los proveedores
                                    Div_Contenedor_Msj_Error.Visible = true;
                                    Lbl_Mensaje_Error.Text = "+ Es necesario asignar todos los proveedores a las requisiciones</br>";
                                }

                                if (Div_Contenedor_Msj_Error.Visible == false)
                                {
                                    Cargar_Datos_Negocio();
                                    //LLamamos el metodo de mofificar El proceso de COmite de Compras
                                    Datos_Cotizacion.Modificar_Cotizacion();
                                    
                                    Estatus_Formulario ("Inicial");
                                    Limpiar_Formulario();
                                    Habilitar_Componentes(false);
                                    Llenar_Grid_Cotizaciones();                                  
                                    
                                }
                                break;
                            case "CANCELADA":
                                Cargar_Datos_Negocio();
                                //LLamamos el metodo de mofificar El proceso de COmite de Compras
                                Datos_Cotizacion.Modificar_Cotizacion();
                                Estatus_Formulario("Inicial");
                                Limpiar_Formulario();
                                Habilitar_Componentes(false);
                                Llenar_Grid_Cotizaciones();     
                                break;

                        }
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores Cotizacion", "alert('Se Modifico la Cotizacion');", true);

                    }
                    catch (Exception Ex)
                    {
                        throw new Exception("Error al modificar el registro del Proceso de Comite de Compras :Error[" + Ex.Message + "]");
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Proveedores Cotizacion", "alert('Fallo la Modificacion de la Cotizacion');", true);
                    }
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "+ Es necesario seleccionar el estatus</br>";
                }

                break;
        }
    }
  
    #endregion

    protected void Btn_Add_Proveedor_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Cmb_Concepto.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ Es necesario seleccionar un Giro";
        }
        if (Cmb_Proveedores.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es necesario seleccionar un Proveedor";
        }
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            if (Session["Dt_Proveedores"] != null)
            {
                Agregar_Proveedor(Cmb_Concepto.SelectedValue.ToString(), Cmb_Proveedores.SelectedValue.ToString());               
            }
            else
            {
                DataTable Dt_Proveedores = new DataTable();
                Dt_Proveedores.Columns.Add("Concepto_ID", typeof(System.String));
                Dt_Proveedores.Columns.Add("Clave_Concepto", typeof(System.String));
                Dt_Proveedores.Columns.Add("Descripcion_Concepto", typeof(System.String));
                Dt_Proveedores.Columns.Add("Nombre_Proveedor", typeof(System.String));
                Dt_Proveedores.Columns.Add("Proveedor_ID", typeof(System.String));
                Session["Dt_Proveedores"] = Dt_Proveedores;
                Agregar_Proveedor(Cmb_Concepto.SelectedValue.ToString(), Cmb_Proveedores.SelectedValue.ToString());
            }
        }//fin del if Div_Contenedor_Msj_Error.Visible = false
    
    }

    protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Nombre_Producto.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es necesario seleccionar un producto </br>";
        }
        if (Txt_Nombre_Proveedor.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Es necesario asignar antes un proveedor</br>";
        }

        if (Txt_Costo_Producto.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Ingrese el costo sin impuesto</br>";
        }

        
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        int num_fila = int.Parse(Session["Producto_Seleccionado"].ToString());
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            Dt_Productos.Rows[num_fila][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado] = Txt_Costo_Producto.Text.Trim();
            //CALCULAMOS LOS IMPUESTOS DEL PRODUCTO
            Dt_Productos = Calcular_Impuestos(Dt_Productos, num_fila);
            //CARGAMOS NUEVAMENTE EL GRID_pRODUCTOS
            Session["Dt_Productos"] = Dt_Productos;
            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();
            Txt_Total_Cotizado.Text = Calcular_Importe(Dt_Productos).ToString();
        }
       
        //limpiamos las cajas seleccionadas 
        Txt_Nombre_Producto.Text = "";
        Txt_Nombre_Proveedor.Text = "";
        Txt_Costo_Producto.Text = "";
    }

    protected void Cmb_Concepto_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        //Hacemos la consulta de acuerdo al giro seleccionado 
        if (Cmb_Concepto.SelectedIndex != 0)
        {
            //Asignamos el giro a la clase de negocio
            Datos_Cotizacion.P_Giro_ID = Cmb_Concepto.SelectedValue;
            //Llenamos el combo de proveedores que tienen este giro 
            Cmb_Proveedores.Enabled = true;
            Cmb_Proveedores.Items.Clear();
            DataTable Dt_Proveedores = Datos_Cotizacion.Consulta_Proveedores();
            Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Proveedores, Dt_Proveedores);
        }
        else
        {
            Cmb_Proveedores.Enabled = false;
            Cmb_Proveedores.Items.Clear();
        }

    }

    #endregion FinEventos

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
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Buscar);

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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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

}
