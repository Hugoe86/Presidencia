using System;
using System.Collections.Generic;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Liquidacion_Temporal : System.Web.UI.Page
{

    #region Variables
    private const int Const_Estado_Inicial = 0;

    private String M_Cuenta_ID;
    private Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    private DataTable Dt_Agregar_Co_Propietarios = new DataTable();
    private DataTable Dt_Agregar_Diferencias = new DataTable();


    #endregion

    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            Master.Etiqueta_Body_Master_Page.Attributes.Add("onkeydown", "cancelBack()");

            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones                
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de cuentas predial
                //Cargar_Grid_Ordenes_Variacion(0);
                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
                Txt_Cuenta_Predial.Enabled = false;
            }
            Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE','PENDIENTE')";
            Session["TIPO_CONTRIBUYENTE"] = " IN ('PROPIETARIO','POSEEDOR') ";

            Mensaje_Error();
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }
    #endregion

    #region Metodos/Generales [Limpiar Todo,Mensaje_Error,Cargar_Combos,Llenar_Combo_ID,Estado_Botones,Iniciliza_Controles ]

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
        Hdn_No_Orden_Variacion.Value = null;
        Hdn_Contrarecibo.Value = null;
        Hdn_Cuenta_ID.Value = null;
        Hdn_Cuota_Minima.Value = null;
        Hdn_Propietario_ID.Value = null;
        Hdn_Tasa_ID.Value = null;
        Hdn_Contrarecibo.Value = null;

        //QUITA LOS TEXTOS
        //Generales
        Txt_Cuenta_Predial.Text = "";
        Txt_Tipo_Predio.Text = "";
        Txt_Uso_Predio.Text = "";
        Txt_Valor_Fiscal.Text = "";
        Txt_Tasa_Porcentaje.Text = "";
        Txt_Cuota_Bimestral.Text = "";
        Txt_Colonia_Cuenta.Text = "";
        Txt_Calle_Cuenta.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Efectos.Text = "";
        Txt_Ultimo_Movimiento.Text = "";

        //Propietario
        Txt_Nombre_Propietario.Text = "";
        Txt_RFC_Propietario.Text = "";
        Txt_Domicilio_Foraneo.Text = "";
        Txt_Colonia_Propietario.Text = "";
        Txt_Calle_Propietario.Text = "";
        Txt_Numero_Exterior_Propietario.Text = "";
        Txt_Numero_Interior_Propietario.Text = "";
        Txt_Estado_Propietario.Text = "";
        Txt_Ciudad_Propietario.Text = "";
        Txt_CP.Text = "";

        //Resumen de adeudos
        Txt_Periodo_Inicial.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Total_Impuesto.Text = "";
        Txt_Total.Text = "";
        Cmb_Hasta_Anio_Periodo.Items.Clear();

        //QUITA LOS TEXTOS TOOLTIP
        //Generales
        Txt_Cuenta_Predial.ToolTip = "";
        Txt_Tipo_Predio.ToolTip = "";
        Txt_Uso_Predio.ToolTip = "";
        Txt_Valor_Fiscal.ToolTip = "";
        Txt_Tasa_Porcentaje.ToolTip = "";
        Txt_Cuota_Bimestral.ToolTip = "";
        Txt_Colonia_Cuenta.ToolTip = "";
        Txt_Calle_Cuenta.ToolTip = "";
        Txt_No_Exterior.ToolTip = "";
        Txt_No_Interior.ToolTip = "";
        Txt_Efectos.ToolTip = "";
        Txt_Ultimo_Movimiento.ToolTip = "";

        //Propietario
        Txt_Nombre_Propietario.ToolTip = "";
        Txt_RFC_Propietario.ToolTip = "";
        Txt_Domicilio_Foraneo.ToolTip = "";
        Txt_Colonia_Propietario.ToolTip = "";
        Txt_Calle_Propietario.ToolTip = "";
        Txt_Numero_Exterior_Propietario.ToolTip = "";
        Txt_Numero_Interior_Propietario.ToolTip = "";
        Txt_Estado_Propietario.ToolTip = "";
        Txt_Ciudad_Propietario.ToolTip = "";
        Txt_CP.ToolTip = "";

        //Resumen de adeudos
        Txt_Periodo_Inicial.ToolTip = "";
        Txt_Total_Recargos_Ordinarios.ToolTip = "";
        Txt_Total_Impuesto.ToolTip = "";
        Txt_Total.ToolTip = "";

        //QUITA LOS IDs
        Hdn_Tipo_Predio_ID.Value = "";
        Hdn_Uso_Predio_ID.Value = "";

        //QUITA LAS NEGRITAS EN TEXTBOX
        //Generales
        Txt_Cuenta_Predial.Font.Bold = false;
        Txt_Tipo_Predio.Font.Bold = false;
        Txt_Uso_Predio.Font.Bold = false;
        Txt_Valor_Fiscal.Font.Bold = false;
        Txt_Tasa_Porcentaje.Font.Bold = false;
        Txt_Cuota_Bimestral.Font.Bold = false;
        Txt_Colonia_Cuenta.Font.Bold = false;
        Txt_Calle_Cuenta.Font.Bold = false;
        Txt_No_Exterior.Font.Bold = false;
        Txt_No_Interior.Font.Bold = false;
        Txt_Efectos.Font.Bold = false;
        Txt_Ultimo_Movimiento.Font.Bold = false;

        //Propietario
        Txt_Nombre_Propietario.Font.Bold = false;
        Txt_RFC_Propietario.Font.Bold = false;
        Txt_Domicilio_Foraneo.Font.Bold = false;
        Txt_Colonia_Propietario.Font.Bold = false;
        Txt_Calle_Propietario.Font.Bold = false;
        Txt_Numero_Exterior_Propietario.Font.Bold = false;
        Txt_Numero_Interior_Propietario.Font.Bold = false;
        Txt_Estado_Propietario.Font.Bold = false;
        Txt_Ciudad_Propietario.Font.Bold = false;
        Txt_CP.Font.Bold = false;

        //QUITA LAS NEGRITAS EN LABELS
        //Generales
        Lbl_Cuenta_Predial.Font.Bold = false;
        Lbl_Tipo_Predio.Font.Bold = false;
        Lbl_Uso_Predio.Font.Bold = false;
        Lbl_Valor_Fiscal.Font.Bold = false;
        Lbl_Tasa_Porcentaje.Font.Bold = false;
        Lbl_Cuota_Bimestral.Font.Bold = false;
        Lbl_Colonia_Cuenta.Font.Bold = false;
        Lbl_Calle_Cuenta.Font.Bold = false;
        Lbl_No_Exterior.Font.Bold = false;
        Lbl_No_Interior.Font.Bold = false;
        Lbl_Efectos.Font.Bold = false;
        Lbl_Ultimo_Movimiento.Font.Bold = false;

        //Propietario
        Lbl_Nombre_Propietario.Font.Bold = false;
        Lbl_RFC_Propietario.Font.Bold = false;
        Lbl_Domicilio_Foraneo.Font.Bold = false;
        Lbl_Colonia_Propietario.Font.Bold = false;
        Lbl_Calle_Propietario.Font.Bold = false;
        Lbl_Numero_Exterior_Propietario.Font.Bold = false;
        Lbl_Numero_Interior_Propietario.Font.Bold = false;
        Lbl_Estado_Propietario.Font.Bold = false;
        Lbl_Ciudad_Propietario.Font.Bold = false;
        Lbl_CP.Font.Bold = false;

        // Limpiar grid de adeudos
        Grid_Adeudos.DataSource = null;
        Grid_Adeudos.DataBind();
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
    }

    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Encabezado_Error.Text = "";
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combos
    ///DESCRIPCIÓN: metodo usado para cargar la informacion de todos los combos del formulario con la respectiva consulta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/03/2011 08:46:12 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combos()
    {
        DataSet Ds_Cargar_combos;
        try
        {
            Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Botones
    ///DESCRIPCIÓN: Metodo para establecer el estado de los botones y componentes del formulario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 02/02/2011 05:49:53 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Estado_Botones(int P_Estado)
    {
        switch (P_Estado)
        {
            case 0: //Estado inicial                

                Btn_Imprimir.AlternateText = "Imprimir";
                Btn_Salir.AlternateText = "Salir";

                Btn_Imprimir.ToolTip = "Imprimir";
                Btn_Salir.ToolTip = "Salir";

                Btn_Imprimir.Visible = true;
                Btn_Salir.Visible = true;

                Btn_Imprimir.ImageUrl = "~/paginas/imagenes/gridview/grid_print.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";

                break;

            case 3: //Modificar

                Btn_Imprimir.Enabled = false;
                Btn_Imprimir.Visible = false;
                Btn_Salir.Enabled = true;
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";

                Configurar_Estatus_Controles(true, true);

                break;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Configurar_Estatus_Controles
    ///DESCRIPCIÓN          : Configura los controles para manipular
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Configurar_Estatus_Controles(Boolean Estatus_Activo, Boolean Estatus_Visible)
    {
        //Generales
        Txt_Cuenta_Predial.Enabled = Estatus_Activo;
        Txt_Tipo_Predio.Enabled = Estatus_Activo;
        Txt_Uso_Predio.Enabled = Estatus_Activo;
        Txt_Valor_Fiscal.Enabled = Estatus_Activo;
        Txt_Tasa_Porcentaje.Enabled = Estatus_Activo;
        Txt_Cuota_Bimestral.Enabled = Estatus_Activo;
        Txt_Colonia_Cuenta.Enabled = Estatus_Activo;
        Txt_Calle_Cuenta.Enabled = Estatus_Activo;
        Txt_No_Exterior.Enabled = Estatus_Activo;
        Txt_No_Interior.Enabled = Estatus_Activo;
        Txt_Efectos.Enabled = Estatus_Activo;
        Txt_Ultimo_Movimiento.Enabled = Estatus_Activo;

        //Propietario
        Txt_Nombre_Propietario.Enabled = Estatus_Activo;
        Txt_RFC_Propietario.Enabled = Estatus_Activo;
        Txt_Domicilio_Foraneo.Enabled = Estatus_Activo;
        Txt_Colonia_Propietario.Enabled = Estatus_Activo;
        Txt_Calle_Propietario.Enabled = Estatus_Activo;
        Txt_Numero_Exterior_Propietario.Enabled = Estatus_Activo;
        Txt_Numero_Interior_Propietario.Enabled = Estatus_Activo;
        Txt_Estado_Propietario.Enabled = Estatus_Activo;
        Txt_Ciudad_Propietario.Enabled = Estatus_Activo;
        Txt_CP.Enabled = Estatus_Activo;

        //Resumen de adeudos
        Txt_Periodo_Inicial.Enabled = Estatus_Activo;
        Txt_Total_Recargos_Ordinarios.Enabled = Estatus_Activo;
        Txt_Total_Impuesto.Enabled = Estatus_Activo;
        Txt_Total.Enabled = Estatus_Activo;
        Cmb_Hasta_Bimestre_Periodo.Enabled = true;
        Cmb_Hasta_Anio_Periodo.Enabled = true;

        //Grid_Ordenes_Variacion.Visible = !Estatus_Activo;
        //Tr_Encabezado_Ordenenes.Visible = !Estatus_Activo;

        Txt_Total.Style["text-align"] = "right";
        Txt_Total_Recargos_Ordinarios.Style["text-align"] = "right";
        Txt_Total_Impuesto.Style["text-align"] = "right";
        Panel.Visible = Estatus_Visible;
        if (Panel.Visible)
        {
            if (Btn_Salir.ToolTip != "Cancelar")
            {
                Btn_Salir.ToolTip = "Regresar";
            }
        }
        else
        {
            Btn_Salir.ToolTip = "Salir";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Configurar_Estatus_Controles
    ///DESCRIPCIÓN          : Configura los controles para manipular
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Configurar_Edicion_Controles(Boolean Estatus_Edicion)
    {
        //Generales
        Txt_Cuenta_Predial.ReadOnly = !Estatus_Edicion;
        Txt_Tipo_Predio.ReadOnly = !Estatus_Edicion;
        Txt_Uso_Predio.ReadOnly = !Estatus_Edicion;
        Txt_Valor_Fiscal.ReadOnly = !Estatus_Edicion;
        Txt_Tasa_Porcentaje.ReadOnly = !Estatus_Edicion;
        Txt_Cuota_Bimestral.ReadOnly = !Estatus_Edicion;
        Txt_Colonia_Cuenta.ReadOnly = !Estatus_Edicion;
        Txt_Calle_Cuenta.ReadOnly = !Estatus_Edicion;
        Txt_No_Exterior.ReadOnly = !Estatus_Edicion;
        Txt_No_Interior.ReadOnly = !Estatus_Edicion;
        Txt_Efectos.ReadOnly = !Estatus_Edicion;
        Txt_Ultimo_Movimiento.ReadOnly = !Estatus_Edicion;

        //Propietario
        Txt_Nombre_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_RFC_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Domicilio_Foraneo.ReadOnly = !Estatus_Edicion;
        Txt_Colonia_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Calle_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Numero_Exterior_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Numero_Interior_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Estado_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_Ciudad_Propietario.ReadOnly = !Estatus_Edicion;
        Txt_CP.ReadOnly = !Estatus_Edicion;

        //Resumen de adeudos
        Txt_Periodo_Inicial.ReadOnly = !Estatus_Edicion;
        Txt_Total_Recargos_Ordinarios.ReadOnly = !Estatus_Edicion;
        Txt_Total_Impuesto.ReadOnly = !Estatus_Edicion;
        Txt_Total.ReadOnly = !Estatus_Edicion;

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Inicializa_Controles
    ///DESCRIPCIÓN: inicializa los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: tres/agosto/2011 06:28:07 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Inicializa_Controles()
    {
        try
        {
            //if ( ((DataSet)Session["Ds_Consulta_Combos"]) == null)
            Consulta_Combos();
            Cargar_Combos();
            Configurar_Estatus_Controles(true, true);
            Estado_Botones(Const_Estado_Inicial);
            Dt_Agregar_Co_Propietarios.Clear();
            //Dt_Agregar_Co_Propietarios.Columns.Add("CONTRIBUYENTE_ID");
            //Dt_Agregar_Co_Propietarios.Columns.Add("NOMBRE_CONTRIBUYENTE");
            Dt_Agregar_Diferencias.Clear();
            //Dt_Agregar_Diferencias.Columns.Add("PERIODO");
            //Dt_Agregar_Diferencias.Columns.Add("TASA");
            //Dt_Agregar_Diferencias.Columns.Add("TIPO");
            //Dt_Agregar_Diferencias.Columns.Add("IMPORTE");
            //Dt_Agregar_Diferencias.Columns.Add("CUOTA_BIMESTRAL");
            //Dt_Agregar_Diferencias.Columns.Add("VALOR_FISCAL");
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Metodos ABC [Consulta_Combos,Consulta_Valor_Excedente]

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Combos
    ///DESCRIPCIÓN: consulta los datos de todos los combos de la pagina
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/03/2011 11:50:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Consulta_Combos()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            Session["Ds_Consulta_Combos"] = Orden_Negocio.Consulta_Combos();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Metodos Cargar Datos [Cargar_datos,Cargar_generales,Cargar_Popietarios,Cargar_Datos_Cuota_Fija]

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
    private Boolean Cargar_Datos()
    {
        Boolean Datos_Cargados = false;
        try
        {
            if (Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]))
            {
                Datos_Cargados = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
        return Datos_Cargados;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Variacion
    /// DESCRIPCIÓN: Consulta los datos de la Orden de Variacción
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-ago-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************  
    private void Cargar_Variacion()
    {
        var Orden_Variacion = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        DataTable Dt_Orden_Variacion;
        int Anio;
        decimal Cuota_Anual;

        int.TryParse(Session["Anio"].ToString().Trim(), out Anio);
        Orden_Variacion.P_Incluir_Campos_Foraneos = true;
        Orden_Variacion.P_Incluir_Generales_Cuenta = true;
        Orden_Variacion.P_No_Orden_Variacion = Convert.ToString(Session["No_Orden_Variacion_ID"]);
        Orden_Variacion.P_Anio_Orden = Anio;
        Dt_Orden_Variacion = Orden_Variacion.Consultar_Ordenes_Variacion();

        // verificar que la consulta regresa datos
        if (Dt_Orden_Variacion != null && Dt_Orden_Variacion.Rows.Count > 0)
        {
            // ubicacion
            Txt_Calle_Cuenta.Text = Dt_Orden_Variacion.Rows[0]["NOMBRE_CALLE_UBICACION"].ToString();
            Txt_Colonia_Cuenta.Text = Dt_Orden_Variacion.Rows[0]["NOMBRE_COLONIA_UBICACION"].ToString();
            Txt_No_Exterior.Text = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Exterior].ToString();
            Txt_No_Interior.Text = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Interior].ToString();
            // notificacion
            Txt_Calle_Propietario.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString().ToUpper();
            Txt_Colonia_Propietario.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString().ToUpper();
            Txt_Numero_Exterior_Propietario.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion].ToString();
            Txt_Numero_Interior_Propietario.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion].ToString();
            Txt_Estado_Propietario.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion].ToString();
            Txt_Ciudad_Propietario.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion].ToString();
            // generales cuenta
            Txt_Tipo_Predio.Text = Dt_Orden_Variacion.Rows[0]["TIPO_PREDIO"].ToString();
            Txt_Uso_Predio.Text = Dt_Orden_Variacion.Rows[0]["USO_SUELO"].ToString();
            decimal Valor_Fiscal;
            decimal.TryParse(Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal].ToString(), out Valor_Fiscal);
            Txt_Valor_Fiscal.Text = Valor_Fiscal.ToString("#,##0.00");
            Txt_Efectos.Text = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Efectos].ToString();
            Txt_Domicilio_Foraneo.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo].ToString();
            decimal.TryParse(Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual].ToString(),
                             out Cuota_Anual);
            Txt_Cuota_Bimestral.Text = (Cuota_Anual / 6).ToString("#,##0.00");
            Txt_Tasa_Porcentaje.Text =
                Dt_Orden_Variacion.Rows[0][Cat_Pre_Tasas_Predial_Anual.Campo_Tasa_Anual].ToString();
            Hdn_Tasa_ID.Value = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID].ToString();
            Txt_CP.Text = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal].ToString();

            // datos propietario
            Txt_Nombre_Propietario.Text = Dt_Orden_Variacion.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
            Txt_RFC_Propietario.Text = Dt_Orden_Variacion.Rows[0]["RFC_PROPIETARIO"].ToString();
            Hdn_Propietario_ID.Value =
                Dt_Orden_Variacion.Rows[0][Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString();

            Cargar_Grid_Variacion_Diferencias(0);
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Propietario_Cuenta
    ///DESCRIPCIÓN: asignar datos de notificación de la cuenta tomados de la cuenta predial
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 18/nov/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Datos_Notificacion_Cuenta(DataTable dataTable)
    {
        try
        {
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
                {
                    Txt_Domicilio_Foraneo.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
                }
                Txt_Estado_Propietario.Text = (!String.IsNullOrEmpty(dataTable.Rows[0]["NOMBRE_ESTADO_CUENTA"].ToString())
                    ? dataTable.Rows[0]["NOMBRE_ESTADO_CUENTA"].ToString()
                    : dataTable.Rows[0]["ESTADO_NOTIFICACION"].ToString());
                M_Orden_Negocio.P_Estado_Propietario = Txt_Estado_Propietario.Text;
                Txt_Colonia_Propietario.Text = dataTable.Rows[0]["NOMBRE_COLONIA"].ToString() != ""
                    ? dataTable.Rows[0]["NOMBRE_COLONIA"].ToString().ToUpper()
                    : dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString().ToUpper();
                M_Orden_Negocio.P_Colonia_Propietario = Txt_Colonia_Propietario.Text;
                Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["NOMBRE_CIUDAD_CUENTA"].ToString() != ""
                    ? dataTable.Rows[0]["NOMBRE_CIUDAD_CUENTA"].ToString()
                    : dataTable.Rows[0]["CIUDAD_NOTIFICACION"].ToString();
                M_Orden_Negocio.P_Ciudad_Propietario = Txt_Ciudad_Propietario.Text;
                Txt_Calle_Propietario.Text = !String.IsNullOrEmpty(dataTable.Rows[0]["NOMBRE_CALLE"].ToString())
                    ? dataTable.Rows[0]["NOMBRE_CALLE"].ToString().ToUpper()
                    : dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString().ToUpper();
                Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
                Txt_CP.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Propietario: " + Ex.Message);
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
    private Boolean Cargar_Generales_Cuenta(DataTable dataTable)
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); // Variable de conexión hacia la capa de Negocios
        Boolean Datos_Cargados = false;
        try
        {
            if (dataTable.Rows.Count > 0)
            {
                //Asignacion de valores a Objeto de Negocio y cajas de texto
                try
                {
                    Hdn_Contrarecibo.Value = dataTable.Rows[0]["CONTRARECIBO"].ToString();
                    Hdn_Cuenta_ID.Value = dataTable.Rows[0]["ID"].ToString();
                    M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["ID"].ToString();
                }
                catch
                {
                    Hdn_Cuenta_ID.Value = dataTable.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                    M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["CUENTA_PREDIAL_ID"].ToString();
                }
                //SESION PARA MOSTRAR LA CONSULTA DE RESUMEN DE CUENTA
                Session["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;
                M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString().ToUpper();
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text;


                Rs_Consulta_Ope_Resumen_Predio.P_Cuenta_Predial_ID = dataTable.Rows[0]["Cuenta_Predial_ID"].ToString().Trim();
                DataTable Dt_Ultimo_Movimiento = Rs_Consulta_Ope_Resumen_Predio.Consultar_Ultimo_Movimiento();
                if (Dt_Ultimo_Movimiento.Rows.Count > 0)
                {
                    if (Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString() != string.Empty)
                    {
                        Txt_Ultimo_Movimiento.Text = Dt_Ultimo_Movimiento.Rows[0]["Identificador"].ToString().Trim();
                    }
                }
                if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
                {
                    Rs_Consulta_Ope_Resumen_Predio.P_Tasa_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
                    DataTable Dt_Tasa = Rs_Consulta_Ope_Resumen_Predio.Consultar_Tasa();
                    if (Dt_Tasa.Rows.Count > 0)
                    {
                        Txt_Tasa_Porcentaje.Text = Dt_Tasa.Rows[0]["Descripcion"].ToString();
                    }
                }
                if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
                {
                    double Cuota_Bimestral = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6;
                    Txt_Cuota_Bimestral.Text = String.Format("{0:#,###.00}", Cuota_Bimestral);
                }
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
                {
                    Txt_Tipo_Predio.Text = dataTable.Rows[0]["TIPO_PREDIO_DESCRIPCION"].ToString().ToUpper();
                }
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
                {
                    Txt_Uso_Predio.Text = dataTable.Rows[0]["USO_SUELO_DESCRIPCION"].ToString().ToUpper();
                }
                if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
                {
                    Txt_Colonia_Cuenta.Text = dataTable.Rows[0]["NOMBRE_COLONIA"].ToString().ToUpper();
                    Txt_Calle_Cuenta.Text = dataTable.Rows[0]["NOMBRE_CALLE"].ToString().ToUpper();
                }
                if (dataTable.Rows[0]["NOMBRE_ESTADO_CUENTA"].ToString() != "")
                {
                    Hdn_Estado_Cuenta.Value = dataTable.Rows[0]["NOMBRE_ESTADO_CUENTA"].ToString().ToUpper();
                    Hdn_Ciudad_Cuenta.Value = dataTable.Rows[0]["NOMBRE_CIUDAD_CUENTA"].ToString().ToUpper();
                }
                Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().ToUpper();
                Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().ToUpper();
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
                {
                    Txt_Efectos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
                }
                decimal Valor_Fiscal;
                decimal.TryParse(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString(), out Valor_Fiscal);
                Txt_Valor_Fiscal.Text = Valor_Fiscal.ToString("#,##0.00");

                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
                {
                    Hdn_Cuota_Minima_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
                }

                Cargar_Datos_Notificacion_Cuenta(dataTable);

                Datos_Cargados = true;
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }

        return Datos_Cargados;
    }

    #endregion


    #region Eventos/Botones [Nuevo,Modificar,Salir,Busqueda]

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: salir de la orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:47:11 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Panel.Visible)
            {
                Limpiar_Todo();
                Estado_Botones(Const_Estado_Inicial);
                Configurar_Estatus_Controles(false, false);
                //Configurar_Controles_Validacion(false);
            }
            else
            {
                if (Btn_Salir.AlternateText.Equals("Salir"))
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
                else
                {
                    Limpiar_Todo();
                    Estado_Botones(Const_Estado_Inicial);
                    Configurar_Estatus_Controles(false, false);
                    //Configurar_Controles_Validacion(false);
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Imprimir_Click
    /// DESCRIPCIÓN: Manejo del evento clic en el boton imprimir (enviar impresion de calculo de impuesto
    ///             y del formato seleccionado)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Decimal Total_Adeudo = 0;

        // verificar que hay una cuenta seleccionada
        if (Hdn_Cuenta_ID.Value != "")
        {
            // verificar que el adeudo es mayor a cero
            if (Decimal.TryParse(Txt_Total.Text, out Total_Adeudo) && Total_Adeudo > 0)
            {
                Lbl_Mensaje_Error.Visible = false;
                Img_Error.Visible = false;
                //Validar_Campos();

                //Si faltaron campos por capturar envia un mensaje al usuario indicando cuáles
                if (Lbl_Mensaje_Error.Text.Length > 0)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Img_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Es necesario: <br />" + Lbl_Mensaje_Error.Text;
                }
                else
                {
                    // llamar metodo impresion de reporte
                    Imprimir_Reporte(Crear_Ds_Liquidacion_Temporal(),
                        "Rpt_Pre_Liquidacion_Temporal.rpt",
                        "LIQUIDACION_TEMPORAL");
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No es posible imprimir porque no hay adeudos.<br />";
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Debe seleccionar una cuenta predial<br />";
        }
    }

    #endregion

    #region Eventos Combos

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Hasta_Periodo_SelectedIndexChanged
    ///DESCRIPCIÓN          : Manejo del evento cambio de indice para los combos hasta
    ///                         bimestre y anio (actualizar datos de adeudos hasta periodo 
    ///                         seleccionado)
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 26-nov-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Hasta_Periodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cargar_Adeudos_Actual_Diferencias(true);
    }

    #endregion

    #region Metodos Reportes

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

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Liquidacion_Temporal
    ///DESCRIPCIÓN          : Crea un Dataset con los datos para imprimir el reporte de
    ///                         la liquidacion temporal
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 14-oct-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Liquidacion_Temporal()
    {
        Ds_Pre_Liquidacion_Temporal Ds_Liquidacion = new Ds_Pre_Liquidacion_Temporal();


        DataTable Dt_Generales;
        DataTable Dt_Propietario;
        DataTable Dt_Impuestos;
        DataTable Dt_Estado_Cuenta;
        DataTable Dt_Adeudos_Bimestre;

        //LLenado de datos
        Dt_Impuestos = Asignar_Datos_Impuestos();
        Dt_Propietario = Asignar_Datos_Propietarios();
        Dt_Generales = Asignar_Datos_Generales();
        Dt_Estado_Cuenta = Asignar_Datos_Estado_Cuenta();
        Dt_Adeudos_Bimestre = Asignar_Datos_Adeudos_Bimestre();

        Dt_Generales.TableName = "Dt_Generales";
        Dt_Estado_Cuenta.TableName = "Dt_Estado_Cuenta";
        Dt_Propietario.TableName = "Dt_Propietario";
        Dt_Impuestos.TableName = "Dt_Impuestos";
        Dt_Adeudos_Bimestre.TableName = "Dt_Adeudos_Bimestre";

        Ds_Liquidacion.Clear();
        Ds_Liquidacion.Tables.Clear();
        Ds_Liquidacion.Tables.Add(Dt_Generales.Copy());
        Ds_Liquidacion.Tables.Add(Dt_Estado_Cuenta.Copy());
        Ds_Liquidacion.Tables.Add(Dt_Propietario.Copy());
        Ds_Liquidacion.Tables.Add(Dt_Impuestos.Copy());
        Ds_Liquidacion.Tables.Add(Dt_Adeudos_Bimestre.Copy());

        return Ds_Liquidacion;
    }

    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Impuestos
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Roberto González Oseguera
    /// FECHA MODIFICO:      16-oct-2011
    /// CAUSA MODIFICACIÓN:  Agregar campo Cuenta_Predial_ID
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
            Dt_Impuestos.Columns.Add("Cuota_Bimestral");
            Dt_Impuestos.Columns.Add("Cuenta_Predial_ID");


            Impuestos = Dt_Impuestos.NewRow();
            Impuestos["Valor_Fiscal"] = Txt_Valor_Fiscal.Text.Trim();
            Impuestos["Tasa"] = Txt_Tasa_Porcentaje.Text.Trim();
            Impuestos["Periodo_Corriente"] = Txt_Periodo_Inicial.Text.Trim();
            Impuestos["Tipo_Predio"] = Txt_Tipo_Predio.Text.Trim();
            Impuestos["Cuota_Bimestral"] = Txt_Cuota_Bimestral.Text.Trim();
            Impuestos["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;
            if (Dt_Impuestos.Rows.Count == 0)
            {
                Dt_Impuestos.Rows.InsertAt(Impuestos, 0);
            }

        }
        catch (Exception Ex)
        {
            throw new Exception("Asignar_Datos_Impuestos: " + Ex.Message);
        }
        return Dt_Impuestos;

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Liquidacion: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 14-oct-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Liquidacion, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Archivo = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta_Archivo);
            Reporte.SetDataSource(Ds_Liquidacion);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String PDF_Formato = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar    
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Formato);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(PDF_Formato, "PDF");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Propietarios
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Roberto González Oseguera
    /// FECHA MODIFICO:      16-oct-2011
    /// CAUSA MODIFICACIÓN:  Agregar campo Cuenta_Predial_ID
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
        Dt_Propietario.Columns.Add("Cuenta_Predial_ID");

        Propietarios = Dt_Propietario.NewRow();
        Propietarios["Nombre"] = Txt_Nombre_Propietario.Text.Trim();
        Propietarios["Rfc"] = Txt_RFC_Propietario.Text.Trim();
        Propietarios["Colonia"] = Txt_Colonia_Propietario.Text.Trim();
        Propietarios["Calle"] = Txt_Calle_Propietario.Text.Trim();
        Propietarios["Numero_Exterior"] = Txt_Numero_Exterior_Propietario.Text.Trim();
        Propietarios["Numero_Interior"] = Txt_Numero_Interior_Propietario.Text.Trim();
        Propietarios["Estado"] = Txt_Estado_Propietario.Text.Trim();
        Propietarios["Ciudad"] = Txt_Ciudad_Propietario.Text.Trim();
        Propietarios["Cod_Pos"] = Txt_CP.Text.Trim();
        Propietarios["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;
        if (Dt_Propietario.Rows.Count == 0)
        {
            Dt_Propietario.Rows.InsertAt(Propietarios, 0);

        }
        return Dt_Propietario;

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Asignar_Datos_Generales
    /// DESCRIPCIÓN: Metodo para asignar los datos generales de la cuenta al datatable del dataset para imprimir reporte
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 15-oct-2011
    /// MODIFICO: Roberto González Oseguera
    /// FECHA MODIFICO: 16-oct-2011
    /// CAUSA MODIFICACIÓN: Agregar campo Cuenta_Predial_ID
    ///*******************************************************************************************************
    protected DataTable Asignar_Datos_Generales()
    {
        DataTable Dt_Generales = new DataTable();
        DataRow Generales;

        Dt_Generales.Columns.Add("Cuenta_Predial");
        Dt_Generales.Columns.Add("Colonia");
        Dt_Generales.Columns.Add("Ubicacion");
        Dt_Generales.Columns.Add("Numero_Exterior");
        Dt_Generales.Columns.Add("Numero_Interior");
        Dt_Generales.Columns.Add("Efectos");
        Dt_Generales.Columns.Add("Ultimo_Movimiento");
        Dt_Generales.Columns.Add("Cuenta_Predial_ID");

        Generales = Dt_Generales.NewRow();
        Generales["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
        Generales["Colonia"] = Txt_Colonia_Cuenta.Text.Trim();
        Generales["Ubicacion"] = Txt_Calle_Cuenta.Text.Trim();
        Generales["Numero_Exterior"] = Txt_No_Exterior.Text.Trim();
        Generales["Numero_Interior"] = Txt_No_Interior.Text.Trim();
        Generales["Efectos"] = Txt_Efectos.Text.Trim();
        Generales["Ultimo_Movimiento"] = Txt_Ultimo_Movimiento.Text.Trim();
        Generales["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;

        if (Dt_Agregar_Co_Propietarios.Rows.Count == 0)
        {
            Dt_Generales.Rows.InsertAt(Generales, 0);

        }
        return Dt_Generales;

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Asignar_Datos_Adeudos_Bimestre
    /// DESCRIPCIÓN: Metodo para cargar los adeudos en el grid a la tabla de adeudos por bimestre
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-oct-2011
    /// MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    ///*******************************************************************************************************
    protected DataTable Asignar_Datos_Adeudos_Bimestre()
    {
        DataTable Dt_Generales = new DataTable();
        DataRow Generales;

        Dt_Generales.Columns.Add("Anio");
        Dt_Generales.Columns.Add("Bimestre_1");
        Dt_Generales.Columns.Add("Bimestre_2");
        Dt_Generales.Columns.Add("Bimestre_3");
        Dt_Generales.Columns.Add("Bimestre_4");
        Dt_Generales.Columns.Add("Bimestre_5");
        Dt_Generales.Columns.Add("Bimestre_6");
        Dt_Generales.Columns.Add("Total");
        Dt_Generales.Columns.Add("Cuenta_Predial_ID");

        Dt_Generales.Rows.Clear();

        foreach (GridViewRow Adeudo in Grid_Adeudos.Rows)
        {
            Generales = Dt_Generales.NewRow();
            Generales["Anio"] = Adeudo.Cells[0].Text;
            Generales["Bimestre_1"] = Adeudo.Cells[1].Text;
            Generales["Bimestre_2"] = Adeudo.Cells[2].Text;
            Generales["Bimestre_3"] = Adeudo.Cells[3].Text;
            Generales["Bimestre_4"] = Adeudo.Cells[4].Text;
            Generales["Bimestre_5"] = Adeudo.Cells[5].Text;
            Generales["Bimestre_6"] = Adeudo.Cells[6].Text;
            Generales["Total"] = Adeudo.Cells[7].Text;
            Generales["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;

            // agregar fila a la tabla
            Dt_Generales.Rows.Add(Generales);
        }

        return Dt_Generales;
    }

    /// *************************************************************************************
    /// NOMBRE:              Asignar_Datos_Estado_Cuenta
    /// DESCRIPCIÓN:         Metodo para asignar los datos de los propietarios a una datatable
    /// PARÁMETROS:          
    ///                      
    /// USUARIO CREO:        Christian Perez Ibarra.
    /// FECHA CREO:          26/Agosto/2011 18:20 p.m.
    /// USUARIO MODIFICO:    Roberto González Oseguera
    /// FECHA MODIFICO:      16-oct-2011
    /// CAUSA MODIFICACIÓN:  Agregar campo Cuenta_Predial_ID
    /// *************************************************************************************
    protected DataTable Asignar_Datos_Estado_Cuenta()
    {
        String Total_Construccion = String.Empty;
        DataTable Dt_Estado_Cuenta = new DataTable();
        String Periodo_Final = "";

        try
        {
            DataRow Estado_Cuenta;
            Dt_Estado_Cuenta.Columns.Add("Periodo_Rezago");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Rezago");
            Dt_Estado_Cuenta.Columns.Add("Periodo_Actual");
            Dt_Estado_Cuenta.Columns.Add("Adeudo_Actual");
            Dt_Estado_Cuenta.Columns.Add("Total_Recargos_Ordinarios");
            Dt_Estado_Cuenta.Columns.Add("Honorarios");
            Dt_Estado_Cuenta.Columns.Add("Recargos_Moratorios");
            Dt_Estado_Cuenta.Columns.Add("Subtotal");
            Dt_Estado_Cuenta.Columns.Add("Descuentos_Pronto_Pago");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Ordinarios");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Recargos_Moratorios");
            Dt_Estado_Cuenta.Columns.Add("Descuento_Honorarios");
            Dt_Estado_Cuenta.Columns.Add("Total");
            Dt_Estado_Cuenta.Columns.Add("Cuenta_Predial_ID");

            Estado_Cuenta = Dt_Estado_Cuenta.NewRow();
            Estado_Cuenta["Periodo_Rezago"] = Txt_Periodo_Inicial.Text.Trim();
            //Estado_Cuenta["Adeudo_Rezago"] = Txt_Adeudo_Rezago.Text.Trim();
            if (Cmb_Hasta_Anio_Periodo.SelectedIndex > -1 && Cmb_Hasta_Bimestre_Periodo.SelectedIndex > -1)
            {
                Periodo_Final = Cmb_Hasta_Bimestre_Periodo.SelectedValue + "/" + Cmb_Hasta_Anio_Periodo.SelectedValue;
            }
            Estado_Cuenta["Periodo_Actual"] = Periodo_Final;
            //Estado_Cuenta["Adeudo_Actual"] = Txt_Adeudo_Actual.Text.Trim();
            Estado_Cuenta["Adeudo_Actual"] = Txt_Total_Impuesto.Text.Trim();
            Estado_Cuenta["Total_Recargos_Ordinarios"] = Txt_Total_Recargos_Ordinarios.Text.Trim();
            Estado_Cuenta["Recargos_Moratorios"] = Txt_Total_Recargos_Ordinarios.Text.Trim();
            Estado_Cuenta["Subtotal"] = Txt_Total.Text.Trim();
            //Detalles
            Estado_Cuenta["Total"] = Txt_Total.Text.Trim();
            Estado_Cuenta["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;

            if (Dt_Estado_Cuenta.Rows.Count == 0)
            {
                Dt_Estado_Cuenta.Rows.InsertAt(Estado_Cuenta, 0);

            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Asignar_Datos_Estado_Cuenta: " + Ex.Message);
        }
        return Dt_Estado_Cuenta;
    }

    #endregion

    #region Eventos TxtChanged

    //protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    //{
    //    Txt_Contrarecibo_TextChanged(null, EventArgs.Empty);
    //}
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Cuenta_Predial_Textchanged
    ///DESCRIPCIÓN: evento para buscar datos de la cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:30:26 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************        
    protected void Txt_Cuenta_Predial_TextChanged(object sender, EventArgs e)
    {

        DataSet Ds_Cuenta;
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                Hdn_Contrarecibo.Value = null;
                M_Cuenta_ID = Txt_Cuenta_Predial.Text.Trim();
                M_Orden_Negocio.P_Cuenta_Predial = M_Cuenta_ID;
                M_Orden_Negocio.P_Contrarecibo = null;
                Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta();
                if (Ds_Cuenta.Tables[0].Rows.Count > 0)
                {
                    if (Ds_Cuenta.Tables[0].Rows.Count > 1)
                    {
                        Estado_Botones(Const_Estado_Inicial);
                        Mensaje_Error("Se encontró mas de un registro favor de especificar por número de contrarecibo");
                    }
                    else
                    {
                        Session.Remove("Ds_Cuenta_Datos");
                        M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                        Limpiar_Todo();
                        Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
                        Cargar_Datos();
                    }
                }
                else
                {
                    Estado_Botones(Const_Estado_Inicial);
                    Limpiar_Todo();
                    Mensaje_Error("La cuenta Seleccionada no tiene movimientos pendientes");
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Eventos Grid

    private void Cargar_Grid_Variacion_Diferencias(int Page_Index)
    {
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Generar_Orden_No_Orden = Hdn_No_Orden_Variacion.Value;
            Orden_Variacion.P_Generar_Orden_Anio = Session["Anio"].ToString().Trim();
            Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();

            // cargar todos los adeudos
            Cargar_Adeudos_Actual_Diferencias(false);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Adeudos_Actual_Diferencias
    /// DESCRIPCIÓN: Lee adeudos de la cuenta y adeudos en analisis de rezago y los muestra sumados
    /// PARÁMETROS:
    ///             1. Tomar_Periodo_Final: Si es falso, se mostrarán todos los adeudos y si es verdadero, 
    ///                         se tomará el periodo final indicado en los combos hasta anio y hasta bimestre
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: Roberto González Oseguera
    /// FECHA_MODIFICÓ: 16-sep-2011
    /// CAUSA_MODIFICACIÓN: En lugar de tomar la cuota bimestral y sumarla a cada bimestre, 
    ///             se toma el importe, se divide entre los bimestres a considerar y se suma a los bimestres
    ///*******************************************************************************************************
    private void Cargar_Adeudos_Actual_Diferencias(bool Tomar_Periodo_Final)
    {
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Adeudo_Actual = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            Cls_Cat_Pre_Tabulador_Recargos_Negocio Rs_Recargos = new Cls_Cat_Pre_Tabulador_Recargos_Negocio();
            DataTable Dt_Adeudos_Actuales;
            Int32 Desde_Anio = 99999;
            Int32 Hasta_Anio = 0;
            Int32 Tmp_Desde_Anio = 0;
            Int32 Tmp_Hasta_Anio = 0;
            Int32 Mes_Actual = DateTime.Now.Month;
            Int32 Anio_Actual = DateTime.Now.Year;
            Decimal Total_Adeudo_Impuesto = 0;
            Decimal Total_Adeudo_Recargos = 0;
            DataTable Dt_Ordenes_Variacion_Aceptadas = (DataTable)Session["Dt_Ordenes_Variacion_Aceptadas"];

            String Periodo_Inicial = "-";
            String Periodo_Final = "-";
            String Anio_Adeudo = "";

            Dictionary<String, Decimal> Dic_Adeudos_Diferencias = new Dictionary<string, decimal>();
            Dictionary<String, Decimal> Dicc_Tabulador_recargos = new Dictionary<String, Decimal>();
            Dictionary<int, decimal> Dic_Cuotas_Minimas;

            Dic_Cuotas_Minimas = Obtener_Diccionario_Cuotas_Minimas();

            // obtener los adeudos actuales de la cuenta
            Dt_Adeudos_Actuales = Rs_Adeudo_Actual.Calcular_Recargos_Predial(Hdn_Cuenta_ID.Value);
            // validar que se obtuvieron adeudos de la cuenta
            if (Dt_Adeudos_Actuales != null)
            {
                if (Dt_Adeudos_Actuales.Rows.Count > 0)
                {
                    // recorrer cada fila y agregar al diccionario
                    foreach (DataRow Fila_Adeudo in Dt_Adeudos_Actuales.Rows)
                    {
                        Decimal Cuota_Bimestral = 0;
                        if (Decimal.TryParse(Fila_Adeudo["ADEUDO"].ToString(), out Cuota_Bimestral) && Cuota_Bimestral > 0)
                        {
                            if (!Dic_Adeudos_Diferencias.ContainsKey(Fila_Adeudo["PERIODO"].ToString().Trim()))
                            {
                                Dic_Adeudos_Diferencias.Add(Fila_Adeudo["PERIODO"].ToString().Trim(), Cuota_Bimestral);
                            }
                            else
                            {
                                Dic_Adeudos_Diferencias[Fila_Adeudo["PERIODO"].ToString().Trim()] += Cuota_Bimestral;
                            }
                        }
                    }
                }
            }

            // obtener adeudos del analisis de rezago en la orden de variacion
            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            // recorrer todas las ordenes aceptadas y agregarlas al diccionario
            if (Dt_Ordenes_Variacion_Aceptadas != null)
            {
                foreach (DataRow orden in Dt_Ordenes_Variacion_Aceptadas.Rows)
                {
                    Orden_Variacion.P_Generar_Orden_No_Orden = orden["No_Orden_Variacion"].ToString().Trim();
                    Orden_Variacion.P_Generar_Orden_Anio = orden["Anio"].ToString().Trim();
                    Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();

                    // procesar cada fila obtenida  para agregar bimestres al diccionario
                    foreach (DataRow Fila_Grid in Dt_Agregar_Diferencias.Rows)
                    {
                        Decimal Importe = 0;
                        String Periodo = Fila_Grid[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Trim();
                        Decimal.TryParse(Fila_Grid[Ope_Pre_Diferencias_Detalle.Campo_Importe].ToString(), out Importe);
                        if (Fila_Grid["TIPO"].ToString().Trim() == "BAJA")
                            Importe *= -1;
                        Dic_Adeudos_Diferencias = Agregar_Periodos(Periodo, Importe, Dic_Adeudos_Diferencias, Dic_Cuotas_Minimas);

                        // obtener los periodos minimo y maximo a tomar en cuenta de la tabla de diferencias
                        if (Obtener_Anio_Minimo_Maximo(
                            out Tmp_Desde_Anio,
                            out Tmp_Hasta_Anio,
                            Dt_Agregar_Diferencias,
                            Ope_Pre_Diferencias_Detalle.Campo_Periodo,
                            2))
                        {
                            // si el valor regresado por la funcion es menor, asignar, si no, dejar el mismo
                            if (Tmp_Desde_Anio < Desde_Anio)
                            {
                                Desde_Anio = Tmp_Desde_Anio;
                            }
                            // si el valor regresado por la funcion es mayor, asignarlo a la variable, si no, dejar el mismo
                            if (Tmp_Hasta_Anio > Hasta_Anio)
                            {
                                Hasta_Anio = Tmp_Hasta_Anio;
                            }
                        }

                    }
                }
            }

            // comprobar que hay adeudos para mostrar
            if (Dic_Adeudos_Diferencias.Count > 0)
            {

                Dicc_Tabulador_recargos = Rs_Recargos.Consultar_Tabulador_Recargos_Diccionario(Mes_Actual, Anio_Actual);

                if (Obtener_Anio_Minimo_Maximo(out Tmp_Desde_Anio, out Tmp_Hasta_Anio, Dt_Adeudos_Actuales, "PERIODO", 1))
                {
                    // si el valor regresado por la funcion es menor, asignar, si no, dejar el mismo
                    if (Tmp_Desde_Anio < Desde_Anio)
                    {
                        Desde_Anio = Tmp_Desde_Anio;
                    }
                    // si el valor regresado por la funcion es mayor, asignarlo a la variable, si no, dejar el mismo
                    if (Tmp_Hasta_Anio > Hasta_Anio)
                    {
                        Hasta_Anio = Tmp_Hasta_Anio;
                    }
                }

                // si se va a tomar hasta el año seleccionado
                if (Tomar_Periodo_Final == true)
                {
                    Hasta_Anio = Convert.ToInt32(Cmb_Hasta_Anio_Periodo.SelectedValue);
                }

                decimal Adeudo_Bimestre = 0;
                DataTable Dt_Adeudos = Crear_Tabla_Adeudos();
                // formar la tabla de adeudos a partir de los adeudos en el diccionario
                for (int anio = Desde_Anio; anio <= Hasta_Anio; anio++)
                {
                    DataRow Nuevo_Adeudo = Dt_Adeudos.NewRow();
                    Decimal Total_Adeudo_Anio = 0;
                    Nuevo_Adeudo[0] = anio.ToString();
                    Int32 Hasta_Bimestre = 0;

                    // si Tomar bimestre es verdadero y es el año seleccionado, establecer el bimestre seleccionado
                    if (Tomar_Periodo_Final == true && anio == Hasta_Anio)
                    {
                        Hasta_Bimestre = Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue);
                    }
                    else
                    {
                        Hasta_Bimestre = 6;
                    }

                    // agregar bimestre del diccionario a la tabla de adeudos Dt_Adeudos
                    for (int bimestre = Hasta_Bimestre; bimestre >= 1; bimestre--)
                    {
                        if (Dic_Adeudos_Diferencias.ContainsKey(bimestre.ToString() + anio.ToString()))
                        {
                            String Bimestre = bimestre.ToString() + anio.ToString();
                            Adeudo_Bimestre = Dic_Adeudos_Diferencias[Bimestre];
                            if (Adeudo_Bimestre >= 0)
                            {
                                Nuevo_Adeudo[bimestre] = Adeudo_Bimestre.ToString("#,##0.00");
                                Total_Adeudo_Anio += Adeudo_Bimestre;
                                // calcular recargos con el tabulador
                                if (Dicc_Tabulador_recargos.ContainsKey(Bimestre))
                                {
                                    Total_Adeudo_Recargos += Math.Round(((Adeudo_Bimestre * Dicc_Tabulador_recargos[Bimestre]) / 100M), 2, MidpointRounding.AwayFromZero);
                                }
                            }
                            else
                            {
                                Nuevo_Adeudo[bimestre] = "0.00";
                            }

                            if (anio == Hasta_Anio)
                            {
                                // identificar periodo final (el primero con adeudo del año Hasta_Anio)
                                if (Periodo_Final == "-" && Adeudo_Bimestre > 0)
                                {
                                    Periodo_Final = "0" + bimestre.ToString() + "/" + Hasta_Anio;
                                }
                            }
                            if (anio == Desde_Anio)
                            {
                                Periodo_Inicial = "0" + bimestre.ToString() + "/" + Desde_Anio;
                            }
                        }
                        else
                        {
                            Nuevo_Adeudo[bimestre] = "0.00";
                        }
                    }
                    Total_Adeudo_Impuesto += Total_Adeudo_Anio;
                    Nuevo_Adeudo["TOTAL"] = Total_Adeudo_Anio.ToString("#,##0.00");
                    Dt_Adeudos.Rows.Add(Nuevo_Adeudo);
                }

                // cargar tabla de adeudos en el grid
                Grid_Adeudos.DataSource = Dt_Adeudos;
                Grid_Adeudos.DataBind();

                // recalcular total (suma de parcialidades)
                foreach (GridViewRow Adeudo in Grid_Adeudos.Rows)
                {
                    decimal Adeudo_Anual = 0;
                    decimal.TryParse(Adeudo.Cells[1].Text, out Adeudo_Bimestre);
                    Adeudo_Anual += Adeudo_Bimestre;
                    decimal.TryParse(Adeudo.Cells[2].Text, out Adeudo_Bimestre);
                    Adeudo_Anual += Adeudo_Bimestre;
                    decimal.TryParse(Adeudo.Cells[3].Text, out Adeudo_Bimestre);
                    Adeudo_Anual += Adeudo_Bimestre;
                    decimal.TryParse(Adeudo.Cells[4].Text, out Adeudo_Bimestre);
                    Adeudo_Anual += Adeudo_Bimestre;
                    decimal.TryParse(Adeudo.Cells[5].Text, out Adeudo_Bimestre);
                    Adeudo_Anual += Adeudo_Bimestre;
                    decimal.TryParse(Adeudo.Cells[6].Text, out Adeudo_Bimestre);
                    Adeudo_Anual += Adeudo_Bimestre;

                    Adeudo.Cells[7].Text = Adeudo_Anual.ToString("#,##0.00");
                }

                bool Aumentar_Periodo_Final = false;
                Int32 Contador_Adeudos_Tabla = Dt_Adeudos.Rows.Count;
                Int32 Bimestre_Seleccionado = Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue);

                // recorrer los bimestres desde el bimestre seleccionado y si por lo menos un bimestre tiene adeudo, aumentar bimestre periodo final (caso cuotas minimas)
                for (int i = Bimestre_Seleccionado; i >= 1; i--)
                {
                    // si el adeudo es mayor que cero, seleccionar el bimestre anterior al de i
                    if (Contador_Adeudos_Tabla > 0 && Convert.ToDecimal(Dt_Adeudos.Rows[Contador_Adeudos_Tabla - 1][i].ToString()) > 0)
                    {
                        Aumentar_Periodo_Final = true;
                        break;
                    }
                }

                // si se va a tomar el periodo del combo y el bimestre es diferente de 6
                if (Tomar_Periodo_Final == true && Cmb_Hasta_Bimestre_Periodo.SelectedValue != "6" && Aumentar_Periodo_Final)
                {
                    Anio_Adeudo = Cmb_Hasta_Anio_Periodo.SelectedValue;
                    // recorrer los bimestres siguientes al seleccionado hasta el sexto o hasta encontrar un adeudo myor a cero
                    for (int i = Bimestre_Seleccionado + 1; i <= 6; i++)
                    {
                        // si el diccioario contiene valor para el bimestre, validar monto
                        if (Dic_Adeudos_Diferencias.ContainsKey(i.ToString() + Anio_Adeudo))
                        {
                            // si el adeudo es mayor que cero, seleccionar el bimestre anterior al de i
                            if (Dic_Adeudos_Diferencias[i.ToString() + Anio_Adeudo] > 0)
                            {
                                Cmb_Hasta_Bimestre_Periodo.SelectedValue = (i - 1).ToString();
                                break;
                            }
                        }
                        else // seleccionar i en el bimestre
                        {
                            Cmb_Hasta_Bimestre_Periodo.SelectedValue = i.ToString();
                        }
                    }
                }

                // solo si se muestran todos los bimestres (no se muestran los adeudos hasta cierto periodo)
                if (!Tomar_Periodo_Final)
                {
                    // llenar combos periodo final con valores en el grid de adeudos
                    for (int i = Contador_Adeudos_Tabla - 1; i >= 0; i--)
                    {
                        Anio_Adeudo = Dt_Adeudos.Rows[i][0].ToString();
                        if (Cmb_Hasta_Anio_Periodo.Items.FindByText(Anio_Adeudo) == null)
                        {
                            System.Web.UI.WebControls.ListItem Lst_Anio_Adeudo = new System.Web.UI.WebControls.ListItem(Anio_Adeudo, Anio_Adeudo);
                            Cmb_Hasta_Anio_Periodo.Items.Add(Lst_Anio_Adeudo);
                        }
                    }
                    // si hay adeudos en el grid, seleccionar el ultimo año y bimestre con adeudos
                    if (Contador_Adeudos_Tabla > 0)
                    {
                        Cmb_Hasta_Anio_Periodo.SelectedValue = Dt_Adeudos.Rows[Contador_Adeudos_Tabla - 1][0].ToString();
                        Cmb_Hasta_Bimestre_Periodo.SelectedValue = "6";
                    }
                }


                // mostrar datos en las cajas de texto
                Txt_Periodo_Inicial.Text = Periodo_Inicial;
                Txt_Total_Recargos_Ordinarios.Text = Math.Round(Total_Adeudo_Recargos, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
                Txt_Total_Impuesto.Text = Total_Adeudo_Impuesto.ToString("#,##0.00");
                Txt_Total.Text = (Total_Adeudo_Impuesto + Total_Adeudo_Recargos).ToString("#,##0.00");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Agregar_Periodos
    /// DESCRIPCIÓN: Lee el periodo que recibe como parametro y agrega bimestres correspondientes
    ///             con la cuota especificada en el diccionario 
    /// PARÁMETROS:
    /// 		1. Periodo: Periodo a agregar (formato: 1/2011-6/2011)
    /// 		2. Cuota_Bimestral: Valor decimal con el que se agregaran las parcialidades
    /// 		3. Dic_Adeudos_Diferencias: Diccionario en el que se van a agregar los periodos (con el formato AñoBimestre: 20111)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<String, Decimal> Agregar_Periodos(String Periodo, Decimal Importe, Dictionary<String, Decimal> Dic_Adeudos, Dictionary<int, decimal> Dic_Cuotas_Minimas)
    {
        String[] Arr_Periodos;
        String[] Arr_Bimestres;
        Decimal Importe_Bimestral = 0;
        Int32 Anio = 0;
        Int32 Divisor = 0;
        Int32 Bimestre_Inicial = 0;
        Int32 Bimestre_Final = 0;
        Decimal Sum_Adeudos_Periodo = 0;
        int Cont_Cuotas_Minimas_Periodo = 0;

        // separar periodo por guion medio
        Arr_Periodos = Periodo.Split('-');
        if (Arr_Periodos.Length >= 2)
        {
            // separar bimestre y año inicial por diagonal
            Arr_Bimestres = Arr_Periodos[0].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Inicial);
                Int32.TryParse(Arr_Bimestres[1].ToString().Trim(), out Anio);
            }
            // separar bimestre y año final por diagonal
            Arr_Bimestres = Arr_Periodos[1].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Final);
            }

            // si se obtuvieron valores, agregar al diccionario
            if (Bimestre_Final > 0 && Bimestre_Inicial > 0 && Anio > 0)
            {
                Sum_Adeudos_Periodo = 0;

                // sumar los adeudos del periodo y contar las cuotas minimas 
                for (int i = Bimestre_Inicial; i <= Bimestre_Final; i++)
                {
                    Divisor++;
                    if (Dic_Adeudos.ContainsKey(i.ToString() + Anio.ToString()))
                    {
                        // si el adeudo es igual a la cuota minima, incrementar contador
                        if (Dic_Adeudos[i.ToString() + Anio.ToString()] == Dic_Cuotas_Minimas[Anio])
                        {
                            Cont_Cuotas_Minimas_Periodo++;
                        }
                        // si el adeudo del bimestre es diferente de cero, sumar al periodo
                        if (Dic_Adeudos[i.ToString() + Anio.ToString()] != 0)
                        {
                            Sum_Adeudos_Periodo += Dic_Adeudos[i.ToString() + Anio.ToString()];
                        }
                    }
                }

                // VALIDACIONES PARA CASOS DE CUOTAS MÍNIMAS Y APLICACIÓN DE ADEUDOS
                // si el adeudo anterior contenia una sola cuota minima y el nuevo importe no es cuota minima y abarca todo el año
                if (Cont_Cuotas_Minimas_Periodo == 1 && Importe != Dic_Cuotas_Minimas[Anio] && Divisor == 6)
                {
                    // sumar la cuota minima (adeudo anterior) más el nuevo importe y prorratear
                    // se utiliza toString para redondear a dos decimales con el mismo método que en otros cálculos
                    Importe_Bimestral = Convert.ToDecimal(((Importe + Sum_Adeudos_Periodo) / Divisor).ToString("0.00"));
                    // desde bimestre inicial hasta final, agregar al diccionario
                    for (int i = Bimestre_Inicial; i <= Bimestre_Final; i++)
                    {
                        // si no existe en el diccionario, agregar cuota bimestral
                        if (!Dic_Adeudos.ContainsKey(i.ToString() + Anio.ToString()))
                        {
                            Dic_Adeudos.Add(i.ToString() + Anio.ToString(), Importe_Bimestral);
                        }
                        // si ya existe asignar nueva cuota bimestral
                        else
                        {
                            Dic_Adeudos[i.ToString() + Anio.ToString()] = Importe_Bimestral;
                        }
                    }
                }
                else
                {
                    // si  el nuevo importe mas los adeudos previos (6 bimestres) son iguales a la cuota mínima
                    if (Divisor == 6 && Sum_Adeudos_Periodo + Importe == Dic_Cuotas_Minimas[Anio])
                    {
                        // aplicar cuota minima en el primer bimestre
                        if (!Dic_Adeudos.ContainsKey("1" + Anio.ToString()))
                        {
                            Dic_Adeudos.Add("1" + Anio.ToString(), Dic_Cuotas_Minimas[Anio]);
                        }
                        else
                        {
                            Dic_Adeudos["1" + Anio.ToString()] = Dic_Cuotas_Minimas[Anio];
                        }
                        // eliminar los otros adeudos
                        for (int Cont_Bimestres = 2; Cont_Bimestres <= 6; Cont_Bimestres++)
                        {
                            if (Dic_Adeudos.ContainsKey(Cont_Bimestres.ToString() + Anio.ToString()))
                            {
                                Dic_Adeudos.Remove(Cont_Bimestres.ToString() + Anio.ToString());
                            }
                        }
                    }
                    else
                    {
                        // si el importe es cuota minima, agregar al bimestre inicial del periodo
                        if (Importe == Dic_Cuotas_Minimas[Anio])
                        {
                            // si no existe en el diccionario de adeudos, agregar el importe
                            if (!Dic_Adeudos.ContainsKey(Bimestre_Inicial.ToString() + Anio.ToString()))
                            {
                                Dic_Adeudos.Add(Bimestre_Inicial.ToString() + Anio.ToString(), Importe);
                            }
                            else // si ya existe en le diccionario, sumar el importe
                            {
                                Dic_Adeudos[Bimestre_Inicial.ToString() + Anio.ToString()] += Importe;
                            }
                        }
                        else // prorratear
                        {
                            // se utiliza toString para redondear a dos decimales con el mismo método que en otros cálculos
                            Importe_Bimestral = Convert.ToDecimal((Importe / Divisor).ToString("0.00"));
                            for (int Cont_Bimestres = Bimestre_Inicial; Cont_Bimestres <= Bimestre_Final; Cont_Bimestres++)
                            {
                                if (!Dic_Adeudos.ContainsKey(Cont_Bimestres.ToString() + Anio.ToString()))
                                {
                                    Dic_Adeudos.Add(Cont_Bimestres.ToString() + Anio.ToString(), Importe_Bimestral);
                                }
                                else
                                {
                                    Dic_Adeudos[Cont_Bimestres.ToString() + Anio.ToString()] += Importe_Bimestral;
                                }
                            }
                        }
                    }
                }
            }
        }

        return Dic_Adeudos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Anio_Minimo_Maximo
    /// DESCRIPCIÓN: Breve descripción de lo que hace la función.
    /// PARÁMETROS:
    /// 		1. Desde_Anio: Variable que almacena el valor del año menor
    /// 		2. Hasta_Anio: Variable que almacena el valor del año mayor
    /// 		3. Dt_Adeudos: Datatable con periodos a procesar
    /// 		4. Nombre_Fila: Nombre de la fila con los periodos
    /// 		5. Indice: Valor entero desde el que se obtiene el año (formatos: 1/2011 y 12011)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Boolean Obtener_Anio_Minimo_Maximo(out Int32 Desde_Anio, out Int32 Hasta_Anio, DataTable Dt_Adeudos, String Nombre_Fila, Int32 Indice)
    {
        Int32 Anio = 0;
        Desde_Anio = 99999;
        Hasta_Anio = 0;

        // para cada fila en la tabla
        foreach (DataRow Fila_Tabla in Dt_Adeudos.Rows)
        {
            // obtener y parsear el año
            String St_Anio = Fila_Tabla[Nombre_Fila].ToString().Substring(Indice, 4);
            if (Int32.TryParse(St_Anio, out Anio))
            {
                if (Anio < Desde_Anio)
                {
                    Desde_Anio = Anio;
                }
                if (Anio > Hasta_Anio)
                {
                    Hasta_Anio = Anio;
                }
            }
        }

        // regresar verdadero si se asignaron años a las variables desde y hasta anio
        if (Desde_Anio < 99999 && Hasta_Anio > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Diccionario_Cuotas_Minimas
    /// DESCRIPCIÓN: Consulta el catalogo de cuotas minimas para regresarlo como un diccionario: <Año,Cuota>
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 08-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<int, decimal> Obtener_Diccionario_Cuotas_Minimas()
    {
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Catalogo_Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Dictionary<int, decimal> Dic_Cuotas_Minimas = new Dictionary<int, decimal>();
        DataTable Dt_Cuotas;
        Int32 Anio = 0;
        Decimal Cuota = 0;

        // consultar cuotas minimas del catalogo
        Catalogo_Cuotas_Minimas.P_Anio = "";
        Dt_Cuotas = Catalogo_Cuotas_Minimas.Consultar_Cuotas_Minimas();

        // si el datatable no es nulo
        if (Dt_Cuotas != null)
        {
            // recorrer las filas del datatable para agregar cuotas al diccionario
            foreach (DataRow Fila_Cuota in Dt_Cuotas.Rows)
            {
                // si la fila contiene el año y la cuota, agregar al diccionario
                if (Int32.TryParse(Fila_Cuota["ANIO"].ToString(), out Anio) && Decimal.TryParse(Fila_Cuota["CUOTA"].ToString(), out Cuota))
                {
                    // validar que el año no se encuentra ya en el diccionario
                    if (!Dic_Cuotas_Minimas.ContainsKey(Anio))
                    {
                        Dic_Cuotas_Minimas.Add(Anio, Cuota);
                    }
                }
            }
        }

        return Dic_Cuotas_Minimas;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Adeudos
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las parcialidades
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 27-sep-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Adeudos()
    {
        DataTable Dt_Adeudos = new DataTable();
        Dt_Adeudos.Columns.Add(new DataColumn("ANIO", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre1", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre2", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre3", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre4", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre5", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("Bimestre6", typeof(String)));
        Dt_Adeudos.Columns.Add(new DataColumn("TOTAL", typeof(String)));

        return Dt_Adeudos;
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

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                //Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;

                //Consultar_Datos_Cuenta_Constancia();
                Mostrar_Liquidacion();
            }
        }
        Session["BUSQUEDA_CUENTAS_PREDIAL"] = null;
        Session["CUENTA_PREDIAL_ID"] = null;
        Session["CUENTA_PREDIAL"] = null;

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Liquidacion
    /// DESCRIPCIÓN: Consulta de ordenes de variacion para la llamada al metodo que calcula 
    ///         los adeudos de la cuenta
    /// PARÁMETROS:
    /// CREO: Nombre de programador
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Mostrar_Liquidacion()
    {
        DataSet Ds_Cuenta;
        try
        {
            Boolean Buscando_Datos = true;
            Limpiar_Todo();
            Hdn_Contrarecibo.Value = "";
            var Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            if (Buscando_Datos)
            {
                Orden_Variacion.P_Incluir_Campos_Foraneos = true;
                Orden_Variacion.P_Join_Contrarecibo = true;
                //Orden_Variacion.P_Generar_Orden_Estatus = "ACEPTADA";
                // indicar los estatus de contrarecibo que no se van a incluir (solo incluir estatus VALIDADO y POR PAGAR)
                Orden_Variacion.P_Contrarecibo_Estatus = "'PENDIENTE','RECHAZADO','GENERADO','PAGADO','POR VALIDAR'";
                Orden_Variacion.P_Filtros_Dinamicos =
                    Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                    + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '"
                    + Convert.ToString(Session["CUENTA_PREDIAL_ID"]) + "' AND "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                    + Ope_Pre_Ordenes_Variacion.Campo_No_Contrarecibo +
                    " IS NOT NULL AND "
                    + Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "."
                    + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'ACEPTADA' ";

                Orden_Variacion.P_Ordenar_Dinamico =
                    Ope_Pre_Ordenes_Variacion.Campo_Fecha_Creo + " DESC, (SELECT " +
                    Cat_Pre_Movimientos.Campo_Identificador + " FROM " +
                    Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + " WHERE " +
                    Cat_Pre_Movimientos.Campo_Movimiento_ID + " = " +
                    Ope_Pre_Ordenes_Variacion.Tabla_Ope_Pre_Ordenes_Variacion + "." +
                    Ope_Pre_Ordenes_Variacion.Campo_Movimiento_ID + ")";

                M_Orden_Negocio.P_Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                //Orden_Variacion.P_Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Orden_Variacion.P_Año = DateTime.Now.Year;
                DataTable Dt_Orden_Variacion = Orden_Variacion.Consultar_Ordenes_Variacion_Contrarecibos();
                Session["Dt_Ordenes_Variacion_Aceptadas"] = Dt_Orden_Variacion;

                M_Orden_Negocio.P_Contrarecibo = null;
                Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta_Sin_Contrarecibo();
                if (Ds_Cuenta.Tables[0].Rows.Count > 0 && Dt_Orden_Variacion.Rows.Count > 0)
                {
                    Session["No_Orden_Variacion_ID"] =
                        Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                    Hdn_Contrarecibo.Value = Dt_Orden_Variacion.Rows[0]["No_Contrarecibo"].ToString().Trim();
                    Hdn_No_Orden_Variacion.Value = Dt_Orden_Variacion.Rows[0]["No_Orden_Variacion"].ToString().Trim();
                    Session["Anio"] = Dt_Orden_Variacion.Rows[0]["Anio"].ToString().Trim();
                    Session["Observaciones"] = Dt_Orden_Variacion.Rows[0]["Observaciones"].ToString().Trim();

                    Session.Remove("Ds_Cuenta_Datos");
                    M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();

                    Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
                    if (Cargar_Datos())
                    {
                        Cargar_Variacion();
                        Configurar_Estatus_Controles(false, true);
                        Configurar_Edicion_Controles(false);
                        //Configurar_Controles_Validacion(false);
                        Buscando_Datos = false;
                    }

                }
            }

            if (Buscando_Datos)
            {
                Mensaje_Error("La cuenta predial no tiene órdenes de variación con estatus ACEPTADA, pendientes de aplicar.");
                Limpiar_Todo();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

}