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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Control_Patrimonial_Operacion_Siniestros.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Vehiculos.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Aseguradoras.Negocio;
using Presidencia.Catalogo_Compras_Marcas.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;

public partial class paginas_predial_Frm_Ope_Pat_Com_Seguimiento_Siniestros : System.Web.UI.Page {
    
    #region Page_Load

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
        ///PARAMETROS:     
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        protected void Page_Load(object sender, EventArgs e){
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Llenar_Combos_Independientes();
                Llenar_Combo_Tipos_Siniestro();
                Configuracion_Formulario(true, "");
                Grid_Observaciones.Columns[0].Visible = false;
            }
            Div_Contenedor_Msj_Error.Visible = false;
        }

    #endregion
    
    #region Metodos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipos_Siniestro
        ///DESCRIPCIÓN: Llena el Combo de Tipos de Siniestros
        ///PARAMETROS:     
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        private void Llenar_Combo_Tipos_Siniestro() {
            try {
                Cls_Ope_Pat_Com_Siniestros_Negocio Negocio = new Cls_Ope_Pat_Com_Siniestros_Negocio();
                Negocio.P_Tipo_DataTable = "TIPOS_SINIESTROS";
                DataTable Tipos_Siniestros = Negocio.Consultar_DataTable();
                DataRow Fila_Tipo_Siniestro = Tipos_Siniestros.NewRow();
                Fila_Tipo_Siniestro["SINIESTRO_ID"] = "SELECCIONE";
                Fila_Tipo_Siniestro["DESCRIPCION"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
                Tipos_Siniestros.Rows.InsertAt(Fila_Tipo_Siniestro, 0);
                Cmb_Tipo_Siniestros.DataSource = Tipos_Siniestros;
                Cmb_Tipo_Siniestros.DataValueField = "SINIESTRO_ID";
                Cmb_Tipo_Siniestros.DataTextField = "DESCRIPCION";
                Cmb_Tipo_Siniestros.DataBind();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;            
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
        ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
        ///PARAMETROS:     
        ///             1. Estatus.    Estatus en el que se cargara la configuración de los
        ///                             controles.
        ///             2. Operacion.   Operacion que se va a realizar para hacer la
        ///                             configuración
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN 
        ///*******************************************************************************
        private void Configuracion_Formulario( Boolean Estatus, String Operacion ) {
            if (Operacion.Trim().Equals("MODIFICAR")) {
                Btn_Modificar.AlternateText = "Actualizar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/sias_aceptarplan.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
            } else {
                Btn_Modificar.Visible = true;
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            }
            Btn_Generar_Reporte_PDF.Visible = Estatus;
            Txt_Observaciones.Enabled = !Estatus;
            Cmb_Estatus.Enabled = !Estatus;
            Div_Busqueda.Visible = Estatus;
            Configuracion_Acceso("Frm_Ope_Pat_Com_Seguimiento_Siniestros.aspx");
            Configuracion_Acceso_LinkButton("Frm_Ope_Pat_Com_Seguimiento_Siniestros.aspx");
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
        ///DESCRIPCIÓN: Limpia los controles del Formulario
        ///PARAMETROS:     
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Limpiar_Catalogo() {
            Hdf_Siniestro_ID.Value = "";
            Txt_Siniestro_ID.Text = "";
            Cmb_Tipo_Siniestros.SelectedIndex = 0;
            Txt_Bien.Text = "";
            Hdf_Bien_ID.Value = "";
            Txt_Aseguradora.Text = "";
            Hdf_Aseguradora.Value = "";
            Txt_Fecha.Text = "";
            Cmb_Estatus.SelectedIndex = 0;
            Txt_Parte_Averiguacion.Text = "";
            Chk_Responsable_Municipio.Checked = false;
            Chk_Consignado.Checked = false;
            Chk_Pago_Danios_Sindicos.Checked = false;
            Cmb_Reparacion.SelectedIndex = 0;
            Txt_Descripcion.Text = "";
            Txt_Observaciones.Text = "";
            Txt_Numero_Reporte.Text = "";
            Grid_Observaciones.DataSource = new DataTable();
            Grid_Observaciones.DataBind();
            Session.Remove("Dt_Observaciones");
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Tipos_Siniestros
        ///DESCRIPCIÓN: Llena la tabla de Tipos Siniestros con una consulta que puede o no
        ///             tener Filtros.
        ///PARAMETROS:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Listado_Siniestros(Int32 Pagina) {
            try{
                Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro = new Cls_Ope_Pat_Com_Siniestros_Negocio();
                Siniestro.P_Tipo_DataTable = "SINIESTROS";
                if (Txt_MPE_Siniestros_Numero_Inventario.Text.Trim().Length > 0) {
                    Siniestro.P_Numero_Inventario = Convert.ToInt32(Txt_MPE_Siniestros_Numero_Inventario.Text.Trim());
                }
                if (Txt_MPE_Siniestros_Clave_Sistema.Text.Trim().Length > 0) {
                    Siniestro.P_Clave_Sistema = Convert.ToInt32(Txt_MPE_Siniestros_Clave_Sistema.Text.Trim());
                }
                if (Txt_MPE_Siniestros_Descripcion.Text.Trim().Length > 0) {
                    Siniestro.P_Descripcion = Txt_MPE_Siniestros_Descripcion.Text.Trim();
                } 
                if (Cmb_MPE_Siniestros_Estatus.SelectedIndex > 0) {
                    Siniestro.P_Estatus = Cmb_MPE_Siniestros_Estatus.SelectedItem.Value.Trim();
                }
                if (Cmb_MPE_Siniestros_Dependencias.SelectedIndex > 0) {
                    Siniestro.P_Dependencia = Cmb_MPE_Siniestros_Dependencias.SelectedItem.Value.Trim();
                }
                if (Cmb_MPE_Siniestros_Aseguradoras.SelectedIndex > 0) {
                    Siniestro.P_Aseguradora_ID = Cmb_MPE_Siniestros_Aseguradoras.SelectedItem.Value.Trim();
                }
                if (Txt_MPE_Siniestros_Fecha.Text.Trim().Length > 0) {
                    Siniestro.P_Fecha = Convert.ToDateTime(Txt_MPE_Siniestros_Fecha.Text.Trim());
                    Siniestro.P_Buscar_Fecha = true;
                } 
                Grid_Listado_Siniestros.DataSource = Siniestro.Consultar_DataTable();
                Grid_Listado_Siniestros.PageIndex = Pagina;
                Grid_Listado_Siniestros.DataBind();
                MPE_Busqueda_Siniestro.Show();
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Observaciones
        ///DESCRIPCIÓN: Llena la tabla de Observaciones.
        ///PARAMETROS:     
        ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Llenar_Grid_Observaciones(Int32 Pagina, DataTable Tabla) {
            try {
                Grid_Observaciones.Columns[0].Visible = true;
                Grid_Observaciones.DataSource = Tabla;
                Grid_Observaciones.PageIndex = Pagina;
                Grid_Observaciones.DataBind();
                Grid_Observaciones.Columns[0].Visible = false;
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Mostrar_Detalles_Siniestro
        ///DESCRIPCIÓN: Muestra a detalle un Siniestro.
        ///PARAMETROS:     
        ///             1. Siniestro.  Objeto cargado con los detalles del siniestro.
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Mostrar_Detalles_Siniestro(Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro) {
             try {
                Limpiar_Catalogo();
                Hdf_Siniestro_ID.Value = Siniestro.P_Siniestro_ID;
                Txt_Siniestro_ID.Text = Siniestro.P_Siniestro_ID;
                Cmb_Tipo_Siniestros.SelectedIndex = Cmb_Tipo_Siniestros.Items.IndexOf(Cmb_Tipo_Siniestros.Items.FindByValue(Siniestro.P_Tipo_Siniestro_ID));
                String Vehiculo_Seleccionado_ID = Siniestro.P_Bien_ID;

                Cls_Ope_Pat_Com_Vehiculos_Negocio Vehiculo = new Cls_Ope_Pat_Com_Vehiculos_Negocio();
                Vehiculo.P_Vehiculo_ID = Vehiculo_Seleccionado_ID;
                Vehiculo = Vehiculo.Consultar_Detalles_Vehiculo();
                Hdf_Bien_ID.Value = Vehiculo.P_Vehiculo_ID;
                Hdf_Aseguradora.Value = Vehiculo.P_Aseguradora_ID;

                Cls_Cat_Com_Marcas_Negocio Marca_Negocio = new Cls_Cat_Com_Marcas_Negocio();
                Marca_Negocio.P_Marca_ID = Vehiculo.P_Marca_ID;
                DataTable Dt_Detalles_Marca = Marca_Negocio.Consulta_Marcas();

                Txt_Bien.Text = "[" + Vehiculo.P_Numero_Inventario.ToString() + "] " + Vehiculo.P_Nombre_Producto + ((Dt_Detalles_Marca.Rows.Count > 0) ? " " + Dt_Detalles_Marca.Rows[0][Cat_Com_Marcas.Campo_Nombre].ToString() : "") + " - " + Vehiculo.P_Modelo_ID;

                Hdf_Bien_ID.Value = Siniestro.P_Bien_ID;
                Hdf_Aseguradora.Value = Siniestro.P_Aseguradora_ID;
                Txt_Fecha.Text = Siniestro.P_Fecha.ToString("dd/MMM/yyyy");
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Siniestro.P_Estatus));
                Txt_Parte_Averiguacion.Text = Siniestro.P_Parte_Averiguacion;
                Cmb_Reparacion.SelectedIndex = Cmb_Reparacion.Items.IndexOf(Cmb_Reparacion.Items.FindByValue(Siniestro.P_Reparacion));
                Chk_Responsable_Municipio.Checked = Siniestro.P_Responsable_Municipio;
                Chk_Consignado.Checked = Siniestro.P_Consignado;
                Chk_Pago_Danios_Sindicos.Checked = Siniestro.P_Pago_Danos_Sindicos;
                Txt_Descripcion.Text = Siniestro.P_Descripcion;
                Txt_Numero_Reporte.Text = Siniestro.P_No_Reporte;

                if (Siniestro.P_Aseguradora_ID != null) {
                    if(Siniestro.P_Aseguradora_ID.Trim().Length > 0){
                        Cls_Cat_Pat_Com_Aseguradoras_Negocio Aseguradora = new Cls_Cat_Pat_Com_Aseguradoras_Negocio();
                        Aseguradora.P_Aseguradora_ID = Siniestro.P_Aseguradora_ID;
                        Aseguradora = Aseguradora.Consultar_Datos_Aseguradora();
                        Txt_Aseguradora.Text = Aseguradora.P_Nombre_Fiscal + " / " + Aseguradora.P_Nombre;
                    } else {
                        Txt_Aseguradora.Text = "NO HAY ASEGURADORA ASIGNADA AL VEHICULO.";
                    }
                } else {
                    Txt_Aseguradora.Text = "NO HAY ASEGURADORA ASIGNADA AL VEHICULO.";
                }
                Llenar_Grid_Observaciones(0, Siniestro.P_Dt_Observaciones);
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
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
        private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Siniestro_Detalles.pdf");
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
            String Ruta = "../../Reporte/Rpt_Pat_Siniestro_Detalles.pdf";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Reporte
        ///DESCRIPCIÓN: Carga en un DataSet los datos que se mostrarán el el Reporte.
        ///PARAMETROS: 
        ///CREO: Francisco antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO:
        ///CAUSA_MODIFICACIÓN:
        ///*******************************************************************************
        private DataSet Cargar_Datos_Reporte() {

            DataTable Dt_Generales = new DataTable("DT_GENERALES");
            Dt_Generales.Columns.Add("SINIESTRO_ID", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("TIPO_SINIESTRO", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("VEHICULO", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("ASEGURADORA", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("FECHA_SINIESTRO", Type.GetType("System.DateTime"));
            Dt_Generales.Columns.Add("ESTATUS", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("NO_AVERIGUACION", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("REPARACION", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("RESPONSABLE_MPIO", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("CONSIGNADO", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("PAGO_SINDICOS", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("NUMERO_REPORTE", Type.GetType("System.String"));
            Dt_Generales.Columns.Add("DESCRIPCION", Type.GetType("System.String"));

            DataRow Fila_Generales = Dt_Generales.NewRow();
            Fila_Generales["SINIESTRO_ID"] = Txt_Siniestro_ID.Text.Trim();
            Fila_Generales["TIPO_SINIESTRO"] = Cmb_Tipo_Siniestros.SelectedItem.Text.Trim();
            Fila_Generales["VEHICULO"] = Txt_Bien.Text.Trim();
            Fila_Generales["ASEGURADORA"] = Txt_Aseguradora.Text.Trim();
            Fila_Generales["FECHA_SINIESTRO"] = Convert.ToDateTime(Txt_Fecha.Text.Trim());
            Fila_Generales["ESTATUS"] = Cmb_Estatus.SelectedItem.Text.Trim();
            Fila_Generales["NO_AVERIGUACION"] = Txt_Parte_Averiguacion.Text.Trim();
            Fila_Generales["REPARACION"] = Cmb_Reparacion.SelectedItem.Text.Trim();
            Fila_Generales["RESPONSABLE_MPIO"] = (Chk_Responsable_Municipio.Checked) ? "SI" : "NO";
            Fila_Generales["CONSIGNADO"] = (Chk_Consignado.Checked) ? "SI" : "NO";
            Fila_Generales["PAGO_SINDICOS"] = (Chk_Pago_Danios_Sindicos.Checked) ? "SI" : "NO";
            Fila_Generales["DESCRIPCION"] = Txt_Descripcion.Text.Trim();
            Fila_Generales["NUMERO_REPORTE"] = (Txt_Numero_Reporte.Text.Trim().Length > 0) ? Txt_Numero_Reporte.Text.Trim() : "-";
            Dt_Generales.Rows.Add(Fila_Generales);

            DataTable Dt_Observaciones = new DataTable("DT_OBSERVACIONES");
            Dt_Observaciones.Columns.Add("FECHA_OBSERVACION", Type.GetType("System.DateTime"));
            Dt_Observaciones.Columns.Add("OBSERVACION", Type.GetType("System.String"));
            Dt_Observaciones.Columns.Add("AUTOR_OBSERVACION", Type.GetType("System.String"));
            for (Int32 Contador = 0; Contador < Grid_Observaciones.Rows.Count; Contador++) {
                DataRow Fila = Dt_Observaciones.NewRow();
                Fila["FECHA_OBSERVACION"] = Convert.ToDateTime(Grid_Observaciones.Rows[Contador].Cells[1].Text.Trim());
                Fila["AUTOR_OBSERVACION"] = HttpUtility.HtmlDecode(Grid_Observaciones.Rows[Contador].Cells[2].Text.Trim());
                Fila["OBSERVACION"] = HttpUtility.HtmlDecode(Grid_Observaciones.Rows[Contador].Cells[3].Text.Trim());
                Dt_Observaciones.Rows.Add(Fila);
            }
            DataSet Ds_Consulta = new DataSet();
            Ds_Consulta.Tables.Add(Dt_Generales);
            Ds_Consulta.Tables.Add(Dt_Observaciones);
            
            return Ds_Consulta;
        }

        #region Modal Busqueda

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Llenar_Combos_Independientes
            ///DESCRIPCIÓN: Se llenan los Combos Generales Independientes.
            ///PROPIEDADES:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 19/Enero/2010 
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
                    DataRow Fila_Dependencia = Dependencias.NewRow();
                    Fila_Dependencia["DEPENDENCIA_ID"] = "TODAS";
                    Fila_Dependencia["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                    Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
                    Cmb_MPE_Siniestros_Dependencias.DataSource = Dependencias;
                    Cmb_MPE_Siniestros_Dependencias.DataValueField = "DEPENDENCIA_ID";
                    Cmb_MPE_Siniestros_Dependencias.DataTextField = "NOMBRE";
                    Cmb_MPE_Siniestros_Dependencias.DataBind();

                    //SE LLENA EL COMBO DE ASEGURADORAS
                    Combos.P_Tipo_DataTable = "ASEGURADORAS";
                    DataTable Aseguradoras = Combos.Consultar_DataTable();
                    DataRow Fila_Aseguradora = Aseguradoras.NewRow();
                    Fila_Aseguradora["ASEGURADORA_ID"] = "TODAS";
                    Fila_Aseguradora["NOMBRE"] = HttpUtility.HtmlDecode("&lt;TODAS&gt;");
                    Aseguradoras.Rows.InsertAt(Fila_Aseguradora, 0);
                    Cmb_MPE_Siniestros_Aseguradoras.DataSource = Aseguradoras;
                    Cmb_MPE_Siniestros_Aseguradoras.DataValueField = "ASEGURADORA_ID";
                    Cmb_MPE_Siniestros_Aseguradoras.DataTextField = "NOMBRE";
                    Cmb_MPE_Siniestros_Aseguradoras.DataBind();

                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

        #endregion

        #region Validaciones

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
            ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
            ///             una operación.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 19/Enero/2011 
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************
            private bool Validar_Componentes() {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
                String Mensaje_Error = "";
                Boolean Validacion = true;
                if (Cmb_Tipo_Siniestros.SelectedIndex == 0) {
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opcion del Combo de Tipo de Siniestros.";
                    Validacion = false;
                }
                if (Hdf_Bien_ID.Value.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Seleccionar el Bien.";
                    Validacion = false;
                }
                if (Hdf_Aseguradora.Value.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ El Bien no tiene asignada alguna Aseguradora, verificarlo.";
                    //Validacion = false;
                }
                if (Txt_Fecha.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha.";
                    Validacion = false;
                }
                if (Txt_Descripcion.Text.Trim().Length == 0) {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br/>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introducir la Descripción.";
                    Validacion = false;
                }
                if (Txt_Observaciones.Text.Trim().Length > 0) {
                    if (Txt_Observaciones.Text.Trim().Length > 255) {
                        Mensaje_Error = Mensaje_Error + "+ Las Observaciones no deben sobrepasar los 255 Carácteres (Se pasa por " + (Txt_Observaciones.Text.Trim().Length-255).ToString() + ").";
                        Validacion = false;
                    }                    
                }
                if (!Validacion) {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
                return Validacion;
            }

        #endregion

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_A_Formato_ID
        ///DESCRIPCIÓN: Pasa un numero entero a Formato de ID.
        ///PARÁMETROS:     
        ///             1. Dato_ID. Dato que se desea pasar al Formato de ID.
        ///             2. Longitud_ID. Longitud que tendra el ID. 
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 10/Marzo/2010 
        ///MODIFICO             : 
        ///FECHA_MODIFICO       : 
        ///CAUSA_MODIFICACIÓN   : 
        ///*******************************************************************************
        private String Convertir_A_Formato_ID(Int32 Dato_ID, Int32 Longitud_ID) {
            String Retornar = "";
            String Dato = "" + Dato_ID;
            for (int Cont_Temp = Dato.Length; Cont_Temp < Longitud_ID; Cont_Temp++) {
                Retornar = Retornar + "0";
            }
            Retornar = Retornar + Dato;
            return Retornar;
        }

    #endregion

    #region Grids

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Siniestros_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Siniestros
        ///PARAMETROS:     
        ///CREO     : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Siniestros_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                Grid_Listado_Siniestros.SelectedIndex = (-1);
                Llenar_Grid_Listado_Siniestros(e.NewPageIndex);
                Limpiar_Catalogo();
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Observaciones_PageIndexChanging
        ///DESCRIPCIÓN: Maneja la paginación del GridView de los Siniestros
        ///PARAMETROS:     
        ///CREO     : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Observaciones_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            try {
                DataTable Tabla = new DataTable();
                if (Session["Dt_Observaciones"] != null) {
                    Tabla = (DataTable)Session["Dt_Observaciones"];
                }
                Grid_Listado_Siniestros.SelectedIndex = (-1);
                Llenar_Grid_Observaciones(e.NewPageIndex, Tabla);
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Grid_Tipos_Siniestros_SelectedIndexChanged
        ///DESCRIPCIÓN: Obtiene los datos de un Siniestro para mostrarlos a detalle
        ///PARAMETROS:     
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 18/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Grid_Listado_Siniestros_SelectedIndexChanged(object sender, EventArgs e) {
            try {
                if (Grid_Listado_Siniestros.SelectedIndex > (-1)){
                    String Siniestro_ID = Grid_Listado_Siniestros.SelectedRow.Cells[1].Text.Trim();
                    Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro = new Cls_Ope_Pat_Com_Siniestros_Negocio();
                    Siniestro.P_Siniestro_ID = Siniestro_ID;
                    Siniestro = Siniestro.Consultar_Datos_Siniestro();
                    Mostrar_Detalles_Siniestro(Siniestro);
                    System.Threading.Thread.Sleep(500);
                    Grid_Listado_Siniestros.SelectedIndex = (-1);
                }
                MPE_Busqueda_Siniestro.Hide();
            } catch(Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }
   
    #endregion
     
    #region Eventos

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
        ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Tipo de 
        ///             Siniesto.
        ///PARAMETROS:     
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Modificar_Click(object sender, EventArgs e){
            try{
                if (Btn_Modificar.AlternateText.Equals("Modificar")){
                    if (Hdf_Siniestro_ID.Value.Length > 0){
                        Configuracion_Formulario(false, "MODIFICAR");
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                        Lbl_Mensaje_Error.Text = "";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                } else {
                    if (Validar_Componentes()){
                        Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro = new Cls_Ope_Pat_Com_Siniestros_Negocio();
                        Siniestro.P_Tipo_Actualizacion = "SEGUIMIENTO";
                        Siniestro.P_Siniestro_ID = Hdf_Siniestro_ID.Value;
                        Siniestro.P_Tipo_Siniestro_ID = Cmb_Tipo_Siniestros.SelectedItem.Value;
                        Siniestro.P_Descripcion = Txt_Descripcion.Text.Trim();
                        Siniestro.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text.Trim());
                        Siniestro.P_Parte_Averiguacion = Txt_Parte_Averiguacion.Text.Trim();
                        if (Cmb_Reparacion.SelectedIndex > 0) {
                            Siniestro.P_Reparacion = Cmb_Reparacion.SelectedItem.Value.Trim();
                        }
                        Siniestro.P_Responsable_Municipio = Chk_Responsable_Municipio.Checked;
                        Siniestro.P_Estatus = Cmb_Estatus.SelectedItem.Value;
                        Siniestro.P_Observacion = Txt_Observaciones.Text.Trim();
                        Siniestro.P_Usuario_Nombre = Cls_Sessiones.Nombre_Empleado;
                        Siniestro.P_Usuario_ID = Cls_Sessiones.No_Empleado;
                        Siniestro.Modificar_Siniestro();
                        Configuracion_Formulario(true, "");
                        Limpiar_Catalogo();
                        Grid_Listado_Siniestros.DataSource = new DataTable();
                        Grid_Listado_Siniestros.DataBind();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Siniestos", "alert('Actualización de Siniesto Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Siniestro = Siniestro.Consultar_Datos_Siniestro();
                        Mostrar_Detalles_Siniestro(Siniestro);
                        System.Threading.Thread.Sleep(500);
                    }
                }
            }catch(Exception Ex){
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;                
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
        ///DESCRIPCIÓN: Carga el Modal Popup de Busqueda de Siniestros.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 19/Enero/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e) {
            Div_Contenedor_Msj_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Pnl_MPE_Buscar_Siniestro.Visible = true;
            MPE_Busqueda_Siniestro.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
        ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale 
        ///             del Formulario.
        ///PARAMETROS:     
        ///CREO : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: 19/Enero/2011 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Salir_Click(object sender, EventArgs e){
            if (Btn_Salir.AlternateText.Equals("Salir")){
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }else {
                Configuracion_Formulario(true, "");
                Limpiar_Catalogo();
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Genera el Reporte para poder imprimir el Siniestro.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 07/Septiembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e){
            try { 
                if (Hdf_Siniestro_ID.Value.Trim().Length > 0) {
                    DataSet Ds_Consulta = new DataSet();
                    Ds_Consulta = Cargar_Datos_Reporte();
                    Ds_Pat_Siniestro_Detalles Ds_Reporte = new Ds_Pat_Siniestro_Detalles();
                    Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Siniestro_Detalles.rpt");
                } else {
                    Lbl_Ecabezado_Mensaje.Text = "Es necesario:";
                    Lbl_Mensaje_Error.Text = "Seleccionar el Siniestro.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error al Generar el Reporte:";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        #region Modal Busqueda Siniestros

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Siniestros_Limpiar_Filtros_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Limpieza de los filtros
            ///             para la busqueda de siniestros.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 19/Enero/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Siniestros_Limpiar_Filtros_Click(object sender, ImageClickEventArgs e) {
                try {
                    Txt_MPE_Siniestros_Numero_Inventario.Text = "";
                    Txt_MPE_Siniestros_Clave_Sistema.Text = "";
                    Cmb_MPE_Siniestros_Dependencias.SelectedIndex = 0;
                    Cmb_MPE_Siniestros_Aseguradoras.SelectedIndex = 0;
                    Txt_MPE_Siniestros_Fecha.Text = "";
                    Cmb_MPE_Siniestros_Estatus.SelectedIndex = 0;
                    Txt_MPE_Siniestros_Descripcion.Text = "";
                    MPE_Busqueda_Siniestro.Show();
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Siniestros_Buscar_Click
            ///DESCRIPCIÓN: Maneja el Evento del Boton para realizar la Busqueda de los
            ///             siniestros.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 19/Enero/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_MPE_Siniestros_Buscar_Click(object sender, ImageClickEventArgs e) {
                try {
                    Llenar_Grid_Listado_Siniestros(0);
                } catch (Exception Ex) {
                    Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }

            ///*******************************************************************************
            ///NOMBRE DE LA FUNCIÓN: Btn_MPE_Siniestros_Buscar_Click
            ///DESCRIPCIÓN: Cierra el Modal.
            ///PARAMETROS:     
            ///CREO: Francisco Antonio Gallardo Castañeda.
            ///FECHA_CREO: 08/Julio/2011
            ///MODIFICO:
            ///FECHA_MODIFICO
            ///CAUSA_MODIFICACIÓN
            ///*******************************************************************************    
            protected void Btn_Cerrar_MPE_Siniestros_Click(object sender, ImageClickEventArgs e) {
                MPE_Busqueda_Siniestro.Hide();
            }

        #endregion

       ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Folio_Click
        ///DESCRIPCIÓN:Hace una busqueda de un Siniestro directo por No. Folio.
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: 03/Noviembre/2011
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        protected void Btn_Busqueda_Folio_Click(object sender, ImageClickEventArgs e) {
            try {
                Limpiar_Catalogo();
                String No_Folio = "";
                if (Txt_Busqueda_Folio.Text.Trim().Length > 0) {
                    No_Folio = Txt_Busqueda_Folio.Text.Trim();
                    No_Folio = Convertir_A_Formato_ID(Convert.ToInt32(No_Folio), 10);
                    Cls_Ope_Pat_Com_Siniestros_Negocio Siniestro_Negocio = new Cls_Ope_Pat_Com_Siniestros_Negocio();
                    Siniestro_Negocio.P_Siniestro_ID = No_Folio;
                    Siniestro_Negocio = Siniestro_Negocio.Consultar_Datos_Siniestro();
                    if (Siniestro_Negocio.P_Siniestro_ID != null && Siniestro_Negocio.P_Siniestro_ID.Trim().Length > 0) {
                        Mostrar_Detalles_Siniestro(Siniestro_Negocio);
                    } else {
                        Lbl_Ecabezado_Mensaje.Text = "No se encontró el No. Folio";
                        Lbl_Mensaje_Error.Text = "[" + No_Folio + "]";
                        Div_Contenedor_Msj_Error.Visible = true;                        
                    }
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Error al Buscar";
                Lbl_Mensaje_Error.Text = "[" + Ex.Message + "]";
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
                Botones.Add(Btn_Modificar);

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
        protected void Configuracion_Acceso_LinkButton(String URL_Pagina)
        {
            List<LinkButton> Botones = new List<LinkButton>();//Variable que almacenara una lista de los botones de la página.
            DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

            try
            {
                //Agregamos los botones a la lista de botones de la página.
                Botones.Add(Btn_Busqueda_Avanzada);

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