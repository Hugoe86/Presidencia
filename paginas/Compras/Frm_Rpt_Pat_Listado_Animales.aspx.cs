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
using Presidencia.Control_Patrimonial_Operacion_Cemovientes.Negocio;
using Presidencia.Control_Patrimonial_Reporte_Listado_Bienes.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.ReportSource;
using System.Text;


public partial class paginas_Compras_Frm_Rpt_Pat_Listado_Animales : System.Web.UI.Page {

    #region "Page Load"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Evento que se carga cuando la Pagina de Inicia.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Diciembre/2011 
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
    ///FECHA_CREO: 05/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    public void Llenar_Combos() {
        try {
            Cls_Ope_Pat_Com_Cemovientes_Negocio BM_Negocio = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
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

            //Se llena combo de Tipos de Animales
            BM_Negocio.P_Tipo_DataTable = "TIPOS_CEMOVIENTES";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Tipo.DataSource = Dt_Temporal;
            Cmb_Tipo.DataTextField = "NOMBRE";
            Cmb_Tipo.DataValueField = "TIPO_CEMOVIENTE_ID";
            Cmb_Tipo.DataBind();
            Cmb_Tipo.Items.Insert(0, new ListItem("< TODOS >", ""));

            //Se llena combo de Razas
            BM_Negocio.P_Tipo_DataTable = "RAZAS";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Raza.DataSource = Dt_Temporal;
            Cmb_Raza.DataTextField = "NOMBRE";
            Cmb_Raza.DataValueField = "RAZA_ID";
            Cmb_Raza.DataBind();
            Cmb_Raza.Items.Insert(0, new ListItem("< TODAS >", ""));

            //Se llena combo de Colores
            BM_Negocio.P_Tipo_DataTable = "COLORES";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Color.DataSource = Dt_Temporal;
            Cmb_Color.DataTextField = "DESCRIPCION";
            Cmb_Color.DataValueField = "COLOR_ID";
            Cmb_Color.DataBind();
            Cmb_Color.Items.Insert(0, new ListItem("< TODOS >", ""));

            //Se llena combo de Tipos de Adiestramiento
            BM_Negocio.P_Tipo_DataTable = "TIPOS_ADIESTRAMIENTO";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Tipo_Adiestramiento.DataSource = Dt_Temporal;
            Cmb_Tipo_Adiestramiento.DataTextField = "NOMBRE";
            Cmb_Tipo_Adiestramiento.DataValueField = "TIPO_ADIESTRAMIENTO_ID";
            Cmb_Tipo_Adiestramiento.DataBind();
            Cmb_Tipo_Adiestramiento.Items.Insert(0, new ListItem("< TODOS >", ""));

            //Se llena combo de Tipos de Adiestramiento
            BM_Negocio.P_Tipo_DataTable = "TIPOS_ALIMENTACION";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Tipo_Alimentacion.DataSource = Dt_Temporal;
            Cmb_Tipo_Alimentacion.DataTextField = "NOMBRE";
            Cmb_Tipo_Alimentacion.DataValueField = "TIPO_ALIMENTACION_ID";
            Cmb_Tipo_Alimentacion.DataBind();
            Cmb_Tipo_Alimentacion.Items.Insert(0, new ListItem("< TODOS >", ""));

            //Se llena combo de Funciones
            BM_Negocio.P_Tipo_DataTable = "FUNCIONES";
            Dt_Temporal = BM_Negocio.Consultar_DataTable();
            Cmb_Funcion.DataSource = Dt_Temporal;
            Cmb_Funcion.DataTextField = "NOMBRE";
            Cmb_Funcion.DataValueField = "FUNCION_ID";
            Cmb_Funcion.DataBind();
            Cmb_Funcion.Items.Insert(0, new ListItem("< TODOS >", ""));

        }
        catch (Exception Ex)
        {
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
    ///FECHA_CREO: 05/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Busqueda_Empleados_Resguardo()
    {
        Grid_Busqueda_Empleados_Resguardo.SelectedIndex = (-1);
        Grid_Busqueda_Empleados_Resguardo.Columns[1].Visible = true;
        Cls_Ope_Pat_Com_Cemovientes_Negocio Negocio = new Cls_Ope_Pat_Com_Cemovientes_Negocio();
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
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Busqueda_Empleados_Resguardo
    ///DESCRIPCIÓN: Llena el Grid con los empleados que cumplan el filtro
    ///PROPIEDADES:     
    ///CREO:                 
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Generar_Reporte(String Tipo)
    {
        try
        {
            String Filtros = null;
            Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            if (Txt_Inventario_Anterior.Text.Trim().Length > 0) { Negocio.P_No_Inventario_Anterior = Txt_Inventario_Anterior.Text.Trim(); Filtros += "No. Inventario: " + Txt_Inventario_Anterior.Text.Trim() + "<br />"; }
            if (Txt_Inventario_SIAS.Text.Trim().Length > 0) { Negocio.P_No_Inventario_SIAS = Txt_Inventario_SIAS.Text.Trim(); Txt_Inventario_Anterior.Text.Trim(); Filtros += "No. Inventario SIAS: " + Txt_Inventario_SIAS.Text.Trim() + "<br />"; }
            if (Txt_Nombre_Producto.Text.Trim().Length > 0) { Negocio.P_Nombre_Producto = Txt_Nombre_Producto.Text.Trim().ToUpper(); Filtros += "Nombre del Animal: " + Txt_Nombre_Producto.Text.Trim() + "<br>"; }
            if (Cmb_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value.Trim(); Filtros += "Unidad Responsable: " + Cmb_Dependencia.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Tipo.SelectedIndex > 0) { Negocio.P_Tipo = Cmb_Tipo.SelectedItem.Value.Trim(); Filtros += "Tipo de Animal: " + Txt_Inventario_SIAS.Text.Trim() + "<br />"; }
            if (Cmb_Raza.SelectedIndex > 0) { Negocio.P_Raza_ID = Cmb_Raza.SelectedItem.Value.Trim(); Filtros += "Raza: " + Cmb_Raza.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Color.SelectedIndex > 0) { Negocio.P_Color_ID = Cmb_Color.SelectedItem.Value.Trim(); Filtros += "Color: " + Cmb_Color.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Tipo_Alimentacion.SelectedIndex > 0) { Negocio.P_Tipo_Alimentacion_ID = Cmb_Tipo_Alimentacion.SelectedItem.Value.Trim(); Filtros += "Tipo de Alimentaci&oacute;n: " + Cmb_Tipo_Alimentacion.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Tipo_Adiestramiento.SelectedIndex > 0) { Negocio.P_Tipo_Adiestramiento_ID = Cmb_Tipo_Adiestramiento.SelectedItem.Value.Trim(); Filtros += "Tipo de Alimentaci&oacute;n: " + Cmb_Tipo_Adiestramiento.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Funcion.SelectedIndex > 0) { Negocio.P_Funcion_ID = Cmb_Funcion.SelectedItem.Value.Trim(); Filtros += "Funci&oacute;n: " + Cmb_Funcion.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Sexo.SelectedIndex > 0) { Negocio.P_Sexo = Cmb_Sexo.SelectedItem.Value.Trim(); Filtros += "Sexo: " + Cmb_Sexo.SelectedItem.Text.Trim() + "<br />"; }
            if (Txt_Factura.Text.Trim().Length > 0) { Negocio.P_Factura = Txt_Factura.Text.Trim(); Filtros += "Factura: " + Txt_Factura.Text.Trim() + "<br />"; }
            if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) {
                Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text.Trim());
                Negocio.P_Tomar_Fecha_Inicial = true;
                Filtros += "Fecha de Adquisici&oacute;n [Inicial]: " + Txt_Fecha_Adquisicion_Inicial.Text.Trim() + "<br />"; 
            }
            if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) {
                Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text.Trim());
                Negocio.P_Tomar_Fecha_Final = true;
                Filtros += "Fecha de Adquisici&oacute;n [Final]:: " + Txt_Fecha_Adquisicion_Final.Text.Trim() + "<br />"; 
            }
            if (Cmb_Ascendencia.SelectedIndex > 0) { Negocio.P_Tipo_Ascendencia = Cmb_Ascendencia.SelectedItem.Value.Trim(); Filtros += "Tipo de Ascendencia: " + Cmb_Ascendencia.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Estatus.SelectedIndex > 0) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim(); Filtros += "Estatus: " + Cmb_Estatus.SelectedItem.Text.Trim() + "<br />"; }
            if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Negocio.P_Resguardante_ID = Hdf_Resguardante_ID.Value.Trim(); Filtros += "Resguardante: " + Txt_Resguardante.Text.Trim() + "<br />"; }
            DataTable Dt_Datos = Negocio.Consultar_Animales();

            DataTable Dt_Filtros = Filtros_Reporte();
            Dt_Filtros.TableName = "FILTROS";
            DataSet Ds_Consulta = new DataSet();
            Ds_Consulta.Tables.Add(Dt_Datos.Copy());
            Ds_Consulta.Tables.Add(Dt_Filtros.Copy());

            Ds_Rpt_Pat_Listado_Animales Ds_Reporte = new Ds_Rpt_Pat_Listado_Animales();

            Generar_Reporte(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Animales.rpt", Tipo);
        }
        catch (Exception Ex)
        {
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
    private void Generar_Reporte(DataSet Data_Set_Consulta_DB, DataSet Ds_Reporte, string Nombre_Reporte, string Tipo) {

        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Compras/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        for (Int32 Contador_Principal = 0; Contador_Principal < (Math.Floor(Convert.ToDouble(Data_Set_Consulta_DB.Tables[0].Rows.Count / 1000)) + 1); Contador_Principal++) {
            StringBuilder Animales = new StringBuilder();
            Animales.Length = 0;
            if ((Contador_Principal * 1000) > Data_Set_Consulta_DB.Tables[0].Rows.Count) {
                break;
            }
            for (Int32 Contador = 0; Contador < 1000; Contador = Contador + 1) {
                Int32 Posicion = (Contador_Principal * 1000) + Contador;
                if (Posicion >= Data_Set_Consulta_DB.Tables[0].Rows.Count) { break; }
                Ds_Reporte.Tables["PRINCIPAL"].ImportRow(Data_Set_Consulta_DB.Tables[0].Rows[Posicion]);
                if (Contador > 0) { Animales.Append(","); }
                Animales.Append("'" + Data_Set_Consulta_DB.Tables[0].Rows[Posicion]["CLAVE_BIEN"].ToString().Trim() + "'");
            }
            Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            Negocio.P_Tipo_Bien = "CEMOVIENTE";
            Negocio.P_Bien_ID = Animales.ToString();
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
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Animales.pdf");
        if (Tipo.Equals("Excel")) { Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/Rpt_Pat_Listado_Animales.xls"); }
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
        if (Tipo.Equals("Excel")) { Export_Options.ExportFormatType = ExportFormatType.Excel; }

        Reporte.Export(Export_Options);
        String Ruta = "../../Reporte/Rpt_Pat_Listado_Animales.pdf";
        if (Tipo.Equals("Excel")) { Ruta = "../../Reporte/Rpt_Pat_Listado_Animales.xls"; }
        ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
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
        if (Txt_Inventario_Anterior.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Inventario Anterior:", Txt_Inventario_Anterior.Text.Trim()); }
        if (Txt_Inventario_SIAS.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Inventario [SIAS]:", Txt_Inventario_SIAS.Text.Trim()); }
        if (Txt_Nombre_Producto.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Nombre Animal:", Txt_Nombre_Producto.Text.Trim()); }
        if (Cmb_Dependencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Unidad Responsable:", Cmb_Dependencia.SelectedItem.Text.Trim()); }
        if (Cmb_Tipo.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Tipo de Animal:", Cmb_Tipo.SelectedItem.Text.Trim()); }
        if (Cmb_Raza.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Raza:", Cmb_Raza.SelectedItem.Text.Trim()); }
        if (Cmb_Color.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Color:", Cmb_Color.SelectedItem.Text.Trim()); }
        if (Cmb_Tipo_Alimentacion.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Alimentación:", Cmb_Tipo_Alimentacion.SelectedItem.Text.Trim()); }
        if (Cmb_Tipo_Adiestramiento.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Tipo Adiestramiento:", Cmb_Tipo_Adiestramiento.SelectedItem.Text.Trim()); }
        if (Cmb_Funcion.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Función:", Cmb_Funcion.SelectedItem.Text.Trim()); }
        if (Cmb_Sexo.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Sexo:", Cmb_Sexo.SelectedItem.Text.Trim()); }
        if (Txt_Factura.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Factura:", Txt_Factura.Text.Trim()); }
        if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Fecha Adquisición [Inicio]:", Txt_Fecha_Adquisicion_Inicial.Text.Trim()); }
        if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0) { Cargar_Filtro(Dt_Filtros, "Fecha Adquisición [Fin]:", Txt_Fecha_Adquisicion_Final.Text.Trim()); }
        if (Cmb_Ascendencia.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Ascendencia:", Cmb_Ascendencia.SelectedItem.Text.Trim()); }
        if (Cmb_Estatus.SelectedIndex > 0) { Cargar_Filtro(Dt_Filtros, "Estatus:", Cmb_Estatus.SelectedItem.Text.Trim()); }
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
        String Ruta = "../../Reporte/Rpt_Pat_Listado_Animales" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".pdf";
        if (Tipo.Equals("Excel")) { Ruta = "../../Reporte/Rpt_Pat_Listado_Animales" + Session.SessionID + String.Format("{0:ddMMyyyyhhmmss}", DateTime.Now) + ".xls"; }
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
    private void Generar_Reporte_Completo(String Tipo) {
        try {

            String Filtros = null;
            Cls_Rpt_Pat_Listado_Bienes_Negocio Negocio = new Cls_Rpt_Pat_Listado_Bienes_Negocio();
            if (Txt_Inventario_Anterior.Text.Trim().Length > 0) { Negocio.P_No_Inventario_Anterior = Txt_Inventario_Anterior.Text.Trim(); Filtros += "No. Inventario: " + Txt_Inventario_Anterior.Text.Trim() + "<br />"; }
            if (Txt_Inventario_SIAS.Text.Trim().Length > 0) { Negocio.P_No_Inventario_SIAS = Txt_Inventario_SIAS.Text.Trim(); Txt_Inventario_Anterior.Text.Trim(); Filtros += "No. Inventario SIAS: " + Txt_Inventario_SIAS.Text.Trim() + "<br />"; }
            if (Txt_Nombre_Producto.Text.Trim().Length > 0) { Negocio.P_Nombre_Producto = Txt_Nombre_Producto.Text.Trim().ToUpper(); Filtros += "Nombre del Animal: " + Txt_Nombre_Producto.Text.Trim() + "<br>"; }
            if (Cmb_Dependencia.SelectedIndex > 0) { Negocio.P_Dependencia_ID = Cmb_Dependencia.SelectedItem.Value.Trim(); Filtros += "Unidad Responsable: " + Cmb_Dependencia.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Tipo.SelectedIndex > 0) { Negocio.P_Tipo = Cmb_Tipo.SelectedItem.Value.Trim(); Filtros += "Tipo de Animal: " + Txt_Inventario_SIAS.Text.Trim() + "<br />"; }
            if (Cmb_Raza.SelectedIndex > 0) { Negocio.P_Raza_ID = Cmb_Raza.SelectedItem.Value.Trim(); Filtros += "Raza: " + Cmb_Raza.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Color.SelectedIndex > 0) { Negocio.P_Color_ID = Cmb_Color.SelectedItem.Value.Trim(); Filtros += "Color: " + Cmb_Color.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Tipo_Alimentacion.SelectedIndex > 0) { Negocio.P_Tipo_Alimentacion_ID = Cmb_Tipo_Alimentacion.SelectedItem.Value.Trim(); Filtros += "Tipo de Alimentaci&oacute;n: " + Cmb_Tipo_Alimentacion.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Tipo_Adiestramiento.SelectedIndex > 0) { Negocio.P_Tipo_Adiestramiento_ID = Cmb_Tipo_Adiestramiento.SelectedItem.Value.Trim(); Filtros += "Tipo de Alimentaci&oacute;n: " + Cmb_Tipo_Adiestramiento.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Funcion.SelectedIndex > 0) { Negocio.P_Funcion_ID = Cmb_Funcion.SelectedItem.Value.Trim(); Filtros += "Funci&oacute;n: " + Cmb_Funcion.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Sexo.SelectedIndex > 0) { Negocio.P_Sexo = Cmb_Sexo.SelectedItem.Value.Trim(); Filtros += "Sexo: " + Cmb_Sexo.SelectedItem.Text.Trim() + "<br />"; }
            if (Txt_Factura.Text.Trim().Length > 0) { Negocio.P_Factura = Txt_Factura.Text.Trim(); Filtros += "Factura: " + Txt_Factura.Text.Trim() + "<br />"; }
            if (Txt_Fecha_Adquisicion_Inicial.Text.Trim().Length > 0)
            {
                Negocio.P_Fecha_Adquisicion_Inicial = Convert.ToDateTime(Txt_Fecha_Adquisicion_Inicial.Text.Trim());
                Negocio.P_Tomar_Fecha_Inicial = true;
                Filtros += "Fecha de Adquisici&oacute;n [Inicial]: " + Txt_Fecha_Adquisicion_Inicial.Text.Trim() + "<br />";
            }
            if (Txt_Fecha_Adquisicion_Final.Text.Trim().Length > 0)
            {
                Negocio.P_Fecha_Adquisicion_Final = Convert.ToDateTime(Txt_Fecha_Adquisicion_Final.Text.Trim());
                Negocio.P_Tomar_Fecha_Final = true;
                Filtros += "Fecha de Adquisici&oacute;n [Final]:: " + Txt_Fecha_Adquisicion_Final.Text.Trim() + "<br />";
            }
            if (Cmb_Ascendencia.SelectedIndex > 0) { Negocio.P_Tipo_Ascendencia = Cmb_Ascendencia.SelectedItem.Value.Trim(); Filtros += "Tipo de Ascendencia: " + Cmb_Ascendencia.SelectedItem.Text.Trim() + "<br />"; }
            if (Cmb_Estatus.SelectedIndex > 0) { Negocio.P_Estatus = Cmb_Estatus.SelectedItem.Value.Trim(); Filtros += "Estatus: " + Cmb_Estatus.SelectedItem.Text.Trim() + "<br />"; }
            if (Hdf_Resguardante_ID.Value.Trim().Length > 0) { Negocio.P_Resguardante_ID = Hdf_Resguardante_ID.Value.Trim(); Filtros += "Resguardante: " + Txt_Resguardante.Text.Trim() + "<br />"; }
            DataTable Dt_Datos = Negocio.Consultar_Animales_Completo();

            DataTable Dt_Filtros = Filtros_Reporte();
            Dt_Datos.TableName = "PRINCIPAL";
            Dt_Filtros.TableName = "FILTROS";
            DataTable Dt_Temporal = Dt_Datos.DefaultView.ToTable(true, "CLAVE_BIEN", "VALOR_INCIAL");
            DataSet Ds_Consulta = new DataSet();
            Ds_Consulta.Tables.Add(Dt_Datos.Copy());
            Ds_Consulta.Tables.Add(Dt_Filtros.Copy());

            Ds_Rpt_Pat_Listado_Animales_2 Ds_Reporte = new Ds_Rpt_Pat_Listado_Animales_2();
            

            Int32 No_Registros = Dt_Temporal.Rows.Count;
            Double Total_Inicial = 0;
            Double Total_Actual = 0;
            Object Suma_Total_Inicial = Dt_Temporal.Compute("Sum(VALOR_INCIAL)", "VALOR_INCIAL <> 0");
            Object Suma_Total_Actual = Dt_Temporal.Compute("Sum(VALOR_INCIAL)", "VALOR_INCIAL <> 0");
            if (Suma_Total_Actual != null && Suma_Total_Actual != DBNull.Value) Total_Actual = Convert.ToDouble(Suma_Total_Actual);
            if (Suma_Total_Inicial != null && Suma_Total_Inicial != DBNull.Value) Total_Inicial = Convert.ToDouble(Suma_Total_Inicial);


            if (Filtros == null) { Filtros = "Ninguno"; } else { Filtros = HttpUtility.HtmlDecode(Filtros); }

            Generar_Reporte_Completo(Ds_Consulta, Ds_Reporte, "Rpt_Pat_Listado_Animales_2.rpt", Tipo, Filtros, No_Registros, Total_Inicial, Total_Actual);
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
    ///FECHA_CREO: 05/Diciembre/2011 
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
    ///FECHA_CREO: 05/Diciembre/2011 
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

    #region "Eventos"

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Resguardante_Click
    ///DESCRIPCIÓN: Lanza la Busqueda Avanzada para el Resguardante.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Diciembre/2011 
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
    ///FECHA_CREO: 05/Diciembre/2011 
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Reporte_PDF_Click
    ///DESCRIPCIÓN: Lanza el Reporte en PDF para Imprimir.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 05/Diciembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Generar_Reporte_PDF_Click(object sender, ImageClickEventArgs e) {
        try {
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
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Generar_Reporte_Completo("Excel");
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

}
