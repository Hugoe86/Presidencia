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
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;

public partial class paginas_Nomina_Reportes_Nomina_Frm_Rpt_Nom_Empleados : System.Web.UI.Page
{
    #region (Load/Init)
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
    protected DataTable Consultar_Empleados()
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacena una lista de empleados.

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim()))
                Obj_Empleados.P_No_Empleado = Txt_Busqueda_No_Empleado.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim()))
                Obj_Empleados.P_Nombre = Txt_Busqueda_Nombre_Empleado.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_RFC_Empleado.Text.Trim()))
                Obj_Empleados.P_RFC = Txt_Busqueda_RFC_Empleado.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_CURP_Empleado.Text.Trim()))
                Obj_Empleados.P_CURP = Txt_Busqueda_CURP_Empleado.Text.Trim();

            if (Cmb_Busqueda_Tipo_Nomina.SelectedIndex > 0)
                Obj_Empleados.P_Tipo_Nomina_ID = Cmb_Busqueda_Tipo_Nomina.SelectedValue.Trim();

            if (Cmb_Busqueda_Sindicato.SelectedIndex > 0)
                Obj_Empleados.P_Sindicado_ID = Cmb_Busqueda_Sindicato.SelectedValue.Trim();

            if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0)
                Obj_Empleados.P_Dependencia_ID = Cmb_Busqueda_Unidad_Responsable.SelectedValue.Trim();

            if (Cmb_Busqueda_Areas.SelectedIndex > 0)
                Obj_Empleados.P_Area_ID = Cmb_Busqueda_Areas.SelectedValue.Trim();

            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                Obj_Empleados.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();

            if (!String.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text.Replace("__/___/____", "").Trim()) &&
                !String.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text.Replace("__/___/____", "").Trim()))
            {
                if (Validar_Datos())
                {
                    Obj_Empleados.P_Fecha_Inicio_Busqueda = Txt_Busqueda_Fecha_Inicio.Text.Trim();
                    Obj_Empleados.P_Fecha_Fin_Busqueda = Txt_Busqueda_Fecha_Fin.Text.Trim();
                    Dt_Empleados = Obj_Empleados.Consultar_Rpt_Empleados();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            else
            {
                Dt_Empleados = Obj_Empleados.Consultar_Rpt_Empleados();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los empleados. Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados;
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
            Cargar_Combos(Cmb_Busqueda_Tipo_Nomina, Dt_Tipos_Nominas, Cat_Nom_Tipos_Nominas.Campo_Nomina,
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
    protected void Consultar_Sindicatos() {
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
            Cargar_Combos(Cmb_Busqueda_Unidad_Responsable, Dt_Unidades_Responsables, Cat_Dependencias.Campo_Nombre,
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
            Cargar_Combos(Cmb_Busqueda_Areas, Dt_Areas, Cat_Areas.Campo_Nombre, Cat_Areas.Campo_Area_ID, 0);
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
    private Boolean Validar_Datos()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "";

        if (!string.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text) && !string.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text))
        {
            if ((Validar_Formato_Fecha(Txt_Busqueda_Fecha_Inicio.Text.Trim())) && (Validar_Formato_Fecha(Txt_Busqueda_Fecha_Inicio.Text.Trim())))
            {
                if (!Validar_Fechas(Txt_Busqueda_Fecha_Inicio.Text, Txt_Busqueda_Fecha_Fin.Text))
                {
                    Lbl_Mensaje_Error.Text += "+ Fecha Fin no puede ser menor que la Fecha Inicio. <br>";
                    Datos_Validos = false;
                }
            }
            else {
                Lbl_Mensaje_Error.Text += "+ Formato de Fecha Incorrecto <br>";
                Datos_Validos = false;
            }
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
            if (Reporte is ReportDocument) {
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
    /// *************************************************************************************
    /// NOMBRE: Mostrar_Excel
    /// 
    /// DESCRIPCIÓN: Muestra el reporte en excel.
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    private void Mostrar_Excel(CarlosAg.ExcelXmlWriter.Workbook Libro, String Nombre_Archivo)
    {
        try
        {
            Response.Clear();
            Response.Buffer = true;
            Response.ContentType = "application/vnd.ms-excel";
            Response.AddHeader("Content-Disposition", "attachment;filename=" + Nombre_Archivo);
            Response.Charset = "UTF-8";
            Response.ContentEncoding = Encoding.Default;
            Libro.Save(Response.OutputStream);
            Response.End();
        }
        catch (Exception Ex)
        {
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
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 3/Mayo/2011 12:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte = null;
        DataTable Dt_Empleados = null;//Variable que almacena un listado de empleados.
        try
        {
            Ds_Reporte = new DataSet();
            Dt_Empleados = Consultar_Empleados();
            Dt_Empleados.TableName = "Empleados";
            Ds_Reporte.Tables.Add(Dt_Empleados.Copy());

            //Se llama al método que ejecuta la operación de generar el reporte.
            Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Empleados.rpt", "Reporte_Empleados" + Session.SessionID + ".pdf");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Btn_Generar_Reporte_Minimo_Click
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda, Genera y muestra el reporte. 
    ///              
    /// PARÁMETROS: No Aplicá
    /// 
    /// USUARIO CREO: Juan Alberto Hernández Negrete.
    /// FECHA CREO: 10/Diciembre/2011.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Minimo_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Empleados = null;//Variable que almacena un listado de empleados.
        CarlosAg.ExcelXmlWriter.Workbook Libro = null;//Creamos la variable que almacenara el libro de excel.

        try
        {
            //Consultamos la información del empleado.
            Dt_Empleados = Consultar_Empleados();
            //Obtenemos el libro.
            Libro = Cls_Ayudante_Crear_Excel.Generar_Excel(Dt_Empleados);
            //Mandamos a imprimir el reporte en excel.
            Mostrar_Excel(Libro, "Empleados.xls");
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte de empleados minimo. Error: [" + Ex.Message + "]");
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
            if (Cmb_Busqueda_Unidad_Responsable.SelectedIndex > 0) {
                Consultar_Areas(Cmb_Busqueda_Unidad_Responsable.SelectedValue.Trim());
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

    #endregion
}
