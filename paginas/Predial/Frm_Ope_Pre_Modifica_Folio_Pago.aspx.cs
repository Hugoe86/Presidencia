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
using Presidencia.Operacion_Modifica_Folio_Pago.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Modifica_Folio_Pago : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Ope_Pre_Modifica_Folio_Pago.aspx");
                Configuracion_Formulario(true);
                Consulta_Caja_Empleado();
                Llenar_Tabla_Modifica_Folio_Pago(0);
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    #region Metodos

    ///****************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PROPIEDADES:     
    ///             1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Folio_Actual.Enabled = !Estatus;
        Txt_Folio_Nuevo.Enabled = !Estatus;
        Txt_Motivo.Enabled = !Estatus;
        Grid_Modifica_Folio_Pago.SelectedIndex = (-1);
        Btn_Buscar.Enabled = Estatus;
        Txt_Busqueda.Enabled = Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Folio_Actual.Text = "";
        Txt_Folio_Nuevo.Text = "";
        Txt_Motivo.Text = "";
        Txt_Busqueda.Text = "";
        Grid_Modifica_Folio_Pago.DataSource = new DataTable();
        Grid_Modifica_Folio_Pago.DataBind();
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Folio_Actual.Text.Trim().Length == 0 || Txt_Folio_Actual.Text.Trim().Length > 10)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Folio Actual.";
            Validacion = false;
        }
        if (Txt_Folio_Nuevo.Text.Trim().Length == 0 || Txt_Folio_Nuevo.Text.Trim().Length > 10)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Folio Nuevo";
            Validacion = false;
        }

        if (!Folio_Existe()) 
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Ese Folio ya existe, teclee un Folio correcto";
            Validacion = false;
        
        }
        if (Txt_Motivo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Motivo por el cual se desea modificar el folio.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 18/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Folio_Existe() 
    {
        Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Folios = new Cls_Ope_Pre_Modifica_Folio_Pago_Negocio();
        Folios.P_Recibo = Txt_Folio_Nuevo.Text.Trim();
        DataTable Tabla = Folios.Folio_Existe();
        if (Tabla.Rows.Count == 1)
        {
            return false;
        }
        else 
        {
            return true;
        }
    }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Colonias
    ///DESCRIPCIÓN: Llena la tabla de Colonias
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Modifica_Folio_Pago(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Modificacion = new Cls_Ope_Pre_Modifica_Folio_Pago_Negocio();
            Modificacion.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Modificacion.P_No_Turno = Hfd_No_Turno.Value;
            Grid_Modifica_Folio_Pago.DataSource = Modificacion.Consultar_Folios();
            Grid_Modifica_Folio_Pago.PageIndex = Pagina;
            Grid_Modifica_Folio_Pago.Columns[1].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[6].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[7].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[9].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[10].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[11].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[12].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[13].Visible = true;
            Grid_Modifica_Folio_Pago.Columns[14].Visible = true;
            Grid_Modifica_Folio_Pago.DataBind();
            Grid_Modifica_Folio_Pago.Columns[1].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[6].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[7].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[9].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[10].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[11].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[12].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[13].Visible = false;
            Grid_Modifica_Folio_Pago.Columns[14].Visible = false;
        }

        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Colonias_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Colonias de auerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Modifica_Folio_Pago_Busqueda(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Modificaciones = new Cls_Ope_Pre_Modifica_Folio_Pago_Negocio();
            Modificaciones.P_Recibo = Txt_Busqueda.Text.ToUpper().Trim();
            Modificaciones.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Modificaciones.P_No_Turno = Hfd_No_Turno.Value;
            Grid_Modifica_Folio_Pago.DataSource = Modificaciones.Consultar_Folios_Busqueda();
            Grid_Modifica_Folio_Pago.PageIndex = Pagina;
            Grid_Modifica_Folio_Pago.Columns[1].Visible = true;
            Grid_Modifica_Folio_Pago.DataBind();
            Grid_Modifica_Folio_Pago.Columns[1].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Caja_Empleado
    /// DESCRIPCION : Consulta la caja que tiene abierto el empleado
    /// PARAMETROS  : 
    /// CREO        : Yazmin A Delgado Gómez
    /// FECHA_CREO  : 22-Agosto-2011
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Caja_Empleado()
    {
        DataTable Dt_Caja; //Variable que obtendra los datos de la consulta 
        Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Rs_Consulta_Cat_Pre_Cajas = new Cls_Ope_Pre_Modifica_Folio_Pago_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Rs_Consulta_Cat_Pre_Cajas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Caja = Rs_Consulta_Cat_Pre_Cajas.Consulta_Caja_Empleado();
            if (Dt_Caja.Rows.Count > 0)
            {
                //Muestra todos los datos que tiene el folio que proporciono el usuario
                foreach (DataRow Registro in Dt_Caja.Rows)
                {
                    Hfd_No_Turno.Value = Registro[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                    //Txt_Caja.Text = Registro["Caja"].ToString();
                    //Txt_Modulo.Text = Registro["Modulo"].ToString();
                    //Txt_Cajero.Text = Cls_Sessiones.Nombre_Empleado;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Caja_Empleado " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Colonias_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Colonias
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Modifica_Folio_Pago_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Modifica_Folio_Pago.SelectedIndex = (-1);
            Llenar_Tabla_Modifica_Folio_Pago(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Colonias_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de la Colonia Seleccionada para mostrarlos a detalle
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Modifica_Folio_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Modifica_Folio_Pago.SelectedIndex > (-1))
            {
                Txt_Pago_ID.Text = Grid_Modifica_Folio_Pago.SelectedRow.Cells[1].Text;
                Txt_Folio_Actual.Text = Grid_Modifica_Folio_Pago.SelectedRow.Cells[2].Text;
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nueva colonia
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                if (Grid_Modifica_Folio_Pago.Rows.Count > 0 && Grid_Modifica_Folio_Pago.SelectedIndex > (-1))
                {
                Configuracion_Formulario(false);
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Folio_Actual.Enabled = false;
                Grid_Modifica_Folio_Pago.Enabled = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Modificar.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Componentes_Generales())
                {

                    Cls_Ope_Pre_Modifica_Folio_Pago_Negocio Modificaciones = new Cls_Ope_Pre_Modifica_Folio_Pago_Negocio();
                    Modificaciones.P_No_Pago_ID= Txt_Pago_ID.Text.Trim();
                    Modificaciones.P_Folio_Actual = Txt_Folio_Actual.Text.Trim();
                    Modificaciones.P_Folio_Nuevo = Txt_Folio_Nuevo.Text.Trim();
                    Modificaciones.P_Motivo = Txt_Motivo.Text.ToUpper().Trim();
                    Modificaciones.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Limpiar_Catalogo();
                    Grid_Modifica_Folio_Pago.Columns[1].Visible = true;
                    Modificaciones.Alta_Modificacion();
                    Grid_Modifica_Folio_Pago.Columns[1].Visible = false;
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Modifica_Folio_Pago(Grid_Modifica_Folio_Pago.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Proceso de Modificacion de Folio de Pagos", "alert('Modificacion de Folio exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Modifica_Folio_Pago.Enabled = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Llena la Tabla de Colonias con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Llenar_Tabla_Modifica_Folio_Pago_Busqueda(0);
            Txt_Pago_ID.Text = "";
            Txt_Folio_Actual.Text = "";
            if (Grid_Modifica_Folio_Pago.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todos los Folios almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Modifica_Folio_Pago(0);
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 19/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Configuracion_Formulario(true);
                Limpiar_Catalogo();
                Llenar_Tabla_Modifica_Folio_Pago(0);
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Grid_Modifica_Folio_Pago.Enabled = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
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
            Botones.Add(Btn_Salir);
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
