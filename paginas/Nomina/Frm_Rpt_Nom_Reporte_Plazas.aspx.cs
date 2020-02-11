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
using Presidencia.Dependencias.Negocios;
using System.Text;
using Presidencia.Reporte_Plazas.Negocio;
using Presidencia.Cat_Parametros_Nomina.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Reporte_Plazas : System.Web.UI.Page
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
                Inicializa_Controles();
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
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Salvador L. Rea Ayala
    /// FECHA_CREO  : 13/Octubre/2011
    /// MODIFICO          : 
    /// FECHA_MODIFICO    : 
    /// CAUSA_MODIFICACION: 
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Cls_Cat_Dependencias_Negocio Dependencia_Negocio = new Cls_Cat_Dependencias_Negocio();
            Cls_Rpt_Nom_Plazas_Negocio Rs_Consulta_Puestos = new Cls_Rpt_Nom_Plazas_Negocio();
            Cls_Rpt_Nom_Plazas_Negocio Rs_Consulta_Nomina = new Cls_Rpt_Nom_Plazas_Negocio();
            DataTable Dt_Consulta_2 = new DataTable();
            DataTable Dt_Dependencias = Dependencia_Negocio.Consulta_Dependencias();
            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Unidad_Responsable_Busqueda, Dt_Dependencias, 1, 0);
            Cmb_Unidad_Responsable_Busqueda.SelectedValue = Cls_Sessiones.Dependencia_ID_Empleado;
            DataTable Dt_Consulta = new DataTable();
                if (Cmb_Unidad_Responsable_Busqueda.SelectedIndex > 0)
                {
                    Dt_Consulta = Rs_Consulta_Nomina.Consultar_Tipo_Nomina();
                    Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Tipo_Nomina, Dt_Consulta, 1, 0);
                    Rs_Consulta_Puestos.P_Dependencia_ID = Cmb_Unidad_Responsable_Busqueda.SelectedValue;
                    Dt_Consulta_2 = Rs_Consulta_Puestos.Consultar_Puestos_Depenencia();
                    Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Puesto, Dt_Consulta, 1, 0);
                    Cmb_Tipo_Nomina.SelectedIndex = 0;
                }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
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
            Cmb_Unidad_Responsable_Busqueda.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString(), ex);
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
    ///*********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cmb_Unidad_Responsable_Busqueda_SelectedIndexChanged
    ///DESCRIPCIÓN          : Evento del combo de unidad responsable
    ///PROPIEDADES          :
    ///CREO                 : Leslie González Vázquez
    ///FECHA_CREO           : 22/Diciembre/2011 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Cmb_Unidad_Responsable_Busqueda_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Rpt_Nom_Plazas_Negocio Rs_Consulta_Puestos = new Cls_Rpt_Nom_Plazas_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            if (Cmb_Unidad_Responsable_Busqueda.SelectedIndex > 0)
            {
                Rs_Consulta_Puestos.P_Dependencia_ID = Cmb_Unidad_Responsable_Busqueda.SelectedValue;
                Dt_Consulta = Rs_Consulta_Puestos.Consultar_Puestos_Depenencia();
                Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Puesto, Dt_Consulta, 1, 0);
                Cmb_Tipo_Nomina.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al generar el evento del combo de unidad responsable Error[" + ex.Message + "]");
        }
    }
    /////*********************************************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Cmb_Tipo_Nomina_SelectedIndexChanged
    /////DESCRIPCIÓN          : Evento del combo de Tipo Nomina
    /////PROPIEDADES          :
    /////CREO                 : Sergio Manuel Gallardo Andrade
    /////FECHA_CREO           : 11/Abril/2012 
    /////MODIFICO             :
    /////FECHA_MODIFICO       :
    /////CAUSA_MODIFICACIÓN...:
    /////*********************************************************************************************************
    //protected void Cmb_Tipo_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    //{
    //    Cls_Rpt_Nom_Plazas_Negocio Rs_Consulta_Nomina = new Cls_Rpt_Nom_Plazas_Negocio();
    //    DataTable Dt_Consulta = new DataTable();
    //    try
    //    {
    //        if (Cmb_Unidad_Responsable_Busqueda.SelectedIndex > 0)
    //        {
    //            Dt_Consulta = Rs_Consulta_Nomina.Consultar_Tipo_Nomina();
    //            Cls_Util.Llenar_Combo_Con_DataTable_Generico(Cmb_Tipo_Nomina, Dt_Consulta, 1, 0);
    //        }
    //    }
    //    catch (Exception ex)
    //    {
    //        throw new Exception("Error al generar el evento del combo de unidad responsable Error[" + ex.Message + "]");
    //    }
    //}
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
        //String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        //Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        //Lbl_Mensaje_Error.Text = "";
        //Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";

        //if (Cmb_Empleado.SelectedIndex == 0)
        //{
        //    Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecione algun empleado.<br>";
        //    Datos_Validos = false;
        //}
        
       return Datos_Validos;
    }
    #endregion
    #region (Consulta)
    //*******************************************************************************
    // NOMBRE DE LA FUNCION: Consulta_Plazas
    // DESCRIPCION : Consulta todos las cuentas de credito fonacot por empleado
    // PARAMETROS  : 
    // CREO        : Sergio Manuel Gallardo Andrade
    // FECHA_CREO  : 11-abril-2012
    // MODIFICO          :
    // FECHA_MODIFICO    :
    // CAUSA_MODIFICACION:
    //*******************************************************************************
    private void Consulta_Plazas()
    {
        Cls_Rpt_Nom_Plazas_Negocio Rs_Consulta = new Cls_Rpt_Nom_Plazas_Negocio(); //Conexion hacia la capa de negocios
        Cls_Cat_Nom_Parametros_Negocio Rs_Parametros = new Cls_Cat_Nom_Parametros_Negocio();
        DataTable Dt_Parametro = new DataTable();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Final = new DataTable();
        DataTable Dt_Temporal = new DataTable();
        Ds_Rpt_Nom_Reporte_Plazas Ds_Reporte = new Ds_Rpt_Nom_Reporte_Plazas();
        ReportDocument Reporte = new ReportDocument();
        String Espacios_Blanco;
        Double Ocupados = 0;
        Double Disponible = 0;
        Double Porcentaje_PSM = 0;
        Boolean insertar = true;
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte Plazas" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al document
        try
        {
            if (Cmb_Unidad_Responsable_Busqueda.SelectedIndex > 0)
            {
                Rs_Consulta.P_Dependencia_ID = Cmb_Unidad_Responsable_Busqueda.SelectedValue;
            }
            if(Cmb_Puesto.SelectedIndex >0){
                Rs_Consulta.P_Puesto_ID = Cmb_Puesto.SelectedValue;
            }
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
            {
                Rs_Consulta.P_Tipo_Nomina = Cmb_Tipo_Nomina.SelectedValue;
            }
            Rs_Consulta.P_Usuario_Creo = Cls_Sessiones.Empleado_ID.Trim();
            Dt_Consulta = Rs_Consulta.Consultar_Plazas();
            Dt_Parametro = Rs_Parametros.Consulta_Parametros();
            if (Dt_Parametro.Rows.Count > 0)
            {
                Porcentaje_PSM = Convert.ToDouble(Dt_Parametro.Rows[0]["PORCENTAJE_PSM"].ToString());

                if (Dt_Consulta.Rows.Count > 0)
                {
                    Dt_Consulta.DefaultView.RowFilter = "Estatus='OCUPADO'";
                    if (Dt_Consulta.DefaultView.ToTable().Rows.Count > 0)
                    {
                        Ocupados = Dt_Consulta.DefaultView.ToTable().Rows.Count;
                    }
                    Dt_Consulta.DefaultView.RowFilter = "Estatus='DISPONIBLE'";
                    if (Dt_Consulta.DefaultView.ToTable().Rows.Count > 0)
                    {
                        Disponible = Dt_Consulta.DefaultView.ToTable().Rows.Count;
                    }
                    if (Dt_Temporal.Rows.Count <= 0)
                    {
                        //Agrega los campos que va a contener el DataTable
                        Dt_Temporal.Columns.Add("DIRECCION_GENERAL", typeof(System.String));
                        Dt_Temporal.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
                        Dt_Temporal.Columns.Add("PLAZAS_PRESUPUESTADAS", typeof(System.Double));
                        Dt_Temporal.Columns.Add("PLAZAS_OCUPADAS", typeof(System.Double));
                        Dt_Temporal.Columns.Add("PLAZAS_DISPONIBLES", typeof(System.Double));
                        Dt_Temporal.Columns.Add("NIVEL", typeof(System.String));
                        Dt_Temporal.Columns.Add("SUELDO_MENSUAL", typeof(System.Double));
                        Dt_Temporal.Columns.Add("PSM", typeof(System.Double));
                        Dt_Temporal.Columns.Add("OTROS", typeof(System.String));
                        Dt_Temporal.Columns.Add("TIPO_PLAZA", typeof(System.String));
                        //Agrega los campos que va a contener el DataTable
                        Dt_Final.Columns.Add("DIRECCION_GENERAL", typeof(System.String));
                        Dt_Final.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
                        Dt_Final.Columns.Add("PLAZAS_PRESUPUESTADAS", typeof(System.Double));
                        Dt_Final.Columns.Add("PLAZAS_OCUPADAS", typeof(System.Double));
                        Dt_Final.Columns.Add("PLAZAS_DISPONIBLES", typeof(System.Double));
                        Dt_Final.Columns.Add("NIVEL", typeof(System.String));
                        Dt_Final.Columns.Add("SUELDO_MENSUAL", typeof(System.Double));
                        Dt_Final.Columns.Add("PSM", typeof(System.Double));
                        Dt_Final.Columns.Add("OTROS", typeof(System.String));
                        Dt_Final.Columns.Add("TIPO_PLAZA", typeof(System.String));
                    }
                    
                    foreach (DataRow Fila in Dt_Consulta.Rows){
                        Double ocupadas = 0;
                        Double disponibles = 0;
                        DataRow row = Dt_Temporal.NewRow();
                        row["DIRECCION_GENERAL"] = Fila["DIRECCION_GENERAL"].ToString();
                        row["CODIGO_PROGRAMATICO"] = Fila["CODIGO_PROGRAMATICO"].ToString();
                        row["NIVEL"] = Fila["NIVEL"].ToString();
                        foreach (DataRow Registro in Dt_Consulta.Rows)
                        {
                            if (row["NIVEL"].ToString() == Registro["NIVEL"].ToString() && Registro["ESTATUS"].ToString() == "OCUPADO" && row["CODIGO_PROGRAMATICO"] == Registro["CODIGO_PROGRAMATICO"].ToString() && row["DIRECCION_GENERAL"] == Registro["DIRECCION_GENERAL"].ToString())
                            {
                                ocupadas = ocupadas + 1;
                            }
                            if (row["NIVEL"].ToString() == Registro["NIVEL"].ToString() && Registro["ESTATUS"].ToString() == "DISPONIBLE" && row["CODIGO_PROGRAMATICO"] == Registro["CODIGO_PROGRAMATICO"].ToString() && row["DIRECCION_GENERAL"] == Registro["DIRECCION_GENERAL"].ToString())
                            {
                                disponibles = disponibles + 1;
                            }
                        }
                        row["PLAZAS_PRESUPUESTADAS"] = ocupadas + disponibles;
                        row["PLAZAS_OCUPADAS"] = ocupadas;
                        row["PLAZAS_DISPONIBLES"]=disponibles;
                        row["SUELDO_MENSUAL"] = Convert.ToDouble(Fila["SUELDO_MENSUAL"].ToString()); //*30.42;
                        row["PSM"] = (Convert.ToDouble(row["SUELDO_MENSUAL"].ToString()) * Porcentaje_PSM) / 100;
                        row["OTROS"] = Fila["USUARIO_CREO"].ToString();
                        row["TIPO_PLAZA"] = Fila["TIPO_PLAZA"].ToString();
                        Dt_Temporal.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                        Dt_Temporal.AcceptChanges();
                    }
                    foreach (DataRow Fila in Dt_Temporal.Rows)
                    {
                        insertar = true;
                        if (Dt_Final.Rows.Count > 0)
                        {
                            foreach(DataRow Registro in Dt_Final.Rows){
                                if (Fila["CODIGO_PROGRAMATICO"].ToString() == Registro["CODIGO_PROGRAMATICO"].ToString() && Fila["DIRECCION_GENERAL"].ToString() == Registro["DIRECCION_GENERAL"].ToString() && Fila["NIVEL"].ToString() == Registro["NIVEL"].ToString())
                                {
                                    insertar = false;
                                }
                            }
                            if (insertar == true)
                            {
                                DataRow row = Dt_Final.NewRow();
                                row["DIRECCION_GENERAL"] = Fila["DIRECCION_GENERAL"].ToString();
                                row["CODIGO_PROGRAMATICO"] = Fila["CODIGO_PROGRAMATICO"].ToString();
                                row["NIVEL"] = Fila["NIVEL"].ToString();
                                row["PLAZAS_PRESUPUESTADAS"] = Fila["PLAZAS_PRESUPUESTADAS"].ToString();
                                row["PLAZAS_OCUPADAS"] = Fila["PLAZAS_OCUPADAS"].ToString();
                                row["PLAZAS_DISPONIBLES"] = Fila["PLAZAS_DISPONIBLES"].ToString();
                                row["SUELDO_MENSUAL"] = Fila["SUELDO_MENSUAL"].ToString();
                                row["PSM"] = Fila["PSM"].ToString();
                                row["OTROS"] = Fila["OTROS"].ToString();
                                row["TIPO_PLAZA"] = Fila["TIPO_PLAZA"].ToString();
                                Dt_Final.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                                Dt_Final.AcceptChanges();
                            }
                        }
                        else
                        {
                            DataRow row = Dt_Final.NewRow();
                            row["DIRECCION_GENERAL"] = Fila["DIRECCION_GENERAL"].ToString();
                            row["CODIGO_PROGRAMATICO"] = Fila["CODIGO_PROGRAMATICO"].ToString();
                            row["NIVEL"] = Fila["NIVEL"].ToString();
                            row["PLAZAS_PRESUPUESTADAS"] = Fila["PLAZAS_PRESUPUESTADAS"].ToString();
                            row["PLAZAS_OCUPADAS"] = Fila["PLAZAS_OCUPADAS"].ToString();
                            row["PLAZAS_DISPONIBLES"] = Fila["PLAZAS_DISPONIBLES"].ToString();
                            row["SUELDO_MENSUAL"] = Fila["SUELDO_MENSUAL"].ToString();
                            row["PSM"] = Fila["PSM"].ToString();
                            row["OTROS"] = Fila["OTROS"].ToString();
                            row["TIPO_PLAZA"] = Fila["TIPO_PLAZA"].ToString();
                            Dt_Final.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                            Dt_Final.AcceptChanges();
                        } 
                    }
                    Dt_Final.TableName = "Dt_Plazas";
                    Ds_Reporte.Clear();
                    Ds_Reporte.Tables.Clear();
                    Ds_Reporte.Tables.Add(Dt_Final.Copy());
                    Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Plazas.rpt");
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
            else {
                Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
                Lbl_Mensaje_Error.Text = "";
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: los parametros de PSM <br>";
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
    // NOMBRE DE LA FUNCION: Consulta_Reporte_Plazas_Excel
    // DESCRIPCION : Consulta todas las palzas por dependencia
    // PARAMETROS  : 
    // CREO        : Sergio Manuel Gallardo Andrade
    // FECHA_CREO  : 09-abril-2012
    // MODIFICO          :
    // FECHA_MODIFICO    :
    // CAUSA_MODIFICACION:
    //*******************************************************************************
    private void Consulta_Reporte_Plazas_Excel()
    {
       Cls_Rpt_Nom_Plazas_Negocio Rs_Consulta = new Cls_Rpt_Nom_Plazas_Negocio(); //Conexion hacia la capa de negocios
        Cls_Cat_Nom_Parametros_Negocio Rs_Parametros = new Cls_Cat_Nom_Parametros_Negocio();
        DataTable Dt_Parametro = new DataTable();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Final = new DataTable();
        DataTable Dt_Temporal = new DataTable();
        Ds_Rpt_Nom_Reporte_Plazas Ds_Reporte = new Ds_Rpt_Nom_Reporte_Plazas();
        ReportDocument Reporte = new ReportDocument();
        String Espacios_Blanco;
        Double Ocupados = 0;
        Double Disponible = 0;
        Double Porcentaje_PSM = 0;
        Boolean insertar = true;
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte Plazas" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al document
        try
        {
            if (Cmb_Unidad_Responsable_Busqueda.SelectedIndex > 0)
            {
                Rs_Consulta.P_Dependencia_ID = Cmb_Unidad_Responsable_Busqueda.SelectedValue;
            }
            if(Cmb_Puesto.SelectedIndex >0){
                Rs_Consulta.P_Puesto_ID = Cmb_Puesto.SelectedValue;
            }
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
            {
                Rs_Consulta.P_Tipo_Nomina = Cmb_Tipo_Nomina.SelectedValue;
            }
            Rs_Consulta.P_Usuario_Creo = Cls_Sessiones.Empleado_ID.Trim();
            Dt_Consulta = Rs_Consulta.Consultar_Plazas();
            Dt_Parametro = Rs_Parametros.Consulta_Parametros();
            if (Dt_Parametro.Rows.Count > 0)
            {
                Porcentaje_PSM = Convert.ToDouble(Dt_Parametro.Rows[0]["PORCENTAJE_PSM"].ToString());

                if (Dt_Consulta.Rows.Count > 0)
                {
                    Dt_Consulta.DefaultView.RowFilter = "Estatus='OCUPADO'";
                    if (Dt_Consulta.DefaultView.ToTable().Rows.Count > 0)
                    {
                        Ocupados = Dt_Consulta.DefaultView.ToTable().Rows.Count;
                    }
                    Dt_Consulta.DefaultView.RowFilter = "Estatus='DISPONIBLE'";
                    if (Dt_Consulta.DefaultView.ToTable().Rows.Count > 0)
                    {
                        Disponible = Dt_Consulta.DefaultView.ToTable().Rows.Count;
                    }
                    if (Dt_Temporal.Rows.Count <= 0)
                    {
                        //Agrega los campos que va a contener el DataTable
                        Dt_Temporal.Columns.Add("DIRECCION_GENERAL", typeof(System.String));
                        Dt_Temporal.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
                        Dt_Temporal.Columns.Add("PLAZAS_PRESUPUESTADAS", typeof(System.Double));
                        Dt_Temporal.Columns.Add("PLAZAS_OCUPADAS", typeof(System.Double));
                        Dt_Temporal.Columns.Add("PLAZAS_DISPONIBLES", typeof(System.Double));
                        Dt_Temporal.Columns.Add("NIVEL", typeof(System.String));
                        Dt_Temporal.Columns.Add("SUELDO_MENSUAL", typeof(System.Double));
                        Dt_Temporal.Columns.Add("PSM", typeof(System.Double));
                        Dt_Temporal.Columns.Add("OTROS", typeof(System.String));
                        Dt_Temporal.Columns.Add("TIPO_PLAZA", typeof(System.String));
                        //Agrega los campos que va a contener el DataTable
                        Dt_Final.Columns.Add("DIRECCION_GENERAL", typeof(System.String));
                        Dt_Final.Columns.Add("CODIGO_PROGRAMATICO", typeof(System.String));
                        Dt_Final.Columns.Add("PLAZAS_PRESUPUESTADAS", typeof(System.Double));
                        Dt_Final.Columns.Add("PLAZAS_OCUPADAS", typeof(System.Double));
                        Dt_Final.Columns.Add("PLAZAS_DISPONIBLES", typeof(System.Double));
                        Dt_Final.Columns.Add("NIVEL", typeof(System.String));
                        Dt_Final.Columns.Add("SUELDO_MENSUAL", typeof(System.Double));
                        Dt_Final.Columns.Add("PSM", typeof(System.Double));
                        Dt_Final.Columns.Add("OTROS", typeof(System.String));
                        Dt_Final.Columns.Add("TIPO_PLAZA", typeof(System.String));
                    }

                    foreach (DataRow Fila in Dt_Consulta.Rows)
                    {
                        Double ocupadas = 0;
                        Double disponibles = 0;
                        DataRow row = Dt_Temporal.NewRow();
                        row["DIRECCION_GENERAL"] = Fila["DIRECCION_GENERAL"].ToString();
                        row["CODIGO_PROGRAMATICO"] = Fila["CODIGO_PROGRAMATICO"].ToString();
                        row["NIVEL"] = Fila["NIVEL"].ToString();
                        foreach (DataRow Registro in Dt_Consulta.Rows)
                        {
                            if (row["NIVEL"].ToString() == Registro["NIVEL"].ToString() && Registro["ESTATUS"].ToString() == "OCUPADO")
                            {
                                ocupadas = ocupadas + 1;
                            }
                            if (row["NIVEL"].ToString() == Registro["NIVEL"].ToString() && Registro["ESTATUS"].ToString() == "DISPONIBLE")
                            {
                                disponibles = disponibles + 1;
                            }
                        }
                        row["PLAZAS_PRESUPUESTADAS"] = ocupadas + disponibles;
                        row["PLAZAS_OCUPADAS"] = ocupadas;
                        row["PLAZAS_DISPONIBLES"] = disponibles;
                        row["SUELDO_MENSUAL"] = Convert.ToDouble(Fila["SUELDO_MENSUAL"].ToString()) * 30.42;
                        row["PSM"] = (Convert.ToDouble(row["SUELDO_MENSUAL"].ToString()) * Porcentaje_PSM) / 100;
                        row["OTROS"] = Fila["USUARIO_CREO"].ToString();
                        row["TIPO_PLAZA"] = Fila["TIPO_PLAZA"].ToString();
                        Dt_Temporal.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                        Dt_Temporal.AcceptChanges();
                    }
                    foreach (DataRow Fila in Dt_Temporal.Rows)
                    {
                        if (Dt_Final.Rows.Count > 0)
                        {
                            insertar = true;
                            foreach (DataRow Registro in Dt_Final.Rows)
                            {
                                if (Fila["CODIGO_PROGRAMATICO"].ToString() == Registro["CODIGO_PROGRAMATICO"].ToString() && Fila["DIRECCION_GENERAL"].ToString() == Registro["DIRECCION_GENERAL"].ToString() && Fila["NIVEL"].ToString() == Registro["NIVEL"].ToString())
                                {
                                    insertar = false;
                                }
                            }
                            if (insertar == true)
                            {
                                DataRow row = Dt_Final.NewRow();
                                row["DIRECCION_GENERAL"] = Fila["DIRECCION_GENERAL"].ToString();
                                row["CODIGO_PROGRAMATICO"] = Fila["CODIGO_PROGRAMATICO"].ToString();
                                row["NIVEL"] = Fila["NIVEL"].ToString();
                                row["PLAZAS_PRESUPUESTADAS"] = Fila["PLAZAS_PRESUPUESTADAS"].ToString();
                                row["PLAZAS_OCUPADAS"] = Fila["PLAZAS_OCUPADAS"].ToString();
                                row["PLAZAS_DISPONIBLES"] = Fila["PLAZAS_DISPONIBLES"].ToString();
                                row["SUELDO_MENSUAL"] = Fila["SUELDO_MENSUAL"].ToString();
                                row["PSM"] = Fila["PSM"].ToString();
                                row["OTROS"] = Fila["OTROS"].ToString();
                                row["TIPO_PLAZA"] = Fila["TIPO_PLAZA"].ToString();
                                Dt_Final.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                                Dt_Final.AcceptChanges();
                            }
                        }
                        else
                        {
                            DataRow row = Dt_Final.NewRow();
                            row["DIRECCION_GENERAL"] = Fila["DIRECCION_GENERAL"].ToString();
                            row["CODIGO_PROGRAMATICO"] = Fila["CODIGO_PROGRAMATICO"].ToString();
                            row["NIVEL"] = Fila["NIVEL"].ToString();
                            row["PLAZAS_PRESUPUESTADAS"] = Fila["PLAZAS_PRESUPUESTADAS"].ToString();
                            row["PLAZAS_OCUPADAS"] = Fila["PLAZAS_OCUPADAS"].ToString();
                            row["PLAZAS_DISPONIBLES"] = Fila["PLAZAS_DISPONIBLES"].ToString();
                            row["SUELDO_MENSUAL"] = Fila["SUELDO_MENSUAL"].ToString();
                            row["PSM"] = Fila["PSM"].ToString();
                            row["OTROS"] = Fila["OTROS"].ToString();
                            row["TIPO_PLAZA"] = Fila["TIPO_PLAZA"].ToString();
                            Dt_Final.Rows.Add(row); //Agrega el registro creado con todos sus valores a la tabla
                            Dt_Final.AcceptChanges();
                        }
                    }
                    Dt_Final.TableName = "Dt_Plazas";
                    Ds_Reporte.Clear();
                    Ds_Reporte.Tables.Clear();
                    Ds_Reporte.Tables.Add(Dt_Final.Copy());
                    Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Plazas.rpt", "Reporte_Nomina_Plazas" + Session.SessionID, "xls", ExportFormatType.Excel);
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
                    Consulta_Plazas();
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
                Consulta_Reporte_Plazas_Excel();

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
