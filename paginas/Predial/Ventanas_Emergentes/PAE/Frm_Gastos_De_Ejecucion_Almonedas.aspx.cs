using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Caja_Pagos.Negocio;
using Presidencia.Catalogo_Cajas.Negocio;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Predial_Pae_Etapas.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Gastos_Ejecucion.Negocio;
using Presidencia.Predial_Pae_Notificaciones.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;


public partial class paginas_Predial_Ventanas_Emergentes_PAE_Frm_Gastos_De_Ejecucion_Almonedas : System.Web.UI.Page
{
    string Cuenta_Predial;
    string Adeudo;
    string Proceso;
    string Estatus;
    string No_Detalle;
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Cuenta_Predial = Request.QueryString["Cuenta_Predial"].ToString();
            Adeudo = Request.QueryString["Adeudo"].ToString();
            Proceso = Request.QueryString["Proceso"].ToString();
            Estatus = Request.QueryString["Estatus"].ToString();
            No_Detalle = Request.QueryString["NO_DETALLE_ETAPA"].ToString();

            Session["AGREGA_GASTOS"] = false;
            Session["NO_DETALLE_ETAPA"] = No_Detalle;
            Session["Adeudo"] = Adeudo;
            Session["CUENTA"] = Cuenta_Predial;
            Cargar_Combo_Gastos_Ejecucion();
            Cargar_Notificacion();
            Txt_Adeudo.Text = String.Format("{0:C2}", Convert.ToDouble(Adeudo));
        }
        Frm_Gastos_Alomedas.Page.Title = "Gastos de Ejecución";
        Lbl_Title.Text = "Gastos de Ejecución";
        Mensaje_Error();

    }
    #endregion

    #region Metodos
    #region Pagos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Generales
    ///DESCRIPCIÓN: Limpia los Generales de la Forma de Recepción de Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpiar_Generales()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Txt_Cuenta_Predial.Text = "";
        Txt_Bimestre_Inicial.Text = "";
        Txt_Anio_Inicial.Text = "";
        Cmb_Bimestre_Final.SelectedIndex = 0;
        Cmb_Anio_Final.Items.Clear();
        Grid_Listado_Adeudos.DataSource = new DataTable();
        Grid_Listado_Adeudos.DataBind();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Calculos
    ///DESCRIPCIÓN: Limpia la sección de cálculos
    ///PARAMETROS: 
    ///CREO: Miguel Angel Bedolla Moreno  
    ///FECHA_CREO: 18/Enero/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpiar_Calculos()
    {
        Txt_Periodo_Rezago.Text = "";
        Txt_Periodo_Actual.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Actual.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_SubTotal.Text = "";
        Txt_Descuento_Corriente.Text = "";
        Txt_Total.Text = "";
        Txt_Ajuste_Tarifario.Text = "";
        Txt_Total_Pagar.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Calculos_Pago
    ///DESCRIPCIÓN: Limpia los caluclos Finales de la Forma de Recepción de Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Limpiar_Calculos_Pago()
    {
        Hfd_No_Descuento_Recargos.Value = "";
        Txt_Periodo_Rezago.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Periodo_Actual.Text = "";
        Txt_Adeudo_Actual.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Recargos_Moratorios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_SubTotal.Text = "";
        Txt_Descuento_Corriente.Text = "";
        Txt_Total.Text = "";
        Txt_Ajuste_Tarifario.Text = "";
        Txt_Total_Pagar.Text = "";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Impuesto_Corriente
    ///DESCRIPCIÓN: Obtiene los Recargos Impuesto.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Impuesto_Corriente()
    {
        Double Impuesto = 0;
        Double Corriente = 0;
        Double Descuento_Corriente = 0;

        if (Txt_Adeudo_Actual.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Adeudo_Actual.Text.Trim().Replace("$", "")) > 0)
            {
                Corriente = Convert.ToDouble(Txt_Adeudo_Actual.Text.Trim().Replace("$", ""));
            }
        }

        if (Txt_Descuento_Corriente.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Descuento_Corriente.Text.Trim().Replace("$", "")) > 0)
            {
                Descuento_Corriente = Convert.ToDouble(Txt_Descuento_Corriente.Text.Trim().Replace("$", ""));
            }
        }

        Impuesto = Corriente - Descuento_Corriente;

        return Impuesto;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Recargos_Moratorios
    ///DESCRIPCIÓN: Obtiene los Recargos Moratorios Totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Recargos_Moratorios()
    {
        Double Recargos_Moratorios = 0;
        Double Recargos_Moratorios_Neto = 0;
        Double Recargos_Moratorios_Descuento = 0;
        if (Txt_Recargos_Moratorios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Recargos_Moratorios.Text.Trim().Replace("$", "")) > 0)
            {
                Recargos_Moratorios_Neto = Convert.ToDouble(Txt_Recargos_Moratorios.Text.Trim().Replace("$", ""));
            }
        }
        Recargos_Moratorios = Recargos_Moratorios_Neto - Recargos_Moratorios_Descuento;
        return Recargos_Moratorios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Impuesto_Rezago
    ///DESCRIPCIÓN: Obtiene los Recargos Impuesto.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Impuesto_Rezago()
    {
        Double Impuesto = 0;
        Double Rezago = 0;

        if (Txt_Adeudo_Rezago.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Adeudo_Rezago.Text.Trim().Replace("$", "")) > 0)
            {
                Rezago = Convert.ToDouble(Txt_Adeudo_Rezago.Text.Trim().Replace("$", ""));
            }
        }


        Impuesto = Rezago;
        return Impuesto;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Ingreso
    ///DESCRIPCIÓN:Obtiene la Clave de Ingreso.
    ///PARAMETROS: Tipo. Tipo de la Clave que se buscara
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Clave_Ingreso(String Tipo)
    {
        String Clave = null;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Clave_Ingreso = Tipo;
        DataTable Dt_Temporal = RP_Negocio.Consultar_Clave_Ingreso();
        if (Dt_Temporal.Rows.Count > 0)
        {
            Clave = Dt_Temporal.Rows[0]["CLAVE_INGRESO"].ToString();
        }
        return Clave;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Clave_Ingreso
    ///DESCRIPCIÓN:Obtiene la Dependencia de una Clave de Ingreso.
    ///PARAMETROS: Tipo. Tipo de la Clave que se buscara
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Obtener_Dependencia_Clave_Ingreso(String Clave_Ingreso)
    {
        String Dependencia = null;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Clave_Ingreso = Clave_Ingreso;
        DataTable Dt_Temporal = RP_Negocio.Consultar_Dependencia();
        if (Dt_Temporal.Rows.Count > 0)
        {
            Dependencia = Dt_Temporal.Rows[0]["DEPENDENCIA"].ToString();
        }
        return Dependencia;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Honorarios
    ///DESCRIPCIÓN: Obtiene los Honorarios a Cobrar.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Honorarios()
    {
        Double Honorarios = 0;
        Double Honorarios_Neto = 0;
        Double Honorarios_Descuento = 0;
        if (Txt_Honorarios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Honorarios.Text.Trim().Replace("$", "")) > 0)
            {
                Honorarios_Neto = Convert.ToDouble(Txt_Honorarios.Text.Trim().Replace("$", ""));
            }
        }
        Honorarios = Honorarios_Neto - Honorarios_Descuento;
        return Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Recargos_Ordinarios
    ///DESCRIPCIÓN: Obtiene los Recargos Ordinarios Totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Double Obtener_Recargos_Ordinarios()
    {
        Double Recargos_Ordinarios = 0;
        Double Recargos_Ordinarios_Neto = 0;
        Double Recargos_Ordinarios_Descuento = 0;
        if (Txt_Total_Recargos_Ordinarios.Text.Trim().Length > 0)
        {
            if (Convert.ToDouble(Txt_Total_Recargos_Ordinarios.Text.Trim().Replace("$", "")) > 0)
            {
                Recargos_Ordinarios_Neto = Convert.ToDouble(Txt_Total_Recargos_Ordinarios.Text.Trim().Replace("$", ""));
            }
        }
        Recargos_Ordinarios = Recargos_Ordinarios_Neto - Recargos_Ordinarios_Descuento;
        return Recargos_Ordinarios;
    }
    public void Configuracion_Formulario(Boolean Habilidato)
    {
        Txt_Cuenta_Predial.Enabled = true;
        Cmb_Tipo_Pago.SelectedIndex = 0;
        Cmb_Tipo_Pago.Enabled = Habilidato;
        //Txt_Codigo_Seguridad.Enabled = Habilidato;
        //Txt_Titular_Tarjeta.Enabled = Habilidato;
        //Txt_No_Tarjeta.Enabled = Habilidato;
        //Cmb_Validez_Mes.Enabled = Habilidato;
        //Cmb_Valido_Anio.Enabled = Habilidato;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Registrar_Pasivos
    ///DESCRIPCIÓN: Registra los pasivos que será cobrados en la Caja.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Registrar_Pasivos()
    {
        //Eliminar_Pasivos_No_Pagados();

        DataTable Dt_Pasivos = new DataTable();
        DataRow Dr_Renglon;
        Dt_Pasivos.Columns.Add(new DataColumn("Cuenta_Predial", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Cuenta_Predial_Id", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Clave_Ingreso", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Descripcion", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Fecha_Tramite", typeof(DateTime)));
        Dt_Pasivos.Columns.Add(new DataColumn("Fecha_Vencimiento", typeof(DateTime)));
        Dt_Pasivos.Columns.Add(new DataColumn("Monto", typeof(Double)));
        Dt_Pasivos.Columns.Add(new DataColumn("Estatus", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Dependencia", typeof(String)));
        Dt_Pasivos.Columns.Add(new DataColumn("Contribuyente", typeof(String)));
        Double Impuestos = Obtener_Impuesto_Corriente();
        if (Impuestos > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            if (Hfd_Tipo_Predio.Value == "00001")
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("IMPUESTO URBANO");
            }
            else
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("IMPUESTO RUSTICO");
            }
            Dr_Renglon["Descripcion"] = "IMPUESTO CORRIENTE [" + Txt_Periodo_Actual.Text.Trim() + "]";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Impuestos;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Rezago = Obtener_Impuesto_Rezago();
        if (Rezago > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            if (Hfd_Tipo_Predio.Value == "00001")
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("REZAGO URBANO");
            }
            else
            {
                Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("REZAGO RUSTICO");
            }
            Dr_Renglon["Descripcion"] = "IMPUESTO REZAGO [" + Txt_Periodo_Rezago.Text.Trim() + "]";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Rezago;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Honorarios = Obtener_Honorarios();
        if (Honorarios > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("HONORARIOS");
            Dr_Renglon["Descripcion"] = "HONORARIOS";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Honorarios;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Recargos_Ordinarios = Obtener_Recargos_Ordinarios();
        if (Recargos_Ordinarios > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("RECARGOS ORDINARIOS");
            Dr_Renglon["Descripcion"] = "RECARGOS ORDINARIOS";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Recargos_Ordinarios;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        Double Recargos_Moratorios = Obtener_Recargos_Moratorios();
        if (Recargos_Moratorios > 0)
        {
            Dr_Renglon = Dt_Pasivos.NewRow();
            Dr_Renglon["Cuenta_Predial_Id"] = Hdf_Cuenta_Predial_ID.Value;
            Dr_Renglon["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Dr_Renglon["Clave_Ingreso"] = Obtener_Clave_Ingreso("RECARGOS MORATORIOS");
            Dr_Renglon["Descripcion"] = "RECARGOS MORATORIOS";
            Dr_Renglon["Fecha_Tramite"] = DateTime.Today;
            Dr_Renglon["Fecha_Vencimiento"] = DateTime.Today;
            Dr_Renglon["Monto"] = Recargos_Moratorios;
            Dr_Renglon["Estatus"] = "POR PAGAR";
            Dr_Renglon["Dependencia"] = Obtener_Dependencia_Clave_Ingreso(Dr_Renglon["Clave_Ingreso"].ToString());
            Dr_Renglon["Contribuyente"] = Txt_Propietario.Text;
            Dt_Pasivos.Rows.Add(Dr_Renglon);
        }
        return Dt_Pasivos;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Adeudos
    ///DESCRIPCIÓN: Crea la Tabla con el listado y los montos de los adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Tabla_Adeudos()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();
        Dt_Adeudos_Predial.Columns.Add("NO_ADEUDO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_CONVENIO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("NO_PAGO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("BIMESTRE", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("ANIO", Type.GetType("System.Int32"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Double"));

        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                if (Chk_Seleccion_Adeudo_Tmp.Checked)
                {
                    DataRow Fila_Tmp = Dt_Adeudos_Predial.NewRow();
                    Fila_Tmp["NO_ADEUDO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[1].Text.Trim());
                    Fila_Tmp["BIMESTRE"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.Trim());
                    Fila_Tmp["ANIO"] = HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.Trim());
                    Fila_Tmp["MONTO"] = Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""))) + Convert.ToDouble(HttpUtility.HtmlDecode(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", "")));
                    Dt_Adeudos_Predial.Rows.Add(Fila_Tmp);
                }
            }
        }

        return Dt_Adeudos_Predial;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Crear_Tabla_Totales
    ///DESCRIPCIÓN: Crea la Tabla con el listado y los montos de los totales.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 13 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataTable Crear_Tabla_Totales()
    {
        DataTable Dt_Adeudos_Predial = new DataTable();
        Dt_Adeudos_Predial.Columns.Add("CONCEPTO", Type.GetType("System.String"));
        Dt_Adeudos_Predial.Columns.Add("MONTO", Type.GetType("System.Double"));
        Dt_Adeudos_Predial.Columns.Add("REFERENCIA", Type.GetType("System.String"));
        //Agrega la Fila de Importe Corriente
        DataRow Fila_Corriente = Dt_Adeudos_Predial.NewRow();
        Fila_Corriente["CONCEPTO"] = "CORRIENTE";
        Fila_Corriente["MONTO"] = Convert.ToDouble(Txt_Adeudo_Actual.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Corriente);
        //Agrega la Fila de Importe REzago
        DataRow Fila_Rezago = Dt_Adeudos_Predial.NewRow();
        Fila_Rezago["CONCEPTO"] = "REZAGO";
        Fila_Rezago["MONTO"] = Convert.ToDouble(Txt_Adeudo_Rezago.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Rezago);
        //Agrega la Fila de Importe Honorarios
        DataRow Fila_Honorarios = Dt_Adeudos_Predial.NewRow();
        Fila_Honorarios["CONCEPTO"] = "HONORARIOS";
        Fila_Honorarios["MONTO"] = Convert.ToDouble(Txt_Honorarios.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Honorarios);

        //Agrega la Fila de Recargos
        DataRow Fila_Recargos = Dt_Adeudos_Predial.NewRow();
        Fila_Recargos["CONCEPTO"] = "RECARGOS";
        Fila_Recargos["MONTO"] = Convert.ToDouble(Txt_Total_Recargos_Ordinarios.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Recargos);

        //Agrega la Fila de Recargos Moratorios
        DataRow Fila_Recargos_Moratorios = Dt_Adeudos_Predial.NewRow();
        Fila_Recargos_Moratorios["CONCEPTO"] = "RECARGOS_MORATORIOS";
        Fila_Recargos_Moratorios["MONTO"] = Convert.ToDouble(Txt_Recargos_Moratorios.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Recargos_Moratorios);

        //Agrega la Fila de Descuento Corriente
        DataRow Fila_Descuento_Corriente = Dt_Adeudos_Predial.NewRow();
        Fila_Descuento_Corriente["CONCEPTO"] = "DESCUENTOS_CORRIENTES";
        Fila_Descuento_Corriente["MONTO"] = Convert.ToDouble(Txt_Descuento_Corriente.Text.Replace("$", "").Trim());
        Dt_Adeudos_Predial.Rows.Add(Fila_Descuento_Corriente);

        return Dt_Adeudos_Predial;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Cuenta_Predial
    ///DESCRIPCIÓN: Carga los Campos con los Datos de la Cuenta predial Seleccionada.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 22 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Cuenta_Predial(String P_Cuenta_Predial)
    {
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        DataTable Dt_Datos = RP_Negocio.Consultar_Cuentas_Predial();
        if (Dt_Datos.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
            Txt_Cuenta_Predial.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL"].ToString().Trim() : "";
            Txt_Propietario.Text = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim())) ? Dt_Datos.Rows[0]["PROPIETARIO"].ToString().Trim() : "---------------------------";

            Visibilidad_Controles("PAGO_NORMAL");
            if (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim()))
            {
                Llenar_Grid_Adeudos(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim(), false);
            }
            RP_Negocio.P_Cuenta_Predial = (!string.IsNullOrEmpty(Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim())) ? Dt_Datos.Rows[0]["CUENTA_PREDIAL_ID"].ToString().Trim() : "";
            DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
            Llenar_Combo_Anios(Dt_Adeudos);
            Cmb_Bimestre_Final.SelectedIndex = (Cmb_Bimestre_Final.Items.Count - 1);
            Cmb_Anio_Final.SelectedIndex = (Cmb_Anio_Final.Items.Count - 1);
            if (Txt_Bimestre_Inicial.Text.Length < 1) { Cmb_Tipo_Pago.Enabled = false; }
            if (Txt_Total_Pagar.Text == "$ 0.00") { Btn_Ejecutar_Pago.Enabled = false; }
        }
        else
        {
            Hdf_Cuenta_Predial_ID.Value = "";
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Visibilidad_Controles
    ///DESCRIPCIÓN: Carga la Visibilidad de los controles.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Octubre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Visibilidad_Controles(String Tipo_Visibilidad)
    {
        if (Tipo_Visibilidad.Trim().Equals("PAGO_NORMAL"))
        {
            //Txt_Convenio.Text = "SIN CONVENIO";
            Div_Listado_Adeudos_Predial.Visible = true;
        }
        //else if (Tipo_Visibilidad.Trim().Equals("PAGO_CONVENIO"))
        //{
        //    Div_Listado_Adeudos_Predial.Visible = false;
        //    Div_Listado_Adeudos_Convenio.Visible = true;
        //    Txt_Bimestre_Inicial.Text = "-";
        //    Txt_Anio_Inicial.Text = "-";
        //    Cmb_Bimestre_Final.Items.Clear();
        //    Cmb_Bimestre_Final.Items.Insert(0, new ListItem("-", ""));
        //    Cmb_Bimestre_Final.Enabled = false;
        //    Cmb_Anio_Final.Items.Clear();
        //    Cmb_Anio_Final.Items.Insert(0, new ListItem("-", ""));
        //    Cmb_Anio_Final.Enabled = false;
        //}
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Adeudos
    ///DESCRIPCIÓN: Llena el grid de los Adeudos dependiendo de los seleccionados.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Grid_Adeudos(String P_Cuenta_Predial, Boolean Tomar_Filtrado)
    {
        Grid_Listado_Adeudos.Visible = true;
        Grid_Listado_Adeudos.Columns[1].Visible = true;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        if (Tomar_Filtrado)
        {
            RP_Negocio.P_Anio_Filtro = Convert.ToInt32(Cmb_Anio_Final.SelectedItem.Text);
            RP_Negocio.P_Bimestre_Filtro = Convert.ToInt32(Cmb_Bimestre_Final.SelectedItem.Text);
        }
        DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
        Grid_Listado_Adeudos.DataSource = Dt_Adeudos;
        Grid_Listado_Adeudos.DataBind();
        Grid_Listado_Adeudos.Columns[1].Visible = false;
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            Txt_Anio_Inicial.Text = Dt_Adeudos.Rows[0]["ANIO"].ToString();
            Txt_Bimestre_Inicial.Text = Dt_Adeudos.Rows[0]["BIMESTRE"].ToString();
        }
        else
        {
            Txt_Anio_Inicial.Text = "";
            Txt_Bimestre_Inicial.Text = "";
        }
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable dt = GAP_Negocio.Calcular_Recargos_Predial(P_Cuenta_Predial);
        Txt_Periodo_Actual.Text = GAP_Negocio.p_Periodo_Corriente;
        Txt_Periodo_Rezago.Text = GAP_Negocio.p_Periodo_Rezago;
        Realizar_Calculos_Pago(false);
        Grid_Listado_Adeudos.Visible = false;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Anios
    ///DESCRIPCIÓN: Llena el Combo de los Años en los que existe un Adeudo.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 23 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Llenar_Combo_Anios(DataTable Dt_Anios)
    {
        Dt_Anios = Dt_Anios.DefaultView.ToTable(true, "ANIO");
        Cmb_Anio_Final.DataSource = Dt_Anios;
        Cmb_Anio_Final.DataTextField = "ANIO";
        Cmb_Anio_Final.DataValueField = "ANIO";
        Cmb_Anio_Final.DataBind();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Realizar_Calculos_Pago
    ///DESCRIPCIÓN: Realiza los Calculos del Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Realizar_Calculos_Pago(Boolean Existe_Convenio)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Honorarios;
        //Declaración de las Variables para la realización de los Calculos.
        Double Corriente = 0;
        Double Rezagos = 0;
        Double Recargos_Ordinarios = 0;
        Double Recargos_Moratorios = 0;
        Double Honorarios = 0;
        Double Gastos_Ejecucion = 0;
        Double Subtotal = 0;
        Double Porcentaje_Descuento_Corriente = 0;
        Double Descuento_Corriente = 0;
        Double Porcentaje_Descuento_Recargos_Ordinarios = 0;
        Double Descuento_Recargos_Ordinarios = 0;
        Double Porcentaje_Descuento_Recargos_Moratorios = 0;
        Double Descuento_Recargos_Moratorios = 0;
        Double Porcentaje_Descuento_Honorarios = 0;
        Double Descuento_Honorarios = 0;
        Double Total = 0;
        Double Ajuste_Tarifario = 0;
        Double Total_Pagar = 0;

        Cls_Ope_Pre_Pae_Honorarios_Negocio Consulta_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        Consulta_Honorarios.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Honorarios = Consulta_Honorarios.Consultar_Total_Honorarios();
        Btn_Ejecutar_Pago.Enabled = true;


        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                if (Chk_Seleccion_Adeudo.Checked)
                {
                    Corriente = Corriente + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[7].Text.Trim().Replace("$", ""));
                    Rezagos = Rezagos + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[8].Text.Trim().Replace("$", ""));
                    Recargos_Ordinarios = Recargos_Ordinarios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[9].Text.Trim().Replace("$", ""));
                    Recargos_Moratorios = Recargos_Moratorios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[10].Text.Trim().Replace("$", ""));
                    Honorarios = Honorarios + Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[11].Text.Trim().Replace("$", ""));
                }
            }
        }

        DataTable Dt_Descuento_Pronto_Pago = Resumen_Predio.Consultar_Descuentos_Pronto_Pago();
        if (Dt_Descuento_Pronto_Pago.Rows.Count > 0)
        {
            if (Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMMMMMMMMMMMM}", DateTime.Now)].ToString().Trim() != "0")
            {
                Porcentaje_Descuento_Corriente = Convert.ToDouble(Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMMMMMMMMMMMM}", DateTime.Now)].ToString().Trim()) / 100;
            }
            else
            {
                Porcentaje_Descuento_Corriente = 0;
            }
        }
        else
        {
            Porcentaje_Descuento_Corriente = 0;
        }

        Resumen_Predio.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        DataTable Dt_Descuentos_Recargos = Resumen_Predio.Consultar_Descuentos_Recargos();
        if (Dt_Descuentos_Recargos.Rows.Count > 0)
        {
            Hfd_No_Descuento_Recargos.Value = Dt_Descuentos_Recargos.Rows[0]["No_Descuento_Predial"].ToString();
            Double Periodo_Descuento = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Hasta_Periodo_Anio"].ToString() + Convert.ToInt32(Dt_Descuentos_Recargos.Rows[0]["Hasta_Periodo_Bimestre"].ToString()).ToString());
            Double Periodo_Pago = 0;
            if (Cmb_Anio_Final.Items.Count > 0)
            {
                Periodo_Pago = Convert.ToDouble(Cmb_Anio_Final.SelectedItem.Text + Convert.ToInt32(Cmb_Bimestre_Final.SelectedItem.Text).ToString());
            }
            else
            {
                for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
                {
                    if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                    {
                        CheckBox Chk_Seleccion_Adeudo = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                        if (Chk_Seleccion_Adeudo.Checked)
                        {
                            Periodo_Pago = Convert.ToDouble(Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text + Convert.ToInt32(Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text));
                        }
                    }
                }
                if (Periodo_Pago == 0)
                {
                    Periodo_Pago = Convert.ToDouble(DateTime.Now.Year + Convert.ToInt32("6").ToString());
                }
                else
                {
                    if (Obtener_Dato_Consulta(Hdf_Cuenta_Predial_ID.Value) == "SI")
                    {
                        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                        DataTable Dt_Adeudos = Adeudos.Obtener_Dato_Consulta(Hdf_Cuenta_Predial_ID.Value, Convert.ToInt32(Periodo_Pago.ToString().Substring(0, 4)));
                        int Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                        Periodo++;
                        for (int i = Periodo; i < 7; i++)
                        {
                            if (Convert.ToDouble(Dt_Adeudos.Rows[0]["PAGO_" + i].ToString()) > 0)
                            {
                                Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                                break;
                            }
                            else
                            {
                                Periodo = i;
                            }
                        }
                        if (Periodo >= 7)
                        {
                            Periodo = 6;
                        }
                        Periodo_Pago = Convert.ToDouble(Periodo_Pago.ToString().Substring(0, 4) + Periodo);
                    }
                    else
                    {
                        String Valores = "";
                        Valores = Obtener_Dato_Consulta_Valor(Hdf_Cuenta_Predial_ID.Value);
                        if (Valores != "")
                        {
                            String[] valores_cuota_minima = Valores.Split('-');
                            if (valores_cuota_minima[1].Trim() == "")
                            {
                                valores_cuota_minima[1] = Obtener_Dato_Consulta_Cuota(Periodo_Pago.ToString().Substring(0, 4));
                            }
                            if (Convert.ToDouble(valores_cuota_minima[0]) < Convert.ToDouble(valores_cuota_minima[1]))
                            {
                                Periodo_Pago = Convert.ToDouble(DateTime.Now.Year + Convert.ToInt32("6").ToString());
                            }
                            else
                            {
                                Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
                                DataTable Dt_Adeudos = Adeudos.Obtener_Dato_Consulta(Hdf_Cuenta_Predial_ID.Value, Convert.ToInt32(Periodo_Pago.ToString().Substring(0, 4)));
                                int Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                                Periodo++;
                                for (int i = Periodo; i < 7; i++)
                                {
                                    if (Convert.ToDouble(Dt_Adeudos.Rows[0]["PAGO_" + i].ToString()) > 0)
                                    {
                                        Periodo = Convert.ToInt32(Periodo_Pago.ToString().Substring(4));
                                        break;
                                    }
                                    else
                                    {
                                        Periodo = i;
                                    }
                                }
                                if (Periodo >= 7)
                                {
                                    Periodo = 6;
                                }
                                Periodo_Pago = Convert.ToDouble(Periodo_Pago.ToString().Substring(0, 4) + Periodo);
                            }
                        }
                    }
                }
            }
            if (Periodo_Descuento == Periodo_Pago)
            {
                if (!string.IsNullOrEmpty(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo"].ToString().Trim()))
                {
                    Porcentaje_Descuento_Recargos_Ordinarios = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo"]) / 100;
                }
                else
                {
                    Porcentaje_Descuento_Recargos_Ordinarios = 0;
                }
                if (!string.IsNullOrEmpty(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo_Moratorio"].ToString().Trim()))
                {
                    Porcentaje_Descuento_Recargos_Moratorios = Convert.ToDouble(Dt_Descuentos_Recargos.Rows[0]["Porcentaje_Recargo_Moratorio"]) / 100;
                }
                else
                {
                    Porcentaje_Descuento_Recargos_Moratorios = 0;
                }
            }
            else
            {
                Porcentaje_Descuento_Recargos_Ordinarios = 0;
                Porcentaje_Descuento_Recargos_Moratorios = 0;
                Btn_Ejecutar_Pago.Enabled = false;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "gaco", "alert('EXISTE UN DESCUENTO QUE NO COINCIDE CON EL PERIODO A PAGAR, FAVOR DE VERIFICARLO');", true);
                //Lbl_Ecabezado_Mensaje.Text = "Verificar el periodo del descuento aplicado.";
                //Lbl_Mensaje_Error.Text = "No es posible pagar el adeudo ya que tiene un descuento pero no coincide el periodo del mismo con el del pago.";
                //Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        else
        {
            Porcentaje_Descuento_Recargos_Ordinarios = 0;
            Porcentaje_Descuento_Recargos_Moratorios = 0;
        }

        //Se calcula el Valor de los Descuentos
        if (Dt_Honorarios.Rows.Count > 0)
        {
            Honorarios = Convert.ToDouble(Dt_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString());
        }
        Descuento_Corriente = (Corriente * Porcentaje_Descuento_Corriente);
        Descuento_Recargos_Ordinarios = (Recargos_Ordinarios * Porcentaje_Descuento_Recargos_Ordinarios);
        Descuento_Recargos_Moratorios = (Recargos_Moratorios * Porcentaje_Descuento_Recargos_Moratorios);
        Descuento_Honorarios = (Honorarios * Porcentaje_Descuento_Honorarios);

        //Se calcula el Subtotal
        Subtotal = Rezagos + Corriente + Honorarios + Gastos_Ejecucion + Recargos_Ordinarios + Recargos_Moratorios;

        //Se calcula el Total Neto
        Total = Subtotal - Descuento_Corriente;
        Total = Total - Descuento_Recargos_Ordinarios;
        Total = Total - Descuento_Recargos_Moratorios;
        Total = Total - Descuento_Honorarios;

        //Se obtiene el Ajuste Tarifario y Total a Pagar
        Total_Pagar = Math.Round(Total);
        Ajuste_Tarifario = Total - Total_Pagar;

        //Se muestran los resultados
        Txt_Adeudo_Rezago.Text = "$ " + Rezagos.ToString("#,####,###0.00");
        Txt_Adeudo_Actual.Text = "$ " + Corriente.ToString("#,####,###0.00");
        Txt_Total_Recargos_Ordinarios.Text = "$ " + Recargos_Ordinarios.ToString("#,####,###0.00");
        Txt_Recargos_Moratorios.Text = "$ " + Recargos_Moratorios.ToString("#,####,###0.00");
        Txt_Honorarios.Text = "$ " + Honorarios.ToString("#,####,###0.00");
        Txt_Gastos_Ejecucion.Text = "$ " + Gastos_Ejecucion.ToString("#,####,###0.00");
        Txt_SubTotal.Text = "$ " + Subtotal.ToString("#,####,###0.00");
        Txt_Descuento_Corriente.Text = "$ " + Descuento_Corriente.ToString("#,####,###0.00");
        //Txt_Descuento_Recargos_Ordinarios.Text = "$ " + Descuento_Recargos_Ordinarios.ToString("#,####,###0.00");
        //Txt_Descuento_Recargos_Moratorios.Text = "$ " + Descuento_Recargos_Moratorios.ToString("#,####,###0.00");
        //Txt_Descuento_Honorarios.Text = "$ " + Descuento_Honorarios.ToString("#,####,###0.00");
        Txt_Total.Text = "$ " + Total.ToString("#,####,###0.00");
        Txt_Ajuste_Tarifario.Text = "$ " + (Ajuste_Tarifario * (-1)).ToString("#,####,###0.00");
        Txt_Total_Pagar.Text = "$ " + Total_Pagar.ToString("#,####,###0.00");

        Cargar_Periodos_Pagar();
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
    private static Int32 Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones)
    {
        String Mi_SQL;
        Int32 Dato_Consulta = 0;

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

            //OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Cmmd.CommandText = Mi_SQL;
            Dato_Consulta = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
            if (Convert.IsDBNull(Dato_Consulta))
            {
                Dato_Consulta = 1;
            }
            else
            {
                Dato_Consulta = Dato_Consulta + 1;
            }
        }
        catch (OracleException Ex)
        {
            //Indicamos el mensaje 
            throw new Exception(Ex.ToString());
        }
        return Dato_Consulta;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta
    ///DESCRIPCIÓN          : Consulta los montos de un convenio o reestructura según sea el caso
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 21/Diciembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static DataTable Obtener_Dato_Consulta(String cuenta_predial, Int32 Anio)
    {
        String Mi_SQL;
        DataTable Dt_Montos = new DataTable();

        try
        {
            Mi_SQL = "SELECT ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_1 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_1 + ",0) as PAGO_1, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_2 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_2 + ",0) as PAGO_2, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_3 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_3 + ",0) as PAGO_3, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_4 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_4 + ",0) as PAGO_4, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_5 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_5 + ",0) as PAGO_5, ";
            Mi_SQL += "NVL(" + Ope_Pre_Adeudos_Predial.Campo_Bimestre_6 + ",0)-NVL(" + Ope_Pre_Adeudos_Predial.Campo_Pago_Bimestre_6 + ",0) as PAGO_6 ";
            Mi_SQL += "FROM " + Ope_Pre_Adeudos_Predial.Tabla_Ope_Pre_Adeudos_Predial + " WHERE " + Ope_Pre_Adeudos_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial + "' AND " + Ope_Pre_Adeudos_Predial.Campo_Anio + "=" + Anio;
            DataSet dataset = OracleHelper.ExecuteDataset(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            if (dataset != null)
            {
                Dt_Montos = dataset.Tables[0];
            }
        }
        catch
        {
        }
        finally
        {
        }

        return Dt_Montos;
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
    private static String Obtener_Dato_Consulta(String cuenta_predial_id)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Mi_SQL = "SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial_id + "'";

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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta_Valor
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static String Obtener_Dato_Consulta_Valor(String cuenta_predial_id)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
            //select cuenta.valor_fiscal*(select tasa.tasa_anual from cat_pre_tasas_predial_anual tasa where tasa.tasa_PREDIAL_id=cuenta.tasa_PREDIAL_id 
            //and tasa.ANIO=2012)/1000||' - '||(SELECT MINIMA.CUOTA FROM CAT_PRE_CUOTAS_MINIMAS MINIMA WHERE MINIMA.CUOTA_MINIMA_ID=CUENTA.CUOTA_MINIMA_ID) 
            //from cat_pre_cuentas_predial cuenta where cuenta.cuenta_predial_id='0000031126';
            Mi_SQL = "SELECT CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + " * (SELECT TASA." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual + " FROM " + Cat_Pre_Tasas_Predial_Anual.Tabla_Cat_Pre_Tasas_Predial_Anual + " TASA WHERE  TASA." + Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_ID + "=CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ")/1000||' - '||(SELECT MINIMA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + " MINIMA WHERE MINIMA." + Cat_Pre_Cuotas_Minimas.Campo_Cuota_Minima_ID + "=CUENTA." + Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID + ") FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " CUENTA WHERE " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + "='" + cuenta_predial_id + "'";

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

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Obtener_Dato_Consulta_Valor
    ///DESCRIPCIÓN          : Consulta el Campo dado de la Tabla Indicada
    ///PARAMETROS:     
    ///CREO                 : Miguel Angel Bedolla Moreno
    ///FECHA_CREO           : 24/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private static String Obtener_Dato_Consulta_Cuota(String Anio)
    {
        String Mi_SQL;
        String Dato_Consulta = "";

        try
        {
            //select cuenta.valor_fiscal*(select tasa.tasa_anual from cat_pre_tasas_predial_anual tasa where tasa.tasa_PREDIAL_id=cuenta.tasa_PREDIAL_id 
            //and tasa.ANIO=2012)/1000||' - '||(SELECT MINIMA.CUOTA FROM CAT_PRE_CUOTAS_MINIMAS MINIMA WHERE MINIMA.CUOTA_MINIMA_ID=CUENTA.CUOTA_MINIMA_ID) 
            //from cat_pre_cuentas_predial cuenta where cuenta.cuenta_predial_id='0000031126';
            Mi_SQL = "SELECT " + Cat_Pre_Cuotas_Minimas.Campo_Cuota + " FROM " + Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas + "  WHERE " + Cat_Pre_Cuotas_Minimas.Campo_Año + "=" + Anio;

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
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Periodos_Pagar
    ///DESCRIPCIÓN: Obtiene los Periodos tanto de Rezsago como Actual y los muestra en
    ///             los Campos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 16 Octubre 2011 [Domingo ¬¬]
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Periodos_Pagar()
    {
        Txt_Periodo_Actual.Text = "";
        Txt_Periodo_Rezago.Text = "";
        if (Grid_Listado_Adeudos.Rows.Count > 0)
        {
            String Bimestre_Rezago_Inicial = "";
            String Anio_Rezago_Inicial = "";
            String Bimestre_Rezago_Final = "";
            String Anio_Rezago_Final = "";
            Boolean Capturo_Inicial_Rezago = false;
            String Bimestre_Corriente_Inicial = "";
            String Anio_Corriente_Inicial = "";
            String Bimestre_Corriente_Final = "";
            String Anio_Corriente_Final = "";
            Boolean Capturo_Inicial_Corriente = false;
            for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
            {
                if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
                {
                    CheckBox Chk_Seleccion_Adeudo = (CheckBox)(Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo"));
                    if (Chk_Seleccion_Adeudo.Checked)
                    {
                        if (Grid_Listado_Adeudos.Rows[Contador].Cells[2].Text.ToString().Trim().Equals("REZAGO"))
                        {
                            if (!Capturo_Inicial_Rezago)
                            {
                                Bimestre_Rezago_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                                Anio_Rezago_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                                Capturo_Inicial_Rezago = true;
                            }
                            Bimestre_Rezago_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                            Anio_Rezago_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                        }
                        else if (Grid_Listado_Adeudos.Rows[Contador].Cells[2].Text.ToString().Trim().Equals("CORRIENTE"))
                        {
                            if (!Capturo_Inicial_Corriente)
                            {
                                Bimestre_Corriente_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                                Anio_Corriente_Inicial = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                                Capturo_Inicial_Corriente = true;
                            }
                            Bimestre_Corriente_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[4].Text.ToString().Trim();
                            Anio_Corriente_Final = Grid_Listado_Adeudos.Rows[Contador].Cells[3].Text.ToString().Trim();
                        }
                    }
                }
            }
            if (Bimestre_Rezago_Inicial.Trim().Length > 0)
            {
                Txt_Periodo_Rezago.Text = Bimestre_Rezago_Inicial + "/" + Anio_Rezago_Inicial + " - " + Bimestre_Rezago_Final + "/" + Anio_Rezago_Final;
            }
            if (Bimestre_Corriente_Inicial.Trim().Length > 0)
            {
                Txt_Periodo_Actual.Text = Bimestre_Corriente_Inicial + "/" + Anio_Corriente_Inicial + " - " + Bimestre_Corriente_Final + "/" + Anio_Corriente_Final;
            }
        }
    }


    #endregion

    #region Tablas
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Honorarios
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Honorarios()
    {
        DataTable Dt_Honorarios = new DataTable();
        Dt_Honorarios.Columns.Add(new DataColumn("GASTO_EJECUCION_ID", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("TIPO_DE_GASTO", typeof(String)));
        Dt_Honorarios.Columns.Add(new DataColumn("IMPORTE", typeof(Decimal)));
        //Dt_Honorarios.Columns.Add(new DataColumn("FECHA_HONORARIO", typeof(String)));
        //Dt_Honorarios.Columns.Add(new DataColumn("PROCESO", typeof(String)));
        //Dt_Honorarios.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        return Dt_Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Notiticaciones
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las Notiticaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Notiticaciones()
    {
        DataTable Dt_Notificaciones = new DataTable();
        Dt_Notificaciones.Columns.Add(new DataColumn("FECHA_HORA", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("NOTIFICADOR", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("RECIBIO", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("ACUSE_RECIBIO", typeof(String)));
        Dt_Notificaciones.Columns.Add(new DataColumn("FOLIO", typeof(String)));
        //Dt_Notificaciones.Columns.Add(new DataColumn("PROCESO", typeof(String)));
        //Dt_Notificaciones.Columns.Add(new DataColumn("MEDIO_NOTIFICACION", typeof(String)));        
        return Dt_Notificaciones;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Publicaciones
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las Publicaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/03/2012 01:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Publicaciones()
    {
        DataTable Dt_Publicaciones = new DataTable();
        Dt_Publicaciones.Columns.Add(new DataColumn("FECHA_PUBLICACION", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("MEDIO_PUBLICACION", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("PAGINA", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("TOMO", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("PARTE", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("FOJA", typeof(String)));
        Dt_Publicaciones.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        //Dt_Publicaciones.Columns.Add(new DataColumn("PROCESO", typeof(String)));
        return Dt_Publicaciones;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Remates
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los Remates
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 04/05/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Remates()
    {
        DataTable Dt_Remates = new DataTable();
        Dt_Remates.Columns.Add(new DataColumn("LUGAR_REMATE", typeof(String)));
        Dt_Remates.Columns.Add(new DataColumn("FECHA_HORA", typeof(String)));
        Dt_Remates.Columns.Add(new DataColumn("INI_FECHA", typeof(String)));
        Dt_Remates.Columns.Add(new DataColumn("FIN_FECHA", typeof(String)));
        return Dt_Remates;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Abonos
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para los Abonos
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 04/05/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Abonos()
    {
        DataTable Dt_Abonos = new DataTable();
        Dt_Abonos.Columns.Add(new DataColumn("ABONO", typeof(String)));
        Dt_Abonos.Columns.Add(new DataColumn("ADEUDO_RESTANTE", typeof(String)));
        Dt_Abonos.Columns.Add(new DataColumn("ETAPA", typeof(String)));
        return Dt_Abonos;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Honorarios
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Honorarios(DataTable Dt_Honorarios)
    {
        DataRow Dr_Honorario;
        Dr_Honorario = Dt_Honorarios.NewRow();
        Dr_Honorario["GASTO_EJECUCION_ID"] = Cmb_Gastos_Ejecucion.SelectedValue;
        Dr_Honorario["TIPO_DE_GASTO"] = Cmb_Gastos_Ejecucion.SelectedItem.Text;
        Dr_Honorario["IMPORTE"] = Txt_Costo.Text;
        Dt_Honorarios.Rows.Add(Dr_Honorario);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Publicaciones
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla Publicaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Publicaciones(DataTable Dt_Publicaciones)
    {
        DataRow Dr_Publicaciones;
        Dr_Publicaciones = Dt_Publicaciones.NewRow();
        Dr_Publicaciones["MEDIO_PUBLICACION"] = Txt_Publicacion.Text.ToUpper();
        Dr_Publicaciones["FECHA_PUBLICACION"] = String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Fecha_Publicacion.Text));
        Dr_Publicaciones["PAGINA"] = Txt_Pagina.Text.ToUpper();
        Dr_Publicaciones["TOMO"] = Txt_Tomo.Text.ToUpper();
        Dr_Publicaciones["PARTE"] = Txt_Parte.Text.ToUpper();
        Dr_Publicaciones["FOJA"] = Txt_Foja.Text.ToUpper();
        Dt_Publicaciones.Rows.Add(Dr_Publicaciones);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Notificaciones
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de notificaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 01:49:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Notificaciones(DataTable Dt_Notificaciones)
    {
        DataRow Dr_Notificaciones;
        DateTime Hora;
        DateTime.TryParse(Txt_Fecha.Text + " " + Txt_Hora.Text, out Hora);

        Dr_Notificaciones = Dt_Notificaciones.NewRow();
        Dr_Notificaciones["FECHA_HORA"] = Hora.ToString("dd/MM/yyyy HH:mm:ss");
        Dr_Notificaciones["ESTATUS"] = Cmb_Estatus_Ejecucion.SelectedItem.Text;
        Dr_Notificaciones["NOTIFICADOR"] = Txt_Notificador.Text.ToUpper();
        Dr_Notificaciones["RECIBIO"] = Txt_Recibio.Text.ToUpper();
        Dr_Notificaciones["ACUSE_RECIBIO"] = Txt_Acuse.Text.ToUpper();
        Dr_Notificaciones["FOLIO"] = Txt_Folio.Text.ToUpper();
        Dt_Notificaciones.Rows.Add(Dr_Notificaciones);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Abonos
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla de Abonos
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 04/05/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Abonos(DataTable Dt_Abonos)
    {
        DataRow Dr_Abono;
        String Etapa;
        if (Cmb_Pasar_Etapa.SelectedIndex > 0) { Etapa = Cmb_Pasar_Etapa.SelectedItem.Text; }
        else { Etapa = ""; }
        Dr_Abono = Dt_Abonos.NewRow();
        Dr_Abono["ABONO"] = Txt_Abono.Text;
        Dr_Abono["ADEUDO_RESTANTE"] = Txt_Adeudo;
        Dr_Abono["ETAPA"] = Etapa;
        Dt_Abonos.Rows.Add(Dr_Abono);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Remates
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla Remates
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 04/05/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Remates(DataTable Dt_Remates)
    {
        DateTime Hora;
        DateTime.TryParse(Txt_Fecha_Remate.Text + " " + Txt_Hora_Remante.Text, out Hora);

        DataRow Dr_Remate;
        Dr_Remate = Dt_Remates.NewRow();
        Dr_Remate["LUGAR_REMATE"] = Txt_Remate.Text.ToUpper();
        Dr_Remate["FECHA_HORA"] = Hora.ToString("dd/MM/yyyy HH:mm:ss");
        Dr_Remate["INI_FECHA"] = String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Ini_Fecha.Text));
        Dr_Remate["FIN_FECHA"] = String.Format("{0:dd/MM/yy}", Convert.ToDateTime(Txt_Fin_Fecha.Text));
        Dt_Remates.Rows.Add(Dr_Remate);//Se asigna la nueva fila a la tabla
    }
    #endregion

    #region Generales
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: obtener_Recargos_Moratorios
    /// DESCRIPCIÓN: Leer las parcialidades del ultimo convenio o reestructura para obtener los adeudos
    ///            a tomar en cuenta para la reestructura
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 13-dic-2011
    /// MODIFICÓ: Armando Zavala Moreno
    /// FECHA_MODIFICÓ: 09-Feb-2012
    /// CAUSA_MODIFICACIÓN: Para que devuelva un valor decimal y pasarlo al Grid
    ///*******************************************************************************************************
    private decimal Obtener_Recargos_Moratorios(String Cuenta_Predial_ID)
    {
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Parcialidades = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Convenios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        DataTable Dt_Parcialidades;
        DataTable Dt_Convenios;
        Decimal Recargos_Moratorios = 0;
        Decimal Recargos_Ordinarios = 0;
        Decimal Honorarios = 0;
        Decimal Monto_Impuesto = 0;
        Decimal Monto_Base = 0;
        Decimal Adeudo_Honorarios = 0;
        Decimal Adeudo_Recargos = 0;
        Decimal Adeudo_Moratorios = 0;
        Decimal Monto_Total_Moratorios = 0;
        String No_Convenio = "";
        int Parcialidad = 0;
        DateTime Fecha_Vencimiento = DateTime.MinValue;
        int Meses_Transcurridos = 0;

        // consultar convenios de la cuenta
        Consulta_Convenios.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Consulta_Convenios.P_Ordenar_Dinamico = Ope_Pre_Convenios_Predial.Campo_Fecha + " DESC,"
            + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
        Dt_Convenios = Consulta_Convenios.Consultar_Convenio_Predial();
        // si la consulta arrojó resultado, utilizar el primer registro (convenio mas reciente)
        if (Dt_Convenios != null && Dt_Convenios.Rows.Count > 0)
        {
            No_Convenio = Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
            // consultar las parcialidades del ultimo convenio guardado (convenio o ultima reestructura)
            Consulta_Parcialidades.P_No_Convenio = No_Convenio;
            Dt_Parcialidades = Consulta_Parcialidades.Consultar_Parcialidades_Ultimo_Convenio();

            // llamar metodo para determinar si el convenio esta vencido
            if (Convenio_Vencido(Dt_Parcialidades))
            {
                Parcialidad = Dt_Parcialidades.Rows.Count - 1;
                // recorrer la tabla de parcialidades hasta encontrar parcialidades con estatus PAGADO
                while (Parcialidad >= 0)
                {
                    // si la parcialidad tiene estatus diferente de PAGADO, sumar adeudos
                    if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() != "PAGADO")
                    {
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString(), out Honorarios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString(), out Recargos_Ordinarios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString(), out Recargos_Moratorios);
                        Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString(), out Monto_Impuesto);
                        DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                        Adeudo_Honorarios += Honorarios;
                        Adeudo_Recargos += Recargos_Ordinarios;
                        Adeudo_Moratorios += Recargos_Moratorios;
                        Monto_Base += Monto_Impuesto;
                    }
                    Parcialidad--;
                }

                Meses_Transcurridos = Calcular_Meses_Entre_Fechas(Fecha_Vencimiento, DateTime.Now);
                Recargos_Moratorios = Calcular_Recargos_Moratorios(Monto_Base, Meses_Transcurridos);
            }
        }
        return Monto_Total_Moratorios = Convert.ToDecimal(Math.Round(Math.Round(Recargos_Moratorios + Adeudo_Moratorios, 3), 2).ToString("#,##0.00"));
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
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
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
    /// NOMBRE_FUNCIÓN: Calcular_Meses_Entre_Fechas
    /// DESCRIPCIÓN: Regresa un enteron con el numero de meses vencidos entre dos fechas
    ///             (tomando el primer dia de cada mes)
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inicial a comparar
    /// 		2. Hasta_Fecha: Fecha final a comparar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Int32 Calcular_Meses_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha)
    {
        DateTime Fecha_Inicial = Convert.ToDateTime(Desde_Fecha.Month + "/1" + "/" + Desde_Fecha.Year);
        DateTime Fecha_Final = Convert.ToDateTime(Hasta_Fecha.ToShortDateString());
        int Meses = 0;

        // aumentar el numero de meses mientras la fecha inicial mas los meses no supere la fecha final
        while (Fecha_Final > Fecha_Inicial.AddMonths(Meses))
        {
            Meses++;
        }

        return Meses;
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Recargos_Moratorios
    /// DESCRIPCIÓN: Calcular los recargos moratorios para una cantidad a partir de una fecha dados
    ///             (el numero de meses por el porcentaje de recargos por el monto base)
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Recargos_Moratorios(Decimal Monto_Base, Int32 Meses)
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Parametros;
        Decimal Recargos_Moratorios = 0;
        Decimal Porcentaje_Recargos = 0;

        // recuperar el porcentaje de recargos moratorios de la tabla de parametros
        Dt_Parametros = Parametros.Consultar_Parametros();
        if (Dt_Parametros != null)
        {
            if (Dt_Parametros.Rows.Count > 0)
            {
                Decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString(), out Porcentaje_Recargos);
            }
        }

        // obtener el producto de los meses por el porcentaje de recargos
        Porcentaje_Recargos *= Meses;

        // calcular recargos
        Recargos_Moratorios = Monto_Base * Porcentaje_Recargos / 100;

        return Recargos_Moratorios;
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Actualiza_PAE
    /// DESCRIPCIÓN: Actualiza la tabla OPE_PRE_PAE_ETAPAS
    /// PARÁMETROS:
    /// CREO: Armando Zavala Moreno
    /// FECHA_CREO: 09-May-2012 12:00:00 p.m.
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Actualiza_PAE(String Estatus)
    {
        try
        {
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Pae_Det_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
            Cls_Ope_Pre_Pae_Honorarios_Negocio Rs_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();

            Rs_Consulta_Adeudos.Calcular_Recargos_Predial(Hdf_Cuenta_Predial_ID.Value);//lLama al metodo y calcula sus adeudos
            Decimal Recargos_Moratorios = Obtener_Recargos_Moratorios(Hdf_Cuenta_Predial_ID.Value);
            Rs_Honorarios.P_No_Detalle_Etapa = Session["NO_DETALLE_ETAPA"].ToString();
            Decimal Honorarios = (Rs_Honorarios.Consultar_Total_Honorarios().Rows.Count > 0) ? Convert.ToDecimal(Rs_Honorarios.Consultar_Total_Honorarios().Rows[0]["TOTAL_HONORARIOS"]) : 0;

            Rs_Pae_Det_Etapas.P_Periodo_Corriente = Rs_Consulta_Adeudos.p_Periodo_Corriente;
            Rs_Pae_Det_Etapas.P_Adeudo_Corriente = Rs_Consulta_Adeudos.p_Total_Corriente.ToString();
            Rs_Pae_Det_Etapas.P_Periodo_Rezago = Rs_Consulta_Adeudos.p_Periodo_Rezago;
            Rs_Pae_Det_Etapas.P_Adeudo_Rezago = Rs_Consulta_Adeudos.p_Total_Rezago.ToString();
            Rs_Pae_Det_Etapas.P_Adeudo_Recargos_Ordinarios = Rs_Consulta_Adeudos.p_Total_Recargos_Generados.ToString();
            Rs_Pae_Det_Etapas.P_Adeudo_Recargos_Moratorios = Recargos_Moratorios.ToString();
            Rs_Pae_Det_Etapas.P_Adeudo_Honorarios = Honorarios.ToString();
            Rs_Pae_Det_Etapas.P_Adeudo_Total = Rs_Consulta_Adeudos.p_Total_Corriente + Rs_Consulta_Adeudos.p_Total_Rezago + Rs_Consulta_Adeudos.p_Total_Recargos_Generados + Recargos_Moratorios + Honorarios.ToString();
            Rs_Pae_Det_Etapas.P_No_Detalle_Etapa = Session["NO_DETALLE_ETAPA"].ToString();
            Rs_Pae_Det_Etapas.P_Estatus = Estatus;
            Rs_Pae_Det_Etapas.Actualiza_Pae_Det_Etapas();
            
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Armando Zavala Moreno.
    ///FECHA_CREO  : 17-Abril-2012
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        IBtn_Imagen_Error.Visible = true;
        Lbl_Ecabezado_Mensaje.Text = "";
        Lbl_Ecabezado_Mensaje.Text += P_Mensaje;// +"</br>";
        Div_Contenedor_Msj_Error.Visible = true;

    }
    private void Mensaje_Error()
    {
        IBtn_Imagen_Error.Visible = false;
        Lbl_Ecabezado_Mensaje.Text = "";
        Div_Contenedor_Msj_Error.Visible = false;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Guardar_Parametros_Pasar_Etapa
    ///DESCRIPCIÓN          : Llena la tabla Abonos
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 04/05/2012 05:18:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Guardar_Parametros_Pasar_Etapa()
    {
        Session["Etapa_Pae"] = Cmb_Pasar_Etapa.SelectedItem.Value;
        Session["Despacho_ID"] = Cmb_Despachos.SelectedValue;
        Session["Entrega"] = Txt_No_Entrega.Text;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Guardar_Remates
    ///DESCRIPCIÓN          : Llena la tabla Remates
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 04/05/2012 05:18:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Guardar_Remates()
    {
        DataTable Dt_Remates = Crear_Tabla_Remates();
        Llenar_DataRow_Remates(Dt_Remates);
        Session["Remates"] = Dt_Remates;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Guardar_Notificacion
    ///DESCRIPCIÓN          : Llena la tabla notifiaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 26/03/2012 05:18:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Guardar_Notificacion()
    {
        if (Txt_Notificador.Enabled == true)
        {
            DataTable Dt_Notificaciones = Crear_Tabla_Notiticaciones();
            Llenar_DataRow_Notificaciones(Dt_Notificaciones);
            Session["Notificaciones"] = Dt_Notificaciones;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Notificacion
    ///DESCRIPCIÓN          : Se carga la configuracion del combo Notificaciones, para que
    ///                       no existan dos registros de notificacion,
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 17/04/2012 04:40:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Notificacion()
    {
        Cls_Ope_Pre_Pae_Notificaciones_Negocio Notificacion = new Cls_Ope_Pre_Pae_Notificaciones_Negocio();
        DataTable Dt_Notificaciones = new DataTable();
        Notificacion.P_Cuenta_predial = Cuenta_Predial;
        Notificacion.P_Proceso = Proceso;
        Dt_Notificaciones = Notificacion.Consulta_Notificacion_Cuenta_Predial();

        foreach (DataRow Dr_Fila in Dt_Notificaciones.Rows)
        {
            if (Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Estatus].ToString() == "NOTIFICACION")
            {
                Txt_Fecha.Enabled = false;
                Txt_Hora.Enabled = false;
                Txt_Notificador.Enabled = false;
                Txt_Recibio.Enabled = false;
                Txt_Acuse.Enabled = false;
                Txt_Folio.Enabled = false;
                Txt_Fecha.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora].ToString().Substring(0, 10);
                Txt_Hora.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Fecha_Hora].ToString().Substring(11, 13);
                Txt_Notificador.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Notificador].ToString();
                Txt_Recibio.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Recibio].ToString();
                Txt_Acuse.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Acuse_Recibo].ToString();
                Txt_Folio.Text = Dr_Fila[Ope_Pre_Pae_Notificaciones.Campo_Folio].ToString();
                //Cmb_Estatus_Ejecucion.SelectedIndex = 1;
            }
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Notificacion
    ///DESCRIPCIÓN          : Se carga la configuracion del combo Notificaciones, para que
    ///                       no existan dos registros de notificacion,
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 17/04/2012 04:40:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validaciones(int Numero_Validacion)
    {
        Boolean Validacion = true;
        DateTime Hora;
        switch (Numero_Validacion)
        {
            case 1:
                if (Txt_Remate.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce el lugar del remate <br>";
                    Validacion = false;
                }
                if (Txt_Fecha_Remate.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce la fecha del remate <br>";
                    Validacion = false;
                }
                if (Txt_Hora_Remante.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce la hora del remate <br>";
                    Validacion = false;
                }
                if (Txt_Ini_Fecha.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce el inicio de la publicacion del remate <br>";
                    Validacion = false;
                }
                if (Txt_Fin_Fecha.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce el final de la publicacion del ramate <br>";
                    Validacion = false;
                }
                break;
            case 2:
                if (Txt_Fecha.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un la fecha de notificación <br>";
                    Validacion = false;
                }
                if (Txt_Hora.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un la hora de notificación <br>";
                    Validacion = false;
                }
                else
                {
                    if (DateTime.TryParse(Txt_Fecha.Text + " " + Txt_Hora.Text, out Hora))
                    {

                    }
                    else
                    {
                        Lbl_Ecabezado_Mensaje.Text += "Hora invalida <br>";
                        Validacion = false;
                    }
                }
                if (Txt_Notificador.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un notificador <br>";
                    Validacion = false;
                }
                if (Txt_Recibio.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce quien recibió <br>";
                    Validacion = false;
                }
                if (Txt_Acuse.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce el acuse de recibo <br>";
                    Validacion = false;
                }
                if (Txt_Folio.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce el folio";
                    Validacion = false;
                }

                if (Cmb_Estatus_Ejecucion.SelectedIndex < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Selecciona un estatus";
                    Validacion = false;
                }
                break;
            case 3:
                if (Txt_Publicacion.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce una publicación <br>";
                    Validacion = false;
                }
                if (Txt_Fecha_Publicacion.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce una fecha de publicación <br>";
                    Validacion = false;
                }
                if (Txt_Tomo.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un tomo de publicacion <br>";
                    Validacion = false;
                }
                if (Txt_Parte.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce la parte de publicacion <br>";
                    Validacion = false;
                }
                if (Txt_Foja.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce la foja de publicacion <br>";
                    Validacion = false;
                }
                if (Txt_Pagina.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce la pagina de publicacion <br>";
                    Validacion = false;
                }
                break;
            case 4:
            case 5:
                //if (Txt_Abono.Text.Length < 1)
                //{
                //    Lbl_Ecabezado_Mensaje.Text += "Introduce el abono";
                //    Validacion = false;
                //}
                //break;
                if (Cmb_Pasar_Etapa.SelectedIndex < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Selecciona una etapa del PAE";
                    Validacion = false;
                }
                break;
            case 6:
                if (Cmb_Gastos_Ejecucion.SelectedIndex < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Selecciona un gasto de ejecucion <br>";
                    Validacion = false;
                }
                if (Txt_Costo.Text.Length < 1)
                {
                    Lbl_Ecabezado_Mensaje.Text += "Introduce un costo de ejecucion <br>";
                    Validacion = false;
                }
                break;
            default:
                Validacion = true;
                break;
        }
        return Validacion;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cerrar_Guardar
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cerrar_Guardar()
    {
        DataTable Dt_Honorarios = (DataTable)Session["Honorarios"];
        DataTable Dt_Publicaciones = (DataTable)Session["Publicaciones"];
        DataTable Dt_Notificaciones = (DataTable)Session["Notificaciones"];
        DataTable Dt_Remates = (DataTable)Session["Remates"];
        String Nueva_Etapa = (Session["Etapa_Pae"] != null) ? Session["Etapa_Pae"].ToString() : "NO";
        //Si inserta y despues son borrados la tabla ya no es nula pero no tiene datos y marca error
        if (Dt_Honorarios != null && Dt_Honorarios.Rows.Count < 1) { Dt_Honorarios = null; }
        if (Dt_Publicaciones != null && Dt_Publicaciones.Rows.Count < 1) { Dt_Publicaciones = null; }
        if (Dt_Notificaciones != null && Dt_Notificaciones.Rows.Count < 1) { Dt_Notificaciones = null; }
        if (Dt_Remates != null && Dt_Remates.Rows.Count < 1) { Dt_Remates = null; }

        if (Dt_Honorarios != null || Dt_Publicaciones != null || Dt_Notificaciones != null || Dt_Remates != null || Nueva_Etapa!="NO")
        {
            Session["AGREGA_GASTOS"] = true;
            //Cierra la ventana
            string Pagina = "<script language='JavaScript'>";
            Pagina += "window.close();";
            Pagina += "</script>";
            ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
        }
        else
        {
            Mensaje_Error("No se introdujo ningun dato para guardar");
        }
    }
    #endregion

    #endregion

    #region Eventos

    #region Texbox
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Hora_TextChanged
    ///DESCRIPCIÓN          : Valida la hora para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Hora_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Hora.Text != "")
        {
            if (DateTime.TryParse(Txt_Hora.Text, out Fecha_valida))
            {
                Txt_Hora.Text = Fecha_valida.ToString("hh:mm tt");
            }
            else
            {
                Txt_Hora.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha.Text, out Fecha_valida))
            {
                Txt_Fecha.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Publicacion_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Publicacion_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Publicacion.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Publicacion.Text, out Fecha_valida))
            {
                Txt_Fecha_Publicacion.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Publicacion.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Remate_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Remate_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Remate.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Remate.Text, out Fecha_valida))
            {
                Txt_Fecha_Remate.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Remate.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Hora_Remante_TextChanged
    ///DESCRIPCIÓN          : Valida la hora para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Hora_Remante_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Hora_Remante.Text != "")
        {
            if (DateTime.TryParse(Txt_Hora_Remante.Text, out Fecha_valida))
            {
                Txt_Hora_Remante.Text = Fecha_valida.ToString("hh:mm tt");
            }
            else
            {
                Txt_Hora_Remante.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Ini_Fecha_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Ini_Fecha_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Ini_Fecha.Text != "")
        {
            if (DateTime.TryParse(Txt_Ini_Fecha.Text, out Fecha_valida))
            {
                Txt_Ini_Fecha.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Ini_Fecha.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fin_Fecha_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fin_Fecha_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fin_Fecha.Text != "")
        {
            if (DateTime.TryParse(Txt_Fin_Fecha.Text, out Fecha_valida))
            {
                Txt_Fin_Fecha.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fin_Fecha.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fin_Fecha_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Abono_TextChanged(object sender, EventArgs e)
    {
        Mensaje_Error();
        if (Txt_Abono.Text.Length > 0)
        {
            Double Abono = Convert.ToDouble(Txt_Abono.Text);
            Double Adeudo_Inicial = Convert.ToDouble(Session["Adeudo"]);
            Double Adeudo_Final = 0;
            Adeudo_Final = Adeudo_Inicial - Abono;
            if (Adeudo_Final >= 0)
            {
                Txt_Adeudo.Text = String.Format("{0:C2}", Adeudo_Final);
            }
            else
            {
                Mensaje_Error("El abono no puede ser mayor al adeudo");
                Txt_Abono.Text = "";
                Txt_Adeudo.Text = String.Format("{0:C2}", Convert.ToDouble(Session["Adeudo"].ToString()));
            }
        }
        else
        {
            Txt_Adeudo.Text = String.Format("{0:C2}", Convert.ToDouble(Session["Adeudo"].ToString()));
        }
    }
    #endregion

    #region Combos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Despachos_Externos
    ///DESCRIPCIÓN: Metodo usado para cargar la informacion de los despachos externos
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 02/02/2012 10:22:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Despachos_Externos(DropDownList Cmb_Despachos)
    {
        DataTable Dt_Despachos = new DataTable();
        try
        {
            Cls_Cat_Pre_Despachos_Externos_Negocio Despachos_Externos = new Cls_Cat_Pre_Despachos_Externos_Negocio();
            Despachos_Externos.P_Filtro = "";
            Cmb_Despachos.DataTextField = Cat_Pre_Despachos_Externos.Campo_Despacho;
            Cmb_Despachos.DataValueField = Cat_Pre_Despachos_Externos.Campo_Despacho_Id;

            Dt_Despachos = Despachos_Externos.Consultar_Despachos_Externos();

            foreach (DataRow Dr_Fila in Dt_Despachos.Rows)
            {
                if (Dr_Fila["ESTATUS"].ToString() != "VIGENTE")//Busca la cuenta en el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Despachos.DataSource = Dt_Despachos;
            Cmb_Despachos.DataBind();
            Cmb_Despachos.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));

        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Despachos_SelectedIndexChanged
    ///DESCRIPCIÓN          : Obtiene el numero de entrega del despacho cuando se selecciona
    ///                       un despacho
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 16/02/2012 01:37:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Despachos_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DateTime Año = DateTime.Now;
            Cls_Ope_Pre_Pae_Etapas_Negocio Obtener_No = new Cls_Ope_Pre_Pae_Etapas_Negocio();
            Obtener_No.P_Despacho_Id = Cmb_Despachos.SelectedValue;
            Txt_No_Entrega.Text = Obtener_No.Consultar_No_Entrega(Año.Year.ToString());
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Gastos_Ejecucion
    ///DESCRIPCIÓN: Metodo usado para cargar el listado de los gastos de ejecucion
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 06/03/2012 12:22:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Gastos_Ejecucion()
    {
        DataTable Dt_Gastos = new DataTable();
        try
        {
            Cls_Cat_Pre_Gastos_Ejecucion_Negocio Rs_Gastos = new Cls_Cat_Pre_Gastos_Ejecucion_Negocio();
            Rs_Gastos.P_Filtro = "";
            Cmb_Gastos_Ejecucion.DataTextField = Cat_Pre_Gastos_Ejecucion.Campo_Descripcion;
            Cmb_Gastos_Ejecucion.DataValueField = Cat_Pre_Gastos_Ejecucion.Campo_Gasto_Ejecucion_ID;

            Dt_Gastos = Rs_Gastos.Consultar_Gastos_Ejecucion();
            Session["Gastos"] = Dt_Gastos;
            foreach (DataRow Dr_Fila in Dt_Gastos.Rows)
            {
                if (Dr_Fila[Cat_Pre_Gastos_Ejecucion.Campo_Estatus].ToString() != "VIGENTE")//Busca el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Gastos_Ejecucion.DataSource = Dt_Gastos;
            Cmb_Gastos_Ejecucion.DataBind();
            Cmb_Gastos_Ejecucion.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Estatus_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo usado para cargar el listado de los estatus de ejecucion
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 06/03/2012 12:22:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Estatus_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        Int16 Indice;
        try
        {
            Indice = Convert.ToInt16(Cmb_Estatus_Ejecucion.SelectedIndex);
            switch (Indice)
            {
                case 1:
                    Div_Remates.Visible = true;
                    Div_Abonos.Visible = false;
                    Div_Notificaciones.Visible = false;
                    Div_Publicaciones.Visible = false;
                    Div_Gastos.Visible = false;
                    break;
                case 2:
                    Div_Notificaciones.Visible = true;
                    Div_Gastos.Visible = true;
                    Div_Publicaciones.Visible = false;
                    Div_Abonos.Visible = false;
                    Div_Remates.Visible = false;
                    break;
                case 3:
                    Div_Publicaciones.Visible = true;
                    Div_Abonos.Visible = false;
                    Div_Notificaciones.Visible = false;
                    Div_Gastos.Visible = false;
                    Div_Remates.Visible = false;
                    break;
                case 4:
                case 5:
                    Div_Abonos.Visible = true;
                    Div_Notificaciones.Visible = false;
                    Div_Publicaciones.Visible = false;
                    Div_Gastos.Visible = false;
                    Div_Remates.Visible = false;
                    Cargar_Datos_Cuenta_Predial(Session["CUENTA"].ToString());
                    Cargar_Combo_Despachos_Externos(Cmb_Despachos);
                    break;
                default:
                    Div_Abonos.Visible = false;
                    Div_Notificaciones.Visible = false;
                    Div_Publicaciones.Visible = false;
                    Div_Gastos.Visible = false;
                    Div_Remates.Visible = false;
                    break;
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN: Metodo para cargar el costo del gasto de ejecucion en un TextBox
    ///PARAMETROS: 
    ///CREO: Angel Antonio Escamilla Trejo 
    ///FECHA_CREO: 23/03/2012 04:53:12 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Gastos_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable Dt_Gastos = (DataTable)Session["Gastos"];
            String Valor = Cmb_Gastos_Ejecucion.SelectedItem.Text;
            for (int Cont_Gastos = 0; Cont_Gastos < Dt_Gastos.Rows.Count; Cont_Gastos++)
            {
                if (Valor == Dt_Gastos.Rows[Cont_Gastos]["NOMBRE"].ToString() && Dt_Gastos.Rows[Cont_Gastos]["COSTO"].ToString() != null)
                {
                    Txt_Costo.Text = Dt_Gastos.Rows[Cont_Gastos]["COSTO"].ToString();
                    Txt_Costo.ReadOnly = true;
                    break;
                }
                else
                {
                    Txt_Costo.ReadOnly = false;
                    Txt_Costo.Text = "";
                    break;
                }

            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN: Regresa a la Bandeja de Caja.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Tipo_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Cmb_Tipo_Pago.Text == "PAGO TOTAL")
        {
            Cmb_Bimestre_Final.SelectedIndex = (Cmb_Bimestre_Final.Items.Count - 1);
            Cmb_Anio_Final.SelectedIndex = (Cmb_Anio_Final.Items.Count - 1);
            Cmb_Bimestre_Final.Enabled = false;
            Cmb_Anio_Final.Enabled = false;
            if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
            {
                Grid_Listado_Adeudos.DataSource = new DataTable();
                Grid_Listado_Adeudos.DataBind();
                Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
            }
        }
        else
        {
            Cmb_Bimestre_Final.Enabled = true;
            Cmb_Anio_Final.Enabled = true;
            Cmb_Bimestre_Final.Focus();
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Bimestre_Final_SelectedIndexChanged
    ///DESCRIPCIÓN: Actualiza el Listado de los Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Bimestre_Final_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Grid_Listado_Adeudos.DataSource = new DataTable();
            Grid_Listado_Adeudos.DataBind();
            Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_Final_SelectedIndexChanged
    ///DESCRIPCIÓN: Actualiza el Listado de los Adeudos.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Anio_Final_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Hdf_Cuenta_Predial_ID.Value.Trim().Length > 0)
        {
            Grid_Listado_Adeudos.DataSource = new DataTable();
            Grid_Listado_Adeudos.DataBind();
            Llenar_Grid_Adeudos(Hdf_Cuenta_Predial_ID.Value, true);
        }
    }
    #endregion

    #region Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Gasto_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Agregar_Gasto_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Honorarios;
        Mensaje_Error();
        if (Session["Honorarios"] != null)
        {
            Dt_Honorarios = (DataTable)Session["Honorarios"];
        }
        else
        {
            Dt_Honorarios = Crear_Tabla_Honorarios();
        }
        if (Validaciones(6))
        {
            Llenar_DataRow_Honorarios(Dt_Honorarios);
            Grid_Gastos_Ejecucion.DataSource = Dt_Honorarios;
            Grid_Gastos_Ejecucion.DataBind();
            //Cargar_Combo_Gastos_Ejecucion();
            Cmb_Gastos_Ejecucion.SelectedIndex = 0;
            Txt_Costo.Text = "";
            Session["Honorarios"] = Dt_Honorarios;
        }
        else
        {
            IBtn_Imagen_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Agregar_Publicacion_Click
    ///DESCRIPCIÓN          : Llena el Grid de Las Publicaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 06/03/2012 05:25:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Agregar_Costo_Publicacion_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Publicaciones;
        Mensaje_Error();

        if (Session["Publicaciones"] != null)
        {
            Dt_Publicaciones = (DataTable)Session["Publicaciones"];
        }
        else
        {
            Dt_Publicaciones = Crear_Tabla_Publicaciones();
        }
        if (Validaciones(3))
        {
            Llenar_DataRow_Publicaciones(Dt_Publicaciones);
            Grid_Publicacion.DataSource = Dt_Publicaciones;
            Grid_Publicacion.DataBind();
            Session["Publicaciones"] = Dt_Publicaciones;
            Txt_Publicacion.Text = "";
            Txt_Fecha_Publicacion.Text = "";
            Txt_Parte.Text = "";
            Txt_Tomo.Text = "";
            Txt_Foja.Text = "";
            Txt_Pagina.Text = "";
        }
        else
        {
            IBtn_Imagen_Error.Visible = true;
            Div_Contenedor_Msj_Error.Visible = true;
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Aceptar_Click
    ///DESCRIPCIÓN          : Acepta el motivo de omision
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 13/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Aceptar_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Correcto = true;
        Mensaje_Error();
        try
        {
            Int16 Indice = Convert.ToInt16(Cmb_Estatus_Ejecucion.SelectedIndex);

            switch (Indice)
            {
                case 1:
                    if (Validaciones(1)) { Guardar_Remates(); }
                    else Correcto = false;
                    break;
                case 2:
                    if (Validaciones(2)) { Guardar_Notificacion(); }
                    else Correcto = false;
                    break;
                case 3:
                    if (Validaciones(3) || Grid_Publicacion.Rows.Count > 0) { }
                    else Correcto = false;
                    break;
                case 4:
                case 5:
                    if (Validaciones(4)) { Guardar_Parametros_Pasar_Etapa(); }
                    else Correcto = false;
                    break;
                default:
                    Mensaje_Error("Selecciona en estatus");
                    Correcto = false;
                    break;

            }
            if (Correcto)
            {
                Cerrar_Guardar();
            }
            else
            {
                IBtn_Imagen_Error.Visible = true;
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Regresar_Click
    ///DESCRIPCIÓN          : Evento Click del control Button Regresar
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 14/Marzo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Btn_Regresar_Click(object sender, ImageClickEventArgs e)
    {
        Session["AGREGA_GASTOS"] = false;
        Session.Remove("Honorarios");
        Session.Remove("Notificaciones");
        Session.Remove("Publicaciones");
        Session.Remove("Remates");
        Session.Remove("Abonos");
        Session.Remove("NO_DETALLE_ETAPA");
        Session.Remove("Adeudo");
        Session.Remove("CUENTA");
        Session.Remove("Etapa_Pae");
        Session.Remove("Despacho_ID");
        Session.Remove("Entrega");
        Session.Remove("Etapa_Pae");
        //Cierra la ventana
        string Pagina = "<script language='JavaScript'>";
        Pagina += "window.close();";
        Pagina += "</script>";
        ClientScript.RegisterStartupScript(this.GetType(), "Cerrar_Script", Pagina);
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Pago_Click
    ///DESCRIPCIÓN: Recalcula el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ejecutar_Pago_Click(object sender, EventArgs e)
    {
        DataTable Dt_Formas_Pago = new DataTable(); //Obtiene las formas de pago con las cuales fue cubierto el importe del ingreso
        DataRow Renglon;
        Cls_Ope_Caj_Pagos_Negocio Rs_Alta_Ope_Caj_Pagos = new Cls_Ope_Caj_Pagos_Negocio(); //Variable de conexión hacia la capa de negocios para envio de los datos a dar de alta
        try
        {
            Dt_Formas_Pago.Columns.Add(new DataColumn("Forma_Pago", typeof(String)));
            Dt_Formas_Pago.Columns.Add(new DataColumn("Monto", typeof(Double)));


            //Agrega los datos de pago por internet
            Renglon = Dt_Formas_Pago.NewRow();
            Renglon["Forma_Pago"] = "EFECTIVO";
            Renglon["Monto"] = Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$", ""));
            Dt_Formas_Pago.Rows.Add(Renglon);

            //Si tiene ajuste tarifario
            if (Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$", "")) != 0) //Ajuste tarfario
            {
                //Agrega los datos del ajuste tarifario
                Renglon = Dt_Formas_Pago.NewRow();
                Renglon["Forma_Pago"] = "AJUSTE TARIFARIO";
                Renglon["Monto"] = Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$", ""));
                Dt_Formas_Pago.Rows.Add(Renglon);
            }

            Rs_Alta_Ope_Caj_Pagos.P_No_Recibo = "";
            Rs_Alta_Ope_Caj_Pagos.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Rs_Alta_Ope_Caj_Pagos.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Rs_Alta_Ope_Caj_Pagos.P_Referencia = Txt_Cuenta_Predial.Text.Trim();

            //Consulta el id de la caja para pagos por internet.
            Cls_Ope_Pre_Parametros_Negocio Caja_Pago_Internet = new Cls_Ope_Pre_Parametros_Negocio();
            Cls_Cat_Pre_Cajas_Negocio Caja_Pago = new Cls_Cat_Pre_Cajas_Negocio();
            String Caja_Id;
            Caja_Id = Caja_Pago_Internet.Consultar_Parametro_Caja_Pagos_Pae().Rows[0][Ope_Pre_Parametros.Campo_Caja_Id_Pago_Pae].ToString();
            if (Caja_Id == "")
            {
                throw new Exception("Error: No se puede realizar el pago, porque no esta establecida la caja de pago, favor de verificarlo.");
            }
            Caja_Pago.P_Caja_ID = Caja_Id;
            Caja_Pago = Caja_Pago.Consultar_Datos_Caja();
            Rs_Alta_Ope_Caj_Pagos.P_Caja_ID = Caja_Pago.P_Caja_ID;
            Rs_Alta_Ope_Caj_Pagos.P_No_Caja = Caja_Pago.P_Numero_De_Caja.ToString();
            Rs_Alta_Ope_Caj_Pagos.P_No_Turno = "";
            Rs_Alta_Ope_Caj_Pagos.P_Fecha_Pago = DateTime.Now;
            Rs_Alta_Ope_Caj_Pagos.P_Ajuste_Tarifario = Convert.ToDouble(Txt_Ajuste_Tarifario.Text.Replace("$", ""));
            Rs_Alta_Ope_Caj_Pagos.P_Total_Pagar = Convert.ToDouble(Txt_Total_Pagar.Text.Replace("$", ""));
            Rs_Alta_Ope_Caj_Pagos.P_Empleado_ID = "";
            Rs_Alta_Ope_Caj_Pagos.P_Nombre_Usuario = "";
            Rs_Alta_Ope_Caj_Pagos.P_Monto_Corriente = Convert.ToDouble(Txt_Adeudo_Actual.Text.Replace("$", ""));
            Rs_Alta_Ope_Caj_Pagos.P_Dt_Formas_Pago = Dt_Formas_Pago;
            DataTable Dt_Adeudos = Crear_Tabla_Adeudos();
            DataTable Dt_Totales = Crear_Tabla_Totales();
            Rs_Alta_Ope_Caj_Pagos.P_Dt_Adeudos_Predial_Cajas = Crear_Tabla_Adeudos();
            Rs_Alta_Ope_Caj_Pagos.P_Dt_Adeudos_Predial_Cajas_Detalle = Crear_Tabla_Totales();
            Rs_Alta_Ope_Caj_Pagos.P_Dt_Pasivos = Registrar_Pasivos();

            Rs_Alta_Ope_Caj_Pagos.Alta_Pago_Pae(); //Da de alta el pago del ingreso
            Actualiza_PAE(Cmb_Estatus_Ejecucion.SelectedItem.Text);
            Session["Abonos"] = "SI";
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago de Predial por Internet", "alert('Pago realizado exitosamente.');", true);
            //Imprimir_Comprobante_Pago();
            Limpiar_Generales();
            Limpiar_Calculos();
            Limpiar_Calculos_Pago();
            Configuracion_Formulario(false);
            Cmb_Bimestre_Final.Enabled = false;
            Cmb_Anio_Final.Enabled = false;
        }
        catch (Exception Exc)
        {
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Pago de Predial por Internet", "alert('Error: [" + Exc.Message.Split(new String[] { " ." }, StringSplitOptions.None)[0] + "].');", true);
        }
    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Pago_Click
    ///DESCRIPCIÓN: Recalcula el Pago.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 24 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Seleccion_Adeudo_CheckedChanged(object sender, EventArgs e)
    {
        Int32 No_Fila = Convert.ToInt32(((CheckBox)(sender)).TabIndex);
        for (Int32 Contador = 0; Contador < Grid_Listado_Adeudos.Rows.Count; Contador++)
        {
            if (Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo_Tmp = (CheckBox)Grid_Listado_Adeudos.Rows[Contador].FindControl("Chk_Seleccion_Adeudo");
                if (Contador < No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = true;
                }
                else if (Contador > No_Fila)
                {
                    Chk_Seleccion_Adeudo_Tmp.Checked = false;
                }
            }
        }
        Realizar_Calculos_Pago(false);
    }
    #endregion
    #endregion

    #region Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Listado_Adeudos_RowDataBound
    ///DESCRIPCIÓN: Evento RowDataBound del Grid de Adeudos sin Convenio.
    ///PARAMETROS: 
    ///CREO: Francisco Antonio Gallardo Castañeda.  
    ///FECHA_CREO: 25 Agosto 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Listado_Adeudos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (e.Row.FindControl("Chk_Seleccion_Adeudo") != null)
            {
                CheckBox Chk_Seleccion_Adeudo = (CheckBox)e.Row.FindControl("Chk_Seleccion_Adeudo");
                Chk_Seleccion_Adeudo.TabIndex = (short)e.Row.RowIndex;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Publicacion_SelectedIndexChanged
    ///DESCRIPCIÓN          : Borra un Registro del Grid Publicacion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Publicacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Publicacion = (DataTable)Session["Publicaciones"];
        Dt_Publicacion.Rows.RemoveAt(Grid_Publicacion.SelectedIndex);
        Grid_Publicacion.DataSource = Dt_Publicacion;
        Grid_Publicacion.DataBind();
        Session["Publicaciones"] = Dt_Publicacion;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Publicacion_PageIndexChanging
    ///DESCRIPCIÓN          : Cambia de pagina en el Grid Publicacion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Publicacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Publicacion.PageIndex = e.NewPageIndex;
            Grid_Publicacion.DataSource = Session["Publicaciones"];
            Grid_Publicacion.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Gastos_Ejecucion_SelectedIndexChanged
    ///DESCRIPCIÓN          : Borra un Registro del Grid Gastos Ejecucion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Honorarios = (DataTable)Session["Honorarios"];
        Dt_Honorarios.Rows.RemoveAt(Grid_Gastos_Ejecucion.SelectedIndex);
        Grid_Gastos_Ejecucion.DataSource = Dt_Honorarios;
        Grid_Gastos_Ejecucion.DataBind();
        Session["Honorarios"] = Dt_Honorarios;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Gastos_Ejecucion_PageIndexChanging
    ///DESCRIPCIÓN          : Cambia de pagina en el Grid Gastos de ejecucion
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/03/2012 05:53:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Gastos_Ejecucion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Gastos_Ejecucion.PageIndex = e.NewPageIndex;
            Grid_Gastos_Ejecucion.DataSource = Session["Honorarios"];
            Grid_Gastos_Ejecucion.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion
}
