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
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio;
using CrystalDecisions.Shared;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Catalogo_Tipos_Constancias.Negocio;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using Presidencia.Numalet;
using System.IO;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

public partial class paginas_Predial_Frm_Ope_Pre_Convenios_Traslado : System.Web.UI.Page
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
            if (Page.PreviousPage != null)
            {
                Btn_Nuevo_Click(Btn_Nuevo, null);
                Hdf_Cuenta_Predial_ID.Value = PreviousPage.Cuenta_Predial_ID;
                if (Validar_Cuenta_Sin_Convenios_Vigente(Hdf_Cuenta_Predial_ID.Value))
                {
                    if (Validar_Estatus_Cuenta(Hdf_Cuenta_Predial_ID.Value, "IN ('VIGENTE', 'BLOQUEADA', 'PENDIENTE')"))
                    {
                        Txt_Cuenta_Predial.Text = PreviousPage.Cuenta_Predial;
                        Hdf_No_Contrarecibo.Value = String.Format("{0:0000000000}", Convert.ToInt32(PreviousPage.No_Contrarecibo));
                        Hdf_Cuenta_Predial_ID.Value = PreviousPage.Cuenta_Predial_ID;
                        Consultar_Datos_Cuenta_Predial();
                        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                        Calculo.P_Anio_Calculo = Convert.ToInt32(PreviousPage.Anio_Calculo);
                        Hdf_Anio.Value = Calculo.P_Anio_Calculo.ToString();
                        Calculo.P_No_Calculo = string.Format("{0:0000000000}", Convert.ToInt32(PreviousPage.No_Calculo));
                        Hdf_No_Calculo.Value = Calculo.P_No_Calculo;
                        Txt_Cuenta_Predial_TextChanged();
                        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                        Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                        if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
                        {
                            Cargar_Datos();
                        }
                        //Calcular_Montos(Calculo.Consulta_Detalles_Calculo());
                        DataTable Dt_Calculo = Calcular_Impuestos();
                        if (Dt_Calculo.Rows[0]["MONTO_TRASLADO"].ToString() != "")
                        {
                            Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_TRASLADO"].ToString()).ToString("#,###,###,###,##0.00");
                        }
                        else
                        {
                            Txt_Monto_Impuesto.Text = "0.00";
                        }
                        if (Dt_Calculo.Rows[0]["MONTO_DIVISION"].ToString() != "")
                        {
                            Txt_Imp_Div.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_DIVISION"].ToString()).ToString("#,###,###,###,##0.00");
                        }
                        else
                        {
                            Txt_Imp_Div.Text = "0.00";
                        }
                        if (Dt_Calculo.Rows[0]["COSTO_CONSTANCIA"].ToString() != "")
                        {
                            Txt_Costo_Constancia.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["COSTO_CONSTANCIA"].ToString()).ToString("#,###,###,###,##0.00");
                        }
                        else
                        {
                            Txt_Costo_Constancia.Text = "0.00";
                        }
                        if (Dt_Calculo.Rows[0]["MONTO_MULTAS"].ToString() != "")
                        {
                            Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_MULTAS"].ToString()).ToString("#,###,###,###,##0.00");
                        }
                        else
                        {
                            Txt_Monto_Multas.Text = "0.00";
                        }
                        if (Dt_Calculo.Rows[0]["MONTO_RECARGOS"].ToString() != "")
                        {
                            Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_RECARGOS"].ToString()).ToString("#,###,###,###,##0.00");
                        }
                        else
                        {
                            Txt_Monto_Recargos.Text = "0.00";
                        }
                        Txt_Estatus_Calculo.Text = Dt_Calculo.Rows[0]["ESTATUS"].ToString();
                        Txt_Fecha_Calculo.Text = Convert.ToDateTime(Dt_Calculo.Rows[0]["FECHA_CALCULO"].ToString()).ToString("dd/MMM/yyyy");
                        Txt_Realizo_Calculo.Text = Dt_Calculo.Rows[0]["REALIZO_CALCULO"].ToString();

                        Txt_Solicitante.Text = Txt_Propietario.Text;
                        Grid_Contrarecibos_Pendientes.Visible = false;
                        Grid_Convenios_Impuestos_Traslado.Visible = false;
                        Panel_Datos.Visible = true;
                        Btn_Salir.AlternateText = "Atras";
                        //Session["CUENTA_PREDIAL_ID"] = Hdf_Cuenta_Predial_ID.Value;
                        //Session["CUENTA_PREDIAL"] = PreviousPage.Cuenta_Predial;
                        //Session["AÑO"] = Convert.ToDateTime(Txt_Fecha_Calculo.Text.Trim()).Year.ToString();
                        //Session["NO_CALCULO"] = Hdf_No_Calculo.Value;



                        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculos_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
                        DataTable Dt_Calculos_Impuesto_Traslado;
                        Calculos_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                        Calculos_Impuesto_Traslado.P_No_Calculo = Calculo.P_No_Calculo;
                        Dt_Calculos_Impuesto_Traslado = Calculos_Impuesto_Traslado.Consulta_Calculos_Contrarecibo();
                        if (Dt_Calculos_Impuesto_Traslado.Rows.Count > 0)
                        {
                            if (!Dt_Calculos_Impuesto_Traslado.Rows[0]["DESC_MULTA"].ToString().Equals(""))
                                Txt_Monto_Multas.Text = (Convert.ToDouble(Txt_Monto_Multas.Text) - Convert.ToDouble(Dt_Calculos_Impuesto_Traslado.Rows[0]["DESC_MULTA"].ToString())).ToString("#,##0.00");
                            if (!Dt_Calculos_Impuesto_Traslado.Rows[0]["DESC_RECARGO"].ToString().Equals(""))
                                Txt_Monto_Recargos.Text = (Convert.ToDouble(Txt_Monto_Recargos.Text) - Convert.ToDouble(Dt_Calculos_Impuesto_Traslado.Rows[0]["DESC_RECARGO"].ToString())).ToString("#,##0.00");
                        }

                        Cargar_Descuentos();


                        Calcular_Total_Descuento();
                        Calcular_Total_Adeudos();
                        Calcular_Sub_Total();
                        Calcular_Total_Anticipo();
                        Calcular_Total_Convenio();

                        Grid_Contrarecibos_Pendientes.SelectedIndex = 0;
                        Cargar_Ventana_Emergente_Resolucion();
                        Cargar_Ventana_Emergente_Resumen_Predio();
                        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                        Convenio.P_No_Contrarecibo = Hdf_No_Contrarecibo.Value;
                        Convenio.P_Anio = Hdf_Anio.Value;
                        if (Convenio.Consultar_Convenio_Existente())
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Ya existe un convenio para este cálculo.');", true);
                            Btn_Salir_Click(null, null);
                        }
                        if (Txt_Estatus_Calculo.Text != "POR PAGAR")
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('El cálculo no tiene un estatus POR PAGAR.');", true);
                            Btn_Salir_Click(null, null);
                        }
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('La Cuenta seleccionada No tiene un estatus Vigente o Bloqueado.');", true);
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Ya existen Convenios para la Cuenta Seleccioanda.');", true);
                    Btn_Salir_Click(null, null);
                }
            }
            else
            {
                Cargar_Grid_Contrarecibos_Pendientes(0);
                Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
                Grid_Contrarecibos_Pendientes.Visible = true;
                Grid_Convenios_Impuestos_Traslado.Visible = true;
                Panel_Datos.Visible = false;
                Session["Cuenta_Predial"] = null;
            }

            Session.Remove("ESTATUS_CUENTAS");
            Session.Remove("TIPO_CONTRIBUYENTE");

            //Scrip para mostrar Ventana Modal de las Tasas de Traslado
            Session["ESTATUS_CUENTAS"] = "IN ('LISTO', 'BLOQUEADA','PENDIENTE')";
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal);
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
        String Propiedades = ",'height=600,width=800,scrollbars=1');";
        Btn_Detalles_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
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
        Txt_Imp_Div.Enabled = false;
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
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Estatus;
            Btn_Detalles_Cuenta_Predial.Enabled = !Estatus;
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
                Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = !Estatus;
                Btn_Detalles_Cuenta_Predial.Enabled = !Estatus;
                //Convenio
                Cmb_Estatus.Enabled = !Estatus;
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
        }

        Txt_Propietario.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_Calle.Enabled = false;
        Txt_No_Exterior.Enabled = false;
        Txt_No_Interior.Enabled = false;
        Txt_Imp_Div.Enabled = false;

        Txt_Monto_Impuesto.Style["text-align"] = "right";
        Txt_Imp_Div.Style["text-align"] = "right";
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

        if (Btn_Modificar.AlternateText == "Actualizar")
        {
            Cmb_Estatus.Enabled = true;
        }
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
        if (!No_Limpiar_Campos_Clave)
        {
            Grid_Convenios_Impuestos_Traslado.DataSource = null;
            Grid_Convenios_Impuestos_Traslado.DataBind();
        }
        Grid_Parcialidades.DataSource = null;
        Grid_Parcialidades.DataBind();
        Txt_Propietario.Text = "";
        Txt_Colonia.Text = "";
        Txt_Calle.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Imp_Div.Text = "";

        Hdf_No_Descuento.Value = "";
        Hdf_Desc_Multas.Value = "0";
        Hdf_Desc_Recargos.Value = "0";
        Hdf_Anio.Value = "";
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

            Calculos_Impuesto_Traslado.P_Estatus = "POR PAGAR";
            if (Txt_Busqueda.Text.Trim() != "")
            {
                switch (Cmb_Busqueda_General.SelectedIndex)
                {
                    case 0://POR CONTRARECIBO
                        Calculos_Impuesto_Traslado.P_No_Contrarecibo = Convert.ToInt64(Txt_Busqueda.Text.Trim()).ToString("0000000000");
                        break;
                    case 1://POR CUENTA
                        Calculos_Impuesto_Traslado.P_Cuenta_Predial_ID = " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%')";
                        break;
                    case 2://POR CONVENIO
                        Calculos_Impuesto_Traslado.P_Cuenta_Predial_ID = " IN (SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Convert.ToInt64(Txt_Busqueda.Text.Trim()).ToString("0000000000") + "')";
                        Calculos_Impuesto_Traslado.P_No_Contrarecibo = " IN (SELECT " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " WHERE " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " IN (SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Convert.ToInt64(Txt_Busqueda.Text.Trim()).ToString("0000000000") + "'))";
                        break;
                }
            }
            Calculos_Impuesto_Traslado.P_Filtro_Dinamico = "CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " NOT IN (SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Calculo + " IS NOT NULL AND " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + "." + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " IN ('ACTIVO','PENDIENTE'))";
            Calculos_Impuesto_Traslado.P_Filtro_Dinamico += " AND CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " IN ('VIGENTE','BLOQUEADA','PENDIENTE')";

            //Campos dínamicos...
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos = " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " TO_NUMBER(CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + ") AS CONTRARECIBO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " NVL(CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", 'SIN REGISTRO') AS " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Fecha_Creo + " AS FECHA_CALCULO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " AS FECHA_ORDEN,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Realizo_Calculo + " AS REALIZO_CALCULO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += "  (SELECT " + Cat_Pre_Movimientos.Campo_Descripcion + " FROM "
                + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE "
                + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = ORDEN."
                + Cat_Pre_Movimientos.Campo_Movimiento_ID + ") MOVIMIENTO, ";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus + " AS ESTATUS_CALCULO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " NVL(REPLACE(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Estatus
                + ", 'RECHAZADO', 'RECHAZO CALCULO'), REPLACE(CONTRARECIBO."
                + Ope_Pre_Contrarecibos.Campo_Estatus + ", 'RECHAZADO', 'TRASLADO')) AS ESTATUS_CONTRARECIBO_CALCULO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " AS ESTATUS_ORDEN,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " AS ESTATUS_DESCUENTO,";
            //Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ADEUDO." + Ope_Pre_Descuento_Traslado.Campo_Estatus + " AS ESTATUS_ADEUDO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Recargos + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " NVL(TO_NUMBER(DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Desc_Recargo + "),0) AS DESC_RECARGO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Multa + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " NVL(TO_NUMBER(DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Desc_Multa + "),0) AS DESC_MULTA,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Monto_Total_Pagar + " AS MONTO_TOTAL_CALCULO,";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Costo_Constancia + " AS COSTO_CONSTANCIA,";
            //Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " DESCUENTO." + Ope_Pre_Adeudos_Folio.Campo_Monto + ",";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " TO_NUMBER(ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion;
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += ") || '/' || ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " AS FOLIO_ORDEN, ";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " TO_NUMBER(CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo;
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += ") || '/' || CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " AS FOLIO_CALCULO";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " FROM ";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " CONTRARECIBO";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " LEFT JOIN " + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + " ORDEN";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ON CONTRARECIBO." + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo;
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " LEFT JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ON CONTRARECIBO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Cuenta_Predial_Id + " = CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " LEFT JOIN " + Ope_Pre_Calculo_Imp_Traslado.Tabla_Ope_Pre_Calculo_Imp_Traslado + " CALCULO";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ON ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " = CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Orden_Variacion;
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " AND ORDEN." + Ope_Pre_Ordenes_Variacion.Campo_Anio + " = CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Orden;
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " LEFT JOIN " + Ope_Pre_Descuento_Traslado.Tabla_Ope_Pre_Descuento_Traslado + " DESCUENTO";
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ON CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_No_Calculo + " = DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_No_Calculo;
            Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " AND CALCULO." + Ope_Pre_Calculo_Imp_Traslado.Campo_Anio_Calculo + " = DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_Anio_Calculo;
            //Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " LEFT JOIN " + Ope_Pre_Adeudos_Folio.Tabla_Ope_Pre_Adeudos_Folio + " ADEUDO";
            //Calculos_Impuesto_Traslado.P_Campos_Dinamicos += " ON DESCUENTO." + Ope_Pre_Descuento_Traslado.Campo_No_Descuento + " = ADEUDO." + Ope_Pre_Adeudos_Folio.Campo_No_Descuento;








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
                Grid_Contrarecibos_Pendientes.Columns[2].Visible = true;
                Grid_Contrarecibos_Pendientes.Columns[5].Visible = true;
                Grid_Contrarecibos_Pendientes.Columns[6].Visible = true;
                Grid_Contrarecibos_Pendientes.Columns[7].Visible = true;
                Grid_Contrarecibos_Pendientes.Columns[8].Visible = true;
                Grid_Contrarecibos_Pendientes.Columns[11].Visible = true;
                Grid_Contrarecibos_Pendientes.Columns[12].Visible = true;
                Grid_Contrarecibos_Pendientes.Columns[13].Visible = true;
                Grid_Contrarecibos_Pendientes.DataSource = Dt_Calculos_Impuesto_Traslado;
                Grid_Contrarecibos_Pendientes.PageIndex = Pagina;
                Grid_Contrarecibos_Pendientes.DataBind();
                Grid_Contrarecibos_Pendientes.Columns[2].Visible = false;
                Grid_Contrarecibos_Pendientes.Columns[5].Visible = false;
                Grid_Contrarecibos_Pendientes.Columns[6].Visible = false;
                Grid_Contrarecibos_Pendientes.Columns[7].Visible = false;
                Grid_Contrarecibos_Pendientes.Columns[8].Visible = false;
                Grid_Contrarecibos_Pendientes.Columns[11].Visible = false;
                Grid_Contrarecibos_Pendientes.Columns[12].Visible = false;
                Grid_Contrarecibos_Pendientes.Columns[13].Visible = false;
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
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Convenios_Impuestos_Traslado_Dominio
    ///DESCRIPCIÓN          : Llena el grid de Convenios con los registros encontrados
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(Int32 Pagina)
    {
        try
        {
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            DataTable Dt_Convenios_Fraccionamientos;

            Convenios_Traslado.P_Campos_Foraneos = true;
            if (Txt_Busqueda.Text.Trim() != "")
            {
                //Convenios_Traslado.P_Filtros_Dinamicos = "(";
                //Convenios_Traslado.P_Filtros_Dinamicos += Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " = '" + Txt_Busqueda.Text.Trim() + "'";
                //Convenios_Traslado.P_Filtros_Dinamicos += " OR " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%')";
                //Convenios_Traslado.P_Filtros_Dinamicos += ")";
                switch (Cmb_Busqueda_General.SelectedIndex)
                {
                    case 0://POR CONTRARECIBO
                        Convenios_Traslado.P_Cuenta_Predial_ID = " IN (SELECT " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Convert.ToInt64(Txt_Busqueda.Text.Trim()).ToString("0000000000") + "')";
                        Convenios_Traslado.P_No_Convenio = " IN (SELECT " + Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Convenio + " FROM " + Ope_Pre_Convenios_Traslados_Dominio.Tabla_Ope_Pre_Convenios_Traslados_Dominio + " WHERE " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + " IN (SELECT " + Ope_Pre_Contrarecibos.Campo_Cuenta_Predial_ID + " FROM " + Ope_Pre_Contrarecibos.Tabla_Ope_Pre_Contrarecibos + " WHERE " + Ope_Pre_Contrarecibos.Campo_No_Contrarecibo + " = '" + Convert.ToInt64(Txt_Busqueda.Text.Trim()).ToString("0000000000") + "'))";
                        break;
                    case 1://POR CUENTA
                        Convenios_Traslado.P_Cuenta_Predial_ID = " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%')";
                        break;
                    case 2://POR CONVENIO
                        Convenios_Traslado.P_No_Convenio = Convert.ToInt64(Txt_Busqueda.Text.Trim()).ToString("0000000000");
                        break;
                }
            }
            Dt_Convenios_Fraccionamientos = Convenios_Traslado.Consultar_Convenio_Traslado_Dominio();

            if (Dt_Convenios_Fraccionamientos != null)
            {
                //Grid_Convenios_Impuestos_Traslado.Columns[5].Visible = true;
                //Grid_Convenios_Impuestos_Traslado.Columns[6].Visible = true;
                //Grid_Convenios_Impuestos_Traslado.Columns[7].Visible = true;
                Grid_Convenios_Impuestos_Traslado.Columns[8].Visible = true;
                Grid_Convenios_Impuestos_Traslado.Columns[9].Visible = true;
                Grid_Convenios_Impuestos_Traslado.Columns[10].Visible = true;
                Grid_Convenios_Impuestos_Traslado.DataSource = Dt_Convenios_Fraccionamientos;
                Grid_Convenios_Impuestos_Traslado.PageIndex = Pagina;
                Grid_Convenios_Impuestos_Traslado.DataBind();
                //Grid_Convenios_Impuestos_Traslado.Columns[5].Visible = false;
                //Grid_Convenios_Impuestos_Traslado.Columns[6].Visible = false;
                //Grid_Convenios_Impuestos_Traslado.Columns[7].Visible = false;
                Grid_Convenios_Impuestos_Traslado.Columns[8].Visible = false;
                Grid_Convenios_Impuestos_Traslado.Columns[9].Visible = false;
                Grid_Convenios_Impuestos_Traslado.Columns[10].Visible = false;
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
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO_DIVISION", typeof(Double)));
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
                        Hdf_Anio.Value = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Anio].ToString();
                        Hdf_No_Descuento.Value = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_No_Descuento].ToString();
                        Txt_Fecha.Text = Convert.ToDateTime(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
                        Txt_Observaciones.Text = Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Observaciones].ToString();
                        Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Ordinarios]).ToString();
                        Hdf_Desc_Recargos.Value = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Ordinarios]).ToString();
                        Txt_Descuento_Recargos_Moratorios.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Recargos_Moratorios]).ToString();
                        Txt_Descuento_Multas.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Multas]).ToString();
                        Hdf_Desc_Multas.Value = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Descuento_Multas]).ToString();
                        Txt_Total_Adeudo.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Adeudo]).ToString();
                        Txt_Total_Descuento.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Descuento]).ToString();
                        Txt_Sub_Total.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Sub_Total]).ToString();
                        Txt_Porcentaje_Anticipo.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Porcentaje_Anticipo]).ToString();
                        Txt_Total_Anticipo.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Anticipo]).ToString();
                        Txt_Total_Convenio.Text = Convert.ToDouble(Dt_Convenios_Traslado_Dominio.Rows[0][Ope_Pre_Convenios_Traslados_Dominio.Campo_Total_Convenio]).ToString();
                        Grid_Parcialidades.DataSource = Convenios_Traslado_Dominio.P_Dt_Parcialidades;
                        Grid_Parcialidades.PageIndex = 0;
                        Grid_Parcialidades.DataBind();
                        if (Cmb_Estatus.SelectedValue == "ACTIVO")
                        {
                            Cambiar_Convenio_Incumplido();
                        }
                        try
                        {
                            Sumar_Totales_Parcialidades();
                        }
                        catch { }
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

        if (Convert.ToDouble(Txt_Total_Anticipo.Text) < ((Convert.ToDouble(Txt_Monto_Multas.Text) - (Convert.ToDouble(Txt_Descuento_Multas.Text) * Convert.ToDouble(Txt_Monto_Multas.Text)) / 100) + Convert.ToDouble(Txt_Costo_Constancia.Text)))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ El anticipo debe de cubrir el monto de las multas y la constancia.";
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
            Lbl_Mensaje_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    #endregion

    #endregion

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Contrarecibos_Pendientes_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Contrarecibos_Pendientes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Contrarecibos_Pendientes.SelectedIndex = (-1);
            Cargar_Grid_Contrarecibos_Pendientes(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Convenio_Existe
    ///DESCRIPCIÓN          : Valida s ya existe un convenio para este calculo de traslado
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 28/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected Boolean Validar_Convenio_Existe()
    {

        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Conv_Tras = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
        Conv_Tras.P_No_Contrarecibo = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[11].Text;
        Conv_Tras.P_Anio = Convert.ToDateTime(Grid_Contrarecibos_Pendientes.SelectedRow.Cells[4].Text).ToString("yyyy");
        return Conv_Tras.Consultar_Convenio_Existente();

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Contrarecibos_Pendientes_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de fila en el GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Contrarecibos_Pendientes_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Contrarecibos_Pendientes.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = Grid_Contrarecibos_Pendientes.DataKeys[Grid_Contrarecibos_Pendientes.SelectedIndex].Value.ToString();
            if (Validar_Cuenta_Sin_Convenios_Vigente(Hdf_Cuenta_Predial_ID.Value))
            {
                if (Validar_Estatus_Cuenta(Hdf_Cuenta_Predial_ID.Value, "IN ('VIGENTE','BLOQUEADA','PENDIENTE')"))
                {
                    if (!Validar_Convenio_Existe())
                    {
                        Double Descuento_Recargos_Ordinarios = 0;
                        Double Recargos_Ordinarios = 0;
                        Grid_Convenios_Impuestos_Traslado.SelectedIndex = -1;
                        No_Limpiar_Campos_Clave = true;
                        Limpiar_Controles();
                        No_Limpiar_Campos_Clave = false;
                        Hdf_No_Contrarecibo.Value = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[1].Text;
                        Txt_Cuenta_Predial.Text = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[3].Text;
                        Hdf_Cuenta_Predial_ID.Value = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[2].Text;
                        Txt_Monto_Recargos.Text = Convert.ToDouble(Grid_Contrarecibos_Pendientes.SelectedRow.Cells[5].Text.Replace("$", "")).ToString("0.00");
                        Txt_Monto_Multas.Text = Convert.ToDouble(Grid_Contrarecibos_Pendientes.SelectedRow.Cells[7].Text.Replace("$", "")).ToString("0.00");
                        Hdf_No_Calculo.Value = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[11].Text;
                        Hdf_Anio.Value = Convert.ToDateTime(Grid_Contrarecibos_Pendientes.SelectedRow.Cells[4].Text).ToString("yyyy");
                        Cargar_Descuentos();
                        DataTable Dt_Calculo = Calcular_Impuestos();
                        Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_TRASLADO"].ToString()).ToString("#,###,###,###,##0.00");
                        Txt_Imp_Div.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_DIVISION"].ToString()).ToString("#,###,###,###,##0.00");
                        Txt_Costo_Constancia.Text = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[13].Text.Replace("$", "");
                        Txt_Costo_Constancia.Text = Dt_Calculo.Rows[0]["COSTO_CONSTANCIA"].ToString();
                        Txt_Cuenta_Predial_TextChanged();
                        Txt_Estatus_Calculo.Text = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[10].Text;
                        Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado;
                        Txt_Realizo_Calculo.Text = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[12].Text;
                        Txt_Fecha_Calculo.Text = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[4].Text.Replace("-", "/");
                        Hdf_No_Contrarecibo.Value = Grid_Contrarecibos_Pendientes.SelectedRow.Cells[11].Text.Replace("-", "/");
                        Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                        Cmb_Estatus.SelectedIndex = 1;
                        Txt_Solicitante.Text = Txt_Propietario.Text;
                        Txt_RFC.Text = Hdf_RFC.Value;
                        Calcular_Total_Descuento();
                        if (Grid_Contrarecibos_Pendientes.SelectedRow.Cells[5].Text.Trim() != "")
                        {
                            Recargos_Ordinarios = Convert.ToDouble(Grid_Contrarecibos_Pendientes.SelectedRow.Cells[5].Text.Replace("$", "").Trim());
                        }
                        if (Grid_Contrarecibos_Pendientes.SelectedRow.Cells[6].Text.Trim() != "")
                        {
                            Descuento_Recargos_Ordinarios = Convert.ToDouble(Grid_Contrarecibos_Pendientes.SelectedRow.Cells[6].Text.Replace("$", "").Trim());
                        }
                        Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["DESC_RECARGO"].ToString()).ToString("0.00");
                        Txt_Descuento_Recargos_Moratorios.Text = "0.00";
                        Txt_Descuento_Multas.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["DESC_MULTA"].ToString()).ToString("0.00");
                        Calcular_Total_Descuento();
                        Calcular_Total_Adeudos();
                        Calcular_Sub_Total();
                        Calcular_Total_Anticipo();
                        Calcular_Total_Convenio();
                        //Cargar_Convenio();
                        Grid_Contrarecibos_Pendientes.Visible = false;
                        Grid_Convenios_Impuestos_Traslado.Visible = false;
                        Panel_Datos.Visible = true;
                        Btn_Salir.AlternateText = "Atras";
                        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                        Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                        if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
                        {
                            Cargar_Datos();
                        }
                        //Session["CUENTA_PREDIAL_ID"] = Hdf_Cuenta_Predial_ID.Value;
                        //Session["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
                        //Session["AÑO"] = Convert.ToDateTime(Txt_Fecha_Calculo.Text.Trim()).Year.ToString();
                        //Session["NO_CALCULO"] = Hdf_No_Calculo.Value;
                        Btn_Detalles_Cuenta_Predial.Enabled = true;
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('Ya existe convenio para este cálculo de impuesto');", true);
                        Btn_Salir.AlternateText = "Atras";
                        Btn_Salir_Click(null, null);
                        Grid_Convenios_Impuestos_Traslado.SelectedIndex = -1;
                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('La Cuenta seleccionada No tiene un estatus Vigente o Bloqueado.');", true);
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Ya existen Convenios para la Cuenta Seleccioanda.');", true);
                //Btn_Salir_Click(null, null);
            }
        }
        Cargar_Ventana_Emergente_Resumen_Predio();
        Cargar_Ventana_Emergente_Resolucion();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Fraccionamientos_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_Impuestos_Traslado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Convenios_Impuestos_Traslado.SelectedIndex = (-1);
            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Fraccionamientos_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de fila en el GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_Impuestos_Traslado_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid_Convenios_Impuestos_Traslado.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = Grid_Convenios_Impuestos_Traslado.DataKeys[Grid_Convenios_Impuestos_Traslado.SelectedIndex].Value.ToString();
            Hdf_No_Calculo.Value = Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[10].Text;
            //if (Validar_Cuenta_Sin_Convenios_Vigente(Hdf_Cuenta_Predial_ID.Value))
            //{
            //if (Validar_Estatus_Cuenta(Hdf_Cuenta_Predial_ID.Value, "VIGENTE"))
            //{
            Hdf_No_Convenio.Value = Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[1].Text;
            Cargar_Convenio();
            Txt_Cuenta_Predial_TextChanged();
            //------------------------------------------
            Consultar_Adeudos_Convenio(false);
            DataTable Dt_Calculo = Calcular_Impuestos();
            if (Dt_Calculo.Rows[0]["MONTO_TRASLADO"].ToString() != "")
            {
                Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_TRASLADO"].ToString()).ToString("#,###,###,###,##0.00");
            }
            else
            {
                Txt_Monto_Impuesto.Text = "0.00";
            }
            if (Dt_Calculo.Rows[0]["MONTO_DIVISION"].ToString() != "")
            {
                Txt_Imp_Div.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_DIVISION"].ToString()).ToString("#,###,###,###,##0.00");
            }
            else
            {
                Txt_Imp_Div.Text = "0.00";
            }
            if (Dt_Calculo.Rows[0]["COSTO_CONSTANCIA"].ToString() != "")
            {
                Txt_Costo_Constancia.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["COSTO_CONSTANCIA"].ToString()).ToString("#,###,###,###,##0.00");
            }
            else
            {
                Txt_Costo_Constancia.Text = "0.00";
            }
            if (Dt_Calculo.Rows[0]["MONTO_MULTAS"].ToString() != "")
            {
                Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_MULTAS"].ToString()).ToString("#,###,###,###,##0.00");
            }
            else
            {
                Txt_Monto_Multas.Text = "0.00";
            }
            if (Dt_Calculo.Rows[0]["MONTO_RECARGOS"].ToString() != "")
            {
                Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Calculo.Rows[0]["MONTO_RECARGOS"].ToString()).ToString("#,###,###,###,##0.00");
            }
            else
            {
                Txt_Monto_Recargos.Text = "0.00";
            }
            Txt_Estatus_Calculo.Text = Dt_Calculo.Rows[0]["ESTATUS"].ToString();
            Txt_Fecha_Calculo.Text = Convert.ToDateTime(Dt_Calculo.Rows[0]["FECHA_CALCULO"].ToString()).ToString("dd/MMM/yyyy");
            Txt_Realizo_Calculo.Text = Dt_Calculo.Rows[0]["REALIZO_CALCULO"].ToString();

            Calcular_Total_Adeudos();
            Calcular_Total_Descuento();
            Calcular_Sub_Total();
            Calcular_Total_Convenio();
            Txt_Cuenta_Predial_TextChanged();
            Btn_Detalles_Cuenta_Predial.Enabled = true;
            //------------------------------------------
            Grid_Contrarecibos_Pendientes.SelectedIndex = -1;
            Grid_Convenios_Impuestos_Traslado.Visible = false;
            Grid_Contrarecibos_Pendientes.Visible = false;
            Panel_Datos.Visible = true;
            Btn_Salir.AlternateText = "Atras";
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Cuenta_Pendiente = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            Cuenta_Pendiente.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            if (!Cuenta_Pendiente.Consultar_Cuenta_Pendiente())
            {
                Cargar_Datos();
            }
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('La Cuenta seleccionada No tiene un estatus Vigente.');", true);
            //}
            //}
            //else
            //{
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Ya existen Convenios para la Cuenta Seleccioanda.');", true);
            //    //Btn_Salir_Click(null, null);
            //}

            //Session["NO_CALCULO"] = Hdf_No_Calculo.Value;
            //Session["CUENTA_PREDIAL_ID"] = Hdf_Cuenta_Predial_ID.Value;
            //Session["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
            //if (Txt_Fecha_Calculo.Text.Trim() != "")
            //{
            //    Session["AÑO"] = Convert.ToDateTime(Txt_Fecha_Calculo.Text.Trim()).Year.ToString();
            //}
        }
        Grid_Contrarecibos_Pendientes.SelectedIndex = -1;
        Cargar_Ventana_Emergente_Resumen_Predio();
        Cargar_Ventana_Emergente_Resolucion();
    }

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
            if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[9].Text.Replace("&nbsp;", "").Equals(""))
            {
                Convenio.P_Reestructura = false;
            }
            else
            {
                Convenio.P_Reestructura = true;
            }
            Dt_Adeudos = Convenio.Consultar_Adeudos_Convenio(Total_O_A_Pagar);
            if (Dt_Adeudos.Rows.Count > 0)
            {
                Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_IMPUESTO"].ToString()).ToString("0.00");
                Txt_Monto_Multas.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_MULTAS"].ToString()).ToString("0.00");
                Txt_Monto_Recargos.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_ORDINARIOS"].ToString()).ToString("0.00");
                Txt_Costo_Constancia.Text = Convert.ToDouble(Dt_Adeudos.Rows[0]["TOTAL_CONSTANCIA"].ToString()).ToString("0.00");
                DataTable Dt_Impuestos = Calcular_Impuestos();
                if (Dt_Impuestos != null)
                {
                    if (Dt_Impuestos.Rows.Count > 0)
                    {
                        if (Dt_Impuestos.Rows[0]["MONTO_TRASLADO"] != System.DBNull.Value)
                        {
                            Txt_Monto_Impuesto.Text = Convert.ToDouble(Dt_Impuestos.Rows[0]["MONTO_TRASLADO"].ToString()).ToString("###,###,###,##0.00");
                        }
                        else
                        {
                            Txt_Monto_Impuesto.Text = "0.00";
                        }
                        if (Dt_Impuestos.Rows[0]["MONTO_DIVISION"] != System.DBNull.Value)
                        {
                            Txt_Imp_Div.Text = Convert.ToDouble(Dt_Impuestos.Rows[0]["MONTO_DIVISION"].ToString()).ToString("###,###,###,##0.00");
                        }
                        else
                        {
                            Txt_Imp_Div.Text = "0.00";
                        }
                    }
                    else
                    {
                        Txt_Monto_Impuesto.Text = "0.00";
                        Txt_Imp_Div.Text = "0.00";
                    }
                }
                else
                {
                    Txt_Monto_Impuesto.Text = "0.00";
                    Txt_Imp_Div.Text = "0.00";
                }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para dar de Alta un nuevo Convenios_Traslado_Dominio
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid_Contrarecibos_Pendientes.SelectedIndex > -1 || Page.PreviousPage != null)
            {
                Boton_Pulsado = ((ImageButton)sender).ID;
                if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
                {
                    Configuracion_Formulario(false);
                    if (Grid_Contrarecibos_Pendientes.SelectedIndex == -1)
                    {
                        Limpiar_Controles();
                    }
                    Btn_Nuevo.AlternateText = "Dar de Alta";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.Visible = false;
                    Btn_Imprimir.Visible = false;

                    Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado.ToUpper();
                    Txt_Fecha.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                    Cmb_Estatus.SelectedIndex = 1;
                    Grid_Convenios_Impuestos_Traslado.Visible = false;
                    Grid_Contrarecibos_Pendientes.Visible = false;
                    Panel_Datos.Visible = true;
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                        Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                        if (Hdf_Propietario_ID.Value != "")
                        {
                            Convenios_Traslado_Dominio.P_Propietario_ID = Hdf_Propietario_ID.Value;
                        }
                        else
                        {
                            if (Cmb_Tipo_Solicitante.SelectedIndex == 1)
                            {
                                Convenios_Traslado_Dominio.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();
                                Convenios_Traslado_Dominio.P_RFC = Txt_RFC.Text.Trim().ToUpper();
                            }
                        }
                        Convenios_Traslado_Dominio.P_Realizo = Cls_Sessiones.Empleado_ID;
                        Convenios_Traslado_Dominio.P_Estatus = Cmb_Estatus.SelectedValue;
                        Convenios_Traslado_Dominio.P_Numero_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
                        Convenios_Traslado_Dominio.P_Periodicidad_Pago = Cmb_Periodicidad_Pago.SelectedValue;
                        Convenios_Traslado_Dominio.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text);
                        Convenios_Traslado_Dominio.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                        Convenios_Traslado_Dominio.P_Descuento_Recargos_Ordinarios = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text);
                        Convenios_Traslado_Dominio.P_Descuento_Recargos_Moratorios = Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text);
                        Convenios_Traslado_Dominio.P_Descuento_Multas = Convert.ToDouble(Txt_Descuento_Multas.Text);
                        Convenios_Traslado_Dominio.P_Total_Adeudo = Convert.ToDouble(Txt_Total_Adeudo.Text);
                        Convenios_Traslado_Dominio.P_Total_Descuento = Convert.ToDouble(Txt_Total_Descuento.Text);
                        Convenios_Traslado_Dominio.P_Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
                        Convenios_Traslado_Dominio.P_Porcentaje_Anticipo = Convert.ToDouble(Txt_Porcentaje_Anticipo.Text);
                        Convenios_Traslado_Dominio.P_Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
                        Convenios_Traslado_Dominio.P_Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
                        Convenios_Traslado_Dominio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                        Convenios_Traslado_Dominio.P_No_Contrarecibo = Hdf_No_Contrarecibo.Value;
                        Convenios_Traslado_Dominio.P_No_Calculo = Hdf_No_Calculo.Value;
                        Convenios_Traslado_Dominio.P_Dt_Parcialidades = Crear_Tabla_Parcialidades();
                        Convenios_Traslado_Dominio.P_No_Descuento = Hdf_No_Descuento.Value.Trim();
                        Convenios_Traslado_Dominio.P_Anio = Hdf_Anio.Value;
                        if (Convenios_Traslado_Dominio.Alta_Convenio_Traslado_Dominio())
                        {
                            //Insertar_Pasivo("CTRA" + Convenios_Traslado_Dominio.P_No_Convenio);
                            Hdf_No_Convenio.Value = Convenios_Traslado_Dominio.P_No_Convenio;
                            //Imprimir_Reporte(Crear_Ds_Convenios_Traslado(), "Rpt_Convenio_TD.rpt", "Rpt_Convenio_Traslado_Dominio", "Window_Frm", "Convenio");
                            Imprimir_Convenio();
                            Imprimir_Reporte(Crear_Ds_Convenios_Folio("CTRA" + Convert.ToInt32(Hdf_No_Convenio.Value)), "Rpt_Pre_Folio_Convenios.rpt", "Rpt_Convenio_Traslado", "Window_Rpt", "Folio");
                            Convenios_Traslado_Dominio.Eliminar_Pasivo("TD" + Convert.ToInt32(Convenios_Traslado_Dominio.P_No_Calculo).ToString() + Convenios_Traslado_Dominio.P_Anio);
                            Configuracion_Formulario(true);
                            Limpiar_Controles();
                            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
                            Cargar_Grid_Contrarecibos_Pendientes(0);
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Alta de Convenio por Traslado de Dominio Exitosa');", true);
                            Btn_Nuevo.AlternateText = "Nuevo";
                            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                            Btn_Modificar.Visible = true;
                            Btn_Imprimir.Visible = true;
                            Btn_Salir.AlternateText = "Salir";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                            Grid_Contrarecibos_Pendientes.Visible = true;
                            Grid_Convenios_Impuestos_Traslado.Visible = true;
                            Panel_Datos.Visible = false;
                        }
                        else
                        {
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Alta de Convenio por Traslado de Dominio No fue Exitosa');", true);
                        }
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Seleccione un impuesto por favor');", true);
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
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 22/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Insertar_Pasivo(String Referencia)
    {
        try
        {
            //OracleConnection Cn = new OracleConnection();
            //OracleCommand Cmd = new OracleCommand();
            //OracleTransaction Trans = null;
            Cls_Cat_Pre_Claves_Ingreso_Negocio Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
            Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Calculo_Impuesto_Traslado = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
            DataTable Dt_Clave;
            Double Recargos_Ord = 0;
            Double Multas = 0;
            Double Impuestos = 0;
            Double Constancia = 0;

            String Consulta = " SELECT TP." + Cat_Pre_Tipos_Predio.Campo_Descripcion + " FROM " + Cat_Pre_Tipos_Predio.Tabla_Cat_Pre_Tipos_Predio + " TP LEFT OUTER JOIN " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CP ON TP." + Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + "=CP." + Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID + " WHERE CP." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + Hdf_Cuenta_Predial_ID.Value + "'";

            ////// crear transaccion para modificar tabla de calculos y de adeudos folio
            ////Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            ////Cn.Open();
            ////Trans = Cn.BeginTransaction();
            ////Cmd.Connection = Cn;
            ////Cmd.Transaction = Trans;
            ////Calculo_Impuesto_Traslado.P_Cmd_Calculo = Cmd;

            Recargos_Ord = Convert.ToDouble(Txt_Monto_Recargos.Text);
            Multas = Convert.ToDouble(Txt_Monto_Multas.Text);
            Impuestos = Convert.ToDouble(Txt_Monto_Impuesto.Text) + Convert.ToDouble(Txt_Imp_Div.Text);
            Constancia = Convert.ToDouble(Txt_Costo_Constancia.Text);

            if (Impuestos > 0)
            {
                Claves_Ingreso.P_Tipo = "TRASLADO";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "IMPUESTO " + Obtener_Dato_Consulta(Consulta);
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "IMPUESTO TRASLADO";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Impuestos.ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Realizo_Calculo.Text.ToUpper();
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }
            if (Recargos_Ord > 0)
            {
                Claves_Ingreso.P_Tipo = "TRASLADO";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "RECARGOS ORDINARIOS";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "RECARGOS ORDINARIOS";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Recargos_Ord.ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Realizo_Calculo.Text.ToUpper();
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }
            if (Multas > 0)
            {
                Claves_Ingreso.P_Tipo = "TRASLADO";
                Claves_Ingreso.P_Tipo_Predial_Traslado = "MULTAS";
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "MULTAS";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Multas.ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Realizo_Calculo.Text.ToUpper();
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
            }
            if (Constancia > 0)
            {
                Cls_Cat_Pre_Tipos_Constancias_Negocio Cls_Constancia = new Cls_Cat_Pre_Tipos_Constancias_Negocio();
                DataTable Dt_Constancia;
                Cls_Constancia.P_Nombre = "CONSTANCIA DE NO ADEUDO";
                Dt_Constancia = Cls_Constancia.Consultar_Tipos_Constancias();



                Claves_Ingreso.P_Documento_ID = Dt_Constancia.Rows[0][Cat_Pre_Tipos_Constancias.Campo_Tipo_Constancia_ID].ToString();
                Dt_Clave = Claves_Ingreso.Consultar_Clave_Ingreso();
                if (Dt_Clave.Rows.Count > 0)
                {
                    Calculo_Impuesto_Traslado.P_Referencia = Referencia;
                    Calculo_Impuesto_Traslado.P_Descripcion = "CONSTANCIA";
                    Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                    Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                    Calculo_Impuesto_Traslado.P_Monto_Total_Pagar = Constancia.ToString("0.00");
                    Calculo_Impuesto_Traslado.P_Fecha_Tramite = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Calculo_Impuesto_Traslado.P_Contribuyente = Txt_Realizo_Calculo.Text.ToUpper();
                    Calculo_Impuesto_Traslado.P_Fecha_Vencimiento_Pasivo = DateTime.Now.ToString("dd/MMM/yyyy");
                    Calculo_Impuesto_Traslado.Alta_Pasivo();
                }
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Convenios_Traslado_Dominio.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        if (Grid_Convenios_Impuestos_Traslado.SelectedIndex > -1)
        {
            try
            {
                Boton_Pulsado = ((ImageButton)sender).ID;
                if (!Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[8].Text.Equals("PAGADO") && Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[9].Text.Replace("&nbsp;", "").Equals("") && !Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CANCELADO") && !Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CUENTA_CANCELADA"))
                {
                    if (Btn_Modificar.AlternateText.Equals("Modificar"))
                    {
                        if (Txt_Numero_Convenio.Text.Trim() != "")
                        {
                            Btn_Modificar.AlternateText = "Actualizar";
                            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                            Btn_Salir.AlternateText = "Cancelar";
                            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                            Btn_Nuevo.Visible = false;
                            Btn_Imprimir.Visible = false;
                            Configuracion_Formulario(false);
                            Grid_Convenios_Impuestos_Traslado.Visible = false;
                            Grid_Contrarecibos_Pendientes.Visible = false;
                            Panel_Datos.Visible = true;
                            Cmb_Estatus.Items.RemoveAt(4);
                            Cmb_Estatus.Items.RemoveAt(4);
                            Cmb_Estatus.Items.RemoveAt(4);
                        }
                        else
                        {
                            Lbl_Ecabezado_Mensaje.Text = "Selecciona el Registro que quieres Modificar.";
                            Lbl_Mensaje_Error.Text = "";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                    else
                    {
                        if (Validar_Componentes())
                        {
                            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                            Convenios_Traslado_Dominio.P_No_Convenio = Txt_Numero_Convenio.Text;
                            Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                            if (Hdf_Propietario_ID.Value != "")
                            {
                                Convenios_Traslado_Dominio.P_Propietario_ID = Hdf_Propietario_ID.Value;
                            }
                            else
                            {
                                if (Cmb_Tipo_Solicitante.SelectedIndex == 1)
                                {
                                    Convenios_Traslado_Dominio.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();
                                    Convenios_Traslado_Dominio.P_RFC = Txt_RFC.Text.Trim().ToUpper();
                                }
                            }
                            Convenios_Traslado_Dominio.P_Realizo = Cls_Sessiones.Empleado_ID;
                            Convenios_Traslado_Dominio.P_Estatus = Cmb_Estatus.SelectedValue;
                            Convenios_Traslado_Dominio.P_Numero_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
                            Convenios_Traslado_Dominio.P_Periodicidad_Pago = Cmb_Periodicidad_Pago.SelectedValue;
                            Convenios_Traslado_Dominio.P_Fecha = Convert.ToDateTime(Txt_Fecha.Text);
                            Convenios_Traslado_Dominio.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                            Convenios_Traslado_Dominio.P_Descuento_Recargos_Ordinarios = Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text);
                            Convenios_Traslado_Dominio.P_Descuento_Recargos_Moratorios = Convert.ToDouble(Txt_Descuento_Recargos_Moratorios.Text);
                            Convenios_Traslado_Dominio.P_Descuento_Multas = Convert.ToDouble(Txt_Descuento_Multas.Text);
                            Convenios_Traslado_Dominio.P_Total_Adeudo = Convert.ToDouble(Txt_Total_Adeudo.Text);
                            Convenios_Traslado_Dominio.P_Total_Descuento = Convert.ToDouble(Txt_Total_Descuento.Text);
                            Convenios_Traslado_Dominio.P_Sub_Total = Convert.ToDouble(Txt_Sub_Total.Text);
                            Convenios_Traslado_Dominio.P_Porcentaje_Anticipo = Convert.ToDouble(Txt_Porcentaje_Anticipo.Text);
                            Convenios_Traslado_Dominio.P_Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
                            Convenios_Traslado_Dominio.P_Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
                            Convenios_Traslado_Dominio.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                            Convenios_Traslado_Dominio.P_No_Descuento = Hdf_No_Descuento.Value.Trim();
                            Convenios_Traslado_Dominio.P_Dt_Parcialidades = Crear_Tabla_Parcialidades();
                            Convenios_Traslado_Dominio.P_No_Descuento = Hdf_No_Descuento.Value.Trim();
                            if (Convenios_Traslado_Dominio.Modificar_Convenio_Traslado_Dominio())
                            {
                                Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Mod_Pasivo = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                                if (!(Cmb_Estatus.SelectedValue == "CANCELADO"))
                                {
                                    Mod_Pasivo.Cancelar_Pasivo("CTRA" + Convert.ToInt32(Convenios_Traslado_Dominio.P_No_Convenio), Cmb_Estatus.SelectedValue, Grid_Parcialidades.Rows[0].Cells[7].Text.Replace("$", ""));
                                }
                                else
                                {
                                    Insertar_Pasivo("TD" + Convert.ToInt32(Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[10].Text).ToString() + Convert.ToDateTime(Txt_Fecha_Calculo.Text).ToString("yyyy"));
                                }
                                if (Cmb_Estatus.SelectedValue == "ACTIVO")
                                {
                                    Imprimir_Reporte(Crear_Ds_Convenios_Folio("CTRA" + Convert.ToInt32(Hdf_No_Convenio.Value)), "Rpt_Pre_Folio_Convenios.rpt", "Rpt_Convenio_Traslado", "Window_Rpt", "Folio");
                                    Imprimir_Convenio();
                                }
                                Limpiar_Controles();
                                Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('Actualización de Convenio por Traslado de Dominio Exitosa');", true);
                                Btn_Modificar.AlternateText = "Modificar";
                                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                                Btn_Nuevo.Visible = true;
                                Btn_Modificar.Visible = true;
                                Btn_Imprimir.Visible = true;
                                Btn_Salir.AlternateText = "Salir";
                                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                Configuracion_Formulario(true);
                                Grid_Convenios_Impuestos_Traslado.Visible = true;
                                Grid_Contrarecibos_Pendientes.Visible = true;
                                Panel_Datos.Visible = false;
                                Cmb_Estatus.Items.Insert(4, new ListItem("TERMINADO", "TERMINADO"));
                                Cmb_Estatus.Items.Insert(5, new ListItem("INCUMPLIDO", "INCUMPLIDO"));
                                Cmb_Estatus.Items.Insert(6, new ListItem("CUENTA CANCELADA", "CUENTA_CANCELADA"));
                            }
                            else
                            {
                                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('Actualización de Convenio por Traslado de Dominio No fue Exitosa');", true);
                            }
                        }
                    }
                }
                else if (!Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[9].Text.Replace("&nbsp;", "").Equals(""))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('El convenio ya contiene reestructura(s)');", true);
                }
                else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CANCELADO"))
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('El convenio se encuentra cancelado');", true);
                }
                else if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[6].Text.Equals("CUENTA_CANCELADA"))
                {
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de Dominio", "alert('No puede modificar el convenio porque ya contiene uno o más pagos realizados.');", true);
                }
            }
            catch (Exception Ex)
            {
                Lbl_Ecabezado_Mensaje.Text = Ex.Message;
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        else
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Seleccione un convenio por favor');", true);
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
    private DataSet Crear_Ds_Convenios_Folio(String Folio)
    {
        Ds_Pre_Folio_Convenios Ds_Convenio = new Ds_Pre_Folio_Convenios();
        DataTable Dt_Convenio = Ds_Convenio.Tables["Dt_Convenio"];
        DataRow Dr_Convenio;

        Dr_Convenio = Dt_Convenio.NewRow();
        Dr_Convenio["FOLIO"] = Folio;
        Dr_Convenio["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text.ToUpper();
        Dr_Convenio["PROPIETARIO"] = Txt_Propietario.Text.ToUpper();
        Dr_Convenio["TIPO_CONVENIO"] = "Convenio de traslado de dominio";
        String Domicilio = "";
        Domicilio = Txt_Calle.Text;
        if (Txt_No_Exterior.Text != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", #";
            }
            Domicilio += Txt_No_Exterior.Text;
        }
        if (Txt_No_Interior.Text != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", #";
            }
            Domicilio += Txt_No_Interior.Text;
        }
        if (Txt_Colonia.Text != "")
        {
            if (Domicilio != "")
            {
                Domicilio += ", ";
            }
            Domicilio += Txt_Colonia.Text;
        }

        Dr_Convenio["UBICACION"] = Domicilio;
        Dr_Convenio["MONTO_IMPUESTO"] = Grid_Parcialidades.Rows[0].Cells[6].Text;
        Dr_Convenio["MONTO_MULTAS"] = Grid_Parcialidades.Rows[0].Cells[3].Text;
        Dr_Convenio["MONTO_RECARGOS_ORD"] = Grid_Parcialidades.Rows[0].Cells[4].Text;
        Dr_Convenio["MONTO_RECARGOS_MOR"] = Grid_Parcialidades.Rows[0].Cells[5].Text;
        Dr_Convenio["CONSTANCIA"] = Grid_Parcialidades.Rows[0].Cells[2].Text;
        Dr_Convenio["TOTAL_A_PAGAR"] = Grid_Parcialidades.Rows[0].Cells[7].Text;
        Dt_Convenio.Rows.Add(Dr_Convenio);

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
    private void Mostrar_Reporte(String Nombre_Reporte, String Formato, String Frm_Formato)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            //if (Formato == "PDF")
            //{
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), Frm_Formato, "window.open('" + Pagina + "', '" + Formato + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
            //else if (Formato == "Excel")
            //{
            //    String Ruta = "../../Reporte/" + Nombre_Reporte;
            //    ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            //}
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
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo, String Formato, String Tipo)
    {
        ReportDocument Reporte = new ReportDocument();
        String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
        try
        {
            Reporte.Load(File_Path);
            Reporte.SetDataSource(Ds_Convenios);
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
            Mostrar_Reporte(Archivo_PDF, Tipo, Formato);
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Boton_Pulsado = ((ImageButton)sender).ID;
            Limpiar_Controles();
            Grid_Convenios_Impuestos_Traslado.SelectedIndex = (-1);
            Grid_Parcialidades.SelectedIndex = (-1);
            Cargar_Grid_Contrarecibos_Pendientes(0);
            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
            if ((Grid_Contrarecibos_Pendientes.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
                && (Grid_Convenios_Impuestos_Traslado.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0))
            {
                Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotrarón coincidencias";
                Lbl_Mensaje_Error.Text = "";
                Div_Contenedor_Msj_Error.Visible = true;
                Txt_Busqueda.Text = "";
                //Cargar_Convenio();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Click
    /////DESCRIPCIÓN          : Elimina un Convenios_Traslado_Dominio de la Base de Datos
    /////PARAMETROS          :     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Julio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Eliminar_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Grid_Parcialidades.Rows.Count > 0 && Grid_Parcialidades.SelectedIndex > (-1))
    //        {
    //            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
    //            Convenios_Traslado_Dominio.P_Folio = Grid_Parcialidades.SelectedRow.Cells[3].Text;
    //            if (Convenios_Traslado_Dominio.Eliminar_Constancia_Propiedad())
    //            {
    //                Grid_Parcialidades.SelectedIndex = (-1);
    //                Llenar_Tabla_Constancias_Propiedad(Grid_Parcialidades.PageIndex);
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Fraccionamientos", "alert('Convenio por Fraccionamiento fue Eliminada Exitosamente');", true);
    //                Limpiar_Controles();
    //            }
    //            else
    //            {
    //                ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Fraccionamientos", "alert('La Convenio por Fraccionamiento No fue Eliminada');", true);
    //            }
    //        }
    //        else
    //        {
    //            Lbl_Ecabezado_Mensaje.Text = "Debe seleccionar el Registro que se desea Eliminar.";
    //            Lbl_Mensaje_Error.Text = "";
    //            Div_Contenedor_Msj_Error.Visible = true;
    //        }
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Ecabezado_Mensaje.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //        Div_Contenedor_Msj_Error.Visible = true;
    //    }

    //}

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Julio/2011
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
            if (Btn_Modificar.AlternateText == "Actualizar")
            {
                Cmb_Estatus.Items.Insert(4, new ListItem("TERMINADO", "TERMINADO"));
                Cmb_Estatus.Items.Insert(5, new ListItem("INCUMPLIDO", "INCUMPLIDO"));
                Cmb_Estatus.Items.Insert(6, new ListItem("CUENTA CANCELADA", "CUENTA_CANCELADA"));
            }
            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Imprimir.Visible = true;
            Configuracion_Formulario(true);
            Limpiar_Controles();
            Cargar_Grid_Convenios_Impuestos_Traslado_Dominio(0);
            Cargar_Grid_Contrarecibos_Pendientes(0);
            Grid_Contrarecibos_Pendientes.Visible = true;
            Grid_Contrarecibos_Pendientes.SelectedIndex = -1;
            Grid_Convenios_Impuestos_Traslado.SelectedIndex = -1;
            Grid_Convenios_Impuestos_Traslado.Visible = true;
            Panel_Datos.Visible = false;
            Btn_Salir.AlternateText = "Salir";
            Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
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
            Cargar_Ventana_Emergente_Resumen_Predio();
            Cargar_Ventana_Emergente_Resolucion();
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
                Impuestos_Traslado_Tominio.P_Estatus = "LISTO";
                Dt_Impuestos_Derechos_Supervision = Impuestos_Traslado_Tominio.Consulta_Calculos_Contrarecibo();

                if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
                {
                    Txt_Estatus_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0]["ESTATUS_CALCULO"].ToString();
                    if (Dt_Impuestos_Derechos_Supervision.Rows[0]["FECHA_CALCULO"] != System.DBNull.Value)
                    {
                        //Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo].ToString();
                        Txt_Fecha_Calculo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Impuestos_Derechos_Supervision.Rows[0]["FECHA_CALCULO"].ToString()));
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

    private Boolean Validar_Existe_Convenio_Activo(String No_Convenio, String Cuenta_Predial_ID)
    {
        Boolean Convenio_Activo = false;
        if (Grid_Convenios_Impuestos_Traslado.SelectedIndex != -1 || Grid_Contrarecibos_Pendientes.SelectedIndex != -1)
        {
            Convenio_Activo = false;
        }
        else
        {
            Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Traslado_Dominio_Activos = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
            DataTable Dt_Convenios_Traslado_Dominio_Activos;
            if (No_Convenio != "")
            {
                Convenios_Traslado_Dominio_Activos.P_No_Convenio = No_Convenio;
            }
            else
            {
                if (Cuenta_Predial_ID != "")
                {
                    Convenios_Traslado_Dominio_Activos.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                }
            }
            if (No_Convenio != "" || Cuenta_Predial_ID != "")
            {
                Convenios_Traslado_Dominio_Activos.P_Estatus = "ACTIVO";
                Dt_Convenios_Traslado_Dominio_Activos = Convenios_Traslado_Dominio_Activos.Consultar_Convenio_Traslado_Dominio();
                if (Dt_Convenios_Traslado_Dominio_Activos.Rows.Count > 0)
                {
                    Convenio_Activo = true;
                }
                else
                {
                    Convenio_Activo = false;
                }
            }
        }
        return Convenio_Activo;
    }

    private double Calcular_Total_Adeudos()
    {
        Double Monto_Impuesto = 0;
        Double Monto_Recargos = 0;
        Double Monto_Multas = 0;
        Double Monto_Constancia = 0;
        Double Total_Adeudo = 0;

        if (Txt_Monto_Impuesto.Text.Trim() != "" && Txt_Imp_Div.Text != "")
        {
            Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text) + Convert.ToDouble(Txt_Imp_Div.Text);
        }
        else
        {
            Txt_Monto_Impuesto.Text = "0.00";
            Txt_Imp_Div.Text = "0.00";
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
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("###,###,##0.00");
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
                Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text) + Convert.ToDouble(Txt_Imp_Div.Text);
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
            Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
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
                    Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
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
                Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
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

        Grid_Parcialidades.FooterRow.Cells[1].Text = Total_Honorarios.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[2].Text = Total_Constancia.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[3].Text = Total_Multas.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[4].Text = Total_Recargos_Ordinarios.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[5].Text = Total_Recargos_Moratorios.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[6].Text = Total_Impuesto.ToString("$###,###,##0.00");
        Grid_Parcialidades.FooterRow.Cells[7].Text = Total_Importe.ToString("$###,###,##0.00");
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
            //if (Cuenta.P_RFC_Propietario != null || Cuenta.P_Nombre_Calle != null || Cuenta.P_Nombre_Propietario != null || Cuenta.P_Nombre_Colonia != null || Cuenta.P_No_Exterior != null || Cuenta.P_No_Interior != null || Cuenta.P_Pro_Propietario_ID != null)
            //{
            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;
            if (Cuenta.P_Propietario_ID != null)
            {
                Hdf_Propietario_ID.Value = Cuenta.P_Propietario_ID;
            }
            if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
            {
                Hdf_RFC.Value = Cuenta.P_RFC_Propietario;
                Txt_Solicitante.Text = Txt_Propietario.Text;
            }
            Txt_RFC.Text = Hdf_RFC.Value;
            //}
            //else
            //{
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
                if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
                {
                    Txt_RFC.Text = Dt_Orden.Rows[0]["RFC"].ToString();
                    Txt_Solicitante.Text = Dt_Orden.Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                }
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

    /////******************************************************************************* 
    /////NOMBRE DE LA FUNCIÓN : Imprimir_Reporte
    /////DESCRIPCIÓN          : Genera el reporte de Crystal con los datos proporcionados en el DataTable y lo manda a la impresora default
    /////PARAMETROS: 
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Julio/2011
    /////MODIFICO: 
    /////FECHA_MODIFICO:
    /////CAUSA_MODIFICACIÓN:
    /////*******************************************************************************
    //private void Imprimir_Reporte(DataTable Dt_Constancias, String Nombre_Reporte, String Nombre_Archivo)
    //{
    //    ReportDocument Reporte = new ReportDocument();
    //    String File_Path = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);
    //    Reporte.Load(File_Path);
    //    Reporte.Subreports["Rpt_Constancias_Propiedad"].SetDataSource(Dt_Constancias);
    //    Reporte.Subreports["Rpt_Constancias_No_Propiedad"].SetDataSource(Dt_Constancias);
    //    Reporte.Subreports["Rpt_Constancias_No_Adeudo"].SetDataSource(Dt_Constancias);
    //    Reporte.Subreports["Rpt_Certificaciones"].SetDataSource(Dt_Constancias);

    //    String Archivo_PDF = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar        
    //    ExportOptions Export_Options = new ExportOptions();
    //    DiskFileDestinationOptions Disk_File_Destination_Options = new DiskFileDestinationOptions();
    //    Disk_File_Destination_Options.DiskFileName = Server.MapPath("../../Reporte/" + Archivo_PDF);
    //    Export_Options.ExportDestinationOptions = Disk_File_Destination_Options;
    //    Export_Options.ExportDestinationType = ExportDestinationType.DiskFile;
    //    Export_Options.ExportFormatType = ExportFormatType.PortableDocFormat;

    //    Reporte.Export(Export_Options);
    //    Reporte.PrintToPrinter(1, true, 0, 0);
    //}

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
                Convenios_Traslado_Dominio.P_Filtros_Dinamicos = Ope_Pre_Convenios_Traslados_Dominio.Campo_Cuenta_Predial_ID + "='" + Cuenta_Predial_ID + "' AND " + Ope_Pre_Convenios_Traslados_Dominio.Campo_Estatus + " IN ('ACTIVO','PENDIENTE','INCUMPLIDO')";
                //Convenios_Traslado_Dominio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                //Convenios_Traslado_Dominio.P_Estatus = "VIGENTE";
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
            //try
            //{
            //    Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            //    DataTable Dt_Cuentas_Predial;
            //    Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ", ";
            //    Cuentas_Predial.P_Campos_Dinamicos += Cat_Pre_Cuentas_Predial.Campo_Estatus;
            //    Cuentas_Predial.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            //    Cuentas_Predial.P_Estatus = Estatus;
            //    Cuentas_Predial.P_Filtros_Dinamicos = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Cuenta_Predial_ID + "' AND ";
            //    Cuentas_Predial.P_Filtros_Dinamicos += "(" + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'VIGENTE' OR " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'BLOQUEADA')";
            //    Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            //    if (Dt_Cuentas_Predial != null)
            //    {
            //        if (Dt_Cuentas_Predial.Rows.Count == 1)
            //        {
            //            Estatus_Valido = true;
            //        }
            //        else
            //        {
            //            Estatus_Valido = false;
            //        }
            //    }
            //    else
            //    {
            //        Estatus_Valido = false;
            //    }
            //}
            //catch (Exception ex)
            //{
            //    throw new Exception(ex.Message);
            //}
        }
        else
        {
            throw new Exception("Seleccione una Cuenta para Validar su Estatus");
        }
        return Estatus_Valido;
    }

    private DataTable Calcular_Impuestos()
    {
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Convenio = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        if (Grid_Contrarecibos_Pendientes.SelectedIndex > -1)
        {
            Convenio.P_No_Calculo = Hdf_No_Calculo.Value;
        }
        else if (Hdf_No_Calculo.Value != "")
        {
            Convenio.P_No_Calculo = Hdf_No_Calculo.Value;
        }
        else
        {
            Convenio.P_No_Calculo = HttpUtility.HtmlDecode(Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[10].Text).Trim();
        }
        DataTable Dt_Impuestos = Convenio.Consulta_Calculos_Convenio();
        return Dt_Impuestos;
    }

    private void Cargar_Descuentos()
    {
        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio_Descuentos = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
        Convenio_Descuentos.P_No_Calculo = Hdf_No_Calculo.Value;
        Convenio_Descuentos.P_Anio = Hdf_Anio.Value;
        DataTable Dt_Descuentos = Convenio_Descuentos.Consultar_Descuentos();
        if (Dt_Descuentos.Rows.Count > 0)
        {
            Txt_Descuento_Multas.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Multa].ToString()).ToString("0.00");
            Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDouble(Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_Desc_Recargo].ToString()).ToString("0.00");
            Hdf_No_Descuento.Value = Dt_Descuentos.Rows[0][Ope_Pre_Descuento_Der_Sup.Campo_No_Descuento].ToString();
        }
        else
        {
            Txt_Descuento_Multas.Text = "0.00";
            Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
        }
        Txt_Descuento_Recargos_Moratorios.Text = "0.00";
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    protected void Cmb_Estatus_Selected_Index_Changed(object sender, EventArgs e)
    {
        if (Btn_Modificar.AlternateText == "Actualizar" && Cmb_Estatus.SelectedValue == "ACTIVO")
        {
            Cargar_Descuentos();
        }
        else
        {
            Txt_Descuento_Multas.Text = Hdf_Desc_Multas.Value;
            Txt_Descuento_Recargos_Ordinarios.Text = Hdf_Desc_Recargos.Value;
            Calcular_Total_Descuento();
            Calcular_Sub_Total();
            Calcular_Total_Anticipo();
            Calcular_Total_Convenio();
            Calcular_Parcialidades();
        }
    }







    private void Cargar_Datos()
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                //KONSULTA DATOS CUENTA HACER DS
                Busqueda_Cuentas();
                //LLENAR CAJAS
                if (Session["Ds_Cuenta_Datos"] != null)
                {
                    Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
                    Busqueda_Propietario();
                }

            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio;

    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Ds_Cuenta = Resumen_Predio.Consulta_Datos_Cuenta_Generales();
            if (Ds_Cuenta.Tables[0].Rows.Count - 1 >= 0)
            {
                if (Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim() != string.Empty)
                {
                    Session["Cuenta_Predial_ID"] = Ds_Cuenta.Tables[0].Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
                }
            }
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
            }
            else
            {
                //Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
                //Lbl_Mensaje_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Generales_Cuenta
    ///DESCRIPCIÓN: asignar datos generales de cuenta a los controles y objeto de negocio
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            DataTable Dt_Ultimo_Movimiento = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ultimo_Movimiento();
            if (Dt_Ultimo_Movimiento.Rows.Count > 0)
            {
                if (Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString() != string.Empty)
                {
                    if (Dt_Ultimo_Movimiento.Rows[0]["descripcion"].ToString() != "APERTURA")
                    {
                        //Txt_Ultimo_Movimiento_General.Text = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
                    }
                }
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BLOQUEADA")
            {
                //Lbl_Estatus.Text = " BLOQUEADA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "PENDIENTE")
            {
                //Lbl_Estatus.Text = " Cuenta No Generada";
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('Cuenta No Generada')", true);

                //Bloquear_Controles();
            }
            //Txt_Cuenta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            //M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
                //Txt_Tipo_Periodo_Impuestos.Text = Dt_Tipo_Predio.Rows[0]["Descripcion"].ToString().Trim();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
                //Txt_Uso_Predio_General.Text = Dt_Uso_Suelo.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                //Txt_Estado_Predio_General.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Calle.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString() != string.Empty)
            {
                Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString() != string.Empty)
            {
                Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            }
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            //{
            //    //Txt_Estatus_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //    //M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            //}

            ////Txt_Supe_Construida_General.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();
            //M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            ////Txt_Super_Total_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
            //    DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
            //    //Txt_Calle.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            //    M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            //}
            ////Txt_Numero_Exterior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            //M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            ////Txt_Numero_Interior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            //M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            ////Txt_Clave_Catastral_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            //{
            //    //Txt_Efectos_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            //}
            ////Txt_Valor_Fiscal_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()));
            //M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            ////Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            ////Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            //{
            //    //Txt_Cuota_Anual_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()));
            //    //double Cuota_Bimestral = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6;
            //    //Txt_Cuota_Bimestral_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Cuota_Bimestral);
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
            //    M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            //}
            ////Txt_Porciento_Exencion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString();
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            //{
            //    M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null)
            //{
            //    if (dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
            //    {
            //        //Txt_Fecha_Avaluo_Impuestos.Text = "";
            //        M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
            //    }
            //    else
            //    {
            //        //Txt_Fecha_Avaluo_Impuestos.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim());
            //        M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null)
            //{
            //    if (dataTable.Rows[0]["Termino_Exencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
            //    {
            //        //Txt_Fecha_Termino_Extencion.Text = "";
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }
            //    else
            //    {
            //        //Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Termino_Exencion"].ToString().Trim());
            //        M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //    }

            //}
            ////Txt_Dif_Construccion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
            //M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();

            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            //{
            //    M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
            //    //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            //}
            ////Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            //{
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "NO" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "no" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "No")
            //    {
            //        //Chk_Cuota_Fija.Checked = false;
            //        M_Orden_Negocio.P_Cuota_Fija = "NO";
            //    }
            //    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
            //    {
            //        //Chk_Cuota_Fija.Checked = true;
            //        M_Orden_Negocio.P_Cuota_Fija = "SI";

            //        //----K4RG4R D47OZ D3 14 CU0T4 F1J4!!!!!
            //        if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
            //        {
            //            M_Orden_Negocio.P_No_Cuota_Fija = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString();
            //            //Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
            //        }
            //    }
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            //{
            //    M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
            //    //Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            //}

            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
            //    DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
            //    if (Dt_Tasa.Rows.Count > 0)
            //    {
            //        //Txt_Tasa_Impuestos.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
            //    }
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            //{
            //    //Cmb_Domicilio_Foraneo.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //    M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
            //    DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
            //    if (Dt_Estado_Propietario.Rows.Count > 0)
            //    {
            //        //Txt_Estado_Propietario.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
            //        M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
            //    }
            //}
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            //{
            //    //Txt_Estado_Propietario.Text = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            //}
            //if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            //{
            //    Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            //    DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
            //    //Txt_Ciudad_Propietario.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
            //    M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            //}
            //else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            //{
            //    //Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            //}
            //if (dataTable.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
            //{
            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_Notificacion"].ToString()))
            //    {
            //        //Txt_Colonia_Propietario.Text = dataTable.Rows[0]["Colonia_Notificacion"].ToString();
            //    }
            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_Notificacion"].ToString()))
            //    {
            //        //Txt_Calle_Propietario.Text = dataTable.Rows[0]["Calle_Notificacion"].ToString();
            //    }
            //}
            //else
            //{

            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
            //    {
            //        Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
            //        DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
            //        //Txt_Colonia_Propietario.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
            //    }

            //    if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
            //    {
            //        Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
            //        DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
            //        //Txt_Calle_Propietario.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
            //    }
            //}

            ////Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            ////Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            //M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            ////Txt_Cod_Postal_Propietario.Text = dataTable.Rows[0]["Codigo_Postal"].ToString();
            //M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();


            //M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            //M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
            ////Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios;
            ////if (Dt_Agregar_Co_Propietarios.Rows.Count - 1 >= 0)
            ////{
            ////    for (int x = 0; x <= Dt_Agregar_Co_Propietarios.Rows.Count - 1; x++)
            ////    {
            ////        if (Dt_Agregar_Co_Propietarios.Rows[0]["Tipo"].ToString().Trim() == "COPROPIETARIO")
            ////        {
            ////            Txt_Copropietarios_Propietario.Text += Dt_Agregar_Co_Propietarios.Rows[x]["Nombre_Contribuyente"].ToString().Trim() + " \t" + Dt_Agregar_Co_Propietarios.Rows[x]["Rfc"].ToString().Trim() + "\n";

            ////        }
            ////    }
            ////}
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }


    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Prop_Datos");
                Session["Ds_Prop_Datos"] = Ds_Prop;
                //Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }








    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Convenios_Fraccionamientos
    ///DESCRIPCIÓN          : Crea un DataTable con las columnas y datos del convenios de Fraccionamiento Seleccionado en el GridView
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 26/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Convenios_Traslado()
    {
        Ds_Convenio_Impuesto_Traslado_Dominio Ds_Convenios_TD = new Ds_Convenio_Impuesto_Traslado_Dominio();

        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenios_Td = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();

        //DataTable Dt_Impuestos_Fraccionamientos;
        DataTable Dt_Cuenta_Predial = null;
        //DataTable Dt_Tipo_Predio;
        DataTable Dt_Temp;
        DataTable Dt_Temp_Detalles = null;
        DataRow Dr_Convenios_Td;

        DataTable Dt_Traslado = Ds_Convenios_TD.Tables["Dt_Pre_Convenios_TD"];
        Convenios_Td.P_No_Convenio = Hdf_No_Convenio.Value;
        Convenios_Td.P_Mostrar_Convenio_Con_Reestructura = false;
        Dt_Temp = Convenios_Td.Consultar_Convenio_Traslado_Dominio();
        Dt_Temp_Detalles = Convenios_Td.P_Dt_Parcialidades;

        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        {
            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
            Dr_Convenios_Td = Dt_Traslado.NewRow();
            Dr_Convenios_Td["NO_CONVENIO"] = Hdf_No_Convenio.Value;
            Dr_Convenios_Td["REALIZO"] = Dr_Temp[Ope_Pre_Convenios_Traslados_Dominio.Campo_Realizo];
            Dr_Convenios_Td["SOLICITANTE"] = Dr_Temp[Ope_Pre_Convenios_Traslados_Dominio.Campo_Solicitante];
            Dt_Traslado.Rows.Add(Dr_Convenios_Td);
        }

        Dt_Traslado = Ds_Convenios_TD.Tables["Dt_Pre_Convenios_Det_TD"];
        foreach (DataRow Dr_Temp in Dt_Temp_Detalles.Rows)
        {
            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
            Dr_Convenios_Td = Dt_Traslado.NewRow();
            Dr_Convenios_Td["NO_PAGO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Pago].ToString();
            Dr_Convenios_Td["NO_CONVENIO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_No_Convenio].ToString();
            Dr_Convenios_Td["FECHA_PAGO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Fecha_Vencimiento];
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto].ToString().Equals(""))
            {
                Dr_Convenios_Td["MONTO_IMPUESTO"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["MONTO_IMPUESTO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Impuesto];
            }
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios].ToString().Equals(""))
            {
                Dr_Convenios_Td["RECARGOS_ORDINARIOS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["RECARGOS_ORDINARIOS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Ordinarios];
            }
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas].ToString().Equals(""))
            {
                Dr_Convenios_Td["MONTO_MULTAS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["MONTO_MULTAS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Monto_Multas];
            }

            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios].ToString().Equals(""))
            {
                Dr_Convenios_Td["RECARGOS_MORATORIOS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["RECARGOS_MORATORIOS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Traslados_Dominio.Campo_Recargos_Moratorios];
            }
            Dr_Convenios_Td["HONORARIOS"] = 0.00;
            if (Dr_Temp["MONTO_IMPORTE"].ToString().Equals(""))
            {
                Dr_Convenios_Td["MONTO_IMPORTE"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["MONTO_IMPORTE"] = Dr_Temp["MONTO_IMPORTE"];
            }
            if (Dr_Temp["CONSTANCIA"].ToString().Equals(""))
            {
                Dr_Convenios_Td["CONSTANCIA"] = 0.00;
            }
            else
            {
                Dr_Convenios_Td["CONSTANCIA"] = Dr_Temp["CONSTANCIA"];
            }
            Dt_Traslado.Rows.Add(Dr_Convenios_Td);
        }

        Dt_Traslado = Ds_Convenios_TD.Tables["Dt_Pre_Cuentas_Predial"];

        //Inserta los datos de los Impuestos de Traslado en la Tabla
        Dr_Convenios_Td = Dt_Traslado.NewRow();
        Dr_Convenios_Td["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Convenios_Td["DOMICILIO"] = "COL. " + Txt_Colonia.Text + ", CALLE " + Txt_Calle.Text + ", NO EXT " + Txt_No_Exterior.Text + ", NO INT. " + Txt_No_Interior.Text;
        Dt_Traslado.Rows.Add(Dr_Convenios_Td);
        return Ds_Convenios_TD;
    }

    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        if (Hdf_No_Convenio.Value != "")
        {
            if (Grid_Convenios_Impuestos_Traslado.SelectedRow.Cells[8].Text.Equals("PAGADO"))
            {
                //Imprimir_Reporte(Crear_Ds_Convenios_Traslado(), "Rpt_Convenio_TD.rpt", "Rpt_Convenio_Traslado_Dominio", "Impresion", "Convenio");
                Imprimir_Convenio();
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Convenio
    /// DESCRIPCIÓN: Generar convenio (con OpenXML SDK a partir de documento con controles de contenido)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Convenio()
    {
        string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Convenio_Traslado.docx");
        string Documento_Salida = Server.MapPath("../../Reporte/" + "Convenio_Traslado.docx");
        //Documento_Salida = Server.MapPath("~/Reporte/" + "Convenio_Der_Sup.docx");

        //create copy of template so that we don't overwrite it
        if (System.IO.File.Exists(Documento_Salida))
        {
            System.IO.File.Delete(Documento_Salida);
        }
        File.Copy(Ruta_Plantilla, Documento_Salida);

        ReportDocument Reporte = new ReportDocument();
        String Nombre_Archivo = "Convenio_Traslado.docx";
        String Tipo_Solicitante;
        String Calle_Numero;
        DateTime Fecha_Convenio;
        String PDF_Convenio = Nombre_Archivo + ".pdf";
        String Importe_Letra;
        string Periodicidad = "";

        // si no existe el directorio, crearlo
        if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
            System.IO.Directory.CreateDirectory("../../Reporte");

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // tipo de contribuyente
        if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
            Tipo_Solicitante = "CONTRIBUYENTE";
        else
            Tipo_Solicitante = Cmb_Tipo_Solicitante.SelectedValue;
        // formar ubicacion (calle, numero y si existe, numero interior)
        Calle_Numero = Txt_Calle.Text + " " + Txt_No_Exterior.Text + " ";
        if (Txt_No_Interior.Text != "")
            Calle_Numero += "INT " + Txt_No_Interior.Text + " ";
        if (!DateTime.TryParse(Txt_Fecha.Text, out Fecha_Convenio))
            Fecha_Convenio = DateTime.Now;

        // convertir el importe del convenio a letra
        Numalet Cantidad = new Numalet();
        Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
        Cantidad.SeparadorDecimalSalida = "Pesos";
        Cantidad.ApocoparUnoParteEntera = true;
        Cantidad.LetraCapital = true;
        Importe_Letra = Cantidad.ToCustomCardinal(Txt_Sub_Total.Text.Replace(",", ""));
        // periodicidad
        switch (Cmb_Periodicidad_Pago.SelectedValue)
        {
            case "7":
                Periodicidad = "PAGOS SEMANALES";
                break;
            case "14":
                Periodicidad = "PAGOS CATORCENALES";
                break;
            case "15":
                Periodicidad = "PAGOS QUINCENALES";
                break;
            case "Mensual":
                Periodicidad = "MENSUALIDADES";
                break;
            case "Anual":
                Periodicidad = "ANUALIDADES";
                break;
        }

        try
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
            {
                Int32 Parcialidades = 0;
                //create XML string matching custom XML part
                string newXml = "<root>"
                    + "<Folio>" + "CTRA" + Convert.ToInt32(Hdf_No_Convenio.Value) + "</Folio>"
                    + "<Nombre_Solicitante>" + Txt_Solicitante.Text.Trim().ToUpper() + "</Nombre_Solicitante>"
                    + "<Tipo_Solicitante>" + Tipo_Solicitante + "</Tipo_Solicitante>"
                    + "<Tipo_Solicitante2>" + Tipo_Solicitante + "</Tipo_Solicitante2>"
                    + "<Tipo_Solicitante3>" + Tipo_Solicitante + "</Tipo_Solicitante3>"
                    + "<rfc>" + Txt_RFC.Text + "</rfc>"
                    + "<Nombre_Titular>" + Txt_Propietario.Text.Trim().ToUpper() + "</Nombre_Titular>"
                    + "<Calle_Numero>" + Calle_Numero + "</Calle_Numero>"
                    + "<Nombre_Colonia>" + Txt_Colonia.Text + "</Nombre_Colonia>"
                    + "<Cuenta_Predial>" + Txt_Cuenta_Predial.Text + "</Cuenta_Predial>"
                    + "<Cantidad_Letra>" + Importe_Letra + "</Cantidad_Letra>"
                    + "<Cantidad_Numero>" + Txt_Sub_Total.Text + "</Cantidad_Numero>"
                    + "<Numero_Parcialidades>" + Txt_Numero_Parcialidades.Text + "</Numero_Parcialidades>"
                    + "<Periodicidad>" + Periodicidad + "</Periodicidad>"
                    + "<Dia_Del_Mes>" + Fecha_Convenio.Day.ToString() + "</Dia_Del_Mes>"
                    + "<Mes>" + Fecha_Convenio.ToString("MMMM").ToUpper() + "</Mes>"
                    + "<Anio>" + Fecha_Convenio.Year.ToString() + "</Anio>"
                    + "</root>";

                MainDocumentPart main = doc.MainDocumentPart;
                main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                //add and write new XML part
                CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                //*** openXML_Wp = DocumentFormat.OpenXml.Wordprocessing
                // localizar la etiqueta del control de contenido en la que esta la tabla
                openXML_Wp.SdtBlock ccWithTable = main.Document.Body.Descendants<openXML_Wp.SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<openXML_Wp.Tag>().Val == "TABLA_PARCIALIDADES").Single();
                // Localizar la tabla
                openXML_Wp.Table Tabla_Parcialidades = ccWithTable.Descendants<openXML_Wp.Table>().Single();
                // localizar la ultima fila de la tabla
                openXML_Wp.TableRow Fila_Vacia = Tabla_Parcialidades.Elements<openXML_Wp.TableRow>().Last();

                // estilo para los campos (crear un estilo y asignarle el id del estilo en el documento original de word)
                openXML_Wp.ParagraphProperties Propiedades_Parrafo = new openXML_Wp.ParagraphProperties();
                Propiedades_Parrafo.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "EstiloTablaParcialidades" };
                openXML_Wp.ParagraphProperties Prop_Montos_Parcialidades = new openXML_Wp.ParagraphProperties();
                Prop_Montos_Parcialidades.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "MontoTablaParcialidades" };
                openXML_Wp.ParagraphProperties Propiedades_Parrafo0;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo1;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo2;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo3;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo4;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo5;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo6;

                foreach (GridViewRow fila in Grid_Parcialidades.Rows)
                {
                    //if (fila.Cells[8].Text == "POR PAGAR")
                    //{
                    DateTime Fecha_Parcialidad;

                    // convertir la fecha
                    DateTime.TryParse(fila.Cells[8].Text, out Fecha_Parcialidad);

                    // copiar una fila nueva en la tabla
                    openXML_Wp.TableRow Nueva_Fila = (openXML_Wp.TableRow)Fila_Vacia.CloneNode(true);

                    // generar estilos para cada elemento (una vez enlazado no se puede agregar a otro, por eso se duplican con OuterXML)
                    Propiedades_Parrafo0 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                    Propiedades_Parrafo1 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                    Propiedades_Parrafo2 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo3 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo4 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo5 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo6 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);

                    // agregar datos a la nueva fila (el texto debe ir dentro de un elemento Run, para poderlo agregar en un elemento párrafo)
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(0).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(0).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo0,
                        new openXML_Wp.Run(new openXML_Wp.Text((++Parcialidades).ToString()))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(1).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(1).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo1,
                        new openXML_Wp.Run(new openXML_Wp.Text(Fecha_Parcialidad.ToString("dd-MMM-yyyy").ToUpper()))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(2).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(2).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo2,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[2].Text.Replace("$", "")))));

                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(3).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(3).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo3,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[3].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(4).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(4).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo4,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[4].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(5).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(5).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo5,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[6].Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(6).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(6).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo6,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[7].Text.Replace("$", "")))
                        ));

                    // agregar la nueva fila a la tabla
                    Tabla_Parcialidades.AppendChild(Nueva_Fila);

                    //} // si estatus == POR PAGAR
                }

                // copiar una fila nueva en la tabla para los totales
                openXML_Wp.TableRow Fila_Totales = (openXML_Wp.TableRow)Fila_Vacia.CloneNode(true);

                openXML_Wp.Paragraph Parrafo_Nuevo = new openXML_Wp.Paragraph();

                // cambiar estilo para totales de la tabla
                Propiedades_Parrafo.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "EstiloEncabezadoParcialidades" };
                Prop_Montos_Parcialidades.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "MontoEncabezadoParcialidades" };
                Propiedades_Parrafo0 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                Propiedades_Parrafo1 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                Propiedades_Parrafo2 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo3 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo4 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo5 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo6 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);

                // agregar datos a la fila de totales
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(0).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(0).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo0,
                    new openXML_Wp.Run(new openXML_Wp.Text(""))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(1).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(1).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo1,
                    new openXML_Wp.Run(new openXML_Wp.Text("TOTALES"))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(2).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(2).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo2,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[2].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(3).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(3).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo3,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[3].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(4).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(4).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo4,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[4].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(5).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(5).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo5,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[6].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(6).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(6).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo6,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[7].Text.Replace("$", "")))
                    ));

                // agregar la nueva fila a la tabla
                Tabla_Parcialidades.AppendChild(Fila_Totales);

                // eliminar la fila vacia que se tomo como base
                Tabla_Parcialidades.RemoveChild(Fila_Vacia);
                using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                {
                    ts.Write(newXml);
                }
                // guardar los cambios en el documento
                main.Document.Save();

                //closing WordprocessingDocument automatically saves the document
            }

            //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
            //// ofrecer para descarga
            //Response.Clear();
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/x-msword";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
            ////           'Visualiza el archivo
            //Response.WriteFile(Ruta);
            //Response.Flush();
            //Response.Close();

            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            Pagina = Pagina + "Convenio_Traslado.docx";
            ScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato_Convenio",
                "window.open('" + Pagina +
                "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Imprimir convenio: " + Ex.Message);
        }
    }


    private void Cambiar_Convenio_Incumplido()
    {
        DateTime Fecha_Vencimiento;
        DateTime Fecha_Actual;
        Fecha_Actual = DateTime.Now;
        foreach (GridViewRow Renglon_Actual in Grid_Parcialidades.Rows)
        {
            if (Renglon_Actual.Cells[9].Text == "POR PAGAR")
            {
                Fecha_Vencimiento = Convert.ToDateTime(Renglon_Actual.Cells[8].Text).AddDays(1);
                if (DateTime.Compare(Fecha_Actual, Fecha_Vencimiento) == 1)
                {
                    if (Renglon_Actual.Cells[0].Text != "1")
                    {
                        Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio Convenio_Tras = new Cls_Ope_Pre_Convenios_Impuestos_Traslado_Dominio_Negocio();
                        Convenio_Tras.P_No_Convenio = Hdf_No_Convenio.Value;
                        Convenio_Tras.Convenio_Incumplido();
                        Cargar_Convenio();
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('Convenio incumplido');", true);
                    }
                    else
                    {
                        ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Traslado de dominio", "alert('El convenio se encuentra vencido, sin embargo aún puede modificarlo.');", true);
                    }
                }
                return;
            }
        }
    }

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
    private String Obtener_Dato_Consulta(String Consulta)
    {
        String Dato_Consulta = "";

        try
        {
            OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Consulta);

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
}
