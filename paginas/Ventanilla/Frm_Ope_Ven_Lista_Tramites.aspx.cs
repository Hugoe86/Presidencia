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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using AjaxControlToolkit;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Solicitud_Tramites.Negocios;
using Presidencia.Ventanilla_Lista_Tramites.Negocio;
using Presidencia.Dependencias.Negocios;

public partial class paginas_Ventanilla_Frm_Ope_Ven_Lista_Tramites : System.Web.UI.Page
{
    #region (Page Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones   //Limpia los controles del forma
                //Accion_Solicitar_Tramite();
            }
            String Ventana_Modal = "Abrir_Ventana_Modal('../Atencion_Ciudadana/Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);

        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE:         Configuracion_Acceso
    /// DESCRIPCIÓN:    Habilita las operaciones que podrá realizar el usuario en la página.
    /// PARÁMETROS:     No Áplica.
    /// USUARIO CREO:   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   20/Enero/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Salir);
            //Botones.Add(Btn_Mostrar_Popup_Busqueda);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION :   Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS:     Cadena.- El dato a evaluar si es numerico.
    /// USUARIO CREO:   Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  :   20/Enero/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
    {
        Boolean Resultado = true;
        Char[] Array = Cadena.ToCharArray();
        try
        {
            for (int index = 0; index < Array.Length; index++)
            {
                if (!Char.IsDigit(Array[index])) return false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar si es un dato numerico. Error [" + Ex.Message + "]");
        }
        return Resultado;
    }
    #endregion

    #region Metodos Generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 02/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Limpiar_Controles();
            Habilitar_Controles();
            Cargar_Grid_Top_5();
            Cargar_Combo_Unidad_Responsable();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cargar_Grid_Top_5
    /// DESCRIPCION : cargara los 5 registros de tramites mas usados
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 16/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Grid_Top_5()
    {
        Cls_Ope_Ven_Lista_Tramites_Negocio Rs_Consulta_Top_5 = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        DataSet Ds_Consulta = new DataSet();
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Top_5= new DataTable();
        try
        {
            Rs_Consulta_Top_5.P_Estatus = "ACTIVO";
            Dt_Consulta = Rs_Consulta_Top_5.Consultar_Tramites_Populares();

            if (Dt_Consulta is DataTable)
            {
                if (Dt_Consulta.Rows.Count > 0)
                {
                    Dt_Top_5 = Dt_Consulta.Clone();
                    for (int Contador_For = 0; Contador_For < 10; Contador_For++)
                    {
                        if (  Contador_For < Dt_Consulta.Rows.Count)
                        {
                            Dt_Top_5.ImportRow(Dt_Consulta.Rows[Contador_For]);
                            Dt_Top_5.AcceptChanges();
                        }
                    }

                    Grid_Lista_Tramites.Columns[7].Visible = true;
                    Grid_Lista_Tramites.Columns[0].Visible = true;
                    Grid_Lista_Tramites.Columns[3].Visible = true;
                    Grid_Lista_Tramites.DataSource = Dt_Top_5;
                    Grid_Lista_Tramites.DataBind();
                    Grid_Lista_Tramites.Columns[0].Visible = false;
                    Grid_Lista_Tramites.Columns[3].Visible = false;

                    if (String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
                    {
                        Grid_Lista_Tramites.Columns[7].Visible = false;
                    }

                     
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE:       Cargar_Combo_Unidad_Responsable
    /// DESCRIPCION : Cargara las unidades responsables 
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 30/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Cargar_Combo_Unidad_Responsable()
    {
        Cls_Cat_Dependencias_Negocio Rs_Responsable = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Unidad_Responsable = new DataTable();   
        try
        {
            //  1 para la unidad resposable
            Dt_Unidad_Responsable = Rs_Responsable.Consulta_Dependencias();
            //   2 SE ORDENA LA TABLA POR 
            DataView Dv_Ordenar = new DataView(Dt_Unidad_Responsable);
            Dv_Ordenar.Sort = Cat_Dependencias.Campo_Nombre;
            Dt_Unidad_Responsable = Dv_Ordenar.ToTable();
            Cmb_Unidad_Responsable_Filtro.DataSource = Dt_Unidad_Responsable;
            Cmb_Unidad_Responsable_Filtro.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Unidad_Responsable_Filtro.DataTextField = Cat_Dependencias.Campo_Nombre;
            Cmb_Unidad_Responsable_Filtro.DataBind();
            Cmb_Unidad_Responsable_Filtro.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 02/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Cls_Sessiones.No_Empleado = "";

            if (String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
            {
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = " Requiere tener cuenta para realizar algun tramite, Registrese por favor";
            }
            else
            {
                Img_Error.Visible = false;
                Lbl_Mensaje_Error.Visible = false;
                Lbl_Mensaje_Error.Text = "";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 03/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles()
    {
        try
        {
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Solicitar_Tramite
    /// DESCRIPCION : Carga la pantalla de solicitud de tramite con la informacion del 
    ///               tramite seleccionado por el usuario
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 02/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Accion_Solicitar_Tramite()
    {
        String Accion = String.Empty;
        String Tramite_ID = String.Empty;
        try
        {
            if (Request.QueryString["Accion"] != null)
            {
                Accion = HttpUtility.UrlDecode(Request.QueryString["Accion"].ToString());
                if (Request.QueryString["id"] != null)
                {
                    Tramite_ID = HttpUtility.UrlDecode(Request.QueryString["id"].ToString());
                }

                switch (Accion)
                {
                    case "Accion_Solicitar_Tramite":
                        Cls_Sessiones.No_Empleado = Tramite_ID;

                        break;

                    case "Deshabilitar":
                        Deshabilitar_Grid();
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #region (Reportes)
    /// *************************************************************************************
    /// NOMBRE:         Generar_Reporte
    /// DESCRIPCIÓN:    Método que invoca la generación del reporte.
    /// PARÁMETROS:     Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///                 Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     02/Mayo/2012
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
            Ruta = @Server.MapPath("../Rpt/Ventanilla/" + Nombre_Plantilla_Reporte);
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
    /// NOMBRE:         Exportar_Reporte_PDF
    /// DESCRIPCIÓN:    Método que guarda el reporte generado en formato PDF en la ruta
    ///                 especificada.
    /// PARÁMETROS:     Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                 Nombre_Reporte.- Nombre que se le dará al reporte.
    /// USUARIO CREO:   Hugo Enrique Ramírez Aguilera.
    /// FECHA CREO:     02/Mayo/2012
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
    /// NOMBRE:         Mostrar_Reporte
    /// DESCRIPCIÓN:    Muestra el reporte en pantalla.
    /// PARÁMETROS:     Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// USUARIO CREO:   Hugo Enrique Ramírez Aguilera.
    /// FECHA CREO:     02/Mayo/2012
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


    #region Eventos
    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operacion actual que se este realizando
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Cls_Sessiones.Ciudadano_ID == "")
        {
            Response.Redirect("../Ventanilla/Frm_Apl_Login_Ventanilla.aspx");
        }
        else
        {
            Response.Redirect("../Ventanilla/Frm_Apl_Ventanilla.aspx");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Tramite_Click
    ///DESCRIPCIÓN: Buscara el tramite para luego cargarlo en el grid
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Tramite_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Ven_Lista_Tramites_Negocio Rs_Consulta_Tramites = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        Boolean Estado = false;
        try
        {
            if (Txt_Nombre_Tramite.Text != "")
            {
                Rs_Consulta_Tramites.P_Nombre_Tramite = Txt_Nombre_Tramite.Text;
                Estado = true;
            }
            if (Txt_Clave_Tramite.Text != "")
            {
                Rs_Consulta_Tramites.P_Clave_Tramite = Txt_Clave_Tramite.Text;
                Estado = true;
            }
            if (Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
            {
                Rs_Consulta_Tramites.P_Dependencia_Tramite = Cmb_Unidad_Responsable_Filtro.SelectedValue;
                Estado = true;
            }


            if (Estado == true)
            {
                Rs_Consulta_Tramites.P_Estatus = "ACTIVO";
                Dt_Consulta = Rs_Consulta_Tramites.Consultar_Tramites();

                if (Dt_Consulta is DataTable)
                {
                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        Grid_Lista_Tramites.Columns[0].Visible = true;
                        Grid_Lista_Tramites.Columns[3].Visible = true;
                        Grid_Lista_Tramites.DataSource = Dt_Consulta;
                        Grid_Lista_Tramites.DataBind();
                        Grid_Lista_Tramites.Columns[0].Visible = false;
                        Grid_Lista_Tramites.Columns[3].Visible = false;
                    }
                    else
                    {
                        Grid_Lista_Tramites.Columns[0].Visible = true;
                        Grid_Lista_Tramites.Columns[3].Visible = true;
                        Grid_Lista_Tramites.DataSource = new DataTable();
                        Grid_Lista_Tramites.DataBind();
                        Grid_Lista_Tramites.Columns[0].Visible = false;
                        Grid_Lista_Tramites.Columns[3].Visible = false;
                    }
                }
            }
            else
            {
                Cargar_Grid_Top_5();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Requisitos_Click
    ///DESCRIPCIÓN: Mostrara los documentos que se requieren para el tramite
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ver_Requisitos_Click(object sender, EventArgs e)
    {
        Cls_Ope_Ven_Lista_Tramites_Negocio Rs_Consulta_Documentos = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        String Tramite_ID = String.Empty;
        String Nombre_Tramite = String.Empty;
        String Descipcion_Tramite = String.Empty;
        DataTable Dt_Consulta = new DataTable();
        DataSet Ds_Reporte = new DataSet(); 
        DataTable Dt_Actividades = new DataTable();
        try
        {
            ImageButton ImageButton = (ImageButton)sender;
            TableCell TableCell = (TableCell)ImageButton.Parent;
            GridViewRow Row = (GridViewRow)TableCell.Parent;
            Grid_Lista_Tramites.SelectedIndex = Row.RowIndex;
            int Fila = Row.RowIndex;

            Grid_Lista_Tramites.Columns[0].Visible = true;
            Tramite_ID = Grid_Lista_Tramites.Rows[Fila].Cells[0].Text.Trim();
            
            Grid_Lista_Tramites.Columns[0].Visible = false;

            Rs_Consulta_Documentos.P_Tramite_ID = Tramite_ID;
            Dt_Consulta = Rs_Consulta_Documentos.Consultar_Documentos_Tramites(); 
            Dt_Actividades = Rs_Consulta_Documentos.Consultar_Actividades_Tramites();
            Dt_Consulta.TableName = "Dt_Datos_Tramite";
            Dt_Actividades.TableName = "Dt_Actividades";
           

            Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
            Ds_Reporte.Tables.Add(Dt_Actividades.Copy());
            Generar_Reporte(ref Ds_Reporte, "Rpt_Ven_Lista_Documentos_Tramites.rpt", "Reporte_Documentos_Tramite" + Session.SessionID + ".pdf");

            Grid_Lista_Tramites.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_DEPENDENCIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_DEPENDENCIAS"]) == true)
            {
                try
                {
                    string Dependencia_ID = Session["DEPENDENCIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Unidad_Responsable_Filtro.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Unidad_Responsable_Filtro.SelectedValue = Dependencia_ID;
                        if (Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
                        {
                            Btn_Buscar_Tramite_Click(sender, null);
                        }
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Autorizar_Solicitud_Click
    ///DESCRIPCIÓN: Autorizar Mostrara los documentos que se requieren para el tramite
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  03/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Autorizar_Solicitud_Click(object sender, EventArgs e)
    {
        DataTable Dt_Informacion_Grid = new DataTable();
        String Tramite_ID = "";
        try
        {
            ImageButton Imagen_Boton = (ImageButton)sender;
            TableCell Celda = (TableCell)Imagen_Boton.Parent;
            GridViewRow Renglon = (GridViewRow)Celda.Parent;
            Grid_Lista_Tramites.SelectedIndex = Renglon.RowIndex;
            int Fila = Renglon.RowIndex;


            Grid_Lista_Tramites.Columns[0].Visible = true;
            Tramite_ID = Grid_Lista_Tramites.Rows[Fila].Cells[0].Text.Trim();
            Grid_Lista_Tramites.Columns[0].Visible = false;

            Cls_Sessiones.No_Empleado = Tramite_ID;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
        Response.Redirect("../Ventanilla/Frm_Ope_Ven_Solicitar_Tramite.aspx");

              
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cancelar_Click
    ///DESCRIPCIÓN: deshabilita el grid y los elementos seleccionados
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  03/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cancelar_Click(object sender, EventArgs e)
    {
        try
        {
            Cargar_Grid_Top_5();

            if (!String.IsNullOrEmpty(Cls_Sessiones.Nombre_Empleado))
            {

            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Formato_Click
    ///DESCRIPCIÓN: mostrara el documento
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ver_Formato_Click(object sender, ImageClickEventArgs e)
    {
        String URL = String.Empty;
        int Fila = 0;
        TableCell Celda = new TableCell();
        GridViewRow Renglon;
        ImageButton Boton = new ImageButton();
        String Nombre_Archivo = "";
        String Nombre_Documento = "";
        String Directorio_Portafolio = "";
        try
        {
            //  para obtener el id del documento 
            Boton = (ImageButton)sender;
            Celda = (TableCell)Boton.Parent;
            Renglon = (GridViewRow)Celda.Parent;
            Grid_Lista_Tramites.SelectedIndex = Renglon.RowIndex;
            Fila = Renglon.RowIndex;

            //  se obtiene el nombre del documento y el id del ciudadano
            Nombre_Documento = Grid_Lista_Tramites.Rows[Fila].Cells[1].Text.Trim();
            
            if (Nombre_Documento.Contains("/"))
            {
                Nombre_Documento = Nombre_Documento.Replace("/", "");
            }

            //  se obtiene el nombre de los archivos existentes en la carpeta
            String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/Formato_Tramite/"));

            //  se busca el archivo
            for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
            {
                Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                if (Nombre_Archivo.Contains(Nombre_Documento))
                {
                    URL = Archivos[Contador].Trim();
                    Mostrar_Archivo(URL);
                    break;
                }

            }// fin del for

            if (URL == null)
            {
                
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Cargar_Grid " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo
    ///DESCRIPCIÓN: Muestra un Archivo del cual se le pasa la ruta como parametro.
    ///PARAMETROS:
    ///             1.  Ruta.  Ruta del Archivo.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Archivo(String Ruta)
    {
        try
        {
            if (System.IO.File.Exists(Ruta))
            {
                //System.Diagnostics.Process proceso = new System.Diagnostics.Process();
                //proceso.StartInfo.FileName = Ruta;
                //proceso.Start();
                //proceso.Close();

                String Archivo = "";
                Archivo = "../../Archivos/Formato_Tramite/" + Path.GetFileName(Ruta);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe o fue eliminado');", true);
            }
        }
        catch (Exception ex)
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Deshabilitar_Grid
    ///DESCRIPCIÓN: deshabilita el grid y los elementos seleccionados
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  07/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Deshabilitar_Grid()
    {
        try
        {
           
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region ComboBox
    

         ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Cmb_Unidad_Responsable_Filtro_SelectedIndexChanged
    ///DESCRIPCIÓN: Manejo del evento cambio de índice en el combo condicion, consultar el nombre 
    ///             de la actividad a la que procede
    ///PARÁMETROS:
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 27-jul-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cmb_Unidad_Responsable_Filtro_SelectedIndexChanged(object sender, EventArgs e)
    {
        
        try
        {
            if (Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
            {
                Btn_Buscar_Tramite_Click(sender, null);
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region CheckBox
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Chk_Solicitar_OnCheckedChanged
    ///DESCRIPCIÓN: Autorizar Mostrara los documentos que se requieren para el tramite
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  03/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Solicitar_OnCheckedChanged(object sender, EventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Cls_Sessiones.Nombre_Empleado))
            {
                Grid_Lista_Tramites.Enabled = false;
            }
            else
            {
                Grid_Lista_Tramites.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }


    #endregion
    #endregion


    #region Grid
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Lista_Tramites_OnRowDataBound
    ///DESCRIPCIÓN: Habilitara el boton de autorizar solicitud
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  16/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Lista_Tramites_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        String Nombre_Archivo = "";
        String Nombre_Documento = "";
        Boolean Formato_Disponible = false;
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (!String.IsNullOrEmpty(Cls_Sessiones.Ciudadano_ID))
                {
                    ImageButton Boton_Autorizar = (ImageButton)e.Row.Cells[8].FindControl("Btn_Autorizar_Solicitud");
                    Boton_Autorizar.Enabled = true;

                    ImageButton Boton_Formato = (ImageButton)e.Row.Cells[7].FindControl("Btn_Ver_Formato");

                    //  se obtiene el nombre del documento y el id del ciudadano
                    Nombre_Documento = e.Row.Cells[1].Text;

                    if (Nombre_Documento.Contains("/"))
                    {
                        Nombre_Documento = Nombre_Documento.Replace("/", "");
                    }
                    //  se obtiene el nombre de los archivos existentes en la carpeta
                    String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/Formato_Tramite/"));

                    //  se busca el archivo
                    for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                    {
                        Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                        if (Nombre_Archivo.Contains(Nombre_Documento))
                        {
                            Formato_Disponible = true;
                            break;
                        }

                    }// fin del for

                    if (Formato_Disponible == true)
                    {
                        Boton_Formato.Visible = true;
                    }
                    else
                    {
                        Boton_Formato.Visible = false;
                    }

                    
                }
                else
                {
                    ImageButton Boton_Autorizar = (ImageButton)e.Row.Cells[8].FindControl("Btn_Autorizar_Solicitud");
                    Boton_Autorizar.Enabled = false;

                    ImageButton Boton_Formato = (ImageButton)e.Row.Cells[7].FindControl("Btn_Ver_Formato");

                    //  se obtiene el nombre del documento y el id del ciudadano
                    Nombre_Documento = e.Row.Cells[1].Text;

                    //  se obtiene el nombre de los archivos existentes en la carpeta
                    String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/Formato_Tramite/"));

                    //  se busca el archivo
                    for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                    {
                        Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                        if (Nombre_Archivo.Contains(Nombre_Documento))
                        {
                            Formato_Disponible = true;
                            break;
                        }

                    }// fin del for

                    if (Formato_Disponible == true)
                    {
                        Boton_Formato.Visible = true;
                    }
                    else
                    {
                        Boton_Formato.Visible = false;
                    }

                }
                

               
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    #endregion

}
