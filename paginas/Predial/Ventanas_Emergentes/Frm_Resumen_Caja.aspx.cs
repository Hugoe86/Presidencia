using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Predial_Historial_Pagos.Negocio;
using Presidencia.Reportes;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Ope_Pre_Resumen_Caja : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 2;
    private const int Const_Estado_Modificar = 3;

    private static String M_Cuenta_ID;
    private static Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    private static DataTable Dt_Agregar_Co_Propietarios = new DataTable();
    private static DataTable Dt_Agregar_Diferencias = new DataTable();
    #endregion

    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                if (Request.QueryString["Cuenta_Predial"] != null)
                {
                    Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
                    String Cuenta_Predial_ID = Request.QueryString["Cuenta_Predial"].ToString();
                    Session["Cuenta_Predial_ID"] = Cuenta_Predial_ID;
                    Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                    Session["Cuenta_Predial_ID_Convenios"] = Cuenta_Predial_ID;
                    String Cuenta_Predial = Request.QueryString["Cuenta_Predial"].ToString();
                    Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Cuenta_Predial;
                    Txt_Cuenta_Predial.Text = Cuenta_Predial;
                    Consultar_Datos_Cuenta_Constancia();
                    Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
                    Session.Remove("CUENTA_PREDIAL_ID");
                    Habilitar_Todo();
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



    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);

        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Session["Cuenta_Predial_ID"] = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_ID.Value = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Session["Cuenta_Predial_ID_Convenios"] = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Cuenta_Predial;
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
            }
            Consultar_Datos_Cuenta_Constancia();
        }
        Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
        Session.Remove("CUENTA_PREDIAL_ID");
    }


    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Constanica
    ///DESCRIPCIÓN          : Realiza la búsqueda de los datos de la cuenta predial introducida
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 29/Junio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///**************************************************************************** ***
    protected void Consultar_Datos_Cuenta_Constancia()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio RP_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        try
        {

            if (Txt_Cuenta_Predial.Text.Trim() != "")
            {
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
                //Consulta la Cuenta Predial
                if (Session["Cuenta_Predial_ID"] == null)
                {
                    DataTable Dt_Cuentas;
                    Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
                    Cuentas_Predial.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                    Dt_Cuentas = Cuentas_Predial.Consultar_Cuenta();
                    if (Dt_Cuentas != null)
                    {
                        if (Dt_Cuentas.Rows.Count > 0)
                        {
                            Session["Cuenta_Predial_ID"] = Dt_Cuentas.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                        }
                    }
                }
                M_Cuenta_ID = Txt_Cuenta_Predial.Text.Trim();
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Limpiar_Todo();
                Cargar_Datos();

                String Cuenta_Predial_ID = (Session["Cuenta_Predial_ID"] != null) ? Session["Cuenta_Predial_ID"].ToString().Trim() : "";
                if (Cuenta_Predial_ID != "")
                {

                    Llenar_Grid_Historial_Pagos(0);      //Manda a llamar el metodo para llenar el; grid de historial de pagos

                    RP_Negocio.P_Cuenta_Predial = Cuenta_Predial_ID;

                    DataTable Dt_Adeudos = RP_Negocio.Consultar_Adeudos_Cuentas_Predial();
                    //Llenar_Combo_Anios(Dt_Adeudos); 
                    Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
                    Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                    DataTable Dt_Contribuyentes = Rs_Consulta_Ope_Resumen_Predio.Consultar_Contribuyentes();
                    DataSet Ds_Impuestos = Rs_Consulta_Ope_Resumen_Predio.Consulta_Datos_Cuenta_Impuestos();
                    if (Ds_Impuestos.Tables[0].Rows.Count - 1 > 0)
                    {
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuenta_Origen"].ToString().Trim()))
                        {
                            Txt_Cuenta_Origen.Text = Ds_Impuestos.Tables[0].Rows[0]["Cuenta_Origen"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Valor_Fiscal"].ToString().Trim()))
                        {
                            Txt_Valor_Fiscal_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Ds_Impuestos.Tables[0].Rows[0]["Valor_Fiscal"].ToString().Trim()));
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Periodo_Corriente"].ToString().Trim()))
                        {
                            Txt_Periodo_Corriente_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Periodo_Corriente"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Anual"].ToString().Trim()))
                        {
                            Txt_Cuota_Anual_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Anual"].ToString().Trim()));
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Bimestral"].ToString().Trim()))
                        {
                            Txt_Cuota_Bimestral_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Bimestral"].ToString().Trim()));
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Diferencia_Construccion"].ToString().Trim()))
                        {
                            Txt_Dif_Construccion_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Diferencia_Construccion"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Porcentaje_Excencion"].ToString().Trim()))
                        {
                            Txt_Porciento_Exencion_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Porcentaje_Excencion"].ToString().Trim();
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Termino_Excencion"].ToString().Trim()))
                        {
                            if (Ds_Impuestos.Tables[0].Rows[0]["Termino_Excencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
                            {
                                Txt_Fecha_Termino_Extencion.Text = "";
                            }
                            else
                            {
                                Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", Ds_Impuestos.Tables[0].Rows[0]["Termino_Excencion"].ToString().Trim());
                            }
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Fecha_Avaluo"].ToString().Trim()))
                        {
                            if (Ds_Impuestos.Tables[0].Rows[0]["Fecha_Avaluo"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
                            {
                                Txt_Fecha_Avaluo_Impuestos.Text = "";
                            }
                            else
                            {
                                Txt_Fecha_Avaluo_Impuestos.Text = String.Format("{0:dd/MMM/yyyy}", Ds_Impuestos.Tables[0].Rows[0]["Fecha_Avaluo"].ToString().Trim());
                            }
                        }
                        if (!string.IsNullOrEmpty(Ds_Impuestos.Tables[0].Rows[0]["Cuota_Fija"].ToString().Trim()))
                        {
                            Txt_Cuota_Fija_Impuestos.Text = Ds_Impuestos.Tables[0].Rows[0]["Cuota_Fija"].ToString().Trim();
                        }

                        Rs_Consulta_Ope_Resumen_Predio.P_No_Cuota_Fija = Ds_Impuestos.Tables[0].Rows[0]["Cuota_Fija_ID"].ToString();
                        DataTable Dt_Cuota_Detalles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Cuota_Fija_Detalles();
                        //Validar que la tabla traigo un registro
                        if (Dt_Cuota_Detalles.Rows.Count > 0)
                        {

                            if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Excedente_Contruccion"].ToString().Trim()))
                            {
                                //Txt_Excedente_Construccion_Multiplicador.Text = Dt_Cuota_Detalles.Rows[0]["Excedente_Contruccion"].ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Excedente_Valor"].ToString().Trim()))
                            {
                                //Txt_Excedente_Valor_Multiplicador.Text = Dt_Cuota_Detalles.Rows[0]["Excedente_Valor"].ToString().Trim();
                            }

                            if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Cuota_Minima"].ToString().Trim()))
                            {
                                //Txt_Cuota_Minima.Text = Dt_Cuota_Detalles.Rows[0]["Cuota_Minima"].ToString().Trim();
                            }

                            if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim()))
                            {
                                //Txt_Excedente_Construccion_Multiplicando.Text = Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim()))
                            {
                                //Txt_Excedente_Valor_Multiplicando.Text = Dt_Cuota_Detalles.Rows[0]["Tasa_Valor"].ToString().Trim();
                            }
                            if (!string.IsNullOrEmpty(Dt_Cuota_Detalles.Rows[0]["Tipo"].ToString().Trim()))
                            {
                                Txt_Cuota_Fija.Text = Dt_Cuota_Detalles.Rows[0]["Tipo"].ToString().Trim();
                                //Asignar si es por financiamiento el total 
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Constancias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Constancias();
            Grid_Constancias.DataSource = Dt_Pagos;
            Grid_Constancias.PageIndex = e.NewPageIndex;
            Grid_Constancias.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Derechos_Supervision_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Constancias.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Derechos_Supervision();
            Grid_Derechos_Supervision.DataSource = Dt_Pagos;
            Grid_Derechos_Supervision.PageIndex = e.NewPageIndex;
            Grid_Derechos_Supervision.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Impuesto_Fraccionamiento_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Grid_Constancias.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Fraccionamientos();
            Grid_Impuesto_Fraccionamiento.DataSource = Dt_Pagos;
            Grid_Impuesto_Fraccionamiento.PageIndex = e.NewPageIndex;
            Grid_Impuesto_Fraccionamiento.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Traslado_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Historial_Pagos.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Grid_Constancias.DataBind();
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Pagos = new DataTable();
        String P_Cuenta_Predial = Hdf_Cuenta_Predial_ID.Value;
        try
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Traslado();
            Grid_Traslado.DataSource = Dt_Pagos;
            Grid_Traslado.PageIndex = e.NewPageIndex;
            Grid_Traslado.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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
                        Txt_Ultimo_Movimiento_General.Text = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
                    }
                }
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "BLOQUEADA")
            {
                Lbl_Estatus.Text = " BLOQUEADA" + " " + Lbl_Estatus.Text;
            }
            if (dataTable.Rows[0]["Estatus"].ToString().Trim() == "PENDIENTE")
            {
                Lbl_Estatus.Text = " Cuenta No Generada";
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Resumen de Predio", "alert('Cuenta No Generada')", true);

            }
            Txt_Cuenta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tipo_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                DataTable Dt_Tipo_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tipo_Predio();
                Txt_Tipo_Periodo_Impuestos.Text = Dt_Tipo_Predio.Rows[0]["Descripcion"].ToString().Trim();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                DataTable Dt_Uso_Suelo = Rs_Consulta_Ope_Resumen_Predio.Consultar_Uso_Predio();
                Txt_Uso_Predio_General.Text = Dt_Uso_Suelo.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                DataTable Dt_Estado_Predio = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio();
                Txt_Estado_Predio_General.Text = Dt_Estado_Predio.Rows[0]["Descripcion"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString() != string.Empty)
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                DataTable Dt_Calles = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                Txt_Ubicacion_General.Text = Dt_Calles.Rows[0]["Nombre"].ToString();
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = Dt_Calles.Rows[0]["Colonia_ID"].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Colonia_General.Text = Dt_Colonia.Rows[0]["Nombre"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            {
                Txt_Estatus_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
                M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            }

            Txt_Supe_Construida_General.Text = dataTable.Rows[0]["Superficie_Construida"].ToString();
            M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            Txt_Super_Total_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                DataTable Dt_Colonia = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                Txt_Calle_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            Txt_Numero_Exterior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            Txt_Numero_Interior_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            Txt_Clave_Catastral_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            {
                Txt_Efectos_General.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            }
            Txt_Valor_Fiscal_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()));
            M_Orden_Negocio.P_Valor_Fiscal = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString();
            Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            Txt_Periodo_Corriente_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            M_Orden_Negocio.P_Periodo_Corriente_Inicial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            {
                Txt_Cuota_Anual_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Convert.ToDecimal(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()));
                double Cuota_Bimestral = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6;
                Txt_Cuota_Bimestral_Impuestos.Text = "$ " + String.Format("{0:#,###,###.00}", Cuota_Bimestral);
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] != null)
            {
                M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
                M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
            }
            Txt_Porciento_Exencion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] != null)
            {
                M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo] != null)
            {
                if (dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
                {
                    Txt_Fecha_Avaluo_Impuestos.Text = "";
                    M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
                }
                else
                {
                    Txt_Fecha_Avaluo_Impuestos.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Fecha_Avaluo"].ToString().Trim());
                    M_Orden_Negocio.P_Fecha_Avaluo = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Fecha_Avaluo].ToString());
                }
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion] != null)
            {
                if (dataTable.Rows[0]["Termino_Exencion"].ToString().Trim() == "01/01/0001 12:00:00 a.m.")
                {
                    Txt_Fecha_Termino_Extencion.Text = "";
                    M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
                }
                else
                {
                    Txt_Fecha_Termino_Extencion.Text = String.Format("{0:dd/MMM/yyyy}", dataTable.Rows[0]["Termino_Exencion"].ToString().Trim());
                    M_Orden_Negocio.P_Fecha_Termina_Exencion = Convert.ToDateTime(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Termino_Exencion].ToString());
                }

            }
            Txt_Dif_Construccion_Impuestos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
            M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();

            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            {
                M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
                //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            }
            //Z1 HAY KU07A FIJ4!!! Seccion de carga de datos de la cuota fija
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] != null)
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "NO" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "no" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "No")
                {
                    Chk_Cuota_Fija.Checked = false;
                    M_Orden_Negocio.P_Cuota_Fija = "NO";
                }
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
                {
                    Chk_Cuota_Fija.Checked = true;
                    M_Orden_Negocio.P_Cuota_Fija = "SI";

                    //----K4RG4R D47OZ D3 14 CU0T4 F1J4!!!!!
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
                    {
                        M_Orden_Negocio.P_No_Cuota_Fija = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString();
                        Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
                    }
                }
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            {
                M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString());
                Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            }

            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
                DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
                if (Dt_Tasa.Rows.Count > 0)
                {
                    Txt_Tasa_Impuestos.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
                }
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
            {
                //Cmb_Domicilio_Foraneo.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
                //M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0]["Estado_ID_Notificacion"].ToString()))
            {
                Rs_Consulta_Ope_Resumen_Predio.P_Estado_Predio = (dataTable.Rows[0]["Estado_ID_Notificacion"].ToString());
                DataTable Dt_Estado_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Estado_Predio_Propietario();
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
            if (dataTable.Rows[0]["Domicilio_Foraneo"].ToString().Trim() == "SI")
            {
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_Notificacion"].ToString()))
                {
                    Txt_Colonia_Propietario.Text = dataTable.Rows[0]["Colonia_Notificacion"].ToString();
                }
                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_Notificacion"].ToString()))
                {
                    Txt_Calle_Propietario.Text = dataTable.Rows[0]["Calle_Notificacion"].ToString();
                }
            }
            else
            {

                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Colonia_ID = dataTable.Rows[0]["Colonia_ID_Notificacion"].ToString();
                    DataTable DT_Colonia_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Colonia_Generales();
                    Txt_Colonia_Propietario.Text = DT_Colonia_Propietario.Rows[0]["Nombre"].ToString();
                }

                if (!String.IsNullOrEmpty(dataTable.Rows[0]["Calle_ID_Notificacion"].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Calle_ID = dataTable.Rows[0]["Calle_ID_Notificacion"].ToString();//*
                    DataTable Dt_Calle_Propietario = Rs_Consulta_Ope_Resumen_Predio.Consultar_Calle_Generales();
                    Txt_Calle_Propietario.Text = Dt_Calle_Propietario.Rows[0]["Nombre"].ToString();
                }
            }

            Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["No_Exterior_Notificacion"].ToString();
            Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["No_Interior_Notificacion"].ToString();
            Txt_Cod_Postal_Propietario.Text = dataTable.Rows[0]["Codigo_Postal"].ToString();
            M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["Codigo_Postal"].ToString();


            M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
            M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
            Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios;
            if (Dt_Agregar_Co_Propietarios.Rows.Count - 1 >= 0)
            {
                for (int x = 0; x <= Dt_Agregar_Co_Propietarios.Rows.Count - 1; x++)
                {
                    if (Dt_Agregar_Co_Propietarios.Rows[0]["Tipo"].ToString().Trim() == "COPROPIETARIO")
                    {
                        Txt_Copropietarios_Propietario.Text += Dt_Agregar_Co_Propietarios.Rows[x]["Nombre_Contribuyente"].ToString().Trim() + " \t" + Dt_Agregar_Co_Propietarios.Rows[x]["Rfc"].ToString().Trim() + "\n";

                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
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
            if (Dt_Cuota_Detalles.Rows.Count - 1 >= 0)
            {
                Txt_Cuota_Fija.Text = Dt_Cuota_Detalles.Rows[0]["Descripcion"].ToString();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Cuota_Fija: " + Ex.Message);
        }
    }

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
                Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
                Lbl_Mensaje_Error.Visible = true;
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
                if (dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                {
                    Txt_Propietario_Poseedor_Propietario.Text = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                    M_Orden_Negocio.P_Tipo_Propietario = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                }

                Txt_Rfc_Propietario.Text = dataTable.Rows[0]["RFC"].ToString();
                M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Propietario: " + Ex.Message);
        }
    }
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

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpiar_Todo
    ///DESCRIPCION : Limpia los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 05-Agsoto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Habilitar_Todo()
    {
        Txt_Cuenta_Predial.Enabled = false;
        Txt_Cuenta_Origen.Enabled = false;
        Txt_Uso_Predio_General.Enabled = false;
        Txt_Estado_Predio_General.Enabled = false;
        Txt_Estatus_General.Enabled = false;
        Txt_Supe_Construida_General.Enabled = false;
        Txt_Super_Total_General.Enabled = false;
        Txt_Ubicacion_General.Enabled = false;
        Txt_Colonia_General.Enabled = false;
        Txt_Numero_Exterior_General.Enabled = false;
        Txt_Numero_Interior_General.Enabled = false;
        Txt_Clave_Catastral_General.Enabled = false;
        Txt_Efectos_General.Enabled = false;
        Txt_Ultimo_Movimiento_General.Enabled = false;


        Txt_Nombre_Propietario.Enabled = false;
        Txt_Propietario_Poseedor_Propietario.Enabled = false;
        Txt_Rfc_Propietario.Enabled = false;
        Txt_Calle_Propietario.Enabled = false;
        Txt_Colonia_Propietario.Enabled = false;
        Txt_Ciudad_Propietario.Enabled = false;
        Txt_Numero_Exterior_Propietario.Enabled = false;
        Txt_Numero_Interior_Propietario.Enabled = false;
        Txt_Estado_Propietario.Enabled = false;
        Txt_Ciudad_Propietario.Enabled = false;
        Txt_Cod_Postal_Propietario.Enabled = false;
        Txt_Copropietarios_Propietario.ReadOnly = true;

        Txt_Valor_Fiscal_Impuestos.Enabled = false;
        Txt_Tasa_Impuestos.Enabled = false;
        Txt_Periodo_Corriente_Impuestos.Enabled = false;
        Txt_Tipo_Periodo_Impuestos.Enabled = false;
        Txt_Cuota_Anual_Impuestos.Enabled = false;
        Txt_Cuota_Bimestral_Impuestos.Enabled = false;
        Txt_Dif_Construccion_Impuestos.Enabled = false;
        Txt_Porciento_Exencion_Impuestos.Enabled = false;
        Txt_Fecha_Termino_Extencion.Enabled = false;
        Txt_Fecha_Avaluo_Impuestos.Enabled = false;
        Txt_Cuota_Fija_Impuestos.Enabled = false;
        Chk_Cuota_Fija.Enabled = false;

        Txt_Cuota_Fija.Enabled = false;

    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpiar_Todo
    ///DESCRIPCION : Limpia los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 05-Agsoto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Limpiar_Todo()
    {
        Txt_Cuenta_Origen.Text = "";
        Txt_Uso_Predio_General.Text = "";
        Txt_Estado_Predio_General.Text = "";
        Txt_Estatus_General.Text = "";
        Txt_Supe_Construida_General.Text = "";
        Txt_Super_Total_General.Text = "";
        Txt_Ubicacion_General.Text = "";
        Txt_Colonia_General.Text = "";
        Txt_Numero_Exterior_General.Text = "";
        Txt_Numero_Interior_General.Text = "";
        Txt_Clave_Catastral_General.Text = "";
        Txt_Efectos_General.Text = "";
        Txt_Ultimo_Movimiento_General.Text = "";
        Lbl_Estatus.Text = "";

        Txt_Nombre_Propietario.Text = "";
        Txt_Propietario_Poseedor_Propietario.Text = "";
        Txt_Rfc_Propietario.Text = "";
        Txt_Calle_Propietario.Text = "";
        Txt_Colonia_Propietario.Text = "";
        Txt_Ciudad_Propietario.Text = "";
        Txt_Numero_Exterior_Propietario.Text = "";
        Txt_Numero_Interior_Propietario.Text = "";
        Txt_Estado_Propietario.Text = "";
        Txt_Ciudad_Propietario.Text = "";
        Txt_Cod_Postal_Propietario.Text = "";
        Txt_Copropietarios_Propietario.Text = "";

        Txt_Valor_Fiscal_Impuestos.Text = "";
        Txt_Tasa_Impuestos.Text = "";
        Txt_Periodo_Corriente_Impuestos.Text = "";
        Txt_Tipo_Periodo_Impuestos.Text = "";
        Txt_Cuota_Anual_Impuestos.Text = "";
        Txt_Cuota_Bimestral_Impuestos.Text = "";
        Txt_Dif_Construccion_Impuestos.Text = "";
        Txt_Porciento_Exencion_Impuestos.Text = "";
        Txt_Fecha_Termino_Extencion.Text = "";
        Txt_Fecha_Avaluo_Impuestos.Text = "";
        Txt_Cuota_Fija_Impuestos.Text = "";
        Chk_Cuota_Fija.Checked = false;

        Txt_Cuota_Fija.Text = "";

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Historial_Pagos
    ///DESCRIPCIÓN: Metodo que llena el grid de historial de pagos
    ///PROPIEDADES:     
    ///CREO: Christian Perez Ibarra
    ///FECHA_CREO: 14/Agosto/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Llenar_Grid_Historial_Pagos(int Page_Index)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
        DataTable Dt_Resumen_Predio_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Pagos();
        Boolean Monto_Corriente = false;
        //Session["Dt_Historial"] = Dt_Historial_Pagos;
        Grid_Historial_Pagos.DataSource = Dt_Resumen_Predio_Pagos;
        Grid_Historial_Pagos.PageIndex = Page_Index;
        Grid_Historial_Pagos.DataBind();
        for (int x = 0; x < Dt_Resumen_Predio_Pagos.Rows.Count; x++)
        {
            if (Dt_Resumen_Predio_Pagos.Rows[x]["Monto_Corriente"].ToString().Trim() != "0")
                Monto_Corriente = true;
        }
        if (Monto_Corriente != true)
            Grid_Historial_Pagos.Columns[4].Visible = false;
        Grid_Historial_Pagos.Columns[12].Visible = false;
        Grid_Historial_Pagos.Columns[15].Visible = false;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_Impuestos_Derechos_Supervision_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Historial_Pagos_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Historial_Pagos.SelectedIndex = (-1);
            Llenar_Grid_Historial_Pagos(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }




    ///***********************************************************************************
    /// NOMBRE:              Generar_Reporte
    /// DESCRIPCIÓN:         Genera el reporte.
    /// PARÁMETROS:          Dt_Imprimir_Historial.- Data table de los campos de la consulta.
    ///                      Historial_Pagos.- Data set para  mandar a la imporesion del reporte
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          12/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Generar_Reporte(DataTable Dt_Imprimir_Historial, DataSet Historial_Pagos)
    {
        DataRow Renglon;
        String Nombre_Reporte = "Rpt_Historial_Pagos" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));
        Cls_Reportes Reportes = new Cls_Reportes();
        try
        {
            Renglon = Dt_Imprimir_Historial.Rows[0];
            Historial_Pagos.Tables[1].ImportRow(Renglon);
            for (int Detalles = 0; Detalles < Dt_Imprimir_Historial.Rows.Count; Detalles++)
            {
                Renglon = Dt_Imprimir_Historial.Rows[Detalles];
                Historial_Pagos.Tables[0].ImportRow(Renglon);

            }
            Reportes.Generar_Reporte(ref Historial_Pagos, "../Rpt/Predial/Rpt_Pre_Historial_Pagos.rpt", Nombre_Reporte, "PDF");
            Mostrar_Reporte(Nombre_Reporte, "PDF");

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }

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
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

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
        Dt_Propietario.Columns.Add("Propietario_Poseedor");
        Dt_Propietario.Columns.Add("Rfc");
        Dt_Propietario.Columns.Add("Colonia");
        Dt_Propietario.Columns.Add("Calle");
        Dt_Propietario.Columns.Add("Numero_Exterior");
        Dt_Propietario.Columns.Add("Numero_Interior");
        Dt_Propietario.Columns.Add("Estado");
        Dt_Propietario.Columns.Add("Ciudad");
        Dt_Propietario.Columns.Add("Cod_Pos");
        Dt_Propietario.Columns.Add("Copropietarios");
        Propietarios = Dt_Propietario.NewRow();
        Propietarios["Nombre"] = Txt_Nombre_Propietario.Text.Trim();
        Propietarios["Propietario_Poseedor"] = Txt_Propietario_Poseedor_Propietario.Text.Trim();
        Propietarios["Rfc"] = Txt_Rfc_Propietario.Text.Trim();
        Propietarios["Colonia"] = Txt_Colonia_Propietario.Text.Trim();
        Propietarios["Calle"] = Txt_Calle_Propietario.Text.Trim();
        Propietarios["Numero_Exterior"] = Txt_Numero_Exterior_Propietario.Text.Trim();
        Propietarios["Numero_Interior"] = Txt_Numero_Interior_Propietario.Text.Trim();
        Propietarios["Estado"] = Txt_Estado_Propietario.Text.Trim();
        Propietarios["Ciudad"] = Txt_Ciudad_Propietario.Text.Trim();
        Propietarios["Cod_Pos"] = Txt_Cod_Postal_Propietario.Text.Trim();
        Propietarios["Copropietarios"] = Txt_Copropietarios_Propietario.Text.Trim();
        if (Dt_Propietario.Rows.Count == 0)
        {
            Dt_Propietario.Rows.InsertAt(Propietarios, 0);

        }
        return Dt_Propietario;

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
            Dt_Impuestos.Columns.Add("Valor_Fiscal");
            Dt_Impuestos.Columns.Add("Tasa");
            Dt_Impuestos.Columns.Add("Periodo_Corriente");
            Dt_Impuestos.Columns.Add("Tipo_Predio");
            Dt_Impuestos.Columns.Add("Cuota_Anual");
            Dt_Impuestos.Columns.Add("Cuota_Bimestral");
            Dt_Impuestos.Columns.Add("Dif_Construccion");
            Dt_Impuestos.Columns.Add("Porciento_Exencion");
            Dt_Impuestos.Columns.Add("Termino_Exencion");
            Dt_Impuestos.Columns.Add("Fecha_Avaluo");
            Dt_Impuestos.Columns.Add("Cuota_Fija");
            Dt_Impuestos.Columns.Add("Beneficio");
            //Detalles
            Dt_Impuestos.Columns.Add("Cuota_Fija_Por");
            Dt_Impuestos.Columns.Add("Plazo_Financiamiento");
            Dt_Impuestos.Columns.Add("Cuota_Minima");
            Dt_Impuestos.Columns.Add("Excedente_Construccion_Multiplicador");
            Dt_Impuestos.Columns.Add("Excedente_Construccion_Multiplicando");
            Dt_Impuestos.Columns.Add("Excedente_Valor_Multiplicador");
            Dt_Impuestos.Columns.Add("Excedente_Valor_Multiplicando");
            Dt_Impuestos.Columns.Add("Total_Impuesto");
            Dt_Impuestos.Columns.Add("Fundamento_Legal");
            Dt_Impuestos.Columns.Add("Excedente_Construccion_Resultado");
            Dt_Impuestos.Columns.Add("Excedente_Valor_Resultado");

            Impuestos = Dt_Impuestos.NewRow();
            Impuestos["Valor_Fiscal"] = Txt_Valor_Fiscal_Impuestos.Text.Trim();
            Impuestos["Tasa"] = Txt_Tasa_Impuestos.Text.Trim();
            Impuestos["Periodo_Corriente"] = Txt_Periodo_Corriente_Impuestos.Text.Trim();
            Impuestos["Tipo_Predio"] = Txt_Tipo_Periodo_Impuestos.Text.Trim();
            Impuestos["Cuota_Anual"] = Txt_Cuota_Anual_Impuestos.Text.Trim();
            Impuestos["Cuota_Bimestral"] = Txt_Cuota_Bimestral_Impuestos.Text.Trim();
            Impuestos["Dif_Construccion"] = Txt_Dif_Construccion_Impuestos.Text.Trim();
            Impuestos["Porciento_Exencion"] = Txt_Porciento_Exencion_Impuestos.Text.Trim();
            Impuestos["Termino_Exencion"] = Txt_Fecha_Termino_Extencion.Text.Trim();
            Impuestos["Fecha_Avaluo"] = Txt_Fecha_Avaluo_Impuestos.Text.Trim();
            if (Chk_Cuota_Fija.Checked)
            {
                Impuestos["Cuota_Fija"] = "SI";// Txt_Cuota_Fija_Impuestos.Text.Trim();
            }
            else
            {
                Impuestos["Cuota_Fija"] = "NO";// Txt_Cuota_Fija_Impuestos.Text.Trim();
            }
            Impuestos["Beneficio"] = Txt_Cuota_Fija.Text.Trim();
            //Detalles
            Impuestos["Cuota_Fija_Por"] = Txt_Cuota_Fija.Text.Trim();

            Impuestos["Excedente_Construccion_Resultado"] = Total_Construccion;
            if (Dt_Impuestos.Rows.Count == 0)
            {
                Dt_Impuestos.Rows.InsertAt(Impuestos, 0);

            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Asignar Datos Impuestos" + Ex.Message);
        }
        return Dt_Impuestos;

    }
    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Propietario_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos generales del resumen de predio
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Propietario_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Propietario = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        //Crea un nuevo data table
        DataTable Dt_Generales = new DataTable();
        //LLenado de datos
        DataTable Dt_Propietarios = Asignar_Datos_Propietarios();
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Propietario = new Ds_Pre_Resumen_Predio_Generales();
        //obtiene el numero de cuenta predial
        Imprimir_Resumen_Predio_Propietario.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        //manda a llamar a la consulta para traer los datos
        Dt_Generales = Imprimir_Resumen_Predio_Propietario.Consultar_Imprimir_Resumen_Generales();
        Dt_Generales.TableName = "Dt_Generales";
        Dt_Propietarios.TableName = "Dt_Propietario";
        Resumen_Predio_Propietario.Clear();
        Resumen_Predio_Propietario.Tables.Clear();
        Resumen_Predio_Propietario.Tables.Add(Dt_Generales.Copy());
        Resumen_Predio_Propietario.Tables.Add(Dt_Propietarios.Copy());
        Nombre_Reporte = "Reporte Propietario";
        Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Propietario.rpt";
        Formato = "PDF";
        //llama el metodo con los parametros de la consulta y el data set
        Generar_Reportes(Resumen_Predio_Propietario, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
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
        Ruta_Reporte_Crystal = "../Rpt/Predial/Resumen_Predio/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Recibo, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
    }

    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Generales_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos generales del resumen de predio
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          20/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Generales_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Propietario = new Ds_Pre_Resumen_Predio_Generales();
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Generales = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Imprimir_Resumen_General = new DataTable();
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_General = new Ds_Pre_Resumen_Predio_Generales();
        Imprimir_Resumen_Predio_Generales.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();

        Dt_Imprimir_Resumen_General = Imprimir_Resumen_Predio_Generales.Consultar_Imprimir_Resumen_Generales();
        Dt_Imprimir_Resumen_General.TableName = "Dt_Generales";
        Resumen_Predio_Propietario.Clear();
        Resumen_Predio_Propietario.Tables.Clear();
        Resumen_Predio_Propietario.Tables.Add(Dt_Imprimir_Resumen_General.Copy());
        Nombre_Reporte = "Reporte General";
        Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Generales.rpt";
        Formato = "PDF";
        Generar_Reportes(Resumen_Predio_Propietario, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
    }

    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Impuestos_Click
    /// DESCRIPCIÓN:         Genera el reporte de los datos generales del resumen de predio
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          29/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Impuestos_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Impuestos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        //Crea un nuevo data table
        DataTable Dt_Generales = new DataTable();
        //LLenado de datos
        DataTable Dt_Impuestos = Asignar_Datos_Impuestos();
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Impuestos = new Ds_Pre_Resumen_Predio_Generales();
        //obtiene el numero de cuenta predial
        Imprimir_Resumen_Predio_Impuestos.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        //manda a llamar a la consulta para traer los datos
        Dt_Generales = Imprimir_Resumen_Predio_Impuestos.Consultar_Imprimir_Resumen_Generales();
        Dt_Generales.TableName = "Dt_Generales";
        Dt_Impuestos.TableName = "Dt_Impuestos";
        Resumen_Predio_Impuestos.Clear();
        Resumen_Predio_Impuestos.Tables.Clear();
        Resumen_Predio_Impuestos.Tables.Add(Dt_Generales.Copy());
        Resumen_Predio_Impuestos.Tables.Add(Dt_Impuestos.Copy());
        Nombre_Reporte = "Reporte Impuetos";
        Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Impuestos.rpt";
        Formato = "PDF";
        //llama el metodo con los parametros de la consulta y el data set
        Generar_Reportes(Resumen_Predio_Impuestos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
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
        //LLenado de datos
        //instancia el data set que contiene el data table 
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Estado_Cuenta = new Ds_Pre_Resumen_Predio_Generales();
        //obtiene el numero de cuenta predial
        Imprimir_Resumen_Predio_Estado_Cuenta.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        //manda a llamar a la consulta para traer los datos
        Dt_Impuestos = Asignar_Datos_Impuestos();
        Dt_Propietario = Asignar_Datos_Propietarios();
        Dt_Generales = Imprimir_Resumen_Predio_Estado_Cuenta.Consultar_Imprimir_Resumen_Generales();
        Dt_Generales.TableName = "Dt_Generales";
        Dt_Propietario.TableName = "Dt_Propietario";
        Dt_Impuestos.TableName = "Dt_Impuestos";
        Resumen_Predio_Estado_Cuenta.Clear();
        Resumen_Predio_Estado_Cuenta.Tables.Clear();
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Generales.Copy());
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Propietario.Copy());
        Resumen_Predio_Estado_Cuenta.Tables.Add(Dt_Impuestos.Copy());
        Nombre_Reporte = "Reporte Estado Cuenta";
        Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Estado_Cuenta.rpt";
        Formato = "PDF";
        //llama el metodo con los parametros de la consulta y el data set
        Generar_Reportes(Resumen_Predio_Estado_Cuenta, Nombre_Repote_Crystal, Nombre_Reporte, Formato);

    }


    /// *************************************************************************************
    /// NOMBRE:              Btn_Imprimir_Click
    /// DESCRIPCIÓN:         Botron para mandar a imprimir el reporte en cristal
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          12/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Histotial_Pagos_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        DataTable Dt_Generales = new DataTable();
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Historial_Movimientos = new Ds_Pre_Resumen_Predio_Generales();
        try
        {
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Pagos();
            Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
            Dt_Generales.TableName = "Dt_Generales";
            Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos";
            Resumen_Predio_Historial_Movimientos.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
            Nombre_Reporte = "Reporte Historial Pagos";
            Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio_Historial_Pagos.rpt";
            Formato = "PDF";
            //llama el metodo con los parametros de la consulta y el data set
            Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    /// *************************************************************************************
    /// NOMBRE:              Img_Btn_Imprimir_Cuenta_Predial_Click
    /// DESCRIPCIÓN:         Botron para mandar a imprimir el reporte en cristal
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          12/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    
    /// FECHA MODIFICO:      
    /// CAUSA MODIFICACIÓN:  
    /// *************************************************************************************
    protected void Img_Btn_Imprimir_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        String Nombre_Reporte = String.Empty;
        String Nombre_Repote_Crystal = String.Empty;
        String Formato = String.Empty;
        DataTable Dt_Generales = new DataTable();
        int Propietarios = 0;
        int Impuestos = 0;
        int Estado_Cuenta = 0;
        int Historial_Pagos = 0;
        int Historial_Movimientos = 0;
        int Convenios = 0;
        //instacia la clase de negocio
        Cls_Ope_Pre_Resumen_Predio_Negocio Imprimir_Resumen_Predio_Estado_Cuenta = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
        Ds_Pre_Resumen_Predio_Generales Resumen_Predio_Historial_Movimientos = new Ds_Pre_Resumen_Predio_Generales();
        try
        {
            Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            DataTable Dt_Propietarios = Asignar_Datos_Propietarios();
            DataTable Dt_Impuestos = Asignar_Datos_Impuestos();
            DataTable Dt_Resumen_Predio_Historial_Pagos = Rs_Consulta_Ope_Resumen_Predio.Consultar_Historial_Pagos();
            if (Dt_Propietarios.Rows.Count > 0)
            {
                Propietarios = 1;
            }
            if (Dt_Impuestos.Rows.Count > 0)
            {
                Impuestos = 1;
            }
            if (Dt_Resumen_Predio_Historial_Pagos.Rows.Count > 0)
            {
                Historial_Pagos = 1;
            }
            DataTable Dt_Comparacion = new DataTable();
            DataRow Estado;
            Dt_Comparacion.Columns.Add("Propietarios");
            Dt_Comparacion.Columns.Add("Impuestos");
            Dt_Comparacion.Columns.Add("Estado_Cuenta");
            Dt_Comparacion.Columns.Add("Historial_Pagos");

            Estado = Dt_Comparacion.NewRow();
            Estado["Propietarios"] = Propietarios;
            Estado["Impuestos"] = Impuestos;
            Estado["Estado_Cuenta"] = Estado_Cuenta;
            Estado["Historial_Pagos"] = Historial_Pagos;
            if (Dt_Comparacion.Rows.Count == 0)
            {
                Dt_Comparacion.Rows.InsertAt(Estado, 0);

            }
            Dt_Comparacion.TableName = "Dt_Comparacion";
            Dt_Generales = Rs_Consulta_Ope_Resumen_Predio.Consultar_Imprimir_Resumen_Generales();
            Dt_Generales.TableName = "Dt_Generales";
            Dt_Propietarios.TableName = "Dt_Propietario";
            Dt_Impuestos.TableName = "Dt_Impuestos";
            Dt_Resumen_Predio_Historial_Pagos.TableName = "Dt_Historial_Pagos";
            Resumen_Predio_Historial_Movimientos.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Clear();
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Generales.Copy());
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Propietarios.Copy());
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Impuestos.Copy());
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Resumen_Predio_Historial_Pagos.Copy());
            Resumen_Predio_Historial_Movimientos.Tables.Add(Dt_Comparacion.Copy());
            Nombre_Reporte = "Reporte Resumen Predio";
            Nombre_Repote_Crystal = "Rpt_Pre_Resumen_Predio.rpt";
            Formato = "PDF";
            //llama el metodo con los parametros de la consulta y el data set
            Generar_Reportes_Historial_Movimientos(Resumen_Predio_Historial_Movimientos, Nombre_Repote_Crystal, Nombre_Reporte, Formato);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
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
    private void Generar_Reportes_Historial_Movimientos(DataSet Ds_Recibo, String Nombre_Reporte_Crystal, String Nombre_Reporte, String Formato)
    {

        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";


        // Ruta donde se encuentra el reporte Crystal
        Ruta_Reporte_Crystal = "../Rpt/Predial/Resumen_Predio/" + Nombre_Reporte_Crystal;

        // Se crea el nombre del reporte
        String Nombre_Report = Nombre_Reporte + "_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("HH'-'mm'-'ss"));

        // Se da el nombre del reporte que se va generar
        if (Formato == "PDF")
            Nombre_Reporte_Generar = Nombre_Report + ".pdf";  // Es el nombre del reporte PDF que se va a generar
        else if (Formato == "Excel")
            Nombre_Reporte_Generar = Nombre_Report + ".xls";  // Es el nombre del repote en Excel que se va a generar

        Cls_Reportes Reportes = new Cls_Reportes();
        Reportes.Generar_Reporte(ref Ds_Recibo, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, Formato);
        Mostrar_Reporte(Nombre_Reporte_Generar, Formato);
    }


    protected void Cmb_Consultar_Tipo_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Consultar_Pagos = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        String P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
        DataTable Dt_Pagos = new DataTable();
        Grid_Historial_Pagos.DataBind();
        Grid_Traslado.DataBind();
        Grid_Impuesto_Fraccionamiento.DataBind();
        Grid_Derechos_Supervision.DataBind();
        Grid_Constancias.DataBind();
        if (Cmb_Consultar_Tipo_Pago.SelectedValue == "IMPUESTOS FRACCIONAMIENTO")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Fraccionamientos();
            Grid_Impuesto_Fraccionamiento.DataSource = Dt_Pagos;
            Grid_Impuesto_Fraccionamiento.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "DERECHOS SUPERVISION")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Derechos_Supervision();
            Grid_Derechos_Supervision.DataSource = Dt_Pagos;
            Grid_Derechos_Supervision.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "CONSTANCIAS")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Constancias();
            Grid_Constancias.DataSource = Dt_Pagos;
            Grid_Constancias.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "TRASLADO")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Pagos_Traslado();
            Grid_Traslado.DataSource = Dt_Pagos;
            Grid_Traslado.DataBind();
        }
        else if (Cmb_Consultar_Tipo_Pago.SelectedValue == "PREDIAL")
        {
            Consultar_Pagos.P_Cuenta_Predial_ID = P_Cuenta_Predial;
            Dt_Pagos = Consultar_Pagos.Consultar_Historial_Pagos();
            Grid_Historial_Pagos.DataSource = Dt_Pagos;
            Grid_Historial_Pagos.DataBind();
        }
    }
}