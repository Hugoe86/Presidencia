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
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.DateDiff;
using Presidencia.Operacion_Fechas_Aplicacion.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Fechas_Aplicacion : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Ope_Pre_Fechas_Aplicacion.aspx");
                Configuracion_Formulario(true);
                Llenar_Fechas(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                            controles.
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        // Btn_Nuevo.Visible = true;
        if (Btn_Modificar.AlternateText.Equals("Actualizar"))
        {
        }
        else
        {
            Cmb_Estatus.SelectedIndex = (0);
        }
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = true;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Fecha_Alta.Enabled = !estatus;
        Txt_Fecha_Aplicacion.Enabled = false;
        Txt_Fecha_Movimiento.Enabled = false;
        Txt_Motivo.Enabled = !estatus;
        Cmb_Estatus.Enabled = !estatus;
        Img_Fecha_1.Enabled = !estatus;
        Img_Fecha_2.Enabled = !estatus;
        Grid_Fechas.Enabled = estatus;
        Grid_Fechas.SelectedIndex = (-1);
        Btn_Buscar.Enabled = estatus;
        Btn_Fecha_Busqueda.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Busqueda.Text = "";
        Txt_Id.Text = "";
        Txt_Fecha_Alta.Text = "";
        Txt_Fecha_Aplicacion.Text = "";
        Txt_Fecha_Movimiento.Text = "";
        Txt_Motivo.Text = "";
        Cmb_Estatus.SelectedIndex = (0);
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Fechas
    ///DESCRIPCIÓN: Llena la tabla de Fecchas de Aplicacion
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Fechas(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Fechas_Aplicacion_Negocio Fechas = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio();
            if (!String.IsNullOrEmpty(Txt_Busqueda.Text))
            {
                if (Txt_Busqueda.Text.ToString() != "<Buscar Fecha Aplicacion>") Fechas.P_Filtro = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Busqueda.Text));
            }
            Grid_Fechas.Columns[1].Visible = true;
            Grid_Fechas.DataSource = Fechas.Consultar_Fechas();
            Grid_Fechas.PageIndex = Pagina;
            Grid_Fechas.DataBind();
            Grid_Fechas.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Motivo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduce un Motivo.";
            Validacion = false;
        }
        else
        {
            if (Txt_Motivo.Text.Length > 250)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ El texto de Motivo no debe ser mayor a 250 caracteres.";
                Validacion = false;
            }
        }
        if (Txt_Fecha_Aplicacion.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir una Fecha de Aplicaci&oacute;n.";
            Validacion = false;
        }
        else {
            Cls_Ope_Pre_Fechas_Aplicacion_Negocio fecha = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio();
            fecha.P_Fecha_Aplicacion = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Aplicacion.Text));
            try
            {
                if (fecha.Consultar_Dias_Inhabiles())
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ La fecha de aplicaci&oacute;n es un día inhabil.";
                    Validacion = false;
                }
                else
                {
                    if (Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Aplicacion.Text))).DayOfWeek == DayOfWeek.Saturday || Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Aplicacion.Text))).DayOfWeek == DayOfWeek.Sunday)
                    {
                        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                        Mensaje_Error = Mensaje_Error + "+ La fecha de aplicaci&oacute;n es un día inhabil.";
                        Validacion = false;
                    }
                }
            }
            catch(Exception ex)
            {
                throw new Exception(ex.Message.ToString());
            }
        }
        if (Txt_Fecha_Movimiento.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione una Fecha de Movimiento.";
            Validacion = false;
        }
        if (Cls_DateAndTime.DateDiff(DateInterval.Day, Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Movimiento.Text.ToString()))), Convert.ToDateTime(String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Aplicacion.Text.ToString())))) <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La fecha del movimiento no puede ser mayor o igual a la fecha de aplicación. <br>";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
        return Validacion;
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Fechas_Repetidas
    /// DESCRIPCION : Verifica que la fecha del movimiento y fecha de aplicación no
    ///               esten ya dadas de alta con anticipación en la base de datos
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 09-Octubre-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Consulta_Fechas_Repetidas()
    {
        Boolean Fechas_Validas = true; //Variable que almacena el valor de true si las fechas no estan asignadas y false si ya estan asignadas a otro registro
        DataTable Dt_Fecha_Repetidas = new DataTable(); //Obtiene los registros de la consulta
        Cls_Ope_Pre_Fechas_Aplicacion_Negocio Rs_Consulta_Cat_Pre_Fechas_Aplicacion = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Lbl_Mensaje_Error.Text = "Favor de cambiar <br>";
            Rs_Consulta_Cat_Pre_Fechas_Aplicacion.P_Fecha_Movimiento = String.Format("{0:MM/dd/yyyy}", Convert.ToDateTime(Txt_Fecha_Movimiento.Text.ToString()));
            Dt_Fecha_Repetidas = Rs_Consulta_Cat_Pre_Fechas_Aplicacion.Consulta_Fecha_Repetida(); //Consulta si el día del movimiento propocionado por el usuario ya fue registrado con anterioridad

            //Si encontro la fecha del movimiento dado de alta y pertenece a otro ID visualiza un mensaje al usuario
            if (Dt_Fecha_Repetidas.Rows.Count > 0)
            {
                foreach (DataRow Registro in Dt_Fecha_Repetidas.Rows)
                {
                    //Si diferente el ID al cual se esta registrando o modificando entonces visualiza un mensaje al usuario
                    if (Txt_Id.Text.ToString() != Registro[Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion_ID].ToString())
                    {
                        Lbl_Mensaje_Error.Text += "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp; + La Fecha del Movimiento ya que se encuentra registrado con anterioridad con la fecha de aplicación " + String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Registro[Cat_Pre_Fechas_Aplicacion.Campo_Fecha_Aplicacion].ToString())) + "</br>";
                        Fechas_Validas = false;
                    }
                }
            }
            return Fechas_Validas;
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Fechas_Repetidas " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Fechas_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de las Fechas de Aplicación 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Fechas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Fechas.SelectedIndex = (-1);
        Llenar_Fechas(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Fechas_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una Fecha de Aplicación seleccionada para mostrarla a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Fechas_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Fechas.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Fechas.SelectedRow.Cells[1].Text;
                Cls_Ope_Pre_Fechas_Aplicacion_Negocio Fechas = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio();
                Fechas.P_Fecha_Aplicacion_ID = ID_Seleccionado;
                Fechas = Fechas.Consultar_Datos_Fecha();
                Txt_Id.Text = Fechas.P_Fecha_Aplicacion_ID;
                Txt_Fecha_Alta.Text = String.Format("{0:dd/MMM/yy}", Convert.ToDateTime(Fechas.P_Fecha_Alta));
                Txt_Fecha_Aplicacion.Text = String.Format("{0:dd/MMM/yy}", Convert.ToDateTime(Fechas.P_Fecha_Aplicacion));
                Txt_Fecha_Movimiento.Text = String.Format("{0:dd/MMM/yy}", Convert.ToDateTime(Fechas.P_Fecha_Movimiento));
                Txt_Motivo.Text = Fechas.P_Motivo;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Fechas.P_Estatus));
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva Fecha de aplicación
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Txt_Fecha_Alta.Text = String.Format("{0:dd/MM/yy}",DateTime.Now);
                Txt_Fecha_Alta.Enabled = false;
                Cmb_Estatus.SelectedIndex = 1;
                Cmb_Estatus.Enabled = false;
            }
            else
            {
                if (Validar_Componentes())
                {
                    //Valida que las fechas de aplicación y movimiento no esten dadas de alta con anterioridad para
                    //dar de alta el registro en la base de datos
                    if (Consulta_Fechas_Repetidas())
                    {
                        Cls_Ope_Pre_Fechas_Aplicacion_Negocio Fechas = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio();
                        Fechas.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                        Fechas.P_Fecha_Alta = Txt_Fecha_Alta.Text;
                        Fechas.P_Fecha_Aplicacion = Txt_Fecha_Aplicacion.Text;
                        Fechas.P_Fecha_Movimiento = Txt_Fecha_Movimiento.Text;
                        Fechas.P_Motivo = Txt_Motivo.Text.ToUpper();
                        Fechas.Alta_Fecha();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Fechas(Grid_Fechas.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Fechas de aplicación", "alert('Alta de fecha de aplicación Exitosa');", true);
                        Btn_Nuevo.AlternateText = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    }
                    //Si se encontró la fecha de aplicaciíon y/o fecha de movimiento muestra el mensaje al usuario
                    else
                    {
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Visible = true;
                    }
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de una Fecha de Aplicación
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 22/Julio/2011 
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
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Fechas.Rows.Count > 0 && Grid_Fechas.SelectedIndex > (-1))
                {
                    Btn_Modificar.AlternateText = "Actualizar";
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Txt_Fecha_Alta.Enabled = false;
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el registro que quiere modificar.";
                }
            }
            else
            {
                if (Validar_Componentes())
                {
                    //Valida que las fechas de aplicación y movimiento no esten dadas de alta con anterioridad
                    if (Consulta_Fechas_Repetidas())
                    {
                        Cls_Ope_Pre_Fechas_Aplicacion_Negocio Fechas = new Cls_Ope_Pre_Fechas_Aplicacion_Negocio();
                        Fechas.P_Estatus = Cmb_Estatus.SelectedItem.Value.ToUpper();
                        Fechas.P_Fecha_Alta = Txt_Fecha_Alta.Text;
                        Fechas.P_Fecha_Aplicacion = Txt_Fecha_Aplicacion.Text;
                        Fechas.P_Fecha_Movimiento = Txt_Fecha_Movimiento.Text;
                        Fechas.P_Motivo = Txt_Motivo.Text.ToUpper();
                        Fechas.P_Fecha_Aplicacion_ID = Txt_Id.Text;
                        Fechas.Modificar_Fecha();
                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Fechas(Grid_Fechas.PageIndex);
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Fechas de Aplicación", "alert('Actualización de Fecha de Aplicación Exitosa');", true);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Btn_Nuevo.Visible = true;
                    }
                    //Si se encontró la fecha de aplicaciíon y/o fecha de movimiento muestra el mensaje al usuario
                    else
                    {
                        Img_Error.Visible = true;
                        Lbl_Mensaje_Error.Visible = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Fechas_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 22/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Fechas_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Fechas.SelectedIndex = (-1);
        Llenar_Fechas(0);
        Limpiar_Catalogo();
        if (Grid_Fechas.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Fechas(0);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 02/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.AlternateText.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Configuracion_Formulario(true);
            Limpiar_Catalogo();
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Nuevo.Visible = true;
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
            Botones.Add(Btn_Buscar);
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
