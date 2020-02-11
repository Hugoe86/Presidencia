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
using Presidencia.Percepciones_Variables.Negocio;
using Presidencia.Cat_Nom_Percepciones_Deducciones_Opcional.Negocios;
using Presidencia.Empleados.Negocios;
using System.Text.RegularExpressions;
using Presidencia.Dependencias.Negocios;
using Presidencia.Faltas_Empleado.Negocio;
using Presidencia.Calendario_Nominas.Negocios;
using Presidencia.Prestamos.Negocio;
using System.Collections.Generic;
using System.Text;

public partial class paginas_Nomina_Frm_Ope_Nom_Percepciones_Variables : System.Web.UI.Page
{
    #region (Page_Load)
    protected void Page_Load(object sender, EventArgs e)
    {
        //Estilo para agregar al boton que cuando el cursor este sobre de el.
        String Estilo_Raton_Sobre = "this.style.backgroundColor='#DFE8F6';this.style.cursor='hand';this.style.color='DarkBlue';" +
            "this.style.borderStyle='none';this.style.borderColor='Silver';";
        try
        {
            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
            }
            Btn_Busqueda_Percepcion.Attributes.Add("onmouseover", Estilo_Raton_Sobre);//Se agrega el estilo que tendra boton al estar el mouse sobrede el.
            //Se agrega el estilo que tendra el boton cuando el mouse salga fuera de el.
            Btn_Busqueda_Percepcion.Attributes.Add("onmouseout", "this.style.backgroundColor='#FFFFFF';this.style.color='Black';this.style.borderStyle='none';");
            Btn_Autorizacion_Percepcion_Variable.Visible = false;
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
    ///DESCRIPCIÓN: Configuracion Inicial de los controles del formulario.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Inicial()
    {
        Limpiar_Controles();
        Habilitar_Controles("Inicial");
        Consultar_Percepciones_Variables_Asignadas();
        Consultar_Percepciones_Variables_Opcionales();
        Consultar_Calendarios_Nomina();
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Ctlr
    /// DESCRIPCION : Limpia los Controles de la pagina.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_No_Percepcion.Text = "";
            Cmb_Percepcion.SelectedIndex = -1;
            Cmb_Estatus.SelectedIndex = -1;
            Txt_Comentarios.Text = "";
            Txt_Empleados.Text = "";
            Txt_Busqueda_No_Percepcion.Text = "";
            Cmb_Busqueda_Estatus.SelectedIndex = -1;
            Grid_Percepciones_Variables.SelectedIndex = -1;

            Grid_Empleados.Columns[0].Visible = true;
            Grid_Empleados.SelectedIndex = -1;
            Grid_Empleados.DataSource = new DataTable();
            Grid_Empleados.DataBind();
            Grid_Empleados.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
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
    /// FECHA_CREO  : 28/Noviembre/2010
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
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Busqueda_No_Percepcion_Variable.Enabled = true;

                    Lbl_Mensaje_Error.Text = "";
                    Lbl_Mensaje_Error.Visible = false;
                    Img_Error.Visible = false;
                    TPnl_Contenedor.ActiveTabIndex = 0;

                    Cmb_Busqueda_Estatus.SelectedIndex = Cmb_Busqueda_Estatus.Items.IndexOf(Cmb_Busqueda_Estatus.Items.FindByText("Pendiente"));

                    Configuracion_Acceso("Frm_Ope_Nom_Percepciones_Variables.aspx");
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";

                    Btn_Busqueda_No_Percepcion_Variable.Enabled = false;
                    if (Session["Dt_Empleados"] != null) Session.Remove("Dt_Empleados");
                    
                    TPnl_Contenedor.ActiveTabIndex = 1;
                    Cmb_Estatus.SelectedIndex = 1;
                    Cmb_Estatus.Focus();
                    break;
                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Eliminar.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";

                    Btn_Busqueda_No_Percepcion_Variable.Enabled = false;
                    TPnl_Contenedor.ActiveTabIndex = 1;
                    Cmb_Estatus.Focus();
                    break;
            }

            Txt_No_Percepcion.Enabled = false;
            Cmb_Percepcion.Enabled = Habilitado;
            Txt_Comentarios.Enabled = Habilitado;
            Txt_Empleados.Enabled = Habilitado;
            Cmb_Empleados.Enabled = Habilitado;
            Grid_Percepciones_Variables.Enabled = !Habilitado;
            Grid_Empleados.Enabled = Habilitado;
            Btn_Agregar_Empleado.Enabled = Habilitado;
            Btn_Buscar_Empleados.Enabled = Habilitado;
            Btn_Autorizacion_Percepcion_Variable.Visible = false;
            Cmb_Estatus.Enabled = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? Habilitado : false;            
            
            Tr_Periodos_Fiscales.Visible = (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006")) ? true : false;
            Txt_Cantidad.Enabled = Habilitado;

            Cmb_Calendario_Nomina.Enabled = Habilitado;
            Cmb_Periodos_Catorcenales_Nomina.Enabled = Habilitado;
            Cmb_Estatus.SelectedIndex = 2;
        }
        catch (Exception ex)
        {
            throw new Exception("Error al Habilitar los Controles del formulario. Error:[" + ex.Message.ToString() + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos_Asignacion_Percepciones_Variables
    /// DESCRIPCION : Validar que se hallan proporcionado todos los datos.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos_Asignacion_Percepciones_Variables()
    {
        Boolean Datos_Validos = true;
        Lbl_Mensaje_Error.Text = "Es necesario Introducir: <br>";

        if (Cmb_Percepcion.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Seleccione alguna percepcion variable <br>";
            Datos_Validos = false;
        }

        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + Estatus <br>";
            Datos_Validos = false;
        }

        if (Grid_Empleados.Rows.Count <= 0)
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
    /// FECHA_CREO  : 22/Noviembre/2010
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
                        Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(new Presidencia.Ayudante_Calendario_Nomina.Cls_Ayudante_Calendario_Nomina().P_Periodo));
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
    /// ***************************************************************************************************
    /// Nombre: Quitar_Caracteres_Cantidad
    /// 
    /// Descripción: Recorrecorre todas las celdas de la tabla y revisa si en alguna celda hay algun 
    ///              caracter de simbolo de pesos y lo elimina.
    /// 
    /// Parámetros: Dt_Perc_Deduc_Empl.- Tabla la cuál se recorrera en para eliminar los caracteres
    ///             de simbolo de pesos.
    /// 
    /// Usuario creo: Juan ALberto Hernández Negrete.
    /// Fecha Creó: 14/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// ***************************************************************************************************
    protected DataTable Quitar_Caracteres_Cantidad(DataTable Dt_Perc_Deduc_Empl, String Caracter)
    {
        try
        {
            if (Dt_Perc_Deduc_Empl is DataTable)
            {
                if (Dt_Perc_Deduc_Empl.Rows.Count > 0)
                {
                    foreach (DataRow FILA in Dt_Perc_Deduc_Empl.Rows)
                    {
                        if (FILA is DataRow)
                        {
                            if (Dt_Perc_Deduc_Empl.Columns.Count > 0)
                            {
                                foreach (DataColumn COLUMNA in Dt_Perc_Deduc_Empl.Columns)
                                {
                                    if (COLUMNA is DataColumn)
                                    {
                                        FILA[COLUMNA.ColumnName.Trim()] = FILA[COLUMNA.ColumnName.Trim()].ToString().Trim().Replace(Caracter, "");
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
            throw new Exception("Error al quitar los signos de pesos. Error: [" + Ex.Message + "]");
        }
        return Dt_Perc_Deduc_Empl;
    }
    /// ******************************************************************************************************************************
    /// Nombre: Obtener_Nombre_Empleado
    /// 
    /// Descripción: Recibe un registro de un empleado y obtiene el identificador del empleado el cuál se usara para
    ///              consultar la información del empleado y unir el nombre del empleado con su número de empleado.
    /// 
    /// Parámetros: ref Dt_Empleado.- ref indica que este método afecta al objeto que se le pasa como parámetro.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 15/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// ******************************************************************************************************************************
    protected void Obtener_Nombre_Empleado(ref DataTable Dt_Empleado)
    {
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();//Variable de conexión con la capa de negocios.
        DataTable Dt_Inf_Empleado = null;//Variable que almacena la información del empleado consultado.

        try
        {
            if (Dt_Empleado is DataTable)
            {
                if (Dt_Empleado.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleado.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            if (!String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim()))
                            {
                                String Empleado_ID = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();

                                Obj_Empleados.P_Empleado_ID = Empleado_ID;
                                Dt_Inf_Empleado = Obj_Empleados.Consulta_Empleados_General();
                                Dt_Inf_Empleado = Juntar_No_Empleado_Empleado(Dt_Inf_Empleado);

                                if (Dt_Inf_Empleado is DataTable)
                                {
                                    if (Dt_Inf_Empleado.Rows.Count > 0)
                                    {
                                        foreach (DataRow INF_EMPLEADO in Dt_Inf_Empleado.Rows)
                                        {
                                            if (INF_EMPLEADO is DataRow)
                                            {
                                                EMPLEADO[Cat_Empleados.Campo_Nombre] = INF_EMPLEADO["EMPLEADO"].ToString().Trim();
                                            }
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
            throw new Exception("Error al obetener el nombre del empleado. Error: [" + Ex.Message + "]");
        }
    }
    /// ******************************************************************************************************************************
    /// Nombre: Juntar_No_Empleado_Empleado
    /// 
    /// Descripción: este método une el nombre del empleado con su número de empleado.
    /// 
    /// Parámetros: Dt_Empleado.- Este parám etro contiene una tabla que se recorrera e unira el número de empleado con su nombre.
    /// 
    /// Usuario Creo: Juan Alberto Hernández Negrete.
    /// Fecha Creó: 15/Julio/2011
    /// Usuario Modifico:
    /// Fecha Modifico:
    /// Causa Modificacion:
    /// ******************************************************************************************************************************
    protected DataTable Juntar_No_Empleado_Empleado(DataTable Dt_Empleados)
    {
        try
        {
            if (Dt_Empleados is DataTable)
            {
                if (Dt_Empleados.Rows.Count > 0)
                {
                    foreach (DataRow EMPLEADO in Dt_Empleados.Rows)
                    {
                        if (EMPLEADO is DataRow)
                        {
                            Int32 No_Empleado = Convert.ToInt32(String.IsNullOrEmpty(EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim()) ? "0" : EMPLEADO[Cat_Empleados.Campo_No_Empleado].ToString().Trim());
                            EMPLEADO["EMPLEADO"] = "[" + String.Format("{0:000000}", No_Empleado) + "] -- " +
                                EMPLEADO["EMPLEADO"].ToString().Trim();
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al unir el número de empleado con el nombre del mismo. Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Crear_Tabla
    /// 
    /// DESCRIPCION :Crea la tabla de empleados para quitar el campo de tipo cantidad
    ///              que se encuentra como decimal.
    /// 
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 29/Septiembre/2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected DataTable Crear_Tabla(DataTable Dt_Datos)
    {
        DataTable Dt_Empleados = new DataTable();//Variable que almacenara una lista de empleados

        try
        {
            //Definicion de sus columnas.
            Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(System.String));
            Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(System.String));
            Dt_Empleados.Columns.Add(Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad, typeof(System.String));

            if (Dt_Datos is DataTable)
            {
                if (Dt_Datos.Rows.Count > 0)
                {
                    foreach (DataRow RENGLON in Dt_Datos.Rows)
                    {
                        if (RENGLON is DataRow)
                        {
                            DataRow RENGLON_NEW = Dt_Empleados.NewRow();
                            RENGLON_NEW[Cat_Empleados.Campo_Empleado_ID] = RENGLON[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                            RENGLON_NEW[Cat_Empleados.Campo_Nombre] = RENGLON[Cat_Empleados.Campo_Nombre].ToString().Trim();
                            RENGLON_NEW[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad] = RENGLON[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad].ToString().Trim();
                            Dt_Empleados.Rows.Add(RENGLON_NEW);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al construir la tabla de empleados. Error: [" + Ex.Message + "]");
        }
        return Dt_Empleados;
    }
    #endregion

    #region (Metodos Alta - Modificar - Actualizar)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Alta_Asignacion_Percepciones_Variables
    /// DESCRIPCION : Ejecuta el alta de una Asiganacion de la Percepcion Variable
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Alta_Asignacion_Percepciones_Variables()
    {
        Cls_Ope_Nom_Percepciones_Var_Negocio Alta_Asiganacion_Percepcion_Variable = new Cls_Ope_Nom_Percepciones_Var_Negocio();//Variable de conexion con la capa de negocios
        try
        {
            Alta_Asiganacion_Percepcion_Variable.P_Percepcion_Deduccion_ID = Cmb_Percepcion.SelectedValue.Trim();
            Alta_Asiganacion_Percepcion_Variable.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Alta_Asiganacion_Percepcion_Variable.P_Comentarios = Txt_Comentarios.Text;
            Alta_Asiganacion_Percepcion_Variable.P_Usuario_Creo = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Alta_Asiganacion_Percepcion_Variable.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Alta_Asiganacion_Percepcion_Variable.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Alta_Asiganacion_Percepcion_Variable.P_Empleado_ID = Cmb_Empleados.SelectedValue.Trim();

            if (Session["Dt_Empleados"] != null)
            {
                Alta_Asiganacion_Percepcion_Variable.P_Dt_Empleados = Quitar_Caracteres_Cantidad(((DataTable)Session["Dt_Empleados"]), "$");
            }
            if (Session["Dt_Empleados"] != null)
            {
                Session.Remove("Dt_Empleados");
            }
            //Alta Asigancion de Percepcion Variable
            if (Alta_Asiganacion_Percepcion_Variable.Alta_Percepcion_Variable())
            {
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
                Limpiar_Controles();//limpia los controles de la pagina.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Alta Asignacion Percepcion Variable]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al dar de Alta Asignacion Percepcion Variable. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Modificar_Asignacion_Percepciones_Variables
    /// DESCRIPCION : Modificar Asigancion de Percepcion Variable.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Modificar_Asignacion_Percepciones_Variables()
    {
        Cls_Ope_Nom_Percepciones_Var_Negocio Modificar_Asignacion_Percepcion_Variable = new Cls_Ope_Nom_Percepciones_Var_Negocio();//Variable de conexion con la capa de negocios
        try
        {
            Modificar_Asignacion_Percepcion_Variable.P_No_Percepcion = Txt_No_Percepcion.Text.Trim();
            Modificar_Asignacion_Percepcion_Variable.P_Percepcion_Deduccion_ID = Cmb_Percepcion.SelectedValue.Trim();
            Modificar_Asignacion_Percepcion_Variable.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
            Modificar_Asignacion_Percepcion_Variable.P_Comentarios = Txt_Comentarios.Text;
            Modificar_Asignacion_Percepcion_Variable.P_Usuario_Modifico = HttpUtility.HtmlDecode((String)Cls_Sessiones.Nombre_Empleado);
            Modificar_Asignacion_Percepcion_Variable.P_Nomina_ID = Cmb_Calendario_Nomina.SelectedValue.Trim();
            Modificar_Asignacion_Percepcion_Variable.P_No_Nomina = Convert.ToInt32(Cmb_Periodos_Catorcenales_Nomina.SelectedValue.Trim());
            Modificar_Asignacion_Percepcion_Variable.P_Empleado_ID = Cmb_Empleados.SelectedValue.Trim();

            if (Session["Dt_Empleados"] != null)
            {
                Modificar_Asignacion_Percepcion_Variable.P_Dt_Empleados = Quitar_Caracteres_Cantidad(((DataTable)Session["Dt_Empleados"]), "$");
            }
            if (Session["Dt_Empleados"] != null)
            {
                Session.Remove("Dt_Empleados");
            }
            //Modificar Asigancion de Percepcion Variable.
            if (Modificar_Asignacion_Percepcion_Variable.Modificar_Percepcion_Empleado())
            {
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina.
                Limpiar_Controles();//limpia los controles de la pagina.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Modificar Asigancion de Percepcion Variable]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Modificar Asigancion de Percepcion Variable. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Eliminar_Asignacion_Percepciones_Variables
    /// DESCRIPCION : Eliminar Asignacion Percepcion Variable
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Eliminar_Asignacion_Percepciones_Variables()
    {
        Cls_Ope_Nom_Percepciones_Var_Negocio Baja_Asignacion_Percepcion_Variable = new Cls_Ope_Nom_Percepciones_Var_Negocio();//Variable de conexion con la capa de negocios
        try
        {
            Baja_Asignacion_Percepcion_Variable.P_No_Percepcion = Txt_No_Percepcion.Text.Trim();
            //Eliminar Asignacion Percepcion Variable
            if (Baja_Asignacion_Percepcion_Variable.Eliminar_Percepcion_Variable())
            {
                Configuracion_Inicial();//Habilita la configuracion inicial de los controles de la pagina
                Limpiar_Controles();//limpia los controles de la pagina.
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "", "alert('Operación Exitosa [Eliminar Asignacion Percepcion Variable]');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error producido al Eliminar Asignacion Percepcion Variable. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #region(Metodos Add & Delete Empleados)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Agregar_Empleado
    /// DESCRIPCION : Agrega un Empleado
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Agregar_Empleado(DataTable _DataTable, GridView _GridView, DropDownList _DropDownList)
    {
        DataRow[] Filas;//Variable que almacenara un arreglo de DataRows
        DataTable Dt_Empleados_Temporal = _DataTable;//Variable que almacenara una lista de empleados.
        Cls_Cat_Empleados_Negocios Cat_Empleados_Interna = new Cls_Cat_Empleados_Negocios();//Variable de conexion con la capa de negocios

        try
        {
            int indice = _DropDownList.SelectedIndex;

            if (indice > 0)
            {
                Filas = _DataTable.Select(Cat_Empleados.Campo_Empleado_ID + "='" + _DropDownList.SelectedValue.Trim() + "'");
                if (Filas.Length > 0)
                {
                    //Si se encontro algun coincidencia entre el grupo a agregar con alguno agregado anteriormente, se avisa
                    //al usuario que elemento ha agregar ya existe en la tabla de grupos.
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                        "alert('No se puede agregar el Empleado, ya que esta ya se ha agregado');", true);
                    Cmb_Empleados.SelectedIndex = -1;
                }
                else
                {
                    Cat_Empleados_Interna.P_Empleado_ID = _DropDownList.SelectedValue.Trim();
                    DataTable Dt_Inf_Empleado = Cat_Empleados_Interna.Consulta_Empleados_General();
                    Dt_Inf_Empleado = Juntar_No_Empleado_Empleado(Dt_Inf_Empleado);

                    if (Dt_Inf_Empleado  is DataTable )
                    {
                        if (Dt_Inf_Empleado.Rows.Count > 0)
                        {
                            foreach (DataRow EMPLEADO in Dt_Inf_Empleado.Rows)
                            {
                                if (EMPLEADO is DataRow)
                                {
                                    DataRow Renglon = Dt_Empleados_Temporal.NewRow();
                                    Renglon[Cat_Empleados.Campo_Empleado_ID] = EMPLEADO[Cat_Empleados.Campo_Empleado_ID].ToString().Trim();
                                    Renglon[Cat_Empleados.Campo_Nombre] = EMPLEADO["EMPLEADO"].ToString().Trim();
                                    Renglon[Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad] = String.Format("{0:c}", Convert.ToDouble(String.IsNullOrEmpty(Txt_Cantidad.Text.Trim()) ? "0" : Txt_Cantidad.Text.Trim()));

                                    Dt_Empleados_Temporal.Rows.Add(Renglon);
                                    Dt_Empleados_Temporal.AcceptChanges();
                                    Session["Dt_Empleados"] = Dt_Empleados_Temporal;

                                    _GridView.Columns[0].Visible = true;
                                    _GridView.DataSource = (DataTable)Session["Dt_Empleados"];
                                    _GridView.DataBind();
                                    _GridView.Columns[0].Visible = false;

                                    Cmb_Empleados.SelectedIndex = -1;
                                    Txt_Cantidad.Text = "";
                                }
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
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Dependencias(DropDownList _DropDownList)
    {
        Cls_Cat_Dependencias_Negocio _Cat_Dependencias = new Cls_Cat_Dependencias_Negocio();//Variable de conexion con la capa de negocios
        DataTable Dt_Dependencias = null;//Variable que almacenara una lista de dependencias.
        try
        {
            Dt_Dependencias = _Cat_Dependencias.Consulta_Dependencias();//consulta las dependencias.
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
    private void Consultar_Empleados()
    {
        Cls_Ope_Nom_Percepciones_Var_Negocio _Cat_Empleados = new Cls_Ope_Nom_Percepciones_Var_Negocio();//Variable de conexion con la capa de negocios
        DataTable Dt_Empleados = null;//Variable que almacenara una lista de empleados
        Cls_Cat_Empleados_Negocios Obj_Empleados = new Cls_Cat_Empleados_Negocios();

        try
        {
            Cmb_Empleados.Items.Clear();

            if (Es_Numero(Txt_Empleados.Text.Trim()))
            {
                Txt_Empleados.Text = String.Format("{0:000000}", Convert.ToInt64(Txt_Empleados.Text.Trim()));
                Obj_Empleados.P_No_Empleado = Txt_Empleados.Text.Trim();
            }
            else
            {
                Obj_Empleados.P_Nombre = Txt_Empleados.Text.Trim();
            }

            Dt_Empleados = Obj_Empleados.Consulta_Empleados_General();//Consulta los empleados.
            Dt_Empleados = Juntar_No_Empleado_Empleado(Dt_Empleados);
            Cmb_Empleados.DataSource = Dt_Empleados;
            Cmb_Empleados.DataTextField = "EMPLEADO";
            Cmb_Empleados.DataValueField = Cat_Empleados.Campo_Empleado_ID;
            Cmb_Empleados.DataBind();
            Cmb_Empleados.Items.Insert(0, new ListItem("< Seleccione >", ""));

            if (Cmb_Empleados.Items.Count > 1)
            {
                Cmb_Empleados.SelectedIndex = 1;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar a los empleados por depèndencia. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consultar_Percepciones_Variables_Opcionales
    /// DESCRIPCION : Consulta las Percepciones Variables Opcionales en el sistema que existen actualmente.
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consultar_Percepciones_Variables_Opcionales()
    {
        Cls_Cat_Nom_Percepciones_Deducciones_Business Cat_Percepciones_Deducciones_Consulta = new Cls_Cat_Nom_Percepciones_Deducciones_Business();//Variable de conexion con la capa de negocios
        DataTable Dt_Deducciones_Variables_Opcionales = null;//Variable que almacenara una lista de las percepciones variables asignadas

        try
        {
            Cat_Percepciones_Deducciones_Consulta.P_Concepto = "TIPO_NOMINA";
            Cat_Percepciones_Deducciones_Consulta.P_TIPO_ASIGNACION = "VARIABLE";
            Cat_Percepciones_Deducciones_Consulta.P_TIPO = "PERCEPCION";

            Dt_Deducciones_Variables_Opcionales = Cat_Percepciones_Deducciones_Consulta.Consulta_Avanzada_Percepciones_Deducciones();
            Dt_Deducciones_Variables_Opcionales = Juntar_Clave_Percepcion_Deduccion(Dt_Deducciones_Variables_Opcionales);
            Cmb_Percepcion.DataSource = Dt_Deducciones_Variables_Opcionales;
            Cmb_Percepcion.DataTextField = Cat_Nom_Percepcion_Deduccion.Campo_Nombre;
            Cmb_Percepcion.DataValueField = Cat_Nom_Percepcion_Deduccion.Campo_Percepcion_Deduccion_ID;
            Cmb_Percepcion.DataBind();
            Cmb_Percepcion.Items.Insert(0, new ListItem("< Seleccione >", ""));
            Cmb_Percepcion.SelectedIndex = -1;
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las percepciones Variables Opcionales en el sistema. Error: [" + Ex.Message + "]");
        }
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
                            PERCEPCION_DEDUCCION[Cat_Nom_Percepcion_Deduccion.Campo_Clave].ToString().Replace("-", "").Trim() + ") .- " +
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
            Botones.Add(Btn_Busqueda_No_Percepcion_Variable);

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

    #region (Grid_Empleados)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_PageIndexChanging
    ///DESCRIPCIÓN: Realiza el Cambio de la pagina de la tabla.
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_PageIndexChanging(object sender, GridViewPageEventArgs e)
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
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void LLenar_Grid_Empleados(Int32 Pagina, DataTable Tabla)
    {
        Obtener_Nombre_Empleado(ref Tabla);

        Grid_Empleados.Columns[0].Visible = true;
        Grid_Empleados.SelectedIndex = (-1);
        Grid_Empleados.DataSource = Tabla;
        Grid_Empleados.PageIndex = Pagina;
        Grid_Empleados.DataBind();
        Session["Dt_Empleados"] = Tabla;
        Grid_Empleados.Columns[0].Visible = false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Empleados_RowDataBound
    ///DESCRIPCIÓN: Es el evento previo antes cargar el grid con informacion de 
    ///los empleados
    ///PARAMETROS:  
    ///CREO: Juan Alberto Hernandez Negrete
    ///FECHA_CREO: 28/Noviembre/2010
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Empleados_RowDataBound(object sender, GridViewRowEventArgs e)
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

    #region (Grid_Percepciones_Variables)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Percepciones_Variables_PageIndexChanging
    ///DESCRIPCIÓN: Cambia la Pagina del Grid Percepciones Variables
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Percepciones_Variables_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Percepciones_Variables.PageIndex = e.NewPageIndex;
            Consultar_Percepciones_Variables_Asignadas();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al cambiar la de pagina del Grid de Percepciones Variables. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Percepciones_Variables_SelectedIndexChanged
    ///DESCRIPCIÓN: Selecciona un elemnto de la tabla de Percepciones Variables
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 22/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Percepciones_Variables_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Percepciones_Var_Negocio Cls_Ope_Nom_Percepciones_Var_Consulta = new Cls_Ope_Nom_Percepciones_Var_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos del puesto
        Cls_Ope_Nom_Percepciones_Var_Negocio Cls_Ope_Nom_Percepcion_Variable_Consultada = null;//Variable de tipo clase que almacenara todos los atributos de capa de negocio.
        try
        {
            Cls_Ope_Nom_Percepciones_Var_Consulta.P_No_Percepcion = Grid_Percepciones_Variables.Rows[Grid_Percepciones_Variables.SelectedIndex].Cells[1].Text.Trim();
            Cls_Ope_Nom_Percepcion_Variable_Consultada = Cls_Ope_Nom_Percepciones_Var_Consulta.Consulta_Percepciones_Variables();

            Txt_No_Percepcion.Text = Cls_Ope_Nom_Percepcion_Variable_Consultada.P_No_Percepcion;                        
            Cmb_Percepcion.SelectedIndex = Cmb_Percepcion.Items.IndexOf(Cmb_Percepcion.Items.FindByValue(Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Percepcion_Deduccion_ID));
            Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Estatus));
            Txt_Comentarios.Text = Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Comentarios;

            if (!string.IsNullOrEmpty(Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Nomina_ID))
            {
                Cmb_Calendario_Nomina.SelectedIndex = Cmb_Calendario_Nomina.Items.IndexOf(Cmb_Calendario_Nomina.Items.FindByValue(Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Nomina_ID));
                Consultar_Periodos_Catorcenales_Nomina(Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Nomina_ID);
                Cmb_Periodos_Catorcenales_Nomina.SelectedIndex = Cmb_Periodos_Catorcenales_Nomina.Items.IndexOf(Cmb_Periodos_Catorcenales_Nomina.Items.FindByText(Cls_Ope_Nom_Percepcion_Variable_Consultada.P_No_Nomina.ToString()));
            }

            LLenar_Grid_Empleados(0, Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Dt_Empleados);

            //Modulo de Autorizacion de Percepciones Variables
            if (Cls_Util.Consultar_Grupo_Rol_ID(Cls_Sessiones.Datos_Empleado.Rows[0][Cat_Empleados.Campo_Rol_ID].ToString()).Rows[0][Apl_Grupos_Roles.Campo_Grupo_Roles_ID].ToString().Equals("00006"))
            {
                if (!Cls_Ope_Nom_Percepcion_Variable_Consultada.P_Estatus.Trim().ToUpper().Equals("ACEPTADO"))
                {
                    Btn_Autorizacion_Percepcion_Variable.Visible = true;
                    Btn_Autorizacion_Percepcion_Variable.NavigateUrl = "Frm_Ope_Nom_Seguimiento_Perc_Var.aspx?No_Percepcion=" + Grid_Percepciones_Variables.Rows[Grid_Percepciones_Variables.SelectedIndex].Cells[1].Text.Trim() + "&Percepcion=" + Cmb_Percepcion.SelectedItem.Text.Trim() + "&PAGINA=" + Request.QueryString["PAGINA"];
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
    ///NOMBRE DE LA FUNCIÓN: Consultar_Percepciones_Variables_Asignadas
    ///DESCRIPCIÓN: Consulta las Percepciones Variables Asignadas que existen en la Base de Datos
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO:28/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Consultar_Percepciones_Variables_Asignadas()
    {
        Cls_Ope_Nom_Percepciones_Var_Negocio Ope_Nom_Percepciones_Var_Consulta = new Cls_Ope_Nom_Percepciones_Var_Negocio();//Variable de conexion con la capa de negocio.
        try
        { 
            //Consulta de los Perciones Variables que existen actualmente.            

            if (Cmb_Busqueda_Estatus.SelectedIndex > 0)
                Ope_Nom_Percepciones_Var_Consulta.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();//"Pendiente";
            else Ope_Nom_Percepciones_Var_Consulta.P_Estatus = String.Empty;

            Grid_Percepciones_Variables.DataSource = Ope_Nom_Percepciones_Var_Consulta.Consulta_Percepciones_Variables().P_Dt_Ope_Nom_Percepciones_Var;
            Grid_Percepciones_Variables.DataBind();
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al consultar las Percepciones Variables Asignadas existentes. Error: [" + Ex.Message + "]");
        }
    }
    #endregion

    #endregion

    #region (Eventos)

    #region (Eventos Alta- Baja - Modificar)
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Alta de un Asignacion de una Percepcion Variable
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 22/Noviembre/2010 
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
                Limpiar_Controles();//limpia los controles de la pagina.
                Habilitar_Controles("Nuevo");//Habilita la configuracion de para ejecutar el alta.              
            }
            else
            {
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Asignacion_Percepciones_Variables())
                {
                    Alta_Asignacion_Percepciones_Variables();//ejecuta el alta.
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Percepciones_Variables();", true);
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
    ///DESCRIPCIÓN: Modificacion de un Asignacion de una Percepcion Variable
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 28/Noviembre/2010
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
                if (Grid_Percepciones_Variables.SelectedIndex != -1 & !Txt_No_Percepcion.Text.Equals(""))
                {
                    if (Cmb_Estatus.SelectedItem.Text.Trim().ToUpper().Equals("ACEPTADO"))
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = "El registro ya fea aceptado, ya no es posible realizar ninguna modificacion <br>";
                    }
                    else
                    {
                        Habilitar_Controles("Modificar");//Habilita la configuracion de los controles para ejecutar la operacion de modificar.
                    }
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
                //Valida los datos ingresados por el usuario.
                if (Validar_Datos_Asignacion_Percepciones_Variables())
                {
                    Modificar_Asignacion_Percepciones_Variables();//Ejecuta la modificacion.
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Percepciones_Variables();", true);
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
    ///DESCRIPCIÓN: Eliminar un Percepcion Variable Asignada
    ///CREO: Juan alberto Hernández Negrete
    ///FECHA_CREO: 22/Noviembre/2010 
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
                //Valida que se halla seleccionado la percepcion variable a eliminar.
                if (Grid_Percepciones_Variables.SelectedIndex != -1 & !Txt_No_Percepcion.Text.Equals(""))
                {
                    Eliminar_Asignacion_Percepciones_Variables();//Ejecuta la baja.
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el registro que desea eliminar <br>";
                }
            }
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Percepciones_Variables();", true);
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
            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Percepciones_Variables();", true);
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
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Buscar_Empleados_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Consultar_Empleados();

            if (Cmb_Empleados.Items.Count > 1)
            {
                Txt_Cantidad.Text = string.Empty;
                Txt_Cantidad.Focus();
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
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Busqueda_Percepcion_Click
    /// DESCRIPCION : Ejecuta la Busqueda de Percepciones Variables Asignadas Dados de Alta en el Sistema
    /// por diferentes filtros. [No_Percepcion, Estatus, Dependencia]
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Busqueda_Percepcion_Click(object sender, EventArgs e)
    {
        Cls_Ope_Nom_Percepciones_Var_Negocio Cls_Ope_Nom_Percepciones_Variables_Consulta = new Cls_Ope_Nom_Percepciones_Var_Negocio();//Variable de conexion con la capa de negocio.
        Cls_Ope_Nom_Percepciones_Var_Negocio Cls_Ope_Nom_Percepciones_Variables_Consultadas = null;//Variable que alamcenara todas las propiedades de la clase de Cls_Ope_Nom_Percepciones_Var_Negocio

        try
        {
            if(!String.IsNullOrEmpty(Txt_Busqueda_No_Empleado.Text.Trim())) Cls_Ope_Nom_Percepciones_Variables_Consulta.P_No_Empleado=Txt_Busqueda_No_Empleado.Text.Trim();
            if (!String.IsNullOrEmpty(Txt_Busqueda_Nombre_Empleado.Text.Trim())) Cls_Ope_Nom_Percepciones_Variables_Consulta.P_Nombre_Empleado = Txt_Busqueda_Nombre_Empleado.Text.Trim();


            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Percepcion.Text.Trim())) Txt_Busqueda_No_Percepcion.Text = String.Format("{0:0000000000}", Convert.ToInt64(Txt_Busqueda_No_Percepcion.Text.Trim()));
            if (!string.IsNullOrEmpty(Txt_Busqueda_No_Percepcion.Text.Trim())) Cls_Ope_Nom_Percepciones_Variables_Consulta.P_No_Percepcion = Txt_Busqueda_No_Percepcion.Text.Trim();
            if (Cmb_Busqueda_Estatus.SelectedIndex > 0) Cls_Ope_Nom_Percepciones_Variables_Consulta.P_Estatus = Cmb_Busqueda_Estatus.SelectedItem.Text.Trim();
            

            Cls_Ope_Nom_Percepciones_Variables_Consultadas = Cls_Ope_Nom_Percepciones_Variables_Consulta.Consulta_Percepciones_Variables();
            Grid_Percepciones_Variables.DataSource = Cls_Ope_Nom_Percepciones_Variables_Consultadas.P_Dt_Ope_Nom_Percepciones_Var;
            Grid_Percepciones_Variables.DataBind();

            ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Percepciones_Variables();", true);
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
    /// FECHA_CREO  : 28/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Empleado_Click(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Empleados.SelectedIndex > 0)
            {
                if (Session["Dt_Empleados"] != null)
                {
                    if (!string.IsNullOrEmpty(Txt_Cantidad.Text.Trim()))
                    {
                        if (Convert.ToDouble(Txt_Cantidad.Text.Trim()) > 0)
                        {
                            DataTable Dt_Aux = Crear_Tabla((DataTable)Session["Dt_Empleados"]);
                            Agregar_Empleado(Dt_Aux, Grid_Empleados, Cmb_Empleados);

                            Txt_Empleados.Text = string.Empty;
                            Txt_Empleados.Focus();
                        }
                        else
                        {
                            Txt_Cantidad.Text = string.Empty;
                            Txt_Cantidad.Focus();
                        }
                    }
                }
                else
                {
                    DataTable Dt_Empleados = new DataTable();//Variable que almacenara una lista de empleados
                    //Definicion de sus columnas.
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(System.String));
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(System.String));
                    Dt_Empleados.Columns.Add(Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad, typeof(System.String));

                    Session["Dt_Empleados"] = Dt_Empleados;
                    Grid_Empleados.Columns[0].Visible = true;
                    Grid_Empleados.DataSource = (DataTable)Session["Dt_Empleados"];
                    Grid_Empleados.DataBind();
                    Grid_Empleados.Columns[0].Visible = false;

                    if (!string.IsNullOrEmpty(Txt_Cantidad.Text.Trim()))
                    {
                        if (Convert.ToDouble(Txt_Cantidad.Text.Trim()) > 0)
                        {
                            Agregar_Empleado(Dt_Empleados, Grid_Empleados, Cmb_Empleados);
                            Txt_Empleados.Text = "";

                            Txt_Empleados.Text = string.Empty;
                            Txt_Empleados.Focus();
                        }
                        else {
                            Txt_Cantidad.Text = string.Empty;
                            Txt_Cantidad.Focus();
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ningun empleado a agregar');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al buscar el Empleado. Error: [" + Ex.Message + "]");
        }
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
        DataRow[] Renglones;//Variable que almacena una lista de DataRows del  Grid_Empleados
        DataRow Renglon;//Variable que almacenara un Renglon del Grid_Empleados
        ImageButton Btn_Eliminar_Empleado = (ImageButton)sender;//Variable que almacenra el control Btn_Eliminar_Empleado

        if (Session["Dt_Empleados"] != null)
        {
            Renglones = ((DataTable)Session["Dt_Empleados"]).Select(Cat_Empleados.Campo_Empleado_ID + "='" + Btn_Eliminar_Empleado.CommandArgument + "'");

            if (Renglones.Length > 0)
            {
                Renglon = Renglones[0];
                DataTable Tabla = (DataTable)Session["Dt_Empleados"];
                Tabla.Rows.Remove(Renglon);
                Session["Dt_Empleados"] = Tabla;
                Grid_Empleados.SelectedIndex = (-1);
                LLenar_Grid_Empleados(Grid_Empleados.PageIndex, Tabla);
            }
        }
        else
        {
            Lbl_Mensaje_Error.Text = "Se debe seleccionar de la tabla el Empleados a quitar";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Percepciones_Variables();", true);
    }
    #endregion

    #region (Eventos Combos)
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
        ScriptManager.RegisterStartupScript(Upd_Panel, typeof(string), "Imagen", "javascript:Inicializar_Eventos_Percepciones_Variables();", true);
    }
    #endregion

    #region (TextBox)
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Empleados_TextChanged
    /// DESCRIPCION : Busca al empleado por Nombre o Numero de Control
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Empleados_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Consultar_Empleados();

            if (Cmb_Empleados.Items.Count > 1)
            {
                Txt_Cantidad.Text = string.Empty;
                Txt_Cantidad.Focus();
            }
            else {
                Txt_Empleados.Text = string.Empty;
                Txt_Empleados.Focus();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al buscar el Empleado. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Txt_Empleados_TextChanged
    /// DESCRIPCION : Busca al empleado por Nombre o Numero de Control
    /// CREO        : Juan Alberto Hernandez Negrete
    /// FECHA_CREO  : 22/Noviembre/2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Txt_Cantidad_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Cmb_Empleados.SelectedIndex > 0)
            {
                if (Session["Dt_Empleados"] != null)
                {
                    if (!string.IsNullOrEmpty(Txt_Cantidad.Text.Trim()))
                    {
                        if (Convert.ToDouble(Txt_Cantidad.Text.Trim()) > 0)
                        {
                            DataTable Dt_Aux = Crear_Tabla((DataTable)Session["Dt_Empleados"]);
                            Agregar_Empleado(Dt_Aux, Grid_Empleados, Cmb_Empleados);

                            Txt_Empleados.Text = string.Empty;
                            Txt_Empleados.Focus();
                        }
                        else
                        {
                            Txt_Cantidad.Text = string.Empty;
                            Txt_Cantidad.Focus();
                        }
                    }
                }
                else
                {
                    DataTable Dt_Empleados = new DataTable();//Variable que almacenara una lista de empleados
                    //Definicion de sus columnas.
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Empleado_ID, typeof(System.String));
                    Dt_Empleados.Columns.Add(Cat_Empleados.Campo_Nombre, typeof(System.String));
                    Dt_Empleados.Columns.Add(Ope_Nom_Perc_Var_Emp_Det.Campo_Cantidad, typeof(System.String));

                    Session["Dt_Empleados"] = Dt_Empleados;
                    Grid_Empleados.Columns[0].Visible = true;
                    Grid_Empleados.DataSource = (DataTable)Session["Dt_Empleados"];
                    Grid_Empleados.DataBind();
                    Grid_Empleados.Columns[0].Visible = false;

                    if (!string.IsNullOrEmpty(Txt_Cantidad.Text.Trim()))
                    {
                        if (Convert.ToDouble(Txt_Cantidad.Text.Trim()) > 0)
                        {
                            Agregar_Empleado(Dt_Empleados, Grid_Empleados, Cmb_Empleados);
                            Txt_Empleados.Text = "";

                            Txt_Empleados.Text = string.Empty;
                            Txt_Empleados.Focus();
                        }
                        else {
                            Txt_Cantidad.Text = string.Empty;
                            Txt_Cantidad.Focus();
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "",
                    "alert('No se a seleccionado ningun empleado a agregar');", true);
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
