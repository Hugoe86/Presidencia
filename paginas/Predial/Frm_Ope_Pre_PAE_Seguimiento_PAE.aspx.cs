using System;
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

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Seguimiento_PAE : System.Web.UI.Page
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
        }
        else
        {
            Mensaje_Error();
            Limpiar_Formulario();
            Cargar_Combo_Despachos_Externos(Cmb_Asignado_a);
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
        Boolean Terminar = false;

        Mensaje_Error();//Limpia el mensaje error
        
        try
        {
            if (Txt_Folio_Inicial.Text.Length > 0 || Txt_Folio_Final.Text.Length > 0)
            {
                if (Cmb_Asignado_a.SelectedIndex < 1)
                {
                    Mensaje_Error("Selecciona un despacho");
                    Terminar = true;
                }
            }
            if (Terminar != true)
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
                Dt_Busqueda = Etapas_Pae.Consultar_Pae_Det_Etapas();
                for (int Conta_Tabla = 0; Conta_Tabla < Dt_Busqueda.Rows.Count; Conta_Tabla++)
                {
                    Llenar_DataRow_Generadas(Dt_Generadas, Dt_Busqueda, Conta_Tabla);
                }

                Grid_Generadas.DataSource = Dt_Generadas;
                Grid_Generadas.DataBind();
                Session["Grid_Generadas"] = Dt_Generadas;

                if (Dt_Generadas.Rows.Count < 1)
                {
                    Mensaje_Error("No se encontraron Etapas con esos parametros");
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
        //Dt_Generadas.Columns.Add(new DataColumn("CUENTA_PREDIAL_ID", typeof(String)));
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
    protected void Llenar_DataRow_Generadas(DataTable Dt_Generadas, DataTable Dt_Busqueda, int Contador)
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
        //Dr_Nueva_Fila["CUENTA_PREDIAL_ID"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id].ToString();
        Dt_Generadas.Rows.Add(Dr_Nueva_Fila);//Se asigna la nueva fila a la tabla
    }
    #endregion
    #endregion

    #region Eventos
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
    protected void Btn_Busca_Pae_Click(object sender, ImageClickEventArgs e)
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
            Rs_Etapas.P_Folio = Txt_Busqueda.Text;
            Dt_Busqueda = Rs_Etapas.Consultar_Pae_Det_Etapas();
            for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
            {
                Llenar_DataRow_Generadas(Dt_Generadas, Dt_Busqueda, Contador);
            }
            Grid_Generadas.DataSource = Dt_Generadas;
            Grid_Generadas.DataBind();
            Session["Grid_Generadas"] = Dt_Generadas;
            Txt_Busqueda.Text = "";
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region Grids
       ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Determinaciones_Generadas_SelectedIndexChanged
    ///DESCRIPCIÓN          : Recibe una variable de session con los gastos de ejecucion que se crearon
    ///                       e inserta los registros de los gastos en la tabla de Honorarios
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 06/03/2012 01:16:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Generadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Pae_Honorarios_Negocio Rs_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Pae_Det_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Honorarios = (DataTable)Session["HONORARIOS"];
        DataTable Dt_Cambios = (DataTable)Session["CAMBIOS"];
        DataTable Dt_Generadas = (DataTable)Session["Grid_Generadas"];
        String No_Detalle_Etapa;
        String Estatus = "";

        if (Dt_Honorarios != null || Dt_Cambios != null)
        {
            try
            {
                No_Detalle_Etapa = Dt_Generadas.Rows[Grid_Generadas.SelectedIndex + (Grid_Generadas.PageSize * Grid_Generadas.PageIndex)]["NO_DETALLE_ETAPA"].ToString();
                if (Dt_Cambios != null)
                {
                    Rs_Pae_Det_Etapas.P_Estatus = Dt_Cambios.Rows[0]["ESTATUS"].ToString();
                    Rs_Pae_Det_Etapas.P_Motivo_Cambio_Estatus = Dt_Cambios.Rows[0]["MOTIVO"].ToString();
                    Rs_Pae_Det_Etapas.P_Resolucion = Dt_Cambios.Rows[0]["RESOLUCION"].ToString();
                    Estatus = Dt_Cambios.Rows[0]["ESTATUS"].ToString();
                }
                else
                {
                    Estatus = Dt_Generadas.Rows[Grid_Generadas.SelectedIndex + (Grid_Generadas.PageSize * Grid_Generadas.PageIndex)]["ESTATUS"].ToString(); 
                }

                if (Dt_Honorarios != null)
                {
                    for (int Conta_Ejecucion = 0; Conta_Ejecucion < Dt_Honorarios.Rows.Count; Conta_Ejecucion++)
                    {
                        Rs_Honorarios.P_No_Detalle_Etapa = No_Detalle_Etapa;
                        Rs_Honorarios.P_Gasto_Ejecucion_Id = Dt_Honorarios.Rows[Conta_Ejecucion]["GASTO_EJECUCION_ID"].ToString();
                        Rs_Honorarios.P_Proceso = Dt_Generadas.Rows[Grid_Generadas.SelectedIndex + (Grid_Generadas.PageSize * Grid_Generadas.PageIndex)]["ETAPA"].ToString(); 
                        Rs_Honorarios.P_Importe = Dt_Honorarios.Rows[Conta_Ejecucion]["IMPORTE"].ToString();
                        Rs_Honorarios.P_Estatus = Estatus;
                        Rs_Honorarios.Alta_Honorario();
                    }
                }

                Session.Remove("NO_DETALLE_ETAPA");
                Session.Remove("HONORARIOS");

                Rs_Pae_Det_Etapas.P_No_Detalle_Etapa = No_Detalle_Etapa;
                Rs_Pae_Det_Etapas.Actualiza_Pae_Det_Etapas();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento a Requerimientos", "alert('Alta Exitosa');", true);
                int Pagina = Grid_Generadas.PageIndex;
                Buscar_PAE();
                Session["Grid_Generadas"] = Dt_Generadas;
                Grid_Generadas.PageIndex = Pagina;
            }
            catch (Exception Ex)
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }
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
            Grid_Generadas.PageIndex = e.NewPageIndex;
            Grid_Generadas.DataSource = Session["Grid_Generadas"];
            Grid_Generadas.DataBind();
        }
        catch (Exception ex) { Mensaje_Error(ex.Message); }
    }
    #endregion
}
