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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Text;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Constantes;
using Presidencia.Catalogo_Compras_Proveedores.Negocio;

public partial class paginas_Compras_Frm_Rpt_Pat_Listado_Vehiculos : System.Web.UI.Page {

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se carga cuando la Pagina de Inicia.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Febrero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Combos_Formulario();
                Lbl_Proveedor.Visible = false;
                Cmb_Proveedor.Visible = false;
            }
        }

    #endregion

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Formulario
        ///DESCRIPCIÓN: Llena los Combos del Formulario
        ///PROPIEDADES:     
        ///CREO:                 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Combos_Formulario() {
            Cls_Ope_Pat_Com_Vehiculos_Negocio Combos = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
            Combos.P_Tipo_DataTable = "DEPENDENCIAS";
            DataTable Temporal = Combos.Consultar_DataTable();
            Cmb_Dependencia.DataSource = Temporal;
            Cmb_Dependencia.DataTextField = "NOMBRE";
            Cmb_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencia.DataBind();
            Cmb_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));
            Cmb_Busqueda_Dependencia.DataSource = Temporal;
            Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
            Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));

            Combos.P_Tipo_DataTable = "ZONAS";
            Temporal = Combos.Consultar_DataTable();
            Cmb_Zona.DataSource = Temporal;
            Cmb_Zona.DataTextField = "DESCRIPCION";
            Cmb_Zona.DataValueField = "ZONA_ID";
            Cmb_Zona.DataBind();
            Cmb_Zona.Items.Insert(0, new ListItem("< TODAS >", ""));

            Combos.P_Tipo_DataTable = "TIPOS_VEHICULOS";
            Temporal = Combos.Consultar_DataTable();
            Cmb_Tipo_Vehiculo.DataSource = Temporal;
            Cmb_Tipo_Vehiculo.DataTextField = "DESCRIPCION";
            Cmb_Tipo_Vehiculo.DataValueField = "TIPO_VEHICULO_ID";
            Cmb_Tipo_Vehiculo.DataBind();
            Cmb_Tipo_Vehiculo.Items.Insert(0, new ListItem("< TODOS >", ""));

            Combos.P_Tipo_DataTable = "ASEGURADORAS";
            Temporal = Combos.Consultar_DataTable();
            Cmb_Aseguradora.DataSource = Temporal;
            Cmb_Aseguradora.DataTextField = "NOMBRE";
            Cmb_Aseguradora.DataValueField = "ASEGURADORA_ID";
            Cmb_Aseguradora.DataBind();
            Cmb_Aseguradora.Items.Insert(0, new ListItem("< TODAS >", ""));

            //Cls_Cat_Com_Proveedores_Negocio Proveedores = new Cls_Cat_Com_Proveedores_Negocio();
            //Cmb_Proveedor.DataSource = Proveedores.Consulta_Datos_Proveedores();
            //Cmb_Proveedor.DataTextField = Cat_Com_Proveedores.Campo_Nombre;
            //Cmb_Proveedor.DataValueField = Cat_Com_Proveedores.Campo_Proveedor_ID;
            //Cmb_Proveedor.DataBind();
            Cmb_Proveedor.Items.Insert(0, new ListItem("< TODOS >", ""));
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
    
        #region "Reporte"

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
                if (Cmb_Dependencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Unidad Responsable:", Cmb_Dependencia.SelectedItem.Text.Trim()); }
                if (Cmb_Zona.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Zona:", Cmb_Zona.SelectedItem.Text.Trim()); }
                if (Cmb_Tipo_Vehiculo.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Tipo de Vehículo:", Cmb_Tipo_Vehiculo.SelectedItem.Text.Trim()); }
                if (Cmb_Estatus.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Tipo de Vehículo:", Cmb_Tipo_Vehiculo.SelectedItem.Text.Trim()); }
                if (Cmb_Aseguradora.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Aseguradora:", Cmb_Aseguradora.SelectedItem.Text.Trim()); }
                if (Cmb_Proveedor.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Proveedor:", Cmb_Proveedor.SelectedItem.Text.Trim()); }
                if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Fecha Adquisición [Inicio]:", Txt_Fecha_Adquisicion_Inicial.Text.Trim()); }
                if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Fecha Adquisición [Fin]:", Txt_Fecha_Adquisicion_Final.Text.Trim()); }
                if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Resguardante:", Txt_Resguardante.Text.Trim()); }
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
            private DataTable Cargar_Filtro(DataTable Dt_Datos, String Filtro, String Valor) {
                if (Dt_Datos != null) {
                    DataRow Fila = Dt_Datos.NewRow();
                    Fila["Filtro"] = Filtro;
                    Fila["Valor"] = Valor;
                    Dt_Datos.Rows.Add(Fila);
                }
                return Dt_Datos;
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Crear_Reporte
            ///DESCRIPCIÓN: Se obtienen los datos del reporte.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Febrero/2012
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private void Crear_Reporte(String Tipo_Reporte) {
                if (Tipo_Reporte.Trim().Equals("PDF") || Tipo_Reporte.Trim().Equals("Excel")) {
                    String Filtros = null;
                    Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                    if (Cmb_Dependencia.SelectedIndex > (-1)) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value; }
                    if (Cmb_Zona.SelectedIndex > (-1)) { Negocio.P_Zona_ID = Cmb_Zona.SelectedItem.Value; }
                    if (Cmb_Tipo_Vehiculo.SelectedIndex > (-1)) { Negocio.P_Tipo = Cmb_Tipo_Vehiculo.SelectedItem.Value; }
                    if (Cmb_Estatus.SelectedIndex > (-1)) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
                    if (Cmb_Aseguradora.SelectedIndex > (-1)) { Negocio.P_Aseguradora_ID = Cmb_Aseguradora.SelectedItem.Value; }
                    if (Cmb_Proveedor.SelectedIndex > (-1)) { Negocio.P_Proveedor = Cmb_Proveedor.SelectedItem.Value; }
                    if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) {
                        Negocio.P_Tomar_Fecha_Inicial = true;
                        Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text);
                    }
                    if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) {
                        Negocio.P_Tomar_Fecha_Final = true;
                        Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text);
                    }
                    if (Hdf_Resguardante_ID.Value.Length > 0) { Negocio.P_Resguardante_ID = Hdf_Resguardante_ID.Value; }
                    DataTable Dt_Datos = Negocio.Consultar_Vehiculos();
                    DataTable Dt_Filtros = new DataTable();
                    Dt_Filtros.TableName = "FILTROS";
                    DataSet Ds_Consulta = new DataSet();
                    Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                    Ds_Consulta.Tables.Add(Dt_Filtros.Copy());
                    Ds_Rpt_Pat_Listado_Vehiculos Ds_Reporte = new Ds_Rpt_Pat_Listado_Vehiculos();
                    if (Filtros == null) { Filtros = "Ninguno"; } else { Filtros = HttpUtility.HtmlDecode(Filtros); }
                    Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Vehiculos.rpt", Tipo_Reporte, Filtros);
                }
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
            private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Tipo, String Filtros) {

                ReportDocument Reporte = new ReportDocument();
                String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
                Reporte.Load(File_Path);
                for (Int32 Contador_Principal = 0; Contador_Principal<(Math.Floor(Convert.ToDouble(Data_Set_Consulta_DB.Tables[0].Rows.Count / 1000)) + 1); Contador_Principal++) {
                    StringBuilder Bienes = new StringBuilder();
                    Bienes.Length = 0;
                    if ((Contador_Principal * 1000) > Data_Set_Consulta_DB.Tables[0].Rows.Count) {
                        break;
                    }
                    for (Int32 Contador = 0; Contador < 1000; Contador= Contador + 1) {
                        Int32 Posicion = (Contador_Principal * 1000) + Contador;
                        if (Posicion >= Data_Set_Consulta_DB.Tables[0].Rows.Count) { break; }
                        Ds_Reporte.Tables["PRINCIPAL"].ImportRow(Data_Set_Consulta_DB.Tables[0].Rows[Posicion]);
                        if (Contador > 0) { Bienes.Append(","); }
                        Bienes.Append("'" + Data_Set_Consulta_DB.Tables[0].Rows[Posicion]["CLAVE_BIEN"].ToString().Trim() + "'");
                    }
                    Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                    Negocio.P_Tipo_Bien = "VEHICULO";
                    Negocio.P_Bien_ID = Bienes.ToString();
                    Negocio.P_Estatus = "VIGENTE";
                    DataTable Dt_Detalles = Negocio.Consultar_Resguardantes();
                    for (Int32 Contador = 0; Contador < Dt_Detalles.Rows.Count; Contador++) {
                        Ds_Reporte.Tables["DETALLES"].ImportRow(Dt_Detalles.Rows[Contador]);
                    }
                }
                for (Int32 Contador = 0; Contador < Data_Set_Consulta_DB.Tables["FILTROS"].Rows.Count; Contador++) {
                    Ds_Reporte.Tables["FILTROS"].ImportRow(Data_Set_Consulta_DB.Tables["FILTROS"].Rows[Contador]);
                }
                Reporte.SetDataSource(Ds_Reporte);
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
                Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Vehiculos.pdf");
                if (Tipo.Equals("Excel")) { Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Vehiculos.xls"); }
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                if (Tipo.Equals("Excel")) { Export_Options.ExportFormatType = ExportFormatType.Excel; }

                Reporte.Export(Export_Options);
                String Ruta = "../../Reporte/Rpt_Pat_Listado_Vehiculos.pdf";
                if (Tipo.Equals("Excel")) { Ruta = "../../Reporte/Rpt_Pat_Listado_Vehiculos.xls"; }
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
            private void Generar_Reporte_Completo(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Tipo, String Filtros, Int32 No_Registros, Double Total) {
                ReportDocument Reporte = new ReportDocument();
                String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
                Reporte.Load(File_Path);
                Ds_Reporte = Data_Set_Consulta_DB;
                Reporte.SetDataSource(Ds_Reporte);
                ExportOptions Export_Options = new ExportOptions();
                DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
                String Ruta = "../../Reporte/Rpt_Pat_Listado_Vehiculos" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
                if (Tipo.Equals("Excel")) { Ruta = "../../Reporte/Rpt_Pat_Listado_Vehiculos" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls"; }
                Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
                if (Tipo.Equals("Excel")) { Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta); }
                Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
                Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
                Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
                if (Tipo.Equals("Excel")) { Export_Options.ExportFormatType = ExportFormatType.Excel; }
                Reporte.SetParameterValue("No_Registros", No_Registros);
                Reporte.SetParameterValue("Valor_Total", Total);
                Reporte.Export(Export_Options);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
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
            private void Generar_Reporte_Completo(String Tipo) {
                try {
                    String Filtros = null;
                    Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                    if (Cmb_Dependencia.SelectedIndex > (-1)) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value; }
                    if (Cmb_Zona.SelectedIndex > (-1)) { Negocio.P_Zona_ID = Cmb_Zona.SelectedItem.Value; }
                    if (Cmb_Tipo_Vehiculo.SelectedIndex > (-1)) { Negocio.P_Tipo = Cmb_Tipo_Vehiculo.SelectedItem.Value; }
                    if (Cmb_Estatus.SelectedIndex > (-1)) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
                    if (Cmb_Aseguradora.SelectedIndex > (-1)) { Negocio.P_Aseguradora_ID = Cmb_Aseguradora.SelectedItem.Value; }
                    if (Cmb_Proveedor.SelectedIndex > (-1)) { Negocio.P_Proveedor = Cmb_Proveedor.SelectedItem.Value; }
                    if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) {
                        Negocio.P_Tomar_Fecha_Inicial = true;
                        Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text);
                    }
                    if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) {
                        Negocio.P_Tomar_Fecha_Final = true;
                        Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text);
                    }
                    if (Hdf_Resguardante_ID.Value.Length > 0) { Negocio.P_Resguardante_ID = Hdf_Resguardante_ID.Value; }
                    DataTable Dt_Datos = Negocio.Consultar_Vehiculos_Completo();

                    DataTable Dt_Filtros = Filtros_Reporte();
                    Dt_Datos.TableName = "PRINCIPAL";
                    Dt_Filtros.TableName = "FILTROS";
                    DataTable Dt_Temporal = Dt_Datos.DefaultView.ToTable(true, "CLAVE_BIEN", "COSTO");
                    DataSet Ds_Consulta = new DataSet();
                    Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                    Ds_Consulta.Tables.Add(Dt_Filtros.Copy());

                    Ds_Rpt_Pat_Listado_Vehiculos_2 Ds_Reporte = new Ds_Rpt_Pat_Listado_Vehiculos_2();
                    
                    Int32 No_Registros = Dt_Temporal.Rows.Count;
                    Double Total = 0;
                    Object Suma_Total = Dt_Temporal.Compute("Sum(COSTO)", "COSTO <> 0");
                    if (Suma_Total != null && Suma_Total != DBNull.Value) Total = Convert.ToDouble(Suma_Total);

                    if (Filtros == null) { Filtros = "Ninguno"; } else { Filtros = HttpUtility.HtmlDecode(Filtros); }

                    Generar_Reporte_Completo(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Vehiculos_2.rpt", Tipo, Filtros, No_Registros, Total);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                    Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
                    Div_Contenedor_Msj_Error.Visible = true;
                }

            }

        #endregion

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
        protected void Grid_Busqueda_Empleados_Resguardo_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Grid_Busqueda_Empleados_Resguardo.SelectedIndex > (-1)) {
                    Hdf_Resguardante_ID.Value = "";
                    Txt_Resguardante.Text = "";
                    String Empleado_Seleccionado_ID = Grid_Busqueda_Empleados_Resguardo.SelectedRow.Cells[1].Text.Trim();
                    Cls_Cat_Empleados_Negocios Empleado_Negocio = new Cls_Cat_Empleados_Negocios();
                    Empleado_Negocio.P_Empleado_ID = Empleado_Seleccionado_ID.Trim();
                    DataTable Dt_Datos_Empleado = Empleado_Negocio.Consulta_Empleados_General();
                    String Dependencia_ID = (!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString().Trim() : null;
                    Int32 Index_Combo = (-1);
                    if (Dependencia_ID != null && Dependencia_ID.Trim().Length > 0) {
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

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Se genera el reporte en formato PDF
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Febrero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
            Generar_Reporte_Completo("PDF");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Se genera el reporte en formato PDF
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Febrero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e) {
            Generar_Reporte_Completo("Excel");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Descargar_Padron_Click
        ///DESCRIPCIÓN: Se ejecuta la operación para descargar el archivo
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 08/Febrero/2012
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Descargar_Padron_Click(object sender, EventArgs e) {
            try {
                Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                if (Cmb_Dependencia.SelectedIndex > (-1)) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value; }
                if (Cmb_Zona.SelectedIndex > (-1)) { Negocio.P_Zona_ID = Cmb_Zona.SelectedItem.Value; }
                if (Cmb_Tipo_Vehiculo.SelectedIndex > (-1)) { Negocio.P_Tipo = Cmb_Tipo_Vehiculo.SelectedItem.Value; }
                if (Cmb_Estatus.SelectedIndex > (-1)) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value; }
                if (Cmb_Aseguradora.SelectedIndex > (-1)) { Negocio.P_Aseguradora_ID = Cmb_Aseguradora.SelectedItem.Value; }
                if (Cmb_Proveedor.SelectedIndex > (-1)) { Negocio.P_Proveedor = Cmb_Proveedor.SelectedItem.Value; }
                if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0)
                {
                    Negocio.P_Tomar_Fecha_Inicial = true;
                    Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text);
                }
                if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0)
                {
                    Negocio.P_Tomar_Fecha_Final = true;
                    Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text);
                }
                if (Hdf_Resguardante_ID.Value.Length > 0) { Negocio.P_Resguardante_ID = Hdf_Resguardante_ID.Value; }
                DataTable Dt_Padron = Negocio.Obtener_Listado_Padron_Vehiculos();
                Pasar_DataTable_A_Excel(Dt_Padron);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "Ex [" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        /// *************************************************************************************************************************
        /// Nombre: Pasar_DataTable_A_Excel
        /// 
        /// Descripción: Pasa DataTable a Excel.
        /// 
        /// Parámetros: Dt_Reporte.- DataTable que se pasara a excel.
        /// 
        /// Usuario Creo: Juan Alberto Hernández Negrete.
        /// Fecha Creó: 18/Octubre/2011.
        /// Usuario Modifico:
        /// Fecha Modifico:
        /// Causa Modificación:
        /// *************************************************************************************************************************
        public void Pasar_DataTable_A_Excel(System.Data.DataTable Dt_Reporte)
        {
            String Ruta = "Padron_Vehicular_" + String.Format("{0:dd_MMM_yyyy}", DateTime.Now) + ".xls";//Variable que almacenara el nombre del archivo. 

            try {
                //Creamos el libro de Excel.
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

                Libro.Properties.Title = "Padron Vehicular";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "Patrimonio";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Registros");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_1 = Libro.Styles.Add("EstatusStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_2 = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_3 = Libro.Styles.Add("URStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_4 = Libro.Styles.Add("HeaderTableStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_5 = Libro.Styles.Add("InvNumStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_6 = Libro.Styles.Add("EcoNumStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_7 = Libro.Styles.Add("EmptyStyle");

                Estilo_1.Font.FontName = "Arial";
                Estilo_1.Font.Size = 12;
                Estilo_1.Font.Bold = true;
                Estilo_1.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_1.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_1.Font.Color = "#000000";
                Estilo_1.Interior.Color = "#92D050";
                Estilo_1.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_1.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2, "Black");
                Estilo_1.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2, "Black");
                Estilo_1.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2, "Black");
                Estilo_1.Alignment.WrapText = true;

                Estilo_2.Font.FontName = "Arial Black";
                Estilo_2.Font.Size = 26;
                Estilo_2.Font.Bold = true;
                Estilo_2.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_2.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_2.Font.Color = "#000000";
                Estilo_2.Interior.Color = "#CCFFCC";
                Estilo_2.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_2.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2, "Black");
                Estilo_2.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2, "Black");
                Estilo_2.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2, "Black");
                Estilo_2.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2, "Black");
                Estilo_2.Alignment.WrapText = true;

                Estilo_3.Font.FontName = "Arial";
                Estilo_3.Font.Size = 12;
                Estilo_3.Font.Bold = true;
                Estilo_3.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_3.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_3.Font.Color = "#000000";
                Estilo_3.Interior.Color = "#FFFF99";
                Estilo_3.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_3.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2, "Black");
                Estilo_3.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2, "Black");
                Estilo_3.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2, "Black");
                Estilo_3.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2, "Black");
                Estilo_3.Alignment.WrapText = true;

                Estilo_4.Font.FontName = "Arial";
                Estilo_4.Font.Size = 9;
                Estilo_4.Font.Bold = true;
                Estilo_4.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_4.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_4.Font.Color = "#000000";
                Estilo_4.Interior.Color = "#CCFFCC";
                Estilo_4.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_4.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2, "Black");
                Estilo_4.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2, "Black");
                Estilo_4.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 2, "Black");
                Estilo_4.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 2, "Black");
                Estilo_4.Alignment.WrapText = true;

                Estilo_5.Font.FontName = "Arial";
                Estilo_5.Font.Size = 9;
                Estilo_5.Font.Bold = false;
                Estilo_5.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_5.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_5.Font.Color = "#000000";
                Estilo_5.Interior.Color = "#FFC000";
                Estilo_5.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_5.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2, "Black");
                Estilo_5.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2, "Black");
                Estilo_5.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_5.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_5.Alignment.WrapText = true;

                Estilo_6.Font.FontName = "Arial";
                Estilo_6.Font.Size = 9;
                Estilo_6.Font.Bold = false;
                Estilo_6.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_6.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_6.Font.Color = "#000000";
                Estilo_6.Interior.Color = "#FFFF00";
                Estilo_6.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_6.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 2, "Black");
                Estilo_6.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 2, "Black");
                Estilo_6.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_6.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_6.Alignment.WrapText = true;

                Estilo_7.Font.FontName = "Arial";
                Estilo_7.Font.Size = 9;
                Estilo_7.Font.Bold = false;
                Estilo_7.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_7.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_7.Font.Color = "#000000";
                Estilo_7.Interior.Color = "#FFFFFF";
                Estilo_7.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_7.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_7.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_7.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_7.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_7.Alignment.WrapText = true;

                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50)); 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50)); 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100)); 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(150)); 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(50));
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(150)); 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60)); 
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(180));
                String Titulo_Reporte = "PARQUE VEHICULAR";
                
                switch (Cmb_Estatus.SelectedValue) {
                    case "VIGENTE": Titulo_Reporte += " [EN OPERATIVIDAD]"; break;
                    case "TEMPORAL": Titulo_Reporte += " [EN BAJA TEMPORAL]"; break;
                    case "DEFINITIVA": Titulo_Reporte += " [EN BAJA DEFINITIVA]"; break;
                    default: Titulo_Reporte += " COMPLETO"; break;
                }
                
                WorksheetCell cell = Renglon.Cells.Add(Titulo_Reporte);
                cell.MergeAcross = 7;             
                cell.StyleID = "EstatusStyle";
                Renglon.Height = 15;
                Renglon = Hoja.Table.Rows.Add();

                cell = Renglon.Cells.Add("PADRON " + DateTime.Today.Year.ToString());
                cell.MergeAcross = 7;             
                cell.StyleID = "HeaderStyle";
                Renglon.Height = 42;
                Renglon = Hoja.Table.Rows.Add();

                if (Dt_Reporte != null) {
                    if (Dt_Reporte.Rows.Count > 0) {
                        DataTable Dt_URS = Dt_Reporte.DefaultView.ToTable(true, "DEPENDENCIA_ID", "DEPENDENCIA_NOMBRE");
                        DataTable Dt_Dependencia_Vacia = Dt_Reporte.Clone();

                        if (Dt_URS != null && Dt_URS.Rows.Count > 0) {
                            foreach (System.Data.DataRow Dr_UR in Dt_URS.Rows) {

                                String Sentencia = "DEPENDENCIA_ID = '" + Dr_UR["DEPENDENCIA_ID"].ToString() + "'";
                                if (String.IsNullOrEmpty(Dr_UR["DEPENDENCIA_ID"].ToString())) { Sentencia = "DEPENDENCIA_ID IS NULL"; }

                                DataRow[] Dr_URS = Dt_Reporte.Select(Sentencia);
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", DataType.String, "EmptyStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", DataType.String, "EmptyStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", DataType.String, "EmptyStyle"));
                                String Nombre_Dependencia = " - - - - - ";
                                if (!Sentencia.Equals("DEPENDENCIA_ID IS NULL")) { Nombre_Dependencia = Dr_URS[0]["DEPENDENCIA_NOMBRE"].ToString(); }
                                cell = Renglon.Cells.Add(Nombre_Dependencia);
                                cell.MergeAcross = 2;
                                cell.StyleID = "URStyle";
                                Renglon.Height = 32;
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", DataType.String, "EmptyStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("", DataType.String, "EmptyStyle"));
                                Renglon = Hoja.Table.Rows.Add();
                                
                                //Se cargan las cabeceras de los datos
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("Nº  INV.", DataType.String, "HeaderTableStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("N° ECO.", DataType.String, "HeaderTableStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("CLASE", DataType.String, "HeaderTableStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("MARCA/TIPO", DataType.String, "HeaderTableStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("MOD.", DataType.String, "HeaderTableStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("SERIE", DataType.String, "HeaderTableStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("PLACAS", DataType.String, "HeaderTableStyle"));
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell("RESGUARDANTE", DataType.String, "HeaderTableStyle"));
                                Renglon.Height = 15;
                                Renglon = Hoja.Table.Rows.Add();

                                foreach (DataRow Fila in Dr_URS) {
                                    //Se cargan los datos
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["NUMERO_INVENTARIO"].ToString(), DataType.String, "InvNumStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["NUMERO_ECONOMICO"].ToString(), DataType.String, "EcoNumStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["CLASE"].ToString(), DataType.String, "EmptyStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["MARCA_TIPO"].ToString(), DataType.String, "EmptyStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["MODELO"].ToString(), DataType.String, "EmptyStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["SERIE"].ToString(), DataType.String, "EmptyStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["PLACAS"].ToString(), DataType.String, "EmptyStyle"));
                                    Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(Fila["RESGUARDANTE"].ToString(), DataType.String, "EmptyStyle"));
                                    Renglon.Height = 15;
                                    Renglon = Hoja.Table.Rows.Add();
                                }
                            }
                        }
                    }
                }

                    

                //Abre el archivo de excel
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", "attachment;filename=" + Ruta);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Libro.Save(Response.OutputStream);
                Response.End();        
            
                } catch (Exception Ex) {
                    throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
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
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

}