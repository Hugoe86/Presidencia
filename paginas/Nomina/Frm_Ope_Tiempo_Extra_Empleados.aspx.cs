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
using Presidencia.Tiempo_Extra.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Empleados.Negocios;
using Presidencia.Dependencias.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using System.Globalization;
using System.Text.RegularExpressions;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.ReportSource;
using Presidencia.Informacion_Presupuestal;

public partial class paginas_Nomina_Frm_Ope_Tiempo_Extra_Empleados : System.Web.UI.Page
{

    #region (Page_Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();
            }Btn_Autorizacion_Horas_Extras.Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }
    #endregion

    #region (Metodos)

    #region (Metodos Generales)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Configuracion Inicial del Catalogo de Tiempo de Horas Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {        
        Consultar_Dependencias(Cmb_Dependencia);
        Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
        Consultar_Dependencias(Cmb_Busqueda_Dependencia);

        Limpiar_Ctlrs();
        Consultar_Horas_Extra();
        Habilitar_Controles("Inicial");
        Consultar_Calendarios_Nomina();
        Consultar_Busqueda_Calendarios_Nomina();//Consulta los calendarios de nómina que existen actualmente en el sistema.
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Ctlrs()
    {
        try
        {
            Txt_No_Tiempo_Extra_Empleado.Text = "";
            Cmb_Estatus.SelectedIndex = -1;
            Txt_Fecha.Text = "";
            Txt_Comentarios.Text = "";
            Txt_Empleados.Text = "";
            Txt_Horas.Text = "";
            Cmb_Pago_Dia_Doble.SelectedIndex = -1;
            Txt_Busqueda_No_Tiempo_Extra.Text = "";
            Cmb_Busqueda_Estatus.SelectedIndex = -1;
            Txt_Busqueda_Fecha_Inicio.Text = "";
            Txt_Busqueda_Fecha_Fin.Text = "";

            Grid_Tiempo_Extra_Empleado.SelectedIndex = -1;
            Grid_Tiempo_Extra_Empleado.DataSource = new DataTable();
            Grid_Tiempo_Extra_Empleado.DataBind();

            //Cmb_Calendario_Nomina.SelectedIndex = -1;
            //Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
        }catch(Exception Ex){
            throw new Exception("Error al limpiar los controles del formulario. Error: [" + Ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;
        Boolean Modificar_RH;

        try
        {
            Habilitado = false;
            Modificar_RH = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";

                    Btn_Busqueda_Tiempo_Extra_Empleado.Enabled = true;

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    Cmb_Dependencia.Enabled = false;
                    TPnl_Contenedor.ActiveTabIndex = 0;

                    Cmb_Busqueda_Estatus.SelectedIndex = Cmb_Busqueda_Estatus.Items.IndexOf(Cmb_Busqueda_Estatus.Items.FindByText("Pendiente"));

                    Configuracion_Acceso("Frm_Ope_Tiempo_Extra_Empleados.aspx");                    
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    Btn_Busqueda_Tiempo_Extra_Empleado.Enabled = false;
                    if (Session["Dt_Empleados"] != null) Session.Remove("Dt_Empleados");

                    Cmb_Dependencia.Enabled = false;//Editado
                    TPnl_Contenedor.ActiveTabIndex = 1;
                    Cmb_Estatus.SelectedIndex = 1;

                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";

                    Btn_Busqueda_Tiempo_Extra_Empleado.Enabled = false;
                    TPnl_Contenedor.ActiveTabIndex = 1;
                    Modificar_RH = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;                    
                    break;
            }

            Txt_No_Tiempo_Extra_Empleado.Enabled = false;
            if (Modificar_RH == true)
            {
                Btn_Fecha.Enabled = false;
                Txt_Fecha.Enabled = false;
            }
            else
            {
                Txt_Fecha.Enabled = Habilitado;
                Btn_Fecha.Enabled = Habilitado;
            }
            Txt_Comentarios.Enabled = Habilitado;
            Txt_Empleados.Enabled = Habilitado;
            Txt_Horas.Enabled = false;
            Cmb_Pago_Dia_Doble.Enabled = Habilitado;
            Cmb_Empleados.Enabled = Habilitado;
            Grid_Horas_Extra.Enabled = !Habilitado;
            Grid_Tiempo_Extra_Empleado.Enabled = Habilitado;
            Btn_Agregar_Empleado.Enabled = Habilitado;
            Btn_Buscar_Empleados.Enabled = Habilitado;            
            Btn_Autorizacion_Horas_Extras.Visible = false;

            Cmb_Dependencia.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;
            Cmb_Estatus.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;
            Cmb_Busqueda_Dependencia.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;
            Tr_Periodos_Fiscales.Visible = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;

            
            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
            Cmb_Calendario_Nomina.Focus();
        }
        catch (Exception ex)
        {
            throw new Exception("Error al Habilitar los Controles del formulario. Error:[" + ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Horas_Extra
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Horas_Extra()
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Consultar_Periodos = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        DataTable Dt_Periodos_Validos = null;
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (string.IsNullOrEmpty(Txt_Fecha.Text.Trim()))
        {
            //Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha <br>";
            //Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato de Fecha Incorrecto  <br>";
            Datos_Validos = false;
        }
        else 
        {
            //Consultar_Periodos.P_Fecha = string.Format("{0:dd/MM/yyyy}", DateTime.Today);
            //Dt_Periodos_Validos = Consultar_Periodos.Consultar_Periodo_Por_Fecha();

            //if (Dt_Periodos_Validos != null)
            //{
            //    if (Dt_Periodos_Validos.Rows.Count > 0)
            //    {
            //        DateTime Fecha_Inicia_Actual = Convert.ToDateTime(Dt_Periodos_Validos.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString().Trim());
            //        DateTime Fecha_Fin_Actual = Convert.ToDateTime(Dt_Periodos_Validos.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString().Trim());
            //        DateTime Fecha_Seleccionada = Convert.ToDateTime(Txt_Fecha.Text.Trim());

            //        if (!(Fecha_Seleccionada >= Fecha_Inicia_Actual) || !(Fecha_Seleccionada <= Fecha_Fin_Actual))
            //        {
            //            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La fecha del tiempo extra no puede ser fuera de la catorcena actual. <br>";
            //            Datos_Validos = false;
            //        }
            //    }
            //}             
        }

        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Estatus <br>";
            Datos_Validos = false;
        }

        if (Cmb_Pago_Dia_Doble.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Pago Dia Doble <br>";
            Datos_Validos = false;
        }
        if (string.IsNullOrEmpty(Txt_Horas.Text) && Cmb_Pago_Dia_Doble.SelectedItem.Text.Equals("NO"))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Horas <br>";
            Datos_Validos = false;
        }
        //if (string.IsNullOrEmpty(Txt_Comentarios.Text))
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Comentarios <br>";
        //    Datos_Validos = false;
        //}
        if (Cmb_Dependencia.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Dependencia <br>";
            Datos_Validos = false;
        }
        if (Grid_Tiempo_Extra_Empleado.Rows.Count <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se ha asignado a ningún empleado<br>";
            Datos_Validos = false;
        }

        if ((Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")))
        {
            if (Cmb_Calendario_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se a seleccionado ninguna nómina. <br />";
                Datos_Validos = false;
            }

            if (Cmb_Periodos_Catorcenales_Nomina.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + No se a seleccionado ningún periodo nominal. <br />";
                Datos_Validos = false;
            }
        }

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: IsNumeric
    /// DESCRIPCION : Evalua que la cadena pasada como parametro sea un Numerica.
    /// PARÁMETROS: Cadena.- El dato a evaluar si es numerico.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
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
    /// NOMBRE DE LA FUNCION: Validar_Anios
    /// DESCRIPCION : Valida las fechas de busqueda, tanto inicial como final
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Anios(String Fecha_Inicio, String Fecha_Fin)
    {
        Boolean Valida = true;

        try
        {
            if (!string.IsNullOrEmpty(Fecha_Inicio) && !string.IsNullOrEmpty(Fecha_Fin))
            {
                if (Validar_Formato_Fecha(Fecha_Inicio))
                {
                    if (Validar_Formato_Fecha(Fecha_Fin))
                    {
                        Fecha_Inicio = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Inicio));
                        Fecha_Fin = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Fecha_Fin));

                        DateTime _Fecha_Inicio = DateTime.ParseExact(Fecha_Inicio, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                        DateTime _Fecha_Fin = DateTime.ParseExact(Fecha_Fin, "dd/MM/yyyy", CultureInfo.InvariantCulture);

                        if (_Fecha_Inicio > _Fecha_Fin)
                        {
                            Valida = false;
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Al realizar la busqueda la Fecha Inicial no puede ser mayor que la final";
                        }
                    }
                    else
                    {
                        Valida = false;
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "Formato de Fecha Final Incorrecto";
                    }
                }
                else
                {
                    Valida = false;
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Formato de Fecha Inicial Incorrecto";
                }
            }
            else {
                Valida = false;
            }
        }
        catch (Exception Ex) {
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
        if (Fecha != null)
        {
            return Regex.IsMatch(Fecha, Cadena_Fecha);
        }
        else
        {
            return false;
        }
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
    /// NOMBRE DE LA FUNCION: Consultar_Busqueda_Calendarios_Nomina
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
    private void Consultar_Busqueda_Calendarios_Nomina()
    {
        Cls_Cat_Nom_Calendario_Nominas_Negocio Obj_Calendario_Nominales = new Cls_Cat_Nom_Calendario_Nominas_Negocio();//Variable de conexión con la capa de negocios.
        DataTable Dt_Calendarios_Nominales = null;//Variable que almacena los calendarios nominales que existén actualmente en el sistema.
        try
        {
            Dt_Calendarios_Nominales = Obj_Calendario_Nominales.Consultar_Calendario_Nominas();
            Dt_Calendarios_Nominales = Formato_Fecha_Calendario_Nomina(Dt_Calendarios_Nominales);

            if (Dt_Calendarios_Nominales is DataTable)
            {
                Cmb_Busqueda_Calendario_Nomina.DataSource = Dt_Calendarios_Nominales;
                Cmb_Busqueda_Calendario_Nomina.DataTextField = "Nomina";
                Cmb_Busqueda_Calendario_Nomina.DataValueField = Cat_Nom_Calendario_Nominas.Campo_Nomina_ID;
                Cmb_Busqueda_Calendario_Nomina.DataBind();
                Cmb_Busqueda_Calendario_Nomina.Items.Insert(0, new ListItem("<-- Seleccione -->", ""));

                Cmb_Busqueda_Calendario_Nomina.SelectedIndex = Cmb_Busqueda_Calendario_Nomina.Items.IndexOf
                    (Cmb_Busqueda_Calendario_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Anyo));

                if (Cmb_Busqueda_Calendario_Nomina.SelectedIndex > 0)
                {
                    Consultar_Busqueda_Periodos_Catorcenales_Nomina(Cmb_Busqueda_Calendario_Nomina.SelectedValue.Trim());
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
    ///**************************************************************************************************************************************************************
    ///Nombre: Validar_No_Permitir_Empleados_Diferentes_Dependencias
    ///
    ///Descripción: Este método valida que no sea posible agregar empleados de diferentes
    ///             unidades responsables a una solicitud de incidencia.
    ///             
    /// Parámetros: No Aplica.
    ///
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///**************************************************************************************************************************************************************
    protected Boolean Validar_No_Permitir_Empleados_Diferentes_Dependencias(String Empleado_ID_Nuevo)
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;//Variable de conexión con la capa de negocios.
        Cls_Cat_Empleados_Negocios INF_EMPLEADO_NUEVO = null;//Variable de conexión con la capa de negocios.
        Boolean Resultado = false;//Variable que guarda el estatus para validar que no existan empleados de diferentes unidades responsables.
        String Empleado_ID = String.Empty;//Variable que almacena el identificador unico del empleado.

        try
        {
            //Consultamos la información del empleado a agregar a la solicitud.
            INF_EMPLEADO_NUEVO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Empleado_ID_Nuevo);

            if (Grid_Tiempo_Extra_Empleado.Rows.Count > 0)
            {
                foreach (GridViewRow Registro_Empleado in Grid_Tiempo_Extra_Empleado.Rows)
                {
                    if (Registro_Empleado is GridViewRow)
                    {
                        if (!String.IsNullOrEmpty(Registro_Empleado.Cells[0].Text))
                        {

                            //Get identificador del empleado.
                            Empleado_ID = Registro_Empleado.Cells[0].Text.Trim();
                            //Consultamos los datos del empleado. 
                            INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Empleado_ID);

                            if (INF_EMPLEADO is Cls_Cat_Empleados_Negocios)
                            {
                                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Dependencia_ID))
                                {

                                    //Validamos que los empleados pertenescan a la misma unidad responsable.
                                    if (INF_EMPLEADO_NUEVO.P_Dependencia_ID.Equals(INF_EMPLEADO.P_Dependencia_ID))
                                    {
                                        Resultado = true;
                                    }
                                    else { Resultado = false; break; }
                                }
                            }
                        }
                    }
                }
            }
            else
            {
                Resultado = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al validar que no se agreguen empleados de diferente dependencia. Error: [" + Ex.Message + "]");
        }
        return Resultado;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Buscar_Empleado
    /// DESCRIPCION : Busca al empleado por Nombre o Numero de Control
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          : Armando Zavala Moreno
    /// FECHA_MODIFICO    : 29/Marzo/2010
    /// CAUSA_MODIFICACION: Validar el Txt_Empleados para que cuando sea numeros solo admita 6
    ///*******************************************************************************
    private void Buscar_Empleado()
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;
        String Empleado_ID = String.Empty;
        String Dependencia_ID = String.Empty;
        Boolean Terminar = false;

        try
        {
            if (!String.IsNullOrEmpty(Txt_Empleados.Text.Trim()))
            {
                if (Es_Numero(Txt_Empleados.Text.Trim()))
                {
                    if (Txt_Empleados.Text.Length > 6)
                    {
                        Lbl_Mensaje_Error.Text = "El número de empleado es demasiado grande, revíselo e intente de nuevo.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Terminar = true;
                    }
                    else
                    {
                        Txt_Empleados.Text = String.Format("{0:000000}", Convert.ToInt64(Txt_Empleados.Text.Trim()));
                        Obj_Empleados.P_No_Empleado = Txt_Empleados.Text.Trim();
                    }
                }
                else
                {
                    Obj_Empleados.P_Nombre = Txt_Empleados.Text.Trim();
                }
                if (Terminar != true)
                {

                    if ((Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false)
                    {
                        Obj_Empleados.P_Dependencia_ID = Cmb_Dependencia.SelectedValue.Trim();
                    }

                    Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();

                    if (Dt_Empleados is DataTable)
                    {
                        if (Dt_Empleados.Rows.Count > 0)
                        {
                            foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                            {
                                if (EMPLEADO is DataRow)
                                {
                                    if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                        Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                                    if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim()))
                                        Dependencia_ID = EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();

                                    Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(
                                        Cmb_Dependencia.Items.FindByValue(Dependencia_ID));

                                    Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());

                                    Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(
                                        Cmb_Empleados.Items.FindByValue(Empleado_ID));
                                }
                            }
                            if (Cmb_Empleados.Items.Count > 1)
                            {
                                Cmb_Empleados.Focus();
                            }
                            else
                            {
                                Txt_Empleados.Text = string.Empty;
                                Txt_Empleados.Focus();
                            }
                        }
                        else
                        {
                            Cmb_Empleados.Items.Clear();
                            Lbl_Mensaje_Error.Text = "No se encontraron empleados con los datos ingresados<br />&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;El empleado pertence a otra dependencia.";
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                        }
                    }
                }
            }
            else
            {
                Cmb_Empleados.Items.Clear();
                Lbl_Mensaje_Error.Text = "No se ha proporcionado ningún dato para realizar la búsqueda.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);            
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al buscar el Empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Alta - Modificar - Actualizar)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Tiempo_Extra
    /// DESCRIPCION : Ejecuta el alta de un Tiempo Extra
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Tiempo_Extra()
    {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Ope_Tiempo_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();
        try
        {
            Ope_Tiempo_Extra.P_Dependencia_ID = Cmb_Dependencia.SelectedValue.ToString().Trim();
            Ope_Tiempo_Extra.P_Fecha = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha.Text.Trim()));
            Ope_Tiempo_Extra.P_Pago_Dia_Doble = Cmb_Pago_Dia_Doble.SelectedItem.Text;
            Ope_Tiempo_Extra.P_Horas = Convert.ToDouble((string.IsNullOrEmpty(Txt_Horas.Text)) ? "0" : Txt_Horas.Text);
            Ope_Tiempo_Extra.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Ope_Tiempo_Extra.P_Comentarios = Txt_Comentarios.Text;
            Ope_Tiempo_Extra.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if ((Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")))
            {
                Ope_Tiempo_Extra.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Ope_Tiempo_Extra.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            }

            if (Session["Dt_Empleados"] != null)
            {
                Ope_Tiempo_Extra.P_Dt_Empleados = (DataTable)Session["Dt_Empleados"];
            }
            if (Session["Dt_Empleados"] != null)
            {
                Session.Remove("Dt_Empleados");
            }

            if (Ope_Tiempo_Extra.Alta_Tiempo_Extra())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Alta Tiempo Horas Extra]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta a un Hora Extra. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Tiempo_Extra
    /// DESCRIPCION : Ejecuta la Modificacion de un Tiempo Extra
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Tiempo_Extra()
    {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Ope_Tiempo_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();
        try
        {
            Ope_Tiempo_Extra.P_No_Tiempo_Extra = Txt_No_Tiempo_Extra_Empleado.Text.Trim();
            Ope_Tiempo_Extra.P_Dependencia_ID = Cmb_Dependencia.SelectedValue.ToString().Trim();
            Ope_Tiempo_Extra.P_Fecha = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha.Text.Trim()));
            Ope_Tiempo_Extra.P_Pago_Dia_Doble = Cmb_Pago_Dia_Doble.SelectedItem.Text;
            Ope_Tiempo_Extra.P_Horas = Convert.ToDouble((string.IsNullOrEmpty(Txt_Horas.Text)) ? "0" : Txt_Horas.Text);
            Ope_Tiempo_Extra.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Ope_Tiempo_Extra.P_Comentarios = Txt_Comentarios.Text;
            Ope_Tiempo_Extra.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);

            if ((Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")))
            {
                Ope_Tiempo_Extra.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Ope_Tiempo_Extra.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            }

            if (Session["Dt_Empleados"] != null)
            {
                Ope_Tiempo_Extra.P_Dt_Empleados = (DataTable)Session["Dt_Empleados"];
            }
            if (Session["Dt_Empleados"] != null)
            {
                Session.Remove("Dt_Empleados");
            }

            if (Ope_Tiempo_Extra.Modificar_Tiempo_Extra())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Modificar Tiempo Horas Extra]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Modificar una Hora Extra. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Tiempo_Extra
    /// DESCRIPCION : Ejecuta la Baja de un Tiempo Extra
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Tiempo_Extra()
    {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Ope_Tiempo_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();
        try
        {
            Ope_Tiempo_Extra.P_No_Tiempo_Extra = Txt_No_Tiempo_Extra_Empleado.Text.Trim();

            if (Ope_Tiempo_Extra.Eliminar_Tiempo_Extra())
            {
                Configuracion_Inicial();
                Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Tiempo Horas Extra]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Eliminar una Hora Extra. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region(Metodos Add & Delete Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Empleado
    /// DESCRIPCION : Agrega un Empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 10/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Empleado(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;
        DataTable Dt = (DataTable)Session["Dt_Empleados"];
        Cls_Cat_Empleados_Negocios _Cat_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            int index = _DropDownList.SelectedIndex;
            if (index > 0)
            {
                Filas = _DataTable.Select(Cat_Empleados.Campo_Empleado_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar el Empleado, ya que esta ya se ha agregado');", true);
                    Cmb_Empleados.SelectedIndex = 0;
                }
                else
                {
                    _Cat_Empleados.P_Empleado_ID = _DropDownList.SelectedValue.Trim();
                    DataTable Dt_Temporal = _Cat_Empleados.Consulta_Empleados_General();
                    if (!(Dt_Temporal == null))
                    {
                        if (Dt_Temporal.Rows.Count > 0)
                        {
                            foreach (DataRow EMPLEADO in Dt_Temporal.Rows)
                            {
                                DataRow row = Dt.NewRow();
                                row[Cat_Empleados.Campo_Empleado_ID] = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                                row[Cat_Empleados.Campo_Nombre] = EMPLEADO["EMPLEADOS"].ToString().Trim();

                                Dt.Rows.Add(row);
                                Dt.AcceptChanges();
                                Session["Dt_Empleados"] = Dt;

                                _GridView.Columns[0].Visible = true;
                                _GridView.DataSource = (DataTable)Session["Dt_Empleados"];
                                _GridView.DataBind();
                                _GridView.Columns[0].Visible = false;
                                Cmb_Empleados.SelectedIndex = 0;
                            }
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ningun Empleado a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al agregar al Empleado" + Ex.Message);
        }
    }
    #endregion

    #region (Metodos Cargar Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias
    /// DESCRIPCION : Consulta las dependencia que existen actualmente. Y carga el 
    /// Combo de Dependencias.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias(DropDownList _DropDownList)
    {
        Cls_Cat_Dependencias_Negocio _Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Dependencias = null;
        try
        {
            Dt_Dependencias = _Cat_Dependencias.Consulta_Dependencias();
            _DropDownList.DataSource = Dt_Dependencias;
            _DropDownList.DataTextField = Cat_Dependencias.Campo_Nombre;
            _DropDownList.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            _DropDownList.DataBind();
            _DropDownList.Items.Insert(0, new ListItem("< Seleccione >", ""));
            //Solo podra realizar busquedas de su de la dependencia a la que pertence el usuario logueado.
            if (Cls_Sessiones.Datos_Empleado != null)
            {
                if (Cls_Sessiones.Datos_Empleado.Rows.Count > 0)
                {
                    _DropDownList.SelectedIndex = _DropDownList.Items.IndexOf(_DropDownList.Items.FindByValue(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()));
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Dependencias. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Consultar_Busqueda_Periodos_Catorcenales_Nomina
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
    private void Consultar_Busqueda_Periodos_Catorcenales_Nomina(String Nomina_ID)
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
                    Cmb_Busqueda_Periodos_Catorcenales_Nomina.DataSource = Dt_Periodos_Catorcenales;
                    Cmb_Busqueda_Periodos_Catorcenales_Nomina.DataTextField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Busqueda_Periodos_Catorcenales_Nomina.DataValueField = Cat_Nom_Nominas_Detalles.Campo_No_Nomina;
                    Cmb_Busqueda_Periodos_Catorcenales_Nomina.DataBind();
                    Cmb_Busqueda_Periodos_Catorcenales_Nomina.Items.Insert(0, new ListItem("< Seleccione >", ""));
                    Cmb_Busqueda_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

                    Validar_Periodos_Pago(Cmb_Busqueda_Periodos_Catorcenales_Nomina);

                    Cmb_Busqueda_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Busqueda_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Busqueda_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
                    Cargar_Fechas_Busqueda();
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
    ///NOMBRE DE LA FUNCIÓN: Cargar_Fechas_Busqueda
    ///DESCRIPCIÓN: Consulta las fecchas del periodo nominal que fue seleccionado por
    ///             el usuario para poder realizar las asistencias de los empleados
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 05-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cargar_Fechas_Busqueda()
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Int32 index = Cmb_Busqueda_Periodos_Catorcenales_Nomina.SelectedIndex;
            Txt_Busqueda_Fecha_Inicio.Text = "";
            Txt_Busqueda_Fecha_Fin.Text = "";
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
        Cls_Cat_Nom_Calendario_Nominas_Negocio Rs_Cat_Nom_Calendario_Nominas_Negocio = new Cls_Cat_Nom_Calendario_Nominas_Negocio(); //Variable de conexión hacia la capa de negocios
        DataTable Dt_Periodo_Nominal; //Obtiene los valores de la consulta y servira para poder asignar estos a los controles correspondientes
        try
        {
            Rs_Cat_Nom_Calendario_Nominas_Negocio.P_Nomina_ID = Cmb_Busqueda_Calendario_Nomina.SelectedValue.ToString();
            Rs_Cat_Nom_Calendario_Nominas_Negocio.P_No_Nomina = Convert.ToInt32(Cmb_Busqueda_Periodos_Catorcenales_Nomina.SelectedValue.ToString());
            Dt_Periodo_Nominal = Rs_Cat_Nom_Calendario_Nominas_Negocio.Consulta_Periodos_Nomina(); //Consulta las fechas de inicio y fin del periodo nominal seleccionado por el usuario

            //Asigna los valores obtenidos de la consulta a los controles correspondientes
            foreach (DataRow Registro in Dt_Periodo_Nominal.Rows)
            {
                if (!String.IsNullOrEmpty(Registro[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()) &&
                    !String.IsNullOrEmpty(Registro[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString()))
                {
                    Txt_Busqueda_Fecha_Inicio.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString()));
                    Txt_Busqueda_Fecha_Fin.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fechas_Periodo_Nominal " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Empleados_Por_Dependencia
    /// DESCRIPCION : Consulta los empleados que pernecen a la dependencia que ha 
    /// sido pasada como parametro al metodo.
    /// PARÁMETROS: Dependencia_ID.- Es la dependencia seleccionada y  de la cual se 
    /// se quiere obtener los empleados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Empleados_Por_Dependencia(String Dependencia_ID)
    {
        Cls_Cat_Empleados_Negocios _Cat_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados

        try
        {
            Cmb_Empleados.DataSource = null;//Limpia combo para evitar errores si es llamado 2 veces la funcion
            Cmb_Empleados.DataBind();
            _Cat_Empleados.P_Dependencia_ID = Dependencia_ID;
            Dt_Empleados = _Cat_Empleados.Consulta_Empleados_General();//Consulta los empleados.
            Cmb_Empleados.DataSource = Dt_Empleados;
            Cmb_Empleados.DataTextField = "EMPLEADOS";
            Cmb_Empleados.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Empleados.DataBind();
            Cmb_Empleados.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Empleados.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar a los empleados por depèndencia. Error: [" + Ex.Message + "]");
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
            Botones.Add(Btn_Modificar);
            Botones.Add(Btn_Eliminar);
            Botones.Add(Btn_Busqueda_Tiempo_Extra_Empleado);

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

    #region (Grid)

    #region (Grid Empleados Pertencen Hora Extra Seleccionada)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tiempo_Extra_Empleado_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tiempo_Extra_Empleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Empleados"] != null)
            {
                LLenar_Grid_Empleados(e.NewPageIndex, (DataTable)Session["Dt_Empleados"]);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error cambiar de un de la tabla. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: LLenar_Grid_Empleados
    ///DESCRIPCIÓN: LLena el grid de Empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid_Empleados(Int32 Pagina, DataTable Tabla)
    {
        Grid_Tiempo_Extra_Empleado.SelectedIndex = (-1);
        Grid_Tiempo_Extra_Empleado.Columns[0].Visible = true;
        Grid_Tiempo_Extra_Empleado.DataSource = Tabla;
        Grid_Tiempo_Extra_Empleado.PageIndex = Pagina;
        Grid_Tiempo_Extra_Empleado.DataBind();
        Grid_Tiempo_Extra_Empleado.Columns[0].Visible = false;
        Session["Dt_Empleados"] = Tabla;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tiempo_Extra_Empleado_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion de 
    ///los empleados
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 17/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tiempo_Extra_Empleado_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((ImageButton)e.Row.Cells[2].FindControl("Btn_Eliminar_Empleado")).CommandArgument = e.Row.Cells[0].Text.Trim();
                ((ImageButton)e.Row.Cells[2].FindControl("Btn_Eliminar_Empleado")).ToolTip = "Quitar al Empleado " + HttpUtility.HtmlDecode(e.Row.Cells[1].Text);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion

    #region (Grid Horas Extra)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Horas_Extra_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la Pagina del Grid de Horas Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Horas_Extra_PageIndexChanging(object sender, GridViewPageEventArgs e) {
        try
        {
            Grid_Horas_Extra.PageIndex = e.NewPageIndex;
            Consultar_Horas_Extra();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cambiar la de pagina del Grid de Horas Extra. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Horas_Extra_RowDataBound
    ///DESCRIPCIÓN:
    ///PARAMETROS:  
    ///CREO:    Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 31/Marzo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Horas_Extra_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType.Equals(DataControlRowType.DataRow))
            {
                ((System.Web.UI.WebControls.ImageButton)e.Row.Cells[5].FindControl("Btn_Reporte_Tiempo_Extra")).ToolTip = "No_Hora_Extra:" + e.Row.Cells[1].Text;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Horas_Extra_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemnto de la tabla de Horas extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Horas_Extra_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Rs_Consulta_Ope_Horas_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del puesto
        Cls_Ope_Nom_Tiempo_Extra_Negocio Horas_Extra = null; 
        try
        {
            Rs_Consulta_Ope_Horas_Extra.P_No_Tiempo_Extra = Grid_Horas_Extra.Rows[Grid_Horas_Extra.SelectedIndex].Cells[1].Text.Trim();
            Horas_Extra = Rs_Consulta_Ope_Horas_Extra.Consultar_Tiempo_Extra("", "");

            Txt_No_Tiempo_Extra_Empleado.Text = Horas_Extra.P_No_Tiempo_Extra;
            Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf( Cmb_Dependencia.Items.FindByValue( Horas_Extra.P_Dependencia_ID ) );
            Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
            Txt_Fecha.Text = string.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Horas_Extra.P_Fecha));
            Cmb_Pago_Dia_Doble.SelectedIndex = Cmb_Pago_Dia_Doble.Items.IndexOf(Cmb_Pago_Dia_Doble.Items.FindByText(Horas_Extra.P_Pago_Dia_Doble));
            Txt_Horas.Text = "" + Horas_Extra.P_Horas;
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Horas_Extra.P_Estatus));
            Txt_Comentarios.Text = Horas_Extra.P_Comentarios;

            if (!string.IsNullOrEmpty(Horas_Extra.P_Nomina_ID))
            {
                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Horas_Extra.P_Nomina_ID));
                Consultar_Periodos_Catorcenales_Nomina(Horas_Extra.P_Nomina_ID);
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Horas_Extra.P_No_Nomina.ToString()));
            }

            LLenar_Grid_Empleados(0, Horas_Extra.P_Dt_Empleados);

            //Modulo de Autorizacion de Horas Extras
            if (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006"))
            {
                if (!Horas_Extra.P_Estatus.Trim().ToUpper().Equals("ACEPTADO"))
                {
                    Btn_Autorizacion_Horas_Extras.Visible = true;
                    Btn_Autorizacion_Horas_Extras.NavigateUrl = "Frm_Ope_Seguimiento_Tiempo_Extra.aspx?No_Hora_Extra=" 
                        + Grid_Horas_Extra.Rows[Grid_Horas_Extra.SelectedIndex].Cells[1].Text.Trim() + "&PAGINA=" + 
                        Request.QueryString["PAGINA"];
                }
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
    ///NOMBRE DE LA FUNCIÓN: Consultar_Horas_Extra
    ///DESCRIPCIÓN: Consulta las Horas Extra que existen en la BAse de Datos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Horas_Extra() {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Ope_Horas_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();
        try
        {
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) Ope_Horas_Extra.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.Trim();
            Ope_Horas_Extra.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();//"Pendiente";

            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                Ope_Horas_Extra.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();
            else Ope_Horas_Extra.P_Estatus = String.Empty;

            Grid_Horas_Extra.DataSource = Ope_Horas_Extra.Consultar_Tiempo_Extra("", "").P_Dt_Horas_Extra;
            Grid_Horas_Extra.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las horas extra existentes. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos Alta- Baja - Modificar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Hora Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
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
                Limpiar_Ctlrs();
                Habilitar_Controles("Nuevo");
            }
            else
            {
                if (Validar_Datos_Horas_Extra())
                {
                    Alta_Tiempo_Extra();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Modificar un Hora Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Modificar.ToolTip.Equals("Modificar"))
            {
                if (Grid_Horas_Extra.SelectedIndex != -1 & !Txt_No_Tiempo_Extra_Empleado.Text.Equals(""))
                {
                    //if (Cmb_Estatus.SelectedItem.Text.Trim().ToUpper().Equals("ACEPTADO"))
                    //{
                    //    Lbl_Mensaje_Error.Visible = true;
                    //    Img_Error.Visible = true;
                    //    Lbl_Mensaje_Error.Text = "El registro ya fea aceptado, ya no es posible realizar ninguna modificacion <br>";
                    //}
                    //else
                    //{
                    //if (Validar_Datos_Horas_Extra())
                    //{
                    Habilitar_Controles("Modificar");//Habilita la configuracion de los controles para ejecutar la operacion de modificar.
                    //}
                    //}
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea modificar sus datos <br>";
                }
            }
            else
            {
                if (Validar_Datos_Horas_Extra())
                {
                    Modificar_Tiempo_Extra();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Eliminar un Hora Extra
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            if (Btn_Eliminar.ToolTip.Equals("Eliminar"))
            {
                if (Grid_Horas_Extra.SelectedIndex != -1 & !Txt_No_Tiempo_Extra_Empleado.Text.Equals(""))
                {
                    Eliminar_Tiempo_Extra();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Salir de la Operacion Actual
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 17/Noviembre/2010 
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
                Session.Remove("Dt_Empleados");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Buscar_Empleados_Click
    /// DESCRIPCION : Busca al empleado por Nombre o Numero de Control
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          : Armando Zavala Moreno
    /// FECHA_MODIFICO    : 29/Marzo/2010
    /// CAUSA_MODIFICACION: Validar el Txt_Empleados para que cuando sea numeros solo admita 6
    ///*******************************************************************************
    protected void Btn_Buscar_Empleados_Click(object sender, ImageClickEventArgs e)
    {
        Buscar_Empleado();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Tiempo_Horas_Extra_Click
    /// DESCRIPCION : Ejecuta la Busqueda de Horas Extra Dadas de Alta en el Sistema
    /// por diferentes filtros. [No_Tiempo_Extra, Estatus, Dependencia y Fecha]
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Tiempo_Horas_Extra_Click(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
        Cls_Ope_Nom_Tiempo_Extra_Negocio _Tiempo_Horas_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();
        Cls_Ope_Nom_Tiempo_Extra_Negocio Cls_Resultante = null;

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text))                
            {
                INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_Busqueda_No_Empleado.Text);

                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                    _Tiempo_Horas_Extra.P_Empleado_ID = INF_EMPLEADO.P_Empleado_ID;
            }

            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Tiempo_Extra.Text.Trim())) Txt_Busqueda_No_Tiempo_Extra.Text = String.Format("{0:0000000000}", Convert.ToInt64(Txt_Busqueda_No_Tiempo_Extra.Text.Trim()));
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Tiempo_Extra.Text.Trim())) _Tiempo_Horas_Extra.P_No_Tiempo_Extra = Txt_Busqueda_No_Tiempo_Extra.Text.Trim();
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0) _Tiempo_Horas_Extra.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) _Tiempo_Horas_Extra.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue.Trim();
            if (Validar_Anios(Txt_Busqueda_Fecha_Inicio.Text.Trim(), Txt_Busqueda_Fecha_Fin.Text.Trim()))
            {
                Cls_Resultante = _Tiempo_Horas_Extra.Consultar_Tiempo_Extra(string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text.Trim())),
                        string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text.Trim())));
                Grid_Horas_Extra.DataSource = Cls_Resultante.P_Dt_Horas_Extra;
                Grid_Horas_Extra.DataBind();
            }
            else
            {
                Cls_Resultante = _Tiempo_Horas_Extra.Consultar_Tiempo_Extra("", "");
                Grid_Horas_Extra.DataSource = Cls_Resultante.P_Dt_Horas_Extra;
                Grid_Horas_Extra.DataBind();
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error producido al realizar la Busqueda. Error: [" + Ex.Message + "]"; 
        }
    }
    #endregion

    #region(Eventos Agregar y Quitar Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Empleado_Click
    /// DESCRIPCION : Evento que genera la peticion para agregar un nuevo Empleado del
    /// combo de empleados a la tabla de Empelados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Empleado_Click(object sender, EventArgs e)
    {
        if (Cmb_Empleados.SelectedIndex > 0)
        {
            if (Validar_No_Permitir_Empleados_Diferentes_Dependencias(Cmb_Empleados.SelectedValue.Trim()))
            {
                if (Session["Dt_Empleados"] != null)
                {
                    Agregar_Empleado((DataTable)Session["Dt_Empleados"], Grid_Tiempo_Extra_Empleado, Cmb_Empleados);
                }
                else
                {
                    DataTable Dt_Empleados = new DataTable();
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(System.String));
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(System.String));

                    Session["Dt_Empleados"] = Dt_Empleados;
                    Grid_Tiempo_Extra_Empleado.Columns[0].Visible = true;
                    Grid_Tiempo_Extra_Empleado.DataSource = (DataTable)Session["Dt_Empleados"];
                    Grid_Tiempo_Extra_Empleado.DataBind();
                    Grid_Tiempo_Extra_Empleado.Columns[0].Visible = false;

                    Agregar_Empleado(Dt_Empleados, Grid_Tiempo_Extra_Empleado, Cmb_Empleados);
                    Txt_Empleados.Text = "";
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No es posible agregar empleados de diferentes unidades responsables a una solicitud de incidencia.');", true);
            }
        }
        else
        {
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                "alert('No se a seleccionado ninguna percepcion a agregar');", true);
        }

        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Eliminar_Empleado_Click
    /// DESCRIPCION : Evento que genera la peticion para Quitar el Empleado seleccionado
    /// de la tabla de Empelados.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 17/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Eliminar_Empleado_Click(object sender, EventArgs e)
    {
        DataRow[] Renglones;
        DataRow Renglon;
        ImageButton Btn_Eliminar_Empleado = (ImageButton)sender;

        if (Session["Dt_Empleados"] != null)
        {
            Renglones = ((DataTable)Session["Dt_Empleados"]).Select(Cat_Empleados.Campo_Empleado_ID + "='" + Btn_Eliminar_Empleado.CommandArgument + "'");

            if (Renglones.Length > 0)
            {
                Renglon = Renglones[0];
                DataTable Tabla = (DataTable)Session["Dt_Empleados"];
                Tabla.Rows.Remove(Renglon);
                Session["Dt_Empleados"] = Tabla;
                Grid_Tiempo_Extra_Empleado.SelectedIndex = (-1);
                LLenar_Grid_Empleados(Grid_Tiempo_Extra_Empleado.PageIndex, Tabla);
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Se debe seleccionar de la tabla el Empleados a quitar";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }

        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
    }
    #endregion


    #region Reporte de cancelacion
    //Btn_Reporte_Tiempo_Extra_Click

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Reporte_Tiempo_Extra_Click
    /// DESCRIPCION : Evento que genera un reporte sencillo con la informacion del tiempo extra
    /// de la tabla de Empelados.
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 30/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Reporte_Tiempo_Extra_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Tiempo_Extra_Negocio Rs_Consulta_Ope_Horas_Extra = new Cls_Ope_Nom_Tiempo_Extra_Negocio();
        String Tiempo_Extra_ID = String.Empty;
        DataTable Dt_Reporte = new DataTable();
        Ds_Rpt_Tiempo_Extra_Sencillo Ds_Reporte = new Ds_Rpt_Tiempo_Extra_Sencillo();
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Tiempo_Extra_Estatus" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento
        
        try
        {
            ImageButton imageButton = (ImageButton)sender;
            TableCell tableCell = (TableCell)imageButton.Parent;
            GridViewRow row = (GridViewRow)tableCell.Parent;
            Grid_Horas_Extra.SelectedIndex = row.RowIndex;
            int fila = row.RowIndex;



            Tiempo_Extra_ID = Grid_Horas_Extra.Rows[fila].Cells[1].Text.Trim();
            Rs_Consulta_Ope_Horas_Extra.P_No_Tiempo_Extra = Tiempo_Extra_ID;
            Dt_Reporte = Rs_Consulta_Ope_Horas_Extra.Consultar_Mini_Reporte_Tiempo_Extra();

            Dt_Reporte.TableName = "Dt_Reporte";
            //  se llena el dataset
            Ds_Reporte.Clear();
            Ds_Reporte.Tables.Clear();
            Ds_Reporte.Tables.Add(Dt_Reporte.Copy());

            //  se carga el reporte
            Reporte.Load(Ruta_Archivo + "Cr_Rpt_Nom_Tiempo_Extra_Sencillo.rpt");
            Reporte.SetDataSource(Ds_Reporte);

            //  se genera el tipo del archivo y se muestra el reporte
            DiskFileDestinationOptions m_crDiskFileDestinationOptions = new DiskFileDestinationOptions();
            Nombre_Archivo += ".pdf";
            Ruta_Archivo = @Server.MapPath("../../Reporte/");
            m_crDiskFileDestinationOptions.DiskFileName = Ruta_Archivo + Nombre_Archivo;

            ExportOptions Opciones_Exportacion = new ExportOptions();
            Opciones_Exportacion.ExportDestinationOptions = m_crDiskFileDestinationOptions;
            Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
            Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Opciones_Exportacion);
            Abrir_Ventana(Nombre_Archivo);

            Grid_Horas_Extra.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
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
    #endregion

    #region (Eventos Combos)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Dependencias_SelectedIndexChanged
    /// DESCRIPCION : Selecciona una dependencia y carga el combo de empleados con
    /// los empleados que pertenecen a dicha dependencia.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 18/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Dependencia.SelectedIndex > 0)
            {
                Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
            }
            Cmb_Dependencia.Focus();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Cmb_Pago_Dia_Doble_SelectedIndexChanged
    /// DESCRIPCION : Selecciona si aplicá el pago día doble.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Enero/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Cmb_Pago_Dia_Doble_SelectedIndexChanged(object sender, EventArgs e) {
        String Opcion_Seleccionada = Cmb_Pago_Dia_Doble.SelectedItem.Text;

        try
        {
            if (Opcion_Seleccionada.Equals("SI"))
            {
                Txt_Horas.Enabled = false;
                Txt_Horas.Text = "";                
            }
            else if (Opcion_Seleccionada.Equals("NO"))
            {
                Txt_Horas.Enabled = true;
                Txt_Horas.Text = "";                
            }
            Cmb_Pago_Dia_Doble.Focus();
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Error generado al seleccionadar si aplica o no el Dia Doble. Error:" + Ex.Message + "]");
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
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Tiempo_Extra();", true);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Busqueda_Calendario_Nomina_SelectedIndexChanged
    ///DESCRIPCIÓN: Consulta los periodos catorcenales de la nomina seleccionada.
    ///CREO       : Juan Alberto Hernández Negrete
    ///FECHA_CREO : 06/Abril/2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Busqueda_Calendario_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int32 index = Cmb_Busqueda_Calendario_Nomina.SelectedIndex;
        if (index > 0)
        {
            Consultar_Busqueda_Periodos_Catorcenales_Nomina(Cmb_Busqueda_Calendario_Nomina.SelectedValue.Trim()); //Consulta los periodos nominales validos
        }
        else
        {
            Cmb_Periodos_Catorcenales_Nomina.DataSource = new DataTable();
            Cmb_Periodos_Catorcenales_Nomina.DataBind();
        }
        MPE_Msj.Show();
    }
    protected void Cmb_Busqueda_Periodos_Catorcenales_Nomina_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Int32 index = Cmb_Busqueda_Periodos_Catorcenales_Nomina.SelectedIndex;
            Txt_Busqueda_Fecha_Inicio.Text = "";
            Txt_Busqueda_Fecha_Fin.Text = "";
            if (index > 0)
            {
                Consulta_Fechas_Periodo_Nominal(); //Consulta la fecha de inicio y termino para la generación de asistencias del empleado

            }
            MPE_Msj.Show();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion
    #region (TextBox)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Empleados_TextChanged
    /// DESCRIPCION : Busca al empleado por Nombre o Numero de Control
    /// CREO        : Armando Zavala Moreno
    /// FECHA_CREO  : 30/Marzo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Empleados_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Buscar_Empleado();
            if (Cmb_Empleados.Items.Count > 1)
            {
                Cmb_Empleados.Focus();
            }
            else
            {
                Txt_Empleados.Text = string.Empty;
                Txt_Empleados.Focus();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al buscar el Empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion
    #endregion
}
