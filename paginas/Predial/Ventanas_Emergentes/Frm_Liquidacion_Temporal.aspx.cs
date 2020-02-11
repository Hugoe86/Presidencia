using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OracleClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using Operacion_Predial_Orden_Variacion.Negocio;
using Presidencia.Catalogo_Tabulador_Recargos.Negocio;
using Presidencia.Constantes;
using Presidencia.Reportes;
using Presidencia.Sessiones;
using SharpContent.ApplicationBlocks.Data;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;

public partial class paginas_Predial_Ventanas_Emergentes_Frm_Liquidacion_Temporal : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;

    private static String M_Cuenta_ID;
    private static Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    private static DataTable Dt_Agregar_Co_Propietarios = new DataTable();
    private static DataTable Dt_Agregar_Diferencias = new DataTable();


    #endregion

    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones                
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de cuentas predial
                //Cargar_Grid_Ordenes_Variacion(0);
                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
            }

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
        Txt_Periodo_Final.Text = "";
        Txt_Total_Recargos_Ordinarios.Text = "";
        Txt_Total_Impuesto.Text = "";
        Txt_Total.Text = "";

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
        Txt_Periodo_Final.ToolTip = "";
        Txt_Total_Recargos_Ordinarios.ToolTip = "";
        Txt_Total_Impuesto.ToolTip = "";
        Txt_Total.ToolTip = "";

        //QUITA LOS IDs
        Hdn_Calle_ID.Value = "";
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
    ///NOMBRE DE LA METODO: LLenar_Combo_Id
    ///        DESCRIPCIÓN: llena todos los combos
    ///         PARAMETROS: 1.- Obj_DropDownList: Combo a llenar
    ///                     2.- Dt_Temporal: DataTable genarada por una consulta a la base de datos
    ///                     3.- Texto: nombre de la columna del dataTable que mostrara el texto en el combo
    ///                     3.- Valor: nombre de la columna del dataTable que mostrara el valor en el combo
    ///                     3.- Seleccion: Id del combo el cual aparecera como seleccionado por default
    ///               CREO: Jesus S. Toledo Rdz.
    ///         FECHA_CREO: 06/9/2010
    ///           MODIFICO:
    ///     FECHA_MODIFICO:
    /// CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row["DESCRIPCION"].ToString(), row["ID"].ToString()));
            }
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList, DataTable Dt_Temporal, String P_Text, String P_Value)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            foreach (DataRow row in Dt_Temporal.Rows)
            {
                Obj_DropDownList.Items.Add(new ListItem(row[P_Text].ToString(), row[P_Value].ToString()));
            }
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    public void Llenar_Combo_ID(DropDownList Obj_DropDownList)
    {
        try
        {
            Obj_DropDownList.Items.Clear();
            Obj_DropDownList.Items.Add(new ListItem("<SELECCIONE>", "0"));
            Obj_DropDownList.SelectedValue = "0";
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
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

                Configurar_Estatus_Controles(true);

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
    private void Configurar_Estatus_Controles(Boolean Estatus_Activo)
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
        Txt_Periodo_Final.Enabled = Estatus_Activo;
        Txt_Total_Recargos_Ordinarios.Enabled = Estatus_Activo;
        Txt_Total_Impuesto.Enabled = Estatus_Activo;
        Txt_Total.Enabled = Estatus_Activo;

        //Grid_Ordenes_Variacion.Visible = !Estatus_Activo;
        //Tr_Encabezado_Ordenenes.Visible = !Estatus_Activo;
        Panel.Visible = Estatus_Activo;
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
        Txt_Periodo_Final.ReadOnly = !Estatus_Edicion;
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
            Configurar_Estatus_Controles(true);
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
                Busqueda_Propietarios();
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
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion].ToString();
            Txt_Colonia_Propietario.Text =
                Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion].ToString();
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
            Txt_Valor_Fiscal.Text = Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal].ToString();
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
                Hdn_Propietario_ID.Value = dataTable.Rows[0]["CONTRIBUYENTE"].ToString();
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["CONTRIBUYENTE"].ToString(); ;

                Txt_Nombre_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                if (dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                {
                    M_Orden_Negocio.P_Tipo_Propietario = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                }

                Txt_RFC_Propietario.Text = dataTable.Rows[0]["RFC"].ToString();
                M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() != String.Empty)
                {
                    Txt_Domicilio_Foraneo.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
                    M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString();
                }
                Txt_Estado_Propietario.Text = (dataTable.Rows[0]["NOMBRE_ESTADO"].ToString());
                M_Orden_Negocio.P_Estado_Propietario = (dataTable.Rows[0]["NOMBRE_ESTADO"].ToString());
                Txt_Colonia_Propietario.Text = dataTable.Rows[0]["NOMBRE_COLONIA"].ToString();
                M_Orden_Negocio.P_Colonia_Propietario = dataTable.Rows[0]["NOMBRE_COLONIA"].ToString();
                Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["NOMBRE_CIUDAD"].ToString();
                M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["NOMBRE_CIUDAD"].ToString();
                Txt_Calle_Propietario.Text = dataTable.Rows[0]["NOMBRE_CALLE"].ToString();
                M_Orden_Negocio.P_Domilicio_Propietario = dataTable.Rows[0]["NOMBRE_CALLE"].ToString();
                Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
                M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
                Txt_CP.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString();
                M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString();
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
        Cls_Ope_Pre_Resumen_Predio_Negocio Rs_Consulta_Ope_Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio(); //Variable de conexión hacia la capa de Negocios
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
                    Txt_Cuota_Bimestral.Text = "$ " + String.Format("{0:#,###,###.00}", Cuota_Bimestral);
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
                Txt_Valor_Fiscal.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal]).ToString("##,###,##0.00");

                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
                {
                    Hdn_Cuota_Minima_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
                }

                //Seccion de carga de datos de los copropietarios
                //if (dataTable.Rows[0]["LISTA_PROPIETARIOS_ID"].ToString() != "")
                //{
                M_Orden_Negocio.P_Orden_Variacion_ID = Hdn_No_Orden_Variacion.Value;
                //M_Orden_Negocio.P_Año = Convert.ToDateTime(Grid_Ordenes_Variacion.Rows[Grid_Ordenes_Variacion.SelectedIndex].Cells[5].Text).Year;
                M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
                Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios;

                //}
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
                Configurar_Estatus_Controles(false);
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
                    Configurar_Estatus_Controles(false);
                    //Configurar_Controles_Validacion(false);
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Eventos Combos

    #endregion

    #region Metodos Reportes

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Generar_Reporte
    ///DESCRIPCIÓN:          Carga el data set físico con el cual se genera el Reporte especificado
    ///PARAMETROS:           1.-Data_Set_Consulta_Inventario.- Contiene la informacion de la consulta a la base de datos
    ///                      2.-Ds_Reporte_Stock, Objeto que contiene la instancia del Data set fisico del Reporte a generar
    ///                      3.-Nombre_Reporte, contiene el nombre del Reporte a mostrar en pantalla
    ///CREO:                 Salvador Hernández Ramírez
    ///FECHA_CREO:           17/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Generar_Reporte(DataSet Ds_Reporte_Ordenes_Salida)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";

        try
        {
            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Orden_Variacion.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Orden_Variacion_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            Cls_Reportes Reportes = new Cls_Reportes();
            Reportes.Generar_Reporte(ref Ds_Reporte_Ordenes_Salida, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
            Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
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

    public String Get_Cuenta_ID()
    {
        if (!String.IsNullOrEmpty(Hdn_Cuenta_ID.Value))
        {
            Int32 Valor = 0;
            Int32.TryParse(Hdn_Cuenta_ID.Value, out Valor);
            return String.Format("{0:0000000000}", Valor);

        }
        return null;
    }

    public String Get_Contra_ID()
    {
        if (!String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
        {
            Int32 Valor = 0;
            Int32.TryParse(Hdn_Contrarecibo.Value, out Valor);
            return String.Format("{0:0000000000}", Valor);
        }
        return null;
    }

    private void Busqueda_Propietarios()
    {
        DataSet Ds_Prop;
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial_ID = Get_Cuenta_ID();
            M_Orden_Negocio.P_Contrarecibo = Get_Contra_ID();
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
    ///NOMBRE DE LA FUNCIÓN: Txt_Contrarecibo_TextChanged
    ///DESCRIPCIÓN: evento para buscar datos de la cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:30:26 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    //protected void Txt_Contrarecibo_TextChanged(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        if (Panel.Visible)
    //        {
    //            Btn_Salir_Click(sender, null);
    //        }
    //        Mostrar_Liquidacion();
    //    }
    //    catch (Exception Ex)
    //    {
    //        Mensaje_Error(Ex.Message);
    //    }
    //}

    #endregion

    #region Eventos Grid
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Diferencias_PageIndexChanging
    ///DESCRIPCIÓN          : paginar grid de Diferencias
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 04/Septiembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Diferencias_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Variacion_Diferencias(e.NewPageIndex);
    }

    private void Cargar_Grid_Variacion_Diferencias(int Page_Index)
    {
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Generar_Orden_No_Orden = Hdn_No_Orden_Variacion.Value;
            Orden_Variacion.P_Generar_Orden_Anio = Session["Anio"].ToString().Trim();
            Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();

            Cargar_Adeudos_Actual_Diferencias();
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
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Adeudos_Actual_Diferencias()
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

            String Periodo_Inicial = "-";
            String Periodo_Final = "-";

            Dictionary<String, Decimal> Dic_Adeudos_Diferencias = new Dictionary<string, decimal>();
            Dictionary<String, Decimal> Dicc_Tabulador_recargos = new Dictionary<String, Decimal>();

            // obtener adeudos del analisis de rezago en la orden de variacion
            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Generar_Orden_No_Orden = Hdn_No_Orden_Variacion.Value;
            //Orden_Variacion.P_Generar_Orden_Anio = Convert.ToDateTime(Grid_Ordenes_Variacion.Rows[Grid_Ordenes_Variacion.SelectedIndex].Cells[5].Text).Year.ToString();
            Dt_Agregar_Diferencias = Orden_Variacion.Consulta_Diferencias();

            // procesar cada fila obtenida  para agregar bimestres al diccionario
            foreach (DataRow Fila_Grid in Dt_Agregar_Diferencias.Rows)
            {
                Decimal Cuota_Bimestral_Prediodo = 0;
                String Periodo = Fila_Grid[Ope_Pre_Diferencias_Detalle.Campo_Periodo].ToString().Trim();
                Decimal.TryParse(Fila_Grid[Ope_Pre_Diferencias_Detalle.Campo_Cuota_Bimestral].ToString(), out Cuota_Bimestral_Prediodo);
                if (Fila_Grid["TIPO"].ToString().Trim() == "BAJA")
                    Cuota_Bimestral_Prediodo *= -1;
                Dic_Adeudos_Diferencias = Agregar_Periodos(Periodo, Cuota_Bimestral_Prediodo, Dic_Adeudos_Diferencias);
            }

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

            DataTable Dt_Adeudos = Crear_Tabla_Adeudos();
            // formar la tabla de adeudos a partir de los adeudos en el diccionario
            for (int anio = Hasta_Anio; anio >= Desde_Anio; anio--)
            {
                DataRow Nuevo_Adeudo = Dt_Adeudos.NewRow();
                Decimal Total_Adeudo_Anio = 0;
                Nuevo_Adeudo[0] = anio.ToString();
                // agregar bimestre del diccionario
                for (int bimestre = 6; bimestre >= 1; bimestre--)
                {
                    if (Dic_Adeudos_Diferencias.ContainsKey(bimestre.ToString() + anio.ToString()))
                    {
                        String Bimestre = bimestre.ToString() + anio.ToString();
                        Nuevo_Adeudo[bimestre] = Dic_Adeudos_Diferencias[Bimestre].ToString("#,##0.00");
                        Total_Adeudo_Anio += Dic_Adeudos_Diferencias[Bimestre];
                        // calcular recargos con el tabulador
                        if (Dicc_Tabulador_recargos.ContainsKey(Bimestre))
                        {
                            Total_Adeudo_Recargos += (Dic_Adeudos_Diferencias[Bimestre] * Dicc_Tabulador_recargos[Bimestre]) / 100;
                        }
                        // identificar periodo inicial
                        if (Periodo_Final == "-" && Dic_Adeudos_Diferencias[Bimestre] > 0)
                        {
                            Periodo_Final = "0" + bimestre.ToString() + "/" + anio.ToString();
                        }
                        Periodo_Inicial = "0" + bimestre.ToString() + "/" + anio.ToString();
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
            GridView2.DataSource = Dt_Adeudos;
            GridView2.DataBind();

            // recalcular total (suma de parcialidades)
            decimal Adeudo_Bimestre;
            foreach (GridViewRow Adeudo in GridView2.Rows)
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

            // mostrar datos en las cajas de texto
            Txt_Periodo_Final.Text = Periodo_Final;
            Txt_Periodo_Inicial.Text = Periodo_Inicial;
            Txt_Total_Recargos_Ordinarios.Text = Total_Adeudo_Recargos.ToString("#,##0.00");
            Txt_Total_Impuesto.Text = Total_Adeudo_Impuesto.ToString("#,##0.00");
            Txt_Total.Text = (Total_Adeudo_Impuesto + Total_Adeudo_Recargos).ToString("#,##0.00");
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
    private Dictionary<String, Decimal> Agregar_Periodos(String Periodo, Decimal Cuota_Bimestral, Dictionary<String, Decimal> Dic_Adeudos)
    {
        String[] Arr_Periodos;
        String[] Arr_Bimestres;
        Int32 Anio = 0;
        Int32 Bimestre_Inicial = 0;
        Int32 Bimestre_Final = 0;
        Dictionary<String, Decimal> Dic_Adeudos_Periodo = Dic_Adeudos;

        // separar periodo por guion medio
        Arr_Periodos = Periodo.Split('-');
        if (Arr_Periodos.Length >= 2)
        {
            // separar bimestre y año por diagonal
            Arr_Bimestres = Arr_Periodos[0].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Inicial);
                Int32.TryParse(Arr_Bimestres[1].ToString().Trim(), out Anio);
            }
            // separar bimestre y año por diagonal
            Arr_Bimestres = Arr_Periodos[1].Trim().Split('/');
            if (Arr_Bimestres.Length >= 2)
            {
                Int32.TryParse(Arr_Bimestres[0].ToString().Trim(), out Bimestre_Final);
            }

            // si se obtuvieron valores, agregar al diccionario
            if (Bimestre_Final > 0 && Bimestre_Inicial > 0 && Anio > 0)
            {
                // desde bimestre inicial hasta final, agregar al diccionario
                for (int i = Bimestre_Inicial; i <= Bimestre_Final; i++)
                {
                    // si no existe en el diccionario, agregar
                    if (!Dic_Adeudos.ContainsKey(i.ToString() + Anio.ToString()))
                    {
                        Dic_Adeudos.Add(i.ToString() + Anio.ToString(), Cuota_Bimestral);
                    }
                    // si ya existe, sumar (por si los bimestres se traslapan)
                    else
                    {
                        Dic_Adeudos[i.ToString() + Anio.ToString()] += Cuota_Bimestral;
                    }
                }
            }
        }

        return Dic_Adeudos_Periodo;
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


    private String Obtener_Periodos_Bimestre(String Periodos, out Boolean Periodo_Corriente_Validado, out Boolean Periodo_Rezago_Validado)
    {
        String Periodo = "";
        int Indice = 0;
        Periodo_Corriente_Validado = false;
        Periodo_Rezago_Validado = false;

        if (Periodos.IndexOf("-") >= 0)
        {
            if (Periodos.Split('-').Length == 2)
            {
                //Valida el segundo nodo del arreglo
                if (Periodos.Split('-').GetValue(1).ToString().IndexOf("/") >= 0)
                {
                    Periodo = Periodos.Split('-').GetValue(0).ToString().Trim().Substring(0, 1);
                    Periodo += "-";
                    Periodo += Periodos.Split('-').GetValue(1).ToString().Trim().Substring(0, 1);
                    Periodo_Rezago_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Split('-').GetValue(0).ToString().Replace("/", "-").Trim();
                    Periodo_Corriente_Validado = true;
                }
            }
            else
            {
                if (Periodos.Contains("/"))
                {
                    Indice = Periodos.IndexOf("/");
                    Periodo = Periodos.Substring(Indice - 1, 1);
                    Periodo += "-";
                    Indice = Periodos.IndexOf("/", Indice + 1);
                    Periodo += Periodos.Substring(Indice - 1, 1);
                    Periodo_Rezago_Validado = true;
                }
                else
                {
                    Periodo = Periodos.Substring(0, 3);
                    Periodo_Corriente_Validado = true;
                }
            }
        }
        return Periodo;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Ordenes_Variacion_PageIndexChanging
    ///DESCRIPCIÓN          : Manejo de la paginación del GridView
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 20/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    //protected void Grid_Ordenes_Variacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    try
    //    {
    //        Grid_Ordenes_Variacion.SelectedIndex = (-1);
    //        Cargar_Grid_Ordenes_Variacion(e.NewPageIndex);
    //    }
    //    catch (Exception Ex)
    //    {
    //        Lbl_Encabezado_Error.Text = Ex.Message;
    //        Lbl_Mensaje_Error.Text = "";
    //    }
    //}
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
            }
        }

        //Consultar_Datos_Cuenta_Constancia();
        Mostrar_Liquidacion();
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
                        Configurar_Estatus_Controles(true);
                        Configurar_Edicion_Controles(false);
                        //Configurar_Controles_Validacion(false);
                        Buscando_Datos = false;
                    }

                }
            }

            if (Buscando_Datos)
            {
                Mensaje_Error("no se encontraron datos.");
                Limpiar_Todo();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Metodos Validaciones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Revisar_Actualizaciones
    ///DESCRIPCIÓN: Realiza la comparacion de dos data set para 
    ///verificar las diferencias de cada uno de sus campos
    ///PARAMETROS: 1.-Ds_Anterior, es el primer data a comparar
    ///            2.-Ds_Actual, segundo data set a comparar
    ///CREO: Silvia Morales Portuhondo
    ///FECHA_CREO: 17/Septiembre/2010 
    ///MODIFICO: Jesus Toledo
    ///FECHA_MODIFICO: 15-Agosto-2011
    ///CAUSA_MODIFICACIÓN:se adapto para agregar variaciones
    ///                   al datatable del objeto negocio
    ///                   en caso de encotrar atualizaciones
    ///*******************************************************************************
    public static void Revisar_Actualizaciones(DataSet Ds_Anterior, DataSet Ds_Actual)
    {
        String Dato1 = "";
        String Dato2 = "";
        for (int i = 0; i < Ds_Actual.Tables[0].Columns.Count; i++)
        {
            Dato1 = Ds_Anterior.Tables[0].Rows[0].ItemArray[i].ToString();
            Dato2 = Ds_Actual.Tables[0].Rows[0].ItemArray[i].ToString();
            if (Dato1 != Dato2)
            {
                M_Orden_Negocio.Agregar_Variacion(Ds_Actual.Tables[0].Columns[i].ToString(), Dato2);
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
    #endregion


    protected void Grid_Diferencias_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //Double Importe = 0;
            //Double Cuota_Bimestral = 0;
            Double Tasa = 0;

            //if (e.Row.Cells[6].Text.Trim() != "")
            //{
            //    Importe = Convert.ToDouble(e.Row.Cells[6].Text.Replace("$", ""));
            //}
            //Cuota_Bimestral = Importe / 6;
            //e.Row.Cells[7].Text = String.Format("{0:c2}", Cuota_Bimestral);

            if (e.Row.Cells[6].Text.Trim() != "")
            {
                Tasa = Convert.ToDouble(e.Row.Cells[6].Text.Replace("%", ""));
            }
            e.Row.Cells[6].Text = String.Format("{0:p2}", Tasa / 100);
        }
    }
}
