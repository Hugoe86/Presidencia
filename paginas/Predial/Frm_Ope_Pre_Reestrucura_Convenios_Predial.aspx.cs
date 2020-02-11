using System;
using System.Web;
using System.Collections.Generic;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using Presidencia.Sessiones;
using Presidencia.Constantes;
using Presidencia.Operacion_Predial_Convenios_Predial.Negocio;
using Presidencia.Catalogo_Cuentas_Predial.Negocio;
using Presidencia.Operacion_Predial_Dias_Inhabiles.Negocio;
using Presidencia.Operacion_Calculo_Impuesto_Traslado.Negocio;
using Presidencia.Catalogo_Claves_Ingreso.Negocio;
using Presidencia.Catalogo_Contribuyentes.Negocio;
using Presidencia.Numalet;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Data.OracleClient;
using Presidencia.Operacion_Predial_Generar_Adeudo_Predial.Negocio;
using Presidencia.Descuentos_Predial.Negocio;
using System.IO;
using System.Linq;
using DocumentFormat.OpenXml.Packaging;
using openXML_Wp = DocumentFormat.OpenXml.Wordprocessing;
using Presidencia.Operacion_Predial_Parametros.Negocio;
using System.Diagnostics;
using Presidencia.Predial_Pae_Honorarios.Negocio;

public partial class paginas_Predial_Frm_Ope_Pre_Reestrucura_Convenios_Predial : System.Web.UI.Page
{
    private bool _Grid_Parcialidades_Editable = false;
    protected bool Grid_Parcialidades_Editable
    {
        get { return this._Grid_Parcialidades_Editable; }
        set { this._Grid_Parcialidades_Editable = value; }
    }

    private bool _Grid_Parcialidades_Manuales = false;
    protected bool Grid_Parcialidades_Manuales
    {
        get { return this._Grid_Parcialidades_Manuales; }
        set { this._Grid_Parcialidades_Manuales = value; }
    }

    private enum Orden_Datos
    {
        Ascendente,
        Descendente
    }

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: 
    ///FECHA_CREO: 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************        
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Etiqueta_Body_Master_Page.Attributes.Add("onkeydown", "cancelBack()");
        ScriptManager.RegisterPostBackControl(Btn_Enviar_Archivo);
        Page.Form.Attributes.Add("enctype", "multipart/form-data");

        Session["Activa"] = true;//Variable para mantener la session activa.

