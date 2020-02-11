using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using System.Data.OracleClient;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Descuentos_Predial.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Convenios_Frm_Convenios_Predial : System.Web.UI.Page
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
        if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty))
        {
            Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
        }
        if (!IsPostBack)
        {
            Inicializa_Controles();

            Hdf_Cuenta_Predial_ID.Value = Request.QueryString["Cuenta_Predial_ID"].ToString();// Session["CPRE_CUENTA_PREDIAL_ID"].ToString();
            Session["Cuenta_Predial_ID"] = Hdf_Cuenta_Predial_ID.Value;
            Hdf_No_Convenio.Value = Request.QueryString["No_Convenio"].ToString();//Convert.ToDouble(Session["CPRE_NO_CONVENIO"].ToString()).ToString ("0000000000");
            Cargar_Convenio();

            Session["ESTATUS_CUENTAS"] = "IN ('ACTIVA','VIGENTE')";
            Session["TIPO_CONTRIBUYENTE"] = "SIN TIPO";

        }
    }

    #endregion Pago_Load
    
    #region Metodos

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 29-ago-2010
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
            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Cmb_Hasta_Anio_Periodo.Enabled = Habilitado;
            Cmb_Hasta_Bimestre_Periodo.Enabled = Habilitado;
            Cmb_Tipo_Solicitante.Enabled = Habilitado;
            Txt_Solicitante.Enabled = false;
            Txt_RFC.Enabled = false;
            Cmb_Periodicidad_Pago.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;
            Txt_Numero_Parcialidades.Enabled = Habilitado;
            Txt_Observaciones.Enabled = Habilitado;
            Txt_Descuento_Recargos_Ordinarios.Enabled = false;
            Txt_Descuento_Recargos_Moratorios.Enabled = false;
            Txt_Porcentaje_Anticipo.Enabled = Habilitado;
            Txt_Total_Anticipo.Enabled = Habilitado;
            // no editables
            Txt_Cuenta_Predial.Enabled = false;
            Txt_Propietario.Enabled = false;
            Txt_Colonia.Enabled = false;
            Txt_Calle.Enabled = false;
            Txt_No_Exterior.Enabled = false;
            Txt_No_Interior.Enabled = false;
            Txt_Monto_Impuesto.Enabled = false;
            Txt_Monto_Recargos.Enabled = false;
            Txt_Monto_Moratorios.Enabled = false;
            Txt_Adeudo_Honorarios.Enabled = false;
            Txt_Numero_Convenio.Enabled = false;
            Txt_Realizo.Enabled = false;
            Txt_Fecha_Convenio.Enabled = false;
            Txt_Fecha_Vencimiento.Enabled = false;
            Txt_Total_Adeudo.Enabled = false;
            Txt_Total_Descuento.Enabled = false;
            Txt_Sub_Total.Enabled = false;
            Txt_Total_Convenio.Enabled = false;

            Grid_Parcialidades.Enabled = true;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Catálogo
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_Propietario_ID.Value = "";
        Hdf_No_Convenio.Value = "";
        Hdf_Propietario_ID.Value = "";
        Hdf_RFC_Propietario.Value = "";
        Hdf_RFC_Solicitante.Value = "";
        Hdf_Solicitante.Value = "";
        //Datos Cuenta
        Txt_Cuenta_Predial.Text = "";
        Txt_Monto_Impuesto.Text = "";
        Txt_Monto_Impuesto.Text = "";
        Txt_Adeudo_Honorarios.Text = "";
        Txt_Monto_Moratorios.Text = "";
        Txt_Monto_Recargos.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Propietario.Text = "";
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
        Txt_Total_Adeudo.Text = "";
        Txt_Total_Descuento.Text = "";
        Txt_Sub_Total.Text = "";
        //Parcialidades
        Txt_Porcentaje_Anticipo.Text = "";
        Txt_Total_Anticipo.Text = "";
        Txt_Total_Convenio.Text = "";
        Grid_Parcialidades.DataSource = null;
        Grid_Parcialidades.DataBind();
    }
    
    private static void Busqueda_Propietarios(String Cuenta_Predial_ID)
    {
        DataSet Ds_Prop;
        var M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            //string Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            //M_Orden_Negocio.P_Contrarecibo = Get_Contra_ID();
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                //Session.Remove("Ds_Prop_Datos");
                //Session["Ds_Prop_Datos"] = Ds_Prop;
                //Hdn_Propietario_ID.Value = Ds_Prop.Tables[0].Rows[0]["Propietario"].ToString().Trim();
                //Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Parcialidades
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las parcialidades
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 01-sep-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Parcialidades()
    {
        DataTable Dt_Parcialidades = new DataTable();
        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(String)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_HONORARIOS", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
        Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Parcialidades.Columns.Add(new DataColumn("PERIODO", typeof(String)));

        return Dt_Parcialidades;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Recuperar_Datos_Tabla_Parcialidades
    ///DESCRIPCIÓN          : Lee el grid de las parcialidades y devuelve una instancia como DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 08/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Recuperar_Datos_Tabla_Parcialidades()
    {
        DataTable Dt_Parcialidades = Crear_Tabla_Parcialidades();

        DataRow Dr_Parcialidades;
        //Se barre el Grid para cargar el DataTable con los valores del grid
        foreach (GridViewRow Row in Grid_Parcialidades.Rows)
        {
            Dr_Parcialidades = Dt_Parcialidades.NewRow();
            Dr_Parcialidades["NO_PAGO"] = Row.Cells[0].Text;
            Dr_Parcialidades["MONTO_HONORARIOS"] = Convert.ToDecimal(Row.Cells[2].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Convert.ToDecimal(Row.Cells[3].Text.Replace("$", ""));
            Dr_Parcialidades["RECARGOS_MORATORIOS"] = Convert.ToDecimal(Row.Cells[4].Text.Replace("$", ""));
            Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Row.Cells[5].Text.Replace("$", ""));
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Convert.ToDateTime(Row.Cells[7].Text);
            Dr_Parcialidades["ESTATUS"] = Row.Cells[8].Text;
            Dr_Parcialidades["PERIODO"] = HttpUtility.HtmlDecode(Row.Cells[1].Text);
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
        }
        return Dt_Parcialidades;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Convenio
    ///DESCRIPCIÓN          : Llena la tabla de Convenios de predial con una consulta que puede o no tener Filtros.
    ///PARAMETROS
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 27-ago-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Convenio()
    {
        Cls_Ope_Pre_Convenios_Predial_Negocio Convenio = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        DataTable Dt_Convenios_Predial;
        Convenio.P_Reestructura = false;
        String Periodo;

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {

            Convenio.P_Campos_Foraneos = true;
            Convenio.P_No_Convenio = Hdf_No_Convenio.Value;
            Convenio.P_Validar_Convenios_Cumplidos = true;

            Dt_Convenios_Predial = Convenio.Consultar_Convenio_Predial();

            if (Dt_Convenios_Predial != null)
            {
                if (Dt_Convenios_Predial.Rows.Count > 0)
                {
                    foreach (DataRow Row in Dt_Convenios_Predial.Rows)
                    {
                        Hdf_Cuenta_Predial_ID.Value = Row[Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id].ToString();
                        Session["Cuenta_Predial_ID"] = Hdf_Cuenta_Predial_ID.Value;
                        Hdf_Propietario_ID.Value = Row[Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id].ToString();
                        Txt_Cuenta_Predial.Text = Row["Cuenta_Predial"].ToString();
                        //Txt_Clasificacion.Text = Row["Tipo_Predio"].ToString();
                        Consultar_Datos_Cuenta_Predial();
                        Txt_Numero_Convenio.Text = Row[Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                        Cmb_Estatus.SelectedValue = Row[Ope_Pre_Convenios_Predial.Campo_Estatus].ToString();
                        if (Row[Ope_Pre_Convenios_Predial.Campo_Solicitante].ToString() != "")
                        {
                            Txt_Solicitante.Text = Row[Ope_Pre_Convenios_Predial.Campo_Solicitante].ToString();
                            Txt_RFC.Text = Row[Ope_Pre_Convenios_Predial.Campo_RFC].ToString();
                            Cmb_Tipo_Solicitante.SelectedIndex = 1;
                        }
                        else
                        {
                            Txt_Solicitante.Text = Row["Nombre_Propietario"].ToString();
                            Cmb_Tipo_Solicitante.SelectedIndex = 0;
                        }
                        Txt_Numero_Parcialidades.Text = Row[Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades].ToString();
                        Cmb_Periodicidad_Pago.SelectedValue = Row[Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago].ToString();
                        Txt_Realizo.Text = Row["Nombre_Realizo"].ToString();
                        Periodo = Row[Ope_Pre_Convenios_Predial.Campo_Hasta_Periodo].ToString();
                        if (Periodo != "")
                        {
                            Cmb_Hasta_Anio_Periodo.SelectedValue = Periodo.Substring(1, 4);
                            Cmb_Hasta_Bimestre_Periodo.SelectedValue = Periodo.Substring(0, 1);
                        }
                        Txt_Fecha_Convenio.Text = Convert.ToDateTime(Row[Ope_Pre_Convenios_Predial.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
                        Txt_Observaciones.Text = Row[Ope_Pre_Convenios_Predial.Campo_Observaciones].ToString();
                        Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios]).ToString("#,##0.00");
                        Txt_Descuento_Recargos_Moratorios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios]).ToString("#,##0.00");
                        Txt_Total_Adeudo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Adeudo]).ToString("#,##0.00");
                        Txt_Monto_Impuesto.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Predial]).ToString("#,##0.00");
                        Txt_Monto_Recargos.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Recargos]).ToString("#,##0.00");
                        Txt_Adeudo_Honorarios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Honorarios]).ToString("#,##0.00");
                        Txt_Total_Descuento.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Descuento]).ToString("#,##0.00");
                        Txt_Sub_Total.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Sub_Total]).ToString("#,##0.00");
                        Txt_Porcentaje_Anticipo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo]).ToString("#,##0.00");
                        Txt_Total_Anticipo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Anticipo]).ToString("#,##0.00");
                        Txt_Total_Convenio.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Convenio]).ToString("#,##0.00");
                        Grid_Parcialidades.DataSource = Convenio.P_Dt_Parcialidades;
                        Grid_Parcialidades.PageIndex = 0;
                        Grid_Parcialidades.DataBind();

                        Sumar_Totales_Parcialidades();
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }

    }

    #region Validaciones

    #endregion Validaciones

