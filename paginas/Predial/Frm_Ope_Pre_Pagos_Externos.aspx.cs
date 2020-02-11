using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Negocio;
using Presidencia.Operacion_Predial_Convenios_Derechos_Supervision.Negocio;
using Presidencia.Predial_Listado_Adeudos_Predial.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Instituciones_Recepcion_Pago_Predial.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Caja_Pagos.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Pagos_Externos : System.Web.UI.Page
{

    Boolean Cuenta_Predial_TextChanged_En_Proceso;

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Cuenta_Predial_TextChanged_En_Proceso = false;

            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                //Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Habilitar_Controles(false);
                Llenar_Grid_Pagos_Externos();
                Llenar_Grid_Adeudos_Cuenta_Predial();

                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");

                //Scrip para mostrar Ventana Modal de las Tasas de Traslado
                Session["ESTATUS_CUENTAS"] = "IN ('PENDIENTE','ACTIVA','VIGENTE','BLOQUEADA','SUSPENDIDA')";
                String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal);

                Cargar_Combo_Lugar_Pago();
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

    #region Regios métodos funcionamiento página.

    private void Cargar_Combo_Lugar_Pago()
    {
        Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio Instituciones = new Cls_Cat_Pre_Instituciones_Recepcion_Pagos_Predial_Negocio();
        DataTable Dt_Instituciones;
        DataRow Dr_Instituciones;

        Dt_Instituciones = Instituciones.Consultar_Instituciones_Reciben_Pago();
        Dr_Instituciones = Dt_Instituciones.NewRow();
        Dr_Instituciones[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id] = "SELECCIONE";
        Dr_Instituciones[Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion] = "<SELECCIONE>";
        Dt_Instituciones.Rows.InsertAt(Dr_Instituciones, 0);
        Cmb_Lugar_Pago.DataTextField = Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion;
        Cmb_Lugar_Pago.DataValueField = Cat_Pre_Instituciones_Que_Reciben_Pagos.Campo_Institucion_Id;
        Cmb_Lugar_Pago.DataSource = Dt_Instituciones;
        Cmb_Lugar_Pago.DataBind();
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    /////DESCRIPCIÓN: Metodo para configurar los componentes.
    /////PARAMETROS: Habilitado. Habilita o deshabilita el control.
    /////CREO: Miguel Angel Bedolla Moreno
    /////FECHA_CREO: 16/Diciembre/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Configuracion_Formulario(Boolean Habilitado)
    //{
    //    //Seccion Formulario Datos a Ingresar
    //    Btn_Busqueda_Avanzada_Cuentas_Predial.Enabled = Habilitado;
    //    Txt_Fecha_Pago.Enabled = false;
    //    Txt_No_Recibo.Enabled = false;
    //    Txt_No_Recibo_Folio.Enabled = false;
    //    Cmb_Lugar_Pago.Enabled = Habilitado;
    //    Txt_Cantidad_Pagada.Enabled = Habilitado;
    //    Txt_Descuento.Enabled = Habilitado;
    //    Txt_No_Operacion.Enabled = Habilitado;
    //    Txt_Fecha_Aplicacion.Enabled = false;
    //    Txt_Ajuste_Tarifario.Enabled = false;

    //    //Datos Cuenta Predial
    //    Txt_Cuenta_Predial.Enabled = false;
    //    Txt_Propietario.Enabled = false;
    //    Txt_Ubicacion.Enabled = false;
    //    Txt_No_Exterior.Enabled = false;
    //    Txt_No_Interior.Enabled = false;
    //    Txt_Estado_Cuenta.Enabled = false;
    //    Txt_Ciudad_Cuenta.Enabled = false;
    //    Txt_Codigo_Postal.Enabled = false;
    //    Txt_Domicilio_Foraneo.Enabled = false;
    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Campos
    ///DESCRIPCIÓN: Metodo encargado de limpiar los campos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 16/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Campos()
    {
        //Seccion Formulario Datos a Ingresar
        Txt_Fecha_Pago.Text = "";
        Txt_No_Folio.Text = "";
        Cmb_Lugar_Pago.SelectedIndex = 0;

        Txt_Desde_Periodo.Text = "";
        Cmb_Hasta_Periodo_Bimestre.SelectedIndex = -1;
        Cmb_Hasta_Periodo_Año.Items.Clear();

        //Datos Cuenta Predial
        Txt_Cuenta_Predial.Text = "";
        Txt_Propietario.Text = "";
        Txt_Ubicacion.Text = "";

        Txt_Desde_Periodo.Text = "";
        Txt_Periodo_Rezago.Text = "";
        Txt_Periodo_Corriente.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Corriente.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Subtotal.Text = "";
        Txt_Descuento_Pronto_Pago.Text = "";
        Txt_Descuento_Recargos_Ordinarios.Text = "";
        Txt_Descuento_Recargos_Moratorios.Text = "";
        Txt_Total.Text = "";

        Grid_Listado_Adeudos.DataBind();
        Grid_Listado_Adeudos_Convenio.DataBind();
    }

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Pagos_Externos
    ///DESCRIPCIÓN: Llena el Grid con los datos de la bd.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 16/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Pagos_Externos()
    {
        try
        {
            Grid_Pagos_Externos.DataBind();
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Adeudos_Cuenta_Predial
    ///DESCRIPCIÓN: Llena el Grid con los adeudos de la cuenta
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 16/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Adeudos_Cuenta_Predial()
    {
        try
        {
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Pagos_Externos_Selected_Index_Changed
    ///DESCRIPCIÓN: Carga los datos en los controles cuando se selecciona un elemento del Grid de pagos externos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 16/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Pagos_Externos_Selected_Index_Changed(object sender, EventArgs e)
    {

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Pagos_Externos_Page_Index_Changing
    ///DESCRIPCIÓN: Cambia de página el Grid.
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno
    ///FECHA_CREO: 16/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Pagos_Externos_Page_Index_Changing(object sender, GridViewPageEventArgs e)
    {

    }

    #endregion

    #region Eventos botones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Busqueda_Avanzada_Cuentas_Predial_Click
    ///DESCRIPCIÓN          : Obtiene el Id y Nombre de la Cuenta Predial de la Búsqueda Avanzada.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Txt_Cuenta_Predial_TextChanged(sender, null);
                //Cargar_Datos_Cuenta_Predial(Cuenta_Predial);
            }
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Evento de Botón para controlar el proceso de Nuevo y Dar de Alta
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Btn_Nuevo.ToolTip = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Nuevo.Visible = true;
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Modificar.Visible = false;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Habilitar_Controles(true); //Habilita los controles para la introducción de datos por parte del usuario
                Limpiar_Campos();
            }
            else
            {
                if (Validar_Campos_Obligatorios())
                {
                    if (Alta_Pago())
                    {
                        Btn_Nuevo.ToolTip = "Nuevo";
                        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                        Btn_Nuevo.Visible = true;
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Modificar.Visible = true;
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Habilitar_Controles(false);
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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Habilitar_Controles
    ///DESCRIPCIÓN          : Método parra habilitar/Deshabilitar los campos requeridos para entrada de datos.
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Habilitar_Controles(Boolean Habilitar)
    {
        //Btn_Busqueda_Avanzada_Cuentas_Predial.Enabled = Habilitar;
        Txt_Fecha_Pago.Enabled = Habilitar;
        Cmb_Lugar_Pago.Enabled = Habilitar;
        Txt_Desde_Periodo.Enabled = false;
        Cmb_Hasta_Periodo_Bimestre.Enabled = Habilitar;
        Cmb_Hasta_Periodo_Año.Enabled = Habilitar;

        Txt_Periodo_Rezago.Enabled = false;
        Txt_Periodo_Corriente.Enabled = false;
        Txt_Adeudo_Rezago.Enabled = Habilitar;
        Txt_Adeudo_Corriente.Enabled = Habilitar;
        Txt_Honorarios.Enabled = Habilitar;
        Txt_Gastos_Ejecucion.Enabled = Habilitar;
        Txt_Total_Recargos_Ordinarios.Enabled = Habilitar;
        Txt_Recargos_Moratorios.Enabled = Habilitar;
        Txt_Subtotal.Enabled = false;
        Txt_Descuento_Pronto_Pago.Enabled = Habilitar;
        Txt_Descuento_Recargos_Ordinarios.Enabled = Habilitar;
        Txt_Descuento_Recargos_Moratorios.Enabled = Habilitar;
        Txt_Total.Enabled = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Campos_Obligatorios
    ///DESCRIPCIÓN          : Valida los campo obligatorios de la página par captura
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Campos_Obligatorios()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_No_Folio.Text.Trim().Length == 0 && Txt_Cuenta_Predial.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el No. de Recibo o la Cuenta Predial.";
            Validacion = false;
        }
        if (Txt_Fecha_Pago.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Pago.";
            Validacion = false;
        }
        if (Cmb_Lugar_Pago.SelectedIndex == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar un Lugar de Pago.";
            Validacion = false;
        }
        if (Cmb_Hasta_Periodo_Año.SelectedIndex == -1)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar un Año Final.";
            Validacion = false;
        }
        if (Validacion == false)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Mensaje_Error;
            Img_Error.Visible = true;
        }
        else
        {
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Img_Error.Visible = false;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Evento de Botón para controlar el Modificar y Actualizar los datos de los Pagos
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        //String Mensajes_Error = "";

        //Mensaje_Error(null);

        //// si se asigno un valor al campo oculto con el id de la cuenta predial, hay un descuento seleccionado
        //if (Hdn_Cuenta_ID.Value != "")
        //{
        //    //if (Grid_Convenios.SelectedRow.Cells[9].Text.Replace("&nbsp;", "").Equals(""))
        //    //{
        //    try
        //    {

        //        if (Btn_Modificar.ToolTip.Equals("Modificar"))
        //        {
        //            // verificar que hay un numero de descuento seleccionado
        //            if (Hdn_No_Descuento.Value.Trim() != "")
        //            {
        //                // si el estatus es cancelado, no se permite la edicion
        //                if (Cmb_Estatus.SelectedValue == "CANCELADO")
        //                {
        //                    Mensaje_Error("No es posible modificar descuentos CANCELADOS.");
        //                }
        //                else if (Cmb_Estatus.SelectedValue == "APLICADO")
        //                {
        //                    Mensaje_Error("No es posible modificar el descuento porque ya fue aplicado.");
        //                }
        //                else
        //                {
        //                    // cargar datos del grid de parcialidades (para que muestre los controles editables del importe)
        //                    Habilitar_Controles("Modificar");
        //                }
        //            }
        //            else
        //            {
        //                Mensaje_Error("Seleccione el Registro que desea modificar.");
        //            }
        //        }
        //        else
        //        {
        //            Mensajes_Error = Validar_Componentes();

        //            //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
        //            if (Mensajes_Error.Length > 0)
        //            {
        //                Mensaje_Error(Mensajes_Error);
        //            }
        //            else
        //            {
        //                Modificar_Descuento();
        //            }
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        Lbl_Mensaje_Error.Text = "Modificar: " + Ex.Message;
        //        Lbl_Mensaje_Error.Visible = true;
        //        Img_Error.Visible = true;
        //    }

        //}
        //else if (Grid_Descuentos_Predial.SelectedIndex > -1)
        //{
        //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de impuesto predial", "alert('Seleccione un descuento a modificar, por favor.');", true);
        //}
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Evento de Botón para controlar el Salir de la página o Cancelar alguna operación de Nuevo o Modificar.
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        if (Btn_Salir.ToolTip.Equals("Salir"))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Habilitar_Controles(false);
            Limpiar_Campos();
            Btn_Nuevo.ToolTip = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Nuevo.Visible = true;
            Btn_Modificar.ToolTip = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Modificar.Visible = true;
            Btn_Salir.ToolTip = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Click
    ///DESCRIPCIÓN          : Evento de Botón para controlar el proceso de Búsqueda de Registros
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {

    }

    #endregion

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Adeudos
    ///DESCRIPCIÓN: Llena el grid de los Adeudos dependiendo de los seleccionados.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Adeudos(String P_Cuenta_Predial, Boolean Tomar_Filtrado)
    {
        Grid_Listado_Adeudos.Columns[1].Visible = true;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();

        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        if (Tomar_Filtrado)
        {
            RP_Negocio.P_Anio_Filtro = Convert.ToInt32(Cmb_Hasta_Periodo_Año.SelectedItem.Text);
            RP_Negocio.P_Bimestre_Filtro = Convert.ToInt32(Cmb_Hasta_Periodo_Bimestre.SelectedItem.Text);
        }
        DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
        if (Dt_Adeudos != null)
        {
            Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
            if (Dt_Adeudos.Select("ANIO <> " + Anio_Corriente.Consultar_Anio_Corriente().ToString()).Count() != 0)
            {
                if (Btn_Salir.ToolTip == "Cancelar")
                {
                    String Referencia = Txt_No_Folio.Text.Trim().ToUpper();
                    String Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
                    //Se cancela el Proceso de Alta.
                    Btn_Salir_Click(null, null);
                    if (Referencia.Length > 0)
                    {
                        Txt_No_Folio.Text = Referencia;
                        //Txt_No_Folio_TextChanged(null, null);
                        if (Referencia.StartsWith("CPRE") || Referencia.StartsWith("CTRA") || Referencia.StartsWith("CFRA") || Referencia.StartsWith("CDER"))
                        {
                            Consulta_Datos_Convenio(Referencia);
                        }
                        else
                        {
                            Mostrar_Informacion_Referencia(Referencia);
                        }
                    }
                    else
                    {
                        if (Cuenta_Predial.Length > 0)
                        {
                            Txt_Cuenta_Predial.Text = Cuenta_Predial;
                            //Txt_Cuenta_Predial_TextChanged(null, null);
                            Cargar_Datos_Cuenta_Predial(Cuenta_Predial);
                        }
                    }
                }
                //Se notifica que se encontraron Adeudos de Rezago en la Cuenta
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "La Cuenta Predial Tiene Adeudos con Rezago.";
                Img_Error.Visible = true;
            }
            //Se cargan los datos del Adeudo en el grid.
            Grid_Listado_Adeudos.DataSource = Dt_Adeudos;
            Grid_Listado_Adeudos.DataBind();
            Grid_Listado_Adeudos.Columns[1].Visible = false;
            if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
            {
                Llenar_Combo_Anios(Dt_Adeudos);
                Txt_Desde_Periodo.Text = Dt_Adeudos.Rows[0]["BIMESTRE"].ToString() + "/" + Dt_Adeudos.Rows[0]["ANIO"].ToString();
                Cmb_Hasta_Periodo_Año.SelectedValue = Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1]["ANIO"].ToString();
                Cmb_Hasta_Periodo_Bimestre.SelectedValue = Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1]["BIMESTRE"].ToString();
                Decimal Sum_Corriente = 0;
                foreach (DataRow Dr_Adeudos in Dt_Adeudos.Rows)
                {
                    if (Dr_Adeudos["CORRIENTE"] != null)
                    {
                        Sum_Corriente = Convert.ToDecimal(Dr_Adeudos["CORRIENTE"]);
                    }
                }
                Hfd_Adeudo_Actual.Value = Sum_Corriente.ToString();
            }
            else
            {
                Txt_Desde_Periodo.Text = "";
                Cmb_Hasta_Periodo_Año.SelectedIndex = -1;
                Cmb_Hasta_Periodo_Bimestre.SelectedIndex = -1;
            }
            //Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            //DataTable dt = GAP_Negocio.Calcular_Recargos_Predial(P_Cuenta_Predial);
            //Txt_Periodo_Actual.Text = GAP_Negocio.p_Periodo_Corriente;
            //Txt_Periodo_Rezago.Text = GAP_Negocio.p_Periodo_Rezago;
            Realizar_Calculos_Pago(false);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Adeudos
    ///DESCRIPCIÓN: Llena el Grid de Adeudos.
    ///PARAMETROS: 1. Dt_Datos. Datos para llenar el Grid.
    ///            2. Agrupar. Agrupar similares o no.
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Adeudos(DataTable Dt_Datos)
    {
        Grid_Listado_Adeudos.Columns[1].Visible = true;
        Grid_Listado_Adeudos.Columns[2].Visible = true;
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "ADEUDO_PREDIAL");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "CONSTANCIA");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "TRASLADO_DOMINIO");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "DERECHO_SUPERVISION");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "FRACCIONAMIENTO");
        Dt_Datos = Agrupar_Adeudos(Dt_Datos, "PASIVO");
        Dt_Datos.DefaultView.Sort = "[DESCRIPCION] ASC";
        Grid_Listado_Adeudos.DataSource = Dt_Datos;
        Grid_Listado_Adeudos.DataBind();
        Grid_Listado_Adeudos.Columns[1].Visible = false;
        Grid_Listado_Adeudos.Columns[2].Visible = false;

        //Valida si hay traslado y predial
        String Ref_Grid = "";
        String Ref_Cuenta = "";
        Boolean Tiene_Traslado = false;
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            Ref_Grid = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
            if (Ref_Grid.StartsWith("TD"))
            {
                Tiene_Traslado = true;
            }
            else
            {
                if (char.IsDigit(Ref_Grid, 1))
                {
                    Ref_Cuenta = Ref_Grid;
                }
            }
        }
        if (Tiene_Traslado && Ref_Cuenta != "")
        {
            Lbl_Mensaje_Error.Text = "";
            Img_Error.Visible = true;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('La cuenta consultada adeuda predial y tiene traslado(s) pendiente(s) de pagar, favor de recomendar al contribuyente que debe de pagar PRIMERO el ADEUDO predial.');", true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Agrupar_Adeudos
    ///DESCRIPCIÓN: Agrupa los Adeudos [Predial, Fraccionamiento y Derechos
    ///             de Supervisión].
    ///PARAMETROS: 1. Dt_Datos. Datos para llenar el Grid.
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Agrupar_Adeudos(DataTable Dt_Parametros, String Dato_Agrupar)
    {
        DataTable Dt_Datos = new DataTable();
        Int32 No_Fila = (-1);
        String Referencia = "";
        try
        {
            Dt_Datos.Columns.Add("IDENTIFICADOR", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("TIPO_CONCEPTO", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("REFERENCIA", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("FECHA", Type.GetType("System.DateTime"));
            Dt_Datos.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
            Dt_Datos.Columns.Add("IMPORTE", Type.GetType("System.Double"));

            for (Int32 Contador = 0; Contador < Dt_Parametros.Rows.Count; Contador++)
            {
                if (!Dt_Parametros.Rows[Contador]["TIPO_CONCEPTO"].ToString().Trim().Equals(Dato_Agrupar))
                {
                    Dt_Datos.ImportRow(Dt_Parametros.Rows[Contador]);
                }
                else
                {
                    No_Fila = -1;
                    Referencia = Dt_Parametros.Rows[Contador]["REFERENCIA"].ToString();
                    for (Int32 Cnt_Interno = 0; Cnt_Interno < Dt_Datos.Rows.Count; Cnt_Interno++)
                    {
                        if (Referencia.Trim().Equals(Dt_Datos.Rows[Cnt_Interno]["REFERENCIA"].ToString().Trim()))
                        {
                            No_Fila = Cnt_Interno;
                            break;
                        }
                    }
                    if (No_Fila > (-1))
                    {
                        Double Monto_Agregar = (Dt_Parametros.Rows[Contador]["IMPORTE"] != null) ? Convert.ToDouble(Dt_Parametros.Rows[Contador]["IMPORTE"]) : 0.0;
                        Double Monto_Acumulado = (Dt_Datos.Rows[No_Fila]["IMPORTE"] != null) ? Convert.ToDouble(Dt_Datos.Rows[No_Fila]["IMPORTE"]) : 0.0;
                        Dt_Datos.DefaultView.AllowEdit = true;
                        Dt_Datos.Rows[No_Fila].BeginEdit();
                        Dt_Datos.Rows[No_Fila]["IMPORTE"] = Monto_Acumulado + Monto_Agregar;
                        Dt_Datos.Rows[No_Fila].EndEdit();
                    }
                    else
                    {
                        if (((Dt_Parametros.Rows[Contador]["IMPORTE"] != null) ? Convert.ToDouble(Dt_Parametros.Rows[Contador]["IMPORTE"]) : 0.0) > 0.0)
                        {
                            Dt_Datos.ImportRow(Dt_Parametros.Rows[Contador]);
                            Referencia = Dt_Parametros.Rows[Contador]["REFERENCIA"].ToString();
                            No_Fila = Contador;
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("ADEUDO_PREDIAL"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Cuenta_Predial();
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                        else
                        {
                            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
                            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            Dt_Datos.DefaultView.AllowEdit = true;
                            Dt_Datos.Rows[No_Fila].BeginEdit();
                            Dt_Datos.Rows[No_Fila]["IMPORTE"] = Negocio.Consultar_Adeudos_Predial_Cuenta();
                            Dt_Datos.Rows[No_Fila].EndEdit();
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("TRASLADO_DOMINIO"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Traslado_Dominio(Referencia);
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("FRACCIONAMIENTO"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Fraccionamiento(Referencia);
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                    }
                }
            }
            if (Dato_Agrupar.Trim().Equals("DERECHO_SUPERVISION"))
            {
                if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
                {
                    if (No_Fila > (-1))
                    {
                        DataTable Dt_Convenio = Buscar_Convenio_Derechos_Supervision(Referencia);
                        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
                        {
                            if (Dt_Convenio.Rows.Count > 1)
                            {
                                for (Int32 Contador = 0; Contador < Dt_Convenio.Rows.Count; Contador++)
                                {
                                    DataRow Fila = Dt_Datos.NewRow();
                                    Fila["IDENTIFICADOR"] = Dt_Datos.Rows[No_Fila]["IDENTIFICADOR"];
                                    Fila["TIPO_CONCEPTO"] = Dt_Datos.Rows[No_Fila]["TIPO_CONCEPTO"];
                                    Fila["REFERENCIA"] = Dt_Datos.Rows[No_Fila]["REFERENCIA"];
                                    Fila["FECHA"] = Dt_Convenio.Rows[Contador]["FECHA"];
                                    Fila["DESCRIPCION"] = Dt_Convenio.Rows[Contador]["DESCRIPCION"];
                                    Fila["IMPORTE"] = Dt_Convenio.Rows[Contador]["MONTO"];
                                    Dt_Datos.Rows.Add(Fila);
                                }
                                Dt_Datos.Rows.RemoveAt(No_Fila);
                            }
                            else
                            {
                                Dt_Datos.DefaultView.AllowEdit = true;
                                Dt_Datos.Rows[No_Fila].BeginEdit();
                                Dt_Datos.Rows[No_Fila]["FECHA"] = Dt_Convenio.Rows[0]["FECHA"];
                                Dt_Datos.Rows[No_Fila]["DESCRIPCION"] = Dt_Convenio.Rows[0]["DESCRIPCION"].ToString();
                                Dt_Datos.Rows[No_Fila]["IMPORTE"] = Convert.ToDouble(Dt_Convenio.Rows[0]["MONTO"].ToString());
                                Dt_Datos.Rows[No_Fila].EndEdit();
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "['" + Ex.Message + "']";
            Img_Error.Visible = false;
        }
        return Dt_Datos;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Traslado_Dominio
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: Referencia, pasa el dato dela referencia del traslado
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 1/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Traslado_Dominio(String Referencia)
    {
        string No_Calculo = "";
        Int32 Anio_Calculo = 0;

        No_Calculo = Referencia.Substring(2);
        Anio_Calculo = 0;
        if (No_Calculo.Length > 4)
        {
            Anio_Calculo = Convert.ToInt16(No_Calculo.Substring(No_Calculo.Length - 4));
            No_Calculo = No_Calculo.Substring(0, No_Calculo.Length - 4);
        }

        DataTable Dt_Convenio = new DataTable();
        Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Convenio.Columns.Add("FECHA", Type.GetType("System.DateTime"));
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            Negocio.P_No_Traslado = Convert.ToInt64(No_Calculo).ToString("0000000000");
            Negocio.P_Anio_Traslado = Anio_Calculo;
            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Temporal = Negocio.Consultar_Convenio_Traslado_Dominio();
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                if (!Validar_Convenio_No_Imcumplido(Dt_Temporal))
                {
                    DataRow Fila = Dt_Convenio.NewRow();
                    Fila["NO_CONVENIO"] = Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim();
                    Fila["DESCRIPCION"] = "TRASLADO DE DOMINIO : No.Convenio " + Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[0]["PARCIALIDAD"].ToString().Trim();
                    Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[0]["IMPORTE"].ToString().Trim());
                    Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                    Dt_Convenio.Rows.Add(Fila);
                }
                else
                {
                    if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
                    {
                        for (Int32 Contador = 0; Contador < 1; Contador++)
                        {
                            DataRow Fila = Dt_Convenio.NewRow();
                            Fila["NO_CONVENIO"] = Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim();
                            Fila["DESCRIPCION"] = "TRASLADO DE DOMINIO : No.Convenio " + Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[Contador]["PARCIALIDAD"].ToString().Trim() +
                                                  "... INCUMPLIDO";
                            Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[Contador]["IMPORTE"].ToString().Trim());
                            Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[Contador]["FECHA_VENCIMIENTO"].ToString().Trim());
                            Dt_Convenio.Rows.Add(Fila);
                        }
                    }
                }
            }
        }
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Fraccionamiento
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: Referencia, pasa el dato dela referencia del fraccionamiento
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 13/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Fraccionamiento(String Referencia)
    {
        string No_Impuesto = "";
        Int32 Anio_Calculo = 0;

        No_Impuesto = Referencia.Substring(3);
        Anio_Calculo = 0;
        if (No_Impuesto.Length > 4)
        {
            Anio_Calculo = 2000 + Convert.ToInt16(No_Impuesto.Substring(0, 2));
            No_Impuesto = No_Impuesto.Substring(2);
        }

        DataTable Dt_Convenio = new DataTable();
        Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Convenio.Columns.Add("FECHA", Type.GetType("System.DateTime"));
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            Negocio.P_No_Impuesto_Fraccionamiento = Convert.ToInt64(No_Impuesto).ToString("0000000000");
            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Temporal = Negocio.Consultar_Convenio_Fraccionamiento();
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                if (!Validar_Convenio_No_Imcumplido(Dt_Temporal))
                {
                    DataRow Fila = Dt_Convenio.NewRow();
                    Fila["NO_CONVENIO"] = Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim();
                    Fila["DESCRIPCION"] = "IMPUESTO DE FRACCIONAMIENTO : No.Convenio " + Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[0]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim();
                    Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[0]["IMPORTE"].ToString().Trim());
                    Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                    Dt_Convenio.Rows.Add(Fila);
                }
                else
                {
                    if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
                    {
                        for (Int32 Contador = 0; Contador < 1; Contador++)
                        {
                            DataRow Fila = Dt_Convenio.NewRow();
                            Fila["NO_CONVENIO"] = Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim();
                            Fila["DESCRIPCION"] = "IMPUESTO DE FRACCIONAMIENTO : No.Convenio " + Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[Contador]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim() +
                                                  "... INCUMPLIDO";
                            Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[Contador]["IMPORTE"].ToString().Trim());
                            Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[Contador]["FECHA_VENCIMIENTO"].ToString().Trim());
                            Dt_Convenio.Rows.Add(Fila);
                        }
                    }
                }
            }
        }
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Derechos_Supervision
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: Referencia, pasa el dato dela referencia de derechos de supervision
    ///CREO: Ismael Prieto Sánchez
    ///FECHA_CREO: 13/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Derechos_Supervision(String Referencia)
    {
        string No_Impuesto = "";
        Int32 Anio_Calculo = 0;

        No_Impuesto = Referencia.Substring(3);
        Anio_Calculo = 0;
        if (No_Impuesto.Length > 4)
        {
            Anio_Calculo = 2000 + Convert.ToInt16(No_Impuesto.Substring(0, 2));
            No_Impuesto = No_Impuesto.Substring(2);
        }

        DataTable Dt_Convenio = new DataTable();
        Dt_Convenio.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
        Dt_Convenio.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Convenio.Columns.Add("FECHA", Type.GetType("System.DateTime"));
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
            Negocio.P_No_Impuesto_Derecho_Supervision = Convert.ToInt64(No_Impuesto).ToString("0000000000");
            Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Temporal = Negocio.Consultar_Convenio_Derechos_Supervision();
            if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
            {
                if (!Validar_Convenio_No_Imcumplido(Dt_Temporal))
                {
                    DataRow Fila = Dt_Convenio.NewRow();
                    Fila["NO_CONVENIO"] = Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim();
                    Fila["DESCRIPCION"] = "DERECHOS DE SUPERVISION : No.Convenio " + Dt_Temporal.Rows[0]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[0]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim();
                    Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[0]["IMPORTE"].ToString().Trim());
                    Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[0]["FECHA_VENCIMIENTO"].ToString().Trim());
                    Dt_Convenio.Rows.Add(Fila);
                }
                else
                {
                    if (Dt_Temporal != null && Dt_Temporal.Rows.Count > 0)
                    {
                        for (Int32 Contador = 0; Contador < 1; Contador++)
                        {
                            DataRow Fila = Dt_Convenio.NewRow();
                            Fila["NO_CONVENIO"] = Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim();
                            Fila["DESCRIPCION"] = "DERECHOS DE SUPERVISION : No.Convenio " + Dt_Temporal.Rows[Contador]["NO_CONVENIO"].ToString().Trim() + ", Parcialidad: " + Dt_Temporal.Rows[Contador]["PARCIALIDAD"].ToString().Trim() + "/" + Dt_Temporal.Rows[0]["NUMERO_PARCIALIDADES"].ToString().Trim() +
                                                  "... INCUMPLIDO";
                            Fila["MONTO"] = Convert.ToDouble(Dt_Temporal.Rows[Contador]["IMPORTE"].ToString().Trim());
                            Fila["FECHA"] = Convert.ToDateTime(Dt_Temporal.Rows[Contador]["FECHA_VENCIMIENTO"].ToString().Trim());
                            Dt_Convenio.Rows.Add(Fila);
                        }
                    }
                }
            }
        }
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Convenio_No_Imcumplido
    ///DESCRIPCIÓN: Valdia el Incumplimiento de un Convenio de Predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Convenio_No_Imcumplido(DataTable Dt_Parcialidades)
    {
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabiles = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        Boolean Convenio_Incumplido = false;
        DateTime Fecha_Actual = DateTime.Today;

        if (Dt_Parcialidades != null && Dt_Parcialidades.Rows.Count > 0)
        {
            for (Int32 Contador = 0; Contador < Dt_Parcialidades.Rows.Count; Contador++)
            {
                if (!String.IsNullOrEmpty(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString()))
                {
                    DateTime Fecha_Vencimiento = Convert.ToDateTime(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString());
                    Fecha_Vencimiento = Convert.ToDateTime(Dias_Inhabiles.Calcular_Fecha(Fecha_Vencimiento.ToShortDateString(), "10"));
                    if (Fecha_Vencimiento < Fecha_Actual)
                    {
                        Convenio_Incumplido = true;
                        break;
                    }
                }
            }
        }
        return Convenio_Incumplido;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Realizar_Calculos_Pago
    ///DESCRIPCIÓN: Realiza los Calculos del Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Realizar_Calculos_Pago(Boolean Existe_Convenio)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        //Declaración de las Variables para la realización de los Calculos.
        Double Corriente = 0;
        Double Rezagos = 0;
        Double Recargos_Ordinarios = 0;
        Double Recargos_Moratorios = 0;
        Double Honorarios = 0;
        Double Gastos_Ejecucion = 0;
        Double Subtotal = 0;
        Double Porcentaje_Descuento_Corriente = 0;
        Double Descuento_Corriente = 0;
        Double Porcentaje_Descuento_Recargos_Ordinarios = 0;
        Double Descuento_Recargos_Ordinarios = 0;
        Double Porcentaje_Descuento_Recargos_Moratorios = 0;
        Double Descuento_Recargos_Moratorios = 0;
        Double Porcentaje_Descuento_Honorarios = 0;
        Double Descuento_Honorarios = 0;
        Double Total = 0;
        Double Ajuste_Tarifario = 0;
        Double Total_Pagar = 0;

        //Se calcula el Adeudo y el Corriente en caso de no haber convenio
        if (Existe_Convenio)
        {
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
            {

                if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                    if (Chk_Seleccion_Adeudo.Checked)
                    {
                        Corriente = Corriente + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""));
                        Rezagos = Rezagos + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[8].Text.Trim().Replace("$", ""));
                        Recargos_Ordinarios = Recargos_Ordinarios + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[9].Text.Trim().Replace("$", ""));
                        Recargos_Moratorios = Recargos_Moratorios + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[10].Text.Trim().Replace("$", ""));
                        Honorarios = Honorarios + Convert.ToDouble(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[11].Text.Trim().Replace("$", ""));
                    }
                }
            }
            Obtener_Recargos_Moratorios();
            //Recargos_Moratorios += Convert.ToDouble(Txt_Recargos_Moratorios.Text);
        }
        else
        {
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                    if (Chk_Seleccion_Adeudo.Checked)
                    {
                        Corriente = Corriente + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""));
                        Rezagos = Rezagos + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", ""));
                        Recargos_Ordinarios = Recargos_Ordinarios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[9].Text.Trim().Replace("$", ""));
                        Recargos_Moratorios = Recargos_Moratorios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[10].Text.Trim().Replace("$", ""));
                        Honorarios = Honorarios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[11].Text.Trim().Replace("$", ""));
                    }
                }
            }

            DataTable Dt_Descuento_Pronto_Pago = Resumen_Predio.Consultar_Descuentos_Pronto_Pago();
            if (Dt_Descuento_Pronto_Pago.Rows.Count > 0)
            {
                if (Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMMMMMMMMMMMM}", DateTime.Now)].ToString().Trim() != "0")
                {
                    Porcentaje_Descuento_Corriente = Convert.ToDouble(Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMMMMMMMMMMMM}", DateTime.Now)].ToString().Trim()) / 100;
                }
                else
                {
                    Porcentaje_Descuento_Corriente = 0;
                }
            }
            else
            {
                Porcentaje_Descuento_Corriente = 0;
            }

            Resumen_Predio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            DataTable Dt_Descuentos_Recargos = Resumen_Predio.Consultar_Descuentos_Recargos();
            if (Dt_Descuentos_Recargos.Rows.Count > 0)
            {
                //Hfd_No_Descuento_Recargos.Value = Dt_Descuentos_Recargos.Rows[0]["No_Descuento_Predial"].ToString();
                Double Periodo_Descuento = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Hasta_Periodo_Anio"].ToString() + Convert.ToInt32(Dt_Descuentos_Recargos.Rows[0]["Hasta_Periodo_Bimestre"].ToString()).ToString());
                Double Periodo_Pago = 0;
                if (Cmb_Hasta_Periodo_Año.Items.Count > 0)
                {
                    Periodo_Pago = Convert.ToDouble(Cmb_Hasta_Periodo_Año.SelectedItem.Text + Convert.ToInt32(Cmb_Hasta_Periodo_Bimestre.SelectedItem.Text).ToString());
                }
                else
                {
                    Periodo_Pago = Convert.ToDouble(DateTime.Now.Year + Convert.ToInt32("6").ToString());
                }
                if (Periodo_Descuento == Periodo_Pago)
                {
                    if (!string.IsNullOrEmpty(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo"].ToString().Trim()))
                    {
                        Porcentaje_Descuento_Recargos_Ordinarios = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo"]) / 100;
                    }
                    else
                    {
                        Porcentaje_Descuento_Recargos_Ordinarios = 0;
                    }
                    if (!string.IsNullOrEmpty(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo_Moratorio"].ToString().Trim()))
                    {
                        Porcentaje_Descuento_Recargos_Moratorios = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo_Moratorio"]) / 100;
                    }
                    else
                    {
                        Porcentaje_Descuento_Recargos_Moratorios = 0;
                    }
                }
                else
                {
                    Porcentaje_Descuento_Recargos_Ordinarios = 0;
                    Porcentaje_Descuento_Recargos_Moratorios = 0;
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('EXISTE UN DESCUENTO QUE NO COINCIDE CON EL PERIODO A PAGAR, FAVOR DE VERIFICARLO');", true);
                    Lbl_Mensaje_Error.Text = "No es posible pagar el adeudo ya que tiene un descuento pero no coincide el periodo del mismo con el del pago.";
                    Img_Error.Visible = true;
                }
            }
            else
            {
                Porcentaje_Descuento_Recargos_Ordinarios = 0;
                Porcentaje_Descuento_Recargos_Moratorios = 0;
            }

            //Se calcula el Valor de los Descuentos
            Descuento_Corriente = (Corriente * Porcentaje_Descuento_Corriente);
            Descuento_Recargos_Ordinarios = (Recargos_Ordinarios * Porcentaje_Descuento_Recargos_Ordinarios);
            Descuento_Recargos_Moratorios = (Recargos_Moratorios * Porcentaje_Descuento_Recargos_Moratorios);
            Descuento_Honorarios = (Honorarios * Porcentaje_Descuento_Honorarios);
        }

        //Se calcula el Subtotal
        Subtotal = Rezagos + Corriente + Honorarios + Gastos_Ejecucion + Recargos_Ordinarios + Recargos_Moratorios;

        //Se calcula el Total Neto
        Total = Subtotal - Descuento_Corriente;
        Total = Total - Descuento_Recargos_Ordinarios;
        Total = Total - Descuento_Recargos_Moratorios;
        Total = Total - Descuento_Honorarios;

        //Se obtiene el Ajuste Tarifario y Total a Pagar
        Total_Pagar = Math.Round(Total);
        Ajuste_Tarifario = Total - Total_Pagar;

        //Se muestran los resultados
        //Txt_Adeudo_Rezago.Text = "$ " + Rezagos.ToString("#,####,###0.00");
        //Txt_Adeudo_Actual.Text = "$ " + Corriente.ToString("#,####,###0.00");
        //Txt_Total_Recargos_Ordinarios.Text = "$ " + Recargos_Ordinarios.ToString("#,####,###0.00");
        //Txt_Recargos_Moratorios.Text = "$ " + Recargos_Moratorios.ToString("#,####,###0.00");
        //Txt_Honorarios.Text = "$ " + Honorarios.ToString("#,####,###0.00");
        //Txt_Gastos_Ejecucion.Text = "$ " + Gastos_Ejecucion.ToString("#,####,###0.00");
        //Txt_SubTotal.Text = "$ " + Subtotal.ToString("#,####,###0.00");
        //Txt_Descuento_Corriente.Text = "$ " + Descuento_Corriente.ToString("#,####,###0.00");
        //Txt_Descuento_Recargos_Ordinarios.Text = "$ " + Descuento_Recargos_Ordinarios.ToString("#,####,###0.00");
        //Txt_Descuento_Recargos_Moratorios.Text = "$ " + Descuento_Recargos_Moratorios.ToString("#,####,###0.00");
        //Txt_Descuento_Honorarios.Text = "$ " + Descuento_Honorarios.ToString("#,####,###0.00");
        //Txt_Total.Text = "$ " + Total.ToString("#,####,###0.00");
        //Txt_Ajuste_Tarifario.Text = "$ " + (Ajuste_Tarifario * (-1)).ToString("#,####,###0.00");
        //Txt_Total_Pagar.Text = "$ " + Total_Pagar.ToString("#,####,###0.00");
        if (Existe_Convenio)
        {
            Cargar_Periodos_Parcialidades_Pagar();
        }
        else
        {
            Cargar_Periodos_Pagar();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Periodos_Pagar
    ///DESCRIPCIÓN: Obtiene los Periodos tanto de Rezsago como Actual y los muestra en
    ///             los Campos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 16 Octubre 2011 [Domingo ¬¬]
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Periodos_Pagar()
    {
        //Txt_Periodo_Actual.Text = "";
        //Txt_Periodo_Rezago.Text = "";
        if (Grid_Listado_Adeudos.Rows.Count > 0)
        {
            String Bimestre_Rezago_Inicial = "";
            String Anio_Rezago_Inicial = "";
            String Bimestre_Rezago_Final = "";
            String Anio_Rezago_Final = "";
            Boolean Capturo_Inicial_Rezago = false;
            String Bimestre_Corriente_Inicial = "";
            String Anio_Corriente_Inicial = "";
            String Bimestre_Corriente_Final = "";
            String Anio_Corriente_Final = "";
            Boolean Capturo_Inicial_Corriente = false;
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)(Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo"));
                    if (Chk_Seleccion_Adeudo.Checked)
                    {
                        if (Grid_Listado_Adeudos.Rows[Contador].Cells[2].Text.ToString().Trim().Equals("REZAGO"))
                        {
                            if (!Capturo_Inicial_Rezago)
                            {
                                Bimestre_Rezago_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                                Anio_Rezago_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                                Capturo_Inicial_Rezago = true;
                            }
                            Bimestre_Rezago_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                            Anio_Rezago_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                        }
                        else if (Grid_Listado_Adeudos.Rows[Contador].Cells[2].Text.ToString().Trim().Equals("CORRIENTE"))
                        {
                            if (!Capturo_Inicial_Corriente)
                            {
                                Bimestre_Corriente_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                                Anio_Corriente_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                                Capturo_Inicial_Corriente = true;
                            }
                            Bimestre_Corriente_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                            Anio_Corriente_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                        }
                    }
                }
            }
            if (Bimestre_Rezago_Inicial.Trim().Length > 0)
            {
                //Txt_Periodo_Rezago.Text = Bimestre_Rezago_Inicial + "/" + Anio_Rezago_Inicial + " - " + Bimestre_Rezago_Final + "/" + Anio_Rezago_Final;
            }
            if (Bimestre_Corriente_Inicial.Trim().Length > 0)
            {
                //if (Hfd_Cuota_Fija.Value == "")
                //{
                //    Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - " + Bimestre_Corriente_Final + "/" + Anio_Corriente_Final;
                //}
                //else
                //{
                //    Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - 6/" + Anio_Corriente_Final;
                //}
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Periodos_Parcialidades_Pagar
    ///DESCRIPCIÓN: Obtiene los Periodos tanto de Rezsago como Actual y los muestra en
    ///             los Campos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 28 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Periodos_Parcialidades_Pagar()
    {
        //Txt_Periodo_Actual.Text = "";
        //Txt_Periodo_Rezago.Text = "";
        if (Grid_Listado_Adeudos_Convenio.Rows.Count > 0)
        {
            String Bimestre_Rezago_Inicial = "";
            String Anio_Rezago_Inicial = "";
            String Bimestre_Rezago_Final = "";
            String Anio_Rezago_Final = "";
            Boolean Capturo_Inicial_Rezago = false;
            String Bimestre_Corriente_Inicial = "";
            String Anio_Corriente_Inicial = "";
            String Bimestre_Corriente_Final = "";
            String Anio_Corriente_Final = "";
            Boolean Capturo_Inicial_Corriente = false;
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                    if (Chk_Seleccion_Adeudo_Tmp.Checked)
                    {
                        if (Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[4].Text.Trim().Length > 0)
                        {
                            Int32 Anio_Registro = (Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[4].Text.Trim().Length > 0) ? Convert.ToInt32(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[4].Text.Trim()) : 0;
                            if (Anio_Registro < DateTime.Today.Year)
                            {
                                if (!Capturo_Inicial_Rezago)
                                {
                                    Bimestre_Rezago_Inicial = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring(0, 1));
                                    Anio_Rezago_Inicial = Anio_Registro.ToString();
                                    Capturo_Inicial_Rezago = true;
                                }
                                Bimestre_Rezago_Final = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring((Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Length - 1), 1));
                                Anio_Rezago_Final = Anio_Registro.ToString();
                            }
                            else
                            {
                                if (!Capturo_Inicial_Corriente)
                                {
                                    Bimestre_Corriente_Inicial = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring(0, 1));
                                    Anio_Corriente_Inicial = Anio_Registro.ToString();
                                    Capturo_Inicial_Corriente = true;
                                }
                                Bimestre_Corriente_Final = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Substring((Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[5].Text.Trim().Length - 1), 1));
                                Anio_Corriente_Final = Anio_Registro.ToString();
                            }
                        }
                    }
                }
            }

            if (Bimestre_Rezago_Inicial.Trim().Length > 0)
            {
                //Txt_Periodo_Rezago.Text = Bimestre_Rezago_Inicial + "/" + Anio_Rezago_Inicial + " - " + Bimestre_Rezago_Final + "/" + Anio_Rezago_Final;
            }
            if (Bimestre_Corriente_Inicial.Trim().Length > 0)
            {
                //Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - " + Bimestre_Corriente_Final + "/" + Anio_Corriente_Final;
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: obtener_Recargos_Moratorios
    /// DESCRIPCIÓN: Leer las parcialidades del ultimo convenio o reestructura para obtener los adeudos
    ///            a tomar en cuenta para la reestructura
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Obtener_Recargos_Moratorios()
    {
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Parcialidades = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Convenios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        DataTable Dt_Parcialidades;
        DataTable Dt_Convenios;
        Decimal Recargos_Moratorios = 0;
        Decimal Recargos_Ordinarios = 0;
        Decimal Honorarios = 0;
        Decimal Monto_Impuesto = 0;
        Decimal Monto_Base = 0;
        Decimal Adeudo_Honorarios = 0;
        Decimal Adeudo_Recargos = 0;
        Decimal Adeudo_Moratorios = 0;
        String No_Convenio = "";
        int Parcialidad = 0;
        DateTime Fecha_Vencimiento = DateTime.MinValue;
        int Meses_Transcurridos = 0;
        int Hasta_Anio = 0;
        int Hasta_Bimestre = 0;


        // consultar convenios de la cuenta
        Consulta_Convenios.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Consulta_Convenios.P_Ordenar_Dinamico = Ope_Pre_Convenios_Predial.Campo_Fecha + " DESC,"
            + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
        Dt_Convenios = Consulta_Convenios.Consultar_Convenio_Predial();
        // si la consulta arrojó resultado, utilizar el primer registro (convenio mas reciente)
        if (Dt_Convenios != null && Dt_Convenios.Rows.Count > 0)
        {
            No_Convenio = Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
            // consultar las parcialidades del ultimo convenio guardado (convenio o ultima reestructura)
            Consulta_Parcialidades.P_No_Convenio = No_Convenio;
            Dt_Parcialidades = Consulta_Parcialidades.Consultar_Parcialidades_Ultimo_Convenio();

            // llamar metodo para determinar si el convenio esta vencido
            if (Convenio_Vencido(Dt_Parcialidades))
            {
                // obtener el ultimo periodo incluido en el convenio
                if (Hasta_Anio <= 0 || Hasta_Bimestre <= 0)
                {
                    string Periodo_Parcialidad = Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString();
                    if (Periodo_Parcialidad.Trim().Length >= 13)
                    {
                        int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 4, 4), out Hasta_Anio);
                        int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 6, 1), out Hasta_Bimestre);
                    }
                }

                Parcialidad = Dt_Parcialidades.Rows.Count - 1;
                // recorrer la tabla de parcialidades hasta encontrar parcialidades con estatus PAGADO
                while (Parcialidad >= 0)
                {
                    // si la parcialidad tiene estatus diferente de PAGADO, sumar adeudos
                    if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() != "PAGADO")
                    {
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString(), out Honorarios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString(), out Recargos_Ordinarios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString(), out Recargos_Moratorios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString(), out Monto_Impuesto);
                        DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                        Adeudo_Honorarios += Honorarios;
                        Adeudo_Recargos += Recargos_Ordinarios;
                        Adeudo_Moratorios += Recargos_Moratorios;
                        Monto_Base += Monto_Impuesto;
                    }
                    Parcialidad--;
                }

                // agregar adeudos vencidos despues de convenio
                Monto_Base += Adeudos_Predial_Actuales_Despues_Convenio(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre);

                Meses_Transcurridos = Calcular_Meses_Entre_Fechas(Fecha_Vencimiento, DateTime.Now);
                Recargos_Moratorios = Calcular_Recargos_Moratorios(Monto_Base, Meses_Transcurridos);
            }
        }

        //Txt_Recargos_Moratorios.Text = Math.Round(Math.Round(Recargos_Moratorios + Adeudo_Moratorios, 3), 2).ToString("###,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Adeudos_Predial_Actuales_Despues_Convenio
    /// DESCRIPCIÓN: Regresa la suma de los adeudos vencidos despues del periodo indicado como parametro
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial para consultar adeudos
    /// 		2. Desde_Anio: Año del periodo inicial a tomar
    /// 		3. Desde_Bimestre: bimestre del periodo inicial a tomar
    /// CREO: Nombre del programador
    /// FECHA_CREO: 18-feb-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Adeudos_Predial_Actuales_Despues_Convenio(string Cuenta_Predial_ID, int Desde_Anio, int Desde_Bimestre)
    {
        decimal Adeudos_Despues_Convenio = 0;
        var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Adeudos;
        int Anio_Actual = DateTime.Now.Year;
        int Bimestre_Vencido = DateTime.Now.Month / 2;

        // periodo a partir del cual se va a tomar en cuenta (desde_bimestre + 1)
        Desde_Bimestre++;
        if (Desde_Bimestre > 6)
        {
            Desde_Bimestre = 1;
            Desde_Anio++;
        }

        // consultar adeudos actuales de la cuenta
        Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Hdf_Cuenta_Predial_ID.Value, "POR PAGAR", 0, 0);
        // agregar adeudos vencidos a la fecha
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            // recorrer todas las filas de la tabla de adeudos
            for (int Contador_Filas = 0; Contador_Filas < Dt_Adeudos.Rows.Count; Contador_Filas++)
            {
                int Anio_Adeudo;
                int.TryParse(Dt_Adeudos.Rows[Contador_Filas][Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio_Adeudo);
                // si el año es menor que Desde_Anio pasar al siguiente adeudo
                if (Anio_Adeudo < Desde_Anio)
                {
                    continue;
                }
                // si el año es menor que el año actual, agregar adeudos al monto total
                if (Anio_Adeudo < Anio_Actual)
                {
                    // recorrer todos los bimestres para agregar al adeudo
                    for (int Contador_Bimestres = 0; Contador_Bimestres <= 6; Contador_Bimestres++)
                    {
                        decimal Adeudo_Bimestre;
                        decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                        Adeudos_Despues_Convenio += Adeudo_Bimestre;
                    }
                }
                // si el año del adeudo es igual al año actual
                if (Anio_Adeudo == Anio_Actual)
                {
                    // recorrer los bimestres Desde_Bimestre hasta Bimestre_vencido
                    for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Bimestre_Vencido; Contador_Bimestres++)
                    {
                        decimal Adeudo_Bimestre;
                        decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                        Adeudos_Despues_Convenio += Adeudo_Bimestre;
                    }
                }

            } // for
        }

        return Adeudos_Despues_Convenio;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Convenio_Vencido
    /// DESCRIPCIÓN: Revisar las parcialidades en busca de parcialidades vencidas 
    ///             parcialidades sin pagar con fecha de vencimiento de hace mas de 10 dias habiles
    ///             Regresa verdadero si el convenio esta vencido.
    /// PARÁMETROS:
    /// 		1. Dt_Parcialidades: datatable con parcialidades de un convenio
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private bool Convenio_Vencido(DataTable Dt_Parcialidades)
    {
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        DateTime Fecha_Periodo;
        DateTime Fecha_Vencimiento;
        int Dias = 0;
        int Meses = 0;
        bool Convenio_Vencido = false;

        // recorrer las parcialidades del convenio
        for (int Pago = 0; Pago < Dt_Parcialidades.Rows.Count; Pago++)
        {
            // si el estatus de la parcialidad es INCUMPLIDO
            if (Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString().Trim() == "INCUMPLIDO")
            {
                // obtener la fecha de vencimiento de la parcialidad
                DateTime.TryParse(Dt_Parcialidades.Rows[Pago][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Periodo);
                Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo.ToShortDateString(), "10");
                // obtener el tiempo transcurrido desde la fecha de vencimiento
                Calcular_Tiempo_Entre_Fechas(Fecha_Vencimiento, DateTime.Now, out Dias, out Meses);
                // si el numero de dias transcurridos en mayor que cero, escribir fecha de vencimiento
                if (Dias > 0)
                {
                    Convenio_Vencido = true;
                }
                // abandonar el ciclo for
                break;
            }
        }
        if (!Convenio_Vencido)
        {
            DateTime Fecha_Actual = DateTime.Today;
            for (Int32 Contador = 0; Contador < Dt_Parcialidades.Rows.Count; Contador++)
            {
                if (!String.IsNullOrEmpty(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString()))
                {
                    Fecha_Vencimiento = Convert.ToDateTime(Dt_Parcialidades.Rows[Contador]["FECHA_VENCIMIENTO"].ToString());
                    Fecha_Vencimiento = Convert.ToDateTime(Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Vencimiento.ToShortDateString(), "10"));
                    if (Fecha_Vencimiento < Fecha_Actual)
                    {
                        Convenio_Vencido = true;
                        break;
                    }
                }
            }
        }
        return Convenio_Vencido;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Tiempo_Entre_Fechas
    /// DESCRIPCIÓN: Calcular numero de dias y meses transcurridos entre una fecha y otra
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inferior a tomar
    /// 		2. Hasta_Fecha: Fecha final hasta la que se calcula
    /// 		3. Dias: Se almacenan los dias de diferencia entre las fechas
    /// 		4. Meses: Almacena los meses transcurridos entre una fecha y otra
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Tiempo_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha, out Int32 Dias, out Int32 Meses)
    {
        TimeSpan Transcurrido = Hasta_Fecha - Desde_Fecha;
        if (Transcurrido > TimeSpan.Parse("0"))
        {
            DateTime Tiempo = DateTime.MinValue + Transcurrido;
            Meses = ((Tiempo.Year - 1) * 12) + (Tiempo.Month - 1);

            long tickDiff = Hasta_Fecha.Ticks - Desde_Fecha.Ticks;
            tickDiff = tickDiff / 10000000; // segundos
            Dias = (int)(tickDiff / 86400);
        }
        else
        {
            Dias = 0;
            Meses = 0;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Meses_Entre_Fechas
    /// DESCRIPCIÓN: Regresa un enteron con el numero de meses vencidos entre dos fechas
    ///             (tomando el primer dia de cada mes)
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inicial a comparar
    /// 		2. Hasta_Fecha: Fecha final a comparar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Int32 Calcular_Meses_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha)
    {
        DateTime Fecha_Inicial = Convert.ToDateTime(Desde_Fecha.Month + "/1" + "/" + Desde_Fecha.Year);
        DateTime Fecha_Final = Convert.ToDateTime(Hasta_Fecha.ToShortDateString());
        int Meses = 0;

        // aumentar el numero de meses mientras la fecha inicial mas los meses no supere la fecha final
        while (Fecha_Final > Fecha_Inicial.AddMonths(Meses))
        {
            Meses++;
        }

        return Meses;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Recargos_Moratorios
    /// DESCRIPCIÓN: Calcular los recargos moratorios para una cantidad a partir de una fecha dados
    ///             (el numero de meses por el porcentaje de recargos por el monto base)
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Recargos_Moratorios(Decimal Monto_Base, Int32 Meses)
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Parametros;
        Decimal Recargos_Moratorios = 0;
        Decimal Porcentaje_Recargos = 0;

        // recuperar el porcentaje de recargos moratorios de la tabla de parametros
        Dt_Parametros = Parametros.Consultar_Parametros();
        if (Dt_Parametros != null)
        {
            if (Dt_Parametros.Rows.Count > 0)
            {
                Decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString(), out Porcentaje_Recargos);
            }
        }

        // obtener el producto de los meses por el porcentaje de recargos
        Porcentaje_Recargos *= Meses;

        // calcular recargos
        Recargos_Moratorios = Monto_Base * Porcentaje_Recargos / 100;

        return Recargos_Moratorios;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Bimestre_Final_SelectedIndexChanged
    ///DESCRIPCIÓN: Actualiza el Listado de los Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Bimestre_Final_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Grid_Listado_Adeudos.DataSource = new DataTable();
            Grid_Listado_Adeudos.DataBind();
            Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_Final_SelectedIndexChanged
    ///DESCRIPCIÓN: Actualiza el Listado de los Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Anio_Final_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Grid_Listado_Adeudos.DataSource = new DataTable();
            Grid_Listado_Adeudos.DataBind();
            Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Pago_Click
    ///DESCRIPCIÓN: Recalcula el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Seleccion_Adeudo_CheckedChanged(object sender, EventArgs e)
    {
        Int32 No_Fila = Convert.ToInt32(((CheckBox)(sender)).TabIndex);
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                if (Contador < No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = true;
                }
                else if (Contador > No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = false;
                }
            }
        }
        Realizar_Calculos_Pago(false);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Chk_Seleccion_Parcialidad_CheckedChanged
    ///DESCRIPCIÓN: Recalcula el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 27 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Seleccion_Parcialidad_CheckedChanged(object sender, EventArgs e)
    {
        Int32 No_Fila = Convert.ToInt32(((CheckBox)(sender)).TabIndex);
        String No_Pago = Grid_Listado_Adeudos_Convenio.Rows[No_Fila].Cells[1].Text.Trim();
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                if (Contador < No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = true;
                }
                else if (Contador > No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = false;
                }
                if (Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[1].Text.Trim().Equals(No_Pago))
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = ((CheckBox)(sender)).Checked;
                }
            }
        }
        Realizar_Calculos_Pago(true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Adeudos_Convenio_RowDataBound
    ///DESCRIPCIÓN: Evento RowDataBound del Grid de Adeudos del Convenio.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_Adeudos_Convenio_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Parcialidad = (CheckBox)e.Row.FindControl("Chk_Seleccion_Parcialidad");
                Chk_Seleccion_Parcialidad.TabIndex = (short)e.Row.RowIndex;
                if (e.Row.Cells[2].Text.Trim().Equals("OBLIGATORIO"))
                {
                    Chk_Seleccion_Parcialidad.Enabled = false;
                }
                else if (e.Row.Cells[2].Text.Trim().Equals("OPCIONAL"))
                {
                    Chk_Seleccion_Parcialidad.Enabled = true;
                }
            }
            if (e.Row.Cells[4].Text.Trim().Length > 0 && e.Row.Cells[4].Text.Trim().Equals("0"))
            {
                e.Row.Cells[4].Text = "";
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Adeudos_RowDataBound
    ///DESCRIPCIÓN: Evento RowDataBound del Grid de Adeudos sin Convenio.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_Adeudos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo = (CheckBox)e.Row.FindControl("Chk_Seleccion_Adeudo");
                Chk_Seleccion_Adeudo.TabIndex = (short)e.Row.RowIndex;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Cuenta_Predial
    ///DESCRIPCIÓN: Carga los Campos con los Datos de la Cuenta predial Seleccionada.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Cuenta_Predial(String P_Cuenta_Predial)
    {
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        DataTable Dt_Datos = RP_Negocio.Consultar_Cuentas_Predial();
        if (Dt_Datos.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
            Txt_Cuenta_Predial.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim() : "";
            Hfd_Tipo_Predio.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["TIPO_PREDIO"].ToString().Trim())) ? Dt_Datos.Rows[0]["TIPO_PREDIO"].ToString().Trim() : "";
            Txt_Propietario.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim())) ? Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim() : "---------------------------";
            String Ubicacion = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["UBICACION"].ToString().Trim())) ? Dt_Datos.Rows[0]["UBICACION"].ToString().Trim() : "";
            Txt_Ubicacion.Text = (!Ubicacion.Trim().Equals("S/N COL.")) ? Ubicacion : "---------------------------";
            Hfd_Cuota_Fija.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUOTA_FIJA"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUOTA_FIJA"].ToString().Trim() : "";
            //Se verifica si hay o no convenios
            DataTable Dt_Convenio = Buscar_Convenio_Cuenta_Predial();
            if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
            {
                Visibilidad_Controles("PAGO_CONVENIO");
                //Txt_Convenio.Text = Dt_Convenio.Rows[0]["NO_CONVENIO"].ToString();
                Hdf_Convenio.Value = Dt_Convenio.Rows[0]["NO_CONVENIO"].ToString();
                Hfd_No_Descuento_Recargos.Value = Dt_Convenio.Rows[0]["NO_DESCUENTO"].ToString();
                Llenar_Grid_Convenio_Adeudos(Dt_Convenio);
            }
            else
            {
                Visibilidad_Controles("PAGO_NORMAL");
                if (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim()))
                {
                    Llenar_Grid_Adeudos(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim(), false);
                }
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se han encontrado datos para la Cuenta Predial.";
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Cuenta_Predial
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Buscar_Convenio_Cuenta_Predial()
    {
        DataTable Dt_Convenio = new DataTable();
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Negocio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Convenio = Negocio.Consultar_Convenio_Cuenta_Predia();
        return Dt_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Visibilidad_Controles
    ///DESCRIPCIÓN: Carga la Visibilidad de los controles.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Visibilidad_Controles(String Tipo_Visibilidad)
    {
        if (Tipo_Visibilidad.Trim().Equals("PAGO_NORMAL"))
        {
            //Txt_Convenio.Text = "SIN CONVENIO";
            Div_Listado_Adeudos_Predial.Visible = true;
            Div_Listado_Adeudos_Convenio.Visible = false;
        }
        else if (Tipo_Visibilidad.Trim().Equals("PAGO_CONVENIO"))
        {
            Div_Listado_Adeudos_Predial.Visible = false;
            Div_Listado_Adeudos_Convenio.Visible = true;
            //Txt_Bimestre_Inicial.Text = "-";
            //Txt_Anio_Inicial.Text = "-";
            Cmb_Hasta_Periodo_Bimestre.Items.Clear();
            Cmb_Hasta_Periodo_Bimestre.Items.Insert(0, new ListItem("-", ""));
            Cmb_Hasta_Periodo_Bimestre.Enabled = false;
            Cmb_Hasta_Periodo_Año.Items.Clear();
            Cmb_Hasta_Periodo_Año.Items.Insert(0, new ListItem("-", ""));
            Cmb_Hasta_Periodo_Año.Enabled = false;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Convenio_Adeudos
    ///DESCRIPCIÓN: Llena el grid de los Adeudos dependiendo de los seleccionados.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 28 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Convenio_Adeudos(DataTable Dt_Datos)
    {
        //Se llena el Grid de convenios de adeudos
        Grid_Listado_Adeudos_Convenio.Columns[1].Visible = true;
        Grid_Listado_Adeudos_Convenio.Columns[2].Visible = true;
        Grid_Listado_Adeudos_Convenio.DataSource = Dt_Datos;
        Grid_Listado_Adeudos_Convenio.DataBind();
        Grid_Listado_Adeudos_Convenio.Columns[1].Visible = false;
        Grid_Listado_Adeudos_Convenio.Columns[2].Visible = false;

        //Se seleccionan solo los adeudos obligatorios
        for (Int32 Cnt = 0; Cnt < Grid_Listado_Adeudos_Convenio.Rows.Count; Cnt++)
        {
            if (Grid_Listado_Adeudos_Convenio.Rows[Cnt].FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Parcialidad = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Cnt].FindControl("Chk_Seleccion_Parcialidad");
                if (Grid_Listado_Adeudos_Convenio.Rows[Cnt].Cells[2].Text.Trim().Equals("OBLIGATORIO"))
                {
                    Chk_Seleccion_Parcialidad.Checked = true;
                }
                else if (Grid_Listado_Adeudos_Convenio.Rows[Cnt].Cells[2].Text.Trim().Equals("OPCIONAL"))
                {
                    Chk_Seleccion_Parcialidad.Checked = false;
                }
            }
        }

        //Se ejecuta el metodo para calcular pagos
        Realizar_Calculos_Pago(true);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Anios
    ///DESCRIPCIÓN: Llena el Combo de los Años en los que existe un Adeudo.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Anios(DataTable Dt_Anios)
    {
        Dt_Anios = Dt_Anios.DefaultView.ToTable(true, "ANIO");
        Cmb_Hasta_Periodo_Año.DataSource = Dt_Anios;
        Cmb_Hasta_Periodo_Año.DataTextField = "ANIO";
        Cmb_Hasta_Periodo_Año.DataValueField = "ANIO";
        Cmb_Hasta_Periodo_Año.DataBind();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_No_Folio_TextChanged
    ///DESCRIPCIÓN          : Evento de TextBox para controlar la consulta y carga de datos de la Cuenta Predial y Adeudos.
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_No_Folio_TextChanged(object sender, EventArgs e)
    {
        String Referencia = Txt_No_Folio.Text.Trim().ToUpper();
        Limpiar_Campos();
        if (Referencia.Length > 0)
        {
            Txt_No_Folio.Text = Referencia;
            if (Referencia.StartsWith("CPRE") || Referencia.StartsWith("CTRA") || Referencia.StartsWith("CFRA") || Referencia.StartsWith("CDER"))
            {
                Consulta_Datos_Convenio(Referencia);
            }
            else
            {
                Mostrar_Informacion_Referencia(Referencia);
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Datos_Convenio
    ///DESCRIPCIÓN: Consulta los datos del convenio por la referencia
    ///PARAMETROS: 
    ///CREO: Ismael Prieto Sánchez  
    ///FECHA_CREO: 01/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Datos_Convenio(String Referencia)
    {
        DataTable Dt_Convenio;
        String No_Convenio = "";
        DataRow Registro;

        string No_Impuesto = "";

        //Valida para los convenios de predial
        if (Referencia.StartsWith("CPRE"))
        {
            Cls_Ope_Pre_Convenios_Predial_Negocio Rs_Convenio_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            No_Convenio = String.Format("{0:0000000000}", Convert.ToInt32(Referencia.Substring(4).ToString()));
            Rs_Convenio_Predial.P_Campos_Foraneos = true;
            Rs_Convenio_Predial.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, ESTATUS";
            Rs_Convenio_Predial.P_No_Convenio = No_Convenio;
            Dt_Convenio = Rs_Convenio_Predial.Consultar_Convenio_Predial();
            if (Dt_Convenio.Rows.Count > 0)
            {
                //Asigna el registro
                Registro = Dt_Convenio.Rows[0];
                //Valida que sea un convenio activo
                if (Registro["ESTATUS"].ToString() == "ACTIVO")
                {
                    //Asigna la información de la cuenta
                    Mostrar_Informacion_Cuenta_Predial(Registro["CUENTA_PREDIAL"].ToString(), false);
                    //Recorre para seleccionar el tipo convenio predial
                    for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
                    {
                        GridViewRow Dr_Formas_Pago = Grid_Listado_Adeudos.Rows[Contador];
                        String Concepto = Dr_Formas_Pago.Cells[5].Text.Trim();
                        if (Concepto.StartsWith("IMPUESTO DE PREDIAL : No.Convenio"))
                        {
                            Referencia = HttpUtility.HtmlDecode(Dr_Formas_Pago.Cells[3].Text.Trim());
                            break;
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " se encuentra " + Registro["ESTATUS"].ToString();
                    Img_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " no se encuentra registrado, favor de verificarlo.";
                Img_Error.Visible = true;
            }
        }
        //Valida para los convenios de traslado, fraccionamientos y derechos de supervision
        if (Referencia.StartsWith("CTRA") || Referencia.StartsWith("CFRA") || Referencia.StartsWith("CDER"))
        {
            DataTable Dt_Consulta = null;
            No_Impuesto = Referencia.Substring(4);
            if (Referencia.StartsWith("CTRA"))
            {
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Rs_Convenio_Traslado = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                Rs_Convenio_Traslado.P_Campos_Foraneos = true;
                Rs_Convenio_Traslado.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, NO_CALCULO, ESTATUS, ANIO";
                Rs_Convenio_Traslado.P_No_Convenio = Convert.ToInt64(No_Impuesto).ToString("0000000000");
                Dt_Consulta = Rs_Convenio_Traslado.Consultar_Convenio_Traslado_Dominio();
            }
            if (Referencia.StartsWith("CFRA"))
            {
                Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Rs_Convenio_Fraccionamiento = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
                Rs_Convenio_Fraccionamiento.P_Campos_Foraneos = true;
                Rs_Convenio_Fraccionamiento.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, NO_IMPUESTO_FRACCIONAMIENTO, ESTATUS, ANIO";
                Rs_Convenio_Fraccionamiento.P_No_Convenio = Convert.ToInt64(No_Impuesto).ToString("0000000000");
                Dt_Consulta = Rs_Convenio_Fraccionamiento.Consultar_Convenio_Fraccionamiento();
            }
            if (Referencia.StartsWith("CDER"))
            {
                Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio Rs_Convenio_Derechos = new Cls_Ope_Pre_Convenios_Derechos_Supervision_Negocio();
                Rs_Convenio_Derechos.P_Campos_Foraneos = true;
                Rs_Convenio_Derechos.P_Campos_Dinamicos = "NO_CONVENIO, CUENTA_PREDIAL_ID, NO_IMPUESTO_DERECHO_SUPERVISIO, ESTATUS, ANIO";
                Rs_Convenio_Derechos.P_No_Convenio = Convert.ToInt64(No_Impuesto).ToString("0000000000");
                Dt_Consulta = Rs_Convenio_Derechos.Consultar_Convenio_Derecho_Supervisions();
            }
            Dt_Convenio = Dt_Consulta;
            if (Dt_Convenio.Rows.Count > 0)
            {
                //Asigna el registro
                Registro = Dt_Convenio.Rows[0];
                //Forma el numero de impuesto
                if (Referencia.StartsWith("CTRA"))
                {
                    No_Impuesto = "TD" + Convert.ToDouble(Registro["No_Calculo"].ToString()).ToString() + Registro["Anio"].ToString();
                }
                if (Referencia.StartsWith("CFRA"))
                {
                    No_Impuesto = "IMP" + Registro["Anio"].ToString().Substring(2) + Registro["No_Impuesto_Fraccionamiento"].ToString();
                }
                if (Referencia.StartsWith("CDER"))
                {
                    No_Impuesto = "DER" + Registro["Anio"].ToString().Substring(2) + Registro["No_Impuesto_Derecho_Supervisio"].ToString();
                }
                //Valida que sea un convenio activo
                if (Registro["ESTATUS"].ToString() == "ACTIVO")
                {
                    //Asigna la información de la cuenta
                    Mostrar_Informacion_Cuenta_Predial(Registro["CUENTA_PREDIAL"].ToString(), false);
                    //Recorre para seleccionar el tipo convenio predial
                    for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
                    {
                        GridViewRow Dr_Formas_Pago = Grid_Listado_Adeudos.Rows[Contador];
                        String Concepto = Dr_Formas_Pago.Cells[5].Text.Trim();
                        if (Concepto.StartsWith("TRASLADO DE DOMINIO : No.Convenio") || Concepto.StartsWith("IMPUESTO DE FRACCIONAMIENTO : No.Convenio") || Concepto.StartsWith("DERECHOS DE SUPERVISION : No.Convenio"))
                        {
                            Referencia = HttpUtility.HtmlDecode(Dr_Formas_Pago.Cells[3].Text.Trim());
                            if (Referencia == No_Impuesto)
                            {
                                //Alta_Pasivo_Convenios(Referencia, Concepto);
                                //Direccionar_Pagina("Caja", Referencia);
                                break;
                            }
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " se encuentra " + Registro["ESTATUS"].ToString();
                    Img_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "El convenio " + Referencia + " no se encuentra registrado, favor de verificarlo.";
                Img_Error.Visible = true;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion_Referencia
    ///DESCRIPCIÓN: Se Muestra la información cuando se hace una busqueda por referencia.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 14 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion_Referencia(String Referencia)
    {
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        Negocio.P_Referencia = Referencia;
        DataTable Dt_Referencias = Negocio.Consultar_Adeudos_Referencia();
        if (Dt_Referencias.Rows.Count > 0)
        {
            DataRow Fila = Dt_Referencias.Rows[0];
            if (!String.IsNullOrEmpty(Fila["Cuenta_Predial_ID"].ToString()))
            {
                if (Referencia.StartsWith("TD"))
                {
                    Mostrar_Informacion_Cuenta_Predial(Fila["Cuenta_Predial_ID"].ToString(), false);
                }
                else
                {
                    Mostrar_Informacion_Cuenta_Predial(Fila["Cuenta_Predial_ID"].ToString(), true);
                    //Llenar_Grid_Adeudos(Dt_Referencias);
                }
            }
            else
            {
                Txt_Propietario.Text = Fila["Contribuyente"].ToString();
                //Llenar_Grid_Adeudos(Dt_Referencias);
                Grid_Listado_Adeudos.DataBind();
                Grid_Listado_Adeudos_Convenio.DataBind();
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se han encontrado datos para la Cuenta Predial.";
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Informacion_Cuenta_Predial
    ///DESCRIPCIÓN: Muestra la información de la Cuenta Predial.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 12 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Informacion_Cuenta_Predial(String Cuenta_Predial, Boolean Solo_Datos_Cuenta)
    {
        Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio Negocio = new Cls_Ope_Pre_Listado_Adeudos_Predial_Negocio();
        //String Ventana_Modal;
        //String Propiedades;

        Negocio.P_Referencia = Txt_No_Folio.Text.Trim();
        if (Solo_Datos_Cuenta)
        {
            Negocio.P_Cuenta_Predial_ID = Cuenta_Predial;
        }
        else
        {
            Negocio.P_Cuenta_Predial = Cuenta_Predial;
        }
        DataTable Dt_Datos = Negocio.Consultar_Datos_Cuentas_Predial();
        if (Dt_Datos != null && Dt_Datos.Rows.Count > 0)
        {
            Cargar_Datos_Cuenta_Predial(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim());
            //if (!Solo_Datos_Cuenta)
            //{
            //    Hdf_Cuenta_Predial_Id.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
            //    Txt_Cuenta_Predial.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim() : "";
            //}
            //Txt_Propietario.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim())) ? Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim() : "---------------------------";
            //String Ubicacion = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["UBICACION"].ToString().Trim())) ? Dt_Datos.Rows[0]["UBICACION"].ToString().Trim() : "";
            //Txt_Ubicacion.Text = (!Ubicacion.Trim().Equals("S/N COL.")) ? Ubicacion : "---------------------------";
            ////Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Caja.aspx";
            ////Propiedades = ", 'resizable=yes,status=no,width=750,scrollbars=yes');";
            ////Btn_Resumen_Cuenta.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text + "'" + Propiedades);
            ////Ventana_Modal = "Abrir_Ventana_Estado_Cuenta('Ventanas_Emergentes/Resumen_Predial/Frm_Estado_Cuenta.aspx";
            ////Propiedades = ", 'height=600,width=800,scrollbars=1');";
            ////Btn_Estado_Cuenta.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text + "'" + Propiedades);

            //if (!Solo_Datos_Cuenta)
            //{
            //    //Negocio.P_Cuenta_Predial = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim() : "";
            //    //Negocio.P_Cuenta_Predial_ID = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
            //    //Dt_Datos = Negocio.Consultar_Adeudos_Totales();
            //    //Llenar_Grid_Adeudos(Dt_Datos);
            //    Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_Id.Value, false);
            //}
        }
        else
        {
            Lbl_Mensaje_Error.Text = "No se ha encontrado la Cuenta predial";
            Img_Error.Visible = false;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Cuenta_Predial_TextChanged
    ///DESCRIPCIÓN          : Evento de TextBox para procesar el cambio de texto en el control
    ///PROPIEDADES:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 19/Enero/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {
        String Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim().ToUpper();
        if (!Cuenta_Predial_TextChanged_En_Proceso)
        {
            Cuenta_Predial_TextChanged_En_Proceso = true;
            Limpiar_Campos();
            if (Cuenta_Predial.Length > 0)
            {
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Cargar_Datos_Cuenta_Predial(Cuenta_Predial);
                Cargar_Adeudos_Cuenta(Hdf_Cuenta_Predial_ID.Value);
            }
            Cuenta_Predial_TextChanged_En_Proceso = false;
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Alta_Pago
    /// DESCRIPCION             : Da de Alta el Pago con los datos proporcionados por el usuario
    /// PARAMETROS: 
    /// CREO                    : Antonio Salvador Benavides Guardao
    /// FECHA_CREO              : 20/Enero/2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Alta_Pago()
    {
        Cls_Ope_Caj_Pagos_Negocio Pagos = new Cls_Ope_Caj_Pagos_Negocio();
        Boolean Estatus_Alta_Pago = false;
        try
        {
            Pagos.P_No_Recibo = "";// string.Format("{0:0000000000}", Convert.ToInt32(Txt_No_Recibo.Text));
            Pagos.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Pagos.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
            Pagos.P_Referencia = Txt_No_Folio.Text;
            //Pagos.P_Caja_ID = Txt_Caja_ID.Text;
            //Pagos.P_No_Caja = Hdn_No_Caja.Value;
            //Pagos.P_No_Turno = Txt_No_Turno.Text;
            Pagos.P_Fecha_Pago = DateTime.Now;// Convert.ToDateTime(Txt_Fecha_Aplicacion.Text);
            Pagos.P_Ajuste_Tarifario = 0;// Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$", ""));
            //Pagos.P_Total_Pagar = Convert.ToDouble(Txt_Total_Pagar_Ingreso.Text.Replace("$", ""));
            Pagos.P_Empleado_ID = Cls_Sessiones.Empleado_ID;
            Pagos.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
            //Pagos.P_Monto_Recargos=Convert.ToDouble(Txt_Importe_Recargos.Text);
            //Pagos.P_Monto_Corriente = Convert.ToDouble(Txt_Importe_Ingresos.Text);
            Pagos.P_Dt_Formas_Pago = Crear_Tabla_Formas_Pago();
            Pagos.P_Dt_Adeudos_Predial_Cajas = Crear_Tabla_Adeudos();
            Pagos.P_Dt_Adeudos_Predial_Cajas_Detalle = Crear_Tabla_Totales();
            Pagos.Alta_Pago_Caja(); //Da de alta el pago del ingreso
            //Hdn_No_Pago.Value = Pagos.P_No_Pago;
            //Txt_Estatus_Ingresos.Text = "PAGADO";
            Estatus_Alta_Pago = true;
        }
        catch (Exception ex)
        {
            Estatus_Alta_Pago = false;
            throw new Exception("Alta_Pago " + ex.Message.ToString(), ex);
        }
        return Estatus_Alta_Pago;
    }

    private DataTable Crear_Tabla_Formas_Pago()
    {
        DataTable Dt_Formas_Pago;
        DataRow Dr_Formas_Pago;
        //Agrega los datos del ajuste tarifario
        Dt_Formas_Pago = new DataTable();
        Dt_Formas_Pago.Columns.Add("Banco_ID", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("Banco", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("Forma_Pago", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("No_Transaccion", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("No_Autorizacion", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("Plan_Pago", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("Meses", typeof(System.Int16));
        Dt_Formas_Pago.Columns.Add("Monto", typeof(System.Double));
        Dt_Formas_Pago.Columns.Add("No_Cheque", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("Referencia_Transferencia", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("Referencia", typeof(System.String));
        Dt_Formas_Pago.Columns.Add("No_Tarjeta_Bancaria", typeof(System.String));

        Dr_Formas_Pago = Dt_Formas_Pago.NewRow();
        Dr_Formas_Pago["Forma_Pago"] = "PAGOS EXTERNOS";
        Dr_Formas_Pago["Monto"] = 0;// Convert.ToDouble(Txt_Ajuste_Tarifario.Text);
        Dt_Formas_Pago.Rows.Add(Dr_Formas_Pago);

        return Dt_Formas_Pago;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Adeudos
    ///DESCRIPCIÓN: Crea la Tabla con el listado y los montos de los adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Tabla_Adeudos()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();
        Dt_Adeudos_Predial.Columns.Add("NO_ADEUDO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_PAGO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("BIMESTRE", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("ANIO", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Double"));
        if (Hdf_Convenio.Value.Trim().Length > 0)
        {
            String BS = Obtener_Pagos_Seleccionados();
            if (BS.Trim().Length > 0)
            {
                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                Negocio.P_No_Convenio = Hdf_Convenio.Value.Trim();
                Negocio.P_No_Pagos = BS;
                DataTable Dt_Tmp_Parcialidades = Negocio.Obtener_Biemestres_A_Pagar();
                if (Dt_Tmp_Parcialidades != null && Dt_Tmp_Parcialidades.Rows.Count > 0)
                {
                    for (Int32 Cnt_Pacialidades = 0; Cnt_Pacialidades < Dt_Tmp_Parcialidades.Rows.Count; Cnt_Pacialidades++)
                    {
                        DataRow Fila_Nueva = Dt_Adeudos_Predial.NewRow();
                        Fila_Nueva["NO_CONVENIO"] = Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["NO_CONVENIO"].ToString();
                        Fila_Nueva["NO_PAGO"] = Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["NO_PAGO"].ToString();
                        Fila_Nueva["BIMESTRE"] = (!String.IsNullOrEmpty(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["BIMESTRE"].ToString())) ? Convert.ToInt32(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["BIMESTRE"]) : 0;
                        Fila_Nueva["ANIO"] = (!String.IsNullOrEmpty(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["ANIO"].ToString())) ? Convert.ToInt32(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["ANIO"]) : 0;
                        Fila_Nueva["MONTO"] = (!String.IsNullOrEmpty(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["MONTO"].ToString())) ? Convert.ToDouble(Dt_Tmp_Parcialidades.Rows[Cnt_Pacialidades]["MONTO"]) : 0.0;
                        Dt_Adeudos_Predial.Rows.Add(Fila_Nueva);
                    }
                }
                else
                {
                    for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
                    {
                        if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
                        {
                            CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                            if (Chk_Seleccion_Adeudo_Tmp.Checked)
                            {
                                DataRow Fila_Nueva = Dt_Adeudos_Predial.NewRow();
                                Fila_Nueva["NO_CONVENIO"] = Hdf_Convenio.Value.Trim();
                                Fila_Nueva["NO_PAGO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[1].Text.Trim());
                                Dt_Adeudos_Predial.Rows.Add(Fila_Nueva);
                                //DataRow Fila_Tmp = Dt_Adeudos_Predial.NewRow();
                                //Fila_Tmp["NO_ADEUDO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[1].Text.Trim());
                                //Fila_Tmp["BIMESTRE"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.Trim());
                                //Fila_Tmp["ANIO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
                                //Fila_Tmp["MONTO"] = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""))) + Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", "")));
                                //Dt_Adeudos_Predial.Rows.Add(Fila_Tmp);
                            }
                        }
                    }
                }
            }

        }
        else
        {
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                    if (Chk_Seleccion_Adeudo_Tmp.Checked)
                    {
                        DataRow Fila_Tmp = Dt_Adeudos_Predial.NewRow();
                        Fila_Tmp["NO_ADEUDO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[1].Text.Trim());
                        Fila_Tmp["BIMESTRE"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.Trim());
                        Fila_Tmp["ANIO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
                        Fila_Tmp["MONTO"] = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""))) + Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", "")));
                        Dt_Adeudos_Predial.Rows.Add(Fila_Tmp);
                    }
                }
            }
        }

        return Dt_Adeudos_Predial;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Totales
    ///DESCRIPCIÓN: Crea la Tabla con el listado y los montos de los totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Tabla_Totales()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();
        Dt_Adeudos_Predial.Columns.Add("CONCEPTO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Adeudos_Predial.Columns.Add("REFERENCIA", Type.GetType("System.String"));
        //Agrega la Fila de Importe Corriente
        DataRow Fila_Corriente = Dt_Adeudos_Predial.NewRow();
        Fila_Corriente["CONCEPTO"] = "CORRIENTE";
        Fila_Corriente["MONTO"] = Convert.ToDouble(Hfd_Adeudo_Actual.Value.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Corriente);
        //Agrega la Fila de Importe REzago
        //DataRow Fila_Rezago = Dt_Adeudos_Predial.NewRow();
        //Fila_Rezago["CONCEPTO"] = "REZAGO";
        //Fila_Rezago["MONTO"] = Convert.ToDouble(Hfd_Adeudo_Rezago.Value.Replace("$", "").Trim());
        //Dt_Adeudos_Predial.Rows.Add(Fila_Rezago);
        ////Agrega la Fila de Importe Honorarios
        //DataRow Fila_Honorarios = Dt_Adeudos_Predial.NewRow();
        //Fila_Honorarios["CONCEPTO"] = "HONORARIOS";
        //Fila_Honorarios["MONTO"] = Convert.ToDouble(Txt_Honorarios.Text.Replace("$", "").Trim());
        //Dt_Adeudos_Predial.Rows.Add(Fila_Honorarios);

        ////Agrega la Fila de Recargos
        //DataRow Fila_Recargos = Dt_Adeudos_Predial.NewRow();
        //Fila_Recargos["CONCEPTO"] = "RECARGOS";
        //Fila_Recargos["MONTO"] = Convert.ToDouble(Txt_Total_Recargos_Ordinarios.Text.Replace("$", "").Trim());
        //Dt_Adeudos_Predial.Rows.Add(Fila_Recargos);

        ////Agrega la Fila de Recargos Moratorios
        //DataRow Fila_Recargos_Moratorios = Dt_Adeudos_Predial.NewRow();
        //Fila_Recargos_Moratorios["CONCEPTO"] = "MORATORIOS";
        //Fila_Recargos_Moratorios["MONTO"] = Convert.ToDouble(Txt_Recargos_Moratorios.Text.Replace("$", "").Trim());
        //Dt_Adeudos_Predial.Rows.Add(Fila_Recargos_Moratorios);

        ////Agrega la Fila de Descuento Corriente
        //DataRow Fila_Descuento_Corriente = Dt_Adeudos_Predial.NewRow();
        //Fila_Descuento_Corriente["CONCEPTO"] = "DESCUENTOS_CORRIENTES";
        //Fila_Descuento_Corriente["MONTO"] = Convert.ToDouble(Txt_Descuento_Corriente.Text.Replace("$", "").Trim());
        //Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Corriente);

        ////Agrega la Fila de Descuento Honorarios
        //DataRow Fila_Descuento_Honorarios = Dt_Adeudos_Predial.NewRow();
        //Fila_Descuento_Honorarios["CONCEPTO"] = "DESCUENTOS_HONORARIOS";
        //Fila_Descuento_Honorarios["MONTO"] = Convert.ToDouble(Txt_Descuento_Honorarios.Text.Replace("$", "").Trim());
        //Fila_Descuento_Honorarios["REFERENCIA"] = Hfd_No_Descuento_Recargos.Value;
        //Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Honorarios);

        ////Agrega la Fila de Descuento Recargos
        //DataRow Fila_Descuento_Recargos = Dt_Adeudos_Predial.NewRow();
        //Fila_Descuento_Recargos["CONCEPTO"] = "DESCUENTOS_RECARGOS";
        //Fila_Descuento_Recargos["MONTO"] = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Replace("$", "").Trim());
        //Fila_Descuento_Recargos["REFERENCIA"] = Hfd_No_Descuento_Recargos.Value;
        //Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Recargos);

        ////Agrega la Fila de Descuento Moratorios
        //DataRow Fila_Descuento_Recargos_Moratorios = Dt_Adeudos_Predial.NewRow();
        //Fila_Descuento_Recargos_Moratorios["CONCEPTO"] = "DESCUENTOS_MORATORIOS";
        //Fila_Descuento_Recargos_Moratorios["MONTO"] = Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text.Replace("$", "").Trim());
        //Fila_Descuento_Recargos_Moratorios["REFERENCIA"] = Hfd_No_Descuento_Recargos.Value;
        //Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Recargos_Moratorios);
        return Dt_Adeudos_Predial;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Pagos_Seleccionados
    ///DESCRIPCIÓN: Obtiene los pagos seleccionados.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 29 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Pagos_Seleccionados()
    {
        String No_Pagos = "";
        Boolean Poner_Coma = false;
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos_Convenio.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos_Convenio.Rows[Contador].FindControl("Chk_Seleccion_Parcialidad");
                if (Chk_Seleccion_Adeudo_Tmp.Checked)
                {
                    if (Poner_Coma)
                    {
                        No_Pagos = No_Pagos + ", ";
                    }
                    No_Pagos = No_Pagos + Grid_Listado_Adeudos_Convenio.Rows[Contador].Cells[1].Text.Trim();
                    Poner_Coma = true;
                }
            }
        }
        return No_Pagos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Adeudos_Cuenta
    ///DESCRIPCIÓN          : Consulta los Adeudos y los muestra en pantalla.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 04/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cargar_Adeudos_Cuenta(String P_Cuenta_Predial)
    {
        Int32 Anio_Corriente = 0;
        var Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        var Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        var RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();

        DataTable Dt_Estado_Cuenta = new DataTable();
        String Fecha_Anio = Cmb_Hasta_Periodo_Año.SelectedValue.Trim();
        String Fecha_Mes = Cmb_Hasta_Periodo_Bimestre.SelectedValue.Trim();

        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Recargos_Ordinarios = 0;
        decimal Recargos_Moratorios = 0;
        decimal Honorarios = 0;
        String Periodo_Actual_Inicial = "";
        String Periodo_Actual_Final = "";
        String Periodo_Rezago_Inicial = "";
        String Periodo_Rezago_Final = "";
        int Hasta_Anio = 0;
        int hasta_Bimestre = 0;

        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Corriente.Text = "";

        // obtener año corriente
        Anio_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();
        // verificar que se obtuvo valor mayor que cero, si no, tomar año actual
        if (Anio_Corriente <= 0)
        {
            Anio_Corriente = DateTime.Now.Year;
        }

        if (Fecha_Anio == "")
        {
            Fecha_Anio = "0";
        }

        Dt_Estado_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(P_Cuenta_Predial, null, 0, Convert.ToInt16(Fecha_Anio));

        // limpiar controles
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Corriente.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "0.00";
        Txt_Recargos_Moratorios.Text = "0.00";
        Txt_Subtotal.Text = "0.00";
        Txt_Descuento_Pronto_Pago.Text = "0.00";
        Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
        Txt_Descuento_Recargos_Moratorios.Text = "0.00";
        Txt_Total.Text = "0.00";
        Txt_Desde_Periodo.Text = "";
        Txt_Periodo_Rezago.Text = "";
        Txt_Periodo_Corriente.Text = "";

        // tomar valores de combos periodo final si hay valores seleccionados
        if (Cmb_Hasta_Periodo_Año.SelectedIndex >= 0 && Cmb_Hasta_Periodo_Bimestre.SelectedIndex >= 0)
        {
            int.TryParse(Cmb_Hasta_Periodo_Año.SelectedValue, out Hasta_Anio);
            int.TryParse(Cmb_Hasta_Periodo_Bimestre.SelectedValue, out hasta_Bimestre);
        }

        for (int x = 0; x < Dt_Estado_Cuenta.Rows.Count; x++)
        {
            decimal Monto_Adeudo;
            int Anio_Adeudo;

            // recorrer los adeudos para extraer los periodos
            for (int Contador_Bimestre = 1; Contador_Bimestre <= 6; Contador_Bimestre++)
            {
                decimal.TryParse(Dt_Estado_Cuenta.Rows[x]["Adeudo_Bimestre_" + Contador_Bimestre].ToString().Trim(), out Monto_Adeudo);
                int.TryParse(Dt_Estado_Cuenta.Rows[x]["Anio"].ToString(), out Anio_Adeudo);
                // validar contra año y bimestre final seleccionado
                if (Anio_Adeudo == Hasta_Anio && Contador_Bimestre > hasta_Bimestre)
                {
                    break;
                }
                // si hay adeudo
                if (Monto_Adeudo >= 0)
                {
                    // año rezago, tomar dato para periodo rezago y periodo inicial
                    if (Anio_Adeudo < Anio_Corriente)
                    {
                        if (Periodo_Rezago_Inicial.Length <= 0)
                        {
                            Periodo_Rezago_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                        }
                        Periodo_Rezago_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                    }
                    // año corriente, tomar dato para periodo corriente
                    if (Anio_Adeudo == Anio_Corriente)
                    {
                        if (Periodo_Actual_Inicial.Length <= 0)
                        {
                            Periodo_Actual_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                        }
                        Periodo_Actual_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                    }
                }
            }

        }

        // si el combo con los años de los adeudos está vacío, llenarlo
        if (Cmb_Hasta_Periodo_Año.Items.Count == 0)
        {
            Llenar_Combo_Anios(Dt_Adeudos);
        }

        // cargar periodo inicial del periodo rezago o periodo corriente
        if (Periodo_Rezago_Inicial.Length > 0)
        {
            Txt_Periodo_Rezago.Text = Periodo_Rezago_Inicial + " - " + Cmb_Hasta_Periodo_Bimestre.SelectedValue + "/" + Cmb_Hasta_Periodo_Año.SelectedValue;
        }
        else if (Periodo_Actual_Inicial.Length > 0)
        {
            Txt_Desde_Periodo.Text = Periodo_Actual_Inicial;
        }
        // cargar cajas de texto de periodos rezago y corriente
        if (Periodo_Rezago_Inicial.Length > 0)
        {
            Txt_Periodo_Rezago.Text = Periodo_Rezago_Inicial + " - " + Periodo_Rezago_Final;
        }
        if (Periodo_Actual_Inicial.Length > 0)
        {
            Txt_Periodo_Corriente.Text = Periodo_Actual_Inicial + " - " + Periodo_Actual_Final;
        }
        if (Txt_Desde_Periodo.Text == "")
        {
            Txt_Desde_Periodo.Text = Periodo_Rezago_Inicial;
        }

        decimal Descuento_Pronto_Pago = 0;
        decimal Porcentaje_Descuento;
        decimal Subtotal = 0;
        decimal Total_Estado_Cuenta = 0;
        decimal Descuento_Recargos_Ordinarios = 0;
        decimal Descuento_Recargos_Moratorios = 0;
        decimal Cuota_Minima = 0;
        var GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        var Cuotas_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        var Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        var Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Pae_Honorarios_Negocio PAE_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();

        DataTable Dt_Cuotas_Minimas;
        DataTable Dt_Pagos;
        DataTable Dt_Datos_Cuenta;
        GAP_Negocio.Calcular_Recargos_Predial_Hasta_Bimestre(P_Cuenta_Predial, Convert.ToInt16(Fecha_Anio), Convert.ToInt16(Fecha_Mes));

        // obtener cuota minima
        Cuotas_Minima.P_Anio = DateTime.Now.Year.ToString();
        Dt_Cuotas_Minimas = Cuotas_Minima.Consultar_Cuotas_Minimas();
        if (Dt_Cuotas_Minimas != null && Dt_Cuotas_Minimas.Rows.Count > 0)
        {
            Decimal.TryParse(Dt_Cuotas_Minimas.Rows[0]["CUOTA"].ToString(), out Cuota_Minima);
        }

        // tomar datos de la consulta al método calcular recargos en la clase generar adeudos predial
        Adeudo_Corriente = GAP_Negocio.p_Total_Corriente;
        Adeudo_Rezago = GAP_Negocio.p_Total_Rezago;
        Recargos_Ordinarios = GAP_Negocio.p_Total_Recargos_Generados;
        Txt_Adeudo_Corriente.Text = Adeudo_Corriente.ToString("##,###,##0.00");
        Txt_Adeudo_Rezago.Text = Adeudo_Rezago.ToString("##,###,##0.00");
        Txt_Total_Recargos_Ordinarios.Text = Recargos_Ordinarios.ToString("##,###,##0.00");

        DataTable Dt_PAE_Honorarios = null;
        PAE_Honorarios.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Dt_PAE_Honorarios = PAE_Honorarios.Consultar_Total_Honorarios();
        if (Dt_PAE_Honorarios != null)
        {
            if (Dt_PAE_Honorarios.Rows.Count > 0)
            {
                if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"] != null)
                {
                    if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString().Trim() != "")
                    {
                        Honorarios = Convert.ToDecimal(Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"]);
                    }
                }
            }
        }
        Txt_Honorarios.Text = Honorarios.ToString("#,##0.00");
        Txt_Gastos_Ejecucion.Text = "0.00";

        // consultar y mostrar recargos moratorios
        var Consulta_Moratorios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Consulta_Moratorios.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Recargos_Moratorios = Consulta_Moratorios.Obtener_Recargos_Moratorios();
        Txt_Recargos_Moratorios.Text = Recargos_Moratorios.ToString("#,##0.00");

        // calcular subtotal
        Subtotal = Adeudo_Rezago + Adeudo_Corriente + Recargos_Ordinarios + Recargos_Moratorios + Honorarios;
        Txt_Subtotal.Text = Subtotal.ToString("#,##0.00");

        // consultar pagos de la cuenta
        Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Consultar_Pagos.p_Estatus = "PAGADO";
        Consultar_Pagos.p_Periodo_Corriente = " LIKE '%" + Anio_Corriente + "%'";
        Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Predial_Por_Periodo();

        // consultar si la cuenta tiene beneficio
        string Cuota_Fija = "";
        Cuenta_Predial.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Dt_Datos_Cuenta = Cuenta_Predial.Consultar_Cuenta();
        if (Dt_Datos_Cuenta != null && Dt_Datos_Cuenta.Rows.Count > 0)
        {
            Cuota_Fija = Dt_Datos_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString().Trim().ToUpper();
        }
        // solo si el adeudo consultado es hasta el sexto bimestre del año corriente, calcular descuento por pronto pago (no debe tener cuota fija)
        if (Txt_Periodo_Corriente.Text.Contains("6/" + Anio_Corriente) && (Dt_Pagos == null || Dt_Pagos.Rows.Count == 0) && !Cuota_Fija.Contains("SI"))
        {
            DataTable Dt_Descuento_Pronto_Pago = Resumen_Predio.Consultar_Descuentos_Pronto_Pago();
            if (Dt_Descuento_Pronto_Pago != null && Dt_Descuento_Pronto_Pago.Rows.Count > 0)
            {
                decimal.TryParse(
                    Dt_Descuento_Pronto_Pago.Rows[0][DateTime.Now.ToString("MMMM")].ToString().Trim(),
                    out Porcentaje_Descuento);
                Descuento_Pronto_Pago = Adeudo_Corriente * Porcentaje_Descuento / 100;
                // validar que el descuento restado al adeudo corriente no sea menor que la cuota minima
                if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento_Pronto_Pago < Cuota_Minima)
                {
                    Descuento_Pronto_Pago = Adeudo_Corriente - Cuota_Minima;
                }
                // validar descuentos negativos
                if (Descuento_Pronto_Pago < 0)
                {
                    Descuento_Pronto_Pago = 0;
                }
                // mostrar resultado
                Txt_Descuento_Pronto_Pago.Text = Descuento_Pronto_Pago.ToString("#,##0.00");
            }
        }
        else
        {
            Txt_Descuento_Pronto_Pago.Text = "0.00";
        }
        // consultar descuento a recargos
        Resumen_Predio.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        DataTable Dt_Descuentos_Recargos = Resumen_Predio.Consultar_Descuentos_Recargos();
        if (Dt_Descuentos_Recargos != null && Dt_Descuentos_Recargos.Rows.Count > 0)
        {
            decimal.TryParse(Dt_Descuentos_Recargos.Rows[0]["Desc_Recargo"].ToString().Trim(), out Descuento_Recargos_Ordinarios);
            decimal.TryParse(Dt_Descuentos_Recargos.Rows[0]["Desc_Recargo_Moratorio"].ToString(), out Descuento_Recargos_Moratorios);

        }
        Txt_Descuento_Recargos_Ordinarios.Text = Descuento_Recargos_Ordinarios.ToString("#,##0.00");
        Txt_Descuento_Recargos_Moratorios.Text = Descuento_Recargos_Moratorios.ToString("#,##0.00");

        // adeudo total (aplicar los descuentos solo a los recargos correspondientes)
        Total_Estado_Cuenta = Subtotal - Descuento_Pronto_Pago
            - (Recargos_Ordinarios - Descuento_Recargos_Ordinarios < 0 ? Recargos_Ordinarios : Descuento_Recargos_Ordinarios)
            - (Recargos_Moratorios - Descuento_Recargos_Moratorios < 0 ? Recargos_Moratorios : Descuento_Recargos_Moratorios);
        Txt_Total.Text = String.Format("{0:#,##0.00}", Convert.ToDecimal(Total_Estado_Cuenta.ToString()));
    }
}
