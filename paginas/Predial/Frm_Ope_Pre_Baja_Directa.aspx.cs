using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Data.OracleClient;
using SharpContent.ApplicationBlocks.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Bitacora_Eventos;
using Operacion_Predial_Orden_Variacion.Negocio;
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using Presidencia.Catalogo_Cuotas_Minimas.Negocio;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using CrystalDecisions.Shared;
using Presidencia.Catalogo_Movimientos.Negocio;
using Presidencia.Catalogo_Descuentos_Predial.Negocio;
using Presidencia.Operacion.Predial_Tasas_Anuales.Negocio;
using Presidencia.Operacion_Resumen_Predio.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Recepcion_Pagos.Negocio;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Baja_Directa : System.Web.UI.Page
{
    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 2;
    private const int Const_Estado_Modificar = 3;
    public static int Const_Anio_Corriente = 0;
    private static String M_Cuenta_ID;
    private static Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
    private static DataTable Dt_Agregar_Co_Propietarios = new DataTable();
    //private static DataTable Dt_Agregar_Diferencias = new DataTable();
    private bool _Grid_Editable = false;
    protected bool Grid_Editable
    {
        get { return this._Grid_Editable; }
        set { this._Grid_Editable = value; }
    }
    #endregion

    #region Load/Init
    protected void Page_Load(object sender, EventArgs e)
    {
        string Ventana_Modal;
        try
        {
            if (Cls_Sessiones.Nombre_Empleado == null || Cls_Sessiones.Nombre_Empleado.Equals(String.Empty)) Response.Redirect("../Paginas_Generales/Frm_Apl_Login.aspx");

            if (!IsPostBack)
            {
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones                
                Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
                Session.Remove("ESTATUS_CUENTAS");
                Session.Remove("TIPO_CONTRIBUYENTE");
                Session.Remove("Dt_Agregar_Diferencias");
                //Se define el Año Corriente                
                //Si no tiene ningun valor se consulta de nuevo si no se salta esta consulta
                if (Const_Anio_Corriente <= 0)
                    Const_Anio_Corriente = Anio_Corriente.Consultar_Anio_Corriente();
                //si no hay resultado en la consulta del año se toma el actual del sistema
                if (Const_Anio_Corriente <= 0)
                    Const_Anio_Corriente = DateTime.Today.Year;
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de cuentas predial
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Cuentas.Attributes.Add("onclick", Ventana_Modal);
                //Cargar_Grid_Cuentas_canceladas(0);
                Cargar_Combo_Movimientos();
                Cargar_Grid_Bajas(0);
                Panel_Bajas.Visible = true;
                Panel_Datos.Visible = false;
            }
            Mensaje_Error();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Estado_Cuenta
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Estado de Cuenta con la ruta y parámtros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Estado_Cuenta()
    {
        string Ventana_Modal_Estado_Cuenta = "Abrir_Ventana_Estado_Cuenta('Ventanas_Emergentes/Resumen_Predial/Frm_Estado_Cuenta.aspx?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "', 'height=600,width=800,scrollbars=1');";
        Btn_Estado_Cuenta.Attributes.Add("onclick", Ventana_Modal_Estado_Cuenta);
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
        String Ventana_Modal = "Abrir_Resumen('Ventanas_Emergentes/Resumen_Predial/Frm_Adeudo_Diferencias.aspx";
        String Propiedades = ", 'resizable=no,status=no,width=580,height=500,scrollbars=yes');";
        Btn_Vista_Adeudos.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }
    private void Borrar_Ventana_Emergente_Adeudos()
    {
        Btn_Vista_Adeudos.Attributes.Clear();
    }
    #endregion

    #region Metodos/Generales

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
            //Consulta_Combos();
            Cargar_Combos();
            Estado_Botones(Const_Estado_Inicial);
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
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
        M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        Txt_Cuenta_Predial.Text = "";
        Hdn_Cuenta_ID.Value = null;
        Hdn_Propietario_ID.Value = null;
        Hdn_Excedente_Valor.Value = "";
        Txt_Cta_Origen.Text = "";
        Txt_Superficie_Construida.Text = "";
        Txt_Superficie_Total.Text = "";
        Txt_Estatus.Font.Bold = false;
        Txt_Colonia_Cuenta.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Catastral.Text = "";
        Txt_Nombre_Propietario.Text = "";
        Txt_Tipos_Predio.Text = "";
        Txt_Calle_Cuenta.Text = "";
        Txt_Valor_Fiscal.Text = "";
        Grid_Diferencias.DataSource = null;
        Grid_Adeudos_Editable.DataSource = null;
        Grid_Adeudos_Editable.DataBind();
        if (Cmb_Financiado.Items.Count > 0)
            Cmb_Financiado.SelectedIndex = 0;
        if (Cmb_Solicitante.Items.Count > 0)
            Cmb_Solicitante.SelectedIndex = 0;
        if (Cmb_Movimientos.Items.Count > 0)
            Cmb_Movimientos.SelectedIndex = 0;
        Txt_Fundamento.Text = "";
        Txt_Usos_Predio.Text = "";
        Txt_Estados_Predio.Text = "";
        Txt_Estatus.Text = "";
        Txt_Efectos.Text = "";

        Grid_Diferencias.DataSource = null;
        Grid_Diferencias.DataBind();

        Txt_Observaciones_Cuenta.Text = "";
        Session.Remove("Tabla_Adeudos_Editados");
        Session.Remove("Tabla_Adeudos");
        Session.Remove("Tabla_Periodos");
        Session.Remove("Dt_Agregar_Diferencias");
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
        Lbl_Error_Adeudos.Text = "";
    }
    private void Mensaje_Error_Adeudos(String P_Mensaje)
    {
        Lbl_Error_Adeudos.Text += P_Mensaje + "</br>";
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
            //Borrar sesion de la tabla de adeudos editada y cargar adeudos originales
            Session.Remove("Tabla_Adeudos_Editados");
            Session.Remove("Tabla_Adeudos");
            Session.Remove("Dt_Agregar_Diferencias");
            Llenar_Tabla_Adeudos(0);
            Cargar_Grid_Diferencias(0);
            Calcular_Resumen();
            Resumen_Grid_Diferencias();
            Btn_Mostrar_Tasas_Diferencias.Visible = false;
            Btn_Generar_Diferencias.Visible = true;
        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object.")
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Generar_Diferencias_Click
    ///DESCRIPCIÓN: Calcular Diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 03/Ago/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Generar_Diferencias_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Grid_Editable = false;
            Calcular_Bajas();

            Quitar_Adeudos();
            Btn_Mostrar_Tasas_Diferencias.Visible = true;
        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object.")
            {
                Mensaje_Error(Ex.Message);
            }
        }
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
            Ds_Cargar_combos = ((DataSet)Session["Ds_Consulta_Combos"]);
            Llenar_Combo_ID(Cmb_Financiado, Ds_Cargar_combos.Tables["Dt_Casos_Especiales_Financiamiento"]);
            Llenar_Combo_ID(Cmb_Solicitante, Ds_Cargar_combos.Tables["Dt_Casos_Especiales_Solicitante"]);
            //for (int i = 1; i <= 6; i++)
            //{
            //    Cmb_P_R_Bimestre_Final.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    Cmb_P_R_Bimestre_Inicial.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    Cmb_P_C_Bimestre_Final.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //    Cmb_P_C_Bimestre_Inicial.Items.Add(new ListItem(i.ToString(), i.ToString()));
            //}
            //Cmb_P_R_Bimestre_Final.SelectedIndex = 5;
            //Cmb_P_C_Bimestre_Final.SelectedIndex = 5;
            //Cmb_P_C_Anio.Items.Add(new ListItem(DateTime.Today.Year.ToString(), DateTime.Today.Year.ToString()));

            //for (int anio = Int32.Parse(DateTime.Now.Year.ToString()) - 1; anio >= 1980; anio--)
            //{
            //    Cmb_P_R_Anio_Inicial.Items.Add(new ListItem(anio.ToString(), anio.ToString()));
            //    Cmb_P_R_Anio_Final.Items.Add(new ListItem(anio.ToString(), anio.ToString()));
            //}
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
        Boolean Estado = false;
        switch (P_Estado)
        {
            case 0: //Estado inicial                

                Txt_Cuenta_Predial.Enabled = false;
                Btn_Mostrar_Busqueda_Cuentas.Enabled = false;
                Btn_Estado_Cuenta.Enabled = false;
                Btn_Vista_Adeudos.Enabled = false;
                //Btn_Agregar_P_Corriente.Enabled = false;
                //Btn_Agregar_P_Regazo.Enabled = false;
                Cmb_Movimientos.Enabled = false;
                Grid_Diferencias.Enabled = false;
                Grid_Editable = false;
                Estado = false;

                Txt_Hasta_Periodo_Corriente.Text = "0";
                Txt_Desde_Periodo_Corriente.Text = "0";
                Txt_Hasta_Periodo_Regazo.Text = "0";
                Txt_Desde_Periodo_Regazo.Text = "0";
                Txt_Alta_Periodo_Corriente.Text = "0";
                Txt_Baja_Periodo_Corriente.Text = "0";
                Txt_Baja_Periodo_Regazo.Text = "0";
                Txt_Alta_Periodo_Regazo.Text = "0";
                Lbl_P_C_Anio_Final.Text = "0";
                Lbl_P_C_Anio_Inicio.Text = "0";
                Txt_Desde_Anio_Periodo_Corriente.Text = "0";
                Txt_Hasta_Anio_Periodo_Corriente.Text = "0";
                //Cmb_Financiado.SelectedIndex = 0;
                //Cmb_Movimientos.SelectedIndex = 0;
                Txt_Fundamento.Text = "";
                Btn_Mostrar_Tasas_Diferencias.Visible = false;
                Btn_Generar_Diferencias.Visible = false;
                Btn_Buscar.AlternateText = "Buscar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Nuevo.Visible = true;
                Btn_Salir.AlternateText = "Inicio";
                Btn_Buscar.ToolTip = "Buscar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Buscar.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Nuevo.OnClientClick = "";
                Btn_Nuevo.Visible = true;
                Txt_Estatus.Enabled = false;

                Panel_Bajas.Visible = true;
                Panel_Datos.Visible = false;
                Cmb_Financiado.Enabled = false;
                Cmb_Solicitante.Enabled = false;
                Borrar_Ventana_Emergente_Adeudos();
                break;

            case 2: //Nuevo

                Estado = false;
                Txt_Cuenta_Predial.Enabled = false;
                Btn_Mostrar_Busqueda_Cuentas.Enabled = true;
                Btn_Estado_Cuenta.Enabled = true;
                Btn_Vista_Adeudos.Enabled = true;
                //Btn_Agregar_P_Corriente.Enabled = true;
                //Btn_Agregar_P_Regazo.Enabled = true;
                Cmb_Movimientos.Enabled = true;
                Grid_Diferencias.Enabled = true;
                Txt_Cuenta_Predial.Focus();
                Btn_Salir.Enabled = true;
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Txt_Estatus.Enabled = false;
                Txt_Cuenta_Predial.Focus();
                Grid_Editable = true;
                Panel_Bajas.Visible = false;
                Panel_Datos.Visible = true;
                Cmb_Financiado.Enabled = true;
                Cmb_Solicitante.Enabled = true;
                Cargar_Ventana_Emergente_Adeudo_Diferencias();
                break;

            case 3: //Mod
                Estado = false;
                Txt_Estatus.Enabled = true;
                Txt_Estatus.ReadOnly = true;
                Txt_Estatus.Font.Bold = true;
                Txt_Estatus.Text = "VIGENTE";
                Txt_Cuenta_Predial.Enabled = false;
                Btn_Mostrar_Busqueda_Cuentas.Enabled = false;
                Btn_Estado_Cuenta.Enabled = false;
                Btn_Vista_Adeudos.Enabled = false;
                //Btn_Agregar_P_Corriente.Enabled = false;
                //Btn_Agregar_P_Regazo.Enabled = false;
                Cmb_Movimientos.Enabled = false;
                Grid_Diferencias.Enabled = false;
                Btn_Salir.Enabled = true;
                Btn_Nuevo.Visible = false;
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Txt_Cuenta_Predial.Focus();
                Grid_Editable = true;
                Panel_Bajas.Visible = false;
                Panel_Datos.Visible = true;
                Cmb_Financiado.Enabled = true;
                Cmb_Solicitante.Enabled = true;
                Cargar_Ventana_Emergente_Adeudo_Diferencias();
                break;
        }

        Txt_Cta_Origen.Enabled = Estado;
        Txt_Superficie_Construida.Enabled = Estado;
        Txt_Superficie_Total.Enabled = Estado;
        Txt_Calle_Cuenta.Enabled = Estado;
        Txt_Colonia_Cuenta.Enabled = Estado;
        Txt_No_Exterior.Enabled = Estado;
        Txt_No_Interior.Enabled = Estado;
        Txt_Catastral.Enabled = Estado;
        Txt_Nombre_Propietario.Enabled = Estado;

        Txt_Tipos_Predio.Enabled = Estado;
        Txt_Usos_Predio.Enabled = Estado;
        Txt_Estados_Predio.Enabled = Estado;
        Txt_Efectos.Enabled = Estado;

        Txt_Excedente_Valor_Total.Enabled = false;
        Txt_Excedente_Construccion_Total.Enabled = false;
        Txt_Costo_M2.Enabled = false;
        Txt_Dif_Construccion.Enabled = false;
    }

    #endregion

    #region Eventos Botones
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
            if (Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]))
            {
                if (Session["CUENTA_PREDIAL_ID"].ToString() != "" && Session["CUENTA_PREDIAL_ID"] != null)
                {
                    Cuenta_Predial_ID = Session["CUENTA_PREDIAL_ID"].ToString();
                    Cuenta_Predial = Session["CUENTA_PREDIAL"].ToString();
                    Txt_Cuenta_Predial.Text = Cuenta_Predial;
                    M_Cuenta_ID = Cuenta_Predial_ID;
                    Txt_Cuenta_Predial_TextChanged(null, EventArgs.Empty);
                    Llenar_Tabla_Adeudos(0);
                }
            }
            Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
            Session.Remove("CUENTA_PREDIAL_ID");
        }
        catch (Exception Ex)
        {
            if (Ex.Message != "Object reference not set to an instance of an object.")
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
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                if (Btn_Salir.AlternateText.Equals("Regresar")
                    || Btn_Salir.AlternateText.Equals("Cancelar"))
                {
                    Limpiar_Todo();
                    Estado_Botones(Const_Estado_Inicial);
                }
                else
                {
                    Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                }
            }
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
                if (Validar_Mes_Descuento())
                {
                    Estado_Botones(Const_Estado_Nuevo);
                    Limpiar_Todo();
                    Btn_Nuevo.OnClientClick = "return confirm('¿Está seguro de Continuar?');";
                }
                else
                {
                    Mensaje_Error("No puede aplicar Bajas Directas ya que no hay Descuentos para este Mes.");
                }
            }
            else
            {
                if (Validar_Campos_Obligatorios())
                {
                    Alta_Bloqueo(0);
                    Btn_Nuevo.OnClientClick = "";
                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion

    #region Metodos ABC
    private void Alta_Bloqueo(int modo)
    {
        DataTable Dt_Diferencias_Header = new DataTable();
        DataTable Dt_Agregar_Diferencias = new DataTable();
        String Cuota_Fija_ID;
        DataRow[] Dr_No_cuota_Fija = null;
        double Cuota_Minima = 0;
        double Exedente_Construccion = 0;
        double Excedente_Valor = 0;
        double Total_Cuota_Fija = 0;
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Cls_Ope_Pre_Parametros_Negocio Anio_Corriente = new Cls_Ope_Pre_Parametros_Negocio();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();

            DataTable Dt_Diferencias = new DataTable();
            DataSet Dt_Imprimir_Reactivacion = new DataSet();

            String Grupo_Movimiento_ID = "";
            String Tipo_Predio_ID = "";

            if (Session["Dt_Agregar_Diferencias"] != null)
                Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            else
                Dt_Agregar_Diferencias = Formar_Tabla_Diferencias();

            Orden_Variacion.P_Generar_Orden_Anio = DateTime.Now.Year.ToString();
            Orden_Variacion.P_Generar_Orden_Cuenta_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Generar_Orden_Obserbaciones = Txt_Observaciones_Cuenta.Text.ToUpper();
            Orden_Variacion.P_Generar_Orden_Movimiento_ID = Cmb_Movimientos.SelectedItem.Value;
            Orden_Variacion.P_Generar_Orden_Estatus = "ACEPTADA";
            Orden_Variacion.P_Generar_Orden_Cuenta_ID = Hdn_Cuenta_ID.Value;
            //Segmento de generacion de cuota fija

            if (Cmb_Solicitante.SelectedIndex > 0)
                Orden_Variacion.P_Cuota_Fija_Caso_Especial = Cmb_Solicitante.SelectedValue;
            if (Cmb_Financiado.SelectedIndex > 0)
                Orden_Variacion.P_Cuota_Fija_Caso_Especial = Cmb_Financiado.SelectedValue;
            //Calculo de CF
            Cuota_Minima = (Convert.ToDouble(Obtener_Dato_Consulta("CUOTA", Cat_Pre_Cuotas_Minimas.Tabla_Cat_Pre_Cuotas_Minimas, Cat_Pre_Cuotas_Minimas.Campo_Año + " = 2012", "")));
            Exedente_Construccion = (Convert.ToDouble(Txt_Excedente_Construccion_Total.Text.Trim()));
            Excedente_Valor = (Convert.ToDouble(Txt_Excedente_Valor_Total.Text.Trim()));
            if (Exedente_Construccion >= Excedente_Valor)
            {
                Total_Cuota_Fija = (Cuota_Minima + Exedente_Construccion);
            }
            else if (Excedente_Valor > Exedente_Construccion)
            {
                Total_Cuota_Fija = (Cuota_Minima + Excedente_Valor);
            }
            Orden_Variacion.P_Cuota_Fija_Cuota_Minima = Cuota_Minima.ToString("#,###,#0.00");
            Orden_Variacion.P_Cuota_Fija_Excedente_Cons = Txt_Dif_Construccion.Text.Trim();
            Orden_Variacion.P_Cuota_Fija_Excedente_Cons_Total = Exedente_Construccion.ToString("#,###,#0.00");
            Orden_Variacion.P_Cuota_Fija_Excedente_Valor = Hdn_Excedente_Valor.Value;
            Orden_Variacion.P_Cuota_Fija_Excedente_Valor_Total = Excedente_Valor.ToString("#,###,#0.00");
            Orden_Variacion.P_Cuota_Fija_Tasa_ID = Hdn_Tasa_ID.Value;
            Orden_Variacion.P_Cuota_Fija_Tasa_Valor = Txt_Tasa_Porcentaje.Text.Trim();

            Orden_Variacion.P_Cuota_Fija_Total = Total_Cuota_Fija.ToString("#,###,#0.00");

            Cuota_Fija_ID = Orden_Variacion.Alta_Beneficio_Couta_Fija();
            Dr_No_cuota_Fija = Orden_Variacion.P_Generar_Orden_Dt_Detalles.Select("CAMPO = 'NO_CUOTA_FIJA'");
            if (Dr_No_cuota_Fija.Length > 0)
                Orden_Variacion.P_Generar_Orden_Dt_Detalles.Rows.Remove(Dr_No_cuota_Fija[0]);
            Dr_No_cuota_Fija = Orden_Variacion.P_Generar_Orden_Dt_Detalles.Select("CAMPO = 'CUOTA_FIJA'");
            if (Dr_No_cuota_Fija.Length > 0)
                Orden_Variacion.P_Generar_Orden_Dt_Detalles.Rows.Remove(Dr_No_cuota_Fija[0]);
            Orden_Variacion.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija, Cuota_Fija_ID);
            Orden_Variacion.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija, "SI");


            Orden_Variacion.P_Dt_Diferencias = Dt_Agregar_Diferencias;
            String Orden_Reactivacion = Orden_Variacion.Generar_Orden_Variacion();

            Orden_Variacion.P_Orden_Variacion_ID = Orden_Reactivacion;
            Orden_Variacion.P_Año = DateTime.Now.Year;
            Grupo_Movimiento_ID = Obtener_Dato_Consulta(Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID, Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + ", " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos, Cat_Pre_Grupos_Movimiento.Tabla_Cat_Pre_Grupos_Movimiento + "." + Cat_Pre_Grupos_Movimiento.Campo_Grupo_Movimiento_ID + " = " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Grupo_Id + " AND " + Cat_Pre_Movimientos.Tabla_Cat_Pre_Movimientos + "." + Cat_Pre_Movimientos.Campo_Movimiento_ID + " = '" + Cmb_Movimientos.SelectedItem.Value + "'", "");
            Tipo_Predio_ID = Obtener_Dato_Consulta(Cat_Pre_Cuentas_Predial.Campo_Tipo_Predio_ID, Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas, Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " = '" + Hdn_Cuenta_ID.Value + "'", "");
            Int32 No_Nota_Consecutivo = Convert.ToInt32(Obtener_Dato_Consulta("NVL(MAX(" + Ope_Pre_Orden_Variacion.Campo_No_Nota + "), 0) + 1", Ope_Pre_Orden_Variacion.Tabla_Ope_Pre_Orden_Variacion, Ope_Pre_Orden_Variacion.Campo_Anio + " || " + Ope_Pre_Orden_Variacion.Campo_Grupo_Movimiento_ID + " || " + Ope_Pre_Orden_Variacion.Campo_Tipo_Predio_ID + " = '" + DateTime.Now.Year.ToString() + Grupo_Movimiento_ID + Tipo_Predio_ID + "' ORDER BY " + Ope_Pre_Orden_Variacion.Campo_No_Nota, "1"));
            Int32 No_Nota_Inicila = Convert.ToInt32(Obtener_Dato_Consulta("NVL(" + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Folio_Inicial + ", 0)", Cat_Pre_Grupos_Movimiento_Detalles.Tabla_Cat_Pre_Grupos_Movimiento_Detalles, Cat_Pre_Grupos_Movimiento_Detalles.Campo_Año + " = " + DateTime.Now.Year.ToString() + " AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Grupo_Movimiento_ID + " = '" + Grupo_Movimiento_ID + "' AND " + Cat_Pre_Grupos_Movimiento_Detalles.Campo_Tipo_Predio_ID + " = '" + Tipo_Predio_ID + "'", "1"));
            Orden_Variacion.P_No_Nota = No_Nota_Consecutivo > No_Nota_Inicila ? No_Nota_Consecutivo : No_Nota_Inicila;
            Orden_Variacion.P_Grupo_Movimiento_ID = Grupo_Movimiento_ID;
            Orden_Variacion.P_Tipo_Predio_ID = Tipo_Predio_ID;
            Orden_Variacion.P_Fecha_Nota = DateTime.Now;
            Orden_Variacion.P_No_Nota_Impreso = "NO";
            Orden_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Orden_Variacion.Modificar_Orden_Variacion();

            Dt_Diferencias.Columns.Add(new DataColumn("CUOTA_ANUAL", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));
            Dt_Diferencias.Columns.Add(new DataColumn("AÑO", typeof(int)));
            Dt_Diferencias.Columns.Add(new DataColumn("ALTA_BAJA", typeof(String)));

            String Periodo = "";
            Decimal Sum_Adeudos_Año = 0;
            Decimal Sum_Adeudos_Periodo = 0;
            int Cont_Cuotas_Minimas_Año = 0;
            int Cont_Cuotas_Minimas_Periodo = 0;
            int Cont_Adeudos_Año = 0;
            int Cont_Adeudos_Periodo = 0;
            int Desde_Bimestre = 0;
            int Hasta_Bimestre = 0;
            int Cont_Bimestres = 0;
            int Año_Periodo = 0;
            int Signo = 1;
            Boolean Periodo_Corriente_Validado = false;
            Boolean Periodo_Rezago_Validado = false;
            Decimal Importe_Rezago = 0;
            Decimal Cuota_Fija = 0;
            Decimal Cuota_Minima_Año = 0;
            Decimal Cuota_Anual = 0;
            Boolean Nueva_Cuota_Fija = false;
            Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
            Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
            DataTable Dt_Adeudos_Cuenta = new DataTable();
            DataRow Dr;

            //if (Hdn_No_Cuota_Fija_Nuevo.Value.Trim() != ""
            //    && Hdn_No_Cuota_Fija_Nuevo.Value.Trim() != Hdn_No_Cuota_Fija_Anterior.Value.Trim())
            //{
            //    Nueva_Cuota_Fija = true;
            //}

            foreach (GridViewRow Dr_Diferencias in Grid_Diferencias.Rows)
            {
                if (Dr_Diferencias.Cells[1] != null)
                {
                    if (Dr_Diferencias.Cells[1].Text.Trim() == "ALTA")
                    {
                        Signo = 1;
                    }
                    else
                    {
                        if (Dr_Diferencias.Cells[1].Text.Trim() == "BAJA")
                        {
                            Signo = -1;
                        }
                    }
                }

                Cuota_Anual = Convert.ToDecimal((Dr_Diferencias.Cells[5].Text.Replace("$", "").Replace(",", "")));
                Año_Periodo = Convert.ToInt32(Dr_Diferencias.Cells[0].Text.Substring(Dr_Diferencias.Cells[0].Text.Length - 4));
                Cuota_Minima_Año = Cuotas_Minimas.Consultar_Cuota_Minima_Anio(Año_Periodo.ToString());
                Importe_Rezago = Convert.ToDecimal((Dr_Diferencias.Cells[4].Text.Replace("$", "").Replace(",", "")));
                Periodo = Obtener_Periodos_Bimestre(Dr_Diferencias.Cells[0].Text.Trim(), out Periodo_Corriente_Validado, out Periodo_Rezago_Validado);
                if (Periodo.Trim() != "")
                {
                    Desde_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(0));
                    Hasta_Bimestre = Convert.ToInt32(Periodo.Split('-').GetValue(1));

                    //Cuotas_Minimas_Encontradas_Año = false;
                    Cont_Cuotas_Minimas_Año = 0;
                    Cont_Adeudos_Año = 0;
                    Sum_Adeudos_Año = 0;
                    //Cuotas_Minimas_Encontradas_Periodo = false;
                    Cont_Cuotas_Minimas_Periodo = 0;
                    Cont_Adeudos_Periodo = 0;
                    Sum_Adeudos_Periodo = 0;

                    Dt_Adeudos_Cuenta = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Hdn_Cuenta_ID.Value, null, Año_Periodo, Año_Periodo);
                    if (Dt_Adeudos_Cuenta != null)
                    {
                        if (Dt_Adeudos_Cuenta.Rows.Count > 0)
                        {
                            //Contador de los Adeudos/Cuotas del Año
                            for (Cont_Bimestres = 1; Cont_Bimestres <= 6; Cont_Bimestres++)
                            {
                                if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                {
                                    if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                    {
                                        Cont_Cuotas_Minimas_Año += 1;
                                    }
                                    if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                    {
                                        Cont_Adeudos_Año += 1;
                                        Sum_Adeudos_Año += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                    }
                                }
                            }
                            //Contador de los Adeudos/Cuotas del Periodo indicado
                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                            {
                                if (Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres] != System.DBNull.Value)
                                {
                                    if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) == Cuota_Minima_Año)
                                    {
                                        Cont_Cuotas_Minimas_Periodo += 1;
                                    }
                                    if (Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]) != 0)
                                    {
                                        Cont_Adeudos_Periodo += 1;
                                        Sum_Adeudos_Periodo += Convert.ToDecimal(Dt_Adeudos_Cuenta.Rows[0][Cont_Bimestres]);
                                    }
                                }
                            }
                        }
                    }

                    Dr = Dt_Diferencias.NewRow();
                    Dr["CUOTA_ANUAL"] = Cuota_Anual;
                    Dr["AÑO"] = Año_Periodo;
                    Dr["ALTA_BAJA"] = Dr_Diferencias.Cells[1].Text.Trim();
                    //VALIDACIONES PARA CASOS DE CUOTAS MÍNIMAS Y APLICACIÓN DE ADEUDOS
                    //if (Cont_Cuotas_Minimas_Periodo == 1 && Importe_Rezago != Cuota_Minima_Año && !Nueva_Cuota_Fija)
                    //{
                    //    Dr["ALTA_BAJA"] = "SOB";
                    //    //SUMA LA CUOTA MÍNIMA AL IMPORTE Y EL RESULTADO LO PRORRATEA EN EL PERIODO INDICADO
                    //    for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                    //    {
                    //        Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = (Importe_Rezago + Cuota_Minima_Año) / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo;
                    //    }
                    //}
                    //else
                    {
                        if (((Importe_Rezago == Cuota_Minima_Año)
                                || ((Hasta_Bimestre - Desde_Bimestre + 1) == 6
                                    && ((Sum_Adeudos_Periodo - Importe_Rezago) == Cuota_Minima_Año || (Sum_Adeudos_Periodo + Importe_Rezago) == Cuota_Minima_Año)))
                            && !Nueva_Cuota_Fija
                            && !(Importe_Rezago == Cuota_Minima_Año && (Hasta_Bimestre - Desde_Bimestre + 1) == 1))
                        {
                            //APLICA LA CUOTA MÍNIMA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CEROS
                            if (Importe_Rezago == Cuota_Minima_Año || Signo > 0)
                            {
                                Dr["ALTA_BAJA"] = "SOB1";
                            }
                            else
                            {
                                Dr["ALTA_BAJA"] = "SOB";
                            }
                            for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                            {
                                if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                {
                                    Cuota_Minima_Año = 0;
                                }
                                Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Cuota_Minima_Año * Signo;
                            }
                        }
                        else
                        {
                            if (Nueva_Cuota_Fija && Signo < 0)
                            {
                                //APLICA LA CUOTA FIJA EN EL PRIMER BIMESTRE INDICADO, EL RESTO DE BIMESTRES LOS DEJA EN CERO
                                Dr["ALTA_BAJA"] = "SOB";
                                Cuota_Fija = Importe_Rezago;
                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                {
                                    if (Cont_Bimestres > Desde_Bimestre && Cuota_Minima_Año != 0)
                                    {
                                        Cuota_Fija = 0;
                                    }
                                    Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Cuota_Fija;
                                }
                            }
                            else
                            {
                                //PRORRATEA EL IMPORTE EN EL PERIODO INDICADO
                                for (Cont_Bimestres = Desde_Bimestre; Cont_Bimestres <= Hasta_Bimestre; Cont_Bimestres++)
                                {
                                    Dr["BIMESTRE_" + Cont_Bimestres.ToString()] = Importe_Rezago / (Hasta_Bimestre - Desde_Bimestre + 1) * Signo;
                                }
                            }
                        }
                    }
                    Dt_Diferencias.Rows.Add(Dr);
                }
            }
            Orden_Variacion.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Orden_Variacion.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Orden_Variacion.P_Dt_Diferencias = Dt_Diferencias;
            Orden_Variacion.Aplicar_Variacion_Diferencias();

            Cuentas_Predial.P_Adeudo_Predial_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Cuentas_Predial.Validar_Estatus_Adeudos();
            Cuentas_Predial.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            Cuentas_Predial.P_Cuota_Fija = "SI";
            Cuentas_Predial.P_No_Cuota_Fija = Cuota_Fija_ID;
            Cuentas_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            if (!String.IsNullOrEmpty(Cuota_Fija_ID))
                Cuentas_Predial.Modifcar_Cuenta();
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Bajas Directas", "alert('Las Diferencias se guardaron Correctamente');", true);

            Orden_Variacion.P_Generar_Orden_No_Orden = Orden_Reactivacion;
            Orden_Variacion.P_Generar_Orden_Anio = Anio_Corriente.Consultar_Anio_Corriente().ToString();
            Dt_Imprimir_Reactivacion = Orden_Variacion.Consulta_Datos_Reporte();
            String Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Orden_Variacion_Bajas_Directas.rpt";
            // Se crea el nombre del reporte
            String Nombre_Reporte = "Orden_Variacion_Bajas_Directas" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));
            String Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            //Agregar Campos
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
            Dt_Imprimir_Reactivacion.Tables.Add(Dt_Diferencias_Header.Copy());
            Dt_Imprimir_Reactivacion.Tables[4].TableName = "Dt_Diferencias_Header";

            if (Dt_Agregar_Diferencias != null)
            {
                if (Dt_Agregar_Diferencias.Rows.Count > 0)
                    Dt_Agregar_Diferencias = Acomodar_Periodos(Dt_Agregar_Diferencias);

                Dt_Imprimir_Reactivacion.Tables.Add(Dt_Agregar_Diferencias.Copy());
                Dt_Imprimir_Reactivacion.Tables[5].TableName = "Dt_Diferencias";
            }
            else
            {
                Dt_Imprimir_Reactivacion.Tables.Add(new DataTable());
                Dt_Imprimir_Reactivacion.Tables[5].TableName = "Dt_Diferencias";
            }
            Dt_Imprimir_Reactivacion.AcceptChanges();
            Generar_Reporte(ref Dt_Imprimir_Reactivacion, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
            Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");

            Cargar_Grid_Bajas(0);
            Limpiar_Todo();
            Estado_Botones(Const_Estado_Inicial);
            //Cargar_Grid_Cuentas_canceladas(0);
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Alta Bloqueo Error:[ " + Ex.Message + "]");
        }
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
    #endregion

    #region Eventos Cajas
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
            Hdn_Cuenta_ID.Value = "";
            M_Cuenta_ID = Txt_Cuenta_Predial.Text.Trim();
            M_Orden_Negocio.P_Cuenta_Predial = M_Cuenta_ID;
            M_Orden_Negocio.P_Contrarecibo = null;
            Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta_Sin_Contrarecibo();
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Limpiar_Todo();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
                Cargar_Datos();
                Session["Cuenta_Predial_ID_Adeudos"] = Hdn_Cuenta_ID.Value;
                Btn_Estado_Cuenta.Enabled = true;
                Btn_Vista_Adeudos.Enabled = true;
            }
            else
            {
                Estado_Botones(Const_Estado_Inicial);
                Limpiar_Todo();
                Btn_Estado_Cuenta.Enabled = false;
                Btn_Vista_Adeudos.Enabled = false;
                Mensaje_Error("No se encontraron datos relacionados con la búsqueda");
            }
        }
        catch (Exception Ex)
        {
            Btn_Estado_Cuenta.Enabled = false;
            Btn_Vista_Adeudos.Enabled = false;
            Mensaje_Error(Ex.Message);
        }
        finally
        {
            Cargar_Ventana_Emergente_Estado_Cuenta();
            Cargar_Ventana_Emergente_Adeudo_Diferencias();
        }
    }
    #endregion

    #region Metodos Operacion
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
            Cargar_Generales_Cuenta(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Generales"]);
            Cargar_Datos_Propietario(((DataSet)Session["Ds_Cuenta_Datos"]).Tables["Dt_Propietarios"]);
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
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
    private void Cargar_Generales_Cuenta(DataTable dataTable)
    {
        try
        {
            //Asignacion de valores a Objeto de Negocio y cajas de texto            
            M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Hdn_Cuenta_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
            M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString();

            Txt_Cta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString();
            if (dataTable.Rows[0]["TIPO_PREDIO_DESCRIPCION"].ToString() != string.Empty)
            {
                Txt_Tipos_Predio.Text = dataTable.Rows[0]["TIPO_PREDIO_DESCRIPCION"].ToString();
                M_Orden_Negocio.P_Tipo = dataTable.Rows[0]["TIPO_PREDIO_DESCRIPCION"].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                M_Orden_Negocio.P_Uso_Suelo = dataTable.Rows[0]["USO_SUELO_DESCRIPCION"].ToString();
                Txt_Usos_Predio.Text = dataTable.Rows[0]["USO_SUELO_DESCRIPCION"].ToString();
            }
            if (dataTable.Rows[0]["ESTADO_PREDIO_DESCRIPCION"].ToString() != string.Empty)
            {
                Txt_Estados_Predio.Text = dataTable.Rows[0]["ESTADO_PREDIO_DESCRIPCION"].ToString();
                M_Orden_Negocio.P_Estado_Predial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            {
                Txt_Estatus.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
                M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            }
            Txt_Superficie_Construida.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString();
            Txt_Superficie_Total.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString();
            if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            {
                Txt_Colonia_Cuenta.Text = dataTable.Rows[0]["NOMBRE_COLONIA"].ToString();
                M_Orden_Negocio.P_Colonia_Cuenta = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString();

                Txt_Calle_Cuenta.Text = dataTable.Rows[0]["NOMBRE_CALLE"].ToString();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString();
            }
            Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
            Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
            Txt_Catastral.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            {
                Txt_Efectos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
                M_Orden_Negocio.P_Efectos_Año = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString();
            }
            if (!String.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion].ToString()))
            {
                Txt_Dif_Construccion.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Diferencia_Construccion]).ToString("#,###,#0.00");
            }
            if (!string.IsNullOrEmpty(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString()))
            {
                Txt_Costo_M2.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Costo_m2].ToString()).ToString("#,###,#0.00");
            }
            Txt_Valor_Fiscal.Text = Convert.ToDouble(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Valor_Fiscal].ToString()).ToString("#,###,#0.00");
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "SI" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "si" || dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuota_Fija].ToString() == "Si")
            {
                //----Cargar detalles de la cuota Fija
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString() != "")
                {
                    Cargar_Datos_Cuota_Fija(dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Couta_Fija].ToString());
                }
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID] != null)
            {
                if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString() != "")
                {
                    DataRow Dr_Tasa_Seleccionada;
                    Hdn_Tasa_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Tasa_ID].ToString();
                    Cls_Ope_Pre_Tasas_Anuales_Negocio Tasas_Negocio = new Cls_Ope_Pre_Tasas_Anuales_Negocio();
                    Tasas_Negocio.P_Tasa_Predial_ID = Hdn_Tasa_ID.Value;

                    Dr_Tasa_Seleccionada = Tasas_Negocio.Consultar_Tasas_Anuales().Rows[0];
                    Txt_Tasa_Descripcion.Text = Dr_Tasa_Seleccionada["IDENTIFICADOR"].ToString() + " - " + Dr_Tasa_Seleccionada["DESCRIPCION"].ToString() + " - " + Dr_Tasa_Seleccionada["ANIO"].ToString();
                    Txt_Tasa_Porcentaje.Text = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                    //Txt_Tasa_Excedente_Valor.Text = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                    //Txt_Tasa_Exedente_Construccion.Text = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
                    //Calcular_Cuota();
                    //Calcular_Excedentes();
                    Consulta_Excedente_Valor();
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }

    //protected void Grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    //{
    //    Cargar_Grid_Cuentas_canceladas(e.NewPageIndex);
    //    Grid.SelectedIndex = (-1);
    //}

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
        DataTable Dt_Diferencias_Header = new DataTable();
        DataTable Dt_Agregar_Diferencias = new DataTable();
        try
        {
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
            Ds_Reporte_Ordenes_Salida.Tables.Add(Dt_Diferencias_Header.Copy());
            Ds_Reporte_Ordenes_Salida.Tables[1].TableName = "Dt_Diferencias_Header";
            if (Session["Dt_Agregar_Diferencias"] != null)
                Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            else
                Dt_Agregar_Diferencias = Formar_Tabla_Diferencias();

            if (Dt_Agregar_Diferencias != null)
            {
                if (Dt_Agregar_Diferencias.Rows.Count > 0)
                    Dt_Agregar_Diferencias = Acomodar_Periodos(Dt_Agregar_Diferencias);

                Ds_Reporte_Ordenes_Salida.Tables.Add(Dt_Agregar_Diferencias.Copy());
                Ds_Reporte_Ordenes_Salida.Tables[2].TableName = "Dt_Diferencias";
            }
            else
            {
                Ds_Reporte_Ordenes_Salida.Tables.Add(new DataTable());
                Ds_Reporte_Ordenes_Salida.Tables[2].TableName = "Dt_Diferencias";
            }
            Ds_Reporte_Ordenes_Salida.AcceptChanges();
            // Ruta donde se encuentra el reporte Crystal
            Ruta_Reporte_Crystal = "../Rpt/Predial/Rpt_Pre_Orden_Variacion.rpt";

            // Se crea el nombre del reporte
            String Nombre_Reporte = "Orden_Variacion_" + Cls_Sessiones.No_Empleado + "_" + Convert.ToString(DateTime.Now.ToString("yyyy'-'MM'-'dd'_t'HH'-'mm'-'ss"));

            Nombre_Reporte_Generar = Nombre_Reporte + ".pdf";  // Es el nombre del reporte PDF que se va a generar
            Cls_Reportes Reportes = new Cls_Reportes();
            Generar_Reporte(ref Ds_Reporte_Ordenes_Salida, Ruta_Reporte_Crystal, Nombre_Reporte_Generar, "PDF");
            Mostrar_Reporte(Nombre_Reporte_Generar, "PDF");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message, ex);
        }
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
    ///NOMBRE DE LA FUNCIÓN: Cargar_Grid_Diferencias
    ///DESCRIPCIÓN: Cargar_Grid_Diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Cargar_Grid_Diferencias(int Page_Index)
    {
        try
        {
            Grid_Diferencias.DataSource = null;
            if (Session["Dt_Agregar_Diferencias"] != null)
            {
                Grid_Diferencias.DataSource = (DataTable)Session["Dt_Agregar_Diferencias"];
            }
            Grid_Diferencias.PageIndex = Page_Index;
            Grid_Diferencias.DataBind();
            Calcular_Resumen();
            Resumen_Grid_Diferencias();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
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

        //if (Cmb_P_R_Anio_Inicial.SelectedIndex == Cmb_P_R_Anio_Final.SelectedIndex)
        //{
        //    if (Cmb_P_R_Bimestre_Inicial.SelectedIndex > Cmb_P_R_Bimestre_Final.SelectedIndex)
        //    {
        //        Resultado = false;
        //    }
        //}

        return Resultado;

    }
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
            double Total_Alta_Corriente = 0;
            double Total_Baja_Corriente = 0;
            double Total_Alta_Rezago = 0;
            double Total_Baja_Rezago = 0;
            DataTable Dt_Agregar_Diferencias;

            if (Session["Dt_Agregar_Diferencias"] != null)
                Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
            else
                Dt_Agregar_Diferencias = Formar_Tabla_Diferencias();

            if (Dt_Agregar_Diferencias.Rows.Count > 0)
            {
                foreach (DataRow Dr_Diferencias in Dt_Agregar_Diferencias.Rows)
                {
                    //Agregar al resumen                        
                    if (Dr_Diferencias["PERIODO"].ToString().Trim().Contains(DateTime.Today.Year.ToString()))
                    {
                        if (Dr_Diferencias["TIPO"].ToString().Trim() == "ALTA")
                        {
                            Total_Alta_Corriente += double.Parse(Dr_Diferencias["IMPORTE"].ToString().Replace('$', ' ').Trim());
                        }
                        else
                        {
                            Total_Baja_Corriente += double.Parse(Dr_Diferencias["IMPORTE"].ToString().Replace('$', ' ').Trim());
                        }
                    }
                    else
                    {
                        if (Dr_Diferencias["TIPO"].ToString().Trim() == "ALTA")
                        {
                            Total_Alta_Rezago += double.Parse(Dr_Diferencias["IMPORTE"].ToString().Replace('$', ' ').Trim());
                        }
                        else
                        {
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
            }
        }
        catch (Exception Ex) { Mensaje_Error(Ex.Message); }
    }
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
            Txt_Grid_Dif_Valor_Fiscal_TextChanged(null, null);
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
            if (Cmb_Solicitante.SelectedIndex > 0)
            {
                Txt_Fundamento.Text = "";
                Txt_Fundamento.Text = Orden_Negocio.Consulta_Fundamento(Cmb_Solicitante.SelectedValue.ToString());
                Cmb_Financiado.SelectedIndex = 0;
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
            //TextBox Text_Grid = sender as TextBox;
            //GridViewRow gvr = Text_Grid.NamingContainer as GridViewRow;
            //index = gvr.DataItemIndex;
            //TextBox Text_Importe = gvr.FindControl("Txt_Grid_Importe") as TextBox;
            //TextBox Text_C_Bimestral = gvr.FindControl("Txt_Grid_Cuota_Bimestral") as TextBox;
            //Dt_Agregar_Diferencias.Rows[index]["IMPORTE"] = Text_Importe.Text.Trim();
            //Dt_Agregar_Diferencias.Rows[index]["CUOTA_BIMESTRAL"] = Text_C_Bimestral.Text.Trim();
            //Calcular_Resumen();
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
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
        try
        {
            //Double Importe;
            //Double C_Bimestral;
            //Double V_Fiscal;
            //Double Tasa;
            //Double Factor;
            //int B_Inicial;
            //int B_Final;
            //int Lapzo;
            //string Periodo;
            //string[] Bimestres;
            //if (Grid_Diferencias.Rows.Count > 0)
            //{
            //    for (Int32 Contador = 0; Contador < Grid_Diferencias.Rows.Count; Contador++)
            //    {
            //        TextBox Text_Valor_Temporal = (TextBox)Grid_Diferencias.Rows[Contador].Cells[2].FindControl("Txt_Grid_Dif_Valor_Fiscal");
            //        TextBox Text_Cuota_Bim_Temp = (TextBox)Grid_Diferencias.Rows[Contador].Cells[6].FindControl("Txt_Grid_Cuota_Bimestral");
            //        TextBox Text_Importe = (TextBox)Grid_Diferencias.Rows[Contador].Cells[5].FindControl("Txt_Grid_Importe");
            //        DropDownList Cmb_Tipo_Dif = (DropDownList)Grid_Diferencias.Rows[Contador].Cells[1].FindControl("Cmb_Tipo_Diferencias");
            //        Tasa = Convert.ToDouble(Grid_Diferencias.Rows[Contador].Cells[3].Text);
            //        V_Fiscal = Convert.ToDouble(Text_Valor_Temporal.Text.Trim());
            //        Factor = Tasa / 1000;
            //        Importe = Factor * V_Fiscal;
            //        Periodo = Grid_Diferencias.Rows[Contador].Cells[0].Text;
            //        Bimestres = Periodo.Split('-');
            //        if (Bimestres[1].Contains('/'))
            //        {
            //            Dt_Agregar_Diferencias.Rows[Contador]["TIPO_PERIODO"] = "REZAGO";
            //            B_Inicial = Int32.Parse(Bimestres[0][0].ToString());
            //            B_Final = Int32.Parse(Bimestres[1][1].ToString());
            //        }
            //        else
            //        {
            //            Dt_Agregar_Diferencias.Rows[Contador]["TIPO_PERIODO"] = "CORRIENTE";
            //            B_Inicial = Int32.Parse(Bimestres[0][0].ToString());
            //            B_Final = Int32.Parse(Bimestres[0][2].ToString());
            //        }
            //        Lapzo = B_Final - B_Inicial;
            //        C_Bimestral = Importe / 6;
            //        Importe = C_Bimestral * Lapzo;

            //        if (String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()) || (Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()) == 0))
            //            Text_Importe.Text = Importe.ToString("#,###,###.##");
            //        if (String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()) || (Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()) == 0))
            //            Text_Cuota_Bim_Temp.Text = C_Bimestral.ToString("#,###,###.##");

            //        //Devolver Valores
            //        Dt_Agregar_Diferencias.Rows[Contador]["PERIODO"] = Grid_Diferencias.Rows[Contador].Cells[0].Text;
            //        Dt_Agregar_Diferencias.Rows[Contador]["TASA"] = Grid_Diferencias.Rows[Contador].Cells[3].Text;
            //        Dt_Agregar_Diferencias.Rows[Contador]["TIPO"] = Cmb_Tipo_Dif.SelectedValue.ToString();
            //        Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"] = Text_Importe.Text.Trim();
            //        Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"] = Text_Cuota_Bim_Temp.Text.Trim();
            //        Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"] = Text_Valor_Temporal.Text.Trim();
            //        //Dt_Agregar_Diferencias.Rows[Contador]["TASA"] = Grid_Diferencias.Rows[Contador].Cells[4].Text;
            //        //Dt_Agregar_Diferencias.Rows[Contador]["TASA_ID"] = Grid_Diferencias.Rows[Contador].Cells[3].Text;
            //    }
            //    Calcular_Resumen();
            //}
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
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
        try
        {
            //if (Convert.ToInt32(Cmb_P_C_Bimestre_Inicial.SelectedValue) <= Convert.ToInt32(Cmb_P_C_Bimestre_Final.SelectedValue))
            //{
            //    Periodo = Cmb_P_C_Bimestre_Inicial.SelectedValue.ToString() + "/" + DateTime.Today.Year.ToString() + " - " + Cmb_P_C_Bimestre_Final.SelectedValue.ToString() + "/" + DateTime.Today.Year.ToString();
            //    DataRow[] Dr_Validar = Dt_Agregar_Diferencias.Select("PERIODO = '" + Periodo + "'");
            //    if (Dr_Validar.Length <= 0)
            //    {
            //        if ((Int32.Parse(Cmb_P_C_Bimestre_Inicial.SelectedValue.ToString()) < Int32.Parse(Txt_Desde_Periodo_Corriente.Text) || Txt_Desde_Periodo_Corriente.Text == "0"))
            //        {
            //            Txt_Desde_Periodo_Corriente.Text = Cmb_P_C_Bimestre_Inicial.SelectedValue.ToString();
            //            Txt_Desde_Anio_Periodo_Corriente.Text = DateTime.Today.Year.ToString();
            //        }

            //        if (Int32.Parse(Cmb_P_C_Bimestre_Final.SelectedValue.ToString()) > Int32.Parse(Txt_Hasta_Periodo_Corriente.Text))
            //        {
            //            Txt_Hasta_Periodo_Corriente.Text = Cmb_P_C_Bimestre_Final.SelectedValue.ToString();
            //            Txt_Hasta_Anio_Periodo_Corriente.Text = DateTime.Today.Year.ToString();
            //        }
            //        DataRow Dr_Periodo = Dt_Agregar_Diferencias.NewRow();
            //        Dr_Periodo["PERIODO"] = Periodo;

            //        Cmb_P_C_Bimestre_Final.SelectedIndex = 5;
            //        Cmb_P_C_Bimestre_Inicial.SelectedIndex = 0;

            //        if (Dr_Periodo != null)
            //            Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo);
            //        Dr_Periodo = null;
            //        Cargar_Grid_Diferencias(0);
            //    }
            //    else
            //        Lbl_Mensaje_Error_Diferencias.Text = "Ya se ha agregado este periodo";
            //}
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
        try
        {
            //if (Validar_Diferencia_Regazo())
            //{
            //    Periodo = Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString() + "/" + Cmb_P_R_Anio_Inicial.SelectedValue.ToString() + " - " + Cmb_P_R_Bimestre_Final.SelectedValue.ToString() + "/" + Cmb_P_R_Anio_Final.SelectedValue.ToString();
            //    DataRow[] Dr_Validar = Dt_Agregar_Diferencias.Select("PERIODO = '" + Periodo + "'");
            //    if (Dr_Validar.Length <= 0)
            //    {
            //        DataRow Dr_Periodo = Dt_Agregar_Diferencias.NewRow();
            //        //String[] cadena = Cmb_Grid_Tasas.SelectedItem.Text.Split('-');

            //        Dr_Periodo["PERIODO"] = Periodo;
            //        //Agregar al resumen                        

            //        if ((Int32.Parse(Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString()) < Int32.Parse(Txt_Desde_Periodo_Regazo.Text) || Txt_Desde_Periodo_Regazo.Text == "0"))
            //        {
            //            if ((Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue.ToString()) <= Int32.Parse(Lbl_P_C_Anio_Inicio.Text) || Lbl_P_C_Anio_Inicio.Text == "0"))
            //                Txt_Desde_Periodo_Regazo.Text = Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString();
            //        }

            //        if (Int32.Parse(Cmb_P_R_Bimestre_Final.SelectedValue.ToString()) > Int32.Parse(Txt_Hasta_Periodo_Regazo.Text))
            //        {
            //            if ((Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue.ToString()) >= Int32.Parse(Lbl_P_C_Anio_Final.Text)))
            //                Txt_Hasta_Periodo_Regazo.Text = Cmb_P_R_Bimestre_Final.SelectedValue.ToString();
            //        }

            //        if ((Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue.ToString()) < Int32.Parse(Lbl_P_C_Anio_Inicio.Text) || Lbl_P_C_Anio_Inicio.Text == "0"))
            //        {
            //            Lbl_P_C_Anio_Inicio.Text = Cmb_P_R_Anio_Inicial.SelectedValue.ToString();
            //        }

            //        if ((Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue.ToString()) > Int32.Parse(Lbl_P_C_Anio_Final.Text)))
            //        {
            //            Lbl_P_C_Anio_Final.Text = Cmb_P_R_Anio_Final.SelectedValue.ToString();
            //        }

            //        if (Dr_Periodo != null)
            //        {
            //            if (Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue) < Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue))
            //            {
            //                Bimestre_Final = Int32.Parse(Cmb_P_R_Bimestre_Final.SelectedValue.ToString());
            //                Bimestre_Inicial = Int32.Parse(Cmb_P_R_Bimestre_Inicial.SelectedValue.ToString());
            //                Anio_Inicial = Int32.Parse(Cmb_P_R_Anio_Inicial.SelectedValue.ToString());
            //                Anio_Final = Int32.Parse(Cmb_P_R_Anio_Final.SelectedValue.ToString());
            //                DataRow Dr_Periodo_Temp;
            //                Dr_Periodo_Temp = Dr_Periodo;
            //                anios = Anio_Final - Anio_Inicial;
            //                for (int cont = Anio_Inicial; cont <= Anio_Final; cont++)
            //                {
            //                    if (cont == Anio_Inicial)
            //                    {
            //                        DataRow Dr_Periodo_Temporal1 = Dt_Agregar_Diferencias.NewRow();
            //                        Dr_Periodo_Temporal1["PERIODO"] = Bimestre_Inicial.ToString() + "/" + cont.ToString() + " - 6/" + cont.ToString();
            //                        //Dr_Periodo_Temporal1["TASA"] = Txt_Tasa_Porcentaje_Diferencias.Text.Trim();//cadena[2].Trim();
            //                        //Dr_Periodo_Temporal1["TASA_ID"] = Hdn_Tasa_Dif.Value.ToString();                                        
            //                        Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo_Temporal1);
            //                    }
            //                    else if (cont == Anio_Final)
            //                    {
            //                        DataRow Dr_Periodo_Temporal2 = Dt_Agregar_Diferencias.NewRow();
            //                        Dr_Periodo_Temporal2["PERIODO"] = "1/" + cont.ToString() + " - " + Bimestre_Final.ToString() + "/" + cont.ToString();
            //                        //Dr_Periodo_Temporal2["TASA"] = Txt_Tasa_Porcentaje_Diferencias.Text.Trim();//cadena[2].Trim();
            //                        //Dr_Periodo_Temporal2["TASA_ID"] = Hdn_Tasa_Dif.Value.ToString();                                        
            //                        Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo_Temporal2);
            //                    }
            //                    else
            //                    {
            //                        DataRow Dr_Periodo_Temporal3 = Dt_Agregar_Diferencias.NewRow();
            //                        Dr_Periodo_Temporal3["PERIODO"] = "1/" + cont.ToString() + " - 6/" + cont.ToString();
            //                        //Dr_Periodo_Temporal3["TASA"] = Txt_Tasa_Porcentaje_Diferencias.Text.Trim();//cadena[2].Trim();
            //                        //Dr_Periodo_Temporal3["TASA_ID"] = Hdn_Tasa_Dif.Value.ToString();                                        
            //                        Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo_Temporal3);
            //                    }
            //                }
            //            }
            //            else
            //                Dt_Agregar_Diferencias.Rows.Add(Dr_Periodo);
            //        }

            //        Cmb_P_R_Bimestre_Final.SelectedIndex = 5;
            //        Cmb_P_R_Bimestre_Inicial.SelectedIndex = 0;
            //        Cmb_P_R_Anio_Inicial.SelectedIndex = 0;
            //        Cmb_P_R_Anio_Final.SelectedIndex = 0;

            //        Dr_Periodo = null;
            //        Cargar_Grid_Diferencias(0);
            //    }
            //}
        }
        catch (Exception Ex)
        {

        }
    }

    #endregion

    #region Eventos Grid
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
            //String Ventana = "";
            //String Propiedades = "";
            //String Valor_Fiscal = "0";
            //for (int Contador = 0; Contador < Grid_Diferencias.Rows.Count; Contador++)
            //{
            //    TextBox Text_Valor_Temporal = (TextBox)Grid_Diferencias.Rows[Contador].Cells[2].FindControl("Txt_Grid_Dif_Valor_Fiscal");
            //    TextBox Text_Cuota_Bim_Temp = (TextBox)Grid_Diferencias.Rows[Contador].Cells[6].FindControl("Txt_Grid_Cuota_Bimestral");
            //    TextBox Text_Importe = (TextBox)Grid_Diferencias.Rows[Contador].Cells[5].FindControl("Txt_Grid_Importe");
            //    DropDownList Cmb_Tipo_Dif = (DropDownList)Grid_Diferencias.Rows[Contador].Cells[1].FindControl("Cmb_Tipo_Diferencias");
            //    ImageButton Boton_Tasas_Diferencias = (ImageButton)Grid_Diferencias.Rows[Contador].Cells[4].FindControl("Btn_Tasa_Seleccionar");
            //    Cmb_Tipo_Dif.SelectedValue = Dt_Agregar_Diferencias.Rows[Contador]["TIPO"].ToString();
            //    Valor_Fiscal = Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString();
            //    if (!String.IsNullOrEmpty(Valor_Fiscal))
            //        Text_Valor_Temporal.Text = Convert.ToDouble(Valor_Fiscal).ToString("#,###,##0.00");
            //    if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()))
            //        Text_Cuota_Bim_Temp.Text = Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()).ToString("#,###,##0.00");
            //    if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()))
            //        Text_Importe.Text = Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()).ToString("#,###,##0.00");
            //    if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["TASA"].ToString()))
            //        Grid_Diferencias.Rows[Contador].Cells[3].Text = Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["TASA"].ToString()).ToString("#,###,##0.00"); ;
            //    Ventana = "Abrir_Ventana_Modal('Ventanas_Emergentes/Orden_Variacion/Frm_Menu_Pre_Tasas.aspx";
            //    Propiedades = ", 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHide:true;help:no;scroll:no');";
            //    Boton_Tasas_Diferencias.Attributes.Add("OnClick", Ventana + "?Fecha=False'" + Propiedades);
            //    Calcular_Analisis_Rezago("Grid_Diferencias_DataBound");
            //}
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
        //try
        //{
        //String Tasa_Predial_ID;
        //String Tasa_Concepto;
        //String Tasa_Porcentaje;
        //String Tasa_Previa;
        //String Tasa_Anio;
        //String Tasa_General;
        //DataRow Dr_Tasa_Seleccionada;
        //int index = 0;
        //if (e.CommandName == "Cmd_Tasa")
        //{
        //    index = Convert.ToInt32(e.CommandArgument);
        //}

        //try
        //{
        //    Tasa_Previa = Dt_Agregar_Diferencias.Rows[index + (Grid_Diferencias.PageIndex * Grid_Diferencias.PageSize)]["TASA"].ToString();
        //    if (Session["Dr_Tasa_Seleccionada"] != null)
        //    {
        //        Dr_Tasa_Seleccionada = ((DataRow)Session["Dr_Tasa_Seleccionada"]);
        //        Tasa_Predial_ID = Dr_Tasa_Seleccionada["TASA_ANUAL_ID"].ToString();
        //        Tasa_Concepto = Dr_Tasa_Seleccionada["IDENTIFICADOR"].ToString() + " - " + Dr_Tasa_Seleccionada["DESCRIPCION"].ToString() + " - " + Dr_Tasa_Seleccionada["ANIO"].ToString();
        //        Tasa_Porcentaje = Dr_Tasa_Seleccionada["TASA_ANUAL"].ToString();
        //        Tasa_Anio = Dr_Tasa_Seleccionada["ANIO"].ToString();
        //        Tasa_General = Dr_Tasa_Seleccionada["TASA_PREDIAL_ID"].ToString();
        //        Grid_Diferencias.Rows[index].Cells[3].Text = Tasa_Porcentaje;
        //        Dt_Agregar_Diferencias.Rows[index + (Grid_Diferencias.PageIndex * Grid_Diferencias.PageSize)]["TASA"] = Tasa_Porcentaje;
        //        Dt_Agregar_Diferencias.Rows[index + (Grid_Diferencias.PageIndex * Grid_Diferencias.PageSize)]["TASA_ID"] = Tasa_Predial_ID;
        //        if (!String.IsNullOrEmpty(Tasa_Previa))
        //            Calcular_Analisis_Rezago("Grid_Diferencias_RowCommand");
        //        else
        //            Calcular_Analisis_Rezago("Tasa_Nueva");
        //    }

        //}
        //catch (Exception Ex)
        //{
        //    if (Ex.Message != "Object reference not set to an instance of an object." && Ex.Message != "Referencia a objeto no establecida como instancia de un objeto.")
        //    {
        //        Mensaje_Error(Ex.Message);
        //    }
        //}
        //}
        //catch (Exception Ex)
        //{
        //    Mensaje_Error(Ex.Message);
        //}
    }

    protected void Grid_Diferencias_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //if (e.Row.RowType == DataControlRowType.DataRow)
        //{
        //    DataTable Dt_Datos = Dt_Agregar_Diferencias;
        //    if (Dt_Datos.Rows[e.Row.RowIndex]["TIPO_PERIODO"].ToString().Trim().Equals("CORRIENTE"))
        //    {
        //        if (e.Row.FindControl("Txt_Grid_Dif_Valor_Fiscal") != null)
        //        {
        //            TextBox Txt_Grid_Dif_Valor_Fiscal = (TextBox)e.Row.FindControl("Txt_Grid_Dif_Valor_Fiscal");
        //            Txt_Grid_Dif_Valor_Fiscal.Text = Dt_Datos.Rows[e.Row.RowIndex]["VALOR_FISCAL"].ToString();
        //        }
        //        if (e.Row.FindControl("Btn_Tasa_Seleccionar") != null)
        //        {
        //            ImageButton Btn_Tasa_Seleccionar = (ImageButton)e.Row.FindControl("Btn_Tasa_Seleccionar");
        //        }
        //    }
        //}
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
    private void Calcular_Analisis_Rezago(String Metodo)
    {
        try
        {
            //Double Importe = 0;
            //Double C_Bimestral = 0;
            //Double V_Fiscal = 0;
            //Double Tasa = 0;
            //Double Factor = 0;
            //int B_Inicial;
            //int B_Final;
            //int Lapzo;
            //string Periodo;
            //string[] Bimestres;
            //string Anio_Final = "0";
            //Double Validacion_Cuota_Minima = 0;
            //String Calcular = "SI";
            //Boolean Recalcular = true;

            //if (Session["Calcular_Grid_Diferencias"] != null)
            //{
            //    if (Session["Calcular_Grid_Diferencias"].ToString() == "NO")
            //        Calcular = "NO";
            //}
            //if (Grid_Diferencias.Rows.Count > 0 && Calcular == "SI")
            //{
            //    for (Int32 Contador = 0; Contador < Grid_Diferencias.Rows.Count; Contador++)
            //    {
            //        TextBox Text_Valor_Temporal = (TextBox)Grid_Diferencias.Rows[Contador].Cells[2].FindControl("Txt_Grid_Dif_Valor_Fiscal");
            //        TextBox Text_Cuota_Bim_Temp = (TextBox)Grid_Diferencias.Rows[Contador].Cells[6].FindControl("Txt_Grid_Cuota_Bimestral");
            //        TextBox Text_Importe = (TextBox)Grid_Diferencias.Rows[Contador].Cells[5].FindControl("Txt_Grid_Importe");
            //        DropDownList Cmb_Tipo_Dif = (DropDownList)Grid_Diferencias.Rows[Contador].Cells[1].FindControl("Cmb_Tipo_Diferencias");
            //        Tasa = Convert.ToDouble(Grid_Diferencias.Rows[Contador].Cells[3].Text);
            //        if (String.IsNullOrEmpty(Text_Valor_Temporal.Text.Trim()))
            //        {
            //            V_Fiscal = 0.00;
            //        }
            //        else
            //        {
            //            V_Fiscal = Convert.ToDouble(Text_Valor_Temporal.Text.Trim());

            //        }
            //        Factor = Tasa / 1000;
            //        Importe = Factor * V_Fiscal;
            //        Periodo = Grid_Diferencias.Rows[Contador].Cells[0].Text;
            //        Anio_Final = Periodo.Substring(Periodo.Length - 4);
            //        Bimestres = Periodo.Split('-');
            //        if (Anio_Final.Trim() != DateTime.Today.Year.ToString().Trim())
            //        {
            //            Dt_Agregar_Diferencias.Rows[Contador]["TIPO_PERIODO"] = "REZAGO";
            //            B_Inicial = Int32.Parse(Bimestres[0][0].ToString());
            //            B_Final = Int32.Parse(Bimestres[1][1].ToString());
            //        }
            //        else
            //        {
            //            Dt_Agregar_Diferencias.Rows[Contador]["TIPO_PERIODO"] = "CORRIENTE";
            //            B_Inicial = Int32.Parse(Bimestres[0][0].ToString());
            //            B_Final = Int32.Parse(Bimestres[1][1].ToString());
            //        }
            //        Validacion_Cuota_Minima = Consulta_Cuota_Minima_Anio(Anio_Final);
            //        if (Importe < Validacion_Cuota_Minima)
            //            Importe = Validacion_Cuota_Minima;
            //        Lapzo = (B_Final - B_Inicial) + 1;
            //        C_Bimestral = Importe / 6;
            //        Importe = C_Bimestral * Lapzo;
            //        if (!String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString()))
            //        {
            //            if (Convert.ToDouble(V_Fiscal.ToString()) - Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString()) > 0 || Convert.ToDouble(V_Fiscal.ToString()) - Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"].ToString()) < 0)
            //                Recalcular = true;
            //        }
            //        if (!String.IsNullOrEmpty(Text_Importe.Text.Trim()))
            //        {
            //            if (((Convert.ToDouble(Text_Importe.Text.Trim()) - Importe) > 1 || (Convert.ToDouble(Text_Importe.Text.Trim()) - Importe) < 1) && Convert.ToDouble(Text_Importe.Text.Trim()) > 0)
            //                Recalcular = false;
            //        }

            //        if (Metodo == "Grid_Diferencias_RowCommand" && V_Fiscal > 0.00 || Recalcular)
            //        {
            //            Text_Importe.Text = Importe.ToString("#,###,##0.00");
            //            Text_Cuota_Bim_Temp.Text = C_Bimestral.ToString("#,###,##0.00");
            //        }
            //        else
            //        {
            //            if (V_Fiscal > 0.00 && Recalcular)
            //            {
            //                if (String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()) || (Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"].ToString()) == 0))
            //                    Text_Importe.Text = Importe.ToString("#,###,##0.00");
            //                if (String.IsNullOrEmpty(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()) || (Convert.ToDouble(Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"].ToString()) == 0))
            //                    Text_Cuota_Bim_Temp.Text = C_Bimestral.ToString("#,###,##0.00");
            //            }
            //        }
            //        Recalcular = true;
            //        //Devolver Valores

            //        Dt_Agregar_Diferencias.Rows[Contador]["PERIODO"] = Grid_Diferencias.Rows[Contador].Cells[0].Text;
            //        Dt_Agregar_Diferencias.Rows[Contador]["TASA"] = Grid_Diferencias.Rows[Contador].Cells[3].Text;
            //        Dt_Agregar_Diferencias.Rows[Contador]["TIPO"] = Cmb_Tipo_Dif.SelectedValue.ToString();
            //        if (!String.IsNullOrEmpty(Text_Importe.Text.Trim()))
            //            Dt_Agregar_Diferencias.Rows[Contador]["IMPORTE"] = Text_Importe.Text.Trim();
            //        if (!String.IsNullOrEmpty(Text_Cuota_Bim_Temp.Text.Trim()))
            //            Dt_Agregar_Diferencias.Rows[Contador]["CUOTA_BIMESTRAL"] = Text_Cuota_Bim_Temp.Text.Trim();
            //        if (!String.IsNullOrEmpty(Text_Valor_Temporal.Text.Trim()))
            //            Dt_Agregar_Diferencias.Rows[Contador]["VALOR_FISCAL"] = Text_Valor_Temporal.Text.Trim();
            //        //Dt_Agregar_Diferencias.Rows[Contador]["TASA"] = Grid_Diferencias.Rows[Contador].Cells[4].Text;
            //        //Dt_Agregar_Diferencias.Rows[Contador]["TASA_ID"] = Grid_Diferencias.Rows[Contador].Cells[3].Text;
            //    }
            //    Calcular_Resumen();
            //}
        }
        catch (Exception Ex)
        {
            //Mensaje_Error(Ex.Message);
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

    #region Cuota_Fija
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
            //if (Txt_Cuota_Minima_Aplicar.Text.Trim() != "" && Txt_Tasa_Porcentaje.Text.Trim() != "")
            //{
            //    if (Txt_Dif_Construccion.Text.Trim() != "" && Txt_Tasa_Exedente_Construccion.Text.Trim() != "")
            //        Txt_Excedente_Construccion_Total.Text = (((Convert.ToDouble(Txt_Dif_Construccion.Text.Trim()) * Convert.ToDouble(Txt_Costo_M2.Text.Trim())) * (Convert.ToDouble(Txt_Tasa_Exedente_Construccion.Text.Trim()) / 1000))).ToString("#,###,##0.00");

            //    if (!String.IsNullOrEmpty(Hdn_Excedente_Valor.Value))
            //    {
            //        if (Hdn_Excedente_Valor.Value == "0")
            //            Txt_Tasa_Valor_Total.Text = "0";
            //        else
            //            Txt_Tasa_Valor_Total.Text = (((Convert.ToDouble(Hdn_Excedente_Valor.Value)) * ((Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim())) / 1000))).ToString("#,###,##0.00");
            //    }

            //    if (!String.IsNullOrEmpty(Txt_Cuota_Minima.Text.Trim()))
            //    {
            //        Cuota_Minima = (Convert.ToDouble(Txt_Cuota_Minima.Text.Trim()));
            //        if (!Chk_Beneficio_Completo.Checked)
            //            Cuota_Minima = (Convert.ToDouble(Txt_Cuota_Minima_Aplicar.Text.Trim()));
            //        Exedente_Construccion = (Convert.ToDouble(Txt_Excedente_Construccion_Total.Text.Trim()));
            //        Excedente_Valor = (Convert.ToDouble(Txt_Tasa_Valor_Total.Text.Trim()));
            //        if (Exedente_Construccion >= Excedente_Valor)
            //        {
            //            Total_Cuota_Fija = (Cuota_Minima + Exedente_Construccion);
            //        }
            //        else if (Excedente_Valor > Exedente_Construccion)
            //        {
            //            Total_Cuota_Fija = (Cuota_Minima + Excedente_Valor);
            //        }
            //        Txt_Total_Cuota_Fija.Text = Math.Round(Total_Cuota_Fija, 2).ToString("#,###,##0.00");
            //        Session["Cuota_Fija_Nueva"] = "";
            //        Session.Remove("Cuota_Fija_Nueva");
            //        if (Chk_Cuota_Fija.Checked)
            //            Session["Cuota_Fija_Nueva"] = Txt_Total_Cuota_Fija.Text.Trim();
            //    }
            //}
            //else
            //    Lbl_Error_Cuota_Fija.Text = "+ Es necesario seleccionar la Tasa en la seccion de Impuestos";
        }
        catch (Exception Ex)
        {
            Lbl_Error_Cuota_Fija.Text = "Error:[ Ocurrio un problema al Calcular los Excedentes ]";
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
            {
                Txt_Excedente_Valor_Total.Text = "0.00";
                Hdn_Excedente_Valor.Value = "0.00";
            }
            else
            {
                Txt_Excedente_Valor_Total.Text = (((Convert.ToDouble(Excedente_Valor)) * ((Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim())) / 1000))).ToString("#,###,##0.00");
                Hdn_Excedente_Valor.Value = Excedente_Valor.ToString("#,###,##0.00"); ;
            }
            if (Txt_Dif_Construccion.Text.Trim() != "" && Txt_Tasa_Porcentaje.Text.Trim() != "")
                Txt_Excedente_Construccion_Total.Text = (((Convert.ToDouble(Txt_Dif_Construccion.Text.Trim()) * Convert.ToDouble(Txt_Costo_M2.Text.Trim())) * (Convert.ToDouble(Txt_Tasa_Porcentaje.Text.Trim()) / 1000))).ToString("#,###,##0.00");
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }

    #endregion



    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Formar_Tabla_Adeudos
    /// DESCRIPCIÓN: Crear tabla con columnas para almacenar adeudos
    /// PARÁMETROS:
    /// CREO: Jesus Toledo
    /// FECHA_CREO: 01-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Formar_Tabla_Adeudos()
    {
        // tabla y columnas
        DataTable Dt_Adeudos = new DataTable();

        // agregar columnas a la tabla        
        Dt_Adeudos.Columns.Add(Ope_Pre_Adeudos_Predial.Campo_Anio, System.Type.GetType("System.Int32"));
        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_1", System.Type.GetType("System.String"));
        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_2", System.Type.GetType("System.String"));
        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_3", System.Type.GetType("System.String"));
        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_4", System.Type.GetType("System.String"));
        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_5", System.Type.GetType("System.String"));
        Dt_Adeudos.Columns.Add("ADEUDO_BIMESTRE_6", System.Type.GetType("System.String"));
        Dt_Adeudos.Columns.Add("ADEUDO_TOTAL_ANIO", System.Type.GetType("System.String"));
        // regresar tabla adeudos 
        return Dt_Adeudos;
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Formar_Tabla_Diferencias
    /// DESCRIPCIÓN: Crear tabla con columnas para almacenar Diferencias
    /// PARÁMETROS:
    /// CREO: Jesus Toledo
    /// FECHA_CREO: 01-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Formar_Tabla_Diferencias()
    {
        // tabla y columnas
        DataTable Dt_Diferencias = new DataTable();

        // agregar columnas a la tabla        
        Dt_Diferencias.Columns.Add("NO_DIFERENCIA");
        Dt_Diferencias.Columns.Add("CUENTA_PREDIAL_ID");
        Dt_Diferencias.Columns.Add("IMPORTE");
        Dt_Diferencias.Columns.Add("TIPO");
        Dt_Diferencias.Columns.Add("PERIODO");
        Dt_Diferencias.Columns.Add("TASA_ID");
        Dt_Diferencias.Columns.Add("VALOR_FISCAL");
        Dt_Diferencias.Columns.Add("TASA");
        Dt_Diferencias.Columns.Add("CUOTA_BIMESTRAL");
        Dt_Diferencias.Columns.Add("TIPO_PERIODO");
        // regresar tabla adeudos 
        return Dt_Diferencias;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Adeudos
    ///DESCRIPCIÓN: Llena la tabla de Adeudos
    ///Paramentros: Page_Index Pagina en la cual se mostrará el Grid_VIew
    ///CREO: Jesus Toledo Rodriguez
    ///FECHA_CREO: 27/Abril/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Tabla_Adeudos(int Page_Index)
    {
        DataTable DT_Adeudos = Formar_Tabla_Adeudos();
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen_Predio = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataTable Dt_Cuota_Minima;
        Cls_Cat_Pre_Cuotas_Minimas_Negocio Cuotas_Minimas = new Cls_Cat_Pre_Cuotas_Minimas_Negocio();
        Double Dbl_Cuota_Minima = 0;
        Double Dbl_Cuota_Minima_Aplicada = 0;
        Boolean Resultado = false;
        int Periodo_Corriente = 1;
        try
        {
            if (Session["Tabla_Adeudos_Editados"] != null)
            {
                DT_Adeudos = (DataTable)Session["Tabla_Adeudos_Editados"];
                Grid_Adeudos_Editable.DataSource = DT_Adeudos;
                Grid_Adeudos_Editable.DataBind();
            }
            else if (Session["Tabla_Adeudos"] != null)
            {
                DT_Adeudos = (DataTable)Session["Tabla_Adeudos"];
                Grid_Adeudos_Editable.DataSource = DT_Adeudos;
                Grid_Adeudos_Editable.DataBind();
            }
            else if (!String.IsNullOrEmpty(Hdn_Cuenta_ID.Value))
            {
                DT_Adeudos = Resumen_Predio.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Hdn_Cuenta_ID.Value, null, 0, Const_Anio_Corriente);
                Session["Tabla_Adeudos"] = DT_Adeudos.Copy();
                //Se Modifica la tabla de adeudos considerando el beneficio
                Cuotas_Minimas.P_Anio = Const_Anio_Corriente.ToString();
                Dt_Cuota_Minima = Cuotas_Minimas.Consultar_Cuotas_Minimas_Ventana_Emergente();
                if (Dt_Cuota_Minima.Rows.Count > 0)
                {
                    if (Double.TryParse(Dt_Cuota_Minima.Rows[0]["Cuota"].ToString(), out Dbl_Cuota_Minima))
                    {
                        Periodo_Corriente = Int32.Parse(Obtener_Periodo_Corriente().Substring(9, 1));
                        if (Periodo_Corriente == 6)
                            Periodo_Corriente = 0;
                        Dbl_Cuota_Minima_Aplicada = Convert.ToDouble((Dbl_Cuota_Minima / 6).ToString("#,###,##0.00")) * (6 - Periodo_Corriente);//se divide entre el numero de periodo en curso para sacar el proporcional de la cuota
                        foreach (DataRow Adeudo in DT_Adeudos.Rows)
                        {
                            if (Adeudo["ANIO"].ToString().Trim() == Const_Anio_Corriente.ToString())
                            {
                                Double Adeudo_Total = Convert.ToDouble(Adeudo["ADEUDO_TOTAL_ANIO"]);
                                for (int i = Periodo_Corriente + 1; i <= 6; i++)
                                {
                                    Adeudo_Total = Adeudo_Total - Convert.ToDouble(Adeudo[i]);
                                    Adeudo[i] = 0;
                                }
                                Adeudo[Periodo_Corriente + 1] = Dbl_Cuota_Minima_Aplicada.ToString("#,###,##0.00");
                                Adeudo_Total += Dbl_Cuota_Minima_Aplicada;
                                Adeudo["ADEUDO_TOTAL_ANIO"] = Adeudo_Total.ToString("#,###,##0.00");
                                DT_Adeudos.AcceptChanges();
                                Session["Tabla_Adeudos_Editados"] = DT_Adeudos;
                                Btn_Generar_Diferencias.Visible = true;
                            }
                        }
                    }
                }
                //Se Modifica la tabla de adeudos considerando el beneficio
                Grid_Adeudos_Editable.DataSource = DT_Adeudos;
                Grid_Adeudos_Editable.DataBind();
                Calcular_Adeudos();

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
    protected void Grid_Adeudos_Editable_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        try
        {
            if (e.CommandName == "Cmd_Editar_Adeudos")
            {
                Grid_Editable = true;
                Llenar_Tabla_Adeudos(0);
                Btn_Mostrar_Tasas_Diferencias.Visible = true;
            }
            if (e.CommandName == "Cmd_Calcular_Totales")
            {
                Grid_Editable = false;
                Calcular_Bajas();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Bajas
    ///DESCRIPCIÓN: Cargar datos de las diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private void Calcular_Bajas()
    {
        try
        {
            if (Calcular_Adeudos())
            {
                Llenar_Tabla_Adeudos(0);
                Btn_Generar_Diferencias.Visible = true;
            }
            else
            {
                Llenar_Tabla_Adeudos(0);
                Mensaje_Error_Adeudos("No se efectuaron cambios en los adeudos");
                Btn_Generar_Diferencias.Visible = false;
                Btn_Mostrar_Tasas_Diferencias.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Calcular_Adeudos
    ///DESCRIPCIÓN: Cargar datos de las diferencias
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 08/Ago/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    private bool Calcular_Adeudos()
    {
        DataTable Dt_Adeudos;
        DataTable Dt_Adeudos_Editados;
        bool Resultado = false;
        List<int> Periodo = new List<int>();
        try
        {
            Dt_Adeudos_Editados = Obtener_DataTable_Recargos();
            if (Session["Tabla_Adeudos"] != null && Dt_Adeudos_Editados.Rows.Count > 0)
            {
                Dt_Adeudos = (DataTable)Session["Tabla_Adeudos"];
                if (Dt_Adeudos_Editados.Rows.Count == Dt_Adeudos.Rows.Count)
                {
                    for (int cont = 0; cont < Dt_Adeudos_Editados.Rows.Count; cont++)
                    {
                        if (Convert.ToDecimal(Dt_Adeudos_Editados.Rows[cont]["ADEUDO_TOTAL_ANIO"]) != Convert.ToDecimal(Dt_Adeudos.Rows[cont]["ADEUDO_TOTAL_ANIO"]))
                        {
                            Periodo.Add((cont));
                            Resultado = true;
                        }
                    }
                }
            }
            Session["Tabla_Periodos"] = Periodo;
            Session["Tabla_Adeudos_Editados"] = Dt_Adeudos_Editados;
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message);
        }
        return Resultado;
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
        DataTable Dt_Agregar_Diferencias = null;
        DataTable Dt_Adeudos = null;
        DataTable Dt_Adeudos_Editados = null;
        List<int> Lista_Periodos = new List<int>();
        Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio Adeudos_Negocio = new Cls_Ope_Pre_Recepcion_Pagos_Predial_Negocio();
        Cls_Ope_Pre_Resumen_Predio_Negocio Resumen = new Cls_Ope_Pre_Resumen_Predio_Negocio();
        DataRow Dr_Dif;
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
        Boolean Periodos_Diferentes = false;
        Boolean Insertar = false;
        Boolean Validar_Calculo = true;
        int Periodo_Corriente = 1;
        int Fin_Periodo = 0;
        int Inicio_Periodo = 0;
        try
        {
            Session["Dt_Agregar_Diferencias"] = null;
            Dt_Agregar_Diferencias = Formar_Tabla_Diferencias();

            if (Session["Tabla_Adeudos"] != null)
            {
                Dt_Adeudos = (DataTable)Session["Tabla_Adeudos"];
            }
            else
            {
                Validar_Calculo = false;
            }
            if (Session["Tabla_Adeudos_Editados"] != null)
            {
                Dt_Adeudos_Editados = (DataTable)Session["Tabla_Adeudos_Editados"];
            }
            else
            {
                Validar_Calculo = false;
            }
            if (Session["Tabla_Periodos"] != null)
            {
                Lista_Periodos = (List<int>)Session["Tabla_Periodos"];
            }
            else
            {
                Validar_Calculo = false;
            }

            if (Validar_Calculo)
            {
                Double Dbl_Adeudo = 0;
                for (int Renglon = 0; Renglon < Dt_Adeudos_Editados.Rows.Count; Renglon++)
                {
                    if (Lista_Periodos.Contains(Renglon))
                    {
                        Periodo_Inicial = "0";
                        for (int i = 1; i < 6; i++)
                        {
                            if (Convert.ToDouble(Dt_Adeudos.Rows[Renglon][i]) == Convert.ToDouble(Dt_Adeudos.Rows[Renglon][i + 1]) && Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon][i]) == Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon][i + 1]))
                            {
                                Periodos_Diferentes = true;
                                break;
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
                                DBL_IMPORTE = Convert.ToDouble(Dt_Adeudos.Rows[Renglon][contador_periodos]) - Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon][contador_periodos]);
                                TIPO = "BAJA";
                                if (DBL_IMPORTE < 0)
                                {
                                    DBL_IMPORTE = Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon][contador_periodos]) - Convert.ToDouble(Dt_Adeudos.Rows[Renglon][contador_periodos]);
                                    TIPO = "ALTA";
                                }
                                IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");
                                DBL_IMPORTE = (Convert.ToDouble(IMPORTE));

                                TASA_ID = Hdn_Tasa_ID.Value;
                                VALOR_FISCAL = Txt_Valor_Fiscal.Text.Trim();
                                TASA = Txt_Tasa_Porcentaje.Text.Trim();
                                CUOTA_BIMESTRAL = ((Convert.ToDouble(VALOR_FISCAL) * (Convert.ToDouble(TASA)) / 1000) / 6).ToString("#,###,##0.00");
                                TIPO_PERIODO = "REZAGO";
                                if (Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString() == Const_Anio_Corriente.ToString())
                                    TIPO_PERIODO = "CORRIENTE";
                                //    //if (Convert.ToDouble(IMPORTE)>0)

                                if (Fin_Periodo != 6)
                                {
                                    if (!Insertar)
                                        Inicio_Periodo = contador_periodos;
                                    Fin_Periodo = contador_periodos;
                                    if (Convert.ToDouble(Dt_Adeudos.Rows[Renglon][contador_periodos]) == Convert.ToDouble(Dt_Adeudos.Rows[Renglon][contador_periodos + 1]) && Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon][contador_periodos]) == Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon][contador_periodos + 1]))
                                    {
                                        Fin_Periodo = Fin_Periodo + 1;
                                        Insertar = true;
                                    }
                                    else
                                    {
                                        PERIODO = Inicio_Periodo + "/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString() + " - " + Fin_Periodo + "/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString();
                                        IMPORTE = ((DBL_IMPORTE) * ((Fin_Periodo - Inicio_Periodo) + 1)).ToString("#,###,##0.00");
                                        CUOTA_BIMESTRAL = (Convert.ToDouble(CUOTA_BIMESTRAL)).ToString("#,###,##0.00");
                                        if (DBL_IMPORTE > 0)
                                            Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                                        Insertar = false;
                                    }
                                }
                                else
                                {
                                    if (Insertar)
                                    {
                                        PERIODO = Inicio_Periodo + "/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString() + " - " + Fin_Periodo + "/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString();
                                        IMPORTE = ((DBL_IMPORTE) * ((Fin_Periodo - Inicio_Periodo) + 1)).ToString("#,###,##0.00");
                                        CUOTA_BIMESTRAL = (Convert.ToDouble(CUOTA_BIMESTRAL)).ToString("#,###,##0.00");
                                        if (DBL_IMPORTE > 0)
                                            Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                                        Insertar = false;
                                    }
                                }
                                //    //Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                                //    //break;
                            }
                        }//fap
                        else
                        {
                            Dr_Dif = Dt_Agregar_Diferencias.NewRow();
                            NO_DIFERENCIA = "";
                            CUENTA_PREDIAL_ID = Hdn_Cuenta_ID.Value;
                            DBL_IMPORTE = Convert.ToDouble(Dt_Adeudos.Rows[Renglon]["ADEUDO_TOTAL_ANIO"]) - Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon]["ADEUDO_TOTAL_ANIO"]);
                            TIPO = "BAJA";
                            if (DBL_IMPORTE < 0)
                            {
                                DBL_IMPORTE = Convert.ToDouble(Dt_Adeudos_Editados.Rows[Renglon]["ADEUDO_TOTAL_ANIO"]) - Convert.ToDouble(Dt_Adeudos.Rows[Renglon]["ADEUDO_TOTAL_ANIO"]);
                                TIPO = "ALTA";
                            }
                            IMPORTE = DBL_IMPORTE.ToString("#,###,##0.00");
                            DBL_IMPORTE = (Convert.ToDouble(IMPORTE));

                            PERIODO = "1/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString() + " - 6/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString();
                            if (Periodo_Inicial != "0")
                                PERIODO = Periodo_Inicial + "/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString() + " - 6/" + Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString();

                            TASA_ID = Hdn_Tasa_ID.Value;
                            VALOR_FISCAL = Txt_Valor_Fiscal.Text.Trim();
                            TASA = Txt_Tasa_Porcentaje.Text.Trim();
                            CUOTA_BIMESTRAL = (Convert.ToDouble(IMPORTE.ToString()) / (6)).ToString("#,###,##0.00");
                            TIPO_PERIODO = "REZAGO";
                            if (Dt_Adeudos_Editados.Rows[Renglon]["ANIO"].ToString() == Const_Anio_Corriente.ToString())
                                TIPO_PERIODO = "CORRIENTE";
                            if (DBL_IMPORTE > 0)
                                Quitar_Agregar_Diferencia(NO_DIFERENCIA, CUENTA_PREDIAL_ID, IMPORTE, TIPO, PERIODO, TASA_ID, VALOR_FISCAL, TASA, CUOTA_BIMESTRAL, TIPO_PERIODO);
                            //break;

                        }//fap
                        Periodos_Diferentes = false;
                        Insertar = false;
                        Fin_Periodo = 0;
                        Inicio_Periodo = 0;
                    }
                }
                //Resumen_Grid_Diferencias();
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Generar diferencias Error:[" + Ex.Message + "]");
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
        DataRow[] Dr_Periodos;
        DataRow Dr_Dif;
        DataTable Dt_Agregar_Diferencias = null;
        if (Session["Dt_Agregar_Diferencias"] != null)
            Dt_Agregar_Diferencias = (DataTable)Session["Dt_Agregar_Diferencias"];
        else
            Dt_Agregar_Diferencias = Formar_Tabla_Diferencias();
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
                if (Obtener_Dato_Consulta(Cat_Pre_Casos_Especiales.Campo_Tipo, Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales, Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Cuota_Fija_ID.Trim() + "'", "") == "SOLICITANTE")
                    Cmb_Solicitante.SelectedValue = Cuota_Fija_ID.Trim();
                if (Obtener_Dato_Consulta(Cat_Pre_Casos_Especiales.Campo_Tipo, Cat_Pre_Casos_Especiales.Tabla_Cat_Pre_Casos_Especiales, Cat_Pre_Casos_Especiales.Campo_Caso_Especial_ID + " = '" + Cuota_Fija_ID.Trim() + "'", "") == "FINANCIAMIENTO")
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


                    Txt_Excedente_Construccion_Total.Text = String.Format("{0:#,###,###.00}", Dt_Cuota_Detalles.Rows[0][Ope_Pre_Cuotas_Fijas.Campo_Excedente_Construccion_Total].ToString());
                    Txt_Fundamento.Text = "ARTICULO " + Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Articulo].ToString() + "INCISO " + Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Inciso].ToString() + Dt_Cuota_Detalles.Rows[0][Cat_Pre_Casos_Especiales.Campo_Observaciones].ToString();

                }
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Cargar_Datos_Cuota_Fija: " + Ex.Message);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_DataTable_Recargos
    /// DESCRIPCIÓN: Forma un datatable con el tabulador de recargos en el grid y valida que no haya
    ///             registros repetidos de año y bimestre
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Obtener_DataTable_Recargos()
    {
        String Mensaje = "";
        Boolean Validacion = true;
        Boolean Validacion_Suma = true;
        DataTable Dt_Adeudos;
        decimal Valor_Decimal;
        decimal Total = 0;

        Dt_Adeudos = Formar_Tabla_Adeudos();
        // recorrer el grid
        foreach (GridViewRow Adeudo in Grid_Adeudos_Editable.Rows)
        {
            DataRow Fila_Adeudo;

            TextBox Txt_Bimestre_1 = (TextBox)Adeudo.Cells[2].FindControl("Txt_Grid_Bimestre_1");
            TextBox Txt_Bimestre_2 = (TextBox)Adeudo.Cells[3].FindControl("Txt_Grid_Bimestre_2");
            TextBox Txt_Bimestre_3 = (TextBox)Adeudo.Cells[4].FindControl("Txt_Grid_Bimestre_3");
            TextBox Txt_Bimestre_4 = (TextBox)Adeudo.Cells[5].FindControl("Txt_Grid_Bimestre_4");
            TextBox Txt_Bimestre_5 = (TextBox)Adeudo.Cells[6].FindControl("Txt_Grid_Bimestre_5");
            TextBox Txt_Bimestre_6 = (TextBox)Adeudo.Cells[7].FindControl("Txt_Grid_Bimestre_6");

            // recuperar valores de las cajas de texto y agregar a una nueva fila
            Fila_Adeudo = Dt_Adeudos.NewRow();
            Fila_Adeudo[Ope_Pre_Adeudos_Predial.Campo_Anio] = Adeudo.Cells[1].Text;

            if (Decimal.TryParse(Txt_Bimestre_1.Text, out Valor_Decimal))
            {
                Fila_Adeudo["ADEUDO_BIMESTRE_1"] = Valor_Decimal.ToString("#,###,#0.00");
                Total += Valor_Decimal;
            }
            else
            {
                Validacion = false;
                Validacion_Suma = false;
            }

            if (Decimal.TryParse(Txt_Bimestre_2.Text, out Valor_Decimal))
            {
                Fila_Adeudo["ADEUDO_BIMESTRE_2"] = (Valor_Decimal.ToString("#,###,#0.00"));
                Total += Valor_Decimal;
            }
            else
            {
                Validacion = false;
                Validacion_Suma = false;
            }

            if (Decimal.TryParse(Txt_Bimestre_3.Text, out Valor_Decimal))
            {
                Fila_Adeudo["ADEUDO_BIMESTRE_3"] = (Valor_Decimal.ToString("#,###,#0.00"));
                Total += Valor_Decimal;
            }
            else
            {
                Validacion = false;
                Validacion_Suma = false;
            }

            if (Decimal.TryParse(Txt_Bimestre_4.Text, out Valor_Decimal))
            {
                Fila_Adeudo["ADEUDO_BIMESTRE_4"] = (Valor_Decimal.ToString("#,###,#0.00"));
                Total += Valor_Decimal;
            }
            else
            {
                Validacion = false;
                Validacion_Suma = false;
            }

            if (Decimal.TryParse(Txt_Bimestre_5.Text, out Valor_Decimal))
            {
                Fila_Adeudo["ADEUDO_BIMESTRE_5"] = (Valor_Decimal.ToString("#,###,#0.00"));
                Total += Valor_Decimal;
            }
            else
            {
                Validacion = false;
                Validacion_Suma = false;
            }

            if (Decimal.TryParse(Txt_Bimestre_6.Text, out Valor_Decimal))
            {
                Fila_Adeudo["ADEUDO_BIMESTRE_6"] = (Valor_Decimal.ToString("#,###,#0.00"));
                Total += Valor_Decimal;
            }
            else
            {
                Validacion = false;
                Validacion_Suma = false;
            }
            if (Validacion_Suma)
                Fila_Adeudo["ADEUDO_TOTAL_ANIO"] = Total.ToString("#,###,#0.00");
            else
                Fila_Adeudo["ADEUDO_TOTAL_ANIO"] = Adeudo.Cells[7].Text;
            Validacion_Suma = true;
            Total = 0;
            Dt_Adeudos.Rows.Add(Fila_Adeudo);
        }
        // validar que haya registros
        if (!Validacion)
        {
            Mensaje_Error("No se llenaron los adeudos correctamente");
        }

        return Dt_Adeudos;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN     : Validar_Campos_Obligatorios
    ///DESCRIPCIÓN              : Determina qué campo no cumplen los requisitos para estar completos y manda mensaje de advertencia
    ///PARAMETROS: 
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 17/Noviembre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private Boolean Validar_Campos_Obligatorios()
    {
        Boolean Resultado = true;
        try
        {
            if (String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
            {
                Mensaje_Error("+ Seleccione una Cuenta");
                Resultado = false;
            }
            if (Cmb_Movimientos.SelectedIndex <= 0)
            {
                Mensaje_Error("+ Seleccione un Tipo de Movimiento");
                Resultado = false;
            }
            if (Cmb_Solicitante.SelectedIndex <= 0 && Cmb_Financiado.SelectedIndex <= 0)
            {
                Mensaje_Error("+ Seleccione un Beneficio");
                Resultado = false;
            }
            if (Grid_Diferencias != null)
            {
                if (Grid_Diferencias.Rows.Count <= 0)
                {
                    Mensaje_Error("+ Agregue Diferencias");
                    Resultado = false;
                }
                //else
                //{
                //    foreach (GridViewRow Fila_Grid in Grid_Diferencias.Rows)
                //    {
                //        if (Convert.ToDouble(((TextBox)Fila_Grid.Cells[5].FindControl("Txt_Grid_Importe")).Text.Trim().Replace("$", "")) == 0)
                //        {
                //            Resultado = false;
                //        }
                //        //if (Convert.ToDouble(((TextBox)Fila_Grid.Cells[6].FindControl("Txt_Grid_Cuota_Bimestral")).Text.Trim().Replace("$", "")) == 0)
                //        //{
                //        //    Resultado = false;
                //        //}
                //    }
                //    if (!Resultado)
                //    {
                //        Mensaje_Error("+ Hay Diferencias sin Importe");
                //    }
                //}
            }
            else
            {
                Mensaje_Error("+ Agregue Diferencias");
                Resultado = false;
            }
            if (HttpUtility.HtmlDecode(Txt_Observaciones_Cuenta.Text).Trim() == "")
            {
                Mensaje_Error("+ Introduzca las Observaciones de la Baja");
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
    ///NOMBRE DE LA FUNCIÓN     : Cargar_Combo_Movimientos
    ///DESCRIPCIÓN              : Llena el combo de movimientos
    ///PROPIEDADES:         
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 18/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Combo_Movimientos()
    {
        try
        {
            Cls_Cat_Pre_Movimientos_Negocio Movimientos = new Cls_Cat_Pre_Movimientos_Negocio();
            Movimientos.P_Cargar_Modulos = "LIKE 'BAJAS_DIRECTAS'";
            DataTable Dt_Movimientos = Movimientos.Consultar_Movimientos_Bajas_Directas();
            DataRow Dr_Movimientos = Dt_Movimientos.NewRow();
            Dr_Movimientos[Cat_Pre_Movimientos.Campo_Movimiento_ID] = "SELECCIONE";
            Dr_Movimientos[Cat_Pre_Movimientos.Campo_Identificador] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
            Dt_Movimientos.Rows.InsertAt(Dr_Movimientos, 0);
            Cmb_Movimientos.DataSource = Dt_Movimientos;
            Cmb_Movimientos.DataValueField = Cat_Pre_Movimientos.Campo_Movimiento_ID;
            Cmb_Movimientos.DataTextField = Cat_Pre_Movimientos.Campo_Identificador;
            Cmb_Movimientos.DataBind();
        }
        catch (Exception Ex)
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Cargar_Grid_Bajas
    ///DESCRIPCIÓN              : Llena el grid con los registros de Bajas encontrados
    ///PROPIEDADES              : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 18/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Bajas(int Pagina)
    {
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            DataTable Dt_Ordenes_Bajas_Directas;
            Dt_Ordenes_Bajas_Directas = Ordenes_Variacion.Consultar_Ordenes_Bajas_Directas();
            Grid_Bajas.DataSource = Dt_Ordenes_Bajas_Directas;
            Grid_Bajas.PageIndex = Pagina;
            Grid_Bajas.DataBind();
        }
        catch (Exception Ex)
        {
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Grid_Bajas_PageIndexChanging
    ///DESCRIPCIÓN              : Evento PageIndexChanging del Grid de Bajas Directas
    ///PROPIEDADES:
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 18/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Bajas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Grid_Bajas.PageIndex = 0;
        Cargar_Grid_Bajas(e.NewPageIndex);
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Grid_Bajas_SelectedIndexChanged
    ///DESCRIPCIÓN              : Evento SelectedIndexChanged del Grid de Bajas Directas
    ///PROPIEDADES:
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 18/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Bajas_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Agregar_Diferencias;
        Cls_Ope_Pre_Orden_Variacion_Negocio Ordenes_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();

        Txt_Cuenta_Predial.Text = Grid_Bajas.SelectedRow.Cells[2].Text;
        Txt_Cuenta_Predial_TextChanged(null, EventArgs.Empty);
        Txt_Observaciones_Cuenta.Text = Grid_Bajas.DataKeys[Grid_Bajas.SelectedIndex].Values[2].ToString();
        try
        {
            Cmb_Movimientos.SelectedValue = Grid_Bajas.DataKeys[Grid_Bajas.SelectedIndex].Values[1].ToString();
        }
        catch
        {
        }

        Ordenes_Variacion.P_Cuenta_Predial_ID = Grid_Bajas.DataKeys[Grid_Bajas.SelectedIndex].Values[0].ToString();
        Ordenes_Variacion.P_Generar_Orden_No_Orden = Grid_Bajas.SelectedRow.Cells[1].Text;
        Ordenes_Variacion.P_Generar_Orden_Anio = Grid_Bajas.SelectedRow.Cells[6].Text.Substring(Grid_Bajas.SelectedRow.Cells[6].Text.Length - 4);
        Dt_Agregar_Diferencias = Ordenes_Variacion.Consulta_Diferencias();
        if (Dt_Agregar_Diferencias.Rows.Count > 0)
        {
            Session["Dt_Agregar_Diferencias"] = Dt_Agregar_Diferencias;
            Cargar_Grid_Diferencias(0);
            Calcular_Resumen();
            Resumen_Grid_Diferencias();
        }

        Btn_Salir.AlternateText = "Regresar";
        Btn_Salir.ToolTip = "Regresar";
        Panel_Bajas.Visible = false;
        Panel_Datos.Visible = true;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN     : Validar_Mes_Descuento
    ///DESCRIPCIÓN              : Valida que para el mes Actual exista un Descuento
    ///PROPIEDADES:
    ///CREO                     : Antonio Salvador Benavides Guardado
    ///FECHA_CREO               : 22/Noviembre/2011 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Mes_Descuento()
    {
        Cls_Cat_Pre_Descuentos_Predial_Negocio Descuentos_Predial = new Cls_Cat_Pre_Descuentos_Predial_Negocio();
        Boolean Mes_Valido = true;
        DateTime Fecha_Servidor = DateTime.MinValue;

        try
        {
            Fecha_Servidor = Convert.ToDateTime(Obtener_Dato_Consulta("SYSDATE", "DUAL", "", ""));
        }
        catch
        {
            Fecha_Servidor = DateTime.Now;
        }

        Descuentos_Predial.P_Mes = Fecha_Servidor.ToString("MMMM").ToUpper();
        Descuentos_Predial.P_Anio = Fecha_Servidor.Year;
        if (Descuentos_Predial.Consultar_Descuento_Mes() > 0)
        {
            Mes_Valido = true;
        }
        return Mes_Valido;
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
    private String Obtener_Dato_Consulta(String Campo, String Tabla, String Condiciones, String Dato_Default)
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

        if (Dato_Consulta == "")
        {
            Dato_Consulta = Dato_Default;
        }

        return Dato_Consulta;
    }
}
