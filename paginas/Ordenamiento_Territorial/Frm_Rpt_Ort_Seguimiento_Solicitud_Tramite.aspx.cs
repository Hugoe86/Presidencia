using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using System.Drawing;
using System.Drawing.Drawing2D;
using Presidencia.Constantes;
using AjaxControlToolkit;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Ventanilla_Consultar_Tramites.Negocio;
using Presidencia.Ordenamiento_Territorial_Areas_Publicas.Negocio;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Rpt_Ort_Seguimiento_Solicitud_Tramite : System.Web.UI.Page
{
    #region Load
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : 
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 02/Julio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Inicializar_Controles();
            }
            string Ventana_Modal = "Abrir_Ventana_Modal('../Tramites/Ventanas_Emergente/Frm_Busqueda_Avanzada_Solicitud_Tramite.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Solicitud.Attributes.Add("onclick", Ventana_Modal);
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Metodos generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializar_Controles()
    {
        try
        {
            Limpiar_Controles();
            Cargar_Combo_Solicitud();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Mostrar_Mensaje_Error(false);
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Mensaje_Error
    ///DESCRIPCIÓN          : se habilitan los mensajes de error
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Mensaje_Error(Boolean Accion)
    {
        try
        {
            Img_Error.Visible = Accion;
            Lbl_Mensaje_Error.Visible = Accion;
        }
        catch (Exception ex)
        {
            throw new Exception("Mostrar_Mensaje_Error " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Solicitud
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Cargar_Combo_Solicitud()
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Consultar_Solicitud = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consultar_Solicitud.Consultar_Solicitudes();
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Solicitud.DataSource = Dt_Consulta;
                Cmb_Solicitud.DataValueField = "SOLICITUD_ID";
                Cmb_Solicitud.DataTextField = "CLAVE_SOLICITUD";
                Cmb_Solicitud.DataBind();
                Cmb_Solicitud.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {
                Cmb_Solicitud.DataSource = new DataTable();
                Cmb_Solicitud.DataBind();

                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "No se encuentran solicitudes de tramites";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN: genera el reporte de pdf
    ///PARÁMETROS : 	
    ///         1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 02-Julio-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    public void Generar_Reporte(DataSet Ds_Reporte, String Extension_Archivo)
    {
        String Nombre_Archivo = "Reporte_Seguimiento_Solicitud_" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
        String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo
        ReportDocument Reporte = new ReportDocument();
        DataRow Renglon;
        try
        {
            Reporte.Load(Ruta_Archivo + "Rpt_Ort_Seguimiento_Solicitud_Tramite.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Nombre_Archivo += ".pdf";
            else if (Extension_Archivo == "EXCEL")
                Nombre_Archivo += ".xls";

            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;

            //  para el tipo de archivo
            if (Extension_Archivo == "PDF")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            else if (Extension_Archivo == "EXCEL")
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;

            Reporte.Export(Opciones_Exportacion);

            if (Extension_Archivo == "PDF")
                Abrir_Ventana(Nombre_Archivo);
            else if (Extension_Archivo == "EXCEL")
            {
                String Ruta_Destino = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta_Destino + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
                
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 02-Julio-2012
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

    #region Validaciones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
        Mostrar_Mensaje_Error(true);

        if (Cmb_Solicitud.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione la solcitidud de tramite.<br>";
            Datos_Validos = false;
        }


        return Datos_Validos;
    }
    #endregion

    #region Eventos

    #region Botones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Exportar_Excel_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Exportar_Excel_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
        DataTable Dt_Consulta_Solicitud = new DataTable();
        DataTable Dt_Actividades_Realizadas = new DataTable(); 
        DataTable Dt_Reporte_Datos = new DataTable(); 
        Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
        DataSet Ds_Consulta = new DataSet();
        try
        {
            if (Validar_Datos())
            {
                Mostrar_Mensaje_Error(false);

                Dt_Consulta_Solicitud = Ds_Reporte.Dt_Datos_Solicitud.Clone();
                Dt_Actividades_Realizadas = Ds_Reporte.Dt_Seguimiento_Solicitud.Clone();
                Dt_Reporte_Datos = Ds_Reporte.Dt_Datos.Clone();

                Negocio_Actividades_Realizadas.P_Solicitud_id = Cmb_Solicitud.SelectedValue;
                Dt_Consulta_Solicitud = Negocio_Actividades_Realizadas.Consultar_Tramites();
                Dt_Actividades_Realizadas = Negocio_Actividades_Realizadas.Consultar_Historial_Actividades();
                Dt_Reporte_Datos = Negocio_Actividades_Realizadas.Consultar_Historial_Documentos();

                Dt_Consulta_Solicitud.TableName = "Dt_Datos_Solicitud";
                Dt_Actividades_Realizadas.TableName = "Dt_Seguimiento_Solicitud";
                Dt_Reporte_Datos.TableName = "Dt_Datos";

                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Consulta_Solicitud.Copy());
                Ds_Reporte.Tables.Add(Dt_Actividades_Realizadas.Copy());
                Ds_Reporte.Tables.Add(Dt_Reporte_Datos.Copy());

                Generar_Reporte(Ds_Reporte, "EXCEL");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Reporte_Pdf_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Reporte_Pdf_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Rpt_Ven_Consultar_Tramites_Negocio Negocio_Actividades_Realizadas = new Cls_Rpt_Ven_Consultar_Tramites_Negocio();
        DataTable Dt_Consulta_Solicitud = new DataTable(); 
        DataTable Dt_Actividades_Realizadas = new DataTable();
        DataTable Dt_Reporte_Datos = new DataTable();
        Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
        DataSet Ds_Consulta = new DataSet();
        try
        {
            if (Validar_Datos())
            {
                Mostrar_Mensaje_Error(false);

                Dt_Consulta_Solicitud = Ds_Reporte.Dt_Datos_Solicitud.Clone();
                Dt_Actividades_Realizadas = Ds_Reporte.Dt_Seguimiento_Solicitud.Clone();
                Dt_Reporte_Datos = Ds_Reporte.Dt_Datos.Clone();

                Negocio_Actividades_Realizadas.P_Solicitud_id = Cmb_Solicitud.SelectedValue;
                Dt_Consulta_Solicitud = Negocio_Actividades_Realizadas.Consultar_Tramites();
                Dt_Actividades_Realizadas = Negocio_Actividades_Realizadas.Consultar_Historial_Actividades();
                Dt_Reporte_Datos = Negocio_Actividades_Realizadas.Consultar_Historial_Documentos();

                Dt_Consulta_Solicitud.TableName = "Dt_Datos_Solicitud";
                Dt_Actividades_Realizadas.TableName = "Dt_Seguimiento_Solicitud";
                Dt_Reporte_Datos.TableName = "Dt_Datos";

                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Consulta_Solicitud.Copy());
                Ds_Reporte.Tables.Add(Dt_Actividades_Realizadas.Copy()); 
                Ds_Reporte.Tables.Add(Dt_Reporte_Datos.Copy());
                Generar_Reporte(Ds_Reporte, "PDF");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Cancelar")
            {
                Inicializar_Controles();
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Solicitud_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Solicitud_Click(object sender, EventArgs e)
    {
        Cls_Ope_Ort_Bitacora_Documentos_Negocio Negocio_Buscar = new Cls_Ope_Ort_Bitacora_Documentos_Negocio();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Tramite = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false);
            if (Session["BUSQUEDA_SOLICITUD"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_SOLICITUD"].ToString());

                if (Estado != false)
                {
                    String Solicitud_id = Session["SOLICITUD_ID"].ToString();
                    Negocio_Buscar.P_Solicitud_ID = Solicitud_id;
                    Negocio_Buscar.P_Estatus_Busqueda = "ENTREGADO";
                    Dt_Consulta = Negocio_Buscar.Consultar_Bitacora();

                    if (Cmb_Solicitud.Items.FindByValue(Solicitud_id) != null)
                    {
                        Cmb_Solicitud.SelectedValue = Solicitud_id;
                    }
                    

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true);
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion
    #endregion
}
