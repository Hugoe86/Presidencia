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
using Presidencia.Reporte_Pago_Cajas_Diario.Negocio;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Sessiones;
using Presidencia.Constantes;

public partial class paginas_Ingresos_Rpt_Caj_Pagos : System.Web.UI.Page
{
    #region (Load/Init)
    /// *****************************************************************************************************
    /// Nombre: Page_Load
    /// 
    /// Descripción: Carga la configuración inicial de la página.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernandez Negrete.
    /// Fecha Creó: 24/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Redireccionamos la página sin no existe algún usuario logueado.
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack) {
                IBtn_PDF.Enabled = false;
            }

            Lbl_Mensaje_Error.Text = String.Empty;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Generar_Reporte();
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

    #region (Generales)
    /// *****************************************************************************************************
    /// Nombre: Crear_Tabla_Rpt_Ingresos_Diarios_Presidencia
    /// 
    /// Descripción: Crea la tabla de ingresos.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernandez Negrete.
    /// Fecha Creó: 24/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected DataTable Crear_Tabla_Rpt_Ingresos_Diarios_Presidencia()
    {
        Cls_Rpt_Caj_Pagos_Negocio Obj_Rpt_Caj_Diario = new Cls_Rpt_Caj_Pagos_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_RAMAS = null;
        DataTable Dt_GRUPOS = null;
        DataTable Dt_INGRESOS = null;
        Double Contador_Grupos_Rama = 0;
        Double Contador_Ingresos_Grupo = 0;
        String Clave_Rama = String.Empty;
        String Clave_Grupo = String.Empty;
        String Clave_Ingreso = String.Empty;
        Double Monto_Rama = 0.0;
        Double Monto_Grupo = 0.0;
        Double Monto_Ingreso = 0.0;
        DataTable Dt_Tabla_Montos_Ingresos_Diarios = new DataTable();
        DataRow Dr_Rama = null;
        List<DataRow> Dr_Ingresos = new List<DataRow>();
        List<DataRow> Dr_Grupos = new List<DataRow>();
        Dictionary<String, List<DataRow>> Listado_Ingresos_Grupo = new Dictionary<string, List<DataRow>>();
        String Fecha_Inicio = String.Empty;//Variable que almacena la fecha apartir de donde se comenzaraa  consultar.
        String Fecha_Fin = String.Empty;//Variable que almacena la fecha final hasta donde se consultara.


        try
        {
            Dt_Tabla_Montos_Ingresos_Diarios.Columns.Add("CLAVE", typeof(String));
            Dt_Tabla_Montos_Ingresos_Diarios.Columns.Add("NOMBRE", typeof(String));
            Dt_Tabla_Montos_Ingresos_Diarios.Columns.Add("CANTIDAD", typeof(String));
            Dt_Tabla_Montos_Ingresos_Diarios.Columns.Add("MONTO", typeof(Double));

            Obj_Rpt_Caj_Diario.P_Fecha_Inicio_Busqueda = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text.Trim()));
            Obj_Rpt_Caj_Diario.P_Fecha_Fin_Busqueda = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text.Trim()));

            Dt_RAMAS = Obj_Rpt_Caj_Diario.Consultar_Rama();

            if (Dt_RAMAS is DataTable)
            {
                if (Dt_RAMAS.Rows.Count > 0)
                {
                    foreach (DataRow RAMA in Dt_RAMAS.Rows)
                    {
                        if (RAMA is DataRow)
                        {
                            if (!String.IsNullOrEmpty(RAMA["RAMA"].ToString().Trim()))
                            {

                                if (!String.IsNullOrEmpty(RAMA["MONTO"].ToString().Trim()))
                                {
                                    Monto_Rama = Convert.ToDouble(RAMA["MONTO"].ToString().Trim());
                                }

                                Clave_Rama = RAMA["RAMA"].ToString().Trim();

                                Obj_Rpt_Caj_Diario.P_Clave_Rama = Clave_Rama;
                                Dt_GRUPOS = Obj_Rpt_Caj_Diario.Consultar_Grupos();

                                if (Dt_GRUPOS is DataTable)
                                {
                                    if (Dt_GRUPOS.Rows.Count > 0)
                                    {
                                        foreach (DataRow GRUPO in Dt_GRUPOS.Rows)
                                        {
                                            if (GRUPO is DataRow)
                                            {
                                                if (GRUPO is DataRow)
                                                {
                                                    if (!String.IsNullOrEmpty(GRUPO["GRUPO"].ToString().Trim()))
                                                    {
                                                        if (!String.IsNullOrEmpty(GRUPO["MONTO"].ToString().Trim()))
                                                        {
                                                            Monto_Grupo = Convert.ToDouble(GRUPO["MONTO"].ToString().Trim());
                                                        }

                                                        Clave_Grupo = GRUPO["GRUPO"].ToString().Trim();

                                                        Obj_Rpt_Caj_Diario.P_Clave_Rama = Clave_Rama;
                                                        Obj_Rpt_Caj_Diario.P_Clave_Grupo = Clave_Grupo;
                                                        Dt_INGRESOS = Obj_Rpt_Caj_Diario.Consultar_Ingresos();

                                                        if (Dt_INGRESOS is DataTable)
                                                        {
                                                            if (Dt_INGRESOS.Rows.Count > 0)
                                                            {
                                                                foreach (DataRow INGRESO in Dt_INGRESOS.Rows)
                                                                {
                                                                    if (INGRESO is DataRow)
                                                                    {
                                                                        if (!String.IsNullOrEmpty(INGRESO["INGRESO"].ToString().Trim()))
                                                                        {

                                                                            if (!String.IsNullOrEmpty(INGRESO["MONTO"].ToString().Trim()))
                                                                            {
                                                                                Monto_Ingreso = Convert.ToDouble(INGRESO["MONTO"].ToString().Trim());
                                                                            }

                                                                            Clave_Ingreso = INGRESO["INGRESO"].ToString().Trim();

                                                                            DataRow Dr_Ingreso = Dt_Tabla_Montos_Ingresos_Diarios.NewRow();
                                                                            Dr_Ingreso["CLAVE"] = Clave_Ingreso;
                                                                            Dr_Ingreso["NOMBRE"] = INGRESO["NOMBRE"].ToString().Trim();
                                                                            Dr_Ingreso["CANTIDAD"] = 1;
                                                                            Dr_Ingreso["MONTO"] = Monto_Ingreso;
                                                                            Dr_Ingresos.Add(Dr_Ingreso);

                                                                            Contador_Ingresos_Grupo++;
                                                                        }
                                                                    }
                                                                }
                                                            }
                                                        }

                                                        DataRow Dr_Grupo = Dt_Tabla_Montos_Ingresos_Diarios.NewRow();
                                                        Dr_Grupo["CLAVE"] = Clave_Grupo;
                                                        Dr_Grupo["NOMBRE"] = GRUPO["NOMBRE"].ToString().Trim();
                                                        Dr_Grupo["CANTIDAD"] = Contador_Ingresos_Grupo;
                                                        Dr_Grupo["MONTO"] = Monto_Grupo;
                                                        Dr_Grupos.Add(Dr_Grupo);

                                                        Listado_Ingresos_Grupo.Add(Clave_Grupo, Dr_Ingresos);
                                                        Dr_Ingresos = new List<DataRow>();

                                                        Contador_Grupos_Rama += Contador_Ingresos_Grupo;
                                                        Contador_Ingresos_Grupo = 0;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }

                                Dr_Rama = Dt_Tabla_Montos_Ingresos_Diarios.NewRow();
                                Dr_Rama["CLAVE"] = Clave_Rama;
                                Dr_Rama["NOMBRE"] = RAMA["NOMBRE"].ToString().Trim();
                                Dr_Rama["CANTIDAD"] = Contador_Grupos_Rama;
                                Dr_Rama["MONTO"] = Monto_Rama;
                                Dt_Tabla_Montos_Ingresos_Diarios.Rows.Add(Dr_Rama);

                                foreach (DataRow _GRUPO in Dr_Grupos)
                                {
                                    Dt_Tabla_Montos_Ingresos_Diarios.Rows.Add(_GRUPO);

                                    List<DataRow> Lst_Temp = (List<DataRow>)Listado_Ingresos_Grupo[_GRUPO["CLAVE"].ToString().Trim()];

                                    foreach (DataRow _INGRESO in Lst_Temp)
                                    {
                                        DataRow[] Dr_Temp = Dt_Tabla_Montos_Ingresos_Diarios.Select("CLAVE='" + _INGRESO["CLAVE"].ToString() + "'");

                                        if (Dr_Temp.Length == 0)
                                        {
                                            Dt_Tabla_Montos_Ingresos_Diarios.Rows.Add(_INGRESO);
                                        }
                                        else
                                        {
                                            Dr_Temp[0]["CANTIDAD"] = (Convert.ToDouble(Dr_Temp[0]["CANTIDAD"].ToString().Trim()) + 1).ToString();
                                        }
                                    }
                                }

                                Dr_Grupos = new List<DataRow>();

                                Contador_Grupos_Rama = 0;
                            }
                        }

                        Contador_Grupos_Rama = 0;
                        Contador_Ingresos_Grupo = 0;

                        Clave_Rama = String.Empty;
                        Clave_Grupo = String.Empty;
                        Clave_Ingreso = String.Empty;

                        Monto_Rama = 0.0;
                        Monto_Grupo = 0.0;
                        Monto_Ingreso = 0.0;

                        Listado_Ingresos_Grupo = new Dictionary<string, List<DataRow>>();
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la tabla de ingresos diarios de presidencia. Error: [" + Ex.Message + "]");
        }
        return Dt_Tabla_Montos_Ingresos_Diarios;
    }
    #endregion

    #region (Validación)
    protected Boolean Validar_Datos()
    {
        Boolean Estatus = true;
        Lbl_Mensaje_Error.Text = "Introducir: <br />";

        try
        {
            if (String.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text.Trim())) {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario ingresar la fecha de inicio de la búsqueda. <br />";
                Estatus = false;
            }
            else if (Txt_Busqueda_Fecha_Inicio.Text.Trim().Contains("__/___/____"))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario ingresar la fecha de inicio de la búsqueda. <br />";
                Estatus = false;
            }

            if (String.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario ingresar la fecha de fin de la búsqueda. <br />";
                Estatus = false;
            }
            else if (Txt_Busqueda_Fecha_Fin.Text.Trim().Contains("__/___/____"))
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Es necesario ingresar la fecha de fin de la búsqueda. <br />";
                Estatus = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los filtros para mostrar el reporte. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #endregion

    #region (Grids)
    /// *****************************************************************************************************
    /// Nombre: LLenar_Tabla_Ingresos
    /// 
    /// Descripción: LLena la tabla de ingresos con la tabla creada por RAMA, GRUPO y INGRESO.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernandez Negrete.
    /// Fecha Creó: 24/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected void LLenar_Tabla_Ingresos()
    {
        DataTable Dt_Ingresos = null;

        try
        {
            Dt_Ingresos = Crear_Tabla_Rpt_Ingresos_Diarios_Presidencia();

            Grid_Ingresos_Diarios.DataSource = Dt_Ingresos;
            Grid_Ingresos_Diarios.DataBind();
            Grid_Ingresos_Diarios.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar la tabla de ingresos. Error: [" + Ex.Message + "]");
        }
    }
    /// *****************************************************************************************************
    /// Nombre: Grid_Ingresos_Diarios_PageIndexChanging
    /// 
    /// Descripción: Cambia la pagina del grid de ingresos.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernandez Negrete.
    /// Fecha Creó: 24/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected void Grid_Ingresos_Diarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Ingresos_Diarios.PageIndex = e.NewPageIndex;
            LLenar_Tabla_Ingresos();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cambiar la pagina del grid de ingresos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Eventos)

    #region (Botones)
    /// *****************************************************************************************************
    /// Nombre: Btn_Generar_Reporte_Click
    /// 
    /// Descripción: Muestra los ingresos cobrados según los filtros seleccionados.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernandez Negrete.
    /// Fecha Creó: 24/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validar_Datos()) {
                LLenar_Tabla_Ingresos();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }

            if (Grid_Ingresos_Diarios.Rows.Count > 0) IBtn_PDF.Enabled = true; else IBtn_PDF.Enabled = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// *****************************************************************************************************
    /// Nombre: IBtn_PDF_Click
    /// 
    /// Descripción: Genera el reporte de ingresos segun el filtro seleccionado.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernandez Negrete.
    /// Fecha Creó: 24/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *****************************************************************************************************
    protected void IBtn_PDF_Click(object sender, EventArgs e)
    {
        DataSet Ds_Ingresos = null;
        DataTable Dt_Ingresos_Diarios = null;

        try
        {
            if (Validar_Datos())
            {
                Dt_Ingresos_Diarios = Crear_Tabla_Rpt_Ingresos_Diarios_Presidencia();

                if (Dt_Ingresos_Diarios is DataTable)
                {
                    if (Dt_Ingresos_Diarios.Rows.Count > 0)
                    {
                        Dt_Ingresos_Diarios.TableName = "Ingresos_Diarios";
                        Ds_Ingresos = new DataSet();
                        Ds_Ingresos.Tables.Add(Dt_Ingresos_Diarios.Copy());

                        Generar_Reporte(ref Ds_Ingresos, "Cr_Rpt_Ing_Ingresos_Diarios.rpt", "Rpt_Ing_Ingresos_Diarios_" + Session.SessionID + ".pdf");
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "La búsqueda no tuvo ningun reultado. ¡Vuelva a Intentar!";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            else {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de Ingresos Diarios. Error: [" + Ex.Message + "]");
        }
    }
    #endregion   

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
            Ruta = @Server.MapPath("../Rpt/Ingresos/" + Nombre_Plantilla_Reporte);
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


    /// *********************************************************************************************************************
    /// Nombre: Generar_Reporte
    /// 
    /// Descripción: Metodo que genera el reporte de cierre de caja.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 25/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// *********************************************************************************************************************
    protected void Generar_Reporte()
    {
        Cls_Rpt_Caj_Pagos_Negocio Obj_Pagos = new Cls_Rpt_Caj_Pagos_Negocio();//Variable de conexión con la capa de negocios.
        DataSet Ds_Reporte = null;//Variable que almacena las tabla de ingresos.
        
        try
        {
            Ds_Reporte = Obj_Pagos.Rpt_Caj_Ingresos("", "0000000001", "00001");//Consulta de las tablas de ingresos.
            Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Ing_Corte_Caja.rpt", "Rpt_Ingresos_" + Session.SessionID + ".pdf");//Generamos el reporte de ingresos.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }

}
