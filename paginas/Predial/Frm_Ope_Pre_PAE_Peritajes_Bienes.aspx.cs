using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Presidencia.Constantes;
using Presidencia.Catalogo_Despachos_Externos.Negocio;
using Presidencia.Predial_Pae_Bienes.Negocio;
using Presidencia.Catalogo_Tipos_Bienes.Negocio;
using Presidencia.Operacion_Predial_Pagos_Instit_Externas.Negocio;
using Presidencia.Sessiones;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.IO;

public partial class paginas_Predial_Frm_Ope_Pre_PAE_Peritaje_Bienes : System.Web.UI.Page
{
    #region PageLoad
    protected void Page_Load(object sender, EventArgs e)
    {
        String Ventana_Modal = "";
        this.Form.Enctype = "multipart/form-data";
        Tool_ScriptManager.RegisterPostBackControl(Btn_Subir_Archivo);
        try
        {
            if (!Page.IsPostBack)
            {
                Session["Activa"] = true;//Variable para mantener la session activa.
                Inicializa_Controles();//Inicializa los controles de la pantalla para que el usuario pueda realizar las siguientes operaciones           
            }
            Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:yes;status:no;dialogWidth:580px;dialogHeight:450px;dialogHide:true;help:no;scroll:no');";
            Btn_Busca_Contribuyente.Attributes.Add("onClick", Ventana_Modal);
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }
    #endregion

    #region METODOS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Limpiar_Detalles_Peritaje
    /// DESCRIPCIÓN: Limpia los controles con los detalles del un peritaje
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Limpiar_Detalles_Peritaje()
    {
        Cmb_Avaluo.Items.Clear();
        Txt_Fecha_Peritaje.Text = "";
        Txt_Perito.Text = "";
        Txt_Valor_Peritaje.Text = "";
        Txt_Observaciones.Text = "";

        Cmb_Tipo_de_bien.SelectedIndex = 0;
        Txt_Valor_Bien.Text = "";
        Txt_Descripcion.Text = "";
        Txt_Fotografias.Text = "";

        RBtn_Lugar.SelectedIndex = -1;
        Txt_Lugar.Text = "";
        Txt_Costo.Text = "";
        Txt_Dimensiones.Text = "";
        Txt_Fecha_Ingreso.Text = "";
        Txt_Tiempo_transcurrido.Text = "";
        Txt_Costo_Almacenamiento.Text = "";

        Grid_Bienes.DataSource = null;
        Grid_Bienes.DataBind();
        Grid_Depositario.DataSource = null;
        Grid_Depositario.DataBind();
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
        Session.Remove("Bienes");
        Session.Remove("Lista_Bienes_Eliminar");
        Grid_Embargos_Generados.DataSource = null;
        //Grid_Determinaciones_Generadas.DataBind();
    }

    ///****************************************************************************************
    ///NOMBRE DE LA FUNCION:Mostrar_Mensaje_Error
    ///DESCRIPCION : Muestra el error
    ///PARAMETROS  : P_Texto: texto de un TextBox
    ///CREO        : Toledo Rodriguez Jesus S.
    ///FECHA_CREO  : 04-Septiembre-2010
    ///MODIFICO          :
    ///FECHA_MODIFICO    :
    ///CAUSA_MODIFICACION:
    ///****************************************************************************************
    private void Mostrar_Mensaje_Error(String P_Mensaje)
    {
        Img_Error.Visible = true;
        Lbl_Encabezado_Error.Text = "";
        Lbl_Encabezado_Error.Text = P_Mensaje + "</br>";
    }

    private void Limpia_Mensaje_Error()
    {
        Img_Error.Visible = false;
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
    private void Cargar_Combo_Despachos_Externos()
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
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Cmb_Tipo_de_bien
    ///DESCRIPCIÓN: Metodo usado para cargar la informacion del catalogo de bienes
    ///PARAMETROS: 
    ///CREO: Armando Zavala Moreno
    ///FECHA_CREO: 22/03/2012 10:13:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Cmb_Tipo_de_bien()
    {
        DataTable Dt_Tipo_Bien = new DataTable();
        try
        {
            Cls_Cat_Pre_Tipos_Bienes_Negocio Tipos_Bienes = new Cls_Cat_Pre_Tipos_Bienes_Negocio();
            Tipos_Bienes.P_Filtro = "";
            Cmb_Tipo_de_bien.DataTextField = Cat_Pre_Tipos_Bienes.Campo_Nombre;
            Cmb_Tipo_de_bien.DataValueField = Cat_Pre_Tipos_Bienes.Campo_Tipo_Bien_Id;
            Dt_Tipo_Bien = Tipos_Bienes.Consultar_Bien();

            foreach (DataRow Dr_Fila in Dt_Tipo_Bien.Rows)
            {
                if (Dr_Fila[Cat_Pre_Despachos_Externos.Campo_Estatus].ToString() != "VIGENTE")//Busca el estatus
                {
                    Dr_Fila.Delete();//Borra el registro                    
                    break;
                }
            }
            Cmb_Tipo_de_bien.DataSource = Dt_Tipo_Bien;
            Cmb_Tipo_de_bien.DataBind();
            Cmb_Tipo_de_bien.Items.Insert(0, new ListItem("<-- SELECCIONE -->", "0"));
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Inicializa_Controles
    /// DESCRIPCIÓN: Prepara los controles en la forma para que el usuario pueda realizar diferentes operaciones
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Inicializa_Controles()
    {
        Limpia_Mensaje_Error();
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpiar_Formulario(); //Limpia los controles del forma
            Cargar_Combo_Despachos_Externos();
            Cargar_Combo_Cmb_Tipo_de_bien();
            Grid_Embargos_Generados.SelectedIndex = -1;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Habilitar_Controles
    /// DESCRIPCIÓN: Habilita o Deshabilita los controles de la forma para según se requiera 
    ///             para la siguiente operación
    /// PARÁMETROS:
    ///         1. Operacion: Indica la operación que se desea realizar por parte del usuario
    /// 	             (inicial, nuevo, modificar)
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; // Indica si el control de la forma va a ser habilitado para que los edite el usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Inicio";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                    break;

                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    break;
            }

            // deshabilitar controles filtro búsqueda cuando se está modificando o agregando
            Cmb_Avaluo.Enabled = !Habilitado;
            Grid_Embargos_Generados.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar_Bienes.Enabled = !Habilitado;

            Txt_Folio_Inicial.Enabled = !Habilitado;
            Txt_Folio_Final.Enabled = !Habilitado;
            Txt_Numero_Cuenta.Enabled = !Habilitado;
            Txt_Contribuyente.Enabled = !Habilitado;
            Btn_Busca_Contribuyente.Enabled = !Habilitado;
            Txt_Fecha_Inicial.Enabled = !Habilitado;
            Txt_Fecha_Final.Enabled = !Habilitado;
            Btn_Txt_Fecha_Inicial.Enabled = !Habilitado;
            Btn_Txt_Fecha_Final.Enabled = !Habilitado;
            Cmb_Asignado_a.Enabled = !Habilitado;
            Cmb_Estatus.Enabled = !Habilitado;

            // Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Txt_Fecha_Peritaje.Enabled = Habilitado;
            Txt_Perito.Enabled = Habilitado;
            Txt_Valor_Peritaje.Enabled = Habilitado;
            Txt_Observaciones.Enabled = Habilitado;

            RBtn_Lugar.Enabled = Habilitado;
            Txt_Lugar.Enabled = Habilitado;
            Txt_Costo.Enabled = Habilitado;
            Txt_Dimensiones.Enabled = Habilitado;
            Txt_Dimensiones.Enabled = Habilitado;
            Txt_Fecha_Ingreso.Enabled = Habilitado;
            Txt_Tiempo_transcurrido.Enabled = Habilitado;
            Txt_Costo_Almacenamiento.Enabled = Habilitado;

            //visibilidad de controles para ingresar bienes
            Tr_Fila_Tipo_Bien.Visible = Habilitado;
            Tr_Fila_Descripcion_Bien.Visible = Habilitado;
            Tr_Fila_Fotografias_Bien.Visible = Habilitado;

            Div_Detalles.Visible = Habilitado;
            Div_Depositarios.Visible = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
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
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("FECHA", typeof(DateTime)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("CORRIENTE", typeof(Decimal)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("REZAGO", typeof(Decimal)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Decimal)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Decimal)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("HONORARIOS", typeof(Decimal)));
        Dt_Determinaciones_Generadas.Columns.Add(new DataColumn("GASTOS_DE_EJECUCION", typeof(Decimal)));
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
    protected void Llenar_DataRow_Embargos(DataTable Dt_Embargos_Generados, DataTable Dt_Pre_Pae_Det_Etapas, String Cuenta_Predial, int Contador, String Despacho, String Entrega)
    {
        DataRow Dr_Determinadas;
        Dr_Determinadas = Dt_Embargos_Generados.NewRow();
        Dr_Determinadas["CUENTA"] = Cuenta_Predial;
        Dr_Determinadas["ADEUDO"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador]["TOTAL"].ToString();
        Dr_Determinadas["FOLIO"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Folio].ToString();
        Dr_Determinadas["ASIGNADO"] = Despacho;
        Dr_Determinadas["ENTREGA"] = Entrega;
        Dr_Determinadas["FECHA"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador]["PROCESO_CAMBIO"].ToString();
        Dr_Determinadas["ESTATUS"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Estatus].ToString();
        Dr_Determinadas["CORRIENTE"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Corriente].ToString();
        Dr_Determinadas["REZAGO"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Rezago].ToString();
        Dr_Determinadas["RECARGOS_ORDINARIOS"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Ordinarios].ToString();
        Dr_Determinadas["RECARGOS_MORATORIOS"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_Adeudo_Recargos_Moratorios].ToString();
        Dr_Determinadas["HONORARIOS"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador]["SUMA_HONORARIOS"].ToString();
        Dr_Determinadas["GASTOS_DE_EJECUCION"] = 0;
        Dr_Determinadas["NO_DETALLE_ETAPA"] = Dt_Pre_Pae_Det_Etapas.Rows[Contador][Ope_Pre_Pae_Det_Etapas.Campo_No_Detalle_Etapa].ToString();

        Dt_Embargos_Generados.Rows.Add(Dr_Determinadas);//Se asigna la nueva fila a la tabla
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Comprobar_Filtros_Determinacione
    ///DESCRIPCIÓN          : Comprueba que la nueva fila que se agrego en la tabla de determinaciones generadas
    ///                       cumpla con los demas filtros seleccionados, si no cumple es borrada,
    ///                       regresa el numero de posicion en la tabla Dt_Determinaciones_Generadas
    ///PARAMETROS:          : 1.-Dt_Determinaciones_Generadas: Se guardan los registros que cumplen con los filtos
    ///                     : 2.-Dt_Pre_Pae_Det_Etapas: Obtien los registros que estan dados de alta en las determinaciones
    ///                     : 3.-Cont_Borrado: Posicion actual de la tabla Dt_Determinaciones_Generadas
    ///                     : 4.-Cont_Det_Etapas: Posicion actual de la tabla Dt_Pre_Pae_Det_Etapas
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 27/02/2012 06:32:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private int Comprobar_Filtros_Determinacione(DataTable Dt_Determinaciones_Generadas, DataTable Dt_Pre_Pae_Det_Etapas, int Cont_Borrado, int Cont_Det_Etapas)
    {
        Boolean Fila_Borrada = false;

        if (Dt_Determinaciones_Generadas.Rows.Count > 0 && Cmb_Asignado_a.SelectedIndex > 0 && Fila_Borrada != true)
        {
            if (Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["ASIGNADO"].ToString() != Cmb_Asignado_a.SelectedItem.Text)
            {
                Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                Fila_Borrada = true;
            }
        }
        if (Dt_Determinaciones_Generadas.Rows.Count > 0 && Cmb_Estatus.SelectedIndex > 0 && Fila_Borrada != true)
        {
            if (Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["ESTATUS"].ToString() != Cmb_Estatus.Text)
            {
                Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                Fila_Borrada = true;
            }
        }
        if (Txt_Folio_Inicial.Text.Length > 0 && Fila_Borrada != true)
        {
            if (Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FOLIO"].ToString() != null && Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FOLIO"].ToString() != "")
            {
                if (Convert.ToInt32(Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FOLIO"].ToString()) < Convert.ToInt32(Txt_Folio_Inicial.Text))
                {
                    Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                    Fila_Borrada = true;
                }
            }
            else
            {
                Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                Fila_Borrada = true;
            }
        }
        if (Txt_Folio_Final.Text.Length > 0 && Fila_Borrada != true)
        {
            if (Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FOLIO"].ToString() != null && Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FOLIO"].ToString() != "")
            {
                if (Convert.ToInt32(Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FOLIO"].ToString()) > Convert.ToInt32(Txt_Folio_Final.Text))
                {
                    Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                    Fila_Borrada = true;
                }
            }
            else
            {
                Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                Fila_Borrada = true;
            }
        }
        if (Txt_Fecha_Inicial.Text.Length > 0 && Fila_Borrada != true)
        {
            DateTime Fecha_Inicial;
            Fecha_Inicial = Convert.ToDateTime(Txt_Fecha_Inicial.Text);
            if (Convert.ToDateTime(Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FECHA"].ToString()) < Fecha_Inicial)
            {
                Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                Fila_Borrada = true;
            }
        }
        if (Txt_Fecha_Final.Text.Length > 0 && Fila_Borrada != true)
        {
            DateTime Fecha_Final;
            Fecha_Final = Convert.ToDateTime(Txt_Fecha_Final.Text);
            Fecha_Final = Fecha_Final.AddHours(23).AddMinutes(59).AddSeconds(59);
            if (Convert.ToDateTime(Dt_Determinaciones_Generadas.Rows[Cont_Borrado]["FECHA"].ToString()) > Fecha_Final)
            {
                Dt_Determinaciones_Generadas.Rows[Cont_Borrado].Delete();
                Fila_Borrada = true;
            }
        }
        if (Fila_Borrada != true)
            Cont_Borrado++;

        return Cont_Borrado;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Lugar_Almacenamiento
    ///DESCRIPCIÓN          : Activa o Desactiva los controles para capturar el lugar de almacenamiento
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 22/032/2012 04:48:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Lugar_Almacenamiento(Boolean Activar)
    {
        Txt_Lugar.Enabled = Activar;
        Txt_Costo.Enabled = Activar;
        Txt_Dimensiones.Enabled = Activar;
        Txt_Fecha_Ingreso.Enabled = Activar;
        Txt_Tiempo_transcurrido.Enabled = Activar;
        Txt_Costo_Almacenamiento.Enabled = Activar;

        if (Activar == false)
        {
            Txt_Lugar.Text = "";
            Txt_Costo.Text = "";
            Txt_Dimensiones.Text = "";
            Txt_Fecha_Ingreso.Text = "";
            Txt_Tiempo_transcurrido.Text = "";
            Txt_Costo_Almacenamiento.Text = "";
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Bienes
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las publicaciones
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 06/03/2012 05:20:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Bienes()
    {
        DataTable Dt_Bienes = new DataTable();
        Dt_Bienes.Columns.Add(new DataColumn("NO_BIEN", typeof(String)));
        Dt_Bienes.Columns.Add(new DataColumn("TIPO_BIEN_ID", typeof(String)));
        Dt_Bienes.Columns.Add(new DataColumn("TIPO_BIEN", typeof(String)));
        Dt_Bienes.Columns.Add(new DataColumn("DESCRIPCION", typeof(String)));
        Dt_Bienes.Columns.Add(new DataColumn("VALOR", typeof(Decimal)));
        Dt_Bienes.Columns.Add(new DataColumn("FOTOGRAFIAS", typeof(String)));
        Dt_Bienes.Columns.Add(new DataColumn("ARCHIVOS", typeof(String)));
        return Dt_Bienes;
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Llenar_DataRow_Bienes
    ///DESCRIPCIÓN          : Agrega una nueva fila a la tabla Bienes que llega como parámetro
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 24/02/2012 11:49:00 a.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Llenar_DataRow_Bienes(DataTable Dt_Bienes)
    {
        DataRow Dr_Bien;
        decimal Valor;

        decimal.TryParse(Txt_Valor_Bien.Text.Replace("$", ""), out Valor);

        Dr_Bien = Dt_Bienes.NewRow();
        Dr_Bien["TIPO_BIEN_ID"] = Cmb_Tipo_de_bien.SelectedValue;
        Dr_Bien["TIPO_BIEN"] = Cmb_Tipo_de_bien.SelectedItem.Text;
        Dr_Bien["DESCRIPCION"] = Txt_Descripcion.Text.ToUpper();
        Dr_Bien["VALOR"] = Valor;
        Dr_Bien["FOTOGRAFIAS"] = Txt_Fotografias.Text;
        Dr_Bien["ARCHIVOS"] = Hdn_Archivos_Bien.Value;
        Dt_Bienes.Rows.Add(Dr_Bien);//Se asigna la nueva fila a la tabla
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Datos_Peritajes
    /// DESCRIPCIÓN: Carga los datos de peritajes que llegan en la tabla a los controles correspondientes
    ///         (cada renglón de la tabla debe contener información de un peritaje).
    /// PARÁMETROS:
    /// 		1. Dt_Peritajes: tabla con los datos a cargar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cargar_Datos_Peritajes(DataTable Dt_Peritajes)
    {
        DateTime Fecha_Peritaje;
        decimal Valor_Peritaje;

        if (Dt_Peritajes != null)
        {
            Cmb_Avaluo.Items.Clear();
            // agregar elementos en la tabla (hasta tres)
            for (int i = 0; i < Dt_Peritajes.Rows.Count && i < 3; i++)
            {
                ListItem Nuevo_Elemento = new ListItem(Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Avaluo].ToString(), Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_No_Peritaje].ToString());
                DateTime.TryParse(Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje].ToString(), out Fecha_Peritaje);
                decimal.TryParse(Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Valor].ToString(), out Valor_Peritaje);
                // cargar datos peritaje
                Cmb_Avaluo.Items.Add(Nuevo_Elemento);
                Cmb_Avaluo.SelectedValue = Nuevo_Elemento.Value;
                Txt_Fecha_Peritaje.Text = Fecha_Peritaje.ToString("dd/MMM/yyyy");
                Txt_Perito.Text = Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Perito].ToString();
                Txt_Valor_Peritaje.Text = Valor_Peritaje.ToString("#,##0.00");
                Txt_Observaciones.Text = Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Observaciones].ToString();

                // validar lugar de almacenamiento
                if (string.IsNullOrEmpty(Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento].ToString()))
                {
                    // almacenado en el predio
                    RBtn_Lugar.SelectedIndex = 0;
                }
                else
                {
                    DateTime Fecha_Almacenamiento;
                    decimal Costo_Metro;
                    decimal Dimensiones;
                    DateTime.TryParse(Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso].ToString(), out Fecha_Almacenamiento);
                    decimal.TryParse(Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Costo_Metro_Cuadrado].ToString(), out Costo_Metro);
                    decimal.TryParse(Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Dimensiones].ToString(), out Dimensiones);
                    // seleccionar Lugar externo y cargar datos almacenamiento
                    RBtn_Lugar.SelectedIndex = 1;
                    Txt_Lugar.Text = Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento].ToString();
                    Txt_Costo.Text = Costo_Metro.ToString("#,##0.00");
                    Txt_Dimensiones.Text = Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Dimensiones].ToString();
                    // validar fecha de ingreso
                    if (Fecha_Almacenamiento != DateTime.MinValue)
                    {
                        Txt_Fecha_Ingreso.Text = Fecha_Almacenamiento.ToString("dd/MMM/yyyy");
                    }
                    else
                    {
                        Txt_Fecha_Ingreso.Text = "";
                    }
                    Txt_Tiempo_transcurrido.Text = Dt_Peritajes.Rows[i][Ope_Pre_Pae_Peritajes.Campo_Tiempo_Transcurrido].ToString();
                }
            }

            // cargar bienes si hay un peritaje seleccionado
            if (Cmb_Avaluo.SelectedValue != "0")
            {
                Cls_Ope_Pre_Pae_Bienes_Negocio Consulta_Bienes = new Cls_Ope_Pre_Pae_Bienes_Negocio();
                DataTable Dt_Bienes;

                Consulta_Bienes.P_No_Peritaje = Cmb_Avaluo.SelectedValue;
                // consultar bienes
                Dt_Bienes = Consulta_Bienes.Consulta_Bienes();
                // cargar bienes en el combo
                Grid_Bienes.DataSource = Dt_Bienes;
                Grid_Bienes.DataBind();
            }

            // agregar peritajes faltantes
            if (Cmb_Avaluo.Items.FindByText("CONTRIBUYENTE") == null)
            {
                Cmb_Avaluo.Items.Insert(Cmb_Avaluo.Items.Count, new ListItem("CONTRIBUYENTE", "0"));
            }
            else if (Cmb_Avaluo.Items.FindByText("TERCERO") == null)
            {
                Cmb_Avaluo.Items.Insert(Cmb_Avaluo.Items.Count, new ListItem("TERCERO", "0"));
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Datos_Peritaje
    /// DESCRIPCIÓN: Revisar que los campos obligatorios hayan sido llenados y si no, generar el mensaje 
    ///             correspondiente (regresa un string con errores encontrados).
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private string Validar_Datos_Peritaje()
    {
        //Si falta alguno de los campos mencionarlo en la etiqueta Mensaje para mostrarla 
        string Mensaje = "";
        decimal Monto;
        DateTime Fecha;
        DataTable Dt_Bienes = new DataTable();

        if (Txt_Perito.Text.Trim() == "")  // Validar campo Perito (no vacío)
        {
            Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el nombre del Perito <br />";
        }

        if (!decimal.TryParse(Txt_Valor_Peritaje.Text, out Monto) || Monto <= 0)  // Validar campo Valor_Peritaje
        {
            Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir un valor para el peritaje<br />";
        }
        Txt_Valor_Peritaje.Text = Monto.ToString("#,##0.00");

        if (!DateTime.TryParse(Txt_Fecha_Peritaje.Text, out Fecha) || Fecha == DateTime.MinValue)  // Validar campo Fecha_Peritaje
        {
            Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir una fecha de peritaje válida <br />";
            Txt_Fecha_Peritaje.Text = "";
        }
        else
        {
            Txt_Fecha_Peritaje.Text = Fecha.ToString("dd/MMM/yyyy");
        }

        // intentar recuperar dt_Bienes de la sesión
        if (Session["Bienes"] != null)
        {
            Dt_Bienes = (DataTable)Session["Bienes"];
        }
        // validar que la tabla bienes contenga filas
        if (Dt_Bienes.Rows.Count <= 0)
        {
            Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Agregar bienes al peritaje <br />";
        }

        // si Lugar externo está seleccionado, validar que se hayan agregado datos para el lugar de almacenamiento
        if (RBtn_Lugar.SelectedIndex > 0)
        {

            if (Txt_Lugar.Text.Trim() == "")  // Validar campo lugar de almacenamiento (no vacío)
            {
                Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el Lugar de almacenamiento <br />";
            }

            if (!decimal.TryParse(Txt_Costo.Text, out Monto) || Monto <= 0)  // Validar campo Costo m2
            {
                Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir el Costo m&sup2; del almacenamiento<br />";
            }
            Txt_Costo.Text = Monto.ToString("#,##0.00");

            if (Txt_Dimensiones.Text.Trim() == "")  // Validar campo dimensiones (no vacío)
            {
                Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir las dimensiones para el almacenamiento <br />";
            }

            if (!DateTime.TryParse(Txt_Fecha_Ingreso.Text, out Fecha) || Fecha == DateTime.MinValue)  // Validar campo Fecha almacenamiento
            {
                Mensaje += "&nbsp; &nbsp; &nbsp; &nbsp; + Introducir una fecha de almacenamiento válida <br />";
                Txt_Fecha_Ingreso.Text = "";
            }

        }

        return Mensaje;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Alta_Peritaje
    /// DESCRIPCIÓN: Da de alta un Peritaje en la base de datos a través de la capa de negocio
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 27-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Alta_Peritaje()
    {
        DataTable Dt_Bienes;
        var Alta_Peritaje_Negocio = new Cls_Ope_Pre_Pae_Bienes_Negocio(); // Variable de conexión hacia la capa de negocio para envio de los datos a dar de alta

        try
        {
            Alta_Peritaje_Negocio.P_Avaluo = Cmb_Avaluo.SelectedItem.Text;
            Alta_Peritaje_Negocio.P_Fecha_Peritaje = DateTime.Parse(Txt_Fecha_Peritaje.Text);
            Alta_Peritaje_Negocio.P_Perito = Txt_Perito.Text.Trim().ToUpper();
            Alta_Peritaje_Negocio.P_No_Detalle_Etapa = Grid_Embargos_Generados.SelectedRow.Cells[9].Text;
            Alta_Peritaje_Negocio.P_Valor_Peritaje = Txt_Valor_Peritaje.Text.Replace(",", "");
            Alta_Peritaje_Negocio.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
            Alta_Peritaje_Negocio.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            // agregar datos de almacenamiento si Lugar externo está seleccionado
            if (RBtn_Lugar.SelectedIndex > 0)
            {
                Alta_Peritaje_Negocio.P_Lugar_Almacenamiento = Txt_Lugar.Text.Trim().ToUpper();
                Alta_Peritaje_Negocio.P_Dimensiones = Txt_Dimensiones.Text.Trim().ToUpper();
                Alta_Peritaje_Negocio.P_Costo_Metro_Cuadrado = Txt_Costo.Text.Trim().ToUpper();
                Alta_Peritaje_Negocio.P_Fecha_Ingreso = DateTime.Parse(Txt_Fecha_Ingreso.Text);
            }
            // tratar de recuperar la tabla de bienes
            if (Session["Bienes"] != null)
            {
                Dt_Bienes = (DataTable)Session["Bienes"];
                Alta_Peritaje_Negocio.P_Dt_Bienes = Dt_Bienes;
                Asignar_Ruta_Relativa_Archivos(Dt_Bienes);
                Guardar_Archivos(Dt_Bienes);
            }

            if (Alta_Peritaje_Negocio.Alta_Pae_Peritajes() > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Peritajes bienes", "alert('El Alta del Peritaje fue Exitosa');", true);
                Inicializa_Controles();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Peritajes bienes", "alert('Ocurrió un error y el Peritaje no se dio de alta');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta_Peritaje " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Modificar_Peritaje
    /// DESCRIPCIÓN: Actualiza un Peritaje en la base de datos a través de la capa de negocio
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Modificar_Peritaje()
    {
        DataTable Dt_Bienes;
        DateTime Fecha_Peritaje;
        DateTime Fecha_Ingreso;
        var Modificar_Peritaje_Negocio = new Cls_Ope_Pre_Pae_Bienes_Negocio(); // Variable de conexión hacia la capa de negocio

        try
        {
            Modificar_Peritaje_Negocio.P_No_Peritaje = Cmb_Avaluo.SelectedValue;
            Modificar_Peritaje_Negocio.P_Avaluo = Cmb_Avaluo.SelectedItem.Text;
            DateTime.TryParse(Txt_Fecha_Peritaje.Text, out Fecha_Peritaje);
            Modificar_Peritaje_Negocio.P_Fecha_Peritaje = Fecha_Peritaje;
            Modificar_Peritaje_Negocio.P_Perito = Txt_Perito.Text.Trim().ToUpper();
            Modificar_Peritaje_Negocio.P_No_Detalle_Etapa = Grid_Embargos_Generados.SelectedRow.Cells[9].Text;
            Modificar_Peritaje_Negocio.P_Valor_Peritaje = Txt_Valor_Peritaje.Text.Replace(",", "");
            Modificar_Peritaje_Negocio.P_Observaciones = Txt_Observaciones.Text.Trim().ToUpper();
            Modificar_Peritaje_Negocio.P_Nombre_Usuario = Cls_Sessiones.Nombre_Empleado.ToUpper();
            // agregar datos de almacenamiento

            Modificar_Peritaje_Negocio.P_Lugar_Almacenamiento = Txt_Lugar.Text.Trim().ToUpper();
            Modificar_Peritaje_Negocio.P_Dimensiones = Txt_Dimensiones.Text.Trim().ToUpper();
            Modificar_Peritaje_Negocio.P_Costo_Metro_Cuadrado = Txt_Costo.Text.Trim().ToUpper();
            DateTime.TryParse(Txt_Fecha_Ingreso.Text, out Fecha_Ingreso);
            Modificar_Peritaje_Negocio.P_Fecha_Ingreso = Fecha_Ingreso;

            // tratar de recuperar la tabla de bienes
            if (Session["Bienes"] != null)
            {
                Dt_Bienes = (DataTable)Session["Bienes"];
                Modificar_Peritaje_Negocio.P_Dt_Bienes = Dt_Bienes;
                Asignar_Ruta_Relativa_Archivos(Dt_Bienes);
                Guardar_Archivos(Dt_Bienes);
            }
            // tratar de recuperar la lista de bienes a eliminar
            if (Session["Lista_Bienes_Eliminar"] != null)
            {
                Modificar_Peritaje_Negocio.P_Bienes_Eliminar = (List<string>)Session["Lista_Bienes_Eliminar"];
            }

            if (Modificar_Peritaje_Negocio.Modificar_Peritajes() > 0)
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Peritajes bienes", "alert('La actualización del Peritaje fue Exitosa');", true);
                Inicializa_Controles();
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Peritajes bienes", "alert('Ocurrió un error y el Peritaje no se actualizó');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Modificar_Peritaje " + Ex.Message.ToString(), Ex);
        }
    }

    ///*******************************************************************************************************
    /// 	NOMBRE_FUNCIÓN: Obtener_Diccionario_Archivos
    /// 	DESCRIPCIÓN: Regresa el diccionario checksum-archivo si se encuentra en variable de sesion y si no,
    /// 	            regresa un diccionario vacio
    /// 	PARÁMETROS:
    /// 	CREO: Roberto González Oseguera
    /// 	FECHA_CREO: 02-may-2012
    /// 	MODIFICÓ: 
    /// 	FECHA_MODIFICÓ: 
    /// 	CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<String, Byte[]> Obtener_Diccionario_Archivos()
    {
        // si existe el diccionario en variable de sesion
        if (Session["Diccionario_Archivos"] != null)
        {
            return (Dictionary<String, Byte[]>)Session["Diccionario_Archivos"];
        }
        else
        {
            return new Dictionary<String, Byte[]>();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Asignar_Ruta_Relativa_Archivos
    /// DESCRIPCIÓN: Lee los nombres de archivos contenidos en la tabla que llega como parámetro y les 
    ///         agrega el nombre de directorio relativo: 
    ///         \Archivos\PAE\Bienes\Cuenta_predial\No_detalle_etapa\archivo
    /// PARÁMETROS:
    ///         1. Dt_Bienes: tabla que contiene los nombres de los archivos a los que se va a asignar una ruta relativa
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Asignar_Ruta_Relativa_Archivos(DataTable Dt_Bienes)
    {
        String Nombre_Directorio;
        int Contador_Fila;
        int No_Bien;
        String[] Arr_Nombres_Archivos;
        string Archivos_Bien;
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();

        try
        {
            if (Dt_Bienes != null)     //si la tabla tramites contiene datos
            {
                foreach (DataRow Fila_Bien in Dt_Bienes.Rows)   // recorrer la tabla
                {
                    // si el bien tiene un NO_BIEN, pasar al siguiente renglón (son bienes que ya están guardados)
                    if (int.TryParse(Fila_Bien[Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString(), out No_Bien) && No_Bien > 0)
                    {
                        continue;
                    }

                    // nombre directorio: ..\..\Archivos\PAE\Bienes\Cuenta_predial\No_detalle_etapa
                    Nombre_Directorio = @"..\..\Archivos\PAE\Bienes\NCP_" + Grid_Embargos_Generados.SelectedRow.Cells[1].Text + @"\" + Grid_Embargos_Generados.SelectedRow.Cells[9].Text;

                    //separar los nombres de archivo en un arreglo
                    Arr_Nombres_Archivos = Fila_Bien["FOTOGRAFIAS"].ToString().Split(',');
                    Archivos_Bien = "";
                    for (Contador_Fila = 0; Contador_Fila < Arr_Nombres_Archivos.Length; Contador_Fila++)   //recorrer el arreglo de nombres de archivo
                    {
                        // Si contiene un nombre de archivo y no contiene diagonales, agregar ruta relativa
                        if (Arr_Nombres_Archivos[Contador_Fila] != "" && !Arr_Nombres_Archivos[Contador_Fila].Contains(@"\"))
                        {
                            // actualizar nombres de archivos (ruta completa)
                            Archivos_Bien += Nombre_Directorio + @"\" + Arr_Nombres_Archivos[Contador_Fila].Trim() + ",";
                        }
                        else
                        {
                            // dejar valor original
                            Archivos_Bien += Arr_Nombres_Archivos[Contador_Fila] + ",";
                        }
                    }
                    Fila_Bien["FOTOGRAFIAS"] = Archivos_Bien.Substring(0, Archivos_Bien.Length - 1);
                }
                // guardar cambios a la tabla tramites en variable de sesion
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Asignar_Ruta_Relativa_Archivos: " + Ex.Message.ToString(), Ex);
        }

        return Dt_Bienes;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Guardar_Archivos
    /// DESCRIPCIÓN: Guardar en el servidor los archivos que se hayan recibido
    /// PARÁMETROS:
    ///         1. Dt_Bienes: tabla que contiene los nombres de los archivos a guardar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private DataTable Guardar_Archivos(DataTable Dt_Bienes)
    {
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();
        String Nombre_Directorio;
        String Ruta_Archivo;
        int Contador_Archivo_Existente;
        int No_Bien;

        try
        {
            if (Dt_Bienes != null)     //si la tabla tramites contiene datos
            {
                foreach (DataRow Fila_Bien in Dt_Bienes.Rows)   // recorrer la tabla
                {
                    // si el bien tiene un NO_BIEN, pasar al siguiente renglón (son bienes que ya están guardados)
                    if (int.TryParse(Fila_Bien[Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString(), out No_Bien) && No_Bien > 0)
                    {
                        continue;
                    }

                    // separar los nombres de archivo y los checksums en arreglos
                    String[] Arr_Nombres_Archivos = Fila_Bien["FOTOGRAFIAS"].ToString().Replace(", ", "").Split(',');
                    String[] Arr_Checksum = Fila_Bien["ARCHIVOS"].ToString().Split(',');
                    for (int i = 0; i < Arr_Checksum.Length; i++)   //recorrer el arreglo de checksums
                    {
                        Contador_Archivo_Existente = 0;
                        // si contiene un checksum y ese checksum existe en el diccionario checksum-archivo
                        if (Arr_Checksum[i] != "" && Diccionario_Archivos.ContainsKey(Arr_Checksum[i]))
                        {
                            Nombre_Directorio = Server.MapPath(Path.GetDirectoryName(Arr_Nombres_Archivos[i]));
                            Ruta_Archivo = Server.MapPath(HttpUtility.HtmlDecode(Arr_Nombres_Archivos[i]));
                            if (!Directory.Exists(Nombre_Directorio))                       //si el directorio no existe, crearlo
                                Directory.CreateDirectory(Nombre_Directorio);
                            // si el archivo ya existe, agregar contador numérico
                            string Nombre_Original = Arr_Nombres_Archivos[i];
                            while (File.Exists(Ruta_Archivo))
                            {
                                // obtener un checksum del archivo existente
                                HashAlgorithm sha = HashAlgorithm.Create("SHA1");
                                FileStream Archivo_Existente = new FileStream(Ruta_Archivo, FileMode.Open, FileAccess.ReadWrite);
                                String Checksum_Archivo_Existente = BitConverter.ToString(sha.ComputeHash(Archivo_Existente));
                                Archivo_Existente.Close();
                                // si el checksum del archivo nuevo es igual al del archivo existente con el mismo nombre, se abandona el ciclo y el archivo será sustituido
                                if (Arr_Checksum[i] == Checksum_Archivo_Existente)
                                {
                                    break;
                                }

                                string Extension = Path.GetExtension(Arr_Nombres_Archivos[i]);
                                Arr_Nombres_Archivos[i] = Nombre_Original.Replace(Extension, (++Contador_Archivo_Existente).ToString() + Extension);
                                // actualizar valor en datatable
                                Fila_Bien.BeginEdit();
                                Ruta_Archivo = Arr_Nombres_Archivos[i];
                                Fila_Bien["FOTOGRAFIAS"] = Ruta_Archivo;
                                Fila_Bien.AcceptChanges();
                            }
                            //crear filestream y binarywriter para guardar archivo
                            FileStream Escribir_Archivo = new FileStream(Ruta_Archivo, FileMode.Create, FileAccess.Write);
                            BinaryWriter Datos_Archivo = new BinaryWriter(Escribir_Archivo);

                            // Guardar archivo (escribir datos en el filestream)
                            Datos_Archivo.Write(Diccionario_Archivos[Arr_Checksum[i]]);
                        }
                    }
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Guardar_Archivos " + Ex.Message.ToString(), Ex);
        }

        return Dt_Bienes;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Detalles_Peritaje
    /// DESCRIPCIÓN: Consultar información del peritaje (o avalúo) y lo muestra
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************   
    protected void Cargar_Detalles_Peritaje()
    {
        DataTable Dt_Peritajes;
        var Consulta_Peritajes_Bienes = new Cls_Ope_Pre_Pae_Bienes_Negocio();

        // validar que haya una fila seleccionada
        if (Grid_Embargos_Generados.SelectedIndex > -1)
        {
            Consulta_Peritajes_Bienes.P_No_Peritaje = Cmb_Avaluo.SelectedValue;
            Dt_Peritajes = Consulta_Peritajes_Bienes.Consulta_Peritajes();
            // si hay peritajes, cargar datos
            if (Dt_Peritajes != null && Dt_Peritajes.Rows.Count > 0)
            {
                DateTime Fecha_Peritaje;
                decimal Valor_Peritaje;
                DataTable Dt_Bienes;
                DateTime.TryParse(Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Fecha_Peritaje].ToString(), out Fecha_Peritaje);
                decimal.TryParse(Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Valor].ToString(), out Valor_Peritaje);
                // cargar datos peritaje
                Txt_Fecha_Peritaje.Text = Fecha_Peritaje.ToString("dd/MMM/yyyy");
                Txt_Perito.Text = Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Perito].ToString();
                Txt_Valor_Peritaje.Text = Valor_Peritaje.ToString("#,##0.00");
                Txt_Observaciones.Text = Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Observaciones].ToString();

                // validar lugar de almacenamiento
                if (string.IsNullOrEmpty(Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento].ToString()))
                {
                    // almacenado en el predio
                    RBtn_Lugar.SelectedIndex = 0;
                }
                else
                {
                    DateTime Fecha_Almacenamiento;
                    decimal Costo_Metro;
                    decimal Dimensiones;
                    DateTime.TryParse(Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Fecha_Ingreso].ToString(), out Fecha_Almacenamiento);
                    decimal.TryParse(Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Costo_Metro_Cuadrado].ToString(), out Costo_Metro);
                    decimal.TryParse(Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Dimensiones].ToString(), out Dimensiones);
                    // seleccionar Lugar externo y cargar datos almacenamiento
                    RBtn_Lugar.SelectedIndex = 1;
                    Txt_Lugar.Text = Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Lugar_Almacenamiento].ToString();
                    Txt_Costo.Text = Costo_Metro.ToString("#,##0.00");
                    Txt_Dimensiones.Text = Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Dimensiones].ToString();
                    // validar fecha de ingreso
                    if (Fecha_Almacenamiento != DateTime.MinValue)
                    {
                        Txt_Fecha_Ingreso.Text = Fecha_Almacenamiento.ToString("dd/MMM/yyyy");
                    }
                    else
                    {
                        Txt_Fecha_Ingreso.Text = "";
                    }
                    Txt_Tiempo_transcurrido.Text = Dt_Peritajes.Rows[0][Ope_Pre_Pae_Peritajes.Campo_Tiempo_Transcurrido].ToString();
                }

                // consultar bienes
                Dt_Bienes = Consulta_Peritajes_Bienes.Consulta_Bienes();
                // cargar bienes en el combo
                Grid_Bienes.DataSource = Dt_Bienes;
                Grid_Bienes.DataBind();

            }

        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Recuperar_Tabla_Bienes
    /// DESCRIPCIÓN: Consultar información de los bienes para formar el datatable que se guarda en una sesión 
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************   
    protected void Recuperar_Tabla_Bienes()
    {
        DataTable Dt_Bienes;
        var Consulta_Bienes = new Cls_Ope_Pre_Pae_Bienes_Negocio();

        // validar que haya una fila seleccionada
        if (Cmb_Avaluo.SelectedIndex > -1 && Cmb_Avaluo.SelectedValue != "0")
        {
            Consulta_Bienes.P_No_Peritaje = Cmb_Avaluo.SelectedValue;
            // consultar bienes
            Dt_Bienes = Consulta_Bienes.Consulta_Bienes();
            // cargar bienes en el combo
            Grid_Bienes.DataSource = Dt_Bienes;
            Grid_Bienes.DataBind();
            // si hay bienes, agregar columna archivos y guardar en variable de sesión
            if (Dt_Bienes != null && Dt_Bienes.Rows.Count > 0)
            {
                Dt_Bienes.Columns.Add(new DataColumn("ARCHIVOS", typeof(String)));
                // guardar en variable de sesión
                Session["Bienes"] = Dt_Bienes;
            }

        }
    }

    #endregion METODOS

    #region EVENTOS
    #region TextBox
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
        var Consulta_Propietario_Negocio = new Cls_Ope_Pre_Pagos_Instituciones_Externas_Negocio();
        DataTable Dt_Propietario = new DataTable();
        try
        {
            if (Session["BUSQUEDA_CUENTAS_PREDIAL"] != null)
            {
                if (Session["CUENTA_PREDIAL_ID"] != null)
                {
                    Consulta_Propietario_Negocio.P_Cuenta_Predial_Id = Session["CUENTA_PREDIAL_ID"].ToString();
                    Dt_Propietario = Consulta_Propietario_Negocio.Consultar_Propietario();
                    if (Dt_Propietario != null && Dt_Propietario.Rows.Count > 0)
                    {
                        Txt_Contribuyente.Text = Dt_Propietario.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                        Hdn_Contribuyente_ID.Value = Dt_Propietario.Rows[0][Cat_Pre_Propietarios.Campo_Contribuyente_ID].ToString();
                    }
                    Session.Remove("CUENTA_PREDIAL_ID");
                }
                Session.Remove("BUSQUEDA_CUENTAS_PREDIAL");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Nuevo_Click
    /// DESCRIPCIÓN: Dependiendo del estado de los controles llama al método para preparar los controles 
    ///         para permitir insertar nuevos datos (si hay una remoción seleccionada) o si ya se agregaron 
    ///         datos, llama al método que los da de alta en la base de datos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Limpia_Mensaje_Error();
        try
        {
            if (Btn_Nuevo.ToolTip == "Nuevo")
            {
                // habilitar controles para insertar datos
                Habilitar_Controles("Nuevo");
                RBtn_Lugar.SelectedIndex = 0;
                Lugar_Almacenamiento(false);
                // inicializar fecha peritaje
                Txt_Fecha_Peritaje.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                Txt_Perito.Focus();
            }
            else
            {
                string Mensaje_Error = "";
                Mensaje_Error = Validar_Datos_Peritaje();

                //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                if (Mensaje_Error != "")
                {
                    Mostrar_Mensaje_Error("Es necesario: <br />" + Mensaje_Error);
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario, dar de alta los mismos en la base de datos
                else
                {
                    Alta_Peritaje(); //Da de alta los datos proporcionados por el usuario
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Modificar_Click
    /// DESCRIPCIÓN: Dependiendo del estado de los controles llama al método para preparar los controles 
    ///         para permitir modificar datos (si hay una remoción seleccionada) o si ya se agregaron 
    ///         datos, llama al método que actualiza el bien en la base de datos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Limpia_Mensaje_Error();
        try
        {
            if (Btn_Modificar.ToolTip == "Modificar")
            {
                // habilitar controles para insertar datos
                Habilitar_Controles("Modificar");
                Recuperar_Tabla_Bienes();
            }
            else
            {
                string Mensaje_Error = "";
                Mensaje_Error = Validar_Datos_Peritaje();

                //Si faltaron campos por capturar envía un mensaje al usuario indicando cuáles
                if (Mensaje_Error != "")
                {
                    Mostrar_Mensaje_Error("Es necesario: <br />" + Mensaje_Error);
                }
                //Si todos los campos requeridos fueron proporcionados por el usuario, modificar en la base de datos
                else
                {
                    Modificar_Peritaje();
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
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
                Session.Remove("Bienes");
                Session.Remove("Lista_Bienes_Eliminar");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
                HttpContext.Current.Session.Remove("Activa");
            }
            else
            {
                Inicializa_Controles();
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Buscar_Bienes_Click
    /// DESCRIPCIÓN: Buscar remociones de acuerdo con los filtros especificados
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 25-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Buscar_Bienes_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Pre_Pae_Bienes_Negocio Consulta_Remociones = new Cls_Ope_Pre_Pae_Bienes_Negocio();
        DataTable Dt_Remociones = null;
        int Folio_Inicial;
        int Folio_Final;
        DateTime Fecha_Inicial;
        DateTime Fecha_Final;

        Limpia_Mensaje_Error();//Limpia el mensaje error

        try
        {
            Consulta_Remociones.P_Proceso_Actual = "REMOCION";

            // agregar filtro por despacho a la consulta si hay un despacho seleccionado
            if (Cmb_Asignado_a.SelectedIndex > 0)
            {
                Consulta_Remociones.P_Despacho_ID = Cmb_Asignado_a.SelectedValue;
            }

            // agregar filtro por ESTATUS a la consulta si hay uno seleccionado
            if (Cmb_Estatus.SelectedIndex > 0)
            {
                Consulta_Remociones.P_Estatus_Etapa = Cmb_Estatus.SelectedValue;
            }

            // agregar filtro por CUENTA PREDIAL a la consulta si hay una seleccionada
            if (Txt_Numero_Cuenta.Text.Length > 0)
            {
                Consulta_Remociones.P_Cuenta_Predial = Txt_Numero_Cuenta.Text;
            }

            // agregar filtro por Contribuyente a la consulta si hay uno seleccionado
            if (Hdn_Contribuyente_ID.Value.Length > 0)
            {
                Consulta_Remociones.P_Contribuyente_ID = Hdn_Contribuyente_ID.Value;
            }

            // agregar filtro por Folio a la consulta
            if (int.TryParse(Txt_Folio_Inicial.Text, out Folio_Inicial) && Folio_Inicial > 0)
            {
                Consulta_Remociones.P_Folio_Inicial = Folio_Inicial;
            }

            // agregar filtro por Folio a la consulta
            if (int.TryParse(Txt_Folio_Final.Text, out Folio_Final) && Folio_Final > 0)
            {
                Consulta_Remociones.P_Folio_Final = Folio_Final;
            }

            // agregar filtro por Fecha a la consulta
            if (DateTime.TryParse(Txt_Fecha_Final.Text, out Fecha_Final))
            {
                Consulta_Remociones.P_Fecha_Final = Fecha_Final;
            }

            // agregar filtro por Fecha a la consulta
            if (DateTime.TryParse(Txt_Fecha_Inicial.Text, out Fecha_Inicial))
            {
                Consulta_Remociones.P_Fecha_Inicial = Fecha_Inicial;
            }

            // ejecutar consulta
            Dt_Remociones = Consulta_Remociones.Consultar_Detalles_Etapas();

            Grid_Embargos_Generados.Columns[8].Visible = true;
            Grid_Embargos_Generados.Columns[9].Visible = true;
            Grid_Embargos_Generados.DataSource = Dt_Remociones;
            Grid_Embargos_Generados.DataBind();
            Grid_Embargos_Generados.Columns[8].Visible = false;
            Grid_Embargos_Generados.Columns[9].Visible = false;

            Session["Grid_Embargos_Generados"] = Dt_Remociones;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error("Btn_Buscar: " + ex.Message.ToString());
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : RBtn_Lugar_SelectedIndexChanged
    ///DESCRIPCIÓN          : Activa o desactiva los controles para capturar Lugar de almacenamiento
    ///PARAMETROS: 
    ///CREO                 : Armando Zavala Moreno
    ///FECHA_CREO           : 22/03/2012 05:05:00 p.m.
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///******************************************************************************* 
    protected void RBtn_Lugar_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (RBtn_Lugar.SelectedIndex != 0)
            Lugar_Almacenamiento(true);
        else
            Lugar_Almacenamiento(false);
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Agregar_Bien_Click
    /// DESCRIPCIÓN: Manejo del evento click en el Btn_Agregar_Bien
    ///             Llamar al método que agrega el bien ingresado a la tabla en la sesión "Bienes", 
    ///             sólo si se ingresaron los datos obligatorios
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Agregar_Bien_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Bienes;
        decimal Valor_Bien;

        if (Session["Bienes"] != null)
        {
            Dt_Bienes = (DataTable)Session["Bienes"];
        }
        else
        {
            Dt_Bienes = Crear_Tabla_Bienes();
        }

        // validar que se hayan ingresado datos 
        if (Cmb_Tipo_de_bien.SelectedIndex > 0 && decimal.TryParse(Txt_Valor_Bien.Text, out Valor_Bien) && Valor_Bien > 0 && Txt_Descripcion.Text.Length > 0)
        {
            Llenar_DataRow_Bienes(Dt_Bienes);
            Grid_Bienes.DataSource = Dt_Bienes;
            Grid_Bienes.DataBind();
            Session["Bienes"] = Dt_Bienes;

            // limpiar controles con datos del bien
            Txt_Valor_Bien.Text = "";
            Txt_Descripcion.Text = "";
            Cmb_Tipo_de_bien.SelectedIndex = 0;
            Txt_Fotografias.Text = "";
            Hdn_Archivos_Bien.Value = "";
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Btn_Subir_Archivo_Click
    /// DESCRIPCIÓN: Se completo el envio de archivo, guardar archivo en variable de sesion 
    ///             (diccionario <checksum-archivo>) y marcar el checkbox del tipo de documentos recibido
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Btn_Subir_Archivo_Click(object sender, ImageClickEventArgs e)
    {
        HashAlgorithm sha = HashAlgorithm.Create("SHA1");
        String Extension_Archivo = Path.GetExtension(Fup_Bien.FileName).ToLower();
        // arreglo con las extensiones de archivo permitidas
        String[] Extensiones_Permitidas = { ".jpg", ".jpeg", ".png", ".gif", ".doc", ".docx", ".ppt", ".pptx" };
        String Checksum_Archivo = BitConverter.ToString(sha.ComputeHash(Fup_Bien.FileBytes));       //obtener checksum del archivo
        Dictionary<String, Byte[]> Diccionario_Archivos = Obtener_Diccionario_Archivos();   //obtener diccionario checksum-archivo

        if (Fup_Bien.HasFile)
        {
            //limpiar mensajes de error y campos de texto
            Limpia_Mensaje_Error();

            // si la extension del archivo recibido no es valida, regresar
            if (Array.IndexOf(Extensiones_Permitidas, Extension_Archivo) < 0)
            {
                Mostrar_Mensaje_Error(" No se permite subir archivos con extensión: " + Extension_Archivo);
                return;
            }

            if (Fup_Bien.FileBytes.Length > 2048000) // si la longitud del archivo recibido es mayor que 2MB, mostrar mensaje
            {
                Mostrar_Mensaje_Error(" El tamaño del archivo es mayor al permitido.");
                return;
            }

            //si el checksum no esta en el diccionario, agregarlo y guardar en variable de sesion
            if (!Diccionario_Archivos.ContainsKey(Checksum_Archivo))
            {
                Diccionario_Archivos.Add(Checksum_Archivo, Fup_Bien.FileBytes);
                Session["Diccionario_Archivos"] = Diccionario_Archivos;
            }

            // agregar nombre del archivo a caja de texto
            if (Txt_Fotografias.Text.Length > 0)
            {
                Txt_Fotografias.Text += "," + Fup_Bien.FileName.Replace(' ', '_');
            }
            else
            {
                Txt_Fotografias.Text = Fup_Bien.FileName.Replace(' ', '_');
            }

            // agregar checksum a control oculto
            if (Hdn_Archivos_Bien.Value.Length > 0)
            {
                Hdn_Archivos_Bien.Value += "," + Checksum_Archivo;
            }
            else
            {
                Hdn_Archivos_Bien.Value = Checksum_Archivo;
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cmb_Avaluo_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de índice seleccionado en el combo Avaluo
    ///             llamar método que consultar información del peritaje 
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 03-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************   
    protected void Cmb_Avaluo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Limpia_Mensaje_Error();

        try
        {
            // Limpiar_Detalles_Peritaje
            Txt_Fecha_Peritaje.Text = "";
            Txt_Perito.Text = "";
            Txt_Valor_Peritaje.Text = "";
            Txt_Observaciones.Text = "";

            Cmb_Tipo_de_bien.SelectedIndex = 0;
            Txt_Valor_Bien.Text = "";
            Txt_Descripcion.Text = "";
            Txt_Fotografias.Text = "";

            RBtn_Lugar.SelectedIndex = -1;
            Txt_Lugar.Text = "";
            Txt_Costo.Text = "";
            Txt_Dimensiones.Text = "";
            Txt_Fecha_Ingreso.Text = "";
            Txt_Tiempo_transcurrido.Text = "";
            Txt_Costo_Almacenamiento.Text = "";
            Grid_Bienes.DataBind();

            // cargar si hay un No_Peritaje
            if (Cmb_Avaluo.SelectedValue != "0")
            {
                Btn_Nuevo.Visible = false;
                Btn_Modificar.Visible = true;
                Cargar_Detalles_Peritaje();
            }
            else
            {
                Btn_Nuevo.Visible = true;
                Btn_Modificar.Visible = false;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    #endregion EVENTOS

    #region GRIDS

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Bienes_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de índice seleccionado en el grid Bienes
    ///             Eliminar la fila seleccionada de la tabla Bienes en la variable de sesión
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Bienes_SelectedIndexChanged(object sender, EventArgs e)
    {
        int No_Bien;
        DataTable Dt_Bienes = (DataTable)Session["Bienes"];
        // si el registro a eliminar tiene un número de Bien, agregar el número a la lista de bienes a eliminar
        if (int.TryParse(Dt_Bienes.Rows[Grid_Bienes.SelectedIndex][Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString(), out No_Bien) && No_Bien > 0)
        {
            List<string> Lst_Bienes_Eliminar;
            if (Session["Lista_Bienes_Eliminar"] == null)
            {
                Lst_Bienes_Eliminar = new List<string>();
            }
            else
            {
                Lst_Bienes_Eliminar = (List<string>)Session["Lista_Bienes_Eliminar"];
            }
            // agregar a la lista de bienes a eliminar
            Lst_Bienes_Eliminar.Add(Dt_Bienes.Rows[Grid_Bienes.SelectedIndex][Ope_Pre_Pae_Bienes.Campo_No_Bien].ToString());
            Session["Lista_Bienes_Eliminar"] = Lst_Bienes_Eliminar;
        }

        // eliminar de la tabla el registro seleccionado
        Dt_Bienes.Rows.RemoveAt(Grid_Bienes.SelectedIndex);
        Grid_Bienes.DataSource = Dt_Bienes;
        Grid_Bienes.DataBind();
        Session["Bienes"] = Dt_Bienes;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Bienes_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de página del grid Bienes
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Bienes_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Limpia_Mensaje_Error();
        try
        {
            if (Session["Bienes"] != null)
            {
                DataTable Dt_Bienes = (DataTable)Session["Bienes"];
                Grid_Bienes.PageIndex = e.NewPageIndex;
                Grid_Bienes.DataSource = Dt_Bienes;
                Grid_Bienes.DataBind();
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Embargos_Generados_PageIndexChanging
    /// DESCRIPCIÓN: Manejo del evento cambio de página del grid Embargos_Generados
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Embargos_Generados_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Limpia_Mensaje_Error();
        try
        {
            if (Session["Grid_Embargos_Generados"] != null)
            {
                DataTable Dt_Embargos_Generados = (DataTable)Session["Grid_Embargos_Generados"];
                Grid_Embargos_Generados.PageIndex = e.NewPageIndex;
                Grid_Embargos_Generados.Columns[8].Visible = true;
                Grid_Embargos_Generados.Columns[9].Visible = true;
                Grid_Embargos_Generados.DataSource = Dt_Embargos_Generados;
                Grid_Embargos_Generados.DataBind();
                Grid_Embargos_Generados.Columns[8].Visible = false;
                Grid_Embargos_Generados.Columns[9].Visible = false;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Embargos_Generados_SelectedIndexChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de índice seleccionado en el grid
    ///             consultar información del embargo seleccionado y mostrar controles
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************   
    protected void Grid_Embargos_Generados_SelectedIndexChanged(object sender, EventArgs e)
    {
        DataTable Dt_Depositario;
        DataTable Dt_Peritajes;
        Cls_Ope_Pre_Pae_Bienes_Negocio Consulta_Depositarios_Peritajes = new Cls_Ope_Pre_Pae_Bienes_Negocio();

        Limpia_Mensaje_Error();

        try
        {
            Limpiar_Detalles_Peritaje();
            // validar que haya una fila seleccionada
            if (Grid_Embargos_Generados.SelectedIndex > -1)
            {
                // depositarios
                Consulta_Depositarios_Peritajes.P_No_Detalle_Etapa = Grid_Embargos_Generados.SelectedRow.Cells[9].Text;
                Dt_Depositario = Consulta_Depositarios_Peritajes.Consulta_Depositarios();
                Grid_Depositario.DataSource = Dt_Depositario;
                Grid_Depositario.DataBind();

                Div_Depositarios.Visible = true;
                Div_Detalles.Visible = true;

                // peritajes, con ordenamiento por ejecutor, contribuyente y tercero
                Consulta_Depositarios_Peritajes.P_Ordenar_Dinamico = "CASE " + Ope_Pre_Pae_Peritajes.Campo_Avaluo + " WHEN 'EJECUTOR' THEN 1 WHEN 'CONTRIBUYENTE' THEN 2 WHEN 'TERCERO' THEN 3 END";
                Dt_Peritajes = Consulta_Depositarios_Peritajes.Consulta_Peritajes();
                // si hay peritajes, cargar en el combo, si no, agregar al combo el peritaje EJECUTOR
                if (Dt_Peritajes != null && Dt_Peritajes.Rows.Count > 0)
                {
                    Cargar_Datos_Peritajes(Dt_Peritajes);
                }
                else
                {
                    Cmb_Avaluo.Items.Insert(0, new ListItem("EJECUTOR", "0"));
                    // mostrar botón para insertar nuevo registro
                    Btn_Nuevo.Visible = true;
                    Lugar_Almacenamiento(false);
                    RBtn_Lugar.SelectedIndex = 0;
                }

                // configurar botones si hay un No_Peritaje
                if (Cmb_Avaluo.SelectedIndex > -1 && Cmb_Avaluo.SelectedValue != "0")
                {
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                }
                else
                {
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(ex.Message.ToString());
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Grid_Bienes_RowDataBound
    /// DESCRIPCIÓN: Habilitar o deshabilitar el botón si se está editando y si no, sólo mostrar los enlaces a los archivos
    /// PARÁMETROS: NO APLICA
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Grid_Bienes_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Limpia_Mensaje_Error();

        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // si se está dando de alta o modificando un peritaje, habilitar la celda con el botón eliminar bien
                if (Btn_Nuevo.ToolTip == "Dar de Alta" || Btn_Modificar.ToolTip == "Actualizar")
                {
                    e.Row.Cells[4].Enabled = true;
                }
                else
                {
                    e.Row.Cells[4].Enabled = false;
                    // separar por coma los nombres de archivo de fotografías y sustituir por enlaces
                    string[] Arr_Nombres_Archivos = e.Row.Cells[3].Text.Split(',');
                    e.Row.Cells[3].Text = "";
                    e.Row.Cells[3].Controls.Clear();
                    for (int i = 0; i < Arr_Nombres_Archivos.Length; i++)
                    {
                        // si el archivo existe, generar como enlace y agregar al campo
                        if (Arr_Nombres_Archivos[i].Length > 0 && File.Exists(Server.MapPath(Arr_Nombres_Archivos[i])))
                        {
                            // crear enlace para archivo
                            HyperLink Hlk_Enlace = new HyperLink();
                            Hlk_Enlace.Text = Path.GetFileName(Arr_Nombres_Archivos[i]);
                            Hlk_Enlace.NavigateUrl = Arr_Nombres_Archivos[i].Replace("\\", "/");
                            Hlk_Enlace.CssClass = "enlace_fotografia";
                            Hlk_Enlace.Target = "blank";
                            e.Row.Cells[3].Controls.Add(Hlk_Enlace);
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error("Grid_Bienes_RowDataBound: " + ex.Message.ToString());
        }
    }

    #endregion GRIDS

}
