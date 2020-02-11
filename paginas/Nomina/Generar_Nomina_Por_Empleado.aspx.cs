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

public partial class paginas_Nomina_Generar_Nomina_Por_Empleado : System.Web.UI.Page
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
    private void Configuracion_Inicial()
    {
        Cargar_Combo_Tipos_Nomina();//Se cargan los tipos de nómina que existen actualmente en el sistema.
        Consultar_Calendario_Nominas();//Se consultan los calendarios de nómina que existen actualmente en el sistema.
        Btn_Generar_Nomina.Visible = false;
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

        if (String.IsNullOrEmpty(Txt_Empleado.Text)) {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Falta ingresar el número de empleado. <br>";
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
            Cmb_Tipo_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
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


                    Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByText(
                        new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                    Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim());

                    Cmb_Periodo.SelectedIndex = Cmb_Periodo.Items.IndexOf(Cmb_Periodo.Items.FindByText(
                        new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));

                    Cmb_Periodo_SelectedIndexChanged(Cmb_Periodo, new EventArgs()); 
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
                                    if (No_Empleado.Equals(Txt_Empleado.Text.Trim()))
                                        Generar_Nomina.Generar_Nomina_Catorcenal(Empleado_ID, No_Empleado, Fecha, Nomina_Generar);
                                    break;
                                case "PVI":
                                    if (No_Empleado.Equals(Txt_Empleado.Text.Trim()))
                                        Generar_Nomina.Generar_Nomina_Prima_Vacacional(Empleado_ID, No_Empleado, Fecha, Nomina_Generar);
                                    break;
                                case "PVII_AGUINALDO":
                                    if (No_Empleado.Equals(Txt_Empleado.Text.Trim()))
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
                    }
                    else
                    {
                        Btn_Generar_Nomina.Text = "Regenerar Nómina";
                        Btn_Generar_Nomina.Visible = true;
                    }
                }
                else
                {
                    Btn_Generar_Nomina.Visible = true;
                    Btn_Generar_Nomina.Text = "Generar Nómina";
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
                        }
                        else
                        {
                            Btn_Generar_Nomina.Text = "Regenerar Nómina";
                            Btn_Generar_Nomina.Visible = true;
                        }
                    }
                    else
                    {
                        Btn_Generar_Nomina.Visible = true;
                        Btn_Generar_Nomina.Text = "Generar Nómina";
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

    protected void Txt_Empleado_TextChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

        try
        {
            INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_Empleado.Text);

            Cmb_Tipo_Nomina.SelectedIndex = Cmb_Tipo_Nomina.Items.IndexOf(
                Cmb_Tipo_Nomina.Items.FindByValue(INF_EMPLEADO.P_Tipo_Nomina_ID));
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    #endregion

}
