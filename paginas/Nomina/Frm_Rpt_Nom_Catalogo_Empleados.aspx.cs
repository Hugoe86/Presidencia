using System;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Ayudante_CarlosAG;
using Presidencia.Reportes_Nomina_Catalogo_Empleados.Negocio;
using Presidencia.Sessiones;
using Presidencia.Ayudante_Informacion;
using Presidencia.Cat_Parametros_Nomina.Negocio;

public partial class paginas_Nomina_Reporte_Frm_Rpt_Nom_Catalogo_Empleados : System.Web.UI.Page
{
    #region (Load/Init)
    /// *************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Habilita la configuración inicial de la página.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:25 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Estado_Inicial();
            }

            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Métodos)

    #region (Métodos Generales)
    /// *************************************************************************************
    /// NOMBRE: Estado_Inicial
    /// 
    /// DESCRIPCIÓN: Método que carga y habilita los controles a un estado inicial
    ///              para comenzar las operaciones.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:34 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Estado_Inicial()
    {
        try
        {
            Consultar_Tipos_Nominas();          //Carga los tipos de nómina registradas en sistema.
            Consultar_Sindicatos();             //Carga los sindicatos registrados en sistema.
            Consultar_Unidades_Responsables();  //Carga la unidades responsables registrdas en sistema.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al habilitar el estado inicial de los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consultar)

    /// *************************************************************************************
    /// NOMBRE: Consultar_Empleados
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:34 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Consultar_Empleados()
    {
        var Obj_Empleados = new Cls_Rpt_Nom_Catalogo_Empleados_Negocio();// Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;// datatable que almacena una lista de empleados.

        try
        {
            // agregar filtro si se especifica NUMERO EMPLEADO
            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim()))
                Obj_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();

            // agregar filtro si se especifica NOMBRE EMPLEADO
            if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim()))
                Obj_Empleados.P_Nombre_Empleado = Txt_Busqueda_Nombre_Empleado.Text.Trim();

            // agregar filtro si se especifica RFC
            if (!String.IsNullOrEmpty(Txt_Busqueda_RFC_Empleado.Text.Trim()))
                Obj_Empleados.P_RFC_Empleado = Txt_Busqueda_RFC_Empleado.Text.Trim();

            // agregar filtro si se especific CURP
            if (!String.IsNullOrEmpty(Txt_Busqueda_CURP_Empleado.Text.Trim()))
                Obj_Empleados.P_CURP_Empleado = Txt_Busqueda_CURP_Empleado.Text.Trim();

            // agregar filtro si se especifica TIPO NOMINA
            if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0)
                Obj_Empleados.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();

            // agregar filtro si se especifica SINDICATO
            if (Cmb_Busqueda_Sindicato.SelectedIndex > 0)
                Obj_Empleados.P_Sindicato_ID = Cmb_Busqueda_Sindicato.SelectedValue.Trim();

            // agregar filtro si se especifica UNIDAD RESPONSABLE
            if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0)
                Obj_Empleados.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedValue.Trim();

            // agregar filtro si se especifica ESTATUS
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                Obj_Empleados.P_Estatus_Empleado = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();

            Cls_Cat_Nom_Parametros_Negocio Parametros_Nomina = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();
            Obj_Empleados.P_Percepcion_Deduccion_ID = Parametros_Nomina.P_Percepcion_Despensa;
            
            // si el ordenamiento seleccionado es por sindicato, filtrar sindicato_id nulo
            if (Cmb_Ordenamiento.SelectedValue.Contains("POR SINDICATO"))
            {
                Obj_Empleados.P_Filtro_Dinamico = Cat_Empleados.Campo_Sindicato_ID + " IS NOT NULL";
            }

            Dt_Empleados = Obj_Empleados.Consultar_Catalogo_Empleados();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los empleados. Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados;
    }

    /// *************************************************************************************
    /// NOMBRE: Consultar_Parametros_Reporte
    /// DESCRIPCIÓN: Forma una tabla con el nombre del empleado en la sesión
    /// PARÁMETROS: No Aplica
    /// USUARIO CREO: Roberto González Oseguera
    /// FECHA CREO: 05-abr-2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Consultar_Parametros_Reporte()
    {
        DataTable Dt_Parametros = new DataTable();
        DataRow Dr_Parametro;

        Dt_Parametros.Columns.Add("Elaboro", typeof(string));
        Dt_Parametros.Columns.Add("TITULO_REPORTE", typeof(string));
        Dr_Parametro = Dt_Parametros.NewRow();
        Dr_Parametro["Elaboro"] = Cls_Sessiones.Nombre_Empleado.ToUpper();
        // agregar parámetro título reporte dependiendo de selección en el combo de ordenamiento
        if (Cmb_Ordenamiento.SelectedIndex == 0)
        {
            Dr_Parametro["TITULO_REPORTE"] = "CATALOGO DE EMPLEADOS";
        }
        else 
        {
            Dr_Parametro["TITULO_REPORTE"] = Cmb_Ordenamiento.SelectedValue;
        }
        Dt_Parametros.Rows.Add(Dr_Parametro);

        return Dt_Parametros;
    }
    #endregion (Consultar)

    #region (Consultas Combos)
    /// *************************************************************************************
    /// NOMBRE: Consultar_Tipos_Nominas
    /// 
    /// DESCRIPCIÓN: Consulta los tipos de nómina que se encuantran dadas de alta 
    ///              actualmente en sistema.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 10:52 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Tipos_Nominas()
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;//Variable que almacena la lista de tipos de nominas. 
        try
        {
            Dt_Tipos_Nominas = Obj_Tipos_Nominas.Consulta_Tipos_Nominas();//Consulta los tipos de nominas.
            Cargar_Combos(Cmb_Busqueda_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
                Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, 0);//Carga el combo de tipos de nómina.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Sindicatos
    /// 
    /// DESCRIPCIÓN: Consulta los sindicatos que se encuentran registrados actualmente
    ///              en sistema.
    ///              
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Sindicatos()
    {
        Cls_Cat_Nom_Sindicatos_Negocio Obj_Sindicatos = new Cls_Cat_Nom_Sindicatos_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Sindicatos = null;//Variable que almacena una lista de sindicatos.

        try
        {
            Dt_Sindicatos = Obj_Sindicatos.Consulta_Sindicato();//Consulta los sindicatos.
            Cargar_Combos(Cmb_Busqueda_Sindicato, Dt_Sindicatos, Cat_Nom_Sindicatos.Campo_Nombre,
                Cat_Nom_Sindicatos.Campo_Sindicato_ID, 0);//Carga el combo de sindicatos.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los sindicatos que existen actualmente en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Unidades_Responsables
    /// 
    /// DESCRIPCIÓN: Consulta las Unidades responsables que se encuentran registrados actualmente
    ///              en sistema.
    ///              
    /// PARÁMETROS: No Aplica.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Unidades_Responsables()
    {
        Cls_Cat_Dependencias_Negocio Obj_Unidades_Responsables = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que almacena una lista de las unidades resposables en sistema.

        try
        {
            Dt_Unidades_Responsables = Obj_Unidades_Responsables.Consulta_Dependencias();//Consulta las unidades responsables registradas en  sistema.
            Cargar_Combos(Cmb_Busqueda_Unidad_Responsable, Dt_Unidades_Responsables, Cat_Dependencias.Campo_Nombre,
                Cat_Dependencias.Campo_Dependencia_ID, 0);//Se carga el control que almacena las unidades responsables.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Cargar_Combos
    /// 
    /// DESCRIPCIÓN: Carga cualquier ctlr DropDownList que se le pase como parámetro.
    ///              
    /// PARÁMETROS: Combo.- Ctlr que se va a cargar.
    ///             Dt_Datos.- Informacion que se cargara en el combo.
    ///             Text.- Texto que será la parte visible de la lista de tipos de nómina.
    ///             Value.- Valor que será el que almacenará el elemnto seleccionado.
    ///             Index.- Indice el cuál será el que se mostrara inicialmente. 
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:12 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Cargar_Combos(DropDownList Combo, DataTable Dt_Datos, String Text, String Value, Int32 Index)
    {
        try
        {
            Combo.DataSource = Dt_Datos;
            Combo.DataTextField = Text;
            Combo.DataValueField = Value;
            Combo.DataBind();
            Combo.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Combo.SelectedIndex = Index;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar el Ctlr de Tipo DropDownList. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Validacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_RFC
    /// DESCRIPCION : Valida el RFC Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Mayo/2011 12:20p.m.
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_RFC()
    {
        string Patron_RFC = @"^[a-zA-Z]{3,4}(\d{6})((\D|\d){3})?$";

        if (Txt_Busqueda_RFC_Empleado.Text != null) return Regex.IsMatch(Txt_Busqueda_RFC_Empleado.Text, Patron_RFC);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_CURP
    /// DESCRIPCION : Valida el Fax Ingresado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Mayo/2011 12:20p.m.
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_CURP()
    {
        string Patron_Curp = @"^[a-zA-Z]{4}(\d{6})([a-zA-Z]{6})(\d{2})?$";

        if (Txt_Busqueda_CURP_Empleado.Text != null) return Regex.IsMatch(Txt_Busqueda_CURP_Empleado.Text, Patron_Curp);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Mayo/2011 12:20p.m.
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null)
        {
            return Regex.IsMatch(Fecha, Cadena_Fecha);
        }
        else
        {
            return false;
        }
    }

    #endregion

    #region (Reportes)
    /// *************************************************************************************
    /// NOMBRE: Generar_Reporte
    /// 
    /// DESCRIPCIÓN: Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS: Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///             Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Nomina/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Exportar_Reporte_PDF
    /// 
    /// DESCRIPCIÓN: Método que guarda el reporte generado en formato PDF en la ruta
    ///              especificada.
    ///              
    /// PARÁMETROS: Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///             Nombre_Reporte.- Nombre que se le dará al reporte.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Reporte
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en pantalla.
    ///              
    /// PARÁMETROS: Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Empleados",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Exportar_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Reporte: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Exportar_Reporte(DataSet Ds_Reporte, String Nombre_Reporte, String Nombre_Archivo, String Extension_Archivo, ExportFormatType Formato)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Nomina/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Reporte);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte";
        }

        String Archivo_Reporte = Nombre_Archivo + "." + Extension_Archivo;  // formar el nombre del archivo a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_Reporte);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = Formato;
            Reporte.Export(Export_Options_Calculo);

            if (Formato == ExportFormatType.Excel)
            {
                Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-excel");
            }
            else if (Formato == ExportFormatType.WordForWindows)
            {
                Mostrar_Excel(Server.MapPath("../../Reporte/" + Archivo_Reporte), "application/vnd.ms-word");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(string Ruta_Archivo, string Contenido)
    {
        try
        {
            System.IO.FileInfo ArchivoExcel = new System.IO.FileInfo(Ruta_Archivo);
            if (ArchivoExcel.Exists)
            {
                Response.Clear();
                Response.Buffer = true;
                Response.ContentType = Contenido;
                Response.AddHeader("Content-Disposition", "attachment;filename=" + ArchivoExcel.Name);
                Response.Charset = "UTF-8";
                Response.ContentEncoding = Encoding.Default;
                Response.WriteFile(ArchivoExcel.FullName);
                Response.End();
            }
        }
        catch (Exception Ex)
        {
            //// Response.End(); siempre genera una excepción (http://support.microsoft.com/kb/312629/EN-US/)
            throw new Exception("Error al mostrar el reporte en excel. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 12:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Empleados;//Variable que almacena un listado de empleados.
        DataTable Dt_Parametros;

        try
        {
            Ds_Reporte = new DataSet();
            Dt_Empleados = Consultar_Empleados();
            //  para ordenar por rfc
            if (Cmb_Ordenamiento.SelectedValue.Contains("POR RFC"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA,UNIDAD_RESPONSABLE," + Cat_Empleados.Campo_RFC;
                Dt_Empleados = Dv_Ordenar.ToTable();
            }
            //  para ordenar por puesto
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR PUESTO"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA,UNIDAD_RESPONSABLE,PUESTO";
                Dt_Empleados = Dv_Ordenar.ToTable();
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR ANTIGÜEDAD"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA,UNIDAD_RESPONSABLE,Fecha Desc";
                Dt_Empleados = Dv_Ordenar.ToTable();
            }

            Dt_Empleados.TableName = "Dt_Catalogo_Empleados";
            Dt_Parametros = Consultar_Parametros_Reporte();
            Dt_Parametros.TableName = "Dt_Parametros";
            Ds_Reporte.Tables.Add(Dt_Empleados.Copy());
            Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

            //Se llama al método que ejecuta la operación de generar el reporte.
            // dependiendo del ordenamiento seleccionado, llamar el reporte correspondiente (cambian campos y ordenamiento)
            if (Cmb_Ordenamiento.SelectedIndex == 0)
            {
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Codigo.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID + ".pdf");
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR NUMERO DE EMPLEADO"))
            {
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID + ".pdf");
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR SINDICATO"))
            {
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Sindicato.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID + ".pdf");
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR RFC"))
            {
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Rfc.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID + ".pdf");
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR PUESTO"))
            {
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Puesto.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID + ".pdf");
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR BANCO"))
            {
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Banco.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID + ".pdf");
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR ANTIGÜEDAD"))
            {
                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Antiguedad.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID + ".pdf");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Excel_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Empleados;//Variable que almacena un listado de empleados.
        DataTable Dt_Parametros;

        try
        {
            Ds_Reporte = new DataSet();
            Dt_Empleados = Consultar_Empleados();

            //  para ordenar por rfc
            if (Cmb_Ordenamiento.SelectedValue.Contains("POR RFC"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA_ID,UNIDAD_RESPONSABLE," + Cat_Empleados.Campo_RFC;
                Dt_Empleados = Dv_Ordenar.ToTable();
            }
            //  para ordenar por puesto
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR PUESTO"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA_ID,UNIDAD_RESPONSABLE,PUESTO";
                Dt_Empleados = Dv_Ordenar.ToTable();
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR ANTIGÜEDAD"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA,UNIDAD_RESPONSABLE,Fecha Desc";
                Dt_Empleados = Dv_Ordenar.ToTable();
            }
            Dt_Empleados.TableName = "Dt_Catalogo_Empleados";
            Dt_Parametros = Consultar_Parametros_Reporte();
            Dt_Parametros.TableName = "Dt_Parametros";
            Ds_Reporte.Tables.Add(Dt_Empleados.Copy());
            Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

            //Se llama al método que ejecuta la operación de generar el reporte.
            // dependiendo del ordenamiento seleccionado, llamar el reporte correspondiente (cambian campos y ordenamiento)
            if (Cmb_Ordenamiento.SelectedIndex == 0)
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Codigo.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR NUMERO DE EMPLEADO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR SINDICATO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Sindicato.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR RFC"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Rfc.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR PUESTO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Puesto.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR BANCO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Banco.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR ANTIGÜEDAD"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Antiguedad.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "xls", ExportFormatType.Excel);
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        { }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte Catálogo de empleados. Error: [" + Ex.Message + "]");
        }
    }

    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Word_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y ofrece para descarga reporte en formato MS Word. 
    /// PARÁMETROS: No Aplica
    /// USUARIO CREO: Roberto González Oseguera
    /// FECHA CREO: 05-abr-2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Word_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Empleados;//Variable que almacena un listado de empleados.
        DataTable Dt_Parametros;

        try
        {
            Ds_Reporte = new DataSet();
            Dt_Empleados = Consultar_Empleados();
            
            //  para ordenar por rfc
            if (Cmb_Ordenamiento.SelectedValue.Contains("POR RFC"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA_ID,UNIDAD_RESPONSABLE," + Cat_Empleados.Campo_RFC;
                Dt_Empleados = Dv_Ordenar.ToTable();
            }

            //  para ordenar por puesto
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR PUESTO"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA_ID,UNIDAD_RESPONSABLE,PUESTO";
                Dt_Empleados = Dv_Ordenar.ToTable();
            }

            //  para ordenar por ANTIGÜEDAD
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR ANTIGÜEDAD"))
            {
                DataView Dv_Ordenar = new DataView(Dt_Empleados);
                Dv_Ordenar.Sort = "TIPO_NOMINA,UNIDAD_RESPONSABLE,Fecha Desc";
                Dt_Empleados = Dv_Ordenar.ToTable();
            }

            Dt_Empleados.TableName = "Dt_Catalogo_Empleados";
            Dt_Parametros = Consultar_Parametros_Reporte();
            Dt_Parametros.TableName = "Dt_Parametros";
            Ds_Reporte.Tables.Add(Dt_Empleados.Copy());
            Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

            //Se llama al método que ejecuta la operación de generar el reporte.
            // dependiendo del ordenamiento seleccionado, llamar el reporte correspondiente (cambian campos y ordenamiento)
            if (Cmb_Ordenamiento.SelectedIndex == 0)
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Codigo.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR NUMERO DE EMPLEADO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR SINDICATO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Sindicato.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR RFC"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Rfc.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR PUESTO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Puesto.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR BANCO"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Banco.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
            else if (Cmb_Ordenamiento.SelectedValue.Contains("POR ANTIGÜEDAD"))
            {
                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Catalogo_Empleados_Orden_Antiguedad.rpt", "Reporte_Catalogo_Empleados" + Session.SessionID, "doc", ExportFormatType.WordForWindows);
            }
        }
        catch (System.Threading.ThreadAbortException ex)
        { }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte Catálogo de empleados. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion
}
