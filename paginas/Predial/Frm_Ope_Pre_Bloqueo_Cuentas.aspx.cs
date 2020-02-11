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
using CrystalDecisions.CrystalReports.Engine;
using Presidencia.Reportes;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Bloqueo_Cuentas : System.Web.UI.Page
{

    #region Variables
    private const int Const_Estado_Inicial = 0;
    private const int Const_Estado_Nuevo = 2;
    private const int Const_Estado_Modificar = 3;

    private static String M_Cuenta_ID;
    private static DataTable Dt_Agregar_Co_Propietarios = new DataTable();
    private static DataTable Dt_Agregar_Diferencias = new DataTable();
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
                //Scrip para mostrar Ventana Modal de la Busqueda Avanzada de cuentas predial
                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Mostrar_Busqueda_Cuentas.Attributes.Add("onclick", Ventana_Modal);
                Session["ESTATUS_CUENTAS"] = "IN ('BLOQUEADA','ACTIVA','VIGENTE')";
                //String Ventana_Modal_Estado = "Abrir_Ventana_Estado_Cuenta('Ventanas_Emergentes/Resumen_Predial/Frm_Estado_Cuenta.aspx', 'center:yes;resizable:yes;status:no ;dialogWidth:680px;dialogHeight:800px;dialogHide:false;help:no ;scroll:yes');";
                //Btn_Estado_Cuenta.Attributes.Add("onclick", Ventana_Modal_Estado);
                Cargar_Grid_Cuentas_canceladas(0);
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
            Consulta_Combos();
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
        Txt_Cuenta_Predial.Text = "";
        Hdn_Cuenta_ID.Value = null;
        Hdn_Propietario_ID.Value = null;
        Txt_Cta_Origen.Text = "";
        Txt_Superficie_Construida.Text = "";
        Txt_Superficie_Total.Text = "";
        //Cmb_Calle_Cuenta.SelectedIndex = 0;
        Txt_Estatus.Font.Bold = false;
        Txt_Colonia_Cuenta.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Catastral.Text = "";
        //Propietario
        Txt_Nombre_Propietario.Text = "";
        //Txt_Rfc_Propietario.Text = "";
        //Txt_Colonia_Propietario.Text = "";
        //Txt_Calle_Propietario.Text = "";
        //Txt_Numero_Exterior_Propietario.Text = "";
        //Txt_Numero_Interior_Propietario.Text = "";
        //Txt_Estado_Propietario.Text = "";
        //Txt_Ciudad_Propietario.Text = "";
        //Txt_CP.Text = "";
        Txt_Tipos_Predio.Text = "";
        Txt_Calle_Cuenta.Text = "";

        //combos
        //Generales

        //Txt_Tipo_Propietario.Text = "";
        Txt_Usos_Predio.Text = "";
        Txt_Estados_Predio.Text = "";
        Txt_Estatus.Text = "";
        Txt_Efectos.Text = "";
        Session["Estatus_Cuenta"] = null;
        //propietarios
        //Txt_Tipo_Propietario.Text = "";
        //Txt_Domicilio_Foraneo.Text = "";        
        //Txt_Observaciones_Cuenta.Text = "";
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

            //Llenar_Combo_ID(Cmb_Estatus);
            //Llenar_Combo_ID(Cmb_Efectos);
            //Llenar_Combo_ID(Cmb_Tipos_Propietario);
            //Llenar_Combo_ID(Cmb_Domicilio_Foraneo);            
            //Llenar_Combo_ID(Cmb_Tipos_Predio, Ds_Cargar_combos.Tables["Dt_Tipos_Predio"]);            
            //Llenar_Combo_ID(Cmb_Usos_Predio, Ds_Cargar_combos.Tables["Dt_Usos_Predio"]);
            //Llenar_Combo_ID(Cmb_Estados_Predio, Ds_Cargar_combos.Tables["Dt_Estados_Predio"]);                        
            //Llenar_Combo_ID(Cmb_Colonia_Cuenta, Ds_Cargar_combos.Tables["Dt_Colonias"]);

            ////Cmb_Tipos_Movimiento
            //Cmb_Tipos_Propietario.Items.Add(new ListItem("PROPIETARIO", "PROPIETARIO"));
            //Cmb_Tipos_Propietario.Items.Add(new ListItem("COPROPIETARIO", "COPROPIETARIO"));            

            //Cmb_Domicilio_Foraneo.Items.Add(new ListItem("SI", "SI"));
            //Cmb_Domicilio_Foraneo.Items.Add(new ListItem("NO", "NO"));

            //Cmb_Estatus.Items.Add(new ListItem("ACTIVA", "ACTIVA"));
            //Cmb_Estatus.Items.Add(new ListItem("INACTIVA", "INACTIVA"));
            //Cmb_Estatus.Items.Add(new ListItem("CANCELADA", "CANCELADA"));

            //for (int anio = Int32.Parse(DateTime.Now.Year.ToString()) + 1; anio >= 1980; anio--)
            //{
            //    Cmb_Efectos.Items.Add(new ListItem(anio.ToString(), anio.ToString()));                
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


                Estado = false;
                Btn_Buscar.AlternateText = "Buscar";
                Btn_Nuevo.AlternateText = "Nuevo";
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = true;
                Btn_Modificar.AlternateText = "Modificar";
                Btn_Modificar.ToolTip = "Modificar";
                Btn_Salir.AlternateText = "Inicio";
                Btn_Buscar.ToolTip = "Buscar";
                Btn_Nuevo.ToolTip = "Nuevo";
                Btn_Salir.ToolTip = "Inicio";
                Btn_Buscar.ImageUrl = "~/paginas/imagenes/paginas/busqueda.png";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                Btn_Nuevo.Visible = true;
                Txt_Estatus.Enabled = false;

                break;

            case 2: //Nuevo

                Estado = false;

                Txt_Cuenta_Predial.Enabled = false;
                Btn_Mostrar_Busqueda_Cuentas.Enabled = true;
                Txt_Cuenta_Predial.Focus();
                Btn_Salir.Enabled = true;
                Btn_Nuevo.AlternateText = "Guardar";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Nuevo.ToolTip = "Guardar";
                Btn_Modificar.Visible = false;
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Txt_Estatus.Enabled = false;
                Txt_Cuenta_Predial.Focus();

                break;

            case 3: //Mod

                Estado = false;
                Txt_Estatus.Enabled = true;
                Txt_Estatus.ReadOnly = true;
                Txt_Estatus.Font.Bold = true;
                Txt_Estatus.Text = "VIGENTE";
                Txt_Cuenta_Predial.Enabled = false;
                Btn_Mostrar_Busqueda_Cuentas.Enabled = false;
                Btn_Salir.Enabled = true;
                Btn_Nuevo.Visible = false;
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Modificar.ToolTip = "Guardar";
                Btn_Modificar.AlternateText = "Guardar";
                Btn_Salir.ToolTip = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Txt_Cuenta_Predial.Focus();

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
        //Propietario
        Txt_Nombre_Propietario.Enabled = Estado;
        //Txt_Rfc_Propietario.Enabled = Estado;
        //Txt_Colonia_Propietario.Enabled = Estado;
        //Txt_Calle_Propietario.Enabled = Estado;
        //Txt_Numero_Exterior_Propietario.Enabled = Estado;
        //Txt_Numero_Interior_Propietario.Enabled = Estado;
        //Txt_Estado_Propietario.Enabled = Estado;
        //Txt_Ciudad_Propietario.Enabled = Estado;
        //Txt_CP.Enabled = Estado;

        //combos
        //Generales        
        Txt_Tipos_Predio.Enabled = Estado;
        Txt_Usos_Predio.Enabled = Estado;
        Txt_Estados_Predio.Enabled = Estado;
        Txt_Efectos.Enabled = Estado;
        //propietarios
        //Txt_Tipo_Propietario.Enabled = Estado;
        //Txt_Domicilio_Foraneo.Enabled = Estado;
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

            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Grid.SelectedIndex = (-1);
                Cuenta_Predial_ID = Session["CUENTA_PREDIAL_ID"].ToString();
                Cuenta_Predial = Session["CUENTA_PREDIAL"].ToString();
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                M_Cuenta_ID = Cuenta_Predial_ID;
                Txt_Cuenta_Predial_TextChanged(null, EventArgs.Empty);
                Session["CUENTA_PREDIAL_ID"] = null;
                Cargar_Ventana_Emergente_Resumen_Predio();
            }
            else
            {
                Btn_Estado_Cuenta.Attributes.Remove("onclick");
            }
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
        //Scrip para mostrar Ventana Modal del resumen de predio
        //String Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Resumen_Predio.aspx";
        //String Propiedades = ", 'center:yes;resizable:yes;status:no;dialogWidth:680px;dialogHide:true;help:no;scroll:yes');";
        String Ventana_Modal = "Abrir_Ventana_Estado_Cuenta('Ventanas_Emergentes/Resumen_Predial/Frm_Estado_Cuenta.aspx";
        String Propiedades = ", 'resizable=yes,status=no,width=750,scrollbars=yes');";
        Btn_Estado_Cuenta.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
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
            if (Btn_Salir.AlternateText.Equals("Inicio"))
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Limpiar_Todo();
                Estado_Botones(Const_Estado_Inicial);
                Grid.SelectedIndex = (-1);
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
                Estado_Botones(Const_Estado_Nuevo);
                Limpiar_Todo();
                Grid.PageIndex = 1;
                Grid.SelectedIndex = (-1);
            }
            else
            {
                if (!String.IsNullOrEmpty(Hdn_Cuenta_ID.Value))
                    Alta_Bloqueo(0);
            }
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Grid.SelectedIndex > (-1))
            {
                if (Btn_Modificar.AlternateText.Equals("Modificar"))
                {
                    Estado_Botones(Const_Estado_Modificar);
                }
                else
                {
                    Alta_Bloqueo(1);
                }
            }
            else
                Mensaje_Error("No hay ninguna cuenta seleccionada");
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }


    #endregion

    #region Metodos Grid
    ///******************************************************************************* 
    ///NOMBRE DE LA CLASE: Grid_Selectedindexchanged
    ///DESCRIPCIÓN: Seleccionar renglon del grid
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/24/2011 01:42:49 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Grid_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (Grid.SelectedIndex >= 0)
        {

            Limpiar_Todo();
            Session["CUENTA_PREDIAL_ID"] = null;
            Txt_Cuenta_Predial.Text = Grid.Rows[Grid.SelectedIndex].Cells[1].Text;
            Grid.Columns[6].Visible = true;
            Hdn_Cuenta_ID.Value = Grid.Rows[Grid.SelectedIndex].Cells[6].Text;
            Grid.Columns[6].Visible = false;
            Txt_Cuenta_Predial_TextChanged(sender, e);
        }
    }

    // llenar grid con columnas en blanco
    private void Cargar_Grid_Cuentas_canceladas(int Page_Index)
    {
        DataTable Tabla_Nueva = new DataTable();

        try
        {
            Grid.SelectedIndex = (-1);
            Cls_Ope_Pre_Orden_Variacion_Negocio Consulta_Cuentas_Canceladas = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Consulta_Cuentas_Canceladas.P_Filtros_Dinamicos = " CUEN." + Cat_Pre_Cuentas_Predial.Campo_Estatus + " = 'BLOQUEADA' ";
            Tabla_Nueva = Consulta_Cuentas_Canceladas.Consulta_Datos_Cuenta_Datos().Tables[0];
            Grid.Columns[6].Visible = true;
            Grid.PageIndex = Page_Index;
            Grid.DataSource = Tabla_Nueva;
            Grid.DataBind();
            Grid.Columns[6].Visible = false;
        }
        catch (Exception ex)
        {
            throw new Exception("Llenar_Grid: " + ex.Message.ToString(), ex);
        }
    }
    #endregion

    #region Metodos ABC
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Alta_Bloqueo
    ///DESCRIPCIÓN: Metodo para bloquear una cuenta predial
    ///PARAMETROS: 
    ///CREO: jtoledo
    ///FECHA_CREO: 10/24/2011 11:27:27 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    private void Alta_Bloqueo(int modo)
    {
        try
        {
            Cls_Ope_Pre_Orden_Variacion_Negocio Orden_Variacion = new Cls_Ope_Pre_Orden_Variacion_Negocio();
            Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
            Cuenta.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            if (modo == 0)
                Cuenta.P_Estatus = "BLOQUEADA";
            else
                Cuenta.P_Estatus = "VIGENTE";
            Cuenta.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Cuenta.Modifcar_Cuenta();
            //Orden_Variacion.P_Generar_Orden_Anio = DateTime.Now.Year.ToString();
            //Orden_Variacion.P_Generar_Orden_Cuenta_ID = Hdn_Cuenta_ID.Value;            
            //Orden_Variacion.P_Generar_Orden_Estatus = "REALIZADA";            
            //Orden_Variacion.Consulta_Id_Movimiento("CANCEL");
            //if (modo == 0)
            //Orden_Variacion.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_Estatus, "BLOQUEADA");
            //else
            //    Orden_Variacion.Agregar_Variacion(Cat_Pre_Cuentas_Predial.Campo_Estatus, "VIGENTE");
            ////Generar_Reporte(M_Orden_Negocio.Consulta_Datos_Reporte(Orden_Variacion.Generar_Orden_Variacion()));            
            //Orden_Variacion.Aplicar_Variacion_Orden();
            if (modo == 0)
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('El bloqueo de la cuenta se Guardó Exitosamente');", true);
            else
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Conceptos", "alert('Desbloqueo de la cuenta guardado exitosamente');", true);
            Limpiar_Todo();
            Estado_Botones(Const_Estado_Inicial);
            Cargar_Grid_Cuentas_canceladas(0);
        }
        catch (Exception Ex)
        {
            Mensaje_Error("Alta Bloqueo Error:[ " + Ex.Message + "]");
        }
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
        Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            if (!String.IsNullOrEmpty(Txt_Cuenta_Predial.Text.Trim()))
                Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            else
                Session["Cuenta_Predial"] = null;
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Hdn_Cuenta_ID.Value = Session["CUENTA_PREDIAL_ID"].ToString();
                M_Orden_Negocio.P_Cuenta_Predial_ID = Hdn_Cuenta_ID.Value;
            }
            else
            {
                M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            }
            M_Orden_Negocio.P_Contrarecibo = null;
            Ds_Cuenta = M_Orden_Negocio.Consulta_Datos_Cuenta_Sin_Contrarecibo();
            if (Ds_Cuenta.Tables[0].Rows.Count > 0)
            {
                Session.Remove("Ds_Cuenta_Datos");
                M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
                Limpiar_Todo();
                Session["Ds_Cuenta_Datos"] = Ds_Cuenta;
                Cargar_Datos();
                if (Session["Estatus_Cuenta"] != null)
                {
                    if (Session["Estatus_Cuenta"].ToString() == "BLOQUEADA" && Grid.SelectedIndex < 0)
                    {
                        Estado_Botones(Const_Estado_Inicial);
                        Mensaje_Error("La cuenta Esta Bloqueada");
                    }
                }
            }
            else
            {
                Estado_Botones(Const_Estado_Inicial);
                Limpiar_Todo();
                Mensaje_Error("No se encontraron datos relacionados con la búsqueda");
            }
            //}

        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Metodos Operacion
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
                Hdn_Propietario_ID.Value = dataTable.Rows[0]["PROPIETARIO"].ToString().Trim();
                //M_Orden_Negocio.P_Propietario_ID = dataTable.Rows[0]["PROPIETARIO"].ToString().Trim();

                Txt_Nombre_Propietario.Text = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString().Trim();
                // M_Orden_Negocio.P_Nombre_Propietario = dataTable.Rows[0]["NOMBRE_PROPIETARIO"].ToString().Trim();
                //if (dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString() != "")
                //{
                //    Txt_Tipo_Propietario.Text = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                //    M_Orden_Negocio.P_Tipo_Propietario = dataTable.Rows[0]["TIPO_PROPIETARIO"].ToString();
                //}

                //Txt_Rfc_Propietario.Text = dataTable.Rows[0]["RFC"].ToString();
                //M_Orden_Negocio.P_RFC_Propietario = dataTable.Rows[0]["RFC"].ToString();
                //if (dataTable.Rows[0]["FORANEO"].ToString() != String.Empty)
                //{
                //    Txt_Domicilio_Foraneo.Text = dataTable.Rows[0]["FORANEO"].ToString();
                //    M_Orden_Negocio.P_Domicilio_Foraneo = dataTable.Rows[0]["FORANEO"].ToString();
                //}
                //Txt_Estado_Propietario.Text = (dataTable.Rows[0]["ESTADO"].ToString());
                //M_Orden_Negocio.P_Estado_Propietario = (dataTable.Rows[0]["ESTADO"].ToString());
                //Txt_Colonia_Propietario.Text = dataTable.Rows[0]["COLONIA"].ToString();
                //M_Orden_Negocio.P_Colonia_Propietario = dataTable.Rows[0]["COLONIA"].ToString();
                //Txt_Ciudad_Propietario.Text = dataTable.Rows[0]["CIUDAD"].ToString();
                //M_Orden_Negocio.P_Ciudad_Propietario = dataTable.Rows[0]["CIUDAD"].ToString();
                //Txt_Calle_Propietario.Text = dataTable.Rows[0]["DOMICILIO"].ToString();
                //M_Orden_Negocio.P_Domilicio_Propietario = dataTable.Rows[0]["DOMICILIO"].ToString();
                //Txt_Numero_Exterior_Propietario.Text = dataTable.Rows[0]["EXTERIOR"].ToString();
                //M_Orden_Negocio.P_Exterior_Propietario = dataTable.Rows[0]["EXTERIOR"].ToString();
                //Txt_Numero_Interior_Propietario.Text = dataTable.Rows[0]["INTERIOR"].ToString();
                //M_Orden_Negocio.P_Interior_Propietario = dataTable.Rows[0]["INTERIOR"].ToString();
                //Txt_CP.Text = dataTable.Rows[0]["CP"].ToString();
                //M_Orden_Negocio.P_CP_Propietario = dataTable.Rows[0]["CP"].ToString();
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
        Cls_Ope_Pre_Orden_Variacion_Negocio M_Orden_Negocio = new Cls_Ope_Pre_Orden_Variacion_Negocio();
        try
        {
            //Asignacion de valores a Objeto de Negocio y cajas de texto            
            M_Orden_Negocio.P_Cuenta_Predial = Txt_Cuenta_Predial.Text.Trim();
            Hdn_Cuenta_ID.Value = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString().Trim();
            M_Orden_Negocio.P_Cuenta_Predial_ID = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString().Trim();
            Txt_Cuenta_Predial.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial].ToString().Trim();

            Txt_Cta_Origen.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString().Trim();
            M_Orden_Negocio.P_Cuenta_Origen = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Origen].ToString().Trim();
            if (dataTable.Rows[0]["TIPO_PREDIO_DESCRIPCION"].ToString() != string.Empty)
            {
                Txt_Tipos_Predio.Text = dataTable.Rows[0]["TIPO_PREDIO_DESCRIPCION"].ToString().Trim();
                M_Orden_Negocio.P_Tipo = dataTable.Rows[0]["TIPO_PREDIO_DESCRIPCION"].ToString().Trim();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Uso_Suelo_ID].ToString() != string.Empty)
            {
                M_Orden_Negocio.P_Uso_Suelo = dataTable.Rows[0]["USO_SUELO_DESCRIPCION"].ToString().Trim();
                Txt_Usos_Predio.Text = dataTable.Rows[0]["USO_SUELO_DESCRIPCION"].ToString().Trim();
            }
            if (dataTable.Rows[0]["ESTADO_PREDIO_DESCRIPCION"].ToString() != string.Empty)
            {
                Txt_Estados_Predio.Text = dataTable.Rows[0]["ESTADO_PREDIO_DESCRIPCION"].ToString().Trim();
                M_Orden_Negocio.P_Estado_Predial = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estado_Predio_ID].ToString().Trim();
            }
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() != string.Empty)
            {
                Txt_Estatus.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString().Trim();
                M_Orden_Negocio.P_Estatus_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString().Trim();
                Session["Estatus_Cuenta"] = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString();
            }
            Txt_Superficie_Construida.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString().Trim();
            M_Orden_Negocio.P_Superficie_Construida = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Construida].ToString().Trim();
            Txt_Superficie_Total.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString().Trim();
            M_Orden_Negocio.P_Superficie_Total = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Superficie_Total].ToString().Trim();
            if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString() != "")
            {
                Txt_Colonia_Cuenta.Text = dataTable.Rows[0]["NOMBRE_COLONIA"].ToString().Trim();
                M_Orden_Negocio.P_Colonia_Cuenta = dataTable.Rows[0][Cat_Pre_Calles.Campo_Colonia_ID].ToString().Trim();

                Txt_Calle_Cuenta.Text = dataTable.Rows[0]["NOMBRE_CALLE"].ToString().Trim();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString().Trim();
            }
            else if (dataTable.Rows[0][Cat_Pre_Calles.Campo_Calle_ID].ToString() != "")
            {
                Txt_Calle_Cuenta.Text = dataTable.Rows[0]["NOMBRE_CALLE"].ToString().Trim();
                M_Orden_Negocio.P_Ubicacion_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Calle_ID].ToString().Trim();
            }
            Txt_No_Exterior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim();
            M_Orden_Negocio.P_Exterior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString().Trim();
            Txt_No_Interior.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim();
            M_Orden_Negocio.P_Interior_Cuenta = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString().Trim();
            Txt_Catastral.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString().Trim();
            M_Orden_Negocio.P_Clave_Catastral = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Clave_Catastral].ToString().Trim();
            if (dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString() != string.Empty)
            {
                Txt_Efectos.Text = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString().Trim();
                M_Orden_Negocio.P_Efectos_Año = dataTable.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Efectos].ToString().Trim();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Datos_Cuenta: " + Ex.Message);
        }
    }

    protected void Grid_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Cargar_Grid_Cuentas_canceladas(e.NewPageIndex);
        Grid.SelectedIndex = (-1);
    }

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
        DataRow Renglon;
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


}
