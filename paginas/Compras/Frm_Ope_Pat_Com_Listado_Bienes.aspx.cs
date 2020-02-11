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
using System.IO;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Listado_Bienes.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;


public partial class paginas_predial_Frm_Ope_Pat_Com_Listado_Bienes : System.Web.UI.Page {

    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 09/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Page_Load(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Combo_Marcas();
                Llenar_Combo_Unidades_Responsables();
                Llenar_Combo_Tipo_Cemovientes();
                Llenar_Combo_Razas();
            }
            Configuracion_Acceso("Frm_Ope_Pat_Com_Listado_Bienes.aspx");
        }

    #endregion

    #region Metodos

        #region Limpiar_Datos

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Limpiar_Datos_Generales
            ///DESCRIPCIÓN: Limpia los Campos para los datos generales del listado.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 12/Septiembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Limpiar_Datos_Generales() {
                Txt_Inventario_Anterior.Text = "";
                Txt_Numero_Inventario.Text = "";
                Txt_Producto.Text = "";
                Cmb_Exacto_Aproximado.SelectedIndex = 0;
                Cmb_Tipo.SelectedIndex = 0;
                Cmb_Marca.SelectedIndex = 0;
                Txt_Modelo.Text = "";
                Cmb_Estatus.SelectedIndex = 0;
                Txt_Factura.Text = "";
                Txt_Numero_Serie.Text = "";
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Limpiar_Datos_Resguardantes
            ///DESCRIPCIÓN: Limpia los Campos para los datos de los resguardantes del listado.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 12/Septiembre/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Limpiar_Datos_Resguardantes() {
                Txt_No_Empleado_Resguardante.Text = "";
                Txt_RFC_Resguardante.Text = "";
                Cmb_Resguardantes_Dependencias.SelectedIndex = 0;
                Cmb_Nombre_Resguardante.Items.Clear();
                Cmb_Nombre_Resguardante.Items.Insert(0, new ListItem("<- TODOS ->", ""));
            }

        #endregion

        #region Llenado_Componentes

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Marcas
            ///DESCRIPCIÓN: Llena el combo de las marcas.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 12/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Marcas() {
                Cls_Ope_Pat_Com_Listado_Bienes_Negocio Listado_Negocio = new Cls_Ope_Pat_Com_Listado_Bienes_Negocio();
                Listado_Negocio.P_Tipo_DataTable = "MARCAS";
                Cmb_Marca.DataSource = Listado_Negocio.Consultar_DataTable();
                Cmb_Marca.DataTextField = "NOMBRE";
                Cmb_Marca.DataValueField = "MARCA_ID";
                Cmb_Marca.DataBind();
                Cmb_Marca.Items.Insert(0, new ListItem("<- TODAS ->", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipo_Cemoviente
            ///DESCRIPCIÓN: Llena el combo de los Tipos de Cemovientes.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 13/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Tipo_Cemovientes() {
                Cls_Ope_Pat_Com_Listado_Bienes_Negocio Listado_Negocio = new Cls_Ope_Pat_Com_Listado_Bienes_Negocio();
                Listado_Negocio.P_Tipo_DataTable = "TIPOS_CEMOVIENTES";
                Cmb_Tipo_Cemoviente.DataSource = Listado_Negocio.Consultar_DataTable();
                Cmb_Tipo_Cemoviente.DataTextField = "NOMBRE";
                Cmb_Tipo_Cemoviente.DataValueField = "TIPO_CEMOVIENTE_ID";
                Cmb_Tipo_Cemoviente.DataBind();
                Cmb_Tipo_Cemoviente.Items.Insert(0, new ListItem("<- TODOS ->", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Razas
            ///DESCRIPCIÓN: Llena el combo de las Razas.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 13/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Razas() {
                Cls_Ope_Pat_Com_Listado_Bienes_Negocio Listado_Negocio = new Cls_Ope_Pat_Com_Listado_Bienes_Negocio();
                Listado_Negocio.P_Tipo_DataTable = "RAZAS";
                Cmb_Raza.DataSource = Listado_Negocio.Consultar_DataTable();
                Cmb_Raza.DataTextField = "NOMBRE";
                Cmb_Raza.DataValueField = "RAZA_ID";
                Cmb_Raza.DataBind();
                Cmb_Raza.Items.Insert(0, new ListItem("<- TODOS ->", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Unidades_Responsables
            ///DESCRIPCIÓN: Llena el combo de las Unidades Responsables.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 12/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Unidades_Responsables() {
                Cls_Ope_Pat_Com_Listado_Bienes_Negocio Listado_Negocio = new Cls_Ope_Pat_Com_Listado_Bienes_Negocio();
                Listado_Negocio.P_Tipo_DataTable = "DEPENDENCIAS";
                Cmb_Resguardantes_Dependencias.DataSource = Listado_Negocio.Consultar_DataTable();
                Cmb_Resguardantes_Dependencias.DataTextField = "NOMBRE";
                Cmb_Resguardantes_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Resguardantes_Dependencias.DataBind();
                Cmb_Resguardantes_Dependencias.Items.Insert(0, new ListItem("<- TODAS ->", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Resguardantes
            ///DESCRIPCIÓN: Llena el combo de los Resguardantes.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 12/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Combo_Resguardantes(DataTable Datos) {
                Cmb_Nombre_Resguardante.Items.Clear();
                if (Datos.Rows.Count > 0) {
                    Cmb_Nombre_Resguardante.DataSource = Datos;
                    Cmb_Nombre_Resguardante.DataTextField = "NOMBRE";
                    Cmb_Nombre_Resguardante.DataValueField = "EMPLEADO_ID";
                    Cmb_Nombre_Resguardante.DataBind();
                }
                Cmb_Nombre_Resguardante.Items.Insert(0, new ListItem("<- TODOS ->", ""));
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Ejecutar_Listado
            ///DESCRIPCIÓN: Llena el Grid de Listado de Bienes
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 12/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Ejecutar_Listado(String Visualizacion) { 
                try {
                    String Filtro = null;
                    if (Session["FILTRO"] != null) { Filtro = Session["FILTRO"].ToString(); }
                    if (Filtro != null) {
                        Cls_Ope_Pat_Com_Listado_Bienes_Negocio Listado_Negocio = new Cls_Ope_Pat_Com_Listado_Bienes_Negocio();
                        if (Filtro.Trim().Equals("GENERALES")) {
                            if (Txt_Inventario_Anterior.Text.Trim().Length > 0) { Listado_Negocio.P_Inventario_Anterior = Txt_Inventario_Anterior.Text.Trim(); }
                            if (Txt_Numero_Inventario.Text.Trim().Length > 0) { Listado_Negocio.P_Numero_Inventario = Txt_Numero_Inventario.Text.Trim(); }
                            if (Txt_Producto.Text.Trim().Length>0) {
                                Listado_Negocio.P_Producto = Txt_Producto.Text.Trim();
                                Listado_Negocio.P_Filtro = Cmb_Exacto_Aproximado.SelectedItem.Value;
                            }
                            Listado_Negocio.P_Tipo = Cmb_Tipo.SelectedItem.Value;
                            if (Cmb_Marca.SelectedIndex > 0) { Listado_Negocio.P_Marca = Cmb_Marca.SelectedItem.Value; }
                            if (Cmb_Tipo_Cemoviente.SelectedIndex > 0) { Listado_Negocio.P_Tipo_Cemoviente = Cmb_Tipo_Cemoviente.SelectedItem.Value; }
                            if (Cmb_Raza.SelectedIndex > 0) { Listado_Negocio.P_Raza = Cmb_Raza.SelectedItem.Value; }
                            if (Cmb_Estatus.SelectedIndex > 0) { Listado_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
                            if (Txt_Modelo.Text.Trim().Length > 0) { Listado_Negocio.P_Modelo = Txt_Modelo.Text.Trim(); }
                            if (Cmb_Estatus.SelectedIndex > 0) { Listado_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
                            if (Txt_Factura.Text.Trim().Length > 0) { Listado_Negocio.P_Numero_Factura = Txt_Factura.Text.Trim(); }
                            if (Txt_Numero_Serie.Text.Trim().Length > 0) { Listado_Negocio.P_Numero_Serie = Txt_Numero_Serie.Text.Trim(); }
                        } else if (Filtro.Trim().Equals("RESGUARDANTES")) { 
                            Listado_Negocio.P_Tipo = "TODOS";
                            if (Txt_No_Empleado_Resguardante.Text.Trim().Length > 0) { Listado_Negocio.P_No_Empleado = String.Format("{0:000000}", Convert.ToInt32(Txt_No_Empleado_Resguardante.Text.Trim())); }
                            if (Txt_RFC_Resguardante.Text.Trim().Length > 0) { Listado_Negocio.P_RFC_Resguardante = Txt_RFC_Resguardante.Text.Trim(); }
                            if (Cmb_Resguardantes_Dependencias.SelectedIndex > 0) { Listado_Negocio.P_Unidad_Responsable = Cmb_Resguardantes_Dependencias.SelectedItem.Value; }
                            if (Cmb_Nombre_Resguardante.SelectedIndex > 0) { Listado_Negocio.P_Resguardante = String.Format("{0:0000000000}", Convert.ToInt32(Cmb_Nombre_Resguardante.SelectedItem.Value)); }
                        }
                        DataTable Datos = Listado_Negocio.Consultar_Listado_Bienes();
                        if (Visualizacion.Equals("VISUALIZAR")) {
                            Llenar_Grid_Listado_Bienes(Datos);
                        } else if (Visualizacion.Equals("PDF")) {
                            DataSet Ds_Consulta = new DataSet();
                            Datos.TableName = "DATOS";
                            Ds_Consulta.Tables.Add(Datos.Copy());

                            Ds_Pat_Com_Listado_Bienes Ds_Reporte = new Ds_Pat_Com_Listado_Bienes();
                            Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Com_Listado_Bienes.rpt");
                        } 
                    }
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = "Error: ";
                    Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Listado_Bienes
            ///DESCRIPCIÓN: Llena el Grid de Listado de Bienes
            ///PARAMETROS: Datos. Datos para llenar el Grid.
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 13/Septiembre/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Llenar_Grid_Listado_Bienes(DataTable Datos) {
                Grid_Listado_Bienes.Columns[0].Visible = true;
                DataTable Dt_Resultados = Datos;
                Lbl_Numero_Resultados.Text = "[El Número Total de Resultados Encontrados fue de " + Dt_Resultados.Rows.Count.ToString() + "]";
                Grid_Listado_Bienes.DataSource = Dt_Resultados;
                Grid_Listado_Bienes.DataBind();
                Grid_Listado_Bienes.Columns[0].Visible = false;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
            ///DESCRIPCIÓN: caraga el data set fisico con el cual se genera el Reporte especificado
            ///PARAMETROS:  1.-Data_Set_Consulta_DB.- Contiene la informacion de la consulta a la base de datos
            ///             2.-Ds_Reporte, Objeto que contiene la instancia del Data set fisico del Reporte a generar
            ///             3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
            ///CREO: Susana Trigueros Armenta.
            ///FECHA_CREO: 01/Mayo/2011
            ///MODIFICO:
            ///FECHA_MODIFICO:
            ///CAUSA_MODIFICACIÓN:
            ///*******************************************************************************
            private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
                ReportDocument Reporte = new ReportDocument();
                String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
                Reporte.Load(File_Path);
                Ds_Reporte = Data_Set_Consulta_DB;
                Reporte.SetDataSource(Ds_Reporte);
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Bienes.pdf");
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Export_Options);
                String Ruta = "../../Reporte/Rpt_Pat_Listado_Bienes.pdf";
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }

        #endregion

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Bienes_PageIndexChanging
        ///DESCRIPCIÓN: Llena el Grid de Listado de Bienes con la pagina seleccionada
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 12/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Bienes_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Bienes.PageIndex = e.NewPageIndex;
            Ejecutar_Listado("VISUALIZAR");
        }

    #endregion

    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Datos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Datos Generales.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Limpiar_Filtros_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
            Limpiar_Datos_Generales();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Datos_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
        ///             Datos Generales.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 19/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Buscar_Datos_Click(object sender, ImageClickEventArgs e) {
            Session["FILTRO"] = "GENERALES";
            Ejecutar_Listado("VISUALIZAR");
        }
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Datos_Click
        ///DESCRIPCIÓN: Lanza el Reporte Generado por los datos generales.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_Datos_Click(object sender, ImageClickEventArgs e) {
            Session["FILTRO"] = "GENERALES";
            Ejecutar_Listado("PDF");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Resguardante_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
        ///             Reguardante
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 19/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
            Session["FILTRO"] = "RESGUARDANTES";
            Ejecutar_Listado("VISUALIZAR");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Resguardante_Click
        ///DESCRIPCIÓN: Lanza el Reporte Generado por los datos del Reguardante
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 13/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_Resguardante_Click(object sender, ImageClickEventArgs e) {
            Session["FILTRO"] = "RESGUARDANTES";
            Ejecutar_Listado("PDF");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Buscar_Resguardante_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda por parte de los Listados.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 20/Noviembre/2010 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Btn_Limpiar_Filtros_Buscar_Resguardante_Click(object sender, ImageClickEventArgs e) {
            Limpiar_Datos_Resguardantes();
        }
        
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Resguardantes_Dependencias_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del Combo de Dependencias.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 12/Septiembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Cmb_Resguardantes_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Cmb_Resguardantes_Dependencias.SelectedIndex > 0) {
                    Cls_Ope_Pat_Com_Listado_Bienes_Negocio Combo = new Cls_Ope_Pat_Com_Listado_Bienes_Negocio();
                    Combo.P_Tipo_DataTable = "EMPLEADOS";
                    Combo.P_Unidad_Responsable = Cmb_Resguardantes_Dependencias.SelectedItem.Value.Trim();
                    DataTable Tabla = Combo.Consultar_DataTable();
                    Llenar_Combo_Resguardantes(Tabla);
                } else {
                    Cmb_Nombre_Resguardante.Items.Clear();
                    Cmb_Nombre_Resguardante.Items.Insert(0, new ListItem("<- TODOS ->", ""));
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }



    #endregion

    #region (Control Acceso Pagina)

    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
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
            Botones.Add(Btn_Buscar_Datos);
            Botones.Add(Btn_Buscar_Resguardante);

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
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS_AlternateText(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
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
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
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

}