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
using Presidencia.Reportes_nomina_Vacaciones_Empleados.Negocio;

public partial class paginas_Nomina_Frm_Rpt_Nom_Vacaciones_Empleados : System.Web.UI.Page
{
    #region Page_Load
    protected void Page_Load(object sender, EventArgs e)
    {
        Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        try
        {
            if (!IsPostBack)
            {
                Inicializacion();

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

    #region Metodos

    #region Metodos Generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializacion
    /// DESCRIPCION :
    /// PARAMETROS  :
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 28/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializacion()
    {
        try
        {
            Limpiar_Controles();//Limpia los controles de la forma
            Consultar_Unidades_Responsables();
            Consultar_Tipos_Nominas();
            Consultar_Calendarios_Nomina();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 28/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_No_Empleado.Text = "";
            Txt_Nombre_Empleado.Text = "";

            if (Cmb_Nombre_Empleado.SelectedIndex > 0)
                Cmb_Nombre_Empleado.SelectedIndex = 0;

            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                Cmb_Unidad_Responsable.SelectedIndex = 0;

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = 0;

            if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                Cmb_Calendario_Nomina.SelectedIndex = 0;            
        }
        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
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

    #region Validaciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///             a partir del periodo actual.
    ///CREO       : Juan alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
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
    /// NOMBRE DE LA FUNCION: Validar_Reporte
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el reporte
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 28/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Reporte()
    {
        String Espacios_Blanco;
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario: <br>";
        Lbl_Mensaje_Error.Visible = true;
        Img_Error.Visible = true;

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
        return Datos_Validos;
    }

    #endregion // fin de validaciones

    #region Consultas
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
            Cargar_Combos(Cmb_Unidad_Responsable, Dt_Unidades_Responsables, "CLAVE_NOMBRE",
                Cat_Dependencias.Campo_Dependencia_ID, 0);//Se carga el control que almacena las unidades responsables.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las unidades responsables registradas en sistema. Error: [" + Ex.Message + "]");
        }
    }
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
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio(); //Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null; //Variable que almacena los calendarios nominales que existén actualmente en el sistema.
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
                Cmb_Calendario_Nomina.Items.Insert(0, new ListItem("<Seleccione>", ""));
                Cmb_Calendario_Nomina.SelectedIndex = -1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los calendarios de nómina que existen actualmente registrados en el sistema. Error: [" + Ex.Message + "]");
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
    #endregion

    #region Reporte
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Reporte
    /// DESCRIPCION : Consulta las ordenes judiciales que tenga el empleado
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 27/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte(String Formato)
    {
        Ds_Rpt_Nom_Vacaciones Ds_Reporte = new Ds_Rpt_Nom_Vacaciones();
        Cls_Rpt_Nom_Vacaciones_Empleados_Negocio Rs_Reporte_Vacaciones = new Cls_Rpt_Nom_Vacaciones_Empleados_Negocio();
        DataTable Dt_Reporte = new DataTable();
        DataTable Dt_Reporte_Final = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataRow Dt_Row;
        Double Nomina = 0.0;
        DataTable Dt_Elaboro = new DataTable();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Orden_Judicial_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        //String Cantidad_Letra = String.Empty;
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  para el filtro de numero de empleado
            if (!String.IsNullOrEmpty(Txt_No_Empleado.Text))
                Rs_Reporte_Vacaciones.P_No_Empleado = Txt_No_Empleado.Text;

            //  para el filtro de tipo de nomina
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Rs_Reporte_Vacaciones.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;

            //  para el filtro de unidad responsable
            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                Rs_Reporte_Vacaciones.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;


            //  para el filtro normal 
            Rs_Reporte_Vacaciones.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;
            Rs_Reporte_Vacaciones.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;
            Dt_Reporte = Rs_Reporte_Vacaciones.Consultar_Dias_Vacaciones();
            
            //  se construye la tabla
            Dt_Reporte_Final = Construir_Tabla_Vacaciones();
            if (Dt_Reporte is DataTable)
            {
                if (Dt_Reporte.Rows.Count > 0)
                {
                    foreach (DataRow Dt_Row_Vacaciones in Dt_Reporte.Rows)
                    {
                        if (Dt_Row_Vacaciones is DataRow)
                        {
                            Dt_Row = Dt_Reporte_Final.NewRow();
                            
                            //  para el tipo de la nomina
                            Dt_Row["TIPO_NOMINA"] = Dt_Row_Vacaciones[Cat_Nom_Tipos_Nominas.Campo_Nomina].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Vacaciones[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString()))
                                Nomina = Convert.ToDouble(Dt_Row_Vacaciones[Cat_Nom_Tipos_Nominas.Campo_Tipo_Nomina_ID].ToString());
                            
                            Dt_Row["CLAVE_NOMINA"] = "" + Nomina;
                            //  para la dependencia
                            Dt_Row["NOMBRE_DEPENDENCIA"] = Dt_Row_Vacaciones["NOMBRE_DEPENDENCIA"].ToString();
                            Dt_Row["CLAVE_DEPENDENCIA"] = Dt_Row_Vacaciones["Clave_Dependencia"].ToString();
                            //  para los datos del empleado
                            Dt_Row["NOMBRE_EMPLEADO"] = Dt_Row_Vacaciones["Nombre_Empleado"].ToString();
                            Dt_Row["CLAVE_EMPLEADO"] = Dt_Row_Vacaciones[Cat_Empleados.Campo_No_Empleado].ToString();

                            //  Para los datos de las vacaciones
                            Dt_Row["DIAS"] = Dt_Row_Vacaciones["Cantidad"].ToString();
                            
                            if(!String.IsNullOrEmpty(Dt_Row_Vacaciones[Cat_Empleados.Campo_Salario_Diario].ToString()))
                                Dt_Row["SALARIO_DIARIO"] =  Convert.ToDouble(Dt_Row_Vacaciones[Cat_Empleados.Campo_Salario_Diario].ToString());

                            //  para saber si tiene psm
                            if (Dt_Row_Vacaciones[Cat_Empleados.Campo_Aplica_ISSEG].ToString() == "SI")
                            {
                                //  se agregan el psm diario y el total de los dias de vacaciones con el psm
                                if (!String.IsNullOrEmpty(Dt_Row_Vacaciones["Psm_diario"].ToString()))
                                    Dt_Row["PSM"] = Convert.ToDouble(Dt_Row_Vacaciones["Psm_diario"].ToString());

                                if (!String.IsNullOrEmpty(Dt_Row_Vacaciones["Sueldo_Normal_Vacaciones"].ToString()))
                                    Dt_Row["TOTAL"] = Convert.ToDouble(Dt_Row_Vacaciones["Sueldo_Normal_Vacaciones"].ToString());
                            }

                            else
                            {
                                if (!String.IsNullOrEmpty(Dt_Row_Vacaciones["Sueldo_Normal_Vacaciones"].ToString()))
                                    Dt_Row["TOTAL"] = Convert.ToDouble(Dt_Row_Vacaciones["Sueldo_Normal_Vacaciones"].ToString());
                            }
                            
                            //  para las fechas
                            if (!String.IsNullOrEmpty(Dt_Row_Vacaciones[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()))
                                Dt_Row["FECHA_INICIO"] = Dt_Row_Vacaciones[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString();

                            if (!String.IsNullOrEmpty(Dt_Row_Vacaciones[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString()))
                                Dt_Row["FECHA_FIN"] = Dt_Row_Vacaciones[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString();

                            //  para el nombre de la persona que realiza el reporte
                            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;

                            //  se agrega la fila a la tabla  
                            Dt_Reporte_Final.Rows.Add(Dt_Row);
                        }
                    }
                }
            }

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte_Final.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Vacaciones.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            //  se genera el tipo del archivo y se muestra el reporte
            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            if (Formato == "PDF")
            {
                Nombre_Archivo += ".pdf";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
                Abrir_Ventana(Nombre_Archivo);
            }
            else if (Formato == "EXCEL")
            {
                Nombre_Archivo += ".xls";
                Ruta_Archivo = @Server.MapPath("../../Reporte/");
                m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

                ExportOptions Opciones_Exportacion = new ExportOptions();
                Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.Excel;
                Reporte.Export(Opciones_Exportacion);

                String Ruta = "../../Reporte/" + Nombre_Archivo;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Abrir_Ventana
    ///DESCRIPCIÓN: Abre en otra ventana el archivo pdf
    ///PARÁMETROS : Nombre_Archivo: Guarda el nombre del archivo que se desea abrir
    ///                             para mostrar los datos al usuario
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 21-Febrero-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private void Abrir_Ventana(String Nombre_Archivo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
        try
        {
            Pagina = Pagina + Nombre_Archivo;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
            "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    
    #endregion //   fin de reportes

    #region Operaciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla_Vacaciones
    ///DESCRIPCIÓN: Genera la tabla con las columnas que se necesitan para el reporte
    ///PARÁMETROS : 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 29-Marzo-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private DataTable Construir_Tabla_Vacaciones()
    {
        DataTable Dt_Reporte = new DataTable();
        try
        {
            //  para el tipo de la nomina
            Dt_Reporte.Columns.Add("TIPO_NOMINA", typeof(System.String));
            Dt_Reporte.Columns.Add("CLAVE_NOMINA", typeof(System.String));

            //  para la dependencia
            Dt_Reporte.Columns.Add("NOMBRE_DEPENDENCIA", typeof(System.String));
            Dt_Reporte.Columns.Add("CLAVE_DEPENDENCIA", typeof(System.String));

            //  para los datos del empleado
            Dt_Reporte.Columns.Add("NOMBRE_EMPLEADO", typeof(System.String));
            Dt_Reporte.Columns.Add("CLAVE_EMPLEADO", typeof(System.String));

            //  Para los datos de las vacaciones
            Dt_Reporte.Columns.Add("DIAS", typeof(System.Double));
            Dt_Reporte.Columns.Add("SALARIO_DIARIO", typeof(System.Double));
            Dt_Reporte.Columns.Add("PSM", typeof(System.Double));
            Dt_Reporte.Columns.Add("TOTAL", typeof(System.Double));

            //  Para las fechas
            Dt_Reporte.Columns.Add("FECHA_INICIO", typeof(System.DateTime));
            Dt_Reporte.Columns.Add("FECHA_FIN", typeof(System.DateTime));

            //  para el nombre de la persona que realiza el reporte
            Dt_Reporte.Columns.Add("ELABORO", typeof(System.String));

            Dt_Reporte.TableName = "Dt_Rpt_Vacaciones";
            return Dt_Reporte;
        }
        catch (Exception ex)
        {
            throw new Exception("Construir_Tabla_Vacaciones " + ex.Message.ToString(), ex);
        }
    }
    
    #endregion

    #endregion //  fin de metodos


    #region Eventos

    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Pdf_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Pdf_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Reporte())
            {
                Generar_Reporte("PDF");
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Reporte_Excel_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Reporte_Excel_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Validar_Reporte())
            {
                Generar_Reporte("EXCEL");
            }

        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }

    ///*********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Empleado_Click
    ///DESCRIPCIÓN          : Evento del boton de busqueda de empleados
    ///PROPIEDADES          :
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 24/Marzo/2012 
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Btn_Buscar_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Cat_Empleados_Negocios Rs_Consulta_Ca_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (!String.IsNullOrEmpty(Txt_Nombre_Empleado.Text))
            {
                if (!string.IsNullOrEmpty(Txt_Nombre_Empleado.Text.Trim()))
                {
                    Rs_Consulta_Ca_Empleados.P_Nombre = Txt_Nombre_Empleado.Text.Trim();
                }
                Rs_Consulta_Ca_Empleados.P_Estatus = "ACTIVO";
                Dt_Empleados = Rs_Consulta_Ca_Empleados.Consulta_Empleados_General();
                Cmb_Nombre_Empleado.DataSource = new DataTable();
                Cmb_Nombre_Empleado.DataBind();
                Cmb_Nombre_Empleado.DataSource = Dt_Empleados;
                Cmb_Nombre_Empleado.DataTextField = "Empleado";
                Cmb_Nombre_Empleado.DataValueField = Cat_Empleados.Campo_No_Empleado;
                Cmb_Nombre_Empleado.DataBind();
                Cmb_Nombre_Empleado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
                Cmb_Nombre_Empleado.SelectedIndex = -1;
            }
            else
            {
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Realizara los metodos requeridos para el reporte
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  13/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }
    }
    #endregion //   fin de botones


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
        Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador = new Cls_Cat_Nom_Calendario_Reloj_Checador_Negocio(); //Variable de conexión hacia la capa de negocios
        DataTable Dt_Periodo_Nominal; //Obtiene los valores de la consulta y servira para poder asignar estos a los controles correspondientes
        try
        {
            Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.ToString();
            Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
            Dt_Periodo_Nominal = Rs_Consulta_Cat_Nom_Calendario_Reloj_Checador.Consulta_Fechas_Calendario_Reloj_Checador(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

            //Asigna los valores obtenidos de la consulta a los controles correspondientes
            foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
            {
                if (!String.IsNullOrEmpty(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()))
                {
                    //Txt_Fecha_Inicial.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Inicio].ToString()));
                    //Txt_Fecha_Final.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Calendario_Reloj.Campo_Fecha_Fin].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Nombre_Empleado_OnSelectedIndexChanged
    ///DESCRIPCIÓN: carga la caja de texto de empleado con el numero de empleado
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO : 24/Marzo/2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Nombre_Empleado_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Nombre_Empleado.SelectedIndex > 0)
            {
                Txt_No_Empleado.Text = Cmb_Nombre_Empleado.SelectedValue;
            }
            else
            {
                Txt_No_Empleado.Text = "";
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
            throw new Exception(Ex.Message, Ex);
        }

    }   
    #endregion //   fin de combos

    #endregion  // fin de eventos
}
