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


public partial class paginas_Compras_Frm_Ope_Com_Cotizacion_Manual_PULL : System.Web.UI.Page
{
    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Session["Activa"] = true;
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
                Grid_Requisiciones.Visible = true;
                Grid_Requisiciones.Enabled = true;
                Div_Detalle_Requisicion.Visible = false;
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
                Llenar_Combo_Cotizadores_Busqueda();
                Txt_Elaboro_Cotizacion.Enabled = false;
                break;
            case "General":
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Cotizar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Enabled = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //
                Div_Busqueda.Visible = false;
                Div_Grid_Requisiciones.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = false;
                //Deshabilitar controles 
                Txt_Reg_Padron_Prov.Enabled = false;
                Btn_Calcular_Precios_Cotizados.Enabled = false;
                Btn_Fecha_Fin.Enabled = false;
                Txt_Garantia.Enabled = false;
                Txt_Tiempo_Entrega.Enabled = false;
                Txt_Fecha_Elaboracio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Cmb_Estatus_Propuesta.Enabled = false;
                Txt_Elaboro_Cotizacion.Enabled = false;
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
                Div_Busqueda.Visible = false;
                Div_Detalle_Requisicion.Visible = true;
                Grid_Productos.Enabled = true;
                //habilitar controles 
                Txt_Reg_Padron_Prov.Enabled = false;
                Btn_Calcular_Precios_Cotizados.Enabled = true;
                Btn_Fecha_Fin.Enabled = true;
                Txt_Garantia.Enabled = true;
                Txt_Tiempo_Entrega.Enabled = true;
                Txt_Fecha_Elaboracio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Cmb_Estatus_Propuesta.Enabled = true;
                Txt_Elaboro_Cotizacion.Enabled = true;
                break;
        }//fin del switch

    }

    public void Llenar_Combo_Cotizadores_Busqueda()
    {
        //Cmb_Cotizador.Items.Clear();
        //Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Negocios = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
        //DataTable Data_Table = Negocios.Consultar_Cotizadores();
        //Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Cotizador, Data_Table);
        //Cmb_Cotizador.SelectedIndex = 0;
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
        Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
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
        Clase_Negocio.P_Tipo_Articulo = Txt_Tipo_Articulo.Text.Trim();
        //cONSULTAMOS LOS IMPUESTOS DEL PRODUCTO 
        DataTable Dt_Impuestos_Producto = Clase_Negocio.Consultar_Impuesto_Producto();
        try
        {
            if (Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString() != "")
            {
                Impuesto_porcentaje_1 = double.Parse(Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString());

            }
            if (Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_2"].ToString() != "")
            {
                Impuesto_porcentaje_2 = double.Parse(Dt_Impuestos_Producto.Rows[0]["IMPUESTO_PORCENTAJE_2"].ToString());
            }
        }
        catch(Exception Ex)
        {
            Ex.ToString();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Propuesta Cotizacion", "alert('No se pueden calcular importes. Verifique los impuestos de los productos!');", true);
            return null;
        }
        //Asignamos valores a Cantidad y precio unitario
        Cantidad = double.Parse(_TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Cantidad].ToString());
        Precio_Unitario = double.Parse(_TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Sin_Imp_Cotizado].ToString());
        //Calculas los Impuestos en caso de tener 2 para obtener el importe total del producto
        if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 != 0)
        {
            Mayor = Math.Max(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
            Menor = Math.Min(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
            //Calculamos el Precio con Impuesto
            Precio_C_Impuesto = Math.Round((Precio_Unitario * Mayor) / 100,2);
            Precio_C_Impuesto = Math.Round((Precio_C_Impuesto * Menor) / 100,2);
            Precio_C_Impuesto = Math.Round( Precio_C_Impuesto + Precio_Unitario,2);
            _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado] = Precio_C_Impuesto;
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
            _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado] = Importe_Total;
        }
        //En caso de tener un solo impuesto 
        if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 == 0)
        {
            //Calculamos el costo con impuesto 
            Precio_C_Impuesto = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
            _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado] = Precio_C_Impuesto + Precio_Unitario;
            //Calculamos el importe total
            Importe_Total = ((Precio_Unitario * Cantidad) * Impuesto_porcentaje_1) / 100;
            Importe_Total = Importe_Total + (Precio_Unitario * Cantidad);
            _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado] = Importe_Total;
            //Calculamos el monto de IVA o IEPS dependiendo cual le corresponda
            if (Dt_Impuestos_Producto.Rows[0]["TIPO_IMPUESTO_1"].ToString() == "IVA")
            {
                //Asignamos el Monto IVA 
                IVA = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
                IVA = IVA * Cantidad;
                IEPS = 0;
            }
            if (Dt_Impuestos_Producto.Rows[0]["TIPO_IMPUESTO_1"].ToString() == "IEPS")
            {
                //Asignamos el moento IEPS
                IEPS = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
                IEPS = IEPS * Cantidad;
                IVA = 0;
            }
        }
        if (Impuesto_porcentaje_1 == 0 && Impuesto_porcentaje_2 == 0)
        {
            //en caso de no tener impuestos el producto
            Importe_Total = (Precio_Unitario * Cantidad);
            _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado] = Importe_Total;
            _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Precio_U_Con_Imp_Cotizado] = Precio_Unitario;
            IVA = 0;
            IEPS = 0;
        }
        //cARGAMOS LOS DATOS
        _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado] = IVA;
        _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado] = IEPS;
        //Calculamos el Subtotal, (es el total sin impuestos)
        _TableProductos.Rows[Num_Fila][Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado] = Cantidad * Precio_Unitario;

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
                IVA_Cotizado = IVA_Cotizado + Math.Round(double.Parse(Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_IVA_Cotizado].ToString().Trim()),2);
                IEPS_Cotizado = IEPS_Cotizado + Math.Round(double.Parse(Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_IEPS_Cotizado].ToString().Trim()), 2);
                Subtotal_Cotizado = Subtotal_Cotizado + Math.Round(double.Parse(Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Subtota_Cotizado].ToString().Trim()),2);
                Total_Cotizado = Total_Cotizado + Math.Round(double.Parse(Dt_Productos.Rows[i][Ope_Com_Propuesta_Cotizacion.Campo_Total_Cotizado].ToString().Trim()), 2);
            }

            Total_Cotizado = Math.Round(Total_Cotizado, 2);
            IVA_Cotizado = Math.Round(IVA_Cotizado, 2);
            Subtotal_Cotizado = Math.Round(Subtotal_Cotizado, 2);

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
        Txt_Elaboro_Cotizacion.Text = "";
        Txt_Proveedor.Text = "";
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
    public void Llenar_Grid_Requisiciones()
    {
        Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
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
        Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
        GridViewRow selectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
        int num_fila = Grid_Requisiciones.SelectedIndex;
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];

        if (Session["Dt_Requisiciones_Aux"] != null)
        {
            Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones_Aux"];
        }

        Clase_Negocio.P_Proveedor_ID = Dt_Requisiciones.Rows[num_fila]["Proveedor_ID"].ToString().Trim();
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
        Txt_Proveedor.Text = Dt_Requisiciones.Rows[num_fila]["Nombre_Proveedor"].ToString().Trim();
        Txt_Dependencia.Text = Dt_Detalle_Requisicion.Rows[0]["DEPENDENCIA"].ToString().Trim();
        Txt_Folio.Text = Dt_Detalle_Requisicion.Rows[0]["FOLIO"].ToString().Trim();
        Txt_Concepto.Text = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO"].ToString().Trim();
        Txt_Fecha_Generacion.Text = Dt_Detalle_Requisicion.Rows[0]["FECHA_GENERACION"].ToString().Trim();
        Txt_Tipo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO"].ToString().Trim();
        Txt_Tipo_Articulo.Text = Dt_Detalle_Requisicion.Rows[0]["TIPO_ARTICULO"].ToString().Trim();
        Txt_Estatus.Text = Dt_Detalle_Requisicion.Rows[0]["ESTATUS"].ToString().Trim();
        Txt_Justificacion.Text = Dt_Detalle_Requisicion.Rows[0]["JUSTIFICACION_COMPRA"].ToString().Trim();
        Txt_Especificacion.Text = Dt_Detalle_Requisicion.Rows[0]["ESPECIFICACION_PROD_SERV"].ToString().Trim();

        Txt_Compra_Especial.Text = Dt_Detalle_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Especial_Ramo_33].ToString().Trim();

        Session["TIPO_ARTICULO"] = Txt_Tipo_Articulo.Text.Trim();
        Session["Concepto_ID"] = Dt_Detalle_Requisicion.Rows[0]["CONCEPTO_ID"].ToString().Trim();
        //LLenamos los text de la propuesta de Cotizacion
        DataTable Dt_Propuesta = Clase_Negocio.Consultar_Propuesta_Cotizacion();
        Txt_Reg_Padron_Prov.Text = int.Parse(Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Registro_Padron_Prov].ToString().Trim()).ToString();
        Txt_Garantia.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Garantia].ToString().Trim();
        Txt_Fecha_Elaboracio.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Fecha_Elaboracion].ToString().Trim();
        Txt_Tiempo_Entrega.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Tiempo_Entrega].ToString().Trim();
        Txt_Vigencia.Text = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Vigencia_Propuesta].ToString().Trim();
        Cmb_Estatus_Propuesta.SelectedValue = Dt_Propuesta.Rows[0][Ope_Com_Propuesta_Cotizacion.Campo_Estatus].ToString().Trim();
        Txt_Elaboro_Cotizacion.Text = Dt_Propuesta.Rows[0]["ELABORO_PROPUESTA"].ToString().Trim();
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
        Session["Dt_Requisiciones_Aux"] = null;
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
        Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
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

                if (Txt_Reg_Padron_Prov.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "- Es necesario ingresar el Registro de Padron de Proveedores</br>";

                }
                if (Txt_Vigencia.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "- Es necesario ingresar la fecha de la vigencia de la propuesta</br>";

                }
                if (Txt_Garantia.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "- Es necesario ingresar la garantia </br>";

                }
                if (Txt_Tiempo_Entrega.Text.Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "- Es necesario ingresar el tiempo de entrega </br>";

                }
                if (String.IsNullOrEmpty(Txt_Elaboro_Cotizacion.Text))
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "- Ingrese nombre de quién firma la cotización </br>";

                }

                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Agregear_Marca_Descripcion_A_DataTable_Productos_Y_Calcular_Totales();
                    //Cargamos los datos de la clase de negocios
                    Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
                    Clase_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString().Trim();
                    Clase_Negocio.P_Proveedor_ID = Session["Proveedor_ID"].ToString().Trim();
                    Clase_Negocio.P_Dt_Productos = (DataTable)Session["Dt_Productos"];
                    Clase_Negocio.P_Subtotal_Cotizado = Txt_SubTotal_Cotizado_Requisicion.Text.Trim();
                    Clase_Negocio.P_Total_Cotizado = Txt_Total_Cotizado_Requisicion.Text.Trim();
                    Clase_Negocio.P_IEPS_Cotizado = Txt_IEPS_Cotizado.Text.Trim();
                    Clase_Negocio.P_IVA_Cotizado = Txt_IVA_Cotizado.Text.Trim();

                    Configurar_Formulario("Inicio");
                    Clase_Negocio.P_Vigencia_Propuesta = Formato_Fecha(Txt_Vigencia.Text);
                    Clase_Negocio.P_Fecha_Elaboracion = Formato_Fecha(Txt_Fecha_Elaboracio.Text);
                    Clase_Negocio.P_Reg_Padron_Proveedor = Txt_Reg_Padron_Prov.Text;
                    Clase_Negocio.P_Garantia = Txt_Garantia.Text;
                    Clase_Negocio.P_Tiempo_Entrega = Txt_Tiempo_Entrega.Text;
                    Clase_Negocio.P_Estatus_Propuesta = Cmb_Estatus_Propuesta.SelectedValue;
                    Clase_Negocio.P_Elabora_Propuesta = Txt_Elaboro_Cotizacion.Text;
                    //damos de alta 
                    bool Operacion_Realizada = Clase_Negocio.Guardar_Cotizacion();

                    if (Operacion_Realizada)
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Propuesta Cotizacion", "alert('Se realizo la Cotizacion Exitosamente');", true);
                    else
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Propuesta Cotizacion", "alert('No se realizo la Cotizacion');", true);


                    Limpiar_Componentes();
                    ////Cargamos otra ves el grid requisiciones
                    Llenar_Grid_Requisiciones();
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
                Limpiar_Componentes();
                Llenar_Grid_Requisiciones();

                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Limpiar_Componentes();
                Llenar_Grid_Requisiciones();
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
        //REalizamos las validaciones
        Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio Clase_Negocio = new Cls_Ope_Com_Cotizacion_Manual_PULL_Negocio();
        if (Txt_Requisicion_Busqueda.Text.Trim() != String.Empty)
        {
            Clase_Negocio.P_Requisicion_Busqueda = Txt_Requisicion_Busqueda.Text;
        }
        DataTable Dt_Consulta_Req = Clase_Negocio.Consultar_Requisiciones();

        if (Dt_Consulta_Req.Rows.Count != 0)
        {
            Session["Dt_Requisiciones_Aux"] = Dt_Consulta_Req;
            Grid_Requisiciones.DataSource = Dt_Consulta_Req;
            Grid_Requisiciones.DataBind();
            

        }
        else
        {
            Grid_Requisiciones.EmptyDataText = "No se han encontrado registros.";
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();
            Session["Dt_Requisiciones"] = null;
        }//fin else
    }

    private void Agregear_Marca_Descripcion_A_DataTable_Productos_Y_Calcular_Totales()
    {
        DataTable Dt_Productos = new DataTable();
        Dt_Productos = (DataTable)Session["Dt_Productos"];
        double Num = 0;
        for (int i = 0; i < Dt_Productos.Rows.Count; i++)
        {
            Dt_Productos = (DataTable)Session["Dt_Productos"];
            TextBox Txt_Precio_Unitario = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
            TextBox Txt_Marca = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Marca");
            TextBox Txt_Descripcion_Producto_Cot = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Descripcion_Producto_Cot");

            if (!String.IsNullOrEmpty(Txt_Precio_Unitario.Text.Trim()))
            {
                try
                {
                    Num = Convert.ToDouble(Txt_Precio_Unitario.Text.Trim());
                }
                catch (Exception Ex)
                {
                    Ex.ToString();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Propuesta Cotizacion", "alert('Es posible que no se ingresaran los precios correctamente, Verifique!!');", true);
                    return;
                }
                Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"] = Txt_Precio_Unitario.Text;

            }//fin del IF
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Propuesta Cotizacion", "alert('Es posible que no se ingresaran los precios correctamente, Verifique!!');", true);
                return;
            }
            Dt_Productos.Rows[i]["Marca"] = Txt_Marca.Text;
            if (Txt_Descripcion_Producto_Cot.Text.Trim().Length == 0)
            {
                Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"] = "";
            }
            else
            {
                Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"] = Txt_Descripcion_Producto_Cot.Text;
            }

            //if (!String.IsNullOrEmpty(Txt_Marca.Text))
            //{
            //    Dt_Productos.Rows[i]["Marca"] = Txt_Marca.Text;
            //}
            //if (!String.IsNullOrEmpty(Txt_Descripcion_Producto_Cot.Text))
            //{
            //    Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"] = Txt_Descripcion_Producto_Cot.Text;
            //}

            //CALCULAMOS LOS IMPUESTOS DEL PRODUCTO
            Dt_Productos = Calcular_Impuestos(Dt_Productos, i);
            if (Dt_Productos != null)
            {
                Session["Dt_Productos"] = Dt_Productos;
            }

        }
        Llenar_Grid_Productos();
        //CAlculamos el total cotizado
        Calcular_Importe_Total();
    }

    protected void Btn_Calcular_Precios_Cotizados_Click(object sender, EventArgs e)
    {
        Agregear_Marca_Descripcion_A_DataTable_Productos_Y_Calcular_Totales();
        //DataTable Dt_Productos = new DataTable();
        //Dt_Productos = (DataTable)Session["Dt_Productos"];
        //for (int i = 0; i < Dt_Productos.Rows.Count; i++)
        //{
        //    Dt_Productos = (DataTable)Session["Dt_Productos"];
        //    TextBox Txt_Precio_Unitario = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Precio_Unitario");
        //    TextBox Txt_Marca = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Marca");
        //    TextBox Txt_Descripcion_Producto_Cot = (TextBox)Grid_Productos.Rows[i].FindControl("Txt_Descripcion_Producto_Cot");

        //    if (Txt_Precio_Unitario != null)
        //    {
        //        Dt_Productos.Rows[i]["Precio_U_Sin_Imp_Cotizado"] = Txt_Precio_Unitario.Text;

        //    }//fin del IF
        //    if (Txt_Marca != null)
        //    {
        //        Dt_Productos.Rows[i]["Marca"] = Txt_Marca.Text;
        //    }

        //    if (Txt_Descripcion_Producto_Cot != null)
        //    {
        //        Dt_Productos.Rows[i]["DESCRIPCION_PRODUCTO_COT"] = Txt_Descripcion_Producto_Cot.Text;
        //    }

        //    //CALCULAMOS LOS IMPUESTOS DEL PRODUCTO
        //    Dt_Productos = Calcular_Impuestos(Dt_Productos, i);

        //    Session["Dt_Productos"] = Dt_Productos;

        //}
        //Llenar_Grid_Productos();
        ////CAlculamos el total cotizado
        //Calcular_Importe_Total();
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if(Div_Contenedor_Msj_Error.Visible == false)
        {
            //Mandamos los datos para la impresion
            //Dt de los detalles de la Requisicion
            DataTable Dt_Detalles_Propuesta_Cotizacion = new DataTable();
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Folio", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Dependencia", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Tipo", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Tipo_Articulo", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Verificacion", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Justificacion", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Especificacion", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Registro_Padron", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Fecha_Elaboracion", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Fecha_Entrega", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Vigencia_Propuesta", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Garantia", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Subtotal_Cotizado", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("IVA_Cotizado", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("IEPS_Cotizado", typeof(System.String));
            Dt_Detalles_Propuesta_Cotizacion.Columns.Add("Total_Cotizado", typeof(System.String));
            //Agregamos una nueva fila con los valores
            DataRow Fila_Nueva = Dt_Detalles_Propuesta_Cotizacion.NewRow();
            Fila_Nueva["Folio"] = Txt_Folio.Text.Trim();
            Fila_Nueva["Dependencia"] = Txt_Dependencia.Text.Trim();
            Fila_Nueva["Tipo"] = Txt_Tipo.Text.Trim();
            Fila_Nueva["Tipo_Articulo"] = Txt_Tipo_Articulo.Text.Trim();
            if (Chk_Verificacion.Checked == true)
            {
                Fila_Nueva["Verificacion"] = "SI";
            }
            else
            {
                Fila_Nueva["Verificacion"] = "NO";
            }
            Fila_Nueva["Verificacion"] = Txt_Tipo_Articulo.Text.Trim();



        }

    }

    //protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    //{
    //    Div_Contenedor_Msj_Error.Visible = false;
    //    Lbl_Mensaje_Error.Text = "";
    //    //Verificamos las fechas
    //    Cls_Ope_Com_Resultados_Propuestas_Negocio Clase_Negocio = new Cls_Ope_Com_Resultados_Propuestas_Negocio();
    //    //Cargamos los datos en la clase de negocio
    //    if (Txt_Requisicion_Busqueda.Text.Trim() != String.Empty)
    //    {
    //        //Cargamos los datos de la Requisicion
    //        Clase_Negocio.P_Requisicion_Busqueda = Txt_Requisicion_Busqueda.Text.Trim();

    //    }

    //    if (Cmb_Cotizador.SelectedIndex != 0)
    //    {
    //        //Cargamos los datos de la Requisicion
    //        Clase_Negocio.P_Busqueda_Cotizador = Cmb_Cotizador.SelectedValue;

    //    }

    //    if (Chk_Fecha_Elaboracion.Checked == true)
    //    {
    //        //checamos si la fecha final es mayor k la inicial
    //        if (Verificar_Fecha(Txt_Busqueda_Fecha_Elaboracion_Ini, Txt_Busqueda_Fecha_Elaboracion_Fin))
    //        {
    //            Clase_Negocio.P_Busqueda_Fecha_Elaboracion_Ini = Formato_Fecha(Txt_Busqueda_Fecha_Elaboracion_Ini.Text);
    //            Clase_Negocio.P_Busqueda_Fecha_Elaboracion_Fin = Formato_Fecha(Txt_Busqueda_Fecha_Elaboracion_Fin.Text);

    //        }
    //        else
    //        {
    //            Div_Contenedor_Msj_Error.Visible = true;
    //            Lbl_Mensaje_Error.Text = " Las fechas de Entrega no son correctas </br>";
    //        }
    //    }
    //    if (Chk_Fecha_Entrega.Checked == true)
    //    {
    //        //checamos si la fecha final es mayor k la inicial
    //        if (Verificar_Fecha(Txt_Busqueda_Fecha_Entrega_Ini, Txt_Busqueda_Fecha_Entrega_Fin))
    //        {
    //            Clase_Negocio.P_Busqueda_Fecha_Entrega_Ini = Formato_Fecha(Txt_Busqueda_Fecha_Entrega_Ini.Text);
    //            Clase_Negocio.P_Busqueda_Fecha_Entrega_Fin = Formato_Fecha(Txt_Busqueda_Fecha_Entrega_Fin.Text);

    //        }
    //        else
    //        {
    //            Div_Contenedor_Msj_Error.Visible = true;
    //            Lbl_Mensaje_Error.Text = " Las fechas de Entrega no son correctas </br>";
    //        }
    //    }

    //    if (Chk_Verificacion.Checked == true)
    //    {
    //        //checamos si la fecha final es mayor k la inicial
    //        if (Verificar_Fecha(Txt_Busqueda_Vigencia_Propuesta_Ini, Txt_Busqueda_Vigencia_Propuesta_Fin))
    //        {
    //            Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Ini = Formato_Fecha(Txt_Busqueda_Vigencia_Propuesta_Ini.Text);
    //            Clase_Negocio.P_Busqueda_Vigencia_Propuesta_Fin = Formato_Fecha(Txt_Busqueda_Vigencia_Propuesta_Fin.Text);

    //        }
    //        else
    //        {
    //            Div_Contenedor_Msj_Error.Visible = true;
    //            Lbl_Mensaje_Error.Text = " Las fechas de Entrega no son correctas </br>";
    //        }
    //    }

    //    //si paso todas las validaciones reaizamos la busqueda
    //    if (Div_Contenedor_Msj_Error.Visible == false)
    //    {
    //        Llenar_Grid_Requisiciones(Clase_Negocio);
    //    }

    //}

}
