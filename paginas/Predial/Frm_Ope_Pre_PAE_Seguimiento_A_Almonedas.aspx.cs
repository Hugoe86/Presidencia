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
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Catalogo_Gastos_Ejecucion.Negocio;
using Presidencia.Predial_Pae_Honorarios.Negocio;
using Presidencia.Predial_Pae_Publicaciones.Negocio;
using Presidencia.Predial_Pae_Notificaciones.Negocio;
using Presidencia.Predial_Pae_Almonedas.Negocio;
using Presidencia.Predial_Pae_Remates.Negocio;
using Presidencia.Predial_Pae_Postores.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Seguimiento_A_Almonedas : System.Web.UI.Page
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
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_A_Etapa
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las cuentas Generadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 03/02/2012 05:10:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_A_Etapa()
    {
        DataTable Dt_Generadas = new DataTable();
        Dt_Generadas.Columns.Add(new DataColumn("CUENTA_PREDIAL_ID", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("PERIODO_CORRIENTE", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("CORRIENTE", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("PERIODO_REZAGO", typeof(String)));
        Dt_Generadas.Columns.Add(new DataColumn("REZAGO", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("HONORARIOS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("MULTAS", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("ADEUDO", typeof(Decimal)));
        Dt_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        return Dt_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_A_Etapa
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas a generar y Caulcula el Perido, Rezago, etc
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 08/02/2012 04:38:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_A_Etapa(DataTable Dt_Busqueda, DataTable Dt_Generadas)
    {
        DataRow Dr_Generadas;
        Dr_Generadas = Dt_Generadas.NewRow();
        Dr_Generadas["CUENTA_PREDIAL_ID"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id].ToString();
        Dr_Generadas["PERIODO_CORRIENTE"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Corriente].ToString();
        Dr_Generadas["CORRIENTE"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente].ToString();
        Dr_Generadas["PERIODO_REZAGO"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Periodo_Rezago].ToString();
        Dr_Generadas["REZAGO"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago].ToString();
        Dr_Generadas["RECARGOS_ORDINARIOS"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios].ToString();
        Dr_Generadas["RECARGOS_MORATORIOS"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios].ToString();
        Dr_Generadas["HONORARIOS"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Honorarios].ToString();
        Dr_Generadas["MULTAS"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Multas].ToString();
        Dr_Generadas["ADEUDO"] = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Total].ToString();
        Dr_Generadas["ESTATUS"] = "PENDIENTE";
        Dt_Generadas.Rows.Add(Dr_Generadas);//Se asigna la nueva fila a la tabla
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
        Grid_Almonedas_Generadas.DataSource = null;
        //Grid_Determinaciones_Generadas.DataBind();
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
    ///NOMBRE DE LA FUNCIÓN : Comprobar_Filtros_Busqueda
    ///DESCRIPCIÓN          : Comprueba que la nueva fila que se agrego en la tabla de determinaciones generadas
    ///                       cumpla con los demas filtros seleccionados, si no cumple es borrada,
    ///                       regresa el numero de posicion en la tabla Dt_Determinaciones_Generadas
    ///PARAMETROS:          : 1.-Dt_Embargos_Generados: Se guardan los registros que cumplen con los filtos
    ///                     : 2.-Dt_Pre_Pae_Det_Etapas: Obtien los registros que estan dados de alta en las determinaciones
    ///                     : 3.-Cont_Borrado: Posicion actual de la tabla Dt_Determinaciones_Generadas
    ///                     : 4.-Cont_Det_Etapas: Posicion actual de la tabla Dt_Pre_Pae_Det_Etapas
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
    ///NOMBRE DE LA FUNCIÓN: Buscar_Almonedas
    ///DESCRIPCIÓN: Metodo usado para buscar las determinaciones por filtros
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 10/03/2012 12:55:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Buscar_Almonedas()
    {
        //Cls_Cat_Pre_Cuentas_Predial_Negocio Consulta_Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        Cls_Ope_Pre_Pae_Almoneda_Negocios Almonedas = new Cls_Ope_Pre_Pae_Almoneda_Negocios();
        DataTable Dt_Almonedas_Generadas = Crear_Tabla_Almonedas_Generadas();//Se crea la tabla para pasarla algrid
        DataTable Dt_Busqueda;

        Boolean Comprobacion_Termida = false;//Si es verdadero termina la comprobacion de los filtos
        Int32 Cont_Borrado = 0;//Posicion de la Tabla Dt_Determinaciones_Generadas
        Mensaje_Error();//Limpia el mensaje error
        Almonedas.P_Proceso_Actual = "ALMONEDA";
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
                if (Txt_Numero_Cuenta.Text.Length > 0)
                {
                    Almonedas.P_Cuenta_Predial = Txt_Numero_Cuenta.Text;//Asigna el ID de la Cuenta Predial                
                }

                if (Cmb_Asignado_a.SelectedIndex > 0 )//Si la busqueda es por cuentas asignadas al despacho determinado
                {
                    Almonedas.P_Despacho_Id = Cmb_Asignado_a.SelectedValue;
                }

                if (Cmb_Estatus.SelectedIndex > 0 )
                {
                    Almonedas.P_Estatus_Etapa = Cmb_Estatus.SelectedItem.Text;
                }
                if (Txt_Folio_Inicial.Text.Length > 0)
                {
                    Almonedas.P_Folio_Inicial = Txt_Folio_Inicial.Text;
                }
                if (Txt_Folio_Final.Text.Length > 0)
                {
                    Almonedas.P_Folio_Final = Txt_Folio_Final.Text;
                }
                Dt_Busqueda = Almonedas.Consulta_Det_Etapas_Almonedas_Remocion();
                for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
                {
                    Llenar_DataRow_Almonedas(Dt_Almonedas_Generadas, Dt_Busqueda, Contador);
                }
                //Comprobar fechas
                for (int Cont_Fechas = 0; Cont_Fechas < Dt_Busqueda.Rows.Count; Cont_Fechas++)
                {
                    Comprobar_Filtros_Busqueda(Dt_Busqueda, Cont_Borrado, Cont_Fechas);
                }

                Grid_Almonedas_Generadas.DataSource = Dt_Almonedas_Generadas;
                Grid_Almonedas_Generadas.DataBind();
                Session["Grid_Almonedas_Generadas"] = Dt_Almonedas_Generadas;
                if (Dt_Almonedas_Generadas.Rows.Count < 1)
                {
                    Mensaje_Error("No se encontraron Almonedas con esos parametros");
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
            Cls_Ope_Pre_Pae_Almoneda_Negocios Almonedas = new Cls_Ope_Pre_Pae_Almoneda_Negocios();
            DataTable Dt_Almonedas_Generadas = Crear_Tabla_Almonedas_Generadas();//Se crea la tabla para pasarla algrid
            DataTable Dt_Busqueda = null;
            Mensaje_Error();
            Almonedas.P_Folio = Txt_Busqueda.Text;
            Almonedas.P_Proceso_Actual = "ALMONEDA";
            Dt_Busqueda = Almonedas.Consulta_Det_Etapas_Almonedas_Remocion();
            for (int Contador = 0; Contador < Dt_Busqueda.Rows.Count; Contador++)
            {
                Llenar_DataRow_Almonedas(Dt_Almonedas_Generadas, Dt_Busqueda, Contador);
            }
            Grid_Almonedas_Generadas.DataSource = Dt_Almonedas_Generadas;
            Grid_Almonedas_Generadas.DataBind();
            Session["Grid_Almonedas_Generadas"] = Dt_Almonedas_Generadas;
            Txt_Busqueda.Text = "";
        }
        catch (Exception ex)
        {
            Mensaje_Error(ex.Message.ToString());
        }
    }
    #region Tablas
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Almonedas_Generadas
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las Almonedas generadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 09/02/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Almonedas_Generadas()
    {
        DataTable Dt_Almonedas_Generadas = new DataTable();
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("CUENTA", typeof(String)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("ADEUDO", typeof(Decimal)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("FOLIO", typeof(String)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("ASIGNADO", typeof(String)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("ENTREGA", typeof(String)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("VALOR_AVALUO", typeof(Decimal)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("POSTURA_LEGAL", typeof(Decimal)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("FECHA", typeof(String)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("NO_DETALLE_ETAPA", typeof(String)));
        Dt_Almonedas_Generadas.Columns.Add(new DataColumn("CUENTA_PREDIAL_ID", typeof(String)));
        return Dt_Almonedas_Generadas;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Almonedas
    ///DESCRIPCIÓN          : Agrega una nueva fila a las cuentas omitidas y Calcula el Perido, Rezago, etc
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 11:49:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Almonedas(DataTable Dt_Almonedas_Generadas, DataTable Dt_Busqueda, Int32 Contador)
    {
        DataRow Dr_Determinadas;
        String Fecha_Notificacion = Dt_Busqueda.Rows[Contador]["FECHA_NOTIFICACION"].ToString();
        if (Fecha_Notificacion == null || Fecha_Notificacion == "")
        {
            Fecha_Notificacion = "";
        }
        Dr_Determinadas = Dt_Almonedas_Generadas.NewRow();
        Dr_Determinadas["CUENTA"] = Dt_Busqueda.Rows[Contador]["CUENTA_PREDIAL"].ToString();
        Dr_Determinadas["ADEUDO"] = Dt_Busqueda.Rows[Contador]["TOTAL"].ToString();
        Dr_Determinadas["FOLIO"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Folio].ToString();
        Dr_Determinadas["ASIGNADO"] = Dt_Busqueda.Rows[Contador][Cat_Pre_Despachos_Externos.Campo_Despacho].ToString(); ;
        Dr_Determinadas["ENTREGA"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Etapas.Campo_Numero_Entrega].ToString(); ;
        Dr_Determinadas["VALOR_AVALUO"] = Dt_Busqueda.Rows[Contador]["AVALUO_PERITAJE"].ToString();

        if (Convert.ToInt16(Dt_Busqueda.Rows[Contador]["NUMERO_ALMONEDA"].ToString()) > 1)
        {
            Dr_Determinadas["POSTURA_LEGAL"] = (Convert.ToDouble(Dt_Busqueda.Rows[Contador]["AVALUO_PERITAJE"].ToString()) - (Convert.ToDouble(Dt_Busqueda.Rows[Contador]["VALOR_AVALUO"].ToString()) * 0.2));
        }
        else
        {
            Dr_Determinadas["POSTURA_LEGAL"] = Dt_Busqueda.Rows[Contador]["VALOR_AVALUO"].ToString();
        }

        Dr_Determinadas["FECHA"] = Fecha_Notificacion;
        Dr_Determinadas["ESTATUS"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Estatus].ToString();
        Dr_Determinadas["NO_DETALLE_ETAPA"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa].ToString();
        Dr_Determinadas["CUENTA_PREDIAL_ID"] = Dt_Busqueda.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Cuenta_Predial_Id].ToString();

        Dt_Almonedas_Generadas.Rows.Add(Dr_Determinadas);//Se asigna la nueva fila a la tabla
    }
    #endregion
    #endregion

    #region Eventos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Btn_Busca_Almonedas_Click
    ///DESCRIPCIÓN          : Llena el Grid de determinaciones Generadas, con una consulta
    ///                       que tiene varios filtros
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 10:32:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busca_Almonedas_Click(object sender, ImageClickEventArgs e)
    {
        Buscar_Almonedas();
    }
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
    ///NOMBRE DE LA FUNCIÓN : Grid_Determinaciones_Generadas_SelectedIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView de seguimiento a determinadas
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 01/03/2012 05:44:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Grid_Determinaciones_Generadas_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Almonedas_Generadas.PageIndex = e.NewPageIndex;
            Grid_Almonedas_Generadas.DataSource = Session["Grid_Almonedas_Generadas"];
            Grid_Almonedas_Generadas.DataBind();
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
    protected void Grid_Almonedas_Generadas_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Pae_Honorarios_Negocio Rs_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        Cls_Ope_Pre_Pae_Notificaciones_Negocio Rs_Notificaciones = new Cls_Ope_Pre_Pae_Notificaciones_Negocio();
        Cls_Ope_Pre_Pae_Publicaciones_Negocio Rs_Publicaciones = new Cls_Ope_Pre_Pae_Publicaciones_Negocio();
        Cls_Ope_Pre_Pae_Etapas_Negocio Rs_Pae_Det_Etpas = new Cls_Ope_Pre_Pae_Etapas_Negocio();
        Cls_Ope_Pre_Pae_Remates_Negocio Rs_Remates = new Cls_Ope_Pre_Pae_Remates_Negocio();
        
        DataTable Dt_Honorarios = (DataTable)Session["Honorarios"];
        DataTable Dt_Publicaciones = (DataTable)Session["Publicaciones"];
        DataTable Dt_Notificaciones = (DataTable)Session["Notificaciones"];
        DataTable Dt_Remates = (DataTable)Session["Remates"];
        DataTable Dt_Postore = (DataTable)Session["Postor_Nuevo"];
        
        String Abonos = (Session["Abonos"] != null) ? Session["Abonos"].ToString() : "NO";
        String Nueva_Etapa = (Session["Etapa_Pae"] != null) ? Session["Etapa_Pae"].ToString() : "NO";
        String Despacho_ID = (Session["Despacho_ID"] != null) ? Session["Despacho_ID"].ToString() : "";
        String No_Entrega = (Session["Entrega"] != null) ? Session["Entrega"].ToString() : "";


        DataTable Dt_Almonedas_Generadas = (DataTable)Session["Grid_Almonedas_Generadas"];
        String No_Detalle_Etapa;
        String Estatus = "";

        if (Dt_Honorarios != null || Dt_Publicaciones != null || Dt_Notificaciones != null || Dt_Remates != null || Abonos == "SI"||Nueva_Etapa!="NO")
        {
            try
            {
                No_Detalle_Etapa = Dt_Almonedas_Generadas.Rows[Grid_Almonedas_Generadas.SelectedIndex + (Grid_Almonedas_Generadas.PageSize * Grid_Almonedas_Generadas.PageIndex)]["NO_DETALLE_ETAPA"].ToString();
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
                    Rs_Notificaciones.P_Proceso = "ALMONEDA";
                    Estatus = "NOTIFICACION";
                    Rs_Notificaciones.Alta_Notificaciones();
                }
                if (Dt_Honorarios != null)
                {
                    for (int Conta_Ejecucion = 0; Conta_Ejecucion < Dt_Honorarios.Rows.Count; Conta_Ejecucion++)
                    {
                        Rs_Honorarios.P_No_Detalle_Etapa = No_Detalle_Etapa;
                        Rs_Honorarios.P_Gasto_Ejecucion_Id = Dt_Honorarios.Rows[Conta_Ejecucion]["GASTO_EJECUCION_ID"].ToString();
                        Rs_Honorarios.P_Proceso = "ALMONEDA";
                        Rs_Honorarios.P_Importe = Dt_Honorarios.Rows[Conta_Ejecucion]["IMPORTE"].ToString();
                        Rs_Honorarios.P_Estatus = "NOTIFICACION";
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
                        Rs_Publicaciones.P_Proceso = "ALMONEDA";
                        Rs_Publicaciones.P_Estatus = "PUBLICACION";
                        Rs_Publicaciones.Alta_Publicaciones();
                    }
                }
                if (Dt_Remates != null)
                {
                    Rs_Remates.P_No_Detalle_Etapa = No_Detalle_Etapa;
                    Rs_Remates.P_Lugar_Remate = Dt_Remates.Rows[0]["LUGAR_REMATE"].ToString();
                    Rs_Remates.P_Fecha_Hora_Remate = Dt_Remates.Rows[0]["FECHA_HORA"].ToString();
                    Rs_Remates.P_Inicio_Publicacion = Dt_Remates.Rows[0]["INI_FECHA"].ToString();
                    Rs_Remates.P_Fin_Publicacion = Dt_Remates.Rows[0]["FIN_FECHA"].ToString();
                    Rs_Remates.Alta_Pae_Remates();
                }
                
                Rs_Pae_Det_Etpas.P_No_Detalle_Etapa = No_Detalle_Etapa;
                if (Nueva_Etapa!="NO" && Despacho_ID!="" && No_Entrega!="")
                {
                    DataTable Dt_Busqueda;
                    DataTable Dt_Generadas = Crear_Tabla_A_Etapa();

                    Rs_Pae_Det_Etpas.P_Proceso_Actual = "ALMONEDA";
                    Rs_Pae_Det_Etpas.P_No_Detalle_Etapa = No_Detalle_Etapa;
                    Rs_Pae_Det_Etpas.P_Campos_Dinamicos = Ope_Pre_Pae_Det_Etapas.Tabla_Ope_Pre_Pae_Det_Etapas + ".* ";

                    Dt_Busqueda = Rs_Pae_Det_Etpas.Consultar_Pae_Det_Etapas();
                    Llenar_DataRow_A_Etapa(Dt_Busqueda, Dt_Generadas);

                    Rs_Pae_Det_Etpas.P_Despacho_Id = Despacho_ID;
                    Rs_Pae_Det_Etpas.P_Numero_Entrega = No_Entrega;
                    Rs_Pae_Det_Etpas.P_Total_Etapa = Dt_Busqueda.Rows[0][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Total].ToString();
                    Rs_Pae_Det_Etpas.P_Modo_Generacion = "Normal";//Cambiar
                    Rs_Pae_Det_Etpas.P_Nombre_Archivo = "";
                    Rs_Pae_Det_Etpas.P_Comentario = "";
                    Rs_Pae_Det_Etpas.P_Proceso_Actual = Nueva_Etapa;
                    Rs_Pae_Det_Etpas.P_Dt_Generadas = Dt_Generadas;
                    Rs_Pae_Det_Etpas.Alta_Pae_Etapas();
                    Rs_Pae_Det_Etpas.P_Proceso_Actual = "";
                }
               
                Rs_Pae_Det_Etpas.P_Estatus = Estatus;
                Rs_Pae_Det_Etpas.Actualiza_Pae_Det_Etapas();

                
                Session.Remove("Honorarios");
                Session.Remove("Publicaciones");
                Session.Remove("Notificaciones");
                Session.Remove("Generadas");
                Session.Remove("Gastos");
                Session.Remove("AGREGA_GASTOS");
                Session.Remove("Remates");
                Session.Remove("Abonos");
                Session.Remove("NO_DETALLE_ETAPA");
                Session.Remove("Adeudo");
                Session.Remove("CUENTA");
                Session.Remove("Despacho_ID");
                Session.Remove("Entrega");
                Session.Remove("Etapa_Pae");

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento a Almonedas", "alert('Alta Exitosa');", true);
                int Pagina = Grid_Almonedas_Generadas.PageIndex;
                Buscar_Almonedas();
                Session["Grid_Almonedas_Generadas"] = Dt_Almonedas_Generadas;
                Grid_Almonedas_Generadas.PageIndex = Pagina;
            }
            catch (Exception Ex)
            {
                Mensaje_Error(Ex.Message);
            }
        }
        if (Dt_Postore != null && Dt_Postore.Rows.Count > 0)
        {
            try
            {
                Cls_Ope_Pre_Pae_Postores_Negocio Postor = new Cls_Ope_Pre_Pae_Postores_Negocio();
                Postor.P_No_Detalle_Etapa = Dt_Almonedas_Generadas.Rows[Grid_Almonedas_Generadas.SelectedIndex + (Grid_Almonedas_Generadas.PageSize * Grid_Almonedas_Generadas.PageIndex)]["NO_DETALLE_ETAPA"].ToString();
                Postor.P_Nombre_Postor = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Nombre_Postor].ToString();
                Postor.P_Deposito = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Deposito].ToString();
                Postor.P_Porcentaje = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Porcentaje].ToString();
                Postor.P_Domicilio = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Domicilio].ToString();
                Postor.P_Telefono = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Telefono].ToString();
                Postor.P_Rfc = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Rfc].ToString();
                Postor.P_No_Ife = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_No_Ife].ToString();
                Postor.P_Sexo = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Sexo].ToString();
                Postor.P_Estado_Civil = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Estado_Civil].ToString();
                Postor.P_Estatus = Dt_Postore.Rows[0][Ope_Pre_Pae_Postores.Campo_Estatus].ToString();
                Postor.Alta_Pae_Postores();
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Seguimiento a Almonedas", "alert('Alta Exitosa');", true);
            }
            catch (Exception Ex)
            {
                Mensaje_Error(Ex.Message);
            }
        }
    }
    #endregion
}
