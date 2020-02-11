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
using System.Drawing;
using System.IO;

public partial class paginas_Control_Patrimonial_Frm_Rpt_Pat_Listado_Fichas_Tecnicas : System.Web.UI.Page {

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
            Cmb_Clase_Activo.Items.Insert(0, new ListItem("<TODOS>", ""));
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Cargar_Tablas_Reporte
        ///DESCRIPCIÓN: Maneja las tablas del Reporte de Ficha Tecnica
        ///PROPIEDADES:   1.  P_Imagen.  Imagen a Convertir.    
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private void Cargar_Tablas_Reporte(String Tipo) {
            Cls_Rpt_Pat_Listado_Bienes_Negocio Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            Reporte_Negocio.P_Escritura = Txt_Escritura.Text.Trim();
            Reporte_Negocio.P_Calle_ID = Hdf_Calle_ID.Value;
            Reporte_Negocio.P_Colonia_ID = Hdf_Colonia_ID.Value;
            Reporte_Negocio.P_Uso_ID = Cmb_Uso.SelectedItem.Value;
            Reporte_Negocio.P_Destino_ID = Cmb_Destino.SelectedItem.Value;
            Reporte_Negocio.P_Origen_ID = Cmb_Origen.SelectedItem.Value;
            Reporte_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;
            Reporte_Negocio.P_Area_Donacion = Cmb_Area_Donacion.SelectedItem.Value;
            Reporte_Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value;
            Reporte_Negocio.P_Sector = Cmb_Sector.SelectedItem.Value;
            Reporte_Negocio.P_Clasificacion_ID = Cmb_Clasificacion_Zona.SelectedItem.Value;
            Reporte_Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value;
            Reporte_Negocio.P_Estado = Cmb_Estado.SelectedItem.Value;
            Reporte_Negocio.P_Libre_Gravamen = Cmb_Libertad_Gravament.SelectedItem.Value;
            Reporte_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Reporte_Negocio.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedItem.Value;
            if (Txt_Superficie_Desde.Text.Trim().Length > 0) { Reporte_Negocio.P_Superficie_Inicial = Convert.ToDouble(Txt_Superficie_Desde.Text); }
            if (Txt_Superficie_Hasta.Text.Trim().Length > 0) { Reporte_Negocio.P_Superficie_Final = Convert.ToDouble(Txt_Superficie_Hasta.Text); }
            if (Txt_Fecha_Registro_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Registro_Inicio.Text.Trim().Equals("__/___/____")) { Reporte_Negocio.P_Fecha_Registral_Inicial = Convert.ToDateTime(Txt_Fecha_Registro_Inicio.Text); }
            if (Txt_Fecha_Registro_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Registro_Fin.Text.Trim().Equals("__/___/____")) { Reporte_Negocio.P_Fecha_Registral_Final = Convert.ToDateTime(Txt_Fecha_Registro_Fin.Text); }
            if (Txt_Fecha_Escritura_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Escritura_Inicio.Text.Trim().Equals("__/___/____")) { Reporte_Negocio.P_Fecha_Escritura_Inicial = Convert.ToDateTime(Txt_Fecha_Escritura_Inicio.Text); }
            if (Txt_Fecha_Escritura_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Escritura_Fin.Text.Trim().Equals("__/___/____")) { Reporte_Negocio.P_Fecha_Escritura_Final = Convert.ToDateTime(Txt_Fecha_Escritura_Fin.Text); }
            if (Txt_Fecha_Baja_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Baja_Inicio.Text.Trim().Equals("__/___/____")) { Reporte_Negocio.P_Fecha_Baja_Inicial = Convert.ToDateTime(Txt_Fecha_Baja_Inicio.Text); }
            if (Txt_Fecha_Baja_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Baja_Fin.Text.Trim().Equals("__/___/____")) { Reporte_Negocio.P_Fecha_Baja_Final = Convert.ToDateTime(Txt_Fecha_Baja_Fin.Text); }
            Reporte_Negocio.P_Bien_ID = (Txt_Bien_Mueble_ID.Text.Trim().Length > 0) ? String.Format("{0:0000000000}", Convert.ToInt32(Txt_Bien_Mueble_ID.Text.Trim())) : "";
            DataTable Dt_Datos_Generales_Reporte = Reporte_Negocio.Consultar_Datos_Generales_BI_Ficha_Tecnica();
            DataTable Dt_Datos_Medidas_Colindancias_Reporte = Reporte_Negocio.Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica();
            Dt_Datos_Generales_Reporte.Columns.Add("FOTO", Type.GetType("System.Byte[]"));
            Dt_Datos_Generales_Reporte.Columns.Add("MAPA", Type.GetType("System.Byte[]"));
            Dt_Datos_Generales_Reporte.Columns.Add("COMPLEMENTOS", Type.GetType("System.String"));
            Dt_Datos_Generales_Reporte.Columns.Add("LEVANTAMIENTO_TOPOGRAFICO", Type.GetType("System.Byte[]"));
            StringBuilder Bienes_Inmuebles_ID = new StringBuilder();
            for (Int32 Contador = 0; Contador < Dt_Datos_Generales_Reporte.Rows.Count; Contador++) {
                if (Contador > 0) {
                    Bienes_Inmuebles_ID.Append(",'");
                }
                Bienes_Inmuebles_ID.Append(Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim() + "'");
                //Se carga la foto
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Reporte_Negocio.P_Tipo = "FOTOGRAFIA";
                DataTable Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Archivos_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    String Nombre_Archivo = Dt_Tmp.Rows[0]["RUTA_ARCHIVO"].ToString().Trim();
                    String Directorio = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + Dt_Tmp.Rows[0]["BIEN_INMUEBLE_ID"].ToString().Trim() + "/FOTOGRAFIA");
                    String Nombre_Completo_Archivo = Directorio + "/" + Nombre_Archivo;
                    if (File.Exists(Nombre_Completo_Archivo)) {
                        Dt_Datos_Generales_Reporte.Rows[Contador]["FOTO"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Nombre_Completo_Archivo));
                    }
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;

