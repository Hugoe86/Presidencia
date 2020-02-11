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
using Presidencia.Reportes_Tramites.Negocios;
using Presidencia.Ventanilla_Consultar_Tramites.Negocio;
using Presidencia.Ordenamiento_Territorial_Inspectores.Negocio;
using Presidencia.Orden_Territorial_Bitacora_Documentos.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Rpt_Ort_Obras_Arquitecto : System.Web.UI.Page
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
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Arquitecto_Perito.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Personal.Attributes.Add("onclick", Ventana_Modal);
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
            Cargar_Combo_Perito();
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
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Perito
    ///DESCRIPCIÓN          : se cargara las areas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Cargar_Combo_Perito()
    {
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Consultar_Inspectores = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consultar_Inspectores.Consultar_Inspectores();
            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Perito.DataSource = Dt_Consulta;
                Cmb_Perito.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
                Cmb_Perito.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
                Cmb_Perito.DataBind();
                Cmb_Perito.Items.Insert(0, "< SELECCIONE >");
            }
            else
            {
                Cmb_Perito.DataSource = new DataTable();
                Cmb_Perito.DataBind();

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
        String Nombre_Archivo = "Reporte_Obras_Arquitecto" + Convert.ToString(String.Format("{0:ddMMMyyy}", DateTime.Today)); //Obtiene el nombre del archivo que sera asignado al documento
        String Ruta_Archivo = @Server.MapPath("../Rpt/Ordenamiento_Territorial/");//Obtiene la ruta en la cual será guardada el archivo
        ReportDocument Reporte = new ReportDocument();
        DataRow Renglon;
        try
        {
            Reporte.Load(Ruta_Archivo + "Rpt_Ort_Obras_Arquitecto.rpt");
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

        if (Cmb_Perito.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione al personal.<br>";
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
        Cls_Ope_Tra_Reportes_Negocio Negocio_Obras_Arquitecto = new Cls_Ope_Tra_Reportes_Negocio();
        DataTable Dt_Consultar_Obras = new DataTable();
        Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
        DataSet Ds_Consulta = new DataSet();
        try
        {
            if (Validar_Datos())
            {
                Mostrar_Mensaje_Error(false);

                Negocio_Obras_Arquitecto.P_Estatus = "TERMINADO";
                Negocio_Obras_Arquitecto.P_Inspector_ID = Cmb_Perito.SelectedValue;
                Dt_Consultar_Obras = Negocio_Obras_Arquitecto.Consulta_Obras_Inspector();

                Dt_Consultar_Obras.TableName = "Dt_Obras_Arquitecto";

                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Consultar_Obras.Copy());

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
        Cls_Ope_Tra_Reportes_Negocio Negocio_Obras_Arquitecto = new Cls_Ope_Tra_Reportes_Negocio();
        DataTable Dt_Consultar_Obras = new DataTable();
        DataTable Dt_Actividades_Realizadas = new DataTable();
        Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite Ds_Reporte = new Ds_Rpt_Ort_Seguimiento_Solicitud_Tramite();
        DataSet Ds_Consulta = new DataSet();
        try
        {
            if (Validar_Datos())
            {
                Mostrar_Mensaje_Error(false);

                Negocio_Obras_Arquitecto.P_Estatus = "TERMINADO";
                Negocio_Obras_Arquitecto.P_Inspector_ID = Cmb_Perito.SelectedValue;
                Dt_Consultar_Obras = Negocio_Obras_Arquitecto.Consulta_Obras_Inspector();

                Dt_Consultar_Obras.TableName = "Dt_Obras_Arquitecto";

                Ds_Reporte.Clear();
                Ds_Reporte.Tables.Clear();
                Ds_Reporte.Tables.Add(Dt_Consultar_Obras.Copy());

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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Personal_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Personal_Click(object sender, EventArgs e)
    {
        Cls_Cat_Ort_Inspectores_Negocio Negocio_Buscar = new Cls_Cat_Ort_Inspectores_Negocio();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Tramite = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false);
            if (Session["BUSQUEDA_ARQUITECTO"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_ARQUITECTO"].ToString());

                if (Estado != false)
                {
                    String Arquitecto_ID = Session["INSPECTOR_ID"].ToString();

                    if (Cmb_Perito.Items.FindByValue(Arquitecto_ID) != null)
                    {
                        Cmb_Perito.SelectedValue = Arquitecto_ID;
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
