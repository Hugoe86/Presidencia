using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Convenios_Fraccionamientos.Negocio;
using Presidencia.Operacion_Predial_Impuestos_Fraccionamientos.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Calles.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

public partial class paginas_Predial_Ventanas_Emergentes_Convenios_Frm_Convenios_Fraccionamiento : System.Web.UI.Page
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
        string Ventana_Modal;
        if (!IsPostBack)
        {
            Configuracion_Formulario(true);
            Hdf_Cuenta_Predial_ID.Value = Request.QueryString["Cuenta_Predial_ID"].ToString();// Session["CFRA_CUENTA_PREDIAL_ID"].ToString();
            Hdf_No_Convenio.Value = Request.QueryString["No_Convenio"].ToString();// Session["CFRA_NO_CONVENIO"].ToString();
            Hdf_No_Impuesto_Fraccionamiento.Value = Request.QueryString["No_Impuesto_Fraccionamiento"].ToString();// Session["CFRA_NO_IMPUESTO_FRACCIONAMIENTO"].ToString();
            Consultar_Datos_Cuenta_Predial();
            Cargar_Convenio();
            Txt_Cuenta_Predial_TextChanged();

            Session.Remove("ESTATUS_CUENTAS");
            Session.Remove("TIPO_CONTRIBUYENTE");
        }
        Div_Contenedor_Msj_Error.Visible = false;
    }

    #endregion

    private String Boton_Pulsado = "";

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
        Txt_Tipo_Fraccionamiento.Enabled = false;
        Txt_Monto_Recargos.Enabled = false;
        Txt_Monto_Multas.Enabled = false;
        Txt_Realizo_Calculo.Enabled = false;
        Txt_Fecha_Calculo.Enabled = false;
        Txt_Calle.Enabled = false;
        Txt_Colonia.Enabled = false;
        Txt_No_Exterior.Enabled = false;
        Txt_No_Interior.Enabled = false;
        Txt_Propietario.Enabled = false;
        //Convenio
        Txt_Numero_Convenio.Enabled = false;
        Txt_Realizo.Enabled = false;
        Txt_Fecha_Convenio.Enabled = false;
        Cmb_Tipo_Solicitante.Enabled = !Estatus;
        Cmb_Periodicidad_Pago.Enabled = !Estatus;
        Txt_Numero_Parcialidades.Enabled = !Estatus;
        Txt_Observaciones.Enabled = !Estatus;
        //Descuentos
        Txt_Total_Adeudo.Enabled = false;
        Txt_Total_Descuento.Enabled = false;
        Txt_Sub_Total.Enabled = false;
        Txt_Total_Convenio.Enabled = false;
        Txt_Descuento_Recargos_Ordinarios.Enabled = !Estatus;
        Txt_Descuento_Recargos_Moratorios.Enabled = !Estatus;
        Txt_Descuento_Multas.Enabled = !Estatus;
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
            //Convenio
            Cmb_Estatus.Enabled = false;
        }
        else
        {
            if (Boton_Pulsado == "Btn_Modificar")
            {
                //Convenio
                Cmb_Estatus.Enabled = !Estatus;
            }
        }
        //Parcialidades
        Txt_Porcentaje_Anticipo.Enabled = !Estatus;
        Txt_Total_Anticipo.Enabled = !Estatus;

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
        Txt_Honorarios.Style["text-align"] = "right";

        Txt_Fecha_Vencimiento.Enabled = false;
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
        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_Propietario_ID.Value = "";
        Hdf_No_Impuesto_Fraccionamiento.Value = null;
        //Datos Cuenta
        Txt_Cuenta_Predial.Text = "";
        Txt_Monto_Impuesto.Text = "";
        Txt_Tipo_Fraccionamiento.Text = "";
        Txt_Monto_Recargos.Text = "";
        Txt_Monto_Multas.Text = "";
        Txt_Realizo_Calculo.Text = "";
        Txt_Fecha_Calculo.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Propietario.Text = "";
        Txt_Fecha_Vencimiento.Enabled = false;
        Txt_Honorarios.Enabled = false;
        //Convenio
        Txt_Numero_Convenio.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Solicitante.Text = "";
        Txt_RFC.Text = "";
        Cmb_Tipo_Solicitante.SelectedIndex = 0;
        Txt_Numero_Parcialidades.Text = "";
        Cmb_Periodicidad_Pago.SelectedIndex = 0;
        Txt_Realizo.Text = "";
        Txt_Fecha_Convenio.Text = "";
        Txt_Observaciones.Text = "";
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
        Txt_Fecha_Vencimiento.Text = "";
        Session["Cuenta_Predial"] = null;
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
            Dr_Parcialidades["MONTO_MULTAS"] = Convert.ToDouble(Row.Cells[2].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Convert.ToDouble(Row.Cells[3].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_MORATORIOS"] = Convert.ToDouble(Row.Cells[4].Text.Replace("$", ""));
            Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDouble(Row.Cells[5].Text.Replace("$", ""));
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Convert.ToDateTime(Row.Cells[7].Text);
            Dr_Parcialidades["ESTATUS"] = Row.Cells[8].Text;
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
        }
        return Dt_Parcialidades;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Convenio
    ///DESCRIPCIÓN          : Llena la tabla de Convenios por Derechos de Supervisión con una consulta que puede o no tener Filtros.
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Convenio()
    {
        try
        {
            Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenios_Fraccionamientos = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
            DataTable Dt_Convenios_Fraccionamientos;

            Convenios_Fraccionamientos.P_Campos_Foraneos = true;
            Convenios_Fraccionamientos.P_No_Convenio = Hdf_No_Convenio.Value;
            Dt_Convenios_Fraccionamientos = Convenios_Fraccionamientos.Consultar_Convenio_Fraccionamiento();

            if (Dt_Convenios_Fraccionamientos != null)
            {
                if (Dt_Convenios_Fraccionamientos.Rows.Count > 0)
                {
                    foreach (DataRow Row in Dt_Convenios_Fraccionamientos.Rows)
                    {
                        Hdf_Cuenta_Predial_ID.Value = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID].ToString();
                        Hdf_Propietario_ID.Value = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID].ToString();
                        Txt_Cuenta_Predial.Text = Row["Cuenta_Predial"].ToString();
                        //Txt_Clasificacion.Text = Row["Tipo_Predio"].ToString();
                        Consultar_Datos_Cuenta_Predial();
                        Txt_Numero_Convenio.Text = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio].ToString();
                        Cmb_Estatus.SelectedValue = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Estatus].ToString();
                        if (Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID] != null
                            && Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID].ToString() != "")
                        {
                            Txt_Solicitante.Text = Row["Nombre_Propietario"].ToString();
                            Cmb_Tipo_Solicitante.SelectedIndex = 0;
                        }
                        else
                        {
                            if (Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante].ToString() != "")
                            {
                                Txt_Solicitante.Text = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante].ToString();
                                Txt_RFC.Text = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_RFC].ToString();
                                Cmb_Tipo_Solicitante.SelectedIndex = 1;
                            }
                        }
                        Txt_Numero_Parcialidades.Text = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Numero_Parcialidades].ToString();
                        Cmb_Periodicidad_Pago.SelectedValue = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Periodicidad_Pago].ToString();
                        Txt_Realizo.Text = Row["Nombre_Realizo"].ToString();
                        Txt_Fecha_Convenio.Text = Convert.ToDateTime(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
                        Txt_Observaciones.Text = Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Observaciones].ToString();
                        Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Ordinarios]).ToString("###,###,##0.00");
                        Txt_Descuento_Recargos_Moratorios.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Recargos_Moratorios]).ToString("###,###,##0.00");
                        Txt_Descuento_Multas.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Descuento_Multas]).ToString("###,###,##0.00");
                        Txt_Total_Adeudo.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Adeudo]).ToString("###,###,##0.00");
                        Txt_Total_Descuento.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Descuento]).ToString("###,###,##0.00");
                        Txt_Sub_Total.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Sub_Total]).ToString("###,###,##0.00");
                        Txt_Porcentaje_Anticipo.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Porcentaje_Anticipo]).ToString("###,###,##0.00");
                        Txt_Total_Anticipo.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Anticipo]).ToString("###,###,##0.00");
                        Txt_Total_Convenio.Text = Convert.ToDouble(Row[Ope_Pre_Convenios_Fraccionamientos.Campo_Total_Convenio]).ToString("###,###,##0.00");
                        Grid_Parcialidades.DataSource = Convenios_Fraccionamientos.P_Dt_Parcialidades;
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
        //if (Btn_Nuevo.AlternateText != "Dar de Alta" && Btn_Modificar.AlternateText == "Actualizar")
        //{
        //    if (Txt_Observaciones.Text.Equals(""))
        //    {
        //        if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //        Mensaje_Error = Mensaje_Error + "+ Introduzca las Observaciones.";
        //        Validacion = false;
        //    }
        //}
        if (!(Convert.ToDouble(Txt_Total_Anticipo.Text) >= (Convert.ToDouble(Txt_Monto_Multas.Text)+Convert.ToDouble(Txt_Honorarios.Text))))
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ El anticipo debe cubrir las multas y los honorarios.";
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
    ///NOMBRE DE LA FUNCIÓN : Validar_Estatus_Parcialidades
    ///DESCRIPCIÓN          : Itera el grid de Parcialidades para devolver un True cuando encuentre un estatus con el valor indicado en el parámetro.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 23/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Estatus_Parcialidades(String Tipo_Estatus)
    {
        Boolean Validado = false;
        foreach (GridViewRow Fila_Grid in Grid_Parcialidades.Rows)
        {
            if (Fila_Grid.Cells[3].Text == Tipo_Estatus)
            {
                Validado = true;
                break;
            }
        }
        return Validado;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Fecha_Vencimiento
    ///DESCRIPCIÓN          : Determina que la fecha de Vencimiento de Convenio no se halla alcanzado
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 23/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Fecha_Vencimiento()
    {
        DateTime Fecha_Vencimiento = Convert.ToDateTime(DateTime.Now.ToShortDateString());
        TimeSpan Ts_Temp;
        DateTime Dt_Temp;
        if (Txt_Fecha_Vencimiento.Text.Trim() != "")
        {
            Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
            
        }
        Ts_Temp = DateTime.Now - Fecha_Vencimiento;
        Dt_Temp = DateTime.MinValue + Ts_Temp;

        return (Dt_Temp.Month - 1) < 0;
    }

    #endregion

    #endregion

    #region Grids

    #endregion

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Insertar_Pasivo
    ///DESCRIPCIÓN          : Consulta el Costo del Documento y lo Inserta en Pasivo
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 05/Septiembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
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
                Calculo_Impuesto_Traslado.P_Descripcion = "COBRO POR CONVENIO DE FRACCIONAMIENTO DE LA CUENTA PREDIAL "+Txt_Cuenta_Predial.Text;
                Calculo_Impuesto_Traslado.P_Estatus = "POR PAGAR";
                Calculo_Impuesto_Traslado.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                Calculo_Impuesto_Traslado.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                Calculo_Impuesto_Traslado.P_Monto_Total_Pagar =""+Convert.ToDouble(Grid_Parcialidades.Rows[0].Cells[6].Text.Replace("$", ""));
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
                Hdf_No_Impuesto_Fraccionamiento.Value = "";
                Txt_Cuenta_Predial_TextChanged();
            }
            Consultar_Datos_Cuenta_Predial();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
        //Session.Remove("CUENTA_PREDIAL");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Predial
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
                //if (!Validar_Existe_Convenio_Activo("", Hdf_Cuenta_Predial_ID.Value))
                //{
                    //Pone los datos de la cuenta y los Impuestos
                    Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio Impuestos_Fraccionamientos = new Cls_Ope_Pre_Impuestos_Fraccionamientos_Negocio();
                    DataTable Dt_Impuestos_Derechos_Supervision;
                    DataTable Dt_Dettales_Impuestos_Derechos_Supervision;
                    Double Sum_Importes = 0;
                Double Sum_Multas = 0;
                Double Sum_Honorarios = 0;
                    Double Sum_Recargos = 0;
                    Double Sum_Totales = 0;

                    Impuestos_Fraccionamientos.P_Campos_Dinamicos = Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo + ", " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo + ", " + Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento;
                    Impuestos_Fraccionamientos.P_No_Impuesto_Fraccionamiento = Hdf_No_Impuesto_Fraccionamiento.Value;
                    Impuestos_Fraccionamientos.P_Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                    Impuestos_Fraccionamientos.P_Estatus = "POR PAGAR";
                    Impuestos_Fraccionamientos.P_Ordenar_Dinamico = Ope_Pre_Impuestos_Fraccionamientos.Campo_No_Impuesto_Fraccionamiento + " DESC";
                    Impuestos_Fraccionamientos.P_Campos_Sumados = true;
                    Dt_Impuestos_Derechos_Supervision = Impuestos_Fraccionamientos.Consultar_Impuestos_Fraccionamiento();
                    Dt_Dettales_Impuestos_Derechos_Supervision = Impuestos_Fraccionamientos.P_Dt_Detalles_Impuestos_Fraccionamiento;

                    if (Dt_Dettales_Impuestos_Derechos_Supervision.Rows.Count > 0)
                    {
                        Txt_Tipo_Fraccionamiento.Text = Dt_Cuentas_Predial.Rows[0]["DESCRIPCION_TIPO_PREDIO"].ToString();
                        if (Dt_Impuestos_Derechos_Supervision != null)
                        {
                            if (Dt_Impuestos_Derechos_Supervision.Rows.Count > 0)
                            {
                                Txt_Realizo_Calculo.Text = Dt_Impuestos_Derechos_Supervision.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Usuario_Creo].ToString();
                                Txt_Fecha_Calculo.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Impuestos_Derechos_Supervision.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Creo].ToString()));
                                Txt_Fecha_Vencimiento.Text = String.Format("{0:dd/MMM/yyyy}", Convert.ToDateTime(Dt_Impuestos_Derechos_Supervision.Rows[0][Ope_Pre_Impuestos_Fraccionamientos.Campo_Fecha_Vencimiento].ToString()));
                            }
                        }

                        foreach (DataRow Row in Dt_Dettales_Impuestos_Derechos_Supervision.Rows)
                        {
                            if (Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe] != null
                            && Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe].ToString() != "")
                        {
                            Sum_Importes += Convert.ToDouble(Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Importe].ToString());
                        }
                        else
                        {
                            Sum_Importes += 0.00;
                        }
                        if (Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos] != null
                            && Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos].ToString() != "")
                        {
                            Sum_Recargos += Convert.ToDouble(Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Recargos].ToString());
                        }
                        else
                        {
                            Sum_Recargos += 0.00;
                        }
                        if (Row[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Monto] != null
                            && Row[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Monto].ToString() != "")
                        {
                            Sum_Multas += Convert.ToDouble(Row[Cat_Pre_Multas_Fraccionamientos_Detalles.Campo_Monto].ToString());
                        }
                        else
                        {
                            Sum_Multas += 0.00;
                        }
                        if (Row["HONORARIOS"] != null
                            && Row["HONORARIOS"].ToString() != "")
                        {
                            Sum_Honorarios += Convert.ToDouble(Row["HONORARIOS"].ToString());
                        }
                        else
                        {
                            Sum_Honorarios += 0.00;
                        }
                        if (Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total] != null
                            && Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total].ToString() != "")
                        {
                            Sum_Totales += Convert.ToDouble(Row[Ope_Pre_Detalles_Impuestos_Fraccionamientos.Campo_Total].ToString());
                        }
                        else
                        {
                            Sum_Totales += 0.00;
                        }
                        }

                        Txt_Monto_Impuesto.Text = Sum_Importes.ToString("###,###,##0.00");
                    Txt_Monto_Recargos.Text = Sum_Recargos.ToString("###,###,##0.00");
                    Txt_Monto_Multas.Text = Sum_Multas.ToString("###,###,##0.00");
                    Txt_Honorarios.Text = Sum_Honorarios.ToString("###,###,##0.00");

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
                    else
                    {
                        Txt_Monto_Impuesto.Text = "0.00";
                        Txt_Monto_Multas.Text = "0.00";
                        Txt_Monto_Recargos.Text = "0.00";
                        Txt_Honorarios.Text = "0.00";
                        Txt_Realizo_Calculo.Text = "----------";
                        Txt_Tipo_Fraccionamiento.Text = "----------";
                        Calcular_Total_Adeudos();
                        Calcular_Total_Descuento();
                        Calcular_Sub_Total();
                        Calcular_Total_Convenio();
                        Calcular_Parcialidades();
                        Calcular_Total_Anticipo();
                        Txt_Fecha_Calculo.Text = "" + String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                        Txt_Realizo.Text = "" + Cls_Sessiones.Nombre_Empleado;
                    }
                //}
                //else
                //{
                //    ScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios por Fraccionamientos", "alert('Ya existe un Número de Convenio Activo para esta Cuenta');", true);
                //    Btn_Salir_Click(null, null);
                //}
            }
        }
    }

    private double Calcular_Total_Adeudos()
    {
        Double Monto_Impuesto = 0;
        Double Monto_Ordinarios = 0;
        Double Monto_Multas = 0;
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
            Monto_Ordinarios = Convert.ToDouble(Txt_Monto_Recargos.Text);
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
        Total_Adeudo = Monto_Impuesto + Monto_Ordinarios + Monto_Multas;
        Txt_Total_Adeudo.Text = Total_Adeudo.ToString("###,###,##0.00");

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
        Txt_Sub_Total.Text = Sub_Total.ToString("###,###,##0.00");

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
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("###,###,###,##0.00");
        Total_Convenio = Sub_Total - Total_Anticipo;
        Txt_Total_Convenio.Text = Total_Convenio.ToString("###,###,##0.00");

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
        Calcular_Parcialidades();
        Calcular_Total_Anticipo();
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
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();        
    }

    private void Calcular_Total_Descuento()
    {
        Double Monto_Ordinarios = 0.0;
        Double Monto_Multas = 0.0;
        Double Descuento_Recargos_Ordinarios = 0.0;
        Double Descuento_Recargos_Moratorios = 0.0;
        Double Descuento_Recargos_Multa = 0.0;
        Double Total_Descuento = 0.0;

        if (Txt_Monto_Recargos.Text.Trim() != "")
        {
            Monto_Ordinarios = Convert.ToDouble(Txt_Monto_Recargos.Text.Trim());
        }
        else
        {
            Txt_Monto_Recargos.Text = "0.00";
        }
        if (Txt_Monto_Multas.Text.Trim() != "")
        {
            Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text.Trim());
        }
        else
        {
            Txt_Monto_Multas.Text = "0.00";
        }
        if (Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "") != ""
            && Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "") != ".")
        {
            Descuento_Recargos_Ordinarios = Monto_Ordinarios * (Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("_", "").Replace(",", "")) / 100);
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
            Descuento_Recargos_Multa = Monto_Multas * (Convert.ToDouble(Txt_Descuento_Multas.Text.Trim().Replace("_", "").Replace(",", "")) / 100);
        }
        else
        {
            Txt_Descuento_Multas.Text = "0.00";
        }

        Total_Descuento = Descuento_Recargos_Ordinarios + Descuento_Recargos_Moratorios + Descuento_Recargos_Multa;
        Txt_Total_Descuento.Text = Total_Descuento.ToString("###,###,##0.00");
    }

    //[System.Web.Services.WebMethod]
    //public static void WM_Calcular_Parcialidades()
    //{
    //    paginas_Predial_Frm_Ope_Pre_Convenios_Derechos_Supervision Convenios_Derechos_Supervision = new paginas_Predial_Frm_Ope_Pre_Convenios_Derechos_Supervision();
    //    Convenios_Derechos_Supervision.Calcular_Parcialidades();
    //}

    //private void Calcular_Parcialidades()
    //{
    //    Int32 Cont_Parcialidades;
    //    Int32 Cant_Parcialidades = 0;
    //    DataTable Dt_Parcialidades = new DataTable();
    //    Double Monto_Impuesto = 0;
    //    Double Monto_Recargos = 0;
    //    Double Monto_Multas = 0;
    //    Double Sub_Total_Adeudo = 0;
    //    Double Total_Convenio = 0;
    //    Double Monto_Parcialidades = 0;
    //    //Double Temp_Monto_Parcialidades = 0;
    //    //Double Ajuste_Monto = 0;
    //    String Dias_Periodo = "";
    //    Double Temp_Monto_Impuesto = 0;
    //    Double Temp_Monto_Recargos = 0;
    //    Double Temp_Monto_Multas = 0;
    //    Double Total_Anticipo = 0;
    //    Double Total_Importe_Parcialidad = 0;
    //    Double Monto_Importe = 0;
    //    Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

    //    if (Txt_Numero_Parcialidades.Text.Trim() != "")
    //    {
    //        Cant_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);

    //        if (Txt_Monto_Impuesto.Text.Trim() != "")
    //        {
    //            Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
    //        }

    //        if (Txt_Monto_Recargos.Text.Trim() != "")
    //        {
    //            Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text);
    //        }

    //        if (Txt_Monto_Multas.Text.Trim() != "")
    //        {
    //            Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text);
    //        }

    //        if (Txt_Sub_Total.Text.Trim() != "")
    //        {
    //            Sub_Total_Adeudo = Convert.ToDouble(Txt_Sub_Total.Text);
    //        }

    //        if (Txt_Total_Anticipo.Text.Trim() != "")
    //        {
    //            Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
    //        }

    //        if (Txt_Total_Convenio.Text.Trim() != "")
    //        {
    //            Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
    //        }

    //        //if (Txt_Numero_Parcialidades.Text.Trim() != "")
    //        //{
    //        //    Monto_Parcialidades = Total_Convenio / Cant_Parcialidades;

    //        //    Temp_Monto_Parcialidades = Convert.ToDouble(String.Format("{0:n2}", Monto_Parcialidades));
    //        //    Ajuste_Monto = Convert.ToDouble(String.Format("{0:n2}", (Temp_Monto_Parcialidades * Cant_Parcialidades) - Total_Convenio));
    //        //}

    //        if (Cmb_Periodicidad_Pago.SelectedIndex > 0)
    //        {
    //            Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
    //        }

    //        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_MULTAS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));

    //        Temp_Monto_Impuesto = Monto_Impuesto;
    //        Temp_Monto_Multas = Monto_Multas;
    //        Temp_Monto_Recargos = Monto_Recargos;

    //        DataRow Dr_Parcialidades;
    //        for (Cont_Parcialidades = 0; Cont_Parcialidades < Cant_Parcialidades; Cont_Parcialidades++)
    //        {
    //            Total_Importe_Parcialidad = 0;

    //            if (Temp_Monto_Multas + Temp_Monto_Recargos != 0)
    //            {
    //                Monto_Importe = Total_Anticipo;
    //            }
    //            else
    //            {
    //                if (Monto_Parcialidades == 0)
    //                {
    //                    Monto_Parcialidades = Total_Convenio / (Cant_Parcialidades - (Cont_Parcialidades));
    //                }
    //                Monto_Importe = Monto_Parcialidades;
    //            }

    //            Dr_Parcialidades = Dt_Parcialidades.NewRow();
    //            Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;

    //            if (Monto_Importe - Total_Importe_Parcialidad >= Temp_Monto_Multas && Temp_Monto_Multas != 0)
    //            {
    //                Dr_Parcialidades["MONTO_MULTAS"] = Temp_Monto_Multas;
    //                Total_Importe_Parcialidad += Temp_Monto_Multas;
    //                Temp_Monto_Multas = 0;
    //            }
    //            else
    //            {
    //                if (Monto_Importe != Total_Importe_Parcialidad && Temp_Monto_Multas != 0)
    //                {
    //                    Dr_Parcialidades["MONTO_MULTAS"] = Monto_Importe - Total_Importe_Parcialidad;
    //                    Temp_Monto_Multas -= Monto_Importe - Total_Importe_Parcialidad;
    //                    Total_Importe_Parcialidad += Monto_Importe - Total_Importe_Parcialidad;
    //                }
    //                else
    //                {
    //                    Dr_Parcialidades["MONTO_MULTAS"] = 0;
    //                }
    //            }
    //            if (Monto_Importe - Total_Importe_Parcialidad >= Temp_Monto_Recargos && Temp_Monto_Recargos != 0)
    //            {
    //                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Temp_Monto_Recargos;
    //                Total_Importe_Parcialidad += Temp_Monto_Recargos;
    //                Temp_Monto_Recargos = 0;
    //            }
    //            else
    //            {
    //                if (Monto_Importe != Total_Importe_Parcialidad && Temp_Monto_Recargos != 0)
    //                {
    //                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Importe - Total_Importe_Parcialidad;
    //                    Temp_Monto_Recargos -= Monto_Importe - Total_Importe_Parcialidad;
    //                    Total_Importe_Parcialidad += Monto_Importe - Total_Importe_Parcialidad;
    //                }
    //                else
    //                {
    //                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = 0;
    //                }
    //            }
    //            Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
    //            if (Monto_Importe - Total_Importe_Parcialidad >= Temp_Monto_Impuesto && Temp_Monto_Impuesto != 0)
    //            {
    //                Dr_Parcialidades["MONTO_IMPUESTO"] = Temp_Monto_Impuesto;
    //                Total_Importe_Parcialidad += Temp_Monto_Impuesto;
    //                Temp_Monto_Impuesto = 0;
    //            }
    //            else
    //            {
    //                if (Monto_Importe != Total_Importe_Parcialidad && Temp_Monto_Impuesto != 0)
    //                {
    //                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Importe - Total_Importe_Parcialidad;
    //                    Temp_Monto_Impuesto -= Monto_Importe - Total_Importe_Parcialidad;
    //                    Total_Importe_Parcialidad += Monto_Importe - Total_Importe_Parcialidad;
    //                }
    //                else
    //                {
    //                    Dr_Parcialidades["MONTO_IMPUESTO"] = 0;
    //                }
    //            }

    //            //if (Cont_Parcialidades + 1 == Cant_Parcialidades)
    //            //{
    //            //    Dr_Parcialidades["MONTO"] = Monto_Parcialidades - Ajuste_Monto;
    //            //}
    //            //else
    //            //{
    //            //    Dr_Parcialidades["MONTO"] = Monto_Parcialidades;
    //            //}
    //            Dr_Parcialidades["MONTO_IMPORTE"] = Total_Importe_Parcialidad;
    //            Dr_Parcialidades["HONORARIOS"] = 0.00;
    //            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
    //            Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
    //            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
    //        }

    //        Grid_Parcialidades.DataSource = Dt_Parcialidades;
    //        Grid_Parcialidades.PageIndex = 0;
    //        Grid_Parcialidades.DataBind();

    //        Sumar_Totales_Parcialidades();
    //    }
    //}


    //private void Calcular_Parcialidades()
    //{
    //    if (Txt_Numero_Parcialidades.Text != "" && Convert.ToInt32(Txt_Numero_Parcialidades.Text) > 0)
    //    {
    //        //Parcialidades
    //        Int32 Parcialidades;
    //        Parcialidades = 0;
    //        Int32 Cont_Parcialidades;
    //        Cont_Parcialidades = 0;
    //        //DataTable
    //        DataTable Dt_Parcialidades = new DataTable();
    //        DataRow Dr_Parcialidades;
    //        //Cantidades
    //        Double Monto_Impuesto = 0;
    //        Double Monto_Recargos = 0;
    //        Double Monto_Multas = 0;
    //        Double Monto_Honorarios = 0;
    //        Double Total_Convenio = 0;
    //        Double Monto_Importe = 0;
    //        //Monto de cada parcialidad de la 2 en adelante
    //        Double Monto_Parcialidades = 0;
    //        //Periodicidad
    //        String Dias_Periodo = "";
    //        //Cantidades para restar
    //        Double Ay_Monto_Impuesto = 0;
    //        Double Ay_Monto_Recargos = 0;
    //        Double Ay_Monto_Multas = 0;
    //        Double Ay_Monto_Honorarios = 0;
    //        Double Total_Anticipo = 0;
    //        Double Total_Importe_Parcialidad = 0;
    //        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

    //        if (Txt_Honorarios.Text != "")
    //        {
    //            Monto_Honorarios = Convert.ToDouble(Txt_Honorarios.Text);
    //        }
    //        if (Txt_Monto_Multas.Text != "")
    //        {
    //            Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text) - ((Convert.ToDouble(Txt_Descuento_Multas.Text) / 100) * Convert.ToDouble(Txt_Monto_Multas.Text));
    //        }
    //        if (Txt_Monto_Recargos.Text != "")
    //        {
    //            Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text) - ((Convert.ToDouble(Txt_Descuento_Recargos_Ordinarios.Text) / 100) * Convert.ToDouble(Txt_Monto_Recargos.Text));
    //        }
    //        if (Txt_Monto_Impuesto.Text != "")
    //        {
    //            Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
    //        }
    //        if (Txt_Numero_Parcialidades.Text != "")
    //        {
    //            Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
    //        }
    //        if (Txt_Total_Anticipo.Text != "")
    //        {
    //            Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
    //        }
    //        if (Txt_Total_Convenio.Text != "")
    //        {
    //            Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
    //        }
    //        if (Cmb_Periodicidad_Pago.SelectedIndex > 0)
    //        {
    //            Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
    //        }
    //        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_MULTAS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));

    //        Dr_Parcialidades = Dt_Parcialidades.NewRow();
    //        Dr_Parcialidades["NO_PAGO"] = 1;
    //        if (Monto_Honorarios <= Total_Anticipo)
    //        {
    //            Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
    //            Total_Anticipo = Total_Anticipo - Monto_Honorarios;
    //            Monto_Importe += Monto_Honorarios;
    //            Monto_Honorarios = 0;
    //        }
    //        else
    //        {
    //            Dr_Parcialidades["HONORARIOS"] = Total_Anticipo;
    //            Monto_Honorarios = Monto_Honorarios - Total_Anticipo;
    //            Monto_Importe += Total_Anticipo;
    //            Total_Anticipo = 0;
    //        }
    //        if (Monto_Multas <= Total_Anticipo)
    //        {
    //            Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
    //            Total_Anticipo = Total_Anticipo - Monto_Multas;
    //            Monto_Importe += Monto_Multas;
    //            Monto_Multas = 0;
    //        }
    //        else
    //        {
    //            Dr_Parcialidades["MONTO_MULTAS"] = Total_Anticipo;
    //            Monto_Multas = Monto_Multas - Total_Anticipo;
    //            Monto_Importe += Total_Anticipo;
    //            Total_Anticipo = 0;
    //        }
    //        if (Monto_Recargos <= Total_Anticipo)
    //        {
    //            Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
    //            Total_Anticipo = Total_Anticipo - Monto_Recargos;
    //            Monto_Importe += Monto_Recargos;
    //            Monto_Recargos = 0;
    //        }
    //        else
    //        {
    //            Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Total_Anticipo;
    //            Monto_Recargos = Monto_Recargos - Total_Anticipo;
    //            Monto_Importe += Total_Anticipo;
    //            Total_Anticipo = 0;
    //        }
    //        Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
    //        if (Monto_Impuesto <= Total_Anticipo)
    //        {
    //            Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
    //            Total_Anticipo = Total_Anticipo - Monto_Impuesto;
    //            Monto_Importe += Monto_Impuesto;
    //            Monto_Impuesto = 0;
    //        }
    //        else
    //        {
    //            Dr_Parcialidades["MONTO_IMPUESTO"] = Total_Anticipo;
    //            Monto_Impuesto = Monto_Impuesto - Total_Anticipo;
    //            Monto_Importe += Total_Anticipo;
    //            Total_Anticipo = 0;
    //        }

    //        Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
    //        Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
    //        Dr_Parcialidades["ESTATUS"] = "PENDIENTE";
    //        Dt_Parcialidades.Rows.Add(Dr_Parcialidades);

    //        Total_Importe_Parcialidad = Convert.ToDouble((Total_Convenio / (Parcialidades - 1)).ToString("0.00"));

    //        for (Cont_Parcialidades = 1; Cont_Parcialidades < Parcialidades; Cont_Parcialidades++)
    //        {
    //            Monto_Parcialidades = Total_Importe_Parcialidad;
    //            Monto_Importe = 0;
    //            Dr_Parcialidades = Dt_Parcialidades.NewRow();
    //            Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;
    //            if (Monto_Honorarios <= Monto_Parcialidades)
    //            {
    //                Dr_Parcialidades["HONORARIOS"] = Monto_Honorarios;
    //                Monto_Parcialidades = Monto_Parcialidades - Monto_Honorarios;
    //                Monto_Importe += Monto_Honorarios;
    //                Monto_Honorarios = 0;
    //            }
    //            else
    //            {
    //                Dr_Parcialidades["HONORARIOS"] = Monto_Parcialidades;
    //                Monto_Honorarios = Monto_Honorarios - Monto_Parcialidades;
    //                Monto_Importe += Monto_Parcialidades;
    //                Monto_Parcialidades = 0;
    //            }
    //            if (Monto_Multas <= Monto_Parcialidades)
    //            {
    //                Dr_Parcialidades["MONTO_MULTAS"] = Monto_Multas;
    //                Monto_Parcialidades = Monto_Parcialidades - Monto_Multas;
    //                Monto_Importe += Monto_Multas;
    //                Monto_Multas = 0;
    //            }
    //            else
    //            {
    //                Dr_Parcialidades["MONTO_MULTAS"] = Monto_Parcialidades;
    //                Monto_Multas = Monto_Multas - Monto_Parcialidades;
    //                Monto_Importe += Monto_Parcialidades;
    //                Monto_Parcialidades = 0;
    //            }
    //            if (Monto_Recargos <= Monto_Parcialidades)
    //            {
    //                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
    //                Monto_Parcialidades = Monto_Parcialidades - Monto_Recargos;
    //                Monto_Importe += Monto_Recargos;
    //                Monto_Recargos = 0;
    //            }
    //            else
    //            {
    //                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Parcialidades;
    //                Monto_Recargos = Monto_Recargos - Monto_Parcialidades;
    //                Monto_Importe += Monto_Parcialidades;
    //                Monto_Parcialidades = 0;
    //            }
    //            Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
    //            if (Monto_Impuesto <= Monto_Parcialidades)
    //            {
    //                Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
    //                Monto_Parcialidades = Monto_Parcialidades - Monto_Impuesto;
    //                Monto_Importe += Monto_Impuesto;
    //                Monto_Impuesto = 0;
    //            }
    //            else
    //            {
    //                Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Parcialidades;
    //                Monto_Impuesto = Monto_Impuesto - Monto_Parcialidades;
    //                Monto_Importe += Monto_Parcialidades;
    //                Monto_Parcialidades = 0;
    //            }

    //            Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
    //            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
    //            Dr_Parcialidades["ESTATUS"] = "PENDIENTE";
    //            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
    //        }
    //        Grid_Parcialidades.DataSource = Dt_Parcialidades;
    //        Grid_Parcialidades.PageIndex = 0;
    //        Grid_Parcialidades.DataBind();

    //        Sumar_Totales_Parcialidades();
    //    }
    //    else
    //    {
    //        Grid_Parcialidades.DataSource = null;
    //        Grid_Parcialidades.DataBind();
    //    }
    //}

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Parcialidades
    ///DESCRIPCIÓN          : Calcula las parcialidades
    ///PARAMETROS: 
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Calcular_Parcialidades()
    {
        if (Txt_Numero_Parcialidades.Text != "" && Convert.ToInt32(Txt_Numero_Parcialidades.Text) > 0 && Cmb_Periodicidad_Pago.SelectedIndex>0)
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
            //Monto de cada parcialidad de la 2 en adelante
            Double Monto_Parcialidades = 0;
            //Periodicidad
            String Dias_Periodo = "";
            //Cantidades para restar
            Double Ay_Monto_Impuesto = 0;
            Double Ay_Monto_Recargos = 0;
            Double Ay_Monto_Multas = 0;
            Double Ay_Monto_Honorarios = 0;
            Double Total_Anticipo = 0;
            Double Total_Importe_Parcialidad = 0;
            Double Monto_Final = 0;
            Double Total_Importe = 0;
            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();

            if (Txt_Honorarios.Text != "")
            {
                Monto_Honorarios = Convert.ToDouble(Txt_Honorarios.Text);
            }
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
            if (Cmb_Periodicidad_Pago.SelectedIndex > 0)
            {
                Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
            }
            Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
            Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
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
            //Total_Importe += Monto_Importe;
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
            Dr_Parcialidades["ESTATUS"] = "PENDIENTE";
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);

            Total_Importe_Parcialidad = Convert.ToDouble((Total_Convenio / (Parcialidades - 1)).ToString("0.00"));

            for (Cont_Parcialidades = 1; Cont_Parcialidades < Parcialidades; Cont_Parcialidades++)
            {
                if (Cont_Parcialidades == (Parcialidades - 1))
                {
                    Monto_Importe = 0;
                    Monto_Final = Total_Convenio - Total_Importe;
                    Dr_Parcialidades = Dt_Parcialidades.NewRow();
                    Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;
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
                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Final;
                    Monto_Importe += Monto_Final;
                    Total_Importe += Monto_Importe;
                    //if (Total_Importe > Total_Convenio)
                    //{
                    //    Monto_Final = Monto_Final - (Total_Importe - Total_Convenio);
                    //}
                    //else if (Total_Convenio > Total_Importe)
                    //{
                    //    Monto_Final = Monto_Final - (Total_Convenio - Total_Importe);
                    //}
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
                Monto_Parcialidades = Total_Importe_Parcialidad;
                Monto_Importe = 0;
                Dr_Parcialidades = Dt_Parcialidades.NewRow();
                Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;
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





    //private void Calcular_Parcialidades()
    //{
    //    Int32 Cont_Parcialidades;
    //    Int32 Cant_Parcialidades = 0;
    //    DataTable Dt_Parcialidades = new DataTable();
    //    Double Monto_Impuesto = 0;
    //    Double Monto_Recargos = 0;
    //    Double Monto_Multas = 0;
    //    Double Sub_Total_Adeudo = 0;
    //    Double Total_Convenio = 0;
    //    Double Monto_Parcialidades = 0;
    //    //Double Temp_Monto_Parcialidades = 0;
    //    //Double Ajuste_Monto = 0;
    //    String Dias_Periodo = "";
    //    Double Temp_Monto_Impuesto = 0;
    //    Double Temp_Monto_Recargos = 0;
    //    Double Temp_Monto_Multas = 0;
    //    Double Total_Anticipo = 0;
    //    Double Total_Importe_Parcialidad = 0;
    //    Double Monto_Importe = 0;
    //    Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();


    //    if (Txt_Numero_Parcialidades.Text.Trim() != "")
    //    {
    //        Cant_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);

    //        if (Txt_Monto_Impuesto.Text.Trim() != "")
    //        {
    //            Monto_Impuesto = Convert.ToDouble(Txt_Monto_Impuesto.Text);
    //        }

    //        if (Txt_Monto_Recargos.Text.Trim() != "")
    //        {
    //            Monto_Recargos = Convert.ToDouble(Txt_Monto_Recargos.Text);
    //        }

    //        if (Txt_Monto_Multas.Text.Trim() != "")
    //        {
    //            Monto_Multas = Convert.ToDouble(Txt_Monto_Multas.Text);
    //        }

    //        if (Txt_Sub_Total.Text.Trim() != "")
    //        {
    //            Sub_Total_Adeudo = Convert.ToDouble(Txt_Sub_Total.Text);
    //        }

    //        if (Txt_Total_Anticipo.Text.Trim() != "")
    //        {
    //            Total_Anticipo = Convert.ToDouble(Txt_Total_Anticipo.Text);
    //        }

    //        if (Txt_Total_Convenio.Text.Trim() != "")
    //        {
    //            Total_Convenio = Convert.ToDouble(Txt_Total_Convenio.Text);
    //        }

    //        if (Cmb_Periodicidad_Pago.SelectedIndex > 0)
    //        {
    //            Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
    //        }
    //        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("HONORARIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_MULTAS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Double)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
    //        Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));

    //        Temp_Monto_Impuesto = Monto_Impuesto;
    //        Temp_Monto_Multas = Monto_Multas;
    //        Temp_Monto_Recargos = Monto_Recargos;

    //        DataRow Dr_Parcialidades;
    //        for (Cont_Parcialidades = 0; Cont_Parcialidades < Cant_Parcialidades; Cont_Parcialidades++)
    //        {
    //            Total_Importe_Parcialidad = 0;

    //            if (Temp_Monto_Multas + Temp_Monto_Recargos != 0)
    //            {
    //                Monto_Importe = Total_Anticipo;
    //            }
    //            else
    //            {
    //                if (Monto_Parcialidades == 0)
    //                {
    //                    Monto_Parcialidades = Total_Convenio / (Cant_Parcialidades - (Cont_Parcialidades));
    //                }
    //                Monto_Importe = Monto_Parcialidades;
    //            }

    //            Dr_Parcialidades = Dt_Parcialidades.NewRow();
    //            Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;

    //            if (Monto_Importe - Total_Importe_Parcialidad >= Temp_Monto_Multas && Temp_Monto_Multas != 0)
    //            {
    //                Dr_Parcialidades["MONTO_MULTAS"] = Temp_Monto_Multas;
    //                Total_Importe_Parcialidad += Temp_Monto_Multas;
    //                Temp_Monto_Multas = 0;
    //            }
    //            else
    //            {
    //                if (Monto_Importe != Total_Importe_Parcialidad && Temp_Monto_Multas != 0)
    //                {
    //                    Dr_Parcialidades["MONTO_MULTAS"] = Monto_Importe - Total_Importe_Parcialidad;
    //                    Temp_Monto_Multas -= Monto_Importe - Total_Importe_Parcialidad;
    //                    Total_Importe_Parcialidad += Monto_Importe - Total_Importe_Parcialidad;
    //                }
    //                else
    //                {
    //                    Dr_Parcialidades["MONTO_MULTAS"] = 0;
    //                }
    //            }
    //            if (Monto_Importe - Total_Importe_Parcialidad >= Temp_Monto_Recargos && Temp_Monto_Recargos != 0)
    //            {
    //                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Temp_Monto_Recargos;
    //                Total_Importe_Parcialidad += Temp_Monto_Recargos;
    //                Temp_Monto_Recargos = 0;
    //            }
    //            else
    //            {
    //                if (Monto_Importe != Total_Importe_Parcialidad && Temp_Monto_Recargos != 0)
    //                {
    //                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Importe - Total_Importe_Parcialidad;
    //                    Temp_Monto_Recargos -= Monto_Importe - Total_Importe_Parcialidad;
    //                    Total_Importe_Parcialidad += Monto_Importe - Total_Importe_Parcialidad;
    //                }
    //                else
    //                {
    //                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = 0;
    //                }
    //            }
    //            Dr_Parcialidades["RECARGOS_MORATORIOS"] = 0;
    //            if (Monto_Importe - Total_Importe_Parcialidad >= Temp_Monto_Impuesto && Temp_Monto_Impuesto != 0)
    //            {
    //                Dr_Parcialidades["MONTO_IMPUESTO"] = Temp_Monto_Impuesto;
    //                Total_Importe_Parcialidad += Temp_Monto_Impuesto;
    //                Temp_Monto_Impuesto = 0;
    //            }
    //            else
    //            {
    //                if (Monto_Importe != Total_Importe_Parcialidad && Temp_Monto_Impuesto != 0)
    //                {
    //                    Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Importe - Total_Importe_Parcialidad;
    //                    Temp_Monto_Impuesto -= Monto_Importe - Total_Importe_Parcialidad;
    //                    Total_Importe_Parcialidad += Monto_Importe - Total_Importe_Parcialidad;
    //                }
    //                else
    //                {
    //                    Dr_Parcialidades["MONTO_IMPUESTO"] = 0;
    //                }
    //            }

    //            //if (Cont_Parcialidades + 1 == Cant_Parcialidades)
    //            //{
    //            //    Dr_Parcialidades["MONTO"] = Monto_Parcialidades - Ajuste_Monto;
    //            //}
    //            //else
    //            //{
    //            //    Dr_Parcialidades["MONTO"] = Monto_Parcialidades;
    //            //}
    //            Dr_Parcialidades["MONTO_IMPORTE"] = Total_Importe_Parcialidad;
    //            Dr_Parcialidades["HONORARIOS"] = 0.00;
    //            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Dias_Inhabilies.Calcular_Fecha(Obtener_Fecha_Periodo(DateTime.Now, Dias_Periodo, Cont_Parcialidades).ToShortDateString(), "1");
    //            Dr_Parcialidades["ESTATUS"] = "PENDIENTE";
    //            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
    //        }

    //        Grid_Parcialidades.DataSource = Dt_Parcialidades;
    //        Grid_Parcialidades.PageIndex = 0;
    //        Grid_Parcialidades.DataBind();

    //        Sumar_Totales_Parcialidades();
    //    }
    //}









    private void Sumar_Totales_Parcialidades()
    {
        Double Total_Honorarios = 0;
        Double Total_Multas = 0;
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
                Total_Multas += Convert.ToDouble(Fila_Grid.Cells[2].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[3].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Ordinarios += Convert.ToDouble(Fila_Grid.Cells[3].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[4].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Moratorios += Convert.ToDouble(Fila_Grid.Cells[4].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto += Convert.ToDouble(Fila_Grid.Cells[5].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[6].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Importe += Convert.ToDouble(Fila_Grid.Cells[6].Text.Replace("$", ""));
            }
        }

        if (Grid_Parcialidades.FooterRow != null)
        {
            Grid_Parcialidades.FooterRow.Cells[1].Text = Total_Honorarios.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[2].Text = Total_Multas.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[3].Text = Total_Recargos_Ordinarios.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[4].Text = Total_Recargos_Moratorios.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[5].Text = Total_Impuesto.ToString("$###,###,##0.00");
            Grid_Parcialidades.FooterRow.Cells[6].Text = Total_Importe.ToString("$###,###,##0.00");
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
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("###,###,###,##0.00");
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
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("###,###,##0.00");
        return Total_Anticipo;
    }

    protected void Txt_Cuenta_Predial_TextChanged()
    {
        if (Hdf_Cuenta_Predial_ID.Value.Length <= 0)
        {
            Txt_Propietario.Text = "";
            Txt_Colonia.Text = "";
            Txt_Calle.Text = "";
            Txt_No_Exterior.Text = "";
            Txt_No_Interior.Text = "";
        }
        else
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
            Hdf_Propietario_ID.Value = Cuenta.P_Propietario_ID;
            if (Cmb_Tipo_Solicitante.SelectedIndex == 0)
            {
                Txt_RFC.Text = Cuenta.P_RFC_Propietario;
                Txt_Solicitante.Text = Txt_Propietario.Text;
            }
            Hdf_RFC.Value = Txt_RFC.Text;
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
        Reporte.SetDataSource(Dt_Constancias);
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
        //Reporte.PrintToPrinter(1, true, 0, 0);
        Mostrar_Reporte(Archivo_PDF, "PDF");
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
    private void Imprimir_Reporte(DataSet Ds_Convenios, String Nombre_Reporte, String Nombre_Archivo)
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

    #endregion

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
    private DataSet Crear_Ds_Convenios_Fraccionamientos()
    {
        Ds_Pre_Convenios_Fraccionamientos Ds_Convenios_Fraccionamientos = new Ds_Pre_Convenios_Fraccionamientos();

        Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio Convenios_Fraccionamientos = new Cls_Ope_Pre_Convenios_Fraccionamientos_Negocio();
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Cat_Pre_Calles_Negocio Calles = new Cls_Cat_Pre_Calles_Negocio();
        Cls_Ate_Colonias_Negocio Colonias = new Cls_Ate_Colonias_Negocio();

        //DataTable Dt_Impuestos_Fraccionamientos;
        DataTable Dt_Cuenta_Predial = null;
        //DataTable Dt_Tipo_Predio;
        DataTable Dt_Temp;
        DataTable Dt_Temp_Detalles = null;
        DataRow Dr_Convenios_Fraccionameinto;
        String Calle_Id = "";
        String Colonia_Id = "";

        DataTable Dt_Fraccionamientos = Ds_Convenios_Fraccionamientos.Tables["Dt_Pre_Convenios_Fraccionamientos"];
        Convenios_Fraccionamientos.P_No_Convenio = Hdf_No_Convenio.Value;
        Dt_Temp = Convenios_Fraccionamientos.Consultar_Convenio_Fraccionamiento();
        Dt_Temp_Detalles = Convenios_Fraccionamientos.P_Dt_Parcialidades;

        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        {
            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
            Dr_Convenios_Fraccionameinto = Dt_Fraccionamientos.NewRow();
            Dr_Convenios_Fraccionameinto["NO_CONVENIO"] = Dr_Temp[Ope_Pre_Convenios_Fraccionamientos.Campo_No_Convenio];
            Dr_Convenios_Fraccionameinto["CUENTA_PREDIAL_ID"] = Dr_Temp[Ope_Pre_Convenios_Fraccionamientos.Campo_Cuenta_Predial_ID];
            Dr_Convenios_Fraccionameinto["FECHA"] = Dr_Temp[Ope_Pre_Convenios_Fraccionamientos.Campo_Fecha];
            Dr_Convenios_Fraccionameinto["REALIZO"] = Dr_Temp[Ope_Pre_Convenios_Fraccionamientos.Campo_Realizo];
            Dr_Convenios_Fraccionameinto["SOLICITANTE"] = Dr_Temp[Ope_Pre_Convenios_Fraccionamientos.Campo_Solicitante];
            Dr_Convenios_Fraccionameinto["PROPIETARIO_ID"] = Dr_Temp[Ope_Pre_Convenios_Fraccionamientos.Campo_Propietario_ID];
            Dt_Fraccionamientos.Rows.Add(Dr_Convenios_Fraccionameinto);
        }

        Dt_Fraccionamientos = Ds_Convenios_Fraccionamientos.Tables["Dt_Pre_Convenios_Det_Fraccionamientos"];
        foreach (DataRow Dr_Temp in Dt_Temp_Detalles.Rows)
        {
            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
            Dr_Convenios_Fraccionameinto = Dt_Fraccionamientos.NewRow();
            Dr_Convenios_Fraccionameinto["NO_PAGO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Pago].ToString();
            Dr_Convenios_Fraccionameinto["NO_CONVENIO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_No_Convenio].ToString();
            Dr_Convenios_Fraccionameinto["PERIODO"] = "----------";
            Dr_Convenios_Fraccionameinto["FECHA_PAGO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Fecha_Vencimiento];
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto].ToString().Equals(""))
            {
                Dr_Convenios_Fraccionameinto["MONTO_IMPUESTO"] = 0.00;
            }
            else
            {
                Dr_Convenios_Fraccionameinto["MONTO_IMPUESTO"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Impuesto];
            }
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios].ToString().Equals(""))
            {
                Dr_Convenios_Fraccionameinto["RECARGOS_ORDINARIOS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Fraccionameinto["RECARGOS_ORDINARIOS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Ordinarios];
            }
            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas].ToString().Equals(""))
            {
                Dr_Convenios_Fraccionameinto["MONTO_MULTAS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Fraccionameinto["MONTO_MULTAS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Monto_Multas];
            }

            if (Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios].ToString().Equals(""))
            {
                Dr_Convenios_Fraccionameinto["RECARGOS_MORATORIOS"] = 0.00;
            }
            else
            {
                Dr_Convenios_Fraccionameinto["RECARGOS_MORATORIOS"] = Dr_Temp[Ope_Pre_Detalles_Convenios_Fraccionamientos.Campo_Recargos_Moratorios];
            }
            Dr_Convenios_Fraccionameinto["HONORARIOS"] = 0.00;
            if (Dr_Temp["MONTO_IMPORTE"].ToString().Equals(""))
            {
                Dr_Convenios_Fraccionameinto["MONTO_IMPORTE"] = 0.00;
            }
            else
            {
                Dr_Convenios_Fraccionameinto["MONTO_IMPORTE"] = Dr_Temp["MONTO_IMPORTE"];
            }
            Dt_Fraccionamientos.Rows.Add(Dr_Convenios_Fraccionameinto);
        }

        Dt_Fraccionamientos = Ds_Convenios_Fraccionamientos.Tables["Dt_Pre_Cuentas_Predial"];
        Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Temp = Cuentas_Predial.Consultar_Cuenta();
        Dt_Cuenta_Predial = Dt_Temp;

        foreach (DataRow Dr_Temp in Dt_Temp.Rows)
        {
            //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
            Dr_Convenios_Fraccionameinto = Dt_Fraccionamientos.NewRow();
            Dr_Convenios_Fraccionameinto["CUENTA_PREDIAL_ID"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID];
            Dr_Convenios_Fraccionameinto["CUENTA_PREDIAL"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial];
            Dr_Convenios_Fraccionameinto["NO_EXTERIOR"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_No_Exterior];
            Dr_Convenios_Fraccionameinto["CALLE_ID_NOTIFICACION"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion];
            Calle_Id = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString();
            Dr_Convenios_Fraccionameinto["COLONIA_ID_NOTIFICACION"] = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion];
            Colonia_Id = Dr_Temp[Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString();
            Dt_Fraccionamientos.Rows.Add(Dr_Convenios_Fraccionameinto);
        }

        if (Colonia_Id.Length != 0)
        {
            Dt_Fraccionamientos = Ds_Convenios_Fraccionamientos.Tables["Dt_Ate_Colonias"];
            Colonias.P_Campos_Dinamicos = Cat_Ate_Colonias.Campo_Colonia_ID + ", " + Cat_Ate_Colonias.Campo_Nombre;
            Colonias.P_Filtros_Dinamicos = Cat_Ate_Colonias.Campo_Colonia_ID + " = '" + Colonia_Id + "'";
            Dt_Temp = Colonias.Consultar_Colonias();

            foreach (DataRow Dr_Temp in Dt_Temp.Rows)
            {
                //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
                Dr_Convenios_Fraccionameinto = Dt_Fraccionamientos.NewRow();
                Dr_Convenios_Fraccionameinto["COLONIA_ID"] = Dr_Temp[Cat_Ate_Colonias.Campo_Colonia_ID];
                Dr_Convenios_Fraccionameinto["NOMBRE"] = Dr_Temp[Cat_Ate_Colonias.Campo_Nombre];
                Dt_Fraccionamientos.Rows.Add(Dr_Convenios_Fraccionameinto);
            }
        }

        if (Calle_Id.Length != 0)
        {
            Dt_Fraccionamientos = Ds_Convenios_Fraccionamientos.Tables["Dt_Pre_Calles"];
            Calles.P_Calle_ID = Calle_Id;
            Dt_Temp = Calles.Consultar_Nombre_Id_Calles();

            foreach (DataRow Dr_Temp in Dt_Temp.Rows)
            {
                //Inserta los datos de los Impuestos de Fraccionamiento en la Tabla
                Dr_Convenios_Fraccionameinto = Dt_Fraccionamientos.NewRow();
                Dr_Convenios_Fraccionameinto["CALLE_ID"] = Dr_Temp[Cat_Pre_Calles.Campo_Calle_ID];
                Dr_Convenios_Fraccionameinto["NOMBRE"] = Dr_Temp[Cat_Pre_Calles.Campo_Nombre];
                Dt_Fraccionamientos.Rows.Add(Dr_Convenios_Fraccionameinto);
            }
        }

        return Ds_Convenios_Fraccionamientos;
    }

}
