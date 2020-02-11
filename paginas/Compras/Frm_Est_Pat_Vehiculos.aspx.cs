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
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_Compras_Frm_Est_Pat_Vehiculos : System.Web.UI.Page {

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
            Cls_Rpt_Pat_Listado_Bienes_Negocio Vehiculo_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            if (Txt_Fecha_Inicial.Text.Trim().Length > 0 && (!Txt_Fecha_Inicial.Text.Trim().Equals("__/___/____")))  {
                Vehiculo_Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim());
                Vehiculo_Negocio.P_Tomar_Fecha_Inicial = true;
            }
            if (Txt_Fecha_Final.Text.Trim().Length > 0 && (!Txt_Fecha_Final.Text.Trim().Equals("__/___/____")))  {
                Vehiculo_Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim());
                Vehiculo_Negocio.P_Tomar_Fecha_Final = true;
            }
            DataTable Dt_Datos = Vehiculo_Negocio.Consultar_Vehiculos();

            DataSet Ds_Consulta = new DataSet();
            if (Chk_Tipo_Vehiculo.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica_Por_Tipo_Vehiculo(Dt_Datos);
                Dt_Tmp.TableName = "DT_POR_TIPO_VEHICULO";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            if (Chk_Estatus.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica_Por_Estatus(Dt_Datos);
                Dt_Tmp.TableName = "DT_POR_ESTATUS";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            if (Chk_Por_Procedencia.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica_Por_Procedencia(Dt_Datos);
                Dt_Tmp.TableName = "DT_POR_PROCEDENCIA";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            Ds_Est_Pat_Vehiculos Ds_Reporte = new Ds_Est_Pat_Vehiculos();
            Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Est_Pat_Vehiculos.rpt", Tipo);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Estadistica_Por_Tipo_Vehiculo
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private DataTable Obtener_Estadistica_Por_Tipo_Vehiculo(DataTable Dt_Datos) {
            DataTable Dt_Estadistica = new DataTable();
            Dt_Estadistica.Columns.Add("DATO", Type.GetType("System.String"));
            Dt_Estadistica.Columns.Add("NO_COINCIDENCIAS", Type.GetType("System.Int32"));
            Dt_Estadistica.Columns.Add("VALOR_PORCENTUAL", Type.GetType("System.Double"));
            Int32 No_Total_Registros = Dt_Datos.Rows.Count;
            DataTable Dt_Diferencias = Dt_Datos.DefaultView.ToTable(true, "TIPO_VEHICULO");
            foreach (DataRow Fila in Dt_Diferencias.Rows) {
                DataRow Fila_Cargar = Dt_Estadistica.NewRow();
                Fila_Cargar["DATO"] = Fila["TIPO_VEHICULO"].ToString().Trim();
                Int32 Parcialidad = Dt_Datos.Select("TIPO_VEHICULO = '" + Fila["TIPO_VEHICULO"].ToString().Trim() + "'").Length;
                Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                Dt_Estadistica.Rows.Add(Fila_Cargar);
            }
            return Dt_Estadistica;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Estadistica_Por_Estatus
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private DataTable Obtener_Estadistica_Por_Estatus(DataTable Dt_Datos) {
            DataTable Dt_Estadistica = new DataTable();
            Dt_Estadistica.Columns.Add("DATO", Type.GetType("System.String"));
            Dt_Estadistica.Columns.Add("NO_COINCIDENCIAS", Type.GetType("System.Int32"));
            Dt_Estadistica.Columns.Add("VALOR_PORCENTUAL", Type.GetType("System.Double"));
            Int32 No_Total_Registros = Dt_Datos.Rows.Count;
            DataTable Dt_Diferencias = Dt_Datos.DefaultView.ToTable(true, "ESTATUS");
            foreach (DataRow Fila in Dt_Diferencias.Rows)
            {
                DataRow Fila_Cargar = Dt_Estadistica.NewRow();
                Fila_Cargar["DATO"] = Fila["ESTATUS"].ToString().Trim();
                Int32 Parcialidad = Dt_Datos.Select("ESTATUS = '" + Fila["ESTATUS"].ToString().Trim() + "'").Length;
                Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                Dt_Estadistica.Rows.Add(Fila_Cargar);
            }
            return Dt_Estadistica;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Estadistica_Por_Procedencia
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : 27/Febrero/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private DataTable Obtener_Estadistica_Por_Procedencia(DataTable Dt_Datos) {
            DataTable Dt_Estadistica = new DataTable();
            Dt_Estadistica.Columns.Add("DATO", Type.GetType("System.String"));
            Dt_Estadistica.Columns.Add("NO_COINCIDENCIAS", Type.GetType("System.Int32"));
            Dt_Estadistica.Columns.Add("VALOR_PORCENTUAL", Type.GetType("System.Double"));
            Int32 No_Total_Registros = Dt_Datos.Rows.Count;
            DataTable Dt_Diferencias = Dt_Datos.DefaultView.ToTable(true, "PROCEDENCIA");
            foreach (DataRow Fila in Dt_Diferencias.Rows) {
                DataRow Fila_Cargar = Dt_Estadistica.NewRow();
                Fila_Cargar["DATO"] = Fila["PROCEDENCIA"].ToString().Trim();
                Int32 Parcialidad = Dt_Datos.Select("PROCEDENCIA = '" + Fila["PROCEDENCIA"].ToString().Trim() + "'").Length;
                Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                Dt_Estadistica.Rows.Add(Fila_Cargar);
            }
            return Dt_Estadistica;
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
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Est_Pat_Vehiculos.pdf");
            } else {
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Est_Pat_Vehiculos.xls");
            }
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (!Chk_Tipo_Vehiculo.Checked) { Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Vehiculos_Tipo_Vehiculo"].ObjectFormat.EnableSuppress = true; }
            if (!Chk_Estatus.Checked) { Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Vehiculos_Estatus"].ObjectFormat.EnableSuppress = true; }
            if (!Chk_Por_Procedencia.Checked) { Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Vehiculos_Procedencia"].ObjectFormat.EnableSuppress = true; } 
            Reporte.Export(Export_Options);
            String Ruta = "../../Reporte/Rpt_Est_Pat_Vehiculos.pdf";
            if (Tipo.Equals("EXCEL")) { Ruta = "../../Reporte/Rpt_Est_Pat_Vehiculos.xls"; }
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
            if (Chk_Tipo_Vehiculo.Checked || Chk_Estatus.Checked || Chk_Por_Procedencia.Checked) {
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
            if (Chk_Tipo_Vehiculo.Checked || Chk_Estatus.Checked || Chk_Por_Procedencia.Checked) {
                Cargar_Tabla_Estisticas("EXCEL");
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario seleccionar alguno de los tipos de estadistica.";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion
}