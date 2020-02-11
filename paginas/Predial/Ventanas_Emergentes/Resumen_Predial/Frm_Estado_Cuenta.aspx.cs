using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Reportes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Resumen_Predial_Frm_Estado_Cuenta : System.Web.UI.Page
{
    private static Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {

            //if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Habilitar_Botones();
                //Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones
                Txt_Cuenta_Predial.Text = Request.QueryString["Cuenta_Predial"].ToString();// Session["Cuenta_Predial"].ToString().Trim();
                Hdn_Cuenta_Predial.Value = Txt_Cuenta_Predial.Text;
                Cargar_Datos();

            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    protected void Habilitar_Botones()
    {
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Nombre_Propietario.Enabled = false;
        Txt_Rfc_Propietario.Enabled = false;


        Txt_Periodo_Inicial.Enabled = false;
        Txt_Periodo_Rezago.Enabled = false;
        //Txt_Adeudo_Rezago.Enabled = false;
        Txt_Periodo_Actual.Enabled = false;
        //Txt_Adeudo_Actual.Enabled = false;
        Txt_Total_Recargos_Ordinarios.Enabled = false;
        Txt_Honorarios.Enabled = false;
        Txt_Recargos_Moratorios.Enabled = false;
        Txt_Gastos_Ejecucion.Enabled = false;


        Txt_Subtotal.Enabled = false;
        Txt_Descuento_Pronto_Pago.Enabled = false;
        Txt_Descuento_Recargos_Ordinarios.Enabled = false;
        Txt_Descuento_Recargos_Moratorios.Enabled = false;
        //Txt_Descuento_Honorarios.Enabled = false;
        Txt_Total.Enabled = false;
        Txt_Adeudo_Actual.Enabled = false;
        Txt_Adeudo_Rezago.Enabled = false;

    }
    /////*******************************************************************************
    /////NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    /////DESCRIPCIÓN          : Muestra los datos de la consulta
    /////PARAMETROS:     
    /////CREO                 : Antonio Salvador Benavides Guardado
    /////FECHA_CREO           : 29/Junio/2011
    /////MODIFICO:
    /////FECHA_MODIFICO
    /////CAUSA_MODIFICACIÓN
    /////*******************************************************************************
    //protected void Btn_Mostrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    //{
    //    Boolean Busqueda_Ubicaciones;
    //    String Cuenta_Predial_ID;
    //    String Cuenta_Predial;

    //    Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
    //    if (Busqueda_Ubicaciones)
    //    {
    //        if (Session["CUENTA_PREDIAL_ID"] != null)
    //        {
    //            Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
    //            //Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
    //            Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
    //            Txt_Cuenta_Predial.Text = Cuenta_Predial;
    //        }
    //    }
    //    Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
    //    Session.Remove("CUENTA_PREDIAL_ID");
    //    Session.Remove("CUENTA_PREDIAL");

    //    Consultar_Datos_Cuenta_Constancia();
    //}
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Encabezado_Error.Text = "";
    }
    private void Mensaje_Error(String P_Mensaje)
    {

        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Cuota_Fija
    ///DESCRIPCIÓN: asignar datos cuota fijao
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 05/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Datos_Cuota_Fija(String Cuota_Fija_ID)
    {
        DataTable Dt_Cuota_Detalles;
        try
        {
            Dt_Cuota_Detalles = M_Orden_Negocio.Consultar_Cuota_Fija_Detalles();
            if (Dt_Cuota_Detalles.Rows.Count - 1 > 0)
            {
                //if (Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString() != string.Empty)
                //{
                //    Txt_Tasa_General.Text = Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString();
                //}
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Cuota_Fija: " + Ex.Message);
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
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {

            //Asignacion de valores a Objeto de Negocio y cajas de texto            
            //M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            //Hdn_Cuenta_ID.Value = dataTable.Rows[0]["ID"].ToString();
            //M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["ID"].ToString();
            //Session["Cuenta_Predial_ID"] = dataTable.Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
            Txt_Cuenta_Predial.Text = Hdn_Cuenta_Predial.Value;
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();



            //Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            //{

            //    Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
            //}

            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
                DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                //v
                if (Dt_Estado_Propietario.Rows.Count > 0)
                {
                    //Txt_Estado_Propietario.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    //M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                }
            }


            String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
            RP_Negocio.P_Cuenta_Predial = Cuenta_Predial_ID;

            Llenar_Grid_Estado_Cuenta(Cuenta_Predial_ID, false, 0, false);
            DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
            Llenar_Combo_Anios(Dt_Adeudos);


            //Asignacion de valores a Objeto de Negocio y cajas de texto            
            //M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            //Hdn_Cuenta_ID.Value = dataTable.Rows[0]["ID"].ToString();
            //M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["ID"].ToString();
            Session["Cuenta_Predial_ID"] = dataTable.Rows[0]["Cuenta_Predial_Id"].ToString().Trim();
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();


            M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
                Txt_Tipo_Predio_General.Text = Dt_Tipo_Predio.Rows[0]["Descripcion"].ToString().Trim();
                //M_Orden_Negocio.P_Tipo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
                Txt_Uso_Predio_General.Text = Dt_Uso_Suelo.Rows[0]["Descripcion"].ToString();
                //M_Orden_Negocio.P_Uso_Suelo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
            }
            if (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0]["Estado_ID_Notificacion"].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
                Txt_Estado_Propietario.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
                //M_Orden_Negocio.P_Estado_Predial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
                DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
                if (Dt_Tasa.Rows.Count > 0)
                {
                    Txt_Tasa_General.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
                }
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Calle_Propietario.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia_Propietario.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
                //M_Orden_Negocio.P_Estado_Predial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            {
                //Txt_Estatus_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
                M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            }
            //if (!string.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString()))
            //Txt_Costo_M2.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString();

            //Txt_Supe_Construida_General.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();
            //M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            //Txt_Super_Total_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            //M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            if (dataTable.Rows[0]["Nombre_Calle"].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                //Txt_Colonia_General.Text = Dt_Colonia.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                //M_Orden_Negocio.P_Colonia_Cuenta = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                //Cmb_Colonia_Cuenta_SelectedIndexChanged(null, EventArgs.Empty);
                Txt_Ubicacion_General.Text = dataTable.Rows[0]["Nombre_Calle"].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            if (dataTable.Rows[0]["Nombre_Colonia"].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                //Txt_Colonia_General.Text = Dt_Colonia.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                //M_Orden_Negocio.P_Colonia_Cuenta = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                //Cmb_Colonia_Cuenta_SelectedIndexChanged(null, EventArgs.Empty);
                Txt_Colonia_General.Text = dataTable.Rows[0]["Nombre_Colonia"].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            Txt_Numero_Exterior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            Txt_Numero_Interior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            //Txt_Clave_Catastral_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            //M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            {
                Txt_Efectos_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
                //M_Orden_Negocio.P_Efectos_Año = Convert.ToInt32(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos]);
            }
            decimal Valor_Fiscal;
            decimal.TryParse(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString(), out Valor_Fiscal);
            Txt_Valor_Fiscal_General.Text = Valor_Fiscal.ToString("$#,##0.00");
            M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            //Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            //M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            decimal Cuota_Anual;
            decimal.TryParse(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString(), out Cuota_Anual);
            Hdn_Cuota_Anual.Value = Cuota_Anual.ToString("0.##");
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            {
                //Txt_Cuota_Anual_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString();
                Txt_Cuota_Bimestral_General.Text = "$ " + String.Format("{0:#,##0.00}", Convert.ToDecimal(Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6));
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            {
                M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
                M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            }
            //Txt_Porciento_Exencion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            {
                M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            }
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null)
            //{
            //    M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
            //    Txt_Fecha_Avaluo_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString();
            //}
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null)
            //{
            //    Txt_Fecha_Termino_Extencion.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString();
            //    M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
            //}
            //Txt_Dif_Construccion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
            //M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();

            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            {
                M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
                //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            }
            //Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            //if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            //{

            //    Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
            //}
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            {
                M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
                Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            {
                //Cmb_Domicilio_Foraneo.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
                //M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
                DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                //v
                if (Dt_Estado_Propietario.Rows.Count > 0)
                {
                    Txt_Estado_Propietario.Text = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                    M_Orden_Negocio.P_Estado_Propietario = (Dt_Estado_Propietario.Rows[0]["Descripcion"].ToString());
                }
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_Notificacion"].ToString()))
            {
                Txt_Estado_Propietario.Text = dataTable.Rows[0]["Estado_Notificacion"].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
                DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia_Propietario.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
            }
            else
            {
                Txt_Colonia_Propietario.Text = dataTable.Rows[0]["Colonia_Notificacion"].ToString();
            }

            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Ciudad_ID = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
                DataTable Dt_Ciudad_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ciudad();
                Txt_Ciudad_Propietario.Text = Dt_Ciudad_Propietario.Rows[0]["Nombre"].ToString();
                M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["Ciudad_ID_Notificacion"].ToString();
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0]["Ciudad_Notificacion"].ToString()))
            {
                Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["Ciudad_Notificacion"].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
                DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Calle_Propietario.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
            }
            else
            {
                Txt_Calle_Propietario.Text = dataTable.Rows[0]["Calle_Notificacion"].ToString();
            }
            Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            Txt_Cod_Pos_Propietario.Text = dataTable.Rows[0]["Codigo_Postal"].ToString();
            M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }
    private void Busqueda_Cuentas()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataSet Ds_Cuenta;
        try
        {
            Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            //M_Orden_Negocio.P_Contrarecibo = Get_Contra_ID();
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
                //Limpiar_Todo();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
            }
            else
            {
                //Estado_Botones(Const_Estado_Inicial);
                //Limpiar_Todo();
                Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos
    ///DESCRIPCIÓN: asignar datos de cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
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
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Lugar_Pago
    ///DESCRIPCIÓN: Metodo que llena el Combo de Lugar de Pago del catalogo de Modulos.
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 10/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Combo_Anios(DataTable Dt_Anios)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        try
        {
            if (Dt_Anios.Rows.Count == 0)
            {
                if (Grid_Estado_Cuenta.Rows.Count > 0)
                {
                    DataRow Dr_Años;
                    foreach (GridViewRow Dr_Estado_Cuenta in Grid_Estado_Cuenta.Rows)
                    {
                        Dr_Años = Dt_Anios.NewRow();
                        Dr_Años["ANIO"] = Dr_Estado_Cuenta.Cells[0].Text;
                        Dt_Anios.Rows.Add(Dr_Años);
                    }
                }
            }
            Dt_Anios = Dt_Anios.DefaultView.ToTable(true, "ANIO");
            Cmb_Anio.DataSource = Dt_Anios;
            Cmb_Anio.DataTextField = "ANIO";
            Cmb_Anio.DataValueField = "ANIO";
            Cmb_Anio.SelectedIndex = (Cmb_Anio.Items.Count - 1);
            Cmb_Anio.DataBind();
            Cmb_Anio.SelectedIndex = Dt_Anios.Rows.Count - 1;
        }
        catch (Exception)
        {
            //Lbl_Ecabezado_Mensaje.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
            //throw new Exception("Resumen Predio " + ex.Message.ToString(), ex);
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    protected void Grid_Estado_Cuenta_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        Llenar_Grid_Estado_Cuenta(Cuenta_Predial_ID, false, e.NewPageIndex, true);
    }

    /// *************************************************************************************
    /// NOMBRE              : Grid_Estado_Cuenta_RowDataBound
    /// DESCRIPCIÓN         : Valida el Bimestre Final seleccionado para hacer cortes y recalcular Total por Anualidad
    /// PARÁMETROS:
    /// USUARIO CREO        : Antonio Salvador Benavides Guardado
    /// FECHA CREO          : 08/Noviembre/2011
    /// USUARIO MODIFICO:   
    /// FECHA MODIFICO:     
    /// CAUSA MODIFICACIÓN: 
    /// *************************************************************************************
    protected void Grid_Estado_Cuenta_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        int Cont_Bimestres = 0;
        int Bimestre_Final = Convert.ToInt16(Cmb_Bimestre.SelectedItem.Text);
        Double Sum_Anualidad = 0;

        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            if (Cmb_Anio.SelectedItem != null)
            {
                if (e.Row.Cells[0].Text.Trim() == Cmb_Anio.SelectedItem.Text)
                {
                    for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                    {
                        if (Cont_Bimestres > Bimestre_Final)
                        {
                            e.Row.Cells[Cont_Bimestres].Text = "$0.00";
                        }
                        if (HttpUtility.HtmlDecode(e.Row.Cells[Cont_Bimestres].Text).Trim() != "")
                        {
                            Sum_Anualidad += Convert.ToDouble(HttpUtility.HtmlDecode(e.Row.Cells[Cont_Bimestres].Text).Trim().Replace("$", ""));
                        }
                    }
                    e.Row.Cells[7].Text = Sum_Anualidad.ToString("$##,###,##0.00");
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Estado_Cuenta
    ///DESCRIPCIÓN: Metodo que llena el grid de l estado de cuenta
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 19/Agosto/2011 
    ///MODIFICO: Roberto González Oseguera
    ///FECHA_MODIFICO: 20-feb-2012
    ///CAUSA_MODIFICACIÓN: Agregar validaciones al cálculo de descuento por pronto pago
    ///             - sólo cuando es para los seis bimestres y no se han recibido pagos del año corriente
    ///*******************************************************************************
    protected void Llenar_Grid_Estado_Cuenta(String P_Cuenta_Predial, Boolean Tomar_Filtrado, int Page_Index, Boolean Paginacion)
    {
        Int32 Anio_Corriente = 0;
        var Consulta_Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        var Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        var RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        DataTable Dt_Estado_Cuenta = new DataTable();
        String Fecha_Anio = Cmb_Anio.SelectedValue.Trim();
        String Fecha_Mes = Cmb_Bimestre.SelectedValue.Trim();
        int Pagina_Grid = Grid_Estado_Cuenta.PageIndex;

        decimal Adeudo_Corriente = 0;
        decimal Adeudo_Rezago = 0;
        decimal Recargos_Ordinarios = 0;
        decimal Recargos_Moratorios = 0;
        decimal Honorarios = 0;
        String Periodo_Actual_Inicial = "";
        String Periodo_Actual_Final = "";
        String Periodo_Rezago_Inicial = "";
        String Periodo_Rezago_Final = "";
        Int16 Cont_Bimestres;
        int Hasta_Anio = 0;
        int hasta_Bimestre = 0;

        Grid_Estado_Cuenta.Columns[0].Visible = true;
        RP_Negocio.P_Cuenta_Predial = P_Cuenta_Predial;
        DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Actual.Text = "";

        // obtener año corriente
        Anio_Corriente = Consulta_Parametros.Consultar_Anio_Corriente();
        // verificar que se obtuvo valor mayor que cero, si no, tomar año actual
        if (Anio_Corriente <= 0)
        {
            Anio_Corriente = DateTime.Now.Year;
        }

        if (Fecha_Anio == "")
        {
            Fecha_Anio = "0";
        }

        Dt_Estado_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(P_Cuenta_Predial, null, 0, Convert.ToInt16(Fecha_Anio));

        // limpiar controles
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Actual.Text = "";
        Txt_Honorarios.Text = "";
        Txt_Gastos_Ejecucion.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "$ 0.00";
        Txt_Recargos_Moratorios.Text = "$ 0.00";
        Txt_Subtotal.Text = "$ 0.00";
        Txt_Descuento_Pronto_Pago.Text = "$ 0.00";
        Txt_Descuento_Recargos_Ordinarios.Text = "$ 0.00";
        Txt_Descuento_Recargos_Moratorios.Text = "$ 0.00";
        Txt_Total.Text = "$ 0.00";
        Txt_Periodo_Inicial.Text = "";
        Txt_Periodo_Rezago.Text = "";
        Txt_Periodo_Actual.Text = "";

        // tomar valores de combos periodo final si hay valores seleccionados
        if (Cmb_Anio.SelectedIndex >= 0 && Cmb_Bimestre.SelectedIndex >= 0)
        {
            int.TryParse(Cmb_Anio.SelectedValue, out Hasta_Anio);
            int.TryParse(Cmb_Bimestre.SelectedValue, out hasta_Bimestre);
        }

        for (int x = 0; x < Dt_Estado_Cuenta.Rows.Count; x++)
        {
            decimal Monto_Adeudo;
            int Anio_Adeudo;

            // recorrer los adeudos para extraer los periodos
            for (int Contador_Bimestre = 1; Contador_Bimestre <= 6; Contador_Bimestre++)
            {
                decimal.TryParse(Dt_Estado_Cuenta.Rows[x]["Adeudo_Bimestre_" + Contador_Bimestre].ToString().Trim(), out Monto_Adeudo);
                int.TryParse(Dt_Estado_Cuenta.Rows[x]["Anio"].ToString(), out Anio_Adeudo);
                // si hay adeudo
                if (Monto_Adeudo >= 0)
                {
                    // año rezago, tomar dato para periodo rezago y periodo inicial
                    if (Anio_Adeudo < Anio_Corriente)
                    {
                        if (Periodo_Rezago_Inicial.Length <= 0 && Monto_Adeudo > 0)
                        {
                            Periodo_Rezago_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                        }
                        Periodo_Rezago_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                    }
                    // año corriente, tomar dato para periodo corriente
                    if (Anio_Adeudo == Anio_Corriente)
                    {
                        if (Periodo_Actual_Inicial.Length <= 0 && Monto_Adeudo > 0)
                        {
                            Periodo_Actual_Inicial = Contador_Bimestre + "/" + Anio_Adeudo;
                        }
                        Periodo_Actual_Final = Contador_Bimestre + "/" + Anio_Adeudo;
                    }
                }
            }
        }

        // cargar el grid con los adeudos
        Session["Dt_Estado_Cuenta"] = Dt_Estado_Cuenta;
        Grid_Estado_Cuenta.DataSource = Dt_Estado_Cuenta;
        if (Paginacion)
        {
            Grid_Estado_Cuenta.PageIndex = Page_Index;
        }
        else
        {
            if (Pagina_Grid < Grid_Estado_Cuenta.PageCount)
            {
                Grid_Estado_Cuenta.PageIndex = Pagina_Grid;
            }
        }
        Grid_Estado_Cuenta.DataBind();
        // si el combo con los años de los adeudos está vacío, llenarlo
        if (Cmb_Anio.Items.Count == 0)
        {
            Llenar_Combo_Anios(Dt_Adeudos);
        }

        // cargar periodo inicial del periodo rezago o periodo corriente
        if (Periodo_Rezago_Inicial.Length > 0)
        {
            Txt_Periodo_Inicial.Text = Periodo_Rezago_Inicial;
        }
        else if (Periodo_Actual_Inicial.Length > 0)
        {
            Txt_Periodo_Inicial.Text = Periodo_Actual_Inicial;
        }
        // cargar cajas de texto de periodos rezago y corriente
        if (Periodo_Rezago_Inicial.Length > 0)
        {
            Txt_Periodo_Rezago.Text = Periodo_Rezago_Inicial + " - " + Periodo_Rezago_Final;
        }
        if (Periodo_Actual_Inicial.Length > 0)
        {
            Txt_Periodo_Actual.Text = Periodo_Actual_Inicial + " - " + Periodo_Actual_Final;
        }

        decimal Descuento_Pronto_Pago = 0;
        decimal Porcentaje_Descuento;
        decimal Subtotal = 0;
        decimal Total_Estado_Cuenta = 0;
        decimal Descuento_Recargos_Ordinarios = 0;
        decimal Descuento_Recargos_Moratorios = 0;
        decimal Cuota_Minima = 0;
        var GAP_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        var Cuotas_Minima = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        var Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        var Cuenta_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Pae_Honorarios_Negocio PAE_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();

        DataTable Dt_Cuotas_Minimas;
        DataTable Dt_Pagos;
        DataTable Dt_Datos_Cuenta;
        GAP_Negocio.Calcular_Recargos_Predial_Hasta_Bimestre(P_Cuenta_Predial, Convert.ToInt16(Fecha_Anio), Convert.ToInt16(Fecha_Mes));

        // obtener cuota minima
        Cuotas_Minima.P_Anio = DateTime.Now.Year.ToString();
        Dt_Cuotas_Minimas = Cuotas_Minima.Consultar_Cuotas_Minimas();
        if (Dt_Cuotas_Minimas != null && Dt_Cuotas_Minimas.Rows.Count > 0)
        {
            Decimal.TryParse(Dt_Cuotas_Minimas.Rows[0]["CUOTA"].ToString(), out Cuota_Minima);
        }

        if (GAP_Negocio.p_Total_Corriente == 0 && GAP_Negocio.p_Total_Rezago == 0)
        {
            if (Grid_Estado_Cuenta.Rows.Count > 0)
            {
                foreach (GridViewRow Dr_Estado_Cuenta in Grid_Estado_Cuenta.Rows)
                {
                    if (Dr_Estado_Cuenta.Cells[0].Text == DateTime.Now.Year.ToString())
                    {
                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Trim() != "")
                            {
                                GAP_Negocio.p_Total_Corriente += Convert.ToDecimal(Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Replace("$", ""));
                            }
                        }
                    }
                    else
                    {
                        for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Trim() != "")
                            {
                                GAP_Negocio.p_Total_Rezago += Convert.ToDecimal(Dr_Estado_Cuenta.Cells[Cont_Bimestres].Text.Replace("$", ""));
                            }
                        }
                    }
                }
            }
        }

        // tomar datos de la consulta al método calcular recargos en la clase generar adeudos predial
        Adeudo_Corriente = GAP_Negocio.p_Total_Corriente;
        Adeudo_Rezago = GAP_Negocio.p_Total_Rezago;
        Recargos_Ordinarios = GAP_Negocio.p_Total_Recargos_Generados;
        Txt_Adeudo_Actual.Text = String.Format("{0:c}", Adeudo_Corriente);
        Txt_Adeudo_Rezago.Text = String.Format("{0:c}", Adeudo_Rezago);
        Txt_Total_Recargos_Ordinarios.Text = String.Format("{0:c}", Recargos_Ordinarios);

        DataTable Dt_PAE_Honorarios = null;
        PAE_Honorarios.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Dt_PAE_Honorarios = PAE_Honorarios.Consultar_Total_Honorarios();
        if (Dt_PAE_Honorarios != null)
        {
            if (Dt_PAE_Honorarios.Rows.Count > 0)
            {
                if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"] != null)
                {
                    if (Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString().Trim() != "")
                    {
                        Honorarios = Convert.ToDecimal(Dt_PAE_Honorarios.Rows[0]["TOTAL_HONORARIOS"]);
                    }
                }
            }
        }
        Txt_Honorarios.Text = "$ " + Honorarios.ToString("#,##0.00");
        Txt_Gastos_Ejecucion.Text = "$ 0.00";

        // consultar y mostrar recargos moratorios
        var Consulta_Moratorios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        Consulta_Moratorios.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Recargos_Moratorios = Consulta_Moratorios.Obtener_Recargos_Moratorios();
        Txt_Recargos_Moratorios.Text = Recargos_Moratorios.ToString("#,##0.00");

        // calcular subtotal
        Subtotal = Adeudo_Rezago + Adeudo_Corriente + Recargos_Ordinarios + Recargos_Moratorios + Honorarios;
        Txt_Subtotal.Text = "$ " + Subtotal.ToString("#,##0.00");

        // consultar pagos de la cuenta
        Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Consultar_Pagos.p_Estatus = "PAGADO";
        Consultar_Pagos.p_Periodo_Corriente = " LIKE '%" + Anio_Corriente + "%'";
        Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Predial_Por_Periodo();

        // consultar si la cuenta tiene beneficio
        string Cuota_Fija = "";
        Cuenta_Predial.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        Dt_Datos_Cuenta = Cuenta_Predial.Consultar_Cuenta();
        if (Dt_Datos_Cuenta != null && Dt_Datos_Cuenta.Rows.Count > 0)
        {
            Cuota_Fija = Dt_Datos_Cuenta.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString().Trim().ToUpper();
        }

        // solo si el adeudo consultado es hasta el sexto bimestre del año corriente, calcular descuento por pronto pago (no debe tener cuota fija)
        if (Txt_Periodo_Actual.Text.Contains("6/" + Anio_Corriente) && (Dt_Pagos == null || Dt_Pagos.Rows.Count == 0) && !Cuota_Fija.Contains("SI"))
        {
            DataTable Dt_Descuento_Pronto_Pago = Resumen_Predio.Consultar_Descuentos_Pronto_Pago();
            if (Dt_Descuento_Pronto_Pago != null && Dt_Descuento_Pronto_Pago.Rows.Count > 0)
            {
                decimal.TryParse(
                    Dt_Descuento_Pronto_Pago.Rows[0][String.Format("{0:MMMM}", DateTime.Now)].ToString().Trim(),
                    out Porcentaje_Descuento);
                Descuento_Pronto_Pago = Adeudo_Corriente * Porcentaje_Descuento / 100;
                // validar que el descuento restado al adeudo corriente no sea menor que la cuota minima
                if (Cuota_Minima > 0 && Adeudo_Corriente - Descuento_Pronto_Pago < Cuota_Minima)
                {
                    Descuento_Pronto_Pago = Adeudo_Corriente - Cuota_Minima;
                }
                // validar descuentos negativos
                if (Descuento_Pronto_Pago < 0)
                {
                    Descuento_Pronto_Pago = 0;
                }
                // mostrar resultado
                Txt_Descuento_Pronto_Pago.Text = "$ " + Descuento_Pronto_Pago.ToString("#,##0.00");
            }
        }
        else
        {
            Txt_Descuento_Pronto_Pago.Text = "$ 0.00";
        }
        // consultar descuento a recargos
        Resumen_Predio.P_Cuenta_Predial_ID = P_Cuenta_Predial;
        DataTable Dt_Descuentos_Recargos = Resumen_Predio.Consultar_Descuentos_Recargos();
        if (Dt_Descuentos_Recargos != null && Dt_Descuentos_Recargos.Rows.Count > 0)
        {
            decimal.TryParse(Dt_Descuentos_Recargos.Rows[0]["Desc_Recargo"].ToString().Trim(), out Descuento_Recargos_Ordinarios);
            decimal.TryParse(Dt_Descuentos_Recargos.Rows[0]["Desc_Recargo_Moratorio"].ToString(), out Descuento_Recargos_Moratorios);

        }
        Txt_Descuento_Recargos_Ordinarios.Text = "$ " + Descuento_Recargos_Ordinarios.ToString("#,##0.00");
        Txt_Descuento_Recargos_Moratorios.Text = "$ " + Descuento_Recargos_Moratorios.ToString("#,##0.00");

        // adeudo total (aplicar los descuentos solo a los recargos correspondientes)
        Total_Estado_Cuenta = Subtotal - Descuento_Pronto_Pago
            - (Recargos_Ordinarios - Descuento_Recargos_Ordinarios < 0 ? Recargos_Ordinarios : Descuento_Recargos_Ordinarios)
            - (Recargos_Moratorios - Descuento_Recargos_Moratorios < 0 ? Recargos_Moratorios : Descuento_Recargos_Moratorios);
        Txt_Total.Text = "$ " + String.Format("{0:#,##0.00}", Convert.ToDecimal(Total_Estado_Cuenta.ToString()));
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Propietario
    ///DESCRIPCIÓN: Recuperar los datos del propietatio y los guarda en ena variable de sesión
    ///PARAMETROS: 
    ///CREO: Chistian Perez
    ///FECHA_CREO: 10-Ago-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
            //M_Orden_Negocio.P_Contrarecibo = Get_Contra_ID();
            Ds_Prop = M_Orden_Negocio.Consulta_Datos_Propietario();
            if (Ds_Prop.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Prop_Datos");
                Session["Ds_Prop_Datos"] = Ds_Prop;
                Cargar_Datos_Propietario(((DataSet)Session["Ds_Prop_Datos"]).Tables["Dt_Propietarios"]);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Propietario
    ///DESCRIPCIÓN: asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Propietario(DataTable dataTable)
    {
        try
        {
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                Hdn_Propietario_ID.Value = dataTable.Rows[0]["PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["PROPIETARIO"].ToString(); ;

                Txt_Nombre_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();

                Txt_Rfc_Propietario.Text = dataTable.Rows[0]["RFC"].ToString();
                M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
            }

        }
        catch
        {
            //Mensaje_Error("Cargar_Datos_Propietario: " + Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Anio_SelectedIndexChanged
    ///DESCRIPCIÓN: asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Anio_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        Llenar_Grid_Estado_Cuenta(Cuenta_Predial_ID, false, 0, false);
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Bimestre_SelectedIndexChanged
    ///DESCRIPCIÓN: asignar datos de propietario de la cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:44:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Bimestre_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Cuenta_Predial_ID = Session["Cuenta_Predial_ID"].ToString().Trim();
        Llenar_Grid_Estado_Cuenta(Cuenta_Predial_ID, false, 0, false);
    }
    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Estado_Cuenta_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos del estado de cuenta
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          29/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Estado_Cuenta_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        //Crea un nuevo data table
        DataTable Dt_Generales = new DataTable();
        DataTable Dt_Propietario = new DataTable();
        DataTable Dt_Impuestos = new DataTable();
        DataTable Dt_Grid_Estado_Cuenta = (DataTable)Session["Dt_Estado_Cuenta"];
        //LLenado de datos
        DataTable Dt_Estado_Cuenta = Asignar_Datos_Estado_Cuenta();
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Estado_Cuenta = new Ds_Pre_Resumen_Predio_Generales();
        //obtiene el numero de cuenta predial
        Imprimir_Resumen_Predio_Estado_Cuenta.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        //manda a llamar a la consulta para traer los datos
        Dt_Generales = Imprimir_Resumen_Predio_Estado_Cuenta.Consultar_Imprimir_Resumen_Generales();
        Dt_Impuestos = Asignar_Datos_Impuestos();
        Dt_Propietario = Asignar_Datos_Propietarios();
        Dt_Generales.TableName = "Dt_Generales";
        Dt_Estado_Cuenta.TableName = "Dt_Estado_Cuenta";
        Dt_Propietario.TableName = "Dt_Propietario";
        Dt_Impuestos.TableName = "Dt_Impuestos";
        Dt_Grid_Estado_Cuenta.TableName = "Dt_Grid_Estado_Cuenta";
        Resumen_Predio_Estado_Cuenta.Clear();
        Resumen_Predio_Estado_Cuenta.Tables.Clear();
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Generales.Copy());
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Estado_Cuenta.Copy());
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Propietario.Copy());
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Impuestos.Copy());
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Grid_Estado_Cuenta.Copy());
        Nombre_Reporte = "Reporte Estado Cuenta";
        Nombre_Repote_Crystal = "Rpt_Pre_Estado_Cuenta_Cobro_anual.rpt";
        //Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Estado_Cuenta.rpt";
        Formato = "PDF";
        //llama el metodo con los parametros de la consulta y el data set
        Generar_Reportes(Resumen_Predio_Estado_Cuenta, Nombre_Repote_Crystal, Nombre_Reporte, Formato);

    }
    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Propietarios
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Propietarios()
    {
        DataTable Dt_Propietario = new DataTable();
        DataRow Propietarios;
        Dt_Propietario.Columns.Add("Nombre");

        Dt_Propietario.Columns.Add("Rfc");
        Dt_Propietario.Columns.Add("Colonia");
        Dt_Propietario.Columns.Add("Calle");
        Dt_Propietario.Columns.Add("Numero_Exterior");
        Dt_Propietario.Columns.Add("Numero_Interior");
        Dt_Propietario.Columns.Add("Estado");
        Dt_Propietario.Columns.Add("Ciudad");
        Dt_Propietario.Columns.Add("Cod_Pos");
        Propietarios = Dt_Propietario.NewRow();
        Propietarios["Nombre"] = Txt_Nombre_Propietario.Text.Trim();

        Propietarios["Rfc"] = Txt_Rfc_Propietario.Text.Trim();
        Propietarios["Colonia"] = Txt_Colonia_Propietario.Text.Trim();
        Propietarios["Calle"] = Txt_Calle_Propietario.Text.Trim();
        Propietarios["Numero_Exterior"] = Txt_Numero_Exterior_Propietario.Text.Trim();
        Propietarios["Numero_Interior"] = Txt_Numero_Interior_Propietario.Text.Trim();
        Propietarios["Estado"] = Txt_Estado_Propietario.Text.Trim();
        Propietarios["Ciudad"] = Txt_Ciudad_Propietario.Text.Trim();
        Propietarios["Cod_Pos"] = Txt_Cod_Pos_Propietario.Text.Trim();
        if (Dt_Propietario.Rows.Count == 0)
        {
            Dt_Propietario.Rows.InsertAt(Propietarios, 0);

        }
        return Dt_Propietario;

    }
    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Estado_Cuenta
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Estado_Cuenta()
    {
        String Total_Construccion = String.Empty;
        DataTable Dt_Estado_Cuenta = new DataTable();
        try
        {

            DataRow Estado_Cuenta;
            Dt_Estado_Cuenta.Columns.Add("Periodo_Rezago");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Rezago", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Periodo_Actual");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Actual", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Total_Recargos_Ordinarios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Honorarios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Recargos_Moratorios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Gastos_Ejecucion", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Subtotal", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Descuentos_Pronto_Pago", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Ordinarios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Moratorios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Descuento_Honorarios", typeof(Double));
            Dt_Estado_Cuenta.Columns.Add("Total", typeof(Double));

            Estado_Cuenta = Dt_Estado_Cuenta.NewRow();
            Estado_Cuenta["Periodo_Rezago"] = Txt_Periodo_Rezago.Text.Trim();
            //Estado_Cuenta["Adeudo_Rezago"] = Txt_Adeudo_Rezago.Text.Trim();
            Estado_Cuenta["Periodo_Actual"] = Txt_Periodo_Actual.Text.Trim();
            //Estado_Cuenta["Adeudo_Actual"] = Txt_Adeudo_Actual.Text.Trim();
            Estado_Cuenta["Total_Recargos_Ordinarios"] = Txt_Total_Recargos_Ordinarios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Honorarios"] = Txt_Honorarios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Recargos_Moratorios"] = Txt_Recargos_Moratorios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Gastos_Ejecucion"] = Txt_Gastos_Ejecucion.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Subtotal"] = Txt_Subtotal.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Descuentos_Pronto_Pago"] = Txt_Descuento_Pronto_Pago.Text.Trim().Replace("$", "").Replace(",", "");
            //Detalles
            Estado_Cuenta["Adeudo_Rezago"] = Txt_Adeudo_Rezago.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Adeudo_Actual"] = Txt_Adeudo_Actual.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Descuento_Recargos_Ordinarios"] = Txt_Descuento_Recargos_Ordinarios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Descuento_Recargos_Moratorios"] = Txt_Descuento_Recargos_Moratorios.Text.Trim().Replace("$", "").Replace(",", "");
            Estado_Cuenta["Total"] = Txt_Total.Text.Trim().Replace("$", "").Replace(",", "");

            if (Dt_Estado_Cuenta.Rows.Count == 0)
            {
                Dt_Estado_Cuenta.Rows.InsertAt(Estado_Cuenta, 0);

            }
        }
        catch (Exception)
        {
        }
        return Dt_Estado_Cuenta;

    }

    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Impuestos
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Impuestos()
    {
        String Total_Construccion = String.Empty;
        DataTable Dt_Impuestos = new DataTable();
        try
        {

            DataRow Impuestos;
            Dt_Impuestos.Columns.Add("Valor_Fiscal", typeof(Double));
            Dt_Impuestos.Columns.Add("Tasa");
            Dt_Impuestos.Columns.Add("Periodo_Corriente");
            Dt_Impuestos.Columns.Add("Tipo_Predio");
            Dt_Impuestos.Columns.Add("Cuota_Bimestral", typeof(Double));
            Dt_Impuestos.Columns.Add("Cuota_Anual", typeof(Double));


            Impuestos = Dt_Impuestos.NewRow();
            Impuestos["Valor_Fiscal"] = Txt_Valor_Fiscal_General.Text.Trim().Replace("$", "").Replace(",", "");
            Impuestos["Tasa"] = Txt_Tasa_General.Text.Trim();
            Impuestos["Tipo_Predio"] = Txt_Tipo_Predio_General.Text.Trim();
            Impuestos["Cuota_Bimestral"] = Txt_Cuota_Bimestral_General.Text.Trim().Replace("$", "").Replace(",", "");
            Impuestos["Cuota_Anual"] = Hdn_Cuota_Anual.Value.Trim().Replace("$", "").Replace(",", "");
            if (Dt_Impuestos.Rows.Count == 0)
            {
                Dt_Impuestos.Rows.InsertAt(Impuestos, 0);

            }

        }
        catch (Exception)
        {
        }
        return Dt_Impuestos;

    }
    /// *************************************************************************************
    /// NOMBRE:              Generar_Reportes
    /// DESCRIPCIÓN:         Metodo para generar el reporte
    /// PARÁMETROS:          Dataset a imprimir
    ///                      Nombre del reporte de Crystal
    ///                      Nombre como se llamara el reporte
    ///                      Formato del reporte
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    private void Generar_Reportes(DataSet Ds_Recibo, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
    {

        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";


        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../../../Rpt/Predial/Resumen_Predio/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Recibo, Ruta_Reporte_Crystal, "../../../../Reporte/", Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
    }
    /// *************************************************************************************
    /// NOMBRE:              Mostrar_Reporte
    /// DESCRIPCIÓN:         Muestra el reporte en pantalla.
    /// PARÁMETROS:          Nombre_Reporte_Generar.- Nombre que tiene el reporte que se mostrará en pantalla.
    ///                      Formato.- Variable que contiene el formato en el que se va a generar el reporte "PDF" O "Excel"
    /// USUARIO CREO:        Juan Alberto Hernández Negrete.
    /// FECHA CREO:          3/Mayo/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Salvador Hernández Ramírez
    /// FECHA MODIFICO:      16-Mayo-2011
    /// CAUSA MODIFICACIÓN:  Se asigno la opción para que en el mismo método se muestre el reporte en excel
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte_Generar, String Formato)
    {
        String Pagina = "../../../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            if (Formato == "PDF")
            {
                Pagina = Pagina + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt",
                "window.open('" + Pagina + "', 'Reporte','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
            else if (Formato == "Excel")
            {
                String Ruta = "../../Reporte/" + Nombre_Reporte_Generar;
                ScriptManager.RegisterStartupScript(this, this.GetType(), "open", "window.open('" + Ruta + "', 'Reportes','toolbar=0,dire ctories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }
}
