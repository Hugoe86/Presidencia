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
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Control_Patrimonial_Catalogo_Clases_Activos.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Usos_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Destinos_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Operacion_Bienes_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Origenes_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Areas_Donacion.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Clasificaciones_Zonas_Inmuebles.Negocio;
using Presidencia.Control_Patrimonial_Catalogo_Orientaciones_Inmuebles.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Colonias.Negocios;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using System.Text;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;

public partial class paginas_Control_Patrimonial_Frm_Rpt_Pat_Listado_Bienes_Inmuebles : System.Web.UI.Page{

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Page_Load
        ///DESCRIPCIÓN: Evento que se carga cuando la Pagina de Inicia.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012
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
                Llenar_Combo_Destino();
                Llenar_Combo_Tipo_Predio();
                Llenar_Combo_Uso();
                Llenar_Combo_Origenes();
                Llenar_Combo_Areas_Donacion();
                Llenar_Combo_Sectores();
                Llenar_Combo_Clasificaciones_Zonas();
                Llenar_Combo_Clase_Activo();
            }
        }

    #endregion

    #region "Metodos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Uso
        ///DESCRIPCIÓN: Llena el Combo de Usos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        private void Llenar_Combo_Uso() {
            Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio Uso_Suelo = new Cls_Cat_Pat_Com_Usos_Inmuebles_Negocio();
            Uso_Suelo.P_Estatus = "VIGENTE";
            Cmb_Uso.DataSource = Uso_Suelo.Consultar_Usos();
            Cmb_Uso.DataTextField = "DESCRIPCION";
            Cmb_Uso.DataValueField = "USO_ID";
            Cmb_Uso.DataBind();
            Cmb_Uso.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Uso.Items.Insert(0, new ListItem("<TODOS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Destino
        ///DESCRIPCIÓN: Llena el Combo de Destinos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Destino() {
            Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio Destino_Suelo = new Cls_Cat_Pat_Com_Destinos_Inmuebles_Negocio();
            Destino_Suelo.P_Estatus = "VIGENTE";
            Cmb_Destino.DataSource = Destino_Suelo.Consultar_Destinos();
            Cmb_Destino.DataTextField = "DESCRIPCION";
            Cmb_Destino.DataValueField = "DESTINO_ID";
            Cmb_Destino.DataBind();
            Cmb_Destino.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Destino.Items.Insert(0, new ListItem("<TODOS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Tipo_Predio
        ///DESCRIPCIÓN: Llena el Combo de Tipos de Predio
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************      
        private void Llenar_Combo_Tipo_Predio() {
            Cls_Cat_Pre_Tipos_Predio_Negocio Tipo_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
            Cmb_Tipo_Predio.DataSource = Tipo_Predio.Consultar_Tipo_Predio();
            Cmb_Tipo_Predio.DataTextField = "DESCRIPCION";
            Cmb_Tipo_Predio.DataValueField = "TIPO_PREDIO_ID";
            Cmb_Tipo_Predio.DataBind();
            Cmb_Tipo_Predio.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Tipo_Predio.Items.Insert(0, new ListItem("<TODOS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Calles
        ///DESCRIPCIÓN: Llena el Combo de Calles
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Calles() {
            Grid_Listado_Calles.SelectedIndex = (-1);
            Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
            Calles.P_Nombre_Calle = Txt_Nombre_Calles_Buscar.Text.Trim().ToUpper();
            DataTable Resultados_Calles = Calles.Consultar_Nombre();
            Resultados_Calles.Columns[Cat_Pre_Calles.Campo_Calle_ID].ColumnName = "CALLE_ID";
            Resultados_Calles.Columns[Cat_Pre_Calles.Campo_Nombre].ColumnName = "NOMBRE_CALLE";
            Grid_Listado_Calles.Columns[1].Visible = true;
            Grid_Listado_Calles.DataSource = Resultados_Calles;
            Grid_Listado_Calles.DataBind();
            Grid_Listado_Calles.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Cuentas_Predial
        ///DESCRIPCIÓN: Llena el Combo de Cuentas Predial
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Listado_Cuentas_Predial() {
            Grid_Listado_Cuentas_Predial.SelectedIndex = (-1);
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuentas_Predial.P_Cuenta_Predial = Txt_Nombre_Cuenta_Predial_Buscar.Text.Trim().ToUpper();
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            DataTable Resultados_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            Grid_Listado_Cuentas_Predial.Columns[1].Visible = true;
            Grid_Listado_Cuentas_Predial.DataSource = Resultados_Cuentas_Predial;
            Grid_Listado_Cuentas_Predial.DataBind();
            Grid_Listado_Cuentas_Predial.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Listado_Colonias
        ///DESCRIPCIÓN: Llena el Combo de Colonias
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///******************************************************************************* 
        private void Llenar_Listado_Colonias() {
            Grid_Listado_Colonias.SelectedIndex = (-1);
            Cls_Cat_Ate_Colonias_Negocio Colonias_Negocio = new Cls_Cat_Ate_Colonias_Negocio();
            Colonias_Negocio.P_Nombre = Txt_Nombre_Colonia_Buscar.Text.Trim().ToUpper();
            DataTable Resultados_Colonias = Colonias_Negocio.Consulta_Datos().Tables[0];
            Resultados_Colonias.Columns[Cat_Ate_Colonias.Campo_Nombre].ColumnName = "NOMBRE_COLONIA";
            Resultados_Colonias.DefaultView.Sort = "NOMBRE_COLONIA";
            Grid_Listado_Colonias.Columns[1].Visible = true;
            Grid_Listado_Colonias.DataSource = Resultados_Colonias;
            Grid_Listado_Colonias.DataBind();
            Grid_Listado_Colonias.Columns[1].Visible = false;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Origenes
        ///DESCRIPCIÓN: Llena el Combo de Origenes
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        private void Llenar_Combo_Origenes() {
            Cls_Cat_Pat_Com_Origenes_Inmuebles_Negocio Destino_Suelo = new Cls_Cat_Pat_Com_Origenes_Inmuebles_Negocio();
            Destino_Suelo.P_Estatus = "VIGENTE";
            Cmb_Origen.DataSource = Destino_Suelo.Consultar_Origenes();
            Cmb_Origen.DataTextField = "NOMBRE";
            Cmb_Origen.DataValueField = "ORIGEN_ID";
            Cmb_Origen.DataBind();
            Cmb_Origen.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Origen.Items.Insert(0, new ListItem("<TODOS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Areas_Donacion
        ///DESCRIPCIÓN: Llena el Combo de Areas de Donación
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        private void Llenar_Combo_Areas_Donacion() {
            Cls_Cat_Pat_Com_Areas_Donacion_Negocio Area_Negocio = new Cls_Cat_Pat_Com_Areas_Donacion_Negocio();
            Area_Negocio.P_Estatus = "VIGENTE";
            Cmb_Area_Donacion.DataSource = Area_Negocio.Consultar_Areas();
            Cmb_Area_Donacion.DataTextField = "DESCRIPCION";
            Cmb_Area_Donacion.DataValueField = "AREA_ID";
            Cmb_Area_Donacion.DataBind();
            Cmb_Area_Donacion.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Area_Donacion.Items.Insert(0, new ListItem("<TODAS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Sectores
        ///DESCRIPCIÓN: Llena el Combo de Sectores
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Sectores() {
            Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio Sectores = new Cls_Cat_Pat_Com_Orientaciones_Inmuebles_Negocio();
            Sectores.P_Estatus = "VIGENTE";
            Cmb_Sector.DataSource = Sectores.Consultar_Orientaciones();
            Cmb_Sector.DataTextField = "DESCRIPCION";
            Cmb_Sector.DataValueField = "ORIENTACION_ID";
            Cmb_Sector.DataBind();
            Cmb_Sector.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Sector.Items.Insert(0, new ListItem("<TODOS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Clasificaciones_Zonas
        ///DESCRIPCIÓN: Llena el Combo de Clasificaciones de Zonas
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Clasificaciones_Zonas() {
            Cls_Cat_Pat_Com_Clasificaciones_Zonas_Inmuebles_Negocio Negocio = new Cls_Cat_Pat_Com_Clasificaciones_Zonas_Inmuebles_Negocio();
            Negocio.P_Estatus = "VIGENTE";
            Cmb_Clasificacion_Zona.DataSource = Negocio.Consultar_Clasificaciones();
            Cmb_Clasificacion_Zona.DataTextField = "DESCRIPCION";
            Cmb_Clasificacion_Zona.DataValueField = "CLASIFICACION_ID";
            Cmb_Clasificacion_Zona.DataBind();
            Cmb_Clasificacion_Zona.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Clasificacion_Zona.Items.Insert(0, new ListItem("<TODAS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Clase_Activo
        ///DESCRIPCIÓN: Llena el Combo de Clases de Activos
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Llenar_Combo_Clase_Activo() {
            Cls_Cat_Pat_Com_Clases_Activo_Negocio CA_Negocio = new Cls_Cat_Pat_Com_Clases_Activo_Negocio();
            CA_Negocio.P_Estatus = "VIGENTE";
            CA_Negocio.P_Tipo_DataTable = "CLASES_ACTIVOS";
            Cmb_Clase_Activo.DataSource = CA_Negocio.Consultar_DataTable();
            Cmb_Clase_Activo.DataValueField = "CLASE_ACTIVO_ID";
            Cmb_Clase_Activo.DataTextField = "CLAVE_DESCRIPCION";
            Cmb_Clase_Activo.DataBind();
            Cmb_Clase_Activo.Items.Insert(0, new ListItem("<SIN ASIGNACIÓN>", "SIN_ASIGNACION"));
            Cmb_Clase_Activo.Items.Insert(0, new ListItem("<TODOS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Reporte
        ///DESCRIPCIÓN: Carga los Datos del Reporte
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Cargar_Datos_Reporte(String Tipo) {
            Cls_Rpt_Pat_Listado_Bienes_Negocio Listado_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            Listado_Negocio.P_Escritura = Txt_Escritura.Text.Trim();
            Listado_Negocio.P_Calle_ID = Hdf_Calle_ID.Value;
            Listado_Negocio.P_Colonia_ID = Hdf_Colonia_ID.Value;
            if (Cmb_Uso.SelectedIndex == 1) Listado_Negocio.P_Sin_Uso = true; else Listado_Negocio.P_Uso_ID = Cmb_Uso.SelectedItem.Value;
            if (Cmb_Destino.SelectedIndex == 1) Listado_Negocio.P_Sin_Destino = true; else Listado_Negocio.P_Destino_ID = Cmb_Destino.SelectedItem.Value;
            if (Cmb_Origen.SelectedIndex == 1) Listado_Negocio.P_Sin_Origen = true; else Listado_Negocio.P_Origen_ID = Cmb_Origen.SelectedItem.Value;
            if (Cmb_Estatus.SelectedIndex == 1) Listado_Negocio.P_Sin_Estatus = true; else Listado_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;
            if (Cmb_Area_Donacion.SelectedIndex == 1) Listado_Negocio.P_Sin_Areas_Donacion = true; else Listado_Negocio.P_Area_Donacion = Cmb_Area_Donacion.SelectedItem.Value;
            if (Cmb_Tipo_Bien.SelectedIndex == 1) Listado_Negocio.P_Sin_Tipo_Bien = true; else Listado_Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value;
            if (Cmb_Sector.SelectedIndex == 1) Listado_Negocio.P_Sin_Sector = true; else Listado_Negocio.P_Sector = Cmb_Sector.SelectedItem.Value;
            if (Cmb_Clasificacion_Zona.SelectedIndex == 1) Listado_Negocio.P_Sin_Clasificacion_Zona = true; else Listado_Negocio.P_Clasificacion_ID = Cmb_Clasificacion_Zona.SelectedItem.Value;
            if (Cmb_Clase_Activo.SelectedIndex == 1) Listado_Negocio.P_Sin_Clase_Activo = true; else Listado_Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value;
            Listado_Negocio.P_Estado = Cmb_Estado.SelectedItem.Value;
            Listado_Negocio.P_Libre_Gravamen = Cmb_Libertad_Gravament.SelectedItem.Value;
            Listado_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            if (Cmb_Tipo_Predio.SelectedIndex == 1) Listado_Negocio.P_Sin_Tipo_Predio = true; else Listado_Negocio.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedItem.Value;
            if (Txt_Superficie_Desde.Text.Trim().Length > 0) { Listado_Negocio.P_Superficie_Inicial = Convert.ToDouble(Txt_Superficie_Desde.Text); }
            if (Txt_Superficie_Hasta.Text.Trim().Length > 0 ) { Listado_Negocio.P_Superficie_Final = Convert.ToDouble(Txt_Superficie_Hasta.Text); }
            if (Txt_Valor_Comercial_Desde.Text.Trim().Length > 0) { Listado_Negocio.P_Valor_Comercial_Inicial = Convert.ToDouble(Txt_Valor_Comercial_Desde.Text); }
            if (Txt_Valor_Comercial_Hasta.Text.Trim().Length > 0 ) { Listado_Negocio.P_Valor_Comercial_Final = Convert.ToDouble(Txt_Valor_Comercial_Hasta.Text); }
            if (Txt_Fecha_Registro_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Registro_Inicio.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Registral_Inicial = Convert.ToDateTime(Txt_Fecha_Registro_Inicio.Text); }
            if (Txt_Fecha_Registro_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Registro_Fin.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Registral_Final = Convert.ToDateTime(Txt_Fecha_Registro_Fin.Text); }
            if (Txt_Fecha_Escritura_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Escritura_Inicio.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Escritura_Inicial = Convert.ToDateTime(Txt_Fecha_Escritura_Inicio.Text); }
            if (Txt_Fecha_Escritura_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Escritura_Fin.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Escritura_Final = Convert.ToDateTime(Txt_Fecha_Escritura_Fin.Text); }
            if (Txt_Fecha_Baja_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Baja_Inicio.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Baja_Inicial = Convert.ToDateTime(Txt_Fecha_Baja_Inicio.Text); }
            if (Txt_Fecha_Baja_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Baja_Fin.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Baja_Final = Convert.ToDateTime(Txt_Fecha_Baja_Fin.Text); }
            Listado_Negocio.P_Bien_ID = (Txt_Bien_Mueble_ID.Text.Trim().Length > 0) ? String.Format("{0:0000000000}", Convert.ToInt32(Txt_Bien_Mueble_ID.Text.Trim())) : "";
            DataTable Dt_Bienes_Inmuebles = Listado_Negocio.Consultar_Bienes_Inmuebles();
            if (Tipo.Trim().Equals("PDF")) {
                Dt_Bienes_Inmuebles.TableName = "DT_LISTADO_BIENES";
                DataSet Ds_Consulta = new DataSet();
                Ds_Pat_Listado_Bienes_Inmuebles Ds_Reporte = new Ds_Pat_Listado_Bienes_Inmuebles();
                Ds_Consulta.Tables.Add(Dt_Bienes_Inmuebles.Copy());
                Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Bienes_Inmuebles.rpt");
            } else if (Tipo.Trim().Equals("EXCEL")) {
                Adecuar_Tabla_A_Exportar(ref Dt_Bienes_Inmuebles);
                Pasar_DataTable_A_Excel(Dt_Bienes_Inmuebles);
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
            String Valor_Fiscal_Total = "";
            if (Chk_Valor_Total_Comercial.Checked) { Valor_Fiscal_Total = "Valor Total Fiscal: " + String.Format("{0:c}", Obtener_Valor_Fiscal_Total(Data_Set_Consulta_DB.Tables["DT_LISTADO_BIENES"])); }
            if (!Chk_Inluir_Columna_Valores_Fiscales.Checked) {
                Data_Set_Consulta_DB.Tables["DT_LISTADO_BIENES"].Columns.Remove("VALOR_FISCAL");
                Data_Set_Consulta_DB.Tables["DT_LISTADO_BIENES"].Columns.Add("VALOR_FISCAL", Type.GetType("System.Double"));
            }
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            String Nombre_Reporte_Generar = "Rpt_Pat_Listado_Bienes_Inmuebles" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
            String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            Reporte.SetParameterValue("TOTAL_REGISTROS", Data_Set_Consulta_DB.Tables["DT_LISTADO_BIENES"].Rows.Count);
            Reporte.SetParameterValue("TOTAL_VALOR_FISCAL", Valor_Fiscal_Total);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
            Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
        }

        /// *************************************************************************************
        /// NOMBRE:              Mostrar_Reporte
        /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
        /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
        ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
        /// USUARIO CREO:        Juan Alberto Hernández Negrete.
        /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
        /// USUARIO MODIFICO:    Salvador Hernández Ramírez
        /// FECHA MODIFICO:      23-Mayo-2011
        /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
        /// *************************************************************************************
        protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
        {
            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            try
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            catch (Exception Ex)
            {
                throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Obtener_Valor_Fiscal_Total
        ///DESCRIPCIÓN: Obtiene el Valor Total Final Fiscal
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Julio/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private Double Obtener_Valor_Fiscal_Total(DataTable Dt_Datos) {
            Double Valor_Total = 0.0;
            foreach (DataRow Fila in Dt_Datos.Rows) {
                if (!String.IsNullOrEmpty(Fila["VALOR_FISCAL"].ToString())) {
                    Valor_Total = Valor_Total + Convert.ToDouble(Fila["VALOR_FISCAL"]);
                }
            }
            return Valor_Total;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Reporte
        ///DESCRIPCIÓN: Carga los Datos del Reporte
        ///PROPIEDADES:     
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        private void Adecuar_Tabla_A_Exportar(ref DataTable Dt_Registros) {
            Dt_Registros.Columns["CALLE"].ColumnName = "Calle";
            Dt_Registros.Columns["NUMERO_EXTERIOR"].ColumnName = "# Ext.";
            Dt_Registros.Columns["NUMERO_INTERIOR"].ColumnName = "# Int.";
            Dt_Registros.Columns["COLONIA"].ColumnName = "Colonia";
            Dt_Registros.Columns["CUENTA_PREDIAL"].ColumnName = "Cuenta Predial";
            Dt_Registros.Columns["MANZANA"].ColumnName = "Mzn.";
            Dt_Registros.Columns["LOTE"].ColumnName = "Lote";
            Dt_Registros.Columns["SUPERFICIE"].ColumnName = "Superficie [m2]";
            Dt_Registros.Columns["VALOR_FISCAL"].ColumnName = "Valor Fiscal [$]";
            Dt_Registros.Columns["VALOR_COMERCIAL"].ColumnName = "Valor Comercial [$]";
            Dt_Registros.Columns["USO_INMUEBLE"].ColumnName = "Uso";
            Dt_Registros.Columns["AREA_DONACION"].ColumnName = "Área Donación";
            Dt_Registros.Columns["ESTADO"].ColumnName = "Estado";
            Dt_Registros.Columns["ESTATUS"].ColumnName = "Estatus";

            Dt_Registros.Columns.Remove("BIEN_INMUEBLE_ID");
            Dt_Registros.Columns.Remove("AREA_DONACION_ID");
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
        public void Pasar_DataTable_A_Excel(System.Data.DataTable Dt_Reporte) {
            String Ruta = "Reporte de Bienes Inmuebles " + String.Format("{0:ddMMyyyyhhmmsstt}", DateTime.Now) + ".xls";

            try {
                //Creamos el libro de Excel.
                CarlosAg.ExcelXmlWriter.Workbook Libro = new CarlosAg.ExcelXmlWriter.Workbook();

                Libro.Properties.Title = "Reporte";
                Libro.Properties.Created = DateTime.Now;
                Libro.Properties.Author = "Control Patrimonial";

                //Creamos una hoja que tendrá el libro.
                CarlosAg.ExcelXmlWriter.Worksheet Hoja = Libro.Worksheets.Add("Registros");
                //Agregamos un renglón a la hoja de excel.
                CarlosAg.ExcelXmlWriter.WorksheetRow Renglon = Hoja.Table.Rows.Add();
                //Creamos el estilo cabecera para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Cabecera = Libro.Styles.Add("HeaderStyle");
                //Creamos el estilo contenido para la hoja de excel. 
                CarlosAg.ExcelXmlWriter.WorksheetStyle Estilo_Contenido = Libro.Styles.Add("BodyStyle");

                Estilo_Cabecera.Font.FontName = "Tahoma";
                Estilo_Cabecera.Font.Size = 10;
                Estilo_Cabecera.Font.Bold = true;
                Estilo_Cabecera.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Cabecera.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Cabecera.Font.Color = "#FFFFFF";
                Estilo_Cabecera.Interior.Color = "#193d61";
                Estilo_Cabecera.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Cabecera.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_Cabecera.Alignment.WrapText = true;

                Estilo_Contenido.Font.FontName = "Tahoma";
                Estilo_Contenido.Font.Size = 8;
                Estilo_Contenido.Font.Bold = true;
                Estilo_Contenido.Alignment.Horizontal = StyleHorizontalAlignment.Center;
                Estilo_Contenido.Alignment.Vertical = StyleVerticalAlignment.Center;
                Estilo_Contenido.Font.Color = "#000000";
                Estilo_Contenido.Interior.Color = "White";
                Estilo_Contenido.Interior.Pattern = StyleInteriorPattern.Solid;
                Estilo_Contenido.Borders.Add(StylePosition.Top, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Bottom, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Left, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Borders.Add(StylePosition.Right, LineStyleOption.Continuous, 1, "Black");
                Estilo_Contenido.Alignment.WrapText = true;

                //Agregamos las columnas que tendrá la hoja de excel.
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(250));//Calle
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//Numero Exterior
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//Numero Interior
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Colonia
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Cuenta Predial
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//Manzana
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(60));//Lote
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(120));//Superficie
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//Valor Fiscal
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(105));//Valor Comercial
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Uso
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(100));//Area donacion
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//Estado
                Hoja.Table.Columns.Add(new CarlosAg.ExcelXmlWriter.WorksheetColumn(80));//Estatus

                if (Dt_Reporte is System.Data.DataTable) {
                    if (Dt_Reporte.Rows.Count > 0) {
                        foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns) {
                            if (COLUMNA is System.Data.DataColumn) {
                                Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(COLUMNA.ColumnName, "HeaderStyle"));
                            }
                            Renglon.Height = 25;
                        }

                        foreach (System.Data.DataRow FILA in Dt_Reporte.Rows) {
                            if (FILA is System.Data.DataRow) {
                                Renglon = Hoja.Table.Rows.Add();

                                foreach (System.Data.DataColumn COLUMNA in Dt_Reporte.Columns) {
                                    if (COLUMNA is System.Data.DataColumn) {
                                        if (COLUMNA.ColumnName.Equals("Valor Fiscal [$]") || COLUMNA.ColumnName.Equals("Valor Comercial [$]")) {
                                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.Number, "BodyStyle"));
                                        } else {
                                            Renglon.Cells.Add(new CarlosAg.ExcelXmlWriter.WorksheetCell(FILA[COLUMNA.ColumnName].ToString(), DataType.String, "BodyStyle"));
                                        }
                                    }
                                }
                                Renglon.Height = 25;
                                Renglon.AutoFitHeight = true;
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

    #endregion

    #region "Grids"

        protected void Grid_Listado_Calles_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Calles.PageIndex = e.NewPageIndex;
            Llenar_Listado_Calles();
        }

        protected void Grid_Listado_Calles_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Calles.SelectedIndex > (-1)) {
                Hdf_Calle_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Calles.SelectedRow.Cells[1].Text.Trim());
                Txt_Calle.Text = HttpUtility.HtmlDecode(Grid_Listado_Calles.SelectedRow.Cells[2].Text.Trim());
                Mpe_Calles_Cabecera.Hide();
            }
        }

        protected void Grid_Listado_Cuentas_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Cuentas_Predial.PageIndex = e.NewPageIndex;
            Llenar_Listado_Cuentas_Predial();
        }

        protected void Grid_Listado_Cuentas_Predial_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Cuentas_Predial.SelectedIndex > (-1)) {
                Hdf_Cuenta_Predial_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Cuentas_Predial.SelectedRow.Cells[1].Text.Trim());
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0) {
                    Cls_Cat_Pre_Cuentas_Predial_Negocio CP_Negocio = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                    CP_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value.Trim();
                    CP_Negocio.P_Incluir_Campos_Foraneos = true;
                    DataTable DT_Cuentas_Predial = CP_Negocio.Consultar_Cuenta();
                    if (DT_Cuentas_Predial != null && DT_Cuentas_Predial.Rows.Count > 0) {
                        Txt_Numero_Cuenta_Predial.Text = DT_Cuentas_Predial.Rows[0]["CUENTA_PREDIAL"].ToString().Trim();
                    }
                }
                Mpe_Cuentas_Predial_Cabecera.Hide();
            }
        }

        protected void Grid_Listado_Colonias_PageIndexChanging(object sender, GridViewPageEventArgs e) {
            Grid_Listado_Colonias.PageIndex = e.NewPageIndex;
            Llenar_Listado_Colonias();
        }

        protected void Grid_Listado_Colonias_SelectedIndexChanged(object sender, EventArgs e) {
            if (Grid_Listado_Colonias.SelectedIndex > (-1)) {
                Hdf_Colonia_ID.Value = HttpUtility.HtmlDecode(Grid_Listado_Colonias.SelectedRow.Cells[1].Text.Trim());
                Txt_Colonia.Text = HttpUtility.HtmlDecode(Grid_Listado_Colonias.SelectedRow.Cells[2].Text.Trim());
                Mpe_Colonias.Hide();
            }
        }

    #endregion

    #region "Eventos"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
        ///DESCRIPCIÓN: Lanza el Reporte en PDF para Imprimir.
        ///PARAMETROS:     
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
            try {
                Cargar_Datos_Reporte("PDF");
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
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************    
        protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e){
            try {
                Cargar_Datos_Reporte("EXCEL");
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
    }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Calles_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de Calles
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        protected void Btn_Ejecutar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Calles.PageIndex = 0;
            Llenar_Listado_Calles();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Calles_Buscar_TextChanged
        ///DESCRIPCIÓN: Ejecuta la Busqueda de Calles
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************   
        protected void Txt_Nombre_Calles_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Calles.PageIndex = 0;
                Llenar_Listado_Calles();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calle_Click
        ///DESCRIPCIÓN: Lanza el Buscador de Calles
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Btn_Buscar_Calle_Click(object sender, ImageClickEventArgs e) {
            Mpe_Calles_Cabecera.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Cuenta_Predial_Click
        ///DESCRIPCIÓN: Ejecuta la Busqueda de Cuentas Predial
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Btn_Ejecutar_Busqueda_Cuenta_Predial_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Cuentas_Predial.PageIndex = 0;
            Llenar_Listado_Cuentas_Predial();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Cuenta_Predial_Buscar_TextChanged
        ///DESCRIPCIÓN: Ejecuta la Busqueda de Cuentas Predial
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Txt_Nombre_Cuenta_Predial_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Cuentas_Predial.PageIndex = 0;
                Llenar_Listado_Cuentas_Predial();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Numero_Cuenta_Predial_Click
        ///DESCRIPCIÓN:  Lanza el Buscador de Cuentas Predial
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Btn_Buscar_Numero_Cuenta_Predial_Click(object sender, ImageClickEventArgs e) {
            Mpe_Cuentas_Predial_Cabecera.Show();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Ejecutar_Busqueda_Colonia_Click
        ///DESCRIPCIÓN: Ejecuta la busqueda de Colonias
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Btn_Ejecutar_Busqueda_Colonia_Click(object sender, ImageClickEventArgs e) {
            Grid_Listado_Colonias.PageIndex = 0;
            Llenar_Listado_Colonias();
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Txt_Nombre_Colonia_Buscar_TextChanged
        ///DESCRIPCIÓN: Ejecuta la busqueda de Colonias
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Txt_Nombre_Colonia_Buscar_TextChanged(object sender, EventArgs e) {
            try {
                Grid_Listado_Colonias.PageIndex = 0;
                Llenar_Listado_Colonias();
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
        ///DESCRIPCIÓN: Lanza el Buscador de Colonias
        ///PARAMETROS:
        ///CREO: Francisco Antonio Gallardo Castañeda.
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************  
        protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e) {
            Mpe_Colonias.Show();
        }

    #endregion

}