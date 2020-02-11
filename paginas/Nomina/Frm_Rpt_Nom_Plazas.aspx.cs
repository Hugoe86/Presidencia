using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.WebControls;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Dependencias.Negocios;
using Presidencia.Empleados.Negocios;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text;
using Presidencia.Ayudante_CarlosAG;

public partial class paginas_Nomina_Frm_Rpt_Nom_Plazas : System.Web.UI.Page
{
    #region (Init/Load)
    /// ***********************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Carga Inicial de la página.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack) {
                Configuracion_Inicial();
            }

            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #region (Métodos)

    #region (Métodos Generales)
    /// ***********************************************************************************************
    /// NOMBRE: Configuración_Inicial
    /// 
    /// DESCRIPCIÓN: Carga Inicial de la página.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Configuracion_Inicial()
    {
        try
        {
            Consultar_Unidades_Responsables();//Consulta las unidades responsables del sistema.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuración inicial de la página. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Métodos Consulta)
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

            Cmb_Unidades_Responsables.DataSource = Dt_Unidades_Responsables;
            Cmb_Unidades_Responsables.DataTextField = "CLAVE_NOMBRE";
            Cmb_Unidades_Responsables.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Unidades_Responsables.DataBind();

            Cmb_Unidades_Responsables.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables. Error: [" + Ex.Message + "]");
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
            if (Cmb_Unidades_Responsables.SelectedIndex > 0) Obj_Empleados.P_Dependencia_ID = Cmb_Unidades_Responsables.SelectedValue.Trim();
            if (Cmb_Estatus.SelectedIndex > 0) Obj_Empleados.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();

            Dt_Puestos = Obj_Empleados.Consultar_Puestos_Dependencia();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los puestos. Error: [" + Ex.Message + "]");
        }
        return Dt_Puestos;
    }
    #endregion

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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Empleados",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)
    /// ***********************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Click
    /// 
    /// DESCRIPCIÓN: Evento que ejecuta el reporte de puestos
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREÓ:Juan Alberto Hernández Negrete
    /// FECHA CREÓ: 21/Junio/2011 13:53 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// ***********************************************************************************************
    protected void Btn_Generar_Reporte_Click(Object sender, ImageClickEventArgs e) {
        System.Data.DataTable Dt_Puestos = null;
        System.Data.DataSet Ds_Puestos = null;

        try
        {
            Ds_Puestos = new System.Data.DataSet();
            Dt_Puestos = Consultar_Puestos();
            Dt_Puestos.TableName = "Puestos";
            Ds_Puestos.Tables.Add(Dt_Puestos.Copy());

            if (Cmb_Estatus.SelectedIndex > 0)
            {
                if (Cmb_Estatus.SelectedItem.Text == "DISPONIBLE")
                {
                    Generar_Reporte(ref Ds_Puestos, "Cr_Rpt_Nom_Puestos.rpt", "Puestos_" + Session.SessionID + ".pdf");
                }
                else
                {
                    Generar_Reporte(ref Ds_Puestos, "Cr_Rpt_Nom_Puestos_Ocupados.rpt", "Puestos_" + Session.SessionID + ".pdf");
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Se debe seleccionar un estatus para generar el reporte.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion


    protected void Btn_Excel_Click(object sender, ImageClickEventArgs e)
    {
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.
        DataTable Dt_Puestos = Consultar_Puestos();
        Dt_Puestos.Columns.Remove("TOTAL_PLAZAS");
        Dt_Puestos.Columns.Remove("TIPO_PLAZA");
        Dt_Puestos.Columns.Remove("NO_PLAZAS");
        Dt_Puestos.Columns.Remove("CODIGO_PROGRAMATICO");
        Dt_Puestos.Columns.Remove("CLAVE");
        Dt_Puestos.Columns.Remove("EMPLEADO");
        
        Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Puestos);
        //Mandamos a imprimir el reporte en excel.
        Mostrar_Excel(Libro, "");
    }


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
}
