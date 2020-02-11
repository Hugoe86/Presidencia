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
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Reporte_Movimientos.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_Compras_Frm_Rpt_Pat_Listado_Movimientos : System.Web.UI.Page
{
    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se Ejecuta al Cargar la Pagina
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 16/Julio/2011
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
        private void Limpiar_Filtros_Busqueda() {
            Chk_Altas.Checked = false;
            Chk_Modificaciones.Checked = false;
            Chk_Bajas.Checked = false;
            Txt_Fecha_Adquisicion_Inicial.Text = "";
            Txt_Fecha_Adquisicion_Final.Text = "";
            Cmb_Busqueda_Dependencias.SelectedIndex = 0;
            Cmb_Busqueda_Nombre_Empleado.DataSource = new DataTable();
            Cmb_Busqueda_Nombre_Empleado.DataBind();
            Cmb_Busqueda_Nombre_Empleado.Items.Insert(0, new ListItem("<-- HACE FALTA SELECCIONAR UNA UNIDAD RESPONSABLE -->", ""));
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
                Cls_Rpt_Pat_Listado_Movimientos_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Listado_Movimientos_Negocio();
                Cmb_Busqueda_Dependencias.DataSource = Reporte_Negocio.Consultar_Dependecias();
                Cmb_Busqueda_Dependencias.DataTextField = "NOMBRE";
                Cmb_Busqueda_Dependencias.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Dependencias.DataBind();
                Cmb_Busqueda_Dependencias.Items.Insert(0, new ListItem("<-- TODAS -->", ""));
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
                Cls_Rpt_Pat_Listado_Movimientos_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Listado_Movimientos_Negocio();
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
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Movimientos.pdf");
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
            String Ruta = "../../Reporte/Rpt_Pat_Listado_Movimientos.pdf";
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
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Movimientos.xls");
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.Excel;
            Reporte.Export(Export_Options);
            String Ruta = "../../Reporte/Rpt_Pat_Listado_Movimientos.xls";

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
                if (Chk_Altas.Checked || Chk_Modificaciones.Checked || Chk_Modificaciones.Checked) {
                    Cls_Rpt_Pat_Listado_Movimientos_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Listado_Movimientos_Negocio();
                    if (Chk_Altas.Checked) { Reporte_Negocio.P_Altas = true; }
                    if (Chk_Modificaciones.Checked) { Reporte_Negocio.P_Modificaciones = true; }
                    if (Chk_Bajas.Checked) { Reporte_Negocio.P_Bajas = true; }
                    if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) {
                        Reporte_Negocio.P_Busqueda_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text.Trim());
                        Reporte_Negocio.P_Tomar_Fecha_Inicial = true;
                    }
                    if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) {
                        Reporte_Negocio.P_Busqueda_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text.Trim());
                        Reporte_Negocio.P_Tomar_Fecha_Final = true;
                    }
                    if (Cmb_Busqueda_Dependencias.SelectedIndex > 0) {
                        Reporte_Negocio.P_Busqueda_Dependencia_ID = Cmb_Busqueda_Dependencias.SelectedItem.Value;
                    }
                    if (Cmb_Busqueda_Nombre_Empleado.SelectedIndex > 0) {
                        Reporte_Negocio.P_Busqueda_Empleado_ID = Cmb_Busqueda_Nombre_Empleado.SelectedItem.Value;
                    }
                    DataTable Dt_Datos = Reporte_Negocio.Consultar_Registros_Reporte();
                    Ds_Pat_Listado_Movimientos Ds_Reporte = new Ds_Pat_Listado_Movimientos();
                    DataSet Ds_Consulta = new DataSet();
                    Dt_Datos.TableName = "DT_REGISTROS";
                    Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                    if (Tipo.Trim().Equals("PDF")) {
                        Generar_Reporte_PDF(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Movimientos.rpt");
                    } else if (Tipo.Trim().Equals("EXCEL")) {
                        Generar_Reporte_Excel(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Movimientos.rpt");
                    }
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                    Lbl_Mensaje_Error.Text = "Seleccionar una Opción Principal como minimo [Altas, Actualizaciones o Bajas].";
                    Div_Contenedor_Msj_Error.Visible = true;           
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;            
            }
        }

    #endregion "Metodos"

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
                Limpiar_Filtros_Busqueda();
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

    #endregion "Eventos"
}
