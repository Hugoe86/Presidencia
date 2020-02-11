using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Bitacora_Eventos;
using Operacion_Predial_Orden_Variacion.Negocio;
using System.Threading;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Operacion.Predial_Tasas_Anuales.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Catalogo_Uso_Suelo.Negocio;
using Presidencia.Catalogo_Estados_Predio.Negocio;
using CrystalDecisions.Shared;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Catalogo_Tipos_Predio.Negocio;
using Presidencia.Catalogo_Movimientos.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Traslado.Negocio;
using System.Text;
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using System.Globalization;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;

public partial class paginas_Predial_Frm_Ope_Pre_Orden_Variacion : System.Web.UI.Page
{
    //Region de declaracion de constantes usadas para el estado de los botones
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 2;
    private const int Const_Estado_Modificar = 3;
    public static int Const_Anio_Corriente = 0;
    private const int Const_Estado_Busqueda_Cuenta = 4;
    #endregion
    //Region del Page Load
    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");
            if (!IsPostBack)
            {
                //Se define el Año Corriente                
                Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
                //Si no tiene ningun valor se consulta de nuevo si no se salta esta consulta
                if (Const_Anio_Corriente <= 0)
                    Const_Anio_Corriente = Anio_Corriente.Consultar_Anio_Corriente();
                //si no hay resultado en la consulta del año se toma el actual del sistema
                if (Const_Anio_Corriente <= 0)
                    Const_Anio_Corriente = DateTime.Today.Year;
                //Variable en la que se baja el tipo de orden Traslado o Predial dependiento del Menu
                String Tipo_Orden_Variacion = "Traslado";
                if (Request.QueryString["Opcion"] != null)
                {
                    //Se baja el QueryString que viene en la URL
                    Tipo_Orden_Variacion = HttpUtility.HtmlDecode(Request.QueryString["Opcion"]).Trim();
                }
                //Session y HdnField para almacenar el tipo de OV
                Session["Opcion_Tipo_Orden"] = Tipo_Orden_Variacion;
                Hdn_Opcion_Tipo_Orden.Value = Session["Opcion_Tipo_Orden"].ToString();
                //Declaracion de variables para Abrir ventanas emergentes
                String Ventana_Modal;
                String Ventana_Cuotas;
                String Ventana;
                String Propiedades;
                //Se limpian las sesiones de las diferencias y el tipo de contribuyente variables usadas para la modificacion de la diferencia de contruccion
                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");
                Session.Remove("Dt_Agregar_Diferencias");
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones                
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de cuentas predial
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:yes');";
                Btn_Mostrar_Busqueda_Cuentas.Attributes.Add("onclick", Ventana_Modal);
                Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE','SUSPENDIDA')";
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de Propietarios
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Orden_Variacion/Frm_Menu_Pre_Propietarios.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:780px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Propietarios.Attributes.Add("onclick", Ventana_Modal);
                Btn_Busqueda_Co_Propietarios.Attributes.Add("onclick", Ventana_Modal);
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de Tasas
                Ventana = "Abrir_Ventana_Modal('Ventanas_Emergentes/Orden_Variacion/Frm_Menu_Pre_Tasas.aspx";
                Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Tasas.Attributes.Add("onclick", Ventana + "?Fecha=False'" + Propiedades);
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de Cuotas Minimas
                Ventana_Cuotas = "Abrir_Ventana_Modal('Ventanas_Emergentes/Orden_Variacion/Frm_Menu_Pre_Cuotas_Minimas.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Cuota_Minima.Attributes.Add("onclick", Ventana_Cuotas);
                //Scrip para mostrar Ventana Modal para la Busqueda Avanzada de colonias y Calles
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Seleccionar_Calle.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Seleccionar_Colonia.Attributes.Add("onclick", Ventana_Modal);
                Txt_Observaciones_Cuenta.Attributes.Add("onkeypress", " Validar_Longitud_Texto(this, 500);");
                Txt_Observaciones_Cuenta.Attributes.Add("onkeyup", " Validar_Longitud_Texto(this, 500);");
                Txt_Comentarios.Attributes.Add("onkeypress", " Validar_Longitud_Texto(this, 500);");
                Txt_Comentarios.Attributes.Add("onkeyup", " Validar_Longitud_Texto(this, 500);");
                Cargar_Ventana_Emergente_Resumen_Predio();
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Verificar Mismo Domicilio", "Mismo_Domicilio();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Verificar Mismo Domicilio", "Foraneo();", true);
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Verificar Domicilio Foraneo", "Cuota_Fija();", true);
                Mensaje_Error();
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error("Error al cargar la Página");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Adeudo_Diferencias
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente de los Adeudos con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Adeudo_Diferencias()
    {
        String Ventana_Modal = "Abrir_Vista_Adeudos('Ventanas_Emergentes/Resumen_Predial/Frm_Adeudo_Diferencias.aspx";
        String Propiedades = ", 'resizable=yes,status=no,width=580,scrollbars=yes');";
        Btn_Vista_Adeudos.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);

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
        String Propiedades = ", 'resizable=yes,status=no,width=750,scrollbars=yes');";
        Btn_Resumen_Cuenta.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }

