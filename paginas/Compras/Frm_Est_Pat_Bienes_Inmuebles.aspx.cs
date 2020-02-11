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
using Presidencia.Dependencias.Negocios;
using Presidencia.Constantes;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
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
using System.Text;
using System.Reflection;
using CarlosAg.ExcelXmlWriter;

public partial class paginas_Control_Patrimonial_Frm_Est_Pat_Bienes_Inmuebles : System.Web.UI.Page {

    #region "Page Load"

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Page_Load
        ///DESCRIPCIÓN          : Metodo de Arranque.
        ///PARAMETROS           : 
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : Marzo/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        protected void Page_Load(object sender, EventArgs e) {
            Lbl_Ecabezado_Mensaje.Text = "";
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
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
        private void Llenar_Combo_Uso()
        {
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
        private void Llenar_Combo_Destino()
        {
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
        private void Llenar_Combo_Tipo_Predio()
        {
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
        private void Llenar_Listado_Calles()
        {
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
        private void Llenar_Listado_Cuentas_Predial()
        {
            Grid_Listado_Cuentas_Predial.SelectedIndex = (-1);
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuentas_Predial.P_Cuenta_Predial = Txt_Nombre_Cuenta_Predial_Buscar.Text.Trim();
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
        private void Llenar_Listado_Colonias()
        {
            Grid_Listado_Colonias.SelectedIndex = (-1);
            Cls_Cat_Ate_Colonias_Negocio Colonias_Negocio = new Cls_Cat_Ate_Colonias_Negocio();
            Colonias_Negocio.P_Nombre = Txt_Nombre_Colonia_Buscar.Text.Trim();
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
        private void Llenar_Combo_Origenes()
        {
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
        private void Llenar_Combo_Areas_Donacion()
        {
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
        private void Llenar_Combo_Sectores()
        {
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
        private void Llenar_Combo_Clasificaciones_Zonas()
        {
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
        private void Llenar_Combo_Clase_Activo()
        {
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
            Cls_Rpt_Pat_Listado_Bienes_Negocio Listado_Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            Listado_Negocio.P_Calle_ID = Hdf_Calle_ID.Value;
            Listado_Negocio.P_Colonia_ID = Hdf_Colonia_ID.Value;
            Listado_Negocio.P_Uso_ID = Cmb_Uso.SelectedItem.Value;
            Listado_Negocio.P_Destino_ID = Cmb_Destino.SelectedItem.Value;
            Listado_Negocio.P_Origen_ID = Cmb_Origen.SelectedItem.Value;
            Listado_Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value;
            Listado_Negocio.P_Area_Donacion = Cmb_Area_Donacion.SelectedItem.Value;
            Listado_Negocio.P_Tipo_Bien = Cmb_Tipo_Bien.SelectedItem.Value;
            Listado_Negocio.P_Sector = Cmb_Sector.SelectedItem.Value;
            Listado_Negocio.P_Clasificacion_ID = Cmb_Clasificacion_Zona.SelectedItem.Value;
            Listado_Negocio.P_Clase_Activo_ID = Cmb_Clase_Activo.SelectedItem.Value;
            Listado_Negocio.P_Estado = Cmb_Estado.SelectedItem.Value;
            Listado_Negocio.P_Libre_Gravamen = Cmb_Libertad_Gravament.SelectedItem.Value;
            Listado_Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Listado_Negocio.P_Tipo_Predio = Cmb_Tipo_Predio.SelectedItem.Value;
            if (Txt_Superficie_Desde.Text.Trim().Length > 0) { Listado_Negocio.P_Superficie_Inicial = Convert.ToDouble(Txt_Superficie_Desde.Text); }
            if (Txt_Superficie_Hasta.Text.Trim().Length > 0) { Listado_Negocio.P_Superficie_Final = Convert.ToDouble(Txt_Superficie_Hasta.Text); }
            if (Txt_Valor_Comercial_Desde.Text.Trim().Length > 0) { Listado_Negocio.P_Valor_Comercial_Inicial = Convert.ToDouble(Txt_Valor_Comercial_Desde.Text); }
            if (Txt_Valor_Comercial_Hasta.Text.Trim().Length > 0) { Listado_Negocio.P_Valor_Comercial_Final = Convert.ToDouble(Txt_Valor_Comercial_Hasta.Text); }
            if (Txt_Fecha_Registro_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Registro_Inicio.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Registral_Inicial = Convert.ToDateTime(Txt_Fecha_Registro_Inicio.Text); }
            if (Txt_Fecha_Registro_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Registro_Fin.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Registral_Final = Convert.ToDateTime(Txt_Fecha_Registro_Fin.Text); }
            if (Txt_Fecha_Escritura_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Escritura_Inicio.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Escritura_Inicial = Convert.ToDateTime(Txt_Fecha_Escritura_Inicio.Text); }
            if (Txt_Fecha_Escritura_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Escritura_Fin.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Escritura_Final = Convert.ToDateTime(Txt_Fecha_Escritura_Fin.Text); }
            if (Txt_Fecha_Baja_Inicio.Text.Trim().Length > 0 && !Txt_Fecha_Baja_Inicio.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Baja_Inicial = Convert.ToDateTime(Txt_Fecha_Baja_Inicio.Text); }
            if (Txt_Fecha_Baja_Fin.Text.Trim().Length > 0 && !Txt_Fecha_Baja_Fin.Text.Trim().Equals("__/___/____")) { Listado_Negocio.P_Fecha_Baja_Final = Convert.ToDateTime(Txt_Fecha_Baja_Fin.Text); }
            DataTable Dt_Datos = Listado_Negocio.Consultar_Bienes_Inmuebles();
            DataSet Ds_Consulta = new DataSet();
            if (Chk_Areas_Donacion.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica(Dt_Datos, "AREA_DONACION");
                Dt_Tmp.TableName = "DT_POR_AREAS_DONACION";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            if (Chk_Estatus.Checked) {
                DataTable Dt_Tmp = Obtener_Estadistica(Dt_Datos, "ESTATUS");
                Dt_Tmp.TableName = "DT_POR_ESTATUS";
                Dt_Tmp.DefaultView.Sort = "DATO";
                Ds_Consulta.Tables.Add(Dt_Tmp);
            }
            Ds_Est_Pat_Bienes_Inmuebles Ds_Reporte = new Ds_Est_Pat_Bienes_Inmuebles();
            Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Est_Pat_Bienes_Inmuebles.rpt", "PDF");
        }
    
        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Obtener_Estadistica_Por_Areas_Donacion
        ///DESCRIPCIÓN          :  
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : Marzo/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private DataTable Obtener_Estadistica(DataTable Dt_Datos, String ESTADISTICA) {
            DataTable Dt_Estadistica = new DataTable();
            Dt_Estadistica.Columns.Add("DATO", Type.GetType("System.String"));
            Dt_Estadistica.Columns.Add("NO_COINCIDENCIAS", Type.GetType("System.Int32"));
            Dt_Estadistica.Columns.Add("VALOR_PORCENTUAL", Type.GetType("System.Double"));
            Int32 No_Total_Registros = Dt_Datos.Rows.Count;
            DataTable Dt_Diferencias = Dt_Datos.DefaultView.ToTable(true, ESTADISTICA);
            foreach (DataRow Fila in Dt_Diferencias.Rows) {
                DataRow Fila_Cargar = Dt_Estadistica.NewRow();
                if (!String.IsNullOrEmpty(Fila[ESTADISTICA].ToString())) {
                    Fila_Cargar["DATO"] = Fila[ESTADISTICA].ToString().Trim();
                    Int32 Parcialidad = Dt_Datos.Select(ESTADISTICA + " = '" + Fila[ESTADISTICA].ToString().Trim() + "'").Length;
                    Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                    Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                } else {
                    Fila_Cargar["DATO"] = "SIN ASIGNACIÓN";
                    Int32 Parcialidad = Dt_Datos.Select(ESTADISTICA + " IS NULL").Length;
                    Fila_Cargar["NO_COINCIDENCIAS"] = Parcialidad;
                    Fila_Cargar["VALOR_PORCENTUAL"] = ((Parcialidad * 100.00) / No_Total_Registros);
                }
                    Dt_Estadistica.Rows.Add(Fila_Cargar);
            }
            return Dt_Estadistica;
        }

        ///*******************************************************************************
        ///NOMBRE DE LA FUNCIÓN : Generar_Reporte
        ///DESCRIPCIÓN          :  Lanza el Reporte
        ///PARAMETROS           :  
        ///CREO                 : Francisco Antonio Gallardo Castañeda
        ///FECHA_CREO           : Marzo/2012 
        ///MODIFICO             :
        ///FECHA_MODIFICO       :
        ///CAUSA_MODIFICACIÓN   :
        ///*******************************************************************************
        private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, String Nombre_Reporte, String Tipo) {
            ReportDocument Reporte = new ReportDocument();
            String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
            String Nombre_Reporte_Generar = "Rpt_Est_Pat_Bienes_Inmuebles_" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
            String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
            Reporte.Load(File_Path);
            Ds_Reporte = Data_Set_Consulta_DB;
            Reporte.SetDataSource(Ds_Reporte);
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath(Ruta);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            if (!Chk_Areas_Donacion.Checked) { Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Bienes_Inmuebles_Por_Area_Donacion"].ObjectFormat.EnableSuppress = true; }
            if (!Chk_Estatus.Checked) { Reporte.ReportDefinition.ReportObjects["Rpt_Est_Pat_Bienes_Inmuebles_Por_Estatus"].ObjectFormat.EnableSuppress = true; }
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
            if (Chk_Areas_Donacion.Checked || Chk_Estatus.Checked) {
                Cargar_Tabla_Estisticas("PDF");
            } else {
                Lbl_Ecabezado_Mensaje.Text = "Es necesario seleccionar alguno de los tipos de estadistica.";
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
        protected void Btn_Ejecutar_Busqueda_Calles_Click(object sender, ImageClickEventArgs e)
        {
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
        protected void Txt_Nombre_Calles_Buscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Grid_Listado_Calles.PageIndex = 0;
                Llenar_Listado_Calles();
            }
            catch (Exception Ex)
            {
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
        protected void Btn_Buscar_Calle_Click(object sender, ImageClickEventArgs e)
        {
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
        protected void Btn_Ejecutar_Busqueda_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
        {
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
        protected void Txt_Nombre_Cuenta_Predial_Buscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Grid_Listado_Cuentas_Predial.PageIndex = 0;
                Llenar_Listado_Cuentas_Predial();
            }
            catch (Exception Ex)
            {
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
        protected void Btn_Buscar_Numero_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
        {
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
        protected void Btn_Ejecutar_Busqueda_Colonia_Click(object sender, ImageClickEventArgs e)
        {
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
        protected void Txt_Nombre_Colonia_Buscar_TextChanged(object sender, EventArgs e)
        {
            try
            {
                Grid_Listado_Colonias.PageIndex = 0;
                Llenar_Listado_Colonias();
            }
            catch (Exception Ex)
            {
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
        protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e)
        {
            Mpe_Colonias.Show();
        }

    #endregion

}