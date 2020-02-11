using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Divisiones.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Resolucion_Calculo_Traslado : System.Web.UI.Page
{

    #region Propiedades
    public String Cuenta_Predial_ID
    {
        get
        {
            return Hdn_Cuenta_Predial_ID.Value;
        }
    }
    public String Cuenta_Predial
    {
        get
        {
            return Txt_Cuenta_Predial.Text;
        }
    }

    public String No_Calculo
    {
        get
        {
            String[] Folio_Calculo = Grid_Calculos.SelectedRow.Cells[3].Text.Split('/');
            return Folio_Calculo[0];
        }
    }


    public String Anio_Calculo
    {
        get
        {
            String[] Folio_Calculo = Grid_Calculos.SelectedRow.Cells[3].Text.Split('/');
            return Folio_Calculo[1];
        }
    }

    public String No_Contrarecibo
    {
        get
        {
            return Grid_Calculos.SelectedRow.Cells[1].Text;
        }
    }
    #endregion

    ///********************************************************************************
    ///                                 METODOS
    ///********************************************************************************

    #region METODOS
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 09-ago-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpiar_Controles(); //Limpia los controles del forma
            Pnl_Controles.Visible = false;
            Pnl_Grid.Visible = true;
            Consulta_Calculos(true);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
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
    /// FECHA_CREO: 09-ago-2010
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va a ser habilitado para que los edite el usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Convenio.Visible = false;
                    Btn_Convenio.CausesValidation = false;
                    Btn_Imprimir.Visible = false;
                    Txt_Estatus.Enabled = false;
                    Txt_Folio_Pago.ReadOnly = true;
                    Txt_Fundamento.Enabled = false;
                    Opt_Formato_Oficial.Enabled = false;
                    Opt_Formato_Notario.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    break;

            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Folio_Pago.Enabled = false;
            Txt_Cuenta_Predial.Enabled = false;
            Chk_Predio_Colindante.Enabled = false;
            Chk_Constancia_No_Adeudo.Enabled = false;
            Txt_Base_Impuesto.Enabled = false;
            Txt_Minimo_Elevado_Anio.Enabled = false;
            Txt_Base_Gravable_Traslado.Enabled = false;
            Txt_Tasa_Traslado_Dominio.Enabled = false;
            Txt_Impuesto_Traslado_Dominio.Enabled = false;
            Txt_Fecha_Escritura.Enabled = false;
            Txt_Tipo_Division_Lotificacion.Enabled = false;
            Txt_Base_Impuesto_Div_Lotif.Enabled = false;
            Txt_Tasa_Division_Lotificacion.Enabled = false;
            Txt_Impuesto_Division_Lotificacion.Enabled = false;
            // el costo de constancias no se edita
            Txt_Costo_Constancia_No_Adeudo.Enabled = false;
            Txt_Multa.Enabled = false;
            Txt_Recargos.Enabled = false;
            Txt_Total.Enabled = false;
            Txt_Comentarios_Area.Enabled = Habilitado;
            Opt_Tipo_Avaluo_Predial.Enabled = false;
            Opt_Tipo_Valor_Fiscal.Enabled = false;
            Opt_Tipo_Valor_Operacion.Enabled = false;


            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Grid_Calculos.Enabled = !Habilitado;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION: Limpia los controles que se encuentran en la forma
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 09-ago-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            Txt_Busqueda.Text = "";

            Txt_Cuenta_Predial.Text = "";
            Chk_Predio_Colindante.Checked = false;
            Txt_Base_Impuesto.Text = "";
            Txt_Minimo_Elevado_Anio.Text = "";
            Txt_Base_Gravable_Traslado.Text = "";
            Txt_Tasa_Traslado_Dominio.Text = "";
            Txt_Impuesto_Traslado_Dominio.Text = "";
            Txt_Fecha_Escritura.Text = "";
            Txt_Tipo_Division_Lotificacion.Text = "";
            Txt_Base_Impuesto_Div_Lotif.Text = "";
            Txt_Tasa_Division_Lotificacion.Text = "";
            Txt_Impuesto_Division_Lotificacion.Text = "";
            Txt_Costo_Constancia_No_Adeudo.Text = "";
            Chk_Constancia_No_Adeudo.Text = "";
            Txt_Multa.Text = "";
            Txt_Recargos.Text = "";
            Txt_Total.Text = "";
            Txt_Comentarios_Area.Text = "";
            Opt_Tipo_Avaluo_Predial.Checked = false;
            Opt_Tipo_Valor_Fiscal.Checked = false;
            Opt_Tipo_Valor_Operacion.Checked = false;

            Hdn_No_Calculo.Value = "";
            Hdn_No_Orden.Value = "";
            Hdn_Anio_Orden.Value = "";
            Hdn_Anio_Calculo.Value = "";
            Hdn_Tasa_Div_Lotif_ID.Value = "";
            Hdn_Tasa_Traslado_ID.Value = "";
            Hdn_Cuenta_Predial_ID.Value = "";

            // limpiar campos observaciones de seguimiento
            Contenedor_Observaciones_Anteriores.InnerHtml = "";
            Contenedor_Observaciones_Anteriores.Style.Value = "display:none;";
            Encabezado_Observaciones_Anteriores.Style.Value = "display:none;";
        }
        catch (Exception ex)
        {
            throw new Exception("Limpiar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Campos
    /// DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    ///             correspondiente.
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Campos()
    {
        //Si falta alguno de los campos mencionarlo en la etiqueta Lbl_Mensaje_Error para mostrarla 
        Lbl_Mensaje_Error.Text = "";

        if (Txt_Cuenta_Predial.Text == "")  //Validar campo NOMBRE (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + No hay cuenta predial <br />";
        }
        if (Opt_Formato_Notario.Checked == false && Opt_Formato_Oficial.Checked == false)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Tipo de impresión.<br />";
        }
        if (Txt_Fundamento.Text.Length <= 0)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Proporcionar el fundamento legal para la impresión.<br />";
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Impuesto_Traslado
    /// DESCRIPCIÓN: Realizar calculo de impuesto de traslado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Validar_Calculo_Impuesto_Traslado()
    {
        Decimal Base_Impuesto = 0;
        Decimal Minimo_Elevado_Al_Anio = 0;
        Decimal Base_Gravable = 0;
        Decimal Tasa_Traslado = 0;
        Decimal Total_Impuesto_Traslado = 0;

        // solo si se encuentran valores para todos los parametros, llamar al método que calcular el impuesto
        if (Decimal.TryParse(Txt_Base_Impuesto.Text, out Base_Impuesto) && Base_Impuesto > 0 &&
            Decimal.TryParse(Txt_Minimo_Elevado_Anio.Text, out Minimo_Elevado_Al_Anio) &&
            Decimal.TryParse(Txt_Tasa_Traslado_Dominio.Text, out Tasa_Traslado) && Tasa_Traslado > 0)
        {
            // si no es predio colindante, calcular con todos los datos
            if (Chk_Predio_Colindante.Checked == false)
            {
                Calcular_Impuesto_Traslado(Base_Impuesto, Minimo_Elevado_Al_Anio, Tasa_Traslado,
                    out Base_Gravable, out Total_Impuesto_Traslado);
            }
            else    // si es predio colindante, pasar minimo elevado al anio como 0 (no se resta a la base del impuesto)
            {
                Calcular_Impuesto_Traslado(Base_Impuesto, 0, Tasa_Traslado,
                    out Base_Gravable, out Total_Impuesto_Traslado);
            }
            // si se obtuvieron valores en la consulta, mostrar montos redondeados
            if (Base_Gravable > 0 && Total_Impuesto_Traslado > 0)
            {
                Txt_Base_Gravable_Traslado.Text = Decimal.Round(Base_Gravable, 2).ToString();
                Txt_Impuesto_Traslado_Dominio.Text = Decimal.Round(Total_Impuesto_Traslado, 2).ToString();
            }
            else    // si no, limpiar campos
            {
                Txt_Base_Gravable_Traslado.Text = "";
                Txt_Impuesto_Traslado_Dominio.Text = "";
                Txt_Base_Impuesto_Div_Lotif.Text = "";
            }
        }

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Modificar_Calculo
    /// DESCRIPCIÓN: Modifica los datos del calculo (guardar fundamento)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ago-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Modificar_Calculo(Boolean Insertar_Pasivo)
    {
        var Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión hacia la capa de Negocios para envio de datos a modificar
        var Consulta_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        var Consulta_Orden_Variacion = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();

        OracleConnection Cn = new OracleConnection();
        OracleCommand Cmd = new OracleCommand();
        OracleTransaction Trans = null;

        DataTable Dt_Datos_Orden;
        DataTable Dt_Clave;
        Decimal Monto = 0;
        Int32 Anio_Orden = 0;
        String Tipo_Predio = "";
        String Tipo_Predio_ID = "";
        String Propietario = "";

        try
        {
            // consultar el propietario y tipo de predio de la orden de variacion 
            Consulta_Orden_Variacion.P_No_Orden_Variacion = Hdn_No_Orden.Value;
            Consulta_Orden_Variacion.P_Anio_Orden = Int32.TryParse(Hdn_Anio_Orden.Value, out Anio_Orden) ? Anio_Orden : DateTime.Now.Year;
            Consulta_Orden_Variacion.P_Incluir_Campos_Foraneos = true;
            Dt_Datos_Orden = Consulta_Orden_Variacion.Consultar_Ordenes_Variacion();
            // si la consulta regreso datos, obtener el TIPO_PREDIO_ID
            if (Dt_Datos_Orden != null && Dt_Datos_Orden.Rows.Count > 0)
            {
                Tipo_Predio_ID = Dt_Datos_Orden.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID].ToString();
                Propietario = Dt_Datos_Orden.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
            }

            // consultar nombre del tipo de predio a partir del ID obtenido
            String Dato_Consulta = Obtener_Dato_Consulta(
                Cat_Pre_Tipos_Predio.Campo_Descripcion,
                Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio,
                Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '"
                + Tipo_Predio_ID + "'"
                );
            if (!string.IsNullOrEmpty(Dato_Consulta))
            {
                Tipo_Predio = Dato_Consulta;
            }

            // crear transaccion para modificar tabla de calculos y de adeudos folio
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            Modificar_Calculo.P_Cmd_Calculo = Cmd;
            // datos del calculo
            Modificar_Calculo.P_No_Calculo = Hdn_No_Calculo.Value;
            Modificar_Calculo.P_Fundamento = Txt_Fundamento.Text;
            Modificar_Calculo.P_Observaciones = Txt_Comentarios_Area.Text;
            Modificar_Calculo.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            Modificar_Calculo.P_Anio_Calculo = Convert.ToInt32(Hdn_Anio_Calculo.Value);
            Modificar_Calculo.P_Estatus = "POR PAGAR";
            // llamar metodos para afectar calculo y generar adeudo
            Modificar_Calculo.Actualizar_Estatus_Calculo();
            // llamar metodo para actualizar estatus del contrarecibo
            Modificar_Calculo.Actualizar_Estatus_Contrarecibo();

            // datos para el pasivo (validando condicion)
            if (Insertar_Pasivo)
            {
                Modificar_Calculo.P_Referencia = "TD" + Convert.ToInt32(Hdn_No_Calculo.Value) + Hdn_Anio_Calculo.Value;
                // eliminar pasivos con la misma referencia con estatus POR PAGAR
                Modificar_Calculo.Eliminar_Referencias_Pasivo();
                //Rs_Modificar_Calculo.P_Descripcion = "CALCULO POR TRASLADO";
                Modificar_Calculo.P_Fecha_Tramite = Grid_Calculos.SelectedRow.Cells[6].Text;
                Modificar_Calculo.P_Fecha_Vencimiento_Pasivo = Grid_Calculos.SelectedRow.Cells[6].Text;
                Modificar_Calculo.P_Estatus = "POR PAGAR";
                Modificar_Calculo.P_Cuenta_Predial_ID = Hdn_Cuenta_Predial_ID.Value;
                Modificar_Calculo.P_Contribuyente = Propietario;

                // tratar de obtener un monto de traslado
                if (Decimal.TryParse(Txt_Impuesto_Traslado_Dominio.Text, out Monto))
                {
                    // si se obtuvo un monto mayor que cero, insertar pasivo
                    if (Monto > 0)
                    {
                        Consulta_Claves_Ingreso.P_Tipo = "TRASLADO";
                        Consulta_Claves_Ingreso.P_Tipo_Predial_Traslado = "IMPUESTO " + Tipo_Predio;
                        Dt_Clave = Consulta_Claves_Ingreso.Consultar_Clave_Ingreso();
                        // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            Modificar_Calculo.P_Descripcion = "IMPUESTO DE TRASLADO";
                            Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Modificar_Calculo.P_Monto_Total_Pagar = Monto.ToString();
                            Modificar_Calculo.Alta_Pasivo();
                        }
                        else
                        {
                            throw new Exception("Falta la clave de ingreso para el IMPUESTO DE TRASLADO " + Tipo_Predio);
                        }
                    }
                }
                // tratar de obtener un monto de division
                if (Decimal.TryParse(Txt_Impuesto_Division_Lotificacion.Text, out Monto))
                {
                    // si se obtuvo un monto mayor que cero, insertar pasivo
                    if (Monto > 0)
                    {
                        Consulta_Claves_Ingreso.P_Tipo = "DIVISION";
                        Consulta_Claves_Ingreso.P_Tipo_Predial_Traslado = "IMPUESTO";
                        Dt_Clave = Consulta_Claves_Ingreso.Consultar_Clave_Ingreso();
                        // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            Modificar_Calculo.P_Descripcion = "IMPUESTO DE DIVISION";
                            Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Modificar_Calculo.P_Monto_Total_Pagar = Monto.ToString();
                            Modificar_Calculo.Alta_Pasivo();
                        }
                        else
                        {
                            throw new Exception("Falta la clave de ingreso para el IMPUESTO DE DIVISIÓN");
                        }
                    }
                }
                // tratar de obtener un monto de constancia
                if (Decimal.TryParse(Txt_Costo_Constancia_No_Adeudo.Text, out Monto))
                {
                    // si se obtuvo un monto mayor que cero, insertar pasivo
                    if (Monto > 0)
                    {
                        // consultar id de constancia de no adeudo del catalogo de parametros
                        Cls_Ope_Pre_Parametros_Negocio Rs_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
                        DataTable Dt_Parametros = Rs_Parametros.Consultar_Parametros();

                        Consulta_Claves_Ingreso.P_Tipo = String.Empty;
                        Consulta_Claves_Ingreso.P_Tipo_Predial_Traslado = String.Empty;
                        Consulta_Claves_Ingreso.P_Documento_ID = Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Constancia_No_Adeudo].ToString();
                        Dt_Clave = Consulta_Claves_Ingreso.Consultar_Clave_Ingreso();
                        // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            Modificar_Calculo.P_Descripcion = "CONSTANCIA";
                            Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Modificar_Calculo.P_Monto_Total_Pagar = Monto.ToString();
                            Modificar_Calculo.Alta_Pasivo();
                        }
                        else
                        {
                            throw new Exception("Falta la clave de ingreso de CONSTANCIA");
                        }
                    }
                }
                // tratar de obtener un monto de multas
                if (Decimal.TryParse(Txt_Multa.Text, out Monto))
                {
                    // si se obtuvo un monto mayor que cero, insertar pasivo
                    if (Monto > 0)
                    {
                        Consulta_Claves_Ingreso.P_Tipo = "TRASLADO";
                        Consulta_Claves_Ingreso.P_Tipo_Predial_Traslado = "MULTAS";
                        Dt_Clave = Consulta_Claves_Ingreso.Consultar_Clave_Ingreso();
                        // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            Modificar_Calculo.P_Descripcion = "MULTA";
                            Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Modificar_Calculo.P_Monto_Total_Pagar = Monto.ToString();
                            Modificar_Calculo.Alta_Pasivo();
                        }
                        else
                        {
                            throw new Exception("Falta la clave de ingreso de MULTAS de TRASLADO");
                        }
                    }
                }
                // tratar de obtener un monto de recargos
                if (Decimal.TryParse(Txt_Recargos.Text, out Monto))
                {
                    // si se obtuvo un monto mayor que cero, insertar pasivo
                    if (Monto > 0)
                    {
                        Consulta_Claves_Ingreso.P_Tipo = "TRASLADO";
                        Consulta_Claves_Ingreso.P_Tipo_Predial_Traslado = "RECARGOS ORDINARIOS";
                        Dt_Clave = Consulta_Claves_Ingreso.Consultar_Clave_Ingreso();
                        // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                        if (Dt_Clave.Rows.Count > 0)
                        {
                            Modificar_Calculo.P_Descripcion = "RECARGOS";
                            Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                            Modificar_Calculo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                            Modificar_Calculo.P_Monto_Total_Pagar = Monto.ToString();
                            Modificar_Calculo.Alta_Pasivo();
                        }
                        else
                        {
                            throw new Exception("Falta la clave de ingreso de RECARGOS de TRASLADO");
                        }
                    }
                }

            } // termina insertar pasivo (si Insertar_Pasivo==true)

            // aplicar cambios a la base de datos 
            Trans.Commit();
        }
        catch (OracleException Ex)
        {
            Trans.Rollback();
            throw new Exception("Error: " + Ex.Message);
        }
        catch (Exception ex)
        {
            if (Cmd != null)
            {
                Trans.Rollback();
            }
            throw new Exception("Modificar_Calculo: " + ex.Message.ToString(), ex);
        }
        finally
        {
            Cn.Close();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Impuesto_Traslado
    /// DESCRIPCIÓN: Realizar calculo de impuesto de traslado
    /// PARÁMETROS:
    /// 		1. Base_Impuesto: Base del impuesto a calcular
    /// 		2. Minimo_Elevado_Al_Anio: Valor del deducible
    /// 		3. Tasa_Traslado: Valor de la tasa de traslado
    /// 		4. Base_Gravable: monto base para calcular impuesto (Base_Impuesto - Minimo_Elevado_Al_Anio)
    /// 		5. Total_Impuesto_Traslado: Donde se va a regresar el total calculado
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Impuesto_Traslado(Decimal Base_Impuesto, Decimal Minimo_Elevado_Al_Anio,
        Decimal Tasa_Traslado, out Decimal Base_Gravable, out Decimal Total_Impuesto_Traslado)
    {
        Base_Gravable = Base_Impuesto - Minimo_Elevado_Al_Anio;
        Total_Impuesto_Traslado = Base_Gravable * Tasa_Traslado / 100;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Mostrar_Total_Impuesto
    /// DESCRIPCIÓN: Calcular y mostrar en los campos correspondientes el total impuesto
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Mostrar_Total_Impuesto()
    {
        Decimal Impuesto_Division_Lotif;
        Decimal Costo_Constancia = 0;
        Decimal Multa;
        Decimal Recargos;
        Decimal Total_Impuesto;

        Decimal.TryParse(Txt_Impuesto_Division_Lotificacion.Text, out Impuesto_Division_Lotif);
        if (Chk_Constancia_No_Adeudo.Checked)
        {
            Decimal.TryParse(Txt_Costo_Constancia_No_Adeudo.Text, out Costo_Constancia);
        }
        Decimal.TryParse(Txt_Recargos.Text, out Recargos);
        Decimal.TryParse(Txt_Multa.Text, out Multa);
        // calcular total
        Total_Impuesto = Impuesto_Division_Lotif + Costo_Constancia + Recargos + Multa;
        // mostrar en las cajas de texto correspondientes
        Txt_Multa.Text = Decimal.Round(Multa, 2).ToString();
        Txt_Recargos.Text = Decimal.Round(Recargos, 2).ToString();
        Txt_Total.Text = Decimal.Round(Total_Impuesto, 2).ToString();
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Extraer_Numero
    /// DESCRIPCIÓN: Mediante una expresión regular encuentra números en el texto
    /// PARÁMETROS:
    /// 	1. Texto: Texto en el que se va a buscar un número
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-mar-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private String Extraer_Numero(String Texto)
    {
        Regex Rge_Decimal = new Regex(@"(?<entero>[0-9]{1,12})(?:\.[0-9]{0,2})?");
        Match Numero_Encontrado = Rge_Decimal.Match(Texto);
        if (Numero_Encontrado.Value != "")
            return Numero_Encontrado.Value;
        else
            return "0";
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Tasas_Traslado_Anio
    /// DESCRIPCIÓN: Consulta las tasas de traslado de dominiopor anio
    ///             Se almacenan en variable de sesion
    /// PARÁMETROS:
    /// 	1. Anio: Anio de la tasa a buscar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consultar_Tasas_Traslado_Anio(String Anio)
    {
        Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Rs_Tasas = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();
        DataTable Dt_Tasa_Anio;

        // si no se proporciona anio, tomar el actual
        if (String.IsNullOrEmpty(Anio))
        {
            Rs_Tasas.P_Anio = DateTime.Now.Year.ToString();
        }

        Dt_Tasa_Anio = Rs_Tasas.Consultar_Anio();

        // mostrar datos en campos 
        if (Dt_Tasa_Anio.Rows.Count > 0)
        {
            Session["Tasas_Traslado_Anio"] = Dt_Tasa_Anio;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Tasas_Traslado_Por_ID
    /// DESCRIPCIÓN: Consulta las tasas de traslado de dominio por ID y se muestra en el campo tasa taslado
    /// PARÁMETROS:
    /// 	1. Tasa: ID de la tasa a buscar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consultar_Tasas_Traslado_Por_ID(String Tasa)
    {
        Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio Rs_Tasas = new Cls_Cat_Pre_Impuestos_Traslado_Dominio_Negocio();
        DataTable Dt_Tasa_Anio;

        if (!String.IsNullOrEmpty(Tasa))
        {
            Rs_Tasas.P_Tasa_ID = Tasa;
        }
        else if (Hdn_Tasa_Traslado_ID.Value != "")
        {
            Rs_Tasas.P_Tasa_ID = Hdn_Tasa_Traslado_ID.Value;
        }

        Dt_Tasa_Anio = Rs_Tasas.Consultar_Tasas_ID();

        // mostrar datos en campos 
        if (Dt_Tasa_Anio.Rows.Count > 0)
        {
            Txt_Tasa_Traslado_Dominio.Text = Dt_Tasa_Anio.Rows[0][Cat_Pre_Impuestos_Traslado_Dominio.Campo_Tasa].ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Tasas_Division
    /// DESCRIPCIÓN: Consulta las tasas de division y lotificacion
    /// PARÁMETROS:
    /// 	1. Tasa: ID de la tasa a buscar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consultar_Tasas_Division(String Tasa)
    {
        Cls_Cat_Pre_Divisiones_Negocio Rs_Tasas = new Cls_Cat_Pre_Divisiones_Negocio();
        DataTable Dt_Tasa;

        if (!String.IsNullOrEmpty(Tasa))
        {
            Dt_Tasa = Rs_Tasas.Consultar_Divisiones_ID(Tasa);

            // mostrar datos en campos 
            if (Dt_Tasa.Rows.Count > 0)
            {
                Txt_Tasa_Division_Lotificacion.Text = Dt_Tasa.Rows[0][Cat_Pre_Divisiones_Impuestos.Campo_Tasa].ToString();
                Txt_Tipo_Division_Lotificacion.Text = Dt_Tasa.Rows[0][Cat_Pre_Divisiones.Campo_Descripcion].ToString();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consultar_Costo_Constancia
    /// DESCRIPCIÓN: Consulta el costo de la constancia de no adeudo del catalogo de parametros
    ///             y la muestra en el campo correspondiente
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Consultar_Costo_Constancia()
    {
        Cls_Ope_Pre_Parametros_Negocio Rs_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        Cls_Cat_Pre_Tipos_Constancias_Negocio Rs_Constancias = new Cls_Cat_Pre_Tipos_Constancias_Negocio();

        DataTable Dt_Constancia;
        DataTable Dt_Parametros = Rs_Parametros.Consultar_Parametros();
        if (Dt_Parametros.Rows.Count > 0)
        {
            Rs_Constancias.P_Filtros_Dinamicos = " " + Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID + " = '"
                + Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Constancia_No_Adeudo].ToString() + "' ";
            Dt_Constancia = Rs_Constancias.Consultar_Tipos_Constancias();

            // mostrar datos en campos 
            if (Dt_Constancia.Rows.Count > 0)
            {
                Txt_Costo_Constancia_No_Adeudo.Text = Dt_Constancia.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Costo].ToString();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    ///         para generar dos reportes al mismo tiempo (folio para pago y formato)
    /// PARÁMETROS:
    /// 		1. Ds_Impuesto_Calculo: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Impuesto_Calculo, String Nombre_Reporte_Calculo, String Nombre_Archivo_Calculo, String Nombre_Reporte_Formato, String Nombre_Archivo_Formato)
    {
        ReportDocument Reporte_Calculo = new ReportDocument();
        ReportDocument Reporte_Formato = new ReportDocument();
        String Ruta_Calculo = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte_Calculo);
        String Ruta_Formato = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte_Formato);

        try
        {
            Reporte_Calculo.Load(Ruta_Calculo);
            Reporte_Calculo.SetDataSource(Ds_Impuesto_Calculo);
            Reporte_Formato.Load(Ruta_Formato);
            Reporte_Formato.SetDataSource(Ds_Impuesto_Calculo);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String PDF_Calculo = Nombre_Archivo_Calculo + ".pdf";  // Es el nombre del PDF que se va a generar 
        String PDF_Formato = Nombre_Archivo_Formato + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Calculo);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte_Calculo.Export(Export_Options_Calculo);

            ExportOptions Export_Options_Formato = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Formato = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Formato.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Formato);
            Export_Options_Formato.ExportDestinationOptions = Disk_File_Destination_Options_Formato;
            Export_Options_Formato.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Formato.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte_Formato.Export(Export_Options_Formato);

            Mostrar_Reporte(PDF_Calculo, "Calculo", "Window_Rpt");
            Mostrar_Reporte(PDF_Formato, "Formato", "Window_Fmt");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    ///             Para imprimir un solo reporte
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-oct-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Convenio);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String PDF_Convenio = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(PDF_Convenio, "Formato", "Window_Fmt");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Calculo_Impuesto_Traslado
    ///DESCRIPCIÓN          : Crea un Dataset con los datos del cálculo seleccionado (NO_CALCULO )
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 20-ago-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Calculo_Impuesto_Traslado()
    {
        var Consulta_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        var Ds_Calculo = new Ds_Ope_Pre_Calculo_Impuesto_Traslado();
        DataTable Dt_Datos_Orden;

        DataRow Dr_Orden = Ds_Calculo.Tables[1].NewRow();
        DataRow Dr_Calculo = Ds_Calculo.Tables[0].NewRow();
        DataRow Dr_Calculo_Dup = Ds_Calculo.Tables[2].NewRow();

        // insertar datos en la fila instanciada directamente de los controles en pantalla
        Dr_Calculo["NO_CALCULO"] = Hdn_No_Calculo.Value;
        Dr_Calculo["BASE_IMPUESTO"] = Txt_Base_Impuesto.Text;
        Dr_Calculo["MINIMO_ELEVADO_ANIO"] = Txt_Minimo_Elevado_Anio.Text;
        Dr_Calculo["BASE_GRAVABLE_TRASLADO"] = Txt_Base_Gravable_Traslado.Text;
        Dr_Calculo["BASE_GRAVABLE_DIVISION"] = Txt_Base_Impuesto_Div_Lotif.Text;
        Dr_Calculo["TASA_TRASLADO"] = Txt_Tasa_Traslado_Dominio.Text;
        Dr_Calculo["TASA_DIVISION"] = Txt_Tasa_Division_Lotificacion.Text;
        Dr_Calculo["IMPUESTO_TRASLADO"] = Txt_Impuesto_Traslado_Dominio.Text;
        Dr_Calculo["IMPUESTO_DIVISION"] = Txt_Impuesto_Division_Lotificacion.Text;
        Dr_Calculo["CONSTANCIA"] = Txt_Costo_Constancia_No_Adeudo.Text;
        Dr_Calculo["RECARGOS"] = Txt_Recargos.Text;
        Dr_Calculo["MULTAS"] = Txt_Multa.Text;
        Dr_Calculo["TOTAL"] = Txt_Total.Text;
        Dr_Calculo["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Calculo["NO_ORDEN_VARIACION"] = Hdn_No_Orden.Value;
        Dr_Calculo["REALIZO_CALCULO"] = Hdn_Realizo_Calculo.Value;
        Dr_Calculo["FUNDAMENTO"] = Txt_Fundamento.Text;
        Dr_Calculo["OBSERVACIONES"] = Txt_Comentarios_Area.Text.ToUpper();
        Dr_Calculo["TANTOS_SALARIO"] = "0.00";
        if (Opt_Tipo_Avaluo_Predial.Checked)
            Dr_Calculo["TIPO"] = Opt_Tipo_Avaluo_Predial.Text;
        else if (Opt_Tipo_Valor_Fiscal.Checked)
            Dr_Calculo["TIPO"] = Opt_Tipo_Valor_Fiscal.Text;
        else if (Opt_Tipo_Valor_Operacion.Checked)
            Dr_Calculo["TIPO"] = Opt_Tipo_Valor_Operacion.Text;
        // agregar fila a la tabla
        Ds_Calculo.Tables[0].Rows.Add(Dr_Calculo);

        // obtener informacion de orden de variacion y su contrarecibo
        int Anio_Orden;
        int.TryParse(Hdn_Anio_Orden.Value, out Anio_Orden);
        Consulta_Calculo.P_No_Orden_Variacion = Hdn_No_Orden.Value;
        Consulta_Calculo.P_Anio_Orden = Anio_Orden;
        Consulta_Calculo.P_Incluir_Campos_Foraneos = true;
        Dt_Datos_Orden = Consulta_Calculo.Consultar_Ordenes_Variacion();

        // si la consulta regreso valores en la tabla, asignar datos
        if (Dt_Datos_Orden != null && Dt_Datos_Orden.Rows.Count > 0)
        {
            String Propietario = "";
            String Ubicacion_Predio = "";
            String Calle = "";
            String Colonia = "";
            String Numero_Interior = "";
            String Numero_Exterior = "";

            Calle = Dt_Datos_Orden.Rows[0]["NOMBRE_CALLE_UBICACION"].ToString();
            Numero_Exterior = Dt_Datos_Orden.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString();
            Numero_Interior = Dt_Datos_Orden.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString();
            Colonia = Dt_Datos_Orden.Rows[0]["NOMBRE_COLONIA_UBICACION"].ToString();
            Propietario = Dt_Datos_Orden.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
            // formar domicilio
            Ubicacion_Predio = Calle + " " + Numero_Exterior + " ";
            if (!string.IsNullOrEmpty(Numero_Interior))
            {
                Ubicacion_Predio += " int. " + Numero_Interior + " ";
            }
            Ubicacion_Predio += Colonia;

            //Inserta los datos de la orden en la tabla
            Dr_Orden["NO_ORDEN_VARIACION"] =
                Dt_Datos_Orden.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion].ToString();
            Dr_Orden["FOLIO_ORDEN"] = Txt_Folio_Pago.Text;
            Dr_Orden["PROPIETARIO"] = Propietario;
            Dr_Orden["UBICACION"] = Ubicacion_Predio;
            Dr_Orden["CONTRARECIBO"] = Dt_Datos_Orden.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo];
            // agregar fila a la tabla
            Ds_Calculo.Tables[1].Rows.Add(Dr_Orden);
        }

        // insertar datos en la fila instanciada directamente de los controles en pantalla
        Dr_Calculo_Dup["NO_CALCULO"] = Hdn_No_Calculo.Value;
        Dr_Calculo_Dup["BASE_IMPUESTO"] = Txt_Base_Impuesto.Text;
        Dr_Calculo_Dup["MINIMO_ELEVADO_ANIO"] = Txt_Minimo_Elevado_Anio.Text;
        Dr_Calculo_Dup["BASE_GRAVABLE_TRASLADO"] = Txt_Base_Gravable_Traslado.Text;
        Dr_Calculo_Dup["BASE_GRAVABLE_DIVISION"] = Txt_Base_Impuesto_Div_Lotif.Text;
        Dr_Calculo_Dup["TASA_TRASLADO"] = Txt_Tasa_Traslado_Dominio.Text;
        Dr_Calculo_Dup["TASA_DIVISION"] = Txt_Tasa_Division_Lotificacion.Text;
        Dr_Calculo_Dup["IMPUESTO_TRASLADO"] = Txt_Impuesto_Traslado_Dominio.Text;
        Dr_Calculo_Dup["IMPUESTO_DIVISION"] = Txt_Impuesto_Division_Lotificacion.Text;
        Dr_Calculo_Dup["CONSTANCIA"] = Txt_Costo_Constancia_No_Adeudo.Text;
        Dr_Calculo_Dup["RECARGOS"] = Txt_Recargos.Text;
        Dr_Calculo_Dup["MULTAS"] = Txt_Multa.Text;
        Dr_Calculo_Dup["TOTAL"] = Txt_Total.Text;
        Dr_Calculo_Dup["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Calculo_Dup["NO_ORDEN_VARIACION"] = Hdn_No_Orden.Value;
        Dr_Calculo_Dup["REALIZO_CALCULO"] = Hdn_Realizo_Calculo.Value;
        Dr_Calculo_Dup["FUNDAMENTO"] = Txt_Fundamento.Text;
        Dr_Calculo_Dup["OBSERVACIONES"] = Txt_Comentarios_Area.Text.ToUpper();
        Dr_Calculo_Dup["TANTOS_SALARIO"] = "0.00";
        if (Opt_Tipo_Avaluo_Predial.Checked)
            Dr_Calculo_Dup["TIPO"] = Opt_Tipo_Avaluo_Predial.Text;
        else if (Opt_Tipo_Valor_Fiscal.Checked)
            Dr_Calculo_Dup["TIPO"] = Opt_Tipo_Valor_Fiscal.Text;
        else if (Opt_Tipo_Valor_Operacion.Checked)
            Dr_Calculo_Dup["TIPO"] = Opt_Tipo_Valor_Operacion.Text;
        // agregar fila a la tabla
        Ds_Calculo.Tables[2].Rows.Add(Dr_Calculo_Dup);

        return Ds_Calculo;
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
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo, String Window_Fmt)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                Window_Fmt,
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Formato_Campos
    /// DESCRIPCIÓN: Dar formato a las cantidades de las cajas de texto (0.00) y poner ceros en las vacias 
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Formato_Campos()
    {
        Decimal Numero;
        List<TextBox> Cajas_Texto = new List<TextBox>();

        Cajas_Texto.Add(Txt_Base_Impuesto);
        Cajas_Texto.Add(Txt_Base_Gravable_Traslado);
        Cajas_Texto.Add(Txt_Tasa_Traslado_Dominio);
        Cajas_Texto.Add(Txt_Impuesto_Traslado_Dominio);
        Cajas_Texto.Add(Txt_Base_Impuesto_Div_Lotif);
        Cajas_Texto.Add(Txt_Tasa_Division_Lotificacion);
        Cajas_Texto.Add(Txt_Impuesto_Division_Lotificacion);
        Cajas_Texto.Add(Txt_Costo_Constancia_No_Adeudo);
        Cajas_Texto.Add(Txt_Multa);
        Cajas_Texto.Add(Txt_Recargos);
        Cajas_Texto.Add(Txt_Total);

        // recorrer la lista de cajas de texto
        foreach (TextBox Texto in Cajas_Texto)
        {
            // si se encontro numero, redondear a dos digitos y mostrar con formato
            if (Decimal.TryParse(Texto.Text, out Numero))
            {
                Texto.Text = String.Format("{0:#,##0.00}", Decimal.Round(Numero, 2));
            }
            else    // si no, mostrar cero
            {
                Texto.Text = "0.00";
            }
        }
    }
    #endregion METODOS

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
    /// NOMBRE DE LA FUNCION: Es_Numero
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

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Consulta_Calculos
    /// DESCRIPCION: Consulta los calculos que estan dados de alta en la BD
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 23-mar-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Consulta_Calculos(Boolean Sin_Calculo)
    {
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio RS_Consulta_Calculos = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión hacia la capa de Negocios
        DataTable Dt_Calculos; //Variable que obtendrá los datos de la consulta 
        Regex regexAlphaNum = new Regex("[a-zA-Z]+");

        try
        {
            if (Txt_Busqueda.Text != "")        //si no está vacío, buscar por anio y numero de calculo
            {
                // si el texto en el campo de busqueda contiene letras, buscar por cuenta predial
                if (regexAlphaNum.IsMatch(Txt_Busqueda.Text.Trim()))
                {
                    RS_Consulta_Calculos.P_Cuenta_Predial = Txt_Busqueda.Text.Trim().ToUpper();
                }
                else    // si no, buscar por numero de contrarecibo
                {
                    Int32 No_Contrarecibo;
                    Int32 Anio_Contrarecibo = DateTime.Now.Year;
                    String[] No_Contrarecibo_Anio;

                    // separar numero y anio
                    No_Contrarecibo_Anio = Txt_Busqueda.Text.Trim().Split('/');
                    // si se obtuvieron dos valores, tomar el anio del segundo valor
                    if (No_Contrarecibo_Anio.Length == 2)
                    {
                        if (Int32.TryParse(No_Contrarecibo_Anio[1], out Anio_Contrarecibo))
                        {
                            RS_Consulta_Calculos.P_Anio_Contrarecibo = Anio_Contrarecibo;
                        }
                    }
                    if (No_Contrarecibo_Anio.Length >= 1) // si hay por lo menos un valor, tomarlo para el no_calculo
                    {
                        if (Int32.TryParse(No_Contrarecibo_Anio[0], out No_Contrarecibo))
                        {
                            RS_Consulta_Calculos.P_No_Contrarecibo = No_Contrarecibo.ToString();
                        }
                    }
                    RS_Consulta_Calculos.P_Mostrar_Contrarecibos_Sin_Calculo = Sin_Calculo;
                }
                // consultar cuentas
                Dt_Calculos = RS_Consulta_Calculos.Consulta_Calculos_Contrarecibo();
                Session["Consulta_Calculos"] = Dt_Calculos;
                Llena_Grid_Calculos();
            }
            else
            {
                //RS_Consulta_Calculos.P_Estatus = "LISTO";
                //RS_Consulta_Calculos.P_Filtro_Dinamico = " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fundamento + " IS NULL";
                RS_Consulta_Calculos.P_Mostrar_Contrarecibos_Sin_Calculo = Sin_Calculo;
                Dt_Calculos = RS_Consulta_Calculos.Consulta_Calculos_Contrarecibo();
                Session["Consulta_Calculos"] = Dt_Calculos;
                Llena_Grid_Calculos();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consulta_Calculos " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Llena_Grid_Calculos
    /// DESCRIPCION: Llena el grid con los Calculos por realizar
    /// PARAMETROS: 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-ago-2011
    /// MODIFICO:
    /// FECHA_MODIFICO:
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llena_Grid_Calculos()
    {
        DataTable Dt_Calculos; //Variable que obtendrá los datos de la consulta 
        try
        {
            Grid_Calculos.DataBind();
            Dt_Calculos = (DataTable)Session["Consulta_Calculos"];
            Grid_Calculos.Columns[3].Visible = true;
            Grid_Calculos.Columns[8].Visible = true;
            Grid_Calculos.DataSource = Dt_Calculos;
            Grid_Calculos.DataBind();
            Grid_Calculos.Columns[3].Visible = false;
            Grid_Calculos.Columns[8].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llena_Grid_Calculos: " + ex.Message.ToString(), ex);
        }
    }

    #endregion (Control Acceso Pagina)


    ///********************************************************************************
    ///                                 EVENTOS
    ///********************************************************************************
    #region EVENTOS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Ope_Pre_Resolucion.aspx");
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones

            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Salir_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Salir. Ir a la página principal o 
    ///         inicializar controles 
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Salir.ToolTip == "Inicio")
            {
                // si el panel con el grid no esta visible, mostrarlo, y si esta visible, ir a la pagina principal
                if (Pnl_Grid.Visible == false)
                {
                    Inicializa_Controles();
                    //Pnl_Grid.Visible = true;
                    //Pnl_Controles.Visible = false;
                }
                else
                {
                    Session.Remove("Consulta_Partidas");
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
            else
            {
                Inicializa_Controles();//Habilita los controles para la siguiente operación del usuario
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Chk_Predio_Colindante_CheckedChanged
    /// DESCRIPCIÓN: Al cambiar de estado, quitar el monto minimo elevado anio o no considerarlo en el calculo
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Chk_Predio_Colindante_CheckedChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            if (Chk_Predio_Colindante.Checked == true)
            {
                Txt_Minimo_Elevado_Anio.Text = "0.0";
            }
            Validar_Calculo_Impuesto_Traslado();

            // si hay mensaje de error, mostrarlo
            if (Lbl_Mensaje_Error.Text.Length > 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
            }
            Txt_Base_Impuesto.Focus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Chk_Constancia_No_Adeudo_CheckedChanged
    /// DESCRIPCIÓN: Al cambiar de estado, no se considera el costo de constancia en el calculo
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Chk_Constancia_No_Adeudo_CheckedChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            if (Chk_Predio_Colindante.Checked == true)
            {
                Txt_Minimo_Elevado_Anio.Text = "0.0";
            }
            Validar_Calculo_Impuesto_Traslado();

            // si hay mensaje de error, mostrarlo
            if (Lbl_Mensaje_Error.Text.Length > 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
            }
            Txt_Base_Impuesto.Focus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Calculos_SelectedIndexChanged
    /// DESCRIPCIÓN: Consulta los datos del elemento que seleccionó el usuario y los muestra 
    ///             en los campos correspondientes
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Calculos_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Consulta_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
        DataTable Dt_Calculo; // Datatable que obtendrá los datos de la consulta a la base de datos
        Limpiar_Controles();

        try
        {
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            // si el estatus es LISTO o por pagar, mostrar el boton de impresion, si no, ocultar
            if (Grid_Calculos.SelectedRow.Cells[7].Text == "POR PAGAR" || Grid_Calculos.SelectedRow.Cells[7].Text == "LISTO")
            {
                Btn_Imprimir.Visible = true;
            }
            else
            {
                Btn_Imprimir.Visible = false;
            }
            // si el elemento seleccionado no contiene un numero de calculo, solo llenar los datos para nuevo calculo
            if (Grid_Calculos.SelectedRow.Cells[3].Text != "" || Grid_Calculos.SelectedRow.Cells[3].Text != "&nbsp;")
            {
                // si hay un folio de calculo, separar elementos y consultar por numero de calculo y anio
                String[] Folio_Calculo = Grid_Calculos.SelectedRow.Cells[3].Text.Split('/');
                if (Folio_Calculo.Length == 2)
                {
                    Int32 int_Anio_Calculo;
                    Int32 int_Folio_Calculo;
                    Decimal Costo_Constancia;
                    Int32.TryParse(Folio_Calculo[1], out int_Anio_Calculo);
                    Int32.TryParse(Folio_Calculo[0], out int_Folio_Calculo);
                    Rs_Consulta_Calculo.P_No_Calculo = String.Format("{0:0000000000}", int_Folio_Calculo);
                    Rs_Consulta_Calculo.P_Anio_Calculo = int_Anio_Calculo;
                    Dt_Calculo = Rs_Consulta_Calculo.Consulta_Detalles_Calculo();

                    if (Dt_Calculo.Rows.Count > 0)
                    {
                        Decimal Numero = 0;
                        //Escribe los valores de los campos a los controles correspondientes de la forma
                        Txt_Cuenta_Predial.Text = Dt_Calculo.Rows[0]["CUENTA_PREDIAL"].ToString();
                        Txt_Fecha_Escritura.Text = String.Format("{0:dd/MMM/yy}", Convert.ToDateTime(Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Escritura].ToString()));
                        Txt_Base_Impuesto.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto].ToString();
                        Txt_Base_Impuesto_Div_Lotif.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Base_Impuesto_Division].ToString();
                        Txt_Impuesto_Division_Lotificacion.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division].ToString();
                        Decimal.TryParse(Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia].ToString(), out Costo_Constancia);
                        Txt_Costo_Constancia_No_Adeudo.Text = Costo_Constancia.ToString("#,##0.00");
                        if (Costo_Constancia > 0)
                        {
                            Chk_Constancia_No_Adeudo.Checked = true;
                        }
                        else
                        {
                            Chk_Constancia_No_Adeudo.Checked = false;
                        }
                        Txt_Multa.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa].ToString();
                        Decimal.TryParse(Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Minimo_Elevado_Anio].ToString(), out Numero);
                        Txt_Minimo_Elevado_Anio.Text = Numero.ToString("#,##0.00");
                        // obtener impuesto traslado
                        Validar_Calculo_Impuesto_Traslado();
                        Txt_Recargos.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos].ToString();
                        Txt_Total.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar].ToString();
                        Txt_Fundamento.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Fundamento].ToString();
                        if (Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Tipo].ToString() == "VALOR FISCAL")
                        {
                            Opt_Tipo_Valor_Fiscal.Checked = true;
                        }
                        else if (Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Tipo].ToString() == "VALOR OPERACION")
                        {
                            Opt_Tipo_Valor_Operacion.Checked = true;
                        }
                        else if (Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Tipo].ToString() == "AVALUO PREDIAL")
                        {
                            Opt_Tipo_Avaluo_Predial.Checked = true;
                        }

                        Hdn_No_Calculo.Value = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo].ToString();
                        Hdn_Anio_Orden.Value = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden].ToString();
                        Hdn_Anio_Calculo.Value = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo].ToString();
                        Hdn_No_Orden.Value = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion].ToString();
                        Hdn_Tasa_Div_Lotif_ID.Value = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Impuesto_Division_Lot_Id].ToString();
                        Hdn_Tasa_Traslado_ID.Value = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Tasa_ID].ToString();
                        Hdn_Realizo_Calculo.Value = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Realizo_Calculo].ToString();
                        Hdn_Cuenta_Predial_ID.Value = Grid_Calculos.SelectedRow.Cells[8].Text;
                        Consultar_Tasas_Division(Hdn_Tasa_Div_Lotif_ID.Value);
                        Consultar_Tasas_Traslado_Por_ID(Hdn_Tasa_Traslado_ID.Value);
                        Txt_Folio_Pago.Text = "TD" + Convert.ToInt32(Hdn_No_Calculo.Value) + Hdn_Anio_Calculo.Value;

                        Chk_Predio_Colindante.Checked = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Predio_Colindante].ToString() == "SI" ? true : false;
                        Txt_Estatus.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus].ToString();

                        // activar campos fundamento y comentarios, si el estatus es = a LISTO
                        if (Txt_Estatus.Text == "LISTO" || Txt_Estatus.Text == "POR PAGAR")
                        {
                            Txt_Fundamento.Enabled = true;
                            Txt_Comentarios_Area.Enabled = true;
                            Opt_Formato_Oficial.Enabled = true;
                            Opt_Formato_Notario.Enabled = true;
                        }
                        else
                        {
                            Txt_Fundamento.Enabled = false;
                            Txt_Comentarios_Area.Enabled = false;
                            Opt_Formato_Oficial.Enabled = false;
                            Opt_Formato_Notario.Enabled = false;
                        }

                        // si el estatus seleccionado es LISTO o POR PAGAR, habilitar boton convenio
                        if (Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus].ToString() == "LISTO"
                            || Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus].ToString() == "POR PAGAR")
                        {
                            Btn_Convenio.Enabled = true;
                        }
                        else
                        {
                            Btn_Convenio.Enabled = false;
                        }

                        Contenedor_Observaciones_Anteriores.InnerHtml = "";
                        foreach (DataRow Registro in Dt_Calculo.Rows)
                        {
                            Contenedor_Observaciones_Anteriores.InnerHtml +=
                                "<table id=\"Tbl_Observaciones_Anteriores\" width=\"94%\" border=\"1\" cellspacing=\"0\" runat=\"server\" class=\"Tabla_Comentarios\" style=\"text-transform:uppercase\">";
                            Contenedor_Observaciones_Anteriores.InnerHtml +=
                                "<tr> <th style=\"width:25%;\">";
                            Contenedor_Observaciones_Anteriores.InnerHtml +=
                                String.Format("{0:dd/MMM/yyyy h:mm:ss tt}", Registro[Ope_Pre_Calc_Imp_Tras_Det.Campo_Fecha_Hora]);
                            Contenedor_Observaciones_Anteriores.InnerHtml += "</th><th>";
                            Contenedor_Observaciones_Anteriores.InnerHtml +=
                                Registro[Ope_Pre_Calc_Imp_Tras_Det.Campo_Realizo_Observacion].ToString();
                            Contenedor_Observaciones_Anteriores.InnerHtml += "</th></tr><tr><td colspan='2'>";
                            Contenedor_Observaciones_Anteriores.InnerHtml +=
                                Registro[Ope_Pre_Calc_Imp_Tras_Det.Campo_Observaciones].ToString();
                            Contenedor_Observaciones_Anteriores.InnerHtml += "</td></tr>";
                            Contenedor_Observaciones_Anteriores.InnerHtml += "</table><br />";
                        }
                        if (Dt_Calculo.Rows[0][Ope_Pre_Calc_Imp_Tras_Det.Campo_Observaciones].ToString() != "")
                        {
                            Contenedor_Observaciones_Anteriores.Style.Value = "display:block;";
                            Encabezado_Observaciones_Anteriores.Style.Value = "color: #25406D;display:block;";
                        }
                        else
                        {
                            Contenedor_Observaciones_Anteriores.Style.Value = "display:none;";
                            Encabezado_Observaciones_Anteriores.Style.Value = "display:none;";
                        }
                        Validar_Calculo_Impuesto_Traslado();
                    }
                }
                Formato_Campos();
                Btn_Imprimir.Visible = true;
                Btn_Convenio.Visible = true;
            }
            // ocultar el panel que contiene el grid
            Pnl_Grid.Visible = false;
            Pnl_Controles.Visible = true;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Grid_Calculos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[7].Text != "POR PAGAR" && e.Row.Cells[7].Text != "LISTO" && e.Row.Cells[7].Text != "PAGADO")
                {
                    e.Row.Cells[0].Enabled = false;
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Calculos_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento de paginación del grid (cargar los datos de la página seleccionada)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Calculos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            //Limpiar_Controles(); //Limpia todos los controles de la forma
            Grid_Calculos.PageIndex = e.NewPageIndex; //Indica la Página a visualizar
            Llena_Grid_Calculos(); //Carga los elementos que están asignados a la página seleccionada
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el boton buscar calculo
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Consulta_Calculos(false); //Método que consulta los elementos en la base de datos
            }
            else
            {
                Consulta_Calculos(true); //Método que consulta los elementos en la base de datos
            }
            Limpiar_Controles(); //Limpia los controles de la forma
            // ocultar el panel que contiene los controles y mostrar el que contiene el grid
            Pnl_Grid.Visible = true;
            Pnl_Controles.Visible = false;
            //Si no se encontraron Cálculos con un nombre similar al proporcionado por el usuario entonces manda un mensaje al usuario
            Btn_Salir.ToolTip = "Regresar";
            if (Grid_Calculos.Rows.Count <= 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se encontraron Cálculos con el criterio proporcionado<br />";
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Btn_Convenio_Click(object sender, ImageClickEventArgs e)
    {

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
        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Rs_Convenio_Traslado = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();

        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // verificar que hay un calculo seleccionado
        if (Hdn_No_Calculo.Value != "")
        {
            // verificar que el calculo tiene estatus LISTO o POR PAGAR
            if (Grid_Calculos.SelectedRow.Cells[7].Text == "LISTO" || Grid_Calculos.SelectedRow.Cells[7].Text == "POR PAGAR")
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Validar_Campos();

                //Si faltaron campos por capturar envia un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
                }
                else // si no faltan parametros, imprimir
                {
                    DataTable Dt_Convenios;
                    // formar consulta de convenios
                    Rs_Convenio_Traslado.P_Mostrar_Ultimo_Convenio = false;
                    Rs_Convenio_Traslado.P_Campos_Foraneos = false;
                    Rs_Convenio_Traslado.P_Campos_Dinamicos = Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio;
                    Rs_Convenio_Traslado.P_Filtros_Dinamicos = Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + " = '"
                        + Hdn_No_Calculo.Value + "' AND "
                        + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " = 'VIGENTE' ";
                    // verificar si el calculo tiene convenio
                    Dt_Convenios = Rs_Convenio_Traslado.Consultar_Convenio_Traslado_Dominio();


                    try
                    {
                        // si la consulta no regresa valores, no existe convenio, imprimir folio y formato
                        if (Dt_Convenios.Rows.Count <= 0)
                        {
                            // llamar metodo impresion oficial o notario dependiendo de seleccion
                            // -- los nombres de los archivos rpt quedaron al reves
                            if (Opt_Formato_Oficial.Checked == true)
                            {
                                // llamar al metodo modificar con Insertar_Pasivo == true
                                Modificar_Calculo(true);
                                Imprimir_Reporte(Crear_Ds_Calculo_Impuesto_Traslado(),
                                    "Rpt_Pre_Calculo_Impuesto_Traslado.rpt",
                                    "CALCULO_IMPUESTO_TRASLADO",
                                    "Rpt_Pre_Calculo_Impuesto_Traslado_Notario.rpt",
                                    "FORMATO_OFICIAL");
                            }
                            else if (Opt_Formato_Notario.Checked == true)
                            {
                                // llamar al metodo modificar con Insertar_Pasivo == true
                                Modificar_Calculo(true);
                                Imprimir_Reporte(Crear_Ds_Calculo_Impuesto_Traslado(),
                                    "Rpt_Pre_Calculo_Impuesto_Traslado.rpt",
                                    "CALCULO_IMPUESTO_TRASLADO",
                                    "Rpt_Pre_Calculo_Impuesto_Traslado_Oficial.rpt",
                                    "FORMATO_NOTARIO");
                            }
                        }
                        else // si existe convenio para el calculo, imprimir solo el formato
                        {
                            // llamar metodo impresion oficial o notario dependiendo de seleccion
                            // -- los nombres de los archivos rpt quedaron al reves
                            if (Opt_Formato_Oficial.Checked == true)
                            {
                                // llamar al metodo modificar con Insertar_Pasivo == true
                                Modificar_Calculo(false);
                                Imprimir_Reporte(Crear_Ds_Calculo_Impuesto_Traslado(),
                                    "Rpt_Pre_Calculo_Impuesto_Traslado_Notario.rpt",
                                    "FORMATO_OFICIAL");
                            }
                            else if (Opt_Formato_Notario.Checked == true)
                            {
                                // llamar al metodo modificar con Insertar_Pasivo == true
                                Modificar_Calculo(false);
                                Imprimir_Reporte(Crear_Ds_Calculo_Impuesto_Traslado(),
                                    "Rpt_Pre_Calculo_Impuesto_Traslado_Oficial.rpt",
                                    "FORMATO_NOTARIO");
                            }
                            // indicar motivo por el que se imprime solo el formato
                            Lbl_Mensaje_Error.Visible = true;
                            Lbl_Mensaje_Error.Text = "Sólo se imprime formato porque el cálculo tiene convenio";

                        }

                    }
                    catch (Exception Ex)
                    {
                        Lbl_Mensaje_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = Ex.Message;
                    }

                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Sólo se pueden imprimir cálculos con estatus: LISTO<br />";
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Seleccione el cálculo que desea imprimir<br />";
        }
    }


    #endregion EVENTOS


}