                //Se carga el mapa
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Reporte_Negocio.P_Tipo = "MAPA";
                Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Archivos_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    String Nombre_Archivo = Dt_Tmp.Rows[0]["RUTA_ARCHIVO"].ToString().Trim();
                    String Directorio = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + Dt_Tmp.Rows[0]["BIEN_INMUEBLE_ID"].ToString().Trim() + "/MAPA");
                    String Nombre_Completo_Archivo = Directorio + "/" + Nombre_Archivo;
                    if (File.Exists(Nombre_Completo_Archivo)) {
                        Dt_Datos_Generales_Reporte.Rows[Contador]["MAPA"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Nombre_Completo_Archivo));
                    }
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;

                //Se carga el Levantamiento Topografico
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Reporte_Negocio.P_Tipo = "LEVANTAMIENTO_TOPOGRAFICO";
                Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Archivos_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    String Nombre_Archivo = Dt_Tmp.Rows[0]["RUTA_ARCHIVO"].ToString().Trim();
                    String Directorio = Server.MapPath(Ope_Pat_B_Inm_Archivos.Ruta_Archivos_Inmuebles + Dt_Tmp.Rows[0]["BIEN_INMUEBLE_ID"].ToString().Trim() + "/LEVANTAMIENTO_TOPOGRAFICO");
                    String Nombre_Completo_Archivo = Directorio + "/" + Nombre_Archivo;
                    if (File.Exists(Nombre_Completo_Archivo)) {
                        Dt_Datos_Generales_Reporte.Rows[Contador]["LEVANTAMIENTO_TOPOGRAFICO"] = Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image.FromFile(Nombre_Completo_Archivo));
                    }
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;

                //Se cargan los complementos
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = true;
                Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
                Reporte_Negocio.P_Bien_ID = Dt_Datos_Generales_Reporte.Rows[Contador]["BIEN_INMUEBLE_ID"].ToString().Trim();
                Dt_Tmp = null;
                Dt_Tmp = Reporte_Negocio.Consultar_Datos_Observaciones_BI_Ficha_Tecnica();
                if (Dt_Tmp != null && Dt_Tmp.Rows.Count > 0) {
                    Dt_Datos_Generales_Reporte.Rows[Contador].BeginEdit();
                    Dt_Datos_Generales_Reporte.Rows[Contador]["COMPLEMENTOS"] = Dt_Tmp.Rows[0]["OBSERVACION"].ToString();
                    Dt_Datos_Generales_Reporte.Rows[Contador].EndEdit();
                }
                Dt_Datos_Generales_Reporte.DefaultView.AllowEdit = false;
            }

            //Se Consultan las Medidas y Colindancias
            Reporte_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            Reporte_Negocio.P_Bien_ID = Bienes_Inmuebles_ID.ToString().Trim('\'');
            DataTable Dt_Tmp_ = null;
            Dt_Tmp_ = Reporte_Negocio.Consultar_Datos_Medidas_Colindancias_BI_Ficha_Tecnica();
            Dt_Datos_Generales_Reporte.TableName = "DT_GENERALES";
            Dt_Tmp_.TableName = "DT_MEDIAS_COLINDANCIAS";
            DataSet Ds_Consulta = new DataSet();
            Ds_Consulta.Tables.Add(Dt_Datos_Generales_Reporte.Copy());
            Ds_Consulta.Tables.Add(Dt_Tmp_.Copy());
            Ds_Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles Ds_Reporte = new Ds_Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles();
            if (Tipo.Equals("PDF")) { Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles.rpt"); }
            else if (Tipo.Equals("EXCEL")) { Generar_Reporte_Excel(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Ficha_Tecnica_Bienes_Inmuebles.rpt"); }
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
            String Nombre_Reporte_Generar = "Ficha_Tecnica_" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
            String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
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
        private void Generar_Reporte_Excel(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            Reporte.Load(File_Path);
            String Ruta = "../../Reporte/Ficha_Tecnica_" + Session.SessionID + ".xls";
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.Excel;
            Reporte.Export(Export_Options);
            ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN: Convertir_Imagen_A_Cadena_Bytes
        ///DESCRIPCIÓN: Convierte la Imagen a una Cadena de Bytes.
        ///PROPIEDADES:   1.  P_Imagen.  Imagen a Convertir.    
        ///CREO: Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO: Marzo/2012 
        ///MODIFICO:
        ///FECHA_MODIFICO
        ///CAUSA_MODIFICACIÓN
        ///*******************************************************************************
        private Byte[] Convertir_Imagen_A_Cadena_Bytes(System.Drawing.Image P_Imagen) {
            Byte[] Img_Bytes = null;
            try {
                if (P_Imagen != null) {
                    MemoryStream MS_Tmp = new MemoryStream();
                    P_Imagen.Save(MS_Tmp, P_Imagen.RawFormat);
                    Img_Bytes = MS_Tmp.GetBuffer();
                    MS_Tmp.Close();
                }
            } catch (Exception Ex) {
                Lbl_Ecabezado_Mensaje.Text = "Verificar.";
                Lbl_Mensaje_Error.Text = "Verificar.";
                Div_Contenedor_Msj_Error.Visible = false;
            }
            return Img_Bytes;
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
                Cargar_Tablas_Reporte("PDF");
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
                Cargar_Tablas_Reporte("EXCEL");
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