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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.ReportSource;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Sessiones;
using Presidencia.Reporte_Credito_Fonacot.Negocio;
using System.Text;

public partial class paginas_Nomina_Frm_Rpt_Nom_Estado_Cuentas_Credito_Fonacot : System.Web.UI.Page
{
    #region (Load/Init)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Refresca la session del usuario lagueado al sistema.
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            //Valida que exista algun usuario logueado al sistema.
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Limpia_Controles();//Limpia los controles de la forma
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Metodos)
    #region (Generales)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 21-Febrero-2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpia_Controles()
    {
        try
        {
            Cmb_Empleado.SelectedIndex = 0;
            Txt_Folio_Fonacot.Text = "";
            Txt_No_credito.Text = "";
            Txt_Nombre_Empleado.Text = "";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Buscar_Empleado_Click
    /// DESCRIPCION : Consulta a todos los Empleados que coincidan con el nombre
    /// PARAMETROS  : 
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 09-Abril-2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!String.IsNullOrEmpty(Txt_Nombre_Empleado.Text))
            {
                if (!string.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                {
                    Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Nombre_Empleado.Text.Trim();
                }
                Rs_Consulta_Ca_Empleados.P_Estatus = "ACTIVO";
                Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General();
                Cmb_Empleado.DataSource = new DataTable();
                Cmb_Empleado.DataBind();
                Cmb_Empleado.DataSource = Dt_Empleados;
                Cmb_Empleado.DataTextField = "Empleado";
                Cmb_Empleado.DataValueField = Cat_Empleados.Campo_Empleado_ID;
                Cmb_Empleado.DataBind();
                Cmb_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                Cmb_Empleado.SelectedIndex = -1;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }   
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 21-Febrero-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
            "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #region(Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Reporte
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el reporte
    /// CREO        : Sergio Manuel Gallardo Andrade
    /// FECHA_CREO  : 18/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Reporte()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";

        if (Cmb_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione algun empleado.<br>";
            Datos_Validos = false;
        }
        
        return Datos_Validos;
    }
    #endregion
    #region (Consulta)
    //*******************************************************************************
    // NOMBRE DE LA FUNCION: Consulta_Estado_Cuenta_Credito_Fonacot
    // DESCRIPCION : Consulta todos las cuentas de credito fonacot por empleado
    // PARAMETROS  : 
    // CREO        : Sergio Manuel Gallardo Andrade
    // FECHA_CREO  : 09-abril-2012
    // MODIFICO          :
    // FECHA_MODIFICO    :
    // CAUSA_MODIFICACION:
    //*******************************************************************************
    private void Consulta_Estado_Cuenta_Credito_Fonacot()
    {
        Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio Rs_Consulta = new Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio(); //Conexion hacia la capa de negocios
        DataTable Dt_Tipo_Reporte = new DataTable(); //Variable a conter los valores a pasar al reporte
        DataTable Dt_Consulta = new DataTable();
        Ds_Rpt_Nom_Credito_Fonacot Ds_Reporte = new Ds_Rpt_Nom_Credito_Fonacot();
        ReportDocument Reporte = new ReportDocument();
        String Espacios_Blanco;
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Estado_Cuentas_Credito_Fonacot"+ Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al document
        try
        {
            Rs_Consulta.P_Empleado_ID  = Cmb_Empleado.SelectedValue;
            Rs_Consulta.P_Usuario_Creo = Cls_Sessiones.Empleado_ID.Trim();
            if(!String.IsNullOrEmpty(Txt_Folio_Fonacot.Text)){
              Rs_Consulta.P_Folio_Fonacot= Txt_Folio_Fonacot.Text.Trim();
            }
             if(!String.IsNullOrEmpty(Txt_No_credito.Text)){
              Rs_Consulta.P_No_Credito= Txt_No_credito.Text.Trim();
            }
                Dt_Consulta = Rs_Consulta.Consulta_CreditoS_Fonacot_Empleado();
                if (Dt_Consulta.Rows.Count > 0)
                {
                    Dt_Consulta.TableName = "Dt_Creditos_Fonacot";
                    Ds_Reporte.Clear();
                    Ds_Reporte.Tables.Clear();
                    Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
                    Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Credito_Fonacot.rpt");
                    Reporte.SetDataSource(Ds_Reporte);

                    DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();

                    Nombre_Archivo += ".pdf";
                    Ruta_Archivo = @Server.MapPath("../../Reporte/");
                    m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                    ExportOptions Opciones_Exportacion = new ExportOptions();
                    Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                    Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                    Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                    Reporte.Export(Opciones_Exportacion);

                    Abrir_Ventana(Nombre_Archivo);
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Limpia_Controles();
                }
                else
                {
                    Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }

        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Cuentas_Credito_Fonacot " + ex.Message.ToString(), ex);
        }
    }
    //*******************************************************************************
    // NOMBRE DE LA FUNCION: Consulta_Estado_Cuenta_Credito_Fonacot
    // DESCRIPCION : Consulta todos las cuentas de credito fonacot por empleado
    // PARAMETROS  : 
    // CREO        : Sergio Manuel Gallardo Andrade
    // FECHA_CREO  : 09-abril-2012
    // MODIFICO          :
    // FECHA_MODIFICO    :
    // CAUSA_MODIFICACION:
    //*******************************************************************************
    private void Consulta_Estado_Cuenta_Credito_Fonacot_Excel()
    {
        Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio Rs_Consulta = new Cls_Rpt_Nom_Reporte_Credito_Fonacot_Negocio(); //Conexion hacia la capa de negocios
        DataTable Dt_Tipo_Reporte = new DataTable(); //Variable a conter los valores a pasar al reporte
        DataTable Dt_Consulta = new DataTable();
        Ds_Rpt_Nom_Credito_Fonacot Ds_Reporte = new Ds_Rpt_Nom_Credito_Fonacot();
        ReportDocument Reporte = new ReportDocument();
        String Espacios_Blanco;
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Estado_Cuentas_Credito_Fonacot" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al document
        try
        {
            Rs_Consulta.P_Empleado_ID = Cmb_Empleado.SelectedValue;
            Rs_Consulta.P_Usuario_Creo = Cls_Sessiones.Empleado_ID.Trim();
            if (!String.IsNullOrEmpty(Txt_Folio_Fonacot.Text))
            {
                Rs_Consulta.P_Folio_Fonacot = Txt_Folio_Fonacot.Text.Trim();
            }
            if (!String.IsNullOrEmpty(Txt_No_credito.Text))
            {
                Rs_Consulta.P_No_Credito = Txt_No_credito.Text.Trim();
            }
            Dt_Consulta = Rs_Consulta.Consulta_CreditoS_Fonacot_Empleado();
            if (Dt_Consulta.Rows.Count > 0)
            {
                Dt_Consulta.TableName = "Dt_Creditos_Fonacot";
                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Credito_Fonacot.rpt", "Reporte_Nomina_Credito_Fonacot" + Session.SessionID, "xls", ExportFormatType.Excel);
                Limpia_Controles();
            }
            else
            {
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                Lbl_Mensaje_Error.Text = "";
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Cuentas_Credito_Fonacot " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    ///           1. Ds_Reporte: Dataset con datos a imprimir
    ///           2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    ///           3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Reporte);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte";
        }

        String Archivo_Reporte = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Reporte);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            if (Formato == ExportFormatType.Excel)
            {
                Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-excel");
            }
            else if (Formato == ExportFormatType.WordForWindows)
            {
                Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-word");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(string Ruta_Archivo, string Contenido)
    {
        try
        {
            System.IO.FileInfo ArchivoExcel = new System.IO.FileInfo(Ruta_Archivo);
            if (ArchivoExcel.Exists)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = Contenido;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ArchivoExcel.Name);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.WriteFile(ArchivoExcel.FullName);
                Response.End();
            }
        }
        catch (Exception Ex)
        {

            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }


    #endregion
 #endregion

    #region Eventos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Sergio Manuel Gallardo
    ///FECHA_CREO:  09-Abril-2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Consulta = new DataTable();
        try
        {

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Validar_Reporte())
            {
                    Consulta_Estado_Cuenta_Credito_Fonacot();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Excel_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Sergio Manuel Gallardo
    ///FECHA_CREO:  19/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            if (Validar_Reporte())
            {
                Consulta_Estado_Cuenta_Credito_Fonacot_Excel();

            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    #endregion



}
