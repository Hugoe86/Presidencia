using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Reportes_Nomina.Reporteador_Empleados.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using System.Web.Security;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Empleados.Negocios;


public partial class paginas_Nomina_Frm_Rpt_Nom_Aportacion : System.Web.UI.Page
{
    #region "Page_Load"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Evento se carga al iniciar la pagina
    ///PARAMETROS:  
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 04/Abril/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        Mensaje_Error();
        if (!IsPostBack)
        {
            Consultar_Unidades_Responsables();
            Llenar_Combo_Tipos_Nomina();
        }
        Cmb_Tipos_Nomina.Focus();
    }

    #endregion

    #region "Metodos"

    /// ***********************************************************************************************
    /// NOMBRE: Consulta_Unidades_Responsables
    /// 
    /// DESCRIPCIÓN: consulta las unidades responsables registradas en sistema.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Consultar_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        System.Data.DataTable Dt_Unidades_Responsables = null;//Variable que almacenara las dependencias.

        try
        {
            Dt_Unidades_Responsables = Obj_Dependencias.Consulta_Dependencias();

            Cmb_Unidad_Responsable.DataSource = Dt_Unidades_Responsables;
            Cmb_Unidad_Responsable.DataTextField = "CLAVE_NOMBRE";
            Cmb_Unidad_Responsable.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Unidad_Responsable.DataBind();

            Cmb_Unidad_Responsable.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Unidades_Responsables
    ///DESCRIPCIÓN: Se llena el Combo de las Dependencias
    ///PARAMETROS:  1.- Tipo.- Tipo de Reporte.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_Tipos_Nomina()
    {
        Cmb_Tipos_Nomina.DataSource = Cls_Rpt_Nom_Reporteador_Empleados_Negocio.Consultar_Tipos_Nomina();
        Cmb_Tipos_Nomina.DataTextField = "NOMBRE";
        Cmb_Tipos_Nomina.DataValueField = "TIPO_NOMINA_ID";
        Cmb_Tipos_Nomina.DataBind();
        Cmb_Tipos_Nomina.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mensaje_Error
    ///DESCRIPCIÓN: Se crea un mensaje de error, por medio de un Label y una Imagen de 
    ///             Precaucion
    ///PARAMETROS:  1.- Mensaje.- Cadena de caracteres que tendra el mensaje.
    ///CREO: Armando Zavala Moreno.
    ///FECHA_CREO: 10/Abril/2012 03:10:00 a.m
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mensaje_Error(String Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text = Mensaje;
        Lbl_Mensaje_Error.Visible = true;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mensaje_Error
    ///DESCRIPCIÓN: Borra la cadena de caracteres que tiene la etiqueta para mostrar el
    ///             mensaje de error, hace invisible la etiqueta y la imagen de precaucion
    ///PARAMETROS:  
    ///CREO: Armando Zavala Moreno.
    ///FECHA_CREO: 10/Abril/2012 03:10:00 a.m
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
    }
    #region (Reportes)
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
            Ruta = @Server.MapPath("../Rpt/Nomina/" + Nombre_Plantilla_Reporte);
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
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
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
            //// Response.End(); siempre genera una excepción (http://support.microsoft.com/kb/312629/EN-US/)
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
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
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// ***********************************************************************************************
    /// NOMBRE: Consulta_Puestos
    /// 
    /// DESCRIPCIÓN: Cosnulta los puestos de la unidad responsable seleccionada.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected System.Data.DataTable Consultar_Puestos()
    {
        System.Data.DataTable Dt_Puestos = null;
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            Obj_Empleados.P_Estatus = "OCUPADO";
            if (Cmb_Unidad_Responsable.SelectedIndex > 0) Obj_Empleados.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();
            if (Cmb_Tipos_Nomina.SelectedIndex > 0) Obj_Empleados.P_Tipo_Nomina_ID = Cmb_Tipos_Nomina.SelectedValue;
            if (Txt_Nombre_Empleado.Text.Length > 0) Obj_Empleados.P_Nombre = Txt_Nombre_Empleado.Text;
            if (Txt_No_Empleado.Text.Length > 0) Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text;

            Dt_Puestos = Obj_Empleados.Consultar_Aportacion();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los puestos. Error: [" + Ex.Message + "]");
        }
        return Dt_Puestos;
    }
    #endregion

    #endregion
    #region (Eventos)
        /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Excel_Click
    /// 
    /// DESCRIPCIÓN: Exporta el reporte a un archivo de Word Excel
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Armando Zavala Moreno
    /// FECHA CREÓ: 11/Abril/2012 05:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************    
    protected void Btn_Generar_Reporte_Excel_Click(Object sender, EventArgs e)
    {
        System.Data.DataTable Dt_Puestos = null;
        System.Data.DataSet Ds_Puestos = null;

        try
        {
            Ds_Puestos = new System.Data.DataSet();
            Dt_Puestos = Consultar_Puestos();
            Dt_Puestos.TableName = "Empleado";
            Ds_Puestos.Tables.Add(Dt_Puestos.Copy());

            Exportar_Reporte(Ds_Puestos, "Cr_Rpt_Nom_Aportacion.rpt", "Reporte_Aportación" + Session.SessionID, "xls", ExportFormatType.Excel);            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.ToString());
        }
    }
    /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Word_Click
    /// 
    /// DESCRIPCIÓN: Exporta el reporte a un archivo de Word
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Armando Zavala Moreno
    /// FECHA CREÓ: 11/Abril/2012 05:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Btn_Generar_Reporte_Word_Click(Object sender, EventArgs e)
    {
        System.Data.DataTable Dt_Puestos = null;
        System.Data.DataSet Ds_Puestos = null;

        try
        {
            Ds_Puestos = new System.Data.DataSet();
            Dt_Puestos = Consultar_Puestos();
            Dt_Puestos.TableName = "Empleado";
            Ds_Puestos.Tables.Add(Dt_Puestos.Copy());

            Exportar_Reporte(Ds_Puestos, "Cr_Rpt_Nom_Aportacion.rpt", "Reporte_Aportación" + Session.SessionID, "doc", ExportFormatType.WordForWindows);            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.ToString());
        }
    }
    #endregion
}
