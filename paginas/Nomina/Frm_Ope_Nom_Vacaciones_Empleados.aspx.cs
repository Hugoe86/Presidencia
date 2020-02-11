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
using Presidencia.Vacaciones_Empleado.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using System.Globalization;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Empleados.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using Presidencia.Tipos_Nominas.Negocios;
using Presidencia.Dias_Festivos.Negocios;
using Presidencia.DateDiff;
using System.Collections.Generic;

public partial class paginas_Nomina_Frm_Nom_Vacaciones_Empleados : System.Web.UI.Page
{

    #region (Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            String onmouseoverStyle = "this.style.backgroundColor='#DFE8F6';this.style.cursor='hand';this.style.color='DarkBlue';" +
                "this.style.borderStyle='none';this.style.borderColor='Silver';";

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
            }

            Div_Mensajes_Dias_Vacaciones.Attributes.Add("onmouseout", onmouseoverStyle);
            Div_Mensajes_Dias_Vacaciones.Attributes.Add("onmouseover", "this.style.backgroundColor='#F0F8FF';this.style.color='gray';this.style.borderStyle='none';");

            if (Cls_Sessiones.Datos_Empleado != null)
            {
                if (Cls_Sessiones.Datos_Empleado.Rows.Count > 0)
                {
                    Txt_No_Empleado.Text = Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial de los controles del Formulario.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Consultar_Dependencias();//Consulta las dependencias que existen actualmente.
        Habilitar_Controles("Inicial");//Habilita la configuracion inicial de los controles
        Limpiar_Controles();//Limpia los controles de la pagina.
        Consultar_Calendarios_Nomina();//Consulta los calendarios de nómina que existen actualmente en el sistema.
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {            
            Txt_Dias_Vacaciones_Empleado.Text = "";
            Txt_No_Vacacion.Text = "";
            Cmb_Estatus.SelectedIndex = -1;
            Cmb_Dependencia.SelectedIndex = -1;
            Cmb_Empleado.SelectedIndex = -1;
            Txt_Fecha_Inicio_Vacaciones.Text = "";
            Txt_Fecha_Termino_Vacaciones.Text = "";
            Txt_Fecha_Regreso_Vacaciones.Text = String.Empty;
            Txt_Comentarios.Text = "";
            //Limpiar Combos
            Cmb_Empleado.DataSource = new DataTable();
            Cmb_Empleado.DataBind();

            //Cmb_Calendario_Nomina.SelectedIndex = -1;
            //Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al limpiar los controles de la pagina. Error: [" + Ex + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;//Variable que sirve para almacenar el estatus de los controles habilitado o deshabilitado.

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    //Mensajes de Error.
                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    Cmb_Estatus.Enabled = false;
                    Txt_No_Empleado.Text = "";
                    Txt_No_Empleado.Enabled = false;
                    //Boton que consulta los dias de vacaciones del empleado.
                    Btn_Consultar_Dias_Vacaciones.Enabled = false;
                    Div_Mensajes_Dias_Vacaciones.Visible = false;
                    Lbl_Dias_Vacaciones.Text = "";
                    Txt_No_Empleado.Text = "";

                    Configuracion_Acceso("Frm_Ope_Nom_Vacaciones_Empleados.aspx");
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    Cmb_Estatus.SelectedIndex = 1;
                    Cmb_Estatus.Enabled = false;
                    //Boton que consulta los dias de vacaciones del empleado.
                    Btn_Consultar_Dias_Vacaciones.Enabled = true;
                    Cmb_Calendario_Nomina.Focus();
                    break;
            }

            Txt_Dias_Vacaciones_Empleado.Enabled = false;
            Txt_No_Vacacion.Enabled = false;
            Cmb_Dependencia.Enabled = false;
            Cmb_Empleado.Enabled = true;
            Txt_Fecha_Inicio_Vacaciones.Enabled = Habilitado;
            Txt_Fecha_Termino_Vacaciones.Enabled = Habilitado;
            Txt_Fecha_Regreso_Vacaciones.Enabled = Habilitado;

            Txt_Comentarios.Enabled = Habilitado;
            Btn_Fecha_Inicio_Vacaciones.Enabled = Habilitado;
            Btn_Fecha_Termino_Vacaciones.Enabled = Habilitado;
            Btn_Fecha_Regreso_Vacaciones.Enabled = Habilitado;

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;

            Tr_Periodos_Fiscales.Visible = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Vacaciones_Empleado
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Vacaciones_Empleado()
    {
        Boolean Datos_Validos = true;//Variable que alamacenara el resultado de la validacion de los datos ingresados por el usuario.
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Fecha_Inicio_Vacaciones.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de Inicio de Vacaciones <br>";
            Datos_Validos = false;
        }
        //else if (Validar_Formato_Fecha(Txt_Fecha_Inicio_Vacaciones.Text.Trim()))
        //{
        //    DateTime Hoy = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0);
        //    DateTime Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicio_Vacaciones.Text.Trim());

        //    if ((DateTime.Compare(Fecha_Inicial, Hoy) < 0) || (DateTime.Compare(Fecha_Inicial, Hoy) == 0))
        //    {
        //        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha de Inicio no puede ser menor al dia de hoy.<br>";
        //        Datos_Validos = false;
        //    }
        //}

        if (string.IsNullOrEmpty(Txt_Fecha_Termino_Vacaciones.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Termino de vacaciones <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Regreso_Vacaciones.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Regreso de vacaciones <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Dias_Vacaciones_Empleado.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Ingrese los dias que desea tomar de vacaciones.  <br>";
            Datos_Validos = false;
        }
        else if (Convert.ToInt32(Txt_Dias_Vacaciones_Empleado.Text.Trim()) > Convert.ToInt32(Lbl_Dias_Vacaciones.Text.Trim()) || Convert.ToInt32(Txt_Dias_Vacaciones_Empleado.Text.Trim()) <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Los dias tomados no pueden ser mayor a lo que ley tiene derecho.  <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Selecciones el Estatus <br>";
            Datos_Validos = false;
        }

        if (Cmb_Dependencia.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Selecciones la Dependencia <br>";
            Datos_Validos = false;
        }

        if (Cmb_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Selecciones el Empleado <br>";
            Datos_Validos = false;
        }

        if (Validar_Solicitudes_Vacaciones_Pendientes(Txt_No_Empleado.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Empleado actualmente cuenta con una solicitud en proceso de autorización. <br>";
            Datos_Validos = false;
        }

        //if (Validar_Solicitudes_Autorizadas_Con_Dias_Pendientes_Tomar(Txt_No_Empleado.Text.Trim()))
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + El Empleado actualmente cuenta con una solicitud autorizada con dias pendientes por tomar. <br>";
        //    Datos_Validos = false;
        //}

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Anios
    /// DESCRIPCION : Valida las fechas de busqueda, tanto inicial como final
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Fechas(String Fecha_Inicio, String Fecha_Fin)
    {
        Boolean Valida = true;
        try
        {
            //Se valida que las fecha ingresadas por el usuario no sean nulas
            if (!string.IsNullOrEmpty(Fecha_Inicio) && !string.IsNullOrEmpty(Fecha_Fin))
            {
                DateTime _Fecha_Inicio = DateTime.ParseExact(Fecha_Inicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);//Se establece un formato adecuado a las fecha inicio.
                DateTime _Fecha_Fin = DateTime.ParseExact(Fecha_Fin, "dd/MM/yyyy", CultureInfo.InvariantCulture);//Se estable un formato correcto a la fecha fin
                //Se realiza la comparacion de fecha de inicio y fin
                //La validacion es correcta si la fecha de inicio siempre sea menor que la final.
                if (DateTime.Compare(_Fecha_Inicio, _Fecha_Fin) > 0)
                {
                    Valida = false;
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Al realizar la busqueda la Fecha Inicial no puede ser mayor que la final";
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido por ingresar una fecha con formato incorrecto. Error: [" + Ex.Message + "]");
        }
        return Valida;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Formato_Fecha
    /// DESCRIPCION : Valida el formato de las fechas.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Formato_Fecha(String Fecha)
    {
        String Cadena_Fecha = @"^(([0-9])|([0-2][0-9])|([3][0-1]))\/(ene|feb|mar|abr|may|jun|jul|ago|sep|oct|nov|dic)\/\d{4}$";
        if (Fecha != null) return Regex.IsMatch(Fecha, Cadena_Fecha);
        else return false;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Calcular_Fecha_Termino_Vacaciones
    /// DESCRIPCION : Calcula la fecha de termino de las vacaciones del empleado
    /// dependiendo de la fecha de inicio seleccionada, sus dias laborales de la semana y
    /// y los dias solicitados.
    /// PARAMETROS: Fecha_Termino_Vacaciones.- Fecha inicial de las vacaciones que al final se
    ///                                        convertira en la fecha de termino de las mismas.
    ///             Dias_Solicitados.- No de dias de vacaciones solicitados por el empleado.
    ///             
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************  
    private DateTime Calcular_Fecha_Termino_Vacaciones(DateTime Fecha_Termino_Vacaciones, Int32 Dias_Solicitados)
    {
        Cls_Cat_Empleados_Negocios Cat_Empleados_Consulta = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados consultados.
        Boolean LUNES = false;//Dia Lunes
        Boolean MARTES = false;//Dia Martes
        Boolean MIERCOLES = false;//Dia Miercoles
        Boolean JUEVES = false;//Dia Jueves
        Boolean VIERNES = false;//Dia Viernes
        Boolean SABADO = false;//Dia Sabado
        Boolean DOMINGO = false;//Dia Domingo
        Boolean[] Dias_Laborales_Semana = new Boolean[7];//Varuable que almacenara la configuracion laboral de los dias de la semana para el empleado.

        try
        {
            Cat_Empleados_Consulta.P_No_Empleado = Txt_No_Empleado.Text;//Establecemos el no de empleado a consultar.
            Dt_Empleados = Cat_Empleados_Consulta.Consulta_Datos_Empleado();//Ejecutamos la consulta de los empleados.
            //Validamos que la consulta halla encontrado resultados.
            if (Dt_Empleados != null)
            {
                //Vallidamos que por lo menos exista un registro.
                if (Dt_Empleados.Rows.Count > 0)
                {
                    //Obtenemos la configuracion laboral de los dias que si labora el empleado por semana.
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString())) LUNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString())) MARTES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString())) MIERCOLES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString())) JUEVES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString())) VIERNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString())) SABADO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString())) DOMINGO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                }
            }
            //Pasamos la configuracion laboral a la variable que los almacenara.
            Dias_Laborales_Semana[0] = LUNES;
            Dias_Laborales_Semana[1] = MARTES;
            Dias_Laborales_Semana[2] = MIERCOLES;
            Dias_Laborales_Semana[3] = JUEVES;
            Dias_Laborales_Semana[4] = VIERNES;
            Dias_Laborales_Semana[5] = SABADO;
            Dias_Laborales_Semana[6] = DOMINGO;
            //Se realiza un barrido teniendo como fin del ciclo el numero de dia de vacaciones que solicito el empleado.
            for (int index = 1; index <= Dias_Solicitados; index++)
            {
                //verificamos si es un dia laboral para el empleado.
                if (Verifica_Dia_Laboral_Empleado(Fecha_Termino_Vacaciones.DayOfWeek, Dias_Laborales_Semana) && !No_Es_Un_Dia_Festivo(Fecha_Termino_Vacaciones))
                {
                    //Verificamos que no sea el ultimo dia de vacaciones. si es el ultimo dia de vacaciones,
                    //no se realiza el incremente del dia y se toma como fecha termino de vacaciones.
                    //Si no es el ultimo se agrega un dia a la fecha y se continua con el proceso.
                    if (index != Dias_Solicitados)
                    {
                        Fecha_Termino_Vacaciones = Fecha_Termino_Vacaciones.AddDays(1);
                    }
                }
                else
                {
                    //Si no es un dia laboral para el empleado. 
                    //Se incrementa el dia a la fecha pero el dia no se cuenta como dia de vacacion para el empleado.
                    Fecha_Termino_Vacaciones = Fecha_Termino_Vacaciones.AddDays(1);
                    --index;//Decrementamos el indice para no contarlo como dia vacacional.
                }
            }

            Boolean Ctrl_Iteraciones = true;
            DateTime Fecha_Regreso_Laboral = Fecha_Termino_Vacaciones;

            while (Ctrl_Iteraciones)
            {
                Fecha_Regreso_Laboral = Fecha_Regreso_Laboral.AddDays(1);

                //verificamos si es un dia laboral para el empleado.
                if (Verifica_Dia_Laboral_Empleado(Fecha_Regreso_Laboral.DayOfWeek, Dias_Laborales_Semana) && !No_Es_Un_Dia_Festivo(Fecha_Regreso_Laboral))
                {
                    Txt_Fecha_Regreso_Vacaciones.Text = String.Format("{0:dd/MMM/yyyy}", Fecha_Regreso_Laboral);
                    Ctrl_Iteraciones = false;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Fecha_Termino_Vacaciones;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Verifica_Dia_Laboral_Empleado
    /// DESCRIPCION : Verifica si el dia de la semana es un dia laboral para el empleado.
    /// PARAMETROS:  DayOfWeek Dia.- Dia de la semana a evaluar.
    ///              Dias_Laborales_Semana.- Programacion de dias laborales del empleado.
    ///              
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///******************************************************************************* 
    private Boolean Verifica_Dia_Laboral_Empleado(DayOfWeek Dia, Boolean[] Dias_Laborales_Semana)
    {
        Boolean Resultado = false;//Variable que almacenara el valor si es o no un dia laboral para el empleado.
        //Se realiza la validacion del dia laboral del empleado.
        switch (Dia)
        {
            case DayOfWeek.Monday:
                if (Dias_Laborales_Semana[0]) Resultado = true;
                break;
            case DayOfWeek.Tuesday:
                if (Dias_Laborales_Semana[1]) Resultado = true;
                break;
            case DayOfWeek.Wednesday:
                if (Dias_Laborales_Semana[2]) Resultado = true;
                break;
            case DayOfWeek.Thursday:
                if (Dias_Laborales_Semana[3]) Resultado = true;
                break;
            case DayOfWeek.Friday:
                if (Dias_Laborales_Semana[4]) Resultado = true;
                break;
            case DayOfWeek.Saturday:
                if (Dias_Laborales_Semana[5]) Resultado = true;
                break;
            case DayOfWeek.Sunday:
                if (Dias_Laborales_Semana[6]) Resultado = true;
                break;
            default:
                Resultado = false;
                break;
        }
        return Resultado;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Calendarios_Nomina
    /// DESCRIPCION : 
    /// 
    /// PARAMETROS:
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 21/Febrero/2011
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
    /// NOMBRE DE LA FUNCION: Formato_Fecha_Calendario_Nomina
    /// DESCRIPCION : Crea el DataTable con la consulta de las nomina vigentes en el 
    /// sistema.
    /// PARAMETROS: Dt_Calendario_Nominas.- Lista de las nominas vigentes actualmente 
    ///             en el sistema.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 01/Diciembre/2010
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
    ///FECHA_CREO: 01/Diciembre/2010
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Periodos_Pago
    ///DESCRIPCIÓN: Valida que el empleado solo puedan comenzar a descontar la deduccion 
    ///a partir del periodo actual.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 06/Diciembre/2010
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
    ///***************************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_Tabla_Detalles_Vacaciones
    /// 
    /// DESCRIPCION : Crea una tabla con  los detalles de los dias de vacaciones que tomara
    ///               el empleado. De acuerdo a la fecha deinicio y de fin.
    /// 
    /// PARÁMETROS: Fecha_Inicia_Vacaciones.- Fecha en que se establece como inicio de las vacaciones.
    ///             Fecha_Termino_Vacaciones.- fecha que se establece como inicio a finalizar las vacaciones.
    ///             
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Febrero/2011
    /// MODIFICO          : Juan Alberto Hernández Negrete.
    /// FECHA_MODIFICO    : 26/Noviembre/2011
    /// CAUSA_MODIFICACION: Se válido para que si entre la fecha inicio y fin de las vacaciones hay un dia festivo no se tome en cuenta al crear el registro de detalle.
    ///***************************************************************************************************************************************************************
    private DataTable Crear_Tabla_Detalles_Vacaciones(String Fecha_Inicia_Vacaciones, String Fecha_Termino_Vacaciones)
    {
        DataTable Dt_Detalles_Vacaciones = new DataTable("VACACIONES_DIA_DETALLE");//Variable que almacenara los detalles de los dias de  vacaciones del empleado.
        DataRow Registro_Detalle_Vacacion = null;//Variable que almacenara un registro o detalle de la vacación.
        DateTime? Fecha_Inicio = null;//Fecha de inicio de las vacaciones del empleado.
        DateTime? Fecha_Fin = null;//Fecha de fin de las vacaciones del empleado.
        DateTime? Fecha_Temporal = null;//Variable que almacena las fechas temporales.
        DateTimeFormatInfo Formato = new DateTimeFormatInfo();//Formato que recibira la fecha al convertila en DateTime.
        Boolean[] Dias_Laborales = null;

        try
        {
            Dias_Laborales = Obtener_Dias_Laborales_Empleado();

            //Creamos la estructura de la tabla de vacaciones del empelado.
            Dt_Detalles_Vacaciones.Columns.Add(Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha, typeof(String));//Fecha el día de vacación a tomar.
            Dt_Detalles_Vacaciones.Columns.Add(Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus, typeof(String));//Estatus del día a tomar de vacaciones.
            Dt_Detalles_Vacaciones.Columns.Add(Ope_Nom_Vacaciones_Dia_Det.Campo_Estado, typeof(String));//Estado del día a tomar de vacciones.

            if (!string.IsNullOrEmpty(Fecha_Inicia_Vacaciones) && !string.IsNullOrEmpty(Fecha_Termino_Vacaciones))
            {
                Formato.ShortDatePattern = "dd/MM/yyyy";//Establecemos el formato con el que el metodo Convert.To_Date_time leera la cadena que contiene las fechas.
                Fecha_Inicio = Convert.ToDateTime(Fecha_Inicia_Vacaciones, Formato);//Obtenemos el objeto DateTime de la fecha de inicio. 
                Fecha_Fin = Convert.ToDateTime(Fecha_Termino_Vacaciones, Formato);//Obtenemos el objeto DateTime de la fecha de final.

                Fecha_Temporal = Fecha_Inicio;//Establecemos el valor de la variable temporal, esto para no afectar el valor de la fecha inicio.
                while ((((DateTime)Fecha_Temporal) >= ((DateTime)Fecha_Inicio)) && (((DateTime)Fecha_Temporal) <= ((DateTime)Fecha_Fin)))
                {
                    //Validamos que se trate de un día labora del empleado.
                    if (Verifica_Dia_Laboral_Empleado(((DateTime)Fecha_Temporal).DayOfWeek, Dias_Laborales) && !No_Es_Un_Dia_Festivo(((DateTime)Fecha_Temporal)))
                    {
                        //Si es un día laboral generamos el registro del dia de vacación.
                        Registro_Detalle_Vacacion = Dt_Detalles_Vacaciones.NewRow();
                        Registro_Detalle_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Fecha] = string.Format("{0:dd/MM/yyyy}", ((DateTime)Fecha_Temporal));
                        Registro_Detalle_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Estatus] = "Pendiente";
                        Registro_Detalle_Vacacion[Ope_Nom_Vacaciones_Dia_Det.Campo_Estado] = "Pendiente";
                        Dt_Detalles_Vacaciones.Rows.Add(Registro_Detalle_Vacacion);
                    }
                    //Incrementamos en un día la fecha temporal, que nos ayuda a recorrer los dias de vacaciones que tomara el empleado.
                    Fecha_Temporal = ((DateTime)Fecha_Temporal).AddDays(1);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al crear la tabla de detalles de las vacaciones. Error: [" + Ex.Message + "]");
        }
        return Dt_Detalles_Vacaciones;
    }
    ///***************************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Dias_Laborales_Empleado
    /// 
    /// DESCRIPCION : Obtiene un arreglo con la configuración de los días laborales del empleado.
    ///             
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Febrero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************************
    private Boolean[] Obtener_Dias_Laborales_Empleado()
    {
        Cls_Cat_Empleados_Negocios Cat_Empleados_Consulta = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados consultados.
        Boolean LUNES = false;//Dia Lunes
        Boolean MARTES = false;//Dia Martes
        Boolean MIERCOLES = false;//Dia Miercoles
        Boolean JUEVES = false;//Dia Jueves
        Boolean VIERNES = false;//Dia Viernes
        Boolean SABADO = false;//Dia Sabado
        Boolean DOMINGO = false;//Dia Domingo
        Boolean[] Dias_Laborales_Semana = new Boolean[7];//Varuable que almacenara la configuracion laboral de los dias de la semana para el empleado.

        try
        {
            Cat_Empleados_Consulta.P_No_Empleado = Txt_No_Empleado.Text;//Establecemos el no de empleado a consultar.
            Dt_Empleados = Cat_Empleados_Consulta.Consulta_Empleados_General();//Ejecutamos la consulta de los empleados.
            //Validamos que la consulta halla encontrado resultados.
            if (Dt_Empleados != null)
            {
                //Vallidamos que por lo menos exista un registro.
                if (Dt_Empleados.Rows.Count > 0)
                {
                    //Obtenemos la configuracion laboral de los dias que si labora el empleado por semana.
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString())) LUNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Lunes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString())) MARTES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Martes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString())) MIERCOLES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Miercoles].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString())) JUEVES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Jueves].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString())) VIERNES = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Viernes].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString())) SABADO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Sabado].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                    if (!string.IsNullOrEmpty(Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString())) DOMINGO = (Dt_Empleados.Rows[0][Cat_Empleados.Campo_Domingo].ToString().Trim().ToUpper().Equals("SI")) ? true : false;
                }
            }
            //Pasamos la configuracion laboral a la variable que los almacenara.
            Dias_Laborales_Semana[0] = LUNES;
            Dias_Laborales_Semana[1] = MARTES;
            Dias_Laborales_Semana[2] = MIERCOLES;
            Dias_Laborales_Semana[3] = JUEVES;
            Dias_Laborales_Semana[4] = VIERNES;
            Dias_Laborales_Semana[5] = SABADO;
            Dias_Laborales_Semana[6] = DOMINGO;

        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al obtener los dias laborales del empleado. Error: [" + Ex.Message + "]");
        }
        return Dias_Laborales_Semana;
    }
    ///***************************************************************************************************************************************************************
    /// NOMBRE DE LA FUNCION: No_Es_Un_Dia_Festivo
    /// 
    /// DESCRIPCION : Consulta los dias festivos que se encuentran dados de alta en el sistema
    ///             
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 08/Abril/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///***************************************************************************************************************************************************************
    public Boolean No_Es_Un_Dia_Festivo(DateTime Dia)
    {
        Cls_Tab_Nom_Dias_Festivos_Negocios Obj_Tabulador_Dias_Festivos = new Cls_Tab_Nom_Dias_Festivos_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Tabulador_Dias_Festivos = null;//Variable que almacenara la lista de dias festivos registrados en el sistema para la nomina seleccionada.
        DateTime? Fecha_Dia_Festivo = null;//Variable que almacenara la fecha del dia festivo.
        Boolean Estatus = false;//Variable que almacenara [true/false]. True si el dia corresponde a un dia festivo. False si no es un dia festivo.

        try
        {
            Obj_Tabulador_Dias_Festivos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Dt_Tabulador_Dias_Festivos = Obj_Tabulador_Dias_Festivos.Consulta_Datos_Dia_Festivo();

            if (Dt_Tabulador_Dias_Festivos is DataTable)
            {
                if (Dt_Tabulador_Dias_Festivos.Rows.Count > 0)
                {
                    foreach (DataRow Renglon in Dt_Tabulador_Dias_Festivos.Rows)
                    {
                        if (Renglon is DataRow)
                        {
                            if (!string.IsNullOrEmpty(Renglon[Tab_Nom_Dias_Festivos.Campo_Fecha].ToString().Trim()))
                            {
                                Fecha_Dia_Festivo = Convert.ToDateTime(Renglon[Tab_Nom_Dias_Festivos.Campo_Fecha].ToString().Trim());

                                if (DateTime.Compare(((DateTime)Fecha_Dia_Festivo), Dia) == 0)
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
            throw new Exception("Error al consultar si el dia no corresponde a un dia festivo. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Solicitudes_Vacaciones_Pendientes
    ///
    ///DESCRIPCIÓN: Consulta si el empleado tiene una solicitud vigente actualmente.
    ///
    /// PARÁMETROS: Empelado_ID.- Identificador del empleado.
    /// 
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 09/Abril/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Solicitudes_Vacaciones_Pendientes(String No_Empleado)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión a la capa de negocios.
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Vacaciones_Empleado = null;//Variable que almacena el registro si el empleado tiene alguna soliciytud actualmente de vacaciones.
        DataTable DT_Empleados = null;//Variable que almacena la informacion del empleado.
        Boolean Estatus = false;//Variable que almacena el estatus. [true/false] True si ele mpleado tiene alguna solictud pendiente de autorizar actualmente o  False en caso contrario.
        String Empleado_ID = String.Empty;//Variable que almacena el identificador del empleado.

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            DT_Empleados = Obj_Empleados.Consulta_Empleados_General();

            if (DT_Empleados is DataTable)
            {
                if (DT_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow Empleado in DT_Empleados.Rows)
                    {
                        if (Empleado is DataRow)
                        {
                            if (!string.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString()))
                            {
                                Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();

                                Obj_Vacaciones.P_Empleado_ID = Empleado_ID;
                                Obj_Vacaciones.P_Estatus = "Pendiente";
                                Dt_Vacaciones_Empleado = Obj_Vacaciones.Consultar_Vacaciones().P_Dt_Vacaciones;

                                if (Dt_Vacaciones_Empleado is DataTable)
                                {
                                    if (Dt_Vacaciones_Empleado.Rows.Count > 0)
                                    {
                                        Estatus = true;
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
            throw new Exception("Error al consultar si el empleado tiene alguna solicitud de vacaciones actualmente pendiente de autorizar. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }

    private Boolean Validar_Solicitudes_Autorizadas_Con_Dias_Pendientes_Tomar(String No_Empleado)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión a la capa de negocios.
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        DataTable Dt_Vacaciones_Empleado = null;//Variable que almacena el registro si el empleado tiene alguna soliciytud actualmente de vacaciones.
        DataTable DT_Empleados = null;//Variable que almacena la informacion del empleado.
        Boolean Estatus = false;//Variable que almacena el estatus. [true/false] True si ele mpleado tiene alguna solictud pendiente de autorizar actualmente o  False en caso contrario.
        String Empleado_ID = String.Empty;//Variable que almacena el identificador del empleado.

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
            DT_Empleados = Obj_Empleados.Consulta_Empleados_General();

            if (DT_Empleados is DataTable)
            {
                if (DT_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow Empleado in DT_Empleados.Rows)
                    {
                        if (Empleado is DataRow)
                        {
                            if (!string.IsNullOrEmpty(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString()))
                            {
                                Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();

                                Obj_Vacaciones.P_Empleado_ID = Empleado_ID;
                                Dt_Vacaciones_Empleado = Obj_Vacaciones.Consultar_Vacaciones_Autorizadas_Con_Dias_Pendientes_Tomar();

                                if (Dt_Vacaciones_Empleado is DataTable)
                                {
                                    if (Dt_Vacaciones_Empleado.Rows.Count > 0)
                                    {
                                        Estatus = true;
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
            throw new Exception("Error al consultar si hay solicitudes autorizadas con dias pendientes por tomar. Error: [" + Ex.Message + "]");
        }
        return Estatus;
    }
    #endregion

    #region (Metodos de Operacion)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Vacaciones_Empleado
    /// DESCRIPCION : Ejecuta el alta de las vacaciones de un empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Vacaciones_Empleado()
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Alta_Vacaciones_Empleado = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocio.
        try
        {
            Alta_Vacaciones_Empleado.P_Dependencia_ID = Cmb_Dependencia.SelectedValue.Trim();
            Alta_Vacaciones_Empleado.P_Empleado_ID = Cmb_Empleado.SelectedValue.Trim();
            Alta_Vacaciones_Empleado.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Inicio_Vacaciones.Text.Trim()));
            Alta_Vacaciones_Empleado.P_Fecha_Termino = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Termino_Vacaciones.Text.Trim()));
            Alta_Vacaciones_Empleado.P_Fecha_Regreso_Laboral = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Regreso_Vacaciones.Text.Trim()));

            Alta_Vacaciones_Empleado.P_Cantidad_Dias = Convert.ToInt32(Txt_Dias_Vacaciones_Empleado.Text.Trim());
            Alta_Vacaciones_Empleado.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Alta_Vacaciones_Empleado.P_Comentarios = Txt_Comentarios.Text;
            Alta_Vacaciones_Empleado.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            if (Cmb_Calendario_Nomina.SelectedIndex > 0)
                Alta_Vacaciones_Empleado.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex > 0)
                Alta_Vacaciones_Empleado.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());

            //Obtenemos los detalles de los dias de vacaciones que tomara el empleado.
            Alta_Vacaciones_Empleado.P_Dt_Detalles_Vacaciones = Crear_Tabla_Detalles_Vacaciones(Alta_Vacaciones_Empleado.P_Fecha_Inicio,
                Alta_Vacaciones_Empleado.P_Fecha_Termino);

            //Ejecuta el alta de las vacaciones de los empleados.
            if (Alta_Vacaciones_Empleado.Alta_Vacaciones_Empleado())
            {
                Configuracion_Inicial();//HAbilita la configuracion inicial de los controles de la pagina.
                Limpiar_Controles();//Limpia los controles de la pagina.
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");//Redireccionamos a la pagina de principal.
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta de las Vacaciones del Empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Consultar Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias
    /// DESCRIPCION : Consultar las Dependencias
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias()
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Ope_Faltas_Empleado = new Cls_Ope_Nom_Faltas_Empleado_Negocio();//Variable de conexion con la capa de negocios
        DataTable Dt_Dependencias;//Variable que almacenara una lista de dependencias
        try
        {
            Dt_Dependencias = Ope_Faltas_Empleado.Consultar_Dependencia();//Consultamos las dependencias que actualmente existen.
            Cmb_Dependencia.DataSource = Dt_Dependencias;//Establecemos la nueva lista de dependencias al combo de dependencias.
            Cmb_Dependencia.DataTextField = Cat_Dependencias.Campo_Nombre;//Definimos el valor a mostrar al usuario en el combo.
            Cmb_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;//Definimos la clave para cada opcion del combo.
            Cmb_Dependencia.DataBind();//Actualizamos el combo.
            Cmb_Dependencia.Items.Insert(0, new ListItem("< Seleccione >", ""));//Agregamos la opcion de inicio al combo.
            Cmb_Dependencia.SelectedIndex = -1;//Volvemos el indice del combo al inicio.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Dependencias. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Empleados_Por_Dependencia
    /// DESCRIPCION : Consulta los Empleados que pertenecen a la dependencia Seleccionada
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Empleados_Por_Dependencia(String Dependencia_ID)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados_Negocios = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados

        try
        {
            Obj_Empleados_Negocios.P_Dependencia_ID = Dependencia_ID;//Establecemos la dependencia a la propiedad de la clase transportadora.
            Dt_Empleados = Obj_Empleados_Negocios.Consulta_Empleados_General();//Ejecutamos la peticion de consulta de empleados a la capa de negocios.
            Cmb_Empleado.DataSource = Dt_Empleados;//Establecemos la nueva lista de empleados al combo.
            Cmb_Empleado.DataTextField = "EMPLEADOS";//Definimos el texto que mostrara el combo.
            Cmb_Empleado.DataValueField = Cat_Empleados.Campo_Empleado_ID;//Definimos cual sera la clave para cada elemento del combo.
            Cmb_Empleado.DataBind();//Actualizamos los datos del combo.
            Cmb_Empleado.Items.Insert(0, new ListItem("< Seleccione >", ""));//Agregamos la opcion inicial del combo.
            Cmb_Empleado.SelectedIndex = -1;//Volvemos su indice la posicion de inicio.
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Dependencias. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Control Acceso Pagina)
    /// *****************************************************************************************************************************
    /// NOMBRE: Configuracion_Acceso
    /// 
    /// DESCRIPCIÓN: Habilita las operaciones que podrá realizar el usuario en la página.
    /// 
    /// PARÁMETROS: No Áplica.
    /// USUARIO CREÓ: Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 23/Mayo/2011 10:43 a.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *****************************************************************************************************************************
    protected void Configuracion_Acceso(String URL_Pagina)
    {
        List<ImageButton> Botones = new List<ImageButton>();//Variable que almacenara una lista de los botones de la página.
        DataRow[] Dr_Menus = null;//Variable que guardara los menus consultados.

        try
        {
            //Agregamos los botones a la lista de botones de la página.
            Botones.Add(Btn_Nuevo);

            if (!String.IsNullOrEmpty(Request.QueryString["PAGINA"]))
            {
                if (Es_Numero(Request.QueryString["PAGINA"].Trim()))
                {
                    //Consultamos el menu de la página.
                    Dr_Menus = Cls_Sessiones.Menu_Control_Acceso.Select("MENU_ID=" + Request.QueryString["PAGINA"]);

                    if (Dr_Menus.Length > 0)
                    {
                        //Validamos que el menu consultado corresponda a la página a validar.
                        if (Dr_Menus[0][Apl_Cat_Menus.Campo_URL_Link].ToString().Contains(URL_Pagina))
                        {
                            Cls_Util.Configuracion_Acceso_Sistema_SIAS(Botones, Dr_Menus[0]);//Habilitamos la configuracón de los botones.
                        }
                        else
                        {
                            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                        }
                    }
                    else
                    {
                        Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                    }
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al habilitar la configuración de accesos a la página. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Es_Numero(String Cadena)
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

    #endregion

    #region (Eventos)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de las vacaciones para el empelado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip.Equals("Nuevo"))
            {
                Limpiar_Controles();//Limpia los controles de la pagina.
                Habilitar_Controles("Nuevo");//Habilita la configuracion de los controles de la pagina para la operacion de alta.
                Pnl_Datos_Vacaciones_Empleado.Enabled = false;

                Btn_Consultar_Dias_Vacaciones_Click(sender, e);
            }
            else
            {
                //Validamos que los datos ingresados por el usuario sean validos.
                if (Validar_Datos_Vacaciones_Empleado())
                {
                    Alta_Vacaciones_Empleado();//Ejecuta el alta de vacacion para el empleado.
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:inicializarEventosVacaciones();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al ejecutar la alta de la vacacion solicitada por el empleado. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");//Redireccionamos a la pagina de principal.
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
                Limpiar_Controles();//Limpia los controles de la pagina.
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:inicializarEventosVacaciones();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Consultar_Dias_Vacaciones_Click
    ///DESCRIPCIÓN: Busqueda de los dias de vacaciones del empleado.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Consultar_Dias_Vacaciones_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Cls_Vacaciones_Empleado = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios Cat_Empleados_Consulta = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios.
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable que almacenara la información del empleado.
        DataTable Dt_Empleado = null;//Variable que almacenara una lista de Empleados
        Int32 Dias = 0;//Variable que almacenara los dias de vacaciones que el empleado puede tomar.
        Int32 Dias_Comprometidos = 0;

        try
        {
            INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_No_Empleado.Text.Trim());
            Cls_Vacaciones_Empleado.P_Empleado_ID = INF_EMPLEADO.P_Empleado_ID;
            Dias_Comprometidos = Cls_Vacaciones_Empleado.Consultar_Dias_Comprometidos();

            Cls_Vacaciones_Empleado.P_No_Empleado = Txt_No_Empleado.Text.Trim();//Asignamos el no de vacacion que deseamos consultar.
            //Dias = Cls_Vacaciones_Empleado.Consultar_Dias_Vacaciones_Empleado();//Consultamos los dias de vacaciones del empleado.
            //Dias = Obtener_Dias_Disponibles_Vacaciones(Obtener_Identificador_Empleado(Txt_No_Empleado.Text.Trim()));
            Dias = Dias_Vacaciones_Base_Formula(Txt_No_Empleado.Text.Trim());
            //Linea para descontar los dias comprometidos del empleado.
            Dias = Dias - Dias_Comprometidos;
            //Establecemos el valor que mostrar la etiqueta de los dias de vacaciones.
            Lbl_Dias_Vacaciones.Text = Dias.ToString();

            //Validamos que los dias de vacaciones sean mayor a 0.
            if (Dias > 0)
            {
                Txt_Dias_Vacaciones_Empleado.Enabled = true;//Habilitamos el combo para seleccionar los dias a tomar.
                Pnl_Datos_Vacaciones_Empleado.Enabled = true;//Habilitamos los controles para ejecutar el alta de las vacaciones.        
                Div_Mensajes_Dias_Vacaciones.Visible = true;
            }
            else
            {
                Txt_Dias_Vacaciones_Empleado.Enabled = false;//Deshabilitamos el combo para seleccionar los dias a tomar.
                Pnl_Datos_Vacaciones_Empleado.Enabled = false;//Deshabilitamos los controles para ejecutar el alta de las vacaciones.
                Lbl_Dias_Vacaciones.Text = "";
                Div_Mensajes_Dias_Vacaciones.Visible = false;
                //Mensaje
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No tiene dias disponibles para tomar vacaciones";
            }

            Cat_Empleados_Consulta.P_No_Empleado = Txt_No_Empleado.Text.Trim();//Asiganmos el no de empleado que deseamos conscultar.
            Dt_Empleado = Cat_Empleados_Consulta.Consulta_Datos_Empleado();//Consultamos al empleado por el no de empleado.
            //Validamos que la variable no sea nula
            if (Dt_Empleado != null)
            {
                //Validamos que hallamos encontrado algun empleado.
                if (Dt_Empleado.Rows.Count > 0)
                {
                    //Obtenemos la dependencia y  la buscamos en el combo.
                    if (!string.IsNullOrEmpty(Dt_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString())) Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(Dt_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()));
                    //Cargamos el combo de empleados dependiendo de la dependencia seleccionada.
                    Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
                    //Obteemos el empleado y lo buscamos en el combo de empleados.
                    if (!string.IsNullOrEmpty(Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString())) Cmb_Empleado.SelectedIndex = Cmb_Empleado.Items.IndexOf(Cmb_Empleado.Items.FindByValue(Dt_Empleado.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString()));
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:inicializarEventosVacaciones();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al consultar los dias de vacacion del empleado. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Vacaciones_Empleados_Click
    ///DESCRIPCIÓN: Busqueda de las vacaciones de los empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 23/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Vacaciones_Empleados_Click(object sender, EventArgs e)
    {
        try
        {
            Configuracion_Inicial();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:inicializarEventosVacaciones();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al consultar las vacaciones. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Fecha_Inicio_Vacaciones_TextChanged
    /// DESCRIPCION : Estable fecha de termino de vacacion en funcion de la fecha fecha de
    /// inicio seleccionada y los dias a tomar de vacaciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Fecha_Inicio_Vacaciones_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Txt_Dias_Vacaciones_Empleado.Text.Trim()))
            {
                if (Validar_Formato_Fecha(Txt_Fecha_Inicio_Vacaciones.Text.Trim()))
                {
                    DateTime Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicio_Vacaciones.Text);//Se establece un formato adecuado a las fecha inicio.
                    Txt_Fecha_Termino_Vacaciones.Text = String.Format("{0:dd/MMM/yyyy}", Calcular_Fecha_Termino_Vacaciones(Fecha_Inicio, Convert.ToInt32(Txt_Dias_Vacaciones_Empleado.Text.Trim())));
                }
                else
                {
                    Txt_Fecha_Termino_Vacaciones.Text = "";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "El formato de la Fecha Inicial no es correcto";
                }

            }
            else
            {
                Txt_Fecha_Termino_Vacaciones.Text = "";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese la cantidad de dias que desea tomar de vacaciones.";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:inicializarEventosVacaciones();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al seleccionar la fecha de inicio. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Fecha_Inicio_Vacaciones_TextChanged
    /// DESCRIPCION : Estable fecha de termino de vacacion en funcion de la fecha fecha de
    /// inicio seleccionada y los dias a tomar de vacaciones.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 23/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Dias_Vacaciones_Empleado_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (!string.IsNullOrEmpty(Txt_Dias_Vacaciones_Empleado.Text.Trim()))
            {
                if (Convert.ToInt32(Lbl_Dias_Vacaciones.Text.Trim()) >= Convert.ToInt32(Txt_Dias_Vacaciones_Empleado.Text.Trim()))
                {
                    if (!string.IsNullOrEmpty(Txt_Fecha_Inicio_Vacaciones.Text.Trim()))
                    {

                        if (Validar_Formato_Fecha(Txt_Fecha_Inicio_Vacaciones.Text))
                        {
                            DateTime Fecha_Inicio = Convert.ToDateTime(Txt_Fecha_Inicio_Vacaciones.Text);//Se establece un formato adecuado a las fecha inicio.
                            Txt_Fecha_Termino_Vacaciones.Text = String.Format("{0:dd/MMM/yyyy}", Calcular_Fecha_Termino_Vacaciones(Fecha_Inicio, Convert.ToInt32(Txt_Dias_Vacaciones_Empleado.Text.Trim())));
                        }
                        else
                        {
                            Txt_Fecha_Termino_Vacaciones.Text = "";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "El formato de la Fecha Inicial no es correcto";
                        }

                    }
                    else
                    {
                        Txt_Fecha_Termino_Vacaciones.Text = "";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Seleccione la Fecha de Inicio de sus vacaciones.";
                    }
                }
                else
                {
                    Txt_Fecha_Termino_Vacaciones.Text = "";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "La cantidad de dias a tomar de vacaciones no puede ser mayor a la cantiadad de dias disponibles.";
                }
            }
            else
            {
                Txt_Fecha_Termino_Vacaciones.Text = "";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Ingrese la cantidad de dias que desea tomar de vacaciones.";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:inicializarEventosVacaciones();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al ingresar los dias de vacaciones. Error: [" + Ex.Message.ToString() + "]";
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 01/Diciembre/2010
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
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:inicializarEventosVacaciones();", true);
    }
    #endregion

    #region (Modificacion Vacaciones)
    ///***********************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Dias_Disponibles_Vacaciones
    ///
    ///DESCRIPCIÓN: Obtiene los dias de vacaciones que tiene disponibles el empleado.
    ///
    ///PARAMETROS: Empleado_ID.- Identificador único del empleado.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 7/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***********************************************************************************************************************************
    private Int32 Obtener_Dias_Disponibles_Vacaciones(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalle_Vacacion_Empleado = null;                                                                   //Variable que almacena la lista de vacaciones que a tomado el empleado.
        Int32 Contador_Dias_Vacaciones_Empleados = 0;                                                                    //Variable que almacena la cantidad de dias de vacaciones que tiene asignados el empleado.

        try
        {
            //Consultamos los dias que tiene el empleado como dias disponibles de vacaciones en el periodo actual, el periodo
            //anterior al actual y el periodo siguiente al actual.
            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Dt_Detalle_Vacacion_Empleado = Obj_Vacaciones_Empleados.Consultar_Cantidad_Dias_Disponiubles_Por_Periodo_Empleado();

            if (Dt_Detalle_Vacacion_Empleado is DataTable)
            {
                foreach (DataRow Detalle_Vacacion in Dt_Detalle_Vacacion_Empleado.Rows)
                {
                    if (Detalle_Vacacion is DataRow)
                    {
                        if (!String.IsNullOrEmpty(Detalle_Vacacion[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles].ToString()))
                        {
                            Contador_Dias_Vacaciones_Empleados += Convert.ToInt32(Detalle_Vacacion[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles].ToString());
                        }
                    }
                }
            }

            //A los dias que tiene empleado como dias disponibles de vacaciones de acuerdos al periodo vacacional
            //se le suman los dias que tomo el empleado de vacaciones anteriormente y se encuntran en un estatus 
            //de PAGADOS y PENDIENTES por tomar.
            Contador_Dias_Vacaciones_Empleados += Consultar_Dias_Pagados_Perndientes_Tomar(Empleado_ID);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los dias de vacaciones que na tenido el empelado. Error: [" + Ex.Message + "]");
        }
        return Contador_Dias_Vacaciones_Empleados;
    }
    ///***********************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Identificador_Empleado
    ///
    ///DESCRIPCIÓN: Obtiene el Id del empleado a partir del número del empleado.
    ///
    ///PARAMETROS: No_Empleado.- Identificador único del empleado.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 7/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***********************************************************************************************************************************
    private String Obtener_Identificador_Empleado(String No_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;                                              //Variable que almacenara la información del empleado consultado.
        String Empleado_ID = "";                                                    //Variable que almacenara el identificador del empleado.

        try
        {
            Obj_Empleados.P_No_Empleado = No_Empleado;
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
                                Empleado_ID = Empleado[Cat_Empleados.Campo_Empleado_ID].ToString();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar el identificador del empleado. Error: [" + Ex.Message + "]");
        }
        return Empleado_ID;
    }
    ///***********************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar
    ///
    ///DESCRIPCIÓN: Obtiene los dias que el empleado tiene en estatus de pagado y pendientes por tomar.
    ///
    ///PARAMETROS: No_Empleado.- Identificador único del empleado.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 7/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***********************************************************************************************************************************
    private Int32 Obtener_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Vacaciones_Dia_Det = null;//Variable que almacena los resultados de la búsqueda.  
        Int32 Dias_Pagados_Pendientes_Tomar = 0;

        try
        {
            Obj_Vacaciones.P_Empleado_ID = Empleado_ID;
            Dt_Vacaciones_Dia_Det = Obj_Vacaciones.Consultar_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar();

            if (Dt_Vacaciones_Dia_Det is DataTable)
            {
                if (Dt_Vacaciones_Dia_Det.Rows.Count > 0)
                {
                    Dias_Pagados_Pendientes_Tomar = Dt_Vacaciones_Dia_Det.Rows.Count;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los dias que el empleado tiene en estatus de Pagados y Pendientes por tomar. Error: [" + Ex.Message + "]");
        }
        return Dias_Pagados_Pendientes_Tomar;
    }
    #endregion


    private Int32 Consultar_Dias_Pagados_Perndientes_Tomar(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Vacaciones = null;
        Int32 Periodo_Actual = 0;                   //Variable que almacenara el periodo actual vacacional.
        Int32 Periodo_Anterior = 0;                 //Variable que almacenara el periodo anterior vacacional.
        Int32 Periodo_Siguiente = 0;                //Variable que almacenara el siguiente periodo vacacional.
        Int32 Anio_Actual = 0;                      //Variable que almacenara el año actual del calendario de nomina.
        Int32 Anio_Temporal = 0;                    //Variable que almacenara el año en funcion del periodo actual. Podria ser un año  atras o  adelante
        DateTime? Fecha_Inicio = null;
        DateTime? Fecha_Fin = null;
        Int32 Dias_Vacaciones_Pagadas_Pendientes_Tomar = 0;
        try
        {
            Periodo_Actual = Obtener_Periodo_Vacacional();
            Anio_Actual = Obtener_Anio_Calendario_Nomina();

            if (Periodo_Actual == 1) {
                Fecha_Inicio = new DateTime((Anio_Actual - 1), 7, 1, 0, 0, 0);
                Fecha_Fin = new DateTime(Anio_Actual, 6, 30, 23, 59, 59);
            } if (Periodo_Actual == 2) {
                Fecha_Inicio = new DateTime(Anio_Actual, 1, 1, 0, 0, 0);
                Fecha_Fin = new DateTime(Anio_Actual, 12, 31, 23, 59, 59);
            }

            Obj_Vacaciones.P_Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Fecha_Inicio);
            Obj_Vacaciones.P_Fecha_Termino = string.Format("{0:dd/MM/yyyy}", Fecha_Fin);
            Obj_Vacaciones.P_Empleado_ID = Empleado_ID;
            Dt_Vacaciones = Obj_Vacaciones.Consultar_Vacaciones_Dia_Det_Pagados_Pendientes_Tomar();

            if (Dt_Vacaciones is DataTable) {
                if (Dt_Vacaciones.Rows.Count > 0) {
                    Dias_Vacaciones_Pagadas_Pendientes_Tomar = Dt_Vacaciones.Rows.Count;
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cosnultar los dias que tiene el empleado en estatus de Pagado y Pendiente por tomar del periodo anterior al actual y el actual. Error: [" + Ex.Message + "]");
        }
        return Dias_Vacaciones_Pagadas_Pendientes_Tomar;
    }
    ///***********************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Periodo_Vacacional
    ///
    ///DESCRIPCIÓN: Obtiene periodo vacacional en el que nos encontramos actualmente.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 9/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***********************************************************************************************************************************
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
                (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 6, 30, 23, 59, 59)) <= 0))
            {
                //PERIODO VACACIONAL I
                Periodo_Vacacional = 1;
            }
            else if ((DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 7, 1)) >= 0) &&
                (DateTime.Compare(Fecha_Actual, new DateTime(Anio_Calendario_Nomina, 12, 31, 23, 59,59)) <= 0))
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
    ///***********************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Anio_Calendario_Nomina
    ///
    ///DESCRIPCIÓN: Obtiene el año del calendario de nomina que se encuentra vigente actualmente. 
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 9/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***********************************************************************************************************************************
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
    private Int32 Dias_Vacaciones_Base_Formula(String No_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Empleados = null;                                              //Variable que almacena la informacion del empleado consultado.     
        Int32 Cantidad_Dias_Vacaciones = 0;                                                    //Cantidad de dias que le corresponden al empleado segun su fecha de ingreso a presidencia.
        Int32 Dias_Aplican_Calculo = 0;                                                             //Cantidad de dias que lleva el empleado laborando en presidencia.
        DateTime? Fecha_Ingreso_Empleado = null;                                    //Variable que alamaceba la fecha de ingreso del empleado a presidencia.
        DateTime Fecha_Actual = DateTime.Now;                                       //Variable que almacena la fecha actual.
        String Tipo_Nomina_ID = "";                                                 //Variable que almacenara el tipo de nómina al que pertence el empleado.
        Int32 Anio_Nomina = 0;                                                             //Variable que almacenara el año de calendario de nómina vigente actualmente.         
        Int32 Periodo_Actual = 0;                                                   //Variable que almacenara el periodo vacacional actual.
        Int32 Auxiliar = 0;                                                         //Variable auxiliar que almacenara los dias entre la fecha de ingreso y el 30 de Junio del año actual.
        Int32 Dias_Totales_Primer_Periodo_Vacacional = 0;                           //Variable que almacenara los dias totales del primer periodo [Enero - Junio].
        Int32 Dias_Totales_Segundo_Periodo_Vacacional = 0;                          //Variable que almacenara los dias totales del segundo periodo [Julio - Diciembre].
        Int32 Dias_Periodo_II_Anio_Anterior = 0;
        Int32 Dias_VP_I = 0;
        Int32 Dias_VP_II = 0;

        try
        {
            Anio_Nomina = Obtener_Anio_Calendario_Nomina();//Obtenemos el anio del calendario de nomina vigente actualmente.
            Periodo_Actual = Obtener_Periodo_Vacacional();//Obtenemos el periodo vacacional en el que nos encontramos actualmente.

            //Obtenemos el total de dias del periodo de [Enero - Junio] y del periodo [Julio - Diciembre] del anio del calendario de nomina vigente.
            Dias_Totales_Primer_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 1, 1), new DateTime(Anio_Nomina, 6, 30)) + 1;
            Dias_Totales_Segundo_Periodo_Vacacional = (Int32)Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 7, 1), new DateTime(Anio_Nomina, 12, 31)) + 1;

            //Consultamos la información general del empleado.
            Obj_Empleados.P_No_Empleado = No_Empleado;
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

                                //Consultamos el tipo de nomina al que pertence el empleado.
                                Tipo_Nomina_ID = Empleado[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString();
                                //Obtenemos los dias de vacaciones que le corresponden al periodo vacacional de acuerdo al tipo de nómina.
                                 Dias_VP_I = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, 1);
                                 Dias_VP_II = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, 2);

                                //Si el año de ingreso del empleado a presidencia es igual a l año actual entoncés:
                                if (((DateTime)Fecha_Ingreso_Empleado).Year == Anio_Nomina)
                                {
                                    //Identificamos el periodo, si el periodo actual es el primero entonces:
                                    if (Periodo_Actual == 1)
                                    {
                                        //Obtenemos los dias que el empleado lleva laborando en presidencia.
                                        Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), Fecha_Actual) + 1);
                                        Cantidad_Dias_Vacaciones = (Int32)(Dias_Aplican_Calculo * Dias_VP_I) / Dias_Totales_Primer_Periodo_Vacacional;
                                    }
                                    else if (Periodo_Actual == 2)
                                    {
                                        if (((DateTime)Fecha_Ingreso_Empleado) <= new DateTime(Anio_Nomina, 06, 30, 23, 59, 59)) {
                                            //Consultamos los dias que lleva laborando el empleado en presidencia.
                                            Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, ((DateTime)Fecha_Ingreso_Empleado), new DateTime(Anio_Nomina, 06, 30, 23, 59, 59)) + 1);
                                            Cantidad_Dias_Vacaciones += (Int32)(Dias_Aplican_Calculo * Dias_VP_I) / Dias_Totales_Primer_Periodo_Vacacional;
                                        }

                                        //Consultamos los dias que lleva laborando el empleado en presidencia.
                                        Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 07, 1, 0, 0, 0), Fecha_Actual) + 1);
                                        Auxiliar = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 06, 30, 23, 59, 59), ((DateTime)Fecha_Ingreso_Empleado)) + 1);
                                        if (Auxiliar < 0) Auxiliar = 0;
                                        Dias_Aplican_Calculo = Dias_Aplican_Calculo - Auxiliar;
                                        Cantidad_Dias_Vacaciones += (Int32)(Dias_Aplican_Calculo * Dias_VP_II) / Dias_Totales_Segundo_Periodo_Vacacional;
                                    }
                                }
                                else if (((DateTime)Fecha_Ingreso_Empleado).Year < Anio_Nomina)
                                {
                                    //Identificamos el periodo, si el periodo actual es el primero entonces:
                                    if (Periodo_Actual == 1)
                                    {
                                        //Obtenemos los dias del periodo anterior.
                                        Dias_Periodo_II_Anio_Anterior = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, (DateTime)Fecha_Ingreso_Empleado, new DateTime((Anio_Nomina - 1), 12, 31)) + 1);
                                        Auxiliar = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, (DateTime)Fecha_Ingreso_Empleado, new DateTime((Anio_Nomina - 1), 06, 30)) + 1);
                                        if (Auxiliar < 0) Auxiliar = 0;
                                        Dias_Periodo_II_Anio_Anterior = Dias_Periodo_II_Anio_Anterior - Auxiliar;
                                        Cantidad_Dias_Vacaciones += (Int32)((Dias_Periodo_II_Anio_Anterior * Dias_VP_II) / Dias_Totales_Segundo_Periodo_Vacacional);

                                        //Obtenemos dias periodo actual.
                                         Dias_VP_I = Consultar_Dias_Vacaciones_Tipo_Nomina(Tipo_Nomina_ID, 1);
                                        Cantidad_Dias_Vacaciones += Dias_VP_I;
                                    }
                                    else if (Periodo_Actual == 2)
                                    {
                                        Cantidad_Dias_Vacaciones += Dias_VP_I;

                                        Dias_Aplican_Calculo = (Int32)(Cls_DateAndTime.DateDiff(DateInterval.Day, new DateTime(Anio_Nomina, 06, 30), Fecha_Actual) + 1);
                                        Cantidad_Dias_Vacaciones += (Int32)((Dias_Aplican_Calculo * Dias_VP_II) / Dias_Totales_Segundo_Periodo_Vacacional);
                                    }
                                }

                                if (Cantidad_Dias_Vacaciones > 0) Cantidad_Dias_Vacaciones = Cantidad_Dias_Vacaciones - Obtener_Dias_Tomados_Vacaciones(Empleado[Cat_Empleados.Campo_Empleado_ID].ToString().Trim());
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
        return Cantidad_Dias_Vacaciones;
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
    ///***********************************************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Dias_Disponibles_Vacaciones
    ///
    ///DESCRIPCIÓN: Obtiene los dias de vacaciones que tiene disponibles el empleado.
    ///
    ///PARAMETROS: Empleado_ID.- Identificador único del empleado.
    ///
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 7/Marzo/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///***********************************************************************************************************************************
    private Int32 Obtener_Dias_Tomados_Vacaciones(String Empleado_ID)
    {
        Cls_Ope_Nom_Vacaciones_Empleado_Negocio Obj_Vacaciones_Empleados = new Cls_Ope_Nom_Vacaciones_Empleado_Negocio();//Variable de conexion con la capa de negocios.
        DataTable Dt_Detalle_Vacacion_Empleado = null;                                                                   //Variable que almacena la lista de vacaciones que a tomado el empleado.
        Int32 Contador_Dias_Vacaciones_Empleados = 0;                                                                    //Variable que almacena la cantidad de dias de vacaciones que tiene asignados el empleado.

        try
        {
            //Consultamos los dias que tiene el empleado como dias disponibles de vacaciones en el periodo actual, el periodo
            //anterior al actual y el periodo siguiente al actual.
            Obj_Vacaciones_Empleados.P_Empleado_ID = Empleado_ID;
            Dt_Detalle_Vacacion_Empleado = Obj_Vacaciones_Empleados.Consultar_Cantidad_Dias_Disponiubles_Por_Periodo_Empleado();

            if (Dt_Detalle_Vacacion_Empleado is DataTable)
            {
                foreach (DataRow Detalle_Vacacion in Dt_Detalle_Vacacion_Empleado.Rows)
                {
                    if (Detalle_Vacacion is DataRow)
                    {
                        if (!String.IsNullOrEmpty(Detalle_Vacacion[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Disponibles].ToString()))
                        {
                            Contador_Dias_Vacaciones_Empleados += Convert.ToInt32(Detalle_Vacacion[Ope_Nom_Vacaciones_Empleado_Detalles.Campo_Dias_Tomados].ToString());
                        }
                    }
                }
            }

            //A los dias que tiene empleado como dias disponibles de vacaciones de acuerdos al periodo vacacional
            //se le suman los dias que tomo el empleado de vacaciones anteriormente y se encuntran en un estatus 
            //de PAGADOS y PENDIENTES por tomar.
           // Contador_Dias_Vacaciones_Empleados += Consultar_Dias_Pagados_Perndientes_Tomar(Empleado_ID);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar los dias de vacaciones que na tenido el empelado. Error: [" + Ex.Message + "]");
        }
        return Contador_Dias_Vacaciones_Empleados;
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
}
