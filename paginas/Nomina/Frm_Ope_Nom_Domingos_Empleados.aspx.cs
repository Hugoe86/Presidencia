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
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Dependencias.Negocios;
using Presidencia.Empleados.Negocios;
using Presidencia.Domingos_Trabajados.Negocios;
using System.Globalization;
using System.Text.RegularExpressions;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using CrystalDecisions.Web;
using CrystalDecisions.ReportSource;

public partial class paginas_Nomina_Frm_Ope_Nom_Domingos_Empleados : System.Web.UI.Page
{
    #region (Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();
            }

            Btn_Autorizacion_Domingo_Trabajado.Visible = false;
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Inicial
    ///DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar
    ///             diferentes operaciones
    ///CREO       : Yazmin A Delgado Gómez
    ///FECHA_CREO : 29-Noviembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();            //Limpia los controles de la forma para las operaciones a realizar por parte del usuario
        Habilitar_Controles("Inicial"); //Habilita y deshabilita los contronles de la forma de acuerdo al tipo de operación que desea realizar el usuario
        Consultar_Dependencias();       //Consultas las dependencias que se encuentran activas en la base de datos
        Consultar_Busqueda_Dependencias();
        Consulta_Domingos_Trabajados(); //Consulta todos los domingos que fueron dados de alta
        Consultar_Calendarios_Nomina();
        Consultar_Busqueda_Calendarios_Nomina();//Consulta los calendarios de nómina que existen actualmente en el sistema.
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles de la forma para la realización de las 
    ///               operaciones
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 29-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_No_Domingo.Text = "";
            Cmb_Estatus_Domingo_Trabajado.SelectedIndex = -1;
            Txt_Fecha_Domingo_Trabajado.Text = "";
            Txt_Comentarios_Domingo_Trabajado.Text = "";
            Txt_Empleado_Domingo_Trabajado.Text = "";
            Cmb_Dependencia_Domingo_Trabajado.DataSource = null;
            Cmb_Empleado_Domingo_Trabajado.DataBind();
            Grid_Empleados_Domingos_Trabajado.DataSource = null;
            Grid_Empleados_Domingos_Trabajado.DataBind();
            if (Session["Empleados_Domingo_Trabajado"] != null) Session.Remove("Empleados_Domingo_Trabajado");
            if (Session["Consulta_Domingos_Trabajados"] != null) Session.Remove("Consulta_Domingos_Trabajados");

            //Cmb_Calendario_Nomina.SelectedIndex = -1;
            //Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///                para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 30-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va hacer habilitado para utilización del usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Tab_Dias_Domingos_Trabajados.ActiveTabIndex = 0;

                    Cmb_Busqueda_Estatus.SelectedIndex = Cmb_Busqueda_Estatus.Items.IndexOf(Cmb_Busqueda_Estatus.Items.FindByText("Pendiente"));

                    Configuracion_Acceso("Frm_Ope_Nom_Domingos_Empleados.aspx");
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

                    Tab_Dias_Domingos_Trabajados.ActiveTabIndex = 1;
                    Cmb_Estatus_Domingo_Trabajado.SelectedIndex = 1;
                    Cmb_Calendario_Nomina.Focus();
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

                    Tab_Dias_Domingos_Trabajados.ActiveTabIndex = 1;
                    Cmb_Calendario_Nomina.Focus();
                    break;
            }
            Txt_Fecha_Domingo_Trabajado.Enabled = Habilitado;
            Txt_Comentarios_Domingo_Trabajado.Enabled = Habilitado;
            Txt_Empleado_Domingo_Trabajado.Enabled = Habilitado;
            Cmb_Dependencia_Domingo_Trabajado.Enabled = Habilitado;
            Cmb_Empleado_Domingo_Trabajado.Enabled = Habilitado;
            Btn_Fecha_Domingo_Trabajado.Enabled = Habilitado;
            Btn_Agregar_Empleado_Domingo_Trabajado.Enabled = Habilitado;
            Btn_Buscar_Empleado_Domingo_Trabajado.Enabled = Habilitado;
            Btn_Busqueda_Domingo_Trabajado.Enabled = !Habilitado;
            Grid_Domingos_Trabajados.Enabled = !Habilitado;
            Grid_Empleados_Domingos_Trabajado.Enabled = Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;

            Cmb_Estatus_Domingo_Trabajado.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;
            Cmb_Dependencia_Domingo_Trabajado.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;
            Tr_Periodos_Fiscales.Visible = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;
            Cmb_Busqueda_Dependencia.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias
    /// DESCRIPCION : Consulta las Dependencias que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 29-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias()
    {
        DataTable Dt_Dependencias;//Variable a contener todas las dependencias dadas de alta
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias();
            Cmb_Dependencia_Domingo_Trabajado.DataSource = Dt_Dependencias;
            Cmb_Dependencia_Domingo_Trabajado.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Dependencia_Domingo_Trabajado.DataTextField = "CLAVE_NOMBRE";
            Cmb_Dependencia_Domingo_Trabajado.DataBind();
            Cmb_Dependencia_Domingo_Trabajado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Dependencia_Domingo_Trabajado.SelectedIndex = 0;

            if (Cls_Sessiones.Datos_Empleado != null)
            {
                if (Cls_Sessiones.Datos_Empleado.Rows.Count > 0)
                {
                    Cmb_Dependencia_Domingo_Trabajado.SelectedIndex = Cmb_Dependencia_Domingo_Trabajado.Items.IndexOf(Cmb_Dependencia_Domingo_Trabajado.Items.FindByValue(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias
    /// DESCRIPCION : Consulta las Dependencias que estan dadas de alta en la DB
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 29-Noviembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Busqueda_Dependencias()
    {
        DataTable Dt_Dependencias;//Variable a contener todas las dependencias dadas de alta
        Cls_Cat_Dependencias_Negocio Rs_Consulta_Cat_Dependencias = new Cls_Cat_Dependencias_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Dt_Dependencias = Rs_Consulta_Cat_Dependencias.Consulta_Dependencias();
            Cmb_Busqueda_Dependencia.DataSource = Dt_Dependencias;
            Cmb_Busqueda_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Busqueda_Dependencia.DataTextField = "CLAVE_NOMBRE";
            Cmb_Busqueda_Dependencia.DataBind();
            Cmb_Busqueda_Dependencia.Items.Insert(0, new ListItem("<- Seleccione ->", ""));
            Cmb_Busqueda_Dependencia.SelectedIndex = 0;

            if (Cls_Sessiones.Datos_Empleado != null)
            {
                if (Cls_Sessiones.Datos_Empleado.Rows.Count > 0)
                {
                    Cmb_Busqueda_Dependencia.SelectedIndex = Cmb_Busqueda_Dependencia.Items.IndexOf(Cmb_Busqueda_Dependencia.Items.FindByValue(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()));
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Dependencias " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Domingos_Trabajados
    /// DESCRIPCION : Consulta todos los domingos que estan dados de alta en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 15-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Domingos_Trabajados()
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;
        Cls_Ope_Nom_Domingos_Empleados_Negocios Rs_Consulta_Ope_Nom_Domingos = new Cls_Ope_Nom_Domingos_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Domingos; //Variable que obtendra los datos de la consulta 

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text))                
            {
                INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_Busqueda_No_Empleado.Text);

                if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                    Rs_Consulta_Ope_Nom_Domingos.P_Empleado_ID = INF_EMPLEADO.P_Empleado_ID;
            }

            if (Txt_Busqueda_No_Domingo.Text != "") Rs_Consulta_Ope_Nom_Domingos.P_No_Domingo = Convert.ToString(String.Format("{0:0000000000}", Convert.ToInt64(Txt_Busqueda_No_Domingo.Text)));
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
            {
                Rs_Consulta_Ope_Nom_Domingos.P_Estatus = Cmb_Busqueda_Estatus.SelectedValue;
            }
            else {
                Rs_Consulta_Ope_Nom_Domingos.P_Estatus = "Pendiente";
            }
            if (Cmb_Busqueda_Dependencia.SelectedIndex > 0) Rs_Consulta_Ope_Nom_Domingos.P_Dependencia_ID = Cmb_Busqueda_Dependencia.SelectedValue;
            
            if (Txt_Busqueda_Fecha_Inicio.Text != "" && Txt_Busqueda_Fecha_Fin.Text != "")
            {
                Rs_Consulta_Ope_Nom_Domingos.P_Fecha_Inicio = Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text);
                Rs_Consulta_Ope_Nom_Domingos.P_Fecha_Final = Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text);
            }
            Dt_Domingos = Rs_Consulta_Ope_Nom_Domingos.Consulta_Domingos_Trabajados(); //Consulta todos los domingos con sus datos generales
            Session["Consulta_Domingos_Trabajados"] = Dt_Domingos;
            Llena_Grid_Domingos_Trabajados();
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Domingos_Trabajados " + ex.Message.ToString(), ex);
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
    /// NOMBRE DE LA FUNCION: Llena_Grid_Domgos_Trabajados
    /// DESCRIPCION : Llena el grid con los domingos trabajados que se encuentran en la BD
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 04-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Domingos_Trabajados()
    {
        DataTable Dt_Domingos_Trabajdos; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Domingos_Trabajados.Columns[0].Visible = true; //Seleccionar
            Grid_Domingos_Trabajados.Columns[1].Visible = true; //No Domingos
            Grid_Domingos_Trabajados.Columns[2].Visible = true; //Fecha
            Grid_Domingos_Trabajados.Columns[3].Visible = true; //Dependencia ID
            Grid_Domingos_Trabajados.Columns[4].Visible = true; //Dependencia
            Grid_Domingos_Trabajados.Columns[5].Visible = true; //Estatus
            Grid_Domingos_Trabajados.Columns[6].Visible = true; //Comentarios
            Grid_Domingos_Trabajados.Columns[7].Visible = true;
            Grid_Domingos_Trabajados.Columns[8].Visible = true;
            Grid_Domingos_Trabajados.DataBind();
            Dt_Domingos_Trabajdos = (DataTable)Session["Consulta_Domingos_Trabajados"];
            Grid_Domingos_Trabajados.DataSource = Dt_Domingos_Trabajdos;
            Grid_Domingos_Trabajados.DataBind();

            Grid_Domingos_Trabajados.Columns[3].Visible = false; //Dependencia_ID
            Grid_Domingos_Trabajados.Columns[6].Visible = false; //Comentarios
            Grid_Domingos_Trabajados.Columns[7].Visible = false;
            Grid_Domingos_Trabajados.Columns[8].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Domingos_Trabajados " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Empleados_Domgos_Trabajados
    /// DESCRIPCION : Llena el grid con los empleados que trabajaron el domingo
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 04-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Empleados_Domingos_Trabajados()
    {
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta 
        try
        {
            Grid_Empleados_Domingos_Trabajado.Columns[0].Visible = true;
            Grid_Empleados_Domingos_Trabajado.DataBind();
            Dt_Empleados = (DataTable)Session["Empleados_Domingo_Trabajado"];
            Grid_Empleados_Domingos_Trabajado.DataSource = Dt_Empleados;
            Grid_Empleados_Domingos_Trabajado.DataBind();
            Grid_Empleados_Domingos_Trabajado.Columns[0].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados_Domingos_Trabajados " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validacion_Datos_Faltantes
    /// DESCRIPCION : Indica al usuario que datos son los que le falta introducir para
    ///               poder dar de alta o modificar los datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 04-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Validacion_Datos_Faltantes()
    {
        String Espacios = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;"; //Variable que contiene la cantidad de espacios a considerar para dejar de sangria a los mensajes
        try
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";
            if (Txt_Fecha_Domingo_Trabajado.Text == "")
            {
                Lbl_Mensaje_Error.Text += Espacios + " + La Fecha en que laboraron en Domingo <br>";
            }
            if (Grid_Empleados_Domingos_Trabajado.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Text += Espacios + " + Los empleados a los cuales se les desea asignar el Domingo Trabajado <br>";
            }
            if (Txt_Comentarios_Domingo_Trabajado.Text.Length > 250)
            {
                Lbl_Mensaje_Error.Text += Espacios + " + Los comentarios proporcionados no deben ser mayor a 250 caracteres <br>";
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Empleados_Domgos_Trabajados " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Domingos_Trabajados
    /// DESCRIPCION : Da de alta el domingo trabajado por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Domingos_Trabajados()
    {
        Cls_Ope_Nom_Domingos_Empleados_Negocios Rs_Alta_Ope_Nom_Domingos = new Cls_Ope_Nom_Domingos_Empleados_Negocios(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Ope_Nom_Domingos.P_Dependencia_ID = Convert.ToString(Cmb_Dependencia_Domingo_Trabajado.SelectedValue);
            Rs_Alta_Ope_Nom_Domingos.P_Fecha = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Domingo_Trabajado.Text.Trim());
            Rs_Alta_Ope_Nom_Domingos.P_Estatus = Cmb_Estatus_Domingo_Trabajado.SelectedItem.Text.Trim();
            Rs_Alta_Ope_Nom_Domingos.P_Comentarios = Convert.ToString(Txt_Comentarios_Domingo_Trabajado.Text);
            Rs_Alta_Ope_Nom_Domingos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Alta_Ope_Nom_Domingos.P_Detalles_Domingos_Empleados = (DataTable)Session["Empleados_Domingo_Trabajado"];

            if ((Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")))
            {
                Rs_Alta_Ope_Nom_Domingos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Rs_Alta_Ope_Nom_Domingos.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            }

            Rs_Alta_Ope_Nom_Domingos.Alta_Domingo_Trabajado(); //Da de alta los datos del Domingo Trabajado proporcionados por el usuario en la BD
            Configuracion_Inicial();                           //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Operación Domingos Trabajados", "alert('El Alta del Domingo Trabajado fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Alta_Domingos_Trabajados " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Domingo_Trabajado
    /// DESCRIPCION : Modifica los datos del registro del domingo con los datos proporcionados
    ///               por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 06-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Domingo_Trabajado()
    {
        Cls_Ope_Nom_Domingos_Empleados_Negocios Rs_Modificar_Ope_Nom_Domingos = new Cls_Ope_Nom_Domingos_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        try
        {
            Rs_Modificar_Ope_Nom_Domingos.P_No_Domingo = Convert.ToString(Txt_No_Domingo.Text);
            Rs_Modificar_Ope_Nom_Domingos.P_Dependencia_ID = Convert.ToString(Cmb_Dependencia_Domingo_Trabajado.SelectedValue);
            Rs_Modificar_Ope_Nom_Domingos.P_Fecha = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Domingo_Trabajado.Text.Trim());
            Rs_Modificar_Ope_Nom_Domingos.P_Estatus = Cmb_Estatus_Domingo_Trabajado.SelectedItem.Text.Trim();
            Rs_Modificar_Ope_Nom_Domingos.P_Comentarios = Convert.ToString(Txt_Comentarios_Domingo_Trabajado.Text);
            Rs_Modificar_Ope_Nom_Domingos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            Rs_Modificar_Ope_Nom_Domingos.P_Detalles_Domingos_Empleados = (DataTable)Session["Empleados_Domingo_Trabajado"];

            if ((Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")))
            {
                Rs_Modificar_Ope_Nom_Domingos.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
                Rs_Modificar_Ope_Nom_Domingos.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            }

            Rs_Modificar_Ope_Nom_Domingos.Modificar_Domingo_Trabajado(); //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            Configuracion_Inicial(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Operación Domingos Trabajados", "alert('La Modificación del Domingo Trabajado fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Domingo_Trabajado " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Domingo_Trabajado
    /// DESCRIPCION : Elimina el Domingo Trabajado que fue seleccionado por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 15-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Domingo_Trabajado()
    {
        Cls_Ope_Nom_Domingos_Empleados_Negocios Rs_Eliminar_Ope_Nom_Domingos = new Cls_Ope_Nom_Domingos_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios para la eliminación de los datos
        try
        {
            Rs_Eliminar_Ope_Nom_Domingos.P_No_Domingo = Convert.ToString(Txt_No_Domingo.Text);
            Rs_Eliminar_Ope_Nom_Domingos.Eliminar_Domingo_Trabajado(); //Elimina el Domingo Trabajado que selecciono el usuario de la BD
            Configuracion_Inicial(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Operación Domingos Trabajados", "alert('La Eliminación del Domingo Trabajado fue Exitosa');", true);
        }
        catch (Exception ex)
        {
            throw new Exception("Eliminar_Domingo_Trabajado " + ex.Message.ToString(), ex);
        }
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
        String Cadena_Fecha = @"^(0[1-9]|[12][0-9]|3[01])(0[1-9]|1[012])(19|20)\d\d$";
        if (Fecha != null) return Regex.IsMatch(Fecha, Cadena_Fecha);
        else return false;
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

            if (Grid_Empleados_Domingos_Trabajado.Rows.Count > 0)
            {
                foreach (GridViewRow Registro_Empleado in Grid_Empleados_Domingos_Trabajado.Rows)
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

    #region (Grid)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Domingos_Trabajados_SelectedIndexChanged
    /// DESCRIPCION : Consulta los datos del domingo seleccionado por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Domingos_Trabajados_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Empleados; //Variable que obtendra los datos de la consulta de los empleados pertenecientes al domingo seleccionado            
        Cls_Ope_Nom_Domingos_Empleados_Negocios Rs_Consulta_Ope_Nom_Domingos_Emp_Det = new Cls_Ope_Nom_Domingos_Empleados_Negocios(); //Variable de conexión a la capa de Negocios para la consulta de los datos del empleado

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            Grid_Domingos_Trabajados.Columns[3].Visible = true;
            Txt_No_Domingo.Text = Grid_Domingos_Trabajados.SelectedRow.Cells[1].Text;
            Txt_Fecha_Domingo_Trabajado.Text = String.Format("{0:ddMMyyyy}", Convert.ToDateTime(Grid_Domingos_Trabajados.SelectedRow.Cells[2].Text.ToString()));
            Cmb_Dependencia_Domingo_Trabajado.SelectedValue = Grid_Domingos_Trabajados.SelectedRow.Cells[3].Text;
            Cmb_Estatus_Domingo_Trabajado.SelectedIndex = Cmb_Estatus_Domingo_Trabajado.Items.IndexOf(Cmb_Estatus_Domingo_Trabajado.Items.FindByText(HttpUtility.HtmlDecode(Grid_Domingos_Trabajados.SelectedRow.Cells[5].Text)));
            Txt_Comentarios_Domingo_Trabajado.Text = HttpUtility.HtmlDecode(Grid_Domingos_Trabajados.SelectedRow.Cells[6].Text);
            Grid_Domingos_Trabajados.Columns[3].Visible = false;

            if (!string.IsNullOrEmpty(Grid_Domingos_Trabajados.SelectedRow.Cells[7].Text))
            {
                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Grid_Domingos_Trabajados.SelectedRow.Cells[7].Text));
                Consultar_Periodos_Catorcenales_Nomina(Grid_Domingos_Trabajados.SelectedRow.Cells[7].Text);
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Grid_Domingos_Trabajados.SelectedRow.Cells[8].Text));
            }

            if (Session["Empleados_Domingo_Trabajado"] != null) Session.Remove("Empleados_Domingo_Trabajado");
            Rs_Consulta_Ope_Nom_Domingos_Emp_Det.P_No_Domingo = Convert.ToString(Txt_No_Domingo.Text);
            Dt_Empleados = Rs_Consulta_Ope_Nom_Domingos_Emp_Det.Consulta_Empleados_Domingo_Trabajado(); //Consulta todos los empleados que pertenecen al domingo trabajado

            //Si el domingo seleccionado tiene empleados asignados entonces los muestra el grid
            if (Dt_Empleados.Rows.Count > 0)
            {
                Session["Empleados_Domingo_Trabajado"] = Dt_Empleados;
                Llena_Grid_Empleados_Domingos_Trabajados(); //Muestra los empleados que tiene asignado el domingo trabajado
            }

            if (!Grid_Domingos_Trabajados.SelectedRow.Cells[5].Text.Trim().ToUpper().Equals("ACEPTADO"))
            {
                Btn_Autorizacion_Domingo_Trabajado.Visible = true;
                Btn_Autorizacion_Domingo_Trabajado.NavigateUrl = "Frm_Ope_Nom_Seguimiento_Domingos.aspx?No_Domingo=" + Grid_Domingos_Trabajados.SelectedRow.Cells[1].Text.Trim() + "&PAGINA=" + Request.QueryString["PAGINA"];
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Domingos_Trabajados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Domingos_Trabajados.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            //Llena_Grid_Domingos_Trabajados(); //Carga los domingos trabajados de la página seleccionada por el usuario
            Consulta_Domingos_Trabajados();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Grid_Empleados_Domingos_Trabajado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Grid_Empleados_Domingos_Trabajado.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Empleados_Domingos_Trabajados(); //Carga los domingos trabajados de la página seleccionada por el usuario
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Empleados_Domingos_Trabajado_RowCommand
    /// DESCRIPCION : Elimina el empleado seleccionado por el usuario del grid
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 04-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Empleados_Domingos_Trabajado_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String Empleado_ID;                               //Obtiene el ID del empleado a Eliminar
        int Count_Fila;                                   //Obtiene el registro que se desea eliminar
        DataTable Dt_Empleados_Domingo = new DataTable(); //Obtiene los empleados que tienen el domingo asignado

        try
        {
            //Si selecciono el botón de eliminar entonces elimina el registro seleccionado por el usuario
            if (e.CommandName == "Eliminar_Empleado")
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si se esta danodo de alta o modificando los registro del domingo trabajado
                if (Btn_Nuevo.ToolTip != "Nuevo" || Btn_Modificar.ToolTip != "Modificar")
                {
                    if (Session["Empleados_Domingo_Trabajado"] != null)
                    {
                        //Hace a todos los datos del grid visibles para que los pueda visualizar el usuario
                        Grid_Empleados_Domingos_Trabajado.Columns[0].Visible = true; //Empleado_ID
                        Grid_Empleados_Domingos_Trabajado.Columns[1].Visible = true; //No_Empleado
                        Grid_Empleados_Domingos_Trabajado.Columns[2].Visible = true; //Empleado
                        Grid_Empleados_Domingos_Trabajado.Columns[3].Visible = true; //Estatus
                        Grid_Empleados_Domingos_Trabajado.Columns[4].Visible = true; //Botón Eliminar

                        Empleado_ID = Convert.ToString(e.CommandArgument);

                        Dt_Empleados_Domingo = (DataTable)Session["Empleados_Domingo_Trabajado"];

                        Count_Fila = -1;
                        //Verifica que el usuario no haya seleccionado una deducción que ya fue agregada previamente
                        foreach (DataRow Registro in Dt_Empleados_Domingo.Rows)
                        {
                            Count_Fila = Count_Fila + 1;//Indica que indice es el que se esta consultado del datatable
                            //Si el empleado seleccionado por el usuario ya fue agregado previamente entonces
                            //hace visible el mensaje al usuario para indicar que el empleado ya se encuentra
                            //agregado a la lista y no permite avanzar con las operaciones siguientes
                            if (Convert.ToString(Empleado_ID) == Convert.ToString(Registro[Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID]))
                            {
                                //Remueve el empleado que fue seleccionado por el usuario y el cual indico que desea eliminar de la lista
                                Dt_Empleados_Domingo.Rows.RemoveAt(Count_Fila);
                                Dt_Empleados_Domingo.AcceptChanges();

                                //Remueve la sesión con todos los empleados para poder asignar la nueva lista de los empleados
                                Session.Remove("Empleados_Domingo_Trabajado");
                                Session["Empleados_Domingo_Trabajado"] = null;
                                Session["Empleados_Domingo_Trabajado"] = Dt_Empleados_Domingo; //Asigna la nueva lista de los empleados

                                //Limpìa el grid para mostrar la lista actual de los empleados
                                Grid_Empleados_Domingos_Trabajado.DataSource = null;
                                Grid_Empleados_Domingos_Trabajado.DataBind();
                                //Asigna la lista de los empleados al grid para ser visualizados por el empleado
                                Grid_Empleados_Domingos_Trabajado.DataSource = Dt_Empleados_Domingo;
                                Grid_Empleados_Domingos_Trabajado.DataBind();
                                Grid_Empleados_Domingos_Trabajado.Columns[0].Visible = false; //Empleado_ID
                                return;
                            }
                        }
                    }
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
    #endregion

    #region (Eventos)
    protected void Cmb_Dependencia_Domingo_Trabajado_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cmb_Empleado_Domingo_Trabajado.DataBind();
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Buscar_Empleado_Domingo_Trabajado_Click
    /// DESCRIPCION : Consulta los empleados que pertenecen a la dependencia y que se
    ///               encuentren activos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 01-Diciembre-2010
    /// MODIFICO          : Armando Zavala Moreno   
    /// FECHA_MODIFICO    : 30/marzo/2012   
    /// CAUSA_MODIFICACION: Validar el Txt_Empleado_Domingo_Trabajado para que cuando sean
    ///                     numeros solo acepte 6 numeros, se agrego el metodo ya establecido
    ///                     por Juan Alberto Hernandez Negrete
    ///*******************************************************************************
    protected void Btn_Buscar_Empleado_Domingo_Trabajado_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Empleados; //Variable que va a contener todos los empleados que coincidieron con lo que proporciono el usuario
        Cls_Cat_Empleados_Negocios Rs_Consulta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";

            if (Txt_Empleado_Domingo_Trabajado.Text == "" || Cmb_Dependencia_Domingo_Trabajado.SelectedIndex <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                if (Cmb_Dependencia_Domingo_Trabajado.SelectedIndex <= 0) Lbl_Mensaje_Error.Text = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Proporcione la dependencia para poder realizar la busqueda <br>";
                if (Txt_Empleado_Domingo_Trabajado.Text == "") Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; Proporcione el nombre o No. Empleado para poder realizar la busqueda <br>";
                return;
            }
            else
            {
                //Asigna los valores que deseamos que cumplan los empleados para la realizacion de la consulta
                Rs_Consulta_Cat_Empleados.P_Dependencia_ID = Convert.ToString(Cmb_Dependencia_Domingo_Trabajado.SelectedValue); //Indica de que dependencia se desea consultar a los empleados
                Rs_Consulta_Cat_Empleados.P_Estatus = "ACTIVO"; //Asigna el estatus de los empleados que deseamos consultar de la dependencia

                if (Es_Numero(Txt_Empleado_Domingo_Trabajado.Text.Trim()))
                {
                    if (Txt_Empleado_Domingo_Trabajado.Text.Length > 6)
                        Txt_Empleado_Domingo_Trabajado.Text = String.Format("{0:000000}", Convert.ToDouble(Txt_Empleado_Domingo_Trabajado.Text.Trim().Substring(0, 5)));
                    else
                        Txt_Empleado_Domingo_Trabajado.Text = String.Format("{0:000000}", Convert.ToDouble(Txt_Empleado_Domingo_Trabajado.Text.Trim()));
                }
                Rs_Consulta_Cat_Empleados.P_Nombre = Convert.ToString(Txt_Empleado_Domingo_Trabajado.Text); //Indica ya sea el nombre, número de empleado o RFC a consultar del empleado
                Dt_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Empleados_Dependencia(); //Consulta a los empleados de la dependencia que fue seleccionada y que se encuentre activo

                //Asigna los valores de la consulta de los empleados obtenidos
                Cmb_Empleado_Domingo_Trabajado.DataSource = Dt_Empleados;
                Cmb_Empleado_Domingo_Trabajado.DataValueField = Cat_Empleados.Campo_Empleado_ID;
                Cmb_Empleado_Domingo_Trabajado.DataTextField = "Empleado";
                Cmb_Empleado_Domingo_Trabajado.DataBind();

                //Inserta en el primer registro la palabra seleccione para mostrar esta al usuario como primera instancia
                Cmb_Empleado_Domingo_Trabajado.Items.Insert(0, new ListItem("<- Seleccione ->", ""));

                if (IsNumeric(Txt_Empleado_Domingo_Trabajado.Text.Trim()))
                {
                    Cls_Cat_Empleados_Negocios INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(Txt_Empleado_Domingo_Trabajado.Text.Trim());
                    if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                        Cmb_Empleado_Domingo_Trabajado.SelectedIndex = Cmb_Empleado_Domingo_Trabajado.Items.IndexOf(
                            Cmb_Empleado_Domingo_Trabajado.Items.FindByValue(INF_EMPLEADO.P_Empleado_ID));
                }
                else
                {
                    Cls_Cat_Empleados_Negocios INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado_Nombre(Txt_Empleado_Domingo_Trabajado.Text.Trim());
                    if (!String.IsNullOrEmpty(INF_EMPLEADO.P_Empleado_ID))
                        Cmb_Empleado_Domingo_Trabajado.SelectedIndex = Cmb_Empleado_Domingo_Trabajado.Items.IndexOf(
                            Cmb_Empleado_Domingo_Trabajado.Items.FindByValue(INF_EMPLEADO.P_Empleado_ID));
                }

            }

            Btn_Agregar_Empleado_Domingo_Trabajado.Focus();

            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Empleados_Por_Dependencia
    /// DESCRIPCION : Consulta los empleados que pernecen a la dependencia que ha 
    /// sido pasada como parametro al metodo.
    /// PARÁMETROS: Dependencia_ID.- Es la dependencia seleccionada y de la cual se 
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
            _Cat_Empleados.P_Dependencia_ID = Dependencia_ID;
            Dt_Empleados = _Cat_Empleados.Consulta_Empleados_General();//Consulta los empleados.
            Cmb_Empleado_Domingo_Trabajado.DataSource = Dt_Empleados;
            Cmb_Empleado_Domingo_Trabajado.DataTextField = "EMPLEADOS";
            Cmb_Empleado_Domingo_Trabajado.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Empleado_Domingo_Trabajado.DataBind();
            Cmb_Empleado_Domingo_Trabajado.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Empleado_Domingo_Trabajado.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar a los empleados por depèndencia. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Empleado_Domingo_Trabajado_Click
    /// DESCRIPCION : Consulta que el empleado seleccionado por el usuario no este ya
    ///               asignado a la lista si no es así entonces consulta sus datos
    ///               generales y este registro lo agrega el grid para ser visualizado
    ///               por el usuario
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 02-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Empleado_Domingo_Trabajado_Click(object sender, EventArgs e)
    {
        DataTable Dt_Empleados_Domingo = new DataTable(); //Guarda los empleados que tiene el domingo asignado
        DataTable Dt_Empleados = new DataTable();         //Para obtener los datos del empleado seleccionado por el usuario            
        DataRow Renglon = null;                           //Renglon para el llenado de la tabla
        String Espacios = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Cls_Cat_Empleados_Negocios Rs_Consulta_Cat_Empleados = new Cls_Cat_Empleados_Negocios(); //Variable de conexión hacia la capa de Negocios            

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Verifica el usuario haya seleccionado un empleado
            if (Cmb_Empleado_Domingo_Trabajado.SelectedIndex > 0 && Cmb_Dependencia_Domingo_Trabajado.SelectedIndex > 0)
            {
                if (Validar_No_Permitir_Empleados_Diferentes_Dependencias(Cmb_Empleado_Domingo_Trabajado.SelectedValue.Trim()))
                {
                    //Si ya tiene el domingo empleados asignados entonces estos empleados
                    //se las asigna a la tabla para poder guardar el empleado que el usuario desea agregar
                    if (Session["Empleados_Domingo_Trabajado"] != null)
                    {
                        Dt_Empleados_Domingo = (DataTable)Session["Empleados_Domingo_Trabajado"];
                    }
                    //Si aun no se han agregado empleados entonces crea la estructura de la tabla para poder
                    //contener sus valores
                    else
                    {
                        //Crear la tabla de los empleados
                        Dt_Empleados_Domingo.Columns.Add(Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID, Type.GetType("System.String"));
                        Dt_Empleados_Domingo.Columns.Add(Cat_Empleados.Campo_No_Empleado, Type.GetType("System.String"));
                        Dt_Empleados_Domingo.Columns.Add("Empleado", Type.GetType("System.String"));
                        Dt_Empleados_Domingo.Columns.Add(Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus, Type.GetType("System.String"));
                    }
                    //Verifica que el usuario no haya seleccionado una deducción que ya fue agregada previamente
                    foreach (DataRow Registro in Dt_Empleados_Domingo.Rows)
                    {
                        //Si el empleado seleccionado por el usuario ya fue agregado previamente entonces
                        //hace visible el mensaje al usuario para indicar que el empleado ya se encuentra
                        //agregado a la lista y no permite avanzar con las operaciones siguientes
                        if (Convert.ToString(Cmb_Empleado_Domingo_Trabajado.SelectedValue) == Convert.ToString(Registro[Cat_Empleados.Campo_Empleado_ID]))
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = Espacios + " El Empleado seleccionado ya fue agregado a la lista <br>";
                            return;
                        }
                    }
                    //Consulta los datos generales del empleado que selecciono el usuario para poderlo agregar a la lista
                    Rs_Consulta_Cat_Empleados.P_Dependencia_ID = Cmb_Dependencia_Domingo_Trabajado.SelectedValue;
                    Rs_Consulta_Cat_Empleados.P_Empleado_ID = Convert.ToString(Cmb_Empleado_Domingo_Trabajado.SelectedValue);
                    Dt_Empleados = Rs_Consulta_Cat_Empleados.Consulta_Empleados_Dependencia();

                    //Crear renglon para agregarlo a la tabla y colocarlo en la variable de sesion
                    Renglon = Dt_Empleados_Domingo.NewRow(); //Crea un nuevo renglon
                    //Paso los valores a los campos correspondientes al renglon que se desea agregar a la tabla
                    Renglon[Ope_Nom_Domingos_Empleado_Detalles.Campo_Empleado_ID] = Dt_Empleados.Rows[0][Cat_Empleados.Campo_Empleado_ID].ToString();
                    Renglon[Cat_Empleados.Campo_No_Empleado] = Dt_Empleados.Rows[0][Cat_Empleados.Campo_No_Empleado].ToString();
                    Renglon["Empleado"] = Dt_Empleados.Rows[0]["Empleado"].ToString();
                    Renglon[Ope_Nom_Domingos_Empleado_Detalles.Campo_Estatus] = "ACEPTADO";
                    Dt_Empleados_Domingo.Rows.Add(Renglon);                        //Agrega el renglon a la tabla                    
                    Dt_Empleados_Domingo.AcceptChanges();                          //Para que acepte los cambios el 
                    Session["Empleados_Domingo_Trabajado"] = null;                 //Remueve los valores de la sesion para poder asignarle los valores actuales de los empleados asignados al domingo
                    Session["Empleados_Domingo_Trabajado"] = Dt_Empleados_Domingo; //Agrega a la sesión los valores actuales de los empleados
                    //Asigna los empleados que se tienen actualmente en el grid para poder mostrarlas al usuario
                    //Grid_Empleados_Domingos_Trabajado.DataBind();

                    //Hace a todos los datos del grid visibles para que los pueda visualizar el usuario
                    Grid_Empleados_Domingos_Trabajado.Columns[0].Visible = true; //Empleado_ID
                    Grid_Empleados_Domingos_Trabajado.Columns[1].Visible = true; //No_Empleado
                    Grid_Empleados_Domingos_Trabajado.Columns[2].Visible = true; //Empleado
                    Grid_Empleados_Domingos_Trabajado.Columns[3].Visible = true; //Estatus
                    Grid_Empleados_Domingos_Trabajado.Columns[4].Visible = true; //Botón Eliminar

                    //Asigna los registros al grid para ser vistos por el usuario
                    Grid_Empleados_Domingos_Trabajado.DataSource = (DataTable)Session["Empleados_Domingo_Trabajado"];//Dt_Empleados_Domingo;
                    Grid_Empleados_Domingos_Trabajado.DataBind();

                    Grid_Empleados_Domingos_Trabajado.Columns[0].Visible = false; //Empleado_ID
                    //Cmb_Dependencia_Domingo_Trabajado.Enabled = false;             //Deshabilita la dependencia para que el usuario no pudea agregar empleados de otra dependencia
                    Cmb_Empleado_Domingo_Trabajado.DataSource = null;
                    Cmb_Empleado_Domingo_Trabajado.DataBind();                     //Limpia la selección del empleado para que el usuario seleccione otro
                    Txt_Empleado_Domingo_Trabajado.Text = "";                      //Limpia el texto para que el usuario pueda consultar el nombre del siguiente empleado

                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No es posible agregar empleados de diferentes unidades responsables a una solicitud de incidencia.');", true);
                }

            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Espacios + " Seleccione al empleado que desea agregar <br>";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        DateTime Dia_Domingo = new DateTime();//Guarda la fecha seleccionada por el usuario
        DateTime Fecha_Diaria = DateTime.Now;
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Limpiar_Controles();           //Limpia los controles de la forma para poder introducir nuevos datos
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario                
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces da de alta los mismo en la base de datos
                if (Txt_Fecha_Domingo_Trabajado.Text != "" && Grid_Empleados_Domingos_Trabajado.Rows.Count > 0 &&
                    Txt_Comentarios_Domingo_Trabajado.Text.Length <= 250)
                {
                    Dia_Domingo = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Domingo_Trabajado.Text.Trim());
                    //Valida que el usuario haya seleccionado un día domingo
                    if (Convert.ToUInt32(Dia_Domingo.DayOfWeek) == 0)
                    {
                        //if (Dia_Domingo.CompareTo(Fecha_Diaria) == 1)
                        //{
                        //    Lbl_Mensaje_Error.Visible = true;
                        //    Img_Error.Visible = true;
                        //    Lbl_Mensaje_Error.Text = "La fecha proporcionada no debe ser mayor al día de hoy, favor de verificar";
                        //}
                        //else
                        //{
                            Alta_Domingos_Trabajados(); //Da de alta los datos proporcionados por el usuario
                        //}
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La fecha proporcionada no es correcta ya que no es un día domingo, favor de verificar";
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Validacion_Datos_Faltantes(); //Indica al usuario que datos son los que faltan de introducir para poder dar de alta
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        DateTime Dia_Domingo = new DateTime();//Guarda la fecha seleccionada por el usuario
        DateTime Fecha_Diaria = DateTime.Now;

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                if (Txt_No_Domingo.Text != "")
                {
                    Habilitar_Controles("Modificar"); //Habilita los controles para la modificación de los datos
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el Tipo de Nómina que desea modificar sus datos <br>";
                }
            }
            else
            {
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces modifica estos valores en la base de datos
                if (Txt_Fecha_Domingo_Trabajado.Text != "" && Grid_Empleados_Domingos_Trabajado.Rows.Count > 0 &&
                    Txt_Comentarios_Domingo_Trabajado.Text.Length <= 250)
                {
                    Dia_Domingo = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Domingo_Trabajado.Text.Trim());
                    //Valida que el usuario haya seleccionado un día domingo
                    if (Convert.ToUInt32(Dia_Domingo.DayOfWeek) == 0)
                    {
                        if (Cmb_Estatus_Domingo_Trabajado.SelectedIndex == 2)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "El registro ya fea aceptado, ya no es posible realizar ninguna modificacion <br>";
                        }
                        else
                        {
                            Modificar_Domingo_Trabajado(); //Modifica los datos del domingo trabajado
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La fecha proporcionada no es correcta ya que no es un día domingo, favor de verificar";
                    }
                }
                //Si faltaron campos por capturar envia un mensaje al usuario indicando que campos faltaron de proporcionar
                else
                {
                    Validacion_Datos_Faltantes(); //Indica al usuario que datos son los que faltan de introducir para poder modificar el registro
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Btn_Eliminar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Si el usuario selecciono un Domingo Trabajado entonces la elimina de la base de datos
            if (Txt_No_Domingo.Text != "")
            {
                Eliminar_Domingo_Trabajado(); //Elimina el domingo Trabajado que fue seleccionada por el usuario
            }
            //Si el usuario no selecciono algún Domingo manda un mensaje indicando que es necesario que seleccione alguna para
            //poder eliminar
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Seleccione el Domingo Trabajado que desea eliminar <br>";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Reporte_Prima_Dominical_Click
    /// DESCRIPCION : Evento que genera un reporte sencillo con la informacion de la prima dominical
    /// de la tabla de Empelados.
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 02/Abril/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Reporte_Prima_Dominical_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Domingos_Empleados_Negocios Prima_Dominical_Consulta = new Cls_Ope_Nom_Domingos_Empleados_Negocios();
        String No_Domingo_ID = String.Empty;
        DataTable Dt_Reporte = new DataTable();
        Ds_Rpt_Tiempo_Extra_Sencillo Ds_Reporte = new Ds_Rpt_Tiempo_Extra_Sencillo();
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = @Server.MapPath("../Rpt/Nomina/");//Obtiene la ruta en la cual será guardada el archivo
        String Nombre_Archivo = "Reporte_Dias_Festivos_Estatus" + Session.SessionID; //Obtiene el nombre del archivo que sera asignado al documento

        try
        {
            ImageButton imageButton = (ImageButton)sender;
            TableCell tableCell = (TableCell)imageButton.Parent;
            GridViewRow row = (GridViewRow)tableCell.Parent;
            Grid_Domingos_Trabajados.SelectedIndex = row.RowIndex;
            int fila = row.RowIndex;

            No_Domingo_ID = Grid_Domingos_Trabajados.Rows[fila].Cells[1].Text.Trim();
            Prima_Dominical_Consulta.P_No_Domingo = No_Domingo_ID;
            Dt_Reporte = Prima_Dominical_Consulta.Consultar_Reporte_Prima_Dominical();

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

            Grid_Domingos_Trabajados.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al selecionar una dependencia. Error: [" + Ex.Message + "]");
        }
    }


    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Domingos_Trabajados_Click
    /// DESCRIPCION : Ejecuta la Busqueda de los domingos trabajados y que estan dados
    ///               dados de alta en la base de datos de acuerdo a los valores 
    ///               proporcionados por el usuario
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 15-Diciembre-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Domingos_Trabajados_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Domingos_Empleados_Negocios Rs_Consulta_Ope_Nom_Domingos = new Cls_Ope_Nom_Domingos_Empleados_Negocios();

        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //Valida las fechas que introdujo el usuario para las consultas de domingos trabajados
            if (!string.IsNullOrEmpty(Txt_Busqueda_Fecha_Inicio.Text.Trim()) && !string.IsNullOrEmpty(Txt_Busqueda_Fecha_Fin.Text.Trim()))
            {
                //Valida que el usuario haya introducido una fecha correcta de inicio para la consulta
                if (Validar_Formato_Fecha(Convert.ToString(Txt_Busqueda_Fecha_Inicio.Text)))
                {
                    //Valida que el usuario haya introducido una fecha correcta de fin para la consulta
                    if (Validar_Formato_Fecha(Convert.ToString(Txt_Busqueda_Fecha_Fin.Text)))
                    {
                        DateTime Fecha_Inicio = new DateTime();//Guarda la fecha de inicio seleccionada por el usuario
                        DateTime Fecha_Final = new DateTime(); //Guarda la fecha de fin seleccionada por el usuario
                        DateTime Fecha_Diaria = DateTime.Now;  //Indica la fecha del día de hoy

                        Fecha_Inicio = Convert.ToDateTime(Txt_Busqueda_Fecha_Inicio.Text);
                        Fecha_Final = Convert.ToDateTime(Txt_Busqueda_Fecha_Fin.Text);
                        //Si la fecha de inicio es mayor a la fecha final entonces manda un mensaje al usuario
                        if (Fecha_Inicio.CompareTo(Fecha_Final) == 1)
                        {
                            Lbl_Mensaje_Error.Visible = true;
                            Img_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "La Fecha de Inicio de la Busqueda no puede ser mayor a la Fecha Final, favor de verificar <br>";
                            return;
                        }
                        else
                        {
                            if (Fecha_Final.CompareTo(Fecha_Diaria) == 1)
                            {
                                Lbl_Mensaje_Error.Visible = true;
                                Img_Error.Visible = true;
                                Lbl_Mensaje_Error.Text = "La Fecha Final de la Busqueda no puede ser mayor al día de hoy, favor de verificar <br>";
                                return;
                            }
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "La Fecha Final de la Busqueda no es correcta, favor de verificar <br>";
                        return;
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "La Fecha Inicial de la Busqueda no es correcta, favor de verificar <br>";
                    return;
                }
            }
            Consulta_Domingos_Trabajados(); //Consulta los domingos trabajados que coincidan con los parámetros porporcionados por el usuario
            Limpiar_Controles();            //Limpia los controles de la forma
            //Si no se encontraron Domingos Trabajados con los parámetros seleccionados manda un mensaje al usuario
            if (Grid_Domingos_Trabajados.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Domingos Trabajados con los datos proporcionados <br>";
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error producido al realizar la Busqueda. Error: [" + Ex.Message + "]";
        }
    }
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Session.Remove("Empleados_Domingo_Trabajado");
            if (Btn_Salir.ToolTip == "Salir")
            {
                Session.Remove("Consulta_Domingos_Trabajados");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
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
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Domingos();", true);
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
            Int32 index = Cmb_Periodos_Catorcenales_Nomina.SelectedIndex;
            Txt_Busqueda_Fecha_Inicio.Text = "";
            Txt_Busqueda_Fecha_Fin.Text = "";
            if (index > 0)
            {
                Consulta_Fechas_Periodo_Nominal(); //Consulta la fecha de inicio y termino para la generación de asistencias del empleado
                Txt_Busqueda_Fecha_Inicio.Focus();

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
            Botones.Add(Btn_Busqueda_Domingo_Trabajado);

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

}
