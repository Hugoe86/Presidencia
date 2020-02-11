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
using Presidencia.Cotizacion_Manual.Negocio;
using Presidencia.Constantes;


public partial class paginas_Compras_Frm_Ope_Com_Cotizacion_Manual : System.Web.UI.Page
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

        switch (Estatus)
        {
            case "Inicio":

                Div_Detalle_Requisicion.Visible = false;
                Div_Grid_Requisiciones.Visible = true;

                //Boton Modificar
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Nuevo.Enabled = true;
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
                Cmb_Estatus.Enabled = true;

                break;
        }//fin del switch

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores
    ///DESCRIPCIÓN: Metodo que Consulta los proveedores dados de alta en la tabla CAT_COM_PROVEEDORES
    ///PARAMETROS: 1.- DropDownList Cmb_Combo: combo dentro de la pagina a llenar 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/JULIO/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Proveedores(DropDownList Cmb_Combo)
    {
        Cmb_Combo.Items.Clear();
        Cls_Ope_Com_Cotizacion_Manual_Negocio Negocios = new Cls_Ope_Com_Cotizacion_Manual_Negocio();
        Negocios.P_Concepto_ID = Session["Concepto_ID"].ToString().Trim();
        DataTable Data_Table = Negocios.Consultar_Proveedores();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Combo, Data_Table);
        Cmb_Combo.SelectedIndex = 0;
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
    public DataTable Calcular_Impuestos_Producto(DataTable _TableProductos, int Num_Fila)
    {
        Cls_Ope_Com_Cotizacion_Manual_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_Negocio();
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
        Clase_Negocio.P_Producto_ID = _TableProductos.Rows[Num_Fila]["Prod_Serv_ID"].ToString();
        //cONSULTAMOS LOS IMPUESTOS DEL PRODUCTO 
        Clase_Negocio.P_Tipo_Articulo =Txt_Tipo_Articulo.Text.Trim();

        DataTable Dt_Impuestos_Producto = Clase_Negocio.Consultar_Impuesto_Producto();
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
            IEPS = IEPS * Cantidad;
            //Calculamos el IVA
            IVA = ((Precio_Unitario * Cantidad) * Menor) / 100;
            //calculamos el IVA cotizado total multiplicando por la cantidad de piezas solicitadas
            IVA = IVA * Cantidad;
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
    public DataTable Calcular_Impuestos_Servicio(DataTable _TableProductos, int Num_Fila)
    {
        Cls_Ope_Com_Cotizacion_Manual_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_Negocio();
        double Impuesto_porcentaje_1 = 0;
        double IEPS = 0;
        double IVA = 0;
        double Importe_Total = 0;
        double Cantidad = 0;
        double Precio_Unitario = 0;
        double Precio_C_Impuesto = 0;
        //aSIGNAMOS A LA CLASE DE NEGOCIOS EL ID DEL PRODUCTO DEL CUAL QUEREMOS CONSULTAR SUS IMPUESTOS
        Clase_Negocio.P_Producto_ID = _TableProductos.Rows[Num_Fila]["Prod_Serv_ID"].ToString();
        //cONSULTAMOS LOS IMPUESTOS DEL PRODUCTO 
        Clase_Negocio.P_Tipo_Articulo = Txt_Tipo_Articulo.Text.Trim();

        DataTable Dt_Impuestos_Producto = Clase_Negocio.Consultar_Impuesto_Producto();
        if (Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE"].ToString() != "")
        {
            Impuesto_porcentaje_1 = double.Parse(Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE"].ToString());

        }
        
        //Asignamos valores a Cantidad y precio unitario
        Cantidad = double.Parse(_TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Cantidad].ToString());
        Precio_Unitario = double.Parse(_TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado].ToString());
        
        //En caso de tener un solo impuesto 
        if (Impuesto_porcentaje_1 != 0 )
        {
            //Calculamos el costo con impuesto 
            Precio_C_Impuesto = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Precio_U_Con_Imp_Cotizado] = Precio_C_Impuesto + Precio_Unitario;
            //Calculamos el importe total
            Importe_Total = ((Precio_Unitario * Cantidad) * Impuesto_porcentaje_1) / 100;
            Importe_Total = Importe_Total + (Precio_Unitario * Cantidad);
            _TableProductos.Rows[Num_Fila][Ope_Com_Req_Producto.Campo_Total_Cotizado] = Importe_Total;
            //Calculamos el monto de IVA o IEPS dependiendo cual le corresponda
            if (Dt_Impuestos_Producto.Rows[0]["TIPO_IMPUESTO"].ToString() == "IVA")
            {
                //Asignamos el Monto IVA 
                IVA = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
                IEPS = 0;
            }
            
        }
        if (Impuesto_porcentaje_1 == 0)
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
                IVA_Cotizado = IVA_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IVA_Cotizado].ToString().Trim());
                IEPS_Cotizado = IEPS_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_IEPS_Cotizado].ToString().Trim());
                Subtotal_Cotizado = Subtotal_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Subtota_Cotizado].ToString().Trim());
                Total_Cotizado = Total_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim());
            }
            Txt_Total_Cotizado_Requisicion.Text = Total_Cotizado.ToString();
            Txt_SubTotal_Cotizado_Requisicion.Text = Subtotal_Cotizado.ToString();
            Txt_IVA_Cotizado.Text = IVA_Cotizado.ToString();
            Txt_IEPS_Cotizado.Text = IEPS_Cotizado.ToString();


            

        }
        else
        {
            Txt_Total_Cotizado_Requisicion.Text = "0.0";

        }
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
         Cmb_Estatus.Items.Add("FILTRADA");
         Cmb_Estatus.Items.Add("COTIZADA");
         Cmb_Estatus.Items[0].Value = "0";
         Cmb_Estatus.Items[0].Selected = true;
     }


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
    public void Llenar_Grid_Requisiciones()
    {
        Cls_Ope_Com_Cotizacion_Manual_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_Negocio();
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
        Cls_Ope_Com_Cotizacion_Manual_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_Negocio();

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
        //llenamos el grid de productos
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
            for (int i = 0; i < Grid_Productos.Rows.Count; i++)
            {

                TextBox Temporal = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
                DropDownList Cmb_Temporal_Proveedores = (DropDownList)Grid_Productos.Rows[i].FindControl("Cmb_Proveedor");
                Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Temporal_Proveedores, Dt_Proveedores);
                Cmb_Temporal_Proveedores.SelectedIndex = 0;

                if (Temporal != null)
                {
                    Temporal.Text = Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"].ToString().Trim();
                }//fin del IF

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

    public  void Llenar_Grid_Productos()
    {
        Cls_Ope_Com_Cotizacion_Manual_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_Negocio();
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        Grid_Productos.DataSource = Dt_Productos;
        Grid_Productos.DataBind();
        //Consultamos los proveedores
        //Consultamos los proveedores
        Clase_Negocio.P_Concepto_ID = Session["Concepto_ID"].ToString().Trim();
        DataTable Dt_Proveedores = Clase_Negocio.Consultar_Proveedores();

        for (int i = 0; i < Grid_Productos.Rows.Count; i++)
        {

            TextBox Temporal = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
            DropDownList Cmb_Temporal_Proveedores = (DropDownList)Grid_Productos.Rows[i].FindControl("Cmb_Proveedor");
            Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Temporal_Proveedores, Dt_Proveedores);
            Cmb_Temporal_Proveedores.SelectedIndex = 0;

            if (Temporal != null)
            {
                Temporal.Text = Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"].ToString().Trim();
            }//fin del IF

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

        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];

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
        Cls_Ope_Com_Cotizacion_Manual_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_Negocio();
        switch (Btn_Nuevo.ToolTip)
        {
            case "Nuevo":
                Configurar_Formulario("Nuevo");
                Cmb_Estatus.Enabled = true;
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
                    Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "Es necesario seleccionar un Estatus";
                }

                //Validamos en caso de seleccionar el estatus de Cotizada
                if (Cmb_Estatus.SelectedValue.Trim() == "COTIZADA")
                {
                    DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
                    //Validamos que todos los productos sean cotizados
                    for (int i=0; i<Dt_Productos.Rows.Count; i++)
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
                        if (Operacion_Realizada == true)
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cotizacion Manual", "alert('Se realizo la Cotizacion Manual Exitosamente');", true);
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cotizacion Manual", "alert('No se realizo la Cotizacion Manual');", true);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Cotizacion Manual", "alert('No se realizo la Cotizacion Manual');", true);
                    }

                }

                Limpiar_Componentes();



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
        for (int i = 0; i < Dt_Productos.Rows.Count; i++)
        {
            Dt_Productos = (DataTable)Session["Dt_Productos"];
            TextBox Temporal = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
            DropDownList Cmb_Temporal_Proveedores = (DropDownList)Grid_Productos.Rows[i].FindControl("Cmb_Proveedor");

            if (Temporal != null)
            {
                if(Temporal.Text.Trim() == String.Empty)
                    Temporal.Text = "0";
                Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"] = Temporal.Text;

            }//fin del IF

            //CALCULAMOS LOS IMPUESTOS DEL PRODUCTO
            if (Txt_Tipo_Articulo.Text.Trim() == "PRODUCTO")
            {
                Dt_Productos = Calcular_Impuestos_Producto(Dt_Productos, i);
            }
            if (Txt_Tipo_Articulo.Text.Trim() == "SERVICIO")
            {
                Dt_Productos = Calcular_Impuestos_Servicio(Dt_Productos, i);
            }

            //Asignamos el proveedor seleccionado al grid
            if(Cmb_Temporal_Proveedores.SelectedIndex!=0)
            {
                Dt_Productos.Rows[i]["Proveedor_ID"] = Cmb_Temporal_Proveedores.SelectedValue;
                Dt_Productos.Rows[i]["Nombre_Proveedor"] = Cmb_Temporal_Proveedores.SelectedItem;
            }
            Session["Dt_Productos"] = Dt_Productos;
            
        }
        Llenar_Grid_Productos();
        //CAlculamos el total cotizado
        Calcular_Importe_Total();

    }

    #endregion
}
