using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Predial_Pae_Etapas.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Cargar_Honorarios : System.Web.UI.Page
{
    #region Page Load
    protected void Page_Load(object sender, EventArgs e)
    {
        String Ventana_Modal = "";
        try
        {
            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Estado_Formulario(false);//Habilita la configuración inicial de los controles de la página.                
            }
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Seleccionar_Colonia.Attributes.Add("onclick", Ventana_Modal);

            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Seleccionar_Calle.Attributes.Add("onclick", Ventana_Modal);

            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:450px;dialogHide:true;help:no;scroll:no');";
            Btn_Busca_Cuenta.Attributes.Add("onClick", Ventana_Modal);
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos
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
        Lbl_Encabezado_Error.Text = "";
        Lbl_Encabezado_Error.Text += P_Mensaje + "</br>";
        Lbl_Mensaje_Error.Text = "";

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
    private void Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Encabezado_Error.Text = "";
        Lbl_Mensaje_Error.Text = "";

    }
    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Limpiar_Formulario
    ///DESCRIPCION : Limpia los controles del formulario
    ///PARAMETROS  : 
    ///CREO        : Armando Zavala Moreno
    ///FECHA_CREO  : 01-Febrero-2011
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Limpiar_Formulario()
    {
        foreach (Control Txt_Lmpia in Div_Generadas.Controls)
        {
            if (Txt_Lmpia is TextBox)
            {
                ((TextBox)Txt_Lmpia).Text = "";
            }
        }
        Grid_Generadas.DataSource = null;
        //Grid_Determinaciones_Generadas.DataBind();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Estado_Formulario
    ///DESCRIPCIÓN: Establece en que estado esta el formulario, si esta disponible para guardar,
    ///             crear una nueva determinacion.
    ///PARAMETROS:  Estado, Estado en el que se cargara la configuración de los
    ///                            controles.
    ///CREO:        Armando Zavala Moreno
    ///FECHA_CREO:  02/02/2012 04:00:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    ///
    private void Estado_Formulario(Boolean Estado)
    {
        Limpiar_Formulario();
        if (Estado == true)//Si es verdadero se activan las opciones para guardar
        {
            Mensaje_Error();
            Limpiar_Formulario();
            Cargar_Combo_Despachos_Externos(Cmb_Asignado_a);
            Lbl_Msg_Col.Visible = false;
        }
        else
        {
            Mensaje_Error();
            Btn_Guardar.Visible = false;
            Limpiar_Formulario();
            Cargar_Combo_Despachos_Externos(Cmb_Asignado_a);
            Lbl_Msg_Col.Visible = false;
        }
    }
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
    private void Cargar_Combo_Despachos_Externos(DropDownList Cmb_Asignado_a)
    {
        DataTable Dt_Despachos = new DataTable();
        try
        {
            Cls_Cat_Pre_Despachos_Externos_Negocio Despachos_Externos = new Cls_Cat_Pre_Despachos_Externos_Negocio();
            Despachos_Externos.P_Filtro = "";
            Cmb_Asignado_a.DataTextField = Cat_Pre_Despachos_Externos.Campo_Despacho;
            Cmb_Asignado_a.DataValueField = Cat_Pre_Despachos_Externos.Campo_Despacho_Id;

            Dt_Despachos = Despachos_Externos.Consultar_Despachos_Externos();

            foreach (DataRow Dr_Fila in Dt_Despachos.Rows)
            {
                if (Dr_Fila[Cat_Pre_Despachos_Externos.Campo_Estatus].ToString() != "VIGENTE")//Busca el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Asignado_a.DataSource = Dt_Despachos;
            Cmb_Asignado_a.DataBind();
            Cmb_Asignado_a.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));

        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_PAE
    ///DESCRIPCIÓN: Metodo usado para buscar las cuentas por filtros
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 10/03/2012 12:55:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Buscar_PAE()
    {
        Cls_Ope_Pre_Pae_Etapas_Negocio Etapas_Pae = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Generadas = Crear_Tabla_Generadas();//Se crea la tabla para pasarla algrid
        DataTable Dt_Busqueda;
        Boolean Terminar=false;

        Mensaje_Error();//Limpia el mensaje error

        try
        {
            Grid_Generadas.Columns[9].Visible = true;
            Grid_Generadas.Columns[10].Visible = true;
            Grid_Generadas.Columns[11].Visible = true;
            Lbl_Msg_Col.Visible = false;

            if (Txt_Folio_Inicial.Text.Length > 0 || Txt_Folio_Final.Text.Length > 0)
            {
                if (Cmb_Asignado_a.SelectedIndex < 1)
                {
                    Mensaje_Error("Selecciona un despacho");
                    Terminar = true;
                }
            }
            if(Terminar!=true)
            {
                if (Cmb_Etapa.SelectedIndex > 0)
                {
                    Etapas_Pae.P_Proceso_Actual = Cmb_Etapa.SelectedItem.Text;
                }
                if (Txt_Numero_Cuenta.Text.Length > 0)
                {
                    Etapas_Pae.P_Cuenta_Predial = Txt_Numero_Cuenta.Text;//Asigna el ID de la Cuenta Predial                
                }

                if (Cmb_Asignado_a.SelectedIndex > 0)//Si la busqueda es por cuentas asignadas al despacho determinado
                {
                    Etapas_Pae.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                }

                if (Cmb_Estatus.SelectedIndex > 0)
                {
                    Etapas_Pae.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                }
                if (Txt_Folio_Inicial.Text.Length > 0)
                {
                    Etapas_Pae.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                }
                if (Txt_Folio_Final.Text.Length > 0)
                {
                    Etapas_Pae.P_Folio_Final = Txt_Folio_Final.Text;
                }
                
                if (!String.IsNullOrEmpty(Hdn_Colonia_ID.Value.ToString()))
                {
                    Etapas_Pae.P_Colonia_ID = Hdn_Colonia_ID.Value.ToString();
                    Lbl_Msg_Col.Visible = true;
                }
                if (!String.IsNullOrEmpty(Hdn_Calle_ID.Value.ToString()))
                {                    
                    Etapas_Pae.P_Calle_ID = Hdn_Calle_ID.Value.ToString();
                    Lbl_Msg_Col.Visible = true;
                }

                Dt_Busqueda = Etapas_Pae.Consulta_Cuentas_Honorarios();
                for (int Conta_Tabla = 0; Conta_Tabla < Dt_Busqueda.Rows.Count; Conta_Tabla++)
                {
                    Llenar_DataRow_Generadas(Dt_Generadas, Dt_Busqueda, Consulta_Estatus_Convenio(Dt_Busqueda.Rows[Conta_Tabla][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString()), Conta_Tabla);
                }

                Grid_Generadas.DataSource = Dt_Generadas;
                Grid_Generadas.DataBind();
                Grid_Generadas.Columns[9].Visible = false;
                Grid_Generadas.Columns[10].Visible = false;
                Grid_Generadas.Columns[11].Visible = false;
                Session["Grid_Generadas"] = Dt_Generadas;

                if (Dt_Generadas.Rows.Count < 1)
                {
                    Mensaje_Error("No se encontraron Etapas con esos parametros");
                }
                Lbl_Msg_Col.Text = "Colonia: " + Txt_Colonia.Text + " Calle: " + Txt_Calle.Text;
                Hdn_Calle_ID.Value = "";
                Hdn_Colonia_ID.Value = "";
                Txt_Calle.Text = "";
                Txt_Colonia.Text = "";
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consulta_Estatus_Convenio
    /// DESCRIPCIÓN: Busca una cuenta predial para verificar cual es el estado de su convenio y de su cuenta predial
    ///              
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: La cuenta que se va a consultar
    /// CREO: Armando Zavala Moreno
    /// FECHA_CREO: 09-Feb-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public String Consulta_Estatus_Convenio(String Cuenta_Predial_ID)
    {
        String Convenio = "";
        DataTable Dt_Convenio;
        
        Cls_Ope_Pre_Convenios_Predial_Negocio Consulta_Convenios = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        //Consulta si la cuenta tiene convenio
        Consulta_Convenios.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
        Consulta_Convenios.P_Ordenar_Dinamico = Ope_Pre_Convenios_Predial.Campo_Fecha + " DESC," + Ope_Pre_Convenios_Predial.Campo_No_Convenio + " DESC";
        Dt_Convenio = Consulta_Convenios.Consultar_Convenio_Predial();                

        if (Dt_Convenio.Rows.Count < 1)
            Convenio = "NO";//Si esta disponible para cargar honorarios por que no tiene convenio
        else
        {
            if (Dt_Convenio.Rows[0]["ESTATUS"].ToString() == "ACTIVO")
            {
                Convenio = "SI";//Es cuenta omitida por que tiene convenio
            }
            if (Dt_Convenio.Rows[0]["ESTATUS"].ToString() == "INCUMPLIDO")
                Convenio = "NO";//Si esta disponible para cargar honorarios
            if (Dt_Convenio.Rows[0]["ESTATUS"].ToString() == "TERMINADO")
                Convenio = "NO";//Si esta disponible para cargar honorarios
        }
        return Convenio;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Busqueda_Colonia_Calle
    ///DESCRIPCIÓN: Busca la colonia, calle y pasa los datos a los textbox.            
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 02/02/2012 05:20:09 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    ///
    private void Busqueda_Colonia_Calle()
    {
        try
        {
            if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
                {
                    Hdn_Colonia_ID.Value = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    Txt_Colonia.Text = Session["NOMBRE_COLONIA"].ToString().Replace("&nbsp;", "");
                    Hdn_Calle_ID.Value = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    Txt_Calle.Text = Session["NOMBRE_CALLE"].ToString().Replace("&nbsp;", "");
                    Session.Remove("BUSQUEDA_COLONIAS_CALLES");
                    Session.Remove("COLONIA_ID");
                    Session.Remove("NOMBRE_COLONIA");
                    Session.Remove("CALLE_ID");
                    Session.Remove("NOMBRE_CALLE");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }

    #region Tablas
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Generadas
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las generadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 09/02/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Generadas()
    {
        DataTable Dt_Generadas = new DataTable();
        Dt_Generadas.Columns.Add(new DataColumn("CUENTA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ADEUDO", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("FOLIO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ASIGNADO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ENTREGA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("FECHA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ETAPA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("NO_DETALLE_ETAPA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("HONORARIOS_PAGAR", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("OMITIDA", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("CONVENIO", typeof(String)));
        return Dt_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Generadas
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas Generadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 11:49:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Generadas(DataTable Dt_Generadas, DataTable Dt_Busqueda, String Convenio, int Contador)
    {
        DataRow Dr_Nueva_Fila;
        String Fecha_Notificacion;
        Fecha_Notificacion = Dt_Busqueda.Rows[Contador]["FECHA_NOTIFICACION"].ToString();
        Fecha_Notificacion = (!String.IsNullOrEmpty(Fecha_Notificacion)) ? Fecha_Notificacion : "";

        Dr_Nueva_Fila = Dt_Generadas.NewRow();
        Dr_Nueva_Fila["CUENTA"] = Dt_Busqueda.Rows[Contador]["CUENTA_PREDIAL"].ToString();
        Dr_Nueva_Fila["ADEUDO"] = Dt_Busqueda.Rows[Contador]["TOTAL"].ToString();
        Dr_Nueva_Fila["FOLIO"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Folio].ToString();
        Dr_Nueva_Fila["ASIGNADO"] = Dt_Busqueda.Rows[Contador][Cat_Pre_Despachos_Externos.Campo_Despacho].ToString(); ;
        Dr_Nueva_Fila["ENTREGA"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Etapas.Campo_Numero_Entrega].ToString();
        Dr_Nueva_Fila["FECHA"] = Fecha_Notificacion;
        Dr_Nueva_Fila["ETAPA"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Proceso_Actual].ToString();
        Dr_Nueva_Fila["ESTATUS"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Estatus].ToString();
        Dr_Nueva_Fila["NO_DETALLE_ETAPA"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa].ToString();
        Dr_Nueva_Fila["HONORARIOS_PAGAR"] = Dt_Busqueda.Rows[Contador]["HONORARIOS_PAGAR"].ToString();
        Dr_Nueva_Fila["OMITIDA"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Omitida].ToString();
        Dr_Nueva_Fila["CONVENIO"] = (!String.IsNullOrEmpty(Convenio)) ? Convenio : "NO";
        Dt_Generadas.Rows.Add(Dr_Nueva_Fila);//Se asigna la nueva fila a la tabla
    }
    #endregion

    #endregion

    #region Eventos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Seleccionar_Colonia_Click
    ///DESCRIPCIÓN          : Llama un formulario para buscar la colonia
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 09:14:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Colonia_Click(object sender, ImageClickEventArgs e)
    {
        Busqueda_Colonia_Calle();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Seleccionar_Calle_Click
    ///DESCRIPCIÓN          : Llama un formulario para buscar la calle
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 04:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Seleccionar_Calle_Click(object sender, ImageClickEventArgs e)
    {
        Busqueda_Colonia_Calle();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Busca_Pae_Click
    ///DESCRIPCIÓN          : Llena el Grid de determinaciones Generadas, con una consulta
    ///                       que tiene varios filtros
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 10:32:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busca_Cuentas_Click(object sender, ImageClickEventArgs e)
    {
        Buscar_PAE();
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta) o Sale del Formulario.
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
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
                HttpContext.Current.Session.Remove("Activa");
            }
            else
            {
                Btn_Salir.AlternateText = "Salir";
                Estado_Formulario(false);
                HttpContext.Current.Session.Remove("Activa");
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Busca_Cuenta_Click
    ///DESCRIPCIÓN          : Obtiene la cuenta predial de la ventana emergente por medio de las sessiones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busca_Cuenta_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Session["BUSQUEDA_CUENTAS_PREDIAL"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]) == true)
                {
                    Txt_Numero_Cuenta.Text = HttpUtility.HtmlDecode(Session["CUENTA_PREDIAL"].ToString().Replace("&nbsp;", ""));
                    Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
                    Session.Remove("CUENTA_PREDIAL");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Click
    ///DESCRIPCIÓN          : Obtiene la cuenta predial de la ventana emergente por medio de las sessiones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
            DataTable Dt_Generadas = Crear_Tabla_Generadas();//Se crea la tabla para pasarla algrid
            DataTable Dt_Busqueda = null;
            Mensaje_Error();
            Grid_Generadas.Columns[9].Visible = true;
            Grid_Generadas.Columns[10].Visible = true;
            Grid_Generadas.Columns[11].Visible = true;

            Rs_Etapas.P_Folio = Txt_Busqueda.Text;
            Dt_Busqueda = Rs_Etapas.Consulta_Cuentas_Honorarios();
            for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
            {
                Llenar_DataRow_Generadas(Dt_Generadas, Dt_Busqueda, Consulta_Estatus_Convenio(Dt_Busqueda.Rows[Contador][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString()), Contador);
            }
            Grid_Generadas.DataSource = Dt_Generadas;
            Grid_Generadas.DataBind();
            Grid_Generadas.Columns[9].Visible = false;
            Grid_Generadas.Columns[10].Visible = false;
            Grid_Generadas.Columns[11].Visible = false;
            Session["Grid_Generadas"] = Dt_Generadas;
            Txt_Busqueda.Text = "";
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Chk_Todos_CheckedChanged
    ///DESCRIPCIÓN          : Carga los honorarios a las cuentas que fueron seleccionadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 16/05/2012 10:07:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Todos_CheckedChanged(object sender, EventArgs e)
    { 
        CheckBox Chk_Todos=(CheckBox)Grid_Generadas.HeaderRow.FindControl("Chk_Todos");
        StringBuilder Cuentas = new StringBuilder();
        Mensaje_Error();

        if (Chk_Todos.Checked == true)
        {
            foreach (GridViewRow Recorer_Generadas in Grid_Generadas.Rows)
            {
                CheckBox Chk_Uno = (CheckBox)Recorer_Generadas.FindControl("Chk_Uno");
                Chk_Uno.Checked = true;
                Cuentas.Append(Grid_Generadas.DataKeys[Recorer_Generadas.RowIndex].Value.ToString() + " ");
                Recorer_Generadas.ForeColor = System.Drawing.Color.Black;
            }
            Mensaje_Error(Cuentas.ToString());
        }        
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Chk_Todos_CheckedChanged
    ///DESCRIPCIÓN          : Carga los honorarios a las cuentas que fueron seleccionadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 16/05/2012 10:07:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Chk_Uno_CheckedChanged(object sender, EventArgs e)
    {
        Mensaje_Error();
        foreach (GridViewRow Recorer_Generadas in Grid_Generadas.Rows)
        {
            CheckBox Chk_Uno = (CheckBox)Recorer_Generadas.FindControl("Chk_Uno");
            if (Chk_Uno.Checked)
            {
                Btn_Guardar.Visible = true;
                break;
            }
            else
                Btn_Guardar.Visible = false;
        }
    }
     ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Guardar_Click
    ///DESCRIPCIÓN          : Obtiene la cuenta predial de la ventana emergente por medio de las sessiones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/02/2012 06:43:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Guardar_Click(object sender, ImageClickEventArgs e)
    {
        StringBuilder Cuentas = new StringBuilder();
        Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        Cls_Ope_Pre_Pae_Honorarios_Negocio Rs_Honoraios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        DataTable Dt_Gastos = new DataTable();
        DataTable Dt_Consulta_Honorario=new DataTable();
        try
        {

            Mensaje_Error();
            foreach (GridViewRow Recorer_Generadas in Grid_Generadas.Rows)
            {
                CheckBox Chk_Uno = (CheckBox)Recorer_Generadas.FindControl("Chk_Uno");
                if (Chk_Uno.Checked)
                {
                    Rs_Honoraios.P_No_Detalle_Etapa = Grid_Generadas.DataKeys[Recorer_Generadas.RowIndex].Value.ToString();
                    Dt_Consulta_Honorario = Rs_Honoraios.Consultar_Total_Honorarios();
                    if (Dt_Consulta_Honorario.Rows.Count > 0)
                    {
                        Rs_Etapas.P_No_Detalle_Etapa = Grid_Generadas.DataKeys[Recorer_Generadas.RowIndex].Value.ToString();
                        Rs_Etapas.P_Adeudo_Honorarios = Dt_Consulta_Honorario.Rows[0]["TOTAL_HONORARIOS"].ToString();
                        Rs_Etapas.Actualiza_Pae_Det_Etapas();
                    }
                }
            }

            ScriptManager.RegisterStartupScript(this, this.GetType(), "Carga honorarios", "alert('Alta Exitosa');", true);
            int Pagina = Grid_Generadas.PageIndex;
            Buscar_PAE();
            Grid_Generadas.PageIndex = Pagina;
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
    #endregion

    #region Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Generadas_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de seguimiento a Pae
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/03/2012 05:44:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Generadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Generadas.Columns[9].Visible = true;
            Grid_Generadas.Columns[10].Visible = true;
            Grid_Generadas.Columns[11].Visible = true;

            Grid_Generadas.PageIndex = e.NewPageIndex;
            Grid_Generadas.DataSource = Session["Grid_Generadas"];
            Grid_Generadas.DataBind();

            Grid_Generadas.Columns[9].Visible = false;
            Grid_Generadas.Columns[10].Visible = false;
            Grid_Generadas.Columns[11].Visible = false;
        }
        catch (Exception ex) { Mensaje_Error(ex.Message); }
    }
    ///********************************************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Generadas_OnRowDataBound
    ///DESCRIPCIÓN          : Evento del grid del registro que seleccionaremos
    ///PROPIEDADES          :
    ///CREO                 : Armando Zavala Moreno
    /// FECHA_CREO          : 17/Mayo/2012
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN...:
    ///*********************************************************************************************************
    protected void Grid_Generadas_OnRowDataBound(object sender, GridViewRowEventArgs e)
    {
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (e.Row.Cells[11].Text == "SI")//Si tiene Convenio el color es rojo y no se pueden cargar honorarios
                {
                    e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#D2B48C");
                    e.Row.Cells[0].Enabled = false;
                }
                else
                {
                    if (Convert.ToDecimal(e.Row.Cells[9].Text) < 1)//Si no tiene adeduos de honorarios para recargar
                    {
                        e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FFFFCC");
                        e.Row.Cells[0].Enabled = false;
                    }
                    else
                    {
                        if (Convert.ToDecimal(e.Row.Cells[9].Text) > 0)//Si tiene adeduos de honorarios
                        {
                            if (e.Row.Cells[10].Text == "SI")//Si la cuenta esta omitida por cualquier razon no se pueden cargar honorarios
                            {
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#FF6633");
                                e.Row.Cells[0].Enabled = false;
                            }
                            else
                            {
                                e.Row.BackColor = System.Drawing.ColorTranslator.FromHtml("#CCFFCC");
                            }
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion
}
