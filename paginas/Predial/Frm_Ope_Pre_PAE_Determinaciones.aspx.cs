using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Predial_Pae_Etapas.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.Data.OleDb;
//using Microsoft.Office.Interop.Excel;
using System.Configuration;
using System.IO;

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Determinaciones : System.Web.UI.Page
{
    #region Page_Load

    protected void Page_Load(object sender, EventArgs e)
    {
        ScriptManager.GetCurrent(Page).RegisterPostBackControl(Btn_Cargar_Archivo);
        String Ventana_Modal = "";
        Grid_Cuentas_Generar.Columns[0].Visible = false;//Oculta columna Cuenta_Predial_ID
        Grid_Cuentas_Omitidas.Columns[1].Visible = false;//Oculta columna Cuenta_Predial_ID
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Estado_Formulario(false);//Habilita la configuración inicial de los controles de la página.                
            }
            //Limpiamos algún mensaje de error que se halla quedado en el log, que muestra los errores del sistema.
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Seleccionar_Colonia.Attributes.Add("onclick", Ventana_Modal);

            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Seleccionar_Calle.Attributes.Add("onclick", Ventana_Modal);

            //Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:450px;dialogHide:true;help:no;scroll:no');";            
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/PAE/Frm_Busqueda_Contribuyentes_PAE.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:450px;dialogHide:true;help:no;scroll:no');";            
            Btn_Seleccionar_Propietario.Attributes.Add("onClick", Ventana_Modal);

            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Busca_Domicilio.Attributes.Add("onclick", Ventana_Modal);

            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:450px;dialogHide:true;help:no;scroll:no');";
            Btn_Busca_Cuenta.Attributes.Add("onClick", Ventana_Modal);
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos/Generales [Limpiar Todo,Mensaje_Error,Cargar_Combos,Llenar_Combo_ID,Estado_Botones,Iniciliza_Controles ]

    #region Reportes
    /// *************************************************************************************
    /// NOMBRE: Generar_Reporte
    /// 
    /// DESCRIPCIÓN: Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS: Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///             Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Predial/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Reporte
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en pantalla.
    ///              
    /// PARÁMETROS: Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Empleados",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Exportar_Reporte_PDF
    /// 
    /// DESCRIPCIÓN: Método que guarda el reporte generado en formato PDF en la ruta
    ///              especificada.
    ///              
    /// PARÁMETROS: Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///             Nombre_Reporte.- Nombre que se le dará al reporte.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpiar_Formulario
    ///DESCRIPCION : Limpia los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Armando Zavala Moreno
    ///FECHA_CREO  : 01-Febrero-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Limpiar_Formulario()
    {
        try
        {
            foreach (Control Txt_Lmpia in Div_Generadas.Controls)
            {
                if (Txt_Lmpia is TextBox)
                {
                    ((TextBox)Txt_Lmpia).Text = "";
                }
            }
            foreach (Control Txt_Lmpia in Div_Generacion.Controls)
            {
                if (Txt_Lmpia is TextBox)
                {
                    ((TextBox)Txt_Lmpia).Text = "";
                }
            }

            Grid_Cuentas_Generar.DataSource = null;
            Grid_Cuentas_Omitidas.DataSource = null;
            Grid_Determinaciones_Generadas.DataSource = null;
            Grid_Cuentas_Omitidas.DataBind();
            Grid_Cuentas_Generar.DataBind();
            //Grid_Determinaciones_Generadas.DataBind();

            Hdn_Calle_ID.Value = null;
            Hdn_Colonia_ID.Value = null;
            Hdn_Cuenta_ID.Value = null;
            Hdn_Modo_Generacion.Value = "Normal";
            Hdn_Motivo_Omision.Value = null;
            Session_Remove();
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Session_Remove
    ///DESCRIPCION : Elimina las sessiones usadas para guardar datos de la orden de variacion
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Session_Remove()
    {
        try
        {
            Session.Remove("Dt_Agregar_Diferencias");
            Session.Remove("Dt_Agregar_Co_Propietarios");
            Session.Remove("Dt_Contribuyentes");
            Session.Remove("M_Orden_Negocio");
            Session.Remove("M_Orden_Negocio");
            Session.Remove("P_Generar_Orden_Dt_Detalles");
            Session.Remove("Ds_Cuenta_Datos");
            Session.Remove("Contrarecibo_Traslado");
            Session.Remove("Dt_Contrarecibo");
            Session.Remove("Quitar_Cuota_Fija");
            Session["Estatus_Cuenta"] = "NINGUNO";
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Encabezado_Error.Text = "";
        //Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
        Lbl_Encabezado_Error.Text += P_Mensaje + "</br>";
        Lbl_Mensaje_Error.Text = "";

    }
    private void Limpia_Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Encabezado_Error.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Despachos_Externos
    ///DESCRIPCIÓN: Metodo usado para cargar la informacion de los despachos externos
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 02/02/2012 10:22:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Despachos_Externos(DropDownList Cmb_Despachos)
    {
        DataTable Dt_Despachos = new DataTable();
        try
        {
            Cls_Cat_Pre_Despachos_Externos_Negocio Despachos_Externos = new Cls_Cat_Pre_Despachos_Externos_Negocio();
            Despachos_Externos.P_Filtro = "";
            Cmb_Despachos.DataTextField = Cat_Pre_Despachos_Externos.Campo_Despacho;
            Cmb_Despachos.DataValueField = Cat_Pre_Despachos_Externos.Campo_Despacho_Id;

            Dt_Despachos = Despachos_Externos.Consultar_Despachos_Externos();

            foreach (DataRow Dr_Fila in Dt_Despachos.Rows)
            {
                if (Dr_Fila["ESTATUS"].ToString() != "VIGENTE")//Busca la cuenta en el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Despachos.DataSource = Dt_Despachos;
            Cmb_Despachos.DataBind();
            Cmb_Despachos.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));

        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Tipo_Predio
    ///DESCRIPCIÓN: Metodo usado para cargar la informacion de los tipos de predio
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 23/02/2012 06:24:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Tipo_Predio()
    {
        try
        {
            Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio_Consulta = new Cls_Cat_Pre_Tipos_Predio_Negocio();
            Tipo_Predio_Consulta.P_Filtros_Dinamicos = "";
            Cmb_Tipo_Predio.DataTextField = Cat_Pre_Tipos_Predio.Campo_Descripcion;
            Cmb_Tipo_Predio.DataValueField = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID;
            Cmb_Tipo_Predio.DataSource = Tipo_Predio_Consulta.Consultar_Tipo_Predio();
            Cmb_Tipo_Predio.DataBind();
            Cmb_Tipo_Predio.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));
            

            ////Cargar combo Tipo Domicilio
            //Cmb_Tipo_Domicilio.Items.Add(new ListItem("<-- SELECCIONE -->", "0"));
            //Cmb_Tipo_Domicilio.Items.Add(new ListItem("UBICACIÓN", "1"));
            //Cmb_Tipo_Domicilio.Items.Add(new ListItem("NOTIFICACIÓN", "2"));

            ////Cargar combo estatus            
            //Cmb_Estatus.Items.Add(new ListItem("<-- SELECCIONE -->", "0"));
            //Cmb_Estatus.Items.Add(new ListItem("NOTIFICADO", "1"));
            //Cmb_Estatus.Items.Add(new ListItem("NO DILIGENCIADO", "2"));
            //Cmb_Estatus.Items.Add(new ListItem("ILOCALIZABLE", "3"));
            //Cmb_Estatus.Items.Add(new ListItem("PENDIENTE", "4"));

        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row["DESCRIPCION"].ToString(), row["ID"].ToString()));
            }
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String P_Text, String P_Value)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row[P_Text].ToString(), row[P_Value].ToString()));
            }
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Inicializa_Controles
    ///DESCRIPCIÓN: inicializa los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: tres/agosto/2011 06:28:07 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Inicializa_Controles()
    {



    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Formulario
    ///DESCRIPCIÓN: Establece en que estado esta el formulario, si esta disponible para guardar,
    ///             crear una nueva determinacion.
    ///PARAMETROS:  Estado, Estado en el que se cargara la configuración de los
    ///                            controles.
    ///CREO:        Armando Zavala Moreno
    ///FECHA_CREO:  02/02/2012 04:00:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    ///
    private void Estado_Formulario(Boolean Estado)
    {
        try
        {
            Limpiar_Formulario();
            if (Estado == true)//Si es verdadero se activan las opciones para guardar
            {
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Imprimir.AlternateText = "Buscar";
                Btn_Imprimir.ToolTip = "Buscar";
                Btn_Imprimir.ImageUrl = "~/paginas/imagenes/paginas/icono_consultar.png";
                Btn_Subir_Archivo.Visible = false;
                Btn_Buscar.Visible = false;
                Txt_Busqueda.Visible = false;
                Cargar_Combo_Despachos_Externos(Cmb_Despachos);
                Cargar_Combo_Tipo_Predio();
                Div_Generacion.Visible = true;
                Div_Generadas.Visible = false;
                Limpia_Mensaje_Error();
                Limpiar_Formulario();
            }
            else
            {
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ToolTip = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Imprimir.AlternateText = "Imprimir";
                Btn_Imprimir.ToolTip = "Imprimir";
                Btn_Imprimir.ImageUrl = "~/paginas/imagenes/gridview/grid_print.png";
                Btn_Subir_Archivo.Visible = true;
                Div_Generacion.Visible = false;
                Div_Generadas.Visible = true;
                Btn_Buscar.Visible = true;
                Txt_Busqueda.Visible = true;
                Limpia_Mensaje_Error();
                Limpiar_Formulario();
                Cargar_Combo_Tipo_Predio();
                Cargar_Combo_Despachos_Externos(Cmb_Asignado_a);
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PARAMETROS: 
    ///CREO:        Armando Zavala Moreno
    ///FECHA_CREO:  09/02/2012 06:24:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Boolean Validacion = true;
        Limpia_Mensaje_Error();
        if (Txt_Colonia.Text.Length < 1)
        {
            Mensaje_Error("Seleccione una colonia.");
            Validacion = false;
        }

        if (Cmb_Despachos.SelectedIndex < 1)
        {
            Mensaje_Error("Seleccione un Despacho");
            Validacion = false;
        }
        if (Grid_Cuentas_Generar.Rows.Count < 1 && Grid_Cuentas_Omitidas.Rows.Count < 1)
        {
            Mensaje_Error("No existen Cuentas");
            Validacion = false;
        }

        Div_Generacion.Visible = true;
        return Validacion;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Colonia_Calle
    ///DESCRIPCIÓN: Busca la colonia, calle y pasa los datos a los textbox.            
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 02/02/2012 05:20:09 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    ///
    private void Busqueda_Colonia_Calle()
    {
        try
        {
            if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
                {
                    Hdn_Colonia_ID.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    Txt_Colonia.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                    Hdn_Calle_ID.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    Txt_Calle.Text = Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
                    Div_Generacion.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Propietario
    ///DESCRIPCIÓN: Busca la colonia, calle y pasa los datos a los textbox.            
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 22/02/2012 12:00:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Busqueda_Propietario()
    {
        Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Cuentas_Predial = new DataTable();
        try
        {
            if (Session["BUSQUEDA_CONTRIBUYENTE"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_CONTRIBUYENTE"]) == true)
                {
                    Pae_Etapas.P_Contribuyente_Id = HttpUtility.HtmlDecode(Session["CONTRIBUYENTE_ID"].ToString().Replace("&nbsp;", ""));
                    Dt_Cuentas_Predial = Pae_Etapas.Consultar_Contribuyente_Etapas_Pae();
                    Txt_Propietario.Text = HttpUtility.HtmlDecode(Session["CONTRIBUYENTE_NOMBRE"].ToString().Replace("&nbsp;", ""));
                    Session["CUENTAS_PREDIAL_CONTRIBUYENTE"] = Dt_Cuentas_Predial;
                    Session.Remove("CONTRIBUYENTE_ID");
                    Session.Remove("CONTRIBUYENTE_NOMBRE");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Regresa_Cuentas_Determinar_Omitidas
    ///DESCRIPCIÓN: Llama a la funcion Consulta_Estatus_Convenio para ver el estado de la cuenta
    ///             y del convenio, despues llama a los metodos para calcular las cuentas
    ///             a determinar y cuentas omitidas
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 03/02/2012 01:01:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Regresa_Cuentas_Determinar_Omitidas()
    {
        //Se crea la tabla para las cuentas generadas y se empiezan las busqueda de cada cuenta
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Consulta_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Generadas = Crear_Tabla_A_Determinar();//Tabla para Cuentas A determinar
        DataTable Dt_Omitida = Crear_Tabla_Omitodas();//Tabla para Cuentas omitidas
        DataTable Dt_Busqueda_ID = Regresa_CuentaID_Por_Colonia_Calle();
        decimal Rezago_Mayor_A=0;
        decimal Rezago_Menor_A=0;
        try
        {
            if (Dt_Busqueda_ID != null)//Si existen registros en la tabla
            {
                for (int Contador = 0; Contador < Dt_Busqueda_ID.Rows.Count; Contador++)
                {
                    if (Dt_Busqueda_ID.Rows[Contador][0] != null)
                    {
                        if (Dt_Busqueda_ID.Rows[Contador][0].ToString() != "")
                        {
                            Rs_Consulta_Etapas.P_Cuenta_Predial_Id = Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();                            
                            if (Rs_Consulta_Etapas.Consultar_Pae_Det_Etapas().Rows.Count < 1)//Comprueba si la cuenta se encuentra en alguna etapa PAE
                            {
                                if (Consulta_Estatus_Convenio(Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString()) != false)//Si la cuenta esta activa y el convenio 
                                {   //Cuentas a determinar 
                                    Calcula_Adeudos_A_Determinar(Rs_Consulta_Adeudos, Dt_Busqueda_ID, Dt_Generadas, Contador, Rezago_Mayor_A, Rezago_Menor_A);
                                }
                                else//Cuentas Omitidas
                                {
                                    Calcula_Adeudos_Omitidas(Rs_Consulta_Adeudos, Dt_Busqueda_ID, Dt_Omitida, Contador, Rezago_Mayor_A, Rezago_Menor_A);
                                }
                            }
                        }
                    }
                }
                //Llena Grid cuentas a determinar
                Grid_Cuentas_Generar.DataSource = Dt_Generadas;
                Grid_Cuentas_Generar.DataBind();
                Session["Grid_Determinar"] = Dt_Generadas;//Mantiene el DataTable para hacer la paginacion del Grid
                Txt_Adeudo_Determinar.Text = Dt_Generadas.Rows.Count.ToString();
                //Llena Grid cuentas Omitidas
                Grid_Cuentas_Omitidas.DataSource = Dt_Omitida;
                Grid_Cuentas_Omitidas.DataBind();
                Session["Grid_Omitida"] = Dt_Omitida;//Mantiene el DataTable para hacer la paginacion del Grid
                Txt_Total_Adeudo_Omitidas.Text = Dt_Omitida.Rows.Count.ToString();
                if (Dt_Generadas.Rows.Count < 1 && Dt_Omitida.Rows.Count < 1)
                {
                    Mensaje_Error("No se encontraron cuentas con rezago");
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Busca una Colonia ó Calle";
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Generadas
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las cuentas Generadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 05:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_A_Determinar()
    {
        DataTable Dt_Generadas = new DataTable();
        Dt_Generadas.Columns.Add(new DataColumn("CUENTA_PREDIAL_ID", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("CUENTA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("PERIODO_CORRIENTE", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("CORRIENTE", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("PERIODO_REZAGO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("REZAGO", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("HONORARIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("MULTAS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("ADEUDO", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        return Dt_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Omitodas
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las cuentas Omitidas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 09/02/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Omitodas()
    {
        DataTable Dt_Generadas = new DataTable();
        Dt_Generadas.Columns.Add(new DataColumn("CUENTA_PREDIAL_ID", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("CUENTA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("PERIODO_CORRIENTE", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("CORRIENTE", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("PERIODO_REZAGO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("REZAGO", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("HONORARIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("MULTAS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("ADEUDO", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("MOTIVO_OMISIÓN", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        return Dt_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Determinaciones_Generadas
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las cuentas Omitidas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 09/02/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Determinaciones_Generadas()
    {
        DataTable Dt_Determinaciones_Generadas = new DataTable();
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("CUENTA", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ADEUDO", typeof(Decimal)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("FECHA", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("FOLIO", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ASIGNADO", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ENTREGA", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        return Dt_Determinaciones_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Subir_Archivo
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las cuentas
    ///                       que se suben en un archivo
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 20/02/2012 06:35:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Subir_Archivo()
    {
        DataColumn Columna0_CUENTA;
        DataColumn Columna1_PERIODO_CORRIENTE;
        DataColumn Columna2_CORRIENTE;
        DataColumn Columna3_PERIODO_REZAGO;
        DataColumn Columna4_REZAGO;
        DataColumn Columna5_RECARGOS_ORDINARIOS;
        DataColumn Columna6_RECARGOS_MORATORIOS;
        DataColumn Columna7_HONORARIOS;
        DataColumn Columna8_MULTAS;
        DataColumn Columna9_ADEUDO;
        System.Data.DataTable Tabla_Datos = new System.Data.DataTable();

        // ---------- Inicializar columnas
        Columna0_CUENTA = new DataColumn();
        Columna0_CUENTA.DataType = System.Type.GetType("System.String");
        Columna0_CUENTA.ColumnName = "CUENTA";
        Tabla_Datos.Columns.Add(Columna0_CUENTA);
        Columna1_PERIODO_CORRIENTE = new DataColumn();
        Columna1_PERIODO_CORRIENTE.DataType = System.Type.GetType("System.String");
        Columna1_PERIODO_CORRIENTE.ColumnName = "PERIODO_CORRIENTE";
        Tabla_Datos.Columns.Add(Columna1_PERIODO_CORRIENTE);
        Columna2_CORRIENTE = new DataColumn();
        Columna2_CORRIENTE.DataType = System.Type.GetType("System.String");
        Columna2_CORRIENTE.ColumnName = "CORRIENTE";
        Tabla_Datos.Columns.Add(Columna2_CORRIENTE);
        Columna3_PERIODO_REZAGO = new DataColumn();
        Columna3_PERIODO_REZAGO.DataType = System.Type.GetType("System.String");
        Columna3_PERIODO_REZAGO.ColumnName = "PERIODO_REZAGO";
        Tabla_Datos.Columns.Add(Columna3_PERIODO_REZAGO);
        Columna4_REZAGO = new DataColumn();
        Columna4_REZAGO.DataType = System.Type.GetType("System.String");
        Columna4_REZAGO.ColumnName = "REZAGO";
        Tabla_Datos.Columns.Add(Columna4_REZAGO);
        Columna5_RECARGOS_ORDINARIOS = new DataColumn();
        Columna5_RECARGOS_ORDINARIOS.DataType = System.Type.GetType("System.String");
        Columna5_RECARGOS_ORDINARIOS.ColumnName = "RECARGOS_ORDINARIOS";
        Tabla_Datos.Columns.Add(Columna5_RECARGOS_ORDINARIOS);
        Columna6_RECARGOS_MORATORIOS = new DataColumn();
        Columna6_RECARGOS_MORATORIOS.DataType = System.Type.GetType("System.String");
        Columna6_RECARGOS_MORATORIOS.ColumnName = "RECARGOS_MORATORIOS";
        Tabla_Datos.Columns.Add(Columna6_RECARGOS_MORATORIOS);
        Columna7_HONORARIOS = new DataColumn();
        Columna7_HONORARIOS.DataType = System.Type.GetType("System.String");
        Columna7_HONORARIOS.ColumnName = "HONORARIOS";
        Tabla_Datos.Columns.Add(Columna7_HONORARIOS);
        Columna8_MULTAS = new DataColumn();
        Columna8_MULTAS.DataType = System.Type.GetType("System.String");
        Columna8_MULTAS.ColumnName = "MULTAS";
        Tabla_Datos.Columns.Add(Columna8_MULTAS);
        Columna9_ADEUDO = new DataColumn();
        Columna9_ADEUDO.DataType = System.Type.GetType("System.String");
        Columna9_ADEUDO.ColumnName = "ADEUDO";
        Tabla_Datos.Columns.Add(Columna9_ADEUDO);
        return Tabla_Datos;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Busqueda_Colonia_Calle
    ///DESCRIPCIÓN          : Devuelve un DataTable con la Cuenta_Predial_ID,
    ///                       Cuenta_Predial
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 05:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Regresa_CuentaID_Por_Colonia_Calle()
    {
        //Se pasan los parametros de la busqueda de todas las cuentas y se pasa al datatable
        Cls_Cat_Pre_Cuentas_Predial_Negocio Rs_Consulta_Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Busqueda_ID;
        if (Hdn_Colonia_ID.Value.ToString() == "" & Hdn_Calle_ID.Value.ToString() == "")//Si no existe ninguna calle o colonia
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Busca una Colonia ó Calle";
        }
        else
        {
            if (Hdn_Colonia_ID.Value.ToString() != "" && Hdn_Calle_ID.Value.ToString() == "")
            {
                Rs_Consulta_Cuenta.P_Filtros_Dinamicos = Cat_Pre_Calles_Colonias.Campo_Colonia_ID + " = " + Hdn_Colonia_ID.Value.ToString();
            }
            if (Hdn_Colonia_ID.Value.ToString() != "" && Hdn_Calle_ID.Value.ToString() != "")
            {
                Rs_Consulta_Cuenta.P_Filtros_Dinamicos = Cat_Pre_Calles_Colonias.Campo_Colonia_ID + " = " + Hdn_Colonia_ID.Value.ToString() + " AND " + Cat_Pre_Calles_Colonias.Campo_Calle_ID + " = " + Hdn_Calle_ID.Value.ToString();
            }
            if (Hdn_Colonia_ID.Value.ToString() == "" && Hdn_Calle_ID.Value.ToString() != "")
            {
                Rs_Consulta_Cuenta.P_Filtros_Dinamicos = Cat_Pre_Calles_Colonias.Campo_Calle_Colonia_ID + " = " + Hdn_Calle_ID.Value.ToString();
            }
            Dt_Busqueda_ID = Rs_Consulta_Cuenta.Consultar_Cuenta();//Obtiene las Cuentas A Determinar de la Colonia o Calle especificada
            return Dt_Busqueda_ID;
        }
        return null;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Generadas
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas a generar y Caulcula el Perido, Rezago, etc
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/02/2012 04:38:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_A_Determinar(DataTable Dt_Generadas, DataTable Dt_Busqueda_ID, int Contador, Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos)
    {
        try
        {
            Decimal Recargos_Moratorios = Obtener_Recargos_Moratorios(Dt_Busqueda_ID.Rows[Contador][0].ToString());
            DataRow Dr_Generadas;
            Dr_Generadas = Dt_Generadas.NewRow();
            Dr_Generadas["CUENTA_PREDIAL_ID"] = Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
            Dr_Generadas["CUENTA"] = Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Dr_Generadas["PERIODO_CORRIENTE"] = Rs_Consulta_Adeudos.p_Periodo_Corriente;
            Dr_Generadas["CORRIENTE"] = Rs_Consulta_Adeudos.p_Total_Corriente;
            Dr_Generadas["PERIODO_REZAGO"] = Rs_Consulta_Adeudos.p_Periodo_Rezago;
            Dr_Generadas["REZAGO"] = Rs_Consulta_Adeudos.p_Total_Rezago;
            Dr_Generadas["RECARGOS_ORDINARIOS"] = Rs_Consulta_Adeudos.p_Total_Recargos_Generados;
            Dr_Generadas["RECARGOS_MORATORIOS"] = Recargos_Moratorios;
            Dr_Generadas["HONORARIOS"] = "0.0";
            Dr_Generadas["MULTAS"] = "0.0";
            Dr_Generadas["ADEUDO"] = Rs_Consulta_Adeudos.p_Total_Corriente + Rs_Consulta_Adeudos.p_Total_Rezago + Rs_Consulta_Adeudos.p_Total_Recargos_Generados + Recargos_Moratorios;
            Dr_Generadas["ESTATUS"] = "PENDIENTE";
            Dt_Generadas.Rows.Add(Dr_Generadas);//Se asigna la nueva fila a la tabla
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Omitidas
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas omitidas y Calcula el Perido, Rezago, etc
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 09/02/2012 05:51:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Omitidas(DataTable Dt_Omitidas, DataTable Dt_Busqueda_ID, int Contador, Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos)
    {
        try
        {
            Decimal Recargos_Moratorios = Obtener_Recargos_Moratorios(Dt_Busqueda_ID.Rows[Contador][0].ToString());
            DataRow Dr_Omitidas;
            Dr_Omitidas = Dt_Omitidas.NewRow();
            Dr_Omitidas["CUENTA_PREDIAL_ID"] = Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
            Dr_Omitidas["CUENTA"] = Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Dr_Omitidas["PERIODO_CORRIENTE"] = Rs_Consulta_Adeudos.p_Periodo_Corriente;
            Dr_Omitidas["CORRIENTE"] = Rs_Consulta_Adeudos.p_Total_Corriente;
            Dr_Omitidas["PERIODO_REZAGO"] = Rs_Consulta_Adeudos.p_Periodo_Rezago;
            Dr_Omitidas["REZAGO"] = Rs_Consulta_Adeudos.p_Total_Rezago;
            Dr_Omitidas["RECARGOS_ORDINARIOS"] = Rs_Consulta_Adeudos.p_Total_Recargos_Generados;
            Dr_Omitidas["RECARGOS_MORATORIOS"] = Recargos_Moratorios;
            Dr_Omitidas["HONORARIOS"] = "0.0";
            Dr_Omitidas["MULTAS"] = "0.0";
            Dr_Omitidas["ADEUDO"] = Rs_Consulta_Adeudos.p_Total_Corriente + Rs_Consulta_Adeudos.p_Total_Rezago + Rs_Consulta_Adeudos.p_Total_Recargos_Generados + Recargos_Moratorios;
            Dr_Omitidas["MOTIVO_OMISIÓN"] = Hdn_Motivo_Omision.Value.ToString();
            Dr_Omitidas["ESTATUS"] = "PENDIENTE";
            Dt_Omitidas.Rows.Add(Dr_Omitidas);//Se asigna la nueva fila a la tabla
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Determinadas
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas omitidas y Calcula el Perido, Rezago, etc
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 11:49:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Determinadas(DataTable Dt_Determinaciones_Generadas, DataTable Dt_Busqueda,Int32 Cont_Busqueda)
    {
        try
        {
            DataRow Dr_Determinadas;
            Dr_Determinadas = Dt_Determinaciones_Generadas.NewRow();
            String Fecha_Notificacion = Dt_Busqueda.Rows[Cont_Busqueda]["FECHA_NOTIFICACION"].ToString();
            if (Fecha_Notificacion == null || Fecha_Notificacion == "")
            {
                Fecha_Notificacion = "";
            }
            Dr_Determinadas["CUENTA"] = Dt_Busqueda.Rows[Cont_Busqueda]["CUENTA_PREDIAL"].ToString(); ;
            Dr_Determinadas["ADEUDO"] = Dt_Busqueda.Rows[Cont_Busqueda]["TOTAL"].ToString();
            Dr_Determinadas["FECHA"] = Fecha_Notificacion;
            Dr_Determinadas["FOLIO"] = Dt_Busqueda.Rows[Cont_Busqueda][Ope_Pre_Pae_Det_Etapas.Campo_Folio].ToString();
            Dr_Determinadas["ASIGNADO"] = Dt_Busqueda.Rows[Cont_Busqueda][Cat_Pre_Despachos_Externos.Campo_Despacho].ToString();
            Dr_Determinadas["ENTREGA"] = Dt_Busqueda.Rows[Cont_Busqueda][Ope_Pre_Pae_Etapas.Campo_Numero_Entrega].ToString();
            Dr_Determinadas["ESTATUS"] = Dt_Busqueda.Rows[Cont_Busqueda][Ope_Pre_Pae_Det_Etapas.Campo_Estatus].ToString();
            Dt_Determinaciones_Generadas.Rows.Add(Dr_Determinadas);//Se asigna la nueva fila a la tabla        
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: obtener_Recargos_Moratorios
    /// DESCRIPCIÓN: Leer las parcialidades del ultimo convenio o reestructura para obtener los adeudos
    ///            a tomar en cuenta para la reestructura
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: Armando Zavala Moreno
    /// FECHA_MODIFICÓ: 09-Feb-2012
    /// CAUSA_MODIFICACIÓN: Para que devuelva un valor decimal y pasarlo al Grid
    ///*******************************************************************************************************
    private decimal Obtener_Recargos_Moratorios(String Cuenta_Predial_ID)
    {
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Parcialidades = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Convenios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        DataTable Dt_Parcialidades;
        DataTable Dt_Convenios;
        Decimal Recargos_Moratorios = 0;
        Decimal Recargos_Ordinarios = 0;
        Decimal Honorarios = 0;
        Decimal Monto_Impuesto = 0;
        Decimal Monto_Base = 0;
        Decimal Adeudo_Honorarios = 0;
        Decimal Adeudo_Recargos = 0;
        Decimal Adeudo_Moratorios = 0;
        Decimal Monto_Total_Moratorios = 0;
        String No_Convenio = "";
        int Parcialidad = 0;
        DateTime Fecha_Vencimiento = DateTime.MinValue;
        int Meses_Transcurridos = 0;

        // consultar convenios de la cuenta
        Consulta_Convenios.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Consulta_Convenios.P_Ordenar_Dinamico = Ope_Pre_Convenios_Predial.Campo_Fecha + " DESC,"
            + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
        Dt_Convenios = Consulta_Convenios.Consultar_Convenio_Predial();
        // si la consulta arrojó resultado, utilizar el primer registro (convenio mas reciente)
        if (Dt_Convenios != null && Dt_Convenios.Rows.Count > 0)
        {
            No_Convenio = Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
            // consultar las parcialidades del ultimo convenio guardado (convenio o ultima reestructura)
            Consulta_Parcialidades.P_No_Convenio = No_Convenio;
            Dt_Parcialidades = Consulta_Parcialidades.Consultar_Parcialidades_Ultimo_Convenio();

            // llamar metodo para determinar si el convenio esta vencido
            if (Convenio_Vencido(Dt_Parcialidades))
            {
                Parcialidad = Dt_Parcialidades.Rows.Count - 1;
                // recorrer la tabla de parcialidades hasta encontrar parcialidades con estatus PAGADO
                while (Parcialidad >= 0)
                {
                    // si la parcialidad tiene estatus diferente de PAGADO, sumar adeudos
                    if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() != "PAGADO")
                    {
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString(), out Honorarios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString(), out Recargos_Ordinarios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString(), out Recargos_Moratorios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString(), out Monto_Impuesto);
                        DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                        Adeudo_Honorarios += Honorarios;
                        Adeudo_Recargos += Recargos_Ordinarios;
                        Adeudo_Moratorios += Recargos_Moratorios;
                        Monto_Base += Monto_Impuesto;
                    }
                    Parcialidad--;
                }

                Meses_Transcurridos = Calcular_Meses_Entre_Fechas(Fecha_Vencimiento, DateTime.Now);
                Recargos_Moratorios = Calcular_Recargos_Moratorios(Monto_Base, Meses_Transcurridos);
            }
        }
        return Monto_Total_Moratorios = Convert.ToDecimal(Math.Round(Math.Round(Recargos_Moratorios + Adeudo_Moratorios, 3), 2).ToString("#,##0.00"));
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Convenio_Vencido
    /// DESCRIPCIÓN: Revisar las parcialidades en busca de parcialidades vencidas 
    ///             parcialidades sin pagar con fecha de vencimiento de hace mas de 10 dias habiles
    ///             Regresa verdadero si el convenio esta vencido.
    /// PARÁMETROS:
    /// 		1. Dt_Parcialidades: datatable con parcialidades de un convenio
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Convenio_Vencido(DataTable Dt_Parcialidades)
    {
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        DateTime Fecha_Periodo;
        DateTime Fecha_Vencimiento;
        int Dias = 0;
        int Meses = 0;
        bool Convenio_Vencido = false;

        // recorrer las parcialidades del convenio
        for (int Pago = 0; Pago < Dt_Parcialidades.Rows.Count; Pago++)
        {
            // si el estatus de la parcialidad es INCUMPLIDO
            if (Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "INCUMPLIDO")
            {
                // obtener la fecha de vencimiento de la parcialidad
                DateTime.TryParse(Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Periodo);
                Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo.ToShortDateString(), "10");
                // obtener el tiempo transcurrido desde la fecha de vencimiento
                Calcular_Tiempo_Entre_Fechas(Fecha_Vencimiento, DateTime.Now, out Dias, out Meses);
                // si el numero de dias transcurridos en mayor que cero, escribir fecha de vencimiento
                if (Dias > 0)
                {
                    Convenio_Vencido = true;
                }
                // abandonar el ciclo for
                break;
            }
        }
        return Convenio_Vencido;
    }
        ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Tiempo_Entre_Fechas
    /// DESCRIPCIÓN: Calcular numero de dias y meses transcurridos entre una fecha y otra
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inferior a tomar
    /// 		2. Hasta_Fecha: Fecha final hasta la que se calcula
    /// 		3. Dias: Se almacenan los dias de diferencia entre las fechas
    /// 		4. Meses: Almacena los meses transcurridos entre una fecha y otra
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Tiempo_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha, out Int32 Dias, out Int32 Meses)
    {
        TimeSpan Transcurrido = Hasta_Fecha - Desde_Fecha;
        if (Transcurrido > TimeSpan.Parse("0"))
        {
            DateTime Tiempo = DateTime.MinValue + Transcurrido;
            Meses = ((Tiempo.Year - 1) * 12) + (Tiempo.Month - 1);

            long tickDiff = Hasta_Fecha.Ticks - Desde_Fecha.Ticks;
            tickDiff = tickDiff / 10000000; // segundos
            Dias = (int)(tickDiff / 86400);
        }
        else
        {
            Dias = 0;
            Meses = 0;
        }
    }
        ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Meses_Entre_Fechas
    /// DESCRIPCIÓN: Regresa un enteron con el numero de meses vencidos entre dos fechas
    ///             (tomando el primer dia de cada mes)
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inicial a comparar
    /// 		2. Hasta_Fecha: Fecha final a comparar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Int32 Calcular_Meses_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha)
    {
        DateTime Fecha_Inicial = Convert.ToDateTime(Desde_Fecha.Month + "/1" + "/" + Desde_Fecha.Year);
        DateTime Fecha_Final = Convert.ToDateTime(Hasta_Fecha.ToShortDateString());
        int Meses = 0;

        // aumentar el numero de meses mientras la fecha inicial mas los meses no supere la fecha final
        while (Fecha_Final > Fecha_Inicial.AddMonths(Meses))
        {
            Meses++;
        }

        return Meses;
    }
        ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Recargos_Moratorios
    /// DESCRIPCIÓN: Calcular los recargos moratorios para una cantidad a partir de una fecha dados
    ///             (el numero de meses por el porcentaje de recargos por el monto base)
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Recargos_Moratorios(Decimal Monto_Base, Int32 Meses)
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Parametros;
        Decimal Recargos_Moratorios = 0;
        Decimal Porcentaje_Recargos = 0;

        // recuperar el porcentaje de recargos moratorios de la tabla de parametros
        Dt_Parametros = Parametros.Consultar_Parametros();
        if (Dt_Parametros != null)
        {
            if (Dt_Parametros.Rows.Count > 0)
            {
                Decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString(), out Porcentaje_Recargos);
            }
        }

        // obtener el producto de los meses por el porcentaje de recargos
        Porcentaje_Recargos *= Meses;

        // calcular recargos
        Recargos_Moratorios = Monto_Base * Porcentaje_Recargos / 100;

        return Recargos_Moratorios;
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consulta_Estatus_Convenio
    /// DESCRIPCIÓN: Busca una cuenta predial para verificar cual es el estado de su convenio y de su cuenta predial
    ///              
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: La cuenta que se va a consultar
    /// CREO: Armando Zavala Moreno
    /// FECHA_CREO: 09-Feb-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public bool Consulta_Estatus_Convenio(String Cuenta_Predial_ID)
    {
        bool Bandera = false;
        DataTable Dt_Cuentas;
        DataTable Dt_Convenio;
        String Estatus_Cuenta;
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Convenios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Consulta_Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        //Consulta si la cuenta tiene convenio
        Consulta_Convenios.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Consulta_Convenios.P_Ordenar_Dinamico = Ope_Pre_Convenios_Predial.Campo_Fecha + " DESC,"
            + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
        Dt_Convenio = Consulta_Convenios.Consultar_Convenio_Predial();
        //Consulta si la cuenta esta vigente
        Consulta_Cuenta.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Dt_Cuentas = Consulta_Cuenta.Consultar_Cuenta();
        Estatus_Cuenta = Dt_Cuentas.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();//Se asigna a la variable el estado de la cuenta

        switch (Estatus_Cuenta)
        {
            case "VIGENTE":
                if (Dt_Convenio.Rows.Count < 1)
                    Bandera = true;//Si esta disponible para determinar por que no tiene convenio
                else
                {
                    if (Dt_Convenio.Rows[0]["ESTATUS"].ToString() == "ACTIVO")
                    {
                        Bandera = false;//Es cuenta omitida por que tiene convenio
                        Hdn_Motivo_Omision.Value = "CONVENIO";
                    }
                    if (Dt_Convenio.Rows[0]["ESTATUS"].ToString() == "INCUMPLIDO")
                        Bandera = true;//Si esta disponible para determinar
                    if (Dt_Convenio.Rows[0]["ESTATUS"].ToString() == "TERMINADO")
                        Bandera = true;//Si esta disponible para determinar
                }
                Hdn_Estatus.Value = "VIGENTE";
                break;
            //Todos estos casos la cuenta es omitida
            case "PENDIENTE":
                Bandera = false;
                Hdn_Motivo_Omision.Value = "PENDIENTE";
                Hdn_Estatus.Value = "PENDIENTE";
                break;
            case "BAJA":
                Bandera = false;
                Hdn_Motivo_Omision.Value = "BAJA";
                Hdn_Estatus.Value = "BAJA";
                break;
            case "TEMPORAL":
                Bandera = false;
                Hdn_Motivo_Omision.Value = "TEMPORAL";
                Hdn_Estatus.Value = "TEMPORAL";
                break;
            case "CANCELADA":
                Bandera = false;
                Hdn_Motivo_Omision.Value = "CANCELADA";
                Hdn_Estatus.Value = "CANCELADA";
                break;
            case "BLOQUEADA":
                Bandera = false;
                Hdn_Motivo_Omision.Value = "BLOQUEADA";
                Hdn_Estatus.Value = "BLOQUEADA";
                break;
            default:
                Hdn_Motivo_Omision.Value = Estatus_Cuenta;
                Hdn_Estatus.Value = Estatus_Cuenta;
                break;
        }
        return Bandera;
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcula_Adeudos_A_Determinar
    /// DESCRIPCIÓN: Regresa las cuentas que tienen rezago y estan para determinar, calcula sus rezago, adeudos
    ///              
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: La cuenta que se va a consultar
    /// CREO: Armando Zavala Moreno
    /// FECHA_CREO: 10-Feb-2012 09:07:00 a.m.
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Calcula_Adeudos_A_Determinar(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos, DataTable Dt_Busqueda_ID, DataTable Dt_Cuentas, int Contador, Decimal Rezago_Mayor_A, Decimal Rezago_Menor_A)
    {
        try
        {
            Rs_Consulta_Adeudos.Calcular_Recargos_Predial(Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString());//lLama al metodo y calcula sus adeudos
            if (Txt_Rezago_Mayor.Text.Length > 0 & Txt_Rezago_Menor.Text.Length > 0)
            {
                Rezago_Menor_A = Convert.ToDecimal(Txt_Rezago_Menor.Text);
                Rezago_Mayor_A = Convert.ToDecimal(Txt_Rezago_Mayor.Text);
                if (Rs_Consulta_Adeudos.p_Total_Rezago >= Rezago_Mayor_A & Rs_Consulta_Adeudos.p_Total_Rezago <= Rezago_Menor_A)
                {
                    Llenar_DataRow_A_Determinar(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
            if (Txt_Rezago_Mayor.Text.Length > 0 & Txt_Rezago_Menor.Text.Length < 0)
            {
                Rezago_Mayor_A = Convert.ToDecimal(Txt_Rezago_Mayor.Text);
                if (Rs_Consulta_Adeudos.p_Total_Rezago > Rezago_Mayor_A)
                {
                    Llenar_DataRow_A_Determinar(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
            if (Txt_Rezago_Mayor.Text.Length < 1 & Txt_Rezago_Menor.Text.Length > 0)
            {
                Rezago_Menor_A = Convert.ToDecimal(Txt_Rezago_Menor.Text);
                if (Rs_Consulta_Adeudos.p_Total_Rezago > 1 & Rs_Consulta_Adeudos.p_Total_Rezago <= Rezago_Menor_A)
                {
                    Llenar_DataRow_A_Determinar(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
            if (Txt_Rezago_Mayor.Text.Length < 1 & Txt_Rezago_Menor.Text.Length < 1)
            {
                if (Rs_Consulta_Adeudos.p_Total_Rezago > 1)
                {
                    Llenar_DataRow_A_Determinar(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcula_Adeudos_Omitidas
    /// DESCRIPCIÓN: Regresa las cuentas que tienen rezago y son cuentas omitidas ya sea por que tienen convenio
    ///              ó la cuenta esta dada de baja, calcula sus rezago, adeudos
    ///              
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: La cuenta que se va a consultar
    /// CREO: Armando Zavala Moreno
    /// FECHA_CREO: 10-Feb-2012 10:48:00 a.m.
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Calcula_Adeudos_Omitidas(Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos, DataTable Dt_Busqueda_ID, DataTable Dt_Cuentas, int Contador, Decimal Rezago_Mayor_A, Decimal Rezago_Menor_A)
    {
        try
        {
            Rs_Consulta_Adeudos.Calcular_Recargos_Predial(Dt_Busqueda_ID.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString());//lLama al metodo y calcula sus adeudos
            if (Txt_Rezago_Mayor.Text.Length > 0 & Txt_Rezago_Menor.Text.Length > 0)
            {
                Rezago_Menor_A = Convert.ToDecimal(Txt_Rezago_Menor.Text);
                Rezago_Mayor_A = Convert.ToDecimal(Txt_Rezago_Mayor.Text);
                if (Rs_Consulta_Adeudos.p_Total_Rezago >= Rezago_Mayor_A & Rs_Consulta_Adeudos.p_Total_Rezago <= Rezago_Menor_A)
                {
                    Llenar_DataRow_Omitidas(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
            if (Txt_Rezago_Mayor.Text.Length > 0 & Txt_Rezago_Menor.Text.Length < 0)
            {
                Rezago_Mayor_A = Convert.ToDecimal(Txt_Rezago_Mayor.Text);
                if (Rs_Consulta_Adeudos.p_Total_Rezago > Rezago_Mayor_A)
                {
                    Llenar_DataRow_Omitidas(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
            if (Txt_Rezago_Mayor.Text.Length < 1 & Txt_Rezago_Menor.Text.Length > 0)
            {
                Rezago_Menor_A = Convert.ToDecimal(Txt_Rezago_Menor.Text);
                if (Rs_Consulta_Adeudos.p_Total_Rezago > 1 & Rs_Consulta_Adeudos.p_Total_Rezago <= Rezago_Menor_A)
                {
                    Llenar_DataRow_Omitidas(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
            if (Txt_Rezago_Mayor.Text.Length < 1 & Txt_Rezago_Menor.Text.Length < 1)
            {
                if (Rs_Consulta_Adeudos.p_Total_Rezago > 1)
                {
                    Llenar_DataRow_Omitidas(Dt_Cuentas, Dt_Busqueda_ID, Contador, Rs_Consulta_Adeudos);
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cambiar_Cuenta_A_Omitidas
    ///DESCRIPCIÓN          : Cambia un registro a cuentas omitidas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/02/2012 06:00:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cambiar_Cuenta_A_Omitidas()
    {
        DataTable Dt_Generadas = (DataTable)Session["Grid_Determinar"];
        DataTable Dt_Omitida = (DataTable)Session["Grid_Omitida"];
        DataRow Dr_Omitidas;
        Int32 Indice_Seleccionado=0;
        string Cuenta_Predial;//Almacena la cuenta predial para buscarla en mi tabla y borrarla        
        Indice_Seleccionado = Grid_Cuentas_Generar.SelectedIndex + (Grid_Cuentas_Generar.PageSize * Grid_Cuentas_Generar.PageIndex);
        try
        {   //Se crea una nueva fila con los valores del Grid Cuentas Omitidas          
            Dr_Omitidas = Dt_Omitida.NewRow();
            Dr_Omitidas["CUENTA_PREDIAL_ID"] = Dt_Generadas.Rows[Indice_Seleccionado]["CUENTA_PREDIAL_ID"].ToString();
            Dr_Omitidas["CUENTA"] = Dt_Generadas.Rows[Indice_Seleccionado]["CUENTA"].ToString();
            Dr_Omitidas["PERIODO_CORRIENTE"] = Dt_Generadas.Rows[Indice_Seleccionado]["PERIODO_CORRIENTE"].ToString();
            Dr_Omitidas["CORRIENTE"] = Dt_Generadas.Rows[Indice_Seleccionado]["CORRIENTE"].ToString();
            Dr_Omitidas["PERIODO_REZAGO"] = Dt_Generadas.Rows[Indice_Seleccionado]["PERIODO_REZAGO"].ToString();
            Dr_Omitidas["REZAGO"] = Dt_Generadas.Rows[Indice_Seleccionado]["REZAGO"].ToString();
            Dr_Omitidas["RECARGOS_ORDINARIOS"] = Dt_Generadas.Rows[Indice_Seleccionado]["RECARGOS_ORDINARIOS"].ToString();
            Dr_Omitidas["RECARGOS_MORATORIOS"] = Dt_Generadas.Rows[Indice_Seleccionado]["RECARGOS_MORATORIOS"].ToString();
            Dr_Omitidas["HONORARIOS"] = Dt_Generadas.Rows[Indice_Seleccionado]["HONORARIOS"].ToString();
            Dr_Omitidas["MULTAS"] = Dt_Generadas.Rows[Indice_Seleccionado]["MULTAS"].ToString();
            Dr_Omitidas["ADEUDO"] = Dt_Generadas.Rows[Indice_Seleccionado]["ADEUDO"].ToString();
            Dr_Omitidas["MOTIVO_OMISIÓN"] = Hdn_Motivo_Omision.Value.ToString();
            Dr_Omitidas["ESTATUS"] = Dt_Generadas.Rows[Indice_Seleccionado]["ESTATUS"].ToString();
            Dt_Omitida.Rows.Add(Dr_Omitidas);//Se asigna la nueva fila a la tabla Generadas

            if (Grid_Cuentas_Generar.SelectedIndex > -1)
            {
                Grid_Cuentas_Omitidas.DataSource = Dt_Omitida;//Se actualiza el grid
                Grid_Cuentas_Omitidas.DataBind();
                Session["Grid_Omitida"] = Dt_Omitida;//Mantiene el DataTable para hacer la paginacion del Grid
                Txt_Total_Adeudo_Omitidas.Text = Dt_Omitida.Rows.Count.ToString();
            }
            //Se asigna la cuenta_predial seleccionada a la variable            
            Cuenta_Predial = Dt_Generadas.Rows[Indice_Seleccionado]["CUENTA"].ToString();
            foreach (DataRow Dr_Fila in Dt_Generadas.Rows)
            {
                if (Cuenta_Predial == Dr_Fila["CUENTA"].ToString())//Busca la cuenta en la tabla
                {
                    Dr_Fila.Delete();//Borra el registro
                    Grid_Cuentas_Generar.DataSource = Dt_Generadas;//actualiza el grid
                    Grid_Cuentas_Generar.DataBind();
                    Session["Grid_Determinar"] = Dt_Generadas;//Mantiene el DataTable para hacer la paginacion del Grid
                    Txt_Adeudo_Determinar.Text = Dt_Generadas.Rows.Count.ToString();
                    break;
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cambiar_Cuenta_A_Determinar
    ///DESCRIPCIÓN          : Cambia la cuenta de omitida a cuentas a generar
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 15/02/2012 11:47:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cambiar_Cuenta_A_Determinar()
    {
        DataTable Dt_Generadas = (DataTable)Session["Grid_Determinar"]; ;
        DataTable Dt_Omitida = (DataTable)Session["Grid_Omitida"];
        DataRow Dr_Generada;
        Int32 Indice_Seleccionado = 0;
        string Cuenta_Predial;//Almacena la cuenta predial para buscarla en mi tabla y borrarla
        Indice_Seleccionado = Grid_Cuentas_Omitidas.SelectedIndex + (Grid_Cuentas_Omitidas.PageSize * Grid_Cuentas_Omitidas.PageIndex);
        try
        {   //Se crea una nueva fila con los valores del Grid Cuentas Omitidas
            Dr_Generada = Dt_Generadas.NewRow();
            Dr_Generada["CUENTA_PREDIAL_ID"] = Dt_Omitida.Rows[Indice_Seleccionado]["CUENTA_PREDIAL_ID"].ToString();
            Dr_Generada["CUENTA"] = Dt_Omitida.Rows[Indice_Seleccionado]["CUENTA"].ToString();
            Dr_Generada["PERIODO_CORRIENTE"] = Dt_Omitida.Rows[Indice_Seleccionado]["PERIODO_CORRIENTE"].ToString();
            Dr_Generada["CORRIENTE"] = Dt_Omitida.Rows[Indice_Seleccionado]["CORRIENTE"].ToString();
            Dr_Generada["PERIODO_REZAGO"] = Dt_Omitida.Rows[Indice_Seleccionado]["PERIODO_REZAGO"].ToString();
            Dr_Generada["REZAGO"] = Dt_Omitida.Rows[Indice_Seleccionado]["REZAGO"].ToString();
            Dr_Generada["RECARGOS_ORDINARIOS"] = Dt_Omitida.Rows[Indice_Seleccionado]["RECARGOS_ORDINARIOS"].ToString();
            Dr_Generada["RECARGOS_MORATORIOS"] = Dt_Omitida.Rows[Indice_Seleccionado]["RECARGOS_MORATORIOS"].ToString();
            Dr_Generada["HONORARIOS"] = Dt_Omitida.Rows[Indice_Seleccionado]["HONORARIOS"].ToString();
            Dr_Generada["MULTAS"] = Dt_Omitida.Rows[Indice_Seleccionado]["MULTAS"].ToString();
            Dr_Generada["ADEUDO"] = Dt_Omitida.Rows[Indice_Seleccionado]["ADEUDO"].ToString();
            Dr_Generada["ESTATUS"] = Dt_Omitida.Rows[Indice_Seleccionado]["ESTATUS"].ToString();
            Dt_Generadas.Rows.Add(Dr_Generada);//Se asigna la nueva fila a la tabla Generadas

            if (Grid_Cuentas_Omitidas.SelectedIndex > -1)
            {
                Grid_Cuentas_Generar.DataSource = Dt_Generadas;//Se actualiza el grid
                Grid_Cuentas_Generar.DataBind();
                Session["Grid_Determinar"] = Dt_Generadas;//Mantiene el DataTable para hacer la paginacion del Grid
                Txt_Adeudo_Determinar.Text = Dt_Generadas.Rows.Count.ToString();
            }
            Grid_Cuentas_Omitidas.Columns[1].Visible = false;
            //Se asigna la cuenta_predial seleccionada a la variable
            Cuenta_Predial = Dt_Omitida.Rows[Indice_Seleccionado]["CUENTA"].ToString();
            foreach (DataRow Dr_Fila in Dt_Omitida.Rows)
            {
                if (Cuenta_Predial == Dr_Fila["CUENTA"].ToString())//Busca la cuenta en la tabla
                {
                    Dr_Fila.Delete();//Borra el registro
                    Grid_Cuentas_Omitidas.DataSource = Dt_Omitida;//actualiza el grid
                    Grid_Cuentas_Omitidas.DataBind();
                    Session["Grid_Omitida"] = Dt_Omitida;//Mantiene el DataTable para hacer la paginacion del Grid
                    Txt_Total_Adeudo_Omitidas.Text = Dt_Omitida.Rows.Count.ToString();
                    break;
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cambiar_Cuenta_A_Determinar
    ///DESCRIPCIÓN          : Cambia la cuenta de omitida a cuentas a generar
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 15/02/2012 11:47:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Guardar_Cuentas_Determinar()
    {
        DataTable Dt_Generadas = (DataTable)Session["Grid_Determinar"];
        DataTable Dt_Omitidas = (DataTable)Session["Grid_Omitida"];
        try
        {
            Decimal Suma_Total_Etapa = 0;

            for (int Contador = 0; Contador < Dt_Generadas.Rows.Count; Contador++)
            {
                Suma_Total_Etapa += Convert.ToDecimal(Dt_Generadas.Rows[Contador][10]);
            }
            Cls_Ope_Pre_Pae_Etapas_Negocio Alta_Cuentas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
            Alta_Cuentas.P_Despacho_Id = Cmb_Despachos.SelectedValue;
            Alta_Cuentas.P_Numero_Entrega = Txt_No_Entrega.Text;
            Alta_Cuentas.P_Total_Etapa = Suma_Total_Etapa.ToString();
            Alta_Cuentas.P_Modo_Generacion = "Normal";//Cambiar
            Alta_Cuentas.P_Nombre_Archivo = "";
            Alta_Cuentas.P_Comentario = Txt_Comentarios.Text;
            Alta_Cuentas.P_Proceso_Actual = "DETERMINACION";
            Alta_Cuentas.P_Proceso_Anterior = "";
            Alta_Cuentas.P_Dt_Generadas = Dt_Generadas;
            Alta_Cuentas.P_Dt_Omitidas = Dt_Omitidas;
            Alta_Cuentas.Alta_Pae_Etapas();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Cuentas a Determinar", "alert('Alta Exitosa');", true);
            Limpiar_Formulario();
            Estado_Formulario(false);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Archivo_Cuentas
    ///DESCRIPCIÓN          : Carga un archivo con cuentas predial para determinarlas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 21/02/2012 06:45:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Archivo_Cuentas(string Ruta_Archivo)
    {
        String Cadena_Conexion = "";


        if (Ruta_Archivo.Contains(".xlsx"))       // Formar la cadena de conexion si el archivo es Exceml xml
        {
            Cadena_Conexion = @"Provider=Microsoft.ACE.OLEDB.12.0;" +
                    "Data Source=" + Ruta_Archivo + ";" +
                    "Extended Properties=\"Excel 12.0 Xml;HDR=YES\"";
        }
        else if (Ruta_Archivo.Contains(".xls"))   // Formar la cadena de conexion si el archivo es Exceml binario
        {
            Cadena_Conexion = @"Provider=Microsoft.Jet.OLEDB.4.0;" +
                    "Data Source=" + Ruta_Archivo + ";" +
                    "Extended Properties=Excel 8.0;";
        }

        OleDbConnection Ole_Conexion = new OleDbConnection(Cadena_Conexion);
        OleDbCommand Ole_Comando_Excel = new OleDbCommand();
        OleDbDataAdapter Ole_Adaptador = new OleDbDataAdapter();
        DataTable Dt_Archivo = new DataTable();

        Ole_Comando_Excel.Connection = Ole_Conexion;
        //Get the name of First Sheet
        Ole_Conexion.Open();
        DataTable Dt_Excel_Archivo;
        Dt_Excel_Archivo = Ole_Conexion.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);

        for (int x = 0; x < Dt_Excel_Archivo.Rows.Count; x++)
        {
            string SheetName = Dt_Excel_Archivo.Rows[x]["TABLE_NAME"].ToString();
            Ole_Conexion.Close();

            //Read Data from First Sheet
            Ole_Conexion.Open();
            Ole_Comando_Excel.CommandText = "SELECT * From [" + SheetName + "]";
            Ole_Adaptador.SelectCommand = Ole_Comando_Excel;
            Ole_Adaptador.Fill(Dt_Archivo);
            Ole_Conexion.Close();
        }
        //Bind Data to GridView
        Grid_Cargar_Cuentas.Caption = Path.GetFileName(Ruta_Archivo);
        Grid_Cargar_Cuentas.DataSource = Dt_Archivo;
        Grid_Cargar_Cuentas.DataBind();
        //Mpe_Subir_Archivo.Show();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Comprobar_Filtros_Determinacione
    ///DESCRIPCIÓN          : Comprueba que la nueva fila que se agrego en la tabla de determinaciones generadas
    ///                       cumpla con los demas filtros seleccionados, si no cumple es borrada,
    ///                       regresa el numero de posicion en la tabla Dt_Determinaciones_Generadas
    ///PARAMETROS:          : 1.-Dt_Generadas: Se guardan los registros que cumplen con los filtos    ///                     : 
    ///                     : 2.-Cont_Borrado: Posicion actual de la tabla Dt_Determinaciones_Generadas
    ///                     : 3.-Cont_Det_Etapas: Posicion actual de la tabla Dt_Pre_Pae_Det_Etapas
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 06:32:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Comprobar_Filtros_Busqueda(DataTable Dt_Generadas, int Cont_Borrado, int Cont_Det_Etapas)
    {
        Boolean Fila_Borrada = false;

        if (Txt_Fecha_Inicial.Text.Length > 0 && Fila_Borrada != true)
        {
            DateTime Fecha_Inicial;
            Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text);
            String Fecha_Notificacion = Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString();//Es el campo de la tabla que acaba de ser creada
            if (!String.IsNullOrEmpty(Fecha_Notificacion))
            {
                if (Convert.ToDateTime(Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString()) < Fecha_Inicial)//Es el campo de la tabla que acaba de ser creada
                {
                    Dt_Generadas.Rows[Cont_Borrado].Delete();
                    Fila_Borrada = true;
                }
            }
        }
        if (Txt_Fecha_Final.Text.Length > 0 && Fila_Borrada != true)
        {
            DateTime Fecha_Final;
            Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text);
            Fecha_Final = Fecha_Final.AddHours(23).AddMinutes(59).AddSeconds(59);
            String Fecha_Notificacion = Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString();//Es el campo de la tabla que acaba de ser creada
            if (!String.IsNullOrEmpty(Fecha_Notificacion))
            {
                if (Convert.ToDateTime(Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString()) > Fecha_Final)//Es el campo de la tabla que acaba de ser creada
                {
                    Dt_Generadas.Rows[Cont_Borrado].Delete();
                    Fila_Borrada = true;
                }
            }
        }
        if (Fila_Borrada != true)
            Cont_Borrado++;

        return Cont_Borrado;
    }
    #endregion

    #region Eventos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta las cuentas que se van a determinar
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 04:12:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            if (Btn_Nuevo.AlternateText == "Nuevo")
            {
                Estado_Formulario(true);
                Grid_Determinaciones_Generadas.DataBind();
            }
            else
            {

                if (Validar_Componentes())
                {
                    //Estado_Formulario(false);
                    Guardar_Cuentas_Determinar();
                }

            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta) o Sale del Formulario.
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                HttpContext.Current.Session.Remove("Activa");
            }
            else
            {
                Btn_Salir.AlternateText = "Salir";
                Estado_Formulario(false);
                HttpContext.Current.Session.Remove("Activa");
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Seleccionar_Colonia_Click
    ///DESCRIPCIÓN          : Llama un formulario para buscar la colonia
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 09:14:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Colonia_Click(object sender, ImageClickEventArgs e)
    {
        Busqueda_Colonia_Calle();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Seleccionar_Calle_Click
    ///DESCRIPCIÓN          : Llama un formulario para buscar la calle
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 04:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Calle_Click(object sender, ImageClickEventArgs e)
    {
        Busqueda_Colonia_Calle();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Subir_Archivo_Click
    ///DESCRIPCIÓN          : Llena el Grid de la cuentas a determinar
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 06:30:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Subir_Archivo_Click(object sender, ImageClickEventArgs e)
    {
        //Hdn_Modo_Generacion.Value = "Archivo";
        //Mpe_Subir_Archivo.Show();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Despachos_SelectedIndexChanged
    ///DESCRIPCIÓN          : Obtiene el numero de entrega del despacho cuando se selecciona
    ///                       un despacho
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 16/02/2012 01:37:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Despachos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime Año = DateTime.Now;
            Cls_Ope_Pre_Pae_Etapas_Negocio Obtener_No = new Cls_Ope_Pre_Pae_Etapas_Negocio();
            Obtener_No.P_Despacho_Id = Cmb_Despachos.SelectedValue;
            Txt_No_Entrega.Text = Obtener_No.Consultar_No_Entrega(Año.Year.ToString());
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Imprimir_Click
    ///DESCRIPCIÓN          : Imprime los datos, regresa las cuentas a determinar
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 16/02/2012 04:56:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Comprobacion_Termida = false;//Si es verdadero termina la comprobacion de los filtos
        DataTable Dt_Cuentas_Predial;
        Limpia_Mensaje_Error();//Limpia el mensaje error

        try
        {
            if (Btn_Imprimir.AlternateText == "Buscar")
            {
                Limpia_Mensaje_Error();
                Regresa_Cuentas_Determinar_Omitidas();
                Div_Generacion.Visible = true;
            }
            else
            {
                Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
                DataTable Dt_Busqueda = null;
                Rs_Etapas.P_Proceso_Actual = "DETERMINACION";

                if (Txt_Folio_Inicial.Text.Length > 0 || Txt_Folio_Final.Text.Length > 0)
                {
                    if (Cmb_Asignado_a.SelectedIndex < 1 && Comprobacion_Termida != true)
                    {
                        Mensaje_Error("Selecciona un despacho");
                        Comprobacion_Termida = true;
                    }
                }
                if (Txt_Domicilio.Text.Length < 1 && Cmb_Tipo_Domicilio.SelectedIndex > 0 && Comprobacion_Termida != true)
                {
                    Mensaje_Error("Selecciona un domicilio");
                    Comprobacion_Termida = true;
                }
                if (Comprobacion_Termida != true)
                {
                    if (Session["CUENTAS_PREDIAL_CONTRIBUYENTE"] != null && Session["CUENTAS_PREDIAL_CONTRIBUYENTE"].ToString() != "")
                    {
                        Dt_Cuentas_Predial = (DataTable)Session["CUENTAS_PREDIAL_CONTRIBUYENTE"];
                        for (int Cont_Contribuyente = 0; Cont_Contribuyente < Dt_Cuentas_Predial.Rows.Count; Cont_Contribuyente++)
                        {
                            Rs_Etapas.P_Cuenta_Predial_Id = Dt_Cuentas_Predial.Rows[Cont_Contribuyente][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();//Asigna el ID de la Cuenta Predial
                            if (Txt_Cuenta_Predial.Text.Length > 0)
                            {
                                Rs_Etapas.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;//Asigna el ID de la Cuenta Predial                
                            }

                            if (Cmb_Asignado_a.SelectedIndex > 0)//Si la busqueda es por cuentas asignadas al despacho determinado
                            {
                                Rs_Etapas.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                            }

                            if (Cmb_Estatus.SelectedIndex > 0)
                            {
                                Rs_Etapas.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                            }

                            if (Cmb_Tipo_Predio.SelectedIndex > 0)
                            {
                                Rs_Etapas.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedValue.ToString();
                            }
                            if (Txt_Domicilio.Text.Length > 0 && Cmb_Tipo_Domicilio.SelectedIndex > 0)
                            {
                                if (Cmb_Tipo_Domicilio.SelectedIndex == 1)//Comprueba que tipo de domicilio fue seleccionado
                                {
                                    Rs_Etapas.P_Colonia_ID = Hdn_Colonia_ID.Value;
                                    Rs_Etapas.P_Calle_ID = Hdn_Calle_ID.Value;
                                }
                                else
                                {
                                    Rs_Etapas.P_Colonia_ID_Notificacion = Hdn_Colonia_ID.Value;
                                    Rs_Etapas.P_Calle_ID_Notificacion = Hdn_Calle_ID.Value;
                                }
                            }
                            if (Txt_Folio_Inicial.Text.Length > 0)
                            {
                                Rs_Etapas.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                            }
                            if (Txt_Folio_Final.Text.Length > 0)
                            {
                                Rs_Etapas.P_Folio_Final = Txt_Folio_Final.Text;
                            }
                            if (Txt_Fecha_Inicial.Text.Length > 0)
                            {
                                Rs_Etapas.P_Fecha_Creo_Ini = Txt_Fecha_Inicial.Text;
                            }
                            if (Txt_Fecha_Final.Text.Length > 0)
                            {
                                Rs_Etapas.P_Fecha_Creo_Ini = Txt_Fecha_Final.Text;
                            }
                            Dt_Busqueda = Rs_Etapas.Consulta_Reporte_Impresas();
                        }
                    }
                    else
                    {

                        if (Txt_Cuenta_Predial.Text.Length > 0)
                        {
                            Rs_Etapas.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;//Asigna el ID de la Cuenta Predial                
                        }

                        if (Cmb_Asignado_a.SelectedIndex > 0)//Si la busqueda es por cuentas asignadas al despacho determinado
                        {
                            Rs_Etapas.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                        }

                        if (Cmb_Estatus.SelectedIndex > 0)
                        {
                            Rs_Etapas.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                        }

                        if (Cmb_Tipo_Predio.SelectedIndex > 0)
                        {
                            Rs_Etapas.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedValue.ToString();
                        }
                        if (Txt_Domicilio.Text.Length > 0 && Cmb_Tipo_Domicilio.SelectedIndex > 0)
                        {
                            if (Cmb_Tipo_Domicilio.SelectedIndex == 1)//Comprueba que tipo de domicilio fue seleccionado
                            {
                                Rs_Etapas.P_Colonia_ID = Hdn_Colonia_ID.Value;
                                Rs_Etapas.P_Calle_ID = Hdn_Calle_ID.Value;
                            }
                            else
                            {
                                Rs_Etapas.P_Colonia_ID_Notificacion = Hdn_Colonia_ID.Value;
                                Rs_Etapas.P_Calle_ID_Notificacion = Hdn_Calle_ID.Value;
                            }
                        }
                        if (Txt_Folio_Inicial.Text.Length > 0)
                        {
                            Rs_Etapas.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                        }
                        if (Txt_Folio_Final.Text.Length > 0)
                        {
                            Rs_Etapas.P_Folio_Final = Txt_Folio_Final.Text;
                        }
                        Dt_Busqueda = Rs_Etapas.Consulta_Reporte_Impresas();
                    }


                    DataTable Dt_Impresiones = new Ds_Ope_Pre_PAE_Impresas().Tables["IMPRESIONES"].Clone();
                    Dt_Impresiones.TableName = "IMPRESIONES";
                    if (Dt_Busqueda != null && Dt_Busqueda.Rows.Count > 0)
                    {
                        foreach (DataRow FilaSeleccionada in Dt_Busqueda.Rows)
                        {
                            if (String.IsNullOrEmpty(FilaSeleccionada["FECHA_NOTI"].ToString()))
                            {
                                DataRow Fila = Dt_Impresiones.NewRow();
                                if (FilaSeleccionada["DOMICILIO_FORANEO"].ToString() == "NO")
                                {
                                    Fila["CALLE_ID"] = FilaSeleccionada["CALLE_ID"];
                                    Fila["NOM_CALLE"] = FilaSeleccionada["NOM_CALLE"];
                                    Fila["COLONIA_ID"] = FilaSeleccionada["COLONIA_ID"];
                                    Fila["NOM_COLONIA"] = FilaSeleccionada["NOM_COLONIA"];
                                    Fila["NO_EXTERIOR"] = FilaSeleccionada["NO_EXTERIOR"];
                                    Fila["NO_INTERIOR"] = FilaSeleccionada["NO_INTERIOR"];
                                    Fila["DOMICILIO_FORANEO"] = FilaSeleccionada["DOMICILIO_FORANEO"];
                                    Fila["CALLE_ID_NOTIFICACION"] = FilaSeleccionada["CALLE_ID_NOTIFICACION"];
                                    Fila["NOM_CALLE_NOTI"] = FilaSeleccionada["NOM_CALLE_NOTI"];
                                    Fila["CALLE_NOTIFICACION"] = FilaSeleccionada["CALLE_NOTIFICACION"];
                                    Fila["COLONIA_ID_NOTIFICACION"] = FilaSeleccionada["COLONIA_ID_NOTIFICACION"];
                                    Fila["NOM_COLONIA_NOTI"] = FilaSeleccionada["NOM_COLONIA_NOTI"];
                                    Fila["COLONIA_NOTIFICACION"] = FilaSeleccionada["COLONIA_NOTIFICACION"];
                                    Fila["NO_EXTERIOR_NOTIFICACION"] = FilaSeleccionada["NO_EXTERIOR_NOTIFICACION"];
                                    Fila["NO_INTERIOR_NOTIFICACION"] = FilaSeleccionada["NO_INTERIOR_NOTIFICACION"];
                                }
                                else
                                {
                                    Fila["CALLE_ID"] = FilaSeleccionada["CALLE_ID"];
                                    Fila["NOM_CALLE"] = FilaSeleccionada["NOM_CALLE"];
                                    Fila["COLONIA_ID"] = FilaSeleccionada["COLONIA_ID"];
                                    Fila["NOM_COLONIA"] = FilaSeleccionada["NOM_COLONIA"];
                                    Fila["NO_EXTERIOR"] = FilaSeleccionada["NO_EXTERIOR"];
                                    Fila["NO_INTERIOR"] = FilaSeleccionada["NO_INTERIOR"];
                                    Fila["DOMICILIO_FORANEO"] = FilaSeleccionada["DOMICILIO_FORANEO"];
                                    Fila["CALLE_ID_NOTIFICACION"] = FilaSeleccionada["CALLE_ID"];
                                    Fila["NOM_CALLE_NOTI"] = FilaSeleccionada["NOM_CALLE"];
                                    Fila["CALLE_NOTIFICACION"] = FilaSeleccionada["CALLE_NOTIFICACION"];
                                    Fila["COLONIA_ID_NOTIFICACION"] = FilaSeleccionada["COLONIA_ID"];
                                    Fila["NOM_COLONIA_NOTI"] = FilaSeleccionada["NOM_COLONIA"];
                                    Fila["COLONIA_NOTIFICACION"] = FilaSeleccionada["COLONIA_NOTIFICACION"];
                                    Fila["NO_EXTERIOR_NOTIFICACION"] = FilaSeleccionada["NO_EXTERIOR"];
                                    Fila["NO_INTERIOR_NOTIFICACION"] = FilaSeleccionada["NO_INTERIOR"];
                                }
                                Fila["CUENTA_PREDIAL"] = FilaSeleccionada["CUENTA_PREDIAL"];
                                Fila["FOLIO"] = FilaSeleccionada["FOLIO"];
                                Fila["CONTRIBUYENTE"] = FilaSeleccionada["CONTRIBUYENTE"];
                                Fila["VALOR_FISCAL"] = FilaSeleccionada["VALOR_FISCAL"];
                                Fila["PERIODO_REZAGO"] = FilaSeleccionada["PERIODO_REZAGO"];
                                Fila["ADEUDO_REZAGO"] = FilaSeleccionada["ADEUDO_REZAGO"];
                                Fila["PERIODO_CORRIENTE"] = FilaSeleccionada["PERIODO_CORRIENTE"];
                                Fila["ADEUDO_CORRIENTE"] = FilaSeleccionada["ADEUDO_CORRIENTE"];
                                Fila["RECARGOS"] = FilaSeleccionada["RECARGOS"];
                                Fila["TOTAL_PAGAR"] = FilaSeleccionada["TOTAL_PAGAR"];
                                Fila["EFECTOS"] = FilaSeleccionada["EFECTOS"];
                                Fila["TASA_ID"] = FilaSeleccionada["TASA_ID"];
                                Fila["TASA_ANUAL"] = FilaSeleccionada["TASA_ANUAL"];
                                Fila["CUOTA_BIMESTRAL"] = FilaSeleccionada["CUOTA_BIMESTRAL"];
                                Fila["CUENTA_PREDIAL_ID"] = FilaSeleccionada["CUENTA_PREDIAL_ID"];
                                Fila["HONORARIOS"] = FilaSeleccionada["HONORARIOS"];
                                Fila["TOTAL_REQ"] = FilaSeleccionada["TOTAL_REQ"];
                                Dt_Impresiones.Rows.Add(Fila);
                            }
                        }
                        DataSet Ds_Impresiones = null;
                        Ds_Impresiones = new DataSet();
                        Ds_Impresiones.Tables.Add(Dt_Impresiones.Copy());
                        Generar_Reporte(ref Ds_Impresiones, "Rpt_Ope_Pre_PAE_Impresiones_Determinacion.rpt", "Impresiones_PAE_" + Session.SessionID + ".pdf");
                    }
                    else
                    {
                        Mensaje_Error("No existen cuentas para imprimir, posiblemente ya estén impresas ó  no haya cuentas en esta etapa.");
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Cargar_Archivo_Click
    ///DESCRIPCIÓN          : Abre una venta para cargar un archivo y llama al metodo
    ///                       Cargar_Archivo_Cuentas() el cual carga las cuentas a un Grid
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 21/02/2012 04:00:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cargar_Archivo_Click(object sender, EventArgs e)
    {
        try
        {
            String Ruta_Archivo;
            if (Fle_Cargar_Archivo.HasFile)
            {
                //Si el directorio no existe, crearlo
                if (!Directory.Exists(MapPath("Sincronizacion_Presupuestos")))
                    Directory.CreateDirectory(MapPath("Sincronizacion_Presupuestos"));
                // Guardar archivo en el servidor con un nombre especifico (incluyendo fecha)
                Ruta_Archivo = MapPath("Sincronizacion_Presupuestos/presupuestos_" + DateTime.Now.ToString("yyyyMMdd_HHmmss") + Path.GetExtension(Fle_Cargar_Archivo.FileName));
                Fle_Cargar_Archivo.SaveAs(Ruta_Archivo);
                Fle_Cargar_Archivo.FileContent.Close();

                Cargar_Archivo_Cuentas(Ruta_Archivo);
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Seleccionar_Propietario_Click
    ///DESCRIPCIÓN          : Llama un formulario para buscar el nombre del propietario,
    ///                       cuenta predial, calle o colonia
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 22/02/2012 11:00:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Propietario_Click(object sender, ImageClickEventArgs e)
    {
        Busqueda_Propietario();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Busca_Domicilio_Click
    ///DESCRIPCIÓN          : Llama un formulario para buscar la colonia y calle
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 12:44:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busca_Domicilio_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
                {
                    Hdn_Colonia_ID.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    Hdn_Calle_ID.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    if (Hdn_Calle_ID.Value != null && Hdn_Calle_ID.Value != "")
                        Txt_Domicilio.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "") + ", Calle " + Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
                    else
                        Txt_Domicilio.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Determinaciones_Click
    ///DESCRIPCIÓN          : Llena el Grid de determinaciones Generadas, con una consulta
    ///                       que tiene varios filtros
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 10:32:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Determinaciones_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Pae_Etapas_Negocio Etapas_PAE = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Cuentas_Predial;
        DataTable Dt_Determinaciones_Generadas = Crear_Tabla_Determinaciones_Generadas();//Se crea la tabla para pasarla algrid
        DataTable Dt_Busqueda;

        Boolean Comprobacion_Termida = false;//Si es verdadero termina la comprobacion de los filtos
        Int32 Cont_Borrado = 0;//Posicion de la Tabla Dt_Determinaciones_Generadas
        Limpia_Mensaje_Error();//Limpia el mensaje error
        Etapas_PAE.P_Proceso_Actual = "DETERMINACION";
        try
        {
            if (Txt_Folio_Inicial.Text.Length > 0 || Txt_Folio_Final.Text.Length > 0)
            {
                if (Cmb_Asignado_a.SelectedIndex < 1 && Comprobacion_Termida != true)
                {
                    Mensaje_Error("Selecciona un despacho");
                    Comprobacion_Termida = true;
                }
            }
            if (Txt_Domicilio.Text.Length < 1 && Cmb_Tipo_Domicilio.SelectedIndex > 0 && Comprobacion_Termida != true)
            {
                Mensaje_Error("Selecciona un domicilio");
                Comprobacion_Termida = true;
            }
            if (Comprobacion_Termida != true)
            {
                if (Session["CUENTAS_PREDIAL_CONTRIBUYENTE"] != null && Session["CUENTAS_PREDIAL_CONTRIBUYENTE"].ToString() != "")
                {
                    Dt_Cuentas_Predial = (DataTable)Session["CUENTAS_PREDIAL_CONTRIBUYENTE"];
                    for (int Cont_Contribuyente = 0; Cont_Contribuyente < Dt_Cuentas_Predial.Rows.Count; Cont_Contribuyente++)
                    {
                        Etapas_PAE.P_Cuenta_Predial_Id = Dt_Cuentas_Predial.Rows[Cont_Contribuyente][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();//Asigna el ID de la Cuenta Predial
                        if (Txt_Cuenta_Predial.Text.Length > 0)
                        {
                            Etapas_PAE.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;//Asigna el ID de la Cuenta Predial                
                        }

                        if (Cmb_Asignado_a.SelectedIndex > 0)//Si la busqueda es por cuentas asignadas al despacho determinado
                        {
                            Etapas_PAE.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                        }

                        if (Cmb_Estatus.SelectedIndex > 0)
                        {
                            Etapas_PAE.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                        }

                        if (Cmb_Tipo_Predio.SelectedIndex > 0)
                        {
                            Etapas_PAE.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedValue.ToString();
                        }
                        if (Txt_Domicilio.Text.Length > 0 && Cmb_Tipo_Domicilio.SelectedIndex > 0)
                        {
                            if (Cmb_Tipo_Domicilio.SelectedIndex == 1)//Comprueba que tipo de domicilio fue seleccionado
                            {
                                Etapas_PAE.P_Colonia_ID = Hdn_Colonia_ID.Value;
                                Etapas_PAE.P_Calle_ID = Hdn_Calle_ID.Value;
                            }
                            else
                            {
                                Etapas_PAE.P_Colonia_ID_Notificacion = Hdn_Colonia_ID.Value;
                                Etapas_PAE.P_Calle_ID_Notificacion = Hdn_Calle_ID.Value;
                            }
                        }
                        if (Txt_Folio_Inicial.Text.Length > 0)
                        {
                            Etapas_PAE.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                        }
                        if (Txt_Folio_Final.Text.Length > 0)
                        {
                            Etapas_PAE.P_Folio_Final = Txt_Folio_Final.Text;
                        }
                        Dt_Busqueda = Etapas_PAE.Consultar_Pae_Det_Etapas();
                        for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
                        {
                            Llenar_DataRow_Determinadas(Dt_Determinaciones_Generadas, Dt_Busqueda, Contador);
                        }
                    }
                }
                else
                {

                    if (Txt_Cuenta_Predial.Text.Length > 0)
                    {
                        Etapas_PAE.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;//Asigna el ID de la Cuenta Predial                
                    }

                    if (Cmb_Asignado_a.SelectedIndex > 0)//Si la busqueda es por cuentas asignadas al despacho determinado
                    {
                        Etapas_PAE.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                    }

                    if (Cmb_Estatus.SelectedIndex > 0)
                    {
                        Etapas_PAE.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                    }

                    if (Cmb_Tipo_Predio.SelectedIndex > 0)
                    {
                        Etapas_PAE.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedValue.ToString();
                    }
                    if (Txt_Domicilio.Text.Length > 0 && Cmb_Tipo_Domicilio.SelectedIndex > 0)
                    {
                        if (Cmb_Tipo_Domicilio.SelectedIndex == 1)//Comprueba que tipo de domicilio fue seleccionado
                        {
                            Etapas_PAE.P_Colonia_ID = Hdn_Colonia_ID.Value;
                            Etapas_PAE.P_Calle_ID = Hdn_Calle_ID.Value;
                        }
                        else
                        {
                            Etapas_PAE.P_Colonia_ID_Notificacion = Hdn_Colonia_ID.Value;
                            Etapas_PAE.P_Calle_ID_Notificacion = Hdn_Calle_ID.Value;
                        }
                    }
                    if (Txt_Folio_Inicial.Text.Length > 0)
                    {
                        Etapas_PAE.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                    }
                    if (Txt_Folio_Final.Text.Length > 0)
                    {
                        Etapas_PAE.P_Folio_Final = Txt_Folio_Final.Text;
                    }
                    Dt_Busqueda = Etapas_PAE.Consultar_Pae_Det_Etapas();
                    for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
                    {
                        Llenar_DataRow_Determinadas(Dt_Determinaciones_Generadas, Dt_Busqueda, Contador);
                    }
                }
                //Comprobar fechas
                for (int Cont_Fechas = 0; Cont_Fechas < Dt_Determinaciones_Generadas.Rows.Count; Cont_Fechas++)
                {
                    Comprobar_Filtros_Busqueda(Dt_Determinaciones_Generadas, Cont_Borrado, Cont_Fechas);
                }

                Grid_Determinaciones_Generadas.DataSource = Dt_Determinaciones_Generadas;
                Grid_Determinaciones_Generadas.DataBind();
                Session["Grid_Determinaciones_Generadas"] = Dt_Determinaciones_Generadas;
                Session.Remove("CUENTAS_PREDIAL_CONTRIBUYENTE");

                foreach (Control Txt_Lmpia in Div_Generadas.Controls)
                {
                    if (Txt_Lmpia is DropDownList)
                    {
                        ((DropDownList)Txt_Lmpia).SelectedValue = "0";
                    }
                }
                Limpiar_Formulario();
                if (Dt_Determinaciones_Generadas.Rows.Count < 1)
                {
                    Mensaje_Error("No se encontraron Determinaciones con esos parametros");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Busca_Cuenta_Click
    ///DESCRIPCIÓN          : Obtiene la cuenta predial de la ventana emergente por medio de las sessiones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busca_Cuenta_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Session["BUSQUEDA_CUENTAS_PREDIAL"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]) == true)
                {
                    Txt_Cuenta_Predial.Text = HttpUtility.HtmlDecode(Session["CUENTA_PREDIAL"].ToString().Replace("&nbsp;", ""));
                    Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
                    Session.Remove("CUENTA_PREDIAL");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Click
    ///DESCRIPCIÓN          : Obtiene la cuenta predial de la ventana emergente por medio de las sessiones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
            DataTable Dt_Determinaciones_Generadas = Crear_Tabla_Determinaciones_Generadas();//Se crea la tabla para pasarla algrid
            DataTable Dt_Busqueda = null;
            Limpia_Mensaje_Error();
            Rs_Etapas.P_Folio = Txt_Busqueda.Text;
            Rs_Etapas.P_Proceso_Actual = "DETERMINACION";
            Dt_Busqueda = Rs_Etapas.Consultar_Pae_Det_Etapas();
            for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
            {
                Llenar_DataRow_Determinadas(Dt_Determinaciones_Generadas, Dt_Busqueda, Contador);
            }
            Grid_Determinaciones_Generadas.DataSource = Dt_Determinaciones_Generadas;
            Grid_Determinaciones_Generadas.DataBind();
            Session["Grid_Determinaciones_Generadas"] = Dt_Determinaciones_Generadas;
            Txt_Busqueda.Text = "";
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #region Texbox
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Inicial_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Inicial_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Inicial.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Inicial.Text, out Fecha_valida))
            {
                Txt_Fecha_Inicial.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Inicial.Text = "";
            }
        }
    }
        ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Final_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Final_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Final.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Final.Text, out Fecha_valida))
            {
                Txt_Fecha_Final.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Final.Text = "";
            }
        }
    }
    #endregion
    #endregion

    #region Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Cuentas_Generar_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de las Cuentas a Generar
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/02/2012 11:41:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cuentas_Generar_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Grid_Cuentas_Generar.SelectedIndex = (-1);
            Grid_Cuentas_Generar.PageIndex = e.NewPageIndex;
            Grid_Cuentas_Generar.DataSource = Session["Grid_Determinar"];
            Grid_Cuentas_Generar.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Cuentas_Omitidas_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de las Cuentas Omitidas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 09/02/2012 06:14:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cuentas_Omitidas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Cuentas_Omitidas.PageIndex = e.NewPageIndex;
            Grid_Cuentas_Omitidas.DataSource = Session["Grid_Omitida"];
            Grid_Cuentas_Omitidas.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Cuentas_Omitidas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento del grid de Omitidas cambia una cuenta omitida a 
    ///                       cuentas a determinar
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 10/02/2012 12:34:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cuentas_Omitidas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cambiar_Cuenta_A_Determinar();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Cuentas_Omitidas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento del grid de Omitidas cambia una cuenta que esta para Determinar 
    ///                       a cuentas omitidas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 10/02/2012 06:33:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Cuentas_Generar_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Session["BUSQUEDA_MOTIVO"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_MOTIVO"]) == true)
                {
                    Hdn_Motivo_Omision.Value = Session["MOTIVO"].ToString().Replace("&nbsp;", "");
                    Cambiar_Cuenta_A_Omitidas();
                }
            }
            Session.Remove("BUSQUEDA_MOTIVO");
            Session.Remove("MOTIVO");
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Determinaciones_Generadas_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de las Cuentas que Se Determinaron
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 06:30:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Determinaciones_Generadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Grid_Cuentas_Generar.SelectedIndex = (-1);            
            Grid_Determinaciones_Generadas.PageIndex = e.NewPageIndex;
            Grid_Determinaciones_Generadas.DataSource = Session["Grid_Determinaciones_Generadas"];
            Grid_Determinaciones_Generadas.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion
}
