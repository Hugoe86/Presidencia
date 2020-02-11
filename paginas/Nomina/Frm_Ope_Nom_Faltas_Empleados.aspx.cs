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
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using System.Text.RegularExpressions;
using Presidencia.Empleados.Negocios;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using Presidencia.Puestos.Negocios;
using Presidencia.Utilidades_Nomina;
using Presidencia.Dependencias.Negocios;

using Presidencia.Ayudante_Informacion;

public partial class paginas_Nomina_Frm_Ope_Nom_Faltas_Empleados : System.Web.UI.Page
{
    #region (Load)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Carga la configuracion que tendra la pagina al terminarse de
    ///cargar.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 12/Noviembre/2010 12:10pm
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
                Configuracion_Inicial();
                ViewState["SortDirection"] = "ASC";
            }
        }
        catch (Exception Ex) 
        {            
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = Ex.Message.ToString();
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
    ///DESCRIPCIÓN: Configuracion Inicial del Formulario de Faltas de Empleados
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 12/Noviembre/2010 12:10pm
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Ctlrs();
        Consultar_Dependencias();
        Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
        Habilitar_Controles("Inicial");
        Consultar_Calendarios_Nomina();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010 12:20pm
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Ctlrs()
    {
        Txt_No_Falta_Empleado.Text = "";
        //Cmb_Dependencia.SelectedIndex = -1;
        Cmb_Empleados.SelectedIndex = -1;
        Cmb_Tipo_Falta_Empleado.SelectedIndex = -1;
        Txt_Fecha_Falta_Empleado.Text = "";
        Cmb_Retardo_Empleado.SelectedIndex = 2;
        Txt_Cantidad_Minutos_Retardo_Empleado.Text = "";
        Txt_Comentarios.Text = "";
        Grid_Faltas_Empleado.DataSource = new DataTable();
        Grid_Faltas_Empleado.DataBind();
        Grid_Faltas_Empleado.SelectedIndex = -1;

        //Cmb_Calendario_Nomina.SelectedIndex = -1;
        //Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;
        Txt_Cantidad_Descontar.Text = String.Empty;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlrs_Select
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010 12:20pm
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Ctlrs_Select()
    {
        Txt_No_Falta_Empleado.Text = "";
        //Cmb_Dependencia.SelectedIndex = -1;
        Cmb_Empleados.SelectedIndex = -1;
        Cmb_Tipo_Falta_Empleado.SelectedIndex = -1;
        Txt_Fecha_Falta_Empleado.Text = "";
        Cmb_Retardo_Empleado.SelectedIndex = -1;
        Txt_Cantidad_Minutos_Retardo_Empleado.Text = "";
        Txt_Comentarios.Text = "";

        //Cmb_Calendario_Nomina.SelectedIndex = -1;
        //Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = -1;

        Txt_Cantidad_Minutos_Retardo_Empleado.Text = String.Empty;
        Txt_Cantidad_Descontar.Text = String.Empty;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Habilitar_Controles
    /// DESCRIPCION : Habilita y Deshabilita los controles de la forma para prepara la página
    ///               para a siguiente operación
    /// PARAMETROS  : Operacion: Indica la operación que se desea realizar por parte del usuario
    ///                          si es una alta, modificacion
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado;

        try
        {
            Habilitado = false;
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

                    Txt_Busqueda_Falta_Empleado.Enabled = true;
                    Txt_Busqueda_Falta_Empleado.Text = "";
                    Btn_Busqueda_Faltas_Empleado.Enabled = true;

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;

                    //Cmb_Dependencia.Enabled = true;
                    Cmb_Empleados.Enabled = true;
                    Cmb_Empleados.Focus();
                    Configuracion_Acceso("Frm_Ope_Nom_Faltas_Empleados.aspx");
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

                    Txt_Busqueda_Falta_Empleado.Enabled = true;
                    Txt_Busqueda_Falta_Empleado.Text = "";
                    Btn_Busqueda_Faltas_Empleado.Enabled = true;

                    //Cmb_Dependencia.Enabled = true;
                    Cmb_Empleados.Enabled = true;
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

                    Txt_Busqueda_Falta_Empleado.Enabled = true;
                    Txt_Busqueda_Falta_Empleado.Text = "";
                    Btn_Busqueda_Faltas_Empleado.Enabled = true;

                    //Cmb_Dependencia.Enabled = false;
                    Cmb_Empleados.Enabled=false;
                    Cmb_Calendario_Nomina.Focus();
                    break;
            }

            Txt_No_Falta_Empleado.Enabled = false;
            Cmb_Tipo_Falta_Empleado.Enabled=Habilitado;
            Txt_Fecha_Falta_Empleado.Enabled=Habilitado;
            Cmb_Retardo_Empleado.Enabled = Habilitado;

            if(Cmb_Retardo_Empleado.SelectedIndex == 1)Txt_Cantidad_Minutos_Retardo_Empleado.Enabled = true;
            else Txt_Cantidad_Minutos_Retardo_Empleado.Enabled = false;

            Txt_Comentarios.Enabled = Habilitado;
            Grid_Faltas_Empleado.Enabled = !Habilitado;

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;

            Cmb_Dependencia.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;
            Tr_Periodos_Fiscales.Visible = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Falta_Empleado
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Falta_Empleado()
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Consultar_Periodos = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        DataTable Dt_Periodos_Validos = null;
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Dependencia.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione Dependencia <br>";
            Datos_Validos = false;
        }
        if (Cmb_Empleados.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione Empleado <br>";
            Datos_Validos = false;
        }
        if (Cmb_Tipo_Falta_Empleado.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione el Tipo de Falta <br>";
            Datos_Validos = false;
        }

        if (string.IsNullOrEmpty(Txt_Fecha_Falta_Empleado.Text))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Fecha Falta del Empleado <br>";
            Datos_Validos = false;
        }
        else if (!Validar_Formato_Fecha(Txt_Fecha_Falta_Empleado.Text.Trim()))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Formato  Incorrecto de Fecha Falta del Empleado <br>";
            Datos_Validos = false;
        }
        else if (!(Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Falta_Empleado.Text.Trim()).Year == DateTime.Now.Year))
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La fecha de la falta no puede ser de un año diferente al de la nomina actual <br>";
            Datos_Validos = false;
        }
        else
        {
            Consultar_Periodos.P_Fecha = string.Format("{0:dd/MM/yyyy}", DateTime.Today);
            Dt_Periodos_Validos = Consultar_Periodos.Consultar_Periodo_Por_Fecha();

            if (Dt_Periodos_Validos != null)
            {
                if (Dt_Periodos_Validos.Rows.Count > 0)
                {
                    DateTime Fecha_Inicia_Actual = Convert.ToDateTime(Dt_Periodos_Validos.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Inicio].ToString().Trim());
                    DateTime Fecha_Fin_Actual = Convert.ToDateTime(Dt_Periodos_Validos.Rows[0][Cat_Nom_Nominas_Detalles.Campo_Fecha_Fin].ToString().Trim());
                    DateTime Fecha_Inicia_Anterior = Fecha_Inicia_Actual.AddDays(-28);
                    DateTime Fecha_Fin_Anterior = Fecha_Inicia_Actual;
                    DateTime Fecha_Seleccionada = Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Falta_Empleado.Text.Trim());
                    DateTime Fecha_Actual = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 23, 59, 0);

                    if ((Fecha_Seleccionada >= Fecha_Inicia_Actual) && (Fecha_Seleccionada <= Fecha_Actual))
                    {
                        Datos_Validos = true;
                    }
                    else if ((Fecha_Seleccionada >= Fecha_Inicia_Anterior) && (Fecha_Seleccionada <= Fecha_Fin_Anterior))
                    {
                        Datos_Validos = true;
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Las fecha de faltas son validas solo al dia de hoy de la catorcena actual o en la catorcena anterior. <br>";
                        Datos_Validos = false;
                    }
                }
            }
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

        //if (Cmb_Retardo_Empleado.SelectedIndex == 2) {
        //    if (Cmb_Tipo_Falta_Empleado.SelectedIndex == 1) {
        //        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Si no es un retardo el tipo de falta no puede ser de tipo retardo. <br />";
        //        Datos_Validos = false;
        //    }
        //}

        return Datos_Validos;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Fecha_Falta 
    /// DESCRIPCION : Validar la fecha de la falta.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Validar_Fecha_Falta(String Fecha)
    {
        try
        {
            DateTime Fecha_Falta = Convert.ToDateTime(Fecha);

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Numero
    /// DESCRIPCION : Valida el numero
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 25/Octubre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    public Boolean Validar_Numero(String cadena)
    {
        Boolean Es_Numero = false;
        try
        {
            Convert.ToInt32(cadena);
            Es_Numero = true;
        }
        catch (Exception Ex)
        {
            if (!Ex.Message.Equals(""))
            {
                Es_Numero = false;
            }
        }
        return Es_Numero;
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

                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(
                        Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(
                        new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
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
    /// ***********************************************************************************
    /// Nombre: Ejecutar_Evento
    /// 
    /// Descripción: Metodo que sirve como puente para ejecutar la misma operacion por dos eventos
    ///              de ctrl diferentes.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 16/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************
    protected void Ejecutar_Evento()
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Ope_Faltas_Empleado = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

        try
        {
            Limpiar_Ctlrs();
            Cmb_Empleados.SelectedIndex = -1;

            if (Validar_Numero(Txt_Busqueda_Falta_Empleado.Text.Trim()))
            {
                Txt_Busqueda_Falta_Empleado.Text = string.Format("{0:000000}", Convert.ToInt64(Txt_Busqueda_Falta_Empleado.Text.Trim()));

                INF_EMPLEADO = Consultar_Informacion_Empleado(String.Empty, Txt_Busqueda_Falta_Empleado.Text.Trim(), String.Empty);
                Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(INF_EMPLEADO.P_Dependencia_ID));
                Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
                Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(INF_EMPLEADO.P_Empleado_ID));
                Consultar_Faltas_Empleado(Cmb_Empleados.SelectedValue.Trim());
            }
            else
            {
                INF_EMPLEADO = Consultar_Informacion_Empleado(String.Empty, String.Empty, Txt_Busqueda_Falta_Empleado.Text.Trim());
                Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(INF_EMPLEADO.P_Dependencia_ID));
                Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
                Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(INF_EMPLEADO.P_Empleado_ID));
                Consultar_Faltas_Empleado(Cmb_Empleados.SelectedValue.Trim());
            }

            if (Cmb_Empleados.SelectedIndex == -1 || Cmb_Empleados.SelectedIndex == 0)
            {
                if (!string.IsNullOrEmpty(Txt_Busqueda_Falta_Empleado.Text.Trim()))
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No se encontraron resultados en la busqueda";
                    Grid_Faltas_Empleado.DataSource = new DataTable();
                    Grid_Faltas_Empleado.DataBind();
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al ejecutar el metodo que sirve como puente para ejecutar la misma operacion por dos eventos. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region (Metodos Operacion)
    private void Alta_Falta_Empelado()
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Ope_Faltas_Empleados = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        try
        {
            Ope_Faltas_Empleados.P_Dependencia_ID = Cmb_Dependencia.SelectedValue.Trim();
            Ope_Faltas_Empleados.P_Empleado_ID = Cmb_Empleados.SelectedValue.Trim();
            Ope_Faltas_Empleados.P_Tipo_Falta = Cmb_Tipo_Falta_Empleado.SelectedItem.Text.Trim();
            //Ope_Faltas_Empleados.P_Fecha = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Falta_Empleado.Text.Trim()));
            Ope_Faltas_Empleados.P_Fecha = string.Format("{0:dd/MM/yyyy}", Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Falta_Empleado.Text.Trim()));
            Ope_Faltas_Empleados.P_Retardo = Cmb_Retardo_Empleado.SelectedItem.Text.Trim();
            Ope_Faltas_Empleados.P_Cantidad = (Txt_Cantidad_Minutos_Retardo_Empleado.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Cantidad_Minutos_Retardo_Empleado.Text));
            Ope_Faltas_Empleados.P_Comentarios= Txt_Comentarios.Text;
            Ope_Faltas_Empleados.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Ope_Faltas_Empleados.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Ope_Faltas_Empleados.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());

            if (Ope_Faltas_Empleados.Alta_Falta_Empleado())
            {
                //Configuracion_Inicial();
                //Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Alta Falta del Empleado]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta de una Falta para el Empleado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Falta_Empelado
    /// DESCRIPCION : Ejecuta la Actualizacion de una Falta del Empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Falta_Empelado()
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Ope_Faltas_Empleados = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        try
        {
            Ope_Faltas_Empleados.P_No_Falta = Txt_No_Falta_Empleado.Text;
            Ope_Faltas_Empleados.P_Tipo_Falta = Cmb_Tipo_Falta_Empleado.SelectedItem.Text.Trim();
            //Ope_Faltas_Empleados.P_Fecha = string.Format("{0:dd/MM/yyyy}", Convert.ToDateTime(Txt_Fecha_Falta_Empleado.Text.Trim()));
            Ope_Faltas_Empleados.P_Fecha = string.Format("{0:dd/MM/yyyy}", Presidencia.Fechas.Cls_Fechas.Obtener_Fecha(Txt_Fecha_Falta_Empleado.Text.Trim()));
            Ope_Faltas_Empleados.P_Retardo = Cmb_Retardo_Empleado.SelectedItem.Text.Trim();
            Ope_Faltas_Empleados.P_Cantidad = (Txt_Cantidad_Minutos_Retardo_Empleado.Text.Equals("") ? 0 : Convert.ToDouble(Txt_Cantidad_Minutos_Retardo_Empleado.Text));
            Ope_Faltas_Empleados.P_Comentarios = Txt_Comentarios.Text;
            Ope_Faltas_Empleados.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Ope_Faltas_Empleados.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Ope_Faltas_Empleados.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());

            if (Ope_Faltas_Empleados.Modificar_Falta_Empleado())
            {
                //Configuracion_Inicial();
                //Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Modificar Falta del Empleado]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Modifiacar la Falta de un Empleado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Falta_Empelado
    /// DESCRIPCION : Ejecuta la Actualizacion de una Falta del Empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Falta_Empelado()
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Ope_Faltas_Empleados = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        try
        {
            Ope_Faltas_Empleados.P_No_Falta = Txt_No_Falta_Empleado.Text;
            if (Ope_Faltas_Empleados.Eliminar_Falta_Empleado())
            {
                //Configuracion_Inicial();
                //Limpiar_Ctlrs();
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Falta del Empleado]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Modifiacar la Falta de un Empleado. Error: [" + Ex.Message + "]");
        }
    }
    #endregion 

    #region (Metodos Consulta)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Dependencias
    /// DESCRIPCION : Consultar las Dependencias
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias() {
        Cls_Cat_Dependencias_Negocio Obj_Dependencias = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Dependencias;
        try
        {
            Dt_Dependencias = Obj_Dependencias.Consulta_Dependencias();
            Cmb_Dependencia.DataSource = Dt_Dependencias;
            Cmb_Dependencia.DataTextField = "CLAVE_NOMBRE";
            Cmb_Dependencia.DataValueField = Cat_Dependencias.Campo_Dependencia_ID;
            Cmb_Dependencia.DataBind();
            Cmb_Dependencia.Items.Insert(0, new ListItem("< Seleccione >", ""));
            if (Cls_Sessiones.Datos_Empleado != null)
            {
                if (Cls_Sessiones.Datos_Empleado.Rows.Count > 0)
                {
                    Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Dependencia_ID].ToString()));
                }
            }
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
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Empleados_Por_Dependencia(String Dependencia_ID)
    {
        Cls_Cat_Empleados_Negocios Ope_Faltas_Empleado = new Cls_Cat_Empleados_Negocios();
        DataTable Dt_Empleados = null;
        try
        {
            Ope_Faltas_Empleado.P_Dependencia_ID = Dependencia_ID;
            Dt_Empleados = Ope_Faltas_Empleado.Consulta_Empleados_General();
            Cmb_Empleados.DataSource = Dt_Empleados;
            Cmb_Empleados.DataTextField = "EMPLEADOS";
            Cmb_Empleados.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Empleados.DataBind();
            Cmb_Empleados.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Empleados.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Dependencias. Error: [" + Ex.Message + "]");
        }
    }
    /// ***********************************************************************************
    /// Nombre: Consultar_Informacion_Empleado
    /// 
    /// Descripción: Consulta la información general del empleado.
    /// 
    /// Parámetros: No_Empleado.- Identificador interno del sistema para las operaciones que
    ///                           se realizan sobre los empelados.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 16/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************
    protected Cls_Cat_Empleados_Negocios Consultar_Informacion_Empleado(String Empleado_ID, String No_Empleado, String Nombre)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = new Cls_Cat_Empleados_Negocios();//Variable que almacenara la información del empleado.
        DataTable Dt_Empleado = null;//Variable que almacena el registro búscado del empleado.

        try
        {
            Obj_Empleados.P_Empleado_ID = Empleado_ID;
            Obj_Empleados.P_No_Empleado = No_Empleado;
            Obj_Empleados.P_Nombre = Nombre;
            Dt_Empleado = Obj_Empleados.Consulta_Empleados_General();//Consultamos la información del empleado.

            if (Dt_Empleado is DataTable)
            {
                if (Dt_Empleado.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Tipo_Nomina_ID = EMPLEADO[Cat_Empleados.Campo_Tipo_Nomina_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim()))
                                INF_EMPLEADO.P_No_Empleado = EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Zona_ID = EMPLEADO[Cat_Empleados.Campo_Zona_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Terceros_ID = EMPLEADO[Cat_Empleados.Campo_Terceros_ID].ToString().Trim();

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim()))
                                INF_EMPLEADO.P_Salario_Diario = Convert.ToDouble(EMPLEADO[Cat_Empleados.Campo_Salario_Diario].ToString().Trim());

                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim()))
                                INF_EMPLEADO.P_Dependencia_ID = EMPLEADO[Cat_Empleados.Campo_Dependencia_ID].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las información del empleado. Error: [" + Ex.Message + "]");
        }
        return INF_EMPLEADO;
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
            Botones.Add(Btn_Busqueda_Faltas_Empleado);

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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Faltas_Empleados
    /// DESCRIPCION : Consulta las Faltas del Empleado Seleccionado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Faltas_Empleado(String Empleado_ID) {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Ope_Faltas_Empleado = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        DataTable Dt_Empleado;
        try
        {
            Grid_Faltas_Empleado.Columns[2].Visible = true;
            Grid_Faltas_Empleado.Columns[3].Visible = true;
            Grid_Faltas_Empleado.Columns[4].Visible = true;
            Grid_Faltas_Empleado.Columns[6].Visible = true;
            Grid_Faltas_Empleado.Columns[7].Visible = true;
            Grid_Faltas_Empleado.Columns[8].Visible = false;
            Grid_Faltas_Empleado.Columns[9].Visible = true;
            Grid_Faltas_Empleado.Columns[10].Visible = true;
            Ope_Faltas_Empleado.P_Empleado_ID = Empleado_ID;
            Dt_Empleado = Ope_Faltas_Empleado.Consultar_Faltas_Empelado();
            Grid_Faltas_Empleado.DataSource = Dt_Empleado;
            Grid_Faltas_Empleado.DataBind();
            Grid_Faltas_Empleado.Columns[2].Visible = false;
            Grid_Faltas_Empleado.Columns[3].Visible = true;
            Grid_Faltas_Empleado.Columns[4].Visible = true;
            Grid_Faltas_Empleado.Columns[6].Visible = false;
            Grid_Faltas_Empleado.Columns[8].Visible = false;
            Grid_Faltas_Empleado.Columns[7].Visible = false;
            Grid_Faltas_Empleado.Columns[9].Visible = false;
            Grid_Faltas_Empleado.Columns[10].Visible = false;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Faltas del Empleado. Error:["+Ex.Message+"]"); 
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Faltas_Empleado_PageIndexChanging
    /// DESCRIPCION : Cambiar de pagina ala tabla de Faltas del Empelado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Grid_Faltas_Empleado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Faltas_Empleado.PageIndex = e.NewPageIndex;
        if (Cmb_Empleados.SelectedIndex > 0)
        {
            Consultar_Faltas_Empleado(Cmb_Empleados.SelectedValue.Trim());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Grid_Faltas_Empleado_SelectedIndexChanged
    /// DESCRIPCION : Carga los datos de la Falta del Empleado Sleccionada
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 12/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************    
    protected void Grid_Faltas_Empleado_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Empleados_Negocios INF_EMPLEADO = null;

        try
        {
            int index = Grid_Faltas_Empleado.SelectedIndex;

            INF_EMPLEADO = Presidencia.Ayudante_Informacion.Cls_Ayudante_Nom_Informacion._Informacion_Empleado(
                HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[2].Text.ToString()));

            if (index != -1)
            {
                Limpiar_Ctlrs_Select();
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[1].Text.ToString()).Trim())) 
                    Txt_No_Falta_Empleado.Text = Grid_Faltas_Empleado.Rows[index].Cells[1].Text;
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[2].Text.ToString()).Trim())) 
                    Cmb_Empleados.SelectedIndex = Cmb_Empleados.Items.IndexOf(Cmb_Empleados.Items.FindByValue(Grid_Faltas_Empleado.Rows[index].Cells[2].Text));
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[3].Text.ToString()).Trim()))
                    Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(INF_EMPLEADO.P_Dependencia_ID));
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[4].Text.ToString()).Trim()))
                    Txt_Fecha_Falta_Empleado.Text = String.Format("{0:ddMMyyyy}", Convert.ToDateTime(Grid_Faltas_Empleado.Rows[index].Cells[4].Text.ToString()));
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[5].Text.ToString()).Trim())) 
                    Cmb_Tipo_Falta_Empleado.SelectedIndex = Cmb_Tipo_Falta_Empleado.Items.IndexOf(Cmb_Tipo_Falta_Empleado.Items.FindByValue(Grid_Faltas_Empleado.Rows[index].Cells[5].Text));
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[6].Text.ToString()).Trim())) 
                    Cmb_Retardo_Empleado.SelectedIndex = Cmb_Retardo_Empleado.Items.IndexOf(Cmb_Retardo_Empleado.Items.FindByValue(Grid_Faltas_Empleado.Rows[index].Cells[6].Text));
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[7].Text.ToString()).Trim())) 
                    Txt_Cantidad_Minutos_Retardo_Empleado.Text = Grid_Faltas_Empleado.Rows[index].Cells[7].Text;
                if (!string.IsNullOrEmpty(HttpUtility.HtmlDecode(Grid_Faltas_Empleado.Rows[index].Cells[8].Text.ToString()).Trim())) 
                    Txt_Comentarios.Text = Grid_Faltas_Empleado.Rows[index].Cells[8].Text;

                if (!string.IsNullOrEmpty(Grid_Faltas_Empleado.Rows[index].Cells[9].Text))
                {
                    Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Grid_Faltas_Empleado.Rows[index].Cells[9].Text));
                    Consultar_Periodos_Catorcenales_Nomina(Grid_Faltas_Empleado.Rows[index].Cells[9].Text);
                    Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Grid_Faltas_Empleado.Rows[index].Cells[10].Text));
                }

                Txt_Cantidad_Minutos_Retardo_Empleado_TextChanged(sender, new EventArgs());
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cargar los datos de la Falta del Empleado. Error: [" + Ex.Message + "]");
        }
    }
    /// **************************************************************************************************************************************
    /// NOMBRE: Grid_Programa_Sorting
    /// 
    /// DESCRIPCIÓN: Ordena las columnas en orden ascendente o descendente.
    /// 
    /// CREÓ:   Juan Alberto Hernández Negrete.
    /// FECHA CREÓ: 18/Febrero/2011 19:04 pm.
    /// MODIFICÓ:
    /// FECHA MODIFICÓ:
    /// CAUSA MODIFICACIÓN:
    /// **************************************************************************************************************************************
    protected void Grid_Programa_Sorting(object sender, GridViewSortEventArgs e)
    {
        Consultar_Faltas_Empleado(Cmb_Empleados.SelectedValue.Trim());
        DataTable Dt_Faltas_Empleados = (Grid_Faltas_Empleado.DataSource as DataTable);

        if (Dt_Faltas_Empleados != null)
        {
            DataView Dv_Faltas_Empleados = new DataView(Dt_Faltas_Empleados);
            String Orden = ViewState["SortDirection"].ToString();

            if (Orden.Equals("ASC"))
            {
                Dv_Faltas_Empleados.Sort = e.SortExpression + " " + "DESC";
                ViewState["SortDirection"] = "DESC";
            }
            else
            {
                Dv_Faltas_Empleados.Sort = e.SortExpression + " " + "ASC";
                ViewState["SortDirection"] = "ASC";
            }

            Grid_Faltas_Empleado.DataSource = Dv_Faltas_Empleados;
            Grid_Faltas_Empleado.DataBind();
        }
    }
    #endregion

    #region (Eventos)

    #region (Eventos Alta - Modificar -Eliminar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Falta Empleado
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 12/Noviembre/2010
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
                Habilitar_Controles("Nuevo");
                Limpiar_Ctlrs();
            }
            else
            {
                if (Validar_Datos_Falta_Empleado())
                {
                    Alta_Falta_Empelado();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
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
    ///DESCRIPCIÓN: Modificar un Falta Falta Empleado
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 12/Noviembre/2010
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
                if (Grid_Faltas_Empleado.SelectedIndex != -1 & !Txt_No_Falta_Empleado.Text.Equals(""))
                {
                    Habilitar_Controles("Modificar");
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
                if (Validar_Datos_Falta_Empleado())
                {
                    Modificar_Falta_Empelado();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
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
    ///DESCRIPCIÓN: Eliminar un Falta Empleado
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 12/Noviembre/2010
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
                if (Grid_Faltas_Empleado.SelectedIndex != -1 & !Txt_No_Falta_Empleado.Text.Equals(""))
                {
                    Eliminar_Falta_Empelado();
                    Configuracion_Inicial();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
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
                Session.Remove("Proveedores");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Inicial();//Habilita los controles para la siguiente operación del usuario en el catálogo
                Limpiar_Ctlrs();
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Faltas_Empleado_Click
    ///DESCRIPCIÓN: Busqueda de Empleados Por Usuario_ID o Nombre
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 12/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Faltas_Empleado_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Txt_Busqueda_Falta_Empleado_TextChanged(Txt_Busqueda_Falta_Empleado, new EventArgs());

            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }    
    #endregion

    #region (Eventos Combos)
    protected void Cmb_Dependencia_SelectedIndexChanged(object sender, EventArgs e)
    {
        Txt_Cantidad_Descontar.Text = String.Format("{0:c}", 0);
        Txt_Cantidad_Minutos_Retardo_Empleado.Text = String.Empty;

        if (Cmb_Dependencia.SelectedIndex > 0)
        {
            Consultar_Empleados_Por_Dependencia(Cmb_Dependencia.SelectedValue.Trim());
            Grid_Faltas_Empleado.DataSource = new DataTable();
            Grid_Faltas_Empleado.DataBind();
        }
        Cmb_Dependencia.Focus();
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
    }

    protected void Cmb_Empleados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Faltas_Empleado_Negocio Cat_Faltas_Empleado = new Cls_Ope_Nom_Faltas_Empleado_Negocio();
        String Empleado_ID = String.Empty;
        String Salario_Diario = String.Empty;
        String Puesto_ID = String.Empty;
        Double Salario_Por_Dia = 0.0;
        Double Salario_Por_Hora = 0.0;
        Double Salario_Por_Minuto = 0.0;
        Double Cantidad_Descontar_Minutos_Retardo = 0.0;

        Txt_Cantidad_Descontar.Text = String.Format("{0:c}", 0);

        if (Cmb_Empleados.SelectedIndex > 0)
        {
            Consultar_Faltas_Empleado(Cmb_Empleados.SelectedValue.Trim());

            Cat_Faltas_Empleado.P_Empleado_ID = Cmb_Empleados.SelectedValue.Trim();
            DataTable Dt_Dependencia = Cat_Faltas_Empleado.Consulta_Dependencia_Del_Empelado();
            if (Dt_Dependencia != null)
            {
                if (Dt_Dependencia.Rows.Count > 0)
                {
                    Cmb_Dependencia.SelectedIndex = Cmb_Dependencia.Items.IndexOf(Cmb_Dependencia.Items.FindByValue(Dt_Dependencia.Rows[0][0].ToString()));
                }
            }

            if (Cmb_Retardo_Empleado.SelectedIndex > 0)
            {
                if (Cmb_Retardo_Empleado.SelectedItem.Text.Trim().ToUpper().Equals("SI"))
                {
                    Int32 Minutos = Convert.ToInt32(((String.IsNullOrEmpty(Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim())) ? "0" : Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim()));
                    Salario_Por_Dia = Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim());
                    Salario_Por_Hora = Salario_Por_Dia / 8;
                    Salario_Por_Minuto = Salario_Por_Hora / 60;
                    Cantidad_Descontar_Minutos_Retardo = Salario_Por_Minuto * Minutos;
                    Txt_Cantidad_Descontar.Text = String.Format("{0:c}", Cantidad_Descontar_Minutos_Retardo);
                }
                else
                {
                    Txt_Cantidad_Descontar.Text = String.Format("{0:c}", Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim()));
                }
            }
        }
        Cmb_Empleados.Focus();
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
    }

    protected void Cmb_Retardo_Empleado_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Salario_Diario = String.Empty;
        Txt_Cantidad_Descontar.Text = String.Format("{0:c}", 0);

        if (Cmb_Retardo_Empleado.SelectedItem.Text.Trim().Equals("SI"))
        {
            Txt_Cantidad_Minutos_Retardo_Empleado.Enabled = true;
            Txt_Cantidad_Minutos_Retardo_Empleado.Text = String.Empty;

            Cmb_Tipo_Falta_Empleado.Enabled = false;
            Cmb_Tipo_Falta_Empleado.SelectedIndex = 1;
            Cmb_Retardo_Empleado.Focus();
        }
        else if (Cmb_Retardo_Empleado.SelectedItem.Text.Trim().Equals("NO"))
        {
            Txt_Cantidad_Minutos_Retardo_Empleado.Enabled = false;
            if (Cmb_Empleados.SelectedIndex > 0)
            {
                Txt_Cantidad_Descontar.Text = String.Format("{0:c}", Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim()));
                Txt_Cantidad_Minutos_Retardo_Empleado.Text = String.Empty;
            }

            Cmb_Tipo_Falta_Empleado.Enabled = true;
            Cmb_Tipo_Falta_Empleado.SelectedIndex = -1;
            Cmb_Tipo_Falta_Empleado.Focus();
        }

        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
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
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
    }
    #endregion

    #region (TextBox)
    /// ***********************************************************************************
    /// Nombre: Txt_Cantidad_Minutos_Retardo_Empleado_TextChanged
    /// 
    /// Descripción: Metodo que calcula la cantidad en $ de los que se le descontara al empleado
    ///              por los minutos de retardo.
    /// 
    /// Parámetros: No_Empleado.- Identificador interno del sistema para las operaciones que
    ///                           se realizan sobre los empelados.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 16/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************
    protected void Txt_Cantidad_Minutos_Retardo_Empleado_TextChanged(Object sender, EventArgs e)
    {
        Double Salario_Por_Dia = 0.0;
        Double Salario_Por_Hora = 0.0;
        Double Salario_Por_Minuto = 0.0;
        Double Cantidad_Descontar_Minutos_Retardo = 0.0;

        try
        {
            Txt_Cantidad_Descontar.Text = String.Format("{0:c}", 0);

            if (Cmb_Empleados.SelectedIndex > 0)
            {
                if (!String.IsNullOrEmpty(Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim()))
                {
                    if (Cmb_Retardo_Empleado.SelectedItem.Text.Trim().ToUpper().Equals("SI"))
                    {
                        Double Minutos = Convert.ToDouble(((String.IsNullOrEmpty(Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim())) ? "0" : Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim()));
                        Salario_Por_Dia = Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim());
                        Salario_Por_Hora = Salario_Por_Dia / 8;
                        Salario_Por_Minuto = Salario_Por_Hora / 60;
                        Cantidad_Descontar_Minutos_Retardo = Salario_Por_Minuto * Minutos;
                        Txt_Cantidad_Descontar.Text = String.Format("{0:c}", Cantidad_Descontar_Minutos_Retardo);
                    }
                    else
                    {
                        Txt_Cantidad_Descontar.Text = String.Format("{0:c}", Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim()));
                    }
                }
            }

            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    /// ***********************************************************************************
    /// Nombre: Txt_Busqueda_Falta_Empleado_TextChanged
    /// 
    /// Descripción: Metodo que calcula la cantidad en $ de los que se le descontara al empleado.
    ///              Y consulta la informacion del mismo.
    /// 
    /// Parámetros: No_Empleado.- Identificador interno del sistema para las operaciones que
    ///                           se realizan sobre los empelados.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 16/Agosto/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***********************************************************************************
    protected void Txt_Busqueda_Falta_Empleado_TextChanged(Object sender, EventArgs e)
    {
        Double Salario_Por_Dia = 0.0;
        Double Salario_Por_Hora = 0.0;
        Double Salario_Por_Minuto = 0.0;
        Double Cantidad_Descontar_Minutos_Retardo = 0.0;

        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_Falta_Empleado.Text))
            {
                Ejecutar_Evento();

                Txt_Cantidad_Descontar.Text = String.Format("{0:c}", 0);

                if (Cmb_Empleados.SelectedIndex > 0)
                {
                    if (!String.IsNullOrEmpty(Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim()))
                    {
                        if (Cmb_Retardo_Empleado.SelectedItem.Text.Trim().ToUpper().Equals("SI"))
                        {
                            Double Minutos = Convert.ToDouble(((String.IsNullOrEmpty(Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim())) ? "0" : Txt_Cantidad_Minutos_Retardo_Empleado.Text.Trim()));
                            Salario_Por_Dia = Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim());
                            Salario_Por_Hora = Salario_Por_Dia / 8;
                            Salario_Por_Minuto = Salario_Por_Hora / 60;
                            Cantidad_Descontar_Minutos_Retardo = Salario_Por_Minuto * Minutos;
                            Txt_Cantidad_Descontar.Text = String.Format("{0:n}", Cantidad_Descontar_Minutos_Retardo);
                        }
                        else
                        {
                            Txt_Cantidad_Descontar.Text = String.Format("{0:n}", Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim()));
                        }
                    }
                    else
                    {
                        Txt_Cantidad_Descontar.Text = String.Format("{0:n}", Cls_Ayudante_Nom_Informacion.Obtener_Cantidad_Diaria(Cmb_Empleados.SelectedValue.Trim()));
                    }
                }
            }
            else {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron resultados en la busqueda";
                Grid_Faltas_Empleado.DataSource = new DataTable();
                Grid_Faltas_Empleado.DataBind();
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Faltas_Empleados();", true);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    #endregion

    #endregion



}
