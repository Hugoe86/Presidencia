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
using Presidencia.Control_Patrimonial_Reporte_Licencias.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.IO;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Empleados.Negocios;

public partial class paginas_predial_Frm_Rpt_Pat_Com_Licencias_Vencidas : System.Web.UI.Page
{
    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se Ejecuta al Cargar la Pagina
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            try {
                Div_Contenedor_Msj_Error.Visible = false;
                if (Cls_Sessiones.No_Empleado == null || Cls_Sessiones.No_Empleado.Trim().Length==0) {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
                }
                if (!IsPostBack) {
                    Llenar_Combo_Dependencias();
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
    #endregion "Page Load"

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Filtros_Busqueda
        ///DESCRIPCIÓN: Limpia los campos del Modal de Busqueda de los Empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Filtros_Busqueda(Boolean Limpiar_Grid) {
            Txt_Busqueda_RFC_Empleado.Text = "";
            Txt_Busqueda_No_Empleado.Text = "";
            Cmb_Busqueda_Dependencias.SelectedIndex = 0;
            Cmb_Busqueda_Nombre_Empleado.DataSource = new DataTable();
            Cmb_Busqueda_Nombre_Empleado.DataBind();
            Cmb_Busqueda_Nombre_Empleado.Items.Insert(0, new ListItem("<-- HACE FALTA SELECCIONAR UNA UNIDAD RESPONSABLE -->", ""));
            Txt_Fecha_Desde.Text = "";
            Txt_Fecha_Hasta.Text = "";
            if (Limpiar_Grid) {
                Grid_Listado_Empleados.DataSource = new DataTable();
                Grid_Listado_Empleados.DataBind();
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Filtros_Busqueda
        ///DESCRIPCIÓN: Llena el Grid de listado de los Empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Listado_Empleados() {
            try {
                Grid_Listado_Empleados.Columns[0].Visible = true;
                Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio();
                if (Txt_Busqueda_RFC_Empleado.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_RFC = Txt_Busqueda_RFC_Empleado.Text.Trim();
                }
                if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();
                }
                if (Cmb_Busqueda_Dependencias.SelectedIndex > 0) {
                    Reporte_Negocio.P_Busqueda_Dependencia_ID = Cmb_Busqueda_Dependencias.SelectedItem.Value;
                }
                if (Cmb_Busqueda_Nombre_Empleado.SelectedIndex > 0) {
                    Reporte_Negocio.P_Busqueda_Empleado_ID = Cmb_Busqueda_Nombre_Empleado.SelectedItem.Value;
                }
                if (Txt_Fecha_Desde.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Desde.Text.Trim());
                    Reporte_Negocio.P_Tomar_Fecha_Inicial = true;
                }
                if (Txt_Fecha_Hasta.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Hasta.Text.Trim());
                    Reporte_Negocio.P_Tomar_Fecha_Final = true;
                }
                if (Cmb_Con_Licencia.SelectedIndex > 0) {
                    Reporte_Negocio.P_Busqueda_Con_Sin_Licencia = Cmb_Con_Licencia.SelectedItem.Value;
                }
                Grid_Listado_Empleados.DataSource = Reporte_Negocio.Consultar_Empleados();
                Grid_Listado_Empleados.DataBind();
                Grid_Listado_Empleados.Columns[0].Visible = false;
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Dependencias
        ///DESCRIPCIÓN: Llena el Combo de Dependencias del Modal de Busqueda.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Dependencias() { 
            try{
                Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio();
                DataTable Dt_Dependencias = Reporte_Negocio.Consultar_Dependecias();
                Cmb_Busqueda_Dependencias.DataSource = Dt_Dependencias;
                Cmb_Busqueda_Dependencias.DataTextField = "NOMBRE";
                Cmb_Busqueda_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Dependencias.DataBind();
                Cmb_Busqueda_Dependencias.Items.Insert(0, new ListItem("<-- TODAS -->", ""));
                Cmb_Busqueda_Dependencia.DataSource = Dt_Dependencias;
                Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
                Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Dependencia.DataBind();
                Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<-- TODAS -->", ""));
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Empleados
        ///DESCRIPCIÓN: Llena el Combo de Empleados del Modal de Busqueda.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combo_Empleados() { 
            try{
                Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio();
                Reporte_Negocio.P_Busqueda_Dependencia_ID = Cmb_Busqueda_Dependencias.SelectedItem.Value;
                Cmb_Busqueda_Nombre_Empleado.DataSource = Reporte_Negocio.Consultar_Empleados();
                Cmb_Busqueda_Nombre_Empleado.DataTextField = "NOMBRE_COMPLETO";
                Cmb_Busqueda_Nombre_Empleado.DataValueField = "EMPLEADO_ID";
                Cmb_Busqueda_Nombre_Empleado.DataBind();
                Cmb_Busqueda_Nombre_Empleado.Items.Insert(0, new ListItem("<-- TODOS -->", ""));
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_PDF
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
        private void Generar_Reporte_PDF(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Com_Licencias_Vencidas.pdf");
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
            String Ruta = "../../Reporte/Rpt_Pat_Com_Licencias_Vencidas.pdf";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte_Excel
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
        public void Generar_Reporte_Excel(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Com_Licencias_Vencidas.xls");
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.Excel;
            Reporte.Export(Export_Options);
            String Ruta = "../../Reporte/Rpt_Pat_Com_Licencias_Vencidas.xls";

            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
        ///DESCRIPCIÓN: Genera los Datos para el Reporte y dependiendo de la Opción hace
        ///             la exportación de los datos.
        ///PARAMETROS:  1. Tipo.    Tipo de Reporte ya se PDF y Excel.
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        public void Generar_Reporte(String Tipo) {
            try { 
                Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Com_Licencias_Vencidas_Negocio();
                if (Txt_Busqueda_RFC_Empleado.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_RFC = Txt_Busqueda_RFC_Empleado.Text.Trim();
                }
                if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();
                }
                if (Cmb_Busqueda_Dependencias.SelectedIndex > 0) {
                    Reporte_Negocio.P_Busqueda_Dependencia_ID = Cmb_Busqueda_Dependencias.SelectedItem.Value;
                }
                if (Cmb_Busqueda_Nombre_Empleado.SelectedIndex > 0) {
                    Reporte_Negocio.P_Busqueda_Empleado_ID = Cmb_Busqueda_Nombre_Empleado.SelectedItem.Value;
                }
                if (Txt_Fecha_Desde.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Desde.Text.Trim());
                    Reporte_Negocio.P_Tomar_Fecha_Inicial = true;
                }
                if (Txt_Fecha_Hasta.Text.Trim().Length > 0) {
                    Reporte_Negocio.P_Busqueda_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Hasta.Text.Trim());
                    Reporte_Negocio.P_Tomar_Fecha_Final = true;
                }
                if (Cmb_Con_Licencia.SelectedIndex > 0) {
                    Reporte_Negocio.P_Busqueda_Con_Sin_Licencia = Cmb_Con_Licencia.SelectedItem.Value;
                }
                DataTable Dt_Datos = Reporte_Negocio.Consultar_Empleados();
                Ds_Pat_Com_Licencias_Vencidas Ds_Reporte = new Ds_Pat_Com_Licencias_Vencidas();
                DataSet Ds_Consulta = new DataSet();
                Dt_Datos.TableName = "DT_EMPLEADOS";
                Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                if (Tipo.Trim().Equals("PDF")) {
                    Generar_Reporte_PDF(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Com_Licencias_Vencidas.rpt");
                } else if (Tipo.Trim().Equals("EXCEL")) {
                    Generar_Reporte_Excel(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Com_Licencias_Vencidas.rpt");
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;            
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARAMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2011 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID)
        {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++)
            {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Busqueda_Empleados_Resguardo
        ///DESCRIPCIÓN: Llena el Grid con los empleados que cumplan el filtro
        ///PROPIEDADES:     
        ///CREO:                 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Busqueda_Empleados_Resguardo()
        {
            Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = true;
            Cls_Ope_Pat_Com_Vehiculos_Negocio Negocio = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            if (Txt_Buscar_Resguardante_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado_Resguardante = Convertir_A_Formato_ID(Convert.ToInt32(Txt_Buscar_Resguardante_No_Empleado.Text.Trim()), 6); }
            if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Resguardante = Txt_Busqueda_RFC.Text.Trim(); }
            if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Resguardante = Txt_Busqueda_Nombre_Empleado.Text.Trim().ToUpper(); }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
            Grid_Busqueda_Empleados_Resguardo.DataSource = Negocio.Consultar_Empleados_Resguardos();
            Grid_Busqueda_Empleados_Resguardo.DataBind();
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = false;
        }

    #endregion "Metodos"

    #region "Grids"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Empleados_PageIndexChanging
        ///DESCRIPCIÓN: Maneja el Cambio de Pagina del Grid de Empleados.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Listado_Empleados.PageIndex = e.NewPageIndex;
                Llenar_Grid_Listado_Empleados();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_PageIndexChanging
    ///DESCRIPCIÓN: Maneja el evento de cambio de Página del GridView de Busqueda
    ///             de empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_Resguardo_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Busqueda_Empleados_Resguardo.PageIndex = e.NewPageIndex;
            Llenar_Grid_Busqueda_Empleados_Resguardo();
            MPE_Resguardante.Show();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged
    ///DESCRIPCIÓN: Maneja el evento de cambio de Selección del GridView de Busqueda
    ///             de empleados.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Busqueda_Empleados_Resguardo.SelectedIndex > (-1))
            {
                String Empleado_Seleccionado_ID = Grid_Busqueda_Empleados_Resguardo.SelectedRow.Cells[1].Text.Trim();
                Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleado_Negocio.P_Empleado_ID = Empleado_Seleccionado_ID.Trim();
                DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                String Dependencia_ID = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim() : null;
                Int32 Index_Combo = (-1);
                if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0) {
                    Cmb_Busqueda_Dependencias.SelectedIndex = Cmb_Busqueda_Dependencias.Items.IndexOf(Cmb_Busqueda_Dependencias.Items.FindByValue(Dependencia_ID));
                    Cmb_Busqueda_Dependencias_SelectedIndexChanged(Cmb_Busqueda_Dependencias, null);
                    Cmb_Busqueda_Nombre_Empleado.SelectedIndex = Cmb_Busqueda_Nombre_Empleado.Items.IndexOf(Cmb_Busqueda_Nombre_Empleado.Items.FindByValue(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim()));
                }
                MPE_Resguardante.Hide();
                Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Filtros_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
        ///             para la busqueda.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Limpiar_Filtros_Click(object sender, ImageClickEventArgs e) {
            try {
                Limpiar_Filtros_Busqueda(false);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Listado_Click
        ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda con los filtros
        ///             para la busqueda.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Listado_Click(object sender, ImageClickEventArgs e) {
            try {
                Grid_Listado_Empleados.PageIndex = 0;
                Llenar_Grid_Listado_Empleados();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cmb_Busqueda_Dependencias_SelectedIndexChanged
        ///DESCRIPCIÓN: Maneja el Evento del Cambio de Seleccion para el los Combos de
        ///             Dependencias.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Cmb_Busqueda_Dependencias_SelectedIndexChanged(object sender, EventArgs e) {
            if (Cmb_Busqueda_Dependencias.SelectedIndex > 0) {
                Llenar_Combo_Empleados();
            } else{
                Cmb_Busqueda_Nombre_Empleado.DataSource = new DataTable();
                Cmb_Busqueda_Nombre_Empleado.DataBind();
                Cmb_Busqueda_Nombre_Empleado.Items.Insert(0,new ListItem("<-- HACE FALTA SELECCIONAR UNA UNIDAD RESPONSABLE -->",""));
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Sale del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, ImageClickEventArgs e) {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Genera el Reporte en PDF.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
            Generar_Reporte("PDF");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Excel_Click
        ///DESCRIPCIÓN: Genera el Reporte en Excel.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 14/Julio/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e) {
            Generar_Reporte("EXCEL");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Resguardante_Click
        ///DESCRIPCIÓN: Lanza la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Busqueda_Avanzada_Resguardante_Click(object sender, ImageClickEventArgs e) {
            try {
                MPE_Resguardante.Show();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Empleados_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda Avanzada para el Resguardante.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Busqueda_Empleados_Click(object sender, EventArgs e) {
            try {
                Grid_Busqueda_Empleados_Resguardo.PageIndex = 0;
                Llenar_Grid_Busqueda_Empleados_Resguardo();
                MPE_Resguardante.Show();
            }  catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion "Eventos"

}