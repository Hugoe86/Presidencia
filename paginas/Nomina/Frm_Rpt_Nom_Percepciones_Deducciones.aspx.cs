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
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using CrystalDecisions.Shared;
using CrystalDecisions.CrystalReports.Engine;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using System.Text;

public partial class paginas_Nomina_Frm_Rpt_Nom_Percepciones_Deducciones : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    /// **************************************************************************************************************************
    /// Nombre: Busqueda_Conceptos
    /// 
    /// Descripción: Ejecuta una busqueda avanzada por los filtros especificados de los conceptos.
    /// 
    /// Parámetros: Tipo.- Este parámetro indica si se tratara de obtener el consecutivo de la clave de percepciones o deducciones.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 4/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// **************************************************************************************************************************
    protected void Busqueda_Conceptos()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Conceptos = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocio.
        DataTable Dt_Conceptos = null;
        Int32 Total_Registros = 1;

        try
        {
            if (!String.IsNullOrEmpty(Txt_Clave_Percepcion_Deduccion_Busqueda.Text.Trim()))
                Obj_Conceptos.P_CLAVE = Txt_Clave_Percepcion_Deduccion_Busqueda.Text.Trim();

            if (Cmb_Tipo_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_TIPO = Cmb_Tipo_Busqueda.SelectedItem.Text.Trim();

            if (Cmb_Estatus_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_ESTATUS = Cmb_Estatus_Busqueda.SelectedItem.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Nombre_Busqueda.Text.Trim()))
                Obj_Conceptos.P_NOMBRE = Txt_Nombre_Busqueda.Text.Trim();

            if (Cmb_Aplica_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_TIPO_ASIGNACION = Cmb_Aplica_Busqueda.SelectedItem.Text.Trim();

            if (Cmb_Concepto_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_Concepto = Cmb_Concepto_Busqueda.SelectedItem.Text.Trim();

            Dt_Conceptos = Obj_Conceptos.Consultar_Percepciones_Deducciones_General();

            Dt_Conceptos.TableName = "percepciones_deducciones";
            DataSet Ds_Percepciones_Deducciones = new DataSet();
            Ds_Percepciones_Deducciones.Tables.Add(Dt_Conceptos.Copy());
            Generar_Reporte(ref Ds_Percepciones_Deducciones, "Cr_Rpt_Nom_Perc_Dedu.rpt", "Percepciones_Deducciones_" + Session.SessionID + ".pdf");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado en el método Busqueda_Conceptos. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************
    /// Nombre: Busqueda_Conceptos
    /// 
    /// Descripción: Ejecuta una busqueda avanzada por los filtros especificados de los conceptos.
    /// 
    /// Parámetros: Tipo.- Este parámetro indica si se tratara de obtener el consecutivo de la clave de percepciones o deducciones.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 4/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificación:
    /// **************************************************************************************************************************
    protected DataTable Busqueda_Conceptos_Excel()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Conceptos = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocio.
        DataTable Dt_Conceptos = null;

        try
        {
            if (!String.IsNullOrEmpty(Txt_Clave_Percepcion_Deduccion_Busqueda.Text.Trim()))
                Obj_Conceptos.P_CLAVE = Txt_Clave_Percepcion_Deduccion_Busqueda.Text.Trim();

            if (Cmb_Tipo_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_TIPO = Cmb_Tipo_Busqueda.SelectedItem.Text.Trim();

            if (Cmb_Estatus_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_ESTATUS = Cmb_Estatus_Busqueda.SelectedItem.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Nombre_Busqueda.Text.Trim()))
                Obj_Conceptos.P_NOMBRE = Txt_Nombre_Busqueda.Text.Trim();

            if (Cmb_Aplica_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_TIPO_ASIGNACION = Cmb_Aplica_Busqueda.SelectedItem.Text.Trim();

            if (Cmb_Concepto_Busqueda.SelectedIndex > 0)
                Obj_Conceptos.P_Concepto = Cmb_Concepto_Busqueda.SelectedItem.Text.Trim();

            Dt_Conceptos = Obj_Conceptos.Consultar_Percepciones_Deducciones_General();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado en el método Busqueda_Conceptos. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(CarlosAg.ExcelXmlWriter.Workbook Libro, String Nombre_Archivo)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Nombre_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Nominas_Negativas",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Busqueda_Conceptos();
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "<b>+</b> Código : [" + Ex.Message + "]";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Minimo_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Minimo_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Percepciones_Deducciones = null;//Variable que almacena un listado de empleados.
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.

        try
        {
            //Consultamos la información del empleado.
            Dt_Percepciones_Deducciones = Busqueda_Conceptos_Excel();
            Dt_Percepciones_Deducciones = Ordenar_Tabla_Conceptos(Dt_Percepciones_Deducciones);
            //Obtenemos el libro.
            Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Percepciones_Deducciones);
            //Mandamos a imprimir el reporte en excel.
            Mostrar_Excel(Libro, "");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de empleados minimo. Error: [" + Ex.Message + "]");
        }
    }

    private DataTable Ordenar_Tabla_Conceptos(DataTable Dt_Conceptos_Sin_Ordenar)
    {
        DataTable Dt_Conceptos = new DataTable();

        try
        {
            Dt_Conceptos.Columns.Add("CLAVE" , typeof(String));
            Dt_Conceptos.Columns.Add("NOMBRE", typeof(String));
            Dt_Conceptos.Columns.Add("TIPO", typeof(String));
            Dt_Conceptos.Columns.Add("TIPO_ASIGNACION", typeof(String));
            Dt_Conceptos.Columns.Add("CONCEPTO", typeof(String));
            Dt_Conceptos.Columns.Add("ESTATUS", typeof(String));

            if (Dt_Conceptos_Sin_Ordenar is DataTable)
            {
                if (Dt_Conceptos_Sin_Ordenar.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO in Dt_Conceptos_Sin_Ordenar.Rows)
                    {
                        if (CONCEPTO is DataRow) {
                            DataRow Dr_Concepto = Dt_Conceptos.NewRow();

                            if (!String.IsNullOrEmpty(CONCEPTO["CLAVE"].ToString()))
                                Dr_Concepto["CLAVE"] = CONCEPTO["CLAVE"].ToString();

                            if (!String.IsNullOrEmpty(CONCEPTO["NOMBRE"].ToString()))
                                Dr_Concepto["NOMBRE"] = CONCEPTO["NOMBRE"].ToString();

                            if (!String.IsNullOrEmpty(CONCEPTO["TIPO"].ToString()))
                                Dr_Concepto["TIPO"] = CONCEPTO["TIPO"].ToString();

                            if (!String.IsNullOrEmpty(CONCEPTO["TIPO_ASIGNACION"].ToString()))
                                Dr_Concepto["TIPO_ASIGNACION"] = CONCEPTO["TIPO_ASIGNACION"].ToString();

                            if (!String.IsNullOrEmpty(CONCEPTO["CONCEPTO"].ToString()))
                                Dr_Concepto["CONCEPTO"] = CONCEPTO["CONCEPTO"].ToString();

                            if (!String.IsNullOrEmpty(CONCEPTO["ESTATUS"].ToString()))
                                Dr_Concepto["ESTATUS"] = CONCEPTO["ESTATUS"].ToString();

                            Dt_Conceptos.Rows.Add(Dr_Concepto);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ordenar la tabla de conceptos. Error: [" + Ex.Message + "]");
        }
        return Dt_Conceptos;
    }
}
