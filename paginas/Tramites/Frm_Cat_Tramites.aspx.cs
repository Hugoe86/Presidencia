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
using AjaxControlToolkit;
using System.Globalization;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Presidencia.Constantes;
using Presidencia.Sessiones;
using Presidencia.Catalogo_Tramites.Negocio;
using Presidencia.Ventanilla_Lista_Tramites.Negocio;
using Presidencia.Dependencias.Negocios;
using Presidencia.Tramites_Actividades.Negocio;
using Presidencia.Areas.Negocios;
using Presidencia.Solicitud_Tramites.Negocios;

public partial class paginas_tramites_Frm_Cat_Tramites : System.Web.UI.Page
{

    #region Page_Load

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Load
    ///DESCRIPCIÓN: Metodo que se carga cada que ocurre un PostBack de la Página
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************* 
    protected void Page_Load(object sender, EventArgs e)
    {
        try
        {
            if (!IsPostBack)
            {
                Div_Contenedor_Msj_Error.Visible = false;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Page_Init
    ///DESCRIPCIÓN: Metodo que se carga la configuracion y combos que se necesitan desde
    ///             el inicio del catalogo.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************          
    protected void Page_Init()
    {
        try
        {
            if (!IsPostBack)
            {
                Configuracion_Formulario(true);
                Llenar_Combos();
                Llenar_Combo_Cuentas_Contables();
                Llenar_Combo_Actividades();
                //Llenar_Grid_Tramites(0);
                Configuracion_Subcatalogos_Estado_Original();
                Accion_Buscar_Clave();

                string Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Tramites.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada.Attributes.Add("onclick", Ventana_Modal);
                Ventana_Modal = "Abrir_Ventana_Modal('../Atencion_Ciudadana/Ventanas_Emergente/Frm_Busqueda_Avanzada_Dependencias.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Buscar_Dependencia.Attributes.Add("onclick", Ventana_Modal);

                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Cuenta_Contable.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada_Cuenta_Contable.Attributes.Add("onclick", Ventana_Modal);

                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Plantillas.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada_Plantillas.Attributes.Add("onclick", Ventana_Modal);

                Ventana_Modal = "Abrir_Ventana_Modal('Ventanas_Emergente/Frm_Busqueda_Avanzada_Formatos.aspx', 'center:yes;resizable:no;status:no;dialogWidth:680px;dialogHeight:400px;dialogHide:true;help:no;scroll:no');";
                Btn_Busqueda_Avanzada_Formatos.Attributes.Add("onclick", Ventana_Modal);

                Session["BUSQUEDA_TRAMITES"] = false;
                Session["BUSQUEDA_CIUDADANO"] = false;
                Session["BUSQUEDA_CUENTA"] = false;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Metodos

    #region Generales

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION: Solicitar_Tramite
    /// DESCRIPCION : Carga la pantalla de solicitud de tramite con la informacion del 
    ///               tramite seleccionado por el usuario
    /// PARAMETROS  : 
    /// CREO        : Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO  : 09/Mayo/2012
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    protected void Accion_Buscar_Clave()
    {
        String Accion = String.Empty;
        String Clave_Tramite = String.Empty;
        String Cadena = "";
        try
        {
            if (Request.QueryString["Accion"] != null)
            {
                Accion = HttpUtility.UrlDecode(Request.QueryString["Accion"].ToString());
                if (Request.QueryString["id"] != null)
                {
                    Clave_Tramite = HttpUtility.UrlDecode(Request.QueryString["id"].ToString());
                }
                if (Clave_Tramite.Length == 5)
                {
                    switch (Accion)
                    {
                        case "Consultar_Clave":
                            Cadena = Consultar_Clave_Tramite(Clave_Tramite.ToUpper().Trim());
                            break;
                    }
                    Response.Write(Cadena);
                    Response.Flush();
                    Response.Close();
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }

    /// *******************************************************************************
    /// NOMBRE:         Consultar_Clave_Tramite
    /// COMENTARIOS:    consultara la clave esto para saber si se repite la clave del tramite
    /// PARÁMETROS:     String Clave la clave del tramite a buscar 
    /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
    /// FECHA CREÓ:     09/Mayo/2012 
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA DE LA MODIFICACIÓN:
    /// ******************************************************************************
    private String Consultar_Clave_Tramite(String Clave)
    {
        Cls_Cat_Tramites_Negocio Rs_Conusltar_Clave_Tramite = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        String Estatus = "";
        try
        {
            Mostrar_Mensaje_Error(false, "");

            //  se consulta la clave para ver si no esta repetida
            Rs_Conusltar_Clave_Tramite.P_Clave_Tramite = Clave;
            Dt_Consulta = Rs_Conusltar_Clave_Tramite.Consultar_Clave_Repetida();

            if (Dt_Consulta.Rows.Count > 0)
            {
                Estatus = "Repetido";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        return Estatus;

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Formulario
    ///DESCRIPCIÓN: Carga una configuracion de los controles del Formulario
    ///PARAMETROS:     
    ///             1. estatus.    Estatus en el que se cargara la configuración de los
    ///                             controles.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Formulario(Boolean estatus)
    {
        try
        {
            Mostrar_Mensaje_Error(false, "");

            Btn_Nuevo.Visible = true;
            Btn_Nuevo.AlternateText = "Nuevo";
            Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
            Btn_Modificar.Visible = true;
            Btn_Modificar.AlternateText = "Modificar";
            Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
            Btn_Eliminar.Visible = estatus;
            Grid_Tramites_Generales.Enabled = estatus;
            Grid_Tramites_Generales.SelectedIndex = (-1);
            Txt_Clave_Tramite.Enabled = !estatus;
            Cmb_Dependencias.Enabled = !estatus;
            Cmb_Areas.Enabled = !estatus;
            Txt_Nombre.Enabled = !estatus;
            //Cmb_Tipo_Tramite.Enabled = !estatus;
            Txt_Tiempo_Estimado.Enabled = !estatus;
            Txt_Costo.Enabled = !estatus;
            //Txt_Cuenta.Enabled = !estatus;
            //Cmb_Cuenta.Enabled = !estatus;
            //Chck_Solicitar_Internet.Enabled = !estatus;
            Txt_Descripcion.Enabled = !estatus;
            Cmb_Autorizadores.Enabled = !estatus;
            Btn_Agregar_Perfil.Enabled = !estatus;
            Btn_Modificar_Perfil.Enabled = !estatus;
            Btn_Quitar_Perfil.Enabled = !estatus;
            Txt_Nombre_Dato.Enabled = !estatus;
            Chk_Dato_Requerido.Enabled = !estatus;
            Chk_Dato_Inicial.Enabled = !estatus;
            Chk_Dato_Final.Enabled = !estatus;
            Chk_Documento_Requerido.Enabled = !estatus;
            Txt_Descripcion_Dato.Enabled = !estatus;
            Btn_Agregar_Dato.Enabled = !estatus;
            Btn_Subir_Dato.Enabled = !estatus;
            Btn_Bajar_Dato.Enabled = !estatus;
            Btn_Modificar_Dato.Enabled = !estatus;
            Btn_Quitar_Dato.Enabled = !estatus;
            Cmb_Documentos_Tramites.Enabled = !estatus;
            Btn_Agregar_Documento.Enabled = !estatus;
            Btn_Modificar_Documento.Enabled = !estatus;
            Btn_Quitar_Documento.Enabled = !estatus;
            //Txt_Nombre_Subproceso.Enabled = !estatus;
            Cmb_Nombre_Actividad.Enabled = !estatus;
            Txt_Valor_Subproceso.Enabled = !estatus;
            Cmb_Platillas.Enabled = !estatus;
            Txt_Descripcion_Subproceso.Enabled = !estatus;
            Btn_Agregar_Subproceso.Enabled = !estatus;
            Btn_Modificar_Subproceso.Enabled = !estatus;
            Btn_Quitar_Subproceso.Enabled = !estatus;
            Btn_Subir_Orden_Subproceso.Enabled = !estatus;
            Btn_Bajar_Orden_Subproceso.Enabled = !estatus;
            Cmb_Tipo_Actividad.Enabled = !estatus;
            Cmb_Formato.Enabled = !estatus;
            FileUp.Enabled = !estatus;
            //Txt_Persona_Avala_Documento.Enabled = !estatus;

            Cmb_Platillas.Enabled = !estatus;
            Cmb_Formato.Enabled = !estatus;
            Grid_Detalle_Formato.Enabled = !estatus;
            Grid_Detalle_Plantilla.Enabled = !estatus;
            Grid_Documentos_Tramite.Enabled = !estatus;
            Btn_Busqueda_Avanzada.Enabled = estatus;
            Btn_Buscar_Dependencia.Enabled = !estatus;
            Btn_Ver_Requisitos.Enabled = false;
            Cmb_Estatus_Tramite.Enabled = !estatus;

            Txt_Parametro1.Enabled = !estatus;
            Txt_Parametro2.Enabled = !estatus;
            Txt_Parametro3.Enabled = !estatus;
            Txt_Operador1.Enabled = !estatus;
            Txt_Operador2.Enabled = !estatus;
            Txt_Operador3.Enabled = !estatus;


            Txt_Matriz_Tipo.Enabled = !estatus;
            Txt_Matriz_Costo.Enabled = !estatus;
            Btn_Busqueda_Avanzada_Cuenta_Contable.Enabled = !estatus;

            //  se obtiene el nombre de los archivos existentes en la carpeta
            String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/Formato_Tramite/"));
            String Nombre_Archivo = "";

            if (Txt_Clave_Tramite.Text != "")
            {
                //  se busca el archivo
                for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
                {
                    Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());
                    if (Nombre_Archivo.Contains(Txt_Clave_Tramite.Text))
                    {
                        Btn_Ver_Formato.Visible = true;
                        break;
                    }
                }// fin del for
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Habilitar_Mensaje_Error
    ///DESCRIPCIÓN:          habilitara los mensajes de error
    ///PARAMETROS:           1.  Habilitar.  el tipo de visibilidad del objeto
    ///CREO:                 Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:           22/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    private void Mensaje_Error(Boolean Habilitar)
    {
        try
        {
            IBtn_Imagen_Error.Visible = Habilitar;
            Lbl_Mensaje_Error.Visible = Habilitar;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Limpiar_Catalogo
    ///DESCRIPCIÓN: Limpia los controles del Formulario
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Limpiar_Catalogo()
    {
        try
        {
            Hdf_Tramite_ID.Value = "";
            Hdf_Detalle_Autorizar_ID.Value = "";
            Hdf_Dato_Tramite_ID.Value = "";
            Hdf_Detalle_Documento.Value = "";
            Hdf_SubProceso_ID.Value = "";
            Txt_ID_Tramite.Text = "";
            Txt_Clave_Tramite.Text = "";
            Cmb_Dependencias.SelectedIndex = 0;
            Cmb_Areas.Items.Clear();
            Txt_Nombre.Text = "";
            Cmb_Tipo_Tramite.SelectedIndex = 1;
            Txt_Tiempo_Estimado.Text = "";
            Hdf_Cuenta_Contable_ID.Value = "";

            Txt_Parametro1.Text = "";
            Txt_Parametro2.Text = "";
            Txt_Parametro3.Text = "";
            Txt_Operador1.Text = "";
            Txt_Operador2.Text = "";
            Txt_Operador3.Text = "";

            Txt_Costo.Text = "";
            //Txt_Cuenta.Text = "";
            if (Cmb_Cuenta.SelectedIndex > 0)
                Cmb_Cuenta.SelectedIndex = 0;

            Txt_Descripcion.Text = "";
            Cmb_Autorizadores.SelectedIndex = 0;
            Txt_Dato_Tramite_ID.Text = "";
            Txt_Nombre_Dato.Text = "";
            Chk_Dato_Requerido.Checked = false;
            //Chck_Solicitar_Internet.Checked = false;
            Txt_Descripcion_Dato.Text = "";
            Cmb_Documentos_Tramites.SelectedIndex = 0;
            //  datos de el flujo del tramite
            Txt_Subproceso_ID.Text = "";
            //Txt_Nombre_Subproceso.Text = "";
            Txt_Valor_Subproceso.Text = "";
            Txt_Valor_Subproceso_Acumulado.Text = "0";
            Cmb_Platillas.SelectedIndex = 0;
            Txt_Descripcion_Subproceso.Text = "";
            Cmb_Tipo_Actividad.SelectedIndex = 0;
            //  datos de los grid
            Grid_Datos_Tramite.DataSource = new DataTable();
            Grid_Datos_Tramite.DataBind();
            Grid_Documentos_Tramite.DataSource = new DataTable();
            Grid_Documentos_Tramite.DataBind();
            Grid_Perfiles.DataSource = new DataTable();
            Grid_Perfiles.DataBind();
            Grid_Subprocesos_Tramite.DataSource = new DataTable();
            Grid_Subprocesos_Tramite.DataBind();
            Grid_Matriz_Costo.DataSource = new DataTable();
            Grid_Matriz_Costo.DataBind();
            Btn_Ver_Formato.Visible = false;


            Grid_Detalle_Formato.DataSource = new DataTable();
            Grid_Detalle_Formato.DataBind();

            Grid_Detalle_Plantilla.DataSource = new DataTable();
            Grid_Detalle_Plantilla.DataBind();

            //  datos de las sesiones
            Session.Remove("Dt_Autorizadores_Tramites");
            Session.Remove("Dt_Datos_Tramites");
            Session.Remove("Dt_Documentos_Tramites");
            Session.Remove("Dt_Subprocesos_Tramites");
            Session.Remove("Dt_Detalle_Formato");
            Session.Remove("Dt_Detalle_Plantilla");
            Session.Remove("Dt_Matriz_Costo");
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Configuracion_Subcatalogos_Estado_Original
    ///DESCRIPCIÓN: Regresa los botones de los catalogos dependientes (subcatalogos)
    ///             a su estado orignal.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Configuracion_Subcatalogos_Estado_Original()
    {
        try
        {
            Mostrar_Mensaje_Error(false, "");

            Btn_Agregar_Perfil.Visible = true;
            Btn_Modificar_Perfil.AlternateText = "Modificar Autorizador";
            Btn_Modificar_Perfil.Visible = true;
            Btn_Quitar_Perfil.Visible = true;
            Btn_Agregar_Dato.Visible = true;
            Btn_Modificar_Dato.AlternateText = "Modificar Dato";
            Btn_Modificar_Dato.Visible = true;
            Btn_Quitar_Dato.Visible = true;
            Btn_Agregar_Documento.Visible = true;
            Btn_Modificar_Documento.AlternateText = "Modificar Documento";
            Btn_Modificar_Documento.Visible = true;
            Btn_Quitar_Documento.Visible = true;
            Btn_Agregar_Subproceso.Visible = true;
            Btn_Modificar_Subproceso.AlternateText = "Modificar Subproceso";
            Btn_Modificar_Subproceso.Visible = true;
            Btn_Quitar_Subproceso.Visible = true;
            Btn_Subir_Orden_Subproceso.Visible = true;
            Btn_Bajar_Orden_Subproceso.Visible = true;
            Grid_Perfiles.Enabled = true;
            Grid_Datos_Tramite.Enabled = true;
            Grid_Documentos_Tramite.Enabled = true;
            Grid_Subprocesos_Tramite.Enabled = true;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Actualizar_Orden_Subprocesos
    ///DESCRIPCIÓN: Actualiza el campo de Orden que tienen los Subprocesos de la Tabla
    ///             para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Actualizar_Orden_Subprocesos()
    {
        try
        {
            if (Session["Dt_Subprocesos_Tramites"] != null)
            {
                DataTable Tabla = (DataTable)Session["Dt_Subprocesos_Tramites"];
                for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
                {
                    Tabla.DefaultView.AllowEdit = true;
                    Tabla.Rows[cnt].BeginEdit();
                    Tabla.Rows[cnt][4] = (cnt + 1).ToString();
                    Tabla.Rows[cnt].EndEdit();
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Actualizar_Orden_Datos
    ///DESCRIPCIÓN: Actualiza el campo de Orden que tienen los datos de la Tabla
    ///             para este tramite.
    ///PARAMETROS:     
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 06/Septiembre/20102
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Actualizar_Orden_Datos()
    {
        try
        {
            if (Session["Dt_Datos_Tramites"] != null)
            {
                DataTable Tabla = (DataTable)Session["Dt_Datos_Tramites"];
                for (int cnt = 0; cnt < Tabla.Rows.Count; cnt++)
                {
                    Tabla.DefaultView.AllowEdit = true;
                    Tabla.Rows[cnt].BeginEdit();
                    Tabla.Rows[cnt][5] = (cnt + 1).ToString();
                    Tabla.Rows[cnt].EndEdit();
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Obtener_Extension
    ///DESCRIPCIÓN: Maneja la extencion del archivo
    ///PROPIEDADES: String Ruta, direccion que 
    ///contiene el nombre del archivo al cual se le sacara la extension
    ///CREO: Francisco Gallardo
    ///FECHA_CREO: 16/Marzo/2010
    ///MODIFICO: Silvia Morales
    ///FECHA_MODIFICO: 19/Octubre/2010
    ///CAUSA_MODIFICACIÓN: Se adecuo al estandar
    ///*******************************************************************************
    private string Obtener_Extension(String Ruta)
    {

        String Extension = "";
        try
        {
            int index = Ruta.LastIndexOf(".");
            if (index < Ruta.Length)
            {
                Extension = Ruta.Substring(index + 1);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        return Extension;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Archivo
    ///DESCRIPCIÓN: Muestra un Archivo del cual se le pasa la ruta como parametro.
    ///PARAMETROS:
    ///             1.  Ruta.  Ruta del Archivo.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 19/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************  
    public void Mostrar_Archivo(String Ruta)
    {
        try
        {
            if (System.IO.File.Exists(Ruta))
            {
                //System.Diagnostics.Process proceso = new System.Diagnostics.Process();
                //proceso.StartInfo.FileName = Ruta;
                //proceso.Start();
                //proceso.Close();

                String Archivo = "";
                Archivo = "../../Archivos/Formato_Tramite/" + Path.GetFileName(Ruta);

                ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Archivo_Archivos", "window.open('" + Archivo + "','Window_Archivo','left=0,top=0')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('El archivo no existe o fue eliminado');", true);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "nada", "alert('" + ex.Message + "');", true);
        }
    }

    #endregion

    #region Llenar Combos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combos
    ///DESCRIPCIÓN: Llena los Combos que son utilizados en el Catalogo ( DEPENDENCIAS,
    ///             CUENTAS, AUTORIZADORES, PLANTILLAS Y DOCUMENTACION )
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 29/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************         
    private void Llenar_Combos()
    {
        Cls_Cat_Dependencias_Negocio Rs_Responsable = new Cls_Cat_Dependencias_Negocio();
        DataTable Dt_Unidad_Responsable = new DataTable();
        try
        {
            Cls_Cat_Tramites_Negocio Combos_Tramites = new Cls_Cat_Tramites_Negocio();
            //  filtro para las dependencias
            Combos_Tramites.P_Tipo_DataTable = "DEPENDENCIAS";
            DataTable Dependencias = Combos_Tramites.Consultar_DataTable();
            DataRow Fila_Dependencia = Dependencias.NewRow();
            Fila_Dependencia["DEPENDENCIA_ID"] = HttpUtility.HtmlDecode("SELECCIONE");
            Fila_Dependencia["NOMBRE"] = "< SELECCIONE >";
            Dependencias.Rows.InsertAt(Fila_Dependencia, 0);
            Cmb_Dependencias.DataSource = Dependencias;
            Cmb_Dependencias.DataValueField = "DEPENDENCIA_ID";
            Cmb_Dependencias.DataTextField = "NOMBRE";
            Cmb_Dependencias.DataBind();

            //  filtro para los perfiles
            Combos_Tramites.P_Tipo_DataTable = "PERFILES";
            DataTable Perfiles = Combos_Tramites.Consultar_DataTable();
            DataRow Fila_Perfiles = Perfiles.NewRow();
            Fila_Perfiles["PERFIL_ID"] = HttpUtility.HtmlDecode("SELECCIONE");
            Fila_Perfiles["NOMBRE"] = "< SELECCIONE >";
            Perfiles.Rows.InsertAt(Fila_Perfiles, 0);
            Cmb_Autorizadores.DataSource = Perfiles;
            Cmb_Autorizadores.DataValueField = "PERFIL_ID";
            Cmb_Autorizadores.DataTextField = "NOMBRE";
            Cmb_Autorizadores.DataBind();
            Combos_Tramites.P_Tipo_DataTable = "DOCUMENTOS";

            //  PARA LOS DOCUMENTOS
            DataTable Documentos = Combos_Tramites.Consultar_DataTable();
            //  se ordenaran de forma alfabetica
            DataView Dv_Ordenar = new DataView(Documentos);
            Dv_Ordenar.Sort = Cat_Tra_Documentos.Campo_Nombre;
            DataTable Dt_Documentos_Ordenados = Dv_Ordenar.ToTable();

            DataRow Fila_Documento = Dt_Documentos_Ordenados.NewRow();
            Fila_Documento["DOCUMENTO_ID"] = HttpUtility.HtmlDecode("SELECCIONE");
            Fila_Documento["NOMBRE"] = "< SELECCIONE >";
            Dt_Documentos_Ordenados.Rows.InsertAt(Fila_Documento, 0);
            Cmb_Documentos_Tramites.DataSource = Dt_Documentos_Ordenados;
            Cmb_Documentos_Tramites.DataValueField = "DOCUMENTO_ID";
            Cmb_Documentos_Tramites.DataTextField = "NOMBRE";
            Cmb_Documentos_Tramites.DataBind();
            Combos_Tramites.P_Tipo_DataTable = "PLANTILLAS";

            //  PARA LAS PLANTILLAS
            DataTable Plantillas = Combos_Tramites.Consultar_DataTable();
            DataRow Fila_Plantilla = Plantillas.NewRow();
            Fila_Plantilla["PLANTILLA_ID"] = HttpUtility.HtmlDecode("00000");
            Fila_Plantilla["NOMBRE"] = "< SELECCIONE >";
            Plantillas.Rows.InsertAt(Fila_Plantilla, 0);
            Cmb_Platillas.DataSource = Plantillas;
            Cmb_Platillas.DataValueField = "PLANTILLA_ID";
            Cmb_Platillas.DataTextField = "NOMBRE";
            Cmb_Platillas.DataBind();

            //  se cargara el combo de los formatos
            Combos_Tramites.P_Tipo_DataTable = "FORMATOS";
            DataTable Formato = Combos_Tramites.Consultar_DataTable();
            DataRow Fila_Formato = Plantillas.NewRow();

            Cmb_Formato.DataSource = Formato;
            Cmb_Formato.DataValueField = "FORMATO_ID";
            Cmb_Formato.DataTextField = "NOMBRE";
            Cmb_Formato.DataBind();
            Cmb_Formato.Items.Insert(0, "< SELECCIONE >");

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Llenar_Combo_Cuentas_Contables
    /// DESCRIPCION             : llama el combo de las cuenta contables
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 31-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Combo_Cuentas_Contables()
    {
        Cls_Cat_Tramites_Negocio Negocio_Cuentas_Contables = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Cuentas_Contables = new DataTable();
        try
        {

            Dt_Cuentas_Contables = Negocio_Cuentas_Contables.Consultar_Cuenta_Contable();

            if (Dt_Cuentas_Contables is DataTable)
            {
                if (Dt_Cuentas_Contables.Rows.Count > 0)
                {
                    Cmb_Cuenta.DataSource = Dt_Cuentas_Contables;
                    Cmb_Cuenta.DataValueField = Cat_Psp_SubConcepto_Ing.Campo_Clave;
                    Cmb_Cuenta.DataTextField = "Clave_Nombre";
                    Cmb_Cuenta.DataBind();
                    Cmb_Cuenta.Items.Insert(0, "< SELECCIONE >");
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Mostrar_Mensaje_Error
    ///DESCRIPCIÓN: Metodo que llena el grid view con el metodo de Consulta_tramites
    ///PARAMETROS:   
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  23/Octubre/2012
    ///MODIFICO:  
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    public void Mostrar_Mensaje_Error(Boolean Estatus, String Mensaje)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = Estatus;
            IBtn_Imagen_Error.Visible = Estatus;
            Lbl_Mensaje_Error.Visible = Estatus;
            Lbl_Mensaje_Error.Text = Mensaje;
        }
        catch (Exception Ex)
        {
            Mostrar_Mensaje_Error(true, Ex.Message.ToString());
            Div_Contenedor_Msj_Error.Visible = Estatus;
            IBtn_Imagen_Error.Visible = true;
            Lbl_Mensaje_Error.Visible = true;
            Lbl_Mensaje_Error.Text = "Error al mostrar mensaje de error. Error: [" + Ex.Message + "]";
            throw new Exception("Error al mostrar mensaje de error. Error: [" + Ex.Message + "]");
        }
    }
    ///*******************************************************************************
    /// NOMBRE DE LA FUNCION    : Llenar_Combo_Cuentas_Contables
    /// DESCRIPCION             : llama el combo de las cuenta contables
    ///CREO                     : Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO               : 31-may-2012 
    /// MODIFICO          :
    /// FECHA_MODIFICO    :
    /// CAUSA_MODIFICACION:
    ///*******************************************************************************
    private void Llenar_Combo_Actividades()
    {
        Cls_Cat_Tra_Actividades_Negocio Negocio_Actividades = new Cls_Cat_Tra_Actividades_Negocio();
        DataTable Dt_Actividades = new DataTable();
        try
        {
            Dt_Actividades = Negocio_Actividades.Consultar_Actividades();

            if (Dt_Actividades is DataTable)
            {
                if (Dt_Actividades.Rows.Count > 0)
                {
                    Cmb_Nombre_Actividad.DataSource = Dt_Actividades;
                    Cmb_Nombre_Actividad.DataValueField = Cat_Tra_Actividades.Campo_Nombre;
                    Cmb_Nombre_Actividad.DataTextField = Cat_Tra_Actividades.Campo_Nombre;
                    Cmb_Nombre_Actividad.DataBind();
                    Cmb_Nombre_Actividad.Items.Insert(0, "< SELECCIONE >");
                }
            }

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Combo_Cuentas
    ///DESCRIPCIÓN: Llena el Combo de Cuentas con una consulta que toma como filtro el
    ///             ID de la Dependencia al que pertenece.
    ///PARAMETROS:     
    ///             1. Dependencia_ID.  Filtro para sacar las cuentas de esta Dependencia.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    //private void Llenar_Combo_Cuentas(String Dependencia_ID){
    //    Cls_Cat_Tramites_Negocio Combo_Cuentas = new Cls_Cat_Tramites_Negocio();
    //    Combo_Cuentas.P_Tipo_DataTable = "CUENTAS";
    //    Combo_Cuentas.P_Dependencia_ID = Dependencia_ID;
    //    DataTable Cuentas = Combo_Cuentas.Consultar_DataTable();
    //    DataRow Fila_Cuenta = Cuentas.NewRow();
    //    Fila_Cuenta["CUENTA_ID"] = HttpUtility.HtmlDecode("SELECCIONE");
    //    Fila_Cuenta["NUMERO_CUENTA"] = HttpUtility.HtmlDecode("&lt;SELECCIONE&gt;");
    //    Cuentas.Rows.InsertAt(Fila_Cuenta, 0);
    //    Txt_Cuenta.DataSource = Cuentas;
    //    Txt_Cuenta.DataValueField = "CUENTA_ID";
    //    Txt_Cuenta.DataTextField = "NUMERO_CUENTA";
    //    Txt_Cuenta.DataBind();
    //}

    #endregion

    #region Validaciones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Generales_Tramites
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación en la Pestaña de Generales (Pestaña 1) de Tramites.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Generales_Tramites()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Clave_Tramite.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir la Clave del Tramite (Pestaña 1 de 6).";
            Validacion = false;
        }
        if (Cmb_Dependencias.SelectedIndex == 0)
        {
            if (!Validacion)
            {
                Mensaje_Error = Mensaje_Error + "<br>";
            }
            Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Dependecias (Pestaña 1 de 5).";
            Validacion = false;
        }
        if (Txt_Nombre.Text.Trim().Length == 0)
        {
            if (!Validacion)
            {
                Mensaje_Error = Mensaje_Error + "<br>";
            }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre (Pestaña 1 de 6).";
            Validacion = false;
        }
        //if (Cmb_Tipo_Tramite.SelectedIndex == 0) {
        //    if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
        //    Mensaje_Error = Mensaje_Error + "+ Seleccionar una opci&oacute;n en el Combo de Tipo de Tramite (Pestaña 1 de 5).";
        //    Validacion = false;
        //}
        if (Txt_Tiempo_Estimado.Text.Trim().Length == 0)
        {
            if (!Validacion)
            {
                Mensaje_Error = Mensaje_Error + "<br>";
            }
            Mensaje_Error = Mensaje_Error + "+ Introducir el Tiempo Estimado (Pestaña 1 de 6).";
            Validacion = false;
        }

        if (Txt_Descripcion.Text.Trim().Length == 0)
        {
            if (!Validacion)
            {
                Mensaje_Error = Mensaje_Error + "<br>";
            }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n (Pestaña 1 de 6).";
            Validacion = false;
        }

        //  validacion para el grupo de operador y parametro 3
        if (!String.IsNullOrEmpty(Txt_Operador3.Text.Trim()) || !String.IsNullOrEmpty(Txt_Parametro3.Text.Trim()))
        {
            if (String.IsNullOrEmpty(Txt_Operador2.Text.Trim()) || String.IsNullOrEmpty(Txt_Parametro2.Text.Trim()))
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ El parametro y operador 2 debe de ser llenado para poder contener al parametro y operador 3 (Pestaña 2 de 6).";
                Validacion = false;
            }

            if (Txt_Operador3.Text.Trim() != "" && Txt_Parametro3.Text.Trim() != "")
            {
            }
            else
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ El parametro y operador 3 debe de ser llenado  (Pestaña 2 de 6).";
                Validacion = false;
            }
        }
        //  validacion para el grupo de operador y parametro 2
        if (!String.IsNullOrEmpty(Txt_Operador2.Text.Trim()) || !String.IsNullOrEmpty(Txt_Parametro2.Text.Trim()))
        {
            if (String.IsNullOrEmpty(Txt_Operador1.Text.Trim()) || String.IsNullOrEmpty(Txt_Parametro1.Text.Trim()))
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ El parametro y operador 1 debe de ser llenado para poder contener al parametro y operador 2 (Pestaña 2 de 6).";
                Validacion = false;
            }
            if (Txt_Operador2.Text.Trim() != "" && Txt_Parametro2.Text.Trim() != "")
            {
            }
            else
            {
                if (!Validacion)
                {
                    Mensaje_Error = Mensaje_Error + "<br>";
                }
                Mensaje_Error = Mensaje_Error + "+ El parametro y operador 2 debe de ser llenado  (Pestaña 2 de 6).";
                Validacion = false;
            }
        }
        //  validacion para el grupo de operador y parametro 1
        if (String.IsNullOrEmpty(Txt_Operador1.Text.Trim()) || String.IsNullOrEmpty(Txt_Parametro1.Text.Trim()))
        {
            if (Txt_Operador1.Text.Trim() != "" && Txt_Parametro1.Text.Trim() != "")
            {
            }
            else
            {
                if ((Txt_Operador2.Text.Trim() != "" && Txt_Parametro2.Text.Trim() != "") ||
                    (Txt_Operador3.Text.Trim() != "" && Txt_Parametro3.Text.Trim() != ""))
                {
                    if (!Validacion)
                    {
                        Mensaje_Error = Mensaje_Error + "<br>";
                    }
                    Mensaje_Error = Mensaje_Error + "+ El parametro y operador 1 debe de ser llenado (Pestaña 2 de 6).";
                    Validacion = false;
                }
            }
        }


        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
            Mostrar_Mensaje_Error(true, Lbl_Mensaje_Error.Text);
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Datos_Tramites
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación en la Pestaña de Datos (Pestaña 3) de Tramites.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Datos_Tramites()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Txt_Nombre_Dato.Text.Trim().Length == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Introducir el Nombre del Dato.";
            Validacion = false;
        }
        if (Txt_Descripcion_Dato.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n del Dato.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    /// *******************************************************************************
    /// NOMBRE:         Validar_Clave_Tramite
    /// COMENTARIOS:    consultara la clave esto para saber si se repite la clave del tramite
    /// PARÁMETROS:     
    /// USUARIO CREÓ:   Hugo Enrique Ramírez Aguilera
    /// FECHA CREÓ:     09/Mayo/2012 
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA DE LA MODIFICACIÓN:
    /// ******************************************************************************
    private Boolean Validar_Clave_Tramite(String Clave_Modificar)
    {
        Cls_Cat_Tramites_Negocio Rs_Conusltar_Clave_Tramite = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        Boolean Validacion = true;
        Lbl_Mensaje_Error.Text = "Verificar.";
        String Mensaje_Error = "";
        try
        {
            if (Clave_Modificar == "")
            {
                //  se consulta la clave para ver si no esta repetida
                Rs_Conusltar_Clave_Tramite.P_Clave_Tramite = Txt_Clave_Tramite.Text.Trim().ToUpper();
                Dt_Consulta = Rs_Conusltar_Clave_Tramite.Consultar_Clave_Repetida();

                if (Dt_Consulta.Rows.Count > 0)
                {
                    Mensaje_Error += "* La clave " + Txt_Clave_Tramite.Text + " ya se encuentra registrada.";
                    Validacion = false;
                }

                if (!Validacion)
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else if (Clave_Modificar.ToUpper().Trim() != Txt_Clave_Tramite.Text.ToUpper())
            {
                Rs_Conusltar_Clave_Tramite.P_Clave_Tramite = Txt_Clave_Tramite.Text.Trim().ToUpper();
                Dt_Consulta = Rs_Conusltar_Clave_Tramite.Consultar_Clave_Repetida();

                if (Dt_Consulta.Rows.Count > 0)
                {
                    Mensaje_Error += "* La clave " + Txt_Clave_Tramite.Text.ToUpper() + " ya se encuentra registrada.";
                    Validacion = false;
                }

                if (!Validacion)
                {
                    Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
                    Div_Contenedor_Msj_Error.Visible = true;
                    Mostrar_Mensaje_Error(true, Lbl_Mensaje_Error.Text);
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        return Validacion;

    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Validar_Subprocesos_Tramites
    ///DESCRIPCIÓN: Hace una validacion de que haya datos en los componentes antes de hacer
    ///             una operación en la Pestaña de Subprocesos (Pestaña 5) de Tramites.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Validar_Subprocesos_Tramites()
    {
        Lbl_Mensaje_Error.Text = "Es necesario.";
        String Mensaje_Error = "";
        Boolean Validacion = true;
        if (Cmb_Nombre_Actividad.SelectedIndex == 0)
        {
            Mensaje_Error = Mensaje_Error + "+ Seleccionar el Nombre de la actividad.";
            Validacion = false;
        }
        if (Txt_Valor_Subproceso.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la el Valor de la actividad.";
            Validacion = false;
        }
        if (Txt_Descripcion_Subproceso.Text.Trim().Length == 0)
        {
            if (!Validacion) { Mensaje_Error = Mensaje_Error + "<br>"; }
            Mensaje_Error = Mensaje_Error + "+ Introducir la Descripci&oacute;n de la actividad.";
            Validacion = false;
        }

        if (Cmb_Tipo_Actividad.SelectedIndex == 0)
        {
            if (!Validacion)
            {
                Mensaje_Error = Mensaje_Error + "<br>";
            }
            Mensaje_Error = Mensaje_Error + "+ Seleccione el tipo de actividad.";
            Validacion = false;
        }
        if (!Validacion)
        {
            Lbl_Mensaje_Error.Text = HttpUtility.HtmlDecode(Mensaje_Error);
            Div_Contenedor_Msj_Error.Visible = true;
        }
        return Validacion;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_Repetido_En_Grid
    ///DESCRIPCIÓN: Busca un elemento en un DataTable
    ///PROPIEDADES:  
    ///             1.  ID_Elemento.Identificador del elemento que se esta buscando.
    ///             2.  Tabla.      Datatable donde se va a buscar el Elemento.
    ///             3.  Columna.    Columna en la que se buscara el identificador del
    ///                             elemento que se desea agregar.
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private Boolean Buscar_Repetido_En_Grid(Object ID_Elemento, DataTable Dt_Documentos, Int32 Columna)
    {
        Boolean Encontrada = false;
        try
        {
            if (Dt_Documentos != null && Dt_Documentos.Rows.Count > 0)
            {
                for (int cnt = 0; cnt < Dt_Documentos.Rows.Count; cnt++)
                {
                    if (Dt_Documentos.Rows[cnt][Columna].ToString().Equals(ID_Elemento))
                    {
                        Encontrada = true;
                        break;

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
        return Encontrada;
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Buscar_Documento_Repetido
    ///DESCRIPCIÓN: Busca un elemento en un DataTable
    ///PROPIEDADES:  
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  01/Noviembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///******************************************************************************i
    private Boolean Buscar_Documento_Repetido(Object ID_Elemento, DataTable Dt_Documentos, Int32 Columna, String Documento_Requerido, Int32 Indice)
    {
        Boolean Encontrada = false;
        DataTable Dt_Sesion = (DataTable)Session["Dt_Documentos_Tramites"];
        try
        {
            if (Dt_Documentos != null && Dt_Documentos.Rows.Count > 0)
            {
                for (int cnt = 0; cnt < Dt_Documentos.Rows.Count; cnt++)
                {
                    String Nombre = Dt_Documentos.Rows[cnt][Columna].ToString();
                    String Requerido = Dt_Documentos.Rows[cnt][2].ToString();

                    if (Dt_Documentos.Rows[cnt][Columna].ToString().Equals(ID_Elemento))
                    {
                        if (Nombre.Equals(Dt_Sesion.Rows[Indice][Columna].ToString()) && Requerido.Equals(Dt_Sesion.Rows[Indice][2].ToString()))
                        {
                            Encontrada = false;
                            break;
                        }
                        else
                        {
                            Encontrada = true;
                            break;
                        }

                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
        return Encontrada;
    }

    #endregion

    #endregion

    #region Grid

    #region Eventos

    #region Grid_Tramites_Generales

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tramites_Generales_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Tramites 
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tramites_Generales_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            Grid_Tramites_Generales.SelectedIndex = (-1);
            Llenar_Grid_Tramites(e.NewPageIndex);
            Limpiar_Catalogo();
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Tramites_Generales_PageIndexChanging
    ///DESCRIPCIÓN: Obtiene los datos de un Tramite Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Tramites_Generales_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Tramites_Negocio Tramite = new Cls_Cat_Tramites_Negocio();
        Cls_Cat_Tramites_Negocio Negocio_Cuentas_Contables = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Cuentas_Contables = new DataTable();

        DataTable Dt_Consultar_Avance = new DataTable();
        DataTable Dt_Detalles_Plantillas = new DataTable();
        DataTable Dt_Detalles_Formato = new DataTable();
        Double Valor = 0.0;
        try
        {
            if (Grid_Tramites_Generales.SelectedIndex > (-1))
            {
                Limpiar_Catalogo();

                String ID_Seleccionado = Grid_Tramites_Generales.SelectedRow.Cells[1].Text;

                Tramite.P_Tramite_ID = ID_Seleccionado;

                //  se consultara el avance del flujo del tramite
                Dt_Consultar_Avance = Tramite.Consultar_Avance();
                if (Dt_Consultar_Avance.Rows.Count > 0)
                    Txt_Valor_Subproceso_Acumulado.Text = Dt_Consultar_Avance.Rows[0][0].ToString();
                else
                    Txt_Valor_Subproceso_Acumulado.Text = "0";

                //  se consultan los datos del tramite
                Tramite = Tramite.Consultar_Datos_Tramite();
                Hdf_Tramite_ID.Value = Tramite.P_Tramite_ID;
                Txt_ID_Tramite.Text = Tramite.P_Tramite_ID;
                Txt_Clave_Tramite.Text = Tramite.P_Clave_Tramite;
                Hdf_Clave_Tramite.Value = Tramite.P_Clave_Tramite;
                Txt_Nombre.Text = Tramite.P_Nombre;
                Txt_Tiempo_Estimado.Text = Tramite.P_Tiempo_Estimado.ToString();
                Txt_Costo.Text = Tramite.P_Costo.ToString("#,###,###.00");

                /***************** inicio parametros de cobro de ordenamiento *********************/
                Txt_Parametro1.Text = Tramite.P_Parametro1.ToString();
                Txt_Parametro2.Text = Tramite.P_Parametro2.ToString();
                Txt_Parametro3.Text = Tramite.P_Parametro3.ToString();
                Txt_Operador1.Text = Tramite.P_Operador1.ToString();
                Txt_Operador2.Text = Tramite.P_Operador2.ToString();
                Txt_Operador3.Text = Tramite.P_Operador3.ToString();
                /***************** fin parametros de cobro de ordenamiento *********************/

                //Chck_Solicitar_Internet.Checked = (Tramite.P_Solicitar_Intenet == "S") ? true : false;
                Txt_Descripcion.Text = Tramite.P_Descripcion;
                Cmb_Dependencias.SelectedIndex = Cmb_Dependencias.Items.IndexOf(Cmb_Dependencias.Items.FindByValue(Tramite.P_Dependencia_ID));
                Cmb_Dependencias_SelectedIndexChanged(sender, null);
                Cmb_Estatus_Tramite.SelectedIndex = Cmb_Estatus_Tramite.Items.IndexOf(Cmb_Estatus_Tramite.Items.FindByValue(Tramite.P_Estatus_Tramite));
                Llenar_Combo_Cuentas_Contables();

                Negocio_Cuentas_Contables.P_Cuenta = Tramite.P_Cuenta_ID.Trim();
                Dt_Cuentas_Contables = Negocio_Cuentas_Contables.Consultar_Cuenta_Contable();
                String Nombre_Cuenta = "";
                if (Dt_Cuentas_Contables != null && Dt_Cuentas_Contables.Rows.Count > 0)
                {
                    Nombre_Cuenta = Dt_Cuentas_Contables.Rows[0][Cat_Psp_SubConcepto_Ing.Campo_Descripcion].ToString();
                }

                Cmb_Cuenta.SelectedIndex = Cmb_Cuenta.Items.IndexOf(Cmb_Cuenta.Items.FindByText(Tramite.P_Cuenta_ID.Trim() + "     " + Nombre_Cuenta));

                //Cmb_Estatus.SelectedIndex = Cmb_Estatus.Items.IndexOf(Cmb_Estatus.Items.FindByValue(Estatus));
                Hdf_Cuenta_Contable_ID.Value = Tramite.P_Cuenta_Contable_Clave;
                Llenar_Grid_Autorizadores(0, Tramite.P_Perfiles_Autorizar);
                Llenar_Grid_Datos(0, Tramite.P_Datos_Tramite);
                Llenar_Grid_Documentos(0, Tramite.P_Documentacion_Tramite);
                Llenar_Grid_Subprocesos(0, Tramite.P_SubProcesos_Tramite);
                Llenar_Grid_Matriz(Tramite.P_Matriz_Costo);

                DataTable Dt_Orden = Tramite.P_SubProcesos_Tramite.Clone();
                Dt_Orden = Tramite.P_SubProcesos_Tramite.Copy();

                if (Dt_Orden != null && Dt_Orden.Rows.Count > 0)
                    Hdf_Orden_Subporceso.Value = "" + (Convert.ToDouble(Dt_Orden.Rows[Dt_Orden.Rows.Count - 1]["ORDEN"].ToString()) + 1);

                Btn_Ver_Requisitos.Enabled = true;
                Btn_Salir.AlternateText = "Regresar";
                System.Threading.Thread.Sleep(1000);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Datos_Tramite_OnSelectedIndexChanged
    ///DESCRIPCIÓN: Obtiene los datos de un requisito Seleccionado para mostrarlos a detalle
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 31/Agosto/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Datos_Tramite_OnSelectedIndexChanged(object sender, EventArgs e)
    {
    }

    #endregion

    #region Grid_Internos_SubCatalogos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Perfiles_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Perfiles 
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Perfiles_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Autorizadores_Tramites"] != null)
            {
                Grid_Perfiles.SelectedIndex = (-1);
                Llenar_Grid_Autorizadores(e.NewPageIndex, (DataTable)Session["Dt_Autorizadores_Tramites"]);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Datos_Tramite_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Datos 
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Datos_Tramite_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Datos_Tramites"] != null)
            {
                Grid_Datos_Tramite.SelectedIndex = (-1);
                Llenar_Grid_Datos(e.NewPageIndex, (DataTable)Session["Dt_Datos_Tramites"]);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Documentos_Tramite_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Documentos 
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Documentos_Tramite_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Documentos_Tramites"] != null)
            {
                Grid_Documentos_Tramite.SelectedIndex = (-1);
                Llenar_Grid_Documentos(e.NewPageIndex, (DataTable)Session["Dt_Documentos_Tramites"]);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Grid_Subprocesos_Tramite_PageIndexChanging
    ///DESCRIPCIÓN: Maneja la paginación del GridView de los Subprocesos 
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Grid_Subprocesos_Tramite_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        try
        {
            if (Session["Dt_Subprocesos_Tramites"] != null)
            {
                Grid_Subprocesos_Tramite.SelectedIndex = (-1);
                Llenar_Grid_Subprocesos(e.NewPageIndex, (DataTable)Session["Dt_Subprocesos_Tramites"]);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #endregion

    #region Metodos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Tramites
    ///DESCRIPCIÓN: Llena la tabla de Tramites con una consulta que puede o no
    ///             tener Filtros.
    ///PARAMETROS:     
    ///             1. Pagina.  Pagina en la cual se mostrará el Grid de Generales
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 28/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Tramites(int Pagina)
    {
        try
        {
            Cls_Cat_Tramites_Negocio Tramite = new Cls_Cat_Tramites_Negocio();
            Tramite.P_Tipo_DataTable = "TRAMITES";
            //Tramite.P_Nombre = Txt_Busqueda_Tramite.Text.Trim();
            //cargar la consulta dentro del grid
            Grid_Tramites_Generales.Columns[1].Visible = true;
            Grid_Tramites_Generales.DataSource = new DataTable();
            Grid_Tramites_Generales.PageIndex = Pagina;
            Grid_Tramites_Generales.DataBind();
            Grid_Tramites_Generales.Columns[1].Visible = false;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Autorizadores
    ///DESCRIPCIÓN: Llena el Grid de Autorizadores con los que estan dados de alta para este
    ///             tramite.
    ///PARAMETROS:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Autorizadores(int Pagina, DataTable Tabla)
    {
        try
        {
            Grid_Perfiles.Columns[1].Visible = true;
            Grid_Perfiles.DataSource = Tabla;
            Grid_Perfiles.PageIndex = Pagina;
            Grid_Perfiles.DataBind();
            Grid_Perfiles.Columns[1].Visible = false;
            Session["Dt_Autorizadores_Tramites"] = Tabla;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Datos
    ///DESCRIPCIÓN: Llena el Grid de Datos con los que estan dados de alta para este
    ///             tramite.
    ///PARAMETROS:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Datos(int Pagina, DataTable Tabla)
    {
        try
        {
            if (Tabla != null && Tabla.Rows.Count > 0)
            {
                DataView Dv_Ordenar = new DataView(Tabla);
                Dv_Ordenar.Sort = Cat_Tra_Datos_Tramite.Campo_Orden;
                Tabla = Dv_Ordenar.ToTable();
            }

            Grid_Datos_Tramite.Columns[1].Visible = true;
            Grid_Datos_Tramite.DataSource = Tabla;
            Grid_Datos_Tramite.PageIndex = Pagina;
            Grid_Datos_Tramite.DataBind();
            Grid_Datos_Tramite.Columns[1].Visible = false;
            Session["Dt_Datos_Tramites"] = Tabla;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentos
    ///DESCRIPCIÓN: Llena el Grid de Documentos con los que estan dados de alta para este
    ///             tramite.
    ///PARAMETROS:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Documentos(int Pagina, DataTable Tabla)
    {
        try
        {
            DataView Dv_Ordenar = new DataView(Tabla);
            Dv_Ordenar.Sort = Cat_Tra_Documentos.Campo_Nombre;
            Tabla = Dv_Ordenar.ToTable();

            Grid_Documentos_Tramite.Columns[1].Visible = true;
            Grid_Documentos_Tramite.Columns[2].Visible = true;
            Grid_Documentos_Tramite.DataSource = Tabla;
            Grid_Documentos_Tramite.PageIndex = Pagina;
            Grid_Documentos_Tramite.DataBind();
            Grid_Documentos_Tramite.Columns[1].Visible = false;
            Grid_Documentos_Tramite.Columns[2].Visible = false;
            Session["Dt_Documentos_Tramites"] = Tabla;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Limpia_Controles " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Documentos
    ///DESCRIPCIÓN: Llena el Grid de Documentos con los que estan dados de alta para este
    ///             tramite.
    ///PARAMETROS:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Matriz(DataTable Tabla)
    {
        try
        {
            //DataView Dv_Ordenar = new DataView(Tabla);
            //Dv_Ordenar.Sort = Cat_Tra_Documentos.Campo_Nombre;
            //Tabla = Dv_Ordenar.ToTable();

            Grid_Matriz_Costo.Columns[0].Visible = true;
            Grid_Matriz_Costo.Columns[1].Visible = true;
            Grid_Matriz_Costo.DataSource = Tabla;
            Grid_Matriz_Costo.DataBind();
            Grid_Matriz_Costo.Columns[0].Visible = false;
            Grid_Matriz_Costo.Columns[1].Visible = false;
            Session["Dt_Matriz_Costo"] = Tabla;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Llenar_Grid_Matriz " + ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Subprocesos
    ///DESCRIPCIÓN: Llena el Grid de Subprocesos con los que estan dados de alta para este
    ///             tramite.
    ///PARAMETROS:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Subprocesos(int Pagina, DataTable Tabla)
    {
        try
        {
            Grid_Subprocesos_Tramite.Columns[1].Visible = true;
            Grid_Subprocesos_Tramite.DataSource = Tabla;
            Grid_Subprocesos_Tramite.PageIndex = Pagina;
            Grid_Subprocesos_Tramite.DataBind();
            Grid_Subprocesos_Tramite.Columns[1].Visible = false;
            Session["Dt_Subprocesos_Tramites"] = Tabla;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Tabla_Subprocesos
    ///DESCRIPCIÓN: Llena el Grid de Subprocesos con los que estan dados de alta para este
    ///             tramite.
    ///PARAMETROS:     
    ///             1.  Pagina. Pagina en la cual se mostrará el Grid_VIew
    ///             2.  tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Matriz_Costo(DataTable Dt_Tabla)
    {
        try
        {
            Grid_Matriz_Costo.Columns[0].Visible = true;
            Grid_Matriz_Costo.Columns[1].Visible = true;
            Grid_Subprocesos_Tramite.DataSource = Dt_Tabla;
            Grid_Subprocesos_Tramite.DataBind();
            Grid_Subprocesos_Tramite.Columns[0].Visible = false;
            Grid_Subprocesos_Tramite.Columns[1].Visible = false;
            Session["Dt_Matriz_Costo"] = Dt_Tabla;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Datos
    ///DESCRIPCIÓN: Llena el Grid de Subprocesos con los que estan dados de alta para este
    ///             tramite.
    ///PARAMETROS:     tabla.  Tabla que se va a cargar en el Grid.    
    ///CREO: Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO: 06/Septiembre/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Datos(DataTable Tabla)
    {
        try
        {
            Grid_Datos_Tramite.Columns[1].Visible = true;
            Grid_Datos_Tramite.DataSource = Tabla;
            Grid_Datos_Tramite.DataBind();
            Grid_Datos_Tramite.Columns[1].Visible = false;
            Session["Dt_Datos_Tramites"] = Tabla;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Plantillas
    ///DESCRIPCIÓN: Agrega el detalle de la plantilla
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  17/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Plantillas(DataTable Tabla)
    {
        try
        {
            Grid_Detalle_Plantilla.Columns[1].Visible = true;
            Grid_Detalle_Plantilla.DataSource = Tabla;
            Grid_Detalle_Plantilla.DataBind();
            Grid_Detalle_Plantilla.Columns[1].Visible = false;
            if (Tabla.Rows.Count == 0)
                Session.Remove("Dt_Detalle_Plantilla");

            else
            {
                Session["Dt_Detalle_Plantilla"] = Tabla;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Llenar_Grid_Formato
    ///DESCRIPCIÓN: Agrega el detalle de la plantilla
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  17/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    private void Llenar_Grid_Formato(DataTable Tabla)
    {
        try
        {
            Grid_Detalle_Formato.Columns[1].Visible = true;
            Grid_Detalle_Formato.DataSource = Tabla;
            Grid_Detalle_Formato.DataBind();
            Grid_Detalle_Formato.Columns[1].Visible = false;

            if (Tabla.Rows.Count == 0)
                Session.Remove("Dt_Detalle_Formato");

            else
                Session["Dt_Detalle_Formato"] = Tabla;


        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #endregion

    #region Eventos

    #region Autorizadores

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Perfil_Click
    ///DESCRIPCIÓN: Agrega un nuevo Autorizador a la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Perfil_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cmb_Autorizadores.SelectedIndex > 0)
            {
                DataTable Tabla;
                String Elemento_ID = Cmb_Autorizadores.SelectedItem.Value;
                if (Session["Dt_Autorizadores_Tramites"] == null)
                {
                    Tabla = new DataTable("Perfiles_Tramites");
                    Tabla.Columns.Add("DETALLE_AUTORIZACION_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("PERFIL_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Autorizadores_Tramites"];
                }
                if (!Buscar_Repetido_En_Grid(Elemento_ID, Tabla, 1))
                {
                    DataRow Fila = Tabla.NewRow();
                    Fila["DETALLE_AUTORIZACION_ID"] = HttpUtility.HtmlDecode("");
                    Fila["PERFIL_ID"] = HttpUtility.HtmlDecode(Cmb_Autorizadores.SelectedItem.Value);
                    Fila["NOMBRE"] = HttpUtility.HtmlDecode(Cmb_Autorizadores.SelectedItem.Text);
                    Tabla.Rows.Add(Fila);
                    Grid_Perfiles.SelectedIndex = (-1);
                    Llenar_Grid_Autorizadores(Grid_Perfiles.PageIndex, Tabla);
                    Cmb_Autorizadores.SelectedIndex = 0;
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "El Perfil Seleccionado ya se encuentra seleccionado.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Debe Seleccionar un Perfil para Autorizar del Combo.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Perfil_Click
    ///DESCRIPCIÓN: Modifica un Autorizador de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Perfil_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Btn_Modificar_Perfil.AlternateText.Equals("Modificar Autorizador"))
            {
                if (Grid_Perfiles.Rows.Count > 0 && Grid_Perfiles.SelectedIndex > (-1))
                {
                    if (Session["Dt_Autorizadores_Tramites"] != null && ((DataTable)Session["Dt_Autorizadores_Tramites"]).Rows.Count > 0)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Autorizadores_Tramites"];
                        Int32 Registro = ((Grid_Perfiles.PageIndex) * Grid_Perfiles.PageSize) + (Grid_Perfiles.SelectedIndex);
                        Hdf_Detalle_Autorizar_ID.Value = Tabla.Rows[Registro][0].ToString();
                        Cmb_Autorizadores.SelectedIndex = Cmb_Autorizadores.Items.IndexOf(Cmb_Autorizadores.Items.FindByValue(Tabla.Rows[Registro][1].ToString()));
                        Btn_Modificar_Perfil.AlternateText = "Actualizar Autorizador";
                        Btn_Quitar_Perfil.Visible = false;
                        Btn_Agregar_Perfil.Visible = false;
                        Grid_Perfiles.Enabled = false;
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el Autorizador que se desea Modificar.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Cmb_Autorizadores.SelectedIndex > 0)
                {
                    Int32 Registro = ((Grid_Perfiles.PageIndex) * Grid_Perfiles.PageSize) + (Grid_Perfiles.SelectedIndex);
                    if (Session["Dt_Autorizadores_Tramites"] != null)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Autorizadores_Tramites"];
                        if (!Buscar_Repetido_En_Grid(Cmb_Autorizadores.SelectedItem.Value, Tabla, 1))
                        {
                            Tabla.DefaultView.AllowEdit = true;
                            Tabla.Rows[Registro].BeginEdit();
                            Tabla.Rows[Registro][1] = Cmb_Autorizadores.SelectedItem.Value;
                            Tabla.Rows[Registro][2] = Cmb_Autorizadores.SelectedItem.Text;
                            Tabla.Rows[Registro].EndEdit();
                            Session["Dt_Autorizadores_Tramites"] = Tabla;
                            Grid_Perfiles.SelectedIndex = (-1);
                            Llenar_Grid_Autorizadores(Grid_Perfiles.PageIndex, Tabla);
                            Btn_Modificar_Perfil.AlternateText = "Modificar Autorizador";
                            Btn_Quitar_Perfil.Visible = true;
                            Btn_Agregar_Perfil.Visible = true;
                            Tab_Contenedor_Pestagnas.TabIndex = 0;
                            Grid_Perfiles.Enabled = true;
                            Hdf_Detalle_Autorizar_ID.Value = "";
                            Cmb_Autorizadores.SelectedIndex = 0;
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Text = "El Perfil Seleccionado ya se encuentra seleccionado.";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Debe Seleccionar un Perfil para Autorizar del Combo.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Perfil_Click
    ///DESCRIPCIÓN: Quita un Autorizador de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Perfil_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;

            if (Grid_Perfiles.Rows.Count > 0 && Grid_Perfiles.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Perfiles.PageIndex) * Grid_Perfiles.PageSize) + (Grid_Perfiles.SelectedIndex);
                if (Session["Dt_Autorizadores_Tramites"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Autorizadores_Tramites"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Autorizadores_Tramites"] = Tabla;
                    Grid_Perfiles.SelectedIndex = (-1);
                    Llenar_Grid_Autorizadores(Grid_Perfiles.PageIndex, Tabla);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Debe seleccionar el Autorizador que se desea Quitar.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Documentacion





    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Matiz_Click
    ///DESCRIPCIÓN: Agrega un nuevo elemento a la matriz
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  09/Octubre/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Matiz_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Txt_Matriz_Tipo.Text != "" && Txt_Matriz_Costo.Text != "")
            {
                DataTable Dt_Matriz_Costo = new DataTable();
                //String Elemento_ID = Cmb_Documentos_Tramites.SelectedItem.Value;
                if (Session["Dt_Matriz_Costo"] == null)
                {
                    Dt_Matriz_Costo = new DataTable("Dt_Matriz_Costo");
                    Dt_Matriz_Costo.Columns.Add("MATRIZ_ID", Type.GetType("System.String"));
                    Dt_Matriz_Costo.Columns.Add("TRAMITE_ID", Type.GetType("System.String"));
                    Dt_Matriz_Costo.Columns.Add("TIPO", Type.GetType("System.String"));
                    Dt_Matriz_Costo.Columns.Add("COSTO_BASE", Type.GetType("System.String"));
                }
                else
                {
                    Dt_Matriz_Costo = (DataTable)Session["Dt_Matriz_Costo"];
                }
                if (!Buscar_Repetido_En_Grid(Txt_Matriz_Tipo.Text, Dt_Matriz_Costo, 2))
                {
                    DataRow Fila = Dt_Matriz_Costo.NewRow();
                    Fila["MATRIZ_ID"] = HttpUtility.HtmlDecode("");
                    Fila["TRAMITE_ID"] = HttpUtility.HtmlDecode("");
                    Fila["TIPO"] = HttpUtility.HtmlDecode(Txt_Matriz_Tipo.Text);
                    Fila["COSTO_BASE"] = HttpUtility.HtmlDecode(Txt_Matriz_Costo.Text);
                    Dt_Matriz_Costo.Rows.Add(Fila);
                    Llenar_Grid_Matriz(Dt_Matriz_Costo);
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "El Tipo ya se encuentra seleccionado.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }

                Txt_Matriz_Costo.Text = "";
                Txt_Matriz_Tipo.Text = "";
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Debe ingresar el tipo y costo";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE:          Btn_Img_Quitar_OnClick
    ///DESCRIPCIÓN:     Realizara los el perfil del grid
    ///PARAMETROS: 
    ///CREO:            Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:      25/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Img_Quitar_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            ImageButton Btn_Eliminar = (ImageButton)sender;
            TableCell Tabla = (TableCell)Btn_Eliminar.Parent;
            GridViewRow Row = (GridViewRow)Tabla.Parent;
            Grid_Matriz_Costo.SelectedIndex = Row.RowIndex;
            int Fila = Row.RowIndex;

            DataTable Dt_Elimiar_Registro = (DataTable)Session["Dt_Matriz_Costo"];
            Dt_Elimiar_Registro.Rows.RemoveAt(Fila);
            Session["Dt_Matriz_Costo"] = Dt_Elimiar_Registro;
            Grid_Matriz_Costo.SelectedIndex = (-1);
            Llenar_Grid_Matriz(Dt_Elimiar_Registro);


        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Btn_Img_Quitar_Click " + ex.Message.ToString(), ex);
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Documento_Click
    ///DESCRIPCIÓN: Agrega un nuevo Documento a la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Documentos = new DataTable();
        String Elemento_ID = "";
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cmb_Documentos_Tramites.SelectedIndex > 0)
            {
                Elemento_ID = Cmb_Documentos_Tramites.SelectedItem.Value;
                if (Session["Dt_Documentos_Tramites"] == null)
                {
                    Dt_Documentos = new DataTable("Documentos_Tramites");
                    Dt_Documentos.Columns.Add("DETALLE_DOCUMENTO_ID", Type.GetType("System.String"));
                    Dt_Documentos.Columns.Add("DOCUMENTO_ID", Type.GetType("System.String"));
                    Dt_Documentos.Columns.Add("DOCUMENTO_REQUERIDO", Type.GetType("System.String"));
                    Dt_Documentos.Columns.Add("NOMBRE", Type.GetType("System.String"));
                }
                else
                    Dt_Documentos = (DataTable)Session["Dt_Documentos_Tramites"];


                if (!Buscar_Repetido_En_Grid(Elemento_ID, Dt_Documentos, 1))
                {
                    DataRow Fila = Dt_Documentos.NewRow();
                    Fila["DETALLE_DOCUMENTO_ID"] = HttpUtility.HtmlDecode("");
                    Fila["DOCUMENTO_ID"] = HttpUtility.HtmlDecode(Cmb_Documentos_Tramites.SelectedItem.Value);
                    Fila["NOMBRE"] = HttpUtility.HtmlDecode(Cmb_Documentos_Tramites.SelectedItem.Text);

                    if (Chk_Documento_Requerido.Checked == true)
                        Fila["DOCUMENTO_REQUERIDO"] = "S";

                    else
                        Fila["DOCUMENTO_REQUERIDO"] = "N";

                    Dt_Documentos.Rows.Add(Fila);
                    Grid_Documentos_Tramite.SelectedIndex = (-1);
                    Llenar_Grid_Documentos(Grid_Documentos_Tramite.PageIndex, Dt_Documentos);

                    //  se inicializan los elementos
                    Cmb_Documentos_Tramites.SelectedIndex = 0;
                    Chk_Documento_Requerido.Checked = false;
                }

                else
                    Mostrar_Mensaje_Error(true, "El Documento Seleccionado ya se encuentra seleccionado.");

            }
            else
            {
                Lbl_Mensaje_Error.Text = "Debe Seleccionar un Documento del Combo.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Documento_Click
    ///DESCRIPCIÓN: Modifica un Documento de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Documentos = new DataTable();
        String Documento_Requerido = "";
        Int32 Registro = 0;
        try
        {
            Mostrar_Mensaje_Error(false, "");

            if (Btn_Modificar_Documento.AlternateText.Equals("Modificar Documento"))
            {
                if (Grid_Documentos_Tramite.Rows.Count > 0 && Grid_Documentos_Tramite.SelectedIndex > (-1))
                {
                    if (Session["Dt_Documentos_Tramites"] != null && ((DataTable)Session["Dt_Documentos_Tramites"]).Rows.Count > 0)
                    {
                        Dt_Documentos = (DataTable)Session["Dt_Documentos_Tramites"];
                        Registro = ((Grid_Documentos_Tramite.PageIndex) * Grid_Documentos_Tramite.PageSize) + (Grid_Documentos_Tramite.SelectedIndex);
                        Hdf_Detalle_Documento.Value = Dt_Documentos.Rows[Registro][0].ToString();

                        if (Dt_Documentos.Rows[Registro][2].ToString() == "S")
                            Chk_Documento_Requerido.Checked = true;
                        else
                            Chk_Documento_Requerido.Checked = false;

                        Cmb_Documentos_Tramites.SelectedIndex = Cmb_Documentos_Tramites.Items.IndexOf(Cmb_Documentos_Tramites.Items.FindByValue(Dt_Documentos.Rows[Registro][1].ToString()));
                        Btn_Modificar_Documento.AlternateText = "Actualizar Documento";
                        Btn_Quitar_Documento.Visible = false;
                        Btn_Agregar_Documento.Visible = false;
                        Grid_Documentos_Tramite.Enabled = false;
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el Documento que se desea Modificar.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Cmb_Documentos_Tramites.SelectedIndex > 0)
                {
                    Registro = ((Grid_Documentos_Tramite.PageIndex) * Grid_Documentos_Tramite.PageSize) + (Grid_Documentos_Tramite.SelectedIndex);
                    if (Session["Dt_Documentos_Tramites"] != null)
                    {
                        Dt_Documentos = (DataTable)Session["Dt_Documentos_Tramites"];

                        if (Chk_Documento_Requerido.Checked == true)
                            Documento_Requerido = "S";
                        else
                            Documento_Requerido = "N";

                        if (!Buscar_Documento_Repetido(Cmb_Documentos_Tramites.SelectedItem.Value, Dt_Documentos, 1, Documento_Requerido, Registro))
                        {
                            Dt_Documentos.DefaultView.AllowEdit = true;
                            Dt_Documentos.Rows[Registro].BeginEdit();
                            Dt_Documentos.Rows[Registro][1] = Cmb_Documentos_Tramites.SelectedItem.Value;
                            Dt_Documentos.Rows[Registro][3] = Cmb_Documentos_Tramites.SelectedItem.Text;
                            Dt_Documentos.Rows[Registro][2] = Documento_Requerido;
                            Dt_Documentos.Rows[Registro].EndEdit();

                            DataView Dv_Ordenar = new DataView(Dt_Documentos);
                            Dv_Ordenar.Sort = Cat_Tra_Documentos.Campo_Nombre;
                            Dt_Documentos = Dv_Ordenar.ToTable();

                            Session["Dt_Documentos_Tramites"] = Dt_Documentos;
                            Grid_Documentos_Tramite.SelectedIndex = (-1);
                            Llenar_Grid_Documentos(Grid_Documentos_Tramite.PageIndex, Dt_Documentos);
                            Btn_Modificar_Documento.AlternateText = "Modificar Documento";
                            Btn_Quitar_Documento.Visible = true;
                            Btn_Agregar_Documento.Visible = true;
                            Tab_Contenedor_Pestagnas.TabIndex = 0;
                            Grid_Documentos_Tramite.Enabled = true;
                            Hdf_Detalle_Documento.Value = "";
                            Cmb_Documentos_Tramites.SelectedIndex = 0;
                            Chk_Documento_Requerido.Checked = false;
                        }
                        else
                        {
                            Mostrar_Mensaje_Error(true, "El Documento Seleccionado ya se encuentra dentro de la tabla.");
                        }
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Debe Seleccionar un Documento para Autorizar del Combo.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Documento_Click
    ///DESCRIPCIÓN: Quita un Documento de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Documento_Click(object sender, ImageClickEventArgs e)
    {
        DataTable Dt_Documentos = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false, "");

            if (Grid_Documentos_Tramite.Rows.Count > 0 && Grid_Documentos_Tramite.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Documentos_Tramite.PageIndex) * Grid_Documentos_Tramite.PageSize) + (Grid_Documentos_Tramite.SelectedIndex);
                if (Session["Dt_Documentos_Tramites"] != null)
                {
                    Dt_Documentos = (DataTable)Session["Dt_Documentos_Tramites"];
                    Dt_Documentos.Rows.RemoveAt(Registro);

                    DataView Dv_Ordenar = new DataView(Dt_Documentos);
                    Dv_Ordenar.Sort = Cat_Tra_Documentos.Campo_Nombre;
                    Dt_Documentos = Dv_Ordenar.ToTable();

                    Session["Dt_Documentos_Tramites"] = Dt_Documentos;
                    Grid_Documentos_Tramite.SelectedIndex = (-1);
                    Llenar_Grid_Documentos(Grid_Documentos_Tramite.PageIndex, Dt_Documentos);
                }
            }
            else
            {
                Mostrar_Mensaje_Error(true, "Debe seleccionar el Documento que se desea Quitar.");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Requisitos_Click
    ///DESCRIPCIÓN: Mostrara los documentos que se requieren para el tramite
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ver_Requisitos_Click(object sender, EventArgs e)
    {
        Cls_Ope_Ven_Lista_Tramites_Negocio Rs_Consulta_Documentos = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        String Tramite_ID = String.Empty;
        String Nombre_Tramite = String.Empty;
        String Descipcion_Tramite = String.Empty;
        DataTable Dt_Consulta = new DataTable();
        DataTable Dt_Actividades = new DataTable();
        DataSet Ds_Reporte = new DataSet();
        try
        {
            Mensaje_Error(false);
            if (Hdf_Tramite_ID.Value != "")
            {
                Rs_Consulta_Documentos.P_Tramite_ID = Hdf_Tramite_ID.Value;
                Dt_Consulta = Rs_Consulta_Documentos.Consultar_Documentos_Tramites();
                Dt_Actividades = Rs_Consulta_Documentos.Consultar_Actividades_Tramites();
                Dt_Consulta.TableName = "Dt_Datos_Tramite";
                Dt_Actividades.TableName = "Dt_Actividades";
                Ds_Reporte.Tables.Add(Dt_Consulta.Copy());
                Ds_Reporte.Tables.Add(Dt_Actividades.Copy());

                Generar_Reporte(ref Ds_Reporte, "Rpt_Ven_Lista_Documentos_Tramites.rpt", "Reporte_Documentos_Tramite" + Session.SessionID + ".pdf");
            }
            else
            {
                Mensaje_Error(true);
                Lbl_Mensaje_Error.Text = "Seleccion el tramite";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Error al selecionar una dependencia. Error: [" + ex.Message + "]");
        }
    }

    #endregion

    #region Datos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Dato_Click
    ///DESCRIPCIÓN: Agrega un nuevo Dato a la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Dato_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

            Div_Contenedor_Msj_Error.Visible = false;
            if (Validar_Datos_Tramites())
            {
                DataTable Tabla;
                if (Session["Dt_Datos_Tramites"] == null)
                {
                    Tabla = new DataTable("Datos_Tramites");
                    Tabla.Columns.Add("DATO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                    Tabla.Columns.Add("DATO_REQUERIDO", Type.GetType("System.String"));
                    Tabla.Columns.Add("TIPO_DATO", Type.GetType("System.String"));
                    Tabla.Columns.Add("ORDEN", Type.GetType("System.String"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Datos_Tramites"];
                }

                if (Chk_Dato_Inicial.Checked)
                {
                    DataRow Fila = Tabla.NewRow();
                    Fila["DATO_ID"] = HttpUtility.HtmlDecode("");
                    Fila["NOMBRE"] = HttpUtility.HtmlDecode(Txt_Nombre_Dato.Text.Trim());
                    Fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Dato.Text.Trim());
                    Fila["DATO_REQUERIDO"] = HttpUtility.HtmlDecode((Chk_Dato_Requerido.Checked) ? "S" : "N");
                    Fila["ORDEN"] = HttpUtility.HtmlDecode((Tabla.Rows.Count + 1).ToString());
                    Fila["TIPO_DATO"] = "INICIAL";
                    Tabla.Rows.Add(Fila);
                }
                if (Chk_Dato_Final.Checked)
                {
                    DataRow Fila = Tabla.NewRow();
                    Fila["DATO_ID"] = HttpUtility.HtmlDecode("");
                    Fila["NOMBRE"] = HttpUtility.HtmlDecode(Txt_Nombre_Dato.Text.Trim());
                    Fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Dato.Text.Trim());
                    Fila["DATO_REQUERIDO"] = HttpUtility.HtmlDecode((Chk_Dato_Requerido.Checked) ? "S" : "N");
                    Fila["ORDEN"] = HttpUtility.HtmlDecode((Tabla.Rows.Count + 1).ToString());
                    Fila["TIPO_DATO"] = "FINAL";
                    Tabla.Rows.Add(Fila);
                }

                Grid_Datos_Tramite.SelectedIndex = (-1);
                Llenar_Grid_Datos(Grid_Datos_Tramite.PageIndex, Tabla);
                Txt_Nombre_Dato.Text = "";
                Txt_Descripcion_Dato.Text = "";
                Chk_Dato_Requerido.Checked = false;
                Chk_Dato_Inicial.Checked = false;
                Chk_Dato_Final.Checked = false;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        Cls_Ope_Ven_Lista_Tramites_Negocio Negocio_Cargar_Grid = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        DataTable Dt_Tramite = new DataTable();
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;

            if (Session["BUSQUEDA_TRAMITES"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_TRAMITES"].ToString());

                if (Estado != false)
                {
                    Negocio_Cargar_Grid.P_Tramite_ID = Session["TRAMITE_ID"].ToString();
                    Dt_Tramite = Negocio_Cargar_Grid.Consultar_Tramites();

                    if (Dt_Tramite is DataTable)
                    {
                        if (Dt_Tramite.Rows.Count > 0)
                        {
                            Grid_Tramites_Generales.Columns[1].Visible = true;
                            Grid_Tramites_Generales.DataSource = Dt_Tramite;
                            Grid_Tramites_Generales.DataBind();
                            Grid_Tramites_Generales.Columns[1].Visible = false;

                            Grid_Tramites_Generales.SelectedIndex = 0;
                            Grid_Tramites_Generales_SelectedIndexChanged(sender, null);
                        }
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Cuenta_Contable_Click
    ///DESCRIPCIÓN: habilita el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  14/junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Cuenta_Contable_Click(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;

            if (Session["BUSQUEDA_CUENTA"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_CUENTA"].ToString());

                if (Estado != false)
                {
                    string Cuenta_ID = Session["CUENTA_CLAVE"].ToString();

                    if (Cmb_Cuenta.Items.FindByValue(Cuenta_ID) != null)
                    {
                        Cmb_Cuenta.SelectedValue = Cuenta_ID;
                        Hdf_Cuenta_Contable_ID.Value = Session["CUENTA_CONTABLE_ID"].ToString();
                    }

                    Txt_Costo.Text = Session["COSTO_CUENTA"].ToString();

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Plantillas_Click
    ///DESCRIPCIÓN: cargara las plantillas
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  19/junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Plantillas_Click(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;

            if (Session["BUSQUEDA_PLANTILLAS"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_PLANTILLAS"].ToString());

                if (Estado != false)
                {
                    string Plantilla_ID = Session["PLANTILLA_ID"].ToString();

                    if (Cmb_Platillas.Items.FindByValue(Plantilla_ID) != null)
                    {
                        Cmb_Platillas.SelectedValue = Plantilla_ID;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Formatos_Click
    ///DESCRIPCIÓN: cargara los formatos
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  19/junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Busqueda_Avanzada_Formatos_Click(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;

            if (Session["BUSQUEDA_FORMATOS"] != null)
            {
                Boolean Estado = Convert.ToBoolean(Session["BUSQUEDA_FORMATOS"].ToString());

                if (Estado != false)
                {
                    string Formato_ID = Session["FORMATO_ID"].ToString();

                    if (Cmb_Formato.Items.FindByValue(Formato_ID) != null)
                    {
                        Cmb_Formato.SelectedValue = Formato_ID;
                    }

                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Limpiar_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN: limpia los controles de la busqueda avanzada
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Limpiar_Busqueda_Avanzada_Click(object sender, EventArgs e)
    {
        try
        {
            //Txt_Nombre_Tramite_Filtro.Text = "";
            //Txt_Clave_Tramite_Filtro.Text = "";
            //Cmb_Unidad_Responsable_Filtro.SelectedIndex = 0;
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Busqueda_Avanzada_Click
    ///DESCRIPCIÓN: oculta el div con los filtros para realizar la busqueda de los tramites
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Cerrar_Busqueda_Avanzada_Click(object sender, ImageClickEventArgs e)
    {
        try
        {

        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Tramite_Filtro_Click
    ///DESCRIPCIÓN: Buscara el tramite para luego cargarlo en el grid
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  02/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Buscar_Tramite_Filtro_Click(object sender, ImageClickEventArgs e)
    {
        Cls_Ope_Ven_Lista_Tramites_Negocio Rs_Consulta_Tramites = new Cls_Ope_Ven_Lista_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        Boolean Estado = false;
        try
        {
            //if (Txt_Nombre_Tramite_Filtro.Text != "")
            //{
            //    Rs_Consulta_Tramites.P_Nombre_Tramite = Txt_Nombre_Tramite_Filtro.Text;
            //    Estado = true;
            //}
            //if (Txt_Clave_Tramite_Filtro.Text != "")
            //{
            //    Rs_Consulta_Tramites.P_Clave_Tramite = Txt_Clave_Tramite_Filtro.Text;
            //    Estado = true;
            //}

            //if (Cmb_Unidad_Responsable_Filtro.SelectedIndex > 0)
            //{
            //    Rs_Consulta_Tramites.P_Dependencia_Tramite = Cmb_Unidad_Responsable_Filtro.SelectedValue;
            //    Estado = true;
            //}

            if (Estado == true)
            {
                Dt_Consulta = Rs_Consulta_Tramites.Consultar_Tramites();

                if (Dt_Consulta is DataTable)
                {
                    if (Dt_Consulta.Rows.Count > 0)
                    {
                        Grid_Tramites_Generales.Columns[1].Visible = true;
                        Grid_Tramites_Generales.DataSource = Dt_Consulta;
                        Grid_Tramites_Generales.DataBind();
                        Grid_Tramites_Generales.Columns[1].Visible = false;
                    }
                    else
                    {
                        Grid_Tramites_Generales.Columns[1].Visible = true;
                        Grid_Tramites_Generales.DataSource = new DataTable();
                        Grid_Tramites_Generales.DataBind();
                        Grid_Tramites_Generales.Columns[1].Visible = false;
                    }
                }
            }
            else
            {
                Grid_Tramites_Generales.Columns[1].Visible = true;
                Grid_Tramites_Generales.DataSource = new DataTable();
                Grid_Tramites_Generales.DataBind();
                Grid_Tramites_Generales.Columns[1].Visible = false;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Error al selecionar una dependencia. Error: [" + ex.Message + "]");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Dato_Click
    ///DESCRIPCIÓN: Modifica un Dato de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Dato_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Btn_Modificar_Dato.AlternateText.Equals("Modificar Dato"))
            {
                if (Grid_Datos_Tramite.Rows.Count > 0 && Grid_Datos_Tramite.SelectedIndex > (-1))
                {
                    if (Session["Dt_Datos_Tramites"] != null)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Datos_Tramites"];
                        Int32 Registro = ((Grid_Datos_Tramite.PageIndex) * Grid_Datos_Tramite.PageSize) + (Grid_Datos_Tramite.SelectedIndex);
                        Hdf_Dato_Tramite_ID.Value = Tabla.Rows[Registro][0].ToString();
                        Txt_Dato_Tramite_ID.Text = Tabla.Rows[Registro][0].ToString();
                        Txt_Nombre_Dato.Text = Tabla.Rows[Registro][1].ToString();
                        Txt_Descripcion_Dato.Text = Tabla.Rows[Registro][2].ToString();
                        Chk_Dato_Requerido.Checked = (Tabla.Rows[Registro][3].ToString() == "S") ? true : false;
                        Chk_Dato_Inicial.Checked = (Tabla.Rows[Registro][4].ToString().Trim() == "INICIAL") ? true : false;
                        Chk_Dato_Final.Checked = (Tabla.Rows[Registro][4].ToString().Trim() == "FINAL") ? true : false;

                        Tabla.DefaultView.RowFilter = "NOMBRE = '" + Tabla.Rows[Registro][1].ToString() + "' AND DESCRIPCION = '" + Tabla.Rows[Registro][2].ToString() + "'";

                        foreach (DataRow Dr_Renglon in Tabla.DefaultView.ToTable().Rows)
                        {
                            if (Dr_Renglon[4].ToString().Trim() == "INICIAL")
                                Chk_Dato_Inicial.Checked = true;

                            if (Dr_Renglon[4].ToString().Trim() == "FINAL")
                                Chk_Dato_Final.Checked = true;
                        }

                        Tabla.DefaultView.RowFilter = "NOMBRE <> '" + Tabla.Rows[Registro][1].ToString() + "' AND DESCRIPCION <> '" + Tabla.Rows[Registro][2].ToString() + "'";

                        DataTable Dt_Datos_Grid = Tabla.DefaultView.ToTable();

                        Dt_Datos_Grid.DefaultView.AllowEdit = true;
                        for (int Cont_Renglon = 0; Cont_Renglon < Dt_Datos_Grid.Rows.Count; Cont_Renglon++)
                        {
                            Dt_Datos_Grid.Rows[Cont_Renglon].BeginEdit();
                            Dt_Datos_Grid.Rows[Cont_Renglon][5] = HttpUtility.HtmlDecode((Cont_Renglon + 1).ToString());
                            Dt_Datos_Grid.Rows[Cont_Renglon].EndEdit();
                        }

                        Session["Dt_Datos_Tramites"] = Dt_Datos_Grid;
                        Grid_Datos_Tramite.DataSource = Tabla;
                        Grid_Datos_Tramite.DataBind();
                        Grid_Datos_Tramite.SelectedIndex = Grid_Datos_Tramite.Rows.Count;

                        Btn_Modificar_Dato.AlternateText = "Actualizar Dato";
                        Btn_Quitar_Dato.Visible = false;
                        Btn_Agregar_Dato.Visible = false;
                        Grid_Datos_Tramite.Enabled = false;
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Selecciona el Dato que quieres Modificar.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Datos_Tramites())
                {
                    Int32 Registro = ((Grid_Datos_Tramite.PageIndex) * Grid_Datos_Tramite.PageSize) + (Grid_Datos_Tramite.SelectedIndex);
                    if (Session["Dt_Datos_Tramites"] != null)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Datos_Tramites"];
                        //Tabla.DefaultView.AllowEdit = true;
                        //Tabla.Rows[Registro].BeginEdit();
                        //Tabla.Rows[Registro][1] = HttpUtility.HtmlDecode(Txt_Nombre_Dato.Text.Trim());
                        //Tabla.Rows[Registro][2] = HttpUtility.HtmlDecode(Txt_Descripcion_Dato.Text.Trim());
                        //Tabla.Rows[Registro][3] = HttpUtility.HtmlDecode((Chk_Dato_Requerido.Checked) ? "S" : "N");
                        //Tabla.Rows[Registro].EndEdit();

                        if (Chk_Dato_Inicial.Checked)
                        {
                            DataRow Fila = Tabla.NewRow();
                            Fila["DATO_ID"] = HttpUtility.HtmlDecode("");
                            Fila["NOMBRE"] = HttpUtility.HtmlDecode(Txt_Nombre_Dato.Text.Trim());
                            Fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Dato.Text.Trim());
                            Fila["DATO_REQUERIDO"] = HttpUtility.HtmlDecode((Chk_Dato_Requerido.Checked) ? "S" : "N");
                            Fila["ORDEN"] = HttpUtility.HtmlDecode((Tabla.Rows.Count + 1).ToString());
                            Fila["TIPO_DATO"] = "INICIAL";
                            Tabla.Rows.Add(Fila);
                        }
                        if (Chk_Dato_Final.Checked)
                        {
                            DataRow Fila = Tabla.NewRow();
                            Fila["DATO_ID"] = HttpUtility.HtmlDecode("");
                            Fila["NOMBRE"] = HttpUtility.HtmlDecode(Txt_Nombre_Dato.Text.Trim());
                            Fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Dato.Text.Trim());
                            Fila["DATO_REQUERIDO"] = HttpUtility.HtmlDecode((Chk_Dato_Requerido.Checked) ? "S" : "N");
                            Fila["ORDEN"] = HttpUtility.HtmlDecode((Tabla.Rows.Count + 1).ToString());
                            Fila["TIPO_DATO"] = "FINAL";
                            Tabla.Rows.Add(Fila);
                        }

                        Session["Dt_Datos_Tramites"] = Tabla;
                        Llenar_Grid_Datos(Grid_Datos_Tramite.PageIndex, Tabla);
                        Grid_Datos_Tramite.SelectedIndex = (-1);
                        Btn_Modificar_Dato.AlternateText = "Modificar Dato";
                        Btn_Quitar_Dato.Visible = true;
                        Btn_Agregar_Dato.Visible = true;
                        Tab_Contenedor_Pestagnas.TabIndex = 0;
                        Grid_Datos_Tramite.Enabled = true;
                        Hdf_Dato_Tramite_ID.Value = "";
                        Txt_Dato_Tramite_ID.Text = "";
                        Txt_Nombre_Dato.Text = "";
                        Chk_Dato_Requerido.Checked = false;
                        Chk_Dato_Inicial.Checked = false;
                        Chk_Dato_Final.Checked = false;
                        Txt_Descripcion_Dato.Text = "";
                    }
                }
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Dato_Click
    ///DESCRIPCIÓN: Quita un Dato de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Dato_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Datos_Tramite.Rows.Count > 0 && Grid_Datos_Tramite.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Datos_Tramite.PageIndex) * Grid_Datos_Tramite.PageSize) + (Grid_Datos_Tramite.SelectedIndex);
                if (Session["Dt_Datos_Tramites"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Datos_Tramites"];
                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Datos_Tramites"] = Tabla;
                    Actualizar_Orden_Datos();
                    Llenar_Grid_Datos(Tabla);
                    Grid_Datos_Tramite.SelectedIndex = (-1);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona el Dato que se desea Quitar.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Subprocesos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Subproceso_Click
    ///DESCRIPCIÓN: Agrega un nuevo Subproceso a la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Subproceso_Click(object sender, ImageClickEventArgs e)
    {
        Double Valor = 0.0;
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Validar_Subprocesos_Tramites())
            {
                DataTable Tabla;
                if (Session["Dt_Subprocesos_Tramites"] == null)
                {
                    Tabla = new DataTable("Dt_Subprocesos_Tramites");
                    Tabla.Columns.Add("SUBPROCESO_ID", Type.GetType("System.String"));
                    Tabla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Tabla.Columns.Add("DESCRIPCION", Type.GetType("System.String"));
                    Tabla.Columns.Add("VALOR", Type.GetType("System.String"));
                    Tabla.Columns.Add("ORDEN", Type.GetType("System.String"));
                    Tabla.Columns.Add("PLANTILLA", Type.GetType("System.String"));
                    //Tabla.Columns.Add("PERSONA_FIRMA_DOCUMENTO", Type.GetType("System.String"));
                    Tabla.Columns.Add("TIPO_ACTIVIDAD", Type.GetType("System.String"));
                    Tabla.Columns.Add("CONDICION_SI", Type.GetType("System.Decimal"));
                    Tabla.Columns.Add("CONDICION_NO", Type.GetType("System.Decimal"));
                }
                else
                {
                    Tabla = (DataTable)Session["Dt_Subprocesos_Tramites"];
                }

                DataRow Fila = Tabla.NewRow();
                Fila["SUBPROCESO_ID"] = HttpUtility.HtmlDecode("");
                Fila["NOMBRE"] = HttpUtility.HtmlDecode(Cmb_Nombre_Actividad.SelectedValue);
                Fila["DESCRIPCION"] = HttpUtility.HtmlDecode(Txt_Descripcion_Subproceso.Text.Trim());
                Fila["VALOR"] = HttpUtility.HtmlDecode(Txt_Valor_Subproceso.Text.Trim());
                Fila["ORDEN"] = HttpUtility.HtmlDecode((Tabla.Rows.Count + 1).ToString());
                Fila["PLANTILLA"] = HttpUtility.HtmlDecode((Cmb_Platillas.SelectedIndex > 0) ? Cmb_Platillas.SelectedItem.Value : "00000");
                // Fila["PERSONA_FIRMA_DOCUMENTO"] = HttpUtility.HtmlDecode(Txt_Persona_Avala_Documento.Text.Trim());
                Fila["TIPO_ACTIVIDAD"] = HttpUtility.HtmlDecode((Cmb_Tipo_Actividad.SelectedValue));

                if (Txt_Condicion_Si.Text != "")
                    Fila["CONDICION_SI"] = Convert.ToDecimal(HttpUtility.HtmlDecode((Txt_Condicion_Si.Text)));

                if (Txt_Condicion_No.Text != "")
                    Fila["CONDICION_NO"] = Convert.ToDecimal(HttpUtility.HtmlDecode((Txt_Condicion_No.Text)));


                Tabla.Rows.Add(Fila);
                Grid_Subprocesos_Tramite.SelectedIndex = (-1);
                Actualizar_Orden_Subprocesos();
                Llenar_Grid_Subprocesos(Grid_Subprocesos_Tramite.PageIndex, Tabla);

                //  se cargara la caja de texto con el avance
                if (!String.IsNullOrEmpty(Txt_Valor_Subproceso.Text))
                {
                    if (Txt_Valor_Subproceso_Acumulado.Text == "")
                        Txt_Valor_Subproceso_Acumulado.Text = "0";

                    Valor = Convert.ToDouble(Txt_Valor_Subproceso.Text) + Convert.ToDouble(Txt_Valor_Subproceso_Acumulado.Text);
                    Txt_Valor_Subproceso_Acumulado.Text = "" + Valor;
                }

                //  se limpian las cajas 
                //Txt_Nombre_Subproceso.Text = "";
                Cmb_Nombre_Actividad.SelectedIndex = 0;
                Txt_Descripcion_Subproceso.Text = "";
                Txt_Valor_Subproceso.Text = "";

                if (Cmb_Platillas.Items.Count > 0)
                    Cmb_Platillas.SelectedIndex = 0;

                if (Cmb_Formato.Items.Count > 0)
                    Cmb_Formato.SelectedIndex = 0;

                Cmb_Tipo_Actividad.SelectedIndex = 0;
                //Txt_Persona_Avala_Documento.Text = "";
                if (Hdf_Orden_Subporceso.Value == "")
                    Hdf_Orden_Subporceso.Value = "0";

                double Orden = Convert.ToDouble(Hdf_Orden_Subporceso.Value);
                Orden++;
                Hdf_Orden_Subporceso.Value = "" + Orden;

                Btn_Subir_Orden_Subproceso.Enabled = false;
                Btn_Bajar_Orden_Subproceso.Enabled = false;
                Btn_Quitar_Subproceso.Enabled = false;
                Btn_Modificar_Subproceso.Enabled = false;

                Cmb_Tipo_Tramite.SelectedIndex = 0;
                Cmb_Tipo_Actividad_SelectedIndexChanged(sender, null);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Plantilla_Click
    ///DESCRIPCIÓN: Agrega el detalle de la plantilla
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  17/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Plantilla_Click(object sender, ImageClickEventArgs e)
    {
        Double Orden = 0.0;
        DataTable Dt_Plantilla = new DataTable(); ;
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cmb_Platillas.SelectedIndex > 0)
            {

                if (Session["Dt_Detalle_Plantilla"] == null)
                {
                    Dt_Plantilla = new DataTable("Dt_Detalle_Plantilla");
                    Dt_Plantilla.Columns.Add("SUBPROCESO_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("TRAMITE_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("PLANTILLA_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("DETALLE_PLANTILLA_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("ORDEN", Type.GetType("System.String"));
                }
                else
                {
                    Dt_Plantilla = (DataTable)Session["Dt_Detalle_Plantilla"];
                }

                if (Hdf_Orden_Subporceso.Value != "")
                    Orden = Convert.ToDouble(Hdf_Orden_Subporceso.Value);
                else
                    Orden = 1;

                DataRow Fila = Dt_Plantilla.NewRow();
                Fila["SUBPROCESO_ID"] = HttpUtility.HtmlDecode("");
                Fila["TRAMITE_ID"] = Hdf_Tramite_ID.Value;
                Fila["PLANTILLA_ID"] = Cmb_Platillas.SelectedValue;
                Fila["NOMBRE"] = Cmb_Platillas.SelectedItem.Text;
                Fila["ORDEN"] = "" + Orden;


                Dt_Plantilla.Rows.Add(Fila);
                Grid_Detalle_Plantilla.SelectedIndex = (-1);
                Llenar_Grid_Plantillas(Dt_Plantilla);

                Cmb_Platillas.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///******************************************************************************* 
    ///NOMBRE DE LA FUNCIÓN: Btn_Buscar_Dependencia_Click
    ///DESCRIPCIÓN: Obtener de la variable de sesión el ID de la Dependencia seleccionada en la 
    ///             búsqueda avanzada
    ///PARAMETROS: 
    ///CREO: Roberto González Oseguera
    ///FECHA_CREO: 17/may/2012
    ///MODIFICO: 
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************    
    protected void Btn_Buscar_Dependencia_Click(object sender, ImageClickEventArgs e)
    {
        // validar que la variable de sesión existe
        if (Session["BUSQUEDA_DEPENDENCIAS"] != null)
        {
            // si el valor de la sesión es igual a true
            if (Convert.ToBoolean(Session["BUSQUEDA_DEPENDENCIAS"]) == true)
            {
                try
                {
                    string Dependencia_ID = Session["DEPENDENCIA_ID"].ToString().Replace("&nbsp;", "");
                    // si el combo colonias contiene la colonia con el ID, seleccionar
                    if (Cmb_Dependencias.Items.FindByValue(Dependencia_ID) != null)
                    {
                        Cmb_Dependencias.SelectedValue = Dependencia_ID;
                        Cmb_Dependencias_SelectedIndexChanged(sender, null);
                    }
                }
                catch (Exception ex)
                {
                    Mostrar_Mensaje_Error(true, ex.Message.ToString());
                    throw new Exception(ex.Message.ToString());
                }

                // limpiar variables de sesión
                Session.Remove("DEPENDENCIA_ID");
                Session.Remove("NOMBRE_DEPENDENCIA");
            }
            // limpiar variable de sesión
            Session.Remove("BUSQUEDA_DEPENDENCIAS");
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Agregar_Formato_Click
    ///DESCRIPCIÓN: Agrega el detalle de la formatos
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  17/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Agregar_Formato_Click(object sender, ImageClickEventArgs e)
    {
        Double Orden = 0.0;
        DataTable Dt_Plantilla = new DataTable(); ;
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Cmb_Formato.SelectedIndex > 0)
            {

                if (Session["Dt_Detalle_Formato"] == null)
                {
                    Dt_Plantilla = new DataTable("Dt_Detalle_Plantilla");
                    Dt_Plantilla.Columns.Add("SUBPROCESO_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("TRAMITE_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("PLANTILLA_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("DETALLE_FORMATO_ID", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("NOMBRE", Type.GetType("System.String"));
                    Dt_Plantilla.Columns.Add("ORDEN", Type.GetType("System.String"));
                }
                else
                {
                    Dt_Plantilla = (DataTable)Session["Dt_Detalle_Formato"];
                }

                Orden = Convert.ToDouble(Hdf_Orden_Subporceso.Value);

                DataRow Fila = Dt_Plantilla.NewRow();
                Fila["SUBPROCESO_ID"] = HttpUtility.HtmlDecode("");
                Fila["TRAMITE_ID"] = Hdf_Tramite_ID.Value;
                Fila["PLANTILLA_ID"] = Cmb_Formato.SelectedValue;
                Fila["NOMBRE"] = Cmb_Formato.SelectedItem.Text;
                Fila["ORDEN"] = "" + Orden;

                Dt_Plantilla.Rows.Add(Fila);
                Grid_Detalle_Formato.SelectedIndex = (-1);
                Llenar_Grid_Formato(Dt_Plantilla);

                Cmb_Formato.SelectedIndex = 0;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Plantilla_Click
    ///DESCRIPCIÓN: QUITA el detalle de la plantilla
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  17/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Plantilla_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Detalle_Plantilla.Rows.Count > 0 && Grid_Detalle_Plantilla.SelectedIndex > (-1))
            {
                Int32 Registro = Grid_Detalle_Plantilla.SelectedIndex;
                if (Session["Dt_Detalle_Plantilla"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Detalle_Plantilla"];

                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Detalle_Plantilla"] = Tabla;
                    Grid_Detalle_Plantilla.SelectedIndex = (-1);
                    Llenar_Grid_Plantillas(Tabla);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona la plantilla que se desea Quitar.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Formato_Click
    ///DESCRIPCIÓN: QUITA el detalle del formato
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  17/Mayo/2012 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Formato_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Detalle_Formato.Rows.Count > 0 && Grid_Detalle_Formato.SelectedIndex > (-1))
            {
                Int32 Registro = Grid_Detalle_Formato.SelectedIndex;
                if (Session["Dt_Detalle_Formato"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Detalle_Formato"];

                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Detalle_Formato"] = Tabla;
                    Grid_Detalle_Formato.SelectedIndex = (-1);
                    Llenar_Grid_Formato(Tabla);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona el formato que se desea Quitar.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Subproceso_Click
    ///DESCRIPCIÓN: Modifica un Subproceso de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Subproceso_Click(object sender, ImageClickEventArgs e)
    {
        Double Valor = 0.0;
        DataTable Dt_Detalles_Plantillas = new DataTable();
        DataTable Dt_Detalles_Formato = new DataTable();
        DataTable Dt_Tipo_Actividad = new DataTable();
        Cls_Cat_Tramites_Negocio Negocio_Subproceso = new Cls_Cat_Tramites_Negocio();
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;

            if (Btn_Modificar_Subproceso.AlternateText.Equals("Modificar Subproceso"))
            {
                if (Grid_Subprocesos_Tramite.Rows.Count > 0 && Grid_Subprocesos_Tramite.SelectedIndex > (-1))
                {
                    if (Session["Dt_Subprocesos_Tramites"] != null)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Subprocesos_Tramites"];
                        Int32 Registro = ((Grid_Subprocesos_Tramite.PageIndex) * Grid_Subprocesos_Tramite.PageSize) + (Grid_Subprocesos_Tramite.SelectedIndex);
                        Hdf_SubProceso_ID.Value = Tabla.Rows[Registro][0].ToString();
                        Txt_Subproceso_ID.Text = Tabla.Rows[Registro][0].ToString();
                        //Txt_Nombre_Subproceso.Text = Tabla.Rows[Registro][1].ToString();
                        Cmb_Nombre_Actividad.SelectedValue = Tabla.Rows[Registro][1].ToString();
                        Txt_Descripcion_Subproceso.Text = Tabla.Rows[Registro][2].ToString();

                        //  se consultan los detalles de las plantillas y formatos
                        Negocio_Subproceso.P_Sub_Proceso_ID = Txt_Subproceso_ID.Text;
                        Dt_Tipo_Actividad = Negocio_Subproceso.Consultar_Tipo_Actividad();
                        Cmb_Tipo_Actividad.SelectedValue = Dt_Tipo_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Tipo_Actividad].ToString();
                        Cmb_Tipo_Actividad_SelectedIndexChanged(sender, null);

                        Txt_Condicion_Si.Text = Dt_Tipo_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Condicion_Si].ToString();
                        Txt_Condicion_No.Text = Dt_Tipo_Actividad.Rows[0][Cat_Tra_Subprocesos.Campo_Condicion_No].ToString();
                        Negocio_Subproceso.P_Tipo_Actividad = Txt_Subproceso_ID.Text;
                        Dt_Detalles_Plantillas = Negocio_Subproceso.Consultar_Detalles_Plantilla();
                        Dt_Detalles_Formato = Negocio_Subproceso.Consultar_Detalles_Formato();

                        Llenar_Grid_Formato(Dt_Detalles_Formato);
                        Llenar_Grid_Plantillas(Dt_Detalles_Plantillas);

                        if (!String.IsNullOrEmpty(Tabla.Rows[Registro][3].ToString()))
                        {
                            Txt_Valor_Subproceso.Text = Tabla.Rows[Registro][3].ToString();
                            Valor = Convert.ToDouble(Tabla.Rows[Registro][3].ToString());
                            //  se reduce el acumulado del avance y se asigna el nuevo valor a la caja de texto
                            Valor = Convert.ToDouble(Txt_Valor_Subproceso_Acumulado.Text) - Valor;
                            Txt_Valor_Subproceso_Acumulado.Text = "" + Valor;
                        }

                        Cmb_Platillas.SelectedIndex = (!Tabla.Rows[Registro][5].ToString().Equals("00000")) ? Cmb_Platillas.Items.IndexOf(Cmb_Platillas.Items.FindByValue(Tabla.Rows[Registro][5].ToString())) : 0;
                        Btn_Modificar_Subproceso.AlternateText = "Actualizar Subproceso";
                        Btn_Quitar_Subproceso.Visible = false;
                        Btn_Agregar_Subproceso.Visible = false;
                        Btn_Subir_Orden_Subproceso.Visible = false;
                        Btn_Bajar_Orden_Subproceso.Visible = false;
                        Grid_Subprocesos_Tramite.Enabled = false;
                    }
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Selecciona el Subproceso que quieres Modificar.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Subprocesos_Tramites())
                {
                    Int32 Registro = ((Grid_Subprocesos_Tramite.PageIndex) * Grid_Subprocesos_Tramite.PageSize) + (Grid_Subprocesos_Tramite.SelectedIndex);
                    if (Session["Dt_Subprocesos_Tramites"] != null)
                    {
                        Negocio_Subproceso.P_Dt_Detalle_Formato = (DataTable)Session["Dt_Detalle_Formato"];
                        Negocio_Subproceso.P_Dt_Detalle_Plantilla = (DataTable)Session["Dt_Detalle_Plantilla"];
                        Negocio_Subproceso.P_Tramite_ID = Hdf_Tramite_ID.Value;
                        Negocio_Subproceso.P_Sub_Proceso_ID = Hdf_SubProceso_ID.Value;
                        Negocio_Subproceso.P_Tipo_Actividad = Hdf_SubProceso_ID.Value;
                        Negocio_Subproceso.Modificar_Detalles_Plantillas_Formato();


                        DataTable Tabla = (DataTable)Session["Dt_Subprocesos_Tramites"];
                        Tabla.DefaultView.AllowEdit = true;
                        Tabla.Rows[Registro].BeginEdit();
                        Tabla.Rows[Registro][1] = HttpUtility.HtmlDecode(Cmb_Nombre_Actividad.SelectedValue);
                        Tabla.Rows[Registro][2] = HttpUtility.HtmlDecode(Txt_Descripcion_Subproceso.Text.Trim());
                        Tabla.Rows[Registro][3] = HttpUtility.HtmlDecode(Txt_Valor_Subproceso.Text.Trim());
                        Tabla.Rows[Registro][5] = HttpUtility.HtmlDecode((Cmb_Platillas.SelectedIndex > 0) ? Cmb_Platillas.SelectedItem.Value : "00000");
                        Tabla.Rows[Registro][6] = HttpUtility.HtmlDecode((Cmb_Tipo_Actividad.SelectedValue));

                        if (Cmb_Tipo_Actividad.SelectedValue == "CONDICION")
                        {
                            Tabla.Rows[Registro][7] = HttpUtility.HtmlDecode((Txt_Condicion_Si.Text));
                            Tabla.Rows[Registro][8] = HttpUtility.HtmlDecode((Txt_Condicion_No.Text));
                        }
                        else
                        {
                            Tabla.Rows[Registro][7] = "0";
                            Tabla.Rows[Registro][8] = "0";
                        }

                        Tabla.Rows[Registro].EndEdit();
                        Session["Dt_Subprocesos_Tramites"] = Tabla;
                        Actualizar_Orden_Subprocesos();
                        Llenar_Grid_Subprocesos(Grid_Datos_Tramite.PageIndex, Tabla);
                        Grid_Subprocesos_Tramite.SelectedIndex = (-1);
                        Btn_Modificar_Subproceso.AlternateText = "Modificar Subproceso";
                        Btn_Quitar_Subproceso.Visible = true;
                        Btn_Agregar_Subproceso.Visible = true;
                        Btn_Subir_Orden_Subproceso.Visible = true;
                        Btn_Bajar_Orden_Subproceso.Visible = true;
                        Tab_Contenedor_Pestagnas.TabIndex = 0;
                        Grid_Subprocesos_Tramite.Enabled = true;
                        Llenar_Grid_Plantillas(new DataTable());
                        Llenar_Grid_Formato(new DataTable());
                        //  se incrementa el valor acumulado
                        Valor = Convert.ToDouble(Txt_Valor_Subproceso.Text.Trim()) + Convert.ToDouble(Txt_Valor_Subproceso_Acumulado.Text.Trim());
                        Txt_Valor_Subproceso_Acumulado.Text = "" + Valor;

                        Hdf_SubProceso_ID.Value = "";
                        Txt_Subproceso_ID.Text = "";
                        Txt_Condicion_No.Text = "";
                        Txt_Condicion_Si.Text = "";
                        //Txt_Nombre_Subproceso.Text = "";
                        Cmb_Nombre_Actividad.SelectedIndex = 0;
                        Txt_Descripcion_Subproceso.Text = "";
                        Txt_Valor_Subproceso.Text = "";
                        Cmb_Platillas.SelectedIndex = 0;
                        Cmb_Tipo_Actividad.SelectedIndex = 0;
                        Cmb_Tipo_Actividad_SelectedIndexChanged(sender, null);
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Quitar_Subproceso_Click
    ///DESCRIPCIÓN: Quita un Subproceso de la tabla para este tramite.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 01/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Quitar_Subproceso_Click(object sender, ImageClickEventArgs e)
    {
        Double Valor = 0.0;
        int Orden = 0;
        String SubProceso_ID = "";
        Cls_Cat_Tramites_Negocio Negocio_Subproceso = new Cls_Cat_Tramites_Negocio();
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Subprocesos_Tramite.Rows.Count > 0 && Grid_Subprocesos_Tramite.SelectedIndex > (-1))
            {
                Int32 Registro = ((Grid_Subprocesos_Tramite.PageIndex) * Grid_Subprocesos_Tramite.PageSize) + (Grid_Subprocesos_Tramite.SelectedIndex);
                if (Session["Dt_Subprocesos_Tramites"] != null)
                {
                    DataTable Tabla = (DataTable)Session["Dt_Subprocesos_Tramites"];

                    //  se quitan los detalles de las plantillas y formatos
                    SubProceso_ID = Tabla.Rows[Registro]["SUBPROCESO_ID"].ToString().Trim();
                    Negocio_Subproceso.P_Sub_Proceso_ID = SubProceso_ID;
                    Negocio_Subproceso.Eliminar_Detalles_Plantillas_Formato();


                    //  para reducir el acumulado del avance y se asigna el nuevo valor a la caja de texto
                    if (!String.IsNullOrEmpty(Tabla.Rows[Registro][3].ToString()))
                    {
                        Valor = Convert.ToDouble(Tabla.Rows[Registro][3].ToString());
                        //  se reduce el acumulado del avance y se asigna el nuevo valor a la caja de texto
                        Valor = Convert.ToDouble(Txt_Valor_Subproceso_Acumulado.Text) - Valor;
                        Txt_Valor_Subproceso_Acumulado.Text = "" + Valor;
                    }


                    Tabla.Rows.RemoveAt(Registro);
                    Session["Dt_Subprocesos_Tramites"] = Tabla;
                    Grid_Subprocesos_Tramite.SelectedIndex = (-1);
                    Actualizar_Orden_Subprocesos();
                    Llenar_Grid_Subprocesos(Grid_Subprocesos_Tramite.PageIndex, Tabla);
                    Orden = Convert.ToInt32(Hdf_Orden_Subporceso.Value);
                    Orden--;
                    Hdf_Orden_Subporceso.Value = "" + Orden;
                    Cmb_Tipo_Actividad.SelectedIndex = 0;
                    Cmb_Tipo_Actividad_SelectedIndexChanged(sender, null);
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona el Subproceso que se desea Quitar.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Subir_Orden_Subproceso_Click
    ///DESCRIPCIÓN: Sube el número de orden del Subproceso
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Subir_Orden_Subproceso_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Subprocesos_Tramite.SelectedIndex > (-1))
            {
                if (Session["Dt_Subprocesos_Tramites"] != null)
                {
                    Int32 Registro = ((Grid_Subprocesos_Tramite.PageIndex) * Grid_Subprocesos_Tramite.PageSize) + (Grid_Subprocesos_Tramite.SelectedIndex);
                    if (Registro > 0)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Subprocesos_Tramites"];
                        DataRow Fila_Temporal = Tabla.Rows[Registro];
                        DataRow Fila_Seleccionada = Tabla.NewRow();
                        Fila_Seleccionada.ItemArray = Fila_Temporal.ItemArray;
                        Tabla.Rows.RemoveAt(Registro);
                        Tabla.Rows.InsertAt(Fila_Seleccionada, Registro - 1);
                        Session["Dt_Subprocesos_Tramites"] = Tabla;
                        Actualizar_Orden_Subprocesos();
                        Llenar_Grid_Subprocesos(Grid_Subprocesos_Tramite.PageIndex, Tabla);
                        Grid_Subprocesos_Tramite.SelectedIndex = (-1);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "No puede Subir mas, es el primero en Orden.";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona el Subproceso que quieres Subir.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Subir_Dato_Click
    ///DESCRIPCIÓN: Sube el número de orden del Subproceso
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Subir_Dato_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Datos_Tramite.SelectedIndex > (-1))
            {
                if (Session["Dt_Datos_Tramites"] != null)
                {
                    Int32 Registro = ((Grid_Datos_Tramite.PageIndex) * Grid_Datos_Tramite.PageSize) + (Grid_Datos_Tramite.SelectedIndex);

                    if (Registro > 0)
                    {
                        DataTable Tabla = (DataTable)Session["Dt_Datos_Tramites"];
                        DataRow Fila_Temporal = Tabla.Rows[Registro];
                        DataRow Fila_Seleccionada = Tabla.NewRow();
                        Fila_Seleccionada.ItemArray = Fila_Temporal.ItemArray;
                        Tabla.Rows.RemoveAt(Registro);
                        Tabla.Rows.InsertAt(Fila_Seleccionada, Registro - 1);
                        Session["Dt_Datos_Tramites"] = Tabla;
                        Actualizar_Orden_Datos();
                        Llenar_Grid_Datos(Tabla);
                        Grid_Datos_Tramite.SelectedIndex = (Registro - 1);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "No puede Subir mas, es el primero en Orden.";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona el Subproceso que quieres Subir.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Bajar_Dato_Click
    ///DESCRIPCIÓN: Baja el número de orden del Subproceso
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Bajar_Dato_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Datos_Tramite.SelectedIndex > (-1))
            {
                if (Session["Dt_Datos_Tramites"] != null)
                {
                    Int32 Registro = ((Grid_Datos_Tramite.PageIndex) * Grid_Datos_Tramite.PageSize) + (Grid_Datos_Tramite.SelectedIndex);
                    DataTable Tabla = (DataTable)Session["Dt_Datos_Tramites"];
                    if (Registro < Tabla.Rows.Count)
                    {
                        DataRow Fila_Temporal = Tabla.Rows[Registro];
                        DataRow Fila_Seleccionada = Tabla.NewRow();
                        Fila_Seleccionada.ItemArray = Fila_Temporal.ItemArray;
                        Tabla.Rows.RemoveAt(Registro);
                        Tabla.Rows.InsertAt(Fila_Seleccionada, Registro + 1);
                        Session["Dt_Datos_Tramites"] = Tabla;
                        Actualizar_Orden_Datos();
                        Llenar_Grid_Datos(Tabla);
                        Grid_Datos_Tramite.SelectedIndex = (Registro - 1);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "No puede Bajar mas, es el ultimo en Orden.";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona el Subproceso que quieres Subir.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Bajar_Orden_Subproceso_Click
    ///DESCRIPCIÓN: Baja el número de orden del Subproceso
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 04/Octubre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Bajar_Orden_Subproceso_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Grid_Subprocesos_Tramite.SelectedIndex > (-1))
            {
                if (Session["Dt_Subprocesos_Tramites"] != null)
                {
                    Int32 Registro = ((Grid_Subprocesos_Tramite.PageIndex) * Grid_Subprocesos_Tramite.PageSize) + (Grid_Subprocesos_Tramite.SelectedIndex);
                    DataTable Tabla = (DataTable)Session["Dt_Subprocesos_Tramites"];
                    if (Registro < Tabla.Rows.Count)
                    {
                        DataRow Fila_Temporal = Tabla.Rows[Registro];
                        DataRow Fila_Seleccionada = Tabla.NewRow();
                        Fila_Seleccionada.ItemArray = Fila_Temporal.ItemArray;
                        Tabla.Rows.RemoveAt(Registro);
                        Tabla.Rows.InsertAt(Fila_Seleccionada, Registro + 1);
                        Session["Dt_Subprocesos_Tramites"] = Tabla;
                        Actualizar_Orden_Subprocesos();
                        Llenar_Grid_Subprocesos(Grid_Subprocesos_Tramite.PageIndex, Tabla);
                        Grid_Subprocesos_Tramite.SelectedIndex = (-1);
                    }
                    else
                    {
                        Lbl_Mensaje_Error.Text = "No puede Bajar mas, es el ultimo en Orden.";
                        Div_Contenedor_Msj_Error.Visible = true;
                    }
                }
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Selecciona el Subproceso que quieres Subir.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #region Combos

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Dependencias_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga las Cuentas que pertenescan a la cuenta seleccionada.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Dependencias_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Areas_Negocio Negocio = new Cls_Cat_Areas_Negocio();
        try
        {
            Mostrar_Mensaje_Error(false, "");

            Negocio.P_Dependencia_ID = Cmb_Dependencias.SelectedValue;
            DataTable Dt_Areas = Negocio.Consulta_Areas();
            if (Dt_Areas.Rows.Count > 0 && Dt_Areas != null)
            {
                Cmb_Areas.DataSource = Dt_Areas;
                Cmb_Areas.DataTextField = "NOMBRE";
                Cmb_Areas.DataValueField = "AREA_ID";
                Cmb_Areas.DataBind();
            }
            else
            {
                Cmb_Areas.Items.Clear();
            }
        }
        catch (Exception Ex)
        {
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Cuenta_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga la cuenta contable id de la cuenta de ingresos seleccionada
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  15/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Cuenta_SelectedIndexChanged(object sender, EventArgs e)
    {
        Cls_Cat_Tramites_Negocio Negocio_Consulta_Usuario = new Cls_Cat_Tramites_Negocio();
        DataTable Dt_Consulta = new DataTable();
        try
        {
            Mostrar_Mensaje_Error(false, "");
            if (Cmb_Cuenta.SelectedIndex > 0)
            {
                Negocio_Consulta_Usuario.P_Cuenta_Contable_Clave = Cmb_Cuenta.SelectedValue;
                Dt_Consulta = Negocio_Consulta_Usuario.Consultar_Cuenta_Contable();

                if (Dt_Consulta != null && Dt_Consulta.Rows.Count > 0)
                {
                    Hdf_Cuenta_Contable_ID.Value = Dt_Consulta.Rows[0][Cat_Con_Cuentas_Contables.Campo_Cuenta_Contable_ID].ToString();
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }
    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Cmb_Tipo_Actividad_SelectedIndexChanged
    ///DESCRIPCIÓN: Carga las condiciones del flujo de tramite
    ///PARAMETROS:     
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  26/Junio/2012
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Cmb_Tipo_Actividad_SelectedIndexChanged(object sender, EventArgs e)
    {
        try
        {
            Mostrar_Mensaje_Error(false, "");

            if (Cmb_Tipo_Actividad.SelectedValue == "CONDICION")
            {
                Txt_Condicion_Si.Enabled = true;
                Txt_Condicion_Si.Text = "";
                Txt_Condicion_No.Enabled = true;
                Txt_Condicion_No.Text = "";
            }
            else
            {
                Txt_Condicion_Si.Enabled = false;
                Txt_Condicion_Si.Text = "";
                Txt_Condicion_No.Enabled = false;
                Txt_Condicion_No.Text = "";
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }


    #endregion

    #region Botones

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Nuevo_Click
    ///DESCRIPCIÓN: Deja los componentes listos para dar de Alta una nuevo Tramite
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Nuevo_Click(object sender, EventArgs e)
    {
        Cls_Cat_Tramites_Negocio Tramite = new Cls_Cat_Tramites_Negocio();

        try
        {
            if (Btn_Nuevo.AlternateText.Equals("Nuevo"))
            {
                Configuracion_Formulario(false);
                Limpiar_Catalogo();
                Btn_Nuevo.AlternateText = "Dar de Alta";
                Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_guardar.png";
                Btn_Salir.AlternateText = "Cancelar";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                Btn_Modificar.Visible = false;
                Txt_ID_Tramite.Text = "";
                Btn_Subir_Orden_Subproceso.Enabled = false;
                Btn_Bajar_Orden_Subproceso.Enabled = false;
                Btn_Quitar_Subproceso.Enabled = false;
                Hdf_Orden_Subporceso.Value = "1";
                Btn_Ver_Requisitos.Enabled = true;
            }
            else
            {
                if (Validar_Generales_Tramites())
                {
                    if (Validar_Clave_Tramite(""))
                    {
                        if (Session["Dt_Subprocesos_Tramites"] != null)
                        {
                            //  validara que existan actividades para el tramite
                            if (Txt_Valor_Subproceso_Acumulado.Text != "")
                            {
                                //  se revisara que las actividades sea el 100%
                                if (Convert.ToDouble(Txt_Valor_Subproceso_Acumulado.Text) == 100)
                                {
                                    Tramite.P_Clave_Tramite = Txt_Clave_Tramite.Text.Trim().ToUpper();
                                    Tramite.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value;
                                    Tramite.P_Area_Dependencia = Cmb_Areas.SelectedItem.ToString();
                                    Tramite.P_Nombre = Txt_Nombre.Text.Trim();
                                    Tramite.P_Tipo = Cmb_Tipo_Tramite.SelectedItem.Value;
                                    Tramite.P_Tiempo_Estimado = (Txt_Tiempo_Estimado.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Tiempo_Estimado.Text.Trim()) : 0;
                                    if (Txt_Costo.Text.Trim().Length > 0)
                                    {
                                        Tramite.P_Costo = Convert.ToDouble(Txt_Costo.Text);
                                        if (Tramite.P_Costo > 0)
                                        {
                                            Tramite.P_Cuenta_ID = Cmb_Cuenta.SelectedValue;
                                            Tramite.P_Cuenta_Contable_Clave = Hdf_Cuenta_Contable_ID.Value;
                                        }
                                    }
                                    //Tramite.P_Solicitar_Intenet = (Chck_Solicitar_Internet.Checked) ? "S" : "N";
                                    Tramite.P_Descripcion = Txt_Descripcion.Text.Trim();
                                    //  se cargan las sesiones
                                    Tramite.P_Perfiles_Autorizar = (DataTable)Session["Dt_Autorizadores_Tramites"];
                                    Tramite.P_Datos_Tramite = (DataTable)Session["Dt_Datos_Tramites"];
                                    Tramite.P_Documentacion_Tramite = (DataTable)Session["Dt_Documentos_Tramites"];
                                    Tramite.P_SubProcesos_Tramite = (DataTable)Session["Dt_Subprocesos_Tramites"];
                                    Tramite.P_Dt_Detalle_Formato = (DataTable)Session["Dt_Detalle_Formato"];
                                    Tramite.P_Dt_Detalle_Plantilla = (DataTable)Session["Dt_Detalle_Plantilla"];
                                    Tramite.P_Matriz_Costo = (DataTable)Session["Dt_Matriz_Costo"];

                                    Tramite.P_Usuario = Cls_Sessiones.Nombre_Empleado;
                                    Tramite.P_Tipo_Actividad = Cmb_Tipo_Actividad.SelectedValue;
                                    Tramite.P_Estatus_Tramite = Cmb_Estatus_Tramite.SelectedValue;

                                    /*  para los parametros y operadores de ordenamiento */
                                    Tramite.P_Parametro1 = Txt_Parametro1.Text.Trim();
                                    Tramite.P_Parametro2 = Txt_Parametro2.Text.Trim();
                                    Tramite.P_Parametro3 = Txt_Parametro3.Text.Trim();
                                    Tramite.P_Operador1 = Txt_Operador1.Text.Trim();
                                    Tramite.P_Operador2 = Txt_Operador2.Text.Trim();
                                    Tramite.P_Operador3 = Txt_Operador3.Text.Trim();

                                    AsyncFileUpload AsFileUp = (AsyncFileUpload)Div_tipo_Tramite.FindControl("FileUp");
                                    String Extension = Obtener_Extension(AsFileUp.FileName);
                                    if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg")
                                    {
                                        //HttpPostedFile HttpFile = AsFileUp.PostedFile; 
                                        String Directorio_Expediente = "Formato_Tramite";
                                        String Raiz = @Server.MapPath("../../Archivos");
                                        String Direccion_Archivo = "";
                                        //verifica si existe el directorio donde se guardan los archivos
                                        // si no existe lo crea
                                        if (!Directory.Exists(Raiz))
                                        {
                                            Directory.CreateDirectory(Raiz);
                                        }//FIN IF EXISTE DIRECTORIO raiz
                                        String URL = AsFileUp.FileName;
                                        //verifica que ya exista una url osea un archivo seleccionado para ser subido
                                        if (URL != "")
                                        {
                                            //verifica si existe un directorio llamado con ese Nombre_Commando de expediente
                                            if (!Directory.Exists(Raiz + Directorio_Expediente))
                                            {
                                                Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);
                                            }//fin if si existe directorio expediente


                                            String Clave = "";
                                            Clave = Txt_Clave_Tramite.Text.Replace("/", "");

                                            //se crea el Nombre_Commando del archivo que se va a guardar
                                            Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                                "/" + Server.HtmlEncode(Clave + ".");

                                            //se valida que contega un archivo 
                                            if (AsFileUp.HasFile)
                                            {
                                                if (File.Exists(Direccion_Archivo + "pdf"))
                                                    File.Delete(Direccion_Archivo + "pdf");
                                                if (File.Exists(Direccion_Archivo + "jpg"))
                                                    File.Delete(Direccion_Archivo + "jpg");
                                                if (File.Exists(Direccion_Archivo + "jpeg"))
                                                    File.Delete(Direccion_Archivo + "jpeg");
                                                //se guarda el archivo
                                                Direccion_Archivo += Extension;
                                                AsFileUp.SaveAs(Direccion_Archivo);
                                            }//fin if hasFile
                                        }//fin if url
                                    }

                                    /*********************************** Alta Tramite **********************************/
                                    Tramite.Alta_Tramite();
                                    /***********************************************************************************/

                                    Configuracion_Formulario(true);
                                    Limpiar_Catalogo();
                                    Llenar_Grid_Tramites(Grid_Tramites_Generales.PageIndex);
                                    ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramites", "alert('Alta de Tramite Exitosa');", true);
                                    Btn_Nuevo.AlternateText = "Nuevo";
                                    Btn_Nuevo.ImageUrl = "~/paginas/imagenes/paginas/icono_nuevo.png";
                                    Btn_Salir.AlternateText = "Salir";
                                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                                    Configuracion_Subcatalogos_Estado_Original();
                                    Div_Contenedor_Msj_Error.Visible = false;
                                    //Btn_Subir_Orden_Subproceso.Enabled = true;
                                    //Btn_Bajar_Orden_Subproceso.Enabled = true;
                                    //Btn_Quitar_Subproceso.Enabled = true;
                                }
                                else
                                {
                                    Lbl_Mensaje_Error.Text = "Verificar.";
                                    Lbl_Mensaje_Error.Text += "Las actividades no cumplen con el 100%";
                                    Div_Contenedor_Msj_Error.Visible = true;
                                }
                            }
                        }
                        else
                        {
                            Lbl_Mensaje_Error.Text = "Es necesario.";
                            Lbl_Mensaje_Error.Text += "Ingrese las actividades del tramite";
                            Div_Contenedor_Msj_Error.Visible = true;
                        }


                    }// fin de la validacion de la clave

                }// fin de la validacion de tramites generales

            }// fin del else

        }// fin del try
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Modificar_Click
    ///DESCRIPCIÓN: Deja los componentes listos para hacer la modificacion de un Tramite
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Modificar_Click(object sender, EventArgs e)
    {
        try
        {
            Div_Contenedor_Msj_Error.Visible = false;
            if (Btn_Modificar.AlternateText.Equals("Modificar"))
            {
                if (Grid_Tramites_Generales.Rows.Count > 0 && Grid_Tramites_Generales.SelectedIndex > (-1))
                {
                    Configuracion_Formulario(false);
                    Btn_Modificar.AlternateText = "Actualizar";
                    Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_actualizar.png";
                    Btn_Salir.AlternateText = "Cancelar";
                    Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_cancelar.png";
                    Btn_Nuevo.Visible = false;
                    Btn_Ver_Requisitos.Enabled = true;
                }
                else
                {
                    Lbl_Mensaje_Error.Text = "Debe seleccionar el Registro que se desea Modificar.";
                    Div_Contenedor_Msj_Error.Visible = true;
                }
            }
            else
            {
                if (Validar_Generales_Tramites())
                {
                    if (Validar_Clave_Tramite(Hdf_Clave_Tramite.Value))
                    {
                        Cls_Cat_Tramites_Negocio Tramite = new Cls_Cat_Tramites_Negocio();
                        Tramite.P_Tramite_ID = Hdf_Tramite_ID.Value;
                        Tramite.P_Clave_Tramite = Txt_Clave_Tramite.Text.Trim().ToUpper();
                        Tramite.P_Dependencia_ID = Cmb_Dependencias.SelectedItem.Value;
                        Tramite.P_Estatus_Tramite = Cmb_Estatus_Tramite.SelectedValue;

                        if (Cmb_Areas.Items.Count > 0)
                            if (Cmb_Areas.SelectedItem.ToString() != "")
                                Tramite.P_Area_Dependencia = Cmb_Areas.SelectedItem.ToString();
                            else
                                Tramite.P_Area_Dependencia = "null";

                        Tramite.P_Nombre = Txt_Nombre.Text.Trim();
                        Tramite.P_Tipo = Cmb_Tipo_Tramite.SelectedItem.Value;
                        Tramite.P_Tiempo_Estimado = (Txt_Tiempo_Estimado.Text.Trim().Length > 0) ? Convert.ToInt32(Txt_Tiempo_Estimado.Text.Trim()) : 0;


                        /*  para los parametros y operadores de ordenamiento */
                        Tramite.P_Parametro1 = Txt_Parametro1.Text.Trim();
                        Tramite.P_Parametro2 = Txt_Parametro2.Text.Trim();
                        Tramite.P_Parametro3 = Txt_Parametro3.Text.Trim();
                        Tramite.P_Operador1 = Txt_Operador1.Text.Trim();
                        Tramite.P_Operador2 = Txt_Operador2.Text.Trim();
                        Tramite.P_Operador3 = Txt_Operador3.Text.Trim();

                        if (Txt_Costo.Text.Trim().Length > 0)
                        {
                            Tramite.P_Costo = Convert.ToDouble(Txt_Costo.Text);
                            if (Cmb_Cuenta.SelectedIndex > 0)
                            {
                                Tramite.P_Cuenta_ID = Cmb_Cuenta.SelectedValue;
                                Tramite.P_Cuenta_Contable_Clave = Hdf_Cuenta_Contable_ID.Value;
                            }
                        }

                        //Tramite.P_Solicitar_Intenet = (Chck_Solicitar_Internet.Checked) ? "S" : "N";
                        Tramite.P_Descripcion = Txt_Descripcion.Text.Trim();
                        Tramite.P_Perfiles_Autorizar = (DataTable)Session["Dt_Autorizadores_Tramites"];
                        Tramite.P_Datos_Tramite = (DataTable)Session["Dt_Datos_Tramites"];
                        Tramite.P_Documentacion_Tramite = (DataTable)Session["Dt_Documentos_Tramites"];
                        Tramite.P_SubProcesos_Tramite = (DataTable)Session["Dt_Subprocesos_Tramites"];
                        Tramite.P_Dt_Detalle_Formato = (DataTable)Session["Dt_Detalle_Formato"];
                        Tramite.P_Dt_Detalle_Plantilla = (DataTable)Session["Dt_Detalle_Plantilla"];
                        Tramite.P_Matriz_Costo = (DataTable)Session["Dt_Matriz_Costo"];

                        Tramite.P_Usuario = Cls_Sessiones.Nombre_Empleado;


                        AsyncFileUpload AsFileUp = (AsyncFileUpload)Div_tipo_Tramite.FindControl("FileUp");
                        String Extension = Obtener_Extension(AsFileUp.FileName);
                        if (Extension == "pdf" || Extension == "jpg" || Extension == "jpeg")
                        {
                            //HttpPostedFile HttpFile = AsFileUp.PostedFile; 
                            String Directorio_Expediente = "Formato_Tramite";
                            String Raiz = @Server.MapPath("../../Archivos");
                            String Direccion_Archivo = "";
                            //verifica si existe el directorio donde se guardan los archivos
                            // si no existe lo crea
                            if (!Directory.Exists(Raiz))
                            {
                                Directory.CreateDirectory(Raiz);
                            }//FIN IF EXISTE DIRECTORIO raiz
                            String URL = AsFileUp.FileName;
                            //verifica que ya exista una url osea un archivo seleccionado para ser subido
                            if (URL != "")
                            {
                                //verifica si existe un directorio llamado con ese Nombre_Commando de expediente
                                if (!Directory.Exists(Raiz + Directorio_Expediente))
                                {
                                    Directory.CreateDirectory(Raiz + "/" + Directorio_Expediente);
                                }//fin if si existe directorio expediente


                                String Clave = "";
                                Clave = Txt_Clave_Tramite.Text.Replace("/", "");
                                //se crea el Nombre_Commando del archivo que se va a guardar
                                Direccion_Archivo = Raiz + "/" + Directorio_Expediente +
                                    "/" + Server.HtmlEncode(Clave + ".");

                                //se valida que contega un archivo 
                                if (AsFileUp.HasFile)
                                {
                                    if (File.Exists(Direccion_Archivo + "pdf"))
                                        File.Delete(Direccion_Archivo + "pdf");
                                    if (File.Exists(Direccion_Archivo + "jpg"))
                                        File.Delete(Direccion_Archivo + "jpg");
                                    if (File.Exists(Direccion_Archivo + "jpeg"))
                                        File.Delete(Direccion_Archivo + "jpeg");
                                    //se guarda el archivo
                                    Direccion_Archivo += Extension;
                                    AsFileUp.SaveAs(Direccion_Archivo);
                                }//fin if hasFile
                            }//fin if url
                        }

                        Boolean Tramites_en_Proceso = false;
                        //if (Cmb_Estatus_Tramite.SelectedValue == "BAJA")
                        //{
                        //    Cls_Ope_Solicitud_Tramites_Negocio Tramites = new Cls_Ope_Solicitud_Tramites_Negocio();
                        //    Tramites.P_Tramite_ID = Hdf_Tramite_ID.Value;
                        //    DataTable Dt_Tramites = Tramites.Consultar_Tabla_Tramites();

                        //    foreach (DataRow Dr_Renglon in Dt_Tramites.Rows)
                        //    {
                        //        if (Dr_Renglon[Ope_Tra_Solicitud.Campo_Estatus].ToString() != "TERMINADO")
                        //        {
                        //            Tramites_en_Proceso = true;
                        //            break;
                        //        }
                        //    }
                        //}

                        if (!Tramites_en_Proceso)
                        {
                            Tramite.Modificar_Tramite();
                            ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramites", "alert('Actualización Tramite Exitosa');", true);
                        }

                        Configuracion_Formulario(true);
                        Limpiar_Catalogo();
                        Llenar_Grid_Tramites(Grid_Tramites_Generales.PageIndex);
                        Btn_Modificar.AlternateText = "Modificar";
                        Btn_Modificar.ImageUrl = "~/paginas/imagenes/paginas/icono_modificar.png";
                        Btn_Salir.AlternateText = "Salir";
                        Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                        Configuracion_Subcatalogos_Estado_Original();

                        if (Tramites_en_Proceso)
                        {
                            Mostrar_Mensaje_Error(true, "El Tramite no se puede modificar. Existen solicitudes en proceso.");
                        }
                    }
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Eliminar_Click
    ///DESCRIPCIÓN: Elimina un Tramite de la Base de Datos
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Eliminar_Click(object sender, EventArgs e)
    {
        try
        {
            if (Grid_Tramites_Generales.Rows.Count > 0 && Grid_Tramites_Generales.SelectedIndex > (-1))
            {
                Cls_Cat_Tramites_Negocio Tramite = new Cls_Cat_Tramites_Negocio();
                Tramite.P_Tramite_ID = Grid_Tramites_Generales.SelectedRow.Cells[1].Text;
                Tramite = Tramite.Consultar_Datos_Tramite();

                Tramite.Dar_Baja_Tramite(Tramite.P_Tramite_ID);

                //Tramite.Eliminar_Tramite();
                Grid_Tramites_Generales.SelectedIndex = (-1);
                Llenar_Grid_Tramites(Grid_Tramites_Generales.PageIndex);
                ScriptManager.RegisterStartupScript(this, this.GetType(), "Catalogo de Tramites", "alert('El Tramites fue dado de BAJA exitosamente');", true);
                Limpiar_Catalogo();
            }
            else
            {
                Lbl_Mensaje_Error.Text = "Debe seleccionar el Registro que se desea Eliminar.";
                Div_Contenedor_Msj_Error.Visible = true;
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Salir_Click
    ///DESCRIPCIÓN: Cancela la operación que esta en proceso (Alta o Actualizar) o 
    ///             sale del Formulario.
    ///PROPIEDADES:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Salir_Click(object sender, EventArgs e)
    {
        try
        {
            if (Btn_Salir.AlternateText.Equals("Salir"))
            {
                Session.Remove("Dt_Autorizadores_Tramites");
                Session.Remove("Dt_Datos_Tramites");
                Session.Remove("Dt_Documentos_Tramites");
                Session.Remove("Dt_Subprocesos_Tramites");
                Response.Redirect("../Paginas_Generales/Frm_Apl_Principal.aspx");
            }
            else
            {
                Limpiar_Catalogo();
                Configuracion_Formulario(true);
                Btn_Salir.AlternateText = "Salir";
                Btn_Salir.ImageUrl = "~/paginas/imagenes/paginas/icono_salir.png";
                Configuracion_Subcatalogos_Estado_Original();
                Response.Redirect("../Tramites/Frm_Cat_Tramites.aspx");
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            //Lbl_Ecabezado_Mensaje.Text = "Verificar.";
            //Lbl_Mensaje_Error.Text = ex.ToString();
            //Div_Contenedor_Msj_Error.Visible = true;
        }
    }

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Btn_Ver_Formato_Click
    ///DESCRIPCIÓN: mostrara el documento
    ///PARAMETROS: 
    ///CREO:        Hugo Enrique Ramírez Aguilera
    ///FECHA_CREO:  24/Mayo/2012
    ///MODIFICO:
    ///FECHA_MODIFICO:
    ///CAUSA_MODIFICACIÓN:
    ///*******************************************************************************
    protected void Btn_Ver_Formato_Click(object sender, ImageClickEventArgs e)
    {
        String URL = String.Empty;
        String Nombre_Archivo = "";
        String Nombre_Documento = "";
        try
        {
            //  se obtiene el nombre del documento y el id del ciudadano
            Nombre_Documento = Txt_Clave_Tramite.Text;

            //  se obtiene el nombre de los archivos existentes en la carpeta
            String[] Archivos = Directory.GetFiles(MapPath("../../Archivos/Formato_Tramite/"));

            //  se busca el archivo
            for (Int32 Contador = 0; Contador < Archivos.Length; Contador++)
            {
                Nombre_Archivo = Path.GetFileName(Archivos[Contador].Trim());

                if (Nombre_Archivo.Contains(Nombre_Documento))
                {
                    URL = Archivos[Contador].Trim();
                    Mostrar_Archivo(URL);
                    break;
                }
            }// fin del for
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(" " + ex.Message.ToString());
        }
    }

    #endregion

    #region Textbox

    ///*******************************************************************************
    ///NOMBRE DE LA FUNCIÓN: Txt_Busqueda_Tramite_Click
    ///DESCRIPCIÓN: Llena Grid de Tramites usando el filtro que se le proporcione.
    ///PARAMETROS:     
    ///CREO: Francisco Antonio Gallardo Castañeda.
    ///FECHA_CREO: 30/Septiembre/2010 
    ///MODIFICO:
    ///FECHA_MODIFICO
    ///CAUSA_MODIFICACIÓN
    ///*******************************************************************************
    protected void Btn_Busqueda_Tramite_Click(object sender, ImageClickEventArgs e)
    {
        try
        {
            //Limpiar_Catalogo();
            //Grid_Tramites_Generales.SelectedIndex = (-1);
            //Llenar_Grid_Tramites(0);
            //if (Grid_Tramites_Generales.Rows.Count == 0 && Txt_Busqueda_Tramite.Text.Trim().Length > 0)
            //{
            //    Lbl_Ecabezado_Mensaje.Text = "Para la Busqueda con el nombre \"" + Txt_Busqueda_Tramite.Text + "\" no se encotrarón coincidencias";
            //    Lbl_Mensaje_Error.Text = "(Se cargarón todos los tramites almacenados)";
            //    Div_Contenedor_Msj_Error.Visible = true;
            //    Txt_Busqueda_Tramite.Text = "";
            //    Llenar_Grid_Tramites(0);
            //}
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception(ex.Message.ToString());
        }
    }

    #endregion

    #endregion

    #region Reportes

    /// *************************************************************************************
    /// NOMBRE:         Generar_Reporte
    /// DESCRIPCIÓN:    Método que invoca la generación del reporte.
    /// PARÁMETROS:     Nombre_Plantilla_Reporte.- Nombre del archivo del Crystal Report.
    ///                 Nombre_Reporte_Generar.- Nombre que tendrá el reporte generado.
    /// CREO:           Hugo Enrique Ramírez Aguilera
    /// FECHA_CREO:     02/Mayo/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Generar_Reporte(ref DataSet Ds_Datos, String Nombre_Plantilla_Reporte, String Nombre_Reporte_Generar)
    {
        ReportDocument Reporte = new ReportDocument();//Variable de tipo reporte.
        String Ruta = String.Empty;//Variable que almacenara la ruta del archivo del crystal report. 

        try
        {
            Ruta = @Server.MapPath("../Rpt/Ventanilla/" + Nombre_Plantilla_Reporte);
            Reporte.Load(Ruta);

            if (Ds_Datos is DataSet)
            {
                if (Ds_Datos.Tables.Count > 0)
                {
                    Reporte.SetDataSource(Ds_Datos);
                    Exportar_Reporte_PDF(Reporte, Nombre_Reporte_Generar);
                    Mostrar_Reporte(Nombre_Reporte_Generar);
                }
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Error al generar el reporte. Error: [" + ex.Message + "]");
        }
    }

    /// *************************************************************************************
    /// NOMBRE:         Exportar_Reporte_PDF
    /// DESCRIPCIÓN:    Método que guarda el reporte generado en formato PDF en la ruta
    ///                 especificada.
    /// PARÁMETROS:     Reporte.- Objeto de tipo documento que contiene el reporte a guardar.
    ///                 Nombre_Reporte.- Nombre que se le dará al reporte.
    /// USUARIO CREO:   Hugo Enrique Ramírez Aguilera.
    /// FECHA CREO:     02/Mayo/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Exportar_Reporte_PDF(ReportDocument Reporte, String Nombre_Reporte)
    {
        ExportOptions Opciones_Exportacion = new ExportOptions();
        DiskFileDestinationOptions Direccion_Guardar_Disco = new DiskFileDestinationOptions();
        PdfRtfWordFormatOptions Opciones_Formato_PDF = new PdfRtfWordFormatOptions();

        try
        {
            if (Reporte is ReportDocument)
            {
                Direccion_Guardar_Disco.DiskFileName = @Server.MapPath("../../Reporte/" + Nombre_Reporte);
                Opciones_Exportacion.ExportDestinationOptions = Direccion_Guardar_Disco;
                Opciones_Exportacion.ExportDestinationType = ExportDestinationType.DiskFile;
                Opciones_Exportacion.ExportFormatType = ExportFormatType.PortableDocFormat;
                Reporte.Export(Opciones_Exportacion);
            }
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Error al exportar el reporte. Error: [" + ex.Message + "]");
        }
    }

    /// *************************************************************************************
    /// NOMBRE:         Mostrar_Reporte
    /// DESCRIPCIÓN:    Muestra el reporte en pantalla.
    /// PARÁMETROS:     Nombre_Reporte.- Nombre que tiene el reporte que se mostrara en pantalla.
    /// USUARIO CREO:   Hugo Enrique Ramírez Aguilera.
    /// FECHA CREO:     02/Mayo/2012
    /// USUARIO MODIFICO:
    /// FECHA MODIFICO:
    /// CAUSA MODIFICACIÓN:
    /// *************************************************************************************
    protected void Mostrar_Reporte(String Nombre_Reporte)
    {
        String Pagina = "../Paginas_Generales/Frm_Apl_Mostrar_Reportes.aspx?Reporte=";

        try
        {
            Pagina = Pagina + Nombre_Reporte;
            ScriptManager.RegisterStartupScript(this, this.GetType(), "Window_Rpt_Empleados",
                "window.open('" + Pagina + "', 'Busqueda_Empleados','toolbar=0,directories=0,menubar=0,status=0,scrollbars=0,resizable=1,width=1000,height=600')", true);
        }
        catch (Exception ex)
        {
            Mostrar_Mensaje_Error(true, ex.Message.ToString());
            throw new Exception("Error al mostrar el reporte. Error: [" + ex.Message + "]");
        }
    }

    #endregion
}