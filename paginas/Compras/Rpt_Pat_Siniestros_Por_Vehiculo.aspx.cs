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
using Presidencia.Control_Patrimonial_Reporte_Siniestros.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Control_Patrimonial_Operacion_Siniestros.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Negocio;

public partial class paginas_Compras_Rpt_Pat_Siniestros_Por_Vehiculo : System.Web.UI.Page {

#region  Page Load
    
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo Inicial de la página.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 06/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e) {
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        Div_Contenedor_Msj_Error.Visible = false;
        try {
            if (!IsPostBack) {
                Llenar_Combos_Independientes();
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

#endregion

#region Metodos

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
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte)
    {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set_Consulta_DB;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Siniestros.pdf");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Rpt_Pat_Siniestros.pdf";
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
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
    public void Exportar_Excel(DataSet Data_Set, DataSet Ds_Reporte, String Nombre_Reporte)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Ds_Reporte = Data_Set;
        Reporte.SetDataSource(Ds_Reporte);
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Siniestros.xls");
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.Excel;
        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Rpt_Pat_Siniestros.xls";

        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Independientes
    ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:  06/Mayo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combos_Independientes() {
        try {
            Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();

            //SE LLENA EL COMBO DE DEPENDENCIAS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "DEPENDENCIAS";
            DataTable Dependencias = Combos.Consultar_DataTable();
            Cmb_Dependencia.DataSource = Dependencias;
            Cmb_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencia.DataTextField = "NOMBRE";
            Cmb_Dependencia.DataBind();
            Cmb_Dependencia.Items.Insert(0, new ListItem("< TODOS >", ""));
            Cmb_Busqueda_Dependencia.DataSource = Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("< TODOS >", ""));

            //SE LLENA EL COMBO DE TIPO DE VEHICULOS DE LAS BUSQUEDAS
            Combos.P_Tipo_DataTable = "TIPOS_VEHICULOS";
            DataTable Tipos_Vehiculos = Combos.Consultar_DataTable();
            Cmb_Tipos_Vehiculo.DataSource = Tipos_Vehiculos;
            Cmb_Tipos_Vehiculo.DataTextField = "DESCRIPCION";
            Cmb_Tipos_Vehiculo.DataValueField = "TIPO_VEHICULO_ID";
            Cmb_Tipos_Vehiculo.DataBind();
            Cmb_Tipos_Vehiculo.Items.Insert(0, new ListItem("< TODOS >", ""));

            Llenar_Combo_Tipos_Siniestro();
            Llenar_Combo_Aseguradora();

        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
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
        if (Txt_Busqueda_No_Empleado.Text.Trim().Length > 0) { Negocio.P_No_Empleado_Resguardante = Convertir_A_Formato_ID(Convert.ToInt32(Txt_Busqueda_No_Empleado.Text.Trim()), 6); }
        if (Txt_Busqueda_RFC.Text.Trim().Length > 0) { Negocio.P_RFC_Resguardante = Txt_Busqueda_RFC.Text.Trim(); }
        if (Txt_Busqueda_Nombre_Empleado.Text.Trim().Length > 0) { Negocio.P_Nombre_Resguardante = Txt_Busqueda_Nombre_Empleado.Text.Trim().ToUpper(); }
        if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedItem.Value; }
        Grid_Busqueda_Empleados_Resguardo.DataSource = Negocio.Consultar_Empleados_Resguardos();
        Grid_Busqueda_Empleados_Resguardo.DataBind();
        Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Siniestro
    ///DESCRIPCIÓN: Llena el Combo de Tipos de Siniestros
    ///PARAMETROS:     
    ///CREO : Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: 17/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private void Llenar_Combo_Tipos_Siniestro()
    {
        try
        {
            Cls_Ope_Pat_Com_Siniestros_Negocio Negocio = new Cls_Ope_Pat_Com_Siniestros_Negocio();
            Negocio.P_Tipo_DataTable = "TIPOS_SINIESTROS";
            DataTable Tipos_Siniestros = Negocio.Consultar_DataTable();
            Cmb_Tipo_Siniestros.DataSource = Tipos_Siniestros;
            Cmb_Tipo_Siniestros.DataValueField = "SINIESTRO_ID";
            Cmb_Tipo_Siniestros.DataTextField = "DESCRIPCION";
            Cmb_Tipo_Siniestros.DataBind();
            Cmb_Tipo_Siniestros.Items.Insert(0, new ListItem("< TODOS >", ""));
            
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Aseguradora
    ///DESCRIPCIÓN: Llena el Combo de Aseguradora
    ///PARAMETROS:     
    ///CREO : Francisco Antonio Gallardo Castañeda
    ///FECHA_CREO: 17/Enero/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN 
    ///*******************************************************************************
    private void Llenar_Combo_Aseguradora()  {
        try {
            Cls_Cat_Pat_Com_Aseguradoras_Negocio Negocio = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
            Negocio.P_Tipo_DataTable = "ASEGURADORAS";
            DataTable Aseguradora = Negocio.Consultar_DataTable();
            Cmb_Aseguradora.DataSource = Aseguradora;
            Cmb_Aseguradora.DataValueField = "ASEGURADORA_ID";
            Cmb_Aseguradora.DataTextField = "NOMBRE";
            Cmb_Aseguradora.DataBind();
            Cmb_Aseguradora.Items.Insert(0, new ListItem("< TODAS >", ""));
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Filtros_Reporte
    ///DESCRIPCIÓN: Carga los Filtros.
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 12/Dic/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Filtros_Reporte() {
        DataTable Dt_Filtros = new DataTable();
        Dt_Filtros.Columns.Add("Filtro", Type.GetType("System.String"));
        Dt_Filtros.Columns.Add("Valor", Type.GetType("System.String"));
        if (Cmb_Tipo_Siniestros.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Tipo de Siniestro:", Cmb_Tipo_Siniestros.SelectedItem.Text); }
        if (Cmb_Reparacion.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Reparación:", Cmb_Reparacion.SelectedItem.Text); }
        if (Cmb_Estatus.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Estatus:", Cmb_Estatus.SelectedItem.Text); }
        if (Cmb_Mpio_Responsable.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Responsabilidad de Mpio:", Cmb_Mpio_Responsable.SelectedItem.Text); }
        if (Cmb_Consignado.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Consignado:", Cmb_Consignado.SelectedItem.Text); }
        if (Cmb_Pago_Danio_Sindicos.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Pago a Sindicos [Daños]:", Cmb_Pago_Danio_Sindicos.SelectedItem.Text); }
        if (Txt_No_Inventario.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "No. Inventario Vehiculo:", Txt_No_Inventario.Text.Trim()); }
        if (Cmb_Tipos_Vehiculo.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Tipo de Vehículo:", Cmb_Tipos_Vehiculo.SelectedItem.Text); }
        if (Cmb_Dependencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Unidad Responsable", Cmb_Dependencia.SelectedItem.Text); }
        if (Cmb_Aseguradora.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Aseguradora:", Cmb_Aseguradora.SelectedItem.Text); }
        if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Resguardante:", Txt_Resguardante.Text.Trim()); }
        if (Txt_Fecha_Inicial.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Fecha Rango [Inicio]:", Txt_Fecha_Inicial.Text.Trim()); }
        if (Txt_Fecha_Final.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Fecha Rango [Fin]:", Txt_Fecha_Final.Text.Trim()); }
        if (Chk_Reporte_Completo.Checked) { Cargar_Filtro(Dt_Filtros, "TIPO DE REPORTE:", "REPORTE COMPLETO"); } else { Cargar_Filtro(Dt_Filtros, "TIPO DE REPORTE:", "REPORTE INCOMPLETO"); }
        return Dt_Filtros;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Filtro
    ///DESCRIPCIÓN: Carga los Filtros.
    ///PARAMETROS: 
    ///CREO: Francisco A. Gallardo Castañeda.
    ///FECHA_CREO: 09/Dic/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Cargar_Filtro(DataTable Dt_Datos, String Filtro, String Valor)
    {
        if (Dt_Datos != null)
        {
            DataRow Fila = Dt_Datos.NewRow();
            Fila["Filtro"] = Filtro;
            Fila["Valor"] = Valor;
            Dt_Datos.Rows.Add(Fila);
        }
        return Dt_Datos;
    }

#endregion

#region "Grids"

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
                Hdf_Resguardante_ID.Value = "";
                Txt_Resguardante.Text = "";
                String Empleado_Seleccionado_ID = Grid_Busqueda_Empleados_Resguardo.SelectedRow.Cells[1].Text.Trim();
                Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                Empleado_Negocio.P_Empleado_ID = Empleado_Seleccionado_ID.Trim();
                DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                String Dependencia_ID = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim() : null;
                Int32 Index_Combo = (-1);
                if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0)
                {
                    Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(Dependencia_ID));
                    Hdf_Resguardante_ID.Value = ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim() : null);
                    Txt_Resguardante.Text += ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim() : null);
                    Txt_Resguardante.Text += " - " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString().Trim() : null);
                    Txt_Resguardante.Text = Txt_Resguardante.Text.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString().Trim() : null);
                    Txt_Resguardante.Text = Txt_Resguardante.Text.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString().Trim() : null);
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

#region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
    ///DESCRIPCIÓN: Manda llamar los datos para cergarlos en el reporte dependiendo de
    ///             los filtros seleccionados.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
        try {
            Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio Negocio_Reporte = new Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio();
            if (Cmb_Tipo_Siniestros.SelectedIndex > 0) { Negocio_Reporte.P_Tipo_Siniestro = Cmb_Tipo_Siniestros.SelectedItem.Value; }
            if (Cmb_Reparacion.SelectedIndex > 0) { Negocio_Reporte.P_Reparacion = Cmb_Reparacion.SelectedItem.Value; }
            if (Cmb_Estatus.SelectedIndex > 0) { Negocio_Reporte.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
            if (Cmb_Mpio_Responsable.SelectedIndex > 0) { Negocio_Reporte.P_Mpio_Responsable = Cmb_Mpio_Responsable.SelectedItem.Value; }
            if (Cmb_Consignado.SelectedIndex > 0) { Negocio_Reporte.P_Consignado = Cmb_Consignado.SelectedItem.Value; }
            if (Cmb_Pago_Danio_Sindicos.SelectedIndex > 0) { Negocio_Reporte.P_Pago_Sindicos = Cmb_Pago_Danio_Sindicos.SelectedItem.Value; }
            if (Txt_No_Inventario.Text.Trim().Length > 0) { Negocio_Reporte.P_No_Inventario = Txt_No_Inventario.Text.Trim(); }
            if (Cmb_Tipos_Vehiculo.SelectedIndex > 0) { Negocio_Reporte.P_Tipo_Vehiculo = Cmb_Tipos_Vehiculo.SelectedItem.Value; }
            if (Cmb_Dependencia.SelectedIndex > 0) { Negocio_Reporte.P_Dependencia = Cmb_Dependencia.SelectedItem.Value; }
            if (Cmb_Aseguradora.SelectedIndex > 0) { Negocio_Reporte.P_Aseguradora = Cmb_Aseguradora.SelectedItem.Value; }
            if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Negocio_Reporte.P_Resguardante = Hdf_Resguardante_ID.Value.Trim(); }
            if (Txt_Fecha_Inicial.Text.Trim().Length > 0) {
                Negocio_Reporte.P_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim());
                Negocio_Reporte.P_Tomar_Fecha_Inicial = true;
            }
            if (Txt_Fecha_Final.Text.Trim().Length > 0) {
                Negocio_Reporte.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim());
                Negocio_Reporte.P_Tomar_Fecha_Final = true;
            }
            String Reporte = "Rpt_Pat_Siniestros_InCompleto.rpt";
            DataTable Dt_Generales = Negocio_Reporte.Obtener_Listado_Siniestros();
            if (Dt_Generales != null && Dt_Generales.Rows.Count > 0) {
                Dt_Generales.TableName = "PRINCIPAL";
                DataTable Dt_Filtros = Filtros_Reporte();
                Dt_Filtros.TableName = "FILTROS";
                DataSet Ds_Reporte = new DataSet();
                Ds_Reporte.Tables.Add(Dt_Generales.Copy());
                Ds_Reporte.Tables.Add(Dt_Filtros.Copy());
                if (Chk_Reporte_Completo.Checked) {
                    DataTable Dt_Observaciones = Negocio_Reporte.Obtener_Listado_Siniestros_Observaciones();
                    Dt_Observaciones.TableName = "OBSERVACIONES";
                    Reporte = "Rpt_Pat_Siniestros_Completo.rpt";
                    Ds_Reporte.Tables.Add(Dt_Observaciones.Copy());
                }else{
                    DataTable Dt_Observaciones = new DataTable();
                    Dt_Observaciones.TableName = "OBSERVACIONES";
                    Ds_Reporte.Tables.Add(Dt_Observaciones.Copy());
                }
                Ds_Pat_Siniestros_Por_Vehiculo Ds_Siniestros = new Ds_Pat_Siniestros_Por_Vehiculo();
                Generar_Reporte(Ds_Reporte, Ds_Siniestros, Reporte);
            } else {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('No hay registros para generar el reporte');", true);
            }
        } catch (Exception Ex) {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Excel_Click
    ///DESCRIPCIÓN: Manda llamar los datos para cergarlos en el reporte dependiendo de
    ///             los filtros seleccionados.
    ///PARAMETROS:  
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO:  30/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try {
            Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio Negocio_Reporte = new Cls_Rpt_Pat_Siniestros_Por_Vehiculo_Negocio();
            if (Cmb_Tipo_Siniestros.SelectedIndex > 0) { Negocio_Reporte.P_Tipo_Siniestro = Cmb_Tipo_Siniestros.SelectedItem.Value; }
            if (Cmb_Reparacion.SelectedIndex > 0) { Negocio_Reporte.P_Reparacion = Cmb_Reparacion.SelectedItem.Value; }
            if (Cmb_Estatus.SelectedIndex > 0) { Negocio_Reporte.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
            if (Cmb_Mpio_Responsable.SelectedIndex > 0) { Negocio_Reporte.P_Mpio_Responsable = Cmb_Mpio_Responsable.SelectedItem.Value; }
            if (Cmb_Consignado.SelectedIndex > 0) { Negocio_Reporte.P_Consignado = Cmb_Consignado.SelectedItem.Value; }
            if (Cmb_Pago_Danio_Sindicos.SelectedIndex > 0) { Negocio_Reporte.P_Pago_Sindicos = Cmb_Pago_Danio_Sindicos.SelectedItem.Value; }
            if (Txt_No_Inventario.Text.Trim().Length > 0) { Negocio_Reporte.P_No_Inventario = Txt_No_Inventario.Text.Trim(); }
            if (Cmb_Tipos_Vehiculo.SelectedIndex > 0) { Negocio_Reporte.P_Tipo_Vehiculo = Cmb_Tipos_Vehiculo.SelectedItem.Value; }
            if (Cmb_Dependencia.SelectedIndex > 0) { Negocio_Reporte.P_Dependencia = Cmb_Dependencia.SelectedItem.Value; }
            if (Cmb_Aseguradora.SelectedIndex > 0) { Negocio_Reporte.P_Aseguradora = Cmb_Aseguradora.SelectedItem.Value; }
            if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Negocio_Reporte.P_Resguardante = Hdf_Resguardante_ID.Value.Trim(); }
            if (Txt_Fecha_Inicial.Text.Trim().Length > 0) {
                Negocio_Reporte.P_Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim());
                Negocio_Reporte.P_Tomar_Fecha_Inicial = true;
            }
            if (Txt_Fecha_Final.Text.Trim().Length > 0) {
                Negocio_Reporte.P_Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text.Trim());
                Negocio_Reporte.P_Tomar_Fecha_Final = true;
            }
            String Reporte = "Rpt_Pat_Siniestros_Incompleto.rpt";
            DataTable Dt_Generales = Negocio_Reporte.Obtener_Listado_Siniestros();
            if (Dt_Generales != null && Dt_Generales.Rows.Count > 0) {
                Dt_Generales.TableName = "PRINCIPAL";
                DataTable Dt_Filtros = Filtros_Reporte();
                Dt_Filtros.TableName = "FILTROS";
                DataSet Ds_Reporte = new DataSet();
                Ds_Reporte.Tables.Add(Dt_Generales.Copy());
                Ds_Reporte.Tables.Add(Dt_Filtros.Copy());
                if (Chk_Reporte_Completo.Checked) {
                    DataTable Dt_Observaciones = Negocio_Reporte.Obtener_Listado_Siniestros_Observaciones();
                    Dt_Observaciones.TableName = "OBSERVACIONES";
                    Reporte = "Rpt_Pat_Siniestros_Completo.rpt";
                    Ds_Reporte.Tables.Add(Dt_Observaciones.Copy());
                }
                Ds_Pat_Siniestros_Por_Vehiculo Ds_Siniestros = new Ds_Pat_Siniestros_Por_Vehiculo();
                Exportar_Excel(Ds_Reporte, Ds_Siniestros, Reporte);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "a", "alert('No hay registros para generar el reporte');", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "Error:";
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Div_Contenedor_Msj_Error.Visible = true;
        }
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

#endregion

}
