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
using Presidencia.Listado_Almacen.Negocio;
using Presidencia.Sessiones;
using Presidencia.Generar_Requisicion.Negocio;
using Presidencia.Constantes;
using System.Collections.Generic;

public partial class paginas_Compras_Frm_Ope_Com_Listado : System.Web.UI.Page
{

    #region Variables
    Cls_Ope_Com_Listado_Negocio Listado_Negocio;
    //objeto en donde se van guardando los productos agregados al listado
    public static DataSet Ds_Productos;
    //objeto de la clase de negocio de Requisicion para acceder a la clase de datos y realizar copnexion
    private Cls_Ope_Com_Requisiciones_Negocio Requisicion_Negocio;
    //variable que permite guardar el monto presupuestal de la partida correspondiente
    public static double Monto_Presupuestal;
    //variable que permite guardar el presupuesto disponible de la partida correspondiente
    public static double Monto_Disponible;
    //variable que permite guardar el presupuesto comprometido de la partida correspondiente
    public static double Monto_Comprometido;
    //Variable que almacena el valor del importe acumulado de los productos agregados al Grid_Productos 
    public static double Importe_Producto_Acumulado;    

    #endregion 

    #region Page Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo de la pagina 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        //Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        
        if (!IsPostBack)
        {
            ViewState["SortDirection"] = "ASC";
            Session["Activa"] = true;
            Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
            Requisicion_Negocio = new Cls_Ope_Com_Requisiciones_Negocio();
            Div_Grid_Listado.Visible = true;
            Div_Datos_Generales.Visible = false;
            Estado_Formulario("Inicial");
            Llenar_Combo_Estatus();
            Llenar_Combo_Tipo();

            if (Grid_Listado.Rows.Count == 0)
            {
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Estado_Formulario("General");                
                Habilitar_Cajas(false);
            }       
        }
               
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: Metodo de la pagina 
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Page_Init(object sender, EventArgs e)
    {
        
    }
    #endregion

    #region Metodos

    #region Manejo de Combos
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Partida
    ///DESCRIPCIÓN: Metodo que carga el combo Cmb_Partida
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Partidas()
    {
        Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
        DataTable Data_Table = Listado_Negocio.Consulta_Partidas();
        Cmb_Partida.Items.Clear();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partida, Data_Table);
        Cmb_Partida.SelectedIndex = 0;
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
        Cmb_Estatus.Items.Add("EN CONSTRUCCION");
        Cmb_Estatus.Items.Add("GENERADA");
        Cmb_Estatus.Items.Add("CANCELADA");
        Cmb_Estatus.Items[0].Value = "0";
        Cmb_Estatus.Items[0].Selected = true;

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipo
    ///DESCRIPCIÓN: Metodo que carga el combo Cmb_Tipo
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Tipo()
    {
        Cmb_Tipo.Items.Clear();
        Cmb_Tipo.Items.Add("<<SELECCIONAR>>");
        Cmb_Tipo.Items.Add("MANUAL");
        Cmb_Tipo.Items.Add("AUTOMATICO");
        Cmb_Tipo.Items[0].Value = "0";
        Cmb_Tipo.Items[0].Selected = true;
    }

    #endregion Fin Manejo de Combos

