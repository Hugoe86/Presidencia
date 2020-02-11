using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Windows.Forms;
using System.Xml.Linq;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Empleados.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Generar_Nomina.Negocio;
using Presidencia.Archivos_Historial_Nomina_Generada;
using System.Text;
using System.IO;
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Calculo_Percepciones.Negocio;
using Presidencia.DateDiff;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Recibos_Empleados.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Parametros_Contables.Negocio;
using Presidencia.Informacion_Presupuestal;
using Presidencia.Ayudante_Informacion;
using Presidencia.Dependencias.Negocios;

public partial class paginas_Nomina_Frm_Ope_Nom_Generar_Nomina : System.Web.UI.Page
{
    #region (Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///
    ///DESCRIPCIÓN: Despues de cargar la página se habilita la configuración inicial
    ///             de los controles de la página.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.

                Configuracion_Inicial();//Habilita la configuración inicial de los controles de la página. 
            }
            //Limpiamos algún mensaje de error que se halla quedado en el log, que muestra los errores del sistema.
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

    #region (Metodos)

    #region (Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///
    ///DESCRIPCIÓN: Habilita la configuración inicial de los controles
    ///             página de generación de la nómina.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial() {
        Cargar_Combo_Tipos_Nomina();//Se cargan los tipos de nómina que existen actualmente en el sistema.
        Consultar_Calendario_Nominas();//Se consultan los calendarios de nómina que existen actualmente en el sistema.
        Btn_Generar_Nomina.Visible = false;
        Btn_Cerrar_Nomina.Visible = false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    ///             
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Validar_Periodos_Pago(DropDownList Combo)
    {
        Cls_Ope_Nom_Pestamos_Negocio Prestamos_Negocio = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Actual = DateTime.Now;//Variable que almacenará la fecha actual.
        DateTime Fecha_Inicio = new DateTime();//Variable que almacenará la fecha de inicio de la catorcena.
        DateTime Fecha_Fin = new DateTime();//Variable que almacenará la fecha final de la catorcena.
        Fecha_Actual = Fecha_Actual.AddDays(-14);//Obtenemos la fecha inicial dela catorcena anterior.

        Prestamos_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();//Obtenemos la nomina seleccionada.

        foreach (ListItem Elemento in Combo.Items)
        {
            if (Es_Numerico(Elemento.Text.Trim()))
            {
                Prestamos_Negocio.P_No_Nomina = Convert.ToInt32(Elemento.Text.Trim());
                Dt_Detalles_Nomina = Prestamos_Negocio.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        //if (Fecha_Fin >= Fecha_Actual)
                        //{
                        //    Elemento.Enabled = true;
                        //}
                        //else
                        //{
                        //    Elemento.Enabled = false;
                        //}
                    }
                }
            }
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
    /// FECHA_CREO  : 19/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private DataTable Formato_Fecha_Calendario_Nomina(DataTable Dt_Calendario_Nominas)
    {
        DataTable Dt_Nominas = new DataTable();//Variable que almacenara los calendarios de nóminas.
        DataRow Renglon_Dt_Clon = null;//Variable que almacenará un renglón del calendario de la nómina.

        //Creamos las columnas.
        Dt_Nominas.Columns.Add("Nomina", typeof(System.String));
        Dt_Nominas.Columns.Add(Cat_Nom_Calendario_Nominas.Campo_Nomina_ID, typeof(System.String));

        foreach (DataRow Renglon in Dt_Calendario_Nominas.Rows)
        {
            Renglon_Dt_Clon = Dt_Nominas.NewRow();
            Renglon_Dt_Clon["Nomina"] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString().Split(new char[] { ' ' })[0].Split(new char[] { '/' })[2];
            Renglon_Dt_Clon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID] = Renglon[Cat_Nom_Calendario_Nominas.Campo_Nomina_ID];
            Dt_Nominas.Rows.Add(Renglon_Dt_Clon);
        }
        return Dt_Nominas;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Es_Numerico
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 19/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numerico(String Cadena)
    {
        Boolean Resultado = true;//Almacena el resultado true si es numerica o false si no lo es.
        Char[] Array = Cadena.ToCharArray();//Obtenemos un arreglo de carácteres a partir de la cadena que se recibio como parámetro el método.

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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Generacion_Nomina
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos de forma correcta.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Generacion_Nomina()
    {
        Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Tipo_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Tipo Nómina <br>";
            Datos_Validos = false;
        }

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Año de la Nómina <br>";
            Datos_Validos = false;
        }

        if (Cmb_Periodo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Periodo de la Nómina  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Inicia_Catorcena.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Inicio del Periodo  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fin_Catorcena.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Fin del Periodo  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(RBL_Tipos_Nominas.SelectedValue.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione la Nómina a Generar. Ej. [Catorcenal, Prima Vacacional I ó Prima Vacacional II & Aguinaldo Integrado] <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Cerrar_Nomina
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos de forma correcta.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Cerrar_Nomina()
    {
        Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Tipo_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Tipo Nómina <br>";
            Datos_Validos = false;
        }

        if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Año de la Nómina <br>";
            Datos_Validos = false;
        }

        if (Cmb_Periodo.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Periodo de la Nómina  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Inicia_Catorcena.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Inicio del Periodo  <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fin_Catorcena.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Fin del Periodo  <br>";
            Datos_Validos = false;
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : limpia los constroles de la página para realizar la siguiente operación.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 1/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Cmb_Calendario_Nomina.SelectedIndex = -1;
            Cmb_Periodo.SelectedIndex = -1;
            Cmb_Tipo_Nomina.SelectedIndex = -1;
            Txt_Fin_Catorcena.Text = "";
            Txt_Inicia_Catorcena.Text = "";
            RBL_Tipos_Nominas.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina de generacion de la nómina. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Consulta)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Tipos_Nomina
    ///DESCRIPCIÓN: Consulta y carga el ctrl que almacena los tipos de nómina que se 
    ///             encuentran dados de alta actualmente en el sistema.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Combo_Tipos_Nomina()
    {
        Cls_Cat_Tipos_Nominas_Negocio Tipos_Nominas = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nominas = null;//Variable que almacenara una lista de las nominas que existe actualmente en el sistema.

        try
        {
            Dt_Tipos_Nominas = Tipos_Nominas.Consulta_Datos_Tipo_Nomina();
            Cmb_Tipo_Nomina.DataSource = Dt_Tipos_Nominas;
            Cmb_Tipo_Nomina.DataTextField = Cat_Nom_Tipos_Nominas.Campo_Nomina;
            Cmb_Tipo_Nomina.DataValueField = Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID;
            Cmb_Tipo_Nomina.DataBind();
            Cmb_Tipo_Nomina.Items.Insert(0, new ListItem("< Seleccione >",""));
            Cmb_Tipo_Nomina.SelectedIndex = 0;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los tipos de nómina que existén actualmente en el sistema. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales para el 
    ///calendario de nomina seleccionado.
    ///PARAMETROS: Nomina_ID.- Indica el calendario de nomina del cuál se desea consultar
    ///                        los periodos catorcenales.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
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
                    Cmb_Periodo.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Periodo.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Periodo.DataValueField = Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID;
                    Cmb_Periodo.DataBind();
                    Cmb_Periodo.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Periodo.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Periodo);
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
    ///NOMBRE DE LA FUNCIÓN: Consultar_Calendario_Nominas
    ///DESCRIPCIÓN: Consulta los calendarios de nomina vigentes actualmente.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Calendario_Nominas()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Consulta_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendario_Nominas = null;//Variable que almacenara una lista de los calendarios de nomina vigentes.
        try
        {
            Dt_Calendario_Nominas = Consulta_Calendario_Nominas.Consultar_Calendario_Nominas();
            if (Dt_Calendario_Nominas != null)
            {
                if (Dt_Calendario_Nominas.Rows.Count > 0)
                {
                    Dt_Calendario_Nominas = Formato_Fecha_Calendario_Nomina(Dt_Calendario_Nominas);
                    Cmb_Calendario_Nomina.DataSource = Dt_Calendario_Nominas;
                    Cmb_Calendario_Nomina.DataTextField = "Nomina";
                    Cmb_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                    Cmb_Calendario_Nomina.DataBind();
                    Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Calendario_Nomina.SelectedIndex = -1;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron nominas vigentes";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las nominas. Error: [" + Ex.Message + "]");
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
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Nominas_Negativas",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Validación)
    /// ***********************************************************************************************
    /// Nombre: Consultar_Nominas_Negativas
    /// 
    /// Descripción: Consulta una vez generada la nómina si hay algunos empleados con nómina negativa.
    ///              Si se encontraron algunos empleados en esta situación se muestra un reporte con 
    ///              los empleados que se encuentran en esta situación.
    /// 
    /// Parámetros: No Aplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 08/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************************
    protected void Consultar_Nominas_Negativas()
    {
        Cls_Ope_Nom_Recibos_Empleados_Negocio Obj_Recibos = new Cls_Ope_Nom_Recibos_Empleados_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Nominas_Negativas = null;//Variable que almacena los registros de las nominas que salieron en negativo.
        DataSet Ds_Datos = null;//Variable que almacenara la tabla con las nóminas en negativo.

        try
        {
            Obj_Recibos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Recibos.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
            Obj_Recibos.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

            Dt_Nominas_Negativas = Obj_Recibos.Consultar_Recibos_Con_Nomina_Negativa();

            if (Dt_Nominas_Negativas is DataTable)
            {
                if (Dt_Nominas_Negativas.Rows.Count > 0)
                {
                    Ds_Datos = new DataSet();
                    Dt_Nominas_Negativas.TableName = "Nominas_Negativas";
                    Ds_Datos.Tables.Add(Dt_Nominas_Negativas.Copy());

                    Generar_Reporte(ref Ds_Datos, "Cr_Rpt_Nom_Nominas_Negativas.rpt",
                        "Rpt_Nom_Nominas_Negativas_" + Session.SessionID + ".pdf");
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las nominas negativas de los empleados. Error: [" + Ex.Message + "]");
        }
    }
    /// **********************************************************************************************************
    /// Nombre: Validar_Si_Nomina_Fue_Cerrada
    /// 
    /// Descripción: Valida si la nómina para el periodo seleccionada ya se encuentra en un estado de cerrada.
    /// 
    /// Parámtros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 15/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// **********************************************************************************************************
    private Boolean Validar_Si_Nomina_Fue_Cerrada()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominas = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Calendario_Nomina = null;//Variable que guardara el registro si la nomina ya fue generada.
        Boolean Estatus = false;
        String Detalle_Nomina_ID = String.Empty;
        DataTable Dt_Nomina_Detalle = null;

        try
        {
            if (Validar_Datos_Cerrar_Nomina())
            {
                Obj_Calendario_Nominas.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Obj_Calendario_Nominas.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                Dt_Nomina_Detalle = Obj_Calendario_Nominas.Consulta_Periodos_Nomina();

                if (Dt_Nomina_Detalle is DataTable)
                {
                    if (Dt_Nomina_Detalle.Rows.Count > 0)
                    {
                        foreach (DataRow Detalle in Dt_Nomina_Detalle.Rows)
                        {
                            if (!string.IsNullOrEmpty(Detalle[Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID].ToString()))
                            {
                                Detalle_Nomina_ID = Detalle[Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID].ToString();

                                Obj_Calendario_Nominas.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();
                                Obj_Calendario_Nominas.P_Detalle_Nomina_ID = Detalle_Nomina_ID;

                                Dt_Calendario_Nomina = Obj_Calendario_Nominas.Consulta_Periodo_Cerrado();

                                if (Dt_Calendario_Nomina is DataTable)
                                {
                                    if (Dt_Calendario_Nomina.Rows.Count > 0)
                                    {
                                        Estatus = true;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al revizar si la nomina ya fue cerrada. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    /// **********************************************************************************************************
    /// Nombre: Validar_Nomina_Prima_Vacacional
    /// 
    /// Descripción: Valida que la generacion de nómina de prima vacacional solo se genere en el periodo indicado
    ///              como parámetro para la generación de este tipo de nómina.
    /// 
    /// Parámtros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 15/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// **********************************************************************************************************
    protected Boolean Validar_Nomina_Prima_Vacacional()
    {
        Cls_Cat_Nom_Parametros_Negocio Obj_Cat_Parametros = new Cls_Cat_Nom_Parametros_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Parametros = null;//Variable que almacenara el registro de parámetro.
        DateTime? Fecha_PV_I = null;//variable que almacenara la fecha de generación de la nomina de prima vacacional.
        Boolean Estatus = false;//Varaibale que guarda el estatus de la operación.

        try
        {
            Dt_Parametros = Obj_Cat_Parametros.Consulta_Parametros();

            if (Dt_Parametros is DataTable)
            {
                if (Dt_Parametros.Rows.Count > 0)
                {
                    foreach (DataRow PARAMETRO in Dt_Parametros.Rows)
                    {
                        if (PARAMETRO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString().Trim()))
                            {
                                Fecha_PV_I = Convert.ToDateTime(PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_1].ToString().Trim());

                                if ((((DateTime)Fecha_PV_I) >= Convert.ToDateTime(Txt_Inicia_Catorcena.Text.Trim())) &&
                                    (((DateTime)Fecha_PV_I) <= Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim())))
                                {
                                    Estatus = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar la generacion de nómina de prima vacacional. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    /// **********************************************************************************************************
    /// Nombre: Validar_Nomina_Prima_Vacacional_II_Aguinaldo
    /// 
    /// Descripción: Valida que la generacion de nómina de prima vacacional y aguinaldo integradasolo se genere 
    ///              en el periodo indicado como parámetro para la generación de este tipo de nómina.
    /// 
    /// Parámtros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 15/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// **********************************************************************************************************
    protected Boolean Validar_Nomina_Prima_Vacacional_II_Aguinaldo()
    {
        Cls_Cat_Nom_Parametros_Negocio Obj_Cat_Parametros = new Cls_Cat_Nom_Parametros_Negocio();
        DataTable Dt_Parametros = null;
        DateTime? Fecha_PV_II = null;
        Boolean Estatus = false;

        try
        {
            Dt_Parametros = Obj_Cat_Parametros.Consulta_Parametros();

            if (Dt_Parametros is DataTable)
            {
                if (Dt_Parametros.Rows.Count > 0)
                {
                    foreach (DataRow PARAMETRO in Dt_Parametros.Rows)
                    {
                        if (PARAMETRO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString().Trim()))
                            {
                                Fecha_PV_II = Convert.ToDateTime(PARAMETRO[Cat_Nom_Parametros.Campo_Fecha_Prima_Vacacional_2].ToString().Trim());

                                if ((((DateTime)Fecha_PV_II) >= Convert.ToDateTime(Txt_Inicia_Catorcena.Text.Trim())) &&
                                    (((DateTime)Fecha_PV_II) <= Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim())))
                                {
                                    Estatus = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar la generacion de nómina de prima vacacional. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #region (Periodos Vacacionales)
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ejecutar_Actualizacion_Dias_Periodos_Vacacionales
    ///
    ///DESCRIPCIÓN: Consulta los EMPLEADOS que pertencén a un determinado tipo de nómina y ejecuta por empleado la actualización de sus
    ///             dias de vacaciones que le corresponden por su antiguedad y periodo vacacional en el que se encuentre.
    ///             
    /// PARÁMETROS: Tipo_Nomina_ID.- Identificador o clave única para identificar los tipos de nómina que se encuentran dadas de alta
    ///             en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private void Ejecutar_Actualizacion_Dias_Periodos_Vacacionales(String Tipo_Nomina_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;                                              //Variable que listara a todos los empleados que pertencan a un detrminado tipo de nómina.
        String Empleado_ID = "";                                                    //Variable que almacena el identificador único del empleado.

        try
        {
            //Obtenemos el identificador del tipo de nómina.
            Obj_Empleados.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            //Consultamos la lista de los empelados que pertencen a este tipo de nomina.
            Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow Empleado in Dt_Empleados.Rows)
                    {
                        if (Empleado is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString()))
                            {
                                //Obtenemos el identificador unico del empleado.
                                Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                                //Ejecutamos la insercion ó modificación de la información de los dias disponibles y dias tomados
                                //de los empleados segun sea el caso. Esta informacion se modifica directamente en la tabla de Ope_Nom_Vacaciones_Empl_Det.
                                Insertar_Actualizar_Detalle_Periodo_Vacacional(Empleado_ID);
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar la actualizacion de los dias de vacaciones de acuerdo al periodo vacacional seleccionado. Error: [" + Ex.Message + "]");
        }
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Insertar_Actualizar_Detalle_Periodo_Vacacional
    ///
    ///DESCRIPCIÓN: Consulta el Año nóminal y el periodo vacacional en el que se encuentra actualmente. Consulta si ya existe previamente
    ///             el registro del periodo vacacional para el empleado en la tabla  de Ope_Nom_Vacaciones_Empl_Det. Si el registro ya existe
    ///             se procede a realizar la actualización de los dias disponibles de vacaciones descontando los dias de vacaciones que se 
    ///             tomaron en la nómina generada el el periodo nóminal actual. En caso contrario se realiza la Inserción del registro del
    ///             periodo vacacional del empleado y los dias disponibles serán en realación asu antiguedad dentro de la empresa.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private void Insertar_Actualizar_Detalle_Periodo_Vacacional(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados =
                            new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Vacaciones_Empl_Det = null;                          //Variable que almacena la información del periodo vacacional actual.
        Int32 Anio_Calendario_Nomina = 0;                                 //Variable que almacenara el año del calendario de nómina actual.
        Int32 Periodo_Vacacional_Actual = 0;                              //Variable que almacenara el número periodo vacacional actual.
        Int32 Dias_Vacaciones_Tipo_Nomina = 0;                            //Variable que almacenara los dias de vacaciones de acuardo al tipo de nómina.
        String Tipo_Nomina_ID = "";                                       //Variable que almacena el tipo de nomina del empleado.

        try
        {
            Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();//Obtenemos el año del calendario nominal actual.
            Periodo_Vacacional_Actual = Obtener_Periodo_Vacacional();//Obtenemos el periodo vacacional actual.

            Tipo_Nomina_ID = Obtener_Tipo_Nomina_Empleado(Empleado_ID);
            Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Vacacional_Actual);

            //Establecemos las variables que son requeridas para completar la operacion de búsqueda del periodo vacacional actual. 
            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Vacaciones_Empleados.P_Anio = Anio_Calendario_Nomina;
            Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Vacacional_Actual;
            //Ejecutamos la busqueda de periodos vacacionales.
            Dt_Vacaciones_Empl_Det = Obj_Vacaciones_Empleados.Consultar_Vacaciones_Empl_Det();


            //Validamos la busqueda de periodos vacacionales registrados previamente en el sistema.
            if (Dt_Vacaciones_Empl_Det is DataTable)
            {
                if (Dt_Vacaciones_Empl_Det.Rows.Count > 0)
                {
                    if (Existe_Registrado_Siguiente_Periodo_Vacacional(Empleado_ID))
                    {
                        //Si no se encontro ningún registro. Se hará un Insert del siguiente periodo vacacional.
                        Alta_Periodo_Vacacional_Actual(Empleado_ID, Dias_Vacaciones_Tipo_Nomina, 0);
                    }
                    Modificar_Periodo_Vacacional_Actual(Empleado_ID);
                }
                else
                {
                    if (Antiguedad_Empleado_Es_Un_Anio(Empleado_ID))
                    {
                        //Si no se encontro ningún registro. Se hará un Insert del periodo vacacional.
                        Alta_Periodo_Vacacional_Actual(Empleado_ID, Dias_Vacaciones_Tipo_Nomina, 0);
                        //Inmediatamente despues de realizar el insert, se hace la modificación de los dias disponibles y de los dias tomados
                        //en el periodo vacacional actual.
                        Modificar_Periodo_Vacacional_Actual(Empleado_ID);
                    }
                    else
                    {
                        //Si no se encontro ningún registro. Se hará un Insert del periodo vacacional.
                        Alta_Periodo_Vacacional_Actual(Empleado_ID, Dias_Vacaciones_Base_Formula(Empleado_ID), 0);
                        //Inmediatamente despues de realizar el insert, se hace la modificación de los dias disponibles y de los dias tomados
                        //en el periodo vacacional actual.
                        Modificar_Periodo_Vacacional_Actual(Empleado_ID);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Insertar o Actualizar un Detalle de las Vacaciones del Empleado. Error: [" + Ex.Message + "]");
        }
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Periodo_Vacacional
    ///
    ///DESCRIPCIÓN: Consulta y obtiene el periodo vacacional en el que se encuentra el empleado. PERIODO I [ENERO - JUNIO] Ó PERIODO II 
    ///             [JULIO - DICIEMBRE].
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private Int32 Obtener_Periodo_Vacacional()
    {
        Int32 Periodo_Vacacional = 0;           //Variable que almacenara el periodo vacacional actual.    
        Int32 Anio_Calendario_Nomina = 0;       //Variable que almacena el año del calendario de la nomina.
        DateTime Fecha_Actual = DateTime.Now;   //Variable que almacena la fecha actual.

        try
        {
            //Consulta el año actual del periodo nóminal.
            Anio_Calendario_Nomina = Obtener_Anio_Calendario_Nomina();

            if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 1, 1)) >= 0) &&
                (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 6, 30)) <= 0))
            {
                //PERIODO VACACIONAL I
                Periodo_Vacacional = 1;
            }
            else if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 7, 1)) >= 0) &&
                (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 12, 31)) <= 0))
            {
                //PERIODO VACACIONAL II
                Periodo_Vacacional = 2;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el periodo vacacional. Error: [" + Ex.Message + "]");
        }
        return Periodo_Vacacional;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Anio_Calendario_Nomina
    ///
    ///DESCRIPCIÓN: Consulta y obtiene el año de la nómina actual. Está búsqueda se realiza en los calendarios de nómina que se encuentran
    ///             registrados actualmente en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private Int32 Obtener_Anio_Calendario_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la clase de negocios.
        DataTable Dt_Calendario_Nomina = null;                                                                      //Variable que guardara la información del calendario de nómina consultado.
        Int32 Anio_Calendario_Nomina = 0;                                                                          //Variable que almacena el año del calendario de nomina.         

        try
        {
            //Consultamos la el calendario de nómina que esta activo actualmente. 
            Dt_Calendario_Nomina = Obj_Calendario_Nomina.Consultar_Calendario_Nomina_Fecha_Actual();

            if (Dt_Calendario_Nomina is DataTable)
            {
                foreach (DataRow Informacion_Calendario_Nomina in Dt_Calendario_Nomina.Rows)
                {
                    if (Informacion_Calendario_Nomina is DataRow)
                    {
                        if (!String.IsNullOrEmpty(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString()))
                        {
                            Anio_Calendario_Nomina = Convert.ToInt32(Informacion_Calendario_Nomina[Cat_Nom_Calendario_Nominas.Campo_Anio].ToString());
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el año del calendario de nómina del año actual. Error: [" + Ex.Message + "]");
        }
        return Anio_Calendario_Nomina;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Insertar_Actualizar_Detalle_Periodo_Vacacional
    ///
    ///DESCRIPCIÓN: Obtiene en base a formúla los dias de vacaciones que le corresponden al empelado según su antiguedad en la empresa.
    ///
    ///             Si Antiguedad Laboral Menor a 1 entoncés:
    ///             
    ///                 Dias_Año [365 ó 366] ---> 20 Dias de Vacaciones al año.
    ///                 N Dias Laborados     --->  X Dias de Vacaciones al año.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private Int32 Dias_Vacaciones_Base_Formula(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;                                              //Variable que almacena la informacion del empleado consultado.     
        Int32 Cantidad_Dias = 0;                                                    //Cantidad de dias que le corresponden al empleado segun su fecha de ingreso a presidencia.
        Int32 DIAS_PERIODO = 0;                                                     //Variable que almacena los dias que tiene el año nominal.
        Int32 Dias = 0;                                                             //Cantidad de dias que lleva el empleado laborando en presidencia.
        Int32 Anio_Calendario_Nomina = 0;                                           //Variable que almacena el año del periodo nominal.
        DateTime? Fecha_Ingreso_Empleado = null;                                    //Variable que alamaceba la fecha de ingreso del empleado a presidencia.
        DateTime Fecha_Actual = DateTime.Now;                                       //Variable que almacena la fecha actual.
        String Tipo_Nomina_ID = "";                                                   //Variable que almacenara el tipo de nómina al que pertence el empleado.
        Int32 Dias_Vacaciones_Tipo_Nomina = 0;                                      //Variable que almacenara los dias de vacaciones que le corresponden al periodo por tipo de nomina consultado.
        Int32 Anio = 0;                                                             //Variable que almacenara el año de calendario de nómina vigente actualmente.         
        Int32 Periodo_Actual = 0;                                                   //Variable que almacenara el periodo vacacional actual.
        Int32 Auxiliar = 0;                                                         //Variable auxiliar que almacenara los dias entre la fecha de ingreso y el 30 de Junio del año actual.
        Int32 Dias_Totales_Primer_Periodo_Vacacional = 0;                           //Variable que almacenara los dias totales del primer periodo [Enero - Junio].
        Int32 Dias_Totales_Segundo_Periodo_Vacacional = 0;                          //Variable que almacenara los dias totales del segundo periodo [Julio - Diciembre].

        try
        {
            Anio = Obtener_Anio_Calendario_Nomina();//Obtenemos el anio del calendario de nomina vigente actualmente.
            Periodo_Actual = Obtener_Periodo_Vacacional();//Obtenemos el periodo vacacional en el que nos encontramos actualmente.

            //Obtenemos el total de dias del periodo de [Enero - Junio] y del periodo [Julio - Diciembre] del anio del calendario de nomina vigente.
            Dias_Totales_Primer_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio, 1, 1), new DateTime(Anio, 6, 30)) + 1;
            Dias_Totales_Segundo_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio, 7, 1), new DateTime(Anio, 12, 31)) + 1;

            //Consultamos el tipo de nomina al que pertence el empleado.
            Tipo_Nomina_ID = Obtener_Tipo_Nomina_Empleado(Empleado_ID);
            //Obtenemos los dias de vacaciones que le corresponden al periodo vacacional de acuerdo al tipo de nómina.
            Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Actual);

            //Consultamos la información general del empleado.
            Obj_Empleados.P_Empleado_ID = Empleado_ID;
            Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

            //Validamos que la búsqueda.
            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow Empleado in Dt_Empleados.Rows)
                    {
                        if (Empleado is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString()))
                            {
                                //Obtenemos la fecha de ingreso del empleado.
                                Fecha_Ingreso_Empleado = Convert.ToDateTime(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString());

                                //Si el año de ingreso del empleado a presidencia es igual a l año actual entoncés:
                                if (((DateTime)Fecha_Ingreso_Empleado).Year == Anio)
                                {
                                    //Identificamos el periodo, si el periodo actual es el primero entonces:
                                    if (Periodo_Actual == 1)
                                    {
                                        //Obtenemos los dias que el empleado lleva laborando en presidencia.
                                        Dias = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), Fecha_Actual) + 1);
                                        //Obtenemos los dias totales del periodo de [Enero - Junio].
                                        DIAS_PERIODO = Dias_Totales_Primer_Periodo_Vacacional;

                                    }
                                    else if (Periodo_Actual == 2)
                                    {
                                        //Si el periodo actual es el segundo entoncés:

                                        //Consultamos los dias que lleva laborando el empleado en presidencia.
                                        Dias = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), Fecha_Actual) + 1);
                                        //Obtenemos los dias laborados del empleado desde su fecha de ingreso hasta el 30 de Junio del año actual.
                                        Auxiliar = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), new DateTime(Anio, 6, 30)));
                                        //A los dias totales que lleva el empleado laborando el empleado en presidencia se le restan los dias que le corresponden 
                                        //al primer periodo, esto quiere decir que solo se consideraran los dias apartir del 1 de Julio del año actual, para el
                                        //calculo de los dias que le corresponden al empleado de vacaciones en base a su antiguedad en presidencia.
                                        Dias = Dias - Auxiliar;
                                        //Obtenemos los dias totales del periodo de [Julio - Dicciembre].
                                        DIAS_PERIODO = Dias_Totales_Segundo_Periodo_Vacacional;
                                    }

                                    //Cantidad de dias que le corresponden al empleado de vacaciones si es que el empleado no tiene un año completo en presidencia
                                    Cantidad_Dias = (Int32)((Dias * Dias_Vacaciones_Tipo_Nomina) / DIAS_PERIODO);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el porcentaje [%] de los dias que le corresponden al" +
                                "empleado en base a la fecha de ingreso que tiene. Error: [" + Ex.Message + "]");
        }
        return Cantidad_Dias;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Alta_Periodo_Vacacional_Actual
    ///
    ///DESCRIPCIÓN: Consulta y obtiene el Año de nómina actual y el periodo en el que nos encontramos actualmente a partir de ahí obtenemos
    ///             el Año [Anterior - Actual - Siguiente] y el Periodo [Anterior - Actual - Siguiente]. Se procede a realizar el alta de 
    ///             del siguiente periodo vacacional.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             Dias_Disponibles.- Son los dias que el empleado tendrá disponibles en ese periodo vacacional.
    ///             Dias_Tomados.- Son los días que el empleado a tomado de este periodo vacacional.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private void Alta_Periodo_Vacacional_Actual(String Empleado_ID, Int32 Dias_Disponibles, Int32 Dias_Tomados)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        Int32 Anio_Periodo_Anterior = 0;    //Variable que almacena el año del periodo vacacional anterior al actual.        
        Int32 Anio_Periodo_Actual = 0;      //Variable que almacena el año del periodo vacacional actual.
        Int32 Anio_Periodo_Siguiente = 0;   //Variable que almacena el año del periodo vacacional siguiente al actual.
        Int32 Periodo_Anterior = 0;         //Variable que almacena el periodo anterior al actual.
        Int32 Periodo_Actual = 0;           //Variable que almacena el periodo actual.
        Int32 Periodo_Siguiente = 0;        //Variable que almacena el periodo siguiente al actual.
        DataTable Dt_Vacaciones_Empl_Det = null;
        Int32 Dias_Vacaciones_Tipo_Nomina = 0;                            //Variable que almacenara los dias de vacaciones de acuardo al tipo de nómina.
        String Tipo_Nomina_ID = "";                                       //Variable que almacena el tipo de nomina del empleado.

        try
        {
            //Consultamos el año del periodo vacacional del empleado.
            Anio_Periodo_Actual = Obtener_Anio_Calendario_Nomina();
            //Consultamos el periodo vacacional del empleado.
            Periodo_Actual = Obtener_Periodo_Vacacional();

            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
            Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Actual;
            Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Actual;
            Dt_Vacaciones_Empl_Det = Obj_Vacaciones_Empleados.Consultar_Vacaciones_Empl_Det();

            if (Dt_Vacaciones_Empl_Det is DataTable)
            {
                if (Dt_Vacaciones_Empl_Det.Rows.Count <= 0)
                {
                    //Establecemos los valores para realizar la operación de alta del periodo vacacional
                    Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                    Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Actual;
                    Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Disponibles;
                    Obj_Vacaciones_Empleados.P_Dias_Tomados = Dias_Tomados;
                    Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Actual;
                    Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                    Obj_Vacaciones_Empleados.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                    //ejecuta el alta.
                    Obj_Vacaciones_Empleados.Alta_Detalle_Vacaciones_Empleados();
                }
            }

            Tipo_Nomina_ID = Obtener_Tipo_Nomina_Empleado(Empleado_ID);
            Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Actual);

            //Identificamos el periodo vacacional que corresponde al siguiente periodo a partir del actual.
            if (Periodo_Actual == 1)
            {
                //Si el periodo actual es el primero entoncés:
                Anio_Periodo_Anterior = Anio_Periodo_Actual - 1;//El año del periodo anterior es el año actual menos 1.
                Anio_Periodo_Siguiente = Anio_Periodo_Actual;//El año del periodo siguiente es el mismo.

                Periodo_Anterior = 2;
                Periodo_Siguiente = 2;
                Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Siguiente);

                //Establecemos los valores requeridos para realizar la inserción del periodo siguiente despues del actual. 
                Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Siguiente;
                Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Vacaciones_Tipo_Nomina;
                Obj_Vacaciones_Empleados.P_Dias_Tomados = 0;
                Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Siguiente;
                Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                Obj_Vacaciones_Empleados.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                //Ejecutamos del alta del periodo siguiente al actual.
                Obj_Vacaciones_Empleados.Alta_Detalle_Vacaciones_Empleados();
            }
            else if (Periodo_Actual == 2)
            {
                //Si el periodo actual es el segundo entoncés:
                Anio_Periodo_Anterior = Anio_Periodo_Actual;//El año del periodo anterior es el mismo.
                Anio_Periodo_Siguiente = Anio_Periodo_Actual + 1;//El año del periodo siguiente es Año actual mas 1.

                Periodo_Anterior = 1;
                Periodo_Siguiente = 1;
                Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Siguiente);

                //Establecemos los valores requeridos para realizar la inserción del periodo siguiente despues del actual. 
                Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Siguiente;
                Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Vacaciones_Tipo_Nomina;
                Obj_Vacaciones_Empleados.P_Dias_Tomados = 0;
                Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Siguiente;
                Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                Obj_Vacaciones_Empleados.P_Usuario_Creo = Cls_Sessiones.Nombre_Empleado;
                //Ejecutamos del alta del periodo siguiente al actual.
                Obj_Vacaciones_Empleados.Alta_Detalle_Vacaciones_Empleados();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al dar de alta el periodo vacacional actual. Error: [" + Ex.Message + "]");
        }
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Modificar_Periodo_Vacacional_Actual
    ///
    ///DESCRIPCIÓN: Consulta el Año del periodo vacacional actual así como también el periodo vacacional actual. Y en base a esta información 
    ///             se obtiene el año [Anterior - Actual - Siguiente] y el Periodo [Anterior - Actual - Siguiente]. YPro ultimos se ejecuta
    ///             la actualización de los dias disponibles y dias tomados del periodo vacacional [Anterior - Actual - Siguiente].
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private void Modificar_Periodo_Vacacional_Actual(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        Cls_Ope_Nom_Percepciones_Negocio Obj_Calculo_Percepciones = new Cls_Ope_Nom_Percepciones_Negocio();              //Variable de conexión con la capa de negocios.
        DataTable Dt_Vacaciones_Empl_Det = null;                                                                        //Variable que almacena la información del periodo vacacional actual.
        Int32 Anio_Periodo_Anterior = 0;                //Variable que almacena el año del periodo vacacional anterior al actual.
        Int32 Anio_Periodo_Actual = 0;                  //Variable que almacena el año del periodo vacacional actual.
        Int32 Anio_Periodo_Siguiente = 0;               //Variable que almacena el año del periodo vacacional siguiente al actual.
        Int32 Periodo_Anterior = 0;                     //Variable que almacena el periodo vacacional anterior al actual.
        Int32 Periodo_Actual = 0;                       //Variable que almacena el periodo vacacional actual.
        Int32 Periodo_Siguiente = 0;                    //Variable que almacena el periodo vacacional siguiente ala actual.
        Int32 Dias_Disponibles_Periodo_Anterior = 0;    //Variable que almacena los dias disponibles de vacaciones del periodo vacacional anterior al actual.
        Int32 Dias_Tomaran_Periodo_Anterior = 0;        //Variable que almacena los dias tomados del periodo de vacaciones siguiente al periodo actual.            
        Int32 Dias_Disponibles_Periodo_Actual = 0;      //Variable que almacena los dias disponibles de vacaciones del periodo actual.
        Int32 Dias_Tomara_Periodo_Actual = 0;           //Variable que almacena los dias tomados del periodo vacacional actual.
        Int32 Dias_Disponibles_Periodo_Siguiente = 0;   //Variable que almacena los dias de vacaciones disponibles del periodo siguiente al actual.
        Int32 Dias_Tomados_Periodo_Siguiente = 0;       //Variable que almacena los dias de vacaciones tomados del periodo siguiente al actual.
        Int32 Dias_Restan_Por_Tomar = 0;                //Variable que almacena los dias que le restan si es que los dias tomados de vacaciones
        //en el periodo nóminal fueran mas de que se tienen disponibles en alguno de los periodos
        //[Anterior - Actual - Siguiente].
        Int32 Dias_Vacaciones_Tomados_Periodo_Actual = 0;//Variable que almacenara los dias de vacaciones tomados del periodo vacacional actual.

        try
        {
            Actualizar_Dias_PV_Antigueadad_Menor_Anio(Empleado_ID);

            Obj_Calculo_Percepciones.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Calculo_Percepciones.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
            Obj_Calculo_Percepciones.P_Fecha_Generar_Nomina = Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim());
            //Paso I .- Consultar los dias tomados en el PERIODO NOMINAL ACTUAL.
            Dias_Vacaciones_Tomados_Periodo_Actual = Obj_Calculo_Percepciones.Obtener_Dias_Vacaciones_Periodo_Actual(Empleado_ID, true);
            //Paso 2.- Obtener el Periodo Vacacional actual [AÑO - PERIODO].
            Anio_Periodo_Actual = Obtener_Anio_Calendario_Nomina();
            Periodo_Actual = Obtener_Periodo_Vacacional();


            //Paso III.- Identificar el periodo vacacional en el que nos encontramos actualmente. Para en base a esa información
            //           Obtener el Año [Anterior - Actual - Siguiente] y el Periodo [Anterior - Actual - Siguiente].
            if (Periodo_Actual == 1)
            {
                //Si el periodo actual es el primero entoncés:
                Anio_Periodo_Anterior = Anio_Periodo_Actual - 1;//El año del periodo anterior es el año actual menos 1.
                Anio_Periodo_Siguiente = Anio_Periodo_Actual;//El año del periodo siguiente es el mismo.

                Periodo_Anterior = 2;
                Periodo_Siguiente = 2;
            }
            else if (Periodo_Actual == 2)
            {
                //Si el periodo actual es el segundo entoncés:
                Anio_Periodo_Anterior = Anio_Periodo_Actual;//El año del periodo anterior es el mismo.
                Anio_Periodo_Siguiente = Anio_Periodo_Actual + 1;//El año del periodo siguiente es Año actual mas 1.

                Periodo_Anterior = 1;
                Periodo_Siguiente = 1;
            }

            //Paso IV.- Consultamos los dias disponibles del periodo anterior. 
            ///Consultamos los dias disponibles del periodo anterior al actual.
            Dias_Disponibles_Periodo_Anterior = Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(Empleado_ID, Anio_Periodo_Anterior,
                Periodo_Anterior);

            //Paso 4.1 .- Revisamos los días disponibles del periodo anterior
            //            si existen dias disponibles se toman los dias del periodo anterior.
            if (Dias_Disponibles_Periodo_Anterior > 0)
            {
                if (Dias_Disponibles_Periodo_Anterior >= Dias_Vacaciones_Tomados_Periodo_Actual)
                {
                    Dias_Tomaran_Periodo_Anterior = Dias_Vacaciones_Tomados_Periodo_Actual;
                    ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional anterior al actual.
                    Actualizar_Periodo(Empleado_ID, Anio_Periodo_Anterior, (Dias_Disponibles_Periodo_Anterior - Dias_Tomaran_Periodo_Anterior),
                        Periodo_Anterior, Dias_Tomaran_Periodo_Anterior);
                }
                else
                {
                    Dias_Tomaran_Periodo_Anterior = Dias_Disponibles_Periodo_Anterior;
                    Dias_Restan_Por_Tomar = (Dias_Vacaciones_Tomados_Periodo_Actual - Dias_Tomaran_Periodo_Anterior);
                    ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional anterior al actual.
                    Actualizar_Periodo(Empleado_ID, Anio_Periodo_Anterior, (Dias_Disponibles_Periodo_Anterior - Dias_Tomaran_Periodo_Anterior),
                        Periodo_Anterior, Dias_Tomaran_Periodo_Anterior);
                    ///Consultamos los dias disponibles del periodo actual.
                    Dias_Disponibles_Periodo_Actual = Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(Empleado_ID, Anio_Periodo_Actual,
                        Periodo_Actual);

                    //Si al tomar los dias disponibles del periodo anterior aun restan dias tomados de vacaciones
                    //se realiza el descuento de los dias al periodo actual.
                    if (Dias_Disponibles_Periodo_Actual > 0)
                    {
                        if (Dias_Disponibles_Periodo_Actual >= Dias_Restan_Por_Tomar)
                        {
                            Dias_Tomara_Periodo_Actual = Dias_Restan_Por_Tomar;

                            ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional actual.
                            Actualizar_Periodo(Empleado_ID, Anio_Periodo_Actual, (Dias_Disponibles_Periodo_Actual - Dias_Tomara_Periodo_Actual),
                                Periodo_Actual, Dias_Tomara_Periodo_Actual);
                        }
                        else
                        {
                            Dias_Tomara_Periodo_Actual = Dias_Disponibles_Periodo_Actual;
                            Dias_Restan_Por_Tomar = (Dias_Restan_Por_Tomar - Dias_Disponibles_Periodo_Actual);

                            ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional actual.
                            Actualizar_Periodo(Empleado_ID, Anio_Periodo_Actual, (Dias_Disponibles_Periodo_Actual - Dias_Tomara_Periodo_Actual),
                                Periodo_Actual, Dias_Tomara_Periodo_Actual);
                            ///Consultamos los dias disponibles del periodo siguiente.
                            Dias_Disponibles_Periodo_Siguiente = Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(Empleado_ID, Anio_Periodo_Siguiente,
                                Periodo_Siguiente);

                            //Si al tomar los dias disponibles del periodo actual aun restan dias tomados de vacaciones
                            //se realiza el descuento de los dias al periodo siguiente.
                            if (Dias_Disponibles_Periodo_Siguiente > 0)
                            {
                                if (Dias_Disponibles_Periodo_Siguiente >= Dias_Restan_Por_Tomar)
                                {
                                    Dias_Tomados_Periodo_Siguiente = Dias_Restan_Por_Tomar;

                                    ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional siguiente al actual.
                                    Actualizar_Periodo(Empleado_ID, Anio_Periodo_Siguiente, (Dias_Disponibles_Periodo_Siguiente - Dias_Tomados_Periodo_Siguiente),
                                        Periodo_Siguiente, Dias_Tomados_Periodo_Siguiente);
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                ///Consultamos los dias disponibles del periodo actual.
                Dias_Disponibles_Periodo_Actual = Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(Empleado_ID, Anio_Periodo_Actual,
                    Periodo_Actual);

                //Si al tomar los dias disponibles del periodo anterior aun restan dias tomados de vacaciones
                //se realiza el descuento de los dias al periodo actual.
                if (Dias_Disponibles_Periodo_Actual > 0)
                {
                    if (Dias_Disponibles_Periodo_Actual >= Dias_Vacaciones_Tomados_Periodo_Actual)
                    {
                        Dias_Tomara_Periodo_Actual = Dias_Vacaciones_Tomados_Periodo_Actual;
                        ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional actual.
                        Actualizar_Periodo(Empleado_ID, Anio_Periodo_Actual, (Dias_Disponibles_Periodo_Actual - Dias_Tomara_Periodo_Actual),
                            Periodo_Actual, Dias_Tomara_Periodo_Actual);
                    }
                    else
                    {
                        Dias_Tomara_Periodo_Actual = Dias_Disponibles_Periodo_Actual;
                        Dias_Restan_Por_Tomar = (Dias_Vacaciones_Tomados_Periodo_Actual - Dias_Disponibles_Periodo_Actual);

                        ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional actual.
                        Actualizar_Periodo(Empleado_ID, Anio_Periodo_Actual, (Dias_Disponibles_Periodo_Actual - Dias_Tomara_Periodo_Actual),
                            Periodo_Actual, Dias_Tomara_Periodo_Actual);
                        ///Consultamos los dias disponibles del periodo siguiente.
                        Dias_Disponibles_Periodo_Siguiente = Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(Empleado_ID, Anio_Periodo_Siguiente,
                            Periodo_Siguiente);

                        //Si al tomar los dias disponibles del periodo actual aun restan dias tomados de vacaciones
                        //se realiza el descuento de los dias al periodo siguiente.
                        if (Dias_Disponibles_Periodo_Siguiente > 0)
                        {
                            if (Dias_Disponibles_Periodo_Siguiente >= Dias_Restan_Por_Tomar)
                            {
                                Dias_Tomados_Periodo_Siguiente = Dias_Restan_Por_Tomar;

                                ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional siguiente al actual.
                                Actualizar_Periodo(Empleado_ID, Anio_Periodo_Siguiente, (Dias_Disponibles_Periodo_Siguiente - Dias_Tomados_Periodo_Siguiente),
                                    Periodo_Siguiente, Dias_Tomados_Periodo_Siguiente);
                            }
                        }
                    }
                }
                else
                {
                    Dias_Disponibles_Periodo_Siguiente = Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(Empleado_ID, Anio_Periodo_Siguiente,
                        Periodo_Siguiente);

                    //Si al tomar los dias disponibles del periodo actual aun restan dias tomados de vacaciones
                    //se realiza el descuento de los dias al periodo siguiente.
                    if (Dias_Disponibles_Periodo_Siguiente > 0)
                    {
                        if (Dias_Disponibles_Periodo_Siguiente >= Dias_Vacaciones_Tomados_Periodo_Actual)
                        {
                            ///Se realiza la actualizaión de los dias disponibles y dias tomados del periodo vacacional siguiente al actual.
                            Actualizar_Periodo(Empleado_ID, Anio_Periodo_Siguiente, (Dias_Disponibles_Periodo_Siguiente - Dias_Tomados_Periodo_Siguiente),
                                Periodo_Siguiente, Dias_Tomados_Periodo_Siguiente);
                        }
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al dar de alta el periodo vacacional actual. Error: [" + Ex.Message + "]");
        }
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Antiguedad_Empleado_Es_Un_Anio
    ///
    ///DESCRIPCIÓN: Consulta la fecha de ingreso del empleado y en base a esta información se identifica si el empleado cuenta con mas de un 
    ///             año de antiguedad en la empresa. Si Cuenta con un año la función retorna un valor booleano iwual a true de lo contrario
    ///             retorna un valor false.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private Boolean Antiguedad_Empleado_Es_Un_Anio(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Informacion_Empelado = null;                                   //Variable que almacenara los datos del empleado.
        DateTime? Fecha_Ingreso_Empleado = null;                                    //Variable que alamaceba la fecha de ingreso del empleado a presidencia.
        DateTime Fecha_Actual = DateTime.Now;                                       //Variable que almacena la fecha actual.
        Boolean Estatus = false;

        try
        {
            Obj_Empleados.P_Empleado_ID = Empleado_ID;
            Dt_Informacion_Empelado = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Informacion_Empelado is DataTable)
            {
                if (Dt_Informacion_Empelado.Rows.Count > 0)
                {
                    foreach (DataRow Empleado in Dt_Informacion_Empelado.Rows)
                    {
                        if (Empleado is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString()))
                            {
                                Fecha_Ingreso_Empleado = Convert.ToDateTime(Empleado[Cat_Empleados.Campo_Fecha_Inicio].ToString());
                                //Validamos si la antiguedad del empelado es de 1 año.
                                if (Cls_DateAndTime.DateDiff(DateInterval.Year, ((DateTime)Fecha_Ingreso_Empleado), Fecha_Actual) >= 1)
                                {
                                    Estatus = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar si la antiguedad del empelado en presidencia es de unn año. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo
    ///
    ///DESCRIPCIÓN: Obtiene los dias disponibles de vacaciones de acuerdo al periodo vacacional consultado.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             Anio .- Año del periodo nominal que esta vigente actualmente.
    ///             Periodo.- Periodo vacacional que se desea consultar.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private Int32 Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(String Empleado_ID, Int32 Anio, Int32 Periodo)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados =
                            new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Vacaciones_Empl_Det = null;                          //Variable que almacena la información del periodo vacacional actual.
        Int32 Dias_Disponibles = 0;                                       //Variable que almacenara los dias disponibles del periodo vacacional actual.
        Int32 Dias_Tomados = 0;                                           //Variable que almacenara los dias tomados en el periodo vacacional actual.

        try
        {
            //Establecemos las variables que son requeridas para completar la operacion de búsqueda del periodo vacacional actual. 
            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Vacaciones_Empleados.P_Anio = Anio;
            Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo;
            //Ejecutamos la busqueda de periodos vacacionales.
            Dt_Vacaciones_Empl_Det = Obj_Vacaciones_Empleados.Consultar_Vacaciones_Empl_Det();

            if (Dt_Vacaciones_Empl_Det is DataTable)
            {
                if (Dt_Vacaciones_Empl_Det.Rows.Count > 0)
                {
                    foreach (DataRow Vacacion_Empl_Detalle in Dt_Vacaciones_Empl_Det.Rows)
                    {
                        if (Vacacion_Empl_Detalle is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Vacacion_Empl_Detalle[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles].ToString()))
                            {
                                Dias_Disponibles = Convert.ToInt32(Vacacion_Empl_Detalle[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles].ToString());
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener la cantidad de dias disponibles del empleado de algún periodo en particular. Error: [" + Ex.Message + "]");
        }
        return Dias_Disponibles;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Actualizar_Periodo
    ///
    ///DESCRIPCIÓN: Actualiza los dias disponibles y los dias tomados del periodo vacacional actual. 
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             Anio .- Año del periodo nominal que esta vigente actualmente.
    ///             Periodo.- Periodo vacacional que se desea consultar.
    ///             Dias_Disponibles.- Dias de vacaciones disponibles del periodo consultado.
    ///             Dias_Tomados_Periodo_Actual.- Almacena los dias que se tomaron del periodo vacacional del que se tomaran las vacaciones.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 04/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private void Actualizar_Periodo(String Empleado_ID, Int32 Anio, Int32 Dias_Disponibles, Int32 Periodo, Int32 Dias_Tomados_Periodo_Actual)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Vacaciones_Empl_Det = null;//Variable que almacenara la lista de periodos vacacionales que tiene resgitrados el empleado.        
        Int32 Dias_Tomados = 0;                 //Variable que almacenara la cantidad de dias que a tomados por el empleado en el periodo vacacional.

        try
        {
            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
            Obj_Vacaciones_Empleados.P_Anio = Anio;
            Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo;
            Dt_Vacaciones_Empl_Det = Obj_Vacaciones_Empleados.Consultar_Vacaciones_Empl_Det();

            if (Dt_Vacaciones_Empl_Det is DataTable)
            {
                if (Dt_Vacaciones_Empl_Det.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Dt_Vacaciones_Empl_Det.Rows[0][Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados].ToString()))
                    {
                        Dias_Tomados = Convert.ToInt32(Dt_Vacaciones_Empl_Det.Rows[0][Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados].ToString());
                        Dias_Tomados += Dias_Tomados_Periodo_Actual;

                        Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                        Obj_Vacaciones_Empleados.P_Anio = Anio;
                        Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Disponibles;
                        Obj_Vacaciones_Empleados.P_Dias_Tomados = Dias_Tomados;
                        Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo;
                        Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                        Obj_Vacaciones_Empleados.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                        Obj_Vacaciones_Empleados.Modificar_Detalle_Vacaciones_Empleados();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al actualizar los dias disponibles del periodo vacacional. Error: [" + Ex.Message + "]");
        }
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Existe_Registrado_Siguiente_Periodo_Vacacional
    ///
    ///DESCRIPCIÓN: Verifica si el periodo vacacional siguiente al actual se encuentra en sistema. Si aun no se encuentra registrado
    ///             lo da de alta.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los que se encuentran dadas dados de alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 15/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private Boolean Existe_Registrado_Siguiente_Periodo_Vacacional(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Vacaciones_Empl_Det = null;            //Variable que almacena la informacion 
        Int32 Anio_Periodo_Anterior = 0;                    //Variable que almacena el año del periodo vacacional anterior al actual.
        Int32 Anio_Periodo_Actual = 0;                      //Variable que almacena el año del periodo vacacional actual.
        Int32 Anio_Periodo_Siguiente = 0;                   //Variable que almacena el año del periodo vacacional siguiente al actual.
        Int32 Periodo_Anterior = 0;                         //Variable que almacena el periodo vacacional anterior al actual.
        Int32 Periodo_Actual = 0;                           //Variable que almacena el periodo vacacional actual.
        Int32 Periodo_Siguiente = 0;                        //Variable que almacena el periodo vacacional siguiente ala actual.
        Boolean Existe_Siguiente_Periodo_Vacacional = false;//Variable que almacena si el siguiente periodo vacacional ya existe o debe crearse.

        try
        {
            //Paso 2.- Obtener el Periodo Vacacional actual [AÑO - PERIODO].
            Anio_Periodo_Actual = Obtener_Anio_Calendario_Nomina();
            Periodo_Actual = Obtener_Periodo_Vacacional();


            //Paso III.- Identificar el periodo vacacional en el que nos encontramos actualmente. Para en base a esa información
            //           Obtener el Año [Anterior - Actual - Siguiente] y el Periodo [Anterior - Actual - Siguiente].
            if (Periodo_Actual == 1)
            {
                //Si el periodo actual es el primero entoncés:
                Anio_Periodo_Anterior = Anio_Periodo_Actual - 1;//El año del periodo anterior es el año actual menos 1.
                Anio_Periodo_Siguiente = Anio_Periodo_Actual;//El año del periodo siguiente es el mismo.

                Periodo_Anterior = 2;
                Periodo_Siguiente = 2;
            }
            else if (Periodo_Actual == 2)
            {
                //Si el periodo actual es el segundo entoncés:
                Anio_Periodo_Anterior = Anio_Periodo_Actual;//El año del periodo anterior es el mismo.
                Anio_Periodo_Siguiente = Anio_Periodo_Siguiente + 1;//El año del periodo siguiente es Año actual mas 1.

                Periodo_Anterior = 1;
                Periodo_Siguiente = 1;
            }

            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
            Obj_Vacaciones_Empleados.P_Anio = Anio_Periodo_Siguiente;
            Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Siguiente;
            //Consultamos si el siguiente periodo vacacional ya ha sido creado.
            Dt_Vacaciones_Empl_Det = Obj_Vacaciones_Empleados.Consultar_Vacaciones_Empl_Det();

            if (Dt_Vacaciones_Empl_Det is DataTable)
            {
                if (Dt_Vacaciones_Empl_Det.Rows.Count > 0)
                {
                    //Esta validación es para identificar si ya existe registrado en sistema el siguiente periodo vacacional.
                    Existe_Siguiente_Periodo_Vacacional = true;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar y validar si ya existe el siguiente periodo vacacional. Error: [" + Ex.Message + "]");
        }
        return Existe_Siguiente_Periodo_Vacacional;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Vacaciones_Tipo_Nomina
    ///
    ///DESCRIPCIÓN: Consulta los dias de vacaciones que le corresponde al empleado por el tipo de nómina al que pertence. Y de acuerdo al 
    ///             periodo vacacional consultado.
    ///             
    /// PARÁMETROS: Tipo_Nomina_ID.- Identificador o clave única para identificar a los tipos de nomina que se encuentran dadas de 
    ///                              alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 15/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private DataTable Consultar_Dias_Vacaciones_Tipo_Nomina(String Tipo_Nomina_ID)
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nomina = null;                                                    //Variable que guarda la información del tipo de nomina.
        DataTable Dt_Tipo_Nomina_Dias_Vacaciones = new DataTable("DIAS_VACACIONES");         //Variable que almacenara los dias de vacaciones de los periodos vacacionales.
        DataRow Registro_Dias_Vacaciones = null;                                             //Variable que almacena un registro de las vacaciones por periodo vacacional.
        Int32 Dias_Vacaciones_PVI = 0;                                                       //Variable que almacena las dias de vacaciones del periodo primer periodo vacacional.
        Int32 Dias_Vacaciones_PVII = 0;                                                      //Variable que almacena las dias de vacaciones del periodo segundo periodo vacacional.

        try
        {
            Dt_Tipo_Nomina_Dias_Vacaciones.Columns.Add("PVI", typeof(Int32));
            Dt_Tipo_Nomina_Dias_Vacaciones.Columns.Add("PVII", typeof(Int32));

            Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Datos_Tipo_Nomina();

            if (Dt_Tipos_Nomina is DataTable)
            {
                if (Dt_Tipos_Nomina.Rows.Count > 0)
                {
                    foreach (DataRow Tipo_Nomina in Dt_Tipos_Nomina.Rows)
                    {
                        if (Tipo_Nomina is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString()))
                            {
                                Dias_Vacaciones_PVI = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString());
                                if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString()))
                                {
                                    Dias_Vacaciones_PVII = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString());

                                    Registro_Dias_Vacaciones = Dt_Tipo_Nomina_Dias_Vacaciones.NewRow();
                                    Registro_Dias_Vacaciones["PVI"] = Dias_Vacaciones_PVI;
                                    Registro_Dias_Vacaciones["PVII"] = Dias_Vacaciones_PVII;
                                    Dt_Tipo_Nomina_Dias_Vacaciones.Rows.Add(Registro_Dias_Vacaciones);
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los dias de vacaciones del empleado de acuerdo a su tipo de nómina. Error: [" + Ex.Message + "]");
        }
        return Dt_Tipo_Nomina_Dias_Vacaciones;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Dias_Vacaciones_Tipo_Nomina
    ///
    ///DESCRIPCIÓN: Consulta los dias de vacaciones que le corresponde al empleado por el tipo de nómina al que pertence. Y de acuerdo al 
    ///             periodo vacacional consultado.
    ///             
    /// PARÁMETROS: Tipo_Nomina_ID.- Identificador o clave única para identificar a los tipos de nomina que se encuentran dadas de 
    ///                              alta en el sistema.
    ///             Periodo.- Periodo Vacacional a consultar los dias de vacaciones del empleado.                  
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 15/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private Int32 Consultar_Dias_Vacaciones_Tipo_Nomina(String Tipo_Nomina_ID, Int32 Periodo)
    {
        Cls_Cat_Tipos_Nominas_Negocio Obj_Tipos_Nomina = new Cls_Cat_Tipos_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Tipos_Nomina = null;                                                    //Variable que guarda la información del tipo de nomina.
        Int32 Dias_Vacaciones_PVI = 0;                                                       //Variable que almacena las dias de vacaciones del primer periodo vacacional.
        Int32 Dias_Vacaciones_PVII = 0;                                                      //Variable que almacena las dias de vacaciones del segundo periodo vacacional.
        Int32 Dias = 0;

        try
        {
            Obj_Tipos_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Dt_Tipos_Nomina = Obj_Tipos_Nomina.Consulta_Datos_Tipo_Nomina();

            if (Dt_Tipos_Nomina is DataTable)
            {
                if (Dt_Tipos_Nomina.Rows.Count > 0)
                {
                    foreach (DataRow Tipo_Nomina in Dt_Tipos_Nomina.Rows)
                    {
                        if (Tipo_Nomina is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString()))
                            {
                                Dias_Vacaciones_PVI = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_1].ToString());
                                if (!String.IsNullOrEmpty(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString()))
                                {
                                    Dias_Vacaciones_PVII = Convert.ToInt32(Tipo_Nomina[Cat_Nom_Tipos_Nominas.Campo_Dias_Prima_Vacacional_2].ToString());

                                    if (Periodo == 1) Dias = Dias_Vacaciones_PVI;
                                    else if (Periodo == 2) Dias = Dias_Vacaciones_PVII;
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los dias de vacaciones del empleado de acuerdo a su tipo de nómina. Error: [" + Ex.Message + "]");
        }
        return Dias;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Tipo_Nomina_Empleado
    ///
    ///DESCRIPCIÓN: Consulta y obtiene el tipo de nomina al que pertence el empleado.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los empleados que se encuentran dadas de 
    ///                           alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 15/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private String Obtener_Tipo_Nomina_Empleado(String Empleado_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Informacion_Empleado = null;                                   //Variable que almacenara los datos del empleado consultado.
        String Tipo_Nomina_ID = "";                                                 //Variable que almacena el tipo de nomina del empleado.

        try
        {
            Obj_Empleados.P_Empleado_ID = Empleado_ID;
            Dt_Informacion_Empleado = Obj_Empleados.Consulta_Empleados_General();

            if (Dt_Informacion_Empleado is DataTable)
            {
                if (Dt_Informacion_Empleado.Rows.Count > 0)
                {
                    foreach (DataRow Empleado in Dt_Informacion_Empleado.Rows)
                    {
                        if (Empleado is DataRow)
                        {
                            if (!String.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString()))
                                Tipo_Nomina_ID = Empleado[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el tipo de nómina del empleado. Error: [" + Ex.Message + "]");
        }
        return Tipo_Nomina_ID;
    }
    ///***************************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Actualizar_Dias_PV_Antigueadad_Menor_Anio
    ///
    ///DESCRIPCIÓN: Actualiza los dias de vacaciones disponibles y tomados del periodo vacacional del empleado.
    ///             
    /// PARÁMETROS: Empleado_ID.- Identificador o clave única para identificar a los empleados que se encuentran dadas de 
    ///                           alta en el sistema.
    ///             
    ///CREO: Juan Alberto Hernández Negrete 
    ///FECHA_CREO: 15/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***************************************************************************************************************************************
    private void Actualizar_Dias_PV_Antigueadad_Menor_Anio(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Vacaciones_Empl_Det = null;                        //Variable que almacenara la lista de periodos vacacionales que tiene resgitrados el empleado.     
        Int32 Dias_Disponibles_Vacaciones_Periodo_Vacacion_Actual = 0;  //Variable que almacenara los dias disponibles del periodo vacacional actual.
        Int32 Dias_Tomados_Vacaciones_Periodo_Vacacion_Actual = 0;      //Variable que almacenara los dias que ya se han tomado del periodo vacacional actual.
        Int32 Anio = 0;                                                 //Variable que almacenara el año del calendario de nomina que se encuentra vigente actualmente.                
        Int32 Periodo_Actual = 0;                                       //Variable que almacenara el periodo vacacional actual.
        Int32 Dias_Totales_Periodo_Vacacional_Actual = 0;               //Variable que almacena los dias totales del periodo vacacional actual.
        String Tipo_Nomina_ID = "";                                       //Variable que almacena el tipo de nomina al que pertence el empleado.
        Int32 Dias_Vacaciones_Tipo_Nomina = 0;                          //Variable que almacena los dias que le corresponde al empleado por periodo vacacional y por tipo de nómina.

        try
        {
            Anio = Obtener_Anio_Calendario_Nomina();//Obtenemos el año del calendario de nomina que se encuentra vigente actualmente.
            Periodo_Actual = Obtener_Periodo_Vacacional();//Variable que almacena elm periodo vacacional actual.
            //Consultamos los dias disponibles de vaccaiones del periodo actual.
            Dias_Disponibles_Vacaciones_Periodo_Vacacion_Actual = Obtener_Cantidad_Dias_Disponibles_Vacaciones_Periodo(Empleado_ID,
                Anio, Periodo_Actual);

            //Consultamos los dias de vacaciones que le corresponden al empleado de acuerdo al tipo de nomina y
            //periodo al que pertence.
            Tipo_Nomina_ID = Obtener_Tipo_Nomina_Empleado(Empleado_ID);
            Dias_Vacaciones_Tipo_Nomina = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, Periodo_Actual);

            //Consultamos si el empleado cuenta con un registro de periodo vacacional.
            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
            Obj_Vacaciones_Empleados.P_Anio = Anio;
            Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Actual;
            //Ejecutamos la consulta de periodo vacacional actual.
            Dt_Vacaciones_Empl_Det = Obj_Vacaciones_Empleados.Consultar_Vacaciones_Empl_Det();

            if (Dt_Vacaciones_Empl_Det is DataTable)
            {
                if (Dt_Vacaciones_Empl_Det.Rows.Count > 0)
                {
                    if (!string.IsNullOrEmpty(Dt_Vacaciones_Empl_Det.Rows[0][Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados].ToString()))
                    {
                        //Obtenemos los dias de vacaciones tomados por el empleado en el periodo actual.
                        Dias_Tomados_Vacaciones_Periodo_Vacacion_Actual = Convert.ToInt32(Dt_Vacaciones_Empl_Det.Rows[0][Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados].ToString());
                        //Se realiza una suma de los dias disponibles de vacaciones y los dias tomados, si la suma de los dias es igual a los que le corresponden
                        //al empleado de acuerdo al periodo actual y asu tipo de nomina entonces ya no se actualizara los dias disponibles del empleado ya que ya
                        //se le han asignado los dias que le correspondian de acuerdo al periodo y tipo de nomina al que peertence.
                        Dias_Totales_Periodo_Vacacional_Actual = Dias_Disponibles_Vacaciones_Periodo_Vacacion_Actual + Dias_Tomados_Vacaciones_Periodo_Vacacion_Actual;

                        //Si los dias totales del periodo vacacional actual son iguales a los que le corresponden por tipo de nómina entoncés:
                        if (Dias_Totales_Periodo_Vacacional_Actual < Dias_Vacaciones_Tipo_Nomina)
                        {
                            //Actualizamos la información del periodo vacacional actual.
                            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
                            Obj_Vacaciones_Empleados.P_Anio = Anio;
                            Obj_Vacaciones_Empleados.P_Dias_Disponibles = Dias_Vacaciones_Base_Formula(Empleado_ID) - Dias_Tomados_Vacaciones_Periodo_Vacacion_Actual;
                            Obj_Vacaciones_Empleados.P_Dias_Tomados = Dias_Tomados_Vacaciones_Periodo_Vacacion_Actual;
                            Obj_Vacaciones_Empleados.P_Periodo_Vacacional = Periodo_Actual;
                            Obj_Vacaciones_Empleados.P_Estatus_Detalle = "ACTIVO";
                            Obj_Vacaciones_Empleados.P_Usuario_Modifico = Cls_Sessiones.Nombre_Empleado;
                            Obj_Vacaciones_Empleados.Modificar_Detalle_Vacaciones_Empleados();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al actualizar los dias de vacaciones del empleado cuando el mismo" +
                " tiene una antiguedad menor a 1 año. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Botones)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Nomina_Click
    ///DESCRIPCIÓN: Ejecuta la Generación de la Nómina. 
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Generar_Nomina_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Generar_Nomina Generar_Nomina = new Cls_Ope_Nom_Generar_Nomina();       //Variable de conexión con la capa de negocios.
        Cls_Cat_Empleados_Negocios Empleados_Negocios = new Cls_Cat_Empleados_Negocios();   //Variable de conexión con la capa de negocios.
        DataTable Dt_Lista_Empelados = null;                                                //Variable que almacena una lista de empleados.
        String Empleado_ID = "";                                                            //Almacena el identificador del empleado, su uso es interno del sistema.
        String No_Empleado = "";                                                            //Almacena el identificador del empleado, su uso es controlado por recursos humanos.
        DateTime Fecha;                                                                     //Fecha para validar la generación de la nómina.
        String Nomina_Generar = "";                                                         //Almacena el tipo de nómina a generar Catorcenal, Prima Vacacional 1ra. Parte ó PV 2da. Parte y Aguinaldo Integrado.
        String Nomina_ID = "";                                                              //Variable que almacenará el Identificador de la nómina.
        Int32 No_Nomina = 0;                                                                //Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        String Detalle_Nomina_ID = "";                                                      //Variable que identifica el perido seleccionado para generar la nómina.
        String Tipo_Nomina_ID = "";                                                         //Variable que almacena el tipo de nómina de la cual se desea generar la nómina.
        StringBuilder Historial_Nomina_Generada = new StringBuilder();                      //Variable que almacenará todos los cambios realizados al generar la nómina, para poder hacer un rollback si asi es necesario.
        DataSet Ds_Tablas_Afectadas_Generacion_Nomina = null;                               //Variable que almacena las tabla que fueron afectadas en algunos registros al generar la nómina.
        String Ruta_Guardar_Archivo = "";                                                   //Variable que almacenará la ruta completa donde se guardara el log de la generacion de la nómina.

        try
        {
            //Obtenemos la ruta donde se guardara el log de la nómina.
            Ruta_Guardar_Archivo = Server.MapPath("Log_Generacion_Nomina");
            //Verificamos si el directorio del log de la nómina existe, en caso contrario se crea. 
            if (!Directory.Exists(Ruta_Guardar_Archivo))
                Directory.CreateDirectory(Ruta_Guardar_Archivo);

            //Establecemos la variable de session que alamacenará la información historica de los registros
            //que tuvieron alguna afectación, al realizar el barrido de la nómina.
            Cls_Sessiones.Historial_Nomina_Generada = Historial_Nomina_Generada;

            //Obtenemos los valores que son necesarios para realizar la generación de la nómina.
            Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Detalle_Nomina_ID = Cmb_Periodo.SelectedValue.Trim();
            No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
            Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

            if (Validar_Datos_Generacion_Nomina())
            {
                if (Btn_Generar_Nomina.Text.Equals("Regenerar Nómina"))
                {
                    //Obtenemos la información de las tablas que fueron afectadas al generar la nómina [PRESTAMOS, AJUSTES DE ISR, RECIBOS DE LA NÓMINA Y TOTALES DE LA NÓMINA].
                    Ds_Tablas_Afectadas_Generacion_Nomina = Cls_Historial_Nomina_Generada.Leer_Archivo_Obtener_Historial_Nomina_Generada(@Ruta_Guardar_Archivo, ("/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Tipo_Nomina_ID), ".txt");
                    //Se ejecuta la actualziación de las tablas afectadas, haciendo un RollBack de los datos afectados.
                    Generar_Nomina.RollBack_Registros_Afectadoos_Generacion_Nomina(Ds_Tablas_Afectadas_Generacion_Nomina);
                    //Validamos que exista el archivo que guarda las tablas que fueron afectadas durante la generación de la nómina.
                    if (File.Exists(@Ruta_Guardar_Archivo + "/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Tipo_Nomina_ID + ".txt"))
                    {
                        //Eliminamos el archivo que guardael Historial de la nomina una vez que ya se ha regenerado la nómina.
                        File.Delete(@Ruta_Guardar_Archivo + "/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Tipo_Nomina_ID + ".txt");
                    }
                }
                else if (Btn_Generar_Nomina.Text.Equals("Generar Nómina"))
                {
                    //Validamos que exista el archivo que guarda las tablas que fueron afectadas durante la generación de la nómina.
                    if (File.Exists(@Ruta_Guardar_Archivo + "/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Tipo_Nomina_ID + ".txt"))
                    {
                        //Eliminamos el archivo que guardael Historial de la nomina una vez que ya se ha regenerado la nómina.
                        File.Delete(@Ruta_Guardar_Archivo + "/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Tipo_Nomina_ID + ".txt");
                    }
                }

                Generar_Nomina.P_Nomina_ID = Nomina_ID;
                Generar_Nomina.P_No_Nomina = No_Nomina;
                Generar_Nomina.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                Generar_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;

                //Genenramos el Esquema de la Tabla de Totales de la Nómina a Generar.
                Generar_Nomina.Plantilla_Total_Percepciones_Deducciones();

                //Obtenemos el tipo de nómina a generar. [Catorcenal, Prima Vacacional I y Prima Vacacional II]
                Nomina_Generar = RBL_Tipos_Nominas.SelectedValue.Trim();

                //Consultamos a los empleados que pertencen al tipo de nómina a generar.
                Empleados_Negocios.P_Estatus = "ACTIVO";
                Empleados_Negocios.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();
                Dt_Lista_Empelados = Empleados_Negocios.Consulta_Empleados_General();
                Fecha = Convert.ToDateTime(Txt_Fin_Catorcena.Text.Trim());

                if (Dt_Lista_Empelados != null)
                {
                    foreach (DataRow Empleado in Dt_Lista_Empelados.Rows)
                    {
                        if (!string.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString()))
                            Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                        if (!string.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_No_Empleado].ToString()))
                            No_Empleado = Empleado[Cat_Empleados.Campo_No_Empleado].ToString();

                        //Validamos que el Empleado_ID y el Número de Empleado no esten vacios.
                        if (!string.IsNullOrEmpty(Empleado_ID) && !string.IsNullOrEmpty(No_Empleado))
                        {
                            switch (Nomina_Generar)
                            {
                                case "CATORCENAL":
                                    //if (No_Empleado.Equals("003967"))
                                    Generar_Nomina.Generar_Nomina_Catorcenal(Empleado_ID, No_Empleado, Fecha, Nomina_Generar);
                                    break;
                                case "PVI":
                                    //if (No_Empleado.Equals("003613"))
                                    Generar_Nomina.Generar_Nomina_Prima_Vacacional(Empleado_ID, No_Empleado, Fecha, Nomina_Generar);
                                    break;
                                case "PVII_AGUINALDO":
                                    //if (No_Empleado.Equals("003613"))
                                    Generar_Nomina.Generar_Nomina_Aguinaldo_Prima_Vacacional(Empleado_ID, No_Empleado, Fecha, Nomina_Generar);
                                    break;
                                default:
                                    break;
                            }
                        }
                    }
                }

                if (Btn_Generar_Nomina.Text.Equals("Generar Nómina"))
                {
                    //Guardamos el registro de la nómina generada.
                    Generar_Nomina.Guardar_Registro_Nomina_Generada(Nomina_Generar);
                }
                //Guardar los Totales de la Nómina Generada.
                Generar_Nomina.Guardar_Totales_Nomina();
                //Aqui se guarda la información historica de la nómina generada. 
                //Registrando cada movimiento que se realizo al generar la nómina en Prestamos y Ajustes de ISR. 
                Cls_Historial_Nomina_Generada.Escribir_Archivo_Historial_Nomina_Generada(Ruta_Guardar_Archivo, ("/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Tipo_Nomina_ID), ".txt", Cls_Sessiones.Historial_Nomina_Generada);
                //Eliminamos la Session que almacena, los registros almacenados de Prestamos, Ajustes de ISR, Recibos de la Nómina y Totales de la Nomina.
                Cls_Sessiones.Historial_Nomina_Generada = null;
                //Limpiar los controles de la pagina.
                //Limpiar_Controles();
                //Avisamos al usuario que la nómina termino de generarse.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Información Generación Nómina", "alert('Generación Nómina Completa');", true);
                ScriptManager.RegisterStartupScript(UPnl_Generar_Nomina, typeof(string), "Imagen", "javascript:inicializarEventos_Generacion_Nomina();", true);
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                ScriptManager.RegisterStartupScript(UPnl_Generar_Nomina, typeof(string), "Imagen", "javascript:inicializarEventos_Generacion_Nomina();", true);
            }
            Consultar_Nominas_Negativas();
            Limpiar_Controles();
            Configuracion_Inicial();
        }
        catch (Exception Ex)
        {
            //Aqui se guarda la información historica de la nómina generada. 
            //Registrando cada movimiento que se realizo al generar la nómina en Prestamos y Ajustes de ISR. 
            Cls_Historial_Nomina_Generada.Escribir_Archivo_Historial_Nomina_Generada(Ruta_Guardar_Archivo, ("/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Tipo_Nomina_ID), ".txt", Cls_Sessiones.Historial_Nomina_Generada);
            //Eliminamos la Session que almacena, los registros almacenados de Prestamos, Ajustes de ISR, Recibos de la Nómina y Totales de la Nomina.
            Cls_Sessiones.Historial_Nomina_Generada = null;

            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Cerrar_Nomina_Click
    ///
    ///DESCRIPCIÓN: Se encarga de cerrar la nómina generada despues de cerrar esta operacion 
    ///             ya no será posible realizar ninguna modificacion a al nomina.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 25/Fecbrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Cerrar_Nomina_Click(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        String Ruta_Guardar_Archivo = "";//Variable que almacenará la ruta completa donde se guardara el log de la generacion de la nómina.
        String Tipo_Nomina_ID = "";      //Variable que almacena el tipo de nómina del empleado.
        DataTable Dt_Nomina_Detalle = null;
        String Detalle_Nomina_ID = String.Empty;

        try
        {
            if (Validar_Nomina_Presupuestalmente())
            {
                if (Validar_Datos_Cerrar_Nomina())
                {
                    //Obtenemos la ruta donde se guardara el log de la nómina.
                    Ruta_Guardar_Archivo = Server.MapPath("Log_Generacion_Nomina");
                    //Verificamos si el directorio del log de la nómina existe, en caso contrario se crea. 
                    if (Directory.Exists(Ruta_Guardar_Archivo))
                    {
                        if (File.Exists(@Ruta_Guardar_Archivo + ("/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Cmb_Tipo_Nomina.SelectedValue.Trim()) + ".txt"))
                        {
                            //Eliminamos el archivo que guardael Historial de la nomina una vez que ya se ha regenerado la nómina.
                            File.Delete(@Ruta_Guardar_Archivo + ("/Log_Nomina_" + RBL_Tipos_Nominas.SelectedValue.Trim() + "_" + Cmb_Tipo_Nomina.SelectedValue.Trim()));
                        }
                    }

                    //Aqui se actualizara los registros de periodos vacacionales de la nómina.
                    Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();
                    Ejecutar_Actualizacion_Dias_Periodos_Vacacionales(Tipo_Nomina_ID);

                    ScriptManager.RegisterStartupScript(UPnl_Generar_Nomina, typeof(string), "Imagen", "javascript:inicializarEventos_Generacion_Nomina();", true);

                    Obj_Calendario_Nomina.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                    Obj_Calendario_Nomina.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                    Dt_Nomina_Detalle = Obj_Calendario_Nomina.Consulta_Periodos_Nomina();

                    if (Dt_Nomina_Detalle is DataTable)
                    {
                        if (Dt_Nomina_Detalle.Rows.Count > 0)
                        {
                            foreach (DataRow Detalle in Dt_Nomina_Detalle.Rows)
                            {
                                if (!string.IsNullOrEmpty(Detalle[Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID].ToString()))
                                {
                                    Detalle_Nomina_ID = Detalle[Cat_Nom_Nominas_Detalles.Campo_Detalle_Nomina_ID].ToString();

                                    Obj_Calendario_Nomina.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();
                                    Obj_Calendario_Nomina.P_Detalle_Nomina_ID = Detalle_Nomina_ID;
                                    if (Obj_Calendario_Nomina.Alta_Cierre_Periodo_Nomina())
                                    {
                                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Cerrar Nomina", "alert('Nomina Cerrada');", true);
                                        Limpiar_Controles();
                                        Btn_Generar_Nomina.Visible = false;
                                        Btn_Cerrar_Nomina.Visible = false;
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Periodos_Catorcenales_Nomina
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 Nomina_Seleccionada = 0;//Variable que almacena la nómina seleccionada del combo.

        try
        {
            //Obtenemos elemento seleccionado del combo.
            Nomina_Seleccionada = Cmb_Calendario_Nomina.SelectedIndex;

            if (Nomina_Seleccionada > 0)
            {
                Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());
                ScriptManager.RegisterStartupScript(UPnl_Generar_Nomina, typeof(string), "Imagen", "javascript:inicializarEventos_Generacion_Nomina();", true);
            }
            else
            {
                Cmb_Periodo.DataSource = new DataTable();
                Cmb_Periodo.DataBind();
                Configuracion_Inicial();
                Limpiar_Controles();
                ScriptManager.RegisterStartupScript(UPnl_Generar_Nomina, typeof(string), "Imagen", "javascript:inicializarEventos_Generacion_Nomina();", true);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Periodo_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga la fecha de inicio y fin del periodo catorcenal seleccionado.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 19/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Periodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Generar_Nomina Generar_Nomina = new Cls_Ope_Nom_Generar_Nomina();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalles_Nomina = null;//Variable que almacenra los detalles del periodo seleccionado.
        DateTime Fecha_Inicio = new DateTime();//Fecha de inicio de la catorcena a generar la nómina.
        DateTime Fecha_Fin = new DateTime();//Fecha de fin de la catorcena a generar la nómina.

        String Nomina_ID = "";                                                              //Variable que almacenará el Identificador de la nómina.
        Int32 No_Nomina = 0;                                                                //Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        String Detalle_Nomina_ID = "";                                                      //Variable que identifica el perido seleccionado para generar la nómina.
        String Tipo_Nomina_ID = "";                                                         //Variable que almacena el tipo de nómina de la cual se desea generar la nómina.

        try
        {
            //Obtenemos los valores que son necesarios para realizar la generación de la nómina.
            Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Detalle_Nomina_ID = Cmb_Periodo.SelectedValue.Trim();
            No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
            Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

            Generar_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
            Generar_Nomina.P_Nomina_ID = Nomina_ID;
            Generar_Nomina.P_No_Nomina = No_Nomina;

            if (Cmb_Periodo.SelectedIndex > 0)
            {
                Prestamos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();

                Prestamos.P_No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                Dt_Detalles_Nomina = Prestamos.Consultar_Fechas_Periodo();

                if (Dt_Detalles_Nomina != null)
                {
                    if (Dt_Detalles_Nomina.Rows.Count > 0)
                    {
                        Fecha_Inicio = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString());
                        Fecha_Fin = Convert.ToDateTime(Dt_Detalles_Nomina.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString());

                        Txt_Inicia_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Inicio);
                        Txt_Fin_Catorcena.Text = string.Format("{0:dd/MMM/yyyy}", Fecha_Fin);
                    }
                }

                //Validamos si la nomina ya fue generada previamente.
                if (Generar_Nomina.Existe_Nomina_Ya_Generada(RBL_Tipos_Nominas.SelectedValue.Trim()))
                {
                    if (Validar_Si_Nomina_Fue_Cerrada())
                    {
                        Btn_Generar_Nomina.Visible = false;
                        Btn_Cerrar_Nomina.Visible = false;
                    }
                    else
                    {
                        Btn_Cerrar_Nomina.Visible = true;
                        Btn_Generar_Nomina.Text = "Regenerar Nómina";
                        Btn_Generar_Nomina.Visible = true;
                    }
                }
                else
                {
                    Btn_Generar_Nomina.Visible = true;
                    Btn_Generar_Nomina.Text = "Generar Nómina";
                    Btn_Cerrar_Nomina.Visible = false;
                }

            }
            else
            {
                Txt_Inicia_Catorcena.Text = "";
                Txt_Fin_Catorcena.Text = "";
                Configuracion_Inicial();
                Limpiar_Controles();
            }

            ScriptManager.RegisterStartupScript(UPnl_Generar_Nomina, typeof(string), "Imagen", "javascript:inicializarEventos_Generacion_Nomina();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    #endregion

    #region (RadionButton)
    /// **********************************************************************************************************
    /// Nombre: RBL_Tipos_Nominas_SelectedIndexChanged
    /// 
    /// Descripción: Evento que se ejecuta al seleccionar el tipo de nómina a generar si es una nómina catorcenal,
    ///              nómina de prima vacacional o nómina de prima vacacional aguinaldo.
    /// 
    /// Parámtros: No Áplica.
    /// 
    /// Usuario Creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 15/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// **********************************************************************************************************
    protected void RBL_Tipos_Nominas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nomina = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Pestamos_Negocio Prestamos = new Cls_Ope_Nom_Pestamos_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Ope_Nom_Generar_Nomina Generar_Nomina = new Cls_Ope_Nom_Generar_Nomina();//Variable de conexion con la capa de negocios.
        String Nomina_ID = "";                                                              //Variable que almacenará el Identificador de la nómina.
        Int32 No_Nomina = 0;                                                                //Variable que almacena el numero de catorcena de la cual se desea generar la nómina.
        String Detalle_Nomina_ID = "";                                                      //Variable que identifica el perido seleccionado para generar la nómina.
        String Tipo_Nomina_ID = "";                                                         //Variable que almacena el tipo de nómina de la cual se desea generar la nómina.

        try
        {
            if (RBL_Tipos_Nominas.SelectedIndex == 1)
            {
                if (!Validar_Nomina_Prima_Vacacional())
                {
                    RBL_Tipos_Nominas.SelectedIndex = -1;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No es posible generar una nómina de prima vacacional en una fecha diferente a la establecida.');", true);
                }
            }

            if (RBL_Tipos_Nominas.SelectedIndex == 2)
            {
                if (!Validar_Nomina_Prima_Vacacional_II_Aguinaldo())
                {
                    RBL_Tipos_Nominas.SelectedIndex = -1;
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('No es posible generar una nómina de prima vacacional II y Aguinaldo en una fecha diferente a la establecida.');", true);
                }
            }

            if (Validar_Datos_Cerrar_Nomina())
            {
                //Obtenemos los valores que son necesarios para realizar la generación de la nómina.
                if (Cmb_Calendario_Nomina.SelectedIndex > 0) Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                if (Cmb_Periodo.SelectedIndex > 0)
                {
                    Detalle_Nomina_ID = Cmb_Periodo.SelectedValue.Trim();
                    No_Nomina = Convert.ToInt32(Cmb_Periodo.SelectedItem.Text.Trim());
                }
                if (Cmb_Tipo_Nomina.SelectedIndex > 0) Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue.Trim();

                Generar_Nomina.P_Tipo_Nomina_ID = Tipo_Nomina_ID;
                Generar_Nomina.P_Nomina_ID = Nomina_ID;
                Generar_Nomina.P_No_Nomina = No_Nomina;

                if (RBL_Tipos_Nominas.SelectedIndex >= 0)
                {
                    if (Generar_Nomina.Existe_Nomina_Ya_Generada(RBL_Tipos_Nominas.SelectedValue.Trim()))
                    {
                        if (Validar_Si_Nomina_Fue_Cerrada())
                        {
                            Btn_Generar_Nomina.Visible = false;
                            Btn_Cerrar_Nomina.Visible = false;
                        }
                        else
                        {
                            Btn_Cerrar_Nomina.Visible = true;
                            Btn_Generar_Nomina.Text = "Regenerar Nómina";
                            Btn_Generar_Nomina.Visible = true;
                        }
                    }
                    else
                    {
                        Btn_Generar_Nomina.Visible = true;
                        Btn_Generar_Nomina.Text = "Generar Nómina";
                        Btn_Cerrar_Nomina.Visible = false;
                    }
                }
            }
            ScriptManager.RegisterStartupScript(UPnl_Generar_Nomina, typeof(string), "Imagen", "javascript:inicializarEventos_Generacion_Nomina();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al seleccionar el tipo de nómina a generar. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Validación Presupuestal)

    #region (Main Validaciones)
    /// ********************************************************************************
    /// Nombre: Crear_Tabla_Mostrar_Errores_Pagina
    /// Descripción: Crea la tabla que almacenara que datos son requeridos 
    /// por el sistema
    /// Creo: Juan Alberto Hernández Negrete 
    /// Fecha Creo: 20/Octubre/2010
    /// Modifico:
    /// Fecha Modifico:
    /// Causa Modifico:
    /// ********************************************************************************
    private String Crear_Tabla_Mostrar_Errores_Pagina(String Errores)
    {
        String Tabla_Inicio = "<table style='width:100%px;font-size:10px;color:red;text-align:left;'>";
        String Tabla_Cierra = "</table>";
        String Fila_Inicia = "<tr>";
        String Fila_Cierra = "</tr>";
        String Celda_Inicia = "<td style='width:25%;text-align:left;vertical-align:top;font-size:10px;' " +
                                "onmouseover=this.style.background='#DFE8F6';this.style.color='#000000'" +
                                " onmouseout=this.style.background='#ffffff';this.style.color='red'>";
        String Celda_Cierra = "</td>";
        char[] Separador = { '+' };
        String[] _Errores_Temp = Errores.Replace("<br>", "").Split(Separador);
        String[] _Errores = new String[(_Errores_Temp.Length - 1)];
        String Tabla;
        String Filas = "";
        String Celdas = "";
        int Contador_Celdas = 1;
        for (int i = 0; i < _Errores.Length; i++) _Errores[i] = _Errores_Temp[i + 1];

        Tabla = Tabla_Inicio;
        for (int i = 0; i < _Errores.Length; i++)
        {
            if (Contador_Celdas == 5)
            {
                Filas += Fila_Inicia;
                Filas += Celdas;
                Filas += Fila_Cierra;
                Celdas = "";
                Contador_Celdas = 0;
                i = i - 1;
            }
            else
            {
                Celdas += Celda_Inicia;
                Celdas += "<b style='font-size:12px;'>+</b>" + _Errores[i];
                Celdas += Celda_Cierra;
            }
            Contador_Celdas = Contador_Celdas + 1;
        }
        if (_Errores.Length < 5 || Contador_Celdas > 0)
        {
            Filas += Fila_Inicia;
            Filas += Celdas;
            Filas += Fila_Cierra;
        }
        Tabla += Filas;
        Tabla += Tabla_Cierra;
        return Tabla;
    }
    ///************************************************************************************
    /// Nombre Método: Consultar_Monto_Por_Nomina_Periodo_UR_Concepto
    /// 
    /// Descripción: Método que realiza la validacion presupuestal de la nomina.
    /// 
    /// Parámetros: No Áplica.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Boolean Validar_Nomina_Presupuestalmente()
    {
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO_NOMINA = null;//Variable que almacenara el parámetro de la nómina.
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Unidades_Responsables = null;//Variable que almacenara un listado de las unidades responsables.
        double Total_Prima_Vacacional = 0.0;//Variable que almacenara el total de la prima vacacional.
        double Total_Aguinaldo = 0.0;//Variable que almacenara el total del aguinaldo.
        double Total_Prestacione_Establecidas_Por_Condiciones_Trabajo = 0.0;//variable que almacenara el total de prestaciones establecidas por condiciones de trabajo.
        double Total_Sueldos = 0.0;//Variable que almacena el total de sueldos.
        double Total_Honorarios_Asimilados = 0.0;//Variables que almacena el total de honorarios asimilados.
        double Total_Remuneraciones_Eventuales = 0.0;//Variable que almacena el total de remuneracion para eventuales.
        double Total_Pensionados = 0.0;//Variable que almacena la cantidad de pensiones a pagar.
        double Total_Dietas = 0.0;
        double Total_Prevision_Social_Multiple = 0.0;//Variable que almacena el total de prevision social multiple.
        Lbl_Mensaje_Error.Text = String.Empty;//Control que mostrara al usuario el mensaje según el resultado de las operaciones de validación del presupuesto.
        Boolean Estatus = true;//variable que almacena el estatus de las validaciones TRUE OR FALSE en la primera opción significa que presupuestalmente la catorcena es sustentable,
                               //la segunda opción significa que el presupuesto es insuficiente para cubrir el pago de la nomina. 

        try
        {
            //SE CONSULTARA EL PARÁMETRO DE NÓMINA, ESTO PARA PODER CONSULTAR EL MONTO TOTAL DE LAS PERCEPCIONES 
            //QUE ENTRARIAN EN LA VALIDACIÓN PRESUPUESTAL DE LA PARTIDA DE OTRAS PRESTACIONES.
            INF_PARAMETRO_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            //CONSULTAMOS LAS UNIDADES RESPONSABLES QUE EXISTEN ACTUALMENTE.
            Dt_Unidades_Responsables = Obj_Dependencias.Consulta_Dependencias();

            //OBTENEMOS UN OBJETO ANONIMO QUE ALMACENARA LA CLAVE Y EL NOMBRE DE LAS UNIDADES RESPONSABLES.
            var unidadesResponsables = from unidadesResponsablesQuery in Dt_Unidades_Responsables.AsEnumerable()
                                       select new
                                                  {
                                                      UR =
                                           unidadesResponsablesQuery.Field<String>(Cat_Dependencias.Campo_Dependencia_ID),
                                                      Nombre =
                                           unidadesResponsablesQuery.Field<String>(Cat_Dependencias.Campo_Nombre),
                                                      Clave =
                                           unidadesResponsablesQuery.Field<String>(Cat_Dependencias.Campo_Clave)
                                                  };


            #region (Validacion de Partidas que Aplican por Unidad Responsable)
            foreach (var unidadResponsable in unidadesResponsables)
            {
                //Limpiamos variables.
                Total_Prima_Vacacional = 0.0;
                Total_Aguinaldo = 0.0;
                Total_Prestacione_Establecidas_Por_Condiciones_Trabajo = 0.0;
                Total_Sueldos = 0.0;
                Total_Honorarios_Asimilados = 0.0;
                Total_Remuneraciones_Eventuales = 0.0;
                Total_Pensionados = 0.0;
                Total_Prevision_Social_Multiple = 0.0;
                Total_Dietas = 0.0;

                /********************************************************************************************************************************************
                 *                                                          SUELDOS BASE Y SUBSEMUN [1131]
                 ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE SUELDOS POR DEPENDENCIA.
                Total_Sueldos +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Sueldo_Normal, "BASE-SUBSEMUN-SUBROGADOS");

                Total_Sueldos += 
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00106", "BASE-SUBSEMUN-SUBROGADOS");//[P-006] REINTEGROS.

                Total_Sueldos +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00108", "BASE-SUBSEMUN-SUBROGADOS");//[P-008] COMPLEMENTO DE SUELDO.

                Total_Sueldos +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00124", "BASE-SUBSEMUN-SUBROGADOS");//[P-026] RETROACTIVO

                Total_Sueldos +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00135", "BASE-SUBSEMUN-SUBROGADOS");//[P-037] VACACIONES

                Total_Sueldos +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00136", "BASE-SUBSEMUN-SUBROGADOS");//[P-038] SUBSIDIO POR INCAPACIDAD

                Total_Sueldos +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00141", "BASE-SUBSEMUN-SUBROGADOS");//[P-043] INASISTENCIA JUSTIFICADA

                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE SUELDOS.
                if (!Validar_Sueldos_Base(unidadResponsable.UR, Total_Sueldos))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de los SUELDOS BASE. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }

                /********************************************************************************************************************************************
                 *                                                          HONORARIOS ASIMILADOS [1212]
                 ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE HONORARIOS ASIMILADOS POR DEPENDENCIA.
                Total_Honorarios_Asimilados =
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Sueldo_Normal, "ASIMILABLE");

                Total_Honorarios_Asimilados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00106", "ASIMILABLE");//[P-006] REINTEGROS.

                Total_Honorarios_Asimilados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00108", "ASIMILABLE");//[P-008] COMPLEMENTO DE SUELDO.

                Total_Honorarios_Asimilados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00124", "ASIMILABLE");//[P-026] RETROACTIVO

                Total_Honorarios_Asimilados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00135", "ASIMILABLE");//[P-037] VACACIONES

                Total_Honorarios_Asimilados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00136", "ASIMILABLE");//[P-038] SUBSIDIO POR INCAPACIDAD

                Total_Honorarios_Asimilados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00141", "ASIMILABLE");//[P-043] INASISTENCIA JUSTIFICADA

                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE HONORARIOS ASIMILADOS.
                if (!Validar_Sueldos_Honorarios_Asimilados(unidadResponsable.UR, Total_Honorarios_Asimilados))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de los HONORARIOS ASIMILADOS. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }

                /********************************************************************************************************************************************
                 *                                                          REMUNERACIONES EVENTUALES [1221]
                 ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE REMUNERACIONES EVENTUALES POR DEPENDENCIA.
                Total_Remuneraciones_Eventuales =
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Sueldo_Normal, "EVENTUAL");

                Total_Remuneraciones_Eventuales +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00106", "EVENTUAL");//[P-006] REINTEGROS.

                Total_Remuneraciones_Eventuales +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00108", "EVENTUAL");//[P-008] COMPLEMENTO DE SUELDO.

                Total_Remuneraciones_Eventuales +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00124", "EVENTUAL");//[P-026] RETROACTIVO

                Total_Remuneraciones_Eventuales +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00135", "EVENTUAL");//[P-037] VACACIONES

                Total_Remuneraciones_Eventuales +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00136", "EVENTUAL");//[P-038] SUBSIDIO POR INCAPACIDAD

                Total_Remuneraciones_Eventuales +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00141", "EVENTUAL");//[P-043] INASISTENCIA JUSTIFICADA

                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE REMUNERACIONES EVENTUALES.
                if (!Validar_Sueldos_Remuneraciones_Eventuales(unidadResponsable.UR, Total_Remuneraciones_Eventuales))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de los REMUNERACIONES EVENTUALES. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }

                /********************************************************************************************************************************************
                *                                                          PENSIONADOS [4511]
                ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE PENSIONADOS POR DEPENDENCIA.
                Total_Pensionados =
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Sueldo_Normal, "PENSIONADO");

                Total_Pensionados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00106", "PENSIONADO");//[P-006] REINTEGROS.

                Total_Pensionados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00108", "PENSIONADO");//[P-008] COMPLEMENTO DE SUELDO.

                Total_Pensionados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00124", "PENSIONADO");//[P-026] RETROACTIVO

                Total_Pensionados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00135", "PENSIONADO");//[P-037] VACACIONES

                Total_Pensionados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00136", "PENSIONADO");//[P-038] SUBSIDIO POR INCAPACIDAD

                Total_Pensionados +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00141", "PENSIONADO");//[P-043] INASISTENCIA JUSTIFICADA

                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE PENSIONADOS.
                if (!Validar_Sueldos_Pensionados(unidadResponsable.UR, Total_Pensionados))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de los PENSIONADOS. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }

                /********************************************************************************************************************************************
                *                                                          DIETAS [1111]
                ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE DIETA POR DEPENDENCIA.
                Total_Dietas =
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Sueldo_Normal, "DIETA");

                Total_Dietas +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00106", "DIETA");//[P-006] REINTEGROS.

                Total_Dietas +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00108", "DIETA");//[P-008] COMPLEMENTO DE SUELDO.

                Total_Dietas +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00124", "DIETA");//[P-026] RETROACTIVO

                Total_Dietas +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00135", "DIETA");//[P-037] VACACIONES

                Total_Dietas +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00136", "DIETA");//[P-038] SUBSIDIO POR INCAPACIDAD

                Total_Dietas +=
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), "00141", "DIETA");//[P-043] INASISTENCIA JUSTIFICADA
                
                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE DIETAS.
                if (!Validar_Sueldos_Dietas(unidadResponsable.UR, Total_Dietas))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de los DIETAS. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }
                /********************************************************************************************************************************************
                 *                                                          PREVISION SOCIAL MULTIPLE [1593]
                 ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE PREVISION SOCIAL MULTIPLE POR DEPENDENCIA.
                Total_Prevision_Social_Multiple =
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Prevision_Social_Multiple);

                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE PREVISION SOCIAL MULTIPLEL.
                if (!Validar_Prevision_Social_Multiple(unidadResponsable.UR, Total_Prevision_Social_Multiple))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de la Previsión Social Múltiple. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }

                /********************************************************************************************************************************************
                 *                                                          PRIMA VACACIONAL [1321]
                 ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE PRIMA VACACIONAL POR DEPENDENCIA.
                Total_Prima_Vacacional =
                    Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                        unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                        Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Prima_Vacacional);
                
                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE PRIMA VACIONAL.
                if (!Validar_Prima_Vacacional(unidadResponsable.UR, Total_Prima_Vacacional))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de prima vacacional. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }

                /********************************************************************************************************************************************
                 *                                                              AGUINALDO [1323] 
                 ********************************************************************************************************************************************/
                //OBTENEMOS EL MONTO DE AGUINALDO POR DEPENDENCIA.
                Total_Aguinaldo = Cls_Help_Nom_Validate_Presupuestal.Consultar_Montos_Conceptos_Por_Unidad_Responsable(
                    unidadResponsable.UR, Cmb_Calendario_Nomina.SelectedValue.Trim(),
                    Cmb_Periodo.SelectedItem.Text.Trim(), INF_PARAMETRO_NOMINA.P_Percepcion_Aguinaldo);
                
                //VALIDAMOS EL PRESUPUESTO POR UNIDAD RESPONSABLE PARA LA PARTIDA DE PRIMA VACIONAL.
                if (!Validar_Aguinaldo(unidadResponsable.UR, Total_Aguinaldo))
                {
                    Lbl_Mensaje_Error.Text +=
                        "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de aguinaldo. Unidad Responsable: [" +
                        unidadResponsable.Clave + "] - " + unidadResponsable.Nombre +
                        ". !!!.</td></tr></tbody></table></center>";
                    Estatus = false;
                }
            }
            #endregion

            #region (Validación de Partidas que Aplican de Forma Global RH)
            /******************************************************************************************
             *                 REMUNERACIONES POR HORAS EXTRAORDINARIAS [1331]
            ******************************************************************************************/
            if (!Validar_Tiempo_Extra(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodo.SelectedItem.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text +=
                    "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de horas extra. La PARTIDA [1331] se encuentra en la Dirección de RH. !!!.</td></tr></tbody></table></center>";
                Estatus = false;
            }

            /******************************************************************************************
            *                      PRIMA DOMINICAL [1322]
            ******************************************************************************************/
            if (!Validar_Prima_Dominical(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodo.SelectedItem.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text +=
                    "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de prima dominical. La PARTIDA [1322] se encuentra en la Dirección de RH. !!!.</td></tr></tbody></table></center>";
                Estatus = false;
            }

            /******************************************************************************************
            *                      CUOTAS PARA EL FONDO DE AHORRO [1511] (FONDO DE RETIRO)
            ******************************************************************************************/
            if (!Validar_Fondo_Retiro(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodo.SelectedItem.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text +=
                    "<tr><td colspan='2' style='color:black; font-size:10px; width:100%;' align='center'>¡¡¡Presupuesto insuficiente para el pago de fondo de ahorro. La PARTIDA [1511] se encuentra en la Dirección de RH. !!!.</td></tr></tbody></table></center>";
                Estatus = false;
            }

            /******************************************************************************************
            *                      OTRAS PRESTACIONES  [1592]
            ******************************************************************************************/
            if (!Validar_Percepciones_Otras_Prestaciones(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodo.SelectedItem.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text +=
                    "<tr><td colspan='2' style='color:black; font-size:10px;' align='center'>¡¡¡ Presupuesto insuficiente para el pago de otras prestaciones. La PARTIDA [1592] se encuentra en la Dirección de RH !!!.</td></tr></tbody></table></center>";
                Estatus = false;
            }
            /******************************************************************************************
            *                     PRESTACIONES ESTABLECIDAS POR CONDICIONES DE TRABAJO  [1541]
            ******************************************************************************************/
            if (!Validar_Percepciones_Prestaciones_Establecidas_Condiciones_Trabajo(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodo.SelectedItem.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text +=
                    "<tr><td colspan='2' style='color:black; font-size:10px;' align='center'>¡¡¡ Presupuesto insuficiente para el pago de Prestaciones Establecidas por Condiciones de Trabajo. La PARTIDA [1541] se encuentra en la Dirección de RH !!!.</td></tr></tbody></table></center>";
                Estatus = false;
            }

            /******************************************************************************************
            *   PARTICIPACIONES POR VIGILANCIA EN EL CUMPLIMIENTO DE LAS LEYES  [1381]
            *******************************************************************************************/
            if (!Validar_Percepciones_Participacion_Por_Vigilancia(Cmb_Calendario_Nomina.SelectedValue.Trim(), Cmb_Periodo.SelectedItem.Text.Trim()))
            {
                Lbl_Mensaje_Error.Text +=
                    "<tr><td colspan='2' style='color:black; font-size:10px;' align='center'>¡¡¡ Presupuesto insuficiente para el pago de Participaciones por Vigilancia. La PARTIDA [1381] se encuentra en la Dirección de RH !!!.</td></tr></tbody></table></center>";
                Estatus = false;
            }

            #endregion
           
            if(!Estatus)
            {
                Estatus = false;

                StringBuilder Texto = new StringBuilder();

                Texto.Append("VALIDACIÓN PRESUPUESTAL: <hr />");
                Texto.Append("<div id='Div_Presupuesto' style='position:relative; left:10px; overflow:auto;height:300px;width:99%;vertical-align:top;'>");
                Texto.Append(Lbl_Mensaje_Error.Text.ToString());
                Texto.Append("</div>");
                Texto.Append("<div align='right'>");
                Texto.Append("<a href='#' onclick='printme($(" + @"""#Div_Presupuesto" + @"""" + "));' style='color:#333; font-size:12px; font-family: Arial;'>Pantalla Completa</a>");
                Texto.Append("</div>");

                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Texto.ToString());
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al consultar las unidades responbles. Error: [" + ex.Message + "]");
        }
        return false;
    }
    ///************************************************************************************
    /// Nombre Método: Obtener_Monto_Total_Tabla
    /// 
    /// Descripción: Método que obtiene el monto total que se pagara del concepto que se pasa
    ///              como parámetro.
    /// 
    /// Parámetros: Dt_Tabla_Totales.- Listado a buscar el total del concepto.
    ///             Percepcion_Deduccion_ID.- Identificador del concepto a consultar su total.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete.
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Double Obtener_Monto_Total_Tabla(DataTable Dt_Tabla_Totales, String Percepcion_Deduccion_ID)
    {
        Double Total = 0.0;//Variable que almacenara el monto total a pagar en la nómina a generar del concepto que se pasa al método como parámtro.

        try
        {
            //Iteramos sobre los registros obtenidos de la tabla de totales que se obtuvo de la consulta realizada por nomina y periodo del concepto
            //el cuál se le pasa a este método como parámetro y del cuál obtendremos el total.
            if (Dt_Tabla_Totales is DataTable)
            {
                if (Dt_Tabla_Totales.Rows.Count > 0)
                {
                    foreach (DataRow CONCEPTO_FILA in Dt_Tabla_Totales.Rows)
                    {
                        if (CONCEPTO_FILA is DataRow)
                        {
                            foreach (DataColumn CONCEPTO in Dt_Tabla_Totales.Columns)
                            {
                                if (CONCEPTO is DataColumn)
                                {
                                    if (CONCEPTO.ColumnName.Contains(Percepcion_Deduccion_ID))
                                    {
                                        if (!String.IsNullOrEmpty(CONCEPTO_FILA[CONCEPTO.ColumnName].ToString()))
                                        {
                                            Total += Convert.ToDouble(CONCEPTO_FILA[CONCEPTO.ColumnName].ToString());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al obtener el monto de la tabla de totales. Error: [" + Ex.Message + "]");
        }
        return Total;
    }
    #endregion

    #region (Partidas que son propias de la unidad responsable de RECURSOS HUMANOS)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Tiempo_Extra
    ///
    ///DESCRIPCIÓN: Se valida que lo que se solicito de tiempo extra durante la catorcena
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Tiempo_Extra(String Nomina_ID, String Periodo)
    {
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;//Variable que almacena el parámetro de la nómina.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacena el parámetro contable.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO_TIEMPO_EXTRA = null;//Variable de conexión con la clase ayudante para la validación presupuestal.         
        DataTable Dt_Tabla_Totales = null;//Variable que almacena el registro de la tabla de totales que se pago en la nómina generada.
        String Unidad_Responsable_ID = "00033";//Variable que almacena el identificador de la unidad responsable de DIRECCION DE RECURSOS HUMANOS.
        Double Presupuesto_Disponible_Horas_Extra = 0.0;//Variable que almacena el monto que presupuestalmente se tiene para el pago de horas extra.
        Double Total_Horas_Extra = 0.0;//Variable que almacena la cantidad que se pagara de horas extra en la catorcena en la cuál se genero la nómina.
        Double Total_Dias_Festivos = 0.0;//Variable que almacena la cantidad que se pagara de días festivos en la catorcena en la cuál se genero la nómina
        Double Total_Pago_Dias_Dobles = 0.0;//Variable que almacena la cantidad que se pagara de días dobles en la catorcena en la cuál se genero la nómina
        Double TOTAL = 0.0;//Variable que almacena el total a pagar por concepto de horas extra. 
        Boolean Estatus = true;//Variable que se usara para la validación presupuestal del tiempo extra solicitado contra la partida de tiempo extra.

        try
        {
            //CREAMOS UNA INSTANCIA DE LA CLASE AYUDANTE PARA VALIDAR EL PRESUPUESTO DE LAS PARTIDAS DE NOMINA.
            PRESUPUESTO_TIEMPO_EXTRA = new Cls_Help_Nom_Validate_Presupuestal();

            //CONSULTAMOS LOS PARAMETROS DE NOMINA.
            INF_PARAMETROS =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            //CONSULTAMOS LOS PARAMETROS CONTABLES PARA NOMINA.
            INF_PARAMETRO_CONTABLE =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //CONSULTAMOS EL PRESUPUESTO DISPONIBLE PARA EL TIEMPO EXTRA.
            Presupuesto_Disponible_Horas_Extra = PRESUPUESTO_TIEMPO_EXTRA.Consultar_Disponible(Unidad_Responsable_ID,
                                                                                               INF_PARAMETRO_CONTABLE.
                                                                                                   P_Horas_Extra);
            //CONSULTAMOS EN LA TABLA DE [OPE_NOM_TOTALES_NOMINA] LOS REGISTROS DE LA NÓMINA GENERADA.
            Dt_Tabla_Totales = Cls_Help_Nom_Validate_Presupuestal.Consultar_Total_Nomina(Nomina_ID, Periodo);

            //OBTENEMOS EL MONTO TOTAL DE HORAS EXTRA A PAGAR.
            Total_Horas_Extra = Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, INF_PARAMETROS.P_Percepcion_Horas_Extra);
            //OBTENEMOS EL MONTO TOTAL DE DIAS FESTIVOS A PAGAR.
            Total_Dias_Festivos = Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, INF_PARAMETROS.P_Percepcion_Dias_Festivos);
            //OBTENEMOS EL MONTO TOTAL DE DIAS DOBLES A PAGAR.
            Total_Pago_Dias_Dobles = Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, INF_PARAMETROS.P_Percepcion_Dia_Doble);

            //TOTAL A PAGAR EN NOMINA POR CONCEPTO DE HORAS EXTRA.
            TOTAL = (Total_Horas_Extra + Total_Dias_Festivos + Total_Pago_Dias_Dobles);
            
            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA HORAS EXTRA SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(TOTAL <= Presupuesto_Disponible_Horas_Extra))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1331]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de Horas Extra a Pagar en la Catorcena " + Periodo);
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", TOTAL));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Remuneracion por Horas Extraordinarias ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Presupuesto_Disponible_Horas_Extra));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //Si el presupuesto para horas extra es insuficiente se le avisara al usuario.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar el tiempo extra solicitado en la catorcena " + Periodo + ". Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Prima_Dominical
    ///
    ///DESCRIPCIÓN: Se valida que lo que se solicito de prima dominical durante la catorcena
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Prima_Dominical(String Nomina_ID, String Periodo)
    {
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;//Variable que almacena el parámetro de la nómina.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacena el parámetro contable.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO_PRIMA_DOMINICAL = null;//Variable de conexión con la clase ayudante para la validación presupuestal.         
        DataTable Dt_Tabla_Totales = null;//Variable que almacena el registro de la tabla de totales que se pago en la nómina generada.
        String Unidad_Responsable_ID = "00033";//Variable que almacena el identificador de la unidad responsable de DIRECCION DE RECURSOS HUMANOS.
        Double Presupuesto_Disponible_Prima_Dominical = 0.0;//Variable que almacena el monto que presupuestalmente se tiene para el pago de prima dominical.
        Double Total_Prima_Dominical = 0.0;//Variable que almacena la cantidad que se pagara de prima dominical en la catorcena en la cuál se genero la nómina.
        Boolean Estatus = true;//Variable que se usara para la validación presupuestal de prima dominical solicitado contra la partida de prima dominical.

        try
        {
            //CREAMOS UNA INSTANCIA DE LA CLASE AYUDANTE PARA VALIDAR EL PRESUPUESTO DE LAS PARTIDAS DE NOMINA.
            PRESUPUESTO_PRIMA_DOMINICAL = new Cls_Help_Nom_Validate_Presupuestal();

            //CONSULTAMOS LOS PARAMETROS DE NOMINA.
            INF_PARAMETROS =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            //CONSULTAMOS LOS PARAMETROS CONTABLES PARA NOMINA.
            INF_PARAMETRO_CONTABLE =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //CONSULTAMOS EL PRESUPUESTO DISPONIBLE PARA EL PAGO DE PRIMA DOMINICAL.
            Presupuesto_Disponible_Prima_Dominical = PRESUPUESTO_PRIMA_DOMINICAL.Consultar_Disponible(Unidad_Responsable_ID,
                                                                                               INF_PARAMETRO_CONTABLE.
                                                                                                   P_Prima_Dominical);
            //CONSULTAMOS EN LA TABLA DE [OPE_NOM_TOTALES_NOMINA] LOS REGISTROS DE LA NÓMINA GENERADA.
            Dt_Tabla_Totales = Cls_Help_Nom_Validate_Presupuestal.Consultar_Total_Nomina(Nomina_ID, Periodo);

            //OBTENEMOS EL MONTO TOTAL DE PRIMA DOMINICAL A PAGAR.
            Total_Prima_Dominical = Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, INF_PARAMETROS.P_Percepcion_Prima_Dominical);
            
            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA PRIMA DOMINICAL SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Prima_Dominical <= Presupuesto_Disponible_Prima_Dominical))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1322]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de PRIMA DOMINICAL a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Prima_Dominical));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Prima Dominical ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Presupuesto_Disponible_Prima_Dominical));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA PRIMA DOMINICAL ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar la prima dominical solicitado en la catorcena " + Periodo + ". Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Fondo_Retiro
    ///
    ///DESCRIPCIÓN: Se valida que lo que se solicito de fondo de retiro durante la catorcena
    ///             
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Fondo_Retiro(String Nomina_ID, String Periodo)
    {
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETROS = null;//Variable que almacena el parámetro de la nómina.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacena el parámetro contable.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO_FONDO_RETIRO = null;//Variable de conexión con la clase ayudante para la validación presupuestal.         
        DataTable Dt_Tabla_Totales = null;//Variable que almacena el registro de la tabla de totales que se pago en la nómina generada.
        String Unidad_Responsable_ID = "00033";//Variable que almacena el identificador de la unidad responsable de DIRECCION DE RECURSOS HUMANOS.
        Double Presupuesto_Disponible_Fondo_Retiro = 0.0;//Variable que almacena el monto que presupuestalmente se tiene para el pago de fondo de retiro.
        Double Total_Fondo_Retiro = 0.0;//Variable que almacena la cantidad que se pagara de fondo de retiro en la catorcena en la cuál se genero la nómina.
        Boolean Estatus = true;//Variable que se usara para la validación presupuestal de fondo de ahorro solicitado contra la partida de fondo de retiro.

        try
        {
            //CREAMOS UNA INSTANCIA DE LA CLASE AYUDANTE PARA VALIDAR EL PRESUPUESTO DE LAS PARTIDAS DE NOMINA.
            PRESUPUESTO_FONDO_RETIRO = new Cls_Help_Nom_Validate_Presupuestal();

            //CONSULTAMOS LOS PARAMETROS DE NOMINA.
            INF_PARAMETROS =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            //CONSULTAMOS LOS PARAMETROS CONTABLES PARA NOMINA.
            INF_PARAMETRO_CONTABLE =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //CONSULTAMOS EL PRESUPUESTO DISPONIBLE PARA EL PAGO DE FONDO DE RETIRO.
            Presupuesto_Disponible_Fondo_Retiro = PRESUPUESTO_FONDO_RETIRO.Consultar_Disponible(Unidad_Responsable_ID,
                                                                                               INF_PARAMETRO_CONTABLE.
                                                                                                   P_Cuotas_Fondo_Retiro);
            //CONSULTAMOS EN LA TABLA DE [OPE_NOM_TOTALES_NOMINA] LOS REGISTROS DE LA NÓMINA GENERADA.
            Dt_Tabla_Totales = Cls_Help_Nom_Validate_Presupuestal.Consultar_Total_Nomina(Nomina_ID, Periodo);

            //OBTENEMOS EL MONTO TOTAL DE FONDO DE RETIRO A PAGAR.
            Total_Fondo_Retiro = Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, INF_PARAMETROS.P_Percepcion_Fondo_Retiro);
            
            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA EL FONDO DE AHORRO SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Fondo_Retiro <= Presupuesto_Disponible_Fondo_Retiro))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1511]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de FONDO RETIRO a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Fondo_Retiro));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Fondo de Ahorro ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Presupuesto_Disponible_Fondo_Retiro));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL FONDO DE AHORO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar el fondo de ahorro solicitado en la catorcena " + Periodo + ". Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Percepciones_Otras_Prestaciones
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo y la clave de la
    ///              percepción a validar presupuestalmente.
    /// 
    /// Parámetros: Nomina_ID.- Año del cuál se genera la nómina.
    ///             Periodo.- Periodo del cuál se genero la nómina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Boolean Validar_Percepciones_Otras_Prestaciones(String Nomina_ID, String Periodo)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida de otras prestaciones.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO_NOMINA = null;//Variable que almacenara el parámetro de la nómina.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        String Unidad_Responsable_ID = "00033";//Variable que almacena el identificador de la UR de RH.
        double MONTO_PRESUPUESTAL_OTRAS_PRESTACIONES = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de otras prestaciones.
        DataTable Dt_Tabla_Totales = null;//Variable que almacenara el listado de totales que se pagaron en la nómina actualmente generada.
        double Total_Percepcion_Nomina_Otras_Prestaciones = 0.0;//Variable que almacena el total que es la suma de lo que se lapago a los empleado de los conceptos que entran en la partida de otras prestaciones.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (OTRAS PRESTACIONES)
            MONTO_PRESUPUESTAL_OTRAS_PRESTACIONES = PRESUPUESTO.Consultar_Disponible(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Prestaciones);

            //CONSULTAMOS EN LA TABLA DE [OPE_NOM_TOTALES_NOMINA] LOS REGISTROS DE LA NÓMINA GENERADA.
            Dt_Tabla_Totales = Cls_Help_Nom_Validate_Presupuestal.Consultar_Total_Nomina(Nomina_ID, Periodo);

            //SE CONSULTARA EL PARÁMETRO DE NÓMINA, ESTO PARA PODER CONSULTAR EL MONTO TOTAL DE LAS PERCEPCIONES 
            //QUE ENTRARIAN EN LA VALIDACIÓN PRESUPUESTAL DE LA PARTIDA DE OTRAS PRESTACIONES.
            INF_PARAMETRO_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            //OBTENEMOS EL MONTO TOTAL DE ... n PERCEPCIONES QUE ENTREN EN LA PARTIDA DE OTRAS PRESTACIONES.
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00107");//RETRIBUCIONES POR ACTIVIDADES ESPECIALES [P-007]
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00109");//INSTRUCTORES DE LA ACADEMIA [P-009]
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00110");//BANDA DE GUERRA [P-010]
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00114");//PREVISION SOCIAL [P-014]
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00119");//MERITO ACADEMICO [P-019]
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00120");//PREVISION SOCIAL [P-020]
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00121");//PRESTACIONES SOCIALES [P-021]
            Total_Percepcion_Nomina_Otras_Prestaciones += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00126");//OTROS INGRESOS FIJOS [P-028]
            //.........

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Percepcion_Nomina_Otras_Prestaciones <= MONTO_PRESUPUESTAL_OTRAS_PRESTACIONES))
            {
                var Mensaje = new StringBuilder();
                Mensaje.Append("<br /><center><table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'>");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1592]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</tr>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("(P-007, P-009, P-010, P-014, P-019, P-020, P-021 y P-028) = ENGLOBAN EN LA PARTIDA DE OTRAS PRESTACIONES");                
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Percepcion_Nomina_Otras_Prestaciones));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Otras Prestaciones");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", MONTO_PRESUPUESTAL_OTRAS_PRESTACIONES));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Percepciones_Otras_Prestaciones]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Percepciones_Prestaciones_Establecidas_Condiciones_Trabajo
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo y la clave de la
    ///              percepción a validar presupuestalmente.
    /// 
    /// Parámetros: Nomina_ID.- Año del cuál se genera la nómina.
    ///             Periodo.- Periodo del cuál se genero la nómina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Boolean Validar_Percepciones_Prestaciones_Establecidas_Condiciones_Trabajo(String Nomina_ID, String Periodo)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida de prestaciones establecidas por condiciones de trabajo.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO_NOMINA = null;//Variable que almacenara el parámetro de la nómina.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        String Unidad_Responsable_ID = "00033";//Variable que almacena el identificador de la UR de RH.
        double MONTO_PRESTACIONES_ESTABLECIDAS_CONDICIONES_TRABAJO = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de prestaciones establecidas por condiciones de trabajo.
        DataTable Dt_Tabla_Totales = null;//Variable que almacenara el listado de totales que se pagaron en la nómina actualmente generada.
        double Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo = 0.0;//Variable que almacena el total que es la suma de lo que se la pago a los empleado de los conceptos que entran en la partida de prestaciones establecidas por condiciones de trabajo.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (PRESTACIONES ESTABLECIDAS CONDICIONES TRABAJO)
            MONTO_PRESTACIONES_ESTABLECIDAS_CONDICIONES_TRABAJO = PRESUPUESTO.Consultar_Disponible(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Prestaciones_Establecidas_Condiciones_Trabajo);

            //CONSULTAMOS EN LA TABLA DE [OPE_NOM_TOTALES_NOMINA] LOS REGISTROS DE LA NÓMINA GENERADA.
            Dt_Tabla_Totales = Cls_Help_Nom_Validate_Presupuestal.Consultar_Total_Nomina(Nomina_ID, Periodo);

            //SE CONSULTARA EL PARÁMETRO DE NÓMINA, ESTO PARA PODER CONSULTAR EL MONTO TOTAL DE LAS PERCEPCIONES 
            //QUE ENTRARIAN EN LA VALIDACIÓN PRESUPUESTAL DE LA PARTIDA DE PRESTACIONES ESTABLECIDAS CONDICIONES TRABAJO.
            INF_PARAMETRO_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            //OBTENEMOS EL MONTO TOTAL DE ... n PERCEPCIONES QUE ENTREN EN LA PARTIDA DE PRESTACIONES ESTABLECIDAS CONDICIONES TRABAJO.
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00111");//BONO DE MATANZA [P-011]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00115");//QUINQUENIO [P-015]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00117");//AYUDA DE TRANSPORTE [P-017]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00123");//DESPENSA SINDICATO [P-024]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00127");//QUINQUENIO (APOYO) [P-029]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00128");//AYUDA DE TRANSPORTE (APOYO) [P-030]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00131");//VALE DE DESPENSA [P-033]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00137");//VALE DE DESPENSA [P-039]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00187");//PRES SIND SER PUB [P-051]
            Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00189");//PRES SIND RASTRO [P-053]
            //.........

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo <= MONTO_PRESTACIONES_ESTABLECIDAS_CONDICIONES_TRABAJO))
            {
                var Mensaje = new StringBuilder();
                Mensaje.Append("<br /><center><table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'>");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1541]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</tr>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("(P-011, P-015, P-017, P-024, P-029, P-030, P-033 y P-039) = ENGLOBAN EN LA PARTIDA DE PRESTACIONES ESTABLECIDAS CONDICIONES TRABAJO");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Percepcion_Prestaciones_Establecidas_Condiciones_Trabajo));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Prestaciones Establecidas Condiciones Trabajo");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", MONTO_PRESTACIONES_ESTABLECIDAS_CONDICIONES_TRABAJO));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Percepciones_Prestaciones_Establecidas_Condiciones_Trabajo]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Percepciones_Participacion_Por_Vigilancia
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo a validar presupuestalmente.
    /// 
    /// Parámetros: Nomina_ID.- Año del cuál se genera la nómina.
    ///             Periodo.- Periodo del cuál se genero la nómina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    ///************************************************************************************
    private Boolean Validar_Percepciones_Participacion_Por_Vigilancia(String Nomina_ID, String Periodo)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida de otras prestaciones.
        Cls_Cat_Nom_Parametros_Negocio INF_PARAMETRO_NOMINA = null;//Variable que almacenara el parámetro de la nómina.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        String Unidad_Responsable_ID = "00033";//Variable que almacena el identificador de la UR de RH.
        double MONTO_PRESUPUESTAL_Participacion_Por_Vigilancia = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de Participación por Vigilancia.
        DataTable Dt_Tabla_Totales = null;//Variable que almacenara el listado de totales que se pagaron en la nómina actualmente generada.
        double Total_Percepcion_Nomina_Participacion_Por_Vigilancia = 0.0;//Variable que almacena el total que es la suma de lo que se pago a los empleado de los conceptos que entran en la partida de Participación por Vigilancia.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (PARTICIPACIÓN POR VIGILANCIA)
            MONTO_PRESUPUESTAL_Participacion_Por_Vigilancia = PRESUPUESTO.Consultar_Disponible(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Participacipaciones_Vigilancia);

            //CONSULTAMOS EN LA TABLA DE [OPE_NOM_TOTALES_NOMINA] LOS REGISTROS DE LA NÓMINA GENERADA.
            Dt_Tabla_Totales = Cls_Help_Nom_Validate_Presupuestal.Consultar_Total_Nomina(Nomina_ID, Periodo);

            //SE CONSULTARA EL PARÁMETRO DE NÓMINA, ESTO PARA PODER CONSULTAR EL MONTO TOTAL DE LAS PERCEPCIONES 
            //QUE ENTRARIAN EN LA VALIDACIÓN PRESUPUESTAL DE LA PARTIDA DE OTRAS PRESTACIONES.
            INF_PARAMETRO_NOMINA = Cls_Ayudante_Nom_Informacion._Informacion_Parametros_Nomina();

            //OBTENEMOS EL MONTO TOTAL DE ... n PERCEPCIONES QUE ENTREN EN LA PARTIDA DE PARTICIPACION POR VIGILANCIA.
            Total_Percepcion_Nomina_Participacion_Por_Vigilancia += Obtener_Monto_Total_Tabla(Dt_Tabla_Totales, "00167");//COMP. SERV. EXTRA. [P-049]
            //.........

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Percepcion_Nomina_Participacion_Por_Vigilancia <= MONTO_PRESUPUESTAL_Participacion_Por_Vigilancia))
            {
                var Mensaje = new StringBuilder();
                Mensaje.Append("<br /><center><table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'>");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1381]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</tr>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("(P-049) = ENGLOBAN EN LA PARTIDA DE PARTICIPACION POR VIGILANCIA");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Percepcion_Nomina_Participacion_Por_Vigilancia));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto de Participaciones por Vigilancia");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", MONTO_PRESUPUESTAL_Participacion_Por_Vigilancia));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Percepciones_Participacion_Por_Vigilancia]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #region (Partidas por UNIDAD RESPONSABLE)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Prima_Vacacional
    ///
    ///DESCRIPCIÓN: Se valida que lo que se solicito presupuestalmente de prima vacacional 
    ///             durante la nómina de prima vacacional.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida la partida de prima vacacional.
    ///             Total_Prima_Vacacional.- Total de Prima Vacacional a pagar en la catorcena que se pagara de nómina.
    /// 
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Prima_Vacacional(String Unidad_Responsable_ID, Double Total_Prima_Vacacional)
    {
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacena el parámetro contable.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO_PRIMA_VACACIONAL = null;//Variable de conexión con la clase ayudante para la validación presupuestal.         
        Double Presupuesto_Disponible_Prima_Vacacional = 0.0;//Variable que almacena el monto que presupuestalmente se tiene para el pago de prima vacacional.
        Boolean Estatus = true;//Variable que se usara para la validación presupuestal del tiempo extra solicitado contra la partida de prima vacacional.

        try
        {
            //CREAMOS UNA INSTANCIA DE LA CLASE AYUDANTE PARA VALIDAR EL PRESUPUESTO DE LAS PARTIDAS DE NOMINA.
            PRESUPUESTO_PRIMA_VACACIONAL = new Cls_Help_Nom_Validate_Presupuestal();

            //CONSULTAMOS LOS PARAMETROS CONTABLES PARA NOMINA.
            INF_PARAMETRO_CONTABLE =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //CONSULTAMOS EL PRESUPUESTO DISPONIBLE PARA EL PAGO DE PRIMA VACACIONAL.
            Presupuesto_Disponible_Prima_Vacacional = PRESUPUESTO_PRIMA_VACACIONAL.Consultar_Disponible(Unidad_Responsable_ID,
                                                                                               INF_PARAMETRO_CONTABLE.
                                                                                                   P_Prima_Vacacional);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA PRIMA VACACIONAL SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Prima_Vacacional <= Presupuesto_Disponible_Prima_Vacacional))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1321]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de PRIMA VACACIONAL a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Prima_Vacacional));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Prima Vacacional ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Presupuesto_Disponible_Prima_Vacacional));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA PRIMA VACACIONAL ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar la prima vacacional solicitado en la catorcena. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Aguinaldo
    ///
    ///DESCRIPCIÓN: Se valida que lo que se solicito presupuestalmente de Aguinaldo
    ///             durante la nómina de Aguinaldo.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida la partida de Aguinaldo.
    ///             Total_Aguinaldo.- Total de Aguinaldo a pagar en la nómina de Aguinaldo.            
    /// 
    ///CREO: Juan Alberto Hernández Negrete
    ///FECHA_CREO: Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Aguinaldo(String Unidad_Responsable_ID, Double Total_Aguinaldo)
    {
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacena el parámetro contable.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO_AGUINALDO = null;//Variable de conexión con la clase ayudante para la validación presupuestal.         
        Double Presupuesto_Disponible_Aguinaldo = 0.0;//Variable que almacena el monto que presupuestalmente se tiene para el pago de aguinaldo.
        Boolean Estatus = true;//Variable que se usara para la validación presupuestal del aguinaldo solicitado contra la partida de aguinaldo.

        try
        {
            //CREAMOS UNA INSTANCIA DE LA CLASE AYUDANTE PARA VALIDAR EL PRESUPUESTO DE LAS PARTIDAS DE NOMINA.
            PRESUPUESTO_AGUINALDO = new Cls_Help_Nom_Validate_Presupuestal();

            //CONSULTAMOS LOS PARAMETROS CONTABLES PARA NOMINA.
            INF_PARAMETRO_CONTABLE =
                Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //CONSULTAMOS EL PRESUPUESTO DISPONIBLE PARA EL PAGO DE AGUINALDO.
            Presupuesto_Disponible_Aguinaldo = PRESUPUESTO_AGUINALDO.Consultar_Disponible(Unidad_Responsable_ID,
                                                                                          INF_PARAMETRO_CONTABLE.
                                                                                              P_Gratificaciones_Fin_Anio);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA AGUINALDO SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Aguinaldo <= Presupuesto_Disponible_Aguinaldo))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1323]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de AGUINALDO a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Aguinaldo));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Gratificaciones de Fin de Año.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Presupuesto_Disponible_Aguinaldo));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA AGUINALDO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar el monto de aguinaldo solicitado en la catorcena [Validar_Aguinaldo]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Sueldos_Base
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo para validar presupuestalmente.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida de Sueldos.
    ///             Total_Sueldos.- Total de Sueldos a pagar en la catorcena que se pagara de nomina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    /// 
    ///************************************************************************************
    private Boolean Validar_Sueldos_Base(String Unidad_Responsable_ID, Double Total_Sueldos)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida Prestaciones Establecidas por Condiciones de Trabajo.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        double Monto_Presupuesto_Sueldo = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de Sueldos Base.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (SUELDOS BASE)
            Monto_Presupuesto_Sueldo = PRESUPUESTO.Consultar_Comprometido_Sueldos(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Sueldos_Base);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Sueldos <= Monto_Presupuesto_Sueldo))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1131]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de SUELDOS BASE a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Sueldos));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Sueldos Base ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Monto_Presupuesto_Sueldo));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Sueldos_Base]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Sueldos_Honorarios_Asimilados
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo para validar presupuestalmente.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida de Sueldos.
    ///             Total_Honorarios_Asimilados.- Total de Sueldos a pagar en la catorcena que se pagara de nomina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    /// 
    ///************************************************************************************
    private Boolean Validar_Sueldos_Honorarios_Asimilados(String Unidad_Responsable_ID, Double Total_Honorarios_Asimilados)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida Prestaciones Establecidas por Condiciones de Trabajo.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        double Monto_Presupuesto_Honorarios_Asimilados = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida Honorarios Asimilados.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (Honorarios Asimilados)
            Monto_Presupuesto_Honorarios_Asimilados = PRESUPUESTO.Consultar_Comprometido_Sueldos(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Honorarios_Asimilados);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Honorarios_Asimilados <= Monto_Presupuesto_Honorarios_Asimilados))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1212]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de SUELDOS HONORARIOS ASIMILADOS a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Honorarios_Asimilados));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Honorarios Asimilados ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Monto_Presupuesto_Honorarios_Asimilados));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Sueldos_Honorarios_Asimilados]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Sueldos_Remuneraciones_Eventuales
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo para validar presupuestalmente.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida de Sueldos.
    ///             Total_Remuneraciones_Eventuales.- Total de Sueldos a pagar en la catorcena que se pagara de nomina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    /// 
    ///************************************************************************************
    private Boolean Validar_Sueldos_Remuneraciones_Eventuales(String Unidad_Responsable_ID, Double Total_Remuneraciones_Eventuales)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida Prestaciones Establecidas por Condiciones de Trabajo.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        double Monto_Presupuesto_Remuneraciones_Eventuales = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de Remuneraciones Eventuales.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (Remuneraciones Eventuales)
            Monto_Presupuesto_Remuneraciones_Eventuales = PRESUPUESTO.Consultar_Comprometido_Sueldos(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Remuneraciones_Eventuales);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Remuneraciones_Eventuales <= Monto_Presupuesto_Remuneraciones_Eventuales))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1221]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de SUELDOS REMUNERACIONES EVENTUALES a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Remuneraciones_Eventuales));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Remuneraciones Eventuales ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Monto_Presupuesto_Remuneraciones_Eventuales));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Sueldos_Remuneraciones_Eventuales]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Sueldos_Pensionados
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo para validar presupuestalmente.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida de Sueldos.
    ///             Total_Pensionados.- Total de Sueldos a pagar en la catorcena que se pagara de nomina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    /// 
    ///************************************************************************************
    private Boolean Validar_Sueldos_Pensionados(String Unidad_Responsable_ID, Double Total_Pensionados)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida Pensionados.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        double Monto_Presupuesto_Pensionados = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de Pensionados.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (Pensionados)
            Monto_Presupuesto_Pensionados = PRESUPUESTO.Consultar_Disponible(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Pensiones);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Pensionados <= Monto_Presupuesto_Pensionados))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [4511]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de SUELDOS PENSIONES a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Pensionados));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Pensionados ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Monto_Presupuesto_Pensionados));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Sueldos_Pensionados]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Sueldos_Dietas
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo para validar presupuestalmente.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida de Sueldos.
    ///             Total_Dietas.- Total de Sueldos a pagar en la catorcena que se pagara de nomina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    /// 
    ///************************************************************************************
    private Boolean Validar_Sueldos_Dietas(String Unidad_Responsable_ID, Double Total_Dietas)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida Dietas.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        double Monto_Presupuesto_Dietas = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de Dietas.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (Dietas)
            Monto_Presupuesto_Dietas = PRESUPUESTO.Consultar_Disponible(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Dietas);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Dietas <= Monto_Presupuesto_Dietas))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1111]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de DIETAS a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Dietas));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de DIETAS ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Monto_Presupuesto_Dietas));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Sueldos_Dietas]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    ///************************************************************************************
    /// Nombre Método: Validar_Prevision_Social_Multiple
    /// 
    /// Descripción: Este método recibira como parámetro la nómina, periodo para validar presupuestalmente.
    /// 
    /// Parámetros: Unidad_Responsable_ID.- Unidad Responsable a validar su partida de Sueldos.
    ///             Total_Prevision_Social_Multiple.- Total de Previsión Social Múltiple a pagar en la catorcena que se pagara de nómina.
    ///
    /// Usuario creó: Juan Alberto Hernández Negrete
    /// Fecha Creó: Marzo/2012
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa modificación:
    /// 
    ///************************************************************************************
    private Boolean Validar_Prevision_Social_Multiple(String Unidad_Responsable_ID, Double Total_Prevision_Social_Multiple)
    {
        Boolean Estatus = true;//Variable que guarda el estus de la validación presupuestal de la partida Prestaciones Establecidas por Condiciones de Trabajo.
        Cls_Cat_Nom_Parametros_Contables_Negocio INF_PARAMETRO_CONTABLE = null;//Variable que almacenara el parámetro contable de la nómina.
        Cls_Help_Nom_Validate_Presupuestal PRESUPUESTO = new Cls_Help_Nom_Validate_Presupuestal();//Variable que almacenara el objeto de negocio de tipo ayudante para consultar el disponible de las partidas presupuestales.
        double Monto_Presupuesto_Prevision_Social_Multiple = 0.0;//Variable que almacenara el monto presupuestal que tiene la partida de Previsión Social Múltiple.

        try
        {
            //Consultamos el parametro contable de la nómina.
            INF_PARAMETRO_CONTABLE = Cls_Ayudante_Nom_Informacion._Informacion_Parametro_Contable();

            //Obtenemos el presupuesto por (PREVISION SOCIAL MULTIPLE)
            Monto_Presupuesto_Prevision_Social_Multiple = PRESUPUESTO.Consultar_Comprometido_Sueldos(Unidad_Responsable_ID,
                                                                  INF_PARAMETRO_CONTABLE.P_Prevision_Social_Multiple);

            //VALIDAMOS QUE EL PRESUPUESTO DISPONIBLE PARA LA PERCEPCION SEA MAYOR O IGUAL AL MONTO A PAGAR POR ESTE CONCEPTO.
            if (!(Total_Prevision_Social_Multiple <= Monto_Presupuesto_Prevision_Social_Multiple))
            {
                var Mensaje = new StringBuilder();

                Mensaje.Append("<br />");
                Mensaje.Append("<center>");
                Mensaje.Append("<table width='95%' style='border-style:solid; background-color: #ffcccc; border-color:silver;'");

                Mensaje.Append("<thead>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:80%;' align='left'>");
                Mensaje.Append("Partida [1593]");
                Mensaje.Append("</th>");
                Mensaje.Append("<th style='font-size:10px; background-color: #ffcccc; color:black; width:20%;' align='left'>");
                Mensaje.Append("($) TOTAL");
                Mensaje.Append("</th>");
                Mensaje.Append("</thead>");

                Mensaje.Append("<tbody>");
                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Total de PREVISIÓN SOCIAL MÚLTIPLE a Pagar.");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Total_Prevision_Social_Multiple));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Mensaje.Append("<tr>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:80%;' align='left'>");
                Mensaje.Append("Presupuesto Partida de Previsión Social Múltiple ");
                Mensaje.Append("</td>");
                Mensaje.Append("<td style='font-size:10px; background-color: #ffffff; width:20%;' align='center'>");
                Mensaje.Append(String.Format("{0:c}", Monto_Presupuesto_Prevision_Social_Multiple));
                Mensaje.Append("</td>");
                Mensaje.Append("</tr>");

                Lbl_Mensaje_Error.Text += Mensaje.ToString();

                //SI EL PRESUPUESTO PARA EL CONCEPTO ES INSUFICIENTE SE LE AVISARA AL USUARIO.
                Estatus = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Error al validar presupuestalmente las percepciones [Validar_Prevision_Social_Multiple]. Error: [" + ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #endregion

}
