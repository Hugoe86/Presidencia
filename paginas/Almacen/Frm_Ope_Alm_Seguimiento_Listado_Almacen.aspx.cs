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
using Presidencia.Seguimiento_Listado.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_Almacen_Frm_Ope_Alm_Seguimiento_Listado_Almacen : System.Web.UI.Page
{

    ///*******************************************************************************
    ///PAGE_LOAD
    ///*******************************************************************************
    #region PAGE_LOAD
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configurar_Formulario("Inicio");
            Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocio = new Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio();
            Llenar_Grid_Listado(Clase_Negocio);
        }
    }
    #endregion

    ///*******************************************************************************
    ///METODOS
    ///*******************************************************************************
    #region Metodos

    public void Configurar_Formulario(String Estatus)
    {
        switch (Estatus)
        {
            case "Inicio":
                Div_Busqueda_Avanzada.Visible = false;
                Div_Listados.Visible = false;
                Div_Grid_Listado.Visible = true;
                Btn_Salir.ToolTip = "Inicio";

                break;
            case "General":
                Div_Busqueda_Avanzada.Visible = false;
                Div_Listados.Visible = true;
                Div_Grid_Listado.Visible = false;
                Btn_Salir.ToolTip = "Listado";

                break;
        }

    }

    public void Limpiar_Formulario()
    {
        Txt_Folio.Text = "";
        Txt_Partida_Especifica.Text = "";
        Txt_Estatus.Text = "";
        Txt_Tipo.Text = "";
        Txt_Fecha_Construccion.Text = "";
        Txt_Empleado_Construccion.Text = "";
        Txt_Fecha_Generacion.Text = "";
        Txt_Empleado_Genero.Text = "";
        Txt_Fecha_Autorizo.Text = "";
        Txt_Empleado_Autorizo.Text = "";
        Txt_Fecha_Cancelacion.Text = "";
        Txt_Empleado_Cancelacion.Text = "";
        Grid_Requisicion.DataSource = new DataTable();


    }




    #endregion

    ///*******************************************************************************
    ///GRID
    ///*******************************************************************************
    #region Grid

    public void Llenar_Grid_Listado(Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocios)
    {
        DataTable Dt_Seguimiento_Listado = Clase_Negocios.Consulta_Listado();
        if (Dt_Seguimiento_Listado.Rows.Count != 0)
        {
            Grid_Listado.DataSource = Dt_Seguimiento_Listado;
            Grid_Listado.DataBind();
            Session["Dt_Seguimiento_Listado"] = Dt_Seguimiento_Listado;

        }
        else
        {
            Grid_Listado.EmptyDataText = "No se han encontrado registros.";
            //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
            Grid_Listado.DataSource = new DataTable();
            Grid_Listado.DataBind();
        }
    }

    protected void Grid_Listado_SelectedIndexChanged(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocio = new Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio();
        Clase_Negocio.P_Listado_ID = Grid_Listado.SelectedDataKey["Listado_ID"].ToString();
        Session["Listado_ID"] = Clase_Negocio.P_Listado_ID;
        DataTable Dt_Detalle_Listado = Clase_Negocio.Consulta_Listado();
        Configurar_Formulario("General");
        if (Dt_Detalle_Listado.Rows.Count != 0)
        {
            Txt_Folio.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Folio].ToString().Trim();
            Txt_Partida_Especifica.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Requisiciones.Campo_Partida_ID].ToString().Trim();
            Txt_Estatus.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Estatus].ToString().Trim();;
            Txt_Tipo.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Tipo].ToString().Trim();;
            Txt_Fecha_Construccion.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Fecha_Construccion].ToString().Trim();;
            Txt_Empleado_Construccion.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Empleado_Construccion_ID].ToString().Trim();;
            Txt_Fecha_Generacion.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Fecha_Generacion].ToString().Trim();;
            Txt_Empleado_Genero.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Empleado_Generacion_ID].ToString().Trim();;
            Txt_Fecha_Autorizo.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Fecha_Autorizacion].ToString().Trim();;
            Txt_Empleado_Autorizo.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Empleado_Autorizacion_ID].ToString().Trim();;
            Txt_Fecha_Cancelacion.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Fecha_Cancelacion].ToString().Trim();;
            Txt_Empleado_Cancelacion.Text = Dt_Detalle_Listado.Rows[0][Ope_Com_Listado.Campo_Empleado_Cancelacion_ID].ToString().Trim();;
            Grid_Requisicion.DataSource = new DataTable();
            //Consultamos las requisiciones del Listado Almacen
            DataTable Dt_Requisiciones_Listado = Clase_Negocio.Consulta_Requisiciones_Listado();
            if (Dt_Requisiciones_Listado.Rows.Count != 0)
            {
                Grid_Requisicion.DataSource = Dt_Requisiciones_Listado;
                Grid_Requisicion.DataBind();
            }
            else
            {
                Grid_Requisicion.EmptyDataText = "No se han encontrado requisiciones.";
                //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
                Grid_Requisicion.DataSource = new DataTable();
                Grid_Requisicion.DataBind();
            }
            DataTable Dt_Productos = Clase_Negocio.Consulta_Detalles_Listado();
            if (Dt_Productos.Rows.Count != 0)
            {
                Grid_Productos.DataSource = Dt_Productos;
                Grid_Productos.DataBind();
            }
            else
            {
                Grid_Productos.EmptyDataText = "No se han encontrado productos.";
                //Lbl_Mensaje_Error.Text = "+ No se encontraron datos <br />";
                Grid_Productos.DataSource = new DataTable();
                Grid_Productos.DataBind();
            }


        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar la informacion";
        }
        Div_Busqueda_Avanzada.Visible = false;


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
    public Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Verificar_Fecha(TextBox Fecha_Inicial, TextBox Fecha_Final, Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocio)
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
                Clase_Negocio.P_Fecha_Inicial = Formato_Fecha(Fecha_Inicial.Text);
                Clase_Negocio.P_Fecha_Final = Formato_Fecha(Fecha_Final.Text);

            }
            else
            {
                Div_Contenedor_Msj_Error.Visible = true;
                Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
            }
        }
        else
        {
            Div_Contenedor_Msj_Error.Visible = true;
            Lbl_Mensaje_Error.Text += "+ Fecha no valida <br />";
        }

        return Clase_Negocio;
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
        String File_Path = Server.MapPath("../Rpt/Almacen/" + Nombre_Reporte);
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

    ///*******************************************************************************
    ///EVENTOS
    ///*******************************************************************************
    #region Eventos

    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Div_Busqueda_Avanzada.Visible = true;
        Div_Grid_Listado.Visible = true;
        Div_Listados.Visible = false;
        //Limpiamos las cajas de Texto y la fecha la llenamos 
        Txt_Folio_Busqueda.Text = "";
        Cmb_Tipo.SelectedIndex = 0;
        Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Chk_Fecha_Busqueda.Checked = false;
        Txt_Fecha_Inicio.Enabled = false;
        Txt_Fecha_Fin.Enabled = false;
        Btn_Fecha_Inicio.Enabled = false;
        Btn_Fecha_Fin.Enabled = false;


    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        switch (Btn_Salir.ToolTip)
        {
            case "Inicio":
                Limpiar_Formulario();
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");

                break;
            case "Listado":
                Configurar_Formulario("Inicio");
                Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocios = new Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio();
                Llenar_Grid_Listado(Clase_Negocios);
                Limpiar_Formulario();

                break;

        }
    }

    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocio = new Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio();
        if (Txt_Folio_Busqueda.Text != String.Empty)
        {
            Clase_Negocio.P_Folio_Busqueda = Txt_Folio_Busqueda.Text.Trim();
        }
        if (Cmb_Tipo.SelectedIndex != 0)
        {
            Clase_Negocio.P_Tipo = Cmb_Tipo.SelectedValue;
        }
        if (Chk_Fecha_Busqueda.Checked == true)
        {
            Clase_Negocio = Verificar_Fecha(Txt_Fecha_Inicio, Txt_Fecha_Fin, Clase_Negocio);
        }
        if (Div_Contenedor_Msj_Error.Visible == false)
        {

            Llenar_Grid_Listado(Clase_Negocio);
            Div_Busqueda_Avanzada.Visible = false;
        }
        

    }


    protected void Btn_Limpiar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Folio_Busqueda.Text = "";
        Cmb_Tipo.SelectedIndex = 0;
        Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Chk_Fecha_Busqueda.Checked = false;
        Txt_Fecha_Inicio.Enabled = false;
        Txt_Fecha_Fin.Enabled = false;
        Btn_Fecha_Inicio.Enabled = false;
        Btn_Fecha_Fin.Enabled = false;

    }

    protected void Btn_Cerrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Folio_Busqueda.Text = "";
        Cmb_Tipo.SelectedIndex = 0;
        Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
        Div_Busqueda_Avanzada.Visible = false;
        Chk_Fecha_Busqueda.Checked = false;
        Txt_Fecha_Inicio.Enabled = false;
        Txt_Fecha_Fin.Enabled = false;
        Btn_Fecha_Inicio.Enabled = false;
        Btn_Fecha_Fin.Enabled = false;
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }

    protected void Chk_Fecha_Busqueda_CheckedChanged(object sender, EventArgs e)
    {
        if (Chk_Fecha_Busqueda.Checked == true)
        {
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Inicio.Enabled = true;
            Txt_Fecha_Fin.Enabled = true;
            Btn_Fecha_Inicio.Enabled = true;
            Btn_Fecha_Fin.Enabled = true;
        }
        else
        {
            Txt_Fecha_Inicio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Fin.Text = DateTime.Now.ToString("dd/MMM/yyyy");
            Txt_Fecha_Inicio.Enabled = false;
            Txt_Fecha_Fin.Enabled = false;
            Btn_Fecha_Inicio.Enabled = false;
            Btn_Fecha_Fin.Enabled = false;

        }
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        if (Txt_Folio.Text.Trim() == String.Empty)
        {
            Lbl_Mensaje_Error.Text = "Es necesario seleccionar un Listado Almacen ";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        if (Div_Contenedor_Msj_Error.Visible == false)
        {
            //Realizamos la Consulta del Listado para realizar el reporte. 
            Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio Clase_Negocio = new Cls_Ope_Alm_Seguimiento_Listado_Almacen_Negocio();
            Clase_Negocio.P_Listado_ID = Session["Listado_ID"].ToString().Trim();
            DataTable DT_LISTADO = Clase_Negocio.Consulta_Listado_Reporte();
            DataTable DT_DETALLE_LISTADO = Clase_Negocio.Consulta_Listado_Detalles_Reporte();
            DataSet Ds_Imprimir_Listado = new DataSet();
            Ds_Imprimir_Listado.Tables.Add(DT_LISTADO.Copy());
            Ds_Imprimir_Listado.Tables[0].TableName = "DT_LISTADO";
            Ds_Imprimir_Listado.AcceptChanges();
            Ds_Imprimir_Listado.Tables.Add(DT_DETALLE_LISTADO.Copy());
            Ds_Imprimir_Listado.Tables[1].TableName = "DT_DETALLE_LISTADO";
            Ds_Imprimir_Listado.AcceptChanges();
            Ds_Alm_Listado_Almacen Obj_Imprimir_Listado = new Ds_Alm_Listado_Almacen();
            Generar_Reporte(Ds_Imprimir_Listado, Obj_Imprimir_Listado, "Rpt_Alm_Listado_Almacen.rpt", "Rpt_Alm_Listado_Almacen.pdf");

        }
    }

    #endregion



   
}
