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
using Presidencia.Reporte_Requisiciones.Negocios;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;


public partial class paginas_Compras_Frm_Rpt_Com_Reporte_Requisiciones: System.Web.UI.Page
{

    ///*******************************************************************
    ///VARIABLES
    ///*******************************************************************
    #region Variables
    Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio;

    #endregion

    ///*******************************************************************
    ///PAGE_LOAD
    ///*******************************************************************
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configurar_Formulario("Inicio");
            //Inicializamos la variable de la clase de negocio
            Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();

            //Cargamos el grid de Requisiciones
            Llenar_Grid_Requisiciones();
        }        
    }


    #endregion 
    ///*******************************************************************
    ///METODOS
    ///*******************************************************************
    #region Metodos 

    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                Div_Grid_Requisiciones.Visible = true;
                Div_Datos_Generales.Visible = false;

                Configuracion_Acceso("Frm_Rpt_Com_Reporte_Requisiciones.aspx");
                Configuracion_Acceso_LinkButton("Frm_Rpt_Com_Reporte_Requisiciones.aspx");
                Btn_Exportar_Excel.Visible = false;
                Btn_Exportar_PDF.Visible = false;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Inicio";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                break;
            case "General":
                Div_Datos_Generales.Visible = true;
                Div_Grid_Requisiciones.Visible = false;

                Configuracion_Acceso("Frm_Rpt_Com_Reporte_Requisiciones.aspx");
                Configuracion_Acceso_LinkButton("Frm_Rpt_Com_Reporte_Requisiciones.aspx");
                Btn_Exportar_Excel.Visible = true;
                Btn_Exportar_PDF.Visible = true;
                //Boton Salir
                Btn_Salir.Visible = true;
                Btn_Salir.ToolTip = "Listado";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                break;

        }
    }

    public void Habilitar_Formas(bool Estatus)
    {
        //Esto es solo cuando se quiera deshabilitar la forma, ya que el habilitar influyen muchos factores
        if (Estatus == false)
        {
            Div_Grid_Comentarios.Visible = Estatus;
            Div_Grid_Productos.Visible = Estatus;
            TabPnl_Historial_Estatus.Enabled = true;
            TabPnl_Productos.Enabled = Estatus;
            TabPanel_Historial_Comentarios.Enabled = Estatus;
            TabPanel_Detalle_Compra.Enabled = Estatus;
        }// fin del if 
        
    }
    #region Metodos ModalPopUp

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
            Cmb_Estatus_Busqueda.Items.Add("AUTORIZADA");
            Cmb_Estatus_Busqueda.Items.Add("CANCELADA");
            Cmb_Estatus_Busqueda.Items.Add("FILTRADA");
            Cmb_Estatus_Busqueda.Items.Add("COMPRA");
            Cmb_Estatus_Busqueda.Items.Add("COTIZADA");
            Cmb_Estatus_Busqueda.Items[0].Value = "0";
            Cmb_Estatus_Busqueda.Items[0].Selected = true;
        }
    }



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Areas
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Areas que esta dentro del ModalPopup
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Dependencias()
    {
        Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
        Cmb_Dependencia.Items.Clear();
        DataTable Data_Table = Requisicion_Negocio.Consulta_Dependencias();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Dependencia, Data_Table);
        Cmb_Dependencia.SelectedIndex = 0;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Areas
    ///DESCRIPCIÓN: Metodo que llena el combo Cmb_Areas que esta dentro del ModalPopup
    ///PARAMETROS:  
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Areas()
    {
        Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
        Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
        Cmb_Area.Items.Clear();
        DataTable Data_Table = Requisicion_Negocio.Consulta_Areas();
        Cls_Util.Llenar_Combo_Con_DataTable(Cmb_Area, Data_Table);
        Cmb_Area.SelectedIndex = 0;
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
    public Cls_Rpt_Com_Reporte_Requisiciones_Negocio Verificar_Fecha(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
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
                    Requisicion_Negocio.P_Fecha_Inicial = Formato_Fecha(Txt_Fecha_Inicial.Text);
                    Requisicion_Negocio.P_Fecha_Final = Formato_Fecha(Txt_Fecha_Final.Text);
                    Session["Descripcion"] = Session["Descripcion"].ToString() + ", De la Fecha " + Txt_Fecha_Inicial.Text + " a " + Txt_Fecha_Final.Text;
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
        return Requisicion_Negocio;

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

    public Cls_Rpt_Com_Reporte_Requisiciones_Negocio Validar_Estatus_Busqueda(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
    {

        if (Chk_Estatus.Checked == true)
        {
            if (Cmb_Estatus_Busqueda.SelectedIndex != 0)
            {
                Requisicion_Negocio.P_Estatus = Cmb_Estatus_Busqueda.SelectedValue;
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text += "+ Debe seleccionar un estatus <br />";
            }

        }
        else
        {
            Requisicion_Negocio.P_Estatus = null;
        }
        return Requisicion_Negocio;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Area
    ///DESCRIPCIÓN: Metodo que valida que seleccione un area dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public Cls_Rpt_Com_Reporte_Requisiciones_Negocio Validar_Dependencia(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
    {

        if (Chk_Dependencia.Checked == true)
        {
            if (Cmb_Dependencia.SelectedIndex != 0)
            {
                Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text = Lbl_Error_Busqueda.Text + "+ Debe seleccionar una Dependencia <br />";
            }
        }
        else
        {
            Requisicion_Negocio.P_Area_ID = null;
        }
        return Requisicion_Negocio;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Area
    ///DESCRIPCIÓN: Metodo que valida que seleccione un area dentro del modalpopup
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 27/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************

    public Cls_Rpt_Com_Reporte_Requisiciones_Negocio Validar_Area(Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio)
    {

        if (Chk_Area.Checked == true)
        {
            if (Cmb_Area.SelectedIndex != 0)
            {
                Requisicion_Negocio.P_Area_ID = Cmb_Area.SelectedValue;
            }
            else
            {
                Img_Error_Busqueda.Visible = true;
                Lbl_Error_Busqueda.Text = Lbl_Error_Busqueda.Text + "+ Debe seleccionar un area <br />";
            }
        }
        else
        {
            Requisicion_Negocio.P_Area_ID = null;
        }
        return Requisicion_Negocio;
    }

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
        Llenar_Combo_Dependencias();
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
        Chk_Area.Checked = false;
        Cmb_Area.Enabled = false;
        //Ponemos por default la dependencia a la que pertenece el usuario
        Chk_Dependencia.Checked = false;
        Cmb_Dependencia.Enabled = false;
        Chk_Estatus.Checked = false;
        Cmb_Estatus_Busqueda.Enabled = false;
        Cmb_Estatus_Busqueda.SelectedIndex = 0;
        Chk_Fecha.Checked = false;
        Txt_Fecha_Inicial.Enabled = false;
        Txt_Fecha_Final.Enabled = false;
    }
    #endregion

    public void Limpiar_Formas()
    {
        ///**********************************************************************************
        ///LLENAMOS LOS DATOS GENERALES
        ///**********************************************************************************
        Txt_Dependencia.Text = "";
        Txt_Area.Text = "";
        Txt_Tipo.Text = "";
        Txt_Folio.Text = "";
        Txt_Estatus.Text = "";
        Txt_Tipo_Articulo.Text = "";
        Chk_Verificacion.Checked = false;
        Txt_Justificacion.Text = "";
        Txt_Especificacion.Text = "";
        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE HISTORIAL ESTATUS
        ///**********************************************************************************
        Txt_Empleado_Construccion.Text = "";
        Txt_Empleado_Genero.Text = "";
        Txt_Empleado_Autorizo.Text = "";
        Txt_Empleado_Filtrado.Text = "";
        Txt_Empleado_Cotizo.Text = "";
        Txt_Empleado_Confirmo.Text = "";
        Txt_Empleado_Surtido.Text = "";
        Txt_Empleado_Distribucion.Text = "";
        Txt_Fecha_Construccion.Text = "";
        Txt_Fecha_Genero.Text = "";
        Txt_Fecha_Autorizo.Text = "";
        Txt_Fecha_Filtrado.Text = "";
        Txt_Fecha_Cotizo.Text = "";
        Txt_Fecha_Confirmo.Text = "";
        Txt_Fecha_Surtido.Text = "";
        Txt_Fecha_Distribucion.Text = "";
        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE DETALLE DE PRODUCTOS
        ///**********************************************************************************
        Grid_Productos.DataSource = new DataTable();
        Grid_Productos.DataBind();
        Txt_Total.Text = "";
        Txt_Total_Cotizado.Text = "";

        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE DETALLE COMPRAS
        ///**********************************************************************************
        Txt_Tipo_Compra.Text = "";
        Txt_Requisicion_Consolidada.Text = "";
        Txt_Clave_Consolidacion.Text = "";
        Txt_Clave_Compra.Text = "";
        
        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE HISTORIAL DE COMENTARIOS
        ///**********************************************************************************
        TabPanel_Historial_Comentarios.Enabled = false;
        Grid_Comentarios.DataSource = new DataTable();
        Grid_Comentarios.DataBind();
        
    }

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
    private void Generar_Reporte_Con_Sub(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Requisicion.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Rpt_Requisicion.pdf";
        Mostrar_Reporte("Rpt_Requisicion.pdf", "PDF");
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

    #endregion

    ///*******************************************************************
    ///GRID
    ///*******************************************************************
    #region Grid

    #region Grid_Requisiciones
    public void Llenar_Grid_Requisiciones()
    {
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        DataTable Dt_Requisicion = Requisicion_Negocio.Consultar_Requisiciones();
        if (Dt_Requisicion.Rows.Count > 0)
        {
            Grid_Requisiciones.DataSource = Dt_Requisicion;
            Grid_Requisiciones.DataBind();
            //Creamos la variable de sesion de DataTable Requisiciones
            Session["Dt_Requisiciones"] = Dt_Requisicion;
        }
        else
        {
            //limpiamos el grin y mostramos mensaje de que no se encontraron requicisiones
            Grid_Requisiciones.DataSource = new DataTable();
            Grid_Requisiciones.DataBind();
            Lbl_Mensaje_Error.Text = "No se encontraron Datos";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    protected void Grid_Requisiciones_SelectedIndexChanged(object sender, EventArgs e)
    {
        GridViewRow selectedRow = Grid_Requisiciones.Rows[Grid_Requisiciones.SelectedIndex];
        Session["No_Requisicion"] = Grid_Requisiciones.SelectedDataKey["NO_REQUISICION"].ToString();
        Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
        Requisicion_Negocio.P_No_Requisicion = Session["No_Requisicion"].ToString();
        //Consultamos todos los detalles de la Requisicion
        DataTable Dt_Requisicion = Requisicion_Negocio.Consultar_Requisiciones();
        Session["Dt_Requisiciones"] = Dt_Requisicion;
        ///**********************************************************************************
        ///LLENAMOS LOS DATOS GENERALES
        ///**********************************************************************************
        Txt_Dependencia.Text = Dt_Requisicion.Rows[0]["DEPENDENCIA"].ToString();
        Txt_Area.Text = Dt_Requisicion.Rows[0]["AREA"].ToString();
        Txt_Tipo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo].ToString();
        Txt_Folio.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Folio].ToString();
        Txt_Estatus.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Estatus].ToString();
        Txt_Tipo_Articulo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo_Articulo].ToString();
        if (Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Verificaion_Entrega].ToString().Trim() == "SI")
        {
            Chk_Verificacion.Checked = true;
        }
        else
        {
            Chk_Verificacion.Checked = false;
        }
        Txt_Justificacion.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Justificacion_Compra].ToString();
        Txt_Especificacion.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Especificacion_Prod_Serv].ToString();
        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE HISTORIAL ESTATUS
        ///**********************************************************************************
        Txt_Empleado_Construccion.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Construccion_ID].ToString();
        Txt_Empleado_Genero.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Generacion_ID].ToString();
        Txt_Empleado_Autorizo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Autorizacion_ID].ToString();
        Txt_Empleado_Filtrado.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Filtrado_ID].ToString();
        Txt_Empleado_Cotizo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Cotizacion_ID].ToString();
        Txt_Empleado_Confirmo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Confirmacion_ID].ToString();
        Txt_Empleado_Surtido.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Surtido_ID].ToString();
        Txt_Empleado_Distribucion.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Empleado_Distribucion_ID].ToString();
        Txt_Fecha_Construccion.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Construccion].ToString();
        Txt_Fecha_Genero.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Generacion].ToString();
        Txt_Fecha_Autorizo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Autorizacion].ToString();
        Txt_Fecha_Filtrado.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Filtrado].ToString();
        Txt_Fecha_Cotizo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Cotizacion].ToString();
        Txt_Fecha_Confirmo.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Confirmacion].ToString();
        Txt_Fecha_Surtido.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Surtido].ToString();
        Txt_Fecha_Distribucion.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Fecha_Distribucion].ToString();
        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE DETALLE DE PRODUCTOS
        ///**********************************************************************************
        ///asignamos a la clase de negocio el tipo de articulo que es para hacer la consulta de producto o servicio dependiendo
        Requisicion_Negocio.P_Tipo_Articulo = Txt_Tipo_Articulo.Text.Trim();
        DataTable Dt_Productos = Requisicion_Negocio.Consultar_Productos();
        if (Dt_Productos.Rows.Count > 0)
        {
            TabPnl_Productos.Enabled = true;
            Div_Grid_Productos.Visible = true;
            //llenamos el grid de Productos
            Grid_Productos.Visible = true;
            Grid_Productos.DataSource = Dt_Productos;
            Grid_Productos.DataBind();
            Session["Dt_Productos"] = Dt_Productos;
        }
        else
        {
            TabPnl_Productos.Enabled = false;
            Grid_Productos.DataSource = new DataTable();
            Grid_Productos.DataBind();
            Session["Dt_Productos"] = Dt_Productos;
        }
        Txt_Total.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total].ToString();
        Txt_Total_Cotizado.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Total_Cotizado].ToString();
        
        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE DETALLE COMPRAS
        ///**********************************************************************************
        TabPanel_Detalle_Compra.Enabled = true;
        Txt_Tipo_Compra.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Tipo_Compra].ToString();
        Txt_Requisicion_Consolidada.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_Consolidada].ToString();
        Txt_Clave_Consolidacion.Text = Dt_Requisicion.Rows[0][Ope_Com_Requisiciones.Campo_No_Consolidacion].ToString();
        Requisicion_Negocio.P_Tipo_Compra = Txt_Tipo_Compra.Text.Trim();
        DataTable Dt_Compras = Requisicion_Negocio.Consultar_Detalle_Compra();
        Session["Dt_Compras"] = Dt_Compras;
        if (Dt_Compras.Rows.Count > 0)
        {
            Txt_Clave_Compra.Text = Dt_Compras.Rows[0]["FOLIO"].ToString();
            
        }
        ///**********************************************************************************
        ///LLENAMOS LA PESTAÑA DE HISTORIAL DE COMENTARIOS
        ///**********************************************************************************
        DataTable Dt_Comentarios = Requisicion_Negocio.Consultar_Comentarios();
        Session["Dt_Comentarios"] = Dt_Comentarios;
        if (Dt_Comentarios.Rows.Count > 0)
        {
            TabPanel_Historial_Comentarios.Enabled = true;
            Div_Grid_Comentarios.Visible = true;
            Grid_Comentarios.DataSource = Dt_Comentarios;
            Grid_Comentarios.DataBind();
        }
        else
        {
            TabPanel_Historial_Comentarios.Enabled = false;
            Div_Grid_Comentarios.Visible = false;
            Grid_Comentarios.DataSource = new DataTable();
            Grid_Comentarios.DataBind();
        }
        Configurar_Formulario("General");

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
    protected void Grid_Requisiciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Requisiciones.PageIndex = e.NewPageIndex;
        Grid_Requisiciones.DataSource = (DataTable)Session["Dt_Requisiciones"];
        Grid_Requisiciones.DataBind();
    }

    #endregion


    #endregion

    ///*******************************************************************
    ///EVENTOS
    ///*******************************************************************
    #region Eventos

    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Btn_Salir.ToolTip = "Inicio";
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Limpiar_Formas();
                Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
                Llenar_Grid_Requisiciones();

                break;
        }
    }


    #region Componentes ModalPopUp
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Fecha_CheckedChanged
    ///DESCRIPCIÓN: 
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
        if (Chk_Fecha.Checked == false)
        {
            Txt_Fecha_Inicial.Enabled = false;
            Txt_Fecha_Final.Enabled = false;
            Btn_Fecha_Inicio.Enabled = false;
            Btn_Fecha_Inicio.Enabled = false;
        }
        else
        {
            Txt_Fecha_Inicial.Enabled = true;
            Txt_Fecha_Final.Enabled = true;
            Btn_Fecha_Inicio.Enabled = true;
            Btn_Fecha_Inicio.Enabled = true;
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Estatus_CheckedChanged
    ///DESCRIPCIÓN: Evento del Check Estatus del ModalPOpUP
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
        if (Chk_Estatus.Checked == false)
        {
            Cmb_Estatus_Busqueda.Enabled = false;
            Cmb_Estatus_Busqueda.SelectedIndex = 0;
            Txt_Fecha_Inicial.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Final.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        }
        else
        {
            Cmb_Estatus_Busqueda.Enabled = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Dependencia_CheckedChanged
    ///DESCRIPCIÓN: evento del Check Areas en el ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Dependencia_CheckedChanged(object sender, EventArgs e)
    {
        Modal_Busqueda.Show();
        if (Chk_Dependencia.Checked == true)
        {
            Cmb_Dependencia.Enabled = true;
        }
        else
        {
            Cmb_Dependencia.Enabled = false;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencia_SelectedIndexChanged
    ///DESCRIPCIÓN: evento del Cmb_Dependencias en el ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
        Modal_Busqueda.Show();
        if (Cmb_Dependencia.SelectedIndex != 0)
        {
            Requisicion_Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedValue;
            Chk_Area.Checked = false;
            Cmb_Area.Enabled = false;
            Llenar_Combo_Areas();
        }

    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Area_CheckedChanged
    ///DESCRIPCIÓN: evento del Check Areas en el ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Area_CheckedChanged(object sender, EventArgs e)
    {
        Modal_Busqueda.Show();
        if (Chk_Area.Checked == false)
        {
            Cmb_Area.Enabled = false;
        }
        else
        {
            Cmb_Area.Enabled = true;
        }
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Avanzada_Click
    ///DESCRIPCIÓN: Evento del boton busqueda avanzada 
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Pnl_Busqueda.Visible = true;
        Carga_Componentes_Busqueda();
        Modal_Busqueda.Show();       

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
        Lbl_Error_Busqueda.Text = "";
        Img_Error_Busqueda.Visible = false;
        Modal_Busqueda.Hide();


    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Aceptar_Click
    ///DESCRIPCIÓN: Evento del boton Aceptar del ModalPopUp
    ///PARAMETROS:   
    ///CREO: Susana Trigueros Armenta
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Aceptar_Click(object sender, EventArgs e)
    {
        //Creamos el objeto de la clase de negocios 
        Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
        //Validamos Si esta seleccionada una Fecha y que esta sea valida
        Img_Error_Busqueda.Visible = false;
        Lbl_Error_Busqueda.Text = "";
        Requisicion_Negocio = Validar_Estatus_Busqueda(Requisicion_Negocio);
        Requisicion_Negocio = Validar_Dependencia(Requisicion_Negocio);
        Requisicion_Negocio = Validar_Area(Requisicion_Negocio);
        Requisicion_Negocio = Verificar_Fecha(Requisicion_Negocio);
        if ((Chk_Area.Checked == false) && (Chk_Dependencia.Checked == false) && (Chk_Estatus.Checked == false) && (Chk_Fecha.Checked == false))
        {
            Img_Error_Busqueda.Visible = true;
            Lbl_Error_Busqueda.Text += "+ Debe seleccionar una opcion <br />";
        }
        if (Img_Error_Busqueda.Visible == false)
        {
            Modal_Busqueda.Hide();
            Requisicion_Negocio.Consultar_Requisiciones();
            Llenar_Grid_Requisiciones();
        }
        else
        {
            Modal_Busqueda.Show();
        }

    }
    #endregion

    protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
    {
        //Juntamos los 4 datatable en un solo data set
        DataSet Ds_Reporte_Requisicion = new DataSet();
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        DataTable Dt_Compras = (DataTable)Session["Dt_Compras"];
        DataTable Dt_Comentarios = (DataTable)Session["Dt_Comentarios"];
        Dt_Requisiciones.TableName = "Dt_Requisicion";
        Dt_Productos.TableName = "Dt_Prod";
        Dt_Compras.TableName = "Dt_Compras";
        Dt_Comentarios.TableName = "Dt_Comentarios";

        Ds_Reporte_Requisicion.Tables.Add(Dt_Requisiciones.Copy());
        Ds_Reporte_Requisicion.Tables.Add(Dt_Productos.Copy());
        Ds_Reporte_Requisicion.Tables.Add(Dt_Compras.Copy());
        Ds_Reporte_Requisicion.Tables.Add(Dt_Comentarios.Copy());

        Ds_Ope_Rpt_Requisiciones Ds_Objeto_Data_Set = new Ds_Ope_Rpt_Requisiciones();
        Generar_Reporte_Con_Sub(Ds_Reporte_Requisicion, Ds_Objeto_Data_Set, "Rpt_Com_Requisiciones.rpt");
        //limpiamos variables de sesion
        Session["Dt_Requisiciones"] = null;
        Session["Dt_Productos"] = null;
        Session["Dt_Compras"] = null;
        Session["Dt_Comentarios"] = null;
        Configurar_Formulario("Inicio");
        Limpiar_Formas();
    }
    protected void Btn_Exportar_Excel_Click(object sender, EventArgs e)
    {
        //Juntamos los 4 datatable en un solo data set
        DataSet Ds_Reporte_Requisicion = new DataSet();
        DataTable Dt_Requisiciones = (DataTable)Session["Dt_Requisiciones"];
        DataTable Dt_Productos = (DataTable)Session["Dt_Productos"];
        DataTable Dt_Compras = (DataTable)Session["Dt_Compras"];
        DataTable Dt_Comentarios = (DataTable)Session["Dt_Comentarios"];
        Dt_Requisiciones.TableName = "Dt_Requisicion";
        Dt_Productos.TableName = "Dt_Prod";
        Dt_Compras.TableName = "Dt_Compras";
        Dt_Comentarios.TableName = "Dt_Comentarios";

        Ds_Reporte_Requisicion.Tables.Add(Dt_Requisiciones.Copy());
        Ds_Reporte_Requisicion.Tables.Add(Dt_Productos.Copy());
        Ds_Reporte_Requisicion.Tables.Add(Dt_Compras.Copy());
        Ds_Reporte_Requisicion.Tables.Add(Dt_Comentarios.Copy());

        Ds_Ope_Rpt_Requisiciones Ds_Objeto_Data_Set = new Ds_Ope_Rpt_Requisiciones();
        Exportar_Excel(Ds_Reporte_Requisicion, Ds_Objeto_Data_Set, "Rpt_Com_Requisiciones.rpt", "Rpt_Requisicion.xls");
        //limpiamos variables de sesion
        Session["Dt_Requisiciones"] = null;
        Session["Dt_Productos"] = null;
        Session["Dt_Compras"] = null;
        Session["Dt_Comentarios"] = null;
        Configurar_Formulario("Inicio");
        Limpiar_Formas();
    }

    protected void Btn_Buscar_Click(object sender, EventArgs e)
    {
        Cls_Rpt_Com_Reporte_Requisiciones_Negocio Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
        Lbl_Mensaje_Error.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
        if (Txt_Busqueda.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text = "Es necesario indicar el folio de la requisicion a buscar";
            Div_Contenedor_Msj_Error.Visible = true;
        }

        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            Requisicion_Negocio.P_Folio_Busqueda = Txt_Busqueda.Text.Trim();
            DataTable Dt_Requisicion = Requisicion_Negocio.Consultar_Requisiciones();

            if (Dt_Requisicion.Rows.Count > 0)
            {
                Grid_Requisiciones.DataSource = Dt_Requisicion;
                Grid_Requisiciones.DataBind();
                //Creamos la variable de sesion de DataTable Requisiciones
                Session["Dt_Requisiciones"] = Dt_Requisicion;

            }
            else
            {
                //limpiamos el grin y mostramos mensaje de que no se encontraron requicisiones
                Grid_Requisiciones.DataSource = new DataTable();
                Grid_Requisiciones.DataBind();
                Lbl_Mensaje_Error.Text = "No se encontraron Datos";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        Requisicion_Negocio = new Cls_Rpt_Com_Reporte_Requisiciones_Negocio();
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
            Botones.Add(Btn_Exportar_Excel);
            Botones.Add(Btn_Exportar_PDF);
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
