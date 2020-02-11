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
using Presidencia.Operacion_Predial_Constancias.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using Presidencia.Colonias.Negocios;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using System.Text;
using System.IO;
using DocumentFormat.OpenXml.Packaging;

public partial class paginas_predial_Frm_Ope_Pre_Constanicas_Propiedad : System.Web.UI.Page
{

    #region Pago_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Llenar_Tabla_Constancias_Propiedad(0);
            //Cargar_Combo_Colonias();
            //Cargar_Combo_Calles();

            //Scrip para mostrar Ventana Modal de las Tasas de Traslado
            Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE','SUSPENDIDA')";
            Session["TIPO_CONTRIBUYENTE"] = "IN ('PROPIETARIO', 'POSEEDOR')";
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Predio.aspx";
        String Propiedades = ",'height=600,width=800,scrollbars=1');";
        Btn_Detalles_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }

    #endregion

    #region Generar Reporte
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: generar_reporte
    ///DESCRIPCIÓN: caraga el data set fisoco con el cual se genera el reporte especificado
    ///PARAMETROS:  1.- data_set.- contiene la informacion de la consulta a la base de datos
    ///             2.-ds_reporte, objeto que contiene la instancia del Data set fisico del reporte a generar
    ///             3.-nombre_reporte, contiene la ruta del reporte a mostrar en pantalla
    ///CREO: Jacqueline Ramirez
    ///FECHA_CREO: 5/Septiembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Generar_Reporte(DataSet data_set, DataSet ds_reporte, string nombre_reporte)
    {


        ReportDocument reporte = new ReportDocument();
        string filePath = Server.MapPath("../Rpt/Predial/" + nombre_reporte);

        reporte.Load(filePath);

        DataRow renglon;

        for (int i = 0; i < data_set.Tables["Dt_Folio"].Rows.Count; i++)
        {
            renglon = data_set.Tables["Dt_Folio"].Rows[i];
            ds_reporte.Tables["Dt_Folio"].ImportRow(renglon);
        }
        reporte.SetDataSource(ds_reporte);

        //1
        ExportOptions exportOptions = new ExportOptions();
        //2
        DiskFileDestinationOptions diskFileDestinationOptions = new DiskFileDestinationOptions();
        //3
        //4
        diskFileDestinationOptions.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pre_Constancias_Folio.pdf");
        //5
        exportOptions.ExportDestinationOptions = diskFileDestinationOptions;
        //6
        exportOptions.ExportDestinationType = ExportDestinationType.DiskFile;
        //7
        exportOptions.ExportFormatType = ExportFormatType.PortableDocFormat;
        //8
        reporte.Export(exportOptions);
        //9
        string ruta = "../../Reporte/Rpt_Pre_Constancias_Folio.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
    }
    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        //Btn_Nuevo.Visible = true;
        //Btn_Nuevo.AlternateText = "Nuevo";
        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        //Btn_Modificar.Visible = true;
        //Btn_Modificar.AlternateText = "Modificar";
        //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Grid_Constancias_Propiedad.Enabled = Estatus;
        //Grid_Constancias_Propiedad.SelectedIndex = (-1);
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Propietario.Enabled = false;// !Estatus;
        //Txt_Domicilio.Enabled = false;// !Estatus;
        Txt_Realizo.Enabled = false;//!Estatus;
        Txt_Folio.Enabled = false;// !Estatus;
        Txt_Fecha.Enabled = false; //!Estatus;
        Txt_Fecha_Vencimiento.Enabled = false;
        if (Btn_Modificar.AlternateText == "Actualizar")
        {
            Txt_No_Recibo_Pago.Enabled = true;// !Estatus;
            Cmb_Estatus.Enabled = true;
        }
        else
        {
            Txt_No_Recibo_Pago.Enabled = false;// !Estatus;
            Cmb_Estatus.Enabled = false;
        }
        Txt_Observaciones.Enabled = !Estatus;
        Btn_Mostrar_Busqueda_Avanzada.Enabled = !Estatus;
        Btn_Detalles_Cuentas_Predial.Enabled = !Estatus;

        Txt_Propietario.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_Calle.Enabled = false;
        Txt_No_Exterior.Enabled = false;
        Txt_No_Interior.Enabled = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_No_Constancia.Value = "";
        Hdf_Propietario_ID.Value = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Cuenta_Predial.Text = "";
        Txt_Propietario.Text = "";
        //Txt_Domicilio.Text = "";
        Txt_Realizo.Text = "";
        Txt_Folio.Text = "";
        Txt_Fecha.Text = "";
        Txt_Fecha_Vencimiento.Text = "";
        Txt_No_Recibo_Pago.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Propietario.Text = "";
        Txt_Colonia.Text = "";
        Txt_Calle.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Session["Cuenta_Predial"] = null;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Tabla_Constancias_Propiedad
    ///DESCRIPCIÓN          : Llena la tabla de Constancias de Propiedad con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Constancias_Propiedad(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Constancias_Negocio Constancia_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
            Constancia_Propiedad.P_Campos_Dinamicos = "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ") AS Cuenta_Predial, ";
            Constancia_Propiedad.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Nombre + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN(SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Propietario_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Propietario_ID + ")) AS Nombre_Propietario, ";
            Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
            Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
            Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
            Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus;
            Constancia_Propiedad.P_Tipo_Constancia_ID = "00001";
            Constancia_Propiedad.P_Filtros_Dinamicos = Ope_Pre_Constancias.Campo_Tipo_Constancia_ID + " = '00001' AND ";
            Constancia_Propiedad.P_Filtros_Dinamicos += "(" + Ope_Pre_Constancias.Campo_Folio + " LIKE '%" + Txt_Busqueda.Text.Trim().ToUpper() + "%' OR ";
            Constancia_Propiedad.P_Filtros_Dinamicos += "(SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = " + Ope_Pre_Constancias.Tabla_Ope_Pre_Constancias + "." + Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ") LIKE '%" + Txt_Busqueda.Text.Trim().ToUpper() + "%')";
            Constancia_Propiedad.P_Ordenar_Dinamico = Ope_Pre_Constancias.Campo_Anio + " DESC, " + Ope_Pre_Constancias.Campo_No_Constancia + " DESC";
            DataTable Tabla = Constancia_Propiedad.Consultar_Constancias();
            //DataView Vista = new DataView(Tabla);
            //String Expresion_De_Busqueda = string.Format("{0} '%{1}%'", Grid_Constancias_Propiedad.SortExpression, Txt_Busqueda.Text.Trim());
            //Vista.RowFilter = Ope_Pre_Constancias.Campo_Folio + " LIKE " + Expresion_De_Busqueda + "OR " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE " + Expresion_De_Busqueda + "";
            Grid_Constancias_Propiedad.DataSource = Tabla;
            Grid_Constancias_Propiedad.PageIndex = Pagina;
            Grid_Constancias_Propiedad.DataBind();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Cargar_Combo_Colonias
    /////DESCRIPCIÓN          : Carga el combo con los datos del catálogo
    /////PARAMETROS           : 
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 12/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Cargar_Combo_Colonias()
    //{
    //    try
    //    {
    //        Cls_Ate_Colonias_Negocio Colonias = new Cls_Ate_Colonias_Negocio();
    //        Colonias.P_Campos_Dinamicos = Cat_Ate_Colonias.Campo_Colonia_ID + " COLONIA_ID, ";
    //        Colonias.P_Campos_Dinamicos += Cat_Ate_Colonias.Campo_Nombre + " NOMBRE";
    //        DataTable Dt_Colonias = Colonias.Consultar_Colonias();
    //        Cmb_Busqueda_Colonia.DataSource = Dt_Colonias;

    //        DataRow Dr_Colonias;
    //        Dr_Colonias = Dt_Colonias.NewRow();
    //        Dr_Colonias["COLONIA_ID"] = HttpUtility.HtmlDecode("00000");
    //        Dr_Colonias["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        Dt_Colonias.Rows.InsertAt(Dr_Colonias, 0);

    //        Cmb_Busqueda_Colonia.DataValueField = "COLONIA_ID";
    //        Cmb_Busqueda_Colonia.DataTextField = "NOMBRE";
    //        Cmb_Busqueda_Colonia.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Cargar_Combo_Calles
    /////DESCRIPCIÓN          : Carga el combo con los datos del catálogo
    /////PARAMETROS           : 
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 12/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Cargar_Combo_Calles()
    //{
    //    try
    //    {
    //        Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
    //        Calles.P_Campos_Dinamicos = Cat_Pre_Calles.Campo_Calle_ID + " CALLE_ID, ";
    //        Calles.P_Campos_Dinamicos += Cat_Pre_Calles.Campo_Nombre + " NOMBRE";
    //        if (Cmb_Busqueda_Colonia.SelectedIndex > 0)
    //        {
    //            Calles.P_Filtros_Dinamicos = Cat_Pre_Calles.Campo_Calle_ID + " IN (SELECT " + Cat_Pre_Calles_Colonias.Campo_Calle_ID + " FROM " + Cat_Pre_Calles_Colonias.Tabla_Cat_Pre_Calles_Colonias + " WHERE  " + Cat_Pre_Calles_Colonias.Campo_Colonia_ID + " IN (SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = " + Cmb_Busqueda_Colonia.SelectedValue + "))";
    //        }
    //        DataTable Dt_Calles = Calles.Consultar_Calles();
    //        Cmb_Busqueda_Calle.DataSource = Dt_Calles;

    //        DataRow Dr_Colonias;
    //        Dr_Colonias = Dt_Calles.NewRow();
    //        Dr_Colonias["CALLE_ID"] = HttpUtility.HtmlDecode("00000");
    //        Dr_Colonias["NOMBRE"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        Dt_Calles.Rows.InsertAt(Dr_Colonias, 0);

    //        Cmb_Busqueda_Calle.DataValueField = "CALLE_ID";
    //        Cmb_Busqueda_Calle.DataTextField = "NOMBRE";
    //        Cmb_Busqueda_Calle.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    protected void Txt_Cuenta_Predial_TextChanged()
    {
        if (Hdf_Cuenta_Predial_ID.Value.Length <= 0)
        {
            Txt_Propietario.Text = "";
            Txt_Colonia.Text = "";
            Txt_Calle.Text = "";
            Txt_No_Exterior.Text = "";
            Txt_No_Interior.Text = "";
        }
        else
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Cuenta = Cuenta.Consultar_Datos_Propietario();
            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Hdf_Cuenta_Predial_ID.Value.Trim() == "" && Txt_Cuenta_Predial.Text != "")
        {
            Consultar_Datos_Cuenta_Predial();
        }
        if (Txt_Cuenta_Predial.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique un Estatus.";
            Validacion = false;
        }
        //if (Btn_Nuevo.AlternateText != "Dar de Alta" && Btn_Modificar.AlternateText == "Actualizar")
        //{
        //    if (Txt_Observaciones.Text.Equals(""))
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Introduzca las Observaciones.";
        //        Validacion = false;
        //    }
        //}
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Constancias_Propiedad_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de los Tipos_Constancias
    ///PARAMETROS:
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_Propiedad_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Constancias_Propiedad.SelectedIndex = (-1);
            Llenar_Tabla_Constancias_Propiedad(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Constancias_Propiedad_RowCommand
    ///DESCRIPCIÓN          : Evento RowCommand para procesas los diferentes botones de comando en el gridview
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_Propiedad_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        switch (e.CommandName)
        {
            case "Print":
                //Cls_Impresion Impresion = new Cls_Impresion();
                //Impresion.Crear_Pagina("HP LaserJet 9000 PCL 6", "Test");
                //Impresion.Printing("HP LaserJet 9000 PCL 6", "F:\\BackUp\\Desk\\reportes astaug abril.pdf");
                if (Grid_Constancias_Propiedad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[6].Text.Trim() == "PAGADA")
                {
                    Grid_Constancias_Propiedad.SelectedIndex = -1;
                    Limpiar_Catalogo();
                    Cls_Ope_Pre_Constancias_Negocio Constancia_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                    DataTable Dt_Constancia_Propiedad;

                    Constancia_Propiedad.P_Campos_Foraneos = true;
                    Constancia_Propiedad.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
                    Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones;
                    Constancia_Propiedad.P_Filtros_Dinamicos = "FOLIO = '" + Grid_Constancias_Propiedad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text.Trim() + "'";
                    Dt_Constancia_Propiedad = Constancia_Propiedad.Consultar_Constancias();

                    if (Dt_Constancia_Propiedad != null)
                    {
                        Hdf_No_Constancia.Value = Dt_Constancia_Propiedad.Rows[0]["NO_CONSTANCIA"].ToString();
                        Hdf_Cuenta_Predial_ID.Value = Dt_Constancia_Propiedad.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                        Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Constancia_Propiedad.Rows[0]["ESTATUS"].ToString()));
                        Txt_Cuenta_Predial.Text = Dt_Constancia_Propiedad.Rows[0]["Cuenta_Predial"].ToString();
                        Txt_Propietario.Text = Dt_Constancia_Propiedad.Rows[0]["Nombre_Propietario"].ToString();
                        //Txt_Domicilio.Text = Dt_Constancia_Propiedad.Rows[0]["Domicilio_Propietario"].ToString();
                        Txt_Realizo.Text = Dt_Constancia_Propiedad.Rows[0]["Nombre_Realizo"].ToString();
                        Txt_Folio.Text = Dt_Constancia_Propiedad.Rows[0]["FOLIO"].ToString();
                        Txt_Fecha_Vencimiento.Text = String.Format("{0:dd/MMM/yyyy}", Dt_Constancia_Propiedad.Rows[0]["FECHA_VENCIMIENTO"]);
                        Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Dt_Constancia_Propiedad.Rows[0]["FECHA"]);
                        Txt_No_Recibo_Pago.Text = Dt_Constancia_Propiedad.Rows[0]["NO_RECIBO"].ToString();
                        Txt_Observaciones.Text = Dt_Constancia_Propiedad.Rows[0]["OBSERVACIONES"].ToString();
                        if (Dt_Constancia_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString() != "")
                        {
                            //Proteccion_Pago = Dt_Constancia_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString().Split('/');
                            Txt_No_Recibo_Pago.Text = Dt_Constancia_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString();
                        }
                        Txt_Cuenta_Predial_TextChanged();
                    }
                    ///////////Imprimir
                    Imprimir_Constancia();
                    Cls_Ope_Pre_Constancias_Negocio Constancias = new Cls_Ope_Pre_Constancias_Negocio();
                    Constancias.P_Folio = Grid_Constancias_Propiedad.Rows[Convert.ToInt32(e.CommandArgument)].Cells[3].Text.Trim();
                    Constancias.Incrementar_No_Impresiones_Constancia();
                    Constancias.P_Folio = Txt_Folio.Text;
                    Constancias.Constancia_Impresa();
                    Llenar_Tabla_Constancias_Propiedad(0);
                    Limpiar_Catalogo();
                }
                break;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Constancias_Propiedad_SelectedIndexChanged
    ///DESCRIPCIÓN          : Obtiene los datos de un Constancia_Propiedad Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_Propiedad_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Constancias_Propiedad.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String[] Proteccion_Pago;
                Cls_Ope_Pre_Constancias_Negocio Constancia_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                DataTable Dt_Constancia_Propiedad;

                Constancia_Propiedad.P_Campos_Foraneos = true;
                Constancia_Propiedad.P_Campos_Dinamicos = Ope_Pre_Constancias.Campo_No_Constancia + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Estatus + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Cuenta_Predial_ID + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Folio + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Fecha_Vencimiento + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Proteccion_Pago + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_No_Recibo + ", ";
                Constancia_Propiedad.P_Campos_Dinamicos += Ope_Pre_Constancias.Campo_Observaciones;
                Constancia_Propiedad.P_Filtros_Dinamicos = "FOLIO = '" + Grid_Constancias_Propiedad.SelectedRow.Cells[3].Text.Trim() + "'";
                Dt_Constancia_Propiedad = Constancia_Propiedad.Consultar_Constancias();

                if (Dt_Constancia_Propiedad != null)
                {
                    Hdf_No_Constancia.Value = Dt_Constancia_Propiedad.Rows[0]["NO_CONSTANCIA"].ToString();
                    Hdf_Cuenta_Predial_ID.Value = Dt_Constancia_Propiedad.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                    Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Dt_Constancia_Propiedad.Rows[0]["ESTATUS"].ToString()));
                    Txt_Cuenta_Predial.Text = Dt_Constancia_Propiedad.Rows[0]["Cuenta_Predial"].ToString();
                    Txt_Propietario.Text = Dt_Constancia_Propiedad.Rows[0]["Nombre_Propietario"].ToString();
                    //Txt_Domicilio.Text = Dt_Constancia_Propiedad.Rows[0]["Domicilio_Propietario"].ToString();
                    Txt_Realizo.Text = Dt_Constancia_Propiedad.Rows[0]["Nombre_Realizo"].ToString();
                    Txt_Folio.Text = Dt_Constancia_Propiedad.Rows[0]["FOLIO"].ToString();
                    Txt_Fecha_Vencimiento.Text = String.Format("{0:dd/MMM/yyyy}", Dt_Constancia_Propiedad.Rows[0]["FECHA_VENCIMIENTO"]);
                    Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", Dt_Constancia_Propiedad.Rows[0]["FECHA"]);
                    Txt_No_Recibo_Pago.Text = Dt_Constancia_Propiedad.Rows[0]["NO_RECIBO"].ToString();
                    Txt_Observaciones.Text = Dt_Constancia_Propiedad.Rows[0]["OBSERVACIONES"].ToString();
                    if (Dt_Constancia_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString() != "")
                    {
                        Proteccion_Pago = Dt_Constancia_Propiedad.Rows[0]["PROTECCION_PAGO"].ToString().Split('/');
                        Txt_No_Recibo_Pago.Text = Proteccion_Pago[6];
                    }
                    Txt_Cuenta_Predial_TextChanged();
                }
                Btn_Detalles_Cuentas_Predial.Enabled = true;
            }

            Cargar_Ventana_Emergente_Resumen_Predio();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Constancia_Propiedad
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Constancias_Negocio Constancias_Negocio = new Cls_Ope_Pre_Constancias_Negocio();
        Cls_Ope_Pre_Parametros_Negocio Dias_Habiles = new Cls_Ope_Pre_Parametros_Negocio();
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                //DataTable Dt_Ayudante;
                //Cls_Cat_Empleados_Negocios Empleados = new Cls_Cat_Empleados_Negocios();
                //Empleados.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
                //Dt_Ayudante = Empleados.Consulta_Datos_Empleado();
                //Txt_Confronto.Text = Dt_Ayudante.Rows[0][Cat_Empleados.Campo_Confronto].ToString();
                //Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;
                Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;
                String Dias = Convert.ToString(Dias_Habiles.Consultar_Dias_Vencimiento());
                Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                Txt_Fecha_Vencimiento.Text = String.Format("{0:dd/MMM/yyyy}", Constancias_Negocio.Calcular_Fecha(Txt_Fecha.Text.Trim(), Dias));
                //Cmb_Estatus.Enabled = false;
                Cmb_Estatus.SelectedIndex = 1;
                Btn_Detalles_Cuentas_Predial.Enabled = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    Cls_Ope_Pre_Constancias_Negocio Constancia_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                    Constancia_Propiedad.P_Tipo_Constancia_ID = "00001";
                    Constancia_Propiedad.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Constancia_Propiedad.P_Propietario_ID = Hdf_Propietario_ID.Value;
                    Constancia_Propiedad.P_Realizo = Cls_Sessiones.Empleado_ID;
                    //Constancia_Propiedad.P_Folio = Txt_Folio.Text.Trim();
                    Constancia_Propiedad.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text.Trim());
                    Constancia_Propiedad.P_Periodo_Año = DateTime.Now.Year;
                    //Constancia_Propiedad.P_No_Recibo = Txt_No_Recibo_Pago.Text.Trim();
                    Constancia_Propiedad.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                    Constancia_Propiedad.P_Observaciones = Txt_Observaciones.Text.Trim();
                    Constancia_Propiedad.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text.Trim());
                    Constancia_Propiedad.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    if (Constancia_Propiedad.Alta_Constancia())
                    {
                        Insertar_Pasivo(Constancia_Propiedad.P_Folio);
                        Configuracion_Formulario(true);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('Alta de Constancia de Propiedad Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Modificar.Visible = true;
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        //Consultar el precio de la Constancia de propiedad...
                        Cls_Cat_Pre_Tipos_Constancias_Negocio Tipos_Constancias = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                        DataTable Dt_Tipos_Constancias;

                        Tipos_Constancias.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + ", " + Cat_Pre_Tipos_Constancias.Campo_Costo;
                        Tipos_Constancias.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave + "='CP'";

                        Dt_Tipos_Constancias = Tipos_Constancias.Consultar_Tipos_Constancias();

                        Imprimir_Reporte_Folio(Crear_Ds_Constancias(Constancia_Propiedad.P_Folio, Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString()), "Rpt_Ope_Pre_Folio_Constancias.rpt", "Folio_Constancia_Propiedad");
                        Limpiar_Catalogo();
                        Llenar_Tabla_Constancias_Propiedad(Grid_Constancias_Propiedad.PageIndex);
                        Grid_Constancias_Propiedad.SelectedIndex = -1;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('Alta de Constancia de Propiedad No fue Exitosa');", true);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costos del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            Cls_Cat_Pre_Tipos_Constancias_Negocio Tipos_Constancias = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
            DataTable Dt_Clave;
            DataTable Dt_Tipos_Constancias;

            Tipos_Constancias.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + ", " + Cat_Pre_Tipos_Constancias.Campo_Costo;
            Tipos_Constancias.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave + "='CP'";

            Calculo_Impuesto_Traslado.P_Referencia = Referencia;
            Calculo_Impuesto_Traslado.Eliminar_Referencias_Pasivo();

            Dt_Tipos_Constancias = Tipos_Constancias.Consultar_Tipos_Constancias();

            if (Dt_Tipos_Constancias.Rows.Count > 0 && Convert.ToDouble(Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo]) != 0)
            {
                Claves_Ingreso.P_Documento_ID = Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID].ToString();
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    if (Dt_Tipos_Constancias.Rows.Count > 0)
                    {
                        if (Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString() != "")
                        {
                            Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                            Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                            Calculo_Impuesto_Traslado.P_Descripcion = "CONSTANCIA DE PROPIEDAD DE LA CUENTA PREDIAL " + Txt_Cuenta_Predial.Text;
                            Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = "" + Convert.ToDouble(Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString());
                            Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Propietario.Text.ToUpper();
                            Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                            Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                            Calculo_Impuesto_Traslado.Alta_Pasivo();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "El Pasivo no pudo ser insertado: " + Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Constancia_Propiedad.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Constancias_Propiedad.SelectedIndex > -1)
        {
            if (Grid_Constancias_Propiedad.SelectedRow.Cells[6].Text != "PAGADA")
            {
                try
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        if (Grid_Constancias_Propiedad.Rows.Count > 0 && Grid_Constancias_Propiedad.SelectedIndex > (-1))
                        {
                            Btn_Modificar.AlternateText = "Actualizar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            Btn_Salir.AlternateText = "Cancelar";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.Visible = false;
                            Configuracion_Formulario(false);
                            Txt_No_Recibo_Pago.Enabled = false;
                            Btn_Mostrar_Busqueda_Avanzada.Enabled = false;
                            //Img_Calendario.Enabled = false;
                            Cmb_Estatus.Items.Remove("PAGADA");
                        }
                    }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Ope_Pre_Constancias_Negocio Constancia_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
                            Constancia_Propiedad.P_No_Constancia = Hdf_No_Constancia.Value;
                            Constancia_Propiedad.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            Constancia_Propiedad.P_Propietario_ID = Hdf_Propietario_ID.Value;
                            Constancia_Propiedad.P_Realizo = Cls_Sessiones.Empleado_ID;
                            Constancia_Propiedad.P_Folio = Txt_Folio.Text.ToUpper().Trim();
                            //Constancia_Propiedad.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text.Trim());
                            Constancia_Propiedad.P_No_Recibo = Txt_No_Recibo_Pago.Text.Trim();
                            Constancia_Propiedad.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                            Constancia_Propiedad.P_Observaciones = Txt_Observaciones.Text.Trim();
                            Constancia_Propiedad.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            if (Constancia_Propiedad.Modificar_Constancia())
                            {
                                Insertar_Pasivo(Constancia_Propiedad.P_Folio);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('Actualización de Constancia de Propiedad Exitosa');", true);
                                Btn_Modificar.AlternateText = "Modificar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                                Btn_Modificar.Visible = true;
                                Btn_Nuevo.Visible = true;
                                Btn_Salir.AlternateText = "Salir";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                Configuracion_Formulario(true);

                                //Consultar el precio de la Constancia de propiedad...
                                Cls_Cat_Pre_Tipos_Constancias_Negocio Tipos_Constancias = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                                DataTable Dt_Tipos_Constancias;

                                Tipos_Constancias.P_Campos_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + ", " + Cat_Pre_Tipos_Constancias.Campo_Costo;
                                Tipos_Constancias.P_Filtros_Dinamicos = Cat_Pre_Tipos_Constancias.Campo_Clave + "='CP'";

                                Dt_Tipos_Constancias = Tipos_Constancias.Consultar_Tipos_Constancias();

                                Imprimir_Reporte_Folio(Crear_Ds_Constancias(Txt_Folio.Text.Trim(), Dt_Tipos_Constancias.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString()), "Rpt_Ope_Pre_Folio_Constancias.rpt", "Folio_Constancia_Propiedad");
                                Cmb_Estatus.Items.Insert(1, new ListItem("PAGADA", "PAGADA"));
                                Limpiar_Catalogo();
                                Llenar_Tabla_Constancias_Propiedad(Grid_Constancias_Propiedad.PageIndex);
                                Grid_Constancias_Propiedad.SelectedIndex = -1;
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('Actualización de Constancia de Propiedad No fue Exitosa');", true);
                            }
                        }
                    }
                }
                catch (Exception Ex)
                {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('La constancia de propiedad se encuentra pagada.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('Selecciona el Registro que quieres Modificar.');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Constancias_Propiedad.SelectedIndex = (-1);
            Llenar_Tabla_Constancias_Propiedad(0);
            if (Grid_Constancias_Propiedad.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron todos los Constancias de Propiedad encontrados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Constancias_Propiedad(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Click
    ///DESCRIPCIÓN          : Manda Imprimir el reporte
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Hdf_No_Constancia.Value != "")
        {
            Imprimir_Reporte(Crear_Ds_Constancias(0), "Rpt_Pre_Constancias.rpt", "Constancia de Propiedad");
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    /////DESCRIPCIÓN          : Elimina un Constancia_Propiedad de la Base de Datos
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Junio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Constancias_Propiedad.Rows.Count > 0 && Grid_Constancias_Propiedad.SelectedIndex > (-1))
    //        {
    //            Cls_Ope_Pre_Constancias_Negocio Constancia_Propiedad = new Cls_Ope_Pre_Constancias_Negocio();
    //            Constancia_Propiedad.P_Folio = Grid_Constancias_Propiedad.SelectedRow.Cells[3].Text;
    //            if (Constancia_Propiedad.Eliminar_Constancia_Propiedad())
    //            {
    //                Grid_Constancias_Propiedad.SelectedIndex = (-1);
    //                Llenar_Tabla_Constancias_Propiedad(Grid_Constancias_Propiedad.PageIndex);
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('Constancia de Propiedad fue Eliminada Exitosamente');", true);
    //                Limpiar_Catalogo();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Constancias de Propiedad", "alert('La Constancia de Propiedad No fue Eliminada');", true);
    //            }
    //        }
    //        else
    //        {
    //            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
    //            Lbl_Mensaje_Error.Text = "";
    //            Div_Contenedor_Msj_Error.Visible = true;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            if (Btn_Modificar.AlternateText == "Actualizar")
            {
                Cmb_Estatus.Items.Insert(1, new ListItem("PAGADA", "PAGADA"));
            }
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Grid_Constancias_Propiedad.SelectedIndex = -1;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Txt_Cuenta_Predial_TextChanged();
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
                Btn_Detalles_Cuentas_Predial.Enabled = true;
            }
            Consultar_Datos_Cuenta_Predial();
            Cargar_Ventana_Emergente_Resumen_Predio();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Cerrar_Detalle_Cuenta_Predial_Click
    ///DESCRIPCIÓN          : Cierra el Modal PopPup
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cerrar_Detalle_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Mpe_Detalles_Cuenta_Predial.Hide();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Detalles_Constancias_Click
    ///DESCRIPCIÓN          : Consulta el Detalle de la cuenta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Detalles_Constancias_Click(object sender, ImageClickEventArgs e)
    {
        Limpiar_Detalle_Cuenta_Predial();
        Consultar_Detalle_Cuenta_Predial();
        Mpe_Detalles_Cuenta_Predial.Show();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Detalle_Cuenta_Predial
    ///DESCRIPCIÓN          : Limpia los controles del Detalle de la Cuenta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 16/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Detalle_Cuenta_Predial()
    {
        Txt_Detalle_Cuenta_Predial.Text = "";
        Txt_Detalle_Estatus.Text = "";
        Txt_Detalle_Propietatio.Text = "";
        Txt_Detalle_Colonia.Text = "";
        Txt_Detalle_Calle.Text = "";
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Constanica
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Predial()
    {
        DataTable Dt_Cuentas_Predial;
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += "(SELECT " + Cat_Pre_Propietarios.Campo_Propietario_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " AND " + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO') AS PROPIETARIO_ID";
            Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Hdf_Cuenta_Predial_ID.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                //Hdf_Propietario_ID.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();

                DataTable Dt_Contribuyentes;
                Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
                //Consulta los datos del Contribuyente
                Contribuyentes.P_Campos_Dinamicos = "P." + Cat_Pre_Propietarios.Campo_Propietario_ID + ", ";
                Contribuyentes.P_Campos_Dinamicos += "(C." + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + "|| ' ' ||C." + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + "|| ' ' ||C." + Cat_Pre_Contribuyentes.Campo_Nombre + ") AS NOMBRE_COMPLETO, ";
                Contribuyentes.P_Campos_Dinamicos += "(CO." + Cat_Ate_Colonias.Campo_Nombre + "|| ', ' ||CA." + Cat_Pre_Calles.Campo_Nombre + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + "|| ', ' ||CU." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ") AS DOMICILIO";
                Contribuyentes.P_Filtros_Dinamicos = "P." + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO' AND ";
                Contribuyentes.P_Filtros_Dinamicos += "C." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " IN (SELECT " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = '" + Hdf_Cuenta_Predial_ID.Value + "')";
                Contribuyentes.P_Ordenar_Dinamico = "P." + Cat_Pre_Propietarios.Campo_Propietario_ID;
                //Variable solo para guardar el id para la consulta del domicilio...
                Contribuyentes.P_Usuario = Hdf_Cuenta_Predial_ID.Value;
                Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
                if (Dt_Contribuyentes.Rows.Count > 0)
                {
                    Hdf_Propietario_ID.Value = Dt_Contribuyentes.Rows[0]["PROPIETARIO_ID"].ToString();
                    Txt_Propietario.Text = Dt_Contribuyentes.Rows[0]["NOMBRE_COMPLETO"].ToString();
                    //Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0]["DOMICILIO"].ToString();
                }

                //DataTable Dt_Propietarios;
                //Cls_Cat_Pre_Propietarios_Negocio Propietarios = new Cls_Cat_Pre_Propietarios_Negocio();
                ////Consulta los Propietarios de la Cuenta Predial
                //Propietarios.P_Campos_Dinamicos = Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                //Propietarios.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                //Propietarios.P_Propietario_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();
                ////Propietarios.P_Tipo = "PROPIETARIO";
                //Dt_Propietarios = Propietarios.Consultar_Propietario();
                //if (Dt_Propietarios.Rows.Count > 0)
                //{
                //    DataTable Dt_Contribuyentes;
                //    Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
                //    //Consulta los datos del Contribuyente
                //    Contribuyentes.P_Contribuyente_ID = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
                //    Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
                //    if (Dt_Contribuyentes.Rows.Count > 0)
                //    {
                //        if (Propietarios.P_Propietario_ID != "")
                //        {
                //            Hdf_Propietario_ID.Value = Propietarios.P_Propietario_ID;
                //        }
                //        else
                //        {
                //            Hdf_Propietario_ID.Value = Contribuyentes.P_Contribuyente_ID;
                //        }
                //        Txt_Propietario.Text = Dt_Contribuyentes.Rows[0]["NOMBRE_COMPLETO"].ToString();
                //        Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0]["DOMICILIO"].ToString();
                //    }
                //}
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Detalle_Cuenta_Predial
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Detalle_Cuenta_Predial()
    {
        DataTable Dt_Cuentas_Predial;
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                Txt_Detalle_Cuenta_Predial.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                Txt_Detalle_Estatus.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();

                //DataTable Dt_Propietarios;
                //Cls_Cat_Pre_Propietarios_Negocio Propietarios = new Cls_Cat_Pre_Propietarios_Negocio();
                ////Consulta los Propietarios de la Cuenta Predial
                //Propietarios.P_Campos_Dinamicos = Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                //Propietarios.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                //Propietarios.P_Propietario_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();
                ////Propietarios.P_Tipo = "PROPIETARIO";
                //Dt_Propietarios = Propietarios.Consultar_Propietario();
                //if (Dt_Propietarios.Rows.Count > 0)
                //{
                //    DataTable Dt_Contribuyentes;
                //    Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
                //    //Consulta los datos del Contribuyente
                //    Contribuyentes.P_Contribuyente_ID = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
                //    Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
                //    if (Dt_Contribuyentes.Rows.Count > 0)
                //    {
                //        if (Propietarios.P_Propietario_ID != "")
                //        {
                //            Hdf_Propietario_ID.Value = Propietarios.P_Propietario_ID;
                //        }
                //        else
                //        {
                //            Hdf_Propietario_ID.Value = Contribuyentes.P_Contribuyente_ID;
                //        }
                //        Txt_Propietario.Text = Dt_Contribuyentes.Rows[0]["NOMBRE_COMPLETO"].ToString();
                //        Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0]["DOMICILIO"].ToString();
                //    }
                //}
            }
        }
    }

    #endregion

    //#region Modal PopUp's

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Busqueda_Cuentas_Predial_Click
    ///// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de 
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 11/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Cerrar_Busqueda_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    //{
    //    Mpe_Busqueda_Cuentas_Predial.Hide();
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Limpiar_Busqueda_Cuentas_Predial_Click
    ///// 	DESCRIPCIÓN         : Limpia los controles de la búsqeuda avanzada
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 11/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Limpiar_Busqueda_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    //{
    //    Txt_Busqueda_Cuenta_Predial.Text = "";
    //    Cmb_Busqueda_Estatus.SelectedIndex = 0;
    //    Txt_Busqueda_Propietatio.Text = "";
    //    Cmb_Busqueda_Colonia.SelectedIndex = 0;
    //    Cmb_Busqueda_Calle.SelectedIndex = 0;
    //    Mpe_Busqueda_Cuentas_Predial.Show();
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Cmb_Busqueda_Colonia_SelectedIndexChanged
    ///// 	DESCRIPCIÓN         : Carga el combo de Calles filtrando de acuerdo a la Colonia seleccionada
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 11/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Cmb_Busqueda_Colonia_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Cargar_Combo_Calles();
    //    Mpe_Busqueda_Cuentas_Predial.Show();
    //}

    /////*******************************************************************************
    ///// NOMBRE DE LA FUNCION    : Btn_Busqueda_Cuentas_Predial_Click
    ///// DESCRIPCION             : Ejecuta la búsqueda de mediante el modal popup 
    ///// CREO                    : Antonio Salvador Benavides Guardado
    ///// FECHA_CREO              : 11/Julio/2011
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACION:
    /////*******************************************************************************
    //protected void Btn_Busqueda_Cuentas_Predial_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        DataTable Dt_Cuentas_Predial;
    //        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
    //        if (Txt_Busqueda_Cuenta_Predial.Text.Trim() != ""
    //        || Cmb_Busqueda_Estatus.SelectedIndex > 0
    //        || Txt_Busqueda_Propietatio.Text.Trim() != ""
    //        || Cmb_Busqueda_Colonia.SelectedIndex > 0
    //        || Cmb_Busqueda_Calle.SelectedIndex > 0)
    //        {
    //            //Consulta la Cuenta Predial
    //            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
    //            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
    //            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
    //            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Estatus + ", ";
    //            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID + ", ";
    //            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ", ";
    //            Cuentas_Predial.P_Campos_Dinamicos += "(SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " IN (SELECT " + Cat_Pre_Calles_Colonias.Campo_Colonia_ID + " FROM " + Cat_Pre_Calles_Colonias.Tabla_Cat_Pre_Calles_Colonias + " WHERE " + Cat_Pre_Calles_Colonias.Campo_Calle_ID + " = " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Calle_ID + ")) COLONIA_ID";
    //            Cuentas_Predial.P_Filtros_Dinamicos = "";
    //            if (Txt_Busqueda_Cuenta_Predial.Text.Trim() != "")
    //            {
    //                if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
    //                {
    //                    Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
    //                }
    //                Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Busqueda_Cuenta_Predial.Text.Trim() + "'";
    //            }
    //            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
    //            {
    //                if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
    //                {
    //                    Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
    //                }
    //                Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Estatus + " = '" + Cmb_Busqueda_Estatus.SelectedValue + "'";
    //            }
    //            if (Txt_Busqueda_Propietatio.Text.Trim() != "")
    //            {
    //                if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
    //                {
    //                    Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
    //                }
    //                Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Propietario_ID + " IN (SELECT " + Cat_Pre_Propietarios.Campo_Propietario_ID + " FROM " + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE " + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " IN (SELECT " + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " FROM " + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE " + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim() + "%' OR " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim() + "%' OR " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim() + "%' OR " + Cat_Pre_Contribuyentes.Campo_Nombre + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim() + "%' OR " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || " + Cat_Pre_Contribuyentes.Campo_Nombre + " LIKE '%" + Txt_Busqueda_Propietatio.Text.Trim() + "%'))";
    //            }
    //            if (Cmb_Busqueda_Colonia.SelectedIndex > 0)
    //            {
    //                if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
    //                {
    //                    Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
    //                }
    //                Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " IN (SELECT " + Cat_Pre_Calles_Colonias.Campo_Calle_ID + " FROM " + Cat_Pre_Calles_Colonias.Tabla_Cat_Pre_Calles_Colonias + " WHERE " + Cat_Pre_Calles_Colonias.Campo_Colonia_ID + " IN (SELECT " + Cat_Ate_Colonias.Campo_Colonia_ID + " FROM " + Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias + " WHERE " + Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Cmb_Busqueda_Colonia.SelectedValue + "'))";
    //            }
    //            if (Cmb_Busqueda_Calle.SelectedIndex > 0)
    //            {
    //                if (Cuentas_Predial.P_Filtros_Dinamicos.Trim() != "")
    //                {
    //                    Cuentas_Predial.P_Filtros_Dinamicos += " AND ";
    //                }
    //                Cuentas_Predial.P_Filtros_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Calle_ID + " = '" + Cmb_Busqueda_Calle.SelectedValue + "'";
    //            }
    //            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
    //            Grid_Cuentas_Predial.DataSource = Dt_Cuentas_Predial;
    //            Grid_Cuentas_Predial.PageIndex = 0;
    //            Grid_Cuentas_Predial.DataBind();
    //        }
    //        else
    //        {
    //            Grid_Cuentas_Predial.DataSource = null;
    //            Grid_Cuentas_Predial.PageIndex = 0;
    //            Grid_Cuentas_Predial.DataBind();
    //        }
    //        Mpe_Busqueda_Cuentas_Predial.Show();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Grid_Cuentas_Predial_PageIndexChanging
    ///// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Página del Grid de 
    ///// 	PARÁMETROS          :
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 11/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Grid_Cuentas_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        Mpe_Busqueda_Cuentas_Predial.Show();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Grid_Cuentas_Predial_SelectedIndexChanged
    ///// 	DESCRIPCIÓN         : Maneja el Evento de Cambio de Selección del Grid 
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 11/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Grid_Cuentas_Predial_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Txt_Busqueda_Cuenta_Predial.Text = Grid_Cuentas_Predial.SelectedRow.Cells[2].Text.Replace("&nbsp;", "");
    //        if (Grid_Cuentas_Predial.SelectedRow.Cells[9].Text.Trim().Replace("&nbsp;", "") != "")
    //        {
    //            Cmb_Busqueda_Estatus.SelectedValue = Grid_Cuentas_Predial.SelectedRow.Cells[9].Text;
    //        }
    //        else
    //        {
    //            Cmb_Busqueda_Estatus.SelectedIndex = -1;
    //        }
    //        Txt_Busqueda_Propietatio.Text = Grid_Cuentas_Predial.SelectedRow.Cells[4].Text.Replace("&nbsp;", "");
    //        if (Grid_Cuentas_Predial.SelectedRow.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
    //        {
    //            Cmb_Busqueda_Colonia.SelectedValue = Grid_Cuentas_Predial.SelectedRow.Cells[5].Text;
    //        }
    //        else
    //        {
    //            Cmb_Busqueda_Colonia.SelectedIndex = -1;
    //        }
    //        if (Grid_Cuentas_Predial.SelectedRow.Cells[7].Text.Trim().Replace("&nbsp;", "") != "")
    //        {
    //            Cmb_Busqueda_Calle.SelectedValue = Grid_Cuentas_Predial.SelectedRow.Cells[7].Text;
    //        }
    //        else
    //        {
    //            Cmb_Busqueda_Calle.SelectedIndex = -1;
    //        }
    //        Mpe_Busqueda_Cuentas_Predial.Show();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Busqueda_Aceptar_Click
    ///// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de Cuentas predial
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 11/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Busqueda_Aceptar_Click(object sender, EventArgs e)
    //{
    //    Txt_Cuenta_Predial.Text = Txt_Busqueda_Cuenta_Predial.Text;
    //    Consultar_Datos_Cuenta_Predial();
    //    Mpe_Busqueda_Cuentas_Predial.Hide();
    //}

    /////*******************************************************************************************************
    ///// 	NOMBRE_FUNCIÓN      : Btn_Cerrar_Detalle_Cuenta_Predial_Click
    ///// 	DESCRIPCIÓN         : Ocultar el modal popup Busqueda de Cuentas predial
    ///// 	PARÁMETROS:
    ///// 	CREO                : Antonio Salvador Benavides Guardado
    ///// 	FECHA_CREO          : 11/Julio/2011
    ///// 	MODIFICÓ: 
    ///// 	FECHA_MODIFICÓ: 
    ///// 	CAUSA_MODIFICACIÓN: 
    /////*******************************************************************************************************
    //protected void Btn_Cerrar_Detalle_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    //{
    //    Mpe_Detalles_Cuenta_Predial.Hide();
    //}

    /////*******************************************************************************
    ///// NOMBRE DE LA FUNCION    : Btn_Refrescar_Detalle_Cuenta_Predial_Click
    ///// DESCRIPCION             : Ejecuta la búsqueda de Cuentas predial mediante el modal popup 
    ///// CREO                    : Antonio Salvador Benavides Guardado
    ///// FECHA_CREO              : 11/Julio/2011
    ///// MODIFICO          :
    ///// FECHA_MODIFICO    :
    ///// CAUSA_MODIFICACION:
    /////*******************************************************************************
    //protected void Btn_Refrescar_Detalle_Cuenta_Predial_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        Mpe_Detalles_Cuenta_Predial.Show();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}

    //#endregion

    #region Impresion Folios

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 19/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataTable Dt_Constancias, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Dt_Constancias);

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

        Reporte.Export(Export_Options);
        Reporte.PrintToPrinter(1, true, 0, 0);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 19/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Imprimir_Reporte(DataSet Ds_Constancias, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Boolean Impresion_Correcta = false;
        StringBuilder Domicilio = new StringBuilder();

        try
        {
            Reporte.Load(File_Path);
            Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_Propiedad"]);
            Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Propiedad"]);
            Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Adeudo"]);
            Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Ds_Constancias.Tables["Dt_Certificaciones"]);

            String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
            try
            {
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
                try
                {
                    //******** PASAMOS EL DOMICILIO COMO PARÁMETRO AL REPORTE ********
                    Domicilio.Append("Calle ");
                    Domicilio.Append(Txt_Calle.Text);
                    if (Txt_No_Exterior.Text.Trim() != "")
                    {
                        Domicilio.Append(", #" + Txt_No_Exterior.Text.Trim());
                    }
                    if (Txt_No_Interior.Text.Trim() != "")
                    {
                        Domicilio.Append(", #" + Txt_No_Interior.Text);
                    }
                    Domicilio.Append(", Colonia ");
                    Domicilio.Append(Txt_Colonia.Text.Trim());
                    Domicilio.Append(".");

                    ParameterFieldDefinitions Cr_Parametros;
                    ParameterFieldDefinition Cr_Parametro;
                    ParameterValues Cr_Valor_Parametro = new ParameterValues();
                    ParameterDiscreteValue Cr_Valor = new ParameterDiscreteValue();

                    Cr_Parametros = Reporte.DataDefinition.ParameterFields;

                    Cr_Parametro = Cr_Parametros["Domicilio"];
                    Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                    Cr_Valor_Parametro.Clear();

                    Cr_Valor.Value = Domicilio.ToString().Trim();
                    Cr_Valor_Parametro.Add(Cr_Valor);
                    Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);


                    Cr_Parametro = Cr_Parametros["Bimestres"];
                    Cr_Valor_Parametro = Cr_Parametro.CurrentValues;
                    Cr_Valor_Parametro.Clear();

                    Cr_Valor.Value = String.Empty;
                    Cr_Valor_Parametro.Add(Cr_Valor);
                    Cr_Parametro.ApplyCurrentValues(Cr_Valor_Parametro);
                    //****************************************************************
                }
                catch { }
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Export_Options);

                try
                {
                    if (Mostrar_Reporte(Archivo_PDF, "PDF"))
                    {
                        Impresion_Correcta = true;
                    }
                    else
                    {
                        Impresion_Correcta = false;
                    }
                }
                catch (Exception Ex)
                {
                    Impresion_Correcta = false;
                    Lbl_Mensaje_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Ex.Message.ToString();
                }
            }
            catch
            {
                Impresion_Correcta = false;
                //Lbl_Mensaje_Error.Visible = true;
                //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
            }
        }
        catch
        {
            Impresion_Correcta = false;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }
        return Impresion_Correcta;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte_Folio(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch //(Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Mostrar_Reporte(String Nombre_Reporte, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        Boolean Impresion_Correcta = false;

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt", "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                Impresion_Correcta = true;
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
                Impresion_Correcta = true;
            }
        }
        catch (Exception Ex)
        {
            Impresion_Correcta = false;
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
        return Impresion_Correcta;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Dt_Constancias_Propiedad
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Constancia de Propiedad Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 19/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Dt_Constancias_Propiedad(int Indice_Fila)
    {
        Ds_Pre_Constancias Ds_Constancias = new Ds_Pre_Constancias();
        DataRow Dr_Constancias_Propiedad;

        //Inserta los datos de la Constancia de Propiedad en la Tabla
        Dr_Constancias_Propiedad = Ds_Constancias.Tables["Dt_Constancias_Propiedad"].NewRow();
        Dr_Constancias_Propiedad["Cuenta_Predial"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[1].Text;
        Dr_Constancias_Propiedad["Propietario"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[2].Text;
        Dr_Constancias_Propiedad["Folio"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[3].Text;
        Dr_Constancias_Propiedad["Fecha"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[4].Text;

        Ds_Constancias.Tables["Dt_Constancias_Propiedad"].Rows.Add(Dr_Constancias_Propiedad);

        return Ds_Constancias.Tables["Dt_Constancias_Propiedad"];
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Constancias
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Constancia de Propiedad Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 19/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Constancias(int Indice_Fila)
    {
        Ds_Pre_Constancias Ds_Constancias = new Ds_Pre_Constancias();
        DataRow Dr_Constancias;

        foreach (DataTable Dt_Constancias in Ds_Constancias.Tables)
        {
            if (Dt_Constancias.TableName == "Dt_Constancias_Propiedad")
            {
                //Inserta los datos de la Constancia de Propiedad en la Tabla
                Dr_Constancias = Dt_Constancias.NewRow();
                Dr_Constancias["Cuenta_Predial"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[1].Text;// .SelectedRow.Cells[1].Text;
                if (!Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[2].Text.Equals("&nbsp;"))
                {
                    Dr_Constancias["Propietario"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[2].Text;//.SelectedRow.Cells[2].Text;
                }
                else
                {
                    Dr_Constancias["Propietario"] = "----------";
                }
                Dr_Constancias["Folio"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[3].Text;//.SelectedRow.Cells[3].Text;
                Dr_Constancias["Fecha"] = Grid_Constancias_Propiedad.Rows[Indice_Fila].Cells[4].Text;//.SelectedRow.Cells[4].Text;
                Dr_Constancias["Iniciales"] = Txt_Realizo.Text;//.SelectedRow.Cells[4].Text;
                Dr_Constancias["Mes"] = DateTime.Now.ToString("MMMM").ToUpper();//.SelectedRow.Cells[4].Text;
                Dr_Constancias["Proteccion_Pago"] = Txt_No_Recibo_Pago.Text;
                Dt_Constancias.Rows.Add(Dr_Constancias);
            }
        }

        return Ds_Constancias;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Constancias_Folio
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos de la Constancia de Propiedad Seleccionada en el GridView
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 29/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Constancias(String Folio, String Total_Pagar)
    {
        Ds_Ope_Pre_Constancias_Folio Ds_Constancias = new Ds_Ope_Pre_Constancias_Folio();
        DataRow Dr_Constancias;

        foreach (DataTable Dt_Constancias in Ds_Constancias.Tables)
        {
            if (Dt_Constancias.TableName == "Dt_Constancia")
            {
                //Inserta los datos de la Certificación en la Tabla
                Dr_Constancias = Dt_Constancias.NewRow();
                Dr_Constancias["Cuenta_predial"] = Txt_Cuenta_Predial.Text;
                Dr_Constancias["Tipo"] = "Constancia de propiedad";
                Dr_Constancias["Propietario"] = Txt_Propietario.Text;
                String Domicilio = "";
                Domicilio = "CALLE " + Txt_Calle.Text;
                if (Txt_No_Exterior.Text != "")
                {
                    if (Domicilio != "")
                    {
                        Domicilio += ", ";
                    }
                    Domicilio += Txt_No_Exterior.Text;
                }
                if (Txt_No_Interior.Text != "")
                {
                    if (Domicilio != "")
                    {
                        Domicilio += ", ";
                    }
                    Domicilio += Txt_No_Interior.Text;
                }
                if (Txt_Colonia.Text != "")
                {
                    if (Domicilio != "")
                    {
                        Domicilio += ", COLONIA ";
                    }
                    Domicilio += Txt_Colonia.Text;
                }
                Dr_Constancias["Ubicacion"] = Domicilio;
                Dr_Constancias["Folio"] = Folio;
                Dr_Constancias["Total_Pagar"] = "$" + Convert.ToDouble(Total_Pagar).ToString("#,###,###,##0.00");
                Dr_Constancias["Fecha"] = Convert.ToDateTime(Txt_Fecha.Text).ToString("dd/MMM/yyyy").ToUpper();
                Dt_Constancias.Rows.Add(Dr_Constancias);
            }
        }

        return Ds_Constancias;
    }

    #endregion
    protected void Grid_Constancias_Propiedad_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.Cells[6].Text != "PAGADA")
            {
                if (e.Row.Cells[7].Controls.Count != 0)
                {
                    ((ImageButton)e.Row.Cells[7].Controls[0]).Enabled = false;
                }
            }
            else
            {
                if (e.Row.Cells[7].Controls.Count != 0)
                {
                    ((ImageButton)e.Row.Cells[7].Controls[0]).Enabled = true;
                }
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Convenio
    /// DESCRIPCIÓN: Generar convenio (con OpenXML SDK a partir de documento con controles de contenido)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Constancia()
    {
        string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Constancia_Propiedad.docx");
        string Documento_Salida = Server.MapPath("../../Reporte/" + "Constancia_Propiedad.docx");
        //Documento_Salida = Server.MapPath("~/Reporte/" + "Convenio_Der_Sup.docx");

        //create copy of template so that we don't overwrite it
        if (System.IO.File.Exists(Documento_Salida))
        {
            System.IO.File.Delete(Documento_Salida);
        }
        File.Copy(Ruta_Plantilla, Documento_Salida);

        ReportDocument Reporte = new ReportDocument();
        String Nombre_Archivo = "Formato_Constancia_Propiedad.docx";
        String PDF_Convenio = Nombre_Archivo + ".pdf";

        // si no existe el directorio, crearlo
        if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
            System.IO.Directory.CreateDirectory("../../Reporte");

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        DateTime Fecha = Convert.ToDateTime(Txt_Fecha.Text);
        String Domicilio = "";
        if (Txt_Calle.Text.Trim() != "")
        {
            Domicilio += "CALLE " + Txt_Calle.Text;
        }
        if (Txt_No_Exterior.Text.Trim() != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", ";
            }
            Domicilio += Txt_No_Exterior.Text;
        }
        if (Txt_No_Interior.Text.Trim() != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", ";
            }
            Domicilio += Txt_No_Interior.Text;
        }
        if (Txt_Colonia.Text.Trim() != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", ";
            }
            Domicilio += "COLONIA " + Txt_Colonia.Text;
        }

        try
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
            {
                //create XML string matching custom XML part
                string newXml = "<root>"
                + "<CUENTA_PREDIAL>" + Txt_Cuenta_Predial.Text + "</CUENTA_PREDIAL>"
                + "<PROPIETARIO>" + Txt_Propietario.Text.ToUpper() + "</PROPIETARIO>"
                + "<DOMICILIO>" + Domicilio + "</DOMICILIO>"
                + "<DIAS>" + DateTime.Now.ToString("dd") + "</DIAS>"
                + "<MES>" + DateTime.Now.ToString("MMMM").ToUpper() + "</MES>"
                + "<ANIO>" + DateTime.Now.ToString("yyyy") + "</ANIO>"
                + "<CONFRONTO>" + Txt_Realizo.Text + "</CONFRONTO>"
                + "<FOLIO>" + Txt_Folio.Text + "</FOLIO>"
                + "<PROTECCION_PAGO>" + Txt_No_Recibo_Pago.Text + "</PROTECCION_PAGO>"
                + "</root>";

                MainDocumentPart main = doc.MainDocumentPart;
                main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                //add and write new XML part
                CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                {
                    ts.Write(newXml);
                }
                // guardar los cambios en el documento
                main.Document.Save();

                //closing WordprocessingDocument automatically saves the document
            }

            //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
            //// ofrecer para descarga
            //Response.Clear();
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/x-msword";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
            ////           'Visualiza el archivo
            //Response.WriteFile(Ruta);
            //Response.Flush();
            //Response.Close();

            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            Pagina = Pagina + "Constancia_Propiedad.docx";
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato_Convenio",
                "window.open('" + Pagina +
                "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Imprimir Constancia: " + Ex.Message);
        }
    }
}