    #region Metodos Generales
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Estado_Formulario
    ///DESCRIPCIÓN: Metodo que permite cambiar las propiedades de los metodos y de los Div de acuerdo al estado que se indique
    ///PARAMETROS:1.- String Estado: Indica el estado del formulario puede ser Inicia, General,Nuevo o Modificar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Estado_Formulario(String Estado)
    {
        switch (Estado)
        {
            case "Inicial":
                //Manejo de Divs
                Div_Grid_Listado.Visible = true;
                Div_Datos_Generales.Visible = false;
                Div_Busqueda.Visible = true;
                Div_Datos_Generales.Visible = false;
                Div_Grid_Productos.Visible = false;
                Grid_Productos.Enabled = false;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Configuracion_Acceso("Frm_Ope_Com_Listado.aspx");
                Configuracion_Acceso_LinkButton("Frm_Ope_Com_Listado.aspx");
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //Regresamos los valores a 0 y limpiamos los valores de la clase de negocios
                Monto_Presupuestal = 0; 
                Monto_Disponible = 0;
                Monto_Comprometido = 0;
                Importe_Producto_Acumulado = 0;
                //llenamos el Grid_de Listado
                Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
                Llenar_Grid_Listado(Listado_Negocio);
                break;
            case "General":
                //Manejo de Divs
                Div_Grid_Listado.Visible = false;
                Div_Datos_Generales.Visible = true;
                Div_Grid_Productos.Visible = false;
                Grid_Productos.Enabled = false;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                //Manejo del Contenido
                Div_Grid_Productos.Visible = false;
                Div_Busqueda.Visible = false;
                Habilitar_Cajas(false);
                Configuracion_Acceso("Frm_Ope_Com_Listado.aspx");
                Configuracion_Acceso_LinkButton("Frm_Ope_Com_Listado.aspx");
                break;
            case "Nuevo":
                //Manejo de Divs
                Div_Grid_Listado.Visible = false;
                Div_Datos_Generales.Visible = true;
                Div_Busqueda.Visible = false;
                Div_Grid_Productos.Visible = false;
                Grid_Productos.Enabled = true;
                //Boton Nuevo
                Btn_Nuevo.Visible = true;
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                //Boton Modificar
                Btn_Modificar.Visible = false;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //Manejo del Contenido
                Div_Grid_Productos.Visible = false;
                //Asignamos el valor por defecto del combo de Estatus EN CONSTRUCCION
                Cmb_Estatus.SelectedIndex = 1;
                break;
            case "Modificar":
                //Manejo de Divs
                Div_Grid_Listado.Visible = false;
                Div_Datos_Generales.Visible = true;
                Div_Busqueda.Visible = false;
                Div_Grid_Productos.Visible = true;
                Grid_Productos.Enabled = true;
                //Boton Nuevo
                Btn_Nuevo.Visible = false;
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                //Boton Modificar
                Btn_Modificar.Visible = true;
                Btn_Modificar.ToolTip = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                //Manejo del Contenido
                Div_Grid_Productos.Visible = true;
                Grid_Productos.Enabled = true;
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Formas
    ///DESCRIPCIÓN: Metodo que permite Limpiar los componentes del formulario
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Formas()
    {
        //Limpiamos datos generales
        Txt_Busqueda.Text = "";
        Txt_Folio.Text = "";
        Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Tipo.SelectedIndex = 0;
        Txt_Comentario.Text = "";
        //Limpiamos Opcion de Busqueda de producto
        Txt_Producto.Text = "";
        //Txt_Ultimo_Costo.Text = "0.0";
        Txt_Cantidad.Text = "0";
        Txt_Total.Text = "0.0";
        //Manejo de Div dentro de Datos Generales de Listado de Almacen
        Div_Grid_Productos.Visible = false;
        Grid_Productos.Enabled = false;
        Grid_Productos.DataBind();
        Grid_Comentarios.DataBind();
        Session["P_Dt_Productos"] = null;
        Session["Importe_Total"] = null;
        Session["Dt_Productos_Busqueda"] = null;
        Monto_Comprometido = 0;
        Monto_Presupuestal = 0;
        Monto_Disponible = 0;
        Importe_Producto_Acumulado = 0;

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Cajas
    ///DESCRIPCIÓN: Metodo que permite habilitar o deshabilitar los componentes del formulario
    ///PARAMETROS:1.- bool Estado: en caso de ser true habilita los componentes 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Habilitar_Cajas(bool Estatus)
    {
        Txt_Folio.Enabled = false;
        Txt_Fecha.Enabled = false;
        Cmb_Estatus.Enabled = Estatus;
        Cmb_Tipo.Enabled = Estatus;
        Cmb_Partida.Enabled = Estatus;
        Txt_Comentario.Enabled = Estatus;
        Txt_Producto.Enabled = false;
        //Estos son campos que el usuario no tiene permitido modificar
        //Txt_Ultimo_Costo.Enabled = false;
        Txt_Cantidad.Enabled = false;
        Txt_Total.Enabled = false;
        //Habilitamos o no los botones
        Btn_Agregar.Enabled = Estatus;
        Btn_Filtro_Productos.Enabled = Estatus;
    }

    #endregion Manejo Generales

    #region Validaciones
     
      

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus
    ///DESCRIPCIÓN: Metodo que permite validar que se seleccione una opcion del Cmb_Estatus
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Estatus()
    {
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Estatus es Obligatorio <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Tipo
    ///DESCRIPCIÓN: Metodo que permite validar que se seleccione una opcion del Cmb_Tipo
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Tipo()
    {
        if (Cmb_Tipo.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += "+ El Tipo es Obligatorio <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        //Realizamos los movimientos necesarios en caso de seleccionar el estatus cancelada
        if (Cmb_Estatus.SelectedValue == "CANCELADA")
        {
            //obtenemos el total del Listado de almacen 
            double Total = Convert.ToDouble(Txt_Total.Text); 
            //Realizamos la consulta para obtener la partida
            //Listado_Negocio.P_Partida_ID = Session["Partida_Almacen_ID"].ToString();
            //obtenemos los presupuestos actuales de la partida correspondiente
            //DataTable Dt_Presupuesto = Listado_Negocio.Consultar_Presupuesto_Partidas();
            //Monto_Disponible = (double.Parse(Dt_Presupuesto.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Disponible].ToString())) + Total;
            //Monto_Comprometido = (double.Parse(Dt_Presupuesto.Rows[0][Ope_Com_Pres_Partida.Campo_Monto_Comprometido].ToString())) - Total;


        }//fin del if
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Negocio
    ///DESCRIPCIÓN: Metodo que permite cargar los datos a la clase de negocio
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Cls_Ope_Com_Listado_Negocio Cargar_Datos_Negocio(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
    {
        //Asignamos el proyecto seleccionado a la clase de negocio de requisicion en caso de ser necesario ocupar este valor 
        if(Session["Listado_ID"] != null)
            Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
        Listado_Negocio.P_Folio = Txt_Folio.Text;
        Listado_Negocio.P_Estatus = Cmb_Estatus.SelectedValue;
        Listado_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
        Listado_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue;
        Listado_Negocio.P_Comentarios = Txt_Comentario.Text;
        Listado_Negocio.P_Productos_Seleccionados = (DataTable)Session["P_Dt_Productos"];
        Listado_Negocio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
        Listado_Negocio.P_Usuario_ID = Cls_Sessiones.Empleado_ID;
        Listado_Negocio.P_Total = Txt_Total.Text;
        return Listado_Negocio;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Productos
    ///DESCRIPCIÓN: Metodo que permite Validar que el usuario agregue productos
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Validar_Productos()
    {
        DataTable Dt_Productos = (DataTable)Session["P_Dt_Productos"];
        if (Dt_Productos.Rows.Count == 0)
        {
            Lbl_Mensaje_Error.Text += "+ Es necesario agregar productos al listado <br />";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Productos
    ///DESCRIPCIÓN: Metodo que permite Validar que el usuario agregue productos
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    #endregion Fin de Validaciones

    #region Metodos Productos

    public void Llenar_Combos_Productos()
    {
        //Combo Partidas
        //DataTable Data_Table = Listado_Negocio.Consulta_Partidas();
        //Cmb_Partida.Items.Clear();
        //Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Partida, Data_Table);
        //Cmb_Partida.SelectedIndex = 0;
        //Combo Subfamilias 
        //Cmb_Subfamilia.Items.Clear();
        //Cmb_Subfamilia.Items.Add("<<SELECCIONAR>>");
        //Cmb_Subfamilia.Items[0].Value = "0";
        //Cmb_Subfamilia.Items[0].Selected = true;
        
        ////Combo Modelo
        //Cmb_Modelo.Items.Clear();
        //Cmb_Modelo.Items.Add("<<SELECCIONAR>>");
        //Cmb_Modelo.Items[0].Value = "0";
        //Cmb_Modelo.Items[0].Selected = true;
       
    }

    public DataRow Calcular_Importe_Productos(DataTable Data_Productos)
    {
        //Calculamos el impuesto de los productos del DataSet 
        double Impuesto_porcentaje_1 = 0;
        double Impuesto_porcentaje_2 = 0;
        double IEPS = 0;
        double IVA = 0;
        double Mayor = 0;
        double Menor = 0;
        double Importe_Producto = 0;
        double Cantidad = 0;
        double Precio_Unitario = 0;
        double Precio_Unitario_X_Cantidad = 0;
        // Creamos el Data_Aux que contendra la estructura del grid de Productos
        DataTable Data_Aux = (DataTable)Session["P_Dt_Productos"];
        DataRow Fila = Data_Aux.NewRow();
        if (Data_Productos.Rows.Count > 0)
        {
            Impuesto_porcentaje_1 = 0;
            Impuesto_porcentaje_2 = 0;
            Mayor = 0;
            Menor = 0;
            Importe_Producto = 0;
            Cantidad = double.Parse(Txt_Cantidad.Text);
            Precio_Unitario = 0;
            if (Data_Productos.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString() != "")
            {
                Impuesto_porcentaje_1 = double.Parse(Data_Productos.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString());

            }
            if (Data_Productos.Rows[0]["IMPUESTO_PORCENTAJE_2"].ToString() != "")
            {
                Impuesto_porcentaje_2 = double.Parse(Data_Productos.Rows[0]["IMPUESTO_PORCENTAJE_2"].ToString());

            }
            //Asignamos valores a Cantidad y precio unitario
            Precio_Unitario = double.Parse(Data_Productos.Rows[0]["PRECIO_UNITARIO"].ToString());
            //Calculas los Impuestos en caso de tener 2 para obtener el importe total del producto
            if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 != 0)
            {
                Mayor = Math.Max(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
                Menor = Math.Min(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
                //Calculamos el IEPS 
                IEPS = ((Precio_Unitario * Cantidad) * Mayor) / 100;
                //Calculamos el IVA
                IVA = ((Precio_Unitario * Cantidad) * Menor) / 100;
                //Primero obtenemos el Impuesto IEPS
                Importe_Producto = ((Precio_Unitario * Cantidad) * Mayor) / 100;
                //Despues a lo obtenido del impuesto ieps le sumamos el impuesto Iva
                Importe_Producto = (Importe_Producto * Menor) / 100;
                //Sumamos el impuesto al importe total 
                Importe_Producto = Importe_Producto + (Precio_Unitario * Cantidad);
                //Le asignamos el valor a la columna de importe
                Fila["Importe"] = Importe_Producto;
            }
            //En caso de tener un solo impuesto 
            if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 == 0)
            {
                Importe_Producto = ((Precio_Unitario * Cantidad) * Impuesto_porcentaje_1) / 100;
                Importe_Producto = Importe_Producto + (Precio_Unitario * Cantidad);
                Fila["Importe"] = Importe_Producto;
                //Calculamos el monto de IVA o IEPS dependiendo cual le corresponda
                if (Data_Productos.Rows[0]["TIPO_IMPUESTO_1"].ToString() == "IVA")
                {
                    //Asignamos el Monto IVA 
                    IVA = ((Precio_Unitario * Impuesto_porcentaje_1) / 100)* Cantidad;
                    Fila["Porcentaje_IVA"] = Data_Productos.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString();
                    Fila["Porcentaje_IEPS"] = "0";
                    IEPS = 0;


                }
                if (Data_Productos.Rows[0]["TIPO_IMPUESTO_1"].ToString() == "IEPS")
                {
                    //Asignamos el monto IEPS
                    IEPS = ((Precio_Unitario * Impuesto_porcentaje_1) / 100)* Cantidad;
                    Fila["Porcentaje_IEPS"] = Data_Productos.Rows[0]["IMPUESTO_PORCENTAJE_1"].ToString();
                    Fila["Porcentaje_IVA"] = "0";
                    IVA = 0;
                }
            }
            if (Impuesto_porcentaje_1 == 0 && Impuesto_porcentaje_2 == 0)
            {
                //en caso de no tener impuestos el producto
                Importe_Producto = (Precio_Unitario * Cantidad);
                Fila["Importe"] = Importe_Producto;
                //Asignamos por default 0 los porcentajes de y montos de los impuestos 
                Fila["Monto_IVA"] = "0";
                Fila["Monto_IEPS"] = "0";
                Fila["Porcentaje_IVA"] = "0";
                Fila["Porcentaje_IEPS"] = "0";
            }
            //Asignamos los valores a la fila
            Fila["Producto_ID"] = Data_Productos.Rows[0]["PRODUCTO_ID"].ToString();
            Fila["Clave"] = Data_Productos.Rows[0]["CLAVE"].ToString();
            Fila["Producto_Nombre"] = Data_Productos.Rows[0]["PRODUCTO_NOMBRE"].ToString();
            Fila["Descripcion"] = Data_Productos.Rows[0]["Descripcion"].ToString();
            Fila["Unidad"] = Data_Productos.Rows[0]["Unidad"].ToString();
            Fila["Existencia"] = Data_Productos.Rows[0]["Existencia"].ToString();
            Fila["Reorden"] = Data_Productos.Rows[0]["REORDEN"].ToString();
            Fila["Cantidad"] = Cantidad.ToString();
            Fila["Precio_Unitario"] = Data_Productos.Rows[0]["PRECIO_UNITARIO"].ToString();
            Fila["Monto_IVA"] = IVA.ToString();
            Fila["Monto_IEPS"] = IEPS.ToString();
            //Data_Aux.Rows.Add(Fila);
            //Data_Aux.AcceptChanges();
            //Session["Dt_Productos"] = Data_Aux;
            Session["Importe_Total"] = Importe_Producto + double.Parse(Session["Importe_Total"].ToString());
        }//fin del if

        return Fila;
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Modal_Busqueda
    ///DESCRIPCIÓN: Metodo que permite limpiar los componentes dentro del div de busqueda de producto
    ///PARAMETROS:
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Limpiar_Modal_Busqueda()
    {
        Txt_Nombre_Producto.Text = "";
        Lbl_Mensaje_Error_Productos.Text = "";

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Agregar_Producto
    ///DESCRIPCIÓN: Metodo que permite agregar un nuevo producto al Grid_Productos
    ///PARAMETROS: 1.- DataTable _DataTable: Data_Table que contiene el nuevo prducto a agregar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Agregar_Producto(DataTable _DataTable)
    {
        //Realizamos la consulta del producto seleccionado
        
        //Listado_Negocio.P_Partida_ID = Session["Partida_Almacen_ID"].ToString(); 
        String Id = Session["Producto_ID"].ToString();
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["P_Dt_Productos"];
        Filas = _DataTable.Select("Producto_ID='" + Id + "'");
        if (Filas.Length > 0)
        {
            //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
            //al usuario que elemento ha agregar ya existe en la tabla de grupos.
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se puede agregar el producto, ya que este ya se ha agregado');", true);
            //Limpiamos las cajas de producto
            Txt_Producto.Text = "";
            Txt_Cantidad.Text = "0";
        }
        else
        {
            Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
            Listado_Negocio.P_Producto_ID = Id;
            //Obtengo los datos del nuevo producto a insertar en el GridView
            DataTable Dt_Temporal = Listado_Negocio.Consultar_Productos_Reorden();
            //Obtenemnos el presupuesto de la partida
            if (!(Dt_Temporal == null))
            {
                if (Dt_Temporal.Rows.Count > 0)
                {
                    DataRow Fila_Nueva = Dt.NewRow();
                    Fila_Nueva = Calcular_Importe_Productos(Dt_Temporal);
                    //Calculamos el importe
                    double Importe = double.Parse(Fila_Nueva["Importe"].ToString());
                        //Asignamos los nuevos valores a Presupuesto comprometido y disponible 
                        Monto_Disponible = Monto_Disponible - Importe;
                        Session["Disponible"] = Monto_Disponible;
                        Monto_Comprometido = Monto_Comprometido + Importe;
                        Session["Comprometido"] = Monto_Comprometido;
                        //Agregamos la fila en caso de cumplir con el presupuesto y las validaciones
                        Dt.Rows.Add(Fila_Nueva);
                        Dt.AcceptChanges();
                        Grid_Productos.DataSource = Dt;
                        Session["P_Dt_Productos"] = Dt;
                        Grid_Productos.DataBind();
                        Div_Grid_Productos.Visible = true;
                        Grid_Productos.Visible = true;
                        //Asignamos el valor Total acumulado 
                        Txt_Total.Text = Session["Importe_Total"].ToString();
                    }
                    //Limpiamos las cajas de texto
                    Txt_Producto.Text = "";
                    Txt_Cantidad.Text = "0";
                }
            
        }//fin del else

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Importe_Acumulado
    ///DESCRIPCIÓN: Metodo que permite agregar un nuevo producto al Grid_Productos
    ///PARAMETROS: 1.- DataTable _DataTable: Data_Table que contiene el nuevo prducto a agregar
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public double Importe_Acumulado()
    {
        double Importe_total_Acumulado = 0; 
        DataTable Dt_Productos = (DataTable)Session["P_Dt_Productos"];

        //Recorremos el Dt_Productos para calcular el importe 
        if (Dt_Productos.Rows.Count != 0)
        {
            for (int i = 0; i < Dt_Productos.Rows.Count; i++)
            {
                Importe_total_Acumulado = Importe_total_Acumulado + double.Parse(Dt_Productos.Rows[i]["Importe"].ToString());
            }
        }
        return Importe_total_Acumulado;
    }

    #endregion fin Metodos Productos

    #region Presupuestos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Existencia_Presupuesto
    ///DESCRIPCIÓN: Metodo que permite agregar un nuevo producto al Grid_Productos
    ///PARAMETROS: 1.- String Tipo: Indica el tipo si es nuevo o modificacion para consultar el presupuesto 
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 8/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Existencia_Presupuesto()
    {
        //Listado_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue;
        //Listado_Negocio.P_Proyecto_ID = Cmb_Proyecto.SelectedValue;
        Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
        DataTable Dt_Presupuesto = Listado_Negocio.Consultar_Presupuesto_Partidas();
        if(double.Parse(Txt_Total.Text) > double.Parse(Dt_Presupuesto.Rows[0][ Cat_Com_Dep_Presupuesto.Campo_Monto_Disponible].ToString()))
        {
           Lbl_Mensaje_Error.Text+="+ El presupuesto de esta partida no es suficiente</br>";
            Div_Contenedor_Msj_Error.Visible=true;
        }
        
    }//fin de Existencia_Presu



    #endregion Fin Presupuestos

    #region Modal Busqueda Avanzada

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Estatus_Busqueda
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Estatus que esta dentro del ModalPopup
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Estatus_Busqueda()
    {
        if (Cmb_Estatus_Busqueda.Items.Count == 0)
        {
            Cmb_Estatus_Busqueda.Items.Add("<<SELECCIONAR>>");
            Cmb_Estatus_Busqueda.Items.Add("EN CONSTRUCCION");
            Cmb_Estatus_Busqueda.Items.Add("GENERADA");
            Cmb_Estatus_Busqueda.Items.Add("CANCELADA");
            Cmb_Estatus_Busqueda.Items[0].Value = "0";
            Cmb_Estatus_Busqueda.Items[0].Selected = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Giro
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Giro que esta dentro del ModalPopup
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 03/Diciembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Giro()
    {
        //Cmb_Giro.Items.Clear();
        //DataTable Data_Table = Listado_Negocio.Consulta_Giros();
        //Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Giro, Data_Table);
        //Cmb_Partida.SelectedIndex = 0;
    }//fin llenar_combo_giro

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Carga_Componentes_Busqueda
    ///DESCRIPCIÓN: Metodo que carga e inicializa los componentes del ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Carga_Componentes_Busqueda()
    {
        Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Btn_Fecha_Inicio.Enabled = false;
        Btn_Fecha_Fin.Enabled = false;
        Llenar_Combo_Estatus_Busqueda();
        Llenar_Combo_Giro();
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
        Chk_Estatus.Checked = false;
        Cmb_Estatus_Busqueda.Enabled = false;
        Cmb_Estatus_Busqueda.SelectedIndex = 0;
        Chk_Fecha.Checked = false;
        Txt_Fecha_Inicial.Enabled = false;
        Txt_Fecha_Final.Enabled = false;              
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Estatus_Busqueda
    ///DESCRIPCIÓN: Metodo que valida que seleccione un estatus dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public Cls_Ope_Com_Listado_Negocio Validar_Estatus_Busqueda(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
    {
        if (Chk_Estatus.Checked == true)
        {
            if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
            {
                Listado_Negocio.P_Estatus_Busqueda = Cmb_Estatus_Busqueda.SelectedValue;

            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Debe seleccionar un estatus <br />";
            }

        }
        else
        {
            Listado_Negocio.P_Estatus_Busqueda = null;
        }
        return Listado_Negocio;
    }

    

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Verificar_Fecha
    ///DESCRIPCIÓN: Metodo que permite generar la cadena de la fecha y valida las fechas 
    ///en la busqueda del Modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public Cls_Ope_Com_Listado_Negocio Verificar_Fecha(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
    {
        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();
        if (Chk_Fecha.Checked == true)
        {
            if ((Txt_Fecha_Inicial.Text.Length == 11) && (Txt_Fecha_Final.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Final.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Listado_Negocio.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text);
                    Listado_Negocio.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final.Text);

                }
                else
                {
                    Img_Error_Busqueda.Visible = true;
                    Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
                }
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Fecha no valida <br />";
            }
        }
        return Listado_Negocio;
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



    #endregion Fin Modal Busqueda Avanzada

    #endregion Fin Metodos

    #region Grid

    #region Grid_Listado
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
    public void Llenar_Grid_Listado(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
    {
        DataSet Data_Set = Listado_Negocio.Consulta_Listado();
        
        if (Data_Set.Tables[0].Rows.Count != 0)
        {
            Session["Ds_Listado"] = Data_Set;
            Grid_Listado.DataSource = Data_Set;
            Grid_Listado.DataBind();
            Listado_Negocio.P_Folio_Busqueda = null;
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Listado.DataSource = new DataTable();
            Grid_Listado.DataBind();            
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que carga los datos del listado seleccionado 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_SelectedIndexChanged(object sender, EventArgs e)
    {
        Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
        //Cargamos los combos 
        Llenar_Combo_Estatus();
        Llenar_Combo_Tipo();
        Llenar_Combo_Partidas();
        Estado_Formulario("General");
        //GridViewRow representa una fila individual de un control gridview
        GridViewRow selectedRow = Grid_Listado.Rows[Grid_Listado.SelectedIndex];
        Listado_Negocio.P_Folio = Convert.ToString(selectedRow.Cells[1].Text);
        DataSet Ds_Lista = Listado_Negocio.Consulta_Listado();
        Session["Listado_ID"] = Ds_Lista.Tables[0].Rows[0].ItemArray[6].ToString();
        Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
        //CARGAMOS LOS DATOS EN LA PAGINA DE LISTADO DE ALMACEN 
        Txt_Folio.Text = Ds_Lista.Tables[0].Rows[0].ItemArray[0].ToString();
        Txt_Fecha.Text = Ds_Lista.Tables[0].Rows[0].ItemArray[1].ToString();
        //Seleccion de Estatus
        Cmb_Estatus.SelectedValue = Ds_Lista.Tables[0].Rows[0].ItemArray[2].ToString();
        Cmb_Tipo.SelectedValue = Ds_Lista.Tables[0].Rows[0].ItemArray[3].ToString();
        Cmb_Partida.SelectedValue = Ds_Lista.Tables[0].Rows[0].ItemArray[5].ToString();
        Txt_Total.Text = Ds_Lista.Tables[0].Rows[0].ItemArray[4].ToString();
        Session["Importe_Total"] = Txt_Total.Text;
        //Cargamos el grid de productos seleccionados
        DataTable Aux_Table = Listado_Negocio.Consultar_Productos_Reorden();
        Session["P_Dt_Productos"] = Aux_Table;
        //llenas el grid de Productos
        Div_Grid_Productos.Visible = true;
        Grid_Productos.Visible = true;
        Grid_Productos.DataSource = (DataTable)Session["P_Dt_Productos"];
        Grid_Productos.DataBind();
        //Llenamos el Grid de Comentarios 
        Listado_Negocio.P_Listado_ID = Session["Listado_ID"].ToString();
        Llenar_Grid_Comentarios( Listado_Negocio);
        Txt_Comentario.Text = "";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Listado
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Listado.PageIndex = e.NewPageIndex;
        Grid_Listado.DataSource = (DataSet)Session["Ds_Listado"];
        Grid_Listado.DataBind();
    }

    protected void Grid_Listado_Sorting(object sender, GridViewSortEventArgs e)
    {
        DataSet Ds_Listado = (DataSet)Session["Ds_Listado"];
        DataTable Dt_Listado = Ds_Listado.Tables[0];

        if (Dt_Listado != null)
        {
            DataView Dv_Listado = new DataView(Dt_Listado);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Listado.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Listado.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Listado.DataSource = Dv_Listado;
            Grid_Listado.DataBind();
        }

    }


    #endregion

    #region Grid_Productos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo que permite eliminar del Grid los productos seleccionados
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    
    protected void Grid_Productos_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow Renglon;
        //Obtenemos el Id del producto seleccionado
        GridViewRow selectedRow = Grid_Productos.Rows[Grid_Productos.SelectedIndex];
        String Id = Convert.ToString(selectedRow.Cells[2].Text);
        Renglones = ((DataTable)Session["P_Dt_Productos"]).Select(Cat_Com_Productos.Campo_Clave + "='" + Id + "'");
        
        if (Renglones.Length > 0)
        {
           Renglon = Renglones[0];
           DataTable Tabla = (DataTable)Session["P_Dt_Productos"];     
           Tabla.Rows.Remove(Renglon);
           Session["P_Dt_Productos"] = Tabla;
           Grid_Productos.SelectedIndex = (-1);
           Grid_Productos.DataSource = Tabla;
           Grid_Productos.DataBind();
           //Calculamos el nuevo importe
           Session["Importe_Total"] = Importe_Acumulado();
           Txt_Total.Text = Session["Importe_Total"].ToString();
           //Asignamos el nuevo valor al datatable de Session
           Session["P_Dt_Productos"] = Tabla;
        }
        
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite paginar el Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Productos.PageIndex = e.NewPageIndex;
        Grid_Productos.DataSource = (DataTable)Session["P_Dt_Productos"];
        Grid_Productos.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Requisiciones
    ///DESCRIPCIÓN: Metodo que permite llenar el Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Grid_Productos(DataSet Data_Set)
    {
        
        if (Data_Set.Tables[0].Rows.Count != 0)
        {
            Grid_Productos.DataSource = Data_Set;
            Grid_Productos.DataBind();
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+ No se encontraron datos de la busqueda de productos<br />";
        }

    }

    protected void Grid_Productos_Sorting(object sender, GridViewSortEventArgs e)
    {
        
        DataTable Dt_Productos = (DataTable)Session["P_Dt_Productos"];

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
    #endregion Grid_Productos

    #region Grid_Productos_Busqueda

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_Busqueda_SelectedIndexChanged
    ///DESCRIPCIÓN: Agregamos a la caja de texto el nombre del producto seleccionado. 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Grid_Productos_Busqueda_SelectedIndexChanged(object sender, EventArgs e)
    {
        Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
        GridViewRow Row = Grid_Productos_Busqueda.SelectedRow;
        Session["Producto_ID"] = Grid_Productos_Busqueda.SelectedDataKey["Producto_Servicio_ID"].ToString();
        Listado_Negocio.P_Producto_ID = Grid_Productos_Busqueda.SelectedDataKey["Producto_Servicio_ID"].ToString();
        Txt_Producto.Text = Row.Cells[2].Text;
        //habilitamos el la caja de cantidad y boton de agregar producto
        Txt_Cantidad.Enabled = true;
        Btn_Agregar.Enabled = true;
        Div_Grid_Productos.Visible = true;
        //Limpiamos el grid de Productos_Busqueda
        Grid_Productos_Busqueda.DataBind();
        Modal_Productos.Hide();
        UPnl_Busqueda.Update();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Productos_Busqueda_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Productos_Busqueda_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Modal_Productos.Show();
        Grid_Productos_Busqueda.PageIndex = e.NewPageIndex;
        Grid_Productos_Busqueda.DataSource = (DataTable)Session["Dt_Productos_Busqueda"];
        Grid_Productos_Busqueda.DataBind();
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
    public void Llenar_Grid_Productos_Busqueda(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
    {

        DataTable Data_Table = Listado_Negocio.Consultar_Productos();
        if (Data_Table.Rows.Count != 0)
        {
            Grid_Productos_Busqueda.DataSource = Data_Table;
            Grid_Productos_Busqueda.DataBind();
            Session["Dt_Productos_Busqueda"] = Data_Table;
            Grid_Productos_Busqueda.Visible = true;
        }
        else
        {
            Lbl_Mensaje_Error_Productos.Text = "+ No se encontraron datos de la busqueda de productos<br />";
            //Lo dejamos vacio
            Grid_Productos_Busqueda.DataSource = new DataTable();
            Grid_Productos_Busqueda.DataBind();
        }

    }

    protected void Grid_Productos_Busqueda_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Productos = (DataTable)Session["Dt_Productos_Busqueda"];

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

            Grid_Productos_Busqueda.DataSource = Dv_Productos;
            Grid_Productos_Busqueda.DataBind();
        }

    }

    #endregion Grid_Productos_Busqueda

    #region Grid Comentarios

    public void Llenar_Grid_Comentarios(Cls_Ope_Com_Listado_Negocio Listado_Negocio)
    {
        
        DataTable Data_Table = Listado_Negocio.Consultar_Observaciones_Listado();
        Session["Dt_Comentarios"] = Data_Table;
        if (Data_Table.Rows.Count != 0)
        {
            Grid_Comentarios.DataSource = Data_Table;
            Grid_Comentarios.DataBind();
            Div_Comentarios.Visible = true;
        }
        else
        {
            //No Mostramos el div de Comentarios en caso de no existir datos 
            Div_Comentarios.Visible = false;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_PageIndexChanging
    ///DESCRIPCIÓN: Metodo que permite la paginacion del Grid_Listado
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Comentarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Comentarios.PageIndex = e.NewPageIndex;
        Grid_Comentarios.DataSource = (DataTable)Session["Dt_Comentarios"];
        Grid_Comentarios.DataBind();
    }


    protected void Grid_Comentarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        int Renglon = Grid_Comentarios.SelectedIndex;
        DataTable Dt_Aux = (DataTable)Session["Dt_Comentarios"];
        Txt_Comentario.Text = Dt_Aux.Rows[Renglon][0].ToString();
    }

    protected void Grid_Comentarios_Sorting(object sender, GridViewSortEventArgs e)
    {

        DataTable Dt_Comentarios = (DataTable)Session["Dt_Comentarios"];

        if (Dt_Comentarios != null)
        {
            DataView Dv_Comentarios = new DataView(Dt_Comentarios);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Comentarios.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Comentarios.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Comentarios.DataSource = Dv_Comentarios;
            Grid_Comentarios.DataBind();
        }

    }
    #endregion Grid Comentarios


    #endregion Fin Grid

    #region Eventos

    #region Eventos Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Evento del Boton Nuevo
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        switch (Btn_Nuevo.ToolTip)
        { 
            case "Nuevo":
                Estado_Formulario("Nuevo");
                Habilitar_Cajas(true);
                Limpiar_Formas();
                //llenamos el combo de Partidas
                Llenar_Combo_Partidas();
                Cmb_Estatus.SelectedIndex = 1;
                //Limpiamos el Grid de Productos
                Grid_Productos.DataBind();
                break;
            case "Dar de Alta":
                
                //llevamos a cabo las validaciones de los combos 
                Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
                Validar_Estatus();
                Validar_Tipo();
                if (Cmb_Partida.SelectedIndex == 0)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "Es obligatorio la partida";
                }
                if (Grid_Productos.Rows.Count == 0)
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "Es obligatorio agregar un producto";
                }

                        //Si los datos ingresados por el usuario son correctos se inserta el registro en la BD
                        try
                        {
                            if (Div_Contenedor_Msj_Error.Visible == false)
                            {
                                Listado_Negocio = Cargar_Datos_Negocio(Listado_Negocio);
                                //if (Listado_Negocio.Modificar_Presupuestos() == true)
                                //{
                                    String Mensaje = Listado_Negocio.Alta_Listado();
                                    //Si existe algun comentario 
                                    if (Txt_Comentario.Text.Trim() != String.Empty)
                                        Listado_Negocio.Alta_Observaciones_Listado();
                                    Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
                                    Estado_Formulario("Inicial");
                                    Habilitar_Cajas(false);

                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Listado Almacen", "alert('" + Mensaje + "');", true);
                                //}
                                //else
                                //{
                                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Listado Almacen", "alert('No existe presupuesto suficiente');", true);
                                //}
                            }

                        }
                        catch (Exception Ex)
                        {
                            throw new Exception("Error al dar de alta el listado de Almacen. Error: [" + Ex.Message + "]");
                            Limpiar_Formas();
                            Estado_Formulario("Inicial");
                        }
                    
                
                break;
        }


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Evento del Boton Modificar
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Modificar.ToolTip)
        {
            case "Modificar":
                if (Txt_Folio.Text.Trim() == String.Empty)
                {
                    Lbl_Mensaje_Error.Text = "+ Debe seleccionar un listado <br />";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                else
                {
                    Estado_Formulario("Modificar");
                    Habilitar_Cajas(true);
                    Cmb_Partida.Enabled = false;
                    Cmb_Tipo.Enabled = false;
                }
                break;
            case "Actualizar":
                //llevamos a cabo las validaciones de los combos 
                Validar_Estatus();
                Validar_Tipo();
                Validar_Productos();
                Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
                //Si los datos ingresados por el usuario son correctos se inserta el registro en la BD
                try
                {
                    
                    if (Div_Contenedor_Msj_Error.Visible == false)
                    {
                        Listado_Negocio= Cargar_Datos_Negocio(Listado_Negocio);
                        //Si existe presupuesto lo modifica y despues modifica el listado 
                        //if (Listado_Negocio.Afectar_Presupuesto() == true)
                        //{
                            String Mensaje = Listado_Negocio.Modificar_Listado();
                            if (Txt_Comentario.Text.Trim() != String.Empty)
                            {
                                Listado_Negocio.Alta_Observaciones_Listado();
                            }
                            Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
                            Limpiar_Formas();
                            Estado_Formulario("Inicial");
                            Habilitar_Cajas(false);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Listado Almacen", "alert('" + Mensaje + "');", true);
                        //}
                        //else
                        //{
                        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Listado Almacen", "alert('No existe presupuesto suficiente');", true);
                        //}
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception("Error al modificar el listado de Almacen. Error: [" + Ex.Message + "]");
                }
                break;
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Evento del Boton Salir
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Habilitar_Cajas(false);
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                break;
            case "Listado":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Limpiar_Formas();
                Habilitar_Cajas(false);
                Div_Busqueda.Visible = true;
                //Llenamos el Grid
                Div_Datos_Generales.Visible = false;
                Estado_Formulario("Inicial");
                break;
            case "Cancelar":
                Div_Contenedor_Msj_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
                Limpiar_Formas();
                Habilitar_Cajas(false);
                Estado_Formulario("Inicial");
                break;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Filtro_Productos_Click
    ///DESCRIPCIÓN: Evento del Boton Filtro, que permite visualizar el div de Busqueda de productos
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Filtro_Productos_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //Validamos que se seleccionen los datos
        Validar_Estatus();
        Validar_Tipo();
        Grid_Productos_Busqueda.Visible = true;
        if (Cmb_Partida.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + " Es necesario seleccionar una partida";
        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
           
            //Deshabilitamos los componentes de datos Generales para que el usuario no pueda campiar lo ya seleccionado
            Txt_Folio.Enabled = false;
            Txt_Fecha.Enabled = false;
            ////Limpiamos el modal de busqueda de productos
            //En caso de ser Manual
            if (Cmb_Tipo.SelectedIndex == 1)
            {
                Modal_Productos.Show();
                Limpiar_Modal_Busqueda();
                Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
                Listado_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue;
                Grid_Productos_Busqueda.Visible = true;
                //Realizamos la busqueda y cargamos el Grid. 
                Llenar_Grid_Productos_Busqueda(Listado_Negocio);
                Cmb_Tipo.Enabled = false;
                Cmb_Partida.Enabled = false;
            }
            //en caso de Ser Reorden o automatico
            if (Cmb_Tipo.SelectedIndex == 2)
            {
                Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
                Div_Grid_Productos.Visible = true;
                Btn_Agregar.Enabled = false;
                Grid_Productos.Enabled = true;
                //Consultamos los productos que esten en punto de reorden
                Listado_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
                Listado_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue;
                DataTable Data_Productos = Listado_Negocio.Consultar_Productos_Reorden();
                //Usamos un datatable auxiliar para calcular los impuestos de los productos en punto de reorden.
                if (Data_Productos.Rows.Count > 0)
                {
                    DataTable Data_Aux = new DataTable();
                    Data_Aux.Columns.Add("Producto_ID", typeof(System.String));
                    Data_Aux.Columns.Add("Clave", typeof(System.String));
                    Data_Aux.Columns.Add("Descripcion", typeof(System.String));
                    Data_Aux.Columns.Add("Unidad", typeof(System.String));
                    Data_Aux.Columns.Add("Producto_Nombre", typeof(System.String));
                    Data_Aux.Columns.Add("Existencia", typeof(System.String));
                    Data_Aux.Columns.Add("Reorden", typeof(System.String));
                    Data_Aux.Columns.Add("Cantidad", typeof(System.String));
                    Data_Aux.Columns.Add("Precio_Unitario", typeof(System.String));
                    Data_Aux.Columns.Add("Importe", typeof(System.String));
                    Data_Aux.Columns.Add("Monto_IVA", typeof(System.String));
                    Data_Aux.Columns.Add("Monto_IEPS", typeof(System.String));
                    Data_Aux.Columns.Add("Porcentaje_IVA", typeof(System.String));
                    Data_Aux.Columns.Add("Porcentaje_IEPS", typeof(System.String));
                    //Calculamos el impuesto de los productos del DataSet en punto de Reorden 
                    double Impuesto_porcentaje_1 = 0;
                    double Impuesto_porcentaje_2 = 0;
                    double IEPS = 0;
                    double IVA = 0;
                    double Mayor = 0;
                    double Menor = 0;
                    double Importe_Total = 0;
                    double Cantidad = 0;
                    double Precio_Unitario = 0;
                    for (int i = 0; i < Data_Productos.Rows.Count; i++)
                    {
                        Impuesto_porcentaje_1 = 0;
                        Impuesto_porcentaje_2 = 0;
                        Mayor = 0;
                        Menor = 0;
                        Importe_Total = 0;
                        Cantidad = 0;
                        Precio_Unitario = 0;
                        DataRow Fila = Data_Aux.NewRow();
                        if (Data_Productos.Rows[i]["IMPUESTO_PORCENTAJE_1"].ToString() != "")
                        {
                            Impuesto_porcentaje_1 = double.Parse(Data_Productos.Rows[i]["IMPUESTO_PORCENTAJE_1"].ToString());
                            
                        }
                        if (Data_Productos.Rows[i]["IMPUESTO_PORCENTAJE_2"].ToString() != "")
                        {
                            Impuesto_porcentaje_2 = double.Parse(Data_Productos.Rows[i]["IMPUESTO_PORCENTAJE_2"].ToString());
                           

                        }
                        //Asignamos valores a Cantidad y precio unitario
                        Cantidad = double.Parse(Data_Productos.Rows[i]["CANTIDAD"].ToString());
                        Precio_Unitario = double.Parse(Data_Productos.Rows[i]["PRECIO_UNITARIO"].ToString());
                        //Calculas los Impuestos en caso de tener 2 para obtener el importe total del producto
                        if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 != 0)
                        {
                            Mayor = Math.Max(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
                            Menor = Math.Min(Impuesto_porcentaje_1, Impuesto_porcentaje_2);
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
                            Fila["Importe"] = Importe_Total;
                        }
                        //En caso de tener un solo impuesto 
                        if (Impuesto_porcentaje_1 != 0 && Impuesto_porcentaje_2 == 0)
                        {
                            Importe_Total = ((Precio_Unitario * Cantidad) * Impuesto_porcentaje_1) / 100;
                            Importe_Total = Importe_Total + (Precio_Unitario * Cantidad);
                            Fila["Importe"] = Importe_Total;
                            //Calculamos el monto de IVA o IEPS dependiendo cual le corresponda
                            if (Data_Productos.Rows[i]["TIPO_IMPUESTO_1"].ToString()=="IVA")
                            {
                                //Asignamos el Monto IVA 
                                IVA = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
                                Fila["Porcentaje_IVA"] = Data_Productos.Rows[i]["IMPUESTO_PORCENTAJE_1"].ToString();
                                Fila["Porcentaje_IEPS"] = "0";
                                IEPS = 0;
                                

                            }
                            if (Data_Productos.Rows[i]["TIPO_IMPUESTO_1"].ToString() == "IEPS")
                            {
                                //Asignamos el moento IEPS
                                IEPS = (Precio_Unitario * Impuesto_porcentaje_1) / 100;
                                Fila["Porcentaje_IEPS"] = Data_Productos.Rows[i]["IMPUESTO_PORCENTAJE_1"].ToString();
                                Fila["Porcentaje_IVA"] = "0";
                                IVA = 0;
                            }
                        }
                        if (Impuesto_porcentaje_1 == 0 && Impuesto_porcentaje_2 == 0)
                        {
                            //en caso de no tener impuestos el producto
                            Importe_Total = (Precio_Unitario * Cantidad);
                            Fila["Importe"] = Importe_Total;
                            //Asignamos por default 0 los porcentajes de y montos de los impuestos 
                            Fila["Monto_IVA"] = "0";
                            Fila["Monto_IEPS"] = "0";
                            Fila["Porcentaje_IVA"] = "0";
                            Fila["Porcentaje_IEPS"] = "0";
                        }
                        //Asignamos los valores a la fila
                        Fila["Producto_ID"] = Data_Productos.Rows[i]["PRODUCTO_ID"].ToString();
                        Fila["Clave"] = Data_Productos.Rows[i]["CLAVE"].ToString();
                        Fila["Descripcion"] = Data_Productos.Rows[i]["DESCRIPCION"].ToString();
                        Fila["Unidad"] = Data_Productos.Rows[i]["UNIDAD"].ToString();
                        Fila["Producto_Nombre"] = Data_Productos.Rows[i]["PRODUCTO_NOMBRE"].ToString();
                        Fila["Existencia"] = Data_Productos.Rows[i]["EXISTENCIA"].ToString();
                        Fila["Reorden"] = Data_Productos.Rows[i]["REORDEN"].ToString();
                        Fila["Cantidad"] = Data_Productos.Rows[i]["CANTIDAD"].ToString();
                        Fila["Precio_Unitario"] = Data_Productos.Rows[i]["PRECIO_UNITARIO"].ToString();
                        Fila["Monto_IVA"] = IVA.ToString();
                        Fila["Monto_IEPS"] = IEPS.ToString();
                        Data_Aux.Rows.Add(Fila);
                        Data_Aux.AcceptChanges();
                        Session["P_Dt_Productos"] = Data_Aux;
                    }//fin del for
                    //Calculamos el total del importe a pagar
                    //Asignamos el valor a la caja de texto total
                    Txt_Total.Text = Convert.ToString(Importe_Acumulado());
                    Grid_Productos.DataSource = (DataTable)Session["P_Dt_Productos"];
                    Grid_Productos.DataBind();
                    Cmb_Tipo.Enabled = false;
                    Cmb_Partida.Enabled = false;
                }//Fin del if
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "+ No se encontraron productos en punto de Reorden <br />";
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Click
    ///DESCRIPCIÓN: Evento del Boton agregar producto al listado 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Agregar_Click(object sender, ImageClickEventArgs e)
    {
        //Iniciamos las variables de apoyo
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        if(Txt_Producto.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "+Es necesario seleccionar un Producto<br /> ";
        }
        if (Txt_Cantidad.Text.Trim() == "0")
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text + "+Es necesario indicar la cantidad solicitada del Producto<br /> ";
        }
        if(Div_Contenedor_Msj_Error.Visible == false)
        {
            Grid_Productos.Enabled = true;
            DataTable Dt = (DataTable)Session["P_Dt_Productos"];
            if (Session["P_Dt_Productos"] != null)
            {
                Cmb_Partida.Enabled = false;
                Cmb_Tipo.Enabled = false;
                Agregar_Producto((DataTable)Session["P_Dt_Productos"]);
            }//fin if
            else
            {
                //Creamos la session por primera ves
                DataTable Dt_Producto = new DataTable();
                Dt_Producto.Columns.Add("Producto_ID", typeof(System.String));
                Dt_Producto.Columns.Add("Clave", typeof(System.String));
                Dt_Producto.Columns.Add("Producto_Nombre", typeof(System.String));
                Dt_Producto.Columns.Add("Descripcion", typeof(System.String));
                Dt_Producto.Columns.Add("Unidad", typeof(System.String));
                Dt_Producto.Columns.Add("Existencia", typeof(System.String));
                Dt_Producto.Columns.Add("Reorden", typeof(System.String));
                Dt_Producto.Columns.Add("Cantidad", typeof(System.String));
                Dt_Producto.Columns.Add("Precio_Unitario", typeof(System.String));
                Dt_Producto.Columns.Add("Importe", typeof(System.String));
                Dt_Producto.Columns.Add("Monto_IVA", typeof(System.String));
                Dt_Producto.Columns.Add("Monto_IEPS", typeof(System.String));
                Dt_Producto.Columns.Add("Porcentaje_IVA", typeof(System.String));
                Dt_Producto.Columns.Add("Porcentaje_IEPS", typeof(System.String));
                Session["P_Dt_Productos"] = Dt_Producto;
                //Llenamos el grid
                Grid_Productos.DataSource = (DataTable)Session["P_Dt_Productos"];
                Grid_Productos.DataBind();
               
                Session["Importe_Total"] = 0;
                Agregar_Producto(Dt_Producto);
            }//fin else
        }//fin del if general 
    }

    #endregion Fin Eventos Botones

    #region Eventos Combos
   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Partida_SelectedIndexChanged
    ///DESCRIPCIÓN: Evento del combo Proyecto, que carga en base a lo seleccionado el de Partidas
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    protected void Cmb_Partida_SelectedIndexChanged(object sender, EventArgs e)
    {
        //siempre que cambie de partida se limpia el grid de productos
        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        Session["P_Dt_Productos"] = null;
        Txt_Total.Text = "";
    }

    protected void Cmb_Tipo_SelectedIndexChanged(object sender, EventArgs e)
    {
        //siempre que cambie de partida se limpia el grid de productos
        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        Session["P_Dt_Productos"] = null;
        Txt_Total.Text = "0.0";
    }
    #endregion Fin Eventos Combos

    #region Eventos Busqueda Avanzada

    protected void Btn_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Estado_Formulario("Inicial");
        Pnl_Busqueda.Visible = true;
        Modal_Busqueda.Show();
        Carga_Componentes_Busqueda();
    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Busqueda.Text.Trim() == String.Empty)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error_Productos.Text = "+ Es necesario indicar el folio a buscar <br />";
        }
        else
        {
            Listado_Negocio.P_Folio_Busqueda = Txt_Busqueda.Text;
            Llenar_Grid_Listado(Listado_Negocio);

        }
    }
    #endregion

    #region Eventos Busqueda de Productos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Realizar_Busqueda_Click
    ///DESCRIPCIÓN: Evento del Boton de Realizar busqueda, el cual carga el grd de Busqueda productos en base al filtro
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Realizar_Busqueda_Click(object sender, EventArgs e)
    {
        //Asignamos los valores a la clase de negocio en caso de seleccionar algun elemento
        Modal_Productos.Show();
        Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
        Lbl_Mensaje_Error_Productos.Text = "";
        Listado_Negocio.P_Partida_ID = null;
        Listado_Negocio.P_Nombre_Producto = null;
              
        
        if (Cmb_Partida.SelectedIndex != 0)
        {
            Listado_Negocio.P_Partida_ID = Cmb_Partida.SelectedValue;
        }
        else
        {
            Lbl_Mensaje_Error_Productos.Text += "+ Es necesario seleccionar una Partida <br />";
        }
        if (Txt_Nombre_Producto.Text.Trim() != String.Empty)
        {
            Listado_Negocio.P_Nombre_Producto = Txt_Nombre_Producto.Text;
        }
               
        if (Lbl_Mensaje_Error_Productos.Text.Trim() == "")
        {
           
            Grid_Productos_Busqueda.Visible = true;
            //Realizamos la busqueda y cargamos el Grid. 
            Llenar_Grid_Productos_Busqueda(Listado_Negocio);
        }


    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Click
    ///DESCRIPCIÓN: Evento del Boton de Cerrar, el cual oculta el div de busueda de productos y muestra el 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Click(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        Div_Grid_Productos.Visible = true;
        Limpiar_Modal_Busqueda();

    }
  

    #endregion Fin Eventos Busqueda de Productos

    #region Busqueda Avanzada

   

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_CheckedChanged
    ///DESCRIPCIÓN: Evento del check Chk_Fecha que habilita o no las cajas de texto de la fecha
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Fecha_CheckedChanged(object sender, EventArgs e)
    {
        Modal_Busqueda.Show();
        if (Chk_Fecha.Checked == true)
        {
            Txt_Fecha_Inicial.Enabled = true;
            Txt_Fecha_Final.Enabled = true;
            Btn_Fecha_Inicio.Enabled = true;
            Btn_Fecha_Fin.Enabled = true;
        }
        else
        {
            Txt_Fecha_Inicial.Enabled = false;
            Txt_Fecha_Final.Enabled = false;
            Btn_Fecha_Inicio.Enabled = false;
            Btn_Fecha_Fin.Enabled = false;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Evento del check Chk_Estatus que habilita o no el combo de estatus
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Estatus_CheckedChanged(object sender, EventArgs e)
    {

        Modal_Busqueda.Show();
        if (Chk_Estatus.Checked == true)
        {
            Cmb_Estatus_Busqueda.Enabled = true;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
        }
        else
        {
            Cmb_Estatus_Busqueda.Enabled = false;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Evento del check Chk_Estatus que habilita o no el combo de estatus
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, EventArgs e)
    {
        Listado_Negocio = new Cls_Ope_Com_Listado_Negocio();
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
        Listado_Negocio = Validar_Estatus_Busqueda(Listado_Negocio);
        Listado_Negocio = Verificar_Fecha(Listado_Negocio);
        if ((Chk_Fecha.Checked == false) && (Chk_Estatus.Checked == false))
        {
            Img_Error_Busqueda.Visible = true;
            Lbl_Error_Busqueda.Text += "+ Debe seleccionar una opcion <br />";
        }
        if (Img_Error_Busqueda.Visible == false)
        {
            Modal_Busqueda.Hide();
            // lo ponemos en el estado inicial para k realice el filtro 
            Estado_Formulario("Inicial");
                        
        }
        else
        {
            Modal_Busqueda.Show();
        }


    }

    #endregion Fin Busqueda Avanzada

    #endregion Fin Eventos

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