    #endregion
    //Region de Metodos de la Pagina
    #region Metodos
    //Region de Metodos Generales de configuración del Formulario
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
    private void Limpiar_Formulario()
    {
        //Cajas de Texto datos Generales
        Txt_Cuenta_Predial.Text = "";
        Txt_Cta_Origen.Text = "";
        Txt_Calle_Cuenta.Text = "";
        Txt_Colonia_Cuenta.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Catastral.Text = "";
        Lbl_Ultimo_Movimiento.Text = "";
        //Cajas de Texto datos Propietario
        Txt_Nombre_Propietario.Text = "";
        Txt_Rfc_Propietario.Text = "";
        Txt_Colonia_Propietario.Text = "";
        Txt_Calle_Propietario.Text = "";
        Txt_Numero_Exterior_Propietario.Text = "";
        Txt_Numero_Interior_Propietario.Text = "";
        Txt_CP.Text = "";
        Txt_Estado.Text = "";
        Txt_Ciudad.Text = "";
        //Cajas de Texto datos Impuestos
        Txt_Valor_Fiscal.Text = "";
        Txt_Tasa_Descripcion.Text = "";
        Txt_Tasa_Porcentaje.Text = "";
        Txt_Periodo_Corriente.Text = "";
        Txt_Fecha_Inicial.Text = "";
        Txt_Fecha_Avaluo.Text = "";
        //Cajas de Texto detalles de Cuota Fija
        Txt_Plazo.Text = "";
        Txt_Fundamento.Text = "";
        Txt_Co_Propietario.Text = "";
        Txt_Fecha_Def.Text = "";
        //Observaciones
        Txt_Observaciones_Cuenta.Text = "";
        Txt_Comentarios.Text = "";
        //Valores Numéricos
        Txt_Superficie_Construida.Text = "0.00";
        Txt_Superficie_Total.Text = "0.00";
        Txt_Costo_M2.Text = "0.00";
        Txt_Dif_Construccion.Text = "0.00";
        Txt_Cuota_Anual.Text = "0.00";
        Txt_Cuota_Bimestral.Text = "0.00";
        Txt_Porcentaje_Excencion.Text = "0.00";
        Txt_Cuota_Minima.Text = "0.00";
        Txt_Exedente_Construccion.Text = "0.00";
        Txt_Tasa_Exedente_Construccion.Text = "0.00";
        Txt_Excedente_Construccion_Total.Text = "0.00";
        Txt_Excedente_Valor.Text = "0.00";
        Txt_Tasa_Excedente_Valor.Text = "0.00";
        Txt_Tasa_Valor_Total.Text = "0.00";
        Txt_Total_Cuota_Fija.Text = "0.00";
        Txt_Excedente_Construccion_Total.Text = "0.00";
        Txt_Tasa_Valor_Total.Text = "0.00";
        //Etiquetas de Texto de Resumen de Analisis de Rezago
        Txt_Desde_Periodo_Corriente.Text = "0";
        Txt_Desde_Anio_Periodo_Corriente.Text = "0";
        Txt_Hasta_Periodo_Corriente.Text = "0";
        Txt_Hasta_Anio_Periodo_Corriente.Text = "0";
        Txt_Alta_Periodo_Corriente.Text = "0";
        Txt_Baja_Periodo_Corriente.Text = "0";
        Txt_Desde_Periodo_Regazo.Text = "0";
        Lbl_P_C_Anio_Inicio.Text = "0";
        Txt_Hasta_Periodo_Regazo.Text = "0";
        Lbl_P_C_Anio_Final.Text = "0";
        Txt_Alta_Periodo_Regazo.Text = "0";
        Txt_Baja_Periodo_Regazo.Text = "0";
        Cmb_P_C_Bimestre_Final.SelectedIndex = 5;
        Cmb_P_C_Anio.SelectedIndex = 0;
        Cmb_P_R_Bimestre_Inicial.SelectedIndex = 0;
        Cmb_P_R_Anio_Inicial.SelectedIndex = 0;
        Cmb_P_R_Bimestre_Final.SelectedIndex = 5;
        Cmb_P_R_Anio_Final.SelectedIndex = 0;
        //Limpiar Combos
        Cmb_Tipos_Movimiento.SelectedIndex = 0;
        Cmb_Tipos_Predio.SelectedIndex = 0;
        Cmb_Usos_Predio.SelectedIndex = 0;
        Cmb_Estados_Predio.SelectedIndex = 0;
        Cmb_Estatus.SelectedIndex = 0;
        Cmb_Efectos.SelectedIndex = 0;
        Cmb_Efectos_Numero.SelectedIndex = 0;
        Cmb_Tipos_Propietario.SelectedIndex = 0;
        Cmb_Domicilio_Foraneo.SelectedIndex = 0;
        Chk_Mismo_Domicilio.Checked = false;
        Chk_Beneficio_Completo.Checked = false;
        Cmb_Solicitante.SelectedIndex = 0;
        Cmb_Financiado.SelectedIndex = 0;
        //Limpiar Hidden fields
        Hdn_Propietario_ID.Value = null;
        Hdn_Cuenta_ID.Value = null;
        Hdn_Cuenta_ID_Temp.Value = null;
        Hdn_Tasa_ID.Value = null;
        Hdn_Tasa_Dif.Value = null;
        Hdn_Cuota_Minima.Value = null;
        Hdn_Contrarecibo.Value = null;
        Hdn_Excedente_Valor.Value = null;
        Tope_Para_Excedente.Value = null;
        Hdn_Orden_Variacion.Value = null;
        Hdn_Orden_Variacion_Anio.Value = null;
        Hdn_Tasa_General_ID.Value = null;
        Hdn_Colonia_ID.Value = null;
        Hdn_Calle_ID.Value = null;
        Hdn_Colonia_ID_Notificacion.Value = null;
        Hdn_Calle_ID_Notificacion.Value = null;
        Hdn_Respuesta_Confirmacion.Value = "";
        Hdn_Superficie_Construccion.Value = "";
        Hdn_Propietario_Validacion_Superficie.Value = "";

        //Limpiar Grids
        //Co-Propietarios
        Grid_Copropietarios.DataSource = null;
        Grid_Copropietarios.DataBind();
        //Diferencias
        Grid_Diferencias.DataSource = null;
        Grid_Diferencias.DataBind();
        //Historial
        Grid_Historial_Observaciones.DataSource = null;
        Grid_Historial_Observaciones.DataBind();
        CE_Txt_Fecha_Inicial.SelectedDate = null;
        ////CE_Txt_Fecha_Avaluo.SelectedDate = null;
        Txt_Fecha_Avaluo.Text = "";
        Cargar_Couta_Minima();
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Session_Remove
    ///DESCRIPCION : Elimina las sessiones usadas para guardar datos de la orden de variacion
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Session_Remove()
    {
        Session.Remove("Dt_Agregar_Diferencias");
        Session.Remove("Dt_Agregar_Co_Propietarios");
        Session.Remove("Dt_Contribuyentes");
        Session.Remove("M_Orden_Negocio");
        Session.Remove("M_Orden_Negocio");
        Session.Remove("P_Generar_Orden_Dt_Detalles");
        Session.Remove("Ds_Cuenta_Datos_Orden");
        Session.Remove("Contrarecibo_Traslado");
        Session.Remove("Dt_Contrarecibo");
        Session.Remove("Quitar_Cuota_Fija");
        Session.Remove("Cuota_Fija_Anterior");
        Session.Remove("Cuota_Fija_Nueva");
        Session.Remove("Cuota_Bimestral");
        Session.Remove("Cuota_Anual");
        Session.Remove("Validar_Ordenes_Variacion_Directas_Cuenta");
        Session["Cuota_Fija_Anterior"] = "";
        Session["Cuota_Fija_Nueva"] = "";
        Session["Estatus_Cuenta"] = "NINGUNO";
        Agregar_Columnas();
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
        Lbl_Mensaje_Error_Diferencias.Text = "";
        Lbl_Error_Cuota_Fija.Text = "";
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
        DataSet Ds_Cargar_combos = null;
        try
        {
            //Obtiene La variable de session con los datos para la carga de los combos
            if (Session["Ds_Consulta_Combos"] != null)
            {
                Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
            }
            else
            {
                Consulta_Combos();
                Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
            }

            Llenar_Combo_ID(Cmb_Estatus);
            Llenar_Combo_ID(Cmb_Efectos);
            Llenar_Combo_ID(Cmb_Tipos_Propietario);
            Llenar_Combo_ID(Cmb_Domicilio_Foraneo);
            if (!Session["Opcion_Tipo_Orden"].ToString().Contains("Predial"))
            {
                Llenar_Combo_ID(Cmb_Tipos_Movimiento, Ds_Cargar_combos.Tables["Dt_Tipos_Movimiento"]);
            }
            else
            {
                Llenar_Combo_ID(Cmb_Tipos_Movimiento, Ds_Cargar_combos.Tables["Dt_Tipos_Movimiento_Predial"]);
            }
            Llenar_Combo_ID(Cmb_Tipos_Predio, Ds_Cargar_combos.Tables["Dt_Tipos_Predio"]);
            Llenar_Combo_ID(Cmb_Usos_Predio, Ds_Cargar_combos.Tables["Dt_Usos_Predio"]);
            Llenar_Combo_ID(Cmb_Estados_Predio, Ds_Cargar_combos.Tables["Dt_Estados_Predio"]);
            Llenar_Combo_ID(Cmb_Financiado, Ds_Cargar_combos.Tables["Dt_Casos_Especiales_Financiamiento"]);
            Llenar_Combo_ID(Cmb_Solicitante, Ds_Cargar_combos.Tables["Dt_Casos_Especiales_Solicitante"]);

            Cmb_Tipos_Propietario.Items.Add(new ListItem("PROPIETARIO", "PROPIETARIO"));
            Cmb_Tipos_Propietario.Items.Add(new ListItem("POSEEDOR", "POSEEDOR"));

            Cmb_Domicilio_Foraneo.Items.Add(new ListItem("SI", "SI"));
            Cmb_Domicilio_Foraneo.Items.Add(new ListItem("NO", "NO"));

            Cmb_Estatus.Items.Add(new ListItem("PENDIENTE", "PENDIENTE"));
            Cmb_Estatus.Items.Add(new ListItem("VIGENTE", "VIGENTE"));
            Cmb_Estatus.Items.Add(new ListItem("INACTIVA", "INACTIVA"));
            Cmb_Estatus.Items.Add(new ListItem("ACTIVA", "ACTIVA"));
            Cmb_Estatus.Items.Add(new ListItem("CANCELADA", "CANCELADA"));
            Cmb_Estatus.Items.Add(new ListItem("SUSPENDIDA", "SUSPENDIDA"));
            Cmb_Estatus.Items.Add(new ListItem("BLOQUEADA", "BLOQUEADA"));

            for (int anio = Const_Anio_Corriente + 1; anio >= 1980; anio--)
            {
                Cmb_Efectos.Items.Add(new ListItem(anio.ToString(), anio.ToString()));
            }
            for (int anio = (Const_Anio_Corriente - 1); anio >= 1980; anio--)
            {
                Cmb_P_R_Anio_Inicial.Items.Add(new ListItem(anio.ToString(), anio.ToString()));
                Cmb_P_R_Anio_Final.Items.Add(new ListItem(anio.ToString(), anio.ToString()));
            }
            for (int i = 1; i <= 6; i++)
            {
                Cmb_Efectos_Numero.Items.Add(new ListItem(i.ToString(), i.ToString()));
                Cmb_P_R_Bimestre_Final.Items.Add(new ListItem(i.ToString(), i.ToString()));
                Cmb_P_R_Bimestre_Inicial.Items.Add(new ListItem(i.ToString(), i.ToString()));
                Cmb_P_C_Bimestre_Final.Items.Add(new ListItem(i.ToString(), i.ToString()));
                Cmb_P_C_Bimestre_Inicial.Items.Add(new ListItem(i.ToString(), i.ToString()));
            }

            Cmb_P_C_Anio.Items.Add(new ListItem(Const_Anio_Corriente.ToString(), Const_Anio_Corriente.ToString()));
            Cmb_P_C_Bimestre_Final.SelectedIndex = 5;
            Cmb_P_R_Bimestre_Final.SelectedIndex = 5;

        }
        catch (Exception Ex)
        {
            Mensaje_Error("No se pudo Cargar los datos de los Catalogos");
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
        Boolean Estado = false;
        switch (P_Estado)
        {
            case 0: //Estado inicial                
                Div_Busqueda_Cuenta.Style.Value = "display:none;";
                Div_Contenido.Style.Value = "display:inline;";
                Pnl_Propietario.Disabled = true;
                Pnl_Detalles_Cuota_Fija.Style.Value = "display:none;";
                Barra_Generales.InnerText = "Generales";
                Btn_Imprimir.Visible = false;
                Btn_Busqueda_Cuota_Minima.Visible = false;
                Cmb_Tipos_Movimiento.Enabled = false;
                Txt_Cuenta_Predial.Enabled = false;
                Txt_Co_Propietario.Enabled = false;
                Btn_Mostrar_Busqueda_Cuentas.Visible = false;
                Estado = false;
                Cmb_Tipos_Movimiento.Focus();
                Btn_Buscar.AlternateText = "Buscar";
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Salir.AlternateText = "Busqueda";
                Btn_Imprimir.AlternateText = "Imprimir";
                Btn_Buscar.ToolTip = "Buscar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Busqueda";
                Btn_Imprimir.ToolTip = "Imprimir";
                Btn_Buscar.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;
                Btn_Imprimir.Visible = false;
                //Establecer el estilo de la fuente de los controles
                Txt_Cuenta_Predial.Font.Bold = false;
                Txt_Cta_Origen.Font.Bold = false;
                Cmb_Tipos_Predio.Font.Bold = false;
                Cmb_Usos_Predio.Font.Bold = false;
                Cmb_Estados_Predio.Font.Bold = false;
                Cmb_Estatus.Font.Bold = false;
                Txt_Costo_M2.Font.Bold = false;
                Txt_Superficie_Construida.Font.Bold = false;
                Txt_Superficie_Total.Font.Bold = false;
                Txt_Colonia_Cuenta.Font.Bold = false;
                Txt_Calle_Cuenta.Font.Bold = false;
                Txt_No_Exterior.Font.Bold = false;
                Txt_No_Interior.Font.Bold = false;
                Txt_Catastral.Font.Bold = false;
                Cmb_Efectos_Numero.Font.Bold = false;
                Cmb_Efectos.Font.Bold = false;
                Txt_Valor_Fiscal.Font.Bold = false;
                Txt_Valor_Fiscal.Font.Bold = false;
                Txt_Cuota_Anual.Font.Bold = false;
                Txt_Cuota_Bimestral.Font.Bold = false;
                Txt_Porcentaje_Excencion.Font.Bold = false;
                Txt_Fecha_Avaluo.Font.Bold = false;
                Txt_Fecha_Inicial.Font.Bold = false;
                Txt_Dif_Construccion.Font.Bold = false;
                Chk_Cuota_Fija.Font.Bold = false;
                Chk_Beneficio_Completo.Font.Bold = false;
                Txt_Tasa_Descripcion.Font.Bold = false;
                Txt_Tasa_Porcentaje.Font.Bold = false;
                Txt_Tasa_Excedente_Valor.Font.Bold = false;
                Txt_Tasa_Exedente_Construccion.Font.Bold = false;
                Cmb_Domicilio_Foraneo.Font.Bold = false;
                Txt_Colonia_Propietario.Font.Bold = false;
                Txt_Calle_Propietario.Font.Bold = false;
                Txt_Numero_Exterior_Propietario.Font.Bold = false;
                Txt_Numero_Interior_Propietario.Font.Bold = false;
                Txt_CP.Font.Bold = false;
                Txt_Estado.Font.Bold = false;
                Txt_Ciudad.Font.Bold = false;
                Txt_Calle_Propietario.Font.Bold = false;
                Txt_Colonia_Propietario.Font.Bold = false;
                //Codigo encargado de limnpiar el combo de uso de predio
                //elimina los que estan dados de baja al limpiar 
                //los demas controles
                List<ListItem> Lista = new List<ListItem>();
                foreach (ListItem item in Cmb_Usos_Predio.Items)
                {
                    Lista.Add(item);
                }
                foreach (ListItem item in Lista)
                {
                    if (item.Text.Contains("BAJA"))
                    {
                        Cmb_Usos_Predio.Items.Remove(item);
                    }
                }
                break;

            case 2: //Nuevo
                Pnl_Propietario.Disabled = false;
                Estado = true;
                Btn_Salir.Visible = true;
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Modificar.Visible = false;
                Btn_Mostrar_Busqueda_Cuentas.Visible = true;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Cmb_Tipos_Movimiento.Focus();
                break;

            case 3: //Modificar
                Div_Busqueda_Cuenta.Style.Value = "display:none;";
                Div_Contenido.Style.Value = "display:inline;";
                Pnl_Propietario.Disabled = false;
                Pnl_Propietario.Style.Add("disabled", "false");
                Estado = true;
                Btn_Salir.Enabled = true;
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Modificar.Visible = true;
                Btn_Nuevo.Visible = false;
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Cmb_Tipos_Movimiento.Focus();
                break;

            case 4: //Busqueda de Cuentas
                Btn_Salir.AlternateText = "Inicio";
                Btn_Salir.ToolTip = "Inicio";
                Div_Busqueda_Cuenta.Style.Value = "display:inline;";
                Div_Contenido.Style.Value = "display:none;";
                break;
        }
        Txt_Cuenta_Predial.Enabled = false;
        Btn_Establecer_Cuenta_Predial.Visible = false;
        Txt_Cta_Origen.Enabled = Estado;
        Txt_Superficie_Construida.Enabled = Estado;
        Txt_Superficie_Total.Enabled = Estado;
        Txt_Calle_Cuenta.Enabled = false;
        Txt_Colonia_Cuenta.Enabled = false;
        Txt_No_Exterior.Enabled = Estado;
        Txt_No_Interior.Enabled = Estado;
        Txt_Catastral.Enabled = Estado;
        //Propietario
        Txt_Nombre_Propietario.Enabled = false;
        Txt_Rfc_Propietario.Enabled = false;
        Txt_Ciudad.Style.Add("disabled", "true");
        Txt_Estado.Style.Add("disabled", "true");
        Txt_Calle_Propietario.Style.Add("disabled", "true");
        Txt_Colonia_Propietario.Style.Add("disabled", "true");
        Txt_Numero_Exterior_Propietario.Enabled = Estado;
        Txt_Numero_Interior_Propietario.Enabled = Estado;
        Txt_CP.Enabled = Estado;
        Txt_Valor_Fiscal.Enabled = Estado;
        Txt_Costo_M2.Enabled = Estado;
        Txt_Tasa_Descripcion.Enabled = false;
        Txt_Tasa_Porcentaje.Enabled = false;
        Txt_Periodo_Corriente.Enabled = false;
        Txt_Cuota_Anual.Enabled = false;
        Txt_Cuota_Bimestral.Enabled = false;
        Txt_Dif_Construccion.Enabled = Estado;
        Txt_Porcentaje_Excencion.Enabled = Estado;
        Txt_Fecha_Avaluo.Enabled = Estado;
        Txt_Fecha_Inicial.Enabled = Estado;
        Txt_Plazo.Enabled = Estado;
        Txt_Exedente_Construccion.Enabled = false;
        Txt_Tasa_Exedente_Construccion.Enabled = false;
        Txt_Excedente_Construccion_Total.Enabled = false;
        Txt_Excedente_Valor.Enabled = false;
        Txt_Tasa_Excedente_Valor.Enabled = false;
        Txt_Tasa_Valor_Total.Enabled = false;
        Txt_Total_Cuota_Fija.Enabled = false;
        Txt_Fundamento.Enabled = false;
        Txt_Co_Propietario.Enabled = false;
        Txt_Observaciones_Cuenta.Enabled = Estado;
        Txt_Comentarios.Enabled = Estado;
        Grid_Diferencias.Enabled = Estado;
        Grid_Copropietarios.Enabled = Estado;

        //Combos
        //Generales
        Cmb_Tipos_Predio.Enabled = Estado;
        Cmb_Usos_Predio.Enabled = Estado;
        Cmb_Estados_Predio.Enabled = Estado;
        Cmb_Estatus.Enabled = false;
        Cmb_Efectos.Enabled = Estado;
        Cmb_Efectos_Numero.Enabled = Estado;
        //propietarios        
        Cmb_Tipos_Propietario.Enabled = Estado;
        Cmb_Domicilio_Foraneo.Style.Add("disabled", "true");
        //Impuestos
        Cmb_Solicitante.Enabled = Estado;
        Cmb_Financiado.Enabled = Estado;
        Lbl_Defuncion.Visible = false;
        Txt_Fecha_Def.Visible = false;
        Btn_CE_Fecha_Defuncion.Visible = false;
        //Analisis de Regazos        
        Cmb_P_C_Bimestre_Inicial.Enabled = Estado;
        Cmb_P_C_Bimestre_Final.Enabled = Estado;
        Cmb_P_C_Anio.Enabled = Estado;
        Cmb_P_R_Bimestre_Inicial.Enabled = Estado;
        Cmb_P_R_Anio_Inicial.Enabled = Estado;
        Cmb_P_R_Bimestre_Final.Enabled = Estado;
        Cmb_P_R_Anio_Final.Enabled = Estado;
        //Botones
        Btn_Agregar_P_Regazo.Enabled = Estado;
        Btn_Agregar_P_Corriente.Enabled = Estado;
        Btn_Mostrar_Tasas_Diferencias.Enabled = Estado;
        Btn_Vista_Adeudos.Enabled = Estado;
        Btn_Busqueda_Propietarios.Enabled = Estado;
        Chk_Mismo_Domicilio.Enabled = Estado;
        Btn_Calcular_Cuota.Enabled = Estado;
        Btn_Calcular_Cuota_Fija.Enabled = Estado;
        Btn_Busqueda_Tasas.Enabled = Estado;
        Btn_CE_Fecha_Avaluo.Enabled = Estado;
        Btn_CE_Txt_Fecha_Inicial.Enabled = Estado;
        Btn_Resumen_Cuenta.Enabled = true;
        Btn_Seleccionar_Colonia.Enabled = Estado;
        Btn_Seleccionar_Calle.Enabled = Estado;
        Chk_Cuota_Fija.Enabled = Estado;
        Chk_Beneficio_Completo.Enabled = Estado;
        //Co-Propietarios
        Btn_Busqueda_Co_Propietarios.Enabled = Estado;
        Btn_Agregar_Co_Propietarios.Enabled = Estado;
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Agregar_Columnas
    ///DESCRIPCION : Define las columnas de los Datatables de copropietarios y diferencias
    ///              que guardaran la informacion de la orden de variacion
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 15-Agosto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Agregar_Columnas()
    {
        DataTable Dt_Agregar_Co_Propietarios = new DataTable();
        DataTable Dt_Agregar_Diferencias = new DataTable();
        DataTable Dt_Contribuyentes = new DataTable();
        Dt_Agregar_Co_Propietarios.Columns.Add("CONTRIBUYENTE_ID");
        Dt_Agregar_Co_Propietarios.Columns.Add("NOMBRE_CONTRIBUYENTE");
        Dt_Agregar_Co_Propietarios.Columns.Add("RFC");
        Dt_Agregar_Diferencias.Columns.Add("NO_DIFERENCIA");
        Dt_Agregar_Diferencias.Columns.Add("CUENTA_PREDIAL_ID");
        Dt_Agregar_Diferencias.Columns.Add("IMPORTE");
        Dt_Agregar_Diferencias.Columns.Add("PERIODO");
        Dt_Agregar_Diferencias.Columns.Add("TASA_ID");
        Dt_Agregar_Diferencias.Columns.Add("TIPO");
        Dt_Agregar_Diferencias.Columns.Add("VALOR_FISCAL");
        Dt_Agregar_Diferencias.Columns.Add("TASA");
        Dt_Agregar_Diferencias.Columns.Add("CUOTA_BIMESTRAL");
        Dt_Agregar_Diferencias.Columns.Add("TIPO_PERIODO");
        Dt_Contribuyentes.Columns.Add("CONTRIBUYENTE_ID");
        Dt_Contribuyentes.Columns.Add("ESTATUS");
        Dt_Contribuyentes.Columns.Add("TIPO");
        Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Session["Dt_Agregar_Co_Propietarios"] = Dt_Agregar_Co_Propietarios;
        Session["Dt_Agregar_Diferencias"] = Dt_Agregar_Diferencias;
        Session["Dt_Contribuyentes"] = Dt_Contribuyentes;
        Session["M_Orden_Negocio"] = M_Orden_Negocio;
    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpiar_Session_Agregar_Diferencias
    ///DESCRIPCION : Limpia las variables de session del analisis de rezago
    ///              utilizadas en el formulario
    ///PARAMETROS  : 
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 05-Agsoto-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Limpiar_Session_Agregar_Diferencias()
    {
        DataTable Dt_Agregar_Diferencias = new DataTable();
        Session.Remove("Dt_Agregar_Diferencias");

        Dt_Agregar_Diferencias.Columns.Add("NO_DIFERENCIA");
        Dt_Agregar_Diferencias.Columns.Add("CUENTA_PREDIAL_ID");
        Dt_Agregar_Diferencias.Columns.Add("IMPORTE");
        Dt_Agregar_Diferencias.Columns.Add("PERIODO");
        Dt_Agregar_Diferencias.Columns.Add("TASA_ID");
        Dt_Agregar_Diferencias.Columns.Add("TIPO");
        Dt_Agregar_Diferencias.Columns.Add("VALOR_FISCAL");
        Dt_Agregar_Diferencias.Columns.Add("TASA");
        Dt_Agregar_Diferencias.Columns.Add("CUOTA_BIMESTRAL");
        Dt_Agregar_Diferencias.Columns.Add("TIPO_PERIODO");
        Session["Dt_Agregar_Diferencias"] = Dt_Agregar_Diferencias;
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
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Orden;
        try
        {
            Limpiar_Formulario();
            Consulta_Combos();
            Cargar_Combos();
            Estado_Botones(Const_Estado_Inicial);
            Estado_Botones(Const_Estado_Busqueda_Cuenta);

            if (Session["Contrarecibo_Traslado"] != null)//Si se trae un contrarecibo desde traslado
            {
                Hdn_Contrarecibo.Value = Session["Contrarecibo_Traslado"].ToString();//Se carga el ID del contrarecibo en Hdn_Contrarecibo
                Session_Remove();//y se borran las sesiones de OV anteriores
                Estado_Botones(Const_Estado_Inicial);
                Hdn_Cuenta_ID.Value = null;
                if (Session["Estatus_Traslado"].ToString() == "GENERADO")
                {
                    Buscar_Contrarecibo();//Se consulta el contrarecibo si tiene estatus de Generado
                    Consulta_Ultimo_Mov();//Se Asigna el Ultimo movimiento a la Etiqueta para la visualizacion por el usuario
                    Consulta_Couta_Minima();//Se Asigna el valor de la cuota minima correspondiente al año y se muestra en la caja de texto
                    Estado_Botones(Const_Estado_Nuevo);
                    //Variables de session que se mandan a la vista previa de adeudos
                    Session.Remove("Cuota_Fija_Anterior");
                    Session["Cuota_Fija_Anterior"] = "";
                    Session["Cuenta_Predial_ID_Adeudos"] = Hdn_Cuenta_ID.Value;
                    Session["Orden_Variacion_ID_Adeudos"] = "";
                    Session["Anio_Orden_Adeudos"] = Hdn_Contrarecibo.Value.Substring(11, 4);

                }
                else
                {//Busqueda de OV directas

                    //Dar formato a cadena de Busqueda
                    Int32 Contrarecibo = 0;
                    Int32 Anio_Contrarecibo = 0;
                    Int32.TryParse(Hdn_Contrarecibo.Value.Substring(0, 10), out Contrarecibo);
                    if (Hdn_Contrarecibo.Value.Length > 10)
                        Int32.TryParse(Hdn_Contrarecibo.Value.Substring(11, 4), out Anio_Contrarecibo);
                    else
                        Anio_Contrarecibo = DateTime.Today.Year;
                    Orden_Negocio.P_Filtros_Dinamicos = Ope_Pre_Orden_Variacion.Campo_No_Contrarecibo + " = '" + String.Format("{0:0000000000}", Contrarecibo) + "' AND ";
                    Orden_Negocio.P_Filtros_Dinamicos += " " + Ope_Pre_Orden_Variacion.Campo_Anio + " = '" + Anio_Contrarecibo + "'";
                    Orden = Orden_Negocio.Consultar_Ordenes_Variacion();
                    if (Orden.Rows.Count > 0)
                    {
                        Txt_Buscar.Text = Orden_Negocio.Consultar_Ordenes_Variacion().Rows[0][0].ToString();
                        ImageClickEventArgs Img_Evnt = null;
                        Btn_Buscar_Click(null, Img_Evnt);
                        Consulta_Ultimo_Mov();//Se Asigna el Ultimo movimiento a la Etiqueta para la visualizacion por el usuario
                        if (Session["Estatus_Traslado"].ToString() == "RECHAZADO" || Session["Estatus_Traslado"].ToString() == "RECHAZADA")
                        {
                            Estado_Botones(Const_Estado_Modificar);//Si se ecuentra una OV rechazada se habilitan los controles para modificar
                        }
                        else
                        {
                            Estado_Botones(Const_Estado_Inicial);
                            Btn_Imprimir.Visible = true;
                        }
                        Txt_Comentarios.Text = "";

                        Session["Cuenta_Predial_ID_Adeudos"] = Hdn_Cuenta_ID.Value;
                        Session["Orden_Variacion_ID_Adeudos"] = Txt_Buscar.Text.Trim();
                        Session["Anio_Orden_Adeudos"] = Hdn_Orden_Variacion_Anio.Value;

                    }
                    else
                    {
                        Txt_Buscar.Text = "";
                    }

                    //else Mensaje_Error("El estatus de la Orden Es " + Session["Estatus_Traslado"].ToString());
                }
                //if (Validar_Historial_Ordenes_Variacion())
                //{
                //    Txt_Cuenta_Predial.Text = "";
                //    Session["M_Cuenta_ID"] = "";
                //    Hdn_Cuenta_ID_Temp.Value = "";
                //    Session["Cuenta_Predial_ID_Adeudos"] = "";
                //    Hdn_Cuenta_ID.Value = "";

                //    Img_Error.Visible = false;
                //    Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación pendiente Por Validar</br>";
                //}
                Validar_Acceso();//Codigo para validar el estatus de las cuentas
            }
            if (Session["Opcion_Tipo_Orden"].ToString().Contains("Predial"))
            {
                Txt_Busqueda_Contrarecibo.Visible = false;
                Lbl_Busqueda_Contrarecibo.Visible = false;
            }
            else
            {
                Txt_Busqueda_Contrarecibo.Visible = true;
                Lbl_Busqueda_Contrarecibo.Visible = true;
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error("Error: [No se pudieron Inicializar los controles]");
        }
    }
    #endregion
    //Region de Metodos De consultas
    #region Metodos ABC [Consulta_Combos,Consulta_Valor_Excedente,Busqueda_Contrarecibo]

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Contrarecibo
    ///DESCRIPCIÓN: Realiza la consulta del folio de contrarecibo
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/20/2011 11:37:37 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Buscar_Contrarecibo()
    {
        DataTable Dt_Contrarecibo;
        Cls_Ope_Pre_Orden_Variacion_Negocio Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            if (!String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
            {
                //Dar formato a cadena de busqueda
                Int32 Contrarecibo = 0;
                Int32 Anio_Contrarecibo = 0;
                Int32.TryParse(Hdn_Contrarecibo.Value.Substring(0, 10), out Contrarecibo);
                //Se separa el año del numero de contrarecibo
                if (Hdn_Contrarecibo.Value.Length > 10)
                    Int32.TryParse(Hdn_Contrarecibo.Value.Substring(11, 4), out Anio_Contrarecibo);
                else
                    Anio_Contrarecibo = DateTime.Today.Year;
                Negocio.P_Contrarecibo = String.Format("{0:0000000000}", Contrarecibo);
                Negocio.P_Año = Anio_Contrarecibo;
                //Se Busca el contrarecibo y la cuenta predial relacionada al contrarecibo
                Dt_Contrarecibo = Negocio.Consulta_General_Contrarecibo();
                Session["Dt_Contrarecibo"] = Dt_Contrarecibo;
                //Si hay resultado
                if (Dt_Contrarecibo.Rows.Count > 0)
                {
                    Barra_Generales.InnerText = "Generales  Contrarecibo No. " + Hdn_Contrarecibo.Value;
                    if (!String.IsNullOrEmpty(Dt_Contrarecibo.Rows[0]["CUENTA_ID"].ToString()))
                    {
                        Hdn_Cuenta_ID.Value = Dt_Contrarecibo.Rows[0]["CUENTA_ID"].ToString();//Se carga ID de la cuenta en Hdn_Cuenta_ID
                        Cmb_Tipos_Movimiento.Enabled = true;
                        Cargar_Datos();
                        Cargar_Ventana_Emergente_Adeudo_Diferencias();
                    }
                    else
                    {
                        //Codigo Obsoleto devido a creacion de cuentas desde Traslado
                        Cmb_Tipos_Movimiento.Enabled = false;
                        Negocio.Consulta_Id_Movimiento("APERTURA");
                        if (!string.IsNullOrEmpty(Negocio.P_Generar_Orden_Movimiento_ID))//Si es vacio el ID de la cuenta se establece el movimiento apertua de cuenta
                        {
                            Seleccionar_Tipo_Movimiento(Negocio.P_Generar_Orden_Movimiento_ID);
                            Cmb_Tipos_Movimiento.Enabled = false;

                        }
                    }
                }
                else
                {
                    Limpiar_Formulario();
                    Session_Remove();
                    Estado_Botones(Const_Estado_Inicial);
                    Mensaje_Error("No se obtuvieron resultados para el Contrarecibo");
                    Barra_Generales.InnerText = "Generales";
                }
            }
            else
                Session["Dt_Contrarecibo"] = new DataTable();
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Error: [No se lograron cargar los datos del contrarecibo]");
            Barra_Generales.InnerText = "Generales";
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Seleccionar_Tipo_Movimiento
    ///DESCRIPCIÓN: Selecciona un valor del combo de movimientos y si esta en estatus de baja lo consulta y lo agrega al combo
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 15/Diciembre/2011 12:21:21 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Seleccionar_Tipo_Movimiento(String Identificador)
    {
        String Valor;
        String Texto;
        DataTable Dt_Movimientos;
        DataSet Ds_Cargar_combos = null;
        if (Session["Ds_Consulta_Combos"] != null)
        {
            Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
        }
        else
        {
            Consulta_Combos();
            Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
        }
        try
        {

            Cmb_Tipos_Movimiento.SelectedValue = Identificador;
            Hdn_Cargar_Modulos.Value = Ds_Cargar_combos.Tables["Dt_Tipos_Movimiento"].Rows[Cmb_Tipos_Movimiento.SelectedIndex - 1][Cat_Pre_Movimientos.Campo_Cargar_Modulos].ToString();
        }
        catch
        {
            Mensaje_Error();
            Mensaje_Error("*Revisar tipo de Movimiento");
            Cls_Cat_Pre_Movimientos_Negocio Movimiento = new Cls_Cat_Pre_Movimientos_Negocio();
            Movimiento.P_Filtros_Dinamicos = Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + Identificador + "'";
            Dt_Movimientos = Movimiento.Consultar_Movimientos();

            if (Dt_Movimientos.Rows.Count > 0)
            {
                Texto = Dt_Movimientos.Rows[0]["DESCRIPCION"].ToString();
                Valor = Dt_Movimientos.Rows[0]["MOVIMIENTO_ID"].ToString();
                Hdn_Cargar_Modulos.Value = Dt_Movimientos.Rows[0][Cat_Pre_Movimientos.Campo_Cargar_Modulos].ToString();
                ListItem Nuevo_Item = new ListItem(Texto + " (B)", Valor.ToString());
                Cmb_Tipos_Movimiento.Items.Add(Nuevo_Item);
                Cmb_Tipos_Movimiento.SelectedValue = Identificador;
                //Hdn_Cargar_Modulos.Value = Ds_Cargar_combos.Tables["Dt_Tipos_Movimiento"].Rows[Cmb_Tipos_Movimiento.SelectedIndex - 1][Cat_Pre_Movimientos.Campo_Cargar_Modulos].ToString();
            }
            else
            {
                Mensaje_Error("Ocurrio un error al cargar el tipo de movimiento");
                Cmb_Tipos_Movimiento.SelectedIndex = 0;
            }
        }

    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Excedente_Valor
    ///DESCRIPCIÓN: consulta los datos del excedente de valor
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Agosto/2011 11:50:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************       
    private void Consulta_Excedente_Valor()
    {
        Double Excedente_Valor;
        try
        {
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Adeudo_Predial_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            Adeudo_Predial_Negocio.p_Salario_Minimo = Adeudo_Predial_Negocio.Obtener_Salario_Minimo(DateTime.Now.Year);//Obtiene el tope de salarios minimos para calcular exedente de valor
            Adeudo_Predial_Negocio.Obtener_Tope_Salarios_Minimos();
            Excedente_Valor = (Convert.ToDouble(Txt_Valor_Fiscal.Text.Trim()) - Convert.ToDouble(Adeudo_Predial_Negocio.p_Tope_Salarios_Minimos.ToString()));
            if (Excedente_Valor < 0)
                Hdn_Excedente_Valor.Value = "0";
            else
                Hdn_Excedente_Valor.Value = Excedente_Valor.ToString();

            Txt_Excedente_Valor.Text = Convert.ToDouble(Hdn_Excedente_Valor.Value).ToString("#,###,#0.00");
        }
        catch (Exception Ex)
        {

        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Excedente_Valor
    ///DESCRIPCIÓN: consulta los datos del excedente de valor
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Agosto/2011 11:50:25 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************       
    private Double Consulta_Excedente_Valor(double Valor_Fiscal, int Anio)
    {
        Double Excedente_Valor = 0.00;
        try
        {
            Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Adeudo_Predial_Negocio = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
            Adeudo_Predial_Negocio.p_Salario_Minimo = Adeudo_Predial_Negocio.Obtener_Salario_Minimo(Anio);//Obtiene el tope de salarios minimos para calcular exedente de valor
            Adeudo_Predial_Negocio.Obtener_Tope_Salarios_Minimos();
            Excedente_Valor = (Convert.ToDouble(Valor_Fiscal) - Convert.ToDouble(Adeudo_Predial_Negocio.p_Tope_Salarios_Minimos.ToString()));
            if (Excedente_Valor < 0)
                Excedente_Valor = 0;
        }
        catch (Exception Ex)
        {

        }
        return Convert.ToDouble(Excedente_Valor.ToString("#,###,#0.00"));
    }

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
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Ultimo_Mov
    ///DESCRIPCIÓN: consultar clave del ultimo movimiento realizado de la cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Sep/2011 07:41:34 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Ultimo_Mov()
    {
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Negocio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Movimiento;
        try
        {
            Resumen_Negocio.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Dt_Movimiento = Resumen_Negocio.Consultar_Ultimo_Movimiento();
            if (Dt_Movimiento.Rows.Count > 0)
                Lbl_Ultimo_Movimiento.Text = Dt_Movimiento.Rows[0][0].ToString();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Couta_Minima
    ///DESCRIPCIÓN: Obtener datos de cuota minima del ano actual
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Sep/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Couta_Minima()
    {
        try
        {
            Cargar_Couta_Minima();//Obtener datos de cuota minima del ano actual
            Calcular_Cuota(false);//Calcula cuota anual y bimestral
            Calcular_Excedentes();//Calcula el Total de la cuota fija con los impuestos por excedentes
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Couta_Minima
    ///DESCRIPCIÓN: Obtener datos de cuota minima del ano actual
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Sep/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Couta_Minima()
    {
        DataTable Dt_Cuota_Minima;
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Double Dbl_Cuota_Minima = 0;
        Double Dbl_Cuota_Minima_Aplicada = 0;
        Boolean Resultado = false;
        int Periodo_Corriente = 1;
        try
        {
            Cuotas_Minimas.P_Anio = Const_Anio_Corriente.ToString();
            Dt_Cuota_Minima = Cuotas_Minimas.Consultar_Cuotas_Minimas_Ventana_Emergente();
            if (Dt_Cuota_Minima.Rows.Count > 0)
            {
                Hdn_Cuota_Minima.Value = Dt_Cuota_Minima.Rows[0]["Cuota_ID"].ToString();
                if (Double.TryParse(Dt_Cuota_Minima.Rows[0]["Cuota"].ToString(), out Dbl_Cuota_Minima))
                {
                    Periodo_Corriente = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                    if (Periodo_Corriente == 6)
                        Periodo_Corriente = 0;
                    Dbl_Cuota_Minima_Aplicada = Convert.ToDouble((Dbl_Cuota_Minima / 6).ToString("#,###,##0.00")) * (6 - Periodo_Corriente);//se divide entre el numero de periodo en curso para sacar el proporcional de la cuota
                    Txt_Cuota_Minima.Text = Dbl_Cuota_Minima.ToString("#,###,##0.00");
                    Txt_Cuota_Minima_Aplicar.Text = Dbl_Cuota_Minima_Aplicada.ToString("#,###,##0.00");
                    Resultado = true;
                }
                Quitar_Porcentaje_Anualidad();//Metodo para Afectar la anualidad a pagar por asociasions civiles y escuelas ya que solo se les cobra una parte de la anualidad
            }
            if (!Resultado)
            {
                Txt_Cuota_Minima_Aplicar.Text = "0";
                Txt_Cuota_Minima_Aplicar.Text = "0";
                Hdn_Cuota_Minima.Value = "0";
                //Mensaje_Error("No se pudo cargar la cuota mínima Revise el Catálogo de Cuotas Minimas");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Cuentas
    ///DESCRIPCIÓN: Busqueda de los datos de la cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Sep/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Busqueda_Cuentas()
    {
        DataSet Ds_Cuenta;
        Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = (Cls_Ope_Pre_Orden_Variacion_Negocio)Session["M_Orden_Negocio"];
        try
        {
            M_Orden_Negocio.P_Cuenta_Predial = Get_Cuenta_ID();//Recupera el ID de la cuenta
            M_Orden_Negocio.P_Contrarecibo = Get_Contra_ID();//Recupera el ID del contrarecibo
            if (String.IsNullOrEmpty(Hdn_Cuenta_ID_Temp.Value))
            {
                Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta_Generales();
                Session["Cuenta_Predial_ID"] = Hdn_Cuenta_ID.Value;
            }
            else
            {
                Hdn_Cuenta_ID.Value = Hdn_Cuenta_ID_Temp.Value;
                M_Orden_Negocio.P_Cuenta_Predial = null;
                M_Orden_Negocio.P_Cuenta_Predial_ID = Get_Cuenta_ID();
                Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta_Sin_Contrarecibo();
                Session["Cuenta_Predial_ID"] = Hdn_Cuenta_ID_Temp.Value;
            }
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Session["Ds_Cuenta_Datos_Orden"] = Ds_Cuenta;
                Session["Estatus_Cuenta"] = Ds_Cuenta.Tables[0].Rows[0]["ESTATUS"].ToString();
                Session["Tipo_Suspencion"] = Ds_Cuenta.Tables[0].Rows[0]["TIPO_SUSPENCION"].ToString();
            }
            else
            {
                Estado_Botones(Const_Estado_Inicial);
                Limpiar_Formulario();
                Mensaje_Error("No se encontraron los datos necesarios para la consulta de la cuenta");
            }
            Session["M_Orden_Negocio"] = M_Orden_Negocio;
            Session["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text.Trim();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Cuentas
    ///DESCRIPCIÓN: Busqueda de los datos de la cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Sep/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Busqueda_Propietario()
    {
        DataSet Ds_Prop;
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            Orden_Negocio.P_Cuenta_Predial_ID = Get_Cuenta_ID();
            Orden_Negocio.P_Contrarecibo = Get_Contra_ID();
            Ds_Prop = Orden_Negocio.Consulta_Datos_Propietario();
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
    ///NOMBRE DE LA FUNCIÓN: Buscar_Orden
    ///DESCRIPCIÓN: Busqueda de los datos de la orden
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 18/Sep/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Buscar_Orden()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Busqueda_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Orden_Variacion;
        Int32 No_Orden = 0;
        Btn_Imprimir.Visible = false;
        String Mi_SQL;
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = (Cls_Ope_Pre_Orden_Variacion_Negocio)Session["M_Orden_Negocio"];
            if (!String.IsNullOrEmpty(Txt_Buscar.Text.Trim()))
            {
                Int32.TryParse(Txt_Buscar.Text.Trim(), out No_Orden);
                Busqueda_Orden_Negocio.P_Orden_Variacion_ID = String.Format("{0:0000000000}", No_Orden);
                //Busqueda_Orden_Negocio.P_Filtros_Dinamicos = "(" + Ope_Pre_Orden_Variacion.Campo_Estatus + " = 'RECHAZADA' OR " + Ope_Pre_Orden_Variacion.Campo_Estatus + " = 'RECHAZAD0' ) AND " + Ope_Pre_Orden_Variacion.Campo_Orden_Variacion_ID + " = '" + Busqueda_Orden_Negocio.P_Orden_Variacion_ID +"'";
                //Busqueda_Orden_Negocio.P_Cargar_Detalles_Orden = true;
                Busqueda_Orden_Negocio.P_Ordenar_Dinamico = Ope_Pre_Observaciones.Campo_Año + " DESC";
                Busqueda_Orden_Negocio.P_Incluir_Campos_Detalles = true;

                Mi_SQL = " " + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Calle_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Calle_ID_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Colonia_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Colonia_ID_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Estado_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Estado_ID_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Ciudad_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Ciudad_ID_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Domicilio_Foraneo;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Exterior;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Interior;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Exterior_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_No_Interior_Notificacion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Estado_Predio_ID;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Uso_Suelo_ID;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Cuota_Minima_ID;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Origen;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Codigo_Postal;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Superficie_Construida;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Superficie_Total;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Clave_Catastral;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Valor_Fiscal;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Efectos;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Periodo_Corriente;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Cuota_Anual;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Cuota_Fija;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo;
                Mi_SQL += ", TO_CHAR(" + Ope_Pre_Ordenes_Variacion.Campo_Fecha_Avaluo + ",'DD/Mon/yyyy') AS FECHA_AVALUO_FORMATEADA ";
                Mi_SQL += ", TO_CHAR(" + Ope_Pre_Ordenes_Variacion.Campo_Termino_Exencion + ",'DD/Mon/yyyy') AS TERMINO_EXENCION_FORMATEADA ";
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Diferencia_Construccion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Costo_M2;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Porcentaje_Exencion;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Tasa_ID;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Tasa_Predial_ID;
                Mi_SQL += ", " + Ope_Pre_Ordenes_Variacion.Campo_Tipo_Suspencion;

                Busqueda_Orden_Negocio.P_Campos_Detalles = Mi_SQL;

                Dt_Orden_Variacion = Busqueda_Orden_Negocio.Consultar_Ordenes_Variacion();
                if (Dt_Orden_Variacion.Rows.Count > 0)
                {
                    Session["FECHA_CREO"] = Dt_Orden_Variacion.Rows[0]["FECHA_ORDEN"].ToString();//Variable de session para almacenar la fecha de creacion
                    Hdn_Orden_Variacion.Value = Busqueda_Orden_Negocio.P_Orden_Variacion_ID;
                    Session["Orden_Variacion_ID_Adeudos"] = Convert.ToString(Hdn_Orden_Variacion.Value);
                    Hdn_Orden_Variacion_Anio.Value = Dt_Orden_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio].ToString();
                    Session["Anio_Orden_Adeudos"] = Hdn_Orden_Variacion_Anio.Value;
                    if (!String.IsNullOrEmpty(Dt_Orden_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Contrarecibo].ToString()))
                        Hdn_Contrarecibo.Value = Dt_Orden_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Contrarecibo].ToString() + "/" + Hdn_Orden_Variacion_Anio.Value;
                    Txt_Observaciones_Cuenta.Text = Dt_Orden_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Observaciones].ToString();
                    Cargar_Grid_Historial_Observaciones(0);//Consulta las Observaciones en el historial                    

                    Buscar_Contrarecibo();//Si existe contrarecibo carga los datos.
                    if (String.IsNullOrEmpty(Hdn_Cuenta_ID.Value))//Si no hay datos cargados Carga orden Directa
                    {
                        Hdn_Cuenta_ID_Temp.Value = Dt_Orden_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Cuenta_Predial_ID].ToString();//Se carga ID de la cuenta en Hdn_Cuenta_ID
                        Cmb_Tipos_Movimiento.Enabled = true;
                        Cargar_Datos_Directa();
                    }
                    //if (((DataTable)Session["Dt_Contrarecibo"]).Rows.Count > 0)
                    //{
                    if (!String.IsNullOrEmpty(Dt_Orden_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Movimiento_ID].ToString()))
                        Seleccionar_Tipo_Movimiento(Dt_Orden_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Movimiento_ID].ToString());
                    //if (Busqueda_Orden_Negocio.P_Generar_Orden_Dt_Detalles.Rows.Count > 0)
                    //{
                    Session["Cargar_Variaciones"] = Dt_Orden_Variacion;
                    Cargar_Variaciones();
                    //}
                    Session.Remove("Cargar_Variaciones");
                    if (Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString() != "RECHAZADA" && Dt_Orden_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString() != "RECHAZADO" && Session["Estatus_Cuenta"].ToString() != "BLOQUEADA" && Session["Estatus_Cuenta"].ToString() != "NINGUNO")
                    {
                        Estado_Botones(Const_Estado_Inicial);
                        Btn_Imprimir.Visible = true;
                    }
                    //}
                    else
                    {
                        Consulta_Ultimo_Mov();
                        if (String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
                            Estado_Botones(Const_Estado_Modificar);
                        Txt_Comentarios.Text = "";
                        Btn_Imprimir.Visible = true;
                        if (Session["Estatus_Cuenta"].ToString() == "BLOQUEADA")
                            Mensaje_Error("La cuenta " + Hdn_Cuenta_ID.Value + " Se encuentra Bloqueada");
                    }

                }
                else
                {
                    Limpiar_Formulario();
                    Session_Remove();
                    Estado_Botones(Const_Estado_Inicial);
                    Mensaje_Error("No se econtraron Datos");
                }
                Session["M_Orden_Negocio"] = M_Orden_Negocio;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    private double Consulta_Cuota_Minima_Anio(String Anio)
    {
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        DataTable Dt_Cuota_Minima;
        String Cuota_Minima = "0";
        Double Dbl_Cuota_Minima = -1.00;

        try
        {
            Cuotas_Minimas.P_Anio = Anio.Trim();
            Dt_Cuota_Minima = Cuotas_Minimas.Consultar_Cuotas_Minimas_Ventana_Emergente();
            if (Dt_Cuota_Minima.Rows.Count > 0)
            {
                Cuota_Minima = Dt_Cuota_Minima.Rows[0]["Cuota"].ToString();
                if (!Double.TryParse(Cuota_Minima, out Dbl_Cuota_Minima))
                    Dbl_Cuota_Minima = -1.00;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Dbl_Cuota_Minima;
    }

    #endregion
    //Metodos Para Cargar los Datos Consultados en Los controles
    # region Metodos Cargar Datos [Cargar_datos,Cargar_generales,Cargar_Popietarios,Cargar_Datos_Cuota_Fija]

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
            if (!String.IsNullOrEmpty(Hdn_Cuenta_ID.Value))
            {
                //Consulta los datos de la cuenta y Hace DS
                Busqueda_Cuentas();
                //Llenar controles con DS
                Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos_Orden"]).Tables["Dt_Generales"], false);
                Busqueda_Propietario();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Error[No se pudieron cargar los datos]");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Datos_Directa
    ///DESCRIPCIÓN: asignar datos de cuenta a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Datos_Directa()
    {
        try
        {
            if (!String.IsNullOrEmpty(Hdn_Cuenta_ID_Temp.Value))
            {
                //Consulta los datos de la cuenta y Hace DS
                Busqueda_Cuentas();//Se buscan los datos de la cuenta
                //Llenar controles con DS
                Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos_Orden"]).Tables["Dt_Generales"], false);//Con la DataTable con los datos de la cuenta se asignan a los controles
                Busqueda_Propietario();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Error[No se pudieron cargar los datos]");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Periodo_Corriente
    ///DESCRIPCIÓN: Se obtiene el periodo actual
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 23/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private String Obtener_Periodo_Corriente()
    {
        String P_Corriente = "";
        String Anio = "";
        double Dbl_Bimestre = 0;
        String Bimestre = "";
        Anio = DateTime.Now.Year.ToString();
        Dbl_Bimestre = DateTime.Now.Month;
        if (Dbl_Bimestre % 2 != 0)
            Dbl_Bimestre = (DateTime.Now.Month + 1) / 2;
        else
            Dbl_Bimestre = (DateTime.Now.Month) / 2;
        Bimestre = "1/" + Anio + " - " + Dbl_Bimestre.ToString() + "/" + Anio;
        return Bimestre;

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Obtener_Periodos_Bimestre
    ///DESCRIPCIÓN: Se obtienen los periodos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 23/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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
        else
        {
            if (Periodos.Trim().IndexOf(" ") >= 0)
            {
                if (Periodos.Split(' ').GetValue(0).ToString().Contains("/"))
                {
                    Periodo = Periodos.Split(' ').GetValue(0).ToString().Replace("/", "-").Trim();
                    Periodo_Corriente_Validado = true;
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
        Cls_Ope_Pre_Orden_Variacion_Negocio Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            Int32 Valor = 0;
            if (Cuota_Fija_ID.Trim().Length == 5)
            {
                if (Obtener_Dato_Consulta(Cat_Pre_Casos_Especiales.Campo_Tipo, Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales, Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Cuota_Fija_ID.Trim() + "'") == "SOLICITANTE")
                    Cmb_Solicitante.SelectedValue = Cuota_Fija_ID.Trim();
                if (Obtener_Dato_Consulta(Cat_Pre_Casos_Especiales.Campo_Tipo, Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales, Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Cuota_Fija_ID.Trim() + "'") == "FINANCIAMIENTO")
                    Cmb_Financiado.SelectedValue = Cuota_Fija_ID.Trim();
            }
            else
            {
                Int32.TryParse(Cuota_Fija_ID.Trim(), out Valor);
                Negocio.P_No_Cuota_Fija = String.Format("{0:0000000000}", Valor);
                Dt_Cuota_Detalles = Negocio.Consultar_Cuota_Fija_Detalles();
                if (Dt_Cuota_Detalles.Rows.Count > 0)
                {
                    if (Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Tipo].ToString() == "SOLICITANTE")
                        Cmb_Solicitante.SelectedValue = Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID].ToString();
                    if (Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Tipo].ToString() == "FINANCIAMIENTO")
                        Cmb_Financiado.SelectedValue = Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID].ToString();

                    Txt_Plazo.Text = Dt_Cuota_Detalles.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Plazo_Financiamiento].ToString();
                    Txt_Excedente_Construccion_Total.Text = String.Format("{0:#,###,###.00}", Dt_Cuota_Detalles.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total].ToString());
                    Txt_Tasa_Valor_Total.Text = String.Format("{0:#,###,###.00}", Dt_Cuota_Detalles.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Valor_Total].ToString());
                    Txt_Total_Cuota_Fija.Text = String.Format("{0:#,###,###.00}", Dt_Cuota_Detalles.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija].ToString());
                    Txt_Fundamento.Text = "ARTICULO " + Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Articulo].ToString() + "INCISO " + Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Inciso].ToString() + Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Observaciones].ToString();

                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Cuota_Fija: " + Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Get_Cuenta_ID
    ///DESCRIPCIÓN: Obtener Valor de Hdn_Field y darle formato
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 25/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
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
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Get_Contra_ID
    ///DESCRIPCIÓN: Obtener Valor de Hdn_Field y darle formato
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 25/Ago/2011 06:31:08 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public String Get_Contra_ID()
    {
        if (!String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
        {
            Int32 Valor = 0;
            Int32.TryParse(Hdn_Contrarecibo.Value.Substring(0, 10), out Valor);
            return String.Format("{0:0000000000}", Valor);
        }
        return null;
    }

    #endregion
    //Metodos de Operacion
    #region Metodos [Operacion Calcular_Excedentes,Calcular_Cuota]
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Resumen_Grid_Diferencias
    ///DESCRIPCIÓN: 
    ///PARAMETROS: 
    ///CREO: 
    ///FECHA_CREO: 08/06/2011 05:25:43 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Resumen_Grid_Diferencias()
    {
        String Periodo = "";
        int Desde_Bimestre_Corriente = 6;
        int Hasta_Bimestre_Corriente = 1;
        int Desde_Bimestre_Rezago = 6;
        int Hasta_Bimestre_Rezago = 1;
        int Desde_Año_Corriente = DateTime.Now.Year + 1;
        int Hasta_Año_Corriente = 0;
        int Desde_Año_Rezago = DateTime.Now.Year + 1;
        int Hasta_Año_Rezago = 0;
        Boolean Periodo_Corriente_Validado = false;
        Boolean Periodo_Rezago_Validado = false;
        int Cont_Periodos_Corriente = 0;
        int Cont_Periodos_Rezago = 0;
        DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];

        foreach (DataRow Fila_Grid in Dt_Agregar_Diferencias.Rows)
        {
            Periodo = Obtener_Periodos_Bimestre(Fila_Grid["PERIODO"].ToString().Trim(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
            if (Fila_Grid["TIPO_PERIODO"].ToString() == "CORRIENTE")
            {
                Periodo_Corriente_Validado = true;
                Periodo_Rezago_Validado = false;
            }
            else
            {
                if (Fila_Grid["TIPO_PERIODO"].ToString() == "REZAGO")
                {
                    Periodo_Corriente_Validado = false;
                    Periodo_Rezago_Validado = true;
                }
            }
            if (Periodo_Rezago_Validado)
            {
                if (Periodo.Trim() != "")
                {
                    if (Convert.ToInt32(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(0).ToString().Split('/').GetValue(1).ToString().Trim()) <= Desde_Año_Rezago)
                    {
                        if (Convert.ToInt32(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(0).ToString().Split('/').GetValue(1).ToString().Trim()) != Desde_Año_Rezago)
                        {
                            Desde_Bimestre_Rezago = 6;
                        }
                        Desde_Año_Rezago = Convert.ToInt32(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(0).ToString().Split('/').GetValue(1).ToString().Trim());
                        if (Convert.ToInt32(Periodo.Split('-').GetValue(0)) < Desde_Bimestre_Rezago)
                        {
                            Desde_Bimestre_Rezago = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                        }
                    }
                    if (Convert.ToInt32(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Split('/').GetValue(1).ToString().Trim()) >= Hasta_Año_Rezago)
                    {
                        if (Convert.ToInt32(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Split('/').GetValue(1).ToString().Trim()) != Hasta_Año_Rezago)
                        {
                            Hasta_Bimestre_Rezago = 1;
                        }
                        Hasta_Año_Rezago = Convert.ToInt32(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Split('/').GetValue(1).ToString().Trim());
                        if (Convert.ToInt32(Periodo.Split('-').GetValue(1)) > Hasta_Bimestre_Rezago)
                        {
                            Hasta_Bimestre_Rezago = Convert.ToInt32(Periodo.Split('-').GetValue(1));
                        }
                    }
                    Cont_Periodos_Rezago++;
                }
            }
            if (Periodo_Corriente_Validado)
            {
                if (Periodo.Trim() != "")
                {
                    if (Fila_Grid["PERIODO"].ToString().Split('-').Length == 2)
                    {
                        if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Length - 4)) <= Desde_Año_Corriente)
                        {
                            if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Length - 4)) != Desde_Año_Corriente)
                            {
                                Desde_Bimestre_Corriente = 6;
                            }
                            Desde_Año_Corriente = Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Length - 4));
                            if (Convert.ToInt32(Periodo.Split('-').GetValue(0)) < Desde_Bimestre_Corriente)
                            {
                                Desde_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                            }
                        }

                        if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Length - 4)) >= Hasta_Año_Corriente)
                        {
                            if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Length - 4)) != Hasta_Año_Corriente)
                            {
                                Hasta_Bimestre_Corriente = 1;
                            }
                            Hasta_Año_Corriente = Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Substring(Fila_Grid["PERIODO"].ToString().Split('-').GetValue(1).ToString().Trim().Length - 4));
                            if (Convert.ToInt32(Periodo.Split('-').GetValue(1)) > Hasta_Bimestre_Corriente)
                            {
                                Hasta_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(1));
                            }
                        }
                        Cont_Periodos_Corriente++;
                    }
                    else
                    {
                        if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Substring(Fila_Grid["PERIODO"].ToString().Length - 4)) <= Desde_Año_Corriente)
                        {
                            if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Substring(Fila_Grid["PERIODO"].ToString().Length - 4)) != Desde_Año_Corriente)
                            {
                                Desde_Bimestre_Corriente = 6;
                            }
                            Desde_Año_Corriente = Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Substring(Fila_Grid["PERIODO"].ToString().Length - 4));
                            if (Convert.ToInt32(Periodo.Split('-').GetValue(0)) < Desde_Bimestre_Corriente)
                            {
                                Desde_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                            }
                        }
                        if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Substring(Fila_Grid["PERIODO"].ToString().Length - 4)) >= Hasta_Año_Corriente)
                        {
                            if (Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Substring(Fila_Grid["PERIODO"].ToString().Length - 4)) != Hasta_Año_Corriente)
                            {
                                Hasta_Bimestre_Corriente = 1;
                            }
                            Hasta_Año_Corriente = Convert.ToInt16(Fila_Grid["PERIODO"].ToString().Substring(Fila_Grid["PERIODO"].ToString().Length - 4));
                            if (Convert.ToInt32(Periodo.Split('-').GetValue(1)) > Hasta_Bimestre_Corriente)
                            {
                                Hasta_Bimestre_Corriente = Convert.ToInt32(Periodo.Split('-').GetValue(1));
                            }
                        }
                        Cont_Periodos_Corriente++;
                    }
                }
            }
        }

        if (Cont_Periodos_Corriente > 0)
        {
            Txt_Desde_Periodo_Corriente.Text = Desde_Bimestre_Corriente.ToString();
            Txt_Hasta_Periodo_Corriente.Text = Hasta_Bimestre_Corriente.ToString();
            Txt_Hasta_Anio_Periodo_Corriente.Text = Hasta_Año_Corriente.ToString();
            Txt_Desde_Anio_Periodo_Corriente.Text = Desde_Año_Corriente.ToString();
        }
        else
        {
            Txt_Desde_Periodo_Corriente.Text = "0";
            Txt_Hasta_Periodo_Corriente.Text = "0";
            Txt_Hasta_Anio_Periodo_Corriente.Text = "0";
            Txt_Desde_Anio_Periodo_Corriente.Text = "0";
        }
        if (Cont_Periodos_Rezago > 0)
        {
            Txt_Desde_Periodo_Regazo.Text = Desde_Bimestre_Rezago.ToString();
            Txt_Hasta_Periodo_Regazo.Text = Hasta_Bimestre_Rezago.ToString();
            Lbl_P_C_Anio_Inicio.Text = Desde_Año_Rezago.ToString();
            Lbl_P_C_Anio_Final.Text = Hasta_Año_Rezago.ToString();
        }
        else
        {
            Txt_Desde_Periodo_Regazo.Text = "0";
            Txt_Hasta_Periodo_Regazo.Text = "0";
            Lbl_P_C_Anio_Inicio.Text = "0";
            Lbl_P_C_Anio_Final.Text = "0";
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Cuota
    ///DESCRIPCIÓN: de acuerdo a la tasa seleccionada calcula la cuota anual y bimestral
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/06/2011 05:25:43 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Calcular_Cuota(bool Calcular_Anualidad)
    {
        try
        {
            Double Dbl_Couta;
            Double Dbl_Valor_Fiscal;
            Double Dbl_Factor;

            if (Txt_Valor_Fiscal.Text.Trim() != "" && Txt_Tasa_Porcentaje.Text.Trim() != "")
            {
                if (Calcular_Anualidad)
                {
                    Dbl_Factor = Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim()) / 1000;
                    Dbl_Valor_Fiscal = Convert.ToDouble(Txt_Valor_Fiscal.Text.Trim());
                    Dbl_Couta = Dbl_Valor_Fiscal * Dbl_Factor;
                    //if (Txt_Porcentaje_Excencion.Text.Trim() != "")
                    //Dbl_Couta = Dbl_Couta - (Dbl_Couta * ((Convert.ToDouble(Txt_Porcentaje_Excencion.Text.Trim())) / 100));
                    Txt_Cuota_Anual.Text = Dbl_Couta.ToString("#,###,##0.00");

                    if (!String.IsNullOrEmpty(Txt_Cuota_Minima.Text.Trim()))
                    {
                        Dbl_Couta = Convert.ToDouble(Txt_Cuota_Minima.Text.Trim());
                        if (Convert.ToDouble(Txt_Cuota_Anual.Text.Trim()) < Dbl_Couta)
                            Txt_Cuota_Anual.Text = Dbl_Couta.ToString("#,###,##0.00");
                    }
                    //Txt_Cuota_Bimestral.Text = Math.Round((Dbl_Couta / 6), 2).ToString("#,###,##0.00");
                    Txt_Cuota_Bimestral.Text = (Convert.ToDouble(Txt_Cuota_Anual.Text.Trim()) / 6).ToString("#,###,##0.00");
                }
                Quitar_Porcentaje_Anualidad();
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error("Error: [Ocurrio un problema al Calcular la cuota Anual]");
        }


    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Excedentes
    ///DESCRIPCIÓN: metodo para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Calcular_Excedentes()
    {
        Double Cuota_Minima = 0;
        Double Exedente_Construccion = 0;
        Double Excedente_Valor = 0;
        Double Total_Cuota_Fija = 0;
        try
        {
            //Consulta de Excedente de valor
            Consulta_Excedente_Valor();
            if (Txt_Cuota_Minima_Aplicar.Text.Trim() != "" && Txt_Tasa_Porcentaje.Text.Trim() != "")
            {
                if (Txt_Dif_Construccion.Text.Trim() != "" && Txt_Tasa_Exedente_Construccion.Text.Trim() != "")
                    Txt_Excedente_Construccion_Total.Text = (((Convert.ToDouble(Txt_Dif_Construccion.Text.Trim()) * Convert.ToDouble(Txt_Costo_M2.Text.Trim())) * (Convert.ToDouble(Txt_Tasa_Exedente_Construccion.Text.Trim()) / 1000))).ToString("#,###,##0.00");

                if (!String.IsNullOrEmpty(Hdn_Excedente_Valor.Value))
                {
                    if (Hdn_Excedente_Valor.Value == "0")
                        Txt_Tasa_Valor_Total.Text = "0";
                    else
                        Txt_Tasa_Valor_Total.Text = (((Convert.ToDouble(Hdn_Excedente_Valor.Value)) * ((Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim())) / 1000))).ToString("#,###,##0.00");
                }

                if (!String.IsNullOrEmpty(Txt_Cuota_Minima.Text.Trim()))
                {
                    Cuota_Minima = (Convert.ToDouble(Txt_Cuota_Minima.Text.Trim()));
                    if (!Chk_Beneficio_Completo.Checked)
                        Cuota_Minima = (Convert.ToDouble(Txt_Cuota_Minima_Aplicar.Text.Trim()));
                    Exedente_Construccion = (Convert.ToDouble(Txt_Excedente_Construccion_Total.Text.Trim()));
                    Excedente_Valor = (Convert.ToDouble(Txt_Tasa_Valor_Total.Text.Trim()));
                    if (Exedente_Construccion >= Excedente_Valor)
                    {
                        Total_Cuota_Fija = (Cuota_Minima + Exedente_Construccion);
                    }
                    else if (Excedente_Valor > Exedente_Construccion)
                    {
                        Total_Cuota_Fija = (Cuota_Minima + Excedente_Valor);
                    }
                    Txt_Total_Cuota_Fija.Text = Math.Round(Total_Cuota_Fija, 2).ToString("#,###,##0.00");
                    Session["Cuota_Fija_Nueva"] = "";
                    Session.Remove("Cuota_Fija_Nueva");
                    if (Chk_Cuota_Fija.Checked)
                        Session["Cuota_Fija_Nueva"] = Txt_Total_Cuota_Fija.Text.Trim();
                }
            }
            else
                Lbl_Error_Cuota_Fija.Text = "+ Es necesario seleccionar la Tasa en la seccion de Impuestos";
        }
        catch (Exception Ex)
        {
            Lbl_Error_Cuota_Fija.Text = "Error:[ Ocurrio un problema al Calcular los Excedentes ]";
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Excedentes
    ///DESCRIPCIÓN: metodo para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Quitar_Adeudos()
    {
        DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataRow Dr_Dif;
        DataTable Dt_Adeudos_Predial;
        double Adeudo = 0;
        String NO_DIFERENCIA = "";
        String CUENTA_PREDIAL_ID = "";
        String IMPORTE = "";
        double DBL_IMPORTE = 0;
        String TIPO = "";
        String PERIODO = "";
        String TASA_ID = "";
        String VALOR_FISCAL = "";
        String TASA = "";
        String CUOTA_BIMESTRAL = "";
        String TIPO_PERIODO = "";
        int Periodo_Corriente = 1;
        try
        {
            if (!String.IsNullOrEmpty(Hdn_Cuenta_ID.Value)) //Si hay ID de cuenta activa
            {
                Adeudos_Negocio.P_Cuenta_Predial = Hdn_Cuenta_ID.Value;
                Adeudos_Negocio.P_Anio_Filtro = Const_Anio_Corriente;
                Dt_Adeudos_Predial = Resumen.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Hdn_Cuenta_ID.Value, "POR PAGAR", Const_Anio_Corriente, Const_Anio_Corriente);
                Double Dbl_Adeudo = 0;
                int Mes = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                int Anio_Actual = Const_Anio_Corriente;
                foreach (DataRow Dr_Adeudos in Dt_Adeudos_Predial.Rows)
                {
                    Dr_Dif = Dt_Agregar_Diferencias.NewRow();
                    NO_DIFERENCIA = "";
                    CUENTA_PREDIAL_ID = Hdn_Cuenta_ID.Value;
                    Periodo_Corriente = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                    if (Periodo_Corriente == 6)
                        Periodo_Corriente = 0;
                    Dbl_Adeudo = (Convert.ToDouble(Dr_Adeudos["ADEUDO_TOTAL_ANIO"]) / 6) * (6 - Periodo_Corriente);
                    if (Chk_Beneficio_Completo.Checked)
                        Dbl_Adeudo = (Convert.ToDouble(Dr_Adeudos["ADEUDO_TOTAL_ANIO"]));
                    if ((Dbl_Adeudo - Convert.ToDouble(Txt_Total_Cuota_Fija.Text.Trim())) > 0)
                    {
                        DBL_IMPORTE = (Dbl_Adeudo - Convert.ToDouble(Txt_Total_Cuota_Fija.Text.Trim()));
                        IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");

                        TIPO = "BAJA";
                    }
                    else if ((Dbl_Adeudo - Convert.ToDouble(Txt_Total_Cuota_Fija.Text.Trim())) < 0)
                    {
                        DBL_IMPORTE = (Convert.ToDouble(Txt_Total_Cuota_Fija.Text.Trim()) - Dbl_Adeudo);
                        IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");
                        TIPO = "ALTA";
                    }
                    if (Mes != 6)
                        PERIODO = (Mes + 1).ToString() + "/" + Const_Anio_Corriente.ToString() + " - 6/" + Const_Anio_Corriente.ToString();
                    else
                        PERIODO = Mes.ToString() + "/" + Const_Anio_Corriente.ToString() + " - " + (Mes).ToString() + "/" + Const_Anio_Corriente.ToString();
                    if (Chk_Beneficio_Completo.Checked)
                        PERIODO = "1/" + Const_Anio_Corriente.ToString() + " - 6/" + Const_Anio_Corriente.ToString();
                    TASA_ID = Hdn_Tasa_ID.Value;
                    VALOR_FISCAL = Txt_Valor_Fiscal.Text.Trim();
                    TASA = Txt_Tasa_Porcentaje.Text.Trim();
                    //CUOTA_BIMESTRAL = (Convert.ToDouble(IMPORTE.ToString()) / (7 - Mes)).ToString("#,###,##0.00");
                    CUOTA_BIMESTRAL = Txt_Cuota_Bimestral.Text;
                    TIPO_PERIODO = "CORRIENTE";
                    if (DateTime.Today.Year == Const_Anio_Corriente && Mes != 6)
                        Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);

                    //break;
                }
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error("Calcular Excedentes Error:[" + Ex.Message + "]");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Excedentes
    ///DESCRIPCIÓN: metodo para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Quitar_Adeudos(double Excencion)
    {
        DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataRow Dr_Dif;
        DataTable Dt_Adeudos_Predial;
        double Adeudo = 0;
        String NO_DIFERENCIA = "";
        String CUENTA_PREDIAL_ID = "";
        String IMPORTE = "";
        double DBL_IMPORTE = 0;
        String TIPO = "";
        String PERIODO = "";
        String TASA_ID = "";
        String VALOR_FISCAL = "";
        String TASA = "";
        String CUOTA_BIMESTRAL = "";
        String TIPO_PERIODO = "";
        String Periodo_Inicial = "0";
        Double Dbl_Excencion = 0.0;
        Boolean Periodos_Diferentes = false;
        Boolean Insertar = false;
        int Periodo_Corriente = 1;
        int Fin_Periodo = 0;
        int Inicio_Periodo = 0;
        try
        {
            if (!String.IsNullOrEmpty(Hdn_Cuenta_ID.Value)) //Si hay ID de cuenta activa
            {
                Dbl_Excencion = Excencion;
                Adeudos_Negocio.P_Cuenta_Predial = Hdn_Cuenta_ID.Value;
                Adeudos_Negocio.P_Anio_Filtro = Const_Anio_Corriente;
                Dt_Adeudos_Predial = Resumen.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Hdn_Cuenta_ID.Value, "POR PAGAR", 0, 0);
                Double Dbl_Adeudo = 0;
                int Mes = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                int Anio_Actual = Const_Anio_Corriente;
                Periodo_Corriente = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                if (Periodo_Corriente == 6)
                    Periodo_Corriente = 0;

                foreach (DataRow Dr_Adeudos in Dt_Adeudos_Predial.Rows)
                {
                    Periodo_Inicial = "0";
                    for (int i = 1; i < 6; i++)
                    {
                        if (Dr_Adeudos[i].ToString() != Dr_Adeudos[i + 1].ToString())
                        {
                            if (Convert.ToDouble(Dr_Adeudos[i].ToString()) != 0)
                            {
                                Periodos_Diferentes = true;
                                break;
                            }
                            else
                            {
                                Periodo_Inicial = (i + 1).ToString();
                            }
                        }
                    }
                    //Si los periodos son diferentes entonces se da el porcentaje de excencion por cada periodo de adeudo
                    if (Periodos_Diferentes)
                    {
                        for (int contador_periodos = 1; contador_periodos <= 6; contador_periodos++)
                        {
                            Dr_Dif = Dt_Agregar_Diferencias.NewRow();
                            NO_DIFERENCIA = "";
                            CUENTA_PREDIAL_ID = Hdn_Cuenta_ID.Value;
                            DBL_IMPORTE = Convert.ToDouble(Dr_Adeudos[contador_periodos]);
                            IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");
                            DBL_IMPORTE = (Convert.ToDouble(IMPORTE));
                            DBL_IMPORTE = DBL_IMPORTE * (Dbl_Excencion / 100);
                            IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");
                            DBL_IMPORTE = (Convert.ToDouble(IMPORTE));
                            TIPO = "BAJA";

                            if (Mes != 6)
                                PERIODO = (contador_periodos).ToString() + "/" + Dr_Adeudos["ANIO"].ToString() + " - " + (contador_periodos).ToString() + "/" + Dr_Adeudos["ANIO"].ToString();
                            else
                                PERIODO = contador_periodos + "/" + Dr_Adeudos["ANIO"].ToString() + " - " + contador_periodos + "/" + Dr_Adeudos["ANIO"].ToString();
                            TASA_ID = Hdn_Tasa_ID.Value;
                            VALOR_FISCAL = Txt_Valor_Fiscal.Text.Trim();
                            TASA = Txt_Tasa_Porcentaje.Text.Trim();
                            CUOTA_BIMESTRAL = (Convert.ToDouble(IMPORTE.ToString()) / (7 - Mes)).ToString("#,###,##0.00");
                            TIPO_PERIODO = "REZAGO";
                            if (Dr_Adeudos["ANIO"].ToString() == Const_Anio_Corriente.ToString())
                                TIPO_PERIODO = "CORRIENTE";
                            //if (Convert.ToDouble(IMPORTE)>0)

                            if (Convert.ToDouble(Dr_Adeudos[contador_periodos]) != 0 && Fin_Periodo != 6)
                            {
                                if (!Insertar)
                                    Inicio_Periodo = contador_periodos;
                                Fin_Periodo = contador_periodos;
                                if (Convert.ToDouble(Dr_Adeudos[contador_periodos]) == Convert.ToDouble(Dr_Adeudos[contador_periodos + 1]))
                                {
                                    Fin_Periodo = Fin_Periodo + 1;
                                    Insertar = true;
                                }
                                else
                                {
                                    PERIODO = Inicio_Periodo + "/" + Dr_Adeudos["ANIO"].ToString() + " - " + Fin_Periodo + "/" + Dr_Adeudos["ANIO"].ToString();
                                    IMPORTE = ((DBL_IMPORTE) * ((Fin_Periodo - Inicio_Periodo) + 1)).ToString("#,###,##0.00");
                                    Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                                    Insertar = false;
                                }
                            }
                            else
                            {
                                if (Insertar)
                                {
                                    PERIODO = Inicio_Periodo + "/" + Dr_Adeudos["ANIO"].ToString() + " - " + Fin_Periodo + "/" + Dr_Adeudos["ANIO"].ToString();
                                    IMPORTE = ((DBL_IMPORTE) * ((Fin_Periodo - Inicio_Periodo) + 1)).ToString("#,###,##0.00");
                                    Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                                    Insertar = false;
                                }
                            }
                            //Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                            //break;
                        }
                    }//fap
                    else
                    {
                        Dr_Dif = Dt_Agregar_Diferencias.NewRow();
                        NO_DIFERENCIA = "";
                        CUENTA_PREDIAL_ID = Hdn_Cuenta_ID.Value;
                        DBL_IMPORTE = Convert.ToDouble(Dr_Adeudos["ADEUDO_TOTAL_ANIO"]);
                        IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");
                        DBL_IMPORTE = (Convert.ToDouble(IMPORTE));
                        DBL_IMPORTE = DBL_IMPORTE * (Dbl_Excencion / 100);
                        IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");
                        DBL_IMPORTE = (Convert.ToDouble(IMPORTE));
                        TIPO = "BAJA";

                        if (Mes != 6)
                        {
                            PERIODO = "1/" + Dr_Adeudos["ANIO"].ToString() + " - 6/" + Dr_Adeudos["ANIO"].ToString();
                            if (Periodo_Inicial != "0")
                                PERIODO = Periodo_Inicial + "/" + Dr_Adeudos["ANIO"].ToString() + " - 6/" + Dr_Adeudos["ANIO"].ToString();
                        }
                        else
                        {
                            PERIODO = Mes.ToString() + "/" + Dr_Adeudos["ANIO"].ToString() + " - " + (Mes).ToString() + "/" + Dr_Adeudos["ANIO"].ToString();
                        }
                        TASA_ID = Hdn_Tasa_ID.Value;
                        VALOR_FISCAL = Txt_Valor_Fiscal.Text.Trim();
                        TASA = Txt_Tasa_Porcentaje.Text.Trim();
                        CUOTA_BIMESTRAL = (Convert.ToDouble(IMPORTE.ToString()) / (7 - Mes)).ToString("#,###,##0.00");
                        TIPO_PERIODO = "REZAGO";
                        if (Dr_Adeudos["ANIO"].ToString() == Const_Anio_Corriente.ToString())
                            TIPO_PERIODO = "CORRIENTE";
                        Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                        //break;

                    }//fap
                    Periodos_Diferentes = false;
                    Insertar = false;
                    Fin_Periodo = 0;
                    Inicio_Periodo = 0;
                }
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error("Calcular Excedentes Error:[" + Ex.Message + "]");
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Quitar_Agregar_Diferencia
    ///DESCRIPCIÓN: agregar renglon al datatable de diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/16/2011 04:33:10 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Quitar_Agregar_Diferencia(String NO_DIFERENCIA, String CUENTA_PREDIAL_ID, String IMPORTE, String TIPO, String PERIODO, String TASA_ID, String VALOR_FISCAL, String TASA, String CUOTA_BIMESTRAL, String TIPO_PERIODO)
    {
        Cargar_Ventana_Emergente_Adeudo_Diferencias();
        DataRow[] Dr_Periodos;
        DataRow Dr_Dif;
        DataTable Dt_Agregar_Diferencias = null;
        if (Session["Dt_Agregar_Diferencias"] != null)
            Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        try
        {
            if (Dt_Agregar_Diferencias != null)
            {
                Dr_Periodos = Dt_Agregar_Diferencias.Select("PERIODO = '" + PERIODO + "'");
                if (Dr_Periodos.Length <= 0)
                {
                    Dr_Dif = Dt_Agregar_Diferencias.NewRow();
                    Dr_Dif["NO_DIFERENCIA"] = NO_DIFERENCIA;
                    Dr_Dif["CUENTA_PREDIAL_ID"] = CUENTA_PREDIAL_ID;
                    Dr_Dif["IMPORTE"] = Convert.ToDouble(IMPORTE).ToString("#,###,##0.00"); ;
                    Dr_Dif["TIPO"] = TIPO;
                    Dr_Dif["PERIODO"] = PERIODO;
                    Dr_Dif["TASA_ID"] = TASA_ID;
                    Dr_Dif["VALOR_FISCAL"] = VALOR_FISCAL;
                    Dr_Dif["TASA"] = TASA;
                    Dr_Dif["CUOTA_BIMESTRAL"] = Convert.ToDouble(CUOTA_BIMESTRAL).ToString("#,###,##0.00"); ;
                    Dr_Dif["TIPO_PERIODO"] = TIPO_PERIODO;
                    Dt_Agregar_Diferencias.Rows.Add(Dr_Dif);
                    Session["Dt_Agregar_Diferencias"] = Dt_Agregar_Diferencias;
                    Cargar_Grid_Diferencias(0);
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error("Error al Quitar adeudos del periodo corriente");
        }


    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Resumen
    ///DESCRIPCIÓN: Calcular el resumen de diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    private void Calcular_Resumen()
    {
        try
        {
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            double Total_Alta_Corriente = 0;
            double Total_Baja_Corriente = 0;
            double Total_Alta_Rezago = 0;
            double Total_Baja_Rezago = 0;

            if (Session["Dt_Agregar_Diferencias"] != null && Dt_Agregar_Diferencias.Rows.Count > 0)
            {
                foreach (DataRow Dr_Diferencias in Dt_Agregar_Diferencias.Rows)
                {
                    //Agregar al resumen                        

                    if (Dr_Diferencias["TIPO_PERIODO"].ToString().Trim() == "CORRIENTE")
                    {
                        if (Dr_Diferencias["TIPO"].ToString().Trim() == "ALTA")
                        {
                            if (!String.IsNullOrEmpty(Dr_Diferencias["IMPORTE"].ToString()))
                                Total_Alta_Corriente += double.Parse(Dr_Diferencias["IMPORTE"].ToString().Replace('$', ' ').Trim());
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(Dr_Diferencias["IMPORTE"].ToString()))
                                Total_Baja_Corriente += double.Parse(Dr_Diferencias["IMPORTE"].ToString().Replace('$', ' ').Trim());
                        }
                    }
                    else
                    {
                        if (Dr_Diferencias["TIPO"].ToString().Trim() == "ALTA")
                        {
                            if (!String.IsNullOrEmpty(Dr_Diferencias["IMPORTE"].ToString()))
                                Total_Alta_Rezago += double.Parse(Dr_Diferencias["IMPORTE"].ToString().Replace('$', ' ').Trim());
                        }
                        else
                        {
                            if (!String.IsNullOrEmpty(Dr_Diferencias["IMPORTE"].ToString()))
                                Total_Baja_Rezago += double.Parse(Dr_Diferencias["IMPORTE"].ToString().Replace('$', ' ').Trim());
                        }
                    }
                }

                Txt_Alta_Periodo_Corriente.Text = Math.Round(Total_Alta_Corriente, 2).ToString("#,###,##0.00");
                Txt_Baja_Periodo_Corriente.Text = Math.Round(Total_Baja_Corriente, 2).ToString("#,###,##0.00");
                Txt_Alta_Periodo_Regazo.Text = Math.Round(Total_Alta_Rezago, 2).ToString("#,###,##0.00");
                Txt_Baja_Periodo_Regazo.Text = Math.Round(Total_Baja_Rezago, 2).ToString("#,###,##0.00");
            }
            else
            {
                Txt_Alta_Periodo_Corriente.Text = "0";
                Txt_Baja_Periodo_Corriente.Text = "0";
                Txt_Alta_Periodo_Regazo.Text = "0";
                Txt_Baja_Periodo_Regazo.Text = "0";
                Lbl_P_C_Anio_Final.Text = "0";
                Lbl_P_C_Anio_Inicio.Text = "0";
                Txt_Desde_Periodo_Corriente.Text = "0";
                Txt_Hasta_Periodo_Corriente.Text = "0";
                Txt_Hasta_Anio_Periodo_Corriente.Text = "";
                Txt_Desde_Anio_Periodo_Corriente.Text = "";
                Txt_Hasta_Periodo_Regazo.Text = "0";
                Txt_Desde_Periodo_Regazo.Text = "0";
            }
        }
        catch (Exception Ex) { Mensaje_Error(Ex.Message); }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Analisis_Rezago
    ///DESCRIPCIÓN: Calcular montos de grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 22/Sep/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Analisis_Rezago(String Metodo, int Index_Row)
    {
        try
        {
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];//Variable de session para almacenar las diferencias
            Double Importe = 0;
            Double C_Bimestral = 0;
            Double V_Fiscal = 0;
            Double Tasa = 0;
            Double Factor = 0;
            int B_Inicial;
            int B_Final;
            int Lapzo;
            string Periodo;
            string[] Bimestres;
            string[] Tipo;
            int Anio_Inicial = 0;
            string Anio_Final = "0";
            Double Validacion_Cuota_Minima = 0;
            String Calcular = "SI";
            Boolean Recalcular = true;
            int Contador = Index_Row;
            if (Session["Calcular_Grid_Diferencias"] != null)
            {
                if (Session["Calcular_Grid_Diferencias"].ToString() == "NO")
                    Calcular = "NO";
            }
            //Calcular los importes y Cuota bimestral del Analisis de Rezago
            if (Grid_Diferencias.Rows.Count > 0 && Calcular == "SI")
            {
                TextBox Text_Valor_Temporal = (TextBox)Grid_Diferencias.Rows[Contador].Cells[2].FindControl("Txt_Grid_Dif_Valor_Fiscal");
                TextBox Text_Cuota_Bim_Temp = (TextBox)Grid_Diferencias.Rows[Contador].Cells[6].FindControl("Txt_Grid_Cuota_Bimestral");
                TextBox Text_Importe = (TextBox)Grid_Diferencias.Rows[Contador].Cells[5].FindControl("Txt_Grid_Importe");
                DropDownList Cmb_Tipo_Dif = (DropDownList)Grid_Diferencias.Rows[Contador].Cells[1].FindControl("Cmb_Tipo_Diferencias");
                Tasa = Convert.ToDouble(Grid_Diferencias.Rows[Contador].Cells[3].Text);
                if (String.IsNullOrEmpty(Text_Valor_Temporal.Text.Trim()))
                {
                    V_Fiscal = 0.00;
                }
                else
                {
                    V_Fiscal = Convert.ToDouble(Text_Valor_Temporal.Text.Trim());

                }
                Factor = Tasa / 1000;
                Importe = Factor * V_Fiscal;
                Periodo = Grid_Diferencias.Rows[Contador].Cells[0].Text;
                Anio_Final = Periodo.Substring(Periodo.Length - 4);
                Bimestres = Periodo.Split('-');
                if (Anio_Final.Trim() != Const_Anio_Corriente.ToString().Trim())
                {
                    Dt_Agregar_Diferencias.Rows[Contador]["TIPO_PERIODO"] = "REZAGO";
                    B_Inicial = Int32.Parse(Bimestres[0][0].ToString());
                    B_Final = Int32.Parse(Bimestres[1][1].ToString());
                }
                else
                {
                    Dt_Agregar_Diferencias.Rows[Contador]["TIPO_PERIODO"] = "CORRIENTE";
                    B_Inicial = Int32.Parse(Bimestres[0][0].ToString());
                    B_Final = Int32.Parse(Bimestres[1][1].ToString());
                }
                Validacion_Cuota_Minima = Consulta_Cuota_Minima_Anio(Anio_Final);
                if (Importe < Validacion_Cuota_Minima)
                    Importe = Validacion_Cuota_Minima;
                Lapzo = (B_Final - B_Inicial) + 1;
                C_Bimestral = Math.Round(Importe, 2) / 6;
                if (Importe != Validacion_Cuota_Minima)
                    Importe = C_Bimestral * Lapzo;
                if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString()))
                {
                    if (Convert.ToDouble(V_Fiscal.ToString()) - Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString()) > 0 || Convert.ToDouble(V_Fiscal.ToString()) - Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString()) < 0)
                        Recalcular = true;
                }
                if (!String.IsNullOrEmpty(Text_Importe.Text.Trim()))
                {
                    if (((Convert.ToDouble(Text_Importe.Text.Trim()) - Importe) > 1 || (Convert.ToDouble(Text_Importe.Text.Trim()) - Importe) < 1) && Convert.ToDouble(Text_Importe.Text.Trim()) > 0)
                        Recalcular = false;
                }

                if (Metodo == "Grid_Diferencias_RowCommand" && V_Fiscal > 0.00 || Recalcular)
                {
                    Text_Importe.Text = Math.Round(Importe, 2).ToString("#,###,##0.00");
                    //Text_Cuota_Bim_Temp.Text = Txt_Cuota_Bimestral.Text.Trim();//Se comento devido a que el usuario pidio que fuera la misma CB que la de impuestos y luego siempre no
                    Text_Cuota_Bim_Temp.Text = C_Bimestral.ToString("#,###,##0.00");
                }
                else
                {
                    if (V_Fiscal > 0.00 && Recalcular)
                    {
                        if (String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()) || (Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()) == 0))
                            Text_Importe.Text = Math.Round(Importe, 2).ToString("#,###,##0.00");
                        if (String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()) || (Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()) == 0))
                            //Text_Cuota_Bim_Temp.Text = Txt_Cuota_Bimestral.Text.Trim();//Se comento devido a que el usuario pidio que fuera la misma CB que la de impuestos y luego siempre no
                            Text_Cuota_Bim_Temp.Text = C_Bimestral.ToString("#,###,##0.00");
                    }
                }
                Recalcular = true;
                //Devolver Valores Calculados al Datatable Agregar diferencias
                for (Int32 Cont = 0; Cont < Grid_Diferencias.Rows.Count; Cont++)
                {
                    TextBox Text_Valor_Temporal_Guardar = (TextBox)Grid_Diferencias.Rows[Cont].Cells[2].FindControl("Txt_Grid_Dif_Valor_Fiscal");
                    TextBox Text_Cuota_Bim_Temp_Guardar = (TextBox)Grid_Diferencias.Rows[Cont].Cells[6].FindControl("Txt_Grid_Cuota_Bimestral");
                    TextBox Text_Importe_Guardar = (TextBox)Grid_Diferencias.Rows[Cont].Cells[5].FindControl("Txt_Grid_Importe");
                    DropDownList Cmb_Tipo_Dif_Guardar = (DropDownList)Grid_Diferencias.Rows[Cont].Cells[1].FindControl("Cmb_Tipo_Diferencias");
                    Tasa = Convert.ToDouble(Grid_Diferencias.Rows[Cont].Cells[3].Text);
                    Dt_Agregar_Diferencias.Rows[Cont]["PERIODO"] = Grid_Diferencias.Rows[Cont].Cells[0].Text;
                    Dt_Agregar_Diferencias.Rows[Cont]["TASA"] = Grid_Diferencias.Rows[Cont].Cells[3].Text;
                    Dt_Agregar_Diferencias.Rows[Cont]["TIPO"] = Cmb_Tipo_Dif_Guardar.SelectedValue.ToString();
                    if (!String.IsNullOrEmpty(Text_Importe_Guardar.Text.Trim()))
                        Dt_Agregar_Diferencias.Rows[Cont]["IMPORTE"] = Text_Importe_Guardar.Text.Trim();
                    if (!String.IsNullOrEmpty(Text_Cuota_Bim_Temp_Guardar.Text.Trim()))
                        Dt_Agregar_Diferencias.Rows[Cont]["CUOTA_BIMESTRAL"] = Text_Cuota_Bim_Temp_Guardar.Text.Trim();
                    if (!String.IsNullOrEmpty(Text_Valor_Temporal_Guardar.Text.Trim()))
                        Dt_Agregar_Diferencias.Rows[Cont]["VALOR_FISCAL"] = Text_Valor_Temporal_Guardar.Text.Trim();
                    //Dt_Agregar_Diferencias.Rows[Contador]["TASA"] = Grid_Diferencias.Rows[Contador].Cells[4].Text;
                    //Dt_Agregar_Diferencias.Rows[Contador]["TASA_ID"] = Grid_Diferencias.Rows[Contador].Cells[3].Text;
                }
                Calcular_Resumen();
            }
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
        }

    }
    #endregion
    //Metodos Grids
    #region Metodos Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Co
    ///DESCRIPCIÓN: Cargar datos de copropietarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:48:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Grid_Co(int Page_Index)
    {
        try
        {
            Grid_Copropietarios.Columns[0].Visible = true;
            Grid_Copropietarios.DataSource = null;
            Grid_Copropietarios.PageIndex = Page_Index;
            Grid_Copropietarios.DataSource = (DataTable)Session["Dt_Agregar_Co_Propietarios"];
            Grid_Copropietarios.DataBind();
            Grid_Copropietarios.Columns[0].Visible = false;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Ordenes
    ///DESCRIPCIÓN: Cargar datos de ordenes de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 01/Dic/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Cargar_Grid_Ordenes(int Page_Index)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Ordenes_Temporal = null;
        try
        {
            Grid_Ordenes_Variacion.DataSource = null;
            Orden_Variacion_Negocio.P_Cuenta_Predial = Txt_Busqueda_Cuenta.Text.Trim();
            Orden_Variacion_Negocio.P_Contrarecibo = Txt_Busqueda_Contrarecibo.Text.Trim();
            Orden_Variacion_Negocio.P_Incluir_Campos_Foraneos = true;
            if (!String.IsNullOrEmpty(Orden_Variacion_Negocio.P_Cuenta_Predial))
                Orden_Variacion_Negocio.P_Filtros_Dinamicos = Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM " + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE UPPER(" + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + ") LIKE UPPER('%" + Orden_Variacion_Negocio.P_Cuenta_Predial + "%') )";
            else if (!String.IsNullOrEmpty(Orden_Variacion_Negocio.P_Contrarecibo))
                Orden_Variacion_Negocio.P_Filtros_Dinamicos += " NO_CONTRARECIBO LIKE '%" + Orden_Variacion_Negocio.P_Contrarecibo + "'";
            Dt_Ordenes_Temporal = Orden_Variacion_Negocio.Consultar_Ordenes_Variacion();
            Session["Dt_Ordenes_Variacion"] = Dt_Ordenes_Temporal;
            if (Session["Dt_Ordenes_Variacion"] != null)
            {
                Grid_Ordenes_Variacion.PageIndex = Page_Index;
                Grid_Ordenes_Variacion.DataSource = (DataTable)Session["Dt_Ordenes_Variacion"];
                Grid_Ordenes_Variacion.DataBind();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Diferencias
    ///DESCRIPCIÓN: Cargar datos del analiisis de rezago
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:48:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    private void Cargar_Grid_Diferencias(int Page_Index)
    {
        try
        {
            Grid_Diferencias.DataSource = null;
            Grid_Diferencias.PageIndex = Page_Index;
            if (Session["Dt_Agregar_Diferencias"] != null && !Hdn_Cargar_Modulos.Value.Contains("CION_CUENTAS"))
                Grid_Diferencias.DataSource = (DataTable)Session["Dt_Agregar_Diferencias"];
            else
                Grid_Diferencias.DataSource = null;
            Grid_Diferencias.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion
    //Metodos de Carga de Datos
    #region Metodos Cargar Datos
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
            Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = (Cls_Ope_Pre_Orden_Variacion_Negocio)Session["M_Orden_Negocio"];
            Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
            DataTable Dt_Contribuyentes = (DataTable)Session["Dt_Contribuyentes"];
            if (dataTable.Rows.Count > 0 && dataTable != null)
            {
                Hdn_Propietario_ID.Value = dataTable.Rows[0]["CONTRIBUYENTE"].ToString();
                M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["CONTRIBUYENTE"].ToString(); ;

                Txt_Nombre_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                if (dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                {
                    Cmb_Tipos_Propietario.SelectedValue = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                    M_Orden_Negocio.P_Tipo_Propietario = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                }
                Txt_Rfc_Propietario.Text = HttpUtility.HtmlDecode(dataTable.Rows[0]["RFC"].ToString());
                M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
                Session["M_Orden_Negocio"] = M_Orden_Negocio;
                M_Orden_Negocio.P_Dt_Contribuyentes = Dt_Contribuyentes;
                M_Orden_Negocio.Alta_Propietario(dataTable.Rows[0]["CONTRIBUYENTE"].ToString(), "ALTA", dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString());
                Session["Dt_Contribuyentes"] = M_Orden_Negocio.P_Dt_Contribuyentes;
                Hdn_Propietario_Validacion_Superficie.Value = Hdn_Propietario_ID.Value;
                Contribuyente.P_Contribuyente_ID = Hdn_Propietario_ID.Value;
                Contribuyente = Contribuyente.Consultar_Datos_Contribuyente();
                Hdn_Propietario_Validacion_Persona.Value = Contribuyente.P_Tipo_Persona;
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
    private void Cargar_Generales_Cuenta(DataTable dataTable, Boolean Modo)
    {
        String Periodo, Efecto, Anio;
        String[] Arr_Efectos;
        DataTable Dt_Agregar_Co_Propietarios = (DataTable)Session["Dt_Agregar_Co_Propietarios"];
        DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        DataTable Dt_Contribuyentes = (DataTable)Session["Dt_Contribuyentes"];
        DataTable Dt_Consulta_Temp;
        DataTable Dt_Consulta_Temp_Tipo;
        try
        {
            Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
            Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = (Cls_Ope_Pre_Orden_Variacion_Negocio)Session["M_Orden_Negocio"];
            //Asignacion de valores a Objeto de Negocio y cajas de texto
            //Hdn_Contrarecibo.Value = dataTable.Rows[0]["CONTRARECIBO"].ToString();
            //if (!String.IsNullOrEmpty(dataTable.Rows[0]["ID"].ToString()))
            //{

            //    M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();

            //    Hdn_Cuenta_ID.Value = dataTable.Rows[0]["ID"].ToString();
            //    M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0]["ID"].ToString();
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString()))
            {
                Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
                //if (Modo)
                //    Txt_Cuenta_Predial.Font.Bold = true;
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString()))
            {
                Txt_Cta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
                M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
                //if (Modo)
                //    Txt_Cta_Origen.Font.Bold = true;
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() != string.Empty)
            {
                try
                {
                    Cmb_Tipos_Predio.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                }
                catch
                { }
                M_Orden_Negocio.P_Tipo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString();
                //if (Modo)
                //    Cmb_Tipos_Predio.Font.Bold = true;
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                try
                {
                    Cmb_Usos_Predio.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                }
                catch
                {
                    Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Suelo = new Cls_Cat_Pre_Uso_Suelo_Negocio();
                    Uso_Suelo.P_Uso_Suelo_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                    Uso_Suelo = Uso_Suelo.Consultar_Datos_Uso_Suelo();
                    if (!String.IsNullOrEmpty(Uso_Suelo.P_Descripcion))
                    {
                        ListItem Nuevo_Item = new ListItem(Uso_Suelo.P_Descripcion + "-BAJA", dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString());
                        Cmb_Usos_Predio.Items.Add(Nuevo_Item);
                        Cmb_Usos_Predio.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                    }
                    else
                    {
                        Mensaje_Error("Ocurrio un error al cargar el uso de suelo");
                        Cmb_Usos_Predio.SelectedIndex = 0;
                    }
                }
                M_Orden_Negocio.P_Uso_Suelo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString();
                //if (Modo)
                //    Cmb_Usos_Predio.Font.Bold = true;
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() != string.Empty)
            {
                try
                {
                    Cmb_Estados_Predio.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                }
                catch
                {

                }
                M_Orden_Negocio.P_Estado_Predial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
                //if (Modo)
                //    Cmb_Estados_Predio.Font.Bold = true;
            }
            if (Modo)
            {
                if (dataTable.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta].ToString() != string.Empty)
                {
                    Cmb_Estatus.SelectedValue = dataTable.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta].ToString();
                    M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta].ToString();
                    //Cmb_Estatus.Font.Bold = true;
                }
            }
            else
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
                {
                    Cmb_Estatus.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
                    M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
                    //Cmb_Estatus.Font.Bold = true;
                }
            }
            if (!string.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString()))
            {
                Txt_Costo_M2.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString()).ToString("#,###,#0.00");
                //if (Modo)
                //Txt_Costo_M2.Font.Bold = true;
            }
            Hdn_Superficie_Construccion.Value = "0";
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString()))
            {
                Txt_Superficie_Construida.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString()).ToString("#,###,#0.00");
                M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
                if (!Modo)
                    Hdn_Superficie_Construccion.Value = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString()).ToString("#,###,#0.00");
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString()))
            {
                Txt_Superficie_Total.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString()).ToString("#,###,#0.00");
                M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
                //if (Modo)
                //    Txt_Superficie_Total.Font.Bold = true;
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString()))
            {
                Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                //if (Modo)
                //    Txt_No_Exterior.Font.Bold = true;
            }
            else
            {
                Txt_No_Exterior.Text = "";
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString()))
            {
                Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                //if (Modo)
                //    Txt_No_Interior.Font.Bold = true;
            }
            else
            {
                Txt_No_Interior.Text = "";
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString()))
            {
                Txt_Catastral.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
                M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
                //if (Modo)
                //    Txt_Catastral.Font.Bold = true;
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            {
                Periodo = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
                Arr_Efectos = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString().Split('/');
                if (Arr_Efectos.Length > 1)
                {
                    Anio = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString().Split('/')[1];
                    try
                    {
                        Cmb_Efectos.SelectedValue = Anio;
                    }
                    catch { }
                    //if (Modo)
                    //    Cmb_Efectos.Font.Bold = true;
                    Efecto = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString().Split('/')[0];
                    try
                    {
                        Cmb_Efectos_Numero.SelectedValue = Efecto;
                    }
                    catch { }
                    M_Orden_Negocio.P_Efectos_Año = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
                    //if (Modo)
                    //    Cmb_Efectos_Numero.Font.Bold = true;
                }
                else
                {
                    try
                    {
                        Cmb_Efectos.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
                    }
                    catch { }
                    M_Orden_Negocio.P_Efectos_Año = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
                    //if (Modo)
                    //    Cmb_Efectos.Font.Bold = true;
                }

            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()))
            {
                Txt_Valor_Fiscal.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("#,###,#0.00");
                M_Orden_Negocio.P_Valor_Fiscal = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("#,###,#0.00");
                //if (Modo)
                //    Txt_Valor_Fiscal.Font.Bold = true;
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString()))
            {
                Txt_Periodo_Corriente.Text = "1/" + Const_Anio_Corriente.ToString() + " - 6/" + Const_Anio_Corriente.ToString();
                M_Orden_Negocio.P_Periodo_Corriente_Inicial = "1/" + Const_Anio_Corriente.ToString() + " - 6/" + Const_Anio_Corriente.ToString();
                if (Modo)
                {
                    Txt_Periodo_Corriente.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente].ToString();
                    //Txt_Periodo_Corriente.Font.Bold = true;
                }
            }
            else
            {
                if (!String.IsNullOrEmpty(Hdn_Orden_Variacion_Anio.Value.ToString()))
                    Txt_Periodo_Corriente.Text = "1/" + Hdn_Orden_Variacion_Anio.Value.ToString() + " - 6/" + Hdn_Orden_Variacion_Anio.Value.ToString();
                else
                    Txt_Periodo_Corriente.Text = "1/" + Const_Anio_Corriente.ToString() + " - 6/" + Const_Anio_Corriente.ToString();
            }


            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()))
            {
                Txt_Cuota_Anual.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()).ToString("#,###,##0.00");
                Txt_Cuota_Bimestral.Text = (Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString()) / 6).ToString("#,###,##0.00");
                //if (Modo)
                //{
                //    Txt_Cuota_Anual.Font.Bold = true;
                //    Txt_Cuota_Bimestral.Font.Bold = true;
                //}
                M_Orden_Negocio.P_Cuota_Anual = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString());
                M_Orden_Negocio.P_Cuota_Bimestral = ((Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual].ToString())) / 6);
                Session["Cuota_Bimestral"] = Txt_Cuota_Bimestral.Text;
                Session["Cuota_Anual"] = Txt_Cuota_Anual.Text;
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString()))
            {
                Txt_Porcentaje_Excencion.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString()).ToString("#,###,#0.00"); ;
                M_Orden_Negocio.P_Exencion = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion].ToString());
                //if (Modo)
                //    Txt_Porcentaje_Excencion.Font.Bold = true;
            }
            if (!Modo)
            {
                if (dataTable.Rows[0]["FECHA_AVALUO_FORMATEADA"].ToString() != "" && dataTable.Rows[0]["FECHA_AVALUO_FORMATEADA"] != null && dataTable.Rows[0]["FECHA_AVALUO_FORMATEADA"].ToString() != "01/Ene/0001")
                {
                    String Str_Fecha = dataTable.Rows[0]["FECHA_AVALUO_FORMATEADA"].ToString();
                    DateTime Dte_Fecha = Formatear_Fecha(Str_Fecha, false);
                    M_Orden_Negocio.P_Fecha_Avaluo = Dte_Fecha;
                    //CE_Txt_Fecha_Avaluo.SelectedDate = Dte_Fecha;
                    Txt_Fecha_Avaluo.Text = Dte_Fecha.Day.ToString() + "/" + Formatear_Fecha(Dte_Fecha.Month.ToString()) + "/" + Dte_Fecha.Year.ToString();
                    //if (Modo)
                    //    Txt_Fecha_Avaluo.Font.Bold = true;
                }

                if (dataTable.Rows[0]["TERMINO_EXENCION_FORMATEADA"].ToString() != "" && dataTable.Rows[0]["TERMINO_EXENCION_FORMATEADA"] != null && dataTable.Rows[0]["TERMINO_EXENCION_FORMATEADA"].ToString() != "01/Ene/0001")
                {
                    String Str_Fecha = dataTable.Rows[0]["TERMINO_EXENCION_FORMATEADA"].ToString();
                    DateTime Dte_Fecha = Formatear_Fecha(Str_Fecha, false);
                    //CE_Txt_Fecha_Inicial.SelectedDate = Dte_Fecha;
                    Txt_Fecha_Inicial.Text = Dte_Fecha.Day.ToString() + "/" + Formatear_Fecha(Dte_Fecha.Month.ToString()) + "/" + Dte_Fecha.Year.ToString();
                    M_Orden_Negocio.P_Fecha_Termina_Exencion = Dte_Fecha;
                    //if (Modo)
                    //    Txt_Fecha_Inicial.Font.Bold = true;
                }
            }
            else
            {
                if (dataTable.Rows[0]["FECHA_AVALUO"].ToString() != "" && dataTable.Rows[0]["FECHA_AVALUO"] != null && dataTable.Rows[0]["FECHA_AVALUO"].ToString().ToUpper().Trim() != "01/01/0001 12:00:00 A.M.")
                {
                    String Str_Fecha = dataTable.Rows[0]["FECHA_AVALUO"].ToString();
                    DateTime Dte_Fecha = Formatear_Fecha(Str_Fecha, false);
                    M_Orden_Negocio.P_Fecha_Avaluo = Dte_Fecha;
                    //CE_Txt_Fecha_Avaluo.SelectedDate = Dte_Fecha;
                    Txt_Fecha_Avaluo.Text = Dte_Fecha.Day.ToString() + "/" + Formatear_Fecha(Dte_Fecha.Month.ToString()) + "/" + Dte_Fecha.Year.ToString();
                    //if (Modo)
                    //    Txt_Fecha_Avaluo.Font.Bold = true;
                }

                if (dataTable.Rows[0]["TERMINO_EXENCION"].ToString() != "" && dataTable.Rows[0]["TERMINO_EXENCION"] != null && dataTable.Rows[0]["TERMINO_EXENCION"].ToString().ToUpper().Trim() != "01/01/0001 12:00:00 A.M.")
                {
                    String Str_Fecha = dataTable.Rows[0]["TERMINO_EXENCION"].ToString();
                    DateTime Dte_Fecha = Formatear_Fecha(Str_Fecha, false);
                    //CE_Txt_Fecha_Inicial.SelectedDate = Dte_Fecha;
                    Txt_Fecha_Inicial.Text = Dte_Fecha.Day.ToString() + "/" + Formatear_Fecha(Dte_Fecha.Month.ToString()) + "/" + Dte_Fecha.Year.ToString();
                    M_Orden_Negocio.P_Fecha_Termina_Exencion = Dte_Fecha;
                    //if (Modo)
                    //    Txt_Fecha_Inicial.Font.Bold = true;
                }

            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString()))
            {
                Txt_Dif_Construccion.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion]).ToString("#,###,#0.00");
                Txt_Exedente_Construccion.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion]).ToString("#,###,#0.00");
                M_Orden_Negocio.P_Diferencia_Construccion = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString();
                //if (Modo)
                //    Txt_Dif_Construccion.Font.Bold = true;
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString() != "" && dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] != null)
            {
                M_Orden_Negocio.P_Cuota_Minima = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString());
                Hdn_Cuota_Minima.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
                //Cmb_Cuota_Minima.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID].ToString();
            }
            //Seccion de carga de datos de la cuota fija
            #region [VISTA PREVIA DE ADEUDOS]
            //SE ESTABLECE LA VARIABLE DE SESSION PARA LA VISTA PREVIA DE ADEUDOS SE PASA EL MONTO DE LA CUOTA FIJA
            Limpiar_Session_Cuotas_Fijas();
            if (!Modo)
            {
                Consultar_Ultima_Cuota_Fija();
            }

            #endregion
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString()))
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "NO" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "no" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "No")
                {
                    Chk_Cuota_Fija.Checked = false;
                    M_Orden_Negocio.P_Cuota_Fija = "NO";
                    Pnl_Detalles_Cuota_Fija.Style.Value = "display:none;";
                }
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
                {
                    Btn_Calcular_Cuota_Fija.Style.Value = "display:none;";
                    Chk_Cuota_Fija.Checked = true;
                    M_Orden_Negocio.P_Cuota_Fija = "SI";
                    Pnl_Detalles_Cuota_Fija.Style.Value = "display:inline;";

                    //----Cargar detalles de la cuota Fija
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
                    {
                        M_Orden_Negocio.P_No_Cuota_Fija = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString();
                        Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
                        if (Modo)
                        {
                            Session.Remove("Cuota_Fija_Nueva");
                            Session["Cuota_Fija_Nueva"] = "";
                            Session["Cuota_Fija_Nueva"] = Txt_Total_Cuota_Fija.Text;
                        }
                    }
                    if (!Modo)
                    {
                        if (Cmb_Solicitante.SelectedItem.Text != "" && Cmb_Solicitante.SelectedIndex > 0)
                        {
                            String Beneficio;
                            Beneficio = Cmb_Solicitante.SelectedItem.Text.ToUpper();
                            if (Beneficio.Contains("PENSION") || Beneficio.Contains("TERCER") || Beneficio.Contains("JUBILA") || Beneficio.Contains("SESENTA"))
                                Session["Quitar_Cuota_Fija"] = "PEDIR_DATOS";
                        }
                    }
                }
                else
                {
                    Cmb_Solicitante.SelectedIndex = 0;
                    Cmb_Financiado.SelectedIndex = 0;

                    Txt_Plazo.Text = "";
                    Txt_Excedente_Construccion_Total.Text = "0.00";
                    Txt_Tasa_Valor_Total.Text = "0.00";
                    Txt_Total_Cuota_Fija.Text = "0.00";
                    Txt_Fundamento.Text = "";
                    Session["Quitar_Cuota_Fija"] = "NO_PEDIR_DATOS";
                }
                //if (Modo)
                //    Chk_Cuota_Fija.Font.Bold = true;               
            }



            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString()))
            {
                DataRow Dr_Tasa_Seleccionada;
                M_Orden_Negocio.P_Tasa = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString());
                Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
                Cls_Ope_Pre_Tasas_Anuales_Negocio Tasas_Negocio = new Cls_Ope_Pre_Tasas_Anuales_Negocio();
                Tasas_Negocio.P_Tasa_Predial_ID = Hdn_Tasa_ID.Value;

                Dr_Tasa_Seleccionada = Tasas_Negocio.Consultar_Tasas_Anuales().Rows[0];
                Txt_Tasa_Descripcion.Text = Dr_Tasa_Seleccionada["IDENTIFICADOR"].ToString() + " - " + Dr_Tasa_Seleccionada["DESCRIPCION"].ToString() + " - " + Dr_Tasa_Seleccionada["ANIO"].ToString();
                Txt_Tasa_Porcentaje.Text = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                Txt_Tasa_Excedente_Valor.Text = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                Txt_Tasa_Exedente_Construccion.Text = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                Calcular_Cuota(false);
                Calcular_Excedentes();
                //if (Modo)
                //{
                //    Txt_Tasa_Descripcion.Font.Bold = true;
                //    Txt_Tasa_Porcentaje.Font.Bold = true;
                //    Txt_Tasa_Excedente_Valor.Font.Bold = true;
                //    Txt_Tasa_Exedente_Construccion.Font.Bold = true;
                //}
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion].ToString()))
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion].ToString() == "00001")
                    Txt_Estado.Text = "GUANAJUATO";
                //M_Orden_Negocio.P_Estado_Propietario = (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString());
                //if (Modo)
                //    Txt_Estado.Font.Bold = true;
            }
            else if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString()))
            {
                Txt_Estado.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion].ToString()))
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion].ToString() == "00000")
                    Txt_Ciudad.Text = "IRAPUATO";
                else if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString()))
                {
                    Txt_Ciudad.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString();
                    //M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString();
                    //if (Modo)
                    //    Txt_Ciudad.Font.Bold = true;
                }
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString()))
            {
                Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                //if (Modo)
                //    Txt_Numero_Exterior_Propietario.Font.Bold = true;
            }
            //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString()))
            //{
            Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
            M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
            //if (Modo)
            //    Txt_Numero_Interior_Propietario.Font.Bold = true;
            //}
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString()))
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo].ToString() == "SI")
                {
                    Cmb_Domicilio_Foraneo.SelectedValue = "SI";
                    M_Orden_Negocio.P_Domicilio_Foraneo = "SI";
                    //Chekar si es mismo domicilio
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString())
                    {
                        if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString())
                        {
                            if ((dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString()) && (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString()))
                                Chk_Mismo_Domicilio.Checked = true;
                            else
                                Chk_Mismo_Domicilio.Checked = false;
                        }
                    }
                    else
                    {
                        Chk_Mismo_Domicilio.Checked = false;
                    }
                    if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString()))
                    {
                        Txt_Calle_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString();
                        //if (Modo)
                        //    Txt_Calle_Propietario.Font.Bold = true;
                    }
                    if (!string.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString()))
                    {
                        Txt_Colonia_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString();
                        //if (Modo)
                        //    Txt_Colonia_Propietario.Font.Bold = true;
                    }
                    if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString()))
                    {
                        Txt_Estado.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString();
                        //if (Modo)
                        //    Txt_Estado.Font.Bold = true;
                    }
                    if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString()))
                    {
                        Txt_Ciudad.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString();
                        //if (Modo)
                        //    Txt_Ciudad.Font.Bold = true;
                    }
                    Txt_Calle_Propietario.Style.Add("disabled", "false");
                    Txt_Colonia_Propietario.Style.Add("disabled", "false");
                }
                else
                {
                    Cmb_Domicilio_Foraneo.SelectedValue = "NO";
                    M_Orden_Negocio.P_Domicilio_Foraneo = "NO";
                    Txt_Calle_Propietario.Style.Add("disabled", "true");
                    Txt_Colonia_Propietario.Style.Add("disabled", "true");
                    if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString()))
                    {
                        Hdn_Colonia_ID_Notificacion.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString();
                        Txt_Colonia_Propietario.Text = M_Orden_Negocio.Consulta_Nombre_Colonia(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString());
                        //if (Modo)
                        //    Txt_Colonia_Propietario.Font.Bold = true;
                    }
                    if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString()))
                    {
                        Hdn_Calle_ID_Notificacion.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString();
                        Txt_Calle_Propietario.Text = M_Orden_Negocio.Consulta_Nombre_Calle(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString());
                        //if (Modo)
                        //    Txt_Calle_Propietario.Font.Bold = true;
                    }
                    //Chekar si es mismo domicilio
                    if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString())
                    {
                        if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString())
                        {
                            if ((dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString().Trim() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim()) && (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString().Trim() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim()))
                                Chk_Mismo_Domicilio.Checked = true;
                            else
                                Chk_Mismo_Domicilio.Checked = false;
                        }
                    }
                    else
                    {
                        Chk_Mismo_Domicilio.Checked = false;
                    }
                }
                //if (Modo)
                //    Cmb_Domicilio_Foraneo.Font.Bold = true;
            }
            else
            {
                if (Modo)
                {
                    if (Cmb_Domicilio_Foraneo.SelectedValue == "SI")
                    {
                        Txt_Calle_Propietario.Style.Add("disabled", "false");
                        Txt_Colonia_Propietario.Style.Add("disabled", "false");
                        if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString()))
                        {
                            Txt_Calle_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion].ToString();
                            //if (Modo)
                            //    Txt_Calle_Propietario.Font.Bold = true;
                        }
                        if (!string.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString()))
                        {
                            Txt_Colonia_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion].ToString();
                            //if (Modo)
                            //    Txt_Colonia_Propietario.Font.Bold = true;
                        }
                        if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString()))
                        {
                            Txt_Estado.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion].ToString();
                            //if (Modo)
                            //    Txt_Estado.Font.Bold = true;
                        }
                        if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString()))
                        {
                            Txt_Ciudad.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion].ToString();
                            //if (Modo)
                            //    Txt_Ciudad.Font.Bold = true;
                        }
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Verificar Mismo Domicilio", "document.getElementById('<%= Chk_Cuota_Fija.ClientID %>').checked = false;", true);
                    }
                    else
                    {
                        if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString()))
                        {
                            Hdn_Colonia_ID_Notificacion.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString();
                            Txt_Colonia_Propietario.Text = M_Orden_Negocio.Consulta_Nombre_Colonia(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString());
                            //if (Modo)
                            //    Txt_Colonia_Propietario.Font.Bold = true;
                        }
                        if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString()))
                        {
                            Hdn_Calle_ID_Notificacion.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString();
                            Txt_Calle_Propietario.Text = M_Orden_Negocio.Consulta_Nombre_Calle(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString());
                            //if (Modo)
                            //    Txt_Calle_Propietario.Font.Bold = true;
                        }

                        //Chekar si es mismo domicilio
                        if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString()) && !string.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString()))
                        {
                            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString())
                            {
                                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString())
                                {
                                    if ((dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString()) && (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString() == dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString()))
                                        Chk_Mismo_Domicilio.Checked = true;
                                    else
                                        Chk_Mismo_Domicilio.Checked = false;
                                }
                                else
                                {
                                    Chk_Mismo_Domicilio.Checked = false;
                                }
                            }
                            else
                            {
                                Chk_Mismo_Domicilio.Checked = false;
                            }
                        }
                        else
                        {
                            Chk_Mismo_Domicilio.Checked = false;
                        }
                    }
                    //Cmb_Domicilio_Foraneo.SelectedValue = "NO";
                    //M_Orden_Negocio.P_Domicilio_Foraneo = "NO";
                    //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString()))
                    //{
                    //    Txt_Colonia_Propietario.Text = M_Orden_Negocio.Consulta_Nombre_Colonia(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString());
                    //    Hdn_Colonia_ID_Notificacion.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID].ToString();
                    //    if (Modo)
                    //        Txt_Colonia_Propietario.Font.Bold = true;
                    //}
                    //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString()))
                    //{
                    //    Txt_Calle_Propietario.Text = M_Orden_Negocio.Consulta_Nombre_Calle(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString());
                    //    Hdn_Calle_ID_Notificacion.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                    //    if (Modo)
                    //        Txt_Calle_Propietario.Font.Bold = true;

                    //}
                    //    if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString()))
                    //{
                    //    Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion].ToString();
                    //    if (Modo)
                    //        Txt_Numero_Exterior_Propietario.Font.Bold = true;
                    //}
                    //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString()))
                    //{
                    //    Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion].ToString();
                    //    if (Modo)
                    //        Txt_Numero_Interior_Propietario.Font.Bold = true;
                    //}
                    //Txt_Calle_Propietario.Font.Bold = true;
                    //Txt_Colonia_Propietario.Font.Bold = true;
                }
            }

            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString()))
            {
                Txt_CP.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString();
                M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal].ToString();
                //if (Modo)
                //    Txt_CP.Font.Bold = true;
            }

            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString()))
            {
                Hdn_Tasa_General_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID].ToString();
            }
            //Consultar Propietarios y Copropietarios de la Orden
            if (!String.IsNullOrEmpty(Hdn_Cuenta_ID.Value))
            {
                M_Orden_Negocio.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
                if (Modo)
                {
                    M_Orden_Negocio.P_Año = Int32.Parse(Hdn_Orden_Variacion_Anio.Value);
                    M_Orden_Negocio.P_Orden_Variacion_ID = Hdn_Orden_Variacion.Value;
                    M_Orden_Negocio.P_Copropietario_Filtra_Estatus = true;
                    DataSet Ds_Co_Prop = M_Orden_Negocio.Consultar_Copropietarios_Variacion();
                    //if (Ds_Co_Prop.Tables[0].Rows.Count > 0)
                    //{
                    M_Orden_Negocio.P_Dt_Copropietarios = Ds_Co_Prop.Tables["Dt_Copropietarios_Variacion"];
                    //}
                    M_Orden_Negocio.P_Propietario_Filtra_Estatus = true;
                    DataSet Ds_Prop = M_Orden_Negocio.Consultar_Propietarios_Variacion();
                    if (Ds_Prop.Tables[0].Rows.Count > 0)
                    {
                        Txt_Nombre_Propietario.Text = Ds_Prop.Tables["Dt_Copropietarios_Variacion"].Rows[0]["NOMBRE_CONTRIBUYENTE"].ToString();
                        Txt_Rfc_Propietario.Text = HttpUtility.HtmlDecode(Ds_Prop.Tables["Dt_Copropietarios_Variacion"].Rows[0]["RFC"].ToString());
                        Cmb_Tipos_Propietario.SelectedValue = Ds_Prop.Tables["Dt_Copropietarios_Variacion"].Rows[0]["TIPO_PROPIETARIO"].ToString();
                        M_Orden_Negocio.P_Dt_Contribuyentes.Rows.Clear();
                        M_Orden_Negocio.Alta_Propietario(Ds_Prop.Tables["Dt_Copropietarios_Variacion"].Rows[0]["CONTRIBUYENTE_ID"].ToString(), "ALTA", Ds_Prop.Tables["Dt_Copropietarios_Variacion"].Rows[0]["TIPO_PROPIETARIO"].ToString());
                        Session["Dt_Contribuyentes"] = M_Orden_Negocio.P_Dt_Contribuyentes;
                        Hdn_Propietario_ID.Value = Ds_Prop.Tables["Dt_Copropietarios_Variacion"].Rows[0]["CONTRIBUYENTE_ID"].ToString();
                        Contribuyente.P_Contribuyente_ID = Hdn_Propietario_ID.Value;
                        Contribuyente = Contribuyente.Consultar_Datos_Contribuyente();
                        Hdn_Propietario_Validacion_Persona.Value = Contribuyente.P_Tipo_Persona;
                    }
                }
                else
                {
                    M_Orden_Negocio.P_Dt_Copropietarios = M_Orden_Negocio.Consulta_Co_Propietarios();
                    foreach (DataRow Dr in M_Orden_Negocio.P_Dt_Copropietarios.Rows)
                    {
                        DataRow Dr_Con = Dt_Contribuyentes.NewRow();
                        Dr_Con["CONTRIBUYENTE_ID"] = Dr["CONTRIBUYENTE_ID"];
                        Dr_Con["ESTATUS"] = "ALTA";
                        Dr_Con["TIPO"] = Dr["TIPO"];
                        Dt_Contribuyentes.Rows.Add(Dr_Con);

                    }
                }
                Dt_Agregar_Co_Propietarios = M_Orden_Negocio.P_Dt_Copropietarios.Copy();
                Session["Dt_Agregar_Co_Propietarios"] = Dt_Agregar_Co_Propietarios;
                Cargar_Grid_Co(0);
            }
            if (Modo)
            {
                //if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Diferencia].ToString()))
                //{
                M_Orden_Negocio.P_Generar_Orden_No_Orden = Hdn_Orden_Variacion.Value;
                M_Orden_Negocio.P_Generar_Orden_Anio = Hdn_Orden_Variacion_Anio.Value;
                Dt_Agregar_Diferencias = M_Orden_Negocio.Consulta_Diferencias();
                if (Dt_Agregar_Diferencias.Rows.Count > 0)
                {
                    Session["Calcular_Grid_Diferencias"] = "NO";
                    Session["Dt_Agregar_Diferencias"] = Dt_Agregar_Diferencias;
                    Cargar_Grid_Diferencias(0);
                    Calcular_Resumen();
                    Resumen_Grid_Diferencias();
                    Session["Calcular_Grid_Diferencias"] = null;
                }
                //}
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString()))
            {
                Txt_Colonia_Cuenta.Text = M_Orden_Negocio.Consulta_Nombre_Colonia(dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString());
                M_Orden_Negocio.P_Colonia_Cuenta = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                Hdn_Colonia_ID.Value = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();
                if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString()))
                {
                    Txt_Calle_Cuenta.Text = M_Orden_Negocio.Consulta_Nombre_Calle(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString());
                    M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                    Hdn_Calle_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                }
                //if (Modo)
                //    Txt_Calle_Cuenta.Font.Bold = true;
                //if (Modo)
                //    Txt_Colonia_Cuenta.Font.Bold = true;
            }
            else
            {
                if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Calles.Campo_Calle_ID].ToString()))
                {
                    Cls_Ope_Pre_Orden_Variacion_Negocio Consulta_Calles = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                    Txt_Calle_Cuenta.Text = M_Orden_Negocio.Consulta_Nombre_Calle(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString());
                    M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                    Hdn_Calle_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                    //Llenar_Combo_ID(Cmb_Calle_Cuenta, Consulta_Calles.Consulta_Calles_Sin_Colonia(M_Orden_Negocio.P_Ubicacion_Cuenta));
                    //Cmb_Calle_Cuenta.SelectedValue = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
                    //if (Modo)
                    //    Txt_Calle_Cuenta.Font.Bold = true;
                }
            }
            Session["M_Orden_Negocio"] = M_Orden_Negocio;
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Verificar Mismo Domicilio", "Mismo_Domicilio();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Verificar Mismo Domicilio", "Foraneo();", true);
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Verificar Domicilio Foraneo", "Cuota_Fija();", true);
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Formatear_Fecha
    ///DESCRIPCIÓN: cargar datos de la tabla de detalles de la orden
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 11/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DateTime Formatear_Fecha(string Str_Fecha, Boolean Directo)
    {
        String Resultado = Str_Fecha;
        DateTime Dtm_Resultado = Convert.ToDateTime("01/01/0001");
        String[] Separado;
        try
        {
            if (Resultado.Contains(':'))
                Resultado = Resultado.Substring(0, 10);
            if (!DateTime.TryParse(Resultado, out Dtm_Resultado) || Directo)
            {
                Separado = Resultado.Split('/');
                Resultado = Separado[1] + "/" + Separado[0] + "/" + Separado[2];
                Dtm_Resultado = Convert.ToDateTime(Resultado);
            }
        }
        catch (Exception Ex) { Mensaje_Error("Error En las fechas  " + Ex.Message); }
        return Dtm_Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Formatear_Fecha
    ///DESCRIPCIÓN: cargar datos de la tabla de detalles de la orden
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 11/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Formatear_Fecha(string Str_Fecha)
    {
        String Fecha = Str_Fecha;
        String Resultado = "";
        try
        {
            switch (Fecha)
            {
                case "1":
                    Resultado = "Ene";
                    break;
                case "2":
                    Resultado = "Feb";
                    break;
                case "3":
                    Resultado = "Mar";
                    break;
                case "4":
                    Resultado = "Abr";
                    break;
                case "5":
                    Resultado = "May";
                    break;
                case "6":
                    Resultado = "Jun";
                    break;
                case "7":
                    Resultado = "Jul";
                    break;
                case "8":
                    Resultado = "Ago";
                    break;
                case "9":
                    Resultado = "Sep";
                    break;
                case "10":
                    Resultado = "Oct";
                    break;
                case "11":
                    Resultado = "Nov";
                    break;
                case "12":
                    Resultado = "Dic";
                    break;
            }
        }
        catch (Exception Ex) { Mensaje_Error("Error En las fechas  " + Ex.Message); }
        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Variaciones
    ///DESCRIPCIÓN: cargar datos de la tabla de detalles de la orden
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 11/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************  
    private void Cargar_Variaciones()
    {
        DataTable Dt_Detalles;
        DataSet Ds_Campos;
        String Columna;
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = (Cls_Ope_Pre_Orden_Variacion_Negocio)Session["M_Orden_Negocio"];
            if (Session["Cargar_Variaciones"] != null)
            {
                Dt_Detalles = (DataTable)Session["Cargar_Variaciones"];
                //Ds_Campos = M_Orden_Negocio.Obtener_Campos_Tabla(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                //for (int Cont = 0; Cont < Dt_Detalles.Rows.Count; Cont++)
                //{
                //    Ds_Campos.Tables[0].Rows[0][Dt_Detalles.Rows[Cont][3].ToString()] = Dt_Detalles.Rows[Cont][5];
                //}
                //Cargar_Generales_Cuenta(Ds_Campos.Tables[0], true);
                Cargar_Generales_Cuenta(Dt_Detalles, true);
                Txt_Comentarios.Text = M_Orden_Negocio.Consultar_Historial(Hdn_Orden_Variacion.Value);
                Session["Ds_Campos_Reporte"] = Dt_Detalles.Copy();
                Session["M_Orden_Negocio"] = M_Orden_Negocio;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion
    //Metodos para la generacion de la orden
    #region Generar_Orden
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Generar_Orden
    ///DESCRIPCIÓN: analiza las diferencias e invoca a la clase negocio para registrarlas en la orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/12/2011 12:29:57 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Generar_Orden(Boolean modificar)
    {
        String Cuota_Fija_ID;
        DataSet Ds_Actual;
        DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        DataSet Ds_Cargar_combos = null;
        DataRow[] Dr_No_cuota_Fija = null;
        if (Session["Ds_Consulta_Combos"] != null)
        {
            Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
        }
        else
        {
            Consulta_Combos();
            Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
        }
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = (Cls_Ope_Pre_Orden_Variacion_Negocio)Session["M_Orden_Negocio"];
            if (Cmb_Tipos_Movimiento.SelectedIndex > 0 && !Cmb_Tipos_Movimiento.SelectedItem.Text.Contains("(B)"))
            {
                Hdn_Cargar_Modulos.Value = Ds_Cargar_combos.Tables["Dt_Tipos_Movimiento"].Rows[Cmb_Tipos_Movimiento.SelectedIndex - 1][Cat_Pre_Movimientos.Campo_Descripcion].ToString();
                Hdn_Cargar_Modulos.Value = Ds_Cargar_combos.Tables["Dt_Tipos_Movimiento"].Rows[Cmb_Tipos_Movimiento.SelectedIndex - 1][Cat_Pre_Movimientos.Campo_Cargar_Modulos].ToString();
                if (string.IsNullOrEmpty(Hdn_Cargar_Modulos.Value))
                {
                    if (String.IsNullOrEmpty(Hdn_Cuenta_ID.Value)) //&& String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
                    {
                        Ds_Actual = M_Orden_Negocio.Obtener_Campos_Tabla(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);
                        M_Orden_Negocio.P_Generar_Orden_Cuenta_ID = null;
                    }
                    else
                    {
                        if (Session["Ds_Cuenta_Datos_Orden"] != null)
                            Ds_Actual = ((DataSet)Session["Ds_Cuenta_Datos_Orden"]).Copy();
                        else
                            Ds_Actual = M_Orden_Negocio.Obtener_Campos_Tabla(Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas);

                        M_Orden_Negocio.P_Generar_Orden_Cuenta_ID = Hdn_Cuenta_ID.Value;
                    }
                    M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
                    Guardar_Cambios(Ds_Actual);
                    //Agregar Modificaciones a Objeto de negocio
                    M_Orden_Negocio.P_Generar_Orden_Dt_Detalles = (DataTable)Session["P_Generar_Orden_Dt_Detalles"];
                    //Alta de Nuevo Benecifio
                    if (Chk_Cuota_Fija.Checked && Txt_Total_Cuota_Fija.Text.Trim() != "")
                    {
                        if (Cmb_Solicitante.SelectedIndex > 0)
                            M_Orden_Negocio.P_Cuota_Fija_Caso_Especial = Cmb_Solicitante.SelectedValue;
                        if (Cmb_Financiado.SelectedIndex > 0)
                            M_Orden_Negocio.P_Cuota_Fija_Caso_Especial = Cmb_Financiado.SelectedValue;

                        M_Orden_Negocio.P_Cuota_Fija_Cuota_Minima = Txt_Cuota_Minima.Text.Trim();
                        M_Orden_Negocio.P_Cuota_Fija_Excedente_Cons = Txt_Dif_Construccion.Text.Trim();
                        M_Orden_Negocio.P_Cuota_Fija_Excedente_Cons_Total = Txt_Excedente_Construccion_Total.Text.Trim();
                        M_Orden_Negocio.P_Cuota_Fija_Excedente_Valor = Txt_Excedente_Valor.Text.Trim();
                        M_Orden_Negocio.P_Cuota_Fija_Excedente_Valor_Total = Txt_Tasa_Valor_Total.Text.Trim();
                        M_Orden_Negocio.P_Cuota_Fija_Plazo = Txt_Plazo.Text.Trim();
                        M_Orden_Negocio.P_Cuota_Fija_Tasa_ID = Hdn_Tasa_ID.Value;
                        M_Orden_Negocio.P_Cuota_Fija_Tasa_Valor = Txt_Tasa_Porcentaje.Text.Trim();
                        M_Orden_Negocio.P_Cuota_Fija_Total = Convert.ToDouble(Txt_Total_Cuota_Fija.Text.Trim()).ToString("0.##");
                        M_Orden_Negocio.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
                        Cuota_Fija_ID = M_Orden_Negocio.Alta_Beneficio_Couta_Fija();
                        Dr_No_cuota_Fija = M_Orden_Negocio.P_Generar_Orden_Dt_Detalles.Select("CAMPO = 'NO_CUOTA_FIJA'");
                        if (Dr_No_cuota_Fija.Length > 0)
                            M_Orden_Negocio.P_Generar_Orden_Dt_Detalles.Rows.Remove(Dr_No_cuota_Fija[0]);
                        Dr_No_cuota_Fija = M_Orden_Negocio.P_Generar_Orden_Dt_Detalles.Select("CAMPO = 'CUOTA_FIJA'");
                        if (Dr_No_cuota_Fija.Length > 0)
                            M_Orden_Negocio.P_Generar_Orden_Dt_Detalles.Rows.Remove(Dr_No_cuota_Fija[0]);
                        M_Orden_Negocio.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija, Cuota_Fija_ID);
                        M_Orden_Negocio.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija, "SI");
                    }
                    if (Dt_Agregar_Diferencias != null)
                    {
                        if (Dt_Agregar_Diferencias.Rows.Count > 0)
                            M_Orden_Negocio.P_Dt_Diferencias = Dt_Agregar_Diferencias;
                    }
                    if (!String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
                    {
                        if (Hdn_Contrarecibo.Value.Length > 10)
                        {
                            M_Orden_Negocio.P_Año = Int32.Parse(Hdn_Contrarecibo.Value.Substring(11, 4));
                            Hdn_Contrarecibo.Value = Hdn_Contrarecibo.Value.Substring(0, 10);
                        }
                        M_Orden_Negocio.P_Contrarecibo = Hdn_Contrarecibo.Value.Trim();
                    }
                    M_Orden_Negocio.P_Generar_Orden_Movimiento_ID = Cmb_Tipos_Movimiento.SelectedValue.ToString();
                    M_Orden_Negocio.P_Generar_Orden_Estatus = "POR VALIDAR";
                    if (Txt_Observaciones_Cuenta.Text.Trim().Length > 499)
                    {
                        M_Orden_Negocio.P_Generar_Orden_Obserbaciones = Txt_Observaciones_Cuenta.Text.Trim().Substring(0, 499);
                    }
                    else
                    {
                        M_Orden_Negocio.P_Generar_Orden_Obserbaciones = Txt_Observaciones_Cuenta.Text.Trim();
                    }
                    M_Orden_Negocio.P_Tipo_Predio_ID = Cmb_Tipos_Predio.SelectedValue.ToString();
                    M_Orden_Negocio.P_Grupo_Movimiento_ID = M_Orden_Negocio.Consultar_Grupo_Mov(Cmb_Tipos_Movimiento.SelectedValue.ToString()).Rows[0]["GRUPO_ID"].ToString();
                    if (Session["Dt_Contribuyentes"] != null)
                    {
                        M_Orden_Negocio.P_Dt_Contribuyentes = (DataTable)Session["Dt_Contribuyentes"];
                        //M_Orden_Negocio.Cambio_Propietarios();
                    }
                    if (!modificar)
                    {
                        M_Orden_Negocio.P_Generar_Orden_Cuenta_ID = Hdn_Cuenta_ID.Value;
                        M_Orden_Negocio.P_Generar_Orden_Anio = Const_Anio_Corriente.ToString();
                        M_Orden_Negocio.P_Orden_Variacion_ID = M_Orden_Negocio.Generar_Ordenes_Variacion();
                        Hdn_Orden_Variacion.Value = M_Orden_Negocio.P_Orden_Variacion_ID;
                    }
                    else
                    {
                        M_Orden_Negocio.P_Observaciones_Descripcion = Txt_Comentarios.Text.Trim();
                        M_Orden_Negocio.P_Generar_Orden_Anio = Hdn_Orden_Variacion_Anio.Value;
                        M_Orden_Negocio.P_Usuario = Presidencia.Sessiones.Cls_Sessiones.Nombre_Empleado;
                        M_Orden_Negocio.P_Orden_Variacion_ID = Hdn_Orden_Variacion.Value;
                        M_Orden_Negocio.P_Generar_Orden_No_Orden = Hdn_Orden_Variacion.Value;
                        M_Orden_Negocio.P_Generar_Orden_Cuenta_ID = Hdn_Cuenta_ID.Value;
                        if (!String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
                            M_Orden_Negocio.P_Contrarecibo = Hdn_Contrarecibo.Value;
                        M_Orden_Negocio.Modificar_Orden_Variacion_Generada();
                    }
                    if (Txt_Comentarios.Text.Trim() != "")
                    {
                        M_Orden_Negocio.P_Observaciones_No_Orden_Variacion = M_Orden_Negocio.P_Orden_Variacion_ID;
                        M_Orden_Negocio.P_Observaciones_Año = Convert.ToInt16(M_Orden_Negocio.P_Generar_Orden_Anio);
                        M_Orden_Negocio.P_Observaciones_Descripcion = Txt_Comentarios.Text;
                        M_Orden_Negocio.P_Observaciones_Usuraio = Cls_Sessiones.Nombre_Empleado;
                        M_Orden_Negocio.Insertar_Observaciones_Variacion();
                    }
                    Session["FECHA_CREO"] = DateTime.Today;
                    //M_Orden_Negocio.Modificar_Contrarecibo();
                    //M_Orden_Negocio.Consulta_Datos_Reporte(M_Orden_Negocio.P_Orden_Variacion_ID);

                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('La orden se Guardó Exitosamente');", true);
                    Generar_Reporte((DataSet)Session["Ds_Cuenta_Datos_Orden"]);

                    M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                    Session_Remove();
                    Limpiar_Formulario();

                    Estado_Botones(Const_Estado_Inicial);
                }
                else
                    Mensaje_Error("No es podible realizar la orden de variacion con este movimiento");
            }
            else
                Mensaje_Error("Especificar el Movimiento");
            Session["M_Orden_Negocio"] = M_Orden_Negocio;
        }

        catch (Exception Ex)
        {
            Mensaje_Error("Generar_Orden " + Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Guardar_Cambios
    ///DESCRIPCIÓN: se guardan los cambios que afectan a la cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Guardar_Cambios(DataSet Ds_Actual)
    {
        DBNull nulo = null;
        if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
        {
            //Generales
            Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial] = Txt_Cuenta_Predial.Text.Trim();
            if (Cmb_Tipos_Predio.SelectedIndex > 0)
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID] = Cmb_Tipos_Predio.SelectedValue.ToString();
            if (Cmb_Usos_Predio.SelectedIndex > 0)
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID] = Cmb_Usos_Predio.SelectedValue.ToString();
            if (Cmb_Estatus.SelectedIndex > 0)
            {
                Ds_Actual.Tables[0].Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta] = Cmb_Estatus.SelectedItem.Text;
            }
            else
            {
                Ds_Actual.Tables[0].Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Cuenta] = "PENDIENTE";
            }
            if (Cmb_Estados_Predio.SelectedIndex > 0)
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID] = Cmb_Estados_Predio.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(Hdn_Colonia_ID.Value))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID] = Hdn_Colonia_ID.Value;
            if (!String.IsNullOrEmpty(Hdn_Calle_ID.Value))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID] = Hdn_Calle_ID.Value;
            if (Cmb_Efectos.SelectedIndex > 0)
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos] = Cmb_Efectos_Numero.SelectedValue.ToString() + "/" + Cmb_Efectos.SelectedValue.ToString();
            if (!String.IsNullOrEmpty(Txt_Cta_Origen.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen] = Txt_Cta_Origen.Text.Trim().ToUpper();
            if (!String.IsNullOrEmpty(Txt_No_Exterior.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior] = Txt_No_Exterior.Text.Trim().ToUpper(); ;
            //if (!String.IsNullOrEmpty(Txt_No_Interior.Text.Trim()))
            Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior] = Txt_No_Interior.Text.Trim().ToUpper();
            if (!String.IsNullOrEmpty(Txt_Superficie_Construida.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida] = Txt_Superficie_Construida.Text.Trim();
            if (!String.IsNullOrEmpty(Txt_Superficie_Total.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total] = Txt_Superficie_Total.Text.Trim();
            if (!String.IsNullOrEmpty(Txt_Catastral.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral] = Txt_Catastral.Text.Trim().ToUpper();
            if (!String.IsNullOrEmpty(Txt_Costo_M2.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2] = Txt_Costo_M2.Text.Trim();
            //Propietario
            if (Chk_Mismo_Domicilio.Checked) //Si se define el mismo domicilio se copia de los datod generales
            {
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo] = "NO";
                if (!String.IsNullOrEmpty(Hdn_Colonia_ID.Value))
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion] = Hdn_Colonia_ID.Value.ToString();
                if (!String.IsNullOrEmpty(Hdn_Calle_ID.Value))
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion] = Hdn_Calle_ID.Value.ToString();
                if (!String.IsNullOrEmpty(Txt_No_Exterior.Text.Trim()))
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion] = Txt_No_Exterior.Text.Trim().ToUpper();
                //if (!String.IsNullOrEmpty(Txt_No_Interior.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion] = Txt_No_Interior.Text.Trim().ToUpper();
                //Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion] = "GUANAJUATO";
                //Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion] = "IRAPUATO";
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion] = Obtener_Dato_Consulta(Cat_Pre_Ciudades.Campo_Ciudad_ID, Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades, Cat_Pre_Ciudades.Campo_Nombre + " = 'IRAPUATO'");
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion] = Obtener_Dato_Consulta(Cat_Pre_Estados.Campo_Estado_ID, Cat_Pre_Estados.Tabla_Cat_Pre_Estados, Cat_Pre_Estados.Campo_Nombre + " IN ('GUANAJUATO','GTO')");
            }
            else
            {
                if (Cmb_Domicilio_Foraneo.SelectedValue == "SI")
                {
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo] = "SI";
                    if (!String.IsNullOrEmpty(Txt_Colonia_Propietario.Text.Trim()))
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_Notificacion] = Txt_Colonia_Propietario.Text.Trim();
                    if (!String.IsNullOrEmpty(Txt_Calle_Propietario.Text.Trim()))
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_Notificacion] = Txt_Calle_Propietario.Text.Trim();
                    if (!String.IsNullOrEmpty(Txt_Numero_Exterior_Propietario.Text.Trim()))
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion] = Txt_Numero_Exterior_Propietario.Text.Trim();
                    //if (!String.IsNullOrEmpty(Txt_Numero_Interior_Propietario.Text.Trim()))
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion] = Txt_Numero_Interior_Propietario.Text.Trim().ToUpper();
                    //if (!String.IsNullOrEmpty(Txt_Estado.Text.Trim()))
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion] = Txt_Estado.Text.Trim().ToUpper();
                    if (!String.IsNullOrEmpty(Txt_Ciudad.Text.Trim()))
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion] = Txt_Ciudad.Text.Trim().ToUpper();

                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion] = "";
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion] = "";
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion] = "";
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion] = "";
                }
                if (Cmb_Domicilio_Foraneo.SelectedValue == "NO")
                {
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Domicilio_Foraneo] = "NO";
                    if (!String.IsNullOrEmpty(Hdn_Colonia_ID_Notificacion.Value))
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion] = Hdn_Colonia_ID_Notificacion.Value.ToString();
                    else
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Colonia_ID_Notificacion] = Hdn_Colonia_ID.Value.ToString();
                    if (!String.IsNullOrEmpty(Hdn_Calle_ID_Notificacion.Value))
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion] = Hdn_Calle_ID_Notificacion.Value.ToString();
                    else
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID_Notificacion] = Hdn_Calle_ID.Value.ToString();
                    if (!String.IsNullOrEmpty(Txt_Numero_Exterior_Propietario.Text.Trim()))
                        Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior_Notificacion] = Txt_Numero_Exterior_Propietario.Text.Trim().ToUpper();
                    //if (!String.IsNullOrEmpty(Txt_Numero_Interior_Propietario.Text.Trim()))
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior_Notificacion] = Txt_Numero_Interior_Propietario.Text.Trim().ToUpper();
                    //Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Notificacion] = "GUANAJUATO";
                    //Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_Notificacion] = "IRAPUATO";
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Ciudad_ID_Notificacion] = Obtener_Dato_Consulta(Cat_Pre_Ciudades.Campo_Ciudad_ID, Cat_Pre_Ciudades.Tabla_Cat_Pre_Ciudades, Cat_Pre_Ciudades.Campo_Nombre + " = 'IRAPUATO'");
                    Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_ID_Notificacion] = Obtener_Dato_Consulta(Cat_Pre_Estados.Campo_Estado_ID, Cat_Pre_Estados.Tabla_Cat_Pre_Estados, Cat_Pre_Estados.Campo_Nombre + " IN ('GUANAJUATO','GTO')");
                }
            }
            IFormatProvider numberInfo = new System.Globalization.CultureInfo("es-MX", true);
            if (!String.IsNullOrEmpty(Txt_CP.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Codigo_Postal] = Txt_CP.Text.Trim();
            //Impuestos
            if (!String.IsNullOrEmpty(Txt_Valor_Fiscal.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal] = Txt_Valor_Fiscal.Text.Trim();
            if (!String.IsNullOrEmpty(Txt_Periodo_Corriente.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Periodo_Corriente] = "1/" + Const_Anio_Corriente.ToString() + " - 6/" + Const_Anio_Corriente.ToString();
            if (!String.IsNullOrEmpty(Txt_Dif_Construccion.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion] = Txt_Dif_Construccion.Text.Trim();
            if (!String.IsNullOrEmpty(Hdn_Tasa_ID.Value))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID] = Hdn_Tasa_ID.Value;
            if (!String.IsNullOrEmpty(Hdn_Tasa_General_ID.Value))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_Predial_ID] = Hdn_Tasa_General_ID.Value;
            if (!String.IsNullOrEmpty(Txt_Porcentaje_Excencion.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Porcentaje_Exencion] = Txt_Porcentaje_Excencion.Text.Trim();
            if (!String.IsNullOrEmpty(Txt_Cuota_Anual.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual] = Txt_Cuota_Anual.Text.Trim();
            if (!String.IsNullOrEmpty(Txt_Fecha_Inicial.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0]["TERMINO_EXENCION"] = Convert.ToDateTime(Txt_Fecha_Inicial.Text.Trim()).ToString("dd/MMM/yyyy");
            if (!String.IsNullOrEmpty(Txt_Fecha_Avaluo.Text.Trim()))
                Ds_Actual.Tables[0].Rows[0]["FECHA_AVALUO"] = Convert.ToDateTime(Txt_Fecha_Avaluo.Text.Trim()).ToString("dd/MMM/yyyy");
            if (!Chk_Cuota_Fija.Checked)
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] = "NO";
            if (Chk_Cuota_Fija.Checked)
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija] = "SI";
            if (!String.IsNullOrEmpty(Hdn_Cuota_Minima.Value))
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Minima_ID] = Hdn_Cuota_Minima.Value;
            if (!Chk_Cuota_Fija.Checked)
                Ds_Actual.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija] = "";

            if (String.IsNullOrEmpty(Hdn_Cuenta_ID.Value))
            {
                Revisar_Actualizaciones(Ds_Actual);
            }
            else
            {
                Revisar_Actualizaciones((DataSet)Session["Ds_Cuenta_Datos_Orden"], Ds_Actual);
            }
            Session["Ds_Cuenta_Datos_Orden"] = Ds_Actual;
        }
    }
    #endregion
    //Metodos de Validacion de campos
    #region Validaciones
    //******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Acceso
    ///DESCRIPCIÓN: se validan los datos necesarios en la generacion de orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 09/17/2011 02:43:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Validar_Acceso()
    {
        if (Session["Opcion_Tipo_Orden"] != null)
        {
            if (Session["Opcion_Tipo_Orden"].ToString().Contains("Traslado"))
            {
                if (Session["Estatus_Cuenta"].ToString() == "BLOQUEADA" || (Session["Estatus_Cuenta"].ToString() == "SUSPENDIDA" && Session["Tipo_Suspencion"].ToString() != "PREDIAL"))
                {
                    Mensaje_Error("Esta Cuenta no tiene Autorizados movientos para Traslado");
                    Estado_Botones(Const_Estado_Inicial);
                    Btn_Resumen_Cuenta.Enabled = true;
                }
            }
            else if (Session["Opcion_Tipo_Orden"].ToString().Contains("Predial"))
            {
                if (Session["Estatus_Cuenta"].ToString() == "SUSPENDIDA" && Session["Tipo_Suspencion"].ToString() != "TRASLADO")
                {
                    Mensaje_Error("Esta Cuenta no tiene Autorizados movientos para Predial");
                    Estado_Botones(Const_Estado_Inicial);
                    Btn_Resumen_Cuenta.Enabled = true;
                }
            }
        }
        else
        {
            Mensaje_Error("No se pudo Cargar la Configuracion de la Página");
            Estado_Botones(Const_Estado_Inicial);
        }

        if (Hdn_Cargar_Modulos.Value == "CANCELACION_CUENTAS" || Hdn_Cargar_Modulos.Value == "BAJAS_DIRECTAS" || Hdn_Cargar_Modulos.Value == "REACTIVACION_CUENTAS")
        {
            Mensaje_Error("La orden de variacion no es valida para este formulario, es de tipo: " + Hdn_Cargar_Modulos.Value);
            Estado_Botones(Const_Estado_Inicial);
        }

    }
    //******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Generar_Orden
    ///DESCRIPCIÓN: se validan los datos necesarios en la generacion de orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 09/17/2011 02:43:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Validar_Generar_Orden()
    {
        Boolean Resultado = true;
        try
        {
            if (Session["P_Generar_Orden_Dt_Detalles"] == null)
            {
                Mensaje_Error("+ Ocurrio un error al cargar los datos de la variación a aplicar");
                Resultado = false;
            }
            if (Session["M_Orden_Negocio"] == null)
            {
                Mensaje_Error("+ Ocurrio un error al cargar los datos del objeto de la variación a aplicar");
                Resultado = false;
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Orden_Variacion
    ///DESCRIPCIÓN: se validan los datos necesarios en la generacion de orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 09/17/2011 02:43:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Validar_Orden_Variacion()
    {
        Boolean Resultado = true;
        try
        {
            if (!Chk_Mismo_Domicilio.Checked)
            {
                if (Cmb_Domicilio_Foraneo.SelectedIndex <= 0)
                {
                    Mensaje_Error("+ Especificar si es Mismo Domicilio o Domicilio Foraneo");
                    Resultado = false;
                }
            }
            if (Chk_Cuota_Fija.Checked)
            {
                if (Cmb_Solicitante.SelectedIndex <= 0 && Cmb_Financiado.SelectedIndex <= 0)
                {
                    Mensaje_Error("+ Seleccionar un Beneficio");
                    Resultado = false;
                }
            }
            if (String.IsNullOrEmpty(Txt_Nombre_Propietario.Text.Trim()))
            {
                Mensaje_Error("+ Seleccionar un Propietario o Poseedor");
                Resultado = false;
            }
            if (Cmb_Tipos_Propietario.SelectedValue.ToString() == "0")
            {
                Mensaje_Error("+ Seleccionar Tipo de Propietario");
                Resultado = false;
            }
            if (Cmb_Tipos_Predio.SelectedValue.ToString() == "0")
            {
                Mensaje_Error("+ Seleccionar Tipo de Predio");
                Resultado = false;
            }
            if (Cmb_Estados_Predio.SelectedValue.ToString() == "0")
            {
                Mensaje_Error("+ Seleccionar Estado de Predio");
                Resultado = false;
            }
            if (Cmb_Usos_Predio.SelectedValue.ToString() == "0")
            {
                Mensaje_Error("+ Seleccionar Uso de Predio");
                Resultado = false;
            }
            else if (Cmb_Usos_Predio.SelectedItem.Text.Contains("-BAJA"))
            {
                Mensaje_Error("+ El Uso de Predio seleccionado esta dado de baja");
                Resultado = false;
            }
            if (String.IsNullOrEmpty(Txt_Colonia_Cuenta.Text.Trim()))
            {
                Mensaje_Error("+ Seleccionar Colonia del predio");
                Resultado = false;
            }
            if (String.IsNullOrEmpty(Txt_Superficie_Total.Text.Trim()) || Txt_Superficie_Total.Text.Trim() == "0.00")
            {
                Mensaje_Error("+ Seleccionar Superficie Total del predio");
                Resultado = false;
            }
            if (String.IsNullOrEmpty(Txt_Valor_Fiscal.Text.Trim()) || Txt_Valor_Fiscal.Text.Trim() == "0.00")
            {
                Mensaje_Error("+ Seleccionar Valor Fiscal del predio");
                Resultado = false;
            }
            if (String.IsNullOrEmpty(Hdn_Tasa_General_ID.Value) || String.IsNullOrEmpty(Hdn_Tasa_ID.Value))
            {
                Mensaje_Error("+ Seleccionar Tasa del predio");
                Resultado = false;
            }
            if (String.IsNullOrEmpty(Hdn_Propietario_ID.Value) && String.IsNullOrEmpty(Txt_Nombre_Propietario.Text.Trim()))
            {
                Mensaje_Error("+ Seleccionar Propietario");
                Resultado = false;
            }
            if (Cmb_Efectos.SelectedValue.ToString() == "0" || Cmb_Efectos_Numero.SelectedValue.ToString() == "0")
            {
                Mensaje_Error("+ Seleccionar los efectos");
                Resultado = false;
            }

        }

        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        return Resultado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Diferencia_Regazo
    ///DESCRIPCIÓN: se validan los combos de agregacion de un rezago
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/09/2011 04:45:21 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Validar_Diferencia_Regazo()
    {
        Boolean Resultado = true;

        if (Cmb_P_R_Anio_Inicial.SelectedIndex == Cmb_P_R_Anio_Final.SelectedIndex)
        {
            if (Cmb_P_R_Bimestre_Inicial.SelectedIndex > Cmb_P_R_Bimestre_Final.SelectedIndex)
            {
                Resultado = false;
            }
        }
        else
        {
            if (Cmb_P_R_Anio_Inicial.SelectedIndex < Cmb_P_R_Anio_Final.SelectedIndex)
            {
                Resultado = false;
            }
        }

        return Resultado;

    }
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
    ///MODIFICO: Jesus Toledo
    ///FECHA_MODIFICO: 23-Nov-2011                   
    ///CAUSA_MODIFICACIÓN:Se agregan todos los campos al DS final
    ///*******************************************************************************
    public void Revisar_Actualizaciones(DataSet Ds_Anterior, DataSet Ds_Actual)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();

        String Dato1 = "";
        String Dato2 = "";
        for (int i = 0; i < Ds_Actual.Tables[0].Columns.Count; i++)
        {
            Dato1 = Ds_Anterior.Tables[0].Rows[0].ItemArray[i].ToString();
            Dato2 = Ds_Actual.Tables[0].Rows[0].ItemArray[i].ToString();
            //Se comento para que aceptara todos lo campos en el DS final
            //if (Dato1 != Dato2)
            //{
            M_Orden_Negocio.Agregar_Variacion(Ds_Actual.Tables[0].Columns[i].ToString(), Dato2);
            //}
        }
        Session["P_Generar_Orden_Dt_Detalles"] = M_Orden_Negocio.P_Generar_Orden_Dt_Detalles;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Revisar_Actualizaciones
    ///DESCRIPCIÓN: realiza la insercion de una variacion si el dato es difrente de nulo
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/30/2011 05:57:18 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    public void Revisar_Actualizaciones(DataSet Ds_Actual)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        String Dato2 = "";
        for (int i = 0; i < Ds_Actual.Tables[0].Columns.Count; i++)
        {
            Dato2 = Ds_Actual.Tables[0].Rows[0].ItemArray[i].ToString();
            if (!String.IsNullOrEmpty(Dato2))
            {
                M_Orden_Negocio.Agregar_Variacion(Ds_Actual.Tables[0].Columns[i].ToString(), Dato2);
            }
        }
        Session["P_Generar_Orden_Dt_Detalles"] = M_Orden_Negocio.P_Generar_Orden_Dt_Detalles;
    }
    #endregion
    //Metodos para Realizar el Reporte
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
        DataRow Renglon;
        ReportDocument Reporte = new ReportDocument();
        String Ruta_Reporte_Crystal = "";
        String Nombre_Reporte_Generar = "";
        DataTable Dt_Diferencias_Header = new DataTable();
        DataTable Dt_Propietarios = new DataTable();
        DataSet Ds_Reporte = null;
        if (Ds_Reporte_Ordenes_Salida.Tables.Count > 1)
        {
            Ds_Reporte = new DataSet();
            Ds_Reporte.Tables.Add(Ds_Reporte_Ordenes_Salida.Tables[0].Copy());
            Ds_Reporte.Tables[0].TableName = "Dt_Generales";
        }
        else
        {
            Ds_Reporte = Ds_Reporte_Ordenes_Salida.Copy();
        }
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Negocio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Cat_Pre_Uso_Suelo_Negocio Uso_Negocio = new Cls_Cat_Pre_Uso_Suelo_Negocio();
        Cls_Cat_Pre_Estados_Predio_Negocio Estado_Negocio = new Cls_Cat_Pre_Estados_Predio_Negocio();
        Cls_Cat_Pre_Tipos_Predio_Negocio Tipos_Predio = new Cls_Cat_Pre_Tipos_Predio_Negocio();
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Consultar = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Agregar_Co_Propietarios = (DataTable)Session["Dt_Agregar_Co_Propietarios"];
        if (Session["Dt_Agregar_Diferencias"] == null)
            Limpiar_Session_Agregar_Diferencias();
        DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        DataTable Dt_Cuota_Fija = new DataTable();

        try
        {
            Ds_Reporte.Tables[0].Columns.Add("NOMBRE_USO_SUELO");
            Ds_Reporte.Tables[0].Columns.Add("NOMBRE_TIPO_PREDIO");
            Ds_Reporte.Tables[0].Columns.Add("NOMBRE_ESTADO_PREDIO");
            Ds_Reporte.Tables[0].Columns.Add("ULTIMO_MOVIMIENTO");
            Ds_Reporte.Tables[0].Columns.Add("CUOTA_BIMESTRAL");
            Ds_Reporte.Tables[0].Columns.Add("NO_ORDEN_VARIACION");
            Ds_Reporte.Tables[0].Columns.Add("TASA_VALOR");
            Ds_Reporte.Tables[0].Columns.Add("COMENTARIOS");

            if (!String.IsNullOrEmpty(Hdn_Orden_Variacion.Value))
            {
                Ds_Reporte.Tables[0].Rows[0]["NO_ORDEN_VARIACION"] = Hdn_Orden_Variacion.Value + "/" + Const_Anio_Corriente.ToString();

                //Resumen_Negocio.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
                //DataTable Dt_Ult_Mov = Resumen_Negocio.Consultar_Ultimo_Movimiento();
                //if (Dt_Ult_Mov.Rows.Count > 0)
                //    Ds_Reporte.Tables[0].Rows[0]["ULTIMO_MOVIMIENTO"] = Resumen_Negocio.Consultar_Ultimo_Movimiento().Rows[0][0];
                //else
                Ds_Reporte.Tables[0].Rows[0]["ULTIMO_MOVIMIENTO"] = Cmb_Tipos_Movimiento.SelectedItem.Text;
                if (!String.IsNullOrEmpty(Ds_Reporte.Tables[0].Rows[0]["CUOTA_ANUAL"].ToString()))
                    Ds_Reporte.Tables[0].Rows[0]["CUOTA_BIMESTRAL"] = (Convert.ToDouble(Ds_Reporte.Tables[0].Rows[0]["CUOTA_ANUAL"].ToString()) / 6).ToString("#,###,##0.00");
                else
                    Ds_Reporte.Tables[0].Rows[0]["CUOTA_BIMESTRAL"] = "-";
                if (!String.IsNullOrEmpty(Ds_Reporte.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString()))
                {
                    Uso_Negocio.P_Filtros_Dinamicos = Cat_Pre_Uso_Suelo.Campo_Uso_Suelo_ID + " = '" + Ds_Reporte.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() + "' ";
                    Ds_Reporte.Tables[0].Rows[0]["NOMBRE_USO_SUELO"] = Uso_Negocio.Consultar_Uso_Suelo().Rows[0][3].ToString();
                }
                else
                    Ds_Reporte.Tables[0].Rows[0]["NOMBRE_USO_SUELO"] = "-";
                if (!String.IsNullOrEmpty(Ds_Reporte.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString()))
                {
                    Estado_Negocio.P_Filtros_Dinamicos = Cat_Pre_Estados_Predio.Campo_Estado_Predio_ID + " = '" + Ds_Reporte.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString() + "' ";
                    Ds_Reporte.Tables[0].Rows[0]["NOMBRE_ESTADO_PREDIO"] = Estado_Negocio.Consultar_Estado_Predio().Rows[0][1].ToString();
                }
                else
                    Ds_Reporte.Tables[0].Rows[0]["NOMBRE_ESTADO_PREDIO"] = "-";
                if (!String.IsNullOrEmpty(Ds_Reporte.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString()))
                {
                    Tipos_Predio.P_Filtros_Dinamicos = Cat_Pre_Tipos_Predio.Campo_Tipo_Predio_ID + " = '" + Ds_Reporte.Tables[0].Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID].ToString() + "' ";
                    Ds_Reporte.Tables[0].Rows[0]["NOMBRE_TIPO_PREDIO"] = Tipos_Predio.Consultar_Tipo_Predio().Rows[0][1].ToString();
                }
                else
                    Ds_Reporte.Tables[0].Rows[0]["NOMBRE_TIPO_PREDIO"] = "-";
                Ds_Reporte.Tables[0].Rows[0]["USUARIO_CREO"] = Presidencia.Sessiones.Cls_Sessiones.Nombre_Empleado;
                Ds_Reporte.Tables[0].Rows[0]["FECHA_CREO"] = Session["FECHA_CREO"].ToString();
                if (!String.IsNullOrEmpty(Ds_Reporte.Tables[0].Rows[0]["TASA_ID"].ToString()))
                    Ds_Reporte.Tables[0].Rows[0]["TASA_VALOR"] = Orden_Consultar.Consultar_Valor_Tasa(Ds_Reporte.Tables[0].Rows[0]["TASA_ID"].ToString());
                else
                    Ds_Reporte.Tables[0].Rows[0]["TASA_VALOR"] = "-";

                Ds_Reporte.Tables[0].Rows[0]["COMENTARIOS"] = Txt_Comentarios.Text.Trim();

                Ds_Reporte.Tables[0].Rows[0]["NOMBRE_CALLE"] = Txt_Calle_Cuenta.Text.Trim();
                Ds_Reporte.Tables[0].Rows[0]["NOMBRE_COLONIA"] = Txt_Colonia_Cuenta.Text.Trim();
                Ds_Reporte.Tables[0].Rows[0]["COMENTARIOS"] = Txt_Observaciones_Cuenta.Text.Trim().ToUpper();
                Ds_Reporte.Tables[0].Rows[0]["ESTATUS"] = Cmb_Estatus.SelectedItem.Text.Trim();
                //if (String.IsNullOrEmpty(Ds_Reporte.Tables[0].Rows[0]["FECHA_AVALUO_FORMATEADA"].ToString()) || Ds_Reporte.Tables[0].Rows[0]["FECHA_AVALUO_FORMATEADA"].ToString() == "01/Ene/0001")
                //    Ds_Reporte.Tables[0].Rows[0]["FECHA_AVALUO_FORMATEADA"] = "";
                //else
                //{
                if (!string.IsNullOrEmpty(Txt_Fecha_Avaluo.Text.Trim()))
                    Ds_Reporte.Tables[0].Rows[0]["FECHA_AVALUO_FORMATEADA"] = Txt_Fecha_Avaluo.Text.Trim();
                else
                    Ds_Reporte.Tables[0].Rows[0]["FECHA_AVALUO_FORMATEADA"] = " -";
                //}//Formatear_Fecha(Ds_Reporte.Tables[0].Rows[0]["FECHA_AVALUO_FORMATEADA"].ToString(), true);
                //if (String.IsNullOrEmpty(Ds_Reporte.Tables[0].Rows[0]["TERMINO_EXENCION_FORMATEADA"].ToString()) || Ds_Reporte.Tables[0].Rows[0]["TERMINO_EXENCION_FORMATEADA"].ToString() == "01/Ene/0001")
                //    Ds_Reporte.Tables[0].Rows[0]["TERMINO_EXENCION_FORMATEADA"] = "";
                //else
                //{
                if (!string.IsNullOrEmpty(Txt_Fecha_Inicial.Text.Trim()))
                    Ds_Reporte.Tables[0].Rows[0]["TERMINO_EXENCION_FORMATEADA"] = Txt_Fecha_Inicial.Text.Trim();//Formatear_Fecha(Ds_Reporte.Tables[0].Rows[0]["TERMINO_EXENCION_FORMATEADA"].ToString(), true);
                else
                    Ds_Reporte.Tables[0].Rows[0]["TERMINO_EXENCION_FORMATEADA"] = " -";//Formatear_Fecha(Ds_Reporte.Tables[0].Rows[0]["TERMINO_EXENCION_FORMATEADA"].ToString(), true);

                //}

                Dt_Propietarios.Columns.Clear();
                Dt_Propietarios.Columns.Add("NOMBRE_PROPIETARIO");
                Dt_Propietarios.Columns.Add("TIPO_PROPIETARIO");
                Dt_Propietarios.Columns.Add("RFC");
                Dt_Propietarios.Columns.Add("NOMBRE_CALLE");
                Dt_Propietarios.Columns.Add("NOMBRE_COLONIA");
                Dt_Propietarios.Columns.Add("NOMBRE_ESTADO");
                Dt_Propietarios.Columns.Add("NOMBRE_CIUDAD");
                Dt_Propietarios.Columns.Add("INTERIOR");
                Dt_Propietarios.Columns.Add("EXTERIOR");
                DataRow Dr_Prop;
                Dr_Prop = Dt_Propietarios.NewRow();
                if (Chk_Mismo_Domicilio.Checked)
                {
                    Dr_Prop["NOMBRE_CALLE"] = Txt_Calle_Cuenta.Text.Trim().ToUpper(); ;
                    Dr_Prop["NOMBRE_COLONIA"] = Txt_Colonia_Cuenta.Text.Trim().ToUpper(); ;
                    Dr_Prop["NOMBRE_ESTADO"] = "GUANAJUATO";
                    Dr_Prop["NOMBRE_CIUDAD"] = "IRAPUATO";
                    Dr_Prop["INTERIOR"] = Txt_No_Interior.Text.Trim().ToUpper();
                    Dr_Prop["EXTERIOR"] = Txt_No_Exterior.Text.Trim().ToUpper();
                }
                else
                {
                    if (Cmb_Domicilio_Foraneo.SelectedItem.Text == "SI")
                    {
                        Dr_Prop["NOMBRE_CALLE"] = Txt_Calle_Propietario.Text.Trim().ToUpper();
                        Dr_Prop["NOMBRE_COLONIA"] = Txt_Colonia_Propietario.Text.Trim().ToUpper();
                        Dr_Prop["NOMBRE_ESTADO"] = Txt_Estado.Text.Trim().ToUpper();
                        Dr_Prop["NOMBRE_CIUDAD"] = Txt_Ciudad.Text.Trim().ToUpper(); ;
                        Dr_Prop["INTERIOR"] = Txt_Numero_Interior_Propietario.Text.Trim().ToUpper();
                        Dr_Prop["EXTERIOR"] = Txt_Numero_Exterior_Propietario.Text.Trim().ToUpper();
                    }
                    if (Cmb_Domicilio_Foraneo.SelectedItem.Text == "NO")
                    {
                        Dr_Prop["NOMBRE_CALLE"] = Txt_Calle_Propietario.Text.Trim().ToUpper();
                        Dr_Prop["NOMBRE_COLONIA"] = Txt_Colonia_Propietario.Text.Trim().ToUpper(); ;
                        Dr_Prop["NOMBRE_ESTADO"] = "GUANAJUATO";
                        Dr_Prop["NOMBRE_CIUDAD"] = "IRAPUATO";
                        Dr_Prop["INTERIOR"] = Txt_Numero_Interior_Propietario.Text.Trim().ToUpper();
                        Dr_Prop["EXTERIOR"] = Txt_Numero_Exterior_Propietario.Text.Trim().ToUpper();
                    }
                }

                Dr_Prop["NOMBRE_PROPIETARIO"] = Txt_Nombre_Propietario.Text.Trim();
                Dr_Prop["TIPO_PROPIETARIO"] = Cmb_Tipos_Propietario.SelectedItem.Text.Trim();
                Dr_Prop["RFC"] = Txt_Rfc_Propietario.Text.Trim();

                Dt_Propietarios.Rows.Add(Dr_Prop);

                Dt_Diferencias_Header.Columns.Clear();
                Dt_Diferencias_Header.Columns.Add("PERIODO_REZAGO");
                Dt_Diferencias_Header.Columns.Add("PERIODO_CORRIENTE");
                Dt_Diferencias_Header.Columns.Add("ALTA_REZAGO");
                Dt_Diferencias_Header.Columns.Add("BAJA_REZAGO");
                Dt_Diferencias_Header.Columns.Add("ALTA_CORRIENTE");
                Dt_Diferencias_Header.Columns.Add("BAJA_CORRIENTE");
                Dt_Diferencias_Header.Columns.Add("NO_DIFERENCIA");
                DataRow Dr_Diferencias_Hdr;
                Dr_Diferencias_Hdr = Dt_Diferencias_Header.NewRow();
                Dr_Diferencias_Hdr["PERIODO_REZAGO"] = Txt_Desde_Periodo_Regazo.Text.Trim() + "/" + Lbl_P_C_Anio_Inicio.Text.Trim() + " - " + Txt_Hasta_Periodo_Regazo.Text.Trim() + "/" + Lbl_P_C_Anio_Final.Text.Trim();
                Dr_Diferencias_Hdr["PERIODO_CORRIENTE"] = Txt_Desde_Periodo_Corriente.Text.Trim() + "/" + Txt_Desde_Anio_Periodo_Corriente.Text.Trim() + " - " + Txt_Hasta_Periodo_Corriente.Text.Trim() + "/" + Txt_Desde_Anio_Periodo_Corriente.Text.Trim();
                Dr_Diferencias_Hdr["ALTA_REZAGO"] = Txt_Alta_Periodo_Regazo.Text.Trim();
                Dr_Diferencias_Hdr["BAJA_REZAGO"] = Txt_Baja_Periodo_Regazo.Text.Trim();
                Dr_Diferencias_Hdr["ALTA_CORRIENTE"] = Txt_Alta_Periodo_Corriente.Text.Trim();
                Dr_Diferencias_Hdr["BAJA_CORRIENTE"] = Txt_Baja_Periodo_Corriente.Text.Trim();
                Dr_Diferencias_Hdr["NO_DIFERENCIA"] = "1";
                Dt_Diferencias_Header.Rows.Add(Dr_Diferencias_Hdr);
                Ds_Reporte.Tables.Add(Dt_Diferencias_Header.Copy());
                Ds_Reporte.Tables[1].TableName = "Dt_Diferencias_Header";
                //if (Dt_Agregar_Diferencias.Rows.Count <= 0)
                //{
                //    DataRow DrDifs = Dt_Agregar_Diferencias.NewRow();
                //    for (int cont = 0; cont <= Dt_Agregar_Diferencias.Columns.Count - 1; cont++)
                //    {
                //        //if (Dt_Agregar_Diferencias.Columns[cont].Namespace == "s")
                //        DrDifs[cont] = DBNull.Value;
                //    }
                //    Dt_Agregar_Diferencias.Rows.Add(DrDifs);

                //}
                //Lineas para ordena tabla
                if (Dt_Agregar_Diferencias != null)
                {
                    if (Dt_Agregar_Diferencias.Rows.Count > 0)
                        Dt_Agregar_Diferencias = Acomodar_Periodos(Dt_Agregar_Diferencias);

                    Ds_Reporte.Tables.Add(Dt_Agregar_Diferencias.Copy());
                    Ds_Reporte.Tables[2].TableName = "Dt_Diferencias";
                }
                else
                {
                    Ds_Reporte.Tables.Add(new DataTable());
                    Ds_Reporte.Tables[2].TableName = "Dt_Diferencias";
                }
                //for (int cont = 0; cont < Ds_Reporte.Tables["Dt_Diferencias"].Rows.Count; cont++)
                //{
                //    Ds_Reporte.Tables["Dt_Diferencias"].Rows[cont]["NO_DIFERENCIA"] = "1";
                //}
                Ds_Reporte.Tables.Add(Dt_Propietarios.Copy());
                Ds_Reporte.Tables[3].TableName = "Dt_Propietarios";
                //Se agrega el DT de los copropietarios Agregados
                if (Dt_Agregar_Co_Propietarios.Rows.Count <= 0)
                {
                    DataRow Dr_Coprop = Dt_Agregar_Co_Propietarios.NewRow();
                    for (int cont = 0; cont <= Dt_Agregar_Co_Propietarios.Columns.Count - 1; cont++)
                    {
                        Dr_Coprop[0] = "";
                        break;
                    }
                    Dt_Agregar_Co_Propietarios.Rows.Add(Dr_Coprop);
                }
                Ds_Reporte.Tables.Add(Dt_Agregar_Co_Propietarios.Copy());
                Ds_Reporte.Tables[4].TableName = "Dt_Copropietarios";

                Dt_Cuota_Fija.Columns.Clear();
                Dt_Cuota_Fija.Columns.Add("NO_CUOTA_FIJA");
                Dt_Cuota_Fija.Columns.Add("DESCRIPCION");
                Dt_Cuota_Fija.Columns.Add("FUNDAMENTO");
                DataRow Dr_Cuota_Fija;
                Dr_Cuota_Fija = Dt_Cuota_Fija.NewRow();
                Dr_Cuota_Fija["NO_CUOTA_FIJA"] = "0";
                if (Cmb_Financiado.SelectedIndex > 0 && Ds_Reporte.Tables[0].Rows[0]["CUOTA_FIJA"].ToString() == "SI")
                    Dr_Cuota_Fija["DESCRIPCION"] = Cmb_Financiado.SelectedItem.Text.Trim();
                else if (Cmb_Solicitante.SelectedIndex > 0 && Ds_Reporte.Tables[0].Rows[0]["CUOTA_FIJA"].ToString() == "SI")
                    Dr_Cuota_Fija["DESCRIPCION"] = Cmb_Solicitante.SelectedItem.Text.Trim();
                else
                    Dr_Cuota_Fija["DESCRIPCION"] = "";
                Dr_Cuota_Fija["FUNDAMENTO"] = Txt_Fundamento.Text.Trim();
                Dt_Cuota_Fija.Rows.Add(Dr_Cuota_Fija);
                Ds_Reporte.Tables.Add(Dt_Cuota_Fija.Copy());
                Ds_Reporte.Tables[5].TableName = "Dt_Cuota_Fija";
                for (int cont = Ds_Reporte.Tables[0].Rows.Count; cont > 1; cont--)
                {
                    if (cont > 1)
                        Ds_Reporte.Tables[0].Rows[cont - 1].Delete();
                    Ds_Reporte.AcceptChanges();
                }
                // Ruta donde se encuentra el reporte CrystalToString("
                Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Ordenes_Variacion.rpt";

                // Se crea el nombre del reporte
                String Nombre_Reporte = "Orden_Variacion_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

                Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
                Cls_Reportes Reportes = new Cls_Reportes();
                Generar_Reporte(ref Ds_Reporte, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
                Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error("Ocurrio un error al Generar Reporte. Favor de Reimprir Orden No. " + Convert.ToInt32(Hdn_Orden_Variacion.Value).ToString() + " [" + ex.Message + "]");
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Acomodar_Periodos
    ///DESCRIPCIÓN:          Ordena el data table de las diferencias los periodos para que al agregar otro al imprimir este salga ordenado
    ///PARAMETROS:           1.-DataTable Dt_Datos.- Contiene la informacion de la tabla a ordenar
    ///CREO:                 Jacqueline Ramírez Sierra
    ///FECHA_CREO:           18/Noviembre/2011
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public DataTable Acomodar_Periodos(DataTable Dt_Datos)
    {
        Int32 Bim_1 = 0;
        Int32 Anio_1 = 0;
        Int32 Bim_2 = 0;
        Int32 Anio_2 = 0;

        for (Int32 Contador = 0; Contador < Dt_Datos.Rows.Count; Contador++)
        {
            Bim_1 = Convert.ToInt32(Dt_Datos.Rows[Contador]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[0].ToString());
            Anio_1 = Convert.ToInt32(Dt_Datos.Rows[Contador]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[1].ToString());
            for (Int32 Contador_2 = (Contador + 1); Contador_2 < Dt_Datos.Rows.Count; Contador_2++)
            {
                Bim_2 = Convert.ToInt32(Dt_Datos.Rows[Contador_2]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[0].ToString());
                Anio_2 = Convert.ToInt32(Dt_Datos.Rows[Contador_2]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[1].ToString());
                if (Anio_2 < Anio_1)
                {
                    Object[] obj1 = Dt_Datos.Rows[Contador].ItemArray;
                    Object[] obj2 = Dt_Datos.Rows[Contador_2].ItemArray;
                    //Dt_Datos.Rows.RemoveAt(Contador_2);
                    //Dt_Datos.Rows.RemoveAt(Contador);
                    Dt_Datos.Rows[Contador_2].ItemArray = obj1;
                    Dt_Datos.Rows[Contador].ItemArray = obj2;
                    Bim_1 = Convert.ToInt32(Dt_Datos.Rows[Contador]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[0].ToString());
                    Anio_1 = Convert.ToInt32(Dt_Datos.Rows[Contador]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[1].ToString());
                }
                else if (Anio_2 == Anio_1)
                {
                    if (Bim_2 < Bim_1)
                    {
                        Object[] obj1 = Dt_Datos.Rows[Contador].ItemArray;
                        Object[] obj2 = Dt_Datos.Rows[Contador_2].ItemArray;
                        //Dt_Datos.Rows.RemoveAt(Contador_2);
                        //Dt_Datos.Rows.RemoveAt(Contador);
                        Dt_Datos.Rows[Contador_2].ItemArray = obj1;
                        Dt_Datos.Rows[Contador].ItemArray = obj2;
                        Bim_1 = Convert.ToInt32(Dt_Datos.Rows[Contador]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[0].ToString());
                        Anio_1 = Convert.ToInt32(Dt_Datos.Rows[Contador]["PERIODO"].ToString().Split('-')[1].ToString().Split('/')[1].ToString());
                    }
                }
            }
        }
        return Dt_Datos;

    }

    /// *************************************************************************************
    /// NOMBRE:             Generar_Reporte
    /// DESCRIPCIÓN:        Método que invoca la generación del reporte.
    ///              
    /// PARÁMETROS:         Ds_Reporte_Crystal.- Es el DataSet con el que se muestra el reporte en cristal 
    ///                     Ruta_Reporte_Crystal.-  Ruta y Nombre del archivo del Crystal Report.
    ///                     Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    ///                     Formato.- Es el tipo de reporte "PDF", "Excel"
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:15 p.m.
    /// USUARIO MODIFICO:   Salvador Henrnandez Ramirez
    /// FECHA MODIFICO:     16/Mayo/2011
    /// CAUSA MODIFICACIÓN: Se cambio Nombre_Plantilla_Reporte por Ruta_Reporte_Crystal, ya que este contendrá tambien la ruta
    ///                     y se asigno la opción para que se tenga acceso al método que muestra el reporte en Excel.
    /// *************************************************************************************
    public void Generar_Reporte(ref DataSet Ds_Reporte_Crystal, String Ruta_Reporte_Crystal, String Nombre_Reporte_Generar, String Formato)
    {
        ReportDocument Reporte = new ReportDocument(); // Variable de tipo reporte.
        String Ruta = String.Empty;  // Variable que almacenará la ruta del archivo del crystal report. 

        try
        {
            Ruta = HttpContext.Current.Server.MapPath(Ruta_Reporte_Crystal);
            Reporte.Load(Ruta);

            if (Ds_Reporte_Crystal is DataSet)
            {
                if (Ds_Reporte_Crystal.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Reporte_Crystal);
                    if (Ds_Reporte_Crystal.Tables["Dt_Copropietarios"] != null)
                        Reporte.Subreports["CO_PROPIETARIOS"].SetDataSource(Ds_Reporte_Crystal.Tables["Dt_Copropietarios"]);
                    if (Ds_Reporte_Crystal.Tables["Dt_Diferencias"] != null)
                        Reporte.Subreports["Diferencias"].SetDataSource(Ds_Reporte_Crystal.Tables["Dt_Diferencias"]);
                    if (Formato == "PDF")
                    {
                        Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al generar el reporte. Error: [" + Ex.Message + "]");
        }
    }
    /// *************************************************************************************
    /// NOMBRE:             Exportar_Reporte_PDF
    /// DESCRIPCIÓN:        Método que guarda el reporte generado en formato PDF en la ruta
    ///                     especificada.
    /// PARÁMETROS:         Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                     Nombre_Reporte.- Nombre que se le dio al reporte.
    /// USUARIO CREO:       Juan Alberto Hernández Negrete.
    /// FECHA CREO:         3/Mayo/2011 18:19 p.m.
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    public void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte_Generar)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = HttpContext.Current.Server.MapPath("../../Reporte/" + Nombre_Reporte_Generar);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al exportar el reporte. Error: [" + Ex.Message + "]");
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
    #endregion
    //Eventos
    #region Eventos
    //Eventos Combos
    #region Eventos Combos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Diferencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Calcular montos de grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Cmb_Tipo_Diferencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            //Calcular_Analisis_Rezago("Cmb_Tipo_Diferencias_SelectedIndexChanged");
            //Txt_Grid_Dif_Valor_Fiscal_TextChanged(null, null);
            for (Int32 Cont = 0; Cont < Grid_Diferencias.Rows.Count; Cont++)
            {
                DropDownList Cmb_Tipo_Dif_Guardar = (DropDownList)Grid_Diferencias.Rows[Cont].Cells[1].FindControl("Cmb_Tipo_Diferencias");
                Dt_Agregar_Diferencias.Rows[Cont]["TIPO"] = Cmb_Tipo_Dif_Guardar.SelectedValue.ToString();
                Session["Dt_Agregar_Diferencias"] = Dt_Agregar_Diferencias;
            }
            Resumen_Grid_Diferencias();
            Calcular_Resumen();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipos_Propietario_SelectedIndexChanged
    ///DESCRIPCIÓN: Sleccionar tipo de propietario
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Cmb_Tipos_Propietario_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Contribuyentes = (DataTable)Session["Dt_Contribuyentes"];
        try
        {
            if (Cmb_Tipos_Propietario.SelectedValue.ToString() != "0")
            {
                for (int cont = 0; cont < Dt_Contribuyentes.Rows.Count; cont++)
                {
                    if (Dt_Contribuyentes.Rows[cont][Ope_Pre_Copropietarios_Orde_Variacion.Campo_Contribuyente_ID].ToString() == Hdn_Propietario_ID.Value)
                    {
                        if (Dt_Contribuyentes.Rows[cont][Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo].ToString() != "COPROPIETARIO")
                        {
                            Dt_Contribuyentes.Rows[cont][Ope_Pre_Copropietarios_Orde_Variacion.Campo_Tipo] = Cmb_Tipos_Propietario.SelectedValue.ToString();
                            Session["Dt_Contribuyentes"] = Dt_Contribuyentes;
                            break;
                        }
                    }
                }
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Financiado_SelectedIndexChanged
    ///DESCRIPCIÓN: Sleccionar Modo de financiamiento
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Cmb_Financiado_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            Cargar_Couta_Minima();
            if (Cmb_Financiado.SelectedIndex > 0)
            {
                Txt_Fundamento.Text = "";
                Txt_Fundamento.Text = Orden_Negocio.Consulta_Fundamento(Cmb_Financiado.SelectedValue.ToString());
                Cmb_Solicitante.SelectedIndex = 0;
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Domicilio_Foraneo_SelectedIndexChanged
    ///DESCRIPCIÓN: Habilitar Ciudad y Estado
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 11/11/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Cmb_Domicilio_Foraneo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            //Nothing
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cmb_Solicitante_SelectedIndexChanged
    ///DESCRIPCIÓN: Seleccionar Modo de solicitante
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Cmb_Solicitante_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            Cargar_Couta_Minima();
            if (Cmb_Solicitante.SelectedIndex > 0)
            {
                Txt_Fundamento.Text = "";
                Txt_Fundamento.Text = Orden_Negocio.Consulta_Fundamento(Cmb_Solicitante.SelectedValue.ToString());
                Cmb_Financiado.SelectedIndex = 0;
                Quitar_Porcentaje_Anualidad();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Quitar_Porcentaje_Anualidad
    ///DESCRIPCIÓN: Afectar la anualidad a pagar por asociasions civiles y escuelas
    ///PARAMETROS: 
    ///CREO: Jesus S. Toledo Rodriguez
    ///FECHA_CREO: 23/Dic/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    private void Quitar_Porcentaje_Anualidad()
    {
        string Solicitante = null;
        int Periodo_Corriente = 1;
        double Anualidad = 0.00;
        double Bimestral = 0.00;
        double Anualidad_Original = 0.00;
        double Porcentaje = 0.25;
        DataTable Dt_Adeudos_Predial;
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        try
        {
            if (Cmb_Solicitante.SelectedItem != null)
            {
                Solicitante = Cmb_Solicitante.SelectedItem.Text.Trim().ToUpper();
                if (Solicitante.Contains("ESCUELAS"))
                {
                    //Se toma el 25 porciento del adeudo para dar de baja                  

                    Adeudos_Negocio.P_Cuenta_Predial = Hdn_Cuenta_ID.Value;
                    Adeudos_Negocio.P_Anio_Filtro = Const_Anio_Corriente;
                    Dt_Adeudos_Predial = Resumen.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Hdn_Cuenta_ID.Value, "POR PAGAR", Const_Anio_Corriente, Const_Anio_Corriente);
                    Double Dbl_Adeudo = 0;
                    int Mes = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                    int Anio_Actual = Const_Anio_Corriente;

                    Periodo_Corriente = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                    if (Periodo_Corriente == 6)
                        Periodo_Corriente = 0;
                    Dbl_Adeudo = (Convert.ToDouble(Dt_Adeudos_Predial.Rows[0]["ADEUDO_TOTAL_ANIO"]) / 6) * (6 - Periodo_Corriente);

                    if (!string.IsNullOrEmpty(Dbl_Adeudo.ToString().Trim()))
                    {
                        Anualidad_Original = Dbl_Adeudo;
                        Anualidad = (Anualidad_Original * Porcentaje);
                        Txt_Excedente_Construccion_Total.Text = "0.00";
                        Txt_Tasa_Valor_Total.Text = "0.00";
                        Txt_Cuota_Minima_Aplicar.Text = "0.00";
                        Txt_Total_Cuota_Fija.Text = Math.Round(Anualidad, 2).ToString("#,###,##0.00");
                        //Txt_Cuota_Anual.Text = Math.Round(Anualidad, 2).ToString("#,###,##0.00");
                        //Anualidad = Convert.ToDouble(Txt_Cuota_Anual.Text.Trim());
                        //Bimestral = Anualidad / 6;
                        //Txt_Cuota_Bimestral.Text = Bimestral.ToString("#,###,##0.00");
                        //Txt_Cuota_Bimestral.Text = Math.Round(Bimestral, 2).ToString("#,###,##0.00");
                    }
                }
                if (Solicitante.Contains("CIVIL"))
                {
                    Txt_Excedente_Construccion_Total.Text = "0.00";
                    Txt_Tasa_Valor_Total.Text = "0.00";
                    Txt_Total_Cuota_Fija.Text = Txt_Cuota_Minima_Aplicar.Text;
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion
    //Eventos Grids
    #region Eventos Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Ordenes_Variacion_PageIndexChanging
    ///DESCRIPCIÓN: Paginar grid de ordenes de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 01/Dic/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Ordenes_Variacion_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Ordenes(e.NewPageIndex);
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Ordenes_Variacion_PageIndexChanging
    ///DESCRIPCIÓN: Seleccionar Orden de Variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 01/Dic/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Ordenes_Variacion_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        ImageClickEventArgs Evento = null;
        if (Grid_Ordenes_Variacion.SelectedIndex > (-1))
        {
            Estado_Botones(Const_Estado_Inicial);
            if (String.IsNullOrEmpty(Grid_Ordenes_Variacion.SelectedDataKey["NO_CONTRARECIBO"].ToString()))
            {
                Txt_Buscar.Text = double.Parse(Grid_Ordenes_Variacion.SelectedDataKey["NO_ORDEN_VARIACION"].ToString()).ToString();
                Btn_Buscar_Click(sender, Evento);
            }
            else
            {
                Session["Contrarecibo_Traslado"] = Grid_Ordenes_Variacion.SelectedDataKey["NO_CONTRARECIBO"].ToString() + "/" + Grid_Ordenes_Variacion.SelectedDataKey["ANIO"].ToString();
                Orden_Variacion.P_Contrarecibo = Grid_Ordenes_Variacion.SelectedDataKey["NO_CONTRARECIBO"].ToString();
                Orden_Variacion.P_Año = Int32.Parse(Grid_Ordenes_Variacion.SelectedDataKey["ANIO"].ToString());
                Session["Estatus_Traslado"] = Orden_Variacion.Consulta_General_Contrarecibo().Rows[0]["ESTATUS_ORDEN"].ToString();
                Inicializa_Controles();
            }
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Copropietarios_Pageindexchanging
    ///DESCRIPCIÓN: paginar grid de propietarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:49:35 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_Copropietarios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Co(e.NewPageIndex);
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Copropietarios_SelectedIndexChanged
    ///DESCRIPCIÓN: Cargar datos de copropietarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:48:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Copropietarios_SelectedIndexChanged(object sender, EventArgs e)
    {
        String Contr_ID;
        int Page_Counter = 0;
        DataRow[] Dr_Seleccionado;
        DataTable Dt_Agregar_Co_Propietarios = (DataTable)Session["Dt_Agregar_Co_Propietarios"];
        DataTable Dt_Agregar_Prop;
        DataTable Dt_Contribuyentes = (DataTable)Session["Dt_Contribuyentes"];
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            Page_Counter = Grid_Copropietarios.PageIndex;
            if (Grid_Copropietarios.SelectedIndex > (-1))
            {
                Grid_Copropietarios.Columns[0].Visible = true;
                //Contr_ID = Grid_Copropietarios.Rows[Grid_Copropietarios.SelectedIndex + (Grid_Copropietarios.PageIndex * Grid_Copropietarios.PageSize)].Cells[0].Text;
                Contr_ID = Grid_Copropietarios.SelectedDataKey["CONTRIBUYENTE_ID"].ToString().Trim();
                Orden_Negocio.Alta_Propietario(Contr_ID, "BAJA", "COPROPIETARIO");
                Dr_Seleccionado = Dt_Agregar_Co_Propietarios.Select("CONTRIBUYENTE_ID = " + Contr_ID);
                Dt_Agregar_Co_Propietarios.Rows.Remove(Dr_Seleccionado[0]);
                Grid_Copropietarios.Columns[0].Visible = false;
                Session["Dt_Agregar_Co_Propietarios"] = Dt_Agregar_Co_Propietarios;
                Dt_Agregar_Prop = Orden_Negocio.P_Dt_Contribuyentes;
                foreach (DataRow Renglon in Dt_Agregar_Prop.Rows)
                {
                    Dt_Contribuyentes.ImportRow(Renglon);
                }
                //Se quito la formacion del datatable con la tabla de agregar_coprop
                Session["Dt_Contribuyentes"] = Dt_Contribuyentes;

            }
            if (Grid_Copropietarios.PageCount < Page_Counter && Page_Counter != 0)
                Page_Counter = Page_Counter - 1;
            Cargar_Grid_Co(Page_Counter);

            //Grid_Copropietarios.PageIndex = Page_Counter;            
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Diferencias_DataBound
    ///DESCRIPCIÓN: Cargar datos de las diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:48:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Diferencias_DataBound(object sender, EventArgs e)
    {
        try
        {
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            String Ventana = "";
            String Propiedades = "";
            String Valor_Fiscal = "0";
            for (Int32 Contador = 0; Contador < Grid_Diferencias.Rows.Count; Contador++)
            {
                TextBox Text_Valor_Temporal = (TextBox)Grid_Diferencias.Rows[Contador].Cells[2].FindControl("Txt_Grid_Dif_Valor_Fiscal");
                TextBox Text_Cuota_Bim_Temp = (TextBox)Grid_Diferencias.Rows[Contador].Cells[6].FindControl("Txt_Grid_Cuota_Bimestral");
                TextBox Text_Importe = (TextBox)Grid_Diferencias.Rows[Contador].Cells[5].FindControl("Txt_Grid_Importe");
                DropDownList Cmb_Tipo_Dif = (DropDownList)Grid_Diferencias.Rows[Contador].Cells[1].FindControl("Cmb_Tipo_Diferencias");
                ImageButton Boton_Tasas_Diferencias = (ImageButton)Grid_Diferencias.Rows[Contador].Cells[4].FindControl("Btn_Tasa_Seleccionar");
                Cmb_Tipo_Dif.SelectedValue = Dt_Agregar_Diferencias.Rows[Contador]["TIPO"].ToString();
                Valor_Fiscal = Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString();
                if (!String.IsNullOrEmpty(Valor_Fiscal))
                    Text_Valor_Temporal.Text = Convert.ToDouble(Valor_Fiscal).ToString("#,###,##0.00");
                if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()))
                    Text_Cuota_Bim_Temp.Text = Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()).ToString("#,###,##0.00");
                if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()))
                    Text_Importe.Text = Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()).ToString("#,###,##0.00");
                if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["TASA"].ToString()))
                    Grid_Diferencias.Rows[Contador].Cells[3].Text = Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["TASA"].ToString()).ToString("#,###,##0.00"); ;
                Ventana = "Abrir_Ventana_Modal('Ventanas_Emergentes/Orden_Variacion/Frm_Menu_Pre_Tasas.aspx";
                Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHide:true;help:no;scroll:no');";
                Boton_Tasas_Diferencias.Attributes.Add("OnClick", Ventana + "?Fecha=False'" + Propiedades);
                //Calcular_Analisis_Rezago("Grid_Diferencias_DataBound");
                //Calcular_Analisis_Rezago("Grid_Diferencias_RowCommand", Contador);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Grid_Diferencias_RowCommand
    ///DESCRIPCIÓN: Cargar datos de las diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:48:54 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Grid_Diferencias_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            String Tasa_Predial_ID;
            String Tasa_Concepto;
            String Tasa_Porcentaje;
            String Tasa_Previa = "";
            String Tasa_Anio;
            String Tasa_General;
            String Valor_Fiscal;
            DataRow Dr_Tasa_Seleccionada;
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            int index = 0;
            int index_row = 0;
            //TextBox Text_Importe = (TextBox)Grid_Diferencias.Rows[Convert.ToInt32(e.CommandArgument)].Cells[5].FindControl("Txt_Grid_Importe");
            //if (Convert.ToDouble(Text_Importe.Text) <= 0)
            //    Text_Importe.BackColor = System.Drawing.Color.Pink;
            //else
            //    Text_Importe.BackColor = System.Drawing.Color.White;

            try
            {
            if (e.CommandName == "Cmd_Calcular")
            {
                Cls_Ope_Pre_Tasas_Anuales_Negocio Tasas_Negocio = new Cls_Ope_Pre_Tasas_Anuales_Negocio();
                DataTable Dt_Tasas;

                index_row = Convert.ToInt32(e.CommandArgument);
                Tasas_Negocio.P_Tasa_Predial_ID = Dt_Agregar_Diferencias.Rows[index_row]["TASA_ID"].ToString();
                Tasas_Negocio.P_Anio = Dt_Agregar_Diferencias.Rows[index_row]["PERIODO"].ToString().Substring(Dt_Agregar_Diferencias.Rows[index_row]["PERIODO"].ToString().Length - 4);
                Dt_Tasas = Tasas_Negocio.Consultar_Tasas_Anuales();
                Session["Dr_Tasa_Seleccionada"] = Dt_Tasas.Rows[0];
            }
            Tasa_Previa = Dt_Agregar_Diferencias.Rows[index + (Grid_Diferencias.PageIndex * Grid_Diferencias.PageSize)]["TASA"].ToString();
                if (e.CommandName == "Cmd_Tasa")
            {
                index = Convert.ToInt32(e.CommandArgument);
                
                if (Session["Dr_Tasa_Seleccionada"] != null)
                {
                    TextBox Text_Valor_Temporal = (TextBox)Grid_Diferencias.Rows[index].Cells[2].FindControl("Txt_Grid_Dif_Valor_Fiscal");
                    if (Text_Valor_Temporal.Text.Trim() != "")
                    {
                        Valor_Fiscal = Text_Valor_Temporal.Text;
                    }
                    else
                    {
                        Text_Valor_Temporal.Text = "0.00";
                        Valor_Fiscal = "0";
                    }
                    Dr_Tasa_Seleccionada = ((DataRow)Session["Dr_Tasa_Seleccionada"]);
                    Tasa_Predial_ID = Dr_Tasa_Seleccionada["TASA_ANUAL_ID"].ToString();
                    Tasa_Concepto = Dr_Tasa_Seleccionada["IDENTIFICADOR"].ToString() + " - " + Dr_Tasa_Seleccionada["DESCRIPCION"].ToString() + " - " + Dr_Tasa_Seleccionada["ANIO"].ToString();
                    Tasa_Porcentaje = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                    Tasa_Anio = Dr_Tasa_Seleccionada["ANIO"].ToString();
                    Tasa_General = Dr_Tasa_Seleccionada["TASA_PREDIAL_ID"].ToString();
                    Grid_Diferencias.Rows[index].Cells[3].Text = Tasa_Porcentaje;
                    Dt_Agregar_Diferencias.Rows[index + (Grid_Diferencias.PageIndex * Grid_Diferencias.PageSize)]["TASA"] = Tasa_Porcentaje;
                    Dt_Agregar_Diferencias.Rows[index + (Grid_Diferencias.PageIndex * Grid_Diferencias.PageSize)]["TASA_ID"] = Tasa_Predial_ID;
                    Dt_Agregar_Diferencias.Rows[index + (Grid_Diferencias.PageIndex * Grid_Diferencias.PageSize)]["VALOR_FISCAL"] = Convert.ToDouble(Valor_Fiscal).ToString("#,###,##0.00");
                }
                }
                    if (e.CommandName == "Cmd_Calcular")
                    {
                        if (!String.IsNullOrEmpty(Tasa_Previa))
                            Calcular_Analisis_Rezago("Grid_Diferencias_RowCommand", index_row);
                        else
                            Calcular_Analisis_Rezago("Tasa_Nueva", index_row);
                        //Txt_Grid_Dif_Valor_Fiscal_TextChanged(sender, null);
                    }
            }
            catch (Exception Ex)
            {
                if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
                {
                    Mensaje_Error(Ex.Message);
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    protected void Grid_Diferencias_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {

            //TextBox Txt_Importe = (TextBox)e.Row.FindControl("Txt_Grid_Importe");
            //if (Convert.ToDouble(Txt_Importe.Text) <= 0)
            //    Txt_Importe.BackColor = System.Drawing.Color.Pink;
            //else
            //    Txt_Importe.BackColor = System.Drawing.Color.White;
            if (Session["Opcion_Tipo_Orden"] != null)
            {
                String Tipo_Orden_Variacion = Session["Opcion_Tipo_Orden"].ToString();
                DataTable Dt_Datos = (DataTable)Session["Dt_Agregar_Diferencias"];
                if (Tipo_Orden_Variacion.Trim().Equals("Predial"))
                {
                    if (Dt_Datos.Rows[e.Row.RowIndex]["TIPO_PERIODO"].ToString().Trim().Equals("CORRIENTE"))
                    {
                        if (e.Row.FindControl("Txt_Grid_Dif_Valor_Fiscal") != null)
                        {
                            TextBox Txt_Grid_Dif_Valor_Fiscal = (TextBox)e.Row.FindControl("Txt_Grid_Dif_Valor_Fiscal");
                            Txt_Grid_Dif_Valor_Fiscal.Text = Dt_Datos.Rows[e.Row.RowIndex]["VALOR_FISCAL"].ToString();
                            DropDownList Cmb_Tipo_Diferencia = (DropDownList)e.Row.FindControl("Cmb_Tipo_Diferencias");
                            if (!String.IsNullOrEmpty(Dt_Datos.Rows[e.Row.RowIndex]["TIPO"].ToString()))
                                Cmb_Tipo_Diferencia.SelectedValue = Dt_Datos.Rows[e.Row.RowIndex]["TIPO"].ToString();
                            else
                                Dt_Datos.Rows[e.Row.RowIndex]["TIPO"] = Cmb_Tipo_Diferencia.SelectedValue.ToString();
                            //Txt_Grid_Dif_Valor_Fiscal.Enabled = false;                            
                        }
                        if (e.Row.FindControl("Btn_Tasa_Seleccionar") != null)
                        {
                            ImageButton Btn_Tasa_Seleccionar = (ImageButton)e.Row.FindControl("Btn_Tasa_Seleccionar");
                            //Btn_Tasa_Seleccionar.Visible = false;
                        }
                        //Se volvio a habilitar a peticion del usuario
                        //if (e.Row.FindControl("Txt_Grid_Importe") != null) {
                        //    TextBox Txt_Grid_Importe = (TextBox)e.Row.FindControl("Txt_Grid_Importe");
                        //    Txt_Grid_Importe.Enabled = false;
                        //}
                        //if (e.Row.FindControl("Txt_Grid_Cuota_Bimestral") != null)
                        //{
                        //    TextBox Txt_Grid_Cuota_Bimestral = (TextBox)e.Row.FindControl("Txt_Grid_Cuota_Bimestral");
                        //    Txt_Grid_Cuota_Bimestral.Enabled = false;
                        //}
                    }
                }
            }
            Resumen_Grid_Diferencias();
            Calcular_Resumen();
        }
    }
    #endregion
    //Eventos de las cajas de texto del Formulario
    #region Eventos Txt_Changed

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Buscar_TextChanged
    ///DESCRIPCIÓN: Busqueda Principal de la orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Txt_Buscar_TextChanged(object sender, EventArgs e)
    {
        try
        {
            ImageClickEventArgs Evnt_Img = null;
            Btn_Buscar_Click(sender, Evnt_Img);
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
    protected void Txt_Contrarecibo_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Buscar_Contrarecibo();
            Estado_Botones(Const_Estado_Nuevo);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Valor_Fiscal_TextChanged
    ///DESCRIPCIÓN: evento para calcular la cuota fija
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Valor_Fiscal_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Cuota(true);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Porcentaje_Excencion_TextChanged
    ///DESCRIPCIÓN: evento para calcular la cuota fija
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Porcentaje_Excencion_TextChanged(object sender, EventArgs e)
    {
        try
        {
            if (Txt_Porcentaje_Excencion.Text.Trim() != "")
            {
                Session["Cuota_Fija_Nueva"] = "";
                Session.Remove("Cuota_Fija_Nueva");
                //Session["Cuota_Fija_Nueva"] = "EXCENCION";
                if (double.Parse(Txt_Porcentaje_Excencion.Text.Trim()) > 0)
                    Quitar_Adeudos(double.Parse(Txt_Porcentaje_Excencion.Text.Trim()));
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Tasa_Exedente_Construccion_TextChanged
    ///DESCRIPCIÓN: evento para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Tasa_Exedente_Construccion_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Excedentes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Tasa_Excedente_Valor_TextChanged
    ///DESCRIPCIÓN: evento para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Tasa_Excedente_Valor_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Excedentes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Exedente_Construccion_TextChanged
    ///DESCRIPCIÓN: evento para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Exedente_Construccion_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Excedentes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Excedente_Valor_TextChanged
    ///DESCRIPCIÓN: evento para calcular los impuestos por excedentes
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Excedente_Valor_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Excedentes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Excedente_Construccion_Total_TextChanged
    ///DESCRIPCIÓN: recalcular cuota fija
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/11/2011 05:47:55 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Excedente_Construccion_Total_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Excedentes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Tasa_Valor_Total_Textchanged
    ///DESCRIPCIÓN: recalcular cuota fija
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/11/2011 05:47:55 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Txt_Tasa_Valor_Total_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Excedentes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Tasa_Porcentaje_TextChanged
    ///DESCRIPCIÓN: Evento para recalcular los exedentes de valor y contruccion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************   
    protected void Txt_Tasa_Porcentaje_TextChanged(object sender, EventArgs e)
    {
        try
        {
            Calcular_Excedentes();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Grid_Dif_Valor_Fiscal_TextChanged
    ///DESCRIPCIÓN: Calcular montos de grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Grid_Importe_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            TextBox Text_Grid = sender as TextBox;
            GridViewRow gvr = Text_Grid.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Importe = gvr.FindControl("Txt_Grid_Importe") as TextBox;
            TextBox Text_C_Bimestral = gvr.FindControl("Txt_Grid_Cuota_Bimestral") as TextBox;
            Dt_Agregar_Diferencias.Rows[index]["IMPORTE"] = Text_Importe.Text.Trim();
            Dt_Agregar_Diferencias.Rows[index]["CUOTA_BIMESTRAL"] = Text_C_Bimestral.Text.Trim();
            Calcular_Resumen();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Grid_Dif_Valor_Fiscal_TextChanged
    ///DESCRIPCIÓN: Calcular montos de grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Grid_Dif_Valor_Fiscal_TextChanged(object sender, EventArgs e)
    {
        int index = 0;
        try
        {
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            TextBox Text_Grid = sender as TextBox;
            GridViewRow gvr = Text_Grid.NamingContainer as GridViewRow;
            index = gvr.DataItemIndex;
            TextBox Text_Importe = gvr.FindControl("Txt_Grid_Dif_Valor_Fiscal") as TextBox;
            Dt_Agregar_Diferencias.Rows[index]["VALOR_FISCAL"] = Text_Importe.Text.Trim();
            Calcular_Resumen();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
        Resumen_Grid_Diferencias();
        Calcular_Resumen();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Txt_Superficie_Construida_Click
    ///DESCRIPCIÓN: Calcular montos de grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 17/Nov/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Txt_Superficie_Construida_Click(object sender, EventArgs e)
    {
        Resumen_Grid_Diferencias();
        Calcular_Resumen();
    }

    #endregion
    //Eventos de los Botones del Formulario
    #region Eventos Botones
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Establecer_Cuenta_Predial_Click
    ///DESCRIPCIÓN: Maneja el Evento del Boton para establecer la cuenta de Predial
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 10/Noviembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    protected void Btn_Establecer_Cuenta_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        String Cuenta_ID;
        String Estatus = "PENDIENTE";
        String[] Cuenta_ID_Estatus;
        DataTable Orden_Variacion;
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()) && Txt_Cuenta_Predial.Text.Trim().Length == 12)
            {
                Cuenta_ID = Validar_Cuenta_Existente(Txt_Cuenta_Predial.Text.Trim());//Validar i ya existe una cuenta igual
                Cuenta_ID_Estatus = Cuenta_ID.Split('-');
                if (!String.IsNullOrEmpty(Cuenta_ID_Estatus[0]))
                    Cuenta_ID = Cuenta_ID_Estatus[0];
                if (Cuenta_ID_Estatus.Length > 1)
                {
                    if (!String.IsNullOrEmpty(Cuenta_ID_Estatus[1]))
                        Estatus = Cuenta_ID_Estatus[1];
                }
                if (String.IsNullOrEmpty(Cuenta_ID))
                {
                    String Cuenta_Predial_ID = Crear_Cuenta_Predial_ID(Txt_Cuenta_Predial.Text.Trim());//Crea registro en la tabla
                    if (!String.IsNullOrEmpty(Cuenta_Predial_ID))
                    {
                        Hdn_Cuenta_ID.Value = Cuenta_Predial_ID;
                        Hdn_Cuenta_ID_Temp.Value = Cuenta_Predial_ID;
                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "Operacion_Predial_Traslado", "alert('Cuenta Registrada Exitosamente!')", true);
                        Session["Cuenta_Predial_ID_Adeudos"] = Hdn_Cuenta_ID.Value;
                        Limpiar_Session_Cuotas_Fijas();
                        Cargar_Ventana_Emergente_Adeudo_Diferencias();
                        Btn_Establecer_Cuenta_Predial.Visible = false;
                        Txt_Cuenta_Predial.Enabled = false;
                        Cargar_Datos();
                        Consulta_Ultimo_Mov();
                        Consulta_Couta_Minima();
                        Estado_Botones(Const_Estado_Nuevo);
                        Cmb_Tipos_Movimiento.Enabled = true;
                    }
                    else
                    {
                        Mensaje_Error("La Cuenta Predial no pudo ser creada");
                    }
                }
                else
                {
                    if (!Estatus.Contains("PENDIENTE"))
                    {
                        Mensaje_Error("La Cuenta Predial No. " + Txt_Cuenta_Predial.Text.Trim().ToUpper() + " ya existe");
                    }
                    else
                    {
                        if (Estatus == "PENDIENTE")
                        {
                            Orden_Negocio.P_Cuenta_Predial_ID = Cuenta_ID;
                            Orden_Negocio.P_Filtros_Dinamicos = Ope_Pre_Ordenes_Variacion.Campo_Cuenta_Predial_ID + " = '" + Cuenta_ID + "' AND " + Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden + " = 'POR VALIDAR' ";
                            Orden_Variacion = Orden_Negocio.Consultar_Ordenes_Variacion();
                            if (Orden_Variacion.Rows.Count <= 0)
                            {
                                Hdn_Cuenta_ID.Value = Cuenta_ID;
                                Hdn_Cuenta_ID_Temp.Value = Cuenta_ID;
                                Btn_Establecer_Cuenta_Predial.Visible = false;
                                Txt_Cuenta_Predial.Enabled = false;
                                Cargar_Datos();
                                Consulta_Ultimo_Mov();
                                Consulta_Couta_Minima();
                                Estado_Botones(Const_Estado_Nuevo);
                                Cmb_Tipos_Movimiento.Enabled = true;
                                Mensaje_Error("La Cuenta Predial ya esta creada pero no cuenta con datos");
                            }
                            else
                                Mensaje_Error("La Cuenta Predial No. " + Txt_Cuenta_Predial.Text.Trim().ToUpper() + " tiene una Orden de Variacion por validar");
                        }
                    }
                }
                //Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            }
            else { Mensaje_Error("Favor de Ingresar Correctamente la Cuenta predial "); }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("No se pudo dar de alta la Cuenta" + Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Session_Cuotas_Fijas
    ///DESCRIPCIÓN          : Limpiar sesion de CF
    ///PARAMETROS           : 
    ///CREO                 : Jesus Salvador Toledo Rodriguez
    ///FECHA_CREO           : 22/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    private void Limpiar_Session_Cuotas_Fijas()
    {
        Session.Remove("Cuota_Fija_Anterior");
        Session["Cuota_Fija_Anterior"] = "";
        Session.Remove("Cuota_Fija_Nueva");
        Session["Cuota_Fija_Nueva"] = "";
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Ultima_Cuota_Fija
    ///DESCRIPCIÓN          : consulta CF de cuenta predial
    ///PARAMETROS           : 
    ///CREO                 : Jesus Salvador Toledo Rodriguez
    ///FECHA_CREO           : 22/Febrero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    private void Consultar_Ultima_Cuota_Fija()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Detalles_Cuota_Fija = null;
        try
        {
            Session.Remove("Cuota_Fija_Anterior");
            Session["Cuota_Fija_Anterior"] = "";
            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Generar_Orden_Anio = Const_Anio_Corriente.ToString();
            Orden_Variacion.P_Campo = "NO_CUOTA_FIJA";
            Detalles_Cuota_Fija = Orden_Variacion.Consultar_Ultima_Cuota_Fija();
            if (Detalles_Cuota_Fija != null)
            {
                if (Detalles_Cuota_Fija.Rows.Count > 0)
                    Session["Cuota_Fija_Anterior"] = Detalles_Cuota_Fija.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Total_Cuota_Fija];
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Consultar_Ultima_Cuota_Fija" + Ex.Message);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Crear_Cuenta_Predial_ID
    ///DESCRIPCIÓN          : Se crea la cuenta en la tabla Cat_Pre_Cuentas_Predial
    ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    private String Crear_Cuenta_Predial_ID(String Cuenta_Predial)
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        String Cuenta_Predial_ID = "";

        Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial;
        Cuentas_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
        Cuentas_Predial.P_Estatus = "PENDIENTE";
        if (Cuentas_Predial.Alta_Cuenta())
        {
            DataTable Dt_Cuentas_Predial;
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID;
            Cuentas_Predial.P_Cuenta_Predial = Cuenta_Predial.ToUpper();
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial != null)
            {
                if (Dt_Cuentas_Predial.Rows.Count > 0)
                {
                    Cuenta_Predial_ID = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
                }
            }
        }
        return Cuenta_Predial_ID;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Cuenta_Existente
    ///DESCRIPCIÓN          : Se crea la cuenta en la tabla Cat_Pre_Cuentas_Predial
    ///PARAMETROS           : Cuenta_Predial, String con el valor del No. de Cuenta
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 15/Enero/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************    
    private String Validar_Cuenta_Existente(String Cuenta_Predial)
    {
        //Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        String Cuenta_Predial_ID = "";
        String Existe = "";
        Cuenta_Predial_ID = Orden_Negocio.Consultar_Cuenta_Existente_ID(Cuenta_Predial);
        return Cuenta_Predial_ID;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonias_Click
    ///DESCRIPCIÓN: Mostrar Busqueda Avanzada de colonias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:49:58 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Colonias_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                Hdn_Colonia_ID.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                Txt_Colonia_Cuenta.Text = Session["CLAVE_COLONIA"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                Hdn_Calle_ID.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                Txt_Calle_Cuenta.Text = Session["CLAVE_CALLE"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
            }
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calles_Click
    ///DESCRIPCIÓN: Mostrar Busqueda Avanzada de calles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:49:58 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Calles_Click(object sender, ImageClickEventArgs e)
    {
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                Hdn_Colonia_ID_Notificacion.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                Txt_Colonia_Propietario.Text = Session["CLAVE_COLONIA"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                Hdn_Calle_ID_Notificacion.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                Txt_Calle_Propietario.Text = Session["CLAVE_CALLE"].ToString().Replace("&nbsp;", "") + " " + Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
                Cmb_Domicilio_Foraneo.SelectedValue = "NO";
                Chk_Mismo_Domicilio.Checked = false;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Tasa_Seleccionar_Click
    ///DESCRIPCIÓN: agregar tasas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:49:58 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Tasa_Seleccionar_Click(object sender, ImageClickEventArgs e)
    {

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Vista_Adeudos_Click
    ///DESCRIPCIÓN: restablecer session para vista de adeudos
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Oct/2011 10:44:50 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Vista_Adeudos_Click(object sender, ImageClickEventArgs e)
    {
        Session["Cuenta_Predial_ID_Adeudos"] = Hdn_Cuenta_ID.Value;
        Session["Orden_Variacion_ID_Adeudos"] = Hdn_Orden_Variacion.Value;
        Session["Anio_Orden_Adeudos"] = Hdn_Orden_Variacion_Anio.Value;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Regresar_Anualidad_Click
    ///DESCRIPCIÓN: Regresar Anualidad
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/Abr/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************  
    protected void Btn_Regresar_Anualidad_Click(object sender, ImageClickEventArgs e)
    {
        Txt_Cuota_Anual.Text = Convert.ToDouble(Obtener_Dato_Consulta(Cat_Pre_Cuentas_Predial.Campo_Cuota_Anual, Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas, Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " = '" + Txt_Cuenta_Predial.Text + "'")).ToString("#,###,##0.00"); ;
        Txt_Cuota_Bimestral.Text = (Convert.ToDouble(Txt_Cuota_Anual.Text.Trim()) / 6).ToString("#,###,##0.00");
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Co_Propietarios_Click
    ///DESCRIPCIÓN: agregar copropietarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011 05:49:58 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Agregar_Co_Propietarios_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Agregar_Co_Propietarios = (DataTable)Session["Dt_Agregar_Co_Propietarios"];
        DataTable Dt_Agregar_Prop;
        DataTable Dt_Contribuyentes = (DataTable)Session["Dt_Contribuyentes"];
        DataRow[] Dr_Validacion_RFC;
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {

            if (Txt_Co_Propietario.Text.Trim() != "" && Session["CONTRIBUYENTE_ID"] != null && Session["RFC"] != null)
            {
                Dr_Validacion_RFC = Dt_Agregar_Co_Propietarios.Select("NOMBRE_CONTRIBUYENTE = '" + Txt_Co_Propietario.Text.Trim() + "' AND RFC = '" + Session["RFC"].ToString() + "'");
                if (Dr_Validacion_RFC.Length <= 0)
                {
                    Orden_Negocio.Alta_Propietario(Session["CONTRIBUYENTE_ID"].ToString(), "ALTA", "COPROPIETARIO");
                    DataRow Dr_Co_Pro = Dt_Agregar_Co_Propietarios.NewRow();
                    Dr_Co_Pro["CONTRIBUYENTE_ID"] = Session["CONTRIBUYENTE_ID"].ToString();
                    Dr_Co_Pro["NOMBRE_CONTRIBUYENTE"] = Txt_Co_Propietario.Text.Trim();
                    Dr_Co_Pro["RFC"] = Session["RFC"].ToString();
                    Txt_Co_Propietario.Text = "";
                    Dt_Agregar_Prop = Orden_Negocio.P_Dt_Contribuyentes;
                    foreach (DataRow Dr in Dt_Agregar_Prop.Rows)
                    {
                        DataRow Dr_Con = Dt_Contribuyentes.NewRow();
                        Dr_Con["CONTRIBUYENTE_ID"] = Dr["CONTRIBUYENTE_ID"];
                        Dr_Con["ESTATUS"] = Dr["ESTATUS"];
                        Dr_Con["TIPO"] = Dr["TIPO"];
                        Dt_Contribuyentes.Rows.Add(Dr_Con);

                    }
                    Session["Dt_Contribuyentes"] = Dt_Contribuyentes;

                    Dt_Agregar_Co_Propietarios.Rows.Add(Dr_Co_Pro);
                    Session["Dt_Agregar_Co_Propietarios"] = Dt_Agregar_Co_Propietarios;
                    Cargar_Grid_Co(0);
                    Session["CONTRIBUYENTE_ID"] = null;
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Orden de Variacion", "alert('Ya existe un Copropietario con el mismo nombre y RFC');", true);
                    Txt_Co_Propietario.Text = "";
                }
            }
            if (Chk_Cuota_Fija.Checked)
            {
                Session["Cuota_Fija_Nueva"] = "";
                Session.Remove("Cuota_Fija_Nueva");
                Session["Cuota_Fija_Nueva"] = Txt_Total_Cuota_Fija.Text.Trim();
            }

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_P_Corriente_Click
    ///DESCRIPCIÓN: se agrega una diferencia en el periodo corriente
    ///PARAMETROS: object sender, ImageClickEventArgs e
    ///CREO: jtoledo
    ///FECHA_CREO: 09/Agosto/2011 04:27:30 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Agregar_P_Corriente_Click(object sender, ImageClickEventArgs e)
    {
        string Periodo;
        int B_Inicial;
        int B_Final;
        int Lapzo;
        string[] Bimestres;
        int Bimestre_Inicio;
        double Importe = 0;
        double C_Bimestral = 0;
        try
        {
            //Si es predial valido que tenga valor fiscal y tasa son obligarorios para predial si es... agregar al calculo por default...
            String Tipo_Orden_Variacion = "Traslado";
            if (Session["Opcion_Tipo_Orden"] != null)
            {
                Tipo_Orden_Variacion = Session["Opcion_Tipo_Orden"].ToString();
            }
            if (Tipo_Orden_Variacion.Trim().Equals("Predial"))
            {
                //if(Txt_Valor_Fiscal.Text.Trim().Length>0 && Txt_tasa_)
            }
            DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            if (Convert.ToInt32(Cmb_P_C_Bimestre_Inicial.SelectedValue) <= Convert.ToInt32(Cmb_P_C_Bimestre_Final.SelectedValue))
            {
                Periodo = Cmb_P_C_Bimestre_Inicial.SelectedValue.ToString() + "/" + Const_Anio_Corriente.ToString() + " - " + Cmb_P_C_Bimestre_Final.SelectedValue.ToString() + "/" + Const_Anio_Corriente.ToString();
                DataRow[] Dr_Validar = Dt_Agregar_Diferencias.Select("PERIODO = '" + Periodo + "'");
                if (Dr_Validar.Length <= 0)
                {
                    if ((Int32.Parse(Cmb_P_C_Bimestre_Inicial.SelectedValue.ToString()) < Int32.Parse(Txt_Desde_Periodo_Corriente.Text) || Txt_Desde_Periodo_Corriente.Text == "0"))
                    {
                        Txt_Desde_Periodo_Corriente.Text = Cmb_P_C_Bimestre_Inicial.SelectedValue.ToString();
                        Txt_Desde_Anio_Periodo_Corriente.Text = Const_Anio_Corriente.ToString();
                    }

                    if (Int32.Parse(Cmb_P_C_Bimestre_Final.SelectedValue.ToString()) > Int32.Parse(Txt_Hasta_Periodo_Corriente.Text))
                    {
                        Txt_Hasta_Periodo_Corriente.Text = Cmb_P_C_Bimestre_Final.SelectedValue.ToString();
                        Txt_Hasta_Anio_Periodo_Corriente.Text = Const_Anio_Corriente.ToString();
                    }
                    DataRow Dr_Periodo = Dt_Agregar_Diferencias.NewRow();
                    Dr_Periodo["PERIODO"] = Periodo;
                    Dr_Periodo["TIPO_PERIODO"] = "CORRIENTE";
                    Dr_Periodo["TIPO"] = "ALTA";
                    if (Tipo_Orden_Variacion.Trim().Equals("Predial"))
                    {
                        Dr_Periodo["VALOR_FISCAL"] = Txt_Valor_Fiscal.Text.Trim();
                        Dr_Periodo["TASA"] = Txt_Tasa_Porcentaje.Text.Trim();
                        Dr_Periodo["TASA_ID"] = Hdn_Tasa_ID.Value.ToString();
                        if (!String.IsNullOrEmpty(Txt_Valor_Fiscal.Text.Trim()) && !String.IsNullOrEmpty(Txt_Tasa_Porcentaje.Text.Trim()))
                        {
                            Bimestres = Periodo.Split('-');
                            B_Inicial = Int32.Parse(Bimestres[0][0].ToString());
                            B_Final = Int32.Parse(Bimestres[1][1].ToString());
                            Lapzo = (B_Final - B_Inicial) + 1;
                            Importe = Math.Round((Convert.ToDouble(Txt_Valor_Fiscal.Text.Trim()) * (Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim()) / 1000)), 2);
                            C_Bimestral = Importe / 6;
                            Importe = C_Bimestral * Lapzo;
                            Dr_Periodo["IMPORTE"] = Importe.ToString("#,###,##0.00");
                            Dr_Periodo["CUOTA_BIMESTRAL"] = C_Bimestral.ToString("#,###,##0.00");
                        }
                    }
                    Cmb_P_C_Bimestre_Final.SelectedIndex = 5;
                    Cmb_P_C_Bimestre_Inicial.SelectedIndex = 0;

                    if (Dr_Periodo != null)
                        Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo);
                    Dr_Periodo = null;
                    Cargar_Grid_Diferencias(0);
                }
                else
                    Lbl_Mensaje_Error_Diferencias.Text = "Ya se ha agregado este periodo";
            }
            if (Chk_Cuota_Fija.Checked)
            {
                Session["Cuota_Fija_Nueva"] = "";
                Session.Remove("Cuota_Fija_Nueva");
                Session["Cuota_Fija_Nueva"] = Txt_Total_Cuota_Fija.Text.Trim();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error_Diferencias.Text = Ex.Message;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_P_Regazo_Click
    ///DESCRIPCIÓN: se agrega un regazo en el periodo corriente
    ///PARAMETROS: object sender, ImageClickEventArgs e
    ///CREO: jtoledo
    ///FECHA_CREO: 09/Agosto/2011 05:27:30 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Agregar_P_Regazo_Click(object sender, ImageClickEventArgs e)
    {
        string Periodo;
        int Bimestre_Final = 0;
        int Bimestre_Inicial = 0;
        int Anio_Inicial = 0;
        int Anio_Final = 0;
        int anios = 0;
        DataTable Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        try
        {
            if (Validar_Diferencia_Regazo())
            {
                Periodo = Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString() + "/" + Cmb_P_R_Anio_Inicial.SelectedValue.ToString() + " - " + Cmb_P_R_Bimestre_Final.SelectedValue.ToString() + "/" + Cmb_P_R_Anio_Final.SelectedValue.ToString();
                DataRow[] Dr_Validar = Dt_Agregar_Diferencias.Select("PERIODO = '" + Periodo + "'");
                if (Dr_Validar.Length <= 0)
                {
                    DataRow Dr_Periodo = Dt_Agregar_Diferencias.NewRow();
                    //String[] cadena = Cmb_Grid_Tasas.SelectedItem.Text.Split('-');

                    Dr_Periodo["PERIODO"] = Periodo;
                    Dr_Periodo["TIPO_PERIODO"] = "REZAGO";
                    Dr_Periodo["TIPO"] = "ALTA";
                    //Agregar al resumen                        

                    if ((Int32.Parse(Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString()) < Int32.Parse(Txt_Desde_Periodo_Regazo.Text) || Txt_Desde_Periodo_Regazo.Text == "0"))
                    {
                        if ((Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue.ToString()) <= Int32.Parse(Lbl_P_C_Anio_Inicio.Text) || Lbl_P_C_Anio_Inicio.Text == "0"))
                            Txt_Desde_Periodo_Regazo.Text = Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString();
                    }

                    if (Int32.Parse(Cmb_P_R_Bimestre_Final.SelectedValue.ToString()) > Int32.Parse(Txt_Hasta_Periodo_Regazo.Text))
                    {
                        if ((Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue.ToString()) >= Int32.Parse(Lbl_P_C_Anio_Final.Text)))
                            Txt_Hasta_Periodo_Regazo.Text = Cmb_P_R_Bimestre_Final.SelectedValue.ToString();
                    }

                    if ((Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue.ToString()) < Int32.Parse(Lbl_P_C_Anio_Inicio.Text) || Lbl_P_C_Anio_Inicio.Text == "0"))
                    {
                        Lbl_P_C_Anio_Inicio.Text = Cmb_P_R_Anio_Inicial.SelectedValue.ToString();
                    }

                    if ((Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue.ToString()) > Int32.Parse(Lbl_P_C_Anio_Final.Text)))
                    {
                        Lbl_P_C_Anio_Final.Text = Cmb_P_R_Anio_Final.SelectedValue.ToString();
                    }

                    if (Dr_Periodo != null)
                    {
                        if (Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue) < Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue))
                        {
                            Bimestre_Final = Int32.Parse(Cmb_P_R_Bimestre_Final.SelectedValue.ToString());
                            Bimestre_Inicial = Int32.Parse(Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString());
                            Anio_Inicial = Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue.ToString());
                            Anio_Final = Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue.ToString());
                            DataRow Dr_Periodo_Temp;
                            Dr_Periodo_Temp = Dr_Periodo;
                            anios = Anio_Final - Anio_Inicial;
                            for (int cont = Anio_Inicial; cont <= Anio_Final; cont++)
                            {
                                if (cont == Anio_Inicial)
                                {
                                    DataRow Dr_Periodo_Temporal1 = Dt_Agregar_Diferencias.NewRow();
                                    Dr_Periodo_Temporal1["PERIODO"] = Bimestre_Inicial.ToString() + "/" + cont.ToString() + " - 6/" + cont.ToString();
                                    //Dr_Periodo_Temporal1["TASA"] = Txt_Tasa_Porcentaje_Diferencias.Text.Trim();//cadena[2].Trim();
                                    //Dr_Periodo_Temporal1["TASA_ID"] = Hdn_Tasa_Dif.Value.ToString();                                        
                                    Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo_Temporal1);
                                }
                                else if (cont == Anio_Final)
                                {
                                    DataRow Dr_Periodo_Temporal2 = Dt_Agregar_Diferencias.NewRow();
                                    Dr_Periodo_Temporal2["PERIODO"] = "1/" + cont.ToString() + " - " + Bimestre_Final.ToString() + "/" + cont.ToString();
                                    //Dr_Periodo_Temporal2["TASA"] = Txt_Tasa_Porcentaje_Diferencias.Text.Trim();//cadena[2].Trim();
                                    //Dr_Periodo_Temporal2["TASA_ID"] = Hdn_Tasa_Dif.Value.ToString();                                        
                                    Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo_Temporal2);
                                }
                                else
                                {
                                    DataRow Dr_Periodo_Temporal3 = Dt_Agregar_Diferencias.NewRow();
                                    Dr_Periodo_Temporal3["PERIODO"] = "1/" + cont.ToString() + " - 6/" + cont.ToString();
                                    //Dr_Periodo_Temporal3["TASA"] = Txt_Tasa_Porcentaje_Diferencias.Text.Trim();//cadena[2].Trim();
                                    //Dr_Periodo_Temporal3["TASA_ID"] = Hdn_Tasa_Dif.Value.ToString();                                        
                                    Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo_Temporal3);
                                }


                            }
                        }
                        else
                            Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo);

                    }

                    Cmb_P_R_Bimestre_Final.SelectedIndex = 5;
                    Cmb_P_R_Bimestre_Inicial.SelectedIndex = 0;
                    Cmb_P_R_Anio_Inicial.SelectedIndex = 0;
                    Cmb_P_R_Anio_Final.SelectedIndex = 0;

                    Dr_Periodo = null;
                    Session["Dt_Agregar_Diferencias"] = Dt_Agregar_Diferencias;
                    Cargar_Grid_Diferencias(0);
                }
            }
        }
        catch (Exception Ex)
        {

        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Click
    ///DESCRIPCIÓN: se agrega un regazo en el periodo corriente
    ///PARAMETROS: object sender, ImageClickEventArgs e
    ///CREO: jtoledo
    ///FECHA_CREO: 09/Agosto/2011 05:27:30 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Inicializa_Controles();
            Limpiar_Formulario();
            Session_Remove();
            Estado_Botones(Const_Estado_Modificar);
            Buscar_Orden();
            Validar_Acceso();
            Cargar_Ventana_Emergente_Resumen_Predio();
            Cargar_Ventana_Emergente_Adeudo_Diferencias();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Cuentas_Click
    ///DESCRIPCIÓN: Busqueda de ordenes por cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 30/Nov/2011 02:52:21 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Btn_Busqueda_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Estado_Botones(Const_Estado_Inicial);
            Limpiar_Formulario();
            Session_Remove();
            Limpiar_Session_Agregar_Diferencias();
            Estado_Botones(Const_Estado_Busqueda_Cuenta);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Consultar_Ordenes_Cuenta_Click
    ///DESCRIPCIÓN: Busqueda de ordenes por cuenta
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 30/Nov/2011 02:52:21 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Btn_Consultar_Ordenes_Cuenta_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Txt_Busqueda_Cuenta.Text.Trim()) || !String.IsNullOrEmpty(Txt_Busqueda_Contrarecibo.Text.Trim()))
                Cargar_Grid_Ordenes(0);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Co_Propietarios_Click
    ///DESCRIPCIÓN: Busqueda de copropietarios
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Ago/2011 02:52:21 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void Btn_Busqueda_Co_Propietarios_Click(object sender, ImageClickEventArgs e)
    {
        String Contribuyente_ID;
        String Contribuyente_Nombre;
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();

        try
        {
            Contribuyente_ID = Session["CONTRIBUYENTE_ID"].ToString();
            Contribuyente_Nombre = HttpUtility.HtmlDecode(Session["CONTRIBUYENTE_NOMBRE"].ToString());
            Txt_Co_Propietario.Text = Contribuyente_Nombre;
        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")

                Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Cuota_Minima_Click
    ///DESCRIPCIÓN: Obtener datos de busqueda avanzada de tasas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Cuota_Minima_Click(object sender, ImageClickEventArgs e)
    {
        String Cuota_Minima_ID;
        String Descripcion;
        String cuota;
        Double Dbl_Couta;
        DataRow Dr_Tasa_Seleccionada;
        try
        {
            if (Session["Dr_Cuotas_Minimas"] != null)
            {
                Dr_Tasa_Seleccionada = ((DataRow)Session["Dr_Cuotas_Minimas"]);
                Cuota_Minima_ID = Dr_Tasa_Seleccionada["Cuota_ID"].ToString();
                Descripcion = Dr_Tasa_Seleccionada["Cuota"].ToString();

                Txt_Cuota_Minima.Text = Descripcion;

                cuota = Txt_Cuota_Minima.Text.Trim();
                Dbl_Couta = Convert.ToDouble(cuota.Trim());
                Hdn_Cuota_Minima.Value = Dbl_Couta.ToString();

                Calcular_Cuota(false);
                Calcular_Excedentes();
            }

        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Tasas_Click
    ///DESCRIPCIÓN: Obtener datos de busqueda avanzada de tasas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Tasas_Click(object sender, ImageClickEventArgs e)
    {
        String Tasa_Predial_ID;
        String Tasa_Concepto;
        String Tasa_Porcentaje;
        String Tasa_Anio;
        DataRow Dr_Tasa_Seleccionada;
        try
        {
            if (Session["Dr_Tasa_Seleccionada"] != null)
            {
                Dr_Tasa_Seleccionada = ((DataRow)Session["Dr_Tasa_Seleccionada"]);
                Tasa_Predial_ID = Dr_Tasa_Seleccionada["TASA_ANUAL_ID"].ToString();
                Tasa_Concepto = Dr_Tasa_Seleccionada["IDENTIFICADOR"].ToString() + " - " + Dr_Tasa_Seleccionada["DESCRIPCION"].ToString() + " - " + Dr_Tasa_Seleccionada["ANIO"].ToString();
                Tasa_Porcentaje = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                Tasa_Anio = Dr_Tasa_Seleccionada["ANIO"].ToString();
                Hdn_Tasa_General_ID.Value = Dr_Tasa_Seleccionada["TASA_ID"].ToString();

                Txt_Tasa_Porcentaje.Text = Tasa_Porcentaje;
                Txt_Tasa_Excedente_Valor.Text = Tasa_Porcentaje;
                Txt_Tasa_Exedente_Construccion.Text = Tasa_Porcentaje;
                Txt_Tasa_Descripcion.Text = Tasa_Concepto;
                Hdn_Tasa_ID.Value = Tasa_Predial_ID;
                Calcular_Cuota(true);
                Calcular_Excedentes();
            }

        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Mostrar_Busqueda_Cuentas_Click
    ///DESCRIPCIÓN: Obtener datos de busqueda avanzada de cuentas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        String Cuenta_Predial_ID;
        String Cuenta_Predial;
        try
        {
            if (Session["BUSQUEDA_CUENTAS_PREDIAL"] != null)
            {
                if (!String.IsNullOrEmpty(Session["BUSQUEDA_CUENTAS_PREDIAL"].ToString()))
                {
                    Limpiar_Formulario();
                    Session_Remove();
                    Limpiar_Session_Cuotas_Fijas();
                    if (Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]))
                    {
                        if (Session["CUENTA_PREDIAL_ID"].ToString() != "" && Session["CUENTA_PREDIAL_ID"] != null)
                        {
                            Session["Cuenta_Predial_ID"] = Session["CUENTA_PREDIAL_ID"].ToString();
                            Cuenta_Predial_ID = Session["CUENTA_PREDIAL_ID"].ToString();
                            Cuenta_Predial = Session["CUENTA_PREDIAL"].ToString();
                            Txt_Cuenta_Predial.Text = Cuenta_Predial;
                            Session["M_Cuenta_ID"] = Cuenta_Predial_ID;
                            Hdn_Cuenta_ID_Temp.Value = Session["M_Cuenta_ID"].ToString();
                            Session["Cuenta_Predial_ID_Adeudos"] = Convert.ToString(Session["Cuenta_Predial_ID"]);
                            Session["Orden_Variacion_ID_Adeudos"] = Hdn_Orden_Variacion.Value;
                            Session["Anio_Orden_Adeudos"] = Hdn_Orden_Variacion_Anio.Value;
                            Hdn_Cuenta_ID.Value = Session["M_Cuenta_ID"].ToString();
                            //if (!Validar_Historial_Ordenes_Variacion())
                            {
                                if (!Validar_Ordenes_Variacion_Directas_Cuenta())
                                {
                                    Cargar_Datos();
                                    Consulta_Ultimo_Mov();
                                    Consulta_Couta_Minima();
                                    Estado_Botones(Const_Estado_Nuevo);
                                    Cmb_Tipos_Movimiento.Enabled = true;
                                    Txt_Cuenta_Predial.Enabled = false;
                                    Btn_Establecer_Cuenta_Predial.Visible = false;
                                    Validar_Acceso();
                                    Cargar_Ventana_Emergente_Resumen_Predio();
                                    Cargar_Ventana_Emergente_Adeudo_Diferencias();
                                }
                                else
                                {
                                    Cuenta_Predial_ID = "";
                                    Cuenta_Predial = "";
                                    Txt_Cuenta_Predial.Text = "";
                                    Session["M_Cuenta_ID"] = "";
                                    Hdn_Cuenta_ID_Temp.Value = "";
                                    Session["Cuenta_Predial_ID_Adeudos"] = "";
                                    Session["Orden_Variacion_ID_Adeudos"] = "";
                                    Session["Anio_Orden_Adeudos"] = "";
                                    Hdn_Cuenta_ID.Value = "";

                                    Img_Error.Visible = false;
                                    if (Session["Validar_Ordenes_Variacion_Directas_Cuenta"] != null)
                                    {
                                        Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación Directa con estatus de " + Session["Validar_Ordenes_Variacion_Directas_Cuenta"].ToString() + "</br>";
                                    }
                                    else
                                    {
                                        Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación Directa pendiente Por Validar</br>";
                                    }
                                }
                            }
                            //else
                            //{
                            //    Cuenta_Predial_ID = "";
                            //    Cuenta_Predial = "";
                            //    Txt_Cuenta_Predial.Text = "";
                            //    Session["M_Cuenta_ID"] = "";
                            //    Hdn_Cuenta_ID_Temp.Value = "";
                            //    Session["Cuenta_Predial_ID_Adeudos"] = "";
                            //    Hdn_Cuenta_ID.Value = "";

                            //    Img_Error.Visible = false;
                            //    Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación pendiente Por Validar</br>";
                            //}
                        }
                    }
                }
                else
                {
                    Cargar_Ventana_Emergente_Resumen_Predio();
                }
            }

        }
        catch (Exception Ex)
        {
            Limpiar_Formulario();
            Session_Remove();
            Limpiar_Session_Agregar_Diferencias();
            if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
            {
                Mensaje_Error(Ex.Message);
            }
        }
        finally
        {
            Session["BUSQUEDA_CUENTAS_PREDIAL"] = String.Empty;
            Session["CUENTA_PREDIAL_ID"] = String.Empty;
            Session["CUENTA_PREDIAL"] = String.Empty;
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Propietarios_Click
    ///DESCRIPCIÓN: lanza la ventana emergente y asigna los valores a los controles
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Busqueda_Propietarios_Click(object sender, ImageClickEventArgs e)
    {
        String Contribuyente_ID;
        String Contribuyente_Nombre;
        String Contribuyente_RFC;
        DataTable Dt_Agregar_Prop;
        DataTable Dt_Contribuyentes = (DataTable)Session["Dt_Contribuyentes"];
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Cls_Cat_Pre_Contribuyentes_Negocio Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();
        try
        {
            if (!String.IsNullOrEmpty(Session["CONTRIBUYENTE_ID"].ToString()))
            {
                //Agregar Quitar Propietario para Orden 
                if (!String.IsNullOrEmpty(Hdn_Propietario_ID.Value))
                    Orden_Negocio.Alta_Propietario(Hdn_Propietario_ID.Value.ToString(), "BAJA", "PROPIETARIO");
                Orden_Negocio.Alta_Propietario(Session["CONTRIBUYENTE_ID"].ToString(), "ALTA", "PROPIETARIO");
                Contribuyente_ID = Session["CONTRIBUYENTE_ID"].ToString();
                Contribuyente_Nombre = HttpUtility.HtmlDecode(Session["CONTRIBUYENTE_NOMBRE"].ToString());
                Contribuyente_RFC = Session["RFC"].ToString();

                Txt_Nombre_Propietario.Text = Contribuyente_Nombre;
                Hdn_Propietario_ID.Value = Contribuyente_ID;
                Txt_Rfc_Propietario.Text = HttpUtility.HtmlDecode(Contribuyente_RFC);
                Dt_Agregar_Prop = Orden_Negocio.P_Dt_Contribuyentes;
                foreach (DataRow Dr in Dt_Agregar_Prop.Rows)
                {
                    DataRow Dr_Con = Dt_Contribuyentes.NewRow();
                    Dr_Con["CONTRIBUYENTE_ID"] = Dr["CONTRIBUYENTE_ID"];
                    Dr_Con["ESTATUS"] = Dr["ESTATUS"];
                    Dr_Con["TIPO"] = Dr["TIPO"];
                    Dt_Contribuyentes.Rows.Add(Dr_Con);
                }
                Session["Dt_Contribuyentes"] = Dt_Contribuyentes;
                Contribuyente.P_Contribuyente_ID = Hdn_Propietario_ID.Value;
                Contribuyente = Contribuyente.Consultar_Datos_Contribuyente();
                Hdn_Propietario_Validacion_Persona.Value = Contribuyente.P_Tipo_Persona;
                Session["CONTRIBUYENTE_ID"] = null;
            }
        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
                Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Cuota_Click
    ///DESCRIPCIÓN: evento para calcular la cuota fija
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Calcular_Cuota_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Calcular_Cuota(true);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Calcular_Cuota_Fija_Click
    ///DESCRIPCIÓN: evento para calcular la cuota fija
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:49:10 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Calcular_Cuota_Fija_Click(object sender, ImageClickEventArgs e)
    {
        Calcular_Excedentes();
        //Codigo para Agregar Diferencia Automatica
        //if (Chk_Cuota_Fija.Checked && Lbl_Ultimo_Movimiento.Text.Length > 0)
        if (Chk_Cuota_Fija.Checked)
        {
            Quitar_Porcentaje_Anualidad();
            Quitar_Adeudos();
        }
        //if (Txt_Porcentaje_Excencion.Text.Trim() != "")
        //{
        //    if (double.Parse(Txt_Porcentaje_Excencion.Text.Trim()) > 0)
        //        Quitar_Adeudos(double.Parse(Txt_Porcentaje_Excencion.Text.Trim()));
        //}
        Session["Cuota_Fija_Nueva"] = "";
        Session.Remove("Cuota_Fija_Nueva");
        Session["Cuota_Fija_Nueva"] = Txt_Total_Cuota_Fija.Text.Trim();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: imprimir orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:47:11 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Generar_Reporte((DataSet)Session["Ds_Reporte_Orden_Variacion"]);
            Guardar_Cambios((DataSet)Session["Ds_Cuenta_Datos_Orden"]);
            Generar_Reporte((DataSet)Session["Ds_Cuenta_Datos_Orden"]);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: generar nueva orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:47:11 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                //if (!String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
                //    Estado_Botones(Const_Estado_Nuevo);
                //else
                //{
                Estado_Botones(Const_Estado_Inicial);
                Limpiar_Formulario();
                Session_Remove();
                Btn_Mostrar_Busqueda_Cuentas.Visible = true;
                if (Session["Opcion_Tipo_Orden"] != null)
                {
                    if (Session["Opcion_Tipo_Orden"].ToString().Contains("Traslado"))
                    {
                        Txt_Cuenta_Predial.Enabled = true;
                        Btn_Establecer_Cuenta_Predial.Visible = true;
                    }
                }

                //}

            }
            else if (Btn_Nuevo.AlternateText.Equals("Guardar"))
            {
                if (!String.IsNullOrEmpty(Hdn_Contrarecibo.Value))
                {
                    //if (!Validar_Historial_Ordenes_Variacion())
                    {
                        if (!Validar_Ordenes_Variacion_Directas_Cuenta())
                        {
                            if (Validar_Orden_Variacion())
                                Generar_Orden(false);
                            else
                            {
                                Lbl_Encabezado_Error.Text = "Es Necesario:";
                            }
                        }

                        else
                        {
                            Img_Error.Visible = false;
                            Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación Directa pendiente Por Validar</br>";
                        }
                    }
                    //else
                    //{
                    //    Img_Error.Visible = false;
                    //    Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación pendiente Por Validar</br>";
                    //}
                }
                else
                {
                    if (!String.IsNullOrEmpty(Hdn_Cuenta_ID_Temp.Value))
                    {
                        if (Validar_Orden_Variacion())
                            Generar_Orden(false);

                    }
                    else
                        Mensaje_Error("No hay ningun contrarecibo seleccionado");
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: modificar orden de variacion
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 07/Agosto/2011 02:47:11 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (!String.IsNullOrEmpty(Hdn_Orden_Variacion.Value))
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    //if (!Validar_Historial_Ordenes_Variacion())
                    {
                        if (!Validar_Ordenes_Variacion_Directas_Cuenta())
                        {
                            if (Validar_Orden_Variacion())
                                Generar_Orden(true);
                            else
                            {
                                Lbl_Encabezado_Error.Text = "Es Necesario:";
                            }
                        }
                        else
                        {
                            Img_Error.Visible = false;
                            Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación Directa pendiente Por Validar</br>";
                        }
                    }
                    //else
                    //{
                    //    Img_Error.Visible = false;
                    //    Lbl_Mensaje_Error.Text += "La Cuenta Predial indicada ya tiene una Orden de Variación pendiente Por Validar</br>";
                    //}
                }
            }
            else
            {
                Mensaje_Error("Favor de seleccionar una Orden de variacion");
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Mostrar_Tasas_Diferencias_Click
    ///DESCRIPCIÓN: Obtener datos de busqueda avanzada de tasas
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2011 06:29:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Mostrar_Tasas_Diferencias_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Limpiar_Session_Agregar_Diferencias();
            Cargar_Grid_Diferencias(0);
            Calcular_Resumen();
        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }
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
        String Tipo_Orden_Variacion = "";
        try
        {
            if (Btn_Salir.AlternateText.Equals("Busqueda"))
            {
                Session_Remove();
                Limpiar_Formulario();
                Estado_Botones(Const_Estado_Inicial);
                Estado_Botones(Const_Estado_Busqueda_Cuenta);
            }
            else if (Btn_Salir.AlternateText.Equals("Inicio"))
            {
                if (Session["Opcion_Tipo_Orden"] != null)
                    Tipo_Orden_Variacion = Session["Opcion_Tipo_Orden"].ToString();
                if (Tipo_Orden_Variacion.Contains("Traslado"))
                    Response.Redirect("../Predial/Frm_Ope_Pre_Traslado.aspx");
                else
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Session_Remove();
                Limpiar_Formulario();
                Estado_Botones(Const_Estado_Inicial);
            }
            Btn_Resumen_Cuenta.Attributes.Remove("onclick");
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    private void Cargar_Grid_Historial_Observaciones(int Pagina)
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Ordenes_Variacion.P_Orden_Variacion_ID = Hdn_Orden_Variacion.Value;
        Ordenes_Variacion.P_Generar_Orden_Anio = Hdn_Orden_Variacion_Anio.Value;
        Ordenes_Variacion.P_Observaciones_No_Orden_Variacion = Hdn_Orden_Variacion.Value;
        Ordenes_Variacion.P_Año = Convert.ToInt16(Hdn_Orden_Variacion_Anio.Value);
        Ordenes_Variacion.Consultar_Ordenes_Variacion();
        Grid_Historial_Observaciones.DataSource = Ordenes_Variacion.P_Dt_Observaciones;
        Grid_Historial_Observaciones.PageIndex = Pagina;
        Grid_Historial_Observaciones.DataBind();
    }

    protected void Grid_Observaciones_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Historial_Observaciones.SelectedIndex = (-1);
            Cargar_Grid_Historial_Observaciones(e.NewPageIndex);
        }
        catch (Exception Ex)
        {
            Lbl_Encabezado_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Text = "";
        }
    }
    #endregion


    #endregion
    protected void Chk_Cuota_Fija_CheckedChanged(object sender, EventArgs e)
    {
        String Comprobar = "";
        Btn_Calcular_Cuota_Fija.Style.Value = "display:inline;";
        //if (!String.IsNullOrEmpty(Hdn_Respuesta_Confirmacion.Value))
        //{
        //    if (Convert.ToBoolean(Hdn_Respuesta_Confirmacion.Value))
        //    {
        if (Session["Quitar_Cuota_Fija"] != null)
        {
            Comprobar = Session["Quitar_Cuota_Fija"].ToString();
            if (Comprobar == "PEDIR_DATOS")
            {
                Lbl_Defuncion.Visible = true;
                Txt_Fecha_Def.Visible = true;
                Btn_CE_Fecha_Defuncion.Visible = true;
                Txt_Fecha_Def.Focus();
                CalendarExtender1.EnableViewState = true;
            }
            //}
            //}

        }
        if (Chk_Cuota_Fija.Checked)
        {
            Session["Cuota_Fija_Nueva"] = "";
            Session.Remove("Cuota_Fija_Nueva");
            Session["Cuota_Fija_Nueva"] = Txt_Total_Cuota_Fija.Text.Trim();
        }
        Hdn_Respuesta_Confirmacion.Value = "";


        //if (Session["Quitar_Cuota_Fija"] != null)
        //{
        //    Comprobar = Session["Quitar_Cuota_Fija"].ToString();
        //    if (Comprobar == "PEDIR_DATOS")
        //    {
        //        Txt_Fecha_Def.Focus();
        //        CalendarExtender1.EnableViewState = true;
        //    }
        //}

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Chk_Beneficio_Completo_CheckedChanged
    ///DESCRIPCIÓN: Obtener datos de cuota minima del ano actual
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 26/Abr/2012 03:03:06 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Beneficio_Completo_CheckedChanged(object sender, EventArgs e)
    {
        double Dbl_Cuota_Minima_Aplicada = 0;
        double Dbl_Cuota_Minima = 0;
        int Periodo_Corriente = 1;
        if (!Chk_Beneficio_Completo.Checked)
        {
            Periodo_Corriente = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
            Dbl_Cuota_Minima = Convert.ToDouble(Txt_Cuota_Minima.Text);
            Dbl_Cuota_Minima_Aplicada = Convert.ToDouble((Dbl_Cuota_Minima / 6).ToString("#,###,##0.00")) * (6 - Periodo_Corriente);//se divide entre el numero de periodo en curso para sacar el proporcional de la cuota
            Txt_Cuota_Minima_Aplicar.Text = Dbl_Cuota_Minima_Aplicada.ToString("#,###,##0.00");
        }
        else
        {
            Txt_Cuota_Minima_Aplicar.Text = Txt_Cuota_Minima.Text;
        }
        Calcular_Excedentes();
    }

    protected void Txt_Fecha_Def_TextChanged(object sender, EventArgs e)
    {
        Periodos_Defuncion();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Periodos_Defuncion
    ///DESCRIPCIÓN: Se generan los periodos para dar de alta 
    ///             las diferencias de los adeudos desde la fecha 
    ///             de defuncion del contribuyente cuando se quita
    ///             un beneficio de tercera edad pensionados 
    ///             o jubilados
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/19/2011 09:37:13 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Periodos_Defuncion()
    {
        String No_Diferencia = "", Cuenta_Predial_ID = "", Importe = "", Tipo = "", Periodo = "", Tasa_ID = "", Valor_Fiscal = "", Tasa = "", Cuota_Bimestral = "", Tipo_Peridodo = "";
        Double V_F_1 = 0;
        Double V_F_2 = 0;
        String Dia = "", Mes = "", Anio = "";
        String Fecha;
        String Inicio_Periodo = "1";
        String Fin_Periodo = "6";
        DateTime Dte_Fecha;
        DateTime Dte_Fecha_Pago;
        Int32 Anios_Periodo = 0;
        Int32 Anio_Def = 0;
        DataTable Dt_Resultado = null;
        DataTable Dt_Adeudos = null;
        DataTable Dt_Det_Cuota_Fija = null;
        DataTable Dt_Orden_Variacion = null;
        DataTable Dt_Resultado_OV = null;
        Double Dbl_Importe = 0;
        Double Dbl_C_Bimestral = 0;
        Double Dbl_Vf = 0;
        Double Dbl_Tasa = 0;
        Double Monto_Pagado = 0;
        Double Excedente_Valor = 0;
        Int32 Anio_Final_Calculo;
        Int32 Anio_Inicial_Calculo;
        bool Inicial = true;
        bool Anio_Completo = false;
        double C_Minima = 0;
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            if (!String.IsNullOrEmpty(Txt_Fecha_Def.Text.Trim()))
            {
                Fecha = Txt_Fecha_Def.Text.Trim();
                Dte_Fecha = Convert.ToDateTime(Fecha);
                Dia = Dte_Fecha.Day.ToString();
                Mes = Dte_Fecha.Month.ToString();
                Anio = Dte_Fecha.Year.ToString();
                if (Int32.TryParse(Anio, out Anio_Def))
                    Anios_Periodo = Const_Anio_Corriente - Anio_Def;
                Anio_Inicial_Calculo = Anio_Def;
                Anio_Final_Calculo = Const_Anio_Corriente;

                if (Anios_Periodo >= 5)
                {
                    Anios_Periodo = 5;
                    Anio_Inicial_Calculo = Const_Anio_Corriente - 5;
                }
                Resumen.P_Cuenta_Predial_ID = Txt_Cuenta_Predial.Text.Trim();
                Dt_Adeudos = Resumen.Consultar_Historial_Pagos();
                for (int cont = 0; cont <= Anios_Periodo; cont++)
                {
                    //if (((Anio_Inicial_Calculo.ToString() == Anio) && cont == 0) || Anios_Periodo < 5)
                    //{
                    foreach (DataRow Pago in Dt_Adeudos.Rows)
                    {
                        String Str_Anio = (Anio_Inicial_Calculo + cont).ToString();
                        if (Pago["PERIODO"].ToString().Contains(Str_Anio.Trim()))
                        {
                            Dte_Fecha_Pago = Convert.ToDateTime(Pago["FECHA"]);
                            Monto_Pagado = Convert.ToDouble(Pago["MONTO_CORRIENTE"]);
                            if (Dte_Fecha_Pago < Dte_Fecha)
                                Inicial = false;
                        }
                        else
                        {
                            Monto_Pagado = 0;
                        }
                        if (!Inicial)
                            break;
                    }
                    //}
                    if (Inicial)
                    {
                        Dt_Resultado_OV = Orden_Negocio.Consulta_Valor_Orden(Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", " + Cat_Pre_Cuentas_Predial.Campo_Efectos, Hdn_Cuenta_ID.Value, null, Cat_Pre_Cuentas_Predial.Campo_Efectos + " LIKE '%" + (Anio_Inicial_Calculo + cont).ToString() + "%' ", " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_No_Orden_Variacion + " DESC ");
                        if (Dt_Resultado_OV.Rows.Count > 0)//SI ES IGUAL A 1 Y PERIODO INICIAL IGUAL A 1
                        {
                            Inicio_Periodo = Dt_Resultado_OV.Rows[0]["EFECTOS"].ToString().Substring(0, 1);//1
                            if (Dt_Resultado_OV.Rows.Count == 1 && Inicio_Periodo == "1")
                            {
                                Insertar_Alta_Defuncion(Anio_Inicial_Calculo + cont, 1, 6, Anio_Inicial_Calculo + cont, Monto_Pagado);
                                Anio_Completo = true;
                            }
                            else if (Dt_Resultado_OV.Rows.Count == 1 && Inicio_Periodo != "1")
                            {
                                Insertar_Alta_Defuncion(Anio_Inicial_Calculo, 1, Convert.ToInt16(Inicio_Periodo), Anio_Inicial_Calculo + cont, -1);
                                Insertar_Alta_Defuncion(Anio_Inicial_Calculo + cont, Convert.ToInt16(Inicio_Periodo), 6, Anio_Inicial_Calculo + cont, Monto_Pagado);
                            }
                            else if (Dt_Resultado_OV.Rows.Count > 1)
                            {
                                for (int i = 0; i < Dt_Resultado_OV.Rows.Count; i++)//3
                                {
                                    if (i + 1 == Dt_Resultado_OV.Rows.Count)
                                    {
                                        Insertar_Alta_Defuncion(Anio_Inicial_Calculo, Convert.ToInt16(Inicio_Periodo), Convert.ToInt16(Dt_Resultado_OV.Rows[i + 1]["EFECTOS"].ToString().Substring(0, 1)), Anio_Inicial_Calculo + cont, -1);
                                        //CALCULAR VF Y TASA DEL ANO - 1
                                        //CALCULA PERIODO FALTANTE APARTIR DE LOS EFECTOS
                                        //SE CALCULA NORMAL
                                    }
                                    else
                                    {
                                        Insertar_Alta_Defuncion(Anio_Inicial_Calculo, 1, Convert.ToInt16(Inicio_Periodo), Anio_Inicial_Calculo + cont, -1);
                                        //SE CALCULA PERIODO APARTIR DE LOS EFECTOS DEL MOVIMIENTO ANTERIOR	
                                        //SI MOVIMIENTOS INICIAL
                                        //RESTAR PAGOS
                                        //AGREGAR DIFERENCIA
                                    }
                                }
                            }
                        }
                        else//Si no hay Actualizacion de valor para este anio
                        {
                            Insertar_Alta_Defuncion(Anio_Inicial_Calculo + cont, 1, 6, Anio_Inicial_Calculo + cont, Monto_Pagado);
                        }
                        Inicial = true;
                        Monto_Pagado = 0;
                    }
                    Inicial = true;
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Ocurrio un problema al generar los peridos para dar de alta");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Insertar_Alta_Defuncion
    ///DESCRIPCIÓN: Se generan los periodos para dar de alta 
    ///             las diferencias de los adeudos desde la fecha 
    ///             de defuncion del contribuyente cuando se quita
    ///             un beneficio de tercera edad pensionados 
    ///             o jubilados
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 26/Abril/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Insertar_Alta_Defuncion(int P_Anio, int P_Inicio, int P_Final, int Corriente, double P_Monto_Pagado)
    {
        String No_Diferencia = "", Cuenta_Predial_ID = "", Importe = "", Tipo = "", Periodo = "", Tasa_ID = "", Valor_Fiscal = "", Tasa = "", Cuota_Bimestral = "", Tipo_Peridodo = "";

        DataTable Dt_Resultado = null;
        Double Dbl_Importe = 0;
        Double Dbl_C_Bimestral = 0;
        Double Dbl_Vf = 0;
        Double Dbl_Tasa = 0;
        Double Monto_Pagado = 0;
        Double Excedente_Valor = 0;
        double C_Minima = 0;
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            if (P_Monto_Pagado >= 0)
                Monto_Pagado = P_Monto_Pagado;
            //Dt_Resultado = Orden_Negocio.Consulta_Valor_Orden(Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal, Hdn_Cuenta_ID.Value, (P_Anio).ToString());
            Dt_Resultado = Orden_Negocio.Consulta_Valor_Orden(Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal + ", " + Cat_Pre_Cuentas_Predial.Campo_Efectos, Hdn_Cuenta_ID.Value, null, "TO_NUMBER(substr(" + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3,4)) <= " + (P_Anio).ToString(), " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC ");
            if (Dt_Resultado.Rows.Count > 0)
                Valor_Fiscal = Dt_Resultado.Rows[0]["VALOR_FISCAL"].ToString();
            else
                Valor_Fiscal = Txt_Valor_Fiscal.Text.Trim();
            Dt_Resultado = Orden_Negocio.Consulta_Valor_Orden(Cat_Pre_Cuentas_Predial.Campo_Tasa_ID + ", " + Cat_Pre_Cuentas_Predial.Campo_Efectos, Hdn_Cuenta_ID.Value, null, "TO_NUMBER(substr(" + Cat_Pre_Cuentas_Predial.Campo_Efectos + ",3,4)) <= " + (P_Anio).ToString(), " ORDER BY " + Ope_Pre_Ordenes_Variacion.Campo_Anio + " DESC ");
            //Dt_Resultado = Orden_Negocio.Consulta_Valor_Orden(Cat_Pre_Cuentas_Predial.Campo_Tasa_ID, Hdn_Cuenta_ID.Value, (P_Anio).ToString());
            if (Dt_Resultado.Rows.Count > 0)
            {
                Tasa_ID = Dt_Resultado.Rows[0]["TASA_ID"].ToString();
                Tasa = Orden_Negocio.Consultar_Valor_Tasa(Tasa_ID);
            }
            else
            {
                Tasa_ID = Hdn_Tasa_ID.Value;
                Tasa = Txt_Tasa_Porcentaje.Text.Trim();
            }
            Periodo = P_Inicio + "/" + (Corriente).ToString() + " - " + P_Final + "/" + (Corriente).ToString();
            C_Minima = Convert.ToDouble(Cuotas_Minimas.Consultar_Cuota_Minima_Anio((P_Anio).ToString()));

            if (double.TryParse(Valor_Fiscal, out Dbl_Vf) && double.TryParse(Tasa, out Dbl_Tasa))
            {
                Dbl_Importe = Dbl_Vf * (Dbl_Tasa / 1000);
                Excedente_Valor = Consulta_Excedente_Valor(Dbl_Vf, P_Anio);
                Excedente_Valor = Excedente_Valor * (Dbl_Tasa / 1000);
                Dbl_C_Bimestral = Dbl_Importe / 6;
                if (P_Monto_Pagado >= 0)
                {
                    if (Monto_Pagado == 0)
                    {
                        Dbl_Importe = Dbl_Importe - C_Minima - Excedente_Valor;
                    }
                    else
                    {
                        Dbl_Importe = Dbl_Importe - Monto_Pagado;
                    }
                }
                Monto_Pagado = 0;
                Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
                Tipo = "ALTA";
                Tipo_Peridodo = "REZAGO";
                if ((P_Anio) == Const_Anio_Corriente)
                    Tipo_Peridodo = "CORRIENTE";
                Importe = Dbl_Importe.ToString();
                Cuota_Bimestral = Dbl_C_Bimestral.ToString();
                if (Dbl_Importe > 0)
                    Quitar_Agregar_Diferencia(No_Diferencia, Cuenta_Predial_ID, Importe, Tipo, Periodo, Tasa_ID, Valor_Fiscal, Tasa, Cuota_Bimestral, Tipo_Peridodo);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Ocurrio un problema al generar los peridos para dar de alta");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Periodo_Actual
    ///DESCRIPCIÓN: Se obtiene el numero de periodo actual con el mes actual
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/19/2011 09:41:54 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private String Periodo_Actual(String Mes)
    {
        String Str_Mes = Mes.ToUpper();
        double Dbl_Bimestre = 0;
        if (!Double.TryParse(Str_Mes, out Dbl_Bimestre))
        {
            if (Str_Mes.Contains("ENE"))
                Dbl_Bimestre = 1;
            if (Str_Mes.Contains("FEB"))
                Dbl_Bimestre = 2;
            if (Str_Mes.Contains("MAR"))
                Dbl_Bimestre = 3;
            if (Str_Mes.Contains("ABR"))
                Dbl_Bimestre = 4;
            if (Str_Mes.Contains("MAY"))
                Dbl_Bimestre = 5;
            if (Str_Mes.Contains("JUN"))
                Dbl_Bimestre = 6;
            if (Str_Mes.Contains("JUL"))
                Dbl_Bimestre = 7;
            if (Str_Mes.Contains("AGO"))
                Dbl_Bimestre = 8;
            if (Str_Mes.Contains("SEP"))
                Dbl_Bimestre = 9;
            if (Str_Mes.Contains("OCT"))
                Dbl_Bimestre = 10;
            if (Str_Mes.Contains("NOV"))
                Dbl_Bimestre = 11;
            if (Str_Mes.Contains("DIC"))
                Dbl_Bimestre = 12;
        }
        if (Dbl_Bimestre % 2 != 0)
            Dbl_Bimestre = (Dbl_Bimestre + 1) / 2;
        else
            Dbl_Bimestre = (Dbl_Bimestre) / 2;
        return Convert.ToInt32(Dbl_Bimestre).ToString();
    }
    protected void Btn_CE_Fecha_Defuncion_Click(object sender, ImageClickEventArgs e)
    {
        String Comprobar = "";
        if (Session["Quitar_Cuota_Fija"] != null)
        {
            Comprobar = Session["Quitar_Cuota_Fija"].ToString();
            if (Comprobar == "PEDIR_DATOS")
            {
                Periodos_Defuncion();
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validar_Ordenes_Variacion_Directas_Cuenta
    ///DESCRIPCIÓN          : Consulta las Órdenes de Variación y Determina si no hay órdenes ya generadas para la Cuenta indicada
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 28/Octubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Ordenes_Variacion_Directas_Cuenta()
    {
        Session["Validar_Ordenes_Variacion_Directas_Cuenta"] = null;
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Ordenes_Variacion;
        Boolean Ordenes_Variacion_Validadas = false;

        if (Hdn_Contrarecibo.Value == null || Hdn_Contrarecibo.Value == "")
        {
            Ordenes_Variacion.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
            //Ordenes_Variacion.P_Orden_Variacion_ID = Hdn_Orden_Variacion.Value;
            //Ordenes_Variacion.P_Año = DateTime.Now.Year;
            Ordenes_Variacion.P_Generar_Orden_Estatus = "IN ('POR VALIDAR','RECHAZADA')";
            Ordenes_Variacion.P_Contrarecibo = "NULL";
            Dt_Ordenes_Variacion = Ordenes_Variacion.Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta();
            if (Dt_Ordenes_Variacion != null)
            {
                if (Dt_Ordenes_Variacion.Rows.Count > 0)
                {
                    if (Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio] != System.DBNull.Value
                        && Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion] != System.DBNull.Value)
                    {
                        Ordenes_Variacion_Validadas = true;
                        Session["Validar_Ordenes_Variacion_Directas_Cuenta"] = Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Ordenes_Variacion.Campo_Estatus_Orden].ToString();
                    }
                    if (Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString() == Hdn_Orden_Variacion.Value.ToString())
                    {
                        Ordenes_Variacion_Validadas = false;
                    }
                }
            }
        }
        return Ordenes_Variacion_Validadas;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Validar_Historial_Ordenes_Variacion
    ///DESCRIPCIÓN          : Valida si existen Órdenes de Variación Pendientes por Validar.
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Boolean Validar_Historial_Ordenes_Variacion()
    {
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        DataTable Dt_Ordenes_Variacion;
        Boolean Orden_Variacion_Validada = false;

        if (Hdn_Contrarecibo.Value != null && Hdn_Contrarecibo.Value != "")
        {
            Ordenes_Variacion.P_Cuenta_Predial = Txt_Cuenta_Predial.Text;
            //Ordenes_Variacion.P_Orden_Variacion_ID = Hdn_Orden_Variacion.Value;
            //Ordenes_Variacion.P_Año = Convert.ToInt16(Hdn_Orden_Variacion_Anio.Value);
            Ordenes_Variacion.P_Generar_Orden_Estatus = "POR VALIDAR";
            Dt_Ordenes_Variacion = Ordenes_Variacion.Consultar_Historial_Estatus_Ordenes_Variacion_Cuenta();
            if (Dt_Ordenes_Variacion != null)
            {
                if (Dt_Ordenes_Variacion.Rows.Count > 0)
                {
                    if (Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio] != System.DBNull.Value
                        && Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion] != System.DBNull.Value)
                    {
                        if (Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_Anio].ToString() == Hdn_Orden_Variacion_Anio.Value
                            && Dt_Ordenes_Variacion.Rows[0][Ope_Pre_Orden_Variacion.Campo_No_Orden_Variacion].ToString() == Hdn_Orden_Variacion.Value)
                        {
                            Orden_Variacion_Validada = true;
                        }
                    }
                }
            }
        }
        return Orden_Variacion_Validada;
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
            if (Dr_Dato != null)
            {
                Dr_Dato.Close();
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

    protected void Chk_Mismo_Domicilio_CheckedChanged(object sender, EventArgs e)
    {

    }
    protected void Cmb_Usos_Predio_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}