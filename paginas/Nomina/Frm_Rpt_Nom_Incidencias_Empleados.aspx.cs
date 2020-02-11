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
using System.Text;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Empleados.Negocios;
using Presidencia.Sindicatos.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Areas.Negocios;
using System.Text.RegularExpressions;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data;
using Presidencia.Prestamos.Negocio;
using Presidencia.Calendario_Nominas.Negocios;

public partial class paginas_Nomina_Frm_Rpt_Nom_Incidencias_Empleados : System.Web.UI.Page
{

    #region (Init/Load)
    /// *************************************************************************************
    /// NOMBRE: Page_Load
    /// 
    /// DESCRIPCIÓN: Habilita la configuración inicial de la página.
    ///              
    /// PARÁMETROS: No Aplicá
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
    /// PARÁMETROS: No Aplicá
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
            Consultar_Calendarios_Nomina();     //Variable que consulta las nominas que existen actualmente activas en sistema.
            Cmb_Calendario_Nomina.Focus();
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
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:34 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Consultar_Incidencias_Empleados()
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Incidencias = null;//Variable que almacena una lista de empleados.

        try
        {
            if (!String.IsNullOrEmpty(RBtn_Tipo_Incidencia.SelectedItem.Text.Trim()))
                Obj_Empleados.P_Tipo_Incidencia = RBtn_Tipo_Incidencia.SelectedItem.Text.Trim();

            if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                Obj_Empleados.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
                Obj_Empleados.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim();

            if (Validar_Fechas_Incidencia())
            {
                if (!String.IsNullOrEmpty(Txt_Incidencia_Fecha_Inicio.Text.Trim()))
                    Obj_Empleados.P_Fecha_Inicio_Busqueda_Incidencia = Txt_Incidencia_Fecha_Inicio.Text.Trim();

                if (!String.IsNullOrEmpty(Txt_Incidencia_Fecha_Fin.Text.Trim()))
                    Obj_Empleados.P_Fecha_Fin_Busqueda_Incidencia = Txt_Incidencia_Fecha_Fin.Text.Trim();
            }

            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text.Trim()))
                Obj_Empleados.P_No_Empleado = Txt_No_Empleado.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                Obj_Empleados.P_Nombre = Txt_Nombre_Empleado.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_RFC_Empleado.Text.Trim()))
                Obj_Empleados.P_RFC = Txt_Busqueda_RFC_Empleado.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_CURP_Empleado.Text.Trim()))
                Obj_Empleados.P_CURP = Txt_Busqueda_CURP_Empleado.Text.Trim();

            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Obj_Empleados.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

            if (Cmb_Sindicato.SelectedIndex > 0)
                Obj_Empleados.P_Sindicado_ID = Cmb_Sindicato.SelectedValue.Trim();

            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                Obj_Empleados.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue.Trim();

            if (Cmb_Area.SelectedIndex > 0)
                Obj_Empleados.P_Area_ID = Cmb_Area.SelectedValue.Trim();

            if (Cmb_Estatus.SelectedIndex > 0)
                Obj_Empleados.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();

            if (Validar_Fechas_Empleado())
            {
                Obj_Empleados.P_Fecha_Inicio_Busqueda = Txt_Busqueda_Fecha_Inicio.Text.Trim();
                Obj_Empleados.P_Fecha_Fin_Busqueda = Txt_Busqueda_Fecha_Fin.Text.Trim();
            }

            Dt_Incidencias = Obtener_Tabla_Por_Tipo_Incidencia(Obj_Empleados);

            if (Dt_Incidencias is DataTable)
            {
                if (Dt_Incidencias.Rows.Count <= 0) {
                    throw new Exception("No se encontaron resultados en la búsqueda realizada.");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los empleados. Error: [" + Ex.Message + "]");
        }
        return Dt_Incidencias;
    }
    #endregion

    #region (Consultas Combos)
    /// *************************************************************************************
    /// NOMBRE: Consultar_Tipos_Nominas
    /// 
    /// DESCRIPCIÓN: Consulta los tipos de nómina que se encuantran dadas de alta 
    ///              actualmente en sistema.
    ///              
    /// PARÁMETROS: No Aplicá
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
            Cargar_Combos(Cmb_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
                Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID, 0);//Carga el combo de tipos de nómina.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Sindicatos
    /// 
    /// DESCRIPCIÓN: Consulta los sindicatos que se encuentran registrados actualmente
    ///              en sistema.
    ///              
    /// PARÁMETROS: No Aplicá.
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
            Cargar_Combos(Cmb_Sindicato, Dt_Sindicatos, Cat_Nom_Sindicatos.Campo_Nombre,
                Cat_Nom_Sindicatos.Campo_Sindicato_ID, 0);//Carga el combo de sindicatos.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los sindicatos que existen actualmente en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Unidades_Responsables
    /// 
    /// DESCRIPCIÓN: Consulta las Unidades responsables que se encuentran registrados actualmente
    ///              en sistema.
    ///              
    /// PARÁMETROS: No Aplicá.
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
            Cargar_Combos(Cmb_Unidad_Responsable, Dt_Unidades_Responsables, Cat_Dependencias.Campo_Nombre,
                Cat_Dependencias.Campo_Dependencia_ID, 0);//Se carga el control que almacena las unidades responsables.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Areas
    /// 
    /// DESCRIPCIÓN: Consulta las Areas que se encuentran registrados actualmente
    ///              en sistema.
    ///              
    /// PARÁMETROS: No Aplicá.
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 12:02 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Consultar_Areas(String Unidad_Responsable_ID)
    {
        Cls_Cat_Areas_Negocio Obj_Areas = new Cls_Cat_Areas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Areas = null;//Variable que almacena un listado de  las areas que le pertencen a la Unidad responsable.

        try
        {
            Obj_Areas.P_Dependencia_ID = Unidad_Responsable_ID;
            Dt_Areas = Obj_Areas.Consulta_Areas();
            Cargar_Combos(Cmb_Area, Dt_Areas, Cat_Areas.Campo_Nombre, Cat_Areas.Campo_Area_ID, 0);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las areas que le pertencen a la unidad responsable seleccionada. Error: [" + Ex.Message + "]");
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
    /// ********************************************************************************
    /// Nombre: Validar_Datos
    /// Descripción: Validar Campos
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 3/Mayo/2011 12:20p.m.
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Fechas_Empleado()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "";

        if (!String.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text.Replace("__/___/____", "").Trim()))
        {
            if (!Validar_Formato_Fecha(Txt_Busqueda_Fecha_Inicio.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "+ Fecha Inicio no tiene un formato correcto [Dia/Mes/Año]. <br>";
                Datos_Validos = false;
            }
        }
        else Datos_Validos = false;

        if (!String.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text.Replace("__/___/____", "").Trim()))
        {
            if (!Validar_Formato_Fecha(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "+ Fecha Fin no tiene un formato correcto [Dia/Mes/Año]. <br>";
                Datos_Validos = false;
            }
        }
        else Datos_Validos = false;

        return Datos_Validos;
    }
    /// ********************************************************************************
    /// Nombre: Validar_Datos
    /// Descripción: Validar Campos
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 3/Mayo/2011 12:20p.m.
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Fechas_Incidencia()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "";

        if (!String.IsNullOrEmpty(Txt_Incidencia_Fecha_Inicio.Text.Replace("__/___/____", "").Trim()))
        {
            if (!Validar_Formato_Fecha(Txt_Incidencia_Fecha_Inicio.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "+ Fecha Inicio no tiene un formato correcto [Dia/Mes/Año]. <br>";
                Datos_Validos = false;
            }
        }
        else Datos_Validos = false;

        if (!String.IsNullOrEmpty(Txt_Incidencia_Fecha_Fin.Text.Replace("__/___/____", "").Trim()))
        {
            if (!Validar_Formato_Fecha(Txt_Incidencia_Fecha_Fin.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text += "+ Fecha Fin no tiene un formato correcto [Dia/Mes/Año]. <br>";
                Datos_Validos = false;
            }
        }
        else Datos_Validos = false;

        return Datos_Validos;
    }
    /// ********************************************************************************
    /// Nombre: Validar_Datos_Requeridos_Consulta
    /// Descripción: Validar Campos
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 3/Mayo/2011 12:20p.m.
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    protected Boolean Validar_Datos_Requeridos_Consulta()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario proporcionar: <br/>";

        try
        {
            if (Cmb_Calendario_Nomina.SelectedIndex <= 0) {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;+ El año de la nomina es un dato requerido para la consulta. <br>";
                Datos_Validos=false;
            }

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0) {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp+ El periodo es un dato requerido para la consulta. <br>";
                Datos_Validos= false;
            }

            if (RBtn_Tipo_Incidencia.SelectedIndex == -1)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp+ Seleccione el Tipo de Incidencia a generar el reporte. <br>";
                Datos_Validos= false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar los datos requeridos por la consulta. Error: [" + Ex.Message + "]");
        }

        return Datos_Validos;
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
    /// ********************************************************************************
    /// Nombre: Validar_Fechas
    /// Descripción: Valida que la Fecha Inicial no sea mayor que la Final
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 3/Mayo/2011 12:20p.m.
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private Boolean Validar_Fechas(String _Fecha_Inicio, String _Fecha_Fin)
    {
        DateTime Fecha_Inicio = Convert.ToDateTime(_Fecha_Inicio);
        DateTime Fecha_Fin = Convert.ToDateTime(_Fecha_Fin);
        Boolean Fecha_Valida = false;
        if (Fecha_Inicio < Fecha_Fin) Fecha_Valida = true;
        return Fecha_Valida;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 3/Mayo/2011 12:20p.m.
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean IsNumeric(String Cadena)
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
    #endregion

    #region (Calendario Nomina)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Calendario_Nomina.DataBind();
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf
                    (Cmb_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                {
                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Periodos_Catorcenales_Nomina(String Nomina_ID)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nomina_Periodos = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Clase de conexion con la capa de negocios
        DataTable Dt_Periodos_Catorcenales = null;//Variable que almacenra unaa lista de los periodos catorcenales que le correspónden a la nomina seleccionada.

        try
        {
            Consulta_Calendario_Nomina_Periodos.P_Nomina_ID = Nomina_ID;
            Dt_Periodos_Catorcenales = Consulta_Calendario_Nomina_Periodos.Consulta_Detalles_Nomina();
            if (Dt_Periodos_Catorcenales != null)
            {
                if (Dt_Periodos_Catorcenales.Rows.Count > 0)
                {
                    Cmb_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodos_Catorcenales_Nomina);
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));

                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron periodos catorcenales para la nomina seleccionada.";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los periodos catorcenales del  calendario de nomina seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();
        DataRow Renglon_Dt_Clon = null;
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        if (Dt_Calendario_Nominas is DataTable)
        {
            foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
            {
                if (Renglon is DataRow)
                {
                    Renglon_Dt_Clon = Dt_Nominas.NewRow();
                    Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
                    Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
                    Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
                }
            }
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;
        DateTime Fecha_Inicio = new DateTime();
        DateTime Fecha_Fin = new DateTime();

        Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

        foreach (ListItem Elemento in Combo.Items)
        {
            if (IsNumeric(Elemento.Text.Trim()))
            {
                Prestamos.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());
                    }
                }
            }
        }
    }
    #endregion

    #region (Métodos Generar Reporte)
    /// *************************************************************************************
    /// NOMBRE: Obtener_Tabla_Por_Tipo_Incidencia
    /// 
    /// DESCRIPCIÓN: Consulta y obtiene un listado de las incidencias que tiene el empleado
    ///              de acuerdo al tipo de incidencia seleccionado.             
    /// 
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 5/Mayo/2011 18:39 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Obtener_Tabla_Por_Tipo_Incidencia(Cls_Cat_Empleados_Negocios Obj_Empleados)
    {
        String Tipo_Incidencia = String.Empty;//Variable que almacena el tipo de incidencia a generar el reporte.
        DataTable Dt_Incidencias = null;//Variable que almacenara la lista de incidencias que se mostraran en el reporte.

        try
        {
            //Obtenemos el tipo de incidencia.
            Tipo_Incidencia = RBtn_Tipo_Incidencia.SelectedItem.Text.ToUpper().Trim();

            switch (Tipo_Incidencia)
            {
                case "FALTAS":
                    Dt_Incidencias = Obj_Empleados.Consulta_Rpt_Incidencias_Empleados();
                    break;
                case "RETARDOS":
                    Dt_Incidencias = Obj_Empleados.Consulta_Rpt_Incidencias_Empleados();
                    break;
                case "VACACIONES":
                    Dt_Incidencias = Obj_Empleados.Consulta_Rpt_Vacaciones_Empleados();
                    break;
                case "TIEMPO EXTRA":
                    Dt_Incidencias = Obj_Empleados.Consulta_Rpt_Tiempo_Extra_Empleados();
                    break;

                default:
                    break;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al realizar la consulta por tipo de incidencia. Error: [" + Ex.Message + "]");
        }
        return Dt_Incidencias;
    }
    /// *************************************************************************************
    /// NOMBRE: Generar_Rpt_Incidencias
    /// 
    /// DESCRIPCIÓN: Genera el reporte de incidencias segun el tipo de incidencia
    ///              seleccionada.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 5/Mayo/2011 18:36 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Generar_Rpt_Incidencias()
    {
        DataSet Ds_Reporte = null;//Variable que almacena las tablas que se utilizaran al generar el reporte de incidencias.
        DataTable Dt_Datos_Reporte = null;//Variable que almacena un listado de empleados.
        String Tipo_Incidencia = String.Empty;//Variable que almacena el tipo de incidencia a generar el reporte.
        DataTable Dt_Incidencias = null;//Variable que almacenara la lista de incidencias que se mostraran en el reporte.

        try
        {
            //Obtenemos el tipo de incidencia.
            Tipo_Incidencia = RBtn_Tipo_Incidencia.SelectedItem.Text.ToUpper().Trim();

            switch (Tipo_Incidencia)
            {
                case "FALTAS":
                    Ds_Reporte = new DataSet();
                    Dt_Datos_Reporte = Consultar_Incidencias_Empleados();
                    Dt_Datos_Reporte.TableName = "Faltas_Retardos";
                    Ds_Reporte.Tables.Add(Dt_Datos_Reporte.Copy());

                    //Se llama al método que ejecuta la operación de generar el reporte.
                    Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Faltas_Retardos_Empleado.rpt", "Faltas_Retardos_" + Session.SessionID + ".pdf");
                    break;
                case "RETARDOS":
                    Ds_Reporte = new DataSet();
                    Dt_Datos_Reporte = Consultar_Incidencias_Empleados();
                    Dt_Datos_Reporte.TableName = "Faltas_Retardos";
                    Ds_Reporte.Tables.Add(Dt_Datos_Reporte.Copy());

                    //Se llama al método que ejecuta la operación de generar el reporte.
                    Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Faltas_Retardos_Empleado.rpt", "Faltas_Retardos_" + Session.SessionID + ".pdf");
                    break;
                case "VACACIONES":
                    Ds_Reporte = new DataSet();
                    Dt_Datos_Reporte = Consultar_Incidencias_Empleados();
                    //  se remueve la columna de no_vacacion sin formato de numero
                    Dt_Datos_Reporte.Columns.RemoveAt(1);
                    Dt_Datos_Reporte.TableName = "Vacaciones";

                    //  PARA ORDENAR LA TABLA POR DEPENDENCIA Y NOMBRE
                    DataView Dv_Ordenar = new DataView(Dt_Datos_Reporte);
                    Dv_Ordenar.Sort = "DEPENDENCIA,EMPLEADO";
                    Dt_Datos_Reporte = Dv_Ordenar.ToTable();

                    Ds_Reporte.Tables.Add(Dt_Datos_Reporte.Copy());
                    //  se cambia el nombre a la columna de no_vacacion que tiene formato numerico
                    Ds_Reporte.Tables[0].Columns[0].ColumnName = "NO_VACACION";

                    //Se llama al método que ejecuta la operación de generar el reporte.
                    Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Vacaciones_Empleados.rpt", "Vacaciones_" + Session.SessionID + ".pdf");
                    break;

                case "TIEMPO EXTRA":
                    Ds_Reporte = new DataSet();
                    Dt_Datos_Reporte = Consultar_Incidencias_Empleados();
                    Dt_Datos_Reporte.TableName = "Tiempo_Extra";
                    Ds_Reporte.Tables.Add(Dt_Datos_Reporte.Copy());

                    //Se llama al método que ejecuta la operación de generar el reporte.
                    Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Tiempo_Extra_Empleados.rpt", "Tiempo_Extra_" + Session.SessionID + ".pdf");
                    break;

                default:
                    break;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al realizar la consulta por tipo de incidencia. Error: [" + Ex.Message + "]");
        }
        return Dt_Incidencias;
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
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 12:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, EventArgs e)
    {
        try
        {
            if (Validar_Datos_Requeridos_Consulta())
            {
                Generar_Rpt_Incidencias();
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (Combos)
    /// *************************************************************************************
    /// NOMBRE: Cmb_Busqueda_Unidad_Responsable_SelectedIndexChanged
    /// 
    /// DESCRIPCIÓN: Carga la lista de areas que le pertecen a la Unidad Responsable
    ///              seleccionada.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 11:55 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Cmb_Busqueda_Unidad_Responsable_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
            {
                Consultar_Areas(Cmb_Unidad_Responsable.SelectedValue.Trim());
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
        Cmb_Unidad_Responsable.Focus();
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 06/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        Cmb_Calendario_Nomina.Focus();
    }
    #endregion

    #endregion
}
