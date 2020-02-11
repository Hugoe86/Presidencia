using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Divisiones.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using System.Data.OracleClient;
using Presidencia.Operacion_Predial_Generar_Adeudo_Folio.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Validacion_Calculo_Traslado : System.Web.UI.Page
{

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
            Consulta_Calculos();
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
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Modificar.Visible = false;
                    Btn_Modificar.CausesValidation = false;
                    Opt_Listo.Enabled = false;
                    Opt_Rechazado.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Lbl_Campo_Observaciones.Text = "Observaciones";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Opt_Listo.Enabled = true;
                    Opt_Rechazado.Enabled = true;
                    Btn_Modificar.Visible = true;
                    Btn_Modificar.CausesValidation = true;
                    Lbl_Campo_Observaciones.Text = "*Observaciones";
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Cuenta_Predial.Enabled = false;
            Chk_Predio_Colindante.Enabled = false;
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
            Chk_Constancia_No_Adeudo.Enabled = false;
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

            // limpiar y ocultar campos observaciones de seguimiento
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
        //Decimal Monto_Total;

        if (Txt_Cuenta_Predial.Text == "")  //Validar campo NOMBRE (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la cuenta predial <br />";
        }
        else if (Txt_Tasa_Division_Lotificacion.Text.Length > 0 && Txt_Base_Impuesto_Div_Lotif.Text.Length < 0)  // que si hay tasa de division haya base del impuesto de division
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la Base de impuesto para división o lotificación<br />";
        }
        //if (Decimal.TryParse(Txt_Total.Text, out Monto_Total))  //Validar campo DESCRIPCION (longitud menor a 250)
        //{
        //    if (Monto_Total <= 0)
        //    {
        //        Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir parámetros para completar el cálculo<br />";
        //    }
        //}
        if (Txt_Fecha_Escritura.Text.Length > 250)  //Validar campo DESCRIPCION (longitud menor a 250)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la fecha de la escritura<br />";
        }
        if (Opt_Tipo_Avaluo_Predial.Checked == false && Opt_Tipo_Valor_Fiscal.Checked == false && Opt_Tipo_Valor_Operacion.Checked == false)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccioanr un Tipo.<br />";
        }
        if (Opt_Listo.Checked == false)
        {
            if (Lbl_Campo_Observaciones.Text == "*Observaciones" && Txt_Comentarios_Area.Text.Length <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir observaciones de la modificación.<br />";
            }
        }
        //// si se actualiza un registro, solo permite LISTO o CANCELADO
        //if (Btn_Modificar.ToolTip == "Actualizar")
        //{
        //    //if (Cmb_Estatus.SelectedValue == "CALCULADO")
        //    //{
        //    //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar estatus LISTO, RECHAZADO o CANCELADO.<br />";
        //    //}
        //}
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
    /// DESCRIPCIÓN: Modifica los datos del calculo
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ago-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Modificar_Calculo()
    {
        Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio Rs_Generar_Adeudo_Folio = new Cls_Ope_Pre_Generar_Adeudo_Folio_Negocio();
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar

        OracleConnection Cn = new OracleConnection();
        OracleCommand Cmd = new OracleCommand();
        OracleTransaction Trans = null;

        Boolean Alta_Exitosa;
        Int32 Numero_Afectaciones;

        try
        {
            // crear transaccion para modificar tabla de calculos y de adeudos folio
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            // si el estatus cambia a listo, generar el adeudo
            if (Opt_Listo.Checked == true )
            {
                // datos de adeudo
                Rs_Generar_Adeudo_Folio.P_Folio = "TD" + Convert.ToInt32(Hdn_No_Calculo.Value) + Hdn_Anio_Calculo.Value;
                Rs_Generar_Adeudo_Folio.P_Fecha = DateTime.Now;
                Rs_Generar_Adeudo_Folio.P_Monto = Convert.ToDouble(Txt_Total.Text);
                Rs_Generar_Adeudo_Folio.P_Concepto = "TRASLADO DE DOMINIO";
                //Rs_Generar_Adeudo_Folio.P_Estatus = "POR PAGAR";
                Rs_Generar_Adeudo_Folio.P_Cuenta_Predial_ID = Hdn_Cuenta_Predial_ID.Value;
                Rs_Generar_Adeudo_Folio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Rs_Generar_Adeudo_Folio.P_Cmd_Adeudo = Cmd;
                // llamar metodo para generar adeudo
                Alta_Exitosa = Rs_Generar_Adeudo_Folio.Alta_Adeudo();
                // datos del calculo
                Rs_Modificar_Calculo.P_Numero_Adeudo = Rs_Generar_Adeudo_Folio.P_No_Adeudo;
                Rs_Modificar_Calculo.P_Cmd_Calculo = Cmd;
            }
            Rs_Modificar_Calculo.P_No_Calculo = Hdn_No_Calculo.Value;
            Rs_Modificar_Calculo.P_Observaciones = Txt_Comentarios_Area.Text.ToUpper();
            Rs_Modificar_Calculo.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            Rs_Modificar_Calculo.P_Anio_Calculo = Convert.ToInt32(Hdn_Anio_Calculo.Value);
            if (Opt_Listo.Checked == false){
                Rs_Modificar_Calculo.P_Estatus = "RECHAZADO";
            }
            else
            {
                Rs_Modificar_Calculo.P_Estatus = "LISTO";
                Rs_Modificar_Calculo.Actualizar_Estatus_Contrarecibo();
            }
            // llamar metodos para afectar calculo
            Numero_Afectaciones = Rs_Modificar_Calculo.Actualizar_Estatus_Calculo();

            Trans.Commit();
            Cn.Close();

            if (Numero_Afectaciones > 0) //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            {
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "Cálculo de traslado de dominio ", "alert('La modificación del Cálculo fue Exitosa');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(),
                    "Cálculo de traslado de dominio ", "alert('Ocurrió un error y el Cálculo no se modificó');", true);
            }
        }
        catch (OracleException Ex)
        {
            Trans.Rollback();
            throw new Exception("Modificar_Calculo: " + Ex.Message.ToString(), Ex);
        }
        catch (Exception Ex)
        {

            throw new Exception("Modificar_Calculo: " + Ex.Message.ToString(), Ex);
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
    private void Consulta_Calculos()
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
                }
                // consultar cuentas
                RS_Consulta_Calculos.P_Estatus = "CALCULADO";
                Dt_Calculos = RS_Consulta_Calculos.Consulta_Calculos_Contrarecibo();
                Session["Consulta_Calculos"] = Dt_Calculos;
                Llena_Grid_Calculos();
            }
            else
            {
                RS_Consulta_Calculos.P_Estatus = "CALCULADO";
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
            Grid_Calculos.Columns[7].Visible = true;
            Grid_Calculos.Columns[8].Visible = true;
            Grid_Calculos.Columns[9].Visible = true;
            Grid_Calculos.DataSource = Dt_Calculos;
            Grid_Calculos.DataBind();
            Grid_Calculos.Columns[3].Visible = false;
            Grid_Calculos.Columns[7].Visible = false;
            Grid_Calculos.Columns[8].Visible = false;
            Grid_Calculos.Columns[9].Visible = false;
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
                Configuracion_Acceso("Frm_Ope_Pre_Validacion_Calculo_Traslado.aspx");
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
    /// NOMBRE_FUNCIÓN: Txt_Base_Impuesto_TextChanged
    /// DESCRIPCIÓN: Al cambiar el texto, llama al metodo que calcula el impuesto de traslado
    /// PARAMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Base_Impuesto_TextChanged(object sender, EventArgs e)
    {
        Decimal Base;
        Decimal.TryParse(Txt_Base_Impuesto.Text, out Base);
        Txt_Base_Impuesto.Text = String.Format("{0:#,###,##0.00}", Base);

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Validar_Calculo_Impuesto_Traslado();

            if (Lbl_Mensaje_Error.Text.Length > 0)
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
            }
            Txt_Minimo_Elevado_Anio.Focus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Modificar_Click
    /// DESCRIPCIÓN: Manejo del evento Click para el control Btn_Modificar. Validar los datos en los campos 
    ///         antes de enviar
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            //si se dio clic en el botón Modificar, revisar que haya un elemento seleccionado, si no mostrar mensaje
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                // verificar que el cálculo no haya sido pagado
                if (Grid_Calculos.SelectedRow.Cells[9].Text == "PAGADO")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No es podible modificar el cálculo porque ya se pagó<br />";
                }
                else if(Hdn_No_Calculo.Value != "")
                {
                    Habilitar_Controles("Modificar");   // Habilita los controles para la modificación de los datos
                    Txt_Base_Gravable_Traslado.Focus();
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el cálculo cuyos datos desea modificar<br />";
                }
            }
            ///Si se da clic en el botón y el tooltip  es Actualizar, verificar la validez de los campos y enviar 
            ///los cambios o mostrar los mensajes de error correspondientes
            else
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
                //Si todos los campos requeridos fueron proporcionados por el usuario entonces actualizar los mismo en la base de datos
                else
                {
                    Modificar_Calculo(); //Actualizar los datos de la partida
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
                    Pnl_Grid.Visible = true;
                    Pnl_Controles.Visible = false;
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
                Txt_Minimo_Elevado_Anio.Text = "0";
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
                Txt_Minimo_Elevado_Anio.Text = "0";
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
        Decimal Monto = 0;
        Limpiar_Controles();

        try
        {
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            // si el elemento seleccionado no contiene un numero de calculo, solo llenar los datos para nuevo calculo
            if (Grid_Calculos.SelectedRow.Cells[3].Text != "" || Grid_Calculos.SelectedRow.Cells[3].Text != "&nbsp;")
            {
                // si hay un folio de calculo, separar elementos y consultar por numero de calculo y anio
                String[] Folio_Calculo = Grid_Calculos.SelectedRow.Cells[3].Text.Split('/');
                if (Folio_Calculo.Length == 2)
                {
                    Int32 Anio_Calculo;
                    Decimal Costo_Constancia;
                    Int32.TryParse(Folio_Calculo[1], out Anio_Calculo);
                    Rs_Consulta_Calculo.P_No_Calculo = String.Format("{0:0000000000}", Convert.ToInt32(Folio_Calculo[0]));
                    Rs_Consulta_Calculo.P_Anio_Calculo = Anio_Calculo;
                    Dt_Calculo = Rs_Consulta_Calculo.Consulta_Detalles_Calculo();

                    if (Dt_Calculo.Rows.Count > 0)
                    {
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
                        Decimal.TryParse(Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Minimo_Elevado_Anio].ToString(), out Monto);
                        Txt_Minimo_Elevado_Anio.Text = Monto.ToString("#,##0.00");
                        // obtener impuesto traslado
                        Validar_Calculo_Impuesto_Traslado();
                        Txt_Recargos.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos].ToString();
                        Txt_Total.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar].ToString();
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
                        Hdn_Cuenta_Predial_ID.Value = Grid_Calculos.SelectedRow.Cells[8].Text;
                        Consultar_Tasas_Division(Hdn_Tasa_Div_Lotif_ID.Value);
                        Consultar_Tasas_Traslado_Por_ID(Hdn_Tasa_Traslado_ID.Value);

                        Chk_Predio_Colindante.Checked = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Predio_Colindante].ToString() == "SI" ? true : false;
                        String Estatus = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus].ToString().ToUpper();
                        if (Estatus.Equals("LISTO"))
                        {
                            Opt_Listo.Checked = true;
                            Opt_Rechazado.Checked = false; 
                        }
                        else
                        {
                            Opt_Listo.Checked = false;
                            Opt_Rechazado.Checked = true; 
                        }

                        Contenedor_Observaciones_Anteriores.InnerHtml = "";
                        foreach (DataRow Registro in Dt_Calculo.Rows)
                        {
                            Contenedor_Observaciones_Anteriores.InnerHtml +=
                                "<table id=\"Tbl_Observaciones_Anteriores\" width=\"94%\" border=\"1\" cellspacing=\"0\" runat=\"server\" class=\"Tabla_Comentarios\">";
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
                Btn_Modificar.Visible = true;
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

    #endregion EVENTOS

    protected void Txt_Minimo_Elevado_Anio_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Txt_Tasa_Traslado_Dominio_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Txt_Base_Gravable_Traslado_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Txt_Tasa_Division_Lotificacion_TextChanged(object sender, EventArgs e)
    {

    }
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
            Consulta_Calculos(); //Método que consulta los elementos en la base de datos
            Limpiar_Controles(); //Limpia los controles de la forma
            // ocultar el panel que contiene los controles y mostrar el que contiene el grid
            Pnl_Grid.Visible = true;
            Pnl_Controles.Visible = false;
            // Si no se encontraron registros 
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
}
