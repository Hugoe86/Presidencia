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
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Divisiones.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using Presidencia.Catalogo_Multas.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Catalogo_Recargos_Traslado.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Calculo_Impuesto_Traslado : System.Web.UI.Page
{
    private Boolean Bnd_Calcular_Recargos = true;

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
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = false;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    //Chk_Constancia_No_Adeudo.Checked = true;
                    //Cmb_Estatus.Enabled = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    //Lbl_Campo_Observaciones.Text = "Observaciones";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Chk_Constancia_No_Adeudo.Checked = true;
                    Cmb_Estatus.Enabled = false;
                    Cmb_Estatus.SelectedValue = "CALCULADO";
                    Txt_Cuenta_Predial.Enabled = false;
                    Txt_Minimo_Elevado_Anio.Enabled = false;
                    Txt_Base_Gravable_Traslado.Enabled = false;
                    Txt_Tasa_Traslado_Dominio.Enabled = false;
                    Txt_Impuesto_Traslado_Dominio.Enabled = false;
                    Txt_Fecha_Escritura.Enabled = false;
                    //Chk_Predio_Colindante.Enabled = false;
                    Txt_Tasa_Division_Lotificacion.Enabled = false;
                    Txt_Impuesto_Division_Lotificacion.Enabled = false;
                    Txt_Costo_Constancia_No_Adeudo.Enabled = false;
                    Txt_Multa.Enabled = false;
                    Txt_Recargos.Enabled = false;
                    Txt_Total.Enabled = false;
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    //Lbl_Campo_Observaciones.Text = "Observaciones";

                    Cmb_Estatus.SelectedValue = "CALCULADO";
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    //Chk_Constancia_No_Adeudo.Checked = true;
                    Cmb_Estatus.Enabled = true;
                    Cmb_Estatus.SelectedValue = "CALCULADO";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    //Lbl_Campo_Observaciones.Text = "*Observaciones";
                    Cmb_Estatus.SelectedValue = "CALCULADO";
                    break;
            }

            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Cuenta_Predial.Enabled = false;
            Chk_Predio_Colindante.Enabled = Habilitado;
            Txt_Base_Impuesto.Enabled = Habilitado;
            Txt_Minimo_Elevado_Anio.Enabled = Habilitado;
            Txt_Base_Gravable_Traslado.Enabled = false;
            Txt_Tasa_Traslado_Dominio.Enabled = false;
            Txt_Impuesto_Traslado_Dominio.Enabled = false;
            Txt_Fecha_Escritura.Enabled = Habilitado;
            Txt_Tipo_Division_Lotificacion.Enabled = false;
            Txt_Base_Impuesto_Div_Lotif.Enabled = Habilitado;
            Txt_Tasa_Division_Lotificacion.Enabled = false;
            Txt_Impuesto_Division_Lotificacion.Enabled = false;
            // el costo de constancias no se edita
            Txt_Costo_Constancia_No_Adeudo.Enabled = false;
            Chk_Constancia_No_Adeudo.Enabled = Habilitado;
            Txt_Multa.Enabled = false;
            Txt_Recargos.Enabled = Habilitado;
            Txt_Total.Enabled = false;
            Txt_Comentarios_Area.Enabled = Habilitado;
            Opt_Tipo_Avaluo_Predial.Enabled = Habilitado;
            Opt_Tipo_Valor_Fiscal.Enabled = Habilitado;
            Opt_Tipo_Valor_Operacion.Enabled = Habilitado;

            Btn_Resumen_Predio.Enabled = Habilitado;
            Btn_Tasas_Traslasdo.Enabled = Habilitado;
            Btn_Tasa_Division.Enabled = Habilitado;
            Btn_Quitar_Division.Enabled = Habilitado;
            Btn_Multas.Enabled = Habilitado;

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

            // limpiar sesion de resumen de predio
            Session["Cuenta_Predial"] = null;
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
        Decimal Monto_Total;
        decimal Impuesto_Traslado;
        decimal Impuesto_División;
        decimal Base_Traslado;
        decimal Base_División;

        decimal.TryParse(Txt_Impuesto_Traslado_Dominio.Text, out Impuesto_Traslado);
        decimal.TryParse(Txt_Impuesto_Division_Lotificacion.Text, out Impuesto_División);
        decimal.TryParse(Txt_Impuesto_Traslado_Dominio.Text, out Base_Traslado);
        decimal.TryParse(Txt_Impuesto_Division_Lotificacion.Text, out Base_División);

        if (Txt_Cuenta_Predial.Text == "")  //Validar campo cuenta predial (no vacío)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la cuenta predial <br />";
        }
        if (Decimal.TryParse(Txt_Total.Text, out Monto_Total))  //Validar campo TOTAL
        {
            if (Monto_Total <= 0)
            {
                Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir parámetros para completar el cálculo<br />";
            }
        }
        if (Txt_Fecha_Escritura.Text.Length <= 0)  //Validar campo FECHA_ESCRITURA
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir la fecha de la escritura<br />";
        }
        if (Opt_Tipo_Avaluo_Predial.Checked == false && Opt_Tipo_Valor_Fiscal.Checked == false && Opt_Tipo_Valor_Operacion.Checked == false)
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un Tipo.<br />";
        }
        if (Hdn_Tasa_Traslado_ID.Value.Length == 0)  // Validar impuesto seleccionado
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar un mínimo al año y tasa para traslado<br />";
        }
        else if (Impuesto_División < 0 || Impuesto_Traslado < 0)  // Validar impuesto calculado
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Agregar un cálculo de traslado o de división<br />";
        }
        if (Base_División < 0 || Base_Traslado < 0)  // Validar impuesto calculado
        {
            Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir un monto base para traslado o división<br />";
        }
        //if (Lbl_Campo_Observaciones.Text == "*Observaciones" && Txt_Comentarios_Area.Text.Length <= 0)
        //{
        //    Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir observaciones de la modificación.<br />";
        //}
        // se se actualiza un registro, solo permite CALCULADO o CANCELADO
        if (Btn_Modificar.ToolTip == "Actualizar")
        {
            if (Cmb_Estatus.SelectedValue != "CALCULADO" && Cmb_Estatus.SelectedValue != "CANCELADO")
            {
                Lbl_Mensaje_Error.Text += "&nbsp; &nbsp; &nbsp; &nbsp; + Seleccionar estatus CALCULADO o CANCELADO.<br />";
            }
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
        Decimal Base_Division = 0;

        Decimal.TryParse(Txt_Base_Impuesto_Div_Lotif.Text, out Base_Division);
        if (Txt_Base_Impuesto_Div_Lotif.Text.Length <= 0 || Base_Division <= 0)
        {
            // si no hay base de impuesto para division copiar la base de traslado
            Txt_Base_Impuesto_Div_Lotif.Text = Txt_Base_Impuesto.Text;
        }

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
                Minimo_Elevado_Al_Anio = 0;
                Txt_Minimo_Elevado_Anio.Text = Minimo_Elevado_Al_Anio.ToString("#,##0.00");
            }
            // si se obtuvieron valores en la consulta, mostrar montos redondeados
            if (Base_Gravable > 0 && Total_Impuesto_Traslado > 0)
            {
                Txt_Base_Gravable_Traslado.Text = Decimal.Round(Base_Gravable, 2).ToString("#,##0.00");
                Txt_Impuesto_Traslado_Dominio.Text = Decimal.Round(Total_Impuesto_Traslado, 2).ToString("#,##0.00");
            }
            else    // si no, limpiar campos
            {
                Txt_Base_Gravable_Traslado.Text = "";
                Txt_Impuesto_Traslado_Dominio.Text = "";
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
        Cajas_Texto.Add(Txt_Minimo_Elevado_Anio);
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

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Alta_Calculo
    /// DESCRIPCIÓN: Da de alta un Calculo en la base de datos a través de la capa de negocio
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Calculo()
    {
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Alta_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Rs_Alta_Calculo.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
            Rs_Alta_Calculo.P_Cuenta_Predial_ID = Hdn_Cuenta_Predial_ID.Value;
            if (Txt_Comentarios_Area.Text.Length > 0)
            {
                Rs_Alta_Calculo.P_Observaciones = Txt_Comentarios_Area.Text.ToUpper();
            }
            Rs_Alta_Calculo.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Alta_Calculo.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            Rs_Alta_Calculo.P_Predio_Colindante = Chk_Predio_Colindante.Checked ? "SI" : "NO";
            Rs_Alta_Calculo.P_Base_Impuesto = Txt_Base_Impuesto.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Minimo_Elevado_Anio = Txt_Minimo_Elevado_Anio.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Base_Impuesto_Division = Txt_Base_Impuesto_Div_Lotif.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Tasas_ID = Hdn_Tasa_Traslado_ID.Value;
            Rs_Alta_Calculo.P_No_Orden_Variacion = Hdn_No_Orden.Value;
            Rs_Alta_Calculo.P_Anio_Orden = Int32.Parse(Hdn_Anio_Orden.Value);
            Rs_Alta_Calculo.P_Tasas_ID = Hdn_Tasa_Traslado_ID.Value;
            Rs_Alta_Calculo.P_Impuesto_Div_Lot = Hdn_Tasa_Div_Lotif_ID.Value;
            Rs_Alta_Calculo.P_Fecha_Escritura = Txt_Fecha_Escritura.Text;
            Rs_Alta_Calculo.P_Costo_Constancia = Txt_Costo_Constancia_No_Adeudo.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Monto_Traslado = Txt_Impuesto_Traslado_Dominio.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Monto_Division = Txt_Impuesto_Division_Lotificacion.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Monto_Multa = Txt_Multa.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Monto_Recargos = Txt_Recargos.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Monto_Total_Pagar = Txt_Total.Text.Replace(",", "");
            Rs_Alta_Calculo.P_Anio_Calculo = DateTime.Now.Year;


            if (Opt_Tipo_Valor_Fiscal.Checked == true)
            {
                Rs_Alta_Calculo.P_Tipo = "VALOR FISCAL";
            }
            else if (Opt_Tipo_Valor_Operacion.Checked == true)
            {
                Rs_Alta_Calculo.P_Tipo = "VALOR OPERACION";
            }
            else if (Opt_Tipo_Avaluo_Predial.Checked == true)
            {
                Rs_Alta_Calculo.P_Tipo = "AVALUO PREDIAL";
            }


            if (Rs_Alta_Calculo.Alta_Calculo() > 0) //Da de alta los datos del Calculo proporcionados por el usuario en la BD
            {
                Rs_Alta_Calculo.Actualizar_Estatus_Contrarecibo();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "C&aacute;lculo de impuesto de traslado", "alert('El Alta del Cálculo fue Exitosa');", true);
                Inicializa_Controles();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "C&aacute;lculo de impuesto de traslado", "alert('Ocurrió un error y el Cálculo no se dio de alta');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Calculo " + Ex.Message.ToString(), Ex);
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
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio(); //Variable de conexión hacia la capa de Negoccios para envio de datos a modificar
        try
        {
            Rs_Modificar_Calculo.P_No_Calculo = Hdn_No_Calculo.Value;
            Rs_Modificar_Calculo.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
            Rs_Modificar_Calculo.P_Cuenta_Predial_ID = Hdn_Cuenta_Predial_ID.Value;
            Rs_Modificar_Calculo.P_Observaciones = Txt_Comentarios_Area.Text.ToUpper();
            Rs_Modificar_Calculo.P_Estatus = Cmb_Estatus.SelectedValue;
            Rs_Modificar_Calculo.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            Rs_Modificar_Calculo.P_Predio_Colindante = Chk_Predio_Colindante.Checked ? "SI" : "NO";
            Rs_Modificar_Calculo.P_Base_Impuesto = Txt_Base_Impuesto.Text.Replace(",", "");
            Rs_Modificar_Calculo.P_Minimo_Elevado_Anio = Txt_Minimo_Elevado_Anio.Text.Replace(",", "");
            Rs_Modificar_Calculo.P_Base_Impuesto_Division = Txt_Base_Impuesto_Div_Lotif.Text.Replace(",", "");
            Rs_Modificar_Calculo.P_No_Orden_Variacion = Hdn_No_Orden.Value;
            Rs_Modificar_Calculo.P_Anio_Orden = Int32.Parse(Hdn_Anio_Orden.Value);
            Rs_Modificar_Calculo.P_Tasas_ID = Hdn_Tasa_Traslado_ID.Value;
            Rs_Modificar_Calculo.P_Impuesto_Div_Lot = Hdn_Tasa_Div_Lotif_ID.Value;
            Rs_Modificar_Calculo.P_Costo_Constancia = Txt_Costo_Constancia_No_Adeudo.Text.Replace(",", "");
            Rs_Modificar_Calculo.P_Fecha_Escritura = Txt_Fecha_Escritura.Text;
            Rs_Modificar_Calculo.P_Monto_Traslado = Txt_Impuesto_Traslado_Dominio.Text.Replace(",", ""); ;
            Rs_Modificar_Calculo.P_Monto_Division = Txt_Impuesto_Division_Lotificacion.Text.Replace(",", ""); ;
            Rs_Modificar_Calculo.P_Monto_Multa = Txt_Multa.Text.Replace(",", "");
            Rs_Modificar_Calculo.P_Monto_Recargos = Txt_Recargos.Text.Replace(",", "");
            Rs_Modificar_Calculo.P_Monto_Total_Pagar = Txt_Total.Text.Replace(",", "");
            Rs_Modificar_Calculo.P_Anio_Calculo = Convert.ToInt32(Hdn_Anio_Calculo.Value);


            if (Opt_Tipo_Valor_Fiscal.Checked == true)
            {
                Rs_Modificar_Calculo.P_Tipo = "VALOR FISCAL";
            }
            else if (Opt_Tipo_Valor_Operacion.Checked == true)
            {
                Rs_Modificar_Calculo.P_Tipo = "VALOR OPERACION";
            }
            else if (Opt_Tipo_Avaluo_Predial.Checked == true)
            {
                Rs_Modificar_Calculo.P_Tipo = "AVALUO PREDIAL";
            }

            Rs_Modificar_Calculo.Actualizar_Estatus_Contrarecibo();

            if (Rs_Modificar_Calculo.Actualizar_Calculo() > 0) //Sustituye los datos que se encuentran en la BD por lo que introdujo el usuario
            {
                Inicializa_Controles(); //Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cálculo de traslado de dominio ", "alert('La modificación del Cálculo fue Exitosa');", true);
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Cálculo de traslado de dominio ", "alert('Ocurrió un error y el Cálculo no se modificó');", true);
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Modificar_Calculo " + ex.Message.ToString(), ex);
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
    /// NOMBRE_FUNCIÓN: Calculo_Impuesto_Division_Lotificacion
    /// DESCRIPCIÓN: Realizar calculo de impuesto de traslado con tasa de division y lotificacion
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calculo_Impuesto_Division_Lotificacion()
    {
        DateTime Fecha_Escritura;
        DateTime Fecha_Base_Recargos_Multas;
        Decimal Recargos;
        Int32 Dias;
        Int32 Meses = 0;
        Decimal Multa = 0;
        Decimal Base_Impuesto;
        Decimal Tasa_Div_Lotif;
        Decimal Impuesto_Div_Lotif = 0;
        Decimal Base_Division = 0;
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Rs_Consulta_Dias = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

        Decimal.TryParse(Txt_Base_Impuesto_Div_Lotif.Text, out Base_Division);
        if (Txt_Base_Impuesto_Div_Lotif.Text.Length <= 0 || Base_Division <= 0)
        {
            // si no hay base de impuesto para division copiar la base de traslado
            Txt_Base_Impuesto_Div_Lotif.Text = Txt_Base_Impuesto.Text;
        }

        // obtener tiempo transcurrido entre fecha de escritura y fecha actual
        if (DateTime.TryParse(Txt_Fecha_Escritura.Text, out Fecha_Escritura))
        {
            // agregar 30 dias habiles a la fecha de escritura para tomar como base
            Fecha_Base_Recargos_Multas = Fecha_Escritura;
            Fecha_Escritura = Rs_Consulta_Dias.Calcular_Fecha(Txt_Fecha_Escritura.Text, "30");
            Calcular_Tiempo_Entre_Fechas(Fecha_Escritura, DateTime.Now, out Dias, out Meses);
            // para calcular desde la fecha de escritura y no desde 30 dias habiles despues
            if (Dias > 0)
            {
                Calcular_Tiempo_Entre_Fechas(Fecha_Base_Recargos_Multas, DateTime.Now, out Dias, out Meses);
            }
        }

        if (Decimal.TryParse(Txt_Tasa_Division_Lotificacion.Text, out Tasa_Div_Lotif) &&
            Decimal.TryParse(Txt_Base_Impuesto_Div_Lotif.Text, out Base_Impuesto))
        {
            Impuesto_Div_Lotif = Base_Impuesto * Tasa_Div_Lotif / 100;
        }
        //calcular impuesto division lotificacion
        Txt_Impuesto_Division_Lotificacion.Text = Decimal.Round(Impuesto_Div_Lotif, 2).ToString("#,##0.00");

        if (Bnd_Calcular_Recargos == true)
        {
            // calcular recargos 
            Recargos = Calcular_Recargos_Traslado(Meses, Fecha_Escritura.Day);
            // mostrar recargos
            Txt_Recargos.Text = Decimal.Round(Recargos, 2).ToString();
            Bnd_Calcular_Recargos = true;
        }

        //obtener y mostrar la multa solo si se esta editando (nuevo o modificar)
        if (Btn_Modificar.ToolTip == "Actualizar" || Btn_Nuevo.ToolTip == "Dar de Alta")
        {
            if (Session["BUSQUEDA_CUOTA_MULTA"] == null)
            {
                Multa = Obtener_Multa();
                Txt_Multa.Text = Decimal.Round(Multa, 2).ToString();
            }
        }
        Calcular_Mostrar_Total_Impuesto();
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
        Decimal Impuesto_Traslado;
        Decimal Costo_Constancia = 0;
        Decimal Multa;
        Decimal Recargos;
        Decimal Total_Impuesto;

        Decimal.TryParse(Txt_Impuesto_Division_Lotificacion.Text, out Impuesto_Division_Lotif);
        if (Chk_Constancia_No_Adeudo.Checked)
        {
            Decimal.TryParse(Txt_Costo_Constancia_No_Adeudo.Text, out Costo_Constancia);
        }
        Decimal.TryParse(Txt_Impuesto_Traslado_Dominio.Text, out Impuesto_Traslado);
        Decimal.TryParse(Txt_Recargos.Text, out Recargos);
        Decimal.TryParse(Txt_Multa.Text, out Multa);
        // calcular total
        Total_Impuesto = Impuesto_Traslado + Impuesto_Division_Lotif + Costo_Constancia + Recargos + Multa;
        // mostrar en las cajas de texto correspondientes
        Txt_Multa.Text = Decimal.Round(Multa, 2).ToString("#,##0.00");
        Txt_Recargos.Text = Decimal.Round(Recargos, 2).ToString("#,##0.00");
        Txt_Total.Text = Decimal.Round(Total_Impuesto, 2).ToString("#,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Recargos_Traslado
    /// DESCRIPCIÓN: Calcular y mostrar en los campos correspondientes el total de impuesto
    /// PARÁMETROS:
    ///             1. Meses: Numero de meses para calcular recargos
    ///             2. Dia: Dia de vencimiento de recargos
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: Roberto González Oseguera
    /// FECHA_MODIFICÓ: 17-sep-2011
    /// CAUSA_MODIFICACIÓN: En lugar de tomar 2% siempre, ahora se consulta la tasa del catalogo 
    ///                     de recargos traslado
    ///*******************************************************************************************************
    private Decimal Calcular_Recargos_Traslado(Int32 Meses, Int32 Dia)
    {
        Decimal Imp_Traslado_Dom;
        Decimal Imp_Div_Lot;
        Decimal Recargos;
        Int32 Meses_Calcular = 0;
        Decimal Tasa_Total = 0;
        Dictionary<String, Decimal> Dic_Tasas;
        Int32 Anio = DateTime.Now.Year;

        Dic_Tasas = Obtener_Diccionario_Tasas_Recargos_Traslado();
        // no se cobran recargos de mas de 5 años (60 meses)
        if (Meses > 60)
            Meses = 60;

        // calcular los meses que se van a tomar con la tasa del anio actual
        Meses_Calcular += DateTime.Now.Month;
        // si el dia del calculo es menor o igual al dia de vencimiento de recargos, restar un mes a calcular
        if (DateTime.Now.Day <= Dia)
            Meses_Calcular--;
        // tasa total del anio actual, verificando si hay tasa para el anio
        if (Dic_Tasas.ContainsKey(Anio.ToString()))
        {
            // si los meses a calcular exceden los meses de recargo, solo tomar Meses
            if (Meses_Calcular > Meses)
            {
                Meses_Calcular = Meses;
                Meses = 0;
            }
            else    // si no, tomar todos los meses del año actual
            {
                Meses -= Meses_Calcular;
            }
            // calcular la tasa
            Tasa_Total += Dic_Tasas[Anio.ToString()] * Meses_Calcular;
            // decrementar el año
            Anio--;
        }
        else    // si no hay tasa para el anio, mostrar mensaje
        {
            Lbl_Mensaje_Error.Text = "No se encontró la tasa para calcular los recargos del año " + Anio
                + " en el catálogo de Recargos de traslado.";
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            return 0;
        }

        // agregar tasas para años anteriores completos (cada 12 mese)
        while (Meses >= 12)
        {
            // verificando si hay tasa para el anio
            if (Dic_Tasas.ContainsKey(Anio.ToString()))
            {
                Meses_Calcular = 12;
                Meses -= Meses_Calcular;
                Tasa_Total += Dic_Tasas[Anio.ToString()] * Meses_Calcular;
                Anio--;
            }
            else    // si no hay tasa para el anio, mostrar mensaje
            {
                Lbl_Mensaje_Error.Text = "No se encontró la tasa para calcular los recargos del año " + Anio
                    + " en el catálogo de Recargos de traslado.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                return 0;
            }
        }

        // agregar tasas para años anteriores si quedan menos de 12 meses
        if (Meses > 0)
        {
            // verificando si hay tasa para el anio
            if (Dic_Tasas.ContainsKey(Anio.ToString()))
            {
                Meses_Calcular = Meses;
                Meses = 0;
                Tasa_Total += Dic_Tasas[Anio.ToString()] * Meses_Calcular;
            }
            else    // si no hay tasa para el anio, mostrar mensaje
            {
                Lbl_Mensaje_Error.Text = "No se encontró la tasa para calcular los recargos del año " + Anio
                    + " en el catálogo de Recargos de traslado.";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                return 0;
            }
        }

        // obtener parametro impuesto division/lotificacion
        Decimal.TryParse(Txt_Impuesto_Division_Lotificacion.Text, out Imp_Div_Lot);
        Decimal.TryParse(Txt_Impuesto_Traslado_Dominio.Text, out Imp_Traslado_Dom);
        Recargos = (Imp_Div_Lot + Imp_Traslado_Dom) * (Tasa_Total / 100);
        return Recargos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Diccionario_Tasas_Recargos_Traslado
    /// DESCRIPCIÓN: Obtener las tasas de recargos de traslado desde la base de datos 
    ///             o desde variable de sesion y regresar como un diccionario
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<String, Decimal> Obtener_Diccionario_Tasas_Recargos_Traslado()
    {
        Cls_Cat_Pre_Recargos_Traslado_Negocio Rs_Recargos = new Cls_Cat_Pre_Recargos_Traslado_Negocio();
        DataTable Dt_Recargos;
        Dictionary<String, Decimal> Dic_Tasas;
        Int32 Anio_Actual = DateTime.Now.Year;

        // si se encuentra en variable de sesion, cargar diccionario
        if (Session["Dicc_Tasas"] != null)
        {
            Dic_Tasas = (Dictionary<String, Decimal>)Session["Dicc_Tasas"];
        }
        else        // si no hay varible de sesion, crearlo
        {
            // obtener las tasas de traslado del catalogo
            Rs_Recargos.P_Filtro = "";
            Dt_Recargos = Rs_Recargos.Consultar_Recargo();
            Dic_Tasas = new Dictionary<string, decimal>();
            // recorrer todas las filas encontradas para formar el diccionario
            foreach (DataRow fila_tasas in Dt_Recargos.Rows)
            {
                String Anio = fila_tasas[Cat_Pre_Recargos_Traslado.Campo_Anio].ToString();
                String Tasa = fila_tasas[Cat_Pre_Recargos_Traslado.Campo_Cuota].ToString();
                Int32 Anio_Entero = 0;
                Decimal Tasa_Decimal = 0;
                // si se pueden convertir el anio y la tasa, agregar al diccionario
                if (Int32.TryParse(Anio, out Anio_Entero) && decimal.TryParse(Tasa, out Tasa_Decimal))
                {
                    // verificar que no existen los valores ya en el diccionario
                    if (!Dic_Tasas.ContainsKey(Anio))
                    {
                        Dic_Tasas.Add(Anio, Tasa_Decimal);
                    }
                }
            }
            // almacenar en variable de sesion
            Session["Dicc_Tasas"] = Dic_Tasas;
        }

        return Dic_Tasas;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Multa
    /// DESCRIPCIÓN: Obtiene la multa correspondiente para el calculo del impuesto
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Obtener_Multa()
    {
        DateTime Fecha_Escritura;
        DateTime Fecha_Base_Recargos_Multas;
        Int32 Dias;
        Int32 Meses;
        Int32 Limite_Inferior;
        Int32 Limite_Superior;
        Decimal Cuota;
        DataTable Dt_Multas;
        Cls_Cat_Pre_Multas_Negocio Rs_Multas = new Cls_Cat_Pre_Multas_Negocio();
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Rs_Consulta_Dias = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();


        // obtener tiempo transcurrido entre fecha de escritura y fecha actual
        if (DateTime.TryParse(Txt_Fecha_Escritura.Text, out Fecha_Escritura))
        {
            Fecha_Base_Recargos_Multas = Fecha_Escritura;
            // agregar 30 dias habiles a la fecha de escritura para tomar como base
            Fecha_Escritura = Rs_Consulta_Dias.Calcular_Fecha(Txt_Fecha_Escritura.Text, "30");
            // obtener dias transccurridos
            Calcular_Tiempo_Entre_Fechas(Fecha_Escritura, DateTime.Now, out Dias, out Meses);
            // para calcular desde la fecha de escritura y no desde 30 dias habiles despues
            if (Dias > 0)
            {
                Calcular_Tiempo_Entre_Fechas(Fecha_Base_Recargos_Multas, DateTime.Now, out Dias, out Meses);
            }

            // Consultar multas
            Rs_Multas.P_Incluir_Campos_Foraneos = true;
            Rs_Multas.P_Ordenar_Dinamico = Cat_Pre_Multas_Cuotas.Campo_Año + " DESC, " + Cat_Pre_Multas.Campo_Desde_Anios + " ASC ";
            Dt_Multas = Rs_Multas.Consultar_Cuotas_Multas();
            foreach (DataRow Fila in Dt_Multas.Rows)
            {
                // obtener el rango de anios de cada multa
                if (Int32.TryParse(Fila[Cat_Pre_Multas.Campo_Desde_Anios].ToString(), out Limite_Inferior) &&
                Int32.TryParse(Fila[Cat_Pre_Multas.Campo_Hasta_Anios].ToString(), out Limite_Superior))
                {
                    Limite_Superior *= 365;
                    Limite_Inferior *= 365;
                    // si la cantidad de dias transcurridos, esta entre el limite inferior y superior, guardar ID de multa
                    if (Dias > Limite_Inferior && Dias <= Limite_Superior)
                    {
                        if (Decimal.TryParse(Fila[Cat_Pre_Multas_Cuotas.Campo_Monto].ToString(), out Cuota))
                        {
                            return Cuota;
                        }
                    }
                }
            }
        }


        return 0;
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
        //Hasta_Fecha = Hasta_Fecha.AddDays(1);
        Hasta_Fecha = Convert.ToDateTime(Hasta_Fecha.ToString("dd/MMM/yy"));
        TimeSpan Transcurrido = Hasta_Fecha - Desde_Fecha;
        if (Transcurrido > TimeSpan.Parse("0"))
        {
            DateTime Tiempo = DateTime.MinValue + Transcurrido;
            //Tiempo = Tiempo.AddDays(-1);
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
        DateTime Fecha_Ayudante;
        DateTime Fecha_Ayudante2;
        Int32 Contador_Dias = Dias;
        Int32 Contador_Dias2 = 0;
        Fecha_Ayudante = Desde_Fecha;
        Fecha_Ayudante2 = Desde_Fecha.AddMonths(1);
        Meses = 0;
        Transcurrido = Fecha_Ayudante2 - Fecha_Ayudante;
        long tickDiff_Prueba = Fecha_Ayudante2.Ticks - Fecha_Ayudante.Ticks;
        tickDiff_Prueba = tickDiff_Prueba / 10000000; // segundos
        Contador_Dias2 = (int)(tickDiff_Prueba / 86400);
        Contador_Dias -= Contador_Dias2;
        while (Contador_Dias > 0)
        {
            Meses++;
            Fecha_Ayudante = Fecha_Ayudante2;
            Fecha_Ayudante2 = Fecha_Ayudante2.AddMonths(1);
            Transcurrido = Fecha_Ayudante2 - Fecha_Ayudante;
            tickDiff_Prueba = Fecha_Ayudante2.Ticks - Fecha_Ayudante.Ticks;
            tickDiff_Prueba = tickDiff_Prueba / 10000000; // segundos
            Contador_Dias2 = (int)(tickDiff_Prueba / 86400);
            Contador_Dias -= Contador_Dias2;
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
        Cls_Cat_Pre_Cuentas_Predial_Negocio Rs_Cuentas = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Cuenta_Predial;

        try
        {
            if (Txt_Busqueda.Text != "")        //si no está vacío, buscar por anio y numero de calculo
            {
                // si el texto en el campo de busqueda contiene letras, buscar por cuenta predial
                if (regexAlphaNum.IsMatch(Txt_Busqueda.Text.Trim()))
                {
                    Rs_Cuentas.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Rs_Cuentas.P_Filtros_Dinamicos = " UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ") = "
                        + "'" + Txt_Busqueda.Text.Trim().ToUpper() + "'";
                    Dt_Cuenta_Predial = Rs_Cuentas.Consultar_Cuenta();
                    if (Dt_Cuenta_Predial != null)
                    {
                        if (Dt_Cuenta_Predial.Rows.Count > 0)
                        {
                            if (Dt_Cuenta_Predial.Rows[0][0].ToString() != "")
                            {
                                RS_Consulta_Calculos.P_Cuenta_Predial_ID = Dt_Cuenta_Predial.Rows[0][0].ToString();
                                RS_Consulta_Calculos.P_Estatus_Orden = "ACEPTADA";
                                RS_Consulta_Calculos.P_Estatus = "RECHAZADO";
                                // consultar cuentas
                                Dt_Calculos = RS_Consulta_Calculos.Consultar_Calculos_Ordenes();
                                Session["Consulta_Calculos"] = Dt_Calculos;
                                Llena_Grid_Calculos();
                            }
                        }
                    }
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
                    RS_Consulta_Calculos.P_Estatus = "RECHAZADO";
                    // consultar cuentas
                    Dt_Calculos = RS_Consulta_Calculos.Consultar_Calculos_Ordenes_Solo_Contrarecibo();
                    Session["Consulta_Calculos"] = Dt_Calculos;
                    Llena_Grid_Calculos();
                }

            }
            else
            {
                // si no hay criterio de busqueda, cargar datos de ordenes aceptadas y calculos rechazados
                RS_Consulta_Calculos.P_Estatus_Orden = "ACEPTADA";
                RS_Consulta_Calculos.P_Estatus = "RECHAZADO";
                Dt_Calculos = RS_Consulta_Calculos.Consulta_Pendientes_Calcular_Rechazados(); //Consulta los calculos de traslado y ordenes de variacion
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
            Grid_Calculos.Columns[4].Visible = true;
            Grid_Calculos.Columns[5].Visible = true;
            Grid_Calculos.Columns[9].Visible = true;
            Grid_Calculos.Columns[10].Visible = true;
            Grid_Calculos.Columns[11].Visible = true;
            Grid_Calculos.DataSource = Dt_Calculos;
            Grid_Calculos.DataBind();
            Grid_Calculos.Columns[4].Visible = false;
            Grid_Calculos.Columns[5].Visible = false;
            Grid_Calculos.Columns[9].Visible = false;
            Grid_Calculos.Columns[10].Visible = false;
            Grid_Calculos.Columns[11].Visible = false;
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
        string Ventana_Modal;
        try
        {
            Master.Etiqueta_Body_Master_Page.Attributes.Add("onkeydown", "cancelBack()");

            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Configuracion_Acceso("Frm_Ope_Pre_Calculo_Impuesto_Traslado.aspx");
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones

                //Scrip para mostrar Ventana Modal de los Tipos de Divisiones y Lotificaciones
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Calculo_Impuestos/Frm_Menu_Pre_Tipos_Divisiones_Lotificaciones.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Tasa_Division.Attributes.Add("onclick", Ventana_Modal);
                //Scrip para mostrar Ventana Modal de las Tasas de Traslado
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Calculo_Impuestos/Frm_Menu_Pre_Tasas_Traslado_Dominio.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Tasas_Traslasdo.Attributes.Add("onclick", Ventana_Modal);

                //Scrip para mostrar Ventana Modal de las Multas
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Calculo_Impuestos/Frm_Menu_Pre_Multas.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Multas.Attributes.Add("onclick", Ventana_Modal);
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
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resumen_Predio
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Resumen de Predial con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resumen_Predio()
    {
        String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Frm_Resumen_Predio.aspx";
        String Propiedades = ", 'resizable=no,status=no,width=750,scrollbars=yes');";
        //String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHeight:600px;dialogHide:true;help:no;scroll:no');";
        Btn_Resumen_Predio.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
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
            Calculo_Impuesto_Division_Lotificacion();

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
    /// NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// DESCRIPCIÓN: Habilita la forma para ingresar datos y permitir guardar un nuevo registro
    ///             en caso de guardar, verifica la validez de los datos ingresados y reporta cualquier
    ///             error
    /// PARAMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 11-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                if (Hdn_No_Calculo.Value != "")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "El contrarecibo seleccionado ya ha sido calculado<br />";
                }
                else if (Hdn_No_Orden.Value != "")
                {
                    Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
                    Consultar_Costo_Constancia();       // Mostrar costo de consulta
                    Txt_Cuenta_Predial.Focus();
                    Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
                    Txt_Fecha_Escritura_TextChanged(null, null);
                }
                else
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Seleccione el contrarecibo para generar cálculo<br />";
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                Validar_Campos();

                //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario, dar de alta los mismos en la base de datos
                else
                {
                    Alta_Calculo(); //Da de alta los datos proporcionados por el usuario
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

                // si el campo estatus no es RECHAZADO
                if (Grid_Calculos.SelectedIndex < 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Debe seleccionar un cálculo para poder hacer modificaciones<br />";
                }
                else if (Grid_Calculos.SelectedRow.Cells[8].Text != "RECHAZADO")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "No es posible modificar cálculos con estatus "
                        + Grid_Calculos.SelectedRow.Cells[8].Text + "<br />";
                }
                else if (Hdn_No_Calculo.Value != "")
                {
                    Habilitar_Controles("Modificar");   // Habilita los controles para la modificación de los datos
                    //Consultar_Costo_Constancia();       // Mostrar costo de consulta
                    Txt_Base_Gravable_Traslado.Focus();

                    // para que no calcule los recargos de nuevo:
                    Bnd_Calcular_Recargos = false;
                    Validar_Calculo_Impuesto_Traslado();
                    Calculo_Impuesto_Division_Lotificacion();
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
                    Session.Remove("Dicc_Tasas");
                    Session.Remove("Cuenta_Predial");
                    Session.Remove("BUSQUEDA_CUOTA_MULTA");
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
            // si se activo el predio colindante guardar valor en variable de sesion
            if (Chk_Predio_Colindante.Checked == true)
            {
                Session["minimo_anio"] = Txt_Minimo_Elevado_Anio.Text;
                Session["tasa_traslado_id"] = Hdn_Tasa_Traslado_ID.Value;
                Txt_Minimo_Elevado_Anio.Text = "0.00";
            }
            else
            {
                // verificar si hay variables de sesion
                if (Session["minimo_anio"] != null && Session["tasa_traslado_id"] != null)
                {
                    // validar que el id de la tasa es el mismo que ya se tenia en el campo oculto, de ser asi, volver a asignar valor
                    if (Session["tasa_traslado_id"].ToString() == Hdn_Tasa_Traslado_ID.Value)
                    {
                        Txt_Minimo_Elevado_Anio.Text = Session["minimo_anio"].ToString();
                        // eliminar variables de sesion
                        Session.Remove("minimo_anio");
                        Session.Remove("tasa_traslado_id");
                    }
                }
            }
            Validar_Calculo_Impuesto_Traslado();
            Calculo_Impuesto_Division_Lotificacion();

            Txt_Base_Impuesto_TextChanged(null, EventArgs.Empty);

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
            if (Chk_Constancia_No_Adeudo.Checked == true)
            {
                Consultar_Costo_Constancia();
            }
            else
            {
                Txt_Costo_Constancia_No_Adeudo.Text = "0";
            }
            Validar_Calculo_Impuesto_Traslado();
            Calculo_Impuesto_Division_Lotificacion();
            Txt_Base_Impuesto.Focus();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    protected void Txt_Minimo_Elevado_Anio_TextChanged(object sender, EventArgs e)
    {
        Validar_Calculo_Impuesto_Traslado();
        Calculo_Impuesto_Division_Lotificacion();
        Formato_Campos();
        Txt_Base_Gravable_Traslado.Focus();
    }
    protected void Txt_Base_Impuesto_Div_Lotif_TextChanged(object sender, EventArgs e)
    {
        Validar_Calculo_Impuesto_Traslado();
        Calculo_Impuesto_Division_Lotificacion();
        Formato_Campos();
        Txt_Tasa_Division_Lotificacion.Focus();
    }
    protected void Txt_Tasa_Traslado_Dominio_TextChanged(object sender, EventArgs e)
    {
        Validar_Calculo_Impuesto_Traslado();
        Calculo_Impuesto_Division_Lotificacion();
        Formato_Campos();
        Txt_Impuesto_Traslado_Dominio.Focus();
    }
    protected void Txt_Recargos_TextChanged(object sender, EventArgs e)
    {
        Bnd_Calcular_Recargos = false;
        Validar_Calculo_Impuesto_Traslado();
        Calculo_Impuesto_Division_Lotificacion();
        Formato_Campos();
        Txt_Tasa_Division_Lotificacion.Focus();
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
            // si el elemento seleccionado no contiene un numero de calculo, solo llenar los datos para nuevo calculo
            if (Grid_Calculos.SelectedRow.Cells[5].Text == "" || Grid_Calculos.SelectedRow.Cells[5].Text == "&nbsp;")
            {
                Int32 Anio_Orden;
                Int32.TryParse(Grid_Calculos.SelectedRow.Cells[10].Text, out Anio_Orden);
                Rs_Consulta_Calculo.P_Anio_Orden = Anio_Orden;
                Rs_Consulta_Calculo.P_No_Orden_Variacion = Grid_Calculos.SelectedRow.Cells[9].Text;
                Dt_Calculo = Rs_Consulta_Calculo.Consulta_Detalles_Orden_Variacion();

                Hdn_Anio_Orden.Value = Grid_Calculos.SelectedRow.Cells[10].Text;
                Hdn_No_Orden.Value = Grid_Calculos.SelectedRow.Cells[9].Text;
                Hdn_Cuenta_Predial_ID.Value = Grid_Calculos.SelectedRow.Cells[11].Text;

                if (Dt_Calculo.Rows.Count > 0)
                {
                    Decimal Monto = 0;
                    //Escribe los valores de los campos a los controles correspondientes de la forma
                    Txt_Cuenta_Predial.Text = Dt_Calculo.Rows[0]["CUENTA_PREDIAL"].ToString();
                    // asignar variable de sesion para el resumen de predio
                    Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text;
                    Decimal.TryParse(Dt_Calculo.Rows[0]["VALOR_FISCAL"].ToString(), out Monto);
                    Txt_Base_Impuesto.Text = Monto.ToString("#,##0.00");
                    if (!String.IsNullOrEmpty(Dt_Calculo.Rows[0]["FECHA_ESCRITURA"].ToString()))
                    {
                        Txt_Fecha_Escritura.Text = String.Format("{0:dd/MMM/yy}", Convert.ToDateTime(Dt_Calculo.Rows[0]["FECHA_ESCRITURA"].ToString()));
                    }
                }
                // mostrar boton nuevo
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;

            }
            else
            {
                // si hay un folio de calculo, separar elementos y consultar por numero de calculo y anio
                String[] Folio_Calculo = Grid_Calculos.SelectedRow.Cells[5].Text.Split('/');
                if (Folio_Calculo.Length == 2)
                {
                    Int32 Anio_Calculo;
                    Decimal Costo_Constancia = 0;
                    Int32.TryParse(Folio_Calculo[1], out Anio_Calculo);
                    Rs_Consulta_Calculo.P_No_Calculo = String.Format("{0:0000000000}", Convert.ToInt32(Folio_Calculo[0]));
                    Rs_Consulta_Calculo.P_Anio_Calculo = Anio_Calculo;
                    Dt_Calculo = Rs_Consulta_Calculo.Consulta_Detalles_Calculo();

                    if (Dt_Calculo.Rows.Count > 0)
                    {
                        //Escribe los valores de los campos a los controles correspondientes de la forma
                        Txt_Cuenta_Predial.Text = Dt_Calculo.Rows[0]["CUENTA_PREDIAL"].ToString();
                        // asignar variable de sesion para el resumen de predio
                        Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text;
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
                        Txt_Minimo_Elevado_Anio.Text = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Minimo_Elevado_Anio].ToString();

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
                        Hdn_Cuenta_Predial_ID.Value = Grid_Calculos.SelectedRow.Cells[11].Text;
                        Consultar_Tasas_Division(Hdn_Tasa_Div_Lotif_ID.Value);
                        Consultar_Tasas_Traslado_Por_ID(Hdn_Tasa_Traslado_ID.Value);

                        Chk_Predio_Colindante.Checked = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Predio_Colindante].ToString() == "SI" ? true : false;
                        Cmb_Estatus.SelectedValue = Dt_Calculo.Rows[0][Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus].ToString().ToUpper();

                        // mostrar los comentarios de validacion
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
                            Contenedor_Observaciones_Anteriores.InnerHtml += "</th></tr><tr><td colspan=\"2\">";
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
                        //Validar_Calculo_Impuesto_Traslado();
                        //Calculo_Impuesto_Division_Lotificacion();
                    }
                }
                Formato_Campos();

                // mostrar boton modificar
                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;
            }
            // ocultar el panel que contiene el grid
            Pnl_Grid.Visible = false;

            Pnl_Controles.Visible = true;

            // agregar script para mostrar el resumen de predio
            Cargar_Ventana_Emergente_Resumen_Predio();
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


    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Tasas_Traslasdo_Click
    /// DESCRIPCIÓN: Obtener parametros de la consulta en la ventana modal de tasas de traslado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Tasas_Traslasdo_Click(object sender, ImageClickEventArgs e)
    {
        // si se obtuvieron valores de la consulta, mostrar en la caja de texto correspondiente
        if (Session["BUSQUEDA_TASA_TRASLADO_DOMINIO"] != null)
        {
            if ((Boolean)Session["BUSQUEDA_TASA_TRASLADO_DOMINIO"] == true)
            {
                Hdn_Tasa_Traslado_ID.Value = Session["TASA_ID"].ToString();
                Txt_Minimo_Elevado_Anio.Text = Session["DEDUCIBLE"].ToString();
                Txt_Tasa_Traslado_Dominio.Text = Session["TASA"].ToString();

                // si el minimo elevado a anio esta vacio, asignar 0
                if (Txt_Minimo_Elevado_Anio.Text == "")
                {
                    Txt_Minimo_Elevado_Anio.Text = "0.00";
                }

                // si se activo el predio colindante guardar valor en variable de sesion
                if (Chk_Predio_Colindante.Checked == true)
                {
                    Session["minimo_anio"] = Txt_Minimo_Elevado_Anio.Text;
                    Session["tasa_traslado_id"] = Hdn_Tasa_Traslado_ID.Value;
                    Txt_Minimo_Elevado_Anio.Text = "0.00";
                }
                Validar_Calculo_Impuesto_Traslado();
                Calculo_Impuesto_Division_Lotificacion();
                Formato_Campos();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Tasa_Division_Click
    /// DESCRIPCIÓN: Obtener parametros de la consulta en la ventana modal de tasas de traslado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Tasa_Division_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_TASA_DIVISION_LOTIFICACION"] != null)
        {
            if ((Boolean)Session["BUSQUEDA_TASA_DIVISION_LOTIFICACION"] == true)
            {
                Hdn_Tasa_Div_Lotif_ID.Value = Session["IMPUESTO_DIVISION_LOT_ID"].ToString();
                Txt_Tipo_Division_Lotificacion.Text = System.Web.HttpUtility.HtmlDecode(Session["CONCEPTO"].ToString());
                Txt_Tasa_Division_Lotificacion.Text = Session["TASA"].ToString();

                Validar_Calculo_Impuesto_Traslado();
                Calculo_Impuesto_Division_Lotificacion();
                Formato_Campos();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Tasa_Division_Click
    /// DESCRIPCIÓN: Obtener parametros de la consulta en la ventana modal de tasas de traslado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Multas_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_CUOTA_MULTA"] != null)
        {
            if ((Boolean)Session["BUSQUEDA_CUOTA_MULTA"] == true)
            {
                //Hdn_Tasa_Div_Lotif_ID.Value = Session["IMPUESTO_DIVISION_LOT_ID"].ToString();
                Txt_Multa.Text = Session["MONTO"].ToString();
                //Txt_Tasa_Division_Lotificacion.Text = Session["TASA"].ToString();

                Validar_Calculo_Impuesto_Traslado();
                Calculo_Impuesto_Division_Lotificacion();
                Formato_Campos();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Click
    /// DESCRIPCIÓN: Manejar el clic en el boton de busqueda
    ///             limpiar controles y llamar metodo que busca los calculos
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
                Lbl_Mensaje_Error.Text = "No se obtuvieron resultados con el criterio proporcionado<br />";
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
    /// NOMBRE_FUNCIÓN: Btn_Quitar_Division_Click
    /// DESCRIPCIÓN: Manejar el clic en el boton de busqueda
    ///             limpiar controles y llamar metodo que busca los calculos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 08-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Quitar_Division_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        try
        {
            Hdn_Tasa_Div_Lotif_ID.Value = "";
            Txt_Tipo_Division_Lotificacion.Text = "";
            Txt_Tasa_Division_Lotificacion.Text = "";

            Validar_Calculo_Impuesto_Traslado();
            Calculo_Impuesto_Division_Lotificacion();
            Formato_Campos();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Fecha_Escritura_TextChanged
    /// DESCRIPCIÓN: Manejar el cambio de texto en la caja Fecha Escritura
    ///             validar la fecha y colver a calcular impuestos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Txt_Fecha_Escritura_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha;
        // si el campo contiene fecha
        if (Txt_Fecha_Escritura.Text.Length > 0)
        {
            // obtener una fecha valida, dar formato y reescribir en la caja de texto
            if (DateTime.TryParse(Txt_Fecha_Escritura.Text, out Fecha))
            {
                Session.Remove("BUSQUEDA_CUOTA_MULTA");
                Txt_Fecha_Escritura.Text = String.Format("{0:dd/MMM/yy}", Fecha);
                Validar_Calculo_Impuesto_Traslado();
                Calculo_Impuesto_Division_Lotificacion();
            }
        }
    }

    #endregion EVENTOS

}
