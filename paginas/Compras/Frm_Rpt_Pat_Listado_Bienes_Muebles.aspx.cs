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
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Muebles.Negocio;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Procedencias.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Text;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;

public partial class paginas_Compras_Frm_Rpt_Pat_Listado_Bienes_Muebles : System.Web.UI.Page {

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se carga cuando la Pagina de Inicia.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            if (!IsPostBack) {
                Llenar_Combos();
            }
        }

    #endregion

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
        ///DESCRIPCIÓN: Llena los combos del Formulario.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 01/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        public void Llenar_Combos() {
            try {
                Cls_Ope_Pat_Com_Bienes_Muebles_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
                DataTable Dt_Temporal = null;

                //Se llena combo de Dependencias
                BM_Negocio.P_Tipo_DataTable = "DEPENDENCIAS";
                Dt_Temporal = BM_Negocio.Consultar_DataTable();
                Cmb_Busqueda_Dependencia.DataSource = Dt_Temporal;
                Cmb_Busqueda_Dependencia.DataTextField = "NOMBRE";
                Cmb_Busqueda_Dependencia.DataValueField = "DEPENDENCIA_ID";
                Cmb_Busqueda_Dependencia.DataBind();
                Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));
                Cmb_Dependencia.DataSource = Dt_Temporal;
                Cmb_Dependencia.DataTextField = "NOMBRE";
                Cmb_Dependencia.DataValueField = "DEPENDENCIA_ID";
                Cmb_Dependencia.DataBind();
                Cmb_Dependencia.Items.Insert(0, new ListItem("< TODAS >", ""));

                //Se llena combo de Marcas
                BM_Negocio.P_Tipo_DataTable = "MARCAS";
                Dt_Temporal = BM_Negocio.Consultar_DataTable();
                Cmb_Marca.DataSource = Dt_Temporal;
                Cmb_Marca.DataTextField = "NOMBRE";
                Cmb_Marca.DataValueField = "MARCA_ID";
                Cmb_Marca.DataBind();
                Cmb_Marca.Items.Insert(0, new ListItem("< TODAS >", ""));

                //Se llena combo de Materiales
                BM_Negocio.P_Tipo_DataTable = "MATERIALES";
                Dt_Temporal = BM_Negocio.Consultar_DataTable();
                Cmb_Material.DataSource = Dt_Temporal;
                Cmb_Material.DataTextField = "DESCRIPCION";
                Cmb_Material.DataValueField = "MATERIAL_ID";
                Cmb_Material.DataBind();
                Cmb_Material.Items.Insert(0, new ListItem("< TODAS >", ""));

                //Se llena combo de Colores
                BM_Negocio.P_Tipo_DataTable = "COLORES";
                Dt_Temporal = BM_Negocio.Consultar_DataTable();
                Cmb_Color.DataSource = Dt_Temporal;
                Cmb_Color.DataTextField = "DESCRIPCION";
                Cmb_Color.DataValueField = "COLOR_ID";
                Cmb_Color.DataBind();
                Cmb_Color.Items.Insert(0, new ListItem("< TODOS >", ""));

                //Se llena combo de Zonas
                BM_Negocio.P_Tipo_DataTable = "ZONAS";
                Dt_Temporal = BM_Negocio.Consultar_DataTable();
                Cmb_Zona.DataSource = Dt_Temporal;
                Cmb_Zona.DataTextField = "DESCRIPCION";
                Cmb_Zona.DataValueField = "ZONA_ID";
                Cmb_Zona.DataBind();
                Cmb_Zona.Items.Insert(0, new ListItem("< TODAS >", ""));

                Cls_Cat_Pat_Com_Procedencias_Negocio Procedencia_Negocio = new Cls_Cat_Pat_Com_Procedencias_Negocio();
                Procedencia_Negocio.P_Estatus = "VIGENTE";
                Procedencia_Negocio.P_Tipo_DataTable = "PROCEDENCIAS";
                Cmb_Procedencia.DataSource = Procedencia_Negocio.Consultar_DataTable();
                Cmb_Procedencia.DataTextField = "NOMBRE";
                Cmb_Procedencia.DataValueField = "PROCEDENCIA_ID";
                Cmb_Procedencia.DataBind();
                Cmb_Procedencia.Items.Insert(0, new ListItem("< TODAS >", ""));

                Cls_Cat_Pat_Com_Clases_Activo_Negocio CA_Negocio = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
                CA_Negocio.P_Estatus = "VIGENTE";
                CA_Negocio.P_Tipo_DataTable = "CLASES_ACTIVOS";
                Cmb_Clase_Activo.DataSource = CA_Negocio.Consultar_DataTable();
                Cmb_Clase_Activo.DataValueField = "CLASE_ACTIVO_ID";
                Cmb_Clase_Activo.DataTextField = "CLAVE_DESCRIPCION";
                Cmb_Clase_Activo.DataBind();
                Cmb_Clase_Activo.Items.Insert(0, new ListItem("< TODAS >", ""));

                Cls_Cat_Pat_Com_Clasificaciones_Negocio Clasificaciones_Negocio = new Cls_Cat_Pat_Com_Clasificaciones_Negocio();
                Clasificaciones_Negocio.P_Estatus = "VIGENTE";
                Clasificaciones_Negocio.P_Tipo_DataTable = "CLASIFICACIONES";
                Cmb_Tipo_Activo.DataSource = Clasificaciones_Negocio.Consultar_DataTable();
                Cmb_Tipo_Activo.DataValueField = "CLASIFICACION_ID";
                Cmb_Tipo_Activo.DataTextField = "CLAVE_DESCRIPCION";
                Cmb_Tipo_Activo.DataBind();
                Cmb_Tipo_Activo.Items.Insert(0, new ListItem("< TODOS >", ""));

            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar al Llenar los Combos.";
                Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
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
        private void Llenar_Grid_Busqueda_Empleados_Resguardo() {
            Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
            Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = true;
            Cls_Ope_Pat_Com_Bienes_Muebles_Negocio Negocio = new Cls_Ope_Pat_Com_Bienes_Muebles_Negocio();
            Negocio.P_Estatus_Empleado = "ACTIVO','INACTIVO";
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
        private static String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
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
        private void Generar_Reporte(String Tipo) {
            try
            {
                String Filtros = null;
                Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                if (Txt_Inventario_Anterior.Text.Trim().Length > 0) { Negocio.P_No_Inventario_Anterior = Txt_Inventario_Anterior.Text.Trim(); }
                if (Txt_Inventario_SIAS.Text.Trim().Length > 0) { Negocio.P_No_Inventario_SIAS = Txt_Inventario_SIAS.Text.Trim(); }
                if (Txt_Nombre_Producto.Text.Trim().Length > 0) { Negocio.P_Nombre_Producto = Txt_Nombre_Producto.Text.Trim(); }
                if (Cmb_Tipo_Activo.SelectedIndex > 0) { Negocio.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim(); }
                if (Cmb_Clase_Activo.SelectedIndex > 0) { Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim(); }
                if (Cmb_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value.Trim(); }
                if (Txt_Modelo.Text.Trim().Length > 0) { Negocio.P_Modelo = Txt_Modelo.Text.Trim(); }
                if (Cmb_Marca.SelectedIndex > 0) { Negocio.P_Marca_ID = Cmb_Marca.SelectedItem.Value.Trim(); }
                if (Cmb_Material.SelectedIndex > 0) { Negocio.P_Material_ID = Cmb_Material.SelectedItem.Value.Trim(); }
                if (Cmb_Procedencia.SelectedIndex > 0) { Negocio.P_Procedencia = Cmb_Procedencia.SelectedItem.Value.Trim(); }
                if (Cmb_Color.SelectedIndex > 0) { Negocio.P_Color_ID = Cmb_Color.SelectedItem.Value.Trim(); }
                if (Cmb_Zona.SelectedIndex > 0) { Negocio.P_Zona_ID = Cmb_Zona.SelectedItem.Value.Trim(); }
                if (Txt_Factura.Text.Trim().Length > 0) { Negocio.P_Factura = Txt_Factura.Text.Trim(); }
                if (Txt_Numero_Serie.Text.Trim().Length > 0) { Negocio.P_Serie = Txt_Numero_Serie.Text.Trim(); }
                if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) {
                    Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text.Trim());
                    Negocio.P_Tomar_Fecha_Inicial = true;
                }
                if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) {
                    Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text.Trim());
                    Negocio.P_Tomar_Fecha_Final = true;
                }
                if (Txt_Fecha_Modifico_Inicio.Text.Trim().Length > 0) {
                    Negocio.P_Fecha_Modificacion_Inicial = Convert.ToDateTime(Txt_Fecha_Modifico_Inicio.Text.Trim());
                    Negocio.P_Tomar_Fecha_Inicial_Modificacion = true;
                }
                if (Txt_Fecha_Modifico_Fin.Text.Trim().Length > 0) {
                    Negocio.P_Fecha_Modificacion_Final = Convert.ToDateTime(Txt_Fecha_Modifico_Fin.Text.Trim());
                    Negocio.P_Tomar_Fecha_Final_Modificacion = true;
                }
                if (Cmb_Estatus.SelectedIndex > 0) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim(); }
                if (Cmb_Estado.SelectedIndex > 0) { Negocio.P_Estado = Cmb_Estado.SelectedItem.Value.Trim(); }
                if (Cmb_Operacion.SelectedIndex > 0) { Negocio.P_Operacion = Cmb_Operacion.SelectedItem.Value.Trim(); } else { Negocio.P_Operacion = "TODOS"; }
                if (Cmb_Estatus_Resguardo.SelectedIndex > 0) { Negocio.P_Estatus_Resguardante = Cmb_Estatus_Resguardo.SelectedItem.Value.Trim(); }

                if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Negocio.P_Resguardante_ID = Hdf_Resguardante_ID.Value.Trim(); }
                DataTable Dt_Datos = Negocio.Consultar_Bienes_Muebles();
                DataTable Dt_Filtros = Obtener_Filtros();
                Dt_Filtros.TableName = "FILTROS";
                DataSet Ds_Consulta = new DataSet();
                Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                Ds_Consulta.Tables.Add(Dt_Filtros.Copy());

                Ds_Rpt_Pat_Listado_Bienes_Muebles Ds_Reporte = new Ds_Rpt_Pat_Listado_Bienes_Muebles();

                if (Filtros == null) { Filtros = "Ninguno"; } else { Filtros = HttpUtility.HtmlDecode(Filtros); }

                Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Bienes_Muebles.rpt", Tipo, Filtros);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
                Div_Contenedor_Msj_Error.Visible = true;
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
            for (Int32 Contador_Principal = 0; Contador_Principal < (Math.Floor(Convert.ToDouble(Data_Set_Consulta_DB.Tables[0].Rows.Count / 1000)) + 1); Contador_Principal++) {
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
                Negocio.P_Tipo_Bien = "BIEN_MUEBLE";
                Negocio.P_Bien_ID = Bienes.ToString();
                if (Cmb_Estatus_Resguardo.SelectedIndex > 0) {
                    Negocio.P_Estatus = Cmb_Estatus_Resguardo.SelectedItem.Value;
                }
                DataTable Dt_Detalles = Negocio.Consultar_Resguardantes();
                
                for (Int32 Contador = 0; Contador < Dt_Detalles.Rows.Count; Contador++) {
                    DataRow[] bienes = Ds_Reporte.Tables["PRINCIPAL"].Select("CLAVE_BIEN = '" + Dt_Detalles.Rows[Contador]["CLAVE_BIEN"].ToString().Trim() + "'");
                    foreach (DataRow bien in bienes) {
                        if (Dt_Detalles.Rows[Contador]["ESTATUS_RESGUARDO"].ToString().Trim().Equals("VIGENTE")) {
                            bien.SetField("UNIDAD_RESPONSABLE", Dt_Detalles.Rows[Contador]["DEPENDENCIA"].ToString().Trim());
                        }
                    }
                    Ds_Reporte.Tables["DETALLES"].ImportRow(Dt_Detalles.Rows[Contador]);
                }
            }
            for (Int32 Contador = 0; Contador < Data_Set_Consulta_DB.Tables["FILTROS"].Rows.Count; Contador++) {
                Ds_Reporte.Tables["FILTROS"].ImportRow(Data_Set_Consulta_DB.Tables["FILTROS"].Rows[Contador]);
            }
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            String Ruta = "../../Reporte/Rpt_Pat_Listado_Bienes_Muebles" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
            if (Tipo.Equals("Excel")) { Ruta = "../../Reporte/Rpt_Pat_Listado_Bienes_Muebles" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls"; }
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            if (Tipo.Equals("Excel")) { Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta); }
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (Tipo.Equals("Excel")) { Export_Options.ExportFormatType = ExportFormatType.Excel; }

            Reporte.Export(Export_Options);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Filtros
        ///DESCRIPCIÓN: Genera y Carga los Filtros del Reporte
        ///PARAMETROS: 
        ///CREO: Francisco A. Gallardo Castañeda.
        ///FECHA_CREO: 09/Dic/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private DataTable Obtener_Filtros() {
            DataTable Dt_Datos = new DataTable();
            Dt_Datos.Columns.Add("Filtro", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("Valor", Type.GetType("System.String"));
            if (Txt_Inventario_Anterior.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "No. Inventario Anterior: ", Txt_Inventario_Anterior.Text.Trim()); }
            if (Txt_Inventario_SIAS.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "No. Inventario [SIAS]: ", Txt_Inventario_SIAS.Text.Trim()); }
            if (Txt_Nombre_Producto.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "Producto: ", Txt_Nombre_Producto.Text.Trim()); }
            if (Cmb_Tipo_Activo.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Tipo de Activo: ", Cmb_Tipo_Activo.SelectedItem.Text.Trim()); }
            if (Cmb_Clase_Activo.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Clase de Activo: ", Cmb_Clase_Activo.SelectedItem.Text.Trim()); }
            if (Cmb_Dependencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Unidad Responsable: ", Cmb_Dependencia.SelectedItem.Text.Trim()); }
            if (Txt_Modelo.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "Modelo: ", Txt_Modelo.Text.Trim()); }
            if (Cmb_Marca.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Marca: ", Cmb_Marca.SelectedItem.Text.Trim()); }
            if (Cmb_Material.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Material: ", Cmb_Material.SelectedItem.Text.Trim()); }
            if (Cmb_Procedencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Procedencia: ", Cmb_Procedencia.SelectedItem.Text.Trim()); }
            if (Cmb_Color.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Color: ", Cmb_Color.SelectedItem.Text.Trim()); }
            if (Cmb_Zona.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Zona: ", Cmb_Zona.SelectedItem.Text.Trim()); }
            if (Txt_Factura.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "No. Factura: ", Txt_Factura.Text.Trim()); }
            if (Txt_Numero_Serie.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "No. Serie: ", Txt_Numero_Serie.Text.Trim()); }
            if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) {  Cargar_Filtro(Dt_Datos, "Fecha Adquisición Inicial: ", Txt_Fecha_Adquisicion_Inicial.Text.Trim()); }
            if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "Fecha Adquisición Final: ", Txt_Fecha_Adquisicion_Final.Text.Trim()); }
            if (Txt_Fecha_Modifico_Inicio.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "Fecha Modificación Inicial: ", Txt_Fecha_Modifico_Inicio.Text.Trim()); }
            if (Txt_Fecha_Modifico_Fin.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "Fecha Modificación Final: ", Txt_Fecha_Modifico_Fin.Text.Trim()); }
            if (Cmb_Estatus.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Estatus [Bien]: ", Cmb_Estatus.SelectedItem.Text.Trim()); }
            if (Cmb_Estado.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Estado: ", Cmb_Estado.SelectedItem.Text.Trim()); }
            if (Cmb_Operacion.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Operación: ", Cmb_Operacion.SelectedItem.Text.Trim()); }
            if (Cmb_Estatus_Resguardo.SelectedIndex > 0) { Cargar_Filtro(Dt_Datos, "Estatus [Resguardo]: ", Cmb_Estatus_Resguardo.SelectedItem.Value.Trim()); }
            if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Cargar_Filtro(Dt_Datos, "Resguardante: ", Txt_Resguardante.Text.Trim()); }
            return Dt_Datos;
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
        private void Generar_Reporte_Completo(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Tipo, String Filtros, Int32 No_Registros, Double Total_Incial, Double Total_Actual) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            String Ruta = "../../Reporte/Rpt_Pat_Listado_Bienes_Muebles" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
            if (Tipo.Equals("Excel")) { Ruta = "../../Reporte/Rpt_Pat_Listado_Bienes_Muebles" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls"; }
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            if (Tipo.Equals("Excel")) { Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta); }
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (Tipo.Equals("Excel")) { Export_Options.ExportFormatType = ExportFormatType.Excel; }
            Reporte.SetParameterValue("No_Registros", No_Registros);
            Reporte.SetParameterValue("Valor_Total_Inicial", Total_Incial);
            Reporte.SetParameterValue("Valor_Total_Actual", Total_Actual);
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
        private void Generar_Reporte_Completo(String Tipo)
        {
            try
            {
                String Filtros = null;
                Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                if (Txt_Inventario_Anterior.Text.Trim().Length > 0) { Negocio.P_No_Inventario_Anterior = Txt_Inventario_Anterior.Text.Trim(); }
                if (Txt_Inventario_SIAS.Text.Trim().Length > 0) { Negocio.P_No_Inventario_SIAS = Txt_Inventario_SIAS.Text.Trim(); }
                if (Txt_Nombre_Producto.Text.Trim().Length > 0) { Negocio.P_Nombre_Producto = Txt_Nombre_Producto.Text.Trim(); }
                if (Cmb_Tipo_Activo.SelectedIndex > 0) { Negocio.P_Clasificacion_ID = Cmb_Tipo_Activo.SelectedItem.Value.Trim(); }
                if (Cmb_Clase_Activo.SelectedIndex > 0) { Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value.Trim(); }
                if (Cmb_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value.Trim(); }
                if (Txt_Modelo.Text.Trim().Length > 0) { Negocio.P_Modelo = Txt_Modelo.Text.Trim(); }
                if (Cmb_Marca.SelectedIndex > 0) { Negocio.P_Marca_ID = Cmb_Marca.SelectedItem.Value.Trim(); }
                if (Cmb_Material.SelectedIndex > 0) { Negocio.P_Material_ID = Cmb_Material.SelectedItem.Value.Trim(); }
                if (Cmb_Procedencia.SelectedIndex > 0) { Negocio.P_Procedencia = Cmb_Procedencia.SelectedItem.Value.Trim(); }
                if (Cmb_Color.SelectedIndex > 0) { Negocio.P_Color_ID = Cmb_Color.SelectedItem.Value.Trim(); }
                if (Cmb_Zona.SelectedIndex > 0) { Negocio.P_Zona_ID = Cmb_Zona.SelectedItem.Value.Trim(); }
                if (Txt_Factura.Text.Trim().Length > 0) { Negocio.P_Factura = Txt_Factura.Text.Trim(); }
                if (Txt_Numero_Serie.Text.Trim().Length > 0) { Negocio.P_Serie = Txt_Numero_Serie.Text.Trim(); }
                if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0)
                {
                    Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text.Trim());
                    Negocio.P_Tomar_Fecha_Inicial = true;
                }
                if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0)
                {
                    Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text.Trim());
                    Negocio.P_Tomar_Fecha_Final = true;
                }
                if (Txt_Fecha_Modifico_Inicio.Text.Trim().Length > 0)
                {
                    Negocio.P_Fecha_Modificacion_Inicial = Convert.ToDateTime(Txt_Fecha_Modifico_Inicio.Text.Trim());
                    Negocio.P_Tomar_Fecha_Inicial_Modificacion = true;
                }
                if (Txt_Fecha_Modifico_Fin.Text.Trim().Length > 0)
                {
                    Negocio.P_Fecha_Modificacion_Final = Convert.ToDateTime(Txt_Fecha_Modifico_Fin.Text.Trim());
                    Negocio.P_Tomar_Fecha_Final_Modificacion = true;
                }
                if (Cmb_Estatus.SelectedIndex > 0) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim(); }
                if (Cmb_Estado.SelectedIndex > 0) { Negocio.P_Estado = Cmb_Estado.SelectedItem.Value.Trim(); }
                if (Cmb_Operacion.SelectedIndex > 0) { Negocio.P_Operacion = Cmb_Operacion.SelectedItem.Value.Trim(); } else { Negocio.P_Operacion = "TODOS"; }
                if (Cmb_Estatus_Resguardo.SelectedIndex > 0) { Negocio.P_Estatus_Resguardante = Cmb_Estatus_Resguardo.SelectedItem.Value.Trim(); }

                if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Negocio.P_Resguardante_ID = Hdf_Resguardante_ID.Value.Trim(); }
                DataTable Dt_Datos = Negocio.Consultar_Bienes_Muebles_Completo();
                DataTable Dt_Filtros = Obtener_Filtros();
                DataTable Dt_Temporal = Dt_Datos.DefaultView.ToTable(true, "CLAVE_BIEN", "VALOR_INCIAL");
                Int32 No_Registros = Dt_Temporal.Rows.Count;
                Double Total_Inicial = 0;
                Double Total_Actual = 0;
                Object Suma_Total_Inicial = Dt_Temporal.Compute("Sum(VALOR_INCIAL)", "VALOR_INCIAL <> 0");
                Object Suma_Total_Actual = Dt_Temporal.Compute("Sum(VALOR_INCIAL)", "VALOR_INCIAL <> 0");
                if (Suma_Total_Actual != null && Suma_Total_Actual != DBNull.Value) Total_Actual = Convert.ToDouble(Suma_Total_Actual);
                if (Suma_Total_Inicial != null && Suma_Total_Inicial != DBNull.Value) Total_Inicial = Convert.ToDouble(Suma_Total_Inicial);

                Dt_Datos.TableName = "PRINCIPAL";
                Dt_Filtros.TableName = "FILTROS";

                DataSet Ds_Consulta = new DataSet();
                Ds_Consulta.Tables.Add(Dt_Datos.Copy());
                Ds_Consulta.Tables.Add(Dt_Filtros.Copy());

                Ds_Rpt_Pat_Listado_Bienes_Muebles_2 Ds_Reporte = new Ds_Rpt_Pat_Listado_Bienes_Muebles_2();

                if (Filtros == null) { Filtros = "Ninguno"; } else { Filtros = HttpUtility.HtmlDecode(Filtros); }

                Generar_Reporte_Completo(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Bienes_Muebles_2.rpt", Tipo, Filtros, No_Registros, Total_Inicial, Total_Actual);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "'" + Ex.Message + "'";
                Div_Contenedor_Msj_Error.Visible = true;
            }

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
        protected void Grid_Busqueda_Empleados_Resguardo_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Busqueda_Empleados_Resguardo.PageIndex = e.NewPageIndex;
                Llenar_Grid_Busqueda_Empleados_Resguardo();
                MPE_Resguardante.Show();
            } catch (Exception Ex) {
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
                    } else {
                        Cmb_Dependencia.SelectedIndex = 0;
                    }
                    Hdf_Resguardante_ID.Value = ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString().Trim() : null);
                    Txt_Resguardante.Text += ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString().Trim() : null);
                    Txt_Resguardante.Text += " - " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Paterno].ToString().Trim() : null);
                    Txt_Resguardante.Text = Txt_Resguardante.Text.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Apellido_Materno].ToString().Trim() : null);
                    Txt_Resguardante.Text = Txt_Resguardante.Text.Trim() + " " + ((!String.IsNullOrEmpty(Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString())) ? Dt_Datos_Empleado.Rows[0][Cat_Empleados.Campo_Nombre].ToString().Trim() : null);
                    MPE_Resguardante.Hide();
                    Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion

    #region "Eventos"

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

    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Lanza el Reporte en PDF para Imprimir.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 02/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
            try
            {
                Generar_Reporte_Completo("Pdf");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_Excel_Click
        ///DESCRIPCIÓN: Lanza el Reporte en Excel.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 05/Diciembre/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e){
            try{
                Generar_Reporte_Completo("Excel");
            }catch (Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

    #endregion
}
