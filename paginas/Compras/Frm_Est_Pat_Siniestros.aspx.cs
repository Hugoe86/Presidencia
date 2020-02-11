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
using Presidencia.Control_Patrimonial_Reporte_Siniestros.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_Compras_Frm_Est_Pat_Siniestros : System.Web.UI.Page{

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Metodo de Arranque.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack) {
                Btn_Generar_Reporte_Excel.Visible = false;
            }
        }

    #endregion

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Tabla_Estisticas
        ///DESCRIPCIÓN          : .
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Cargar_Tabla_Estisticas(String Tipo) {
            Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio Siniestros = new Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio();
            if (Txt_Fecha_Inicial.Text.Trim().Length > 0 && (!Txt_Fecha_Inicial.Text.Trim().Equals("__/___/____")))  {
                Siniestros.P_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim());
                Siniestros.P_Tomar_Fecha_Inicial = true;
            }
            if (Txt_Fecha_Final.Text.Trim().Length > 0 && (!Txt_Fecha_Final.Text.Trim().Equals("__/___/____")))  {
                Siniestros.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim());
                Siniestros.P_Tomar_Fecha_Final = true;
            }
            DataTable Dt_Datos = Siniestros.Obtener_Listado_Siniestros();
            Cargar_Dependencia_Siniestros(ref Dt_Datos);
            DataSet Ds_Consulta = new DataSet();
            if (Chk_Responsabilidad.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica_Por_Responsabilidad(Dt_Datos);
                Dt_Tmp.TableName = "DT_POR_RESPONSABILIDAD";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            if (Chk_Unidad_Responsable.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica_Por_Unidad_Responsable(Dt_Datos);
                Dt_Tmp.TableName = "DT_POR_UNIDAD_RESPONSABLE";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            if (Chk_Tipo_Siniestro.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica_Por_Tipo_Siniestro(Dt_Datos);
                Dt_Tmp.TableName = "DT_POR_TIPO_SINIESTRO";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            Ds_Est_Pat_Siniestros Ds_Reporte = new Ds_Est_Pat_Siniestros();
            Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Est_Pat_Siniestros.rpt", Tipo);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Estadistica_Por_Responsabilidad
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private DataTable Obtener_Estadistica_Por_Responsabilidad(DataTable Dt_Datos) {
            DataTable Dt_Estadistica = new DataTable();
            Dt_Estadistica.Columns.Add("DATO", Type.GetType("System.String"));
            Dt_Estadistica.Columns.Add("NO_COINCIDENCIAS", Type.GetType("System.Int32"));
            Dt_Estadistica.Columns.Add("VALOR_PORCENTUAL", Type.GetType("System.Double"));
            Int32 No_Total_Registros = Dt_Datos.Rows.Count;
            DataTable Dt_Diferencias = Dt_Datos.DefaultView.ToTable(true, "MUNICIPIO_RESPONSABLE");
            foreach (DataRow Fila in Dt_Diferencias.Rows) {
                DataRow Fila_Cargar = Dt_Estadistica.NewRow();
                Fila_Cargar["DATO"] = Fila["MUNICIPIO_RESPONSABLE"].ToString().Trim();
                Int32 Parcialidad = Dt_Datos.Select("MUNICIPIO_RESPONSABLE = '" + Fila["MUNICIPIO_RESPONSABLE"].ToString().Trim() + "'").Length;
                Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                Dt_Estadistica.Rows.Add(Fila_Cargar);
            }
            return Dt_Estadistica;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Estadistica_Por_Unidad_Responsable
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private DataTable Obtener_Estadistica_Por_Unidad_Responsable(DataTable Dt_Datos) {
            DataTable Dt_Estadistica = new DataTable();
            Dt_Estadistica.Columns.Add("DATO", Type.GetType("System.String"));
            Dt_Estadistica.Columns.Add("NO_COINCIDENCIAS", Type.GetType("System.Int32"));
            Dt_Estadistica.Columns.Add("VALOR_PORCENTUAL", Type.GetType("System.Double"));
            Int32 No_Total_Registros = Dt_Datos.Rows.Count;
            DataTable Dt_Diferencias = Dt_Datos.DefaultView.ToTable(true, "UNIDAD_RESPONSABLE_ID");
            Dt_Diferencias.DefaultView.Sort = "UNIDAD_RESPONSABLE_ID";
            foreach (DataRow Fila in Dt_Diferencias.Rows) {
                DataRow Fila_Cargar = Dt_Estadistica.NewRow();
                if (!String.IsNullOrEmpty(Fila["UNIDAD_RESPONSABLE_ID"].ToString())) {
                    Cls_Cat_Dependencias_Negocio Negocio = new Cls_Cat_Dependencias_Negocio();
                    Negocio.P_Dependencia_ID = Fila["UNIDAD_RESPONSABLE_ID"].ToString().Trim();
                    DataTable Dt_Temporal = Negocio.Consulta_Dependencias();
                    Fila_Cargar["DATO"] = Dt_Temporal.Rows[0][Cat_Dependencias.Campo_Clave].ToString() + " - " + Dt_Temporal.Rows[0][Cat_Dependencias.Campo_Nombre].ToString();
                    Int32 Parcialidad = Dt_Datos.Select("UNIDAD_RESPONSABLE_ID = '" + Fila["UNIDAD_RESPONSABLE_ID"].ToString().Trim() + "'").Length;
                    Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                    Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                } else {
                    Fila_Cargar["DATO"] = " ---- ";
                    Int32 Parcialidad = Dt_Datos.Select("UNIDAD_RESPONSABLE_ID IS NULL").Length;
                    Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                    Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                }
                Dt_Estadistica.Rows.Add(Fila_Cargar);
            }
            return Dt_Estadistica;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Estadistica_Por_Tipo_Siniestro
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private DataTable Obtener_Estadistica_Por_Tipo_Siniestro(DataTable Dt_Datos) {
            DataTable Dt_Estadistica = new DataTable();
            Dt_Estadistica.Columns.Add("DATO", Type.GetType("System.String"));
            Dt_Estadistica.Columns.Add("NO_COINCIDENCIAS", Type.GetType("System.Int32"));
            Dt_Estadistica.Columns.Add("VALOR_PORCENTUAL", Type.GetType("System.Double"));
            Int32 No_Total_Registros = Dt_Datos.Rows.Count;
            DataTable Dt_Diferencias = Dt_Datos.DefaultView.ToTable(true, "TIPO_SINIESTRO");
            foreach (DataRow Fila in Dt_Diferencias.Rows) {
                DataRow Fila_Cargar = Dt_Estadistica.NewRow();
                Fila_Cargar["DATO"] = Fila["TIPO_SINIESTRO"].ToString().Trim();
                Int32 Parcialidad = Dt_Datos.Select("TIPO_SINIESTRO = '" + Fila["TIPO_SINIESTRO"].ToString().Trim() + "'").Length;
                Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                Dt_Estadistica.Rows.Add(Fila_Cargar);
            }
            return Dt_Estadistica;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Cargar_Dependencia_Siniestros
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Cargar_Dependencia_Siniestros(ref DataTable Dt_Datos) {
            Dt_Datos.Columns.Add("UNIDAD_RESPONSABLE_ID", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("UNIDAD_RESPONSABLE_NOMBRE", Type.GetType("System.String"));
            foreach (DataRow Fila in Dt_Datos.Rows) {
                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculo.P_Vehiculo_ID = Fila["VEHICULO_ID"].ToString().Trim();
                Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                if (!String.IsNullOrEmpty(Vehiculo.P_Dependencia_ID)) {
                    Fila["UNIDAD_RESPONSABLE_ID"] = Vehiculo.P_Dependencia_ID;
                    Cls_Cat_Dependencias_Negocio Dependencia = new Cls_Cat_Dependencias_Negocio();
                    Dependencia.P_Dependencia_ID = Vehiculo.P_Dependencia_ID;
                    DataTable Dt_Dependencias = Dependencia.Consulta_Dependencias();
                    if (Dt_Dependencias != null && Dt_Dependencias.Rows.Count > 0) {
                        String Texto = Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Clave].ToString().Trim() + " - " + Dt_Dependencias.Rows[0][Cat_Dependencias.Campo_Nombre].ToString().Trim();
                        Fila["UNIDAD_RESPONSABLE_NOMBRE"] = Texto;
                    }
                }
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Generar_Reporte
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, String Nombre_Reporte, String Tipo) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            if (Tipo.Equals("PDF")) {
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Est_Pat_Siniestros.pdf");
            } else {
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Est_Pat_Siniestros.xls");
            }
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (!Chk_Responsabilidad.Checked) { 
                Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Siniestros_Por_Responsabilidad"].ObjectFormat.EnableSuppress = true;
            }
            if (!Chk_Tipo_Siniestro.Checked) { Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Siniestros_Por_Tipo_Siniestro"].ObjectFormat.EnableSuppress = true; }
            if (!Chk_Unidad_Responsable.Checked) { Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Siniestros_Por_Unidad_Responsable"].ObjectFormat.EnableSuppress = true; } 
            Reporte.Export(Export_Options);
            String Ruta = "../../Reporte/Rpt_Est_Pat_Siniestros.pdf";
            if (Tipo.Equals("EXCEL")) { Ruta = "../../Reporte/Rpt_Est_Pat_Siniestros.xls"; }
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }

    #endregion

    #region "Eventos"
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN          : 
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
            if (Chk_Responsabilidad.Checked || Chk_Tipo_Siniestro.Checked || Chk_Unidad_Responsable.Checked) {
                Cargar_Tabla_Estisticas("PDF");
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario seleccionar alguno de los tipos de estadistica.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Btn_Generar_Reporte_Excel_Click
        ///DESCRIPCIÓN          : 
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e) {
            if (Chk_Responsabilidad.Checked || Chk_Tipo_Siniestro.Checked || Chk_Unidad_Responsable.Checked) {
                Cargar_Tabla_Estisticas("EXCEL");
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario seleccionar alguno de los tipos de estadistica.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

}