using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using SharpContent.ApplicationBlocks.Data;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Descuentos_Predial.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Catalogo_Descuentos_Predial.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Descuentos_Predial : System.Web.UI.Page
{

    #region Variables

    private Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();

    #endregion

    #region METODOS

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Inicializa_Controles
    ///DESCRIPCIÓN: Inicializa los controles en la forma
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 12/dic/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Inicializa_Controles()
    {
        Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
        Limpiar_Controles_Datos_Generales(); //Limpia los controles del forma
        Cargar_Grid_Descuentos(0);
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION: Limpia los controles que se encuentran en la forma
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-dic-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles_Datos_Generales()
    {
        Hdn_No_Orden_Variacion.Value = null;
        Hdn_Contrarecibo.Value = null;
        Hdn_Cuenta_ID.Value = null;
        Hdn_Propietario_ID.Value = null;
        Hdn_Busqueda.Value = "";
        Hdn_Contribuyente_ID.Value = null;
        Hdn_No_Descuento.Value = null;
        Hdn_Nuevo_Contribuyente_ID.Value = null;
        Lbl_Mensaje_Cuenta_Predial.Text = "";

        // QUITA LOS TEXTOS
        // Generales
        Txt_Cuenta_Predial.Text = "";
        Txt_Nombre_Propietario.Text = "";
        Txt_Ubicacion.Text = "";
        Txt_Busqueda.Text = "";

        // Resumen de adeudos
        Limpiar_Cantidades();
        Txt_Periodo_Inicial.Text = "";
        Cmb_Hasta_Anio_Periodo.Items.Clear();

        // Limpiar grid de adeudos
        Grid_Adeudos.DataSource = null;
        Grid_Adeudos.DataBind();
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Cantidades
    /// DESCRIPCION: Limpia los controles de montos que se encuentran en la forma
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-dic-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Cantidades()
    {
        Txt_Adeudo_Corriente.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Recargos_Ordinarios.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Total_Adeudo.Text = "";
        Txt_Total_Pagar.Text = "";
        Txt_Total_Descuento.Text = "";
        Txt_Monto_Descuento_Recargos.Text = "";
        Txt_Porciento_Descuento_Recargos.Text = "";
        Txt_Monto_Descuento_Moratorios.Text = "";
        Txt_Porciento_Descuento_Moratorios.Text = "";
        Txt_Monto_Descuento_Pronto_Pago.Text = "";
        Txt_Porciento_Descuento_Pronto_Pago.Text = "";
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Obtener_Descuento_Pronto_Pago
    /// DESCRIPCION: Obtiene el descuento por pronto pago para la cuenta predial
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 28-feb-2012
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Obtener_Descuento_Pronto_Pago()
    {
        var Cuotas_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        var Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        var Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        var Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        var Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();

        Int32 Anio_Corriente = 0;
        DataTable Dt_Cuotas_Minimas;
        DataTable Dt_Pagos;
        DataTable Dt_Datos_Cuenta;
        decimal Porcentaje_Descuento;
        decimal Descuento_Pronto_Pago;
        decimal Adeudo_Corriente;
        decimal Cuota_Minima = 0;

        // obtener año corriente
        Anio_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();
        // verificar que se obtuvo valor mayor que cero, si no, tomar año actual
        if (Anio_Corriente <= 0)
        {
            Anio_Corriente = DateTime.Now.Year;
        }

        // adeudos
        decimal.TryParse(Txt_Adeudo_Corriente.Text.Replace("$", "").Replace(",", ""), out Adeudo_Corriente);

        // obtener cuota minima
        Cuotas_Minima.P_Anio = DateTime.Now.Year.ToString();
        Dt_Cuotas_Minimas = Cuotas_Minima.Consultar_Cuotas_Minimas();
        if (Dt_Cuotas_Minimas != null && Dt_Cuotas_Minimas.Rows.Count > 0)
        {
            Decimal.TryParse(Dt_Cuotas_Minimas.Rows[0]["CUOTA"].ToString(), out Cuota_Minima);
        }

        // consultar pagos de la cuenta
        Consultar_Pagos.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
        Consultar_Pagos.p_Estatus = "PAGADO";
        Consultar_Pagos.p_Periodo_Corriente = " LIKE '%" + Anio_Corriente + "%'";
        Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Predial_Por_Periodo();

        // consultar si la cuenta tiene beneficio
        string Cuota_Fija = "";
        Cuenta_Predial.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
        Dt_Datos_Cuenta = Cuenta_Predial.Consultar_Cuenta();
        if (Dt_Datos_Cuenta != null && Dt_Datos_Cuenta.Rows.Count > 0)
        {
            Cuota_Fija = Dt_Datos_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString().Trim().ToUpper();
        }
        // solo si el adeudo consultado es hasta el sexto bimestre del año corriente, calcular descuento por pronto pago (no debe tener cuota fija)
        if (Cmb_Hasta_Bimestre_Periodo.SelectedValue.Contains("6") && (Dt_Pagos == null || Dt_Pagos.Rows.Count == 0) && !Cuota_Fija.Contains("SI"))
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
                Txt_Porciento_Descuento_Pronto_Pago.Text = Porcentaje_Descuento.ToString("0.00");
                Txt_Monto_Descuento_Pronto_Pago.Text = Descuento_Pronto_Pago.ToString("#,##0.00");
            }
        }
        else
        {
            Txt_Porciento_Descuento_Pronto_Pago.Text = "0.00";
            Txt_Monto_Descuento_Pronto_Pago.Text = "$ 0.00";
        }
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error o si el parametro es nulo, limpia el mensaje de error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        if (!string.IsNullOrEmpty(P_Mensaje))
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
        }
        else
        {
            Img_Error.Visible = false;
            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Lbl_Encabezado_Error.Text = "";
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Descuento
    /// DESCRIPCIÓN: Consulta el descuento para el mes actiual y lo regresa como decimal
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 29-feb-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Consultar_Descuento()
    {
        var Consultar_Descuento = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Descuentos;
        decimal Descuento = 0;

        try
        {
            // obtener descuento del mes actual
            Dt_Descuentos = Consultar_Descuento.Consultar_Descuentos_Pronto_Pago();
            if (Dt_Descuentos != null && Dt_Descuentos.Rows.Count > 0)
            {
                decimal.TryParse(
                    Dt_Descuentos.Rows[0][DateTime.Now.ToString("MMMM")].ToString().Trim(),
                    out Descuento);
            }

            return Descuento;
        }
        catch
        {
            throw new Exception("Error al intentar consultar el descuento por pronto pago para el mes actual.");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma para según se requiera 
    ///             para la siguiente operación
    /// PARÁMETROS:
    ///         1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 	             (inicial, nuevo, modificar)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va a ser habilitado para que los edite el usuario
        decimal Descuento;

        try
        {
            Descuento = Consultar_Descuento();

            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Imprimir.Visible = true;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Imprimir.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Imprimir.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    break;
            }

            Pnl_Descuentos.Visible = Habilitado;
            Grid_Descuentos_Predial.Visible = !Habilitado; ;

            // Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            //Generales
            Txt_Cuenta_Predial.Enabled = false;
            Txt_Ubicacion.Enabled = false;
            Txt_Nombre_Propietario.Enabled = false;
            Txt_Periodo_Inicial.Enabled = false;
            Cmb_Hasta_Bimestre_Periodo.Enabled = Habilitado;
            Cmb_Hasta_Anio_Periodo.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;

            Btn_Fecha_Vencimiento.Enabled = Habilitado;
            Txt_Fecha_Vencimiento.Enabled = Habilitado;
            Txt_Realizo.Enabled = Habilitado;
            Txt_Observaciones.Enabled = Habilitado;

            Txt_Realizo.Enabled = false;

            Txt_Adeudo_Corriente.Enabled = false;
            Txt_Adeudo_Rezago.Enabled = false;
            Txt_Recargos_Ordinarios.Enabled = false;
            Txt_Recargos_Moratorios.Enabled = false;
            Txt_Honorarios.Enabled = false;
            Txt_Porciento_Descuento_Pronto_Pago.Enabled = false;
            Txt_Monto_Descuento_Pronto_Pago.Enabled = false;
            tr_descuento_pronto_pago.Visible = Descuento > 0 ? true : false;

            Txt_Porciento_Descuento_Recargos.Enabled = Habilitado;
            Txt_Porciento_Descuento_Moratorios.Enabled = Habilitado;
            Txt_Monto_Descuento_Recargos.Enabled = Habilitado;
            Txt_Monto_Descuento_Moratorios.Enabled = Habilitado;
            Txt_Total_Adeudo.Enabled = false;
            Txt_Total_Descuento.Enabled = false;
            Txt_Total_Pagar.Enabled = false;

            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: asignar datos de cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Cargar_Datos()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Cuenta;
        String Domicilio = "";
        String Calle = "";
        String Colonia = "";
        String Numero_Interior = "";
        String Numero_Exterior = "";
        String Estatus = "";
        String Contribuyente_ID = "";

        Boolean Datos_Cargados = false;
        try
        {
            Cuenta_Predial.P_Incluir_Campos_Foraneos = true;
            Cuenta_Predial.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Dt_Cuenta = Cuenta_Predial.Consultar_Cuenta();

            Calle = Dt_Cuenta.Rows[0]["NOMBRE_CALLE"].ToString().Trim();
            Colonia = Dt_Cuenta.Rows[0]["NOMBRE_COLONIA"].ToString().Trim();
            Numero_Exterior = Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim();
            Numero_Interior = Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim();
            Estatus = Dt_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString().ToUpper();
            Contribuyente_ID = Dt_Cuenta.Rows[0]["PROPIETARIO_ID"].ToString().Trim();
            // se se modifica el descuento, asignar el nuevo id de propietario
            Hdn_Nuevo_Contribuyente_ID.Value = Contribuyente_ID;

            Obtener_Descuento_Pronto_Pago();

            // mostrar mensaje si la cuenta tiene estatus PENDIENTE
            if (Estatus != "PENDIENTE")
            {
                Lbl_Mensaje_Cuenta_Predial.Text = "";
            }
            else
            {
                Lbl_Mensaje_Cuenta_Predial.Text = "Cuenta predial con estatus: PENDIENTE";
            }

            Domicilio = Calle + " " + Colonia + " " + Numero_Exterior;
            if (!string.IsNullOrEmpty(Numero_Exterior))
            {
                Domicilio += " Int. " + Numero_Interior;
            }

            if (Domicilio.Length > 3)
            {
                Txt_Ubicacion.Text = Domicilio;
                Txt_Nombre_Propietario.Text = Dt_Cuenta.Rows[0]["NOMBRE_PROPIETARIO"].ToString().Trim();
                Hdn_Contribuyente_ID.Value = Contribuyente_ID;
            }
            else
            {
                Cargar_Datos_Cuenta_Variacion();
            }

        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Datos_Cargados;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Datos_Cuenta_Variacion
    ///DESCRIPCIÓN          : Consulta los datos de la Orden de Variacción
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 14-dic-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Datos_Cuenta_Variacion()
    {
        var Consultar_Orden_Variacion = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        DataTable Dt_Orden_Variacion;
        DataTable Dt_Ordenes_Variacion_Aceptadas;

        String Domicilio = "";
        String Calle = "";
        String Colonia = "";
        String Numero_Interior = "";
        String Numero_Exterior = "";

        // verificar que hay ordenes de variacion aceptadas
        if (Session["Dt_Ordenes_Variacion_Aceptadas"] != null)
        {
            Dt_Ordenes_Variacion_Aceptadas = (DataTable)Session["Dt_Ordenes_Variacion_Aceptadas"];

            if (Dt_Ordenes_Variacion_Aceptadas.Rows.Count > 0)
            {
                int Anio_Orden;
                int.TryParse(Dt_Ordenes_Variacion_Aceptadas.Rows[0]["Anio"].ToString().Trim(), out Anio_Orden);
                Consultar_Orden_Variacion.P_No_Orden_Variacion =
                    Dt_Ordenes_Variacion_Aceptadas.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                Consultar_Orden_Variacion.P_Anio_Orden = Anio_Orden;
                Consultar_Orden_Variacion.P_Incluir_Campos_Foraneos = true;
                Dt_Orden_Variacion = Consultar_Orden_Variacion.Consultar_Ordenes_Variacion();

                if (Dt_Orden_Variacion != null && Dt_Orden_Variacion.Rows.Count > 0)
                {
                    Calle = Dt_Orden_Variacion.Rows[0]["NOMBRE_CALLE_UBICACION"].ToString().Trim();
                    Colonia = Dt_Orden_Variacion.Rows[0]["NOMBRE_COLONIA_UBICACION"].ToString().Trim();
                    Numero_Exterior =
                        Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString().Trim();
                    Numero_Interior =
                        Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString().Trim();

                    Hdn_Propietario_ID.Value =
                        Dt_Orden_Variacion.Rows[0][Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString().Trim();
                    Txt_Nombre_Propietario.Text = Dt_Orden_Variacion.Rows[0]["NOMBRE_PROPIETARIO"].ToString().Trim();

                    Domicilio = Calle + " " + Numero_Exterior;
                    if (!string.IsNullOrEmpty(Numero_Exterior))
                    {
                        Domicilio += " Int. " + Numero_Interior;
                    }
                    Domicilio += Colonia;
                    if (Domicilio.Length > 3)
                    {
                        Txt_Ubicacion.Text = Domicilio;
                    }
                }
            }
            else
            {
                if (Lbl_Mensaje_Cuenta_Predial.Text.Contains("PENDIENTE"))
                {
                    Lbl_Mensaje_Cuenta_Predial.Text += ". La cuenta aún no tiene órdenes de variación ACEPTADAS.";
                }
            }
        }
        else
        {
            if (Lbl_Mensaje_Cuenta_Predial.Text.Contains("PENDIENTE"))
            {
                Lbl_Mensaje_Cuenta_Predial.Text += ". La cuenta aun no tiene órdenes de varaición ACEPTADAS.";
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Descuentos
    ///DESCRIPCIÓN          : Llena el grid descuentos con los registros encontrados
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Descuentos(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Descuentos_Predial_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Predial_Negocio();
            DataTable Dt_Descuentos;

            Descuentos.P_Campos_Foraneos = true;
            if (Hdn_Busqueda.Value != "")
            {
                Descuentos.P_Cuenta_Predial = Hdn_Busqueda.Value.ToUpper();
            }
            else
            {
                Descuentos.P_Estatus = "VIGENTE";
            }
            Dt_Descuentos = Descuentos.Consultar_Descuentos_Predial_Busqueda();

            if (Dt_Descuentos != null)
            {
                Grid_Descuentos_Predial.Columns[1].Visible = true;
                Grid_Descuentos_Predial.Columns[2].Visible = true;
                Grid_Descuentos_Predial.DataSource = Dt_Descuentos;
                Grid_Descuentos_Predial.PageIndex = Pagina;
                Grid_Descuentos_Predial.DataBind();
                Grid_Descuentos_Predial.Columns[1].Visible = false;
                Grid_Descuentos_Predial.Columns[2].Visible = false;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Grid_Descuentos: " + Ex.Message);
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Descuentos_Predial_SelectedIndexChanged
    /////DESCRIPCIÓN: Manejar el evento de cambio de elemento seleccionado del grid  
    /////           para mostrar los detalles del descuento seleccionado
    /////PROPIEDADES:     
    /////CREO: Roberto González Oseguera
    /////FECHA_CREO: 10/dic/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    protected void Grid_Descuentos_Predial_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Descuentos_Predial.SelectedIndex > (-1))
            {
                Hdn_Cuenta_ID.Value = Grid_Descuentos_Predial.SelectedRow.Cells[2].Text;
                Session["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;
                Hdn_No_Descuento.Value = Grid_Descuentos_Predial.SelectedRow.Cells[1].Text;
                Cargar_Descuento();
                Pnl_Descuentos.Visible = true;
                // ocultar grid y mostrar boton modificar e imprimir
                Grid_Descuentos_Predial.Visible = false;
                Btn_Modificar.Visible = true;
                Btn_Imprimir.Visible = true;
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            }
        }

        catch (Exception Ex)
        {
            Mensaje_Error("Grid_Descuentos_Predial_SelectedIndexChanged " + Ex.Message);
        }
    }

    private void Cargar_Datos_Descuento(String Cuenta_Predial_ID, String No_Descuento)
    {
        Hdn_Cuenta_ID.Value = Cuenta_Predial_ID;
        Session["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;
        //Hdn_No_Descuento.Value = No_Descuento;
        Cargar_Descuento();
        Pnl_Descuentos.Visible = true;
        // ocultar grid y mostrar boton modificar e imprimir
        Grid_Descuentos_Predial.Visible = false;
        Btn_Modificar.Visible = true;
        Btn_Imprimir.Visible = true;
        Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Descuentos_Predial_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Descuentos
    ///PROPIEDADES:     
    ///CREO: Roberto González Oseguera
    /////FECHA_CREO: 10/dic/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Descuentos_Predial_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Descuentos_Predial.SelectedIndex = (-1);
            Cargar_Grid_Descuentos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #region Metodos Reportes

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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Descuentos_Predial
    ///DESCRIPCIÓN          : Crea un Dataset con los datos para imprimir el reporte
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 13-dic-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Descuentos_Predial()
    {
        Ds_Ope_Pre_Descuentos_Predial Ds_Descuentos_Predial = new Ds_Ope_Pre_Descuentos_Predial();
        DataRow Dr_Descuentos_Predial;
        decimal Descuento_Pronto_Pago;

        Descuento_Pronto_Pago = Consultar_Descuento();

        foreach (DataTable Dt_Descuentos_Predial in Ds_Descuentos_Predial.Tables)
        {
            if (Dt_Descuentos_Predial.TableName == "Dt_Descuentos_Predial")
            {
                //Inserta los datos
                Dr_Descuentos_Predial = Dt_Descuentos_Predial.NewRow();
                Dr_Descuentos_Predial["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text.Trim();
                Dr_Descuentos_Predial["CUENTA_PREDIAL_ID"] = Hdn_Cuenta_ID.Value;
                Dr_Descuentos_Predial["PROPIETARIO"] = Txt_Nombre_Propietario.Text.Trim();
                Dr_Descuentos_Predial["UBICACION"] = Txt_Ubicacion.Text.Trim();
                Dr_Descuentos_Predial["COLONIA"] = "";
                Dr_Descuentos_Predial["NO_EXTERIOR"] = "";
                Dr_Descuentos_Predial["NO_INTERIOR"] = "";
                Dr_Descuentos_Predial["PERIODO"] = Txt_Periodo_Inicial.Text + " - "
                    + Cmb_Hasta_Bimestre_Periodo.SelectedItem + "/" + Cmb_Hasta_Anio_Periodo.SelectedValue;
                Dr_Descuentos_Predial["IMPUESTO_CORRIENTE"] = Txt_Adeudo_Corriente.Text.Replace("$", "");
                Dr_Descuentos_Predial["IMPUESTO_REZAGO"] = Txt_Adeudo_Rezago.Text.Replace("$", "");
                Dr_Descuentos_Predial["TOTAL_IMPUESTO"] = Txt_Total_Adeudo.Text.Replace("$", "");
                Dr_Descuentos_Predial["RECARGOS"] = Txt_Recargos_Ordinarios.Text.Replace("$", "");
                Dr_Descuentos_Predial["MORATORIOS"] = Txt_Recargos_Moratorios.Text.Replace("$", "");
                Dr_Descuentos_Predial["HONORARIOS"] = Txt_Honorarios.Text.Replace("$", "");
                Dr_Descuentos_Predial["PORCENTAJE_RECARGOS"] = Txt_Porciento_Descuento_Recargos.Text.Trim();
                Dr_Descuentos_Predial["DESCUENTO_RECARGOS"] = Txt_Monto_Descuento_Recargos.Text.Replace("$", "");
                Dr_Descuentos_Predial["PORCENTAJE_MORATORIOS"] = Txt_Porciento_Descuento_Moratorios.Text.Trim();
                Dr_Descuentos_Predial["DESCUENTO_MORATORIOS"] = Txt_Monto_Descuento_Moratorios.Text.Replace("$", "");
                Dr_Descuentos_Predial["PORCENTAJE_DESCUENTO_PRONTO_PAGO"] = Txt_Porciento_Descuento_Pronto_Pago.Text.Trim();
                // agregar monto descuento o si el mes actual no tiene, asignar leyenda INVISIBLE a DESCUENTO_PRONTO_PAGO para que no se muestre en el reporte
                if (Descuento_Pronto_Pago > 0)
                {
                    Dr_Descuentos_Predial["DESCUENTO_PRONTO_PAGO"] = Txt_Monto_Descuento_Pronto_Pago.Text.Trim();
                }
                else
                {
                    Dr_Descuentos_Predial["DESCUENTO_PRONTO_PAGO"] = "INVISIBLE";
                }
                Dr_Descuentos_Predial["TOTAL_PAGAR"] = Txt_Total_Pagar.Text.Replace("$", "");
                Dr_Descuentos_Predial["REALIZO"] = Txt_Realizo.Text.Trim();

                Dr_Descuentos_Predial["FECHA_VENCIMIENTO"] = Txt_Fecha_Vencimiento.Text;
                Dr_Descuentos_Predial["OBSERVACIONES"] = Txt_Observaciones.Text.Trim();
                Dt_Descuentos_Predial.Rows.Add(Dr_Descuentos_Predial);
            }
        }
        return Ds_Descuentos_Predial;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Liquidacion: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-oct-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Liquidacion, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        if (Txt_Porciento_Descuento_Moratorios.Text.Trim() == "")
        {
            Txt_Porciento_Descuento_Moratorios.Text = "0";
            Txt_Monto_Descuento_Moratorios.Text = "0.00";
        }

        try
        {
            Reporte.Load(Ruta_Archivo);
            Reporte.SetDataSource(Ds_Liquidacion);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String PDF_Formato = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Formato);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(PDF_Formato, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    #endregion

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Totales
    /// DESCRIPCIÓN: Calcular el total descuento y total a pagar 
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Totales()
    {
        Decimal Adeudo_Corriente = 0;
        Decimal Adeudo_Rezago = 0;
        Decimal Adeudo_Recargos = 0;
        Decimal Adeudo_Moratorios = 0;
        Decimal Honorarios = 0;
        Decimal Descuento_Recargos = 0;
        Decimal Descuento_Moratorios = 0;
        Decimal Descuento_Pronto_Pago = 0;
        Decimal Porcentaje_Descuento_Recargos = 0;
        Decimal Porcentaje_Descuento_Moratorios = 0;
        Decimal Porcentaje_Descuento_Pronto_Pago = 0;
        Decimal Total_Adeudo = 0;
        Decimal Total_Descuento = 0;
        Decimal Total_A_Pagar = 0;

        // obtener montos de las cajas de texto
        Decimal.TryParse(Txt_Adeudo_Corriente.Text, out Adeudo_Corriente);
        Decimal.TryParse(Txt_Adeudo_Rezago.Text, out Adeudo_Rezago);
        Decimal.TryParse(Txt_Recargos_Ordinarios.Text, out Adeudo_Recargos);
        Decimal.TryParse(Txt_Recargos_Moratorios.Text, out Adeudo_Moratorios);
        Decimal.TryParse(Txt_Honorarios.Text, out Honorarios);
        Decimal.TryParse(Txt_Monto_Descuento_Recargos.Text, out Descuento_Recargos);
        Decimal.TryParse(Txt_Monto_Descuento_Moratorios.Text, out Descuento_Moratorios);
        Decimal.TryParse(Txt_Monto_Descuento_Pronto_Pago.Text, out Descuento_Pronto_Pago);
        Decimal.TryParse(Txt_Porciento_Descuento_Recargos.Text, out Porcentaje_Descuento_Recargos);
        Decimal.TryParse(Txt_Porciento_Descuento_Moratorios.Text, out Porcentaje_Descuento_Moratorios);
        Decimal.TryParse(Txt_Porciento_Descuento_Pronto_Pago.Text, out Porcentaje_Descuento_Pronto_Pago);

        // calcular totales
        Total_Adeudo = Adeudo_Corriente + Adeudo_Rezago + Adeudo_Recargos + Adeudo_Moratorios + Honorarios;
        Total_Descuento = Descuento_Recargos + Descuento_Moratorios + Descuento_Pronto_Pago;
        Total_A_Pagar = Total_Adeudo - Total_Descuento;

        // volver a poner valores (por si alguna caja de texto esta vacia)
        Txt_Monto_Descuento_Recargos.Text = Descuento_Recargos.ToString("#,##0.00");
        Txt_Monto_Descuento_Moratorios.Text = Descuento_Moratorios.ToString("#,##0.00");
        Txt_Monto_Descuento_Pronto_Pago.Text = Descuento_Pronto_Pago.ToString("#,##0.00");
        Txt_Porciento_Descuento_Recargos.Text = Porcentaje_Descuento_Recargos.ToString("#,##0.##");
        Txt_Porciento_Descuento_Moratorios.Text = Porcentaje_Descuento_Moratorios.ToString("#,##0.##");
        Txt_Porciento_Descuento_Pronto_Pago.Text = Porcentaje_Descuento_Pronto_Pago.ToString("#,##0.##");

        // escribir totales en las cajas de texto
        Txt_Total_Adeudo.Text = Math.Round(Math.Round(Total_Adeudo, 3), 2).ToString("#,##0.00");
        Txt_Total_Descuento.Text = Math.Round(Math.Round(Total_Descuento, 3), 2).ToString("#,##0.00");
        Txt_Total_Pagar.Text = Math.Round(Math.Round(Total_A_Pagar, 3), 2).ToString("#,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Porcentaje_Descuento_Recargos
    /// DESCRIPCIÓN: Calcular el porcentaje de descuento a partir de un monto a descontar de recargos ordinarios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Porcentaje_Descuento_Recargos()
    {
        Decimal Recargos = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Recargos_Ordinarios.Text, out Recargos);
        Decimal.TryParse(Txt_Monto_Descuento_Recargos.Text, out Monto);

        // solo calcular si la cantidad de recargos es mayor que cero
        if (Recargos > 0)
        {
            Porcentaje = (Monto * 100) / Recargos;
        }
        else
        {
            Monto = 0;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Porciento_Descuento_Recargos.Text = Math.Round(Math.Round(Porcentaje, 3), 2).ToString("0.##");
        Txt_Monto_Descuento_Recargos.Text = Math.Round(Math.Round(Monto, 3), 2).ToString("#,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Porcentaje_Descuento_Moratorios
    /// DESCRIPCIÓN: Calcular el porcentaje de descuento a partir de un monto a descontar de recargos moratorios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Porcentaje_Descuento_Moratorios()
    {
        Decimal Recargos = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Recargos_Moratorios.Text, out Recargos);
        Decimal.TryParse(Txt_Monto_Descuento_Moratorios.Text, out Monto);

        // solo calcular si la cantidad de recargos es mayor que cero
        if (Recargos > 0)
        {
            Porcentaje = (Monto * 100) / Recargos;
        }
        else
        {
            Monto = 0;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Porciento_Descuento_Moratorios.Text = Math.Round(Math.Round(Porcentaje, 3), 2).ToString("0.##");
        Txt_Monto_Descuento_Moratorios.Text = Math.Round(Math.Round(Monto, 3), 2).ToString("#,##0.00");

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Monto_Descuento_Recargos
    /// DESCRIPCIÓN: Calcular el monto de descuento a partir de un porcentaje a descontar de recargos ordinarios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Monto_Descuento_Recargos()
    {
        Decimal Recargos = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Recargos_Ordinarios.Text, out Recargos);
        Decimal.TryParse(Txt_Porciento_Descuento_Recargos.Text, out Porcentaje);

        Monto = (Recargos * Porcentaje) / 100;
        if (Recargos <= 0 || Monto <= 0 || Porcentaje <= 0)
        {
            Monto = 0;
            Porcentaje = 0;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Porciento_Descuento_Recargos.Text = Math.Round(Math.Round(Porcentaje, 3), 2).ToString("0.##");
        Txt_Monto_Descuento_Recargos.Text = Math.Round(Math.Round(Monto, 3), 2).ToString("#,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Monto_Descuento_Moratorios
    /// DESCRIPCIÓN: Calcular el monto de descuento a partir de un porcentaje a descontar de recargos moratorios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Monto_Descuento_Moratorios()
    {
        Decimal Recargos = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Recargos_Moratorios.Text, out Recargos);
        Decimal.TryParse(Txt_Porciento_Descuento_Moratorios.Text, out Porcentaje);

        Monto = (Recargos * Porcentaje) / 100;
        if (Recargos <= 0 || Monto <= 0 || Porcentaje <= 0)
        {
            Monto = 0;
            Porcentaje = 0;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Porciento_Descuento_Moratorios.Text = Math.Round(Math.Round(Porcentaje, 3), 2).ToString("0.##");
        Txt_Monto_Descuento_Moratorios.Text = Math.Round(Math.Round(Monto, 3), 2).ToString("#,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Descuentos
    /// DESCRIPCIÓN: Validar los descuentos
    ///             - que el porcentaje asignado no sea mayor que 100 o que el máximo autorizado para el usuario
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Descuentos()
    {
        Cls_Ope_Pre_Descuentos_Predial_Negocio Descuento_Autorizado = new Cls_Ope_Pre_Descuentos_Predial_Negocio();
        Decimal Porcentaje_Descuento_Recargos = 0;
        Decimal Porcentaje_Descuento_Moratorios = 0;
        Decimal Porcentaje_Autorizado = 0;
        String Str_Mensaje_Error = "";

        // consultar descuento permitido para usuario actual
        Descuento_Autorizado.P_Usuario = Cls_Sessiones.Empleado_ID;
        Descuento_Autorizado.P_Tipo_Descuento = "PREDIAL";
        String Porcentaje = Descuento_Autorizado.Traer_Descuento();
        Decimal.TryParse(Porcentaje, out Porcentaje_Autorizado);

        // recuperar porcenraje de descuentos
        Decimal.TryParse(Txt_Porciento_Descuento_Recargos.Text, out Porcentaje_Descuento_Recargos);
        Decimal.TryParse(Txt_Porciento_Descuento_Moratorios.Text, out Porcentaje_Descuento_Moratorios);

        // validar que el porcentaje del descuento no sea mayor que el autorizado
        if (Porcentaje_Descuento_Recargos > Porcentaje_Autorizado)
        {
            Str_Mensaje_Error += "No tiene autorización para asignar descuentos mayores a " + Porcentaje_Autorizado + "%<br />";
            Txt_Porciento_Descuento_Recargos.Text = Porcentaje_Autorizado.ToString("0.##");
            Calcular_Monto_Descuento_Recargos();
        }

        // validar que el porcentaje del descuento no sea mayor que el autorizado
        if (Porcentaje_Descuento_Moratorios > Porcentaje_Autorizado)
        {
            Str_Mensaje_Error += "No tiene autorización para asignar descuentos mayores a " + Porcentaje_Autorizado + "%<br />";
            Txt_Porciento_Descuento_Moratorios.Text = Porcentaje_Autorizado.ToString("0.##");
            Calcular_Monto_Descuento_Moratorios();
        }

        // validar que el porcentaje no sea mayor que 100
        if (Porcentaje_Descuento_Recargos > 100)
        {
            Str_Mensaje_Error += "No es posible asignar descuentos mayores a 100%<br />";
            Txt_Porciento_Descuento_Recargos.Text = Porcentaje_Autorizado.ToString("0.##");
            Calcular_Monto_Descuento_Recargos();
        }

        // validar que el porcentaje no sea mayor que 100
        if (Porcentaje_Descuento_Moratorios > 100)
        {
            Str_Mensaje_Error += "No es posible asignar descuentos mayores a 100%<br />";
            Txt_Porciento_Descuento_Moratorios.Text = Porcentaje_Autorizado.ToString("0.##");
            Calcular_Porcentaje_Descuento_Moratorios();
        }

        Mensaje_Error(Str_Mensaje_Error);
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: obtener_Recargos_Moratorios
    /// DESCRIPCIÓN: Muestra los recargos moratorio en la caja de texto correspondiente 
    ///             (se consultan de la capa de negocio de convenios de predial)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-feb-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Obtener_Recargos_Moratorios()
    {
        var Consulta_Recargos = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        decimal Recargos_Moratorios = 0;

        // consultar convenios de la cuenta
        Consulta_Recargos.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
        Recargos_Moratorios = Consulta_Recargos.Obtener_Recargos_Moratorios();

        Txt_Recargos_Moratorios.Text = Recargos_Moratorios.ToString("#,##0.00");
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
        var Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
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
        return Convenio_Vencido;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Valores_Iniciales_Nuevo_Descuento
    /// DESCRIPCIÓN: Establecer los valores iniciales para un nuevo convenio
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Valores_Iniciales_Nuevo_Descuento()
    {
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        DateTime Fecha_Vencimiento;

        // fecha de vencimiento un dia habil a partir de fecha actual
        Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(DateTime.Now.ToShortDateString(), "1");

        Txt_Fecha_Vencimiento.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
        Cmb_Estatus.SelectedValue = "VIGENTE";
        Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;

        Txt_Porciento_Descuento_Recargos.Text = "0";
        Txt_Monto_Descuento_Recargos.Text = "0.00";

        Txt_Porciento_Descuento_Moratorios.Text = "0";
        Txt_Monto_Descuento_Moratorios.Text = "0.00";

        Txt_Porciento_Descuento_Pronto_Pago.Text = "0";
        Txt_Monto_Descuento_Pronto_Pago.Text = "0.00";

        Td_Chk_Liquidacion_Temporal.Visible = false;
        Chk_Liquidacion_Temporal.Checked = false;
        Chk_Liquidacion_Temporal.Enabled = false;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Validar_Componentes()
    {
        Decimal Monto = 0;
        String Mensaje = "";

        if (Txt_Cuenta_Predial.Text == "")
        {
            Mensaje += "+ Es necesario seleccionar una cuenta predial primero.<br />";
            return Mensaje;
        }

        if (Cmb_Estatus.SelectedValue == "APLICADO")
        {
            Mensaje += "+ No puede seleccionar el estatus APLICADO.<br />";
        }

        if (Cmb_Estatus.SelectedIndex < 0)
        {
            Mensaje += "+ Debe seleccionar un estatus para el descuento.<br />";
        }

        if (!Decimal.TryParse(Txt_Total_Pagar.Text, out Monto) || Monto <= 0)
        {
            Mensaje += "+ No hay adeudo para hacer el descuento.<br />";
        }

        if (!Decimal.TryParse(Txt_Porciento_Descuento_Recargos.Text, out Monto) || Monto <= 0)
        {
            if (!Decimal.TryParse(Txt_Porciento_Descuento_Moratorios.Text, out Monto) || Monto <= 0)
            {
                Mensaje += "+ No se ha especificado un descuento para el adeudo.<br />";
            }
        }

        return Mensaje;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Alta_Descuento
    ///DESCRIPCIÓN          : Dar de alta descuento de predial en la base de datos
    ///PARAMETROS:     
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 12-dic-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Alta_Descuento()
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        if (Txt_Porciento_Descuento_Moratorios.Text.Trim() == "")
        {
            Txt_Porciento_Descuento_Moratorios.Text = "0";
            Txt_Monto_Descuento_Moratorios.Text = "0.00";
        }

        try
        {
            Cls_Ope_Pre_Descuentos_Predial_Negocio Descuento_Predial = new Cls_Ope_Pre_Descuentos_Predial_Negocio();
            Descuento_Predial.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Descuento_Predial.P_Realizo = Cls_Sessiones.Empleado_ID;
            Descuento_Predial.P_Estatus = Cmb_Estatus.SelectedValue;
            //Descuento_Predial.P_Fecha = Txt_Fecha_Vencimiento.Text;
            Descuento_Predial.P_Hasta_Bimestre = Cmb_Hasta_Bimestre_Periodo.SelectedValue + Cmb_Hasta_Anio_Periodo.SelectedValue;
            Descuento_Predial.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
            Descuento_Predial.P_Observaciones = Txt_Observaciones.Text.ToUpper();
            Descuento_Predial.P_Desc_Recargo = Txt_Monto_Descuento_Recargos.Text;
            Descuento_Predial.P_Desc_Moratorio = Txt_Monto_Descuento_Moratorios.Text;
            Descuento_Predial.P_Total_Por_Pagar = Txt_Total_Pagar.Text;
            Descuento_Predial.P_Realizo = Txt_Realizo.Text;
            Descuento_Predial.P_Rezagos = Txt_Adeudo_Rezago.Text;
            Descuento_Predial.P_Corriente = Txt_Adeudo_Corriente.Text;
            Descuento_Predial.P_Recargos = Txt_Recargos_Ordinarios.Text;
            Descuento_Predial.P_Contribuyente_ID = Hdn_Contribuyente_ID.Value;
            Descuento_Predial.P_Recargos_Moratorios = Txt_Recargos_Moratorios.Text;
            Descuento_Predial.P_Honorarios = Txt_Honorarios.Text;
            Descuento_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Descuento_Predial.P_Desde_Anio = Txt_Periodo_Inicial.Text.Substring(Txt_Periodo_Inicial.Text.Length - 4, 4);
            Descuento_Predial.P_Desde_Bimestre = Txt_Periodo_Inicial.Text.Substring(0, 1);
            Descuento_Predial.P_Hasta_Anio = Cmb_Hasta_Anio_Periodo.SelectedValue;
            Descuento_Predial.P_Hasta_Bimestre = Cmb_Hasta_Bimestre_Periodo.SelectedValue;
            Descuento_Predial.P_Porcentaje_Recargo = Txt_Porciento_Descuento_Recargos.Text;
            Descuento_Predial.P_Porcentaje_Recargo_Moratorio = Txt_Porciento_Descuento_Moratorios.Text;
            Descuento_Predial.P_Porcentaje_Pronto_Pago = Txt_Porciento_Descuento_Pronto_Pago.Text.Replace(",", "");
            Descuento_Predial.P_Descuento_Pronto_Pago = Txt_Monto_Descuento_Pronto_Pago.Text.Replace(",", "");

            if (Descuento_Predial.Alta_Descuentos_Predial())
            {
                Hdn_No_Descuento.Value = Descuento_Predial.P_No_Descuento;
                Btn_Imprimir_Click(null, null);
                //limpiar sesiones y controles
                Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
                Session.Remove("CUENTA_PREDIAL_ID");
                Session.Remove("CUENTA_PREDIAL");
                Session.Remove("Dt_Ordenes_Variacion_Aceptadas");
                Inicializa_Controles();
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "Descuentos de impuesto predial", "alert('El alta del Descuento predial fue Exitosa');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de impuesto predial", "alert('El alta del Descuento de predial No fue Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta Descuento: " + Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Modificar_Descuento
    ///DESCRIPCIÓN          : Actualizar los datos de un descuento de predial en la base de datos
    ///PARAMETROS:     
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 12-dic-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Modificar_Descuento()
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Cls_Ope_Pre_Descuentos_Predial_Negocio Descuento_Predial = new Cls_Ope_Pre_Descuentos_Predial_Negocio();
            Descuento_Predial.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Descuento_Predial.P_Realizo = Cls_Sessiones.Empleado_ID;
            Descuento_Predial.P_Estatus = Cmb_Estatus.SelectedValue;
            //Descuento_Predial.P_Fecha = Txt_Fecha_Vencimiento.Text;
            Descuento_Predial.P_Hasta_Bimestre = Cmb_Hasta_Bimestre_Periodo.SelectedValue + Cmb_Hasta_Anio_Periodo.SelectedValue;
            Descuento_Predial.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
            Descuento_Predial.P_Observaciones = Txt_Observaciones.Text.ToUpper();
            Descuento_Predial.P_Desc_Recargo = Txt_Monto_Descuento_Recargos.Text;
            Descuento_Predial.P_Desc_Moratorio = Txt_Monto_Descuento_Moratorios.Text;
            Descuento_Predial.P_Total_Por_Pagar = Txt_Total_Pagar.Text;
            Descuento_Predial.P_Realizo = Txt_Realizo.Text;
            Descuento_Predial.P_Rezagos = Txt_Adeudo_Rezago.Text;
            Descuento_Predial.P_Corriente = Txt_Adeudo_Corriente.Text;
            Descuento_Predial.P_Recargos = Txt_Recargos_Ordinarios.Text;
            Descuento_Predial.P_Recargos_Moratorios = Txt_Recargos_Moratorios.Text;
            Descuento_Predial.P_Honorarios = Txt_Honorarios.Text;
            Descuento_Predial.P_Contribuyente_ID = Hdn_Nuevo_Contribuyente_ID.Value;
            Descuento_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Descuento_Predial.P_Desde_Anio = Txt_Periodo_Inicial.Text.Substring(Txt_Periodo_Inicial.Text.Length - 4, 4);
            Descuento_Predial.P_Desde_Bimestre = Txt_Periodo_Inicial.Text.Substring(0, 1);
            Descuento_Predial.P_Hasta_Anio = Cmb_Hasta_Anio_Periodo.SelectedValue;
            Descuento_Predial.P_Hasta_Bimestre = Cmb_Hasta_Bimestre_Periodo.SelectedValue;
            Descuento_Predial.P_Porcentaje_Recargo = Txt_Porciento_Descuento_Recargos.Text;
            Descuento_Predial.P_Porcentaje_Recargo_Moratorio = Txt_Porciento_Descuento_Moratorios.Text;
            Descuento_Predial.P_No_Descuento = Hdn_No_Descuento.Value;
            Descuento_Predial.P_Porcentaje_Pronto_Pago = Txt_Porciento_Descuento_Pronto_Pago.Text.Replace(",", "");
            Descuento_Predial.P_Descuento_Pronto_Pago = Txt_Monto_Descuento_Pronto_Pago.Text.Replace(",", "");

            if (Descuento_Predial.Modificar_Descuento_Predial())
            {
                Hdn_No_Descuento.Value = Descuento_Predial.P_No_Descuento;
                // si el estatus es cancelado, no se imprime descuento
                if (Cmb_Estatus.SelectedValue != "CANCELADO")
                {
                    Btn_Imprimir_Click(null, null);
                }
                //limpiar sesiones y controles
                Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
                Session.Remove("CUENTA_PREDIAL_ID");
                Session.Remove("CUENTA_PREDIAL");
                Session.Remove("Dt_Ordenes_Variacion_Aceptadas");
                Inicializa_Controles();
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "Descuentos de impuesto predial", "alert('El Descuento predial se actualizó Exitosamente');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de impuesto predial", "alert('La actualización del Descuento predial No fue Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Modificar Descuento: " + Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Descuento
    ///DESCRIPCIÓN          : Consulta los datos del descuento seleccionado y los 
    ///                     muestra en los controles correspondientes
    ///PARAMETROS
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 12-dic-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Descuento()
    {
        Cls_Ope_Pre_Descuentos_Predial_Negocio Descuento = new Cls_Ope_Pre_Descuentos_Predial_Negocio();
        Cls_Ope_Pre_Pae_Honorarios_Negocio PAE_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        DataTable Dt_Descuentos_Predial;
        Decimal Monto = 0;
        DateTime Fecha;
        String Contribuyente_ID = "";
        String Nombre_Contribuyente = "";

        Mensaje_Error(null);

        try
        {

            Descuento.P_Cuenta_Predial = Hdn_Cuenta_ID.Value;
            Descuento.P_No_Descuento = Hdn_No_Descuento.Value;
            Dt_Descuentos_Predial = Descuento.Consultar_Descuentos_Predial();

            if (Dt_Descuentos_Predial != null)
            {
                if (Dt_Descuentos_Predial.Rows.Count > 0)
                {
                    Mostrar_Liquidacion();
                    Hdn_Cuenta_ID.Value = Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Cuenta_Predial_ID].ToString();
                    Txt_Cuenta_Predial.Text = Dt_Descuentos_Predial.Rows[0]["Cuenta_Predial"].ToString();
                    Contribuyente_ID = Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Constribuyente_ID].ToString();
                    Hdn_Contribuyente_ID.Value = Contribuyente_ID;
                    Nombre_Contribuyente = Obtener_Dato_Consulta(
                        Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
                        + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
                        + Cat_Pre_Contribuyentes.Campo_Nombre + " NOMBRE ",
                        Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes,
                        Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = '" + Contribuyente_ID + "'");
                    Cargar_Datos();
                    // si se consigue el nombre del contribuyente, escribirlo en la caja de texto (sobreescribe el propietario actual)
                    if (Nombre_Contribuyente != "")
                    {
                        Txt_Nombre_Propietario.Text = Nombre_Contribuyente;
                    }

                    Hdn_No_Descuento.Value = Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial].ToString();
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Corriente].ToString(), out Monto);
                    Txt_Adeudo_Corriente.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Rezagos].ToString(), out Monto);
                    Txt_Adeudo_Rezago.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Recargos].ToString(), out Monto);
                    Txt_Recargos_Ordinarios.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Recargos_Moratorios].ToString(), out Monto);
                    Txt_Recargos_Moratorios.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Honorarios].ToString(), out Monto);
                    if (Monto == 0)
                    {
                        DataTable Dt_PAE_Honorarios = null;
                        PAE_Honorarios.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
                        Dt_PAE_Honorarios = PAE_Honorarios.Consultar_Total_Honorarios();
                        if (Dt_PAE_Honorarios != null)
                        {
                            if (Dt_PAE_Honorarios.Rows.Count > 0)
                            {
                                if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"] != null)
                                {
                                    if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString().Trim() != "")
                                    {
                                        Monto = Convert.ToDecimal(Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"]);
                                    }
                                }
                            }
                        }
                    }
                    Txt_Honorarios.Text = Monto.ToString("#,##0.00");
                    Txt_Total_Descuento.Text = "";
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0]["DESCUENTO_RECARGO"].ToString(), out Monto);
                    Txt_Monto_Descuento_Recargos.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0]["DESCUENTO_RECARGO_MORATORIO"].ToString(), out Monto);
                    Txt_Monto_Descuento_Moratorios.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Descuento_Pronto_Pago].ToString(), out Monto);
                    Txt_Monto_Descuento_Pronto_Pago.Text = Monto.ToString("#,##0.00");
                    tr_descuento_pronto_pago.Visible = Monto > 0 ? true : false;
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo].ToString(), out Monto);
                    Txt_Porciento_Descuento_Recargos.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Recargo_Moratorio].ToString(), out Monto);
                    Txt_Porciento_Descuento_Moratorios.Text = Monto.ToString("#,##0.00");
                    Decimal.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Porcentaje_Pronto_Pago].ToString(), out Monto);
                    Txt_Porciento_Descuento_Pronto_Pago.Text = Monto.ToString("#,##0.00");
                    Cmb_Estatus.SelectedValue = Dt_Descuentos_Predial.Rows[0]["ESTATUS"].ToString();
                    DateTime.TryParse(Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha);
                    Txt_Fecha_Vencimiento.Text = Fecha.ToString("dd/MMM/yyyy");
                    Txt_Observaciones.Text = Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Observaciones].ToString();
                    Txt_Realizo.Text = Dt_Descuentos_Predial.Rows[0][Ope_Pre_Descuentos_Predial.Campo_Usuario_Creo].ToString();
                    Txt_Periodo_Inicial.Text = Dt_Descuentos_Predial.Rows[0]["DESDE_PERIODO_BIMESTRE"].ToString() + "/"
                        + Dt_Descuentos_Predial.Rows[0]["DESDE_PERIODO_ANIO"].ToString();
                    Cmb_Hasta_Bimestre_Periodo.SelectedValue = Dt_Descuentos_Predial.Rows[0]["HASTA_PERIODO_BIMESTRE"].ToString();
                    String Hasta_Anio = Dt_Descuentos_Predial.Rows[0]["HASTA_PERIODO_ANIO"].ToString();

                    if (Cmb_Hasta_Anio_Periodo.Items.FindByText(Hasta_Anio) == null)
                    {
                        System.Web.UI.WebControls.ListItem Anio_Adeudo = new System.Web.UI.WebControls.ListItem(Hasta_Anio, Hasta_Anio);
                        Cmb_Hasta_Anio_Periodo.Items.Add(Anio_Adeudo);
                    }
                    Cmb_Hasta_Anio_Periodo.SelectedValue = Hasta_Anio;
                    Calcular_Totales();
                    Btn_Salir.ToolTip = "Regresar";
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Convenio_Cuenta_Predial
    ///DESCRIPCIÓN: Busca un convenio Activo para la cuenta predial.
    ///PARAMETROS: 
    ///CREO: Ismael Prieto Sánchez. 
    ///FECHA_CREO: 22/Mayo/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Buscar_Convenio_Cuenta_Predial()
    {
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();    //Asigna el manejo de la clase
        DataTable Dt_Convenio = new DataTable();    //Almacena la consulta del convenio
        Double Rezagos = 0;                         //Almacena el monto del rezago del convenio

        //Realiza la consulta del convenio de la cuenta
        Negocio.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
        Dt_Convenio = Negocio.Consultar_Convenio_Cuenta_Predia();
        if (Dt_Convenio != null && Dt_Convenio.Rows.Count > 0)
        {
            foreach (DataRow Registro in Dt_Convenio.Rows)
            {
                Rezagos = Rezagos + Convert.ToDouble(Registro["RECARGOS_ORDINARIOS"].ToString());
            }
            Txt_Recargos_Ordinarios.Text = Math.Round(Rezagos, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
            Calcular_Totales();
        }
    }

    #endregion METODOS


    #region Eventos/Botones [Nuevo,Modificar,Salir,Busqueda]

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Master.Etiqueta_Body_Master_Page.Attributes.Add("onkeydown", "cancelBack()");

            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
                Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();

                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx',"
                    + " 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:yes');";
                Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
                Txt_Cuenta_Predial.Enabled = false;
            }
            Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE','PENDIENTE')";
            Session["TIPO_CONTRIBUYENTE"] = " IN ('PROPIETARIO','POSEEDOR') ";

            Mensaje_Error(null);
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// DESCRIPCIÓN: Habilita la forma para ingresar datos y permitir guardar un nuevo registro
    ///             en caso de guardar, verifica la validez de los datos ingresados y reporta cualquier
    ///             error
    /// PARAMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        String Mensajes_Error = "";
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                Limpiar_Controles_Datos_Generales();
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                Valores_Iniciales_Nuevo_Descuento();
                Btn_Mostrar_Busqueda_Avanzada.Focus();
            }
            else
            {
                Mensaje_Error(null);
                Mensajes_Error = Validar_Componentes();

                //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                if (Mensajes_Error.Length > 0)
                {
                    Mensaje_Error(Mensajes_Error);
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario, dar de alta los mismos en la base de datos
                else
                {
                    Alta_Descuento(); //Da de alta los datos proporcionados por el usuario
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Descuento
    ///PARAMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-dic-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        String Mensajes_Error = "";

        Mensaje_Error(null);

        // si se asigno un valor al campo oculto con el id de la cuenta predial, hay un descuento seleccionado
        if (Hdn_Cuenta_ID.Value != "")
        {
            //if (Grid_Convenios.SelectedRow.Cells[9].Text.Replace("&nbsp;", "").Equals(""))
            //{
            try
            {

                if (Btn_Modificar.ToolTip.Equals("Modificar"))
                {
                    // verificar que hay un numero de descuento seleccionado
                    if (Hdn_No_Descuento.Value.Trim() != "")
                    {
                        // si el estatus es cancelado, no se permite la edicion
                        if (Cmb_Estatus.SelectedValue == "CANCELADO")
                        {
                            Mensaje_Error("No es posible modificar descuentos CANCELADOS.");
                        }
                        else if (Cmb_Estatus.SelectedValue == "APLICADO")
                        {
                            Mensaje_Error("No es posible modificar el descuento porque ya fue aplicado.");
                        }
                        else
                        {
                            // cargar datos del grid de parcialidades (para que muestre los controles editables del importe)
                            Habilitar_Controles("Modificar");
                            //Muestra la informacion del adeudo
                            Mostrar_Liquidacion();
                            //calcula los montos del descuento
                            Calcular_Monto_Descuento_Recargos();
                            Calcular_Monto_Descuento_Moratorios();
                            Calcular_Totales();
                        }
                    }
                    else
                    {
                        Mensaje_Error("Seleccione el Registro que desea modificar.");
                    }
                }
                else
                {
                    Mensajes_Error = Validar_Componentes();

                    //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                    if (Mensajes_Error.Length > 0)
                    {
                        Mensaje_Error(Mensajes_Error);
                    }
                    else
                    {
                        Modificar_Descuento();
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = "Modificar: " + Ex.Message;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }

        }
        else if (Grid_Descuentos_Predial.SelectedIndex > -1)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Descuentos de impuesto predial", "alert('Seleccione un descuento a modificar, por favor.');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        String Busqueda = "";

        Mensaje_Error(null);
        try
        {
            Busqueda = Txt_Busqueda.Text.Trim();

            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpiar_Controles_Datos_Generales(); //Limpia los controles del forma

            // restablecer criteior de busqueda y llamar metodo que carga el grid
            Hdn_Busqueda.Value = Busqueda;
            Cargar_Grid_Descuentos(0);

            // mostrar mensaje si no se encontraron coincidencias
            if (Grid_Descuentos_Predial.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Mensaje_Error("No se encontraron descuentos para: \"" + Txt_Busqueda.Text + "\"");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Buscar: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: salir de la orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:47:11 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
                Session.Remove("CUENTA_PREDIAL_ID");
                Session.Remove("CUENTA_PREDIAL");
                Session.Remove("Dt_Ordenes_Variacion_Aceptadas");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Imprimir_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el boton imprimir (enviar impresion de calculo de impuesto
    ///             y del formato seleccionado)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Decimal Total_Adeudo = 0;

        // verificar que hay una cuenta seleccionada
        if (Hdn_Cuenta_ID.Value != "")
        {
            // verificar que el adeudo es mayor a cero
            if (Decimal.TryParse(Txt_Total_Pagar.Text, out Total_Adeudo) && Total_Adeudo > 0)
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Validar_Campos();

                //Si faltaron campos por capturar envia un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = Lbl_Mensaje_Error.Text;
                }
                else
                {
                    // llamar metodo impresion de reporte
                    Imprimir_Reporte(Crear_Ds_Descuentos_Predial(),
                        "Rpt_Pre_Descuentos_Predial.rpt",
                        "Descuentos de Predial");
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No es posible imprimir porque no hay adeudos.<br />";
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Debe seleccionar un descuento para imprimir.<br />";
        }
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Chk_Liquidacion_Temporal_CheckedChanged
    /// DESCRIPCIÓN: Manejar evento cambio de estado del checkbox (cargar adeudos de la cuenta)
    /// PARÁMETROS:
    /// 		1. sender: control que envia el evento
    /// 		2. e: argumentos del evento
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Chk_Liquidacion_Temporal_CheckedChanged(object sender, EventArgs e)
    {
        Limpiar_Cantidades();
        // Limpiar grid de adeudos
        Grid_Adeudos.DataSource = null;
        Grid_Adeudos.DataBind();

        Obtener_Recargos_Moratorios();
        Cargar_Adeudos_Actual_Diferencias(false);
        // si se activa la liquidación temporal, cargar datos del propietario desde orden de variación
        if (Chk_Liquidacion_Temporal.Checked)
            Cargar_Datos_Cuenta_Variacion();
        else
            Cargar_Datos();
    }

    #endregion

    #region Eventos Combos

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Hasta_Periodo_SelectedIndexChanged
    ///DESCRIPCIÓN          : Manejo del evento cambio de indice para los combos hasta
    ///                         bimestre y anio (actualizar datos de adeudos hasta periodo 
    ///                         seleccionado)
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 26-nov-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Hasta_Periodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cargar_Adeudos_Actual_Diferencias(true);
        Obtener_Descuento_Pronto_Pago();
        Obtener_Recargos_Moratorios();
        Buscar_Convenio_Cuenta_Predial();
        Calcular_Monto_Descuento_Recargos();
        Calcular_Monto_Descuento_Moratorios();
        Calcular_Totales();
    }

    #endregion

    #region Eventos TxtChanged

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Porciento_Descuento_Recargos_TextChanged
    /// DESCRIPCIÓN: Llamar al método que calcular el monto del descuento cuando se cambia el porcentaje
    /// PARÁMETROS:
    /// 		1. sender: objecto que desencadena el evento
    /// 		2. e: argumentos del evento
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Porciento_Descuento_Recargos_TextChanged(object sender, EventArgs e)
    {
        // limpiar mensaje de error
        Mensaje_Error(null);
        // llamar metodos para calculo
        Calcular_Monto_Descuento_Recargos();
        Validar_Descuentos();
        Calcular_Totales();
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Monto_Descuento_Recargos_TextChanged
    /// DESCRIPCIÓN: Llamar al método que calcular el porcentaje del descuento cuando se cambia el monto
    /// PARÁMETROS:
    /// 		1. sender: objecto que desencadena el evento
    /// 		2. e: argumentos del evento
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Monto_Descuento_Recargos_TextChanged(object sender, EventArgs e)
    {
        // limpiar mensaje de error
        Mensaje_Error(null);
        // llamar metodos para calculo
        Calcular_Porcentaje_Descuento_Recargos();
        Validar_Descuentos();
        Calcular_Totales();
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Porciento_Descuento_Moratorios_TextChanged
    /// DESCRIPCIÓN: Llamar al método que calcular el monto del descuento cuando se cambia el porcentaje
    /// PARÁMETROS:
    /// 		1. sender: objecto que desencadena el evento
    /// 		2. e: argumentos del evento
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Porciento_Descuento_Moratorios_TextChanged(object sender, EventArgs e)
    {
        // limpiar mensaje de error
        Mensaje_Error(null);
        // llamar metodos para calculo
        Calcular_Monto_Descuento_Moratorios();
        Validar_Descuentos();
        Calcular_Totales();
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Monto_Descuento_Moratorios_TextChanged
    /// DESCRIPCIÓN: Llamar al método que calcular el porcentaje del descuento cuando se cambia el monto
    /// PARÁMETROS:
    /// 		1. sender: objecto que desencadena el evento
    /// 		2. e: argumentos del evento
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Monto_Descuento_Moratorios_TextChanged(object sender, EventArgs e)
    {
        // limpiar mensaje de error
        Mensaje_Error(null);
        // llamar metodos para calculo
        Calcular_Porcentaje_Descuento_Moratorios();
        Validar_Descuentos();
        Calcular_Totales();
    }

    #endregion

    #region Eventos Grid

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Adeudos_Actual_Diferencias
    /// DESCRIPCIÓN: Lee adeudos de la cuenta y adeudos en analisis de rezago y los muestra sumados
    /// PARÁMETROS:
    ///             1. Tomar_Periodo_Final: Si es falso, se mostrarán todos los adeudos y si es verdadero, 
    ///                         se tomará el periodo final indicado en los combos hasta anio y hasta bimestre
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: Roberto González Oseguera
    /// FECHA_MODIFICÓ: 16-sep-2011
    /// CAUSA_MODIFICACIÓN: En lugar de tomar la cuota bimestral y sumarla a cada bimestre, 
    ///             se toma el importe, se divide entre los bimestres a considerar y se suma a los bimestres
    ///*******************************************************************************************************
    private void Cargar_Adeudos_Actual_Diferencias(bool Tomar_Periodo_Final)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Adeudo_Actual = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Cls_Cat_Pre_Tabulador_Recargos_Negocio Rs_Recargos = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
        Cls_Ope_Pre_Pae_Honorarios_Negocio PAE_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();

        DataTable Dt_Adeudos_Actuales;
        Int32 Desde_Anio = 99999;
        Int32 Hasta_Anio = 0;
        Int32 Tmp_Desde_Anio = 0;
        Int32 Tmp_Hasta_Anio = 0;
        Int32 Mes_Actual = DateTime.Now.Month;
        Int32 Anio_Actual = DateTime.Now.Year;
        Decimal Total_Adeudo_Rezago = 0;
        Decimal Total_Adeudo_Corriente = 0;
        Decimal Total_Adeudo_Recargos = 0;
        Decimal Total_Honorarios = 0;
        Int32 Anio_Corriente;
        DataTable Dt_Ordenes_Variacion_Aceptadas = (DataTable)Session["Dt_Ordenes_Variacion_Aceptadas"];
        Cls_Ope_Pre_Parametros_Negocio Parametro_Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();

        String Periodo_Inicial = "-";
        String Periodo_Final = "-";
        String Anio_Adeudo = "";

        Dictionary<String, Decimal> Dic_Adeudos_Diferencias = new Dictionary<string, decimal>();
        Dictionary<String, Decimal> Dicc_Tabulador_recargos = new Dictionary<String, Decimal>();
        Dictionary<int, decimal> Dic_Cuotas_Minimas;

        // validar que haya una cuenta seleccionada
        if (Txt_Cuenta_Predial.Text != "")
        {
            try
            {
                Anio_Corriente = Parametro_Anio_Corriente.Consultar_Anio_Corriente();

                Dic_Cuotas_Minimas = Obtener_Diccionario_Cuotas_Minimas();

                // obtener los adeudos actuales de la cuenta
                Dt_Adeudos_Actuales = Rs_Adeudo_Actual.Calcular_Recargos_Predial(Hdn_Cuenta_ID.Value);
                // validar que se obtuvieron adeudos de la cuenta
                if (Dt_Adeudos_Actuales != null)
                {
                    if (Dt_Adeudos_Actuales.Rows.Count > 0)
                    {
                        // recorrer cada fila y agregar al diccionario
                        foreach (DataRow Fila_Adeudo in Dt_Adeudos_Actuales.Rows)
                        {
                            Decimal Cuota_Bimestral = 0;
                            if (Decimal.TryParse(Fila_Adeudo["ADEUDO"].ToString(), out Cuota_Bimestral) && Cuota_Bimestral > 0)
                            {
                                if (!Dic_Adeudos_Diferencias.ContainsKey(Fila_Adeudo["PERIODO"].ToString().Trim()))
                                {
                                    Dic_Adeudos_Diferencias.Add(Fila_Adeudo["PERIODO"].ToString().Trim(), Cuota_Bimestral);
                                }
                                else
                                {
                                    Dic_Adeudos_Diferencias[Fila_Adeudo["PERIODO"].ToString().Trim()] += Cuota_Bimestral;
                                }
                            }
                        }
                    }
                }

                // solo cargar adeudos si el checkbox de liquidacion temporal esta activado
                if (Chk_Liquidacion_Temporal.Checked)
                {
                    // obtener adeudos del analisis de rezago en la orden de variacion
                    Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
                    // recorrer todas las ordenes aceptadas y agregarlas al diccionario
                    if (Dt_Ordenes_Variacion_Aceptadas != null)
                    {
                        foreach (DataRow orden in Dt_Ordenes_Variacion_Aceptadas.Rows)
                        {
                            Orden_Variacion.P_Generar_Orden_No_Orden = orden["No_Orden_Variacion"].ToString().Trim();
                            Orden_Variacion.P_Generar_Orden_Anio = orden["Anio"].ToString().Trim();
                            DataTable Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();

                            // procesar cada fila obtenida  para agregar bimestres al diccionario
                            foreach (DataRow Fila_Grid in Dt_Agregar_Diferencias.Rows)
                            {
                                Decimal Importe = 0;
                                String Periodo = Fila_Grid[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Trim();
                                Decimal.TryParse(Fila_Grid[Ope_Pre_Diferencias_Detalle.Campo_Importe].ToString(), out Importe);
                                if (Fila_Grid["TIPO"].ToString().Trim() == "BAJA")
                                    Importe *= -1;
                                Dic_Adeudos_Diferencias = Agregar_Periodos(Periodo, Importe, Dic_Adeudos_Diferencias, Dic_Cuotas_Minimas);

                                // obtener los periodos minimo y maximo a tomar en cuenta de la tabla de diferencias
                                if (Obtener_Anio_Minimo_Maximo(
                                    out Tmp_Desde_Anio,
                                    out Tmp_Hasta_Anio,
                                    Dt_Agregar_Diferencias,
                                    Ope_Pre_Diferencias_Detalle.Campo_Periodo,
                                    2))
                                {
                                    // si el valor regresado por la funcion es menor, asignar, si no, dejar el mismo
                                    if (Tmp_Desde_Anio < Desde_Anio)
                                    {
                                        Desde_Anio = Tmp_Desde_Anio;
                                    }
                                    // si el valor regresado por la funcion es mayor, asignarlo a la variable, si no, dejar el mismo
                                    if (Tmp_Hasta_Anio > Hasta_Anio)
                                    {
                                        Hasta_Anio = Tmp_Hasta_Anio;
                                    }
                                }

                            }
                        }
                    }
                }
                // comprobar que hay adeudos para mostrar
                if (Dic_Adeudos_Diferencias.Count > 0)
                {

                    Dicc_Tabulador_recargos = Rs_Recargos.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);

                    if (Obtener_Anio_Minimo_Maximo(out Tmp_Desde_Anio, out Tmp_Hasta_Anio, Dt_Adeudos_Actuales, "PERIODO", 1))
                    {
                        // si el valor regresado por la funcion es menor, asignar, si no, dejar el mismo
                        if (Tmp_Desde_Anio < Desde_Anio)
                        {
                            Desde_Anio = Tmp_Desde_Anio;
                        }
                        // si el valor regresado por la funcion es mayor, asignarlo a la variable, si no, dejar el mismo
                        if (Tmp_Hasta_Anio > Hasta_Anio)
                        {
                            Hasta_Anio = Tmp_Hasta_Anio;
                        }
                    }

                    // si se va a tomar hasta el año seleccionado
                    if (Tomar_Periodo_Final == true)
                    {
                        Hasta_Anio = Convert.ToInt32(Cmb_Hasta_Anio_Periodo.SelectedValue);
                    }

                    DataTable Dt_Adeudos = Crear_Tabla_Adeudos();
                    // formar la tabla de adeudos a partir de los adeudos en el diccionario
                    for (int anio = Desde_Anio; anio <= Hasta_Anio; anio++)
                    {
                        DataRow Nuevo_Adeudo = Dt_Adeudos.NewRow();
                        Decimal Total_Adeudo_Anio = 0;
                        Nuevo_Adeudo[0] = anio.ToString();
                        Int32 Hasta_Bimestre = 0;

                        // si se Tomar bimestre es verdadero y es el año seleccionado, establecer el bimestre seleccionado
                        if (Tomar_Periodo_Final == true && anio == Hasta_Anio)
                        {
                            Hasta_Bimestre = Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue);
                        }
                        else
                        {
                            Hasta_Bimestre = 6;
                        }

                        // agregar bimestre del diccionario
                        for (int bimestre = Hasta_Bimestre; bimestre >= 1; bimestre--)
                        {
                            if (Dic_Adeudos_Diferencias.ContainsKey(bimestre.ToString() + anio.ToString()))
                            {
                                String Bimestre = bimestre.ToString() + anio.ToString();
                                Nuevo_Adeudo[bimestre] = Math.Round(Math.Round(Dic_Adeudos_Diferencias[Bimestre], 3), 2).ToString("#,##0.00");
                                Total_Adeudo_Anio += Dic_Adeudos_Diferencias[Bimestre];
                                // calcular recargos con el tabulador
                                if (Dicc_Tabulador_recargos.ContainsKey(Bimestre))
                                {
                                    Total_Adeudo_Recargos += Math.Round(((Dic_Adeudos_Diferencias[Bimestre] * Dicc_Tabulador_recargos[Bimestre]) / 100M), 2, MidpointRounding.AwayFromZero);
                                }
                                if (anio == Hasta_Anio)
                                {
                                    // identificar periodo final (el primero con adeudo del año Hasta_Anio)
                                    if (Periodo_Final == "-" && Dic_Adeudos_Diferencias[Bimestre] > 0)
                                    {
                                        Periodo_Final = bimestre.ToString() + "/" + Hasta_Anio;
                                    }
                                }
                                if (anio == Desde_Anio)
                                {
                                    Periodo_Inicial = bimestre.ToString() + "/" + Desde_Anio;
                                }
                            }
                            else
                            {
                                Nuevo_Adeudo[bimestre] = "0.00";
                            }
                        }

                        Nuevo_Adeudo["TOTAL"] = Math.Round(Math.Round(Total_Adeudo_Anio, 3), 2).ToString("#,##0.00");
                        Dt_Adeudos.Rows.Add(Nuevo_Adeudo);
                    }

                    // cargar tabla de adeudos en el grid
                    Grid_Adeudos.DataSource = Dt_Adeudos;
                    Grid_Adeudos.DataBind();

                    // recalcular total (suma de parcialidades)
                    decimal Adeudo_Bimestre;
                    foreach (GridViewRow Adeudo in Grid_Adeudos.Rows)
                    {
                        int int_Anio_Adeudo;
                        decimal Adeudo_Anual = 0;
                        decimal.TryParse(Adeudo.Cells[1].Text, out Adeudo_Bimestre);
                        Adeudo_Anual += Adeudo_Bimestre;
                        decimal.TryParse(Adeudo.Cells[2].Text, out Adeudo_Bimestre);
                        Adeudo_Anual += Adeudo_Bimestre;
                        decimal.TryParse(Adeudo.Cells[3].Text, out Adeudo_Bimestre);
                        Adeudo_Anual += Adeudo_Bimestre;
                        decimal.TryParse(Adeudo.Cells[4].Text, out Adeudo_Bimestre);
                        Adeudo_Anual += Adeudo_Bimestre;
                        decimal.TryParse(Adeudo.Cells[5].Text, out Adeudo_Bimestre);
                        Adeudo_Anual += Adeudo_Bimestre;
                        decimal.TryParse(Adeudo.Cells[6].Text, out Adeudo_Bimestre);
                        Adeudo_Anual += Adeudo_Bimestre;

                        Adeudo.Cells[7].Text = Adeudo_Anual.ToString("#,##0.00");
                        // separar adeudo corriente y rezago
                        int.TryParse(Adeudo.Cells[0].Text, out int_Anio_Adeudo);
                        if (int_Anio_Adeudo >= Anio_Corriente)
                        {
                            Total_Adeudo_Corriente += Adeudo_Anual;
                        }
                        else
                        {
                            Total_Adeudo_Rezago += Adeudo_Anual;
                        }
                    }

                    bool Aumentar_Periodo_Final = false;
                    Int32 Contador_Adeudos_Tabla = Dt_Adeudos.Rows.Count;
                    Int32 Bimestre_Seleccionado = Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue);

                    // recorrer los bimestres desde el bimestre seleccionado y si por lo menos un bimestre tiene adeudo, aumentar bimestre periodo final (caso cuotas minimas)
                    for (int i = Bimestre_Seleccionado; i >= 1; i--)
                    {
                        // si el adeudo es mayor que cero, seleccionar el bimestre anterior al de i
                        if (Contador_Adeudos_Tabla > 0 && Convert.ToDecimal(Dt_Adeudos.Rows[Contador_Adeudos_Tabla - 1][i].ToString()) > 0)
                        {
                            Aumentar_Periodo_Final = true;
                            break;
                        }
                    }

                    // si se va a tomar el periodo del combo y el bimestre es diferente de 6
                    if (Tomar_Periodo_Final == true && Cmb_Hasta_Bimestre_Periodo.SelectedValue != "6" && Aumentar_Periodo_Final)
                    {
                        Anio_Adeudo = Cmb_Hasta_Anio_Periodo.SelectedValue;
                        // recorrer los bimestres siguientes al seleccionado hasta el sexto o hasta encontrar un adeudo myor a cero
                        for (int i = Bimestre_Seleccionado + 1; i <= 6; i++)
                        {
                            // si el diccioario contiene valor para el bimestre, validar monto
                            if (Dic_Adeudos_Diferencias.ContainsKey(i.ToString() + Anio_Adeudo))
                            {
                                // si el adeudo es mayor que cero, seleccionar el bimestre anterior al de i
                                if (Dic_Adeudos_Diferencias[i.ToString() + Anio_Adeudo] > 0)
                                {
                                    Cmb_Hasta_Bimestre_Periodo.SelectedValue = (i - 1).ToString();
                                    break;
                                }
                            }
                            else // seleccionar i en el bimestre
                            {
                                Cmb_Hasta_Bimestre_Periodo.SelectedValue = i.ToString();
                            }
                        }
                    }

                    // solo si se muestran todos los bimestres (no se muestran los adeudos hasta cierto periodo)
                    if (!Tomar_Periodo_Final)
                    {
                        Cmb_Hasta_Anio_Periodo.Items.Clear();
                        // llenar combos periodo final con valores en el grid de adeudos
                        for (int i = Contador_Adeudos_Tabla - 1; i >= 0; i--)
                        {
                            Anio_Adeudo = Dt_Adeudos.Rows[i][0].ToString();
                            if (Cmb_Hasta_Anio_Periodo.Items.FindByText(Anio_Adeudo) == null)
                            {
                                System.Web.UI.WebControls.ListItem Lst_Anio_Adeudo = new System.Web.UI.WebControls.ListItem(Anio_Adeudo, Anio_Adeudo);
                                Cmb_Hasta_Anio_Periodo.Items.Add(Lst_Anio_Adeudo);
                            }
                        }
                        // si hay adeudos en el grid, seleccionar el ultimo año y bimestre con adeudos
                        if (Contador_Adeudos_Tabla > 0)
                        {
                            Cmb_Hasta_Anio_Periodo.SelectedValue = Dt_Adeudos.Rows[Contador_Adeudos_Tabla - 1][0].ToString();
                            Cmb_Hasta_Bimestre_Periodo.SelectedValue = "6";
                        }
                    }
                }

                // mostrar datos en las cajas de texto
                Txt_Periodo_Inicial.Text = Periodo_Inicial;
                Txt_Recargos_Ordinarios.Text = Math.Round(Total_Adeudo_Recargos, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                Txt_Adeudo_Corriente.Text = Math.Round(Total_Adeudo_Corriente, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                Txt_Adeudo_Rezago.Text = Math.Round(Total_Adeudo_Rezago, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");

                DataTable Dt_PAE_Honorarios = null;
                PAE_Honorarios.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
                Dt_PAE_Honorarios = PAE_Honorarios.Consultar_Total_Honorarios();
                if (Dt_PAE_Honorarios != null)
                {
                    if (Dt_PAE_Honorarios.Rows.Count > 0)
                    {
                        if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"] != null)
                        {
                            if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString().Trim() != "")
                            {
                                Total_Honorarios = Convert.ToDecimal(Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"]);
                            }
                        }
                    }
                }
                Txt_Honorarios.Text = Total_Honorarios.ToString("#,###,##0.00");

                Calcular_Totales();
            }
            catch (Exception Ex)
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Agregar_Periodos
    /// DESCRIPCIÓN: Lee el periodo que recibe como parametro y agrega bimestres correspondientes
    ///             con la cuota especificada en el diccionario 
    /// PARÁMETROS:
    /// 		1. Periodo: Periodo a agregar (formato: 1/2011-6/2011)
    /// 		2. Cuota_Bimestral: Valor decimal con el que se agregaran las parcialidades
    /// 		3. Dic_Adeudos_Diferencias: Diccionario en el que se van a agregar los periodos (con el formato AñoBimestre: 20111)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<String, Decimal> Agregar_Periodos(String Periodo, Decimal Importe, Dictionary<String, Decimal> Dic_Adeudos, Dictionary<int, decimal> Dic_Cuotas_Minimas)
    {
        String[] Arr_Periodos;
        String[] Arr_Bimestres;
        Decimal Importe_Bimestral = 0;
        Int32 Anio = 0;
        Int32 Divisor = 0;
        Int32 Bimestre_Inicial = 0;
        Int32 Bimestre_Final = 0;
        Decimal Sum_Adeudos_Periodo = 0;
        int Cont_Cuotas_Minimas_Periodo = 0;

        // separar periodo por guion medio
        Arr_Periodos = Periodo.Split('-');
        if (Arr_Periodos.Length >= 2)
        {
            // separar bimestre y año inicial por diagonal
            Arr_Bimestres = Arr_Periodos[0].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Inicial);
                Int32.TryParse(Arr_Bimestres[1].ToString().Trim(), out Anio);
            }
            // separar bimestre y año final por diagonal
            Arr_Bimestres = Arr_Periodos[1].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Final);
            }

            // si se obtuvieron valores, agregar al diccionario
            if (Bimestre_Final > 0 && Bimestre_Inicial > 0 && Anio > 0)
            {
                Sum_Adeudos_Periodo = 0;

                // sumar los adeudos del periodo y contar las cuotas minimas 
                for (int i = Bimestre_Inicial; i <= Bimestre_Final; i++)
                {
                    Divisor++;
                    if (Dic_Adeudos.ContainsKey(i.ToString() + Anio.ToString()))
                    {
                        // si el adeudo es igual a la cuota minima, incrementar contador
                        if (Dic_Adeudos[i.ToString() + Anio.ToString()] == Dic_Cuotas_Minimas[Anio])
                        {
                            Cont_Cuotas_Minimas_Periodo++;
                        }
                        // si el adeudo del bimestre es diferente de cero, sumar al periodo
                        if (Dic_Adeudos[i.ToString() + Anio.ToString()] != 0)
                        {
                            Sum_Adeudos_Periodo += Dic_Adeudos[i.ToString() + Anio.ToString()];
                        }
                    }
                }

                // VALIDACIONES PARA CASOS DE CUOTAS MÍNIMAS Y APLICACIÓN DE ADEUDOS
                // si el adeudo anterior contenia una sola cuota minima y el nuevo importe no es cuota minima y abarca todo el año
                if (Cont_Cuotas_Minimas_Periodo == 1 && Importe != Dic_Cuotas_Minimas[Anio] && Divisor == 6)
                {
                    // sumar la cuota minima (adeudo anterior) más el nuevo importe y prorratear
                    // se utiliza toString para redondear a dos decimales con el mismo método que en otros cálculos
                    Importe_Bimestral = Convert.ToDecimal(((Importe + Sum_Adeudos_Periodo) / Divisor).ToString("0.00"));
                    // desde bimestre inicial hasta final, agregar al diccionario
                    for (int i = Bimestre_Inicial; i <= Bimestre_Final; i++)
                    {
                        // si no existe en el diccionario, agregar cuota bimestral
                        if (!Dic_Adeudos.ContainsKey(i.ToString() + Anio.ToString()))
                        {
                            Dic_Adeudos.Add(i.ToString() + Anio.ToString(), Importe_Bimestral);
                        }
                        // si ya existe asignar nueva cuota bimestral
                        else
                        {
                            Dic_Adeudos[i.ToString() + Anio.ToString()] = Importe_Bimestral;
                        }
                    }
                }
                else
                {
                    // si  el nuevo importe mas los adeudos previos (6 bimestres) son iguales a la cuota mínima
                    if (Divisor == 6 && Sum_Adeudos_Periodo + Importe == Dic_Cuotas_Minimas[Anio])
                    {
                        // aplicar cuota minima en el primer bimestre
                        if (!Dic_Adeudos.ContainsKey("1" + Anio.ToString()))
                        {
                            Dic_Adeudos.Add("1" + Anio.ToString(), Dic_Cuotas_Minimas[Anio]);
                        }
                        else
                        {
                            Dic_Adeudos["1" + Anio.ToString()] = Dic_Cuotas_Minimas[Anio];
                        }
                        // eliminar los otros adeudos
                        for (int Cont_Bimestres = 2; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dic_Adeudos.ContainsKey(Cont_Bimestres.ToString() + Anio.ToString()))
                            {
                                Dic_Adeudos.Remove(Cont_Bimestres.ToString() + Anio.ToString());
                            }
                        }
                    }
                    else
                    {
                        // si el importe es cuota minima, agregar al bimestre inicial del periodo
                        if (Importe == Dic_Cuotas_Minimas[Anio])
                        {
                            // si no existe en el diccionario de adeudos, agregar el importe
                            if (!Dic_Adeudos.ContainsKey(Bimestre_Inicial.ToString() + Anio.ToString()))
                            {
                                Dic_Adeudos.Add(Bimestre_Inicial.ToString() + Anio.ToString(), Importe);
                            }
                            else // si ya existe en le diccionario, sumar el importe
                            {
                                Dic_Adeudos[Bimestre_Inicial.ToString() + Anio.ToString()] += Importe;
                            }
                        }
                        else // prorratear
                        {
                            // se utiliza toString para redondear a dos decimales con el mismo método que en otros cálculos
                            Importe_Bimestral = Convert.ToDecimal((Importe / Divisor).ToString("0.00"));
                            for (int Cont_Bimestres = Bimestre_Inicial; Cont_Bimestres <= Bimestre_Final; Cont_Bimestres++)
                            {
                                if (!Dic_Adeudos.ContainsKey(Cont_Bimestres.ToString() + Anio.ToString()))
                                {
                                    Dic_Adeudos.Add(Cont_Bimestres.ToString() + Anio.ToString(), Importe_Bimestral);
                                }
                                else
                                {
                                    Dic_Adeudos[Cont_Bimestres.ToString() + Anio.ToString()] += Importe_Bimestral;
                                }
                            }
                        }
                    }
                }
            }
        }

        return Dic_Adeudos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Anio_Minimo_Maximo
    /// DESCRIPCIÓN: Breve descripción de lo que hace la función.
    /// PARÁMETROS:
    /// 		1. Desde_Anio: Variable que almacena el valor del año menor
    /// 		2. Hasta_Anio: Variable que almacena el valor del año mayor
    /// 		3. Dt_Adeudos: Datatable con periodos a procesar
    /// 		4. Nombre_Fila: Nombre de la fila con los periodos
    /// 		5. Indice: Valor entero desde el que se obtiene el año (formatos: 1/2011 y 12011)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Boolean Obtener_Anio_Minimo_Maximo(out Int32 Desde_Anio, out Int32 Hasta_Anio, DataTable Dt_Adeudos, String Nombre_Fila, Int32 Indice)
    {
        Int32 Anio = 0;
        Desde_Anio = 99999;
        Hasta_Anio = 0;

        // para cada fila en la tabla
        foreach (DataRow Fila_Tabla in Dt_Adeudos.Rows)
        {
            // obtener y parsear el año
            String St_Anio = Fila_Tabla[Nombre_Fila].ToString().Substring(Indice, 4);
            if (Int32.TryParse(St_Anio, out Anio))
            {
                if (Anio < Desde_Anio)
                {
                    Desde_Anio = Anio;
                }
                if (Anio > Hasta_Anio)
                {
                    Hasta_Anio = Anio;
                }
            }
        }

        // regresar verdadero si se asignaron años a las variables desde y hasta anio
        if (Desde_Anio < 99999 && Hasta_Anio > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Diccionario_Cuotas_Minimas
    /// DESCRIPCIÓN: Consulta el catalogo de cuotas minimas para regresarlo como un diccionario: <Año,Cuota>
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 08-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<int, decimal> Obtener_Diccionario_Cuotas_Minimas()
    {
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Catalogo_Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Dictionary<int, decimal> Dic_Cuotas_Minimas = new Dictionary<int, decimal>();
        DataTable Dt_Cuotas;
        Int32 Anio = 0;
        Decimal Cuota = 0;

        // consultar cuotas minimas del catalogo
        Catalogo_Cuotas_Minimas.P_Anio = "";
        Dt_Cuotas = Catalogo_Cuotas_Minimas.Consultar_Cuotas_Minimas();

        // si el datatable no es nulo
        if (Dt_Cuotas != null)
        {
            // recorrer las filas del datatable para agregar cuotas al diccionario
            foreach (DataRow Fila_Cuota in Dt_Cuotas.Rows)
            {
                // si la fila contiene el año y la cuota, agregar al diccionario
                if (Int32.TryParse(Fila_Cuota["ANIO"].ToString(), out Anio) && Decimal.TryParse(Fila_Cuota["CUOTA"].ToString(), out Cuota))
                {
                    // validar que el año no se encuentra ya en el diccionario
                    if (!Dic_Cuotas_Minimas.ContainsKey(Anio))
                    {
                        Dic_Cuotas_Minimas.Add(Anio, Cuota);
                    }
                }
            }
        }

        return Dic_Cuotas_Minimas;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Adeudos
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las parcialidades
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 27-sep-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Adeudos()
    {
        DataTable Dt_Adeudos = new DataTable();
        Dt_Adeudos.Columns.Add(new DataColumn("ANIO", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre1", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre2", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre3", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre4", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre5", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre6", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("TOTAL", typeof(String)));

        return Dt_Adeudos;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Limpiar_Controles_Datos_Generales();

                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdn_Cuenta_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Mostrar_Liquidacion();
                Cargar_Datos();
                // validar que no exista un convenio activo para la cuenta
                Existe_Descuento_Activo(Cuenta_Predial_ID);
            }
        }
        Session["BUSQUEDA_CUENTAS_PREDIAL"] = null;
        Session["CUENTA_PREDIAL_ID"] = null;
        Session["CUENTA_PREDIAL"] = null;

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Liquidacion
    /// DESCRIPCIÓN: Buscar ordenes de variacion de la cuenta para mostrar datos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Mostrar_Liquidacion()
    {
        try
        {
            Hdn_Contrarecibo.Value = "";
            var Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

            Orden_Variacion.P_Incluir_Campos_Foraneos = true;
            Orden_Variacion.P_Join_Contrarecibo = true;
            //Orden_Variacion.P_Generar_Orden_Estatus = "ACEPTADA";
            // indicar los estatus de contrarecibo que no se van a incluir (solo incluir estatus VALIDADO y POR PAGAR)
            Orden_Variacion.P_Contrarecibo_Estatus = "'PENDIENTE','RECHAZADO','GENERADO','PAGADO','POR VALIDAR'";
            Orden_Variacion.P_Filtros_Dinamicos = Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                        + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '"
                        + Convert.ToString(Session["CUENTA_PREDIAL_ID"]) + "' AND "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                        + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " IS NOT NULL AND "
                        + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                        + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA' ";

            Orden_Variacion.P_Ordenar_Dinamico = Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC, (SELECT "
                + Cat_Pre_Movimientos.Campo_Identificador + " FROM " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos
                + " WHERE " + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = "
                + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ")";

            M_Orden_Negocio.P_Cuenta_Predial = Hdn_Cuenta_ID.Value;
            //Orden_Variacion.P_Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
            //Orden_Variacion.P_Año = DateTime.Now.Year;
            DataTable Dt_Orden_Variacion = Orden_Variacion.Consultar_Ordenes_Variacion_Contrarecibos();
            Session["Dt_Ordenes_Variacion_Aceptadas"] = Dt_Orden_Variacion;
            // validar que se encontraron ordenes de variacion
            if (Dt_Orden_Variacion != null && Dt_Orden_Variacion.Rows.Count > 0)
            {
                // mostrarcheckbox de liquidacion temporal
                Td_Chk_Liquidacion_Temporal.Visible = true;
                Chk_Liquidacion_Temporal.Checked = false;
                // solo activar el check si es alta o modificacion
                if (Btn_Modificar.ToolTip == "Actualizar" || Btn_Nuevo.ToolTip == "Dar de Alta")
                    Chk_Liquidacion_Temporal.Enabled = true;
            }
            else
            {
                Td_Chk_Liquidacion_Temporal.Visible = false;
                Chk_Liquidacion_Temporal.Checked = false;
                Chk_Liquidacion_Temporal.Enabled = false;
            }

            //M_Orden_Negocio.P_Contrarecibo = null;
            //Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta_Sin_Contrarecibo();

            //se obtienen los recargos moratorios
            Obtener_Recargos_Moratorios();

            //cargo los adeudos totales de la cuenta
            Cargar_Adeudos_Actual_Diferencias(false);

            //Se verifica si hay o no convenio
            Buscar_Convenio_Cuenta_Predial();

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

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Existe_Descuento_Activo
    /// DESCRIPCIÓN: Consulta si existen convenios no aplicados para una cuenta predial, de ser así, 
    ///             regresa el numero de descuento
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: ID de la cuenta predial
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Existe_Descuento_Activo(String Cuenta_Predial_ID)
    {
        String No_Descuento = "";
        DataTable Dt_Descuentos;
        Cls_Ope_Pre_Descuentos_Predial_Negocio Consulta_Descuentos = new Cls_Ope_Pre_Descuentos_Predial_Negocio();

        if (Cuenta_Predial_ID != null)
        {
            Consulta_Descuentos.P_Filtros_Dinamicos = " DESCUENTO." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID
                + " = '" + Cuenta_Predial_ID + "'"
                + " AND DESCUENTO." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " IN ('VIGENTE', 'PENDIENTE') ";

            Dt_Descuentos = Consulta_Descuentos.Consultar_Descuentos_Predial_Busqueda();

            // si se encontraron descuentos
            if (Dt_Descuentos != null && Dt_Descuentos.Rows.Count > 0)
            {
                No_Descuento = Dt_Descuentos.Rows[0][Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial].ToString();
                Btn_Salir_Click(null, null);
                Cargar_Datos_Descuento(Cuenta_Predial_ID, No_Descuento);
                Mensaje_Error("La cuenta tiene " + Dt_Descuentos.Rows.Count + " descuento(s) por aplicar.");
            }
        }

        return No_Descuento;
    }

    #endregion

}