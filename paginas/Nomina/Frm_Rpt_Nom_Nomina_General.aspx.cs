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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.ReportSource;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Reflection;
using Presidencia.Nomina_Reporte_Retardos_Faltas.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Tipos_Nominas.Negocios;
using CarlosAg.ExcelXmlWriter;
using Presidencia.Ayudante_CarlosAG;
using System.Text;
using Presidencia.Reportes_nomina_Fijos.Negocio;
using Presidencia.Incidencias_Checadas.Negocios;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Calendario_Reloj_Checador.Negocio;
using Presidencia.Cat_Parametros_Nomina.Negocio;
using Presidencia.Ayudante_Informacion;
using Presidencia.Prestamos.Negocio;
using Presidencia.Empleados.Negocios;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Reportes_Nomina_General.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Nomina_General : System.Web.UI.Page
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
                Inicializacion();
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
    #region Metodos Generales
    /// *************************************************************************************
    /// NOMBRE: Inicializacion
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
    protected void Inicializacion()
    {
        try
        {
            Consultar_Calendarios_Nomina();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al habilitar el estado inicial de los controles de la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
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

    #region Consultas
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION :
    /// PARAMETROS  :
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

        Cls_Cat_Nom_Calendario_Nominas_Negocio Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio = new Cls_Cat_Nom_Calendario_Nominas_Negocio(); //Variable de conexión hacia la capa de negocios
        DataTable Dt_Periodo_Nominal; //Obtiene los valores de la consulta y servira para poder asignar estos a los controles correspondientes

        try
        {
            Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.ToString();
            Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
            Dt_Periodo_Nominal = Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio.Consulta_Periodos_Nomina(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

            Dt_Parametros.Columns.Add("FECHA_INICIO", typeof(DateTime));
            Dt_Parametros.Columns.Add("FECHA_FIN", typeof(DateTime));
            Dr_Parametro = Dt_Parametros.NewRow();

            //Asigna los valores obtenidos de la consulta a los controles correspondientes
            foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
            {
                if (!String.IsNullOrEmpty(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                {
                    Dr_Parametro["FECHA_INICIO"] = Convert.ToDateTime(Registro[Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio].ToString());
                    Dr_Parametro["FECHA_FIN"] = Convert.ToDateTime(Registro[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString());
                }
            }

            Dt_Parametros.Rows.Add(Dr_Parametro);

            Dt_Parametros.TableName = "Dt_Parametros";
            return Dt_Parametros;
        }

        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los tipos de nomina que existen actualemte en sistema. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Parametros_Reporte
    /// DESCRIPCIÓN: Forma una tabla con el nombre del empleado en la sesión
    /// PARÁMETROS: No Aplica
    /// USUARIO CREO: Hugo Enrique Ramirez Aguilera
    /// FECHA CREO: 09-abr-2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Construir_Percepcion_Deduccion_Reporte(DataTable Dt_Percepcion, DataTable Dt_Deduccion)
    {
        DataTable Dt_Percepciones = new DataTable();
        DataRow Dr_Parametro;

        Dt_Percepciones.Columns.Add("CONCEPTO_PERCEPCION", typeof(string));
        Dt_Percepciones.Columns.Add("PERCEPCION", typeof(string));
        Dt_Percepciones.Columns.Add("IMPORTE_PERCEPCION", typeof(double));

        Dt_Percepciones.Columns.Add("CONCEPTO_DEDUCCION", typeof(string));
        Dt_Percepciones.Columns.Add("DEDUCCION", typeof(string));
        Dt_Percepciones.Columns.Add("IMPORTE_DEDUCCION", typeof(double));
        Dt_Percepciones.Columns.Add("SALDO", typeof(double));




        int Limite = 0;
        int Limite_Deduccion = 0;
        int Limite_Percepcion = 0;
        int Contador = 0;
        double Deduccion = 0.0;
        if (Dt_Percepcion is DataTable)
        {
            if (Dt_Percepcion.Rows.Count > 0)
            {
                if (Dt_Deduccion is DataTable)
                {
                    //  para saber el limite de la tabla mas grande
                    if (Dt_Deduccion.Rows.Count > 0)
                    {
                        if (Dt_Percepcion.Rows.Count > Dt_Deduccion.Rows.Count)
                        {
                            Limite = Dt_Percepcion.Rows.Count;
                            Limite_Deduccion=Dt_Deduccion.Rows.Count;
                            Limite_Percepcion = Dt_Percepcion.Rows.Count;
                        }
                        else
                        {
                            Limite = Dt_Deduccion.Rows.Count;
                            Limite_Deduccion = Dt_Deduccion.Rows.Count;
                            Limite_Percepcion = Dt_Percepcion.Rows.Count;
                        }

                         for (Contador=0; Contador <= Limite; Contador++)
                         {
                             Dr_Parametro = Dt_Percepciones.NewRow();

                             
                             if (Contador < Limite_Percepcion)
                             {
                                 Dr_Parametro["CONCEPTO_PERCEPCION"] = Dt_Percepcion.Rows[Contador][0].ToString();
                                 Dr_Parametro["PERCEPCION"] = Dt_Percepcion.Rows[Contador][1].ToString();

                                 if (!String.IsNullOrEmpty(Dt_Percepcion.Rows[Contador][2].ToString()))
                                     Dr_Parametro["IMPORTE_PERCEPCION"] = Convert.ToDouble(Dt_Percepcion.Rows[Contador][2].ToString());
                             }

                             if (Contador < Limite_Deduccion)
                             {
                                 Dr_Parametro["CONCEPTO_DEDUCCION"] = Dt_Deduccion.Rows[Contador][0].ToString();
                                 Dr_Parametro["DEDUCCION"] = Dt_Deduccion.Rows[Contador][1].ToString();

                                 Deduccion = Convert.ToDouble(Dt_Deduccion.Rows[Contador][2].ToString());

                                 if (!String.IsNullOrEmpty(Dt_Deduccion.Rows[Contador][2].ToString()))
                                     Dr_Parametro["IMPORTE_DEDUCCION"] = Convert.ToDouble(Dt_Deduccion.Rows[Contador][2].ToString());
                             }

                             Dt_Percepciones.Rows.Add(Dr_Parametro);
                         }
                    }
                }
            }
        }

        Dt_Percepciones.TableName = "Dt_Reporte";
        return Dt_Percepciones;
    }
    /// *************************************************************************************
    /// NOMBRE: Consultar_Percepcion_Deduccion
    /// 
    /// DESCRIPCIÓN: Consulta los Empleados de acuerdo a los filtros establecidos para
    ///              ejecutar la búsqueda.
    ///              
    /// PARÁMETROS: No Aplica
    /// 
    /// USUARIO CREO: Hugo Enrique Ramírez Aguilera
    /// FECHA CREO: 09/Abril/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected DataTable Consultar_Percepcion_Deduccion(String Tipo)
    {
        var Obj_Empleados = new Cls_Rpt_Nom_Nomina_General_Negocio();// Variable de conexión con la capa de negocios.
        DataTable Dt_Percepcion_Deduccion = null;// datatable que almacena una lista de empleados.

        try
        {
            // agregar filtro si se nomina id
            if (Cmb_Calendario_Nomina.SelectedIndex>0)
                Obj_Empleados.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;

            // agregar filtro numero de nomina
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
                Obj_Empleados.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;

            Obj_Empleados.P_Tipo = Tipo;
            Dt_Percepcion_Deduccion = Obj_Empleados.Consultar_Nomina_General();

        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los empleados. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepcion_Deduccion;
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
                    Consulta_Fechas_Periodo_Nominal();
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
    ///NOMBRE DE LA FUNCIÓN: Consulta_Fechas_Periodo_Nominal
    ///DESCRIPCIÓN: Consulta las fecchas del periodo nominal que fue seleccionado por
    ///             el usuario para poder realizar las asistencias de los empleados
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 05-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Fechas_Periodo_Nominal()
    {
        //cat_nom_nominas_detalles
        Cls_Cat_Nom_Calendario_Nominas_Negocio Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio = new Cls_Cat_Nom_Calendario_Nominas_Negocio(); //Variable de conexión hacia la capa de negocios
        DataTable Dt_Periodo_Nominal; //Obtiene los valores de la consulta y servira para poder asignar estos a los controles correspondientes
        try
        {
            Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.ToString();
            Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
            Dt_Periodo_Nominal = Rs_Consulta_Cat_Nom_Calendario_Nominas_Negocio.Consulta_Periodos_Nomina(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

            //Asigna los valores obtenidos de la consulta a los controles correspondientes
            foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
            {
                if (!String.IsNullOrEmpty(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                {
                    Txt_Fecha_Inicial.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Nominas.Campo_Fecha_Inicio].ToString()));
                    Txt_Fecha_Final.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Nominas.Campo_Fecha_Fin].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
        }
    }
    #endregion


    #region Formato
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION: Crea el DataTable con la consulta de las nomina vigentes en el 
    ///              sistema.
    /// PARAMETROS : Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///              en el sistema.
    /// CREO       : Juan Alberto Hernandez Negrete
    /// FECHA_CREO : 06/Abril/2011
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

    #endregion

    #region Validaciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    /// USUARIO CREO: Hugo Emrique Ramírez Aguilera
    /// FECHA CREO: 09/Mayo/2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Reporte
    ///DESCRIPCIÓN: Verifica que los campos esten seleccionados
    /// USUARIO CREO: Hugo Emrique Ramírez Aguilera
    /// FECHA CREO: 09/Mayo/2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Reporte()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario: <br>";
        Lbl_Mensaje_Error.Visible = true;
        Img_Error.Visible = true;

        //  validacion para cuando se selecciona algun numero de nomina o periodo
        if (((Txt_Fecha_Inicial.Text == "") && (Txt_Fecha_Final.Text == "")))
        {
            if (Cmb_Calendario_Nomina.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecciona la nomina.<br>";
                Datos_Validos = false;
            }
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex == 0)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Selecciona el periodo de la nomina.<br>";
                Datos_Validos = false;
            }
        }

        return Datos_Validos;
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
    /// USUARIO CREO: Hugo Emrique Ramírez Aguilera
    /// FECHA CREO: 09/Mayo/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Percepcion;//Variable que almacena un listado de empleados.
        DataTable Dt_Deduccion;//Variable que almacena un listado de empleados.
        DataTable Dt_Reporte;
        DataTable Dt_Parametros;

        try
        {
            if (Validar_Reporte())
            {
                Ds_Reporte = new DataSet();
                Dt_Percepcion = Consultar_Percepcion_Deduccion("PERCEPCION");
                Dt_Percepcion.TableName = "Dt_Percepcion";

                Dt_Deduccion = Consultar_Percepcion_Deduccion("DEDUCCION");
                Dt_Deduccion.TableName = "Dt_Deduccion";

                Dt_Parametros = Consultar_Parametros_Reporte();
                Dt_Parametros.TableName = "Dt_Parametros";

                Dt_Reporte = Construir_Percepcion_Deduccion_Reporte(Dt_Percepcion, Dt_Deduccion);

                Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
                Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

                Generar_Reporte(ref Ds_Reporte, "Cr_Rpt_Nom_Nomina_General.rpt", "Reporte_Nomina_General" + Session.SessionID + ".pdf");
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
    /// USUARIO CREO: Hugo Emrique Ramírez Aguilera
    /// FECHA CREO: 09/Mayo/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Btn_Generar_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        DataSet Ds_Reporte;
        DataTable Dt_Percepcion;//Variable que almacena un listado de empleados.
        DataTable Dt_Deduccion;
        DataTable Dt_Reporte;
        DataTable Dt_Parametros;

        try
        {
            if (Validar_Reporte())
            {
                Ds_Reporte = new DataSet();
                Dt_Percepcion = Consultar_Percepcion_Deduccion("PERCEPCION");
                Dt_Percepcion.TableName = "Dt_Percepcion";

                Dt_Deduccion = Consultar_Percepcion_Deduccion("DEDUCCION");
                Dt_Deduccion.TableName = "Dt_Deduccion";

                Dt_Parametros = Consultar_Parametros_Reporte();
                Dt_Parametros.TableName = "Dt_Parametros";

                Dt_Reporte = Construir_Percepcion_Deduccion_Reporte(Dt_Percepcion, Dt_Deduccion);

                Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
                Ds_Reporte.Tables.Add(Dt_Parametros.Copy());

                Exportar_Reporte(Ds_Reporte, "Cr_Rpt_Nom_Nomina_General.rpt", "Reporte_Nomina_General" + Session.SessionID, "xls", ExportFormatType.Excel);
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


    #region Combos
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO       : Juan Alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Periodos_Catorcenales_Nomina(Cmb_Calendario_Nomina.SelectedValue.Trim()); //Consulta los periodos nominales validos
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
    }
    protected void Cmb_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Int32 index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
            Txt_Fecha_Inicial.Text = "";
            Txt_Fecha_Final.Text = "";
            if (index > 0)
            {
                Consulta_Fechas_Periodo_Nominal(); //Consulta la fecha de inicio y termino para la generación de asistencias del empleado
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    #endregion

    #endregion

}
