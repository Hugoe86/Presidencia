using System;
using System.IO;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Linq;
using System.Windows.Forms;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using Presidencia.Sessiones;
using System.Drawing;
using System.Drawing.Drawing2D;
using Presidencia.Constantes;
using AjaxControlToolkit;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using Presidencia.Cls_Cat_Ven_Registro_Usuarios.Negocio;
using System.Collections.Generic;
using Presidencia.Orden_Territorial_Administracion_Urbana.Negocio;
using Presidencia.Orden_Territorial_Formato_Ficha_Inspeccion.Negocio;
using Presidencia.Catalogo_Calles.Negocio;

public partial class paginas_Ordenamiento_Territorial_Frm_Ope_Ort_Formato_Ficha_Inspeccion : System.Web.UI.Page
{
    #region Page load
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Page_Load
    /// DESCRIPCION : 
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 06/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Inicializar_Controles();
        }
        string Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        Btn_Buscar_Calle.Attributes.Add("onclick", Ventana_Modal);
        Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        Btn_Buscar_Colonia.Attributes.Add("onclick", Ventana_Modal);
        Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias_Calles.aspx', 'center:yes;resizable:no;status:no;dialogWidth:580px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        Btn_Buscar_Calles_Solicitante.Attributes.Add("onclick", Ventana_Modal);
        Ventana_Modal = "Abrir_Ventana_Modal('../Predial/Ventanas_Emergentes/Frm_Busqueda_Avanzada_Colonias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
        Btn_Buscar_Colonia_Solicitante.Attributes.Add("onclick", Ventana_Modal);
    }
    #endregion

    #region Metodos Generales
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Inicializa_Controles
    /// DESCRIPCION : Prepara los controles en la forma para que el usuario pueda
    ///               realizar diferentes operaciones
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 06/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Inicializar_Controles()
    {
        try
        {
            Limpiar_Controles();
            Habilitar_Controles("Inicial");
            Consultar_Llenado_Formatos(Consultar_Formato_ID());


            Cargar_Combo_Tipo_Residuos();
            Cargar_Combo_Inspectores();
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Limpiar_Controles
    /// DESCRIPCION : Limpia los controles que se encuentran en la forma
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 01/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Limpiar_Controles()
    {
        try
        {
            //  elementos ocultos id
            Hdf_Tramite_ID.Value = "";
            Hdf_Solicitud_ID.Value = "";
            Hdf_Subproceso_ID.Value = "";

            //  para las cajas de texto
            Txt_Aprovechamiento_Colindancia.Text = "";
            Txt_Aprovechamiento_Emisiones_Atmosfera.Text = "";
            Txt_Aprovechamiento_Horario_Final.Text = "";
            Txt_Aprovechamiento_Horario_Inicial.Text = "";
            Txt_Aprovechamiento_Letrina.Text = "";
            Txt_Aprovechamiento_Metodo_Sepearacion.Text = "";
            Txt_Aprovechamiento_Nivel_Ruido.Text = "";
            Txt_Aprovechamiento_Servicio_Recoleccion.Text = "";
            Txt_Aprovechamiento_Tipo_Contenedor.Text = "";
            Txt_Aprovechamiento_Tipo_Ruido.Text = "";
            Txt_Autorizacion_Altura.Text = "";
            Txt_Autorizacion_Condiciones_Arbol.Text = "";
            Txt_Autorizacion_Diametro.Text = "";
            Txt_Autorizacion_Diametro_Fronda.Text = "";
            Txt_Autorizacion_Especie.Text = "";
            Txt_Consecutivo.Text = "";
            Txt_Fecha_Entraga.Text = "";
            Txt_Inmueble_Nombre_Inmueble.Text = "";
            Txt_Inmueble_Numero.Text = "";
            Txt_Inmueble_Telefono.Text = "";
            Txt_Licencia_Equipo_Emisor.Text = "";
            Txt_Licencia_Gastos_Combustible.Text = "";
            Txt_Licencia_Hora_Funcionamiento.Text = "";
            Txt_Licencia_Tipo_Conbustible.Text = "";
            Txt_Licencia_Tipo_Emision.Text = "";
            Txt_Manifiesto_Afectacion.Text = "";
            Txt_Manifiesto_Colindancia.Text = "";
            Txt_Manifiesto_Superficie_Total.Text = "";
            Txt_Manifiesto_Tipo_Proyecto.Text = "";
            //Txt_Materiales_Accesibilidad.Text = "";
            Txt_Materiales_Flora.Text = "";
            Txt_Materiales_Inclinacion.Text = "";
            Txt_Materiales_Petreo.Text = "";
            Txt_Materiales_Superficie_Total.Text = "";
            Txt_Observaciones_Del_Inspector.Text = "";
            Txt_Observaciones_Para_Inspector.Text = "";
            Txt_Solicitante_Nombre.Text = "";
            Txt_Solicitante_Numero.Text = "";
            Txt_Solicitante_Telefono.Text = "";
            Txt_Tiempo_Respuesta.Text = "";
            Txt_Fecha_Inspeccion.Text = "";
            Txt_Materiales_Profundidad.Text = "";

            //  para los CheckBox
            //Chk_Aprovechamiento_Lista_Biologico.Checked = false;
            //Chk_Aprovechamiento_Lista_Manejo_Especila.Checked = false;
            //Chk_Aprovechamiento_Lista_Peligros.Checked = false;
            //Chk_Aprovechamiento_Lista_Solidos.Checked = false;
            Chk_Dias_Labor_Jueves.Checked = false;
            Chk_Dias_Labor_Lunes.Checked = false;
            Chk_Dias_Labor_Martes.Checked = false;
            Chk_Dias_Labor_Miercoles.Checked = false;
            Chk_Dias_Labor_Viernes.Checked = false;

            //  para los RadioButtonList
            RBtn_Aprovechamiento_Conexion_Drenaje.SelectedIndex = -1;
            RBtn_Aprovechamiento_Existe_Separacion.SelectedIndex = -1;
            RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedIndex = -1;
            RBtn_Aprovechamiento_Uso_Suelo.SelectedIndex = -1;
            RBtn_Material_Permiso_Ecologia.SelectedIndex = -1;
            RBtn_Material_Permiso_Suelo.SelectedIndex = -1;
            RBtn_Aprovechamiento_Almacen_Residuos.SelectedIndex = -1;
            RBtn_Material_Accesibilidad_Vehiculo.SelectedIndex = -1;


            //  para los combos
            Cargar_Combo_Colonias();
            Cargar_Combo_Calles_Inmueble(new DataTable());
            Cargar_Combo_Calles_Solicitante(new DataTable());

            Session.Remove("Dt_Tipo_Residuo");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE:         Habilitar_Controles
    /// DESCRIPCION :   Habilita y Deshabilita los controles de la forma para prepara la página
    ///                 para a siguiente operación
    /// PARAMETROS:     1.- Operacion: Indica la operación que se desea realizar 
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     01/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Habilitar_Controles(String Operacion)
    {
        Boolean Habilitado = false; ///Indica si el control de la forma va hacer habilitado para utilización del usuario
        try
        {
            Habilitado = false;
            switch (Operacion)
            {
                case "Inicial":
                    Habilitado = false;
                    Btn_Nuevo.ToolTip = "Nuevo";
                    Btn_Salir.ToolTip = "Salir";
                    Btn_Nuevo.Visible = false;
                    Btn_Nuevo.CausesValidation = false;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                    Div_Lista_Formatos.Style.Value = "display:block";
                    Div_Principal_Llenado_Formato.Style.Value = "display:none";
                    break;
                case "Nuevo":
                    Habilitado = true;
                    Btn_Nuevo.ToolTip = "Dar de Alta";
                    Btn_Salir.ToolTip = "Cancelar";
                    Btn_Nuevo.Visible = true;
                    Btn_Nuevo.CausesValidation = true;
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                    break;
            }
            //  mensajes de error
            Mostrar_Mensaje_Error(false);

            //  para las cajas de texto
            Txt_Aprovechamiento_Colindancia.Enabled = Habilitado;
            Txt_Aprovechamiento_Emisiones_Atmosfera.Enabled = Habilitado;
            Txt_Aprovechamiento_Horario_Final.Enabled = Habilitado;
            Txt_Aprovechamiento_Horario_Inicial.Enabled = Habilitado;
            Txt_Aprovechamiento_Letrina.Enabled = Habilitado;
            Txt_Aprovechamiento_Metodo_Sepearacion.Enabled = Habilitado;
            Txt_Aprovechamiento_Nivel_Ruido.Enabled = Habilitado;
            Txt_Aprovechamiento_Servicio_Recoleccion.Enabled = Habilitado;
            Txt_Aprovechamiento_Tipo_Contenedor.Enabled = Habilitado;
            Txt_Aprovechamiento_Tipo_Ruido.Enabled = Habilitado;
            Txt_Autorizacion_Altura.Enabled = Habilitado;
            Txt_Autorizacion_Condiciones_Arbol.Enabled = Habilitado;
            Txt_Autorizacion_Diametro.Enabled = Habilitado;
            Txt_Autorizacion_Diametro_Fronda.Enabled = Habilitado;
            Txt_Autorizacion_Especie.Enabled = Habilitado;
            Txt_Consecutivo.Enabled = false;
            Txt_Fecha_Entraga.Enabled = false;
            Txt_Inmueble_Nombre_Inmueble.Enabled = Habilitado;
            Txt_Inmueble_Numero.Enabled = Habilitado;
            Txt_Inmueble_Telefono.Enabled = Habilitado;
            Txt_Licencia_Equipo_Emisor.Enabled = Habilitado;
            Txt_Licencia_Gastos_Combustible.Enabled = Habilitado;
            Txt_Licencia_Hora_Funcionamiento.Enabled = Habilitado;
            Txt_Licencia_Tipo_Conbustible.Enabled = Habilitado;
            Txt_Licencia_Tipo_Emision.Enabled = Habilitado;
            Txt_Manifiesto_Afectacion.Enabled = Habilitado;
            Txt_Manifiesto_Colindancia.Enabled = Habilitado;
            Txt_Manifiesto_Superficie_Total.Enabled = Habilitado;
            Txt_Manifiesto_Tipo_Proyecto.Enabled = Habilitado;
            //Txt_Materiales_Accesibilidad.Enabled = Habilitado;
            Txt_Materiales_Flora.Enabled = Habilitado;
            Txt_Materiales_Inclinacion.Enabled = Habilitado;
            Txt_Materiales_Petreo.Enabled = Habilitado;
            Txt_Materiales_Superficie_Total.Enabled = Habilitado;
            Txt_Observaciones_Del_Inspector.Enabled = Habilitado;
            Txt_Observaciones_Para_Inspector.Enabled = Habilitado;
            Txt_Solicitante_Nombre.Enabled = Habilitado;
            Txt_Solicitante_Numero.Enabled = Habilitado;
            Txt_Solicitante_Telefono.Enabled = Habilitado;
            Txt_Tiempo_Respuesta.Enabled = Habilitado;
            Txt_Fecha_Inspeccion.Enabled = false;
            Txt_Materiales_Profundidad.Enabled = Habilitado;

            //  para los CheckBox
            //Chk_Aprovechamiento_Lista_Biologico.Enabled = Habilitado;
            //Chk_Aprovechamiento_Lista_Manejo_Especila.Enabled = Habilitado;
            //Chk_Aprovechamiento_Lista_Peligros.Enabled = Habilitado;
            //Chk_Aprovechamiento_Lista_Solidos.Enabled = Habilitado;
            Chk_Dias_Labor_Jueves.Enabled = Habilitado;
            Chk_Dias_Labor_Lunes.Enabled = Habilitado;
            Chk_Dias_Labor_Martes.Enabled = Habilitado;
            Chk_Dias_Labor_Miercoles.Enabled = Habilitado;
            Chk_Dias_Labor_Viernes.Enabled = Habilitado;

            //  para los RadioButtonList
            RBtn_Aprovechamiento_Conexion_Drenaje.Enabled = Habilitado;
            RBtn_Aprovechamiento_Existe_Separacion.Enabled = Habilitado;
            RBtn_Aprovechamiento_Revuelven_Liquidos.Enabled = Habilitado;
            RBtn_Aprovechamiento_Uso_Suelo.Enabled = Habilitado;
            RBtn_Material_Permiso_Ecologia.Enabled = Habilitado;
            RBtn_Material_Permiso_Suelo.Enabled = Habilitado;
            RBtn_Aprovechamiento_Almacen_Residuos.Enabled = Habilitado;
            RBtn_Material_Accesibilidad_Vehiculo.Enabled = Habilitado;

            //  para los combos
            Cmb_Inmueble_Calle.Enabled = Habilitado;
            Cmb_Inmueble_Colonias.Enabled = Habilitado;
            Cmb_Solicitante_Calle.Enabled = Habilitado;
            Cmb_Solicitante_Colonia.Enabled = Habilitado;
            Cmb_Inspector.Enabled = Habilitado;
            Cmb_Tipo_Residuo.Enabled = Habilitado;


            //  para los botones
            Btn_Fecha_Entrega.Enabled = Habilitado;
            Btn_Fecha_Inspeccion.Enabled = Habilitado;
            Btn_Agregar_Tipo_Residuo.Enabled = Habilitado;
        }

        catch (Exception ex)
        {
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Areas_Donacion
    ///DESCRIPCIÓN          : se cargara el combo con los distintos areas de donacion y via publica
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 04/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private String Consultar_Formato_ID()
    {
        Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Formato_ID = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
        DataTable Dt_Consulta = new DataTable();
        String Formato_ID = "";
        try
        {
            Negocio_Consulta_Formato_ID.P_Nombre_Plantilla = "ficha inspeccion";
            Dt_Consulta = Negocio_Consulta_Formato_ID.Consultar_Formato_ID();

            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Formato_ID = Dt_Consulta.Rows[0][Cat_Tra_Formato_Predefinido.Campo_Formato_ID].ToString();
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Formato_ID " + ex.Message.ToString());
        }
        return Formato_ID;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Consultar_Llenado_Formatos
    ///DESCRIPCIÓN          : se cargara el grid con los formatos a llenar
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 07/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private String Consultar_Llenado_Formatos(String Formato_ID)
    {
        Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio_Consulta_Formato_ID = new Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Negocio_Consulta_Formato_ID.P_Plantilla_ID = Formato_ID;
            Dt_Consulta = Negocio_Consulta_Formato_ID.Consultar_Llenado_Solicitud_Formato();

            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Grid_Formatos.Columns[1].Visible = true;
                Grid_Formatos.Columns[2].Visible = true;
                Grid_Formatos.Columns[3].Visible = true;
                Grid_Formatos.DataSource = Dt_Consulta;
                Grid_Formatos.DataBind();
                Grid_Formatos.Columns[1].Visible = false;
                Grid_Formatos.Columns[2].Visible = false;
                Grid_Formatos.Columns[3].Visible = false;
            }
            else
            {
                Grid_Formatos.Columns[1].Visible = true;
                Grid_Formatos.Columns[2].Visible = true;
                Grid_Formatos.Columns[3].Visible = true;
                Grid_Formatos.DataSource = new DataTable();
                Grid_Formatos.DataBind();
                Grid_Formatos.Columns[1].Visible = false;
                Grid_Formatos.Columns[2].Visible = false;
                Grid_Formatos.Columns[3].Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Consultar_Llenado_Formatos " + ex.Message.ToString());
        }
        return Formato_ID;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles_Inmueble
    ///DESCRIPCIÓN: cargara la informacion de las calles pertenecientes a una colonia
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Calles_Inmueble(DataTable Dt_Calles)
    {
        try
        {
            Cmb_Inmueble_Calle.DataSource = Dt_Calles;
            Cmb_Inmueble_Calle.DataValueField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Inmueble_Calle.DataTextField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Inmueble_Calle.DataBind();
            Cmb_Inmueble_Calle.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************************************
    ///NOMBRE_FUNCIÓN: Llenar_Combo_Con_DataTable
    ///DESCRIPCIÓN: Asigna los valores de la tabla en el combo recibidos como parámetros
    ///PARÁMETROS:
    /// 		1. Obj_Combo: control al que se van a asignar los datos en la tabla
    /// 		2. Dt_Temporal: tabla con los datos a mostrar en el control Obj_Combo
    /// 		3. Indice_Campo_Valor: entero con el número de columna de la tabla con el valor para el combo
    /// 		4. Indice_Campo_Texto: entero con el número de columna de la tabla con el texto para el combo
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17-may-2012
    ///MODIFICÓ: 
    ///FECHA_MODIFICÓ: 
    ///CAUSA_MODIFICACIÓN: 
    ///*******************************************************************************************************
    public void Llenar_Combo_Con_DataTable(DropDownList Obj_Combo, DataTable Dt_Temporal, int Indice_Campo_Valor, int Indice_Campo_Texto)
    {
        Obj_Combo.Items.Clear();
        Obj_Combo.SelectedValue = null;
        Obj_Combo.DataSource = Dt_Temporal;
        Obj_Combo.DataTextField = Dt_Temporal.Columns[Indice_Campo_Texto].ToString();
        Obj_Combo.DataValueField = Dt_Temporal.Columns[Indice_Campo_Valor].ToString();
        Obj_Combo.DataBind();
        Obj_Combo.Items.Insert(0, new ListItem(HttpUtility.HtmlDecode("<SELECCIONAR>"), "0"));
        Obj_Combo.SelectedIndex = 0;
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Calles_Solicitante
    ///DESCRIPCIÓN: cargara la informacion de las calles pertenecientes a una colonia
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Calles_Solicitante(DataTable Dt_Calles)
    {
        try
        {
            Cmb_Solicitante_Calle.DataSource = Dt_Calles;
            Cmb_Solicitante_Calle.DataValueField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Solicitante_Calle.DataTextField = Cat_Pre_Calles.Campo_Nombre;
            Cmb_Solicitante_Calle.DataBind();
            Cmb_Solicitante_Calle.Items.Insert(0, "< SELECCIONE >");
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Inspectores
    ///DESCRIPCIÓN          : se cargara el combo con los distintos inspectores
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 07/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Combo_Inspectores()
    {
        Cls_Cat_Ort_Administracion_Urbana_Negocio Negocio_Consulta_Funcionamiento = new Cls_Cat_Ort_Administracion_Urbana_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consulta_Funcionamiento.Consultar_Inspectores();

            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Inspector.DataSource = Dt_Consulta;
                Cmb_Inspector.DataValueField = Cat_Ort_Inspectores.Campo_Inspector_ID;
                Cmb_Inspector.DataTextField = Cat_Ort_Inspectores.Campo_Nombre;
                Cmb_Inspector.DataBind();
                Cmb_Inspector.Items.Insert(0, "< SELECCIONE >");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Inicializar_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cargar_Combo_Colonias
    ///DESCRIPCIÓN: cargara la informacion de las calles y colonias
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Cargar_Combo_Colonias()
    {
        Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        DataTable Dt_Colonias = new DataTable();
        try
        {
            //  para las colonias
            Dt_Colonias = Negocio_Consulta.Consultar_Colonia();

            Cmb_Solicitante_Colonia.DataSource = Dt_Colonias;
            Cmb_Solicitante_Colonia.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
            Cmb_Solicitante_Colonia.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
            Cmb_Solicitante_Colonia.DataBind();
            Cmb_Solicitante_Colonia.Items.Insert(0, "< SELECCIONE >");

            Cmb_Inmueble_Colonias.DataSource = Dt_Colonias;
            Cmb_Inmueble_Colonias.DataValueField = Cat_Ate_Colonias.Campo_Colonia_ID;
            Cmb_Inmueble_Colonias.DataTextField = Cat_Ate_Colonias.Campo_Nombre;
            Cmb_Inmueble_Colonias.DataBind();
            Cmb_Inmueble_Colonias.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Cargar_Combo_Zona
    ///DESCRIPCIÓN          : se cargara el combo con las zonas
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 04/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Cargar_Combo_Tipo_Residuos()
    {
        Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio_Consulta_Resudios = new Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Dt_Consulta = Negocio_Consulta_Resudios.Consultar_Tipos_Residuos();

            if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
            {
                Cmb_Tipo_Residuo.DataSource = Dt_Consulta;
                Cmb_Tipo_Residuo.DataValueField = Cat_Ort_Tipo_Residuos.Campo_Residuo_ID;
                Cmb_Tipo_Residuo.DataTextField = Cat_Ort_Tipo_Residuos.Campo_Nombre;
                Cmb_Tipo_Residuo.DataBind();
                Cmb_Tipo_Residuo.Items.Insert(0, "< SELECCIONE >");
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Inicializar_Controles " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Mostrar_Mensaje_Error
    ///DESCRIPCIÓN          : se habilitan los mensajes de error
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 06/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Mostrar_Mensaje_Error(Boolean Accion)
    {
        try
        {
            Img_Error.Visible = Accion;
            Lbl_Mensaje_Error.Visible = Accion;
        }
        catch (Exception ex)
        {
            throw new Exception("Inicializar_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Llenar_Grid_Residuos
    ///DESCRIPCIÓN          : se llena el grid con el tipo de residuo que se selecciona
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 06/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Llenar_Grid_Residuos(DataTable Dt_Resiudo)
    {
        try
        {
            Grid_Tipos_Residuos.Columns[0].Visible = true;
            Grid_Tipos_Residuos.Columns[1].Visible = true;
            Grid_Tipos_Residuos.DataSource = Dt_Resiudo;
            Grid_Tipos_Residuos.DataBind();
            Grid_Tipos_Residuos.Columns[1].Visible = false;
            Grid_Tipos_Residuos.Columns[0].Visible = false;
            Session["Dt_Tipo_Residuo"] = Dt_Resiudo;
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Alta_Fomato
    ///DESCRIPCIÓN          : se cargara la clase de negocios con la informacion
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 05/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Alta_Fomato()
    {
        Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio_Alta_Formato = new Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio();
        try
        {
            //  para los datos de los id generales
            Negocio_Alta_Formato.P_Tramite_ID = Hdf_Tramite_ID.Value;
            Negocio_Alta_Formato.P_Solicitud_ID = Hdf_Solicitud_ID.Value;
            Negocio_Alta_Formato.P_Subproceso_ID = Hdf_Subproceso_ID.Value;
            Negocio_Alta_Formato.P_Inspector_ID = Cmb_Inspector.SelectedValue;
            Negocio_Alta_Formato.P_Fecha_Entrega =  Convert.ToDateTime(Txt_Fecha_Entraga.Text);
            Negocio_Alta_Formato.P_Tiempo_Respuesta = Txt_Tiempo_Respuesta.Text;
            Negocio_Alta_Formato.P_Fecha_Inspeccion =  Convert.ToDateTime(Txt_Fecha_Inspeccion.Text);
            //  para los datos del inmueble
            Negocio_Alta_Formato.P_Inmueble_Nombre = Txt_Inmueble_Nombre_Inmueble.Text;
            Negocio_Alta_Formato.P_Inmueble_Telefono = Txt_Inmueble_Telefono.Text;
            Negocio_Alta_Formato.P_Inmueble_Colonia = Cmb_Inmueble_Colonias.SelectedItem.ToString();
            Negocio_Alta_Formato.P_Inmueble_Calle = Cmb_Inmueble_Calle.SelectedItem.ToString();            
            Negocio_Alta_Formato.P_Inmueble_Numero = Txt_Inmueble_Numero.Text;
            //  para los datos del solicitante
            Negocio_Alta_Formato.P_Solicitante_Nombre = Txt_Solicitante_Nombre.Text;
            Negocio_Alta_Formato.P_Solicitante_Telefono = Txt_Solicitante_Telefono.Text;
            Negocio_Alta_Formato.P_Solicitante_Colonia = Cmb_Solicitante_Colonia.SelectedItem.ToString();
            Negocio_Alta_Formato.P_Solicitante_Calle = Cmb_Solicitante_Calle.SelectedItem.ToString();
            Negocio_Alta_Formato.P_Solicitante_Numero = Txt_Solicitante_Numero.Text;
            //  para los datos del manifiesto de impacto ambiental
            Negocio_Alta_Formato.P_Impacto_Afectables = Txt_Manifiesto_Afectacion.Text;
            Negocio_Alta_Formato.P_Impacto_Colindancias = Txt_Manifiesto_Colindancia.Text;
            Negocio_Alta_Formato.P_Impacto_Superficie = Txt_Manifiesto_Superficie_Total.Text;
            Negocio_Alta_Formato.P_Impacto_Tipo_Proyecto = Txt_Manifiesto_Tipo_Proyecto.Text;
            //  para los datos de la licencia ambiental de funcionamiento
            Negocio_Alta_Formato.P_Licencia_Tipo_Equipo = Txt_Licencia_Equipo_Emisor.Text;
            Negocio_Alta_Formato.P_Licencia_Tipo_Emision = Txt_Licencia_Tipo_Emision.Text;
            Negocio_Alta_Formato.P_Licencia_Horario_Funcionamiento = Txt_Licencia_Hora_Funcionamiento.Text;
            Negocio_Alta_Formato.P_Licencia_Tipo_Combustible = Txt_Licencia_Tipo_Conbustible.Text;
            Negocio_Alta_Formato.P_Licencia_Tipo_Gastos_Combustible = Txt_Licencia_Gastos_Combustible.Text;
            //  para los datos de la autorizacion de poda
            Negocio_Alta_Formato.P_Poda_Altura = Txt_Autorizacion_Altura.Text;
            Negocio_Alta_Formato.P_Poda_Diametro_Tronco = Txt_Autorizacion_Diametro.Text;
            Negocio_Alta_Formato.P_Poda_Fronda = Txt_Autorizacion_Diametro_Fronda.Text;
            Negocio_Alta_Formato.P_Poda_Especie = Txt_Autorizacion_Especie.Text;
            Negocio_Alta_Formato.P_Poda_Condiciones = Txt_Autorizacion_Condiciones_Arbol.Text;
            //  para los datos del banco de materiales 
            Negocio_Alta_Formato.P_Material_Permiso_Ecologico = RBtn_Material_Permiso_Ecologia.SelectedValue;
            Negocio_Alta_Formato.P_Material_Permiso_Suelo = RBtn_Material_Permiso_Suelo.SelectedValue;
            Negocio_Alta_Formato.P_Material_Superficie_Total = Txt_Materiales_Superficie_Total.Text;
            Negocio_Alta_Formato.P_Material_Profundidad = Txt_Materiales_Profundidad.Text;
            Negocio_Alta_Formato.P_Material_Inclinacion = Txt_Materiales_Inclinacion.Text;
            Negocio_Alta_Formato.P_Material_Flora = Txt_Materiales_Flora.Text;
            Negocio_Alta_Formato.P_Material_Acceso_Vehiculos = RBtn_Material_Accesibilidad_Vehiculo.SelectedValue;
            Negocio_Alta_Formato.P_Material_Petreo = Txt_Materiales_Petreo.Text;
            //  para los datos de la autorizacion de aprovechamiento ambiental
            Negocio_Alta_Formato.P_Autoriza_Suelos = RBtn_Aprovechamiento_Uso_Suelo.SelectedValue;
            Negocio_Alta_Formato.P_Autoriza_Area_Residuos = RBtn_Aprovechamiento_Almacen_Residuos.SelectedValue;
            Negocio_Alta_Formato.P_Autoriza_Separacion = RBtn_Aprovechamiento_Existe_Separacion.SelectedValue;
            Negocio_Alta_Formato.P_Autoriza_Metodo_Separacion = Txt_Aprovechamiento_Metodo_Sepearacion.Text;
            Negocio_Alta_Formato.P_Autoriza_Servicio_Recoleccion = Txt_Aprovechamiento_Servicio_Recoleccion.Text;
            Negocio_Alta_Formato.P_Autoriza_Revuelven_Solidos_Liquidos = RBtn_Aprovechamiento_Revuelven_Liquidos.SelectedValue;
            Negocio_Alta_Formato.P_Autoriza_Tipo_Contenedor = Txt_Aprovechamiento_Tipo_Contenedor.Text;
            Negocio_Alta_Formato.P_Autoriza_Drenaje = RBtn_Aprovechamiento_Conexion_Drenaje.SelectedValue;
            Negocio_Alta_Formato.P_Autoriza_Tipo_Drenaje = Txt_Aprovechamiento_Letrina.Text;
            Negocio_Alta_Formato.P_Autoriza_Tipo_Ruido = Txt_Aprovechamiento_Tipo_Ruido.Text;
            if (Txt_Aprovechamiento_Nivel_Ruido.Text == "") Txt_Aprovechamiento_Nivel_Ruido.Text = "0";
            Negocio_Alta_Formato.P_Autoriza_Nivel_Ruido = Txt_Aprovechamiento_Nivel_Ruido.Text;
            Negocio_Alta_Formato.P_Autoriza_Horario_Labores = Txt_Aprovechamiento_Horario_Inicial.Text + " a " + Txt_Aprovechamiento_Horario_Final.Text;
            if (Chk_Dias_Labor_Lunes.Checked == true) Negocio_Alta_Formato.P_Autoriza_Lunes = "SI"; else Negocio_Alta_Formato.P_Autoriza_Lunes = "NO";
            if (Chk_Dias_Labor_Martes.Checked == true) Negocio_Alta_Formato.P_Autoriza_Martes = "SI"; else Negocio_Alta_Formato.P_Autoriza_Martes = "NO";
            if (Chk_Dias_Labor_Miercoles.Checked == true) Negocio_Alta_Formato.P_Autoriza_Miercoles = "SI"; else Negocio_Alta_Formato.P_Autoriza_Miercoles = "NO";
            if (Chk_Dias_Labor_Jueves.Checked == true) Negocio_Alta_Formato.P_Autoriza_Jueves = "SI"; else Negocio_Alta_Formato.P_Autoriza_Jueves = "NO";
            if (Chk_Dias_Labor_Viernes.Checked == true) Negocio_Alta_Formato.P_Autoriza_Viernes = "SI"; else Negocio_Alta_Formato.P_Autoriza_Viernes = "NO";
            Negocio_Alta_Formato.P_Autoriza_Colindancia = Txt_Aprovechamiento_Colindancia.Text;
            if (Txt_Aprovechamiento_Emisiones_Atmosfera.Text == "") Txt_Aprovechamiento_Emisiones_Atmosfera.Text = "0";
            Negocio_Alta_Formato.P_Autoriza_Emisiones = Txt_Aprovechamiento_Emisiones_Atmosfera.Text;
            //  para los datos de la autorizacion de aprovechamiento ambiental
            Negocio_Alta_Formato.P_Observaciones_Del_Inspector = Txt_Observaciones_Del_Inspector.Text;
            Negocio_Alta_Formato.P_Observaciones_Para_Inspector = Txt_Observaciones_Para_Inspector.Text;
            //  para los campos de auditoria
            Negocio_Alta_Formato.P_Usuario = Cls_Sessiones.Nombre_Empleado;
            //  para los elementos de residuos peligrosos
            Negocio_Alta_Formato.P_Dt_Residuos = (DataTable)Session["Dt_Tipo_Residuo"];

            //  se ejecuta el metodo de alta
            Negocio_Alta_Formato.Guardar_Formato();
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }
    #endregion

    #region Validaciones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Validar_Datos
    /// DESCRIPCION : Validar que se se encuentre todos los datos para continuar con el proceso
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 09/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private Boolean Validar_Datos()
    {
        String Espacios_Blanco = "";
        Boolean Datos_Validos = true;//Variable que almacena el valor de true si todos los datos fueron ingresados de forma correcta, o false en caso contrario.
        Lbl_Mensaje_Error.Text = "";
        Espacios_Blanco = "&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;";
        Lbl_Mensaje_Error.Text += Espacios_Blanco + "Es necesario Introducir: <br>";
        Mostrar_Mensaje_Error(true);

        if (Cmb_Inspector.SelectedIndex == 0)
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Seleccione al insepector.<br>";
            Datos_Validos = false;
        }
        if (Txt_Inmueble_Numero.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero del inmueble.<br>";
            Datos_Validos = false;
        }
        if (Txt_Solicitante_Numero.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el numero del solicitante.<br>";
            Datos_Validos = false;
        }
        if (Txt_Tiempo_Respuesta.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el tiempo de respuesta.<br>";
            Datos_Validos = false;
        }

        //  para los datos del manifiesto de impacto ambiental
        if (Txt_Manifiesto_Superficie_Total.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el total de la superficie en el manifiesto de impacto ambiental.<br>";
            Datos_Validos = false;
        }
        //  para los datos de la licencia ambiental de funcionamiento
        if (Txt_Licencia_Gastos_Combustible.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el total del gasto de combustible en la licencia ambiental de funcionamiento.<br>";
            Datos_Validos = false;
        }
        //  para los datos de la autorizacion de poda
        if (Txt_Autorizacion_Altura.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la altura del arbol en la la autorizacion de poda.<br>";
            Datos_Validos = false;
        }
        if (Txt_Autorizacion_Diametro.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el diamentro del tronco en la la autorizacion de poda.<br>";
            Datos_Validos = false;
        }
        if (Txt_Autorizacion_Diametro_Fronda.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese el diametro de la fronda en la la autorizacion de poda.<br>";
            Datos_Validos = false;
        }
        //  para los datos del banco de materiales
        if (Txt_Materiales_Superficie_Total.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la superficie total en el banco de materiales.<br>";
            Datos_Validos = false;
        }
        if (Txt_Materiales_Profundidad.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la profundidad en el banco de materiales.<br>";
            Datos_Validos = false;
        }
        if (Txt_Materiales_Inclinacion.Text == "")
        {
            Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Ingrese la inclinacion en el banco de materiales.<br>";
            Datos_Validos = false;
        }
        DateTime Fecha = (DateTime.Now);

        if (Txt_Fecha_Entraga.Text != "")
        {
            if (Convert.ToDateTime(Txt_Fecha_Entraga.Text) > Fecha)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha Superior a la actual.<br>";
                Datos_Validos = false;
            }
        }

        if (Txt_Fecha_Inspeccion.Text != "")
        {
            if (Convert.ToDateTime(Txt_Fecha_Inspeccion.Text) > Fecha)
            {
                Lbl_Mensaje_Error.Text += Espacios_Blanco + "*Fecha Superior a la actual.<br>";
                Datos_Validos = false;
            }
        }

        
        return Datos_Validos;
    }
    #endregion

    #region Botones
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Nuevo_Click
    /// DESCRIPCION : realiza la alta del usuario
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 06/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false);

            if (Btn_Nuevo.ToolTip == "Nuevo")
            {         
                Habilitar_Controles("Nuevo"); //Habilita los controles para la introducción de datos por parte del usuario
            }
            else
            {
                if (Validar_Datos())
                {
                    Mostrar_Mensaje_Error(false);
                    Alta_Fomato();
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramites", "alert('Alta de formato exitosa');", true);
                    Inicializar_Controles();
                    if (Grid_Formatos.Rows.Count > 0)
                    {
                        Grid_Formatos.SelectedIndex = -1;
                    }
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                }
            }

        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Salir_Click
    /// DESCRIPCION : 
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 06/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            if (Div_Principal_Llenado_Formato.Style.Value == "display:block")
            {
                Inicializar_Controles(); //Habilita los controles para la siguiente operación del usuario en el catálogo
            }
            else
            {
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
        }
        catch (Exception ex)
        {
            Lbl_Mensaje_Error.Visible = true;
            Img_Error.Visible = true;
            Lbl_Mensaje_Error.Text = ex.Message.ToString();
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Agregar_Tipo_Residuo_Click
    /// DESCRIPCION :   agregara un tipo de residuo al grid
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 06/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Agregar_Tipo_Residuo_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Residuo;
        DataRow Fila;
        Boolean Estado = false;
        try
        {
            if (Cmb_Tipo_Residuo.SelectedIndex > 0)
            {
                Mostrar_Mensaje_Error(false);
                if (Session["Dt_Tipo_Residuo"] == null)
                {
                    Dt_Residuo = new DataTable("Dt_Tipo_Residuo");
                    Dt_Residuo.Columns.Add("TIPO_RESIDUO_ID", Type.GetType("System.String"));
                    Dt_Residuo.Columns.Add("NOMBRE", Type.GetType("System.String"));
                }
                else
                {
                    Dt_Residuo = (DataTable)Session["Dt_Tipo_Residuo"];
                }

                //  buscar que no se encuentre dentro del grid

                if (Dt_Residuo != null && Dt_Residuo.Rows.Count > 0)
                {
                    foreach (DataRow Registro in Dt_Residuo.Rows)
                    {
                        if ((Cmb_Tipo_Residuo.SelectedValue == Registro["TIPO_RESIDUO_ID"].ToString()))
                        {
                            Estado = true;
                        }
                    }
                }

                if (Estado == false)
                {
                    Fila = Dt_Residuo.NewRow();

                    Fila["TIPO_RESIDUO_ID"] = Cmb_Tipo_Residuo.SelectedValue;
                    Fila["NOMBRE"] = Cmb_Tipo_Residuo.SelectedItem.ToString();
                    Dt_Residuo.Rows.Add(Fila);

                    Llenar_Grid_Residuos(Dt_Residuo);
                    Cmb_Tipo_Residuo.SelectedIndex = 0;
                }
                else
                {
                    Mostrar_Mensaje_Error(true);
                    Lbl_Mensaje_Error.Text = "El tipo de residuo [" + Cmb_Tipo_Residuo.SelectedItem.ToString() + "] ya se encuentra seleccionado";
                }



            }
            else
            {
                Mostrar_Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Seleccione el tipo de residuo";
            }

        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calles_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la calle seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Calles_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

                try
                {
                    string Calle_ID = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // consultar las calles de la colonia a la que pertenece la calle seleccionada
                    Cmb_Inmueble_Calle.Items.Clear();
                    Obj_Calles.P_Colonia_ID = Colonia_ID;
                    Llenar_Combo_Con_DataTable(Cmb_Inmueble_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Inmueble_Colonias.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Inmueble_Colonias.SelectedValue = Colonia_ID;
                    }
                    // si el combo calles contiene un elemento con el ID, seleccionar
                    if (Cmb_Inmueble_Calle.Items.FindByValue(Calle_ID) != null)
                    {
                        Cmb_Inmueble_Calle.SelectedValue = Calle_ID;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("CLAVE_COLONIA");
                Session.Remove("CALLE_ID");
                Session.Remove("CLAVE_CALLE");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS_CALLES");
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Colonia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
            {
                try
                {
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Inmueble_Colonias.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Inmueble_Colonias.SelectedValue = Colonia_ID;
                        Cmb_Inmueble_Colonias_OnSelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                    //Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("NOMBRE_COLONIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS");
        }
    }


    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Calles_Solicitante_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la calle seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Calles_Solicitante_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS_CALLES"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS_CALLES"]) == true)
            {
                var Obj_Calles = new Cls_Cat_Pre_Calles_Negocio();

                try
                {
                    string Calle_ID = Session["CALLE_ID"].ToString().Replace("&nbsp;", "");
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // consultar las calles de la colonia a la que pertenece la calle seleccionada
                    Cmb_Solicitante_Calle.Items.Clear();
                    Obj_Calles.P_Colonia_ID = Colonia_ID;
                    Llenar_Combo_Con_DataTable(Cmb_Solicitante_Calle, Obj_Calles.Consultar_Calles(), 0, 5);
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Solicitante_Colonia.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Solicitante_Colonia.SelectedValue = Colonia_ID;
                    }
                    // si el combo calles contiene un elemento con el ID, seleccionar
                    if (Cmb_Solicitante_Calle.Items.FindByValue(Calle_ID) != null)
                    {
                        Cmb_Solicitante_Calle.SelectedValue = Calle_ID;
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("CLAVE_COLONIA");
                Session.Remove("CALLE_ID");
                Session.Remove("CLAVE_CALLE");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS_CALLES");
        }

    }
    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Colonia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la colonia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Colonia_Solicitante_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_COLONIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_COLONIAS"]) == true)
            {
                try
                {
                    string Colonia_ID = Session["COLONIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Solicitante_Colonia.Items.FindByValue(Colonia_ID) != null)
                    {
                        Cmb_Solicitante_Colonia.SelectedValue = Colonia_ID;
                        Cmb_Solicitante_Colonia_OnSelectedIndexChanged(null, null);
                    }
                }
                catch (Exception Ex)
                {
                    throw new Exception(Ex.Message.ToString());
                    //Mostrar_Informacion(Ex.Message, true);
                }

                // limpiar variables de sesión
                Session.Remove("COLONIA_ID");
                Session.Remove("NOMBRE_COLONIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_COLONIAS");
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Btn_Quitar_Tipo_Residuo_Click
    /// DESCRIPCION :   quitara el tipo de residuo del grid
    /// PARAMETROS: 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 06/Junio/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Btn_Quitar_Tipo_Residuo_Click(object sender, ImageClickEventArgs e)
    {
        TableCell Tabla;
        GridViewRow Row;
        int Fila;
        DataTable Dt_Elimiar_Registro = new DataTable();
        try
        {
            ImageButton Btn_Eliminar = (ImageButton)sender;
            Tabla = (TableCell)Btn_Eliminar.Parent;
            Row = (GridViewRow)Tabla.Parent;
            Grid_Tipos_Residuos.SelectedIndex = Row.RowIndex;
            Fila = Row.RowIndex;

            Dt_Elimiar_Registro = (DataTable)Session["Dt_Tipo_Residuo"];
            Dt_Elimiar_Registro.Rows.RemoveAt(Fila);
            Session["Dt_Tipo_Residuo"] = Dt_Elimiar_Registro;
            Grid_Tipos_Residuos.SelectedIndex = (-1);
            Llenar_Grid_Residuos(Dt_Elimiar_Registro);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Autorizar_Formato_Click
    ///DESCRIPCIÓN: Autorizar el llenado del formato
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  09/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Autorizar_Formato_Click(object sender, EventArgs e)
    {
        DataTable Dt_Informacion_Grid = new DataTable();
        try
        {
            ImageButton Imagen_Boton = (ImageButton)sender;
            TableCell Celda = (TableCell)Imagen_Boton.Parent;
            GridViewRow Renglon = (GridViewRow)Celda.Parent;
            Grid_Formatos.SelectedIndex = Renglon.RowIndex;
            int Fila = Renglon.RowIndex;


            Limpiar_Controles();
            GridViewRow selectedRow = Grid_Formatos.Rows[Fila];
            Hdf_Solicitud_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            Hdf_Tramite_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            Hdf_Subproceso_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();

            Div_Lista_Formatos.Style.Value = "display:none";
            Div_Principal_Llenado_Formato.Style.Value = "display:block";
            Btn_Nuevo.Visible = true;
            Habilitar_Controles("Nuevo"); 
        }
        catch (Exception Ex)
        {
            throw new Exception(Ex.Message);
        }
    }
    #endregion


    #region Combo
    ///*******************************************************************************
    ///NOMBRE:      Cmb_Inmueble_Colonias_OnSelectedIndexChanged
    ///DESCRIPCIÓN: se cargara la colonia 
    ///PARAMETROS:
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Junio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Inmueble_Colonias_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        DataTable Dt_Calles = new DataTable();
        DataTable Dt_Colonias = new DataTable();
        try
        {
            Negocio_Consulta.P_Colonia_ID = Cmb_Inmueble_Colonias.SelectedValue;
            Dt_Calles = Negocio_Consulta.Consultar_Calles();

            Cargar_Combo_Calles_Inmueble(Dt_Calles);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE:      Cmb_Solicitante_Colonia_OnSelectedIndexChanged
    ///DESCRIPCIÓN: se cargara la calles pertenecientes a la colonia
    ///PARAMETROS:
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  06/Junio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Cmb_Solicitante_Colonia_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Ven_Registro_Usuarios_Negocio Negocio_Consulta = new Cls_Cat_Ven_Registro_Usuarios_Negocio();
        DataTable Dt_Calles = new DataTable();
        DataTable Dt_Colonias = new DataTable();
        try
        {
            Negocio_Consulta.P_Colonia_ID = Cmb_Solicitante_Colonia.SelectedValue;
            Dt_Calles = Negocio_Consulta.Consultar_Calles();

            Cargar_Combo_Calles_Solicitante(Dt_Calles);
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Grid
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN : Grid_Formatos_OnSelectedIndexChanged
    ///DESCRIPCIÓN          : permitira el llenado del formato
    ///PARAMETROS           : 
    ///CREO                 : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO           : 07/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    protected void Grid_Formatos_OnSelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Limpiar_Controles();
            GridViewRow selectedRow = Grid_Formatos.Rows[Grid_Formatos.SelectedIndex];
            Hdf_Solicitud_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[1].Text).ToString();
            Hdf_Tramite_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[2].Text).ToString();
            Hdf_Subproceso_ID.Value = HttpUtility.HtmlDecode(selectedRow.Cells[3].Text).ToString();

            Div_Lista_Formatos.Style.Value = "display:none";
            Div_Principal_Llenado_Formato.Style.Value = "display:block";
            Btn_Nuevo.Visible = true;
        }
        catch (Exception ex)
        {
            throw new Exception("Grid_Formatos_OnSelectedIndexChanged " + ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Formatos_Formatos_RowDataBound
    ///DESCRIPCIÓN: Evento de RowDataBound del Grid de formatos.
    ///PARAMETROS:     
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 09/Junio/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Grid_Formatos_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio Negocio_Formato_Ficha_Inspeccion = new Cls_Cat_Ort_Formato_Ficha_Inspeccion_Negocio();
        DataTable Dt_Consulta_Formato = new DataTable();
        try
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                System.Web.UI.WebControls.ImageButton Btn_Solicitar = (System.Web.UI.WebControls.ImageButton)e.Row.Cells[0].FindControl("Btn_Autorizar_Formato");

                Negocio_Formato_Ficha_Inspeccion.P_Tabla = Ope_Ort_Formato_Ficha_Inspec.Tabla_Ope_Ort_Formato_Ficha_Inspec;
                Negocio_Formato_Ficha_Inspeccion.P_Solicitud_ID = e.Row.Cells[1].Text;
                Negocio_Formato_Ficha_Inspeccion.P_Subproceso_ID = e.Row.Cells[3].Text;
                Dt_Consulta_Formato = Negocio_Formato_Ficha_Inspeccion.Consultar_Formato_Existente();

                if (Dt_Consulta_Formato != null && Dt_Consulta_Formato.Rows.Count > 0)
                {
                    Btn_Solicitar.Visible = false;
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception("Grid_Formatos_OnSelectedIndexChanged " + ex.Message.ToString());
        }
    }

    #endregion
}