        if (!IsPostBack)
        {
            Inicializa_Controles();

            Session["ESTATUS_CUENTAS"] = "IN ('ACTIVA','VIGENTE','BLOQUEADA','PENDIENTE')";
            Session["TIPO_CONTRIBUYENTE"] = "IN ('PROPIETARIO', 'POSEEDOR')";

            //Scrip para mostrar Ventana Modal de las Tasas de Traslado
            string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergentes/Frm_Busqueda_Avanzada_Cuentas_Predial.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Attributes.Add("onclick", Ventana_Modal);
        }
        else
        {
            // validar en postback estatus del solicitante (se activan y desactivan del lado del cliente tambien)
            if (Cmb_Tipo_Solicitante.SelectedValue == "DEUDOR SOLIDARIO" && (Btn_Nuevo.ToolTip == "Dar de Alta" || Btn_Modificar.ToolTip == "Actualizar"))
            {
                Txt_Solicitante.Enabled = true;
                Txt_RFC.Enabled = true;
            }
            else
            {
                Txt_Solicitante.Enabled = false;
                Txt_RFC.Enabled = false;
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Ventana_Emergente_Desglose_Adeudos
    ///DESCRIPCIÓN          : Establece el evento onclik del control para abrir la ventana emergente del Desgloce de Adedudos con la ruta y parámetros necesarios
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 21/Otubre/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Ventana_Emergente_Desglose_Adeudos()
    {
        String Ventana_Modal = "Abrir_Ventana_Emergente('Ventanas_Emergentes/Resumen_Predial/Frm_Desglose_Adeudos.aspx?Cuenta_Predial_ID=" + Hdf_Cuenta_Predial_ID.Value + "&Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'";
        String Propiedades = ", 'toolbar=0,location=0,status=0,menubar=0,scrollbars=0,resizable=0,width=680,height=400,left=200,top=100');";
        Btn_Desglose_Adeudos.Attributes.Add("onclick", Ventana_Modal + Propiedades);
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
        String Propiedades = ",'height=600,width=800,scrollbars=1');";
        Btn_Detalles_Cuenta_Predial.Attributes.Add("onclick", Ventana_Modal + "?Cuenta_Predial=" + Txt_Cuenta_Predial.Text.Trim() + "'" + Propiedades);
    }

    #endregion Page_Load

    #region Metodos

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda realizar
    ///               diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 29-ago-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializa_Controles()
    {
        try
        {
            Habilitar_Controles("Inicial"); //Habilita los controles de la forma para que el usuario pueda indica que operación desea realizar
            Limpiar_Controles(); //Limpia los controles del forma
            //Cargar_Grid_Convenios(0);
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
    /// FECHA_CREO: 09-ago-2010
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado; ///Indica si el control de la forma va a ser habilitado para que los edite el usuario

        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Modificar";
                    Btn_Nuevo.Visible = true;
                    Btn_Modificar.Visible = false;
                    Btn_Imprimir.Visible = false;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Modificar.CausesValidation = false;
                    Btn_Detalles_Cuenta_Predial.Enabled = false;
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
                    Btn_Imprimir.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    break;

                case "Modificar":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Modificar.ToolTip = "Actualizar";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Cmb_Estatus.Enabled = true;
                    Btn_Nuevo.Visible = false;
                    Btn_Modificar.Visible = true;
                    Btn_Imprimir.Visible = false;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Modificar.CausesValidation = true;
                    break;
            }

            Panel_Datos.Visible = Habilitado;
            Grid_Convenios.Visible = !Habilitado;
            ///Habilitar campos de texto para edición o inhabilitar si no se requieren de acuerdo con variable Habilitado
            Cmb_Tipo_Solicitante.Enabled = Habilitado;
            Txt_Solicitante.Enabled = Cmb_Tipo_Solicitante.SelectedValue == "DEUDOR SOLIDARIO" ? true : false;
            Txt_RFC.Enabled = Cmb_Tipo_Solicitante.SelectedValue == "DEUDOR SOLIDARIO" ? true : false;
            Cmb_Periodicidad_Pago.Enabled = Habilitado;
            Cmb_Estatus.Enabled = Habilitado;
            Txt_Numero_Parcialidades.Enabled = Habilitado;
            Txt_Observaciones.Enabled = Habilitado;
            Txt_Descuento_Recargos_Ordinarios.Enabled = false;
            Txt_Descuento_Recargos_Moratorios.Enabled = false;
            Txt_Porcentaje_Anticipo.Enabled = Habilitado;
            Txt_Total_Anticipo.Enabled = Habilitado;
            // no editables
            Cmb_Hasta_Anio_Periodo.Enabled = Habilitado;
            Cmb_Hasta_Bimestre_Periodo.Enabled = Habilitado;
            Txt_Cuenta_Predial.Enabled = false;
            Txt_Propietario.Enabled = false;
            Txt_Colonia.Enabled = false;
            Txt_Calle.Enabled = false;
            Txt_No_Exterior.Enabled = false;
            Txt_No_Interior.Enabled = false;
            Txt_Monto_Total_Adeudo.Enabled = false;
            Txt_Adeudo_Corriente.Enabled = false;
            Txt_Adeudo_Rezago.Enabled = false;
            Txt_Numero_Convenio.Enabled = false;
            Txt_Realizo.Enabled = false;
            Txt_Fecha_Vencimiento.Enabled = false;
            Txt_Total_Adeudo.Enabled = false;
            Txt_Total_Descuento.Enabled = false;
            Txt_Sub_Total.Enabled = false;
            Txt_Total_Convenio.Enabled = false;

            Txt_Monto_Moratorios.Enabled = Habilitado;
            Txt_Monto_Moratorios.ReadOnly = false;
            Txt_Monto_Recargos.Enabled = Habilitado;
            Txt_Monto_Recargos.ReadOnly = false;
            Txt_Adeudo_Honorarios.Enabled = Habilitado;
            Txt_Adeudo_Honorarios.ReadOnly = false;
            Txt_Fecha_Convenio.Enabled = Habilitado;
            Txt_Fecha_Convenio.ReadOnly = false;
            Chk_Parcialidades_Manuales.Enabled = Habilitado;

            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Enabled = false;
            Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial.Visible = false;
            Btn_Convenio_Escaneado.Enabled = !Habilitado;
            Btn_Desglose_Adeudos.Enabled = Habilitado;
            Btn_Detalles_Cuenta_Predial.Enabled = Habilitado;

            Txt_Busqueda.Enabled = !Habilitado;
            Btn_Buscar.Enabled = !Habilitado;
            Grid_Parcialidades.Enabled = true;
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            // establecer propiedades de los controles plantilla en el grid
            Grid_Parcialidades_Editable = Habilitado;
        }
        catch (Exception ex)
        {
            throw new Exception("Habilitar_Controles " + ex.Message.ToString(), ex);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Limpiar_Controles
    ///DESCRIPCIÓN          : Limpia los controles del Formulario
    ///PARAMETROS           :     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        Hdf_Cuenta_Predial_ID.Value = "";
        Hdf_Propietario_ID.Value = "";
        Hdf_No_Convenio.Value = "";
        Hdf_Propietario_ID.Value = "";
        Hdf_RFC_Propietario.Value = "";
        Hdf_RFC_Solicitante.Value = "";
        Hdf_Solicitante.Value = "";
        Hdn_Monto_Impuesto.Value = "";
        //Datos Cuenta
        Txt_Cuenta_Predial.Text = "";
        Txt_Monto_Total_Adeudo.Text = "";
        Txt_Adeudo_Corriente.Text = "";
        Txt_Adeudo_Rezago.Text = "";
        Txt_Adeudo_Honorarios.Text = "";
        Txt_Monto_Moratorios.Text = "";
        Txt_Monto_Recargos.Text = "";
        Txt_Calle.Text = "";
        Txt_Colonia.Text = "";
        Txt_No_Exterior.Text = "";
        Txt_No_Interior.Text = "";
        Txt_Propietario.Text = "";
        //Convenio
        Txt_Numero_Convenio.Text = "";
        Cmb_Estatus.SelectedIndex = 0;
        Txt_Solicitante.Text = "";
        Txt_RFC.Text = "";
        Cmb_Tipo_Solicitante.SelectedIndex = 0;
        Txt_Numero_Parcialidades.Text = "";
        Cmb_Periodicidad_Pago.SelectedIndex = 0;
        Txt_Realizo.Text = "";
        Txt_Fecha_Convenio.Text = "";
        Txt_Fecha_Vencimiento.Text = "";
        Txt_Observaciones.Text = "";
        //Descuentos
        Txt_Descuento_Recargos_Ordinarios.Text = "";
        Txt_Descuento_Recargos_Moratorios.Text = "";
        Txt_Total_Adeudo.Text = "";
        Txt_Total_Descuento.Text = "";
        Txt_Sub_Total.Text = "";
        //Parcialidades
        Txt_Porcentaje_Anticipo.Text = "";
        Txt_Total_Anticipo.Text = "";
        Txt_Total_Convenio.Text = "";
        Grid_Convenios.DataSource = null;
        Grid_Convenios.DataBind();
        Grid_Parcialidades.DataSource = null;
        Grid_Parcialidades.DataBind();
        Cmb_Hasta_Anio_Periodo.Items.Clear();
        Cmb_Busqueda_General.SelectedIndex = 0;

        // ocultar contenedor subir convenio escaneado
        Contenedor_Subir_Convenio.Style.Clear();
        Contenedor_Subir_Convenio.Style.Add("display", "none");

        // limpiar descuentos
        Hdn_No_Descuento.Value = "";

        Session.Remove("Cuenta_Predial");
        Session.Remove("Tabla_Adeudos");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("Dt_Convenios_Predial");
        Session.Remove("Dic_Adeudos");
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Grid_Convenios
    ///DESCRIPCIÓN          : Llena el grid de Convenios con los registros encontrados
    ///PARAMETROS           : 1. Pagina.  Pagina en la cual se mostrará el Grid_VIew
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Grid_Convenios(Int32 Pagina)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;
        DateTime Fecha_Filtro = DateTime.Now.AddMonths(-1);
        var tiempo = new Stopwatch();

        try
        {
            Cls_Ope_Pre_Convenios_Predial_Negocio Convenios_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            DataTable Dt_Convenios_Predial;

            Convenios_Predial.P_Campos_Foraneos = true;
            // agregar filtro para solo traer los convenios desde el mes anterior (se sobreescribe para busquedas)
            if (Cmb_Busqueda_General.SelectedIndex != 0)
            {
                //if (Txt_Busqueda.Text.Trim() == "")
                //{
                //    Convenios_Predial.P_Filtros_Dinamicos = Ope_Pre_Convenios_Predial.Campo_Fecha + " > '1-" + Fecha_Filtro.Month + "-" + Fecha_Filtro.Year + "'";
                //}
                //else 
                //{
                Convenios_Predial.P_Filtros_Dinamicos = "";
                //}
                if (Cmb_Busqueda_General.SelectedIndex == 2)
                {
                    Convenios_Predial.P_Filtros_Dinamicos += "" + Ope_Pre_Convenios_Predial.Campo_Estatus + " IN ('INCUMPLIDO','ACTIVO')";
                }
                else if (Cmb_Busqueda_General.SelectedIndex == 3)
                {
                    Convenios_Predial.P_Filtros_Dinamicos += "" + Ope_Pre_Convenios_Predial.Campo_Estatus + " IN ('CANCELADO','INCUMPLIDO')";
                }
                else
                {
                    Convenios_Predial.P_Filtros_Dinamicos = Ope_Pre_Convenios_Predial.Campo_Estatus + " !='CUENTA_CANCELADA'";
                }
            }
            // si es una busqueda, formar filtro por cuenta o numero de convenio
            if (Txt_Busqueda.Text.Trim() != "")
            {
                Convenios_Predial.P_Filtros_Dinamicos = "(";
                Convenios_Predial.P_Filtros_Dinamicos += Ope_Pre_Convenios_Predial.Campo_No_Convenio
                    + " LIKE '%" + Txt_Busqueda.Text.Trim() + "%'";
                Convenios_Predial.P_Filtros_Dinamicos += " OR " + Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id
                    + " IN (SELECT " + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + " FROM "
                    + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + " WHERE "
                    + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial + " LIKE '%"
                    + Txt_Busqueda.Text.Trim().ToUpper() + "%')";
                Convenios_Predial.P_Filtros_Dinamicos += " OR 'CPRE' || TO_NUMBER("
                    + Ope_Pre_Convenios_Predial.Campo_No_Convenio + ") LIKE '"
                    + Txt_Busqueda.Text.Trim().ToUpper() + "')";
                // omitir convenios de cuentas canceladas
                Convenios_Predial.P_Filtros_Dinamicos += " AND " + Ope_Pre_Convenios_Predial.Campo_Estatus + " !='CUENTA_CANCELADA'";
            }
            // omitir convenios de cuentas canceladas, si no hay filtros por estatus
            if (string.IsNullOrEmpty(Convenios_Predial.P_Filtros_Dinamicos))
            {
                Convenios_Predial.P_Filtros_Dinamicos = Ope_Pre_Convenios_Predial.Campo_Estatus + " !='CUENTA_CANCELADA'";
            }

            // especificar que valide el estatus del convenio
            Convenios_Predial.P_Validar_Convenios_Cumplidos = true;
            Dt_Convenios_Predial = Convenios_Predial.Consultar_Convenio_Predial();

            // almacenar tabla en variable de sesion
            Session["Dt_Consulta_Convenios"] = Dt_Convenios_Predial;

            // si se obtuvieron valores de convenios, cargar en el grid
            if (Dt_Convenios_Predial != null)
            {
                Grid_Convenios.Columns[1].Visible = true;
                Grid_Convenios.Columns[2].Visible = true;
                Grid_Convenios.Columns[4].Visible = true;
                Grid_Convenios.Columns[8].Visible = true;
                Grid_Convenios.DataSource = Dt_Convenios_Predial;
                Grid_Convenios.PageIndex = Pagina;
                Grid_Convenios.DataBind();
                Grid_Convenios.Columns[1].Visible = false;
                Grid_Convenios.Columns[2].Visible = false;
                Grid_Convenios.Columns[4].Visible = false;
                Grid_Convenios.Columns[8].Visible = false;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
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
    private Int32 Obtener_Dato_Consulta(ref OracleCommand Cmmd, String Campo, String Tabla, String Condiciones)
    {
        String Mi_SQL;
        Int32 Dato_Consulta = 0;

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

            //OracleDataReader Dr_Dato = OracleHelper.ExecuteReader(Cls_Constantes.Str_Conexion, CommandType.Text, Mi_SQL);
            Cmmd.CommandText = Mi_SQL;
            Dato_Consulta = Convert.ToInt32(Cmmd.ExecuteOracleScalar().ToString());
            if (Convert.IsDBNull(Dato_Consulta))
            {
                Dato_Consulta = 1;
            }
            else
            {
                Dato_Consulta = Dato_Consulta + 1;
            }
        }
        catch (OracleException Ex)
        {
            //Indicamos el mensaje 
            throw new Exception(Ex.ToString());
        }
        return Dato_Consulta;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Parcialidades
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para las parcialidades
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 01-sep-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Parcialidades()
    {
        DataTable Dt_Parcialidades = new DataTable();
        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(String)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_HONORARIOS", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
        Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
        Dt_Parcialidades.Columns.Add(new DataColumn("PERIODO", typeof(String)));

        return Dt_Parcialidades;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Tabla_Desglose_Parcialidades
    ///DESCRIPCIÓN          : Devuelve un DataTable con la estructura para el detalle de las parcialidades
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 26-oct-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected DataTable Crear_Tabla_Desglose_Parcialidades()
    {
        DataTable Dt_Parcialidades = new DataTable();
        Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
        Dt_Parcialidades.Columns.Add(new DataColumn("ANIO", typeof(Int32)));
        Dt_Parcialidades.Columns.Add(new DataColumn("BIMESTRE_1", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("BIMESTRE_2", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("BIMESTRE_3", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("BIMESTRE_4", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("BIMESTRE_5", typeof(Decimal)));
        Dt_Parcialidades.Columns.Add(new DataColumn("BIMESTRE_6", typeof(Decimal)));

        return Dt_Parcialidades;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Recuperar_Datos_Tabla_Parcialidades
    ///DESCRIPCIÓN          : Lee el grid de las parcialidades y devuelve una instancia como DataTable
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 08/Agosto/2011
    ///MODIFICO             : Roberto González Oseguera
    ///FECHA_MODIFICO       : 26-oct-2011
    ///CAUSA_MODIFICACIÓN   : Agregar desglose de cada parcialidad
    ///*******************************************************************************
    protected DataTable Recuperar_Datos_Tabla_Parcialidades(out DataTable Dt_Desglose_Parcialidades)
    {
        Dictionary<String, Decimal> Dic_Adeudos;
        DataTable Dt_Parcialidades = Crear_Tabla_Parcialidades();
        string Periodo;
        Label Etiqueta;

        Dic_Adeudos = Consulta_Adeudos_Diccionario();
        Dt_Desglose_Parcialidades = Crear_Tabla_Desglose_Parcialidades();

        DataRow Dr_Parcialidades;
        //Se barre el Grid para cargar el DataTable con los valores del grid
        foreach (GridViewRow Row in Grid_Parcialidades.Rows)
        {
            Periodo = "-";
            Dr_Parcialidades = Dt_Parcialidades.NewRow();
            Dr_Parcialidades["NO_PAGO"] = Row.Cells[0].Text;

            Etiqueta = (Label)Row.Cells[2].FindControl("Lbl_Txt_Grid_Monto_Honorarios");
            if (Etiqueta != null)
                Dr_Parcialidades["MONTO_HONORARIOS"] = Convert.ToDecimal(Etiqueta.Text.Replace("$", ""));

            Etiqueta = (Label)Row.Cells[3].FindControl("Lbl_Txt_Grid_Monto_Recargos_Ordinarios");
            if (Etiqueta != null)
                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Convert.ToDecimal(Etiqueta.Text.Replace("$", ""));

            Etiqueta = (Label)Row.Cells[4].FindControl("Lbl_Txt_Grid_Monto_Recargos_Moratorios");
            if (Etiqueta != null)
                Dr_Parcialidades["RECARGOS_MORATORIOS"] = Convert.ToDecimal(Etiqueta.Text.Replace("$", ""));

            Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Row.Cells[5].Text.Replace("$", ""));
            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Convert.ToDateTime(Row.Cells[7].Text);
            Dr_Parcialidades["ESTATUS"] = Row.Cells[8].Text;

            Etiqueta = (Label)Row.Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
            if (Etiqueta != null)
                Periodo = HttpUtility.HtmlDecode(Etiqueta.Text);

            Dr_Parcialidades["PERIODO"] = Periodo;
            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);

            // si hay un periodo, incluir desglose
            if (Periodo.Length >= 13)
            {
                Dt_Desglose_Parcialidades = Generar_Desglose_Parcialidad(
                    Dt_Desglose_Parcialidades,
                    Periodo,
                    Row.Cells[0].Text,
                    Dic_Adeudos
                    );
            }
        }
        return Dt_Parcialidades;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Generar_Desglose_Parcialidad
    /// DESCRIPCIÓN: Genera el desglose de adeudos en la tabla que recibe y la regresa modificada
    /// PARÁMETROS:
    /// 		1. Dt_Desglose_Parcialidades: Datatable con desglose de parcialidades
    /// 		2. Periodo: Bimestres que se incluyen en el adeudo
    /// 		3. Parcialidad: Numero de parcialidad del convenios
    /// 		4. Dic_Adeudos: Diccionario con los adeudos de la cuenta
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-oct-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected DataTable Generar_Desglose_Parcialidad(DataTable Dt_Desglose_Parcialidades, String Periodo, String Parcialidad, Dictionary<String, Decimal> Dic_Adeudos)
    {
        Int32 Desde_Bimestre = 0;
        Int32 Hasta_Bimestre = 0;
        Int32 Desde_Anio = 0;
        Int32 Hasta_Anio = 0;

        // validar parametros antes de leer periodo
        if (Periodo.Length >= 13 && Dt_Desglose_Parcialidades != null && Parcialidad.Length > 0)
        {
            // leer periodos
            Int32.TryParse(Periodo.Substring(0, 1), out Desde_Bimestre);
            Int32.TryParse(Periodo.Substring(2, 4), out Desde_Anio);
            Int32.TryParse(Periodo.Substring(7, 1), out Hasta_Bimestre);
            Int32.TryParse(Periodo.Substring(9, 4), out Hasta_Anio);

            // recorrer años del periodo para insertar adeudos
            for (Int32 Anio = Desde_Anio; Anio <= Hasta_Anio; Anio++)
            {
                // agregar una nueva fila
                DataRow Nueva_Fila = Dt_Desglose_Parcialidades.NewRow();
                // agregar año y numero de pago
                Nueva_Fila["ANIO"] = Anio;
                Nueva_Fila["NO_PAGO"] = Convert.ToInt32(Parcialidad);

                // si el periodo consiste de un solo año
                if (Desde_Anio == Hasta_Anio)
                {
                    // recorrer los bimestres desde y hasta
                    for (int Bimestre = Desde_Bimestre; Bimestre <= Hasta_Bimestre; Bimestre++)
                    {
                        // si hay adeudo en el diccionario, asignar a la tabla, si no, asignar 0
                        if (Dic_Adeudos.ContainsKey(Bimestre.ToString() + Anio.ToString()))
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = Dic_Adeudos[Bimestre.ToString() + Anio.ToString()];
                        }
                        else
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = 0;
                        }
                    }
                }// si el contador de años es igual al año inicial, contar bimestres hasta 6
                else if (Anio == Desde_Anio)
                {
                    // recorrer los bimestres desde_bimestre hasta 6
                    for (int Bimestre = Desde_Bimestre; Bimestre <= 6; Bimestre++)
                    {
                        // si hay adeudo en el diccionario, asignar a la tabla, si no, asignar 0
                        if (Dic_Adeudos.ContainsKey(Bimestre.ToString() + Anio.ToString()))
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = Dic_Adeudos[Bimestre.ToString() + Anio.ToString()];
                        }
                        else
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = 0;
                        }
                    }
                } // si el contador de años es igual al año final, contar hasta_bimestre 
                else if (Anio == Hasta_Anio)
                {
                    // recorrer los bimestres desde 1 hasta_bimestre
                    for (int Bimestre = 1; Bimestre <= Hasta_Bimestre; Bimestre++)
                    {
                        // si hay adeudo en el diccionario, asignar a la tabla, si no, asignar 0
                        if (Dic_Adeudos.ContainsKey(Bimestre.ToString() + Anio.ToString()))
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = Dic_Adeudos[Bimestre.ToString() + Anio.ToString()];
                        }
                        else
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = 0;
                        }
                    }
                } // es un año entre hasta y desde año, recorrer bimestres del 1 al 6
                else
                {
                    // recorrer los bimestres del 1 al 6
                    for (int Bimestre = 1; Bimestre <= 6; Bimestre++)
                    {
                        // si hay adeudo en el diccionario, asignar a la tabla, si no, asignar 0
                        if (Dic_Adeudos.ContainsKey(Bimestre.ToString() + Anio.ToString()))
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = Dic_Adeudos[Bimestre.ToString() + Anio.ToString()];
                        }
                        else
                        {
                            Nueva_Fila["BIMESTRE_" + Bimestre.ToString()] = 0;
                        }
                    }
                }
                // agregar la nueva fila a la tabla 
                Dt_Desglose_Parcialidades.Rows.Add(Nueva_Fila);
            }
        }

        return Dt_Desglose_Parcialidades;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Convenio
    ///DESCRIPCIÓN          : Conasulta los datos del convenio seleccionado y los 
    ///                     muestra en los controles correspondientes
    ///PARAMETROS
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 27-ago-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Convenio()
    {
        Cls_Ope_Pre_Convenios_Predial_Negocio Convenio = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        var Consulta_Contribuyente = new Cls_Cat_Pre_Contribuyentes_Negocio();

        DataTable Dt_Convenios_Predial;
        String Periodo;

        if (Grid_Convenios.SelectedRow.Cells[8].Text.Replace("&nbsp;", "").Equals(""))
        {
            Convenio.P_Reestructura = false;
        }
        else
        {
            Convenio.P_Reestructura = true;
        }


        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        Chk_Parcialidades_Manuales.Checked = false;

        try
        {

            Convenio.P_Campos_Foraneos = true;
            Convenio.P_No_Convenio = Hdf_No_Convenio.Value;
            // especificar que valide el estatus del convenio
            Convenio.P_Validar_Convenios_Cumplidos = true;
            // si hay un convenio seleccionado en el grid validar ssu estatus
            if (Grid_Convenios.SelectedIndex > -1)
            {
                // si el estatus del convenio seleccionado es TERMINADO, no validar parcialidades
                if (Grid_Convenios.SelectedRow.Cells[6].Text.Trim() == "TERMINADO")
                {
                    Convenio.P_Validar_Convenios_Cumplidos = false;
                }
            }
            Dt_Convenios_Predial = Convenio.Consultar_Convenio_Predial();

            if (Dt_Convenios_Predial != null)
            {
                if (Dt_Convenios_Predial.Rows.Count > 0)
                {
                    foreach (DataRow Row in Dt_Convenios_Predial.Rows)
                    {
                        Hdf_Cuenta_Predial_ID.Value = Row[Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id].ToString();
                        Session["Cuenta_Predial_ID"] = Hdf_Cuenta_Predial_ID.Value;
                        Txt_Cuenta_Predial.Text = Row["Cuenta_Predial"].ToString();
                        //Txt_Clasificacion.Text = Row["Tipo_Predio"].ToString();
                        Consultar_Datos_Cuenta_Predial();
                        Txt_Numero_Convenio.Text = Row[Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                        Cmb_Estatus.SelectedValue = Row[Ope_Pre_Convenios_Predial.Campo_Estatus].ToString();
                        Txt_Propietario.Text = Row["Nombre_Propietario"].ToString();
                        Hdf_Propietario_ID.Value = Row[Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id].ToString();
                        // consultar datos contribuyente
                        Consulta_Contribuyente.P_Contribuyente_ID = Hdf_Propietario_ID.Value;
                        Consulta_Contribuyente = Consulta_Contribuyente.Consultar_Datos_Contribuyente();
                        Hdf_RFC_Propietario.Value = Consulta_Contribuyente.P_RFC;
                        if (Row[Ope_Pre_Convenios_Predial.Campo_Solicitante].ToString() != "")
                        {
                            Txt_Solicitante.Text = Row[Ope_Pre_Convenios_Predial.Campo_Solicitante].ToString();
                            Txt_RFC.Text = Row[Ope_Pre_Convenios_Predial.Campo_RFC].ToString();
                            Cmb_Tipo_Solicitante.SelectedIndex = 1;
                        }
                        else
                        {
                            Txt_Solicitante.Text = Row["Nombre_Propietario"].ToString();
                            Cmb_Tipo_Solicitante.SelectedIndex = 0;
                        }
                        Txt_Numero_Parcialidades.Text = Row[Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades].ToString();
                        Cmb_Periodicidad_Pago.SelectedValue = Row[Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago].ToString();
                        Txt_Realizo.Text = Row["Nombre_Realizo"].ToString();
                        Periodo = Row[Ope_Pre_Convenios_Predial.Campo_Hasta_Periodo].ToString();
                        // si no se encontro el periodo en el encabezado de los datos del convenio, tomar de la ultima parcialidad
                        if (String.IsNullOrEmpty(Periodo) && Convenio.P_Dt_Parcialidades != null && Convenio.P_Dt_Parcialidades.Rows.Count > 0)
                        {
                            String Periodo_Ultima_Parcialidad;
                            String[] Periodos_Separados;
                            Periodo_Ultima_Parcialidad = Convenio.P_Dt_Parcialidades.Rows[Convenio.P_Dt_Parcialidades.Rows.Count - 1][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString().Trim();
                            Periodos_Separados = Periodo_Ultima_Parcialidad.Split('-');
                            // despues de separado el 
                            if (Periodos_Separados.Length >= 2)
                            {
                                Periodo = Periodos_Separados[1].Replace("/", "");
                            }
                        }

                        // prevenir error si no se agrego el valor a seleccionar (la cuenta ya no tiene adeudos o no se guardo el convenio con Hasta_Periodo), agregar dato al combo
                        // si se obtuvo un valor para el periodo
                        if (!String.IsNullOrEmpty(Periodo))
                        {
                            if (Cmb_Hasta_Anio_Periodo.Items.FindByText(Periodo.Substring(1, 4)) == null)
                            {
                                System.Web.UI.WebControls.ListItem Anio_Adeudo = new System.Web.UI.WebControls.ListItem(Periodo.Substring(1, 4), Periodo.Substring(1, 4));
                                Cmb_Hasta_Anio_Periodo.Items.Add(Anio_Adeudo);
                            }
                            Cmb_Hasta_Anio_Periodo.SelectedValue = Periodo.Substring(1, 4);
                            Cmb_Hasta_Bimestre_Periodo.SelectedValue = Periodo.Substring(0, 1);
                        }

                        if (Row[Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual].ToString() == "SI")
                        {
                            Chk_Parcialidades_Manuales.Checked = true;
                        }
                        else
                        {
                            Chk_Parcialidades_Manuales.Checked = false;
                        }

                        Txt_Numero_Reestructura.Text = Row[Ope_Pre_Convenios_Predial.Campo_No_Reestructura].ToString();
                        Txt_Fecha_Convenio.Text = Convert.ToDateTime(Row[Ope_Pre_Convenios_Predial.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
                        Txt_Observaciones.Text = Row[Ope_Pre_Convenios_Predial.Campo_Observaciones].ToString();
                        Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios]).ToString("#,##0.00");
                        Txt_Descuento_Recargos_Moratorios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios]).ToString("#,##0.00");
                        Hdn_No_Descuento.Value = Row[Ope_Pre_Convenios_Predial.Campo_No_Descuento].ToString();
                        Txt_Total_Adeudo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Adeudo]).ToString("#,##0.00");
                        Txt_Monto_Total_Adeudo.Text = Txt_Total_Adeudo.Text;
                        Hdn_Monto_Impuesto.Value = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Predial]).ToString("#,##0.00");
                        Txt_Adeudo_Corriente.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente]).ToString("#,##0.00");
                        Txt_Adeudo_Rezago.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago]).ToString("#,##0.00");
                        Txt_Monto_Recargos.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Recargos]).ToString("#,##0.00");
                        Txt_Monto_Moratorios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Moratorios]).ToString("#,##0.00");
                        Txt_Adeudo_Honorarios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Honorarios]).ToString("#,##0.00");
                        Txt_Total_Descuento.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Descuento]).ToString("#,##0.00");
                        Txt_Sub_Total.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Sub_Total]).ToString("#,##0.00");
                        Txt_Porcentaje_Anticipo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo]).ToString("#,##0.00");
                        Txt_Total_Anticipo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Anticipo]).ToString("#,##0.00");
                        Txt_Total_Convenio.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Convenio]).ToString("#,##0.00");
                    }

                    Grid_Parcialidades.DataSource = Convenio.P_Dt_Parcialidades;
                    Grid_Parcialidades.PageIndex = 0;
                    Grid_Parcialidades.DataBind();
                    Grid_Convenios.SelectedIndex = -1;
                    Btn_Salir.ToolTip = "Inicio";

                    Sumar_Totales_Parcialidades();
                    Obtener_Fecha_Vencimiento();
                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Parcialidades
    ///DESCRIPCIÓN          : Carga las parcialidades del convenio activo
    ///PARAMETROS
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 28-oct-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Cargar_Parcialidades()
    {
        Cls_Ope_Pre_Convenios_Predial_Negocio Convenio = new Cls_Ope_Pre_Convenios_Predial_Negocio();

        try
        {
            // si Parcialidades manuales está seleccionado, desactivar anticipo y caja de texto importe
            if (Chk_Parcialidades_Manuales.Checked == true)
            {
                Grid_Parcialidades_Editable = false;
                Grid_Parcialidades_Manuales = true;
                // desactivar cajas de texto anticipo
                Txt_Porcentaje_Anticipo.Enabled = false;
                Txt_Total_Anticipo.Enabled = false;
            }
            else
            {
                Grid_Parcialidades_Editable = true;
                Grid_Parcialidades_Manuales = false;
            }

            // datos para la busqueda
            Convenio.P_Reestructura = true;
            Convenio.P_Campos_Foraneos = true;
            Convenio.P_No_Convenio = Hdf_No_Convenio.Value;
            // especificar que valide el estatus del convenio
            Convenio.P_Validar_Convenios_Cumplidos = true;
            // consulta
            Convenio.Consultar_Convenio_Predial();
            // si la consulta regreso valores, asignar el datatable al grid
            if (Convenio.P_Dt_Parcialidades != null)
            {
                Grid_Parcialidades.DataSource = Convenio.P_Dt_Parcialidades;
                Grid_Parcialidades.PageIndex = 0;
                Grid_Parcialidades.DataBind();

                Sumar_Totales_Parcialidades();
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Cargar_Parcialidades: " + Ex.Message);
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN:    Calcular_Parcialidades
    ///DESCRIPCIÓN:             Metodo para generar las parcialidades de tal manera que cubran pagos de adeudos completos.
    ///PARAMETROS:     
    ///CREO:                    Miguel Angel Bedolla Moreno
    ///FECHA_CREO:              05/Octubre/2011
    ///MODIFICO:                Roberto González Oseguera
    ///FECHA_MODIFICO           23-oct-2011
    ///CAUSA_MODIFICACIÓN       Se agregaron validaciones para prevenir la omisión de
    ///                         los primeros o últimos bimestres
    ///*******************************************************************************
    private void Calcular_Parcialidades()
    {
        // sólo calcular parcialidades si el checkbox de parcialiades manuales NO está activado
        if (Chk_Parcialidades_Manuales.Checked == false)
        {
            if (Txt_Numero_Parcialidades.Text != "" && Convert.ToInt32(Txt_Numero_Parcialidades.Text) > 0)
            {
                if ((Convert.ToDecimal(Txt_Monto_Recargos.Text) + Convert.ToDecimal(Txt_Monto_Moratorios.Text) + Convert.ToDecimal(Hdn_Monto_Impuesto.Value) + Convert.ToDecimal(Txt_Adeudo_Honorarios.Text)) > 0)
                {
                    if (Btn_Nuevo.ToolTip == "Dar de Alta" || Btn_Modificar.ToolTip == "Actualizar")
                    {
                        Grid_Parcialidades_Editable = true;
                    }
                    //Anio y bimestre tope
                    Int32 Bimestre = Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue);
                    Int32 Bimestre_Aux = 0;
                    Int32 Anio = Convert.ToInt32(Cmb_Hasta_Anio_Periodo.SelectedValue);
                    Int32 Anio_Aux = 0;
                    //Adeudos
                    DataTable Dt_Adeudos = new DataTable();
                    Cls_Ope_Pre_Convenios_Predial_Negocio Adeudos_Cuenta = new Cls_Ope_Pre_Convenios_Predial_Negocio();
                    Adeudos_Cuenta.P_Año = "" + Anio;
                    Adeudos_Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                    Dt_Adeudos = Adeudos_Cuenta.Consultar_Adeudos_Cuenta();
                    // validar que hay adeudos en la consulta
                    if (Dt_Adeudos != null)
                    {
                        if (Dt_Adeudos.Rows.Count > 0)
                        {
                            //Parcialidades
                            Int32 Parcialidades = 0;
                            Int32 Cont_Parcialidades = 0;
                            //DataTable
                            DataTable Dt_Parcialidades = new DataTable();
                            DataRow Dr_Parcialidades;
                            //Cantidades
                            Decimal Monto_Impuesto = 0;
                            Decimal Monto_Recargos = 0;
                            Decimal Monto_Honorarios = 0;
                            Decimal Monto_Moratorios = 0;
                            Decimal Total_Convenio = 0;
                            Decimal Monto_Importe = 0;
                            //Monto de cada parcialidad de la 2 en adelante
                            Decimal Monto_Parcialidades = 0;
                            int Multiplicador = 1;
                            //Periodicidad
                            String Dias_Periodo = "";
                            //Cantidades totales
                            Decimal Total_Anticipo = 0;
                            Decimal Dec_Total_Anticipo = 0;
                            Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                            Decimal Total_Importe_Parcialidad = 0;
                            Int32 Numero_Parcialidades = 0;
                            DateTime Fecha_Parcialidad;
                            DateTime Fecha_Convenio;

                            // obtener la fecha de parcialidades, o tomar fecha actual si no hay fecha en el campo fecha parcialidaddes
                            if (!DateTime.TryParse(Txt_Fecha_Convenio.Text, out Fecha_Parcialidad))
                            {
                                Fecha_Parcialidad = DateTime.Now;
                            }
                            Fecha_Convenio = Fecha_Parcialidad;
                            Txt_Fecha_Convenio.Text = Fecha_Parcialidad.ToString("dd/MMM/yyyy");

                            // validar que haya periodicidad seleccionada y numero de parcialidades
                            if (Int32.TryParse(Txt_Numero_Parcialidades.Text, out Numero_Parcialidades)
                                && Numero_Parcialidades > 0 &&
                                Cmb_Periodicidad_Pago.SelectedIndex > 0 &&
                                Decimal.TryParse(Txt_Total_Anticipo.Text, out Dec_Total_Anticipo) && Dec_Total_Anticipo > 0)
                            {
                                if (Txt_Adeudo_Honorarios.Text != "")
                                {
                                    Monto_Honorarios = Convert.ToDecimal(Txt_Adeudo_Honorarios.Text);
                                }
                                if (Txt_Monto_Recargos.Text != "")
                                {
                                    Monto_Recargos = Convert.ToDecimal(Txt_Monto_Recargos.Text) - Convert.ToDecimal(Txt_Descuento_Recargos_Ordinarios.Text);
                                }
                                if (Txt_Monto_Moratorios.Text != "")
                                {
                                    Monto_Moratorios = Convert.ToDecimal(Txt_Monto_Moratorios.Text) - Convert.ToDecimal(Txt_Descuento_Recargos_Moratorios.Text);
                                }
                                if (Hdn_Monto_Impuesto.Value != "")
                                {
                                    Monto_Impuesto = Convert.ToDecimal(Hdn_Monto_Impuesto.Value);
                                }
                                if (Txt_Numero_Parcialidades.Text != "")
                                {
                                    Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
                                }
                                if (Txt_Total_Anticipo.Text != "")
                                {
                                    Total_Anticipo = Convert.ToDecimal(Txt_Total_Anticipo.Text);
                                }
                                if (Txt_Total_Convenio.Text != "")
                                {
                                    Total_Convenio = Convert.ToDecimal(Txt_Total_Convenio.Text);
                                }
                                if (Cmb_Periodicidad_Pago.SelectedIndex > 0)
                                {
                                    Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
                                }

                                // estructura de la tabla para los adeudos
                                Dt_Parcialidades.Columns.Add(new DataColumn("NO_PAGO", typeof(Int32)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("PERIODO", typeof(String)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_HONORARIOS", typeof(Decimal)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_ORDINARIOS", typeof(Decimal)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("RECARGOS_MORATORIOS", typeof(Decimal)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPUESTO", typeof(Decimal)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("MONTO_IMPORTE", typeof(Decimal)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("FECHA_VENCIMIENTO", typeof(DateTime)));
                                Dt_Parcialidades.Columns.Add(new DataColumn("ESTATUS", typeof(String)));
                                Dr_Parcialidades = Dt_Parcialidades.NewRow();
                                Dr_Parcialidades["NO_PAGO"] = 1;
                                if (Monto_Honorarios <= Total_Anticipo)
                                {
                                    Dr_Parcialidades["MONTO_HONORARIOS"] = Monto_Honorarios;
                                    Total_Anticipo = Total_Anticipo - Monto_Honorarios;
                                    Monto_Importe += Monto_Honorarios;
                                    Monto_Honorarios = 0;
                                }
                                else
                                {
                                    Dr_Parcialidades["MONTO_HONORARIOS"] = Total_Anticipo;
                                    Monto_Honorarios = Monto_Honorarios - Total_Anticipo;
                                    Monto_Importe += Total_Anticipo;
                                    Total_Anticipo = 0;
                                }
                                if (Monto_Recargos <= Total_Anticipo)
                                {
                                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                                    Total_Anticipo = Total_Anticipo - Monto_Recargos;
                                    Monto_Importe += Monto_Recargos;
                                    Monto_Recargos = 0;
                                }
                                else
                                {
                                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Total_Anticipo;
                                    Monto_Recargos = Monto_Recargos - Total_Anticipo;
                                    Monto_Importe += Total_Anticipo;
                                    Total_Anticipo = 0;
                                }
                                if (Monto_Moratorios <= Total_Anticipo)
                                {
                                    Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Moratorios;
                                    Total_Anticipo = Total_Anticipo - Monto_Moratorios;
                                    Monto_Importe += Monto_Moratorios;
                                    Monto_Moratorios = 0;
                                }
                                else
                                {
                                    Dr_Parcialidades["RECARGOS_MORATORIOS"] = Total_Anticipo;
                                    Monto_Moratorios = Monto_Moratorios - Total_Anticipo;
                                    Monto_Importe += Total_Anticipo;
                                    Total_Anticipo = 0;
                                }
                                Dr_Parcialidades["MONTO_IMPUESTO"] = 0;
                                Bimestre_Aux = 1;
                                int i = Bimestre_Aux;
                                Decimal Monto_A_Sumar = 0;
                                DataRow Dr_Renglon_Actual_Externo = Dt_Adeudos.Rows[0];
                                //for (i = 1; i < 7; i++)
                                //{
                                //    if (!Dt_Adeudos.Rows[0]["ADEUDO_BIMESTRE_" + i].ToString().Equals("0"))
                                //    {
                                //        Dr_Parcialidades["PERIODO"] = "" + i + "/" + Dt_Adeudos.Rows[0]["ANIO"].ToString();
                                //        break;
                                //    }
                                //}
                                foreach (DataRow Dr_Renglon_Actual in Dt_Adeudos.Rows)
                                {
                                    String Desde_Bimestre = "";
                                    String Hasta_Bimestre = "";
                                    // recuperar periodo si ya existe en la nueva fila
                                    if (Dr_Parcialidades["PERIODO"].ToString().Length > 6)
                                    {
                                        Desde_Bimestre = Dr_Parcialidades["PERIODO"].ToString().Substring(0, 6);
                                    }
                                    for (i = 1; i < 7; i++)
                                    {
                                        if (Dr_Renglon_Actual["ANIO"].ToString().Equals("" + Anio) && i == Bimestre)
                                        {
                                            //Validaciones internas para cobrar...
                                            if (Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) <= Total_Anticipo)
                                            {
                                                //Asignar montos... solo si es mayor a cero
                                                if (Decimal.Parse(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) > 0)
                                                {
                                                    Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString())
                                                        + Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                    Total_Anticipo = Total_Anticipo - Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                    Monto_Importe += Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                    // formar el periodo: si la variable desde_bimestre esta vacia, asignarle el bimestre actual
                                                    if (Desde_Bimestre == "")
                                                    {
                                                        Desde_Bimestre = i + "/" + Dr_Renglon_Actual["ANIO"].ToString();
                                                    }
                                                    Hasta_Bimestre = i + "/" + Dr_Renglon_Actual["ANIO"].ToString();
                                                    Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                                }
                                            }
                                            else
                                            {
                                                //Instrucciones para salir del ciclo y mantener variables  para los siguientes calculos...
                                                Bimestre_Aux = i;
                                                break;
                                            }
                                            if (i == 6)
                                            {
                                                //si i representando el bimestre es el ultimo... romper ciclo for y salir al ciclo externo del foreach
                                                Bimestre_Aux = i;
                                                break;
                                            }
                                            Bimestre_Aux = i;
                                        }
                                        //Validaciones internas para cobrar...
                                        if (Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) <= Total_Anticipo)
                                        {
                                            //Asignar montos... solo si es mayor a cero
                                            if (Decimal.Parse(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) > 0)
                                            {
                                                Int32 Siguiente_Bimestre = i + 1;
                                                Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString())
                                                    + Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                Total_Anticipo = Total_Anticipo - Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                Monto_Importe += Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                if (Desde_Bimestre == "")
                                                {
                                                    Desde_Bimestre = i + "/" + Dr_Renglon_Actual["ANIO"].ToString();
                                                }
                                                // si el adeudo de los siguientes bimestres del mismo año es cero, tomar para el periodo actual (agregar validacion cuota minima)
                                                while (Siguiente_Bimestre < 6)
                                                {
                                                    if (Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + Siguiente_Bimestre].ToString()) > 0)
                                                    {
                                                        Siguiente_Bimestre = 6;
                                                    }
                                                    else
                                                    {
                                                        i = Siguiente_Bimestre++;
                                                    }
                                                }
                                                Hasta_Bimestre = i + "/" + Dr_Renglon_Actual["ANIO"].ToString();
                                                Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                            }
                                        }
                                        else
                                        {
                                            //Instrucciones para salir del ciclo y mantener variables  para los siguientes calculos...
                                            Bimestre_Aux = i;
                                            break;
                                        }
                                        if (i == 6)
                                        {
                                            //si i representando el bimestre es el ultimo... romper ciclo for y salir al ciclo externo del foreach
                                            Bimestre_Aux = i;
                                            break;
                                        }
                                        Bimestre_Aux = i;
                                    }
                                    Dr_Renglon_Actual_Externo = Dr_Renglon_Actual;
                                    if (Dr_Renglon_Actual["ANIO"].ToString().Equals("" + Anio) && i == Bimestre)
                                    {
                                        break;
                                    }
                                    if (!(Convert.ToDecimal(Dr_Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) <= Total_Anticipo))
                                    {
                                        break;
                                    }
                                }

                                Anio_Aux = Convert.ToInt32(Dr_Renglon_Actual_Externo["ANIO"].ToString());

                                // recuperar el ultimo periodo agregado
                                if (Dr_Parcialidades["PERIODO"].ToString().Length >= 13)
                                {
                                    Int32 Ultimo_Bimestre_Incluido = 0;
                                    Int32 Ultimo_Anio_Incluido = 0;
                                    Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(7, 1), out Ultimo_Bimestre_Incluido);
                                    Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(9, 4), out Ultimo_Anio_Incluido);

                                    // si el bimestre es menor a 6 incrementarlo en 1, si no, igualar a 1
                                    if (Ultimo_Bimestre_Incluido < 6)
                                    {
                                        Bimestre_Aux = Ultimo_Bimestre_Incluido + 1;
                                    }
                                    else
                                    {
                                        Bimestre_Aux = 1;
                                        Anio_Aux = Ultimo_Anio_Incluido + 1;
                                    }
                                }

                                if (Dr_Parcialidades["MONTO_IMPUESTO"].ToString().Equals("0"))
                                {
                                    Dr_Parcialidades["PERIODO"] = " - ";
                                }
                                Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                                Txt_Total_Anticipo.Text = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPORTE"].ToString()).ToString("#,##0.00");
                                Txt_Porcentaje_Anticipo.Text = ((Convert.ToDecimal(Txt_Total_Anticipo.Text) / Convert.ToDecimal(Txt_Sub_Total.Text)) * 100).ToString("#,##0.00");
                                Txt_Total_Convenio.Text = (Convert.ToDecimal(Txt_Sub_Total.Text) - Convert.ToDecimal(Txt_Total_Anticipo.Text)).ToString("#,##0.00");
                                Total_Convenio = Convert.ToDecimal(Txt_Total_Convenio.Text);
                                // la variable Fecha_Parcialidad se inicializo con DateTime.Now
                                Dr_Parcialidades["FECHA_VENCIMIENTO"] = Fecha_Parcialidad;
                                Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                                Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                                //Grid_Parcialidades.DataSource = Dt_Parcialidades;
                                //Grid_Parcialidades.PageIndex = 0;
                                //Grid_Parcialidades.DataBind();
                                Total_Importe_Parcialidad = Total_Convenio / (Parcialidades - 1);

                                // variables necesarias para volver a generar las parcialidades despues del anticipo
                                Decimal Bkp_Monto_Honorarios = Monto_Honorarios;
                                Decimal Bkp_Monto_Recargos = Monto_Recargos;
                                Decimal Bkp_Monto_Moratorios = Monto_Moratorios;
                                Int32 Bkp_Anio_Aux = Anio_Aux;
                                Int32 Bkp_Bimestre_aux = Bimestre_Aux;
                                DateTime Bkp_Fecha_Parcialidad = Fecha_Parcialidad;

                                // formar las parcialidades despues del anticipo
                                for (Cont_Parcialidades = 1; Cont_Parcialidades < Parcialidades; Cont_Parcialidades++)
                                {
                                    if (Monto_A_Sumar == 0)
                                    {
                                        Monto_Parcialidades = Total_Importe_Parcialidad;
                                    }
                                    else
                                    {
                                        Monto_Parcialidades = Total_Importe_Parcialidad + Monto_A_Sumar;
                                    }
                                    Monto_Importe = 0;
                                    Dr_Parcialidades = Dt_Parcialidades.NewRow();
                                    Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;
                                    if (Monto_Honorarios <= Monto_Parcialidades)
                                    {
                                        Dr_Parcialidades["MONTO_HONORARIOS"] = Monto_Honorarios;
                                        Monto_Parcialidades = Monto_Parcialidades - Monto_Honorarios;
                                        Monto_Importe += Monto_Honorarios;
                                        Monto_Honorarios = 0;
                                    }
                                    else
                                    {
                                        Dr_Parcialidades["MONTO_HONORARIOS"] = Monto_Parcialidades;
                                        Monto_Honorarios = Monto_Honorarios - Monto_Parcialidades;
                                        Monto_Importe += Monto_Parcialidades;
                                        Monto_Parcialidades = 0;
                                    }
                                    if (Monto_Recargos <= Monto_Parcialidades)
                                    {
                                        Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                                        Monto_Parcialidades = Monto_Parcialidades - Monto_Recargos;
                                        Monto_Importe += Monto_Recargos;
                                        Monto_Recargos = 0;
                                    }
                                    else
                                    {
                                        Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Parcialidades;
                                        Monto_Recargos = Monto_Recargos - Monto_Parcialidades;
                                        Monto_Importe += Monto_Parcialidades;
                                        Monto_Parcialidades = 0;
                                    }
                                    if (Monto_Moratorios <= Monto_Parcialidades)
                                    {
                                        Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Moratorios;
                                        Monto_Parcialidades = Monto_Parcialidades - Monto_Moratorios;
                                        Monto_Importe += Monto_Moratorios;
                                        Monto_Moratorios = 0;
                                    }
                                    else
                                    {
                                        Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Parcialidades;
                                        Monto_Moratorios = Monto_Moratorios - Monto_Parcialidades;
                                        Monto_Importe += Monto_Parcialidades;
                                        Monto_Parcialidades = 0;
                                    }
                                    //Dr_Parcialidades["PERIODO"] = Bimestre_Aux + "/" + Anio_Aux;
                                    Dr_Parcialidades["MONTO_IMPUESTO"] = 0;
                                    foreach (DataRow Renglon_Actual in Dt_Adeudos.Rows)
                                    {
                                        if (Convert.ToInt32(Renglon_Actual["ANIO"].ToString()) == Anio_Aux)
                                        {
                                            String Desde_Bimestre = "";
                                            String Hasta_Bimestre = "";

                                            // recuperar periodo si ya existe en la nueva fila
                                            if (Dr_Parcialidades["PERIODO"].ToString().Length > 6)
                                            {
                                                Desde_Bimestre = Dr_Parcialidades["PERIODO"].ToString().Substring(0, 6);
                                            }
                                            // recorrer del bimestre en que se quedo el ultimo periodo hasta el sexto (si no se supera antes)
                                            for (i = Bimestre_Aux; i <= 6; i++)
                                            {
                                                //if (Anio_Aux == Anio && Bimestre == i)
                                                //{
                                                //    Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString())
                                                //        + Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                //    //Dr_Parcialidades["PERIODO"] = Dr_Parcialidades["PERIODO"].ToString() + " - " + i + "/" + Anio_Aux;
                                                //    Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe
                                                //        + Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                //    // obtener la fecha de la siguiente parcialidad a partir de la la fecha anterior
                                                //    Fecha_Parcialidad = Obtener_Fecha_Periodo(Fecha_Parcialidad, Cmb_Periodicidad_Pago.SelectedValue);
                                                //    Dr_Parcialidades["FECHA_VENCIMIENTO"] = Fecha_Parcialidad;
                                                //    Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                                                //    /// formar periodo
                                                //    if (Desde_Bimestre == "")
                                                //    {
                                                //        Desde_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                //    }
                                                //    Hasta_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                //    Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                                //    // agregar fila a la tabla
                                                //    Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                                                //    Grid_Parcialidades.DataSource = Dt_Parcialidades;
                                                //    Grid_Parcialidades.PageIndex = 0;
                                                //    Grid_Parcialidades.DataBind();
                                                //    Sumar_Totales_Parcialidades();
                                                //    return;
                                                //}
                                                if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) <= Monto_Parcialidades)
                                                {
                                                    Int32 Siguiente_Bimestre = i + 1;
                                                    Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString())
                                                        + Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                    Monto_Parcialidades = Monto_Parcialidades - Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                    Monto_Importe += Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                    /// formar periodo
                                                    if (Desde_Bimestre == "")
                                                    {
                                                        Desde_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                    }
                                                    // si el adeudo de los siguientes bimestres del mismo año es cero, tomar para el periodo actual (agregar validacion cuota minima)
                                                    while (Siguiente_Bimestre < 6)
                                                    {
                                                        if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + Siguiente_Bimestre].ToString()) > 0)
                                                        {
                                                            // el siguiente bimestre tiene un adeudo mayor que 0
                                                            break;
                                                        }
                                                        else
                                                        {
                                                            i = Siguiente_Bimestre++;
                                                            // validar que el bimestre seleccionado no sea menor que el bimestre que se esta asignando, de ser asi, se debe seleccionar el bimestre
                                                            if (Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue) < i)
                                                            {
                                                                Cmb_Hasta_Bimestre_Periodo.SelectedValue = i.ToString();
                                                            }
                                                        }
                                                    }
                                                    Hasta_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                    Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                                    if (i == 6)
                                                    {
                                                        break;
                                                    }
                                                }
                                                else if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + (i)].ToString()) > Monto_Parcialidades)
                                                {
                                                    Bimestre_Aux = i;
                                                    //Monto_A_Sumar += Monto_Parcialidades;
                                                    break;
                                                }
                                            }
                                            Anio_Aux = Convert.ToInt32(Renglon_Actual["ANIO"].ToString());
                                            Bimestre_Aux = i;
                                            if (Bimestre_Aux == 6)
                                            {
                                                Int32 Ultimo_Bimestre_Incluido = 0;
                                                Int32 Ultimo_Anio_Incluido = 0;
                                                Anio_Aux += 1;
                                                Bimestre_Aux = 1;
                                                if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + (i)].ToString()) > Monto_Parcialidades)
                                                {
                                                    Anio_Aux -= 1;
                                                    Bimestre_Aux = 6;
                                                    // obtener el ultimo periodo agregado solo si el periodo contiene 13 o mas caracteres
                                                    if (Dr_Parcialidades["PERIODO"].ToString().Length >= 13)
                                                    {
                                                        Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(7, 1), out Ultimo_Bimestre_Incluido);
                                                        Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(9, 4), out Ultimo_Anio_Incluido);
                                                        // si el bimestre ya se incluyo en los adeudos, pasar al siguiente para evitar duplicados
                                                        if (Anio_Aux == Ultimo_Anio_Incluido && Bimestre_Aux == Ultimo_Bimestre_Incluido)
                                                        {
                                                            // si el bimestre es menor a 6 incrementarlo en 1, si no, igualar a 1
                                                            if (Bimestre_Aux < 6)
                                                            {
                                                                Bimestre_Aux++;
                                                            }
                                                            else
                                                            {
                                                                Bimestre_Aux = 1;
                                                            }
                                                            Anio_Aux++;
                                                        }
                                                    }
                                                    break;
                                                }
                                                if (Cont_Parcialidades < Parcialidades && Bimestre_Aux == Bimestre && Anio_Aux == Anio)
                                                {
                                                    break;
                                                }
                                            }
                                            else if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) > Monto_Parcialidades)
                                            {
                                                break;

                                            }
                                        }
                                    }

                                    Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                                    Fecha_Parcialidad = Obtener_Fecha_Periodo(Fecha_Convenio, Cmb_Periodicidad_Pago.SelectedValue, Cont_Parcialidades);
                                    Dr_Parcialidades["FECHA_VENCIMIENTO"] = Fecha_Parcialidad;
                                    Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                                    if (Dr_Parcialidades["MONTO_IMPUESTO"].ToString().Equals("0"))
                                    {
                                        Dr_Parcialidades["PERIODO"] = "-";
                                    }
                                    Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                                    Grid_Parcialidades.DataSource = Dt_Parcialidades;
                                    Grid_Parcialidades.PageIndex = 0;
                                    Grid_Parcialidades.DataBind();

                                    // comprobar que se incluyeron todos los bimestres si ya se llego al final de las parcialidades
                                    if ((Cont_Parcialidades + 1) >= Parcialidades)
                                    {
                                        // comprobar que el periodo contiene 13 caracteres o mas
                                        if (Dr_Parcialidades["PERIODO"].ToString().Length >= 13)
                                        {
                                            String Ultima_Parcialidad_Incluida = Dr_Parcialidades["PERIODO"].ToString().Substring(7, 6);
                                            String Ultima_Parcialidad_Seleccionada = Cmb_Hasta_Bimestre_Periodo.SelectedValue + "/" + Cmb_Hasta_Anio_Periodo.SelectedValue;
                                            // si el periodo no es igual que el seleccionado en los combos hasta periodo, reiniciar conteo y eliminar renglones despues del anticipo
                                            if (Ultima_Parcialidad_Incluida != Ultima_Parcialidad_Seleccionada)
                                            {
                                                Int32 j = Dt_Parcialidades.Rows.Count - 1;
                                                // reiniciar contador de parcialidades y parametros auxiliares
                                                Cont_Parcialidades = 0;
                                                Monto_Honorarios = Bkp_Monto_Honorarios;
                                                Monto_Recargos = Bkp_Monto_Recargos;
                                                Monto_Moratorios = Bkp_Monto_Moratorios;
                                                Anio_Aux = Bkp_Anio_Aux;
                                                Bimestre_Aux = Bkp_Bimestre_aux;
                                                Fecha_Parcialidad = Bkp_Fecha_Parcialidad;

                                                // eliminar las filas del datatable
                                                while (j > 0)
                                                {
                                                    Dt_Parcialidades.Rows[j].Delete();
                                                    j--;
                                                }
                                                Dt_Parcialidades.AcceptChanges();

                                                // obtener el saldo del siguiente bimestre excluido
                                                // recorrer la tabla de adeudos
                                                foreach (DataRow Drw_Adeudo in Dt_Adeudos.Rows)
                                                {
                                                    // si el año es igual que el año de la ultima parcialidad, sumar bimestres
                                                    if (Drw_Adeudo["Anio"].ToString() == Ultima_Parcialidad_Incluida.Substring(2, 4))
                                                    {
                                                        Int32 Ultimo_Bimestre_Incluido = Convert.ToInt32(Ultima_Parcialidad_Incluida.Substring(0, 1));
                                                        Int32 Indice_Fila = Dt_Adeudos.Rows.IndexOf(Drw_Adeudo);
                                                        // Agregar al Monto_A_Sumar el siguiente adeudo
                                                        Decimal Tmp_Monto_A_Sumar = Monto_A_Sumar;
                                                        while (Monto_A_Sumar <= Tmp_Monto_A_Sumar)
                                                        {
                                                            // si es en el mismo año, consultar el adeudo del siguiente bimestre
                                                            if (Ultimo_Bimestre_Incluido < 6)
                                                            {
                                                                if (Convert.ToDecimal(Dt_Adeudos.Rows[Indice_Fila]["ADEUDO_BIMESTRE_" + (Ultimo_Bimestre_Incluido)]) > 0)
                                                                {
                                                                    // agregar el monto del siguiente bimestre
                                                                    Monto_A_Sumar = (Convert.ToDecimal(Drw_Adeudo["ADEUDO_BIMESTRE_" + (Ultimo_Bimestre_Incluido)]) / (Numero_Parcialidades * 2)) * Multiplicador++;
                                                                }
                                                                else
                                                                {
                                                                    Ultimo_Bimestre_Incluido++;
                                                                }
                                                            }
                                                            else
                                                            {
                                                                Ultimo_Bimestre_Incluido = 1;
                                                                // asignar utilizar la siguiente fila
                                                                Indice_Fila++;
                                                            }
                                                            // si ya se alcanzó la ultima fila, asignar valor al monto_A_Asignar
                                                            if (Indice_Fila >= Dt_Adeudos.Rows.Count)
                                                            {
                                                                Monto_A_Sumar += Monto_A_Sumar * Multiplicador++;
                                                            }
                                                        }
                                                        break;

                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                                try
                                {
                                    Sumar_Totales_Parcialidades();
                                }
                                catch { }
                            }
                        }
                        else
                        {
                            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de Predial", "alert('Imposible realizar las parcialidades debido a que no contiene adeudos');", true);
                        }

                    } // calidacion de Dt_Adeudos con mas de cero filas
                } // validacion de Dt_adeudos no nulo

                else
                {
                    Grid_Parcialidades_Editable = false;
                    Grid_Parcialidades.DataSource = null;
                    Grid_Parcialidades.DataBind();
                }
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Parcialidades
    /// DESCRIPCIÓN: Calcular las parcialidades con los parametros especificados desde un numero de pago dado
    /// PARÁMETROS:
    /// 		1. No_Pago: Numero de parcialidad desde la que se vuleve a calcular
    /// 		2. Dt_Parcial: Datatable con las parcialidades anteriores a la indicada en No_Pago
    /// 		3. Monto_Parcial_Honorarios: Monto restante por concepto de honorarios
    /// 		4. Monto_Parcial_Recargos: Monto restante de recargos ordinarios
    /// 		5. Monto_Parcial_Moratorios: monto restante de recargos moratorios
    /// 		6. Monto_Parcial_Impuesto: monto restante de impuesto predial
    /// 		7. Anio_Parcial: Año del primer bimestre a incluir en los adeudos a generar
    /// 		8. Bimestre_Parcial: Primer bimestre a incluir en los adeudos a generar
    /// 		9. Fecha_Parcial: Fecha de vencimiento de la ultima parcialidad
    /// 		10. Monto_Parcial: Monto especificado por el usuario para la parcialidad especificada
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 28-oct-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Parcialidades(Int32 No_Pago, DataTable Dt_Parcial,
        Decimal Monto_Parcial_Honorarios, Decimal Monto_Parcial_Recargos,
        Decimal Monto_Parcial_Moratorios, Decimal Monto_Parcial_Impuesto,
        Decimal Monto_Parcial_Importe, Int32 Anio_Parcial,
        Int32 Bimestre_Parcial, DateTime Fecha_Parcial, Decimal Monto_Parcial)
    {
        if (Txt_Numero_Parcialidades.Text != "" && Convert.ToInt32(Txt_Numero_Parcialidades.Text) > 0)
        {
            if (Btn_Nuevo.ToolTip == "Dar de Alta" || Btn_Modificar.ToolTip == "Actualizar")
            {
                Grid_Parcialidades_Editable = true;
            }
            if ((Convert.ToDecimal(Txt_Monto_Recargos.Text) + Convert.ToDecimal(Txt_Monto_Moratorios.Text) + Convert.ToDecimal(Hdn_Monto_Impuesto.Value) + Convert.ToDecimal(Txt_Adeudo_Honorarios.Text)) > 0)
            {
                Int32 Bimestre = Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue);
                Int32 Anio = Convert.ToInt32(Cmb_Hasta_Anio_Periodo.SelectedValue);
                //Anio y bimestre inicial (utilizar año y bimestre que llegan como parámetro)
                Int32 Bimestre_Aux = Bimestre_Parcial;
                Int32 Anio_Aux = Anio_Parcial;
                //Adeudos
                DataTable Dt_Adeudos = new DataTable();
                Cls_Ope_Pre_Convenios_Predial_Negocio Adeudos_Cuenta = new Cls_Ope_Pre_Convenios_Predial_Negocio();
                Adeudos_Cuenta.P_Año = "" + Anio;
                Adeudos_Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Dt_Adeudos = Adeudos_Cuenta.Consultar_Adeudos_Cuenta();
                // validar que hay adeudos en la consulta
                if (Dt_Adeudos != null)
                {
                    if (Dt_Adeudos.Rows.Count > 0)
                    {
                        //Parcialidades
                        Int32 Parcialidades = 0;
                        Int32 Cont_Parcialidades = No_Pago;
                        //DataTable
                        DataTable Dt_Parcialidades = Dt_Parcial.Copy();
                        DataRow Dr_Parcialidades;
                        //Cantidades
                        Decimal Monto_Impuesto = 0;
                        Decimal Monto_Recargos = 0;
                        Decimal Monto_Honorarios = 0;
                        Decimal Monto_Moratorios = 0;
                        Decimal Total_Convenio = 0;
                        Decimal Monto_Importe = 0;
                        //Monto de cada parcialidad de la 2 en adelante
                        Decimal Monto_Parcialidades = 0;
                        int Multiplicador = 1;
                        //Periodicidad
                        String Dias_Periodo = "";
                        //Cantidades totales
                        Decimal Total_Anticipo = Monto_Parcial;
                        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
                        Decimal Total_Importe_Parcialidad = 0;
                        Int32 Numero_Parcialidades = 0;
                        DateTime Fecha_Parcialidad = Fecha_Parcial;
                        DateTime Fecha_Convenio;

                        // obtener la fecha del convenio, o tomar fecha actual si no hay fecha en el campo fecha parcialidaddes
                        if (!DateTime.TryParse(Txt_Fecha_Convenio.Text, out Fecha_Convenio))
                        {
                            Fecha_Convenio = DateTime.Now;
                        }

                        // validar que haya periodicidad seleccionada y numero de parcialidades
                        if (Int32.TryParse(Txt_Numero_Parcialidades.Text, out Numero_Parcialidades)
                            && Numero_Parcialidades > 0 &&
                            Cmb_Periodicidad_Pago.SelectedIndex > 0 &&
                            Monto_Parcial > 0)
                        {
                            if (Txt_Adeudo_Honorarios.Text != "")
                            {
                                Monto_Honorarios = Convert.ToDecimal(Txt_Adeudo_Honorarios.Text) - Monto_Parcial_Honorarios;
                            }
                            if (Txt_Monto_Recargos.Text != "")
                            {
                                Monto_Recargos = Convert.ToDecimal(Txt_Monto_Recargos.Text) - Convert.ToDecimal(Txt_Descuento_Recargos_Ordinarios.Text)
                                    - Monto_Parcial_Recargos;
                            }
                            if (Txt_Monto_Moratorios.Text != "")
                            {
                                Monto_Moratorios = Convert.ToDecimal(Txt_Monto_Moratorios.Text) - Convert.ToDecimal(Txt_Descuento_Recargos_Moratorios.Text)
                                    - Monto_Parcial_Moratorios;
                            }
                            if (Hdn_Monto_Impuesto.Value != "")
                            {
                                Monto_Impuesto = Convert.ToDecimal(Hdn_Monto_Impuesto.Value) - Monto_Parcial_Impuesto;
                            }
                            if (Txt_Numero_Parcialidades.Text != "")
                            {
                                Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
                            }
                            if (Monto_Parcial > 0)
                            {
                                Total_Anticipo = Monto_Parcial;
                            }
                            if (Txt_Total_Convenio.Text != "")
                            {
                                Total_Convenio = Convert.ToDecimal(Txt_Sub_Total.Text)
                                    - (Monto_Parcial_Honorarios + Monto_Parcial_Recargos + Monto_Parcial_Moratorios + Monto_Parcial_Impuesto);
                            }
                            if (Cmb_Periodicidad_Pago.SelectedIndex > 0)
                            {
                                Dias_Periodo = Cmb_Periodicidad_Pago.SelectedValue;
                            }


                            // Nueva fila para anticipo
                            Dr_Parcialidades = Dt_Parcialidades.NewRow();
                            Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades;
                            if (Monto_Honorarios <= Total_Anticipo)
                            {
                                Dr_Parcialidades["MONTO_HONORARIOS"] = Monto_Honorarios;
                                Total_Anticipo = Total_Anticipo - Monto_Honorarios;
                                Monto_Importe += Monto_Honorarios;
                                Monto_Honorarios = 0;
                            }
                            else
                            {
                                Dr_Parcialidades["MONTO_HONORARIOS"] = Total_Anticipo;
                                Monto_Honorarios = Monto_Honorarios - Total_Anticipo;
                                Monto_Importe += Total_Anticipo;
                                Total_Anticipo = 0;
                            }
                            if (Monto_Recargos <= Total_Anticipo)
                            {
                                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                                Total_Anticipo = Total_Anticipo - Monto_Recargos;
                                Monto_Importe += Monto_Recargos;
                                Monto_Recargos = 0;
                            }
                            else
                            {
                                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Total_Anticipo;
                                Monto_Recargos = Monto_Recargos - Total_Anticipo;
                                Monto_Importe += Total_Anticipo;
                                Total_Anticipo = 0;
                            }
                            if (Monto_Moratorios <= Total_Anticipo)
                            {
                                Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Moratorios;
                                Total_Anticipo = Total_Anticipo - Monto_Moratorios;
                                Monto_Importe += Monto_Moratorios;
                                Monto_Moratorios = 0;
                            }
                            else
                            {
                                Dr_Parcialidades["RECARGOS_MORATORIOS"] = Total_Anticipo;
                                Monto_Moratorios = Monto_Moratorios - Total_Anticipo;
                                Monto_Importe += Total_Anticipo;
                                Total_Anticipo = 0;
                            }
                            Dr_Parcialidades["MONTO_IMPUESTO"] = 0;

                            int i = Bimestre_Aux;
                            int Renglon = 0;
                            Decimal Monto_A_Sumar = 0;
                            DataRow Dr_Renglon_Actual_Externo = Dt_Adeudos.Rows[0];

                            i = Bimestre_Aux;

                            for (Renglon = 0; Renglon < Dt_Adeudos.Rows.Count; Renglon++)
                            {
                                // si el año en el renglon de la tabla es menor que el de la ultima parcialidad, pasar al siguiente renglon
                                if (Int32.Parse(Dt_Adeudos.Rows[Renglon]["ANIO"].ToString()) < Anio_Parcial)
                                {
                                    continue;
                                }

                                String Desde_Bimestre = "";
                                String Hasta_Bimestre = "";
                                // recuperar periodo si ya existe en la nueva fila
                                if (Dr_Parcialidades["PERIODO"].ToString().Length > 6)
                                {
                                    Desde_Bimestre = Dr_Parcialidades["PERIODO"].ToString().Substring(0, 6);
                                }
                                for (i = Bimestre_Aux; i < 7; i++)
                                {
                                    if (Dt_Adeudos.Rows[Renglon]["ANIO"].ToString().Equals("" + Anio) && i == Bimestre)
                                    {
                                        //Validaciones internas para cobrar...
                                        if (Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString()) <= Total_Anticipo)
                                        {
                                            //Asignar montos... solo si es mayor a cero
                                            if (Decimal.Parse(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString()) > 0)
                                            {
                                                Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString()) + Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString());
                                                Total_Anticipo = Total_Anticipo - Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString());
                                                Monto_Importe += Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString());
                                                // formar el periodo: si la variable desde_bimestre esta vacia, asignarle el bimestre actual
                                                if (Desde_Bimestre == "")
                                                {
                                                    Desde_Bimestre = i + "/" + Dt_Adeudos.Rows[Renglon]["ANIO"].ToString();
                                                }
                                                Hasta_Bimestre = i + "/" + Dt_Adeudos.Rows[Renglon]["ANIO"].ToString();
                                                Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                            }
                                        }
                                        else
                                        {
                                            //Instrucciones para salir del ciclo y mantener variables  para los siguientes calculos...
                                            Bimestre_Aux = i;
                                            break;
                                        }
                                        if (i == 6)
                                        {
                                            //si i representando el bimestre es el ultimo... romper ciclo for y salir al ciclo externo del foreach
                                            Bimestre_Aux = 1;
                                            break;
                                        }
                                        Bimestre_Aux = i;
                                    }
                                    //Validaciones internas para cobrar...
                                    if (Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString()) <= Total_Anticipo)
                                    {
                                        //Asignar montos... solo si es mayor a cero
                                        if (Decimal.Parse(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString()) > 0)
                                        {
                                            Int32 Siguiente_Bimestre = i + 1;
                                            Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString()) + Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString());
                                            Total_Anticipo = Total_Anticipo - Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString());
                                            Monto_Importe += Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString());
                                            if (Desde_Bimestre == "")
                                            {
                                                Desde_Bimestre = i + "/" + Dt_Adeudos.Rows[Renglon]["ANIO"].ToString();
                                            }
                                            // si el adeudo de los siguientes bimestres del mismo año es cero, tomar para el periodo actual (agregar validacion cuota minima)
                                            while (Siguiente_Bimestre < 6)
                                            {
                                                if (Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + Siguiente_Bimestre].ToString()) > 0)
                                                {
                                                    break;
                                                }
                                                else
                                                {
                                                    i = Siguiente_Bimestre++;
                                                }
                                            }
                                            Hasta_Bimestre = i + "/" + Dt_Adeudos.Rows[Renglon]["ANIO"].ToString();
                                            Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                        }
                                    }
                                    else
                                    {
                                        //Instrucciones para salir del ciclo y mantener variables  para los siguientes calculos...
                                        Bimestre_Aux = i;
                                        break;
                                    }
                                    if (i == 6)
                                    {
                                        //si i representando el bimestre es el ultimo... romper ciclo for y salir al ciclo externo del foreach
                                        Bimestre_Aux = 1;
                                        break;
                                    }
                                    Bimestre_Aux = i;
                                }
                                Dr_Renglon_Actual_Externo = Dt_Adeudos.Rows[Renglon];
                                if (Dt_Adeudos.Rows[Renglon]["ANIO"].ToString().Equals("" + Anio) && i == Bimestre)
                                {
                                    break;
                                }
                                if (!(Convert.ToDecimal(Dt_Adeudos.Rows[Renglon]["ADEUDO_BIMESTRE_" + i].ToString()) <= Total_Anticipo))
                                {
                                    break;
                                }
                            }
                            i = Bimestre_Aux;
                            Anio_Aux = Convert.ToInt32(Dr_Renglon_Actual_Externo["ANIO"].ToString());

                            // recuperar el ultimo periodo agregado
                            if (Dr_Parcialidades["PERIODO"].ToString().Length >= 13)
                            {
                                Int32 Ultimo_Bimestre_Incluido = 0;
                                Int32 Ultimo_Anio_Incluido = 0;
                                Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(7, 1), out Ultimo_Bimestre_Incluido);
                                Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(9, 4), out Ultimo_Anio_Incluido);

                                // si el bimestre es menor a 6 incrementarlo en 1, si no, igualar a 1
                                if (Ultimo_Bimestre_Incluido < 6)
                                {
                                    Bimestre_Aux = Ultimo_Bimestre_Incluido + 1;
                                }
                                else
                                {
                                    Bimestre_Aux = 1;
                                    Anio_Aux = Ultimo_Anio_Incluido + 1;
                                }
                            }

                            if (Dr_Parcialidades["MONTO_IMPUESTO"].ToString().Equals("0"))
                            {
                                Dr_Parcialidades["PERIODO"] = " - ";
                            }
                            Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                            Total_Convenio -= Monto_Importe;
                            //Txt_Total_Anticipo.Text = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPORTE"].ToString()).ToString("###,###,###,##0.00");
                            //Txt_Porcentaje_Anticipo.Text = ((Convert.ToDecimal(Txt_Total_Anticipo.Text) / Convert.ToDecimal(Txt_Sub_Total.Text)) * 100).ToString("0.00");
                            //Txt_Total_Convenio.Text = (Convert.ToDecimal(Txt_Sub_Total.Text) - Convert.ToDecimal(Txt_Total_Anticipo.Text)).ToString("###,###,###,##0.00");
                            //Total_Convenio = Convert.ToDecimal(Txt_Total_Convenio.Text);
                            // la variable Fecha_Parcialidad se inicializo con DateTime.Now
                            Dr_Parcialidades["FECHA_VENCIMIENTO"] = Fecha_Parcialidad;
                            Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                            Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                            //Grid_Parcialidades.DataSource = Dt_Parcialidades;
                            //Grid_Parcialidades.PageIndex = 0;
                            //Grid_Parcialidades.DataBind();
                            Total_Importe_Parcialidad = Total_Convenio / (Parcialidades - No_Pago);

                            // recuperar el siguiente bimestre a incluir incrementando en 1 el ultimo en la parcialidad
                            if (Dt_Parcialidades.Rows[Dt_Parcialidades.Rows.Count - 1]["PERIODO"].ToString().Length >= 13)
                            {
                                Int32.TryParse(Dt_Parcialidades.Rows[Dt_Parcialidades.Rows.Count - 1]["PERIODO"].ToString().Substring(7, 1), out Bimestre_Aux);
                                Int32.TryParse(Dt_Parcialidades.Rows[Dt_Parcialidades.Rows.Count - 1]["PERIODO"].ToString().Substring(9, 4), out Anio_Aux);
                                // si el bimestre es menor a 6 incrementarlo en 1, si no, igualar a 1
                                if (Bimestre_Aux < 6)
                                {
                                    Bimestre_Aux++;
                                }
                                else
                                {
                                    Bimestre_Aux = 1;
                                    Anio_Aux++;
                                }
                            }

                            // variables necesarias para volver a generar las parcialidades despues del anticipo
                            Decimal Bkp_Monto_Honorarios = Monto_Honorarios;
                            Decimal Bkp_Monto_Recargos = Monto_Recargos;
                            Decimal Bkp_Monto_Moratorios = Monto_Moratorios;
                            Int32 Bkp_Anio_Aux = Anio_Aux;
                            Int32 Bkp_Bimestre_aux = Bimestre_Aux;
                            DateTime Bkp_Fecha_Parcialidad = Fecha_Parcialidad;
                            DataTable Bkp_Dt_Parcial = Dt_Parcialidades.Copy();
                            Int32 Bkp_Cont_Parcialidades = Cont_Parcialidades--;

                            // formar las parcialidades despues del anticipo
                            for (Cont_Parcialidades = Bkp_Cont_Parcialidades; Cont_Parcialidades < Parcialidades; Cont_Parcialidades++)
                            {
                                if (Monto_A_Sumar == 0)
                                {
                                    Monto_Parcialidades = Total_Importe_Parcialidad;
                                }
                                else
                                {
                                    Monto_Parcialidades = Total_Importe_Parcialidad + Monto_A_Sumar;
                                    Monto_A_Sumar = 0;
                                }
                                Monto_Importe = 0;
                                Dr_Parcialidades = Dt_Parcialidades.NewRow();
                                Dr_Parcialidades["NO_PAGO"] = Cont_Parcialidades + 1;
                                if (Monto_Honorarios <= Monto_Parcialidades)
                                {
                                    Dr_Parcialidades["MONTO_HONORARIOS"] = Monto_Honorarios;
                                    Monto_Parcialidades = Monto_Parcialidades - Monto_Honorarios;
                                    Monto_Importe += Monto_Honorarios;
                                    Monto_Honorarios = 0;
                                }
                                else
                                {
                                    Dr_Parcialidades["MONTO_HONORARIOS"] = Monto_Parcialidades;
                                    Monto_Honorarios = Monto_Honorarios - Monto_Parcialidades;
                                    Monto_Importe += Monto_Parcialidades;
                                    Monto_Parcialidades = 0;
                                }
                                if (Monto_Recargos <= Monto_Parcialidades)
                                {
                                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                                    Monto_Parcialidades = Monto_Parcialidades - Monto_Recargos;
                                    Monto_Importe += Monto_Recargos;
                                    Monto_Recargos = 0;
                                }
                                else
                                {
                                    Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Parcialidades;
                                    Monto_Recargos = Monto_Recargos - Monto_Parcialidades;
                                    Monto_Importe += Monto_Parcialidades;
                                    Monto_Parcialidades = 0;
                                }
                                if (Monto_Moratorios <= Monto_Parcialidades)
                                {
                                    Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Moratorios;
                                    Monto_Parcialidades = Monto_Parcialidades - Monto_Moratorios;
                                    Monto_Importe += Monto_Moratorios;
                                    Monto_Moratorios = 0;
                                }
                                else
                                {
                                    Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Parcialidades;
                                    Monto_Moratorios = Monto_Moratorios - Monto_Parcialidades;
                                    Monto_Importe += Monto_Parcialidades;
                                    Monto_Parcialidades = 0;
                                }
                                //Dr_Parcialidades["PERIODO"] = Bimestre_Aux + "/" + Anio_Aux;
                                Dr_Parcialidades["MONTO_IMPUESTO"] = 0;
                                foreach (DataRow Renglon_Actual in Dt_Adeudos.Rows)
                                {
                                    if (Convert.ToInt32(Renglon_Actual["ANIO"].ToString()) == Anio_Aux)
                                    {
                                        String Desde_Bimestre = "";
                                        String Hasta_Bimestre = "";

                                        // recuperar periodo si ya existe en la nueva fila
                                        if (Dr_Parcialidades["PERIODO"].ToString().Length > 6)
                                        {
                                            Desde_Bimestre = Dr_Parcialidades["PERIODO"].ToString().Substring(0, 6);
                                        }
                                        // recorrer del bimestre en que se quedo el ultimo periodo hasta el sexto (si no se supera antes)
                                        for (i = Bimestre_Aux; i <= 6; i++)
                                        {
                                            if (Anio_Aux == Anio && Bimestre == i)
                                            {
                                                Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString())
                                                    + Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                //Dr_Parcialidades["PERIODO"] = Dr_Parcialidades["PERIODO"].ToString() + " - " + i + "/" + Anio_Aux;
                                                Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe
                                                    + Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                // obtener la fecha de la siguiente parcialidad a partir de la la fecha anterior
                                                Fecha_Parcialidad = Obtener_Fecha_Periodo(Fecha_Convenio, Cmb_Periodicidad_Pago.SelectedValue, Cont_Parcialidades);
                                                Dr_Parcialidades["FECHA_VENCIMIENTO"] = Fecha_Parcialidad;
                                                Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                                                /// formar periodo
                                                if (Desde_Bimestre == "")
                                                {
                                                    Desde_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                }
                                                Hasta_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                                // agregar fila a la tabla
                                                Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                                                Grid_Parcialidades.DataSource = Dt_Parcialidades;
                                                Grid_Parcialidades.PageIndex = 0;
                                                Grid_Parcialidades.DataBind();
                                                Sumar_Totales_Parcialidades();
                                                return;
                                            }
                                            if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) <= Monto_Parcialidades)
                                            {
                                                Int32 Siguiente_Bimestre = i + 1;
                                                Dr_Parcialidades["MONTO_IMPUESTO"] = Convert.ToDecimal(Dr_Parcialidades["MONTO_IMPUESTO"].ToString())
                                                    + Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                Monto_Parcialidades = Monto_Parcialidades - Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                Monto_Importe += Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString());
                                                /// formar periodo
                                                if (Desde_Bimestre == "")
                                                {
                                                    Desde_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                }
                                                // si el adeudo de los siguientes bimestres del mismo año es cero, tomar para el periodo actual (agregar validacion cuota minima)
                                                while (Siguiente_Bimestre < 6)
                                                {
                                                    if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + Siguiente_Bimestre].ToString()) > 0)
                                                    {
                                                        break;
                                                    }
                                                    else
                                                    {
                                                        i = Siguiente_Bimestre++;
                                                        // validar que el bimestre seleccionado no sea menor que el bimestre que se esta asignando, de ser asi, se debe seleccionar el bimestre
                                                        if (Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue) < i)
                                                        {
                                                            Cmb_Hasta_Bimestre_Periodo.SelectedValue = i.ToString();
                                                        }
                                                    }
                                                }
                                                Hasta_Bimestre = i + "/" + Renglon_Actual["ANIO"].ToString();
                                                Dr_Parcialidades["PERIODO"] = Desde_Bimestre + "-" + Hasta_Bimestre;
                                                if (i == 6)
                                                {
                                                    break;
                                                }
                                            }
                                            else if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + (i)].ToString()) > Monto_Parcialidades)
                                            {
                                                Bimestre_Aux = i;
                                                Monto_A_Sumar += Monto_Parcialidades;
                                                break;
                                            }
                                        }
                                        Anio_Aux = Convert.ToInt32(Renglon_Actual["ANIO"].ToString());
                                        Bimestre_Aux = i;
                                        if (Bimestre_Aux == 6)
                                        {
                                            Int32 Ultimo_Bimestre_Incluido = 0;
                                            Int32 Ultimo_Anio_Incluido = 0;
                                            Anio_Aux += 1;
                                            Bimestre_Aux = 1;
                                            if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + (i)].ToString()) > Monto_Parcialidades)
                                            {
                                                Anio_Aux -= 1;
                                                Bimestre_Aux = 6;
                                                // obtener el ultimo periodo agregado solo si el periodo contiene 13 o mas caracteres
                                                if (Dr_Parcialidades["PERIODO"].ToString().Length >= 13)
                                                {
                                                    Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(7, 1), out Ultimo_Bimestre_Incluido);
                                                    Int32.TryParse(Dr_Parcialidades["PERIODO"].ToString().Substring(9, 4), out Ultimo_Anio_Incluido);
                                                    // si el bimestre ya se incluyo en los adeudos, pasar al siguiente para evitar duplicados
                                                    if (Anio_Aux == Ultimo_Anio_Incluido && Bimestre_Aux == Ultimo_Bimestre_Incluido)
                                                    {
                                                        // si el bimestre es menor a 6 incrementarlo en 1, si no, igualar a 1
                                                        if (Bimestre_Aux < 6)
                                                        {
                                                            Bimestre_Aux++;
                                                        }
                                                        else
                                                        {
                                                            Bimestre_Aux = 1;
                                                        }
                                                        Anio_Aux++;
                                                    }
                                                }
                                                break;
                                            }
                                            if (Cont_Parcialidades < Parcialidades && Bimestre_Aux == Bimestre && Anio_Aux == Anio)
                                            {
                                                break;
                                            }
                                        }
                                        else if (Convert.ToDecimal(Renglon_Actual["ADEUDO_BIMESTRE_" + i].ToString()) > Monto_Parcialidades)
                                        {
                                            break;

                                        }
                                    }
                                }

                                Dr_Parcialidades["MONTO_IMPORTE"] = Monto_Importe;
                                Fecha_Parcialidad = Obtener_Fecha_Periodo(Fecha_Convenio, Cmb_Periodicidad_Pago.SelectedValue, Cont_Parcialidades);
                                Dr_Parcialidades["FECHA_VENCIMIENTO"] = Fecha_Parcialidad;
                                Dr_Parcialidades["ESTATUS"] = "POR PAGAR";
                                if (Dr_Parcialidades["MONTO_IMPUESTO"].ToString().Equals("0"))
                                {
                                    Dr_Parcialidades["PERIODO"] = "-";
                                }
                                Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
                                Grid_Parcialidades.DataSource = Dt_Parcialidades;
                                Grid_Parcialidades.PageIndex = 0;
                                Grid_Parcialidades.DataBind();

                                // comprobar que se incluyeron todos los bimestres si ya se llego al final de las parcialidades
                                if ((Cont_Parcialidades + 1) >= Parcialidades)
                                {
                                    // comprobar que el periodo contiene 13 caracteres o mas
                                    if (Dr_Parcialidades["PERIODO"].ToString().Length >= 13)
                                    {
                                        String Ultima_Parcialidad_Incluida = Dr_Parcialidades["PERIODO"].ToString().Substring(7, 6);
                                        String Ultima_Parcialidad_Seleccionada = Cmb_Hasta_Bimestre_Periodo.SelectedValue + "/" + Cmb_Hasta_Anio_Periodo.SelectedValue;
                                        // si el periodo no es igual que el seleccionado en los combos hasta periodo, reiniciar conteo y eliminar renglones despues del anticipo
                                        if (Ultima_Parcialidad_Incluida != Ultima_Parcialidad_Seleccionada)
                                        {
                                            // obtener el saldo del siguiente bimestre excluido
                                            // recorrer la tabla de adeudos
                                            foreach (DataRow Drw_Adeudo in Dt_Adeudos.Rows)
                                            {
                                                // si el año es igual que el año de la ultima parcialidad, sumar bimestres
                                                if (Drw_Adeudo["Anio"].ToString() == Ultima_Parcialidad_Incluida.Substring(2, 4))
                                                {
                                                    Int32 Ultimo_Bimestre_Incluido = Convert.ToInt32(Ultima_Parcialidad_Incluida.Substring(0, 1));
                                                    Int32 Indice_Fila = Dt_Adeudos.Rows.IndexOf(Drw_Adeudo);
                                                    // Agregar al Monto_A_Sumar el siguiente adeudo
                                                    Decimal Tmp_Monto_A_Sumar = Monto_A_Sumar;
                                                    while (Monto_A_Sumar <= Tmp_Monto_A_Sumar)
                                                    {
                                                        // si es en el mismo año, consultar el adeudo del siguiente bimestre
                                                        if (Ultimo_Bimestre_Incluido < 6)
                                                        {
                                                            if (Convert.ToDecimal(Dt_Adeudos.Rows[Indice_Fila]["ADEUDO_BIMESTRE_" + (Ultimo_Bimestre_Incluido)]) > 0)
                                                            {
                                                                // agregar el monto del siguiente bimestre
                                                                Monto_A_Sumar = Convert.ToDecimal(Drw_Adeudo["ADEUDO_BIMESTRE_" + (Ultimo_Bimestre_Incluido)]) * Multiplicador++;
                                                            }
                                                            else
                                                            {
                                                                Ultimo_Bimestre_Incluido++;
                                                            }
                                                        }
                                                        else
                                                        {
                                                            Ultimo_Bimestre_Incluido = 1;
                                                            // asignar utilizar la siguiente fila
                                                            Indice_Fila++;
                                                        }
                                                        // si ya se alcanzó la ultima fila, asignar valor al monto_A_Asignar
                                                        if (Indice_Fila >= Dt_Adeudos.Rows.Count)
                                                        {
                                                            Monto_A_Sumar += Monto_A_Sumar * Multiplicador++;
                                                        }
                                                    }
                                                    break;

                                                }
                                            }

                                            // reiniciar contador de parcialidades y parametros auxiliares
                                            Cont_Parcialidades = Bkp_Cont_Parcialidades - 1;
                                            Monto_Honorarios = Bkp_Monto_Honorarios;
                                            Monto_Recargos = Bkp_Monto_Recargos;
                                            Monto_Moratorios = Bkp_Monto_Moratorios;
                                            Anio_Aux = Bkp_Anio_Aux;
                                            Bimestre_Aux = Bkp_Bimestre_aux;
                                            Fecha_Parcialidad = Bkp_Fecha_Parcialidad;
                                            Dt_Parcialidades = Bkp_Dt_Parcial.Copy();

                                        }
                                    }
                                }
                            }
                            try
                            {
                                Sumar_Totales_Parcialidades();
                            }
                            catch { }
                        }
                    }
                    else
                    {
                        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de Predial", "alert('Imposible realizar las parcialidades debido a que no contiene adeudos');", true);
                    }

                } // calidacion de Dt_Adeudos con mas de cero filas
            } // validacion de Dt_adeudos no nulo

            else
            {
                Grid_Parcialidades_Editable = false;
                Grid_Parcialidades.DataSource = null;
                Grid_Parcialidades.DataBind();
            }
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Leer_Parametros_Parcialidades_Hasta_No_Pago
    ///DESCRIPCIÓN          : Lee el grid de las parcialidades y los valores que contiene 
    ///                         hasta la parcialidad indicada como parametro 
    ///PARAMETROS: 
    ///                     
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 28-oct-2011
    ///MODIFICO             :
    ///FECHA_MODIFICO       :
    ///CAUSA_MODIFICACIÓN   :
    ///*******************************************************************************
    protected void Leer_Parametros_Parcialidades_Hasta_No_Pago(String No_Pago, Decimal Nuevo_Monto)
    {
        DataTable Dt_Parcialidades = Crear_Tabla_Parcialidades();

        DateTime Fecha_Parcialidad = DateTime.Now;
        Decimal Total_Honorarios = 0;
        Decimal Total_Recargos = 0;
        Decimal Total_Moratorios = 0;
        Decimal Total_Impuesto = 0;
        Int32 Numero_Parcialidad = 0;
        Decimal Importe_Total = 0;
        Int32 Anio_Aux = 0;
        Int32 Bimestre_Aux = 0;
        Label Lbl_Txt_Grid_Periodo;
        Label Lbl_txt_Monto_Grid;
        string Periodo = "";

        DataRow Dr_Parcialidades;
        //Se recorre el Grid para cargar el DataTable con los valores del grid
        foreach (GridViewRow Row in Grid_Parcialidades.Rows)
        {
            Decimal Monto_Honorarios = 0;
            Decimal Monto_Recargos = 0;
            Decimal Monto_Moratorios = 0;
            Decimal Monto_Impuesto = 0;
            Decimal Importe = 0;

            // convertir valores del grid
            Int32.TryParse(Row.Cells[0].Text, out Numero_Parcialidad);
            // recuperar honorarios de la etiqueta correspondiente
            Lbl_txt_Monto_Grid = (Label)Row.Cells[2].FindControl("Lbl_Txt_Grid_Monto_Honorarios");
            if (Lbl_txt_Monto_Grid != null)
                Decimal.TryParse(Lbl_txt_Monto_Grid.Text.Replace("$", ""), out Monto_Honorarios);
            // recuperar recargos de la etiqueta correspondiente
            Lbl_txt_Monto_Grid = (Label)Row.Cells[3].FindControl("Lbl_Txt_Grid_Monto_Recargos_Ordinarios");
            if (Lbl_txt_Monto_Grid != null)
                Decimal.TryParse(Lbl_txt_Monto_Grid.Text.Replace("$", ""), out Monto_Recargos);
            // recuperar recargos moratorios de la etiqueta correspondiente
            Lbl_txt_Monto_Grid = (Label)Row.Cells[4].FindControl("Lbl_Txt_Grid_Monto_Recargos_Moratorios");
            if (Lbl_txt_Monto_Grid != null)
                Decimal.TryParse(Lbl_txt_Monto_Grid.Text.Replace("$", ""), out Monto_Moratorios);

            Decimal.TryParse(Row.Cells[5].Text.Replace("$", ""), out Monto_Impuesto);
            Importe = Monto_Honorarios + Monto_Recargos + Monto_Moratorios + Monto_Impuesto;

            // recuperar etiqueta con el Periodo
            Lbl_Txt_Grid_Periodo = (Label)Row.Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
            if (Lbl_Txt_Grid_Periodo != null)
                Periodo = HttpUtility.HtmlDecode(Lbl_Txt_Grid_Periodo.Text);

            // si no es el numero de pago recibido como parametro, agregar a la tabla
            if (Row.Cells[0].Text != No_Pago)
            {
                Importe_Total += Importe;
                // sumar valores para formar totales
                Total_Honorarios += Monto_Honorarios;
                Total_Recargos += Monto_Recargos;
                Total_Moratorios += Monto_Moratorios;
                Total_Impuesto += Monto_Impuesto;
                // agregar valores a la tabla
                Dr_Parcialidades = Dt_Parcialidades.NewRow();
                Dr_Parcialidades["NO_PAGO"] = Row.Cells[0].Text;
                Dr_Parcialidades["MONTO_HONORARIOS"] = Monto_Honorarios;
                Dr_Parcialidades["RECARGOS_ORDINARIOS"] = Monto_Recargos;
                Dr_Parcialidades["RECARGOS_MORATORIOS"] = Monto_Moratorios;
                Dr_Parcialidades["MONTO_IMPUESTO"] = Monto_Impuesto;
                Dr_Parcialidades["MONTO_IMPORTE"] = Importe;
                Dr_Parcialidades["FECHA_VENCIMIENTO"] = Row.Cells[7].Text;
                Dr_Parcialidades["ESTATUS"] = Row.Cells[8].Text;
                Dr_Parcialidades["PERIODO"] = Periodo;
                Dt_Parcialidades.Rows.Add(Dr_Parcialidades);
            }
            else
            {
                // validar que haya un periodo en la columna
                if (Periodo.Length >= 6)
                {
                    Int32.TryParse(Periodo.Substring(0, 1), out Bimestre_Aux);
                    Int32.TryParse(Periodo.Substring(2, 4), out Anio_Aux);
                }
                else // si no hay periodo en la columna modificada, tomar el primer adeudo
                {
                    for (int i = 0; i < Grid_Parcialidades.Rows.Count; i++)
                    {
                        Lbl_Txt_Grid_Periodo = (Label)Grid_Parcialidades.Rows[i].Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
                        if (Lbl_Txt_Grid_Periodo != null)
                        {
                            if (Lbl_Txt_Grid_Periodo.Text.Length >= 6)
                            {
                                Int32.TryParse(Lbl_Txt_Grid_Periodo.Text.Substring(0, 1), out Bimestre_Aux);
                                Int32.TryParse(Lbl_Txt_Grid_Periodo.Text.Substring(2, 4), out Anio_Aux);
                                break;
                            }
                        }
                    }
                }

                DateTime.TryParse(Row.Cells[7].Text, out Fecha_Parcialidad);
                // salir del foreach al llegar al pago modificado
                break;
            }
        }

        Calcular_Parcialidades(
            Numero_Parcialidad,
            Dt_Parcialidades,
            Total_Honorarios,
            Total_Recargos,
            Total_Moratorios,
            Total_Impuesto,
            Importe_Total,
            Anio_Aux,
            Bimestre_Aux,
            Fecha_Parcialidad,
            Nuevo_Monto
            );
    }


    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Componentes
    ///DESCRIPCIÓN          : Hace una validacion de que haya datos en los componentes antes de hacer una operación.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private bool Validar_Componentes()
    {
        Boolean Validacion = true;
        DateTime Fecha_Convenio;
        decimal Monto_Importe;
        decimal Subtotal;
        Label Etiqueta_Importe;

        if (Txt_Adeudo_Honorarios.Text == "")
        {
            Txt_Adeudo_Honorarios.Text = "0.00";
        }
        if ((Convert.ToDouble(Txt_Monto_Recargos.Text) + Convert.ToDouble(Txt_Monto_Moratorios.Text) + Convert.ToDouble(Hdn_Monto_Impuesto.Value) + Convert.ToDouble(Txt_Adeudo_Honorarios.Text)) != 0.00)
        {
            Lbl_Mensaje_Error.Text = "Es necesario.";
            Lbl_Mensaje_Error.Visible = false;
            Img_Error.Visible = false;

            String Mensaje_Error = "";
            if (Hdf_Cuenta_Predial_ID.Value.Trim() == "" && Txt_Cuenta_Predial.Text != "")
            {
                Consultar_Datos_Cuenta_Predial();
            }
            if (Txt_Cuenta_Predial.Text.Trim().Equals(""))
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Indique la Cuenta Predial.";
                Validacion = false;
            }
            if (Txt_Numero_Parcialidades.Text.Trim() == "")
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Introduzca el Número de Parcialidades.";
                Validacion = false;
            }
            if (Cmb_Periodicidad_Pago.SelectedIndex <= 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Seleccione un Periodo de Pago.";
                Validacion = false;
            }
            if (Cmb_Estatus.SelectedIndex <= 0)
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Indique un Estatus.";
                Validacion = false;
            }
            if (Cmb_Estatus.SelectedValue == "TERMINADO")
            {
                if (!Validacion) { Mensaje_Error += "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ No puede seleccionar el estatus TERMINADO.";
                Validacion = false;
            }
            if (Cmb_Estatus.SelectedValue == "INCUMPLIDO")
            {
                if (!Validacion) { Mensaje_Error += "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ No puede seleccionar el estatus INCUMPLIDO.";
                Validacion = false;
            }
            if (Txt_Solicitante.Text.Trim() == "")         // validar NOMBRE SOLICITANTE
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Indique el nombre del solicitante.";
                Validacion = false;
            }
            if (Cmb_Tipo_Solicitante.SelectedItem.Text == "DEUDOR SOLIDARIO" && Txt_RFC.Text.Trim() == "") // validar RFC SOLICITANTE
            {
                if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Indique el RFC del solicitante.";
                Validacion = false;
            }
            if (Btn_Nuevo.ToolTip != "Dar de Alta" && Btn_Modificar.ToolTip == "Actualizar")
            {
                if (Txt_Observaciones.Text.Equals(""))
                {
                    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Introduzca las Observaciones.";
                    Validacion = false;
                }
            }
            // validar FECHA
            if (!DateTime.TryParse(Txt_Fecha_Convenio.Text, out Fecha_Convenio))
            {
                if (!Validacion) { Mensaje_Error += "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ Ingresar una fecha de convenio válida (dd/MMM/aaaa).";
                Validacion = false;
            }

            // validar que todas las parcialidades tengan importe mayor que cero
            foreach (GridViewRow Fila_Parcialidad in Grid_Parcialidades.Rows)
            {
                Etiqueta_Importe = (Label)Fila_Parcialidad.Cells[6].FindControl("Lbl_Txt_Grid_Monto_Importe");
                if (!decimal.TryParse(Etiqueta_Importe.Text.Replace("$", ""), out Monto_Importe) || Monto_Importe == 0)
                {
                    if (!Validacion) { Mensaje_Error += "<br>"; }
                    Mensaje_Error = Mensaje_Error + "+ Todas las parcialidades deben tener un importe mayor que cero.";
                    Validacion = false;
                    break;
                }
            }

            // validar Importe contra subtotal
            decimal.TryParse(Txt_Sub_Total.Text, out Subtotal);
            decimal.TryParse(Grid_Parcialidades.FooterRow.Cells[6].Text.Replace("$", ""), out Monto_Importe);
            if (Monto_Importe != Subtotal)
            {
                if (!Validacion) { Mensaje_Error += "<br>"; }
                Mensaje_Error = Mensaje_Error + "+ El importe de las parcialidades no coincide con el total a convenir.";
                Validacion = false;
            }

            if (!Validacion)
            {
                Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        else
        {
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de Predial", "alert('Imposible realizar un convenio a esta cuenta debido a que no contiene adeudos');", true);
            Validacion = false;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Validar_Estatus_Parcialidades
    ///DESCRIPCIÓN          : Itera el grid de Parcialidades para devolver un True 
    ///                 cuando encuentre un estatus con el valor indicado en el parámetro.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benvides Guardado
    ///FECHA_CREO           : 23/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Estatus_Parcialidades(String Tipo_Estatus)
    {
        Boolean Validado = false;
        foreach (GridViewRow Fila_Grid in Grid_Parcialidades.Rows)
        {
            if (Fila_Grid.Cells[7].Text == Tipo_Estatus)
            {
                Validado = true;
                break;
            }
        }
        return Validado;
    }

    #endregion Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Bajar_Archivo
    ///DESCRIPCIÓN          : Mostrar control para subir convenio escaneado y si ya 
    ///                     hay un archivo, ofrecer para descarga
    ///PARAMETROS:
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 14-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Bajar_Archivo()
    {
        Cls_Ope_Pre_Convenios_Predial_Negocio Convenios_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        DataTable Dt_Convenios;
        String Pagina = "";

        try
        {
            // consultar datos del convenio
            Convenios_Predial.P_No_Convenio = Hdf_No_Convenio.Value;
            Dt_Convenios = Convenios_Predial.Consultar_Estatus_Archivo_Convenio();

            // verificar que se obtuvieron datos de la consulta
            if (Dt_Convenios != null && Dt_Convenios.Rows.Count > 0)
            {
                // validar estatus del convenio
                if (Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_Estatus].ToString() == "ACTIVO")
                {
                    // si hay una ruta de archivo, ofrecer archivo para descarga
                    if (Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_Ruta_Convenio_Escaneado].ToString() != "")
                    {
                        String Nombre_Archivo = Dt_Convenios.Rows[0][Ope_Pre_Convenios_Predial.Campo_Ruta_Convenio_Escaneado].ToString();
                        // comprobar que el archivo existe fisicamente antes de ofrecer descarga
                        if (File.Exists(Server.MapPath(Nombre_Archivo)))
                        {
                            // copiar el archivo a Reportes para que Mostrar_Reporte lo pueda localizar
                            File.Copy(Server.MapPath(Nombre_Archivo), Server.MapPath("~/Reporte/" + Path.GetFileName(Nombre_Archivo)));
                            Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
                            Pagina += Path.GetFileName(Nombre_Archivo);
                            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(
                                this,
                                this.GetType(),
                                "Convenio_Predial",
                                "window.open('" + Pagina +
                                "', '" + "Convenio" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                                true
                                );
                        }
                    }
                    // mostrar controles para subir archivo
                    Contenedor_Subir_Convenio.Style.Clear();
                    Contenedor_Subir_Convenio.Style.Add("display", "inline");
                }
                else    // en caso de no estar activo, mostrar mensaje
                {
                    Lbl_Mensaje_Error.Text = "Ya no es posible subir ni descargar archivo, el convenio ya no esta ACTIVO.";
                    Lbl_Mensaje_Error.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Bajar_Archivo: " + ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Guardar_Archivo
    ///DESCRIPCIÓN          : Guardar archivo seleccionado para envio
    ///PARAMETROS:
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 14-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Guardar_Archivo()
    {
        String Ruta_Guardar = Server.MapPath("~/paginas/Convenios/Predial/");
        String Nombre_Archivo = "";
        String Extension = "";

        Cls_Ope_Pre_Convenios_Predial_Negocio Convenios_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();

        Extension = Path.GetExtension(Fup_Subir_Convenio_Predial.FileName);
        // formar el nombre de archivo a guardar con el numero de convenio y la extension del archivo subido
        Nombre_Archivo = "CPRE" + Convert.ToInt32(Hdf_No_Convenio.Value) + Extension;

        try
        {
            // si el directorio en el que se va a guardar el archivo no existe, crearlo
            if (!Directory.Exists(Ruta_Guardar))
            {
                Directory.CreateDirectory(Ruta_Guardar);
            }

            // validar extension de archivo
            if (Extension == ".doc" || Extension == "docx" || Extension == "ppt" || Extension == ".pptx" || Extension == ".pdf")
            {
                // guardar archivo
                Fup_Subir_Convenio_Predial.SaveAs(Ruta_Guardar + Nombre_Archivo);

                // guardar ruta actualizando el convenio
                Convenios_Predial.P_No_Convenio = Hdf_No_Convenio.Value;
                Convenios_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Convenios_Predial.P_Ruta_Convenio_Escaneado = "~/paginas/Convenios/Predial/CPRE" + Convert.ToInt32(Hdf_No_Convenio.Value) + Extension;
                Convenios_Predial.Actualizar_Ruta_Convenio_Escaneado();

                // ocultar el contenedor para subir archivo
                Contenedor_Subir_Convenio.Style.Clear();
                Contenedor_Subir_Convenio.Style.Add("display", "none");

                // mostrar mensaje de que se guardo el archivo
                AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(),
                    "Convenios de impuesto predial", "alert('El archivo se guardó de forma exitosa.');", true);
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "No se permite subir archivos con extensión " + Extension;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Guardar_Archivo: " + ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Habilitar_Parcialidades_Manuales
    ///DESCRIPCIÓN          : Cargar todos los adeudos en la primer parcialidad
    ///PARAMETROS:
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Habilitar_Parcialidades_Manuales()
    {
        int Numero_Parcialidades;
        DateTime Fecha_Vencimiento;
        DateTime Fecha_Convenio;
        DataRow Dr_Parcialidad;
        int Hasta_Anio_Seleccionado;
        int Hasta_Bimestre_Seleccionado;
        decimal Monto_Adeudo;
        decimal Monto_Rezago;
        decimal Importe = 0;
        decimal Descuento_Recargos_Ordinarios;
        decimal Descuento_Moratorios;
        string Periodo = "";
        var Consulta_Adeudos_Predial = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Parcialidades = Formar_Tabla_Parcialidades();

        // sesión con diccionario de adeudos por bimestre
        Session["Dic_Adeudos"] = Consulta_Adeudos_Diccionario();

        int.TryParse(Cmb_Hasta_Anio_Periodo.SelectedValue, out Hasta_Anio_Seleccionado);
        int.TryParse(Cmb_Hasta_Bimestre_Periodo.SelectedValue, out Hasta_Bimestre_Seleccionado);

        // consultar adeudos y periodo
        Consulta_Adeudos_Predial.Calcular_Recargos_Predial_Hasta_Bimestre(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio_Seleccionado, Hasta_Bimestre_Seleccionado);

        // obtener bimiestre inicial, primero intentar del periodo rezago, si no, del periodo corriente
        if (Consulta_Adeudos_Predial.p_Periodo_Rezago.Length > 6)
        {
            Periodo = Consulta_Adeudos_Predial.p_Periodo_Rezago.Substring(1, 6).Replace("-", "/");
        }
        else if (Consulta_Adeudos_Predial.p_Periodo_Corriente.Length > 6)
        {
            Periodo = Consulta_Adeudos_Predial.p_Periodo_Corriente.Substring(1, 6).Replace("-", "/");
        }
        // obtener bimestre final del periodo, primero del adeudo corriente, si no, del periodo rezago
        if (Consulta_Adeudos_Predial.p_Periodo_Corriente.Length > 15)
        {
            Periodo += "-" + Consulta_Adeudos_Predial.p_Periodo_Corriente.Substring(Consulta_Adeudos_Predial.p_Periodo_Corriente.Length - 6, 6).Replace("-", "/");
        }
        else if (Consulta_Adeudos_Predial.p_Periodo_Rezago.Length > 15)
        {
            Periodo += "-" + Consulta_Adeudos_Predial.p_Periodo_Rezago.Substring(Consulta_Adeudos_Predial.p_Periodo_Rezago.Length - 6, 6).Replace("-", "/");
        }

        // obtener la fecha de convenio para la primer parcialidad, si no se obtiene valor, tomar fecha actual
        if (!DateTime.TryParse(Txt_Fecha_Convenio.Text, out Fecha_Vencimiento))
        {
            Fecha_Vencimiento = DateTime.Now;
            Txt_Fecha_Convenio.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
        }
        Fecha_Convenio = Fecha_Vencimiento;

        int.TryParse(Txt_Numero_Parcialidades.Text, out Numero_Parcialidades);
        // si el número de parcialidades es igual al número de filas en el grid, tomar fechas del grid
        if (Numero_Parcialidades >= 0 && Numero_Parcialidades == Grid_Parcialidades.Rows.Count)
        {
            // recorrer cada fila del grid para pasar fechas a la tabla 
            foreach (GridViewRow Fila_Parcialidad in Grid_Parcialidades.Rows)
            {
                Dr_Parcialidad = Dt_Parcialidades.NewRow();
                Dr_Parcialidad["NO_PAGO"] = Fila_Parcialidad.Cells[0].Text;
                if (Dt_Parcialidades.Rows.Count > 0)
                {
                    Dr_Parcialidad["PERIODO"] = "-";
                    Dr_Parcialidad["MONTO_HONORARIOS"] = 0;
                    Dr_Parcialidad["RECARGOS_ORDINARIOS"] = 0;
                    Dr_Parcialidad["RECARGOS_MORATORIOS"] = 0;
                    Dr_Parcialidad["MONTO_IMPUESTO"] = 0;
                    Dr_Parcialidad["MONTO_IMPORTE"] = 0;
                }
                else
                {
                    Dr_Parcialidad["PERIODO"] = Periodo;
                    decimal.TryParse(Txt_Adeudo_Honorarios.Text, out Monto_Adeudo);
                    Importe += Monto_Adeudo;
                    Dr_Parcialidad["MONTO_HONORARIOS"] = Monto_Adeudo;

                    decimal.TryParse(Txt_Monto_Recargos.Text, out Monto_Adeudo);
                    decimal.TryParse(Txt_Descuento_Recargos_Ordinarios.Text, out Descuento_Recargos_Ordinarios);
                    Monto_Adeudo -= Descuento_Recargos_Ordinarios;
                    Importe += Monto_Adeudo;
                    Dr_Parcialidad["RECARGOS_ORDINARIOS"] = Monto_Adeudo;

                    decimal.TryParse(Txt_Monto_Moratorios.Text, out Monto_Adeudo);
                    decimal.TryParse(Txt_Descuento_Recargos_Moratorios.Text, out Descuento_Moratorios);
                    Monto_Adeudo -= Descuento_Moratorios;
                    Importe += Monto_Adeudo;
                    Dr_Parcialidad["RECARGOS_MORATORIOS"] = Monto_Adeudo;

                    decimal.TryParse(Txt_Adeudo_Corriente.Text, out Monto_Adeudo);
                    decimal.TryParse(Txt_Adeudo_Rezago.Text, out Monto_Rezago);
                    Importe += Monto_Adeudo + Monto_Rezago;
                    Dr_Parcialidad["MONTO_IMPUESTO"] = Monto_Adeudo + Monto_Rezago;
                    Dr_Parcialidad["MONTO_IMPORTE"] = Importe;
                }
                Dr_Parcialidad["FECHA_VENCIMIENTO"] = Convert.ToDateTime(Fila_Parcialidad.Cells[7].Text);
                Dr_Parcialidad["ESTATUS"] = "POR PAGAR";

                Dt_Parcialidades.Rows.Add(Dr_Parcialidad);
            }
        }
        else if (Numero_Parcialidades >= 0) // generar la tabla a partir de los datos en la pagina (fecha convenio)
        {
            for (int Contador_Parcialidades = 0; Contador_Parcialidades < Numero_Parcialidades; Contador_Parcialidades++)
            {
                Dr_Parcialidad = Dt_Parcialidades.NewRow();
                Dr_Parcialidad["NO_PAGO"] = (Contador_Parcialidades + 1).ToString();
                if (Dt_Parcialidades.Rows.Count > 0)
                {
                    Dr_Parcialidad["PERIODO"] = "-";
                    Dr_Parcialidad["MONTO_HONORARIOS"] = 0;
                    Dr_Parcialidad["RECARGOS_ORDINARIOS"] = 0;
                    Dr_Parcialidad["RECARGOS_MORATORIOS"] = 0;
                    Dr_Parcialidad["MONTO_IMPUESTO"] = 0;
                    Dr_Parcialidad["MONTO_IMPORTE"] = 0;
                    Fecha_Vencimiento = Obtener_Fecha_Periodo(Fecha_Convenio, Cmb_Periodicidad_Pago.SelectedValue, Contador_Parcialidades);
                    Dr_Parcialidad["FECHA_VENCIMIENTO"] = Fecha_Vencimiento;
                }
                else
                {
                    Dr_Parcialidad["PERIODO"] = Periodo;

                    decimal.TryParse(Txt_Adeudo_Honorarios.Text, out Monto_Adeudo);
                    Importe += Monto_Adeudo;
                    Dr_Parcialidad["MONTO_HONORARIOS"] = Monto_Adeudo;

                    decimal.TryParse(Txt_Monto_Recargos.Text, out Monto_Adeudo);
                    decimal.TryParse(Txt_Descuento_Recargos_Ordinarios.Text, out Descuento_Recargos_Ordinarios);
                    Monto_Adeudo -= Descuento_Recargos_Ordinarios;
                    Importe += Monto_Adeudo;
                    Dr_Parcialidad["RECARGOS_ORDINARIOS"] = Monto_Adeudo;

                    decimal.TryParse(Txt_Monto_Moratorios.Text, out Monto_Adeudo);
                    decimal.TryParse(Txt_Descuento_Recargos_Moratorios.Text, out Descuento_Moratorios);
                    Monto_Adeudo -= Descuento_Moratorios;
                    Importe += Monto_Adeudo;
                    Dr_Parcialidad["RECARGOS_MORATORIOS"] = Monto_Adeudo;

                    decimal.TryParse(Txt_Adeudo_Corriente.Text, out Monto_Adeudo);
                    decimal.TryParse(Txt_Adeudo_Rezago.Text, out Monto_Rezago);
                    Importe += Monto_Adeudo + Monto_Rezago;

                    Dr_Parcialidad["MONTO_IMPUESTO"] = Monto_Adeudo + Monto_Rezago;
                    Dr_Parcialidad["MONTO_IMPORTE"] = Importe;
                    Dr_Parcialidad["FECHA_VENCIMIENTO"] = Fecha_Vencimiento;
                }
                Dr_Parcialidad["ESTATUS"] = "POR PAGAR";

                Dt_Parcialidades.Rows.Add(Dr_Parcialidad);
            }
        }

        // ya no se puede editar el importe
        Grid_Parcialidades_Editable = false;

        // cargar datos en el grid
        Grid_Parcialidades.DataSource = Dt_Parcialidades;
        Grid_Parcialidades.DataBind();


        Sumar_Totales_Parcialidades();
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Actualizar_Fechas_Periodo
    ///DESCRIPCIÓN          : Recorre el grid de parcialidades y actualiza la fecha de 
    ///                       vencimiento volviendola a calcular
    ///PARAMETROS:
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 18-abr-2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Actualizar_Fechas_Periodo()
    {
        int Numero_Parcialidades;
        DateTime Fecha_Vencimiento;
        DateTime Fecha_Convenio;

        // obtener la fecha de convenio para la primer parcialidad, si no se obtiene valor, tomar fecha actual
        if (!DateTime.TryParse(Txt_Fecha_Convenio.Text, out Fecha_Vencimiento))
        {
            Fecha_Vencimiento = DateTime.Now;
            Txt_Fecha_Convenio.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
        }
        Fecha_Convenio = Fecha_Vencimiento;

        int.TryParse(Txt_Numero_Parcialidades.Text, out Numero_Parcialidades);
        // si el número de parcialidades es igual al número de filas en el grid, tomar fechas del grid
        if (Numero_Parcialidades >= 0 && Cmb_Periodicidad_Pago.SelectedIndex > 0)
        {
            // recorrer cada fila del grid para pasar fechas a la tabla 
            for (int Contador_Fila = 0; Contador_Fila < Grid_Parcialidades.Rows.Count; Contador_Fila++)
            {
                if (Contador_Fila == 0)
                {
                    Grid_Parcialidades.Rows[Contador_Fila].Cells[7].Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                }
                else
                {
                    Fecha_Vencimiento = Obtener_Fecha_Periodo(Fecha_Convenio, Cmb_Periodicidad_Pago.SelectedValue, Contador_Fila);
                    Grid_Parcialidades.Rows[Contador_Fila].Cells[7].Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                }
            }
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Formar_Tabla_Parcialidades
    ///DESCRIPCIÓN          : Regresa un datatable con la estructura de la tabla parcialidades
    ///PARAMETROS:
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private DataTable Formar_Tabla_Parcialidades()
    {
        DataTable Dt_Parcialidades = new DataTable();

        Dt_Parcialidades.Columns.Add("NO_PAGO", typeof(string));
        Dt_Parcialidades.Columns.Add("PERIODO", typeof(string));
        Dt_Parcialidades.Columns.Add("MONTO_HONORARIOS", typeof(decimal));
        Dt_Parcialidades.Columns.Add("RECARGOS_ORDINARIOS", typeof(decimal));
        Dt_Parcialidades.Columns.Add("RECARGOS_MORATORIOS", typeof(decimal));
        Dt_Parcialidades.Columns.Add("MONTO_IMPUESTO", typeof(decimal));
        Dt_Parcialidades.Columns.Add("MONTO_IMPORTE", typeof(decimal));
        Dt_Parcialidades.Columns.Add("FECHA_VENCIMIENTO", typeof(DateTime));
        Dt_Parcialidades.Columns.Add("ESTATUS", typeof(string));

        return Dt_Parcialidades;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: obtener_Recargos_Moratorios_Reestructura_Anterior
    /// DESCRIPCIÓN: Leer las parcialidades del convenio o reestructura anterior para obtener los adeudos
    ///            a tomar en cuenta para recalular los recargosmoratorios
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 24-mar-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void obtener_Recargos_Moratorios_Reestructura_Anterior()
    {
        var Consulta_Parcialidades = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        var Consulta_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        DataTable Dt_Parcialidades;
        Decimal Recargos_Moratorios = 0;
        Decimal Recargos_Ordinarios = 0;
        Decimal Honorarios = 0;
        Decimal Monto_Impuesto = 0;
        Decimal Monto_Base = 0;
        Decimal Adeudo_Honorarios = 0;
        Decimal Adeudo_Recargos = 0;
        Decimal Adeudo_Moratorios = 0;
        int Parcialidad = 0;
        DateTime Fecha_Vencimiento = DateTime.MinValue;
        int Meses_Transcurridos = 0;
        DataTable Dt_Honorarios;

        int Hasta_Anio = 0;
        int Hasta_Bimestre = 0;

        // consultar las parcialidades del ultimo convenio guardado (convenio o ultima reestructura)
        Consulta_Parcialidades.P_No_Convenio = Hdf_No_Convenio.Value;
        Consulta_Parcialidades.P_Filtros_Dinamicos = "-1";
        Consulta_Parcialidades.P_Ordenar_Dinamico = Ope_Pre_Detalles_Convenios_Predial.Campo_No_Pago;
        Dt_Parcialidades = Consulta_Parcialidades.Consultar_Parcialidades_Ultimo_Convenio();

        Parcialidad = Dt_Parcialidades.Rows.Count - 1;
        // recorrer la tabla de parcialidades hasta encontrar parcialidades con estatus PAGADO
        while (Parcialidad >= 0)
        {
            // obtener el ultimo periodo incluido en el convenio
            if (Hasta_Anio <= 0 || Hasta_Bimestre <= 0)
            {
                string Periodo_Parcialidad = Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString();
                if (Periodo_Parcialidad.Trim().Length >= 13)
                {
                    int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 4, 4), out Hasta_Anio);
                    int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 6, 1), out Hasta_Bimestre);
                }
            }

            // si la parcialidad tiene estatus CANCELADO, sumar adeudos
            if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() == "CANCELADO")
            {
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString(), out Honorarios);
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString(), out Recargos_Ordinarios);
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString(), out Recargos_Moratorios);
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString(), out Monto_Impuesto);
                DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                Adeudo_Honorarios += Honorarios;
                Adeudo_Recargos += Recargos_Ordinarios;
                Adeudo_Moratorios += Recargos_Moratorios;
                Monto_Base += Monto_Impuesto;
            }
            Parcialidad--;
        }

        // cargar adeudos de la cuenta (para obtener el adeudo total)
        Cargar_Adeudos_Cuenta(Hasta_Anio, Hasta_Bimestre);

        // restar adeudos de bimestres que no han vencido (si el año es mayor al actual o es igual con el bimestre mayor al actual)
        if (Hasta_Anio >= DateTime.Now.Year || (Hasta_Anio == DateTime.Now.Year && Hasta_Bimestre >= DateTime.Now.Month / 2))
        {
            Monto_Base -= Adeudos_Predial_Sin_Vencer(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre);
        }

        // agregar adeudos vencidos despues de convenio
        Monto_Base += Adeudos_Predial_Actuales_Despues_Convenio(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre);

        // calcular moratorios a partir del monto base y meses transcurridos
        Meses_Transcurridos = Calcular_Meses_Entre_Fechas(Fecha_Vencimiento, DateTime.Now);
        Recargos_Moratorios = Calcular_Recargos_Moratorios(Monto_Base, Meses_Transcurridos);

        Txt_Adeudo_Honorarios.Text = Math.Round(Adeudo_Honorarios, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
        Txt_Monto_Recargos.Text = Math.Round(Adeudo_Recargos, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
        Txt_Monto_Moratorios.Text = Math.Round(Recargos_Moratorios + Adeudo_Moratorios, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");

        // consulta de honorarios
        Consulta_Honorarios.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Honorarios = Consulta_Honorarios.Consultar_Total_Honorarios();
        if (Dt_Honorarios != null && Dt_Honorarios.Rows.Count > 0)
        {
            decimal Total_Adeudo_Honorarios;
            decimal.TryParse(Dt_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString(), out Total_Adeudo_Honorarios);
            Txt_Adeudo_Honorarios.Text = Total_Adeudo_Honorarios.ToString("#,##0.00");
        }
        else
        {
            Txt_Adeudo_Honorarios.Text = Math.Round(Adeudo_Honorarios, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
        }

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: obtener_Recargos_Moratorios
    /// DESCRIPCIÓN: Leer las parcialidades del ultimo convenio o reestructura para obtener los adeudos
    ///            a tomar en cuenta para la reestructura
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-nov-2011
    /// MODIFICÓ: Roberto González Oseguera
    /// FECHA_MODIFICÓ: 14/dic/2011
    /// CAUSA_MODIFICACIÓN: Cambio en la forma de calcular recargos moratorios (solo sobre el impuesto)
    ///             y calculando por mes vencido
    ///*******************************************************************************************************
    private void obtener_Recargos_Moratorios()
    {
        var Consulta_Parcialidades = new Cls_Ope_Pre_Convenios_Predial_Negocio();
        var Consulta_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        DataTable Dt_Parcialidades;
        Decimal Recargos_Moratorios = 0;
        Decimal Recargos_Ordinarios = 0;
        Decimal Honorarios = 0;
        Decimal Monto_Impuesto = 0;
        Decimal Monto_Base = 0;
        Decimal Adeudo_Honorarios = 0;
        Decimal Adeudo_Recargos = 0;
        Decimal Adeudo_Moratorios = 0;
        int Parcialidad = 0;
        DateTime Fecha_Vencimiento = DateTime.MinValue;
        int Meses_Transcurridos = 0;
        DataTable Dt_Honorarios;

        int Hasta_Anio = 0;
        int Hasta_Bimestre = 0;

        // consultar las parcialidades del ultimo convenio guardado (convenio o ultima reestructura)
        Consulta_Parcialidades.P_No_Convenio = Hdf_No_Convenio.Value;
        Dt_Parcialidades = Consulta_Parcialidades.Consultar_Parcialidades_Ultimo_Convenio();

        Parcialidad = Dt_Parcialidades.Rows.Count - 1;
        // recorrer la tabla de parcialidades hasta encontrar parcialidades con estatus PAGADO
        while (Parcialidad >= 0)
        {
            // obtener el ultimo periodo incluido en el convenio
            if (Hasta_Anio <= 0 || Hasta_Bimestre <= 0)
            {
                string Periodo_Parcialidad = Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString();
                if (Periodo_Parcialidad.Trim().Length >= 13)
                {
                    int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 4, 4), out Hasta_Anio);
                    int.TryParse(Periodo_Parcialidad.Substring(Periodo_Parcialidad.Trim().Length - 6, 1), out Hasta_Bimestre);
                }
            }

            // si la parcialidad tiene estatus diferente de PAGADO, sumar adeudos
            if (Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Estatus].ToString() != "PAGADO")
            {
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Honorarios].ToString(), out Honorarios);
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Ordinarios].ToString(), out Recargos_Ordinarios);
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Recargos_Moratorios].ToString(), out Recargos_Moratorios);
                Decimal.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Monto_Impuesto].ToString(), out Monto_Impuesto);
                DateTime.TryParse(Dt_Parcialidades.Rows[Parcialidad][Ope_Pre_Detalles_Convenios_Predial.Campo_Fecha_Vencimiento].ToString(), out Fecha_Vencimiento);
                Adeudo_Honorarios += Honorarios;
                Adeudo_Recargos += Recargos_Ordinarios;
                Adeudo_Moratorios += Recargos_Moratorios;
                Monto_Base += Monto_Impuesto;
            }
            Parcialidad--;
        }

        // cargar adeudos de la cuenta (para obtener el adeudo total)
        Cargar_Adeudos_Cuenta(Hasta_Anio, Hasta_Bimestre);

        // restar adeudos de bimestres que no han vencido (si el año es mayor al actual o es igual con el bimestre mayor al actual)
        if (Hasta_Anio > DateTime.Now.Year || (Hasta_Anio == DateTime.Now.Year && Hasta_Bimestre >= DateTime.Now.Month / 2))
        {
            Monto_Base -= Adeudos_Predial_Sin_Vencer(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre);
        }

        // agregar adeudos vencidos despues de convenio
        Monto_Base += Adeudos_Predial_Actuales_Despues_Convenio(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre);

        // calcular moratorios a partir del monto base y meses transcurridos
        Meses_Transcurridos = Calcular_Meses_Entre_Fechas(Fecha_Vencimiento, DateTime.Now);
        Recargos_Moratorios = Calcular_Recargos_Moratorios(Monto_Base, Meses_Transcurridos);

        Txt_Adeudo_Honorarios.Text = Math.Round(Adeudo_Honorarios, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
        Txt_Monto_Recargos.Text = Math.Round(Adeudo_Recargos, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
        Txt_Monto_Moratorios.Text = Math.Round(Recargos_Moratorios + Adeudo_Moratorios, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
        // limpiar cajas de texto con adeudos y quitar selecciones para inicializar convenio
        Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
        Txt_Descuento_Recargos_Moratorios.Text = "0.00";
        Txt_Porcentaje_Anticipo.Text = "0.00";
        Txt_Total_Anticipo.Text = "0.00";
        Txt_Total_Convenio.Text = "0.00";
        Grid_Parcialidades.DataSource = null;
        Grid_Parcialidades.DataBind();
        Cmb_Periodicidad_Pago.SelectedIndex = -1;
        Txt_Numero_Parcialidades.Text = "";


        // consulta de honorarios
        Consulta_Honorarios.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Honorarios = Consulta_Honorarios.Consultar_Total_Honorarios();
        if (Dt_Honorarios != null && Dt_Honorarios.Rows.Count > 0)
        {
            decimal Total_Adeudo_Honorarios;
            decimal.TryParse(Dt_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString(), out Total_Adeudo_Honorarios);
            Txt_Adeudo_Honorarios.Text = Total_Adeudo_Honorarios.ToString("#,##0.00");
        }
        else
        {
            Txt_Adeudo_Honorarios.Text = Math.Round(Adeudo_Honorarios, 2, MidpointRounding.AwayFromZero).ToString("#,##0.00");
        }

        // volver a calcular total
        Calcular_Total_Adeudos();
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Adeudos_Predial_Actuales_Despues_Convenio
    /// DESCRIPCIÓN: Regresa la suma de los adeudos vencidos despues del periodo indicado como parametro
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial para consultar adeudos
    /// 		2. Desde_Anio: Año del periodo inicial a tomar
    /// 		3. Desde_Bimestre: bimestre del periodo inicial a tomar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 18-feb-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Adeudos_Predial_Actuales_Despues_Convenio(string Cuenta_Predial_ID, int Desde_Anio, int Desde_Bimestre)
    {
        decimal Adeudos_Despues_Convenio = 0;
        var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Adeudos;
        int Anio_Actual = DateTime.Now.Year;
        int Bimestre_Vencido;

        Bimestre_Vencido = DateTime.Now.Month % 2 == 0 ? DateTime.Now.Month / 2 : (DateTime.Now.Month / 2) + 1;

        // periodo a partir del cual se va a tomar en cuenta (desde_bimestre + 1)
        Desde_Bimestre++;
        if (Desde_Bimestre > 6)
        {
            Desde_Bimestre = 1;
            Desde_Anio++;
        }

        // consultar adeudos actuales de la cuenta
        Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial_ID, "POR PAGAR", 0, 0);
        // agregar adeudos vencidos a la fecha
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            // recorrer todas las filas de la tabla de adeudos
            for (int Contador_Filas = 0; Contador_Filas < Dt_Adeudos.Rows.Count; Contador_Filas++)
            {
                int Anio_Adeudo;
                int.TryParse(Dt_Adeudos.Rows[Contador_Filas][Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio_Adeudo);
                // si el año es menor que Desde_Anio pasar al siguiente adeudo
                if (Anio_Adeudo < Desde_Anio)
                {
                    continue;
                }
                // si el año del adeudo es igual al año desde el que se calculan los moratorios, agregar solo los adeudos despues del bimestre indicado
                else if (Anio_Adeudo == Desde_Anio)
                {
                    int Hasta_Bimestre = 6;
                    // si es el año actual, tomar hasta el bimestre vencido
                    if (Anio_Adeudo == Anio_Actual)
                        Hasta_Bimestre = Bimestre_Vencido;
                    // recorrer los bimestres para agregar adeudos
                    for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                    {
                        decimal Adeudo_Bimestre;
                        decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                        Adeudos_Despues_Convenio += Adeudo_Bimestre;
                    }
                }
                // si el año es mayor que el año especificado, agregar adeudos al monto total
                else if (Anio_Adeudo > Desde_Anio)
                {
                    int Hasta_Bimestre = 6;
                    // si es el año actual, tomar hasta el bimestre vencido
                    if (Anio_Adeudo == Anio_Actual)
                        Hasta_Bimestre = Bimestre_Vencido;
                    // recorrer los bimestres para agregar al adeudo
                    for (int Contador_Bimestres = 1; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                    {
                        decimal Adeudo_Bimestre;
                        decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                        Adeudos_Despues_Convenio += Adeudo_Bimestre;
                    }
                }

            } // for
        }

        return Adeudos_Despues_Convenio;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Adeudos_Predial_Sin_Vencer
    /// DESCRIPCIÓN: Regresa la suma de los adeudos aún no vencidos incluidos en el convenio
    ///             Consulta adeudos predial y suma los adeudos a partir del bimestre vencido actual
    /// PARÁMETROS:
    /// 		1. Cuenta_Predial_ID: id de la cuenta predial para consultar adeudos
    /// 		2. Hasta_Anio: año del último bimestre incluido en el convenio
    /// 		3. Ultimo_Bimestre: bimestre del último periodo incluido en el convenio
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Adeudos_Predial_Sin_Vencer(string Cuenta_Predial_ID, int Hasta_Anio, int Ultimo_Bimestre)
    {
        decimal Adeudos_Sin_Vencer = 0;
        var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Adeudos;
        int Anio_Actual = DateTime.Now.Year;
        int Bimestre_Vencido;

        Bimestre_Vencido = DateTime.Now.Month % 2 == 0 ? DateTime.Now.Month / 2 : (DateTime.Now.Month / 2) + 1;

        // consultar adeudos actuales de la cuenta
        Dt_Adeudos = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial(Cuenta_Predial_ID, "POR PAGAR", 0, 0);
        // validar que se obtuvieron datos de la consulta de adeudos
        if (Dt_Adeudos != null && Dt_Adeudos.Rows.Count > 0)
        {
            // recorrer todas las filas de la tabla de adeudos
            for (int Contador_Filas = 0; Contador_Filas < Dt_Adeudos.Rows.Count; Contador_Filas++)
            {
                int Anio_Adeudo;
                int.TryParse(Dt_Adeudos.Rows[Contador_Filas][Ope_Pre_Adeudos_Predial.Campo_Anio].ToString(), out Anio_Adeudo);
                // si el año es menor que Hasta_Anio pasar al siguiente adeudo
                if (Anio_Adeudo >= Anio_Actual)
                {
                    int Desde_Bimestre = 1;
                    int Hasta_Bimestre = 6;
                    if (Hasta_Anio == Anio_Actual)
                    {
                        Hasta_Bimestre = Ultimo_Bimestre;
                        Desde_Bimestre = Bimestre_Vencido + 1;
                        // recorrer los bimestres para agregar adeudos
                        for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                        {
                            decimal Adeudo_Bimestre;
                            decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                            Adeudos_Sin_Vencer += Adeudo_Bimestre;
                        }
                    }
                    else if (Hasta_Anio > Anio_Actual)
                    {
                        Hasta_Bimestre = Ultimo_Bimestre;
                        // recorrer los bimestres para agregar adeudos
                        for (int Contador_Bimestres = Desde_Bimestre; Contador_Bimestres <= Hasta_Bimestre; Contador_Bimestres++)
                        {
                            decimal Adeudo_Bimestre;
                            decimal.TryParse(Dt_Adeudos.Rows[Contador_Filas]["ADEUDO_BIMESTRE_" + Contador_Bimestres].ToString(), out Adeudo_Bimestre);
                            Adeudos_Sin_Vencer += Adeudo_Bimestre;
                        }
                    }
                }
            } // for
        }

        return Adeudos_Sin_Vencer;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Recargos_Moratorios
    /// DESCRIPCIÓN: Calcular los recargos moratorios para una cantidad a partir de una fecha dados
    ///             (el numero de meses por el porcentaje de recargos por el monto base)
    /// PARÁMETROS:
    /// 		1. Monto_Base: Cantidad a la que se van a calcular los recargos
    /// 		2. Meses: Numero de meses a considedar para el calculo de recargos 
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 21-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Recargos_Moratorios(Decimal Monto_Base, Int32 Meses)
    {
        Cls_Ope_Pre_Parametros_Negocio Parametros = new Cls_Ope_Pre_Parametros_Negocio();
        DataTable Dt_Parametros;
        Decimal Recargos_Moratorios = 0;
        Decimal Porcentaje_Recargos = 0;

        // recuperar el porcentaje de recargos moratorios de la tabla de parametros
        Dt_Parametros = Parametros.Consultar_Parametros();
        if (Dt_Parametros != null)
        {
            if (Dt_Parametros.Rows.Count > 0)
            {
                Decimal.TryParse(Dt_Parametros.Rows[0][Ope_Pre_Parametros.Campo_Recargas_Traslado].ToString(), out Porcentaje_Recargos);
            }
        }

        // obtener el producto de los meses por el porcentaje de recargos
        Porcentaje_Recargos *= Meses;

        // calcular recargos
        Recargos_Moratorios = Monto_Base * Porcentaje_Recargos / 100;

        return Recargos_Moratorios;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Tiempo_Entre_Fechas
    /// DESCRIPCIÓN: Calcular numero de dias y meses transcurridos entre una fecha y otra
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inferior a tomar
    /// 		2. Hasta_Fecha: Fecha final hasta la que se calcula
    /// 		3. Dias: Se almacenan los dias de diferencia entre las fechas
    /// 		4. Meses: Almacena los meses transcurridos entre una fecha y otra
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 12-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Tiempo_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha, out Int32 Dias, out Int32 Meses)
    {
        TimeSpan Transcurrido = Hasta_Fecha - Desde_Fecha;
        if (Transcurrido > TimeSpan.Parse("0"))
        {
            DateTime Tiempo = DateTime.MinValue + Transcurrido;
            Meses = ((Tiempo.Year - 1) * 12) + (Tiempo.Month - 1);

            long tickDiff = Hasta_Fecha.Ticks - Desde_Fecha.Ticks;
            tickDiff = tickDiff / 10000000; // segundos
            Dias = (int)(tickDiff / 86400);
        }
        else
        {
            Dias = 0;
            Meses = 0;
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Meses_Entre_Fechas
    /// DESCRIPCIÓN: Regresa un enteron con el numero de meses vencidos entre dos fechas
    ///             (tomando el primer dia de cada mes)
    /// PARÁMETROS:
    /// 		1. Desde_Fecha: Fecha inicial a comparar
    /// 		2. Hasta_Fecha: Fecha final a comparar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 05-dic-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Int32 Calcular_Meses_Entre_Fechas(DateTime Desde_Fecha, DateTime Hasta_Fecha)
    {
        DateTime Fecha_Inicial;
        DateTime Fecha_Final;
        int Meses = 0;

        // establecer fecha inicial como el primer día del mes en Desde_Fecha
        DateTime.TryParse(Desde_Fecha.ToString("01-MMM-yyyy"), out Fecha_Inicial);
        // tomar la fecha en Hasta_Fecha (ignorando la hora)
        DateTime.TryParse(Hasta_Fecha.ToString("dd-MMM-yyyy"), out Fecha_Final);

        // validar que se obtuvo una fecha inicial
        if (Fecha_Inicial != DateTime.MinValue)
        {
            // aumentar el numero de meses mientras la fecha inicial mas los meses no supere la fecha final
            while (Fecha_Final >= Fecha_Inicial.AddMonths(Meses))
            {
                Meses++;
            }
        }

        return Meses;
    }

    #endregion Metodos

    #region Grids

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_PageIndexChanging
    ///DESCRIPCIÓN          : Maneja la paginación del GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Grid_Convenios.SelectedIndex = (-1);
            Grid_Convenios.Columns[1].Visible = true;
            Grid_Convenios.Columns[2].Visible = true;
            Grid_Convenios.Columns[4].Visible = true;
            Grid_Convenios.Columns[8].Visible = true;
            Grid_Convenios.DataSource = (DataTable)Session["Dt_Consulta_Convenios"];
            Grid_Convenios.PageIndex = e.NewPageIndex;
            Grid_Convenios.DataBind();
            Grid_Convenios.Columns[1].Visible = false;
            Grid_Convenios.Columns[2].Visible = false;
            Grid_Convenios.Columns[4].Visible = false;
            Grid_Convenios.Columns[8].Visible = false;
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Convenios_SelectedIndexChanged
    ///DESCRIPCIÓN          : Maneja la selección de fila en el GridView
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 14/Agosto/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Convenios_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        if (Grid_Convenios.Rows.Count > 0)
        {
            Hdf_Cuenta_Predial_ID.Value = Grid_Convenios.DataKeys[Grid_Convenios.SelectedIndex].Value.ToString();
            Session["Cuenta_Predial_ID"] = Hdf_Cuenta_Predial_ID.Value;
            //Hdf_Propietario_ID.Value = Grid_Convenios.DataKeys[1].Value.ToString();
            Hdf_No_Convenio.Value = Grid_Convenios.SelectedRow.Cells[1].Text;
            Cargar_Convenio();
            Panel_Datos.Visible = true;
            //Txt_Cuenta_Predial_TextChanged();
            // ocultar grid y mostrar boton modificar e imprimir
            Grid_Convenios.Visible = false;
            Btn_Modificar.Visible = true;
            Btn_Imprimir.Visible = true;
            Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
            Btn_Detalles_Cuenta_Predial.Enabled = true;
            Btn_Desglose_Adeudos.Enabled = true;
            Txt_RFC.Enabled = false;
            Txt_Solicitante.Enabled = false;
        }
        Cargar_Ventana_Emergente_Resumen_Predio();
        Cargar_Ventana_Emergente_Desglose_Adeudos();
    }

    #endregion Grids

    #region Eventos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Nuevo_Click
    ///DESCRIPCIÓN          : Prepara los controle de la pagina para dar de Alta un nuevo convenio
    ///PARAMETROS:     
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 01-sep-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;
        int Numero_Reestructura = 0;

        if (Hdf_No_Convenio.Value != "")
        {
            try
            {
                if (Btn_Nuevo.ToolTip == "Nuevo")
                {
                    // si el convenio está cancelado, mostrar mensaje
                    if (Cmb_Estatus.SelectedValue == "CANCELADO" || Cmb_Estatus.SelectedValue == "TERMINADO")
                    {
                        Lbl_Mensaje_Error.Text = "No es posible reestructurar el convenio porque tiene estatus " + Cmb_Estatus.SelectedValue;
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                    // validar que el convenio este vencido (sólo así se puede generar reestructura)
                    else if (Txt_Fecha_Vencimiento.Text != "")
                    {
                        Habilitar_Controles("Nuevo");

                        Txt_Fecha_Vencimiento.Text = "";
                        Cmb_Estatus.SelectedValue = "ACTIVO";

                        obtener_Recargos_Moratorios();
                        // actualizar usuario realizo y fecha
                        Txt_Fecha_Convenio.Text = DateTime.Now.ToString("dd/MMM/yyy");
                        Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado.ToUpper();

                        // increementar el numero de reestructura
                        int.TryParse(Txt_Numero_Reestructura.Text, out Numero_Reestructura);
                        Txt_Numero_Reestructura.Text = (++Numero_Reestructura).ToString();

                        Consulta_Descuentos(Hdf_Cuenta_Predial_ID.Value);
                        Cmb_Tipo_Solicitante.Focus();
                        Cargar_Combo_Anio();
                        Calcular_Total_Adeudos();
                        Calcular_Total_Descuento();
                        Calcular_Sub_Total();
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "No es posible reestructurar el convenio porque aún no tiene parcialidades vencidas.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
                else if (Btn_Nuevo.ToolTip == "Dar de Alta")
                {
                    if (Validar_Componentes())
                    {
                        Alta_Reestrucura();
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = Ex.Message;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        else
        {
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de impuesto predial", "alert('Seleccione un convenio o reestructura, por favor.');", true);
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Insertar_Pasivo
    /// DESCRIPCIÓN: Insertar el registro del anticipo del convenio en la tabla de pasivos
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011 
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Insertar_Pasivo(String Prefijo_Referencia)
    {
        OracleConnection Cn = new OracleConnection();
        OracleCommand Cmd = new OracleCommand();
        OracleTransaction Trans = null;

        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        Cls_Cat_Pre_Claves_Ingreso_Negocio Rs_Claves_Ingreso = new Cls_Cat_Pre_Claves_Ingreso_Negocio();
        Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio Rs_Pasivo = new Cls_Ope_Pre_Calculo_Impuesto_Traslado_Negocio();
        DataTable Dt_Clave;
        Decimal Monto = 0;

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            // crear transaccion para modificar tabla de calculos y de adeudos folio
            Cn.ConnectionString = Presidencia.Constantes.Cls_Constantes.Str_Conexion;
            Cn.Open();
            Trans = Cn.BeginTransaction();
            Cmd.Connection = Cn;
            Cmd.Transaction = Trans;
            Rs_Pasivo.P_Cmd_Calculo = Cmd;
            // datos generales para el pasivo
            Rs_Pasivo.P_Referencia = Prefijo_Referencia + Convert.ToInt32(Hdf_No_Convenio.Value);
            // eliminar pasivo con la misma referencia con estatus POR PAGAR
            Rs_Pasivo.Eliminar_Referencias_Pasivo();
            Rs_Pasivo.P_Descripcion = "ANTICIPO - CONVENIO DE IMPUESTO PREDIAL";
            Rs_Pasivo.P_Fecha_Tramite = Grid_Parcialidades.Rows[0].Cells[7].Text;
            Rs_Pasivo.P_Fecha_Vencimiento_Pasivo = Dias_Inhabilies.Calcular_Fecha(Convert.ToDateTime(Grid_Parcialidades.Rows[0].Cells[7].Text).ToShortDateString(), "10").ToString("dd/MMM/yyyy");
            Rs_Pasivo.P_Estatus = "POR PAGAR";
            Rs_Pasivo.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;

            // tratar de obtener un monto de HONORARIOS
            if (Decimal.TryParse(Grid_Parcialidades.Rows[0].Cells[2].Text.Replace("$", ""), out Monto))
            {
                // si se obtuvo un monto mayor que cero, insertar pasivo
                if (Monto > 0)
                {
                    Rs_Claves_Ingreso.P_Tipo = "PREDIAL";
                    Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = "HONORARIOS";
                    Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();
                    // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                    if (Dt_Clave.Rows.Count > 0)
                    {
                        Rs_Pasivo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                        Rs_Pasivo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                        Rs_Pasivo.P_Monto_Total_Pagar = Monto.ToString();
                        Rs_Pasivo.Alta_Pasivo();
                    }
                }
            }
            // tratar de obtener un monto de RECARGOS ORDINARIOS
            if (Decimal.TryParse(Grid_Parcialidades.Rows[0].Cells[3].Text.Replace("$", ""), out Monto))
            {
                // si se obtuvo un monto mayor que cero, insertar pasivo
                if (Monto > 0)
                {
                    Rs_Claves_Ingreso.P_Tipo = "PREDIAL";
                    Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = "RECARGOS ORDINARIOS";
                    Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();
                    // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                    if (Dt_Clave.Rows.Count > 0)
                    {
                        Rs_Pasivo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                        Rs_Pasivo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                        Rs_Pasivo.P_Monto_Total_Pagar = Monto.ToString();
                        Rs_Pasivo.Alta_Pasivo();
                    }
                }
            }
            // tratar de obtener un monto de IMPUESTO PREDIAL
            if (Decimal.TryParse(Grid_Parcialidades.Rows[0].Cells[5].Text.Replace("$", ""), out Monto))
            {
                // si se obtuvo un monto mayor que cero, insertar pasivo
                if (Monto > 0)
                {
                    Rs_Claves_Ingreso.P_Tipo = "PREDIAL";
                    Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = "IMPUESTO";
                    Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();
                    // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                    if (Dt_Clave.Rows.Count > 0)
                    {
                        Rs_Pasivo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                        Rs_Pasivo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                        Rs_Pasivo.P_Monto_Total_Pagar = Monto.ToString();
                        Rs_Pasivo.Alta_Pasivo();
                    }
                }
            }
            // tratar de obtener un monto MORATORIOS
            if (Decimal.TryParse(Grid_Parcialidades.Rows[0].Cells[4].Text.Replace("$", ""), out Monto))
            {
                // si se obtuvo un monto mayor que cero, insertar pasivo
                if (Monto > 0)
                {
                    Rs_Claves_Ingreso.P_Tipo = "PREDIAL";
                    Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = "MORATORIOS";
                    Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();
                    // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
                    if (Dt_Clave.Rows.Count > 0)
                    {
                        Rs_Pasivo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
                        Rs_Pasivo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
                        Rs_Pasivo.P_Monto_Total_Pagar = Monto.ToString();
                        Rs_Pasivo.Alta_Pasivo();
                    }
                }
            }
            //// tratar de obtener un monto de DESCUENTO RECARGOS
            //if (Decimal.TryParse(Txt_Total_Descuento.Text, out Monto))
            //{
            //    // si se obtuvo un monto mayor que cero, insertar pasivo
            //    if (Monto > 0)
            //    {
            //        Rs_Claves_Ingreso.P_Tipo = "PREDIAL";
            //        Rs_Claves_Ingreso.P_Tipo_Predial_Traslado = "DESCUENTO RECARGOS";
            //        Dt_Clave = Rs_Claves_Ingreso.Consultar_Clave_Ingreso();
            //        // si se o btuvieron resultados en la consulta de clave, dar de alta el pasivo
            //        if (Dt_Clave.Rows.Count > 0)
            //        {
            //            Rs_Pasivo.P_Clave_Ingreso_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso_Det.Campo_Clave_Ingreso_ID].ToString();
            //            Rs_Pasivo.P_Dependencia_ID = Dt_Clave.Rows[0][Cat_Pre_Claves_Ingreso.Campo_Dependencia_ID].ToString();
            //            Rs_Pasivo.P_Monto_Total_Pagar = (Monto * -1).ToString();
            //            Rs_Pasivo.Alta_Pasivo();
            //        }
            //    }
            //}

            Trans.Commit();
        }
        catch (OracleException Ex)
        {
            Trans.Rollback();
            throw new Exception("Error: " + Ex.Message);
        }
        catch (Exception ex)
        {
            throw new Exception("Insertar_Pasivo: " + ex.Message.ToString(), ex);
        }
        finally
        {
            Cn.Close();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Alta_Reestrucura
    ///DESCRIPCIÓN          : Dar de alta convenio de impuesto predial
    ///PARAMETROS:     
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 29-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Alta_Reestrucura()
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Cls_Ope_Pre_Convenios_Predial_Negocio Convenios_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            Convenios_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Convenios_Predial.P_Propietario_ID = Hdf_Propietario_ID.Value;
            DataTable Dt_Desglose_Parcialidades;

            // si el solicitante es deudor solidario, guardar nombre y rfc
            if (Cmb_Tipo_Solicitante.SelectedValue == "DEUDOR SOLIDARIO")
            {
                Convenios_Predial.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();
                Convenios_Predial.P_RFC = Txt_RFC.Text.Trim().ToUpper();
            }
            else
            {
                Convenios_Predial.P_Solicitante = "";
                Convenios_Predial.P_RFC = "";
            }
            if (Chk_Parcialidades_Manuales.Checked == true)
            {
                Convenios_Predial.P_Parcialidades_Manual = "SI";
            }
            else
            {
                Convenios_Predial.P_Parcialidades_Manual = "NO";
            }
            Convenios_Predial.P_No_Convenio = Txt_Numero_Convenio.Text;
            Convenios_Predial.P_Realizo = Cls_Sessiones.Empleado_ID;
            Convenios_Predial.P_Estatus = Cmb_Estatus.SelectedValue;
            Convenios_Predial.P_Numero_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
            Convenios_Predial.P_Periodicidad_Pago = Cmb_Periodicidad_Pago.SelectedValue;
            Convenios_Predial.P_Hasta_Periodo = Cmb_Hasta_Bimestre_Periodo.SelectedValue + Cmb_Hasta_Anio_Periodo.SelectedValue;
            Convenios_Predial.P_Fecha = Convert.ToDateTime(Txt_Fecha_Convenio.Text);
            //Convenios_Predial.P_Fecha_Vencimiento = Convert.ToDateTime(Txt_Fecha_Vencimiento.Text);
            Convenios_Predial.P_Observaciones = Txt_Observaciones.Text.ToUpper();
            Convenios_Predial.P_Descuento_Recargos_Ordinarios = Convert.ToDecimal(Txt_Descuento_Recargos_Ordinarios.Text);
            Convenios_Predial.P_Descuento_Recargos_Moratorios = Convert.ToDecimal(Txt_Descuento_Recargos_Moratorios.Text);
            Convenios_Predial.P_Adeudo_Corriente = Convert.ToDecimal(Txt_Adeudo_Corriente.Text);
            Convenios_Predial.P_Adeudo_Rezago = Convert.ToDecimal(Txt_Adeudo_Rezago.Text);
            Convenios_Predial.P_Total_Predial = Convert.ToDecimal(Hdn_Monto_Impuesto.Value);
            Convenios_Predial.P_Total_Recargos = Convert.ToDecimal(Txt_Monto_Recargos.Text);
            Convenios_Predial.P_Total_Moratorios = Convert.ToDecimal(Txt_Monto_Moratorios.Text);
            Convenios_Predial.P_Total_Honorarios = Convert.ToDecimal(Txt_Adeudo_Honorarios.Text);
            Convenios_Predial.P_Total_Adeudo = Convert.ToDecimal(Txt_Total_Adeudo.Text);
            Convenios_Predial.P_Total_Descuento = Convert.ToDecimal(Txt_Total_Descuento.Text);
            Convenios_Predial.P_Sub_Total = Convert.ToDecimal(Txt_Sub_Total.Text);
            Convenios_Predial.P_Porcentaje_Anticipo = Convert.ToDecimal(Txt_Porcentaje_Anticipo.Text);
            Convenios_Predial.P_Total_Anticipo = Convert.ToDecimal(Txt_Total_Anticipo.Text);
            Convenios_Predial.P_Total_Convenio = Convert.ToDecimal(Txt_Total_Convenio.Text);
            Convenios_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            Convenios_Predial.P_No_Descuento = Hdn_No_Descuento.Value;
            Convenios_Predial.P_Dt_Parcialidades = Recuperar_Datos_Tabla_Parcialidades(out Dt_Desglose_Parcialidades);
            Convenios_Predial.P_Dt_Desglose_Parcialidades = Dt_Desglose_Parcialidades;
            //Convenios_Predial.Alta_Convenio_Predial();
            if (Convenios_Predial.Alta_Reestructura_Convenio_Predial())
            {
                Hdf_No_Convenio.Value = Convenios_Predial.P_No_Convenio;
                Btn_Imprimir_Click(null, null);
                //Insertar_Pasivo();
                Inicializa_Controles();
                //Cargar_Grid_Convenios(0);
                Grid_Convenios.DataSource = null;
                Grid_Convenios.DataBind();
                AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(),
                    "Convenios de impuesto predial", "alert('El alta del Convenio de predial fue Exitosa');", true);
            }
            else
            {
                AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de impuesto predial", "alert('El alta del Convenio de predial No fue Exitosa');", true);
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta convenio: " + Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Modificar_Convenio
    ///DESCRIPCIÓN          : Actualizar datos de convenio de impuesto predial
    ///PARAMETROS:     
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 01-sep-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Modificar_Convenio()
    {
        Boolean Aceptar_Cambio = false;
        try
        {
            if (Cmb_Estatus.SelectedValue == "CANCELADO")
            {
                // si hay parcialidades pagadas, mostrar mensaje y cancelar la modificacion
                if (Validar_Estatus_Parcialidades("PAGADA"))
                {
                    AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de impuesto predial", "alert('No se pueden Cancelar Convenios con parcialidades Pagadas');", true);
                    Aceptar_Cambio = false;
                }
                else
                {
                    AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de impuesto predial", "confirm('Confirme que desea Cancelar el Convenio');", true);
                    Aceptar_Cambio = false;
                }
            }
            else
            {
                Aceptar_Cambio = true;
            }
            if (Aceptar_Cambio)
            {
                Cls_Ope_Pre_Convenios_Predial_Negocio Convenio_Predial = new Cls_Ope_Pre_Convenios_Predial_Negocio();
                DataTable Dt_Desglose_Parcialidades;

                Convenio_Predial.P_No_Convenio = Txt_Numero_Convenio.Text;
                Convenio_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
                Convenio_Predial.P_Propietario_ID = Hdf_Propietario_ID.Value;
                // si el solicitante es deudor solidario, guardar nombre y rfc
                if (Cmb_Tipo_Solicitante.SelectedValue == "DEUDOR SOLIDARIO")
                {
                    Convenio_Predial.P_Solicitante = Txt_Solicitante.Text.Trim().ToUpper();
                    Convenio_Predial.P_RFC = Txt_RFC.Text.Trim().ToUpper();
                }
                else
                {
                    Convenio_Predial.P_Solicitante = "";
                    Convenio_Predial.P_RFC = "";
                }
                if (Chk_Parcialidades_Manuales.Checked == true)
                {
                    Convenio_Predial.P_Parcialidades_Manual = "SI";
                }
                else
                {
                    Convenio_Predial.P_Parcialidades_Manual = "NO";
                }
                Convenio_Predial.P_Realizo = Cls_Sessiones.Empleado_ID;
                Convenio_Predial.P_Estatus = Cmb_Estatus.SelectedValue;
                Convenio_Predial.P_Numero_Parcialidades = Convert.ToInt32(Txt_Numero_Parcialidades.Text);
                Convenio_Predial.P_Periodicidad_Pago = Cmb_Periodicidad_Pago.SelectedValue;
                Convenio_Predial.P_Hasta_Periodo = Cmb_Hasta_Bimestre_Periodo.SelectedValue + Cmb_Hasta_Anio_Periodo.SelectedValue;
                Convenio_Predial.P_Fecha = Convert.ToDateTime(Txt_Fecha_Convenio.Text);
                Convenio_Predial.P_Observaciones = Txt_Observaciones.Text.ToUpper();
                Convenio_Predial.P_Descuento_Recargos_Ordinarios = Convert.ToDecimal(Txt_Descuento_Recargos_Ordinarios.Text);
                Convenio_Predial.P_Descuento_Recargos_Moratorios = Convert.ToDecimal(Txt_Descuento_Recargos_Moratorios.Text);
                Convenio_Predial.P_Adeudo_Corriente = Convert.ToDecimal(Txt_Adeudo_Corriente.Text);
                Convenio_Predial.P_Adeudo_Rezago = Convert.ToDecimal(Txt_Adeudo_Rezago.Text);
                Convenio_Predial.P_Total_Predial = Convert.ToDecimal(Hdn_Monto_Impuesto.Value);
                Convenio_Predial.P_Total_Recargos = Convert.ToDecimal(Txt_Monto_Recargos.Text);
                Convenio_Predial.P_Total_Honorarios = Convert.ToDecimal(Txt_Monto_Moratorios.Text);
                Convenio_Predial.P_Total_Adeudo = Convert.ToDecimal(Txt_Total_Adeudo.Text);
                Convenio_Predial.P_Total_Descuento = Convert.ToDecimal(Txt_Total_Descuento.Text);
                Convenio_Predial.P_Sub_Total = Convert.ToDecimal(Txt_Sub_Total.Text);
                Convenio_Predial.P_Porcentaje_Anticipo = Convert.ToDecimal(Txt_Porcentaje_Anticipo.Text);
                Convenio_Predial.P_Total_Anticipo = Convert.ToDecimal(Txt_Total_Anticipo.Text);
                Convenio_Predial.P_Total_Convenio = Convert.ToDecimal(Txt_Total_Convenio.Text);
                Convenio_Predial.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                Convenio_Predial.P_No_Descuento = Hdn_No_Descuento.Value;
                Convenio_Predial.P_No_Reestructura = Txt_Numero_Reestructura.Text;
                Convenio_Predial.P_Dt_Parcialidades = Recuperar_Datos_Tabla_Parcialidades(out Dt_Desglose_Parcialidades);
                Convenio_Predial.P_Dt_Desglose_Parcialidades = Dt_Desglose_Parcialidades;
                if (Convenio_Predial.Modificar_Convenio_Predial())
                {
                    Btn_Imprimir_Click(null, null);
                    Inicializa_Controles();
                    Grid_Convenios.DataSource = null;
                    Grid_Convenios.DataBind();
                }
                else
                {
                    AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de impuesto predial", "alert('La actualización de Convenio de predial No fue Exitosa');", true);
                }
            }
        }
        catch (Exception Ex)
        {
            throw new Exception("Alta convenio: " + Ex.Message);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Modificar_Click
    ///DESCRIPCIÓN          : Deja los componentes listos para hacer la modificacion de un Convenio
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        // verificar que hay una reestructura seleccionada
        if (Hdf_No_Convenio.Value.Trim() != "")
        {
            try
            {

                if (Btn_Modificar.ToolTip.Equals("Modificar"))
                {
                    if (Cmb_Estatus.SelectedValue == "TERMINADO")
                    {
                        Lbl_Mensaje_Error.Text = "No es posible modificar convenios TERMINADOS.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                    // validar que sea una reestructura
                    int Numero_Reestructura;
                    int.TryParse(Txt_Numero_Reestructura.Text.Trim(), out Numero_Reestructura);
                    if (Numero_Reestructura <= 0)
                    {
                        Lbl_Mensaje_Error.Text = "El convenio seleccionado no es una reestructura.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                    // si el estatus es TERMINADO, no se permite la edicion
                    else if (Cmb_Estatus.SelectedValue == "TERMINADO")
                    {
                        Lbl_Mensaje_Error.Text = "No es posible modificar convenios TERMINADOS.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                    // si el estatus es cancelado, no se permite la edicion
                    else if (Cmb_Estatus.SelectedValue == "CANCELADO")
                    {
                        Lbl_Mensaje_Error.Text = "No es posible modificar convenios CANCELADOS.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                    // si el estatus es POR PAGAR de la primer parcialidad, habilitar la modificacion
                    else if (Grid_Parcialidades.Rows[0].Cells[8].Text == "POR PAGAR" || Grid_Parcialidades.Rows[0].Cells[8].Text == "INCUMPLIDO")
                    {
                        Habilitar_Controles("Modificar");

                        if (Cmb_Estatus.SelectedValue == "PENDIENTE")
                        {
                            // actualizar datos del convenio
                            Consulta_Descuentos(Hdf_Cuenta_Predial_ID.Value);
                            Calcular_Total_Adeudos();
                            Calcular_Total_Descuento();
                            Calcular_Sub_Total();
                            Calcular_Total_Anticipo();
                            Calcular_Total_Convenio();
                            Calcular_Parcialidades();
                        }
                        else if (Grid_Parcialidades.Rows[0].Cells[8].Text == "INCUMPLIDO")
                        {
                            // recalcular moratorios
                            obtener_Recargos_Moratorios_Reestructura_Anterior();
                            // volver a calcular total
                            Calcular_Total_Adeudos();
                            Calcular_Total_Adeudos();
                            Calcular_Total_Descuento();
                            Calcular_Sub_Total();
                            // actualizar datos del convenio
                            Txt_Total_Anticipo_TextChanged(null, null);

                            Txt_Fecha_Convenio.Text = DateTime.Now.ToString("dd/MMM/yyyy");
                            Txt_Fecha_Vencimiento.Text = "";
                            Cmb_Estatus.SelectedValue = "ACTIVO";
                        }
                        else
                        {
                            // cargar datos del grid de parcialidades (para que muestre los controles editables del importe)
                            Cargar_Parcialidades();
                        }
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "No es posible modificar convenios con parcialidades ya pagadas.";
                        Lbl_Mensaje_Error.Visible = true;
                        Img_Error.Visible = true;
                    }
                }
                else
                {
                    if (Validar_Componentes())
                    {
                        Modificar_Convenio();
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = "Modificar: " + Ex.Message;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
        }
        else
        {
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType(), "Convenios de impuesto predial", "alert('Seleccione una reestructura, por favor.');", true);
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Buscar_Constancia_Propiedad_Click
    ///DESCRIPCIÓN          : Llena la Tabla con la opcion buscada
    ///PARAMETROS          :
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Buscar_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            // limpiar sesion de convenios consultados
            Session.Remove("Dt_Consulta_Convenios");

            Habilitar_Controles("Inicial");
            Limpiar_Controles();
            Grid_Convenios.SelectedIndex = (-1);
            Grid_Parcialidades.SelectedIndex = (-1);
            Cargar_Grid_Convenios(0);
            if (Grid_Convenios.Rows.Count == 0 && Txt_Busqueda.Text.Trim().Length > 0)
            {
                Lbl_Mensaje_Error.Text = "Para la Busqueda de \"" + Txt_Busqueda.Text + "\" no se encotraron coincidencias";
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }
            Txt_Busqueda.Text = "";
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Buscar: " + Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Salir_Click
    ///DESCRIPCIÓN          : Cancela la operación que esta en proceso (Alta o Actualizar) o Sale del Formulario.
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        Session.Remove("Cuenta_Predial");
        Session.Remove("Tabla_Adeudos");
        Session.Remove("CUENTA_PREDIAL_ID");
        Session.Remove("Dic_Adeudos");
        if (Btn_Salir.ToolTip == "Salir")
        {
            // limpiar sesion de convenios consultados
            Session.Remove("Dt_Consulta_Convenios");

            Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
        }
        else
        {
            Inicializa_Controles();
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Convenio_Escaneado_Click
    ///DESCRIPCIÓN          : Manejador el evento para click sobre el boton convenio 
    ///                     permitir subir archivo y bajar mientras el convenio esté VIGENTE
    ///PARAMETROS:     
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 12-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Convenio_Escaneado_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            // validar que solo se permite subir o bajar convenios si el estatus es ACTIVO
            if (Cmb_Estatus.SelectedValue == "ACTIVO")
            {
                // verificar que haya un convenio seleccionado
                if (Hdf_No_Convenio.Value != null)
                {
                    Bajar_Archivo();
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Sólo es posible para convenios ACTIVOS.";
                Lbl_Mensaje_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Buscar: " + Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Enviar_Archivo_Click
    ///DESCRIPCIÓN          : Guardar el archivo seleccionado en el control file upload
    ///PARAMETROS:     
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 14-nov-2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Enviar_Archivo_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            // validar que haya un archivo a guardar
            if (Fup_Subir_Convenio_Predial.HasFile)
            {
                Guardar_Archivo();
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Seleccione un archivo para subir.";
                Lbl_Mensaje_Error.Visible = true;
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Buscar: " + Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Btn_Mostrar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN          : Muestra los datos de la consulta
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Mostrar_Busqueda_Avanzada_Cuentas_Predial_Click(object sender, ImageClickEventArgs e)
    {
        Boolean Busqueda_Ubicaciones;
        String Cuenta_Predial_ID;
        String Cuenta_Predial;

        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        Busqueda_Ubicaciones = Convert.ToBoolean(Session["BUSQUEDA_CUENTAS_PREDIAL"]);
        if (Busqueda_Ubicaciones)
        {
            if (Session["CUENTA_PREDIAL_ID"] != null)
            {
                Cuenta_Predial_ID = Convert.ToString(Session["CUENTA_PREDIAL_ID"]);
                Hdf_Cuenta_Predial_ID.Value = Cuenta_Predial_ID;
                Session["Cuenta_Predial_ID"] = Cuenta_Predial_ID;
                Cuenta_Predial = Convert.ToString(Session["CUENTA_PREDIAL"]);
                Txt_Cuenta_Predial.Text = Cuenta_Predial;
                Session["Cuenta_Predial"] = Cuenta_Predial.Trim();
                Btn_Detalles_Cuenta_Predial.Enabled = true;
                Btn_Desglose_Adeudos.Enabled = true;

                Consultar_Datos_Cuenta_Predial();

                // verificar si se encontro algun convenio
                if (Hdf_No_Convenio.Value == "")
                {
                    // limpiar descuentos
                    Hdn_No_Descuento.Value = "";
                    Txt_Descuento_Recargos_Moratorios.Text = "0.00";
                    Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
                    Consulta_Descuentos(Cuenta_Predial_ID);

                    Calcular_Total_Adeudos();
                    Calcular_Total_Descuento();
                    Calcular_Sub_Total();
                    Calcular_Total_Anticipo();
                    Calcular_Total_Convenio();
                    Calcular_Parcialidades();
                }
            }
            Cargar_Ventana_Emergente_Resumen_Predio();
            Cargar_Ventana_Emergente_Desglose_Adeudos();
        }
        // limpiar variables de sesion
        Session["BUSQUEDA_CUENTAS_PREDIAL"] = null;
        //Session["CUENTA_PREDIAL_ID"] = null;
        //Session["CUENTA_PREDIAL"] = null;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Datos_Cuenta_Predial
    ///DESCRIPCIÓN          : Realiza la búsqueda de los adeudos de la cuenta predial introducida
    ///                         y muestra los adeudos
    ///PARAMETROS:     
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 27/Julio/2011
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Consultar_Datos_Cuenta_Predial()
    {
        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuentas_Predial = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
        String No_Convenio_Activo;

        DataTable Dt_Cuentas_Predial;
        if (Txt_Cuenta_Predial.Text.Trim() != "")
        {
            //Consulta la Cuenta Predial
            Cuentas_Predial.P_Incluir_Campos_Foraneos = true;
            Cuentas_Predial.P_Campos_Dinamicos = Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Cuenta_Predial_ID + ", "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Exterior + ", "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_No_Interior + ", "
                + Cat_Pre_Cuentas_Predial.Tabla_Cat_Pre_Cuentas + "." + Cat_Pre_Cuentas_Predial.Campo_Estatus + ", "
                + "PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_Contribuyente_ID + ", "
                + "PROPIETARIOS." + Cat_Pre_Contribuyentes.Campo_RFC + ", "
                + Cat_Pre_Propietarios.Tabla_Cat_Pre_Propietarios + "." + Cat_Pre_Propietarios.Campo_Propietario_ID;
            Cuentas_Predial.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
            Dt_Cuentas_Predial = Cuentas_Predial.Consultar_Cuenta();
            if (Dt_Cuentas_Predial.Rows.Count > 0)
            {
                if (Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_Estatus].ToString() == "PENDIENTE")
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Cuenta predial con estatus PENDIENTE";
                }
                // metodo que carga datos de la cuenta
                if (Hdf_Cuenta_Predial_ID.Value.Length <= 0)
                {
                    Txt_Propietario.Text = "";
                    Txt_Colonia.Text = "";
                    Txt_Calle.Text = "";
                    Txt_No_Exterior.Text = "";
                    Txt_No_Interior.Text = "";
                }
                else
                {
                    Txt_Propietario.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_PROPIETARIO"].ToString();
                    Txt_Calle.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_CALLE"].ToString();
                    Txt_Colonia.Text = Dt_Cuentas_Predial.Rows[0]["NOMBRE_COLONIA"].ToString();
                    Txt_No_Exterior.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Exterior].ToString();
                    Txt_No_Interior.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Cuentas_Predial.Campo_No_Interior].ToString();
                    Hdf_Propietario_ID.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Contribuyentes.Campo_Contribuyente_ID].ToString();
                    // copiar en el solicitante, solo si esta seleccionado PROPIETARIO
                    if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
                    {
                        Txt_Solicitante.Text = Txt_Propietario.Text;
                        Txt_RFC.Text = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                    }
                    Hdf_RFC_Propietario.Value = Dt_Cuentas_Predial.Rows[0][Cat_Pre_Contribuyentes.Campo_RFC].ToString();
                }
                // datos para convenio nuevo
                Txt_Realizo.Text = Cls_Sessiones.Nombre_Empleado.ToUpper();
                Txt_Fecha_Convenio.Text = String.Format("{0:dd/MMM/yyyy}", DateTime.Now);
                //Txt_Fecha_Vencimiento.Text = String.Format("{0:dd/MMM/yyyy}", Constancias_Negocio.Calcular_Fecha(Txt_Fecha_Convenio.Text.Trim(), Dias));
                Cmb_Estatus.SelectedValue = "ACTIVO";

                if (Validar_Existe_Convenio_Activo("", Hdf_Cuenta_Predial_ID.Value, out No_Convenio_Activo))
                {
                    Cargar_Convenio_Activo(No_Convenio_Activo);
                    Btn_Detalles_Cuenta_Predial.Enabled = true;
                    Btn_Desglose_Adeudos.Enabled = true;
                    AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(this, this.GetType()
                        , "Convenios de impuesto predial", "alert('Ya existe un Número de Convenio Activo para esta Cuenta');", true);
                    //Limpiar_Controles();
                    //Btn_Salir_Click(null, null);
                }
                else
                {

                }
            }

        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Validar_Existe_Convenio_Activo
    /// DESCRIPCIÓN: Consultar si existe un convenio activo para la cuenta predial seleccionada
    /// PARÁMETROS:
    /// 		1. No_Convenio: Numero de convenio
    /// 		2. Cuenta_Predial_ID: ID de la cuenta predial
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Boolean Validar_Existe_Convenio_Activo(String No_Convenio, String Cuenta_Predial_ID, out String No_Convenio_Activo)
    {
        Boolean Convenio_Activo = false;
        No_Convenio_Activo = "";

        if (Grid_Convenios.SelectedIndex != -1)
        {
            Convenio_Activo = false;
        }
        else
        {
            Cls_Ope_Pre_Convenios_Predial_Negocio Rs_Convenios_Activos = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            DataTable Dt_Convenios_Activos;
            if (No_Convenio != "")
            {
                Rs_Convenios_Activos.P_No_Convenio = No_Convenio;
                Rs_Convenios_Activos.P_Estatus = " !='CUENTA_CANCELADA'";
            }
            else if (Cuenta_Predial_ID != "")
            {
                Rs_Convenios_Activos.P_Cuenta_Predial_ID = Cuenta_Predial_ID;
                Rs_Convenios_Activos.P_Estatus = " !='CUENTA_CANCELADA'";
            }
            if (No_Convenio != "" || Cuenta_Predial_ID != "")
            {
                Rs_Convenios_Activos.P_Estatus = " IN ('ACTIVO','PENDIENTE')";
                // especificar que valide el estatus del convenio
                Rs_Convenios_Activos.P_Validar_Convenios_Cumplidos = true;
                Dt_Convenios_Activos = Rs_Convenios_Activos.Consultar_Convenio_Predial();
                if (Dt_Convenios_Activos.Rows.Count > 0)
                {
                    string Estatus_Convenio;
                    // recorrer los convenios para determinar si son activos (debido a validación después de la consulta)
                    foreach (DataRow Convenio in Dt_Convenios_Activos.Rows)
                    {
                        Estatus_Convenio = Dt_Convenios_Activos.Rows[0][Ope_Pre_Convenios_Predial.Campo_Estatus].ToString();
                        if (Estatus_Convenio.Contains("ACTIVO") || Estatus_Convenio.Contains("PENDIENTE"))
                        {
                            No_Convenio_Activo = Dt_Convenios_Activos.Rows[0][Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                            Convenio_Activo = true;
                            break;
                        }
                    }
                }
                else
                {
                    Convenio_Activo = false;
                }
            }
        }
        return Convenio_Activo;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Adeudos_Cuenta
    /// DESCRIPCIÓN: Consultar y mostrar adeudos de la cuenta o mostrar ceros
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 28-nov-2011
    ///MODIFICO: Roberto González Oseguera
    ///FECHA_MODIFICO: 23-abr-2012
    ///CAUSA_MODIFICACIÓN: Se agregó consulta de honorarios
    ///*******************************************************************************************************
    private void Cargar_Adeudos_Cuenta(int Hasta_Anio, int Hasta_Bimestre)
    {
        var Consultar_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        var Parametro = new Cls_Ope_Pre_Parametros_Negocio();
        var Consulta_Honorarios = new Cls_Ope_Pre_Pae_Honorarios_Negocio();
        Int32 Anio_Corriente = Parametro.Consultar_Anio_Corriente();
        Int32 Anio_Primer_Adeudo = 0;
        Int32 Anio_Ultimo_Adeudo = 0;
        DataTable Dt_Honorarios;
        // cargar adeudos de la cuenta
        DataTable Dt_Adeudos = Consultar_Adeudos.Calcular_Recargos_Predial_Hasta_Bimestre(Hdf_Cuenta_Predial_ID.Value, Hasta_Anio, Hasta_Bimestre);
        if (Dt_Adeudos != null)
        {
            Session["Tabla_Adeudos"] = Dt_Adeudos;
        }

        if (Dt_Adeudos.Rows.Count > 0)
        {
            Hdn_Monto_Impuesto.Value = (Consultar_Adeudos.p_Total_Rezago + Consultar_Adeudos.p_Total_Corriente).ToString("#,##0.00");
            Txt_Adeudo_Corriente.Text = Consultar_Adeudos.p_Total_Corriente.ToString("#,##0.00");
            Txt_Adeudo_Rezago.Text = Consultar_Adeudos.p_Total_Rezago.ToString("#,##0.00");
            Txt_Monto_Recargos.Text = String.Format("{0:#,##0.00}", Consultar_Adeudos.p_Total_Recargos_Generados);

            // obtener anio del primer y ultimo adeudo de la tabla obtenida de adeudos
            Int32.TryParse(Dt_Adeudos.Rows[0][0].ToString().Substring(1, 4), out Anio_Primer_Adeudo);
            Int32.TryParse(Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1][0].ToString().Substring(1, 4), out Anio_Ultimo_Adeudo);
            // agregar al combo hasta anio periodo valores para los anio con adeudo
            // Cmb_Hasta_Anio_Periodo.Items.Clear();
            if (Anio_Ultimo_Adeudo > 0 && Anio_Ultimo_Adeudo > Anio_Primer_Adeudo)
            {
                Anio_Ultimo_Adeudo = Convert.ToInt32(Cmb_Hasta_Anio_Periodo.SelectedValue);
                Cmb_Hasta_Anio_Periodo.Items.Clear();
                for (int i = Anio_Corriente; i >= Anio_Primer_Adeudo; --i)
                {
                    System.Web.UI.WebControls.ListItem Anio_Adeudo = new System.Web.UI.WebControls.ListItem(i.ToString(), i.ToString());
                    // si la lista no contiene ya el año, agregarlo
                    if (!Cmb_Hasta_Anio_Periodo.Items.Contains(Anio_Adeudo))
                    {
                        Cmb_Hasta_Anio_Periodo.Items.Add(Anio_Adeudo);
                    }
                }
            }
            // prevenir error si no se agrego el valor a seleccionar (la cuenta ya no tiene adeudos)
            if (Cmb_Hasta_Anio_Periodo.Items.FindByText(Anio_Ultimo_Adeudo.ToString()) == null)
            {
                Cmb_Hasta_Anio_Periodo.Items.Add(Anio_Ultimo_Adeudo.ToString());
            }

            Cmb_Hasta_Anio_Periodo.SelectedIndex = Cmb_Hasta_Anio_Periodo.Items.IndexOf(Cmb_Hasta_Anio_Periodo.Items.FindByValue(Anio_Ultimo_Adeudo.ToString()));
            // seleccionar en combos periodo los valores correspondientes
            //Cmb_Hasta_Anio_Periodo.SelectedValue = Anio_Ultimo_Adeudo.ToString();
            //Cmb_Hasta_Bimestre_Periodo.SelectedValue = Dt_Adeudos.Rows[Dt_Adeudos.Rows.Count - 1][0].ToString().Substring(0, 1);

        }
        else
        {
            Txt_Adeudo_Rezago.Text = "0.00";
            Txt_Adeudo_Corriente.Text = "0.00";
            Hdn_Monto_Impuesto.Value = "0.00";
            Txt_Monto_Moratorios.Text = "0.00";
            Txt_Monto_Recargos.Text = "0.00";
        }

        // consulta de honorarios
        Consulta_Honorarios.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
        Dt_Honorarios = Consulta_Honorarios.Consultar_Total_Honorarios();
        if (Dt_Honorarios != null && Dt_Honorarios.Rows.Count > 0)
        {
            decimal Adeudo_Honorarios;
            decimal.TryParse(Dt_Honorarios.Rows[0]["TOTAL_HONORARIOS"].ToString(), out Adeudo_Honorarios);
            Txt_Adeudo_Honorarios.Text = Adeudo_Honorarios.ToString("#,##0.00");
        }
        else
        {
            Txt_Adeudo_Honorarios.Text = "0.00";
        }

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Convenio_Activo
    /// DESCRIPCIÓN: Carga los datos de un convenio proporcionado como parametro como si se seleccionara del grid
    /// PARÁMETROS:
    /// 		1. No_Convenio: El numero de convenio a cargar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 16-oct-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    protected void Cargar_Convenio_Activo(String No_Convenio)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        Chk_Parcialidades_Manuales.Checked = false;

        if (!String.IsNullOrEmpty(No_Convenio))
        {
            //Hdf_Cuenta_Predial_ID.Value = Grid_Convenios.DataKeys[Grid_Convenios.SelectedIndex].Value.ToString();
            Session["Cuenta_Predial_ID"] = Hdf_Cuenta_Predial_ID.Value;
            //Hdf_Propietario_ID.Value = Grid_Convenios.DataKeys[1].Value.ToString();
            Hdf_No_Convenio.Value = No_Convenio;

            Cls_Ope_Pre_Convenios_Predial_Negocio Convenio = new Cls_Ope_Pre_Convenios_Predial_Negocio();
            DataTable Dt_Convenios_Predial;
            String Periodo;

            if (Grid_Convenios.SelectedRow.Cells[8].Text.Replace("&nbsp;", "").Equals(""))
            {
                Convenio.P_Reestructura = false;
            }
            else
            {
                Convenio.P_Reestructura = true;
            }

            Lbl_Mensaje_Error.Visible = false;
            Lbl_Mensaje_Error.Text = "";
            Img_Error.Visible = false;

            try
            {
                Convenio.P_Campos_Foraneos = true;
                Convenio.P_No_Convenio = No_Convenio;
                // especificar que valide el estatus del convenio
                Convenio.P_Validar_Convenios_Cumplidos = true;

                Dt_Convenios_Predial = Convenio.Consultar_Convenio_Predial();

                if (Dt_Convenios_Predial != null)
                {
                    if (Dt_Convenios_Predial.Rows.Count > 0)
                    {
                        Decimal Monto = 0;
                        foreach (DataRow Row in Dt_Convenios_Predial.Rows)
                        {
                            Hdf_Cuenta_Predial_ID.Value = Row[Ope_Pre_Convenios_Predial.Campo_Cuenta_Predial_Id].ToString();
                            Session["Cuenta_Predial_ID"] = Hdf_Cuenta_Predial_ID.Value;
                            Hdf_Propietario_ID.Value = Row[Ope_Pre_Convenios_Predial.Campo_Contribuyente_Id].ToString();
                            Txt_Cuenta_Predial.Text = Row["Cuenta_Predial"].ToString();
                            Txt_Numero_Convenio.Text = Row[Ope_Pre_Convenios_Predial.Campo_No_Convenio].ToString();
                            Cmb_Estatus.SelectedValue = Row[Ope_Pre_Convenios_Predial.Campo_Estatus].ToString();
                            Txt_Propietario.Text = Row["Nombre_Propietario"].ToString();
                            if (Row[Ope_Pre_Convenios_Predial.Campo_Solicitante].ToString() != "")
                            {
                                Txt_Solicitante.Text = Row[Ope_Pre_Convenios_Predial.Campo_Solicitante].ToString();
                                Txt_RFC.Text = Row[Ope_Pre_Convenios_Predial.Campo_RFC].ToString();
                                Cmb_Tipo_Solicitante.SelectedIndex = 1;
                            }
                            else
                            {
                                Txt_Solicitante.Text = Row["Nombre_Propietario"].ToString();
                                Cmb_Tipo_Solicitante.SelectedIndex = 0;
                            }
                            Txt_Numero_Parcialidades.Text = Row[Ope_Pre_Convenios_Predial.Campo_Numero_Parcialidades].ToString();
                            Cmb_Periodicidad_Pago.SelectedValue = Row[Ope_Pre_Convenios_Predial.Campo_Periodicidad_Pago].ToString();
                            Txt_Realizo.Text = Row["Nombre_Realizo"].ToString();
                            Periodo = Row[Ope_Pre_Convenios_Predial.Campo_Hasta_Periodo].ToString();
                            // si no se encontro el periodo en el encabezado de los datos del convenio, tomar de la ultima parcialidad
                            if (String.IsNullOrEmpty(Periodo) && Convenio.P_Dt_Parcialidades.Rows.Count > 0)
                            {
                                String Periodo_Ultima_Parcialidad;
                                String[] Periodos_Separados;
                                Periodo_Ultima_Parcialidad = Convenio.P_Dt_Parcialidades.Rows[Convenio.P_Dt_Parcialidades.Rows.Count - 1][Ope_Pre_Detalles_Convenios_Predial.Campo_Periodo].ToString().Trim();
                                Periodos_Separados = Periodo_Ultima_Parcialidad.Split('-');
                                // despues de separado el 
                                if (Periodos_Separados.Length >= 2)
                                {
                                    Periodo = Periodos_Separados[1].Replace("/", "");
                                }
                            }

                            // prevenir error si no se agrego el valor a seleccionar (la cuenta ya no tiene adeudos o no se guardo el convenio con Hasta_Periodo), agregar dato al combo
                            // si se obtuvo un valor para el periodo
                            if (!String.IsNullOrEmpty(Periodo))
                            {
                                if (Cmb_Hasta_Anio_Periodo.Items.FindByText(Periodo.Substring(1, 4)) == null)
                                {
                                    System.Web.UI.WebControls.ListItem Anio_Adeudo = new System.Web.UI.WebControls.ListItem(Periodo.Substring(1, 4), Periodo.Substring(1, 4));
                                    Cmb_Hasta_Anio_Periodo.Items.Add(Anio_Adeudo);
                                }
                                Cmb_Hasta_Anio_Periodo.SelectedValue = Periodo.Substring(1, 4);
                                Cmb_Hasta_Bimestre_Periodo.SelectedValue = Periodo.Substring(0, 1);
                            }

                            if (Row[Ope_Pre_Convenios_Predial.Campo_Parcialidades_Manual].ToString() == "SI")
                            {
                                Chk_Parcialidades_Manuales.Checked = true;
                            }
                            else
                            {
                                Chk_Parcialidades_Manuales.Checked = false;
                            }

                            Txt_Fecha_Convenio.Text = Convert.ToDateTime(Row[Ope_Pre_Convenios_Predial.Campo_Fecha].ToString()).ToString("dd/MMM/yyyy");
                            Txt_Observaciones.Text = Row[Ope_Pre_Convenios_Predial.Campo_Observaciones].ToString();
                            Txt_Descuento_Recargos_Ordinarios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Ordinarios]).ToString("#,##0.00");
                            Txt_Descuento_Recargos_Moratorios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Descuento_Recargos_Moratorios]).ToString("#,##0.00");
                            Hdn_No_Descuento.Value = Row[Ope_Pre_Convenios_Predial.Campo_No_Descuento].ToString();
                            Txt_Total_Adeudo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Adeudo]).ToString("#,##0.00");
                            Txt_Monto_Total_Adeudo.Text = Txt_Total_Adeudo.Text;
                            Hdn_Monto_Impuesto.Value = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Predial]).ToString("#,##0.00");
                            Decimal.TryParse(Row[Ope_Pre_Convenios_Predial.Campo_Adeudo_Corriente].ToString(), out Monto);
                            Txt_Adeudo_Corriente.Text = Monto.ToString("#,##0.00");
                            Decimal.TryParse(Row[Ope_Pre_Convenios_Predial.Campo_Adeudo_Rezago].ToString(), out Monto);
                            Txt_Adeudo_Rezago.Text = Monto.ToString("#,##0.00");
                            Txt_Monto_Recargos.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Recargos]).ToString("#,##0.00");
                            Txt_Adeudo_Honorarios.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Honorarios]).ToString("#,##0.00");
                            Txt_Total_Descuento.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Descuento]).ToString("#,##0.00");
                            Txt_Sub_Total.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Sub_Total]).ToString("#,##0.00");
                            Txt_Porcentaje_Anticipo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Porcentaje_Anticipo]).ToString("#,##0.00");
                            Txt_Total_Anticipo.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Anticipo]).ToString("#,##0.00");
                            Txt_Total_Convenio.Text = Convert.ToDecimal(Row[Ope_Pre_Convenios_Predial.Campo_Total_Convenio]).ToString("#,##0.00");
                        }
                        Grid_Parcialidades.DataSource = Convenio.P_Dt_Parcialidades;
                        Grid_Parcialidades.PageIndex = 0;
                        Grid_Parcialidades.DataBind();

                        Sumar_Totales_Parcialidades();

                        Session["Cuenta_Predial"] = Txt_Cuenta_Predial.Text.Trim();
                        Btn_Detalles_Cuenta_Predial.Enabled = true;
                        Btn_Desglose_Adeudos.Enabled = true;
                        Habilitar_Controles("Inicial");
                        Btn_Modificar.Visible = true;
                        Btn_Imprimir.Visible = true;
                        Panel_Datos.Visible = true;
                        Btn_Salir.ToolTip = "Inicio";
                        Grid_Convenios.SelectedIndex = -1;
                        Grid_Convenios.Visible = false;
                        Cmb_Tipo_Solicitante.Enabled = false;
                        Txt_RFC.Enabled = false;
                    }
                }
            }
            catch (Exception Ex)
            {
                Lbl_Mensaje_Error.Text = Ex.Message;
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
            }

        }
    }
    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Total_Adeudos
    /// DESCRIPCIÓN: Calcular el total de adeudo (impuesto + recargos ordinarios y moratorios)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Total_Adeudos()
    {
        Decimal Monto_Impuesto = 0;
        Decimal Monto_Ordinarios = 0;
        Decimal Monto_Moratorios = 0;
        Decimal Monto_Honorarios = 0;
        Decimal Total_Adeudo = 0;

        Decimal.TryParse(Hdn_Monto_Impuesto.Value, out Monto_Impuesto);
        Decimal.TryParse(Txt_Monto_Recargos.Text, out Monto_Ordinarios);
        Decimal.TryParse(Txt_Monto_Moratorios.Text, out Monto_Moratorios);
        Decimal.TryParse(Txt_Adeudo_Honorarios.Text, out Monto_Honorarios);

        // calcular total adeudo
        Total_Adeudo = Monto_Impuesto + Monto_Ordinarios + Monto_Moratorios + Monto_Honorarios;
        // cargar montos en cajas de texto
        Txt_Total_Adeudo.Text = Total_Adeudo.ToString("#,##0.00");
        Txt_Monto_Total_Adeudo.Text = Txt_Total_Adeudo.Text;
        Hdn_Monto_Impuesto.Value = Monto_Impuesto.ToString("#,##0.00");
        Txt_Monto_Recargos.Text = Monto_Ordinarios.ToString("#,##0.00");
        Txt_Monto_Moratorios.Text = Monto_Moratorios.ToString("#,##0.00");
        Txt_Adeudo_Honorarios.Text = Monto_Honorarios.ToString("#,##0.00");

        return Total_Adeudo;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Sub_Total
    /// DESCRIPCIÓN: Calcular subtotal convenio (Total adeudo - descuento)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Sub_Total()
    {
        Decimal Total_Adeudo = 0;
        Decimal Total_Descuento = 0;
        Decimal Sub_Total = 0;

        Decimal.TryParse(Txt_Total_Adeudo.Text, out Total_Adeudo);
        Decimal.TryParse(Txt_Total_Descuento.Text, out Total_Descuento);

        Sub_Total = Total_Adeudo - Total_Descuento;

        Txt_Total_Adeudo.Text = Total_Adeudo.ToString("#,##0.00");
        Txt_Monto_Total_Adeudo.Text = Txt_Total_Adeudo.Text;
        Txt_Total_Descuento.Text = Total_Descuento.ToString("#,##0.00");
        Txt_Sub_Total.Text = Sub_Total.ToString("#,##0.00");

        return Sub_Total;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Total_Convenio
    /// DESCRIPCIÓN: Calcular total a convenir (subtotal - anticipo)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 01-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Decimal Calcular_Total_Convenio()
    {
        Decimal Sub_Total = 0;
        Decimal Total_Anticipo = 0;
        Decimal Total_Convenio = 0;

        Decimal.TryParse(Txt_Sub_Total.Text, out Sub_Total);
        Decimal.TryParse(Txt_Total_Anticipo.Text, out Total_Anticipo);
        // calcular total
        Total_Convenio = Sub_Total - Total_Anticipo;
        // mostrar en valores en las cajas de texto
        Txt_Sub_Total.Text = Sub_Total.ToString("#,##0.00");
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("#,##0.00");
        Txt_Total_Convenio.Text = Total_Convenio.ToString("#,##0.00");

        return Total_Convenio;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Numero_Parcialidades_TextChanged
    ///DESCRIPCIÓN          : Genera las parcialidades en el grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 07/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Numero_Parcialidades_TextChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            if (Chk_Parcialidades_Manuales.Checked == true)
            {
                // reiniciar parcialidades en el grid
                Grid_Parcialidades_Editable = false;
                Grid_Parcialidades_Manuales = true;
                Grid_Parcialidades.DataSource = null;
                Grid_Parcialidades.DataBind();

                Habilitar_Parcialidades_Manuales();

                // desactivar cajas de texto anticipo
                Txt_Porcentaje_Anticipo.Enabled = false;
                Txt_Total_Anticipo.Enabled = false;
                Txt_Porcentaje_Anticipo.Text = "0.00";
                Txt_Total_Anticipo.Text = "0.00";
            }
            else
            {
                Grid_Parcialidades_Manuales = false;
                Calcular_Total_Anticipo();
                Calcular_Total_Convenio();
                Calcular_Parcialidades();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Fecha_Convenio_TextChanged
    ///DESCRIPCIÓN          : Volver a calcular fechas de vencimiento del grid
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 18-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Fecha_Convenio_TextChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            if (Chk_Parcialidades_Manuales.Checked == true)
            {
                if (Grid_Parcialidades.Rows.Count > 0)
                {
                    Actualizar_Fechas_Periodo();
                }
                else
                {
                    Habilitar_Parcialidades_Manuales();

                    // desactivar cajas de texto anticipo
                    Txt_Porcentaje_Anticipo.Enabled = false;
                    Txt_Total_Anticipo.Enabled = false;
                    Txt_Porcentaje_Anticipo.Text = "0.00";
                    Txt_Total_Anticipo.Text = "0.00";
                }
            }
            else
            {
                Calcular_Total_Anticipo();
                Calcular_Total_Convenio();
                Calcular_Parcialidades();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Periodicidad_Pago_SelectedIndexChanged
    ///DESCRIPCIÓN          : Recalcular totales y parcialidades llamando métodos
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Periodicidad_Pago_SelectedIndexChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            if (Chk_Parcialidades_Manuales.Checked == true)
            {
                if (Grid_Parcialidades.Rows.Count > 0)
                {
                    Actualizar_Fechas_Periodo();
                }
                else if (Txt_Numero_Parcialidades.Text.Length > 0)
                {
                    Habilitar_Parcialidades_Manuales();

                    // desactivar cajas de texto anticipo
                    Txt_Porcentaje_Anticipo.Enabled = false;
                    Txt_Total_Anticipo.Enabled = false;
                    Txt_Porcentaje_Anticipo.Text = "0.00";
                    Txt_Total_Anticipo.Text = "0.00";
                }
            }
            else
            {
                Calcular_Total_Adeudos();
                Calcular_Total_Descuento();
                Calcular_Sub_Total();
                Calcular_Total_Anticipo();
                Calcular_Total_Convenio();
                Calcular_Parcialidades();

                Txt_Numero_Parcialidades.Focus();
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Descuento_Recargos_Ordinarios_TextChanged
    ///DESCRIPCIÓN          : Recalcular descuentos, totales y parcialidades llamando métodos
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Recargos_Ordinarios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();

        //Txt_Descuento_Recargos_Moratorios.Focus();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Descuento_Recargos_Moratorios_TextChanged
    ///DESCRIPCIÓN          : Recalcular descuentos, totales y parcialidades llamando métodos
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Descuento_Recargos_Moratorios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Descuento();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();

        //Txt_Porcentaje_Anticipo.Focus();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Grid_Monto_Importe_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto en el grid, vuelve a generar las 
    ///                         parcialidades a partir del periodo que se cambió
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 27-oct-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Grid_Monto_Importe_TextChanged(object sender, EventArgs e)
    {
        TextBox Txt_Monto;
        Label Lbl_Txt_Monto;
        GridViewRow Fila_Grid;
        Int32 No_Pago = 0;

        Txt_Monto = (TextBox)sender;
        // si se recupero la caja de texto, intentar recuperar la fila del grid que lo contiene
        if (Txt_Monto != null)
        {
            Fila_Grid = (GridViewRow)FindControl(Txt_Monto.Parent.Parent.UniqueID);
            if (Fila_Grid != null)
            {
                // recuperar el numero de pago
                Int32.TryParse(Fila_Grid.Cells[0].Text, out No_Pago);

                // si es la fila con la primer parcialidad (anticipo),  todas las parcialidades
                if (No_Pago == 1)
                {
                    Txt_Total_Anticipo.Text = Txt_Monto.Text;
                    Txt_Total_Anticipo_TextChanged(null, null);
                }
                // si es la ultima parcialidad no hacer nada
                else if (No_Pago == (Grid_Parcialidades.Rows.Count))
                {
                    // tratar de recuperar el valor en la etqiqueta del importe
                    Lbl_Txt_Monto = (Label)Fila_Grid.Cells[6].FindControl("Lbl_Txt_Grid_Monto_Importe");
                    if (Lbl_Txt_Monto != null)
                    {
                        Txt_Monto.Text = Lbl_Txt_Monto.Text;
                    }
                }
                else
                {
                    Decimal Nuevo_Monto;
                    Decimal.TryParse(Txt_Monto.Text, out Nuevo_Monto);
                    Leer_Parametros_Parcialidades_Hasta_No_Pago(No_Pago.ToString(), Nuevo_Monto);
                }
            }
        }

        Txt_Monto.Text = Convert.ToDouble(Txt_Monto.Text.Replace("$", "")).ToString("#,##0.00");
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Adeudo_Honorarios_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto, volver a sumar adeudos totales
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Adeudo_Honorarios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Adeudos();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Monto_Recargos_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto, volver a sumar adeudos totales
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Monto_Recargos_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Adeudos();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Monto_Moratorios_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto, volver a sumar adeudos totales
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Monto_Moratorios_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Adeudos();
        Calcular_Sub_Total();
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Grid_Periodo_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto en el grid, actualizar el impuesto
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Grid_Periodo_TextChanged(object sender, EventArgs e)
    {
        TextBox Txt_Monto;
        Int32 No_Pago = 0;
        GridViewRow Fila_Grid;
        int Indice_Fila_Grid_Parcialidades = -1;

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Txt_Monto = (TextBox)sender;
            // si se recupero la caja de texto, intentar recuperar la fila del grid que lo contiene
            if (Txt_Monto != null)
            {
                Fila_Grid = (GridViewRow)FindControl(Txt_Monto.Parent.Parent.UniqueID);
                if (Fila_Grid != null)
                {
                    // recuperar el numero de pago
                    if (Int32.TryParse(Fila_Grid.Cells[0].Text, out No_Pago))
                    {
                        Indice_Fila_Grid_Parcialidades = No_Pago - 1;

                        Actualizar_Periodo_Impuesto_Parcialidades_Modificadas(Indice_Fila_Grid_Parcialidades);
                        Sumar_Importe_Parcialidades();
                        Sumar_Totales_Parcialidades();
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Txt_Grid_Monto_Honorarios_TextChanged: " + Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Grid_Monto_Honorarios_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto en el grid, sumar los valores de las adeudos en la fila del grid
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Grid_Monto_Honorarios_TextChanged(object sender, EventArgs e)
    {
        TextBox Txt_Monto;
        Int32 No_Pago = 0;
        GridViewRow Fila_Grid;
        int Indice_Fila_Grid_Parcialidades = -1;

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Txt_Monto = (TextBox)sender;
            // si se recupero la caja de texto, intentar recuperar la fila del grid que lo contiene
            if (Txt_Monto != null)
            {
                Fila_Grid = (GridViewRow)FindControl(Txt_Monto.Parent.Parent.UniqueID);
                if (Fila_Grid != null)
                {
                    // recuperar el numero de pago
                    if (Int32.TryParse(Fila_Grid.Cells[0].Text, out No_Pago))
                    {
                        Indice_Fila_Grid_Parcialidades = No_Pago - 1;

                        Actualizar_Importe_Parcialidades_Modificadas(Indice_Fila_Grid_Parcialidades, 2);
                        Sumar_Importe_Parcialidades();
                        Sumar_Totales_Parcialidades();
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Txt_Grid_Monto_Honorarios_TextChanged: " + Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Grid_Monto_Recargos_Ordinarios_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto en el grid, sumar los valores de las adeudos en la fila del grid
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Grid_Monto_Recargos_Ordinarios_TextChanged(object sender, EventArgs e)
    {
        TextBox Txt_Monto;
        Int32 No_Pago = 0;
        GridViewRow Fila_Grid;
        int Indice_Fila_Grid_Parcialidades = -1;

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Txt_Monto = (TextBox)sender;
            // si se recupero la caja de texto, intentar recuperar la fila del grid que lo contiene
            if (Txt_Monto != null)
            {
                Fila_Grid = (GridViewRow)FindControl(Txt_Monto.Parent.Parent.UniqueID);
                if (Fila_Grid != null)
                {
                    // recuperar el numero de pago
                    if (Int32.TryParse(Fila_Grid.Cells[0].Text, out No_Pago))
                    {
                        Indice_Fila_Grid_Parcialidades = No_Pago - 1;

                        Actualizar_Importe_Parcialidades_Modificadas(Indice_Fila_Grid_Parcialidades, 3);
                        Sumar_Importe_Parcialidades();
                        Sumar_Totales_Parcialidades();
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Txt_Grid_Monto_Honorarios_TextChanged: " + Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Grid_Monto_Recargos_Moratorios_TextChanged
    ///DESCRIPCIÓN          : Cuando cambia el texto en el grid, sumar los valores de las adeudos en la fila del grid
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 16-abr-2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Grid_Monto_Recargos_Moratorios_TextChanged(object sender, EventArgs e)
    {
        TextBox Txt_Monto;
        Int32 No_Pago = 0;
        GridViewRow Fila_Grid;
        int Indice_Fila_Grid_Parcialidades = -1;

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        try
        {
            Txt_Monto = (TextBox)sender;
            // si se recupero la caja de texto, intentar recuperar la fila del grid que lo contiene
            if (Txt_Monto != null)
            {
                Fila_Grid = (GridViewRow)FindControl(Txt_Monto.Parent.Parent.UniqueID);
                if (Fila_Grid != null)
                {
                    // recuperar el numero de pago
                    if (Int32.TryParse(Fila_Grid.Cells[0].Text, out No_Pago))
                    {
                        Indice_Fila_Grid_Parcialidades = No_Pago - 1;

                        Actualizar_Importe_Parcialidades_Modificadas(Indice_Fila_Grid_Parcialidades, 4);
                        Sumar_Importe_Parcialidades();
                        Sumar_Totales_Parcialidades();
                    }

                }
            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Text = "Txt_Grid_Monto_Honorarios_TextChanged: " + Ex.Message;
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cmb_Hasta_Periodo_SelectedIndexChanged
    ///DESCRIPCIÓN          : Manejo del evento cambio de indice para los combos hasta
    ///                         bimestre y anio 
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 30-ago-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Hasta_Periodo_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Adeudos;

        // validar que hay una cuenta seleccionada y hay valores en el combo hasta anio
        if (Hdf_Cuenta_Predial_ID.Value != "" && Cmb_Hasta_Anio_Periodo.Items.Count > 0)
        {
            // cargar adeudos de la cuenta hasta el periodo seleccionado
            Dt_Adeudos = Rs_Consulta_Adeudos.Calcular_Recargos_Predial_Hasta_Bimestre(
                Hdf_Cuenta_Predial_ID.Value,
                Convert.ToInt32(Cmb_Hasta_Anio_Periodo.SelectedValue),
                Convert.ToInt32(Cmb_Hasta_Bimestre_Periodo.SelectedValue)
                );
            if (Dt_Adeudos.Rows.Count > 0)
            {
                Session["Tabla_Adeudos"] = Dt_Adeudos;
            }

            if (Dt_Adeudos.Rows.Count > 0)
            {
                Hdn_Monto_Impuesto.Value = (Rs_Consulta_Adeudos.p_Total_Rezago + Rs_Consulta_Adeudos.p_Total_Corriente).ToString("#,##0.00");
                Txt_Adeudo_Corriente.Text = Rs_Consulta_Adeudos.p_Total_Corriente.ToString("#,##0.00");
                Txt_Adeudo_Rezago.Text = Rs_Consulta_Adeudos.p_Total_Rezago.ToString("#,##0.00");
                //Txt_Monto_Recargos.Text = String.Format("{0:#,##0.00}", Rs_Consulta_Adeudos.p_Total_Recargos_Generados);

                Consulta_Descuentos(Hdf_Cuenta_Predial_ID.Value);

                Calcular_Total_Adeudos();
                Calcular_Total_Descuento();
                Calcular_Sub_Total();
                Calcular_Total_Anticipo();
                Calcular_Total_Convenio();
                Calcular_Parcialidades();
            }
        }

        Cmb_Hasta_Bimestre_Periodo.Focus();
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Total_Descuento
    /// DESCRIPCIÓN: Breve descripción de lo que hace la función.
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Calcular_Total_Descuento()
    {
        Decimal Monto_Ordinarios = 0;
        Decimal Monto_Moratorios = 0;
        Decimal Descuento_Recargos_Ordinarios = 0;
        Decimal Descuento_Recargos_Moratorios = 0;
        Decimal Total_Descuento = 0;

        Decimal.TryParse(Txt_Monto_Recargos.Text, out Monto_Ordinarios);
        Decimal.TryParse(Txt_Monto_Moratorios.Text, out Monto_Moratorios);
        Decimal.TryParse(Txt_Descuento_Recargos_Ordinarios.Text, out Descuento_Recargos_Ordinarios);
        Decimal.TryParse(Txt_Descuento_Recargos_Moratorios.Text, out Descuento_Recargos_Moratorios);

        Txt_Descuento_Recargos_Ordinarios.Text = Descuento_Recargos_Ordinarios.ToString("#,##0.00");
        Txt_Descuento_Recargos_Moratorios.Text = Descuento_Recargos_Moratorios.ToString("#,##0.00");

        // calcular total descuento
        Total_Descuento = Descuento_Recargos_Ordinarios + Descuento_Recargos_Moratorios;

        // mostrar valores en las cajas de texto
        Txt_Monto_Recargos.Text = Monto_Ordinarios.ToString("#,##0.00");
        Txt_Monto_Moratorios.Text = Monto_Moratorios.ToString("#,##0.00");
        Txt_Total_Descuento.Text = Total_Descuento.ToString("#,##0.00");
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Sumar_Totales_Parcialidades
    /// DESCRIPCIÓN: Sumar el total de parcialidades y mostrar en el grid de parcialidades
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Sumar_Totales_Parcialidades()
    {
        Decimal Total_Honorarios = 0;
        Decimal Total_Recargos_Ordinarios = 0;
        Decimal Total_Recargos_Moratorios = 0;
        Decimal Total_Impuesto = 0;
        Decimal Total_Importe = 0;
        TextBox Txt_Monto_Importe_Grid;
        Label Lbl_Txt_Monto_Importe_Grid;
        Label Lbl_Monto;
        decimal Monto;

        foreach (GridViewRow Fila_Grid in Grid_Parcialidades.Rows)
        {
            Decimal Monto_Importe = 0;

            // recuperar los valores de las etiquetas en los TemplateField
            Lbl_Monto = (Label)Fila_Grid.Cells[2].FindControl("Lbl_Txt_Grid_Monto_Honorarios");
            decimal.TryParse(HttpUtility.HtmlDecode(Lbl_Monto.Text.Replace("$", "")), out Monto);
            Total_Honorarios += Monto;

            Lbl_Monto = (Label)Fila_Grid.Cells[3].FindControl("Lbl_Txt_Grid_Monto_Recargos_Ordinarios");
            decimal.TryParse(HttpUtility.HtmlDecode(Lbl_Monto.Text.Replace("$", "")), out Monto);
            Total_Recargos_Ordinarios += Monto;

            Lbl_Monto = (Label)Fila_Grid.Cells[3].FindControl("Lbl_Txt_Grid_Monto_Recargos_Moratorios");
            decimal.TryParse(HttpUtility.HtmlDecode(Lbl_Monto.Text.Replace("$", "")), out Monto);
            Total_Recargos_Moratorios += Monto;

            if (Fila_Grid.Cells[5].Text.Trim().Replace("&nbsp;", "") != "")
            {
                Total_Impuesto += Convert.ToDecimal(Fila_Grid.Cells[5].Text.Replace("$", ""));
            }
            // tratar de extraer el dato de la etiqueta y si no se puede, del textbox del template
            Lbl_Txt_Monto_Importe_Grid = (Label)Fila_Grid.Cells[6].FindControl("Lbl_Txt_Grid_Monto_Importe");
            if (Lbl_Mensaje_Error != null)
            {
                Decimal.TryParse(Lbl_Txt_Monto_Importe_Grid.Text.Replace("$", ""), out Monto_Importe);
            }
            else
            {
                Txt_Monto_Importe_Grid = (TextBox)Fila_Grid.Cells[6].FindControl("Txt_Grid_Monto_Importe");
                if (Txt_Monto_Importe_Grid != null)
                {
                    Decimal.TryParse(Txt_Monto_Importe_Grid.Text.Replace("$", ""), out Monto_Importe);
                }
            }

            Total_Importe += Monto_Importe;
        }

        if (Grid_Parcialidades.Rows.Count > 0)
        {
            Grid_Parcialidades.FooterRow.Cells[2].Text = Total_Honorarios.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[3].Text = Total_Recargos_Ordinarios.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[4].Text = Total_Recargos_Moratorios.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[5].Text = Total_Impuesto.ToString("$#,##0.00");
            Grid_Parcialidades.FooterRow.Cells[6].Text = Total_Importe.ToString("$#,##0.00");
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Actualizar_Periodo_Impuesto_Parcialidades_Modificadas
    /// DESCRIPCIÓN: Actualiza el valor de la fila en el grid_parcialidades que recibe como parámetro
    ///         compara el valor en l a etiqueta con el de la caja de texto y pasa el resto a las siguientes
    /// PARÁMETROS:
    ///         1. Indice_Fila: fila del grid_parcialidades que se va a actualizar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 17-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Actualizar_Periodo_Impuesto_Parcialidades_Modificadas(int Indice_Fila)
    {
        TextBox Txt_Periodo;
        Label Lbl_Txt_Periodo;
        TextBox Txt_Periodo_Proximo;
        Label Lbl_Txt_Periodo_Proximo;
        Int32 No_Pago = 0;
        int Cantidad_Parcialidades;
        Dictionary<string, decimal> Dic_Adeudos;
        int Ultimo_Bimestre_Incluido = 0;
        int Ultimo_Anio_incluido = 0;
        int Anio_Inicial = 0;
        int Bimestre_Inicial = 0;
        int Anio_Final = 0;
        int Bimestre_Final = 0;
        string Periodo;
        decimal Monto_Impuesto;
        int Hasta_Anio_Seleccionado = 0;
        int Hasta_Bimestre_Seleccionado = 0;

        if (Session["Dic_Adeudos"] != null)
        {
            Dic_Adeudos = (Dictionary<string, decimal>)Session["Dic_Adeudos"];
        }
        else
        {
            Dic_Adeudos = Consulta_Adeudos_Diccionario();
        }

        int.TryParse(Txt_Numero_Parcialidades.Text, out Cantidad_Parcialidades);


        // tratar de recuperar el valor en la etiqueta del periodo
        Lbl_Txt_Periodo = (Label)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
        Txt_Periodo = (TextBox)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Txt_Grid_Periodo");

        // recuperar el número de pago
        Int32.TryParse(Grid_Parcialidades.Rows[Indice_Fila].Cells[0].Text, out No_Pago);

        // si es la última parcialidad, se copia el valor de la etiqueta (no se puede cambiar la última parcialidad)
        if (No_Pago == Cantidad_Parcialidades)
        {
            Txt_Periodo.Text = Lbl_Txt_Periodo.Text;
        }
        else if (No_Pago >= 0 && No_Pago < Cantidad_Parcialidades && Grid_Parcialidades.Rows.Count > 0) // si es el primer pago y el grid tiene más de una parcialidad
        {
            // recorrer las parcialidades anteriores en busca del último periodo incluido
            for (int Fila = 0; Fila < Indice_Fila; Fila++)
            {
                Lbl_Txt_Periodo_Proximo = (Label)Grid_Parcialidades.Rows[Fila].Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
                if (Lbl_Txt_Periodo_Proximo != null)
                {
                    Periodo = HttpUtility.HtmlDecode(Lbl_Txt_Periodo_Proximo.Text).Replace(" ", "");
                    if (Periodo.Length > 12)
                    {
                        int.TryParse(Periodo.Substring(Periodo.Length - 4, 4), out Ultimo_Anio_incluido);
                        int.TryParse(Periodo.Substring(Periodo.Length - 6, 1), out Ultimo_Bimestre_Incluido);
                    }
                }
            }

            // validar controles y actualizar el periodo de la fila seleccionada
            if (Txt_Periodo != null && Lbl_Txt_Periodo != null)
            {
                Periodo = HttpUtility.HtmlDecode(Txt_Periodo.Text).Replace(" ", "");
                if (Periodo.Length > 12)
                {
                    // aumentar el último bimestre incluido para obtener el primer bimestre a incluir 
                    if (Ultimo_Anio_incluido > 0 && Ultimo_Bimestre_Incluido > 0)
                    {
                        Anio_Inicial = Ultimo_Anio_incluido;
                        Bimestre_Inicial = Ultimo_Bimestre_Incluido;
                        Bimestre_Inicial++;
                        // si pasa de seis, regresar a uno y aumentar el año
                        if (Bimestre_Inicial >= 7)
                        {
                            Bimestre_Inicial = 1;
                            Anio_Inicial++;
                        }
                    }

                    int.TryParse(Periodo.Substring(Periodo.Length - 4, 4), out Anio_Final);
                    int.TryParse(Periodo.Substring(Periodo.Length - 6, 1), out Bimestre_Final);
                    // validar que el bimestre est el rango 1 a 6
                    if (Bimestre_Final > 6)
                        Bimestre_Final = 6;
                    else if (Bimestre_Final < 1)
                        Bimestre_Final = 1;

                    // si el año inicial es mayor al año final, descartar cambio
                    if (Ultimo_Anio_incluido > Anio_Final)
                    {
                        Txt_Periodo.Text = Lbl_Txt_Periodo.Text;
                    }
                    else
                    {
                        Monto_Impuesto = Importe_Predial_Periodo(Anio_Inicial, Bimestre_Inicial, Anio_Final, Bimestre_Final, Dic_Adeudos, out Periodo);
                        // actualizar importe y periodo de la fila actual
                        Grid_Parcialidades.Rows[Indice_Fila].Cells[5].Text = Monto_Impuesto.ToString("$#,##0.00");
                        Txt_Periodo.Text = Periodo;
                        Lbl_Txt_Periodo.Text = Periodo;

                        // comparar el ultimo bimestre incluido en el importe con el periodo seleccionado
                        int.TryParse(Cmb_Hasta_Anio_Periodo.SelectedValue, out Hasta_Anio_Seleccionado);
                        int.TryParse(Cmb_Hasta_Bimestre_Periodo.SelectedValue, out Hasta_Bimestre_Seleccionado);
                        // si son diferentes periodos el seleccionado y el último incluido, obtener el importe del siguiente periodo 
                        if (Anio_Final != Hasta_Anio_Seleccionado || Bimestre_Final != Hasta_Bimestre_Seleccionado)
                        {
                            Bimestre_Final++;
                            if (Bimestre_Final > 6)
                            {
                                Bimestre_Final = 1;
                                Anio_Final++;
                            }
                            Monto_Impuesto = Importe_Predial_Periodo(Anio_Final, Bimestre_Final, 0, 0, Dic_Adeudos, out Periodo);
                            // actualizar importe y periodo de la fila siguiente
                            Indice_Fila++;
                            Lbl_Txt_Periodo_Proximo = (Label)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
                            Txt_Periodo_Proximo = (TextBox)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Txt_Grid_Periodo");
                            if (Lbl_Txt_Periodo_Proximo != null)
                            {
                                Grid_Parcialidades.Rows[Indice_Fila].Cells[5].Text = Monto_Impuesto.ToString("$#,##0.00");
                                Txt_Periodo_Proximo.Text = Periodo;
                                Lbl_Txt_Periodo_Proximo.Text = Periodo;
                            }
                        }
                    }
                }
                else if (Periodo == "-" || Periodo == "")
                {
                    // no se incluye impuesto en la parcialidad
                    // actualizar fila actual con cero en importe y "-" en la etiqueta y caja de texto
                    Lbl_Txt_Periodo = (Label)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
                    Txt_Periodo = (TextBox)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Txt_Grid_Periodo");
                    if (Lbl_Txt_Periodo != null && Txt_Periodo != null)
                    {
                        // actualizar periodo
                        Txt_Periodo.Text = "-";
                        Lbl_Txt_Periodo.Text = "-";
                        Grid_Parcialidades.Rows[Indice_Fila].Cells[5].Text = "$0.00";
                    }
                    // agregar el resto de los bimestres (si hay) en la siguiente fila
                    Ultimo_Bimestre_Incluido++;
                    if (Ultimo_Bimestre_Incluido > 6)
                    {
                        Ultimo_Bimestre_Incluido = 1;
                        Ultimo_Anio_incluido++;
                    }
                    Indice_Fila++;
                    // actualizar datos fila
                    Monto_Impuesto = Importe_Predial_Periodo(Ultimo_Anio_incluido, Ultimo_Bimestre_Incluido, 0, 0, Dic_Adeudos, out Periodo);
                    Lbl_Txt_Periodo_Proximo = (Label)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
                    Txt_Periodo_Proximo = (TextBox)Grid_Parcialidades.Rows[Indice_Fila].Cells[1].FindControl("Txt_Grid_Periodo");
                    if (Lbl_Txt_Periodo_Proximo != null && Txt_Periodo_Proximo != null)
                    {
                        Txt_Periodo_Proximo.Text = Periodo;
                        Lbl_Txt_Periodo_Proximo.Text = Periodo;
                    }
                    Grid_Parcialidades.Rows[Indice_Fila].Cells[5].Text = Monto_Impuesto.ToString("$#,##0.00");

                }

            }

            // recorrer las parcialidades siguientes para asignar cero en Importe y "-" en Periodo
            for (int Fila = Indice_Fila + 1; Fila < Grid_Parcialidades.Rows.Count; Fila++)
            {
                Lbl_Txt_Periodo_Proximo = (Label)Grid_Parcialidades.Rows[Fila].Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
                Txt_Periodo_Proximo = (TextBox)Grid_Parcialidades.Rows[Fila].Cells[1].FindControl("Txt_Grid_Periodo");
                if (Lbl_Txt_Periodo_Proximo != null && Txt_Periodo_Proximo != null)
                {
                    // actualizar periodo
                    Txt_Periodo_Proximo.Text = "-";
                    Lbl_Txt_Periodo_Proximo.Text = "-";
                    Grid_Parcialidades.Rows[Fila].Cells[5].Text = "$0.00";
                }
            }

        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Importe_Predial_Periodo
    /// DESCRIPCIÓN: regresa un decimal con la suma de adeudos en el diccionario desde un periodo hasta otro dado como parámetro
    /// PARÁMETROS:
    /// 		1. Anio_Inicial: primer año a incluir de adeudo predial
    /// 		2. Bimestre_Inicial: bimestre del primer año a incluir de adeudo predial
    /// 		3. Anio_Final: último año que se va a incluir en el importe
    /// 		4. Bimestre_Final: bimestre del último año que se va a incluir en el importe
    /// 		5. Dic_Adeudos: Diccionario con adeudos de predial <adeudo, monto>
    /// 		6. Periodo: variable en la que se va a regresar el periodo del que se calcula el importe
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 17-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private decimal Importe_Predial_Periodo(int Anio_Inicial, int Bimestre_Inicial, int Anio_Final, int Bimestre_Final, Dictionary<string, decimal> Dic_Adeudos, out string Periodo)
    {
        decimal Importe = 0;
        int Desde_Bimestre = 0;
        int Hasta_Bimestre = 0;
        int Desde_Anio = 0;
        int Hasta_Anio = 0;
        int Anio;
        int Hasta_Anio_Seleccionado = 0;
        int Hasta_Bimestre_Seleccionado = 0;
        int Contador_Bimestre;

        Periodo = "-";
        // recorrer el diccionario para obtener primer y último periodo
        foreach (KeyValuePair<string, decimal> Adeudo in Dic_Adeudos)
        {
            int.TryParse(Adeudo.Key.Substring(1, 4), out Anio);
            if (Desde_Anio == 0 || Desde_Anio > Anio)
            {
                Desde_Anio = Anio;
            }
            if (Hasta_Anio == 0 || Hasta_Anio < Anio)
            {
                Hasta_Anio = Anio;
            }
        }
        for (Contador_Bimestre = 6; Contador_Bimestre > 0; Contador_Bimestre--)
        {
            if (Dic_Adeudos.ContainsKey(Contador_Bimestre.ToString() + Desde_Anio.ToString()))
            {
                Desde_Bimestre = Contador_Bimestre;
            }
        }
        for (Contador_Bimestre = 1; Contador_Bimestre <= 6; Contador_Bimestre++)
        {
            if (Dic_Adeudos.ContainsKey(Contador_Bimestre.ToString() + Hasta_Anio.ToString()))
            {
                Hasta_Bimestre = Contador_Bimestre;
            }
        }

        // recuperar periodo seleccionado
        int.TryParse(Cmb_Hasta_Anio_Periodo.SelectedValue, out Hasta_Anio_Seleccionado);
        int.TryParse(Cmb_Hasta_Bimestre_Periodo.SelectedValue, out Hasta_Bimestre_Seleccionado);

        // validar parámetros recibidos, sustituir periodos recibidos si estos, no continen valor
        if (Anio_Inicial <= 0 || Bimestre_Inicial <= 0)
        {
            Anio_Inicial = Desde_Anio;
            Bimestre_Inicial = Desde_Bimestre;
        }

        // que el año seleccionado no sea menor que cero, mayor que el año seleccionado (si el año seleccionado mayor que cero) o que el bimestre  sea menor que cero
        if (Anio_Final <= 0 || (Hasta_Anio_Seleccionado > 0 && Anio_Final > Hasta_Anio_Seleccionado) || Bimestre_Final <= 0)
        {
            // asignar periodo seleccionado o ultimo bimestre con adeudos
            if (Hasta_Anio_Seleccionado > 0 && Hasta_Bimestre_Seleccionado > 0)
            {
                Anio_Final = Hasta_Anio_Seleccionado;
                Bimestre_Final = Hasta_Bimestre_Seleccionado;
            }
            else
            {
                Anio_Final = Hasta_Anio;
                Bimestre_Final = Hasta_Bimestre;
            }
        }

        // obtener el impuesto sumando todos los bimestres desde Anio_Inicial hasta Anio_Final
        for (int Contador_Anio = Anio_Inicial; Contador_Anio <= Anio_Final; Contador_Anio++)
        {
            if (Anio_Inicial == Anio_Final && Contador_Anio == Anio_Inicial)
            {
                for (Contador_Bimestre = Bimestre_Inicial; Contador_Bimestre <= Bimestre_Final; Contador_Bimestre++)
                {
                    // si el diccionario contiene adeudo para el periodo, sumar al Importe
                    if (Dic_Adeudos.ContainsKey(Contador_Bimestre.ToString() + Contador_Anio.ToString()))
                    {
                        Importe += Dic_Adeudos[Contador_Bimestre.ToString() + Contador_Anio.ToString()];
                    }
                }
            }
            // si es el Anio_Inicial, incluir bimestres desde bimestre inicial
            else if (Contador_Anio == Anio_Inicial)
            {
                for (Contador_Bimestre = Bimestre_Inicial; Contador_Bimestre <= 6; Contador_Bimestre++)
                {
                    // si el diccionario contiene adeudo para el periodo, sumar al Importe
                    if (Dic_Adeudos.ContainsKey(Contador_Bimestre.ToString() + Contador_Anio.ToString()))
                    {
                        Importe += Dic_Adeudos[Contador_Bimestre.ToString() + Contador_Anio.ToString()];
                    }
                }
            }
            else if (Contador_Anio == Anio_Final) // incluir bimestres hasta bimestre final
            {
                for (Contador_Bimestre = 1; Contador_Bimestre <= Bimestre_Final; Contador_Bimestre++)
                {
                    // si el diccionario contiene adeudo para el periodo, sumar al Importe
                    if (Dic_Adeudos.ContainsKey(Contador_Bimestre.ToString() + Contador_Anio.ToString()))
                    {
                        Importe += Dic_Adeudos[Contador_Bimestre.ToString() + Contador_Anio.ToString()];
                    }
                }
            }
            else // incluir bimestres 1 a 6
            {
                for (Contador_Bimestre = 1; Contador_Bimestre <= 6; Contador_Bimestre++)
                {
                    // si el diccionario contiene adeudo para el periodo, sumar al Importe
                    if (Dic_Adeudos.ContainsKey(Contador_Bimestre.ToString() + Contador_Anio.ToString()))
                    {
                        Importe += Dic_Adeudos[Contador_Bimestre.ToString() + Contador_Anio.ToString()];
                    }
                }
            }
        }

        // formar el Periodo incluido en el Importe si este es mayor que cero
        if (Importe > 0)
        {
            Periodo = Bimestre_Inicial.ToString() + "/" + Anio_Inicial.ToString() + "-" + Bimestre_Final.ToString() + "/" + Anio_Final.ToString();
        }

        return Importe;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Actualizar_Importe_Parcialidades_Modificadas
    /// DESCRIPCIÓN: Actualiza el valor de la fila en el grid_parcialidades que recibe como parámetro
    ///         compara el valor en la etiqueta con el de la caja de texto y pasa el resto a las siguientes
    /// PARÁMETROS:
    ///         1. Indice_Fila: fila del grid_parcialidades que se va a actualizar
    ///         2. Indice_Celda: celda del grid_parcialidades que se va a actualizar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 17-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Actualizar_Importe_Parcialidades_Modificadas(int Indice_Fila, int Indice_Celda)
    {
        TextBox Txt_Monto;
        Label Lbl_Txt_Monto;
        TextBox Txt_Monto_Siguiente;
        Label Lbl_Txt_Monto_Siguiente;
        Int32 No_Pago = 0;
        int Cantidad_Parcialidades;
        decimal Monto_Restante = 0;
        decimal Monto_Ingresado;
        decimal Monto_Parcial;
        decimal Descuento;
        Dictionary<int, string> Dic_Nombre_Controles = new Dictionary<int, string> 
        { 
            {2, "Txt_Grid_Monto_Honorarios"},
            {3, "Txt_Grid_Monto_Recargos_Ordinarios"},
            {4, "Txt_Grid_Monto_Recargos_Moratorios"}
        };

        int.TryParse(Txt_Numero_Parcialidades.Text, out Cantidad_Parcialidades);


        // tratar de recuperar el valor en la etiqueta del importe
        Lbl_Txt_Monto = (Label)Grid_Parcialidades.Rows[Indice_Fila].Cells[Indice_Celda].FindControl("Lbl_" + Dic_Nombre_Controles[Indice_Celda]);
        Txt_Monto = (TextBox)Grid_Parcialidades.Rows[Indice_Fila].Cells[Indice_Celda].FindControl(Dic_Nombre_Controles[Indice_Celda]);

        // recuperar el numero de pago
        Int32.TryParse(Grid_Parcialidades.Rows[Indice_Fila].Cells[0].Text, out No_Pago);

        // obtener monto ingresado (caja de texto) y monto restante (etiqueta)
        decimal.TryParse(Txt_Monto.Text, out Monto_Ingresado);

        // dependiendo de la fila, tomar el valor original de la caja de texto
        if (Indice_Celda == 2)    // honorarios
        {
            decimal.TryParse(Txt_Adeudo_Honorarios.Text, out Monto_Restante);
        }
        else if (Indice_Celda == 3) // recargos ordinarios
        {
            decimal.TryParse(Txt_Monto_Recargos.Text, out Monto_Restante);
            decimal.TryParse(Txt_Descuento_Recargos_Ordinarios.Text, out Descuento);
            Monto_Restante -= Descuento;
        }
        else if (Indice_Celda == 4) // recargos moratorios
        {
            decimal.TryParse(Txt_Monto_Moratorios.Text, out Monto_Restante);
            decimal.TryParse(Txt_Descuento_Recargos_Moratorios.Text, out Descuento);
            Monto_Restante -= Descuento;
        }

        // si es la última parcialidad, se copia el valor de la etiqueta (no se puede cambiar la última parcialidad)
        if (No_Pago == Cantidad_Parcialidades)
        {
            Txt_Monto.Text = Lbl_Txt_Monto.Text.Replace("$", "");
        }
        else if (No_Pago >= 0 && No_Pago < Cantidad_Parcialidades && Grid_Parcialidades.Rows.Count > 0) // si es el primer pago y el grid tiene más de una parcialidad
        {
            // restar el valor de las filas anteriores para obtener el monto restante
            for (int i = 0; i < Indice_Fila; i++)
            {
                Txt_Monto_Siguiente = (TextBox)Grid_Parcialidades.Rows[i].Cells[Indice_Celda].FindControl(Dic_Nombre_Controles[Indice_Celda]);
                decimal.TryParse(Txt_Monto_Siguiente.Text, out Monto_Parcial);
                Monto_Restante -= Monto_Parcial;
            }

            // si el monto ingresado es mayor al monto restante o menor que cero, descartar monto ingresado
            if (Monto_Ingresado > Monto_Restante || Monto_Ingresado < 0)
            {
                Txt_Monto.Text = Monto_Restante.ToString("#,##0.00");
                Lbl_Txt_Monto.Text = Monto_Restante.ToString("$#,##0.00");

                // poner cero en las celdas (Indice_Celda) de las filas siguientes
                for (int i = Indice_Fila + 1; i < Grid_Parcialidades.Rows.Count; i++)
                {
                    Lbl_Txt_Monto_Siguiente = (Label)Grid_Parcialidades.Rows[i].Cells[Indice_Celda].FindControl("Lbl_" + Dic_Nombre_Controles[Indice_Celda]);
                    Txt_Monto_Siguiente = (TextBox)Grid_Parcialidades.Rows[i].Cells[Indice_Celda].FindControl(Dic_Nombre_Controles[Indice_Celda]);
                    if (Lbl_Txt_Monto_Siguiente != null && Txt_Monto_Siguiente != null)
                    {
                        Lbl_Txt_Monto_Siguiente.Text = "$0.00";
                        Txt_Monto_Siguiente.Text = "0.00";
                    }
                }
            }
            else  // actualizar montos de la fila siguiente y la actual del grid
            {
                // actualizar el monto ingresado en la fila actual del grid
                Txt_Monto.Text = Monto_Ingresado.ToString("#,##0.00");
                Lbl_Txt_Monto.Text = Monto_Ingresado.ToString("$#,##0.00");
                // asignar cero a todos los renglones que siguen
                // actualizar honorarios de la siguiente fila
                Lbl_Txt_Monto_Siguiente = (Label)Grid_Parcialidades.Rows[Indice_Fila + 1].Cells[Indice_Celda].FindControl("Lbl_" + Dic_Nombre_Controles[Indice_Celda]);
                Txt_Monto_Siguiente = (TextBox)Grid_Parcialidades.Rows[Indice_Fila + 1].Cells[Indice_Celda].FindControl(Dic_Nombre_Controles[Indice_Celda]);
                if (Lbl_Txt_Monto_Siguiente != null && Txt_Monto_Siguiente != null)
                {
                    Txt_Monto_Siguiente.Text = (Monto_Restante - Monto_Ingresado).ToString("#,##0.00");
                    Lbl_Txt_Monto_Siguiente.Text = (Monto_Restante - Monto_Ingresado).ToString("$#,##0.00");
                }
                // poner cero en las celdas (Indice_Celda) de las filas siguientes
                for (int i = Indice_Fila + 2; i < Grid_Parcialidades.Rows.Count; i++)
                {
                    Lbl_Txt_Monto_Siguiente = (Label)Grid_Parcialidades.Rows[i].Cells[Indice_Celda].FindControl("Lbl_" + Dic_Nombre_Controles[Indice_Celda]);
                    Txt_Monto_Siguiente = (TextBox)Grid_Parcialidades.Rows[i].Cells[Indice_Celda].FindControl(Dic_Nombre_Controles[Indice_Celda]);
                    if (Lbl_Txt_Monto_Siguiente != null && Txt_Monto_Siguiente != null)
                    {
                        Lbl_Txt_Monto_Siguiente.Text = "$0.00";
                        Txt_Monto_Siguiente.Text = "0.00";
                    }
                }
            }
        }

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Sumar_Importe_Parcialidades
    /// DESCRIPCIÓN: Actualiza el valor del importe de las filas en el grid_parcialidades
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 17-abr-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Sumar_Importe_Parcialidades()
    {
        Label Lbl_Txt_Monto_Siguiente;
        TextBox Txt_Importe;
        Label Lbl_Txt_Importe;
        decimal Monto_Parcial;
        decimal Total_Importe;
        Dictionary<int, string> Dic_Nombre_Controles = new Dictionary<int, string> 
        { 
            {2, "Txt_Grid_Monto_Honorarios"},
            {3, "Txt_Grid_Monto_Recargos_Ordinarios"},
            {4, "Txt_Grid_Monto_Recargos_Moratorios"}
        };

        // actualizar el importe de las filas
        for (int Fila_Actual = 0; Fila_Actual < Grid_Parcialidades.Rows.Count; Fila_Actual++)
        {
            Total_Importe = 0;
            // racuperar el valor de las etiquetas en las columnas Honorarios, Recargos ordinarios y Recargos moratorios
            for (int Contador_Celda = 2; Contador_Celda <= 4; Contador_Celda++)
            {
                Lbl_Txt_Monto_Siguiente = (Label)Grid_Parcialidades.Rows[Fila_Actual].Cells[Contador_Celda].FindControl("Lbl_" + Dic_Nombre_Controles[Contador_Celda]);
                decimal.TryParse(Lbl_Txt_Monto_Siguiente.Text.Replace("$", ""), out Monto_Parcial);
                Total_Importe += Monto_Parcial;
            }
            // agregar el monto del impuesto predial
            decimal.TryParse(Grid_Parcialidades.Rows[Fila_Actual].Cells[5].Text.Replace("$", ""), out Monto_Parcial);
            Total_Importe += Monto_Parcial;

            // si es la primera fila del grid, copiar importe al total anticipo y calcular porcentaje anticipo
            if (Fila_Actual == 0)
            {
                decimal Subtotal;
                decimal Porcentaje_Anticipo;
                decimal Saldo;
                // recuperar subtotal (adeudo menos descuento)
                decimal.TryParse(Txt_Sub_Total.Text, out Subtotal);
                // calcular porcentaje anticipo redondeado a dos decimales
                Porcentaje_Anticipo = Math.Round((Total_Importe * 100M / Subtotal), 2, MidpointRounding.AwayFromZero);
                Saldo = Subtotal - Total_Importe;
                // mostrar valores en las cajas de texto
                Txt_Total_Anticipo.Text = Total_Importe.ToString("#,##0.00");
                Txt_Porcentaje_Anticipo.Text = Porcentaje_Anticipo.ToString("#,##0.00");
                Txt_Total_Convenio.Text = Saldo.ToString("#,##0.00");
            }

            // actualizar el campo Importe
            Txt_Importe = (TextBox)Grid_Parcialidades.Rows[Fila_Actual].Cells[6].FindControl("Txt_Grid_Monto_Importe");
            Lbl_Txt_Importe = (Label)Grid_Parcialidades.Rows[Fila_Actual].Cells[6].FindControl("Lbl_Txt_Grid_Monto_Importe");
            if (Txt_Importe != null)
            {
                Txt_Importe.Text = Total_Importe.ToString("#,##0.00");
            }
            if (Lbl_Txt_Importe != null)
            {
                Lbl_Txt_Importe.Text = Total_Importe.ToString("$#,##0.00");
            }
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Calcular_Fecha_Vencimiento
    /// DESCRIPCIÓN: Revisar las parcialidades en busca de parcialidades vencidas 
    ///             (con estatus POR PAGAR y fecha de vencimiento de hace mas de 10 dias habiles)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 29-nov-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Obtener_Fecha_Vencimiento()
    {
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Calcular_Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        DateTime Fecha_Periodo;
        DateTime Fecha_Vencimiento;
        int Dias = 0;
        int Meses = 0;

        // recorrer las parcialidades del convenio
        for (int Pago = 0; Pago < Grid_Parcialidades.Rows.Count; Pago++)
        {
            // si el estatus de la parcialidad es INCUMPLIDO
            if (Grid_Parcialidades.Rows[Pago].Cells[8].Text.Trim() == "INCUMPLIDO")
            {
                // obtener la fecha de vencimiento de la parcialidad
                DateTime.TryParse(Grid_Parcialidades.Rows[Pago].Cells[7].Text, out Fecha_Periodo);
                Fecha_Vencimiento = Calcular_Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo.ToShortDateString(), "10");
                // obtener el tiempo transcurrido desde la fecha de vencimiento
                Calcular_Tiempo_Entre_Fechas(Fecha_Vencimiento, DateTime.Now, out Dias, out Meses);
                // si el numero de dias transcurridos en mayor que cero, escribir fecha de vencimiento
                if (Dias > 0)
                {
                    Txt_Fecha_Vencimiento.Text = Fecha_Vencimiento.ToString("dd/MMM/yyyy");
                }
                else
                {
                    Txt_Fecha_Vencimiento.Text = "";
                }

                // abandonar el ciclo for
                break;
            }
        }

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Obtener_Fecha_Periodo
    /// DESCRIPCIÓN: Calcula la fecha para el siguiente periodo a partir de la fecha de la parcialidad
    ///             anterior y la periodicidad (suma la cantidad de la periodiciad menos 1 día y 
    ///             le agrega un dia hábil mediante el método Calcular_Fecha)
    /// PARÁMETROS:
    /// 		1. Fecha_Periodo_Anterior: fecha a partir de la que se calculara la siguiente fecha
    /// 		2. Periodicidad: especifica el periodo de tiempo a agregar para obtener la fecha
    /// 		3. Numero_Parcialidad: valor entero con el número de parcialidad a calcular
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-ago-2011
    /// MODIFICÓ: Roberto González Oseguera
    /// FECHA_MODIFICÓ: 17-sep-2011
    /// CAUSA_MODIFICACIÓN: Cambio en la forma de calcular las parcialidades (ahora es en dias naturales)
    ///*******************************************************************************************************
    private DateTime Obtener_Fecha_Periodo(DateTime Fecha_Periodo_Anterior, String Periodicidad, int Numero_Parcialidad)
    {
        Cls_Ope_Pre_Dias_Inhabiles_Negocio Dias_Inhabilies = new Cls_Ope_Pre_Dias_Inhabiles_Negocio();
        DateTime Fecha_Periodo = DateTime.Now;

        switch (Periodicidad)
        {
            case "7":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddDays(7 * Numero_Parcialidad).AddDays(-1);
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "14":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddDays(14 * Numero_Parcialidad).AddDays(-1);
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "15":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddDays(15 * Numero_Parcialidad).AddDays(-1);
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "30":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "2MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(2 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "3MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(3 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "4MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(4 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "5MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(5 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "6MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(6 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "7MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(7 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "8MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(8 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "9MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(9 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "10MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(10 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "11MES":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddMonths(11 * Numero_Parcialidad).AddDays(-1);
                // sumar un dia habil para asegurar que quede en dia habil
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
            case "365":
                Fecha_Periodo_Anterior = Fecha_Periodo_Anterior.AddYears(1 * Numero_Parcialidad).AddDays(-1);
                Fecha_Periodo = Dias_Inhabilies.Calcular_Fecha(Fecha_Periodo_Anterior.ToShortDateString(), "1");
                break;
        }
        return Fecha_Periodo;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Porcentaje_Anticipo_TextChanged
    ///DESCRIPCIÓN          : Recalcular descuentos, totales y parcialidades llamando métodos
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Porcentaje_Anticipo_TextChanged(object sender, EventArgs e)
    {
        Calcular_Total_Anticipo();
        Calcular_Total_Convenio();
        Calcular_Parcialidades();

        //Txt_Total_Anticipo.Focus();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Txt_Total_Anticipo_TextChanged
    ///DESCRIPCIÓN          : Recalcular descuentos, totales y parcialidades llamando métodos
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Txt_Total_Anticipo_TextChanged(object sender, EventArgs e)
    {
        Decimal Sub_Total = 0;
        Decimal Total_Anticipo = 0;
        Decimal Porcentaje_Anticipo = 0;

        // tratar de obtener valores de subtotal y porcentaje anticipo
        if (Decimal.TryParse(Txt_Sub_Total.Text, out Sub_Total) &&
        Decimal.TryParse(Txt_Total_Anticipo.Text, out Total_Anticipo))
        {
            // calcular anticipo
            Porcentaje_Anticipo = (Total_Anticipo * 100) / Sub_Total;
        }
        // mostrar valores en las cajas de texto
        Txt_Sub_Total.Text = Sub_Total.ToString("#,##0.00");
        Txt_Porcentaje_Anticipo.Text = Porcentaje_Anticipo.ToString("##0.##");
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("#,##0.00");

        Calcular_Total_Convenio();
        Calcular_Parcialidades();

        //Txt_Total_Anticipo.Focus();
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Porcentaje_Anticipo
    ///DESCRIPCIÓN          : Recalcular porcentaje de anticipo con el subtotal y mostrar
    ///                     en campo correspondiente
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Decimal Calcular_Porcentaje_Anticipo()
    {
        Decimal Sub_Total = 0;
        Decimal Total_Anticipo = 0;
        Decimal Porcentaje_Anticipo = 0;

        // tratar de obtener valores de subtotal y porcentaje anticipo
        if (Decimal.TryParse(Txt_Sub_Total.Text, out Sub_Total) &&
        Decimal.TryParse(Txt_Porcentaje_Anticipo.Text, out Porcentaje_Anticipo))
        {
            // calcular anticipo
            Porcentaje_Anticipo = (Total_Anticipo * 100) / Sub_Total;
            // mostrar valores en las cajas de texto
            Txt_Sub_Total.Text = Sub_Total.ToString("#,##0.00");
            Txt_Porcentaje_Anticipo.Text = Porcentaje_Anticipo.ToString("##0.##");
            Txt_Total_Anticipo.Text = Total_Anticipo.ToString("#,##0.00");
        }

        return Porcentaje_Anticipo;
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Calcular_Total_Anticipo
    ///DESCRIPCIÓN          : Recalcular el anticipo tomando el subtotal y el 
    ///                     porcentaje de descuento y mostrar en el campo correspondiente
    ///PARAMETROS: 
    ///CREO                 : Roberto González Oseguera
    ///FECHA_CREO           : 31/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private Decimal Calcular_Total_Anticipo()
    {
        Decimal Sub_Total = 0;
        Decimal Porcentaje_Anticipo = 0;
        Decimal Total_Anticipo = 0;

        // tratar de obtener valores de subtotal y porcentaje anticipo
        Decimal.TryParse(Txt_Sub_Total.Text, out Sub_Total);
        Decimal.TryParse(Txt_Porcentaje_Anticipo.Text, out Porcentaje_Anticipo);

        // calcular anticipo
        Total_Anticipo = Sub_Total * Porcentaje_Anticipo / 100;
        // mostrar valores en las cajas de texto
        Txt_Sub_Total.Text = Sub_Total.ToString("#,##0.00");
        Txt_Porcentaje_Anticipo.Text = Porcentaje_Anticipo.ToString("##0.##");
        Txt_Total_Anticipo.Text = Total_Anticipo.ToString("#,##0.00");

        return Total_Anticipo;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Txt_Cuenta_Predial_TextChanged
    /// DESCRIPCIÓN: Manejo del evento cambio de texto en la caja TxT_Cuenta_Predial
    ///                actualiza los datos del propietario
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 31-ago-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    //protected void Txt_Cuenta_Predial_TextChanged()
    //{
    //    if (Hdf_Cuenta_Predial_ID.Value.Length <= 0)
    //    {
    //        Txt_Propietario.Text = "";
    //        Txt_Colonia.Text = "";
    //        Txt_Calle.Text = "";
    //        Txt_No_Exterior.Text = "";
    //        Txt_No_Interior.Text = "";
    //    }
    //    else
    //    {
    //        Cls_Cat_Pre_Cuentas_Predial_Negocio Cuenta = new Cls_Cat_Pre_Cuentas_Predial_Negocio();
    //        Cuenta.P_Cuenta_Predial_ID = Hdf_Cuenta_Predial_ID.Value;
    //        Cuenta = Cuenta.Consultar_Datos_Propietario();
    //        Txt_Calle.Text = Cuenta.P_Nombre_Calle;
    //        Txt_Propietario.Text = Cuenta.P_Nombre_Propietario;
    //        Txt_Colonia.Text = Cuenta.P_Nombre_Colonia;
    //        Hdf_Propietario_ID.Value = Cuenta.P_Propietario_ID;
    //        Txt_No_Exterior.Text = Cuenta.P_No_Exterior;
    //        Txt_No_Interior.Text = Cuenta.P_No_Interior;
    //        // copiar en el solicitante, solo si esta seleccionado PROPIETARIO
    //        if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
    //        {
    //            Txt_Solicitante.Text = Txt_Propietario.Text;
    //            Txt_RFC.Text = Cuenta.P_RFC_Propietario;
    //        }
    //        Hdf_RFC_Propietario.Value = Txt_RFC.Text;
    //    }
    //}

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Imprimir_Click
    /// DESCRIPCION : Imprimir formato para pago y una vez pagado el anticipo, imprimir convenio
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 04-sep-2010
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Imprimir_Click(object sender, ImageClickEventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        // verificar que hay un convenio seleccionado
        if (Hdf_No_Convenio.Value != "")
        {
            // verificar que el convenio tiene estatus LISTO
            if (Cmb_Estatus.SelectedValue == "ACTIVO")
            {
                try
                {
                    //Insertar_Pasivo();
                    // solo se imprime folio si el anticipo no se ha pagado
                    if (Grid_Parcialidades.Rows[0].Cells[8].Text == "POR PAGAR")
                    {
                        Imprimir_Reporte(Crear_Ds_Imprimir_Reporte(), "Rpt_Pre_Convenio_Predial_Pago.rpt", "Folio_Convenio");
                    }
                    Imprimir_Convenio();

                }
                catch (Exception Ex)
                {
                    Lbl_Mensaje_Error.Visible = true;
                    Lbl_Mensaje_Error.Text = "Imprimir convenio: " + Ex.Message;
                    Img_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Visible = true;
                Img_Error.Visible = true;
                Lbl_Mensaje_Error.Text = "Sólo se pueden imprimir convenios con estatus: ACTIVO<br />";
            }
        }
        else
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Seleccione el convenio que desea imprimir<br />";
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Chk_Parcialidades_Manuales_CheckedChanged
    /// DESCRIPCION : Si se activa, llama un método para borrar la tabla de parcialidades 
    ///         y cargar todo en la primera parcialidad para que el usuario vaya formando
    ///         las parcialidades; si se desactiva, se llama al método que calcula las parcialidades de forma automática
    /// PARAMETROS  : 
    /// CREO        : Roberto González Oseguera
    /// FECHA_CREO  : 16-abr-2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Chk_Parcialidades_Manuales_CheckedChanged(object sender, EventArgs e)
    {
        Lbl_Mensaje_Error.Visible = false;
        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";

        try
        {
            // solo si se está editando
            if (Btn_Nuevo.ToolTip == "Dar de Alta" || Btn_Modificar.ToolTip == "Actualizar")
            {
                // si se activa el checkbox, llamar al método que inicializa controles para entrada manual de parcialidades
                if (Chk_Parcialidades_Manuales.Checked == true)
                {
                    // ya debe haber una cuenta seleccionada, número de parcialidades y periodicidad del pago
                    if (!string.IsNullOrEmpty(Hdf_Cuenta_Predial_ID.Value) && Txt_Numero_Parcialidades.Text != "" && Cmb_Periodicidad_Pago.SelectedIndex > 0)
                    {
                        Grid_Parcialidades_Manuales = true;
                        Habilitar_Parcialidades_Manuales();

                        // desactivar cajas de texto anticipo
                        Txt_Porcentaje_Anticipo.Enabled = false;
                        Txt_Total_Anticipo.Enabled = false;
                        Txt_Porcentaje_Anticipo.Text = "0.00";
                        Txt_Total_Anticipo.Text = "0.00";
                    }
                }
                else
                {
                    Grid_Parcialidades_Editable = true;
                    Grid_Parcialidades_Manuales = false;
                    Grid_Parcialidades.DataSource = null;
                    Grid_Parcialidades.DataBind();

                    Calcular_Parcialidades();
                    // activar cajas de texto anticipo
                    Txt_Porcentaje_Anticipo.Enabled = true;
                    Txt_Total_Anticipo.Enabled = true;
                    // limpiar sesión de diccionario adeudos
                    Session.Remove("Dic_Adeudos");
                }

            }
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Parcialidades_Manuales: " + Ex.Message;
            Img_Error.Visible = true;
        }
    }

    #endregion EVENTOS

    #region Impresion Folios

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Reporte
    /// DESCRIPCIÓN: Genera el reporte de Crystal con los datos proporcionados en el DataTable 
    /// PARÁMETROS:
    /// 		1. Ds_Convenio: Dataset con datos a imprimir
    /// 		2. Nombre_Reporte: Nombre del archivo de reporte .rpt
    /// 		3. Nombre_Archivo: Nombre del archivo a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Reporte(DataSet Ds_Convenio, String Nombre_Reporte, String Nombre_Archivo)
    {
        ReportDocument Reporte = new ReportDocument();
        String Ruta = Server.MapPath("../Rpt/Predial/" + Nombre_Reporte);

        try
        {
            Reporte.Load(Ruta);
            Reporte.SetDataSource(Ds_Convenio);
        }
        catch
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "No se pudo cargar el reporte para su impresión";
        }

        String PDF_Convenio = Nombre_Archivo + ".pdf";  // Es el nombre del PDF que se va a generar 
        try
        {
            ExportOptions Export_Options_Calculo = new ExportOptions();
            DiskFileDestinationOptions Disk_File_Destination_Options_Calculo = new DiskFileDestinationOptions();
            Disk_File_Destination_Options_Calculo.DiskFileName = Server.MapPath("../../Reporte/" + PDF_Convenio);
            Export_Options_Calculo.ExportDestinationOptions = Disk_File_Destination_Options_Calculo;
            Export_Options_Calculo.ExportDestinationType = ExportDestinationType.DiskFile;
            Export_Options_Calculo.ExportFormatType = ExportFormatType.PortableDocFormat;
            Reporte.Export(Export_Options_Calculo);

            Mostrar_Reporte(PDF_Convenio, "Formato");
        }
        catch (Exception Ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = Ex.Message.ToString();
        }
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Mostrar_Reporte
    /// DESCRIPCIÓN: Visualiza en pantalla el reporte indicado
    /// PARÁMETROS:
    /// 		1. Nombre_Reporte: Nombre del reporte a generar
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Mostrar_Reporte(String Nombre_Reporte, String Tipo)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato",
                "window.open('" + Pagina +
                "', '" + Tipo + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Error al mostrar el reporte. Error: [" + Ex.Message + "]");
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Crear_Ds_Imprimir_Reporte
    ///DESCRIPCIÓN          : Crea un Dataset con los datos del cálculo seleccionado (NO_CALCULO )
    ///PARAMETROS: 
    ///CREO                 : Roberto Gonzalez Oseguera
    ///FECHA_CREO           : 20-ago-2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private DataSet Crear_Ds_Imprimir_Reporte()
    {
        Ds_Pre_Convenio_Predial_Pago Ds_Convenio = new Ds_Pre_Convenio_Predial_Pago();
        TextBox Txt_Monto_Importe_Grid;
        Label Lbl_Txt_Monto_Importe_Grid;
        Label Lbl_Monto_Grid;
        DataRow Dr_Convenio = Ds_Convenio.Tables[0].NewRow();

        Decimal Predial = 0;
        Decimal Recargos = 0;
        Decimal Moratorios = 0;
        Decimal Honorarios = 0;
        Decimal Importe = 0;

        // obtener montos
        Lbl_Monto_Grid = (Label)Grid_Parcialidades.Rows[0].Cells[2].FindControl("Lbl_Txt_Grid_Monto_Honorarios");
        Decimal.TryParse(Lbl_Monto_Grid.Text.Replace("$", ""), out Honorarios);
        Lbl_Monto_Grid = (Label)Grid_Parcialidades.Rows[0].Cells[3].FindControl("Lbl_Txt_Grid_Monto_Recargos_Ordinarios");
        Decimal.TryParse(Lbl_Monto_Grid.Text.Replace("$", ""), out Recargos);
        Lbl_Monto_Grid = (Label)Grid_Parcialidades.Rows[0].Cells[4].FindControl("Lbl_Txt_Grid_Monto_Recargos_Moratorios");
        Decimal.TryParse(Lbl_Monto_Grid.Text.Replace("$", ""), out Moratorios);
        Decimal.TryParse(Grid_Parcialidades.Rows[0].Cells[5].Text.Replace("$", ""), out Predial);

        Lbl_Txt_Monto_Importe_Grid = (Label)Grid_Parcialidades.Rows[0].Cells[6].FindControl("Lbl_Txt_Grid_Monto_Importe");
        if (Lbl_Txt_Monto_Importe_Grid != null)
        {
            if (!Decimal.TryParse(Lbl_Txt_Monto_Importe_Grid.Text.Replace("$", ""), out Importe))
            {
                Txt_Monto_Importe_Grid = (TextBox)Grid_Parcialidades.Rows[0].Cells[6].FindControl("Txt_Grid_Monto_Importe");
                if (Txt_Monto_Importe_Grid != null)
                {
                    Decimal.TryParse(Txt_Monto_Importe_Grid.Text.Replace("$", ""), out Importe);
                }
            }
        }
        // insertar datos en la fila instanciada directamente de los controles en pantalla
        Dr_Convenio["NO_CONVENIO"] = Txt_Numero_Convenio.Text;
        Dr_Convenio["FOLIO"] = "CPRE" + Convert.ToInt32(Hdf_No_Convenio.Value);
        Dr_Convenio["UBICACION"] = Txt_Calle.Text + " " + Txt_No_Exterior.Text + " "
            + Txt_No_Interior.Text + " " + Txt_Colonia.Text;
        Dr_Convenio["CUENTA_PREDIAL"] = Txt_Cuenta_Predial.Text;
        Dr_Convenio["PROPIETARIO"] = Txt_Propietario.Text;
        Dr_Convenio["MONTO_PREDIAL"] = Predial.ToString("#,##0.00");
        Dr_Convenio["MONTO_RECARGOS"] = Recargos.ToString("#,##0.00");
        Dr_Convenio["MONTO_MORATORIOS"] = Moratorios.ToString("#,##0.00");
        Dr_Convenio["MONTO_HONORARIOS"] = Honorarios.ToString("#,##0.00");
        Dr_Convenio["IMPORTE_PARCIALIDAD"] = Importe.ToString("#,##0.00");
        Dr_Convenio["FECHA_SIGUIENTE_PAGO"] = Grid_Parcialidades.Rows[1].Cells[7].Text;
        // agregar fila a la tabla
        Ds_Convenio.Tables[0].Rows.Add(Dr_Convenio);

        return Ds_Convenio;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Imprimir_Convenio
    /// DESCRIPCIÓN: Generar convenio (con OpenXML SDK a partir de documento con controles de contenido)
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 04-sep-2011
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Imprimir_Convenio()
    {
        string Ruta_Plantilla = Server.MapPath("PlantillasWord/" + "Formato_Convenio_Predial.docx");
        string Documento_Salida = Server.MapPath("../../Reporte/" + "Convenio_Predial.docx");

        //create copy of template so that we don't overwrite it
        if (System.IO.File.Exists(Documento_Salida))
        {
            System.IO.File.Delete(Documento_Salida);
        }
        File.Copy(Ruta_Plantilla, Documento_Salida);

        ReportDocument Reporte = new ReportDocument();
        String Nombre_Archivo = "Convenio_Predial.docx";
        String Tipo_Solicitante;
        String Calle_Numero;
        DateTime Fecha_Convenio;
        String PDF_Convenio = Nombre_Archivo + ".pdf";
        String Importe_Letra;
        string Periodicidad = "";

        // si no existe el directorio, crearlo
        if (!System.IO.Directory.Exists(Server.MapPath("../../Reporte")))
            System.IO.Directory.CreateDirectory("../../Reporte");

        Lbl_Mensaje_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Img_Error.Visible = false;

        // tipo de contribuyente
        if (Cmb_Tipo_Solicitante.SelectedValue == "PROPIETARIO")
            Tipo_Solicitante = "CONTRIBUYENTE";
        else
            Tipo_Solicitante = Cmb_Tipo_Solicitante.SelectedValue;
        // formar ubicacion (calle, numero y si existe, numero interior)
        Calle_Numero = Txt_Calle.Text + " " + Txt_No_Exterior.Text + " ";
        if (Txt_No_Interior.Text != "")
            Calle_Numero += "INT " + Txt_No_Interior.Text + " ";
        if (!DateTime.TryParse(Txt_Fecha_Convenio.Text, out Fecha_Convenio))
            Fecha_Convenio = DateTime.Now;

        // convertir el importe del convenio a letra
        Numalet Cantidad = new Numalet();
        Cantidad.MascaraSalidaDecimal = "00/100 M.N.";
        Cantidad.SeparadorDecimalSalida = "Pesos";
        Cantidad.ApocoparUnoParteEntera = true;
        Cantidad.LetraCapital = true;
        Importe_Letra = Cantidad.ToCustomCardinal(Txt_Sub_Total.Text.Replace(",", ""));
        // periodicidad
        switch (Cmb_Periodicidad_Pago.SelectedValue)
        {
            case "7":
                Periodicidad = "PAGOS SEMANALES";
                break;
            case "14":
                Periodicidad = "PAGOS CATORCENALES";
                break;
            case "15":
                Periodicidad = "PAGOS QUINCENALES";
                break;
            case "30":
                Periodicidad = "MENSUALIDADES";
                break;
            case "2MES":
                Periodicidad = "PAGOS CADA DOS MESES";
                break;
            case "3MES":
                Periodicidad = "PAGOS CADA TRES MESES";
                break;
            case "4MES":
                Periodicidad = "PAGOS CADA CUATRO MESES";
                break;
            case "5MES":
                Periodicidad = "PAGOS CADA CINCO MESES";
                break;
            case "6MES":
                Periodicidad = "PAGOS CADA SEIS MESES";
                break;
            case "7MES":
                Periodicidad = "PAGOS CADA SIETE MESES";
                break;
            case "8MES":
                Periodicidad = "PAGOS CADA OCHO MESES";
                break;
            case "9MES":
                Periodicidad = "PAGOS CADA NUEVE MESES";
                break;
            case "10MES":
                Periodicidad = "PAGOS CADA DIEZ MESES";
                break;
            case "11MES":
                Periodicidad = "PAGOS CADA ONCE MESES";
                break;
            case "365":
                Periodicidad = "ANUALIDADES";
                break;
        }

        try
        {
            using (WordprocessingDocument doc = WordprocessingDocument.Open(Documento_Salida, true))
            {
                Int32 Parcialidades = 0;
                //create XML string matching custom XML part
                string newXml = "<root>"
                    + "<Folio>" + "CPRE" + Convert.ToInt32(Hdf_No_Convenio.Value).ToString() + "</Folio>"
                    + "<Nombre_Solicitante>" + Txt_Solicitante.Text.Trim().ToUpper() + "</Nombre_Solicitante>"
                    + "<Tipo_Solicitante>" + Tipo_Solicitante + "</Tipo_Solicitante>"
                    + "<Tipo_Solicitante2>" + Tipo_Solicitante + "</Tipo_Solicitante2>"
                    + "<Tipo_Solicitante3>" + Tipo_Solicitante + "</Tipo_Solicitante3>"
                    + "<rfc>" + Txt_RFC.Text + "</rfc>"
                    + "<Nombre_Titular>" + Txt_Propietario.Text.Trim().ToUpper() + "</Nombre_Titular>"
                    + "<Calle_Numero>" + Calle_Numero + "</Calle_Numero>"
                    + "<Nombre_Colonia>" + Txt_Colonia.Text + "</Nombre_Colonia>"
                    + "<Cuenta_Predial>" + Txt_Cuenta_Predial.Text + "</Cuenta_Predial>"
                    + "<Cantidad_Letra>" + Importe_Letra + "</Cantidad_Letra>"
                    + "<Cantidad_Numero>" + Txt_Sub_Total.Text + "</Cantidad_Numero>"
                    + "<Numero_Parcialidades>" + Txt_Numero_Parcialidades.Text + "</Numero_Parcialidades>"
                    + "<Periodicidad>" + Periodicidad + "</Periodicidad>"
                    + "<Dia_Del_Mes>" + Fecha_Convenio.Day.ToString() + "</Dia_Del_Mes>"
                    + "<Mes>" + Fecha_Convenio.ToString("MMMM").ToUpper() + "</Mes>"
                    + "<Anio>" + Fecha_Convenio.Year.ToString() + "</Anio>"
                    + "</root>";

                MainDocumentPart main = doc.MainDocumentPart;
                main.DeleteParts<CustomXmlPart>(main.CustomXmlParts);

                //add and write new XML part
                CustomXmlPart customXml = main.AddCustomXmlPart(CustomXmlPartType.CustomXml);

                //*** openXML_Wp = DocumentFormat.OpenXml.Wordprocessing
                // localizar la etiqueta del control de contenido en la que esta la tabla
                openXML_Wp.SdtBlock ccWithTable = main.Document.Body.Descendants<openXML_Wp.SdtBlock>().Where(r => r.SdtProperties.GetFirstChild<openXML_Wp.Tag>().Val == "TABLA_PARCIALIDADES").Single();
                // Localizar la tabla
                openXML_Wp.Table Tabla_Parcialidades = ccWithTable.Descendants<openXML_Wp.Table>().Single();
                // localizar la ultima fila de la tabla
                openXML_Wp.TableRow Fila_Vacia = Tabla_Parcialidades.Elements<openXML_Wp.TableRow>().Last();

                // estilo para los campos (crear un estilo y asignarle el id del estilo en el documento original de word)
                openXML_Wp.ParagraphProperties Propiedades_Parrafo = new openXML_Wp.ParagraphProperties();
                Propiedades_Parrafo.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "EstiloTablaParcialidades" };
                openXML_Wp.ParagraphProperties Prop_Montos_Parcialidades = new openXML_Wp.ParagraphProperties();
                Prop_Montos_Parcialidades.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "MontoTablaParcialidades" };
                openXML_Wp.ParagraphProperties Propiedades_Parrafo0;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo1;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo2;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo3;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo4;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo5;
                openXML_Wp.ParagraphProperties Propiedades_Parrafo6;

                Label Etiqueta_Honorarios_Grid;
                Label Etiqueta_Recargos_Grid;
                Label Etiqueta_Moratorios_Grid;
                Label Etiqueta_Periodo_Grid;
                decimal Monto_Recargos;
                decimal Monto_Moratorios;

                foreach (GridViewRow fila in Grid_Parcialidades.Rows)
                {
                    //if (fila.Cells[8].Text == "POR PAGAR")
                    //{
                    DateTime Fecha_Parcialidad;
                    TextBox Txt_Grid_Monto_Importe;
                    Label Lbl_Txt_Monto_Importe_Grid;
                    String Monto_Importe = "0.00";

                    Lbl_Txt_Monto_Importe_Grid = (Label)fila.Cells[6].FindControl("Lbl_Txt_Grid_Monto_Importe");
                    if (Lbl_Txt_Monto_Importe_Grid != null)
                    {
                        Monto_Importe = Lbl_Txt_Monto_Importe_Grid.Text.Replace("$", "");
                    }
                    else
                    {
                        Txt_Grid_Monto_Importe = (TextBox)fila.Cells[6].FindControl("Txt_Grid_Monto_Importe");
                        if (Txt_Grid_Monto_Importe != null)
                        {
                            Monto_Importe = Txt_Grid_Monto_Importe.Text;
                        }
                    }

                    Etiqueta_Periodo_Grid = (Label)fila.Cells[1].FindControl("Lbl_Txt_Grid_Periodo");
                    Etiqueta_Honorarios_Grid = (Label)fila.Cells[2].FindControl("Lbl_Txt_Grid_Monto_Honorarios");
                    Etiqueta_Recargos_Grid = (Label)fila.Cells[3].FindControl("Lbl_Txt_Grid_Monto_Recargos_Ordinarios");
                    Etiqueta_Moratorios_Grid = (Label)fila.Cells[4].FindControl("Lbl_Txt_Grid_Monto_Recargos_Moratorios");
                    decimal.TryParse(Etiqueta_Recargos_Grid.Text.Replace("$", ""), out Monto_Recargos);
                    decimal.TryParse(Etiqueta_Moratorios_Grid.Text.Replace("$", ""), out Monto_Moratorios);

                    // convertir la fecha
                    DateTime.TryParse(fila.Cells[7].Text, out Fecha_Parcialidad);

                    // copiar una fila nueva en la tabla
                    openXML_Wp.TableRow Nueva_Fila = (openXML_Wp.TableRow)Fila_Vacia.CloneNode(true);

                    // generar estilos para cada elemento (una vez enlazado no se puede agregar a otro, por eso se duplican con OuterXML)
                    Propiedades_Parrafo0 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                    Propiedades_Parrafo1 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                    Propiedades_Parrafo2 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                    Propiedades_Parrafo3 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo4 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo5 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                    Propiedades_Parrafo6 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);

                    // agregar datos a la nueva fila (el texto debe ir dentro de un elemento Run, para poderlo agregar en un elemento párrafo)
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(0).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(0).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo0,
                        new openXML_Wp.Run(new openXML_Wp.Text((++Parcialidades).ToString()))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(1).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(1).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo1,
                        new openXML_Wp.Run(new openXML_Wp.Text(Fecha_Parcialidad.ToString("dd-MMM-yyyy").ToUpper()))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(2).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(2).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo2,
                        new openXML_Wp.Run(new openXML_Wp.Text(Etiqueta_Periodo_Grid.Text))));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(3).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(3).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo3,
                        new openXML_Wp.Run(new openXML_Wp.Text(fila.Cells[5].Text.Replace("$", "")))
                        ));
                    // total recargos (ordinarios + moratorios)
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(4).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(4).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo4,
                        new openXML_Wp.Run(new openXML_Wp.Text((Monto_Recargos + Monto_Moratorios).ToString("#,##0.00")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(5).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(5).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo5,
                        new openXML_Wp.Run(new openXML_Wp.Text(Etiqueta_Honorarios_Grid.Text.Replace("$", "")))
                        ));
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(6).RemoveAllChildren();
                    Nueva_Fila.Descendants<openXML_Wp.TableCell>().ElementAt(6).Append(new openXML_Wp.Paragraph(
                        Propiedades_Parrafo6,
                        new openXML_Wp.Run(new openXML_Wp.Text(Monto_Importe))
                        ));

                    // agregar la nueva fila a la tabla
                    Tabla_Parcialidades.AppendChild(Nueva_Fila);

                    //} // si estatus == POR PAGAR
                }

                // copiar una fila nueva en la tabla para los totales
                openXML_Wp.TableRow Fila_Totales = (openXML_Wp.TableRow)Fila_Vacia.CloneNode(true);

                openXML_Wp.Paragraph Parrafo_Nuevo = new openXML_Wp.Paragraph();

                // cambiar estilo para totales de la tabla
                Propiedades_Parrafo.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "EstiloEncabezadoParcialidades" };
                Prop_Montos_Parcialidades.ParagraphStyleId = new openXML_Wp.ParagraphStyleId { Val = "MontoEncabezadoParcialidades" };
                Propiedades_Parrafo0 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                Propiedades_Parrafo1 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                Propiedades_Parrafo2 = new openXML_Wp.ParagraphProperties(Propiedades_Parrafo.OuterXml);
                Propiedades_Parrafo3 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo4 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo5 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);
                Propiedades_Parrafo6 = new openXML_Wp.ParagraphProperties(Prop_Montos_Parcialidades.OuterXml);

                // recuperar decimal con recargos ordinarios y moratorios
                decimal.TryParse(Grid_Parcialidades.FooterRow.Cells[3].Text.Replace("$", ""), out Monto_Recargos);
                decimal.TryParse(Grid_Parcialidades.FooterRow.Cells[4].Text.Replace("$", ""), out Monto_Moratorios);

                // agregar datos a la fila de totales
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(0).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(0).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo0,
                    new openXML_Wp.Run(new openXML_Wp.Text(""))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(1).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(1).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo1,
                    new openXML_Wp.Run(new openXML_Wp.Text("TOTALES"))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(2).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(2).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo2,
                    new openXML_Wp.Run(new openXML_Wp.Text(""))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(3).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(3).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo3,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[5].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(4).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(4).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo4,
                    new openXML_Wp.Run(new openXML_Wp.Text((Monto_Recargos + Monto_Moratorios).ToString("#,##0.00")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(5).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(5).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo5,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[2].Text.Replace("$", "")))
                    ));
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(6).RemoveAllChildren();
                Fila_Totales.Descendants<openXML_Wp.TableCell>().ElementAt(6).Append(new openXML_Wp.Paragraph(
                    Propiedades_Parrafo6,
                    new openXML_Wp.Run(new openXML_Wp.Text(Grid_Parcialidades.FooterRow.Cells[6].Text.Replace("$", "")))
                    ));

                // agregar la nueva fila a la tabla
                Tabla_Parcialidades.AppendChild(Fila_Totales);

                // eliminar la fila vacia que se tomo como base
                Tabla_Parcialidades.RemoveChild(Fila_Vacia);
                using (StreamWriter ts = new StreamWriter(customXml.GetStream()))
                {
                    ts.Write(newXml);
                }
                // guardar los cambios en el documento
                main.Document.Save();

                //closing WordprocessingDocument automatically saves the document
            }

            //string Ruta = @HttpContext.Current.Server.MapPath("~/Reporte/" + Nombre_Archivo);
            //// ofrecer para descarga
            //Response.Clear();
            //Response.ClearContent();
            //Response.ClearHeaders();
            //Response.ContentType = "application/x-msword";
            //Response.AddHeader("Content-Disposition", "attachment; filename=" + Ruta);
            ////           'Visualiza el archivo
            //Response.WriteFile(Ruta);
            //Response.Flush();
            //Response.Close();

            String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";
            Pagina = Pagina + "Convenio_Predial.docx";
            AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(
                this,
                this.GetType(),
                "Formato_Convenio",
                "window.open('" + Pagina +
                "', '" + "msword" + "','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')",
                true
                );
        }
        catch (Exception Ex)
        {
            throw new Exception("Imprimir convenio: " + Ex.Message);
        }
    }
    #endregion Impresion Folios

    #region Metodo Consulta Descuentos
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Consulta_Descuentos
    ///DESCRIPCIÓN: Consulta los datos de descuentos de una Cuenta Predial
    ///PARAMETROS: 
    ///CREO: Jacqueline Ramirez Sierra
    ///FECHA_CREO: 22 Septiembre 2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Consulta_Descuentos(String Cuenta_Predial_ID)
    {
        Cls_Ope_Pre_Descuentos_Predial_Negocio Descuentos = new Cls_Ope_Pre_Descuentos_Predial_Negocio();
        DataTable Dt_Descuentos_Cuentas;
        Descuentos.P_Cuenta_Predial = Cuenta_Predial_ID;
        Dt_Descuentos_Cuentas = Descuentos.Consultar_Descuentos_Predial();

        Img_Error.Visible = false;
        Lbl_Mensaje_Error.Text = "";
        Lbl_Mensaje_Error.Visible = false;

        if (Dt_Descuentos_Cuentas != null)
        {
            // recorrer los descuentos encontrados
            foreach (DataRow descuento in Dt_Descuentos_Cuentas.Rows)
            {
                // validar que el estatus del convenio sea VIGENTE
                if (descuento[Ope_Pre_Descuentos_Predial.Campo_Estatus].ToString() == "VIGENTE")
                {
                    Int32 Hasta_Anio_Convenio = 0;
                    Int32 Hasta_Bimestre_Convenio = 0;
                    Int32 Hasta_Anio_Descuento = 0;
                    Int32 Hasta_Bimestre_Descuento = 0;

                    // obtener el periodo del convenio como entero
                    Int32.TryParse(Cmb_Hasta_Anio_Periodo.SelectedValue, out Hasta_Anio_Convenio);
                    Int32.TryParse(Cmb_Hasta_Bimestre_Periodo.SelectedValue, out Hasta_Bimestre_Convenio);
                    // obtener el periodo del descuento
                    Int32.TryParse(descuento[Ope_Pre_Descuentos_Predial.Campo_Anio_Final].ToString(), out Hasta_Anio_Descuento);
                    Int32.TryParse(descuento[Ope_Pre_Descuentos_Predial.Campo_Bimestre_Final].ToString(), out Hasta_Bimestre_Descuento);

                    // si el periodo del descuento coincide con el periodo del descuento, aplicar descuento
                    if (Hasta_Anio_Convenio == Hasta_Anio_Descuento && Hasta_Bimestre_Convenio == Hasta_Bimestre_Descuento)
                    {
                        Decimal Descuento_Recargos_Ordinarios = 0;
                        Decimal Descuento_Recargos_Moratorios = 0;
                        Decimal.TryParse(descuento["DESCUENTO_RECARGO"].ToString(), out Descuento_Recargos_Ordinarios);
                        Decimal.TryParse(descuento["DESCUENTO_RECARGO_MORATORIO"].ToString(), out Descuento_Recargos_Moratorios);

                        // si el descuento es mayor a 0, aplicarlo
                        if (Descuento_Recargos_Moratorios > 0 || Descuento_Recargos_Ordinarios > 0)
                        {
                            Decimal Adeudo_Recargos_Ordinarios = 0;
                            Decimal Adeudo_Recargos_Moratorios = 0;
                            Decimal.TryParse(Txt_Monto_Recargos.Text, out Adeudo_Recargos_Ordinarios);
                            Decimal.TryParse(Txt_Monto_Moratorios.Text, out Adeudo_Recargos_Moratorios);

                            // si el descuento es mayor que el adeudo, igualar para evitar numeros negativos
                            if (Descuento_Recargos_Ordinarios > Adeudo_Recargos_Ordinarios)
                            {
                                Lbl_Mensaje_Error.Text = "El descuento en recargos ordinarios ($"
                                + Descuento_Recargos_Ordinarios.ToString("#,##0.00")
                                + ") es mayor que el adeudo.<br />";
                                Lbl_Mensaje_Error.Visible = true;
                                Descuento_Recargos_Ordinarios = Adeudo_Recargos_Ordinarios;
                            }
                            if (Descuento_Recargos_Moratorios > Adeudo_Recargos_Moratorios)
                            {
                                Lbl_Mensaje_Error.Text += "El descuento en recargos moratorios ($"
                                + Descuento_Recargos_Moratorios.ToString("#,##0.00")
                                + ") es mayor que el adeudo.";
                                Lbl_Mensaje_Error.Visible = false;
                                Descuento_Recargos_Moratorios = Adeudo_Recargos_Moratorios;
                            }

                            // aplicar descuento
                            Txt_Descuento_Recargos_Ordinarios.Text = Descuento_Recargos_Ordinarios.ToString("#,##0.00");
                            Txt_Descuento_Recargos_Moratorios.Text = Descuento_Recargos_Moratorios.ToString("#,##0.00");
                            Hdn_No_Descuento.Value = descuento[Ope_Pre_Descuentos_Predial.Campo_No_Descuento_Predial].ToString();
                            return;
                        }
                    }
                    else
                    {
                        Hdn_No_Descuento.Value = "";
                        Txt_Descuento_Recargos_Moratorios.Text = "0.00";
                        Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
                        Lbl_Mensaje_Error.Visible = true;
                        Lbl_Mensaje_Error.Text = " Se encontró un descuento para la cuenta que cubre hasta el periodo "
                            + Hasta_Bimestre_Descuento + "/" + Hasta_Anio_Descuento + "<br />";
                        Img_Error.Visible = true;
                    }
                }
                else
                {
                    Hdn_No_Descuento.Value = "";
                    Txt_Descuento_Recargos_Moratorios.Text = "0.00";
                    Txt_Descuento_Recargos_Ordinarios.Text = "0.00";
                }
            }
        }

    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Consulta_Adeudos_Diccionario
    /// DESCRIPCIÓN: COnsulta los adeudos de la cuenta y regresa un diccionario con los adeudos por bimestre
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 26-oct-2010
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private Dictionary<String, Decimal> Consulta_Adeudos_Diccionario()
    {

        Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio Rs_Adeudo_Actual = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        Dictionary<String, Decimal> Dic_Adeudos = new Dictionary<String, Decimal>();
        DataTable Dt_Adeudos_Actuales;

        // obtener los adeudos actuales de la cuenta
        Dt_Adeudos_Actuales = Rs_Adeudo_Actual.Calcular_Recargos_Predial(Hdf_Cuenta_Predial_ID.Value);
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
                        if (!Dic_Adeudos.ContainsKey(Fila_Adeudo["PERIODO"].ToString().Trim()))
                        {
                            Dic_Adeudos.Add(Fila_Adeudo["PERIODO"].ToString().Trim(), Cuota_Bimestral);
                        }
                        else
                        {
                            Dic_Adeudos[Fila_Adeudo["PERIODO"].ToString().Trim()] += Cuota_Bimestral;
                        }
                    }
                }
            }
        }

        return Dic_Adeudos;
    }

    ///*******************************************************************************************************
    /// NOMBRE_FUNCIÓN: Cargar_Combo_Anio
    /// DESCRIPCIÓN: COnsulta los adeudos de la cuenta y carga los años con adeudo en el combo Cmb_Hasta_Anio
    /// PARÁMETROS:
    /// CREO: Roberto González Oseguera
    /// FECHA_CREO: 02-may-2012
    /// MODIFICÓ: 
    /// FECHA_MODIFICÓ: 
    /// CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    private void Cargar_Combo_Anio()
    {

        var Consulta_Adeudos = new Cls_Ope_Pre_Generar_Adeudo_Predial_Negocio();
        DataTable Dt_Adeudos_Actuales;
        string Valor_Seleccionado;

        Valor_Seleccionado = Cmb_Hasta_Anio_Periodo.SelectedItem.Text;

        // obtener los adeudos actuales de la cuenta
        Dt_Adeudos_Actuales = Consulta_Adeudos.Consultar_Adeudos_Cuenta_Predial_Con_Totales(Hdf_Cuenta_Predial_ID.Value, "POR PAGAR", 0, 0);
        // validar que se obtuvieron adeudos de la cuenta
        if (Dt_Adeudos_Actuales != null && Dt_Adeudos_Actuales.Rows.Count > 0)
        {
            // ordenar la tabla
            Dt_Adeudos_Actuales.DefaultView.Sort = Ope_Pre_Adeudos_Predial.Campo_Anio + " DESC";
            // limpiar valores del combo
            Cmb_Hasta_Anio_Periodo.Items.Clear();
            // agregar elementos al combo
            Cmb_Hasta_Anio_Periodo.DataTextField = Ope_Pre_Adeudos_Predial.Campo_Anio;
            Cmb_Hasta_Anio_Periodo.DataValueField = Ope_Pre_Adeudos_Predial.Campo_Anio;
            Cmb_Hasta_Anio_Periodo.DataSource = Dt_Adeudos_Actuales;
            Cmb_Hasta_Anio_Periodo.DataBind();
        }

        if (Valor_Seleccionado.Length > 0)
        {
            // validar que el valor seleccionado anterior exista en el combo y seleccionarlo
            if (Cmb_Hasta_Anio_Periodo.Items.FindByText(Valor_Seleccionado) == null)
            {
                Cmb_Hasta_Anio_Periodo.Items.Add(Valor_Seleccionado);
            }
            Cmb_Hasta_Anio_Periodo.SelectedIndex = Cmb_Hasta_Anio_Periodo.Items.IndexOf(Cmb_Hasta_Anio_Periodo.Items.FindByText(Valor_Seleccionado));
        }
    }

    #endregion Metodo Consulta Descuentos

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Años
    ///DESCRIPCIÓN          : Carga el combo indicado por el parámetro de los años
    ///PARAMETROS: 
    ///CREO                 : Antonio Salvador Benavides Guardado
    ///FECHA_CREO           : 03/Agosto/2011
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    private void Cargar_Combo_Años(DropDownList Combo, Int32 Año_Seleccionado)
    {
        Int32 Cont_Años;

        for (Cont_Años = 1980; Cont_Años <= DateTime.Now.Year; Cont_Años++)
        {
            Combo.Items.Add(Cont_Años.ToString());
        }
        Combo.SelectedValue = Año_Seleccionado.ToString();
    }

}
