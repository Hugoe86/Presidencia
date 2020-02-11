using System;
using System.Collections;
using System.Configuration;
using System.Globalization;
using System.Threading;
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
using Presidencia.Operacion_Descuentos_Traslado.Negocio;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Descuentos_Predial.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Descuentos_Traslado : System.Web.UI.Page
{
    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            //Response.AddHeader("Refresh", Convert.ToString((Session.Timeout * 60) + 5));
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Cls_Ope_Pre_Descuentos_Traslado_Negocio des = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();
                //des.P_No_Descuento="2002";
                //des.Consultar_Descuentos_Traslado();
                Div_Descuentos_Traslado.Visible = true;
                Div_OV.Visible = false;
                Configuracion_Acceso("Frm_Ope_Pre_Descuentos_Traslado.aspx");
                Configuracion_Formulario(true);
                Llenar_Tabla_Contrarecibos(0);
            }
        }

        //Div_Descuentos_Traslado.Visible = false;
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
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        Div_Descuentos_Traslado.Visible = false;
        Grid_Descuentos_Traslado.Visible = true;
        Btn_Nuevo.Visible = !Estatus;
        Btn_Nuevo.AlternateText = "Nuevo";
        Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        Btn_Modificar.Visible = !Estatus;
        Btn_Modificar.AlternateText = "Modificar";
        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Btn_Imprimir.Visible = !Estatus;
        Txt_Cuenta_Predial.Enabled = !Estatus;
        Txt_Contrarecibo.Enabled = !Estatus;
        Txt_Impuesto_Traslado.Enabled = !Estatus;
        Txt_Impuesto_Division.Enabled = !Estatus;
        Txt_Costo_Constancia.Enabled = !Estatus;
        Txt_Total_Impuesto.Enabled = !Estatus;
        Txt_Fecha_Inicial.Enabled = !Estatus;
        Cmb_Estatus.Enabled = !Estatus;
        Txt_Recargos.Enabled = !Estatus;
        Txt_Desc_Recargo.Enabled = !Estatus;
        Txt_Multas.Enabled = !Estatus;
        Txt_Desc_Multas.Enabled = !Estatus;
        //Txt_Subtotal.Enabled = !Estatus;
        Txt_Total_Por_Pagar.Enabled = !Estatus;
        Txt_Realizo.Enabled = !Estatus;
        Txt_Fecha_Vencimiento.Enabled = !Estatus;
        Txt_Observaciones.Enabled = !Estatus;
        Txt_Monto_Multas.Enabled = !Estatus;
        Txt_Monto_Recargos.Enabled = !Estatus;
        //Grid_Descuentos_Traslado.SelectedIndex = (-1);
        Btn_Buscar.Enabled = Estatus;
        Txt_Busqueda.Enabled = Estatus;
        Btn_Txt_Fecha_Vencimiento.Enabled = !Estatus;
        Btn_Txt_Fecha_Inicial.Enabled = !Estatus;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        Txt_Cuenta_Predial.Text = "";
        Txt_Contrarecibo.Text = "";
        Txt_Fecha_Inicial.Text = "";
        Txt_Fecha_Vencimiento.Text = "";
        Txt_Impuesto_Traslado.Text = "";
        Txt_Impuesto_Division.Text = "";
        Txt_Costo_Constancia.Text = "";
        Txt_Recargos.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Desc_Recargo.Text = "0";
        Txt_Desc_Multas.Text = "0";
        Txt_Multas.Text = "";
        //Txt_Subtotal.Text = "";
        Txt_Total_Por_Pagar.Text = "";
        Txt_Realizo.Text = "";
        Txt_Observaciones.Text = "";
        Txt_No_Adeudo.Text = "";
        Txt_No_Descuento.Text = "";
        Txt_No_Calculo.Text = "";
        Txt_Anio_Calculo.Text = "";
        //Txt_Total.Text = "";
        Txt_Monto_Multas.Text = "";
        Txt_Monto_Recargos.Text = "";
        Grid_Descuentos_Traslado.DataSource = new DataTable();
        Grid_Descuentos_Traslado.DataBind();
        Div_Descuentos_Traslado.Visible = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Realizar_Descuento
    ///DESCRIPCIÓN: Regresa el Total por Pagar aplicando el Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private string Realizar_Descuento(String Monto, String Total)
    {
        decimal Descuento = 0;
        decimal dec_Monto;
        decimal dec_Total;
        decimal.TryParse(Monto, out dec_Monto);
        decimal.TryParse(Total, out dec_Total);

        if (dec_Total > 0)
        {
            Descuento = (dec_Monto / dec_Total) * 100;
        }

        return Descuento.ToString("#,##0.00");
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Realizar_Subtotal
    ///DESCRIPCIÓN: Regresa el Subtotal del Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Realizar_Subtotal()
    {
        String Subtotal;
        Double Descuento = 0;

        Descuento = (Math.Round(Convert.ToDouble(Txt_Multas.Text.ToString().Replace("$", "").Trim()), 2) - Math.Round(Convert.ToDouble(Txt_Monto_Multas.Text.ToString().Replace("$", "").Trim()), 2)) +
                    (Math.Round(Convert.ToDouble(Txt_Recargos.Text.ToString().Replace("$", "").Trim()), 2) - Math.Round(Convert.ToDouble(Txt_Monto_Recargos.Text.ToString().Replace("$", "").Trim()), 2));

        Descuento = Math.Round(Descuento, 2);
        return Subtotal = Descuento.ToString();
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Total
    /// DESCRIPCIÓN: Calcular  total a pagar 
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 2-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Total()
    {
        Decimal Constancia;
        Decimal Traslado;
        Decimal Division;
        Decimal Recargos;
        Decimal Multas;
        Decimal Descuento_Recargos;
        Decimal Descuento_Multas;
        Decimal Total_A_Pagar = 0;

        // obtener montos de las cajas de texto
        Decimal.TryParse(Txt_Costo_Constancia.Text, out Constancia);
        Decimal.TryParse(Txt_Impuesto_Traslado.Text, out Traslado);
        Decimal.TryParse(Txt_Impuesto_Division.Text, out Division);
        Decimal.TryParse(Txt_Recargos.Text, out Recargos);
        Decimal.TryParse(Txt_Multas.Text, out Multas);
        Decimal.TryParse(Txt_Monto_Recargos.Text, out Descuento_Recargos);
        Decimal.TryParse(Txt_Monto_Multas.Text, out Descuento_Multas);

        // calcular total
        Total_A_Pagar = Constancia + Traslado + Division
             + (Recargos - (Recargos - Descuento_Recargos < 0 ? Recargos : Descuento_Recargos))
             + (Multas - (Multas - Descuento_Multas < 0 ? Multas : Descuento_Multas));

        // volver a poner valores (por si alguna caja de texto esta vacia)
        Txt_Costo_Constancia.Text = Constancia.ToString("#,##0.00");
        Txt_Impuesto_Traslado.Text = Traslado.ToString("#,##0.00");
        Txt_Impuesto_Division.Text = Division.ToString("#,##0.00");
        Txt_Recargos.Text = Recargos.ToString("#,##0.00");
        Txt_Multas.Text = Multas.ToString("#,##0.00");
        Txt_Monto_Recargos.Text = Descuento_Recargos.ToString("#,##0.00");
        Txt_Monto_Multas.Text = Descuento_Multas.ToString("#,##0.00");

        // escribir totales en las cajas de texto
        Txt_Total_Por_Pagar.Text = Total_A_Pagar.ToString("#,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Porcentaje_Descuento_Recargos
    /// DESCRIPCIÓN: Calcular el porcentaje de descuento a partir de un monto a descontar de recargos ordinarios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 2-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Porcentaje_Descuento_Recargos()
    {
        Decimal Recargos = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Recargos.Text, out Recargos);
        Decimal.TryParse(Txt_Monto_Recargos.Text, out Monto);

        // solo calcular si la cantidad de recargos es mayor que cero
        if (Recargos > 0)
        {
            Porcentaje = (Monto * 100) / Recargos;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Desc_Recargo.Text = Porcentaje.ToString("0.##");
        Txt_Monto_Recargos.Text = Monto.ToString("0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Porcentaje_Descuento_Multas
    /// DESCRIPCIÓN: Calcular el porcentaje de descuento a partir de un monto a descontar de muultas
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 2-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Porcentaje_Descuento_Multas()
    {
        Decimal Multas = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Multas.Text, out Multas);
        Decimal.TryParse(Txt_Monto_Multas.Text, out Monto);

        // solo calcular si la cantidad de recargos es mayor que cero
        if (Multas > 0)
        {
            Porcentaje = (Monto * 100) / Multas;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Desc_Multas.Text = Porcentaje.ToString("0.##");
        Txt_Monto_Multas.Text = Monto.ToString("0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Monto_Descuento_Recargos
    /// DESCRIPCIÓN: Calcular el monto de descuento a partir de un porcentaje a descontar de recargos ordinarios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 2-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Monto_Descuento_Recargos()
    {
        Decimal Recargos = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Recargos.Text, out Recargos);
        Decimal.TryParse(Txt_Desc_Recargo.Text, out Porcentaje);

        Monto = (Recargos * Porcentaje) / 100;
        if (Recargos <= 0 || Monto <= 0 || Porcentaje <= 0)
        {
            Monto = 0;
            Porcentaje = 0;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Desc_Recargo.Text = Porcentaje.ToString("0.##");
        Txt_Monto_Recargos.Text = Monto.ToString("0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Monto_Descuento_Multas
    /// DESCRIPCIÓN: Calcular el monto de descuento a partir de un porcentaje a descontar de recargos ordinarios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 2-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Monto_Descuento_Multas()
    {
        Decimal Multas = 0;
        Decimal Porcentaje = 0;
        Decimal Monto = 0;

        Decimal.TryParse(Txt_Multas.Text, out Multas);
        Decimal.TryParse(Txt_Desc_Multas.Text, out Porcentaje);

        Monto = (Multas * Porcentaje) / 100;
        if (Multas <= 0 || Monto <= 0 || Porcentaje <= 0)
        {
            Monto = 0;
            Porcentaje = 0;
        }

        // mostrar cantidades en las cajas de texto
        Txt_Desc_Multas.Text = Porcentaje.ToString("0.##");
        Txt_Monto_Multas.Text = Monto.ToString("0.00");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Calcula y regresa el Monto Total de los Recargos aplicando el Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Calcular_Monto_Recargo()
    {
        String Recargo;
        Double Monto_Recargo = (Convert.ToDouble(Txt_Recargos.Text.Trim().Replace("$", "").Trim()) * (Convert.ToDouble(Txt_Desc_Recargo.Text.Trim().Replace("$", "").Trim()) / 100));
        Monto_Recargo = Math.Round(Monto_Recargo, 2);
        return Recargo = Monto_Recargo.ToString();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Calcular_Monto_Multa
    ///DESCRIPCIÓN: Calcura y regresa el Monto Total de las Multas aplicando el Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private String Calcular_Monto_Multa()
    {
        String Multa;
        Double Monto_Multa = (Convert.ToDouble(Txt_Multas.Text.Trim().Replace("$", "").Trim()) * (Convert.ToDouble(Txt_Desc_Multas.Text.Trim().Replace("$", "")) / 100));
        Monto_Multa = Math.Round(Monto_Multa, 2);
        return Multa = Monto_Multa.ToString();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ver_Columnas
    ///DESCRIPCIÓN: Muestra las columnas Ocultas del Grid
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Ver_Columnas()
    {
        Grid_Descuentos_Traslado.Columns[11].Visible = true;
        Grid_Descuentos_Traslado.Columns[12].Visible = true;
        Grid_Descuentos_Traslado.Columns[13].Visible = true;
        Grid_Descuentos_Traslado.Columns[14].Visible = true;
        Grid_Descuentos_Traslado.Columns[15].Visible = true;
        Grid_Descuentos_Traslado.Columns[16].Visible = true;
        Grid_Descuentos_Traslado.Columns[17].Visible = true;
        Grid_Descuentos_Traslado.Columns[18].Visible = true;
        Grid_Descuentos_Traslado.Columns[19].Visible = true;
        Grid_Descuentos_Traslado.Columns[20].Visible = true;
        Grid_Descuentos_Traslado.Columns[21].Visible = true;
        Grid_Descuentos_Traslado.Columns[22].Visible = true;
        Grid_Descuentos_Traslado.Columns[23].Visible = true;
        Grid_Descuentos_Traslado.Columns[24].Visible = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Ocultar_Columnas
    ///DESCRIPCIÓN: Oculta las columnas innecesarias del Grid
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Ocultar_Columnas()
    {
        Grid_Descuentos_Traslado.Columns[11].Visible = false;
        Grid_Descuentos_Traslado.Columns[12].Visible = false;
        Grid_Descuentos_Traslado.Columns[13].Visible = false;
        Grid_Descuentos_Traslado.Columns[14].Visible = false;
        Grid_Descuentos_Traslado.Columns[15].Visible = false;
        Grid_Descuentos_Traslado.Columns[16].Visible = false;
        Grid_Descuentos_Traslado.Columns[17].Visible = false;
        Grid_Descuentos_Traslado.Columns[18].Visible = false;
        Grid_Descuentos_Traslado.Columns[19].Visible = false;
        Grid_Descuentos_Traslado.Columns[20].Visible = false;
        Grid_Descuentos_Traslado.Columns[21].Visible = false;
        Grid_Descuentos_Traslado.Columns[22].Visible = false;
        Grid_Descuentos_Traslado.Columns[23].Visible = false;
        Grid_Descuentos_Traslado.Columns[24].Visible = false;
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
    /// NOMBRE_FUNCIÓN: Insertar_Pasivo
    /// DESCRIPCIÓN: Insertar el pasivo del descuento para el cobro del traslado
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 20-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Insertar_Pasivo(OracleCommand Orcl_Cmd)
    {
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Modificar_Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Cls_Cat_Pre_Claves_Ingreso_Negocio Rs_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();

        DataTable Dt_Orden;
        DataTable Dt_Cuenta_Predial;
        DataTable Dt_Datos_Orden;
        DataTable Dt_Claves;
        String Mi_Sql = "";
        Int32 Anio_Orden = 0;
        String Numero_Orden = "";
        String Tipo_Predio_ID = "";
        String Tipo_Predio = "";
        String Propietario = "";
        Decimal Descuento_Recargos = 0;
        Decimal Descuento_Multas = 0;
        String Cuenta_Predial_ID = "";

        Rs_Modificar_Calculo.P_Cmd_Calculo = Orcl_Cmd;

        // obtener datos del grid
        Cuenta_Predial_ID = Grid_Descuentos_Traslado.SelectedRow.Cells[24].Text;
        Numero_Orden = Grid_Descuentos_Traslado.SelectedRow.Cells[14].Text;
        if (Int32.TryParse(Grid_Descuentos_Traslado.SelectedRow.Cells[15].Text, out Anio_Orden))
        {
            Orden_Variacion.P_Año = Anio_Orden;
        }
        else
        {
            Orden_Variacion.P_Año = DateTime.Now.Year;
        }

        // consultar el tipo de predio y propietario registrado en la cuenta predial
        Mi_Sql = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
            + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID
            + ", (SELECT " + Cat_Pre_Contribuyentes.Campo_Apellido_Paterno + " || ' ' || "
            + Cat_Pre_Contribuyentes.Campo_Apellido_Materno + " || ' ' || "
            + Cat_Pre_Contribuyentes.Campo_Nombre + " FROM "
            + Cat_Pre_Contribuyentes.Tabla_Cat_Pre_Contribuyentes + " WHERE "
            + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + " = (SELECT "
            + Cat_Pre_Propietarios.Campo_Contribuyente_ID + " FROM "
            + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + " WHERE "
            + Cat_Pre_Propietarios.Campo_Tipo + " = 'PROPIETARIO' AND "
            + Cat_Pre_Propietarios.Campo_Cuenta_Predial_ID + " = "
            + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "."
            + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ") ) NOMBRE_PROPIETARIO ";
        Cuentas_Predial.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Cuentas_Predial.P_Campos_Dinamicos = Mi_Sql;
        Dt_Cuenta_Predial = Cuentas_Predial.Consultar_Cuenta();
        if (Dt_Cuenta_Predial != null)
        {
            if (Dt_Cuenta_Predial.Rows.Count > 0)
            {
                Tipo_Predio_ID = Dt_Cuenta_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                Propietario = Dt_Cuenta_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
            }
        }
        // obtener valores del descuento
        Decimal.TryParse(Txt_Monto_Recargos.Text.Replace("$", ""), out Descuento_Recargos);
        Decimal.TryParse(Txt_Monto_Multas.Text.Replace("$", ""), out Descuento_Multas);

        // consultar el tipo de predio de la orden de variacion (sobreescribir el valor de la cuenta en caso de encontrar en la orden)
        Dt_Orden = Rs_Modificar_Calculo.Consulta_Folio_Orden_Contrarecibo(Numero_Orden, Anio_Orden);
        Orden_Variacion.P_Orden_Variacion_ID = Numero_Orden;
        Orden_Variacion.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        //Orden_Variacion.Consultar_Ordenes_Variacion();
        //Dt_Datos_Orden = Orden_Variacion.P_Generar_Orden_Dt_Detalles;
        Dt_Datos_Orden = Orden_Variacion.Consultar_Ordenes_Variacion();
        if (Dt_Datos_Orden != null)
        {
            foreach (DataRow Dr_Fila in Dt_Datos_Orden.Rows)
            {
                if (Dr_Fila[Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID].ToString() != "")
                {
                    Tipo_Predio_ID = Dr_Fila[Ope_Pre_Ordenes_Variacion.Campo_Tipo_Predio_ID].ToString();
                }
            }
        }

        // consultar nombre del tipo de predio a partir del ID obtenido
        String Dato_Nuevo = Obtener_Dato_Consulta(
            Cat_Pre_Tipos_Predio.Campo_Descripcion,
            Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio,
            Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '"
            + Tipo_Predio_ID + "'"
            );
        if (!string.IsNullOrEmpty(Dato_Nuevo))
        {
            Tipo_Predio = Dato_Nuevo;
        }

        Rs_Modificar_Calculo.P_Referencia = "TD" + Convert.ToInt32(Txt_No_Calculo.Text) + Txt_Anio_Calculo.Text;
        // agregar filtro para eliminar solo pasivos sin clave de ingreso de descuentos
        Rs_Modificar_Calculo.P_Filtro_Dinamico = " AND UPPER("
            + Ope_Ing_Pasivo.Campo_Descripcion + ") LIKE '%DESCUENTO%'";
        // eliminar pasivos con la misma referencia con estatus POR PAGAR
        Rs_Modificar_Calculo.Eliminar_Referencias_Pasivo();
        //Rs_Modificar_Calculo.P_Descripcion = "CALCULO POR TRASLADO";
        Rs_Modificar_Calculo.P_Fecha_Tramite = Txt_Fecha_Inicial.Text;
        Rs_Modificar_Calculo.P_Fecha_Vencimiento_Pasivo = Txt_Fecha_Vencimiento.Text;
        Rs_Modificar_Calculo.P_Estatus = "POR PAGAR";
        Rs_Modificar_Calculo.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Rs_Modificar_Calculo.P_Contribuyente = Txt_Propietario.Text;//Este dato debe ser de la Orden; no de la Cuenta.
        Rs_Modificar_Calculo.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado;
        //insertar pasivo de multas si es mayor a cero
        if (Descuento_Multas > 0)
        {
            Rs_Claves_Ingreso.P_Tipo = "TRASLADO";
            Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = "MULTAS";
            Dt_Claves = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();
            // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
            if (Dt_Claves != null && Dt_Claves.Rows.Count > 0)
            {
                Rs_Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Claves.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                Rs_Modificar_Calculo.P_Dependencia_ID = Dt_Claves.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                Rs_Modificar_Calculo.P_Descripcion = "DESCUENTO MULTA";
                Rs_Modificar_Calculo.P_Monto_Total_Pagar = (Descuento_Multas * -1).ToString();
                Rs_Modificar_Calculo.Alta_Pasivo();
            }
            else
            {
                throw new Exception("Falta la clave de ingreso de MULTAS de TRASLADO");
            }
        }
        //insertar pasivo de recargos si es mayor a cero
        if (Descuento_Recargos > 0)
        {
            Rs_Claves_Ingreso.P_Tipo = "TRASLADO";
            Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = "RECARGOS ORDINARIOS";
            Dt_Claves = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();
            // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
            if (Dt_Claves != null && Dt_Claves.Rows.Count > 0)
            {
                Rs_Modificar_Calculo.P_Clave_Ingreso_ID = Dt_Claves.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                Rs_Modificar_Calculo.P_Dependencia_ID = Dt_Claves.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                Rs_Modificar_Calculo.P_Descripcion = "DESCUENTO RECARGOS";
                Rs_Modificar_Calculo.P_Monto_Total_Pagar = (Descuento_Recargos * -1).ToString();
                Rs_Modificar_Calculo.Alta_Pasivo();
            }
            else
            {
                throw new Exception("Falta la clave de ingreso de RECARGOS de TRASLADO");
            }
        }
    }

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes_Generales
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación que se vea afectada en la basae de datos.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes_Generales()
    {
        Lbl_Ecabezado_Mensaje.Text = "";
        String Mensaje_Error = "Es necesario.";
        Boolean Validacion = true;
        if (Cmb_Estatus.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Estatus.";
            Validacion = false;
        }
        if (Txt_Desc_Recargo.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Descuento por Recargo.";
            Validacion = false;
        }
        if (Txt_Desc_Multas.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Descuento por Multas.";
            Validacion = false;
        }
        if (Txt_Fecha_Vencimiento.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Fecha de Vencimiento.";
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
    ///NOMBRE DE LA FUNCIÓN: Validar_Porcentaje
    ///DESCRIPCIÓN: Hace una validacion de que el usuario que realizara el Descuento tenga
    ///             permisos para aplicar ese descuento
    ///PROPIEDADES:     
    ///CREO: Jacqueline Ramirez Sierra
    ///FECHA_CREO: 4/oct/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Porcentaje()
    {
        Boolean Validacion = true;
        decimal Descuento_Multas;
        decimal Descuento_Recargos;

        decimal.TryParse(Txt_Monto_Multas.Text, out Descuento_Multas);
        decimal.TryParse(Txt_Monto_Recargos.Text, out Descuento_Recargos);

        if (Descuento_Recargos <= 0 && Descuento_Multas <= 0)
        {
            Lbl_Ecabezado_Mensaje.Text = "Debe proporcionar por lo menos un descuento";
            Validacion = false;
            Div_Contenedor_Msj_Error.Visible = true;
        }

        return Validacion;
    }

    #endregion

    #region Grid

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Contrarecibos
    ///DESCRIPCIÓN: Llena la tabla de Descuentos de Traslado
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_View
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Contrarecibos(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();
            Ver_Columnas();
            Grid_Descuentos_Traslado.DataSource = Descuentos.Consultar_Descuentos_Traslado();
            Grid_Descuentos_Traslado.PageIndex = Pagina;
            Grid_Descuentos_Traslado.DataBind();
            Ocultar_Columnas();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Descuentos_Traslado_Busqueda
    ///DESCRIPCIÓN: Llena la tabla de Descuentos de Traslado de auerdo a la busqueda introducida.
    ///PROPIEDADES:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Descuentos_Traslado_Busqueda(int Pagina)
    {
        Int32 No_Orden = 0;
        try
        {
            Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();
            if (Cmb_Busqueda_General.SelectedItem.Text == "CONTRARECIBO")
            {
                Int32.TryParse(Txt_Busqueda.Text.Trim(), out No_Orden);
                Descuentos.P_No_Contrarecibo = String.Format("{0:0000000000}", No_Orden);
            }
            else
            {
                if (Cmb_Busqueda_General.SelectedItem.Text == "CUENTA")
                {
                    Descuentos.P_Cuenta_Predial = Txt_Busqueda.Text.ToUpper().Trim();
                }
                else
                {
                    if (Cmb_Busqueda_General.SelectedItem.Text == "FOLIO")
                    {
                        Descuentos.P_Folio = Txt_Busqueda.Text.Substring(2);
                    }
                }
            }

            Descuentos.P_Estatus = " = 'POR PAGAR' ";

            Grid_Descuentos_Traslado.DataSource = Descuentos.Consultar_Descuentos_Traslado_Busqueda();
            Grid_Descuentos_Traslado.PageIndex = Pagina;
            Ver_Columnas();
            Grid_Descuentos_Traslado.DataBind();
            Ocultar_Columnas();
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Descuentos_Traslado_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView General de Descuentos de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Descuentos_Traslado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Limpiar_Catalogo();
            Grid_Descuentos_Traslado.SelectedIndex = (-1);
            Div_Descuentos_Traslado.Visible = false;
            Llenar_Tabla_Contrarecibos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN: Grid_Descuentos_Traslado_SelectedIndexChanged
    /////DESCRIPCIÓN: Obtiene los datos del Descuento de Traslado seleccionado para mostrarlos a detalle
    /////PROPIEDADES:     
    /////CREO: José Alfredo García Pichardo.
    /////FECHA_CREO: 15/Agosto/2011 
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    protected void Grid_Descuentos_Traslado_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Descuentos_Traslado.SelectedIndex > (-1))
            {
                // mostrar botones dependiendo del estatus del descuento
                if (HttpUtility.HtmlDecode(Grid_Descuentos_Traslado.SelectedRow.Cells[9].Text).Trim() == "")
                {
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Imprimir.Visible = false;
                }
                else
                {
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Imprimir.Visible = true;
                }

                titulo.Visible = false;
                Txt_No_Descuento.Text = HttpUtility.HtmlDecode(Grid_Descuentos_Traslado.SelectedRow.Cells[11].Text);
                Txt_Contrarecibo.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[1].Text;
                Txt_Cuenta_Predial.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[2].Text;
                Txt_Fecha_Inicial.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[3].Text;
                Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByText(Grid_Descuentos_Traslado.SelectedRow.Cells[9].Text));
                Txt_Recargos.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[4].Text.Replace("$", "");
                Txt_Desc_Recargo.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[5].Text.Replace("$", "");
                Txt_Multas.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[6].Text.Replace("$", "");
                Txt_Desc_Multas.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[7].Text.Replace("$", "");
                //Txt_Monto_Multas.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[6].Text;
                //Txt_Monto_Recargos.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[4].Text.Replace("$", "");
                Txt_Impuesto_Traslado.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[21].Text.Replace("$", "");
                Txt_Impuesto_Division.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[22].Text.Replace("$", "");
                Txt_Costo_Constancia.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[23].Text.Replace("$", "");
                Txt_Total_Impuesto.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[21].Text.Replace("$", "");
                Txt_Total_Por_Pagar.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[8].Text.Replace("$", "");

                Txt_Realizo.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[17].Text;
                Txt_Fecha_Vencimiento.Text = HttpUtility.HtmlDecode(Grid_Descuentos_Traslado.SelectedRow.Cells[18].Text);
                Txt_Observaciones.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[19].Text;
                Txt_No_Calculo.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[12].Text;
                Txt_Anio_Calculo.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[13].Text;
                Txt_No_Adeudo.Text = Grid_Descuentos_Traslado.SelectedRow.Cells[16].Text;

                Grid_Descuentos_Traslado.Visible = false;
                Div_Descuentos_Traslado.Visible = true;
                Btn_Salir.AlternateText = "Cancelar";

                Calcular_Monto_Descuento_Multas();
                Calcular_Monto_Descuento_Recargos();
                Calcular_Total();
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
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta un nuevo Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();

        OracleConnection Cn = new OracleConnection();
        OracleCommand Cmd = new OracleCommand();
        OracleTransaction Trans = null;

        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                if (Grid_Descuentos_Traslado.Rows.Count > 0 && Grid_Descuentos_Traslado.SelectedIndex > (-1) && Txt_No_Descuento.Text == " ")
                {
                    Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                    DateTime Fecha_Vencimiento;

                    Txt_Desc_Recargo.Focus();
                    Div_Descuentos_Traslado.Visible = true;
                    Configuracion_Formulario(false);
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Imprimir.Visible = false;
                    Cmb_Estatus.SelectedIndex = 1;
                    Txt_Impuesto_Traslado.Enabled = false;
                    Txt_Impuesto_Division.Enabled = false;
                    Txt_Costo_Constancia.Enabled = false;
                    Txt_Total_Impuesto.Enabled = false;
                    Txt_Cuenta_Predial.Enabled = false;
                    Txt_Contrarecibo.Enabled = false;
                    Txt_Recargos.Enabled = false;
                    Txt_Multas.Enabled = false;
                    //Txt_Subtotal.Enabled = false;
                    //Txt_Monto_Multas.Enabled = false;
                    //Txt_Monto_Recargos.Enabled = false;
                    Txt_Total_Por_Pagar.Enabled = false;
                    Txt_Realizo.Enabled = false;
                    Txt_Fecha_Inicial.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Today);
                    Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(DateTime.Now.ToShortDateString(), "1");
                    Txt_Fecha_Vencimiento.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                    Txt_Fecha_Inicial.Enabled = false;
                    Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuento = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();
                    Txt_No_Descuento.Text = Descuento.Ultimo_Numero_Descuento();
                    Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;
                    Btn_Modificar.Visible = false;
                    Grid_Descuentos_Traslado.Visible = false;
                    Txt_Fecha_Vencimiento.Enabled = false;
                    Div_Descuentos_Traslado.Visible = true;
                    titulo.Visible = false;
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar un Resgistro de la Tabla el cual no tenga ya un Descuento.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;

                }
            }
            else
            {
                DateTime Fecha_Tramite;
                DateTime Fecha_Vencimiento;

                // crear transaccion para modificar tabla de calculos y de adeudos folio
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;

                if (Validar_Componentes_Generales() && Validar_Porcentaje())
                {
                    DateTime.TryParse(Txt_Fecha_Inicial.Text, out Fecha_Tramite);
                    DateTime.TryParse(Txt_Fecha_Vencimiento.Text, out Fecha_Vencimiento);
                    Div_Descuentos_Traslado.Visible = true;
                    Descuentos.P_No_Descuento = Txt_No_Descuento.Text.Trim();
                    Descuentos.P_No_Calculo = Txt_No_Calculo.Text.Trim();
                    Descuentos.P_Anio_Calculo = Txt_Anio_Calculo.Text.Trim();
                    Descuentos.P_No_Adeudo = Txt_No_Adeudo.Text.Trim();
                    Descuentos.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                    Descuentos.P_Fecha = Fecha_Tramite;
                    Descuentos.P_Desc_Multa = Txt_Desc_Multas.Text.Trim();
                    Descuentos.P_Desc_Recargo = Txt_Desc_Recargo.Text.Trim();
                    Descuentos.P_Total_Por_Pagar = Txt_Total_Por_Pagar.Text.Replace("$", "").Replace(",", "").Trim();
                    Descuentos.P_Total_Impuesto = Txt_Total_Impuesto.Text.Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Realizo = Txt_Realizo.Text.ToUpper().Trim();
                    Descuentos.P_Fecha_Vencimiento = Fecha_Vencimiento;
                    Descuentos.P_Observaciones = Txt_Observaciones.Text.ToUpper().Trim();
                    Descuentos.P_Monto_Multa = Txt_Monto_Multas.Text.Replace("$", "").Replace(",", "").Trim();
                    Descuentos.P_Monto_Recargo = Txt_Monto_Multas.Text.Replace("$", "").Replace(",", "").Trim();
                    Descuentos.P_Desc_Monto_Recargos = Txt_Monto_Recargos.Text.Replace("$", "").Replace(",", "").Trim();
                    Descuentos.P_Desc_Monto_Multas = Txt_Monto_Multas.Text.Replace("$", "").Replace(",", "").Trim();
                    Descuentos.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Descuentos.P_Comando_Transaccion = Cmd;
                    //Limpiar_Catalogo();
                    //Ver_Columnas();
                    Descuentos.Alta_Descuentos_Traslado();
                    Insertar_Pasivo(Cmd);
                    // aplicar cambios a la base de datos 
                    Trans.Commit();

                    //Ocultar_Columnas();
                    Imprimir_Reporte(Crear_Ds_Descuentos_Traslado(), "Rpt_Pre_Descuentos_Traslado.rpt", "Descuentos de Traslado");
                    Configuracion_Formulario(true);
                    Llenar_Tabla_Contrarecibos(Grid_Descuentos_Traslado.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Proceso de Descuentos de Traslado", "alert('Alta de Descuento Exitosa');", true);
                    Btn_Nuevo.AlternateText = "Nuevo";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Grid_Descuentos_Traslado.Enabled = true;
                    Btn_Salir.AlternateText = "Salir";
                    titulo.Visible = false;
                }
            }
        }
        catch (Exception Ex)
        {
            if (Cmd != null)
            {
                Trans.Rollback();
            }
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
        finally
        {
            Cn.Close();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();

        OracleConnection Cn = new OracleConnection();
        OracleCommand Cmd = new OracleCommand();
        OracleTransaction Trans = null;

        try
        {
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                Descuentos.P_No_Descuento = Grid_Descuentos_Traslado.SelectedRow.Cells[11].Text;
                DataTable Dt_Datos_Convenios = Descuentos.Consultar_Convenios_Descuentos_Traslado();
                if (Dt_Datos_Convenios != null && Dt_Datos_Convenios.Rows.Count > 0)
                {
                    foreach (DataRow Convenio in Dt_Datos_Convenios.Rows)
                    {
                        // si se hay convenios de traslado con estatus VIGENTE o TERMIANDO
                        if (Dt_Datos_Convenios.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus].ToString() == "ACTIVO" ||
                            Dt_Datos_Convenios.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus].ToString() == "TERMINADO")
                        {
                            //Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro con Descuento que se desea Modificar.";
                            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                            Lbl_Ecabezado_Mensaje.Text = "Descuento ya aplicado a convenio ";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                            return;
                        }
                    }
                }

                if ((Grid_Descuentos_Traslado.Rows.Count > 0 && Grid_Descuentos_Traslado.SelectedIndex > (-1))
                    && (!string.IsNullOrEmpty(Txt_No_Descuento.Text) && Grid_Descuentos_Traslado.SelectedRow.Cells[9].Text == "VIGENTE"
                    || Grid_Descuentos_Traslado.SelectedRow.Cells[9].Text == "BAJA"
                    || Grid_Descuentos_Traslado.SelectedRow.Cells[9].Text == "VENCIDO"))
                {
                    Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                    DateTime Fecha_Vencimiento;

                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Imprimir.Visible = false;
                    Txt_Cuenta_Predial.Enabled = false;
                    Txt_Contrarecibo.Enabled = false;
                    Txt_Impuesto_Division.Enabled = false;
                    Txt_Impuesto_Traslado.Enabled = false;
                    Txt_Costo_Constancia.Enabled = false;
                    Txt_Total_Impuesto.Enabled = false;
                    Txt_Fecha_Inicial.Enabled = false;
                    //Txt_Monto_Recargos.Enabled = false;
                    //Txt_Monto_Multas.Enabled = false;
                    Txt_Recargos.Enabled = false;
                    Txt_Multas.Enabled = false;
                    Txt_Total_Por_Pagar.Enabled = false;
                    Grid_Descuentos_Traslado.Visible = false;
                    Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;
                    Txt_Realizo.Enabled = false;
                    Txt_Fecha_Vencimiento.Enabled = false;
                    Div_Descuentos_Traslado.Visible = true;
                    titulo.Visible = false;
                    Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(DateTime.Now.ToShortDateString(), "1");
                    Txt_Fecha_Vencimiento.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                }
                else
                {
                }
            }
            else
            {
                DateTime Fecha_Tramite;
                DateTime Fecha_Vencimiento;

                // crear transaccion para modificar tabla de calculos y de adeudos folio
                Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
                Cn.Open();
                Trans = Cn.BeginTransaction();
                Cmd.Connection = Cn;
                Cmd.Transaction = Trans;

                if (Validar_Componentes_Generales() && Validar_Porcentaje())
                {
                    DateTime.TryParse(Txt_Fecha_Inicial.Text, out Fecha_Tramite);
                    DateTime.TryParse(Txt_Fecha_Vencimiento.Text, out Fecha_Vencimiento);

                    Descuentos.P_No_Descuento = Txt_No_Descuento.Text.ToUpper().Trim();
                    Descuentos.P_Estatus = Cmb_Estatus.SelectedItem.Text.Trim();
                    Descuentos.P_Desc_Multa = Txt_Desc_Multas.Text.ToUpper().Trim();
                    Descuentos.P_Desc_Recargo = Txt_Desc_Recargo.Text.ToUpper().Trim();
                    Descuentos.P_Total_Por_Pagar = Txt_Total_Por_Pagar.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Total_Impuesto = Txt_Total_Impuesto.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Realizo = Txt_Realizo.Text.ToUpper().Trim();
                    Descuentos.P_Fecha = Fecha_Tramite;
                    Descuentos.P_Fecha_Vencimiento = Fecha_Vencimiento;
                    Descuentos.P_Observaciones = Txt_Observaciones.Text.ToUpper().Trim();
                    Descuentos.P_No_Adeudo = Txt_No_Adeudo.Text.ToUpper().Trim();
                    Descuentos.P_Monto_Multa = Txt_Monto_Multas.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Monto_Recargo = Txt_Monto_Recargos.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Monto_Traslado = Txt_Impuesto_Traslado.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Monto_Division = Txt_Impuesto_Division.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Costo_Constancia = Txt_Costo_Constancia.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Desc_Monto_Recargos = Txt_Monto_Recargos.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Desc_Monto_Multas = Txt_Monto_Multas.Text.ToUpper().Trim().Replace("$", "").Trim().Replace(",", "").Trim();
                    Descuentos.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                    Descuentos.P_Comando_Transaccion = Cmd;
                    Ver_Columnas();
                    Descuentos.Modificar_Descuentos_Traslado();
                    Ocultar_Columnas();
                    Insertar_Pasivo(Cmd);
                    // aplicar cambios a la base de datos 
                    Trans.Commit();

                    Configuracion_Formulario(true);
                    Limpiar_Catalogo();
                    Llenar_Tabla_Contrarecibos(Grid_Descuentos_Traslado.PageIndex);
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Descuentos de Traslado", "alert('Actualización de Descuento de Traslado Exitoso');", true);
                    Btn_Modificar.AlternateText = "Modificar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    Btn_Salir.AlternateText = "Salir";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Salir.AlternateText = "Salir";
                    titulo.Visible = false;
                    Grid_Descuentos_Traslado.Enabled = true;
                    Div_Descuentos_Traslado.Visible = false;
                }
            }
        }
        //}
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }

    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: Llena la Tabla de Descuentos de Traslado con la opcion buscada
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Cls_Ope_Pre_Descuentos_Traslado_Negocio desc = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();
            Limpiar_Catalogo();
            Llenar_Tabla_Descuentos_Traslado_Busqueda(0);

            if (Grid_Descuentos_Traslado.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el Concepto\"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias o el movimiento tiene un estatus diferente a POR PAGAR";
                Lbl_Mensaje_Error.Text = "(Se cargaron  todos los Contrarecibos almacenados)";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                Llenar_Tabla_Contrarecibos(0);
            }
            Div_Descuentos_Traslado.Visible = false;
            Txt_Busqueda.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Contrarecibos_Pendientes
    ///DESCRIPCIÓN          : Llena el grid de Convenios con los registros encontrados
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Contrarecibos_Pendientes(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Descuentos_Traslado_Negocio Calculos_Impuesto_Traslado = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();

            DataTable Dt_Orden;

            //Calculos_Impuesto_Traslado.P_Estatus = "LISTO' OR CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " = 'POR PAGAR";
            if (Txt_Busqueda.Text.Trim() != "")
            {
                switch (Cmb_Busqueda_General.SelectedIndex)
                {
                    case 0://POR CONTRARECIBO
                        Calculos_Impuesto_Traslado.P_No_Contrarecibo = " IN (SELECT " + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " FROM " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " WHERE " + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo + " = '" + Txt_Busqueda.Text.Trim() + "')";
                        break;
                    case 1://POR CUENTA
                        Calculos_Impuesto_Traslado.P_Cuenta_Predial_ID = " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Busqueda.Text.Trim() + "')";
                        break;
                    case 2://POR FOLIO
                        Calculos_Impuesto_Traslado.P_Folio = " IN (SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Convert.ToInt64(Txt_Busqueda.Text.Trim()).ToString("0000000000") + "')";
                        break;
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
    ///NOMBRE DE LA FUNCIÓN: Btn_Imprimir_Click
    ///DESCRIPCIÓN: Imprime un documento con la informacion del Descuento de Traslado
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 23/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Txt_No_Descuento.Text != " ")
            {
                if (Grid_Descuentos_Traslado.Rows.Count > 0 && Grid_Descuentos_Traslado.SelectedIndex > (-1))
                {
                    Imprimir_Reporte(Crear_Ds_Descuentos_Traslado(), "Rpt_Pre_Descuentos_Traslado.rpt", "Descuentos de Traslado");
                }
                else
                {
                    Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Imprimir.";
                    Lbl_Mensaje_Error.Text = "";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Ecabezado_Mensaje.Text = "El registro que intento imprimir no tiene un Descuento asignado";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;

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
    ///NOMBRE DE LA FUNCIÓN: Impresion_Guardar
    ///DESCRIPCIÓN: Imprime al momento de dar click en guaradr
    ///PROPIEDADES:     
    ///CREO: Jacqueline Ramirez Sierra
    ///FECHA_CREO: 6/Octubre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Impresion_Guardar(object sender, EventArgs e)
    {
        try
        {
            if (Txt_No_Descuento.Text != " ")
            {

                Imprimir_Reporte(Crear_Ds_Descuentos_Traslado(), "Rpt_Pre_Descuentos_Traslado.rpt", "Descuentos de Traslado");
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
    ///FECHA_CREO: 15/Agosto/2011 
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
                Llenar_Tabla_Contrarecibos(0);
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Imprimir.Visible = false;
                Grid_Descuentos_Traslado.Enabled = true;
                Grid_Descuentos_Traslado.Visible = true;
                Div_Descuentos_Traslado.Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN: Txt_Desc_Recargo_TextChanged
    ///DESCRIPCIÓN: Manda ejecutar los metodos para calcular el Subtotal.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Desc_Recargo_TextChanged(object sender, EventArgs e)
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Calcular_Monto_Descuento_Recargos();
        Validar_Descuentos();
        Calcular_Total();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Desc_Multas_TextChanged
    ///DESCRIPCIÓN: Manda llamar los metodos que hacen el calculo del Subtotal.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Desc_Multas_TextChanged(object sender, EventArgs e)
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Calcular_Monto_Descuento_Multas();
        Validar_Descuentos();
        Calcular_Total();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Desc_Recargo_TextChanged
    ///DESCRIPCIÓN: Manda ejecutar los metodos para calcular el Subtotal.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Monto_Recargos_TextChanged(object sender, EventArgs e)
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Calcular_Porcentaje_Descuento_Recargos();
        Validar_Descuentos();
        Calcular_Total();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Desc_Multas_TextChanged
    ///DESCRIPCIÓN: Manda llamar los metodos que hacen el calculo del Subtotal.
    ///PROPIEDADES:     
    ///CREO: José Alfredo García Pichardo.
    ///FECHA_CREO: 15/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Txt_Monto_Multas_TextChanged(object sender, EventArgs e)
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Div_Contenedor_Msj_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Calcular_Porcentaje_Descuento_Multas();
        Validar_Descuentos();
        Calcular_Total();
    }

    #endregion

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
        var Descuento_Autorizado = new Cls_Ope_Pre_Descuentos_Predial_Negocio();
        Decimal Porcentaje_Descuento_Recargos = 0;
        Decimal Porcentaje_Descuento_Multas = 0;
        Decimal Porcentaje_Autorizado_Multas = 0;
        Decimal Porcentaje_Autorizado_Traslado = 0;
        String Str_Mensaje_Error = "";
        Boolean Validacion = false;

        // consultar descuento permitido para usuario actual
        Descuento_Autorizado.P_Usuario = Cls_Sessiones.Empleado_ID;
        Descuento_Autorizado.P_Tipo_Descuento = "MULTAS";
        String Porcentaje = Descuento_Autorizado.Traer_Descuento();
        Decimal.TryParse(Porcentaje, out Porcentaje_Autorizado_Multas);
        Descuento_Autorizado.P_Tipo_Descuento = "TRASLADO";
        Porcentaje = Descuento_Autorizado.Traer_Descuento();
        Decimal.TryParse(Porcentaje, out Porcentaje_Autorizado_Traslado);

        // recuperar porcentaje de descuentos
        Decimal.TryParse(Txt_Desc_Recargo.Text, out Porcentaje_Descuento_Recargos);
        Decimal.TryParse(Txt_Desc_Multas.Text, out Porcentaje_Descuento_Multas);

        // validar que el porcentaje del descuento no sea mayor que el autorizado
        if (Porcentaje_Descuento_Recargos > Porcentaje_Autorizado_Traslado)
        {
            Str_Mensaje_Error += "No tiene autorización para asignar descuentos mayores a " + Porcentaje_Autorizado_Traslado + "%<br />";
            Txt_Desc_Recargo.Text = Porcentaje_Autorizado_Traslado.ToString("0.##");
            Calcular_Monto_Descuento_Recargos();
            Validacion = true;
        }
        if (Porcentaje_Descuento_Multas > Porcentaje_Autorizado_Multas)
        {
            Str_Mensaje_Error += "No tiene autorización para asignar descuentos mayores a " + Porcentaje_Autorizado_Multas + "%<br />";
            Txt_Desc_Multas.Text = Porcentaje_Autorizado_Multas.ToString("0.##");
            Calcular_Monto_Descuento_Multas();
            Validacion = true;
        }

        // validar que el porcentaje no sea mayor que 100
        if (Porcentaje_Descuento_Recargos > 100)
        {
            Str_Mensaje_Error += "No es posible asignar descuentos mayores a 100%<br />";
            Txt_Desc_Recargo.Text = Porcentaje_Autorizado_Traslado.ToString("0.##");
            Calcular_Monto_Descuento_Recargos();
            Validacion = true;
        }

        // validar que el porcentaje no sea mayor que 100
        if (Porcentaje_Descuento_Multas > 100)
        {
            Str_Mensaje_Error += "No es posible asignar descuentos mayores a 100%<br />";
            Txt_Desc_Multas.Text = Porcentaje_Autorizado_Multas.ToString("0.##");
            Calcular_Monto_Descuento_Multas();
            Validacion = true;
        }
        if (Validacion)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Str_Mensaje_Error;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    #region Impresion Folios

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    //private void Imprimir_Reporte(DataTable Dt_Impuestos_Descuentos_Traslado, String Nombre_Reporte, String Nombre_Archivo)
    //{
    //    ReportDocument Reporte = new ReportDocument();
    //    String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
    //    Reporte.Load(File_Path);
    //    Reporte.SetDataSource(Dt_Impuestos_Descuentos_Traslado);

    //    String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
    //    ExportOptions Export_Options = new ExportOptions();
    //    DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
    //    Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
    //    Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
    //    Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
    //    Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

    //    Reporte.Export(Export_Options);
    //    Mostrar_Reporte(Archivo_PDF, "PDF");
    //}

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 23/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Impuestos_Descuentos_Traslado, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Impuestos_Descuentos_Traslado);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
            Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
            Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
            Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options);
        }
        catch //(Exception Ex)
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Reporte
    ///DESCRIPCIÓN          : Visualiza en pantalla el reporte indicado
    ///PARAMETROS           : Nombre_Reporte: cadena con el nombre del archivo.
    ///                     : Formato: Exensión del archivo a visualizar.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 22/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt", "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Descuentos_Traslado
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos del
    ///                     Descuento de Traslado Seleccionado en el GridView
    ///PARAMETROS: 
    ///CREO                 : José Alfredo García Pichardo
    ///FECHA_CREO           : 23/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Descuentos_Traslado()
    {
        Ds_Ope_Pre_Descuentos_Traslado Ds_Descuentos_Traslado = new Ds_Ope_Pre_Descuentos_Traslado();
        Cls_Ope_Pre_Descuentos_Traslado_Negocio Descuentos_Traslado = new Cls_Ope_Pre_Descuentos_Traslado_Negocio();
        var Consulta_Datos_Cuenta = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        DataTable Dt_Datos_Cuenta;

        DataTable Dt_Temp;
        DataRow Dr_Descuentos_Traslado;


        foreach (DataTable Dt_Descuentos_Traslado in Ds_Descuentos_Traslado.Tables)
        {
            if (Dt_Descuentos_Traslado.TableName == "Dt_Descuentos_Traslado")
            {
                Descuentos_Traslado.P_No_Descuento = Txt_No_Descuento.Text.Trim();
                Dt_Temp = Descuentos_Traslado.Consultar_Descuentos_Traslado();

                foreach (DataRow Dr_Temp in Dt_Temp.Rows)
                {
                    // obtener informacion de orden de variacion
                    int Anio_Orden;
                    int.TryParse(Grid_Descuentos_Traslado.SelectedRow.Cells[13].Text, out Anio_Orden);
                    Consulta_Datos_Cuenta.P_No_Orden_Variacion = Grid_Descuentos_Traslado.SelectedRow.Cells[14].Text;
                    Consulta_Datos_Cuenta.P_Anio_Orden = Anio_Orden;
                    Consulta_Datos_Cuenta.P_Incluir_Campos_Foraneos = true;
                    Dt_Datos_Cuenta = Consulta_Datos_Cuenta.Consultar_Ordenes_Variacion();

                    //Inserta los datos de los Impuestos de Derechos de Supervision en la Tabla
                    Dr_Descuentos_Traslado = Dt_Descuentos_Traslado.NewRow();

                    // validar que la consulta trae resultados y agregar datos de cuenta
                    if (Dt_Datos_Cuenta != null && Dt_Datos_Cuenta.Rows.Count > 0)
                    {
                        Dr_Descuentos_Traslado["NO_EXTERIOR"] = Dt_Datos_Cuenta.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString();
                        Dr_Descuentos_Traslado["NO_INTERIOR"] = Dt_Datos_Cuenta.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString();
                        Dr_Descuentos_Traslado["CALLE"] = Dt_Datos_Cuenta.Rows[0]["NOMBRE_CALLE_UBICACION"].ToString();
                        Dr_Descuentos_Traslado["COLONIA"] = Dt_Datos_Cuenta.Rows[0]["NOMBRE_COLONIA_UBICACION"].ToString();
                        Dr_Descuentos_Traslado["PROPIETARIO"] = Dt_Datos_Cuenta.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                    }
                    decimal Recargos;
                    decimal Multas;
                    decimal Porciento_Descuento_Recargos;
                    decimal Monto_Descuento_Recargos;
                    decimal Porciento_Descuento_Multas;
                    decimal Monto_Descuento_Multas;

                    Dr_Descuentos_Traslado["NO_CONTRARECIBO"] = Dr_Temp[Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo];
                    Dr_Descuentos_Traslado["CUENTA_PREDIAL"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial];
                    Dr_Descuentos_Traslado["FECHA_INICIAL"] = String.Format("{0:dddd/ MMMM d/ yyyy}", Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Fecha_Inicial + "_INICIAL"]);
                    Dr_Descuentos_Traslado["MONTO_RECARGO"] = String.Format("{0:$#,##0.00}", Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos]);
                    Dr_Descuentos_Traslado["DESC_RECARGO"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo];
                    Dr_Descuentos_Traslado["MONTO_MULTA"] = String.Format("{0:$#,##0.00}", Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa]);
                    Dr_Descuentos_Traslado["DESC_MULTA"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Desc_Multa];
                    Dr_Descuentos_Traslado["TOTAL_POR_PAGAR"] = String.Format("{0:$#,##0.00}", Dr_Temp["TOTAL"]);
                    Dr_Descuentos_Traslado["ESTATUS"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Estatus];
                    Dr_Descuentos_Traslado["NO_DESCUENTO"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_No_Descuento];
                    Dr_Descuentos_Traslado["NO_CALCULO"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_No_Calculo];
                    Dr_Descuentos_Traslado["ANIO_CALCULO"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo];
                    Dr_Descuentos_Traslado["NO_ORDEN_VARIACION"] = Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion];
                    Dr_Descuentos_Traslado["ANIO_ORDEN"] = Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden];
                    Dr_Descuentos_Traslado["NO_ADEUDO"] = Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_No_Adeudo];
                    Dr_Descuentos_Traslado["REALIZO"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Realizo];
                    Dr_Descuentos_Traslado["FECHA_VENCIMIENTO"] = String.Format("{0:dd/MMM/yyyy}", Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Fecha_Vencimiento]);
                    Dr_Descuentos_Traslado["OBSERVACIONES"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Observaciones];
                    Dr_Descuentos_Traslado["FUNDAMENTO_LEGAL"] = Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Fundamento_Legal];
                    Dr_Descuentos_Traslado["TOTAL"] = String.Format("{0:$#,##0.00}", Dr_Temp["TOTAL"]);
                    Dr_Descuentos_Traslado["COSTO_CONSTANCIA"] = String.Format("{0:$#,##0.00}", Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia]);
                    Dr_Descuentos_Traslado["MONTO_TRASLADO"] = String.Format("{0:$#,##0.00}", Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Traslado]);
                    Dr_Descuentos_Traslado["MONTO_DIVISION"] = String.Format("{0:$#,##0.00}", Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Division]);
                    Dr_Descuentos_Traslado["MONTO_MULTAS"] = String.Format("{0:$#,##0.00}", Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa]);
                    Dr_Descuentos_Traslado["MONTO_RECARGOS"] = String.Format("{0:$#,##0.00}", Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos]);
                    decimal.TryParse(Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Desc_Multa].ToString(), out Porciento_Descuento_Multas);
                    decimal.TryParse(Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo].ToString(), out Porciento_Descuento_Recargos);
                    decimal.TryParse(Dr_Temp[Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa].ToString(), out Multas);
                    decimal.TryParse(Dr_Temp[Ope_Pre_Descuento_Traslado.Campo_Monto_Recargos].ToString(), out Recargos);
                    Monto_Descuento_Multas = Multas * Porciento_Descuento_Multas / 100;
                    Monto_Descuento_Recargos = Recargos * Porciento_Descuento_Recargos / 100;
                    Dr_Descuentos_Traslado["MONTO_DESCUENTO_RECARGOS"] = Monto_Descuento_Recargos.ToString("$#,##0.00");
                    Dr_Descuentos_Traslado["MONTO_DESCUENTO_MULTAS"] = Monto_Descuento_Multas.ToString("$#,##0.00");
                    Dt_Descuentos_Traslado.Rows.Add(Dr_Descuentos_Traslado);
                }
            }

        }

        return Ds_Descuentos_Traslado;
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
            Botones.Add(Btn_Imprimir);
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

    #region Datos OV
    protected void Txt_Cuenta_Predial_TextChanged()
    {
        DataTable Dt_Orden;
        if (Hdf_Cuenta_Predial_ID.Value.Length > 0)
        {
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Cuenta = Cuenta.Consultar_Datos_Propietario();
            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;
            if (Cuenta.P_Propietario_ID != null)
            {
                Hdf_Propietario_ID.Value = Cuenta.P_Propietario_ID;
            }
            Hdf_RFC.Value = Cuenta.P_RFC_Propietario;
        }
        Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Orden.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Orden = Orden.Consultar_Ordenes_Variacion();
        if (Dt_Orden.Rows.Count == 0)
        {
            return;
        }
        Orden.P_Año = Convert.ToInt32(Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio].ToString());
        Orden.P_Orden_Variacion_ID = Dt_Orden.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString();
        Dt_Orden = Orden.Consultar_Domicilio_Y_Propietario();
        if (Dt_Orden.Rows.Count > 0)
        {
            Hdf_Propietario_ID.Value = Dt_Orden.Rows[0]["CONTRIBUYENTE_ID"].ToString();
            String Dom_Foraneo = "";
            String No_int_not = "";
            String No_ext_not = "";
            String No_Int = "";
            String No_Ext = "";
            String Dom_Not_Colonia = "";
            String Dom_Not_Calle = "";
            String Dom_Colonia = "";
            String Dom_Calle = "";
            foreach (DataRow Renglon_Actual in Dt_Orden.Rows)
            {
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString() != "")
                {
                    Dom_Foraneo = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString() != "")
                {
                    No_int_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString() != "")
                {
                    No_ext_not = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString() != "")
                {
                    No_Int = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString() != "")
                {
                    No_Ext = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString() != "")
                {
                    Dom_Not_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString() != "")
                {
                    Dom_Not_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString() != "")
                {
                    Dom_Calle = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString();
                }
                if (Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString() != "")
                {
                    Dom_Colonia = Renglon_Actual[Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString();
                }
            }
            Txt_Propietario.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
            Hdf_RFC.Value = Dt_Orden.Rows[0]["RFC"].ToString();
            if (Dom_Foraneo == "SI" && Dom_Foraneo != "")
            {
                Txt_Calle.Text = Dom_Calle;
                Txt_Colonia.Text = Dom_Colonia;
                Txt_No_Exterior.Text = No_ext_not;
                Txt_No_Interior.Text = No_int_not;
            }
            else if (Dom_Foraneo == "NO" && Dom_Foraneo != "")
            {
                Cls_Cat_Pre_Calles_Negocio Calle = new Cls_Cat_Pre_Calles_Negocio();
                Calle.P_Calle_ID = Dom_Not_Calle;
                Calle.P_Mostrar_Nombre_Calle_Nombre_Colonia = true;
                if (Calle.P_Calle_ID != "")
                {
                    DataTable Dt_Calle_Colonia = Calle.Consultar_Nombre_Id_Calles();
                    String[] Calle_Col = Dt_Calle_Colonia.Rows[0]["NOMBRE"].ToString().Split('-');
                    Txt_Calle.Text = Calle_Col[0];
                    Txt_Colonia.Text = Calle_Col[1];
                }
                Txt_No_Exterior.Text = No_Ext;
                Txt_No_Interior.Text = No_Int;
            }
        }
        //}
    }
    #endregion
}
