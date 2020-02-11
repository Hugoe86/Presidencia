using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Catalogo_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Divisiones.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Resolucion : System.Web.UI.Page
{


    ///********************************************************************************
    ///                                 METODOS
    ///********************************************************************************

    #region METODOS
    
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
        try
        {
            switch (Operacion)
            {
                case "Inicial":
                    Txt_Estatus.Enabled = false;
                    Txt_Folio_Pago.ReadOnly = true;
                    Txt_Fundamento.Enabled = false;
                    break;

            }

            // Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Folio_Pago.Enabled = false;
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
            Txt_Multa.Enabled = false;
            Txt_Recargos.Enabled = false;
            Txt_Total.Enabled = false;
            Txt_Fundamento.Enabled = false;
            Txt_Comentarios_Area.Enabled = false;
            Opt_Tipo_Avaluo_Predial.Enabled = false;
            Opt_Tipo_Valor_Fiscal.Enabled = false;
            Opt_Tipo_Valor_Operacion.Enabled = false;
            Chk_Constancia_No_Adeudo.Enabled = false;
            Lbl_Mensaje_Error.Visible = false;
            IBtn_Imagen_Error.Visible = false;
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


    ///********************************************************************************
    ///                                 EVENTOS
    ///********************************************************************************
    #region EVENTOS
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                //Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Habilitar_Controles("Inicial");
                Cargar_Detalle_Calculo();
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;
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
        IBtn_Imagen_Error.Visible = false;

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
                IBtn_Imagen_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
            }
            Txt_Base_Impuesto.Focus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;
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
        IBtn_Imagen_Error.Visible = false;

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
                IBtn_Imagen_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
            }
            Txt_Base_Impuesto.Focus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    private void Cargar_Detalle_Calculo()
    {
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Consulta_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión a la capa de Negocios para la consulta de los datos
        DataTable Dt_Calculo; // Datatable que obtendrá los datos de la consulta a la base de datos
        Limpiar_Controles();

        try
        {
            Lbl_Mensaje_Error.Text = "";
            Lbl_Mensaje_Error.Visible = false;
            IBtn_Imagen_Error.Visible = false;
            // si el estatus es LISTO o por pagar, mostrar el boton de impresion, si no, ocultar
            // si el elemento seleccionado no contiene un numero de calculo, solo llenar los datos para nuevo calculo
            // si hay un folio de calculo, separar elementos y consultar por numero de calculo y anio
            String No_Calculo = Request.QueryString["No_Calculo"].ToString();// Session["NO_CALCULO"].ToString();
            String Cuenta_Predial_ID = Request.QueryString["Cuenta_Predial_ID"].ToString();// Session["CUENTA_PREDIAL_ID"].ToString();
            String Cuenta_Predial = Request.QueryString["Cuenta_Predial"].ToString();// Session["CUENTA_PREDIAL"].ToString();
            Int16 Año = Convert.ToInt16(Request.QueryString["Año"]);// Convert.ToInt16(Session["AÑO"].ToString());
            if (Cuenta_Predial.Trim() != "")
            {
                Int32 Anio_Calculo;
                Decimal Consto_Constancia;
                Rs_Consulta_Calculo.P_No_Calculo = No_Calculo;
                //Rs_Consulta_Calculo.P_Anio_Calculo = Anio_Calculo;
                Rs_Consulta_Calculo.P_Cuenta_Predial = Cuenta_Predial_ID;
                Rs_Consulta_Calculo.P_Anio_Calculo = Año;
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
                    Decimal.TryParse(Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia].ToString(), out Consto_Constancia);
                    Txt_Costo_Constancia_No_Adeudo.Text = Consto_Constancia.ToString("#,##0.00");
                    if (Consto_Constancia > 0)
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
                    Hdn_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
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
                    }
                    else
                    {
                        Txt_Fundamento.Enabled = false;
                        Txt_Comentarios_Area.Enabled = false;
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
            Pnl_Controles.Visible = true;
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion EVENTOS
}
