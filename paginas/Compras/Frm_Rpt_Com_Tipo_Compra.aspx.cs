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
using Presidencia.Rpt_Tipo_Compra.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;

public partial class paginas_Compras_Frm_Rpt_Com_Tipo_Compra : System.Web.UI.Page
{

    ///*******************************************************************
    ///VARIABLES
    ///*******************************************************************
    #region Variables

    #endregion

    ///*******************************************************************
    ///PAGE_LOAD
    ///*******************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //LLenamos el combo de Tipo
            LLenar_Combo_Tipo();
            LLenar_Combo_Tipo_Articulo();
            LLenar_Combo_Vacio(Cmb_Requisiciones);
            //LLena_Combo_Requisicion();
            Llenar_Combo_Cotizador();
            Llenar_Combo_Proveedor();
            //Llenar_Combo_Productos();
            LLenar_Combo_Vacio(Cmb_Producto);

            //Cargamos los datos de la fecha
            Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        Configuracion_Acceso("Frm_Rpt_Com_Tipo_Compra.aspx");
    }

    #endregion
    ///*******************************************************************
    ///METODOS
    ///*******************************************************************
    #region Metodos

    ///*******************************************************************
    ///LLENAR_COMBOS
    ///*******************************************************************
    #region Llenar_Combos

    public void LLenar_Combo_Tipo()
    {

        if (Cmb_Tipo_Compra.Items.Count == 0)
        {
            Cmb_Tipo_Compra.Items.Add("<<SELECCIONAR>>");
            Cmb_Tipo_Compra.Items.Add("COMPRA DIRECTA");
            Cmb_Tipo_Compra.Items.Add("COMITE COMPRAS");
            Cmb_Tipo_Compra.Items.Add("COTIZACION");
            Cmb_Tipo_Compra.Items.Add("LICITACION");
            Cmb_Tipo_Compra.Items[0].Value = "0";
            Cmb_Tipo_Compra.Items[0].Selected = true;
        }
    }

    public void LLenar_Combo_Tipo_Articulo()
    {
        Cmb_Tipo_Articulo.Items.Clear();
        Cmb_Tipo_Articulo.Items.Add("<<SELECCIONAR>>");
        Cmb_Tipo_Articulo.Items.Add("PRODUCTO");
        Cmb_Tipo_Articulo.Items.Add("SERVICIO");
        Cmb_Tipo_Articulo.Items[0].Value = "0";
        Cmb_Tipo_Articulo.Items[0].Selected = true;
    }

    public void LLenar_Combo_Vacio(DropDownList Cmb_Combo)
    {
        Cmb_Combo.Items.Clear();
        Cmb_Combo.Items.Add("<<SELECCIONAR>>");
        Cmb_Combo.Items[0].Value = "0";
        Cmb_Combo.Items[0].Selected = true;
    }

    public void LLena_Combo_Requisicion()
    {
        Cls_Rpt_Com_Tipo_Compra_Negocio Datos_Negocio = new Cls_Rpt_Com_Tipo_Compra_Negocio();
        Datos_Negocio.P_Tipo_Articulo = Cmb_Tipo_Articulo.SelectedItem.Text.Trim();
        Cmb_Requisiciones.Items.Clear();
        DataTable Data_Table = Datos_Negocio.Consultar_Requisiciones();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Requisiciones, Data_Table);
        Cmb_Requisiciones.SelectedIndex = 0;
    }
    public void Llenar_Combo_Cotizador()
    {
        Cls_Rpt_Com_Tipo_Compra_Negocio Datos_Negocio = new Cls_Rpt_Com_Tipo_Compra_Negocio();
        Cmb_Cotizador.Items.Clear();
        DataTable Data_Table = Datos_Negocio.Consultar_Cotizadores();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Cotizador, Data_Table);
        Cmb_Cotizador.SelectedIndex = 0;
    }
    public void Llenar_Combo_Proveedor()
    {
        Cls_Rpt_Com_Tipo_Compra_Negocio Datos_Negocio = new Cls_Rpt_Com_Tipo_Compra_Negocio();
        Cmb_Proveedor.Items.Clear();
        DataTable Data_Table = Datos_Negocio.Consultar_Proveedores();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Proveedor, Data_Table);
        Cmb_Proveedor.SelectedIndex = 0;
    }
    public void Llenar_Combo_Productos(String Tipo_Articulo)
    {
        Cls_Rpt_Com_Tipo_Compra_Negocio Datos_Negocio = new Cls_Rpt_Com_Tipo_Compra_Negocio();
        Cmb_Producto.Items.Clear();
        Datos_Negocio.P_Tipo_Articulo = Cmb_Tipo_Articulo.SelectedItem.Text;
        if (Tipo_Articulo == "PRODUCTO")
        {
            DataTable Data_Table = Datos_Negocio.Consultar_Productos();
            Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Producto, Data_Table);
            Cmb_Producto.SelectedIndex = 0;
        }
        if (Tipo_Articulo == "SERVICIO")
        {
            DataTable Data_Table = Datos_Negocio.Consultar_Servicios();
            Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Producto, Data_Table);
            Cmb_Producto.SelectedIndex = 0;
        }
    }

    #endregion

    ///*******************************************************************
    ///MANEJO DE FORMULARIO
    ///*******************************************************************
    #region Manejo de Formulario

    public void Limpiar_Formulario()
    {
        Cmb_Tipo_Compra.SelectedIndex = 0;
        Cmb_Tipo_Articulo.SelectedIndex = 0;
        Cmb_Requisiciones.SelectedIndex = 0;
        Cmb_Proveedor.SelectedIndex = 0;
        Cmb_Cotizador.SelectedIndex = 0;
        Cmb_Producto.SelectedIndex = 0;
        Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
    }


    #endregion



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 01/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Nombre_PDF)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/" + Nombre_PDF;
        Mostrar_Reporte(Nombre_PDF, "PDF");
        //ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Exportar_Excel
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el Reporte especificado
    ///PARAMETROS:  1.- Data_Set.- contiene la Mostrar_Informacion de la consulta a la base de datos
    ///             2.-Ds_Reporte, objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///             3.-Nombre_Reporte, contiene la Ruta del Reporte a mostrar en pantalla
    ///             4.- Nombre_Xls, nombre con el que se geradara en disco el archivo xls
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 17/MAYO/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Exportar_Excel(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Xls)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set;
        Reporte.SetDataSource(Ds_Reporte);
        //1
        ExportOptions Export_Options = new ExportOptions();
        //2
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        //3
        //4
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Nombre_Xls);
        //5
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        //6
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        //7
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        //8
        Reporte.Export(Export_Options);
        //9
        String Ruta = "../../Reporte/" + Nombre_Xls;
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

    }

    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    public Cls_Rpt_Com_Tipo_Compra_Negocio Cargar_Datos(Cls_Rpt_Com_Tipo_Compra_Negocio Dato_Negocio)
    {
        //Tipo de compra
        if (Cmb_Tipo_Compra.SelectedIndex != 0)
        {
            Dato_Negocio.P_Tipo_Compra = Cmb_Tipo_Compra.SelectedValue;
        }
        if (Cmb_Tipo_Articulo.SelectedIndex != 0)
        {
            Dato_Negocio.P_Tipo_Articulo = Cmb_Tipo_Articulo.SelectedItem.Text;
        }

        //Cargamos Requisiciones
        if (Cmb_Requisiciones.SelectedIndex != 0)
        {
            Dato_Negocio.P_Requisicion_ID = Cmb_Requisiciones.SelectedValue;
        }
        if (Cmb_Proveedor.SelectedIndex != 0)
        {
            Dato_Negocio.P_Empleado_Proveedor_ID = Cmb_Proveedor.SelectedValue;
        }

        if (Cmb_Cotizador.SelectedIndex != 0)
        {
            Dato_Negocio.P_Empleado_Cotizador_ID = Cmb_Cotizador.SelectedValue;
        }
        if (Cmb_Producto.SelectedIndex != 0)
        {
            Dato_Negocio.P_Producto_ID = Cmb_Producto.SelectedValue;
        }
        return Dato_Negocio;
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
    public Cls_Rpt_Com_Tipo_Compra_Negocio Verificar_Fecha(Cls_Rpt_Com_Tipo_Compra_Negocio Dato_Negocio)
    {

        //Variables que serviran para hacer la convecion a datetime las fechas y poder validarlas 
        DateTime Date1 = new DateTime();
        DateTime Date2 = new DateTime();

            if ((Txt_Fecha_Inicial.Text.Length == 11) && (Txt_Fecha_Final.Text.Length == 11))
            {
                //Convertimos el Texto de los TextBox fecha a dateTime
                Date1 = DateTime.Parse(Txt_Fecha_Inicial.Text);
                Date2 = DateTime.Parse(Txt_Fecha_Final.Text);
                //Validamos que las fechas sean iguales o la final sea mayor que la inicias, si no se manda un mensaje de error 
                if ((Date1 < Date2) | (Date1 == Date2))
                {
                    //Se convierte la fecha seleccionada por el usuario a un formato valido por oracle. 
                    Dato_Negocio.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text);
                    Dato_Negocio.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final.Text);
                }
                else
                {
                    Div_Contenedor_Msj_Error.Visible = true;
                    Lbl_Mensaje_Error.Text += "+ Fecha no valida <br/>";
                }
            }
        
        return Dato_Negocio;

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
    ///*******************************************************************
    ///GRID
    ///*******************************************************************
    #region Grid

    #endregion
    ///*******************************************************************
    ///EVENTOS
    ///*******************************************************************
    #region Eventos 



    protected void Btn_Limpiar_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Formulario();
    }


    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Formulario();
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }
    protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //Creamos el objeto de la clase de negocio
        Cls_Rpt_Com_Tipo_Compra_Negocio Tipo_Compra_Negocio = new Cls_Rpt_Com_Tipo_Compra_Negocio();
        //Hacemos obligatorio seleccionar un tipo de compra
        if (Cmb_Tipo_Compra.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario seleccionar un tipo de compra <br/>";
        }
        if (Cmb_Tipo_Articulo.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario seleccionar un tipo de articulo <br/>";
        }

        Tipo_Compra_Negocio = Verificar_Fecha(Tipo_Compra_Negocio);
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //Cargamos los datos de negocio
            Tipo_Compra_Negocio = Cargar_Datos(Tipo_Compra_Negocio);
            //Generamos el reporte y la consulta de acuerdo al Tipo de Compra que se selecciono
            switch (Tipo_Compra_Negocio.P_Tipo_Compra.Trim())
            {
                case "COMPRA DIRECTA":
                    //Creamos objeto del DataSet correspondiente a este reporte
                    Ds_Com_Rpt_Compra_Directa Ds_Rpt_Compra_Directa = new Ds_Com_Rpt_Compra_Directa();
                    DataSet Ds_Compra_Directa = Tipo_Compra_Negocio.Consultar_Compra_Directa();
                    Ds_Compra_Directa.Tables[0].TableName = "Dt_Compra_Directa";
                    Generar_Reporte(Ds_Compra_Directa, Ds_Rpt_Compra_Directa, "Rpt_Com_Compra_Directa.rpt","Rpt_Compra_Directa.pdf");

                    break;
                case "COMITE COMPRAS":
                    Ds_Com_Comite_Compras Ds_Objeto_Comite_Compras = new Ds_Com_Comite_Compras();
                    DataSet Ds_Comite_Compra = Tipo_Compra_Negocio.Consultar_Comite_Compra();
                    Ds_Comite_Compra.Tables[0].TableName = "DataTable1";
                    Generar_Reporte(Ds_Comite_Compra, Ds_Objeto_Comite_Compras, "Rpt_Com_Comite_Compras.rpt", "Rpt_Comite_Compras.pdf");
                    break;
                case "COTIZACION":
                    Ds_Com_Cotizaciones Ds_Objeto_Cotizacion = new Ds_Com_Cotizaciones();
                    DataSet Ds_Cotizacion = Tipo_Compra_Negocio.Consultar_Cotizacion();
                    Ds_Cotizacion.Tables[0].TableName = "DataTable1";
                    Generar_Reporte(Ds_Cotizacion, Ds_Objeto_Cotizacion, "Rpt_Com_Cotizaciones.rpt", "Rpt_Com_Cotizaciones.pdf");
                    break;
                case "LICITACION":
                    Ds_Com_Licitacion Ds_Objeto_Licitacion = new Ds_Com_Licitacion();
                    DataSet Ds_Licitacion = Tipo_Compra_Negocio.Consultar_Licitacion();
                    Ds_Licitacion.Tables[0].TableName = "DataTable1";
                    Generar_Reporte(Ds_Licitacion, Ds_Objeto_Licitacion, "Rpt_Com_Licitaciones.rpt", "Rpt_Com_Licitaciones.pdf");

                    break;
            }

        }

    }

    protected void Btn_Exportar_Excel_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        //Creamos el objeto de la clase de negocio
        Cls_Rpt_Com_Tipo_Compra_Negocio Tipo_Compra_Negocio = new Cls_Rpt_Com_Tipo_Compra_Negocio();
        //Hacemos obligatorio seleccionar un tipo de compra
        if (Cmb_Tipo_Compra.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario seleccionar un tipo de compra <br/>";
        }
        if (Cmb_Tipo_Articulo.SelectedIndex == 0)
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario seleccionar un tipo de articulo <br/>";
        }

        Tipo_Compra_Negocio = Verificar_Fecha(Tipo_Compra_Negocio);
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //Cargamos los datos de negocio
            Tipo_Compra_Negocio = Cargar_Datos(Tipo_Compra_Negocio);
            //Generamos el reporte y la consulta de acuerdo al Tipo de Compra que se selecciono
            switch (Tipo_Compra_Negocio.P_Tipo_Compra.Trim())
            {
                case "COMPRA DIRECTA":
                    //Creamos objeto del DataSet correspondiente a este reporte
                    Ds_Com_Rpt_Compra_Directa Ds_Rpt_Compra_Directa = new Ds_Com_Rpt_Compra_Directa();
                    DataSet Ds_Compra_Directa = Tipo_Compra_Negocio.Consultar_Compra_Directa();
                    Ds_Compra_Directa.Tables[0].TableName = "Dt_Compra_Directa";
                    Exportar_Excel(Ds_Compra_Directa, Ds_Rpt_Compra_Directa, "Rpt_Com_Compra_Directa.rpt", "Rpt_Compra_Directa.xls");

                    break;
                case "COMITE COMPRAS":
                    Ds_Com_Comite_Compras Ds_Objeto_Comite_Compras = new Ds_Com_Comite_Compras();
                    DataSet Ds_Comite_Compra = Tipo_Compra_Negocio.Consultar_Comite_Compra();
                    Ds_Comite_Compra.Tables[0].TableName = "DataTable1";
                    Exportar_Excel(Ds_Comite_Compra, Ds_Objeto_Comite_Compras, "Rpt_Com_Comite_Compras.rpt", "Rpt_Comite_Compras.xls");
                    break;
                case "COTIZACION":
                    Ds_Com_Cotizaciones Ds_Objeto_Cotizacion = new Ds_Com_Cotizaciones();
                    DataSet Ds_Cotizacion = Tipo_Compra_Negocio.Consultar_Cotizacion();
                    Ds_Cotizacion.Tables[0].TableName = "DataTable1";
                    Exportar_Excel(Ds_Cotizacion, Ds_Objeto_Cotizacion, "Rpt_Com_Cotizaciones.rpt", "Rpt_Com_Cotizaciones.xls");
                    break;
                case "LICITACION":
                    Ds_Com_Licitacion Ds_Objeto_Licitacion = new Ds_Com_Licitacion();
                    DataSet Ds_Licitacion = Tipo_Compra_Negocio.Consultar_Licitacion();
                    Ds_Licitacion.Tables[0].TableName = "DataTable1";
                    Exportar_Excel(Ds_Licitacion, Ds_Objeto_Licitacion, "Rpt_Com_Licitaciones.rpt", "Rpt_Com_Licitaciones.xls");

                    break;
            }

        }
    }


    #endregion
    
    protected void Cmb_Tipo_Articulo_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Tipo_Articulo.SelectedIndex == 0)
        {
            //Validamos para que quede vacio combo de requisiciones y de productos
            LLenar_Combo_Vacio(Cmb_Requisiciones);
            LLenar_Combo_Vacio(Cmb_Producto);
        }
        else
        {
            if (Cmb_Tipo_Articulo.SelectedIndex == 1)
            {
                Llenar_Combo_Productos("PRODUCTO");
                LLena_Combo_Requisicion();
            }
            if (Cmb_Tipo_Articulo.SelectedIndex == 2)
            {
                Llenar_Combo_Productos("SERVICIO");
                LLena_Combo_Requisicion();
            }
        }


    }

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
            Botones.Add(Btn_Exportar_Excel);
            Botones.Add(Btn_Exportar_PDF);

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

}
