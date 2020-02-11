using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Predial_Pae_Etapas.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Gastos_Ejecucion.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Predial_Pae_Publicaciones.Negocio;
using Presidencia.Predial_Pae_Notificaciones.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Seguimiento_A_Embargos : System.Web.UI.Page
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
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/PAE/Frm_Busqueda_Contribuyentes_PAE.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:450px;dialogHide:true;help:no;scroll:no');";            
            Btn_Busca_Contribuyente.Attributes.Add("onClick", Ventana_Modal);
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region Metodos
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
        Grid_Embargos_Generados.DataSource = null;
        //Grid_Determinaciones_Generadas.DataBind();
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
        Lbl_Encabezado_Error.Text = "";
        //Lbl_Mensaje_Error.Text += P_Mensaje + "</br>";
        Lbl_Encabezado_Error.Text += P_Mensaje + "</br>";
        Lbl_Mensaje_Error.Text = "";

    }
    private void Limpia_Mensaje_Error()
    {
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Encabezado_Error.Text = "";
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
            Limpia_Mensaje_Error();
            Limpiar_Formulario();
            Cargar_Combo_Despachos_Externos(Cmb_Asignado_a);
        }
        else
        {
            Limpia_Mensaje_Error();
            Limpiar_Formulario();
            Cargar_Combo_Despachos_Externos(Cmb_Asignado_a);
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Validar_Componentes
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación.
    ///PARAMETROS: 
    ///CREO:        Armando Zavala Moreno
    ///FECHA_CREO:  09/02/2012 06:24:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Boolean Validacion = true;
        Limpia_Mensaje_Error();

        if (Cmb_Asignado_a.SelectedIndex < 1)
        {
            Mensaje_Error("Seleccione un Despacho");
            Validacion = false;
        }
        if (Grid_Embargos_Generados.Rows.Count < 1)
        {
            Mensaje_Error("No existen Cuentas");
            Validacion = false;
        }

        return Validacion;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Embargos_Generados
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las determiancion generadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 09/02/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Embargos_Generados()
    {
        DataTable Dt_Determinaciones_Generadas = new DataTable();
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("CUENTA", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ADEUDO", typeof(Decimal)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("FOLIO", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ASIGNADO", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ENTREGA", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("FECHA", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("NO_DETALLE_ETAPA", typeof(String)));
        return Dt_Determinaciones_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Determinadas
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas omitidas y Calcula el Perido, Rezago, etc
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 11:49:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Embargos(DataTable Dt_Generados, DataTable Dt_Busqueda, Int32 Cont_Busqueda)
    {
        DataRow Dr_Determinadas;
        Dr_Determinadas = Dt_Generados.NewRow();
        String Fecha_Notificacion = Dt_Busqueda.Rows[Cont_Busqueda]["FECHA_NOTIFICACION"].ToString();
        if (Fecha_Notificacion == null || Fecha_Notificacion == "")
        {
            Fecha_Notificacion = "";
        }
        Dr_Determinadas["CUENTA"] = Dt_Busqueda.Rows[Cont_Busqueda]["CUENTA_PREDIAL"].ToString(); ;
        Dr_Determinadas["ADEUDO"] = Dt_Busqueda.Rows[Cont_Busqueda]["TOTAL"].ToString();
        Dr_Determinadas["FOLIO"] = Dt_Busqueda.Rows[Cont_Busqueda][Ope_Pre_Pae_Det_Etapas.Campo_Folio].ToString();
        Dr_Determinadas["ASIGNADO"] = Dt_Busqueda.Rows[Cont_Busqueda][Cat_Pre_Despachos_Externos.Campo_Despacho].ToString();
        Dr_Determinadas["ENTREGA"] = Dt_Busqueda.Rows[Cont_Busqueda][Ope_Pre_Pae_Etapas.Campo_Numero_Entrega].ToString();
        Dr_Determinadas["FECHA"] = Fecha_Notificacion;
        Dr_Determinadas["ESTATUS"] = Dt_Busqueda.Rows[Cont_Busqueda][Ope_Pre_Pae_Det_Etapas.Campo_Estatus].ToString();
        Dr_Determinadas["NO_DETALLE_ETAPA"] = Dt_Busqueda.Rows[Cont_Busqueda][Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa].ToString();
        Dt_Generados.Rows.Add(Dr_Determinadas);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Comprobar_Filtros_Determinacione
    ///DESCRIPCIÓN          : Comprueba que la nueva fila que se agrego en la tabla de determinaciones generadas
    ///                       cumpla con los demas filtros seleccionados, si no cumple es borrada,
    ///                       regresa el numero de posicion en la tabla Dt_Determinaciones_Generadas
    ///PARAMETROS:          : 1.-Dt_Generadas: Se guardan los registros que cumplen con los filtos    ///                     : 
    ///                     : 2.-Cont_Borrado: Posicion actual de la tabla Dt_Determinaciones_Generadas
    ///                     : 3.-Cont_Det_Etapas: Posicion actual de la tabla Dt_Pre_Pae_Det_Etapas
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 06:32:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Comprobar_Filtros_Busqueda(DataTable Dt_Generadas, int Cont_Borrado, int Cont_Det_Etapas)
    {
        Boolean Fila_Borrada = false;

        if (Txt_Fecha_Inicial.Text.Length > 0 && Fila_Borrada != true)
        {
            DateTime Fecha_Inicial;
            Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text);
            String Fecha_Notificacion = Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString();//Es el campo de la tabla que acaba de ser creada
            if (!String.IsNullOrEmpty(Fecha_Notificacion))
            {
                if (Convert.ToDateTime(Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString()) < Fecha_Inicial)//Es el campo de la tabla que acaba de ser creada
                {
                    Dt_Generadas.Rows[Cont_Borrado].Delete();
                    Fila_Borrada = true;
                }
            }
        }
        if (Txt_Fecha_Final.Text.Length > 0 && Fila_Borrada != true)
        {
            DateTime Fecha_Final;
            Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text);
            Fecha_Final = Fecha_Final.AddHours(23).AddMinutes(59).AddSeconds(59);
            String Fecha_Notificacion = Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString();//Es el campo de la tabla que acaba de ser creada
            if (!String.IsNullOrEmpty(Fecha_Notificacion))
            {
                if (Convert.ToDateTime(Dt_Generadas.Rows[Cont_Borrado]["FECHA_NOTIFICACION"].ToString()) > Fecha_Final)//Es el campo de la tabla que acaba de ser creada
                {
                    Dt_Generadas.Rows[Cont_Borrado].Delete();
                    Fila_Borrada = true;
                }
            }
        }
        if (Fila_Borrada != true)
            Cont_Borrado++;

        return Cont_Borrado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Buscar_Embargos
    ///DESCRIPCIÓN: Metodo usado para buscar las determinaciones por filtros
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 10/03/2012 12:55:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Buscar_Embargos()
    {
        Cls_Ope_Pre_Pae_Etapas_Negocio Etapas_PAE = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Cuentas_Predial;
        DataTable Dt_Embargos_Generados = Crear_Tabla_Embargos_Generados();//Se crea la tabla para pasarla algrid
        DataTable Dt_Busqueda;

        Boolean Comprobacion_Termida = false;//Si es verdadero termina la comprobacion de los filtos
        Int32 Cont_Borrado = 0;//Posicion de la Tabla Dt_Determinaciones_Generadas
        Limpia_Mensaje_Error();//Limpia el mensaje error
        Etapas_PAE.P_Proceso_Actual = "EMBARGO";
        try
        {
            if (Txt_Folio_Inicial.Text.Length > 0 || Txt_Folio_Final.Text.Length > 0)
            {
                if (Cmb_Asignado_a.SelectedIndex < 1 && Comprobacion_Termida != true)
                {
                    Mensaje_Error("Selecciona un despacho");
                    Comprobacion_Termida = true;
                }
            }
            if (Comprobacion_Termida != true)
            {
                if (Session["CUENTAS_PREDIAL_CONTRIBUYENTE"] != null && Session["CUENTAS_PREDIAL_CONTRIBUYENTE"].ToString() != "")
                {
                    Dt_Cuentas_Predial = (DataTable)Session["CUENTAS_PREDIAL_CONTRIBUYENTE"];
                    for (int Cont_Contribuyente = 0; Cont_Contribuyente < Dt_Cuentas_Predial.Rows.Count; Cont_Contribuyente++)
                    {
                        Etapas_PAE.P_Cuenta_Predial_Id = Dt_Cuentas_Predial.Rows[Cont_Contribuyente][Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID].ToString();//Asigna el ID de la Cuenta Predial
                        if (Txt_Numero_Cuenta.Text.Length > 0)
                        {
                            Etapas_PAE.P_Cuenta_Predial = Txt_Numero_Cuenta.Text;//Asigna el ID de la Cuenta Predial                
                        }

                        if (Cmb_Asignado_a.SelectedIndex > 0)//Si la busqueda es por cuentas asignadas al despacho determinado
                        {
                            Etapas_PAE.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                        }

                        if (Cmb_Estatus.SelectedIndex > 0)
                        {
                            Etapas_PAE.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                        }
                        if (Txt_Folio_Inicial.Text.Length > 0)
                        {
                            Etapas_PAE.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                        }
                        if (Txt_Folio_Final.Text.Length > 0)
                        {
                            Etapas_PAE.P_Folio_Final = Txt_Folio_Final.Text;
                        }
                        Dt_Busqueda = Etapas_PAE.Consultar_Pae_Det_Etapas();
                        for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
                        {
                            Llenar_DataRow_Embargos(Dt_Embargos_Generados, Dt_Busqueda, Contador);
                        }
                    }
                }
                else
                {

                    if (Txt_Numero_Cuenta.Text.Length > 0)
                    {
                        Etapas_PAE.P_Cuenta_Predial = Txt_Numero_Cuenta.Text;//Asigna el ID de la Cuenta Predial                
                    }

                    if (Cmb_Asignado_a.SelectedIndex > 0)//Si la busqueda es por cuentas asignadas al despacho determinado
                    {
                        Etapas_PAE.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                    }

                    if (Cmb_Estatus.SelectedIndex > 0)
                    {
                        Etapas_PAE.P_Estatus = Cmb_Estatus.SelectedItem.Text;
                    }
                    if (Txt_Folio_Inicial.Text.Length > 0)
                    {
                        Etapas_PAE.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                    }
                    if (Txt_Folio_Final.Text.Length > 0)
                    {
                        Etapas_PAE.P_Folio_Final = Txt_Folio_Final.Text;
                    }
                    Dt_Busqueda = Etapas_PAE.Consultar_Pae_Det_Etapas();
                    for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
                    {
                        Llenar_DataRow_Embargos(Dt_Embargos_Generados, Dt_Busqueda, Contador);
                    }
                }
                //Comprobar fechas
                for (int Cont_Fechas = 0; Cont_Fechas < Dt_Embargos_Generados.Rows.Count; Cont_Fechas++)
                {
                    Comprobar_Filtros_Busqueda(Dt_Embargos_Generados, Cont_Borrado, Cont_Fechas);
                }

                Grid_Embargos_Generados.DataSource = Dt_Embargos_Generados;
                Grid_Embargos_Generados.DataBind();
                Session["Grid_Embargos_Generados"] = Dt_Embargos_Generados;
                Session.Remove("CUENTAS_PREDIAL_CONTRIBUYENTE");

                if (Dt_Embargos_Generados.Rows.Count < 1)
                {
                    Mensaje_Error("No se encontraron Embargos con esos parametros");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region Eventos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Busca_Contribuyente_Click
    ///DESCRIPCIÓN: Llama una venta modal para buscar el contribuyente
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 02/03/2012 10:05:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busca_Contribuyente_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Pae_Etapas_Negocio Pae_Etapas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Cuentas_Predial = new DataTable();
        try
        {
            if (Session["BUSQUEDA_CONTRIBUYENTE"] != null)
            {
                if (Convert.ToBoolean(Session["BUSQUEDA_CONTRIBUYENTE"]) == true)
                {
                    Pae_Etapas.P_Contribuyente_Id = HttpUtility.HtmlDecode(Session["CONTRIBUYENTE_ID"].ToString().Replace("&nbsp;", ""));
                    Dt_Cuentas_Predial = Pae_Etapas.Consultar_Contribuyente_Etapas_Pae();
                    Txt_Contribuyente.Text = HttpUtility.HtmlDecode(Session["CONTRIBUYENTE_NOMBRE"].ToString().Replace("&nbsp;", ""));
                    Session["CUENTAS_PREDIAL_CONTRIBUYENTE"] = Dt_Cuentas_Predial;
                    Session.Remove("CONTRIBUYENTE_ID");
                    Session.Remove("CONTRIBUYENTE_NOMBRE");
                }
            }
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Busca_Deter_Click
    ///DESCRIPCIÓN          : Llena el Grid de determinaciones Generadas, con una consulta
    ///                       que tiene varios filtros
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/03/2012 01:46:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busca_Embargos_Click(object sender, ImageClickEventArgs e)
    {
        Buscar_Embargos();
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
    ///NOMBRE DE LA FUNCIÓN : Btn_Eliminar_Movimiento_Click
    ///DESCRIPCIÓN          : Actualiza los Gastos de ejecucion
    ///                       
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 15/03/2012 10:23:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Eliminar_Movimiento_Click(object sender, ImageClickEventArgs e)
    {
        //DataTable Dt_Determinaciones_Generadas;
        //LinkButton Btn_Enlance = sender as LinkButton;
        //String Cuenta_Predial = Btn_Enlance.Text;
        //Session["Cuenta"] = Cuenta_Predial;

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
            DataTable Dt_Embargos_Generados = Crear_Tabla_Embargos_Generados();//Se crea la tabla para pasarla algrid
            DataTable Dt_Busqueda = null;
            Limpia_Mensaje_Error();
            Rs_Etapas.P_Folio = Txt_Busqueda.Text;
            Rs_Etapas.P_Proceso_Actual = "EMBARGO";
            Dt_Busqueda = Rs_Etapas.Consultar_Pae_Det_Etapas();
            for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
            {
                Llenar_DataRow_Embargos(Dt_Embargos_Generados, Dt_Busqueda, Contador);
            }
            Grid_Embargos_Generados.DataSource = Dt_Embargos_Generados;
            Grid_Embargos_Generados.DataBind();
            Session["Grid_Embargos_Generados"] = Dt_Embargos_Generados;
            Txt_Busqueda.Text = "";
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #region Texbox
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Inicial_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Inicial_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Inicial.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Inicial.Text, out Fecha_valida))
            {
                Txt_Fecha_Inicial.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Inicial.Text = "";
            }
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Final_TextChanged
    ///DESCRIPCIÓN          : Valida la fecha para ver si esta en el formato correcto
    ///PARAMETROS           : sender y e
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 23/Abril/2012  
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Txt_Fecha_Final_TextChanged(object sender, EventArgs e)
    {
        DateTime Fecha_valida;
        if (Txt_Fecha_Final.Text != "")
        {
            if (DateTime.TryParse(Txt_Fecha_Final.Text, out Fecha_valida))
            {
                Txt_Fecha_Final.Text = Fecha_valida.ToString("dd/MMM/yyyy");
            }
            else
            {
                Txt_Fecha_Final.Text = "";
            }
        }
    }
    #endregion
    #endregion

    #region Grids
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Grid_Embargos_Generados_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de seguimiento a determinadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/03/2012 05:44:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Embargos_Generados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Embargos_Generados.PageIndex = e.NewPageIndex;
            Grid_Embargos_Generados.DataSource = Session["Grid_Embargos_Generados"];
            Grid_Embargos_Generados.DataBind();
        }
        catch (Exception Ex)
        {
            Mensaje_Error(Ex.Message);
        }
    }
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
    protected void Grid_Embargos_Generados_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Pae_Honorarios_Negocio Rs_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        Cls_Ope_Pre_Pae_Notificaciones_Negocio Rs_Notificaciones = new Cls_Ope_Pre_Pae_Notificaciones_Negocio();
        Cls_Ope_Pre_Pae_Publicaciones_Negocio Rs_Publicaciones = new Cls_Ope_Pre_Pae_Publicaciones_Negocio();
        Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Pae_Det_Etpas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        DataTable Dt_Honorarios = (DataTable)Session["Honorarios"];
        DataTable Dt_Publicaciones = (DataTable)Session["Publicaciones"];
        DataTable Dt_Notificaciones = (DataTable)Session["Notificaciones"];
        DataTable Dt_Embargos_Generados = (DataTable)Session["Grid_Embargos_Generados"];
        String No_Detalle_Etapa;
        String Estatus = "";

        if (Dt_Honorarios != null || Dt_Publicaciones != null || Dt_Notificaciones != null)
        {
            try
            {
                No_Detalle_Etapa = Dt_Embargos_Generados.Rows[Grid_Embargos_Generados.SelectedIndex + (Grid_Embargos_Generados.PageSize * Grid_Embargos_Generados.PageIndex)]["NO_DETALLE_ETAPA"].ToString();
                if (Dt_Notificaciones != null)
                {
                    Rs_Notificaciones.P_No_Detalle_Etapa = No_Detalle_Etapa;
                    Rs_Notificaciones.P_Fecha_Hora = Dt_Notificaciones.Rows[0]["FECHA_HORA"].ToString();
                    Rs_Notificaciones.P_Estatus = Dt_Notificaciones.Rows[0]["ESTATUS"].ToString();
                    Rs_Notificaciones.P_Notificador = Dt_Notificaciones.Rows[0]["NOTIFICADOR"].ToString();
                    Rs_Notificaciones.P_Recibio = Dt_Notificaciones.Rows[0]["RECIBIO"].ToString();
                    Rs_Notificaciones.P_Acuse_Recibo = Dt_Notificaciones.Rows[0]["ACUSE_RECIBIO"].ToString();
                    Rs_Notificaciones.P_Folio = Dt_Notificaciones.Rows[0]["FOLIO"].ToString();
                    Rs_Notificaciones.P_Medio_Notificacion = "";
                    Rs_Notificaciones.P_Proceso = "EMBARGO";
                    Estatus = Dt_Notificaciones.Rows[0]["ESTATUS"].ToString();
                    Rs_Notificaciones.Alta_Notificaciones();
                }
                if (Estatus == "")
                {
                    Estatus = Dt_Embargos_Generados.Rows[Grid_Embargos_Generados.SelectedIndex + (Grid_Embargos_Generados.PageSize * Grid_Embargos_Generados.PageIndex)]["ESTATUS"].ToString();
                }
                if (Dt_Honorarios != null)
                {
                    for (int Conta_Ejecucion = 0; Conta_Ejecucion < Dt_Honorarios.Rows.Count; Conta_Ejecucion++)
                    {
                        Rs_Honorarios.P_No_Detalle_Etapa = No_Detalle_Etapa;
                        Rs_Honorarios.P_Gasto_Ejecucion_Id = Dt_Honorarios.Rows[Conta_Ejecucion]["GASTO_EJECUCION_ID"].ToString();
                        Rs_Honorarios.P_Proceso = "EMBARGO";
                        Rs_Honorarios.P_Importe = Dt_Honorarios.Rows[Conta_Ejecucion]["IMPORTE"].ToString();
                        Rs_Honorarios.P_Estatus = Estatus;
                        Rs_Honorarios.Alta_Honorario();
                    }
                }
                if (Dt_Publicaciones != null)
                {
                    for (int Conta_Publicacion = 0; Conta_Publicacion < Dt_Publicaciones.Rows.Count; Conta_Publicacion++)
                    {
                        Rs_Publicaciones.P_No_Detalle_Etapa = No_Detalle_Etapa;
                        Rs_Publicaciones.P_Fecha_Publicacion = Dt_Publicaciones.Rows[Conta_Publicacion]["FECHA_PUBLICACION"].ToString();                        
                        Rs_Publicaciones.P_Medio_Publicacion = Dt_Publicaciones.Rows[Conta_Publicacion]["MEDIO_PUBLICACION"].ToString();
                        Rs_Publicaciones.P_Pagina = Dt_Publicaciones.Rows[Conta_Publicacion]["PAGINA"].ToString();
                        Rs_Publicaciones.P_Tomo = Dt_Publicaciones.Rows[Conta_Publicacion]["TOMO"].ToString();
                        Rs_Publicaciones.P_Parte = Dt_Publicaciones.Rows[Conta_Publicacion]["PARTE"].ToString();
                        Rs_Publicaciones.P_Foja = Dt_Publicaciones.Rows[Conta_Publicacion]["FOJA"].ToString();
                        Rs_Publicaciones.P_Proceso = "EMBARGO";
                        Rs_Publicaciones.P_Estatus = Estatus;
                        Rs_Publicaciones.Alta_Publicaciones();
                    }
                }

                Session.Remove("Honorarios");
                Session.Remove("Publicaciones");
                Session.Remove("Notificaciones");
                Session.Remove("Generadas");
                //Actualiza la tabla PAE_DET_ETAPAS el campo de Honorarios
                Rs_Pae_Det_Etpas.P_No_Detalle_Etapa = No_Detalle_Etapa;
                Rs_Pae_Det_Etpas.P_Estatus = Estatus;
                Rs_Pae_Det_Etpas.Actualiza_Pae_Det_Etapas();

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento a Embargos", "alert('Alta Exitosa');", true);
                int Pagina = Grid_Embargos_Generados.PageIndex;
                Buscar_Embargos();
                Session["Generadas"] = Dt_Embargos_Generados;
                Grid_Embargos_Generados.PageIndex = Pagina;
            }
            catch (Exception Ex)
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }
    #endregion
}
 