#endregion Metodos
    
    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Predial
    ///DESCRIPCIÓN          : Realiza la búsqueda de los adeudos de la cuenta predial introducida
    ///                         y muestra los adeudos
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Predial()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();

        Int32 Anio_Primer_Adeudo = 0;
        Int32 Anio_Ultimo_Adeudo = 0;

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
                // metodo que carga datos de la cuenta
                Txt_Cuenta_Predial_TextChanged();
                // datos para convenio nuevo
                Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado.ToUpper();
                Txt_Fecha_Convenio.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                Cmb_Estatus.SelectedValue = "ACTIVO";
                // cargar adeudos de la cuenta
                DataTable Dt_Adeudos = Rs_Consulta_Adeudos.Calcular_Recargos_Predial(Hdf_Cuenta_Predial_ID.Value);
                if (Dt_Adeudos.Rows.Count > 0)
                {
                    Session["Tabla_Adeudos"] = Dt_Adeudos;
                }

                if (Dt_Adeudos.Rows.Count > 0)
                {
                    Txt_Monto_Impuesto.Text = (Rs_Consulta_Adeudos.p_Total_Rezago + Rs_Consulta_Adeudos.p_Total_Corriente).ToString("#,##0.00");
                    Txt_Monto_Recargos.Text = String.Format("{0:#,##0.00}", Rs_Consulta_Adeudos.p_Total_Recargos_Generados);
                    if (Txt_Cuenta_Predial.Text != "111212154PGFT10")
                    {
                        Txt_Adeudo_Honorarios.Text = "0.00";
                    }
                    else
                    {
                        Txt_Adeudo_Honorarios.Text = "300.00";
                    }
                    Txt_Monto_Moratorios.Text = "0.00";

                    // obtener anio del primer y ultimo adeudo de la tabla obtenida de adeudos
                    Int32.TryParse(Dt_Adeudos.Rows[0][0].ToString().Substring(1, 4), out Anio_Primer_Adeudo);
                    Int32.TryParse(Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1][0].ToString().Substring(1, 4), out Anio_Ultimo_Adeudo);
                    // agregar al combo hasta anio periodo valores para los anio con adeudo
                    Cmb_Hasta_Anio_Periodo.Items.Clear();
                    if (Anio_Ultimo_Adeudo > 0 && Anio_Ultimo_Adeudo > Anio_Primer_Adeudo)
                    {
                        for (int i = Anio_Ultimo_Adeudo; i >= Anio_Primer_Adeudo; --i)
                        {
                            ListItem Anio_Adeudo = new ListItem(i.ToString(), i.ToString());
                            Cmb_Hasta_Anio_Periodo.Items.Add(Anio_Adeudo);
                        }
                    }

                    // seleccionar en combos periodo los valores correspondientes
                    Cmb_Hasta_Anio_Periodo.SelectedValue = Anio_Ultimo_Adeudo.ToString();
                    Cmb_Hasta_Bimestre_Periodo.SelectedValue = Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1][0].ToString().Substring(0, 1);

                }
                else
                {
                    Txt_Adeudo_Honorarios.Text = "0.00";
                    Txt_Monto_Impuesto.Text = "0.00";
                    Txt_Monto_Moratorios.Text = "0.00";
                    Txt_Monto_Recargos.Text = "0.00";
                }
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Sumar_Totales_Parcialidades
    /// DESCRIPCIÓN: Sumar el total de parcialidades y mostrar en el grid de parcialidades
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Sumar_Totales_Parcialidades()
    {
        Decimal Total_Honorarios = 0;
        Decimal Total_Recargos_Ordinarios = 0;
        Decimal Total_Recargos_Moratorios = 0;
        Decimal Total_Impuesto = 0;
        Decimal Total_Importe = 0;

        foreach (GridViewRow Fila_Grid in Grid_Parcialidades.Rows)
        {
            if (Fila_Grid.Cells[2].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Ordinarios += Convert.ToDecimal(Fila_Grid.Cells[2].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[3].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Recargos_Moratorios += Convert.ToDecimal(Fila_Grid.Cells[3].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[4].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto += Convert.ToDecimal(Fila_Grid.Cells[4].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Importe += Convert.ToDecimal(Fila_Grid.Cells[5].Text.Replace("$", ""));
            }
            if (Fila_Grid.Cells[6].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Honorarios += Convert.ToDecimal(Fila_Grid.Cells[6].Text.Replace("$", ""));
            }
        }

        if (Grid_Parcialidades.FooterRow != null)
        {
            Grid_Parcialidades.FooterRow.Cells[2].Text = Total_Recargos_Ordinarios.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[3].Text = Total_Recargos_Moratorios.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[4].Text = Total_Impuesto.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[5].Text = Total_Importe.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[6].Text = Total_Honorarios.ToString("$#,##0.00");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Cuenta_Predial_TextChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de texto en la caja TxT_Cuenta_Predial
    ///                actualiza los datos del propietario
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
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
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Cuenta=Cuenta.Consultar_Datos_Propietario();
            Txt_Calle.Text = Cuenta.P_Nombre_Calle;
            Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
            Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
            Hdf_Propietario_ID.Value = Cuenta.P_Propietario_ID;
            Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
            Txt_No_Interior.Text = Cuenta.P_No_Interior;
            // copiar en el solicitante, solo si esta seleccionado PROPIETARIO
            if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
            {
                Txt_Solicitante.Text = Txt_Propietario.Text;
                Txt_RFC.Text = Cuenta.P_RFC_Propietario;
            }
            Hdf_RFC_Propietario.Value = Txt_RFC.Text;
        }
    }

#endregion EVENTOS
    
    #region Impresion Folios

#endregion Impresion Folios

}
