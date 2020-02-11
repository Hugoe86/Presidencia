using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Convenios_Impuestos_Traslado_Dominio.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Parametros.Negocios;
using CrystalDecisions.CrystalReports.Engine;
using Operacion_Predial_Orden_Variacion.Negocio;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Ventanas_Emergentes_Convenios_Frm_Convenios_Traslado : System.Web.UI.Page
{

    #region Pago_Load

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
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Hdf_Cuenta_Predial_ID.Value = Request.QueryString["Cuenta_Predial_ID"].ToString();// Session["CTRA_CUENTA_PREDIAL_ID"].ToString();
            Hdf_No_Convenio.Value = Request.QueryString["No_Convenio"].ToString();//Session["CTRA_NO_CONVENIO"].ToString();
            Cargar_Convenio();
            Txt_Cuenta_Predial_TextChanged();
            Session.Remove("ESTATUS_CUENTAS");
            Session.Remove("TIPO_CONTRIBUYENTE");

            Cargar_Ventana_Emergente_Resolucion();
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Resolucion
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente de la Resolución con la ruta y parámetros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Resolucion()
    {
        String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Resumen_Predial/Frm_Resolucion.aspx";
        String Parametros = "?No_Calculo=" + Hdf_No_Calculo.Value;
        Parametros += "&Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value;
        Parametros += "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text;
        if (Txt_Fecha_Calculo.Text.Trim() != "")
        {
            Parametros += "&Año=" + Convert.ToDateTime(Txt_Fecha_Calculo.Text.Trim()).Year.ToString();
        }
        Parametros += "'";
        String Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:800px;dialogHide:true;help:no;scroll:on');";
        Btn_Detalles_Calculo.Attributes.Add("onclick", Ventana_Modal + Parametros + Propiedades);
    }

    #endregion

    private String Boton_Pulsado = "";
    private Boolean No_Limpiar_Campos_Clave = false;

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Configuracion_Formulario
    ///DESCRIPCIÓN          : Carga una configuracion de los controles del Formulario
    ///PARAMETROS           : 1. Estatus.    Estatus en el que se cargara la configuración de los controles.
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean Estatus)
    {
        //Btn_Nuevo.Visible = true;
        //Btn_Nuevo.AlternateText = "Nuevo";
        //Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
        //Btn_Modificar.Visible = true;
        //Btn_Modificar.AlternateText = "Modificar";
        //Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Monto_Impuesto.Enabled = false;
        Txt_Estatus_Calculo.Enabled = false;
        Txt_Monto_Recargos.Enabled = false;
        Txt_Monto_Multas.Enabled = false;
        Txt_Costo_Constancia.Enabled = false;
        Txt_Realizo_Calculo.Enabled = false;
        Txt_Fecha_Calculo.Enabled = false;
        //Convenio
        Txt_Numero_Convenio.Enabled = false;
        Txt_Realizo.Enabled = false;
        Txt_Fecha.Enabled = false;
        //Descuentos
        Txt_Total_Adeudo.Enabled = false;
        Txt_Total_Descuento.Enabled = false;
        Txt_Sub_Total.Enabled = false;
        Txt_Total_Convenio.Enabled = false;
        if (Cmb_Tipo_Solicitante.SelectedValue == "DEUDOR SOLIDARIO")
        {
            Txt_Solicitante.Enabled = true;
            Txt_RFC.Enabled = true;
        }
        else
        {
            Txt_Solicitante.Enabled = false;
            Txt_RFC.Enabled = false;
        }
        if (Boton_Pulsado == "Btn_Nuevo" || Boton_Pulsado == "")
        {
            Grid_Parcialidades.Enabled = true;
            Grid_Parcialidades.SelectedIndex = (-1);
            //Convenio
            Cmb_Estatus.Enabled = false;
            Cmb_Tipo_Solicitante.Enabled = !Estatus;
            Cmb_Periodicidad_Pago.Enabled = !Estatus;
            Txt_Numero_Parcialidades.Enabled = !Estatus;
            Txt_Observaciones.Enabled = !Estatus;
            //Descuentos
            Txt_Descuento_Recargos_Ordinarios.Enabled = false;
            Txt_Descuento_Recargos_Moratorios.Enabled = false;
            Txt_Descuento_Multas.Enabled = false;
            //Parcialidades
            Txt_Porcentaje_Anticipo.Enabled = !Estatus;
            Txt_Total_Anticipo.Enabled = !Estatus;
        }
        else
        {
            if (Boton_Pulsado == "Btn_Modificar")
            {
                Grid_Parcialidades.Enabled = true;
                Grid_Parcialidades.SelectedIndex = (-1);
                //Convenio
                Cmb_Estatus.Enabled = false;
                Cmb_Tipo_Solicitante.Enabled = !Estatus;
                Cmb_Periodicidad_Pago.Enabled = !Estatus;
                Txt_Numero_Parcialidades.Enabled = !Estatus;
                Txt_Observaciones.Enabled = !Estatus;
                //Descuentos
                Txt_Descuento_Recargos_Ordinarios.Enabled = !Estatus;
                Txt_Descuento_Recargos_Moratorios.Enabled = !Estatus;
                Txt_Descuento_Multas.Enabled = !Estatus;
                //Parcialidades
                Txt_Porcentaje_Anticipo.Enabled = !Estatus;
                Txt_Total_Anticipo.Enabled = !Estatus;
            }
        }

        Txt_Propietario.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_Calle.Enabled = false;
        Txt_No_Exterior.Enabled = false;
        Txt_No_Interior.Enabled = false;

        Txt_Monto_Impuesto.Style["text-align"] = "right";
        Txt_Monto_Recargos.Style["text-align"] = "right";
        Txt_Monto_Multas.Style["text-align"] = "right";
        Txt_Descuento_Recargos_Ordinarios.Style["text-align"] = "right";
        Txt_Descuento_Recargos_Moratorios.Style["text-align"] = "right";
        Txt_Descuento_Multas.Style["text-align"] = "right";
        Txt_Total_Adeudo.Style["text-align"] = "right";
        Txt_Total_Descuento.Style["text-align"] = "right";
        Txt_Sub_Total.Style["text-align"] = "right";
        Txt_Porcentaje_Anticipo.Style["text-align"] = "right";
        Txt_Total_Anticipo.Style["text-align"] = "right";
        Txt_Total_Convenio.Style["text-align"] = "right";
        Txt_No_Exterior.Style["text-align"] = "right";
        Txt_Costo_Constancia.Style["text-align"] = "right";
        //Grid_Convenios_Impuestos_Traslado.Visible=
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        if (!No_Limpiar_Campos_Clave)
        {
            Hdf_Cuenta_Predial_ID.Value = "";
            Hdf_Propietario_ID.Value = "";
            Session["Cuenta_Predial"] = null;
        }
        //Datos Cuenta
        Txt_Cuenta_Predial.Text = "";
        Txt_Monto_Impuesto.Text = "";
        Txt_Estatus_Calculo.Text = "";
        Txt_Monto_Recargos.Text = "";
        Txt_Monto_Multas.Text = "";
        Txt_Realizo_Calculo.Text = "";
        Txt_Fecha_Calculo.Text = "";
        Txt_Costo_Constancia.Text = "";
        //Convenio
        Txt_Numero_Convenio.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Solicitante.Text = "";
        Txt_RFC.Text = "";
        Cmb_Tipo_Solicitante.SelectedIndex = 0;
        Txt_Numero_Parcialidades.Text = "";
        Cmb_Periodicidad_Pago.SelectedIndex = 0;
        Txt_Realizo.Text = "";
        Txt_Fecha.Text = "";
        Txt_Observaciones.Text = "";
        Hdf_RFC.Value = "";
        //Descuentos
        Txt_Descuento_Recargos_Ordinarios.Text = "";
        Txt_Descuento_Recargos_Moratorios.Text = "";
        Txt_Descuento_Multas.Text = "";
        Txt_Total_Adeudo.Text = "";
        Txt_Total_Descuento.Text = "";
        Txt_Sub_Total.Text = "";
        //Parcialidades
        Txt_Porcentaje_Anticipo.Text = "";
        Txt_Total_Anticipo.Text = "";
        Txt_Total_Convenio.Text = "";
        Grid_Parcialidades.DataSource = null;
        Grid_Parcialidades.DataBind();
        Txt_Propietario.Text = "";
        Txt_Colonia.Text = "";
        Txt_Calle.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
    }

    ///*******************************************************************************
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
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculos_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            DataTable Dt_Calculos_Impuesto_Traslado;

            Calculos_Impuesto_Traslado.P_Estatus = "LISTO";
            Dt_Calculos_Impuesto_Traslado = Calculos_Impuesto_Traslado.Consulta_Calculos_Contrarecibo();

            if (Dt_Calculos_Impuesto_Traslado != null)
            {
                if (!(Dt_Calculos_Impuesto_Traslado.Rows.Count == 0))
                {
                    foreach (DataRow Renglon_Actual in Dt_Calculos_Impuesto_Traslado.Rows)
                    {
                        if (Renglon_Actual["MONTO_RECARGOS"].ToString() == "")
                        {
                            Renglon_Actual["MONTO_RECARGOS"] = "0.00";
                        }
                        if (Renglon_Actual["DESC_RECARGO"].ToString() == "")
                        {
                            Renglon_Actual["DESC_RECARGO"] = "0.00";
                        }
                        if (Renglon_Actual["MONTO_MULTA"].ToString() == "")
                        {
                            Renglon_Actual["MONTO_MULTA"] = "0.00";
                        }
                        if (Renglon_Actual["DESC_MULTA"].ToString() == "")
                        {
                            Renglon_Actual["DESC_MULTA"] = "0.00";
                        }
                        if (Renglon_Actual["MONTO_TOTAL_CALCULO"].ToString() == "")
                        {
                            Renglon_Actual["MONTO_TOTAL_CALCULO"] = "0.00";
                        }
                        if (Renglon_Actual["COSTO_CONSTANCIA"].ToString() == "")
                        {
                            Renglon_Actual["COSTO_CONSTANCIA"] = "0.00";
                        }
                    }
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
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Parcialidades
    ///DESCRIPCIÓN          : Lee el grid de las parcialidades y devuelve una instancia en un DataTable
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Crear_Tabla_Parcialidades()
    {
        DataTable Dt_Parcialidades = new DataTable();
        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(String)));
        Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("CONSTANCIA", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_MULTAS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Double)));
        Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
        Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));

        DataRow Dr_Parcialidades;
        //Se barre el Grid para cargar el DataTable con los valores del grid
        foreach (GridViewRow Row in Grid_Parcialidades.Rows)
        {
            Dr_Parcialidades = Dt_Parcialidades.NewRow();
            Dr_Parcialidades["NO_PAGO"] = Row.Cells[0].Text;
            Dr_Parcialidades["HONORARIOS"] = Convert.ToDouble(Row.Cells[1].Text.Replace("$", ""));
            Dr_Parcialidades["CONSTANCIA"] = Convert.ToDouble(Row.Cells[2].Text.Replace("$", ""));
            Dr_Parcialidades["MONTO_MULTAS"] = Convert.ToDouble(Row.Cells[3].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_MORATORIOS"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
            Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDouble(Row.Cells[6].Text.Replace("$", ""));
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Convert.ToDateTime(Row.Cells[8].Text);
            Dr_Parcialidades["ESTATUS"] = Row.Cells[9].Text;
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
        }
        return Dt_Parcialidades;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Convenio
    ///DESCRIPCIÓN          : Llena la tabla de Convenios por Derechos de Supervisión con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Convenio()
    {
        try
        {
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            DataTable Dt_Convenios_Traslado_Dominio;

            Convenios_Traslado_Dominio.P_Campos_Foraneos = true;
            if (Hdf_No_Convenio.Value != "")
            {
                Convenios_Traslado_Dominio.P_No_Convenio = Hdf_No_Convenio.Value;
            }
            else
            {
                if (Hdf_Cuenta_Predial_ID.Value != "")
                {
                    Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                }
            }
            if (Hdf_No_Convenio.Value != "" || Hdf_Cuenta_Predial_ID.Value != "")
            {
                Dt_Convenios_Traslado_Dominio = Convenios_Traslado_Dominio.Consultar_Convenio_Traslado_Dominio();

                if (Dt_Convenios_Traslado_Dominio != null)
                {
                    if (Dt_Convenios_Traslado_Dominio.Rows.Count > 0)
                    {
                        Hdf_Cuenta_Predial_ID.Value = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID].ToString();
                        Hdf_Propietario_ID.Value = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Propietario_ID].ToString();
                        Txt_Cuenta_Predial.Text = Dt_Convenios_Traslado_Dominio.Rows[0]["Cuenta_Predial"].ToString();
                        //Txt_Clasificacion.Text = Dt_Convenios_Traslado_Dominio.Rows[0]["Tipo_Predio"].ToString();
                        Hdf_No_Calculo.Value = Dt_Convenios_Traslado_Dominio.Rows[0]["No_Calculo"].ToString();
                        Hdf_Anio_Calculo.Value = Dt_Convenios_Traslado_Dominio.Rows[0]["Anio"].ToString();
                        Consultar_Datos_Cuenta_Predial();
                        Txt_Numero_Convenio.Text = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio].ToString();
                        Cmb_Estatus.SelectedValue = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus].ToString();
                        if (Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Propietario_ID] != null
                            && Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Propietario_ID].ToString() != "")
                        {
                            Txt_Solicitante.Text = Dt_Convenios_Traslado_Dominio.Rows[0]["Nombre_Propietario"].ToString();
                            Cmb_Tipo_Solicitante.SelectedIndex = 0;
                        }
                        else
                        {
                            if (Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Solicitante].ToString() != "")
                            {
                                Txt_Solicitante.Text = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Solicitante].ToString();
                                Txt_RFC.Text = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_RFC].ToString();
                                Cmb_Tipo_Solicitante.SelectedIndex = 1;
                            }
                        }
                        Txt_Numero_Parcialidades.Text = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Numero_Parcialidades].ToString();
                        Cmb_Periodicidad_Pago.SelectedValue = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Periodicidad_Pago].ToString();
                        Txt_Realizo.Text = Dt_Convenios_Traslado_Dominio.Rows[0]["Nombre_Realizo"].ToString();
                        Txt_Fecha.Text = Convert.ToDateTime(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
                        Txt_Observaciones.Text = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Observaciones].ToString();
                        Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Ordinarios]).ToString();
                        Txt_Descuento_Recargos_Moratorios.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Moratorios]).ToString();
                        Txt_Descuento_Multas.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Multas]).ToString();
                        Txt_Total_Adeudo.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Adeudo]).ToString();
                        Txt_Total_Descuento.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Descuento]).ToString();
                        Txt_Sub_Total.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Sub_Total]).ToString();
                        Txt_Porcentaje_Anticipo.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Porcentaje_Anticipo]).ToString();
                        Txt_Total_Anticipo.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Anticipo]).ToString();
                        Txt_Total_Convenio.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Convenio]).ToString();
                        Grid_Parcialidades.DataSource = Convenios_Traslado_Dominio.P_Dt_Parcialidades;
                        Grid_Parcialidades.PageIndex = 0;
                        Grid_Parcialidades.DataBind();
                        Sumar_Totales_Parcialidades();
                    }
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

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Lbl_Ecabezado_Mensaje.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Hdf_Cuenta_Predial_ID.Value.Trim() == "" && Txt_Cuenta_Predial.Text != "")
        {
            Consultar_Datos_Cuenta_Predial();
        }
        if (Txt_Cuenta_Predial.Text.Trim().Equals(""))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
            Validacion = false;
        }
        if (Txt_Numero_Parcialidades.Text.Trim() == "")
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introduzca el Número de Parcialidades.";
            Validacion = false;
        }
        if (Cmb_Periodicidad_Pago.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Seleccione un Periodo de Pago.";
            Validacion = false;
        }
        if (Cmb_Estatus.SelectedIndex <= 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Indique un Estatus.";
            Validacion = false;
        }

        if (Convert.ToDouble(Txt_Total_Anticipo.Text) < Convert.ToDouble(Txt_Monto_Multas.Text))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ El anticipo debe de cubrir el monto de las multas.";
            Validacion = false;
        }
        //if (Btn_Nuevo.AlternateText != "Dar de Alta" && Btn_Modificar.AlternateText == "Actualizar")
        //{
        //    if (Txt_Observaciones.Text.Equals(""))
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Introduzca las Observaciones.";
        //        Validacion = false;
        //    }
        //}
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Adeudos_Convenio
    ///DESCRIPCIÓN          : Consulta los adeudos del convenio
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 29/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Adeudos_Convenio(Boolean Total_O_A_Pagar)
    {
        try
        {
            DataTable Dt_Adeudos = new DataTable();
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            Convenio.P_No_Convenio = Hdf_No_Convenio.Value;
            //if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[9].Text.Replace("&nbsp;", "").Equals(""))
            //{
            Convenio.P_Reestructura = false;
            //}
            //else
            //{
            //    Convenio.P_Reestructura = true;
            //}
            Dt_Adeudos = Convenio.Consultar_Adeudos_Convenio(Total_O_A_Pagar);
            if (Dt_Adeudos.Rows.Count > 0)
            {
                Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_IMPUESTO"].ToString()).ToString("0.00");
                Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_MULTAS"].ToString()).ToString("0.00");
                Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_ORDINARIOS"].ToString()).ToString("0.00");
                Txt_Costo_Constancia.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_CONSTANCIA"].ToString()).ToString("0.00");
            }
            else
            {
                Txt_Monto_Impuesto.Text = "0.00";
                Txt_Monto_Multas.Text = "0.00";
                Txt_Monto_Recargos.Text = "0.00";
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
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 26/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO       : Miguel Angel Bedolla Moreno
    ///CAUSA_MODIFICACIÓN   : Estaba incompleto...
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            DataTable Dt_Clave;

            Claves_Ingreso.P_Documento_ID = "00007";
            Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
            if (Dt_Clave.Rows.Count > 0)
            {
                Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                Calculo_Impuesto_Traslado.P_Descripcion = "COBRO POR CONVENIO DE TRASLADO DE DOMINIO DE LA CUENTA PREDIAL " + Txt_Cuenta_Predial.Text;
                Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = "" + Convert.ToDouble(Grid_Parcialidades.Rows[0].Cells[6].Text.Replace("$", ""));
                Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");

                Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                Calculo_Impuesto_Traslado.Alta_Pasivo();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = "El Pasivo no pudo ser insertado: " + Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Convenios_Traslado
    ///DESCRIPCIÓN          : Crea un DataTable con el folio de pago
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 13/Octubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Convenios_Traslado(Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio)
    {
        Ds_Pre_Convenio_Traslado_Dominio Ds_Convenio = new Ds_Pre_Convenio_Traslado_Dominio();
        DataTable Dt_Convenio_Traslado = Ds_Convenio.Tables["Dt_Convenio_Traslado"];
        DataRow Dr_Convenio_Traslado;

        Dr_Convenio_Traslado = Dt_Convenio_Traslado.NewRow();
        Dr_Convenio_Traslado["FOLIO"] = "CTRA" + Convenio.P_No_Convenio;
        Dt_Convenio_Traslado.Rows.Add(Dr_Convenio_Traslado);

        return Ds_Convenio;
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
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenio);
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
        catch
        {
            //Lbl_Mensaje_Error.Visible = true;
            //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
        }

        try
        {
            Mostrar_Reporte(Archivo_PDF, "PDF");
            //Reporte.PrintToPrinter(1, true, 0, 0);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
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
            }
            Consultar_Datos_Cuenta_Predial();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Constanica
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Predial()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        DataTable Dt_Cuentas_Predial;
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", ";
            Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Propietario_ID;
            Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                //if (Validar_Cuenta_Sin_Convenios_Vigente(Hdf_Cuenta_Predial_ID.Value))
                //{
                //if (Validar_Estatus_Cuenta(Hdf_Cuenta_Predial_ID.Value, "VIGENTE"))
                //{
                //if (!Validar_Existe_Convenio_Activo("", Hdf_Cuenta_Predial_ID.Value))
                //{
                //Pone los datos de la cuenta y los Impuestos
                Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Impuestos_Traslado_Tominio = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                DataTable Dt_Impuestos_Derechos_Supervision;

                Impuestos_Traslado_Tominio.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                Impuestos_Traslado_Tominio.P_No_Calculo = Hdf_No_Calculo.Value;
                Impuestos_Traslado_Tominio.P_Anio_Calculo = Convert.ToInt32(Hdf_Anio_Calculo.Value);
                Dt_Impuestos_Derechos_Supervision = Impuestos_Traslado_Tominio.Consulta_Calculos_Contrarecibo();

                if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
                {
                    Txt_Estatus_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["ESTATUS_CALCULO"].ToString();
                    if (Dt_Impuestos_Derechos_Supervision != null)
                    {
                        if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
                        {
                            //Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo].ToString();
                            Txt_Fecha_Calculo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Impuestos_Derechos_Supervision.Rows[0]["FECHA_CALCULO"].ToString()));
                        }
                    }

                    Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_TOTAL_CALCULO"].ToString()).ToString("0.00");
                    Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_RECARGOS"].ToString()).ToString("0.00");
                    Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Impuestos_Derechos_Supervision.Rows[0]["MONTO_MULTA"].ToString()).ToString("0.00");
                    Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["REALIZO_CALCULO"].ToString();

                    if (Boton_Pulsado != "Btn_Buscar")
                    {
                        Calcular_Total_Adeudos();
                        Calcular_Total_Descuento();
                        Calcular_Sub_Total();
                        Calcular_Total_Convenio();
                        Calcular_Parcialidades();
                        Calcular_Total_Anticipo();
                    }

                    Txt_Propietario.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                    Txt_Calle.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_CALLE"].ToString();
                    Txt_Colonia.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_COLONIA"].ToString();
                    Txt_No_Exterior.Text = Dt_Cuentas_Predial.Rows[0]["NO_EXTERIOR"].ToString();
                    Txt_No_Interior.Text = Dt_Cuentas_Predial.Rows[0]["NO_INTERIOR"].ToString();
                    //DataTable Dt_Propietarios;
                    //Cls_Cat_Pre_Propietarios_Negocio Propietarios = new Cls_Cat_Pre_Propietarios_Negocio();
                    ////Consulta los Propietarios de la Cuenta Predial
                    //Propietarios.P_Campos_Dinamicos = Cat_Pre_Propietarios.Campo_Contribuyente_ID;
                    //Propietarios.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    //Propietarios.P_Propietario_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Propietario_ID].ToString();
                    ////Propietarios.P_Tipo = "PROPIETARIO";
                    //Dt_Propietarios = Propietarios.Consultar_Propietario();
                    //if (Dt_Propietarios.Rows.Count > 0)
                    //{
                    //    DataTable Dt_Contribuyentes;
                    //    Cls_Cat_Pre_Contribuyentes_Negocio Contribuyentes = new Cls_Cat_Pre_Contribuyentes_Negocio();
                    //    //Consulta los datos del Contribuyente
                    //    Contribuyentes.P_Contribuyente_ID = Dt_Propietarios.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
                    //    Dt_Contribuyentes = Contribuyentes.Consultar_Contribuyentes();
                    //    if (Dt_Contribuyentes.Rows.Count > 0)
                    //    {
                    //        if (Propietarios.P_Propietario_ID != "")
                    //        {
                    //            Hdf_Propietario_ID.Value = Propietarios.P_Propietario_ID;
                    //        }
                    //        else
                    //        {
                    //            Hdf_Propietario_ID.Value = Contribuyentes.P_Contribuyente_ID;
                    //        }
                    //        Hdf_Cuenta_Predial_ID.Value = Propietarios.P_Cuenta_Predial_ID;
                    //        Txt_Propietario.Text = Dt_Contribuyentes.Rows[0]["NOMBRE_COMPLETO"].ToString();
                    //        Txt_Domicilio.Text = Dt_Contribuyentes.Rows[0]["DOMICILIO"].ToString();
                    //    }
                    //}
                }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('El número de Convenio está Activo');", true);
                //    if (Btn_Salir.AlternateText == "Cancelar")
                //    {
                //        Btn_Salir_Click(null, null);
                //    }
                //}
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('La Cuenta seleccionada No tiene un estatus Vigente.');", true);
                //}
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Ya existen Convenios para la Cuenta Seleccioanda.');", true);
                //    Btn_Salir_Click(null, null);
                //}
            }
        }
    }

    private double Calcular_Total_Adeudos()
    {
        Double Monto_Impuesto = 0;
        Double Monto_Recargos = 0;
        Double Monto_Multas = 0;
        Double Monto_Constancia = 0;
        Double Total_Adeudo = 0;

        if (Txt_Monto_Impuesto.Text.Trim() != "")
        {
            Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
        }
        else
        {
            Txt_Monto_Impuesto.Text = "0.00";
        }
        if (Txt_Monto_Recargos.Text.Trim() != "")
        {
            Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text);
        }
        else
        {
            Txt_Monto_Recargos.Text = "0.00";
        }
        if (Txt_Monto_Multas.Text.Trim() != "")
        {
            Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text);
        }
        else
        {
            Txt_Monto_Multas.Text = "0.00";
        }
        if (Txt_Costo_Constancia.Text.Trim() != "")
        {
            Monto_Constancia = Convert.ToDouble(Txt_Costo_Constancia.Text);
        }
        else
        {
            Txt_Costo_Constancia.Text = "0.00";
        }
        Total_Adeudo = Monto_Impuesto + Monto_Recargos + Monto_Multas + Monto_Constancia;
        Txt_Total_Adeudo.Text = Total_Adeudo.ToString("0.00");

        return Total_Adeudo;
    }

    private Double Calcular_Sub_Total()
    {
        Double Total_Adeudo = 0.0;
        Double Total_Descuento = 0.0;
        Double Sub_Total = 0.0;

        if (Txt_Total_Adeudo.Text.Trim() != "")
        {
            Total_Adeudo = Convert.ToDouble(Txt_Total_Adeudo.Text);
        }
        else
        {
            Txt_Total_Adeudo.Text = "0.00";
        }
        if (Txt_Total_Descuento.Text.Trim() != "")
        {
            Total_Descuento = Convert.ToDouble(Txt_Total_Descuento.Text);
        }
        else
        {
            Txt_Total_Descuento.Text = "0.00";
        }
        Sub_Total = Total_Adeudo - Total_Descuento;
        Txt_Sub_Total.Text = Sub_Total.ToString("0.00");

        return Sub_Total;
    }

    private Double Calcular_Total_Convenio()
    {
        Double Sub_Total = 0.0;
        Double Total_Anticipo = 0.0;
        Double Total_Convenio = 0.0;

        if (Txt_Sub_Total.Text.Trim() != "")
        {
            Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
        }
        else
        {
            Txt_Sub_Total.Text = "0.00";
        }
        if (Txt_Total_Anticipo.Text.Trim() != "")
        {
            Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
        }
        else
        {
            Txt_Total_Anticipo.Text = "0.00";
        }
        Total_Convenio = Sub_Total - Total_Anticipo;
        Txt_Total_Convenio.Text = Total_Convenio.ToString("##,###,##0.00");

        return Total_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Numero_Parcialidades_TextChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Numero_Parcialidades_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Periodicidad_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Recargos_Ordinarios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Recargos_Moratorios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Multas_TextChanged(object sender, EventArgs e)
    {
        //Calcular_Total_Descuento();
        //Calcular_Sub_Total();
        //Calcular_Total_Convenio();
        //Calcular_Parcialidades();
        //Calcular_Total_Anticipo();
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    private void Calcular_Total_Descuento()
    {
        Double Descuento_Recargos_Ordinarios = 0.0;
        Double Descuento_Recargos_Moratorios = 0.0;
        Double Descuento_Recargos_Multa = 0.0;
        Double Total_Descuento = 0.0;

        if (Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Descuento_Recargos_Ordinarios = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", ""));
        }
        else
        {
            Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
        }
        if (Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Descuento_Recargos_Moratorios = Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("_", "").Replace(",", ""));
        }
        else
        {
            Txt_Descuento_Recargos_Moratorios.Text = "0.00";
        }
        if (Txt_Descuento_Multas.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Descuento_Multas.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Descuento_Recargos_Multa = Convert.ToDouble(Txt_Descuento_Multas.Text.Trim().Replace("_", "").Replace(",", ""));
        }
        else
        {
            Txt_Descuento_Multas.Text = "0.00";
        }

        Descuento_Recargos_Moratorios = 0;
        Descuento_Recargos_Multa = (Descuento_Recargos_Multa / 100) * Convert.ToDouble(Txt_Monto_Multas.Text);
        Descuento_Recargos_Ordinarios = (Descuento_Recargos_Ordinarios / 100) * Convert.ToDouble(Txt_Monto_Recargos.Text);
        Total_Descuento = Descuento_Recargos_Ordinarios + Descuento_Recargos_Moratorios + Descuento_Recargos_Multa;
        Txt_Total_Descuento.Text = Total_Descuento.ToString("0.00");
    }

    //[System.Web.Services.WebMethod]
    //public static void WM_Calcular_Parcialidades()
    //{
    //    paginas_Predial_Frm_Ope_Pre_Convenios_Derechos_Supervision Convenios_Derechos_Supervision = new paginas_Predial_Frm_Ope_Pre_Convenios_Derechos_Supervision();
    //    Convenios_Derechos_Supervision.Calcular_Parcialidades();
    //}

    private void Calcular_Parcialidades()
    {
        if (Txt_Numero_Parcialidades.Text != "" && Convert.ToInt32(Txt_Numero_Parcialidades.Text) > 0 && Cmb_Periodicidad_Pago.SelectedIndex > 0)
        {
            //Parcialidades
            Int32 Parcialidades;
            Parcialidades = 0;
            Int32 Cont_Parcialidades;
            Cont_Parcialidades = 0;
            //DataTable
            DataTable Dt_Parcialidades = new DataTable();
            DataRow Dr_Parcialidades;
            //Cantidades
            Double Monto_Impuesto = 0;
            Double Monto_Recargos = 0;
            Double Monto_Multas = 0;
            Double Monto_Honorarios = 0;
            Double Total_Convenio = 0;
            Double Monto_Importe = 0;
            Double Monto_Constancia = 0;
            //Monto de cada parcialidad de la 2 en adelante
            Double Monto_Parcialidades = 0;
            //Periodicidad
            String Dias_Periodo = "";
            Double Total_Anticipo = 0;
            Double Total_Importe_Parcialidad = 0;
            Double Monto_Final = 0;
            Double Total_Importe = 0;
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

            if (Txt_Monto_Multas.Text != "")
            {
                Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text) - ((Convert.ToDouble(Txt_Descuento_Multas.Text) / 100) * Convert.ToDouble(Txt_Monto_Multas.Text));
            }
            if (Txt_Monto_Recargos.Text != "")
            {
                Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text) - ((Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text) / 100) * Convert.ToDouble(Txt_Monto_Recargos.Text));
            }
            if (Txt_Monto_Impuesto.Text != "")
            {
                Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
            }
            if (Txt_Costo_Constancia.Text != "")
            {
                Monto_Constancia = Convert.ToDouble(Txt_Costo_Constancia.Text);
            }
            if (Txt_Numero_Parcialidades.Text != "")
            {
                Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
            }
            if (Txt_Total_Anticipo.Text != "")
            {
                Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
            }
            if (Txt_Total_Convenio.Text != "")
            {
                Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
            }
            Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
            Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
            Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("CONSTANCIA", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_MULTAS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Double)));
            Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
            Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));

            Dr_Parcialidades = Dt_Parcialidades.NewRow();
            Dr_Parcialidades["NO_PAGO"] = 1;
            if (Monto_Honorarios <= Total_Anticipo)
            {
                Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
                Total_Anticipo = Total_Anticipo - Monto_Honorarios;
                Monto_Importe += Monto_Honorarios;
                Monto_Honorarios = 0;
            }
            else
            {
                Dr_Parcialidades["HONORARIOS"] = Total_Anticipo;
                Monto_Honorarios = Monto_Honorarios - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Constancia <= Total_Anticipo)
            {
                Dr_Parcialidades["CONSTANCIA"] = Monto_Constancia;
                Total_Anticipo = Total_Anticipo - Monto_Constancia;
                Monto_Importe += Monto_Constancia;
                Monto_Constancia = 0;
            }
            else
            {
                Dr_Parcialidades["CONSTANCIA"] = Total_Anticipo;
                Monto_Constancia = Monto_Constancia - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Multas <= Total_Anticipo)
            {
                Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
                Total_Anticipo = Total_Anticipo - Monto_Multas;
                Monto_Importe += Monto_Multas;
                Monto_Multas = 0;
            }
            else
            {
                Dr_Parcialidades["MONTO_MULTAS"] = Total_Anticipo;
                Monto_Multas = Monto_Multas - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            if (Monto_Recargos <= Total_Anticipo)
            {
                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                Total_Anticipo = Total_Anticipo - Monto_Recargos;
                Monto_Importe += Monto_Recargos;
                Monto_Recargos = 0;
            }
            else
            {
                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Total_Anticipo;
                Monto_Recargos = Monto_Recargos - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }
            Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
            if (Monto_Impuesto <= Total_Anticipo)
            {
                Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
                Total_Anticipo = Total_Anticipo - Monto_Impuesto;
                Monto_Importe += Monto_Impuesto;
                Monto_Impuesto = 0;
            }
            else
            {
                Dr_Parcialidades["MONTO_IMPUESTO"] = Total_Anticipo;
                Monto_Impuesto = Monto_Impuesto - Total_Anticipo;
                Monto_Importe += Total_Anticipo;
                Total_Anticipo = 0;
            }

            Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
            Dr_Parcialidades["ESTATUS"] = "PENDIENTE";
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);

            Total_Importe_Parcialidad = Convert.ToDouble((Total_Convenio / (Parcialidades - 1)).ToString("0.00"));

            for (Cont_Parcialidades = 1; Cont_Parcialidades < Parcialidades; Cont_Parcialidades++)
            {
                Monto_Parcialidades = Total_Importe_Parcialidad;
                Monto_Importe = 0;
                Dr_Parcialidades = Dt_Parcialidades.NewRow();
                Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;
                if (Cont_Parcialidades == (Parcialidades - 1))
                {
                    Monto_Final = Total_Convenio - Total_Importe;
                    if (Monto_Honorarios <= Monto_Final)
                    {
                        Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
                        Monto_Final = Monto_Final - Monto_Honorarios;
                        Monto_Importe += Monto_Honorarios;
                        Monto_Honorarios = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["HONORARIOS"] = Monto_Final;
                        Monto_Honorarios = Monto_Honorarios - Monto_Final;
                        Monto_Importe += Monto_Final;
                        Monto_Final = 0;
                    }
                    if (Monto_Constancia <= Monto_Final)
                    {
                        Dr_Parcialidades["CONSTANCIA"] = Monto_Constancia;
                        Monto_Final = Monto_Final - Monto_Constancia;
                        Monto_Importe += Monto_Constancia;
                        Monto_Constancia = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["CONSTANCIA"] = Monto_Final;
                        Monto_Constancia = Monto_Constancia - Monto_Final;
                        Monto_Importe += Monto_Final;
                        Monto_Final = 0;
                    }
                    if (Monto_Multas <= Monto_Final)
                    {
                        Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
                        Monto_Final = Monto_Final - Monto_Multas;
                        Monto_Importe += Monto_Multas;
                        Monto_Multas = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["MONTO_MULTAS"] = Monto_Final;
                        Monto_Multas = Monto_Multas - Monto_Final;
                        Monto_Importe += Monto_Final;
                        Monto_Final = 0;
                    }
                    if (Monto_Recargos <= Monto_Final)
                    {
                        Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                        Monto_Final = Monto_Final - Monto_Recargos;
                        Monto_Importe += Monto_Recargos;
                        Monto_Recargos = 0;
                    }
                    else
                    {
                        Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Final;
                        Monto_Recargos = Monto_Recargos - Monto_Final;
                        Monto_Importe += Monto_Final;
                        Monto_Final = 0;
                    }
                    Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
                    Total_Importe += Monto_Importe;
                    //if (Total_Importe > Total_Convenio)
                    //{
                    //    Monto_Final = Monto_Final - (Total_Importe - Total_Convenio);
                    //}
                    //else if (Total_Convenio > Total_Importe)
                    //{
                    //    Monto_Final = Monto_Final - (Total_Convenio - Total_Importe);
                    //}
                    Monto_Importe += Monto_Final;
                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Final;
                    Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                    Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
                    Dr_Parcialidades["ESTATUS"] = "PENDIENTE";
                    Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                    Grid_Parcialidades.DataSource = Dt_Parcialidades;
                    Grid_Parcialidades.PageIndex = 0;
                    Grid_Parcialidades.DataBind();

                    Sumar_Totales_Parcialidades();
                    return;
                }
                if (Monto_Honorarios <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Honorarios;
                    Monto_Importe += Monto_Honorarios;
                    Monto_Honorarios = 0;
                    Monto_Final += Monto_Honorarios;
                }
                else
                {
                    Dr_Parcialidades["HONORARIOS"] = Monto_Parcialidades;
                    Monto_Honorarios = Monto_Honorarios - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Final += Monto_Parcialidades;
                }
                if (Monto_Constancia <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["CONSTANCIA"] = Monto_Constancia;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Constancia;
                    Monto_Importe += Monto_Constancia;
                    Monto_Constancia = 0;
                    Monto_Final += Monto_Constancia;
                }
                else
                {
                    Dr_Parcialidades["CONSTANCIA"] = Monto_Parcialidades;
                    Monto_Constancia = Monto_Constancia - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Final += Monto_Parcialidades;
                }
                if (Monto_Multas <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Multas;
                    Monto_Importe += Monto_Multas;
                    Monto_Multas = 0;
                    Monto_Final += Monto_Multas;
                }
                else
                {
                    Dr_Parcialidades["MONTO_MULTAS"] = Monto_Parcialidades;
                    Monto_Multas = Monto_Multas - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Final += Monto_Parcialidades;
                }
                if (Monto_Recargos <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Recargos;
                    Monto_Importe += Monto_Recargos;
                    Monto_Recargos = 0;
                    Monto_Final += Monto_Recargos;
                }
                else
                {
                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Parcialidades;
                    Monto_Recargos = Monto_Recargos - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Final += Monto_Parcialidades;
                }
                Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
                if (Monto_Impuesto <= Monto_Parcialidades)
                {
                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
                    Monto_Parcialidades = Monto_Parcialidades - Monto_Impuesto;
                    Monto_Importe += Monto_Impuesto;
                    Monto_Impuesto = 0;
                    Monto_Final += Monto_Impuesto;
                }
                else
                {
                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Parcialidades;
                    Monto_Impuesto = Monto_Impuesto - Monto_Parcialidades;
                    Monto_Importe += Monto_Parcialidades;
                    Monto_Parcialidades = 0;
                    Monto_Final += Monto_Parcialidades;
                }

                Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                Total_Importe += Monto_Importe;
                Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
                Dr_Parcialidades["ESTATUS"] = "PENDIENTE";
                Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
            }
            Grid_Parcialidades.DataSource = Dt_Parcialidades;
            Grid_Parcialidades.PageIndex = 0;
            Grid_Parcialidades.DataBind();

            Sumar_Totales_Parcialidades();
        }
        else
        {
            Grid_Parcialidades.DataSource = null;
            Grid_Parcialidades.DataBind();
        }
    }

    private void Sumar_Totales_Parcialidades()
    {
        Double Total_Honorarios = 0;
        Double Total_Multas = 0;
        Double Total_Constancia = 0;
        Double Total_Recargos_Ordinarios = 0;
        Double Total_Recargos_Moratorios = 0;
        Double Total_Impuesto = 0;
        Double Total_Importe = 0;

        foreach (GridViewRow Fila_Grid in Grid_Parcialidades.Rows)
        {
            if (Fila_Grid.Cells[1].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Honorarios += Convert.ToDouble(Fila_Grid.Cells[1].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[2].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Constancia += Convert.ToDouble(Fila_Grid.Cells[2].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[3].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Multas += Convert.ToDouble(Fila_Grid.Cells[3].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[4].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Ordinarios += Convert.ToDouble(Fila_Grid.Cells[4].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Moratorios += Convert.ToDouble(Fila_Grid.Cells[5].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[6].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto += Convert.ToDouble(Fila_Grid.Cells[6].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[7].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Importe += Convert.ToDouble(Fila_Grid.Cells[7].Text.Replace("$", ""));
            }
        }

        if (Grid_Parcialidades.FooterRow != null)
        {
            Grid_Parcialidades.FooterRow.Cells[1].Text = Total_Honorarios.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[2].Text = Total_Constancia.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[3].Text = Total_Multas.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[4].Text = Total_Recargos_Ordinarios.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[5].Text = Total_Recargos_Moratorios.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[6].Text = Total_Impuesto.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[7].Text = Total_Importe.ToString("$###,###,##0.00");
        }
    }
    private DateTime Obtener_Fecha_Periodo(DateTime Primer_Fecha_Periodo, String Periodo, Int32 Parcialidades)
    {
        int Dias_Periodo;
        DateTime Fecha_Periodo = Primer_Fecha_Periodo;
        if (int.TryParse(Periodo, out Dias_Periodo))
        {
            Fecha_Periodo = Primer_Fecha_Periodo.AddDays(Dias_Periodo * Parcialidades);
        }
        else
        {
            switch (Periodo)
            {
                case "Mensual":
                    Fecha_Periodo = Primer_Fecha_Periodo.AddMonths(Parcialidades);
                    break;
                case "Anual":
                    Fecha_Periodo = Primer_Fecha_Periodo.AddYears(Parcialidades);
                    break;
            }
        }
        Fecha_Periodo = Fecha_Periodo.AddDays(-1);
        return Fecha_Periodo;
    }

    protected void Txt_Porcentaje_Anticipo_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    protected void Txt_Total_Anticipo_TextChanged(object sender, EventArgs e)
    {
        Calcular_Porcentaje_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    private Double Calcular_Porcentaje_Anticipo()
    {
        Double Sub_Total = 0.0;
        Double Total_Anticipo = 0.0;
        Double Porcentaje_Anticipo = 0.0;

        if (Txt_Sub_Total.Text.Trim() != "")
        {
            Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
        }
        else
        {
            Txt_Sub_Total.Text = "0.00";
        }
        if (Txt_Total_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Total_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text.Trim().Replace("_", "").Replace(",", ""));
        }
        else
        {
            Txt_Total_Anticipo.Text = "0.00";
        }
        Porcentaje_Anticipo = (Total_Anticipo * 100) / Sub_Total;
        Txt_Porcentaje_Anticipo.Text = Porcentaje_Anticipo.ToString("###,###,##0.00");
        return Porcentaje_Anticipo;
    }

    private Double Calcular_Total_Anticipo()
    {
        Double Sub_Total = 0.0;
        Double Porcentaje_Anticipo = 0.0;
        Double Total_Anticipo = 0.0;

        if (Txt_Sub_Total.Text.Trim() != "")
        {
            Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
        }
        else
        {
            Txt_Sub_Total.Text = "0.00";
        }
        if (Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Porcentaje_Anticipo = Convert.ToDouble(Txt_Porcentaje_Anticipo.Text.Trim().Replace("_", "").Replace(",", ""));
        }
        else
        {
            Txt_Porcentaje_Anticipo.Text = "0.00";
        }
        Total_Anticipo = Sub_Total * Porcentaje_Anticipo / 100;
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("##,###,##0.00");
        return Total_Anticipo;
    }

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
                if (Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString() != "")
                {
                    Txt_Propietario.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                }
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
        }
    }

    protected void Calcular_Montos(DataTable Dt_Calculo)
    {
        if (Dt_Calculo.Rows.Count > 0)
        {
            Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_RECARGOS"].ToString()).ToString("0.00");
            Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_MULTA"].ToString()).ToString("0.00");
            Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_TRASLADO"].ToString()).ToString("0.00");
            if (!Dt_Calculo.Rows[0]["COSTO_CONSTANCIA"].ToString().Equals(""))
            {
                Txt_Costo_Constancia.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["COSTO_CONSTANCIA"].ToString()).ToString("0.00");
            }
            else
            {
                Txt_Costo_Constancia.Text = "0.00";
            }
            Txt_Realizo_Calculo.Text = Dt_Calculo.Rows[0]["REALIZO_CALCULO"].ToString();
            Txt_Fecha_Calculo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Calculo.Rows[0]["FECHA_CREO"].ToString()));
            Hdf_No_Calculo.Value = Dt_Calculo.Rows[0]["NO_CALCULO"].ToString();
        }
    }

    #endregion

    #region Impresion Folios

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    ///DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Imprimir_Reporte(DataTable Dt_Constancias, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        Reporte.Load(File_Path);
        Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Dt_Constancias);
        Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Dt_Constancias);

        String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
        ExportOptions Export_Options = new ExportOptions();
        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

        Reporte.Export(Export_Options);
        Reporte.PrintToPrinter(1, true, 0, 0);
    }

    /////******************************************************************************* 
    /////NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    /////DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    /////PARAMETROS: 
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Julio/2011
    /////MODIFICO: 
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////*********************************************************d**********************
    //private void Imprimir_Reporte(DataSet Ds_Constancias, String Nombre_Reporte, String Nombre_Archivo)
    //{
    //    ReportDocument Reporte = new ReportDocument();
    //    String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
    //    try
    //    {
    //        Reporte.Load(File_Path);
    //        Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_Propiedad"]);
    //        Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Propiedad"]);
    //        Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Ds_Constancias.Tables["Dt_Constancias_No_Adeudo"]);
    //        Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Ds_Constancias.Tables["Dt_Certificaciones"]);
    //    }
    //    catch
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
    //    }

    //    String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
    //    try
    //    {
    //        ExportOptions Export_Options = new ExportOptions();
    //        DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
    //        Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
    //        Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
    //        Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
    //        Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;
    //        Reporte.Export(Export_Options);
    //    }
    //    catch
    //    {
    //        //Lbl_Mensaje_Error.Visible = true;
    //        //Lbl_Mensaje_Error.Text = "No se pudo exportar el reporte";
    //    }

    //    try
    //    {
    //        Reporte.PrintToPrinter(1, true, 0, 0);
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Mensaje_Error.Visible = true;
    //        Lbl_Mensaje_Error.Text = Ex.Message.ToString();
    //    }
    //}

    #endregion

    private bool Validar_Cuenta_Sin_Convenios_Vigente(String Cuenta_Predial_ID)
    {
        Boolean Sin_Convenios = true;
        if (Cuenta_Predial_ID != "")
        {
            try
            {
                Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                DataTable Dt_Convenios_Traslado_Dominio;
                Convenios_Traslado_Dominio.P_Campos_Dinamicos = Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + ", ";
                Convenios_Traslado_Dominio.P_Campos_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus;
                Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Convenios_Traslado_Dominio.P_Estatus = "VIGENTE";
                Dt_Convenios_Traslado_Dominio = Convenios_Traslado_Dominio.Consultar_Convenio_Traslado_Dominio();
                if (Dt_Convenios_Traslado_Dominio != null)
                {
                    if (Dt_Convenios_Traslado_Dominio.Rows.Count == 0)
                    {
                        Sin_Convenios = true;
                    }
                    else
                    {
                        Sin_Convenios = false;
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        else
        {
            throw new Exception("Seleccione una Cuenta para Validar sus Convenios");
        }
        return Sin_Convenios;
    }

    private bool Validar_Estatus_Cuenta(String Cuenta_Predial_ID, String Estatus)
    {
        Boolean Estatus_Valido = true;
        if (Cuenta_Predial_ID != "")
        {
            try
            {
                Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
                DataTable Dt_Cuentas_Predial;
                Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
                Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Estatus;
                Cuentas_Predial.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                Cuentas_Predial.P_Estatus = Estatus;
                Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
                if (Dt_Cuentas_Predial != null)
                {
                    if (Dt_Cuentas_Predial.Rows.Count == 1)
                    {
                        Estatus_Valido = true;
                    }
                    else
                    {
                        Estatus_Valido = false;
                    }
                }
                else
                {
                    Estatus_Valido = false;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        else
        {
            throw new Exception("Seleccione una Cuenta para Validar su Estatus");
        }
        return Estatus_Valido;
    }
}
