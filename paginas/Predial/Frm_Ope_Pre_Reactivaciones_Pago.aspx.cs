using System;
using System.Collections;
using System.Collections.Generic;
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
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Operacion_ReActivar_Pago.Negocio;
using Presidencia.Catalogo_Motivos.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Reactivaciones_Pago : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load.
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página.
    ///PROPIEDADES:
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Julio/2011 
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
                //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Configuracion_Acceso("Frm_Ope_Pre_Reactivaciones_Pago.aspx");
                Configuracion_Formulario(true);
                Consulta_Caja_Empleado();
               // Llenar_Combo_Motivos();
                Llenar_Cancelaciones(0);
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
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        Btn_Nuevo.Visible = true;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ToolTip = "Nuevo";
        Btn_Nuevo.OnClientClick = "";
        Btn_Salir.ToolTip = "Salir";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Grid_Cancelacion.Enabled = estatus;
        Grid_Cancelacion.SelectedIndex = (-1);
        Btn_Buscar.Enabled = estatus;
        Txt_Busqueda.Enabled = estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Busqueda.Text = "";
        Txt_Id.Text = "";
        Txt_Observaciones.Text = "";
        Txt_Monto.Text = "";
        Txt_Fecha_Pago.Text = "";
        Txt_No_Recibo.Text = "";
        Txt_No_Operacion.Text = "";
        Txt_Motivo.Text = "";
    }

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Cancelaciones
    ///DESCRIPCIÓN: Llena la tabla de Cancelaciones de pagos
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Cancelaciones(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_ReActivar_Pago_Negocio Cancelacion = new Cls_Ope_Pre_ReActivar_Pago_Negocio();
            Cancelacion.P_Filtro = Txt_Busqueda.Text.Trim().ToUpper();
            Cancelacion.P_No_Turno = Txt_No_Turno.Text;
            Grid_Cancelacion.Columns[1].Visible = true;
            Grid_Cancelacion.Columns[6].Visible = true;
            Grid_Cancelacion.Columns[8].Visible = true;
            Grid_Cancelacion.DataSource = Cancelacion.Consultar_Cancelacion_Pagos();
            Grid_Cancelacion.PageIndex = Pagina;
            Grid_Cancelacion.DataBind();
            Grid_Cancelacion.Columns[1].Visible = false;
            Grid_Cancelacion.Columns[6].Visible = true;
            Grid_Cancelacion.Columns[8].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Motivos
    ///DESCRIPCIÓN: Llena el combo de Motivos
    ///PROPIEDADES:         
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    //private void Llenar_Combo_Motivos()
    //{
    //    try
    //    {
    //        Cls_Cat_Pre_Motivos_Negocio Motivos = new Cls_Cat_Pre_Motivos_Negocio();
    //        DataTable tabla = Motivos.Consultar_Motivo_Nombre_Id();
    //        DataRow fila = tabla.NewRow();
    //        fila[Cat_Pre_Motivos.Campo_Motivo_ID] = "SELECCIONE";
    //        fila[Cat_Pre_Motivos.Campo_Nombre] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //        tabla.Rows.InsertAt(fila, 0);
    //        Cmb_Motivo.DataSource = tabla;
    //        Cmb_Motivo.DataValueField = Cat_Pre_Motivos.Campo_Motivo_ID;
    //        Cmb_Motivo.DataTextField = Cat_Pre_Motivos.Campo_Nombre;
    //        Cmb_Motivo.DataBind();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Text = Ex.Message;
    //        Img_Error.Visible = true;
    //        Lbl_Mensaje_Error.Visible = true;
    //    }
    //}

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
        Cls_Ope_Pre_ReActivar_Pago_Negocio Rs_Consulta_Cat_Pre_Cajas = new Cls_Ope_Pre_ReActivar_Pago_Negocio(); //Variable de conexión hacia la capa de Negocios

        try
        {
            Rs_Consulta_Cat_Pre_Cajas.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Dt_Caja = Rs_Consulta_Cat_Pre_Cajas.Consulta_Caja_Empleado();
            if (Dt_Caja.Rows.Count > 0)
            {
                //Muestra todos los datos que tiene el folio que proporciono el usuario
                foreach (DataRow Registro in Dt_Caja.Rows)
                {
                    Txt_No_Turno.Text = Registro[Ope_Caj_Turnos.Campo_No_Turno].ToString();
                    Txt_Caja.Text = Registro["Caja"].ToString();
                    Txt_Modulo.Text = Registro["Modulo"].ToString();
                    Txt_Cajero.Text = Cls_Sessiones.Nombre_Empleado;
                    Txt_Fecha.Text = DateTime.Now.ToString("dd/MMM/yyyy");
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
    ///NOMBRE DE LA FUNCIÓN: Grid_Cancelaciones_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Salarios Minimos 
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 20/Junio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cancelaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Cancelacion.SelectedIndex = (-1);
        Llenar_Cancelaciones(e.NewPageIndex);
        Limpiar_Catalogo();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Cancelacion_SelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de una cancelación de pago seleccionado para mostrarlo a detalle
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 28/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Cancelacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Cancelacion.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();
                String ID_Seleccionado = Grid_Cancelacion.SelectedRow.Cells[1].Text;
                Cls_Ope_Pre_ReActivar_Pago_Negocio Cancelacion = new Cls_Ope_Pre_ReActivar_Pago_Negocio();
                Cancelacion.P_No_Pago = ID_Seleccionado;
                Cancelacion = Cancelacion.Consultar_Datos_cancelacion_Pagos();
                Txt_Id.Text = Cancelacion.P_No_Pago;
                Txt_Caja.Text = Cancelacion.P_Caja;
                Txt_No_Operacion.Text = Cancelacion.P_No_Operacion.ToString();
                Txt_Cajero.Text = Cancelacion.P_Cajero;
                Txt_Fecha_Pago.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Cancelacion.P_Fecha));
                Txt_Modulo.Text = Cancelacion.P_Modulo;
                Txt_Monto.Text = "$" + String.Format("{0:#,###,##0.00}", Convert.ToDouble(Cancelacion.P_Monto));
                Txt_No_Recibo.Text = Cancelacion.P_No_Recibo.ToString();
                Txt_Observaciones.Text = Cancelacion.P_Comentarios;
                Txt_Motivo.Text = Cancelacion.P_Motivo_Cancelacion_Id;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Salario Minimo
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 20/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                if (Grid_Cancelacion.Rows.Count > 0 && Grid_Cancelacion.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Nuevo.OnClientClick = "return confirm('¿Esta seguro de ReActivar el Pago Seleccionado?');";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el registro que quiere modificar.";
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                }
            }
            else
            {
                    Cls_Ope_Pre_ReActivar_Pago_Negocio Cancelacion = new Cls_Ope_Pre_ReActivar_Pago_Negocio();
                    Cancelacion.P_Motivo_Cancelacion_Id = "";
                    Cancelacion.P_Comentarios = "";
                    Cancelacion.P_No_Pago = Txt_Id.Text.ToUpper();
                    Cancelacion.P_No_Recibo = Convert.ToInt32(Txt_No_Recibo.Text.ToUpper());
                    Cancelacion.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Cancelacion.Reactivar_Pago();
                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Cancelaciones(Grid_Cancelacion.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Formulario de Operación de Re-Activación de Pagos", "alert('Alta de Re-Activación de pago Exitoso');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Nuevo.OnClientClick = "";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

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
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Cancelacion_Click
    ///DESCRIPCIÓN: Llena la Tabla con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: Miguel Angel Bedolla Moreno.
    ///FECHA_CREO: 28/Julio/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Cancelacion_Click(object sender, ImageClickEventArgs e)
    {
        Grid_Cancelacion.SelectedIndex = (-1);
        Llenar_Cancelaciones(0);
        Limpiar_Catalogo();
        if (Grid_Cancelacion.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
        {
            Lbl_Mensaje_Error.Text = "Para la Busqueda con el Identificador \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Txt_Busqueda.Text = "";
            Llenar_Cancelaciones(0);
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
