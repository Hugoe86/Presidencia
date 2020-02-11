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
using Presidencia.Reapertura_Turnos.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Frm_Ope_Pre_Reapertura_Turnos : System.Web.UI.Page
{
    #region (Load/Init)
    ///************************************************************************************************
    /// NOMBRE: Page_Load
    ///
    /// DESCRIPCIÓN: Habilita la configuración inical de la página.
    /// 
    /// PARÁMETROS: No Áplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Inicial();//Habilita la configuración inical de la página.
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la configuración inicial de la página [Page_Load]. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Métodos)

    #region (Generales)
    ///************************************************************************************************
    /// NOMBRE: Configuracion_Inicial
    ///
    /// DESCRIPCIÓN: Habilita la configuración inical de la página.
    /// 
    /// PARÁMETROS: No Áplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Configuracion_Inicial() {
        Btn_Abrir_Cierre_Dia.Enabled = false;
        Txt_Autorizo_Reapertura.Enabled = false;
        Txt_Observaciones.Enabled = false;
        Limpiar_Controles();//Limpia los controles de la página.
    }
    ///************************************************************************************************
    /// NOMBRE: Limpiar_Controles
    ///
    /// DESCRIPCIÓN: Limpia los controles de la página.
    /// 
    /// PARÁMETROS: No Áplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Limpiar_Controles() {
        //Limpia el grid de  cierre de día.
        Grid_Cierres_Turno_Dia.DataSource = new DataTable();
        Grid_Cierres_Turno_Dia.DataBind();
        Grid_Cierres_Turno_Dia.SelectedIndex = -1;
        //Limpia las cajas de texto que almacenan los rangos de fecha de busqueda.
        Txt_Fecha_Inicio.Text = String.Empty;
        Txt_Fecha_Fin.Text = String.Empty;
        Txt_Autorizo_Reapertura.Text = "";
        Txt_Observaciones.Text = "";
    }
    #endregion

    #region (Consulta)
    ///************************************************************************************************
    /// NOMBRE: Consultar_Cierres_Dia
    ///
    /// DESCRIPCIÓN: Consulta los cierres de día.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 22/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected DataTable Consultar_Cierres_Dia()
    {
        Cls_Ope_Pre_Reapertura_Turnos_Negocio Obj_Reapertura_Dia_Negocio = new Cls_Ope_Pre_Reapertura_Turnos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Reapertura_Dias = null;//Variable que almacenara el listado de cierres de día.

        try
        {
            Txt_Fecha_Inicio.Text = Txt_Fecha_Inicio.Text.Replace("__/___/____", String.Empty);
            Txt_Fecha_Fin.Text = Txt_Fecha_Fin.Text.Replace("__/___/____", String.Empty);

            //Establecemos los filtros de busqueda.
            if (!String.IsNullOrEmpty(Txt_Fecha_Inicio.Text))
                Obj_Reapertura_Dia_Negocio.P_Fecha_Inicio_Busqueda_Cierre_Dias = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio.Text.Trim()));

            if (!String.IsNullOrEmpty(Txt_Fecha_Fin.Text))
                Obj_Reapertura_Dia_Negocio.P_Fecha_Fin_Busqueda_Cierre_Dias = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Fin.Text.Trim()));

            //Ejecutamos la consulta de los cierres de día.
            Dt_Reapertura_Dias = Obj_Reapertura_Dia_Negocio.Consultar_Cierres_Dia();
            LLenar_Grid_Cierres_Dia(Dt_Reapertura_Dias, 0);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los dias de cierre. Error: [" + Ex.Message + "]");
        }
        return Dt_Reapertura_Dias;
    }
    #endregion

    #region (Reportes)
    ///************************************************************************************************
    /// NOMBRE: Launch_Reporte
    ///
    /// DESCRIPCIÓN: Lanza el reporte de reapertura de cierre de día.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Launch_Reporte(String No_Cierre_Dia)
    {
        Cls_Ope_Pre_Reapertura_Turnos_Negocio Obj_Reapertura_Cierre_Dia_Negocios = new Cls_Ope_Pre_Reapertura_Turnos_Negocio();//Variable de coenxion con la capa de negocios.
        DataSet Ds_Reapertura_Cierre_Dia = new DataSet();
        DataTable Dt_Reapertura_Cierre_Dia = null;

        try
        {
            Obj_Reapertura_Cierre_Dia_Negocios.P_No_Turno_Dia = No_Cierre_Dia;
            Dt_Reapertura_Cierre_Dia = Obj_Reapertura_Cierre_Dia_Negocios.Rpt_Reapertura_Cierre_Dia();

            if (Dt_Reapertura_Cierre_Dia is DataTable) {
                if (Dt_Reapertura_Cierre_Dia.Rows.Count > 0)
                {
                    Dt_Reapertura_Cierre_Dia.TableName = "Reapertura_Cierre_Dia";
                    Ds_Reapertura_Cierre_Dia.Tables.Add(Dt_Reapertura_Cierre_Dia.Copy());

                    //Se llama al método que ejecuta la operación de generar el reporte.
                    Generar_Reporte(ref Ds_Reapertura_Cierre_Dia, "Cr_Pre_Reapertura_Cierre_Dia.rpt", "Reporte_Reapertura_Cierre_Dia" + Session.SessionID + ".pdf");
                }
                else { ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Reapertura Turno", "alert('No se encontraron resultados para la búsqueda realizada.');", true); }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al lanzar el reporte de reapertura de cierre de día. Error: [" + Ex.Message + "]");
        }
    }
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

    #region (Grids)
    ///************************************************************************************************
    /// NOMBRE: LLenar_Grid_Cierres_Dia
    ///
    /// DESCRIPCIÓN: Se carga el grid de cierres de día.
    /// 
    /// PARÁMETROS: Dt_Cierres_Dia.- Listado de los cierres de día a cargar en la tabla.
    ///             No_Pagina.- No de página a seleccionar del grid de cierre de día.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 22/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void LLenar_Grid_Cierres_Dia(DataTable Dt_Cierres_Dia, Int32 No_Pagina)
    {
        try
        {
            if (Dt_Cierres_Dia is DataTable)
            {
                if (Dt_Cierres_Dia.Rows.Count > 0)
                {
                    //Cargamos el grid de  cierre de dias.
                    Grid_Cierres_Turno_Dia.DataSource = Dt_Cierres_Dia;
                    Grid_Cierres_Turno_Dia.DataBind();
                    Grid_Cierres_Turno_Dia.PageIndex = No_Pagina;
                    Grid_Cierres_Turno_Dia.SelectedIndex = -1;
                }
                else { }
            }
        }
        catch (Exception Ex)
        {

            throw new Exception("Error al cargar el grid de cierre de día. Error: [" + Ex.Message + "]");
        }
    }
    ///************************************************************************************************
    /// NOMBRE: Grid_Cierres_Dia_RowDataBound
    ///
    /// DESCRIPCIÓN: Formatear la fecha de cierre del dia.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 22/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Grid_Cierres_Turno_Dia_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                //Formateamos la fecha del cierre del día.
                if (!String.IsNullOrEmpty(e.Row.Cells[2].Text.Trim())) {
                    e.Row.Cells[2].Text = String.Format("{0:dd MMMM yyyy}", Convert.ToDateTime(e.Row.Cells[2].Text.Trim()));
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al alterar la informacion del grid de cierre de dia antes del renderiazdo de la pagina al usuario final. Error: [" + Ex.Message + "]");
        }
    }
    ///************************************************************************************************
    /// NOMBRE: Grid_Cierres_Dia_PageIndexChanging
    ///
    /// DESCRIPCIÓN: Cambia la pagina del Grid_Cierres_Dia.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Grid_Cierres_Turno_Dia_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Consultar_Cierres_Dia();
            Grid_Cierres_Turno_Dia.PageIndex = e.NewPageIndex;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cambiar la pagina del Grid_Cierres_Dia. Error: [" + Ex.Message + "]");
        }
    }
    ///************************************************************************************************
    /// NOMBRE: Grid_Cierres_Dia_SelectedIndexChanged
    ///
    /// DESCRIPCIÓN: Método que se ejecuta al seleccionar un elemento del Grid_Cierres_Dia.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Grid_Cierres_Turno_Dia_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Estatus_Cierre_Dia = String.Empty;//Variable que almacenara el estatus del cierre de dia seleccionado.

        try
        {
            if (Grid_Cierres_Turno_Dia.SelectedIndex != (-1)) {
                if (Grid_Cierres_Turno_Dia.SelectedIndex >= 0)
                {
                    //Obtenemos el estatus del cierre del dia del registro seleccionado.
                    Estatus_Cierre_Dia = Grid_Cierres_Turno_Dia.Rows[Grid_Cierres_Turno_Dia.SelectedIndex].Cells[5].Text.Trim();

                    if (!String.IsNullOrEmpty(Estatus_Cierre_Dia)) {
                        if (Estatus_Cierre_Dia.Trim().ToUpper().Equals("CERRADO"))
                        {
                            Btn_Abrir_Cierre_Dia.Enabled = true;
                            Txt_Autorizo_Reapertura.Enabled = true;
                            Txt_Observaciones.Enabled = true;
                        }
                        else {
                            Btn_Abrir_Cierre_Dia.Enabled = false;
                            Txt_Autorizo_Reapertura.Enabled = false;
                            Txt_Observaciones.Enabled = false;
                        }
                    }            
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar un elemento del Grid_Cierres_Dia. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos)
    ///************************************************************************************************
    /// NOMBRE: Btn_Busqueda_Cierre_Dias_Click
    ///
    /// DESCRIPCIÓN: Evento que ejecuta la búsqueda de cierres de día en el sistema.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 22/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Btn_Busqueda_Cierre_Dias_Click(Object sender, EventArgs e)
    {
        try
        {
            //Se ejecuta la consulta de cierres de día.
            Consultar_Cierres_Dia();
            Btn_Abrir_Cierre_Dia.Enabled = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar el evento de busqueda de cierre de dias [Btn_Busqueda_Cierre_Dias_Click]. Error: [" + Ex.Message + "]");
        }
    }
    ///************************************************************************************************
    /// NOMBRE: Btn_Abrir_Cierre_Dia_Click
    ///
    /// DESCRIPCIÓN: Evento que ejecuta la reapertura del turno.
    /// 
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete. 
    /// FECHA CREO: 23/Octubre/2011
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    ///************************************************************************************************
    protected void Btn_Abrir_Cierre_Dia_Click(Object sender, EventArgs e)
    {
        Cls_Ope_Pre_Reapertura_Turnos_Negocio Obj_Reapertura_Dia = new Cls_Ope_Pre_Reapertura_Turnos_Negocio();//Variable de conexión con la capa de negocios.
        String No_Cierre_Dia = String.Empty;//No de cierre de dia a reabrir.
        Boolean Operacion_Completa = false;//Variable que guarda el estatus de la operación.

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Grid_Cierres_Turno_Dia.SelectedIndex != (-1))
            {
                if (Grid_Cierres_Turno_Dia.SelectedIndex >= 0)
                {
                    if (Validar_Datos())
                    {
                        No_Cierre_Dia = Grid_Cierres_Turno_Dia.Rows[Grid_Cierres_Turno_Dia.SelectedIndex].Cells[1].Text.Trim();

                        Obj_Reapertura_Dia.P_No_Turno_Dia = No_Cierre_Dia;
                        Obj_Reapertura_Dia.P_Empleado_Reabrio = Cls_Sessiones.Empleado_ID;
                        Obj_Reapertura_Dia.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                        Obj_Reapertura_Dia.P_Autorizo_Reapertura = Txt_Autorizo_Reapertura.Text.Trim().ToUpper();
                        Obj_Reapertura_Dia.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
                        //Se ejecuta la apertura del cierre de día.
                        Operacion_Completa = Obj_Reapertura_Dia.Actualiza_Estatus_Cierre_Dia();

                        if (Operacion_Completa)
                        {
                            //Lanza el reporte de la reapertura del cierre de dia.
                            Launch_Reporte(No_Cierre_Dia);
                            //Vuelve los controles ala configuración inicial de la página.
                            Configuracion_Inicial();
                            //Mostramos un mensaje de operación completa.
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Reapertura Turno", "alert('Reapertura realizada');", true);
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la reapertura del cierre del día. [Btn_Abrir_Cierre_Dia_Click]. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Ismael Prieto Sánchez
    /// FECHA_CREO  : 18-Noviembre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Autorizo_Reapertura.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El nombre de quien autoriza la reapertura. <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Observaciones.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Las observaciones de autorización de reapertura. <br>";
            Datos_Validos = false;
        }
        
        return Datos_Validos;
    }
    #endregion
}
