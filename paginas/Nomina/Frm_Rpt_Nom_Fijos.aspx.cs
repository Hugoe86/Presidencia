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

public partial class paginas_Nomina_Frm_Rpt_Nom_Fijos : System.Web.UI.Page
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
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION :
    /// PARAMETROS  :
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 06/Abril/2011
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
    /// FECHA_CREO  : 22/Marzo/2012
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

            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Cmb_Tipo_Nomina.SelectedIndex = 0;

            Txt_Fecha_Inicial.Text = "";
            Txt_Fecha_Final.Text = "";
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
    /// FECHA_CREO  : 14/Marzo/2012
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

        //  validacion para cuando se selecciona algun numero de nomina o periodo
        if ( ((Txt_Fecha_Inicial.Text == "") && (Txt_Fecha_Final.Text == "")))
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
        else
        {
            if (Txt_Fecha_Inicial.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha inicial.<br>";
                Datos_Validos = false;
            }
            else if (Txt_Fecha_Inicial.Text.Length != 8)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La longitud de la fecha inicial debe de ser de 8 caracteres.<br>";
                Datos_Validos = false;
            }

            if (Txt_Fecha_Final.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la fecha final.<br>";
                Datos_Validos = false;
            }
            else if (Txt_Fecha_Final.Text.Length != 8)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La longitud de la fecha final debe de ser de 8 caracteres.<br>";
                Datos_Validos = false;
            }

            //  para validar la fecha introducida
            if ((Txt_Fecha_Inicial.Text != "") && (Txt_Fecha_Final.Text != ""))
            {
                if (Validar_Fechas())
                {
                    Datos_Validos = false;
                }
            }
        }

       
        
        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Fechas
    /// DESCRIPCION : Validar el rango de fechas introducidas por el usuario
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 24/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Fechas()
    {
        Boolean Estatus_Final = false;
        Boolean Estatus_Fecha_Inicial = false;
        Boolean Estatus_Fecha_final = false;
        DateTime Fecha_Inicial = new DateTime() ;
        DateTime Fecha_Final = new DateTime();
        String Fecha = "";
        String Fecha1 = "";
        String Fecha2 = "";
        String Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        try
        {
            Fecha_Inicial = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Inicial.Text.Trim());
            Fecha_Final = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Final.Text.Trim());

            if (Fecha_Inicial.CompareTo(Fecha_Final) == 1)
            {
                Estatus_Final = true;
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*La fecha final debe ser mayor que la inicial.<br>";
            }

            return Estatus_Final;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al Validar las Fechas . Error: [" + Ex.Message + "]");
        }
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
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario = new Cls_Cat_Nom_Calendario_Nominas_Negocio();
        DataTable Dt_Calendario_Nomina = null;

        try
        {
            Obj_Calendario.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Obj_Calendario.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedItem.Text);

            Dt_Calendario_Nomina = Obj_Calendario.Consulta_Periodos_Nomina();

            var periodos = from periodo in Dt_Calendario_Nomina.AsEnumerable()
                           select new
                           {
                               Fecha_Inicia = periodo.Field<DateTime>(Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio),
                               Fecha_Fin = periodo.Field<DateTime>(Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin)
                           };

            foreach (var fecha in periodos)
            {
                Txt_Fecha_Inicial.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(fecha.Fecha_Inicia));
                Txt_Fecha_Final.Text = String.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(fecha.Fecha_Fin));
            }

        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
        }
    }
    protected void Consultar_Percepciones_Fijas()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Obj_Percepciones_Fijas =
            new Cls_Cat_Nom_Percepciones_Deducciones_Business();
        DataTable Dt_Percepciones_Fijas = null;

        try
        {
            Obj_Percepciones_Fijas.P_TIPO = RBtn_Tipo_Percepcion_Deduccion.SelectedItem.Text.Trim();
            Obj_Percepciones_Fijas.P_ESTATUS = "ACTIVO";
            Obj_Percepciones_Fijas.P_TIPO_ASIGNACION = "FIJA";
            Dt_Percepciones_Fijas = Obj_Percepciones_Fijas.Consultar_Percepciones_Deducciones_General();

            Dt_Percepciones_Fijas = Juntar_Clave_Percepcion_Deduccion(Dt_Percepciones_Fijas);
            Cmb_Percepciones.DataSource = Dt_Percepciones_Fijas;
            Cmb_Percepciones.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
            Cmb_Percepciones.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Percepciones.DataBind();

            Cmb_Percepciones.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

            Cmb_Percepciones.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las percepciones fijas del empleado. Error: [" + Ex.Message + "]");
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
    #endregion

    #region Formato y carga
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
    /// NOMBRE DE LA FUNCION: Juntar_Clave_Percepcion_Deduccion
    /// 
    /// DESCRIPCION : Junta la clave de la percepcion y deduccion con el nombre.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 07/Julio/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Juntar_Clave_Percepcion_Deduccion(DataTable Dt_Percepciones_Deducciones)
    {
        try
        {
            if (Dt_Percepciones_Deducciones is DataTable)
            {
                if (Dt_Percepciones_Deducciones.Rows.Count > 0)
                {
                    foreach (DataRow PERCEPCION_DEDUCCION in Dt_Percepciones_Deducciones.Rows)
                    {
                        PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre] =
                            PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave] + " -- " +
                            PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Nombre];
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al juntar el nombre de la percepcion deduccion con la clave. Error: [" + Ex.Message + "]");
        }
        return Dt_Percepciones_Deducciones;
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
    #endregion


    #region Operaciones



    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Percepciones
    /// DESCRIPCION : Consulta las percepciones que tenga el empleado
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 27/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Percepciones(String Formato)
    {
        Ds_Rpt_Nom_Deducciones_Fijas Ds_Reporte = new Ds_Rpt_Nom_Deducciones_Fijas();
        Cls_Rpt_Nom_Fijos_Negocio Rs_Percepciones = new Cls_Rpt_Nom_Fijos_Negocio();
        DataTable Dt_Reporte = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataTable Dt_Elaboro = new DataTable();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Percepciones_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  filtrar por numero de empleado
            if (Txt_No_Empleado.Text != "")
                Rs_Percepciones.P_No_Empleado = Txt_No_Empleado.Text;

            //  filtrar por clave de deduccion  
            if (Cmb_Percepciones.SelectedIndex > 0)
                Rs_Percepciones.P_Clave_Deduccion = Cmb_Percepciones.SelectedValue;

            //  filtrar por dependencia  
            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                Rs_Percepciones.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;

            //  filtrar por tipo de nomina  
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Rs_Percepciones.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;

            //  filtro normal
            //  para las fechas o periodo
            Rs_Percepciones.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;
            Rs_Percepciones.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;
            //  filtro de tipo de percepcion
            Rs_Percepciones.P_Tipo = "PERCEPCION";
            Rs_Percepciones.P_Tipo_Asignacion = "FIJA";
            //  se realiza la consulta
            Dt_Reporte = Rs_Percepciones.Consultar_Deducciones_Fijas();
            Dt_Reporte.TableName = "Dt_Deducciones_Fijas";

            //  para la tabla de elaboro
            Dt_Elaboro = Construir_Tabla_Elaboro("PERCEPCIONES FIJAS");

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
            Ds_Reporte.Tables.Add(Dt_Elaboro.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Deducciones_Fijas.rpt");
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
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Deducciones_Variables
    /// DESCRIPCION : Consulta las deducciones fijas que tenga
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 26/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Deducciones_Variables(String Formato)
    {
        Ds_Rpt_Nom_Deducciones_Fijas Ds_Reporte = new Ds_Rpt_Nom_Deducciones_Fijas();
        Cls_Rpt_Nom_Fijos_Negocio Rs_Deducciones_Variables = new Cls_Rpt_Nom_Fijos_Negocio();
        DataTable Dt_Reporte = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataTable Dt_Elaboro = new DataTable();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Deducciones_Fijas_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  filtrar por numero de empleado
            if (Txt_No_Empleado.Text != "")
                Rs_Deducciones_Variables.P_No_Empleado = Txt_No_Empleado.Text;

            //  filtrar por clave de deduccion  
            if (Cmb_Percepciones.SelectedIndex > 0)
                Rs_Deducciones_Variables.P_Clave_Deduccion = Cmb_Percepciones.SelectedValue;

            //  filtrar por dependencia  
            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                Rs_Deducciones_Variables.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;

            //  filtrar por tipo de nomina  
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Rs_Deducciones_Variables.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;

            //  filtro normal
            Rs_Deducciones_Variables.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;
            Rs_Deducciones_Variables.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;
            Dt_Reporte = Rs_Deducciones_Variables.Consultar_Deducciones_Variables();
            Dt_Reporte.TableName = "Dt_Deducciones_Fijas";

            //  para la tabla de elaboro
            Dt_Elaboro = Construir_Tabla_Elaboro("DEDUCCIONES VARIABLES");

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
            Ds_Reporte.Tables.Add(Dt_Elaboro.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Deducciones_Fijas.rpt");
            Reporte.SetDataSource(Ds_Reporte);

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
    /// NOMBRE DE LA FUNCION: Generar_Reporte_Deducciones_Fijas
    /// DESCRIPCION : Consulta las deducciones fijas que tenga
    /// PARAMETROS  : String Formato.- Para saber que formato sera el archivo pdf, excel
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 21/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Generar_Reporte_Deducciones_Fijas(String Formato)
    {
        Ds_Rpt_Nom_Deducciones_Fijas Ds_Reporte = new Ds_Rpt_Nom_Deducciones_Fijas();
        Cls_Rpt_Nom_Fijos_Negocio Rs_Deducciones_Fijas = new Cls_Rpt_Nom_Fijos_Negocio();
        DataTable Dt_Reporte = new DataTable();
        ReportDocument Reporte = new ReportDocument();
        DataTable Dt_Elaboro = new DataTable();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Deducciones_Fijas_" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            //  filtrar por numero de empleado
            if (Txt_No_Empleado.Text != "")
                Rs_Deducciones_Fijas.P_No_Empleado = Txt_No_Empleado.Text;

            //  filtrar por clave de deduccion  
            if (Cmb_Percepciones.SelectedIndex > 0)
                Rs_Deducciones_Fijas.P_Clave_Deduccion = Cmb_Percepciones.SelectedValue;

            //  filtrar por dependencia  
            if (Cmb_Unidad_Responsable.SelectedIndex > 0)
                Rs_Deducciones_Fijas.P_Dependencia_ID = Cmb_Unidad_Responsable.SelectedValue;

            //  filtrar por tipo de nomina  
            if (Cmb_Tipo_Nomina.SelectedIndex > 0)
                Rs_Deducciones_Fijas.P_Tipo_Nomina_ID = Cmb_Tipo_Nomina.SelectedValue;

            //  filtro normal
            //  filtro de fechas
            Rs_Deducciones_Fijas.P_Nomina_id = Cmb_Calendario_Nomina.SelectedValue;
            Rs_Deducciones_Fijas.P_No_Nomina = Cmb_Periodos_Catorcenales_Nomina.SelectedValue;
            Rs_Deducciones_Fijas.P_Tipo = "DEDUCCION";
            //  se realiza la consulta
            Dt_Reporte = Rs_Deducciones_Fijas.Consultar_Deducciones_Fijas();
            Dt_Reporte.TableName = "Dt_Deducciones_Fijas";

            //  para la tabla de elaboro
            Dt_Elaboro = Construir_Tabla_Elaboro("DEDUCCIONES FIJAS");

            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());
            Ds_Reporte.Tables.Add(Dt_Elaboro.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Deducciones_Fijas.rpt");
            Reporte.SetDataSource(Ds_Reporte);

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
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600');", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Diario_General " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Construir_Tabla_Elaboro
    ///DESCRIPCIÓN: Genera la tabla con las columnas que se necesitan para el reporte
    ///PARÁMETROS : 
    ///CREO       : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO  : 26-Marzo-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************
    private DataTable Construir_Tabla_Elaboro(String Tipo_Reporte)
    {
        DataTable Dt_Reporte = new DataTable();
        DataRow Dt_Row;
        try
        {
            Dt_Reporte.Columns.Add("ELABORO", typeof(System.String));
            Dt_Reporte.Columns.Add("TIPO_REPORTE", typeof(System.String));
            Dt_Reporte.Columns.Add("COLUMNA_ADEUDO", typeof(System.String));
            Dt_Reporte.Columns.Add("COLUMNA_IMPORTE", typeof(System.String));
            Dt_Reporte.Columns.Add("COLUMNA_SALDO", typeof(System.String));
            Dt_Reporte.TableName = "Dt_Elaboro";


            Dt_Row = Dt_Reporte.NewRow();

            if (Tipo_Reporte == "DEDUCCIONES FIJAS")
            {
               
                Dt_Row["COLUMNA_ADEUDO"] = "ADEUDO";
                Dt_Row["COLUMNA_IMPORTE"] = "IMPORTE";
                Dt_Row["COLUMNA_SALDO"] = "SALDO";

            }
            else
            {
                Dt_Row["COLUMNA_ADEUDO"] = "";
                Dt_Row["COLUMNA_IMPORTE"] = "IMPORTE";
                Dt_Row["COLUMNA_SALDO"] = "";
            }
 
            Dt_Row["TIPO_REPORTE"] = Tipo_Reporte;
            Dt_Row["ELABORO"] = Cls_Sessiones.Nombre_Empleado;
            Dt_Reporte.Rows.Add(Dt_Row);

            return Dt_Reporte;
        }
        catch (Exception ex)
        {
            throw new Exception("Abrir_Ventana " + ex.Message.ToString(), ex);
        }
    }
    #endregion
    #endregion


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

            String Tipo_nomina = Cmb_Tipo_Nomina.SelectedValue;

            if (Validar_Reporte())
            {
                if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE DEDUCCIONES (FIJAS)")
                {
                    Generar_Reporte_Deducciones_Fijas("PDF");
                }

                //if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE DEDUCCIONES (VARIABLES)")
                //{
                //    Generar_Reporte_Deducciones_Variables("PDF");
                //}

                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE PERCEPCIONES (FIJAS)")
                {
                    Generar_Reporte_Percepciones("PDF");
                }
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
                if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE DEDUCCIONES (FIJAS)")
                {
                    Generar_Reporte_Deducciones_Fijas("EXCEL");
                }

                //if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE DEDUCCIONES (VARIABLES)")
                //{
                //    Generar_Reporte_Deducciones_Variables("EXCEL");
                //}

                else if (Cmb_Tipo_Reporte.SelectedValue == "REPORTE DE PERCEPCIONES (FIJAS)")
                {
                    Generar_Reporte_Percepciones("EXCEL");
                }
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
    #endregion

    #region Radio Button
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void RBtn_Tipo_Percepcion_Deduccion_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = RBtn_Tipo_Percepcion_Deduccion.SelectedIndex;

        if (index >= 0)
        {
            Consultar_Percepciones_Fijas();
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "javascript:refresh_tabla_empleados();", true);
        }
    }
    #endregion
    #endregion
}
