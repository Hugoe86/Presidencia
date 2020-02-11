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
using Presidencia.Licitacion_Proveedores.Negocio;
using Presidencia.Licitaciones_Compras.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Com_Licitacion_Proveedores : System.Web.UI.Page
{
    ///*******************************************************************************
    /// VARIABLES
    ///*******************************************************************************
    #region Variables
    //Variable de la clase de Negocio
    Cls_Ope_Com_Licitacion_Proveedores_Negocio Lic_Pro_Negocio;
    //Variable de la clase de Negocio del alta de Licitaciones
    Cls_Ope_Com_Licitaciones_Negocio Licitacion_Alta;

    #endregion 

    ///*******************************************************************************
    /// REGION PAGE_LOAD
    ///*******************************************************************************
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Init(object sender, EventArgs e)
    {
        Lic_Pro_Negocio = new Cls_Ope_Com_Licitacion_Proveedores_Negocio();
        Licitacion_Alta = new Cls_Ope_Com_Licitaciones_Negocio();
        Estado_Formulario("Inicial");
        Llenar_Grid_Licitaciones();
        
    }


    #endregion Fin_Page_Load

    ///*******************************************************************************
    /// REGION METODOS
    ///*******************************************************************************
    #region Metodos

    ///*******************************************************************************
    /// Subregion Metodos Generales
    ///*******************************************************************************
    #region Metodos_Generales

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Formulario
    ///DESCRIPCIÓN: Metodo que permite cambiar las propiedades de los metodos y de los Div de acuerdo al estado que se indique
    ///PARAMETROS:1.- String Estado: Indica el estado del formulario puede ser Inicia, General,Nuevo o Modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Formulario(String Estado)
    {
        switch (Estado)
        {
            case "Inicial":
                //MANEJO DE DIVS
                Div_Licitaciones.Visible = true;
                Div_Datos_Licitacion.Visible = false;
                Div_Busqueda.Visible = true;
                //Boton de Modificar
                Configuracion_Acceso("Frm_Ope_Com_Licitacion_Proveedores.aspx");
                Configuracion_Acceso_LinkButton("Frm_Ope_Com_Licitacion_Proveedores.aspx");
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //llenamos nuevamente el grid de licitaciones 
                Llenar_Grid_Licitaciones();

                break;
            case "General":
                //MANEJO DE DIVS
                Div_Licitaciones.Visible = false;
                Div_Datos_Licitacion.Visible = true;
                Div_Busqueda.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Llenar_Combo_Estatus();
                Habilitar_Cajas(false);
                Tab_Container_Licitacion.Enabled = false;

                Configuracion_Acceso("Frm_Ope_Com_Licitacion_Proveedores.aspx");
                Configuracion_Acceso_LinkButton("Frm_Ope_Com_Licitacion_Proveedores.aspx");
                break;
            case "Modificar":
                //MANEJO DE DIVS
                Div_Licitaciones.Visible = false;
                Div_Datos_Licitacion.Visible = true;
                Div_Busqueda.Visible = false;
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Habilitar_Cajas(true);
                Tab_Container_Licitacion.Enabled = true;
                break;

        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Cajas
    ///DESCRIPCIÓN: Metodo que permite habilitar los componenetes validos 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Cajas(bool Estatus)
    {
        Cmb_Estatus.Enabled = Estatus;
        Txt_Costo_Producto.Enabled = Estatus;
        Cmb_Proveedor.Enabled = Estatus;
        Btn_Agregar.Enabled = Estatus;
        Grid_Productos.Enabled = Estatus;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formulario
    ///DESCRIPCIÓN: Metodo que permite limpiar los componentes del formulario de operacion
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Formulario()
    {
        Txt_Busqueda.Text = "";
        Txt_Folio.Text = "";
        Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Justificacion.Text = "";
        Txt_Comentario.Text = "";
        Txt_Nombre_Producto.Text = "";
        Txt_Costo_Producto.Text = "0.0";
        Txt_Total.Text = "0.0";
        Grid_Requisiciones.DataBind();
        Grid_Productos.DataBind();
        Cmb_Proveedor.Items.Clear();
        //Limpiamos las variables de Session 
        Session["Dt_Requisiciones"] = null;
        Session["Total_Licitacion"] = null;
        Session["No_Licitacion"] = null;
        //limpiamos el objeto de la clase de negocio
        Licitacion_Alta = new Cls_Ope_Com_Licitaciones_Negocio();
        Lic_Pro_Negocio = new Cls_Ope_Com_Licitacion_Proveedores_Negocio();

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus()
    ///DESCRIPCIÓN: Metodo que permite cargar los el combo de estatus
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus()
    {
        Cmb_Estatus.Items.Clear();
        Cmb_Estatus.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus.Items.Add("ASIGNADA");
        Cmb_Estatus.Items.Add("TERMINADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus_Busqueda()
    ///DESCRIPCIÓN: Metodo que permite cargar los el combo de estatus busqueda
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus_Busqueda()
    {
        Cmb_Estatus_Busqueda.Items.Clear();
        Cmb_Estatus_Busqueda.Items.Add("<<SELECCIONAR>>");
        Cmb_Estatus_Busqueda.Items.Add("ASIGNADA");
        Cmb_Estatus_Busqueda.Items.Add("TERMINADA");
        Cmb_Estatus_Busqueda.Items[0].Value = "0";
        Cmb_Estatus_Busqueda.Items[0].Selected = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Proveedores()
    ///DESCRIPCIÓN: Metodo que permite llenar el combo de los proveedores 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Proveedores()
    {
        Cmb_Proveedor.Items.Clear();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Proveedor, Lic_Pro_Negocio.Consultar_Concepto_Proveedor());
        Cmb_Proveedor.SelectedIndex = 0;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Negocio()
    ///DESCRIPCIÓN: Metodo que permite cargar los datos de la clase de negocios que serviran para modificar 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Datos_Negocio()
    {
        Lic_Pro_Negocio.P_No_Licitacion = Session["No_Licitacion"].ToString();
        Lic_Pro_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Lic_Pro_Negocio.P_Monto_Total = Txt_Total_Cotizado.Text;
        Lic_Pro_Negocio.P_Dt_Productos = (DataTable)Session["Dt_Productos"];
        Lic_Pro_Negocio.P_Lista_Requisiciones = Session["Lista_Requisiciones"].ToString();

    }

   

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
    public void Verificar_Fecha(TextBox Fecha_Inicial, TextBox Fecha_Final, Label Mensaje_Error)
    {

        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();

        if ((Fecha_Inicial.Text.Length == 11) && (Fecha_Final.Text.Length == 11))
        {
            //Convertimos el Texto de los TextBox fecha a dateTime
            Date1 = DateTime.Parse(Fecha_Inicial.Text);
            Date2 = DateTime.Parse(Fecha_Final.Text);
            //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
            if ((Date1 < Date2) | (Date1 == Date2))
            {
                //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                Lic_Pro_Negocio.P_Fecha_Inicio = Formato_Fecha(Fecha_Inicial.Text);
                Lic_Pro_Negocio.P_Fecha_Fin = Formato_Fecha(Fecha_Final.Text);

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


    #endregion Fin_Metodos_Generales
    ///*******************************************************************************
    /// Metodos de Manejo de Productos
    ///*******************************************************************************
    #region Metodos_Manejo_De_Productos


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
        Lic_Pro_Negocio.P_Producto_ID = _TableProductos.Rows[Num_Fila]["Prod_Serv_ID"].ToString();
        //cONSULTAMOS LOS IMPUESTOS DEL PRODUCTO 
        DataTable Dt_Impuestos_Producto = Lic_Pro_Negocio.Consultar_Impuesto_Producto();
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

    public void Verificar_Productos_Cotizados()
    {
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        if (Dt_Productos.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                if (Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado].ToString().Trim() == String.Empty)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "+ Es necesario cotizar todos los productos</br>";
                    break;
                }
                else
                {
                    if (double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado].ToString()) == 0)
                    {
                        Div_Contenedor_Msj_Error.Visible = true;
                        Lbl_Mensaje_Error.Text += "+ Es necesario cotizar todos los productos</br>";
                        break;
                    }
                }
            }
        }
    }



    public void Calcular_Importe_Total()
    {
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        double Total_Cotizado = 0;
        if (Dt_Productos.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                Total_Cotizado = Total_Cotizado + double.Parse(Dt_Productos.Rows[i][Ope_Com_Req_Producto.Campo_Total_Cotizado].ToString().Trim());
            }
            Txt_Total_Cotizado.Text = Total_Cotizado.ToString();

        }
        else
        {
            Txt_Total_Cotizado.Text = "0.0";

        }
    }

    #endregion Fin_Manejo_De_Productos


    #endregion Fin_Metodos

    ///*******************************************************************************
    /// REGION GRID
    ///*******************************************************************************
    #region Grid

    ///*******************************************************************************
    /// SUBREGION GRID_LICITACIONES
    ///*******************************************************************************
    #region Grid_Licitaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Licitaciones()
    ///DESCRIPCIÓN: Metodo que permite cargar el grid de Licitaciones
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Licitaciones()
    {
        DataTable Dt_Licitaciones = Lic_Pro_Negocio.Consultar_Licitaciones();
    
        if (Dt_Licitaciones.Rows.Count != 0)
        {
            Session["Dt_Licitaciones"] = Dt_Licitaciones;
            Grid_Licitaciones.DataSource = Dt_Licitaciones;
            Grid_Licitaciones.DataBind();
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se encontraron Licitaciones";
            Grid_Licitaciones.DataSource = new DataTable();
            Grid_Licitaciones.DataBind();
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Licitaciones_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Licitaciones
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Ene/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Licitaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Licitaciones.PageIndex = e.NewPageIndex;
        Grid_Licitaciones.DataSource = (DataTable)Session["Dt_Licitaciones"];
        Grid_Licitaciones.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Licitaciones_SelectedIndexChanged()
    ///DESCRIPCIÓN: Metodo que permite cargar los datos de la licitacion seleccionada
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Licitaciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        Estado_Formulario("General");
        Lic_Pro_Negocio.P_No_Licitacion = Grid_Licitaciones.SelectedDataKey["No_Licitacion"].ToString();
        Session["No_Licitacion"] = Grid_Licitaciones.SelectedDataKey["No_Licitacion"].ToString();
        DataTable Dt_Licitacion_Seleccionada = Lic_Pro_Negocio.Consultar_Licitaciones();
        //Asignamos los valores de la licitacion seleccionada
        Txt_Folio.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Folio].ToString();
        Txt_Fecha_Inicio.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Fecha_Inicio].ToString();
        Txt_Fecha_Fin.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Fecha_Fin].ToString();
        Txt_Justificacion.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Justificacion].ToString();
        Txt_Comentario.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Comentarios].ToString();
        Txt_Total.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Monto_Total].ToString();
        Txt_Tipo.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Tipo].ToString();
        Txt_Clasificacion.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Clasificacion].ToString();
        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Estatus].ToString().Trim()));
        Txt_Total_Cotizado.Text = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Total_Cotizado].ToString();
        Session["Lista_Requisiciones"] = Dt_Licitacion_Seleccionada.Rows[0][Ope_Com_Licitaciones.Campo_Lista_Requisiciones].ToString();
        //Cargamos los detalles de la licitacion seleccionada solo las requisiciones
        Licitacion_Alta.P_No_Licitacion = Session["No_Licitacion"].ToString();
        DataTable Dt_Lic_Detalle = Licitacion_Alta.Consultar_Licitacion_Detalle_Requisicion();
        //Cargamos el grid de requisicion
        Grid_Requisiciones.DataSource = Dt_Lic_Detalle;
        Grid_Requisiciones.DataBind();
        //Cargamos el grid de productos los detalles de la licitacion 
        Lic_Pro_Negocio.P_Tipo = Txt_Tipo.Text.Trim();
        DataTable Dt_Productos = Lic_Pro_Negocio.Consulta_Productos_Detalle();
        if (Dt_Productos.Rows.Count != 0)
        {
            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();
        }
        //Cargamos las variables de session
        Session["Dt_Requisiciones"] = Dt_Lic_Detalle;
        Session["Total_Licitacion"] = 0;
        Session["Dt_Productos"] = Dt_Productos;
        

    }

    #endregion Fin_Grid_Licitaciones

    ///*******************************************************************************
    /// SUBREGION GRID_PRODUCTOS
    ///*******************************************************************************
    #region Grid_Productos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite cargar los datos del proveedor seleccionado para cada producto 
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Guardamos el valor seleccionado en una variable de session 
        Session["Ope_Com_Req_Producto_ID"] = Grid_Productos.SelectedDataKey["Ope_Com_Req_Producto_ID"].ToString();
        //Cargamos el producto seleccionado en las cajas
        if (Grid_Productos.SelectedIndex > (-1))
        {
            GridViewRow selectedRow = Grid_Productos.Rows[Grid_Productos.SelectedIndex];
            Txt_Nombre_Producto.Text = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();
            //Pasamos a un datatable la variable de session que contiene el listado de productos
            DataTable Dt_Productos = (DataTable) Session["Dt_Productos"];
            Lic_Pro_Negocio.P_Concepto_ID = Dt_Productos.Rows[Grid_Productos.SelectedIndex]["CONCEPTO_ID"].ToString();
            //Cargamos el combo de Proveedor de acuerdo al giro que este cumple
            Llenar_Combo_Proveedores();
            //Guardamos el numero de la fila seleccionada ya que se ocupara mas adelane
            int registro = ((Grid_Productos.PageIndex) * Grid_Productos.PageSize) + (Grid_Productos.SelectedIndex);
            Session["Producto_Seleccionado"] = registro;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Ene/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos.PageIndex = e.NewPageIndex;
        Grid_Productos.DataSource = (DataTable)Session["Dt_Productos"];
        Grid_Productos.DataBind();
    }

    #endregion Fin_Grid_Productos
    ///*******************************************************************************
    /// SUBREGION GRID_REQUISICIONES
    ///*******************************************************************************
    #region Grid_Requisiciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Requisiciones_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Requisiciones
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 29/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataSource = (DataTable)Session["Dt_Requisiciones"];
        Grid_Requisiciones.DataBind();
    }


    #endregion Fin_Grid_Requisiciones
    
    #endregion Fin_Grid
    
    ///*******************************************************************************
    /// REGION EVENTOS
    ///*******************************************************************************
    #region Eventos

    ///*******************************************************************************
    /// SUBREGION EVENTOS BARRA BOTONES GENERALES Y BUSQUEDAS
    ///*******************************************************************************
        #region Barra_Botones_Generales y Busqueda

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del boton Modificar
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        
        switch(Btn_Modificar.ToolTip)
        {
            case "Modificar":
                Estado_Formulario("Modificar");

                break;
            case "Actualizar":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                //Validamos que este seleccionado un Estatus
                if (Cmb_Estatus.SelectedIndex == 0)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "+ Debe seleccionar un Estatus<br/>";
                }

                if (Cmb_Estatus.SelectedValue == "TERMINADA")
                {
                    Verificar_Productos_Cotizados();
                }
                if (Div_Contenedor_Msj_Error.Visible == false)
                {
                    Cargar_Datos_Negocio();
                    Lic_Pro_Negocio.Modificar_Licitacion();
                    //limpiamos el formulario 
                    Limpiar_Formulario();
                    Estado_Formulario("Inicial");
                    Llenar_Grid_Licitaciones();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Licitaciones", "alert('Se modifico satisfactoriamente la Licitacion ');", true);

                }
                break;
                
        }
        
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del boton salir
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 15/Enero/2011 
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
                Limpiar_Formulario();
                break;

            case "Cancelar":
                Estado_Formulario("Inicial");
                Limpiar_Formulario();
                break;
            case "Listado":
                Estado_Formulario("Inicial");
                Limpiar_Formulario();
                break;
        }

    }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
        ///DESCRIPCIÓN: Evento del boton Buscar por folio 
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Txt_Busqueda.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ Ingrese un Folio";
            }
            if (Div_Contenedor_Msj_Error.Visible == false)
            {
                Lic_Pro_Negocio.P_Folio = Txt_Busqueda.Text.Trim();
                Llenar_Grid_Licitaciones();
            }
        }


    #endregion Fin_Barra_Botones_Generales y Busqueda
    ///*******************************************************************************
    /// SUBREGION EVENTOS PRODUCTOS
    ///*******************************************************************************
        #region Eventos_Productos
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Click
        ///DESCRIPCIÓN: Evento del boton que agrega un proveedor al detalle del producto  
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 15/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
        {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Txt_Nombre_Producto.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ Es necesario seleccionar un producto <br/>";
            }

            if (Txt_Costo_Producto.Text.Trim() == String.Empty)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "+ Ingrese el costo sin impuesto<br/>";
            }

            if (Cmb_Proveedor.SelectedIndex == 0)
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+ Es necesario seleccionar un proveedor <br/>";
            }

            if(Div_Contenedor_Msj_Error.Visible == false)
            {
                //Insertamos el proveedor seleccionado al Grid_Productos y al Dt_Productos que es una variable de session 
                DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
                int num_fila = int.Parse(Session["Producto_Seleccionado"].ToString());
                Dt_Productos.Rows[num_fila][Ope_Com_Req_Producto.Campo_Nombre_Proveedor] = Cmb_Proveedor.SelectedItem;
                Dt_Productos.Rows[num_fila][Ope_Com_Req_Producto.Campo_Proveedor_ID] = Cmb_Proveedor.SelectedValue;
                Dt_Productos.Rows[num_fila][Ope_Com_Req_Producto.Campo_Precio_U_Sin_Imp_Cotizado] = Txt_Costo_Producto.Text;
                //CALCULAMOS LOS IMPUESTOS DEL PRODUCTO
                Dt_Productos = Calcular_Impuestos(Dt_Productos, num_fila);
                //CARGAMOS NUEVAMENTE EL GRID_pRODUCTOS
                Session["Dt_Productos"] = Dt_Productos;
                Grid_Productos.DataSource = Dt_Productos;
                Grid_Productos.DataBind();
                //CAlculamos el total cotizado
                Calcular_Importe_Total();
                //Limpiamos las cajas de texto
                Txt_Costo_Producto.Text = "0";
                Txt_Nombre_Producto.Text = "";
                Cmb_Proveedor.Items.Clear();
            }
        }
        #endregion Fin_Eventos_Productos
    ///*******************************************************************************
    /// SUBREGION EVENTOS BUSQUEDA AVANZADA
    ///*******************************************************************************
        #region Eventos_Busqueda_Avanzada

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
        ///DESCRIPCIÓN: Evento del boton Busqueda avanzada
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 18/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
        {
            Modal_Busqueda.Show();
            //Limpiamos la clase de negocio
            Lic_Pro_Negocio = new Cls_Ope_Com_Licitacion_Proveedores_Negocio();
            //Cargamos los datos del Modal de busqueda avanzada
            Lbl_Error_Busqueda.Text = "";
            Llenar_Combo_Estatus_Busqueda();
            Txt_Fecha_Avanzada_1.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Avanzada_2.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Click
        ///DESCRIPCIÓN: Evento del boton Aceptar que realiza una busqueda detallada de las licitaciones
        ///PARAMETROS:
        ///CREO: Susana Trigueros Armenta
        ///FECHA_CREO: 18/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        protected void Btn_Aceptar_Click(object sender, EventArgs e)
        {
            Lbl_Error_Busqueda.Text = "";
            //Limpiamos la clase de negocio para realizar la busqueda
            Lic_Pro_Negocio = new Cls_Ope_Com_Licitacion_Proveedores_Negocio();
            //Realializamos las validaciones para mostrar la informacion
            if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
                Lic_Pro_Negocio.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue;
            else
                Lic_Pro_Negocio.P_Estatus = null;
            Verificar_Fecha(Txt_Fecha_Avanzada_1, Txt_Fecha_Avanzada_2, Lbl_Error_Busqueda);
            Lbl_Error_Busqueda.Visible = true;
            if (Lbl_Error_Busqueda.Text.Trim() == "")
            {
                //Al pasar las validaciones Cargamos los datos a la clase de negocio
                Llenar_Grid_Licitaciones();
                Modal_Busqueda.Hide();
            }
            else
            {
                Modal_Busqueda.Show();
            }
        }
        #endregion Fin_Eventos_Busqueda_Avanzada

    #endregion Fin_Eventos

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
        protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
        {
            List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
            DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

            try
            {
                //Agregamos los botones a la lista de botones de la página.
                Botones.Add(Btn_Busqueda_Avanzada);

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
