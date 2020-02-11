using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text.RegularExpressions;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Bitacora_Eventos;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Operacion.Predial_Tasas_Anuales.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Validacion_Orden_Variacion : System.Web.UI.Page
{

    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 2;
    private const int Const_Estado_Modificar = 3;

    private static DataTable Dt_Agregar_Copropietarios = new DataTable();
    private static DataTable Dt_Agregar_Diferencias = new DataTable();

    private enum Busqueda_Ordenes_Variacion_Por
    {
        Orden_Variacion,
        Cuenta_Predial
    }

    private Int16 Año_Orden_Variacion;
    private String Fecha_Orden_Variacion;
    private String No_Orden_Variacion;
    private String No_Contrarecibo;
    private String Cuenta_Predial;
    private String Cuenta_Predial_ID;
    #endregion

    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            }
            Obtener_Valores_Parametros();
            if (!IsPostBack)
            {
                //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Inicializa_Controles();

                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");

                Cargar_Datos_Cuenta_Orden_Variacion();
            }

            Mensaje_Error();
        }
        catch
        {
            Mensaje_Error();
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION : Obtener_Valores_Sesiones
    ///DESCRIPCION          : Setea las variables de la clase con los valores de las sesiones existentes.
    ///PARAMETROS : 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10-Octubre-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Obtener_Valores_Parametros()
    {
        Año_Orden_Variacion = Convert.ToInt16(Request.QueryString["Anio_Orden_Variacion"]);// Convert.ToInt16(Session["VEOV_AÑO_ORDEN_VARIACION"]);
        Fecha_Orden_Variacion = Request.QueryString["Fecha_Orden_Variacion"];
        No_Orden_Variacion = Request.QueryString["No_Orden_Variacion"];// Session["VEOV_NO_ORDEN_VARIACION"].ToString();
        Orden_Variacion.InnerText = "Orden Variación " + Request.QueryString["No_Orden_Variacion"];// "Orden Variación "+Session["VEOV_NO_ORDEN_VARIACION"].ToString();
        No_Contrarecibo = Request.QueryString["No_Contrarecibo"];// Session["VEOV_NO_CONTRARECIBO"].ToString();
        Cuenta_Predial = Request.QueryString["Cuenta_Predial"];// Session["VEOV_CUENTA_PREDIAL"].ToString();
        Cuenta_Predial_ID = Request.QueryString["Cuenta_Predial_ID"];// Session["VEOV_CUENTA_PREDIAL_ID"].ToString();
    }
    #endregion

    #region Metodos/Generales [Limpiar Todo,Mensaje_Error,Cargar_Combos,Llenar_Combo_ID,Estado_Botones,Iniciliza_Controles ]

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION :Limpiar_Todo
    ///DESCRIPCION          : Limpia los controles del formulario
    ///PARAMETROS : 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 05-Agsoto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Limpiar_Todo()
    {
        Hdn_No_Orden_Variacion.Value = null;
        Hdn_Contrarecibo.Value = null;
        Hdn_Cuenta_ID.Value = null;
        Hdn_Cuota_Minima.Value = null;
        Hdn_Propietario_ID.Value = null;
        Hdn_Tasa_ID.Value = null;
        Hdn_Contrarecibo.Value = null;

        //QUITA LOS TEXTOS
        Txt_Tipo_Movimiento.Text = "";
        //Generales
        Txt_Cuenta_Predial.Text = "";
        Txt_Cuenta_Origen.Text = "";
        Txt_Tipo_Predio.Text = "";
        Txt_Uso_Predio.Text = "";
        Txt_Estado_Predio.Text = "";
        Txt_Estatus.Text = "";
        Txt_Superficie_Construida.Text = "";
        Txt_Superficie_Total.Text = "";
        Txt_Colonia_Cuenta.Text = "";
        Txt_Calle_Cuenta.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Catastral.Text = "";
        Txt_Efectos.Text = "";
        Txt_Costo_M2.Text = "";
        Txt_Ultimo_Movimiento.Text = "";

        //Propietario
        Txt_Nombre_Propietario.Text = "";
        Txt_RFC_Propietario.Text = "";
        Txt_Tipo_Propietario.Text = "";
        Chk_Mismo_Domicilio.Checked = true;
        Txt_Domicilio_Foraneo.Text = "";
        Txt_Colonia_Propietario.Text = "";
        Txt_Calle_Propietario.Text = "";
        Txt_Numero_Exterior_Propietario.Text = "";
        Txt_Numero_Interior_Propietario.Text = "";
        Txt_Estado_Propietario.Text = "";
        Txt_Ciudad_Propietario.Text = "";
        Txt_CP.Text = "";

        //Impuestos
        Txt_Valor_Fiscal.Text = "";
        Txt_Tasa_Descripcion.Text = "";
        Txt_Tasa_Porcentaje.Text = "";
        Txt_Periodo_Corriente.Text = "";
        Txt_Cuota_Anual.Text = "";
        Txt_Cuota_Bimestral.Text = "";
        Txt_Diferencia_Construccion.Text = "";
        Txt_Porcentaje_Exencion.Text = "";
        Txt_Termino_Exencion.Text = "";
        Txt_Fecha_Avaluo.Text = "";
        Chk_Cuota_Fija.Checked = false;
        Txt_Cuota_Fija.Text = "";
        Txt_Solicitante.Text = "";
        Txt_Inmueble.Text = "";
        Txt_Plazo_Financiamiento.Text = "";
        Txt_Cuota_Minima_Anual.Text = "";
        Txt_Cuota_Minima_Aplicar.Text = "";
        Txt_Excedente_Construccion.Text = "";
        Txt_Tasa_Excedente_Construccion.Text = "";
        Txt_Excedente_Construccion_Total.Text = "";
        Txt_Excedente_Valor.Text = "";
        Txt_Tasa_Excedente_Valor.Text = "";
        Txt_Excedente_Valor_Total.Text = "";
        Txt_Total_Cuota_Fija.Text = "";
        Txt_Fundamento_Legal.Text = "";

        //Total perdiodo Corriente
        Txt_Desde_Periodo_Corriente.Text = "";
        Txt_Hasta_Periodo_Corriente.Text = "";
        Txt_Alta_Periodo_Corriente.Text = "";
        Txt_Baja_Periodo_Corriente.Text = "";

        //Total perdiodo Rezago
        Txt_Desde_Periodo_Rezago.Text = "";
        Txt_Hasta_Periodo_Rezago.Text = "";
        Txt_Alta_Periodo_Rezago.Text = "";
        Txt_Baja_Periodo_Rezago.Text = "";

        //Observaciones de la Cuenta
        Txt_Observaciones_Cuenta.Text = "";

        //Observaciones de Validación
        Cmb_Estatus_Orden_Variacion.SelectedIndex = 0;
        Txt_Observaciones_Validacion.Text = "";

        //QUITA LOS TEXTOS TOOLTIP
        Txt_Tipo_Movimiento.ToolTip = "";
        //Generales
        Txt_Cuenta_Predial.ToolTip = "";
        Txt_Cuenta_Origen.ToolTip = "";
        Txt_Tipo_Predio.ToolTip = "";
        Txt_Uso_Predio.ToolTip = "";
        Txt_Estado_Predio.ToolTip = "";
        Txt_Estatus.ToolTip = "";
        Txt_Superficie_Construida.ToolTip = "";
        Txt_Superficie_Total.ToolTip = "";
        Txt_Colonia_Cuenta.ToolTip = "";
        Txt_Calle_Cuenta.ToolTip = "";
        Txt_No_Exterior.ToolTip = "";
        Txt_No_Interior.ToolTip = "";
        Txt_Catastral.ToolTip = "";
        Txt_Efectos.ToolTip = "";
        Txt_Costo_M2.ToolTip = "";
        Txt_Ultimo_Movimiento.ToolTip = "";

        //Propietario
        Txt_Nombre_Propietario.ToolTip = "";
        Txt_RFC_Propietario.ToolTip = "";
        Txt_Tipo_Propietario.ToolTip = "";
        Chk_Mismo_Domicilio.Checked = true;
        Txt_Domicilio_Foraneo.ToolTip = "";
        Txt_Colonia_Propietario.ToolTip = "";
        Txt_Calle_Propietario.ToolTip = "";
        Txt_Numero_Exterior_Propietario.ToolTip = "";
        Txt_Numero_Interior_Propietario.ToolTip = "";
        Txt_Estado_Propietario.ToolTip = "";
        Txt_Ciudad_Propietario.ToolTip = "";
        Txt_CP.ToolTip = "";

        //Impuestos
        Txt_Valor_Fiscal.ToolTip = "";
        Txt_Tasa_Descripcion.ToolTip = "";
        Txt_Tasa_Porcentaje.ToolTip = "";
        Txt_Periodo_Corriente.ToolTip = "";
        Txt_Cuota_Anual.ToolTip = "";
        Txt_Cuota_Bimestral.ToolTip = "";
        Txt_Diferencia_Construccion.ToolTip = "";
        Txt_Porcentaje_Exencion.ToolTip = "";
        Txt_Termino_Exencion.ToolTip = "";
        Txt_Fecha_Avaluo.ToolTip = "";
        Chk_Cuota_Fija.Checked = false;
        Txt_Cuota_Fija.ToolTip = "";
        Txt_Solicitante.ToolTip = "";
        Txt_Inmueble.ToolTip = "";
        Txt_Plazo_Financiamiento.ToolTip = "";
        Txt_Cuota_Minima_Anual.ToolTip = "";
        Txt_Excedente_Construccion.ToolTip = "";
        Txt_Tasa_Excedente_Construccion.ToolTip = "";
        Txt_Excedente_Construccion_Total.ToolTip = "";
        Txt_Excedente_Valor.ToolTip = "";
        Txt_Tasa_Excedente_Valor.ToolTip = "";
        Txt_Excedente_Valor_Total.ToolTip = "";
        Txt_Total_Cuota_Fija.ToolTip = "";
        Txt_Fundamento_Legal.ToolTip = "";

        //Total perdiodo Corriente
        Txt_Desde_Periodo_Corriente.ToolTip = "";
        Txt_Hasta_Periodo_Corriente.ToolTip = "";
        Txt_Alta_Periodo_Corriente.ToolTip = "";
        Txt_Baja_Periodo_Corriente.ToolTip = "";

        //Total perdiodo Rezago
        Txt_Desde_Periodo_Rezago.ToolTip = "";
        Txt_Hasta_Periodo_Rezago.ToolTip = "";
        Txt_Alta_Periodo_Rezago.ToolTip = "";
        Txt_Baja_Periodo_Rezago.ToolTip = "";

        //Observaciones de la Cuenta
        Txt_Observaciones_Cuenta.ToolTip = "";

        //Observaciones de Validación
        Cmb_Estatus_Orden_Variacion.SelectedIndex = 0;
        Txt_Observaciones_Validacion.ToolTip = "";

        //QUITA LOS IDs
        Hdn_Cuenta_ID.Value = "";
        Hdn_Tipo_Predio_ID.Value = "";
        Hdn_Uso_Predio_ID.Value = "";
        Hdn_Estado_Predio_ID.Value = "";
        Hdn_Colonia_ID.Value = "";
        Hdn_Estado_Cuenta.Value = "";
        Hdn_Calle_ID.Value = "";
        Hdn_Ciudad_Cuenta.Value = "";
        Hdn_Propietario_ID.Value = "";
        Hdn_Colonia_ID_Notificacion.Value = "";
        Hdn_Colonia_Notificacion_Anterior.Value = "";
        Hdn_Colonia_Notificacion_Nuevo.Value = "";
        Hdn_Calle_ID_Notificacion.Value = "";
        Hdn_Calle_Notificacion_Anterior.Value = "";
        Hdn_Calle_Notificacion_Nuevo.Value = "";
        Hdn_No_Exterior_Propietario_Anterior.Value = "";
        Hdn_No_Exterior_Propietario_Nuevo.Value = "";
        Hdn_No_Interior_Propietario_Anterior.Value = "";
        Hdn_No_Interior_Propietario_Nuevo.Value = "";
        Hdn_Estado_ID_Notificacion.Value = "";
        Hdn_Estado_Notificacion_Anterior.Value = "";
        Hdn_Estado_Notificacion_Nuevo.Value = "";
        Hdn_Ciudad_ID_Notificacion.Value = "";
        Hdn_Ciudad_Notificacion_Anterior.Value = "";
        Hdn_Ciudad_Notificacion_Nuevo.Value = "";
        Hdn_Tasa_Predial_ID.Value = "";
        Hdn_Tasa_ID.Value = "";
        Hdn_No_Cuota_Fija_Nuevo.Value = "";
        Hdn_No_Cuota_Fija_Anterior.Value = "";
        Hdn_Cuota_Minima_ID.Value = "";
        Hdn_Movimiento_ID.Value = "";
        Hdn_Grupo_Movimiento_ID.Value = "";
        Hdn_No_Orden_Variacion.Value = "";
        Hdn_Cuota_Minima.Value = "";
        Hdn_Contrarecibo.Value = "";
        Hdn_Excedente_Valor.Value = "";

        //QUITA LAS NEGRITAS EN TEXTBOX
        Txt_Tipo_Movimiento.Font.Bold = false;
        //Generales
        Txt_Cuenta_Predial.Font.Bold = false;
        Txt_Cuenta_Origen.Font.Bold = false;
        Txt_Tipo_Predio.Font.Bold = false;
        Txt_Uso_Predio.Font.Bold = false;
        Txt_Estado_Predio.Font.Bold = false;
        Txt_Estatus.Font.Bold = false;
        Txt_Superficie_Construida.Font.Bold = false;
        Txt_Superficie_Total.Font.Bold = false;
        Txt_Colonia_Cuenta.Font.Bold = false;
        Txt_Calle_Cuenta.Font.Bold = false;
        Txt_No_Exterior.Font.Bold = false;
        Txt_No_Interior.Font.Bold = false;
        Txt_Catastral.Font.Bold = false;
        Txt_Efectos.Font.Bold = false;
        Txt_Costo_M2.Font.Bold = false;
        Txt_Ultimo_Movimiento.Font.Bold = false;

        //Propietario
        Txt_Nombre_Propietario.Font.Bold = false;
        Txt_RFC_Propietario.Font.Bold = false;
        Txt_Tipo_Propietario.Font.Bold = false;
        Chk_Mismo_Domicilio.Checked = true;
        Txt_Domicilio_Foraneo.Font.Bold = false;
        Txt_Colonia_Propietario.Font.Bold = false;
        Txt_Calle_Propietario.Font.Bold = false;
        Txt_Numero_Exterior_Propietario.Font.Bold = false;
        Txt_Numero_Interior_Propietario.Font.Bold = false;
        Txt_Estado_Propietario.Font.Bold = false;
        Txt_Ciudad_Propietario.Font.Bold = false;
        Txt_CP.Font.Bold = false;

        //Impuestos
        Txt_Valor_Fiscal.Font.Bold = false;
        Txt_Tasa_Descripcion.Font.Bold = false;
        Txt_Tasa_Porcentaje.Font.Bold = false;
        Txt_Periodo_Corriente.Font.Bold = false;
        Txt_Cuota_Anual.Font.Bold = false;
        Txt_Cuota_Bimestral.Font.Bold = false;
        Txt_Diferencia_Construccion.Font.Bold = false;
        Txt_Porcentaje_Exencion.Font.Bold = false;
        Txt_Termino_Exencion.Font.Bold = false;
        Txt_Fecha_Avaluo.Font.Bold = false;
        Chk_Cuota_Fija.Checked = false;
        Txt_Cuota_Fija.Font.Bold = false;
        Txt_Solicitante.Font.Bold = false;
        Txt_Inmueble.Font.Bold = false;
        Txt_Plazo_Financiamiento.Font.Bold = false;
        Txt_Cuota_Minima_Anual.Font.Bold = false;
        Txt_Excedente_Construccion.Font.Bold = false;
        Txt_Tasa_Excedente_Construccion.Font.Bold = false;
        Txt_Excedente_Construccion_Total.Font.Bold = false;
        Txt_Excedente_Valor.Font.Bold = false;
        Txt_Tasa_Excedente_Valor.Font.Bold = false;
        Txt_Excedente_Valor_Total.Font.Bold = false;
        Txt_Total_Cuota_Fija.Font.Bold = false;
        Txt_Fundamento_Legal.Font.Bold = false;

        //Total perdiodo Corriente
        Txt_Desde_Periodo_Corriente.Font.Bold = false;
        Txt_Hasta_Periodo_Corriente.Font.Bold = false;
        Txt_Alta_Periodo_Corriente.Font.Bold = false;
        Txt_Baja_Periodo_Corriente.Font.Bold = false;

        //Total perdiodo Rezago
        Txt_Desde_Periodo_Rezago.Font.Bold = false;
        Txt_Hasta_Periodo_Rezago.Font.Bold = false;
        Txt_Alta_Periodo_Rezago.Font.Bold = false;
        Txt_Baja_Periodo_Rezago.Font.Bold = false;

        //Observaciones de la Cuenta
        Txt_Observaciones_Cuenta.Font.Bold = false;

        //Observaciones de Validación
        Cmb_Estatus_Orden_Variacion.Font.Bold = false;
        Txt_Observaciones_Validacion.Font.Bold = false;

        //QUITA LAS NEGRITAS EN LABELS
        Lbl_Tipo_Movimiento.Font.Bold = false;
        //Generales
        Lbl_Cuenta_Predial.Font.Bold = false;
        Lbl_Cuenta_Origen.Font.Bold = false;
        Lbl_Tipo_Predio.Font.Bold = false;
        Lbl_Uso_Predio.Font.Bold = false;
        Lbl_Estado_Predio.Font.Bold = false;
        Lbl_Estatus.Font.Bold = false;
        Lbl_Superficie_Construida.Font.Bold = false;
        Lbl_Superficie_Total.Font.Bold = false;
        Lbl_Colonia_Cuenta.Font.Bold = false;
        Lbl_Calle_Cuenta.Font.Bold = false;
        Lbl_No_Exterior.Font.Bold = false;
        Lbl_No_Interior.Font.Bold = false;
        Lbl_Catastral.Font.Bold = false;
        Lbl_Efectos.Font.Bold = false;
        Lbl_Costo_M2.Font.Bold = false;
        Lbl_Ultimo_Movimiento.Font.Bold = false;

        //Propietario
        Lbl_Nombre_Propietario.Font.Bold = false;
        Lbl_RFC_Propietario.Font.Bold = false;
        Lbl_Tipo_Propietario.Font.Bold = false;
        Chk_Mismo_Domicilio.Checked = true;
        Lbl_Domicilio_Foraneo.Font.Bold = false;
        Lbl_Colonia_Propietario.Font.Bold = false;
        Lbl_Calle_Propietario.Font.Bold = false;
        Lbl_Numero_Exterior_Propietario.Font.Bold = false;
        Lbl_Numero_Interior_Propietario.Font.Bold = false;
        Lbl_Estado_Propietario.Font.Bold = false;
        Lbl_Ciudad_Propietario.Font.Bold = false;
        Lbl_CP.Font.Bold = false;

        //Impuestos
        Lbl_Valor_Fiscal.Font.Bold = false;
        Lbl_Tasa_Descripcion.Font.Bold = false;
        Lbl_Periodo_Corriente.Font.Bold = false;
        Lbl_Cuota_Anual.Font.Bold = false;
        Lbl_Cuota_Bimestral.Font.Bold = false;
        Lbl_Diferencia_Construccion.Font.Bold = false;
        Lbl_Porcentaje_Exencion.Font.Bold = false;
        Lbl_Termino_Exencion.Font.Bold = false;
        Lbl_Fecha_Avaluo.Font.Bold = false;
        Lbl_Cuota_Fija.Font.Bold = false;
        Lbl_Solicitante.Font.Bold = false;
        Lbl_Inmueble.Font.Bold = false;
        Lbl_Plazo_Financiado.Font.Bold = false;
        Lbl_Cuota_Minima_Anual.Font.Bold = false;
        Lbl_Exedente_Construccion.Font.Bold = false;
        Lbl_Excedente_Valor.Font.Bold = false;
        Lbl_Total_Cuota_Fija.Font.Bold = false;
        Lbl_Fundamento_Legal.Font.Bold = false;

        //Total perdiodo Corriente
        Lbl_Desde_Periodo_Corriente.Font.Bold = false;
        Lbl_Hasta_Periodo_Corriente.Font.Bold = false;
        Lbl_Alta_Periodo_Corriente.Font.Bold = false;
        Lbl_Baja_Periodo_Corriente.Font.Bold = false;

        //Total perdiodo Rezago
        Lbl_Desde_Periodo_Rezago.Font.Bold = false;
        Lbl_Hasta_Periodo_Rezago.Font.Bold = false;
        Lbl_Alta_Periodo_Rezago.Font.Bold = false;
        Lbl_Baja_Periodo_Rezago.Font.Bold = false;

        //Observaciones de la Cuenta
        Lbl_Observaciones_Cuenta.Font.Bold = false;

        Grid_Copropietarios.DataBind();
        Grid_Diferencias.DataBind();
        Grid_Historial_Observaciones.DataBind();
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }

    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Encabezado_Error.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Configurar_Estatus_Controles
    ///DESCRIPCIÓN          : Configura los controles para manipular
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Configurar_Estatus_Controles(Boolean Estatus_Activo)
    {
        Txt_Tipo_Movimiento.Enabled = Estatus_Activo;
        //Generales
        Txt_Cuenta_Predial.Enabled = Estatus_Activo;
        Txt_Cuenta_Origen.Enabled = Estatus_Activo;
        Txt_Tipo_Predio.Enabled = Estatus_Activo;
        Txt_Uso_Predio.Enabled = Estatus_Activo;
        Txt_Estado_Predio.Enabled = Estatus_Activo;
        Txt_Estatus.Enabled = Estatus_Activo;
        Txt_Superficie_Construida.Enabled = Estatus_Activo;
        Txt_Superficie_Total.Enabled = Estatus_Activo;
        Txt_Colonia_Cuenta.Enabled = Estatus_Activo;
        Txt_Calle_Cuenta.Enabled = Estatus_Activo;
        Txt_No_Exterior.Enabled = Estatus_Activo;
        Txt_No_Interior.Enabled = Estatus_Activo;
        Txt_Catastral.Enabled = Estatus_Activo;
        Txt_Efectos.Enabled = Estatus_Activo;
        Txt_Costo_M2.Enabled = Estatus_Activo;
        Txt_Ultimo_Movimiento.Enabled = Estatus_Activo;

        //Propietario
        Txt_Nombre_Propietario.Enabled = Estatus_Activo;
        Txt_RFC_Propietario.Enabled = Estatus_Activo;
        Txt_Tipo_Propietario.Enabled = Estatus_Activo;
        Chk_Mismo_Domicilio.Enabled = Estatus_Activo;
        Txt_Domicilio_Foraneo.Enabled = Estatus_Activo;
        Txt_Colonia_Propietario.Enabled = Estatus_Activo;
        Txt_Calle_Propietario.Enabled = Estatus_Activo;
        Txt_Numero_Exterior_Propietario.Enabled = Estatus_Activo;
        Txt_Numero_Interior_Propietario.Enabled = Estatus_Activo;
        Txt_Estado_Propietario.Enabled = Estatus_Activo;
        Txt_Ciudad_Propietario.Enabled = Estatus_Activo;
        Txt_CP.Enabled = Estatus_Activo;

        //Impuestos
        Txt_Valor_Fiscal.Enabled = Estatus_Activo;
        Txt_Tasa_Descripcion.Enabled = Estatus_Activo;
        Txt_Tasa_Porcentaje.Enabled = Estatus_Activo;
        Txt_Periodo_Corriente.Enabled = Estatus_Activo;
        Txt_Cuota_Anual.Enabled = Estatus_Activo;
        Txt_Cuota_Bimestral.Enabled = Estatus_Activo;
        Txt_Diferencia_Construccion.Enabled = Estatus_Activo;
        Txt_Porcentaje_Exencion.Enabled = Estatus_Activo;
        Txt_Termino_Exencion.Enabled = Estatus_Activo;
        Txt_Fecha_Avaluo.Enabled = Estatus_Activo;
        Chk_Cuota_Fija.Enabled = Estatus_Activo;
        Txt_Cuota_Fija.Enabled = Estatus_Activo;
        Txt_Solicitante.Enabled = Estatus_Activo;
        Txt_Inmueble.Enabled = Estatus_Activo;
        Txt_Plazo_Financiamiento.Enabled = Estatus_Activo;
        Txt_Cuota_Minima_Anual.Enabled = Estatus_Activo;
        Txt_Excedente_Construccion.Enabled = Estatus_Activo;
        Txt_Tasa_Excedente_Construccion.Enabled = Estatus_Activo;
        Txt_Excedente_Construccion_Total.Enabled = Estatus_Activo;
        Txt_Excedente_Valor.Enabled = Estatus_Activo;
        Txt_Tasa_Excedente_Valor.Enabled = Estatus_Activo;
        Txt_Excedente_Valor_Total.Enabled = Estatus_Activo;
        Txt_Total_Cuota_Fija.Enabled = Estatus_Activo;
        Txt_Fundamento_Legal.Enabled = Estatus_Activo;

        //Total perdiodo Corriente
        Txt_Desde_Periodo_Corriente.Enabled = Estatus_Activo;
        Txt_Hasta_Periodo_Corriente.Enabled = Estatus_Activo;
        Txt_Alta_Periodo_Corriente.Enabled = Estatus_Activo;
        Txt_Baja_Periodo_Corriente.Enabled = Estatus_Activo;

        //Total perdiodo Rezago
        Txt_Desde_Periodo_Rezago.Enabled = Estatus_Activo;
        Txt_Hasta_Periodo_Rezago.Enabled = Estatus_Activo;
        Txt_Alta_Periodo_Rezago.Enabled = Estatus_Activo;
        Txt_Baja_Periodo_Rezago.Enabled = Estatus_Activo;

        //Observaciones de la Cuenta
        Txt_Observaciones_Cuenta.Enabled = Estatus_Activo;

        //Observaciones de Validación
        Cmb_Estatus_Orden_Variacion.Enabled = Estatus_Activo;
        Txt_Observaciones_Validacion.Enabled = Estatus_Activo;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Configurar_Estatus_Controles
    ///DESCRIPCIÓN          : Configura los controles para manipular
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Configurar_Edicion_Controles(Boolean Estatus_Edicion)
    {
        Txt_Tipo_Movimiento.ReadOnly = !Estatus_Edicion;
        //Generales
        Txt_Cuenta_Predial.ReadOnly = !Estatus_Edicion;
        Txt_Cuenta_Origen.ReadOnly = !Estatus_Edicion;
        Txt_Tipo_Predio.ReadOnly = !Estatus_Edicion;
        Txt_Uso_Predio.ReadOnly = !Estatus_Edicion;
        Txt_Estado_Predio.ReadOnly = !Estatus_Edicion;
        Txt_Estatus.ReadOnly = !Estatus_Edicion;
        Txt_Superficie_Construida.ReadOnly = !Estatus_Edicion;
        Txt_Superficie_Total.ReadOnly = !Estatus_Edicion;
        Txt_Colonia_Cuenta.ReadOnly = !Estatus_Edicion;
        Txt_Calle_Cuenta.ReadOnly = !Estatus_Edicion;
        Txt_No_Exterior.ReadOnly = !Estatus_Edicion;
        Txt_No_Interior.ReadOnly = !Estatus_Edicion;
        Txt_Catastral.ReadOnly = !Estatus_Edicion;
        Txt_Efectos.ReadOnly = !Estatus_Edicion;
        Txt_Costo_M2.ReadOnly = !Estatus_Edicion;
        Txt_Ultimo_Movimiento.ReadOnly = !Estatus_Edicion;

        //Propietario
        Txt_Nombre_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_RFC_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Tipo_Propietario.ReadOnly = !Estatus_Edicion;
        Chk_Mismo_Domicilio.Enabled = Estatus_Edicion;
        Txt_Domicilio_Foraneo.ReadOnly = !Estatus_Edicion;
        Txt_Colonia_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Calle_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Numero_Exterior_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Numero_Interior_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Estado_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Ciudad_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_CP.ReadOnly = !Estatus_Edicion;

        //Impuestos
        Txt_Valor_Fiscal.ReadOnly = !Estatus_Edicion;
        Txt_Tasa_Descripcion.ReadOnly = !Estatus_Edicion;
        Txt_Tasa_Porcentaje.ReadOnly = !Estatus_Edicion;
        Txt_Periodo_Corriente.ReadOnly = !Estatus_Edicion;
        Txt_Cuota_Anual.ReadOnly = !Estatus_Edicion;
        Txt_Cuota_Bimestral.ReadOnly = !Estatus_Edicion;
        Txt_Diferencia_Construccion.ReadOnly = !Estatus_Edicion;
        Txt_Porcentaje_Exencion.ReadOnly = !Estatus_Edicion;
        Txt_Termino_Exencion.ReadOnly = !Estatus_Edicion;
        Txt_Fecha_Avaluo.ReadOnly = !Estatus_Edicion;
        Chk_Cuota_Fija.Enabled = Estatus_Edicion;
        Txt_Cuota_Fija.ReadOnly = !Estatus_Edicion;
        Txt_Solicitante.ReadOnly = !Estatus_Edicion;
        Txt_Inmueble.ReadOnly = !Estatus_Edicion;
        Txt_Plazo_Financiamiento.ReadOnly = !Estatus_Edicion;
        Txt_Cuota_Minima_Anual.ReadOnly = !Estatus_Edicion;
        Txt_Excedente_Construccion.ReadOnly = !Estatus_Edicion;
        Txt_Tasa_Excedente_Construccion.ReadOnly = !Estatus_Edicion;
        Txt_Excedente_Construccion_Total.ReadOnly = !Estatus_Edicion;
        Txt_Excedente_Valor.ReadOnly = !Estatus_Edicion;
        Txt_Tasa_Excedente_Valor.ReadOnly = !Estatus_Edicion;
        Txt_Excedente_Valor_Total.ReadOnly = !Estatus_Edicion;
        Txt_Total_Cuota_Fija.ReadOnly = !Estatus_Edicion;
        Txt_Fundamento_Legal.ReadOnly = !Estatus_Edicion;

        //Total perdiodo Corriente
        Txt_Desde_Periodo_Corriente.ReadOnly = !Estatus_Edicion;
        Txt_Hasta_Periodo_Corriente.ReadOnly = !Estatus_Edicion;
        Txt_Alta_Periodo_Corriente.ReadOnly = !Estatus_Edicion;
        Txt_Baja_Periodo_Corriente.ReadOnly = !Estatus_Edicion;

        //Total perdiodo Rezago
        Txt_Desde_Periodo_Rezago.ReadOnly = !Estatus_Edicion;
        Txt_Hasta_Periodo_Rezago.ReadOnly = !Estatus_Edicion;
        Txt_Alta_Periodo_Rezago.ReadOnly = !Estatus_Edicion;
        Txt_Baja_Periodo_Rezago.ReadOnly = !Estatus_Edicion;

        //Observaciones de la Cuenta
        Txt_Observaciones_Cuenta.ReadOnly = !Estatus_Edicion;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Configurar_Controles_Validacion
    ///DESCRIPCIÓN          : Configura los controles para manipular
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Configurar_Controles_Validacion(Boolean Estatus_Activo)
    {
        Cmb_Estatus_Orden_Variacion.Enabled = Estatus_Activo;
        Txt_Observaciones_Validacion.Enabled = Estatus_Activo;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Inicializa_Controles
    ///DESCRIPCIÓN: inicializa los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: tres/agosto/2011 06:28:07 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Inicializa_Controles()
    {
        try
        {
            Configurar_Estatus_Controles(false);
            Dt_Agregar_Copropietarios.Clear();
            Dt_Agregar_Diferencias.Clear();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Metodos ABC [Consulta_Combos,Consulta_Valor_Excedente]

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Excedente_Valor
    ///DESCRIPCIÓN: consulta los datos del excedente de valor
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Agosto/2011 11:50:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************       
    private void Consulta_Excedente_Valor()
    {
        Double Excedente_Valor = 0;
        try
        {
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Adeudo_Predial_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            Adeudo_Predial_Negocio.p_Salario_Minimo = Adeudo_Predial_Negocio.Obtener_Salario_Minimo(DateTime.Now.Year);
            Adeudo_Predial_Negocio.Obtener_Tope_Salarios_Minimos();
            if (Txt_Valor_Fiscal.Text.Trim() != "")
            {
                Excedente_Valor = (Convert.ToDouble(Txt_Valor_Fiscal.Text.Trim()) - Convert.ToDouble(Adeudo_Predial_Negocio.p_Tope_Salarios_Minimos.ToString()));
            }
            if (Excedente_Valor < 0)
            {
                Hdn_Excedente_Valor.Value = "0";
            }
            else
            {
                Hdn_Excedente_Valor.Value = Excedente_Valor.ToString();
            }
            if (Hdn_Excedente_Valor.Value.Trim() != "")
            {
                Txt_Excedente_Valor.Text = Convert.ToDouble(Hdn_Excedente_Valor.Value).ToString("##,###,##0.00");
            }
            else
            {
                Txt_Excedente_Valor.Text = "0.00";
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Excedente de Valor[" + Ex.Message + "]");
        }
    }

    #endregion

    #region Metodos Cargar Datos [Cargar_datos,Cargar_generales,Cargar_Popietarios,Cargar_Datos_Cuota_Fija]

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Datos_Cuenta
    ///DESCRIPCIÓN          : asignar datos de cuenta a los controles
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Cargar_Datos_Cuenta()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        DataTable Dt_Cuenta_Predial;
        String Periodo_Corriente = "";
        Int16 Año_Periodo_Corriente = 0;
        Boolean Datos_Cargados = false;
        String Clave_Calle = "";
        String Clave_Colonia = "";
        try
        {
            Cuenta_Predial.P_Incluir_Campos_Foraneos = true;
            Cuenta_Predial.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Dt_Cuenta_Predial = Cuenta_Predial.Consultar_Cuenta();

            if (Dt_Cuenta_Predial.Rows.Count > 0)
            {
                Hdn_Cuenta_ID.Value = Dt_Cuenta_Predial.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                //SESION PARA MOSTRAR LA CONSULTA DE RESUMEN DE CUENTA
                Session["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;
                Session["Orden_Variacion_ID_Adeudos"] = Hdn_No_Orden_Variacion.Value;
                Session["Cuenta_Predial_ID_Adeudos"] = Hdn_Cuenta_ID.Value;
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text;

                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString() != "")
                {
                    Hdn_Colonia_ID.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString().Trim();
                    Clave_Colonia = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Clave, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Hdn_Colonia_ID.Value + "'");
                    Txt_Colonia_Cuenta.Text = Clave_Colonia + " " + HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0]["NOMBRE_COLONIA"].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != "")
                {
                    Hdn_Calle_ID.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString().Trim();
                    Clave_Calle = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Clave, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Hdn_Calle_ID.Value + "'");
                    Txt_Calle_Cuenta.Text = Clave_Calle + " " + HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0]["NOMBRE_CALLE"].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != "")
                {
                    Txt_Estado_Predio.Text = Dt_Cuenta_Predial.Rows[0]["DESCRIPCION_ESTADO_PREDIO"].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != "")
                {
                    Txt_Tipo_Predio.Text = Dt_Cuenta_Predial.Rows[0]["DESCRIPCION_TIPO_PREDIO"].ToString().Trim().ToUpper();
                    Hdn_Tipo_Predio_ID.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != "")
                {
                    Txt_Uso_Predio.Text = Dt_Cuenta_Predial.Rows[0]["DESCRIPCION_USO_SUELO"].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "")
                {
                    Hdn_Cuota_Minima_ID.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString().Trim();
                }
                Hdn_Cuota_Minima.Value = "";
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString() != "")
                {
                    Txt_Cuenta_Origen.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != "")
                {
                    Txt_Estatus.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString() != "")
                {
                    Txt_No_Exterior.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString() != "")
                {
                    Txt_No_Interior.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString() != "")
                {
                    Txt_Superficie_Construida.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida]).ToString("##,###,##0.00");
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString() != "")
                {
                    Txt_Superficie_Total.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total]).ToString("##,###,##0.00");
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString() != "")
                {
                    Txt_Catastral.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString() != "")
                {
                    Txt_Valor_Fiscal.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal]).ToString("##,###,##0.00");
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != "")
                {
                    Txt_Efectos.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString().Trim();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString() != "")
                {
                    Txt_Periodo_Corriente.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString().Trim();
                    Periodo_Corriente = HttpUtility.HtmlDecode(Txt_Periodo_Corriente.Text).Trim();
                    if (Periodo_Corriente != "")
                    {
                        if (Periodo_Corriente.Length >= 4)
                        {
                            Int16.TryParse(Periodo_Corriente.Substring(Periodo_Corriente.Length - 4), out Año_Periodo_Corriente);
                            Hdn_Cuota_Minima.Value = Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo_Corriente.ToString()).ToString().Trim();
                        }
                    }
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "")
                {
                    Txt_Cuota_Anual.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual]).ToString("##,###,##0.00");
                    Txt_Cuota_Bimestral.Text = (Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6).ToString("##,###,##0.00");
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "")
                {
                    if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString().ToUpper() == "NO")
                    {
                        Chk_Cuota_Fija.Checked = false;
                    }
                    if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString().ToUpper() == "SI")
                    {
                        Chk_Cuota_Fija.Checked = true;

                        //----K4RG4R D47OZ D3 14 CU0T4 F1J4!!!!!
                        if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija] != null && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
                        {
                            Cargar_Datos_Cuota_Fija(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
                        }
                    }
                    Chk_Cuota_Fija_CheckedChanged(null, null);
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "")
                {
                    if (Convert.ToDateTime(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString()) != DateTime.MinValue)
                    {
                        Txt_Termino_Exencion.Text = Convert.ToDateTime(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString()).ToString("dd/MMM/yyyy");
                    }
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "")
                {
                    if (Convert.ToDateTime(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString()) != DateTime.MinValue)
                    {
                        Txt_Fecha_Avaluo.Text = Convert.ToDateTime(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString()).ToString("dd/MMM/yyyy");
                    }
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString() != "")
                {
                    Txt_Diferencia_Construccion.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion]).ToString("##,###,##0.00");
                    Txt_Excedente_Construccion.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion]).ToString("##,###,##0.00");
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString() != "")
                {
                    Txt_Costo_M2.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2]).ToString("##,###,##0.00");
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "")
                {
                    Txt_Porcentaje_Exencion.Text = Convert.ToDouble(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion]).ToString("##,###,##0.00");
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "")
                {
                    Txt_Cuota_Fija.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString().Trim();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
                {
                    Hdn_No_Cuota_Fija_Anterior.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString().Trim();
                    Hdn_No_Cuota_Fija_Nuevo.Value = "";
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString() != "")
                {
                    DataTable Dt_Tasa_Seleccionada;
                    DataRow Dr_Tasa_Seleccionada;
                    Hdn_Tasa_Predial_ID.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString().Trim();
                    Hdn_Tasa_ID.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString().Trim();

                    Cls_Ope_Pre_Tasas_Anuales_Negocio Tasas_Negocio = new Cls_Ope_Pre_Tasas_Anuales_Negocio();
                    Tasas_Negocio.P_Tasa_Predial_ID = Hdn_Tasa_ID.Value;
                    Tasas_Negocio.P_Tasa_ID = Hdn_Tasa_ID.Value;

                    Dt_Tasa_Seleccionada = Tasas_Negocio.Consultar_Tasas_Anuales();
                    if (Dt_Tasa_Seleccionada != null)
                    {
                        if (Dt_Tasa_Seleccionada.Rows.Count > 0)
                        {
                            Dr_Tasa_Seleccionada = Dt_Tasa_Seleccionada.Rows[0];
                            Txt_Tasa_Descripcion.Text = HttpUtility.HtmlDecode((Dr_Tasa_Seleccionada["IDENTIFICADOR"].ToString() + " - " + Dr_Tasa_Seleccionada["DESCRIPCION"].ToString()).Trim().ToUpper());
                            Txt_Tasa_Porcentaje.Text = Convert.ToDouble(Dr_Tasa_Seleccionada["TASA_ANUAL"]).ToString("##,###,##0.00");
                            Txt_Tasa_Excedente_Valor.Text = Convert.ToDouble(Dr_Tasa_Seleccionada["TASA_ANUAL"]).ToString("##,###,##0.00");
                            Txt_Tasa_Excedente_Construccion.Text = Convert.ToDouble(Dr_Tasa_Seleccionada["TASA_ANUAL"]).ToString("##,###,##0.00");
                            Calcular_Cuota();
                            Validar_Cuota_Minima();
                            Calcular_Excedentes();
                        }
                    }
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString() != "")
                {
                    Hdn_Colonia_ID_Notificacion.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString().Trim();
                    Clave_Colonia = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Clave, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Hdn_Colonia_ID_Notificacion.Value + "'");
                    Txt_Colonia_Propietario.Text = Clave_Colonia + " " + HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0]["NOMBRE_COLONIA_NOTIFICACION"].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString() != "")
                {
                    Hdn_Calle_ID_Notificacion.Value = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString().Trim();
                    Clave_Calle = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Clave, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Hdn_Calle_ID_Notificacion.Value + "'");
                    Txt_Calle_Propietario.Text = Clave_Calle + " " + HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0]["NOMBRE_CALLE_NOTIFICACION"].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0]["NOMBRE_ESTADO_CUENTA"] != null
                    && Dt_Cuenta_Predial.Rows[0]["NOMBRE_ESTADO_CUENTA"].ToString() != "")
                {
                    Hdn_Estado_Cuenta.Value = HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0]["NOMBRE_ESTADO_CUENTA"].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0]["NOMBRE_CIUDAD_CUENTA"] != null
                    && Dt_Cuenta_Predial.Rows[0]["NOMBRE_CIUDAD_CUENTA"].ToString() != "")
                {
                    Hdn_Ciudad_Cuenta.Value = HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0]["NOMBRE_CIUDAD_CUENTA"].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != "")
                {
                    Txt_Domicilio_Foraneo.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString() != "")
                {
                    Txt_Colonia_Propietario.Text = HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString() != "")
                {
                    Txt_Calle_Propietario.Text = HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString() != "")
                {
                    Txt_Numero_Exterior_Propietario.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString() != "")
                {
                    Txt_Numero_Interior_Propietario.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString().Trim().ToUpper();
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString() != "")
                {
                    Txt_CP.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString().Trim().ToUpper();
                }
                //if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Diferencia] != null)
                //{
                //    Txt_DI.Text = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Diferencia].ToString().ToUpper();
                //}
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString() != "")
                {
                    Txt_Estado_Propietario.Text = HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString().Trim().ToUpper());
                }
                if (Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion] != null
                    && Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString() != "")
                {
                    Txt_Ciudad_Propietario.Text = HttpUtility.HtmlDecode(Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString().Trim().ToUpper());
                }

                Cargar_Datos_Propietarios_Cuenta();
                Cargar_Grid_Copropietarios_Cuenta(0);

                Datos_Cargados = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
        return Datos_Cargados;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Variacion
    ///DESCRIPCIÓN          : Consulta los datos de la Orden de Variacción
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Cargar_Variacion_Orden()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        String Periodo_Corriente = "";
        Int16 Año_Periodo_Corriente = 0;
        DataTable Dt_Variacion_Cuenta;
        DataRow[] Arr_Dr_Variacion_Cuenta;
        Boolean Variacion_Cargada = false;
        String Clave_Calle = "";
        String Clave_Colonia = "";
        String Identificador_Movimiento_Nuevo = "";
        String Descripcion_Movimiento_Nuevo = "";
        //String Identificador_Movimiento_Anterior = "";
        //String Descripcion_Movimiento_Anterior = "";

        Orden_Variacion.P_Incluir_Campos_Foraneos = true;
        Orden_Variacion.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Orden_Variacion.P_Orden_Variacion_ID = No_Orden_Variacion;
        Orden_Variacion.P_Año = Año_Orden_Variacion;
        Orden_Variacion.P_Observaciones_No_Orden_Variacion = No_Orden_Variacion;
        Orden_Variacion.Consultar_Ordenes_Variacion();
        Grid_Historial_Observaciones.DataSource = Orden_Variacion.P_Dt_Observaciones;
        Grid_Historial_Observaciones.PageIndex = 0;
        Grid_Historial_Observaciones.DataBind();

        Dt_Variacion_Cuenta = Orden_Variacion.Obtener_Variacion_Cuenta();

        if (Dt_Variacion_Cuenta != null)
        {
            String Dato_Nuevo;
            String Dato_Anterior;
            String Calle_ID_Notificacion_Nuevo = null;
            String Calle_ID_Notificacion_Anterior = null;
            String Calle_Notificacion_Nuevo = null;
            String Calle_Notificacion_Anterior = null;
            String Colonia_ID_Notificacion_Nuevo = null;
            String Colonia_ID_Notificacion_Anterior = null;
            String Colonia_Notificacion_Nuevo = null;
            String Colonia_Notificacion_Anterior = null;
            String Ciudad_ID_Notificacion_Nuevo = null;
            String Ciudad_ID_Notificacion_Anterior = null;
            String Ciudad_Notificacion_Nuevo = null;
            String Ciudad_Notificacion_Anterior = null;
            String Estado_ID_Notificacion_Nuevo = null;
            String Estado_ID_Notificacion_Anterior = null;
            String Estado_Notificacion_Nuevo = null;
            String Estado_Notificacion_Anterior = null;

            //FILTRA LOS DATOS DIFERENTES DE LA VARIACION
            //Arr_Dr_Variacion_Cuenta = Dt_Variacion_Cuenta.Select("DIFERENTE = True AND NOT NOMBRE_CAMPO IN ('" + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + "', '" + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Anio + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "', '" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + "', 'FECHA_ORDEN')");
            Arr_Dr_Variacion_Cuenta = Dt_Variacion_Cuenta.Select("NOT NOMBRE_CAMPO IN ('" + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial + "', '" + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Anio + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + "', '" + Ope_Pre_Ordenes_Variacion.Campo_No_Nota + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Nota + "', '" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + "', 'FECHA_ORDEN')");
            if (Arr_Dr_Variacion_Cuenta.Length > 0)
            {
                //CARGA LOS DATOS DE LA VERIACIÓN EN LA PÁGINA Y LOS PONE EN NEGRITA PARA RESALTARLOS
                Dt_Variacion_Cuenta = Arr_Dr_Variacion_Cuenta.CopyToDataTable();
                foreach (DataRow Dr_Variacion_Cuenta in Dt_Variacion_Cuenta.Rows)
                {
                    Dato_Nuevo = "";
                    Dato_Anterior = "";

                    switch (Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString())
                    {
                        case "IDENTIFICADOR_MOVIMIENTO":
                            Identificador_Movimiento_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            //Identificador_Movimiento_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            break;
                        case "DESCRIPCION_MOVIMIENTO":
                            Descripcion_Movimiento_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            //Descripcion_Movimiento_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            break;
                        case Cat_Pre_Movimientos.Campo_Cargar_Modulos:
                            Hdn_Cargar_Modulos.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID:
                            Hdn_Movimiento_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Hdn_Grupo_Movimiento_ID.Value = Obtener_Dato_Consulta(Cat_Pre_Movimientos.Campo_Grupo_Id, Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos, Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + Hdn_Movimiento_ID.Value + "'");
                            break;
                        //case Ope_Pre_Ordenes_Variacion.Campo_Grupo_Movimiento_ID:
                        //    Hdn_Grupo_Movimiento_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                        //    break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Observaciones:
                            Txt_Observaciones_Cuenta.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            break;

                        case Ope_Pre_Ordenes_Variacion.Campo_Tasa_Predial_ID:
                            Hdn_Tasa_Predial_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Tasas_Predial.Campo_Identificador + " || ' - ' || " + Cat_Pre_Tasas_Predial.Campo_Descripcion, Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Txt_Tasa_Descripcion.Text = HttpUtility.HtmlDecode(Dato_Nuevo);
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Tasas_Predial.Campo_Identificador + " || ' - ' || " + Cat_Pre_Tasas_Predial.Campo_Descripcion, Cat_Pre_Tasas_Predial.Tabla_Cat_Pre_Tasas_Predial, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Txt_Tasa_Descripcion.ToolTip = HttpUtility.HtmlDecode(Dato_Anterior);
                                //Txt_Tasa_Descripcion.Font.Bold = true;
                                //Lbl_Tasa_Descripcion.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID:
                            Hdn_Tasa_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual, Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Txt_Tasa_Porcentaje.Text = Dato_Nuevo;
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual, Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Txt_Tasa_Porcentaje.ToolTip = Dato_Anterior;
                                //Txt_Tasa_Porcentaje.Font.Bold = true;
                                //Lbl_Tasa_Descripcion.Font.Bold = true;
                            }

                            Txt_Tasa_Excedente_Valor.Text = Txt_Tasa_Porcentaje.Text;
                            Txt_Tasa_Excedente_Construccion.Text = Txt_Tasa_Porcentaje.Text;
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Calle_ID:
                            Hdn_Calle_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Nombre, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Clave_Calle = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Clave, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Txt_Calle_Cuenta.Text = Clave_Calle + " " + HttpUtility.HtmlDecode(Dato_Nuevo);
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Nombre, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Clave_Calle = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Clave, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Txt_Calle_Cuenta.ToolTip = Clave_Calle + " " + HttpUtility.HtmlDecode(Dato_Anterior);
                                //Txt_Calle_Cuenta.Font.Bold = true;
                                //Lbl_Calle_Cuenta.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID:
                            Hdn_Colonia_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Nombre, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Clave_Colonia = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Clave, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Txt_Colonia_Cuenta.Text = Clave_Colonia + " " + HttpUtility.HtmlDecode(Dato_Nuevo);
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Nombre, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Clave_Colonia = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Clave, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Txt_Colonia_Cuenta.ToolTip = Clave_Colonia + " " + HttpUtility.HtmlDecode(Dato_Anterior);
                                //Txt_Colonia_Cuenta.Font.Bold = true;
                                //Lbl_Colonia_Cuenta.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Hdn_Calle_ID_Notificacion.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                                Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Nombre, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Hdn_Calle_ID_Notificacion.Value + "'");
                                Clave_Calle = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Clave, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Hdn_Calle_ID_Notificacion.Value + "'");
                                Calle_ID_Notificacion_Nuevo = Clave_Calle + " " + HttpUtility.HtmlDecode(Dato_Nuevo);
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Nombre, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Clave_Calle = Obtener_Dato_Consulta(Cat_Pre_Calles.Campo_Clave, Cat_Pre_Calles.Tabla_Cat_Pre_Calles, Cat_Pre_Calles.Campo_Calle_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Calle_ID_Notificacion_Anterior = Clave_Calle + " " + HttpUtility.HtmlDecode(Dato_Anterior);
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Hdn_Colonia_ID_Notificacion.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                                Dato_Nuevo = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Nombre, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Hdn_Colonia_ID_Notificacion.Value + "'");
                                Clave_Colonia = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Clave, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Hdn_Colonia_ID_Notificacion.Value + "'");
                                Colonia_ID_Notificacion_Nuevo = Clave_Colonia + " " + HttpUtility.HtmlDecode(Dato_Nuevo);
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Nombre, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Clave_Colonia = Obtener_Dato_Consulta(Cat_Ate_Colonias.Campo_Clave, Cat_Ate_Colonias.Tabla_Cat_Ate_Colonias, Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Colonia_ID_Notificacion_Anterior = Clave_Colonia + " " + HttpUtility.HtmlDecode(Dato_Anterior);
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Calle_Notificacion_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Calle_Notificacion_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Colonia_Notificacion_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Colonia_Notificacion_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_No_Exterior:
                            Txt_No_Exterior.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_No_Exterior.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_No_Exterior.Font.Bold = true;
                                //Lbl_No_Exterior.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_No_Interior:
                            Txt_No_Interior.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_No_Interior.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_No_Interior.Font.Bold = true;
                                //Lbl_No_Interior.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion:
                            Hdn_No_Exterior_Propietario_Nuevo.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Txt_Numero_Exterior_Propietario.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Hdn_No_Exterior_Propietario_Anterior.Value = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Numero_Exterior_Propietario.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Numero_Exterior_Propietario.Font.Bold = true;
                                //Lbl_Numero_Exterior_Propietario.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion:
                            Hdn_No_Interior_Propietario_Nuevo.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Txt_Numero_Interior_Propietario.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Hdn_No_Interior_Propietario_Anterior.Value = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Numero_Interior_Propietario.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Numero_Interior_Propietario.Font.Bold = true;
                                //Lbl_Numero_Interior_Propietario.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Hdn_Estado_ID_Notificacion.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                                Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Estados.Campo_Nombre, Cat_Pre_Estados.Tabla_Cat_Pre_Estados, Cat_Pre_Estados.Campo_Estado_ID + " = '" + Hdn_Estado_ID_Notificacion.Value + "'");
                                Estado_ID_Notificacion_Nuevo = HttpUtility.HtmlDecode(Dato_Nuevo);
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Estados.Campo_Nombre, Cat_Pre_Estados.Tabla_Cat_Pre_Estados, Cat_Pre_Estados.Campo_Estado_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Estado_ID_Notificacion_Anterior = HttpUtility.HtmlDecode(Dato_Anterior);
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Hdn_Ciudad_ID_Notificacion.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                                Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Ciudades.Campo_Nombre, Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades, Cat_Pre_Ciudades.Campo_Ciudad_ID + " = '" + Hdn_Ciudad_ID_Notificacion.Value + "'");
                                Ciudad_ID_Notificacion_Nuevo = HttpUtility.HtmlDecode(Dato_Nuevo);
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Ciudades.Campo_Nombre, Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades, Cat_Pre_Ciudades.Campo_Ciudad_ID + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Ciudad_ID_Notificacion_Anterior = HttpUtility.HtmlDecode(Dato_Anterior);
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Estado_Notificacion_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Estado_Notificacion_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion:
                            if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                            {
                                Ciudad_Notificacion_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            }
                            if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                            {
                                Ciudad_Notificacion_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Estado_Predio_ID:
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Estados_Predio.Campo_Descripcion, Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Txt_Estado_Predio.Text = Dato_Nuevo;
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Estados_Predio.Campo_Descripcion, Cat_Pre_Estados_Predio.Tabla_Cat_Pre_Estados_Predio, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Txt_Estado_Predio.ToolTip = Dato_Anterior;
                                //Txt_Estado_Predio.Font.Bold = true;
                                //Lbl_Estado_Predio.Font.Bold = true;
                            }
                            Hdn_Estado_Predio_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID:
                            Hdn_Tipo_Predio_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Tipos_Predio.Campo_Descripcion, Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Txt_Tipo_Predio.Text = Dato_Nuevo;
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Tipos_Predio.Campo_Descripcion, Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Txt_Tipo_Predio.ToolTip = Dato_Anterior;
                                //Txt_Tipo_Predio.Font.Bold = true;
                                //Lbl_Tipo_Predio.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID:
                            Hdn_Uso_Predio_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Uso_Suelo.Campo_Descripcion, Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            Txt_Uso_Predio.Text = Dato_Nuevo;
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Uso_Suelo.Campo_Descripcion, Cat_Pre_Uso_Suelo.Tabla_Cat_Pre_Uso_Suelo, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                                Txt_Uso_Predio.ToolTip = Dato_Anterior;
                                //Txt_Uso_Predio.Font.Bold = true;
                                //Lbl_Uso_Predio.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Cuota_Minima_ID:
                            Dato_Nuevo = Obtener_Dato_Consulta(Cat_Pre_Cuotas_Minimas.Campo_Cuota, Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() + "'");
                            if (Dato_Nuevo != "")
                            {
                                Txt_Cuota_Minima_Anual.Text = Convert.ToDecimal(Dato_Nuevo).ToString("##,###,##0.00");
                            }
                            //if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            //{
                            //    Dato_Anterior = Obtener_Dato_Consulta(Cat_Pre_Cuotas_Minimas.Campo_Cuota, Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas, Dr_Variacion_Cuenta["NOMBRE_CAMPO"].ToString() + " = '" + Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() + "'");
                            //    if (Dato_Anterior != "")
                            //    {
                            //        Txt_Cuota_Minima_Anual.ToolTip = Convert.ToDecimal(Dato_Anterior).ToString("##,###,##0.00");
                            //    }
                            //    Txt_Cuota_Minima_Anual.Font.Bold = true;
                            //    Lbl_Cuota_Minima_Anual.Font.Bold = true;
                            //}
                            Hdn_Cuota_Minima_ID.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Origen:
                            Txt_Cuenta_Origen.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Cuenta_Origen.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Cuenta_Origen.Font.Bold = true;
                                //Lbl_Cuenta_Origen.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta:
                            {
                                Txt_Estatus.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                                if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                                {
                                    Txt_Estatus.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                    //Txt_Estatus.Font.Bold = true;
                                    //Lbl_Estatus.Font.Bold = true;
                                }
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Superficie_Construida:
                            Dato_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Dato_Nuevo != "")
                            {
                                Txt_Superficie_Construida.Text = Convert.ToDecimal(Dato_Nuevo).ToString("##,###,##0.00");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                if (Dato_Anterior != "")
                                {
                                    Txt_Superficie_Construida.ToolTip = Convert.ToDecimal(Dato_Anterior).ToString("##,###,##0.00");
                                }
                                //Txt_Superficie_Construida.Font.Bold = true;
                                //Lbl_Superficie_Construida.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Superficie_Total:
                            Dato_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Dato_Nuevo != "")
                            {
                                Txt_Superficie_Total.Text = Convert.ToDecimal(Dato_Nuevo).ToString("##,###,##0.00");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                if (Dato_Anterior != "")
                                {
                                    Txt_Superficie_Total.ToolTip = Convert.ToDecimal(Dato_Anterior).ToString("##,###,##0.00");
                                }
                                //Txt_Superficie_Total.Font.Bold = true;
                                //Lbl_Superficie_Total.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Clave_Catastral:
                            Txt_Catastral.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Catastral.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Catastral.Font.Bold = true;
                                //Lbl_Catastral.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal:
                            Dato_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Dato_Nuevo != "")
                            {
                                Txt_Valor_Fiscal.Text = Convert.ToDecimal(Dato_Nuevo).ToString("##,###,##0.00");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                if (Dato_Anterior != "")
                                {
                                    Txt_Valor_Fiscal.ToolTip = Convert.ToDecimal(Dato_Anterior).ToString("##,###,##0.00");
                                }
                                //Txt_Valor_Fiscal.Font.Bold = true;
                                //Lbl_Valor_Fiscal.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Efectos:
                            Txt_Efectos.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Efectos.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Efectos.Font.Bold = true;
                                //Lbl_Efectos.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Periodo_Corriente:
                            Txt_Periodo_Corriente.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            //if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            //{
                            //    Txt_Periodo_Corriente.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            //    Txt_Periodo_Corriente.Font.Bold = true;
                            //    Lbl_Periodo_Corriente.Font.Bold = true;
                            //}
                            Periodo_Corriente = HttpUtility.HtmlDecode(Txt_Periodo_Corriente.Text);
                            if (Periodo_Corriente != "")
                            {
                                Año_Periodo_Corriente = Convert.ToInt16(Periodo_Corriente.Substring(Periodo_Corriente.Length - 4));
                                Hdn_Cuota_Minima.Value = Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo_Corriente.ToString()).ToString();
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual:
                            Dato_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Dato_Nuevo != "")
                            {
                                Txt_Cuota_Anual.Text = Convert.ToDecimal(Dato_Nuevo).ToString("##,###,##0.00");
                                Txt_Cuota_Bimestral.Text = (Convert.ToDecimal(Dato_Nuevo) / 6).ToString("##,###,##0.00");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                if (Dato_Anterior != "")
                                {
                                    Txt_Cuota_Anual.ToolTip = Convert.ToDecimal(Dato_Anterior).ToString("##,###,##0.00");
                                    Txt_Cuota_Bimestral.ToolTip = (Convert.ToDecimal(Dato_Anterior) / 6).ToString("##,###,##0.00");
                                }
                                //Txt_Cuota_Anual.Font.Bold = true;
                                //Lbl_Cuota_Anual.Font.Bold = true;
                                //Txt_Cuota_Bimestral.Font.Bold = true;
                                //Lbl_Cuota_Bimestral.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Cuota_Fija:
                            Txt_Cuota_Fija.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Cuota_Fija.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Cuota_Fija.Font.Bold = true;
                                //Lbl_Cuota_Fija.Font.Bold = true;
                            }
                            if (Txt_Cuota_Fija.Text == "SI")
                            {
                                Chk_Cuota_Fija.Checked = true;
                            }
                            else
                            {
                                Chk_Cuota_Fija.Checked = false;
                            }
                            Chk_Cuota_Fija_CheckedChanged(null, null);
                            break;
                        case Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija:
                            Hdn_No_Cuota_Fija_Nuevo.Value = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Hdn_No_Cuota_Fija_Nuevo.Value != "")
                            {
                                Session["Cuota_Fija_Nueva"] = Convert.ToDecimal(Obtener_Dato_Consulta(Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija, Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas, Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = '" + Hdn_No_Cuota_Fija_Nuevo.Value + "'"));
                            }
                            Hdn_No_Cuota_Fija_Anterior.Value = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                            if (Hdn_No_Cuota_Fija_Anterior.Value != "")
                            {
                                Session["Cuota_Fija_Anterior"] = Convert.ToDecimal(Obtener_Dato_Consulta(Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija, Ope_Pre_Cuotas_Fijas.Tabla_Ope_Pre_Cuotas_Fijas, Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija + " = '" + Hdn_No_Cuota_Fija_Anterior.Value + "'"));
                            }
                            if (Txt_Cuota_Fija.Text == "SI")
                            {
                                Cargar_Datos_Cuota_Fija(Dr_Variacion_Cuenta["DATO_NUEVO"].ToString());
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Diferencia_Construccion:
                            if (Hdn_Contrarecibo.Value == "")
                            {
                                Txt_Diferencia_Construccion.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                                if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                                {
                                    Txt_Diferencia_Construccion.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                    //Txt_Diferencia_Construccion.Font.Bold = true;
                                    //Lbl_Diferencia_Construccion.Font.Bold = true;
                                }
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion:
                            DateTime Termino_Exencion_Nuevo = DateTime.MinValue;
                            DateTime Termino_Exencion_Anterior = DateTime.MinValue;

                            if (Dr_Variacion_Cuenta["DATO_NUEVO"] != null)
                            {
                                if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                                {
                                    Termino_Exencion_Nuevo = Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_NUEVO"].ToString());
                                }
                            }
                            if (Termino_Exencion_Nuevo != DateTime.MinValue)
                            {
                                Txt_Termino_Exencion.Text = Termino_Exencion_Nuevo.ToString("dd/MMM/yyyy");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                if (Dr_Variacion_Cuenta["DATO_ANTERIOR"] != null)
                                {
                                    if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                                    {
                                        Termino_Exencion_Anterior = Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString());
                                    }
                                }
                                if (Termino_Exencion_Anterior != DateTime.MinValue)
                                {
                                    Txt_Termino_Exencion.ToolTip = Termino_Exencion_Anterior.ToString("dd/MMM/yyyy");
                                }
                                //Txt_Termino_Exencion.Font.Bold = true;
                                //Lbl_Termino_Exencion.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo:
                            DateTime Fecha_Avaluo_Nuevo = DateTime.MinValue;
                            DateTime Fecha_Avaluo_Anterior = DateTime.MinValue;

                            if (Dr_Variacion_Cuenta["DATO_NUEVO"] != null)
                            {
                                if (Dr_Variacion_Cuenta["DATO_NUEVO"].ToString() != "")
                                {
                                    Fecha_Avaluo_Nuevo = Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_NUEVO"].ToString());
                                }
                            }
                            if (Fecha_Avaluo_Nuevo != DateTime.MinValue)
                            {
                                Txt_Fecha_Avaluo.Text = Fecha_Avaluo_Nuevo.ToString("dd/MMM/yyyy");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                if (Dr_Variacion_Cuenta["DATO_ANTERIOR"] != null)
                                {
                                    if (Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString() != "")
                                    {
                                        Fecha_Avaluo_Anterior = Convert.ToDateTime(Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString());
                                    }
                                }
                                if (Fecha_Avaluo_Anterior != DateTime.MinValue)
                                {
                                    Txt_Fecha_Avaluo.ToolTip = Fecha_Avaluo_Anterior.ToString("dd/MMM/yyyy");
                                }
                                //Txt_Fecha_Avaluo.Font.Bold = true;
                                //Lbl_Fecha_Avaluo.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Costo_M2:
                            Dato_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();

                            if (Dato_Nuevo != "")
                            {
                                Txt_Costo_M2.Text = Convert.ToDecimal(Dato_Nuevo).ToString("##,###,##0.00");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                if (Dato_Anterior != "")
                                {
                                    Txt_Costo_M2.ToolTip = Convert.ToDecimal(Dato_Anterior).ToString("##,###,##0.00");
                                }
                                //Txt_Costo_M2.Font.Bold = true;
                                //Lbl_Costo_M2.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Porcentaje_Exencion:
                            Dato_Nuevo = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Dato_Nuevo != "")
                            {
                                Txt_Porcentaje_Exencion.Text = Convert.ToDecimal(Dato_Nuevo).ToString("##,###,##0.00");
                            }
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Dato_Anterior = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                if (Dato_Anterior != "")
                                {
                                    Txt_Porcentaje_Exencion.ToolTip = Convert.ToDecimal(Dato_Anterior).ToString("##,###,##0.00");
                                }
                                //Txt_Porcentaje_Exencion.Font.Bold = true;
                                //Lbl_Porcentaje_Exencion.Font.Bold = true;
                            }
                            break;

                        //DATOS PROPIETARIO
                        case Cat_Pre_Propietarios.Campo_Tipo:
                            Txt_Tipo_Propietario.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Tipo_Propietario.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Tipo_Propietario.Font.Bold = true;
                                //Lbl_Tipo_Propietario.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo:
                            Txt_Domicilio_Foraneo.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_Domicilio_Foraneo.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_Domicilio_Foraneo.Font.Bold = true;
                                //Lbl_Domicilio_Foraneo.Font.Bold = true;
                            }
                            break;
                        case Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal:
                            Txt_CP.Text = Dr_Variacion_Cuenta["DATO_NUEVO"].ToString();
                            if (Convert.ToBoolean(Dr_Variacion_Cuenta["DIFERENTE"]))
                            {
                                Txt_CP.ToolTip = Dr_Variacion_Cuenta["DATO_ANTERIOR"].ToString();
                                //Txt_CP.Font.Bold = true;
                                //Lbl_CP.Font.Bold = true;
                            }
                            break;
                    }

                    Variacion_Cargada = true;
                }

                if (Identificador_Movimiento_Nuevo.Trim() != "")
                {
                    Txt_Tipo_Movimiento.Text = Identificador_Movimiento_Nuevo + " - " + Descripcion_Movimiento_Nuevo;
                    //Txt_Tipo_Movimiento.ToolTip = Identificador_Movimiento_Anterior + " - " + Descripcion_Movimiento_Anterior;
                    //Txt_Tipo_Movimiento.Font.Bold = true;
                    //Lbl_Tipo_Movimiento.Font.Bold = true;
                }

                {
                    if (Txt_Domicilio_Foraneo.Text.Trim() == "SI")
                    {
                        Chk_Mismo_Domicilio.Checked = false;
                    }
                    else
                    {
                        if ((HttpUtility.HtmlDecode(Hdn_Colonia_ID_Notificacion.Value).Trim() != ""
                            && HttpUtility.HtmlDecode(Hdn_Calle_ID_Notificacion.Value).Trim() != "")
                            && (Hdn_Colonia_ID.Value == Hdn_Colonia_ID_Notificacion.Value
                            && Hdn_Calle_ID.Value == Hdn_Calle_ID_Notificacion.Value)
                            && (Txt_No_Exterior.Text.Trim().ToUpper() == Txt_Numero_Exterior_Propietario.Text.Trim().ToUpper())
                            && (Txt_No_Interior.Text.Trim().ToUpper() == Txt_Numero_Interior_Propietario.Text.Trim().ToUpper()))
                        {
                            Chk_Mismo_Domicilio.Checked = true;

                            Txt_Calle_Propietario.Text = Txt_Calle_Cuenta.Text;
                            Hdn_Calle_ID_Notificacion.Value = Hdn_Calle_ID.Value;
                            Txt_Colonia_Propietario.Text = Txt_Colonia_Cuenta.Text;
                            Hdn_Colonia_ID_Notificacion.Value = Hdn_Colonia_ID.Value;
                            Txt_Numero_Interior_Propietario.Text = Txt_No_Interior.Text;
                            Txt_Numero_Exterior_Propietario.Text = Txt_No_Exterior.Text;
                            Txt_Ciudad_Propietario.Text = Ciudad_ID_Notificacion_Nuevo;
                            Txt_Estado_Propietario.Text = Estado_ID_Notificacion_Nuevo;
                        }
                        else
                        {
                            Chk_Mismo_Domicilio.Checked = false;
                        }
                    }
                    if ((Calle_ID_Notificacion_Nuevo != null && Calle_ID_Notificacion_Nuevo != "")
                        || (Colonia_ID_Notificacion_Nuevo != null && Colonia_ID_Notificacion_Nuevo != ""))
                    {
                        if (!Chk_Mismo_Domicilio.Checked)
                        {
                            Txt_Calle_Propietario.Text = Calle_ID_Notificacion_Nuevo;
                            Txt_Colonia_Propietario.Text = Colonia_ID_Notificacion_Nuevo;
                            Txt_Ciudad_Propietario.Text = Ciudad_ID_Notificacion_Nuevo;
                            Txt_Estado_Propietario.Text = Estado_ID_Notificacion_Nuevo;
                        }
                        //if (Calle_ID_Notificacion_Nuevo != Calle_ID_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Calle_Propietario.ToolTip = Calle_ID_Notificacion_Anterior;
                        //    Txt_Calle_Propietario.Font.Bold = true;
                        //    Lbl_Calle_Propietario.Font.Bold = true;
                        //}
                        //if (Colonia_ID_Notificacion_Nuevo != Colonia_ID_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Colonia_Propietario.ToolTip = Colonia_ID_Notificacion_Anterior;
                        //    Txt_Colonia_Propietario.Font.Bold = true;
                        //    Lbl_Colonia_Propietario.Font.Bold = true;
                        //}
                        //if (Ciudad_ID_Notificacion_Nuevo != Ciudad_ID_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Ciudad_Propietario.ToolTip = Ciudad_ID_Notificacion_Anterior;
                        //    Txt_Ciudad_Propietario.Font.Bold = true;
                        //    Lbl_Ciudad_Propietario.Font.Bold = true;
                        //}
                        //if (Estado_ID_Notificacion_Nuevo != Estado_ID_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Estado_Propietario.ToolTip = Estado_ID_Notificacion_Anterior;
                        //    Txt_Estado_Propietario.Font.Bold = true;
                        //    Lbl_Estado_Propietario.Font.Bold = true;
                        //}
                    }
                    else
                    {
                        if (!Chk_Mismo_Domicilio.Checked)
                        {
                            Txt_Calle_Propietario.Text = Calle_Notificacion_Nuevo;
                            Txt_Colonia_Propietario.Text = Colonia_Notificacion_Nuevo;
                            Txt_Ciudad_Propietario.Text = Ciudad_Notificacion_Nuevo;
                            Txt_Estado_Propietario.Text = Estado_Notificacion_Nuevo;
                        }
                        //if (Calle_Notificacion_Nuevo != Calle_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Calle_Propietario.ToolTip = Calle_Notificacion_Anterior;
                        //    Txt_Calle_Propietario.Font.Bold = true;
                        //    Lbl_Calle_Propietario.Font.Bold = true;
                        //}
                        //if (Colonia_Notificacion_Nuevo != Colonia_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Colonia_Propietario.ToolTip = Colonia_Notificacion_Anterior;
                        //    Txt_Colonia_Propietario.Font.Bold = true;
                        //    Lbl_Colonia_Propietario.Font.Bold = true;
                        //}
                        //if (Ciudad_Notificacion_Nuevo != Ciudad_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Ciudad_Propietario.ToolTip = Ciudad_Notificacion_Anterior;
                        //    Txt_Ciudad_Propietario.Font.Bold = true;
                        //    Lbl_Ciudad_Propietario.Font.Bold = true;
                        //}
                        //if (Estado_Notificacion_Nuevo != Estado_Notificacion_Anterior
                        //    && Txt_Estatus.Text.Trim() != "CANCELADA"
                        //    && Txt_Estatus.ToolTip.Trim() != "CANCELADA")
                        //{
                        //Txt_Estado_Propietario.ToolTip = Estado_Notificacion_Anterior;
                        //    Txt_Estado_Propietario.Font.Bold = tsrue;
                        //    Lbl_Estado_Propietario.Font.Bold = true;
                        //}
                    }
                }
            }

            //Calcular_Cuota();
            //Validar_Cuota_Minima();
            //Calcular_Excedentes();
            //Cargar_Datos_Propietarios_Cuenta();
            if (Cargar_Variacion_Propietarios())
            {
                Variacion_Cargada = true;
            }
            //Cargar_Grid_Copropietarios_Cuenta(0);
            if (Cargar_Grid_Variacion_Copropietarios(0))
            {
                Variacion_Cargada = true;
            }
            if (!Validar_Cuenta_Cancelada())
            {
                if (Cargar_Grid_Variacion_Diferencias(0))
                {
                    Variacion_Cargada = true;
                }
            }
        }

        return Variacion_Cargada;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validar_Cuenta_Cancelada
    ///DESCRIPCIÓN          : Valida si el Movimiento es de Cancelación
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 12/Diciembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Validar_Cuenta_Cancelada()
    {
        Boolean Cuenta_Cancelada = false;
        if (Hdn_Cargar_Modulos.Value.Contains("CANCELACION_CUENTAS"))
        {
            Cuenta_Cancelada = true;
        }
        return Cuenta_Cancelada;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Variacion_Propietarios
    ///DESCRIPCIÓN          : Consulta los datos del Propietario
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Cargar_Variacion_Propietarios()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataSet Ds_Propietarios_Variacion;
        DataTable Dt_Propietarios_Variacion;
        Boolean Propietario_Conusltado = false;
        try
        {
            Ordenes_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Ordenes_Variacion.P_Contrarecibo = Hdn_Contrarecibo.Value;
            Ordenes_Variacion.P_Orden_Variacion_ID = Hdn_No_Orden_Variacion.Value;
            //Ordenes_Variacion.P_Año = Año_Orden_Variacion;
            Ordenes_Variacion.P_Fecha_Modifico = Fecha_Orden_Variacion;
            //Ordenes_Variacion.P_Propietario_Filtra_Estatus = true;
            Ordenes_Variacion.P_Estatus_Orden = "ACEPTADA";
            Ds_Propietarios_Variacion = Ordenes_Variacion.Consultar_Propietarios_Variacion();
            if (Ds_Propietarios_Variacion != null)
            {
                if (Ds_Propietarios_Variacion.Tables.Count > 0)
                {
                    Dt_Propietarios_Variacion = Ds_Propietarios_Variacion.Tables[0];
                    if (Dt_Propietarios_Variacion.Rows.Count > 0)
                    {
                        if (Dt_Propietarios_Variacion.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString() != Txt_Nombre_Propietario.Text)
                        {
                            if (Dt_Propietarios_Variacion.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString() != Txt_Nombre_Propietario.Text)
                            {
                                Hdn_Propietario_ID.Value = Dt_Propietarios_Variacion.Rows[0][Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                                Txt_Nombre_Propietario.ToolTip = Txt_Nombre_Propietario.Text;
                                Txt_Nombre_Propietario.Text = Dt_Propietarios_Variacion.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                                //Txt_Nombre_Propietario.Font.Bold = true;
                                //Lbl_Nombre_Propietario.Font.Bold = true;
                            }
                        }
                        if (Dt_Propietarios_Variacion.Rows[0]["RFC"].ToString() != Txt_RFC_Propietario.Text)
                        {
                            if (Dt_Propietarios_Variacion.Rows[0]["RFC"].ToString() != Txt_RFC_Propietario.Text)
                            {
                                Txt_RFC_Propietario.ToolTip = Txt_RFC_Propietario.Text;
                                Txt_RFC_Propietario.Text = Dt_Propietarios_Variacion.Rows[0]["RFC"].ToString();
                                //Txt_RFC_Propietario.Font.Bold = true;
                                //Lbl_RFC_Propietario.Font.Bold = true;
                            }
                        }
                        if (Dt_Propietarios_Variacion.Rows[0]["TIPO_PROPIETARIO"].ToString() != Txt_Tipo_Propietario.Text)
                        {
                            if (Dt_Propietarios_Variacion.Rows[0]["TIPO_PROPIETARIO"].ToString() != Txt_Tipo_Propietario.Text)
                            {
                                Txt_Tipo_Propietario.ToolTip = Txt_Tipo_Propietario.Text;
                                Txt_Tipo_Propietario.Text = Dt_Propietarios_Variacion.Rows[0]["TIPO_PROPIETARIO"].ToString();
                                //Txt_Tipo_Propietario.Font.Bold = true;
                                //Lbl_Tipo_Propietario.Font.Bold = true;
                            }
                        }
                        Propietario_Conusltado = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Propietario_Conusltado;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Variacion_Copropietarios
    ///DESCRIPCIÓN          : Carga el grid de Copropietarios con las Variaciones
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Cargar_Grid_Variacion_Copropietarios(int Indice_Pagina)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataSet Ds_Copropietarios_Variacion;
        DataTable Dt_Copropietarios_Variacion;
        DataTable Dt_Temp_Copropietarios = new DataTable();
        DataRow Dr_Temp_Copropietario;
        Boolean Copropietarios_Consultados = false;
        try
        {
            Ordenes_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Ordenes_Variacion.P_Contrarecibo = Hdn_Contrarecibo.Value;
            Ordenes_Variacion.P_Orden_Variacion_ID = Hdn_No_Orden_Variacion.Value;
            Ordenes_Variacion.P_Año = Año_Orden_Variacion;
            //Ordenes_Variacion.P_Copropietario_Filtra_Estatus = true;
            Ds_Copropietarios_Variacion = Ordenes_Variacion.Consultar_Copropietarios_Variacion();
            if (Ds_Copropietarios_Variacion != null)
            {
                if (Ds_Copropietarios_Variacion.Tables.Count > 0)
                {
                    Dt_Copropietarios_Variacion = Ds_Copropietarios_Variacion.Tables[0];
                    if (Dt_Copropietarios_Variacion.Rows.Count > 0)
                    {
                        Dt_Temp_Copropietarios.Columns.Add(new DataColumn("CONTRIBUYENTE_ID", typeof(String)));
                        Dt_Temp_Copropietarios.Columns.Add(new DataColumn("RFC", typeof(String)));
                        Dt_Temp_Copropietarios.Columns.Add(new DataColumn("NOMBRE_CONTRIBUYENTE", typeof(String)));
                        Dt_Temp_Copropietarios.Columns.Add(new DataColumn("ESTATUS_VARIACION", typeof(String)));

                        foreach (DataRow Copropietario_Variacion in Dt_Copropietarios_Variacion.Rows)
                        {
                            Dr_Temp_Copropietario = Dt_Temp_Copropietarios.NewRow();
                            Dr_Temp_Copropietario["CONTRIBUYENTE_ID"] = Copropietario_Variacion[Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString();
                            Dr_Temp_Copropietario["RFC"] = Copropietario_Variacion["RFC"].ToString();
                            Dr_Temp_Copropietario["NOMBRE_CONTRIBUYENTE"] = Copropietario_Variacion["NOMBRE_CONTRIBUYENTE"].ToString();
                            Dr_Temp_Copropietario["ESTATUS_VARIACION"] = "NUEVO";
                            Dt_Temp_Copropietarios.Rows.Add(Dr_Temp_Copropietario);
                        }
                        Grid_Copropietarios.DataSource = Dt_Temp_Copropietarios;
                        Grid_Copropietarios.PageIndex = Indice_Pagina;
                        Grid_Copropietarios.DataBind();

                        Copropietarios_Consultados = true;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Copropietarios_Consultados;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validar_Mismo_Domicilio
    ///DESCRIPCIÓN          : Valida los datos del domicilio del Predio con los datos del domicilio del Propietario
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Validar_Mismo_Domicilio()
    {
        if (HttpUtility.HtmlDecode(Txt_Domicilio_Foraneo.Text).Trim() == "SI")
        {
            Chk_Mismo_Domicilio.Checked = false;
            if (HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Nuevo.Value).ToUpper().Trim() != Txt_Colonia_Propietario.Text.Trim())
                {
                    Txt_Colonia_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Nuevo.Value).ToUpper().Trim();
                    Txt_Colonia_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Anterior.Value).ToUpper().Trim();
                    //Txt_Colonia_Propietario.Font.Bold = true;
                    //Lbl_Colonia_Propietario.Font.Bold = true;
                }
            }

            if (HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Nuevo.Value).ToUpper().Trim() != Txt_Calle_Propietario.Text.Trim())
                {
                    Txt_Calle_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Nuevo.Value).ToUpper().Trim();
                    Txt_Calle_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Anterior.Value).ToUpper().Trim();
                    //Txt_Calle_Propietario.Font.Bold = true;
                    //Lbl_Calle_Propietario.Font.Bold = true;
                }
            }

            if (HttpUtility.HtmlDecode(Hdn_No_Exterior_Propietario_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_No_Exterior_Propietario_Nuevo.Value).ToUpper().Trim() != Txt_Numero_Exterior_Propietario.Text.Trim())
                {
                    Txt_Numero_Exterior_Propietario.Text = HttpUtility.HtmlDecode(Hdn_No_Exterior_Propietario_Nuevo.Value).ToUpper().Trim();
                    Txt_Numero_Exterior_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_No_Exterior_Propietario_Anterior.Value).ToUpper().Trim();
                    //Txt_Numero_Exterior_Propietario.Font.Bold = true;
                    //Lbl_Numero_Exterior_Propietario.Font.Bold = true;
                }
            }

            if (HttpUtility.HtmlDecode(Hdn_No_Interior_Propietario_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_No_Interior_Propietario_Nuevo.Value).ToUpper().Trim() != Txt_Numero_Interior_Propietario.Text.Trim())
                {
                    Txt_Numero_Interior_Propietario.Text = HttpUtility.HtmlDecode(Hdn_No_Interior_Propietario_Nuevo.Value).ToUpper().Trim();
                    Txt_Numero_Interior_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_No_Interior_Propietario_Anterior.Value).ToUpper().Trim();
                    //Txt_Numero_Interior_Propietario.Font.Bold = true;
                    //Lbl_Numero_Interior_Propietario.Font.Bold = true;
                }
            }

            if (HttpUtility.HtmlDecode(Hdn_Estado_Notificacion_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_Estado_Notificacion_Nuevo.Value).ToUpper().Trim() != Txt_Estado_Propietario.Text.Trim())
                {
                    Txt_Estado_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Estado_Notificacion_Nuevo.Value).ToUpper().Trim();
                    Txt_Estado_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_Estado_Notificacion_Anterior.Value).ToUpper().Trim();
                    //Txt_Estado_Propietario.Font.Bold = true;
                    //Lbl_Estado_Propietario.Font.Bold = true;
                }
            }

            if (HttpUtility.HtmlDecode(Hdn_Ciudad_Notificacion_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_Ciudad_Notificacion_Nuevo.Value).ToUpper().Trim() != Txt_Ciudad_Propietario.Text.Trim())
                {
                    Txt_Ciudad_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Ciudad_Notificacion_Nuevo.Value).ToUpper().Trim();
                    Txt_Ciudad_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_Ciudad_Notificacion_Anterior.Value).ToUpper().Trim();
                    //Txt_Ciudad_Propietario.Font.Bold = true;
                    //Lbl_Ciudad_Propietario.Font.Bold = true;
                }
            }
        }
        else
        {
            //if (HttpUtility.HtmlDecode(Txt_Domicilio_Foraneo.Text).Trim() == "NO")
            //{
            if ((HttpUtility.HtmlDecode(Hdn_Colonia_ID_Notificacion.Value).Trim() != ""
                && HttpUtility.HtmlDecode(Hdn_Calle_ID_Notificacion.Value).Trim() != "")
                && (Hdn_Colonia_ID_Notificacion.Value == Hdn_Colonia_ID.Value
                && Hdn_Calle_ID_Notificacion.Value == Hdn_Calle_ID.Value)
                && (Txt_No_Exterior.Text.Trim().ToUpper() == Txt_Numero_Exterior_Propietario.Text.Trim().ToUpper())
                && (Txt_No_Interior.Text.Trim().ToUpper() == Txt_Numero_Interior_Propietario.Text.Trim().ToUpper()))
            {
                Chk_Mismo_Domicilio.Checked = true;
                if (HttpUtility.HtmlDecode(Txt_Colonia_Cuenta.Text).Trim() != "")
                {
                    if (HttpUtility.HtmlDecode(Txt_Colonia_Cuenta.Text).ToUpper().Trim() != Txt_Colonia_Propietario.Text.Trim())
                    {
                        Hdn_Colonia_ID_Notificacion.Value = Hdn_Colonia_ID.Value.ToUpper().Trim();
                        Txt_Colonia_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Colonia_Propietario.Text).ToUpper().Trim();
                        Txt_Colonia_Propietario.Text = HttpUtility.HtmlDecode(Txt_Colonia_Cuenta.Text).ToUpper().Trim();
                        //Txt_Colonia_Propietario.Font.Bold = true;
                        //Lbl_Colonia_Propietario.Font.Bold = true;
                    }
                }
                if (HttpUtility.HtmlDecode(Txt_Calle_Cuenta.Text).Trim() != "")
                {
                    if (HttpUtility.HtmlDecode(Txt_Calle_Cuenta.Text).ToUpper().Trim() != Txt_Calle_Propietario.Text.Trim())
                    {
                        Hdn_Calle_ID_Notificacion.Value = Hdn_Calle_ID.Value.ToUpper().Trim();
                        Txt_Calle_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Calle_Propietario.Text).ToUpper().Trim();
                        Txt_Calle_Propietario.Text = HttpUtility.HtmlDecode(Txt_Calle_Cuenta.Text).ToUpper().Trim();
                        //Txt_Calle_Propietario.Font.Bold = true;
                        //Lbl_Calle_Propietario.Font.Bold = true;
                    }
                }
                if (HttpUtility.HtmlDecode(Txt_No_Exterior.Text).Trim() != "")
                {
                    if (HttpUtility.HtmlDecode(Txt_No_Exterior.Text).ToUpper().Trim() != Txt_Numero_Exterior_Propietario.Text.Trim())
                    {
                        Txt_Numero_Exterior_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Numero_Exterior_Propietario.Text).ToUpper().Trim();
                        Txt_Numero_Exterior_Propietario.Text = HttpUtility.HtmlDecode(Txt_No_Exterior.Text).ToUpper().Trim();
                        //Txt_Numero_Exterior_Propietario.Font.Bold = true;
                        //Lbl_Numero_Exterior_Propietario.Font.Bold = true;
                    }
                }
                if (HttpUtility.HtmlDecode(Txt_No_Interior.Text).Trim() != "")
                {
                    if (HttpUtility.HtmlDecode(Txt_No_Interior.Text).ToUpper().Trim() != Txt_Numero_Interior_Propietario.Text.Trim())
                    {
                        Txt_Numero_Interior_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Numero_Interior_Propietario.Text).ToUpper().Trim();
                        Txt_Numero_Interior_Propietario.Text = HttpUtility.HtmlDecode(Txt_No_Interior.Text).ToUpper().Trim();
                        //Txt_Numero_Interior_Propietario.Font.Bold = true;
                        //Lbl_Numero_Interior_Propietario.Font.Bold = true;
                    }
                }
                if (HttpUtility.HtmlDecode(Hdn_Estado_Cuenta.Value).Trim() != "")
                {
                    if (HttpUtility.HtmlDecode(Hdn_Estado_Cuenta.Value).ToUpper().Trim() != Txt_Estado_Propietario.Text.Trim())
                    {
                        Txt_Estado_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Estado_Propietario.Text).ToUpper().Trim();
                        Txt_Estado_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Estado_Cuenta.Value).ToUpper().Trim();
                        //Txt_Estado_Propietario.Font.Bold = true;
                        //Lbl_Estado_Propietario.Font.Bold = true;
                    }
                }
                if (HttpUtility.HtmlDecode(Hdn_Ciudad_Cuenta.Value).Trim() != "")
                {
                    if (HttpUtility.HtmlDecode(Hdn_Ciudad_Cuenta.Value).ToUpper().Trim() != Txt_Ciudad_Propietario.Text.Trim())
                    {
                        Txt_Ciudad_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Ciudad_Propietario.Text).ToUpper().Trim();
                        Txt_Ciudad_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Ciudad_Cuenta.Value).ToUpper().Trim();
                        //Txt_Ciudad_Propietario.Font.Bold = true;
                        //Lbl_Ciudad_Propietario.Font.Bold = true;
                    }
                }
            }
            else
            {
                Chk_Mismo_Domicilio.Checked = false;
            }
            //}
            //else
            //{
            //    Chk_Mismo_Domicilio.Checked = false;
            //}

            //if (HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Nuevo.Value).Trim() != "")
            //{
            //    if (HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Nuevo.Value).Trim() != Txt_Colonia_Propietario.Text.Trim())
            //    {
            //        Txt_Colonia_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Nuevo.Value).Trim();
            //        Txt_Colonia_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_Colonia_Notificacion_Anterior.Value).Trim();
            //        Txt_Colonia_Propietario.Font.Bold = true;
            //        Lbl_Colonia_Propietario.Font.Bold = true;
            //    }
            //}
            //if (HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Nuevo.Value).Trim() != "")
            //{
            //    if (HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Nuevo.Value).Trim() != Txt_Calle_Propietario.Text.Trim())
            //    {
            //        Txt_Calle_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Nuevo.Value).Trim();
            //        Txt_Calle_Propietario.ToolTip = HttpUtility.HtmlDecode(Hdn_Calle_Notificacion_Anterior.Value).Trim();
            //        Txt_Calle_Propietario.Font.Bold = true;
            //        Lbl_Calle_Propietario.Font.Bold = true;
            //    }
            //}
            if (HttpUtility.HtmlDecode(Hdn_Estado_Notificacion_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_Estado_Notificacion_Nuevo.Value).Trim() != Txt_Estado_Propietario.Text.Trim())
                {
                    Txt_Estado_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Estado_Propietario.Text).Trim();
                    Txt_Estado_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Estado_Notificacion_Nuevo.Value).Trim();
                    Txt_Estado_Propietario.Font.Bold = true;
                    Lbl_Estado_Propietario.Font.Bold = true;
                }
            }
            if (HttpUtility.HtmlDecode(Hdn_Ciudad_Notificacion_Nuevo.Value).Trim() != "")
            {
                if (HttpUtility.HtmlDecode(Hdn_Ciudad_Notificacion_Nuevo.Value).Trim() != Txt_Ciudad_Propietario.Text.Trim())
                {
                    Txt_Ciudad_Propietario.ToolTip = HttpUtility.HtmlDecode(Txt_Ciudad_Propietario.Text).Trim();
                    Txt_Ciudad_Propietario.Text = HttpUtility.HtmlDecode(Hdn_Ciudad_Notificacion_Nuevo.Value).Trim();
                    Txt_Ciudad_Propietario.Font.Bold = true;
                    Lbl_Ciudad_Propietario.Font.Bold = true;
                }
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Historial_Observaciones
    ///DESCRIPCIÓN          : Carga el Grid con los datos del Historial de las Observaciones
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Grid_Historial_Observaciones(int Pagina)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Ordenes_Variacion.P_Orden_Variacion_ID = No_Orden_Variacion;
        //if (Txt_Busqueda.Text.Trim() != "")
        //{
        //    Ordenes_Variacion.P_Orden_Variacion_ID = "LIKE '%" + Txt_Busqueda.Text + "%'";
        //}
        //else
        //{
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            Ordenes_Variacion.P_Cuenta_Predial = "LIKE UPPER('%" + Txt_Cuenta_Predial.Text + "%')";
        }
        //}
        Ordenes_Variacion.P_Generar_Orden_Anio = Año_Orden_Variacion.ToString();
        Ordenes_Variacion.P_Observaciones_No_Orden_Variacion = No_Orden_Variacion;
        Ordenes_Variacion.P_Año = Año_Orden_Variacion;
        Ordenes_Variacion.Consultar_Ordenes_Variacion();
        Grid_Historial_Observaciones.DataSource = Ordenes_Variacion.P_Dt_Observaciones;
        Grid_Historial_Observaciones.PageIndex = Pagina;
        Grid_Historial_Observaciones.DataBind();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Datos_Propietario
    ///DESCRIPCIÓN          : asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Propietario(DataTable dataTable)
    {
        try
        {
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                Hdn_Propietario_ID.Value = dataTable.Rows[0]["CONTRIBUYENTE"].ToString();

                Txt_Nombre_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                if (dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                {
                    Txt_Tipo_Propietario.Text = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                }

                Txt_RFC_Propietario.Text = dataTable.Rows[0]["RFC"].ToString();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Propietario: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Cuota_Fija
    ///DESCRIPCIÓN: asignar datos cuota fijao
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 05/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Datos_Cuota_Fija(String Cuota_Fija_ID)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Cuotas_Fijas;
        try
        {
            Ordenes_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Ordenes_Variacion.P_No_Cuota_Fija = Cuota_Fija_ID;
            Dt_Cuotas_Fijas = Ordenes_Variacion.Consultar_Cuota_Fija_Detalles();

            if (Dt_Cuotas_Fijas != null)
            {
                if (Dt_Cuotas_Fijas.Rows.Count > 0)
                {
                    Decimal Cuota_Minima_Aplicar = 0;
                    Decimal Excedente_Construccion_Total = 0;
                    Decimal Excedente_Valor_Total = 0;

                    Hdn_No_Cuota_Fija_Nuevo.Value = Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_No_Cuota_Fija].ToString();
                    Hdn_Caso_Especial_ID.Value = Dt_Cuotas_Fijas.Rows[0][Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID].ToString();
                    if (Dt_Cuotas_Fijas.Rows[0][Cat_Pre_Casos_Especiales.Campo_Tipo].ToString() == "SOLICITANTE")
                    {
                        Txt_Solicitante.Text = Dt_Cuotas_Fijas.Rows[0][Cat_Pre_Casos_Especiales.Campo_Descripcion].ToString();
                    }
                    else
                    {
                        Txt_Inmueble.Text = Dt_Cuotas_Fijas.Rows[0][Cat_Pre_Casos_Especiales.Campo_Descripcion].ToString();
                    }

                    Txt_Plazo_Financiamiento.Text = Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Plazo_Financiamiento].ToString();
                    if (Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Cuota_Minima].ToString() != "")
                    {
                        Txt_Cuota_Minima_Anual.Text = Convert.ToDecimal(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Cuota_Minima]).ToString("##,###,##0.00");
                    }
                    if (Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija].ToString() != "")
                    {
                        //Txt_Cuota_Minima_Aplicar.Text = Convert.ToDecimal(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija]).ToString("##,###,##0.00");
                        Txt_Cuota_Minima_Aplicar.Text = (Convert.ToDecimal(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija]) - (Convert.ToDecimal(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total]) + Convert.ToDecimal(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total]))).ToString("##,###,##0.00");
                        Cuota_Minima_Aplicar = Convert.ToDecimal(Txt_Cuota_Minima_Aplicar.Text);
                    }
                    if (Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Exedente_Construccion].ToString() != "")
                    {
                        Txt_Excedente_Construccion.Text = Convert.ToDouble(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Exedente_Construccion]).ToString("##,###,##0.00");
                    }
                    if (Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Exedente_Valor].ToString() != "")
                    {
                        Txt_Excedente_Valor.Text = Convert.ToDouble(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Exedente_Valor]).ToString("##,###,##0.00");
                    }

                    if (Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total].ToString() != "")
                    {
                        Txt_Excedente_Construccion_Total.Text = Convert.ToDouble(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total]).ToString("##,###,##0.00");
                        Excedente_Construccion_Total = Convert.ToDecimal(Txt_Excedente_Construccion_Total.Text);
                    }
                    if (Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total].ToString() != "")
                    {
                        Txt_Excedente_Valor_Total.Text = Convert.ToDouble(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total]).ToString("##,###,##0.00");
                        Excedente_Valor_Total = Convert.ToDecimal(Txt_Excedente_Valor_Total.Text);
                    }

                    Txt_Total_Cuota_Fija.Text = (Cuota_Minima_Aplicar + Excedente_Construccion_Total + Excedente_Valor_Total).ToString("##,###,##0.00");
                    //if (Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija].ToString() != "")
                    //{
                    //    Txt_Total_Cuota_Fija.Text = Convert.ToDouble(Dt_Cuotas_Fijas.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija]).ToString("##,###,##0.00");
                    //}
                    Txt_Fundamento_Legal.Text = "ARTICULO " + Dt_Cuotas_Fijas.Rows[0][Cat_Pre_Casos_Especiales.Campo_Articulo].ToString() + " INCISO " + Dt_Cuotas_Fijas.Rows[0][Cat_Pre_Casos_Especiales.Campo_Inciso].ToString() + Dt_Cuotas_Fijas.Rows[0][Cat_Pre_Casos_Especiales.Campo_Observaciones].ToString();
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Cuota_Fija: " + Ex.Message);
        }
    }
    #endregion

    #region Metodos [Operacion Calcular_Excedentes,Calcular_Cuota]
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validar_Cuota_Minima
    ///DESCRIPCIÓN          : De acuerdo a la Tasa Minima y Anual coloca la Tasa Anual
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 05/Octubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Validar_Cuota_Minima()
    {
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Double Cuota_Anual = 0;
        Double Cuota_Minima = 0;
        Int16 Año_Periodo_Corriente = 0;
        String Periodo_Corriente = "";

        if (HttpUtility.HtmlDecode(Txt_Cuota_Anual.Text).Trim() != "")
        {
            Cuota_Anual = Convert.ToDouble(HttpUtility.HtmlDecode(Txt_Cuota_Anual.Text).Trim());
        }
        else
        {
            Txt_Cuota_Anual.Text = "0.00";
        }

        Periodo_Corriente = HttpUtility.HtmlDecode(Txt_Periodo_Corriente.Text).Trim();
        if (Periodo_Corriente != "")
        {
            if (Periodo_Corriente.Length >= 4)
            {
                Int16.TryParse(Periodo_Corriente.Substring(Periodo_Corriente.Length - 4), out Año_Periodo_Corriente);
                Cuota_Minima = (Double)Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo_Corriente.ToString());
            }
        }
        if (Cuota_Anual < Cuota_Minima)
        {
            Txt_Cuota_Anual.Text = Cuota_Minima.ToString("##,###,##0.00");
        }
        if (Txt_Cuota_Anual.Text.Trim() != "")
        {
            Txt_Cuota_Bimestral.Text = (Convert.ToDecimal(Txt_Cuota_Anual.Text.Trim()) / 6).ToString("##,###,##0.00");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Cuota
    ///DESCRIPCIÓN          : desacuerdo a la tasa seleccionada calcula la cuota anual y bimestral
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 08/06/2011 05:25:43 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Calcular_Cuota()
    {
        try
        {
            Double Dbl_Couta = 0;
            Double Dbl_Valor_Fiscal = 0;
            Double Dbl_Factor = 0;

            if (Txt_Valor_Fiscal.Text.Trim() != "" && Txt_Tasa_Porcentaje.Text.Trim() != "")
            {
                if (Txt_Cuota_Anual.Text.Trim() != "")
                {
                    if (Txt_Tasa_Porcentaje.Text.Trim() != "")
                    {
                        Dbl_Factor = Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim()) / 1000;
                    }
                    if (Txt_Valor_Fiscal.Text.Trim() != "")
                    {
                        Dbl_Valor_Fiscal = Convert.ToDouble(Txt_Valor_Fiscal.Text.Trim());
                    }
                    Dbl_Couta = Dbl_Valor_Fiscal * Dbl_Factor;
                    if (Txt_Porcentaje_Exencion.Text.Trim() != "")
                    {
                        Dbl_Couta = Dbl_Couta - (Dbl_Couta * ((Convert.ToDouble(Txt_Porcentaje_Exencion.Text.Trim())) / 100));
                    }
                    Txt_Cuota_Anual.Text = Dbl_Couta.ToString("##,###,##0.00");

                    if (!String.IsNullOrEmpty(Txt_Cuota_Minima_Anual.Text))
                    {
                        if (Hdn_Cuota_Minima.Value.Trim() != "")
                        {
                            Dbl_Couta = Convert.ToDouble(Hdn_Cuota_Minima.Value);
                        }
                        if (Convert.ToDouble(Txt_Cuota_Anual.Text.Trim()) < Dbl_Couta)
                        {
                            Txt_Cuota_Anual.Text = Dbl_Couta.ToString("##,###,##0.00");
                        }
                    }

                    if (Txt_Cuota_Anual.Text.Trim() != "")
                    {
                        Txt_Cuota_Bimestral.Text = (Convert.ToDecimal(Txt_Cuota_Anual.Text.Trim()) / 6).ToString("##,###,##0.00");
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Calcular Cuota: " + Ex.Message);
        }


    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Excedentes
    ///DESCRIPCIÓN          : metodo para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Calcular_Excedentes()
    {
        Double Cuota_Minima = 0;
        Double Cuota_Minima_Aplicar = 0;
        Double Exedente_Construccion = 0;
        Double Excedente_Valor = 0;
        Double Total_Cuota_Fija = 0;
        try
        {
            //Consulta_Excedente_Valor();
            if (Txt_Tasa_Porcentaje.Text.Trim() != "")
            {
                if (Txt_Diferencia_Construccion.Text.Trim() != "" && Txt_Costo_M2.Text.Trim() != "" && Txt_Tasa_Excedente_Construccion.Text.Trim() != "")
                    Txt_Excedente_Construccion_Total.Text = (((Convert.ToDouble(Txt_Diferencia_Construccion.Text.Trim()) * Convert.ToDouble(Txt_Costo_M2.Text.Trim())) * (Convert.ToDouble(Txt_Tasa_Excedente_Construccion.Text.Trim()) / 1000)) * 6).ToString("##,###,##0.00");

                if (!String.IsNullOrEmpty(Hdn_Excedente_Valor.Value))
                {
                    if (Hdn_Excedente_Valor.Value == "0")
                        Txt_Excedente_Valor_Total.Text = "0.00";
                    else
                    {
                        if (Hdn_Excedente_Valor.Value.Trim() != "" && Txt_Tasa_Porcentaje.Text.Trim() != "")
                        {
                            Txt_Excedente_Valor_Total.Text = (((Convert.ToDouble(Hdn_Excedente_Valor.Value)) * ((Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim())) / 1000))).ToString("##,###,##0.00");
                        }
                    }
                }

                if (!String.IsNullOrEmpty(Txt_Cuota_Minima_Anual.Text.Trim()))
                {
                    if (Txt_Cuota_Minima_Anual.Text.Trim() != "")
                    {
                        Cuota_Minima = (Convert.ToDouble(Txt_Cuota_Minima_Anual.Text.Trim()));
                    }
                    if (Txt_Cuota_Minima_Aplicar.Text.Trim() != "")
                    {
                        Cuota_Minima_Aplicar = (Convert.ToDouble(Txt_Cuota_Minima_Aplicar.Text.Trim()));
                    }
                    if (Txt_Excedente_Construccion_Total.Text.Trim() != "")
                    {
                        Exedente_Construccion = (Convert.ToDouble(Txt_Excedente_Construccion_Total.Text.Trim()));
                    }
                    if (Txt_Excedente_Valor_Total.Text.Trim() != "")
                    {
                        Excedente_Valor = (Convert.ToDouble(Txt_Excedente_Valor_Total.Text.Trim()));
                    }
                    if (Exedente_Construccion > Excedente_Valor)
                    {
                        Total_Cuota_Fija = (Cuota_Minima_Aplicar + Exedente_Construccion);
                    }
                    else if (Excedente_Valor > Exedente_Construccion)
                    {
                        Total_Cuota_Fija = (Cuota_Minima_Aplicar + Excedente_Valor);
                    }
                    //else
                    //{
                    //    if (Txt_Cuota_Minima.Text.Trim() != "")
                    //    {
                    //        Total_Cuota_Fija = Convert.ToDouble(Txt_Cuota_Minima.Text) + Exedente_Construccion;
                    //    }
                    //}
                    Txt_Total_Cuota_Fija.Text = Total_Cuota_Fija.ToString("##,###,##0.00");
                }
            }
        }
        catch
        {
            //Mensaje_Error("Calcular Excedentes Error:[" + Ex.Message +"]");
        }
    }
    #endregion

    #region Eventos/Botones [Nuevo,Modificar,Salir,Busqueda]

    #endregion

    #region Metodos Reportes

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Data_Set_Consulta_Inventario.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte_Stock, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           17/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Ds_Reporte_Ordenes_Salida)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        try
        {
            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Orden_Variacion.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Orden_Variacion_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte_Ordenes_Salida, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
            Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    #endregion

    #region Eventos TxtChanged

    private void Cargar_Datos_Propietarios_Cuenta()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataSet Ds_Propietarios;
        try
        {
            Ordenes_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Ordenes_Variacion.P_Contrarecibo = Hdn_Contrarecibo.Value;
            Ds_Propietarios = Ordenes_Variacion.Consulta_Datos_Propietario();
            if (Ds_Propietarios != null)
            {
                if (Ds_Propietarios.Tables.Count > 0)
                {
                    if (Ds_Propietarios.Tables[0].Rows.Count > 0)
                    {
                        Hdn_Propietario_ID.Value = Ds_Propietarios.Tables[0].Rows[0]["CONTRIBUYENTE"].ToString();

                        Txt_Nombre_Propietario.Text = Ds_Propietarios.Tables[0].Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                        if (Ds_Propietarios.Tables[0].Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                        {
                            Txt_Tipo_Propietario.Text = Ds_Propietarios.Tables[0].Rows[0]["TIPO_PROPIETARIO"].ToString();
                        }

                        Txt_RFC_Propietario.Text = Ds_Propietarios.Tables[0].Rows[0]["RFC"].ToString();

                        Session.Remove("Ds_Prop_Datos");
                        Session["Ds_Prop_Datos"] = Ds_Propietarios;
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Chk_Cuota_Fija_CheckedChanged
    ///DESCRIPCIÓN: mostrar los detalles de la cuota fija
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Ago/2011 02:52:21 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Chk_Cuota_Fija_CheckedChanged(object sender, EventArgs e)
    {
        Div_Detalles_Cuota_Fija.Style.Value = "display:normal;";
        if (!Chk_Cuota_Fija.Checked)
        {
            Div_Detalles_Cuota_Fija.Style.Value = "display:none;";
        }
    }


    #endregion

    #region Eventos Grid
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Copropietarios_Cuenta
    ///DESCRIPCIÓN: Cargar datos de copropietarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:48:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Grid_Copropietarios_Cuenta(int Page_Index)
    {
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            DataTable Dt_Agregar_Copropietarios;

            Ordenes_Variacion.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Ordenes_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Ordenes_Variacion.P_Orden_Variacion_ID = Hdn_No_Orden_Variacion.Value;
            Ordenes_Variacion.P_Año = Año_Orden_Variacion;
            Ordenes_Variacion.P_Dt_Copropietarios = Ordenes_Variacion.Consulta_Co_Propietarios();
            Dt_Agregar_Copropietarios = Ordenes_Variacion.P_Dt_Copropietarios;

            Grid_Copropietarios.DataSource = null;
            Grid_Copropietarios.PageIndex = Page_Index;
            Grid_Copropietarios.DataSource = Dt_Agregar_Copropietarios;
            Grid_Copropietarios.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Copropietarios_Pageindexchanging
    ///DESCRIPCIÓN: paginar grid de propietarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:49:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Copropietarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Variacion_Copropietarios(e.NewPageIndex);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Diferencias_PageIndexChanging
    ///DESCRIPCIÓN          : paginar grid de Diferencias
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 04/Septiembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Diferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Variacion_Diferencias(e.NewPageIndex);
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Variacion_Diferencias
    ///DESCRIPCIÓN          : Carga el Grid con los datos de la Variación de las Diferencias
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 04/Septiembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Cargar_Grid_Variacion_Diferencias(int Page_Index)
    {
        Boolean Diferencias_Cargadas = false;
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Generar_Orden_No_Orden = Hdn_No_Orden_Variacion.Value;
            Orden_Variacion.P_Generar_Orden_Anio = Año_Orden_Variacion.ToString();
            Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();
            Grid_Diferencias.DataSource = Dt_Agregar_Diferencias;
            Grid_Diferencias.PageIndex = Page_Index;
            Grid_Diferencias.DataBind();

            if (Dt_Agregar_Diferencias != null)
            {
                if (Dt_Agregar_Diferencias.Rows.Count > 0)
                {
                    Diferencias_Cargadas = true;
                }
            }

            String Periodo = "";
            int Desde_Bimestre_Corriente = 6;
            int Hasta_Bimestre_Corriente = 1;
            int Desde_Bimestre_Rezago = 6;
            int Hasta_Bimestre_Rezago = 1;
            int Desde_Año_Corriente = DateTime.Now.Year + 1;
            int Hasta_Año_Corriente = 0;
            int Desde_Año_Rezago = DateTime.Now.Year + 1;
            int Hasta_Año_Rezago = 0;
            Boolean Periodo_Corriente_Validado = false;
            Boolean Periodo_Rezago_Validado = false;
            int Cont_Periodos_Corriente = 0;
            int Cont_Periodos_Rezago = 0;
            Double Suma_Importes_Periodos_Corriente_Alta = 0;
            Double Suma_Importes_Periodos_Corriente_Baja = 0;
            Double Suma_Importes_Periodos_Rezago_Alta = 0;
            Double Suma_Importes_Periodos_Rezago_Baja = 0;

            foreach (GridViewRow Fila_Grid in Grid_Diferencias.Rows)
            {
                Periodo = Obtener_Periodos_Bimestre(Fila_Grid.Cells[0].Text.Trim(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
                if (Grid_Diferencias.DataKeys[Fila_Grid.RowIndex].Values[0].ToString() == "CORRIENTE")
                {
                    Periodo_Corriente_Validado = true;
                    Periodo_Rezago_Validado = false;
                }
                else
                {
                    if (Grid_Diferencias.DataKeys[Fila_Grid.RowIndex].Values[0].ToString() == "REZAGO")
                    {
                        Periodo_Corriente_Validado = false;
                        Periodo_Rezago_Validado = true;
                    }
                }
                if (Periodo_Rezago_Validado)
                {
                    if (Periodo.Trim() != "")
                    {
                        if (Convert.ToInt32(Fila_Grid.Cells[0].Text.Split('-').GetValue(0).ToString().Split('/').GetValue(1).ToString().Trim()) <= Desde_Año_Rezago)
                        {
                            if (Convert.ToInt32(Fila_Grid.Cells[0].Text.Split('-').GetValue(0).ToString().Split('/').GetValue(1).ToString().Trim()) != Desde_Año_Rezago)
                            {
                                Desde_Bimestre_Rezago = 6;
                            }
                            Desde_Año_Rezago = Convert.ToInt32(Fila_Grid.Cells[0].Text.Split('-').GetValue(0).ToString().Split('/').GetValue(1).ToString().Trim());
                            if (Convert.ToInt32(Periodo.Split('-').GetValue(0)) < Desde_Bimestre_Rezago)
                            {
                                Desde_Bimestre_Rezago = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                            }
                        }
                        if (Convert.ToInt32(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Split('/').GetValue(1).ToString().Trim()) >= Hasta_Año_Rezago)
                        {
                            if (Convert.ToInt32(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Split('/').GetValue(1).ToString().Trim()) != Hasta_Año_Rezago)
                            {
                                Hasta_Bimestre_Rezago = 1;
                            }
                            Hasta_Año_Rezago = Convert.ToInt32(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Split('/').GetValue(1).ToString().Trim());
                            if (Convert.ToInt32(Periodo.Split('-').GetValue(1)) > Hasta_Bimestre_Rezago)
                            {
                                Hasta_Bimestre_Rezago = Convert.ToInt32(Periodo.Split('-').GetValue(1));
                            }
                        }
                        Cont_Periodos_Rezago++;
                    }
                    if (Fila_Grid.Cells[2].Text.Trim() != "")
                    {
                        if (Fila_Grid.Cells[2].Text.Trim() == "ALTA")
                        {
                            Suma_Importes_Periodos_Rezago_Alta += Convert.ToDouble(Fila_Grid.Cells[7].Text.Replace("$", ""));
                        }
                        else
                        {
                            if (Fila_Grid.Cells[2].Text.Trim() == "BAJA")
                            {
                                Suma_Importes_Periodos_Rezago_Baja += Convert.ToDouble(Fila_Grid.Cells[7].Text.Replace("$", ""));
                            }
                        }
                    }
                }
                if (Periodo_Corriente_Validado)
                {
                    if (Periodo.Trim() != "")
                    {
                        if (Fila_Grid.Cells[0].Text.Split('-').Length == 2)
                        {
                            if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Length - 4)) <= Desde_Año_Corriente)
                            {
                                if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Length - 4)) != Desde_Año_Corriente)
                                {
                                    Desde_Bimestre_Corriente = 6;
                                }
                                Desde_Año_Corriente = Convert.ToInt16(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Length - 4));
                                if (Convert.ToInt32(Periodo.Split('-').GetValue(0)) < Desde_Bimestre_Corriente)
                                {
                                    Desde_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                                }
                            }
                            if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Length - 4)) >= Hasta_Año_Corriente)
                            {
                                if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Length - 4)) != Hasta_Año_Corriente)
                                {
                                    Hasta_Bimestre_Corriente = 1;
                                }
                                Hasta_Año_Corriente = Convert.ToInt16(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid.Cells[0].Text.Split('-').GetValue(1).ToString().Trim().Length - 4));
                                if (Convert.ToInt32(Periodo.Split('-').GetValue(1)) > Hasta_Bimestre_Corriente)
                                {
                                    Hasta_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(1));
                                }
                            }
                            Cont_Periodos_Corriente++;
                        }
                        else
                        {
                            if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Substring(Fila_Grid.Cells[0].Text.Length - 4)) <= Desde_Año_Corriente)
                            {
                                if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Substring(Fila_Grid.Cells[0].Text.Length - 4)) != Desde_Año_Corriente)
                                {
                                    Desde_Bimestre_Corriente = 6;
                                }
                                Desde_Año_Corriente = Convert.ToInt16(Fila_Grid.Cells[0].Text.Substring(Fila_Grid.Cells[0].Text.Length - 4));
                                if (Convert.ToInt32(Periodo.Split('-').GetValue(0)) < Desde_Bimestre_Corriente)
                                {
                                    Desde_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                                }
                            }
                            if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Substring(Fila_Grid.Cells[0].Text.Length - 4)) >= Hasta_Año_Corriente)
                            {
                                if (Convert.ToInt16(Fila_Grid.Cells[0].Text.Substring(Fila_Grid.Cells[0].Text.Length - 4)) != Hasta_Año_Corriente)
                                {
                                    Hasta_Bimestre_Corriente = 1;
                                }
                                Hasta_Año_Corriente = Convert.ToInt16(Fila_Grid.Cells[0].Text.Substring(Fila_Grid.Cells[0].Text.Length - 4));
                                if (Convert.ToInt32(Periodo.Split('-').GetValue(1)) > Hasta_Bimestre_Corriente)
                                {
                                    Hasta_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(1));
                                }
                            }
                            Cont_Periodos_Corriente++;
                        }
                    }
                    if (Fila_Grid.Cells[2].Text.Trim() != "")
                    {
                        if (Fila_Grid.Cells[2].Text.Trim() == "ALTA")
                        {
                            Suma_Importes_Periodos_Corriente_Alta += Convert.ToDouble(Fila_Grid.Cells[7].Text.Replace("$", ""));
                        }
                        else
                        {
                            if (Fila_Grid.Cells[2].Text.Trim() == "BAJA")
                            {
                                Suma_Importes_Periodos_Corriente_Baja += Convert.ToDouble(Fila_Grid.Cells[7].Text.Replace("$", ""));
                            }
                        }
                    }
                }
            }

            if (Cont_Periodos_Corriente > 0)
            {
                Txt_Desde_Periodo_Corriente.Text = Desde_Bimestre_Corriente.ToString() + "/" + Desde_Año_Corriente.ToString();
                Txt_Desde_Periodo_Corriente.Font.Bold = true;
                Lbl_Desde_Periodo_Corriente.Font.Bold = true;
                Txt_Hasta_Periodo_Corriente.Text = Hasta_Bimestre_Corriente.ToString() + "/" + Hasta_Año_Corriente.ToString();
                Txt_Hasta_Periodo_Corriente.Font.Bold = true;
                Lbl_Hasta_Periodo_Corriente.Font.Bold = true;
            }
            else
            {
                Txt_Desde_Periodo_Corriente.Text = "0/0000";
                Txt_Desde_Periodo_Corriente.Font.Bold = false;
                Lbl_Desde_Periodo_Corriente.Font.Bold = false;
                Txt_Hasta_Periodo_Corriente.Text = "0/0000";
                Txt_Hasta_Periodo_Corriente.Font.Bold = false;
                Lbl_Hasta_Periodo_Corriente.Font.Bold = false;
            }
            if (Cont_Periodos_Rezago > 0)
            {
                Txt_Desde_Periodo_Rezago.Text = Desde_Bimestre_Rezago.ToString() + "/" + Desde_Año_Rezago.ToString();
                Txt_Desde_Periodo_Rezago.Font.Bold = true;
                Lbl_Desde_Periodo_Rezago.Font.Bold = true;
                Txt_Hasta_Periodo_Rezago.Text = Hasta_Bimestre_Rezago.ToString() + "/" + Hasta_Año_Rezago.ToString();
                Txt_Hasta_Periodo_Rezago.Font.Bold = true;
                Lbl_Hasta_Periodo_Rezago.Font.Bold = true;
            }
            else
            {
                Txt_Desde_Periodo_Rezago.Text = "0/0000";
                Txt_Desde_Periodo_Rezago.Font.Bold = false;
                Lbl_Desde_Periodo_Rezago.Font.Bold = false;
                Txt_Hasta_Periodo_Rezago.Text = "0/0000";
                Txt_Hasta_Periodo_Rezago.Font.Bold = false;
                Lbl_Hasta_Periodo_Rezago.Font.Bold = false;
            }

            Txt_Alta_Periodo_Corriente.Text = Suma_Importes_Periodos_Corriente_Alta.ToString("##,###,##0.00");
            if (Suma_Importes_Periodos_Corriente_Alta != 0)
            {
                Txt_Alta_Periodo_Corriente.Font.Bold = true;
                Lbl_Alta_Periodo_Corriente.Font.Bold = true;
            }
            else
            {
                Txt_Alta_Periodo_Corriente.Font.Bold = false;
                Lbl_Alta_Periodo_Corriente.Font.Bold = false;
            }
            Txt_Baja_Periodo_Corriente.Text = Suma_Importes_Periodos_Corriente_Baja.ToString("##,###,##0.00");
            if (Suma_Importes_Periodos_Corriente_Baja != 0)
            {
                Txt_Baja_Periodo_Corriente.Font.Bold = true;
                Lbl_Baja_Periodo_Corriente.Font.Bold = true;
            }
            else
            {
                Txt_Baja_Periodo_Corriente.Font.Bold = false;
                Lbl_Baja_Periodo_Corriente.Font.Bold = false;
            }
            Txt_Alta_Periodo_Rezago.Text = Suma_Importes_Periodos_Rezago_Alta.ToString("##,###,##0.00");
            if (Suma_Importes_Periodos_Rezago_Alta != 0)
            {
                Txt_Alta_Periodo_Rezago.Font.Bold = true;
                Lbl_Alta_Periodo_Rezago.Font.Bold = true;
            }
            else
            {
                Txt_Alta_Periodo_Rezago.Font.Bold = false;
                Lbl_Alta_Periodo_Rezago.Font.Bold = false;
            }
            Txt_Baja_Periodo_Rezago.Text = Suma_Importes_Periodos_Rezago_Baja.ToString("##,###,##0.00");
            if (Suma_Importes_Periodos_Rezago_Baja != 0)
            {
                Txt_Baja_Periodo_Rezago.Font.Bold = true;
                Lbl_Baja_Periodo_Rezago.Font.Bold = true;
            }
            else
            {
                Txt_Baja_Periodo_Rezago.Font.Bold = false;
                Lbl_Baja_Periodo_Rezago.Font.Bold = false;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Diferencias_Cargadas;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Obtener_Periodos_Bimestre
    ///DESCRIPCIÓN          : Valida la cadena indicada para obtener los periodos de la Bimestres quitando los Años
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private String Obtener_Periodos_Bimestre(String Periodos, out Boolean Periodo_Corriente_Validado, out Boolean Periodo_Rezago_Validado)
    {
        String Periodo = "";
        int Indice = 0;
        Periodo_Corriente_Validado = false;
        Periodo_Rezago_Validado = false;

        if (Periodos.IndexOf("-") >= 0)
        {
            if (Periodos.Split('-').Length == 2)
            {
                //Valida el segundo nodo del arreglo
                if (Periodos.Split('-').GetValue(1).ToString().IndexOf("/") >= 0)
                {
                    Periodo = Periodos.Split('-').GetValue(0).ToString().Trim().Substring(0, 1);
                    Periodo += "-";
                    Periodo += Periodos.Split('-').GetValue(1).ToString().Trim().Substring(0, 1);
                    Periodo_Rezago_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Split('-').GetValue(0).ToString().Replace("/", "-").Trim();
                    Periodo_Corriente_Validado = true;
                }
            }
            else
            {
                if (Periodos.Contains("/"))
                {
                    Indice = Periodos.IndexOf("/");
                    Periodo = Periodos.Substring(Indice - 1, 1);
                    Periodo += "-";
                    Indice = Periodos.IndexOf("/", Indice + 1);
                    Periodo += Periodos.Substring(Indice - 1, 1);
                    Periodo_Rezago_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Substring(0, 3);
                    Periodo_Corriente_Validado = true;
                }
            }
        }
        else
        {
            if (Periodos.Trim().IndexOf(" ") >= 0)
            {
                if (Periodos.Split(' ').GetValue(0).ToString().Contains("/"))
                {
                    Periodo = Periodos.Split(' ').GetValue(0).ToString().Replace("/", "-").Trim();
                    Periodo_Corriente_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Substring(0, 3);
                    Periodo_Corriente_Validado = true;
                }
            }
        }
        return Periodo;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Orden_Variacion
    ///DESCRIPCIÓN          : Manejo de la consulta de las Órdenes de Variación creadas
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 10/Octubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Cargar_Datos_Cuenta_Orden_Variacion()
    {
        try
        {
            Limpiar_Todo();

            Hdn_No_Orden_Variacion.Value = No_Orden_Variacion;
            Hdn_Contrarecibo.Value = No_Contrarecibo;
            Txt_Cuenta_Predial.Text = Cuenta_Predial;
            Hdn_Cuenta_ID.Value = Cuenta_Predial_ID;

            //Cargar_Datos_Cuenta();

            if (Cargar_Variacion_Orden())
            {
                Obtener_Ultimo_Movimiento_Orden();
                //Validar_Mismo_Domicilio();
                Configurar_Estatus_Controles(true);
                Configurar_Edicion_Controles(false);
                Configurar_Controles_Validacion(false);
            }
            //else
            //{
            //    Mensaje_Error("No se encontraron datos de Variación");
            //    Limpiar_Todo();
            //}
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Metodos Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 22/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        String Mensaje_Error = "";
        Boolean Validacion = true;
        //if (Txt_Cuenta_Predial.Text.Trim().Equals(""))
        //{
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
        //    Validacion = false;
        //}
        if (Cmb_Estatus_Orden_Variacion.SelectedValue == "SELECCIONE")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique un Estatus para Validar.";
            Validacion = false;
        }
        if (Txt_Observaciones_Validacion.Text.Trim().Equals("") && Cmb_Estatus_Orden_Variacion.SelectedValue == "RECHAZADA")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique las Observaciones por las que Rechaza la Variación.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
        }
        //Lbl_Mensaje_Error.Visible = !Validacion;
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Campo;
            if (Tabla != "")
            {
                Mi_SQL += " FROM " + Tabla;
            }
            if (Condiciones != "")
            {
                Mi_SQL += " WHERE " + Condiciones;
            }

            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);

            if (Dr_Dato.Read())
            {
                if (Dr_Dato[0] != null)
                {
                    Dato_Consulta = Dr_Dato[0].ToString();
                }
                else
                {
                    Dato_Consulta = "";
                }
                Dr_Dato.Close();
            }
            else
            {
                Dato_Consulta = "";
            }
            if (Dr_Dato != null)
            {
                Dr_Dato.Close();
            }
            Dr_Dato = null;
        }
        catch
        {
        }
        finally
        {
        }

        return Dato_Consulta;
    }
    #endregion

    protected void Grid_Historial_Observaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Historial_Observaciones.SelectedIndex = (-1);
            Cargar_Grid_Historial_Observaciones(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
        }
    }

    protected void Grid_Copropietarios_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Grid_Copropietarios.DataKeys[e.Row.RowIndex].Values[1].ToString() == "NUEVO")
            {
                e.Row.Font.Bold = true;
            }
            else
            {
                e.Row.Font.Bold = false;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Ultimo_Movimiento_Orden
    ///DESCRIPCIÓN: consultar clave del ultimo movimiento de la cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Sep/2011 07:41:34 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Obtener_Ultimo_Movimiento_Orden()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Movimiento;
        try
        {
            Resumen_Predio.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Dt_Movimiento = Resumen_Predio.Consultar_Ultimo_Movimiento();
            if (Dt_Movimiento.Rows.Count > 0)
            {
                Txt_Ultimo_Movimiento.Text = Dt_Movimiento.Rows[0][0].ToString() + " - " + Dt_Movimiento.Rows[0][1].ToString();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta
    /////DESCRIPCIÓN          : Carga los datos de la Cuenta y la Variación en base a la Cuenta Predial consultada.
    /////PARAMETROS:     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 07/Octubre/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //private void Consultar_Datos_Cuenta()
    //{
    //    Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    //    DataSet Ds_Cuenta = null;

    //    if (Txt_Cuenta_Predial.Text.Trim() != "")
    //    {
    //        Ordenes_Variacion.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
    //        Ds_Cuenta = Ordenes_Variacion.Consulta_Datos_Cuenta();
    //        if (Ds_Cuenta == null)
    //        {
    //            Ds_Cuenta = Ordenes_Variacion.Consulta_Datos_Cuenta_Sin_Contrarecibo();
    //        }
    //        if (Ds_Cuenta.Tables[0] != null)
    //        {
    //            Ds_Cuenta = Ordenes_Variacion.Consulta_Datos_Cuenta_Sin_Contrarecibo();
    //        }
    //        if (Ds_Cuenta.Tables[0].Rows.Count == 0)
    //        {
    //            Ds_Cuenta = Ordenes_Variacion.Consulta_Datos_Cuenta_Sin_Contrarecibo();
    //        }
    //        if (Ds_Cuenta != null)
    //        {
    //            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
    //            {
    //                Cargar_Datos_Cuenta(Ds_Cuenta.Tables[0]);
    //            }
    //        }
    //        if (Cargar_Variacion())
    //        {
    //            Obtener_Ultimo_Movimiento_Orden();
    //            Validar_Mismo_Domicilio();
    //            Configurar_Estatus_Controles(true);
    //            Configurar_Edicion_Controles(false);
    //            Configurar_Controles_Validacion(false);
    //        }
    //        else
    //        {
    //            Mensaje_Error("No se encontraron datos de Variación");
    //            Limpiar_Todo();
    //        }
    //    }
    //}

    private void Limpiar_Sesiones_Pagina()
    {
        Session.Remove("ESTATUS_CUENTAS");
        Session.Remove("TIPO_CONTRIBUYENTE");
        Session.Remove("Cuenta_Predial_ID");
        Session.Remove("Orden_Variacion_ID_Adeudos");
        Session.Remove("Cuenta_Predial_ID_Adeudos");
        Session.Remove("Cuenta_Predial");
        Session.Remove("Ds_Prop_Datos");
        Session.Remove("Dt_Agregar_Diferencias");
    }
}
